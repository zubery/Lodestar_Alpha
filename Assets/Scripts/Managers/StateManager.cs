using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class StateManager : MonoBehaviour

{

    public static StateManager instance = null;

    private void Awake()
    {

        if (instance) { Destroy(this); }

        else { instance = this; }
    }

    public void EndLastState()
    {
        LA_Player.instance.EndLastState();
        foreach (LodestarActor _actor in MapGenerator.instance.GeneratedActorScripts)
            if (_actor && _actor != LA_Player.instance)
                _actor.EndLastState();
    }
    public void BeginNewState()
    {
        LA_Player.instance.BeginNewState();
        foreach (LodestarActor _actor in MapGenerator.instance.GeneratedActorScripts)
            if (_actor && _actor != LA_Player.instance)
                _actor.BeginNewState();
    }
    public void DoNewStateChecks() {
        LA_Player.instance.DoNewStateChecks();
        foreach (LodestarActor _actor in MapGenerator.instance.GeneratedActorScripts)
            if (_actor && _actor != LA_Player.instance)
                _actor.DoNewStateChecks();
    }
    public void UpdatePhysicalState()
    {
        LA_Player.instance.UpdatePhysicalState();
        foreach (LodestarActor _actor in MapGenerator.instance.GeneratedActorScripts)
            if (_actor && _actor != LA_Player.instance)
                _actor.UpdatePhysicalState();
    }
    


    public void AdvanceState()
    {
        if (MapGenerator.instance.IsMapGenerationSuccess) {
            EndLastState();
            BeginNewState();
            DoNewStateChecks();
            UpdatePhysicalState();
            ++AdvancesPerLevel.Advances;
            ++AdvancesToBeatLevelMetric.Advances;
        }
    }
}

