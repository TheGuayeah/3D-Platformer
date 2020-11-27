using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponController : Weapon
{
    public enum m_FireTypes { Auto, Semiauto }
    public m_FireTypes m_Type;
    [SerializeField] private float m_WeaponDamage = 10f;
    [Tooltip("Distancia a la que el arma puede alcanzar un objetivo")]
    [SerializeField] private float m_Range = 15f;
    public AudioClip m_FireSound;
    public GameObject m_DecalPrefab;
    [HideInInspector] public List<GameObject> m_TotalDecals = new List<GameObject>();
    private Transform m_ShotsParent;
    public AudioSource m_shotSFX;

    private float m_FireRate = 0f;
    private PlayerController m_Player;
    private float m_Height = 1.25f;

    void Awake()
    {
        m_ShotsParent = GameObject.Find("ShotsParent").transform;
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Shoot()
    {
        if(m_Player.m_Animator.GetBool("Gunplay"))
            m_Player.m_Animator.SetBool("Gunplay", false);

        if (m_Ammo > m_MinAmmo)
        {
            switch (m_Type)
            {
                case m_FireTypes.Auto:
                    if (Input.GetButton("Fire1"))
                    {
                        if (!m_Player.m_Animator.GetBool("Gunplay"))
                            m_Player.m_Animator.SetBool("Gunplay", true);
                        DoAutoFire();
                    }
                    else if (Input.GetButtonUp("Fire1"))
                    {
                        if (m_Player.m_Animator.GetBool("Gunplay"))
                            m_Player.m_Animator.SetBool("Gunplay", false);
                    }
                    break;
                case m_FireTypes.Semiauto:
                    if (Input.GetButtonDown("Fire1"))
                    {
                        m_Player.m_Animator.SetTrigger("Shoot");
                        DoSemiautoFire();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Fire for automatic weapons
    /// </summary>
    private void DoAutoFire()
    {
        m_FireRate += Time.deltaTime;

        bool canShoot = m_Ammo > m_MinAmmo && m_FireRate > 0.15f;
        if (canShoot)
        {
            DoSemiautoFire();
            m_FireRate = 0;
        }
    }

    /// <summary>
    /// Fire for semiauto weapons
    /// </summary>
    private void DoSemiautoFire()
    {
        RaycastHit hit;
        Vector3 pos = m_Player.transform.position + new Vector3(0, m_Height, 0);
        Vector3 distance = new Vector3(0, 0, m_Range);
        Vector3 direction = pos + m_Player.transform.TransformDirection(distance);
        if (Physics.Raycast(pos, direction, out hit))
        {
            if (!hit.transform.gameObject.CompareTag("Enemy"))
            {
                // Creo un impacto de bala en el objeto al que apunto
                GameObject tempDecal = Instantiate(m_DecalPrefab, hit.point + hit.normal * 0.01f,
                    Quaternion.FromToRotation(Vector3.forward, -hit.normal), m_ShotsParent);

                m_TotalDecals.Add(tempDecal);

                // Cuando hay 10 o más Decals destruyo el primero y lo elimino de la lista
                if (m_TotalDecals.Count >= 10)
                {
                    Destroy(m_TotalDecals[0]);
                    m_TotalDecals.RemoveAt(0);
                }
            }


            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                hit.transform.GetComponentInParent<EnemyController>().TakeDamage(m_WeaponDamage);
            }
        }

        CreateWeaponSound();

        DecreaseAmmo();
    }

    private void CreateWeaponSound()
    {
        // Creo multiples instancias del audio para que suene cada vez que disparo (Ej: ráfagas)
        AudioSource sound = Instantiate(m_shotSFX, m_ShotsParent);
        sound.clip = m_FireSound;
        sound.Play();
        // Elimino la instancia cuando el audio termina
        Destroy(sound.gameObject, sound.clip.length);
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Comprobar hacia donde mira el jugador
    /// </summary>
    public void OnDrawGizmos()
    {
        if (GetComponent<CharacterController>() == null && m_Player != null)
        {
            Gizmos.color = Color.red;
            Vector3 pos = m_Player.transform.position + new Vector3(0, m_Height, 0);
            Vector3 distance = new Vector3(0, 0, m_Range);
            Vector3 direction = pos + m_Player.transform.TransformDirection(distance);
            Gizmos.DrawLine(pos, direction);
        }
    }
}
