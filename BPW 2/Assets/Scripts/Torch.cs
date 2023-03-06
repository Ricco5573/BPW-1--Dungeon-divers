using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{

    public int dura = 300;

    void Start()
    {
        
    }

    public int GetDura()
    {
        return dura;
    }
    private IEnumerator Decay()
    {
        while (dura >= 0)
        {
            dura--;
            yield return new WaitForSecondsRealtime(1);
        }
        Destroy(this.gameObject);

        yield return null;
    }

    // Update is called once per frame

}
