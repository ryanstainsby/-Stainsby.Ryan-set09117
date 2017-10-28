using DraughtsGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DraughtsTests
{
    [TestClass]
    public class BecomesKing
    {
        Board board = new Board();

        [TestMethod]
        public void WhiteBecomesKing()
        {
            board.piecePositions = new int[,]{ { 0, 0, 0, 2, 0, 2, 0, 2 },
                                               { 1, 0, 2, 0, 2, 0, 2, 0 },
                                               { 0, 0, 0, 2, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 1, 0, 1, 0, 1, 0 },
                                               { 0, 1, 0, 1, 0, 1, 0, 1 },
                                               { 1, 0, 1, 0, 1, 0, 1, 0 } };

            Move move = new Move(1, 0, 0, 1);
            board.MakeMove(1, move);
            Assert.IsTrue(board.piecePositions[0, 1] == Pieces.White_King);
        }

        [TestMethod]
        public void BlackBecomesKing()
        {
            board.piecePositions = new int[,]{ { 0, 2, 0, 2, 0, 2, 0, 2 },
                                               { 2, 0, 2, 0, 2, 0, 2, 0 },
                                               { 0, 0, 0, 2, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 1, 0, 1, 0, 1, 0 },
                                               { 0, 2, 0, 1, 0, 1, 0, 1 },
                                               { 0, 0, 1, 0, 1, 0, 1, 0 } };
            
            Move move = new Move(6, 1, 7, 0);
            board.MakeMove(2, move);
            Assert.IsTrue(board.piecePositions[7, 0] == Pieces.Black_King);
        }
    }
}
