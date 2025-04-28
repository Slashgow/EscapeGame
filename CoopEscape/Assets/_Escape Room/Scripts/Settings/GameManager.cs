using Newtonsoft.Json.Bson;
using UnityEngine;

public class GameManager : PersistentMonoSingleton<GameManager>
{
    public bool IsInMenu { get; set; } = false;

    public void QuitMenu() => IsInMenu = false;
    public void Quit() => Application.Quit();
}
