using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Page currentPage;
	
	void Start ()
    {
        currentPage = new Page();
        TextElement defaultText = new TextElement();
        defaultText.setText("Sample Text");
        currentPage.addPageElement(defaultText);
        currentPage.isVisible(true);
	}
	
	
	void Update () {
		
	}
}
