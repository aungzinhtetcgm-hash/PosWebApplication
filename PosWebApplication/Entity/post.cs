namespace PosWebApplication.Entity
{
    public partial class post
    {
        public post()
        {
            comments = new HashSet<comment>();
        }
    
        public int p_id {  get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? public_flag { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;

        public virtual user user { get; set; }
        public virtual ICollection<comment> comments { get; set; }
    }
}
