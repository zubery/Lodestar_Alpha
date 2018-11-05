using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class AdvanceButtonBehaviour : MonoBehaviour
{

    Button AdvanceButton;
    public static AdvanceButtonBehaviour instance;
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        AdvanceButton = GetComponent<Button>();
        AdvanceButton.onClick.AddListener(Advance);
    }

    void Advance()
    {
        if (!ShieldingManager.instance.IsInSelectionMode)
            StateManager.instance.AdvanceState();
    }
}
