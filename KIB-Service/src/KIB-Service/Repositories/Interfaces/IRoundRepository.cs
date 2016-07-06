﻿using KIB_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Repositories.Interfaces
{
    public interface IRoundRepository
    {
        void Add(Round newRound);
        Round GetCurrentRound(int tournamentId);
        void SetScore(int matchupId, int player1Score, int player2Score);
    }
}
