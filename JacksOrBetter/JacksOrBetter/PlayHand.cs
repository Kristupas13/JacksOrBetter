using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacksOrBetter
{
    class PlayHand : CardDeck
    {
        private enum Prize_Coeff                                                          
        {
            ROYAL_FLUSH = 800, STRAIGHT_FLUSH = 50, FOUR_OF_A_KIND = 25, FULL_HOUSE = 9, FLUSH = 6,
            STRAIGHT = 4, THREE_OF_A_KIND = 3, TWO_PAIRS = 2, JACKS_OR_BETTER = 1, NONE = 0
        };

        private Card[] playerCards; 
        private Card[] sortedPlayerCards; // Sorted player cards to make evaluation easier
        private double playerCoins = 0;
        private double betAmount = 0;

        public PlayHand(ref double _coins)
        {
            playerCoins = _coins;
            playerCards = new Card[5];
            sortedPlayerCards = new Card[5];
        }
        public void deal()
        {
            setDeck(); // First, we set values to each card in the deck and shuffle it
            dealHand(); // Then, we deal cards to player
            showCards(); // We show player his cards
            discardCards(); // Player chooses if he wants to discard or keep them
            showCards(); // We show player his new(or same) cards
            sortHand(); // After player makes up his mind we sort his cards

            calculateReward(evaluateHand());


        }
        public void dealHand()
        {
            for(int i = 0; i < 5; i++)
            {
                playerCards[i] = getCardDeck[i]; // We set player hand. Deck is already shuffled
            }

            Console.Clear();
            Console.WriteLine("Coins: " + playerCoins);
            do
            {
                Console.WriteLine("Please enter bet amount: ");
            }
            while (!GameManager.checkValidInput(ref betAmount)); // We check if input is valid

            if (betAmount > playerCoins) // If player bets more than he has, bet is changed to player's current coins
                betAmount = playerCoins;

            playerCoins -= betAmount;


        }
        public void showCards()
        {
            Console.Clear();
            int x_pos = 0;
            int y_pos = 2;                          // We set default coordinates so our layout starts from top left corner
            Console.WriteLine("Coins: " + playerCoins);
            Console.WriteLine("Bet: " + betAmount);
            for(int i = 0; i < 5; i++)
            {
                PrintCard.setLayout(x_pos, y_pos); // Because setLayout is static, we can access it directly.
                PrintCard.setLayoutInfo(playerCards[i], i + 1); // Each time we send player card and it's index
                x_pos += 15;                                   // We change x coordinates to print next card and it's information to the left
            }
        }
        public void discardCards()
        {
            int[] splitHandHolderint;
            string[] splitHandHolder;
            bool wrongFormat = false;
            do
            {
                Console.WriteLine("\nEnter card(s) index(max 2) you would like to remove (or 0 to keep all): ");
                string HandHolder = Console.ReadLine();
                wrongFormat = false;
                splitHandHolder = HandHolder.Split(',');            // We check every possibility for input. 
                if (splitHandHolder.Length > 2)                     // If player wants to discard more than 2 cards wrongFormat will be set true
                    wrongFormat = true;
                else
                {
                    foreach (string number in splitHandHolder)      // We take a look at every substring to determine if format is correct
                    {
                        int temp;
                        if (int.TryParse(number, out temp))         // If it is a number
                        {
                            if (temp < 0 || temp > 5)               // We check if it is in range
                                wrongFormat = true;                 // If a single number is out of range, wrongFormat is changed to true
                        }
                        else wrongFormat = true;                    // If one of the symbol seperated by comma is not a number, wrongFormat is changed to true
                    }
                }
                if (wrongFormat) Console.WriteLine("Wrong format. Ex: 1  1,2  0");
            }
            while (wrongFormat);


            splitHandHolderint = Array.ConvertAll(splitHandHolder, int.Parse); // We convert everything to int

            splitHandHolderint = splitHandHolderint.Where(val => val != 0).ToArray(); // We remove if there was a zero ex:(0,1), (4,0)

            if (splitHandHolderint.All(x => x >= 0) && splitHandHolderint.All(x => x <= 5)) // We check if input is in valid range. Repeating same number twice does the same thing as typing it once.
            {
                foreach (var index in splitHandHolderint)
                {                                                // Elements in array are one unit smaller than input index
                    playerCards[index-1] = getCardDeck[index+4]; // Because we already called shuffleDeck, the rest of array is shuffled
                }                                                // We can pick any card from the rest of the deck
            }
        }
        public void sortHand()
        {
             sortedPlayerCards = playerCards                    // We sort player hand and put it in sortedPlayerCards array
            .OrderByDescending(x => (int)(x.MyRank))
            .ToArray();
        }
        public int evaluateHand()
        {
            Dictionary<Card.Rank, int> numByRanks = sortedPlayerCards.GroupBy(x => x.MyRank)    // We use dictionary to store ranks and how many times they repeated
                                                .ToDictionary(k => k.Key, v => v.Count());      // to evaluate four of a kind, three of a kind 

            int diff_type_counter = sortedPlayerCards.Select(p => p.MyRank).Distinct().Count(); // We also store the number of different ranks in hand
                                                                                                // to evaluate full house, two pair
            if (Royal_Flush())
                return (int)Prize_Coeff.ROYAL_FLUSH;

            else if (Straight_Flush())
                return (int)Prize_Coeff.STRAIGHT_FLUSH;

            else if (Four_Of_A_Kind(numByRanks))
                return (int)Prize_Coeff.FOUR_OF_A_KIND;

            else if (Full_House(diff_type_counter))
                return (int)Prize_Coeff.FULL_HOUSE;

            else if (Flush())
                return (int)Prize_Coeff.FLUSH;

            else if (Straight())
                return (int)Prize_Coeff.STRAIGHT;

            else if (Three_Of_A_Kind(numByRanks))
                return (int)Prize_Coeff.THREE_OF_A_KIND;

            else if (Two_Pairs(diff_type_counter))
                return (int)Prize_Coeff.TWO_PAIRS;

            else if (Jacks_Or_Better(numByRanks))
                return (int)Prize_Coeff.JACKS_OR_BETTER;

            else
                return (int)Prize_Coeff.NONE;
        }


        // EVALUATION //
        public bool Royal_Flush()
        {
            if(Straight_Flush())                                    // First, we take a look if we have both Straight and Flush
            {
                if (sortedPlayerCards[0].MyRank == Rank.ACE)        // If we do, then we take a look at the highest card value
                    return true;                                  
            }
            return false;
        }
        public bool Straight_Flush()
        {
            if (Straight() && Flush())                              // If it's not royal flush and condition's result is true, then it's a straight flush              
                return true;
            return false;
        }
        public bool Four_Of_A_Kind(Dictionary<Card.Rank, int> _counts)
        {

            if (_counts.Values.Max() == 4)                          // If we find a rank which has a value of 4 (how many times it was found in players hand)
                return true;                                        // it means we only have 2 different type of cards and one of them has a value of 4
            return false;
        }
        public bool Full_House(int _diff_type_counter)
        {
            if (_diff_type_counter == 2)                            // If it only has 2 different type of ranks in hand and it's not four of a kind
                return true;                                        // that means he has three cards of same rank and the other two's are also equal
            return false;                                           // ex: Ranks 8,8,8,8,7 - it has 2 different ranks, but wont work, because four of a kind was called first
        }                                                           // ex: Ranks 8,8,8,7,7 - it has 2 different ranks, it will work, because there are no other possibilities left
        public bool Flush()
        {
            if (sortedPlayerCards.Select(p => p.MySuit).Distinct().Count() == 1)    // If we find that there is only one distinct suit in our hand,
                return true;                                                        // it means that all of our cards are the same suit = > We have a flush.
            return false;
        }
        public bool Straight()
        {
            for (int i = 0; i < sortedPlayerCards.Length-1; i++)                    // We go through our hand
            {
                if(sortedPlayerCards[i].MyRank - 1 != sortedPlayerCards[i+1].MyRank)  // Because we have sorted array, we can compare if our current card's rank lowered by one is equal to next card's rank
                {
                    return false;
                }
            }
                return true;
        }
        public bool Three_Of_A_Kind(Dictionary<Card.Rank, int> _counts)
        {
            if (_counts.Values.Max() == 3)                          // If we find a rank which has a value of 3 (how many times it was found in players hand)
                return true;                                        // it means we found a three same rank cards and the other two cannot be the same
            return false;                                           // ex: Ranks 8,8,8,2,2 - it has a maximum value of 3(three of eights), but wont work, because we have called full house earlier
        }                                                           // ex: Ranks 8,8,8,2,3 - it has a maximum value of 3(three of eights), if statement will be true.
        public bool Two_Pairs(int _diff_type_counter)
        {
            if (_diff_type_counter == 3)                            // If it only has 3 different type of ranks in hand and it's not three of a kind
                return true;                                        // that means player has two pairs of same rank and the last card's rank is different
            return false;                                           // ex: Ranks 8,8,8,7,6 - it has 3 different ranks, but wont work, because three of a kind was called first
                                                                    // ex: Ranks 8,8,7,7,6 - it has 3 different ranks, it will work, if statement will be true
        }
        public bool Jacks_Or_Better(Dictionary<Card.Rank, int> _counts)
        {
            var Rank = from hand in _counts                         // We check if we have any rank has a match.
                       where hand.Value == 2                        
                       select hand.Key;

            if (Rank.Any(z => (int)z >= 9))                         // If we find that card, we have to check for it's rank
                return true;


            return false;
        }
        // END OF EVALUATION //

        public void calculateReward(int coefficient)
        {

            playerCoins = playerCoins + betAmount * coefficient;    
            if (coefficient > 0)
                Console.WriteLine("\nCongratulations. You won {1}x: {0} coins", betAmount * coefficient, coefficient);
            else Console.WriteLine("\nSorry. You lost: " + betAmount);

            Console.SetCursorPosition(0, 0);           // We overwrite player coin line
            Console.WriteLine("Coins: " + playerCoins);
            Console.SetCursorPosition(0, 15);          // We set our cursor position back

            if (playerCoins == 0)                                                        // Game over
            {
                Console.WriteLine("Sorry, you lost. Better luck next time.");                

            }
        }
        public void setPlayerCoins(double _coins)
        {
            playerCoins = _coins;
        }
        public double getPlayerCoins()
        {
            return playerCoins;
        }
    
    }
}
