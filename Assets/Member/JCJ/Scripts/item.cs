using Unity.VisualScripting;
using UnityEngine;

public abstract class item : MonoBehaviour
{
    [SerializeField]private float currentTime = 0f;
    protected virtual void Update()
    {
        transform.Translate(Vector2.left * (3f * Time.deltaTime));
        currentTime += Time.deltaTime;
        if (currentTime >= 6.3f)
        {
            gameObject.SetActive(false);
            currentTime = 0f;
        }
    }

    public abstract void GetItem(BulbController bulbController);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetItem(collision.gameObject.GetComponentInParent<BulbController>());
            gameObject.SetActive(false);
        }
    }
}
