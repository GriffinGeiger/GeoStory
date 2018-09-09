using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditingHandleEventTrigger : EventTrigger, IDragHandler, IPointerClickHandler
{
    public enum EditingFunction { ResizeMax, ResizeMin, Delete, EditProperties , Play}
    public RectTransform rt;
    public EditingFunction editingFunction;
    private bool appliedToPage; //True if the button is applied on a page node graphic, false if it is on an element
    private GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        if (editingFunction == EditingFunction.ResizeMax || editingFunction == EditingFunction.ResizeMin)
            rt = GetComponentInParent<EditingHandlesManager>().editedRect;
        if (GetComponentInParent<PrefabInfo>() != null)
            appliedToPage = false;  //This is on an element displayed in page editor
        else
            appliedToPage = true;   //On a page in story editor
    }

    public new void OnPointerClick(PointerEventData data)
    {
        if(editingFunction == EditingFunction.Delete)
        {
            if (appliedToPage) //This is a page
            {
                PageNodeGraphicManager pngm = GetComponentInParent<PageNodeGraphicManager>();
                pngm.page.storyRef.removePage(pngm.page.name);
                GameObject.Destroy(pngm.gameObject);
            }
            else //This is an element
            {
                Debug.Log("Deleting Element");
                PageElementEventTrigger peet = GetComponentInParent<PageElementEventTrigger>();
                Page pageRef = peet.pageRef;
                GameObject element = peet.gameObject;
                pageRef.removePageElement(element);
            }
        }
        else if(editingFunction == EditingFunction.EditProperties)
        {
            //Implement EditProperties here
            if(appliedToPage)
            {
                //Go to editPage Mode for this page
                gm.changeMode(GameManager.Mode.EditPage,GetComponentInParent<PageNodeGraphicManager>().page);
            }
            else
            {
                //open options based on what type of PageElement this is: such as a photo selector for image
            }
        }
        else if(editingFunction == EditingFunction.Play)
        {
            gm.changeMode(GameManager.Mode.Play, GetComponentInParent<PageNodeGraphicManager>().page);
        }
        
        //Resizing handles don't do anything when clicked
    }
    public new void OnDrag(PointerEventData data)
    {
        //move anchor with drag, dot will move with anchor since pivot is at (1,1)
        if (editingFunction == EditingFunction.ResizeMax)
        {
            Vector2 newMax = ((Vector2)data.position) / new Vector2(Screen.width, Screen.height);
            if(newMax.x<rt.anchorMin.x)
            {
                newMax.x = rt.anchorMin.x;
            }
            if (newMax.y < rt.anchorMin.y)
            {
                newMax.y = rt.anchorMin.y;
            }

            rt.anchorMax = newMax;
        }
        else if(editingFunction == EditingFunction.ResizeMin)
        {
            Vector2 newMin = ((Vector2)data.position) / new Vector2(Screen.width, Screen.height);
            if (newMin.x > rt.anchorMax.x)
                newMin.x = rt.anchorMax.x;
            if (newMin.y > rt.anchorMax.y)
                newMin.y = rt.anchorMax.y;
            rt.anchorMin = newMin;
        }
    }

}
