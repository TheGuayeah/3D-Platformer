using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    public enum ItemTypes { Ammo, Health, Shield }
    public ItemTypes type;
    public int amount = 10;
    public float speed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            bool canDestroy = false;
            switch (type)
            {
                case ItemTypes.Ammo:
                    if (controller.m_CurrentWeapon.m_Ammo < controller.m_CurrentWeapon.m_MaxAmmo)
                    {
                        controller.m_CurrentWeapon.IncreaseAmmo(amount);
                        canDestroy = true;
                    }
                    break;
                case ItemTypes.Health:
                    if (controller.m_Health < controller.m_MaxHealth)
                    {
                        controller.IncreaseHealth(amount);
                        canDestroy = true;
                    }
                    break;
                case ItemTypes.Shield:
                    if (controller.m_Shield < controller.m_MaxShield)
                    {
                        controller.IncreaseShield(amount);
                        canDestroy = true;
                    }
                    break;
            }
            if (canDestroy)
                Destroy(gameObject);
        }
    }
}