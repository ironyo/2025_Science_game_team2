using Unity.VisualScripting;
using UnityEngine;

public abstract class item : MonoBehaviour
{
    [SerializeField]private float currentTime = 0f;
    protected virtual void Update()
    {
        transform.Translate(Vector2.down * (3f * Time.deltaTime));
        currentTime += Time.deltaTime;
        if (currentTime >= 4.5f)
        {
            gameObject.SetActive(false);
            currentTime = 0f;
        }
    }

    public abstract void GetItem();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetItem();
            gameObject.SetActive(false);
        }
    }
}
