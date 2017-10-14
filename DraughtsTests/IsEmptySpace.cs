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
            Assert.AreEqual(true, board.MakeMove(1, 0, 5, 1, 4));
        }

        [TestMethod]
        public void IsEmptySpace_MoveToNonEmpty_False()
        {
            Assert.AreEqual(false, board.MakeMove(1, 0, 7, 1, 6));
        }
    }
}
