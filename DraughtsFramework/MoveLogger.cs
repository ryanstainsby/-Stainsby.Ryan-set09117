using System;
using System.Collections.Generic;

namespace DraughtsFramework
{
    public class MoveLogger
    {
        private LinkedList<Move> undoList;
        private LinkedList<Move> redoList;

        public MoveLogger()
        {
            undoList = new LinkedList<Move>();
            redoList = new LinkedList<Move>();
        }

        public void AddMove(Move move)
        { 
            undoList.AddFirst(move);
            redoList.Clear();
        }

        public Move UndoMove()
        {
            Move move = undoList.First.Value;

            undoList.RemoveFirst();
            redoList.AddFirst(move);

            return move;
        }

        public Move RedoMove()
        {
            Move move = redoList.First.Value;

            redoList.RemoveFirst();
            undoList.AddFirst(move);

            return move;
        }

        public LinkedList<Move> GetFullLog()
        {
            return undoList;
        }

        public Move GetLastMove()
        {
            if (undoList.Count > 0)
            {
                return undoList.First.Value;
            }
            else
            {
                return null;
            }
        }

        public bool UndoLogIsEmpty()
        {
            return undoList.Count == 0;
        }

        public bool RedoLogIsEmpty()
        {
            return redoList.Count == 0;
        }
    }
}
