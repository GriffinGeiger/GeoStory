using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        if(GUILayout.Button("Test Xml"))
        {
            Debug.Log("Testing XML");
            XMLSerializationManager.saveStory(gm.currentStory);
        }
        if (GUILayout.Button("BuildIntro"))
        {
            Debug.Log("Building intro story");
            gm.buildIntro();
        }
    }

}
