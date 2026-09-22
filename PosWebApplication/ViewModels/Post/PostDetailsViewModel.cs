using PosWebApplication.ViewModels.Comment;

namespace PosWebApplication.ViewModels.Post
{
    public class PostDetailsViewModel
    {
        public int p_id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? public_flag { get; set; }
        public string? name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public List<CommentViewModel> Comments { get; set; } = new();
    }
}