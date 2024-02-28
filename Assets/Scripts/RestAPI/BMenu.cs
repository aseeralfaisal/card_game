using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BMenu : MonoBehaviour
{
    public int amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
        
        //amount = CardTurnManager.turnManager.totalChips; 
        BalanceUpdate.Instance.SendBalanceUpdateRequest(amount);
    }
}
