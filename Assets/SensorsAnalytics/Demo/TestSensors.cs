using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorsAnalytics;
using UnityEngine.SceneManagement;

public class TestSensors : MonoBehaviour
{
    private Dictionary<string, string> sceneNameToTitle = new Dictionary<string, string>();
    void Start()
    {
        InitSensors();
        sceneNameToTitle.Add("Start", "开始游戏");
        sceneNameToTitle.Add("Main", "游戏主页");
        sceneNameToTitle.Add("Shop", "商店");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 设置事件公共属性
    /// </summary>
    private void InitSensors()
    {
        try
        {
            SensorsDataAPI.Logout();

            Dictionary<string, object> properties = new Dictionary<string, object>();
            // 生成一个 UUID 作为 event_session_id
            var sessionId = System.Guid.NewGuid();
            properties.Add("$event_session_id", sessionId.ToString());
            SensorsDataAPI.RegisterSuperProperties(properties);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    /// <summary>
    /// 用户注册/登录
    /// </summary>
    /// <param name="username"></param>
    public static void Login(string username)
    {
        SensorsDataAPI.Login(username);
        // 模拟设置用户属性
        SetUserProfile("123456", username);
    }

    /// <summary>
    /// 设置用户属性
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    public static void SetUserProfile(string userId, string userName)
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        properties.Add("userId", userId);
        properties.Add("userName", userName);
        SensorsDataAPI.ProfileSet(properties);
    }

    /// <summary>
    /// 自定义事件 AppViewScreen
    /// </summary>
    /// <param name="elementName"></param>
    public static void AppViewScreen(string screen_name, string url, string title)
    {
        // 设置事件属性
        Dictionary<string, object> properties = new Dictionary<string, object>();
        properties.Add("$screen_name", screen_name);
        properties.Add("$url", url);
        properties.Add("$title", title);

        SensorsDataAPI.Track("$AppViewScreen", properties);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("场景 " + scene.name + " 加载完成");
        if (!sceneNameToTitle.TryGetValue(scene.name, out var title))
        {
            title = scene.name;
        }
        AppViewScreen(scene.name, scene.name, title);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SensorsDataAPI.Logout();
    }

    /*
    public string username = "";
    private void OnGUI()
    {
        username = GUI.TextField(new Rect(100, 10, 200, 30), username);
        if (GUI.Button(new Rect(10, 10, 80, 30), "Login"))
        {
            Debug.Log("login: " + username);
            Login(username);
        }
        if (GUI.Button(new Rect(10, 60, 80, 30), "Logout"))
        {
            SensorsDataAPI.Logout();
        }
        if (GUI.Button(new Rect(10, 110, 80, 30), "SetUserProfile"))
        {
            SetUserProfile("123456", username);
        }
        if (GUI.Button(new Rect(10, 160, 80, 30), "AppClick"))
        {
            AppClick("TestAppClick");
        }
        if (GUI.Button(new Rect(10, 210, 80, 30), "AppViewScreen"))
        {
            AppViewScreen("TestAppViewScreen");
        }
        if (GUI.Button(new Rect(10, 260, 80, 30), "Flush"))
        {
            SensorsDataAPI.Flush();
        }
    }*/
}
