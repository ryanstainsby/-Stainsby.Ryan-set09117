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

        public bool MakeMove(int player, int xFrom, int yFrom, int xTo, int yTo)
        {
            int pieceFrom = piecePositions[xFrom, xTo];

            if (Rules.IsMovingDiagonally(piecePositions, player, xFrom, yFrom, xTo, yTo) && Rules.IsEmptySpace(piecePositions, xTo, yTo))
            {
                piecePositions[xTo, yTo] = pieceFrom;
                piecePositions[xFrom, yFrom] = 0;                
                return true;
            }

            Console.WriteLine(errorMessage);                       
            return false;
        }       
    }
}
