namespace ChangAnWebApi.Model
{
    public class CoolApkRet
    {
        public List<CookApkRetData>? data { get; set; }
    }

    public class CookApkRetData
    {
        public string adminscore { get; set; }
        public int apklength { get; set; }
        public string apkmd5 { get; set; }
        public string apkname { get; set; }
        public string apkname2 { get; set; }
        public string apksize { get; set; }
        public string apkversioncode { get; set; }
        public string apkversionname { get; set; }
        public string cover { get; set; }
        public string description { get; set; }
        public string developername { get; set; }
        public string hotlabel { get; set; }
        public object isbiz { get; set; }
        public object iscps { get; set; }
        public object ishot { get; set; }
        public object ishp { get; set; }
        public string keywords { get; set; }
        public object last_comment_update { get; set; }
        public string lastupdate { get; set; }
        public string logo { get; set; }
        public string open_link { get; set; }
        public object pubdate { get; set; }
        public object recommend { get; set; }
        public object replynum { get; set; }
        public string romversion { get; set; }
        public string score { get; set; }
        public object sdkversion { get; set; }
        public string shortlabel { get; set; }
        public string shorttitle { get; set; }
        public object star { get; set; }
        public object status { get; set; }
        public string title { get; set; }
        public string version { get; set; }
        public int votenum { get; set; }
        public object digest { get; set; }
        public object is_forum_app { get; set; }
        public string id { get; set; }
        public object hot_num { get; set; }
        public object follownum { get; set; }
        public object favnum { get; set; }
        public object downnum { get; set; }
        public object developeruid { get; set; }
        public object commentnum { get; set; }
        public object comment_status { get; set; }
        public object comment_block_num { get; set; }
        public object catid { get; set; }
        public object apktype { get; set; }
        public string index_name { get; set; }
        public int _queryTotal { get; set; }
        public int _queryViewTotal { get; set; }
        public float _querySearchTime { get; set; }
        public string entityType { get; set; }
        public object entityId { get; set; }
        public string packageName { get; set; }
        public string shortTags { get; set; }
        public string apkTypeName { get; set; }
        public string apkTypeUrl { get; set; }
        public string apkUrl { get; set; }
        public string url { get; set; }
        public string catDir { get; set; }
        public string catName { get; set; }
        public object downCount { get; set; }
        public object followCount { get; set; }
        public object voteCount { get; set; }
        public object commentCount { get; set; }
        public object replyCount { get; set; }
        public string hot_num_txt { get; set; }
        public string updateFlag { get; set; }
        public string extraFlag { get; set; }
        public string apkRomVersion { get; set; }
        public string statusText { get; set; }
        public string pubStatusText { get; set; }
        public string commentStatusText { get; set; }
        public string target_id { get; set; }
        public string votescore { get; set; }
        public float rating_star { get; set; }
        public string changelog { get; set; }
        public string[] thumbList { get; set; }
        public string[] screenList { get; set; }
        public string entityTemplate { get; set; }
        public Origindata originData { get; set; }
        public string extraAnalysisData { get; set; }
        public int isDownloadFromYyb { get; set; }
        public int isDownloadFromYybByPy { get; set; }
        public bool hasCoupon { get; set; }
        public object[] couponInfo { get; set; }
        public int is_coolapk_cpa { get; set; }
        public int allow_rating { get; set; }
        public int get_timewit_cpc { get; set; }
    }

    public class Origindata
    {
        public string apkId { get; set; }
        public string apkMd5 { get; set; }
        public string apkUrl { get; set; }
        public string appId { get; set; }
        public string appName { get; set; }
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string channelId { get; set; }
        public int ecpm { get; set; }
        public int ext { get; set; }
        public string fileSize { get; set; }
        public string iconUrl { get; set; }
        public string logoUrl { get; set; }
        public int minSdkVersion { get; set; }
        public int pkgBitType { get; set; }
        public string pkgName { get; set; }
        public string recommendId { get; set; }
        public string shortDesc { get; set; }
        public int source { get; set; }
        public int targetSdkVersion { get; set; }
        public string totalDownloadTimes { get; set; }
        public int versionCode { get; set; }
        public string versionName { get; set; }
    }
}
