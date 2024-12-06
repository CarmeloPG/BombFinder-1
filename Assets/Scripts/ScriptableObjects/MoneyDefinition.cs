using UnityEngine;

/// <summary>
/// A ScriptableObject witch defines data of some MoneyType.
/// </summary>
[CreateAssetMenu(fileName = "New MoneyDefinition", menuName = "ScriptableObjects/MoneyDefinition")]
public class MoneyDefinition : ScriptableObject
{
	#region Fields
	[SerializeField]
	private MoneyType moneyType;
	[SerializeField]
	private Sprite icon;
	[SerializeField]
	private AudioClip sound;
	#endregion

	#region Properties
	public MoneyType MoneyType {
		get => moneyType;
	}

	public Sprite Icon {
		get => icon;
	}

	public AudioClip Sound {
		get => sound;
	}
	#endregion
}
