﻿using Domino.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    class Program
    {
        static void Main(string[] args)
        {
            Game jogo = new Game();                       
            jogo.Play();

            Console.ReadKey();
        }
    }
}
