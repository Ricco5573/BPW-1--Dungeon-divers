using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private Light light;
    [SerializeField]
    private CapsuleCollider box;
    [SerializeField]
    private string destination;
    [SerializeField]
    private ParticleSystem swirl, sparks, spook;

    [SerializeField]
    private UIManager ui;
    [SerializeField]
    private bool enabled = false;
    [SerializeField]
    private int cost;

    // Start is called before the first frame update
    void Start()
    {
        swirl.Stop();
        sparks.Stop();
        spook.Stop();
        light.enabled = false;
        EnablePortal();
    }

    public int GetCost()
    {
        return cost;
    }
    public int TogglePortal()
    {
        if (enabled)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
        EnablePortal();
        return cost;


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        StartCoroutine(MoveScene()); //i have no clue why i have to do this to call the function from another script, but it is what it is.
    }

    private IEnumerator MoveScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.gameObject.GetComponent<Player>().SetValues();
        ui.StartTransition(false);
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene(destination);
    }
    public void EnablePortal()
    {
        if (enabled)
        {
            swirl.Play();
            sparks.Play();
            spook.Play();
            light.enabled = true;
            enabled = true;
            box.enabled = true;
        }
        else
        {
            swirl.Stop();
            sparks.Stop();
            spook.Stop();
            light.enabled = false;
            enabled = false;
            box.enabled = false;
        }
    }
}
