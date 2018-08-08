using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoryEditorManager : MonoBehaviour {

    public GameManager gm;
    public List<GameObject> pageGraphics = new List<GameObject>();
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void buildStoryEditorGraphics(Story story)
    {
        foreach (Page page in gm.currentStory.getPages())
        {
            GameObject go = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/NodeGraphic_Mk2.prefab"), gm.scrollContent);
            PageNodeGraphicManager pngm = go.GetComponent<PageNodeGraphicManager>();
            pngm.buildFromPage(page);
            pageGraphics.Add(pngm.gameObject);
        }

        foreach(GameObject pageGraphic in pageGraphics)
        {
            pageGraphic.GetComponent<PageNodeGraphicManager>().drawConnectionCurves();
        }
    }
}
