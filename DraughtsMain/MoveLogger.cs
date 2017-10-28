using System;
using System.Collections.Generic;

namespace DraughtsGame
{
    class MoveLogger
    {
        private LinkedList<Move> moveList;

        public MoveLogger()
        {
            moveList = new LinkedList<Move>();
        }

        public void AddMove(Move move)
        { 
            moveList.AddFirst(move);
        }

        public Move UndoMove()
        {
            Move move = moveList.First.Value;

            moveList.RemoveFirst();

            return move;
        }

        public void PrintLog()
        {
            foreach (Move move in moveList)
            {
                Console.WriteLine(Environment.NewLine + "___Log___");
                Console.WriteLine(move.XFrom + ", " + move.YFrom + " => " + move.XTo + ", " + move.YTo + Environment.NewLine);
            }            
        }

        public bool IsEmpty()
        {
            return moveList.Count == 0;
        }
    }
}
