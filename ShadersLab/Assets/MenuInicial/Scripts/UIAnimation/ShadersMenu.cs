using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShadersMenu : MonoBehaviour
{
    [SerializeField] private GameObject telaAnterior;
    
    
    private VisualElement grid;

    private Dictionary<String, Dictionary<String, int?>> leans;

    // Start is called before the first frame update
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Label>("BackArrow").RegisterCallback<ClickEvent>((type) => TransicaoDeTelaAnterior(root));

        grid = root.Q<VisualElement>("Grid");
        
        grid.Clear();

        leans = new Dictionary<string, Dictionary<String, int?>>();
        
        List<VisualElement> elementosAAdicionar = new List<VisualElement>();
        
        elementosAAdicionar.Add(CriarGridElement("teste","Dissolve"));
        elementosAAdicionar.Add(CriarGridElement("teste1","Dissolve1"));
        elementosAAdicionar.Add(CriarGridElement("teste2","Dissolve2"));
        elementosAAdicionar.Add(CriarGridElement("teste3","Dissolve3"));
        elementosAAdicionar.Add(CriarGridElement("teste4","Dissolve4"));
        elementosAAdicionar.Add(CriarGridElement("teste5","Dissolve5"));

        AdicionarElementosATela(elementosAAdicionar);
    }

    VisualElement CriarGridElement(String name, String shaderName)
    {
        VisualElement root = new VisualElement();
        root.name = name;
        root.AddToClassList("GridElement");
        
        VisualElement poli = new VisualElement();
        poli.name = "poli";
        poli.AddToClassList("Poli");
        
        root.Add(poli);
        
        Label nameElement = new Label();
        nameElement.name = "name";
        nameElement.text = shaderName;
        nameElement.AddToClassList("Name");
        
        root.Add(nameElement);
        
        VisualElement icone = new VisualElement();
        icone.name = "icone";
        icone.AddToClassList("Icone");
        
        root.Add(icone);
        
        VisualElement interactiveBox = new VisualElement();
        interactiveBox.name = "interactiveBox";
        interactiveBox.AddToClassList("InteractiveBox");

        Dictionary<String, int?> dici = new Dictionary<String, int?>();
        
        dici.Add("Expandir",null);
        dici.Add("MoverIcone",null);
        dici.Add("TirarNome",null);
        
        dici.Add("MoverPoligono",null);
        dici.Add("GirarPoligono",null);
        
        leans.Add(name,dici);

        interactiveBox.RegisterCallback<MouseOverEvent>((type) =>
        {
            foreach (String efeito in new List<String>{ "Expandir","MoverIcone","TirarNome","MoverPoligono","GirarPoligono"})
            {
                if (leans[name][efeito] != null)
                {
                    LeanTween.cancel((int)leans[name][efeito]);
                    leans[name][efeito] = null;
                }
            }
            
            grid.Remove(root);
            grid.Add(root);
            
            leans[name]["Expandir"] = LeanTween.value(root.style.scale.value.value.x, 1.2f, 1).setEaseOutExpo().setOnUpdate((float scale) =>
            {
                root.style.scale = new Scale(Vector3.one * scale);
            }).uniqueId;
            
            leans[name]["MoverIcone"] = LeanTween.value(gameObject,new Vector2(icone.style.left.value.value,icone.style.bottom.value.value), new Vector2(4f,284f), 0.5f).setEaseOutExpo().setOnUpdate((Vector2 leftAndBottom) =>
            {
                icone.style.left = leftAndBottom.x;
                icone.style.bottom = leftAndBottom.y;
            }).uniqueId;
            
            leans[name]["TirarNome"] = LeanTween.value(nameElement.style.opacity.value, 0f, .2f).setEaseOutExpo().setOnUpdate((float alpha) =>
            {
                nameElement.style.opacity = alpha;
            }).uniqueId;
            
            leans[name]["MoverPoligono"] = LeanTween.value(gameObject,new Vector2(poli.style.left.value.value,poli.style.bottom.value.value), new Vector2(-430f,257f), 1f).setEaseOutExpo().setOnUpdate((Vector2 posicao) =>
            {
                poli.style.left = posicao.x;
                poli.style.bottom = posicao.y;
            }).uniqueId;
            
            leans[name]["GirarPoligono"] = LeanTween.value(poli.style.rotate.value.angle.value, 180f, 1f).setEaseOutExpo().setOnUpdate((float rotation) =>
            {
                poli.style.rotate = new Rotate(rotation);
            }).uniqueId;
        });
            
        interactiveBox.RegisterCallback<MouseOutEvent>((type1) =>
        {
            foreach (String efeito in new List<String>{ "Expandir","MoverIcone","TirarNome","MoverPoligono","GirarPoligono"})
            {
                if (leans[name][efeito] != null)
                {
                    LeanTween.cancel((int)leans[name][efeito]);
                    leans[name][efeito] = null;
                }
            }
            
            leans[name]["Expandir"] = LeanTween.value(root.style.scale.value.value.x, 1f, 1).setEaseOutExpo().setOnUpdate((float scale) =>
            {
                root.style.scale = new Scale(Vector3.one * scale);
            }).uniqueId;
            
            leans[name]["MoverIcone"] = LeanTween.value(gameObject,new Vector2(icone.style.left.value.value,icone.style.bottom.value.value), new Vector2(21f,75f), 0.5f).setEaseOutExpo().setOnUpdate((Vector2 leftAndBottom) =>
            {
                icone.style.left = leftAndBottom.x;
                icone.style.bottom = leftAndBottom.y;
            }).uniqueId;
            
            leans[name]["TirarNome"] = LeanTween.value(nameElement.style.opacity.value,1f, .4f).setEaseOutExpo().setOnUpdate((float alpha) =>
            {
                nameElement.style.opacity = alpha;
            }).uniqueId;
            
            leans[name]["MoverPoligono"] = LeanTween.value(gameObject,new Vector2(poli.style.left.value.value,poli.style.bottom.value.value), new Vector2(-67.33f,0f), 1f).setEaseOutExpo().setOnUpdate((Vector2 posicao) =>
            {
                poli.style.left = posicao.x;
                poli.style.bottom = posicao.y;
            }).uniqueId;
            
            leans[name]["GirarPoligono"] = LeanTween.value(poli.style.rotate.value.angle.value, 0f, 1f).setEaseOutExpo().setOnUpdate((float rotation) =>
            {
                poli.style.rotate = new Rotate(rotation);
            }).uniqueId;
        });

        root.Add(interactiveBox);
        
        root.style.scale = new Scale(Vector3.zero);

        return root;
    }

    void AdicionarElementosATela(List<VisualElement> elementosAAdicionar)
    {
        float y = 20f;
        float x = 55f;

        float maxRowSize = 1920 - 25f;
        
        foreach (VisualElement elementoAAdicionar in elementosAAdicionar)
        {
            grid.Add(elementoAAdicionar);
            
            elementoAAdicionar.style.left = x;
            elementoAAdicionar.style.top = y;

            x += 342f + 25f;

            if (x > maxRowSize)
            {
                x = 55f;
                y += 337f + 20f;
            
                elementoAAdicionar.style.left = x;
                elementoAAdicionar.style.top = y;
            }
        }

        LeanTween.value(0f, 1f, 1).setEaseOutExpo().setOnUpdate((float scale) =>
        {
            foreach (VisualElement elementoAAdicionar in elementosAAdicionar)
            {
                elementoAAdicionar.style.scale = new Scale(Vector3.one * scale);
            }
        });
    }

    void TransicaoDeTelaAnterior(VisualElement root)
    {
        LeanTween.value(1f, 0f, 1).setEaseOutExpo().setOnUpdate((float scale) =>
        {
            root.Query<VisualElement>(className: "GridElement").ForEach((VisualElement elemento) =>
            {
                elemento.style.scale = new Scale(Vector3.one * scale);
            });
        });

        LeanTween.value(root.Q<VisualElement>("Background").style.backgroundColor.value.a, 0f, 1).setEaseOutExpo().setOnUpdate((float alpha) =>
            root.Q<VisualElement>("Background").style.backgroundColor = new Color(0, 0, 0, alpha));

        LeanTween.value(root.Q<VisualElement>("TopBar").style.top.value.value,-6f,1).setEaseOutExpo().setOnUpdate((float top)=>root.Q<VisualElement>("TopBar").style.top = Length.Percent(top)).setOnComplete(
            () =>
            {
                gameObject.SetActive(false);
        
                telaAnterior.SetActive(true);
            });
    }
}