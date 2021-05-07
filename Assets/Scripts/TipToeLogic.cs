using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipToeLogic : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    private TipToePlatform plateScript;
    private int width = 10;
    private int depth = 13;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < depth; j++)
            {
                GameObject platform = GameObject.Instantiate(platformPrefab);
                platform.transform.position = new Vector3(-14 + i*3, 0,10+ j*3);
                if(i == 5)
                {
                    platform.GetComponent<TipToePlatform>().isPath = true;
                }
                else { 
                platform.GetComponent<TipToePlatform>().isPath = false;
                }
            }   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
