using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_游戏头
{
    class Program
    {
        //存地图的数组
        public static int[] Maps = new int[100];
        public static int[] PlayPos = new int[2];
        public static string[] PlayerNames = new string[2];
        static void Main(string[] args)
        {
            GameShow();
            #region 玩家姓名输入
            Console.WriteLine("请输入玩家A的姓名：");
            PlayerNames[0] = Console.ReadLine();
            while (PlayerNames[0] == "")
            {
                Console.WriteLine("玩家姓名不能为空！ 请重新输入：");
                PlayerNames[0] = Console.ReadLine();
            }
            Console.WriteLine("请输入玩家B的姓名：");
            PlayerNames[1] = Console.ReadLine();
            while (PlayerNames[1] == "" || PlayerNames[1] == PlayerNames[0])
            {
                if (PlayerNames[1] == "")
                {
                    Console.WriteLine("玩家姓名不能为空，请重新输入：");
                    PlayerNames[1] = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("玩家B的姓名不能喝玩家A的姓名相同！ 请重新输入：");
                    PlayerNames[1] = Console.ReadLine();
                }
            }
            #endregion
            Console.Clear();
            GameShow();
            Console.WriteLine("{0}的士兵用A表示。",PlayerNames[0]);
            Console.WriteLine("{0}的士兵用B表示。",PlayerNames[1]);
            InMaps();
            DrawMaps();
            
            Console.ReadKey();
        }
        /// <summary>
        /// 画游戏头
        /// </summary>
        public static void GameShow()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("******************");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************");
            Console.WriteLine("******************");
            Console.WriteLine("**李柏鹤学习飞行棋*");
            Console.WriteLine("******************");
            Console.WriteLine("******************");
        }


        /// <summary>
        /// 初始化地图
        /// </summary>
        public static void InMaps()
        {
            int[] luckyturn = {6,23,40,55,69,83};
            for (int i = 0; i < luckyturn.Length; i++)
            { 
                 Maps[luckyturn[i]]=1;
            }
            int[] landMine = {5,13,17,33,38,50,64,80,94 };
            for (int i = 0; i < landMine.Length; i++)
            {
                Maps[landMine[i]] = 2;
            }
            int[] pause = {9,27,60,93 };
            for (int i = 0; i < pause.Length; i++)
            {
                Maps[pause[i]] = 3;
            }
            int[] timeTunnel = {20,25,45,63,72,88,90 };
            for (int i = 0; i < timeTunnel.Length; i++)
            {
                Maps[timeTunnel[i]] = 4;
            }
        }


        /// <summary>
        /// 画地图
        /// </summary>
        public static void DrawMaps()
        {
            Console.WriteLine("图例： 幸运轮盘：○  地雷：☆  暂停：△  时空隧道：-");
            #region //第一行地图
            for (int i = 0; i < 30; i++)
            {
                Console.Write(DrawStringMaps(i));
            }
            Console.WriteLine();
            #endregion            
            #region //第一列地图
            for (int i = 30; i < 35; i++)
            {
                for (int j = 0; j < 29; j++)
                {
                    Console.Write("  ");
                }
                Console.WriteLine(DrawStringMaps(i));
            }
            #endregion
            #region//第二行地图
            for (int i = 64; i >= 35; i--)
            {
                Console.Write(DrawStringMaps(i));
            }
            Console.WriteLine();
            #endregion
            #region//第二列地图
            for (int i = 65; i < 70; i++)
            {
                Console.WriteLine(DrawStringMaps(i));
            }
            #endregion
            #region//第三行地图
            for (int i = 70; i < 100; i++)
            {
                Console.Write(DrawStringMaps(i));
            }
            Console.WriteLine();
            #endregion
        }


        /// <summary>
        /// 画地图路径上的符号
        /// </summary>
        /// <param name="i">位置</param>
        /// <returns>符号</returns>
        public static string DrawStringMaps(int i)
        {
            string str = "";
            if (PlayPos[0] == PlayPos[1] && PlayPos[0] == i)
            {
                Console.Write("<>");
            }
            else if (PlayPos[0] == i)
            {
               str="A";
            }
            else if (PlayPos[1] == i)
            {
                str="B";
            }
            else
            {
                switch (Maps[i])
                {
                    case 0: str="□";
                        break;
                    case 1:str="○";
                        break;
                    case 2: str="☆";
                        break;
                    case 3: str="△";
                        break;
                    case 4: str="-";
                        break;
                }
            }
            return str;
        }


    }
}
