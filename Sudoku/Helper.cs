using System;
using System.Collections;








public class Helper
{

    static private Random random = new Random();



    static public bool AreNumbersUsed(ArrayList usedNumbers, int[] numbers)
    {
        int[] rows = { numbers[2], numbers[3], numbers[4] };

        foreach (int row in rows)
        {
            if (usedNumbers.Contains(row))
            {
                return true;
            }
        }

        return false;
    }





    /*
     * reorders the arguments to fit the wished layout
     * 
     * Layout: { first column, second column, first row, second row, third row }
     * 
     */
    static public int[] ConstructResult(int[] columns, int blockcol, int firstRow, int secondRow, int thirdRow)
    {
        int[] result = new int[] { columns[0] + blockcol*3,
            columns[1] + blockcol*3, 
            firstRow, 
            secondRow + 3,
            thirdRow + 6 };


        /*
        Console.Write("\nConstructed Result: ");

        foreach (int i in result)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine("\n");
        */

        return result;
    }





    //returns the indices of the matching numbers
    static public ArrayList SharedNumbers(int[] numbers, int[] row)
    {
        /*
            Console.Write("Shared Numbers in: ");
            foreach (int i in numbers) Console.Write(i);
            Console.Write(" ");
            foreach (int j in row) Console.Write(j);
            Console.WriteLine(" "); 
        */


        ArrayList result = new ArrayList();

        //for each number in the starting row
        for (int i = 0; i < numbers.Length; i++)
        {
            //in combination with each number in the comparison row
            for (int j = 0; j < row.Length; j++)
            {

                //in case the numbers in those positions are the same
                if (numbers[i] == row[j])
                {
                    /*
                    Console.WriteLine($":{numbers[i]} ({i}) == {row[j]} ({j})");
                    */

                    //add the positions to the result
                    result.Add(new int[] { i, j });
                }
            }
        }

        /*
            for (int i = 0; i < result.Count; i++)
            {
                int[] temp = (int[])result[i];
                for (int j = 0; j < temp.Length; j++)
                {
                    Console.Write(temp[j]);
                }
                Console.Write("  ");
            }
        */
        
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
