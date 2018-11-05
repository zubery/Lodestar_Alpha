using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShieldButtonBehaviour : MonoBehaviour
{
    Button ShieldButton;
    [SerializeField] GameObject ShieldImage;
    [SerializeField] GameObject UnshieldImage;

    public static ShieldButtonBehaviour instance;
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        ShieldButton = GetComponent<Button>();
        ShieldButton.onClick.AddListener(ToggleShieldSelection);
        SetButtonToShield();
    }

    void ToggleShieldSelection()
    {
        if (!ShieldingManager.instance.IsShieldUsed)
        {
            ShieldingManager.instance.StartIndicatingShieldableActors();
        }
        else
        {
            ShieldingManager.instance.UnshieldShieldedActor();
        }
    }

    public void SetButtonToShield()
    {        ShieldImage.SetActive(true);
        UnshieldImage.SetActive(false);
    }

    public void SetButtonToUnshield()
    {
        ShieldImage.SetActive(false);
        UnshieldImage.SetActive(true);
    }
}
