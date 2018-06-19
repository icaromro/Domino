using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Model
{
    public class Game
    {
        short MIN = 0, MAX = 6;
        public List<Piece> piecesOnTable;
        Piece endOne;
        Piece endTwo;
        short currentPlayer;
        bool draw;
        int drawCount;

        public List<Player> players = new List<Player>();

        public Game() 
        {
            StartGame();
            
        }

        private void CreateDeck()
        {
            for(int i = MIN; i <= MAX; i++)
            {
                for(int j = MIN; j<= MAX; j++)
                {
                    if(i <= j)
                    {
                        piecesOnTable.Add(new Piece(i, j));
                    }
                }
            }
        }

        private void DistributePieces()
        {            
            Random randomPiece = new Random();
            Player p;
            int pieceToGive; 
            for (int i = 0; i < 4; i++)
            {
                p = new Player();
                do
                {
                    pieceToGive = randomPiece.Next(piecesOnTable.Count);
                    Piece piece = piecesOnTable.ElementAt(pieceToGive);
                    if (piece.sideOne == 6 && piece.sideTwo == 6)
                    {
                        currentPlayer = Convert.ToInt16(i);
                    }
                    p.hand.Add(piece);
                    
                    piecesOnTable.RemoveAt(pieceToGive);
                } while (p.hand.Count < 7);

                players.Add(p);
            }
        }

        private void StartGame()
        {
            piecesOnTable = new List<Piece>();
            CreateDeck();
            endOne = endTwo = null;
            DistributePieces();
        }

        public void Play()
        {
            draw = false;
            drawCount = 0;
            while(currentPlayer < 4)
            {
                WriteMessage("Current game: " + ListPieces());
                WriteMessage(String.Format(@"Player 1: {0} pc(s) | Player 2: {1} pc(s) | Player 3: {2} pc(s) | Player 4: {3} pc(s)"
                    , players.ElementAt(0).hand.Count, players.ElementAt(1).hand.Count, players.ElementAt(2).hand.Count, players.ElementAt(3).hand.Count));

                Console.WriteLine();
                


                if (piecesOnTable.Count == 0)
                {

                    int currentPiece = 0;
                    foreach (Piece piece in players.ElementAt(currentPlayer).hand)
                    {
                        if (piece.sideTwo == 6 && piece.sideOne == 6)
                        {
                            piecesOnTable.Add(piece);
                            players.ElementAt(currentPlayer).hand.Remove(piece);
                            WriteMessage("Player " + (currentPlayer + 1) + " played " + piece.sideOne + "|" + piece.sideTwo);
                            break;
                        }
                        currentPiece++;
                    }
                }
                else
                {
                    if (currentPlayer == 50)//0
                    {
                        UserPlay();
                    }
                    else
                    {                       
                        AIPlay();                        
                    }

                }

                if (draw && drawCount == 3)
                    break;

                if (players.ElementAt(currentPlayer).hand.Count == 0)
                    break;

                if (currentPlayer == 3)
                    currentPlayer = 0;
                else
                {
                    currentPlayer++;                    
                }
            }

            if (draw && drawCount == 3)
                WriteMessage("Draw");//draw logic
            else
                WriteMessage("Winner -> Player " + (currentPlayer + 1));
            
        }

        private void UserPlay()
        {
            Piece first = piecesOnTable.First();
            Piece last = piecesOnTable.Last();

            int possiblePLayOne, possiblePLayTwo;
            if (first.sideOnePiece == null)
                possiblePLayOne = first.sideOne;
            else
                possiblePLayOne = first.sideTwo;
            if (last.sideOnePiece == null)
                possiblePLayTwo = last.sideOne;
            else
                possiblePLayTwo = last.sideTwo;

                        
            bool canPlay = false;
            foreach (Piece piece in players.ElementAt(currentPlayer).hand)
            {
                if (possiblePLayTwo == piece.sideOne || possiblePLayTwo == piece.sideTwo || possiblePLayOne == piece.sideOne || possiblePLayOne == piece.sideTwo)
                {
                    canPlay = true;
                    break;
                }                                
            }

            if (canPlay)
            {
                bool played = false;
                do
                {
                    WriteMessage("Your hand: " + players.ElementAt(currentPlayer).listHand());

                    Console.Write("Insert the position of the piece you want to play and press enter: ");
                    int input = 0;
                    do
                    {
                        input = Convert.ToInt32(Console.ReadLine());
                    } while (input > 0 && input <= players.ElementAt(currentPlayer).hand.Count);

                    int selectedPiecePos = input - 1;
                    Piece selectedPiece = players.ElementAt(currentPlayer).hand.ElementAt(selectedPiecePos);

                    
                    if(selectedPiece.sideOne == possiblePLayOne || selectedPiece.sideTwo == possiblePLayOne)                        
                    {
                                              
                        if (piecesOnTable.ElementAt(0).sideOnePiece == null)
                            piecesOnTable.ElementAt(0).sideOnePiece = selectedPiece;
                        else
                            piecesOnTable.ElementAt(0).sideTwoPiece = selectedPiece;

                        selectedPiece.sideOnePiece = piecesOnTable.ElementAt(0).sideOnePiece;

                        piecesOnTable.Insert(0, selectedPiece);
                        players.ElementAt(currentPlayer).hand.RemoveAt(selectedPiecePos);

                        WriteMessage("You played " + selectedPiece.sideOne + "|" + selectedPiece.sideTwo);
                        played = true;
                    }
                    else if (selectedPiece.sideTwo == possiblePLayOne)
                    {
                                           
                        if (piecesOnTable.ElementAt(0).sideOnePiece == null)
                            piecesOnTable.ElementAt(0).sideOnePiece = selectedPiece;
                        else
                            piecesOnTable.ElementAt(0).sideTwoPiece = selectedPiece;

                        selectedPiece.sideTwoPiece = piecesOnTable.ElementAt(0).sideOnePiece;
                        
                        piecesOnTable.Insert(0, selectedPiece);
                        players.ElementAt(currentPlayer).hand.RemoveAt(selectedPiecePos);

                        WriteMessage("You played " + selectedPiece.sideOne + "|" + selectedPiece.sideTwo);
                        played = true;
                    }
                    else if (selectedPiece.sideOne == possiblePLayTwo)
                    {
                        //colocar no final
                        if (piecesOnTable.Last().sideOnePiece == null)
                            piecesOnTable.Last().sideOnePiece = selectedPiece;
                        else
                            piecesOnTable.Last().sideTwoPiece = selectedPiece;

                        selectedPiece.sideOnePiece = piecesOnTable.Last();

                        piecesOnTable.Add(selectedPiece);
                        WriteMessage("You played " + selectedPiece.sideOne + "|" + selectedPiece.sideTwo);
                        played = true;
                    }
                    else if (selectedPiece.sideTwo == possiblePLayTwo)
                    {
                        if (piecesOnTable.Last().sideOnePiece == null)
                            piecesOnTable.Last().sideOnePiece = selectedPiece;
                        else
                            piecesOnTable.Last().sideTwoPiece = selectedPiece;

                        selectedPiece.sideTwoPiece = piecesOnTable.Last();

                        piecesOnTable.Add(selectedPiece);
                        WriteMessage("You played " + selectedPiece.sideOne + "|" + selectedPiece.sideTwo);
                        played = true;
                    }
                    else
                    {
                        WriteMessage("You can not play the selected piece");
                    }
                    
                } while (!played);
            }
            else
            {
                if (draw)
                    drawCount++;
                else
                    draw = true;
                WriteMessage("You can't play any piece so you missed your play");
            }
        }

        private void AIPlay()
        {
            Piece first = piecesOnTable.First();
            Piece last = piecesOnTable.Last();

            int possiblePLayOne, possiblePLayTwo;
            if (first.sideOnePiece == null)
                possiblePLayOne = first.sideOne;
            else
                possiblePLayOne = first.sideTwo;
            if (last.sideOnePiece == null)
                possiblePLayTwo = last.sideOne;
            else
                possiblePLayTwo = last.sideTwo;

            int currentPiece = 0, selectedPiecePos = -1; Piece possiblePiece = new Piece(); ;
            char side = ' ';
            foreach (Piece piece in players.ElementAt(currentPlayer).hand)
            {
                if(possiblePLayTwo == piece.sideOne || possiblePLayTwo == piece.sideTwo)
                {
                    if (selectedPiecePos >= 0)
                    {
                        possiblePiece = players.ElementAt(currentPlayer).hand.ElementAt(selectedPiecePos);
                        if ((piece.sideTwo + piece.sideOne) > (possiblePiece.sideOne + possiblePiece.sideTwo))
                        {
                            selectedPiecePos = currentPiece;
                            side = 'l';
                        }                        
                    }
                    else
                    {
                        selectedPiecePos = currentPiece;
                        side = 'l'; 
                    }
                    
                }
                if (possiblePLayOne == piece.sideOne || possiblePLayOne == piece.sideTwo)
                {
                    if (selectedPiecePos >= 0)
                    {
                        possiblePiece = players.ElementAt(currentPlayer).hand.ElementAt(selectedPiecePos);
                        if ((piece.sideTwo + piece.sideOne) > (possiblePiece.sideOne + possiblePiece.sideTwo))
                        {
                            selectedPiecePos = currentPiece;
                            side = 'f';
                        }                        
                    }
                    else
                    {
                        selectedPiecePos = currentPiece;
                        side = 'f';
                    }                    
                }

                currentPiece++;
            }

            if(selectedPiecePos > -1)
            {
                Piece selected = players.ElementAt(currentPlayer).hand.ElementAt(selectedPiecePos);
                if (side == 'l')
                {
                    if(piecesOnTable.Last().sideOne == selected.sideOne )
                    {
                        piecesOnTable.Last().sideOnePiece = selected;
                        selected.sideOnePiece = piecesOnTable.Last();
                    }
                    else if (piecesOnTable.Last().sideOne == selected.sideTwo)
                    {
                        piecesOnTable.Last().sideOnePiece = selected;
                        selected.sideTwoPiece = piecesOnTable.Last();
                    }
                    else if (piecesOnTable.Last().sideTwo == selected.sideOne) 
                    {
                        piecesOnTable.Last().sideTwoPiece = selected;
                        selected.sideOnePiece = piecesOnTable.Last();
                    }
                    else if (piecesOnTable.Last().sideTwo == selected.sideTwo) 
                    {
                        piecesOnTable.Last().sideTwoPiece = selected;
                        selected.sideTwoPiece = piecesOnTable.Last();
                    }
                    
                    piecesOnTable.Add(selected);
                }
                else
                {
                    int selectedSide;
                    if (piecesOnTable.ElementAt(0).sideOnePiece == null)
                    {
                        piecesOnTable.ElementAt(0).sideOnePiece = selected;
                        selectedSide = piecesOnTable.ElementAt(0).sideOne;
                    }
                    else
                    {
                        piecesOnTable.ElementAt(0).sideTwoPiece = selected;
                        selectedSide = piecesOnTable.ElementAt(0).sideTwo;
                    }
                    if (selectedSide == selected.sideOne)
                        selected.sideOnePiece = piecesOnTable.ElementAt(0);
                    else
                        selected.sideTwoPiece = piecesOnTable.ElementAt(0);
                    
                    piecesOnTable.Insert(0, selected);

                }

                WriteMessage("Player " + (currentPlayer + 1) + " played " + selected.sideOne + "|" + selected.sideTwo);                
                
                players.ElementAt(currentPlayer).hand.RemoveAt(selectedPiecePos);
                draw = false;
                drawCount = 0;
            }
            else
            {
                if (draw)
                    drawCount++;
                else
                    draw = true;
                WriteMessage("Player " + (currentPlayer + 1) + " missed his play.");                
            }
                        
        }

        private string ListPieces() 
        {
            string list = "";
            if (piecesOnTable.Count > 0)
            {
                foreach (Piece p in piecesOnTable)
                {
                    list += p.sideOne + "|" + p.sideTwo + " <-> ";
                }

                list = list.Substring(0, list.Length - 4);
            }
            return list;
        }

        private void WriteMessage(string msg)
        {
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.WriteLine();
        }
    }
}
