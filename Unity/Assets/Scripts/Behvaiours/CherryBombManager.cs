using UnityEngine;

public class CherryBombManager : MonoBehaviour
{
    #region Members
    [HideInInspector]
    public static bool cherryBombActivated = true;

    private CherryBombBehaviour cherryBomb;
    #endregion

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(10, 9);
        cherryBomb = Resources.Load<CherryBombBehaviour>($"Prefabs/Objects/CherryBomb");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cherryBombActivated)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 forceVec = new Vector2(10, 0);
            if (mousePos.x > transform.position.x)
            {
                cherryBomb.isRight = true;
                Instantiate(cherryBomb, transform.position, Quaternion.identity);
            }
            else
            {
                cherryBomb.isRight = false;
                Instantiate(cherryBomb, transform.position, Quaternion.identity);
            }
        }
    }

    public static void SetCherryBombs(bool activated)
    {
        cherryBombActivated = activated;
    }
}