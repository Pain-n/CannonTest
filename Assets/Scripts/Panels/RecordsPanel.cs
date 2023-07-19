using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordsPanel : MonoBehaviour
{
    public TextMeshProUGUI RecordsText;
    public Button CloseButton;
    private void Start()
    {
        CloseButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        RecordsText.text = "Best scores \n";
        for (int i = 0; i < GlobalContext.Instance.BestScores.Count; i++)
        {
            RecordsText.text += $"{i+1}: {GlobalContext.Instance.BestScores[i]} \n";
        }
    }
}
