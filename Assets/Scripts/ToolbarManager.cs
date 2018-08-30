using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarManager : MonoBehaviour {

    Transform parent;
    private int siblingCount;

	void Start () {
        parent = GetComponentsInParent<RectTransform>()[1];
        transform.SetAsLastSibling();
        siblingCount = parent.childCount;
        Debug.Log("Parent: " + parent.gameObject);
	}

    void Update()
    {
        if(parent.childCount != siblingCount) //if amount of siblings changed
        {
            transform.SetAsLastSibling();
            siblingCount = parent.childCount;
        }
    }
}
