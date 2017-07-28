using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EMIScript : MonoBehaviour
{
    public GameObject explosion;
    private Collider2D parentColl;
    private float timer = 0f;
    bool block = false;
    bool blockOff = false;
    List<UnitController> UcS;
    // Use this for initialization
    void Start()
    {
        UcS = new List<UnitController>();
        GameObject expl = Instantiate(explosion, this.transform.position, this.transform.rotation) as GameObject;
        expl.transform.parent = this.transform;
    }
    public void SetParentCollider(Collider2D coll)
    {
        parentColl = coll;
    }

    void CoolDown()
    {
        timer += Time.deltaTime;
        if (!blockOff & timer > 1f)
        {
            blockOff = true;
            OffAllEngine();
        }
        if (timer > 2f)
        {
            block = true;
            OnAllEngine();
        }
    }

    void OffAllEngine()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 3); // 200 - радиус взрыва
        for (int i = 0; i < hitColliders.Length; i++)
            if (hitColliders[i].gameObject.tag == "Vehicle" && hitColliders[i] != parentColl)
            {
                UnitController uC = hitColliders[i].gameObject.GetComponent<UnitController>();
                UcS.Add(uC);
                if (uC != null)
                    uC.OffEngine();
                
            }
    }

    void OnAllEngine()
    {
        for (int i = 0; i < UcS.Count; i++)
            UcS[i].OnEngine();
    }

    void Update()
    {
        if (!block) CoolDown();
    }
}
