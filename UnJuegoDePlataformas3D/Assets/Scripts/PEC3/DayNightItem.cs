using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightItem : MonoBehaviour
{
    public DayNightManager timeManager;
    public float rotationTime = 2f;
    public Transform gameAreaCenter;
    public float gameAreaSizeX = 50;
    public float gameAreaSizeZ = 50;

    void Start()
    {
        Respawn();
    }

    void Update()
    {
        transform.Rotate(0, 360 * Time.deltaTime / rotationTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Respawn();
    }

    void Respawn()
    {
        float randomX = Random.Range(-gameAreaSizeX / 2, gameAreaSizeX / 2);
        float randomZ = Random.Range(-gameAreaSizeZ / 2, gameAreaSizeZ / 2);

        transform.position = new Vector3( gameAreaSizeX + randomX, 50, gameAreaSizeZ + randomZ);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 100f))
            transform.position = hit.point + new Vector3(0, 1, 0);
        if (hit.collider.CompareTag("Tree") || hit.collider.CompareTag("Light") || hit.collider.CompareTag("Fence"))
            Respawn();

        timeManager.Reset();
    }
}
