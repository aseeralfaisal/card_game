using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TotolChips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalChipsTxt;


    // Update is called once per frame
    void Update()
    {
        totalChipsTxt.text = "" + CardTurnManager.turnManager.totalChips;
    }
}
