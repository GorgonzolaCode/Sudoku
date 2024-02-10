
int[]? cmd;
bool erasing = true;

//create a new and shuffled sudoku
Board board = new Board();
Remover sudoku = new Remover(new SimpleSolver(board));
sudoku.FullShuffle();




while (true)
{
    sudoku.Draw(erasing);



    //get a command
    cmd = null;
    string command = GetCommand();
    command.Trim();

    if (command.StartsWith("unsolve", StringComparison.OrdinalIgnoreCase)) 
        sudoku.Minimize();
    else if (command.StartsWith("solve", StringComparison.OrdinalIgnoreCase))
        sudoku.Solve();
    else cmd = RetrieveThreeNumbers(command);

    

    //execute command
    if ((cmd != null) && (cmd[0] > 0) && (cmd[1] > 0))
    {
        sudoku.Set(cmd[0]-1, cmd[1]-1, cmd[2]);
    }
    
}










string GetCommand()
{

    Console.WriteLine("\n " +
        "Enter a command to change one cell in the format of [column, row, new number].\n" +
        "Rows and columns start at 1, count starts in the upper left corner.\n" +
        "Type 'unsolve' to reduce the sudoku as much as possible.\n");
    return Console.ReadLine();

}


// retrieves three digits from a string and returns them in an int array, or null if there were not enough numbers
int[] RetrieveThreeNumbers(string command)
{
    int[] result = { -1, -1, -1 };
    int counter = 0;

    while (command.Length  > 0 && counter < 3)
    {
        if (Char.IsNumber(command.First()))
        {
            result[counter++] = (int)Char.GetNumericValue(command.First());
        }
        
        command = command.Substring(1);
        
    }
    //if there were not enough numbers, return null
    if (result[2] == -1) { return null; }

    return result;
}











Board GenerateExampleSudoku_Easy(Board sudoku)
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
    //delete block
    for (int i = 0; i < 9; i++)
    {
        sudoku.Set(i%3 + i/3*9, 0);
    }

    return sudoku;
}
