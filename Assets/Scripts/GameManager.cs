using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEditor;

public class GameManager : MonoBehaviour {

    
    
    public Story currentStory;
    public static Canvas canvas;


    public RectTransform testTransform; //Delete this when done with testing
    public GameObject storyEditorScrollWindow;
    public GameObject storyEditorToolbar;
    public GameObject pageEditorToolbar;
    public RectTransform scrollContent;
    public static string defaultFontPath = "Assets/Fonts/jesaya free.ttf";
    public static string defaultBackgroundImagePath = "Assets/Images/low-poly-iphone-6-wallpaper";
    public static string defaultButtonPrefabPath = "Assets/Prefabs/PageElements/Button.prefab";
    public static string defaultBackgroundPrefabPath = "Assets/Prefabs/PageElements/BackgroundImage.prefab";
    public static string defaultImagePrefabPath = "Assets/Prefabs/PageElements/Image.prefab";
    public static string defaultScrollAreaPrefabPath = "Assets/Prefabs/PageElements/ScrollArea.prefab";
    private Mode currentMode;

    public enum Mode { Play, EditStory, EditPage};

    public static bool created = false;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        if(!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }
    void Start ()
    {
        currentStory = XMLSerializationManager.loadStory("Assets/StreamingAssets/XML/intro_data.xml");
        storyEditorToolbar = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/StoryEditorToolbar.prefab"), canvas.transform);
        pageEditorToolbar = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PageEditor/PageEditorToolbar.prefab"), canvas.transform);
        currentStory.currentPage = currentStory.getPage(currentStory.firstPageName);
        changeMode(Mode.Play);

    }

    //Deactivates any elements that are associated with other modes and activates the one for this mode
    public void changeMode(Mode newMode)
    {
        switch(newMode)
        {
            case Mode.Play:
                storyEditorToolbar.SetActive(false);
                storyEditorScrollWindow.SetActive(false);
                //set all editor features on the page items to false
                setPageEditorActive(false);
                currentStory.currentPage.setVisible(true);
                break;
            case Mode.EditStory:
                setPageEditorActive(false);
                storyEditorToolbar.SetActive(true);
                currentStory.currentPage.setVisible(false);
                storyEditorScrollWindow.SetActive(true);
                //delete previous page nodes and rebuild them
                storyEditorScrollWindow.GetComponentInChildren<StoryEditorManager>()
                    .buildStoryEditorGraphics(currentStory);
                break;
            case Mode.EditPage:
                storyEditorScrollWindow.SetActive(false);
                storyEditorToolbar.SetActive(false);
                currentStory.currentPage.setVisible(true);
                setPageEditorActive(true);  //set all editor features on page items to true
                break;
        }
    }
    
    //jumps to the mode and page specified
    public void changeMode(Mode newMode, Page page)
    {
        //Save any changes to previous story
        XMLSerializationManager.saveStory(currentStory);
        try
        {
            currentStory.setCurrentPage(page.getName());
            changeMode(newMode);
        } catch (KeyNotFoundException)
        {
            //if page is not in current story, then change the current story
            Debug.Log("The story you are changing to is not in the same story that was previously active. Changing currentStory");
            currentStory = page.storyRef;
            currentStory.setCurrentPage(page.getName());
            changeMode(newMode);
        }
    }
    //Set this true for editing, false for playing
    public void setPageEditorActive(bool tf)
    {
        pageEditorToolbar.SetActive(tf);
        foreach (EditingHandlesManager ehm in Resources.FindObjectsOfTypeAll<EditingHandlesManager>())
        {
            ehm.gameObject.SetActive(tf);
        }
        foreach(GameObject element in currentStory.currentPage.getElements())
        {
            PrefabInfo info = element.GetComponent<PrefabInfo>();
            switch (info.prefabType)
            {
                case PrefabInfo.PrefabType.ScrollArea:
                case PrefabInfo.PrefabType.Button:
                    InputField input = element.GetComponentInChildren<InputField>();
                    if (tf)
                        input.text = input.textComponent.text;
                    input.enabled = tf;
                    break;
                case PrefabInfo.PrefabType.BackgroundImage:
                    break;
                default:
                    break;
            }

        }
        foreach (PageElementEventTrigger peet in Resources.FindObjectsOfTypeAll<PageElementEventTrigger>())
        {
            peet.buttonsActive = !tf;
        }
    }
    public Story buildIntro()
    {

        Story intro = new Story();
        intro.name = "intro";

        Page page1 = new Page("introPage1",intro);
        Page page2 = new Page("introPage2", intro);
        Page page3 = new Page("introPage3", intro);
        intro.firstPageName = "introPage1";

        GameObject bg = NewElementSpawner.instantiateImagePrefab(currentMode, true);        //Make sure you instantiate with canvas as parent or transform values will go off page
        bg.name = "bg1";
        bg.GetComponent<PageElementEventTrigger>().AddConnection(page2, null, PageElementEventTrigger.Action.Change);
        page1.addPageElement(bg);

        
        GameObject bg2 = NewElementSpawner.instantiateImagePrefab(currentMode, true);
        bg2.name = "bg2";
        page2.addPageElement(bg2);

        GameObject bg3 = NewElementSpawner.instantiateImagePrefab(currentMode, true);
        bg3.name = "bg3";
        page3.addPageElement(bg3);

        GameObject pg1Button = NewElementSpawner.instantiateButtonPrefab(currentMode);
        pg1Button.name = "pg1button";
        pg1Button.GetComponentInChildren<Text>().text = "Page3";
        pg1Button.GetComponent<PageElementEventTrigger>().AddConnection(page3, null, PageElementEventTrigger.Action.Change);
 
        page1.addPageElement(pg1Button);

        GameObject pg2Button = NewElementSpawner.instantiateButtonPrefab(currentMode);
        pg2Button.name = "pg2Button";
        pg2Button.GetComponentInChildren<Text>().text = "Nothing to press here";
        page2.addPageElement(pg2Button);

        GameObject pg2Text = NewElementSpawner.instantiateScrollAreaPrefab(currentMode);
        pg2Text.name = "pg2text";
        pg2Text.GetComponentInChildren<Text>().text = "Welcome to Geostory";
        pg2Text.GetComponent<PageElementEventTrigger>().AddConnection(page3, null, PageElementEventTrigger.Action.Change);
        page2.addPageElement(pg2Text);

        GameObject pg3Text = NewElementSpawner.instantiateButtonPrefab(currentMode);
        pg3Text.name = "pg3Text";
        pg3Text.GetComponentInChildren<Text>().text = "Click button to go to page 3";
        page3.addPageElement(pg3Text);
        pg1Button.GetComponent<PageElementEventTrigger>().AddConnection(page3, pg3Text, PageElementEventTrigger.Action.Show);

        intro.addPage(page1);
        intro.addPage(page2);
        intro.addPage(page3);

        page2.setVisible(false);
        page3.setVisible(false);
        intro.setCurrentPage("introPage1");


        return intro;

      //  XMLSerializationManager.saveStory(intro);

    }


}
