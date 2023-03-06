using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RawImage transition;
    [SerializeField]
    private TextMeshProUGUI Pickup, gold, warning;
    private int goldAmount;
    [SerializeField]
    private Slider sliderDura, sliderHealth, sliderTorch;
    [SerializeField]
    private Image sliderDuraBackground;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(TransitionScene(true));
        warning.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnablePickup(string name, bool enabling, bool door)
    {
        if (enabling && !door)
        {
            Pickup.text = "Pickup " + name + " [E]";
        }
        else if(enabling && door)
        {
            Pickup.text = "Open " + name + " [E]";
        }
        else if (!enabling)
        {
            Pickup.text = "";
        }
    }

    public void StartTransition(bool into)
    {
        StartCoroutine(TransitionScene(into));

    }
    public void StartWarning()
    {
        StartCoroutine(Warning());
    }
    public IEnumerator Warning()
    {

        while(warning.alpha < 1)
        {
            warning.alpha += 0.1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(5);
        while (warning.alpha > 0)
        {
            warning.alpha -= 0.1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

    }
    private IEnumerator TransitionScene(bool into)
    {

        if (into)
        {

            while(transition.color.a > 0.01f)
            {
                var tempcolor = transition.color;
                tempcolor.a -= 0.1f;
                transition.color = tempcolor;
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
        if (!into)
        {
            while (transition.color.a < 1)
            {

                var tempcolor = transition.color;
                tempcolor.a +=   0.2f;
                transition.color = tempcolor;
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
    }
    public void UpdateGold(int value)
    {

        gold.text = "Gold: " + value.ToString();
    }
    public void UpdateDurability(int value, bool enabled)
    {
        if (enabled)
        {
            sliderDura.gameObject.SetActive(true);
        }
        else
            sliderDura.gameObject.SetActive(false);

        if (value < 75)
        {
            sliderDuraBackground.color = Color.red;

        }
        sliderDura.value = value/100f;

    }
    public void UpdateHealth(int Value)
    {
        sliderHealth.value = Value / 10f;
    }
    public void UpdateTorch(int value, bool enabled)
    {
        if (enabled)
        {
            sliderTorch.gameObject.SetActive(true);
            sliderTorch.value = value / 600;
        }
        else
        {
            sliderTorch.gameObject.SetActive(false);

        }
    }
}
