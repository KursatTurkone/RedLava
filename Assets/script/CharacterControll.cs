using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterControll : MonoBehaviour
{
    public Sprite[] beklemeAnim;
    public Sprite[] ziplamaAnim;
    public Sprite[] yurumeAnim;
    public bl_Joystick Joystick;
    public Text canText;
    public Text olumText; 
    public Image DeathScene;
    public Text Altin; 
    int can = 10;
    public int toplamAltin; 
    int altinsayac=0; 
    Animator anim; 

    int beklemeAnimSayac=0;
    int yurumeAnimSayac=0; 
    SpriteRenderer spriteRenderer;
    float horizontal = 0;
    float beklemeAnimZaman = 0;
    float yurumeAnimZaman = 0;
    float siyahArkaPlanSayaci =0;
    float anaMenuyeDonZaman = 0; 
    Rigidbody2D physics;
    Vector3 vec;
    Vector3 camLastPos;
    Vector3 camFirstPos; 
    bool jumpJustOnce=true;

    GameObject camera; 
    void Start()
    {
        Time.timeScale = 1;
        olumText.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        physics = GetComponent<Rigidbody2D>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("kacincilevel"))
        {
            PlayerPrefs.SetInt("kacincilevel", SceneManager.GetActiveScene().buildIndex);
        }
        
        camFirstPos = camera.transform.position - transform.position;
        canText.text = "Can " + can;
        anim = GetComponent<Animator>(); 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&jumpJustOnce)
        {
            jumpJustOnce = false; 
            physics.AddForce(new Vector2(0, 350)); 

        }
    }

    public void JumpButton(int gelenButon)
    {
        if (gelenButon == 1 && jumpJustOnce)
        {
            jumpJustOnce = false;
            physics.AddForce(new Vector2(0, 350));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        karakterHareket(); 
        Animation(); 
        if(can<=0)
        {
            olumText.text = "You Died";
            canText.enabled = false;
            olumText.enabled = true;
            siyahArkaPlanSayaci += 0.04f;
            DeathScene.color = new Color(0, 0, 0, siyahArkaPlanSayaci);
            anaMenuyeDonZaman += Time.deltaTime;
            if (anaMenuyeDonZaman>1)
            {
                SceneManager.LoadScene("anaMenu"); 
            }


        }
    }
    private void LateUpdate()
    {
        cameraControl(); 
    }
    void karakterHareket()
    {
         
        
        horizontal =  Joystick.Horizontal; 

        vec = new Vector3(horizontal*2,physics.velocity.y,0);
        physics.velocity = vec; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpJustOnce = true; 
            
    }

    void Animation()
    {
        if (jumpJustOnce)
        {
            if (horizontal == 0)
            {
                beklemeAnimZaman += Time.deltaTime;
                if (beklemeAnimZaman > 0.05f)
                {

                    spriteRenderer.sprite = beklemeAnim[beklemeAnimSayac++];
                    if (beklemeAnimSayac == beklemeAnim.Length)
                    {
                        beklemeAnimSayac = 0;
                    }
                    beklemeAnimZaman = 0;
                }
            }
            else if (horizontal > 0)
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f)
                {

                    spriteRenderer.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {


                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f)
                {

                    spriteRenderer.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            if(physics.velocity.y<0)
            {
                spriteRenderer.sprite = ziplamaAnim[0];
            }
            else
            {
                spriteRenderer.sprite = ziplamaAnim[1];
            }
            if(horizontal>0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="kursun")
        {
            can--; 
            canText.text = "Can " + can;
        }
        if (collision.gameObject.tag == "dusman")
        {
            can -=10;
            canText.text = "Can " + can;
        }
        if (collision.gameObject.tag == "testere")
        {
            can -= 25;
            canText.text = "Can " + can;
        }
        if (collision.gameObject.tag == "levelbitsin")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1); 
        }

        if (collision.gameObject.tag == "Chest")
        {
            if (can < 100) {
                can += 10;
            }

            collision.GetComponent<canver>().enabled = true;            
         canText.text = "Can " + can;
           collision.GetComponent<BoxCollider2D>().enabled = false; 
            Destroy(collision.gameObject, 3); 
        }
        if (collision.gameObject.tag == "Gold")
        {
            if (can < 100)
            {
                can += 1;
            }
                altinsayac++;
            Altin.text = toplamAltin + " / "+altinsayac;
            
            canText.text = "Can " + can;
            collision.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Water")
        {
            can = 0;
           
            
          
            canText.text = "Can " + can;
        
        }
    }
    void cameraControl()
    {
        camLastPos = camFirstPos + transform.position;
        camera.transform.position = camLastPos; 

    }
    
}
