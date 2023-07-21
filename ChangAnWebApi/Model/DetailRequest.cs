namespace ChangAnWebApi.Model
{
    /// <summary>
    /// 明细请求类
    /// </summary>
    public class DetailRequest
    {
        public int app_type { get; set; }
        public object[] package_version_list { get; set; }
        public string channel { get; set; }
        public int count { get; set; }
        public string external_os_ver { get; set; }
        public string mega_os_ver { get; set; }
        public bool need_not_installed { get; set; }
        public bool need_not_visible { get; set; }
        public int offset { get; set; }
        public string vid { get; set; }
    }

}
