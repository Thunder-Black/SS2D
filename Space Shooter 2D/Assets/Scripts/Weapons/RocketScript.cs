using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RocketScript : AbstractWeapon
{
    public GameObject shot;
    private Collider2D parentCollider;

    // Use this for initialization
    protected override void Start()
    {
        MissileScript bs = shot.GetComponent<MissileScript>();
        bs.damage = damage;
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

            GameObject missile = Instantiate(shot, shotSpawnPos, shotRotation) as GameObject;
            missile.GetComponent<MissileScript>().SetParentCollider(parentCollider);

            cooldown = true;
        }
    }
}

