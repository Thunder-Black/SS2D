using UnityEngine;
using System.Collections;

public class ChangeTextureOnCollided : MonoBehaviour
{

    public Material newMat;
    Material[] tempMat;
    public Renderer[] objRender;
    // Use this for initialization
    void Start()
    {
        tempMat = new Material[objRender.Length];
        for (int i = 0; i < objRender.Length; i++)
            tempMat[i] = objRender[i].material;
    }
    // Update is called once per frame
    public void SetMaterial()
    {
        for (int i = 0; i < objRender.Length; i++)
            objRender[i].sharedMaterial = newMat;
    }
    public void UnsetMaterial()
    {
        for (int i = 0; i < objRender.Length; i++)
            objRender[i].sharedMaterial = tempMat[i];
    }
}
