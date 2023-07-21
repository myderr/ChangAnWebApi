namespace ChangAnWebApi.Model
{
    public class DetailedResponse
    {
        public DetailedResponseItem data { get; set; }
        public string request_id { get; set; }
        public string result_code { get; set; }
        public int server_time { get; set; }
    }
}
