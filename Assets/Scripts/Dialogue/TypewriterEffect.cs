using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField]private float typeWriterSpeed = 50f;
    AudioSource characterVoice;
    public bool isRunning { get; private set; }

    private void Start()
    {
        characterVoice = GetComponent<AudioSource>();
        characterVoice.volume = 0.30f;
    }

    // punctuation list for pausing text
    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>(){'.', '!', '?' }, 0.6f),
        new Punctuation(new HashSet<char>(){',', ';', ':' }, 0.3f)
    };

    private Coroutine typingCoroutine;

    public void Run(string textToType, Text textLabel, List<AudioClip> charVoice)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel, charVoice));
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }
    private IEnumerator TypeText(string textToType, Text textLabel, List<AudioClip> charVoice)
    {
        isRunning = true;
        float t = 0;
        int charIndex = 0;
        

        // types text on screen
        while (charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * typeWriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for (int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;

                // should hopefully make a sound as text displays
                if (charVoice != null)
                {
                    int random = Random.Range(0, charVoice.Count);
                    characterVoice.clip = charVoice[random];
                    characterVoice.Play();
                }
                



                textLabel.text = textToType.Substring(0, i + 1);

                if (IsPunctuation(textToType[i], out float waitTime) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }

        isRunning = false;
    }

    // this is used for determining how long the typewriter effect pauses when there's punctuation based on the list at the top of the file
    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
