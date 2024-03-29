﻿using System.Globalization;
using System.Windows.Forms;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms;

/// <summary>
/// Enables configuration of mediator status.
/// </summary>
public sealed partial class MediatorStatusSelectionForm : Form
{
    private static readonly Dictionary<int, string> CodeToTextMap = new();

    public int StatusCode
    {
        get => GetNumericValue();
        set => statusComboBox.Text = GetTextFor(value);
    }

    static MediatorStatusSelectionForm()
    {
        foreach (int code in KnownMediatorStatusCode.All)
        {
            CodeToTextMap.Add(code, KnownMediatorStatusCode.GetNameFor(code));
        }
    }

    public MediatorStatusSelectionForm()
    {
        InitializeComponent();

        statusComboBox.Items.AddRange(CodeToTextMap.Values.Cast<object>().ToArray());
    }

    private int GetNumericValue()
    {
        foreach (KeyValuePair<int, string> pair in CodeToTextMap.Where(pair => statusComboBox.Text == pair.Value))
        {
            return pair.Key;
        }

        return int.TryParse(statusComboBox.Text, out int parsedValue) && parsedValue is >= 0 and <= 999 ? parsedValue : -1;
    }

    private void OkButton_Click(object? sender, EventArgs e)
    {
        if (StatusCode == -1)
        {
            MessageBox.Show("Status must be in range [0-999].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            DialogResult = DialogResult.OK;
        }
    }

    public static string GetTextFor(int statusCode)
    {
        return CodeToTextMap.ContainsKey(statusCode) ? CodeToTextMap[statusCode] : statusCode.ToString(CultureInfo.InvariantCulture);
    }
}
