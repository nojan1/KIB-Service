using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class TournamentDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTimeOffset? Date { get; set; }
    }
}
