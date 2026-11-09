using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGhostInteraction : MonoBehaviour
{
    public GameObject lives;
    public GameObject ghosts;
    public Text score;

    GhostMovement[] ghostScripts = new GhostMovement[4];
    GameObject[] sprites = new GameObject[3];

    int currentLife = 1;
    bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {
        sprites[0] = lives.transform.GetChild(0).gameObject;
        sprites[1] = lives.transform.GetChild(1).gameObject;
        sprites[2] = lives.transform.GetChild(2).gameObject;
        for (int i = 0; i < ghosts.transform.childCount; i++)
        {
            Debug.Log(ghosts.transform.childCount + "|" + i);
            ghostScripts[i] = gameObject.transform.GetChild(i).GetComponent<GhostMovement>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox" && !invincible)
        {
            if (collision.gameObject.transform.parent.GetComponent<GhostMovement>().mode == Mode.SCATTER ||
                collision.gameObject.transform.parent.GetComponent<GhostMovement>().mode == Mode.RETREAT)
            {
                string[] texts = score.text.Split("\n");
                texts[1] = (int.Parse(texts[1]) + 500).ToString("D6");
                score.text = texts[0] + "\n" + texts[1];
                EatGhost(collision.gameObject.transform.parent.gameObject);
            }
            else
            {
                LoseLife();
            }
            
        }
    }

    public void LoseLife()
    {

        if (currentLife == 1)
        {
            sprites[2].SetActive(false);
            currentLife++;
        }
        else if (currentLife == 2)
        {
            sprites[1].SetActive(false);
            currentLife++;
        }
        else
        {
            sprites[0].SetActive(false);
            currentLife++;
            Debug.Log("Game Over");
        }
        ResetPosition();
    }

    public void EatGhost(GameObject ghost)
    {
        GhostMovement script = ghost.GetComponent<GhostMovement>();
        SpriteRenderer sprite = ghost.transform.GetChild(0).GetComponent<SpriteRenderer>();
        script.mode = Mode.RETREAT;
        sprite.color = new Color(0, 0, 0, 0);
    }

    public void ResetPosition()
    {
        invincible = true;
        transform.position = new Vector2(-0.2f, -2.2f);
        invincible = false;
    }
}
