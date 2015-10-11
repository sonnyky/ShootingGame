using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemy;                // The enemy prefab to be spawned.
    public float spawnTime = 3f;            // How long between each spawn.
   
    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void Spawn()
    {
       
        Vector3 spawnPoint = new Vector3(Random.Range(-1f, 1f), 1.5f, 0f);
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(enemy, spawnPoint, this.transform.rotation);
    }
}
