using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ElementNodeGraphicManager : MonoBehaviour {

    public List<GameObject> selectionConnectors;
    public GameObject selectionConnectorPrefab;
    public RectTransform NodeGraphicRect;   //Will need to edit the sizeDelta when a dropdown is added/removed
    public float headerHeight; //height of the thumbnail and name 
    public float footerHeight; //height of spacing under last selection connector and the add connector button
    public GameObject associatedElement;
    public InputField elementNameInputField;
    public int elementIndex; //needs to change whenever nodeParts changes

    private void Awake()
    {
        NodeGraphicRect = GetComponentInParent<RectTransform>();
        selectionConnectorPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StoryEditor/SelectionConnector.prefab", typeof(GameObject));
        headerHeight = 110f;
        footerHeight = 0f;
        elementNameInputField = GetComponentInChildren<InputField>();
        elementNameInputField.onEndEdit.AddListener(delegate {
            if (elementNameInputField.text.Trim().Length == 0)
            {
                Debug.Log("Name not entered, not reassigning");
                elementNameInputField.text = associatedElement.name;
                return;
            }
            this.name = elementNameInputField.text;
            associatedElement.name = elementNameInputField.text;
        });
    }

    //adds selectionConnectors to the elementNode
    public void addSelectionConnectors(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject selector = GameObject.Instantiate(selectionConnectorPrefab, this.transform);
            selector.name += " " + selectionConnectors.Count;
            selectionConnectors.Add(selector);
        }
        drawSelectionConnectors();
    }

    public void drawSelectionConnectors()
    {
        float elementHeight = headerHeight + footerHeight +  selectionConnectors.Count * selectionConnectorPrefab.GetComponent<RectTransform>().rect.height;
        GetComponent<RectTransform>().sizeDelta = new Vector2(NodeGraphicRect.rect.width, elementHeight);
        PageNodeGraphicManager.stackUIElements(selectionConnectors.ToArray(), GetComponent<RectTransform>(), headerHeight);
    }
}
