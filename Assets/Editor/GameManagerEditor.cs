using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager gm = (GameManager) target;
        if (GUILayout.Button("Toggle Current page visibility"))
        {
            Debug.Log("toggling current page visibility. CurrentPage.isVisible = " + gm.currentStory.getCurrentPage().isVisible);
            if (gm.currentStory.getCurrentPage().isVisible == false)
                gm.currentStory.getCurrentPage().setVisible(true);
            else
                gm.currentStory.getCurrentPage().setVisible(false);
        }
        if(GUILayout.Button("Name of CurrentPage"))
        {
            Debug.Log("The currentPage is : " + gm.currentStory.getCurrentPage().getName());
        }
        if(GUILayout.Button("# of page elements in current page"))
        { 
           Debug.Log(gm.currentStory.getCurrentPage().getNumberOfPageElements());
        }
        if(GUILayout.Button("Save Story to XML"))
        {
            Debug.Log("Testing XML");
            XMLSerializationManager.saveStory(gm.currentStory);
        }
        if(GUILayout.Button("Load IntroStory from XML"))
        {
            gm.currentStory = XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml",gm.canvas);
            gm.currentStory.setCurrentPage("introPage1");
        }
        if (GUILayout.Button("BuildIntro"))
        {
            Debug.Log("Building intro story");
            gm.buildIntro();
        }
        if(GUILayout.Button("Debug serialization"))
        {
            gm.buildIntro();
            StoryData input = XMLSerializationManager.saveStory(gm.currentStory);
            Story output = XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml",gm.canvas);
           // Debug.Log("input: " + checkout(input) + " ouput: " + checkout(output));
        }
        if(GUILayout.Button("Print corners"))
        {
            Vector3[] corners = new Vector3[4];
            gm.testTransform.GetWorldCorners(corners);
            foreach(Vector3 corner in corners)
            {
                Debug.Log("Corner: " + corner);
            }
            Debug.Log("Anchor Max: " + gm.testTransform.anchorMax + " Anchor Min: " + gm.testTransform.anchorMin);
            
            gm.testTransform.offsetMin = new Vector2(0, 0);
            gm.testTransform.offsetMax = new Vector2(0, 0);
            Debug.Log("OffsetMin: " + gm.testTransform.offsetMin);
        }
        if(GUILayout.Button("Build PageNodeGraphic from test page"))
        {
            gm.buildIntro();
            gm.currentStory.setCurrentPage("introPage2");
            gm.currentStory.currentPage.setVisible(false);
            foreach(Page page in gm.currentStory.getPages())
            {
                GameObject go = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/NodeGraphic.prefab"), gm.scrollContent);
                PageNodeGraphicManager pngm = go.GetComponent<PageNodeGraphicManager>();
                pngm.addBodyPanels(page);
            }
            

        }
    }
 
}

