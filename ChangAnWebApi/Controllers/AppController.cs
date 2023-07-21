using ChangAnWebApi.Model;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ChangAnWebApi.Controllers
{
    /// <summary>
    /// 应用商店控制器
    /// </summary>
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]//自定义Swagger版本声明版本
    [ApiVersion("1.0")]//框架支持Swagger版本声明版本
    //{version:apiVersion}
    [Route("[controller]/{version:apiVersion}")]
    public class AppController : ControllerBase
    {
        static private string ScreenKey = string.Empty;

        private readonly IMemoryCache memoryCache;

        public AppController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取App搜索字段
        /// </summary>
        [HttpGet()]
        [Route("/api/App/GetAppScreenKey")]
        public IActionResult GetAppScreenKey()
        {
            return Ok(ScreenKey);
        }

        /// <summary>
        /// 获取Alist应用
        /// </summary>
        [HttpGet()]
        [HttpPost()]
        [Route("/api/App/GetAListApps")]
        public async Task<IActionResult> GetAListApps()
        {
            List<ChanganApk> apks = new List<ChanganApk>();
            apks = await GetAlistApps();
            return Ok(apks);
        }

        /// <summary>
        /// 设置App搜索字段
        /// </summary>
        [HttpGet()]
        [Route("/api/App/SetAppScreenKey")]
        public async Task<IActionResult> SetAppScreenKey(string scree)
        {
            ScreenKey = scree;
            var apks = new List<ChanganApk>();
            memoryCache.TryGetValue(ScreenKey, out apks);
            if (apks == null)
            {
                apks = await GetCoolAppBySrceeName();
                memoryCache.Set(ScreenKey, apks, TimeSpan.FromMinutes(5));
            }
            return Ok(apks);
        }

        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="device_id"></param>
        /// <param name="os"></param>
        /// <param name="app_version"></param>
        /// <param name="os_version"></param>
        /// <param name="device_type"></param>
        /// <param name="app_id"></param>
        /// <param name="nonce"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("/api/1/appstore/app_type/list")]
        public IActionResult app_type_list(string channel, string device_id, string os,
            string app_version, string os_version, string device_type, string app_id, string nonce, string timestamp)
        {
            MenuResponse menuResponse = new MenuResponse();
            menuResponse.data = new List<MenuResponseItem>()
            {
                new MenuResponseItem()
                {
                    id=1,
                    name="服务器",
                    platform="changan",
                    create_time=1653056495,
                    update_time=1653056495
                },
                new MenuResponseItem()
                {
                    id=2,
                    name="酷安",
                    platform="changan",
                    create_time=1653056495,
                    update_time=1653056495
                }
            };
            menuResponse.request_id = "kk-rBgjOmP7zcoDey+g";
            menuResponse.result_code = "success";
            menuResponse.server_time = 1653056495;
            return Ok(menuResponse);
        }

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <param name="detailRequest"></param>
        /// <param name="device_id"></param>
        /// <param name="os"></param>
        /// <param name="app_version"></param>
        /// <param name="os_version"></param>
        /// <param name="device_type"></param>
        /// <param name="app_id"></param>
        /// <param name="nonce"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("/api/1/appstore/app_version/check_update")]
        public async Task<IActionResult> check_update([FromBody] DetailRequest detailRequest, string device_id, string os,
            string app_version, string os_version, string device_type, string app_id, string nonce, string timestamp)
        {
            DetailedResponse detailedResponse = new DetailedResponse();
            detailedResponse.request_id = "kk-rBgjOmP7zcoDey+g";
            detailedResponse.result_code = "success";
            detailedResponse.server_time = 1653056495;
            DetailedResponseItem detailedResponseItem = new DetailedResponseItem();
            detailedResponseItem.has_more = false;
            detailedResponse.data = detailedResponseItem;
            List<ChanganApk> apks = new List<ChanganApk>();
            switch (detailRequest.app_type)
            {
                case 1:
                    {
                        apks = await GetAlistApps();
                        break;
                    }
                case 2:
                    {
                        memoryCache.TryGetValue(ScreenKey, out apks);
                        if (apks != null)
                        {
                            break;
                        }
                        apks = await GetCoolAppBySrceeName();
                        memoryCache.Set(ScreenKey, apks, TimeSpan.FromMinutes(5));
                        break;
                    }
            }
            detailedResponseItem.total = apks.Count;
            detailedResponseItem.list = apks;
            return Ok(detailedResponse);
        }

        /// <summary>
        /// 获取酷安应用
        /// </summary>
        /// <returns></returns>
        protected async Task<List<ChanganApk>> GetCoolAppBySrceeName()
        {
            var apks = new List<ChanganApk>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.coolapk.com/apk/search?q={ScreenKey}&platform=phone&sort=rate&p=1"),
                Headers =
                {
                    { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7" },
                    { "Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,zh-HK;q=0.7,ja;q=0.6" },
                    { "Cache-Control", "no-cache" },
                    { "Connection", "keep-alive" },
                    { "Cookie", "SESSID=375e-64b9f9cd5ae59-1689909709-3679; Hm_lvt_7132d8577cc4aa4ae4ee939cd42eb02b=1689909712; Hm_lpvt_7132d8577cc4aa4ae4ee939cd42eb02b=1689913872" },
                    { "Pragma", "no-cache" },
                    { "Sec-Fetch-Dest", "document" },
                    { "Sec-Fetch-Mode", "navigate" },
                    { "Sec-Fetch-Site", "none" },
                    { "Sec-Fetch-User", "?1" },
                    { "Upgrade-Insecure-Requests", "1" },
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36" },
                    { "sec-ch-ua", "\"Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"" },
                    { "sec-ch-ua-mobile", "?0" },
                    { "sec-ch-ua-platform", "\"Windows\"" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(body);
                var root = html.DocumentNode;
                var nodes = root.SelectNodes("//div[@class='app_list_left']/a");
                var max = nodes.Count > 5 ? 5 : nodes.Count;
                for (int i = 0; i < max; i++)
                {
                    var node = nodes[i];
                    ChanganApk apk = new ChanganApk();
                    var package_name = node.GetAttributeValue("href", "");
                    //去掉前面5个字符
                    package_name = package_name[5..];
                    if (string.IsNullOrEmpty(package_name)) continue;
                    apk.package_name = package_name;

                    var p = node.SelectSingleNode("./div/div/div/p");
                    var app_name = p.InnerText;
                    apk.app_name = app_name;
                    apk.description = app_name;

                    var img = node.SelectSingleNode("./div/div/img");
                    var logo_image_url = img.GetAttributeValue("src", "");
                    if (string.IsNullOrEmpty(logo_image_url))
                        logo_image_url = "http://p0.meituan.net/csc/3e12662d3943ab8db34cb134081508902751.png";
                    logo_image_url = HttpToHttps(logo_image_url);
                    apk.logo_image_url = logo_image_url;

                    (var package_url, var version_name) = await GetCoolApkUrl(apk.package_name);
                    apk.package_url = package_url;
                    apk.version_name = version_name;

                    apk.id = i + 1;
                    apk.version_code = i + 1;
                    apk.owner = "other";
                    apk.preinstall_type = 3;
                    apk.status = 1;
                    apk.min_os_version = "";
                    apk.show_status_bar = false;
                    apk.support_horizontal_key_board = true;
                    apk.release_time = 1689915738;
                    apk.package_size = await GetUrlFileSize(apk.package_url);//10M
                    apk.download_times = apk.package_size / 3000;

                    apks.Add(apk);
                }
            }


            async Task<(string, string)> GetCoolApkUrl(string apkname)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://www.coolapk.com/apk/{apkname}"),
                    Headers =
                            {
                                { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7" },
                                { "Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,zh-HK;q=0.7,ja;q=0.6" },
                                { "Cache-Control", "no-cache" },
                                { "Connection", "keep-alive" },
                                { "Cookie", "SESSID=375e-64b9f9cd5ae59-1689909709-3679; Hm_lvt_7132d8577cc4aa4ae4ee939cd42eb02b=1689909712; Hm_lpvt_7132d8577cc4aa4ae4ee939cd42eb02b=1689913872" },
                                { "Pragma", "no-cache" },
                                { "Sec-Fetch-Dest", "document" },
                                { "Sec-Fetch-Mode", "navigate" },
                                { "Sec-Fetch-Site", "none" },
                                { "Sec-Fetch-User", "?1" },
                                { "Upgrade-Insecure-Requests", "1" },
                                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36" },
                                { "sec-ch-ua", "\"Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"" },
                                { "sec-ch-ua-mobile", "?0" },
                                { "sec-ch-ua-platform", "\"Windows\"" },
                            },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var version_name = "";
                    HtmlDocument html = new HtmlDocument();
                    html.LoadHtml(body);
                    var root = html.DocumentNode;
                    var a = root.SelectSingleNode("//a[@class='show-dialog']");
                    var download_url = a.GetAttributeValue("href", "");
                    var span = root.SelectSingleNode("//span[@class='list_app_info']");
                    version_name = span.InnerText.Trim();
                    if (string.IsNullOrEmpty(version_name))
                        version_name = "1.0.0.";

                    if (string.IsNullOrEmpty(download_url))
                        return ("", version_name);

                    return (HttpToHttps(await GetLocationUrl(download_url)), version_name);
                }
            }

            async Task<string> GetLocationUrl(string url)
            {
                HttpClientHandler handler = new HttpClientHandler
                {
                    AllowAutoRedirect = false // 禁止重定向
                };
                var client = new HttpClient(handler);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Headers =
                            {
                                { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7" },
                                { "Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,zh-HK;q=0.7,ja;q=0.6" },
                                { "Cache-Control", "no-cache" },
                                { "Connection", "keep-alive" },
                                { "Cookie", "SESSID=375e-64b9f9cd5ae59-1689909709-3679; Hm_lvt_7132d8577cc4aa4ae4ee939cd42eb02b=1689909712; Hm_lpvt_7132d8577cc4aa4ae4ee939cd42eb02b=1689913872" },
                                { "Pragma", "no-cache" },
                                { "Sec-Fetch-Dest", "document" },
                                { "Sec-Fetch-Mode", "navigate" },
                                { "Sec-Fetch-Site", "none" },
                                { "Sec-Fetch-User", "?1" },
                                { "Upgrade-Insecure-Requests", "1" },
                                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36" },
                                { "sec-ch-ua", "\"Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"" },
                                { "sec-ch-ua-mobile", "?0" },
                                { "sec-ch-ua-platform", "\"Windows\"" },
                            },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.Headers.TryGetValues("Location", out var values);
                    if (values != null)
                    {
                        var locationUrl = values.FirstOrDefault();
                        if (!string.IsNullOrEmpty(locationUrl))
                        {
                            return locationUrl;
                        }
                    }
                }
                return "";
            }
            return apks;
        }

        async Task<int> GetUrlFileSize(string url)
        {
            try
            {

                var httpClientHandler = new HttpClientHandler
                {
                    // 设置 ServerCertificateCustomValidationCallback
                    ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        // 忽略证书错误，总是返回 true
                        return true;
                    }
                };
                using (HttpClient clienta = new HttpClient(httpClientHandler))
                {
                    using (HttpRequestMessage requesta = new HttpRequestMessage(HttpMethod.Head, url))
                    {
                        using (HttpResponseMessage response = await clienta.SendAsync(requesta))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.Content.Headers.ContentLength.HasValue)
                                {
                                    long fileSize = response.Content.Headers.ContentLength.Value;
                                    return (int)fileSize;
                                }
                                else
                                {
                                    return 1000000;
                                }
                            }
                            else
                            {
                                return 1000000;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                return 1000000;
            }
        }
        private string HttpToHttps(string url)
        {
            return Regex.Replace(url, @"^http://", "https://");
        }

        /// <summary>
        /// 获取Alist服务器应用
        /// </summary>
        /// <returns></returns>
        protected async Task<List<ChanganApk>> GetAlistApps()
        {
            List<ChanganApk> apks = new List<ChanganApk>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://alist.49os.com/api/fs/list"),
                Content = new StringContent("{\"path\":\"/\",\"password\":\"\",\"page\":1,\"per_page\":0,\"refresh\":false}")
                {
                    Headers =
                            {
                                ContentType = new MediaTypeHeaderValue("application/json")
                            }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                AListResponse aListResponse = JsonConvert.DeserializeObject<AListResponse>(body);
                if (aListResponse.code == 200)
                {
                    int i = 0;
                    foreach (var item in aListResponse.data.content)
                    {
                        var nameSplit = item.name.Split('.');
                        if (nameSplit.Length > 0 && nameSplit[nameSplit.Length - 1].ToLower() == "apk")
                        {
                            var apk = new ChanganApk();
                            apk.package_name = nameSplit[0];
                            apk.app_name = item.name;
                            apk.description = item.name;
                            var logo_image_url = "http://p0.meituan.net/csc/3e12662d3943ab8db34cb134081508902751.png";
                            logo_image_url = HttpToHttps(logo_image_url);
                            apk.logo_image_url = logo_image_url;

                            apk.package_url = $"https://alist.49os.com/d/{item.name}";
                            apk.version_name = $"https://alist.49os.com/d/{item.name}";

                            apk.id = i + 1;
                            apk.version_code = i + 1;
                            apk.owner = "other";
                            apk.preinstall_type = 3;
                            apk.status = 1;
                            apk.min_os_version = "";
                            apk.show_status_bar = false;
                            apk.support_horizontal_key_board = true;
                            apk.release_time = 1689915738;
                            apk.package_size = item.size;//10M
                            apk.download_times = item.size / 3000;

                            apks.Add(apk);
                            i++;
                        }
                    }
                }
            }

            return apks;
        }
    }


}
