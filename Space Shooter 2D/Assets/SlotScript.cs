using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotScript : MonoBehaviour
{
    bool deleteOnDrop = false;
    // Use this for initialization
    void Start()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
    public void SetModule(GameObject weapon)
    {
        if (weapon != null)
        {
            AbstractWeapon awScript = weapon.GetComponent<AbstractWeapon>();
            if (awScript != null)
            {
                this.transform.GetChild(0).GetComponent<Image>().sprite = awScript.iconSprite;
                this.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
                this.transform.GetChild(0).GetComponent<DragHandler>().weapon = weapon;
            }
        }
    }
    public void SetDeleteOnDrop()
    {
        deleteOnDrop = true;
    }
    public bool GetDeleteOnDrop()
    {
        return deleteOnDrop;
    }

}
