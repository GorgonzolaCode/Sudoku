using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Remover
{
    private Solver solver;
    private Random random = new Random();


    public Remover(Solver solver) 
    { 
        this.solver = solver;
    }



    public void Minimize()
    {
        List<int> solved = solver.GetSolved();
        if (solved.Count == 0) return;
        int position;

        while (solved.Count > 0)
        {
            position = solved[random.Next(solved.Count)];
            RemoveCell(position);
            solved.Remove(position);
        }
        return;
    }


    private bool RemoveCell(int position)
    {
        Solver copy = solver.GetCopy();

        copy.Remove(position);
        if (copy.CellSolve(position))
        {
            solver.Remove(position);
            return true;
        }
        return false;

    }



    public void Draw(bool erasing)
    {
        solver.Draw(erasing);
    }

    public void FullShuffle()
    {
        solver.FullShuffle();
    }

    public void Set(int column, int row, int value)
    {
        solver.Set(row * 9 + column, value);
    }


}

