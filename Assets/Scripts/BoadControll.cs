using UnityEngine;
public class BoadControll : MonoBehaviour
{
    [SerializeField] GameObject border;

    private void Start()
    {
        Vector2 size = CandyCreator.Instance.matrixSize;
        transform.localScale = size + new Vector2(0.3f, 0.3f);
        border.transform.localScale = transform.localScale + new Vector3(0.3f, 0.3f);
        transform.localPosition = new Vector2(size.x/2 - .5f, size.y/2 - .5f);
        border.transform.position = transform.position;
    }
}
