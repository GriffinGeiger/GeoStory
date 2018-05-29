using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageElement : MonoBehaviour {

    private int index;
    private Page nextPage { get; set; } //set if the gui element has an action that changes the page

    public PageElement()
    {

    }

    public void setIndex(int newIndex)
    {
        if (index >= 0)
            index = newIndex;
        else
            throw new ArgumentException();
    }

    public int getIndex()
    {
        return index;
    }

    public String toString()
    {
        return "at index " + index;
    }
}
