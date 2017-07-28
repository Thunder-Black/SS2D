using UnityEngine;
using System.Collections;

public class ModulesPlaceScript : MonoBehaviour
{

    ButtonScript buttonScript;
    public GameObject button;
    // Use this for initialization
    void Start()
    {
        button = Instantiate(button);
        buttonScript = button.GetComponent<ButtonScript>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        UnitController uc = other.gameObject.GetComponent<UnitController>();
        if (uc != null)
            if (buttonScript != null && uc.playerControlled)
                buttonScript.Show();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        UnitController uc = other.gameObject.GetComponent<UnitController>();
        if (uc != null)
            if (buttonScript != null && uc.playerControlled)
                buttonScript.Hide();
    }
}
