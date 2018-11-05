using UnityEngine;

public class LA_Chaser : LA_Antagonist
{
    public override bool IsKillable { get { return true; } }
    public override bool IsShieldable { get { return false; } }
    const int DetectionRange = 2;
    Vector2 DetectionLocation = -Vector2.one;
    bool Triggered;
    Vector2 LastPlayerPosition;
    Vector2 NextLastPlayerPosition;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        DetectionLocation = GridTransform.Position + LodestarTransform.GetDeltaPosition(GridTransform.Rotation.Current) * DetectionRange;
    }

    protected void BecomeTriggered()
    {
        Triggered = true;
    }

    public override void BeginNewState()
    {
        if (!Triggered)
        {
            //check for player
            if (LA_Player.instance.GetGridTransform().Position == DetectionLocation)
            {
                bool pathExists = true;
                LodestarTransform currTransform = this.GridTransform;

                for (int i = 0; i < DetectionRange; i++) {
                    LodestarTransform nextTransform = LodestarMap.instance.GetConnectedNeighborTransform(currTransform, currTransform.Rotation.Current);
                    if(nextTransform != null) {
                        currTransform = nextTransform;
                    }
                    else {
                        pathExists = false;
                    }
                }

                if(pathExists) {
                    BecomeTriggered();
                    LastPlayerPosition = LA_Player.instance.GetGridTransform().Position;
                }
            }
        }
        Kill();

    }

    public override void EndLastState()
    {
        if (IsKilled) {
            //LodestarMap.instance.BecomeTerrifier(this);
            Debug.Log("he gon die");
        }
        else if (Triggered)
        {
            Move();
        }
    }

    public override void DoNewStateChecks()
    {
    }

    public override void Move()
    {
        #region Moves chaser to next position
        LodestarNode oldNode = LodestarMap.instance.GetNodeFromLayout(this.GridTransform);
        oldNode.OccupyingActors.Remove(this);

        GridTransform.Advance();
        #endregion

        this.Rotate();

        LodestarNode newNode = LodestarMap.instance.GetNodeFromLayout(this.GridTransform);
        newNode.OccupyingActors.Add(this);

       // LastPlayerPosition = NextLastPlayerPosition;
       LastPlayerPosition = LA_Player.instance.GetGridTransform().Position;

    }

    public override void Rotate()
    {
        #region Handles rotating the chaser in new position
        // Updates position variable to get the next position in front of child
        LodestarTransform _NextGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        // TODO: CLEAN UP AFTER REFACTORING L_DIRECTION
        GridTransform.TurnRight();
        LodestarTransform _RightGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnAround();
        LodestarTransform _LeftGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnRight();

        // Checks to see if there's a child at the next position
      
        bool NextIsChild = _NextGridTransform == null ? false : _NextGridTransform.Position == LastPlayerPosition;
        // Checks to see if there's a child at left or right position
        bool RightIsChild = _RightGridTransform == null ? false : _RightGridTransform.Position == LastPlayerPosition;
        bool LeftIsChild = _LeftGridTransform == null ? false : _LeftGridTransform.Position == LastPlayerPosition;

        if (NextIsChild) {
            
        }
        else if (RightIsChild) {
            GridTransform.TurnRight();
        }
        else if (LeftIsChild)
        {
            GridTransform.TurnLeft();
        }
        else if (_NextGridTransform == null) {
            // Block handles possible scenarios if right or left null or not
            if (_RightGridTransform == null)
            {
                if (_LeftGridTransform == null)
                {
                    // No path on all sides
                    GridTransform.TurnAround();
                }
                else
                {
                    // Only path on left
                    GridTransform.TurnLeft();
                }
            }
            else if (_LeftGridTransform == null)
            {
                // Only path on right
                GridTransform.TurnRight();
            }
            else
            {
                // Path on left and right
                GridTransform.TurnAround();
            }
        }
        #endregion
    }

    public override void Kill()
    {
        if (GridTransform.Position == LA_Player.instance.GetGridTransform().Position) {
            LA_Player.instance.SetIsKilled(true);
        }
    }

    public override void UpdatePhysicalState()
    {
        UpdateOwnerTransform();
    }
}
