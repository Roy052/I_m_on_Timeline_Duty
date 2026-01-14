using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CreditBox : MonoBehaviour
{
    public Text text;

    public void Set()
    {
        StringBuilder sb = new();
        foreach(var (modelName, userName, address, cc) in DataManager.credits)
            sb.AppendLine($"\"{modelName}\" by {userName} ({address}) - {cc}");
        text.text = sb.ToString();
    }

    public void Hide()
    {
        this.SetActive(false);
    }
}
