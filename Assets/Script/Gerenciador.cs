using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gerenciador : MonoBehaviour
{

    public static int numeroTotalDeBlocos;
    public static int numeroDeBlocosDestruidos;
    public Image estrelas;
    public GameObject CanvasGo;
    public static Gerenciador instancia;
    public Bola bola;
    public Plataforma plataforma;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        if (Application.loadedLevel == 1)
        {
            CanvasGo.SetActive(false);
            numeroDeBlocosDestruidos = 0;
        }
    }

    public  void FinalizarJogo()
    {
        CanvasGo.SetActive(true);
        estrelas.fillAmount = (float)numeroDeBlocosDestruidos / (float)numeroTotalDeBlocos;
        plataforma.enabled = false;
        Destroy(bola.gameObject);
    }

    public void AlterarCena(string cena)
    {
        Application.LoadLevel(cena);
    }

    public void fecharAplivativo()
    {
        Application.Quit();
    }
}
