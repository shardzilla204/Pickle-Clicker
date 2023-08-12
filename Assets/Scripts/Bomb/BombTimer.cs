using System.Collections;
using UnityEngine;

namespace PickleClicker.Game.Pickle
{
    public class BombTimer : MonoBehaviour
    {
        public void StartCountdown(GameObject timerObject)
        {
            Debug.Log($"Starting Countdown");
            StartCoroutine(Countdown(timerObject));
        }

        IEnumerator Countdown(GameObject timerObject)
        {
            int timeRemaining = 4;
            while (true)
            {
                timeRemaining--;
                if (timeRemaining < 0)
                {
                    Destroy(timerObject);
                    break;
                }
                Debug.Log($"Time Left: {timeRemaining}");
                timerObject.GetComponent<TextMesh>().text = $"{timeRemaining}s";
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
