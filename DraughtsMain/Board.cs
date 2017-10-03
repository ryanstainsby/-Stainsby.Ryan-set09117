using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsGame
{
    public class Board
    {    
        // Represents all the positions on the board
        int[,] piecePositions = new int[8,8];
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

        public bool MakeMove(int player, int columnFrom, int rowFrom, int columnTo, int rowTo)
        {
            int piece = piecePositions[columnFrom, rowFrom];

            if (MovedDiagonal(player, columnFrom, rowFrom, columnTo, rowTo) && EmptySpace(columnTo, rowTo))
            {
                piecePositions[rowTo, columnTo] = piecePositions[rowFrom, columnFrom];
                piecePositions[rowFrom, columnFrom] = 0;
            }
                       
            return false;
        }

        private bool MovedDiagonal(int player, int columnFrom, int rowFrom, int columnTo, int rowTo)
        {
            bool normalMove = (rowTo == rowFrom + 1 || rowTo == rowFrom - 1) && (columnTo == columnFrom + 1 || columnTo == columnFrom - 1);
            bool takingPiece = (rowTo == rowFrom + 2 || rowTo == rowFrom - 2) && (columnTo == columnFrom + 2 || columnTo == columnFrom - 2);

            if ((normalMove || (takingPiece && LegalCapture(player, columnFrom, rowFrom, columnTo, rowTo))))
            {
                return true;
            }

            if (errorMessage == null)
            {
                errorMessage = "Movement was not diagonal";
            }

            return false;
        }

        private bool EmptySpace(int columnTo, int rowTo)
        {
            var test = piecePositions[rowTo, columnTo];
            return ( test == 0);    
        }

        private bool LegalCapture(int player, int columnFrom, int rowFrom, int columnTo, int rowTo)
        {
            int rowToCheck = -1;
            int columnToCheck = -1;
            
            if (columnTo > columnFrom)
            {
                columnToCheck = columnFrom + 1;
            }
            else if (columnTo < columnFrom)
            {
                columnToCheck = columnFrom - 1;
            }

            if (rowTo > rowFrom)
            {
                rowToCheck = rowFrom + 1;
            }
            else if (rowTo < rowFrom)
            {
                rowToCheck = rowFrom - 1;
            }

            if (columnToCheck != -1 && rowToCheck != -1 && (piecePositions[rowToCheck, columnToCheck] != 0 || !BelongsToPlayer(piecePositions[rowToCheck, columnToCheck], player == 1 ? 2 : 1)))
            {
                return true;
            }

            errorMessage = "That is not a legal capture";
            return false;
        }

        private bool BelongsToPlayer(int piece, int player)
        {
            if (player == 1 && (piece == 1 || piece == 3))
            {
                return true;
            }
            else if (player == 1 && (piece == 1 || piece == 3))
            {
                return true;
            }

            errorMessage = "Piece at that position does not belong to the player";
            return false;
        }
    }
}
