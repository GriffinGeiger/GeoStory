﻿using System.Collections;
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
    public GameObject scrollArea;
    public GameObject background;
    public GameObject button;
    public RectTransform testTransform; //Delete this when done with testing
    public RectTransform scrollContent;
    public static string defaultFontPath = "Assets/Fonts/jesaya free.ttf";

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
        currentStory = new Story();
	}
    public void buildIntro()
    {
        Story intro = new Story();
        intro.name = "intro";

        Page page1 = new Page("introPage1",intro);
        Page page2 = new Page("introPage2", intro);
        Page page3 = new Page("introPage3", intro);

        GameObject bg = GameObject.Instantiate(background,canvas.transform);        //Make sure you instantiate with canvas as parent or transform values will go off page
        bg.name = "bg1";
        bg.GetComponent<PageElementEventTrigger>().SetConnectedPage(page2);
        bg.GetComponent<PageElementEventTrigger>().action = PageElementEventTrigger.Action.Change;
        page1.addPageElement(bg);

        
        GameObject bg2 = GameObject.Instantiate(background,canvas.transform);
        bg2.name = "bg2";
        page2.addPageElement(bg2);

        GameObject bg3 = GameObject.Instantiate(background, canvas.transform);
        bg3.name = "bg3";
        page3.addPageElement(bg3);

        GameObject pg1Button = GameObject.Instantiate(button, canvas.transform);
        pg1Button.name = "pg1button";
        pg1Button.GetComponentInChildren<Text>().text = "Page3";
        pg1Button.GetComponent<PageElementEventTrigger>().SetConnectedPage(page3);
        pg1Button.GetComponent<PageElementEventTrigger>().action = PageElementEventTrigger.Action.Change;
        page1.addPageElement(pg1Button);

        GameObject pg2Button = GameObject.Instantiate(button, canvas.transform);
        pg2Button.name = "pg2Button";
        pg2Button.GetComponentInChildren<Text>().text = "Nothing to press here";
        page2.addPageElement(pg2Button);

        GameObject pg2Text = GameObject.Instantiate(scrollArea,canvas.transform);
        pg2Text.name = "pg2text";
        pg2Text.GetComponentInChildren<Text>().text = "Welcome to Geostory";
        pg2Text.GetComponent<PageElementEventTrigger>().SetConnectedPage(page3);
        pg2Text.GetComponent<PageElementEventTrigger>().action = PageElementEventTrigger.Action.Change;
        page2.addPageElement(pg2Text);

        intro.addPage(page1);
        intro.addPage(page2);
        intro.addPage(page3);

        page2.setVisible(false);
        page3.setVisible(false);
        intro.setCurrentPage("introPage1");
        currentStory = intro;
        
        

      //  XMLSerializationManager.saveStory(intro);

    }

    void Update () {
		
	}
}
