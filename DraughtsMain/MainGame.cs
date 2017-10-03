using System;

namespace DraughtsGame
{
    class MainGame
    {
        static void Main(string[] args)
        {
            Board board = new Board();

            string cmd = string.Empty;
            int player = 1;
            while (cmd != "exit")
            {
                board.PrintBoard();

                Console.WriteLine($"Player {(player)} which piece would you like to move? (Format: columnFrom,rowFrom,columnTo,rowTo)");
                cmd = Console.ReadLine();

                if (IsInCorrectFormat(cmd))
                {
                    board.MakeMove(player, int.Parse(cmd.Substring(0, 1)) - 1, int.Parse(cmd.Substring(2, 1)) - 1, int.Parse(cmd.Substring(4,1)) - 1, int.Parse(cmd.Substring(6,1)) - 1);
                    player = player == 1 ? player++ : player--;
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
            if (System.Text.RegularExpressions.Regex.IsMatch(cmd, "[0-9],[0-9],[0-9],[0-9]") )
            {
                return true;
            }

            return false;
        }
    }
}
