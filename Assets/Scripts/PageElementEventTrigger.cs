using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

//This class will eventually handle any events that will cause a page change, element appearance/disappearance, or any other action from a pageElement
public class PageElementEventTrigger : EventTrigger, IPointerClickHandler {

    public enum Action { None, Change, Show, Hide , ToggleVisibility};
    public Dictionary<int, ConnectionInfo> connections = new Dictionary<int, ConnectionInfo>();

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

    //Returns the int key for this connection
    public int AddConnection(Page page, GameObject element, Action action)
    {
        ConnectionInfo connection = new ConnectionInfo(page, element, action);
        int newKey = getNewKey();
        connection.connectionKey = newKey;
        connections.Add(newKey,connection);
        return newKey;
    }
    //adds connection and gives new key
    public int AddConnection(ConnectionInfo connection)
    {
        int newKey = getNewKey();
        connection.connectionKey = newKey;
        connections.Add(newKey, connection);
        return newKey;
    }

    //Returns the lowest value key not used by the connection dictionary
    private int getNewKey()
    {
        int newKey = 0;
        while(connections.ContainsKey(newKey) )
        {
            newKey++;
        }
        return newKey;
    }

}

[System.Serializable]
public class ConnectionInfo
{
    [XmlIgnore]
    public Page connectedPage;  //The page the action will change to
    public string connectedPageName;    //this is needed in deserialization 
    [XmlIgnore]
    public GameObject connectedElement; //The element that the action will switch to or activate (Will be null if the connected node is a Page)
    public int connectedElementIndex;//this is needed in deserialization. Default value is -1
    public PageElementEventTrigger.Action action;
    public int connectionKey;

    public ConnectionInfo() { }

    public ConnectionInfo(Page page, GameObject element, PageElementEventTrigger.Action action)
    {
        connectedPage = page;
        if(page != null)
            connectedPageName = page.getName();
        connectedElement = element;
        this.action = action;
    }
}
