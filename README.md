

# GuinHttp
![version](https://img.shields.io/badge/.NET%20Framework-%3E%3D3.5-blueviolet)  
.NET Framework 기본 라이브러리에 C#의 Attribute 기술을 활용하여 `사용감을 높인 Http 통신 라이브러리`

<br>
<br>
<br>

## 제작 계기

Windows Client Application을 개발할때 xp 이하의 버전을 지원하기 위해 .NET Framework 4.0 이하 버전을 사용하고 있습니다.  

.NET Framework 버전이 낮기 때문에 Http 통신 라이브러리도 .NET Framework 기본 라이브러리인 `Net.HttpWeb`을 사용해왔습니다.  

처음 사용할때는 불편하다고 생각하지 못했지만 Spring을 공부할때 `Annotation`을 사용하여 컨트롤러를 만들때 정말 편하다는 인상을 받았습니다. 

그래서 C#도 이와 비슷한 인터페이스가 있으면 좋겠다고 생각했습니다.  

하지만 비슷한 라이브러리를 찾을 수 없었기 때문에 직접 만들게 되었습니다.  

C#의 Attribute 기능과 Reflection을 활용했고 전체적인 네이밍은 Retrofit2에서 가져왔습니다.  
  
<br>
<br>
<br>
<br>



## 기존 라이브러리와의 비교  
네이버 오픈 API 주소 가져오기  
<br>
<br>
#### .NET Framework 기본 라이브러리를 사용한 경우 
`System.Net`의 `HttpRequest`와 `HttpResponse`  
```c#
public static string Geocode(string query, string coordinate, string filter, string page, string count)
{
    string url = "https://naveropenapi.apigw.ntruss.com/map-geocode/v2/geocode";
    string parameters = string.Format("?query={0}&coordinate={1}&filter={2}&page={3}&count={4}"
                                      , query, coordinate, filter, page, count);

    HttpWebRequest request = WebRequest.Create(url + parameters) as HttpWebRequest;
    request.Method = "GET";
    request.Timeout = 5000;
    request.Headers.Add("X-NCP-APIGW-API-KEY-ID", "Client ID");
    request.Headers.Add("X-NCP-APIGW-API-KEY", "Client Secret");

    HttpWebResponse response;
    string result = string.empty;
    try
    {
        response = request.GetResponse() as HttpWebResponse;
    }
    catch (WebException webException)
    {
        //...
    }
    finally
    {
         using (Stream stream = response.GetResponseStream())
         {
            using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("UTF-8")))
            {
                result = streamReader.ReadToEnd();
            }
         }
    }
    return result;
}
```
* 직접 처리해야하는 작업이 많습니다.
* 위 경우는 query만 처리하지만 POST로 Body에 데이터를 넣어야 하는 경우 직접 포맷을 변경하고 Stream을 열어서 Request에 Write 해야합니다.
* 다른 API 호출시 위와 같은 작업을 반복적으로 해야하기 때문에 중복되는 코드가 양산되기 쉽습니다.

<br>
<br>

#### `GuinHttp`를 사용한 경우
```c#
[Headers("X-NCP-APIGW-API-KEY-ID: ClientId", "X-NCP-APIGW-API-KEY: ClientSecret")]
[Get("/map-geocode/v2/geocode")]
public string Geocode([Query]string query,
                      [Query]string coordinate,
                      [Query]string filter,
                      [Query]string page,
                      [Query]string count)
{
    var params = new { query, coordinate, filter, page, count };
    string response = guinHttp.RequestApi(params, MethodBase.GetCurrentMethod());
    return response;
}

```
* 보다 깔금한 코드를 작성할 수 있습니다.

<br>
<br>
<br>
<br>

## 예제

### Initialize

GuinHttp 빌더를 이용해 객체를 구현합니다. baseUrl을 필수 파라미터로 받습니다.
```c#
GuinHttp guinHttp = GuinHttp.Builder("https://maenguin.com")
                                     .SetTimeOut(5000)
                                     .Build();
```

<br>

### Get Method
Request 요청시, 넘겨주는 데이터는 `익명클래스`로 감싸서 `MethodBase.GetCurrentMethod()`와 같이 `RequestApi`함수에 넘겨줘야됩니다.

```c#
[Get("/order/{orderId}")]
public string FindOrderByIdAndStatus([Path] string orderId, [Query] string status)
{
    var param = new { orderId, status };
    string response = guinHttp.RequestApi(param, MethodBase.GetCurrentMethod());
    return response;
}
```

```c#
[Get("/order/{orderId}")]
public string FindOrderByIdAndStatuses([Path] string orderId, [Query] List<string> statusList)
{
    var param = new { orderId, statusList };
    string response = guinHttp.RequestApi(param, MethodBase.GetCurrentMethod());
    return response;
}
```

<br>

### Post Method
Content-Type은 현재 Json과 formUrlEncoded 두개를 지원합니다.



#### Json
```c#
[JsonBody]
[Post("/order")]
public string SaveOrder([Body] OrderSaveReqDto dto)
{
    var param = new { dto };
    string response = guinHttp.RequestApi(param, MethodBase.GetCurrentMethod());
    return response;
}
```


#### FormUrlEncoded
```c#
[FormUrlEncoded]
[Post("/order")]
public string SaveOrder([Body] OrderSaveReqDto dto)
{
    var param = new { dto };
    string response = guinHttp.RequestApi(param, MethodBase.GetCurrentMethod());
    return response;
}
```

<br>

### Header
```c#
[Headers("Authorization : maenguin", "hi : hello")
[FormUrlEncoded]
[Post("/oauth/token")]
public string GetAccessToken([Body] OauthTokenReqDto dto)
{
    var param = new { dto };
    string response = guinHttp.RequestApi(param, MethodBase.GetCurrentMethod());
    return response;
}
```






