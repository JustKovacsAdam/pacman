using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public GameObject ghosts;
    GhostMovement [] ghostScripts = new GhostMovement[4];
    SpriteRenderer[] ghostSprites = new SpriteRenderer[4];
    float counter = 0;
    bool scatterActive = false;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ghosts.transform.childCount; i++)
        {
            ghostScripts[i] = ghosts.transform.GetChild(i).GetComponent<GhostMovement>();
            ghostSprites[i] = ghosts.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scatterActive)
        {
            if (counter > 0)
            {
                counter -= Time.deltaTime;
            }
            else
            {
                scatterActive = false;
                counter = 0;
                StopScatterMode();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pellet")
        {
            collision.gameObject.SetActive(false);
            if (collision.name == "Special")
            {
                StartScatterMode();
            }
            
        }
    }

    public void StartScatterMode()
    {
        for (int i = 0; i < ghostScripts.Length; i++)
        {
            // ghosts still in the house or already eaten are not affected
            if (ghostScripts[i].mode == Mode.CHASE || ghostScripts[i].mode == Mode.SCATTER)
            {
                ghostScripts[i].mode = Mode.SCATTER;
                ghostSprites[i].color = new Color(0, 0, 1);
            }
        }
        counter = 5;
        scatterActive = true;
        
    }

    public void StopScatterMode()
    {
        for (int i = 0; i < ghostScripts.Length; i++)
        {
            if (ghostScripts[i].mode == Mode.SCATTER)
            {
                ghostScripts[i].mode = Mode.CHASE;
                if (i == 0)
                {
                    ghostSprites[i].color = new Color(1, 0, 0);
                }
                else if (i == 1)
                {
                    ghostSprites[i].color = new Color(0, 1, 0.8431373f);
                }
                else if (i == 2)
                {
                    ghostSprites[i].color = new Color(1, 0.6078432f, 0.7843137f);
                }
                else
                {
                    ghostSprites[i].color = new Color(1, 0.6078432f, 0);
                }
            }   
        }
    }
}
