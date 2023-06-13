using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    Button btn;

    private void Awake() => btn = GetComponent<Button>();

    public void Set(Button origin)
    {
        btn.transform.position = origin.transform.position;
        btn.onClick = origin.onClick;
        //btn.onClick.AddListener(action);
    }

    public void SetEnpty()
    {
        btn.transform.position = new Vector3(-1200, 0, 0);
        btn.onClick = null;
    }

    public void SetFunc(UnityAction action) => btn.onClick.AddListener(action);
}
