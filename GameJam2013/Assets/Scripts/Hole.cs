using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour {
	
	static private Texture2D[] holeTextures;
	private MyAnimation holeAnim;
	float timer;

	// Use this for initialization
	void Start () {
		timer = Constants.HoleLifeSpan;
		this.renderer.material.color = Color.white;
		holeTextures = new Texture2D[9];
		holeTextures[0] = Resources.Load("Animations/Hole/Hole0001") as Texture2D;
		holeTextures[1] = Resources.Load("Animations/Hole/Hole0002") as Texture2D;
		holeTextures[2] = Resources.Load("Animations/Hole/Hole0003") as Texture2D;
		holeTextures[3] = Resources.Load("Animations/Hole/Hole0004") as Texture2D;
		holeTextures[4] = Resources.Load("Animations/Hole/Hole0005") as Texture2D;
		holeTextures[5] = Resources.Load("Animations/Hole/Hole0006") as Texture2D;
		holeTextures[6] = Resources.Load("Animations/Hole/Hole0007") as Texture2D;
		holeTextures[7] = Resources.Load("Animations/Hole/Hole0008") as Texture2D;
		holeTextures[8] = Resources.Load("Animations/Hole/Hole0009") as Texture2D;
		holeAnim = new MyAnimation(this.renderer, holeTextures, Constants.holeAnimSpeed, false);
		holeAnim.PlayReverse();
	}
	
	// Update is called once per frame
	void Update () {
		holeAnim.Update();
		
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			Destroy(gameObject);
			Destroy(this);
		}
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
