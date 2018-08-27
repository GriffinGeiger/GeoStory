using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts;

[Serializable()]
public class Page {

    
    private List<GameObject> elements = new List<GameObject>();
    public Story storyRef;
    public bool isVisible;
    public string name;
    public Vector2 nodeGraphicLocation;
    public GameManager gameManagerRef;
    public Page() { }
    public Page(string name,Story story)
    {
        gameManagerRef = GameObject.FindObjectOfType<GameManager>();
        this.name = name;
        storyRef = story;
        //create default page

        //create options bar
        //create background

    }	
	
    public void buildDefaultPage()
    {
        GameObject bg = GameObject.Instantiate(gameManagerRef.background, gameManagerRef.canvas.transform);        //Make sure you instantiate with canvas as parent or transform values will go off page
        bg.SetActive(false);
        bg.name = "background";
        addPageElement(bg);
    }
    public void setVisible(bool tf)
    {
        if (tf == true)
        {
            //loop through elements and make all elements visible
            foreach(GameObject element in elements)
            {     
                if(element.GetComponent<PrefabInfo>().activeWithPage)
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

    //Assumes only one button per page element. May need to change this
    public void addPageElement(GameObject element)
    {
        EventTrigger trigger = element.GetComponent<PageElementEventTrigger>(); //Implement eventTrigger to do button stuff.
        ((PageElementEventTrigger) trigger).pageRef = this;
        if (trigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick; //this defines the type of clicking this button reacts to. Subject to change
            entry.callback.AddListener((data) => { buttonActions(element); }) ;
            trigger.triggers.Add(entry); //at this point entry reacts to pointer click and calls buttonActions
            elements.Add(element);
        }
        else
        {
            throw new MissingComponentException("Action specified but no EventTrigger associated with element.");
        }
    }

    public void removePageElement(GameObject element)      //Test this later
    {
        ConnectionsLibrary.removeConnectionsTo(storyRef, element);
        elements.Remove(element);
        GameObject.Destroy(element);
    }

    //Triggers when a button is pressed on a Page. 
    //Needs only the element that the event is called from since PageElementEventTrigger stores the action and connectedPage or connectedElement
    public void buttonActions(GameObject element)
    {
        for (int i = 0; i < element.GetComponent<PageElementEventTrigger>().connections.Count; i++)
        {
            PageElementEventTrigger peet = element.GetComponent<PageElementEventTrigger>();
            PageElementEventTrigger.Action action = peet.connections[i].action;
            if (action == PageElementEventTrigger.Action.Change)
            {
                storyRef.changePage(peet.connections[i].connectedPage);
            }
            if (action == PageElementEventTrigger.Action.Show)
            {
                peet.connections[i].connectedElement.GetComponent<PrefabInfo>().activeWithPage = true;
            }
            if (action == PageElementEventTrigger.Action.Hide)
            {
                peet.connections[i].connectedElement.GetComponent<PrefabInfo>().activeWithPage = false;
            }
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
        int i = 1;
        while(storyRef.pageNameExists(newName))
        {
            newName = newName + " (" + i + ")";
            i++;
        }
        name = newName;
    }
}
