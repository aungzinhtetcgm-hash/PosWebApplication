namespace PosWebApplication.ViewModels.Post
{
    public class EditPostViewModel
    {
        public int p_id {  get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string public_flag { get; set; }
    }
}
