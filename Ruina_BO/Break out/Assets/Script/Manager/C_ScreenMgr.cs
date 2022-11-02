using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ScreenMgr : MonoBehaviour
{
    private void Start()
    {
        SetResolution(); 
    }

    
    public void SetResolution()
    {
        int setWidth = 900; // 사용자 설정 너비
        int setHeight = 1600; // 사용자 설정 높이

        int deviceWidth = Screen.width; 
        int deviceHeight = Screen.height; 

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), false); 

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); 
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); 
        }
        else 
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
