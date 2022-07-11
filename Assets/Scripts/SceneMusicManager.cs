using System.Collections;
using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip boss;
    [SerializeField] private AudioClip stage;

    private IEnumerator Start()
    {
        while (true)
        {
            ChangeAudioSource(GameManager.Instance.GetBoss() == null ? stage : boss);

            yield return new WaitForSeconds(4f);
        }
    }

    public void ChangeAudioSource(AudioClip clip)
    {
        if (audioSource.clip == clip) return;
        
        audioSource.clip = clip;
        audioSource.Play();
    }
}
