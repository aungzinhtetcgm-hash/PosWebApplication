namespace PosWebApplication.ViewModels.Post
{
    public class PrivatePostViewModel
    {
        public int p_id {  get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
