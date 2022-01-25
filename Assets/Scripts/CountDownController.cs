using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Logic
{
    public class CountDownController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Text CountDownText;

        [SerializeField]
        private uint StartFrom = 3;
        [SerializeField]
        private string TextAtTheEnd = "Вперед!";

        [SerializeField]
        private AudioSource Audio;
        [SerializeField]
        private AudioClip CountdownSound;
        [SerializeField]
        private bool PlayGoSound = true;
        [SerializeField]
        private AudioClip GoSound;
#pragma warning restore 649

        public UnityEvent TimeIsUp = new UnityEvent();

        private uint _secondsLeft;
        private bool _hasItExpired = false;
        private bool _hasItAlmostExpired = false;
        public bool HasItExpired { get => _hasItExpired; }
        public bool HasItAlmostExpired { get => _hasItAlmostExpired; }

        private static readonly float TICK = 0.2f;

        private void OnEnable()
        {
            if (StartFrom == 0)
            {
                Debug.LogError("CountDownController: The StartFrom value has to be greater than 0. Disabling the script");
                enabled = false;
                return;
            }

            if (CountDownText == null)
            {
                Debug.LogError("CountDownController: The CountDownText can't be left unassigned. Disabling the script");
                enabled = false;
                return;
            }

            if (Audio == null)
            {
                Debug.LogError("CountDownController: The Audio can't be left unassigned. Disabling the script");
                enabled = false;
                return;
            }

            if (CountdownSound == null)
            {
                Debug.LogError("CountDownController: The CountdownSound can't be left unassigned. Disabling the script");
                enabled = false;
                return;
            }

            if (PlayGoSound && GoSound == null)
            {
                Debug.LogError("CountDownController: The GoSound can't be left unassigned. Disabling the script");
                enabled = false;
                return;
            }

            Restart();
        }

        private void Restart()
        {
            _secondsLeft = StartFrom + 1;
            _hasItExpired = _hasItAlmostExpired = false;

            // Call the 'UpdateTimer' method in a second, then every TICK second
            InvokeRepeating("UpdateTimer", 1f, 1f);

            // Call the 'AlmostThere' method when the time interval is about to expire
            Invoke("AlmostThere", StartFrom - TICK);

            // We should call it because the InvokeRepeating method does not work if you set the time argument to 0
            UpdateTimer();
        }

        /**
         * The method is called once per second untill the timer is off
         */
        private void UpdateTimer()
        {
            if (--_secondsLeft > 0) // The timer is still counting
            {
                // Play the 'Countdown' sound
                Audio.Stop();
                Audio.clip = CountdownSound;
                Audio.Play();

                // Update the label of the timer
                CountDownText.text = "0:" + _secondsLeft.ToString("D2");
            }
            else // It's time
            {
                // Play the 'Go' sound
                if (PlayGoSound)
                {
                    Audio.Stop();
                    Audio.clip = GoSound;
                    Audio.Play();
                }

                // Notify recivers of the event, if there are any
                TimeIsUp.Invoke();
                _hasItExpired = true;
                _hasItAlmostExpired = false;

                // Update the label of the timer
                CountDownText.text = TextAtTheEnd;

                // Stop calling this method for now
                CancelInvoke();
            }
        }

        private void AlmostThere()
        {
            _hasItAlmostExpired = true;
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}
