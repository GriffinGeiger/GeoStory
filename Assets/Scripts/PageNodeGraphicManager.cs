using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageNodeGraphicManager : MonoBehaviour {

    
    public GameObject headerPrefab;
    public GameObject actionBodyPrefab; //Prefab to use if this node will have an action associated with it and no picture
    public GameObject thumbnailActionBodyPrefab; //Prefab to use if this node has an action and a picture associated with it
    public GameObject footerPrefab;
    public Vector2 lowestAnchorPoint = new Vector2(1,1); //the lowest anchor point from previous node part that the next anchor point will latch onto

    public void addBodyPanels(Page content)
    {
        GameObject header = GameObject.Instantiate(headerPrefab, this.transform);
        header.GetComponentInChildren<Text>().text = content.getName();

        foreach (GameObject element in content.getElements())
        {
            string prefabType = element.GetComponent<PrefabInfo>().prefabType;
            GameObject body;
            if (prefabType == "BackgroundImage")
            {
                body = GameObject.Instantiate(thumbnailActionBodyPrefab, this.transform);
                body.GetComponentInChildren<Image>().sprite = element.GetComponent<Image>().sprite;
                Debug.Log("If wrong image shows up then the problem is in PageNodeGraphicManager");
            }
            else if (prefabType == "ScrollArea" || prefabType == "Button")
            {
                body = GameObject.Instantiate(actionBodyPrefab, this.transform);
            }
            else
            {
                throw new System.ArgumentException("Prefab type does not match known prefabs");
            }
            RectTransform transform = body.GetComponent<RectTransform>();
            moveAnchors(transform, lowestAnchorPoint);

            body.GetComponentsInChildren<Text>()[1].text = element.name;
            Debug.Log("If whenClicked changes instead, change the index to 0. Name will need to be determined.");
            //Determine the current buttonAction and change the dropdown to reflect that
        }
        GameObject footer = GameObject.Instantiate(footerPrefab, this.transform);
        moveAnchors(footer.GetComponent<RectTransform>(), lowestAnchorPoint);
    }
    private void moveAnchors(RectTransform transform ,Vector2 newMax)
    {
        Vector2 deltaAnchors = transform.anchorMax - transform.anchorMin;
        transform.anchorMax = lowestAnchorPoint;
        transform.anchorMin = transform.anchorMax + deltaAnchors;
        transform.offsetMax = Vector2.zero;
        transform.offsetMin = Vector2.zero;
    }
}
