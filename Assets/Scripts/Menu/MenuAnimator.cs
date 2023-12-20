using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] m_Sprites;
    [SerializeField] private float m_Delay;

    private Image m_Image;
    private int index;


    void Start()
    {
        m_Image = gameObject.GetComponent<Image>();


        InvokeRepeating(nameof(Animate), 0, m_Delay);
    }



    private void Animate()
    {
        index = (index + 1) % m_Sprites.Length;

        m_Image.sprite = m_Sprites[index];
    }

}