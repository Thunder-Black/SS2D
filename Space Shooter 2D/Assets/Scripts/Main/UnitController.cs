using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Fraction
{
    Academy,
    Renegade
}

public class UnitController : MonoBehaviour
{
    public float hitPoint;

    public float velocity;
    public float rotatingSpeed;
    public float enginePower;
    public Fraction fraction;

    public bool isAI { get; private set; }
    public bool playerControlled { get; private set; }
    private float timer = 0f;
    private bool blockTimer;
    private bool engine;
    private List<Transform> targetObj;
    private ParticleSystem ps;
    private Rigidbody2D rb;
    private AbstractWeapon[] weapons;

    public EventHandler ValueChanged = delegate { };
    private int _collCounter;
    public int CollCounter
    {
        get { return _collCounter; }
        set
        {
            _collCounter = value;
            ValueChanged(null, EventArgs.Empty);
        }
    }

    // Use this for initialization
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();

        ModulesScript modulesScript = GetComponent<ModulesScript>();
        if (modulesScript != null)
            weapons = modulesScript.ResetAllWeapons();


        rb = GetComponent<Rigidbody2D>();
        targetObj = GetComponent<FOVScript>().visibleTargets;


        // Если переключать между AI и игроком, то вынести в Update
        // Если AI component есть и включен
        AI ai = GetComponent<AI>();
        if (ai != null && ai.enabled == true)
            isAI = true;
    }

    public void MoveForwBack(float forward)
    {
        if (!engine)
        {
            Vector2 movement = new Vector2(0.0f, forward * enginePower);
            rb.AddRelativeForce(movement);
        }
    }
    public void Rotation(float z)
    {
        if (!engine)
        {
            Vector3 rotate = new Vector3(0, 0, -z);
            rb.transform.Rotate(rotatingSpeed * rotate * Time.deltaTime);
        }
    }

    public void StartTimer()
    {
        blockTimer = false;
    }
    public void TickTimer()
    {
        if (!blockTimer && (targetObj.Count != 0))
            if (timer > 1.0f) // Время
            {
                // Объект собран
                for (int i = 0; i < targetObj.Count; i++)
                {
                    DestroyObject(targetObj[i].gameObject);
                    CollCounter++;
                    Debug.Log("Собран");
                }
                StopTimer();
            }
            else
                // Объект собирается
                timer += Time.deltaTime;
    }
    public void StopTimer()
    {
        blockTimer = true;
        Debug.Log(timer);
        timer = 0;
    }

    public void SetPlayerControlled()
    {
        if (!isAI)
            playerControlled = true;
    }
    public void ResetPlayerControlled()
    {
        playerControlled = false;
    }

    public void Shoot()
    {
        if (!engine && weapons != null && weapons.Length != 0)
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                    weapons[i].Shoot();
            }
        //else Debug.Log("No weapon");
    }
    public void OnEngine()
    {
        engine = false;
        ps.Play();
    }
    public void OffEngine()
    {
        engine = true;
        ps.Stop();
    }
    public void DecreaseHitPoints(float value)
    {
        hitPoint -= value;
        if (hitPoint <= 0)
        {
            Destroy(this.gameObject);

            Instantiate(Resources.Load("Explosions/explosion_player"), this.transform.position, this.transform.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity.sqrMagnitude;
    }
}
