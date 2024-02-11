using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public abstract class Solver
{


    protected PossibilityMatrix matrix;


    public Solver(Board sudoku)
    {
        matrix = new PossibilityMatrix(sudoku);
    }

    public Solver(PossibilityMatrix matrix)
    {
        this.matrix = matrix;
    }




    public Solver(Solver solver)
    {
        matrix = new PossibilityMatrix(solver.GetBoard());
    }

    abstract public Solver GetCopy();


    /// <summary>
    /// Tries to solve the sudoku.
    /// </summary>
    /// <returns> Returns, whether the sudoku was solved. </returns>
    abstract public bool Solve();

    abstract public bool CellSolve(int position);


    public int Get(int position)
    {
        return matrix.Get(position);
    }

    public void Set(int position, int value)
    {
        matrix.Set(position, value);
    }


    public bool IsSolved()
    {
        return matrix.IsSolved();
    }


    public bool IsCorrect()
    {
        return matrix.IsCorrect();
    }



    public List<int> GetSolved() 
    {
        List<int>  unsolved = matrix.GetUnsolved();
        return Helper.InvertPositions(unsolved);
    }



    public void Remove(int position)
    {
        matrix.Set(position, 0);
    }



    public Board GetBoard()
    {
        return matrix.GetBoard();
    }

    public List<int> GetUnsolved()
    {
        return matrix.GetUnsolved();
    }

    public List<int> GetOptions(int position)
    {
        return matrix.GetOptions(position);
    }

    public bool SetOptions(int position, List<int> options)
    {
        return matrix.SetOptions(position, options);
    }

    public void FixCell(int position)
    {
        matrix.FixCell(position);
    }

    public void Draw(bool erasing)
    {
        matrix.Draw(erasing);
        if (IsSolvable()) Console.WriteLine("The sudoku can be solved by the algorithm. Just type 'solve'.");
        else Console.WriteLine("The algorithm can't solve the sudoku.");
    }

    public override string ToString()
    {
        return matrix.ToString();
    }

    public void FullShuffle()
    {
        matrix.FullShuffle();
    }

    /// <summary>
    /// Tells, if the Solver can solve the sudoku.
    /// </summary>
    /// <returns></returns>
    public bool IsSolvable()
    {
        Solver copy = GetCopy();
        return copy.Solve();
    }




}

