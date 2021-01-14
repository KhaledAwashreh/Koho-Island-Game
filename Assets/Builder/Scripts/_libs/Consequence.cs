using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

    public class Consequence : MonoBehaviour
    {
        private List<ResourceData> affectedRresources = new List<ResourceData>();
        public void add(String resourceName,int defaultProductionRate)
        {
            ResourceData newResource=new ResourceData();
            newResource.productionRate = defaultProductionRate;
            newResource.resourceName = resourceName;
            affectedRresources.Add(newResource);
        }

        public void changeProductionRate(String resourceName,int updatedValue )//This method changes the production rate of plants and factories. Animal breeding rate changes by population
        {
            int index = findResourceIndex(resourceName);

            ResourceData result = affectedRresources[index];
            result.productionRate = updatedValue;

        }

        public int findResourceIndex(string resourceName)
        {
            int index=0;
            for(int i=0;i<affectedRresources.Count;i++)
           {
               if (affectedRresources[i].resourceName.Equals(resourceName) == true)
               {
                   index = i;
               }
            }

            return index;


        }
        
        




    }