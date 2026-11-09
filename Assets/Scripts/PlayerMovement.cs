using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody2D rb2d;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement.Normalize();
        if (movement != Vector2.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, movement);
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 100f);
            
        }
    }
    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement*speed*Time.fixedDeltaTime);
    }
}
