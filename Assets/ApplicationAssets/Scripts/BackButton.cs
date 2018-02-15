using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class BackButton : MonoBehaviour
{

    public  bool mShowGUIButton = false;
    private Rect mButtonRect = new Rect( (Screen.width / 7) - Screen.width / 10, (Screen.height /5) - Screen.width / 10, Screen.width / 10, Screen.height / 10);
    private Rect mImageRect;


    //public GameObject soundObj;

    private TrackableBehaviour mTrackableBehaviour;

    void Start()
    {
        mImageRect = new Rect(mButtonRect.x , mButtonRect.y + mButtonRect.height, mButtonRect.width , mButtonRect.width);
    }

    
    public void Update()
    {

        //print(is_Shoting);
    }




    public GUIStyle style;
    public GUIStyle styletexture;
    void OnGUI()
    {
        if(!is_Shoting)
        {
           
            // draw the GUI button
            if (GUI.Button(mButtonRect, "", style))
            {
                // do something on button click 
                SceneManager.LoadScene(1);
            }

            //as texture...
           // GUI.Button(mImageRect, "", styletexture);


        }


    }



    //**********************

    public static bool is_Shoting = false;

    public void SnapShotON()
    {
        is_Shoting = !is_Shoting;
    }





    //************************
    public GameObject LanguagePanel;
    public void LanguageButtonBehavior()
    {
        LanguagePanel.active = !LanguagePanel.active;
    }


}
