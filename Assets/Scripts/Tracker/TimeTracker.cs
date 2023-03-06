using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

namespace PickleClicker.Tracker
{  
    public class TimeTracker : MonoBehaviour
    {   
        [SerializeField] private Text timer; 
        public static ulong seconds = 0;
        public static ulong minutes = 0;
        public static ulong hours = 0;

        private void Start() 
        {
            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }

        private void Update() {
            if (hours > 0)
            {
                timer.text = $"Session Time Played: \n{hours} Hours, {minutes} Minutes and {seconds} Seconds";
            }
            else if (minutes > 0)
            {
                timer.text = $"Session Time Played: \n{minutes} Minutes and {seconds} Seconds";
            } 
            else 
            {
                timer.text = $"Session Time Played: \n{seconds} Seconds";
            }
        }

        IEnumerator StartTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                seconds += 1;
                if (seconds > 59)
                {
                    minutes += (seconds / 60);  
                    seconds %= 60;
                } 

                if (minutes > 59)
                {
                    hours += (minutes / 60);
                    minutes %= 60;
                }
            }
        }
    }
}
