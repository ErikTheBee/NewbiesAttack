using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool autoplay = false;
	public float speed = 10.0f;
	public float padding = 1.0f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate  = 0.2f;
	public float health = 100;
	
	float xmin;
	float xmax;
	
	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,0.0f,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,0.0f,distance));

		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
	
	void Fire ()
	{
		Vector3 startPosition = transform.position + new Vector3(0,0.5f,0);
		GameObject beam = Instantiate (projectile, startPosition, Quaternion.identity ) as GameObject ;
		beam.rigidbody2D.velocity =new Vector3 (0f,projectileSpeed, 0f);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) ){
			InvokeRepeating ("Fire",0.0001f,firingRate);
		}
		
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		else if (Input.GetKey (KeyCode.RightArrow )) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}  
		
		float newX = Mathf.Clamp (transform.position.x,  xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z );
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
