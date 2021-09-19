using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimento : MonoBehaviour
{
    public CharacterController controller;
    public Transform chao;
    public LayerMask maskChao;
    public float speed = 12f;
    public float gravidade = -9.81f;
    public float alturaPulo = 3f;
    public float distanciaChao = 0.4f;
    public bool tocouChao;
    Vector3 velocidade;


    void Update()
    {
        tocouChao = Physics.CheckSphere(chao.position, distanciaChao, maskChao);

        if(tocouChao && velocidade.y < 0)
        {
            velocidade.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;
        controller.Move(mover * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && tocouChao)
        {
            velocidade.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        velocidade.y += gravidade * Time.deltaTime;
        controller.Move(velocidade * Time.deltaTime);
    }
}
