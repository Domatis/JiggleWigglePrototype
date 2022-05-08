using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraAspectRationHandler : MonoBehaviour
{
    [SerializeField] private AspectRatioFitter afitter;     //It needs to attached to gameplaycanvas.
    [SerializeField] private CanvasScaler canvasScaler;
    [Tooltip("X : width, Y : height")]
    [SerializeField] private Vector2 baseAspectRatio;       // X width, Y height
    [SerializeField ]private Camera cam;
    

    

    private void Awake() 
    {
            DoCamSettings();
    }
    
    private void OnPreCull() 
    {
        Rect baseRect = cam.rect;

        Rect basicRect = new Rect(0,0,1,1);

        cam.rect = basicRect;
        GL.Clear(true,true,Color.black,1);

        cam.rect = baseRect;
    }


    public void DoCamSettings()
    {
        float baseratio = baseAspectRatio.x/baseAspectRatio.y;
        afitter.aspectRatio = baseratio;  //Set the base aspect ratio.
        
        //Make calculations.
        float height = Screen.height;
        float width = Screen.width;

        float currentAspectRatio = width/height;
        
        if(currentAspectRatio > baseratio)
        {
            afitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;

            canvasScaler.matchWidthOrHeight = 1;    //Height base.

            //Set cameras width value and x positions.
            float widthValue = baseratio/currentAspectRatio;
            float xPos = (1-widthValue)/2;

            Rect camrect = cam.rect;
            camrect.width = widthValue;
            camrect.x = xPos;
            cam.rect = camrect;
        }

        else if(currentAspectRatio < baseratio)
        {
            afitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;

            canvasScaler.matchWidthOrHeight = 0;    //Width base.

            //Set cameras height value and y positions.
            float heightValue = currentAspectRatio/baseratio;
            float yPos = (1-heightValue)/2;
            
            Rect camrect = cam.rect;
            camrect.height = heightValue;
            camrect.y = yPos;
            cam.rect = camrect;

            
        }
    }
}
