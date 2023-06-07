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
        PlayerInput playerInput = playerDict[key];
        if (playerInput == null) return;
        BindPlayerInput(key, playerInput);
    }

    public static void BindPlayerInput(int playerIndex, PlayerInput playerInput)
    {
        playerDict[playerIndex] = playerInput;
        InputDevice device = inputDict[playerIndex];
        if (device == null) return;
        //InputUser.PerformPairingWithDevice(device, playerInput.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
    }

    public static InputDevice GetInputDevice(int key) => inputDict[key];

    public static bool BanInput(int playerIndex, InputDevice device)
    {
        if (device is Keyboard) return false;
        return GetInputDevice(playerIndex) != device;
    }

}
