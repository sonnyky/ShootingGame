using UnityEngine;
using UnityEngine.Jobs;
using System.Collections;
using ShootingGame.JobSystem;
using Unity.Jobs;

public class EnemySpawn : Spawner {

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

    
    void Start () {
        transforms = new TransformAccessArray(0, -1);
        InvokeRepeating("AddShips", spawnTime, spawnTime);
    }

    void AddShips()
    {
        Enemies enemy = Tinker.Utilites.RandomEnumValue<Enemies>();
        string enemyTag = GetObjectTag(enemy);
        Spawn(enemyTag);

    }
}

