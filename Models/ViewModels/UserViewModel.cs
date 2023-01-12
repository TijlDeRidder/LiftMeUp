using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LiftMeUp.Models.ViewModels
{
        public class UserViewModel
        {
            [Display(Name = "Gebruiker")]
            public string UserName { get; set; }

            [Display(Name = "Voornaam")]
            public string FirstName { get; set; }

            [Display(Name = "Familienaam")]
            public string LastName { get; set; }

            [Display(Name = "E-mail")]
            public string Email { get; set; }

            [Display(Name = "Verwijderd")]
            public bool Deleted { get; set; }


            [Display(Name = "Rollen")]
            public List<string> Roles { get; set; }
        }

        public class RoleViewModel
        {
            [Display(Name = "Gebruiker")]
            public string UserName { get; set; }

            [Display(Name = "Rollen")]
            public List<string> Roles { get; set; }
        }
 }

