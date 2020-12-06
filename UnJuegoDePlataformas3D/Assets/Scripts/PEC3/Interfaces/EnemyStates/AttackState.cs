using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyController myEnemy;
    float actualTimeBetweenAttacks = 0f;

    public AttackState(EnemyController enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        actualTimeBetweenAttacks += Time.deltaTime;
        FaceTarget();
        AttackTarget();
    }

    private void AttackTarget()
    {
        PlayerController player = myEnemy.m_Target.gameObject.GetComponent<PlayerController>();

        // Atacamos al Player
        if (actualTimeBetweenAttacks > myEnemy.timeBetweenAttacks)
        {
            actualTimeBetweenAttacks = 0;
            myEnemy.m_Animator.SetTrigger("Attack");
            player.TakeDamage(myEnemy.damageForce);
        }
    }

    public void FaceTarget()
    {
        Vector3 direction = (myEnemy.m_Target.position - myEnemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        myEnemy.transform.rotation = Quaternion.Slerp(myEnemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void Impact() 
    {
        actualTimeBetweenAttacks = 0;
        myEnemy.m_Animator.SetTrigger("Hit");
    }

    public void GoToAttackState() { }

    public void GoToPatrolState()
    {
        myEnemy.navMeshAgent.isStopped = false;
        myEnemy.m_CurrentState = myEnemy.patrolState;
    }
}
