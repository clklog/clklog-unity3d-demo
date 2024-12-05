using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePerfStarter : MonoBehaviour
{
    // Start is called before the first frame update

#if UNITY_ANDROID
    private GamePerfSDK gamePerfSDK;
    void Awake()
    {
        gamePerfSDK = gameObject.AddComponent<GamePerfSDK>();
        if (gamePerfSDK != null)
        {
            SceneManager.sceneLoaded += LoadedSceneTrack;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        gamePerfSDK.Upload();
    }

    /// <summary>
    /// 加载 Scene 时执行的回调函数
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void LoadedSceneTrack(Scene scene, LoadSceneMode mode)
    {
        gamePerfSDK.OnSceneChange(scene.name);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            gamePerfSDK.Flush();
        }
    }
#else
    private UGUITracker uiTracker;
    void Awake()
    {
        uiTracker = gameObject.AddComponent<UGUITracker>();
        SceneManager.sceneLoaded += LoadSceneTrack;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LoadSceneTrack;
    }

    private void LoadSceneTrack(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded Scene: " + scene.name);
    }
#endif
}
