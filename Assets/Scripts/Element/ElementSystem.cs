using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
public enum ElementType
{
    Fire,
    Water,
    Wind,
    Ice,
    Electric,
    None
}


public class ElementSystem : MonoBehaviour
{
    private Dictionary<ElementType, int> elementPoints = new();

    [SerializeField]
    private List<ElementType> availableElements = new();
    [SerializeField]
    private ElementUIManager elementUIManager;
    public event Action OnElementChanged;
    public event Action<float> OnCooldownStarted;

    private ElementType currentElement = ElementType.None;
    public ElementType CurrentElement => currentElement;

    public List<ElementType> AvailableElements => availableElements;

    private float switchCooldown = 2f;
    private float currentCooldownTime = 0f;
    public bool IsOnCooldown => currentCooldownTime > 0f;
    public float CooldownDuration => switchCooldown;
    public float CooldownRemaining => Mathf.Clamp(currentCooldownTime, 0, switchCooldown);

    private void Update()
    {
        if (IsOnCooldown)
        {
            currentCooldownTime -= Time.deltaTime;
            if (currentCooldownTime <= 0f)
            {
                currentCooldownTime = 0f;
                
            }
        }
    }
    private void Awake()
    {
        foreach (ElementType type in Enum.GetValues(typeof(ElementType)))
        {
            if (type != ElementType.None)
                elementPoints[type] = 0;
        }

        UpdateAvailableElements();
    }


    public void AddElementPoint(ElementType type, int amount)
    {
        if (!elementPoints.ContainsKey(type)) return;

        elementPoints[type] += amount;
        UpdateAvailableElements();
    }

    public void SetElementPoint(ElementType type, int value)
    {
        if (!elementPoints.ContainsKey(type)) return;

        elementPoints[type] = value;
        UpdateAvailableElements();
    }

    private void UpdateAvailableElements()
    {
        availableElements = elementPoints
            .Where(pair => pair.Value > 0)
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => (int)pair.Key) // 保证一致性
            .Take(2)
            .Select(pair => pair.Key)
            .ToList();

        if (availableElements.Count == 0)
        {
            currentElement = ElementType.None;
        }
        else if (!availableElements.Contains(currentElement))
        {
            currentElement = availableElements[0];
        }
        OnElementChanged?.Invoke(); 

    }

    public void SwitchElement()
    {
        if (availableElements.Count < 2) return;
        if (IsOnCooldown) return;

        currentElement = (currentElement == availableElements[0])
            ? availableElements[1]
            : availableElements[0];

        currentCooldownTime = switchCooldown; 
        OnElementChanged?.Invoke();           
        OnCooldownStarted?.Invoke(switchCooldown);          


    }


    public DamageType GetCurrentDamageType()
    {
        return currentElement switch
        {
            ElementType.Fire => DamageType.Fire,
            ElementType.Ice => DamageType.Ice,
            //ElementType.Wind => DamageType.Wind,
            ElementType.Electric => DamageType.Electric,
            //ElementType.Water => DamageType.Water, 
            _ => DamageType.Physical
        };
    }

}

