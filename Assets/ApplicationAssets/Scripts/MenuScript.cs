using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class MenuScript : MonoBehaviour {


    //public Button[] menuButtons;
    public List<Button> menuButtons;

    AudioSource _audioSource;

    // screen shot
    Camera mainCamera;
    RenderTexture renderTex;
    int width = Screen.width;   // for Taking Picture
    int height = Screen.height; // for Taking Picture
    Texture2D screenshot;
    public RawImage rImage;
    public string screenShotName = "FluffyBear_ss_" + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png";
    public GameObject screenShotPanel;
    private bool isProcessing = false;


    //
    public GameObject LoadingMessage;

   



    void Start () {
        _audioSource = GetComponent<AudioSource>();
        checkBundels();
    }


    public GameObject content;
    public GameObject ButtonPrefab;
    public void AddNewButton(string ButtonName , string SceneName , GameObject Contentparent)
    {
        GameObject newButton = new GameObject(ButtonName);
        newButton = Instantiate<GameObject>(ButtonPrefab);
        newButton.transform.parent = Contentparent.transform;
        newButton.transform.localScale = new Vector3(1.0f,1.0f,1.0f);

        newButton.GetComponent<Button>().name = ButtonName;
        menuButtons.Add(newButton.GetComponent<Button>());
       

    }

    public void StopButtons()
    {
        foreach (Button b in menuButtons)
        {

           if (b.gameObject.tag=="animation btn") {
                b.interactable = false;
                b.transform.GetChild(0).gameObject.SetActive(false);
                b.transform.GetChild(1).gameObject.SetActive(false);
                b.transform.GetChild(2).gameObject.SetActive(false);

            }
        }

    }
	
    public void checkBundels()
    {
        foreach (Button b in menuButtons)
        {


            if (b.gameObject.tag == "animation btn") { 
                if (File.Exists(Application.persistentDataPath + "/AssetBundles/" + b.name + ".unity3d"))
                {
                    b.interactable = true;
                    b.transform.GetChild(0).gameObject.SetActive(false);
                    b.transform.GetChild(1).gameObject.SetActive(true);
                    b.transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    print(b.name);
                    b.transform.GetChild(1).gameObject.SetActive(false);
                    b.interactable = false;
                    b.transform.GetChild(0).gameObject.SetActive(true);
                    //print(b.GetComponent<LoadBundles>().isDownloading);
                    if (!b.GetComponent<LoadBundles>().isDownloading)
                        b.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
                }

            }
        }
    }




    public void BackTo()
    {
        SceneManager.LoadScene("MainMenu1");
    }

    public void LoadLevel(int levelID)
    {
        //SceneManager.LoadScene(levelID);
        //need loading bar when load this scene...
        StartCoroutine(Loading(levelID));

    }
    IEnumerator Loading(int levelID)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelID);
        Debug.Log("loading...");
        LoadingMessage.active = true;
        yield return async;
        Debug.Log("Loading complete");
        LoadingMessage.active = false;
    }


    public void playSound(AudioClip clip)
    {

        _audioSource.Stop();
        _audioSource.PlayOneShot(clip);

    }


    void Update()
    {
    }







    //*********************************************

    public void stopSound()
    {
         _audioSource.Stop();
    }

    public void Snapshot()
    {
        StartCoroutine(CaptureScreen());
        StartCoroutine(RStart());
    }

    public IEnumerator CaptureScreen()
    {
        yield return null; // Wait till the last possible moment before screen rendering to hide the UI

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        yield return new WaitForEndOfFrame(); // Wait for screen rendering to complete

        
        mainCamera = Camera.main.GetComponent<Camera>();
        renderTex = new RenderTexture(width, height, 24);
        mainCamera.targetTexture = renderTex;
        RenderTexture.active = renderTex;
        mainCamera.Render();
        screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();
        RenderTexture.active = null;
        mainCamera.targetTexture = null;

        // on Win7 - C:/Users/Username/AppData/LocalLow/CompanyName/GameName
        // on Android - /Data/Data/com.companyname.gamename/Files
        print(Application.persistentDataPath + "/" + screenShotName);
        File.WriteAllBytes(Application.persistentDataPath + "/" + screenShotName, screenshot.EncodeToPNG());
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true; // Show UI after we're done
    }

    IEnumerator RStart()
    {
        yield return new WaitForSeconds(1f);
        WWW www = new WWW("file://" + Application.persistentDataPath + "/" + screenShotName);
        yield return www;
        rImage.texture = www.texture;

        //shaalan start here..
        screenShotPanel.SetActive(true);
    }





    public void ButtonShare()
    {
        if (!isProcessing)
        {
            StartCoroutine(Share());
        }
    }

    public IEnumerator Share()
    {
        isProcessing = true;
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        
        WWW www = new WWW("file://" + Application.persistentDataPath + "/" + screenShotName);
        yield return www;
        Texture2D screenTexture = www.texture;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        byte[] dataToSave = screenTexture.EncodeToPNG();
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);
        if (!Application.isEditor)
        {
#if UNITY_ANDROID
            // block to open the file and share it ------------START
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "" + "");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");

            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);
#elif UNITY_IOS
			CallSocialShareAdvanced("", "", "", destination);
#else
		Debug.Log("No sharing set up for this platform.");
#endif
        }
        isProcessing = false;

    }

    // new share

#if UNITY_IOS
	public struct ConfigStruct
	{
		public string title;
		public string message;
	}

	[DllImport ("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);

	public struct SocialSharingStruct
	{
		public string text;
		public string url;
		public string image;
		public string subject;
	}

	[DllImport ("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);

	public static void CallSocialShare(string title, string message)
	{
		ConfigStruct conf = new ConfigStruct();
		conf.title  = title;
		conf.message = message;
		showAlertMessage(ref conf);
	}


	public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
	{
		SocialSharingStruct conf = new SocialSharingStruct();
		conf.text = defaultTxt;
		conf.url = url;
		conf.image = img;
		conf.subject = subject;

		showSocialSharing(ref conf);
	}
#endif

}
