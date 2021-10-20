using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class GameManager : MonoSingleTon<GameManager>
    {
        [Range(0f, 100f)]
        public float writingSpeed = 60f;
    }
}



