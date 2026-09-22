namespace PosWebApplication.Entity
{
    public partial class user
    {
        public user()
        {
            posts = new HashSet<post>();
            comments = new HashSet<comment>();
        }
        public int u_id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? name { get; set; }
        public string? img { get; set; }
        public int role { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;

        public virtual ICollection<post>? posts { get; set; }
        public virtual ICollection<comment>?comments { get; set; }

    }
}
