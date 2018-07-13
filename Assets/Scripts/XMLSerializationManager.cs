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
using UnityEngine.EventSystems;
using UnityEditor;

public class XMLSerializationManager : MonoBehaviour {
 
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

    public static Story loadStory(string xmlPath,Canvas canvas)  //currently returns storyDAta but needs to return story when done testing
    {
        Stream reader = new FileStream(xmlPath, FileMode.Open);
        XmlSerializer ser = new XmlSerializer(typeof(StoryData));
        StoryData sd = (StoryData) ser.Deserialize(reader);
        reader.Close();
        return sd.toStory(canvas);
    }
    public static void copyComponent(Component original, Component copy)
    {
        Type type = original.GetType();
        FieldInfo[] fields = type.GetFields();
        foreach(FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
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

    public Story toStory(Canvas canvas)
    {
        Story story = new Story();
        story.name = name;
        //currentPage?

        foreach(PageData pd in pages)
        {
            story.addPage(pd.toPage(canvas,story)); //creates a new Page and adds it to list of pages
        }

        return story;
    }
}

[Serializable]
public class PageData
{
    public string name;
    public List<PrefabData> pfd = new List<PrefabData>();
    public PageData() { }
    public PageData(Page page)
    {
        name = page.getName();
        foreach(GameObject element in page.getElements())
        {
            //determine the prefab this game element belongs to (this game object is the highest parent of the prefab)
            PrefabInfo.PrefabType prefabType = element.GetComponent<PrefabInfo>().prefabType;
            if(prefabType == PrefabInfo.PrefabType.BackgroundImage)
            {
                pfd.Add(new BackgroundData(element));
            }
            else if(prefabType == PrefabInfo.PrefabType.ScrollArea)
            {
                pfd.Add(new ScrollAreaData(element));
            }
            else if(prefabType == PrefabInfo.PrefabType.Button)
            {
                pfd.Add(new ButtonData(element));
            }
        }
    }
    public Page toPage(Canvas canvas,Story story)
    {
        Page page = new Page(name,story);
        //fill page
        foreach(PrefabData data in pfd)
        {
            GameObject element = data.toPrefab(canvas);
            string buttonAction = element.GetComponent<PrefabInfo>().buttonAction;
            if (buttonAction != null && buttonAction != "")
                page.addPageElement(element, buttonAction);
            else
                page.addPageElement(element);
        }
        return page;
    }

}

[Serializable]
[XmlInclude(typeof(BackgroundData))]
[XmlInclude(typeof(ScrollAreaData))]
[XmlInclude(typeof(ButtonData))]
public abstract class PrefabData
{ 
    public PrefabData()
    {

    }
    public abstract GameObject toPrefab(Canvas canvas);
}

[Serializable]
public class BackgroundData : PrefabData
{
    public RectTransformData rtd;
    public RawImageData rawImage;
    public string action;

    public BackgroundData(){}
    public BackgroundData(GameObject background)
    {
        rtd = new RectTransformData(background.GetComponent<RectTransform>());
        rawImage = new RawImageData(background.GetComponent<RawImage>());
        action = background.GetComponent<PrefabInfo>().buttonAction;
    }

    /*Side note about rectTransforms: I want anchors to be set at the corners of the rectTransform so all scaling is percentages of the screen size
     * This means the rectTransform scale must be (1,1,1) and offset min and max are (0,0)
     * 
     * Side side note: when the player edits a rect transform they will be dragging the anchor points and the corners of the transform will be dragged to them,
     * not the other way around
     */
    public override GameObject toPrefab(Canvas canvas)         //Decide if I need to return something based on how I add to element list in page
    {
        GameObject bg = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PageElements/BackgroundImage.prefab"), canvas.transform);
        rtd.copyToRectTransform(bg.GetComponent<RectTransform>());
        rawImage.copyToRawImage(bg.GetComponent<RawImage>());
        bg.GetComponent<PrefabInfo>().buttonAction = action;
        return bg;
    }
}

[Serializable]
public class ScrollAreaData : PrefabData
{
    //ScrollArea fields
    public RectTransformData rtd_SA;
    public ImageData image_SA;
    public string action;

    //TextBox fields
    public RectTransformData rtd_TB;
    public ScrollRectData srd_TB;

    //Scrollbar fields
    public RectTransformData rtd_SB;
    public ImageData image_SB;
    public ScrollbarData sb_SB;

    //Sliding Area fields
    public RectTransformData rtd_SlA;

    //Handle fields
    public RectTransformData rtd_H;
    public ImageData image_H;

    //Text fields
    public RectTransformData rtd_T;
    public TextData text_T;


    public ScrollAreaData() { }
    public ScrollAreaData(GameObject scrollArea)
    {
        Debug.Log("Filling ScrollAreaData");
        //SA fields
        rtd_SA = new RectTransformData(scrollArea.GetComponent<RectTransform>());
        image_SA = new ImageData(scrollArea.GetComponent<Image>());
        action = scrollArea.GetComponent<PrefabInfo>().buttonAction;

        //TB fields
        GameObject textBox = scrollArea.transform.GetChild(0).gameObject;
        rtd_TB = new RectTransformData(textBox.GetComponent<RectTransform>());
        srd_TB = new ScrollRectData(textBox.GetComponent<ScrollRect>());

        //SB fields
        GameObject scrollbar = textBox.transform.GetChild(0).gameObject;
        rtd_SB = new RectTransformData(scrollbar.GetComponent<RectTransform>());
        image_SB = new ImageData(scrollbar.GetComponent<Image>());
        sb_SB = new ScrollbarData(scrollbar.GetComponent<Scrollbar>());

        //SlA fields
        GameObject slidingArea = scrollbar.transform.GetChild(0).gameObject;
        rtd_SlA = new RectTransformData(slidingArea.GetComponent<RectTransform>());

        //Handle fields
        GameObject handle = slidingArea.transform.GetChild(0).gameObject;
        rtd_H = new RectTransformData(handle.GetComponent<RectTransform>());
        image_H = new ImageData(handle.GetComponent<Image>());

        //TextFields
        GameObject text = textBox.transform.GetChild(1).gameObject;
        rtd_T = new RectTransformData(text.GetComponent<RectTransform>());
        text_T = new TextData(text.GetComponent<Text>()); 
    }
    public override GameObject toPrefab(Canvas canvas)         //Decide if I need to return something based on how I add to element list in page
    {
        GameObject sa = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PageElements/ScrollArea.prefab"), canvas.transform);
        rtd_SA.copyToRectTransform(sa.GetComponent<RectTransform>());
        image_SA.copyToImage(sa.GetComponent<Image>());
        sa.GetComponent<PrefabInfo>().buttonAction = action;

        GameObject tb = sa.transform.GetChild(0).gameObject;
        rtd_TB.copyToRectTransform(tb.GetComponent<RectTransform>());
        srd_TB.copyToScrollRect(tb.GetComponent<ScrollRect>());

        GameObject sb = tb.transform.GetChild(0).gameObject;
        rtd_SB.copyToRectTransform(sb.GetComponent<RectTransform>());
        image_SB.copyToImage(sb.GetComponent<Image>());
        sb_SB.copyToScrollbar(sb.GetComponent<Scrollbar>());

        GameObject sla = sb.transform.GetChild(0).gameObject;
        rtd_SlA.copyToRectTransform(sla.GetComponent<RectTransform>());

        GameObject h = sla.transform.GetChild(0).gameObject;
        rtd_H.copyToRectTransform(h.GetComponent<RectTransform>());
        image_H.copyToImage(h.GetComponent<Image>());

        GameObject t = tb.transform.GetChild(1).gameObject;
        rtd_T.copyToRectTransform(t.GetComponent<RectTransform>());
        text_T.copyToText(t.GetComponent<Text>());

        return sa;
    }
}

[Serializable]
public class ButtonData : PrefabData
{
    public RectTransformData rtd;
    public ImageData image;
    public EventTriggerData etd;
    public string action;


    public ButtonData() { }
    public ButtonData(GameObject button)
    {
        rtd = new RectTransformData(button.GetComponent<RectTransform>());
        image = new ImageData(button.GetComponent<Image>());
        etd = new EventTriggerData(button.GetComponent<EventTrigger>());
        action = button.GetComponent<PrefabInfo>().buttonAction;
    }
    public override GameObject toPrefab(Canvas canvas)
    {
        GameObject trigger = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PageElements/Button.prefab"), canvas.transform);
        rtd.copyToRectTransform(trigger.GetComponent<RectTransform>());
        image.copyToImage(trigger.GetComponent<Image>());
        etd.copyToEventTrigger(trigger.GetComponent<EventTrigger>());
        trigger.GetComponent<PrefabInfo>().buttonAction = action;
        return trigger;
    }
}


[Serializable]
[XmlInclude(typeof(RectTransformData))]
[XmlInclude(typeof(RawImageData))]
[XmlInclude(typeof(ImageData))]
[XmlInclude(typeof(ScrollRectData))]
[XmlInclude(typeof(ScrollbarData))]
[XmlInclude(typeof(TextData))]
[XmlInclude(typeof(EventTriggerData))]
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

    public void copyToRectTransform(RectTransform target)
    {
        GameObject gameObject = new GameObject();
        RectTransform rc = gameObject.AddComponent<RectTransform>();
        rc.anchoredPosition = anchoredPosition;
        rc.anchorMax = anchorMax;
        rc.anchorMin = anchorMin;
        rc.offsetMax = offsetMax;
        rc.offsetMin = offsetMin;
        rc.pivot = pivot;
        rc.sizeDelta = sizeDelta;
        XMLSerializationManager.copyComponent(rc, target);
        GameObject.Destroy(gameObject);
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
    public Color color;


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
        color = image.color;
    }

    public void copyToImage(Image target)
    {
        GameObject gameObject = new GameObject();
        Image image = gameObject.AddComponent<Image>();
     
        image.sprite =(Sprite) UnityEditor.AssetDatabase.LoadAssetAtPath(sourceImagePath, typeof(Sprite));
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
        image.type =            type;
        image.color =           color;
        XMLSerializationManager.copyComponent(image, target);
        GameObject.Destroy(gameObject);
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

    public void copyToRawImage(RawImage target)
    {
        GameObject gameObject = new GameObject();
        RawImage image = gameObject.AddComponent<RawImage>();
        image.texture = (Texture) UnityEditor.AssetDatabase.LoadAssetAtPath(sourceImagePath, typeof(Texture));
        if (image.texture == null)
        {
            Debug.Log("Image file not found at: " + sourceImagePath);
            //set a default picture in its place
        }

        image.uvRect = uvRect;
        XMLSerializationManager.copyComponent(image, target);
        GameObject.Destroy(gameObject);
    }
}

[Serializable]
public class ScrollRectData : ComponentData
{
    //public RectTransform content;     //get a reference to the text rectTransform that is a child of this while deserializing.
    public float decelerationRate;      //Could probably just have a default value to reduce xml filesize
    public float elasticity;
    //public bool horizontal;  //going to be false
    // public float horizontalNormalizedPosition;
    //all three horizontal scrollbar fields are omitted because not using horizontal scrolling
    public bool inertia;
    public UnityEngine.UI.ScrollRect.MovementType movementType;
    public Vector2 normalizedPosition;
    //onValueChanged I don't believe needs to be set
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
        inertia = sr.inertia;
        movementType = sr.movementType;
        normalizedPosition = sr.normalizedPosition;
        scrollSensitivity = sr.scrollSensitivity;
        velocity = sr.velocity;
        vertical = sr.vertical;
        verticalNormalizedPosition = sr.verticalNormalizedPosition;
        verticalScrollbarSpacing = sr.verticalScrollbarSpacing;
        verticalScrollbarVisibility = sr.verticalScrollbarVisibility;
    }
    public void copyToScrollRect(ScrollRect target)                 //Scroll rect stuff is all kinds of screwed because it needs references to the other gameobjects
    {
        GameObject gameObject = new GameObject();
        ScrollRect rect = gameObject.AddComponent<ScrollRect>();
        //rect.normalizedPosition = normalizedPosition;
        rect.decelerationRate = decelerationRate;
        rect.elasticity = elasticity;
        rect.inertia = inertia;
        rect.movementType = movementType;
        
        rect.scrollSensitivity = scrollSensitivity;
        rect.velocity = velocity;
        rect.vertical = vertical;
       // rect.verticalNormalizedPosition = verticalNormalizedPosition;
        rect.verticalScrollbarSpacing = verticalScrollbarSpacing;
        rect.verticalScrollbarVisibility = verticalScrollbarVisibility;

        XMLSerializationManager.copyComponent(rect, target);
        GameObject.Destroy(gameObject);
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
    public void copyToScrollbar(Scrollbar target)
    {
        GameObject gameObject = new GameObject();
        Scrollbar sb = gameObject.AddComponent<Scrollbar>();
        sb.direction = direction;
        //sb.handleRect = handleRect.toRectTransform(null);   //Needs to get the rect transform of the Handle after its instantiated
        sb.numberOfSteps = numberOfSteps;
        sb.onValueChanged = onValueChanged;
        sb.size = size;
        sb.value = value;

        XMLSerializationManager.copyComponent(sb, target);
        GameObject.Destroy(gameObject);
    }
}

[Serializable]
public class TextData : ComponentData
{
    public bool alignByGeometry;
    public TextAnchor alignment;
    //public TextGenerator cachedTextGenerator;         //create new TextGenerators later. I believe all settings like font are here
    //public TextGenerator cachedTextGeneratorForLayout;
    public string fontPath;         //When deserializing make sure missing fonts/images are handled
    public int fontSize;
    public FontStyle fontStyle;
    //public HorizontalWrapMode horizontalOverflow;  //Text will always Wrap horizontally. Initialize as such.
    public float lineSpacing;
    //public Texture mainTexture;  //probably keep this default since I never changed it before
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
        Debug.Log("FontPath: " + UnityEditor.AssetDatabase.GetAssetPath(text.font));
        fontPath = UnityEditor.AssetDatabase.GetAssetPath(text.font);
        fontSize = text.fontSize;
        fontStyle = text.fontStyle;
        lineSpacing = text.lineSpacing;
        resizeTextForBestFit = text.resizeTextForBestFit;
        resizeTextMaxSize = text.resizeTextMaxSize;
        resizeTextMinSize = text.resizeTextMinSize;
        supportRichText = text.supportRichText;
        this.text = text.text;
    }
    public void copyToText(Text target)
    {
        GameObject gameObject = new GameObject();
        Text t = gameObject.AddComponent<Text>();
        t.alignByGeometry = alignByGeometry;
        t.alignment = alignment;
        t.font = UnityEditor.AssetDatabase.LoadAssetAtPath<Font>(fontPath);
        if(t.font == null)
        {
            Debug.Log("Font not found");
            //set default font
        }

        t.fontSize = fontSize;
        t.fontStyle = fontStyle;
        t.lineSpacing = lineSpacing;
        t.resizeTextForBestFit = resizeTextForBestFit;
        t.resizeTextMaxSize = resizeTextMaxSize;
        t.resizeTextMinSize = resizeTextMinSize;
        t.supportRichText = supportRichText;
        t.text = text;

        XMLSerializationManager.copyComponent(t, target);
        GameObject.Destroy(gameObject);
    }
}

[Serializable]
public class EventTriggerData : ComponentData
{
    public List<EventTrigger.Entry> triggers;

    public EventTriggerData() { }
    public EventTriggerData(EventTrigger trigger)
    {
        triggers = trigger.triggers;
    }
    public void copyToEventTrigger(EventTrigger target)
    {
        GameObject gameObject = new GameObject();
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        trigger.triggers = triggers;
        GameObject.Destroy(gameObject);
    }
}