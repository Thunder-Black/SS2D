using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public Scene tempScene;
    public GameObject tempObject;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void SetScene(Scene scene)
    {
        tempScene = scene;
    }
    public Scene GetScene()
    {
        return tempScene;
    }
    public void ClearGameObjects()
    {
        Destroy(tempObject);
    }
    public void SaveGameObjects()
    {    
        UnitController[] ships = FindObjectsOfType<UnitController>();
        for (int i = 0; i < ships.Length; i++)
            if (ships[i].playerControlled)
            {
                tempObject = ships[i].gameObject;
                DontDestroyOnLoad(tempObject);
                break;
            }
    }
}
