//Будем распределять фишки с кучек выше среднего значения, начиная с тех, у кого соседняя кучка выше
//в противоположном направлении. Если соседние кучки одинаковы - проверяем более удалённых соседей

using System;
using System.Collections.Generic;

namespace Test1
{   
    class Program
    {
        static int TotalChips = 0;
        static int ChipsPerPlayer = 0;
        static void Main()
        {
            //int[] chips1 = new int[] { 1, 5, 9, 10, 5 };
            int[] chips1 = new int[] { 0, 10, 0, 8, 3, 10, 7, 0, 9, 3 };
            int[] chips2 = new int[] { 1, 2, 3 };
            int[] chips3 = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 };

            СhipsToConsole(chips1);
            Console.WriteLine(HelpJose(chips1));

            СhipsToConsole(chips2);
            Console.WriteLine(HelpJose(chips2));

            СhipsToConsole(chips3);
            Console.WriteLine(HelpJose(chips3));
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


                //foreach (var item in Arr)
                //{
                //    Console.Write(item + " ");
                //}

                //Console.WriteLine("- " + StepCount);


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
            int IndexOfPile = -1;
            
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i] > ChipsPerPlayer)
                {
                    int Step = 1;
                    bool NextStep;
                    do
                    {
                        NextStep = false;
                        int TopLeftNeighbor = IndexOfPile == -1 ? 0 : GetNeighbors(IndexOfPile, Step, Arr, false);
                        int TopRightNeighbor = IndexOfPile == -1 ? 0 : GetNeighbors(IndexOfPile, Step, Arr, true);
                        int TopMaxNeighbor = Math.Max(TopLeftNeighbor, TopRightNeighbor);

                        int CurLeftNeighbor = GetNeighbors(i, Step, Arr, false);
                        int CurRightNeighbor = GetNeighbors(i, Step, Arr, true);
                        int CurMaxNeighbor = Math.Max(CurLeftNeighbor, CurRightNeighbor);
                        if (CurMaxNeighbor > TopMaxNeighbor)
                        {
                            IndexOfPile = i;
                        }
                        else if (CurMaxNeighbor == TopMaxNeighbor)
                        {
                            if (Arr.Length / 2 > Step)
                            {
                                Step++;
                                NextStep = true;
                            }

                        }
                    } while (NextStep);
                }
            }
            bool Direction = GetDirection(IndexOfPile, Arr);
            Arr[IndexOfPile]--;
            Arr[GetCircleIndex(IndexOfPile, Direction, 1, Arr.Length)]++;
        }

        static bool GetDirection(int Index, int[] Arr)
        {
            int Step = 1;
            bool NextStep;
            bool Result = true;
            do
            {
                NextStep = false;
                int LeftNeighbor = GetNeighbors(Index, Step, Arr, false);
                int RightNeighbor = GetNeighbors(Index, Step, Arr, true);
                if (LeftNeighbor > RightNeighbor)
                {
                    Result = true;
                } else if (LeftNeighbor < RightNeighbor)
                {
                    Result = false;
                } else
                {
                    //во избежание ситуации, когда соседние кучки на любом растоянии равны, контролируем длинну шага
                    //в таком случае нет разницы переклажывать слева или справа
                    if (Arr.Length / 2 <= Step)
                    {
                        Result = true;
                    }
                    else
                    {
                        Step++;
                        NextStep = true;
                    }
                }
            } while (NextStep);

            return Result;
        }

        static int GetNeighbors(int Index, int Step, int[] Arr, bool Side)
        {
            return Arr[GetCircleIndex(Index, Side, Step, Arr.Length)];
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

        /// <summary>
        /// определяем ближайшее направление от большей кучки к меньшей
        /// </summary>
        /// <param name="MinIndex"></param>
        /// <param name="MaxIndex"></param>
        /// <param name="ArrLenght"></param>
        /// <returns></returns>
        //static bool GetDirection(int MinIndex, int MaxIndex, int ArrLenght)
        //{
        //    int d1, d2;
        //    if (MinIndex > MaxIndex)
        //    {
        //        d1 = MinIndex - MaxIndex;
        //        d2 = MaxIndex + ArrLenght - MinIndex; 
        //    } else
        //    {
        //        d1 = ArrLenght - MaxIndex + MinIndex;
        //        d2 = MaxIndex - MinIndex;
        //    }
        //    return (d1 < d2);
        //}

        /// <summary>
        /// выясняем индекс ближайшей соседней кучки от самой маленьклй с количеством фишек > среднего
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        static int GetSourceIndex(int DestIndex, int[] arr)
        {
            int Result = -1;
            //поверяем ближайшие соседние кучки
            int Step = 1;
            bool NextDoorNeeded;
            do
            {
                NextDoorNeeded = false;
                //кучка слева
                int slIndex = GetCircleIndex(DestIndex, false, Step, arr.Length);
                int sl = arr[slIndex];
                //кучка справа
                int srIndex = GetCircleIndex(DestIndex, true, Step, arr.Length);
                int sr = arr[srIndex];

                СhipsToConsole(arr);

                if (Math.Max(sl, sr) > ChipsPerPlayer)
                {
                    if (sl > sr)
                    {
                        Result = slIndex;
                    } else if (sl < sr)
                    {
                        Result = srIndex;
                    } else //если ближайшие соседние кучки равны, смотрим более удалённые
                    {
                        //во избежание ситуации, когда соседние кучки на любом растоянии равны, контролируем длинну шага
                        //в таком случае нет разницы переклажывать слева или справа
                        if (arr.Length / 2 <= Step)
                        {
                            Result = slIndex;
                        } else
                        {
                            Step++;
                            NextDoorNeeded = true;
                        }
                    }
                }
                else //если обе соседние кучки <= среднему значению, смотрим более удалённые
                {
                    Step++;
                    NextDoorNeeded = true;
                }
            } while (NextDoorNeeded);

            return Result;
        }

        /// <summary>
        /// выясняем индекс минимальной кучки
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        static int GetDestIndex(int[] arr)
        {
            int MinValue = int.MaxValue;
            int Result = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (MinValue > arr[i])
                {
                    MinValue = arr[i];
                    Result = i;
                }
            }
            return Result;
        }
    }
}
