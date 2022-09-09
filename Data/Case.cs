namespace Data
{
    public class Case
    {
        public Case()
        {

        }
        public string Id { get; set; }

        public string? TypeOfCase { get; set; }

        public string? CaseNumber { get; set; }

        public string? TypeOfAct { get; set; }

        public string? Content { get; set; }

        public string? Answer { get; set; }

        public string? Decision { get; set; }

        public string? Motives { get; set; }

        public string? FullText { get; set; }

        public string? JudjeId { get; set; }

        public virtual Judje? Judje { get; set; }
    }
}
