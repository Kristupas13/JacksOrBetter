using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacksOrBetter
{
    class GameManager
    {
        private double player_Coins;
        private int inputNumber;

        public GameManager(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.BufferHeight = height;
            Console.BufferWidth = width;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "JacksOrBetter";
        }
        public void menu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to JacksOrBetter poker game.\n");
                Console.WriteLine("Please type the number what you want to do next: ");                        // Introduction
                Console.WriteLine("1) Play");
                Console.WriteLine("2) See prize rewards");
                Console.WriteLine("3) Exit");
            }
            while (!(int.TryParse(Console.ReadLine(), out inputNumber) && inputNumber >= 1 && inputNumber <= 3)); // We see if input is valid
           
            switch(inputNumber)
            {
                case 1:
                    play();
                    break;
                case 2:
                    showPrizeReward();
                    if (askYes_or_No("\nDo you want to play a game? (Y/N) Type ? to see prize rewards again."))  // After showing prize rewards, we call function to determine if player wants to play the game
                        play();
                    else break;
                    break;
                case 3:
                    break;
            }

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
            
        }


        public void play()
        {



            PlayHand hand = new PlayHand(ref player_Coins);
            do
            {
                if (player_Coins == 0.0)
                {
                    setCoins();
                    hand.setPlayerCoins(player_Coins);                              
                }
                hand.deal();
                player_Coins = hand.getPlayerCoins();
            }
            while (GameManager.askYes_or_No("\nPlay again? (Y/N) Press ? to see prize rewards")); // Main game loop. 


        }
        public void setCoins()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("\nPlease enter the amount of coins you are going to play with (more than 0):");
            }
            while (!checkValidInput(ref player_Coins));                                         // We do the loop until the input is valid

        }

        public static bool checkValidInput(ref double _coins)                  // First validation
        {

            string c = Console.ReadLine();
            if (double.TryParse(c, out _coins) && Convert.ToDouble(c) > 0)      
            {
                return true;
            }
            else Console.WriteLine("Number is not valid.");                
            return false;
        }

        public static bool askYes_or_No(string output_line)                     // Second validation
        {
            while (true)
            {
                Console.WriteLine(output_line);
                ConsoleKeyInfo enteredKey = Console.ReadKey();
                if (enteredKey.Key == ConsoleKey.Y)
                    return true;
                else if (enteredKey.Key == ConsoleKey.N)
                    return false;
                else if (enteredKey.KeyChar == '?')
                    showPrizeReward();

            }
        }
        public static void showPrizeReward()                                    // Function to display prize rewards to player
        {
            string[] hands = {"Royal flush", "Straight flush", "Four of a kind", "Full house",
                         "Flush", "Straight", "Three of a kind", "Two pair", "Jacks or better", "None"};

            int[] prize = { 800, 50, 25, 9, 6, 4, 3, 2, 1, 0 };


            Console.WriteLine("\n{0,-20} {1,5}\n", "Hand", "Prize");
            for (int ctr = 0; ctr < hands.Length; ctr++)
                Console.WriteLine("{0,-20} {1,5}", hands[ctr], prize[ctr]);

            Console.WriteLine("\nPress any key to go back");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
