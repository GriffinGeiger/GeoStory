using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Builds and serializes the Story object that will be viewed on startup. 
public class BuildIntroStory : MonoBehaviour {

    public static GameObject ScrollArea;
    public static GameObject Background;

    /*public static void buildIntro()
    {
        Debug.Log("here");
        Story intro = new Story();
        intro.name = "intro";

        Page page1 = new Page("introPage1");
        GameObject bg = GameObject.Instantiate(Background);
        page1.addPageElement(bg,ButtonActionConstants.CHANGE_PAGE("introPage2"));

        Page page2 = new Page("introPage2");
        GameObject bg2 = GameObject.Instantiate(Background);
        page2.addPageElement(bg2);
        GameObject pg2Text = GameObject.Instantiate(ScrollArea);
        pg2Text.GetComponentInChildren<Text>().text = "Welcome to Geostory";
        page2.addPageElement(pg2Text);

        intro.setCurrentPage("introPage1");

        intro.addPage(page1);
        intro.addPage(page2);

        Stream stream = File.Open("IntroStory.story", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();

        Debug.Log("Serializing intro page");
        bf.Serialize(stream, intro);
        stream.Close();



        }
        */
}
