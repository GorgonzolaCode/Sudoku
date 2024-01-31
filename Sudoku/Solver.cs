using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Solver
{
    private Sudoku sudoku;

    private bool[,,] options = new bool[9,9,9];

    static StreamWriter logFile = File.CreateText("solver.log");

    /// <summary>
    /// Generates a solver for a specific sudoku.
    /// </summary>
    /// <param name="sudoku"></param>
    public Solver(Sudoku sudokuP)
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

        UpdateFull();
    }



    /// <summary>
    /// Updates the options array for solved cells.
    /// </summary>
    public void UpdateFull()
    {
        //update every not found cell, if it has only one option
        for (int i = 0; i < 81; i++ )
        {
            UpdateCell(i);
        }
    }



    /// <summary>
    /// Updates the options array for a single solved cell.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public void UpdateCell(int position)
    {
        Trace.WriteLine($"Updating cell at position {position}: ");
        int value = sudoku.Get(position % 9, position / 9);

        for (int i = 0; i < 9; i++ ) 
        {
            //set the option of the value to true, if there was a value (otherwise false)
            if ((value == 0) || (value - 1 == i))
            {
                options[position / 9, position % 9, i] = true;
                Trace.WriteLine($"possibility of {i} was set to true.");
                continue;
            }
            options[position / 9, position % 9, i] = false;

            Trace.WriteLine($"possibility of {i} was set to false.");
        }

        Trace.WriteLine("");
    }



    /// <summary>
    /// Returns the possible values of the cell at the position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public int[] GetOptions(int position)
    {
        List<int> result = new List<int>();

        //Trace.WriteLine($"Retrieving options at position {position}: ");

        for (int i = 0; i < 9; i++)
        {
            if (options[position / 9, position % 9, i])
            {
                result.Add(i+1);
            }
        }
        /*
        foreach (int i in result) Trace.Write(i + " ");
        Trace.WriteLine("");
        Trace.WriteLine("");
        */
        return result.ToArray();
    }



    public int[] GetOptions(int column, int row)
    {
        return GetOptions(row * 9 + column);
    }



    /// <summary>
    /// Changes the value of a cell.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="value"></param>
    public void SetCell(int position, int value)
    {
        sudoku.Set(position % 9, position / 9, value);
        UpdateCell(position);
    }



    public void SetCell(int column, int row, int value)
    {
        SetCell(row * 9 + column, value);
    }



    public bool CollapseFull()
    {
        bool changed = false;

        for (int i = 0; i < 81; i++)
        {
            if (sudoku.Get(i) == 0) changed = CollapseCell(i) || changed;
        }

        return changed;
    }



    private bool CollapseCell(int position)
    {
        int[] options = GetOptions(position);

        if (options.Length == 1)
        {
            SetCell(position, options[0]);
            return true;
        }

        return false;
    }



    private bool CollapseSingleCell(int column, int row)
    {
        return CollapseCell(row*9 + column);
    }





















}

