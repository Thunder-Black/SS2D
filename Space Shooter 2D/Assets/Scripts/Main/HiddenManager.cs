using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenManager : MonoBehaviour
{
    //Collider2D triggerObjColl;
    // Use this for initialization
    private ChangeTextureOnCollided chTex;
    public HideScript openObj;
    public Collider2D coll;
    void Start()
    {
        chTex = this.GetComponent<ChangeTextureOnCollided>();
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            coll = other;
            openObj.Hide(true);
            chTex.SetMaterial();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            openObj.Hide(false);
            coll = null;
            chTex.UnsetMaterial();
        }
    }

    void FixedUpdate()
    {

    }
}
