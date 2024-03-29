﻿using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Properties;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms;

/// <summary>
/// Displays a clock and/or custom lines of text on a large screen, in-between competition classes.
/// </summary>
public sealed partial class CustomDisplayForm : Form
{
    private const int PaddingTop = 12;
    private const int PaddingBottom = 12;
    private const int CellSpacing = 6;

    private bool IsTextMode
    {
        [UsedImplicitly]
        get => !pictureBox.Visible;
        set
        {
            topPanel.Visible = value;
            middlePanel.Visible = value;
            bottomPanel.Visible = value;
            pictureBox.Visible = !value;

            if (!value)
            {
                pictureBox.Dock = DockStyle.Fill;
            }
        }
    }

    public CustomDisplayForm()
    {
        InitializeComponent();
        ResizePanelsToFitForm();
        ReloadConfiguration();
    }

    private void ResizePanelsToFitForm()
    {
        int h = (ClientSize.Height - (PaddingTop + CellSpacing + CellSpacing + PaddingBottom)) / 3;

        topPanel.Height = h;

        middlePanel.Height = h;

        middlePanel.Location = middlePanel.Location with
        {
            Y = PaddingTop + h + CellSpacing
        };

        bottomPanel.Height = h;

        bottomPanel.Location = bottomPanel.Location with
        {
            Y = PaddingTop + h + CellSpacing + h + CellSpacing
        };
    }

    private void CustomDisplayForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
        }
    }

    private void CustomDisplayForm_SizeChanged(object? sender, EventArgs e)
    {
        ResizePanelsToFitForm();
    }

    public void ReloadConfiguration()
    {
        IsTextMode = Settings.Default.CustomDisplayModeIsText;

        pictureBox.ImageLocation = Settings.Default.CustomDisplayPicturePath;

        topLabel.Text = Settings.Default.CustomDisplayModeFirstLineIsSystemTime ? GetCurrentTime() : Settings.Default.CustomDisplayFirstLine;
        middleLabel.Text = Settings.Default.CustomDisplaySecondLine;
        bottomLabel.Text = Settings.Default.CustomDisplayThirdLine;

        clockTimer.Enabled = Settings.Default.CustomDisplayModeFirstLineIsSystemTime;
    }

    private void ClockTimer_Tick(object? sender, EventArgs e)
    {
        if (Settings.Default.CustomDisplayModeFirstLineIsSystemTime)
        {
            topLabel.Text = GetCurrentTime();
        }
    }

    [Pure]
    private static string GetCurrentTime()
    {
        return SystemContext.Now().ToShortTimeString();
    }
}
