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

        public Board()
        {
            // A new board with all pieces in their original positions
            piecePositions = new int[,]{ { 0, 2, 0, 2, 0, 2, 0, 2 },
                                         { 2, 0, 2, 0, 2, 0, 2, 0 },
                                         { 0, 2, 0, 2, 0, 2, 0, 2 },
                                         { 0, 0, 2, 0, 0, 0, 0, 0 },
                                         { 0, 1, 0, 0, 0, 0, 0, 0 },
                                         { 0, 0, 1, 0, 1, 0, 1, 0 },
                                         { 0, 1, 0, 1, 0, 1, 0, 1 },
                                         { 1, 0, 0, 0, 1, 0, 1, 0 } };
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

                // Check if a piece has been taken and change it's positions value to empty
                if (move.PieceTaken != 0)
                {
                    int xSpaceMovedOver = move.XTo > move.XFrom ? move.XFrom + 1 : move.XFrom - 1;
                    int ySpaceMovedOver = move.YTo > move.YFrom ? move.YFrom + 1 : move.YFrom - 1;

                    piecePositions[xSpaceMovedOver, ySpaceMovedOver] = 0;                      
                }

                piecePositions[move.XFrom, move.YFrom] = 0;

                // Assign successive takes if there are any
                Rules.CanTakeAnotherPiece(piecePositions, move);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Undo a move
        /// </summary>
        /// <param name="move">Move to undo</param>
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
        /// Redo a move
        /// </summary>
        /// <param name="move"></param>
        public void RedoMove(Move move)
        {
            piecePositions[move.XFrom, move.YFrom] = Pieces.Empty;

            // Make piece king
            if (move.CreatedKing)
            {
                piecePositions[move.XTo, move.YTo] = move.Piece == Pieces.White_Man ? Pieces.White_King : Pieces.Black_King;
            }
            else
            {
                piecePositions[move.XTo, move.YTo] = move.Piece;
            }

            // If move involved capturing
            if (move.XTo == move.XFrom + 2 || move.XTo == move.XFrom - 2)
            {
                int xSpaceMovedOver = move.XTo > move.XFrom ? move.XFrom + 1 : move.XFrom - 1;
                int ySpaceMovedOver = move.YTo > move.YFrom ? move.YFrom + 1 : move.YFrom - 1;

                piecePositions[xSpaceMovedOver, ySpaceMovedOver] = Pieces.Empty;
            }            
        }

        /// <summary>
        /// Returns whether or not winning conditions have been met
        /// </summary>
        public bool IsWinner()
        {
            bool noWhitePieces = !piecePositions.OfType<int>().Contains(Pieces.White_Man) && !piecePositions.OfType<int>().Contains(Pieces.White_King);
            bool noBlackPieces = !piecePositions.OfType<int>().Contains(Pieces.Black_Man) && !piecePositions.OfType<int>().Contains(Pieces.Black_King);

            return noWhitePieces || noBlackPieces;
        }
    }
}
