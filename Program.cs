using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    class SnakeGameNe
    {
        #region parameter
        public Random rand = new Random();
        public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        int score, headX, headY, fruitX, fruitY, scorePlus, nTail;
        string fruit;
        int[] TailX = new int[100];
        int[] TailY = new int[100];
        const int height = 20;
        const int width = 60;
        const int panel = 10;
        bool gameOver, reset, isprinted, horizontal, vertical;
        string dir, pre_dir;
        #endregion
        //Hiển thị màn hình khi bắt đầu
        void ShowBanner()
        {
            Console.SetWindowSize(width + 10, height + panel);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false; //tắt con trỏ nháy nháy
            Console.WriteLine("===SNAKE GAME===");
            Console.WriteLine("Press any key to play");
            Console.WriteLine("Tips: - Press P key to PAUSE game");
            Console.WriteLine("      - Press R key to RESET game");
            Console.WriteLine("      - Press Q key to QUIT game");

            keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.Q)
                Environment.Exit(0);
        }

        //Cài đặt thông số ban đầu
        void Setup()
        {
            dir = "RIGHT"; pre_dir = "";    //bước đi đầu tiên qua phải 
            score = 0; nTail = 10;
            gameOver = reset = isprinted = false;
            headX = 20; //ko vuot qua width (vi tri bat dau Ran)
            headY = 10; //ko vuot qua height(vi tri bat dau Ran)
            randonQua();
        }

        //Sinh ngẫu nhiên vị trí quả
        void randonQua()
        {
            int fruitScore5 = 5, fruitScore10 = 10;
            int randFruit = rand.Next(1, 3);
            Console.WriteLine(randFruit);
            if (randFruit == 1)
            {
                scorePlus = fruitScore5;
                fruit = "@";
            }
            else if (randFruit == 2)
            {
                scorePlus = fruitScore10;
                fruit = "$";
            }
            fruitX = rand.Next(1, width - 1);
            fruitY = rand.Next(1, height - 1);
        }

        //Cập nhật màn hình
        void Update()
        {
            while (!gameOver)
            {
                CheckInput();
                Logic();
                Render();
                if (reset) break;
                //DUng man hinh 1s
                Thread.Sleep(30);
            }
            if (gameOver) Lose();
        }

        //Điều khiển phím
        void CheckInput()
        {
            while (Console.KeyAvailable)
            {
                //Cho bam phim bat ky
                keypress = Console.ReadKey(true);
                //dir -> pre_dir
                pre_dir = dir;  //luu lai huong di truoc do
                switch (keypress.Key)
                {
                    case ConsoleKey.Q: Environment.Exit(0); break;
                    case ConsoleKey.P: dir = "PAUSE"; break;
                    case ConsoleKey.LeftArrow:
                        if (pre_dir == "UP" || pre_dir == "DOWN")
                            dir = "LEFT";
                        break;
                    case ConsoleKey.RightArrow:
                        if (pre_dir == "UP" || pre_dir == "DOWN")
                            dir = "RIGHT";
                        break;
                    case ConsoleKey.UpArrow:
                        if (pre_dir == "LEFT" || pre_dir == "RIGHT")
                            dir = "UP";
                        break;
                    case ConsoleKey.DownArrow:
                        if (pre_dir == "LEFT" || pre_dir == "RIGHT")
                            dir = "DOWN";
                        break;
                }
            }

        }

        //Kiểm tra điều hướng
        void Logic()
        {
            int preX = TailX[0], preY = TailY[0]; // (x,y)
            int tempX, tempY;
            //0 1 2 3 4 => Con rắn ăn thêm quà            //x 0 1 2 3 4
            if (dir != "PAUSE")
            {
                TailX[0] = headX; TailY[0] = headY;

                for (int i = 1; i < nTail; i++)
                {
                    tempX = TailX[i]; tempY = TailY[i];
                    TailX[i] = preX; TailY[i] = preY;
                    preX = tempX; preY = tempY;
                }
            }
            switch (dir)
            {
                case "RIGHT": headX++; break;
                case "LEFT": headX--; break;
                case "DOWN": headY++; break;
                case "UP": headY--; break;
                case "STOP":
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Game pause!");
                            Console.WriteLine("- Press P key to PAUSE game");
                            Console.WriteLine("- Press R key to RESET game");
                            Console.WriteLine("- Press Q key to QUIT game");

                            keypress = Console.ReadKey(true);
                            if (keypress.Key == ConsoleKey.Q)
                                Environment.Exit(0);
                            if (keypress.Key == ConsoleKey.R)
                            {
                                reset = true; break; //choi lai game
                            }
                            if (keypress.Key == ConsoleKey.P)
                                break;  //choi tiep game
                        }
                    }
                    dir = pre_dir; break;
            }
            //kiem tra va cham bien (le trai, phai)
            if (headX <= 0 || headX >= width - 1 ||
                headY <= 0 || headY >= height - 1) gameOver = true;
            else gameOver = false;
            //kiem tra an qua
            if (headX == fruitX && headY == fruitY)
            {
                score += scorePlus; nTail++;    //tinh diem khi an qua
                randonQua();            //random diem qua moi
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((headX > 3 && headX < 10) && (headY > 4 && headY < 7)) gameOver = true;
                }
            }
            //kiem tra cai dau va cham than con ran
            for (int i = 1; i < nTail; i++)
            {
                if (headX == TailX[i] && headY == TailY[i])
                {
                    gameOver = true;
                }
                if (fruitX == TailX[i] && fruitY == TailY[i]) //quà trùng thân con rắn cho random lại
                    randonQua();
            }
        }

        //Hiển thị màn hình
        void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("=");
                        Console.ForegroundColor = ConsoleColor.Green;//viền khung trên và dưới
                    }
                    else if (j == 0 || j == width - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("||");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }//viền khung trái và phải
                    else if ((j > 3 && j < 10) && (i > 4 && i < 7))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("*"); //tường
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (j == fruitX && i == fruitY) //hộp quả 
                        Console.Write(fruit);
                    else if (j == headX && i == headY) 
                    { 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }//đầu con rắn
                    
                    else
                    {
                        isprinted = false;
                        for (int k = 0; k < nTail; k++)
                        {
                            if (TailX[k] == j && TailY[k] == i)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("0"); //thân con rắn
                                Console.ForegroundColor = ConsoleColor.Green;

                                isprinted = true;
                            }
                        }
                        if (!isprinted)
                        {
                            Console.Write(" "); //vị trí còn lại
                        }

                    }
                }
                Console.WriteLine();
            }
            //Hiển thị panel thông tin điểm phía dưới khung viền
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Your score: " + score);
        }

        //Xử lý khi thua
        void Lose()
        {
            Console.WriteLine("YOU DIED");
            Console.WriteLine("      - Press R key to RESET game");
            Console.WriteLine("      - Press Q key to QUIT game");

            while (true)
            {
                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.Q)
                    Environment.Exit(0);
                if (keypress.Key == ConsoleKey.R)
                    reset = true; break;
            }
        }

        static void Main(string[] args)
        {
            SnakeGameNe snakegame = new SnakeGameNe(); //game ko xac dinh diem dung
            snakegame.ShowBanner();
            while (true)
            {
                snakegame.Setup();
                snakegame.Update();
                Console.Clear(); //Xoa man hinh hien thi
            }
        }
    }
}
