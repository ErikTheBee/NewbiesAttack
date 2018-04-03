using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	
	public GameObject projectile;
	public float missileSpeed = 2f;
	public float health = 200;
	public float shotsPerSecond = 0.5f;
	
	void Update() {
		float Probability = Time.deltaTime * shotsPerSecond;
		if (Random.value < Probability) {
			Fire ();
		}
	}
	
	void Fire() {
		Vector3 startPosition = transform.position + new Vector3(0,-1f,0);
		GameObject missile = Instantiate (projectile, startPosition , Quaternion.identity ) as GameObject ;
		missile.rigidbody2D.velocity =new Vector3 (0f,-missileSpeed, 0f);
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile) {
			health -= missile.GetDamage  ();
			missile.Hit ();
			if (health <=0) {
				Destroy (gameObject);
			}
		}
	}
}
