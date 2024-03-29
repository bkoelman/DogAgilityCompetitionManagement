﻿using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms;

/// <summary>
/// Displays scrolling competition run results on a large screen.
/// </summary>
public sealed partial class ClassResultsDisplayForm : Form
{
    private const int ControlHeightIncrement = 52;

    private IReadOnlyCollection<CompetitionRunResult>? pendingRefreshOfRankings;

    public ClassResultsDisplayForm()
    {
        InitializeComponent();

        CompetitionClassModel snapshot = CacheManager.DefaultInstance.ActiveModel;

        IReadOnlyCollection<CompetitionRunResult> runResults = snapshot.FilterCompletedAndSortedAscendingByPlacement().Results;
        UpdateRankings(runResults);
        SetClass(snapshot.ClassInfo);
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);

        if (Visible && pendingRefreshOfRankings != null)
        {
            UpdateRankings(pendingRefreshOfRankings);
            pendingRefreshOfRankings = null;
        }
    }

    public void UpdateRankings(IReadOnlyCollection<CompetitionRunResult> rankings)
    {
        Guard.NotNull(rankings, nameof(rankings));

        if (Visible)
        {
            RunHistoryLine[] firstThreeControls =
            {
                runHistoryLine0001,
                runHistoryLine0002,
                runHistoryLine0003
            };

            for (int index = 0; index < firstThreeControls.Length; index++)
            {
                if (index < rankings.Count)
                {
                    CompetitionRunResult runResult = rankings.ElementAt(index);
                    firstThreeControls[index].SetCompetitionRunResult(runResult);
                }
                else
                {
                    firstThreeControls[index].ClearCompetitionRunResult();
                }
            }

            IEnumerable<CompetitionRunResult> remainingRunResults = rankings.Skip(3);

            // Performance optimization: prevents seconds delay and
            // Win32 crash caused by creating too many child controls.
            remainingRunResults = remainingRunResults.Take(100);

            RecreateScrollableRunHistoryLines(remainingRunResults.ToList());
        }
        else
        {
            pendingRefreshOfRankings = rankings;
        }
    }

    public void SetClass(CompetitionClassInfo? classInfo)
    {
        gradeLabel.Text = classInfo != null ? classInfo.Grade : string.Empty;
        classTypeLabel.Text = classInfo != null ? classInfo.Type : string.Empty;
        standardCourseTimeValueLabel.Text = classInfo?.StandardCourseTime != null ? $"{classInfo.StandardCourseTime.Value.TotalSeconds:0}" : string.Empty;
    }

    private void RecreateScrollableRunHistoryLines(IReadOnlyCollection<CompetitionRunResult> runResults)
    {
        UpdateScrollingPanel(() =>
        {
            scrollingPanel.ClearInnerControls();

            int heightOffset = 0;
            int minControlCount = (int)Math.Truncate((decimal)scrollingPanel.Height / ControlHeightIncrement);

            for (int index = 0; index < Math.Max(minControlCount, runResults.Count); index++)
            {
                var runHistoryLine = new RunHistoryLine
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    Location = new Point(2, heightOffset),
                    Name = $"runHistoryLine{index:0000}",
                    Size = new Size(ClientSize.Width - 4, 51)
                };

                CompetitionRunResult? runResult = runResults.ElementAtOrDefault(index);

                if (runResult != null)
                {
                    runHistoryLine.SetCompetitionRunResult(runResult);
                }
                else
                {
                    runHistoryLine.ClearCompetitionRunResult();
                }

                scrollingPanel.AddToInnerControls(runHistoryLine);
                heightOffset += ControlHeightIncrement;
            }
        });
    }

    private void UpdateScrollingPanel(Action action)
    {
        try
        {
            scrollingPanel.BeginUpdate();
            action();
        }
        finally
        {
            scrollingPanel.EndUpdate();
        }
    }

    private void ClassResultsDisplayForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
        }
    }
}
