﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Models.dto
{
    public class PlayerScoreDto
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Affiliation { get; set; }
        public int Score { get; set; }
    }
}