using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgression : MonoBehaviour
{
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
