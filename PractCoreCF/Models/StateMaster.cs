using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PractCoreCF.Models
{
    public class StateMaster
    {
        [Column(TypeName = "int"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }

        
        public int CountryId { get; set; }

        public string StateName { get; set; }

        //This will create relationship in CountryMaster and StateMaster table
        [ForeignKey("CountryId")]
        public virtual CountryMaster CountryMaster { get; set; }
    }

}
