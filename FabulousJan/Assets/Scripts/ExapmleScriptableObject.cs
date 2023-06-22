using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;



[CreateAssetMenu(fileName = "New ExapmleScriptableObject", menuName = "ExapmleScriptableObject/Example")]
public class ExapmleScriptableObject : ScriptableObject
{

    public string swordName;
    public Sprite icon;
    public bool goldCost;

    public string SwordName
    {
        get
        {
            return swordName;
        }
    }

}
