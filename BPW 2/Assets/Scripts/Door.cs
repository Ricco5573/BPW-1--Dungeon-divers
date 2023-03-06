using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool open, opening;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator Open()
    {
        if (!opening)
        {
            opening = true;

            if (!open)
            {
                for (int i = 0; i < 100; i++)
                {
                    gameObject.transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y + 0.01f, gameObject.transform.rotation.z);
                    yield return new WaitForSecondsRealtime(0.0166f);

                }
                open = true;
            }
            else if (open)
            {
                for (int i = 0; i < 100; i++)
                {
                    gameObject.transform.Rotate(gameObject.transform.rotation.x, -gameObject.transform.rotation.y - 0.01f, gameObject.transform.rotation.z);
                    yield return new WaitForSecondsRealtime(0.0166f);
                }
                open = false;
            }
            opening = false;
        }

        }
    }
