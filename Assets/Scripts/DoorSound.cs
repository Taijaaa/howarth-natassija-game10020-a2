using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public PlayerBattery playerBattery;

    AudioSource audioSource;
    //listen for Player Battery OnBatteryUpdate event
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerBattery.OnBatteryUpdate.AddListener(PlayAudio);
    }

    public void PlayAudio(bool batteryCharged)
    {
        if (!batteryCharged)
        {
            audioSource.Play();
        }
    }
}
