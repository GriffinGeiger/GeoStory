using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ElementNodeGraphicManager : MonoBehaviour {

    public List<Dropdown> dropdowns;
    public GameObject selectionConnectorPrefab;
    public RectTransform NodeGraphicRect;   //Will need to edit the sizeDelta when a dropdown is added/removed
    public float headerHeight; //height of the thumbnail and name 
    public float selectionConnectorHeight;
    private void Awake()
    {
        selectionConnectorPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/StoryEditor/SelectionConnector.prefab", typeof(GameObject));
        headerHeight = 110f;
        
    }

    //adds dropdown/nodeconnectors to the elementNode
    public void addDropdowns(int amount)
    {
        float elementHeight = headerHeight + (amount * selectionConnectorHeight);
        
    }
}
