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
        currentPage = new Page();
        GameObject initialText = GameObject.Instantiate(ScrollArea,canvas.transform);
        initialText.GetComponent<Text>().text = "DefaultText";
        initialText.SetActive(false);
        currentPage.addPageElement(initialText);
        currentPage.setVisible(true);
	}
	
	
	void Update () {
		
	}
}
