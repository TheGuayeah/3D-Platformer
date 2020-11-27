using System;
using UnityEngine;

[Serializable]
public class GrenadeController : Weapon
{
    public int m_ExplosionRadius = 3;
    public GameObject m_Explosion;
    public GameObject m_Prefab;
    public AudioSource m_ExplosionSFX;
    public AudioClip m_ExplosionSound;
    [HideInInspector] public Transform m_GrenadeHolder;
    [HideInInspector] public bool m_IsThrown = false;

    private float m_ExplodeTime;
    private float m_GrenadeForce = 0;
    private bool m_IsExploded = false;

    private void OnEnable()
    {
        m_Explosion = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (m_IsThrown)
        {
            GetComponent<Rigidbody>().useGravity = true;
            m_ExplodeTime += Time.deltaTime;
            if (m_ExplodeTime > 3)
            {
                Collider[] tempObjects = Physics.OverlapSphere(transform.position, m_ExplosionRadius);
                for (int i = 0; i < tempObjects.Length; i++)
                {
                    // Distancia relativa al jugador
                    float finalDistance = Vector3.Distance(transform.position, tempObjects[i].transform.position);

                    if (finalDistance < 1)
                        finalDistance = 1;

                    // Daño según la distancia relativa al personaje
                    float damage = 100 / finalDistance;

                    if (tempObjects[i].CompareTag("Player"))
                    {
                        tempObjects[i].GetComponent<PlayerController>().TakeDamage(damage);
                    }
                    else if (tempObjects[i].CompareTag("Enemy"))
                    {
                        tempObjects[i].GetComponent<EnemyController>().TakeDamage(damage);
                    }
                }

            }
            if (m_ExplodeTime > 2.8f)
            {
                m_Explosion.SetActive(true);
                if (!m_IsExploded)
                    Explode();
            }
        }
    }

    private void Explode()
    {
        m_IsExploded = true;
        AudioSource sound = Instantiate(m_ExplosionSFX);
        sound.clip = m_ExplosionSound;
        sound.Play();
        Destroy(sound.gameObject, sound.clip.length);
        Destroy(gameObject, sound.clip.length);
    }

    public override void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            //Cargar la distancia de lanzamiento de la granada
            m_GrenadeForce = Mathf.MoveTowards(m_GrenadeForce, 1, 0.8f * Time.deltaTime);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            GetComponent<Rigidbody>().AddForce(m_GrenadeHolder.transform.forward * 500 * m_GrenadeForce);
            m_GrenadeForce = 0;
            m_IsThrown = true;
            DecreaseAmmo();
        }
    }
}