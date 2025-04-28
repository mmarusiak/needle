using UnityEngine;

namespace Needle.Console.Core
{
    public class ConsoleManager : MonoBehaviour
    {
        void Start()
        {
            CommandRegistry.RegisterStaticCommands();
        }
    }
}