using UnityEngine;

public class controlChipsObjects : MonoBehaviour
{
    public coreSection section;
    public Vector3 EndPosition;
    public float moveSpeed = 5;
    public float force = 0.5f;

    public int chipCost;
   
    public int objectHeight;
    public bool isMove;
    public bool isDestroy;
    public bool isStacked;
    void Start()
    {
        if (!section.chips.Contains(this.gameObject))
            section.chips.Add(this.gameObject);

        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(transform.forward * force * 0.75f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        moveObject();
        if (isDestroy)
            Destroy(this.gameObject, 0.1f);

        if (isStacked && section.chips.Contains(this.gameObject))
        {
            section.chips.Remove(this.gameObject);
            isStacked = false;
        }
    }
    public void moveObject()
    {
        if (isMove)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, EndPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 20 * Time.deltaTime);

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().isTrigger = true;

        }
        if ( isMove && Vector3.Distance(transform.localPosition , EndPosition) < 0.3f)
        {
            transform.localPosition = EndPosition;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            GetComponent<Collider>().isTrigger = false;
            isMove = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            /*GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;*/
            /*transform.rotation = Quaternion.Euler(0, 0, 0);*/
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vault"))
        {
            FindObjectOfType<GameManager>().maxCash += chipCost;
            Destroy(this.gameObject);
        }
    }
}
