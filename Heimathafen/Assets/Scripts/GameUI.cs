using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private Text subtitles;
    public float subtitleDisplayTime;
    private Image image1;
    private Image image2;
    private Color white;
    private Color transparent;
    private Text messages;

    private enum UIobjects
    {
        Subtitle,
        Image1,
        Image2,
        Messages
    }

    // Start is called before the first frame update
    void Start()
    {
        subtitles = GameObject.Find("Subtitles").GetComponent<Text>();
        image1 = GameObject.Find("SubImg1").GetComponent<Image>();
        image2 = GameObject.Find("SubImg2").GetComponent<Image>();
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
        messages = GameObject.Find("Messages").GetComponent<Text>();
    }

    public void ChangeImage1(Sprite img)
    {
        image1.sprite = img;
        image1.gameObject.SetActive(true);
        StartCoroutine(RemoveSubtitles(UIobjects.Image1));
        Debug.Log("img1");
    }

    public void ChangeImage2(Sprite img)
    {
        image2.sprite = img;
        image2.gameObject.SetActive(true);
        StartCoroutine(RemoveSubtitles(UIobjects.Image2));
        Debug.Log("img2");
    }

    public void ChangeSubtitles(string subtext)
    {
        subtitles.text = subtext;
        subtitles.gameObject.SetActive(true);
        StartCoroutine(RemoveSubtitles(UIobjects.Subtitle));
    }

    public void ChangeMessages(string subtext)
    {
        messages.text = subtext;
        messages.gameObject.SetActive(true);
        StartCoroutine(RemoveSubtitles(UIobjects.Messages));
    }

    IEnumerator RemoveSubtitles(UIobjects obj)
    {
        yield return new WaitForSeconds(subtitleDisplayTime);
        switch (obj)
        {
            case UIobjects.Subtitle:
                subtitles.gameObject.SetActive(false);
                break;
            case UIobjects.Image1:
                image1.gameObject.SetActive(false);
                break;
            case UIobjects.Image2:
                image2.gameObject.SetActive(false);
                break;
            case UIobjects.Messages:
                messages.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("Fehler in UI");
                break;
        }
    }
}
