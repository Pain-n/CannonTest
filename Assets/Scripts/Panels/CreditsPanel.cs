using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsPanel : MonoBehaviour
{
    public Button CloseButton;
    void Start()
    {
        CloseButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }
}
