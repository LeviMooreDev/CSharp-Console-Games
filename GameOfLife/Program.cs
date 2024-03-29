﻿using System;
using System.Threading;
using Engine;

namespace GameOfLife
{
    class Program
    {
        private static bool run = true;
        private static bool[,] corrent;
        private static bool[,] next;

        static void Main(string[] args)
        {
            Game.OnStart += Start;
            Game.OnUpdate += Update;
            Game.Begin("Game of Life", new Vector2(100, 100));
        }

        private static void Start()
        {
            corrent = new bool[Game.Size.XInt, Game.Size.YInt];
            next = new bool[Game.Size.XInt, Game.Size.YInt];

            for (int x = 0; x < Game.Size.x; x++)
            {
                for (int y = 0; y < Game.Size.y; y++)
                {
                    next[x, y] = Rng.Value > .5f;
                }
            }
        }

        private static void Update()
        {
            if (Input.KeyDown(ConsoleKey.Spacebar))
            {
                run = !run;
            }
            if (Input.KeyDown(ConsoleKey.C))
            {
                for (int x = 0; x < Game.Size.x; x++)
                {
                    for (int y = 0; y < Game.Size.y; y++)
                    {
                        next[x, y] = false;
                    }
                }
            }

            if (run)
            {
                for (int x = 0; x < Game.Size.x; x++)
                {
                    for (int y = 0; y < Game.Size.y; y++)
                    {
                        corrent[x, y] = next[x, y];
                    }
                }
                for (int x = 0; x < Game.Size.x; x++)
                {
                    for (int y = 0; y < Game.Size.y; y++)
                    {
                        int neightbours = 0;
                        for (int xn = x - 1; xn <= x + 1; xn++)
                        {
                            for (int yn = y - 1; yn <= y + 1; yn++)
                            {
                                if (xn == x && yn == y)
                                {
                                    continue;
                                }

                                if (xn < 0 || yn < 0)
                                {
                                    continue;
                                }

                                if (xn >= Game.Size.x || yn >= Game.Size.y)
                                {
                                    continue;
                                }

                                if (corrent[xn, yn])
                                {
                                    neightbours++;
                                }
                            }
                        }

                        if (corrent[x, y])
                        {
                            next[x, y] = neightbours == 2 || neightbours == 3;
                        }
                        else
                        {
                            next[x, y] = neightbours == 3;
                        }

                        Render.DrawBackgroundColor(new Vector2(x, y), next[x, y] ? Color.White : Color.Black);
                    }
                }

                Thread.Sleep(30);
            }

            if (Input.MouseLeftHold || Input.MouseRightHold)
            {
                Vector2 positon = Input.MousePosition;
                if (positon.x >= 0 && positon.y >= 0)
                {
                    if (positon.x < Game.Size.x && positon.y < Game.Size.y)
                    {
                        next[positon.XInt, positon.YInt] = Input.MouseLeftHold;
                        Render.DrawBackgroundColor(new Vector2(positon.x, positon.y), next[positon.XInt, positon.YInt] ? Color.White : Color.Black);
                    }
                }
            }
            Render.DrawText(Input.LastMousePosition, ' ');
            Render.DrawText(Input.MousePosition, 'x');
            Render.DrawTextColor(Input.MousePosition, Color.Green);
        }
    }
}
