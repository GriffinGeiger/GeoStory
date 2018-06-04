using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    
    public Page currentPage;
    public GameObject ScrollArea;
    public Canvas canvas;
	void Start ()
    {
        currentPage = new Page("InitialPage");
        Debug.Log("Current page is: " + currentPage.name);
        GameObject initialText = GameObject.Instantiate(ScrollArea,canvas.transform);
        initialText.GetComponentInChildren<Text>().text = "DefaultText";
        initialText.SetActive(false);
        currentPage.addPageElement(initialText);
        currentPage.setVisible(true);
        Debug.Log("Name of currentPage: " + currentPage.name);
	}
	
	
	void Update () {
		
	}
}
