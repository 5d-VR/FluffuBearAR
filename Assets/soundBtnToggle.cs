using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBtnToggle : MonoBehaviour {


    bool enableSound = true;

    AudioSource audio;
    Transform child;

   public AudioSource animalAudio;
	void Start () {


        audio = GameObject.Find("Manager").GetComponents<AudioSource>()[1];
        child = transform.GetChild(0);
	}
	
	
	void Update () {
		
	}


    public void toggleBackGroundSound() {

        enabled = !enabled;

        audio.mute = !enabled;
        child.gameObject.SetActive(!enabled);

        muteAnimalSound();

    }


    void muteAnimalSound() {

        if (animalAudio!=null) {

            animalAudio.mute = !enabled;
        }


    }
}
