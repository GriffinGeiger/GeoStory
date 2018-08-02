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
    public float titleHeight = 76f; //the offset between the top of the node graphic and the bottom of the title card
    public float graphicWidth;
    public float footerHeight;
    public List<GameObject> nodeParts;

    public void Awake()
    {
        elementNodePrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StoryEditor/ElementNode.prefab", typeof(GameObject));
        titleHeight = 76f;
        footerHeight = 40f;
        graphicWidth = 275f;
    }
    public void buildFromPage(Page content)
    {
        page = content;
        nodeParts = new List<GameObject>();
       // float heightOfRect = titleHeight + footerHeight; //Starts at height of title since that will always be the minimum height of title
        foreach (GameObject element in content.getElements())
        {
            GameObject body = GameObject.Instantiate(elementNodePrefab, this.transform);
            body.GetComponentsInChildren<Text>()[0].text = element.name;
            body.name = "ElementNode_" + element.name;
            try
            {
                body.GetComponent<Image>().sprite = element.GetComponent<Image>().sprite;
            }
            catch (Exception) { }//in case this element doesn't have a sprite
            body.GetComponent<AssociatedElementReference>().associatedElement = element;


            //add dropdowns and set them to reflect their actions
            int numberOfDropdowns = element.GetComponent<PageElementEventTrigger>().connections.Count;
            ElementNodeGraphicManager engm = body.GetComponent<ElementNodeGraphicManager>();
            engm.addSelectionConnectors(numberOfDropdowns);
            for (int i = 0; i < numberOfDropdowns; i++)
            {
                PageElementEventTrigger.Action action = element.GetComponent<PageElementEventTrigger>().connections[i].action;
                Dropdown dropdown = engm.selectionConnectors[i].GetComponentInChildren<Dropdown>();
                if (dropdown != null)
                {
                    if (action == PageElementEventTrigger.Action.Change)
                        dropdown.captionText.text = "Change to page";
                    else if (action == PageElementEventTrigger.Action.Show)
                        dropdown.captionText.text = "Show element";
                    else if (action == PageElementEventTrigger.Action.Hide)
                        dropdown.captionText.text = "Hide element";
                }
                else Debug.Log("No dropdown found in selection connector");
            }

            //heightOfRect += body.GetComponent<RectTransform>().rect.height; //Make height of rect bigger to accommodate for each new element
            nodeParts.Add(body);
            body.GetComponentInChildren<Text>().text = element.name;
        }
        drawElementNodes();

        //adjust rectTransform of NodeGraphic
       /* RectTransform nodeGraphic_rt = GetComponent<RectTransform>();
        nodeGraphic_rt.sizeDelta = new Vector2(graphicWidth, heightOfRect);
        //draw the elements on the NodeGraphic
        stackUIElements(nodeParts.ToArray(), nodeGraphic_rt, titleHeight);*/

        GetComponentInChildren<Text>().text = page.getName(); //set title of node graphic to page name
        name = "NodeGraphic_" + page.getName();
    }

    /* Places the elements on top of one another in the parentRect with the top of the stack at the offsetFromTop
     * The elements' anchors will be set up so that all 4 are in the top corners so that the elements will scale with width of the parent while height is fixed
     * 
     */
    public static void stackUIElements(GameObject[] elements, RectTransform parentRect, float offsetFromTop)
    {
        float heightOfRect = parentRect.rect.height;
        float percentHeightOfNextAnchors = 1 - (offsetFromTop / heightOfRect); //finds the percentage of the rect of where the first element goes
        foreach (GameObject go in elements)
        {
            RectTransform elementRectTransform = go.GetComponent<RectTransform>();
            float percentHeightOfElement = elementRectTransform.rect.height / heightOfRect;
            //Set anchors
            elementRectTransform.anchorMax = new Vector2(.5f, percentHeightOfNextAnchors);
            elementRectTransform.anchorMin = new Vector2(.5f, percentHeightOfNextAnchors);
            percentHeightOfNextAnchors = percentHeightOfNextAnchors - percentHeightOfElement;
            //set offsets from anchors
            elementRectTransform.anchoredPosition = new Vector2(0, 0);
            elementRectTransform.sizeDelta = new Vector2(parentRect.rect.width, elementRectTransform.rect.height);
        }
    }

    public void drawElementNodes()
    {
        float heightOfRect = titleHeight + footerHeight;

        foreach(GameObject part in nodeParts)
        {
            heightOfRect += part.GetComponent<RectTransform>().rect.height; //Make height of rect bigger to accommodate for each new element
            //update the location of the lines to reconnect with the now shifted nodes
            foreach (ManipulateNodeLines mnl in part.GetComponentsInChildren<ManipulateNodeLines>())
            {
                if(mnl.curve != null)
                    mnl.curve.snapEndpointsToConnectors();
            }
        }
        RectTransform nodeGraphic_rt = GetComponent<RectTransform>();
        nodeGraphic_rt.sizeDelta = new Vector2(graphicWidth, heightOfRect);
        //draw the elements on the NodeGraphic
        stackUIElements(nodeParts.ToArray(), nodeGraphic_rt, titleHeight);
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
