using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            double pc = 0.3;
            double pm = 0.2;
            int tasknumber = 40;//任务数量
            int populationsize = 40;//种群数量
            int crossnumber = 16;//路口数量
            int trucklength = 5;//平板车数量
            int duiweinumber = 231;//堆位数量
            double[] fitarray = new double[populationsize];
            double[] value_array = new double[5];//接受每次禁忌搜索的参数。
            int[] trucknumber = new int[trucklength];
            int candidate_length = 40;
            int tabulength = 28;
            double[,] candidate_array = new double[candidate_length, 7];
            double[,] teshe_array = new double[tabulength, 7];
            int GAiteration = 100;
            int tabuiteration = 300;
            int NIiteration = 100;
            double tempbest = 0;
            int tempbestnumber = 0;
            string calculate_way;
            double[] tabufitarray = new double[tabuiteration + 1];
            int[,] tabuSearch_task = new int[tabuiteration + 1, tasknumber];
            int[,] tabuSearch_truck = new int[tabuiteration + 1, tasknumber];
            int bestnumber = 0;
            Genetic GA = new Genetic(populationsize, tasknumber, crossnumber, trucklength, duiweinumber);
            GA.initialtest();//读取基础数据
            //基于遗传算法的调度方式
            GA.createfirstpop();//产生第一代种群
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < populationsize; i++)
            {
                fitarray[i] = GA.calfitness(i);
            }
            for (int i = 0; i < populationsize; i++)
            {
                if ((GA.popsizepath).Count != 0)
                {
                    (GA.popsizepath).RemoveAt(0);
                }
                if ((GA.taskstarttime).Count != 0)
                {
                    (GA.taskstarttime).RemoveAt(0);
                }
                if ((GA.taskendtime).Count != 0)
                {
                    (GA.taskendtime).RemoveAt(0);
                }
            }
            for (int daishu = 1; daishu < GAiteration + 1; daishu++)
            {
                Console.Write(daishu + "  ");
                GA.savepre();
                GA.wheelselect((int)(populationsize * (pc + pm)));
                GA.createnextpop();
                GA.producenext();
                for (int i = 0; i < populationsize; i++)
                {
                    fitarray[i] = GA.calfitness(i);
                }
                int sick = GA.modify();
                if (sick >= 0)
                {
                    Console.WriteLine("个体修复失败！");
                }
                if (daishu>=100)
                {
                    double tempbestfitness = 5000;
                    int tempcount = 0;
                    for (int i = 0; i < populationsize; i++)
                    {
                        if (tempbestfitness > fitarray[i])
                        {
                            tempbestfitness = fitarray[i];
                            tempcount = i;
                        }
                    }
                    Console.WriteLine("目标函数值为：" + tempbestfitness + "  ");
                }
                if (daishu < GAiteration)
                {
                    for (int i = 0; i < populationsize; i++)
                    {
                        if ((GA.popsizepath).Count != 0)
                        {
                            (GA.popsizepath).RemoveAt(0);
                        }
                        if ((GA.taskstarttime).Count != 0)
                        {
                            (GA.taskstarttime).RemoveAt(0);
                        }
                        if ((GA.taskendtime).Count != 0)
                        {
                            (GA.taskendtime).RemoveAt(0);
                        }
                    }
                }
            }
                double bestfitness = 5000;
                int count = 0;
                for (int i = 0; i < populationsize; i++)
                {
                    if (bestfitness > fitarray[i])
                    {
                        bestfitness = fitarray[i];
                        count = i;
                    }
                }
                Console.WriteLine("目标函数值为：" + bestfitness);
                for (int j = 0; j < tasknumber; j++)
                {
                    Console.Write(GA.pregegeration[count, j] + " ");
                }
                Console.WriteLine();
                for (int j = 0; j < tasknumber; j++)
                {
                    Console.Write(GA.truckpregegeration[count, j] + " ");
                }
                int starttimecount = 0;
                int endtimecount = 0;
                for (int i = 0; i < tasknumber; i++)
                {
                    if (GA.taskstarttime[count][i] < GA.estime[i])
                    {
                        starttimecount += 1;
                    }
                    if (GA.taskendtime[count][i] > GA.letime[i])
                    {
                        endtimecount += 1;
                    }
                }
                Console.WriteLine();
                Console.WriteLine(starttimecount + "  " + endtimecount);
                sw.Stop();
                Console.WriteLine("求解时间：" + sw.Elapsed);
                tempbest = bestfitness;
                Console.WriteLine("遗传算法结束！开始启动禁忌搜索算法");
                //此时运行完遗传算法，开始调用禁忌搜索算法
                Console.WriteLine("请选择禁忌搜索方式：1.TabuSearch_mutation  2.TabuSearch_cross  3.混合  4随机");
                calculate_way = Console.ReadLine();
                #region//第一种禁忌搜索方式
                if (calculate_way == "1")
                {
                    tabufitarray[0] = bestfitness;
                    for (int i = 0; i < tasknumber; i++)
                    {
                        tabuSearch_task[0, i] = GA.pregegeration[count, i];
                        tabuSearch_truck[0, i] = GA.truckpregegeration[count, i];
                    }
                    for (int daishu = 0; daishu < tabuiteration; daishu++)
                    {
                        Console.Write(daishu + "    ");
                        //禁忌表中禁忌元素长度减少1
                        if (GA.tabutable.Count != 0)
                        {
                            int num = GA.tabutable.Count;
                            for (int i = 0; i < num; i++)
                            {
                                GA.tabucount[i] -= 1;
                            }
                        }
                        //如果禁忌元素的长度等于0，则将该元素从禁忌表中移除
                        if (GA.tabutable.Count != 0)
                        {
                            if (GA.tabucount[0] == 0)
                            {
                                GA.tabutable.RemoveAt(0);
                                GA.tabucount.RemoveAt(0);
                                GA.tabutable.RemoveAt(0);
                                GA.tabucount.RemoveAt(0);
                            }
                        }
                        //将此刻的最优染色体保存起来
                        GA.tabusavepre(count);
                        //构建候选集，candidate_length个元素
                        int j = 0;
                        while (j < candidate_length)
                        {
                            for (int i = 0; i < tasknumber; i++)
                            {
                                GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                            }
                            value_array = GA.TabuSearch_mutation(count,false);//返回选中基因的位置，任务号，平板车号，目标函数，时间窗始点，时间窗终点。
                            for (int i = 0; i < 7; i++)
                            {
                                candidate_array[j, i] = value_array[i];
                            }
                            //建立候选集合  任务号-任务号
                            GA.candidatestringCollection.Add(candidate_array[j, 2].ToString() + "-" + candidate_array[j, 3].ToString());
                            GA.candidatestringCollection.Add(candidate_array[j, 3].ToString() + "-" + candidate_array[j, 2].ToString());
                            j++;
                        }
                        while (GA.candidateCollection.Count != 0)
                        {
                            GA.candidateCollection.RemoveAt(0);
                        }
                        double op_fit = 50000;
                        int op_one = -1;
                        for (int i = 0; i < candidate_length; i++)
                        {
                            if (op_fit > candidate_array[i, 6])
                            {
                                op_fit = candidate_array[i, 6];
                                op_one = i;
                            }
                        }
                        tabufitarray[daishu + 1] = op_fit;
                        //特赦规则，如果执行禁忌表中的元素，可以得到最好解，那么则将该元素最为最优解。
                        if (daishu > 0)
                        {
                            int teshe = 0;
                            while (teshe < tabulength)
                            {
                                for (int i = 0; i < tasknumber; i++)
                                {
                                    GA.pregegeration[count, i] = GA.tabutemppre[i];
                                    GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                                }
                                value_array = GA.TabuSearch_mutation(count, true);
                                for (int i = 0; i < 9; i++)
                                {
                                    teshe_array[teshe, i] = value_array[i];
                                }
                                //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                                string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                                string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                                GA.teshetable.Add(firststr);
                                GA.teshetable.Add(secondstr);
                                teshe++;
                            }
                            while (GA.teshetable.Count != 0)
                            {
                                GA.teshetable.RemoveAt(0);
                            }
                            double tesheop_fit = 5000;
                            int tesheop_one = -1;
                            for (int i = 0; i < tabulength; i++)
                            {
                                if (tesheop_fit > teshe_array[i, 6])
                                {
                                    tesheop_fit = candidate_array[i, 6];
                                    tesheop_one = i;
                                }
                            }
                            double tempb = 5000;
                            for (int i = 0; i < daishu + 2; i++)
                            {
                                if (tempb > tabufitarray[i])
                                {
                                    tempb = tabufitarray[i];
                                }
                            }
                            if (tesheop_fit < tempb)
                            {
                                tabufitarray[daishu + 1] = tesheop_fit;
                                candidate_array[op_one, 0] = teshe_array[tesheop_one, 0];
                                candidate_array[op_one, 1] = teshe_array[tesheop_one, 1];
                                candidate_array[op_one, 2] = teshe_array[tesheop_one, 2];
                                candidate_array[op_one, 3] = teshe_array[tesheop_one, 3];
                                candidate_array[op_one, 4] = teshe_array[tesheop_one, 4];
                                candidate_array[op_one, 5] = teshe_array[tesheop_one, 5];
                                candidate_array[op_one, 7] = teshe_array[tesheop_one, 7];
                                candidate_array[op_one, 8] = teshe_array[tesheop_one, 8];
                            }
                        }
                        //将平板车序列恢复至开始状态
                        for (int i = 0; i < tasknumber; i++)
                        {
                            GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                        }
                        //将候选集中最优的个体保留起来，最为这次搜索的最优解
                        GA.truckpregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 4];
                        GA.truckpregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 5];
                        //保存禁忌搜索最优解序列和最优解目标函数值
                        for (int i = 0; i < tasknumber; i++)
                        {
                            tabuSearch_task[daishu + 1, i] = GA.pregegeration[count, i];
                            tabuSearch_truck[daishu + 1, i] = GA.truckpregegeration[count, i];
                        }
                        //更新禁忌表
                        GA.tabutablestring.Add(candidate_array[op_one, 2].ToString() + "-" + candidate_array[op_one, 3].ToString());
                        GA.tabucount.Add(tabulength);
                        GA.tabutablestring.Add(candidate_array[op_one, 3].ToString() + "-" + candidate_array[op_one, 2].ToString());
                        GA.tabucount.Add(tabulength);
                        //如果连续30代最优解没有变化，那么停止迭代。
                        if (tabufitarray[daishu + 1] < tempbest)
                        {
                            tempbest = tabufitarray[daishu + 1];
                            tempbestnumber = daishu;
                        }
                        if ((daishu - tempbestnumber) > NIiteration)
                        {
                            break;
                        }
                    }
                    for (int i = 1; i < tabuiteration + 1; i++)
                    {
                        if ((bestfitness > tabufitarray[i]) && tabufitarray[i] != 0)
                        {
                            bestfitness = tabufitarray[i];
                            bestnumber = i;
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("禁忌搜索后的最优目标函数值为：" + bestfitness);
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_task[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_truck[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                }
                #endregion//
                #region//第二种禁忌搜索方式
                else if (calculate_way == "2")
                {
                    tabufitarray[0] = bestfitness;
                    for (int i = 0; i < tasknumber; i++)
                    {
                        tabuSearch_task[0, i] = GA.pregegeration[count, i];
                        tabuSearch_truck[0, i] = GA.truckpregegeration[count, i];
                    }
                        for (int daishu = 0; daishu < tabuiteration; daishu++)
                        {
                            Console.Write(daishu + "  ");
                            //禁忌表中禁忌元素长度减少1
                            if (GA.tabutablestring.Count != 0)
                            {
                                int num = GA.tabutablestring.Count;
                                for (int i = 0; i < num; i++)
                                {
                                    GA.tabucount[i] -= 1;
                                }
                            }
                            //如果禁忌元素的长度等于0，则将该元素从禁忌表中移除
                            if (GA.tabutablestring.Count != 0)
                            {
                                if (GA.tabucount[0] == 0)
                                {
                                    GA.tabutablestring.RemoveAt(0);
                                    GA.tabucount.RemoveAt(0);
                                    GA.tabutablestring.RemoveAt(0);
                                    GA.tabucount.RemoveAt(0);
                                }
                            }
                            //将此刻的最优染色体保存起来
                            GA.tabusavepre(count);
                            //构建候选集，10个元素
                            int j = 0;
                            while (j < candidate_length)
                            {
                                for (int i = 0; i < tasknumber; i++)
                                {
                                    GA.pregegeration[count, i] = GA.tabutemppre[i];
                                    GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                                }
                                value_array = GA.TabuSearch_cross(count, false);
                                for (int i = 0; i < 7; i++)
                                {
                                    candidate_array[j, i] = value_array[i];
                                }
                                //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                                string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                                string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                                GA.candidatestringCollection.Add(firststr);
                                GA.candidatestringCollection.Add(secondstr);
                                j++;
                            }
                            while (GA.candidatestringCollection.Count != 0)
                            {
                                GA.candidatestringCollection.RemoveAt(0);
                            }
                            double op_fit = 5000;
                            int op_one = -1;
                            for (int i = 0; i < candidate_length; i++)
                            {
                                if (op_fit > candidate_array[i, 6])
                                {
                                    op_fit = candidate_array[i, 6];
                                    op_one = i;
                                }
                            }
                            tabufitarray[daishu + 1] = op_fit;
                            //特赦规则，如果执行禁忌表中的元素，可以得到最好解，那么则将该元素最为最优解。
                            if (daishu > 0)
                            {
                                int teshe = 0;
                                while (teshe < tabulength)
                                {
                                    for (int i = 0; i < tasknumber; i++)
                                    {
                                        GA.pregegeration[count, i] = GA.tabutemppre[i];
                                        GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                                    }
                                    value_array = GA.TabuSearch_cross(count, true);                  
                                    for (int i = 0; i < 7; i++)
                                    {
                                        teshe_array[teshe, i] = value_array[i];
                                    }
                                    //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                                    string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                                    string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                                    GA.teshetable.Add(firststr);
                                    GA.teshetable.Add(secondstr);
                                    teshe++;
                                }
                                while (GA.teshetable.Count != 0)
                                {
                                    GA.teshetable.RemoveAt(0);
                                }
                                double tesheop_fit = 5000;
                                int tesheop_one = -1;
                                for (int i = 0; i < tabulength; i++)
                                {
                                    if (tesheop_fit > teshe_array[i, 6])
                                    {
                                        tesheop_fit = candidate_array[i, 6];
                                        tesheop_one = i;
                                    }
                                }
                                double tempb = 5000;
                                for (int i = 0; i < daishu + 2; i++)
                                {
                                    if (tempb > tabufitarray[i])
                                    {
                                        tempb = tabufitarray[i];
                                    }
                                }
                                if (tesheop_fit < tempb)
                                {
                                    tabufitarray[daishu + 1] = tesheop_fit;
                                    candidate_array[op_one, 0] = teshe_array[tesheop_one, 0];
                                    candidate_array[op_one, 1] = teshe_array[tesheop_one, 1];
                                    candidate_array[op_one, 2] = teshe_array[tesheop_one, 2];
                                    candidate_array[op_one, 3] = teshe_array[tesheop_one, 3];
                                    candidate_array[op_one, 4] = teshe_array[tesheop_one, 4];
                                    candidate_array[op_one, 5] = teshe_array[tesheop_one, 5];
                                }
                            }
                            //保存最优解
                            for (int i = 0; i < tasknumber; i++)
                            {
                                GA.pregegeration[count, i] = GA.tabutemppre[i];
                                GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                            }
                            //将候选集中最优的个体保留起来，最为这次搜索的最优解
                            GA.pregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 2];
                            GA.truckpregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 4];
                            GA.pregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 3];
                            GA.truckpregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 5];
                            //保留此次禁忌搜索最优解
                            for (int i = 0; i < tasknumber; i++)
                            {
                                tabuSearch_task[daishu + 1, i] = GA.pregegeration[count, i];
                                tabuSearch_truck[daishu + 1, i] = GA.truckpregegeration[count, i];
                            }
                            //更新禁忌表
                            GA.tabutablestring.Add(((int)candidate_array[op_one, 2]).ToString() + "-" + ((int)candidate_array[op_one, 3]).ToString());
                            GA.tabucount.Add(tabulength);
                            GA.tabutablestring.Add(((int)candidate_array[op_one, 3]).ToString() + "-" + ((int)candidate_array[op_one, 2]).ToString());
                            GA.tabucount.Add(tabulength);
                            if (tabufitarray[daishu + 1] < tempbest)
                            {
                                tempbest = tabufitarray[daishu + 1];
                                tempbestnumber = daishu;
                            }
                            if ((daishu - tempbestnumber) > NIiteration)
                            {
                                break;
                            }
                        }
                    for (int i = 0; i < tabuiteration; i++)
                    {
                        if ((bestfitness > tabufitarray[i]) && tabufitarray[i] != 0)
                        {
                            bestfitness = tabufitarray[i];
                            bestnumber = i;
                        }
                    }
                    Console.WriteLine("禁忌搜索后的最优目标函数值为："+bestfitness);
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_task[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_truck[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                }
                #endregion
                #region//第三种禁忌搜索方式
                else if (calculate_way == "3")
                {
                    tabufitarray[0] = bestfitness;
                    for (int i = 0; i < tasknumber; i++)
                    {
                        tabuSearch_task[0, i] = GA.pregegeration[count, i];
                        tabuSearch_truck[0, i] = GA.truckpregegeration[count, i];
                    }
                    for (int daishu = 0; daishu < tabuiteration; daishu++)
                    {
                        Console.Write(daishu + "  ");
                        //禁忌表中禁忌元素长度减少1
                        if (GA.tabutablestring.Count != 0)
                        {
                            int num = GA.tabutablestring.Count;
                            for (int i = 0; i < num; i++)
                            {
                                GA.tabucount[i] -= 1;
                            }
                        }
                        //如果禁忌元素的长度等于0，则将该元素从禁忌表中移除
                        if (GA.tabutablestring.Count != 0)
                        {
                            if (GA.tabucount[0] == 0)
                            {
                                GA.tabutablestring.RemoveAt(0);
                                GA.tabucount.RemoveAt(0);
                                GA.tabutablestring.RemoveAt(0);
                                GA.tabucount.RemoveAt(0);
                            }
                        }
                        //将此刻的最优染色体保存起来
                        GA.tabusavepre(count);
                        //构建候选集，10个元素
                        int j = 0;
                        while (j < candidate_length)
                        {
                            for (int i = 0; i < tasknumber; i++)
                            {
                                GA.pregegeration[count, i] = GA.tabutemppre[i];
                                GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                            }
                            if (j<candidate_length/2)
                            {
                                value_array = GA.TabuSearch_cross(count, false);
                            }
                            else
                            {
                                value_array = GA.TabuSearch_mutation(count, false);
                            }
                            for (int i = 0; i < 7; i++)
                            {
                                candidate_array[j, i] = value_array[i];
                            }
                            //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                            string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                            string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                            GA.candidatestringCollection.Add(firststr);
                            GA.candidatestringCollection.Add(secondstr);
                            j++;
                        }
                        while (GA.candidatestringCollection.Count != 0)
                        {
                            GA.candidatestringCollection.RemoveAt(0);
                        }
                        double op_fit = 5000;
                        int op_one = -1;
                        for (int i = 0; i < candidate_length; i++)
                        {
                            if (op_fit > candidate_array[i, 6])
                            {
                                op_fit = candidate_array[i, 6];
                                op_one = i;
                            }
                        }
                        tabufitarray[daishu + 1] = op_fit;
                        //特赦规则，如果执行禁忌表中的元素，可以得到最好解，那么则将该元素最为最优解。
                        if (daishu > 0)
                        {
                            int teshe = 0;
                            while (teshe < GA.tabutablestring.Count() / 2)
                            {
                                for (int i = 0; i < tasknumber; i++)
                                {
                                    GA.pregegeration[count, i] = GA.tabutemppre[i];
                                    GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                                }
                                int num = GA.tabutablestring.Count()/2;
                                if (teshe<num/2)
                                {
                                    value_array = GA.TabuSearch_cross(count, true);
                                }
                                else
                                {
                                    value_array = GA.TabuSearch_mutation(count, true);
                                }
                                for (int i = 0; i < 7; i++)
                                {
                                    teshe_array[teshe, i] = value_array[i];
                                }
                                //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                                string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                                string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                                if (!GA.teshetable.Contains(firststr) && !GA.teshetable.Contains(secondstr))
                                {
                                    GA.teshetable.Add(firststr);
                                    GA.teshetable.Add(secondstr);
                                }
                                teshe++;
                            }
                            while (GA.teshetable.Count != 0)
                            {
                                GA.teshetable.RemoveAt(0);
                            }
                            double tesheop_fit = 5000;
                            int tesheop_one = -1;
                            for (int i = 0; i < tabulength; i++)
                            {
                                if (tesheop_fit > teshe_array[i, 6])
                                {
                                    tesheop_fit = candidate_array[i, 6];
                                    tesheop_one = i;
                                }
                            }
                            double tempb = 5000;
                            for (int i = 0; i < daishu + 2; i++)
                            {
                                if (tempb > tabufitarray[i])
                                {
                                    tempb = tabufitarray[i];
                                }
                            }
                            if (tesheop_fit < tempb)
                            {
                                tabufitarray[daishu + 1] = tesheop_fit;
                                candidate_array[op_one, 0] = teshe_array[tesheop_one, 0];
                                candidate_array[op_one, 1] = teshe_array[tesheop_one, 1];
                                candidate_array[op_one, 2] = teshe_array[tesheop_one, 2];
                                candidate_array[op_one, 3] = teshe_array[tesheop_one, 3];
                                candidate_array[op_one, 4] = teshe_array[tesheop_one, 4];
                                candidate_array[op_one, 5] = teshe_array[tesheop_one, 5];
                            }
                        }
                        //保存最优解
                        for (int i = 0; i < tasknumber; i++)
                        {
                            GA.pregegeration[count, i] = GA.tabutemppre[i];
                            GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                        }
                        //将候选集中最优的个体保留起来，最为这次搜索的最优解
                        GA.pregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 2];
                        GA.truckpregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 4];
                        GA.pregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 3];
                        GA.truckpregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 5];
                        //保留此次禁忌搜索最优解
                        for (int i = 0; i < tasknumber; i++)
                        {
                            tabuSearch_task[daishu + 1, i] = GA.pregegeration[count, i];
                            tabuSearch_truck[daishu + 1, i] = GA.truckpregegeration[count, i];
                        }
                        //更新禁忌表
                        GA.tabutablestring.Add(((int)candidate_array[op_one, 2]).ToString() + "-" + ((int)candidate_array[op_one, 3]).ToString());
                        GA.tabucount.Add(tabulength);
                        GA.tabutablestring.Add(((int)candidate_array[op_one, 3]).ToString() + "-" + ((int)candidate_array[op_one, 2]).ToString());
                        GA.tabucount.Add(tabulength);
                        if (tabufitarray[daishu + 1] < tempbest)
                        {
                            tempbest = tabufitarray[daishu + 1];
                            tempbestnumber = daishu;
                        }
                        Console.WriteLine("目标函数为：" + tempbest);
                        if ((daishu - tempbestnumber) > NIiteration)
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < tabuiteration; i++)
                    {
                        if ((bestfitness > tabufitarray[i]) && tabufitarray[i] != 0)
                        {
                            bestfitness = tabufitarray[i];
                            bestnumber = i;
                        }
                    }
                    Console.WriteLine("禁忌搜索后的最优目标函数值为：" + bestfitness);
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_task[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_truck[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                }
                #endregion
                #region//第四种禁忌搜索方式
                else if (calculate_way == "4")
                {
                    tabufitarray[0] = bestfitness;
                    for (int i = 0; i < tasknumber; i++)
                    {
                        tabuSearch_task[0, i] = GA.pregegeration[count, i];
                        tabuSearch_truck[0, i] = GA.truckpregegeration[count, i];
                    }
                    for (int daishu = 0; daishu < tabuiteration; daishu++)
                    {
                        Console.Write(daishu + "  ");
                        //禁忌表中禁忌元素长度减少1
                        if (GA.tabutablestring.Count != 0)
                        {
                            int num = GA.tabutablestring.Count;
                            for (int i = 0; i < num; i++)
                            {
                                GA.tabucount[i] -= 1;
                            }
                        }
                        //如果禁忌元素的长度等于0，则将该元素从禁忌表中移除
                        if (GA.tabutablestring.Count != 0)
                        {
                            if (GA.tabucount[0] == 0)
                            {
                                GA.tabutablestring.RemoveAt(0);
                                GA.tabucount.RemoveAt(0);
                                GA.tabutablestring.RemoveAt(0);
                                GA.tabucount.RemoveAt(0);
                            }
                        }
                        //将此刻的最优染色体保存起来
                        GA.tabusavepre(count);
                        //构建候选集，10个元素
                        int j = 0;
                        while (j < candidate_length)
                        {
                            for (int i = 0; i < tasknumber; i++)
                            {
                                GA.pregegeration[count, i] = GA.tabutemppre[i];
                                GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                            }
                            Random rand=new Random();
                            int random_number=rand.Next(0,2);
                            if (random_number==0)
                            {
                                value_array = GA.TabuSearch_cross(count, false);
                            }
                            else
                            {
                                value_array = GA.TabuSearch_mutation(count, false);
                            }
                            for (int i = 0; i < 7; i++)
                            {
                                candidate_array[j, i] = value_array[i];
                            }
                            //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                            string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                            string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                            GA.candidatestringCollection.Add(firststr);
                            GA.candidatestringCollection.Add(secondstr);
                            j++;
                        }
                        while (GA.candidatestringCollection.Count != 0)
                        {
                            GA.candidatestringCollection.RemoveAt(0);
                        }
                        double op_fit = 5000;
                        int op_one = -1;
                        for (int i = 0; i < candidate_length; i++)
                        {
                            if (op_fit > candidate_array[i, 6])
                            {
                                op_fit = candidate_array[i, 6];
                                op_one = i;
                            }
                        }
                        tabufitarray[daishu + 1] = op_fit;
                        //特赦规则，如果执行禁忌表中的元素，可以得到最好解，那么则将该元素最为最优解。
                        if (daishu > 0)
                        {
                            int teshe = 0;
                            while (teshe < GA.tabutablestring.Count()/2)
                            {
                                for (int i = 0; i < tasknumber; i++)
                                {
                                    GA.pregegeration[count, i] = GA.tabutemppre[i];
                                    GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                                }
                                Random rand =new Random();
                                int num=rand.Next(0,2);
                                if(num==0)
                                {
                                    value_array = GA.TabuSearch_cross(count, true);
                                }
                                else
                                {
                                    value_array = GA.TabuSearch_mutation(count, true);
                                }
                                for (int i = 0; i < 7; i++)
                                {
                                    teshe_array[teshe, i] = value_array[i];
                                }
                                //建立候选禁忌表，采用“任务号-任务号”的字符串编码
                                string firststr = ((int)value_array[2]).ToString() + "-" + ((int)value_array[3]).ToString();
                                string secondstr = ((int)value_array[3]).ToString() + "-" + ((int)value_array[2]).ToString();
                                if (!GA.teshetable.Contains(firststr) && !GA.teshetable.Contains(secondstr))
                                {
                                    GA.teshetable.Add(firststr);
                                    GA.teshetable.Add(secondstr);
                                }
                                teshe++;
                            }
                            while (GA.teshetable.Count != 0)
                            {
                                GA.teshetable.RemoveAt(0);
                            }
                            double tesheop_fit = 5000;
                            int tesheop_one = -1;
                            for (int i = 0; i < tabulength; i++)
                            {
                                if (tesheop_fit > teshe_array[i, 6])
                                {
                                    tesheop_fit = candidate_array[i, 6];
                                    tesheop_one = i;
                                }
                            }
                            double tempb = 5000;
                            for (int i = 0; i < daishu + 2; i++)
                            {
                                if (tempb > tabufitarray[i])
                                {
                                    tempb = tabufitarray[i];
                                }
                            }
                            if (tesheop_fit < tempb)
                            {
                                tabufitarray[daishu + 1] = tesheop_fit;
                                candidate_array[op_one, 0] = teshe_array[tesheop_one, 0];
                                candidate_array[op_one, 1] = teshe_array[tesheop_one, 1];
                                candidate_array[op_one, 2] = teshe_array[tesheop_one, 2];
                                candidate_array[op_one, 3] = teshe_array[tesheop_one, 3];
                                candidate_array[op_one, 4] = teshe_array[tesheop_one, 4];
                                candidate_array[op_one, 5] = teshe_array[tesheop_one, 5];
                            }
                        }
                        //保存最优解
                        for (int i = 0; i < tasknumber; i++)
                        {
                            GA.pregegeration[count, i] = GA.tabutemppre[i];
                            GA.truckpregegeration[count, i] = GA.tabutemptruckpre[i];
                        }
                        //将候选集中最优的个体保留起来，最为这次搜索的最优解
                        GA.pregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 2];
                        GA.truckpregegeration[count, (int)candidate_array[op_one, 0]] = (int)candidate_array[op_one, 4];
                        GA.pregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 3];
                        GA.truckpregegeration[count, (int)candidate_array[op_one, 1]] = (int)candidate_array[op_one, 5];
                        //保留此次禁忌搜索最优解
                        for (int i = 0; i < tasknumber; i++)
                        {
                            tabuSearch_task[daishu + 1, i] = GA.pregegeration[count, i];
                            tabuSearch_truck[daishu + 1, i] = GA.truckpregegeration[count, i];
                        }
                        //更新禁忌表
                        GA.tabutablestring.Add(((int)candidate_array[op_one, 2]).ToString() + "-" + ((int)candidate_array[op_one, 3]).ToString());
                        GA.tabucount.Add(tabulength);
                        GA.tabutablestring.Add(((int)candidate_array[op_one, 3]).ToString() + "-" + ((int)candidate_array[op_one, 2]).ToString());
                        GA.tabucount.Add(tabulength);
                        if (tabufitarray[daishu + 1] < tempbest)
                        {
                            tempbest = tabufitarray[daishu + 1];
                            tempbestnumber = daishu;
                        }
                        Console.WriteLine("目标函数为："+tempbest);
                        if ((daishu - tempbestnumber) > NIiteration)
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < tabuiteration; i++)
                    {
                        if ((bestfitness > tabufitarray[i]) && tabufitarray[i] != 0)
                        {
                            bestfitness = tabufitarray[i];
                            bestnumber = i;
                        }
                    }
                    Console.WriteLine("禁忌搜索后的最优目标函数值为：" + bestfitness);
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_task[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                    for (int i = 0; i < tasknumber; i++)
                    {
                        Console.Write(tabuSearch_truck[bestnumber, i] + "  ");
                    }
                    Console.WriteLine();
                }
                #endregion
                for (int i = 0; i < populationsize; i++)
                {
                    if ((GA.popsizepath).Count != 0)
                    {
                        (GA.popsizepath).RemoveAt(0);
                    }
                    if ((GA.taskstarttime).Count != 0)
                    {
                        (GA.taskstarttime).RemoveAt(0);
                    }
                    if ((GA.taskendtime).Count != 0)
                    {
                        (GA.taskendtime).RemoveAt(0);
                    }
                }
                for (int i = 0; i < tasknumber; i++)
                {
                    GA.pregegeration[count, i] = tabuSearch_task[bestnumber, i];
                    GA.truckpregegeration[count, i] = tabuSearch_truck[bestnumber, i];
                }
                GA.calfitness(count);
                //int starttimecount = 0;
                //int endtimecount = 0;
                for (int i = 0; i < tasknumber; i++)
                {
                    if (GA.taskstarttime[count][i] < GA.estime[i])
                    {
                        starttimecount += 1;
                    }
                    if (GA.taskendtime[count][i] > GA.letime[i])
                    {
                        endtimecount += 1;
                    }
                }
                Console.WriteLine(starttimecount + "   " + endtimecount);
                Console.WriteLine("第4个任务的结束时间：" + GA.taskendtime[count][3] + "第12个任务的开始时间：" + GA.taskstarttime[count][11]);
                Console.WriteLine("第6个任务的结束时间：" + GA.taskendtime[count][5] + "第15个任务的开始时间：" + GA.taskstarttime[count][14]);
                for (int j = 0; j < trucklength; j++)
                {
                    Console.WriteLine("第{0}个平板车的执行任务的路径是：", j);
                    for (int i = 0; i < (GA.popsizepath[count][j]).Count; i++)
                    {
                        Console.Write(GA.popsizepath[count][j][i] + " ");
                    }
                    Console.WriteLine();
                }
                for (int i = 0; i < tasknumber; i++)
                {
                    Console.WriteLine("第{0}个任务的开始执行时间：{1}，完成时间：{2}。", i + 1, GA.taskstarttime[count][i], GA.taskendtime[count][i]);
                }
                sw.Stop();
                Console.WriteLine("求解运行时间为：" + sw.Elapsed);
                Console.ReadKey();
            }
        }
    }
