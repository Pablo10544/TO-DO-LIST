using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Text t;
    public ToDoListManager tdlm;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;
    public float swipeRange;
    public float tapRange;
    public List<GameObject> tabs;
    public GameObject TaskDone;
    public TasksDone td;
    // Update is called once per frame
    void Update()
    {
        Swipe();
        if (tabs.IndexOf(tdlm.currentTabSelected) > 0) {
            TaskDone.SetActive(false);
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W)&&!TaskDone.activeInHierarchy) {
            TaskDone.SetActive(true);
            td.StartCoroutine(td.AnimateText(td.inst.GetComponent<TMP_Text>()));
        }
    }
    public void Swipe() {
        if (!tdlm.c.config.activeInHierarchy) {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                startTouchPosition = Input.GetTouch(0).position;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                currentTouchPosition = Input.GetTouch(0).position;
                Vector2 distance = currentTouchPosition - startTouchPosition;
                if (!stopTouch) {
                    if (distance.x < -swipeRange)
                    {
                        t.text = "left";
                        stopTouch = true;
                        if (TaskDone.activeInHierarchy) {
                            TaskDone.SetActive(false);
                        }
                        else if (tabs.IndexOf(tdlm.currentTabSelected) < tabs.Count)
                            tdlm.LoadTabContent(tabs[tabs.IndexOf(tdlm.currentTabSelected) + 1]);


                    }
                    else if (distance.x > swipeRange) {
                        stopTouch = true;
                        print("right");
                        if (tabs.IndexOf(tdlm.currentTabSelected) > 0)
                            tdlm.LoadTabContent(tabs[tabs.IndexOf(tdlm.currentTabSelected) - 1]);
                        else if (tabs.IndexOf(tdlm.currentTabSelected) == 0) {
                            TaskDone.SetActive(true);
                            td.StartCoroutine(td.AnimateText(td.inst.GetComponent<TMP_Text>()));
                        }
                    }
                    else if (distance.y > swipeRange) {
                        stopTouch = true;
                        t.text = "up";
                    } else if (distance.y < -swipeRange) {
                        t.text = "down";
                        stopTouch = true;

                        tdlm.RestartScene();
                    }
                }
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

                stopTouch = false;
                endTouchPosition = Input.GetTouch(0).position;
                Vector2 distance = endTouchPosition - startTouchPosition;
                if (Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange) {
                }
            }

        } }
}
