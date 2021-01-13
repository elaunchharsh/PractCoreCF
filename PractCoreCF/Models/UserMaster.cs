using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PractCoreCF.Models
{
    public class UserMaster
    {
        [Column(TypeName = "int"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Password { get; set; }

        public string ContactNumber { get; set; }
        public string Hobbies { get; set; }
        public string PostCode { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        //This will add relationship in database for CountryMaster and UserMaster table
        [ForeignKey("CountryId")]
        public virtual CountryMaster CountryMaster { get; set; }

        //This will add relationship in database for StateMaster and UserMaster table
        [ForeignKey("StateId")]
        public virtual StateMaster StateMaster { get; set; }

        [ForeignKey("UserId")]
        public virtual List<UserImages> UserImages { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
    }
}
