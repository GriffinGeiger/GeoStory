using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This class will eventually handle any events that will cause a page change, element appearance/disappearance, or any other action from a pageElement
public class PageElementEventTrigger : EventTrigger, IPointerClickHandler {

    /*  public List<Page> connectedPages; //The page the action will change to
      public List<string> connectedPageNames; //this is needed in deserialization 
      public List<GameObject> connectedElements; //The element that the action will switch to or activate (Will be null if the connected node is a Page)
      public List<int> connectedElementIndexes; //this is needed in deserialization. Default value is -1

      public List<Action> actions; //the action that will happen
      */
    public enum Action { None, Change, Show, Hide };
    public List<ConnectionInfo> connections = new List<ConnectionInfo>();

    public new void OnPointerClick(PointerEventData data)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            Action action = connections[i].action;
            if (action == Action.Change)
            {
                Debug.Log("Changing page to " + connections[i].connectedPageName + connections[i].connectedPage);
                if (connections[i].connectedPage != null)
                    connections[i].connectedPage.storyRef.changePage(connections[i].connectedPage); //connectedPages[i] has a reference to the story. Since both pages have to be on same story it doesn't matter if this page or that one supplies ref
                else
                    Debug.Log("PageElementEventTrigger is trying to changePage but no page is connected");
            }
            else if (action == Action.Show)
            {
                Debug.Log("Showing " + connections[i].connectedElement.name + " from index: " + connections[i].connectedElementIndex + " from page: " + connections[i].connectedPage.getName());
                connections[i].connectedElement.SetActive(true);
            }
            else if (action == Action.Hide)
            {
                Debug.Log("Hiding " + connections[i].connectedElement.name + " from index: " + connections[i].connectedElementIndex + " from page: " + connections[i].connectedPage.getName());
                connections[i].connectedElement.SetActive(false);
            }
        }
    }

    public void AddConnections(Page page,GameObject element, Action action, int dropdownIndex)
    {
        //If it needs to add placeholder spaces then this will do it, although I think it will be unneccessary after redesign
        while (dropdownIndex >= connections.Count)
        {
            connections.Add(null);
        }
        connections[dropdownIndex] = new ConnectionInfo(page, element, action);
    }

    public void AddConnections(Page page, GameObject element, Action action)
    {
        connections.Add(new ConnectionInfo(page, element, action));
    }

}

public class ConnectionInfo
{
    public Page connectedPage;  //The page the action will change to
    public string connectedPageName;    //this is needed in deserialization 
    public GameObject connectedElement; //The element that the action will switch to or activate (Will be null if the connected node is a Page)
    public string connectedElementName; //this is needed in deserialization. Default value is -1
    public int connectedElementIndex;
    public PageElementEventTrigger.Action action;

    public ConnectionInfo(Page page, GameObject element, PageElementEventTrigger.Action action)
    {
        connectedPage = page;
        if(page != null)
            connectedPageName = page.getName();
        connectedElement = element;
        if(element != null)
            connectedElementName = element.name;
        this.action = action;
    }
}
