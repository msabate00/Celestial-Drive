using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputReader input;
    [SerializeField] Transform followTarget;
    [SerializeField] Transform playerModel;
    [SerializeField] float followDistance = 2f;
    [SerializeField] Vector2 movementLimit = new Vector2 (2, 2);
    [SerializeField] float movementRange =  5f;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float smoothTime = 0.2f;

    [SerializeField] float maxRoll = 15f;
    [SerializeField] float rollSpeed = 2f;
    [SerializeField] float rollDuration = 0.5f;

    Vector3 velocity;
    float roll;



    private void Awake()
    {
        input.LeftTap += OnLeftTap;
        input.RightTap += OnRightTap;
    }



    // Update is called once per frame
    void Update()
    {
        //Calcular la posicion del target basado en el follow distance y target position
        Vector3 targetPos = followTarget.position + followTarget.forward * -followDistance;

        //Aplicar smooth
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        //Calcular la nueva posicion local
        Vector3 localPos = transform.InverseTransformPoint(smoothedPos);
        localPos.x += input.Move.x * movementSpeed * Time.deltaTime * movementRange;
        localPos.y += input.Move.y * movementSpeed * Time.deltaTime * movementRange;

        //Clamp la posicion local
        localPos.x = Mathf.Clamp(localPos.x, -movementLimit.x, movementLimit.x);
        localPos.y = Mathf.Clamp(localPos.y, -movementLimit.y, movementLimit.y);

        //Update player position
        transform.position = transform.TransformPoint(localPos);


        //La rotacion coincida con la rotacion del target
        transform.rotation = followTarget.rotation;

        //Roll basado en el player input
        roll = Mathf.Lerp(roll, input.Move.x * maxRoll, Time.deltaTime * rollSpeed);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, roll);


    }



    private void OnRightTap()
    {
        BarrelRoll(1);
    }

    private void OnLeftTap()
    {
        BarrelRoll(-1);
    }

    void BarrelRoll(int direction = -1)
    {
        Debug.Log("Do a barrelRoll!");
        //animacion de barrel roll
        /*
         if (!DOTween.IsTweening(playerModel)) {
            playerModel.DOLocatrotate(
            new Vector3(
            playerModel.localEulerangles.x,
            playertodellocaleulerangles.y,
            360 * direction), rollDuration, RotateMode.LocalAaxisAdd)
            -SetEase (Ease. OutCubic) ;
        */
    }

    private void OnDestroy()
    {
        input.LeftTap -= OnLeftTap;
        input.RightTap -= OnRightTap;
    }
}
