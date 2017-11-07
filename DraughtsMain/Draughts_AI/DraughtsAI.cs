using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
 
namespace DraughtsGame
{
    class DraughtsAI
    {
        int? originalPlayer = null;

        public Move GetBestMove(Board board, MoveNode moveTree, int player, int depth)
        {
            MoveNode nextMove = MoveTree(board, null, player, depth);

            var highestValues = nextMove.Children.GroupBy(x => x.Value).OrderByDescending(g => g.Key).First().ToArray();
            var random = new Random();
            MoveNode randomMove = highestValues[random.Next(highestValues.Count() - 1)];
            
            return (randomMove.Move);
        }

        public MoveNode MoveTree(Board board, MoveNode prevMove, int player, int depth)
        {
            MoveNode currentPosition = prevMove ?? new MoveNode(board, prevMove?.Move ?? null, null);

            if (originalPlayer == null) originalPlayer = player;

            if (depth != 0)
            {
                int bestMoveScore = player == originalPlayer.Value ? 0 : -100;

                foreach (var move in FindAllLegalMoves(currentPosition, player))
                {
                    MoveNode possibleMove = MoveTree(move.Board, move, player == 1 ? 2 : 1, depth - 1);

                    if (possibleMove.Value > bestMoveScore)
                    {
                        bestMoveScore = possibleMove.Value;
                    }

                    possibleMove.Parent = currentPosition;
                    currentPosition.AddChild(possibleMove);
                }

                currentPosition.Value += bestMoveScore;
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
