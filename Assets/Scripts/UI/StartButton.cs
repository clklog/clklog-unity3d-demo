using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif
#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

public class StartButton : MonoBehaviour
{
    private string username = "usertest";

    public void StartGame()
    {
        if (PlayerData.instance.ftueLevel == 0)
        {
            PlayerData.instance.ftueLevel = 1;
            PlayerData.instance.Save();
#if UNITY_ANALYTICS
            AnalyticsEvent.FirstInteraction("start_button_pressed");
#endif
        }

#if UNITY_PURCHASING
        var module = StandardPurchasingModule.Instance();
#endif
        // 模拟登录
        TestSensors.Login(username);

        SceneManager.LoadScene("main");
    }

    public void SetUsername(string name)
    {
        username = name;
    }


    /*void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "用户名:");
        username = GUI.TextField(new Rect(10, 30, 200, 20), username);
    }*/
}
