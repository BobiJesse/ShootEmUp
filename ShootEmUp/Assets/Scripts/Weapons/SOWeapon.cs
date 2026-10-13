using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class SOWeapon : ScriptableObject
{
    public enum WeaponStyle
    {
        Straight,
        Sine,
        Track
    }

    [System.Serializable]
    public class WeaponLevelStats
    {
        public float damage = 1;
        public float fireSpeed = 1;
    }

    [Header("Weapon Type")]
    public WeaponStyle chosenWeaponStyle;

    [Header("Audio")]
    public AudioClip shootAudioClip;

    [Header("Bullet")]
    public Bullet bulletPrefab;

    [Header("Weapon Levels")]
    public WeaponLevelStats[] levels = new WeaponLevelStats[5];
}