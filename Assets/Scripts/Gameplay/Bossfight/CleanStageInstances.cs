using UnityEngine;

/// <summary>
/// Delets any child gameobjects with an <see cref="EnemyController"/> or <see cref="BouncePad"/>
/// </summary>
public class CleanStageInstances : MonoBehaviour
{
    public void ClearCommon()
    {
        foreach (Transform child in transform)
        {
            var go = child.gameObject;

            if (child.GetComponent<EnemyController>() ||
                child.GetComponent<BouncePad>())
            {
                Destroy(child.gameObject);

                go = null;
            }
        }
    }
    public void ClearAll()
    {
        foreach (Transform child in transform)
        {
            var go = child.gameObject;

            Destroy(child.gameObject);

            go = null;
        }
        var bouncepads = FindObjectsOfType<BouncePad>();
        foreach (BouncePad bouncepad in bouncepads)
        {
            Destroy(bouncepad.gameObject);
        }
    }
}
