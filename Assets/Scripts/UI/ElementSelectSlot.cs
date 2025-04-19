using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelectSlot : MonoBehaviour
{
    public ElementType elementType;
    public TMP_Text introText;
    public Button selectButton;
    public void Init()
    {
        UpdateUI();
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => ElementSelectController.Instance.OnElementChosen(elementType));
        ElementSelectController.Instance.OnElementChosenEvent += UpdateUI;

    }

    public void UpdateUI()
    {
        int elementPoint = Player.Instance.GetElementPoint(elementType);
        introText.text = ElementSelectController.Instance.GetElementIntro(elementType, elementPoint);
    }


    private void OnDestroy()
    {
        if (ElementSelectController.Instance != null)
            ElementSelectController.Instance.OnElementChosenEvent -= UpdateUI;
    }
}
