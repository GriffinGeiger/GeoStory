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
    public Canvas canvas;
    public GameObject ScrollArea;
    public GameObject Background;
    public RectTransform testTransform; //Delete this when done with testing



    public static bool created = false;

    private void Awake()
    {
        if(!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }
    void Start ()
    {
        /*currentStory = new Story();
        
        currentStory.addPage(new Page("initialPage"));
        currentStory.setCurrentPage("initialPage");
        
        Debug.Log("Current page is: " + currentStory.getCurrentPage().getName());

        GameObject initialText = GameObject.Instantiate(ScrollArea,canvas.transform);
        GameObject initialBackground = GameObject.Instantiate(Background, canvas.transform);
        initialBackground.transform.SetAsFirstSibling();
        initialText.GetComponentInChildren<Text>().text = "DefaultText";

        currentStory.getCurrentPage().addPageElement(initialText);
        currentStory.getCurrentPage().addPageElement(initialBackground, ButtonActionConstants.CHANGE_PAGE("testNextPage"));
        */

        currentStory = new Story();

	}
    public void buildIntro()
    {
        Story intro = new Story();
        intro.name = "intro";

        Page page1 = new Page("introPage1",intro);
        GameObject bg = GameObject.Instantiate(Background,canvas.transform);        //Make sure you instantiate with canvas as parent or transform values will go off page
        page1.addPageElement(bg, ButtonActionConstants.CHANGE_PAGE("introPage2"));

        Page page2 = new Page("introPage2",intro);
        GameObject bg2 = GameObject.Instantiate(Background,canvas.transform);
        page2.addPageElement(bg2);


        GameObject pg2Text = GameObject.Instantiate(ScrollArea,canvas.transform);
        pg2Text.GetComponentInChildren<Text>().text = "Welcome to Geostory";
        page2.addPageElement(pg2Text);

        intro.addPage(page1);
        intro.addPage(page2);

        intro.setCurrentPage("introPage1");
        currentStory = intro;
        
        

      //  XMLSerializationManager.saveStory(intro);

    }

    void Update () {
		
	}
}
