using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mov : MonoBehaviour
{
    //Referencias
    public Transform trans;
    public Animator animator;
    public Transform transModelo;
    public Rigidbody myRb;
    public Collider myColl;

    //stats del personaje en uso
    public float velocidad = 0.1f;
    public int saltos = 1;
    public int saltosMaximos;
    public float fuerzaSalto = 7;

    //etc
    public bool moviendose;
    public bool padelante = true;
    public bool saltando;
    public bool cayendo;
    public bool canJump;
    public Vector3 posicionMedia;
    public Vector3 posicionDerecha;
    public Vector3 posicionIzquierda;

    //
    public LayerMask escenario;


    private void Start()
    {
        saltosMaximos = saltos;
    }

    public void FixedUpdate()
    {
        //raycast pa abajo
        if (Physics.Raycast(posicionMedia, Vector3.down, 0.6f, escenario) || Physics.Raycast(posicionDerecha, Vector3.down, 0.6f, escenario) || Physics.Raycast(posicionIzquierda, Vector3.down, 0.6f, escenario))
        {
            cayendo = false;
            Aterrizando();
        }
        else
        {
            cayendo = true;
        }

        

        // Raycast de izquierda a derecha
        if (Physics.Raycast(gameObject.transform.position, Vector3.right, 1, escenario))
        {



        }

        if (Physics.Raycast(gameObject.transform.position, Vector3.left, 1, escenario))
        {



        }

        // Salto
        if (Physics.Raycast(posicionMedia, Vector3.down, 1, escenario) || Physics.Raycast(posicionDerecha, Vector3.down, 1, escenario) || Physics.Raycast(posicionIzquierda, Vector3.down, 1, escenario))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    void Update()
    {
        //Track de posicion
        posicionMedia = trans.position;
        posicionDerecha = (posicionMedia + new Vector3(0.5f, 0, 0));
        posicionIzquierda = (posicionMedia - new Vector3(0.5f, 0, 0));

        //movimiento lao a lao y frenao
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            moviendose = false;
            Frenando();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moviendose = true;
            Moverse(1);
            if (padelante == false)
            {
                Girando();
            }
            
           
        }
        else if (Input.GetKey(KeyCode.A))
        {

            moviendose = true;
            Moverse(-1);
            if (padelante)
            {
                Girando();
            }

        }
        else
        {
            moviendose = false;
            Frenando();
        }

        //salto y caida

        if (Input.GetKey(KeyCode.Space))
        {
            if (saltos >= 1 && canJump)
            {
                saltos -= 1;
                saltando = true;
                Saltando();
            }
        }
    }

	

    void Moverse(float dir)
    {
        if (moviendose)
        {
            if (saltando)
            {
                myRb.AddForce((velocidad/7) * dir, 0, 0, ForceMode.VelocityChange);
            }
            else if (saltando == false)
            {
                myRb.AddForce(velocidad * dir, 0, 0, ForceMode.VelocityChange);
            }
            
        }
    }
    void Frenando()
    {
        if(saltando == false)
        {
            if (cayendo == false)
            {
                myRb.velocity = new Vector3(0, 0, 0);
                myRb.angularVelocity = new Vector3(0, 0, 0);
            }
        }
       
    }
    void Saltando()
    {
        float contador = 0;
        myRb.useGravity = false;

        if (moviendose)
        {
            myRb.AddForce(0, fuerzaSalto / 3, 0, ForceMode.Impulse);
        }
        else
        {
            myRb.AddForce(0, fuerzaSalto, 0, ForceMode.Impulse);
        }
        
        contador += 1 * Time.deltaTime;
        if (contador <= 0.5)
        {
            myRb.useGravity = true;
        }
    }
    void Aterrizando()
    {
        saltando = false;
        saltos = saltosMaximos;
    }

    void Girando()
    {
        if (padelante)
        {
            animator.Play("PlayerFlipMinus");
            padelante = false;
        }
        else if(padelante == false)
        {
            animator.Play("PlayerFlipPlus");
            padelante = true;
        }
    }
}
