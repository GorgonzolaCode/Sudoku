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



        [TestMethod]
        public void Test_Solver_Constructor()
        {
            Sudoku sudoku = new Sudoku();
            Solver solver;
            int[] options;

            for (int j = 0; j < 100; j++)
            {
                sudoku.FullShuffle();
                solver = new Solver(sudoku);

                    for (int i = 0; i < 81; i++)
                    {
                        options = solver.GetOptions(i);

                        //do fixed cells return only their value
                        Assert.AreEqual(options.Length, 1);
                        //does the solver represent the sudoku
                        Assert.AreEqual(sudoku.Get(i % 9, i / 9), options[0]);
                    }
            }

            
        }



        [TestMethod]
        public void Test_Solver_Update()
        {
            Sudoku sudoku;
            Solver solver;

            for (int i = 0; i < 100; i++)
            {
                sudoku = new Sudoku();
                sudoku.FullShuffle();
                solver = new Solver(sudoku);

                Assert.AreEqual(solver.GetOptions(0).Length, 1);
                solver.SetCell(0, 0, 0);
                Assert.AreEqual(solver.GetOptions(0).Length, 9);

            }
        }



        [TestMethod]
        public void Test_Solver_Collapse_Solved()
        {
            Sudoku sudoku = new Sudoku();
            sudoku.FullShuffle();
            Solver solver = new Solver(sudoku);

            Assert.AreEqual(solver.CollapseFull(), false);
        }



        [TestMethod]
        public void Test_Solver_Collapse_Solvable()
        {
            Sudoku sudoku = new Sudoku();
            Solver solver;

            for (int i = 0; i < 100; i++)
            {
                sudoku.FullShuffle();
                solver = new Solver(sudoku);

                //TODO
            }

        }


















    }
}