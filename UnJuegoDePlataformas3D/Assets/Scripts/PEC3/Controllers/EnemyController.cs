using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState m_CurrentState;

    [HideInInspector] public NavMeshAgent navMeshAgent;

    public Light myLight;
    public float m_Helth = 100f;
    public float timeBetweenShots = 1f;
    public float damageForce = 10f;
    public float rotationTime = 3f;
    public float shootHeight = 0.5f;
    public Transform[] wayPoints;

    [SerializeField] private Slider m_HealthSlider;


    void Start()
    {
        // Creamos los estados de nuestra IA
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        // Le decimos que inicialmente empezará patrullando
        m_CurrentState = patrolState;

        // Guardamos la referencia de nuestro NavMesh Agent
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Hacemos el SetUp de la barra de salud
        m_HealthSlider.maxValue = m_Helth;
        m_HealthSlider.value = m_Helth;
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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_CurrentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        m_CurrentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_CurrentState.OnTriggerExit(other);
    }
}
