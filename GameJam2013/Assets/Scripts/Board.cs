using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
	
	public static float top;
	public static float bottom;
	public static float right;
	public static float left;
	public GameObject hole;

	// Use this for initialization
	void Start () {
		top = 9;///this.transform.position.z + this.transform.lossyScale.z;
		bottom = -9;//this.transform.position.z - this.transform.lossyScale.z;
		right = 16;//this.transform.position.x + this.transform.lossyScale.x;
		left = -16;//this.transform.position.x - this.transform.lossyScale.x;
		GameObject pit = Resources.Load("Prefabs/Pit") as GameObject;
		GameObject pt = Instantiate(pit, this.transform.position , transform.rotation) as GameObject;
		hole = Resources.Load("Prefabs/Hole") as GameObject;
		//GameObject hl = Instantiate(hole, this.transform.position - Vector3.right*10 , transform.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if( 0 == Random.Range(0, Constants.HoleSpawnChance))
		{
			GameObject hl = Instantiate(hole, Vector3.right*Random.Range(left, right) + Vector3.forward*Random.Range(bottom, top), transform.rotation) as GameObject;
		}	
	}
}
