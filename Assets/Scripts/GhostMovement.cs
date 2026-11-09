using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Rigidbody2D ghost;
    public GameObject player;
    ResetGhosts reset;
    
    
    BoxCollider2D [] hitboxes = new BoxCollider2D[4];
    bool[] contacts = { false, false, false, false };
    
    Vector2 direction = new Vector2(0, 0);
    Vector2 lastTurnPos;

    public Mode mode;
    public float speed;
    bool switched = false;
    string facing = "LEFT";

    void Start()
    {
        hitboxes[0] = gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>();
        hitboxes[1] = gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>();
        hitboxes[2] = gameObject.transform.GetChild(4).GetComponent<BoxCollider2D>();
        hitboxes[3] = gameObject.transform.GetChild(5).GetComponent<BoxCollider2D>();
        reset = gameObject.transform.parent.gameObject.GetComponent<ResetGhosts>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.CHASE)
        {
            switched = false;
            float[] distances = PathLenght(SelectTarget());
            if (Vector2.Distance(lastTurnPos, transform.position) > 0.4f)
            {
                ChooseChaseDirection(distances);
            }
            DirectionToVector();
        }
        else if (mode == Mode.SCATTER)
        {
            if (!switched)
            {
                lastTurnPos = new Vector2(-0.2f, 0.2f);
                SwitchDirection();
                switched = true;
            }
            float[] distances = PathLenght(new Vector3(3.4f, 4.2f) - player.transform.position + new Vector3(-3.8f, -4.2f));
            if (Vector2.Distance(lastTurnPos, transform.position) > 0.4f)
            {
                ChooseChaseDirection(distances);
            }
            DirectionToVector();
        }
        else if (mode == Mode.RETREAT)
        {
            float[] distances = PathLenght(new Vector3(-0.2f, 1f));
            if (Vector2.Distance(lastTurnPos, transform.position) > 0.4f)
            {
                ChooseChaseDirection(distances);
            }
            DirectionToVector();
            if (Vector2.Distance(gameObject.transform.position, new Vector2(-0.2f, 1f)) < 0.4f)
            {
                mode = Mode.CHASE;
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = PickColor();
            }
        }

    }
    void FixedUpdate()
    {
        ghost.MovePosition(ghost.position + direction * speed * Time.fixedDeltaTime);
    }

    public Vector2 SelectTarget()
    {
        if (gameObject.name == "Red")
        {
            return player.transform.position;

        }
        else if (gameObject.name == "Blue")
        {
            GameObject red = gameObject.transform.parent.GetChild(0).gameObject;
            Vector2 mirror = red.transform.position - player.transform.position;
            Debug.Log(mirror);
            return mirror;
        }
        else if (gameObject.name == "Pink")
        {
            if (player.transform.rotation.z < 90)
            {
                return player.transform.position + new Vector3(0, 0.8f);
            }
            else if(player.transform.rotation.z < 180)
            {
                return player.transform.position + new Vector3(0.8f, 0);
            }
            else if (player.transform.rotation.z < 270)
            {
                return player.transform.position + new Vector3(0, -0.8f);
            }
            else
            {
                return player.transform.position + new Vector3(-0.8f, 0);
            }
        }
        else
        {
            if (Vector2.Distance(gameObject.transform.position, player.transform.position) > 2)
            {
                return player.transform.position;
            }
            else
            {
                return new Vector2(-3.4f, -3.8f);
            }
        }
    }

    public Color PickColor()
    {
        if (gameObject.name == "Red")
        {
            return new Color(1, 0, 0);

        }
        else if (gameObject.name == "Blue")
        {
            return new Color(0, 1, 0.8431373f);
        }
        else if (gameObject.name == "Pink")
        {
            return new Color(1, 0.6078432f, 0.7843137f);
        }
        else
        {
            return new Color(1, 0.6078432f, 0);
        }

    }

    public float[] PathLenght(Vector2 position)
    {
        float[] distances = { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };
        if (!contacts[0])
        {
            distances[0] = Vector2.Distance(hitboxes[0].transform.position, position);
        }
        if (!contacts[1])
        {
            distances[1] = Vector2.Distance(hitboxes[1].transform.position, position);
        }
        if (!contacts[2])
        {
            distances[2] = Vector2.Distance(hitboxes[2].transform.position, position);
        }
        if (!contacts[3])
        {
            distances[3] = Vector2.Distance(hitboxes[3].transform.position, position);
        }

        return distances;
    }

    public void SwitchDirection()
    {
        if (facing == "LEFT")
        {
            facing = "RIGHT";
        }
        else if (facing == "UP")
        {
            facing = "DOWN";
        }
        else if (facing == "RIGHT")
        {
            facing = "LEFT";
        }
        else
        {
            facing = "UP";
        }
    }

    public void ChooseChaseDirection(float[] distances)
    {
        string before = facing;
        if (facing == "LEFT")
        {
            if (distances[3] < distances[0] && distances[3] <= distances[1])
            {
                facing = "DOWN";
            }
            else if (distances[1] < distances[0])
            {
                facing = "UP";
            }
        }
        else if (facing == "RIGHT")
        {
            if (distances[3] < distances[2] && distances[3] <= distances[1])
            {
                facing = "DOWN";
            }
            else if (distances[1] < distances[2])
            {
                facing = "UP";
            }
        }
        else if (facing == "DOWN")
        {
            if (distances[0] < distances[3] && distances[0] <= distances[2])
            {
                facing = "LEFT";
            }
            else if (distances[2] < distances[3])
            {
                facing = "RIGHT";
            }
        }
        else
        {
            if (distances[0] < distances[1] && distances[0] <= distances[2])
            {
                facing = "LEFT";
            }
            else if (distances[2] < distances[1])
            {
                facing = "RIGHT";
            }
        }
        if (before != facing)
        {
            lastTurnPos = transform.position;
        }
    }

    public void DirectionToVector()
    {
        if (facing == "LEFT")
        {
            direction = new Vector2(-1, 0);
        }
        else if (facing == "UP")
        {
            direction = new Vector2(0, 1);
        }
        else if (facing == "RIGHT")
        {
            direction = new Vector2(1, 0);
        }
        else
        {
            direction = new Vector2(0, -1);
        }
    }

    public void SpawnGhost()
    {
        mode = Mode.CHASE;
        transform.position = new Vector2(-0.2f, 1f);
    }

    public void ResetDirection()
    {
        direction = new Vector2(0, 0);
        facing = "LEFT";
        lastTurnPos = new Vector2(-0.2f, 1); 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            if (hitboxes[0].IsTouching(collision))
            {
                contacts[0] = true;
            }
            if (hitboxes[1].IsTouching(collision))
            {
                contacts[1] = true;
            }
            if (hitboxes[2].IsTouching(collision))
            {
                contacts[2] = true;
            }
            if (hitboxes[3].IsTouching(collision))
            {
                contacts[3] = true;
            }
        }
        if (collision.tag == "Ghost" || collision.tag == "Pacman")
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(collider, collision);
        }
        if (mode == Mode.CHASE)
        {
            if ((hitboxes[0].IsTouching(collision) && collision.tag == "Pacman") ||
            (hitboxes[1].IsTouching(collision) && collision.tag == "Pacman") ||
            (hitboxes[2].IsTouching(collision) && collision.tag == "Pacman") ||
            (hitboxes[3].IsTouching(collision) && collision.tag == "Pacman"))
            {
                reset.ResetLocation();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            if (!hitboxes[0].IsTouching(other))
            {
                contacts[0] = false;
            }
            if (!hitboxes[1].IsTouching(other))
            {
                contacts[1] = false;
            }
            if (!hitboxes[2].IsTouching(other))
            {
                contacts[2] = false;
            }
            if (!hitboxes[3].IsTouching(other))
            {
                contacts[3] = false;
            }
        }
        
    }
    
}
