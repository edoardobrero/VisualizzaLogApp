using System.ComponentModel.DataAnnotations;

namespace VisualizzaLog.Models
{
    public class Arplog
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime ArplogTimestamp { get; set; }

        [RegularExpression(@"^[XIHDCP]$")]
        [MaxLength(3)]
        [Required]
        public required string Flag { get; set; }

        [RegularExpression(@"^[0-9.]$")]
        [Required]
        [Display(Name = "Indirizzo IP")]
        public required string Address { get; set; }

        [RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")]
        [MaxLength(18)]
        [Display(Name = "Indirizzo MAC")]
        public string? MacAddress { get; set; }

        [Required]
        [Display(Name = "Interfaccia")]
        public required string Interface {  get; set; }

        public ICollection<ArplogViolation>? Violations { get; set; }

    }
}
