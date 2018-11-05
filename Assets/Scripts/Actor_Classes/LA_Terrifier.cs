using UnityEngine;

public class LA_Terrifier : LodestarActor
{
    public override bool IsKillable { get { return false; } }
    public override bool IsShieldable { get { return true; } }

    public override void Move()
    {
        //throw new System.NotImplementedException();
    }

    public override void Rotate()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();
    }
    public override void BeginNewState()
    {
        //throw new System.NotImplementedException();
    }

    public override void DoNewStateChecks()
    {
       //throw new System.NotImplementedException();
    }

    public override void EndLastState()
    {
      //  throw new System.NotImplementedException();
    }

    public override void UpdatePhysicalState()
    {
        //throw new System.NotImplementedException();
    }
}
