using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Shader", menuName = "ScriptableObjects/ShaderInfos", order = 1)]
public class ShaderInfos : ScriptableObject
{
    public String Nome;
    public TipoInfos Tipo;
    public Sprite MainImage;
}