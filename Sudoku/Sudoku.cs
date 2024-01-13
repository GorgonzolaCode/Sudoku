using System.Collections;
using System.Diagnostics;





public class Sudoku
{

    private int[,] board;
    private bool shuffled = false;



    /// <summary>
    /// Creates a default sudoku.
    /// </summary>
    public Sudoku()
    {
        //code from a tutorial for setting up a log
        StreamWriter logFile = File.CreateText("sudoku.log");
        Trace.Listeners.Add(new TextWriterTraceListener(logFile));
        Trace.AutoFlush = true;
        Trace.WriteLine("Starting Sudoku Log");
        Trace.WriteLine(String.Format("Started {0}", System.DateTime.Now.ToString()));



        //create a default board
        board = new int[,]{
            { 1, 2, 3, 4, 5, 6, 7, 8, 9},
            { 7, 8, 9, 1, 2, 3, 4, 5, 6},
            { 4, 5, 6, 7, 8, 9, 1, 2, 3},
            { 9, 1, 2, 3, 4, 5, 6, 7, 8},
            { 6, 7, 8, 9, 1, 2, 3, 4, 5},
            { 3, 4, 5, 6, 7, 8, 9, 1, 2},
            { 8, 9, 1, 2, 3, 4, 5, 6, 7},
            { 5, 6, 7, 8, 9, 1, 2, 3, 4},
            { 2, 3, 4, 5, 6, 7, 8, 9, 1}
           };

        Trace.WriteLine("Sudoku was created: ");
        Trace.WriteLine(ToString());
    }


    /// <summary>
    /// Tells if a sudoku has no contradictions.
    /// </summary>
    /// <returns> A boolean <c>true</c>, if there were no obvious contradictions. Otherwise a boolean <c>false</c>.</returns>
    public bool IsCorrect()
    {
        int temp;
        bool[] found;
        bool result = true;

        
        
        for (int row = 0; result && row < 9; row++)
        {
            found = new bool[9] { false, false, false,
                                  false, false, false,
                                  false, false, false };

            
            for (int col = 0; col < 9; col++)
            {
                temp = board[row, col] - 1;

                //skip empty cells
                if (temp < 0) continue;

                //check if number was already used
                if (found[temp])
                {
                    result = false;

                    Trace.WriteLine(String.Format("Contradiction in column {0}, row {1}: " +
                        "Number twice in row.", col + 1, row + 1));

                    continue;
                }
                found[temp] = true;
            }
        }


        
        for (int col = 0; result && col < 9; col++)
        {
            found = new bool[9] { false, false, false,
                                  false, false, false,
                                  false, false, false };

            
            for (int row = 0; row < 9; row++)
            {
                temp = board[row, col] - 1;

                //skip empty cells
                if (temp < 0) continue;

                //check if number was already used
                if (found[temp])
                {
                    result = false;

                    Trace.WriteLine(String.Format("Contradiction in column {0}, row {1}: " +
                        "Number twice in column.", col + 1, row + 1));

                    continue;
                }
                found[temp] = true;
            }
        }


        
        for (int block = 0; result && block < 9; block++)
        {
            found = new bool[9] { false, false, false,
                                  false, false, false,
                                  false, false, false };

            
            for (int cell = 0; cell < 9; cell++)
            {
                temp = board[(block/3) * 3 + cell/3, (block%3) * 3 + cell%3] - 1;

                //skip empty cells
                if (temp < 0) continue;

                //check if number was already used
                if (found[temp])
                {
                    result = false;

                    Trace.WriteLine(String.Format("Contradiction in column {0}, row {1}: " +
                        "Number twice in block.", (block%3)*3 + cell%3 + 1, (block/3)*3 + cell/3 + 1));

                    continue;
                }
                found[temp] = true;
            }
        }

        if (result) Trace.WriteLine("No contradictions found in the sudoku. ");
        Trace.WriteLine(ToString());
        return result;
    }


    /// <summary>
    /// Returns the value of a cell. Starts counting at 0.
    /// </summary>
    /// <param name="col"> column </param>
    /// <param name="row"> row </param>
    /// <returns> the value of a cell </returns>
    public int Get(int col, int row)
    {
        return board[row, col];
    }




    /// <summary>
    /// Sets the value of a coll. Starts counting at 0.
    /// </summary>
    /// <param name="col"> column </param>
    /// <param name="row"> row </param>
    /// <param name="value"> new value of a cell </param>
    public void Set(int col, int row, int value)
    {
        Trace.WriteLine(String.Format("Column {0}, row {1} was changed from {2} to {3}. ", 
            col + 1, row + 1, board[row, col], value));

        board[row, col] = value;
    }



    /// <summary>
    /// Clears console and draws the board. Tells if the sudoku is still coherent.
    /// </summary>
    public void Draw()
    {
        Console.Clear();

        Console.WriteLine(ToString());

        if (IsCorrect()) Console.WriteLine("\n The sudoku has no contradictions.");
        else Console.WriteLine("!!!The sudoku has a contradiction!!!");

        Trace.WriteLine("The sudoku was drawn: ");
        Trace.WriteLine(ToString());
    }


    /// <summary>
    /// Returns a string representation of the sudoku.
    /// </summary>
    /// <returns> a string representation with borders </returns>
    override public string ToString()
    {
        string result = "";

        for (int row = 0; row < board.GetLength(0); row++)
        {
            //horizontal divider
            if (row % 3 == 0) result += "+_________+_________+_________+\n";


            //first vertical divider
            result += "| ";



            for (int col = 0; col < board.GetLength(1); col++)
            {

                //number
                result += board[row, col];


                //vertical divider
                if ((col + 1) % 3 == 0)
                {
                    result += " | ";
                }
                else
                {
                    result += ", ";
                }

            }
            result += "\n";
        }


        //last horizontal divider
        result += "+_________+_________+_________+\n";

        return result;
    }


    

    /// <summary>
    /// Uses all viable methods to shuffle a sudoku.
    /// </summary>
    public void FullShuffle()
    {
        SwapNumbers();
        PartialSwap();
        Shuffle();
    }




    /// <summary>
    /// Swaps the numbers of the sudoku around. Doesn't change the fundamental layout.
    /// </summary>
    private void SwapNumbers()
    {
        int[] numbers = Helper.GetNumbers();
        int temp;

        for (int i = 0; i < 81; i++)
        {
            temp = board[i % 9, i / 9];

            //skip empty cells
            if (temp == 0) continue;

            //swap each cell with its new value
            board[i % 9, i / 9] = numbers[temp-1];
        }

        Trace.WriteLine(String.Format("Numbers were changed to {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5], numbers[6], numbers[7], numbers[8]));
        Trace.WriteLine(ToString());
    }




    /// <summary>
    /// Swaps around rows and columns partially.
    /// </summary>
    private void PartialSwap()
    {
        PartialSwapRow();
        PartialSwapCol();
    }




    /// <summary>
    /// Swaps around partial rows in blocks (only before shuffling).
    /// </summary>
    private void PartialSwapRow()
    {
        //don't execute if the sudoku has already been shuffled
        if (shuffled) return;

        int[,] shuffle = Helper.GetShuffleNumbers();
        int[] temp = new int[3];
        int position = 0;

        
        for (int blockrow = 0; blockrow < 3; blockrow++)
        {
            
            for (int column = 0; column < 9; column++)
            {
                //save the column
                for (int row = 0; row < 3; row++)
                {
                    temp[row] = board[blockrow * 3 + row, column];
                }

                

                //overwrite column with new order
                for (int row = 0; row < 3; row++)
                {
                    position = shuffle[(blockrow * 3) + (column % 3), row];
                    board[blockrow * 3 + row, column] = temp[position];
                }


                //describe the change
                Trace.Write(String.Format("Blockrow {0}, column {1} was ", blockrow + 1, column + 1));

                switch (shuffle[(blockrow * 3) + (column % 3), 0])
                {
                    case 1:
                        //if it is a cycle
                        if (shuffle[(blockrow * 3) + (column % 3), 1] == 2)
                        {
                            Trace.WriteLine("shifted up once.");
                        }
                        //if it is a swap
                        else
                        {
                            Trace.WriteLine("swapped in the first and second row.");
                        }
                        break;

                    case 2:
                        //if it is a cycle
                        if (shuffle[(blockrow * 3) + (column % 3), 2] == 1)
                        {
                            Trace.WriteLine("shifted down once.");
                        }
                        //if it is a swap
                        else
                        {
                            Trace.WriteLine("swapped in the first and third row.");
                        }
                        break;

                    default:
                        //if it is unchanged
                        if (shuffle[(blockrow * 3) + (column % 3), 1] == 1)
                        {
                            Trace.WriteLine("left untouched.");
                        }
                        //if it is a swap
                        else
                        {
                            Trace.WriteLine("swapped in the second and third row.");
                        }
                        break;
                }

                Trace.WriteLine(ToString());
            }
        }

        //set the flag so this method won't be executed again
        shuffled = true;
    }




    /// <summary>
    /// Searches for and then swaps around partial columns.
    /// </summary>
    private void PartialSwapCol()
    {
        bool[] usedRows;
        ArrayList possibleSwaps;
        int[] arguments = new int[0];

        
        for (int blockCol = 0; blockCol < 3; blockCol++)
        {
            usedRows = new bool[] { false, false, false, false, false, false, false, false, false };

            possibleSwaps = PossibleSwapCol(blockCol);

            //for every possible partial swap in the block column
            for (int i = 0; i < possibleSwaps.Count; i++)
            {
                //shuffle only half the time
                if (Helper.GetRandomBool()) continue;


                arguments = (int[])possibleSwaps[i];


                if (Helper.AreNumbersUsed(usedRows, arguments)) continue;

                //execute swap and update the array of used rows
                PartialSwapCol(arguments);
                usedRows[arguments[2]] = true;
                usedRows[arguments[3]] = true;
                usedRows[arguments[4]] = true;
            }
        }
    }




    /// <summary>
    /// Swaps around partial columns.
    /// Takes arguments in the form of:
    /// 
    /// <code>{ first column, second column, first row, second row, third row }</code>
    /// 
    /// Reorders board accordingly.
    /// </summary>
    /// <param name="arguments"> { first column, second column, first row, second row, third row } </param>
    private void PartialSwapCol(int[] arguments)
    {
        int[] columns = { arguments[0], arguments[1] };
        int[] rows = { arguments[2], arguments[3], arguments[4] };
        int temp;


        foreach (int row in rows)
        {
            //swap the columns
            temp = board[row, columns[0]];
            board[row, columns[0]] = board[row, columns[1]];
            board[row, columns[1]] = temp;
        }

        Trace.WriteLine(String.Format("Column {0} and {1} were swapped in rows {2}, {3}, and {4}.", columns[0] + 1, columns[1] + 1, rows[0] + 1, rows[1] + 1, rows[2] + 1 ));
        Trace.WriteLine(ToString());
    }




    /// <summary>
    /// Returns an ArrayList of which parts of which columns can be used for a partial swap.
    /// <code> { first column, second column, first row, second row, third row } </code>
    /// </summary>
    /// <param name="blockcol"> the block column in focus </param>
    /// <returns> ArrayList of int[]{ first column, second column, first row, second row, third row } </returns> 
    private ArrayList PossibleSwapCol(int blockcol)
    {
        ArrayList result = new ArrayList();

        
        ArrayList matchingArray;
        ArrayList foundArray;
        int[] firstBlockRow;
        int[] secondBlockRow;
        int[] thirdBlockRow;
        int[] partners = new int[] { 0, 0 };
        int[] matchingPositions;
        int[] construct;
        int[] foundFirstPair;
        int[] foundSecondPair;



        
        for (int startRow = 0; startRow < 3; startRow++)
        {

            
            firstBlockRow = GetBlockRow(blockcol, 0, startRow);

            
            for (int compareRow = 0; compareRow < 3; compareRow++)
            {
                
                secondBlockRow = GetBlockRow(blockcol, 1, compareRow);

                //get the position of any matching numbers
                matchingArray = Helper.SharedNumbers(firstBlockRow, secondBlockRow);


                //for each pair of matching numbers
                for (int matching = 0; matching < matchingArray.Count; matching++)
                {
                    matchingPositions = (int[])matchingArray[matching];

                    //save the partner numbers
                    partners[0] = board[startRow, matchingPositions[1] + blockcol*3];
                    partners[1] = board[compareRow + 3, matchingPositions[0] + blockcol * 3];



                    
                    for (int finalRow = 0; finalRow < 3; finalRow++)
                    {
                        
                        thirdBlockRow = GetBlockRow(blockcol, 2, finalRow);

                        //get the position of any matching numbers
                        foundArray = Helper.SharedNumbers(partners, thirdBlockRow);


                        //if there were not 2 matches: it didn't work
                        if (foundArray.Count < 2) continue;

                        
                        foundFirstPair = (int[])foundArray[0];
                        foundSecondPair = (int[])foundArray[1];

                        //check if the numbers in the third block are in the same column as the numbers from the previous two blocks
                        if ((foundFirstPair[1] != matchingPositions[0])
                            ||  
                            (foundSecondPair[1] != matchingPositions[1]) )
                        {
                            //if not, continue
                            continue;
                        }

                        //add the combination as a construct to the result
                        construct = Helper.ConstructResult(matchingPositions, blockcol, startRow, compareRow, finalRow);
                        result.Add(construct);

                    }
                }
                
            }
        }


        return result;
    }




    /// <summary>
    /// Shuffles the sudoku by swapping rows and columns within blocks, and swapping block rows and block columns.
    /// </summary>
    private void Shuffle()
    {

        //generate shuffle instructions
        int[,] shuffles = Helper.GetShuffleNumbers();
        int used = 0;


        used = ShuffleRows(used, shuffles);

        used = ShuffleColumns(used, shuffles);

        used = ShuffleBlockRows(used, shuffles);

        used = ShuffleBlockColumns(used, shuffles);

    }



    /// <summary>
    /// Swaps rows within all block rows.
    /// </summary>
    /// <param name="used"> how many of the shuffles were used </param>
    /// <param name="shuffles"> list of possible shuffles </param>
    /// <returns> the updated 'used' number </returns>
    private int ShuffleRows(int used, int[,] shuffles)
    {

        int[,] temp = { 
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 } 
        };


        
        for (int blockRow = 0; blockRow < 3; blockRow++)
        {

            
            for (int row = 0; row < 3; row++)
            {

                //do nothing with row if it already is in the right place
                if (row == shuffles[used, row]) continue;


                //save the row if it could be used later
                if (row < 2)
                {
                    //copy the row into temp
                    for (int column = 0; column < 9; column++)
                    {
                        temp[row, column] = board[blockRow * 3 + row, column];
                    }
                }

                //overwrite the row...
                if (shuffles[used, row] > row)
                {
                    //... with a later row
                    for (int column = 0; column < 9; column++)
                    {
                        board[blockRow * 3 + row, column] = board[blockRow * 3 + shuffles[used, row], column];
                    }
                }
                else
                {
                    //... with a saved row
                    for (int column = 0; column < 9; column++)
                    {
                        board[blockRow * 3 + row, column] = temp[shuffles[used, row], column];
                    }
                }

            }



            //log
            Trace.Write(String.Format("In block row {0}, the ", blockRow + 1));

            switch (shuffles[used, 0])
            {
                case 1:
                    //if it is a cycle
                    if (shuffles[used, 1] == 2)
                    {
                        Trace.WriteLine("rows were cycled up.");
                    }
                    //if it is a swap
                    else
                    {
                        Trace.WriteLine("first and second row were swapped.");
                    }
                    break;

                case 2:
                    //if it is a cycle
                    if (shuffles[used, 2] == 1)
                    {
                        Trace.WriteLine("rows were cycled down.");
                    }
                    //if it is a swap
                    else
                    {
                        Trace.WriteLine("first and third row were swapped.");
                    }
                    break;

                default:
                    //if it is unchanged
                    if (shuffles[used, 1] == 1)
                    {
                        Trace.WriteLine("rows were left untouched.");
                    }
                    //if it is a swap
                    else
                    {
                        Trace.WriteLine("second and third row were swapped.");
                    }
                    break;
            }

            Trace.WriteLine(ToString());


            used++;
        }

        return used;

    }



    /// <summary>
    /// Swaps columns within all block columns.
    /// </summary>
    /// <param name="used"> how many of the shuffles were used </param>
    /// <param name="shuffles"> list of possible shuffles </param>
    /// <returns> the updated 'used' number </returns>
    private int ShuffleColumns(int used, int[,] shuffles)
    {
        int[,] temp = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };


        for (int blockCol = 0; blockCol < 3; blockCol++)
        {

            for (int col = 0; col < 3; col++)
            {

                //do nothing with column if it already is in the right place
                if (col == shuffles[used, col]) 
                {
                    continue;
                }

                //save the column if it could be used later
                if (col < 2)
                {
                    //copy the column into temp
                    for (int row = 0; row < 9; row++)
                    {
                        temp[col, row] = board[row, blockCol * 3 + col];
                    }
                }

                //overwrite the column...
                if (shuffles[used, col] > col)
                {
                    //... with a later column
                    for (int row = 0; row < 9; row++)
                    {
                        board[row, blockCol * 3 + col] = board[row, blockCol * 3 + shuffles[used, col]];
                    }
                }
                else
                {
                    //... with a saved column
                    for (int row = 0; row < 9; row++)
                    {
                        board[row, blockCol * 3 + col] = temp[shuffles[used, col], row];
                    }
                }

            }

            //log
            Trace.Write(String.Format("In block column {0}, the ", blockCol + 1));

            switch (shuffles[used, 0])
            {
                case 1:
                    //if it is a cycle
                    if (shuffles[used, 1] == 2)
                    {
                        Trace.WriteLine("columns were cycled left.");
                    }
                    //if it is a swap
                    else
                    {
                        Trace.WriteLine("first and second column were swapped.");
                    }
                    break;

                case 2:
                    //if it is a cycle
                    if (shuffles[used, 2] == 1)
                    {
                        Trace.WriteLine("columns were cycled right.");
                    }
                    //if it is a swap
                    else
                    {
                        Trace.WriteLine("first and third column were swapped.");
                    }
                    break;

                default:
                    //if it is unchanged
                    if (shuffles[used, 1] == 1)
                    {
                        Trace.WriteLine("columns were left untouched.");
                    }
                    //if it is a swap
                    else
                    {
                        Trace.WriteLine("second and third column were swapped.");
                    }
                    break;
            }

            Trace.WriteLine(ToString());


            used++;

        }

        return used;

    }



    /// <summary>
    /// Shuffles the rows of boxes.
    /// </summary>
    /// <param name="used"> how many of the shuffles were used </param>
    /// <param name="shuffles"> list of possible shuffles </param>
    /// <returns> the updated 'used' number </returns>
    private int ShuffleBlockRows(int used, int[,] shuffles)
    {
        int[,] temp = new int[,]{
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
           };


        for (int blockRow = 0; blockRow<3; blockRow++)
        {
            //do nothing with block row if it already is in the right place
            if (shuffles[used, blockRow] == blockRow)
            {
                continue;
            }

            //save the block row if it could be used later
            if (blockRow < 2)
            {
                //copy the block row into temp
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        temp[blockRow * 3 + row, col] = board[blockRow * 3 + row, col];
                    }
                }
            }

            //overwrite the block row...
            if (shuffles[used, blockRow] > blockRow)
            {
                //... with a later block row
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        board[blockRow * 3 + row, col] = board[shuffles[used, blockRow] * 3 + row, col];
                    }
                }
            }
            else
            {
                //... with a saved blockrow
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        board[blockRow * 3 + row, col] = temp[shuffles[used, blockRow] * 3 + row, col];
                    }
                }
            }


        }

        //log
        switch (shuffles[used, 0])
        {
            case 1:
                //if it is a cycle
                if (shuffles[used, 1] == 2)
                {
                    Trace.WriteLine("The block rows were cycled up.");
                }
                //if it is a swap
                else
                {
                    Trace.WriteLine("The first and second block row were swapped.");
                }
                break;

            case 2:
                //if it is a cycle
                if (shuffles[used, 2] == 1)
                {
                    Trace.WriteLine("The block rows were cycled down.");
                }
                //if it is a swap
                else
                {
                    Trace.WriteLine("The first and third block row were swapped.");
                }
                break;

            default:
                //if it is unchanged
                if (shuffles[used, 1] == 1)
                {
                    Trace.WriteLine("The block rows were left untouched.");
                }
                //if it is a swap
                else
                {
                    Trace.WriteLine("The second and third block row were swapped.");
                }
                break;
        }

        Trace.WriteLine(ToString());


        return ++used;
    }




    /// <summary>
    /// Shuffles the columns of boxes.
    /// </summary>
    /// <param name="used"> how many of the shuffles were used </param>
    /// <param name="shuffles"> list of possible shuffles </param>
    /// <returns> the updated 'used' number </returns>
    private int ShuffleBlockColumns(int used, int[,] shuffles)
    {
        int[,] temp = new int[,]{
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
           };


        for (int blockCol = 0; blockCol < 3; blockCol++)
        {
            //do nothing with block column if it already is in the right place
            if (shuffles[used, blockCol] == blockCol)
            {
                continue;
            }

            //save the block column if it could be used later
            if (blockCol < 2)
            {
                //copy the block column into temp
                for (int column = 0; column < 3; column++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        temp[blockCol * 3 + column, row] = board[row, blockCol * 3 + column];
                    }
                }
            }

            //overwrite the block column...
            if (shuffles[used, blockCol] > blockCol)
            {
                //... with a later block column
                for (int column = 0; column < 3; column++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        board[row, blockCol * 3 + column] = board[row, shuffles[used, blockCol] * 3 + column];
                    }
                }
            }
            else
            {
                //... with a saved block column
                for (int column = 0; column < 3; column++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        board[row, blockCol * 3 + column] = temp[shuffles[used, blockCol] * 3 + column, row];
                    }
                }
            }


        }

        //log
        switch (shuffles[used, 0])
        {
            case 1:
                //if it is a cycle
                if (shuffles[used, 1] == 2)
                {
                    Trace.WriteLine("The block columns were cycled left.");
                }
                //if it is a swap
                else
                {
                    Trace.WriteLine("The first and second block column were swapped.");
                }
                break;

            case 2:
                //if it is a cycle
                if (shuffles[used, 2] == 1)
                {
                    Trace.WriteLine("The block columns were cycled down.");
                }
                //if it is a swap
                else
                {
                    Trace.WriteLine("The first and third block column were swapped.");
                }
                break;

            default:
                //if it is unchanged
                if (shuffles[used, 1] == 1)
                {
                    Trace.WriteLine("The block columns were left untouched.");
                }
                //if it is a swap
                else
                {
                    Trace.WriteLine("The second and third block column were swapped.");
                }
                break;
        }

        Trace.WriteLine(ToString());

        return ++used;
    }






    /// <summary>
    /// Returns an array with the numbers in the specified block row.
    /// </summary>
    /// <param name="blockCol"> column of blocks </param> 
    /// <param name="blockRow"> row of blocks </param> 
    /// <param name="row"> row within the block </param> 
    /// <returns> the specified block row </returns>
    private int[] GetBlockRow(int blockCol, int blockRow, int row)
    {

        return new int[] { board[blockRow * 3 + row, blockCol * 3], board[blockRow * 3 + row, blockCol * 3 + 1], board[blockRow * 3 + row, blockCol * 3 + 2] };
    }



}