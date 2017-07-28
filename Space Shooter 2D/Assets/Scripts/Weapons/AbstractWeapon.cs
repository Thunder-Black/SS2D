using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.Collections;

public abstract class AbstractWeapon : MonoBehaviour
{
    public float SPM;
    public int damage;
    public Sprite iconSprite;
    protected bool cooldown;
    private float SPM_Timer = 0f;
    // Use this for initialization
    protected virtual void Start()
    {
    }

    public virtual void Shoot()
    {
    }

    void CoolDown()
    {
        // SPM - кол-во выстрелов в минуту; 60/SPM - периодичночть одного выстрела
        if (SPM_Timer < (60 / SPM))
        {
            SPM_Timer += Time.deltaTime;
            //Debug.Log("Reloading");
        }
        // Как только произошло кд - таймер на 0
        else { SPM_Timer = 0; cooldown = false; }
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown) CoolDown();
    }
}
