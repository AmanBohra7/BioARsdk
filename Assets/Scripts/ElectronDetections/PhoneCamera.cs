﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {

	private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;

	public RawImage background;
	public AspectRatioFitter fit;
	public bool frontFacing;

    WebCamDevice[] devices;
	
	// Use this for initialization
	void Start () {
		defaultBackground = background.texture;
		devices = WebCamTexture.devices;

		if (devices.Length == 0)
			return;

		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];

			if (curr.isFrontFacing == frontFacing)
			{
				cameraTexture = new WebCamTexture(curr.name, Screen.width, Screen.height);
				break;
			}
		}	

		if (cameraTexture == null)
			return;

		cameraTexture.Play (); // Start the camera
		background.texture = cameraTexture; // Set the texture

		camAvailable = true; // Set the camAvailable for future purposes.


        
	}
	
	// Update is called once per frame
	void Update () {
		if (!camAvailable)
			return;

		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
		fit.aspectRatio = ratio; // Set the aspect ratio

		float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
		background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera

		int orient = -cameraTexture.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3(0,0, orient);

	}

      public void FlipCamera(){
        
        frontFacing = !frontFacing;
        camAvailable = false;
        if(devices.Length == 0){
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        for(int i = 0 ; i < devices.Length ; ++i){
            if(frontFacing)
                if(devices[i].isFrontFacing){
                    cameraTexture = new WebCamTexture(devices[i].name,Screen.width,Screen.height);
                }
            else
                if(!devices[i].isFrontFacing){
                    cameraTexture = new WebCamTexture(devices[i].name,Screen.width,Screen.height);
                }
        }

        if(cameraTexture == null){
            // No back camera
            Debug.Log("No back camera");
            return;
        }

        cameraTexture.Play();
        background.texture = cameraTexture;

        camAvailable = true;
    }
}



  