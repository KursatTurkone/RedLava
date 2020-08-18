using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
# endif

public class testere : MonoBehaviour
{

    GameObject []gidilecekNoktalar;
    bool aradakiMesafeyiAl=true;
    Vector3 aradakimesafe;
    int sayac=0;
    bool ileriGeri=true; 
    void Start()
    {
        gidilecekNoktalar = new GameObject[transform.childCount];
        for (int i = 0;i< gidilecekNoktalar.Length;i++ ){
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent); 
        }
    }

  
    void FixedUpdate()
    {
        transform.Rotate(0,0,5);
        noktalaraGit(); 
    }
    void noktalaraGit()
    {
        if (aradakiMesafeyiAl) {
            aradakimesafe= (gidilecekNoktalar[sayac].transform.position - transform.position).normalized;
            aradakiMesafeyiAl = false; 
        }
        float mesafe=Vector3.Distance(transform.position,gidilecekNoktalar[sayac].transform.position); 
        transform.position += aradakimesafe * Time.deltaTime * 10;
        if (mesafe < 0.5f)
        {
            if (sayac == gidilecekNoktalar.Length - 1)
            {
                ileriGeri=false; 
            }
            else if(sayac ==0)
            {
                ileriGeri = true; 
            }
            if (ileriGeri)
            {
                sayac++;
                aradakiMesafeyiAl = true;
            }
            else
            {
                sayac--;
                aradakiMesafeyiAl = true;
            }
        }

    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
   for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i < transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position); 

        }
    }
# endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(testere))]
[System.Serializable]

class testereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        testere script = (testere)target;
        if (GUILayout.Button("Üret",GUILayout.MinWidth(100),GUILayout.Width(100)))
        {
            GameObject yeniObje = new GameObject();
            yeniObje.transform.parent = script.transform;
            yeniObje.transform.position = script.transform.position;
            yeniObje.name = script.transform.childCount.ToString(); 
        }
    }
}
#endif