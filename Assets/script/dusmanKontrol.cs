using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
# endif

public class dusmanKontrol : MonoBehaviour
{

    GameObject[] gidilecekNoktalar;
    GameObject karakter;
    RaycastHit2D ray; 
    bool aradakiMesafeyiAl = true;
    Vector3 aradakimesafe;
    int sayac = 0;
    bool ileriGeri = true;
   public LayerMask layermask;
    int hiz=5;
    public Sprite onTaraf;
    public Sprite arkaTaraf;
    SpriteRenderer spriteRenderer;
    public GameObject kursun;
    float atesZamani=0;
    void Start()
    {
        gidilecekNoktalar = new GameObject[transform.childCount];
        karakter = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>(); 

        for (int i = 0; i < gidilecekNoktalar.Length; i++)
        {
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent);
        }
    }


    void FixedUpdate()
    {
        beniGordumu();
        if (ray.collider.tag=="Player")
        {
            hiz = 8;
            spriteRenderer.sprite = onTaraf;
            atesEt(); 
            
        }
        else
        {
            hiz = 4;
            spriteRenderer.sprite = arkaTaraf;
        }
        noktalaraGit();
    }
    void atesEt()
    {
        atesZamani += Time.deltaTime;
        if (atesZamani >2)//0.2f,1 Random.Range(1, 1,)
        {
            Instantiate(kursun, transform.position, Quaternion.identity);
            atesZamani = 0; 
        }
    }
    void beniGordumu()
    {
        Vector3 rayYonum = karakter.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position,rayYonum,1000,layermask);
        Debug.DrawLine(transform.position, ray.point, Color.magenta);
    }
    void noktalaraGit()
    {
        if (aradakiMesafeyiAl)
        {
            aradakimesafe = (gidilecekNoktalar[sayac].transform.position - transform.position).normalized;
            aradakiMesafeyiAl = false;
        }
        float mesafe = Vector3.Distance(transform.position, gidilecekNoktalar[sayac].transform.position);
        transform.position += aradakimesafe * Time.deltaTime * hiz;
        if (mesafe < 0.5f)
        {
            if (sayac == gidilecekNoktalar.Length - 1)
            {
                ileriGeri = false;
            }
            else if (sayac == 0)
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
   public Vector2 getYon()
    {
        return (karakter.transform.position - transform.position).normalized;
}
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);

        }
    }
# endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(dusmanKontrol))]
[System.Serializable]

class dusmanKontrolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        dusmanKontrol script = (dusmanKontrol)target;
        EditorGUILayout.Space();
        if (GUILayout.Button("Üret", GUILayout.MinWidth(100), GUILayout.Width(100)))
        {
            GameObject yeniObje = new GameObject();
            yeniObje.transform.parent = script.transform;
            yeniObje.transform.position = script.transform.position;
            yeniObje.name = script.transform.childCount.ToString();
        }
        EditorGUILayout.Space(); 
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("arkaTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("kursun"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif