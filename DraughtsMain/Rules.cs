using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsGame
{
    public static class Rules
    {
        // Players piece
        public static bool IsPlayersPiece(int[,] board, int player, int piece)
        {
            bool playerOnes = piece == Pieces.White_Man || piece == Pieces.White_King;
            bool playerTwos = piece == Pieces.Black_Man || piece == Pieces.Black_King;

            if ((player == Pieces.Player_1 && playerOnes) || (player == Pieces.Player_2 && playerTwos))
            {
                return true;
            }

            return false;
        }

        // Moving diagonally
        public static bool IsMovingDiagonally(int[,] board, int player, Move move)
        {
            bool isWhiteKing = move.Piece == Pieces.White_King;
            bool isBlackKing = move.Piece == Pieces.Black_King;
            bool whiteForward = (move.XTo == move.XFrom - 1 || (isWhiteKing && move.XTo == move.XFrom + 1)) && (move.YTo == move.YFrom + 1 || move.YTo == move.YFrom - 1);
            bool whiteTaking = (move.XTo == move.XFrom - 2 || (isWhiteKing && move.XTo == move.XFrom + 2)) && (move.YTo == move.YFrom + 2 || move.YTo == move.YFrom - 2);
            bool blackForward = (move.XTo == move.XFrom + 1 || (isBlackKing && move.XTo == move.XFrom - 1)) && (move.YTo == move.YFrom + 1 || move.YTo == move.YFrom - 1);
            bool blackTaking = (move.XTo == move.XFrom + 2 || (isBlackKing && move.XTo == move.XFrom - 2)) && (move.YTo == move.YFrom + 2 || move.YTo == move.YFrom - 2);

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
                int xSpaceMovedOver = player == 1 ? move.XFrom - 1 : move.XFrom + 1;
                int ySpaceMovedOver = move.YTo > move.YFrom ? move.YFrom + 1 : move.YFrom - 1;
                int opponentsPiece = board[xSpaceMovedOver, ySpaceMovedOver];

                if (!IsPlayersPiece(board, opponent, opponentsPiece))
                {
                    move.PieceTaken = opponentsPiece;
                    return true;
                }

                return false;
            }
        }

        // Space is empty
        public static bool IsEmptySpace(int[,] board, Move move) => board[move.XTo, move.YTo] == Pieces.Empty;
    }
}
