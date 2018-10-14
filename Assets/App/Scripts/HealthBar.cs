using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    private Vector3 curScale;

    Renderer barRenderer;

    // Use this for initialization
    void Start()
    {
        barRenderer = GetComponent<Renderer>();
        curScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitPointReduceTo(float scale)
    {
        float originalValue = barRenderer.bounds.min.y;
        Debug.Log("original value : " + originalValue);
        curScale.y = scale * curScale.y;
        gameObject.transform.localScale = curScale;

        float newValue = barRenderer.bounds.min.y;
        Debug.Log("new value : " + newValue);

        //calculate difference
        float difference = newValue - originalValue;

        //move the bar to the left
        transform.Translate(new Vector3(-0f, -difference, 0f));

    }
}
