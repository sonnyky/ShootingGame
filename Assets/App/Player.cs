using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 5;
	// PlayerBulletプレハブ
	public GameObject bullet;
	// Use this for initialization
	IEnumerator Start () {
		while (true) {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet_Normal");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
			yield return new WaitForSeconds (0.05f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		
		Vector2 direction = new Vector2 (x, y).normalized;
		
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}
}
