using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // 이동 속도 조절을 위한 변수
    public float rotationSpeed = 120f; // 회전 속도 조절을 위한 변수

    void Update()
    {
        // 플레이어 입력을 감지
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 마우스 위치를 플레이어의 로컬 좌표로 변환
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y - transform.position.y;
        Vector3 targetDirection = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;


        // 플레이어가 마우스를 향해 회전
        transform.rotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
        // 입력에 따라 이동 방향 계산
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;

        // 현재 위치에 이동을 적용
        transform.Translate(movement);

        // 마우스 왼쪽 버튼이 눌렸을 때 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("발사");
        }
    }
}
