using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Text subtitles;
    public float subtitleDisplayTime;

    // Start is called before the first frame update
    void Start()
    {
        subtitles = GameObject.Find("Subtitles").GetComponent<Text>();
    }

    public void ChangeSubtitles(string subtext)
    {
        subtitles.text = subtext;
        subtitles.gameObject.SetActive(true);
        StartCoroutine(RemoveSubtitles());
    }

    IEnumerator RemoveSubtitles()
    {
        yield return new WaitForSeconds(subtitleDisplayTime);
        subtitles.gameObject.SetActive(false);
    }
}
