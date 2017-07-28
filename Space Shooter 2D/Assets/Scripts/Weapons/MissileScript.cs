using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MissileScript : MonoBehaviour
{
    public int damage;
    public float startSpeed;
    public float growSpeed;
    public GameObject explosion;
    private Collider2D parentColl;

    private float lifetime = 4;
    Rigidbody2D rb;
    void Start()
    {
        if (damage == 0)
            damage = 45;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * startSpeed;
        Destroy(this.gameObject, lifetime);
    }
    public void SetParentCollider(Collider2D coll)
    {
        parentColl = coll;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.tag != "Bolt" && (parentColl == null || other.gameObject != parentColl.gameObject))
        {
            Destroy(this.gameObject);

            Instantiate(explosion, this.transform.position, this.transform.rotation);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 3); // 200 - радиус взрыва
            for (int i = 0; i < hitColliders.Length; i++)
                if (hitColliders[i].gameObject.tag == "Vehicle")
                {
                    UnitController uC = hitColliders[i].gameObject.GetComponent<UnitController>();
                    if (uC != null)
                        uC.DecreaseHitPoints(damage);
                }
        }
    }
    void Update()
    {
        rb.AddRelativeForce(new Vector2(0, 1) * growSpeed * Time.deltaTime);
    }
}

