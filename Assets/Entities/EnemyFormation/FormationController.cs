using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 10.0f;
	public float height = 5.0f;
	public float speed = 5.0f;
	public float spawnDelay = 0.5f;

	private bool movingRight = false;
	private float xmax;
	private float xmin;
	private int padding = 0;

	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,0.0f,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,0.0f,distance));
		
		xmin = leftBoundary.x + padding;
		xmax = rightBoundary.x - padding;
		
		SpawnUntilFull ();
	}
	
	
	void SpawnUntilFull() {
		Transform freePosition = NextFreePosition ();

		if (freePosition ) {
			GameObject enemy =  Instantiate(enemyPrefab, freePosition.position  , Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;	
		}
		
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull",spawnDelay);
		}
	}
	
	public void OnDrawGizmos () {
		Gizmos.DrawWireCube (transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight ) {
			transform.position += Vector3.right * speed * Time.deltaTime; 		
		}
		else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		//Check if formation is going off screen
		float RightEdgeOfFormation = transform.position.x + (width * 0.5f);
		float LeftEdgeOfFormation = transform.position.x - (width * 0.5f);
		
		if (LeftEdgeOfFormation  < xmin ) {
			movingRight =true;
//			transform.position += new Vector3(0f, -1f, 0);
		} 
		else if (RightEdgeOfFormation > xmax)
		{
			movingRight = false;
		}

		if (AllMembersDead()) {
			Debug.Log ("All enemies dead");
			SpawnUntilFull ();
		}
	}
	
	bool AllMembersDead() {
		foreach (Transform childPositionObject in transform ) {
			if ( childPositionObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
	
	Transform NextFreePosition () {
		foreach (Transform childPositionObject in transform ) {
			if ( childPositionObject.childCount == 0){
				return childPositionObject;
			}
		}
		return null;		
	}
}