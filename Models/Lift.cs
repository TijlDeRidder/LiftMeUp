using LiftMeUp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftMeUp.Models
{
    public class Lift
    {
        public static List<Lift> Lifts = new List<Lift>();
        [Required]
        public int liftId { get; set; }
        [Required]
        [Display(Name = "Naam")]
        public string name { get; set; }
        [Required]
        public int stationId { get; set; }
        [Required]
        [Display(Name = "Werkt")]
        public bool isWorking { get; set; }
        [Required]
        public bool isDeleted { get; set; } = false;
        [ForeignKey("stationId")]
        public virtual Station? Station { get; set; }
    }
}
