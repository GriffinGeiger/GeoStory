using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour {

    private List<PageElement> elements = new List<PageElement>();

	
    public Page()
    {
        //create default page
        //create options bar
        //create background
        
    }	
	
    public void isVisible(bool tf)
    {
        if(tf == true)
        {
            //loop through elements and make all elements visible
            foreach(PageElement element in elements)
            {
                element.enabled = true;
            }
        }
        else
        {
            //loop through elements and make all invisible
            foreach (PageElement element in elements)
            {
                element.enabled = false;
            }
        }
    }

    public void addPageElement(PageElement element)
    {
        
        element.setIndex(elements.Count);
        elements.Add(element);
    }

    public void removePageElement(PageElement element)      //Test this later
    {
        elements.Remove(element);
    }

    public int getNumberOfPageElements()
    {
        return elements.Count;
    }
}
