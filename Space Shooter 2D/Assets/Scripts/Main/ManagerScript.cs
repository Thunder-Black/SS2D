using UnityEngine;
using System.Collections;
using System;

public class ManagerScript : MonoBehaviour
{
    public int timeMultipler;
    public int counter = 0;
    private int objCount;

    public UnitController[] uCs;
    public CameraController cC;
    private GameObject[] units;
    private int ucNumber;
    private KeyCode[] numericKeyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
         KeyCode.Alpha0,
     };

    // Use this for initialization
    void Start()
    {
        objCount = GameObject.FindGameObjectsWithTag("Collectable").Length;

        units = GameObject.FindGameObjectsWithTag("Vehicle");
        uCs = new UnitController[units.Length];
        for (int i = 0; i < units.Length; i++)
        {
            uCs[i] = units[i].GetComponent<UnitController>();
            uCs[i].ValueChanged += Data_ValueChanged;
        }
        SetNextUC(0);
    }

    void SetNextUC(int number)
    {
        if (units.Length > number)
        {
            uCs[ucNumber].ResetPlayerControlled();
            ucNumber = number;
            cC.UC = uCs[ucNumber];
            uCs[ucNumber].SetPlayerControlled();
        }
    }
    void Data_ValueChanged(object sender, EventArgs e)
    {
        counter++;
        if (counter >= objCount)
            Debug.Log("You win!!!");
    }
    void PlayerController()
    {
        uCs[ucNumber].MoveForwBack(Input.GetAxis("Vertical"));
        uCs[ucNumber].Rotation(Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.LeftControl))
            uCs[ucNumber].Shoot();

        // Таймер
        if (Input.GetKeyDown(KeyCode.C))
            uCs[ucNumber].StartTimer();

        if (Input.GetKey(KeyCode.C))
            uCs[ucNumber].TickTimer();

        if (Input.GetKeyUp(KeyCode.C))
            uCs[ucNumber].StopTimer();
        // ------
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Time.timeScale = timeMultipler;
        // Если AI не включен -> Игрок
        if (uCs[ucNumber] != null && !uCs[ucNumber].isAI)
            PlayerController();


        for (int i = 0; i < numericKeyCodes.Length; i++)
            if (Input.GetKeyDown(numericKeyCodes[i]))
            {
                SetNextUC(i);
            }
    }
}
