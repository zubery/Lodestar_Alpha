using UnityEngine;

public class LA_Attractor : LodestarActor
{
    public override bool IsKillable { get { return false; }}
    public override bool IsShieldable { get { return true; } }

    private bool IsUsedUp = false;
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

    }

    public override void DoNewStateChecks()
    {
        IsUsedUp = GridTransform.Position == LA_Player.instance.GetPosition() && !IsShielded;
    }

    public override void EndLastState()
    {
       // throw new System.NotImplementedException();
    }

    public override void UpdatePhysicalState()
    {
        if (IsUsedUp)
        {
            Destroy(gameObject);
        }
    }
}
