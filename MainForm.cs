using System.ComponentModel;
using System.Globalization;
using System.Text;
using Office2007Rendering;

namespace BlackAndWhite
{
	/// <summary>Main form for the Black and White game application</summary>
	public partial class BlackAndWhiteForm : Form
	{
		#region Variables

		/// <summary>True if the game is started</summary>
		private bool isGameStarted;

		/// <summary>Sum of the clicks</summary>
		private int sumClicks;

		/// <summary>Sum of the ticks</summary>
		private int sumTicks;

		/// <summary>Number of the black fields</summary>
		private int numberBlacks;

		/// <summary>Number of the white fields</summary>
		private int numberWhites;

		/// <summary>Sum of the inverted fields</summary>
		private int sumInverts;

		/// <summary>Random number</summary>
		private readonly Random randomNumber = new();

		/// <summary>String builder</summary>
		private readonly StringBuilder stringBuilder = new();

		/// <summary>Lookup of all game field buttons by field ID</summary>
		private readonly Dictionary<ushort, Button> fieldButtonsById = [];

		/// <summary>Culture info</summary>
		private static readonly CultureInfo _culture = CultureInfo.CurrentUICulture;

		#endregion

		#region Helpers

		/// <summary>Apply resources to a control and its children</summary>
		/// <param name="res">Resource manager</param>
		/// <param name="control">Control to apply resources to</param>
		private static void ApplyResourceToControl(ComponentResourceManager res, Control control)
		{
			foreach (Control c in control.Controls)
			{
				ApplyResourceToControl(res: res, control: c);
			}
			string? text = res.GetString(name: $"{control.Name}.Text", culture: _culture);
			control.Text = text ?? control.Text;
		}

		/// <summary>Set a specific text to the status bar</summary>
		/// <param name="text">Text with some information</param>
		private void SetStatusBarText(string text)
		{
			toolStripStatusLabelInformation.Enabled = !string.IsNullOrEmpty(value: text);
			toolStripStatusLabelInformation.Text = text;
		}

		/// <summary>Set one menu item checked and all other provided menu items unchecked</summary>
		/// <param name="selected">Selected menu item</param>
		/// <param name="otherItems">Other menu items</param>
		private static void SetExclusiveChecked(ToolStripMenuItem selected, params ToolStripMenuItem[] otherItems)
		{
			selected.Checked = true;
			foreach (ToolStripMenuItem item in otherItems)
			{
				item.Checked = false;
			}
		}

		/// <summary>Get the currently selected game board and tab page</summary>
		/// <returns>Selected game board and tab page</returns>
		private (TableLayoutPanel GameBoard, TabPage GameTab) GetSelectedGameBoard()
		{
			if (toolStripMenuItemNewGame3x3.Checked)
			{
				return (GameBoard: tableLayoutPanelGame3x3, GameTab: tabPageGame3x3);
			}

			if (toolStripMenuItemNewGame4x4.Checked)
			{
				return (GameBoard: tableLayoutPanelGame4x4, GameTab: tabPageGame4x4);
			}

			return (GameBoard: tableLayoutPanelGame5x5, GameTab: tabPageGame5x5);
		}

		/// <summary>Get the currently active game board</summary>
		/// <returns>Active game board or null if no game size is selected</returns>
		private TableLayoutPanel? GetActiveGameBoard()
		{
			return toolStripMenuItemNewGame3x3.Checked ? tableLayoutPanelGame3x3 :
				toolStripMenuItemNewGame4x4.Checked ? tableLayoutPanelGame4x4 :
				toolStripMenuItemNewGame5x5.Checked ? tableLayoutPanelGame5x5 : null;
		}

		/// <summary>Get the game board size from its row count</summary>
		/// <param name="tableLayoutPanel">Game board</param>
		/// <returns>Size of one board side</returns>
		private static int GetGameBoardSize(TableLayoutPanel tableLayoutPanel) => tableLayoutPanel.RowCount;

		/// <summary>Try to parse a field ID from a string tag</summary>
		/// <param name="fieldTag">Field tag</param>
		/// <param name="fieldId">Parsed field ID</param>
		/// <returns>true if parsing succeeded, otherwise false</returns>
		private static bool TryParseFieldId(string fieldTag, out ushort fieldId) =>
			ushort.TryParse(s: fieldTag, style: NumberStyles.None, provider: CultureInfo.InvariantCulture, result: out fieldId);

		/// <summary>Try to decode a field ID into game board size, row and column</summary>
		/// <param name="fieldId">Field ID</param>
		/// <param name="boardSize">Board size</param>
		/// <param name="row">Field row</param>
		/// <param name="column">Field column</param>
		/// <returns>true if decoding succeeded, otherwise false</returns>
		private static bool TryDecodeFieldId(ushort fieldId, out int boardSize, out int row, out int column)
		{
			boardSize = fieldId / 100;
			row = fieldId / 10 % 10;
			column = fieldId % 10;
			return boardSize is >= 3 and <= 5 &&
				row >= 1 && row <= boardSize &&
				column >= 1 && column <= boardSize;
		}

		/// <summary>Encode board size, row and column to a field ID</summary>
		/// <param name="boardSize">Board size</param>
		/// <param name="row">Field row</param>
		/// <param name="column">Field column</param>
		/// <returns>Encoded field ID</returns>
		private static ushort EncodeFieldId(int boardSize, int row, int column) => (ushort)(boardSize * 100 + row * 10 + column);

		/// <summary>Get neighboring field IDs for a center field</summary>
		/// <param name="centerFieldId">Center field ID</param>
		/// <param name="diagonal">true for diagonal neighbors, false for linear neighbors</param>
		/// <returns>Neighboring field IDs</returns>
		private static ushort[] GetNeighbourFieldIds(ushort centerFieldId, bool diagonal)
		{
			if (!TryDecodeFieldId(fieldId: centerFieldId, boardSize: out int boardSize, row: out int row, column: out int column))
			{
				return [];
			}

			List<ushort> neighbours = [];
			for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
			{
				for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
				{
					if (rowOffset == 0 && columnOffset == 0)
					{
						continue;
					}

					bool isDiagonalOffset = Math.Abs(value: rowOffset) == 1 && Math.Abs(value: columnOffset) == 1;
					if (diagonal != isDiagonalOffset)
					{
						continue;
					}

					int nextRow = row + rowOffset;
					int nextColumn = column + columnOffset;
					if (nextRow < 1 || nextRow > boardSize || nextColumn < 1 || nextColumn > boardSize)
					{
						continue;
					}

					neighbours.Add(item: EncodeFieldId(boardSize: boardSize, row: nextRow, column: nextColumn));
				}
			}

			return [.. neighbours];
		}

		/// <summary>Build the game field button lookup from all game boards</summary>
		private void BuildFieldButtonMap()
		{
			fieldButtonsById.Clear();
			TableLayoutPanel[] gameBoards = [tableLayoutPanelGame3x3, tableLayoutPanelGame4x4, tableLayoutPanelGame5x5];
			foreach (TableLayoutPanel gameBoard in gameBoards)
			{
				foreach (Control control in gameBoard.Controls)
				{
					if (control is Button { Tag: string fieldTag } button &&
						TryParseFieldId(fieldTag: fieldTag, fieldId: out ushort fieldId))
					{
						fieldButtonsById[fieldId] = button;
					}
				}
			}
		}

		/// <summary>Return a randomized background color of the field</summary>
		/// <returns>Randomized background color</returns>
		private Color RandomFieldColor()
		{
			return randomNumber.Next(minValue: 0, maxValue: 2) switch
			{
				0 => Color.Black,
				1 => Color.White,
				_ => SystemColors.Control,
			};
		}

		/// <summary>Invert the background colors of all fields</summary>
		/// <param name="fieldId">Array of field IDs to invert</param>
		private void InvertFields(ushort[] fieldId)
		{
			foreach (ushort i in fieldId)
			{
				if (fieldButtonsById.TryGetValue(key: i, value: out Button? button))
				{
					button.BackColor = InvertFieldColor(color: button.BackColor);
				}
			}
		}

		/// <summary>Invert the background color of a field</summary>
		/// <param name="color">Current background color</param>
		/// <returns>Inverted background color</returns>
		private Color InvertFieldColor(Color color)
		{
			sumInverts++;
			return color == Color.Black ? Color.White : Color.Black;
		}

		/// <summary>Init the game board</summary>
		private void InitGameBoard()
		{
			(TableLayoutPanel gameBoard, TabPage gameTab) = GetSelectedGameBoard();
			InitGameBoard(tableLayoutPanel: gameBoard, tabPage: gameTab);
		}

		/// <summary>Init the game board with the specified size</summary>
		/// <param name="tableLayoutPanel">The TableLayoutPanel to initialize</param>
		/// <param name="tabPage">The TabPage to set as parent</param>
		private void InitGameBoard(TableLayoutPanel tableLayoutPanel, TabPage tabPage)
		{
			timer.Enabled = false;
			toolStripButtonPause.Enabled = false;
			toolStripLabelStatistic.Text = Localization.statisticDefault;
			sumTicks = 0;
			sumClicks = 0;
			sumInverts = 0;
			tabPageGame3x3.Parent = tabPage == tabPageGame3x3 ? tabControl : null;
			tabPageGame4x4.Parent = tabPage == tabPageGame4x4 ? tabControl : null;
			tabPageGame5x5.Parent = tabPage == tabPageGame5x5 ? tabControl : null;
			const int maxRandomizeAttempts = 100;
			int randomizeAttempts = 0;
			do
			{
				foreach (Control button in tableLayoutPanel.Controls)
				{
					button.BackColor = RandomFieldColor();
				}
				randomizeAttempts++;
			}
			while (IsSameColorsInGameBoard(tableLayoutPanel: tableLayoutPanel) && randomizeAttempts < maxRandomizeAttempts);
			if (IsSameColorsInGameBoard(tableLayoutPanel: tableLayoutPanel) && tableLayoutPanel.Controls.Count > 0)
			{
				Control firstField = tableLayoutPanel.Controls[index: 0];
				firstField.BackColor = firstField.BackColor == Color.Black ? Color.White : Color.Black;
			}
		}

		/// <summary>Init the game board with the size 3x3</summary>
		private void InitGameBoard3X3() => InitGameBoard(tableLayoutPanel: tableLayoutPanelGame3x3, tabPage: tabPageGame3x3);

		/// <summary>Init the game board with the size 4x4</summary>
		private void InitGameBoard4X4() => InitGameBoard(tableLayoutPanel: tableLayoutPanelGame4x4, tabPage: tabPageGame4x4);

		/// <summary>Init the game board with the size 5x5</summary>
		private void InitGameBoard5X5() => InitGameBoard(tableLayoutPanel: tableLayoutPanelGame5x5, tabPage: tabPageGame5x5);

		/// <summary>Count the colors of the game board</summary>
		private void CountColorsInGameBoard()
		{
			TableLayoutPanel? tableLayoutPanel = GetActiveGameBoard();
			numberBlacks = 0;
			numberWhites = 0;
			if (tableLayoutPanel == null)
			{
				return;
			}
			foreach (Control button in tableLayoutPanel.Controls)
			{
				if (button.BackColor == Color.Black)
				{
					numberBlacks++;
				}
				else if (button.BackColor == Color.White)
				{
					numberWhites++;
				}
			}
		}

		/// <summary>Check if all fields in the game board have the same color</summary>
		/// <returns>true if all fields have the same color, otherwise false</returns>
		private bool IsSameColorsInGameBoard()
		{
			TableLayoutPanel? tableLayoutPanel = GetActiveGameBoard();
			return tableLayoutPanel != null && IsSameColorsInGameBoard(tableLayoutPanel: tableLayoutPanel);
		}

		/// <summary>Check if all fields in the game board have the same color</summary>
		/// <param name="tableLayoutPanel">The game board to check</param>
		/// <returns>true if all fields have the same color, otherwise false</returns>
		private bool IsSameColorsInGameBoard(TableLayoutPanel tableLayoutPanel)
		{
			int gameSize = GetGameBoardSize(tableLayoutPanel: tableLayoutPanel);
			int numbWhiteColor = 0, numbBlackColor = 0;
			foreach (Control button in tableLayoutPanel.Controls)
			{
				if (button.BackColor == Color.White)
				{
					numbWhiteColor++;
				}
				else if (button.BackColor == Color.Black)
				{
					numbBlackColor++;
				}
			}
			int totalFields = gameSize * gameSize;
			return numbWhiteColor == totalFields || numbBlackColor == totalFields;
		}

		/// <summary>Check for win in the game board</summary>
		private void CheckForWinInGameBoard()
		{
			if (!isGameStarted)
			{
				isGameStarted = true;
				toolStripMenuItemGameOptions.Enabled = false;
			}
			if (IsSameColorsInGameBoard())
			{
				FinishGame();
			}
		}

		/// <summary>Count the colors and check for win in the game board</summary>
		private void CountColorsAndCheckForWinInGameBoard()
		{
			CountColorsInGameBoard();
			CheckForWinInGameBoard();
		}

		/// <summary>Finish the game</summary>
		private void FinishGame()
		{
			timer.Stop();
			timer.Enabled = false;
			string message = new StringBuilder(Localization.wonMessage)

				.Replace("{sumKlicks}", sumClicks.ToString(provider: CultureInfo.CurrentCulture))

				.Replace("{sumTicks}", sumTicks.ToString(provider: CultureInfo.CurrentCulture))

				.ToString();

			_ = MessageBox.Show(
				text: message,
				caption: Localization.won,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information,
				defaultButton: MessageBoxDefaultButton.Button1,
				options: MessageBoxOptions.DefaultDesktopOnly);
			isGameStarted = false;
			toolStripMenuItemGameOptions.Enabled = true;
			sumTicks = 0;
			toolStripLabelStatistic.Text = Localization.statisticDefault;
			InitGameBoard();
		}

		#endregion

		#region Constructor

		/// <summary>Constructor</summary>
		public BlackAndWhiteForm()
		{
			InitializeComponent();
			BuildFieldButtonMap();
		}

		#endregion

		#region Load event handler

		/// <summary>Load the main window</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void MainForm_Load(object sender, EventArgs e)
		{
			ToolStripManager.Renderer = new Office2007Renderer();
			ClearStatusLabel(sender: null, e: EventArgs.Empty);
			tabControl.SelectedTab = tabPageGame3x3;
			InitGameBoard();
			labelProduct.Text = AssemblyInfo.AssemblyProduct;
			labelVersion.Text = AssemblyInfo.AssemblyVersion;
			labelCompany.Text = AssemblyInfo.AssemblyCompany;
			labelCopyright.Text = AssemblyInfo.AssemblyCopyright;
			textBoxDescription.Text = AssemblyInfo.AssemblyDescription;
		}

		#endregion

		#region Statuslabel

		/// <summary>Set a specific text to the status bar</summary>

		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameter <paramref name="e"/> is not needed, but must be indicated.</remarks>
		private void SetStatusLabel(object sender, EventArgs e)
		{
			// Set the status bar text based on the sender's accessible description
			switch (sender)
			{
				// If the sender is a control with an accessible description, set the status bar text
				// If the sender is a ToolStripItem with an accessible description, set the status bar text
				case Control { AccessibleDescription: not null } control:
					SetStatusBarText(text: control.AccessibleDescription);
					break;
				case ToolStripItem { AccessibleDescription: not null } item:
					SetStatusBarText(text: item.AccessibleDescription);
					break;
			}
		}

		/// <summary>Set a specific field text to the status bar</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameter <paramref name="e"/> is not needed, but must be indicated.</remarks>
		private void SetStatusLabelField(object sender, EventArgs e)
		{
			if (sender is not Button button)
			{
				return;
			}

			_ = stringBuilder.Clear().Append(value: button.AccessibleName).Append(value: ": ");
			_ = button.BackColor.Name switch
			{
				"Black" => stringBuilder.Append(value: Localization.colorNameBlack),
				"White" => stringBuilder.Append(value: Localization.colorNameWhite),
				_ => stringBuilder.Append(value: button.BackColor.Name)
			};
			toolStripStatusLabelInformation.Text = stringBuilder.ToString();
		}

		/// <summary>Clear the text of the status bar</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ClearStatusLabel(object? sender, EventArgs e) => SetStatusBarText(text: string.Empty);

		#endregion

		#region Click event handlers

		/// <summary>Init the game board 3x3 while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemNewGame3x3_Click(object sender, EventArgs e)
		{
			SetExclusiveChecked(
				selected: toolStripMenuItemNewGame3x3,
				otherItems: [toolStripMenuItemNewGame4x4, toolStripMenuItemNewGame5x5]);
			tabControl.SelectedTab = tabPageGame3x3;
			InitGameBoard3X3();
		}

		/// <summary>Init the game board 4x4 while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemNewGame4x4_Click(object sender, EventArgs e)
		{
			SetExclusiveChecked(
				selected: toolStripMenuItemNewGame4x4,
				otherItems: [toolStripMenuItemNewGame3x3, toolStripMenuItemNewGame5x5]);
			tabControl.SelectedTab = tabPageGame4x4;
			InitGameBoard4X4();
		}

		/// <summary>Init the game board 4x4 while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemNewGame5x5_Click(object sender, EventArgs e)
		{
			SetExclusiveChecked(
				selected: toolStripMenuItemNewGame5x5,
				otherItems: [toolStripMenuItemNewGame3x3, toolStripMenuItemNewGame4x4]);
			tabControl.SelectedTab = tabPageGame5x5;
			InitGameBoard5X5();
		}

		/// <summary>Active the linear field inverting while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemOptionLinear_Click(object sender, EventArgs e)
		{
			SetExclusiveChecked(
				selected: toolStripMenuItemOptionLinear,
				otherItems: [toolStripMenuItemOptionDiagonal, toolStripMenuItemOptionCombined]);
		}

		/// <summary>Active the diagonal field inverting while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemOptionDiagonal_Click(object sender, EventArgs e)
		{
			SetExclusiveChecked(
				selected: toolStripMenuItemOptionDiagonal,
				otherItems: [toolStripMenuItemOptionLinear, toolStripMenuItemOptionCombined]);
		}

		/// <summary>Active the combined field inverting while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemOptionCombined_Click(object sender, EventArgs e)
		{
			SetExclusiveChecked(
				selected: toolStripMenuItemOptionCombined,
				otherItems: [toolStripMenuItemOptionLinear, toolStripMenuItemOptionDiagonal]);
		}

		/// <summary>Invert the neighbour fields</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <param name="linearNeightbourFields">array with ids of the linear neighbour fields</param>
		/// <param name="diagonalNeightbourFields">array with ids of the diagonal neighbour fields</param>
		/// <param name="centeredField">array with id of the own field</param>
		/// <remarks>The parameter <paramref name="e"/> ist not needed, but must be indicated.</remarks>
		private void InvertNeighbourFields(
			object sender,
			EventArgs e,
			ushort[] linearNeightbourFields,
			ushort[] diagonalNeightbourFields,
			ushort[] centeredField)
		{
			sumClicks++;
			toolStripButtonPause.Enabled = true;
			timer.Enabled = true;
			if (toolStripMenuItemOptionLinear.Checked)
			{
				InvertFields(fieldId: linearNeightbourFields);
			}
			else if (toolStripMenuItemOptionDiagonal.Checked)
			{
				InvertFields(fieldId: diagonalNeightbourFields);
			}
			else if (toolStripMenuItemOptionCombined.Checked)
			{
				ushort[] combinedNeightbourFields = linearNeightbourFields.Union(second: diagonalNeightbourFields).ToArray();
				InvertFields(fieldId: combinedNeightbourFields);
			}
			if (toolStripMenuItemOptionCentered.Checked)
			{
				InvertFields(fieldId: centeredField);
				SetStatusLabelField(sender: sender, e: e);
			}
			CountColorsAndCheckForWinInGameBoard();
		}

		/// <summary>Invert the neighbour fields while clicking on the button</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameter <paramref name="e"/> ist not needed, but must be indicated.</remarks>
		private void ButtonField_Click(object sender, EventArgs e)
		{
			if (sender is not Control { Tag: string fieldTag } ||
				string.IsNullOrWhiteSpace(value: fieldTag) ||
				!TryParseFieldId(fieldTag: fieldTag, fieldId: out ushort centerFieldId))
			{
				return;
			}
			ushort[] linearNeightbourFields = GetNeighbourFieldIds(centerFieldId: centerFieldId, diagonal: false);
			ushort[] diagonalNeightbourFields = GetNeighbourFieldIds(centerFieldId: centerFieldId, diagonal: true);
			ushort[] centeredField = [centerFieldId];
			InvertNeighbourFields(
				sender: sender,
				e: e,
				linearNeightbourFields: linearNeightbourFields,
				diagonalNeightbourFields: diagonalNeightbourFields,
				centeredField: centeredField);
		}

		/// <summary>Begin a new game when the New Game split button is clicked</summary>

		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameter <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripSplitButtonNewGame_Click(object sender, EventArgs e)
		{
			isGameStarted = false;
			toolStripMenuItemGameOptions.Enabled = true;
			InitGameBoard();
		}

		/// <summary>Pause the current game when the Pause button is clicked</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameter <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripButtonPause_Click(object sender, EventArgs e)
		{
			timer.Enabled = false;
			_ = MessageBox.Show(
				text: Localization.gamePaused,
				caption: Localization.paused,
				buttons: MessageBoxButtons.OK,
				icon: MessageBoxIcon.Information,
				defaultButton: MessageBoxDefaultButton.Button1,
				options: MessageBoxOptions.DefaultDesktopOnly);
			timer.Enabled = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameter <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripSplitButtonLanguage_ButtonClick(object sender, EventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(name: "en");
			ComponentResourceManager resources = new(t: GetType());
			ApplyResourceToControl(res: resources, control: this);
		}

		#endregion

		#region Tick event handlers

		/// <summary>Output the statistics message on every tick</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void Timer_Tick(object sender, EventArgs e)
		{
			sumTicks++;
			CountColorsInGameBoard();
			toolStripLabelStatistic.Text = new StringBuilder(Localization.statistics)
				.Replace(oldValue: "{sumKlicks}", newValue: sumClicks.ToString(provider: CultureInfo.CurrentCulture))
				.Replace(oldValue: "{sumTicks}", newValue: sumTicks.ToString(provider: CultureInfo.CurrentCulture))
				.Replace(oldValue: "{numberBlacks}", newValue: numberBlacks.ToString(provider: CultureInfo.CurrentCulture))
				.Replace(oldValue: "{numberWhites}", newValue: numberWhites.ToString(provider: CultureInfo.CurrentCulture))
				.Replace(oldValue: "{sumInverts}", newValue: sumInverts.ToString(provider: CultureInfo.CurrentCulture))
				.ToString();
		}

		#endregion
	}
}
