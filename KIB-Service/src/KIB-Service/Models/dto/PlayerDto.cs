using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class PlayerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Affiliation { get; set; }

        public int Id { get; set; }
        public bool Active { get; set; }
    }
}
