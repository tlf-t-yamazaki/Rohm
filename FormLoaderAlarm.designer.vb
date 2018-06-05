<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormLoaderAlarm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormLoaderAlarm))
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.LblAlarmList = New System.Windows.Forms.Label()
        Me.LblExec = New System.Windows.Forms.Label()
        Me.LblKind = New System.Windows.Forms.Label()
        Me.ListAlarm = New System.Windows.Forms.ListBox()
        Me.TxtExec = New System.Windows.Forms.TextBox()
        Me.BtnBZOff = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnHandVacumeOff = New System.Windows.Forms.Button()
        Me.BtnTableClampOff = New System.Windows.Forms.Button()
        Me.BtnTableVacumeOff = New System.Windows.Forms.Button()
        Me.LblMSG = New System.Windows.Forms.Label()
        Me.BtnHand2VacumeOff = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnOK
        '
        resources.ApplyResources(Me.BtnOK, "BtnOK")
        Me.BtnOK.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.UseVisualStyleBackColor = False
        '
        'LblAlarmList
        '
        resources.ApplyResources(Me.LblAlarmList, "LblAlarmList")
        Me.LblAlarmList.Name = "LblAlarmList"
        '
        'LblExec
        '
        resources.ApplyResources(Me.LblExec, "LblExec")
        Me.LblExec.Name = "LblExec"
        '
        'LblKind
        '
        resources.ApplyResources(Me.LblKind, "LblKind")
        Me.LblKind.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblKind.Name = "LblKind"
        '
        'ListAlarm
        '
        resources.ApplyResources(Me.ListAlarm, "ListAlarm")
        Me.ListAlarm.FormattingEnabled = True
        Me.ListAlarm.Name = "ListAlarm"
        '
        'TxtExec
        '
        resources.ApplyResources(Me.TxtExec, "TxtExec")
        Me.TxtExec.BackColor = System.Drawing.Color.White
        Me.TxtExec.Name = "TxtExec"
        Me.TxtExec.ReadOnly = True
        '
        'BtnBZOff
        '
        resources.ApplyResources(Me.BtnBZOff, "BtnBZOff")
        Me.BtnBZOff.BackColor = System.Drawing.Color.IndianRed
        Me.BtnBZOff.Name = "BtnBZOff"
        Me.BtnBZOff.UseVisualStyleBackColor = False
        '
        'BtnCancel
        '
        resources.ApplyResources(Me.BtnCancel, "BtnCancel")
        Me.BtnCancel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'BtnHandVacumeOff
        '
        resources.ApplyResources(Me.BtnHandVacumeOff, "BtnHandVacumeOff")
        Me.BtnHandVacumeOff.BackColor = System.Drawing.Color.GreenYellow
        Me.BtnHandVacumeOff.Name = "BtnHandVacumeOff"
        Me.BtnHandVacumeOff.UseVisualStyleBackColor = False
        '
        'BtnTableClampOff
        '
        resources.ApplyResources(Me.BtnTableClampOff, "BtnTableClampOff")
        Me.BtnTableClampOff.BackColor = System.Drawing.Color.SpringGreen
        Me.BtnTableClampOff.Name = "BtnTableClampOff"
        Me.BtnTableClampOff.UseVisualStyleBackColor = False
        '
        'BtnTableVacumeOff
        '
        resources.ApplyResources(Me.BtnTableVacumeOff, "BtnTableVacumeOff")
        Me.BtnTableVacumeOff.BackColor = System.Drawing.Color.SpringGreen
        Me.BtnTableVacumeOff.Name = "BtnTableVacumeOff"
        Me.BtnTableVacumeOff.UseVisualStyleBackColor = False
        '
        'LblMSG
        '
        resources.ApplyResources(Me.LblMSG, "LblMSG")
        Me.LblMSG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblMSG.ForeColor = System.Drawing.Color.Blue
        Me.LblMSG.Name = "LblMSG"
        '
        'BtnHand2VacumeOff
        '
        resources.ApplyResources(Me.BtnHand2VacumeOff, "BtnHand2VacumeOff")
        Me.BtnHand2VacumeOff.BackColor = System.Drawing.Color.GreenYellow
        Me.BtnHand2VacumeOff.Name = "BtnHand2VacumeOff"
        Me.BtnHand2VacumeOff.UseVisualStyleBackColor = False
        '
        'FormLoaderAlarm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ControlBox = False
        Me.Controls.Add(Me.BtnHand2VacumeOff)
        Me.Controls.Add(Me.LblMSG)
        Me.Controls.Add(Me.BtnTableVacumeOff)
        Me.Controls.Add(Me.BtnTableClampOff)
        Me.Controls.Add(Me.BtnHandVacumeOff)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnBZOff)
        Me.Controls.Add(Me.TxtExec)
        Me.Controls.Add(Me.ListAlarm)
        Me.Controls.Add(Me.LblKind)
        Me.Controls.Add(Me.LblExec)
        Me.Controls.Add(Me.LblAlarmList)
        Me.Controls.Add(Me.BtnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormLoaderAlarm"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnOK As System.Windows.Forms.Button
    Friend WithEvents LblAlarmList As System.Windows.Forms.Label
    Friend WithEvents LblExec As System.Windows.Forms.Label
    Friend WithEvents LblKind As System.Windows.Forms.Label
    Friend WithEvents ListAlarm As System.Windows.Forms.ListBox
    Friend WithEvents TxtExec As System.Windows.Forms.TextBox
    Friend WithEvents BtnBZOff As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnHandVacumeOff As System.Windows.Forms.Button
    Friend WithEvents BtnTableClampOff As System.Windows.Forms.Button
    Friend WithEvents BtnTableVacumeOff As System.Windows.Forms.Button
    Friend WithEvents LblMSG As System.Windows.Forms.Label
    Friend WithEvents BtnHand2VacumeOff As System.Windows.Forms.Button
End Class
