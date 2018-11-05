using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int LevelAmount { get { return LevelNameList.Count; } }
    public List<string> LevelNameList;
    private LevelManager currentLevel;
    private string lastLevelName = null;
    public LevelManager CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            if (lastLevelName == null || lastLevelName != value.LevelName)
            {
                currentLevel = value;
                lastLevelName = value.LevelName;
                if (LevelNameList != null) LevelNameList.Add(value.LevelName);
                MetricManager.instance.InitializeLevelMetrics();
            }
        }
    }

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        LevelNameList = new List<string>();
    }

    public void GoToNextLevel()
    {
        MetricManager.instance.FinalizeLevelMetrics();
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
        FadeInBehaviour.instance.BeginFade(-1);
    }

    public void ResetLevel()
    {
        ++ResetsInLevelMetric.Resets;
        AdvancesToBeatLevelMetric.Advances = 0;
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        FadeInBehaviour.instance.BeginFade(-1);
    }
}

