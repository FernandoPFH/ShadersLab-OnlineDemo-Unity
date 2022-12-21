using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShadersMenu : MonoBehaviour
{
    [SerializeField] private GameObject telaAnterior;

    private VisualElement root;
    private VisualElement background;
    private VisualElement filtros;
    private VisualElement sideBar;
    private VisualElement searchBar;
    
    private Dictionary<String, Dictionary<String, int?>> leans;
    private Dictionary<String, Vector2> positions;

    private List<VisualElement> todosElementosDeShaders;
    private List<VisualElement> todosFiltrosDeShaders;

    private Dictionary<String, List<Vector2>> possiblePositions;
    private String state;

    private String whoClickedSideBar;

    private MenusIniciaisActions menusControls;

    private bool searchBarActive;
    private bool filtroBarActive;
    
    private void Awake()
    {
        menusControls = new MenusIniciaisActions();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        todosElementosDeShaders = new List<VisualElement>();
        todosFiltrosDeShaders = new List<VisualElement>();

        possiblePositions = new Dictionary<string, List<Vector2>>();
        
        state = "padrao";
        
        searchBarActive = false;
        filtroBarActive = false;
        
        
        
        whoClickedSideBar = null;
        
        menusControls.Enable();
        
        root = GetComponent<UIDocument>().rootVisualElement;
        
        menusControls.UI.Search.performed += _ => TransicaoDaBarraDeBusca();

        root.Q<Label>("BackArrow").RegisterCallback<ClickEvent>((type) => TransicaoDeTelaAnterior());

        root.Q<VisualElement>("BotaoFiltros").RegisterCallback<ClickEvent>((type) => TransicaoDaBarraDeFiltros());
            
        background = root.Q<VisualElement>("Background");

        filtros = root.Q<VisualElement>("Filtros");

        sideBar = root.Q<VisualElement>("SideBar");

        searchBar = root.Q<VisualElement>("SearchBar");

        leans = new Dictionary<string, Dictionary<String, int?>>();

        positions = new Dictionary<string, Vector2>();
        
        todosElementosDeShaders.Add(CriarGridElement("teste","Dissolve"));
        todosElementosDeShaders.Add(CriarGridElement("teste1","Dissolve1"));
        todosElementosDeShaders.Add(CriarGridElement("teste2","Dissolve2"));
        todosElementosDeShaders.Add(CriarGridElement("teste3","Dissolve3"));
        todosElementosDeShaders.Add(CriarGridElement("teste4","Dissolve4"));
        todosElementosDeShaders.Add(CriarGridElement("teste5","Dissolve5"));

        AdicionarElementosDeShadersATela();

        todosFiltrosDeShaders.Add(CriarFilterElement("teste1","Ambiente"));
        todosFiltrosDeShaders.Add(CriarFilterElement("teste2","Ambiente2"));

        AdicionarElementosDeFiltrosATela();

        Dictionary<String, int?> dici = new Dictionary<String, int?>();
        
        dici.Add("AparecerBarraDePesquisa",null);
        
        leans.Add("SearchBar",dici);

        dici = new Dictionary<String, int?>();
        
        dici.Add("AparecerBarraDeFiltros",null);
        
        leans.Add("FiltrosBar",dici);

        dici = new Dictionary<String, int?>();
        
        dici.Add("AparecerBarraDeLado",null);
        
        leans.Add("SideBar",dici);
    }

    private void OnDisable()
    {
        menusControls.Disable();
    }

    VisualElement CriarFilterElement(String name, String filterName)
    {
        VisualElement root = new VisualElement();
        root.name = name;
        root.AddToClassList("Filtro");
        
        VisualElement icone = new VisualElement();
        icone.name = "icone";
        icone.AddToClassList("Icone");
        
        root.Add(icone);
        
        Label nameElement = new Label();
        nameElement.name = "name";
        nameElement.text = filterName;
        nameElement.AddToClassList("Name");
        
        root.Add(nameElement);
        
        VisualElement interactiveBox = new VisualElement();
        interactiveBox.name = "interactiveBox";
        interactiveBox.AddToClassList("InteractiveBox");

        root.Add(interactiveBox);

        return root;
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
        dici.Add("AparecerBarraDeFiltros",null);
        dici.Add("AparecerBarraDeLado",null);
        
        leans.Add(name,dici);

        interactiveBox.RegisterCallback<MouseOverEvent>((type) =>
        {
            foreach (String efeito in new List<String>
                         { "Expandir", "MoverIcone", "TirarNome", "MoverPoligono", "GirarPoligono" })
            {
                if (leans[name][efeito] != null)
                {
                    LeanTween.cancel((int)leans[name][efeito]);
                    leans[name][efeito] = null;
                }
            }

            background.Remove(root);
            background.Add(root);

            AcaoDeEntradaESaidaDoMouse(name,root,icone, nameElement, poli,
                1.2f, new Vector2(4f, 284f), 0f, new Vector2(-430f, 257f), 180f);
        });

        interactiveBox.RegisterCallback<MouseOutEvent>((type1) =>
        {
            AcaoDeEntradaESaidaDoMouse(name,root,icone, nameElement, poli,
                1f, new Vector2(21f,75f), 1f, new Vector2(-67.33f,0f), 0f);
        });

        interactiveBox.RegisterCallback<ClickEvent>((type1) =>
        {
            foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
            {
                if (leans["SideBar"][efeito] != null)
                {
                    LeanTween.cancel((int)leans["SideBar"][efeito]);
                    leans["SideBar"][efeito] = null;
                }
            }
            
            if (whoClickedSideBar == null)
            {
                if (state == "padrao")
                {
                    leans["SideBar"]["AparecerBarraDeLado"] = leanTween(-sideBar.resolvedStyle.right, 0f, 1f,
                        (right => sideBar.style.right = right));

                    whoClickedSideBar = name;
                
                    foreach (var elemento in todosElementosDeShaders)
                    {
                        Vector2 posicao = possiblePositions["dados"][todosElementosDeShaders.IndexOf(elemento)];
                
                        foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
                        {
                            if (leans[elemento.name][efeito] != null)
                            {
                                LeanTween.cancel((int)leans[elemento.name][efeito]);
                                leans[elemento.name][efeito] = null;
                            }
                        }
                
                        leans[elemento.name]["AparecerBarraDeLado"] = leanTween(new Vector2(elemento.resolvedStyle.left,elemento.resolvedStyle.top), posicao, 1,
                            (Vector2 posicao) =>
                            {
                                elemento.style.left = posicao.x;
                                elemento.style.top = posicao.y;
                            });
                    }
                
                    state = "dados";
                }
                else
                {
                    if (state == "busca")
                    {
                        leans["SideBar"]["AparecerBarraDeLado"] = leanTween(-sideBar.resolvedStyle.right, 0f, 1f,
                            (right => sideBar.style.right = right));
                        
                        leans["SearchBar"]["AparecerBarraDeBusca"] = leanTween(searchBar.style.width.value.value, 72f, 1f,
                            (width => searchBar.style.width = Length.Percent(width)));

                        whoClickedSideBar = name;
                
                        foreach (var elemento in todosElementosDeShaders)
                        {
                            Vector2 posicao = possiblePositions["dados+busca"][todosElementosDeShaders.IndexOf(elemento)];
                
                            foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
                            {
                                if (leans[elemento.name][efeito] != null)
                                {
                                    LeanTween.cancel((int)leans[elemento.name][efeito]);
                                    leans[elemento.name][efeito] = null;
                                }
                            }
                
                            leans[elemento.name]["AparecerBarraDeLado"] = leanTween(new Vector2(elemento.resolvedStyle.left,elemento.resolvedStyle.top), posicao, 1,
                                (Vector2 posicao) =>
                                {
                                    elemento.style.left = posicao.x;
                                    elemento.style.top = posicao.y;
                                });
                        }
                
                        state = "dados+busca";
                    }
                    else
                    {
                        leans["SideBar"]["AparecerBarraDeLado"] = leanTween(-sideBar.resolvedStyle.right, 0f, 1f,
                            (right => sideBar.style.right = right));
                        
                        leans["SearchBar"]["AparecerBarraDeBusca"] = leanTween(searchBar.style.width.value.value, 72f, 1f,
                            (width => searchBar.style.width = Length.Percent(width)));

                        whoClickedSideBar = name;
                
                        foreach (var elemento in todosElementosDeShaders)
                        {
                            Vector2 posicao = possiblePositions["dados+filtros"][todosElementosDeShaders.IndexOf(elemento)];
                
                            foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
                            {
                                if (leans[elemento.name][efeito] != null)
                                {
                                    LeanTween.cancel((int)leans[elemento.name][efeito]);
                                    leans[elemento.name][efeito] = null;
                                }
                            }
                
                            leans[elemento.name]["AparecerBarraDeLado"] = leanTween(new Vector2(elemento.resolvedStyle.left,elemento.resolvedStyle.top), posicao, 1,
                                (Vector2 posicao) =>
                                {
                                    elemento.style.left = posicao.x;
                                    elemento.style.top = posicao.y;
                                });
                        }
                
                        state = "dados+filtros";
                    }
                }
            }
            else
            {
                if (whoClickedSideBar.Contains(name))
                {
                    if (state == "dados")
                    {
                        leans["SideBar"]["AparecerBarraDeLado"] = leanTween(sideBar.resolvedStyle.right, -421f, .4f,
                            (right => sideBar.style.right = right));

                        whoClickedSideBar = null;
                
                        foreach (var elemento in todosElementosDeShaders)
                        {
                            Vector2 posicao = possiblePositions["padrao"][todosElementosDeShaders.IndexOf(elemento)];
                
                            foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
                            {
                                if (leans[elemento.name][efeito] != null)
                                {
                                    LeanTween.cancel((int)leans[elemento.name][efeito]);
                                    leans[elemento.name][efeito] = null;
                                }
                            }
                
                            leans[elemento.name]["AparecerBarraDeLado"] = leanTween(new Vector2(elemento.resolvedStyle.left,elemento.resolvedStyle.top), posicao, 1,
                                (Vector2 posicao) =>
                                {
                                    elemento.style.left = posicao.x;
                                    elemento.style.top = posicao.y;
                                });
                        }
                
                        state = "padrao";
                    }
                    else
                    {
                        if (state == "dados+busca")
                        {
                            leans["SideBar"]["AparecerBarraDeLado"] = leanTween(-sideBar.resolvedStyle.right, -421f, 1f,
                                (right => sideBar.style.right = right));
                        
                            leans["SearchBar"]["AparecerBarraDeBusca"] = leanTween(searchBar.style.width.value.value, 94.3f, 1f,
                                (width => searchBar.style.width = Length.Percent(width)));

                            whoClickedSideBar = null;
                
                            foreach (var elemento in todosElementosDeShaders)
                            {
                                Vector2 posicao = possiblePositions["busca"][todosElementosDeShaders.IndexOf(elemento)];
                
                                foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
                                {
                                    if (leans[elemento.name][efeito] != null)
                                    {
                                        LeanTween.cancel((int)leans[elemento.name][efeito]);
                                        leans[elemento.name][efeito] = null;
                                    }
                                }
                
                                leans[elemento.name]["AparecerBarraDeLado"] = leanTween(new Vector2(elemento.resolvedStyle.left,elemento.resolvedStyle.top), posicao, 1,
                                    (Vector2 posicao) =>
                                    {
                                        elemento.style.left = posicao.x;
                                        elemento.style.top = posicao.y;
                                    });
                            }
                
                            state = "busca";
                        }
                        else
                        {
                            leans["SideBar"]["AparecerBarraDeLado"] = leanTween(-sideBar.resolvedStyle.right, -421f, 1f,
                                (right => sideBar.style.right = right));
                        
                            leans["SearchBar"]["AparecerBarraDeBusca"] = leanTween(searchBar.style.width.value.value, 94.3f, 1f,
                                (width => searchBar.style.width = Length.Percent(width)));

                            whoClickedSideBar = null;
                
                            foreach (var elemento in todosElementosDeShaders)
                            {
                                Vector2 posicao = possiblePositions["filtros"][todosElementosDeShaders.IndexOf(elemento)];
                
                                foreach (String efeito in new List<String>{"AparecerBarraDeLado"})
                                {
                                    if (leans[elemento.name][efeito] != null)
                                    {
                                        LeanTween.cancel((int)leans[elemento.name][efeito]);
                                        leans[elemento.name][efeito] = null;
                                    }
                                }
                
                                leans[elemento.name]["AparecerBarraDeLado"] = leanTween(new Vector2(elemento.resolvedStyle.left,elemento.resolvedStyle.top), posicao, 1,
                                    (Vector2 posicao) =>
                                    {
                                        elemento.style.left = posicao.x;
                                        elemento.style.top = posicao.y;
                                    });
                            }
                
                            state = "filtros";
                        }
                    }
                }
                else
                {
                    whoClickedSideBar = name;
                }
            }
        });

        root.Add(interactiveBox);
        
        root.style.scale = new Scale(Vector3.zero);

        return root;
    }

    void AcaoDeEntradaESaidaDoMouse(String name,VisualElement root,VisualElement icone, Label nameElement, VisualElement poli,float valorParaExpandirElemento, Vector2 posicaoParaMoverIcone, float valorParaAlphaNome, Vector2 posicaoParaMoverPoligono, float valorParaGirarPoligono)
    {
        foreach (String efeito in new List<String>{"Expandir","MoverIcone","TirarNome","MoverPoligono","GirarPoligono"})
        {
            if (leans[name][efeito] != null)
            {
                LeanTween.cancel((int)leans[name][efeito]);
                leans[name][efeito] = null;
            }
        }

        leans[name]["Expandir"] = leanTween(root.resolvedStyle.scale.value.x, valorParaExpandirElemento, 1f,
            (scale => root.style.scale = new Scale(Vector3.one * scale))
        );

        leans[name]["MoverIcone"] = leanTween(new Vector2(icone.resolvedStyle.left, icone.resolvedStyle.bottom),
            posicaoParaMoverIcone, 1f, (leftAndBottom =>
            {
                icone.style.left = leftAndBottom.x;
                icone.style.bottom = leftAndBottom.y;
            })
        );

        leans[name]["TirarNome"] = leanTween(nameElement.resolvedStyle.opacity, valorParaAlphaNome, .4f,
            (alpha => nameElement.style.opacity = alpha));

        leans[name]["MoverPoligono"] = leanTween(new Vector2(poli.resolvedStyle.left, poli.resolvedStyle.bottom),
            posicaoParaMoverPoligono, 1f, (posicao =>
            {
                poli.style.left = posicao.x;
                poli.style.bottom = posicao.y;
            })
        );

        leans[name]["GirarPoligono"] = leanTween(poli.resolvedStyle.rotate.angle.value, valorParaGirarPoligono, 1f,
            (rotation => poli.style.rotate = new Rotate(rotation)));
    }

    void CalcularPossiveisPosicoes(int numeroDeElementos)
    {
        possiblePositions = new Dictionary<string, List<Vector2>>();
        
        VisualElement topBar = root.Q<VisualElement>("TopBar");

        // Posicao Padrao
        possiblePositions.Add("padrao",CalcularPosicao(numeroDeElementos,55f, 69f+20f, 1810f + 30f, 342f + 25f,337f + 20f));
        
        // Posicao Busca
        possiblePositions.Add("busca",CalcularPosicao(numeroDeElementos,55f, 69f+20f+87f, 1810f + 30f, 342f + 25f,337f + 20f));
        
        // Posicao Filtros
        possiblePositions.Add("filtros",CalcularPosicao(numeroDeElementos,55f, 69f+20f+87f+58f, 1810f + 30f, 342f + 25f,337f + 20f));
        
        // Posicao Dados
        possiblePositions.Add("dados",CalcularPosicao(numeroDeElementos,30f, 69f+20f, 1810f + 30f - 421f, 342f + 25f,337f + 20f));
        
        // Posicao Dados+Busca
        possiblePositions.Add("dados+busca",CalcularPosicao(numeroDeElementos,30f, 69f+20f+87f, 1810f + 30f - 421f, 342f + 25f,337f + 20f));
        
        // Posicao Dados+Filtros
        possiblePositions.Add("dados+filtros",CalcularPosicao(numeroDeElementos,30f, 69f+20f+87f+58f, 1810f + 30f - 421f, 342f + 25f,337f + 20f));
    }
    
    List<Vector2> CalcularPosicao(int numeroDeElementos,float x,float y,float maxRowSize,float width,float height)
    {
        List<Vector2> posicoes = new List<Vector2>();

        float _x = x;

        for (int i = 0; i < numeroDeElementos; i++)
        {
            posicoes.Add(new Vector2(x,y));

            x += width;

            if (x > maxRowSize)
            {
                x = _x;
                y += height;
            }
        }

        return posicoes;
    }

    void AdicionarElementosDeShadersATela()
    {
        CalcularPossiveisPosicoes(todosElementosDeShaders.Count);

        VisualElement background = root.Q<VisualElement>("Background");
        
        foreach (var elemento in todosElementosDeShaders)
        {
            background.Add(elemento);
            
            Vector2 posicao = possiblePositions["padrao"][todosElementosDeShaders.IndexOf(elemento)];
            
            elemento.style.left = posicao.x;
            elemento.style.top = posicao.y;
        }

        LeanTween.value(0f, 1f, 1).setEaseOutExpo().setOnUpdate((float scale) =>
        {
            foreach (VisualElement elementoAAdicionar in todosElementosDeShaders)
            {
                elementoAAdicionar.style.scale = new Scale(Vector3.one * scale);
            }
        });
    }

    void AdicionarElementosDeFiltrosATela()
    {
        foreach (var elemento in todosFiltrosDeShaders)
        {
            filtros.Add(elemento);
        }
    }

    void TransicaoDeTelaAnterior()
    {
        LeanTween.value(1f, 0f, 1).setEaseOutExpo().setOnUpdate((float scale) =>
        {
            todosElementosDeShaders.ForEach((VisualElement elemento) =>
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

        LeanTween.value(root.Q<VisualElement>("TopBar").style.top.value.value,-6f,1).setEaseOutExpo().setOnUpdate((float top)=>topBar.style.top = Length.Percent(top)).setOnComplete(
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
        VisualElement filtroBar = root.Q<VisualElement>("Filtros");
        
        if (searchBarActive)
        {
            leans["SearchBar"]["AparecerBarraDePesquisa"] = LeanTween.value(searchBar.style.top.value.value ,-87f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
            {
                searchBar.style.top = top;
            }).uniqueId;

            if (state == "busca" || state == "filtros")
            {
                foreach (var elemento in todosElementosDeShaders)
                {
                    float altura = possiblePositions["padrao"][todosElementosDeShaders.IndexOf(elemento)].y;
                
                    foreach (String efeito in new List<String>{"MexerShadersPelaBarraDePesquisa"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }
                
                    leans[elemento.name]["MexerShadersPelaBarraDePesquisa"] = LeanTween.value(elemento.resolvedStyle.top, altura, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                    {
                        elemento.style.top = altura1;
                    }).uniqueId;
                }

                if (state.Contains("filtros"))
                {
                    foreach (String efeito in new List<String>{"AparecerBarraDeFiltros"})
                    {
                        if (leans["FiltrosBar"][efeito] != null)
                        {
                            LeanTween.cancel((int)leans["FiltrosBar"][efeito]);
                            leans["FiltrosBar"][efeito] = null;
                        }
                    }
                    
                    leans["FiltrosBar"]["AparecerBarraDeFiltros"] = LeanTween.value(filtroBar.resolvedStyle.top ,0f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
                    {
                        filtroBar.style.top = top;
                    }).uniqueId;
                    
                    filtroBarActive = !filtroBarActive;
                }
                
                state = "padrao";
            }
            else
            {
                if (state == "dados+busca" || state == "dados+filtros")
                {
                    searchBar.style.width = Length.Percent(72f);

                    foreach (var elemento in todosElementosDeShaders)
                    {
                        float altura = possiblePositions["dados"][todosElementosDeShaders.IndexOf(elemento)].y;

                        foreach (String efeito in new List<String> { "MexerShadersPelaBarraDePesquisa" })
                        {
                            if (leans[elemento.name][efeito] != null)
                            {
                                LeanTween.cancel((int)leans[elemento.name][efeito]);
                                leans[elemento.name][efeito] = null;
                            }
                        }

                        leans[elemento.name]["MexerShadersPelaBarraDePesquisa"] = LeanTween
                            .value(elemento.resolvedStyle.top, altura, 1).setEaseOutExpo()
                            .setOnUpdate((float altura1) => { elemento.style.top = altura1; }).uniqueId;
                    }

                    if (state == "dados+filtros")
                    {
                        foreach (String efeito in new List<String> { "AparecerBarraDeFiltros" })
                        {
                            if (leans["FiltrosBar"][efeito] != null)
                            {
                                LeanTween.cancel((int)leans["FiltrosBar"][efeito]);
                                leans["FiltrosBar"][efeito] = null;
                            }
                        }

                        leans["FiltrosBar"]["AparecerBarraDeFiltros"] = LeanTween
                            .value(filtroBar.resolvedStyle.top, 0f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
                            {
                                filtroBar.style.top = top;
                            }).uniqueId;

                        filtroBarActive = !filtroBarActive;
                    }

                    state = "dados";
                }
            }
        }
        else
        {
            leans["SearchBar"]["AparecerBarraDePesquisa"] = LeanTween.value(searchBar.style.top.value.value ,topBar.resolvedStyle.height+20f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
            {
                searchBar.style.top = top;
            }).uniqueId;

            if (state.Contains("padrao"))
            {
                searchBar.style.width = Length.Percent(94.3f);
                
                foreach (var elemento in todosElementosDeShaders)
                {
                    float altura = possiblePositions["busca"][todosElementosDeShaders.IndexOf(elemento)].y;
                
                    foreach (String efeito in new List<String>{"MexerShadersPelaBarraDePesquisa"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }
                
                    leans[elemento.name]["MexerShadersPelaBarraDePesquisa"] = LeanTween.value(elemento.resolvedStyle.top, altura, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                    {
                        elemento.style.top = altura1;
                    }).uniqueId;
                }

                state = "busca";
            }
            else
            {
                searchBar.style.width = Length.Percent(72f);
                
                foreach (var elemento in todosElementosDeShaders)
                {
                    float altura = possiblePositions["dados+busca"][todosElementosDeShaders.IndexOf(elemento)].y;
                
                    foreach (String efeito in new List<String>{"MexerShadersPelaBarraDePesquisa"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }
                
                    leans[elemento.name]["MexerShadersPelaBarraDePesquisa"] = LeanTween.value(elemento.resolvedStyle.top, altura, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                    {
                        elemento.style.top = altura1;
                    }).uniqueId;
                }

                state = "dados+busca";
            }
        }
        searchBarActive = !searchBarActive;
    }
    
    void TransicaoDaBarraDeFiltros()
    {
        foreach (String efeito in new List<String>{"AparecerBarraDeFiltros"})
        {
            if (leans["FiltrosBar"][efeito] != null)
            {
                LeanTween.cancel((int)leans["FiltrosBar"][efeito]);
                leans["FiltrosBar"][efeito] = null;
            }
        }
        
        VisualElement filtroBar = root.Q<VisualElement>("Filtros");
        
        if (filtroBarActive)
        {
            leans["FiltrosBar"]["AparecerBarraDeFiltros"] = LeanTween.value(filtroBar.resolvedStyle.top ,0, 1f).setEaseOutExpo().setOnUpdate((float top) =>
            {
                filtroBar.style.top = top;
            }).uniqueId;

            if (state == "filtros")
            {
                foreach (var elemento in todosElementosDeShaders)
                {
                    float altura = possiblePositions["busca"][todosElementosDeShaders.IndexOf(elemento)].y;
                
                    foreach (String efeito in new List<String>{"AparecerBarraDeFiltros"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }
                
                    leans[elemento.name]["AparecerBarraDeFiltros"] = LeanTween.value(elemento.resolvedStyle.top, altura, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                    {
                        elemento.style.top = altura1;
                    }).uniqueId;
                }
                
                state = "busca";
            }
            else
            {
                foreach (var elemento in todosElementosDeShaders)
                {
                    Vector2 posicao = possiblePositions["dados+busca"][todosElementosDeShaders.IndexOf(elemento)];
                
                    foreach (String efeito in new List<String>{"AparecerBarraDeFiltros"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }

                    leans[elemento.name]["AparecerBarraDeFiltros"] = leanTween(new Vector2(elemento.resolvedStyle.left, elemento.resolvedStyle.top), posicao, 1,
                        (Vector2 posicao) =>
                        {
                            elemento.style.left = posicao.x;
                            elemento.style.top = posicao.y;
                        });
                }

                state = "dados+busca";
            }
        }
        else
        {
            leans["FiltrosBar"]["AparecerBarraDeFiltros"] = LeanTween.value(filtroBar.resolvedStyle.top ,74f, 1f).setEaseOutExpo().setOnUpdate((float top) =>
            {
                filtroBar.style.top = top;
            }).uniqueId;

            if (state == "busca")
            {
                foreach (var elemento in todosElementosDeShaders)
                {
                    float altura = possiblePositions["filtros"][todosElementosDeShaders.IndexOf(elemento)].y;
                
                    foreach (String efeito in new List<String>{"AparecerBarraDeFiltros"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }
                
                    leans[elemento.name]["AparecerBarraDeFiltros"] = LeanTween.value(elemento.resolvedStyle.top, altura, 1).setEaseOutExpo().setOnUpdate((float altura1) =>
                    {
                        elemento.style.top = altura1;
                    }).uniqueId;
                }

                state = "filtros";
            }
            else
            {
                foreach (var elemento in todosElementosDeShaders)
                {
                    Vector2 posicao = possiblePositions["dados+filtros"][todosElementosDeShaders.IndexOf(elemento)];
                
                    foreach (String efeito in new List<String>{"AparecerBarraDeFiltros"})
                    {
                        if (leans[elemento.name][efeito] != null)
                        {
                            LeanTween.cancel((int)leans[elemento.name][efeito]);
                            leans[elemento.name][efeito] = null;
                        }
                    }

                    leans[elemento.name]["AparecerBarraDeFiltros"] = leanTween(new Vector2(elemento.resolvedStyle.left, elemento.resolvedStyle.top), posicao, 1,
                        (Vector2 posicao) =>
                        {
                            elemento.style.left = posicao.x;
                            elemento.style.top = posicao.y;
                        });
                }

                state = "dados+filtros";
            }
        }
        filtroBarActive = !filtroBarActive;
    }

    int leanTween(float valorInicial, float valorFinal, float tempo,Action<float> funcao)
    {
        return LeanTween.value(valorInicial, valorFinal, tempo).setEaseOutExpo().setOnUpdate((float value) => funcao(value)).setOnComplete(_ => funcao(valorFinal)).uniqueId;
    }

    int leanTween(Vector2 valorInicial, Vector2 valorFinal, float tempo,Action<Vector2> funcao)
    {
        return LeanTween.value(gameObject,valorInicial, valorFinal, tempo).setEaseOutExpo().setOnUpdate((Vector2 value) => funcao(value)).setOnComplete(_ => funcao(valorFinal)).uniqueId;
    }
}