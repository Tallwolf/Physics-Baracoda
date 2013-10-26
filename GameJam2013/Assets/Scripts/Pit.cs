using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
		Character goAsChar = other.gameObject.GetComponent<Character>();
		if(goAsChar)
		{
			goAsChar.Die();
		}
		else
		{
			//Destroy(other.gameObject);
		}
    }
}
