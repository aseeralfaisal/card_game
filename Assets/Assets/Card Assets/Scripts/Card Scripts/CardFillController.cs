using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Realtime;
using Photon.Pun;

public class CardFillController : MonoBehaviour
{

    public PlayerController playerController;
    public Image fillImage;
    public float fillDuration = 10f;
    
    private Coroutine fillCoroutine;

    void Start()
    {

    }

    public void StartCount()
    {
        //Debug.Log("Start Counting Again");
        ResetTimer();
        // Start the filling animation
        //if (fillImage == null) return;
        fillCoroutine = StartCoroutine(FillCardOverTime());
    }


    IEnumerator FillCardOverTime()
    {
        float elapsedTime = 0f;
        float fillStartTime = Time.time;

        while (elapsedTime < fillDuration)
        {


            if (playerController.isTurn && !CardTurnManager.turnManager.winPanel.activeSelf)
            {
                elapsedTime = Time.time - fillStartTime;

                float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);
                fillImage.fillAmount = fillAmount;
            }
            else
            { 
                
            }

            yield return null;
        }

        playerController.isTurn = false;
        fillImage.fillAmount = 1f;
        if(PhotonNetwork.IsMasterClient)
        {
            CardTurnManager.turnManager.EndTurn();
        }
        // Optionally, you can add more logic here when the filling is complete
    }
    
    public void ResetTimer()
    {
        // Stop the current coroutine if it's running
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        // Reset the fill amount to zero
        FillZero();
    }

    public void FillTotal()
    {
        // Ensure the fill amount is exactly 1 at the end of the animation
        fillImage.fillAmount = 1f;
    }

    public void FillZero()
    {
        fillImage.fillAmount = 0f;
    }
}
