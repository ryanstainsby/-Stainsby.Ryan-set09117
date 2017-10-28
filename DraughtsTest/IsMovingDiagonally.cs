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
            Move move = new Move(5, 0, 4, 1);
            Assert.IsTrue(board.MakeMove(1, move));
        }

        [TestMethod]
        public void IsMovingDiagonally_BlackMovingDiagonally_True()
        {
            Move move = new Move(2, 1, 3, 0);
            Assert.IsTrue(board.MakeMove(2, move));
        }

        [TestMethod]
        public void IsMovingDiagonally_WhiteMovingForward_False()
        {
            Move move = new Move(5, 2, 4, 2);
            Assert.IsFalse(board.MakeMove(1, move));
        }

        public void IsMovingDiagonally_BlackMovingForward_False()
        {
            Move move = new Move(2, 3, 3, 3);
            Assert.IsFalse(board.MakeMove(2, move));
        }

        [TestMethod]
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

            Move move = new Move(4, 3, 5, 2);
            Assert.IsFalse(board.MakeMove(1, move));
        }

        [TestMethod]
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

            Move move = new Move(3, 4, 2, 5);
            Assert.IsFalse(board.MakeMove(1, move));
        }

        [TestMethod]
        public void IsMovingDiagonally_WhiteKingMovingDiagonallyBackwards_True()
        {
            board.piecePositions = new int[,]{ { 0, 3, 0, 2, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 2, 0, 2, 0 },
                                               { 0, 0, 0, 2, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 1, 0, 0, 0, 0 },
                                               { 1, 0, 0, 0, 1, 0, 1, 0 },
                                               { 0, 1, 0, 1, 0, 1, 0, 1 },
                                               { 1, 0, 1, 0, 1, 0, 1, 0 } };

            Move move = new Move(0, 1, 1, 0);
            Assert.IsTrue(board.MakeMove(1, move));
        }

        [TestMethod]
        public void IsMovingDiagonally_BlackKingMovingDiagonallyBackwards_True()
        {
            board.piecePositions = new int[,]{ { 0, 2, 0, 2, 0, 2, 0, 2 },
                                               { 2, 0, 2, 0, 2, 0, 2, 0 },
                                               { 0, 2, 0, 2, 0, 2, 0, 2 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 1, 0, 1, 0, 1, 0 },
                                               { 0, 0, 0, 1, 0, 1, 0, 1 },
                                               { 4, 0, 1, 0, 1, 0, 1, 0 } };

            Move move = new Move(7, 0, 6, 1);
            Assert.IsTrue(board.MakeMove(2, move));
        }
    }
}
