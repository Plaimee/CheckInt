using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ยังคงต้องใช้สำหรับ RawImage

public class PreviewImage : MonoBehaviour
{
    [Tooltip("ใส่รูปภาพทั้งหมด (Texture) ที่ต้องการสุ่มแสดงผล")]
    public Texture[] images;

    [Tooltip("ลาก UI RawImage ที่จะใช้แสดงผลมาใส่")]
    public RawImage displayImage;

    [Tooltip("เวลาที่ต้องการให้แต่ละภาพแสดง (วินาที)")]
    public float displayTime = 5f;

    [Tooltip("เวลาที่ใช้ในการทำเอฟเฟกต์ Fade In-Out (วินาที)")]
    public float fadeTime = 1f;

    private int currentIndex = -1;

    void Start()
    {
        if (images.Length == 0)
        {
            Debug.LogError("ยังไม่ได้กำหนดรูปภาพใน Array 'images'");
            return;
        }
        if (displayImage == null)
        {
            Debug.LogError("ยังไม่ได้ลาก UI RawImage มาใส่ในช่อง 'displayImage'");
            return;
        }

        StartCoroutine(CycleImages());
    }

    IEnumerator CycleImages()
    {
        while (true)
        {
            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, images.Length);
            } while (nextIndex == currentIndex && images.Length > 1);

            currentIndex = nextIndex;

            yield return StartCoroutine(Fade(0f, 1f));
            yield return new WaitForSeconds(displayTime);
            yield return StartCoroutine(Fade(1f, 0f));
        }
    }
    
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        
        if (endAlpha > startAlpha)
        {
            displayImage.texture = images[currentIndex];
        }

        while (timer < fadeTime)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeTime);
            displayImage.color = new Color(1f, 1f, 1f, newAlpha);

            timer += Time.deltaTime;
            yield return null;
        }

        displayImage.color = new Color(1f, 1f, 1f, endAlpha);
    }
}