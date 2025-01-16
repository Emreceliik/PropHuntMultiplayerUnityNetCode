using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomAnimalSelector : MonoBehaviourPun
{
    public GameObject pig, dog, sheep, horse, cow;
    private GameObject[] animals; // Hayvanlar listesi
    private int currentAnimalIndex; // Mevcut hayvanın indeksi
    public GameObject camera; // Kamera
    private AnimalMovement animalMovement;

    void Start()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false; // Bu bileşeni devre dışı bırak
            return;
        }

        animals = new GameObject[] { pig, dog, sheep, horse, cow };

        // Hayvanların başlangıçta deaktif hale getirilmesi
        foreach (GameObject animal in animals)
        {
            animal.SetActive(false);
        }
        // Her oyuncu rastgele bir hayvan seçer
        currentAnimalIndex = Random.Range(0, animals.Length);
        photonView.RPC("ChangeAnimal", RpcTarget.AllBuffered, currentAnimalIndex);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("pig"))
                    {
                        photonView.RPC("ChangeAnimal", RpcTarget.AllBuffered, 0);
                    }
                    else if (collider.CompareTag("dog"))
                    {
                        photonView.RPC("ChangeAnimal", RpcTarget.AllBuffered, 1);
                    }
                    else if (collider.CompareTag("sheep"))
                    {
                        photonView.RPC("ChangeAnimal", RpcTarget.AllBuffered, 2);
                    }
                    else if (collider.CompareTag("horse"))
                    {
                        photonView.RPC("ChangeAnimal", RpcTarget.AllBuffered, 3);
                    }
                    else if (collider.CompareTag("cow"))
                    {
                        photonView.RPC("ChangeAnimal", RpcTarget.AllBuffered, 4);
                    }
                }
            }
        }
    }

    private void ChangeAnimalLocal(int animalIndex)
    {
        GameObject newAnimal = animals[animalIndex];

        // Tüm alt objeleri pasif hale getir
        foreach (GameObject animal in animals)
        {
            animal.SetActive(false);
        }

        // Yeni hayvanı aktif hale getir
        newAnimal.SetActive(true);

        // Yeni aktif olan hayvanın animator bileşenini al
        animalMovement = GetComponent<AnimalMovement>();
        if (animalMovement != null)
        {
            animalMovement.SetActiveAnimator();
        }

        camera.SetActive(true);
    }

    [PunRPC]
    public void ChangeAnimal(int animalIndex)
    {
        currentAnimalIndex = animalIndex; // Mevcut hayvan indeksini güncelle
        ChangeAnimalLocal(animalIndex);
    }
}
