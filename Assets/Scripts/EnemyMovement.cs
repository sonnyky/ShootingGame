using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
    Vector3 speed;
    Vector3 cur_pos;
	// Use this for initialization
	void Start () {
        speed = new Vector3(0f, -0.005f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        cur_pos = this.transform.position;
        cur_pos.x = cur_pos.x + speed.x;
        cur_pos.y = cur_pos.y + speed.y;
        cur_pos.z = cur_pos.z + speed.z;

        this.transform.position = cur_pos;
	}
}
