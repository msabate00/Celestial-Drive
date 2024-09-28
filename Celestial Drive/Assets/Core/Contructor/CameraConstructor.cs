using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstructor : MonoBehaviour
{
    public float maxZoomOut, minZoomOut;
    private Vector3 pCenter = new Vector3(0,0,0);
    public float movementSpeed, sensibilitySpeed;

    private float yRotation, xRotation;

    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Movement();
        RotationCamera();

    }


    private void Movement() {

        float fAxis = Input.GetAxisRaw("Forward");
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        //moveDirection = ((transform.forward * fAxis + transform.right * hAxis + transform.up * vAxis) * movementSpeed) * Time.deltaTime;
        moveDirection = ((transform.forward * fAxis + transform.right * hAxis) * movementSpeed) * Time.deltaTime;
        moveDirection.y +=  ((vAxis * movementSpeed) * Time.deltaTime);

        transform.position += moveDirection;
        
    }


    private void RotationCamera() {


        if (Input.GetButton("Fire2")) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxisRaw("Mouse X") * sensibilitySpeed * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensibilitySpeed * Time.deltaTime;


            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);



        }
        else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }

}
