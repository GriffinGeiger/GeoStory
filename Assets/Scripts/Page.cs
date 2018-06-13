using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable()]
public class Page {

    
    private List<GameObject> elements = new List<GameObject>();
    private List<Button> buttons = new List<Button>();
    private Story storyRef;
    public bool isVisible;
    private string name;


    public Page() { }
    public Page(string name,Story story)
    {
        this.name = name;
        storyRef = story;
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
        element.SetActive(false);
        elements.Add(element);
    }



    //Assumes only one button per page element. May need to change this
    public void addPageElement(GameObject element, string action)
    {
        EventTrigger trigger = element.GetComponent<EventTrigger>(); //Implement eventTrigger to do button stuff.
        if (trigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick; //this defines the type of clicking this button reacts to. Subject to change
            entry.callback.AddListener((data) => { buttonActions(action); }) ;
            trigger.triggers.Add(entry); //at this point entry reacts to pointer click and calls buttonActions
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
            storyRef.setCurrentPage(nextPage);
        }
    }

    public int getNumberOfPageElements()
    {
        return elements.Count;
    }

    public GameObject[] getElements()
    {
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
