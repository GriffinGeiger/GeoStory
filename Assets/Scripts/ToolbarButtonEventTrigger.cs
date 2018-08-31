using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ToolbarButtonEventTrigger : EventTrigger, IPointerClickHandler {

    GameManager gm;
    public enum ToolbarAction { ReturnToEditor, AddPage }
    public ToolbarAction toolbarAction;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

    }
	public new void OnPointerClick(PointerEventData data)
    {
        XMLSerializationManager.saveStory(gm.currentStory);
        switch(toolbarAction)
        {
            case ToolbarAction.ReturnToEditor:
                gm.changeMode(GameManager.Mode.EditStory);
                break;
            case ToolbarAction.AddPage:
                Page page = new Page("New Page", gm.currentStory);
                page.buildDefaultPage();
                gm.currentStory.addPage(page);
                FindObjectOfType<StoryEditorManager>().addPageGraphic(page);
                break;
        }
    }
}
