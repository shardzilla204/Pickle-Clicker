using PickleClicker.Data.Player;
using PickleClicker.Data.Auto;
using PickleClicker.Controller.Pickle;
using System.Collections;
using System;
using UnityEngine;

namespace PickleClicker.Game.Auto
{
    public class AutoAdd : MonoBehaviour
    {   
        private void Awake()
        {
            StartCoroutine(AddAutoPickles());
        }

        //Adds the pickles to your current amount of pickles
        IEnumerator AddAutoPickles()
        {   
            while (true)
            {
                double total = 0; 
                for (int index = 0; index < PlayerData.autoDataList.Count; index++)
                {
                    AutoData auto = PlayerData.autoDataList.Find(auto => auto.id == index);

                    double autoRecieve = (auto.currentAmount * Math.Floor(auto.recieve * auto.recieveMultiplier));

                    total += autoRecieve;
                    
                }
                PlayerData.pickleData.gainPerSecond = total;
                PlayerData.pickleData.pickles += PlayerData.pickleData.gainPerSecond/100;
                if (PlayerData.pickleData.pickles <= PickleController.totalPicklesPicked) PickleController.GetHighestAmountOfPickles();
                yield return new WaitForSeconds(0.01f);
            }
        } 
    }
}

