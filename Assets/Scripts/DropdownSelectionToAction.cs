using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownSelectionToAction : MonoBehaviour {

    [HideInInspector]
    public Dropdown myDropdown;

    void Start()
    {
        myDropdown = GetComponent<Dropdown>();
        myDropdown.onValueChanged.AddListener(delegate
        {
            GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().action =  getDropdownSelection();
        });
        GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().action = getDropdownSelection();
    }
    public PageElementEventTrigger.Action getDropdownSelection()
    {
        string selectedOption = myDropdown.captionText.text;
        Debug.Log("Selected: " + myDropdown.value + " " + myDropdown.captionText.text);
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
                Debug.Log("Dropdown selection: " + selectedOption + "Is not recognized. Was name changed?");
                associatedElementAction = PageElementEventTrigger.Action.Change;
                break;
        }
        return associatedElementAction;
    }
}
