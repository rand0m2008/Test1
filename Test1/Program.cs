//Переосмыслил подход к задаче, ибо нет смысла смотреть только соседние кучки.
//Всё стало намного проще. Находим кучку выше среднего и смотрим высоту соседних кучек на растоянии половины всех кучек
//В какой стороне сумма высот меньше - туда и перекладываем
using System;

namespace Test1
{   
    class Program
    {
        //Показать решение
        static bool ShowSolution = true;
        static int TotalChips = 0;
        static int ChipsPerPlayer = 0;
        static void Main()
        {
            int[] chips1 = new int[] { 1, 5, 9, 10, 5 };
            //int[] chips1 = new int[] { 13, 8, 28, 21, 30, 6, 13, 27, 23, 1 };
            int[] chips2 = new int[] { 1, 2, 3 };
            int[] chips3 = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 };
            int[] chips4 = new int[] { 0, 10, 0, 8, 3, 10, 7, 0, 9, 3 };
            int[] chips5 = new int[] { 13, 8, 31, 17, 19, 18, 20, 17, 17, 10 };

            СhipsToConsole(chips1);
            Console.WriteLine(HelpJose(chips1));

            СhipsToConsole(chips2);
            Console.WriteLine(HelpJose(chips2));

            СhipsToConsole(chips3);
            Console.WriteLine(HelpJose(chips3));

            СhipsToConsole(chips4);
            Console.WriteLine(HelpJose(chips4));

            СhipsToConsole(chips5);
            Console.WriteLine(HelpJose(chips5));
        }

        static void СhipsToConsole(int[] Arr)
        {
            Console.Write("chips: [ ");
            foreach (var c in Arr)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine("]");
        }

        static int HelpJose(int[] Arr)
        {
            int StepCount = 0;
            TotalChips = 0;
            foreach (var c in Arr)
            {
                TotalChips += c;
            }
            ChipsPerPlayer = TotalChips / Arr.Length;

            bool IsDone;
            do
            {
                StepCount++;
                IsDone = true;
                LetsStep(ref Arr);

                if (ShowSolution)
                {
                    foreach (var item in Arr)
                    {
                        Console.Write(item + " ");
                    }

                    Console.WriteLine("- " + StepCount);
                }
                

                foreach (var Pile in Arr)
                {
                    if (Pile != ChipsPerPlayer)
                    {
                        IsDone = false;
                        break;
                    }
                }
            } while (!IsDone);

            return StepCount;
        }

        static void LetsStep(ref int[] Arr)
        {
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i] > ChipsPerPlayer)
                {
                    int CurLeftNeighbors = GetNeighborsWeight(i, Arr, false);
                    int CurRightNeighbors = GetNeighborsWeight(i, Arr, true);

                    
                    if (CurLeftNeighbors > CurRightNeighbors)
                    {
                        Arr[i]--;
                        Arr[GetCircleIndex(i, true, 1, Arr.Length)]++;
                        break;
                    }
                    else
                    {
                        Arr[i]--;
                        Arr[GetCircleIndex(i, false, 1, Arr.Length)]++;
                        break;
                    }
                }
            }
        }
        
        //Взвешиваем соседей с указанной стороны
        static int GetNeighborsWeight(int Index, int[] Arr, bool Side)
        {
            int Weight = 0;
            for (int i = 0; i < Arr.Length / 2; i++)
            {
                Weight += Arr[GetCircleIndex(Index, Side, i + 1, Arr.Length)];
            }
            return Weight;
        }

        /// <summary>
        /// определяем индекс кучки с учётом направления, длинны шага и зацикленности массива
        /// </summary>
        /// <param name="CurIndex"></param>
        /// <param name="Direction"></param>
        /// <param name="Steps"></param>
        /// <param name="ArrLenght"></param>
        /// <returns></returns>
        static int GetCircleIndex(int CurIndex, bool Direction, int Steps, int ArrLenght)
        {
            int Result;
            if (Direction)
            {
                Result = CurIndex + Steps % ArrLenght;  //на случай, если шаг больше размера массива
                if (Result > ArrLenght - 1) Result -= ArrLenght;
            } else
            {
                Result = CurIndex - Steps % ArrLenght;
                if (Result < 0) Result = ArrLenght + Result;
            }
            return Result;
        }

    }
}
