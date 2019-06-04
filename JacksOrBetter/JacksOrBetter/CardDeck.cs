using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacksOrBetter
{
    class CardDeck : Card
    {
        const int NUM_CARDS = 52;
        private Card[] deck; 


        public CardDeck()
        {
            deck = new Card[NUM_CARDS]; // create a deck with defined number of cards
        }

        public void setDeck()
        {
            int index = 0;
            foreach(Suit s in (Suit[]) Enum.GetValues(typeof(Suit))) // take every suit
            {
                foreach(Rank r in (Rank[]) Enum.GetValues(typeof(Rank))) // and every rank to combine a unique card
                {
                    deck[index] = new Card{MyRank = r,  MySuit = s}; // assign values to each card in the deck
                    index++;
                }
            }
            shuffleDeck();
        }
        public void shuffleDeck()
        {
            Random r = new Random();
            for (int i = deck.Length - 1; i > 0; --i)     //  http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
            {
                int nextCardID = r.Next(i + 1);
                Card temp = deck[i];
                deck[i] = deck[nextCardID];
                deck[nextCardID] = temp;
            }
       
        }

        public Card[] getCardDeck { get { return deck;} } // To get current deck of cards
    }
}
