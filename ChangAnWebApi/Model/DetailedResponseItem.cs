namespace ChangAnWebApi.Model
{
    public class DetailedResponseItem
    {
        public bool has_more { get; set; }
        public int total { get; set; }
        public List<ChanganApk> list { get; set; }
    }
}
