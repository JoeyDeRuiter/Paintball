
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float ForwardSpeed = 5f;
    public float SidewaysSpeed = 3.5f;
    public float JumpSpeed = 5f;

    public float MouseSensHor = 1f;
    public float MouseSensVert = 1f;

    public Camera PlayerCamera;
    public float CameraUpDownRange = 60f;
    private CharacterController CC;
    private Vector3 MoveDir = Vector3.zero;
    private float VerticalRotation;

	void Start () {
        // Get the local character controller
        CC = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        // Camera rotation
        float rotLeftRight = Input.GetAxis("Mouse X") * MouseSensHor;
        transform.Rotate(0, rotLeftRight, 0);

        VerticalRotation -= Input.GetAxis("Mouse Y") * MouseSensVert;
        VerticalRotation = Mathf.Clamp(VerticalRotation, -CameraUpDownRange, CameraUpDownRange);
        PlayerCamera.transform.localRotation = Quaternion.Euler(VerticalRotation, 0, 0);

        if (CC.isGrounded) {
            // Player movement
            MoveDir = new Vector3(Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ));
            MoveDir.x *= SidewaysSpeed;
            MoveDir.z *= ForwardSpeed;
            MoveDir = transform.TransformDirection(MoveDir);
            
            // Jumping
            if (Input.GetButton( "Jump" ))
                MoveDir.y = JumpSpeed;
        }

        // Gravity
        MoveDir.y += Physics.gravity.y * Time.deltaTime;

        CC.Move(MoveDir * Time.deltaTime); 
	}
}
