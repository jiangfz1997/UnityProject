using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ElementSelectController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ElementSelectController Instance;
    [SerializeField] private GameObject elementSelectUI;

    private Action<ElementType> onElementChosen;
    public List<ElementSelectSlot> slots;
    public event Action OnElementChosenEvent;
    private void Awake()
    {
        Instance = this;
    }
    public void OnClickFire() => OnElementChosen(ElementType.Fire);
    public void OnClickIce() => OnElementChosen(ElementType.Ice);
    public void OnClickLightning() => OnElementChosen(ElementType.Lightning);

    public void OpenElementSelect()
    {
        elementSelectUI.SetActive(true);
        foreach (var slot in slots)
            slot.Init(); 
    }


    public void OnElementChosen(ElementType element) 
    { 
        Player.Instance.AddElementPoint(element, 1);
        elementSelectUI.SetActive(false);
        OnElementChosenEvent?.Invoke();
    }

    public string GetElementIntro(ElementType type, int level)
    {
        int nextLevel = level + 1;

        switch (type)
        {
            case ElementType.Fire:
                int fireNow = 110 + level * 5;
                int fireNext = 110 + nextLevel * 5;
                return "[Fire] Burn\n" +
                       "Burn enemies for 3s.\n" +
                       "- ATK: 110% base\n" +
                       "- +5% / level\n\n" +
                       $"Lv.{level} ¡ú Lv.{nextLevel}   ATK: {fireNow}% ¡ú {fireNext}%";

            case ElementType.Ice:
                int iceNow = 20 + level * 5;
                int iceNext = 20 + nextLevel * 5;
                return "[Ice] Freeze\n" +
                       "Slows for 2s on hit.\n" +
                       "- Slow: 20% base\n" +
                       "- +5% / level\n\n" +
                       $"Lv.{level} ¡ú Lv.{nextLevel}   Slow: {iceNow}% ¡ú {iceNext}%";

            case ElementType.Lightning:
                int shockNow = 10 + level * 4;
                int shockNext = 10 + nextLevel * 4;
                return "[Lightning] Shock\n" +
                       "Chance to Paralyze.\n" +
                       "- Chance: 10% base\n" +
                       "- +4% / level\n\n" +
                       $"Lv.{level} ¡ú Lv.{nextLevel}   Chance: {shockNow}% ¡ú {shockNext}%";

            default:
                return "Unknown Element";
        }
    }

}
