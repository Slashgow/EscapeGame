using Newtonsoft.Json.Bson;
using UnityEngine;

public class GameManager : PersistentMonoSingleton<GameManager>
{
    public void Quit() => Application.Quit();
}
