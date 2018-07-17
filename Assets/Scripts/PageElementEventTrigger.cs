using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This class will eventually handle any events that will cause a page change, element appearance/disappearance, or any other action from a pageElement
public class PageElementEventTrigger : EventTrigger, IPointerClickHandler {
    
    public Page connectedPage; //The page the action will change to
    public string connectedPageName; //this is needed in deserialization 
    public GameObject connectedElement; //The element that the action will switch to or activate (Will be null if the connected node is a Page)
    public int connectedElementIndex = -1; //this is needed in deserialization. Default value is -1
    public enum Action { None, Change, Show, Hide };
    public Action action; //the action that will happen


    public new void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Action" + action);
        if(action == Action.Change)
        {
            Debug.Log("Changing page to " + connectedPageName + connectedPage);
            if (connectedPage != null)
                connectedPage.storyRef.changePage(connectedPage); //connectedPage has a reference to the story. Since both pages have to be on same story it doesn't matter if this page or that one supplies ref
            else
                Debug.Log("PageElementEventTrigger is trying to changePage but no page is connected");
        }
        else if(action == Action.Show)
        {
            connectedElement.SetActive(true);
        }
        else if(action == Action.Hide)
        {
            connectedElement.SetActive(false);
        }
    }
}
