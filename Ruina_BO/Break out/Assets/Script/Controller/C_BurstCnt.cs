using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_BurstCnt : MonoBehaviour
{
    public void ActiveFalse()
	{
        gameObject.SetActive(false);
	}
    public void SetTextAndActive(Vector2 pos, string text)
	{
        gameObject.transform.position = pos;
        GetComponent<Text>().text = text;
        gameObject.SetActive(true);
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
