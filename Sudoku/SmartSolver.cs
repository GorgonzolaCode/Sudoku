using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SmartSolver : Solver
{
    public SmartSolver(Board board) : base(board) { }

    public SmartSolver(PossibilityMatrix matrix) : base(matrix) { }

    public SmartSolver(Solver solver) : base(solver) { }


    override public Solver GetCopy()
    {
        Board board = new Board(matrix.GetBoard());
        return new SmartSolver(board);
    }

    override public bool Solve()
    {
        List<int> unsolved = UpdateMatrix();

        return Solve(unsolved);
    }


    private bool Solve(List<int> unsolved)
    {
        List<int> positionsLeft = new List<int>();

        foreach (int position in unsolved)
        {
            //if the cell wasn't solved, add it to a list
            if (!CellSolve(position)) positionsLeft.Add(position);
        }

        if (positionsLeft.Count == 0) return true;
        if (positionsLeft.Count == unsolved.Count) return false;

        return Solve(positionsLeft);
    }


    override public bool CellSolve(int position)
    {
        if (matrix.FixCell(position))
        {
            //remove the option from all relevant cells
            int value = Get(position);

            for (int i = 0; i < 9; i++)
            {
                //row
                matrix.GetOptions(position / 9, i).Remove(value);

                //column
                matrix.GetOptions(i, position % 9).Remove(value);

                //block
                matrix.GetOptions(
                    Helper.GetBlockFirst(position) + i % 3 + i / 3 * 9
                    ).Remove(value);
            }

            //repair the options of the fixed cell
            matrix.GetOptions(position).Add(value);

            return true;
        }
        else return false;
    }


    /// <summary>
    /// Does setup for the matrix. Updates all options.
    /// </summary>
    /// <returns> Returns a list of unsolved positions. </returns>
    protected List<int> UpdateMatrix()
    {
        List<int> solved = matrix.FixAll();
        List<int> unsolved = Helper.InvertPositions(solved);
        List<int> options;

        foreach (int position in unsolved)
        {
            options = matrix.GetOptions(position);

            for (int i = 0; i < 9; i++)
            {
                //row
                options.Remove(matrix.Get(position / 9, i));

                //column
                options.Remove(matrix.Get(i, position % 9));

                //block
                options.Remove(matrix.Get(
                    Helper.GetBlockFirst(position) + i % 3 + i / 3 * 9
                    ));

                if (options.Count == 1) break;
            }
        }

        return unsolved;
    }





}

