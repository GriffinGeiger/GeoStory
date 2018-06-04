using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour {

    private List<GameObject> elements = new List<GameObject>();
    public bool isVisible;
	
    public Page()
    {
        //create default page
        //create options bar
        //create background
        
    }	
	
    public void setVisible(bool tf)
    {
        if(tf == true)
        {
            //loop through elements and make all elements visible
            foreach(GameObject element in elements)
            {
                element.SetActive(true);
            }
            isVisible = true;
        }
        else
        {
            //loop through elements and make all invisible
            foreach (GameObject element in elements)
            {
                element.SetActive(false);
            }
            isVisible = false;
        }
    }

    public void addPageElement(GameObject element)
    {
        elements.Add(element);
    }

    public void removePageElement(GameObject element)      //Test this later
    {
        elements.Remove(element);
    }

    public int getNumberOfPageElements()
    {
        return elements.Count;
    }
}
