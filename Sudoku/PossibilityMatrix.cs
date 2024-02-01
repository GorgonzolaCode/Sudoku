using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibilityMatrix
{
    private Sudoku sudoku;

    private List<int>[] options = new List<int>[81];

    static StreamWriter logFile = File.CreateText("solver.log");


    /// <summary>
    /// Generates a solver for a specific sudoku.
    /// </summary>
    /// <param name="sudoku"></param>
    public PossibilityMatrix(Sudoku sudokuP)
    {
        //Trace setup, as in tutorial
        Trace.Listeners.Add(new TextWriterTraceListener(logFile));
        Trace.AutoFlush = true;
        Trace.WriteLine("Starting Solver Log");
        Trace.WriteLine(String.Format("Started {0}", System.DateTime.Now.ToString()));

        Trace.WriteLine("");

        Trace.WriteLine("Given sudoku is:");
        Trace.WriteLine(sudokuP.ToString());
        sudoku = sudokuP;

        Trace.WriteLine("");

        UpdateFullOptions();
    }



    /// <summary>
    /// Updates the options array for all solved cells.
    /// </summary>
    private void UpdateFullOptions()
    {
        //update every not found cell, if it has only one option
        for (int i = 0; i < 81; i++ )
        {
            UpdateCellOptions(i);
        }
    }



    /// <summary>
    /// Changes options for a single cell, to either the cell content, or all numbers.
    /// </summary>
    /// <param name="position"></param>
    private void UpdateCellOptions(int position)
    {
        Trace.WriteLine($"Updating cell at position {position}: ");
        int value = sudoku.Get(position % 9, position / 9);

        //set the options to the value if definite
        if (value != 0)
        {
            options[position] = 
                new List<int>() { value };
            Trace.WriteLine($"The only option is {value}");
        }
        else
        {
            options[position] = 
                new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Trace.WriteLine("All values are possible.");
        }

        Trace.WriteLine("");
    }



    /// <summary>
    /// Returns the possible values of the cell at the position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public List<int> GetOptions(int position)
    { 
        return options[position];
    }



    public List<int> GetOptions(int column, int row)
    {
        return GetOptions(row * 9 + column);
    }



    /// <summary>
    /// Changes the options for a cell.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="optionsP"></param>
    /// <returns> Returns, whether there was a change. </returns>
    public bool SetOptions(int position, List<int> optionsP)
    {
        bool result = List<int>.Equals(optionsP, options[position]);
        options[position] = optionsP;
        return !result;
    }



    /// <summary>
    /// Changes the value of a cell. Leaves all options open for unsolved cells.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="value"></param>
    public void SetCell(int position, int value)
    {
        sudoku.Set(position % 9, position / 9, value);
        UpdateCellOptions(position);
    }



    public void SetCell(int column, int row, int value)
    {
        SetCell(row * 9 + column, value);
    }



    /// <summary>
    /// Returns, whether the sudoku has only one option in all cells.
    /// </summary>
    public bool IsSolved()
    {
        foreach (List<int> list in options)
        {
            if (list.Count != 1) return false;
        }
        
        return true;
    }



    /// <summary>
    /// Returns, whether there are no contradictions in the matrix.
    /// </summary>
    public bool IsCorrect()
    {
        for (int i = 0; i < 81; i++)
        {
            if (options[i].Count == 0) return false;
        }

        return sudoku.IsCorrect();
    }



    /// <summary>
    /// Fixes the value of cells with only one possible value.
    /// </summary>
    /// <returns> Returns, whether a cell was collapsed. </returns>
    public bool FixAll()
    {
        bool changed = false;

        for (int i = 0; i < 81; i++)
        {
            if (sudoku.Get(i) == 0) changed = (FixCell(i) || changed);
        }

        return changed;
    }



    /// <summary>
    /// Fixes the cell value in the sudoku if there is only one option.
    /// </summary>
    /// <param name="position"></param>
    /// <returns> Returns, whether the cell was collapsed. </returns>
    private bool FixCell(int position)
    {
        List<int> options = GetOptions(position);

        if (options.Count == 1)
        {
            sudoku.Set(position, options[0]);
            return true;
        }

        return false;
    }



    private bool FixCell(int column, int row)
    {
        return FixCell(row*9 + column);
    }

    /*

    /// <summary>
    /// Tries to solve the sudoku by using the standard rules.
    /// </summary>
    public void RuleSolveFull()
    {
        for (int i = 0; i < 81; i++)
        {
            CheckAddUnique(i);
        }

        while (cellsToCheck.Count > 0)
        {
            RuleSolveCell(cellsToCheck.First());
        }
    }



    /// <summary>
    /// Tries to solve a cell via default rules.
    /// </summary>
    /// <param name="position"></param>
    /// <returns> Returns, whether the cell was solved. </returns>
    private bool RuleSolveCell(int position)
    {
        if (sudoku.Get(position) != 0) return false;

        //reduce options via criteria
        if (RowSolve(position) && FixCell(position))
            {
                return true;
            }
        if (ColSolve(position) && FixCell(position))
            {
                return true;
            }
        if (BlockSolve(position) && FixCell(position))
            {
                return true;
            }

        return false;
    }



    private bool RuleSolveCell(int column, int row)
    {
        return RuleSolveCell(row*9 + column);
    }



    private bool RowSolve(int position)
    {
        List<int> optionsLeft = GetOptions(position);

        for (int i = 0; i < 9; i++)
        {
            optionsLeft.Remove(sudoku.Get(i, position/9));
        }

        return SetOptions(position, optionsLeft);
    }



    private bool ColSolve(int position)
    {
        List<int> optionsLeft = GetOptions(position);

        for (int i = 0; i < 9; i++)
        {
            optionsLeft.Remove(sudoku.Get(position%9, i));
        }

        return SetOptions(position, optionsLeft);
    }



    private bool BlockSolve(int position)
    {
        List<int> optionsLeft = GetOptions(position);

        for (int i = 0; i < 9; i++)
        {
            optionsLeft.Remove(sudoku.Get(
                //start-of-block column + column shift
                position%9/3*3 + i%3, 
                //start-of-block row    + row shift
                position/9/3*3 + i/3
                ));
        }

        return SetOptions(position, optionsLeft);
    }

    

    /// <summary>
    /// Adds all relevant cells to a to-do-list after changing one.
    /// </summary>
    /// <param name="position"></param>
    private void Consequence(int position)
    {
        //row
        for (int i = 0; i < 9; i++)
        {
            CheckAddUnique(position/9*9 + i);
        }
        //column
        for (int i = 0; i < 9; i++)
        {
            CheckAddUnique(position%9 + i*9);
        }
        //block
        for (int i = 0; i < 9; i++)
        {
            CheckAddUnique(
                Helper.getBlockFirst(position) +
                i % 3 +
                i / 3 * 9
                );
        }
    }



    private void CheckAddUnique(int position)
    {
        if (cellsToCheck.Contains(position)) return;
        cellsToCheck.Add(position);
    }


    */


}

