using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CreditBox : MonoBehaviour
{
    public Text text;

    public void Set()
    {
        StringBuilder sb = new();
        foreach(var (modelName, userName, address) in DataManager.credits)
            sb.AppendLine($"\"{modelName}\" by {userName} ({address}) - CC-BY 4.0");
        text.text = sb.ToString();
    }

    public void Hide()
    {
        this.SetActive(false);
    }
}
