using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsGame
{
    public static class Rules
    {
        // Players piece
        public static bool IsPlayersPiece(int[,] board, int player, int xFrom, int yFrom)
        {
            int piece = board[yFrom, xFrom];
            bool playerOnes = piece == Pieces.White_Man || piece == Pieces.White_King;
            bool playerTwos = piece == Pieces.Black_Man || piece == Pieces.Black_King;

            if ((player == Pieces.Player_1 && playerOnes) || (player == Pieces.Player_2 && playerTwos))
            {
                return true;
            }

            return false;
        }


        // Moving diagonally
        public static bool IsMovingDiagonally(int[,] board, int player, int xFrom, int yFrom, int xTo, int yTo)
        {
            bool whiteForward = xTo == xFrom - 1 && (yTo == yFrom + 1 || yTo == yFrom - 1);
            bool whiteTaking = xTo == xFrom - 2 && (yTo == yFrom + 2 || yTo == yFrom - 2);
            bool blackForward = xTo == xFrom + 1 && (yTo == yFrom + 1 || yTo == yFrom - 1);
            bool blackTaking = xTo == xFrom + 2 && (yTo == yFrom + 2 || yTo == yFrom - 2);

            if (player == Pieces.Player_1)
            {
                return whiteForward || (whiteTaking && IsCapturingOpponent(Pieces.Player_2));
            }
            else // Must be player 2
            {
                return blackForward || (blackTaking && IsCapturingOpponent(Pieces.Player_1));
            }

            bool IsCapturingOpponent(int opponent)
            {
                int xSpaceMovedOver = player == 1 ? xFrom - 1 : xFrom + 1;
                int ySpaceMovedOver = yTo > yFrom ? yFrom + 1 : yFrom - 1;

                if (!IsPlayersPiece(board, opponent, xSpaceMovedOver, ySpaceMovedOver))
                {
                    return true;
                }

                return false;
            }
        }

        // Space is empty
        public static bool IsEmptySpace(int[,] board, int xTo, int yTo) => board[xTo, yTo] == Pieces.Empty;

        private static bool LegalCapture(int[,] board, int player, int columnFrom, int rowFrom, int columnTo, int rowTo)
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

            if (columnToCheck != -1 && rowToCheck != -1 && (board[rowToCheck, columnToCheck] != 0 || !BelongsToPlayer(board[rowToCheck, columnToCheck], player == 1 ? 2 : 1)))
            {
                return true;
            }

            //errorMessage = "That is not a legal capture";
            return false;
        }

        private static bool BelongsToPlayer(int piece, int player)
        {
            if (player == 1 && (piece == 1 || piece == 3))
            {
                return true;
            }
            else if (player == 1 && (piece == 1 || piece == 3))
            {
                return true;
            }

            //errorMessage = "Piece at that position does not belong to the player";
            return false;
        }
    }
}
