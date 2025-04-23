
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalFieldAttribute : PropertyAttribute
{
    public string FieldToCheck;
    public bool DesiredValue;

    public ConditionalFieldAttribute(string fieldToCheck, bool desiredValue)
    {
        FieldToCheck = fieldToCheck;
        DesiredValue = desiredValue;
    }
}

