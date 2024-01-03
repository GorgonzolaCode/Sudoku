using System.Collections;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;





public class Sudoku
{

    private int[,] board;

    public Sudoku()
    {
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
    }



    //returns a sudoku with random numbers in a basic layout
    static public Sudoku GetBasicSudoku()
    {
        Sudoku result = new Sudoku();
        int[] numbers = Helper.GetNumbers();

        for (int i = 0; i < 81; i++)
        {
            //swap each cell with its new value
            result.Set(i / 9, i % 9, numbers[result.Get(i / 9, i % 9) - 1]);
        }

        return result;
    }


    //tells if a sudoku has no contradictions
    public bool IsCorrect()
    {
        int temp;
        bool[] found;

        
        //for every row
        for (int row = 0; row < 9; row++)
        {
            found = new bool[9] { false, false, false,
                                  false, false, false,
                                  false, false, false };

            //for every number in row
            for (int col = 0; col < 9; col++)
            {
                //check if it was already used
                temp = board[row, col] - 1;
                if (found[temp]) return false;
                found[temp] = true;
            }
        }


        //for every column
        for (int col = 0; col < 9; col++)
        {
            found = new bool[9] { false, false, false,
                                  false, false, false,
                                  false, false, false };

            //for every number in column
            for (int row = 0; row < 9; row++)
            {
                //check if it was already used
                temp = board[row, col] - 1;
                if (found[temp]) return false;
                found[temp] = true;
            }
        }



        //for every block
        for (int block = 0; block < 9; block++)
        {
            found = new bool[9] { false, false, false,
                                  false, false, false,
                                  false, false, false };

            //for every number in block
            for (int cell = 0; cell < 9; cell++)
            {
                //check if it was already used
                temp = board[(block/3) * 3 + cell/3, (block%3) * 3 + cell%3] - 1;
                if (found[temp]) return false;
                found[temp] = true;
            }
        }


        return true;
    }


    //returns the value of a cell
    public int Get(int col, int row)
    {
        return board[row, col];
    }



    /*  
     *  sets the value of one cell
     *  starts at 0
     */
    public void Set(int col, int row, int value)
    {
        board[row, col] = value;
    }



    //clears console and draws the board
    public void Draw()
    {
        //Console.Clear();

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {

                //horizontal divider
                if (col == 0 && row % 3 == 0) { Console.WriteLine("________+_________+_________"); }

                
                //number
                Console.Write(board[row, col]);


                //vertical divider
                if ((col + 1) % 3 == 0) 
                {
                    Console.Write(" | "); 
                }
                else
                {
                    Console.Write(", ");
                }

            }
            Console.WriteLine();
        }


        Console.WriteLine(IsCorrect());
    }



    //uses all viable methods to shuffle a new sudoku
    public void FullShuffle()
    {
        PartialSwap();
        Shuffle();
    }





    private void PartialSwap()
    {
        PartialSwapRow();
        PartialSwapCol();
    }


    //swaps around partial rows in blocks (only before shuffling)
    private void PartialSwapRow()
    {

        int[,] numbers = Helper.GetShuffleNumbers();
        int[] temp = new int[3];


        //in each blockrow
        for (int blockrow = 0; blockrow < 3; blockrow++)
        {
            //in each column
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
                    board[blockrow * 3 + row, column] = temp[numbers[(blockrow * 3) + (column % 3), row]];
                }
            }
        }


    }



    //searches for possible combinations and then swaps around partial columns
    private void PartialSwapCol()
    {
        ArrayList usedRows;
        ArrayList possibleColumns;
        int[] arguments = new int[0];

        //for each block column
        for (int blockcol = 0; blockcol < 3; blockcol++)
        {
            usedRows = new ArrayList();

            possibleColumns = PossibleColumn(blockcol);

            //for every possible partial shuffle in the block column
            for (int i = 0; i < possibleColumns.Count; i++)
            {
                //shuffle only half the time
                if (Helper.GetRandomBool()) continue;


                arguments = (int[])possibleColumns[i];


                if (Helper.AreNumbersUsed(usedRows, arguments)) continue;

                PartialSwapCol(arguments);
                usedRows.Add(arguments[2]);
                usedRows.Add(arguments[3]);
                usedRows.Add(arguments[4]);
            }

        }
    }

    /*
     * takes arguments in the form of
     * 
     * Layout: { first column, second column, first row, second row, third row }
     * 
     * reorders board accordingly
     */
    private void PartialSwapCol(int[] arguments)
    {
        int[] columns = { arguments[0], arguments[1] };
        int[] rows = { arguments[2], arguments[3], arguments[4] };
        int temp;

        //for each specified row
        foreach (int row in rows)
        {
            //swap the columns
            temp = board[row, columns[0]];
            board[row, columns[0]] = board[row, columns[1]];
            board[row, columns[1]] = temp;

        }
    }



    


    /*
     * returns ArrayList of which parts of which columns can be used for a partial swap
     * 
     * Layout: { first column, second column, first row, second row, third row }
     * 
     */
    private ArrayList PossibleColumn(int blockcol)
    {
        ArrayList result = new ArrayList();

        
        ArrayList shared;
        ArrayList found;
        int[] firstBlockRow;
        int[] secondBlockRow;
        int[] thirdBlockRow;
        int[] partners = new int[] { 0, 0, -1 };         //third element only for SharedNumbers()
        int[] sharedPositions;
        int[] construct;
        int[] foundFirst;
        int[] foundSecond;



        //for each row in the first block
        for (int startRow = 0; startRow < 3; startRow++)
        {

            //the numbers of the row in the first block
            firstBlockRow = GetBlockRow(blockcol, 0, startRow);

            //for each comparison row in second block
            for (int compareRow = 0; compareRow < 3; compareRow++)
            {
                //the numbers of the comparison row in the second block
                secondBlockRow = GetBlockRow(blockcol, 1, compareRow);

                //get the position of any shared numbers
                shared = Helper.SharedNumbers(firstBlockRow, secondBlockRow);


                //for each pair of matching numbers
                for (int i = 0; i < shared.Count; i++)
                {
                    sharedPositions = (int[])shared[i];

                    //save the partner numbers
                    partners[0] = board[startRow, sharedPositions[1] + blockcol*3];
                    partners[1] = board[compareRow + 3, sharedPositions[0] + blockcol * 3];



                    
                    //for each row in third block
                    for (int finalRow = 0; finalRow < 3; finalRow++)
                    {
                        //the numbers of the row in the final block
                        thirdBlockRow = GetBlockRow(blockcol, 2, finalRow);

                        //get the position of matching numbers
                        found = Helper.SharedNumbers(partners, thirdBlockRow);


                        //if there were not 2 matches it didn't work
                        if (found.Count < 2) continue;


                        foundFirst = (int[])found[0];
                        foundSecond = (int[])found[1];

                        //check if the numbers are in the right column
                        if ((foundFirst[1] != sharedPositions[0])
                            ||  
                            (foundSecond[1] != sharedPositions[1]) )
                        {
                            //if not, continue
                            continue;
                        }

                        //add the combination as a construct to the result
                        construct = Helper.ConstructResult(sharedPositions, blockcol, startRow, compareRow, finalRow);
                        result.Add(construct);

                    }
                }
                
            }
        }


        return result;
    }


    



    



    /*
    public void Cycle()
    {
        int[] temp = new int[9];

        for (int col = 0; col < 9; col++)
        {
            for (int row = 0; row < 9; row++)
            {
                //copy the column
                temp[row] = board[row, col];

            }

            for (int row = 0; row < 9; row++)
            {
                //insert the column in a cycled way
                board[row, col] = temp[
                    (row + col) % 3             //cycle within block
                    +
                    (row / 3) * 3               //offset for block
                    ];
            }


        }
    }
    */



    private void Shuffle()
    {

        //generate shuffle instructions
        int[,] shuffles = Helper.GetShuffleNumbers();
        int used = 0;


        used = ShuffleRows(used, shuffles);

        used = ShuffleColumns(used, shuffles);

        used = ShuffleBoxesHor(used, shuffles);

        used = ShuffleBoxesVert(used, shuffles);

    }



    private int ShuffleRows(int used, int[,] shuffles)
    {

        int[,] temp = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };


        //in each block
        for (int i = 0; i < 3; i++)
        {

            //in each row
            for (int j = 0; j < 3; j++)
            {

                //do nothing with row if it already is in the right place
                if (j == shuffles[used, j])
                {
                    continue;
                }

                //save the row if it could be used later
                if (j < 2)
                {
                    //copy the row into temp
                    for (int k = 0; k < 9; k++)
                    {
                        temp[j, k] = board[i * 3 + j, k];
                    }
                }

                //overwrite the row...
                if (shuffles[used, j] > j)
                {
                    //... with a later row
                    for (int k = 0; k < 9; k++)
                    {
                        board[i * 3 + j, k] = board[i * 3 + shuffles[used, j], k];
                    }
                }
                else
                {
                    //... with a saved row
                    for (int k = 0; k < 9; k++)
                    {
                        board[i * 3 + j, k] = temp[shuffles[used, j], k];
                    }
                }

            }
            used++;

        }




        return used;


    }




    private int ShuffleColumns(int used, int[,] shuffles)
    {
        int[,] temp = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };


        //in each block
        for (int i = 0; i < 3; i++)
        {

            //in each column
            for (int j = 0; j < 3; j++)
            {

                //do nothing with column if it already is in the right place
                if (j == shuffles[used, j]) 
                {
                    continue;
                }

                //save the column if it could be used later
                if (j < 2)
                {
                    //copy the column into temp
                    for (int k = 0; k < 9; k++)
                    {
                        temp[j, k] = board[k, i * 3 + j];
                    }
                }

                //overwrite the column...
                if (shuffles[used, j] > j)
                {
                    //... with a later column
                    for (int k = 0; k < 9; k++)
                    {
                        board[k, i * 3 + j] = board[k, i * 3 + shuffles[used, j]];
                    }
                }
                else
                {
                    //... with a saved column
                    for (int k = 0; k < 9; k++)
                    {
                        board[k, i * 3 + j] = temp[shuffles[used, j], k];
                    }
                }

            }
            used++;

        }




        return used;

    }



    //shuffles the boxes horizontally
    private int ShuffleBoxesHor(int used, int[,] shuffles)
    {
        int[,] temp = new int[,]{
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
           };


        //each block
        for (int i = 0; i<3; i++)
        {
            //do nothing with block if it already is in the right place
            if (shuffles[used, i] == i)
            {
                continue;
            }

            //save the block if it could be used later
            if (i < 2)
            {
                //copy the block into temp
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        temp[i * 3 + j, k] = board[i * 3 + j, k];
                    }
                }
            }

            //overwrite the block...
            if (shuffles[used, i] > i)
            {
                //... with a later block
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        board[i * 3 + j, k] = board[shuffles[used, i] * 3 + j, k];
                    }
                }
            }
            else
            {
                //... with a saved block
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        board[i * 3 + j, k] = temp[shuffles[used, i] * 3 + j, k];
                    }
                }
            }


        }


        return ++used;
    }




    //shuffles the boxes vertically
    private int ShuffleBoxesVert(int used, int[,] shuffles)
    {
        int[,] temp = new int[,]{
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
           };


        //each block
        for (int i = 0; i < 3; i++)
        {
            //do nothing with block if it already is in the right place
            if (shuffles[used, i] == i)
            {
                continue;
            }

            //save the block if it could be used later
            if (i < 2)
            {
                //copy the block into temp
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        temp[i * 3 + j, k] = board[k, i * 3 + j];
                    }
                }
            }

            //overwrite the block...
            if (shuffles[used, i] > i)
            {
                //... with a later block
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        board[k, i * 3 + j] = board[k, shuffles[used, i] * 3 + j];
                    }
                }
            }
            else
            {
                //... with a saved block
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        board[k, i * 3 + j] = temp[shuffles[used, i] * 3 + j, k];
                    }
                }
            }


        }


        return ++used;
    }







    private int[] GetBlockRow(int blockCol, int blockRow, int row)
    {

        return new int[] { board[blockRow * 3 + row, blockCol * 3 + 0], board[blockRow * 3 + row, blockCol * 3 + 1], board[blockRow * 3 + row, blockCol * 3 + 2] };
    }



}