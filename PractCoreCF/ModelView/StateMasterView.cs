using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.ModelView
{
    public class StateMasterView
    {
        [Required(ErrorMessage ="Please select state")]
        public int StateId { get; set; }

        public int CountryId { get; set; }

        public string StateName { get; set; }

        public CountryMasterView CountryMaster { get; set; }
    }
}
