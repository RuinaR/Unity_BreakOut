using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Destroy : MonoBehaviour
{
    [SerializeField]
    private C_Ball m_cBall;
    [SerializeField]
    private C_Line m_line;
    [SerializeField]
    private GameObject Dark;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Ball")
		{
            //Time.timeScale = 0.7f;
            m_cBall.SetSpeed(C_BaseDataMgr.Instance.GetLevelData(C_GameMgr.Instance.GetLevel()).fDefaultBallSpeed / 5.0f);
            m_cBall.GetRb().velocity = m_cBall.GetDir() * m_cBall.GetSpeed();
            //m_line.DrawLine();
            Dark.SetActive(true);
            m_cBall.DrawLine();
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
