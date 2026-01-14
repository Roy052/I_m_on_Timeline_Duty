using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] Light thisLight;

    float time = 0f;
    float flickTime = 0f;

    private void Start()
    {
        flickTime = Random.Range(0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > flickTime)
        {
            thisLight.enabled = !thisLight.enabled;
            flickTime = Random.Range(0.5f, 1f);
            time = 0f;
        }
    }
}
