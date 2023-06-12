using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager
{

    private static readonly Dictionary<int, InputDevice> inputDict = new();
    private static readonly Dictionary<int, PlayerInput> playerDict = new();

    public static void BindDevice(int key, InputDevice device)
    {
        inputDict[key] = device;
    }

    public static void BindPlayerInput(int playerIndex, PlayerInput playerInput)
    {
        playerDict[playerIndex] = playerInput;
        if (!inputDict.ContainsKey(playerIndex)) return;
        InputDevice device = inputDict[playerIndex];
        playerInput.SwitchCurrentControlScheme(device);
    }

    public static void Apply()
    {
        BindPlayerInput(1, playerDict[1]);
        BindPlayerInput(2, playerDict[2]);
    }

    public static InputDevice GetInputDevice(int key) => inputDict[key];

}
