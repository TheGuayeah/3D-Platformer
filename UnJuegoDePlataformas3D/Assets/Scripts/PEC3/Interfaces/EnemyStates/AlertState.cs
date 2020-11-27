using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    EnemyController myEnemy;
    float currentRotationTime = 0f;

    public AlertState(EnemyController enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.yellow;

        // Rotamos al enemigo una vuelta completa en el tiempo indicado
        myEnemy.transform.rotation *=
            Quaternion.Euler(0f, Time.deltaTime * 360 * 1f / myEnemy.rotationTime, 0f);

        bool turnedAround = currentRotationTime > myEnemy.rotationTime;
        if (turnedAround)
        {
            currentRotationTime = 0;
            GoToPatrolState();
        }
        else
        {
            // Si aún estamos dándo vueltas, lanzamos un rayo
            // hacia donde mira el enemigo
            RaycastHit hit;
            Vector3 myPosition = myEnemy.transform.position;
            if (Physics.Raycast(new Ray(new Vector3(myPosition.x, 0.5f, myPosition.z),
                myEnemy.transform.forward * 100f), out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log(hit.collider.name);
                    GoToAttackState();
                }
            }
        }

        currentRotationTime += Time.deltaTime;
    }

    public void Impact()
    {
        GoToAttackState();
    }

    public void GoToAlertState() { }

    public void GoToAttackState()
    {
        myEnemy.m_CurrentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {
        // Volvemos a ponerlo en marcha
        myEnemy.navMeshAgent.isStopped = false;
        myEnemy.m_CurrentState = myEnemy.patrolState;
    }

    // Al estar buscando no haremos caso del Trigger
    public void OnTriggerEnter(Collider other) { }

    public void OnTriggerStay(Collider other) { }

    public void OnTriggerExit(Collider other) { }
}
