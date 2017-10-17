using System;

namespace DraughtsGame
{
    class MainGame
    {
        static void Main(string[] args)
        {
            Board board = new Board();

            string cmd = string.Empty;
            string message = "Player 1 please make a move? (Format: rowFrom,columnFrom,rowTo,columnTo)";
            int player = 1;
            while (cmd != "exit")
            {
                board.PrintBoard();

                Console.WriteLine(message);
                cmd = Console.ReadLine();

                if (IsInCorrectFormat(cmd))
                {
                    if (board.MakeMove(player, int.Parse(cmd.Substring(0, 1)) - 1, int.Parse(cmd.Substring(2, 1)) - 1, int.Parse(cmd.Substring(4, 1)) - 1, int.Parse(cmd.Substring(6, 1)) - 1))
                    {
                        player = player == 1 ? player + 1 : player - 1;
                        message = $"Player {player} please make a move? (Format: columnFrom,rowFrom,columnTo,rowTo)";
                    }
                    else
                    {
                        message = $"That move is not legal, player {player} please try another move";
                    }                    
                }
                else
                {
                    Console.WriteLine("Error with format, please try again");
                }
            }
        }

        // Check that the user has entered a column and a row
        private static bool IsInCorrectFormat (string cmd)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(cmd, "[1-8],[1-8],[1-8],[1-8]") )
            {
                return true;
            }

            return false;
        }
    }
}
