using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseAvarta : MonoBehaviour
{
    public GameObject astronault;
    public GameObject astronault_avarta;
   
    public GameObject Ship;
    public GameObject Ship_avarta;
    public GameObject Bird;
    public GameObject Bird_avarta;

    private bool Shipchose = true;
    private bool Birdchose = false;
    private bool astronaultchose = false;
    // Start is called before the first frame update
    public void OnclickChangeImage()
    {
        AudioManager.Instance.PlayClick();
        if (Shipchose)
        {

            Ship.SetActive(false);
            Ship_avarta.SetActive(false);

            Bird.SetActive(true);
            Bird_avarta.SetActive(true);
            

            Shipchose = false;
            Birdchose = true;
            astronaultchose = false;
        }
        else if (Birdchose)
        {

            Bird.SetActive(false);
            Bird_avarta.SetActive(false);

            astronault.SetActive(true);
            astronault_avarta.SetActive(true);


            Shipchose = false;
            Birdchose = false;
            astronaultchose = true;
        }
        else if (astronaultchose)
        {

            astronault.SetActive(false);
            astronault_avarta.SetActive(false);

            Ship.SetActive(true);
            Ship_avarta.SetActive(true);


            Shipchose = true;
            Birdchose = false;
            astronaultchose = false;
        }
    }
}
