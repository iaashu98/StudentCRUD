using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDTS.Models
{
    public class StudentModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        [Required]
        public string Subject_opted { get; set; }
    }
}
