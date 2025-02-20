using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maaping : MonoBehaviour
{
    public SpriteRenderer[] render;

    public Sprite[] grassSprites;

    private void Start()
    {
        GetRenderes();
    }

    private void GetRenderes()
    {
        render = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < render.Length; i++)
        {
            if (render[i].sprite == null)
                continue;

            render[i].sprite = grassSprites[0];
            //Debug.Log(render[i].sprite.name);
            //if (render[i].sprite.name.IndexOf("tree") >= 0)
            //{
            //    render[i].sprite = grassSprites[0];
            //}
        }
    }
}
