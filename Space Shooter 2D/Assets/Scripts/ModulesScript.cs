using UnityEngine;
using System.Collections;

public class ModulesScript : MonoBehaviour
{
    public GameObject[] weaponsObj;
    public Transform[] modulesPositions;
    private AbstractWeapon[] weapons;
    // Use this for initialization
    void Instantinate()
    {
        weapons = new AbstractWeapon[weaponsObj.Length];
        for (int i = 0; i < weaponsObj.Length; i++)
        {
            if (i < modulesPositions.Length && weaponsObj[i] != null)
            {
                weaponsObj[i] = Instantiate(weaponsObj[i], modulesPositions[i].transform.position, modulesPositions[i].transform.rotation) as GameObject;
                weaponsObj[i].transform.parent = gameObject.transform;
                weapons[i] = weaponsObj[i].GetComponent<AbstractWeapon>();
            }
        }
    }
    public AbstractWeapon[] ResetAllWeapons()
    {
        Instantinate();
        return weapons;
    }

    public GameObject InstantinateWeaponById(GameObject weapon, int i)
    {
        if (weaponsObj[i] != null)
        {
            Destroy(weaponsObj[i]);
            weaponsObj[i] = null;
            if (weapon != null)
            {
                weaponsObj[i] = Instantiate(weapon, modulesPositions[i].transform.position, modulesPositions[i].transform.rotation) as GameObject;
                weaponsObj[i].transform.parent = gameObject.transform;
                weapons[i] = weaponsObj[i].GetComponent<AbstractWeapon>();
                weapons[i].enabled = true;
                return weaponsObj[i];
            }
        }
        return null;
    }
}
