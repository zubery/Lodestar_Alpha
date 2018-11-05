using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetsInLevelMetric : Metric
{
    public static int Resets;

    public override string Name { get { return "Reset Count"; } }

    public override void Initialize()
    {
         Resets = 0;
    }

    public override void Finalize()
    {
        ValueToLock = Resets;
    }
}
