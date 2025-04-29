using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public static bool canAnimate = true;

    public void ResetCanAnimate()
    {     
        canAnimate = true;       
    }

    public void SetCanAnimate()
    {      
        canAnimate = false;       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
