using Stardust;
using System.Collections.Generic;

public class DialogData : IComponent, IInitialize
{
    List<string> contentList = new List<string>();

    public void Init()
    {
        contentList = new List<string>
        {
            "你好",
            "Hello",
            "天气不错",
            "Nice day",
            "最近怎么样",
            "What's up",
            "再见",
            "Bye"
        };
    }

    public List<string> GetContent(int count)
    {
        if (count > contentList.Count)
            count = contentList.Count;

        return contentList.GetRange(0, count);
    }
}
