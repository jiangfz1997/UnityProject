using UnityEngine;


public class PlayerEquipment : MonoBehaviour
{

    public NecklaceSO defaultNecklace;
    private Necklace equippedNecklace;
    private NecklaceSO equippedNecklaceSO;
    private Player player;
    public NecklaceUI necklaceUI;


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        if (defaultNecklace != null)
        {
            EquipNecklace(defaultNecklace);
        }
        necklaceUI.UpdateUI(equippedNecklaceSO);
    }
    public Necklace GetEquippedNecklace()
    {
        return equippedNecklace;
    }

    public void EquipNecklace(NecklaceSO necklaceData)
    {
        if (equippedNecklace != null)
        {
            //equippedNecklace.Deactivate(player);
            Debug.Log("Already have one necklace!!");
            return;
        }
        equippedNecklaceSO = necklaceData;

        equippedNecklace = necklaceData.CreateRuntimeInstance();
        equippedNecklace.Activate(player);
        necklaceUI?.UpdateUI(necklaceData);
        Debug.Log($"Equipped Necklace: {necklaceData.displayName}");
    }

    public void UnequipNecklace()
    {
        if (equippedNecklace != null)
        {
            equippedNecklace.Deactivate(player);
            equippedNecklace = null;
            equippedNecklaceSO = null;
            Debug.Log("Unequipped Necklace");
            necklaceUI?.UpdateUI(null);

        }
    }

    public bool HasNecklaceEquipped()
    {
        return equippedNecklace != null;
    }
}

