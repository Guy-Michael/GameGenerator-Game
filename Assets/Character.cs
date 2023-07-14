using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Active,
    Idle,
    Lost
}

[System.Serializable]
public class States : Dictionary<CharacterState, Sprite> {}

public class Character : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite lost;

}
