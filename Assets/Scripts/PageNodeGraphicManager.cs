using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PageNodeGraphicManager : MonoBehaviour {

    
    public GameObject headerPrefab;
    public GameObject actionBodyPrefab; //Prefab to use if this node will have an action associated with it and no picture
    public GameObject thumbnailActionBodyPrefab; //Prefab to use if this node has an action and a picture associated with it
    public GameObject footerPrefab;
    public Vector2 lowestAnchorPoint = new Vector2(1,1); //the lowest anchor point from previous node part that the next anchor point will latch onto

    public void Awake()
    {
        headerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/StoryEditor/NodeHeader.prefab");
        actionBodyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/StoryEditor/NodeBody.prefab");
        thumbnailActionBodyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/StoryEditor/NodeBodyImage.prefab");
        footerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/StoryEditor/NodeFooter.prefab");
    }
    public void addBodyPanels(Page content)
    {
        List<GameObject> nodeParts = new List<GameObject>();
        GameObject header = GameObject.Instantiate(headerPrefab, this.transform);
        header.GetComponentInChildren<Text>().text = content.getName();
        nodeParts.Add(header);
        float heightOfRect = header.GetComponent<RectTransform>().rect.height; //This will set the height of the rect that contains the body panels. Every height of every panel part will add to this
        float widthOfRect = header.GetComponent<RectTransform>().rect.width;
        
        foreach (GameObject element in content.getElements())
        {
             string prefabType = element.GetComponent<PrefabInfo>().prefabType;
             GameObject body;
             if (prefabType == "BackgroundImage")
             {
                 body = GameObject.Instantiate(thumbnailActionBodyPrefab, this.transform);
                 body.GetComponentInChildren<RawImage>().texture = element.GetComponent<RawImage>().texture;
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
             heightOfRect += transform.rect.height;
             nodeParts.Add(body);
             //moveAnchors(transform, lowestAnchorPoint);

             body.GetComponentsInChildren<Text>()[1].text = element.name;
             Debug.Log("If whenClicked changes instead, change the index to 0. Name will need to be determined.");
             //Determine the current buttonAction and change the dropdown to reflect that
         }
         
        GameObject footer = GameObject.Instantiate(footerPrefab, this.transform);
        heightOfRect += footer.GetComponent<RectTransform>().rect.height;
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(widthOfRect, heightOfRect);
        nodeParts.Add(footer);
        //moveAnchors(footer.GetComponent<RectTransform>(), lowestAnchorPoint);

        foreach(GameObject go in nodeParts)
        {
            //get the percentage of the rect that this part will take up and set that as the deltaAnchor height
            RectTransform rt = go.GetComponent<RectTransform>();
            float heightOfPart = rt.rect.height;
            float fractionOfRect = (heightOfPart / heightOfRect);
            rt.anchorMax = new Vector2(1, 1);
            rt.anchorMin = new Vector2(0, 1 - fractionOfRect);
            //moveAnchors so that this part is connected to the bottom anchor of the last nodePart 
            moveAnchors(rt, lowestAnchorPoint);
        }
    }
    private void moveAnchors(RectTransform transform ,Vector2 newMax)
    {
        Debug.Log("Before moveAnchor: Max:" + transform.anchorMax + " Min: " + transform.anchorMin + "LAP: " + lowestAnchorPoint);
        Vector2 deltaAnchor = transform.anchorMax - transform.anchorMin;
        transform.anchorMax = newMax;
        transform.anchorMin = transform.anchorMax - deltaAnchor;
        transform.offsetMax = Vector2.zero;
        transform.offsetMin = Vector2.zero;
        lowestAnchorPoint = transform.anchorMin + new Vector2(1,0);
        Debug.Log("After moveAnchor: Max:" + transform.anchorMax + " Min: " + transform.anchorMin + "LAP: " + lowestAnchorPoint);
    }
}
