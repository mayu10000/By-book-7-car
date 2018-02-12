using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
	
	public WheelCollider wcFrontLeft;
	public WheelCollider wcFrontRight;
	public WheelCollider wcRearLeft;
	public WheelCollider wcRearRight;

	
	public Vector3 centerOfMass;
	public float enginePower = 2000f;
	public float brakePower = 1000f;
	public float steerAngleMax = 25f;
	
	private bool isAccelOn;
	private bool isFoward;
	private bool isBack;
	private bool isGoButtonOn;
	private bool isBackButtonOn;
	private float horizontalInput;

	private Rigidbody _rigidbody;
	
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
		_rigidbody.centerOfMass = centerOfMass;
	}

	void Update () {
		isFoward = Input.GetKey (KeyCode.W) || isGoButtonOn;
		isBack = Input.GetKey (KeyCode.S) || isBackButtonOn;
		isAccelOn = isFoward || isBack;
		horizontalInput = Input.GetAxis("Horizontal") + -Input.acceleration.y;
	}

	void FixedUpdate () {
		
		wcFrontLeft.brakeTorque = 0;
		wcFrontRight.brakeTorque = 0;
		wcRearLeft.brakeTorque = 0;
		wcRearRight.brakeTorque = 0;
		if (isFoward) {
			float torue = enginePower;
			float torueIdle = enginePower * 0.2f;
			wcFrontLeft.motorTorque = (wcFrontLeft.isGrounded) ? torue : torueIdle;
			wcFrontRight.motorTorque = (wcFrontRight.isGrounded) ? torue : torueIdle;
			wcRearLeft.motorTorque = (wcRearLeft.isGrounded) ? torue : torueIdle;
			wcRearRight.motorTorque = (wcRearRight.isGrounded) ? torue : torueIdle;
		} else if (isBack) {
			float torue = enginePower * -0.8f;
			float torueIdle = enginePower * -0.2f;
			wcFrontLeft.motorTorque = (wcFrontLeft.isGrounded) ? torue : torueIdle;
			wcFrontRight.motorTorque = (wcFrontRight.isGrounded) ? torue : torueIdle;
			wcRearLeft.motorTorque = (wcRearLeft.isGrounded) ? torue : torueIdle;
			wcRearRight.motorTorque = (wcRearRight.isGrounded) ? torue : torueIdle;
		} else {
			wcFrontLeft.motorTorque = 0;
			wcFrontRight.motorTorque = 0;
			wcRearLeft.motorTorque = 0;
			wcRearRight.motorTorque = 0;
			
			wcFrontLeft.brakeTorque = brakePower;
			wcFrontRight.brakeTorque = brakePower;
			wcRearLeft.brakeTorque = brakePower;
			wcRearRight.brakeTorque = brakePower;
		}
		
		float angle = horizontalInput * steerAngleMax;
		wcFrontLeft.steerAngle = angle;
		wcFrontRight.steerAngle = angle;
	}
	
	public bool IsAccelOn () {
		return isAccelOn;
	}
	
	public bool IsWheelGrounded () {
		return wcFrontLeft.isGrounded || wcFrontRight.isGrounded 
			|| wcRearLeft.isGrounded || wcRearRight.isGrounded;
	}
	
	public float GetEngineRevolution () {
		return Mathf.Abs(wcFrontLeft.rpm + wcFrontRight.rpm + wcRearLeft.rpm + wcRearRight.rpm) * 0.25f;
	}

	public void OnGoButtonPressed() {
		isGoButtonOn = true;
	}

	public void OnGoButtonReleased() {
		isGoButtonOn = false;
	}

	public void OnBackButtonPressed() {
		isBackButtonOn = true;
	}

	public void OnBackButtonReleased() {
		isBackButtonOn = false;
	}
}
