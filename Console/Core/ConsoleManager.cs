using UnityEngine;

namespace Needle.Console.Core
{
    public class ConsoleManager : MonoBehaviour
    {
        void Start()
        {
            CommandRegistry.RegisterStaticCommands();
            CommandProcessor.RunCommand("hello world", out string[] outcomes);
            foreach (var outcome in outcomes) Debug.Log(outcome);
        }
    }
}