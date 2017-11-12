using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DraughtsFramework
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
                                         { 0, 2, 0, 0, 0, 2, 0, 2 },
                                         { 0, 0, 0, 0, 2, 0, 0, 0 },
                                         { 0, 0, 0, 0, 0, 0, 0, 0 },
                                         { 1, 0, 1, 0, 1, 0, 1, 0 },
                                         { 0, 1, 0, 1, 0, 1, 0, 1 },
                                         { 1, 0, 1, 0, 0, 0, 1, 0 } };
        }

        public void PrintBoard()
        {
            int printCount = 1;
            int rowNum = 8;

            foreach (var item in piecePositions)
            {
                string piece = "--";

                switch (item)
                {
                    case 1:
                        piece = "WP";
                        break;
                    case 2:
                        piece = "BP";
                        break;
                    case 3:
                        piece = "WK";
                        break;
                    case 4:
                        piece = "BK";
                        break;                    
                }


                if (printCount == 1)
                {
                    Console.Write(rowNum + "| ");
                    rowNum--;
                }

                if (printCount % 8 == 0)
                {
                    Console.WriteLine(piece);
                    Console.WriteLine(" |");
                }
                else
                {
                    Console.Write(piece + " ");
                }

                if (printCount % 8 == 0 && printCount < 64)
                {
                    Console.Write(rowNum + "| ");
                    rowNum--;
                }

                printCount++;
            }

            Console.WriteLine("  ------------------------");
            Console.WriteLine("   A  B  C  D  E  F  G  H");
        }

        public bool MakeMove(Move move)
        {
            // Assign the piece being moved to the move
            move.Piece = piecePositions[move.XFrom, move.YFrom];

            if (Rules.IsWithinBoard(move) && Rules.IsPlayersPiece(move.Player, move.Piece) && Rules.IsMovingDiagonally(piecePositions, move) && Rules.IsEmptySpace(piecePositions, move))
            {
                // Check if piece should be switched to a king
                if (move.Piece == Pieces.White_Man && move.XTo == 0)
                {
                    piecePositions[move.XTo, move.YTo] = Pieces.White_King;
                    move.CreatedKing = true;
                }
                else if (move.Piece == Pieces.Black_Man && move.XTo == 7)
                {
                    piecePositions[move.XTo, move.YTo] = Pieces.Black_King;
                    move.CreatedKing = true;
                }
                else
                {
                    piecePositions[move.XTo, move.YTo] = move.Piece;
                }

                if (move.PieceTaken != 0)
                {
                    int xSpaceMovedOver = move.XTo > move.XFrom ? move.XFrom + 1 : move.XFrom - 1;
                    int ySpaceMovedOver = move.YTo > move.YFrom ? move.YFrom + 1 : move.YFrom - 1;
                    piecePositions[xSpaceMovedOver, ySpaceMovedOver] = 0;                    
                }

                piecePositions[move.XFrom, move.YFrom] = 0;
                
                return true;
            }

            return false;
        }

        public void UndoMove(Move move)
        {            
            piecePositions[move.XFrom, move.YFrom] = move.Piece;
            piecePositions[move.XTo, move.YTo] = Pieces.Empty;

            // If move involved capturing
            if (move.XTo == move.XFrom + 2 || move.XTo == move.XFrom - 2)
            {
                int xSpaceMovedOver = move.XTo > move.XFrom ? move.XFrom + 1 : move.XFrom - 1;
                int ySpaceMovedOver = move.YTo > move.YFrom ? move.YFrom + 1 : move.YFrom - 1;

                piecePositions[xSpaceMovedOver, ySpaceMovedOver] = move.PieceTaken;
            }
        }

        /// <summary>
        /// Converts user input into array positions and returns a move
        /// </summary>
        public Move GenerateMoveFromUserInput(int player, int xFrom, string yFrom, int xTo, string yTo)
        {
            int newXFrom = SwitchPositions(xFrom);
            int newYFrom = ((yFrom.ToLower().ToCharArray()[0])) - 97;  
            int newXTo = SwitchPositions(xTo);
            int newYTo = ((yTo.ToLower().ToCharArray()[0])) - 97;

            return new Move(player, newXFrom, newYFrom, newXTo, newYTo);

            int SwitchPositions(int x)
            {
                switch(x)
                {
                    case 1:
                        return 7;
                    case 2:
                        return 6;
                    case 3:
                        return 5;
                    case 4:
                        return 4;
                    case 5:
                        return 3;
                    case 6:
                        return 2;
                    case 7:
                        return 1;
                    case 8:
                        return 0;
                    default:
                        return 0;
                }
            }
        }

        public bool IsWinner()
        {
            bool noWhitePieces = !piecePositions.OfType<int>().Contains(Pieces.White_Man) && !piecePositions.OfType<int>().Contains(Pieces.White_King);
            bool noBlackPieces = !piecePositions.OfType<int>().Contains(Pieces.Black_Man) && !piecePositions.OfType<int>().Contains(Pieces.Black_King);

            return noWhitePieces || noBlackPieces;
        }
    }
}
