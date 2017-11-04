using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsGame
{
    class MainGame
    {
        static void Main(string[] args)
        {
            var board = new Board();
            var logger = new MoveLogger();
            var ai = new DraughtsAI();
            string cmd = string.Empty;
            string message = "Player 1 please make a move? (Format: columnFrom,rowFrom,columnTo,rowTo)";
            int player = 1;


            while (cmd != "exit")
            {
                // Testing that tree can be generated correctly
                MoveNode node = ai.MoveTree(board, null, player, 7);
                int highest = 0;
                int lowest = 0;
                List<int> values = new List<int>();
                GetAllNodeValues(node, values);
                foreach (var idk in values)
                {
                    if (idk > highest) highest = idk;
                    if (idk < lowest) lowest = idk;
                }
                board.PrintBoard();

                Console.WriteLine(message);
                cmd = Console.ReadLine();

                if (IsInMoveFormat(cmd))
                {
                    int xFrom = int.Parse(cmd.Substring(2, 1)) - 1;
                    int yFrom = int.Parse(cmd.Substring(0, 1)) - 1;
                    int xTo = int.Parse(cmd.Substring(6, 1)) - 1;
                    int yTo = int.Parse(cmd.Substring(4, 1)) - 1;

                    Move move = new Move(xFrom, yFrom, xTo, yTo);

                    if (board.MakeMove(player, move))
                    {
                        logger.AddMove(move);
                        player = player == 1 ? 2 : 1;
                        message = $"Player {player} please make a move? (Format: columnFrom,rowFrom,columnTo,rowTo)";
                    }
                    else
                    {
                        message = $"That move is not legal, player {player} please try another move";
                    }                    
                }
                else if (cmd == "print log")
                {
                    logger.PrintLog();
                }
                else if (cmd == "undo")
                {
                    if (logger.IsEmpty())
                    {
                        Console.WriteLine("No moves to undo");
                    }
                    else
                    {
                        board.UndoMove(logger.UndoMove());
                        player = player == 1 ? 2 : 1;
                        message = $"Player {player} please make a move? (Format: columnFrom,rowFrom,columnTo,rowTo)";
                    }                
                }
                else
                {
                    Console.WriteLine("Error with format, please try again");
                }
            }
        }

        // Check if the user has entered a column and a row
        private static bool IsInMoveFormat(string cmd)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(cmd, "[1-8],[1-8],[1-8],[1-8]") )
            {
                return true;
            }

            return false;
        }

        private static void GetAllNodeValues(MoveNode ogNode, List<int> values)
        {
            values.Add(ogNode.Value);

            foreach (MoveNode node in ogNode.Children)
            {
                GetAllNodeValues(node, values);
            }
        }
    }
}
