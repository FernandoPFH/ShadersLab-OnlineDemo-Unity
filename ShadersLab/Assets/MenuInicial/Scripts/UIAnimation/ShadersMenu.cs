using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShadersMenu : MonoBehaviour
{
    [SerializeField] private GameObject telaAnterior;

    private VisualElement root;
    private VisualElement grid;

    private Dictionary<String, Dictionary<String, int?>> leans;
    private Dictionary<String, Vector2> positions;
    
    private MenusIniciaisActions menusControls;

    private bool searchBarActive = false;
    
    private void Awake()
    {
        menusControls = new MenusIniciaisActions();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        menusControls.Enable();
        
        root = GetComponent<UIDocument>().rootVisualElement;
        
        menusControls.UI.Search.performed += _ => TransicaoDaBarraDeBusca();

        root.Q<Label>("BackArrow").RegisterCallback<ClickEvent>((type) => TransicaoDeTelaAnterior());

        grid = root.Q<VisualElement>("GridElements");
        
        grid.Clear();

        leans = new Dictionary<string, Dictionary<String, int?>>();

        positions = new Dictionary<string, Vector2>();
        
        List<VisualElement> elementosAAdicionar = new List<VisualElement>();
        
        elementosAAdicionar.Add(CriarGridElement("teste","Dissolve"));
        elementosAAdicionar.Add(CriarGridElement("teste1","Dissolve1"));
        elementosAAdicionar.Add(CriarGridElement("teste2","Dissolve2"));
        elementosAAdicionar.Add(CriarGridElement("teste3","Dissolve3"));
        elementosAAdicionar.Add(CriarGridElement("teste4","Dissolve4"));
        elementosAAdicionar.Add(CriarGridElement("teste5","Dissolve5"));

        AdicionarElementosATela(elementosAAdicionar);

        Dictionary<String, int?> dici = new Dictionary<String, int?>();
        
        dici.Add("AparecerBarraDePesquisa",null);
        
        leans.Add("SearchBar",dici);
    }

    private void OnDisable()
    {
        menusControls.Disable();
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
        
        dici.Add("MexerShadersPelaBarraDePesquisa",null);
        
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
            }).setOnComplete(() =>
            {
                poli.style.left = -430f;
                poli.style.bottom = 257f;
            }).uniqueId;
            
            leans[name]["GirarPoligono"] = LeanTween.value(poli.style.rotate.value.angle.value, 180f, 1f).setEaseOutExpo().setOnUpdate((float rotation) =>
            {
                poli.style.rotate = new Rotate(rotation);
            }).setOnComplete(() => poli.style.rotate = new Rotate(180f)).uniqueId;
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
            }).setOnComplete(() =>
            {
                poli.style.left = -67.33f;
                poli.style.bottom = 0f;
            }).uniqueId;
            
            leans[name]["GirarPoligono"] = LeanTween.value(poli.style.rotate.value.angle.value, 0f, 1f).setEaseOutExpo().setOnUpdate((float rotation) =>
            {
                poli.style.rotate = new Rotate(rotation);
            }).setOnComplete(() => poli.style.rotate = new Rotate(0f)).uniqueId;
        });

        root.Add(interactiveBox);
        
        root.style.scale = new Scale(Vector3.zero);

        return root;
    }

    void AdicionarElementosATela(List<VisualElement> elementosAAdicionar)
    {
        float y = 0f;
        float x = 0f;

        float maxRowSize = 1810f + 30f;
        
        foreach (VisualElement elementoAAdicionar in elementosAAdicionar)
        {
            grid.Add(elementoAAdicionar);
            
            elementoAAdicionar.style.left = x;
            elementoAAdicionar.style.top = y;
            
            positions.Add(elementoAAdicionar.name,new Vector2(x,y));

            x += 342f + 25f;

            if (x > maxRowSize)
            {
                x = 0f;
                y += 337f + 20f;
            
                elementoAAdicionar.style.left = x;
                elementoAAdicionar.style.top = y;
            
                positions[elementoAAdicionar.name] = new Vector2(x,y);
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

    void TransicaoDeTelaAnterior()
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

        VisualElement searchBar = root.Q<VisualElement>("SearchBar");
        VisualElement topBar = root.Q<VisualElement>("TopBar");

        LeanTween.value(searchBar.style.top.value.value,-87f, 1f).setEaseOutExpo()
            .setOnUpdate((float top) =>
            {
                searchBar.style.top = top;
            });

        LeanTween.value(root.Q<VisualElement>("TopBar").style.top.value.value,-6f,1).setEaseOutExpo().setOnUpdate((float top)=>root.Q<VisualElement>("TopBar").style.top = Length.Percent(top)).setOnComplete(
            () =>
            {
                gameObject.SetActive(false);
        
                telaAnterior.SetActive(true);
            });
    }

    void TransicaoDaBarraDeBusca()
    {
        foreach (String efeito in new List<String>{"AparecerBarraDePesquisa"})
        {
            if (leans["SearchBar"][efeito] != null)
            {
                LeanTween.cancel((int)leans["SearchBar"][efeito]);
                leans["SearchBar"][efeito] = null;
            }
        }
        
        VisualElement searchBar = root.Q<VisualElement>("SearchBar");
        VisualElement topBar = root.Q<VisualElement>("TopBar");
        
        if (searchBarActive)
        {
            leans["SearchBar"]["AparecerBarraDePesquisa"] = LeanTween.value(searchBar.style.top.value.value ,-87f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
            {
                searchBar.style.top = top;
            }).uniqueId;
            
            root.Query<VisualElement>(className: "GridElement").ForEach((VisualElement gridElement) =>
            {
                float altura = positions[gridElement.name].y;
                
                foreach (String efeito in new List<String>{"MexerShadersPelaBarraDePesquisa"})
                {
                    if (leans[gridElement.name][efeito] != null)
                    {
                        LeanTween.cancel((int)leans[gridElement.name][efeito]);
                        leans[gridElement.name][efeito] = null;
                    }
                }
                
                leans[gridElement.name]["MexerShadersPelaBarraDePesquisa"] = LeanTween.value(gridElement.resolvedStyle.top, altura, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                {
                    gridElement.style.top = altura1;
                }).uniqueId;
            });
        }
        else
        {
            leans["SearchBar"]["AparecerBarraDePesquisa"] = LeanTween.value(searchBar.style.top.value.value ,topBar.resolvedStyle.height+20f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
            {
                searchBar.style.top = top;
            }).uniqueId;
            
            root.Query<VisualElement>(className: "GridElement").ForEach((VisualElement gridElement) =>
            {
                float altura = positions[gridElement.name].y;
                
                foreach (String efeito in new List<String>{"MexerShadersPelaBarraDePesquisa"})
                {
                    if (leans[gridElement.name][efeito] != null)
                    {
                        LeanTween.cancel((int)leans[gridElement.name][efeito]);
                        leans[gridElement.name][efeito] = null;
                    }
                }
                
                leans[gridElement.name]["MexerShadersPelaBarraDePesquisa"] = LeanTween.value(gridElement.resolvedStyle.top, altura+87f, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                {
                    gridElement.style.top = altura1;
                }).uniqueId;
            });
        }
        searchBarActive = !searchBarActive;
    }
}