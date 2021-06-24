using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public LoopPath LP1;
    public LoopPath LP2;
    public LoopPath LP3;
    public LoopPath LP4;
    public LoopPath LP5;
    public LoopPath LP6;
    public LoopPath LP7;
    public LoopPath LP8;
    public Dialogue solvedDialogue;
    public GameObject IceBoulders;
    public GameObject IcePath;

    public int count = 0;
    public int loopCount;
    public int prevID;

    public bool isActivated = false;

    public void Check(int ID)
    {   
        if (ID == prevID + 1) 
            count++;
        else if (ID == 1 && prevID == 8)
            count++;
        else 
            count = 0;

        prevID = ID;

        if (count == 8) 
        {
            loopCount++;
            count = 0;
        }
    }

    void Update()
    {
        if (loopCount == 3)
            isActivated = true;

        if (isActivated)
            {
                CameraShake.Instance.shakeDuration = 2f;
                loopCount = 5;
                isActivated = false;
                IceBoulders.SetActive(false);
                IcePath.SetActive(false);

                StartCoroutine(DialogueManager.Instance.ShowDialogue(solvedDialogue));
            }
    }

}
