using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TopScore : MonoBehaviour
{
    [SerializeField] private TMP_Text topScore;
    [SerializeField] private TMP_Text score;
    void Start()
    {
        topScore.text = MainController.Instance.GetTopScore().ToString();
        score.text = MainController.Instance.GetScore().ToString();
    }
}
