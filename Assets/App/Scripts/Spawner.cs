using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using ShootingGame.JobSystem;
using Unity.Jobs;

public class Spawner : MonoBehaviour {

    public float spawnTime = 3f;            // How long between each spawn.

    // How fast is the object
    public float m_ObjectSpeed = -1f;

    // Screen bounds
    public float topBound = 16.5f;
    public float bottomBound = -13.5f;

    // Variables for movement jobs
    internal TransformAccessArray transforms;
    MovementJob moveJob;
    JobHandle moveHandle;

    // Use this for initialization
    void Start () {
       
    }

    // Update is called once per frame
    void Update () {
        moveHandle.Complete();

        moveJob = new MovementJob()
        {
            moveSpeed = m_ObjectSpeed,
            topBound = topBound,
            bottomBound = bottomBound,
            deltaTime = Time.deltaTime
        };

        moveHandle = moveJob.Schedule(transforms);
        JobHandle.ScheduleBatchedJobs();
    }

    private void OnDisable()
    {
        moveHandle.Complete();
        transforms.Dispose();
    }

    /// <summary>
    /// Spawns object
    /// </summary>
    public void Spawn(string objToSpawn)
    {
        moveHandle.Complete();


        GameObject objInstance = ObjectPooler.SharedInstance.GetPooledObject(objToSpawn);
        Vector3 spawnPoint = new Vector3(Random.Range(-1f, 1f), 1.5f, 0f);
        objInstance.transform.position = spawnPoint;
        objInstance.gameObject.SetActive(true);
        objInstance.gameObject.GetComponent<Ship>().InitializeParameters();

        // If the instance is using an available pooled object, do not add to the job queue again.
        if (objInstance.name.Split('(').Length > 1)
        {
            transforms.capacity = transforms.length + 1;

            transforms.Add(objInstance.transform);

            objInstance.name = objToSpawn + transforms.length;

        }
    }
}
