using System;
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
            int gameMode = GetGameMode();
            int player = 1;
            string cmd = string.Empty;
            string message = gameMode != 3 ? $"Player {player} please make a move? (Example move: A3,B4)" : "Press enter to make move";
            string errorMessage = string.Empty;
            
            // Game replay mode
            if (gameMode == 4)
            {
                ReplayGame();
            }
            else
            {
                bool isHumanTurn;
                bool isAiTurn;

                // Keep looping until player decides to exit
                while (gameMode != 0)
                {
                    isHumanTurn = gameMode == 1 || (gameMode == 2 && player == 1);
                    isAiTurn = gameMode == 3 || (gameMode == 2 && player == 2);

                    PrintBoardAndMessage();
                    errorMessage = string.Empty;

                    switch (cmd)
                    {
                        // Is human turn and command is in correct format for making a move
                        case var positions when (System.Text.RegularExpressions.Regex.IsMatch(cmd, "[A-z][1-8],[A-z][1-8]") && isHumanTurn):
                            MakeHumanMove();
                            break;
                        
                        // Is AI turn or a successive capture is available and user has pressed enter
                        case "":
                            if (isAiTurn)
                            {
                                MakeAiMove();
                            }
                            else if (logger.GetLastMove() != null && logger.GetLastMove().SuccessiveMoves != null)
                            {
                                AssignValuesAfterMove(true);
                            }
                            break;

                        case "undo":
                            UndoMove();
                            break;

                        case "redo":
                            RedoMove();
                            break;

                        case "0":
                            gameMode = 0;
                            break;

                        default:
                            errorMessage = "Incorrect move format";
                            break;
                    }
                }           
            }

            void UndoMove()
            {
                if (logger.UndoLogIsEmpty())
                {
                    errorMessage = "No moves to undo,";
                }
                else
                {
                    board.UndoMove(logger.UndoMove());
                    AssignValuesAfterMove();
                }
            }

            void RedoMove()
            {
                if (logger.RedoLogIsEmpty())
                {
                    errorMessage = "No moves to redo,";
                }
                else
                {
                    board.RedoMove(logger.RedoMove());
                    AssignValuesAfterMove();
                }
            }
                        
            void MakeHumanMove()
            {
                Move move = ConsoleHelper.GenerateMoveFromUserInput(player, cmd);

                if (IsPermittedMove() && board.MakeMove(move))
                {
                    logger.AddMove(move);
                    AssignValuesAfterMove();
                }
                else
                {
                    message = $"That move is not legal, player {player} please try another move";
                }
                
                // If the player is making a second move in a row it must be a legal capture with the same piece
                bool IsPermittedMove()
                {
                    if (move.Player != player)
                    {
                        return true;
                    }

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
            }

            void MakeAiMove()
            {
                Move move = ai.GetBestMove(board, new MoveNode(board, logger.GetLastMove()), player, 3);

                // There's no legal move to make, automatic win to the other player
                if (move == null)
                {
                    message = $"Player {(player == 1 ? 2 : 1)} wins press enter to exit";
                    logger.SaveGame();                    
                }
                else
                {
                    board.MakeMove(move);
                    logger.AddMove(move);
                    AssignValuesAfterMove();
                }
            }

            // Replays the game, allows for moving forward and backwards through plays
            void ReplayGame()
            {
                logger.LoadGame();

                message = "Press enter to continue or type 'back' to go back a turn";

                while (cmd != "0")
                {
                    PrintBoardAndMessage();

                    switch (cmd)
                    {
                        case "back":
                            if (logger.UndoLogIsEmpty())
                            {
                                message = "Can't go back any further, press enter to continue";
                            }
                            else
                            {
                                board.UndoMove(logger.UndoMove());
                                message = "Press enter to continue or type 'back' to go back a turn";
                            }
                            break;

                        case "":
                            if (logger.RedoLogIsEmpty())
                            {
                                message = "Game finished, type 'back' to go back a turn or '0' to exit";
                            }
                            else
                            {
                                board.RedoMove(logger.RedoMove());
                                message = "Press enter to continue or type 'back' to go back a turn";
                            }
                            break;

                        case "0":
                            return;
                    }
                }
            }

            void PrintBoardAndMessage()
            {
                Console.Clear();
                ConsoleHelper.PrintBoard(board);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine(errorMessage + " " + message);
                }

                ConsoleHelper.PrintLog(logger);
                cmd = Console.ReadLine();
            }

            // Assigns the correct player and message after a move has been made
            void AssignValuesAfterMove(bool isSkippingSuccession = false)
            {
                if (board.IsWinner())
                {
                    message = $"Player {player} wins type '0' to exit";
                    logger.SaveGame();
                }
                else
                {
                    Move prevMove = logger.GetLastMove();

                    if (prevMove != null)
                    {
                        if (prevMove.SuccessiveMoves != null && prevMove.SuccessiveMoves.Count > 0 && !isSkippingSuccession)
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
        
        /// <summary>
        /// Returns the game mode number based on the users input
        /// </summary>
        /// <returns></returns>
        private static int GetGameMode()
        {
            string message = "Please choose a game mode:\n" +
                             "1 = Player vs Player\n" +
                             "2 = Player vs AI\n" +
                             "3 = AI vs AI\n" +
                             "4 - Replay Last Game\n" +
                             "0 = Exit";

            Console.WriteLine(message);

            string cmd = Console.ReadLine();

            while (cmd.Length != 1 || !System.Text.RegularExpressions.Regex.IsMatch(cmd, "[0-4]"))
            {
                Console.Clear();
                Console.WriteLine(message);
                Console.WriteLine("\nPlease enter a number from 0 to 3");
                cmd = Console.ReadLine();
            }

            return Int32.Parse(cmd);
        }

        /// <summary>
        /// Check if the user has entered a column and a row
        /// </summary>
        /// <param name="cmd">User input</param>
        /// <returns>If move is in correct format</returns>
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
