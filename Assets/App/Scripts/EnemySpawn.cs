using UnityEngine;
using UnityEngine.Jobs;
using System.Collections;
using ShootingGame.JobSystem;
using Unity.Jobs;

public class EnemySpawn : MonoBehaviour {
    public float spawnTime = 3f;            // How long between each spawn.

    public float enemySpeed = -1f;

    public float topBound = 16.5f;
    public float bottomBound = -13.5f;

    public int enemySpawnRate = 1;

    TransformAccessArray transforms;
    MovementJob moveJob;
    JobHandle moveHandle;


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

    private void OnDisable()
    {
        moveHandle.Complete();
        transforms.Dispose();
    }


    // Use this for initialization
    void Start () {
        transforms = new TransformAccessArray(0, -1);
        InvokeRepeating("AddShips", spawnTime, spawnTime);

    }

    // Update is called once per frame
    void Update () {
        moveHandle.Complete();

        moveJob = new MovementJob()
        {
            moveSpeed = enemySpeed,
            topBound = topBound,
            bottomBound = bottomBound,
            deltaTime = Time.deltaTime
        };

        moveHandle = moveJob.Schedule(transforms);
        JobHandle.ScheduleBatchedJobs();
	}

    void AddShips()
    {
        moveHandle.Complete();
        transforms.capacity = transforms.length + enemySpawnRate;

        for(int i=0; i<enemySpawnRate; i++)
        {
            Enemies enemy = Tinker.Utilites.RandomEnumValue<Enemies>();
            string enemyTag = GetObjectTag(enemy);
            GameObject enemyInstance = ObjectPooler.SharedInstance.GetPooledObject(enemyTag);
            Vector3 spawnPoint = new Vector3(Random.Range(-1f, 1f), 1.5f, 0f);
            enemyInstance.transform.position = spawnPoint;
            enemyInstance.gameObject.SetActive(true);
            //enemyInstance.gameObject.GetComponent<Ship>().InitializeParameters();

            transforms.Add(enemyInstance.transform);

        }
    }
    /*
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
    */
}
