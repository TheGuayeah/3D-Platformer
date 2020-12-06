using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    EnemyController myEnemy;

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
        FaceTarget();
        float distance = Vector3.Distance(myEnemy.m_Target.position, myEnemy.transform.position);
        if(distance <= myEnemy.lookRadius)
        {
            myEnemy.navMeshAgent.SetDestination(myEnemy.m_Target.position);
            if(distance <= myEnemy.navMeshAgent.stoppingDistance)
            {
                GoToAttackState();
            }
        }
    }

    public void Impact()
    {
        myEnemy.m_Animator.SetTrigger("Hit");
        myEnemy.navMeshAgent.isStopped = true;        
    }

    public void FaceTarget()
    {
        Vector3 direction = (myEnemy.m_Target.position - myEnemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        myEnemy.transform.rotation = Quaternion.Slerp(myEnemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void GoToAttackState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.m_CurrentState = myEnemy.attackState;
    }

    public void GoToPatrolState() { }
}
