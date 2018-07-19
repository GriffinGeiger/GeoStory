using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this is to be applied to ElementNode so it can get all dropdowns that are children to it
public class DropdownSelectionToAction : MonoBehaviour {
    [HideInInspector]
    public List<Dropdown> myDropdowns;

    void Start()
    {
        Dropdown[] dd = GetComponentsInChildren<Dropdown>();
        for (int i = 0; i < dd.Length; i++)
        {
            myDropdowns.Add(dd[i]);
            dd[i].GetComponentInParent<AssociatedElementReference>().GetComponentInChildren<ManipulateNodeLines>().dropdownIndex = i;
            //Will need to make sure that the correct dropdown is at the same index as its action in peet.actions
            myDropdowns[i].onValueChanged.AddListener(delegate
            {
                GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().actions[i] = getDropdownSelection(i);
            });
        }
      //  GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().action = getDropdownSelection();
    }
    public PageElementEventTrigger.Action getDropdownSelection(int index)
    {
        string selectedOption = myDropdowns[index].captionText.text;
        Debug.Log("Selected: " + myDropdowns[index].value + " " + myDropdowns[index].captionText.text);
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
