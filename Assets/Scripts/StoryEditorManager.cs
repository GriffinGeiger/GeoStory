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
        //Remove any previous editor graphics
        pageGraphics.Clear();
        foreach(BezierCurve4PointRenderer curve in FindObjectsOfType<BezierCurve4PointRenderer>())
        {
            GameObject.Destroy(curve.gameObject);
        }
        foreach (Page page in gm.currentStory.getPages())
        {
            addPageGraphic(page);
        }

        foreach(GameObject pageGraphic in pageGraphics)
        {
            pageGraphic.GetComponent<PageNodeGraphicManager>().drawConnectionCurves();
        }
    }

    public void addPageGraphic(Page page)
    {
        GameObject go = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/NodeGraphic_Mk2.prefab"), gm.scrollContent);
        PageNodeGraphicManager pngm = go.GetComponent<PageNodeGraphicManager>();
        pngm.buildFromPage(page);
        pageGraphics.Add(pngm.gameObject);
    }
}
