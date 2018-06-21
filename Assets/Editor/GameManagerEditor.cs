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
    }
 
}

