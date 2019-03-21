using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PhotoAnimController : MonoBehaviour
{

    [Header("Count Down")]
    public Transform[] countDownElements;
    Text[] txt_countDownTexts;
    //Image[] img_countDownTexts;
    Color color_countDownText;
    Color color_countDownText_transparent;

    Sequence tweenSeq_countDown;

    public float duration_countDown = 1f; // Duration of each text's scaling animation

    [Header("Shot")]
    public Image img_shot;
    public float duration_shot = 0.5f;
    public AudioSource audio_shot;

    Sequence tweenSeq_shot;

    // An event which will be triggered when the countdown animation finishes
    public delegate void CountDownFinish();
    public static event CountDownFinish OnCountDownFinish;

    // An event which will be triggered when the shot animation finishes
    public delegate void ShotFinish();
    public static event ShotFinish OnShotFinish;

    public void Init()
    {

        #region SETUP_COUNTDOWN_ANIM
        // Set up the texts' color
        if (countDownElements[0].gameObject.GetComponent<Text>() != null)
            color_countDownText = countDownElements[0].gameObject.GetComponent<Text>().color;
        else if (countDownElements[0].gameObject.GetComponent<Image>() != null)
            color_countDownText = countDownElements[0].gameObject.GetComponent<Image>().color;

        color_countDownText_transparent = new Color(color_countDownText.r, color_countDownText.g, color_countDownText.b, 0f);

        txt_countDownTexts = new Text[countDownElements.Length];
        //img_countDownTexts = new Image[countDownElements.Length];
        for (int i = 0; i < countDownElements.Length; i++)
        {
            // Set reference
            txt_countDownTexts[i] = countDownElements[i].gameObject.GetComponent<Text>();
            //img_countDownTexts[i] = countDownElements[i].gameObject.GetComponent<Image>();
            // Hide the texts
            txt_countDownTexts[i].color = color_countDownText_transparent;
            //img_countDownTexts[i].color = color_countDownText_transparent;
        }

        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);

        tweenSeq_countDown = DOTween.Sequence().SetAutoKill(false).Pause();

        for (int i = 0; i < countDownElements.Length; i++)
        {
            tweenSeq_countDown.Append(txt_countDownTexts[i].DOColor(color_countDownText_transparent, 0f)); // Set to transparent
            tweenSeq_countDown.Append(txt_countDownTexts[i].DOColor(color_countDownText, 0f)); // Set to full opacity
            //Debug.LogWarning("COLOR: " + i);
            tweenSeq_countDown.Append(countDownElements[i].DOScale(0f, duration_countDown).SetEase(Ease.InSine)); // DOScale
            //Debug.LogWarning("SCALE: " + i);
            tweenSeq_countDown.Join(txt_countDownTexts[i].DOColor(color_countDownText_transparent, duration_countDown)); // Set to transparent
            //Debug.LogWarning("COLOR2: " + i);
        }

        // CountDownFinish callback here
        tweenSeq_countDown.AppendCallback(() => Callback_CountDownFinish());

        #endregion SETUP_COUNTDOWN_ANIM

        #region SETUP_SHOT_ANIM

        tweenSeq_shot = DOTween.Sequence().SetAutoKill(false).Pause();
        tweenSeq_shot.Append(img_shot.DOFade(1f, 0.01f)); // Set to full opacity
        tweenSeq_shot.Append(img_shot.DOFade(0f, duration_shot).SetEase(Ease.OutSine)); // Fade out
        tweenSeq_shot.AppendCallback(() => Callback_ShotFinish());
        // Play shot audio
        if (audio_shot != null)
            tweenSeq_shot.AppendCallback(() => audio_shot.Play());

        #endregion SETUP_SHOT_ANIM

    }

    void Callback_CountDownFinish()
    {
        if (OnCountDownFinish != null)
            OnCountDownFinish();
    }

    void Callback_ShotFinish()
    {
        if (OnShotFinish != null)
            OnShotFinish();
    }

    public void PlayCountDownAnim()
    {
        // Play Count Down Anim (DOTween Sequence)
        tweenSeq_countDown.Restart();
    }

    public void PlayShotAnim()
    {
        // Play Shot Anim (DOTween Sequence)
        tweenSeq_shot.Restart();
    }

   
}
