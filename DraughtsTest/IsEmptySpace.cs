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
            Assert.IsTrue(board.MakeMove(1, 5, 0, 4, 1));
        }

        [TestMethod]
        public void IsEmptySpace_MoveToNonEmpty_False()
        {
            Assert.IsFalse(board.MakeMove(1, 7, 0, 6, 1));
        }
    }
}
