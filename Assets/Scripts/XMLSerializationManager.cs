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

    public static StoryData saveStory(Story story)
    {
        StoryData storyToSerialize = new StoryData(story);
        Type[] extraTypes = { typeof(ComponentData) };
        XmlSerializer ser = new XmlSerializer(typeof(StoryData),extraTypes);
        FileStream fs = new FileStream(Application.dataPath + "/StreamingAssets/XML/" + storyToSerialize.name + "_data.xml", FileMode.Create);
        ser.Serialize(fs, storyToSerialize);
        fs.Close();
        return storyToSerialize;
    }

    public static Story loadStory(string xmlPath)  //currently returns storyDAta but needs to return story when done testing
    {
        Stream reader = new FileStream(xmlPath, FileMode.Open);
        XmlSerializer ser = new XmlSerializer(typeof(StoryData));
        StoryData sd = (StoryData) ser.Deserialize(reader);
        reader.Close();
        return sd.toStory();
    }
}

[Serializable]
public class StoryData
{
    public string name;
    public List<PageData> pages = new List<PageData>();
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

    public Story toStory()
    {
        Story story = new Story();
        story.name = name;
        //currentPage?

        foreach(PageData pd in pages)
        {
            story.addPage(pd.toPage()); //creates a new Page and adds it to list of pages
        }

        return story;
    }
}

[Serializable]
public class PageData
{
    public string name;
    public List<GameObjectData> god = new List<GameObjectData>();
    public PageData() { }
    public PageData(Page page)
    {
        name = page.getName();
        //get all the game objects and construct game object data for each and put into list
        GameObject[] elements = page.getElements();
        foreach(GameObject go in elements)
        {
            god.Add(new GameObjectData(go));
        }
    }
    public Page toPage()
    {
        Page page = new Page();
        page.setName(name);
        //fill page
        foreach(GameObjectData data in god)
        {
            data.toGameObject();
            //instantiate, setVisible(false), and set as child to canvas
        }
        return page;
    }

}

[Serializable]
public class GameObjectData
{
    public string name;
    public List<ComponentData> cd = new List<ComponentData>();
    public List<GameObjectData> god = new List<GameObjectData>();
    public GameObjectData() { }
    public GameObjectData(GameObject go)
    {
        name = go.name;

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
                cd.Add(new ScrollbarData((Scrollbar)c));
            if (c as Text != null)
                cd.Add(new TextData((Text) c));

        }
    }
    public GameObject toGameObject()
    {
        GameObject go = new GameObject();
        foreach(ComponentData c in cd)
        {
            if(c as RectTransformData != null)
            {
                ((RectTransformData)c).toRectTransform(go);
            }
            if(c as ImageData != null)
            {
                ((ImageData)c).toImage(go); //Creates Image of c and applies it to go
            }
            if (c as RawImageData != null)
            {
                ((RawImageData)c).toRawImage(go);
            }
            if (c as ScrollRectData != null)
            {
                ((ScrollRectData)c).toScrollRect(go);
            }
            if (c as ScrollbarData != null)
            {
                ((ScrollbarData)c).toScrollbar(go);
            }
            if (c as TextData != null)
            {
                ((TextData)c).toText(go);
            }
        }
        foreach (GameObjectData data in god)
        {
            //instantiate and set as child to this go
            data.toGameObject().GetComponent<RectTransform>().SetParent(go.GetComponent<RectTransform>());
        }
        return go;
    }
}
[Serializable]
[XmlInclude(typeof(RectTransformData))]
[XmlInclude(typeof(RawImageData))]
[XmlInclude(typeof(ImageData))]
[XmlInclude(typeof(ScrollRectData))]
[XmlInclude(typeof(ScrollbarData))]
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
    public Vector2 sizeDelta;

    public RectTransformData() { }
    public RectTransformData(RectTransform rect)
    {
        anchoredPosition = rect.anchoredPosition;
        anchorMax = rect.anchorMax;
        anchorMin = rect.anchorMin;
        offsetMax = rect.offsetMax;
        offsetMin = rect.offsetMin;
        pivot = rect.pivot;
        sizeDelta = rect.sizeDelta;
    }
    //Creates a rect transform using the fields in this RectTransformData and applies it to a parent. While not necessary to specify a parent this 
    //is to keep a consistent style with the other components, since Unity gui elements need a parent to initialize
    public void toRectTransform(GameObject parent)
    {
        RectTransform rc = parent.AddComponent<RectTransform>();
        rc.anchoredPosition = anchoredPosition;
        rc.anchorMax = anchorMax;
        rc.anchorMin = anchorMin;
        rc.offsetMax = offsetMax;
        rc.offsetMin = offsetMin;
        rc.pivot = pivot;
        rc.sizeDelta = sizeDelta;
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

        preserveAspect = image.preserveAspect;
        type = image.type;
    }

    public void toImage(GameObject parent)
    {
        Image image = parent.AddComponent<Image>();
        //Fill the image fields
        //sourceImagePath = UnityEditor.AssetDatabase.GetAssetPath(image.sprite);
        
        image.sprite =(Sprite) UnityEditor.AssetDatabase.LoadAssetAtPath(sourceImagePath, image.sprite.GetType());
        if(image.sprite == null)
        {
            Debug.Log("Image file not found at: " + sourceImagePath);
            //set a default picture in its place
        }

        image.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
        image.fillAmount =      fillAmount;
        image.fillCenter =      fillCenter;
        image.fillClockwise =   fillClockwise;
        image.fillMethod =      fillMethod;
        image.fillOrigin =      fillOrigin;
        image.preserveAspect =  preserveAspect;
        image.type =            image.type;
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

    public void toRawImage(GameObject parent)
    {

    }
}

[Serializable]
public class ScrollRectData : ComponentData
{
    //public RectTransform content;     //get a reference to the text rectTransform that is a child of this while deserializing.
    public float decelerationRate;      //Could probably just have a default value to reduce xml filesize
    public float elasticity;
    public float flexibleHeight;
    public float flexibleWidth;
    //public bool horizontal;  //going to be false
    // public float horizontalNormalizedPosition;
    //all three horizontal scrollbar fields are omitted because not using horizontal scrolling
    public bool inertia;
    public int layoutPriority;
    public float minHeight;
    public float minWidth;
    public UnityEngine.UI.ScrollRect.MovementType movementType;
    public Vector2 normalizedPosition;
    //onValueChanged I don't believe needs to be set
    public float preferredHeight;
    public float preferredWidth;
    public float scrollSensitivity;
    public Vector2 velocity;
    public bool vertical;
    public float verticalNormalizedPosition;
    //public verticalScrollbar //get reference to verticalScrollbar that is child of this when deserializing
    public float verticalScrollbarSpacing;
    public UnityEngine.UI.ScrollRect.ScrollbarVisibility verticalScrollbarVisibility;
    //I didn't use a viewport


    public ScrollRectData() { }
    public ScrollRectData(ScrollRect sr)
    {
        decelerationRate = sr.decelerationRate;
        elasticity = sr.elasticity;
        flexibleHeight = sr.flexibleHeight;
        flexibleWidth = sr.flexibleWidth;
        inertia = sr.inertia;
        layoutPriority = sr.layoutPriority;
        minHeight = sr.minHeight;
        minWidth = sr.minWidth;
        movementType = sr.movementType;
        normalizedPosition = sr.normalizedPosition;
        preferredHeight = sr.preferredHeight;
        preferredWidth = sr.preferredWidth;
        scrollSensitivity = sr.scrollSensitivity;
        velocity = sr.velocity;
        vertical = sr.vertical;
        verticalNormalizedPosition = sr.verticalNormalizedPosition;
        verticalScrollbarSpacing = sr.verticalScrollbarSpacing;
        verticalScrollbarVisibility = sr.verticalScrollbarVisibility;
    }
    public void toScrollRect(GameObject parent)
    {

    }
}
[Serializable]
public class ScrollbarData : ComponentData
{
    public UnityEngine.UI.Scrollbar.Direction direction;
    public RectTransformData handleRect;
    public int numberOfSteps;
    public UnityEngine.UI.Scrollbar.ScrollEvent onValueChanged;
    public float size;
    public float value;

    public ScrollbarData() { }
    public ScrollbarData(Scrollbar sb)
    {
        direction = sb.direction;
        handleRect = new RectTransformData(sb.handleRect);
        numberOfSteps = sb.numberOfSteps;
        onValueChanged = sb.onValueChanged;
        size = sb.size;
        value = sb.value;
    }
    public void toScrollbar(GameObject parent)
    {

    }
}
[Serializable]
public class TextData : ComponentData
{
    public bool alignByGeometry;
    public TextAnchor alignment;
    //public TextGenerator cachedTextGenerator;         //create new TextGenerators later. I believe all settings like font are here
    //public TextGenerator cachedTextGeneratorForLayout;
    public float flexibleHeight;
    public float flexibleWidth;
    public string fontPath;         //When deserializing make sure missing fonts/images are handled
    public int fontSize;
    public FontStyle fontStyle;
    //public HorizontalWrapMode horizontalOverflow;  //Text will always Wrap horizontally. Initialize as such.
    public int layoutPriority;
    public float lineSpacing;
    //public Texture mainTexture;  //probably keep this default since I never changed it before
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
    //public VerticalWrapMode verticalOverflow;  //Text will always

    public TextData() { }
    public TextData(Text text)
    {
        alignByGeometry = text.alignByGeometry;
        alignment = text.alignment;
        flexibleHeight = text.flexibleHeight;
        flexibleWidth = text.flexibleWidth;
        Debug.Log("FontPath: " + UnityEditor.AssetDatabase.GetAssetPath(text.font));
        fontPath = UnityEditor.AssetDatabase.GetAssetPath(text.font);
        fontSize = text.fontSize;
        fontStyle = text.fontStyle;
        layoutPriority = text.layoutPriority;
        lineSpacing = text.lineSpacing;
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
    }
    public void toText(GameObject parent)
    {

    }
}