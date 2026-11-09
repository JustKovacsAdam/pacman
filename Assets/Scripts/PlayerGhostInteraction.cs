using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGhostInteraction : MonoBehaviour
{
    public GameObject lives;
    public GameObject ghosts;
    public Text score;
    // seconds the player can't be hit again after losing a life
    public float invincibilityTime = 1.5f;

    GameObject[] sprites = new GameObject[3];

    int livesLeft = 3;
    float invincibleUntil = 0f;

    // Start is called before the first frame update
    void Start()
    {
        sprites[0] = lives.transform.GetChild(0).gameObject;
        sprites[1] = lives.transform.GetChild(1).gameObject;
        sprites[2] = lives.transform.GetChild(2).gameObject;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Hitbox" || Time.time < invincibleUntil)
        {
            return;
        }

        GhostMovement ghost = collision.gameObject.transform.parent.GetComponent<GhostMovement>();
        if (ghost == null)
        {
            return;
        }

        if (ghost.mode == Mode.SCATTER)
        {
            string[] texts = score.text.Split("\n");
            texts[1] = (int.Parse(texts[1]) + 500).ToString("D6");
            score.text = texts[0] + "\n" + texts[1];
            EatGhost(ghost.gameObject);
        }
        else if (ghost.mode == Mode.CHASE)
        {
            LoseLife();
        }
        // RETREAT (eyes going home) and WAIT ghosts are harmless
    }

    public void LoseLife()
    {
        livesLeft--;
        if (livesLeft >= 0 && livesLeft < sprites.Length)
        {
            sprites[livesLeft].SetActive(false);
        }

        if (livesLeft <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            return;
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
        invincibleUntil = Time.time + invincibilityTime;
        transform.position = new Vector2(-0.2f, -2.2f);
    }
}
