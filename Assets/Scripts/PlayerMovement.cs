using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody2D rb2d;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
        {
            movement = Vector2.zero;
            return;
        }

        // Raw input so the controls respond instantly instead of slowly ramping up.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Pac-Man only moves along one axis at a time (no diagonal movement).
        if (Mathf.Abs(x) >= Mathf.Abs(y))
        {
            y = 0;
        }
        else
        {
            x = 0;
        }
        movement = new Vector2(Mathf.Sign(x) * (x != 0 ? 1 : 0), Mathf.Sign(y) * (y != 0 ? 1 : 0));

        if (movement != Vector2.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, movement);
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 100f);
        }
    }

    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
    }
}
