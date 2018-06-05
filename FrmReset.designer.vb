<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReset
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmReset))
        Me.Grp1 = New System.Windows.Forms.GroupBox()
        Me.BtnMg1Down = New System.Windows.Forms.Button()
        Me.BtnOther = New System.Windows.Forms.Button()
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblCaption = New System.Windows.Forms.Label()
        Me.GrpLdAlarm = New System.Windows.Forms.GroupBox()
        Me.TxtExec = New System.Windows.Forms.TextBox()
        Me.ListAlarm = New System.Windows.Forms.ListBox()
        Me.Grp2 = New System.Windows.Forms.GroupBox()
        Me.LblMSG = New System.Windows.Forms.Label()
        Me.BtnHand2VacumeOff = New System.Windows.Forms.Button()
        Me.BtnTableVacumeOff = New System.Windows.Forms.Button()
        Me.BtnTableClampOff = New System.Windows.Forms.Button()
        Me.BtnHandVacumeOff = New System.Windows.Forms.Button()
        Me.Grp1.SuspendLayout()
        Me.GrpLdAlarm.SuspendLayout()
        Me.Grp2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grp1
        '
        Me.Grp1.Controls.Add(Me.BtnMg1Down)
        Me.Grp1.Controls.Add(Me.BtnOther)
        Me.Grp1.Controls.Add(Me.BtnOK)
        Me.Grp1.Controls.Add(Me.BtnCancel)
        Me.Grp1.Controls.Add(Me.Label2)
        Me.Grp1.Controls.Add(Me.Label1)
        Me.Grp1.Controls.Add(Me.LblCaption)
        Me.Grp1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        resources.ApplyResources(Me.Grp1, "Grp1")
        Me.Grp1.Name = "Grp1"
        Me.Grp1.TabStop = False
        '
        'BtnMg1Down
        '
        resources.ApplyResources(Me.BtnMg1Down, "BtnMg1Down")
        Me.BtnMg1Down.Name = "BtnMg1Down"
        Me.BtnMg1Down.UseVisualStyleBackColor = True
        '
        'BtnOther
        '
        resources.ApplyResources(Me.BtnOther, "BtnOther")
        Me.BtnOther.Name = "BtnOther"
        Me.BtnOther.UseVisualStyleBackColor = True
        '
        'BtnOK
        '
        resources.ApplyResources(Me.BtnOK, "BtnOK")
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        resources.ApplyResources(Me.BtnCancel, "BtnCancel")
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'LblCaption
        '
        Me.LblCaption.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.LblCaption, "LblCaption")
        Me.LblCaption.Name = "LblCaption"
        '
        'GrpLdAlarm
        '
        Me.GrpLdAlarm.Controls.Add(Me.TxtExec)
        Me.GrpLdAlarm.Controls.Add(Me.ListAlarm)
        resources.ApplyResources(Me.GrpLdAlarm, "GrpLdAlarm")
        Me.GrpLdAlarm.ForeColor = System.Drawing.Color.Red
        Me.GrpLdAlarm.Name = "GrpLdAlarm"
        Me.GrpLdAlarm.TabStop = False
        '
        'TxtExec
        '
        Me.TxtExec.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.TxtExec, "TxtExec")
        Me.TxtExec.Name = "TxtExec"
        Me.TxtExec.ReadOnly = True
        '
        'ListAlarm
        '
        Me.ListAlarm.FormattingEnabled = True
        resources.ApplyResources(Me.ListAlarm, "ListAlarm")
        Me.ListAlarm.Name = "ListAlarm"
        '
        'Grp2
        '
        Me.Grp2.Controls.Add(Me.LblMSG)
        Me.Grp2.Controls.Add(Me.BtnHand2VacumeOff)
        Me.Grp2.Controls.Add(Me.BtnTableVacumeOff)
        Me.Grp2.Controls.Add(Me.BtnTableClampOff)
        Me.Grp2.Controls.Add(Me.BtnHandVacumeOff)
        resources.ApplyResources(Me.Grp2, "Grp2")
        Me.Grp2.Name = "Grp2"
        Me.Grp2.TabStop = False
        '
        'LblMSG
        '
        Me.LblMSG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblMSG, "LblMSG")
        Me.LblMSG.ForeColor = System.Drawing.Color.Blue
        Me.LblMSG.Name = "LblMSG"
        '
        'BtnHand2VacumeOff
        '
        Me.BtnHand2VacumeOff.BackColor = System.Drawing.Color.GreenYellow
        resources.ApplyResources(Me.BtnHand2VacumeOff, "BtnHand2VacumeOff")
        Me.BtnHand2VacumeOff.Name = "BtnHand2VacumeOff"
        Me.BtnHand2VacumeOff.UseVisualStyleBackColor = False
        '
        'BtnTableVacumeOff
        '
        Me.BtnTableVacumeOff.BackColor = System.Drawing.Color.SpringGreen
        resources.ApplyResources(Me.BtnTableVacumeOff, "BtnTableVacumeOff")
        Me.BtnTableVacumeOff.Name = "BtnTableVacumeOff"
        Me.BtnTableVacumeOff.UseVisualStyleBackColor = False
        '
        'BtnTableClampOff
        '
        Me.BtnTableClampOff.BackColor = System.Drawing.Color.SpringGreen
        resources.ApplyResources(Me.BtnTableClampOff, "BtnTableClampOff")
        Me.BtnTableClampOff.Name = "BtnTableClampOff"
        Me.BtnTableClampOff.UseVisualStyleBackColor = False
        '
        'BtnHandVacumeOff
        '
        Me.BtnHandVacumeOff.BackColor = System.Drawing.Color.GreenYellow
        resources.ApplyResources(Me.BtnHandVacumeOff, "BtnHandVacumeOff")
        Me.BtnHandVacumeOff.Name = "BtnHandVacumeOff"
        Me.BtnHandVacumeOff.UseVisualStyleBackColor = False
        '
        'FrmReset
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.Grp2)
        Me.Controls.Add(Me.GrpLdAlarm)
        Me.Controls.Add(Me.Grp1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmReset"
        Me.TopMost = True
        Me.Grp1.ResumeLayout(False)
        Me.GrpLdAlarm.ResumeLayout(False)
        Me.GrpLdAlarm.PerformLayout()
        Me.Grp2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grp1 As System.Windows.Forms.GroupBox
    Friend WithEvents LblCaption As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GrpLdAlarm As System.Windows.Forms.GroupBox
    Friend WithEvents ListAlarm As System.Windows.Forms.ListBox
    Friend WithEvents TxtExec As System.Windows.Forms.TextBox
    Friend WithEvents BtnOK As System.Windows.Forms.Button
    Friend WithEvents Grp2 As System.Windows.Forms.GroupBox
    Friend WithEvents BtnTableVacumeOff As System.Windows.Forms.Button
    Friend WithEvents BtnTableClampOff As System.Windows.Forms.Button
    Friend WithEvents BtnHandVacumeOff As System.Windows.Forms.Button
    Friend WithEvents BtnHand2VacumeOff As System.Windows.Forms.Button
    Friend WithEvents LblMSG As System.Windows.Forms.Label
    Friend WithEvents BtnOther As System.Windows.Forms.Button
    Friend WithEvents BtnMg1Down As System.Windows.Forms.Button
End Class
