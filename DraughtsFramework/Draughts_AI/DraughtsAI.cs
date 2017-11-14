﻿using System;
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
            MoveNode nextMove = BuildMoveTree(board, prevMove, player, depth);
            
            foreach (MoveNode child in nextMove.Children)
            {
                child.Value = AlphaBeta(child, -100, +100);
            }

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
        /// Alpha Beta search to return the value of a node  
        /// </summary>
        private int AlphaBeta(MoveNode node, int alpha, int beta)
        {
            int bestValue;

            if (node.Children.Count == 0)
            {
                bestValue = node.Value;
            }
            else if (node.Move.Player == originalPlayer)
            {
                bestValue = alpha;

                // Recurse for all children of node.
                for (int i = 0; i < node.Children.Count; i++)
                {
                    var childValue = AlphaBeta(node.Children[i], bestValue, beta);
                    bestValue = Math.Max(bestValue, childValue);
                    if (beta <= bestValue)
                    {
                        break;
                    }
                }
            }
            else
            {
                bestValue = beta;

                // Recurse for all children of node.
                for (int i = 0; i < node.Children.Count; i++)
                {
                    var childValue = AlphaBeta(node.Children[i], alpha, bestValue);
                    bestValue = Math.Min(bestValue, childValue);
                    if (bestValue <= alpha)
                    {
                        break;
                    }
                }
            }

            return bestValue;
        }

        /// <summary>
        /// Builds a move tree of all possible moves a player can make from the current board state and assigns values to each
        /// </summary>
        /// <param name="board">Current board state</param>
        /// <param name="prevMove">Last move made</param>
        /// <param name="player">Player tree is being built for</param>
        /// <param name="depth">How many levels should be generated for the tree</param>
        /// <returns></returns>
        private MoveNode BuildMoveTree(Board board, MoveNode prevMove, int player, int depth)
        {
            MoveNode currentPosition = prevMove ?? new MoveNode(board, prevMove?.Move ?? null, null);

            if (depth != 0)
            {
                if (currentPosition.Move != null && currentPosition.Move.SuccessiveMoves != null)
                {
                    foreach (var move in currentPosition.Move.SuccessiveMoves)
                    {
                        MoveNode possibleMove = CheckAndValueMove(currentPosition, move, depth);

                        if (possibleMove == null) continue;

                        AddChildrenToNode(possibleMove);
                    }
                }
                else
                {
                    foreach (var moveNode in FindAllLegalMoves(currentPosition, player, depth))
                    {
                        AddChildrenToNode(moveNode);
                    }
                }                
            }

            return currentPosition;

            void AddChildrenToNode(MoveNode possibleMove)
            {
                if (possibleMove.Value != 100 || possibleMove.Value != -100)
                {
                    if (possibleMove.Move.SuccessiveMoves != null)
                    {                        
                        possibleMove = BuildMoveTree(possibleMove.Board, possibleMove, player, depth - 1);
                    }
                    else
                    {
                        possibleMove = BuildMoveTree(possibleMove.Board, possibleMove, player == 1 ? 2 : 1, depth - 1);
                    }

                    possibleMove.Parent = currentPosition;
                }

                currentPosition.AddChild(possibleMove);
            }
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

            moveValue = moveValue * (depth);

            return moveValue;
        }
    }
}
