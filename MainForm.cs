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
				Button? button = i switch
				{
					311 => buttonGame3Field11,
					312 => buttonGame3Field12,
					313 => buttonGame3Field13,
					321 => buttonGame3Field21,
					322 => buttonGame3Field22,
					323 => buttonGame3Field23,
					331 => buttonGame3Field31,
					332 => buttonGame3Field32,
					333 => buttonGame3Field33,
					411 => buttonGame4Field11,
					412 => buttonGame4Field12,
					413 => buttonGame4Field13,
					414 => buttonGame4Field14,
					421 => buttonGame4Field21,
					422 => buttonGame4Field22,
					423 => buttonGame4Field23,
					424 => buttonGame4Field24,
					431 => buttonGame4Field31,
					432 => buttonGame4Field32,
					433 => buttonGame4Field33,
					434 => buttonGame4Field34,
					441 => buttonGame4Field41,
					442 => buttonGame4Field42,
					443 => buttonGame4Field43,
					444 => buttonGame4Field44,
					511 => buttonGame5Field11,
					512 => buttonGame5Field12,
					513 => buttonGame5Field13,
					514 => buttonGame5Field14,
					515 => buttonGame5Field15,
					521 => buttonGame5Field21,
					522 => buttonGame5Field22,
					523 => buttonGame5Field23,
					524 => buttonGame5Field24,
					525 => buttonGame5Field25,
					531 => buttonGame5Field31,
					532 => buttonGame5Field32,
					533 => buttonGame5Field33,
					534 => buttonGame5Field34,
					535 => buttonGame5Field35,
					541 => buttonGame5Field41,
					542 => buttonGame5Field42,
					543 => buttonGame5Field43,
					544 => buttonGame5Field44,
					545 => buttonGame5Field45,
					551 => buttonGame5Field51,
					552 => buttonGame5Field52,
					553 => buttonGame5Field53,
					554 => buttonGame5Field54,
					555 => buttonGame5Field55,
					_ => null
				};
				if (button != null)
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
			toolStripButtonPause.Enabled = true;
			timer.Enabled = true;
			sumInverts++;
			return color == Color.Black ? Color.White : Color.Black;
		}

		/// <summary>Init the game board</summary>
		private void InitGameBoard()
		{
			if (toolStripMenuItemNewGame3x3.Checked)
			{
				InitGameBoard3X3();
			}
			else if (toolStripMenuItemNewGame4x4.Checked)
			{
				InitGameBoard4X4();
			}
			else
			{
				InitGameBoard5X5();
			}
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
			TableLayoutPanel? tableLayoutPanel = toolStripMenuItemNewGame3x3.Checked ? tableLayoutPanelGame3x3 :
				toolStripMenuItemNewGame4x4.Checked ? tableLayoutPanelGame4x4 :
				toolStripMenuItemNewGame5x5.Checked ? tableLayoutPanelGame5x5 : null;
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
			TableLayoutPanel? tableLayoutPanel = toolStripMenuItemNewGame3x3.Checked ? tableLayoutPanelGame3x3 :
				toolStripMenuItemNewGame4x4.Checked ? tableLayoutPanelGame4x4 :
				toolStripMenuItemNewGame5x5.Checked ? tableLayoutPanelGame5x5 : null;
			return tableLayoutPanel != null && IsSameColorsInGameBoard(tableLayoutPanel: tableLayoutPanel);
		}

		/// <summary>Check if all fields in the game board have the same color</summary>
		/// <param name="tableLayoutPanel">The game board to check</param>
		/// <returns>true if all fields have the same color, otherwise false</returns>
		private bool IsSameColorsInGameBoard(TableLayoutPanel tableLayoutPanel)
		{
			int gameSize = tableLayoutPanel == tableLayoutPanelGame3x3 ? 3 :
				tableLayoutPanel == tableLayoutPanelGame4x4 ? 4 : 5;
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
			string message = $"Du hast mit {sumClicks} Klicks in {sumTicks} Ticks gewonnen.";
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
		public BlackAndWhiteForm() => InitializeComponent();

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
			toolStripMenuItemNewGame3x3.Checked = true;
			toolStripMenuItemNewGame4x4.Checked = false;
			toolStripMenuItemNewGame5x5.Checked = false;
			tabControl.SelectedTab = tabPageGame3x3;
			InitGameBoard3X3();
		}

		/// <summary>Init the game board 4x4 while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemNewGame4x4_Click(object sender, EventArgs e)
		{
			toolStripMenuItemNewGame3x3.Checked = false;
			toolStripMenuItemNewGame4x4.Checked = true;
			toolStripMenuItemNewGame5x5.Checked = false;
			tabControl.SelectedTab = tabPageGame4x4;
			InitGameBoard4X4();
		}

		/// <summary>Init the game board 4x4 while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemNewGame5x5_Click(object sender, EventArgs e)
		{
			toolStripMenuItemNewGame3x3.Checked = false;
			toolStripMenuItemNewGame4x4.Checked = false;
			toolStripMenuItemNewGame5x5.Checked = true;
			tabControl.SelectedTab = tabPageGame5x5;
			InitGameBoard5X5();
		}

		/// <summary>Active the linear field inverting while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemOptionLinear_Click(object sender, EventArgs e)
		{
			toolStripMenuItemOptionLinear.Checked = true;
			toolStripMenuItemOptionDiagonal.Checked = false;
			toolStripMenuItemOptionCombined.Checked = false;
		}

		/// <summary>Active the diagonal field inverting while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemOptionDiagonal_Click(object sender, EventArgs e)
		{
			toolStripMenuItemOptionLinear.Checked = false;
			toolStripMenuItemOptionDiagonal.Checked = true;
			toolStripMenuItemOptionCombined.Checked = false;
		}

		/// <summary>Active the combined field inverting while clicking the ToolStripMenuItem</summary>
		/// <param name="sender">object sender</param>
		/// <param name="e">event arguments</param>
		/// <remarks>The parameters <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void ToolStripMenuItemOptionCombined_Click(object sender, EventArgs e)
		{
			toolStripMenuItemOptionLinear.Checked = false;
			toolStripMenuItemOptionDiagonal.Checked = false;
			toolStripMenuItemOptionCombined.Checked = true;
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
			if (sender is not Control { Tag: string fieldTag } || string.IsNullOrWhiteSpace(fieldTag))
			{
				return;
			}
			ushort[] linearNeightbourFields = [0];
			ushort[] diagonalNeightbourFields = [0];
			ushort[] centeredField = [0];
			switch (fieldTag)
			{
				case "311":
					linearNeightbourFields = [312, 321];
					diagonalNeightbourFields = [322];
					centeredField = [311];
					break;
				case "312":
					linearNeightbourFields = [311, 313, 322];
					diagonalNeightbourFields = [321, 323];
					centeredField = [312];
					break;
				case "313":
					linearNeightbourFields = [312, 323];
					diagonalNeightbourFields = [322];
					centeredField = [313];
					break;
				case "321":
					linearNeightbourFields = [311, 322, 331];
					diagonalNeightbourFields = [312, 332];
					centeredField = [321];
					break;
				case "322":
					linearNeightbourFields = [312, 321, 323, 332];
					diagonalNeightbourFields = [311, 313, 331, 333];
					centeredField = [322];
					break;
				case "323":
					linearNeightbourFields = [313, 322, 333];
					diagonalNeightbourFields = [312, 332];
					centeredField = [323];
					break;
				case "331":
					linearNeightbourFields = [321, 332];
					diagonalNeightbourFields = [322];
					centeredField = [331];
					break;
				case "332":
					linearNeightbourFields = [322, 331, 333];
					diagonalNeightbourFields = [321, 323];
					centeredField = [332];
					break;
				case "333":
					linearNeightbourFields = [323, 332];
					diagonalNeightbourFields = [322];
					centeredField = [333];
					break;
				case "411":
					linearNeightbourFields = [412, 421];
					diagonalNeightbourFields = [422];
					centeredField = [411];
					break;
				case "412":
					linearNeightbourFields = [411, 413, 422];
					diagonalNeightbourFields = [421, 423];
					centeredField = [412];
					break;
				case "413":
					linearNeightbourFields = [412, 414, 423];
					diagonalNeightbourFields = [422, 424];
					centeredField = [413];
					break;
				case "414":
					linearNeightbourFields = [413, 424];
					diagonalNeightbourFields = [423];
					centeredField = [414];
					break;
				case "421":
					linearNeightbourFields = [411, 422, 431];
					diagonalNeightbourFields = [412, 432];
					centeredField = [421];
					break;
				case "422":
					linearNeightbourFields = [412, 421, 423, 432];
					diagonalNeightbourFields = [411, 413, 431, 433];
					centeredField = [422];
					break;
				case "423":
					linearNeightbourFields = [413, 422, 424, 433];
					diagonalNeightbourFields = [412, 414, 432, 434];
					centeredField = [423];
					break;
				case "424":
					linearNeightbourFields = [414, 423, 434];
					diagonalNeightbourFields = [413, 433];
					centeredField = [424];
					break;
				case "431":
					linearNeightbourFields = [421, 432, 441];
					diagonalNeightbourFields = [422, 442];
					centeredField = [431];
					break;
				case "432":
					linearNeightbourFields = [422, 431, 433, 442];
					diagonalNeightbourFields = [421, 423, 441, 443];
					centeredField = [432];
					break;
				case "433":
					linearNeightbourFields = [423, 432, 434, 443];
					diagonalNeightbourFields = [422, 424, 442, 444];
					centeredField = [433];
					break;
				case "434":
					linearNeightbourFields = [424, 433, 444];
					diagonalNeightbourFields = [423, 443];
					centeredField = [434];
					break;
				case "441":
					linearNeightbourFields = [431, 442];
					diagonalNeightbourFields = [432];
					centeredField = [441];
					break;
				case "442":
					linearNeightbourFields = [432, 441, 443];
					diagonalNeightbourFields = [431, 433];
					centeredField = [442];
					break;
				case "443":
					linearNeightbourFields = [433, 442, 444];
					diagonalNeightbourFields = [432, 434];
					centeredField = [443];
					break;
				case "444":
					linearNeightbourFields = [434, 443];
					diagonalNeightbourFields = [433];
					centeredField = [444];
					break;
				case "511":
					linearNeightbourFields = [512, 521];
					diagonalNeightbourFields = [522];
					centeredField = [511];
					break;
				case "512":
					linearNeightbourFields = [511, 513, 522];
					diagonalNeightbourFields = [521, 523];
					centeredField = [512];
					break;
				case "513":
					linearNeightbourFields = [512, 514, 523];
					diagonalNeightbourFields = [522, 524];
					centeredField = [513];
					break;
				case "514":
					linearNeightbourFields = [513, 515, 524];
					diagonalNeightbourFields = [523, 525];
					centeredField = [514];
					break;
				case "515":
					linearNeightbourFields = [514, 525];
					diagonalNeightbourFields = [524];
					centeredField = [515];
					break;
				case "521":
					linearNeightbourFields = [511, 522, 531];
					diagonalNeightbourFields = [512, 532];
					centeredField = [521];
					break;
				case "522":
					linearNeightbourFields = [512, 521, 523, 532];
					diagonalNeightbourFields = [511, 513, 531, 533];
					centeredField = [522];
					break;
				case "523":
					linearNeightbourFields = [513, 522, 524, 533];
					diagonalNeightbourFields = [512, 514, 532, 534];
					centeredField = [523];
					break;
				case "524":
					linearNeightbourFields = [514, 523, 525, 534];
					diagonalNeightbourFields = [513, 515, 533, 535];
					centeredField = [524];
					break;
				case "525":
					linearNeightbourFields = [515, 524, 535];
					diagonalNeightbourFields = [514, 534];
					centeredField = [525];
					break;
				case "531":
					linearNeightbourFields = [521, 532, 541];
					diagonalNeightbourFields = [522, 542];
					centeredField = [531];
					break;
				case "532":
					linearNeightbourFields = [522, 531, 533, 542];
					diagonalNeightbourFields = [521, 523, 541, 543];
					centeredField = [532];
					break;
				case "533":
					linearNeightbourFields = [523, 532, 534, 543];
					diagonalNeightbourFields = [522, 524, 542, 544];
					centeredField = [533];
					break;
				case "534":
					linearNeightbourFields = [524, 533, 535, 544];
					diagonalNeightbourFields = [523, 525, 543, 545];
					centeredField = [534];
					break;
				case "535":
					linearNeightbourFields = [525, 534, 545];
					diagonalNeightbourFields = [524, 544];
					centeredField = [535];
					break;
				case "541":
					linearNeightbourFields = [531, 542, 551];
					diagonalNeightbourFields = [532, 552];
					centeredField = [541];
					break;
				case "542":
					linearNeightbourFields = [532, 541, 543, 552];
					diagonalNeightbourFields = [531, 533, 551, 553];
					centeredField = [542];
					break;
				case "543":
					linearNeightbourFields = [533, 542, 544, 553];
					diagonalNeightbourFields = [532, 534, 552, 554];
					centeredField = [543];
					break;
				case "544":
					linearNeightbourFields = [534, 543, 545, 554];
					diagonalNeightbourFields = [533, 535, 553, 555];
					centeredField = [544];
					break;
				case "545":
					linearNeightbourFields = [535, 544, 555];
					diagonalNeightbourFields = [534, 554];
					centeredField = [545];
					break;
				case "551":
					linearNeightbourFields = [541, 552];
					diagonalNeightbourFields = [542];
					centeredField = [551];
					break;
				case "552":
					linearNeightbourFields = [542, 551, 553];
					diagonalNeightbourFields = [541, 543];
					centeredField = [552];
					break;
				case "553":
					linearNeightbourFields = [543, 552, 554];
					diagonalNeightbourFields = [542, 544];
					centeredField = [553];
					break;
				case "554":
					linearNeightbourFields = [544, 553, 555];
					diagonalNeightbourFields = [543, 545];
					centeredField = [554];
					break;
				case "555":
					linearNeightbourFields = [545, 554];
					diagonalNeightbourFields = [544];
					centeredField = [555];
					break;
				default:
					return;
			}
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

		/// <summary>Pause a new game while the ToolStripButton</summary>
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
		/// <remarks>The parameter <paramref name="sender"/> and <paramref name="e"/> are not needed, but must be indicated.</remarks>
		private void Timer_Tick(object sender, EventArgs e)
		{
			sumTicks++;
			toolStripLabelStatistic.Text = $@"Klicks: {sumClicks} - Ticks: {sumTicks} - Blacks: {numberBlacks} - Whites: {numberWhites} - Inverts: {sumInverts}";
		}

		#endregion
	}
}
