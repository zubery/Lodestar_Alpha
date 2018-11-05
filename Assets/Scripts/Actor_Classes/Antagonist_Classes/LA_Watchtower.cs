using UnityEngine;

public class LA_Watchtower : LA_Antagonist
{
    public override bool IsKillable { get { return false; } }
    public override bool IsShieldable { get { return false; } }
    int KillRange = 1;
    protected override void Awake()
    {
        base.Awake();
    }


    public override void UpdatePhysicalState()
    {
        UpdateOwnerTransform();
    }

    public override void BeginNewState()
    {
        Move();
        Kill();
    }

    public override void DoNewStateChecks()
    {
        //Kill();
    }

    public override void EndLastState()
    {

    }

    public override void Move()
    {
        this.Rotate();
    }

    public override void Rotate()
    {
        #region Turns around the watchtower to the next available direction

        // Gets all the directions the watchtower can turn to, resets transform after
        GridTransform.TurnRight();
        /*
        #region Conditional Turning
        LodestarTransform QuarterRotation = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnRight();
        LodestarTransform HalfRotation = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnRight();
        LodestarTransform ThreeQuarterRotation = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnRight();

        // Turns to the next open spot
        if (QuarterRotation != null)
        {
            GridTransform.TurnRight();
        }
        else if (HalfRotation != null)
        {
            GridTransform.TurnAround();
        }
        else if (ThreeQuarterRotation != null)
        {
            GridTransform.TurnLeft();
        }
        #endregion
        */
        #endregion
    }

    public override void Kill()
    {
        //Vector2 KillSpot = GridTransform.Position;
        //if (GridTransform.Rotation.Current == LodestarDirection.Up) {
        //    KillSpot.y -= KillRange;
        //}
        //else if (GridTransform.Rotation.Current == LodestarDirection.Right) {
        //    KillSpot.x += KillRange;
        //}
        //else if(GridTransform.Rotation.Current == LodestarDirection.Down) {
        //    KillSpot.y += KillRange;
        //}
        //else if(GridTransform.Rotation.Current == LodestarDirection.Left) {
        //    KillSpot.x -= KillRange;
        //}
        Vector2 KillSpot = GridTransform.Position + LodestarTransform.GetDeltaPosition(GridTransform.Rotation.Current) * KillRange;
        if (KillSpot == LA_Player.instance.GetGridTransform().Position)
        {
            LA_Player.instance.SetIsKilled(true);
        }

        LodestarTransform chaserSpot = new LodestarTransform(KillSpot);

        //if (LodestarMap.instance.IsChaser(chaserSpot)) 
        //{
        //    LodestarMap.instance.IsChaser(chaserSpot).SetIsKilled(true);
        //    Debug.Log("killed chaser");
        //}
    }

}
