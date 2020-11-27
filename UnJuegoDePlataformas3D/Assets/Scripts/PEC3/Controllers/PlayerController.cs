using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator m_Animator;

    [HideInInspector] public float m_Health;
    private float m_Shield;
    [SerializeField] private Slider m_GuiHeath;
    [SerializeField] private Slider m_GuiShield;
    [SerializeField] private Text m_GuiWeapon;
    [HideInInspector] public readonly float m_MaxHealth = 100f;
    private readonly float m_MinHealth = 0f;
    private readonly float m_MaxShield = 100f;
    private readonly float m_MinShield = 0f;

    [SerializeField] private bool m_canShoot = true;
    [SerializeField] private Transform m_WeaponHolder;
    [SerializeField] private Transform m_GrenadeHolder;
    public Weapon m_CurrentWeapon;
    public List<Weapon> m_Weapons;
    private int m_CurrenIndexWeapon = 0;
    private MenuManager m_MenuManager;


    void Awake()
    {
        m_MenuManager = FindObjectOfType<MenuManager>().GetComponent<MenuManager>();
    }
    void Start()
    {
        m_Weapons = GameObject.Find("---------ALL WEAPONS---------").GetComponentsInChildren<Weapon>().ToList();
        foreach (Weapon item in m_Weapons)
        {
            item.gameObject.SetActive(false);
        }
        m_Health = m_MaxHealth;
        m_Shield = m_MaxShield;
        GuiUpdate(m_Health, m_Shield);
        ChangeWeapon();
    }

    void Update()
    {
        if (m_canShoot)
            Shoot();

        GetWeaponByAlphaKey();

        if (IsDead())
        {
            m_MenuManager.GoToScene("GameOver");
        }
    }

    /// <summary>
    /// Comprueba el botón pulsado para cambiar el arma
    /// </summary>
    private void GetWeaponByAlphaKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeapon();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (m_Weapons[2].m_Ammo > m_Weapons[2].m_MinAmmo)
                ChangeWeapon(2);
            else
                Debug.Log("No hay suficientes granadas en el inventario");
        }
    }

    /// <summary>
    /// Cambiar el arma seleccionada
    /// </summary>
    /// <param name="index">Posición del arma en Weapons List</param>
    private void ChangeWeapon(int index = 0)
    {
        // Me aseguro de que no se ejecuta la animación del fusil de asalto al cambiar de arma
        if(m_Animator.GetBool("Gunplay"))
            m_Animator.SetBool("Gunplay", false);

        // Elimino los decals del arma que retiro
        if (IsWeapon(m_CurrentWeapon))
        {
            foreach (GameObject item in m_CurrentWeapon.GetComponent<WeaponController>().m_TotalDecals)
            {
                Destroy(item);
            }
            m_CurrentWeapon.GetComponent<WeaponController>().m_TotalDecals.Clear();
        }

        if (m_Weapons.Count > index)
        {
            // Actualizo el setup del arma de la lista
            if (m_CurrentWeapon != null)
                m_Weapons[m_CurrenIndexWeapon].m_Ammo = m_CurrentWeapon.m_Ammo;
            // Cambio el índice del árma actual al de la nueva arma
            m_CurrenIndexWeapon = index;

            // Elimino el arma actual
            if (m_WeaponHolder.childCount > 0)
                Destroy(m_WeaponHolder.GetChild(0).gameObject);

            // Creo la nueva arma en la escena
            Weapon weapon = m_Weapons[index].GetComponent<Weapon>();
            GameObject tempWeapon;
            if (IsWeapon(weapon))
                tempWeapon = Instantiate(weapon.gameObject, m_WeaponHolder);

            else
            {
                weapon.GetComponent<GrenadeController>().m_GrenadeHolder = m_GrenadeHolder;
                tempWeapon = Instantiate(weapon.gameObject, m_GrenadeHolder);
            }

            if (tempWeapon != null)
            {
                tempWeapon.SetActive(true);
                m_CurrentWeapon = tempWeapon.GetComponent<Weapon>();
            }
            m_GuiWeapon.text = weapon.gameObject.name + " " + weapon.m_Ammo + " / " + weapon.m_MaxAmmo;
        }
    }

    /// <summary>
    /// Ejecutar la acción de disparo del arma seleccionada
    /// </summary>
    private void Shoot()
    {
        if (!IsDead())
        {
            m_CurrentWeapon.Shoot();
            m_Weapons[m_CurrenIndexWeapon].m_Ammo = m_CurrentWeapon.m_Ammo;
            Weapon weapon = m_Weapons[m_CurrenIndexWeapon].GetComponent<Weapon>();
            m_GuiWeapon.text = weapon.gameObject.name + " " + weapon.m_Ammo + " / " + weapon.m_MaxAmmo;
            if (!HasWeapon() || IsGrenadeThrown())
            {
                ChangeWeapon();
            }
        }
    }

    /// <summary>
    /// Comprobar si el jugador está muerto
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return m_Health <= m_MinHealth;
    }

    /// <summary>
    /// Comprobar si el jugador tiene escudo
    /// </summary>
    /// <returns></returns>
    public bool HasShield()
    {
        return m_Shield > m_MinShield;
    }

    /// <summary>
    /// Aumentar la salud del jugador
    /// </summary>
    /// <param name="value">Valor a aumentar</param>
    public void IncreaseHealth(float value)
    {
        m_Health += value;
        if (m_Health > m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }
        GuiUpdate(m_Health, m_Shield);
    }

    /// <summary>
    /// Disminuir la salud del jugador
    /// </summary>
    /// <param name="value"> Valor a disminuir</param>
    public void DecreaseHealth(float value)
    {
        m_Health -= value;
        if (m_Health < m_MinHealth)
        {
            m_Health = m_MinHealth;
        }
        GuiUpdate(m_Health, m_Shield);
    }

    /// <summary>
    /// Aunmentar la salud del escudo
    /// </summary>
    /// <param name="value">Valor a aumentar</param>
    public void IncreaseShield(float value)
    {
        m_Shield += value;
        if (m_Health > m_MaxShield)
        {
            m_Health = m_MaxShield;
        }
        GuiUpdate(m_Health, m_Shield);
    }

    /// <summary>
    /// Si el jugador aún tiene escudo, éste se verá reducido y el jugador recibirá un 15% del daño.
    /// De lo contrario el jugador recibirá todo el daño.
    /// </summary>
    /// <param name="value">Daño recibido</param>
    public void TakeDamage(float value)
    {
        m_Shield -= value;
        if (m_Shield < m_MinShield)
        {
            m_Shield = m_MinShield;
        }

        if (HasShield())
        {
            DecreaseHealth(value * 0.15f);
        }
        else
        {
            DecreaseHealth(value);
        }
    }

    /// <summary>
    /// Actualizar los stats del jugador en pantalla
    /// </summary>
    /// <param name="health"></param>
    /// <param name="shield"></param>
    private void GuiUpdate(float health, float shield)
    {
        m_GuiHeath.value = health;
        m_GuiHeath.transform.GetChild(1).GetComponentInChildren<Image>().color =
            Color.Lerp(Color.red, Color.green, m_Health / m_MaxHealth);
        m_GuiShield.value = shield;
        m_GuiShield.transform.GetChild(1).GetComponentInChildren<Image>().color =
            Color.Lerp(Color.blue, Color.cyan, m_Shield / m_MaxShield);
    }

    private bool IsWeapon(Weapon weapon)
    {
        return weapon != null && weapon.GetType() == typeof(WeaponController);
    }

    private bool IsGrenadeThrown()
    {
        return m_CurrentWeapon != null &&
            m_CurrentWeapon.GetType() == typeof(GrenadeController) &&
            m_CurrentWeapon.GetComponent<GrenadeController>().m_IsThrown;
    }

    private bool HasWeapon()
    {
        return m_CurrentWeapon != null;
    }
}
