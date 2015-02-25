using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseFinder
{
    class Program
    {
        static void Main(string[] args)
        {

            CheeseNibbler test = new CheeseNibbler();
            Point cheese;
            Point mouse;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (test.Grid[x, y].Status == Point.PointStatus.Cheese)
                    {
                        cheese = test.Grid[x, y];
                    }
                    else if (test.Grid[x, y].Status == Point.PointStatus.Mouse)
                    {
                        mouse = test.Grid[x, y];
                    }
                }
            }
            //test.DrawGrid();

            //test.GetUserMove();
            test.PlayGame();

        }
    }

    public class Point
    {
        public enum PointStatus
        {
            Empty, Mouse, Cheese
        }
        public int X { get; set; }
        public int Y { get; set; }
        public PointStatus Status { get; set; }
        public Point(int x, int y)
        {
            this.X = x; this.Y = y;
            this.Status = PointStatus.Empty;
        }
    }

    public class CheeseNibbler
    {
        public Point[,] Grid { get; set; }
        public Point Mouse { get; set; }
        public Point Cheese { get; set; }
        public int Round { get; set; }

        public CheeseNibbler()
        {
            Random rng = new Random();
            this.Round = 1;
            this.Grid = new Point[10, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    this.Grid[x, y] = new Point(x, y);
                }
            }
            this.Grid[rng.Next(0, 10), rng.Next(0, 10)].Status = Point.PointStatus.Mouse;
            Point tempPoint = this.Grid[rng.Next(0, 10), rng.Next(0, 10)];
            if (tempPoint.Status != Point.PointStatus.Mouse)
            {
                this.Grid[tempPoint.X, tempPoint.Y].Status = Point.PointStatus.Cheese;
            }
            else
            {
                this.Grid[rng.Next(0, 10), rng.Next(0, 10)].Status = Point.PointStatus.Cheese;
            }

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (this.Grid[x, y].Status == Point.PointStatus.Mouse)
                    {
                        this.Mouse = new Point(x, y);
                    }
                    if (this.Grid[x, y].Status == Point.PointStatus.Cheese)
                    {
                        this.Cheese = new Point(x, y);
                    }
                }

            }
        }
        public void DrawGrid()
        {
            Console.Clear();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    switch (this.Grid[x,y].Status)
                    {
                        case Point.PointStatus.Empty:
                            Console.Write("[   ] ");
                            break;
                        case Point.PointStatus.Mouse:
                            Console.Write("[ M ] ");
                            break;
                        case Point.PointStatus.Cheese:
                            Console.Write("[ C ] ");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        public ConsoleKey GetUserMove()
        {
            //by putting true in the ReadKey, you doesn't show the character its self
            ConsoleKeyInfo userInput = Console.ReadKey(true);
            while (userInput.Key != ConsoleKey.LeftArrow && userInput.Key != ConsoleKey.RightArrow && userInput.Key != ConsoleKey.UpArrow && userInput.Key != ConsoleKey.DownArrow)
            {
                Console.WriteLine("Invalid input");
                userInput = Console.ReadKey(true);
            }
            while (!ValidMove(userInput.Key))
            {
                Console.WriteLine("Move is not valid");
                userInput = Console.ReadKey();
            }
            //if (userInput.Key == ConsoleKey.LeftArrow || userInput.Key == ConsoleKey.RightArrow || userInput.Key == ConsoleKey.UpArrow || userInput.Key == ConsoleKey.DownArrow)
            //{
            //    if (ValidMove(userInput.Key))
            //    {
            //        return userInput.Key;
            //    }
            //    else
            //    {
            //        Console.WriteLine("Move is not valid");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Invalid input");
            //}
            return userInput.Key;
        }

        public bool ValidMove(ConsoleKey input)
        {
            if ((input == ConsoleKey.LeftArrow && this.Mouse.X == 0) || (input == ConsoleKey.RightArrow && this.Mouse.X == 9) || (input == ConsoleKey.UpArrow && this.Mouse.Y == 0) || (input == ConsoleKey.DownArrow && this.Mouse.Y == 9))
            {
                return false;
            }
            return true;
        }

        public bool MoveMouse(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.LeftArrow:
                    this.Mouse.X--;
                    this.Grid[this.Mouse.X + 1, this.Mouse.Y].Status = Point.PointStatus.Empty;
                    break;
                case ConsoleKey.RightArrow:
                    this.Mouse.X++;
                    this.Grid[this.Mouse.X - 1, this.Mouse.Y].Status = Point.PointStatus.Empty;
                    break;
                case ConsoleKey.UpArrow:
                    this.Mouse.Y--;
                    this.Grid[this.Mouse.X, this.Mouse.Y + 1].Status = Point.PointStatus.Empty;
                    break;
                case ConsoleKey.DownArrow:
                    this.Mouse.Y++;
                    this.Grid[this.Mouse.X, this.Mouse.Y - 1].Status = Point.PointStatus.Empty;
                    break;
            }

            if (this.Grid[this.Mouse.X, this.Mouse.Y].Status == Point.PointStatus.Cheese)
            {
                return true;
            } 
            this.Grid[this.Mouse.X, this.Mouse.Y].Status = Point.PointStatus.Mouse;
            return false;
        }

        public void PlayGame()
        {
            bool playing = false;
            while (!playing)
            {
                this.DrawGrid();
                playing = this.MoveMouse(this.GetUserMove());
                this.Round ++;
            }
            Console.WriteLine("Congratulations! You got a cheese after {0} moves!", this.Round);
        }
    }
}
