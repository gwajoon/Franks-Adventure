using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPath : MonoBehaviour
{
    public int ID;
    public GameObject Map;

    private void OnTriggerEnter2D(Collider2D other)
    {   
        Map.GetComponent<Snow>().Check(ID);
    }
}
