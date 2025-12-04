using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        NativeSdk.Instance.ShowLongToast(gameObject.name);
    }
}
