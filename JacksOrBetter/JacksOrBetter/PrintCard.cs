using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacksOrBetter
{
    class PrintCard : Card
    {
        static int start_x = 0;
        static int start_y = 0;

        public static void setLayout(int xcoord, int ycoord)
        {
            int height = 12;

            start_x = xcoord;
            start_y = ycoord;

                Console.SetCursorPosition(start_x, start_y);
                Console.WriteLine(" ____________\n");
                for (int line = 0; line < height; line++)
                {
                    Console.SetCursorPosition(start_x, start_y + 1 + line);
                    if (line == height-1)
                        Console.WriteLine("|____________|\n");
                    else Console.Write("|            |\n");
                }
        }
        public static void setLayoutInfo(Card card, int cardIndex)
        {

            Console.SetCursorPosition(start_x + 1, start_y + 2);
            Console.Write(card.MyRank);
            Console.SetCursorPosition(start_x + 1, start_y + 6);
            Console.Write("OF");
            Console.SetCursorPosition(start_x + 1, start_y + 9);
            Console.Write(card.MySuit);
            Console.SetCursorPosition(start_x + 1, start_y + 12);
            Console.Write("(" + cardIndex + ")");
        }
    }
}
