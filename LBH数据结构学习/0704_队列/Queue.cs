using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0704_队列
{
    public interface IqueueDS<T>
    {
        int GetLength();
        bool IsEmpty();
        bool IsFull();
        void In(T item);
        T Out();
        T GetFront();
    }
    class Queue<T>:IqueueDS<T>
    {
        private int intMaxSize;  
        private T[] tData;  
        private int intFrontPointer;//队头指针  
        private int intRearPointer;//队尾指针  
        public int MaxSize  
        {  
            get { return this.intMaxSize; }  
            set { this.intMaxSize = value; }  
        }  
        public T this[int i]// 只读  
        {  
            get { return this.tData[i]; }  
        }  
        public int FrontPointer  
        {  
            get { return this.intFrontPointer; }  
            set { this.intFrontPointer = value; }  
        }  
        public int RearPointer  
        {  
            get { return this.intRearPointer; }  
            set { this.intRearPointer = value; }  
        }  
        public Queue():this(20)//数组默认容量20  
        {  
              
        }  
        public Queue(int size)  
        {  
            this.intMaxSize = size;  
            this.tData = new T[size];  
            this.intRearPointer = -1;  
            this.intFrontPointer = -1;  
        }  
        #region IQueueDs<T> 成员  
        public int GetLength()  
        {  
            return (this.intRearPointer + this.intMaxSize - this.intFrontPointer) % this.intMaxSize;  
        }  
        public bool IsEmpty()//不能仅凭相等判断，还必须都等于-1。当向队列添加第一个要素时，有rear=front=0，显然这种情况不是空队列。  
        {  
            return (this.intFrontPointer == -1 && this.intRearPointer == -1);  
        }  
        public bool IsFull()  
        {  
            return (this.intRearPointer + 1) % this.intMaxSize == this.intFrontPointer;  
        }  
        public void In(T t)  
        {  
            if (this.IsFull())  
            {  
                Console.WriteLine("The Queue is full!");  
                return;  
            }  
            if (this.IsEmpty())//如果是向空队列添加数据，则front和rear都设置为0  
            {  
                this.intFrontPointer = 0;  
                this.intRearPointer = 0;  
                this.tData[this.intRearPointer] = t;  
            }  
            else  
            {  
                if (this.intRearPointer == this.intMaxSize - 1)//判断是否需要循环，如果rear已经达到数组最大索引，则需要重新从0开始  
                {  
                    this.intRearPointer = 0;  
                    this.tData[this.intRearPointer] = t;  
                }  
                else  
                {  
                    this.intRearPointer++;  
                    this.tData[this.intRearPointer] = t;  
                }  
            }  
        }  
        public T Out()  
        {  
            if (this.IsEmpty())  
            {  
                Console.WriteLine("The queue has no elements!");  
                return default(T);  
            }  
            else  
            {  
                T t = this.tData[this.intFrontPointer];  
                if (this.intFrontPointer == this.intMaxSize -1)//判断是否退到最大索引处  
                {  
                    this.intFrontPointer = 0;  
                }  
                else  
                {  
                    this.intFrontPointer++;  
                }  
                if (this.intRearPointer + 1 == this.intFrontPointer)//如果front比rear大1个，表明队列中已经没有数据，此时要其设置为空队列，即  
                {  
                    this.intFrontPointer = -1;  
                    this.intRearPointer = -1;  
                }  
                return t;  
            }  
        }  
        public T GetFront()  
        {  
            if (this.IsEmpty())  
            {  
                Console.WriteLine("The queue has no elements!");  
                return default(T);  
            }  
            return this.tData[this.intRearPointer];  
        }  
        #endregion  
    }
}
