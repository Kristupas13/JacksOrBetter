using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacksOrBetter
{
    class Card
    {
        public enum Rank { TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE };
        public enum Suit { DIAMONDS, HEARTS, CLUBS, SPADES };

        public Rank MyRank { get; set; }
        public Suit MySuit { get; set; }

        
    }
}
