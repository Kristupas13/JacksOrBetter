// Kristupas Lunskas "JokerOrBetter"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JacksOrBetter
{
    class Program
    {


        static void Main(string[] args)
        {
            GameManager game = new GameManager(80,40);
            game.menu();
        }

    }
}
