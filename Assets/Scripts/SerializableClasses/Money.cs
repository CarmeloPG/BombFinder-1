using UnityEngine;
using System;


/// <summary>
/// A serializable class witch shows the currentQuantity of some MoneyType.
/// </summary>
[Serializable]
public class Money
{
	#region Serialize fields
	[SerializeField]
	private MoneyDefinition definition;
	[SerializeField]
	private int amount;
	#endregion

	#region Private fields
    #endregion

    #region Properties
    public MoneyType MoneyType {
		get => definition.MoneyType;
	}

	public int Amount {
		get => amount;

		set => amount = value;
	}

	public Sprite Icon {
		get => definition.Icon;
	}

	public AudioClip Sound {
		get => definition.Sound;
	}
    #endregion

    #region Constructors
    public Money(MoneyDefinition moneyDefinition, int moneyQuantity = 0) {
		amount = moneyQuantity;
		definition = moneyDefinition;
	}
	#endregion

	#region Public methods
	#endregion

	#region Private methods
    #endregion

}
