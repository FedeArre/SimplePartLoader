﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimplePartLoader
{
    public class ModUtils
    {
        private static GameObject Player;
        private static tools PlayerTools;
        private static AudioManager AudioList;
        private static AudioSource Source;
        
        private static MainCarProperties CurrentPlayerCar;

        internal static List<GameObject> Cars;

        public delegate void OnPlayerCarChangeDelegate();
        public static event OnPlayerCarChangeDelegate PlayerCarChanged;

        internal static void OnLoadCalled()
        {
            Player = GameObject.Find("Player");
            PlayerTools = Player.GetComponent<tools>();

            GameObject PlayerHand = GameObject.Find("hand");
            AudioList = PlayerHand.GetComponent<AudioManager>();
            Source = PlayerHand.GetComponent<AudioSource>();

            Cars = new List<GameObject>();

            GameObject[] carList = GameObject.Find("CarsParent").GetComponent<CarList>().Cars;
            foreach(GameObject car in carList)
            {
                car.AddComponent<SPL_CarTracking>();
            }
            Debug.LogError("dsaaaaaa");
            GameObject dummy = new GameObject("SPL_Dummy");
            dummy.AddComponent<SPL_CarTracking>().AddToAll();
        }

        internal static void UpdatePlayerStatus(bool isOnCar, MainCarProperties mcp = null)
        {
            if (isOnCar)
            {
                CurrentPlayerCar = mcp;
            }
            else
            {
                CurrentPlayerCar = null;
            }

            Debug.LogError($"UpdatePlayerStatus has changed to {isOnCar} - {mcp}");
            if(PlayerCarChanged != null)
            {
                PlayerCarChanged?.Invoke();
            }
        }

        public static GameObject GetPlayer()
        {
            if(Player)
            {
                return Player;
            }
            else
            {
                Debug.LogError("[SPL]: Tried to use GetPlayer but Player was null. Make sure that you are using it after OnLoad!");
                return null;
            }
        }

        public static tools GetPlayerTools()
        {
            if (PlayerTools)
            {
                return PlayerTools;
            }
            else
            {
                Debug.LogError("[SPL]: Tried to use GetPlayerTools but PlayerTools was null. Make sure that you are using it after OnLoad!");
                return null;
            }
        }

        public static AudioManager GetAudios()
        {
            if (AudioList)
            {
                return AudioList;
            }
            else
            {
                Debug.LogError("[SPL]: Tried to use GetAudios but the audio list was null. Make sure that you are using it after OnLoad!");
                return null;
            }
        }

        public static void PlaySound(AudioClip ac)
        {
            if(ac && Source)
            {
                Source.PlayOneShot(ac);
            }
            else
            {
                Debug.LogError("[SPL]: Tried to use PlaySound but AudioClip / source was null. Make sure that you are using it after OnLoad and have a valid AudioClip!");
            }
        }

        public static void PlayCashSound()
        {
            if (Source)
            {
                Source.PlayOneShot(AudioList.Cash);
            }
        }

        public static MainCarProperties GetPlayerCurrentCar()
        {
            return CurrentPlayerCar;
        }
    }
}