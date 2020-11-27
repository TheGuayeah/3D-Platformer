using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyController myEnemy;
    float actualTimeBetweenShots = 0f;

    public AttackState(EnemyController enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.red;
        actualTimeBetweenShots += Time.deltaTime;
    }

    public void Impact() { }

    public void GoToAlertState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.m_CurrentState = myEnemy.alertState;
    }

    public void GoToAttackState() { }

    public void GoToPatrolState() { }

    public void OnTriggerEnter(Collider other) { }

    public void OnTriggerStay(Collider other)
    {
        // Estaremos mirando siempre al Player
        Vector3 lookDirection = other.transform.position - myEnemy.transform.position;

        // Rotando solamente el eje Y
        myEnemy.transform.rotation = Quaternion.FromToRotation(Vector3.forward,
            new Vector3(lookDirection.x, 0f, lookDirection.z));

        // Le toca volver  a disparar
        if (actualTimeBetweenShots > myEnemy.timeBetweenShots)
        {
            actualTimeBetweenShots = 0;
            if (other.CompareTag("Player"))
                other.gameObject.GetComponent<PlayerController>().TakeDamage(myEnemy.damageForce);
        }
    }

    /// <summary>
    /// Si el Player sale de su radio, pasa a modo Alert.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        GoToAlertState();
    }
}
