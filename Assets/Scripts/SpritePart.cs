using UnityEngine;

[CreateAssetMenu(fileName = "SpritePart")]
public class SpritePart : ScriptableObject
{
    public PawnType spCategory;
    public SpritePartType spType;

    public int levelRequirement = 0;

    public string partName = "";

    public Sprite frame1;
    public Sprite frame2;

    public Color defaultColor;

    public Sprite frame1Det;
    public Sprite frame2Det;

    public Color detDefaultColor;
}
