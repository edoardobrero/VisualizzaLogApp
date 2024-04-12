using System.ComponentModel.DataAnnotations;

namespace VisualizzaLog.Models
{
    public class Violation
    {
        public int Id { get; set; } 

        public int ConnectionId { get; set; }
        [Display(Name = "Connessione")]
        public Connection Connection { get; set; }

        public int RuleId { get; set; }
        [Display(Name = "Regola")]
        public Rule Rule { get; set; }
    }
}
