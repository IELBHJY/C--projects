using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace shipbuilding_yard
{
    public class Block
    {
        public string id; //编号
	    //public int t;   //任务时间
        public float sb;  //分段面积
	    public string pb;   //分段起始位置
        public string ps;   //工艺阶段
        public string ft;  //搭载节点
        public string size;
    
        public string dt; //离场时间dipatureTime
        public string position;

	    //public Map<Integer, Integer> positions;//每次调度完存放的位置，仅保存最佳
	    //public Map<Integer, List<Integer>> routes;//存储在每次调度中移动的路径，仅保存最佳
	 
	
	    public Block(string id,string pb){//若ear=-1, 则lat 表示0时刻已入场分段的位置
		    this.id = id;
		    this.pb = pb;
		 }
        public Block(string id, string pb, string dt, string position)//带三个变量,定为阻挡分段。
        {
            this.id = id;
            this.pb = pb;
            this.dt = dt;
            this.position = position;
        }
       
    }
}
