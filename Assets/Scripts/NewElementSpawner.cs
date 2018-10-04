using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                    GameObject buttonPrefab = instantiateButtonPrefab();
                    break;
                case 1: // image
                   // Debug.Log("Image prefab not made yet");
                    GameObject imagePrefab =
                    GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultImagePrefabPath), canvas.transform);
                    gm.currentStory.currentPage.addPageElement(imagePrefab);
                    break;
                case 2: // scroll text
                    GameObject scrollAreaPrefab =
                    GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultScrollAreaPrefabPath), canvas.transform);
                    gm.currentStory.currentPage.addPageElement(scrollAreaPrefab);
                    scrollAreaPrefab.SetActive(true);
                    scrollAreaPrefab.GetComponentInChildren<InputField>().gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            dropdown.value = 3; //default value 
        }
        );
    }
    //mode is what mode you're opening the object in
    GameObject instantiateButtonPrefab(GameManager.Mode mode)
    {
        GameObject buttonPrefab = 
        GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(GameManager.defaultButtonPrefabPath), canvas.transform);
        gm.currentStory.currentPage.addPageElement(buttonPrefab);

        //set up input field for mobile input 
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
        buttonPrefab.SetActive(true);
        //buttonPrefab.GetComponentInChildren<InputField>().enabled = true;
        return buttonPrefab;
    }
}
