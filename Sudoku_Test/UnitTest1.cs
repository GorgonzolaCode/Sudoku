namespace Sudoku_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Default_Sudoku()
        {
            Sudoku sudoku = new Sudoku();

            int[,] board = new int[,]{
            { 1, 2, 3, 4, 5, 6, 7, 8, 9},
            { 7, 8, 9, 1, 2, 3, 4, 5, 6},
            { 4, 5, 6, 7, 8, 9, 1, 2, 3},
            { 9, 1, 2, 3, 4, 5, 6, 7, 8},
            { 6, 7, 8, 9, 1, 2, 3, 4, 5},
            { 3, 4, 5, 6, 7, 8, 9, 1, 2},
            { 8, 9, 1, 2, 3, 4, 5, 6, 7},
            { 5, 6, 7, 8, 9, 1, 2, 3, 4},
            { 2, 3, 4, 5, 6, 7, 8, 9, 1}
           };


            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Assert.AreEqual(sudoku.Get(col, row), board[row, col]);
                }
            }

        }

    


        [TestMethod]
        public void Test_First_Shuffle_Validity()
        {
            Sudoku sudoku;

            for (int i = 0; i < 100; i++)
            {
            sudoku = new Sudoku();
            sudoku.FullShuffle();

            Assert.IsTrue(sudoku.IsCorrect());
            }
        }


        [TestMethod]
        public void Test_Continuous_Shuffle_Validity()
        {
            Sudoku sudoku = new Sudoku();

            for (int i = 0; i < 100; i++)
            {
                sudoku.FullShuffle();

                Assert.IsTrue(sudoku.IsCorrect());
            }
        }


    }
}