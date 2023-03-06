using PickleClicker.Data;
using PickleClicker.Pickle;
using System.Collections;
using System;
using UnityEngine;

namespace PickleClicker.Auto
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
                long total = 0; 
                for (long index = 0; index < PlayerData.autoList.autoBuyables.Count; index++)
                {
                    AutoData autoPickle = PlayerData.autoList.autoBuyables.Find(autoPickle => autoPickle.id == index);

                    long autoRecieve = (long) (autoPickle.amount * Math.Floor(autoPickle.recieve * autoPickle.multiplier));

                    total += autoRecieve;
                    
                }
                PlayerData.pickleData.picklesPerSecond = (long) total;
                PlayerData.pickleData.picklesPicked += (ulong) PlayerData.pickleData.picklesPerSecond;
                PickleController.GetHighestAmountOfPickles();
                yield return new WaitForSeconds(1f);
            }
        } 
    }
}

