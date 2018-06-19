using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Model
{
    public class Player
    {
        public List<Piece> hand;//max 7

        public Player()
        {
            hand = new List<Piece>();
        }

        public string listHand()
        {
            string handList = "";
            foreach(Piece piece in hand)
            {

                handList += piece.sideOne + "|" + piece.sideTwo + ", ";
            }
            handList = handList.Substring(0, handList.Length - 2);

            return handList;
        }
    }
}
