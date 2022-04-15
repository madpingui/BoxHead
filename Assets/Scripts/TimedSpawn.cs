using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawn : MonoBehaviour {

    [SerializeField]
    private GameObject spawnee;

    private int contador;

    private bool stopSpawning = false;

    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private float spawnDelay;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
	}

    public void SpawnObject()
    {
        GameObject enemmigo = Instantiate(spawnee, transform.position , transform.rotation);
        enemmigo.transform.parent = transform.parent;
        contador++;
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }

        if(contador >= 5)
        {
            spawnDelay -= 0.2f;
            if(spawnDelay < 0.5f)
            {
                spawnDelay = 0.5f;
            }
        }
    }
}
