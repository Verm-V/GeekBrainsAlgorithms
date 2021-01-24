using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_04_02
{
    class Program
    {
        static void Main(string[] args)
        {
            BTree tree = new BTree();
            tree.AddNode(30);
            tree.AddNode(20);
            tree.AddNode(40);
            tree.AddNode(10);
            tree.AddNode(50);
            tree.AddNode(15);
            tree.AddNode(35);
            tree.AddNode(25);

            tree.AddNode(53);
            tree.AddNode(12);
            tree.AddNode(33);
            tree.AddNode(21);



            Console.WriteLine("nodes: " + tree.GetCount());
            Console.WriteLine("height: " + tree.GetHeight());
            Console.WriteLine("tree: \n");
            tree.Print();

            Console.ReadLine();
            
        }
    }
}
