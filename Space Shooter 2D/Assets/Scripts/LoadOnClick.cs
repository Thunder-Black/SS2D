using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    string scene;
    public void SetScene(string scene)
    {
        this.scene = scene;
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadSceneAdditive()
    {
        SceneManagerScript sms = FindObjectOfType<SceneManagerScript>();
        //sms.SaveGameObjects();
        if (scene != null)
             SceneManager.LoadScene(scene, LoadSceneMode.Additive); //LoadSceneMode.Additive
    }
    public void UnloadScene(string scene)
    {
        SceneManager.UnloadScene(scene);
    }
}
