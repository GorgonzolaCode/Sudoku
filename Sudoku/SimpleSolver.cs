using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SimpleSolver : Solver
{


    public SimpleSolver(Board board) : base(board) { }

    public SimpleSolver(Solver solver) : base(solver) { }


    override public Solver GetCopy()
    {
        Board sudoku = new Board(matrix.GetBoard());
        return new SimpleSolver(sudoku);
    }

    public SimpleSolver(PossibilityMatrix matrix) : base(matrix) { }


    public bool Solve(List<int> cellsToCheck)
    {
        List<int> cellsLeft = new List<int>();

        foreach (int position in cellsToCheck) 
        {
            //if the cell wasn't solved, add it to a list
            if (!CellSolve(position)) cellsLeft.Add(position);
        }

        if (cellsLeft.Count == 0) return true;
        if (cellsLeft.Count == cellsToCheck.Count) return false;
        return Solve(cellsLeft);
    }



    override public bool Solve()
    {
        return Solve(matrix.GetUnsolved());
    }



    /// <summary>
    /// Solves a single cell by looking at its options.
    /// </summary>
    /// <param name="position"> position of the cell </param>
    /// <returns> Returns, whether the cell was solved. </returns>
    override public bool CellSolve(int position)
    { 
        List<int> options = matrix.GetOptions(position);

        for (int i = 0; i < 9; i++)
        {
            //row
            options.Remove(matrix.Get(position / 9, i));

            //column
            options.Remove(matrix.Get(i, position%9));

            //block
            options.Remove(matrix.Get(
                Helper.GetBlockFirst(position) + i%3 + i/3*9
                ));

            //test if finished
            if (matrix.FixCell(position)) return true;
        }

        return false;
    }


    override public bool IsSolvable()
    {
        Solver copy = GetCopy();
        if (copy.Solve()) return true;
        else return false;
    }












}

