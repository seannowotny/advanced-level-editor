using System.Collections;
using System.Collections.Generic;
using Hertzole.ALE.Tests.TestScripts;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Hertzole.ALE.Tests
{
    public class PlayModeTests : LevelEditorTest
    {
        private ILevelEditorPlayMode playMode;
        
        protected override void OnSceneSetup(List<GameObject> objects)
        {
            GameObject playModeGo = new GameObject("Play Mode");
            playMode = playModeGo.AddComponent<TestPlayMode>();
            levelEditor.PlayMode = playMode;

            objects.Add(playModeGo);
        }

        [UnityTest]
        public IEnumerator ResetByteValues()
        {
            cube.AddComponent<ByteTest1>();
            cube.AddComponent<ByteTest2>();
            cube.AddComponent<ByteTest3>();
            cube.AddComponent<ByteTest4>();
            cube.AddComponent<ByteTest5>();
            cube.AddComponent<ByteTest6>();
            cube.AddComponent<ByteTest7>();
            cube.AddComponent<ByteTest8>();

            yield return null;

            ILevelEditorObject newCube = objectManager.CreateObject("cube");
            var byte1 = newCube.MyGameObject.GetComponent<ByteTest1>();
            var byte2 = newCube.MyGameObject.GetComponent<ByteTest2>();
            var byte3 = newCube.MyGameObject.GetComponent<ByteTest3>();
            var byte4 = newCube.MyGameObject.GetComponent<ByteTest4>();
            var byte5 = newCube.MyGameObject.GetComponent<ByteTest5>();
            var byte6 = newCube.MyGameObject.GetComponent<ByteTest6>();
            var byte7 = newCube.MyGameObject.GetComponent<ByteTest7>();
            var byte8 = newCube.MyGameObject.GetComponent<ByteTest8>();
            byte1.value = 1;
            byte2.value = 2;
            byte3.value1 = 3;
            byte3.value2 = 4;
            byte4.value1 = 5;
            byte4.value2 = 6;
            byte5.Value = 7;
            byte6.Value = 8;
            byte7.Value1 = 9;
            byte7.Value2 = 10;
            byte8.Value1 = 11;
            byte8.Value2 = 12;

            yield return null;

            playMode.StartPlayMode(levelEditor);

            yield return null;

            byte1.value = 0;
            byte2.value = 0;
            byte3.value1 = 0;
            byte3.value2 = 0;
            byte4.value1 = 0;
            byte4.value2 = 0;
            byte5.Value = 0;
            byte6.Value = 0;
            byte7.Value1 = 0;
            byte7.Value2 = 0;
            byte8.Value1 = 0;
            byte8.Value2 = 0;
            
            yield return null;

            playMode.StopPlayMode(levelEditor);
            
            yield return null;
            
            Assert.AreEqual<byte>(byte1.value, 1);
            Assert.AreEqual<byte>(byte2.value, 2);
            Assert.AreEqual<byte>(byte3.value1, 3);
            Assert.AreEqual<byte>(byte3.value2, 4);
            Assert.AreEqual<byte>(byte4.value1, 5);
            Assert.AreEqual<byte>(byte4.value2, 6);
            Assert.AreEqual<byte>(byte5.Value, 7);
            Assert.AreEqual<byte>(byte6.Value, 8);
            Assert.AreEqual<byte>(byte7.Value1, 9);
            Assert.AreEqual<byte>(byte7.Value2, 10);
            Assert.AreEqual<byte>(byte8.Value1, 11);
            Assert.AreEqual<byte>(byte8.Value2, 12);
        }
        
        [UnityTest]
        public IEnumerator ResetVectorValues()
        {
            cube.AddComponent<Vector3Test1>();
            cube.AddComponent<Vector3Test2>();
            cube.AddComponent<Vector3Test3>();
            cube.AddComponent<Vector3Test4>();
            cube.AddComponent<Vector3Test5>();
            cube.AddComponent<Vector3Test6>();
            cube.AddComponent<Vector3Test7>();
            cube.AddComponent<Vector3Test8>();

            yield return null;

            ILevelEditorObject newCube = objectManager.CreateObject("cube");
            var v1 = newCube.MyGameObject.GetComponent<Vector3Test1>();
            var v2 = newCube.MyGameObject.GetComponent<Vector3Test2>();
            var v3 = newCube.MyGameObject.GetComponent<Vector3Test3>();
            var v4 = newCube.MyGameObject.GetComponent<Vector3Test4>();
            var v5 = newCube.MyGameObject.GetComponent<Vector3Test5>();
            var v6 = newCube.MyGameObject.GetComponent<Vector3Test6>();
            var v7 = newCube.MyGameObject.GetComponent<Vector3Test7>();
            var v8 = newCube.MyGameObject.GetComponent<Vector3Test8>();

            yield return null;

            playMode.StartPlayMode(levelEditor);

            yield return null;
            
            v1.value = Vector3.one * 1;
            v2.value = Vector3.one * 2;
            v3.value1 = Vector3.one * 3;
            v3.value2 = Vector3.one * 4;
            v4.value1 = Vector3.one * 5;
            v4.value2 = Vector3.one * 6;
            v5.Value = Vector3.one * 7;
            v6.Value = Vector3.one * 8;
            v7.Value1 = Vector3.one * 9;
            v7.Value2 = Vector3.one * 10;
            v8.Value1 = Vector3.one * 11;
            v8.Value2 = Vector3.one * 12;
            
            yield return null;
            
            playMode.StopPlayMode(levelEditor);

            yield return null;

            Assert.AreEqual(v1.value, Vector3.zero);
            Assert.AreEqual(v2.value, Vector3.zero);
            Assert.AreEqual(v3.value1, Vector3.zero);
            Assert.AreEqual(v3.value2, Vector3.zero);
            Assert.AreEqual(v4.value1, Vector3.zero);
            Assert.AreEqual(v4.value2, Vector3.zero);
            Assert.AreEqual(v5.Value, Vector3.zero);
            Assert.AreEqual(v6.Value, Vector3.zero);
            Assert.AreEqual(v7.Value1, Vector3.zero);
            Assert.AreEqual(v7.Value2, Vector3.zero);
            Assert.AreEqual(v8.Value1, Vector3.zero);
            Assert.AreEqual(v8.Value2, Vector3.zero);
        }
        
        [UnityTest]
        //TODO: Components are not saved properly in wrappers?
        public IEnumerator ResetReferenceValues()
        {
            cube.AddComponent<TransformReferenceScript>();
            cube.AddComponent<GameObjectReferenceScript>();

            yield return null;

            ILevelEditorObject newCube = objectManager.CreateObject("cube");
            var ref1 = newCube.MyGameObject.GetComponent<TransformReferenceScript>();
            var ref2 = newCube.MyGameObject.GetComponent<GameObjectReferenceScript>();
            
            yield return null;

            playMode.StartPlayMode(levelEditor);

            yield return null;
            
            ref1.value = sphere.transform;
            ref2.value = capsule.gameObject;
            
            yield return null;
            
            playMode.StopPlayMode(levelEditor);

            yield return null;
            
            Assert.AreEqual(ref1.value, null);
            Assert.AreEqual(ref2.value, null);
        }
        
        [UnityTest]
        public IEnumerator ResetTransformValues()
        {
            ILevelEditorObject newCube = objectManager.CreateObject("cube");
           
            yield return null;

            playMode.StartPlayMode(levelEditor);

            yield return null;
            
            newCube.MyGameObject.transform.position = new Vector3(10, 20, 30);
            newCube.MyGameObject.transform.eulerAngles = new Vector3(10, 20, 30);
            newCube.MyGameObject.transform.localScale = new Vector3(10, 20, 30);
            
            yield return null;

            playMode.StopPlayMode(levelEditor);

            yield return null;
            
            Assert.AreEqual(newCube.MyGameObject.transform.position, Vector3.zero);
            Assert.AreEqual(newCube.MyGameObject.transform.eulerAngles, Vector3.zero);
            Assert.AreEqual(newCube.MyGameObject.transform.localScale, Vector3.one);
        }
    }
}
