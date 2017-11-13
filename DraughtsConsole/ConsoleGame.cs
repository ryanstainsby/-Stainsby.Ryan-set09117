using System;
using System.Collections.Generic;
using System.Text;
using DraughtsFramework;

namespace DraughtsConsole
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

            while (cmd.Length != 1 || !System.Text.RegularExpressions.Regex.IsMatch(cmd, "[0-3]"))
            {
                Console.Clear();
                Console.WriteLine(message);
                Console.WriteLine("\nPlease enter a number from 0 to 3");
                cmd = Console.ReadLine();
            }

            gameMode = Int32.Parse(cmd);
            message = gameMode != 3 ? $"Player {player} please make a move? (Example move: A3,B4)" : "Press enter to make move";

            while (cmd != "0")
            {
                PrintBoardAndMessage();

                if (cmd == "undo")
                {
                    if (logger.UndoLogIsEmpty())
                    {
                        message = "No moves to undo\n" + message;
                    }
                    else
                    {
                        while (cmd == "undo")
                        {
                            board.UndoMove(logger.UndoMove());
                            message = "To undo further type 'undo' else hit enter";
                            PrintBoardAndMessage();
                        }

                        AssignValuesAfterMove();
                    }
                }
                else if (cmd == "redo")
                {
                    if (logger.RedoLogIsEmpty())
                    {
                        message = "No moves to redo\n" + message;
                    }
                    else
                    {
                        while (cmd == "redo")
                        {
                            board.RedoMove(logger.RedoMove());
                            message = "To redo further type 'redo' else hit enter";
                            PrintBoardAndMessage();
                        }

                        AssignValuesAfterMove();
                    }
                }
                else if (gameMode == 1)
                {
                    if (logger.GetLastMove() != null && logger.GetLastMove().Player == player && logger.GetLastMove().SuccessiveMoves != null && cmd == string.Empty)
                    {
                        SwitchPlayer();
                        message = $"Player {player} please make a move (Example move: A3,B4)";
                    }
                    else if (IsInMoveFormat(cmd))
                    {
                        if (MakeHumanMove($"Player {player} please make a move (Example move: A3,B4)"))
                        {
                            AssignValuesAfterMove();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect move format");
                    }
                }
                else if (gameMode == 2)
                {
                    if (player == 1)
                    {
                        if (logger.GetLastMove() != null && logger.GetLastMove().Player == player && logger.GetLastMove().SuccessiveMoves != null && cmd == string.Empty)
                        {
                            SwitchPlayer();
                            message = "Press enter for opponent to make move";
                        }
                        else if (IsInMoveFormat(cmd) && MakeHumanMove("Press enter for opponent to make move"))
                        {
                            AssignValuesAfterMove();
                        }
                        else
                        {
                            Console.WriteLine("Incorrect move format");
                        }
                    }
                    else
                    {
                        MakeAiMove("Player 1 please make a move (Example move: A3,B4)");
                    }
                }
                else if (gameMode == 3)
                {
                    MakeAiMove("Press enter to make a move");
                    AssignValuesAfterMove();
                }
            }

            bool MakeHumanMove(string messageToDisplay)
            {
                Move move = ConsoleHelper.GenerateMoveFromUserInput(player, cmd);

                if (IsPermittedMove(move) && board.MakeMove(move))
                {
                    logger.AddMove(move);
                    return true;
                }
                else
                {
                    message = $"That move is not legal, player {player} please try another move";
                    return false;
                }
            }

            void MakeAiMove(string messageToDisplay)
            {
                Move move = ai.GetBestMove(board, new MoveNode(board, logger.GetLastMove()), player, 5);
                board.MakeMove(move);
                logger.AddMove(move);  
            }

            void PrintBoardAndMessage()
            {
                Console.Clear();
                ConsoleHelper.PrintBoard(board);
                Console.WriteLine(message);
                ConsoleHelper.PrintLog(logger);
                cmd = Console.ReadLine();
            }

            void SwitchPlayer()
            {
                player = player == 1 ? player = 2 : 1;
            }

            bool IsPermittedMove(Move move)
            {
                if (move.SuccessiveMoves == null)
                {
                    return true;
                }

                foreach (Move m in move.SuccessiveMoves)
                {
                    if (m.XFrom == move.XFrom && m.YFrom == move.YFrom && m.XTo == move.XTo && m.YTo == move.YTo)
                    {
                        return true;
                    }
                }

                return false;
            }

            void AssignValuesAfterMove()
            {
                if (board.IsWinner())
                {
                    message = $"Player {player} wins";
                }
                else
                {
                    Move prevMove = logger.GetLastMove();

                    if (prevMove != null)
                    {
                        if (prevMove.SuccessiveMoves != null && prevMove.SuccessiveMoves.Count > 0)
                        {
                            player = prevMove.Player;
                            message = Message(true);
                        }
                        else
                        {
                            player = prevMove.Player == 1 ? 2 : 1;
                            message = Message();
                        }
                    }
                    else
                    {
                        player = 1;
                        message = Message();
                    }
                }          
                
                string Message(bool successive = false)
                {
                    if (gameMode == 1 || (gameMode == 2 && player == 1))
                    {
                        return successive ? $"Player {player} please make another capturing move, otherwise press enter to continue" : $"Player {player} please make a move (Example move: A3,B4)";
                    }
                    else if (gameMode == 2 && player == 2)
                    {
                        return "Press enter for opponent to make move";
                    }
                    else
                    {
                        return "Press enter to make a move";
                    }
                }
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
