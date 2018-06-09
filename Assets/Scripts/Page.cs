using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable()]
public class Page {

    
    private List<GameObject> elements = new List<GameObject>();
    private List<Button> buttons = new List<Button>();
    public bool isVisible;
    private string name;


    public Page() { }
    public Page(string name)
    {
        this.name = name;
        //create default page
        //create options bar
        //create background
        
    }	
	
    public void setVisible(bool tf)
    {
        if (tf == true)
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

    //Assumes only one button per page element. May need to change this
    public void addPageElement(GameObject element, string action)
    {
        Button btn = element.GetComponentInChildren<Button>();
       // EventTrigger trigger = element.GetComponent<EventTrigger>(); //Implement eventTrigger to do button stuff.
        if (btn != null)
        {
            btn.onClick.AddListener(delegate { buttonActions(action); });
            buttons.Add(btn);
            elements.Add(element);
        }
        else
        {
            throw new MissingComponentException("Action specified but no Button associated with element.");
        }
    }

    public void removePageElement(GameObject element)      //Test this later
    {
        elements.Remove(element);
    }

    public void buttonActions(string action)
    {
        Debug.Log("In buttonActions");
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

    public GameObject[] getElements()
    {
        Debug.Log("Count "+ elements.Count);
        GameObject[] gameObjects = new GameObject[elements.Count];
        elements.CopyTo(gameObjects);
        return gameObjects;
    }

    public string getName()
    {
        return name;
    }

    public void setName(string newName)
    {
        //if( name doesn't exist within story)
        name = newName;
        //remove then readd to story so name updates in dictionary
        //else throw exception telling user to change the name

    }
}
