using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
	
	public static float top;
	public static float bottom;
	public static float right;
	public static float left;

	// Use this for initialization
	void Start () {
		top = 9;///this.transform.position.z + this.transform.lossyScale.z;
		bottom = -9;//this.transform.position.z - this.transform.lossyScale.z;
		right = 16;//this.transform.position.x + this.transform.lossyScale.x;
		left = -16;//this.transform.position.x - this.transform.lossyScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
