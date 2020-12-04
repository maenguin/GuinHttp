using GuinHttpWebW.Attributes.HttpContentType;
using GuinHttpWebW.Attributes.HttpHeader;
using GuinHttpWebW.Attributes.HttpMethod;
using GuinHttpWebW.Attributes.HttpParameter;
using GuinHttpWebW.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace GuinHttpWebW
{
    public class GuinHttpWebW
    {

        #region Property
        public string BaseUrl { get; private set; }
        public int Timeout { get; private set; }
        
        #endregion

        #region Constructor

        private GuinHttpWebW(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public static GuinHttpWebWBuilder Builder(string baseUrl)
        {
            return new GuinHttpWebWBuilder(baseUrl);
        }


        public class GuinHttpWebWBuilder
        {
            private GuinHttpWebW guinHttpWebW;

            public GuinHttpWebWBuilder(string baseUrl)
            {
                guinHttpWebW = new GuinHttpWebW(baseUrl);
            }

            public GuinHttpWebWBuilder SetTimeOut(int timeout)
            {
                guinHttpWebW.Timeout = timeout;
                return this;
            }

            public GuinHttpWebW Build()
            {
                return guinHttpWebW;
            }

        }

        #endregion

        #region Event
        public delegate void ExceptionCatchedHandler(object sender, Exception exception, string memo);
        public event ExceptionCatchedHandler ExceptionCatched;
        #endregion

        #region Method
        public string RequestApi(object anonymousObj, MethodBase caller)
        {
            string contentType = caller.GetAttributeValue((HttpContentTypeAttribute t) => t.Value);
            string apiPath = caller.GetAttributeValue((HttpMethodAttribute t) => t.ApiPath);
            string method = caller.GetAttributeValue((HttpMethodAttribute t) => t.Value);
            string[] headers = caller.GetAttributeValue((HttpHeadersAttribute t) => t.Value);
            byte[] postData = { };
            var query = string.Empty;

            //anonymous 타입의 클래스와 이전 메소드의 parameterInfo를 이용해 RequestParam타입을 구분합니다.
            var anonymousObjProperties = anonymousObj.GetType().GetProperties();
            var callerParameters = caller.GetParameters();


            //Paramer Type 별로 그룹핑 해줍니다. 
            var propertyGroup = from prop in anonymousObjProperties
                                group prop by callerParameters.Where(param => param.Name == prop.Name).FirstOrDefault().GetAttributeValue((HttpParameterAttribute at) => at.GetType()) into groupingResult
                                select new { Key = groupingResult.Key, Value = groupingResult };

            foreach (var group in propertyGroup)
            {
                switch (group.Key)
                {
                    case Type t when t == typeof(QueryAttribute):
                        query = GetQuery(anonymousObj, group.Value);
                        break;
                    case Type t when t == typeof(PathAttribute):
                        apiPath = GetPathVariable(apiPath, anonymousObj, group.Value);
                        break;
                    case Type t when t == typeof(BodyAttribute):
                        if (caller.GetCustomAttributes(typeof(HttpContentTypeAttribute), true).FirstOrDefault() is JsonBodyAttribute)
                            postData = GetJsonBody(anonymousObj, group.Value);
                        else if (caller.GetCustomAttributes(typeof(HttpContentTypeAttribute), true).FirstOrDefault() is FormUrlEncodedAttribute)
                            postData = GetFormUrlEncodedBody(anonymousObj, group.Value);
                        break;
                    default:
                        break;
                }
            }

            var request = WebRequest.Create($"{BaseUrl}{apiPath}{query}") as HttpWebRequest;
            request.Method = method;
            request.Timeout = Timeout;
            request.ContentType = contentType;
            request.Headers = GetHeaderCollection(headers);

            try
            {
                if (postData != null && postData.Length > 0)
                {
                    request.ContentLength = postData.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(postData, 0, postData.Length);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionCatched?.Invoke(this, e, $"{request?.RequestUri} body 생성 도중 오류");
                return e.Message;
            }

            return GetAPIResponse(request);
        }

        private string GetQuery(object anonymousObj, IEnumerable<PropertyInfo> properties)
        {
            return Util.JoinAdvanced("?", "&"
                    , properties
                    , property =>
                    {
                        if (property.GetValue(anonymousObj, null) is List<string>)
                        {
                            return Util.JoinAdvanced("&", property.GetValue(anonymousObj, null) as List<string>, s => $"{property.Name}={HttpUtility.UrlEncode(s)}");
                        }
                        return $"{property.Name}={HttpUtility.UrlEncode(property.GetValue(anonymousObj, null)?.ToString())}";
                    }
                );
        }

        private string GetPathVariable(string apiPath, object anonymousObj, IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                apiPath = apiPath.Replace($"{{{property.Name}}}", $"{HttpUtility.UrlEncode(property.GetValue(anonymousObj, null)?.ToString())}");
            }
            return apiPath;
        }

        private byte[] GetJsonBody(object anonymousObj, IEnumerable<PropertyInfo> properties)
        {
            byte[] body = { };
            foreach (var property in properties)
            {
                var propertyData = property.GetValue(anonymousObj, null);
                if (propertyData != null)
                {
                    //null들어갈시 string "null" 반환
                    var json = JsonConvert.SerializeObject(propertyData);
                    var data = Encoding.UTF8.GetBytes(json);
                    body = Util.CombineByteArray(body, data);
                }
            }
            return body;
        }

        private byte[] GetFormUrlEncodedBody(object anonymousObj, IEnumerable<PropertyInfo> properties)
        {
            byte[] body = { };
            
            foreach (var property in properties)
            {
                var bodyItem = property.GetValue(anonymousObj, null);

                var bodyItemProperties = bodyItem.GetType().GetProperties();
                var formUrlEncodedBodyItem = Util.JoinAdvanced("&"
                    , bodyItemProperties
                    , bodyItemProperty =>
                    {
                        if (bodyItemProperty.GetValue(bodyItem, null) is List<string>)
                        {
                            return Util.JoinAdvanced("&", bodyItemProperty.GetValue(bodyItem, null) as List<string>, s => $"{bodyItemProperty.Name}={HttpUtility.UrlEncode(s)}");
                        }
                        return $"{bodyItemProperty.Name}={HttpUtility.UrlEncode(bodyItemProperty.GetValue(bodyItem, null)?.ToString())}";
                    }
                );
                var data = Encoding.UTF8.GetBytes(formUrlEncodedBodyItem);
                body = Util.CombineByteArray(body, data);
            }

            return body;
        }

        private WebHeaderCollection GetHeaderCollection(string[] headers)
        {
            WebHeaderCollection headerCollection = new WebHeaderCollection();
            headers?.ToList().ForEach(s => 
            {
                string[] pair = s.Split(':');
                if (pair.Length == 2)
                    headerCollection.Add(pair[0].Trim(), pair[1].Trim());
            }) ;
            return headerCollection;
        }


        private string GetAPIResponse (HttpWebRequest request)
        {
            string result = string.Empty;
            HttpWebResponse response = null;
            Exception exception = null;
            bool exceptionCatched = false;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException we)
            {
                response = (HttpWebResponse)we.Response;
                result = we.Message;
                exceptionCatched = true;
                exception = we;

            }
            finally
            {
                if (response != null)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("UTF-8")))
                        {
                            var streamResult = streamReader.ReadToEnd();
                            result = string.IsNullOrEmpty(streamResult) ? result : streamResult;
                        }
                    }
                }

            }
            if (exceptionCatched)
            {
                ExceptionCatched?.Invoke(this, exception, $"{request?.RequestUri} {result}");
            }
            return result;
        }
        #endregion

       

    }
}
