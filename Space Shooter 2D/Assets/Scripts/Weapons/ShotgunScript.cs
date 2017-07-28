using UnityEngine;
using System.Collections;

public class ShotgunScript : AbstractWeapon
{

    public GameObject shot;
    public int shotLines;
    public int scatterAngle;
    private Collider2D parentCollider;

    // Use this for initialization
    protected override void Start()
    {
        BoltScript bs = shot.GetComponent<BoltScript>();
        bs.damage = damage;
        parentCollider = GetComponentInParent<Collider2D>();
    }

    public override void Shoot()
    {
        // Как только нажали - стреляем
        if (!cooldown)
        {
            Quaternion shotRotation;
            for (int i = 0; i < shotLines; i++)
            {
                float angle = Random.Range(-scatterAngle / 2, scatterAngle / 2);
                shotRotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + angle);
                Vector3 shotSpawnPos = gameObject.transform.position;

                GameObject bolt = Instantiate(shot, shotSpawnPos, shotRotation) as GameObject;
                bolt.GetComponent<BoltScript>().SetParentCollider(parentCollider);
            }
            cooldown = true;
        }
    }
}
