using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsFramework
{    
    public class Move
    {
        private int _player;
        private int _xFrom;
        private int _yFrom;
        private int _xTo;
        private int _yTo;
        private int _piece;
        private int _pieceTaken;
        private bool _createdKing;
        private List<Move> _successiveMoves;

        public Move(int player, int xFrom, int yFrom, int xTo, int yTo)
        {
            _player = player;
            _xFrom = xFrom;
            _yFrom = yFrom;
            _xTo = xTo;
            _yTo = yTo;
            _createdKing = false;
        }

        public int Player { get => _player; set => _player = value; }
        public int XFrom { get => _xFrom; set => _xFrom = value; }
        public int YFrom { get => _yFrom; set => _yFrom = value; }
        public int XTo { get => _xTo; set => _xTo = value; }
        public int YTo { get => _yTo; set => _yTo = value; }
        public int Piece { get => _piece; set => _piece = value; }
        public int PieceTaken { get => _pieceTaken; set => _pieceTaken = value; }
        public bool CreatedKing { get => _createdKing; set => _createdKing = value; }
        public List<Move> SuccessiveMoves { get => _successiveMoves; set => _successiveMoves = value; }
    }
}
