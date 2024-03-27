using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public enum SceneType
    {
        TITLE,
        SELECT,
        STAGE_1,
        STAGE_2,
    }

    private SceneType mCurrentScene;

    public SceneType CurScene
    {
        get { return mCurrentScene; }
        set { mCurrentScene = value; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        transform.GetChild(0).gameObject.SetActive(true);

    }



    public void QuitGame()
    {
        Application.Quit();
    }  

    private void OnSceneUnloaded(Scene scene)
    {
        GameObject[] dontDestroyObjects = GameObject.FindGameObjectsWithTag("DontDestroy");

        for (int i = 0; i < dontDestroyObjects.Length; i++)
        {
            Destroy(dontDestroyObjects[i]);
        }
    }
    private void DontDestroyObjectDestroy()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
