using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class Field
    {
        private readonly int[,] field;
        private int minNum;
        private int maxNum;

        //Constructor
        public Field(int fieldZize, int minNum, int maxNum)
        {
            field = new int[fieldZize, fieldZize];

            this.minNum = minNum;
            this.maxNum = maxNum + 1;
        }

        public void Start()
        {
            FillField();
            Console.WriteLine("Beginning field:");
            ShowFieldConsole();
            RemoveTriples();
        }

        public void ShowFieldConsole()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    Console.Write(field[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
        
        private void FillField()
        {
            var rand = new Random();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = rand.Next(minNum, maxNum);
                }
            }
        }

        private void RemoveTriples()
        {
            var rand = new Random();

            var hasTriple = true;
            var count = 0;

            while (hasTriple)
            {
                count++;
                hasTriple = false;

                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        //Vertical match check
                        if (i < field.GetLength(0) - 2 && field[i, j] == field[i + 1, j] && field[i, j] == field[i + 2, j])
                        {
                            if (i == 0)
                            {
                                field[i, j] = rand.Next(minNum, maxNum);
                                field[i + 1, j] = rand.Next(minNum, maxNum);
                                field[i + 2, j] = rand.Next(minNum, maxNum);
                            }
                            else
                            {
                                for (int x = i - 1, y = i + 2; x >= 0; x--, y--)
                                {
                                    field[y, j] = field[x, j];
                                    if (x == 0)
                                    {
                                        for (int d = y - 1; d >= 0; d--)
                                        {
                                            field[d, j] = rand.Next(minNum, maxNum);
                                        }
                                    }
                                }
                            }

                            hasTriple = true;
                        }

                        //Horizontal match check
                        if (j < field.GetLength(1) - 2 && field[i, j] == field[i, j + 1] && field[i, j] == field[i, j + 2])
                        {

                            for (int x = i; x >= 0; x--)
                            {
                                if (x == 0)
                                {
                                    field[x, j] = rand.Next(minNum, maxNum);
                                    field[x, j + 1] = rand.Next(minNum, maxNum);
                                    field[x, j + 2] = rand.Next(minNum, maxNum);
                                }
                                else
                                {
                                    field[x, j] = field[x - 1, j];
                                    field[x, j + 1] = field[x - 1, j + 1];
                                    field[x, j + 2] = field[x - 1, j + 2];
                                }
                            }


                            hasTriple = true;
                        }
                    }
                }
                
                Console.WriteLine($"After {count}st circle:");            //need make more abstract
                ShowFieldConsole();
            }
        }
    }
}
