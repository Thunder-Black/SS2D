using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour {
    //Collider2D triggerObjColl;
	// Use this for initialization
    GameObject door;
    public Collider2D coll;
	void Start () {
        //triggerObjColl = GameObject.Find("TriggerObj").GetComponent<Collider2D>();
        door = GameObject.Find("Door");
	}
	
	// Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            coll = other;
            door.SetActive(false);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            door.SetActive(true);
            coll = null;
        }
    }

    void FixedUpdate()
    {
        
    }
}
