using System;
using DraughtsFramework;

namespace DraughtsConsole
{
    /// <summary>
    /// Helper class for writing to a receiving commands from the console
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Converts user input into array positions and returns a move
        /// </summary>
        public static Move GenerateMoveFromUserInput(int player, string cmd)
        {
            int xFrom = int.Parse(cmd.Substring(1, 1));
            string yFrom = cmd.Substring(0, 1);
            int xTo = int.Parse(cmd.Substring(4, 1));
            string yTo = cmd.Substring(3, 1);

            int newXFrom = SwitchPositions(xFrom);
            int newYFrom = ((yFrom.ToLower().ToCharArray()[0])) - 97;
            int newXTo = SwitchPositions(xTo);
            int newYTo = ((yTo.ToLower().ToCharArray()[0])) - 97;

            return new Move(player, newXFrom, newYFrom, newXTo, newYTo);

            int SwitchPositions(int x)
            {
                switch (x)
                {
                    case 1:
                        return 7;
                    case 2:
                        return 6;
                    case 3:
                        return 5;
                    case 4:
                        return 4;
                    case 5:
                        return 3;
                    case 6:
                        return 2;
                    case 7:
                        return 1;
                    case 8:
                        return 0;
                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        /// Prints the board with current positions
        /// </summary>
        /// <param name="board">The board to be printed</param>
        public static void PrintBoard(Board board)
        {
            int printCount = 1;
            int rowNum = 8;

            foreach (var item in board.piecePositions)
            {
                string piece = "--";

                switch (item)
                {
                    case 1:
                        piece = "WP";
                        break;
                    case 2:
                        piece = "BP";
                        break;
                    case 3:
                        piece = "WK";
                        break;
                    case 4:
                        piece = "BK";
                        break;
                }


                if (printCount == 1)
                {
                    Console.Write(rowNum + "| ");
                    rowNum--;
                }

                if (printCount % 8 == 0)
                {
                    Console.WriteLine(piece);
                    Console.WriteLine(" |");
                }
                else
                {
                    Console.Write(piece + " ");
                }

                if (printCount % 8 == 0 && printCount < 64)
                {
                    Console.Write(rowNum + "| ");
                    rowNum--;
                }

                printCount++;
            }

            Console.WriteLine("  ------------------------");
            Console.WriteLine("   A  B  C  D  E  F  G  H");
        }

        /// <summary>
        /// Prints the move log at the right hand side of the board
        /// </summary>
        /// <param name="logger">The log to be printed</param>
        public static void PrintLog(MoveLogger logger)
        {
            string title = "___Log___";
            var log = logger.GetFullLog();

            if (log.Count == 0)
            {
                Console.SetCursorPosition(28, 0);
                Console.WriteLine(title);
            }
            else
            {
                int height = 1;

                Console.SetCursorPosition(28, 0);
                Console.WriteLine(title);
                foreach (Move move in log)
                {
                    Console.SetCursorPosition(28, height);
                    if (height >= 16)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(GenerateLogEntryFromMove(move) + Environment.NewLine);
                    }

                    height++;                    
                }
            }

            Console.SetCursorPosition(0, 19);

            // Convert log entry into a move format that the user can understand
            string GenerateLogEntryFromMove(Move move)
            {
                string from = Convert.ToChar(move.YFrom + 97).ToString() + SwitchPositions(move.XFrom);
                string to = Convert.ToChar(move.YTo + 97).ToString() + SwitchPositions(move.XTo);

                return from + " => " + to;

                int SwitchPositions(int x)
                {
                    switch (x)
                    {
                        case 7:
                            return 1;
                        case 6:
                            return 2;
                        case 5:
                            return 3;
                        case 4:
                            return 4;
                        case 3:
                            return 5;
                        case 2:
                            return 6;
                        case 1:
                            return 7;
                        case 0:
                            return 8;
                        default:
                            return 0;
                    }
                }
            }
        }
    }
}
