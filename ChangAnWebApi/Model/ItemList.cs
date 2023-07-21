namespace ChangAnWebApi.Model
{
    public class ChanganApk
    {
        public int id { get; set; }
        public string package_name { get; set; }
        public int version_code { get; set; }
        public string version_name { get; set; }
        public string app_name { get; set; }
        public string owner { get; set; }
        public int release_time { get; set; }
        public string description { get; set; }
        public string logo_image_url { get; set; }
        public int preinstall_type { get; set; }
        public int status { get; set; }
        public int download_times { get; set; }
        public string package_url { get; set; }
        public int package_size { get; set; }
        public string min_os_version { get; set; }
        public bool show_status_bar { get; set; }
        public bool support_horizontal_key_board { get; set; }
        public bool driving_restriction_of_central_control_screen { get; set; }
        public bool driving_restriction_of_front_passenger_screen { get; set; }
        public bool show_on_front_passenger_screen { get; set; }
        public bool exit_app_when_locking_car { get; set; }
        public bool add_to_task_manager { get; set; }
        public bool can_hide_status { get; set; }
        public bool is_dark_app { get; set; }
        public bool screen_mutual_exclusion { get; set; }
    }
}
