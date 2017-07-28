using UnityEngine;
using System.Collections;

public class EMIWeaponScript : AbstractWeapon
{
    public GameObject shot;
    private Collider2D parentCollider;

    protected override void Start()
    {
        parentCollider = GetComponentInParent<Collider2D>();
    }

    public override void Shoot()
    {
        // Как только нажали - стреляем
        if (!cooldown)
        {
            Quaternion shotRotation;
            shotRotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z);
            Vector3 shotSpawnPos = gameObject.transform.position;

            GameObject EMI = Instantiate(shot, shotSpawnPos, shotRotation) as GameObject;
            EMI.transform.parent = gameObject.transform;
            EMI.GetComponent<EMIScript>().SetParentCollider(parentCollider);

            cooldown = true;
        }
    }
}
