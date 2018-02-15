using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPanels : MonoBehaviour {


    public static int yesDelete = -1;

    public void yesAnswer()
    {
        yesDelete = 1;

    }
    public void NoAnswer()
    {
        yesDelete = 0;
    }



    public static bool ok = true;
     public void AnswerOK()
    {
        ok = false;
    }



}
