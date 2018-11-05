using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancesToBeatLevelMetric : Metric
{
    public static int Advances;

    public override string Name { get { return "Advances During Successful Run"; } }

    public override void Initialize()
    {
        Advances = 0;
    }
    public override void Finalize()
    {
        ValueToLock = Advances;
    }

}
