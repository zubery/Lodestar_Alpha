using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancesPerLevel : Metric
{
    public static int Advances;
    public override string Name { get { return "Overall Advance Count"; } }
    public override void Initialize()
    {
        Advances = 0;
    }
    public override void Finalize()
    {
        ValueToLock = Advances;
    }
}
