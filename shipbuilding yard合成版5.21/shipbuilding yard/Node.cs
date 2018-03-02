using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace shipbuilding_yard
{
    class Node
    {
        public int id;
        public string name;
        public int yardType;
        public string[] priority;
	    public ArrayList relationNodes = new ArrayList();

	   /* public string getName() {
		    return name;
	    }

	    public void setName(string name) {
		    this.name = name;
	    }

	    public ArrayList getRelationNodes() {
		    return relationNodes;
	    }

	    public void setRelationNodes(ArrayList relationNodes) {
		    this.relationNodes = relationNodes;
	    }
*/
     }
}
