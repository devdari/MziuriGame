using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCheckpointsScript : MonoBehaviour
{
    public int checkpoint;


    public void ChooseCheckPoint()
    {
        if(EventSystem.current.currentSelectedGameObject.name == "Checkpoint1")
        {
            checkpoint = 1;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Checkpoint2")
        {
            checkpoint = 2;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
