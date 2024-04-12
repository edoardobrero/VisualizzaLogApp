using System.ComponentModel.DataAnnotations;

namespace VisualizzaLog.Models
{
    public class Connection
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime ConnectionTimestamp { get; set; }

        [RegularExpression(@"^[ESACDF]$")]
        [MaxLength(6)]
        public string? Flag { get; set; }

        [RegularExpression(@"^[sd]$")]
        [MaxLength(1)]
        [Display(Name = "Flag NAT")]
        public string? NatFlag { get; set; }

        [RegularExpression(@"^[a-zA-z]$")]
        [Display(Name = "Protocollo")]
        public required string Protocol { get; set; }

        [RegularExpression(@"^[0-9.]$")]
        [Display(Name = "IP Sorgente")]
        public required string SRCAddress { get; set; }

        [RegularExpression(@"^[0-9]$")]
        [Display(Name = "Porta Sorgente")]
        public required string SRCPort { get; set; }

        [RegularExpression(@"^[0-9.]$")]
        [Display(Name = "IP Destinatario")]
        public required string DSTAddress { get; set; }

        [RegularExpression(@"^[0-9]$")]
        [Display(Name = "Porta Destinatario")]
        public required string DSTPort { get; set; }

        [RegularExpression(@"^[a-zA-z]$")]
        [Display(Name = "Stato TCP")]
        public string? TCPState { get; set; }

        public ICollection<Violation>? Violations { get; set; }
    }
}
