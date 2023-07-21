namespace ChangAnWebApi.Model
{
    public class AListResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public List<AListResponseItem> content { get; set; }
        public int total { get; set; }
        public string readme { get; set; }
        public bool write { get; set; }
        public string provider { get; set; }
    }

    public class AListResponseItem
    {
        public string name { get; set; }
        public int size { get; set; }
        public bool is_dir { get; set; }
        public DateTime modified { get; set; }
        public string sign { get; set; }
        public string thumb { get; set; }
        public int type { get; set; }
    }

}
