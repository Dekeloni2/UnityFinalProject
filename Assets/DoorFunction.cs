using System.Collections;
using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    private bool played = false;

    public void Open()
    {
        if (!played)
        {
            gameObject.SetActive(false);
        }
    }
}