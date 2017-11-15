using System;
using System.Collections.Generic;
using System.IO;

namespace DraughtsFramework
{
    /// <summary>
    /// Keeps a log of all moves made to allow undoing, redoing and printing of all moves made
    /// </summary>
    public class MoveLogger
    {
        // A log of moves that can be undone
        private Stack<Move> undoStack;
        // A log of moves that can be redone
        private Stack<Move> redoStack;

        public MoveLogger()
        {
            undoStack = new Stack<Move>();
            redoStack = new Stack<Move>();
        }

        /// <summary>
        /// Adds a move to the log
        /// </summary>
        /// <param name="move">Move to be added to the log</param>
        public void AddMove(Move move)
        { 
            undoStack.Push(move);
            redoStack.Clear();
        }

        /// <summary>
        /// Removes the last move from the log and returns it
        /// </summary>
        public Move UndoMove()
        {
            Move move = undoStack.Pop();

            redoStack.Push(move);

            return move;
        }

        /// <summary>
        /// Removes the last move undone from the log and returns it
        /// </summary>
        public Move RedoMove()
        {
            Move move = redoStack.Pop();

            undoStack.Push(move);

            return move;
        }

        /// <summary>
        /// Returns the full log
        /// </summary>
        public Stack<Move> GetFullLog()
        {
            return undoStack;
        }

        /// <summary>
        /// Returns the last move added to the log
        /// </summary>
        /// <returns></returns>
        public Move GetLastMove()
        {
            if (undoStack.Count > 0)
            {
                return undoStack.Peek();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if there are any moves that can be undone
        /// </summary>
        public bool UndoLogIsEmpty()
        {
            return undoStack.Count == 0;
        }


        /// <summary>
        /// Checks if there are any moves that can be redone
        /// </summary>
        /// <returns></returns>
        public bool RedoLogIsEmpty()
        {
            return redoStack.Count == 0;
        }

        /// <summary>
        /// Saves the Stack of all current moves
        /// </summary>
        public void SaveGame()
        {          
            File.WriteAllText("saved_game.json", Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(undoStack));                            
        }

        /// <summary>
        /// Loads the saved Stack of moves
        /// </summary>
        public void LoadGame()
        {
            redoStack = Newtonsoft.Json.JsonConvert.DeserializeObject<Stack<Move>>(File.ReadAllText("saved_game.json"));
        }
    }
}
