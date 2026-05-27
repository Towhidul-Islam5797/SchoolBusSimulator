using UnityEngine;
using UnityEngine.InputSystem;

public class BusSimpleMove : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 10f;
    public float turnSpeed = 60f;

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float move = 0f;
        if (keyboard.wKey.isPressed) move = 1f;
        if (keyboard.sKey.isPressed) move = -1f;

        float turn = 0f;
        if (keyboard.aKey.isPressed) turn = -1f;
        if (keyboard.dKey.isPressed) turn = 1f;

        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
    }
}