using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotGetter : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (BackButton.is_Shoting == true)
        {
            BundlesManager.reference.ScreenShotPanel.transform.parent = gameObject.transform;
        }

        if (BundlesManager.reference.ScreenShotPanel.gameObject.active == true)
        {
            BundlesManager.reference.maincanvas.active = false;
        }
        else
        {
            BundlesManager.reference.maincanvas.active = true;

        }

		
	}
}
