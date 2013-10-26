using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.forward;
	private bool isControllable = true;
	private Vector3 movement = Vector3.zero;
	private int charNum;
	private string xAxis;
	private string yAxis;
	private string lyAxis;
	private string lxAxis;
	private string RTAxis;
	private string LTAxis;
	private float windPos;
	private float windCoolTime;
	
	static private GameObject wind;
	static private Texture2D[] walkTextures;
	static private Texture2D[] castTextures;
	static private Texture2D[] idleTextures;
	private MyAnimation walkAnim;
	private MyAnimation idleAnim;
	private MyAnimation castAnim;
	
	private MyAnimation curAnim;
	
	private Sprite mySprite;
	
	private static int playerNums = 1;
	
	private float deathTimer;
	
	private Vector3 spawnPoint;

	// Use this for initialization
	void Start () {
		
		deathTimer = 0.0f;
		
		mySprite = this.GetComponentInChildren<Sprite>();
		mySprite.renderer.material.color = Color.white;
		
		charNum = playerNums++;
		moveDirection = transform.TransformDirection(Vector3.forward);
		spawnPoint = transform.position;
		xAxis = "Y" + charNum;
		yAxis = "X" + charNum;
		lxAxis = "LX" + charNum;
		lyAxis = "LY" + charNum;
		RTAxis = "RT" + charNum;
		LTAxis = "LT" + charNum;
		
		if(walkTextures == null) //Assuming they all happen at once
		{
			walkTextures = new Texture2D[3];
			walkTextures[0] = Resources.Load("Animations/PlayerWalk/necro_walk_1") as Texture2D;
			walkTextures[1] = Resources.Load("Animations/PlayerWalk/necro_walk_2") as Texture2D;
			walkTextures[2] = Resources.Load("Animations/PlayerWalk/necro_walk_3") as Texture2D;
			
			castTextures = new Texture2D[4];
			castTextures[0] = Resources.Load("Animations/PlayerCast/necro_cast_1") as Texture2D;
			castTextures[1] = Resources.Load("Animations/PlayerCast/necro_cast_2") as Texture2D;
			castTextures[2] = Resources.Load("Animations/PlayerCast/necro_cast_3") as Texture2D;
			castTextures[3] = Resources.Load("Animations/PlayerCast/necro_cast_4") as Texture2D;
			
			idleTextures = new Texture2D[1];
			idleTextures[0] = Resources.Load("Animations/PlayerIdle/necro_idle_1") as Texture2D;
		}
		
		walkAnim = new MyAnimation(mySprite.renderer, walkTextures, Constants.playerAnimSpeed, true);
		idleAnim = new MyAnimation(mySprite.renderer, idleTextures, Constants.playerAnimSpeed, true);
		castAnim = new MyAnimation(mySprite.renderer, castTextures, Constants.playerAnimSpeed, false);
		
		wind = Resources.Load("Prefabs/Wind") as GameObject;
		windPos = (wind.transform.localScale.z + this.transform.localScale.z)*0.5f;
		
		curAnim = idleAnim;
		curAnim.Play();
		
		windCoolTime = 0;
	}
	
	void SwapAnim( MyAnimation toSwap )
	{
		if(curAnim != toSwap)
		{
			curAnim.Pause();
			curAnim = toSwap;
			curAnim.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(deathTimer > 0)
		{
			deathTimer -= Time.deltaTime;
			if(deathTimer <=0)
			{
				Respawn();
			}
			return;
		}
		
		//keep in the level
		float tempx = Mathf.Clamp(this.transform.position.x, Board.left, Board.right);
		float tempy = this.transform.position.y;
		float tempz = Mathf.Clamp(this.transform.position.z, Board.bottom, Board.top);
		Vector3 temp = new Vector3(tempx, tempy, tempz);
		this.transform.position = temp;
		
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
			
		Vector3 forward = -Vector3.forward;
	
		Vector3 right = Vector3.right;
		
		//get control info
		float iy = Input.GetAxisRaw(xAxis);
		float ix = Input.GetAxisRaw(yAxis);
		float ilx = Input.GetAxisRaw(lxAxis);
		float ily = Input.GetAxisRaw(lyAxis);
		float LT = Input.GetAxisRaw(LTAxis);
		float RT = Input.GetAxisRaw(RTAxis);
		
		// Target direction relative to the camera
		Vector3 TarMoveDir = ix * right + iy * forward;
		Vector3 TarLookDir = ilx * right + ily * forward;
		
		moveDirection = TarMoveDir.normalized;
		
		if(windCoolTime <= 0)
		{
			if(TarMoveDir != Vector3.zero)
			{
				SwapAnim(walkAnim);
			}
			else
			{
				SwapAnim(idleAnim);
			}
		}
		
		if( TarLookDir != Vector3.zero )
		{
			lookDirection = Vector3.RotateTowards(lookDirection, TarLookDir, Constants.rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
			lookDirection = lookDirection.normalized;
		}
		
		// Calculate actual motion
		movement.Set(0.0f,0.0f,0.0f);
		//movement = transform.TransformDirection(moveDirection);
		movement = moveDirection;
		movement *= Constants.moveSpeed;
		movement *= Time.deltaTime;

		transform.Translate(movement, Space.World);
		
		transform.rotation = Quaternion.LookRotation(lookDirection);
		
		
		if((LT > 0.5) && (windCoolTime <= 0))
		{
			GameObject go = Instantiate(wind, this.transform.position + lookDirection*windPos, transform.rotation) as GameObject;
			Wind wnd = go.GetComponent<Wind>();
			wnd.SetSpawner(this.collider);
			windCoolTime += Constants.playerWindCooldown;
			SwapAnim(castAnim);
		}
		else if (windCoolTime > 0)
		{
			windCoolTime -= Time.deltaTime;
		}
		
		curAnim.Update();
	}
	
	public void Die()
	{
		deathTimer = Constants.playerRespawnTime;
		this.transform.position = new Vector3(0.0f,0.0f, 100.0f);
	}
	
	public void Respawn()
	{
		this.transform.position = spawnPoint;
		deathTimer = 0;
	}
	
	void OnTriggerEnter(Collider other) {
		
		//Destroy(other.gameObject);
    }
}
