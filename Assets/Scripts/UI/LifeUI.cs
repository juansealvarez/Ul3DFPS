using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    private float saludActual;
    private float saludInicial;
    private float lifePercentage;
    private void Start()
    {
        saludInicial = PlayerController.PlayerHealth;
        saludActual = saludInicial;
        lifePercentage = 100f;
    }

    private void Update()
    {
        saludActual = PlayerController.PlayerHealth;
        lifePercentage = (saludActual/saludInicial)*100;
        GetComponent<TextMeshProUGUI>().text = lifePercentage.ToString()+"%";
    }
}
