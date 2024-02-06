using Godot;
using System;
using System.Collections.Generic;

public partial class ShopCanvas : CanvasLayer
{
	[Export] public Label picklesCounter;
	[Export] public ScrollContainer scrollContainer;
	[Export] public MarginContainer pickleBuyablesContainer;
	[Export] public VScrollBar scrollBar;

	[Export] public Label picklesCost;
	[Export] public Label picklesAmount;
	[Export] public Label picklesUpgradeCost;
	[Export] public Label pickleLevel;

	[Export] public ButtonGroup buyButtons;

	private Pickle pickle;
	private int currentBuyable = 0;
	private int currentCost;
	private int currentAmount;
	private int currentUpgradeCost;
	private int currentLevel;
	
	private const int MIN_SCROLL_BAR_SIZE = 200;
	private const int MAX_SCROLL_BAR_SIZE = 380;
	private int scrollBarRange;
	private bool isHovering;
	private VBoxContainer arrows;

	private const float MULTIPLIER = 1.025f;
	private const int LINEAR = 1;
	
	public override void _Ready()
	{
		pickle = GetNode<Pickle>("/root/MainCanvas/Pickle");
		scrollBarRange = (int) (MAX_SCROLL_BAR_SIZE - scrollBar.CustomMinimumSize.Y);
		arrows = scrollBar.GetNode<VBoxContainer>("../Arrows");
		arrows.GetNode<TextureRect>("./UpArrow").Visible = false;
		UpdateValues();
	}

	public override void _Process(double delta)
	{
		if (!isHovering) return;

		ScrollAnimation();
	}

	public void BarScrolling(float value)
	{
		ScrollAnimation();
	}

	public void ScrollAnimation()
	{
		AutoBuyables pickleBuyables = pickleBuyablesContainer.GetNode<AutoBuyables>("./AutoBuyables");
		int pickleBuyableCount = pickleBuyables.GetChildCount() - 1;
		float scrollBarSections = scrollBarRange/pickleBuyableCount;

		if (Input.IsActionJustPressed("scrollDown"))
		{
			if (scrollBar.CustomMinimumSize.Y >= MAX_SCROLL_BAR_SIZE) return;

			scrollBar.CustomMinimumSize += new Vector2(0, scrollBarSections);

			arrows.GetNode<TextureRect>("./DownArrow").Visible = false;

			if (currentBuyable >= pickleBuyableCount) return;

			currentBuyable++;
			UpdateValues();
			scrollContainer.ScrollVertical = currentBuyable * 165;

			if (currentBuyable < pickleBuyableCount)
			{
				ArrowVisibility(true, true);
			}

			if (currentBuyable <= 0)
			{
				ArrowVisibility(false, true);
			}
		}

		if (Input.IsActionJustPressed("scrollUp"))
		{
			if (scrollBar.CustomMinimumSize.Y <= MIN_SCROLL_BAR_SIZE) return;

			scrollBar.CustomMinimumSize -= new Vector2(0, scrollBarSections);

			arrows.GetNode<TextureRect>("./UpArrow").Visible = false;

			if (currentBuyable <= 0) return;

			currentBuyable--;
			UpdateValues();
			scrollContainer.ScrollVertical = currentBuyable * 165;

			if (currentBuyable > 0)
			{
				ArrowVisibility(true, true);
			}

			if (currentBuyable >= pickleBuyableCount)
			{
				ArrowVisibility(true, false);
			}
		}
	}

	public void UpdateValues()
	{
		currentCost = Auto.autos[currentBuyable].cost; 
		currentAmount = Auto.autos[currentBuyable].amount;
		currentUpgradeCost = Auto.autos[currentBuyable].upgradeCost;
		currentLevel = Auto.autos[currentBuyable].level;
		UpdateLabels();
	}

	public void UpdateLabels()
	{
		picklesCounter.Text = $"{pickle.pickles:N0} Pickles";
		picklesCost.Text = $"{currentCost:N0} Pickles";
		picklesAmount.Text = $"Bought {currentAmount:N0}";
		picklesUpgradeCost.Text = $"{currentUpgradeCost:N0} Pickles";
		pickleLevel.Text = $"Level {currentLevel:N0}";
	}

	public void UpdateButton(BaseButton pressedButton)
	{
		pressedButton.ButtonPressed = false;
	}

	public void ArrowVisibility(bool upArrow, bool downArrow)
	{
		arrows.GetNode<TextureRect>("./UpArrow").Visible = upArrow; 
		arrows.GetNode<TextureRect>("./DownArrow").Visible = downArrow;
	}

	public void Buy()
	{
		GD.Print("Buying");
		SetNewCost();
		UpdateValues();

		if (pickle.pickles < currentCost) return;

		pickle.pickles -= currentCost;
		currentAmount++;

		UpdateValues();
	}

	public void SetNewCost()
	{
		int buyAmount = GetPressedButton();
		int total = 0;
		int cost = currentCost;
		for (int index = 0; buyAmount > index; index++)
		{
			total += cost;
			cost = (int) Math.Floor((cost + LINEAR) * MULTIPLIER);
		}
		currentCost = total;
	}

	public BaseButton GetSpecificButton(int value)
	{
		return buyButtons.GetButtons()[value];
	}

	public int GetPressedButton()
	{
		BaseButton pressedButton = buyButtons.GetPressedButton();

		BaseButton buyOne = GetSpecificButton(0);
		BaseButton buyFive = GetSpecificButton(1);
		BaseButton buyTwentyFive = GetSpecificButton(2);
		BaseButton buyMax = GetSpecificButton(3);

		UpdateButton(pressedButton);

		if (pressedButton == buyOne) return 1;
		if (pressedButton == buyFive) return 5;
		if (pressedButton == buyTwentyFive) return 25;
		if (pressedButton == buyMax) return Auto.MAX_AMOUNT - Auto.autos[currentBuyable].amount;


		return 1;
	}
	
	public void Upgrade()
	{
		if (pickle.pickles < currentUpgradeCost) return;
	}

	public void OnHover(bool value)
	{
		isHovering = value;
	}
  
	public void CloseShop()
	{
		QueueFree();
	}
}
