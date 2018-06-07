using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page {

    private List<GameObject> elements = new List<GameObject>();
    private List<Button> buttons = new List<Button>();
    public bool isVisible;
    private string name;

    

    public Page(string name)
    {
        this.name = name;
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

    public void addPageElement(GameObject element, string action)
    {
        Button btn = element.GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(delegate { buttonActions(action); });
            buttons.Add(btn);
        }
        else
            //possibly throw an exception saying no button on element but action specified
            ;
    }

    public void removePageElement(GameObject element)      //Test this later
    {
        elements.Remove(element);
    }

    public void buttonActions(string action)
    {
        if(action.StartsWith(ButtonActionConstants.CHANGE_PAGE("")))
        {
            string nextPage = action.Substring(6).Trim();
            Debug.Log("Button requests:" + nextPage + ": as next page");
        }
    }

    public int getNumberOfPageElements()
    {
        return elements.Count;
    }

    public string getName()
    {
        return name;
    }

    public void setName(string newName)
    {
        //if( name doesn't exist within story)
        name = newName;
        //else throw exception telling user to change the name
    }
}
