using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DraughtsGame;

namespace DraughtsTests
{
    [TestClass]
    public class IsMovingDiagonally
    {
        Board board = new Board();

        [TestMethod]
        public void IsMovingDiagonally_WhiteMovingDiagonally_True()
        {
            Assert.IsTrue(board.MakeMove(1, 5, 0, 6, 1));
        }

        public void IsMovingDiagonally_BlackMovingDiagonally_True()
        {
            Assert.IsTrue(board.MakeMove(2, 2, 1, 3, 0));
        }

        public void IsMovingDiagonally_WhiteMovingForward_False()
        {
            Assert.IsFalse(board.MakeMove(1, 5, 2, 6, 2));
        }

        public void IsMovingDiagonally_BlackMovingForward_False()
        {
            Assert.IsFalse(board.MakeMove(2, 2, 3, 3, 3));
        }

        public void IsMovingDiagonally_WhiteManMovingDiagonallyBackwards_False()
        {
            board.piecePositions = new int[,]{ { 0, 2, 0, 2, 0, 2, 0, 2 },
                                               { 2, 0, 2, 0, 2, 0, 2, 0 },
                                               { 0, 2, 0, 2, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 1, 0, 0, 0, 0 },
                                               { 1, 0, 0, 0, 1, 0, 1, 0 },
                                               { 0, 1, 0, 1, 0, 1, 0, 1 },
                                               { 1, 0, 1, 0, 1, 0, 1, 0 } };

            Assert.IsFalse(board.MakeMove(1, 4, 3, 5, 2));
        }

        public void IsMovingDiagonally_BlackManMovingDiagonallyBackwards_False()
        {
            board.piecePositions = new int[,]{ { 0, 2, 0, 2, 0, 2, 0, 2 },
                                               { 2, 0, 2, 0, 2, 0, 2, 0 },
                                               { 0, 2, 0, 0, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 2, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 1, 0, 1, 0, 1, 0, 1, 0 },
                                               { 0, 1, 0, 1, 0, 1, 0, 1 },
                                               { 1, 0, 1, 0, 1, 0, 1, 0 } };

            Assert.IsFalse(board.MakeMove(1, 3, 4, 2, 3));
        }
    }
}
