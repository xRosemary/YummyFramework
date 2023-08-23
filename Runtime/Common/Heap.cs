using System;
using System.Linq;
using System.Collections.Generic;

namespace YummyFrameWork
{
    public class Heap<T>
    {
        private List<T> heap;
        private readonly IComparer<T> comparer;

        public int Count => heap.Count;

        public Heap() : this(null) { }

        public Heap(IComparer<T> comparer)
        {
            heap = new List<T>();
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public void Insert(T value)
        {
            heap.Add(value);
            HeapifyUp(heap.Count - 1);
        }

        public T Top()
        {
            if (heap.Count == 0)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            return heap[0];
        }

        public void Pop()
        {
            if (heap.Count == 0)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
        }

        private void HeapifyUp(int index)
        {
            int parentIndex = (index - 1) / 2;

            while (index > 0 && comparer.Compare(heap[index], heap[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                index = parentIndex;
                parentIndex = (index - 1) / 2;
            }
        }

        private void HeapifyDown(int index)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int topIndex = index;

            if (leftChildIndex < heap.Count && comparer.Compare(heap[leftChildIndex], heap[topIndex]) < 0)
                topIndex = leftChildIndex;

            if (rightChildIndex < heap.Count && comparer.Compare(heap[rightChildIndex], heap[topIndex]) < 0)
                topIndex = rightChildIndex;

            if (topIndex != index)
            {
                Swap(index, topIndex);
                HeapifyDown(topIndex);
            }
        }

        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}
