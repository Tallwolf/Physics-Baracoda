using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {
	
	float timer;
	Vector3 direction;
	Collider spawner;

	// Use this for initialization
	void Start () {
		timer = Constants.windLifeSpan;
		direction = transform.TransformDirection(Vector3.forward);
	}
	
	public void SetSpawner( Collider spawn )
	{
		spawner = spawn;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			Destroy(gameObject);
			Destroy(this);
		}
	}
	
	void OnTriggerStay(Collider other) {
		Wind goAsWind = other.gameObject.GetComponent<Wind>();
		if(spawner != null && goAsWind == null && other != spawner)
		{
        	other.transform.Translate(direction*Constants.windForce, Space.World);
		}
    }
}
