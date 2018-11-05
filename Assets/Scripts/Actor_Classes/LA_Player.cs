using UnityEngine;

public class LA_Player : LodestarActor
{
    public override bool IsKillable { get { return true; } }
    public override bool IsShieldable { get { return false; } }
    public static LA_Player instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public override void BeginNewState()
    {
        //UpdateOwnerTransform();
    }

    public override void EndLastState()
    {
        Move();
    }

    public override void Move()
    {
        #region Moves child to next position
        // Gets current transform
        LodestarTransform _NextGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);

        // Quick check to make sure that it can advance
        //this.Rotate();
        //GridTransform.Advance();

        if (_NextGridTransform != null)
        {
            GridTransform.Advance();
            this.Rotate();
            this.CorrectRotate();
        }
        else
        {
            print("dead");
        }

        #endregion


    }

    public override void Rotate()
    {
        #region Handles rotating the child in new position
        // Updates position variable to get the next position in front of child
        LodestarTransform _NextGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        // TODO: CLEAN UP AFTER REFACTORING L_DIRECTION
        GridTransform.TurnRight();
        LodestarTransform _RightGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnAround();
        LodestarTransform _LeftGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        GridTransform.TurnRight();

        // Checks to see if there's a terrifier at the next position
        bool NextIsTerrifier = LodestarMap.instance.IsTerrifier(_NextGridTransform);
        // Checks to see if there's an attractor at left or right position
        bool NextIsAttractor = LodestarMap.instance.IsAttractor(_NextGridTransform);
        bool RightIsAttractor = LodestarMap.instance.IsAttractor(_RightGridTransform);
        bool LeftIsAttractor = LodestarMap.instance.IsAttractor(_LeftGridTransform);

        #region Checks hidden status
        // Checks to see if the terrifier is hidden
        bool NextTerrifierIsHidden = false;
        LodestarNode NextTerrifier = LodestarMap.instance.GetNodeFromLayout(_NextGridTransform);
        if (NextTerrifier)
            for (int i = 0; i < NextTerrifier.OccupyingActors.Count; i++)
            {
                if (NextTerrifier.OccupyingActors[i] is LA_Terrifier)
                {
                    if (NextTerrifier.OccupyingActors[i].GetIsShielded())
                    {
                        NextTerrifierIsHidden = true;
                    }
                }
            }

        // Checks to see if the front attractor is hidden
        bool NextAttractorIsHidden = false;
        LodestarNode NextAttractor = LodestarMap.instance.GetNodeFromLayout(_NextGridTransform);
        if (NextAttractor)
            for (int i = 0; i < NextAttractor.OccupyingActors.Count; i++)
            {
                if (NextAttractor.OccupyingActors[i] is LA_Attractor)
                {
                    if (NextAttractor.OccupyingActors[i].GetIsShielded())
                    {
                        NextAttractorIsHidden = true;
                    }
                }
            }

        // Checks to see if the right attractor is hidden
        bool RightAttractorIsHidden = false;
        LodestarNode RightAttractor = LodestarMap.instance.GetNodeFromLayout(_RightGridTransform);
        if (RightAttractor)
            for (int i = 0; i < RightAttractor.OccupyingActors.Count; i++)
            {
                if (RightAttractor.OccupyingActors[i] is LA_Attractor)
                {
                    if (RightAttractor.OccupyingActors[i].GetIsShielded())
                    {
                        RightAttractorIsHidden = true;
                    }
                }
            }

        //Checks to see if the left attractor is hidden
        bool LeftAttractorIsHidden = false;
        LodestarNode LeftAttractor = LodestarMap.instance.GetNodeFromLayout(_LeftGridTransform);
        if (LeftAttractor)
            for (int i = 0; i < LeftAttractor.OccupyingActors.Count; i++)
            {
                if (LeftAttractor.OccupyingActors[i] is LA_Attractor)
                {
                    if (LeftAttractor.OccupyingActors[i].GetIsShielded())
                    {
                        LeftAttractorIsHidden = true;
                    }
                }
            }
        #endregion

        // Fixes terrifier/attractor based on hidden-ness
        NextIsTerrifier = NextIsTerrifier && (!NextTerrifierIsHidden);
        NextIsAttractor = NextIsAttractor && (!NextAttractorIsHidden);
        RightIsAttractor = RightIsAttractor && (!RightAttractorIsHidden);
        LeftIsAttractor = LeftIsAttractor && (!LeftAttractorIsHidden);

        // Turns the child around if the next is a terrifier
        if (NextIsTerrifier)
        {
            GridTransform.TurnAround();

            if (LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current) == null)
            {
                if (_LeftGridTransform != null)
                {
                    GridTransform.TurnRight();
                }
                else if (_RightGridTransform != null)
                {
                    GridTransform.TurnLeft();
                }
            }

        }
        else if (NextIsAttractor)
        {

        }
        // Handles if there's right but no left attractor
        else if (RightIsAttractor && !LeftIsAttractor)
        {
            GridTransform.TurnRight();
        }
        // Handles if there's left but no right attractor
        else if (!RightIsAttractor && LeftIsAttractor)
        {
            GridTransform.TurnLeft();
        }
        // Handles if the next position is null, neither or both attractors
        else if (_NextGridTransform == null)
        {
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

    public void CorrectRotate()
    {
        LodestarTransform _NextGridTransform = LodestarMap.instance.GetConnectedNeighborTransform(GridTransform, GridTransform.Rotation.Current);
        bool NextIsTerrifier = LodestarMap.instance.IsTerrifier(_NextGridTransform);

        bool NextTerrifierIsHidden = false;
        LodestarNode NextTerrifier = LodestarMap.instance.GetNodeFromLayout(_NextGridTransform);
        if (NextTerrifier)
            for (int i = 0; i < NextTerrifier.OccupyingActors.Count; i++)
            {
                if (NextTerrifier.OccupyingActors[i] is LA_Terrifier)
                {
                    if (NextTerrifier.OccupyingActors[i].GetIsShielded())
                    {
                        NextTerrifierIsHidden = true;
                    }
                }
            }

        NextIsTerrifier = NextIsTerrifier && (!NextTerrifierIsHidden);

        if (NextIsTerrifier) {
            Rotate();
        }
    }


    public override void DoNewStateChecks()
    {
        if (IsKilled)
        {
            print("he ded");
            GameManager.instance.ResetLevel();
        }
    }


    public void RedoStateChecks()
    {
        DoNewStateChecks();
    }

    public override void UpdatePhysicalState()
    {
        UpdateOwnerTransform();
        //throw new System.NotImplementedException();
    }
}
