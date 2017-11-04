using System;
using System.Collections.Generic;
using System.Text;
 
namespace DraughtsGame
{
    class DraughtsAI
    {
        int? originalPlayer = null;

        //public Move MakeMove(Board board, MoveNode moveTree, int player, int depth)
        //{
        //    return MoveTree(board, null, player, depth).BestMove.Move;
        //}

        public MoveNode MoveTree(Board board, MoveNode prevMove, int player, int depth)
        {
            MoveNode currentPosition = prevMove ?? new MoveNode(board, prevMove?.Move ?? null, null);

            if (prevMove != null && prevMove.Value != 0)
            {

            }
            if (originalPlayer == null) originalPlayer = player;

            if (depth != 0)
            {
                var allMoves = FindAllLegalMoves(currentPosition, player);
                foreach (var move in allMoves)
                {
                    MoveNode possibleMove = MoveTree(move.Board, move, player == 1 ? 2 : 1, depth - 1);
                    possibleMove.Parent = currentPosition;                    
                    currentPosition.AddChild(possibleMove);
                }
            }

            return currentPosition;
        }

        private List<MoveNode> FindAllLegalMoves(MoveNode parentNode, int player)
        {
            var moves = new List<MoveNode>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int piece = parentNode.Board.piecePositions[x, y];

                    if (player == Pieces.Player_1 && (piece == Pieces.White_Man || piece == Pieces.White_King))
                    {
                        CheckAndAddMove(x, y, x - 1, y + 1);
                        CheckAndAddMove(x, y, x - 1, y - 1);
                        CheckAndAddMove(x, y, x - 2, y + 2);
                        CheckAndAddMove(x, y, x - 2, y - 2);

                        if (piece == 3)
                        {
                            CheckAndAddMove(x, y, x + 1, y + 1);
                            CheckAndAddMove(x, y, x + 1, y - 1);
                            CheckAndAddMove(x, y, x + 2, y + 2);
                            CheckAndAddMove(x, y, x + 2, y - 2);
                        }
                    }
                    else if (player == Pieces.Player_2 && (piece == Pieces.Black_Man || piece == Pieces.Black_King))
                    {
                        CheckAndAddMove(x, y, x + 1, y + 1);
                        CheckAndAddMove(x, y, x + 1, y - 1);
                        CheckAndAddMove(x, y, x + 2, y + 2);
                        CheckAndAddMove(x, y, x + 2, y - 2);

                        if (piece == 4)
                        {
                            CheckAndAddMove(x, y, x - 1, y + 1);
                            CheckAndAddMove(x, y, x - 1, y - 1);
                            CheckAndAddMove(x, y, x - 2, y + 2);
                            CheckAndAddMove(x, y, x - 2, y - 2);
                        }
                    }
                }
            }

            return moves;

            void CheckAndAddMove(int xFrom, int yFrom, int xTo, int yTo)
            {
                Board copyBoard = new Board
                {
                    piecePositions = (int[,])parentNode.Board.piecePositions.Clone()
                };

                Move move = new Move(xFrom, yFrom, xTo, yTo);               

                if (copyBoard.MakeMove(player, move))
                {
                    int moveValue = 0;
                    
                    if (player == originalPlayer)
                    {
                        if (move.PieceTaken == Pieces.White_King || move.PieceTaken == Pieces.Black_King)
                        {
                            moveValue += 8;
                        }

                        if (move.PieceTaken == Pieces.White_Man || move.PieceTaken == Pieces.Black_Man)
                        {
                            moveValue += 5;
                        }

                        if (move.CreatedKing)
                        {
                            moveValue += 4;
                        }
                    }
                    else
                    {
                        if (move.PieceTaken == Pieces.White_King || move.PieceTaken == Pieces.Black_King)
                        {
                            moveValue -= 10;
                        }

                        if (move.PieceTaken == Pieces.White_Man || move.PieceTaken == Pieces.Black_Man)
                        {
                            moveValue -= 6;
                        }

                        if (move.CreatedKing)
                        {
                            moveValue -= 5;
                        }
                    }

                    var moveNode = new MoveNode(copyBoard, move, parentNode);
                    moveNode.Value = moveValue;
                    moves.Add(moveNode);
                }
            }
        }

        private int[,] DeepCopyArray(int[,] toCopy, int arrySize)
        {
            var newArry = new int[8, 8];

            for (int i = 0; i < arrySize; i++)
            {
                for (int j = 0; j < arrySize; j++)
                {
                    
                }

            }

            return newArry;
        }
    }
}
