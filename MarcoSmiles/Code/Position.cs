[System.Serializable]

/// <summary>
/// Incapsula una configurazione della mano sinistra e destra.
/// </summary>
public class Position
{
    /// <summary>
    /// Informazioni sulla mano sinistra
    /// </summary>
    public DataToStore Left_Hand { get; set; }
    /// <summary>
    /// Informazioni sulla mano destra
    /// </summary>
    public DataToStore Right_Hand { get; set; }

    /// <summary>
    /// Informazioni sulla mano sinistra SECONDO DISPOSITIVO
    /// </summary>
    public DataToStore Left_Hand2 { get; set; }
    /// <summary>
    /// Informazioni sulla mano destra SECONDO DISPOSITIVO
    /// </summary>
    public DataToStore Right_Hand2 { get; set; }

    /// <summary>
    /// Informazioni sull'id della posizione
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Costruttore
    /// </summary>
    /// <param name="left_hand"><paramref name="left_hand"/></param>
    /// <param name="right_hand"><paramref name="right_hand"/></param>
    /// /// <param name="left_hand2"><paramref name="left_hand2"/></param>
    /// <param name="right_hand2"><paramref name="right_hand2"/></param>
    /// <param name="id"><paramref name="id"/></param>
    public Position(DataToStore left_hand, DataToStore right_hand, DataToStore left_hand2, DataToStore right_hand2, int id)
    {
        this.Left_Hand = left_hand;
        this.Right_Hand = right_hand;
        this.Left_Hand2 = left_hand2;
        this.Right_Hand2 = right_hand2;
        this.ID = id;
    }
}
