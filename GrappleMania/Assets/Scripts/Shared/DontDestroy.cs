using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    static GameObject instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }      
    }
}
