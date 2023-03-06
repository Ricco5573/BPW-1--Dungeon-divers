using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{

    [SerializeField]
    private GameObject sword, player, gold, dummy;

    [SerializeField]
    private bool dummyDone;

    [SerializeField]
    private PortalManager portal;

    [SerializeField]
    private AudioClip one, two, three, four, five, six;

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private TextMeshProUGUI subtitle;

    private void Start()
    {
        StartCoroutine(Tutorialtime());
    }

    private IEnumerator Tutorialtime()
    {
        yield return new WaitForSecondsRealtime(3);
        source.PlayOneShot(one);
        StartCoroutine(Subtitles(1));
        yield return new WaitForSecondsRealtime(10);
        while (player.GetComponent<Player>().Getsword() == null)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        source.PlayOneShot(two);
        StartCoroutine(Subtitles(2));
        dummy.gameObject.SetActive(true);
        while (!dummyDone)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        source.PlayOneShot(three);
        StartCoroutine(Subtitles(3));
        yield return new WaitForSecondsRealtime(17);    
        dummy.gameObject.SetActive(false);
        source.PlayOneShot(four);
        StartCoroutine(Subtitles(4));
        gold.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(12);
        while (gold != null)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        source.PlayOneShot(five);
        StartCoroutine(Subtitles(5));
        yield return new WaitForSecondsRealtime(20);
        source.PlayOneShot(six);
        StartCoroutine(Subtitles(6));
        yield return new WaitForSecondsRealtime(12);
        Destroy(sword);
        portal.TogglePortal();

    }

    public IEnumerator Subtitles(int clip)
    {
        switch (clip)
        {
            case 1:
                subtitle.text = "Welcome to the magitarium recruit.";
                yield return new WaitForSecondsRealtime(3);
                subtitle.text = "Here, we will show you the ropes";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "of what it means to dungeon dive";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "First things first, pick up that sword on the ground in front of you.";
                yield return new WaitForSecondsRealtime(6);
                subtitle.text = "";
                break;
            case 2:
                subtitle.text = "Good! now take a couple of swings at the dummy!";
                yield return new WaitForSecondsRealtime(6);
                subtitle.text = "";
                break;
            case 3:
                subtitle.text = "Perfect! your wood killing skills are remarkable";
                yield return new WaitForSecondsRealtime(4);
                subtitle.text = "Although the ones down in the dungeon will fight back";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "Note that attacking took durability off of your weapon.";
                yield return new WaitForSecondsRealtime(3);
                subtitle.text = "the lower it is, the higher the chance of it breaking when you swing with it";
                yield return new WaitForSecondsRealtime(3);
                subtitle.text = "So make sure your weapon has durability before swinging";
                yield return new WaitForSecondsRealtime(6);
                subtitle.text = "";
                break;
            case 4:
                subtitle.text = "Next up is gold, the reason youre going down into the dungeon.";
                yield return new WaitForSecondsRealtime(4);
                subtitle.text = "Your job as a diver is to retrieve as much of the metal as possible";
                yield return new WaitForSecondsRealtime(3.5f);
                subtitle.text = "the money can then be spent on equipment and entrance into the dungeon";
                yield return new WaitForSecondsRealtime(5);
                subtitle.text = "";
                break;
            case 5:
                subtitle.text = "Now look behind you";
                yield return new WaitForSecondsRealtime(1.5f);
                subtitle.text = "You can see the portal there";
                yield return new WaitForSecondsRealtime(1.5f);
                subtitle.text = "you'll enter and exit dungeons through a portal";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "However, we cannot keep them open forever";
                yield return new WaitForSecondsRealtime(2.5f);
                subtitle.text = "In a real dungeon, you will have a total of 15 minutes ";
                yield return new WaitForSecondsRealtime(2.5f);
                subtitle.text = "once that time is up, you wont be coming back up";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "so be carefull not to lose track of time";
                yield return new WaitForSecondsRealtime(2.5f);
                subtitle.text = "You'll be informed when there are 5 minutes left";
                yield return new WaitForSecondsRealtime(3);
                subtitle.text = "but at that time you should already be on your way back";
                break;
            case 6:
                subtitle.text = "on the way back, you also need content with enemies that returned";
                yield return new WaitForSecondsRealtime(3);
                subtitle.text = "The magic of the dungeon is of necromantic origin";
                yield return new WaitForSecondsRealtime(3.5f);
                subtitle.text = "Any enemies slain within the dungeon will return in due time.";
                yield return new WaitForSecondsRealtime(3.5f);
                subtitle.text = "Usually just in time for you to be on your way back.";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "For now, that is all you need to know about the dungeons";
                yield return new WaitForSecondsRealtime(2);
                subtitle.text = "the portal has been reactivated, please return through there to the headquarters";
                yield return new WaitForSecondsRealtime(5);
                subtitle.text = "";


                break;

        }
        yield return null; 
    }
    public void SetDummy(bool value)
    {
        dummyDone = value;
    }
}
