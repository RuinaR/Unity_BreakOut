using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Line : MonoBehaviour
{
    private LineRenderer m_line;
    [SerializeField]
    private C_Ball m_cBall;
    [SerializeField]
    private LayerMask m_layerMask;
    private Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        m_line = GetComponent<LineRenderer>();
        m_line.startColor = Color.red;
        m_line.endColor = Color.red;
        m_line.startWidth = 0.1f;
        m_line.startWidth = 0.1f;
    }

    public void DrawLine()
	{
        Vector2 vec2StartPos = m_cBall.transform.position;
        Vector2 vec2Dir = m_cBall.GetDir();
        RaycastHit2D hit;
        m_line.positionCount = 1;
        m_line.SetPosition(0, m_cBall.transform.position);

        for(int i = 1; i < 5; i ++)
		{
            hit = Physics2D.Raycast(vec2StartPos, vec2Dir, Mathf.Infinity);
		
            if(!hit)
			{
                Debug.Log("레이저 충돌 실패");
                break;
			}
            Debug.Log("name : " + hit.transform.name + "position : " + hit.point);

            m_line.positionCount++;

            m_line.SetPosition(i, hit.point);
            vec2StartPos = hit.point;
            vec2Dir = Vector2.Reflect(vec2Dir, hit.normal);
            if(i > 1)
			{
                col.enabled = true;
			}
            col = hit.collider;
            col.enabled = false;

        }
        col.enabled = true;
    }
    public void ClearLine()
	{
        for(int i = 0; i < m_line.positionCount; i++)
		{
            m_line.SetPosition(i, Vector3.zero);
		}
        m_line.positionCount = 0;
	}
}