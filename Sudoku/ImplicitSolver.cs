using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ImplicitSolver : SmartSolver
{
    public ImplicitSolver(Board board) : base(board) { }

    public ImplicitSolver(Solver solver) : base(solver) { }



    override public bool CellSolve(int position)
    {
        Console.WriteLine("...");
        if (base.CellSolve(position)) return true;
        
        int compPosition;

        //check if it's the only option for a number in the column
        List<int> options = new List<int>(matrix.GetOptions(position));
            
        for (int row = 0; row < 9; row++)
        {
            compPosition = row * 9 + position % 9;
            if (position != compPosition)
            {
                foreach (int compOption in GetOptions(compPosition))
                {
                    options.Remove(compOption);
                }
            }
        }
        if (base.CellSolve(position)) return true;


        //check if it's the only option for a number in the row
        options = new List<int>(matrix.GetOptions(position));

        for (int col = 0; col < 9; col++)
        {
            compPosition = position/9*9 + col;
            if (position != compPosition)
            {
                foreach (int compOption in GetOptions(compPosition))
                {
                    options.Remove(compOption);
                }
            }
        }
        if (base.CellSolve(position)) return true;


        //check if it's the only option for a number in the block
        options = new List<int>(matrix.GetOptions(position));

        for (int i = 0; i < 9; i++)
        {
            compPosition = Helper.GetBlockFirst(position) + i / 3 * 9 + i % 3;
            if (position != compPosition)
            {
                foreach (int compOption in GetOptions(compPosition))
                {
                    options.Remove(compOption);
                }
            }
        }
        if (base.CellSolve(position)) return true;


        return false;
    }


}

