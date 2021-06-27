using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public List<string> textValue;
    public List<Text> textElements;

    void Start()
    {
        for (int i = 0; i < textElements.Count; i++) {
            textElements[i].text = textValue[i];
        }
    }

    void Update()
    {
        for (int i = 0; i < textElements.Count; i++) {
            textElements[i].text = textValue[i];
        }
    }
}
