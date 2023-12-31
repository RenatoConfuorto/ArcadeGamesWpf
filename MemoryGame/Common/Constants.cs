﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Common
{
    public class Constants
    {
        public const int WAIT_TIME_CARD_TURN_BACK = 350; //time to wait before turning back the cards

        public enum CardTypes
        {
            alien = 0,
            bug = 1,
            duck = 2,
            rocket = 3,
            spaceship = 4,
            tiktac = 5
        }

        #region Cards Number
        public const int CARDS_NO_EASY = 12;
        public const int CARDS_NO_MEDIUM = 24;
        public const int CARDS_NO_HARD = 48;
        #endregion
        #region Errors Limit
        public const int ERRORS_LIMIT_EASY = 10;
        public const int ERRORS_LIMIT_MEDIUM = 15;
        public const int ERRORS_LIMIT_HARD = 20;
        #endregion
    }
}
