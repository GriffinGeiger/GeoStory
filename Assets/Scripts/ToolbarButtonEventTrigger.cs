using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolbarButtonEventTrigger : EventTrigger, IPointerClickHandler {

    GameManager gm;
    public enum ToolbarAction { ReturnToEditor, AddPage, EditBackground }
    public ToolbarAction toolbarAction;
    public GameObject background;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    void OnEnable()
    {
        //assign background
        if(toolbarAction == ToolbarAction.EditBackground)
        {
            foreach(PrefabInfo prefabInfo in FindObjectsOfType<PrefabInfo>())
            {
                if(prefabInfo.prefabType == PrefabInfo.PrefabType.BackgroundImage)
                {
                    background = prefabInfo.gameObject;
                }
            }
        }
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
            case ToolbarAction.EditBackground:
                background.GetComponent<Image>().sprite = Sprite.Create((Texture2D)CameraRoll.PickImage(),new Rect(),new Vector2(0.5f,0.5f));
                Debug.Log("here");
                break;
        }
    }
}
