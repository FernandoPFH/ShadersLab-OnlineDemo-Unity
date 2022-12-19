using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;



public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject proximaTela;
    
    private Button _botaoIniciar;
    private VisualElement _barraDeNavegacao;
    private Label _titulo;
    private VisualElement _fundo;

    // Start is called before the first frame update
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _botaoIniciar = root.Q<Button>("StartButton");
        _botaoIniciar.clicked += () => TransicaoDeTela();
        
        _barraDeNavegacao = root.Q<VisualElement>("TopBar");
        _titulo = root.Q<Label>("Title");
        _fundo = root.Q<VisualElement>("Background");
    }

    void TransicaoDeTela()
    {
        LeanTween.value(_botaoIniciar.style.opacity.value,0f,1).setEaseOutExpo().setOnUpdate((float alpha)=>_botaoIniciar.style.opacity = alpha);
        
        LeanTween.value(_barraDeNavegacao.style.top.value.value,0f,1).setEaseOutExpo().setOnUpdate((float top)=>_barraDeNavegacao.style.top = Length.Percent(top));

        _titulo.style.unityFontStyleAndWeight = FontStyle.Normal;
        
        _titulo.style.fontSize = 48f;
        float x = _titulo.layout.width / 4f;
        float y = _titulo.layout.height / 8f;
        _titulo.style.fontSize = 100f;

        LeanTween.value(_titulo.style.fontSize.value.value,48f,1).setEaseOutExpo().setOnUpdate((float fontSize)=>_titulo.style.fontSize = fontSize);
        
        _titulo.style.position = Position.Absolute;

        LeanTween.value(gameObject,(Vector2)_titulo.layout.position, new Vector2(-x, 0),1).setEaseOutExpo().setOnUpdate((Vector2 position) =>
        {
            _titulo.style.left = position.x;
            _titulo.style.top = position.y;
        });

        LeanTween.value(gameObject, _titulo.style.color.value, Color.black, 1f).setEaseOutExpo().setOnUpdate((Color cor) =>
        {
            _titulo.style.color = cor;
        });

        LeanTween.value(_fundo.style.backgroundColor.value.a, 0.5f, 1).setEaseOutExpo().setOnUpdate((float alpha) =>
            _fundo.style.backgroundColor = new Color(0, 0, 0, alpha)).setOnComplete(TrocarTela);
    }

    void TrocarTela()
    {
        gameObject.SetActive(false);
        
        proximaTela.SetActive(true);
    }
}