using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookDirection = Vector3.forward;
	private float moveSpeed = 10.0f;
	private float speedSmoothing = 10.0f;
	private float rotateSpeed = 500.0f;
	private bool isControllable = true;
	private Vector3 movement = Vector3.zero;
	public int charNum = 1;
	private string xAxis;
	private string yAxis;
	private string lyAxis;
	private string lxAxis;

	// Use this for initialization
	void Start () {
		moveDirection = transform.TransformDirection(Vector3.forward);
		xAxis = "Y" + charNum;
		yAxis = "X" + charNum;
		lxAxis = "LX" + charNum;
		lyAxis = "LY" + charNum;
	}
	
	// Update is called once per frame
	void Update () {
		
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
		
		// Target direction relative to the camera
		Vector3 TarMoveDir = ix * right + iy * forward;
		Vector3 TarLookDir = ilx * right + ily * forward;
		
		moveDirection = TarMoveDir.normalized;
		
		if( TarLookDir != Vector3.zero )
		{
			lookDirection = Vector3.RotateTowards(lookDirection, TarLookDir, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
			lookDirection = lookDirection.normalized;
		}
		
		// Calculate actual motion
		movement.Set(0.0f,0.0f,0.0f);
		//movement = transform.TransformDirection(moveDirection);
		movement = moveDirection;
		movement *= moveSpeed;
		movement *= Time.deltaTime;
		
		CharacterController controller = GetComponent<CharacterController>();
		CollisionFlags collisionFlags = controller.Move(movement);
		
		transform.rotation = Quaternion.LookRotation(lookDirection);
		
	}
}
