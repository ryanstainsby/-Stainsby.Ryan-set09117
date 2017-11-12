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
            message = gameMode != 3 ? $"Player {player} please make a move? (Format: positionFrom,positionTo)" : "Press enter to make move";

            while (cmd != "0")
            {
                PrintBoardAndMessage();

                if (cmd == "print log")
                {
                    ConsoleHelper.PrintLog(logger);
                    cmd = Console.ReadLine();
                }
                else if (cmd == "undo")
                {
                    if (logger.IsEmpty())
                    {
                        message = "No moves to undo\n" + message;
                    }
                    else
                    {
                        board.UndoMove(logger.UndoMove());
                        player = logger.GetLastMove().Piece == Pieces.White_Man || logger.GetLastMove().Piece == Pieces.White_King ? 1 : 2;
                        message = "To undo further type 'undo' else hit enter";
                    }
                }
                else if (gameMode == 1)
                {
                    if (logger.GetLastMove().Player == player && logger.GetLastMove().SuccessiveMoves != null && cmd == string.Empty)
                    {
                        SwitchPlayer();
                    }
                    else if (IsInMoveFormat(cmd))
                    {
                        MakeHumanMove("Player 1 please make a move ? (Format: positionFrom, positionTo)");
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
                        if (logger.GetLastMove().Player == player && logger.GetLastMove().SuccessiveMoves != null && cmd == string.Empty)
                        {
                            SwitchPlayer();
                        }
                        else if (IsInMoveFormat(cmd))
                        {
                            MakeHumanMove("Press enter for opponent to make move");
                        }
                        else
                        {
                            Console.WriteLine("Incorrect move format");
                        }
                    }
                    else
                    {
                        MakeAiMove("Player 1 please make a move? (Format: positionFrom,positionTo)");
                    }
                }
                else if (gameMode == 3)
                {
                    MakeAiMove("Press enter to make a move");
                }
            }

            void MakeHumanMove(string messageToDisplay)
            {
                int xFrom = int.Parse(cmd.Substring(1, 1));
                string yFrom = cmd.Substring(0, 1);
                int xTo = int.Parse(cmd.Substring(4, 1));
                string yTo = cmd.Substring(3, 1);

                Move move = ConsoleHelper.GenerateMoveFromUserInput(player, xFrom, yFrom, xTo, yTo);

                if (IsPermittedMove(move) && board.MakeMove(move))
                {
                    logger.AddMove(move);

                    if (Rules.CanTakeAnotherPiece(board.piecePositions, move))
                    {
                        message = $"Player {player} please make another capturing move, otherwise press enter to continue";
                    }
                    else
                    {
                        SwitchPlayer();
                        message = messageToDisplay;
                    }
                }
                else
                {
                    message = $"That move is not legal, player {player} please try another move";
                }
            }

            void MakeAiMove(string messageToDisplay)
            {
                Move move = ai.GetBestMove(board, new MoveNode(board, logger.GetLastMove()), player, 5);
                board.MakeMove(move);
                logger.AddMove(move);
                message = messageToDisplay;
                if (logger.GetLastMove()?.SuccessiveMoves == null)
                {
                    SwitchPlayer();
                }
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
                if (move.SuccessiveMoves == null) return true;

                foreach (Move m in move.SuccessiveMoves)
                {
                    if (m.XFrom == move.XFrom && m.YFrom == move.YFrom && m.XTo == move.XTo && m.YTo == move.YTo)
                    {
                        return true;
                    }
                }

                return false;
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
