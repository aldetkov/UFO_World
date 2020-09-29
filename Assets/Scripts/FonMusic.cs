using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FonMusic : MonoBehaviour
{
    // синглтон
    public static FonMusic instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);

     }
    
}
