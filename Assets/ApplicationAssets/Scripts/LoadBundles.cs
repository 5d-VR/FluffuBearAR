using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LoadBundles : MonoBehaviour
{


    public string _path = "http://fairy003.com/AssetBundles/Shaalan/{0}.unity3d";
    public string localPath;
    public string SceneName;
    float downloadingProgress;
    public MenuScript menu;
    public Image progressbar;
    float loadingProgress;
    //public Text progressText;
    public bool isDownloading;

    public bool load;
    GameObject sceneObject;
    //public Shader altShader;
    Shader currShader;
    Color currColor;
    Texture currTexture;

    public GameObject QuestionPanel;
    static string bundlenametodelete = "";
    public GameObject networkmessage;
    static GameObject downloadprogress;
    public string len;
    public int totalLen;

    // Use this for initialization
    void Start()
    {
        localPath = Application.persistentDataPath + "/AssetBundles/{0}.unity3d";
     
        //StartCoroutine(DownloadBundles("husky"));


        if (load)
            LoadAssetBundle();
    }

    // Update is called once per frame
    void Update()
    {


        if(QuestionPanels.yesDelete == 1) //answer = yes
        {
            QuestionPanels.yesDelete = -1;
            QuestionPanel.active = false;
            print("delete");
            if (File.Exists(string.Format(localPath, bundlenametodelete)))
            {
                File.Delete(string.Format(localPath, bundlenametodelete));

                if (File.Exists(Application.persistentDataPath + "/AssetBundles/" + bundlenametodelete + ".manifest"))
                    File.Delete(Application.persistentDataPath + "/AssetBundles/" + bundlenametodelete + ".manifest");

                menu.checkBundels();
            }
        }
        if(QuestionPanels.yesDelete == 0) //answer = no
        {
            QuestionPanels.yesDelete = -1;
            QuestionPanel.active = false;
            menu.checkBundels();
        }

        if (isDownloading)
        {
            progressbar.gameObject.active = true;
        }


        //no connection message panel process..
        if (!QuestionPanels.ok)
        {
            QuestionPanels.ok = true; 
            networkmessage.active = false;
            menu.checkBundels();
            downloadprogress.active = false;

        }

    }


    private bool CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        //test internet acess..
        WWW www = new WWW("http://google.com");
        if (www.error == null)
        {
            return false;
        }

        return true;
    }

    public void CallDownload(string bundleName)
    {
        //check for network before downloadbundle
        downloadprogress = gameObject.transform.GetChild(2).gameObject;
        if (CheckNetwork())
        {
            StartCoroutine(DownloadBundles(bundleName));
        }
        else
        {
            Debug.Log("Error. Check internet connection!");
            networkmessage.active = true;
        }   
    }



    void getAssetBundleSize(string bundleName)
    {
        WebRequest req = HttpWebRequest.Create(string.Format(_path, bundleName));
        req.Method = "HEAD";
        float ContentLength;

        
        using (System.Net.WebResponse resp = req.GetResponse())
        {
            // float.TryParse(resp.ContentLength.ToString(), out ContentLength);
            len = resp.ContentLength.ToString();
        }

       // len = len.Substring(0, 3);
        totalLen = int.Parse(len);
    }

    IEnumerator DownloadBundles(string bundleName)
    {


        getAssetBundleSize(bundleName);
        //*********************************************
        if (!File.Exists(string.Format(localPath, bundleName)))
        {
            print("kokoook " +_path);
            print(string.Format(_path, bundleName));
            print(string.Format(localPath, bundleName));


            WWW www = new WWW(string.Format(_path, bundleName));
            while (!www.isDone)
            {
                downloadingProgress = www.progress;
                //  progressbar.fillAmount = www.progress;
                //print(www.bytesDownloaded +"/"+ www.bytes.Length);

                  progressbar.fillAmount = (float.Parse(www.bytesDownloaded.ToString()) / float.Parse(totalLen.ToString()));


                //print((www.progress * 100).ToString("F1") + " %");
                isDownloading = true;

                yield return null;

            }


            print("done1");
            if (string.IsNullOrEmpty(www.error))
            {
                print("done");
                if (!Directory.Exists(Application.persistentDataPath + "/AssetBundles"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/AssetBundles");
                }

                byte[] bytes = www.bytes;
                File.WriteAllBytes(string.Format(localPath, bundleName), bytes);


                menu.checkBundels();

                print("hereeee.....");

                isDownloading = false;
                downloadprogress.SetActive(false);
                
                PlayerPrefs.SetString(bundleName, "DONE");
                SelectedBtn.interactable = true;
                
            }
            else
            {
                //_isdownloading = false;
                print("Error while Downloading: "+ www.error);
            }

            // www.assetBundle.Unload(true);

        }

    }
    public void SelectBtn(string bundle)
    {
        if (bundle == "tiger")
            SelectedBtn = TigerBtn;
        else if (bundle == "husky")
            SelectedBtn = HuskyBtn;
        else if (bundle == "trex")
            SelectedBtn = TrexBtn;
        else if (bundle == "dog")
            SelectedBtn = DogBtn;
    }
    Button SelectedBtn;
    public Button TigerBtn, HuskyBtn, TrexBtn, DogBtn;

    /* public void FillProgressBar(Image PBImage)
     {
         StartCoroutine(ReflectProgress(PBImage));
     }

     private IEnumerator ReflectProgress(Image pBImage)
     {
         while(_isdownloading)
         {
             pBImage.fillAmount = downloadingProgress;
             yield return null;
         }


     }*/

    private void LoadAssetBundle()
    {
        if (File.Exists(string.Format(localPath, SceneName)))
        {

            AssetBundle ab = AssetBundle.LoadFromFile(string.Format(localPath, SceneName));
            if (ab)
            {
                sceneObject = ab.LoadAsset<GameObject>(SceneName);
                GameObject go = Instantiate(sceneObject);
                go.transform.parent = transform;
                go.transform.localScale = new Vector3(1,1,1);
               // changeMaterial(go);
                //progressText.text = "Done";
                ab.Unload(false);
            }
            else
            {
                //progressText.text = "failed";
            }

        }

    }



    public void DeleteBundle(string bundleName)
    {
        QuestionPanel.active = true;
        menu.StopButtons();
        print(bundleName);
        bundlenametodelete = bundleName;
        print(bundlenametodelete);
    }


    void changeMaterial(GameObject go)
    {
        Renderer[] goRenderer = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in goRenderer)
        {
            currShader = r.material.shader;
            currColor = r.material.color;

            Material mat = new Material(Shader.Find(currShader.name));
            if (r.material.mainTexture)
            {
                currTexture = r.material.mainTexture;
                mat.mainTexture = currTexture;
            }

            if (r.material.GetTexture("_BumpMap"))
            {
                mat.SetTexture("_BumpMap", r.material.GetTexture("_BumpMap"));

            }

            print(currShader.name);
            if (currShader.name == "Standard")
            {
                // print("change");

                mat.SetInt("_SrcBlend", r.material.GetInt("_SrcBlend"));// (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", r.material.GetInt("_DstBlend"));// (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", r.material.GetInt("_ZWrite"));//1);

                //_METALLICGLOSSMAP
                mat.SetFloat("_Glossiness", r.material.GetFloat("_Glossiness"));
                mat.SetFloat("_Metallic", r.material.GetFloat("_Metallic"));

                mat.EnableKeyword("_ALPHATEST_ON"); // cutout
                mat.DisableKeyword("_ALPHABLEND_ON"); // Fade
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON"); // Trans
                mat.renderQueue = r.material.renderQueue;
            }


            mat.color = currColor;

            r.material = mat;
            //r.material.shader = altShader;
            //yield return new WaitForSeconds(20);
            //r.material.shader = currShader;
        }
    }

}
