using UnityEngine;

public class CherryBombBehaviour : MonoBehaviour
{
    #region Members
    public float force;
    public float bombRange;
    [HideInInspector]
    public bool isRight;
    #endregion

    private void Start()
    {
        Vector2 forceVec = new Vector2(force, 0);
        if (isRight)
        {
            GetComponent<Rigidbody2D>().AddForce(forceVec, ForceMode2D.Impulse);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(forceVec * -1, ForceMode2D.Impulse);
        }
    }
}