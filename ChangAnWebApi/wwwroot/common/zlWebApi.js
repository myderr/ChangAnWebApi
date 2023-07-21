class HrsSecurity {
    static GetTokenKey() {
        let t = sessionStorage.getItem("Token");
        if (!t) {
            t = localStorage.getItem("Token");
            sessionStorage.setItem("Token", t);
        }

        return t;
    }

    static GetUserInfo() {
        let u = sessionStorage.getItem("UserLoginInfo");
        if (!u) {
            u = localStorage.getItem("UserLoginInfo");
            sessionStorage.setItem("UserLoginInfo", u);
        }

        return u;
    }

    static SetSecurity(tk, userInfo) {
        sessionStorage.setItem("UserLoginInfo", JSON.stringify(userInfo));
        sessionStorage.setItem("Token", tk);

        localStorage.setItem("UserLoginInfo", JSON.stringify(userInfo));
        localStorage.setItem("Token", tk);
    }

    static Clear() {
        delete sessionStorage.UserLoginInfo;
        delete localStorage.UserLoginInfo;

        delete sessionStorage.Token;
        delete localStorage.Token;
    }
}


/// <summary>
/// ajax调用--post方法
///<param name="url">调用地址，不包含ip及端口</param>
///<param name="postData">请求参数</param>
///<param name="rooturl">可选参数，服务器地址</param>
///<return>返回对象：{isOk: true,stateCode:"",data: "",msg: ""}</return>
///isOk:true||false
///stateCode:状态码
///data:服务器返回的数据
///msg：错误消息
/// </summary>
function zlPost(url, postData, rooturl = "http://192.168.9.3:8800/", timeout = 0) {
    if (window.HrsEnv?.Release) {
        let ret = window.HrsEnv.zlPost(url, postData);
        if (ret) return ret;
    }

    var result = {
        Success: false,
        Code: "",
        Data: undefined,
        Msg: "",
    }
    var token = HrsSecurity.GetTokenKey();
    //超时时间设置
    if (timeout == undefined || timeout == null || timeout == "" || timeout == "null") {
        timeout = 0;
    }
    $.ajax({
        url: rooturl + url,
        type: "post",
        async: false,
        timeout: timeout,
        data: postData,
        beforeSend: (xhr) => {
            if (token) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + token);
            }
        },
        contentType: "application/json",
        success: function (res, status, xhr) {
            //result.isOk = true;
            //result.stateCode = status;
            //result.data = res;
            //result.msg = res;
            result = res;
        },
        error: function (xhr, status, error) {
            if (xhr.status == 401) {
                sessionStorage.setItem("lastUrl", location.href);
                window.location.href = "/user/login?unauthorized=1";
            }
            if (xhr.responseJSON == undefined || xhr.responseJSON == "" || xhr.responseJSON == null) {
                if (xhr.responseText == undefined || xhr.responseText == "" || xhr.responseText == null) {
                    result.Msg = "出现未知异常！";
                    result.Data = "出现未知异常！";
                    result.Success = false;
                }
                else {
                    result = JSON.parse(xhr.responseText);
                }
            }
            else {
                result = xhr.responseJSON;
            }

            //JSON.parse(postData) 输出基本的错误信息
            zlHint.Error("POST服务调用错误", url, 'status:' + xhr.status, new Error(error + ":" + (xhr.responseJSON?.Msg) ?? ""));
        },
    });

    return result;
}

function asyncPost(url, postData, stateCb, timeout) {
    if (window.HrsEnv?.Release) {
        let ret = window.HrsEnv.zlPost(url, postData);
        if (ret) return ret;
    }

    var result = {
        Success: false,
        Code: "",
        Data: undefined,
        Msg: "",
    }
    var token = HrsSecurity.GetTokenKey();
    //超时时间设置
    if (!timeout) {
        timeout = 0;
    }

    $.ajax({
        url: url,
        type: "post",
        async: true,
        timeout: timeout,
        data: postData,
        beforeSend: (xhr) => {
            if (token) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + token);
            }
        },
        contentType: "application/json",
        success: function (res, status, xhr) {
            //result.isOk = true;
            //result.stateCode = status;
            //result.data = res;
            //result.msg = res;
            if (stateCb) {
                stateCb({
                    res: res,
                    status: status,
                    xhr:xhr,
                })
            }
        },
        error: function (xhr, status, error) {
            if (stateCb) {
                stateCb({
                    status: status,
                    xhr: xhr,
                    error:error
                })
            }
        },
    });

    return result;
}


hrsPost = zlPost;


/// <summary>
/// ajax调用--GET方法
///<param name="url">调用地址，不包含ip及端口</param>
///<param name="rooturl">可选参数，服务器地址</param>
///<return>返回对象：{isOk: true,stateCode:"",data: "",msg: ""}</return>
///isOk:true||false
///stateCode:状态码
///data:服务器返回的数据
///msg：错误消息
/// </summary>
function zlGet(url, rooturl = "http://192.168.9.3:8800/", timeout = 0) {
    if (window.HrsEnv?.Release) {
        let ret = window.HrsEnv.zlGet(url);
        if (ret) return ret;
    }

    var result = {
        Success: false,
        Code: "",
        Data: undefined,
        Msg: "",
    }

    var ipPath = localStorage.getItem(rooturl);
    if (ipPath == null || ipPath == undefined || ipPath == "") {
        ipPath = rooturl;
    }
    var token = HrsSecurity.GetTokenKey();
    //超时时间设置
    if (timeout == undefined || timeout == null || timeout == "" || timeout == "null") {
        timeout = 0;
    }
    $.ajax({
        url: ipPath + url,
        type: "get",
        async: false,
        timeout: timeout,
        contentType: "application/json",
        beforeSend: (xhr) => {
            if (token) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + token);
            }
        },
        success: function (res, status, xhr) {
            //result.isOk = true;
            //result.stateCode = status;
            //result.data = res;
            //result.msg = res;
            result = res;
        },
        error: function (xhr, status, error) {
            result.Success = false;
            result.Code = status;
            result.Data = undefined;
            if (xhr.status == 401) {
                sessionStorage.setItem("lastUrl", location.href);
                window.location.href = "/user/login?unauthorized=1";
            }
            if (xhr.responseJSON == undefined || xhr.responseJSON == "" || xhr.responseJSON == null) {
                if (xhr.responseText == undefined || xhr.responseText == "" || xhr.responseText == null) {
                    result.Msg = "出现未知异常！";
                    result.Data = "出现未知异常！";
                }
                else {
                    result.Msg = xhr.responseText;
                    result.Data = xhr.responseText;
                }
            }
            else {
                //result.msg = xhr.responseJSON;
                //result.data = xhr.responseJSON;
                result = xhr.responseJSON;
            }
            
            zlHint.Error("GET服务调用错误", url, 'status:' + xhr.status, new Error(error + ":" + (xhr.responseJSON?.Msg) ?? ""));
        },
    });
    return result;
}

hrsGet = zlGet;

/// <summary>
/// ajax调用
///<param name="url">调用地址，完整地址</param>
///<param name="method">请求方式：Get|Post</param>
///<param name="requestData">请求参数</param>
///<param name="username">Basic认证：用户名</param>
///<param name="password">Basic认证：密码</param>
///<return>返回对象：{isOk: true,stateCode:"",data: "",msg: ""}</return>
///isOk:true||false
///stateCode:状态码
///data:服务器返回的数据
///msg：错误消息
/// </summary>
function zlWebApi(url, method, requestData, username, password) {
    if (window.HrsEnv?.Release) {
        let ret = window.HrsEnv.zlWebApi(url, method, requestData, username, password);
        if (ret) return ret;
    }

    var result = {
        isOk: false,
        stateCode: "",
        data: undefined,
        msg: "",
    }
    var res = window.btoa(username + ":" + password);
    $.ajax({
        url: url,
        type: method,
        async: false,
        data: requestData,
        beforeSend: (xhr) => {
            xhr.setRequestHeader("Authorization", 'Basic ' + res);
        },
        contentType: "application/json",
        success: function (res, status, xhr) {
            result.isOk = true;
            result.stateCode = status;
            result.data = res;
            result.msg = "";
        },
        error: function (xhr, status, error) {
            result.isOk = false;
            result.stateCode = status;
            result.data = undefined;
            if (xhr.responseJSON == undefined || xhr.responseJSON == "" || xhr.responseJSON == null) {
                if (xhr.responseText == undefined || xhr.responseText == "" || xhr.responseText == null) {
                    result.msg = "出现未知异常！";
                }
                else {
                    result.msg = xhr.responseText;
                }
            }
            else {
                result.msg = xhr.responseJSON;
            }
        },
    });

    return result;
}