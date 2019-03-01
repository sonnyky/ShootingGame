using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class BonusSpawner : Spawner {

    public enum PowerUps
    {
        WEAPONS_RANDOM,
        ARMOR_INCREASE,
        HEALTH_INCREASE
    }

    public string GetObjectTag(PowerUps type)
    {
        switch (type)
        {
            case PowerUps.WEAPONS_RANDOM:
                return "PowerUp_Weapons_Random";
            default:
                return "PowerUp_Weapons_Random";
        }
    }



    // Use this for initialization
    void Start () {
        transforms = new TransformAccessArray(0, -1);
        InvokeRepeating("Add", spawnTime, spawnTime);
    }

    void Add()
    {
        PowerUps powerUp = Tinker.Utilites.RandomEnumValue<PowerUps>();
        string powerUpTag = GetObjectTag(powerUp);
        Spawn(powerUpTag);

    }
}
