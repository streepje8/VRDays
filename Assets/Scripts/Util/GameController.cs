using Openverse.Input;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public float deviceCheckTime = 0.5f;
    private void Awake()
    {
        Instance = this;
    }

    private float timer = 0f;
    
    private void FixedUpdate()
    {
        if (timer <= 0f)
        {
            OpenverseInput.UpdateDevices();
            timer = deviceCheckTime;
        }
        timer -= Time.fixedDeltaTime;
    }
}
