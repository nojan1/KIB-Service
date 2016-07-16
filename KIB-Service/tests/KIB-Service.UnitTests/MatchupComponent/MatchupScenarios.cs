using KIB_Service.TournamentMatchupEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Tests.MatchupComponent
{
    public static class MatchupScenariosContainer
    {
        public static readonly MatchupScenario[] Scenarios = {
            new MatchupScenario
            {
                Name = "Comp based round 1",
                Contestants = new Contestant[]
                {
                    new Contestant
                    {
                        Identifier = 1,
                        CompensationPoints = 20,
                        Score = 0
                    },
                    new Contestant
                    {
                        Identifier = 2,
                        CompensationPoints = 15,
                        Score = 0
                    },
                    new Contestant
                    {
                        Identifier = 3,
                        CompensationPoints = 25,
                        Score = 0
                    },
                    new Contestant
                    {
                        Identifier = 4,
                        CompensationPoints = 18,
                        Score = 0
                    }
                },
                ExpectedOutcome = new int[][]
                {
                    new int[] { 1, 3 },
                    new int[] { 2, 4 }
                }
            },
            new MatchupScenario
            {
                Name = "Round 2 score",
                Contestants = new Contestant[]
                {
                    new Contestant
                    {
                        Identifier = 1,
                        CompensationPoints = 20,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 3 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 2,
                        CompensationPoints = 15,
                        Score = 13,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 4 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 3,
                        CompensationPoints = 25,
                        Score = 23,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 1 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 4,
                        CompensationPoints = 18,
                        Score = 18,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 2 }
                        }
                    }
                },
                ExpectedOutcome = new int[][]
                {
                    new int[] { 1, 4 },
                    new int[] { 2, 3 }
                }
            },
            new MatchupScenario
            {
                Name = "Round 3 score",
                Contestants = new Contestant[]
                {
                    new Contestant
                    {
                        Identifier = 1,
                        CompensationPoints = 20,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 3 },
                            new ContestantWithoutPreviouseOpponents { Identifier = 4 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 2,
                        CompensationPoints = 15,
                        Score = 13,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 4 },
                            new ContestantWithoutPreviouseOpponents { Identifier = 3 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 3,
                        CompensationPoints = 25,
                        Score = 23,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 6 },
                            new ContestantWithoutPreviouseOpponents { Identifier = 2 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 4,
                        CompensationPoints = 18,
                        Score = 18,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 5 },
                            new ContestantWithoutPreviouseOpponents { Identifier = 1 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 5,
                        CompensationPoints = 19,
                        Score = 25,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 4 },
                            new ContestantWithoutPreviouseOpponents { Identifier = 6 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 6,
                        CompensationPoints = 21,
                        Score = 5,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 3 },
                            new ContestantWithoutPreviouseOpponents { Identifier = 5 }
                        }
                    }
                },
                ExpectedOutcome = new int[][]
                {
                    new int[] { 3, 5 },
                    new int[] { 2, 1 },
                    new int[] { 4, 6 }
                }
            },
            new MatchupScenario
            {
                Name = "Round 2 score only",
                Contestants = new Contestant[]
                {
                    new Contestant
                    {
                        Identifier = 1,
                        CompensationPoints = 20,
                        Score = 15,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 10 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 2,
                        CompensationPoints = 15,
                        Score = 17,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 10 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 3,
                        CompensationPoints = 25,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 10 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 4,
                        CompensationPoints = 18,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 5,
                        CompensationPoints = 18,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 6,
                        CompensationPoints = 18,
                        Score = 23,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 7,
                        CompensationPoints = 18,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 8,
                        CompensationPoints = 18,
                        Score = 24,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    }
                },
                ExpectedOutcome = new int[][]
                {
                    new int[] { 8, 6 },
                    new int[] { 7, 5 },
                    new int[] { 4, 3 },
                    new int[] { 2, 1 }
                }
            },
            new MatchupScenario
            {
                Name = "Round 2 score only (where have met before)",
                Contestants = new Contestant[]
                {
                    new Contestant
                    {
                        Identifier = 1,
                        CompensationPoints = 20,
                        Score = 15,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 10 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 2,
                        CompensationPoints = 15,
                        Score = 17,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 10 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 3,
                        CompensationPoints = 25,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 10 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 4,
                        CompensationPoints = 18,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 5,
                        CompensationPoints = 18,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 6,
                        CompensationPoints = 18,
                        Score = 23,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 7,
                        CompensationPoints = 18,
                        Score = 20,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 20 }
                        }
                    },
                    new Contestant
                    {
                        Identifier = 8,
                        CompensationPoints = 18,
                        Score = 24,
                        PreviousOpponents = new ContestantWithoutPreviouseOpponents[]
                        {
                            new ContestantWithoutPreviouseOpponents { Identifier = 6 }
                        }
                    }
                },
                ExpectedOutcome = new int[][]
                {
                    new int[] { 8, 3 },
                    new int[] { 4, 5 },
                    new int[] { 6, 7 },
                    new int[] { 2, 1 }
                }
            }
        };
    }

    public class MatchupScenario
    {
        public string Name { get; set; }
        public ICollection<Contestant> Contestants { get; set; }
        public ICollection<int[]> ExpectedOutcome { get; set; }
    }
}
