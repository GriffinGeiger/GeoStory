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

        Page page1 = new Page("introPage1");
        GameObject bg = GameObject.Instantiate(Background);
        bg.transform.SetParent(canvas.transform);
        bg.GetComponent<RectTransform>().pivot = new Vector3(0, 0, 0);
        bg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        page1.addPageElement(bg, ButtonActionConstants.CHANGE_PAGE("introPage2"));

        Page page2 = new Page("introPage2");
        GameObject bg2 = GameObject.Instantiate(Background);
        bg2.transform.SetParent(canvas.transform);
        bg2.GetComponent<RectTransform>().pivot = new Vector3(0, 0, 0);
        bg2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        page2.addPageElement(bg2);


        GameObject pg2Text = GameObject.Instantiate(ScrollArea);
        pg2Text.GetComponentInChildren<Text>().text = "Welcome to Geostory";
        pg2Text.transform.SetParent(canvas.transform);
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
