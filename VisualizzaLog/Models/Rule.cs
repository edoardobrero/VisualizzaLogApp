using System.ComponentModel.DataAnnotations;

namespace VisualizzaLog.Models
{
    public class Rule
    {
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        public string Descrizione { get; set; }

        [Required]
        public string Contenuto { get; set; }

        public ICollection<Violation>? Violations { get; set; }

        public ICollection<ArplogViolation>? ArplogViolations { get; set; }
    }
}
