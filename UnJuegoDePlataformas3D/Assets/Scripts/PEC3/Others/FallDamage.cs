using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class FallDamage : MonoBehaviour
{
    public float m_DamageThreshold = 10f;
    [SerializeField] private float m_StartPosY;
    [SerializeField] private float m_EndPosY;
    private bool m_FirstCall = false;

    void Update()
    {
        if (GetComponent<CharacterController>() == null)
        {
            if (FloorDist() > 1)
            {
                if (transform.position.y > m_StartPosY)
                {
                    m_FirstCall = true;
                }
                if (m_FirstCall)
                {
                    m_StartPosY = transform.position.y;
                    m_FirstCall = false;
                }
            }
            else
            {
                m_EndPosY = transform.position.y;
                bool damageMe = m_StartPosY - m_EndPosY > m_DamageThreshold;
                if (damageMe)
                {
                    GetComponent<PlayerController>().DecreaseHealth(m_StartPosY - m_EndPosY);

                    m_FirstCall = true;
                }

            }
        }
        else
        {
            if (!GetComponent<CharacterController>().isGrounded)
            {
                if (transform.position.y > m_StartPosY)
                {
                    m_FirstCall = true;
                }
                if (m_FirstCall)
                {
                    m_StartPosY = transform.position.y;
                    m_FirstCall = false;
                }
            }
            else
            {
                m_EndPosY = transform.position.y;
                bool damageMe = m_StartPosY - m_EndPosY > m_DamageThreshold;
                if (damageMe)
                {
                    GetComponent<PlayerController>().DecreaseHealth(m_StartPosY - m_EndPosY);

                    m_FirstCall = true;
                }

            }
        }
    }

    /// <summary>
    /// Calcular la distancia con el suelo
    /// </summary>
    /// <returns></returns>
    private float FloorDist()
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.position, Vector3.down, out hit))
        {
            return hit.distance;
        }
        else
            return 0;
    }

    /// <summary>
    /// Dibujar una línea desde el jugador hasta el suelo
    /// </summary>
    public void OnDrawGizmos()
    {
        if (GetComponent<CharacterController>() == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -FloorDist(), 0));
        }
    }
}
