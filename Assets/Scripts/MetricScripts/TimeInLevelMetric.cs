using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimeInLevelMetric : Metric
{
    Stopwatch LevelTimer;
    public override string Name { get { return "Overall Time Spent"; } }

    public override void Initialize()
    {
        LevelTimer = new Stopwatch();
        LevelTimer.Start();
    }

    public override void Finalize()
    {
        LevelTimer.Stop();
        ValueToLock = LevelTimer.ElapsedMilliseconds/1000;
        LevelTimer.Reset();
    }
}
