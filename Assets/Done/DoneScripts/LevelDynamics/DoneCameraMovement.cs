using UnityEngine;
using System.Collections;

public enum CameraStyle
{
    NormalStyle,
    CloseStyle,
    None
}

public class DoneCameraMovement : MonoBehaviour
{
	public float smooth = 1.5f;         // The relative speed at which the camera will catch up.
    public CameraStyle cameraStyle = CameraStyle.NormalStyle;
    public float rotateSpeed=30.0f;//Ðý×ªËÙ¶È
    public float mouseMoveSpeed = 10.0f;
    public float closeMoveMargin = 0.1f;
    public Transform CloseTranform;

    private float mouseX;
    private float mouseY;
    private bool isShow;

    private Transform player;			// Reference to the player's transform.
	private Vector3 relCameraPos;		// The relative position of the camera from the player.
	private float relCameraPosMag;		// The distance of the camera from the player.
    private Vector3 closeCameraPos;
	private Vector3 newPos;				// The position the camera is trying to reach.

	
	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag(DoneTags.player).transform;
		
		// Setting the relative position as the initial relative position of the camera in the scene.
		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;
        closeCameraPos = CloseTranform.position - player.position;
	}
	
	
	void Update ()
	{
        if (cameraStyle==CameraStyle.NormalStyle)
        {
            // The standard position of the camera is the relative position of the camera from the player.
            Vector3 standardPos = player.position + relCameraPos;

            // The abovePos is directly above the player at the same distance as the standard position.
            Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;

            // An array of 5 points to check if the camera can see the player.
            Vector3[] checkPoints = new Vector3[5];

            // The first is the standard position of the camera.
            checkPoints[0] = standardPos;

            // The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
            checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
            checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
            checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);

            // The last is the abovePos.
            checkPoints[4] = abovePos;

            // Run through the check points...
            for (int i = 0; i < checkPoints.Length; i++)
            {
                // ... if the camera can see the player...
                if (ViewingPosCheck(checkPoints[i]))
                    // ... break from the loop.
                    break;
            }

            // Lerp the camera's position between it's current position and it's new position.
            transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.unscaledDeltaTime);

            // Make sure the camera is looking at the player.
            SmoothLookAt();
        }
        else if(cameraStyle==CameraStyle.CloseStyle)
        {
            newPos = CloseTranform.position;
            if (Vector3.Distance(newPos,transform.position)>closeMoveMargin)
            {
                //lerp progress
                transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.unscaledDeltaTime);
                Vector3 relPlayerPosition = player.position - newPos;
                Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.unscaledDeltaTime);
            }
            else
            {
                //move progress
                //mouse
                if (Input.GetMouseButton(1))
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
                    mouseX = Input.GetAxis("Mouse X");
                    mouseY = Input.GetAxis("Mouse Y");
                    transform.Rotate(Vector3.Slerp(Vector3.zero, new Vector3(-mouseY * mouseMoveSpeed, mouseX * mouseMoveSpeed, 0), rotateSpeed * Time.unscaledDeltaTime));
                    Debug.Log("Mouse: " + mouseX + " and " + mouseY);
                }
                else
                {
                    //keyboard
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
                    mouseX = Input.GetAxisRaw("Horizontal");
                    mouseY = Input.GetAxisRaw("Vertical");
                    transform.Rotate(Vector3.Slerp(Vector3.zero, new Vector3(-mouseY, mouseX, 0), rotateSpeed * Time.unscaledDeltaTime));
                    //Debug.Log("KeyBoard: " + mouseX + " and " + mouseY);
                }
            }
        }
    }

	
	
	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, player.position - checkPos, out hit, relCameraPosMag))
			// ... if it is not the player...
			if(hit.transform != player)
				// This position isn't appropriate.
				return false;
		
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		newPos = checkPos;
		return true;
	}
	
	
	void SmoothLookAt ()
	{
		// Create a vector from the camera towards the player.
		Vector3 relPlayerPosition = player.position - transform.position;
		
		// Create a rotation based on the relative position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.unscaledDeltaTime);
	}

    //private void Update()
    //{
    //    if(cameraStyle==CameraStyle.CloseStyle)
    //        Debug.Log("Update deltatime: " + Time.deltaTime + " unscaleDeltaTime: " + Time.unscaledDeltaTime);
    //}
}
