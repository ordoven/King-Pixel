using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GenericWindow : MonoBehaviour {

    [FormerlySerializedAs("firstSelected")] public GameObject FirstSelected;

    protected EventSystem EventSystem
    {
        get
        {
            return GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    }

    public virtual void OnFocus()
    {
        EventSystem.SetSelectedGameObject(FirstSelected);
    }

    protected virtual void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()
    {
        Display(false);
    }

    protected virtual void Awake()
    {
        Close();
    }
}
