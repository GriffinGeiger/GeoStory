using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

//This class will eventually handle any events that will cause a page change, element appearance/disappearance, or any other action from a pageElement
public class PageElementEventTrigger : EventTrigger, IPointerClickHandler {

    public delegate void PerformAction(PerformAction actionEvent);
    //outward events
    public event PerformAction actionEvent;
    /*public event PerformAction changeEvent;
    public event PerformAction showEvent;
    public event PerformAction hideEvent;
    public event PerformAction toggleVisibilityEvent;
    public event PerformAction editEvent;*/

    //received events
    public Dictionary<PerformAction, Action> actions; //Stores the event and the action associated with it

    public enum Action { None, Change, Show, Hide, ToggleVisibility, Edit };
    public Page pageRef;
    public bool buttonsActive;

    public new void OnPointerClick(PointerEventData data)
    {
        if (buttonsActive)
        {
            if (actionEvent != null)
            {
                actionEvent(actionEvent);
            }
            /*  if (changeEvent != null)
                  changeEvent(Action.Change);
              if (showEvent != null)
                  showEvent(Action.Show);
              if (hideEvent != null)
                  hideEvent(Action.Hide);
              if (toggleVisibilityEvent != null)
                  toggleVisibilityEvent(Action.ToggleVisibility);
              if (editEvent != null)
                  editEvent(Action.Edit);*/
            /*
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
                else if(action == Action.ToggleVisibility)
                {
                    connections[i].connectedElement.SetActive(!connections[i].connectedElement.activeInHierarchy);
                }
                else if (action == Action.Edit)
                {
                    pageRef.gameManagerRef.changeMode(GameManager.Mode.EditPage, connections[i].connectedPage);
                }
                
            } */
        }
    }
    // //////////////////////////////////////////EventSystem stuff///////////////////////////////////////////////////
    private void subscribeToEvent(PerformAction actionEvent)
    {
        actionEvent += executeAction;
    }
    private void unsubscribeToEvent(PerformAction actionEvent)
    {
        actionEvent -= executeAction;
    }
    public void addConnection(PageElementEventTrigger originPeet, Action action) //adds event and action to the dictionary
    {
        actions.Add(originPeet.actionEvent, action);
    }
    public void removeConnection(PageElementEventTrigger originPeet)
    {
        actions.Remove(originPeet.actionEvent);
    }
    void OnEnable()
    {
        foreach(PerformAction actionEvent in actions.Keys)
        {
            subscribeToEvent(actionEvent);
        }
    }
    void OnDisable()
    {
        foreach (PerformAction actionEvent in actions.Keys)
        {
            if(actions[actionEvent] != Action.Hide || actions[actionEvent] != Action.Show || actions[actionEvent] != Action.ToggleVisibility) //visibility events need to be able to be changed when not active
                unsubscribeToEvent(actionEvent);
        }
    }
    //execute the action. Target is this element. The event is needed to get the action out of the dictionary
    public void executeAction(PerformAction actionEvent)
    {
        switch (actions[actionEvent])
        {
            case Action.None:
                break;
            case Action.Change:
                pageRef.storyRef.changePage(pageRef);
                break;
            case Action.Show:
                break;
            case Action.Hide:
                break;
            case Action.ToggleVisibility:
                break;
            case Action.Edit:
                pageRef.gameManagerRef.changeMode(GameManager.Mode.EditPage, pageRef);
                break;
            default:
                break;
        }
    }


   // //////////////////////////////////////////////////////////////////////////////////////////////

}

