using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_EffectMgr : MonoBehaviour
{
    public static C_EffectMgr Instance;

    [SerializeField]
    public C_EffectPool efPool_Dmg;
    [SerializeField]
    public C_EffectPool efPool_N;
    [SerializeField]
    public C_EffectPool efPool_B;

    [SerializeField]
    private C_Effect orgin_Dmg;
    [SerializeField]
    private C_Effect orgin_N;
    [SerializeField]
    private C_Effect orgin_B;
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
    void Start()
    {
        efPool_Dmg.Init(orgin_Dmg);
        efPool_N.Init(orgin_N);
        efPool_B.Init(orgin_B);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
