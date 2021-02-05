using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speedMove = 5.0f;

    public string id;
    public bool isMine;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMine)
            return;

        this.transform.position += (Vector3.right * Input.GetAxis("Horizontal")) * speedMove * Time.deltaTime;
    }
}
