using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Text_DMGPool : C_Pool<C_Text_DMG>
{
    public static C_Text_DMGPool Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
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
