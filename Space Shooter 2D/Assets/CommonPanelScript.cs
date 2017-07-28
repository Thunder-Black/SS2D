using UnityEngine;
using System.Collections;

public class CommonPanelScript : MonoBehaviour
{
    public GameObject slot;
    // Use this for initialization
    void Start()
    {
        var weapons = Resources.LoadAll("Weapons", typeof(GameObject));
        foreach (var weapon in weapons)
        {
            GameObject tempWeapon = weapon as GameObject;
            if (tempWeapon.GetComponent<AbstractWeapon>() != null)
            {
                GameObject tempSlot = Instantiate(slot);
                tempSlot.transform.SetParent(this.transform);
                tempSlot.GetComponent<SlotScript>().SetModule(tempWeapon);
            }
        }
    }
}
