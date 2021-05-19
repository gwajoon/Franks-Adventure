using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void setHP(float hpNormalised)
    {
        health.transform.localScale = new Vector3(hpNormalised, 1f);
    }

    public IEnumerator setHPSmooth(float newHp)
    {
        float currHP = health.transform.localScale.x;
        float changeAmt = currHP - newHp;

        while (currHP - newHp > Mathf.Epsilon)
        {
            currHP -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(currHP, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHp, 1f);
    }
}
