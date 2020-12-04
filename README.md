# GuinHttpWebW

# 개요
낮은 버전의 윈도우에서도 동작할 수 있도록 윈폼 클라이언트 프로그램을 만들 경우 사용해야 하는 닷넷 버전에 제약을 받습니다.
닷넷프레임워크 3.5를 이용해서 개발할때 Http 통신을 위해서 닷넷에 자체 내장되어있는 Net.HttpWebRequest를 사용했습니다.
처음 사용할때는 딱히 불편함을 못느꼈는데 springboot를 배우면서 에노테이션을 사용하여 컨트롤러를 만들때 정말 편하다는 인상을 받았습니다.
그래서 C#에서도 이와 비슷하게 구현하여 사용할 수 있도록 Net.HttpWebRequest 랩핑해서 만들어봤습니다.
Retrofit2의 인터페이스를 모방해서 구현했습니다.

# 예제

## Initialize

```c#
GuinHttpWebW guinHttp = GuinHttpWebW.GuinHttpWebW.Builder("https://maenguin.com")
                                                 .SetTimeOut(5000)
                                                 .Build();
```

## Get Method
C#의 Attribute로 java의 에노테이션을 따라했기에 기술적인 제약이 있습니다. 
그래서 아래 예제처럼, 요청시 넘겨주는 데이터는 `익명클래스`로 감싸서 `MethodBase.GetCurrentMethod()`와 같이 `RequestApi`함수에 넘겨줘야됩니다.

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

### json
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

### formUrlEncoded
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
public string SaveOrder([Body] OrderSaveReqDto dto)
{
    var param = new { dto };
    string response = guinHttp.RequestApi(param, MethodBase.GetCurrentMethod());
    return response;
}
```






