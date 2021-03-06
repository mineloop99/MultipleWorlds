using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void FixedUpdate()
    {
        PlayerControllerSend();
    }

    void PlayerControllerSend()
    {
        bool[] _inputs = {
        Input.GetKey(KeyCode.W),
        Input.GetKey(KeyCode.S),
        Input.GetKey(KeyCode.A),
        Input.GetKey(KeyCode.D),
        Input.GetKey(KeyCode.Space)};
        ClientSend.PlayerMovement(_inputs);
    }
}


