using UnityEngine;
using UnityEngine.UI;

public class LabelText : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] string label;

    private void Awake()
    {
        Observer.onRefreshLanguage += UpdateText;
    }

    private void OnDestroy()
    {
        Observer.onRefreshLanguage -= UpdateText;
    }

    void UpdateText()
    {
        text.text = DataManager.GetString(label);
    }
}
