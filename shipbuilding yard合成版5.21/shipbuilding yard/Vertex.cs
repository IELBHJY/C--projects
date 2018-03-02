using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.IO;

namespace shipbuilding_yard
{
    public class Vertex
    {
          public string id;
          public int name;
          public List<Block> occupiedby;
          public Vertex east,south,west,north;

            public Vertex(string id) {
                this.id = id;
           }
            public Vertex(string id, int name) {
                this.id = id;
                this.name = name;
           }
          

           public Vertex(Vertex vx) {
	            this.id = vx.id;
                this.name = vx.name;
	            this.occupiedby = vx.occupiedby;
                this.east = vx.east;
                this.south = vx.south;
                this.west = vx.west;
                this.north = vx.north;
 
           }
         
    }
}
