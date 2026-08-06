using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public enum BtnClass
{
    head,
    torso,
    legs,
    accesory,
    ability,
    weapon
}

public class UIMenu_Customization : UIMenu
{
    public Button btnPlayerPrefab;
    public GameObject btnPlayerParent;
    public Image preview;
    public Image previewWeapon;

    public BtnClass currentBtnClass = BtnClass.head;

    public CustomList<BtnPlayerCustom> currentButtons = new CustomList<BtnPlayerCustom>();

    public string headString = "";
    public string torsoString = "";
    public string legsString = "";
    public string accesoryString = "";
    public string abilityString = "";
    public string weaponString = "";

    protected override void OnOpen()
    {
        base.OnOpen();

        headString = "";
        torsoString = "";
        legsString = "";
        accesoryString = "";
        abilityString = "";
        weaponString = "";

        preview.enabled = false;
        previewWeapon.enabled = false;

        BtnHead();
    }

    public void BtnHead()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        DestroyButtons();
        currentBtnClass = BtnClass.head;
        List<SpritePart> headParts = GameManager.Instance.spriteParts.Where(x => x.spType == SpritePartType.Head && x.spCategory == PawnType.Player).ToList();

        foreach (SpritePart sp in headParts)
        {
            BtnPlayerCustom btnPC = Instantiate(btnPlayerPrefab.gameObject, btnPlayerParent.transform).GetComponent<BtnPlayerCustom>();
            btnPC.partName = sp.partName;
            btnPC.partSprite.sprite = SetSprites(sp);
            btnPC.menu = this;
            currentButtons.Add(btnPC);
        }
    }


    public void BtnTorso()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        DestroyButtons();
        currentBtnClass = BtnClass.torso;
        List<SpritePart> torsoParts = GameManager.Instance.spriteParts.Where(x => x.spType == SpritePartType.Torso && x.spCategory == PawnType.Player).ToList();

        foreach (SpritePart sp in torsoParts)
        {
            BtnPlayerCustom btnPC = Instantiate(btnPlayerPrefab.gameObject, btnPlayerParent.transform).GetComponent<BtnPlayerCustom>();
            btnPC.partName = sp.partName;
            btnPC.partSprite.sprite = SetSprites(sp);
            btnPC.menu = this;
            currentButtons.Add(btnPC);
        }
    }

    public void BtnLegs()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        DestroyButtons();
        currentBtnClass = BtnClass.legs;
        List<SpritePart> legsParts = GameManager.Instance.spriteParts.Where(x => x.spType == SpritePartType.Legs && x.spCategory == PawnType.Player).ToList();

        foreach (SpritePart sp in legsParts)
        {
            BtnPlayerCustom btnPC = Instantiate(btnPlayerPrefab.gameObject, btnPlayerParent.transform).GetComponent<BtnPlayerCustom>();
            btnPC.partName = sp.partName;
            btnPC.partSprite.sprite = SetSprites(sp);
            btnPC.menu = this;
            currentButtons.Add(btnPC);
        }
    }

    public void BtnAccesory()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        DestroyButtons();
        currentBtnClass = BtnClass.accesory;
        List<SpritePart> accesoryParts = GameManager.Instance.spriteParts.Where(x => x.spType == SpritePartType.Details && x.spCategory == PawnType.Player).ToList();

        foreach (SpritePart sp in accesoryParts)
        {
            BtnPlayerCustom btnPC = Instantiate(btnPlayerPrefab.gameObject, btnPlayerParent.transform).GetComponent<BtnPlayerCustom>();
            btnPC.partName = sp.partName;
            btnPC.partSprite.sprite = SetSprites(sp);
            btnPC.menu = this;
            currentButtons.Add(btnPC);
        }
    }

    public void BtnAbility()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        DestroyButtons();
        currentBtnClass = BtnClass.ability;
        List<StatBlock> abilityBlock = GameManager.Instance.abilities.Where(x => !string.IsNullOrEmpty(x.abilityName)).ToList();

        foreach (StatBlock sp in abilityBlock)
        {
            BtnPlayerCustom btnPC = Instantiate(btnPlayerPrefab.gameObject, btnPlayerParent.transform).GetComponent<BtnPlayerCustom>();
            btnPC.partName = sp.name;
            btnPC.partSprite.sprite = sp.abilitySprite;
            btnPC.menu = this;
            currentButtons.Add(btnPC);
        }
    }

    public void BtnWeapon()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        DestroyButtons();
        currentBtnClass = BtnClass.weapon;
        List<StatBlock> weaponBlock = GameManager.Instance.weapons.Where(x => x.wepType != WeaponType.Enemy).ToList();

        foreach (StatBlock sp in weaponBlock)
        {
            BtnPlayerCustom btnPC = Instantiate(btnPlayerPrefab.gameObject, btnPlayerParent.transform).GetComponent<BtnPlayerCustom>();
            btnPC.partName = sp.name;
            btnPC.partSprite.sprite = sp.wepSprite;
            btnPC.menu = this;
            currentButtons.Add(btnPC);
        }
    }

    protected void DestroyButtons()
    {
        foreach (BtnPlayerCustom btn in currentButtons)
        {
            currentButtons.Remove(btn);
            Destroy(btn.gameObject);
        }

        currentButtons.Clear();
    }

    public void BtnApply()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        Pawn playerPawn = GameManager.Instance.playerPawn;

        if (!string.IsNullOrEmpty(headString)) playerPawn.statBlock.sprNames[0] = headString;
        if (!string.IsNullOrEmpty(torsoString)) playerPawn.statBlock.sprNames[1] = torsoString;
        if (!string.IsNullOrEmpty(legsString)) playerPawn.statBlock.sprNames[2] = legsString;
        if (!string.IsNullOrEmpty(accesoryString)) playerPawn.statBlock.sprNames[3] = accesoryString;

        if (!string.IsNullOrEmpty(headString) ||
            !string.IsNullOrEmpty(torsoString) ||
            !string.IsNullOrEmpty(legsString) ||
            !string.IsNullOrEmpty(accesoryString))
        {
            playerPawn.SetSprites();
        }

        if (!string.IsNullOrEmpty(abilityString))
        {
            playerPawn.statBlock.abilityBlock = abilityString;
            playerPawn.SetAbility(abilityString, true);
        }
        if (!string.IsNullOrEmpty(weaponString))
        {
            playerPawn.statBlock.weaponBlock = weaponString;
            playerPawn.SetWeapon(weaponString, true);
        }

        GameManager.Instance.eventSystem.OpenCloseMenues();
    }

    public Sprite SetSprites()
    {
        Sprite[] sprFrame1 = new Sprite[8];
        Color[] sprFrameColors = new Color[8];

        SpritePart h = GameManager.Instance.GetSpritePart(headString);
        if (h != null)
        {
            sprFrame1[0] = h.frame1;
            sprFrame1[1] = h.frame1Det;
            sprFrameColors[0] = h.defaultColor;
            sprFrameColors[1] = h.detDefaultColor;
        }

        SpritePart t = GameManager.Instance.GetSpritePart(torsoString);
        if (t != null)
        {
            sprFrame1[2] = t.frame1;
            sprFrame1[3] = t.frame1Det;
            sprFrameColors[2] = t.defaultColor;
            sprFrameColors[3] = t.detDefaultColor;
        }

        SpritePart l = GameManager.Instance.GetSpritePart(legsString);
        if (l != null)
        {
            sprFrame1[4] = l.frame1;
            sprFrame1[5] = l.frame1Det;
            sprFrameColors[4] = l.defaultColor;
            sprFrameColors[5] = l.detDefaultColor;
        }

        SpritePart a = GameManager.Instance.GetSpritePart(accesoryString);
        if (a != null)
        {
            sprFrame1[6] = a.frame1;
            sprFrame1[7] = a.frame1Det;
            sprFrameColors[6] = a.defaultColor;
            sprFrameColors[7] = a.detDefaultColor;
        }

        return SetSprite(sprFrame1, sprFrameColors);
    }

    public Sprite SetSprites(SpritePart sp)
    {
        if (sp == null) return null;

        Sprite[] sprFrame1 = new Sprite[2];
        Color[] sprFrameColors = new Color[2];

        sprFrame1[0] = sp.frame1;
        sprFrame1[1] = sp.frame1Det;

        sprFrameColors[0] = sp.defaultColor;
        sprFrameColors[1] = sp.detDefaultColor;

        return SetSprite(sprFrame1, sprFrameColors);
    }

    public Sprite SetSprite(Sprite[] sprFrame, Color[] sprFrameColor, int pixelRes = 32)
    {
        List<Texture2D> allSprites = new List<Texture2D>();

        for (int i = 0; i < sprFrame.Length; i++)
        {
            if (sprFrame[i] != null)
            {
                if (sprFrameColor[i] != null) allSprites.Add(CreateTexture2DFromSprite(sprFrame[i], sprFrameColor[i]));
                else allSprites.Add(CreateTexture2DFromSprite(sprFrame[i], Color.white));
            }
        }

        if (allSprites.Count == 0) return null;

        Texture2D merged = new Texture2D(pixelRes, pixelRes, TextureFormat.RGBA32, false);
        merged.filterMode = FilterMode.Point;

        Color[] clear = new Color[pixelRes * pixelRes];

        for (int i = 0; i < clear.Length; i++)
        {
            clear[i] = Color.clear;
        }

        merged.SetPixels(clear);

        foreach (Texture2D spr in allSprites)
        {
            Color[] pixels = spr.GetPixels(0);

            for (int y = 0; y < pixelRes; y++)
            {
                for (int x = 0; x < pixelRes; x++)
                {
                    int index = (y * pixelRes) + x;
                    if (pixels[index].a > 0) merged.SetPixel(x, y, pixels[index]);
                }
            }
        }

        merged.Apply();

        Sprite mergedSprite = Sprite.Create(merged, new Rect(0, 0, pixelRes, pixelRes), new Vector2(0.5f, 0.5f), pixelRes);


        return mergedSprite;
    }

    private Texture2D CreateTexture2DFromSprite(Sprite toCopy, Color colorToApply)
    {
        if (toCopy.textureRect.width == 0 || toCopy.textureRect.height == 0) return null;

        Texture2D result = new Texture2D((int)toCopy.textureRect.width, (int)toCopy.textureRect.height, TextureFormat.RGBA32, false);
        result.filterMode = FilterMode.Point;

        Color[] pixels = toCopy.texture.GetPixels((int)toCopy.textureRect.x, (int)toCopy.textureRect.y, (int)toCopy.textureRect.width, (int)toCopy.textureRect.height, 0);

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].r *= colorToApply.r;
            pixels[i].g *= colorToApply.g;
            pixels[i].b *= colorToApply.b;
        }

        result.SetPixels(pixels);
        result.Apply();

        return result;
    }

    public void SetProjBasic()
    {
        GameManager.Instance.playerPawn.SetProjectiles(ProjectileType.P_Basic);
    }

    public void SetProjBounce()
    {
        GameManager.Instance.playerPawn.SetProjectiles(ProjectileType.P_Bounce);
    }

    public void SetProjSplit()
    {
        GameManager.Instance.playerPawn.SetProjectiles(ProjectileType.P_Split);
    }
}
