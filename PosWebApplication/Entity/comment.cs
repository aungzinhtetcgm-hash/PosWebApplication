namespace PosWebApplication.Entity
{
    public partial class comment
    {
        public int c_id { get; set; }
        public int u_id { get; set; }
        public int p_id { get; set; }
        public string? comments {  get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;

        public virtual user user { get; set; }
        public virtual post post { get; set; }
    }
}
