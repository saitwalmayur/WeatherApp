using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public Transform m_Icon;
    private void Update()
    {
        m_Icon.Rotate(new Vector3(0, 0, 50 * Time.deltaTime));
    }
}
