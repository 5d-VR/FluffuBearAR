using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class CustomMediaPlayer : MonoBehaviour {


    public MediaPlayer _media;
    float totalLength = 0;

    public Slider track;
    float cursor = 0;

	// Use this for initialization
	void Start ()
    {

        

    }



	
	// Update is called once per frame
	void Update ()
    {

        cursor = _media.m_Control.GetCurrentTimeMs();
        track.value = cursor;
	}


    bool playpause = false;
    public void play()
    {

        playpause = !playpause;

        if(playpause)
        {
            _media.m_Control.Play();
            totalLength = _media.m_Info.GetDurationMs();
            track.maxValue = totalLength;
        }
        else
        {
            _media.m_Control.Pause();
        }
        
    }


    public void sliderChanged()
    {
        float newtime = track.value;
        if(newtime != cursor)
        {
            _media.m_Control.Seek(newtime);
        }
    }

}
