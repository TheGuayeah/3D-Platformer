using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RuntimeAnimatorController))]
public class EnemyController : MonoBehaviour
{
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState m_CurrentState;
    [HideInInspector] public Animator m_Animator;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform m_Target;

    public float m_Helth = 100f;
    public float timeBetweenAttacks = 2f;
    public float damageForce = 10f;
    public float rotationTime = 3f;
    public float lookRadius = 1f;

    [SerializeField] private Slider m_HealthSlider;


    void Start()
    {
        // Creamos los estados de nuestra IA
        patrolState = new PatrolState(this);
        attackState = new AttackState(this);

        // Le decimos que inicialmente empezará patrullando
        m_CurrentState = patrolState;

        // Guardamos la referencia de nuestro NavMesh Agent y del target
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Target = PlayerController.m_Instance.m_player;

        // Hacemos el SetUp de la barra de salud
        m_HealthSlider.maxValue = m_Helth;
        m_HealthSlider.value = m_Helth;

        // Localizamos el Animator Controller
        m_Animator = GetComponent<Animator>();
    }


    void Update()
    {
        // Llamamos al Update de nuestro estado
        m_CurrentState.UpdateState();
    }

    /// <summary>
    /// Disminuir la salud del enemigo
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(float value)
    {
        m_Helth -= value;
        m_CurrentState.Impact();
        m_HealthSlider.value = m_Helth;
        m_HealthSlider.transform.GetChild(1).GetComponentInChildren<Image>().color =
            Color.Lerp(Color.red, Color.green, m_Helth / m_HealthSlider.maxValue);
        if (m_Helth <= 0f)
        {
            m_Animator.SetTrigger("Die");
            Destroy(gameObject, m_Animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 pos = new Vector3(transform.position.x, 1f, transform.position.z);
        Gizmos.DrawWireSphere(pos, lookRadius);
    }
}
