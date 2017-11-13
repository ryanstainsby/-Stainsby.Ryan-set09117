using System;
using System.Collections.Generic;
using System.IO;

namespace DraughtsFramework
{
    public class MoveLogger
    {
        private Stack<Move> undoList;
        private Stack<Move> redoList;

        public MoveLogger()
        {
            undoList = new Stack<Move>();
            redoList = new Stack<Move>();
        }

        public void AddMove(Move move)
        { 
            undoList.Push(move);
            redoList.Clear();
        }

        public Move UndoMove()
        {
            Move move = undoList.Pop();

            redoList.Push(move);

            return move;
        }

        public Move RedoMove()
        {
            Move move = redoList.Pop();

            undoList.Push(move);

            return move;
        }

        public Stack<Move> GetFullLog()
        {
            return undoList;
        }

        public Move GetLastMove()
        {
            if (undoList.Count > 0)
            {
                return undoList.Peek();
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

        public void SaveGame()
        {          
            

            File.WriteAllText("saved_game.json", Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(undoList));                            
        }

        public void LoadGame()
        {
            redoList = Newtonsoft.Json.JsonConvert.DeserializeObject<Stack<Move>>(File.ReadAllText("saved_game.json"));
        }

        public Move ReplayMove()
        {
            return redoList.Pop();
        }
    }
}
