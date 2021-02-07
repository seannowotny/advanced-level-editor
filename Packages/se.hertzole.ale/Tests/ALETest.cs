﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Hertzole.ALE.Tests
{
    public class ALETest
    {
        public List<GameObject> sceneObjects;

        [UnitySetUp]
        public IEnumerator SceneSetup()
        {
            sceneObjects = new List<GameObject>();

            OnSceneSetup();


#if UNITY_EDITOR
            yield return new EnterPlayMode();
#else
            yield return null;
#endif
        }

        protected virtual void OnSceneSetup() { }

        [UnityTearDown]
        public IEnumerator TearDownScene()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                Object.DestroyImmediate(sceneObjects[i].gameObject);
            }

            OnTearDownScene();

#if UNITY_EDITOR
            yield return new ExitPlayMode();
#else
            yield return null;
#endif
        }

        protected virtual void OnTearDownScene() { }
    }
}
