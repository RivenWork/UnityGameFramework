using UnityEngine;

public class Starter : MonoBehaviour
{
    MyEngine gameEngine;
    // Use this for initialization
    void Start()
    {
        gameEngine = new MyEngine();
        var dialog = gameEngine.InstallGameSystem<DialogSystem>();
        dialog.Init();
    }

    // Update is called once per frame
    void Update()
    {
        gameEngine.Update(Time.deltaTime);
    }
}
