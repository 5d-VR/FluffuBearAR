using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BundlesManager : MonoBehaviour {


    public Button snapShotButton;
    public Button screenShotButton;

    public GameObject maincanvas;
    public GameObject ScreenShotPanel;
    public bool shot;

    public static BundlesManager reference;

    // Use this for initialization
    void Awake ()
    {


        if (reference==null) {

            reference = this;

        }
        


        
	}
	


    
	// Update is called once per frame
	void Update ()
    {	
	}
}
