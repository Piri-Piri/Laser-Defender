using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 150f;
	
	public GameObject projectile;
	public float projectileSpeed = 10;
	public float shotsPerSeconds = 0.5f;
	
	// Update is called once per frame
	void Update () {
		float probability = Time.deltaTime * shotsPerSeconds;
		if (Random.value < probability) {
			Fire ();
		}
	}
	
	void Fire () {
		Vector3 startPosition = transform.position + new Vector3(0, -1);
		GameObject laserBeam = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
		laserBeam.rigidbody2D.velocity = new Vector2(0, -projectileSpeed);
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0) {
				Destroy(gameObject);
			}	
		}
	}	
}
