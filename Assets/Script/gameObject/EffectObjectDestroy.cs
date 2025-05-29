using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectDestroy : MonoBehaviour
{

    [SerializeField]
    private float destroyTime = 1.0f;
    private bool doOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        doOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (doOnce == false)
        {
            Invoke(nameof(DestroyObject), destroyTime);
            doOnce = true;
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
