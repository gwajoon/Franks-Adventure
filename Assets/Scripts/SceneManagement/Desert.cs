using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert : MonoBehaviour
{
    public BoulderHolder BH1;
    public BoulderHolder BH2;
    public BoulderHolder BH3;
    public BoulderHolder BH4;
    public BoulderHolder BH5;
    public Dialogue solvedDialogue;

    public List<GameObject> NPC;
    public List<GameObject> SNPC;

    public bool isActivated;

    void Update() 
    {
        if (BH1.activated == true && BH2.activated == true && BH3.activated == true && BH4.activated == true && BH5.activated == true)
        {   
            isActivated = true;
        }

        if (isActivated) 
        {       
            CameraShake.Instance.shakeDuration = 2f;
            BH1.activated = false;
            BH2.activated = false;
            BH3.activated = false;
            BH4.activated = false;
            BH5.activated = false;
            isActivated = false;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(solvedDialogue));

            foreach (GameObject npc in NPC) {
                npc.SetActive(false);
            }

            foreach (GameObject snpc in SNPC) {
                snpc.SetActive(true);
            }
        }
    }
}
