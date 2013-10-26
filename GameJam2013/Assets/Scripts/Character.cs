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
	
	private static int playerNums = 1;

	// Use this for initialization
	void Start () {
		
		charNum = playerNums++;
		moveDirection = transform.TransformDirection(Vector3.forward);
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
			castTextures[0] = Resources.Load("Animations/PlayerWalk/necro_cast_1") as Texture2D;
			castTextures[1] = Resources.Load("Animations/PlayerWalk/necro_cast_2") as Texture2D;
			castTextures[2] = Resources.Load("Animations/PlayerWalk/necro_cast_3") as Texture2D;
			
			idleTextures = new Texture2D[1];
			idleTextures[0] = Resources.Load("Animations/PlayerIdle/necro_idle_1") as Texture2D;
		}
		
		walkAnim = new MyAnimation(this.renderer, walkTextures, Constants.playerAnimSpeed);
		idleAnim = new MyAnimation(this.renderer, idleTextures, Constants.playerAnimSpeed);
		castAnim = new MyAnimation(this.renderer, castTextures, Constants.playerAnimSpeed);
		
		wind = Resources.Load("Prefabs/Wind") as GameObject;
		windPos = (wind.transform.localScale.z + this.transform.localScale.z)*0.5f;
		
		curAnim = idleAnim;
		curAnim.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
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
		
		if(TarMoveDir != Vector3.zero)
		{
			curAnim.Pause();
			curAnim = walkAnim;
			curAnim.Play();
		}
		else
		{
			curAnim.Pause();
			curAnim = idleAnim;
			curAnim.Play();
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
		}
		else if (windCoolTime > 0)
		{
			windCoolTime -= Time.deltaTime;
		}
		
		
		curAnim.Update();
	}
}
