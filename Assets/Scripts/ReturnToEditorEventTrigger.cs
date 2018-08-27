using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ReturnToEditorEventTrigger : EventTrigger, IPointerClickHandler {

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
	public new void OnPointerClick(PointerEventData data)
    {
        XMLSerializationManager.saveStory(gm.currentStory);
        gm.changeMode(GameManager.Mode.EditStory);
    }
}
