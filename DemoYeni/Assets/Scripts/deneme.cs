using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deneme : MonoBehaviour
{

    public GameObject[] MerkezObje;
    public bool don;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (don)
        {
            for (int i = 0; i < MerkezObje.Length; i++)
            {
                MerkezObje[i].transform.RotateAround(transform.position, new Vector3(0, 0, 5), -120f);
            }
            don = false;
        }
    }
}
