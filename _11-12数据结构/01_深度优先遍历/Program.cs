using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_深度优先遍历
{
    class Program
    {
        static void Main(string[] args)
        {
            AdjacencyList<string> a = new AdjacencyList<string>();
            a.AddVertex("8");
            a.AddVertex("2");
            a.AddVertex("3");
            a.AddVertex("4");
            a.AddVertex("5");
            a.AddVertex("6");
            a.AddVertex("7");
            a.AddVertex("1");
            a.AddVertex("9");
            a.AddVertex("10");
            a.AddVertex("11");
            a.AddVertex("12");
            a.AddVertex("13");
            a.AddVertex("14");
            a.AddVertex("15");
            a.AddVertex("16");
            a.AddEdge("1", "3");
            a.AddEdge("1", "2");
            a.AddEdge("2", "4");
            a.AddEdge("3", "4");
            a.AddEdge("4", "5");
            a.AddEdge("4", "6");
            a.AddEdge("5", "7");
            a.AddEdge("6", "7");
            a.AddEdge("6", "11");
            a.AddEdge("7", "12");
            a.AddEdge("8", "9");
            a.AddEdge("8", "13");
            a.AddEdge("9", "10");
            a.AddEdge("9", "14");
            a.AddEdge("10", "11");
            a.AddEdge("10", "15");
            a.AddEdge("11", "12");
            a.AddEdge("11", "16");
            a.AddEdge("13", "14");
            a.AddEdge("14", "15");
            a.AddEdge("15", "16");
            a.DFSTraverse();
            Console.ReadKey();
        }
    }
}
