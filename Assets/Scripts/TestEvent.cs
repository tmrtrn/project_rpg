using System;
using Core.Services.Event;
using Data.Hero;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestEvent : MonoBehaviour,  IGameEvent
    {
        private void Awake()
        {
            for (int i = 0; i < 1000; i++)
            {
                var heroName  =  HeroNameHelper.ReadRandomHeroName();
                Debug.Log("name : " + heroName);
            }

        }
    }
}