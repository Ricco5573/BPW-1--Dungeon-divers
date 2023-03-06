using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour
{
    private int damage = 1;
    [SerializeField]
    private int durability = 100;
    // Start is called before the first frame update
    public int GetDamage()
    {
        return damage;
    }
    public int GetDurability()
    {
        return durability;
    }
    public void SetDurability(int number, bool adding, bool flat)
    {
        if (adding)
        {
            durability += number;
        }
        else if (!adding && !flat)
        {
            durability -= number;
            if (durability < 75)
            {

                if (Random.Range(0, 500) > durability * 10)
                {

                    Destroy(this.gameObject);
                }

            }
        }
        else if (flat)
        {
            durability = number;
        }

    }
}

