using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoll : MonoBehaviour
{
    public static Texture PickImage()
    {
        string imagePath = "";
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                imagePath = path;
            }
        });
        if (imagePath != "")
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(imagePath); //if want to limit filesize this function will do so
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + imagePath);
                return null;
            }
            return texture;
        }
        return null;
    }
	
}
