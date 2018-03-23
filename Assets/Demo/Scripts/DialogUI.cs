using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour {

    public event System.Action<string> onClickEnter;

    public Text text;
    public InputField inputField;

    public void OnClickButton()
    {
        onClickEnter?.Invoke(inputField.text);
    }

    public void SetTextInfo(string str)
    {
        text.text = str;
    }
}
