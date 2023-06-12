using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //用户输入控制
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        //注册交互事件
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += SettingShow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingShow(InputAction.CallbackContext context)
    {
        if (context.action.name != "Setting") return;
        if (context.action.WasPerformedThisFrame())
        {
            if (SceneManager.GetSceneByName("Setting").isLoaded == false)
            {
                Time.timeScale = 0f;
                SceneManager.LoadSceneAsync("Setting", LoadSceneMode.Additive);
            }
            else
            {
                Time.timeScale = 1f;
                SceneManager.UnloadSceneAsync("Setting");
            }
        }
    }
}
