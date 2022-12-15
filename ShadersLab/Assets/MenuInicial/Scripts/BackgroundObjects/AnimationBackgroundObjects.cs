using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationBackgroundObjects : MonoBehaviour
{
    [SerializeField] private Material materialGreen;
    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialRed;
    
    private static readonly int CutoffHeight = Shader.PropertyToID("_CutoffHeight");

    // Start is called before the first frame update
    void Start()
    {
        SetarAlphaInicial();
        StartCoroutine("DissolverLoop");
    }

    private void OnApplicationQuit()
    {
        SetarAlphaInicial();
    }

    void SetarAlphaInicial()
    {
        materialBlue.SetFloat(CutoffHeight,-1.5f);
        materialGreen.SetFloat(CutoffHeight,-1.5f);
        materialRed.SetFloat(CutoffHeight,-1.5f);
    }

    IEnumerator DissolverLoop()
    {
        yield return new WaitForSeconds(4);
        
        LeanTween.value(-1.5f, 2, 2).setEaseLinear().setOnUpdate(MudarAlphaCilindro);
        yield return new WaitForSeconds(2);
        LeanTween.value(-1.5f, 2, 2).setEaseLinear().setOnUpdate(MudarAlphaCuboBola);
        
        yield return new WaitForSeconds(3);
        
        LeanTween.value(2, -1.5f, 2).setEaseLinear().setOnUpdate(MudarAlphaCuboBola);
        yield return new WaitForSeconds(2);
        LeanTween.value(2, -1.5f, 2).setEaseLinear().setOnUpdate(MudarAlphaCilindro);

        StartCoroutine("DissolverLoop");
    }

    void MudarAlphaCilindro(float alpha)
    {
        materialBlue.SetFloat(CutoffHeight,alpha);
    }

    void MudarAlphaCuboBola(float alpha)
    {
        materialGreen.SetFloat(CutoffHeight,alpha);
        materialRed.SetFloat(CutoffHeight,alpha);
    }
}
