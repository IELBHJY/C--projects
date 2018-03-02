using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0703_顺序栈
{
    class Stack<T>
    {
        int maxsize;        //顺序栈的容量
        T [] data;      //数组，用于存储栈中的数据
        int top;            //指示栈顶
        public T this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
        //栈容量属性
        public int Maxsize
        {
            get
            {
                return maxsize;
            }
            set
            {
                maxsize = value;
            }
        }
        //获得栈顶的属性
        public int Top
        {
            get
            {
                return top;
            }
        }
        //使用构造器初始化栈
        public Stack(int size)
        {
            data = new T[size];
            maxsize = size;
            top = -1;
        }
        //求栈的长度（栈中的元素个数）
        public int StackLength()
        {
            return top + 1;
        }
        //清空顺序栈
        public void ClearStack()
        {
            top = -1;
        }
        //判断顺序栈是否为空
        public bool IsEmpty()
        {
            if (top == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //判断顺序栈是否为满
        public bool IsFull()
        {
            if (top == maxsize - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //入栈操作
        public void Push(T item)
        {
            if (IsFull())
            {
                Console.WriteLine("栈已满！");
                return;
            }
            data[++top] = item;
        }
        //出栈操作，并返回出栈的元素
        public T Pop()
        {
            T temp = default(T);
            if (IsEmpty())
            {
                Console.WriteLine("栈为空！");
                return temp;
            }
            temp = data[top];
            top--;
            return temp;
        }
        //获取栈顶数据元素
        public T GetTop()
        {
            if (IsEmpty())
            {
                Console.WriteLine("栈为空！");
                return default(T);
            }
            return data[top];
        }
        //打印顺序栈
        public void print()
        {
            if (IsEmpty())
            {
                Console.WriteLine("empty");
            }
            else
            {
                int count=top;
                while ( count!= -1)
                {
                    Console.WriteLine(data[count--]);
                }
            }
        }
    }
}
