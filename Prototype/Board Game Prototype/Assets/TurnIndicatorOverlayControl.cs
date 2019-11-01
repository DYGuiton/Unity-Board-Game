using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnIndicatorOverlayControl : MonoBehaviour {

    public CanvasGroup canvasGroup;

    public bool visible = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void indicatePlayerTurn(string playerName, Material playerMaterial) {
        gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = playerName + "'s Turn";
        gameObject.GetComponent<Image>().color = playerMaterial.color;
        FadeIn();
    }

    private void FadeIn() {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0.7f));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void FadeOut() {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0));
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f) {


        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true) {

            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            canvasGroup.alpha = currentValue;
            if (percentageComplete >= 1) {
                visible = !visible;
                if (visible) {
                    yield return new WaitForSeconds(2);
                    FadeOut();
                }
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

}
