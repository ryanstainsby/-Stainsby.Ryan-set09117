using System.Collections.Generic;

namespace DraughtsFramework
{
    public static class Rules
    {
        /// <summary>
        /// Checks if the piece being moved is a players piece
        /// </summary>
        /// <param name="player">Player to check</param>
        /// <param name="piece">Piece to check</param>
        /// <returns></returns>
        public static bool IsPlayersPiece(int player, int piece)
        {
            bool playerOnes = piece == Pieces.White_Man || piece == Pieces.White_King;
            bool playerTwos = piece == Pieces.Black_Man || piece == Pieces.Black_King;

            if ((player == 1 && playerOnes) || (player == 2 && playerTwos))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the piece is moving diagonally
        /// </summary>
        /// <param name="board">Board positions to check against</param>
        /// <param name="move">Move to check</param>
        /// <returns></returns>
        public static bool IsMovingDiagonally(int[,] board, Move move)
        {
            bool isWhiteKing = move.Piece == Pieces.White_King;
            bool isBlackKing = move.Piece == Pieces.Black_King;
            bool whiteForward = (move.XTo == move.XFrom - 1 || (isWhiteKing && move.XTo == move.XFrom + 1)) && (move.YTo == move.YFrom + 1 || move.YTo == move.YFrom - 1);
            bool whiteTaking = (move.XTo == move.XFrom - 2 || (isWhiteKing && move.XTo == move.XFrom + 2)) && (move.YTo == move.YFrom + 2 || move.YTo == move.YFrom - 2);
            bool blackForward = (move.XTo == move.XFrom + 1 || (isBlackKing && move.XTo == move.XFrom - 1)) && (move.YTo == move.YFrom + 1 || move.YTo == move.YFrom - 1);
            bool blackTaking = (move.XTo == move.XFrom + 2 || (isBlackKing && move.XTo == move.XFrom - 2)) && (move.YTo == move.YFrom + 2 || move.YTo == move.YFrom - 2);

            if (move.Player == 1)
            {
                return whiteForward || (whiteTaking && IsCapturingOpponent(2));
            }
            else // Must be player 2
            {
                return blackForward || (blackTaking && IsCapturingOpponent(1));
            }

            bool IsCapturingOpponent(int opponent)
            {
                int xSpaceMovedOver = move.XTo > move.XFrom ? move.XFrom + 1 : move.XFrom - 1;
                int ySpaceMovedOver = move.YTo > move.YFrom ? move.YFrom + 1 : move.YFrom - 1;
                int opponentsPiece = board[xSpaceMovedOver, ySpaceMovedOver];

                if (IsPlayersPiece(opponent, opponentsPiece))
                {
                    move.PieceTaken = opponentsPiece;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if the move being made is within the boundries of the board
        /// </summary>
        /// <param name="move">Move to check</param>
        /// <returns></returns>
        public static bool IsWithinBoard(Move move)
        {
            return IsWithinBoard(move.XTo, move.YTo);
        }

        /// <summary>
        /// Checks if the move being made is within the boundries of the board
        /// </summary>
        /// <param name="xTo">X Position to check</param>
        /// <param name="yTo">Y Position to check</param>
        /// <returns></returns>
        private static bool IsWithinBoard(int xTo, int yTo)
        {
            return (xTo < 8 && xTo >= 0 && yTo < 8 && yTo >= 0);
        }

        /// <summary>
        /// Checks if the piece being moved can make another successive take and adds those moves to it's successive moves list
        /// </summary>
        /// <param name="board">Board to check against</param>
        /// <param name="move">Move to check</param>
        /// <returns></returns>
        public static bool CanTakeAnotherPiece(int[,] board, Move move)
        {
            List<Move> nextMoves = new List<Move>();
            
            if (move.PieceTaken != 0)
            {
                CheckAndAddLeftMoves();
                CheckAndAddRightMoves();
            }

            if (nextMoves.Count > 0)
            {
                move.SuccessiveMoves = nextMoves;
                return true;
            }
                        
            return false;


            void CheckAndAddLeftMoves()
            {
                int newYTo = move.YTo - 2;

                if (move.Piece == Pieces.White_Man || move.Piece == Pieces.Black_King || move.Piece == Pieces.White_King || move.CreatedKing)
                {
                    int newXTo = move.XTo - 2;

                    if (IsWithinBoard(newXTo, newYTo) && IsEmptySpace(board, newXTo, newYTo) && IsPlayersPiece(move.Player == 1 ? 2 : 1, board[newXTo + 1, newYTo + 1]))
                    {
                        nextMoves.Add(new Move(move.Player, move.XTo, move.YTo, newXTo, newYTo));
                    }
                }

                if (move.Piece == Pieces.Black_Man || move.Piece == Pieces.Black_King || move.Piece == Pieces.White_King || move.CreatedKing)
                {
                    int newXTo = move.XTo + 2;

                    if (IsWithinBoard(newXTo, newYTo) && IsEmptySpace(board, newXTo, newYTo) && IsPlayersPiece(move.Player == 1 ? 2 : 1, board[newXTo - 1, newYTo + 1]))
                    {
                        nextMoves.Add(new Move(move.Player, move.XTo, move.YTo, newXTo, newYTo));
                    }
                }
            }

            void CheckAndAddRightMoves()
            {
                int newYTo = move.YTo + 2;

                if (move.Piece == Pieces.White_Man || move.Piece == Pieces.Black_King || move.Piece == Pieces.White_King)
                {
                    int newXTo = move.XTo - 2;

                    if (IsWithinBoard(newXTo, newYTo) && IsEmptySpace(board, newXTo, newYTo) && IsPlayersPiece(move.Player == 1 ? 2 : 1, board[newXTo + 1, newYTo - 1]))
                    {
                        nextMoves.Add(new Move(move.Player, move.XTo, move.YTo, newXTo, newYTo));
                    }
                }

                if (move.Piece == Pieces.Black_Man || move.Piece == Pieces.Black_King || move.Piece == Pieces.White_King)
                {
                    int newXTo = move.XTo + 2;

                    if (IsWithinBoard(newXTo, newYTo) && IsEmptySpace(board, newXTo, newYTo) && IsPlayersPiece(move.Player == 1 ? 2 : 1, board[newXTo - 1, newYTo - 1]))
                    {
                        nextMoves.Add(new Move(move.Player, move.XTo, move.YTo, newXTo, newYTo));
                    }
                }
            }
        }

        /// <summary>
        /// Checks that the space being moved to is empty
        /// </summary>
        /// <param name="board">Board to check against</param>
        /// <param name="move">Move to check against</param>
        /// <returns></returns>
        public static bool IsEmptySpace(int[,] board, Move move) => IsEmptySpace(board, move.XTo, move.YTo);


        /// <summary>
        /// Checks that the space being moved to is empty
        /// </summary>
        /// <param name="board">Board to check against</param>
        /// <param name="xTo">X position to check against</param>
        /// <param name="yTo">Y position to check against</param>
        /// <returns></returns>
        private static bool IsEmptySpace(int[,] board, int xTo, int yTo) => board[xTo, yTo] == Pieces.Empty;
    }
}
