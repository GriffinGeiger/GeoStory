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
        if(GUILayout.Button("Load introStory from XML"))
        {
            XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml");
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
            StoryData output = XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml");
            Debug.Log("input: " + checkout(input) + " ouput: " + checkout(output));
        }
    }
    private string checkout(StoryData sd)
    {
        string outputString = "";
        
            TextData rtd = (TextData) sd.pages[1].god[1].god[0].god[1].cd[1];
            try
            {
                outputString +=  rtd.text + " ";

            }
            catch (Exception e) { }
        
        return outputString;
    }
}

