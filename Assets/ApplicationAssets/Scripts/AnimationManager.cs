using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class AnimationManager : MonoBehaviour
{

    Animator _anim;
    public GameObject AnimationMenu;
    public GameObject PlayTorus;
    public AudioSource audioSource;


    public static bool isplaying = true;

    void Start ()
    {
        _anim = GetComponent<Animator>();
	}

	void Update ()
    {

        if(isplaying == false)
        {
            stopSound();
        }
        else
        {
            audioSource.Play();
        }
		
	}


    public void playAnimation(string name)
    {
        _anim.SetTrigger(name);
    }

    public void SetActiveTrue()
    {
        PlayTorus.SetActive(true);
    }

    public void SetActiveFalse()
    {
        PlayTorus.SetActive(false);
    }


    //menu buttons activation ....
    public void EnableActiveMenu()
    {
        Button[] AnimationButtons = AnimationMenu.GetComponentsInChildren<Button>();
        for (int i = 0; i < AnimationButtons.Length; i++)
            AnimationButtons[i].interactable = true;
    }

    public void DisableActiveMenu()
    {
        Button[] AnimationButtons = AnimationMenu.GetComponentsInChildren<Button>();
        for (int i = 0; i < AnimationButtons.Length; i++)
        {
            //if (AnimationButtons[i].gameObject.name.Contains("Shot"))
              //  continue;

            if(AnimationButtons[i].gameObject.tag=="animation btn")
            AnimationButtons[i].interactable = false;
            
        }

        //MyMenu.gameObject.SetActive(false);
    }

    public void playSound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }

    public void stopSound()
    {
        audioSource.Stop();
    }
}
