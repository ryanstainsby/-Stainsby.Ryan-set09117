using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DraughtsGame;

namespace DraughtsTests
{
    [TestClass]
    public class IsEmptySpace
    {
        Board board = new Board();

        [TestMethod]
        public void IsEmptySpace_MoveToEmpty_True()
        {
            Move move = new Move(5, 0, 4, 1);
            Assert.IsTrue(board.MakeMove(1, move));
        }

        [TestMethod]
        public void IsEmptySpace_MoveToNonEmpty_False()
        {
            Move move = new Move(7, 0, 6, 1);
            Assert.IsFalse(board.MakeMove(1, move));
        }
    }
}
