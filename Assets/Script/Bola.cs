using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{
    public Vector3 Direcao;
    public float Velocidade;
    public GameObject ParticulasBloco;
    public ParticleSystem particulaFolhas;
    public LineRenderer guia;
    public int pontosDaGuia = 3;

    // Use this for initialization
    void Start()
    {
        Direcao.Normalize();
        guia.SetVertexCount(pontosDaGuia);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direcao * Velocidade * Time.deltaTime;
        AtualizarLineRenderer();

    }

    void AtualizarLineRenderer()
    {
        int pontoAtual = 1;
        Vector3 direcaoAtual = Direcao;
        Vector3 ultimaPosicao = transform.position;
        guia.SetPosition(0, ultimaPosicao);
        while (pontoAtual < pontosDaGuia)
        {
            RaycastHit2D hit = Physics2D.Raycast(ultimaPosicao, direcaoAtual);
            ultimaPosicao = hit.point;
            guia.SetPosition(pontoAtual, ultimaPosicao);
            direcaoAtual = Vector3.Reflect(direcaoAtual, hit.normal);
            ultimaPosicao += direcaoAtual * 0.0f;
            print(hit.transform.name);
            pontoAtual++;
        }
    }

    void OnCollisionEnter2D(Collision2D colisor)
    {
        bool colisaoInvalida = false;
        Vector2 normal = colisor.contacts[0].normal;
        Plataforma plataforma = colisor.transform.GetComponent<Plataforma>();
        GeradordeArestas geradordearestas = colisor.transform.GetComponent<GeradordeArestas>();
        if (plataforma != null)
        {
            if (normal != Vector2.up)
            {
                colisaoInvalida = true;
            }
            else
            {
                particulaFolhas.transform.position = plataforma.transform.position;
                particulaFolhas.Play();
            }
        }
        else if (geradordearestas != null)
        {
            if (normal == Vector2.up)
            {
                colisaoInvalida = true;
            }
        }
        else
        {
            colisaoInvalida = false;
            Bounds bordasColisor = colisor.transform.GetComponent<SpriteRenderer>().bounds;
            Vector3 posicaoDeCriacao = new Vector3(colisor.transform.position.x + bordasColisor.extents.x,
                colisor.transform.position.y - bordasColisor.extents.y, colisor.transform.position.z); 
            GameObject particulas = (GameObject)Instantiate(ParticulasBloco, posicaoDeCriacao, Quaternion.identity);
            ParticleSystem componenteParticulas = particulas.GetComponent<ParticleSystem>();
            Destroy(particulas, componenteParticulas.duration + componenteParticulas.startLifetime);
            Destroy(colisor.gameObject);
            Gerenciador.numeroDeBlocosDestruidos++;

        }
        if (!colisaoInvalida)
        {
            Direcao = Vector2.Reflect(Direcao, normal);
            Direcao.Normalize();
        }
        else
        {
            Gerenciador.instancia.FinalizarJogo();
        }
    }
}
