using Stardust;
using UnityEngine;

public class DialogAdapter : IComponent, IUpdate, IInitialize {

    DialogData _dialogData { get; set; }

    DialogUI _dialogUI;

    bool _stop;

    public void Init()
    {
        _dialogUI = Object.FindObjectOfType<DialogUI>();
        _dialogUI.onClickEnter += _dialogUI_onClickEnter;
    }

    private void _dialogUI_onClickEnter(string obj)
    {
        if (obj == "list")
        {
            var str = "";
            var list = _dialogData.GetContent(5);
            foreach (var item in list)
                str += item + "\n";

            str += obj;
            _dialogUI.SetTextInfo(str);
            _stop = true;
        }
    }

    public void OnUpdate(float t)
    {
        if (!_stop)
        {
            _dialogUI.SetTextInfo(Time.deltaTime.ToString());
        }
    }
}
