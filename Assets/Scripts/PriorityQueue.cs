using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private readonly List<Tuple<T, int>> heap = new List<Tuple<T, int>>();

    public void Enqueue(T item, int priority)
    {
        heap.Add(Tuple.Create(item, priority));
        int currentIndex = heap.Count - 1;
        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;
            if (heap[parentIndex].Item2 <= heap[currentIndex].Item2)
                break;

            Swap(parentIndex, currentIndex);
            currentIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        T result = heap[0].Item1;
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = 2 * currentIndex + 1;
            int rightChildIndex = 2 * currentIndex + 2;
            int minIndex = currentIndex;

            if (leftChildIndex < lastIndex && heap[leftChildIndex].Item2 < heap[minIndex].Item2)
                minIndex = leftChildIndex;
            if (rightChildIndex < lastIndex && heap[rightChildIndex].Item2 < heap[minIndex].Item2)
                minIndex = rightChildIndex;

            if (minIndex == currentIndex)
                break;

            Swap(currentIndex, minIndex);
            currentIndex = minIndex;
        }

        return result;
    }

    public int Count => heap.Count;

    private void Swap(int indexA, int indexB)
    {
        Tuple<T, int> temp = heap[indexA];
        heap[indexA] = heap[indexB];
        heap[indexB] = temp;
    }
}
