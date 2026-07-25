using UnityEngine;

public enum SpritePartType
{
    Head,
    Torso,
    Legs,
    Details
}

public enum SpritePartCategory
{
    Player,
    Slime,
    Zombie,
    Bug,
    StaticPlant,
    Rat,
    Wolf,
    FleshGolem
}

[CreateAssetMenu(fileName = "SpritePart")]
public class SpritePart : ScriptableObject
{
    public SpritePartCategory spCategory;
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
