using UnityEngine;

public enum SpritePartType
{
    Head,
    HeadDetail,
    Torso,
    TorsoDetail,
    Legs,
    LegsDetail,
    Details
}

public enum SpritePartCategory
{
    Player,
    Slime,
    Zombie,
    Bug,
    StaticPlant,
    MovingPlant,
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
}
