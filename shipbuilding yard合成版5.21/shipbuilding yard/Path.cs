using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;

namespace shipbuilding_yard
{
    public class Path
    {

        public void getConn(Vertex v_start, Vertex v_end, string yid, DataTable cellTable, DataTable occupiedTable,string cell_ID1,string cell_ID2,string b_po)//(起点，终点，场地，堆位信息，堆场占用信息，分段所在堆位编号，分段放置位置)
        {  // int[] distance;        
           // int row;
           // ArrayList way = new ArrayList();


            //string connstr="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\chenkai\\Desktop\\smart shipbuilding\\program\\shipbuilding yard\\input database.accdb";        
            //OleDbConnection conn = new OleDbConnection(connstr);
            //conn.Open();

                      

            //OleDbDataAdapter cell = new OleDbDataAdapter("select * from 堆位信息 where [场地ID]='"+yid+"'", conn);

            //OleDbDataAdapter occupied = new OleDbDataAdapter("select * from 当前全局堆场占用信息", conn);

            //DataTable tempTable = new DataTable();
            //tempTable.Clear();
            //cell.Fill(tempTable);
           
            //读取分段所在堆场的堆位信息
            DataRow[] dr1 = cellTable.Select("[场地ID]='" + yid + "'");
            DataTable tempTable = cellTable.Clone();
            tempTable.Clear();
            for (int c1 = 0; c1 < dr1.Length; c1++)
            {
                tempTable.ImportRow(dr1[c1]);
            }



            DataView dv = new DataView(tempTable);
            dv.Sort = "堆位Name asc";
            tempTable = dv.ToTable();
                     
            //DataTable occupiedTable = new DataTable();
            //occupiedTable.Clear();
            //occupied.Fill(occupiedTable);



           

/*
            List<Vertex> vts = new List<Vertex>();
           
           
            for(int i = 0; i < tempTable.Rows.Count; i++)
            {
                Vertex vt = new Vertex(tempTable.Rows[i][0].ToString());
                for(int j = 0; j < occupiedTable.Rows.Count; j++)
                {    List<Block> occupiedby = new List<Block>();
                     if(tempTable.Rows[i][0].ToString() == occupiedTable.Rows[j][1].ToString())
                     {  
                        Block bk = new Block(occupiedTable.Rows[j][0].ToString(),occupiedTable.Rows[j][1].ToString());
                        occupiedby.Add(bk);
                        
                        Console.Write(tempTable.Rows[i][0]+"  "+ occupiedby.Count.ToString() + occupiedby.Count);
                      }
                     vts.Add(vt);
                 }
                 Console.WriteLine("");
            }
           
            for(int i = 0; i < tempTable.Rows.Count; i++)
            {
                for(int j = 0; j < tempTable.Columns.Count; j++)
                {
                     Console.Write(tempTable.Rows[i][j]+"  ");

                }
                Console.WriteLine("");

            }
*/
            //初始化距离矩阵
            int[,] dismatrix = new int[tempTable.Rows.Count,tempTable.Rows.Count];//固定长度的矩阵数组,+1表示路所在节点;
            for(int i=0; i<tempTable.Rows.Count; i++){           
                  for(int j=0; j<tempTable.Rows.Count; j++){
                     if(i == j){
                        dismatrix[i,j] = 0;
                     }
                     else{dismatrix[i,j] = 90000;}  
                  }
            }

           
		    for(int i=0; i<tempTable.Rows.Count; i++){
                 Vertex vt = new Vertex(tempTable.Rows[i][0].ToString());
                 for(int j=0; j<tempTable.Rows.Count; j++){
                      
                     if(tempTable.Rows[i]["东堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString()){ //东侧堆位距离
                           Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());
                           vt.east = vt1;
                           int c1=0,c2=0;
                           List<Block> occupiedby1 = new List<Block>();
                           List<Block> occupiedby2 = new List<Block>();
                           for(int k=0; k<occupiedTable.Rows.Count; k++){
                               if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby1.Add(bk);
                                  c1 = occupiedby1.Count;
                               }
                               if (tempTable.Rows[i]["东堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby2.Add(bk);
                                  c2 = occupiedby2.Count;
                               }
                         
                           }


                           if(c1==0 && c2 ==0){dismatrix[i,j] = 1;dismatrix[j,i] = 1;}
                           if(c1==0 && c2 ==1){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==0 && c2 ==2){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==0){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==1 && c2 ==1){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==2){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;}
                           if(c1==2 && c2 ==0){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==2 && c2 ==1){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;} 
                           if(c1==2 && c2 ==2){dismatrix[i,j] = 2001;dismatrix[j,i] =2001;}


                           if (c1 == 2 && cell_ID2==null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID1)
                           {
                               if (b_po == "3")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }
                                                     
                           }
                           if (c1 == 1 && cell_ID1 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID2)
                           {
                               if (b_po == "3")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }
                           


                         
                      }
                      
                      if(tempTable.Rows[i]["南堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString()){//南侧堆位距离
                          Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());
                           vt.south = vt1;
                           int c1=0,c2=0;
                           List<Block> occupiedby1 = new List<Block>();
                           List<Block> occupiedby2 = new List<Block>();
                           for(int k=0; k<occupiedTable.Rows.Count; k++){
                               if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby1.Add(bk);
                                  c1 = occupiedby1.Count;
                               }
                               if (tempTable.Rows[i]["南堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby2.Add(bk);
                                  c2 = occupiedby2.Count;
                               }
                         
                           }
                           if(c1==0 && c2 ==0){dismatrix[i,j] = 1;dismatrix[j,i] = 1;}
                           if(c1==0 && c2 ==1){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==0 && c2 ==2){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==0){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==1 && c2 ==1){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==2){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;}
                           if(c1==2 && c2 ==0){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==2 && c2 ==1){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;} 
                           if(c1==2 && c2 ==2){dismatrix[i,j] = 2001;dismatrix[j,i] =2001;}

                           if (c1 == 2 && cell_ID2 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID1)
                           {
                               if (b_po == "1")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }
                           if (c1 == 1 && cell_ID1 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID2)
                           {
                               if (b_po == "1")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }


                      }
                      if (tempTable.Rows[i]["西堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString())
                      {//西侧堆位距离
                          Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());
                           vt.west = vt1;
                           int c1=0,c2=0;
                           List<Block> occupiedby1 = new List<Block>();
                           List<Block> occupiedby2 = new List<Block>();
                           for(int k=0; k<occupiedTable.Rows.Count; k++){
                               if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby1.Add(bk);
                                  c1 = occupiedby1.Count;
                               }
                               if (tempTable.Rows[i]["西堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby2.Add(bk);
                                  c2 = occupiedby2.Count;
                               }
                         
                           }
                           if(c1==0 && c2 ==0){dismatrix[i,j] = 1;dismatrix[j,i] = 1;}
                           if(c1==0 && c2 ==1){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==0 && c2 ==2){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==0){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==1 && c2 ==1){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==2){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;}
                           if(c1==2 && c2 ==0){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==2 && c2 ==1){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;} 
                           if(c1==2 && c2 ==2){dismatrix[i,j] = 2001;dismatrix[j,i] =2001;}

                           if (c1 == 2 && cell_ID2 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID1)
                           {
                               if (b_po == "4")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }
                           if (c1 == 1 && cell_ID1 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID2)
                           {
                               if (b_po == "4")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }
                         
                      }

                      if (tempTable.Rows[i]["北堆位ID"].ToString() == tempTable.Rows[j]["堆位ID"].ToString())
                      {//北侧堆位距离
                          Vertex vt1 = new Vertex(tempTable.Rows[j]["堆位ID"].ToString());
                           vt.north = vt1;
                           int c1=0,c2=0;
                           List<Block> occupiedby1 = new List<Block>();
                           List<Block> occupiedby2 = new List<Block>();
                           for(int k=0; k<occupiedTable.Rows.Count; k++){
                               if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby1.Add(bk);
                                  c1 = occupiedby1.Count;
                               }
                               if (tempTable.Rows[i]["北堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                               {

                                   Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                  occupiedby2.Add(bk);
                                  c2 = occupiedby2.Count;
                               }
                         
                           }
                           if(c1==0 && c2 ==0){dismatrix[i,j] = 1;dismatrix[j,i] = 1;}
                           if(c1==0 && c2 ==1){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==0 && c2 ==2){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==0){dismatrix[i,j] = 501;dismatrix[j,i] =501;}
                           if(c1==1 && c2 ==1){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==1 && c2 ==2){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;}
                           if(c1==2 && c2 ==0){dismatrix[i,j] = 1001;dismatrix[j,i] =1001;}
                           if(c1==2 && c2 ==1){dismatrix[i,j] = 1501;dismatrix[j,i] =1501;} 
                           if(c1==2 && c2 ==2){dismatrix[i,j] = 2001;dismatrix[j,i] =2001;}

                           if (c1 == 2 && cell_ID2 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID1)
                           {
                               if (b_po == "2")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }
                           if (c1 == 1 && cell_ID1 == null && tempTable.Rows[i]["堆位ID"].ToString() == cell_ID2)
                           {
                               if (b_po == "2")
                               {
                                   dismatrix[i, j] = dismatrix[i, j] + 500;
                                   dismatrix[j, i] = dismatrix[j, i] + 500;
                               }
                               else
                               {
                                   dismatrix[i, j] = dismatrix[i, j] - 500;
                                   dismatrix[j, i] = dismatrix[j, i] - 500;
                               }

                           }
                      }

                 }
   
                                

            }
            //string exePath = System.Environment.CurrentDirectory;//本程序所在路径
            //string exePath = System.Windows.Forms.Application.StartupPath;

            string exePath = System.Windows.Forms.Application.ExecutablePath;
            //string exePath = exePath1
            int index;
            for (int i = 0; i < 4; i++)
            {
                index = exePath.LastIndexOf("\\");
                exePath = exePath.Substring(0, index);
            }
           // exePath = exePath + "\\";

            string filepath =   exePath + "\\"+yid; 

          
            //System.IO.StreamWriter swOut = new System.IO.StreamWriter(@"C:\\Users\\chenkai\\Desktop\\smart shipbuilding\\program\\shipbuilding yard\\"+yid+"距离.txt", false, System.Text.Encoding.Default);
            System.IO.StreamWriter swOut = new System.IO.StreamWriter(@"" + filepath + "距离.txt", false, System.Text.Encoding.Default);
            
            for(int i=0; i<tempTable.Rows.Count; i++){           
                  for(int j=0; j<tempTable.Rows.Count; j++){
                      //Console.Write(dismatrix[i,j]+"    ");
                      swOut.Write(dismatrix[i,j]+"    ");
                     
                  }
                  //Console.WriteLine();
                  swOut.WriteLine();
                      
            }
            swOut.Flush();
            swOut.Close();

          
            ArrayList ways = new ArrayList();
            for (int i = 0; i < tempTable.Rows.Count; i++)  //有row个点，则从中心到各点的路有row-1条
            {
                ArrayList w = new ArrayList();
                int j = 0;
                w.Add(j);
                ways.Add(w);
            }
       
            ArrayList S = new ArrayList();
            ArrayList Sr = new ArrayList();
            int []Indexof_distance=new int[tempTable.Rows.Count];
            for(int i=0; i < tempTable.Rows.Count; i++)
            { 
                Indexof_distance[i]=i;
            }

            S.Add( Indexof_distance[0] );                     
            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                Sr.Add( Indexof_distance[i] );
            }
            Sr.RemoveAt(0);
            int[] D = new int[tempTable.Rows.Count];    //存放中心点到每个点的距离
           
                 //---------------以上已经初始化了，S和Sr(里边放的都是点的编号)------------------
            int Count = tempTable.Rows.Count - 1;
            while (Count>0)
            {
                        //假定中心点的编号是0的贪吃法求路径
                 for (int i = 0; i < tempTable.Rows.Count; i++)  
                 D[i] = dismatrix[0,i];
                 int min_num = (int)Sr[0];  //距中心点的最小距离点编号

                 foreach (int s in Sr)
                 {
                      if (D[s] < D[min_num]) min_num = s;
                 }
        
                        //以上可以排序优化
                 S.Add(min_num);
                 Sr.Remove(min_num);
                        //-----------把最新包含进来的点也加到路径中-------------
                 ((ArrayList)ways[min_num]).Add(min_num);
                        //-----------------------------------------------
                 foreach (int element in Sr)
                 {
                      int position = element;
                      bool exchange = false;      //有交换标志

                      if (D[element] < D[min_num] + dismatrix[min_num,position])
                      D[element] = D[element];
                      else
                      {
                            D[element] =  dismatrix[min_num,position] + D[min_num];
                            exchange = true;
                      }
                            //修改距离矩阵                   
                      dismatrix[0,element] = D[element];
                      position = element;      
                      dismatrix[0,position] = D[element];

                            //修改路径---------------
                      if (exchange == true)
                      {                       
                           ((ArrayList)ways[element]).Clear();
                           foreach (int point in (ArrayList)ways[min_num])
                           ((ArrayList)ways[element]).Add(point);
                      }
                  }
                  --Count;
              }
                                             //------中心到各点的最短路径----------

              //System.IO.StreamWriter paOut = new System.IO.StreamWriter(@"C:\\Users\\chenkai\\Desktop\\smart shipbuilding\\program\\shipbuilding yard\\"+yid+"路径.txt", false, System.Text.Encoding.Default);
            System.IO.StreamWriter paOut = new System.IO.StreamWriter(@"" + filepath + "路径.txt", false, System.Text.Encoding.Default);
              
             
              foreach(ArrayList mother in ways)
              {    for(int i = 0; i < tempTable.Rows.Count; i++)
                  {
                      if (tempTable.Rows[i]["堆位ID"].ToString() == v_end.id)
                        { 
                            //int na= (int)tempTable.Rows[i][1]; 
                            if (mother[mother.Count - 1].ToString() == tempTable.Rows[i]["堆位Name"].ToString())
                           {   
                               foreach (int child in mother)
                               {    string vp;
                                    for(int j = 0; j < tempTable.Rows.Count; j++){
                                        if (child == (int)tempTable.Rows[j]["堆位Name"])
                                        {
                                            vp = tempTable.Rows[j]["堆位ID"].ToString();
                                            //Console.Write("{0}-", vp);
                                            paOut.Write("{0}-", vp);
                                         }
                            
                                     }
                                //Console.WriteLine("  路径长 {0}",(float)dismatrix[0,sum_d_index++]/1000);
                       
                        
                                }
                          
                                List<Block> occupiedby = new List<Block>();
                                for(int k=0; k<occupiedTable.Rows.Count; k++){
                                    if (tempTable.Rows[i]["堆位ID"].ToString() == occupiedTable.Rows[k]["堆位ID"].ToString())
                                    {
                                        Block bk = new Block(occupiedTable.Rows[k]["分段ID"].ToString(), occupiedTable.Rows[k]["堆位ID"].ToString());
                                         occupiedby.Add(bk);
                                         
                                     }
                                    
                                     
                                }
                                int c = occupiedby.Count;
                                if(c.Equals(0)) paOut.WriteLine("   mobility:{0}",(float)dismatrix[0,(int)mother[mother.Count-1]]/1000);
                                if (c.Equals(1) && cell_ID2 == null) paOut.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] - 500) / 1000);
                                if(c.Equals(2)) paOut.WriteLine("   mobility:{0}",(float)(dismatrix[0,(int)mother[mother.Count-1]]-500)/1000);
                                if (c.Equals(1) && cell_ID1 == null) paOut.WriteLine("   mobility:{0}", (float)(dismatrix[0, (int)mother[mother.Count - 1]] + 500) / 1000);//目标堆位是放一个小分段的堆位



                                 //paOut.WriteLine("  路径长 {0}",(float)dismatrix[0,(int)mother[mother.Count-1]]/1000);
                                 paOut.Flush();
                                 paOut.Close();
                            }
                        }

                   }
               
             }

            
            //Console.WriteLine(tempTable.Rows.Count);
            //Console.ReadLine();
            //conn.Close(); 

            
        }

    }
}


              

