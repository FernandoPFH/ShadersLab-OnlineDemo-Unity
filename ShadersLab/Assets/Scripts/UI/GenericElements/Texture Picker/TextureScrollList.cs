using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextureScrollList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private GameObject textureItemPrefab;

    public void SetupUI(string listName, List<Texture2D> texture2Ds, Action<Texture2D> setTextureHandler)
    {
        title.text = listName;

        foreach (Texture2D texture2D in texture2Ds)
        {
            GameObject textureItem = Instantiate(textureItemPrefab, contentHolder);

            textureItem.GetComponent<Image>().sprite = Sprite.Create(
                texture2D,
                new Rect(0.0f, 0.0f, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );

            textureItem.GetComponent<Button>().onClick.AddListener(() =>
            {
                Texture2D texture = texture2D;
                setTextureHandler(texture);
            });
        }
    }
}
