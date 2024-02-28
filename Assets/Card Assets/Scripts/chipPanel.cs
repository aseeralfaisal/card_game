using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class chipPanel : MonoBehaviour
{
    public TextMeshProUGUI chipTxt;
    void Start()
    {
       
    }

    public void AddChips(int amount)
    {
        Vector3 targetPos = Vector3.zero;
        targetPos.y = 2.0f;

        transform.DOMove(targetPos, 1.0f).OnComplete(() =>
        {
            CardTurnManager.turnManager.totalChips += amount;

            Destroy(gameObject);
        });
    }
}
