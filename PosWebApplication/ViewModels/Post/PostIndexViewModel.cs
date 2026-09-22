namespace PosWebApplication.ViewModels.Post
{
    public class PostIndexViewModel
    {
        public List<PublicPostViewModel> PublicPosts { get; set; } = new();
        public List<PrivatePostViewModel> PrivatePosts { get; set; } = new();
    }
}
