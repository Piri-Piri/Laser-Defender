using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float health = 500f;
	
	public float speed;
	public float padding;
	public GameObject projectile;
	public float projectileSpeed;
	public float fireRate;

	private float minX;
	private float maxX;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3 (0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3 (1,0,distance));
		
		minX = leftMost.x + padding;
		maxX = rightMost.x - padding;
	}
	
	void Fire () {
		GameObject laserBeam = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		laserBeam.rigidbody2D.velocity = new Vector2 (0, projectileSpeed);
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		// limit laser by a firing rate
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, fireRate); 	
		}
		
		// release repeating
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ();
		}
		
		if (Input.GetKey (KeyCode.LeftArrow) | Input.GetKey (KeyCode.A)) {	
			transform.position += Vector3.left * speed * Time.deltaTime;	
		} else if (Input.GetKey (KeyCode.RightArrow) | Input.GetKey (KeyCode.D)) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} 
		/*
		else if (Input.GetKey (KeyCode.UpArrow) | Input.GetKey (KeyCode.W)) {
			gameObject.transform.position += Vector3.up * speed * Time.deltaTime;
		} 
		else if (Input.GetKey (KeyCode.DownArrow) | Input.GetKey (KeyCode.S)) {
			gameObject.transform.position += Vector3.down * speed * Time.deltaTime;
		}
		*/
		
		// restrict the player to the gamespace
		float newX = Mathf.Clamp (gameObject.transform.position.x, minX, maxX);
		transform.position = new Vector3 (newX, transform.position.y);
	}
	
	// Gsset damage by enemies laser projectiles
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent <Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit();
			if (health <= 0) {
				Die ();
			}	
		}
	}
	
	void Die () {
		LevelManager manager = GameObject.Find("LevelManager").GetComponent <LevelManager> ();
		manager.LoadLevel("Lose Screen");
		Destroy (gameObject);
	}
}
