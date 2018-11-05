using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MetricManager : MonoBehaviour
{
    public static MetricManager instance;
    private Metric[] Metrics;

    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;

        // Force persistence
        DontDestroyOnLoad(gameObject);
        Metrics = GetComponents<Metric>();
        print(Metrics.Length);
    }

    public void InitializeLevelMetrics()
    {
        foreach (Metric metric in Metrics)
        {
            metric.Initialize();
        }
    }

    public void FinalizeLevelMetrics()
    {
        foreach (Metric metric in Metrics)
        {
            metric.Finalize();
            metric.LockIn();
        }
    }
    // Converts all metrics tracked in this script to their string representation
    // so they look correct when printing to a file.
    private List<string> ConvertMetricsToCSVRows()
    {
        List<string> _rows = new List<string>();
        string _topRow = "Stage,";
        foreach (Metric metric in Metrics)
        {
            _topRow += metric.Name + ",";
        }
        _rows.Add(_topRow + System.Environment.NewLine);
        for (int i = 0; i < GameManager.instance.LevelAmount; ++i)
        {
            string row = GameManager.instance.LevelNameList[i] + ",";
            foreach (Metric metric in Metrics)
            {
                if (i < metric.MetricValues.Count)
                    row += metric.MetricValues[i] + ",";
                else
                    row += ",";
            }
            _rows.Add(row + System.Environment.NewLine);
        }
        return _rows;
    }

    // Uses the current date/time on this computer to create a uniquely named file,
    // preventing files from colliding and overwriting data.
    private string CreateUniqueFileName()
    {
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "_");
        dateTime = dateTime.Replace(":", "_");
        dateTime = dateTime.Replace(" ", "___");
        return "LodeStar_metrics_" + dateTime + ".csv";
    }

    // Generate the report that will be saved out to a file.
    private void WriteMetricsToFile()
    {
        string reportFile = CreateUniqueFileName();
        List<string> _CSVRows = ConvertMetricsToCSVRows();
        foreach (string row in _CSVRows)
        {
#if !UNITY_WEBPLAYER
            File.AppendAllText(reportFile, row);
#endif
        }

    }

    private void OnApplicationQuit()
    {
        FinalizeLevelMetrics();
        WriteMetricsToFile();
    }
}

