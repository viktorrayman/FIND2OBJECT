using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    bool isDrag;
    Camera mainCamera;
    public int index;
    public Color color;
    public ParticleSystem particleSystem;
    public Color particlesColor;
    Animator animator;
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Sprite sprite;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDrag)
        {
            Vector3 worldposition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldposition.z = 0;
            transform.position = worldposition;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckMatch(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckMatch(collision);
    }
    void CheckMatch(Collider2D collision)
    {
        
        if (isDrag)
        {
            /*if (gameObject.GetComponent<SpriteRenderer>().sprite == collision.gameObject.GetComponent<SpriteRenderer>().sprite)*/
            if (index == collision.gameObject.GetComponent<Item>().index)
            {
                FindObjectOfType<LevelGenerator>().DeleteItem(this);
                FindObjectOfType<LevelGenerator>().DeleteItem(collision.GetComponent<Item>());

                Destroy(gameObject);
                Destroy(collision.gameObject);

                Vector3 centrePosition = Vector3.Lerp(transform.position, collision.transform.position, 0.5f);
                FindObjectOfType<LevelGenerator>().SpawnNextItem(index, centrePosition);
                ParticleSystem ps = Instantiate(particleSystem, centrePosition, Quaternion.identity);
                ps.startColor = particlesColor;
                Destroy(ps.gameObject, 1);
                FindObjectOfType<TimeManager>()?.AddAdditiveTime();

            }


        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetTrigger("Unselect");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetTrigger("Select");
    }

    /*public void DestroyItem()
    {
        Destroy(gameObject);
    }*/
}
