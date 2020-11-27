using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public int m_Ammo;
    public int m_MaxAmmo = 30;
    [HideInInspector] public int m_MinAmmo = 0;

    public virtual void Shoot() { }

    public void IncreaseAmmo(int value)
    {
        m_Ammo += value;
        if (m_Ammo > m_MaxAmmo)
        {
            m_Ammo = m_MaxAmmo;
        }
    }

    public void DecreaseAmmo(int value = 1)
    {
        m_Ammo -= value;
        if (m_Ammo < m_MinAmmo)
        {
            m_Ammo = m_MinAmmo;
        }
    }
}
