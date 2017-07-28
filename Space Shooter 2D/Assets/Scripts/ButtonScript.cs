using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    Canvas canvas;
    // Use this for initialization
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        Hide();
    }
    public void SetScene(string scene)
    {
        LoadOnClick loc = GetComponentInChildren<LoadOnClick>();
        loc.SetScene(scene);
    }
    public void Show()
    {
        canvas.gameObject.SetActive(true);
    }
    public void Hide()
    {
        canvas.gameObject.SetActive(false);
    }
}
