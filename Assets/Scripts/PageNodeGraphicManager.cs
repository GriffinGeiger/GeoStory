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
    public InputField titleInputField;
    private RectTransform contentWindow;

    public void Awake()
    {
        elementNodePrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StoryEditor/ElementNode.prefab", typeof(GameObject));
        titleHeight = 76f;
        footerHeight = 40f;
        graphicWidth = 275f;
        contentWindow = GetComponentInParent<PinchZoom>().GetComponent<RectTransform>();
        titleInputField = GetComponentInChildren<InputField>();
        titleInputField.onEndEdit.AddListener(delegate {
            string oldName = page.getName();
            string name = titleInputField.text;
            if (name.Trim().Length == 0)
            {
                Debug.Log("Name not entered, not reassigning");
                titleInputField.text = page.getName();
                return;
            }
            this.name = "NodeGraphic_" + name;
            if (page.name != name)   //if the page name is the same as it was previously, setting it again would treat it as a copy
                page.setName(name);
            else return; //page name didn't change so nothing needs to be done
            //all connections related to old name need to be changed to new name
            foreach(PageElementEventTrigger peet in Resources.FindObjectsOfTypeAll<PageElementEventTrigger>())
            {
                foreach(ConnectionInfo connection in peet.connections.Values)
                {
                    if (connection.connectedPageName.Equals(oldName))
                    {
                        connection.connectedPageName = name;
                    }
                }
            }

        });
    }
    public void buildFromPage(Page content)
    {
        page = content;
        nodeParts = new List<GameObject>();
       // float heightOfRect = titleHeight + footerHeight; //Starts at height of title since that will always be the minimum height of title
        foreach (GameObject element in content.getElements())
        {
            GameObject body = GameObject.Instantiate(elementNodePrefab, this.transform);
            body.GetComponentInChildren<InputField>().text = element.name;
            body.name = "ElementNode_" + element.name;
            try
            {
                body.GetComponentInChildren<Image>().sprite = element.GetComponent<Image>().sprite;
            }
            catch (Exception) { }//in case this element doesn't have a sprite
            body.GetComponent<ElementNodeGraphicManager>().associatedElement = element;


            //add dropdowns and set them to reflect their actions
            int numberOfDropdowns = element.GetComponent<PageElementEventTrigger>().connections.Count;
            ElementNodeGraphicManager engm = body.GetComponent<ElementNodeGraphicManager>();
            engm.addSelectionConnectors(numberOfDropdowns);
            for (int i = 0; i < numberOfDropdowns; i++)
            {
                ConnectionInfo connection = element.GetComponent<PageElementEventTrigger>().connections[i];
                PageElementEventTrigger.Action action =connection.action;
                Dropdown dropdown = engm.selectionConnectors[i].GetComponentInChildren<Dropdown>();
                if (dropdown != null)
                {
                    if (action == PageElementEventTrigger.Action.Change)
                        dropdown.value = 0;
                    else if (action == PageElementEventTrigger.Action.Show)
                    {
                        dropdown.value = 1;
                    }
                    else if (action == PageElementEventTrigger.Action.Hide)
                        dropdown.value = 2;
                }
                else Debug.Log("No dropdown found in selection connector");

                //set the connection key in ManipulateNodeLines for this selection connector
                dropdown.GetComponentInParent<SelectionConnectorManager>().GetComponentInChildren<ManipulateNodeLines>()
                    .connectionKey = connection.connectionKey;
            }
            nodeParts.Add(body);
        }
        drawElementNodes();

        GetComponentInChildren<InputField>().text = page.getName(); //set title of node graphic to page name
        name = "NodeGraphic_" + page.getName();
        this.GetComponent<RectTransform>().anchoredPosition = content.nodeGraphicLocation;
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
    public void drawConnectionCurves()
    {
        StoryEditorManager sem = FindObjectOfType<StoryEditorManager>();
        foreach (GameObject element in nodeParts) //for every element in this pages nodeparts
        {
            ElementNodeGraphicManager engm = element.GetComponent<ElementNodeGraphicManager>();
            int connectionKey = 0; //this will increment with every processed selection connector so that it will apply each connection to a selection connector
            foreach (GameObject selectionConnector in engm.selectionConnectors) //for every selection connector in this element
            {
                //get the connection
                ConnectionInfo connection = engm.associatedElement.GetComponent<PageElementEventTrigger>().connections[connectionKey++]; 
                BezierCurve4PointRenderer curve = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/CurveRenderer.prefab")
                    , contentWindow).GetComponent<BezierCurve4PointRenderer>();

                selectionConnector.GetComponentInChildren<ManipulateNodeLines>().curve = curve;
                curve.originConnector = selectionConnector.GetComponentInChildren<ManipulateNodeLines>().gameObject;

                foreach(GameObject graphic in sem.pageGraphics)
                {
                    PageNodeGraphicManager pngm = graphic.GetComponent<PageNodeGraphicManager>();
                    if(connection.connectedPage.Equals(pngm.page))  //if the connected page matches this page
                    {
                        
                        if (connection.connectedElement != null) //if its connected to an element node and not a page node
                        {
                            foreach (GameObject otherElement in pngm.nodeParts) //check all the elements in the origin page
                            {
                                if (connection.connectedElement.Equals(otherElement.GetComponent<ElementNodeGraphicManager>().associatedElement)) //when one matches thats the origin connector
                                {
                                    ReceiveNodeLines rnl = otherElement.GetComponentInChildren<ReceiveNodeLines>();
                                    curve.receivingConnector = rnl.gameObject;
                                    rnl.curves.Add(curve);
                                }
                            }
                        }
                        else
                        {
                            //The first ReceiveNodeLines should be the PageNodeConnector receiver since it his highest in hierarchy
                            ReceiveNodeLines rnl = pngm.GetComponentInChildren<ReceiveNodeLines>();
                            curve.receivingConnector = rnl.gameObject;
                            rnl.curves.Add(curve);

                        }
                    }
                }
                curve.setAction(connection.action);
                curve.snapEndpointsToConnectors();
            }
        }
    }

    public void drawElementNodes()
    {
        float heightOfRect = titleHeight + footerHeight;

        foreach(GameObject part in nodeParts)
        {
            heightOfRect += part.GetComponent<RectTransform>().rect.height; //Make height of rect bigger to accommodate for each new element

        }
        RectTransform nodeGraphic_rt = GetComponent<RectTransform>();
        nodeGraphic_rt.sizeDelta = new Vector2(graphicWidth, heightOfRect);
        //draw the elements on the NodeGraphic
        stackUIElements(nodeParts.ToArray(), nodeGraphic_rt, titleHeight);
        //update the location of the lines to reconnect with the now shifted nodes
        foreach (GameObject part in nodeParts)
        {
            foreach (ManipulateNodeLines mnl in part.GetComponentsInChildren<ManipulateNodeLines>())
            {
                if (mnl.curve != null)
                    mnl.curve.snapEndpointsToConnectors();
            }
        }
    }
}
