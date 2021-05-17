#define TEMPLATE
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hertzole.ALE;
using UnityEngine;


#if TEMPLATE
public class NewSerializeTestScript : MonoBehaviour, IExposedToLevelEditor
#else
public class NewSerializeTestScript : MonoBehaviour
#endif
{
    public struct WrapperTemplate : IExposedWrapper
    {
        public ValueTuple<int, string> testString;
        public ValueTuple<int, int> testInt;
        public ValueTuple<int, Vector3> testVector3;
        public ValueTuple<int, ComponentDataWrapper> reference;

        public WrapperTemplate(string testString, int testInt, Vector3 testVector3, ComponentDataWrapper reference)
        {
            this.testString = (220, testString);
            this.testInt = (1, testInt);
            this.testVector3 = (2, testVector3);
            this.reference = (100, reference);
        }
    }
    
    [SerializeField] 
    [ExposeToLevelEditor(220)]
    private string testString = "Hello world";
    [SerializeField] 
    [ExposeToLevelEditor(1)]
    private int testInt = 42;
    [SerializeField] 
    [ExposeToLevelEditor(2)]
    private Vector3 testVector3 = new Vector3(1, 2, 3);
    [SerializeField] 
    [ExposeToLevelEditor(100)]
    private TestScript reference = null;

#if TEMPLATE
    public string ComponentName { get; } = nameof(NewSerializeTestScript);
    public string TypeName { get; } = nameof(NewSerializeTestScript);
    public Type ComponentType { get; } = typeof(NewSerializeTestScript);
    public bool HasVisibleFields { get; } = true;
    public int Order { get; } = 0;
    public event Action<int, object> OnValueChanged;

    public ReadOnlyCollection<ExposedProperty> GetProperties()
    {
        return new ReadOnlyCollection<ExposedProperty>(new List<ExposedProperty>()
        {
            new ExposedProperty(0, typeof(string), "testString", string.Empty, true),
            new ExposedProperty(1, typeof(int), "testInt", string.Empty, true),
            new ExposedProperty(2, typeof(Vector3), "testVector3", string.Empty, true),
            new ExposedProperty(100, typeof(TestScript), "reference", string.Empty, true),
        });
    }

    public object GetValue(int id)
    {
        switch (id)
        {
            case 0:
                return testString;
            case 1:
                return testInt;
            case 2:
                return testVector3;
            case 100:
                return reference;
            default:
                return null;
        }
    }

    public void SetValue(int id, object value, bool notify)
    {
        switch (id)
        {
            case 0:
                testString = (string) value;
                break;
            case 1:
                testInt = (int) value;
                break;
            case 2:
                testVector3 = (Vector3) value;
                break;
            case 100:
                if (value is ComponentDataWrapper wrapper && !wrapper.Equals(reference))
                {
                    reference = wrapper.GetObject<TestScript>();
                }

                break;
        }
    }

    IExposedWrapper IExposedToLevelEditor.GetWrapper()
    {
        return new WrapperTemplate(testString, testInt, testVector3, new ComponentDataWrapper(reference));
    }

    void IExposedToLevelEditor.ApplyWrapper(IExposedWrapper wrapper)
    {
        if (wrapper is WrapperTemplate w)
        {
            testString = w.testString.Item2;
            testInt = w.testInt.Item2;
            testVector3 = w.testVector3.Item2;
            reference = w.reference.Item2.GetObject<TestScript>();
        }
    }
#endif
}