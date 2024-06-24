using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.left * (5 * Time.deltaTime);        
    }
}
