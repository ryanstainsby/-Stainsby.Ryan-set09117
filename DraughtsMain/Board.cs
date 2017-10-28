using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsGame
{
    public class Board
    {    
        // Represents all the positions on the board
        public int[,] piecePositions = new int[8,8];
        string errorMessage = null;

        public Board()
        {
            // A new board with all pieces in their original positions
            piecePositions = new int[,]{ { 0, 2, 0, 2, 0, 2, 0, 2 },
                                         { 2, 0, 2, 0, 2, 0, 2, 0 },
                                         { 0, 2, 0, 2, 0, 2, 0, 2 },
                                         { 0, 0, 0, 0, 0, 0, 0, 0 },
                                         { 0, 0, 0, 0, 0, 0, 0, 0 },
                                         { 1, 0, 1, 0, 1, 0, 1, 0 },
                                         { 0, 1, 0, 1, 0, 1, 0, 1 },
                                         { 1, 0, 1, 0, 1, 0, 1, 0 } };
        }

        public void PrintBoard()
        {
            int printCount = 1;
            int rowNum = 1;

            foreach (var item in piecePositions)
            {
                if (printCount == 1)
                {
                    Console.Write(rowNum + "| ");
                    rowNum++;
                }

                if (printCount % 8 == 0)
                {
                    Console.WriteLine(item.ToString());
                }
                else
                {
                    Console.Write(item.ToString() + " ");
                }

                if (printCount % 8 == 0 && printCount < 64)
                {
                    Console.Write(rowNum + "| ");
                    rowNum++;
                }

                printCount++;
            }

            Console.WriteLine("  ----------------");
            Console.WriteLine("   1 2 3 4 5 6 7 8");
        }

        public bool MakeMove(int player, Move move)
        {
            // Assign the piece being moved to the move
            move.Piece = piecePositions[move.XFrom, move.YFrom];

            if (Rules.IsPlayersPiece(piecePositions, player, move.Piece) && Rules.IsMovingDiagonally(piecePositions, player, move) && Rules.IsEmptySpace(piecePositions, move))
            {
                // Check if piece should be switched to a king
                if (move.Piece == Pieces.White_Man && move.XTo == 0)
                {
                    piecePositions[move.XTo, move.YTo] = Pieces.White_King;
                }
                else if (move.Piece == Pieces.Black_Man && move.XTo == 7)
                {
                    piecePositions[move.XTo, move.YTo] = Pieces.Black_King;
                }
                else
                {
                    piecePositions[move.XTo, move.YTo] = move.Piece;
                }

                piecePositions[move.XFrom, move.YFrom] = 0;           
                
                return true;
            }

            Console.WriteLine(errorMessage);                       
            return false;
        }       

        public void UndoMove(Move move)
        {
            piecePositions[move.XFrom, move.YFrom] = move.Piece;
            piecePositions[move.XTo, move.YTo] = move.PieceTaken; // Will default to 0 if no piece is taken
        }
    }
}
