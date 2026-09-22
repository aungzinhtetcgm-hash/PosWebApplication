namespace PosWebApplication.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int c_id {  get; set; }
        public int u_id { get; set; }
        public string? name { get; set; }
        public string? comments { get; set; }
        public DateTime created_at { get; set; }

    }
}
