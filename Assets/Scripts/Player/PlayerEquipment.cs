using UnityEngine;

[System.Serializable]
public class PlayerEquipmentSaveData
{
    public int equippedNecklaceId;
}
public class PlayerEquipment : MonoBehaviour, ISaveable
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
        //if (defaultNecklace != null)
        //{
        //    EquipNecklace(defaultNecklace);
        //}
        //necklaceUI.UpdateUI(equippedNecklaceSO);
    }

    public void Initialize(Player player)
    {
        this.player = player;
        //isInitialized = true;

        if (defaultNecklace != null)
        {
            EquipNecklace(defaultNecklace);
        }

        necklaceUI?.UpdateUI(equippedNecklaceSO);
    }
    public Necklace GetEquippedNecklace()
    {
        return equippedNecklace;
    }

    public void EquipNecklace(NecklaceSO necklaceData)
    {
        if (equippedNecklace != null)
        {
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

    public string SaveKey() => "PlayerEquipment";

    public object CaptureState()
    {
        return new PlayerEquipmentSaveData
        {
            equippedNecklaceId = equippedNecklaceSO != null ? equippedNecklaceSO.id : -1
        };
    }

    public void RestoreState(object state)
    {
        var data = state as PlayerEquipmentSaveData;
        if (data == null || data.equippedNecklaceId == -1)
        {
            UnequipNecklace();
            return;
        }
        
        NecklaceFactory.GetNecklaceSOById(data.equippedNecklaceId, (found) =>
        {
            if (found != null)
            {
                UnequipNecklace();
                EquipNecklace(found);
            }
            else
            {
                Debug.LogWarning($"Cannot find necklace with ID: {data.equippedNecklaceId}");
            }
        });
    }
}

