using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewElementSpawner : MonoBehaviour {

    public Dropdown dropdown;
    public Canvas canvas;
    public GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();
        dropdown = GetComponentInChildren<Dropdown>();
        dropdown.value = 3;
        dropdown.onValueChanged.AddListener(delegate
        {
            //spawns element from dropdown selection
            Debug.Log("Value changed");
            switch(dropdown.value)
            {
                case 0: // text button
                    Debug.Log("Adding new button");
                    
                    GameObject buttonPrefab = instantiateButtonPrefab(GameManager.Mode.EditPage);
                    break;
                case 1: // image
                        // Debug.Log("Image prefab not made yet");
                    GameObject imagePrefab = instantiateImagePrefab(GameManager.Mode.EditPage, false);

                    gm.currentStory.currentPage.addPageElement(imagePrefab);
                    break;
                case 2: // scroll text
                    instantiateScrollAreaPrefab(GameManager.Mode.EditPage);
                    break;
                default:
                    break;
            }
            dropdown.value = 3; //default value 
        }
        );
    }
    //mode is what mode you're opening the object in
    public static GameObject instantiateButtonPrefab(GameManager.Mode mode)
    {
        GameObject buttonPrefab = 
        GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultButtonPrefabPath), GameManager.canvas.transform);
        FindObjectOfType<GameManager>().currentStory.currentPage.addPageElement(buttonPrefab);
       
        //set up input field for mobile input. From what I've seen input field should open mobile keyboard on its own but not in Unity Remote 5 
        InputField inputField = buttonPrefab.GetComponentInChildren<InputField>();
        inputField.keyboardType = TouchScreenKeyboardType.Default;

        switch (mode)
        {
            case GameManager.Mode.Play:
                buttonPrefab.SetActive(true);
                inputField.enabled = false;
                buttonPrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(false);
                break;
            case GameManager.Mode.EditStory:
                buttonPrefab.SetActive(false);
                inputField.enabled = false;
                buttonPrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(false);
                break;
            case GameManager.Mode.EditPage:
                buttonPrefab.SetActive(true);
                inputField.enabled = true;
                buttonPrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(true);
                break;
            default:
                break;
        }
        return buttonPrefab;
    }
    public static GameObject instantiateScrollAreaPrefab(GameManager.Mode mode)
    {
        GameObject scrollAreaPrefab =
        GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultScrollAreaPrefabPath), GameManager.canvas.transform);
        FindObjectOfType<GameManager>().currentStory.currentPage.addPageElement(scrollAreaPrefab);

        //inputfield
        InputField inputField = scrollAreaPrefab.GetComponentInChildren<InputField>();
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        switch (mode)
        {
            case GameManager.Mode.Play:
                scrollAreaPrefab.SetActive(true);
                inputField.enabled = false;
                scrollAreaPrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(false);
                break;
            case GameManager.Mode.EditStory:
                scrollAreaPrefab.SetActive(false);
                inputField.enabled = false;
                scrollAreaPrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(false);
                break;
            case GameManager.Mode.EditPage:
                scrollAreaPrefab.SetActive(true);
                inputField.enabled = true;
                scrollAreaPrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(true);
                break;
            default:
                break;
        }

        return scrollAreaPrefab;
    }
    public static GameObject instantiateImagePrefab(GameManager.Mode mode, bool isBackground)
    {
        GameObject imagePrefab;
        if(isBackground)
            imagePrefab = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultBackgroundPrefabPath), GameManager.canvas.transform);
        else
            imagePrefab = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultImagePrefabPath), GameManager.canvas.transform);

        FindObjectOfType<GameManager>().currentStory.currentPage.addPageElement(imagePrefab);

        switch (mode)
        {
            case GameManager.Mode.Play:
                imagePrefab.SetActive(true);
                try
                {
                    imagePrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(false);
                }
                catch (NullReferenceException) { }
                break;
            case GameManager.Mode.EditStory:
                imagePrefab.SetActive(false);
                try
                {
                    imagePrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(false);
                }
                catch (NullReferenceException) { }
                break;
            case GameManager.Mode.EditPage:
                imagePrefab.SetActive(true);
                try
                {
                    imagePrefab.GetComponentInChildren<EditingHandlesManager>().gameObject.SetActive(true);
                }
                catch (NullReferenceException) { }
                break;
            default:
                break;
        }


        return imagePrefab;
    }
}
