using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using GamePerf.Runtime.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

public class GamePerfSDK : MonoBehaviour
{
    private UGUITracker uGUITracker;
    [DllImport("native")]
    private static extern void GamePerf_Init(string filepath);
    
    [DllImport("native")]
    private static extern void GamePerf_Update(float unscaledDeltaTime, int targetFrameRate);

    // [DllImport("native")]
    // private static extern void GamePerf_Save(string filepath);

    [DllImport("native")]
    private static extern void GamePerf_Flush();
    
    [DllImport("native")]
    private static extern void GamePerf_Event(string eventName);
    
    [DllImport("native")]
    private static extern void GamePerf_Scene(string scene_name);
    
    [DllImport("native")]
    private static extern IntPtr /* char const * */ GamePerf_GetToken();
    
    // Init GamePerf SDK
    private void Awake()
    {
        UploadData();
        uGUITracker = gameObject.AddComponent<UGUITracker>();
        GamePerf_Init(Application.persistentDataPath + "/tmp.txt");
    }

    // Update is called once per frame
    void Update()
    {
        GamePerf_Update(Time.unscaledDeltaTime, Application.targetFrameRate);
    }
    
    // Track User Click 
    public void UserClickTrack(string msg)
    {
        //Demo: GamePerf_Event("Button1"); 
        //Todo: @lixiaofeng
        GamePerf_Event(msg); 
    }

    // Track Scene
    public void OnSceneChange(string msg)
    {
        GamePerf_Scene(msg);
    }

    public void Flush()
    {
        GamePerf_Flush();
    }

    // Save GamePerf
    // public void Save()
    // {
    //     GamePerf_Save(Application.persistentDataPath + "/tmp.txt");
    // }

    public void Upload()
    {
        StartCoroutine(UploadData());
    }

    IEnumerator UploadData()
    {
        if (File.Exists(Application.persistentDataPath + "/tmp.txt"))
        {
            var token = Marshal.PtrToStringAnsi(GamePerf_GetToken());

            WWWForm form = new WWWForm();
            form.AddField("token", token);
            form.AddField("fileName", "UE4CommandLine");

            byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/tmp.txt");
            form.AddBinaryData("file", data);

            // http(s)://upload-z2.qiniup.com
            // var www = UnityWebRequest.Post("https://upload-z2.qiniup.com", form);
            // http(s)://up-z2.qiniup.com

            var www = UnityWebRequest.Post("http://up-z2.qiniup.com", form);
            // www.SetRequestHeader("Authorization", uploadToken);
            yield return www.SendWebRequest();
#if UNITY_2020_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogFormat("upload error: {0} / {1}", www.error, www.result);
            }
            else
            {
                string text = www.downloadHandler.text;
                Debug.Log("upload succeed:" + text);
            }
#else
            if (!www.isDone)
            {
                Debug.LogFormat("upload error: {0} / {1} /{2}", www.error, www.isNetworkError, www.isDone);
            }
            else
            {
                string text = www.downloadHandler.text;
                Debug.Log("upload succeed:" + text);
            }
#endif
        }
        else
        {
            Debug.LogError("[Error] File Not Exist");
        }
    }
}
