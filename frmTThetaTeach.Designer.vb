<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTThetaTeach
#Region "Windows フォーム デザイナによって生成されたコード "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'この呼び出しは、Windows フォーム デザイナで必要です。
		InitializeComponent()
		Form_Initialize_renamed()
	End Sub
	'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Windows フォーム デザイナで必要です。
	Private components As System.ComponentModel.IContainer
    Public WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblInfo As System.Windows.Forms.Label
	Public WithEvents lblTitle2 As System.Windows.Forms.Label
	Public WithEvents lblTitle As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTThetaTeach))
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lblTitle2 = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.GrpFram_0 = New System.Windows.Forms.GroupBox()
        Me.LblDisp_5 = New System.Windows.Forms.Label()
        Me.LblDisp_4 = New System.Windows.Forms.Label()
        Me.TxtPosY = New System.Windows.Forms.TextBox()
        Me.TxtPosX = New System.Windows.Forms.TextBox()
        Me.LblDisp_1 = New System.Windows.Forms.Label()
        Me.LblDisp_0 = New System.Windows.Forms.Label()
        Me.GrpFram_1 = New System.Windows.Forms.GroupBox()
        Me.LblDisp_7 = New System.Windows.Forms.Label()
        Me.LblDisp_6 = New System.Windows.Forms.Label()
        Me.TxtPos2Y = New System.Windows.Forms.TextBox()
        Me.TxtPos2X = New System.Windows.Forms.TextBox()
        Me.LblDisp_3 = New System.Windows.Forms.Label()
        Me.LblDisp_2 = New System.Windows.Forms.Label()
        Me.GrpFram_2 = New System.Windows.Forms.GroupBox()
        Me.LblResult_2 = New System.Windows.Forms.Label()
        Me.LblResult_1 = New System.Windows.Forms.Label()
        Me.LblDisp_11 = New System.Windows.Forms.Label()
        Me.LblDisp_9 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GrpFram_0.SuspendLayout()
        Me.GrpFram_1.SuspendLayout()
        Me.GrpFram_2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        resources.ApplyResources(Me.cmdCancel, "cmdCancel")
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        resources.ApplyResources(Me.cmdOK, "cmdOK")
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblInfo
        '
        resources.ApplyResources(Me.lblInfo, "lblInfo")
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInfo.ForeColor = System.Drawing.Color.White
        Me.lblInfo.Name = "lblInfo"
        '
        'lblTitle2
        '
        resources.ApplyResources(Me.lblTitle2, "lblTitle2")
        Me.lblTitle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblTitle2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle2.Name = "lblTitle2"
        '
        'lblTitle
        '
        resources.ApplyResources(Me.lblTitle, "lblTitle")
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Name = "lblTitle"
        '
        'GrpFram_0
        '
        resources.ApplyResources(Me.GrpFram_0, "GrpFram_0")
        Me.GrpFram_0.Controls.Add(Me.LblDisp_5)
        Me.GrpFram_0.Controls.Add(Me.LblDisp_4)
        Me.GrpFram_0.Controls.Add(Me.TxtPosY)
        Me.GrpFram_0.Controls.Add(Me.TxtPosX)
        Me.GrpFram_0.Controls.Add(Me.LblDisp_1)
        Me.GrpFram_0.Controls.Add(Me.LblDisp_0)
        Me.GrpFram_0.Name = "GrpFram_0"
        Me.GrpFram_0.TabStop = False
        '
        'LblDisp_5
        '
        resources.ApplyResources(Me.LblDisp_5, "LblDisp_5")
        Me.LblDisp_5.Name = "LblDisp_5"
        '
        'LblDisp_4
        '
        resources.ApplyResources(Me.LblDisp_4, "LblDisp_4")
        Me.LblDisp_4.Name = "LblDisp_4"
        '
        'TxtPosY
        '
        resources.ApplyResources(Me.TxtPosY, "TxtPosY")
        Me.TxtPosY.Name = "TxtPosY"
        '
        'TxtPosX
        '
        resources.ApplyResources(Me.TxtPosX, "TxtPosX")
        Me.TxtPosX.Name = "TxtPosX"
        '
        'LblDisp_1
        '
        resources.ApplyResources(Me.LblDisp_1, "LblDisp_1")
        Me.LblDisp_1.Name = "LblDisp_1"
        '
        'LblDisp_0
        '
        resources.ApplyResources(Me.LblDisp_0, "LblDisp_0")
        Me.LblDisp_0.Name = "LblDisp_0"
        '
        'GrpFram_1
        '
        resources.ApplyResources(Me.GrpFram_1, "GrpFram_1")
        Me.GrpFram_1.Controls.Add(Me.LblDisp_7)
        Me.GrpFram_1.Controls.Add(Me.LblDisp_6)
        Me.GrpFram_1.Controls.Add(Me.TxtPos2Y)
        Me.GrpFram_1.Controls.Add(Me.TxtPos2X)
        Me.GrpFram_1.Controls.Add(Me.LblDisp_3)
        Me.GrpFram_1.Controls.Add(Me.LblDisp_2)
        Me.GrpFram_1.Name = "GrpFram_1"
        Me.GrpFram_1.TabStop = False
        '
        'LblDisp_7
        '
        resources.ApplyResources(Me.LblDisp_7, "LblDisp_7")
        Me.LblDisp_7.Name = "LblDisp_7"
        '
        'LblDisp_6
        '
        resources.ApplyResources(Me.LblDisp_6, "LblDisp_6")
        Me.LblDisp_6.Name = "LblDisp_6"
        '
        'TxtPos2Y
        '
        resources.ApplyResources(Me.TxtPos2Y, "TxtPos2Y")
        Me.TxtPos2Y.Name = "TxtPos2Y"
        '
        'TxtPos2X
        '
        Me.TxtPos2X.AcceptsTab = True
        resources.ApplyResources(Me.TxtPos2X, "TxtPos2X")
        Me.TxtPos2X.Name = "TxtPos2X"
        '
        'LblDisp_3
        '
        resources.ApplyResources(Me.LblDisp_3, "LblDisp_3")
        Me.LblDisp_3.Name = "LblDisp_3"
        '
        'LblDisp_2
        '
        resources.ApplyResources(Me.LblDisp_2, "LblDisp_2")
        Me.LblDisp_2.Name = "LblDisp_2"
        '
        'GrpFram_2
        '
        resources.ApplyResources(Me.GrpFram_2, "GrpFram_2")
        Me.GrpFram_2.Controls.Add(Me.LblResult_2)
        Me.GrpFram_2.Controls.Add(Me.LblResult_1)
        Me.GrpFram_2.Controls.Add(Me.LblDisp_11)
        Me.GrpFram_2.Controls.Add(Me.LblDisp_9)
        Me.GrpFram_2.Name = "GrpFram_2"
        Me.GrpFram_2.TabStop = False
        '
        'LblResult_2
        '
        resources.ApplyResources(Me.LblResult_2, "LblResult_2")
        Me.LblResult_2.BackColor = System.Drawing.Color.White
        Me.LblResult_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblResult_2.Name = "LblResult_2"
        '
        'LblResult_1
        '
        resources.ApplyResources(Me.LblResult_1, "LblResult_1")
        Me.LblResult_1.BackColor = System.Drawing.Color.White
        Me.LblResult_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblResult_1.Name = "LblResult_1"
        '
        'LblDisp_11
        '
        resources.ApplyResources(Me.LblDisp_11, "LblDisp_11")
        Me.LblDisp_11.Name = "LblDisp_11"
        '
        'LblDisp_9
        '
        resources.ApplyResources(Me.LblDisp_9, "LblDisp_9")
        Me.LblDisp_9.Name = "LblDisp_9"
        '
        'Timer1
        '
        Me.Timer1.Interval = 50
        '
        'frmTThetaTeach
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpFram_2)
        Me.Controls.Add(Me.GrpFram_1)
        Me.Controls.Add(Me.GrpFram_0)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.lblTitle2)
        Me.Controls.Add(Me.lblTitle)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTThetaTeach"
        Me.ShowInTaskbar = False
        Me.GrpFram_0.ResumeLayout(False)
        Me.GrpFram_0.PerformLayout()
        Me.GrpFram_1.ResumeLayout(False)
        Me.GrpFram_1.PerformLayout()
        Me.GrpFram_2.ResumeLayout(False)
        Me.GrpFram_2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpFram_0 As System.Windows.Forms.GroupBox
    Friend WithEvents LblDisp_0 As System.Windows.Forms.Label
    Friend WithEvents TxtPosY As System.Windows.Forms.TextBox
    Friend WithEvents TxtPosX As System.Windows.Forms.TextBox
    Friend WithEvents LblDisp_1 As System.Windows.Forms.Label
    Friend WithEvents GrpFram_1 As System.Windows.Forms.GroupBox
    Friend WithEvents TxtPos2Y As System.Windows.Forms.TextBox
    Friend WithEvents TxtPos2X As System.Windows.Forms.TextBox
    Friend WithEvents LblDisp_3 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_2 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_5 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_4 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_7 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_6 As System.Windows.Forms.Label
    Friend WithEvents GrpFram_2 As System.Windows.Forms.GroupBox
    Friend WithEvents LblDisp_11 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_9 As System.Windows.Forms.Label
    Friend WithEvents LblResult_1 As System.Windows.Forms.Label
    Friend WithEvents LblResult_2 As System.Windows.Forms.Label
#End Region 
End Class