using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParent : MonoBehaviour
{
    public void ToggleVisability()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
