using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Mediator;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.MediatorEmulator.Engine;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms;

/// <summary>
/// The MDI parent that hosts all devices in the emulated wireless network.
/// </summary>
public sealed partial class EmulatorForm : Form
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly StartupArguments startupArguments;
    private readonly DisposableComponent<CirceMediatorSessionManager> sessionManager;
    private readonly RandomSettingsGenerator settingsGenerator = new();
    private readonly MostRecentlyUsedContainer mruContainer = RegistrySettingsProvider.GetMruList();

    private NetworkConfigurationFile file = new();

    // Justification for nullable suppression: This value is assigned during EmulatorForm_Load.
    private MediatorForm mediatorForm = null!;

    public EmulatorForm(StartupArguments startupArguments)
    {
        this.startupArguments = startupArguments;
        InitializeComponent();

        Text += AssemblyReader.GetInformationalVersion();

        sessionManager = new DisposableComponent<CirceMediatorSessionManager>(new CirceMediatorSessionManager(), ref components);
    }

    private void EmulatorForm_Load(object? sender, EventArgs e)
    {
        ApplyLayoutFromStartupArguments();

        CreateMdiChildrenFromFile();

        if (startupArguments.Path != null)
        {
            OpenFromFile(startupArguments.Path);
        }
    }

    private void ApplyLayoutFromStartupArguments()
    {
        if (startupArguments.HasLayout)
        {
            if (startupArguments.Location != null)
            {
                Location = startupArguments.Location.Value;
            }

            if (startupArguments.Size != null)
            {
                Size = startupArguments.Size.Value;
            }

            if (startupArguments.TransparentOnTop)
            {
                Opacity = 0.9;
                TopMost = true;
            }

            WindowState = startupArguments.State;
        }
        else
        {
            MaximizeOnSecondaryScreen();
        }
    }

    private void MaximizeOnSecondaryScreen()
    {
        Screen? secondaryScreen = Screen.AllScreens.FirstOrDefault(s => !s.Primary);

        if (secondaryScreen != null)
        {
            StartPosition = FormStartPosition.Manual;
            Location = secondaryScreen.Bounds.Location;
        }

        WindowState = FormWindowState.Maximized;
    }

    private void CreateMdiChildrenFromFile()
    {
        CreateMediatorForm(file.Configuration.MediatorOrDefault);

        foreach (GateSettingsXml gateSettings in file.Configuration.GatesOrEmpty)
        {
            CreateGateForm(gateSettings);
        }

        foreach (RemoteSettingsXml remoteSettings in file.Configuration.RemotesOrEmpty)
        {
            CreateRemoteForm(remoteSettings);
        }

        foreach (DisplaySettingsXml displaySettings in file.Configuration.DisplaysOrEmpty)
        {
            CreateDisplayForm(displaySettings);
        }
    }

    private void CreateMediatorForm(MediatorSettingsXml settings)
    {
        mediatorForm = new MediatorForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
        {
            MdiParent = this
        };

        mediatorForm.WindowStateChanging += MdiFormOnWindowStateChanging;
        mediatorForm.Show();
        Log.Info("Added Mediator.");
    }

    private void MdiFormOnWindowStateChanging(object? sender, WindowStateChangingEventArgs e)
    {
        if (sender is Form { WindowState: FormWindowState.Maximized })
        {
            file.Configuration.IsMaximized = false;
        }
        else if (e.NewState == FormWindowState.Maximized)
        {
            file.Configuration.IsMaximized = true;
        }
    }

    private void CreateGateForm(GateSettingsXml settings)
    {
        var form = new GateForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
        {
            MdiParent = this
        };

        form.DeviceRemoved += DeviceFormOnDeviceRemoved;
        form.WindowStateChanging += MdiFormOnWindowStateChanging;
        form.Show();
        Log.Info($"Added Gate {settings.DeviceAddressNotNull}.");
    }

    private void DeviceFormOnDeviceRemoved(object? sender, EventArgs<WirelessNetworkAddress> e)
    {
        file.Configuration.RemoveDevice(e.Argument);
        Log.Info($"Removed {e.Argument}.");
    }

    private void CreateRemoteForm(RemoteSettingsXml settings)
    {
        var form = new RemoteForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
        {
            MdiParent = this
        };

        form.DeviceRemoved += DeviceFormOnDeviceRemoved;
        form.WindowStateChanging += MdiFormOnWindowStateChanging;
        form.Show();
        Log.Info($"Added Remote {settings.DeviceAddressNotNull}.");
    }

    private void CreateDisplayForm(DisplaySettingsXml settings)
    {
        var form = new DisplayForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
        {
            MdiParent = this
        };

        form.DeviceRemoved += DeviceFormOnDeviceRemoved;
        form.WindowStateChanging += MdiFormOnWindowStateChanging;
        form.Show();
        Log.Info($"Added Display {settings.DeviceAddressNotNull}.");
    }

    private void GateToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        var settings = new GateSettingsXml
        {
            SignalStrength = settingsGenerator.GetSignalStrength(),
            BatteryStatus = settingsGenerator.GetBatteryStatus()
        };

        file.Configuration.GatesOrEmpty.Add(settings);
        CreateGateForm(settings);
    }

    private void RemoteToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        var settings = new RemoteSettingsXml
        {
            SignalStrength = settingsGenerator.GetSignalStrength(),
            BatteryStatus = settingsGenerator.GetBatteryStatus(),
            Features = RemoteEmulatorFeatures.All
        };

        file.Configuration.RemotesOrEmpty.Add(settings);
        CreateRemoteForm(settings);
    }

    private void DisplayToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        var settings = new DisplaySettingsXml
        {
            SignalStrength = settingsGenerator.GetSignalStrength(),
            BatteryStatus = settingsGenerator.GetBatteryStatus()
        };

        file.Configuration.DisplaysOrEmpty.Add(settings);
        CreateDisplayForm(settings);
    }

    private void OpenToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Open network configuration",
            Filter = "Network Configuration Files (*.xml)|*.xml|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            OpenFromFile(dialog.FileName);
        }
    }

    private bool OpenFromFile(string path)
    {
        Guard.NotNullNorEmpty(path, nameof(path));

        NetworkConfigurationFile newFile;

        try
        {
            newFile = NetworkConfigurationFile.Load(path);
        }
        catch (Exception ex)
        {
            string message = $"Failed to load network configuration from file:\n\n{path}\n\n{ex.Message}";
            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        CloseMdiChildren(true);
        file = newFile;
        CreateMdiChildrenFromFile();

        mruContainer.MarkAsUsed(Path.GetFullPath(path));
        return true;
    }

    private void CloseMdiChildren(bool includeMediator)
    {
        if (includeMediator)
        {
            mediatorForm.CloseWithOverride();
            Log.Info("Removed Mediator.");
        }

        Form[] childrenNotMediator = MdiChildren.Where(form => form != mediatorForm).ToArray();

        foreach (Form child in childrenNotMediator)
        {
            child.Close();
        }
    }

    private void SaveToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (file.FilePath == null)
        {
            SaveAsToolStripMenuItem_Click(this, EventArgs.Empty);
        }
        else
        {
            SaveFile(file.FilePath);
        }
    }

    private void SaveAsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Save network configuration",
            Filter = "Network Configuration Files (*.xml)|*.xml|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            SaveFile(dialog.FileName);
        }
    }

    private void SaveFile(string path)
    {
        string absolutePath = Path.GetFullPath(path);

        file.Configuration.IsMaximized = MdiChildren.Any(child => child.WindowState == FormWindowState.Maximized);
        mruContainer.MarkAsUsed(absolutePath);
        file.SaveAs(absolutePath);
    }

    private void CascadeToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.Cascade);
    }

    private void TileHorizontalToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileHorizontal);
    }

    private void TileVerticalToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileVertical);
    }

    private void ArrangeToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.ArrangeIcons);
    }

    private void MediatorToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        mediatorForm.BringToFront();
    }

    private void MinimizeAllWindowsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        foreach (Form child in MdiChildren)
        {
            child.WindowState = FormWindowState.Minimized;
        }
    }

    private void CloseAllWindowsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        CloseMdiChildren(false);
    }

    private void FileToolStripMenuItem_DropDownOpening(object? sender, EventArgs e)
    {
        foreach (ToolStripItem mruMenuItem in recentFilesToolStripMenuItem.DropDownItems)
        {
            mruMenuItem.Click -= MruMenuItemOnClick;
        }

        recentFilesToolStripMenuItem.DropDownItems.Clear();

        foreach (string path in mruContainer.Items)
        {
            var mruMenuItem = new ToolStripMenuItem(Path.GetFileName(path))
            {
                Tag = path
            };

            mruMenuItem.Click += MruMenuItemOnClick;

            recentFilesToolStripMenuItem.DropDownItems.Add(mruMenuItem);
        }

        recentFilesToolStripMenuItem.Enabled = mruContainer.Items.Count > 0;
    }

    private void MruMenuItemOnClick(object? sender, EventArgs eventArgs)
    {
        if (sender is ToolStripMenuItem mruMenuItem)
        {
            string path = (string)mruMenuItem.Tag;

            if (!OpenFromFile(path))
            {
                mruContainer.Remove(path);
            }
        }
    }

    private void EmulatorForm_FormClosing(object? sender, EventArgs eventArgs)
    {
        RegistrySettingsProvider.SaveMruList(mruContainer);
    }
}
