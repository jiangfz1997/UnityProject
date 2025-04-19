using UnityEngine;

[System.Serializable]
public class PlayerEquipmentSaveData
{
    public int equippedNecklaceId;
    public string equippedNecklaceAbilityUid;
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

    public void EquipNecklace(NecklaceSO necklaceData, string UID=null)
    {
        if (equippedNecklace != null)
        {
            Debug.Log("Already have one necklace!!");
            return;
        }
        equippedNecklaceSO = necklaceData;
        
        equippedNecklace = necklaceData.CreateRuntimeInstance(UID);


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
        if (equippedNecklace == null || equippedNecklaceSO == null)
        {
            return new PlayerEquipmentSaveData
            {
                equippedNecklaceId = -1,
                equippedNecklaceAbilityUid = null
            };
        }

        return new PlayerEquipmentSaveData
        {
            equippedNecklaceId = equippedNecklaceSO.id,
            equippedNecklaceAbilityUid = equippedNecklace.GetUID()
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
                string restoredUid = data.equippedNecklaceAbilityUid;

                EquipNecklace(found, restoredUid);
            }
            else
            {
                Debug.LogWarning($"Cannot find necklace with ID: {data.equippedNecklaceId}");
            }
        });
    }
}

