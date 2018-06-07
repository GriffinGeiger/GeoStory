using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
       
        currentStory = new Story();
        
        currentStory.addPage(new Page("initialPage"));
        currentStory.setCurrentPage("initialPage");
        
        Debug.Log("Current page is: " + currentStory.getCurrentPage().getName());

        GameObject initialText = GameObject.Instantiate(ScrollArea,canvas.transform);
        GameObject initialBackground = GameObject.Instantiate(Background, canvas.transform);
        initialBackground.transform.SetAsFirstSibling();
        initialText.GetComponentInChildren<Text>().text = "DefaultText";

        currentStory.getCurrentPage().addPageElement(initialText);
        currentStory.getCurrentPage().addPageElement(initialBackground, ButtonActionConstants.CHANGE_PAGE("testNextPage"));

	}

	void Update () {
		
	}
}
