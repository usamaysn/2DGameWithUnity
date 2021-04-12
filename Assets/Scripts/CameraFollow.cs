using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float timeOffset;

    [SerializeField]
    Vector2 posOffset;


    void Start()
    { 
    
    
    }

    void Update()
    {



        Vector3 startPos = transform.position;
        Vector3 endPos = player.transform.position;


        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -10;


        transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);



        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    
    }

}
