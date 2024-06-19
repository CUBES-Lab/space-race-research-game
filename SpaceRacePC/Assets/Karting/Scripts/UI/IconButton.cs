using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject rawImageObject;
    public Texture iconTexture;
    public Texture highlightedIconTexture;
    public Color iconColor;
    public Color highlightedIconColor;

    private RawImage myRawImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRawImage = rawImageObject.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myRawImage.texture = highlightedIconTexture;
        myRawImage.color = highlightedIconColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myRawImage.texture = iconTexture;
        myRawImage.color = iconColor;
    }
}
