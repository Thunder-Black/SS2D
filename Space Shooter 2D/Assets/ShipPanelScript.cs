using UnityEngine;
using System.Collections;

public class ShipPanelScript : MonoBehaviour
{
    public GameObject slot;
    public GameObject[] savedSlots;
    // Use this for initialization
    void Start()
    {
        UnitController[] ships = FindObjectsOfType<UnitController>();
        for (int i = 0; i < ships.Length; i++)
            if (ships[i].playerControlled)
            {
                // Выбираем наш корабль
                GameObject ship = ships[i].gameObject;
                ModulesScript modScript = ship.GetComponent<ModulesScript>();

                savedSlots = new GameObject[modScript.weaponsObj.Length];
                for (int j = 0; j < modScript.weaponsObj.Length; j++)
                {
                    GameObject tempWeapon = modScript.weaponsObj[j];
                    GameObject tempSlot = Instantiate(slot);
                    savedSlots[j] = tempSlot;
                    tempSlot.transform.SetParent(this.transform);
                    tempSlot.GetComponent<SlotScript>().SetModule(tempWeapon);
                    tempSlot.GetComponent<SlotScript>().SetDeleteOnDrop();
                }
            }
    }
    public void Accept()
    {
        UnitController[] ships = FindObjectsOfType<UnitController>();
        for (int i = 0; i < ships.Length; i++)
            if (ships[i].playerControlled)
            {
                GameObject ship = ships[i].gameObject;
                ModulesScript modScript = ship.GetComponent<ModulesScript>();

                for (int j = 0; j < savedSlots.Length; j++)
                {
                    if (savedSlots[j].GetComponentInChildren<DragHandler>() != null)
                        modScript.InstantinateWeaponById(savedSlots[j].GetComponentInChildren<DragHandler>().weapon, j);
                    else modScript.InstantinateWeaponById(null, j);
                }
            }
    }
}
