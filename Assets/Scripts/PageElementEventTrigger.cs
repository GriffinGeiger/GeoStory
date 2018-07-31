using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This class will eventually handle any events that will cause a page change, element appearance/disappearance, or any other action from a pageElement
public class PageElementEventTrigger : EventTrigger, IPointerClickHandler {
    
    public List<Page> connectedPages; //The page the action will change to
    public List<string> connectedPageNames; //this is needed in deserialization 
    public List<GameObject> connectedElements; //The element that the action will switch to or activate (Will be null if the connected node is a Page)
    public List<int> connectedElementIndexes; //this is needed in deserialization. Default value is -1
    public enum Action { None, Change, Show, Hide };
    public List<Action> actions; //the action that will happen


    public new void OnPointerClick(PointerEventData data)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            Action action = actions[i];
            if (action == Action.Change)
            {
                Debug.Log("Changing page to " + connectedPageNames[i] + connectedPages[i]);
                if (connectedPages[i] != null)
                    connectedPages[i].storyRef.changePage(connectedPages[i]); //connectedPages[i] has a reference to the story. Since both pages have to be on same story it doesn't matter if this page or that one supplies ref
                else
                    Debug.Log("PageElementEventTrigger is trying to changePage but no page is connected");
            }
            else if (action == Action.Show)
            {
                Debug.Log("Showing " + connectedElements[i].name + " from index: " + connectedElementIndexes[i] + " from page: " + connectedPages[i].getName());
                connectedElements[i].SetActive(true);
            }
            else if (action == Action.Hide)
            {
                Debug.Log("Hiding " + connectedElements[i].name + " from index: " + connectedElementIndexes[i] + " from page: " + connectedPages[i].getName());
                connectedElements[i].SetActive(false);
            }
        }
    }

    public void AddConnections(Page page,GameObject element, Action action, int dropdownIndex)
    {
        if (page != null)
        {
            connectedPages[dropdownIndex] = page;
            connectedPageNames[dropdownIndex] = page.getName();
        }
        if(element != null)
            connectedElements[dropdownIndex] = element;
        actions[dropdownIndex] = action;
    }

    public void AddConnections(Page page, GameObject element, Action action)
    {
        connectedPages.Add(page);
        connectedPageNames.Add(page.getName());
        connectedElements.Add(element);
        actions.Add(action);
        Debug.Log("These numbers should be the same or theres a problem in PEET: " + connectedPages.Count + connectedPageNames.Count + connectedElements.Count + actions.Count);
    }

}
