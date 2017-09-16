using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliddingPuzzle;
using System.Text;

namespace SliddingPuzzleTest
{
    [TestClass]
    public class PuzzleBoardTests
    {
        private PuzzleBoard board;

        [TestInitialize]
        public void Initialize()
        {
            board = new PuzzleBoard(3);
        }

        [TestMethod]
        public void Constructor_InitialConfiguration_ExpectedConfiguration()
        {
            int counter = 0;
            for (int y = 0; y < board.Size; ++y)
            {
                for (int x = 0; x < board.Size; ++x)
                {
                    Assert.AreEqual(counter, board.GetValue(x, y));
                    counter++;
                }
            }
        }

        [TestMethod]
        public void Shuffle_InitialConfiguration_NoException()
        {
            try
            {
                board.Shuffle();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        // Unreliable test
        [TestMethod]
        public void Shuffle_ForceSolveable_IsSolveable()
        {
            board.Shuffle(forceSolveable: true);

            Assert.IsTrue(board.IsSolveable(), "board state: " + board.ToString());
        }

        [TestMethod]
        public void MoveUp_InitialConfiguration_0and3SwapPlaces()
        {
            board.MoveUp();

            Assert.AreEqual(3, board.GetValue(0, 0));
            Assert.AreEqual(0, board.GetValue(0, 1));
        }

        [TestMethod]
        public void MoveDown_InitialConfiguration_NothingHappens()
        {
            board.MoveDown();

            Assert.AreEqual(0, board.GetValue(0, 0));
            Assert.AreEqual(3, board.GetValue(0, 1));
        }

        [TestMethod]
        public void MoveLeft_InitialConfiguration_0and1SwapPlaces()
        {
            board.MoveLeft();

            Assert.AreEqual(0, board.GetValue(1, 0));
            Assert.AreEqual(1, board.GetValue(0, 0));
        }

        [TestMethod]
        public void MoveRight_InitialConfiguration_NothingHappens()
        {
            board.MoveRight();
            
            Assert.AreEqual(0, board.GetValue(0, 0));
            Assert.AreEqual(1, board.GetValue(1, 0));
        }

        [TestMethod]
        public void CalculateInversion_InitialConfiguration_Returns0()
        {
            int inversion = board.CalculateInversions();

            Assert.AreEqual(0, inversion);
        }
                
        /* 3 1 2
         * 0 4 5
         * 6 7 8 */
        [TestMethod]
        public void CalculateInversion_3InFirstPosition_returns2()
        {
            board.MoveUp();

            int inversion = board.CalculateInversions();

            Assert.AreEqual(2, inversion);
        }

        /* 3 1 2
         * 6 4 5
         * 0 7 8 */
        [TestMethod]
        public void CalculateInversion_3InFirstPosition6InForthPosition_returns4()
        {
            board.MoveUp();
            board.MoveUp();

            int inversion = board.CalculateInversions();

            Assert.AreEqual(4, inversion);
        }

        [TestMethod]
        public void IsGoalState_InitialConfiguration_ReturnTrue()
        {
            Assert.IsTrue(board.IsGoalState());
        }

        [TestMethod]
        public void IsGoalState_SingleLeftSlide_ReturnTrue()
        {
            board.MoveLeft();

            Assert.IsTrue( board.IsGoalState());
        }

        [TestMethod]
        public void IsGoalState_SingleUpSlide_ReturnFalse()
        {
            board.MoveUp();

            Assert.IsFalse(board.IsGoalState());
        }

        [TestMethod]
        public void IsSolvable_InitialConfiguration_ReturnTrue()
        {
            Assert.IsTrue(board.IsSolveable());
        }

        [TestMethod]
        public void IsSolvable_InitialConfiguration4x4Board_ReturnTrue()
        {
            board = new PuzzleBoard(4);

            Assert.IsTrue(board.IsSolveable());
        }

        [TestMethod]
        public void IsSolvable_4x4BoardSingleMoveUp_ReturnTrue()
        {
            board = new PuzzleBoard(4);
            board.MoveUp();

            Assert.IsTrue(board.IsSolveable());
        }

        [TestMethod]
        public void CopyConstructor_SingleMoveUp_0And3SwapPlace()
        {
            board.MoveUp();

            PuzzleBoard copy = new PuzzleBoard(board);

            Assert.AreEqual(3, copy.GetValue(0, 0));
            Assert.AreEqual(0, copy.GetValue(0, 1));
        }

        [TestMethod]
        public void Equal_2InitialConfiguration_ReturnTrue()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(3);

            Assert.IsTrue(board.Equals(otherBoard));
        }

        [TestMethod]
        public void Equal_2DoubleUpMoveBoards_ReturnTrue()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(3);

            board.MoveUp();
            board.MoveUp();
            otherBoard.MoveUp();
            otherBoard.MoveUp();

            Assert.IsTrue(board.Equals(otherBoard));
        }

        [TestMethod]
        public void Equal_2UpLeftleftMoveBoards_ReturnTrue()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(3);

            board.MoveUp();
            board.MoveLeft();
            board.MoveLeft();

            otherBoard.MoveUp();
            otherBoard.MoveLeft();
            otherBoard.MoveLeft();

            Assert.IsTrue(board.Equals(otherBoard));
        }

        [TestMethod]
        public void Equal_CopyConstructor_ReturnTrue()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(board);
            
            Assert.IsTrue(board.Equals(otherBoard));
        }

        // Unreliable test
        [TestMethod]
        public void Equal_CopyConstructorAfterShuffle_ReturnTrue()
        {
            board.Shuffle();

            PuzzleBoard otherBoard = new PuzzleBoard(board);

            Assert.IsTrue(board.Equals(otherBoard), board.ToString() + "\n" + otherBoard.ToString());
        }

        [TestMethod]
        public void Equal_CopyConstructorSingleEmptyMove_ReturnTrue()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(board);
            otherBoard.MoveDown();

            Assert.IsTrue(board.Equals(otherBoard));
        }

        [TestMethod]
        public void ToString_InitialConfiguration_ReturnMatchingString()
        {
            StringBuilder expectedStringBuilder = new StringBuilder();
            expectedStringBuilder.AppendLine("0 1 2");
            expectedStringBuilder.AppendLine("3 4 5");
            expectedStringBuilder.Append("6 7 8");

            string actual = board.ToString();

            Assert.AreEqual(expectedStringBuilder.ToString(), actual);
        }

        [TestMethod]
        public void GetHashCode_InitialConfiguration_SameAsOther()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(3);

            Assert.AreEqual(board.GetHashCode(), otherBoard.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_AfterNullMove_SameAsOther()
        {
            PuzzleBoard otherBoard = new PuzzleBoard(3);
            otherBoard.MoveDown();

            Assert.AreEqual(board.GetHashCode(), otherBoard.GetHashCode());
        }
    }
}
