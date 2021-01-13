using System;
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

        [Column(TypeName = "nvarchar")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Password { get; set; }

        public string ContactNumber { get; set; }
        public string Hobbies { get; set; }
        public string PostCode { get; set; }
        public int Gender { get; set; }
        public int Address { get; set; }

        //We will save selected country's id from user in this column
        public int CountryId { get; set; }

        //We will save selected state's id based on country from user in this column
        public int StateId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        //This will add relationship in database for CountryMaster and UserMaster table
        public CountryMaster CountryMaster { get; set; }

        //This will add relationship in database for StateMaster and UserMaster table
        public StateMaster StateMaster { get; set; }
    }

    public class CountryMaster
    {
        [Column(TypeName = "int"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        public string CountryName { get; set; }
    }

    public class StateMaster
    {
        [Column(TypeName = "int"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }

        public int CountryId { get; set; }

        public string StateName { get; set; }

        //This will create relationship in CountryMaster and StateMaster table
        public CountryMaster CountryMaster { get; set; }
    }

    public class TokenMaster
    {
        [Column(TypeName = "int"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenMasterId { get; set; }

        public int UserId { get; set; }

        //We will store JWT Token in database so
        public string Token { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }

}
