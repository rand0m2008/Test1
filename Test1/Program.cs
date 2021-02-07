using System;

namespace Test1
{   
    class Program
    {
        static int TotalChips = 0;
        static int ChipsPerPlayer = 0;
        static void Main()
        {
            int[] chips1 = new int[] { 1, 5, 9, 10, 5 }; //12
            int[] chips2 = new int[] { 1, 2, 3 }; //1
            int[] chips3 = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 }; //1
            int[] chips4 = new int[] { 0, 10, 0, 8, 3, 10, 7, 0, 9, 3 }; 
            int[] chips5 = new int[] { 13, 8, 28, 21, 30, 6, 13, 27, 23, 1 };

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
            TotalChips = 0;
            foreach (var c in Arr)
            {
                TotalChips += c;
            }
            ChipsPerPlayer = TotalChips / Arr.Length;

            //Будем начинать с каждой кучки, после выберем лучший результат
            int MinMove = int.MaxValue;
            int[] ArrCopy = new int[Arr.Length];
            for (int Iter = 0; Iter < Arr.Length; Iter++)
            {
                Arr.CopyTo(ArrCopy, 0);
                int ItreMoveCount = 0;
                for (int i = 0; i < Arr.Length; i++)
                {
                    int CurIndex = Iter + i;
                    //Считаем сколько не хватает данной кучке для баланса
                    if (ArrCopy[CurIndex % ArrCopy.Length] > ChipsPerPlayer) {
                        int CurMove = ArrCopy[CurIndex % ArrCopy.Length] - ChipsPerPlayer;
                        ArrCopy[CurIndex % ArrCopy.Length] -= CurMove;
                        ArrCopy[(CurIndex + 1) % ArrCopy.Length] += CurMove;
                        ItreMoveCount += CurMove;
                    } else if (ArrCopy[CurIndex % ArrCopy.Length] < ChipsPerPlayer)
                    {
                        int CurMove = ChipsPerPlayer - ArrCopy[CurIndex % ArrCopy.Length];
                        ArrCopy[CurIndex % ArrCopy.Length] += CurMove;
                        ArrCopy[GetCircleIndex(CurIndex, true, 1, ArrCopy.Length)] -= CurMove;
                        ItreMoveCount += CurMove;
                    } 
                }
                if (ItreMoveCount < MinMove) MinMove = ItreMoveCount;
            }

            return MinMove;
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
