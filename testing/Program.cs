using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace testing
{
    public static class QueueExtension
    {
        public static bool IsEmpty<T>(this Queue<T> q)
        {
            return q.Count > 0;
        }
    }
    

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Board Size");
            var n = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Source X");
            var x =int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Source Y");
            var y = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Target X");
            var x1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Target Y");
            var y1 = int.Parse(Console.ReadLine());

            int[] source = { x, y };
            int[] target = { x1, y1 };

           var result = Knight.MinimumPathToTarget(source, target, n);

            Console.WriteLine(result);

            Console.ReadLine();
        }



       
    }



    public class Knight
    {
       


        static bool IsValid(int x, int y, int distance)
        {
            if (x >= 1 && x <= distance && y >= 1 && y <= distance)
                return true;

            return false;
        }

        public static int MinimumPathToTarget(int[] source, int[] target, int num)
        {
            int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 };
            int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 };
            
            var q = new Queue<Cell>();
            q.Enqueue(new Cell(source[0], source[1], 0));

            var visited = new bool[num + 1, num + 1];
            visited[source[0], source[1]] = true;

            while (!q.IsEmpty())
            {
                var cell = q.Dequeue();

                if (cell.X == target[0] && cell.Y == target[1])
                    return cell.Distance;

                for(int i=0; i<8; i++)
                {
                    var x = cell.X + dx[i];
                    var y = cell.Y + dy[i];

                    if(IsValid(x,y,num) && !visited[x, y])
                    {
                        q.Enqueue(new Cell(x, y, cell.Distance + 1));
                        visited[x, y] = true;
                    } 
                }
            }

            return -1;
        }
    }

    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance { get; set; }


        public Cell(int x, int y, int dist)
        {
            X = x;
            Y = y;
            Distance = dist;
        }
    }




}
