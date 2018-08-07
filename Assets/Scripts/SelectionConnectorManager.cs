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
        peet = GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>();
        connectionKey = GetComponentInChildren<ManipulateNodeLines>().connectionKey;
        dropdown = GetComponentInChildren<Dropdown>();

        dropdown.onValueChanged.AddListener(delegate
        {
            try
            {
                peet.connections[connectionKey].action = getDropdownSelection();
            }
            catch(Exception) { peet.AddConnection(null, null, getDropdownSelection()); }
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
