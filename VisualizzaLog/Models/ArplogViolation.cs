using System.ComponentModel.DataAnnotations;

namespace VisualizzaLog.Models
{
    public class ArplogViolation
    {
        public int Id { get; set; }

        public int ArplogId { get; set; }
        [Display(Name = "Mapping MAC")]
        public Arplog Arplog { get; set; }

        public int RuleId { get; set; }
        [Display(Name = "Regola")]
        public Rule Rule { get; set; }
    }
}
