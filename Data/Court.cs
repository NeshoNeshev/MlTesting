namespace Data
{
    public class Court
    {
        public Court()
        {
            this.Judjes = new HashSet<Judje>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Judje> Judjes { get; set; }
    }
}
