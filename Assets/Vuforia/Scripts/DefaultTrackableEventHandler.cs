/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    /// 
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;

        #endregion // PRIVATE_MEMBER_VARIABLES
       
       public static Button snapShotButton;
       public static Button screenShotButton;

        #region UNTIY_MONOBEHAVIOUR_METHODS

        IEnumerator Start()
        {

            print("trackble...");

            yield return null;
           
            snapShotButton = BundlesManager.reference.snapShotButton;
          
            screenShotButton = BundlesManager.reference.screenShotButton;
            
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }


            
        }

        

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Animator[] animatorComponents = GetComponentsInChildren<Animator>(true);

            snapShotButton.interactable=true;
            screenShotButton.interactable=true;

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            // Enable Canvas:
            foreach (Canvas component in canvasComponents)
            {
                component.enabled = true;
            }

            //Start Animation
            foreach (Animator component in animatorComponents)
            {
                component.SetTrigger("Start");
            }

            AnimationManager.isplaying = true;

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Animator[] animatorComponents = GetComponentsInChildren<Animator>(true);

            snapShotButton.interactable = false;
            screenShotButton.interactable=false;
            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            // Disable Canvas:
            foreach (Canvas component in canvasComponents)
            {
                component.enabled = false;
            }

            try
            {
                GetComponentInChildren<AnimationManager>().DisableActiveMenu();
                GetComponentInChildren<AnimationManager>().SetActiveFalse();
            }
            catch { }

            //Restart Animation
            foreach (Animator component in animatorComponents)
            {
                component.SetTrigger("Reset");
            }


            AnimationManager.isplaying = false;

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
