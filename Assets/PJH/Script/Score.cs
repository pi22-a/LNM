using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score instance;
    private void Awake() {
        instance = this;
    }

    //초기 점수
    int startScore = 0;
    //현재 점수 UI
    public TextMeshProUGUI scoreText;
    //현재 점수
    int curScore;
    public int CUR_SCORE
    {
        get {return curScore;}
        set 
        {
            curScore = value;
            scoreText.text = $"Score: {curScore}";
            //현재 점수가 최고 점수 보다 크면
            if(curScore > HIGH_SCORE)
            {
                //현재 점수 = 최고 점수
                HIGH_SCORE = curScore;
                scoreText.color = Color.yellow;
            }

        }
    }
    //최고 점수 UI
    public TextMeshProUGUI highScoreText;
    //최고 점수
    int highScore;
    //불러온 최고 점수
    int oldHighScore;
    public int OLD_HIGH_SCORE
    {
        get {return oldHighScore;}
        set
        {
            oldHighScore = value;
        }
    }
    //최고 점수 저장 키
    const string highScoreKey = "HIGH_SCORE";
    public int HIGH_SCORE
    {
        get {return highScore;}
        set
        {
            highScore = value;
            highScoreText.text = $"HighScore: {highScore}";
            //현재 점수 = 최고 점수 일때
            if(CUR_SCORE >= highScore)
            {
                highScoreText.color = Color.yellow;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //점수 초기화
        CUR_SCORE = startScore;
        //최고 점수 읽어오기
        OLD_HIGH_SCORE = PlayerPrefs.GetInt(highScoreKey, 0);
        HIGH_SCORE = OLD_HIGH_SCORE;
        
    }

    // Update is called once per frame
    void Update()
    {
        //텍스트를 현재 점수로 변경
        scoreText.text = $"Score: {curScore}";
    }

    //밸류만큼 점수 증감
    public void incScore(int Value)
    {
        curScore += Value;
    }
    //최고 점수 저장
    public void saveHightScore()
    {
        if(HIGH_SCORE > OLD_HIGH_SCORE)
        {
            PlayerPrefs.SetInt(highScoreKey, HIGH_SCORE);
        }
    }
}
