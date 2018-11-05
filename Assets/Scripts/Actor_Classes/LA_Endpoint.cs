using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class LA_Endpoint : LodestarActor

{
    public override bool IsKillable { get { return false; } }

    public override bool IsShieldable { get { return false; } }

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

        if (LA_Player.instance.GetGridTransform().Position == GridTransform.Position)

        {

            GameManager.instance.GoToNextLevel();
        }
    }

    public override void DoNewStateChecks()
    {
        //throw new System.NotImplementedException();
    }

    public override void EndLastState()
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdatePhysicalState()
    {
       // throw new System.NotImplementedException();
    }
}

