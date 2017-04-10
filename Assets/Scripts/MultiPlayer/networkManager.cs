using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class networkManager : MonoBehaviour
{

    List<GameObject[]> cubesInServers = new List<GameObject[]>();

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(addToServer())
        {
            
        }
	}

    private bool addToServer()
    {
        foreach (GameObject[] server in cubesInServers)
        {
            if (server.Length == 1)
            {
                return true;
                
            }
        }
        return false;
    }
}
