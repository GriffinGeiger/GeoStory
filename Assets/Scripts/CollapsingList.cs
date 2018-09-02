using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingList<T> : IEnumerable
{
    private List<T> internalList;

    public T this[int index]
    {
        get
        {
            return internalList[index];
        }

        set
        {
            internalList[index] = value;
        }
    }

    public int Count()
    {
        return internalList.Count;
    }

    public bool IsReadOnly()
    {
        return false;
    }

    public void Add(T item)
    {
        internalList.Add(item);
    }

    public void Clear()
    {
        internalList.Clear();
    }

    public bool Contains(T item)
    {
        return internalList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        internalList.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return internalList.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return internalList.IndexOf(item);
    }

    //Removes the specified item then shifts everything down so there is no gap
    public bool Remove(T item)
    {
        int removedIndex = internalList.IndexOf(item);
        if (removedIndex == -1) //if there was no occurrance of this item
            return false;
        for(int i = removedIndex; i<internalList.Count; i++) //shift everything down one
        {
            internalList[i] = internalList[i + 1];
        }
        internalList.RemoveAt(internalList.Count-1); //remove the last spot
        return true;
    }

    public void RemoveAt(int index)
    {
        internalList.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
