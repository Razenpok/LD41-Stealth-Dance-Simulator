﻿using System.Linq;
using UnityEditor;

[CustomEditor(typeof(FloorPainter))]
public class FloorPainterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var dancer = (FloorPainter)target;
        var styles = DanceStyle.AllStyles.ToList();
        var currentStyle = styles
            .Select(s => new { s.Name, Index = styles.IndexOf(s) })
            .FirstOrDefault(s => dancer.DanceStyle != null && dancer.DanceStyle.Name.Equals(s.Name));
        styles.Add(new DanceStyle { Name = "Custom" });
        var currentStyleIndex = currentStyle == null ? styles.Count - 1 : currentStyle.Index;
        var names = styles.Select(s => s.Name).ToArray();
        var selectedStyleIndex = EditorGUILayout.Popup(currentStyleIndex, names);
        if (currentStyleIndex == selectedStyleIndex) return;
        dancer.DanceStyle = styles[selectedStyleIndex];
    }
}