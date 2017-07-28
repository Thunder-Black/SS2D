using UnityEngine;
using System.Collections;

public class ShowUIModulesScript : MonoBehaviour {
    public GameObject modulesUI;
	// Use this for initialization
    public void InstaniateUI()
    {
        Instantiate(modulesUI);
    }
}
