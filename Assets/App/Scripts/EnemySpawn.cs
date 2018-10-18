using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
    public float spawnTime = 3f;            // How long between each spawn.
   

    public enum Enemies
    {
        ENEMY_FIGHTER
    }

    public string GetObjectTag(Enemies type)
    {
        switch (type)
        {
            case Enemies.ENEMY_FIGHTER:
                return "Enemy_Fighter";
            default:
                return "Enemy_Fighter";
        }
    }
    

    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void Spawn()
    {
        Enemies enemy = Tinker.Utilites.RandomEnumValue<Enemies>();
        string enemyTag = GetObjectTag(enemy);
        GameObject enemyInstance = ObjectPooler.SharedInstance.GetPooledObject(enemyTag);

        Vector3 spawnPoint = new Vector3(Random.Range(-1f, 1f), 1.5f, 0f);
        enemyInstance.transform.position = spawnPoint;
        enemyInstance.gameObject.SetActive(true);
        enemyInstance.gameObject.GetComponent<Ship>().InitializeParameters();
    }
}
