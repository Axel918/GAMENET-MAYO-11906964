using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public PowerUpType type;
    
    public enum PowerUpType
    {
        SPEED_UP,
        SLOW_DOWN,
        KNOCK_OUT
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Destroys the object if not picked up
        //StartCoroutine(Destructor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Destructor()
    {
        yield return new WaitForSeconds(20f);

        Destroy(gameObject);
    }
}
