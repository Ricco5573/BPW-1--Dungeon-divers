using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Lumin;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 50f, sprintSpeed = 100f;
    private float walkSpeed;
    private int health = 10;
    private bool sprinting;
    private bool parry;
    private bool block;
    [SerializeField]
    private int gold, goldGathered;
    [SerializeField]
    private BoxCollider attackBox;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform swordPos, torchPos;

    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Torch torch;

    private bool hasTorch;
    [SerializeField]
    private GameObject sword, swordPrefab, torchPrefab;
    private bool canAttack = true;
    private Sword swordStats;
    private float camRotate;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;
        GetValues();
        walkSpeed = speed;
        if (sword != null)
        {
            Debug.Log("A sword exists");
            swordStats = sword.GetComponent<Sword>();
            uiManager.UpdateDurability(swordStats.GetDurability(), true);
        }

    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * Time.deltaTime * speed;
            anim.SetBool("Walking", true);
        }
    
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity += -transform.forward * Time.deltaTime * walkSpeed;
            anim.SetBool("Walking", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += transform.right * Time.deltaTime * walkSpeed;
            anim.SetBool("Walking", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity += -transform.right * Time.deltaTime * walkSpeed;
            anim.SetBool("Walking", true);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
        {
            rb.velocity = new Vector3(0, 0, 0);
            anim.SetBool("Walking", false);
        }

        if (torch != null)
        {
            uiManager.UpdateTorch(torch.GetDura(), true);
        }
        else
        {
            uiManager.UpdateTorch(0, false);
            hasTorch = false;
        } 
    }
    void Update()
    {
        RaycastHit hit2;
        Physics.Raycast(mainCam.transform.position, mainCam.transform.TransformDirection(Vector3.forward), out hit2, 5);
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.TransformDirection(Vector3.forward), Color.red);
        if (hit2.collider != null)
        {
            if (hit2.collider.gameObject.tag == "Gold")
            {
                uiManager.EnablePickup("Gold", true, false);
            }
            else if (hit2.collider.gameObject.tag == "Sword")
            {
                uiManager.EnablePickup("Sword", true, false);

            }
            else if (hit2.collider.gameObject.tag == "Door")
            {
                uiManager.EnablePickup("door", true, true);

            }
            else if (hit2.collider.gameObject.tag == "Portal")
            {
                uiManager.EnablePickup("Portal, Cost: " + hit2.collider.gameObject.GetComponent<PortalManager>().GetCost().ToString(), true, true);
            }
            else if (hit2.collider.gameObject.tag == "Swordshop")
            {
                uiManager.EnablePickup("Sword, Cost: 250", true, false);
            }
            else if (hit2.collider.gameObject.tag == "Torchshop")
            {
                uiManager.EnablePickup("Torch, Cost: 150", true, false);
            }
            else if (hit2.collider.gameObject.tag == "Healthpotion")
            {
                uiManager.EnablePickup("Health potion", true, false);
            }
            else
            {
                uiManager.EnablePickup("Pickup", false, false);
            }
        }
    
        else
        {
            uiManager.EnablePickup("Pickup", false, false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
            speed = sprintSpeed;
            anim.SetBool("Sprinting", true);
        }
        else
        {
            sprinting = false;
            speed = walkSpeed;
            anim.SetBool("Sprinting", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (sword != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (swordStats.GetDurability() >= 0 && canAttack)
                {
                    StartCoroutine(Attack());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && canAttack && swordStats.GetDurability() >= 0)
            {
                StartCoroutine(Block());
            }
        }
        else
        {
            uiManager.UpdateDurability(0, false);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            block = false;
        }
        camRotate = Input.GetAxis("Mouse Y") * 5;
        mainCam.transform.Rotate(-camRotate, 0, 0);
        
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * 5, 0);
    }

    private IEnumerator Attack()
    {

        attackBox.enabled = true;
        canAttack = false;
        rb.velocity = gameObject.transform.forward * speed;
        anim.SetBool("attacking", true);
        yield return new WaitForSecondsRealtime(0.3f);      

        attackBox.enabled = false;
        swordStats.SetDurability(1, false, false);
        uiManager.UpdateDurability(swordStats.GetDurability(), true);
        yield return new WaitForSecondsRealtime(0.5f);

        canAttack = true;
        anim.SetBool("attacking", false);
        yield return null;
    }

    private IEnumerator Block()
    {
        block = true;
        parry = true;
        canAttack = false;
        yield return new WaitForSecondsRealtime(0.1f);
        parry = false;
        yield return new WaitForSecondsRealtime(0.3f);
        canAttack = true;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && swordStats != null)
        {
            other.gameObject.GetComponent<Enemy>().Hit(swordStats.GetDamage());

        }
        if(other.gameObject.tag == "Dummy")
        {
            other.gameObject.GetComponent<Dummy>().Hit(); 
        }
    }


    private void Interact()
    {
        RaycastHit hit;
        Physics.Raycast(mainCam.transform.position, mainCam.transform.TransformDirection(Vector3.forward), out hit, 5);


        switch (hit.collider.gameObject.tag)
        {
            case "Door":
                Door door = hit.collider.gameObject.GetComponent<Door>();
                StartCoroutine(door.Open());
                break;
            case "Gold":
                int amount = hit.collider.gameObject.GetComponent<Gold>().GetValue();
                gold += amount;
                Destroy(hit.collider.gameObject);
                uiManager.UpdateGold(gold);
                goldGathered += amount;
                break;
            case "Sword":
                if (sword != null)
                {
                    sword.transform.SetParent(null);
                    sword.GetComponent<Rigidbody>().isKinematic = false;
                    sword.transform.position += new Vector3(5, 0, 3);
                    sword.GetComponent<BoxCollider>().enabled = true;
                }
                sword = hit.collider.gameObject;
                sword.GetComponent<Rigidbody>().isKinematic = true;
                sword.transform.SetParent(swordPos.transform);
                sword.GetComponent<BoxCollider>().enabled = false;
                sword.transform.localPosition = new Vector3(0, 0, 0);
                sword.transform.localRotation = Quaternion.Euler(0, 0, 0);
                swordStats = sword.GetComponent<Sword>();
                uiManager.UpdateDurability(swordStats.GetDurability(), true);
                break;
            case "Portal":
                PortalManager portal = hit.collider.gameObject.GetComponent<PortalManager>();
                if (gold > -1000)
                {
                    int cost = portal.TogglePortal();
                    gold -= cost;
                    uiManager.UpdateGold(gold);
                }
                break;
            case "Swordshop":
                if(gold > 250)
                {
                    gold -= 250;
                    sword = Instantiate(swordPrefab, new Vector3(1, 1), Quaternion.identity);
                    sword.GetComponent<Rigidbody>().isKinematic = true;
                    sword.transform.SetParent(swordPos.transform);
                    sword.transform.localPosition = new Vector3(0, 0, 0);
                    sword.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    sword.GetComponent<BoxCollider>().enabled = false;
                    swordStats = sword.GetComponent<Sword>();
                    uiManager.UpdateGold(gold);
                }
                break;
            case "Torchshop":
                if(gold > 150)
                {
                    gold -= 150;
                    GiveTorch();

                }
                break;

            case "Healthpotion":
                health += 10;
                Destroy(hit.collider.gameObject);
                break;

            default: break;
        }
        rb.velocity = new Vector3(0,0,0);
        
    }

    public bool Damage(int damage)
    {
        if (!parry && !block)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            uiManager.UpdateHealth(health);
            return false;

        }
        else if (block)
        {
            health -= damage / 2;
            swordStats.SetDurability(2, false, false);
            uiManager.UpdateDurability(swordStats.GetDurability(), true);
            uiManager.UpdateHealth(health);
            if (health <= 0)
            {
                uiManager.StartTransition(false);
                Die();
            }
            return false;
        }
        else if (parry)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetValues()
    {
        Debug.Log("Setting values");
        PlayerPrefs.SetInt("GoldCount", gold);
        if (torch != null)
        {
            PlayerPrefs.SetInt("Torch", 1);
        }
        else if (torch == null)
        {
            PlayerPrefs.SetInt("Torch", 0);
        }


        if (sword != null)
        {
            PlayerPrefs.SetInt("SwordDur", swordStats.GetDurability());
        }
        else
        {
            PlayerPrefs.DeleteKey("SwordDur");
        }
    }
    public void GetValues()
    {


        if (PlayerPrefs.HasKey("GoldCount"))
        {
            gold = PlayerPrefs.GetInt("GoldCount");
            uiManager.UpdateGold(gold);
        }
        else
        {
            gold = 750;
            uiManager.UpdateGold(gold);
        }
        if (PlayerPrefs.HasKey("SwordDur"))
        {
            
            sword = Instantiate(swordPrefab, new Vector3(1, 1), Quaternion.identity);
            sword.GetComponent<Rigidbody>().isKinematic = true;
            sword.transform.SetParent(swordPos.transform);
            sword.transform.localPosition = new Vector3(0, 0, 0);
            sword.transform.localRotation = Quaternion.Euler(0, 0, 0);
            sword.GetComponent<BoxCollider>().enabled = false;
            swordStats = sword.GetComponent<Sword>();
            swordStats.SetDurability(PlayerPrefs.GetInt("SwordDur"), false, true);
        }
        if (PlayerPrefs.HasKey("Torch"))
        {


            int torched = PlayerPrefs.GetInt("Torch");
            Debug.Log(PlayerPrefs.GetInt("Torch"));
            if (torched == 1)
            {


                Debug.Log("A torch");
                GiveTorch();
            }
        }
    }

    public void Die()
    {
        StartCoroutine(Death());
    }
    private IEnumerator Death()
    {
        anim.SetBool("dying", true);
        uiManager.StartTransition(false);
        if (gold < -500)
        {
            gold -= 500 + goldGathered;
        }
        else
        {
            gold = -500;
        }
        sword = null;
        swordStats = null;
        SetValues();
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("Hub");
    }
    public GameObject Getsword()
    {
        return sword;
    }
    public void GiveSword()
    {

        sword = Instantiate(swordPrefab, new Vector3(1, 1), Quaternion.identity);
        sword.GetComponent<Rigidbody>().isKinematic = true;
        sword.transform.SetParent(swordPos.transform);
        sword.transform.localPosition = new Vector3(0, 0, 0);
        sword.transform.localRotation = Quaternion.Euler(0, 0, 0);
        sword.GetComponent<BoxCollider>().enabled = false;
        swordStats = sword.GetComponent<Sword>();


    } 
   
    public void GiveTorch()
    {

        GameObject torchitem = Instantiate(torchPrefab, new Vector3(1, 1), Quaternion.identity);
        torchitem.transform.SetParent(torchPos.transform);
        torchitem.transform.localPosition = new Vector3(0, 0, 0);
        torchitem.transform.localRotation = Quaternion.Euler(0, 0, 0);
        torch = torchitem.GetComponent<Torch>();
        hasTorch = true;
    } 
}
