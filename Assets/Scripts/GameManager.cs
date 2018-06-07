using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

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
        try
        {
            Stream stream = File.Open("IntroStory.story", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            currentStory = (Story)bf.Deserialize(stream);
            stream.Close();
        }
        catch(ArgumentNullException ane) { Debug.Log("IntroStory.story likely does not exist: " + ane ); }

        currentStory.getCurrentPage().setVisible(true);
	}

	void Update () {
		
	}
}
