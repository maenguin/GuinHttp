# GuinHttpWebW
Java의 @을 따라한 C# 전용 Http 통신 라이브러리
.netFramwork 3.5






# 개요
윈폼을 개발할때 xp 이하의 버전을 지원하기 위해 닷넷 4.0 이하 버전을 사용하고 있습니다.
닷넷 버전이 낮기 때문에 Http 통신라이브러리도 닷넷에 자체 내장되어있는 `Net.HttpWeb`을 사용해왔습니다.
처음 사용할때는 불편하다고 생각하지 못했지만 springboot를 배우면서 에노테이션을 사용하여 컨트롤러를 만들때 정말 편하다는 인상을 받았습니다. 
그래서 C#도 이와 비슷한 인터페이스가 있으면 좋다고 생각해서 만들게 되었습니다.
Retrofit2의 인터페이스를 모방해서 구현했습니다.
C#의 `Attribute`로 java의 `@`을 따라했기에 기술적인 제약이 있습니다. d



# 예제



## Initialize

GuinHttpWebW 빌더를 이용해 객체를 구현합니다. baseUrl을 필수 파라미터로 받습니다.
```c#
GuinHttpWebW guinHttp = GuinHttpWebW.Builder("https://maenguin.com")
                                     .SetTimeOut(5000)
                                     .Build();
```



## Get Method
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



## Post Method
Content-Type은 현재 Json과 formUrlEncoded 두개를 지원합니다.



### Json
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

### FormUrlEncoded
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



## Header
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






