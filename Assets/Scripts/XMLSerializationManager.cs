using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using System;
using System.Xml.Schema;
using System.Reflection;

public class XMLSerializationManager : MonoBehaviour {
    public static XMLSerializationManager ins;

    private void Awake()
    {
        ins = this;
    }

   

    public static void saveStory(Story story)
    {
        StoryData storyToSerialize = new StoryData(story);
        Type[] extraTypes = { typeof(ComponentData) };
        XmlSerializer ser = new XmlSerializer(typeof(StoryData),extraTypes);
        FileStream fs = new FileStream(Application.dataPath + "/StreamingAssets/XML/" + storyToSerialize.name + "_data.xml", FileMode.Create);
        ser.Serialize(fs, storyToSerialize);
        fs.Close();

    }
}

[Serializable]
public class StoryData
{
    public List<PageData> pages = new List<PageData>();
    public string name;
    public StoryData() { }
    public StoryData(Story story)
    {
        Page[] pageArray = story.getPages();
        name = story.name;
        foreach(Page p in pageArray)
        {
            pages.Add(new PageData(p));
        }
    }
}

[Serializable]
public class PageData
{
    public List<GameObjectData> god = new List<GameObjectData>();
    public PageData() { }
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

[Serializable]
public class GameObjectData
{
    public List<ComponentData> cd = new List<ComponentData>();
    public List<GameObjectData> god = new List<GameObjectData>();
    public GameObjectData() { }
    public GameObjectData(GameObject go)
    {
        foreach(Transform t in go.transform)
        {
            god.Add(new GameObjectData(t.gameObject));
        }
        foreach (Component c in go.GetComponents<Component>())
        {
            if (c as RectTransform != null)
                cd.Add(new RectTransformData((RectTransform)c));
            if (c as Image != null)                                  //Side thought: I might need to serialize/deserialize mask component. If erratic behavior later do that
                cd.Add(new ImageData((Image)c));
            if (c as RawImage != null)
                cd.Add(new RawImageData((RawImage)c));
            if (c as ScrollRect != null)
                cd.Add(new ScrollRectData((ScrollRect)c));
            if (c as Scrollbar != null)
                cd.Add(new ScrollBarData((Scrollbar)c));
            if (c as Text != null)
                cd.Add(new TextData((Text) c));

        }
    }
}
[Serializable]
[XmlInclude(typeof(RectTransformData))]
[XmlInclude(typeof(RawImageData))]
[XmlInclude(typeof(ImageData))]
[XmlInclude(typeof(ScrollRectData))]
[XmlInclude(typeof(ScrollBarData))]
[XmlInclude(typeof(TextData))]
public class ComponentData
{
    public ComponentData()
    {

    }

}
[Serializable]
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

    public RectTransformData() { }
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

[Serializable]
public class ImageData : ComponentData
{
    public string sourceImagePath;
    
    public float alphaHitTestMinimumThreshold;
    public float fillAmount;
    public bool fillCenter;
    public bool fillClockwise;
    public UnityEngine.UI.Image.FillMethod fillMethod;
    public int fillOrigin;
    public float flexibleHeight;
    public float flexibleWidth;
    public bool hasBorder;
    public int layoutPriority;
    //Not saving material because not using them
    public float minHeight;
    public float minWidth;
    //figure out override sprite if needed
    public float preferredHeight;
    public float preferredWidth;
    public bool preserveAspect;
    public UnityEngine.UI.Image.Type type;


    public ImageData() { }
    public ImageData(Image image)
    {
        sourceImagePath = UnityEditor.AssetDatabase.GetAssetPath(image.sprite);
        Debug.Log("Path of image:" + sourceImagePath);

        alphaHitTestMinimumThreshold = image.alphaHitTestMinimumThreshold;
        fillAmount = image.fillAmount;
        fillCenter= image.fillCenter;
        fillClockwise = image.fillClockwise;
        fillMethod= image.fillMethod;
        fillOrigin = image.fillOrigin;
        flexibleHeight = image.flexibleHeight;
        flexibleWidth = image.flexibleWidth;
        hasBorder = image.hasBorder;
        layoutPriority = image.layoutPriority;
        
        minHeight = image.minHeight;
        minWidth = image.minWidth;
        //figure out override sprite if needed
        preferredHeight = image.preferredHeight;
        preferredWidth = image.preferredWidth;
        preserveAspect = image.preserveAspect;
        type = image.type;
    }
}
[Serializable]
public class RawImageData : ComponentData
{
    public string sourceImagePath;
    public Rect uvRect;

    public RawImageData() { }
    public RawImageData(RawImage image)
    {
        
        sourceImagePath = UnityEditor.AssetDatabase.GetAssetPath(image.texture);
        Debug.Log("SourceImagePath of RawImage: " + sourceImagePath);
        uvRect = image.uvRect;
    }
}
[Serializable]
public class ScrollRectData : ComponentData
{
    
    public ScrollRectData() { }
    public ScrollRectData(ScrollRect scrollRect)
    {
        
        Debug.Log("Copying ScrollRectData");
    }
}
[Serializable]
public class ScrollBarData : ComponentData
{
    public UnityEngine.UI.Scrollbar.Direction direction;
    public RectTransformData handleRect;
    public int numberOfSteps;
    public UnityEngine.UI.Scrollbar.ScrollEvent onValueChanged;
    public float size;
    public float value;

    public ScrollBarData() { }
    public ScrollBarData(Scrollbar sb)
    {
        direction = sb.direction;
        handleRect = new RectTransformData(sb.handleRect);
        numberOfSteps = sb.numberOfSteps;
        onValueChanged = sb.onValueChanged;
        size = sb.size;
        value = sb.value;
    }
}
[Serializable]
public class TextData : ComponentData
{
    public bool alignByGeometry;
    public TextAnchor alignment;
    public TextGenerator cachedTextGenerator;
    public TextGenerator cachedTextGeneratorForLayout;
    public float flexibleHeight;
    public float flexibleWidth;
    public Font font;
    public int fontSize;
    public FontStyle fontStyle;
    public HorizontalWrapMode horizontalOverflow;
    public int layoutPriority;
    public float lineSpacing;
    public Texture mainTexture;
    public float minHeight;
    public float minWidth;
    public float pixelsPerUnit;
    public float preferredHeight;
    public float preferredWidth;
    public bool resizeTextForBestFit;
    public int resizeTextMaxSize;
    public int resizeTextMinSize;
    public bool supportRichText;
    public string text;
    public VerticalWrapMode verticalOverflow;





    public TextData() { }
    public TextData(Text text)
    {
        Debug.Log("Copying TextData");
        alignByGeometry = text.alignByGeometry;
        alignment = text.alignment;
        cachedTextGenerator = text.cachedTextGenerator;
        cachedTextGeneratorForLayout = text.cachedTextGeneratorForLayout;
        flexibleHeight = text.flexibleHeight;
        flexibleWidth = text.flexibleWidth;
        font = text.font;
        fontSize = text.fontSize;
        fontStyle = text.fontStyle;
        horizontalOverflow = text.horizontalOverflow;
        layoutPriority = text.layoutPriority;
        lineSpacing = text.lineSpacing;
        mainTexture = text.mainTexture;
        minHeight = text.minHeight;
        minWidth = text.minWidth;
        pixelsPerUnit = text.pixelsPerUnit;
        preferredHeight = text.preferredHeight;
        preferredWidth = text.preferredWidth;
        resizeTextForBestFit = text.resizeTextForBestFit;
        resizeTextMaxSize = text.resizeTextMaxSize;
        resizeTextMinSize = text.resizeTextMinSize;
        supportRichText = text.supportRichText;
        this.text = text.text;
        verticalOverflow = text.verticalOverflow;

    }
}