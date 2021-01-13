using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.ModelView
{
    public class UserMasterView
    {
        public int UserId { get; set; }

        [Required(ErrorMessage ="Please enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Please enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Please enter Email")]
        [EmailAddress(ErrorMessage ="Please enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter contact number")]
        public string ContactNumber { get; set; }

        public List<string> Hobbies { get; set; }

        public string PostCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select gender")]
        [Required(ErrorMessage ="Please select gender")]
        public int Gender { get; set; }

        [Required(ErrorMessage ="Please enter address")]
        public string Address { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public CountryMasterView CountryMaster { get; set; }

        public StateMasterView StateMaster { get; set; }

       
        public List<UserImagesView> UserImages { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
    }

    public class Hobbies { 
        public int HobbyId { get; set; }
        public string HobbyName { get; set; }
    }
}
