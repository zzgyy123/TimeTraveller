using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchCamera : MonoBehaviour {
    private DoneCameraMovement cameraMovement;
    private DonePlayerMovement playerMovement;
    private GameObject MainCamera;
    private GameObject player;

	// Use this for initialization
	void Awake () {
        MainCamera = GameObject.FindGameObjectWithTag(DoneTags.mainCamera);
        player= GameObject.FindGameObjectWithTag(DoneTags.player);
        cameraMovement = MainCamera.GetComponent<DoneCameraMovement>();
        playerMovement = player.GetComponent<DonePlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetButtonDown("SwitchCamera"))
        {
            Debug.Log("SwitchCamera Down!");
            if (cameraMovement.cameraStyle == CameraStyle.NormalStyle)
            {
                cameraMovement.cameraStyle = CameraStyle.CloseStyle;
                playerMovement.enabled = false;
                player.GetComponent<AudioSource>().enabled = false;
                //Vector3 relPlayerPosition = player.transform.position - transform.position;
                //Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
                //MainCamera.transform.rotation = lookAtRotation;
                Time.timeScale = 0f;
            }
            else
            {
                cameraMovement.cameraStyle = CameraStyle.NormalStyle;
                playerMovement.enabled = true;
                player.GetComponent<AudioSource>().enabled = true;
                Time.timeScale = 1.0f;
            }
        }
	}
}
