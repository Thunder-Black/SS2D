using UnityEngine;
using System.Collections;

public class HideScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public void Hide(bool isHide)
    {
        this.gameObject.SetActive(!isHide);
    }
}
