using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.UI.Controls.Shapes;

namespace DogAgilityCompetition.Controller.UI.Controls;

/// <summary>
/// Shows a state transition diagram, highlighting the previous, current and possible next states.
/// </summary>
public sealed class StateVisualizer : Control
{
    private const int ArrowLength = 18;

    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private static readonly List<ShapeState> StateRenderOrder = new()
    {
        ShapeState.Disabled,
        ShapeState.None,
        ShapeState.Candidate,
        ShapeState.Selected
    };

    private readonly List<Shape> shapes = new();
    private readonly StateTransitionArrows arrows;
    private readonly StateBlocks blocks;
    private readonly Icon errorIcon;

    private CompetitionClassState activeState = CompetitionClassState.Offline;
    private int intermediateTimerCount = 3;
    private bool seenUnsupportedTransition;

    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
            blocks.SetFont(value);
        }
    }

    public int IntermediateTimerCount
    {
        get => intermediateTimerCount;
        set
        {
            if (intermediateTimerCount != value)
            {
                Guard.InRangeInclusive(value, nameof(value), 0, 3);
                intermediateTimerCount = value;

                CompetitionClassState? previousState = TryGetPreviousState();
                CompetitionClassState activeStateBefore = activeState;

                blocks.MarkDisabledBlocks();
                arrows.MarkDisabledArrows();

                if (previousState == null || !CanTransitionFromTo(previousState.Value, activeState))
                {
                    // No previous state -or- active state has become disabled.
                    if (activeState != CompetitionClassState.Offline)
                    {
                        activeState = CompetitionClassState.Offline;
                        Log.Warn($"State was reset to {activeState} because active state {activeStateBefore} has become disabled.");
                    }
                }

                UpdateShapeStatesForTransition(previousState, activeState);
                Invalidate();
            }
        }
    }

    public StateVisualizer()
    {
        DoubleBuffered = true;

        blocks = new StateBlocks(this);
        arrows = new StateTransitionArrows(this);

        MarkCandidateShapes(activeState);

        errorIcon = GetIconFromErrorProvider();
    }

    private CompetitionClassState? TryGetPreviousState()
    {
        if (arrows.TransitionTable.Any(pair => pair.Value.State == ShapeState.Selected))
        {
            KeyValuePair<Tuple<CompetitionClassState, CompetitionClassState>, ArrowShape> arrowFromPreviousState =
                arrows.TransitionTable.First(pair => pair.Value.State == ShapeState.Selected);

            return arrowFromPreviousState.Key.Item1;
        }

        return null;
    }

    private static Icon GetIconFromErrorProvider()
    {
        using var provider = new ErrorProvider();
        return (Icon)provider.Icon.Clone();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Guard.NotNull(e, nameof(e));
        base.OnPaint(e);

        PaintShapeShadows(e.Graphics);
        PaintShapeFills(e.Graphics);
        PaintShapeBorders(e.Graphics);
        PaintErrorIcon(e.Graphics);
    }

    private void PaintShapeShadows(Graphics graphics)
    {
        foreach (Shape shape in shapes.OrderBy(x => StateRenderOrder.IndexOf(x.State)))
        {
            shape.DrawShadow(graphics);
        }
    }

    private void PaintShapeFills(Graphics graphics)
    {
        foreach (Shape shape in shapes.OrderBy(x => StateRenderOrder.IndexOf(x.State)))
        {
            shape.DrawFill(graphics);
        }
    }

    private void PaintShapeBorders(Graphics graphics)
    {
        foreach (Shape shape in shapes.OrderBy(x => StateRenderOrder.IndexOf(x.State)))
        {
            shape.DrawBorder(graphics);
        }
    }

    private void PaintErrorIcon(Graphics graphics)
    {
        if (seenUnsupportedTransition)
        {
            graphics.DrawIcon(errorIcon, 5, 5);
        }
    }

    public void TransitionTo(CompetitionClassState nextState)
    {
        if (nextState == activeState)
        {
            return;
        }

        if (!CanTransitionFromTo(activeState, nextState))
        {
            Log.Warn($"Unsupported transition from {activeState} to {nextState}: no path available.");
            seenUnsupportedTransition = true;
        }

        UpdateShapeStatesForTransition(activeState, nextState);

        activeState = nextState;
        Invalidate();
    }

    private void UpdateShapeStatesForTransition(CompetitionClassState? previousState, CompetitionClassState nextState)
    {
        ResetShapeStates();
        MarkCandidateShapes(nextState);
        MarkSelectedShapes(previousState, nextState);
    }

    private bool CanTransitionFromTo(CompetitionClassState previousState, CompetitionClassState nextState)
    {
        Tuple<CompetitionClassState, CompetitionClassState> previousToNextKey = GetTransition(previousState, nextState);

        return arrows.TransitionTable.ContainsKey(previousToNextKey) && blocks.Table.ContainsKey(nextState) &&
            blocks.Table[nextState].State != ShapeState.Disabled;
    }

    private void ResetShapeStates()
    {
        foreach (TextBlock textBlock in blocks.Table.Values.Where(s => s.State != ShapeState.Disabled))
        {
            textBlock.State = ShapeState.None;
        }

        foreach (ArrowShape arrow in arrows.TransitionTable.Values.Where(s => s.State != ShapeState.Disabled))
        {
            arrow.State = ShapeState.None;
        }
    }

    private void MarkCandidateShapes(CompetitionClassState nextState)
    {
        foreach (CompetitionClassState state in AllStatesExcept(nextState))
        {
            Tuple<CompetitionClassState, CompetitionClassState> key = GetTransition(nextState, state);

            if (arrows.TransitionTable.ContainsKey(key))
            {
                if (blocks.Table[state].State != ShapeState.Disabled && arrows.TransitionTable[key].State != ShapeState.Disabled)
                {
                    blocks.Table[state].State = ShapeState.Candidate;
                    arrows.TransitionTable[key].State = ShapeState.Candidate;
                }
            }
        }
    }

    private static IEnumerable<CompetitionClassState> AllStatesExcept(params CompetitionClassState[] states)
    {
        return Enum.GetValues(typeof(CompetitionClassState)).Cast<CompetitionClassState>().Except(states).ToArray();
    }

    private void MarkSelectedShapes(CompetitionClassState? previousState, CompetitionClassState nextState)
    {
        ArrowShape? previousToNextArrow = null;

        if (previousState != null)
        {
            Tuple<CompetitionClassState, CompetitionClassState> previousToNextKey = GetTransition(previousState.Value, nextState);

            if (arrows.TransitionTable.ContainsKey(previousToNextKey))
            {
                previousToNextArrow = arrows.TransitionTable[previousToNextKey];
            }
        }

        if (blocks.Table.ContainsKey(nextState))
        {
            blocks.Table[nextState].State = ShapeState.Selected;

            if (previousToNextArrow != null)
            {
                previousToNextArrow.State = ShapeState.Selected;
            }
        }
    }

    private static Tuple<CompetitionClassState, CompetitionClassState> GetTransition(CompetitionClassState from, CompetitionClassState to)
    {
        return new Tuple<CompetitionClassState, CompetitionClassState>(from, to);
    }

    private sealed class StateTransitionArrows
    {
        private readonly StateVisualizer owner;

        public Dictionary<Tuple<CompetitionClassState, CompetitionClassState>, ArrowShape> TransitionTable { get; }

        public StateTransitionArrows(StateVisualizer owner)
        {
            Guard.NotNull(owner, nameof(owner));
            this.owner = owner;

            TransitionTable = CreateTransitionArrowTable();

            foreach (ArrowShape shape in TransitionTable.Values)
            {
                owner.shapes.Add(shape);
            }
        }

        private Dictionary<Tuple<CompetitionClassState, CompetitionClassState>, ArrowShape> CreateTransitionArrowTable()
        {
            var transitions = new List<Tuple<Tuple<CompetitionClassState, CompetitionClassState>, ArrowShape>>
            {
                ComposeTransition(CompetitionClassState.Offline, CompetitionClassState.SetupCompleted, HorizontalAlignment.Center, VerticalAlignment.Bottom,
                    VerticalAlignment.Top),
                // ***
                ComposeTransition(CompetitionClassState.SetupCompleted, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Middle,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.SetupCompleted, CompetitionClassState.WaitingForSync, HorizontalAlignment.Center,
                    VerticalAlignment.Bottom, VerticalAlignment.Top),
                // ***
                ComposeTransition(CompetitionClassState.WaitingForSync, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Middle,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.WaitingForSync, CompetitionClassState.ReadyToStart, HorizontalAlignment.Center,
                    VerticalAlignment.Bottom, VerticalAlignment.Top),
                // ***
                ComposeTransition(CompetitionClassState.ReadyToStart, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Middle,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.ReadyToStart, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left, VerticalAlignment.Top,
                    VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.ReadyToStart, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.ReadyToStart, CompetitionClassState.StartPassed, HorizontalAlignment.Center, VerticalAlignment.Bottom,
                    VerticalAlignment.Top),
                // ***
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                    VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.ReadyToStart, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                    VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.Intermediate1Passed, HorizontalAlignment.Center,
                    VerticalAlignment.Bottom, VerticalAlignment.Top),
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.FinishPassed, HorizontalAlignment.Right, VerticalAlignment.Bottom,
                    VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.StartPassed, CompetitionClassState.RunCompleted, HorizontalAlignment.Right, VerticalAlignment.Bottom,
                    VerticalAlignment.Bottom, 3),
                // ***
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left,
                    VerticalAlignment.Bottom, VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left,
                    VerticalAlignment.Top, VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.ReadyToStart, HorizontalAlignment.Left,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.Intermediate2Passed, HorizontalAlignment.Center,
                    VerticalAlignment.Bottom, VerticalAlignment.Top),
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.FinishPassed, HorizontalAlignment.Right,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.Intermediate1Passed, CompetitionClassState.RunCompleted, HorizontalAlignment.Right,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 3),
                // ***
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left,
                    VerticalAlignment.Bottom, VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left,
                    VerticalAlignment.Top, VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.ReadyToStart, HorizontalAlignment.Left,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.Intermediate3Passed, HorizontalAlignment.Center,
                    VerticalAlignment.Bottom, VerticalAlignment.Top),
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.FinishPassed, HorizontalAlignment.Right,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.Intermediate2Passed, CompetitionClassState.RunCompleted, HorizontalAlignment.Right,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 3),
                // ***
                ComposeTransition(CompetitionClassState.Intermediate3Passed, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.Intermediate3Passed, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left,
                    VerticalAlignment.Bottom, VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.Intermediate3Passed, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left,
                    VerticalAlignment.Top, VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.Intermediate3Passed, CompetitionClassState.ReadyToStart, HorizontalAlignment.Left,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.Intermediate3Passed, CompetitionClassState.FinishPassed, HorizontalAlignment.Center,
                    VerticalAlignment.Bottom, VerticalAlignment.Top),
                ComposeTransition(CompetitionClassState.Intermediate3Passed, CompetitionClassState.RunCompleted, HorizontalAlignment.Right,
                    VerticalAlignment.Bottom, VerticalAlignment.Bottom, 3),
                // ***
                ComposeTransition(CompetitionClassState.FinishPassed, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.FinishPassed, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                    VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.FinishPassed, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.FinishPassed, CompetitionClassState.ReadyToStart, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                    VerticalAlignment.Bottom, 2),
                ComposeTransition(CompetitionClassState.FinishPassed, CompetitionClassState.RunCompleted, HorizontalAlignment.Center, VerticalAlignment.Bottom,
                    VerticalAlignment.Top),
                // ***
                ComposeTransition(CompetitionClassState.RunCompleted, CompetitionClassState.Offline, HorizontalAlignment.Right, VerticalAlignment.Top,
                    VerticalAlignment.Middle),
                ComposeTransition(CompetitionClassState.RunCompleted, CompetitionClassState.SetupCompleted, HorizontalAlignment.Left, VerticalAlignment.Top,
                    VerticalAlignment.Middle, 3),
                ComposeTransition(CompetitionClassState.RunCompleted, CompetitionClassState.WaitingForSync, HorizontalAlignment.Left, VerticalAlignment.Top,
                    VerticalAlignment.Middle)
            };

            return transitions.ToDictionary(pair => pair.Item1, pair => pair.Item2);
        }

        private Tuple<Tuple<CompetitionClassState, CompetitionClassState>, ArrowShape> ComposeTransition(CompetitionClassState fromState,
            CompetitionClassState toState, HorizontalAlignment side, VerticalAlignment fromConnection, VerticalAlignment toConnection, int depth = 1)
        {
            Tuple<CompetitionClassState, CompetitionClassState> transition = GetTransition(fromState, toState);

            TextBlock fromBlock = owner.blocks.GetBlockForState(fromState);
            TextBlock toBlock = owner.blocks.GetBlockForState(toState);

            PointF fromPoint = GetPointFor(fromBlock, side, fromConnection);
            PointF toPoint = GetPointFor(toBlock, side, toConnection);

            ArrowShape arrow = CreateArrowFor(side, fromPoint, toPoint, depth);
            return Tuple.Create(transition, arrow);
        }

        private static PointF GetPointFor(TextBlock block, HorizontalAlignment side, VerticalAlignment connection)
        {
            switch (side)
            {
                case HorizontalAlignment.Left:
                    switch (connection)
                    {
                        case VerticalAlignment.Top:
                            return block.LeftTopConnection;
                        case VerticalAlignment.Middle:
                            return block.LeftMiddleConnection;
                        case VerticalAlignment.Bottom:
                            return block.LeftBottomConnection;
                        default:
                            throw ExceptionFactory.CreateNotSupportedExceptionFor(connection);
                    }
                case HorizontalAlignment.Center:
                    switch (connection)
                    {
                        case VerticalAlignment.Top:
                            return block.TopCenterConnection;
                        case VerticalAlignment.Bottom:
                            return block.BottomCenterConnection;
                        default:
                            throw ExceptionFactory.CreateNotSupportedExceptionFor(connection);
                    }
                case HorizontalAlignment.Right:
                    switch (connection)
                    {
                        case VerticalAlignment.Top:
                            return block.RightTopConnection;
                        case VerticalAlignment.Middle:
                            return block.RightMiddleConnection;
                        case VerticalAlignment.Bottom:
                            return block.RightBottomConnection;
                        default:
                            throw ExceptionFactory.CreateNotSupportedExceptionFor(connection);
                    }
                default:
                    throw ExceptionFactory.CreateNotSupportedExceptionFor(side);
            }
        }

        private static ArrowShape CreateArrowFor(HorizontalAlignment side, PointF from, PointF to, int depth)
        {
            switch (side)
            {
                case HorizontalAlignment.Left:
                    return CreateArrowAtLeftSide(from, to, depth);
                case HorizontalAlignment.Center:
                    return new SingleLineArrow(from, to);
                case HorizontalAlignment.Right:
                    return CreateArrowAtRightSide(from, to, depth);
                default:
                    throw ExceptionFactory.CreateNotSupportedExceptionFor(side);
            }
        }

        private static MultiLineArrow CreateArrowAtLeftSide(PointF from, PointF to, int depth)
        {
            return new MultiLineArrow.Builder(from).Left(ArrowLength * depth).Up(from.Y - to.Y).Right(ArrowLength * depth).Build();
        }

        private static MultiLineArrow CreateArrowAtRightSide(PointF from, PointF to, int depth)
        {
            return new MultiLineArrow.Builder(from).Right(ArrowLength * depth).Up(from.Y - to.Y).Left(ArrowLength * depth).Build();
        }

        public void MarkDisabledArrows()
        {
            ShapeState stateInt1 = owner.intermediateTimerCount >= 1 ? ShapeState.None : ShapeState.Disabled;

            foreach (ArrowShape arrow in GetArrowsConnectedTo(CompetitionClassState.Intermediate1Passed))
            {
                arrow.State = stateInt1;
            }

            ShapeState stateInt2 = owner.intermediateTimerCount >= 2 ? ShapeState.None : ShapeState.Disabled;

            foreach (ArrowShape arrow in GetArrowsConnectedTo(CompetitionClassState.Intermediate2Passed))
            {
                arrow.State = stateInt2;
            }

            ShapeState stateInt3 = owner.intermediateTimerCount >= 3 ? ShapeState.None : ShapeState.Disabled;

            foreach (ArrowShape arrow in GetArrowsConnectedTo(CompetitionClassState.Intermediate3Passed))
            {
                arrow.State = stateInt3;
            }
        }

        private IEnumerable<ArrowShape> GetArrowsConnectedTo(CompetitionClassState state)
        {
            var shapes = new HashSet<ArrowShape>();

            foreach (ArrowShape incoming in GetIncomingArrowsFor(state))
            {
                shapes.Add(incoming);
            }

            foreach (ArrowShape outgoing in GetOutgoingArrowsFor(state))
            {
                shapes.Add(outgoing);
            }

            return shapes;
        }

        private IEnumerable<ArrowShape> GetIncomingArrowsFor(CompetitionClassState state)
        {
            return from pair in TransitionTable where pair.Key.Item2 == state select pair.Value;
        }

        private IEnumerable<ArrowShape> GetOutgoingArrowsFor(CompetitionClassState state)
        {
            return from pair in TransitionTable where pair.Key.Item1 == state select pair.Value;
        }

        private enum HorizontalAlignment
        {
            Left,
            Center,
            Right
        }

        private enum VerticalAlignment
        {
            Top,
            Middle,
            Bottom
        }
    }

    private sealed class StateBlocks
    {
        private static readonly Dictionary<CompetitionClassState, string> StateNames = new()
        {
            { CompetitionClassState.Offline, "Offline" },
            { CompetitionClassState.SetupCompleted, "Setup completed" },
            { CompetitionClassState.WaitingForSync, "Waiting for clock sync" },
            { CompetitionClassState.ReadyToStart, "Ready to start" },
            { CompetitionClassState.StartPassed, "Start gate passed" },
            { CompetitionClassState.Intermediate1Passed, "Intermediate 1 gate passed" },
            { CompetitionClassState.Intermediate2Passed, "Intermediate 2 gate passed" },
            { CompetitionClassState.Intermediate3Passed, "Intermediate 3 gate passed" },
            { CompetitionClassState.FinishPassed, "Finish gate passed" },
            { CompetitionClassState.RunCompleted, "Run completed" }
        };

        private readonly StateVisualizer owner;

        public Dictionary<CompetitionClassState, TextBlock> Table { get; }

        public StateBlocks(StateVisualizer owner)
        {
            Guard.NotNull(owner, nameof(owner));
            this.owner = owner;

            Table = CreateTextBlockTable();

            foreach (TextBlock block in Table.Values)
            {
                owner.shapes.Add(block);
            }
        }

        private Dictionary<CompetitionClassState, TextBlock> CreateTextBlockTable()
        {
            const int offsetBlockX = 75;
            const int blockWidth = 200;
            const int blockHeight = 30;

            var table = new Dictionary<CompetitionClassState, TextBlock>();

            int offsetX = 0;
            CompetitionClassState[] states = Enum.GetValues(typeof(CompetitionClassState)).Cast<CompetitionClassState>().ToArray();

            foreach (CompetitionClassState state in states)
            {
                var textBlockTopLeft = new PointF(offsetBlockX, offsetX);
                var textBlockSize = new SizeF(blockWidth, blockHeight);

                var textBlock = new TextBlock(StateNames[state], owner.Font, new RectangleF(textBlockTopLeft, textBlockSize));
                table.Add(state, textBlock);

                offsetX += ArrowLength + blockHeight;
            }

            table.Single(x => x.Key == CompetitionClassState.Offline).Value.State = ShapeState.Selected;
            return table;
        }

        public TextBlock GetBlockForState(CompetitionClassState state)
        {
            if (Table.ContainsKey(state))
            {
                return Table[state];
            }

            throw ExceptionFactory.CreateNotSupportedExceptionFor(state);
        }

        public void SetFont(Font value)
        {
            foreach (TextBlock textBlock in Table.Values)
            {
                textBlock.Font = value;
            }
        }

        public void MarkDisabledBlocks()
        {
            TextBlock blockInt1 = GetBlockForState(CompetitionClassState.Intermediate1Passed);
            blockInt1.State = owner.intermediateTimerCount >= 1 ? ShapeState.None : ShapeState.Disabled;

            TextBlock blockInt2 = GetBlockForState(CompetitionClassState.Intermediate2Passed);
            blockInt2.State = owner.intermediateTimerCount >= 2 ? ShapeState.None : ShapeState.Disabled;

            TextBlock blockInt3 = GetBlockForState(CompetitionClassState.Intermediate3Passed);
            blockInt3.State = owner.intermediateTimerCount >= 3 ? ShapeState.None : ShapeState.Disabled;
        }
    }
}
