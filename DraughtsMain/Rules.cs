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
            int piece = board[xFrom, yFrom];
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
            bool isWhiteKing = board[xFrom, yFrom] == Pieces.White_King;
            bool isBlackKing = board[xFrom, yFrom] == Pieces.Black_King;
            bool whiteForward = (xTo == xFrom - 1 || (isWhiteKing && xTo == xFrom + 1)) && (yTo == yFrom + 1 || yTo == yFrom - 1);
            bool whiteTaking = (xTo == xFrom - 2 || (isWhiteKing && xTo == xFrom + 2)) && (yTo == yFrom + 2 || yTo == yFrom - 2);
            bool blackForward = (xTo == xFrom + 1 || (isBlackKing && xTo == xFrom - 1)) && (yTo == yFrom + 1 || yTo == yFrom - 1);
            bool blackTaking = (xTo == xFrom + 2 || (isBlackKing && xTo == xFrom - 2)) && (yTo == yFrom + 2 || yTo == yFrom - 2);

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
    }
}
