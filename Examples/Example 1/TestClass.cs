using UnityEngine;

namespace NeedleAssets.Examples.Example_1
{
    public class TestClass
    {
        public Vector3 Position;
        public string Name;
        public int Age;

        public TestClass(Vector3 pos, string name, int age)
        {
            Position = pos;
            Name = name;
            Age = age;
        }
    }
}