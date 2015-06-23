﻿using UnityEngine;
using System;
using System.Collections;

using AdvancedInspector;

[AdvancedInspector]
public class AIExample32_Dictionary : MonoBehaviour
{
    // The UDictionary is a Unity friendly dictionary that fully implement all .NET collection/dictionary interface
    // However, Unity is unable to properly save a generic object, so we must create a non-generic version like this;
    // The Serializable attribute is also required.
    [Serializable]
    public class StringGameObjectDictionary : UDictionary<string, GameObject> { }

    // The Advanced Inspector is able to display anything having the ICollection or IDictionary interfaces.
    // Note that it is best to initialize the variable yourself in case of Dictionaries;
    [Inspect, Background(0, 1, 1), Descriptor(1, 1, 0)]
    public StringGameObjectDictionary myDictionary = new StringGameObjectDictionary();
}