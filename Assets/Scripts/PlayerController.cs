using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {

    public float rotationSpeed = 450;
    public float walkSpeed = 5;
    public float runSpeed = 8;
    private float acceleration = 5;

    private Quaternion targetRotation;
    private Vector3 curretnvelocityMod;

    public Transform handHold;
    public Gun[] guns;
    private Gun currentGun;
    private CharacterController controller;
    private Camera cam;
    private GameGUI gui;

    public static PlayerController Instance { set; get; }

    // Use this for initialization
    void Start () {
        Instance = this;
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        gui = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameGUI>();

        EquipGun(0);
	}
	
	// Update is called once per frame
	void Update () {
        ControlMouse();
        //ControlWASD();
        if (currentGun)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                currentGun.Shoot();
            }
            else if (Input.GetButton("Shoot"))
            {
                currentGun.ShootAuto();
            }
        }

        for (int i = 0; i < guns.Length; i++)
        {
            if (Input.GetKeyDown((i + 1) + "") || Input.GetKeyDown("[" + (i+1) + "]"))
            {
                EquipGun(i);
                break;
            }
        }
	}

    void EquipGun(int i)
    {
        if (currentGun)
        {
            Destroy(currentGun.gameObject);
        }
        currentGun = Instantiate(guns[i], handHold.position, handHold.rotation) as Gun;
        currentGun.transform.parent = handHold;
        currentGun.gui = gui;
    }

    void ControlMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x,0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        curretnvelocityMod = Vector3.MoveTowards(curretnvelocityMod, input, acceleration * Time.deltaTime);
        Vector3 motion = curretnvelocityMod;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        //motion *= Vector3.down * 8;

        controller.Move(motion * Time.deltaTime);
    }

    void ControlWASD()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        curretnvelocityMod = Vector3.MoveTowards(curretnvelocityMod, input, acceleration * Time.deltaTime);
        Vector3 motion = curretnvelocityMod;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        //motion *= Vector3.down * 8;

        controller.Move(motion * Time.deltaTime);
    }
}
