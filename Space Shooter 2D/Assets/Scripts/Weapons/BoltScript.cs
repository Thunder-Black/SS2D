using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoltScript : MonoBehaviour
{
    public int damage;
    public float speed;
    private Collider2D parentColl;
    private float lifetime = 0.75f;
    void Start()
    {
        if (damage == 0)
            damage = 5;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
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
            //Debug.Break();
            Destroy(this.gameObject);

            if (other.gameObject.tag == "Vehicle")
            {
                UnitController uC = other.gameObject.GetComponent<UnitController>();
                if (uC != null)
                    uC.DecreaseHitPoints(damage);
            }
        }
    }
}
