using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuAnimConrol : MonoBehaviour {



    private Animator anim;


    void Start() {

        anim = GetComponent<Animator>();


    }


    public void playAnimation(string clipName) {

        anim.Play(clipName);


    }


}
