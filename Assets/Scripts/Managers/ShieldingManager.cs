using UnityEngine;



public class ShieldingManager : MonoBehaviour

{
    [HideInInspector]
    public bool IsInSelectionMode = false;
    [HideInInspector]

    public bool IsShieldUsed = false;

    [HideInInspector]

    public LodestarActor ShieldedActor = null;



    public static ShieldingManager instance;

    private void Awake()

    {

        if (instance == null) instance = this;

        else Destroy(this);

    }



    public void StartIndicatingShieldableActors()

    {

        foreach (LodestarActor _actor in MapGenerator.instance.GeneratedActorScripts)

        {
            if(_actor)
                _actor.ActivateShieldableIndicator();

        }

        IsInSelectionMode = true;

    }



    public void StopIndicatingShieldableActors(){
        foreach (LodestarActor _actor in MapGenerator.instance.GeneratedActorScripts)

        {
            if (_actor)
                _actor.DeactivateShieldableIndicator();

        }

        IsInSelectionMode = false;

    }



    public void CancelShieldingSelection()

    {

        StopIndicatingShieldableActors();

        IsInSelectionMode = false;

    }



    public bool ShieldActor(LodestarActor _actorToShield)

    {

        if (!_actorToShield || !_actorToShield.IsShieldable)

            return false;

        _actorToShield.BecomeShielded();

        ShieldedActor = _actorToShield;

        StopIndicatingShieldableActors();

        IsShieldUsed = true;

        ShieldButtonBehaviour.instance.SetButtonToUnshield();

        LA_Player.instance.Rotate();
        LA_Player.instance.UpdatePhysicalState();
        return true;

    }



    public void UnshieldShieldedActor(){

        ShieldedActor.BecomeUnshielded();
        IsShieldUsed = false;
        ShieldButtonBehaviour.instance.SetButtonToShield();

        LA_Player.instance.Rotate();
        LA_Player.instance.UpdatePhysicalState();

        ShieldedActor = null;

    }

}

