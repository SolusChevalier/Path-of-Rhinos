using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    #region FIELDS

    public Camera cam;
    public float speed = 0.1f;
    public float ZoomMultiplier = 1f;
    public float MinX = -10f;
    public float MaxX = 10f;
    public float MinY = -10f;
    public float MaxY = 10f;
    public float MinZ = -10f;
    public float MaxZ = 10f;
    public float MinPitch = -90f;
    public float MaxPitch = 90f;

    #endregion FIELDS

    #region UNITY METHODS

    public void Awake()
    {
        transform.position = new Vector3(0, 25, 0);
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ZoomMultiplier = 2.5f;
        }
        else
        {
            ZoomMultiplier = 1f;
        }
        MoveCam();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void MoveCam()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += move * speed * ZoomMultiplier;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinX, MaxX), 25f, Mathf.Clamp(transform.position.z, MinY, MaxY));
        cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 10;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 10, 60);
    }

    #endregion METHODS
}