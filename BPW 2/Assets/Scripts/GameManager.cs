using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource endMusic;
    [SerializeField]
    private PortalManager Portal;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private Player player;

    [SerializeField]
    private AudioClip end, music;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(30);
        endMusic.Play();
        yield return new WaitForSecondsRealtime(239);
        endMusic.PlayOneShot(end);
        yield return new WaitForSecondsRealtime(31);
        uiManager.StartWarning();
        yield return new WaitForSecondsRealtime(300);
        Portal.TogglePortal();
        yield return new WaitForSecondsRealtime(10);
        player.Die();
    }
}
