using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGhosts : MonoBehaviour
{
    GhostMovement[] ghostScripts = new GhostMovement[4];
    int current = 1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            ghostScripts[i] = gameObject.transform.GetChild(i).gameObject.GetComponent<GhostMovement>();
        }
        StartSpawning();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLocation()
    {
        for (int i = 0; i < ghostScripts.Length; i++)
        {
            ghostScripts[i].ResetDirection();    
            if (i == 0)
            {
                ghostScripts[i].mode = Mode.CHASE;
                ghostScripts[i].gameObject.transform.position = new Vector2(-0.2f, 1);
            }
            else
            {
                ghostScripts[i].mode = Mode.WAIT;
                ghostScripts[i].gameObject.transform.position = new Vector2(-0.6f + (i-1) * 0.4f, 0.2f);
            }
        }
        StartSpawning();

    }
    private void StartSpawning()
    {
        for (int i = 1; i < ghostScripts.Length; i++)
        {
            this.Wait(i, Spawn);
        }
        current = 1;
    }

    public void Spawn()
    {
        ghostScripts[current].gameObject.transform.position = new Vector2(-0.2f, 1);
        ghostScripts[current].mode = Mode.CHASE;
        current++;
    }

}
