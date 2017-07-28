using UnityEngine;
using System.Collections;

public class ModulesRoomManager : MonoBehaviour
{
    SceneManagerScript sms;
    // Use this for initialization
    void Start()
    {
        //ShipSet();
    }

    void ShipSet()
    {
        sms = FindObjectOfType<SceneManagerScript>();
        if (sms != null)
        {
            sms.tempObject.SetActive(false);
            sms.tempObject.transform.position = new Vector3(-1.5f, 0, 10);
            sms.tempObject.transform.eulerAngles = new Vector3(0, 0, 0);
            sms.tempObject.SetActive(true);
            sms.tempObject.GetComponent<UnitController>().OffEngine();
        }
    }
    public void DestroyAllShips()
    {
        if (sms != null && sms.tempObject != null)
            Destroy(sms.tempObject);
    }
}
