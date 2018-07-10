using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownSelectionToAction : MonoBehaviour {

    public Dropdown myDropdown;

    void Start()
    {
        myDropdown = GetComponent<Dropdown>();
        myDropdown.onValueChanged.AddListener(delegate
        {
            applyDropdownSelection(myDropdown);
        });
        applyDropdownSelection(myDropdown);
    }
    public void applyDropdownSelection(Dropdown target)
    {
        string selectedOption = target.captionText.text;
        Debug.Log("Selected: " + target.value + " " + target.captionText.text);
        string associatedElementAction = GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().action;
        //Using names instead of indices because the index will change if element is taken out of the dropdown when it's an invalid option
        switch (selectedOption)
        {
            case "Change to page":
                associatedElementAction = "CHANGE";
                break;
            case "Show element":
                associatedElementAction = "SHOW";
                break;
            case "Hide element":
                associatedElementAction = "HIDE";
                break;
            default:
                Debug.Log("Dropdown selection: " + selectedOption + "Is not recognized. Was name changed?");
                break;
        }

    }
}
