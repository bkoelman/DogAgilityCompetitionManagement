using System;
using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    /// <summary>
    /// Enables configuration of network status for emulated wireless devices, such as logical network membership and assigned roles.
    /// </summary>
    public sealed partial class NetworkStatusControl : UserControl
    {
        private DeviceRoles rolesAssigned = DeviceRoles.None;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInNetwork
        {
            get => isInNetworkCheckBox.Checked;
            set
            {
                if (value != IsInNetwork)
                {
                    isInNetworkCheckBox.Checked = value;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DeviceRoles RolesAssigned
        {
            get => rolesAssigned;
            set
            {
                if (value != RolesAssigned)
                {
                    rolesAssigned = value;
                    rolesLabel.Text = "Roles: " + rolesAssigned;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? StatusChanged;

        public NetworkStatusControl()
        {
            InitializeComponent();
        }

        private void IsInNetworkCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
