using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using 家畜;

public class frmUpdate : Form
{
	private IContainer components;

	private ProgressBar pbStatus;

	public frmUpdate()
	{
		InitializeComponent();
	}

	[Conditional("DEBUG")]
	private static void DebugPrt(string msg)
	{
		MessageBox.Show(msg);
	}

	private void StartProgram()
	{
		Process.Start("Gts.exe");
	}

	private void frmUpdate_Load(object sender, EventArgs e)
	{
		if (Program.IsDeployClickOnce)
		{
			GoCheckUpdate();
		}
	}

	private void GoCheckUpdate()
	{
		if (NetworkInfo.IsConnectionExist(Program.ClickOnceUpdateDN))
		{
			UpdateProgram();
			return;
		}
		StartProgram();
		Close();
	}

	private void UpdateProgram()
	{
		bool flag = false;
		try
		{
			ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
			if (ApplicationDeployment.CurrentDeployment.CheckForUpdate())
			{
				Program.Counter++;
				currentDeployment.UpdateProgressChanged += obj_UpdateProgressChanged;
				currentDeployment.UpdateCompleted += obj_UpdateCompleted;
				currentDeployment.UpdateAsync();
				flag = true;
			}
		}
		catch (Exception)
		{
			Close();
		}
		if (!flag)
		{
			StartProgram();
			Close();
		}
	}

	private void obj_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
	{
		Program.Counter++;
		pbStatus.Value = e.ProgressPercentage;
		Application.DoEvents();
	}

	private void obj_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
	{
		MessageBox.Show("更新完成，重新啟動程式。");
		Program.Upgraded = true;
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(家畜.frmUpdate));
		pbStatus = new System.Windows.Forms.ProgressBar();
		SuspendLayout();
		pbStatus.Location = new System.Drawing.Point(12, 12);
		pbStatus.Name = "pbStatus";
		pbStatus.Size = new System.Drawing.Size(295, 23);
		pbStatus.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(319, 46);
		base.ControlBox = false;
		base.Controls.Add(pbStatus);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmUpdate";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "程式版本更新中，請稍候";
		base.Load += new System.EventHandler(frmUpdate_Load);
		ResumeLayout(false);
	}
}
