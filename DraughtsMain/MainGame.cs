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
            int gameMode = 1;
            int player = 1;
            string cmd = string.Empty;
            string message = "Please choose a game mode:\n" +
                             "1 = Player vs Player\n" +
                             "2 = Player vs AI\n" +
                             "3 = AI vs AI\n" +
                             "0 = Exit";

            Console.WriteLine(message);
            cmd = Console.ReadLine();
                       
            while (!System.Text.RegularExpressions.Regex.IsMatch(cmd, "[0-3]"))
            {
                Console.WriteLine("Please enter a number from 0 to 3");
                Console.WriteLine(message);
                cmd = Console.ReadLine();
            }

            gameMode = Int32.Parse(cmd);
            message = $"Player {player} please make a move? (Format: positionFrom,positionTo)";

            while (cmd != "0")
            {
                if (cmd == "print log")
                {
                    logger.PrintLog();
                    cmd = string.Empty;
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
                    }

                    Console.WriteLine("To undo further type 'undo' else hit enter");
                    cmd = Console.ReadLine();
                }
                else if (gameMode == 1)
                {
                    PrintBoardAndMessage();

                    if (IsInMoveFormat(cmd))
                    {
                        MakeHumanMove();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect move format");
                    }
                }
                else if (gameMode == 2)
                {
                    PrintBoardAndMessage();

                    if (player == 1)
                    {
                        if (IsInMoveFormat(cmd))
                        {
                            if (MakeHumanMove())
                            {
                                message = "Press enter for opponent to make move";
                            }
                        }
                        else
                        {
                            Console.WriteLine("Incorrect move format");
                        }
                    }
                    else
                    {
                        MakeAiMove();
                        message = "Player 1 please make a move? (Format: positionFrom,positionTo)";
                    }
                }
                else if (gameMode == 3)
                {
                    message = "Press enter to make a move";
                    PrintBoardAndMessage();
                    MakeAiMove();
                }
            }
            
            bool MakeHumanMove()
            {                
                int xFrom = int.Parse(cmd.Substring(1, 1));
                string yFrom = cmd.Substring(0, 1);
                int xTo = int.Parse(cmd.Substring(4, 1));
                string yTo = cmd.Substring(3, 1);

                Move move = board.GenerateMoveFromUserInput(xFrom, yFrom, xTo, yTo);

                if (board.MakeMove(player, move))
                {
                    logger.AddMove(move);
                    player = player == 1 ? 2 : 1;
                    message = $"Player {player} please make a move? (Format: positionFrom,positionTo)";
                    return true;
                }
                else
                {
                    message = $"That move is not legal, player {player} please try another move";
                    return false;
                }
            }

            void MakeAiMove()
            {
                Move move = ai.GetBestMove(board, null, player, 5);
                board.MakeMove(player, move);
                logger.AddMove(move);
                
                player = player == 1 ? 2 : 1;
            }

            void PrintBoardAndMessage()
            {
                Console.Clear();
                board.PrintBoard();
                Console.WriteLine(message);
                cmd = Console.ReadLine();
            }
        }
       
        // Check if the user has entered a column and a row
        private static bool IsInMoveFormat(string cmd)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(cmd, "[A-z][1-8],[A-z][1-8]"))
            {
                return true;
            }

            return false;
        }
    }
}
