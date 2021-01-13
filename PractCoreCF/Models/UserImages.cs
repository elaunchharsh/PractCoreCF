using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.Models
{
    public class UserImages
    {
        [Column(TypeName = "int"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserImageId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
    }
}
