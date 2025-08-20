using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TipsHandler : MonoBehaviour
{
    [Tooltip("ลาก Text objects ทั้งหมดของคุณมาใส่ในนี้")]
    public Text[] tips;

    [Tooltip("ระยะเวลาที่แต่ละ Tip จะแสดง (วินาที)")]
    public float displayInterval = 5f;

    void Start()
    {
        if (tips == null || tips.Length == 0)
        {
            Debug.LogError("Tips array is not assigned or is empty!");
            return;
        }

        StartCoroutine(CycleTips());
    }

    IEnumerator CycleTips()
    {
        while (true)
        {
            foreach (Text tip in tips)
            {
                tip.gameObject.SetActive(false);
            }

            int randomIndex = Random.Range(0, tips.Length);

            if (tips[randomIndex] != null)
            {
                tips[randomIndex].gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(displayInterval);
        }
    }
}
