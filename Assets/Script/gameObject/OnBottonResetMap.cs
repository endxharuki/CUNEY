using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBottonResetMap : MonoBehaviour
{
    private GameObject mapObject = null;
    private bool doOnce = false;
    void Start()
    {
        doOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(doOnce == false)
        {
            mapObject = GameObject.Find("map(Clone)");
            if (mapObject != null) doOnce = true;
        }
    }

    public void MapReset()
    {
        //マップのリセット
        mapObject.GetComponent<MapManager>().MapReset();
        mapObject.GetComponent<CursorManager>().enabled = true;
        mapObject.GetComponent<TouchControl>().enabled = true;

    }
}
