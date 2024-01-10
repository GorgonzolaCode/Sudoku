



//create a new and shuffled sudoku
Sudoku sudoku = new Sudoku();
sudoku.FullShuffle();




while (true)
{
    sudoku.Draw();


    
    //get a command
    string command = GetCommand();

    int[] cmd = RetrieveThreeNumbers(command);


    //execute command
    if ((cmd != null) && (cmd[0] > 0) && (cmd[1] > 0))
    {
        sudoku.Set(cmd[0]-1, cmd[1]-1, cmd[2]);
    }
    
}










string GetCommand()
{

    Console.WriteLine("\n Enter a command to change one cell in the format of [column, row, new number].\n Rows and columns start at 1.\n");
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
