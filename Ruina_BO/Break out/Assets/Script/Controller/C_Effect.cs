using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Effect : MonoBehaviour
{
    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    protected void OnEnable()
    {
        Invoke("SetActiveFalse", 1.0f);
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
