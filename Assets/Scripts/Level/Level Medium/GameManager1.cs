using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;

    [Header("Player")]
    public Car player;

    public float baseSpeed;
    public int lap;
    public bool check;

    [Header("GameObj")]
    public Car[] car;
    public Transform[] target;
    public Controller controllPad;
    public Transform cam;

    [Header("Menu")]
    public GameObject startMenu;
    public GameObject selectMenu;
    public GameObject ui;
    public GameObject finishMenu;

    [Header("Text")]
    public TextMeshProUGUI bestLapTimeText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI curSpeedText;
    public TextMeshProUGUI[] lapTimeText;

    float curTime;
    float bestLapTime;

    private void Awake() {
        if(instance == null)
            instance = this;
        
        SpeedSet();
        BestLapTimeSet();
    }

    public void GameStart() {
        StartCoroutine("StartCount");
    }

    void BestLapTimeSet() {
        bestLapTime = PlayerPrefs.GetFloat("BestLap");
        bestLapTimeText.text = string.Format("{0:00}{1:00.00}", (int)(bestLapTime / 60 % 60), bestLapTime % 60);

        if(bestLapTime == 0) {
            bestLapTimeText.text = "Best  -";
        }
    }

    public void LapTime() {
        if(lap == 5) {
            SE_Manager.instance.PlaySound(SE_Manager.instance.goal);
            cam.parent = null;
            StopCoroutine("Timer");
            finishMenu.SetActive(true);

            player.player = false;
            player.StartAI();
            controllPad.gameObject.SetActive(false);
            player.transform.GetChild(5).gameObject.SetActive(false);

            if(curTime < bestLapTime | bestLapTime == 0) {
                bestLapTimeText.gameObject.SetActive(false);
                bestLapTimeText.text = string.Format("Best {0:00}:{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
                bestLapTimeText.gameObject.SetActive(true);

                PlayerPrefs.SetFloat("BestLap", curTime);
            }
        }

        lapTimeText[lap-1].gameObject.SetActive(false);
        lapTimeText[lap-1].text = string.Format("{0:00}{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
        lapTimeText[lap-1].gameObject.SetActive(true);
    }

    IEnumerator StartCount() {
        selectMenu.SetActive(false);
        ui.SetActive(true);

        SE_Manager.instance.PlaySound(SE_Manager.instance.count[3]);
        countText.text = "3";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        SE_Manager.instance.PlaySound(SE_Manager.instance.count[2]);
        countText.gameObject.SetActive(false);
        countText.text = "2";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        SE_Manager.instance.PlaySound(SE_Manager.instance.count[1]);
        countText.gameObject.SetActive(false);
        countText.text = "1";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        SE_Manager.instance.PlaySound(SE_Manager.instance.count[0]);
        countText.gameObject.SetActive(false);
        countText.text = "GO!";
        countText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        countText.gameObject.SetActive(false);

        controllPad.gameObject.SetActive(true);
        player.player = true;
        check = true;

        controllPad.StartController();
        for(int i=0; i<car.Length; i++)
            car[i].StartAI();

        StartCoroutine("Timer");
    }

    IEnumerator Timer() {
        while(true) {
            curTime += Time.deltaTime;

            curTimeText.text = string.Format("{0:00}:{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
            yield return null;
        }
    }

    void SpeedSet() {
        for(int i=0; i<car.Length; i++) {
            car[i].carSpeed = Random.Range(baseSpeed, baseSpeed + 0.5f);
        }
    }

    public void StartBtn() {
        SE_Manager.instance.PlaySound(SE_Manager.instance.btn);

        startMenu.SetActive(false);
        selectMenu.SetActive(true);
    }

    public void ReStart() {
        SE_Manager.instance.PlaySound(SE_Manager.instance.btn);
        SceneManager.LoadScene("SampleScene");
    }
}
