using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Frequency
{
    public class SpectrumGUI : MonoBehaviour
    {

        public AudioSpectrum spectrum;
        public List<Transform> cubes = new List<Transform>();
        public float scale;

        private void Start()
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag("CubeElement"))
            {
                cubes.Add(obj.transform);
            }
        }
        private void Update()
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                var cube = cubes[i];
                var localScale = cube.localScale;
                localScale.y = spectrum.Levels[i] * scale;
                cube.localScale = localScale;
            }
        }
    }
}