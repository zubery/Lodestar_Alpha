
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (ShieldingManager.instance.IsInSelectionMode &&
                !ShieldingManager.instance.IsShieldUsed)
            {
                if (GetActorUnderMouse(out LodestarActor _selectedActor))
                {
                    ShieldingManager.instance.ShieldActor(_selectedActor);
                }
                else
                    ShieldingManager.instance.CancelShieldingSelection();
            }
        }
    }

    bool GetActorUnderMouse(out LodestarActor _hitActor)
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _hitActor = Physics.Raycast(_ray, out RaycastHit _hit)
                           ? _hit.collider.gameObject.transform.parent.gameObject.GetComponent<LodestarActor>()
                           : null;
        return _hitActor;
    }


}
