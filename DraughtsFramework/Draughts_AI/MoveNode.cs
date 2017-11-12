using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DraughtsFramework
{
    public class MoveNode
    {
        protected readonly List<MoveNode> _children = new List<MoveNode>();
        // The board state asssociated with this node
        protected Board _board;
        // The value assosiated with this node after evaluation
        protected int _value;
        // The best move for this node
        protected MoveNode _bestMoveNode;
        // The move that caused this node
        protected Move _move;
        protected MoveNode _parent;

        public MoveNode(Board board, Move move, MoveNode parent = null)
        {
            this._board = board;
            this._parent = parent;
            this._move = move;
        }

        public MoveNode BestMove
        {
            get { return _bestMoveNode; }
            set {
                if (_children.Count == 0)
                {
                    _bestMoveNode = null;
                }
                else
                {
                    _bestMoveNode = value;
                }
            }
        }

        public Board Board
        {
            get => _board; set => _board = value;
        }

        public Move Move
        {
            get => _move; set => _move = value;
        }

        public int Value
        {
            get => _value; set => _value = value;
        }

        public void AddChild(MoveNode node)
        {
            _children.Add(node);
        }

        public MoveNode Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public List<MoveNode> Children
        {
            get => _children;
        }
    }
}
