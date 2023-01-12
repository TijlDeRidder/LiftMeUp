using LiftMeUp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LiftMeUp.Models
{
    public class Melding
    {
        public static List<Melding> Meldingen = new List<Melding>();
        [Required]
        public int MeldingId { get; set; }
        [Required]
        public int liftId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int stationId { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Datum")]
        public DateTime startDate { get; set; } = DateTime.Now;
        [Required]
        public bool isDeleted { get; set; } = false;
        [Required]
        [Display(Name = "uitleg defect")]
        public string uitleg { get; set; }


        public LiftMeUpUser? User { get; set; }
    }
}
