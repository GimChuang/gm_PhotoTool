using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTool : MonoBehaviour
{

    // Rect region to take screenshot
    public int startX;
    public int startY;
    public int width;
    public int height;

    Texture2D tex2d_photo;
    Rect photoRegion;

    // An event which will be triggered when the photo is taken
    public delegate void PhotoTaken(Texture2D _photoTaken);
    public static event PhotoTaken OnPhotoTaken;

    public void Init()
    {
        tex2d_photo = new Texture2D(width, height, TextureFormat.RGB24, false);
        //photoRegion = new Rect(startX, startY, width, height);
        // Do some math here because the coordinate of ReadPixel and GUI.DrawTexture seem to be different
        photoRegion = new Rect(startX, Screen.height - height - startY, width, height);

        Debug.LogWarning("PhotoUtility init with width:" + width);
    }

    // Overload function of Init(). This supports overriding the rect settings.
    public void Init(int _startX, int _startY, int _width, int _height)
    {
        startX = _startX;
        startY = _startY;
        width = _width;
        height = _height;

        tex2d_photo = new Texture2D(width, height, TextureFormat.RGB24, false);
        //photoRegion = new Rect(startX, startY, width, height);
        photoRegion = new Rect(startX, Screen.height - height - startY, width, height);

        Debug.LogWarning("PhotoUtility init with width:" + _width);
    }

    public void TakePhoto()
    {
        StartCoroutine(E_TakePhoto());
    }

    IEnumerator E_TakePhoto()
    {
        yield return new WaitForEndOfFrame();

        tex2d_photo = new Texture2D(width, height, TextureFormat.RGB24, false);
        photoRegion = new Rect(startX, Screen.height - height - startY, width, height);

        tex2d_photo.ReadPixels(photoRegion, 0, 0);
        tex2d_photo.Apply();
        /*
        Debug.Log("photoRegion w: " + photoRegion.width);
        Debug.Log("photoRegion h: " + photoRegion.height);
        Debug.Log("photoRegion x: " + photoRegion.x);
        Debug.Log("photoRegion y: " + photoRegion.y);
        Debug.Log(tex2d_photo.width);
        Debug.Log(tex2d_photo.height);
        */

        if (OnPhotoTaken != null)
            OnPhotoTaken(tex2d_photo);

        Debug.Log("Photo taken!");

    }

    #region Debug Region to take photo

    public bool debugOnGUI;
    public Texture debugRegionTex;
    public Color debugRegionColor = Color.white;

    void OnGUI()
    {
#if UNITY_EDITOR 
        // Leave bool debugOnGUI as is
#elif UNITY_STANDALONE
        debugOnGUI = false;
#endif
        if (debugOnGUI)
        {
            photoRegion = new Rect(startX, startY, width, height);
            GUI.color = debugRegionColor;
            GUI.DrawTexture(photoRegion, debugRegionTex);         
        }
    }

    #endregion Debug Region to take photo
}
