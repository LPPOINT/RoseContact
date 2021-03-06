﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;
using UnityEngine;

namespace AdvancedInspector
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TerrainCollider), true)]
    public class TerrainColliderEditor : InspectorEditor
    {
        protected override void RefreshFields()
        {
            Type type = typeof(TerrainCollider);
            SerializedObject so = new SerializedObject(targets);

            fields.Add(new InspectorField(type, Instances, type.GetProperty("isTrigger"),
                new DescriptorAttribute("Is Trigger", "Is the collider a trigger?", "http://docs.unity3d.com/ScriptReference/Collider-isTrigger.html")));
            fields.Add(new InspectorField(type, Instances, type.GetProperty("terrainData"),
                new DescriptorAttribute("Terrain Data", "The terrain that stores the heightmap.", "http://docs.unity3d.com/ScriptReference/TerrainCollider-terrainData.html")));
            fields.Add(new InspectorField(type, Instances, so.FindProperty("m_CreateTreeColliders"),
                new DescriptorAttribute("Create Tree Colliders", "", "")));
        }
    }
}