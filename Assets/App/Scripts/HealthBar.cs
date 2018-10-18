using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    private Vector3 curScale;
    private Vector3 maxScale;
    private Vector3 initialPosition;

    Renderer barRenderer;

    // Use this for initialization
    void Awake()
    {
        barRenderer = GetComponent<Renderer>();
        maxScale = transform.localScale;
        curScale = maxScale;
        initialPosition = transform.localPosition;
    }

    
    public void HitPointReduceTo(float scale)
    {
        float originalValue = barRenderer.bounds.min.x;
        curScale.x = scale * maxScale.x;
        transform.localScale = curScale;

        //Debug.Log("cur scale : " + curScale + " and new scale");
        float newValue = barRenderer.bounds.min.x;

        //calculate difference
        float difference = newValue - originalValue;

        //move the bar to the left
        transform.Translate(new Vector3(-difference, 0f, 0f));
    }

    public void ResetToMax()
    {
        transform.localScale = maxScale;
        transform.localPosition = initialPosition;
    }
}
