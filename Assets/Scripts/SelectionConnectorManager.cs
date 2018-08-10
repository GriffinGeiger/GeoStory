using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionConnectorManager : MonoBehaviour {

    public int connectionKey; //Each page element has multiple connections, this is the index of which connection in the connections list this selection connector represents
    private Dropdown dropdown;
    private PageElementEventTrigger peet; //The peet this element is associated with

	void Start ()
    {
        peet = GetComponentInParent<ElementNodeGraphicManager>().associatedElement.GetComponent<PageElementEventTrigger>();
        connectionKey = GetComponentInChildren<ManipulateNodeLines>().connectionKey;
        dropdown = GetComponentInChildren<Dropdown>();

        dropdown.onValueChanged.AddListener(delegate
        {
            //Check to see if the change is still valid to the connector it is connected to (Can call hide and show both on an element but not on a page)
            //If valid, change action and color of curve, otherwise break the link
            try
            {
                connectionKey = GetComponentInChildren<ManipulateNodeLines>().connectionKey;
                PageElementEventTrigger.Action selection = getDropdownSelection();
                if (selection == PageElementEventTrigger.Action.Change)
                {

                    BezierCurve4PointRenderer curve = GetComponentInChildren<ManipulateNodeLines>().curve;
                    if (peet.connections[connectionKey].connectedElement != null) //if the connected receiver is from an Element then it cannot be page changed to
                    {
                        Debug.Log("Breaking link because trying to Change to Element");
                        curve.breakLink();
                    }
                    else
                    {
                        Debug.Log("Changing old selection to Change");
                        peet.connections[connectionKey].action = selection;
                        curve.setAction(selection);
                    }
                }
                else
                {
                    BezierCurve4PointRenderer curve = GetComponentInChildren<ManipulateNodeLines>().curve;
                    if (peet.connections[connectionKey].connectedElement == null) //if the connected receiver is from an page then it cannot have an element function applied
                    {
                        Debug.Log("Breaking link because trying to " + selection + " to Page");
                        curve.breakLink();
                    }
                    else
                    {
                        Debug.Log("Changing old selection to " + selection);
                        peet.connections[connectionKey].action = selection;
                        curve.setAction(selection);
                    }
                }
            }
            catch (KeyNotFoundException) { } //If key wasn't found then there wasn't a connection already made
            catch (NullReferenceException) { } //if curve wasn't found then there wasnt a connection already made
        });
    }

    public PageElementEventTrigger.Action getDropdownSelection()
    {
        string selectedOption = dropdown.captionText.text;
        PageElementEventTrigger.Action associatedElementAction;
        switch (selectedOption)
        {
            case "Change to page":
                associatedElementAction = PageElementEventTrigger.Action.Change;
                break;
            case "Show element":
                associatedElementAction = PageElementEventTrigger.Action.Show;
                break;
            case "Hide element":
                associatedElementAction = PageElementEventTrigger.Action.Hide;
                break;
            default:
                associatedElementAction = PageElementEventTrigger.Action.None;
                break;
        }
        return associatedElementAction;
    }

}
