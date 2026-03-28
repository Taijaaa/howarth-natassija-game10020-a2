using UnityEngine;

public class ChargeSound : MonoBehaviour
{
    public BatteryCharger batteryCharger;

    AudioSource audioSource;
    //listen for Battery Charger OnBatteryCharge event
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        batteryCharger.OnBatteryCharge.AddListener(PlayAudio);
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
