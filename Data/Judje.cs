namespace Data
{
    public class Judje
    {
        public Judje()
        {
            this.Cases = new HashSet<Case>();
        }
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string ?CourtId { get; set; }

        public virtual Court? Court { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
