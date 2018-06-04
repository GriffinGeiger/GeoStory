using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    
    public Page currentPage;
    public GameObject ScrollArea;
    public GameObject Background;
    public Canvas canvas;
	void Start ()
    {
        currentPage = new Page("InitialPage");
        Debug.Log("Current page is: " + currentPage.name);
        GameObject initialText = GameObject.Instantiate(ScrollArea,canvas.transform);
        GameObject initialBackground = GameObject.Instantiate(Background, canvas.transform);
        initialBackground.transform.SetAsFirstSibling();
        initialText.GetComponentInChildren<Text>().text = "DefaultText";
        currentPage.addPageElement(initialText);
        currentPage.addPageElement(initialBackground);
	}
	
	
	void Update () {
		
	}
}
