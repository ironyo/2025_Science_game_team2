using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemPoolManager : MonoBehaviour
{
     [SerializeField] private GameObject elecItemPrefab;
     [SerializeField]private List<Transform> SpawnPoints;
     [SerializeField] private QuizManager _quizManager;
     private GameObject[] elecItemPool;
     private int electricPoolSize = 4;
     
     [SerializeField] private GameObject bombItemPrefab;
     private GameObject[] bombItemPool;
     private int bombPoolSize = 4;

     private void Start()
     {
          elecItemPool = new GameObject[electricPoolSize];    
          for (int i = 0; i < electricPoolSize; i++)
          {
               GameObject currentItem = Instantiate(elecItemPrefab, transform);
               elecItemPool[i] = currentItem;
               currentItem.transform.position = gameObject.transform.position;
               currentItem.SetActive(false);
          }
          
          bombItemPool = new GameObject[bombPoolSize];    
          for (int i = 0; i < bombPoolSize; i++)
          {
               GameObject currentItem = Instantiate(bombItemPrefab, transform);
               bombItemPool[i] = currentItem;
               currentItem.transform.position = gameObject.transform.position;
               currentItem.SetActive(false);
          }
     }

     public void SpawnItem(int itemIndex)
     {
          switch (itemIndex)
          {
               case 0:
                    for (int i = 0; i < electricPoolSize; i++)
                    {
                         if (!elecItemPool[i].activeSelf)
                         {
                              elecItemPool[i].transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].position;
                              elecItemPool[i].gameObject.GetComponent<electricItem>().quizManager = _quizManager;
                              elecItemPool[i].SetActive(true);
                              break;
                         }
                    }

                    break;
               case 1:
                    for (int i = 0; i < bombPoolSize; i++)
                    {
                         if (!bombItemPool[i].activeSelf)
                         {
                              bombItemPool[i].transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].position;
                              bombItemPool[i].SetActive(true);
                              break;
                         }
                    }

                    break;
          }

     }
}
