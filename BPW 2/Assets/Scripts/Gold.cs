using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField]
    private int value;
    
    public int GetValue()
    {
        return value;
    }
    public void SetValue(int amount)
    {
        value = amount;
    }

}
