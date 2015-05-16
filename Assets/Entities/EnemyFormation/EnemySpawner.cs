using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width;
	public float height;
	public float speed;
	public float padding;
	public float spawnDelaySeconds = 1;
	
	private bool directionRight = true;
	private float minX;
	private float maxX;
	
	// Use this for initialization
	void Start () {
		// Setup the play scene with bounderies 
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3 (0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3 (1,0,distance));
		
		minX = leftMost.x + padding;
		maxX = rightMost.x - padding;
		
		// Spawn the initial enemy formation 
		SpawnUntilFull();
	}
	
	public void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if (directionRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		// restrict the formation move by the screen edges
		float leftEdgeOfFormation = transform.position.x - (width * 0.5f);
		float rightEdgeOfFormation = transform.position.x + (width * 0.5f);
			
		if (leftEdgeOfFormation < minX || rightEdgeOfFormation > maxX) {
			directionRight = !directionRight;	
		}
		
		if (AllEnemiesAreDead ()) {
			Debug.Log ("All enemies has be fallen!");
			SpawnUntilFull ();
		}
	}
	
	void SpawnUntilFull () {
		Transform freePos = NextFreePosition ();
		if (freePos != null) {
			GameObject enemy = Instantiate (enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
			// set the formation place (formally the Position object) 
			// as the parent transform of the enemy
			enemy.transform.parent = freePos;
			
			if (NextFreePosition ()) {
				Invoke("SpawnUntilFull", spawnDelaySeconds);
			}
		}	
	}
	
	Transform NextFreePosition () {
		// loop over each child of the enemy formation
		foreach(Transform child in transform) {
			if (child.childCount <= 0) {
				return child;
			} 
		}
		return null;	
	}
	
	bool AllEnemiesAreDead () {
		// loop over each child of the enemy formation
		foreach(Transform child in transform) {
			if (child.childCount > 0) {
				return false;
			} 
		}
		return true;		
	}
}
