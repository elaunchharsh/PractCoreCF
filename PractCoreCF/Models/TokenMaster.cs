using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PractCoreCF.Models
{
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
