using System;
using System.Collections.Generic;
using System.Linq;
 
namespace DraughtsFramework
{
    public class DraughtsAI
    {
        // Stores the player the moves being evaluated for
        int? originalPlayer = null;

        /// <summary>
        /// Returns the best move the algorith predicts can be made
        /// </summary>
        public Move GetBestMove(Board board, MoveNode prevMove, int player, int depth)
        {
            originalPlayer = player;
            MoveNode nextMove = BuildAlphaBeta(board, prevMove, player, depth);
                      
            // Check there is a legal move to make
            if (nextMove.Children.Count != 0)
            {
                var highestValues = nextMove.Children.GroupBy(x => x.Value).OrderByDescending(g => g.Key).First().ToArray();
                var random = new Random();
                MoveNode randomMove = highestValues[random.Next(highestValues.Count() - 1)];

                return (randomMove.Move);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a tree off a nodes possible moves with values assigned using alpha beta pruning
        /// </summary>
        /// <param name="board">Current board state</param>
        /// <param name="prevMove">Last move made</param>
        /// <param name="player">Player tree is being built for</param>
        /// <param name="depth">How many levels should be generated for the tree</param>
        /// <returns></returns>
        private MoveNode BuildAlphaBeta(Board board, MoveNode prevMove, int player, int depth)
        {
            MoveNode currentPosition = prevMove ?? new MoveNode(board, prevMove?.Move ?? null, null);

            if (depth != 0)
            {
                if (currentPosition.Move != null && currentPosition.Move.SuccessiveMoves != null)
                {
                    int bestMoveScore = player == originalPlayer.Value ? 0 : -100;

                    foreach (var move in currentPosition.Move.SuccessiveMoves)
                    {
                        MoveNode sucMove = CheckAndValueMove(currentPosition, move, depth);
                        MoveNode possibleMove = BuildAlphaBeta(sucMove.Board, sucMove, player, depth - 1);

                        if (possibleMove == null) continue;

                        if ((player == originalPlayer && possibleMove.Value > bestMoveScore) || (player != originalPlayer && possibleMove.Value < bestMoveScore))
                        {
                            bestMoveScore = possibleMove.Value;
                        }

                        possibleMove.Parent = currentPosition;
                        currentPosition.AddChild(possibleMove);
                    }
                }
                else
                {
                    int bestMoveScore = player == originalPlayer.Value ? -100 : 100;

                    foreach (var move in FindAllLegalMoves(currentPosition, player, depth))
                    {
                        MoveNode possibleMove = BuildAlphaBeta(move.Board, move, player == 1 ? 2 : 1, depth - 1);
                                                
                        if ((player == originalPlayer && possibleMove.Value > bestMoveScore) || (player != originalPlayer && possibleMove.Value < bestMoveScore))
                        {
                            bestMoveScore = possibleMove.Value;
                        }

                        possibleMove.Parent = currentPosition;
                        currentPosition.AddChild(possibleMove);
                    }

                    currentPosition.Value += bestMoveScore;
                }

            }

            return currentPosition;       
        }

        /// <summary>
        /// Returns a list of a legal moves a that can be made by a player for the current board state and assigns a value to each
        /// </summary>
        private List<MoveNode> FindAllLegalMoves(MoveNode parentNode, int player, int depth)
        {
            var moves = new List<MoveNode>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int piece = parentNode.Board.piecePositions[x, y];                    

                    if (player == 1 && (piece == Pieces.White_Man || piece == Pieces.White_King))
                    {
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 1, y + 1), depth);
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 1, y - 1), depth);
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 2, y + 2), depth);
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 2, y - 2), depth);

                        if (piece == Pieces.White_King)
                        {
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 1, y + 1), depth);
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 1, y - 1), depth);
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 2, y + 2), depth);
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 2, y - 2), depth);
                        }
                    }
                    else if (player == 2 && (piece == Pieces.Black_Man || piece == Pieces.Black_King))
                    {
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 1, y + 1), depth);
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 1, y - 1), depth);
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 2, y + 2), depth);
                        CheckAndValueMove(moves, parentNode, new Move(player, x, y, x + 2, y - 2), depth);

                        if (piece == Pieces.Black_King)
                        {
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 1, y + 1), depth);
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 1, y - 1), depth);
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 2, y + 2), depth);
                            CheckAndValueMove(moves, parentNode, new Move(player, x, y, x - 2, y - 2), depth);
                        }
                    }
                }
            }

            return moves;
        }

        /// <summary>
        /// Checks a move is legal and assigns a value to it
        /// </summary>
        private void CheckAndValueMove(List<MoveNode> moves, MoveNode parentNode, Move move, int depth)
        {
            Board copyBoard = new Board
            {
                piecePositions = (int[,])parentNode.Board.piecePositions.Clone()
            };

            if (copyBoard.MakeMove(move))
            {
                var moveNode = new MoveNode(copyBoard, move, parentNode)
                {
                    Value = GetMoveValue(copyBoard, move, depth)
                };

                moves.Add(moveNode);
            }
        }

        /// <summary>
        /// Checks a move is legal and assigns a value to it
        /// </summary>
        private MoveNode CheckAndValueMove(MoveNode parentNode, Move move, int depth)
        {
            Board copyBoard = new Board
            {
                piecePositions = (int[,])parentNode.Board.piecePositions.Clone()
            };

            if (copyBoard.MakeMove(move))
            {
                return new MoveNode(copyBoard, move, parentNode)
                {
                    Value = GetMoveValue(copyBoard, move, depth)
                };               
            }

            return null;
        }

        /// <summary>
        /// Returns the value of a move
        /// </summary>
        private int GetMoveValue(Board board, Move move, int depth)
        {
            int moveValue = 0;
            
            if (move.PieceTaken == Pieces.White_King || move.PieceTaken == Pieces.Black_King)
            {
                moveValue += 12;
            }
            else if (move.PieceTaken == Pieces.White_Man || move.PieceTaken == Pieces.Black_Man)
            {
                moveValue += 10;
            }

            if (move.CreatedKing)
            {
                moveValue += 4;
            }

            if (Rules.CanTakeAnotherPiece(board.piecePositions, move))
            {
                moveValue += 2;
            }
            else if (board.IsWinner())
            {
                moveValue = 200;
            }

            return moveValue;
        }
    }
}
