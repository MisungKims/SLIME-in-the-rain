/**
 * @brief ī�޶��� ������
 * @author ��̼�
 * @date 22-07-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static Camera cam;
    private static Vector3 cameraOriginalPos;

    private void Start()
    {
        cam = Camera.main;
    }

    // ī�޶� ���
    // duration : �����ð�, magnitude : ����
    public static IEnumerator StartShake(float duration, float magnitude)
    {
        Debug.Log("camera shake");

        float timer = 0;
        cameraOriginalPos = cam.transform.localPosition;

        while (timer <= duration)
        {
            cam.transform.localPosition = Random.insideUnitSphere * magnitude + cameraOriginalPos;

            timer += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = cameraOriginalPos;
    }
}
