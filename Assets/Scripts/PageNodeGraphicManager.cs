using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class PageNodeGraphicManager : MonoBehaviour {

    public Page page; //the page this graphic is associated with
    public GameObject elementNodePrefab;
    public Vector2 lowestAnchorPoint;
    public float pixelHeightOfTitle = 76f; //the offset between the top of the node graphic and the bottom of the title card
    public float widthOfGraphic;
    public float pixelHeightOfFooter;
    public List<GameObject> nodeParts;

    public void Awake()
    {
        elementNodePrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StoryEditor/ElementNode.prefab", typeof(GameObject));
        pixelHeightOfTitle = 76f;
        pixelHeightOfFooter = 40f;
        widthOfGraphic = 275f;
    }
    public void buildFromPage(Page content)
    {
        page = content;
        nodeParts = new List<GameObject>();
        float heightOfRect = pixelHeightOfTitle + pixelHeightOfFooter; //Starts at height of title since that will always be the minimum height of title


        foreach (GameObject element in content.getElements())
        {
            GameObject body = GameObject.Instantiate(elementNodePrefab, this.transform);
            body.GetComponentsInChildren<Text>()[0].text = element.name;
            try
            {
                body.GetComponent<Image>().sprite = element.GetComponent<Image>().sprite;
            }
            catch (Exception) { }//in case this element doesn't have a sprite
            body.GetComponent<AssociatedElementReference>().associatedElement = element;
            body.GetComponent<ElementNodeGraphicManager>().addDropdowns(element.GetComponent<PageElementEventTrigger>().connectedPages.Count);
            float heightOfElement = body.GetComponent<RectTransform>().rect.height;
            heightOfRect += heightOfElement; //Make height of rect bigger to accommodate for each new element
            nodeParts.Add(body);
            body.GetComponentsInChildren<Text>()[1].text = element.name;

            //add dropdowns and set them to reflect their actions
            int numberOfDropdowns = element.GetComponent<PageElementEventTrigger>().connectedPages.Count;
            ElementNodeGraphicManager engm = body.GetComponent<ElementNodeGraphicManager>();
            engm.addDropdowns(numberOfDropdowns);
            for (int i = 0; i < numberOfDropdowns; i++) 
            {
                PageElementEventTrigger.Action action = element.GetComponent<PageElementEventTrigger>().actions[i];
                Dropdown dropdown = engm.dropdowns[i];
                if (dropdown != null)
                {
                    if (action == PageElementEventTrigger.Action.Change)
                        dropdown.captionText.text = "Change to page";
                    else if (action == PageElementEventTrigger.Action.Show)
                        dropdown.captionText.text = "Show element";
                    else if (action == PageElementEventTrigger.Action.Hide)
                        dropdown.captionText.text = "Hide element";
                }
            }
        }

        //adjust rectTransform of NodeGraphic
        RectTransform graphicRectTransform = GetComponent<RectTransform>();
        graphicRectTransform.sizeDelta = new Vector2(widthOfGraphic, heightOfRect);
        graphicRectTransform.anchoredPosition = page.nodeGraphicLocation;

        float percentageHeightOfTitle = pixelHeightOfTitle / heightOfRect;
        lowestAnchorPoint = new Vector2(1, 1 - percentageHeightOfTitle);
        Debug.Log("First lowestAnchorPoint " + lowestAnchorPoint);

        foreach (GameObject go in nodeParts)
        {
            //get the percentage of the rect that this part will take up and set that as the deltaAnchor height
            RectTransform elementNode_rt = go.GetComponent<RectTransform>();
            Debug.Log("Height of rect" + heightOfRect);
            float fractionOfRect = elementNode_rt.rect.height / heightOfRect;
            elementNode_rt.anchorMax = new Vector2(1, 1);
            Debug.Log("fraction of rect " +  fractionOfRect);
            elementNode_rt.anchorMin = new Vector2(0,1 - fractionOfRect);
            Debug.Log("Max Min: " + elementNode_rt.anchorMax + elementNode_rt.anchorMin);
            //moveAnchors so that this part is connected to the bottom anchor of the last nodePart 
            lowestAnchorPoint = moveAnchors(elementNode_rt, lowestAnchorPoint);
        }
    }
    //Moves the anchors while preserving size of rectangle to the new max point. Returns new LowestAnchorPoint
    public static Vector2 moveAnchors(RectTransform transform ,Vector2 newMax)
    {
        Vector2 deltaAnchor = transform.anchorMax - transform.anchorMin;
        transform.anchorMax = newMax;
        transform.anchorMin = transform.anchorMax - deltaAnchor;
        transform.offsetMax = Vector2.zero;
        transform.offsetMin = Vector2.zero;
        return transform.anchorMin + new Vector2(1,0);
    }

    
    //Takes the info currently applied in the graphic and adds it to the Page this graphic is associated with
    public void assignChanges()
    {
        foreach(GameObject go in nodeParts)
        {
            PrefabInfo.PrefabType prefabType = go.GetComponent<PrefabInfo>().prefabType;
            if(prefabType == PrefabInfo.PrefabType.NodeHeader)
            {

            }
            else if(prefabType == PrefabInfo.PrefabType.NodeBodyImage || prefabType == PrefabInfo.PrefabType.NodeBody || prefabType == PrefabInfo.PrefabType.NodeFooter) //if this is a Node with an associated element
            {
                GameObject associatedElement = go.GetComponent<AssociatedElementReference>().associatedElement;
                PrefabInfo.PrefabType associatedElementPrefabType = associatedElement.GetComponent<PrefabInfo>().prefabType;
                if(associatedElementPrefabType == PrefabInfo.PrefabType.BackgroundImage)
                {
                    //Get info from the nodeBodyImage and fill out associated element
                }
                else if (associatedElementPrefabType == PrefabInfo.PrefabType.ScrollArea)
                {

                }
                else if(associatedElementPrefabType == PrefabInfo.PrefabType.Button)
                {

                }
            }
        }
    }
}
