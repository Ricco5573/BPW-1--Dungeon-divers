using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private int hits = 3;
    [SerializeField]
    private Tutorial tut;
    public void Hit()
    {
        Debug.Log("Hit");
        hits--;
        if  (hits == 0)
        {
            tut.SetDummy(true);
        }
    }
}
