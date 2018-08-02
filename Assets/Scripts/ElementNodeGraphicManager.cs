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
    private void Awake()
    {
        NodeGraphicRect = GetComponentInParent<RectTransform>();
        selectionConnectorPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StoryEditor/SelectionConnector.prefab", typeof(GameObject));
        headerHeight = 110f;
        
    }

    //adds selectionConnectors to the elementNode
    public void addSelectionConnectors(int amount)
    {
        float elementHeight = headerHeight + ((amount + selectionConnectors.Count) * selectionConnectorPrefab.GetComponent<RectTransform>().rect.height);
        RectTransform element_rt = GetComponent<RectTransform>();
        element_rt.sizeDelta = new Vector2(NodeGraphicRect.rect.width, elementHeight);
        for(int i = 0; i < amount; i++)
        {
            GameObject selector = GameObject.Instantiate(selectionConnectorPrefab, this.transform);
            selector.GetComponentInChildren<ManipulateNodeLines>().connectionIndex = selectionConnectors.Count;
            selector.name += " " + selectionConnectors.Count;
            selectionConnectors.Add(selector);
        }
        PageNodeGraphicManager.stackUIElements(selectionConnectors.ToArray(), GetComponent<RectTransform>(), headerHeight);
    }
}
