using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller;

    public void StartAttack()
    {
        controller.StartAttack();
        Debug.Log("Entro");
    }

    public void StopAttack()
    {
        controller.StopAttack();
        Debug.Log("Salio");
    }
}
