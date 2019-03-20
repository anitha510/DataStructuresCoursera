using System;

namespace MyLibrary
{
    public class PriorityQueue<T>
    {
        T[] data;
        int MaxSize;

        public PriorityQueue(int capacity)
        {
            MaxSize = capacity;
            data = new T[MaxSize];
        }

        public void Insert(T item)
        {

        }
    }
}
