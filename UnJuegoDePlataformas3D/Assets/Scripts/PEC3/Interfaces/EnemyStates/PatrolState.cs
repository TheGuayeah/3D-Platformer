using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    EnemyController myEnemy;
    private int nextWayPoint = 0;

    /// <summary>
    /// Guardamos una referencia a la IA de nuestro enemigo
    /// </summary>
    /// <param name="enemy"></param>
    public PatrolState(EnemyController enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.green;

        // Le indicamos la dirección al NavMeshAgent
        Vector3 nextDestination = new Vector3(myEnemy.wayPoints[nextWayPoint].
            position.x, myEnemy.transform.position.y, myEnemy.wayPoints[nextWayPoint].position.z);

        myEnemy.navMeshAgent.destination = nextDestination;

        // Si hemos llegado al destino, cambiamos la referencia al siguiente Waypoint
        if (myEnemy.navMeshAgent.remainingDistance <=
            myEnemy.navMeshAgent.stoppingDistance)
        {
            nextWayPoint = (nextWayPoint++) % myEnemy.wayPoints.Length;
        }
    }

    public void Impact()
    {
        GoToAttackState();
    }

    public void GoToAlertState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.m_CurrentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.m_CurrentState = myEnemy.attackState;
    }

    public void GoToPatrolState() { }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GoToAlertState();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            GoToAlertState();
    }

    public void OnTriggerExit(Collider other) { }
}
