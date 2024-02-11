namespace Sudoku_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Default_Sudoku()
        {
            Board sudoku = new Board();

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
        public void Test_Template_Constructor()
        {
            Board sudoku_original = new Board();
            sudoku_original.FullShuffle();
            Board sudoku_copy = new Board(sudoku_original);

            for (int position = 0; position < 81; position++)
            {
                Assert.AreEqual(sudoku_original.Get(position), sudoku_copy.Get(position));
                sudoku_copy.Set(position, 0);
                Assert.AreNotEqual(sudoku_original.Get(position), sudoku_copy.Get(position));
            }
        }


        [TestMethod]
        public void Test_First_Shuffle_Validity()
        {
            Board sudoku;

            for (int i = 0; i < 20; i++)
            {
            sudoku = new Board();
            sudoku.FullShuffle();

            Assert.IsTrue(sudoku.IsCorrect());
            }
        }


        [TestMethod]
        public void Test_Continuous_Shuffle_Validity()
        {
            Board sudoku = new Board();

            for (int i = 0; i < 20; i++)
            {
                sudoku.FullShuffle();

                Assert.IsTrue(sudoku.IsCorrect());
            }
        }



        [TestMethod]
        public void Test_Matrix_Constructor()
        {
            Board sudoku = new Board();
            PossibilityMatrix matrix;
            List<int> options;

            for (int j = 0; j < 20; j++)
            {
                sudoku.FullShuffle();
                matrix = new PossibilityMatrix(sudoku);

                    for (int i = 0; i < 81; i++)
                    {
                        options = matrix.GetOptions(i);

                        //do fixed cells return only their value
                        Assert.AreEqual(options.Count, 1);
                        //does the matrix represent the sudoku
                        Assert.AreEqual(sudoku.Get(i % 9, i / 9), options[0]);
                    }
            }

            
        }



        [TestMethod]
        public void Test_Matrix_Update()
        {
            Board sudoku;
            PossibilityMatrix matrix;

            for (int i = 0; i < 20; i++)
            {
                sudoku = new Board();
                sudoku.FullShuffle();
                matrix = new PossibilityMatrix(sudoku);

                Assert.AreEqual(matrix.GetOptions(0).Count, 1);
                matrix.Set(0, 0, 0);
                Assert.AreEqual(matrix.GetOptions(0).Count, 9);

            }
        }



        [TestMethod]
        public void Test_Matrix_Fix_Solved()
        {
            Board sudoku = new Board();
            sudoku.FullShuffle();
            PossibilityMatrix matrix = new PossibilityMatrix(sudoku);

            //no changes when solved
            Assert.AreEqual(matrix.FixAll().Count, 0);
        }



        [TestMethod]
        public void Test_Matrix_Fix_Missing()
        {
            Board sudoku = new Board();
            sudoku.FullShuffle();
            PossibilityMatrix matrix = new PossibilityMatrix(sudoku);

            List<int> values = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                values.Add(matrix.GetOptions(i)[0]);
                matrix.Set(i, 0);
                matrix.SetOptions(i, new List<int> { values[i] });

                Assert.AreEqual(sudoku.Get(i), 0);
            }

            matrix.FixAll();

            for (int i = 0; i < 9; i++)
            {
                Assert.AreEqual(sudoku.Get(i), values[i]);
            }


        }



        [TestMethod]
        public void Test_Helper_First_Position()
        {
            for (int blockrow = 0; blockrow < 3; blockrow++)
            {
                for (int blockcolumn = 0; blockcolumn < 3; ++blockcolumn)
                {
                    for (int row = 0; row < 3; row++)
                    {
                        for (int column = 0; column < 3; column++)
                        {
                            int position =
                                blockrow * 27 +
                                blockcolumn * 3 +
                                row * 9 +
                                column;
                            Assert.AreEqual(
                                Helper.GetBlockFirst(position),
                                blockrow*27 + blockcolumn*3
                                );
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void Test_Helper_Inverter()
        {
            List<int> positions = new List<int>();
            List<int> inverted = Helper.InvertPositions(positions);
            Assert.AreEqual(positions.Count, 0);
            Assert.AreEqual(inverted.Count, 81);

            for (int i = 0; i < 12; i++)
            {
                positions.Add(i);
            }
            inverted = Helper.InvertPositions(positions);

            Assert.AreEqual(positions.Count, 12);
            Assert.AreEqual(inverted.Count, 69);
        }


        [TestMethod]
        public void Test_Simple_Solver()
        {
            //generate solver with unsolved sudoku
            Board sudoku = new Board();
            sudoku.FullShuffle();
            sudoku = GenerateExampleSudoku(sudoku, "easy");
            Solver solver = new SimpleSolver(sudoku);

            if (solver.Solve())
            {
                Assert.IsTrue(solver.IsSolved());
            }
            Assert.IsTrue(solver.IsCorrect());

            
        }


        [TestMethod]
        public void Test_Simple_Solver_Unordered()
        {
            Board sudoku = new Board();
            sudoku = GenerateExampleSudoku(sudoku, "easy");
            sudoku.FullShuffle();
            Solver solver = new SimpleSolver(sudoku);

            if (solver.Solve())
            {
                Assert.IsTrue(solver.IsSolved());
            }
            Assert.IsTrue(solver.IsCorrect());
        }


        [TestMethod]
        public void Test_Remover_Solve_Simple()
        {
            Board board = new Board();
            Remover remover = new Remover(new SimpleSolver(board));

            remover.FullShuffle();
            remover.Minimize();

            Assert.IsTrue(remover.IsSolvable());

            remover.Solve();
            Assert.IsTrue(remover.IsSolved());
        }


        [TestMethod]
        public void Time_Minimize_Simple()
        {
            Board board = new Board();
            Remover remover = new Remover(new SimpleSolver(board));
            for (int i = 0; i < 10; i++)
            {
                remover.Minimize();
                remover.Solve();
            }
            Assert.IsTrue(true);
        }
        //first version: 2,6 min (cell solve)
        //second version: 3,2 min/5,6 min  (cell solve and full solve)


        [TestMethod]
        public void Test_Remover_Solve_Smart()
        {
            Board board = new Board();
            Remover remover = new Remover(new SmartSolver(board));

            remover.FullShuffle();
            remover.Minimize();

            Assert.IsTrue(remover.IsSolvable());

            remover.Solve();
            Assert.IsTrue(remover.IsSolved());
        }

        [TestMethod]
        public void Time_Minimize_Smart()
        {
            Board board = new Board();
            Remover remover = new Remover(new SmartSolver(board));
            for (int i = 0; i < 10; i++)
            {
                remover.Minimize();
                remover.Solve();
            }
            Assert.IsTrue(true);
        }
        //5,2 min




















        private Board GenerateExampleSudoku(Board sudoku, string difficulty)
        {
            switch (difficulty)
            {
                default: return GenerateExampleSudoku_Easy(sudoku);
                    break;
            }
        }


        private Board GenerateExampleSudoku_Easy(Board sudoku)
        {
            //delete row
            for (int i = 0; i < 9; i++)
            {
                sudoku.Set(i, 5, 0);
            }
            //delete column
            for (int i = 0; i < 9; i++)
            {
                sudoku.Set(5, i, 0);
            }
            


            return sudoku;
        }

    }
}