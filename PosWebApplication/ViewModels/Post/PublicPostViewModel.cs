namespace PosWebApplication.ViewModels.Post
{
    public class PublicPostViewModel
    {
        public int p_id {  get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
    }
}
