using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public abstract class Solver
{


    protected PossibilityMatrix matrix;


    public Solver(Sudoku sudoku)
    {
        matrix = new PossibilityMatrix(sudoku);
    }



    /// <summary>
    /// Tries to solve the sudoku.
    /// </summary>
    /// <returns> Returns, whether the sudoku was solved. </returns>
    public abstract bool Solve();



    public int Get(int position)
    {
        return matrix.Get(position);
    }

    public bool IsSolved()
    {
        return matrix.IsSolved();
    }


    public bool isCorrect()
    {
        return matrix.IsCorrect();
    }





}

