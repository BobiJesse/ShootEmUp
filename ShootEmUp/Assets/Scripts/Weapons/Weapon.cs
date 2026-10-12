using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public enum weaponStyle
    {
        normal,
        fire,
        beam
    }

    [Header("Weapon Type")]
    public weaponStyle chosenWeaponStyle;

    [Header ("Audio")]
    public AudioClip shootAudioClip;

    [Header ("Bullet")]
    public Bullet bulletPrefab;

    [Header("Fire parameters")]
    public float fireSpeed = 0f;
    public int damage = 0;
    private int maxLevel = 3;
}
