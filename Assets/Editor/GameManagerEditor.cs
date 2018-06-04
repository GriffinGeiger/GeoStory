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
            if (gm.currentPage.isVisible == false)
                gm.currentPage.setVisible(true);
            else
                gm.currentPage.setVisible(false);
        }
        if(GUILayout.Button("Name of CurrentPage"))
        {
            Debug.Log("The currentPage is : " + gm.currentPage.name);
        }
        if(GUILayout.Button("# of page elements in current page"))
        { 
           Debug.Log(gm.currentPage.getNumberOfPageElements());
        }

    }

}
