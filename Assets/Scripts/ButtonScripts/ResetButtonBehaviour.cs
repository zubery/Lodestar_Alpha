using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetButtonBehaviour : MonoBehaviour
{
    Button AdvanceButton;
    public static ResetButtonBehaviour instance;
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        AdvanceButton = GetComponent<Button>();
        AdvanceButton.onClick.AddListener(ResetLevel);
    }

    void ResetLevel()
    {

        GameManager.instance.ResetLevel();
    }
}