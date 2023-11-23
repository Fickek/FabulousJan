using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ExapmleScriptableObjectData : MonoBehaviour
{

    [SerializeField] public ExapmleScriptableObject _swordName;


    private void OnMouseDown()
    {
        Debug.Log(_swordName.SwordName);
    }

}
