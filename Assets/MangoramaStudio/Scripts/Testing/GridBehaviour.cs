using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public int Column { get; set; }
    public int Row { get; set; }
    public bool IsOccupied { get; set; }
    public BlockBehaviour CurrentBlockBehaviour { get; set; }

    public void ChangeMyColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

}
