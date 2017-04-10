using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SimpleCubeMovement : NetworkBehaviour
{

    public float speed = 5.0f;
    public int port = 7777;

    public GameObject prefub;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        //ASDW
        if (Input.GetKey(KeyCode.D))
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.A))
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.W))
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);

        //RightLeftUpDown
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

}
