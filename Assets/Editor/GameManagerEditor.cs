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

        GUILayout.Label("CurrentStory: " + gm.currentStory.name);
        GUILayout.Label("CurrentPage: " + gm.currentStory.currentPage.name);
        GUILayout.Label("Story Management");
        if (GUILayout.Button("Toggle Current page visibility"))
        {
            Debug.Log("toggling current page visibility. CurrentPage.isVisible = " + gm.currentStory.getCurrentPage().isVisible);
            if (gm.currentStory.getCurrentPage().isVisible == false)
                gm.currentStory.getCurrentPage().setVisible(true);
            else
                gm.currentStory.getCurrentPage().setVisible(false);
        }
        if (GUILayout.Button("BuildIntro"))
        {
            Debug.Log("Building intro story");
            gm.currentStory = gm.buildIntro();
        }
        if (GUILayout.Button("Make currentPage first page"))
        {
            gm.currentStory.firstPageName = gm.currentStory.currentPage.name;
        }
        if (GUILayout.Button("Pages in story"))
        {
            string output = "Page names in currentstory \n";
            foreach (Page page in gm.currentStory.getPages())
            {
                output += page.getName() + "\n";
            }
            Debug.Log(output);
        }

        GUILayout.Label("XML Management");
        if (GUILayout.Button("Load IntroStory from XML"))
        {
            gm.currentStory = XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml", gm.canvas);
            gm.currentStory.setCurrentPage("introPage1");
        }
        if (GUILayout.Button("Save Story to XML"))
        {
            XMLSerializationManager.saveStory(gm.currentStory);
        }

        GUILayout.Label("StoryEditor Management");
        if(GUILayout.Button("Build PageNodeGraphics from testStory"))
        {
            gm.currentStory = XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml", gm.canvas);
            gm.currentStory.setCurrentPage("introPage1");
            gm.currentStory.currentPage.setVisible(false);
            FindObjectOfType<StoryEditorManager>().buildStoryEditorGraphics(gm.currentStory);           
        }
        if(GUILayout.Button("Add Page"))
        {
            Page page = new Page("New Page" , gm.currentStory);
            page.buildDefaultPage();
            gm.currentStory.addPage(page);
            FindObjectOfType<StoryEditorManager>().addPageGraphic(page);
        }

        if(GUILayout.Button("Edit Current Page"))
        {
            gm.changeMode(GameManager.Mode.EditPage, gm.currentStory.currentPage);
        }

    }
 
}

