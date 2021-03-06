﻿// ForwarderTab.cs in bukkitgui2/bukkitgui2
// Created 2014/01/17
// Last edited at 2016/05/15 21:26
// ©Bertware, visit http://bertware.net

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using Net.Bertware.Bukkitgui2.Core;

namespace Net.Bertware.Bukkitgui2.AddOn.Forwarder
{
	public partial class ForwarderTab : MetroUserControl, IAddonTab
	{
		public ForwarderTab()
		{
			InitializeComponent();
			UPnP.MappingUpdateReceived += Displaymapping;
			UPnP.PortForwardApplied += UpdateMappingAsync;
			Load += PortForwarder_Load;
		}

		public IAddon ParentAddon { get; set; }


		private void PortForwarder_Load(object sender, EventArgs e)
		{
			//if (!UPnP.UpnpEnabled)
			//{
			//    MetroMessageBox.Show(Application.OpenForms[0],
			//        Locale.Tr("Port forwarding isn't available") + Constants.vbCrLf +
			//        Locale.Tr("Your network device (router) doesn't support UPnP"),
			//        Locale.Tr("Port forwarding unavailable"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}

			TxtIp.Text = UPnP.LocalIp();
			CBProtocol.SelectedIndex = 0;

			UpdateMappingAsync();
		}


		private void UpdateMappingAsync()
		{
			if (InvokeRequired)
			{
				Invoke((MethodInvoker) (UpdateMappingAsync));
			}
			else
			{
				BtnAdd.Enabled = false;
				BtnRefresh.Enabled = false;
				UPnP.GetMapping();

				// this.lblStatus.Text = "Loading info. This could take a while...";
			}
		}

		private void Displaymapping(List<PortMappingEntry> mapping)
		{
			if (InvokeRequired)
			{
				Invoke((MethodInvoker) (() => Displaymapping(mapping)));
			}
			else
			{
				slvPortMappings.Items.Clear();
				foreach (PortMappingEntry entry in mapping)
				{
					string[] content =
					{
						entry.Name,
						entry.Ip,
						entry.Port.ToString(),
						entry.Protocol.ToString()
					};
					ListViewItem lvi = new ListViewItem(content
						) {Tag = entry};
					slvPortMappings.Items.Add(lvi);
				}
				BtnAdd.Enabled = true;
				BtnRefresh.Enabled = true;
				Cursor = Cursors.Default;
				// this.lblStatus.Text = "idle";
			}
		}


		private void BtnAdd_Click(object sender, EventArgs e)
		{
			UPnP.Protocol protocol;
			if (CBProtocol.SelectedIndex == 1)
				protocol = UPnP.Protocol.UDP;
			else
				protocol = UPnP.Protocol.TCP;
			if (!Regex.IsMatch(TxtIp.Text, "(\\d{1,3}\\.){3}\\d{1,3}"))
			{
				MetroMessageBox.Show(Application.OpenForms[0],
					Locale.Tr(
						"The IP address you entered is invalid. It should be something like for example 192.168.1.2"),
					Locale.Tr("Invalid inpunt"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			UPnP.Forward(TxtName.Text, Convert.ToUInt32(NumPort.Value), TxtIp.Text, protocol);
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (slvPortMappings.SelectedItems.Count < 1) return;
			PortMappingEntry entry = (PortMappingEntry) slvPortMappings.SelectedItems[0].Tag;
			try
			{
				entry.RemoveFromMapping();
			}
			catch
			{
				MetroMessageBox.Show(this, "Error during rule removal",
					"This rule couldn't be removed. Try removing it from the router's webinterface.");
			}
		}
	}
}