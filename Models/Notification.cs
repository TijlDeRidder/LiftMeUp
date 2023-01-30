using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftMeUp.Models
{
    public class Notification
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Status")]
        public bool isFixed { get; set; }
        [Required]
        public bool isDeleted { get; set; } = false;
        [Required]
        public DateTime createTime { get; set; } = DateTime.Now;
        [Required]
        public int liftId { get; set; }
        [Required]
        public string liftName { get; set; }

        [ForeignKey("liftId")]
        public virtual Lift? Lift { get; set; }

    }
}
