using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotionController))]
public class Fall : MonoBehaviour
{
    private CharacterMotionController mc;

    private void Awake()
    {
        mc = GetComponent<CharacterMotionController>();
    }
    private void FixedUpdate()
    {
        mc?.UpdatePosition();
    }
}
