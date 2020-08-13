using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    //Los objetos deberian tener un peso con respecto al gato, asi el gato solo mueve objetos si tiene la fuerza necesaria
    //Paso a seguir
        //Bloquear el movimiento y rotacion del objecto en todos sus ejes
        //Cuando el jugador esta en contacto con el trigger verifica la fuerza del gato
        //Si la fuerza del gato es igual o mayor al peso del objecto
        //entonces desbloquea su movimiento en el eje X
        //si el jugador no esta en contacto entonces bloquea su movimieto en X

    public int weight; //peso 1 para gato gordo, peso 2 para gato normal, peso 3 para gato fit

    
    private void OnCollisionEnter2D(Collision2D Objeto)
    {

        //Verificar si el Objecto es gato y la fuerza es suficiente
        // if (Objeto.gameObject.tag == "Player" && Objeto.gameObject.GetComponent<PlayerController>().force >= weight)
        // {
        //     GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //     GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;//Bloquea su movimiento en Y
        // }
    }
    private void OnCollisionExit2D(Collision2D Objeto)
    {
        //Verificar si el Objecto es gato y la fuerza es suficiente, si la fuerza no era suficiente significa que no estaba empujando
        // if (Objeto.gameObject.tag == "Player" && Objeto.gameObject.GetComponent<PlayerController>().force >= weight)
        // {
        //     GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        // }
    }
}
