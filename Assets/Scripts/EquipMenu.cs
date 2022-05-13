using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipMenu : MonoBehaviour
{
    public Image HeroImage;
    public Image WeaponImage;
    public List<Sprite> WeaponSprites;
    public TextMeshProUGUI HeroInfoText;
    public EquipmentSlot WeaponSlot;

    public void UpdateEquipMenu(Stats s)
    {
        WeaponImage.sprite = WeaponSprites[s.DamageType];
        WeaponSlot.DamageType = s.DamageType;
        HeroInfoText.text = s.HeroName + "\n Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + "/" + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop:x" + s.Discovery;
        HeroImage.sprite = s.HeroSprite;
    }

    public void EquipWeapon()
    {

    }
}
