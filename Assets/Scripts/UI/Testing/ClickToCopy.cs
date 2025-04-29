// Thank you https://discussions.unity.com/t/copy-textfield-or-textarea-text-to-clipboard/392802/9, extremely helpful!

using TMPro;
using UnityEngine;

public class ClickToCopy : MonoBehaviour
{
    private TextEditor te;

    [SerializeField] TextMeshProUGUI textToCopy;

    private void Start() { te = new TextEditor(); }

    public void Copy()
    {
        te.text = textToCopy.text;
        te.SelectAll();
        te.Copy();
    }
}
