using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSprite : MonoBehaviour
{
    public int Speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Speed * Time.deltaTime);
    }
}
