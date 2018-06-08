using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

public class XMLSerializationManager : MonoBehaviour {
    public static XMLSerializationManager ins;

    private void Awake()
    {
        ins = this;
    }

   

    public static void saveStory(Story story)
    {
        StoryData storyToSerialize = new StoryData(story);

    }
}

[System.Serializable]
public class StoryData
{
    public List<PageData> pages = new List<PageData>();
    public StoryData(Story story)
    {
        Page[] pageArray = story.getPages();
        foreach(Page p in pageArray)
        {
            pages.Add(new PageData(p));
        }
    }
}

[System.Serializable]
public class PageData
{
    public List<GameObjectData> god = new List<GameObjectData>();
    public PageData(Page page)
    {
        //get all the game objects and construct game object data for each and put into list
        GameObject[] elements = page.getElements();
        foreach(GameObject go in elements)
        {
            god.Add(new GameObjectData(go));
        }
    }

}

[System.Serializable]
public class GameObjectData
{
    public List<ComponentData> cd = new List<ComponentData>();
    public List<GameObjectData> god = new List<GameObjectData>();
    public GameObjectData(GameObject go)
    {
        foreach(Transform t in go.transform)
        {
            god.Add(new GameObjectData(t.gameObject));
        }
        foreach (Component c in go.GetComponents<Component>())
        {
            if(c as RectTransform != null)
            cd.Add(new RectTransformData((RectTransform) c));
            if (c as Button != null)
                cd.Add(new ButtonData(c));
            if (c as Image != null)                                  //Side thought: I might need to serialize/deserialize mask component. If erratic behavior later do that
                cd.Add(new ImageData(c));
            if (c as RawImage != null)
                cd.Add(new RawImageData(c));
            if (c as ScrollRect != null)
                cd.Add(new ScrollRectData(c));
            if (c as Scrollbar != null)
                cd.Add(new ScrollBarData(c));
            if (c as Text != null)
                cd.Add(new TextData(c));

        }
    }
}
[System.Serializable]
public class ComponentData
{
    public ComponentData()
    {

    }
}
[System.Serializable]
public class RectTransformData : ComponentData
{
    public Vector2 anchoredPosition;
    public Vector2 anchorMax;
    public Vector2 anchorMin;
    public Vector2 offsetMax;
    public Vector2 offsetMin;
    public Vector2 pivot;
    public Rect rect;
    public Vector2 sizeDelta;

    public RectTransformData(RectTransform rect)
    {
        Debug.Log("Copying RectTransformData");
        anchoredPosition = rect.anchoredPosition;
        anchorMax = rect.anchorMax;
        anchorMin = rect.anchorMin;
        offsetMax = rect.offsetMax;
        offsetMin = rect.offsetMin;
        pivot = rect.pivot;
        this.rect = rect.rect;
        sizeDelta = rect.sizeDelta;
    }
}
[System.Serializable]
public class ButtonData : ComponentData
{
    public ButtonData(Component component)
    {
        Debug.Log("Copying ButtonData");
    }
}
[System.Serializable]
public class ImageData : ComponentData
{
    public ImageData(Component component)
    {
        Debug.Log("Copying ImageData");
    }
}
[System.Serializable]
public class RawImageData : ComponentData
{
    public RawImageData(Component component)
    {
        Debug.Log("Copying RawImageData");
    }
}
[System.Serializable]
public class ScrollRectData : ComponentData
{
    public ScrollRectData(Component component)
    {
        Debug.Log("Copying ScrollRectData");
    }
}
[System.Serializable]
public class ScrollBarData : ComponentData
{
    public ScrollBarData(Component component)
    {
        Debug.Log("Copying ScrollBarData");
    }
}
[System.Serializable]
public class TextData : ComponentData
{
    public TextData(Component component)
    {
        Debug.Log("Copying TextData");
    }
}