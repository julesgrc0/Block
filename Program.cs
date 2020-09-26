using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;

namespace JustConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            new ConsoleDetail();
            ShowMenu();
        }

        static void ShowMenu()
        {
            new GameMenu("██");
            string[] pos = { };
            string userPa = String.Empty;

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.D)
                {
                    StartGame(pos,userPa);
                    break;
                }else if (Console.ReadKey(true).Key == ConsoleKey.L)
                {
                    Console.CursorVisible = true;
                    Console.Write("\nEnter your file path and press enter: ");
                    string userPath= Console.ReadLine();
                    Console.CursorVisible = false;
                    if (File.Exists(userPath))
                    {
                        userPa = userPath;
                        List<string> list = new List<string>();
                        using (StreamReader sr = File.OpenText(userPath))
                        {
                            string s = "";
                            while ((s = sr.ReadLine()) != null)
                            {
                                list.Add(s);
                            }
                            pos = list.ToArray();
                        }
                        Console.WriteLine("\n\n     >>>  Press \"D\" to start the game <<<");
                        StartGame(pos, userPa);
                        break;
                    }
                    Console.CursorVisible = false;
                }
            }
        }

        static void StartGame(string[] posE,string userPath)
        {
            string[] pos = posE;
            int[] actual = { Console.CursorLeft, Console.CursorTop };
            bool HavePressD = false;
            int[] dir = { 0,0,1,0 }; // left right top bottom
            int lasth = Console.WindowHeight;
            int lastw = Console.WindowWidth;
            int acth = Console.WindowHeight;
            int actw = Console.WindowWidth;
            string[] map = { };
            int PlayerX = 0;
            int PlayerY = 0;
            string HaveLock = String.Empty;
            //MakeMap m = new MakeMap(actw, acth, map, false);
            //map = m.getMap();
            
            while (true)
            {
               // MakeMap actMake = new MakeMap(actw, acth, map, true);
               // actMake.SetCursorTo(PlayerX, PlayerY);
                acth = Console.WindowHeight;
                actw = Console.WindowWidth;
                if(actw != lastw || acth != lasth)
                {
                    lasth = acth;
                    lastw = actw;
                    Console.CursorVisible = false;
                   // m = new MakeMap(actw, acth, map, false);
                   //  map = m.getMap();
                   // m.SetCursorTo(PlayerX, PlayerY);
                    Console.Clear();
                }
                    Console.CursorVisible = false;
                    Movement move = new Movement("██", Console.ReadKey(true).Key,pos,actual,dir,HaveLock,userPath,HavePressD);
                    pos = move.pos;
                    actual = move.actual;
                    HavePressD = move.HavePressD;
                    dir = move.dir;
                    PlayerX = move.leftPos;
                    PlayerY = move.TopPos;
                    HaveLock = move.HaveLock;
            }
        }
    }
    class ConsoleDetail
    {
        
       /* private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();*/

        public ConsoleDetail()
        {
            Console.Title = "Block";
            Console.CursorVisible = false;
           // Console.SetWindowSize(70, 30);
            /*IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {                                                       
                DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }*/
        }
    }

    class GameMenu
    {
        private string pixel;

        public GameMenu(string pixel)
        {
            this.pixel = pixel;
            this.Intro();
        }

        private void Intro()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"     ________   ___        ________   ________   ___  ___");
            Console.WriteLine(@"    |\   __  \ |\  \      |\   __  \ |\   ____\ |\  \ \  \  ");
            Console.WriteLine(@"    \ \  \|\ /_\ \  \     \ \  \|\  \\ \  \___| \ \  \/  /|_");
            Console.WriteLine(@"     \ \   __  \\ \  \     \ \  \\\  \\ \  \     \ \   ___  \  ");
            Console.WriteLine(@"      \ \  \|\  \\ \  \____ \ \  \\\  \\ \  \____ \ \  \\ \  \ ");
            Console.WriteLine(@"       \|_______| \|_______| \|_______| \|_______| \|__| \|__|");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n");
            string msg = "  Hi " + Environment.UserName + ", welcome to block ! This game hase been made by julesG10 (github)\n\n \"C\" =  change block\n \"D\"= preview next block\n \"S\"= save block at position\n \"ESC\" =  exit game \n \"E\" = save the game in a file \n \"L\" = lock\\unlock on block \n \"A\" = place tree \n \"W\" = place water\n \"P\" = remove last element \n \"U\" = refresh content \n (Use Arrows to move)\n";
            string center = ">> Press D to start the game or L for load your game <<\n\n\n";
            Console.WriteLine(msg);
            int calc = (Console.WindowWidth-center.Length)/2;
            Console.CursorLeft = calc;
            Console.WriteLine(center);
            Console.CursorVisible = false;
        }
    }

    class Movement
    {
        private string pixel;
        public string[] pos;
        public int[] actual;
        public bool HavePressD = false;
        public int[] dir;
        public int leftPos;
        public int TopPos;
        public string HaveLock = String.Empty;
        private string userPath = String.Empty;
        public Movement(string pixel, ConsoleKey key, string[] pos, int[] actual,int[] direction,string HaveLock,string userPath, bool HavePressD = false)
        {
            this.userPath = userPath;
            this.HaveLock = HaveLock;
            this.HavePressD = HavePressD;
            this.pos = pos;
            this.pixel = pixel;
            this.actual = actual;
            this.dir = direction;
            this.Move(key);
        }

        public void SetCursorTo(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        private void User()
        {
            Console.Write(pixel);
        }

        private void Update()
        {
            Console.Clear();
            foreach (string po in pos)
            {
                string[] arrP = po.Split(",");
                Console.SetCursorPosition(int.Parse(arrP[0]), int.Parse(arrP[1]));
                if (arrP[2] == "██")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(arrP[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (arrP[2] == "::")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(arrP[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(arrP[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        
        private void Move(ConsoleKey key)
        {
            int left =  Console.CursorLeft >= 2 ? Console.CursorLeft-2 : Console.CursorLeft;
            int top  =  Console.CursorTop;
            int topE = top;
            int leftE = left;
            string SymbolDefault = "__,top";

            Console.Clear();
            foreach(string po in pos)
            {
               string[] arrP = po.Split(",");
                Console.SetCursorPosition(int.Parse(arrP[0]), int.Parse(arrP[1]));
                if(arrP[2]== "██")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(arrP[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                }else if(arrP[2] == "::")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(arrP[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(arrP[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            if(dir[0] == 1)
            {
                SymbolDefault = "|  ,left";
            }else if(dir[1] == 1)
            {
                SymbolDefault = " |,right";
            }
            else if(dir[2] == 1)
            {
                SymbolDefault = "__,top";
            }
            else if(dir[3] == 1)
            {
                SymbolDefault = "__,bottom";
            }

            if(HaveLock != String.Empty)
            {
                SymbolDefault = HaveLock;
            }

            try
            {
                switch (key)
                {
                    case ConsoleKey.D:

                        Console.SetCursorPosition(MovePrePos(SymbolDefault, top, left)[1], MovePrePos(SymbolDefault, top, left)[0]);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(SymbolDefault.Split(",")[0]);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(left, top);
                        if (HavePressD)
                        {
                            HavePressD = false;
                        }
                        else
                        {
                            HavePressD = true;
                        }
                        break;
                    case ConsoleKey.S:
                        Console.SetCursorPosition(left, top - 2);
                        List<string> list = new List<string>();
                        list.Add(left + "," + (top - 2) + "," + SymbolDefault);
                        foreach (string po in pos)
                        {
                            list.Add(po);
                        }
                        pos = list.ToArray();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("__");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(left, top);
                        break;
                    case ConsoleKey.U:
                        this.Update();
                        break;
                    case ConsoleKey.P:
                        List<string> listS = new List<string>();
                        for(int i = 0; i < pos.Length; i++)
                        {
                            if(i!=0)
                            {
                                listS.Add(pos[i]);
                            }
                        }
                        pos = listS.ToArray();
                        this.Update();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (!((left - 2) < 2))
                        {
                            Console.SetCursorPosition(left - 2, top);
                            leftE = left - 2;
                        }
                        dir[0] = 1;
                        dir[1] = 0;
                        dir[2] = 0;
                        dir[3] = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        Console.SetCursorPosition(left + 2, top);
                        leftE = left + 2;
                        dir[0] = 0;
                        dir[1] = 1;
                        dir[2] = 0;
                        dir[3] = 0;
                        break;
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(left, (top - 2));
                        topE = top - 2;
                        dir[0] = 0;
                        dir[1] = 0;
                        dir[2] = 1;
                        dir[3] = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(left, top + 2);
                        topE = top + 2;
                        dir[0] = 0;
                        dir[1] = 0;
                        dir[2] = 0;
                        dir[3] = 1;
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.C:
                        ChangeDir();
                        break;
                    case ConsoleKey.E:
                        if (!SaveGame(this.userPath))
                        {
                            SaveGame(this.userPath);
                        }
                        break;
                    case ConsoleKey.L:
                        if (HaveLock == String.Empty)
                        {
                            HaveLock=SymbolDefault;
                        }
                        else
                        {
                            HaveLock = String.Empty;
                        }
                        break;
                    case ConsoleKey.A:
                        SymbolDefault = "██";
                        Console.SetCursorPosition(left, top - 2);
                        List<string> list2 = new List<string>();
                        list2.Add(left + "," + (top - 2) + "," + SymbolDefault);
                        foreach (string po in pos)
                        {
                            list2.Add(po);
                        }
                        pos = list2.ToArray();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("██");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(left, top);
                        break;
                    case ConsoleKey.W:
                        SymbolDefault = "::";
                        Console.SetCursorPosition(left, top - 2);
                        List<string> list3 = new List<string>();
                        list3.Add(left + "," + (top - 2) + "," + SymbolDefault);
                        foreach (string po in pos)
                        {
                            list3.Add(po);
                        }
                        pos = list3.ToArray();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("::");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(left, top);
                        break;
                    default:
                        Console.SetCursorPosition(left, top);
                        break;
                }
            }
            catch  {
               
            }
            if (HavePressD)
            {
                try
                {
                    Console.SetCursorPosition(MovePrePos(SymbolDefault, topE, leftE)[1], MovePrePos(SymbolDefault, topE, leftE)[0]);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(SymbolDefault.Split(",")[0]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(leftE, topE);
                }
                catch { }
                
            }
            else
            {
                try
                {
                    Console.SetCursorPosition(MovePrePos(SymbolDefault, topE, leftE)[1], MovePrePos(SymbolDefault, topE, leftE)[0]);
                    Console.Write("");
                    Console.SetCursorPosition(leftE, topE);
                }
                catch { }
            }
            this.TopPos = topE;
            this.leftPos = leftE;
            this.User();
        }

        private bool SaveGame(string userPa)
        {

            DateTime date = new DateTime();
            string date_str = date.ToString("dd-MM-yyyy");
            string path = AppDomain.CurrentDomain.BaseDirectory+"\\game-"+date_str+".txt";
            if(userPa != String.Empty)
            {
                path = userPa;
            }
            if (!File.Exists(path))
            {
                MakeFile(path,pos);
                return false;
            }
            else
            {
                WriteFile(path, pos);
                return true;
            }
        }
        private void MakeFile(string path,string[] lines)
        {
            using (StreamWriter fs = File.AppendText(path))
            {
                foreach (string line in lines)
                {
                    fs.Write(line);
                }
            }
        }

        private void WriteFile(string path,string[] lines)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        private void ChangeDir()
        {
            if (dir[0] == 1) // left
            {
                dir[0] = 0; // left
                dir[1] = 1; // right
                dir[2] = 0; // top
                dir[3] = 0; // bottom
            }
            else if (dir[1] == 1) // right
            {
                dir[0] = 0;
                dir[1] = 0;
                dir[2] = 1;
                dir[3] = 0;
            }
            else if (dir[2] == 1) // top
            {
                dir[0] = 0;
                dir[1] = 0;
                dir[2] = 0;
                dir[3] = 1;
            }
            else if (dir[3] == 1) // bottom
            {
                dir[0] = 1;
                dir[1] = 0;
                dir[2] = 0;
                dir[3] = 0;
            }
        }


        private int[] MovePrePos(string symbol,int top,int left)
        {
            string[] sys = symbol.Split(",");
            /*switch (sys[1])
            {
                case "top":
                    top -= 2;
                    break;
                case "bottom":
                    top += 1;
                    break; 
                case "left":
                    left -= 2;
                    break;
                case "right":
                    left += 2;
                    break;
            }*/
            int[] ret = { top -=2, left };
            return ret;
        }
    }

    class MakeMap
    {

        public int w;
        public int h;
        private string[] map;

        public MakeMap(int w,int h, string[] map,bool haveMap=false)
        {
            this.w = w;
            this.h = h;
            if (!haveMap)
            {
                this.randomOnMap(100);
            }
            else
            {
                this.MakeMapFromPos(map);
            }
                
        }
        public void SetCursorTo(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        private void MakeMapFromPos(string[] mapg)
        {
            for (int i = 0; i < mapg.Length; i++)
            {
                int x = int.Parse(mapg[i].Split(",")[0]);
                int y = int.Parse(mapg[i].Split(",")[1]);
                Console.SetCursorPosition(x,y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("██");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public string[] getMap()
        {
            return map;
        }

        private readonly Random _random = new Random();  
        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        private void randomOnMap(int ge)
        {
            for(int i = 0; i < ge; i++)
            {
                int posY = RandomNumber(0, h);
                int posX = RandomNumber(0, w);
                List<string> list = new List<string>();
                list.Add(posX + "," + posY);
                if(map != null)
                {
                    foreach (string po in map)
                    {
                        list.Add(po);
                    }
                }
                    
                map = list.ToArray();
                Console.SetCursorPosition(posX, posY);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("██");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

    }

}
