using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TableSetScript : MonoBehaviour
{
    public int chipStake;
    public static TableSetScript Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void setChipAMount(int amount)
    {
        chipStake = amount;
    }

    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);

        
    }
}
