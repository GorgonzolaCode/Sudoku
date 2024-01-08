using System;
using System.Collections;








public class Helper
{

    static private Random random = new Random();


    /// <summary>
    /// Only compares the row aspect of the argument with an ArrayList of numbers.
    /// </summary>
    /// <param name="usedNumbers"></param>
    /// <param name="argument"></param>
    /// <returns> Whether a row in the argument was already used. </returns>
    static public bool AreNumbersUsed(bool[] usedNumbers, int[] argument)
    {
        int[] rows = { argument[2], argument[3], argument[4] };

        foreach (int row in rows)
        {
            if (usedNumbers[row] == true)
            {
                return true;
            }
        }

        return false;
    }




    /// <summary>
    /// Reorders the arguments to fit the wished layout.
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="blockcol"></param>
    /// <param name="firstRow"></param>
    /// <param name="secondRow"></param>
    /// <param name="thirdRow"></param>
    /// <returns> { first column, second column, first row, second row, third row } </returns> 
    static public int[] ConstructResult(int[] columns, int blockcol, int firstRow, int secondRow, int thirdRow)
    {
        int[] result = new int[] { columns[0] + blockcol*3,
            columns[1] + blockcol*3, 
            firstRow, 
            secondRow + 3,
            thirdRow + 6 };


        return result;
    }





    /// <summary>
    /// Returns the indices of the matching numbers.
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="row"></param>
    /// <returns> An ArrayList of arrays with the indices of matching numbers. </returns>
    static public ArrayList SharedNumbers(int[] numbers, int[] compare)
    {

        ArrayList result = new ArrayList();

        //for each number in the starting row
        for (int i = 0; i < numbers.Length; i++)
        {
            //in combination with each number in the comparison row
            for (int j = 0; j < compare.Length; j++)
            {

                //in case the numbers in those positions are the same
                if (numbers[i] == compare[j])
                {
                    //add the positions to the result
                    result.Add(new int[] { i, j });
                }
            }
        }

        
        return result;
    }



    static public int[,] GetShuffleNumbers()
    {
        int[,] result = {
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 },
                {0, 1, 2 }
            };

        ArrayList numbers;


        //randomize the result
        for (int i = 0; i < result.GetLength(0); i++)
        {
            numbers = new ArrayList() { 0, 1, 2 };

            for (int j = 0; j < 3; j++)
            {
                result[i, j] = (int)numbers[random.Next(0, 3 - j)];
                numbers.Remove(result[i, j]);
            }

        }


        return result;
    }




    //returns a randomized sequence of valid numbers
    static public int[] GetNumbers()
    {
        int[] result = new int[9];

        //list of possible values
        ArrayList numbersLeft = new ArrayList()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9
        };


        //sort the possible values by random
        int curValIndex;

        for (int i = 0; i < 9; i++)
        {
            curValIndex = random.Next(0, numbersLeft.Count);
            result[i] = (int)numbersLeft[curValIndex];
            numbersLeft.RemoveAt(curValIndex);
        }


        return result;
    }







    //get a random boolean value
    static public bool GetRandomBool()
    {
        int i = random.Next(0, 2);
        return (i == 0);
    }





}
