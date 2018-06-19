using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Model
{
    public class Piece
    {
        public int sideOne { get; set; }
        public int sideTwo { get; set; }

        public Piece sideOnePiece { get; set; }
        public Piece sideTwoPiece { get; set; }

        public Piece(int s1, int s2)
        {
            sideOne = s1;
            sideTwo = s2;
            sideOnePiece = sideTwoPiece = null;
        }

        public Piece() { }
    }
}
