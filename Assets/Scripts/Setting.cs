using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    public Text player1Text;
    public Text player2Text;
    private PlayerInput playerInput;
    private int playerIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnBind;
        playerIndex = 1;
    }

    public void OnBind(InputAction.CallbackContext context)
    {
        if (playerIndex > 2)
        {
            return;
        }
        InputDevice device = context.control.device;
        Debug.Log("device.name = " + device.name);
        if (playerIndex == 1)
        {
            InputManager.BindDevice(playerIndex, device);
            player1Text.text = device.name;
            player2Text.text = "ÇëÒ¡¶¯ÊÖ±ú×óÒ¡¸Ë°ó¶¨";
            playerIndex = 2;
        }
        if (playerIndex == 2)
        {
            InputDevice player1Device = InputManager.GetInputDevice(1);
            if (player1Device == device) return;
            InputManager.BindDevice(playerIndex, device);
            player2Text.text = device.name;
            playerIndex++;
        } 
    }
}
