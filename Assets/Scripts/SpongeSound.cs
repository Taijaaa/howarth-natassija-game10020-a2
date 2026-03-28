using UnityEngine;

public class SpongeSound : MonoBehaviour
{
    public PlayerSponge playerSponge;

    AudioSource audioSource;
    //listen for Player Sponge OnSpongeUpdate event
    private void Start()
    { 
        audioSource = GetComponent<AudioSource>();
        playerSponge.OnSpongeUpdate.AddListener(PlayAudio);
    }

    public void PlayAudio(int amount)
    {
        audioSource.Play();
    }
}
