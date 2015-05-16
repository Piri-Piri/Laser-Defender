using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 150f;
	public int scoreValue = 150;
	
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float shotsPerSeconds = 0.5f;
	
	public AudioClip soundOfDeath;
	
	private ScoreKeeper scoreKeeper;
	
	void Start () {
		scoreKeeper = GameObject.Find ("Score").GetComponent <ScoreKeeper> ();
	}
	
	// Update is called once per frame
	void Update () {
		float probability = Time.deltaTime * shotsPerSeconds;
		if (Random.value < probability) {
			Fire ();
		}
	}
	
	void Fire () {
		GameObject laserBeam = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		laserBeam.rigidbody2D.velocity = new Vector2 (0, -projectileSpeed);
		audio.Play ();
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent <Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				Die ();
			}	
		}
	}
	
	void Die () {
		AudioSource.PlayClipAtPoint(soundOfDeath, gameObject.transform.position);
		scoreKeeper.Score (scoreValue);
		Destroy (gameObject);
	}	
}
