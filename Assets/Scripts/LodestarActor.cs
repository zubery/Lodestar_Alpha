using UnityEngine;


public abstract class LodestarActor : MonoBehaviour
{
    protected LodestarTransform GridTransform;
    protected bool IsKilled = false;
    public abstract bool IsKillable { get; }
    public abstract bool IsShieldable { get; }
    public bool IsShielded { get; set; }
    [SerializeField]
    protected Material ShieldedMat;
    [SerializeField]
    protected Material OriginalMat;

    protected ShieldIndicatorBehaviour ShieldIndicator;
    protected GameObject ShieldIndicatorGO;

    protected virtual void Awake()
    {
        if (IsShieldable){
            ShieldIndicator = GetComponentInChildren<ShieldIndicatorBehaviour>();
            if (!ShieldIndicator)
            {
                Debug.LogError("No shield indicator sprite on shiedable prefab instance: " + name, this);
                return;
            }
            ShieldIndicatorGO = ShieldIndicator.gameObject;
            ShieldIndicator.Deactivate();
            ShieldIndicatorGO.SetActive(false);
        }

    }

    public LodestarTransform GetGridTransform()
    {
        return GridTransform;
    }

    public void SetGridTransform(LodestarTransform _GridTransform)
    {
        GridTransform = _GridTransform;
    }

    public bool GetIsShielded()
    {
        return IsShielded;
    }

    public void SetIsKilled(bool _IsKilled)
    {
        IsKilled = _IsKilled;
    }

    public Vector2 GetPosition()
    {
        return GridTransform.Position;
    }



    abstract public void Move();
    abstract public void Rotate();
    public void ActivateShieldableIndicator()
    {
        if (!IsShieldable) return;
        ShieldIndicator.Activate();
        ShieldIndicatorGO.SetActive(true);
    }

    public void DeactivateShieldableIndicator()
    {
        if (!IsShieldable) return;
        ShieldIndicator.Deactivate();
        ShieldIndicatorGO.SetActive(false);
    }

    public void BecomeShielded()
    {
        IsShielded = true;
        foreach (MeshRenderer mrend in GetComponentsInChildren<MeshRenderer>())
        {
            mrend.material = ShieldedMat;
        }
    }

    public void BecomeUnshielded()
    {
        IsShielded = false;
        foreach (MeshRenderer mrend in GetComponentsInChildren<MeshRenderer>())
        {
            mrend.material = OriginalMat;
        }
    }

    protected Vector3 GetScreenPos(Vector3 _WSOffset)
    {

        return CameraManager.instance.MainCam.WorldToScreenPoint(transform.position + _WSOffset);
    }

    protected void UpdateOwnerTransform()
    {
        gameObject.transform.position = GridTransform.GetWorldSpacePosition();
        gameObject.transform.rotation = GridTransform.GetRotationAsQuaternion();
    }

    public abstract void EndLastState();
    public abstract void BeginNewState();
    public abstract void DoNewStateChecks();
    public abstract void UpdatePhysicalState();
}
