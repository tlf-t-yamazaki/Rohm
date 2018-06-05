<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTxTyTeach
#Region "Windows フォーム デザイナによって生成されたコード "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
        Form_Initialize_Renamed()
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
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTxTyTeach))
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.GrpArrow = New System.Windows.Forms.GroupBox()
        Me.BtnJOG_6 = New System.Windows.Forms.Button()
        Me.BtnJOG_5 = New System.Windows.Forms.Button()
        Me.BtnJOG_7 = New System.Windows.Forms.Button()
        Me.BtnJOG_4 = New System.Windows.Forms.Button()
        Me.BtnHI = New System.Windows.Forms.Button()
        Me.BtnJOG_3 = New System.Windows.Forms.Button()
        Me.BtnJOG_2 = New System.Windows.Forms.Button()
        Me.BtnJOG_1 = New System.Windows.Forms.Button()
        Me.BtnJOG_0 = New System.Windows.Forms.Button()
        Me.BtnZ = New System.Windows.Forms.Button()
        Me.BtnRESET = New System.Windows.Forms.Button()
        Me.BtnSTART = New System.Windows.Forms.Button()
        Me.BtnHALT = New System.Windows.Forms.Button()
        Me.GrpPithPanel = New System.Windows.Forms.GroupBox()
        Me.TBarPause = New System.Windows.Forms.TrackBar()
        Me.TBarHiPitch = New System.Windows.Forms.TrackBar()
        Me.TBarLowPitch = New System.Windows.Forms.TrackBar()
        Me.LblTchMoval2 = New System.Windows.Forms.Label()
        Me.LblTchMoval1 = New System.Windows.Forms.Label()
        Me.LblTchMoval0 = New System.Windows.Forms.Label()
        Me.LblPitch2 = New System.Windows.Forms.Label()
        Me.LblPitch1 = New System.Windows.Forms.Label()
        Me.LblPitch0 = New System.Windows.Forms.Label()
        Me.GrpMain = New System.Windows.Forms.GroupBox()
        Me.GrpFram_2 = New System.Windows.Forms.GroupBox()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.LblResult_2 = New System.Windows.Forms.Label()
        Me.LblResult_1 = New System.Windows.Forms.Label()
        Me.LblResult_0 = New System.Windows.Forms.Label()
        Me.LblDisp_10 = New System.Windows.Forms.Label()
        Me.LblDisp_9 = New System.Windows.Forms.Label()
        Me.LblDisp_11 = New System.Windows.Forms.Label()
        Me.LblDisp_8 = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.GrpFram_1 = New System.Windows.Forms.GroupBox()
        Me.LblDisp_7 = New System.Windows.Forms.Label()
        Me.LblDisp_6 = New System.Windows.Forms.Label()
        Me.TxtPos2Y = New System.Windows.Forms.TextBox()
        Me.TxtPos2X = New System.Windows.Forms.TextBox()
        Me.LblDisp_3 = New System.Windows.Forms.Label()
        Me.LblDisp_2 = New System.Windows.Forms.Label()
        Me.lblTitle2 = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.GrpFram_0 = New System.Windows.Forms.GroupBox()
        Me.LblDisp_5 = New System.Windows.Forms.Label()
        Me.LblDisp_4 = New System.Windows.Forms.Label()
        Me.TxtPosY = New System.Windows.Forms.TextBox()
        Me.TxtPosX = New System.Windows.Forms.TextBox()
        Me.LblDisp_1 = New System.Windows.Forms.Label()
        Me.LblDisp_0 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GrpArrow.SuspendLayout()
        Me.GrpPithPanel.SuspendLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpMain.SuspendLayout()
        Me.GrpFram_2.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpFram_1.SuspendLayout()
        Me.GrpFram_0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCancel, "cmdCancel")
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdOK, "cmdOK")
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.TabStop = False
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'GrpArrow
        '
        Me.GrpArrow.Controls.Add(Me.BtnJOG_6)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_5)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_7)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_4)
        Me.GrpArrow.Controls.Add(Me.BtnHI)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_3)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_2)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_1)
        Me.GrpArrow.Controls.Add(Me.BtnJOG_0)
        Me.GrpArrow.Controls.Add(Me.BtnZ)
        Me.GrpArrow.Controls.Add(Me.BtnRESET)
        Me.GrpArrow.Controls.Add(Me.BtnSTART)
        Me.GrpArrow.Controls.Add(Me.BtnHALT)
        Me.GrpArrow.Controls.Add(Me.GrpPithPanel)
        resources.ApplyResources(Me.GrpArrow, "GrpArrow")
        Me.GrpArrow.Name = "GrpArrow"
        Me.GrpArrow.TabStop = False
        '
        'BtnJOG_6
        '
        Me.BtnJOG_6.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_6.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_6.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_6, "BtnJOG_6")
        Me.BtnJOG_6.Name = "BtnJOG_6"
        Me.BtnJOG_6.TabStop = False
        Me.BtnJOG_6.UseVisualStyleBackColor = False
        '
        'BtnJOG_5
        '
        Me.BtnJOG_5.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_5.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_5.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_5, "BtnJOG_5")
        Me.BtnJOG_5.Name = "BtnJOG_5"
        Me.BtnJOG_5.TabStop = False
        Me.BtnJOG_5.UseVisualStyleBackColor = False
        '
        'BtnJOG_7
        '
        Me.BtnJOG_7.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_7.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_7.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_7, "BtnJOG_7")
        Me.BtnJOG_7.Name = "BtnJOG_7"
        Me.BtnJOG_7.TabStop = False
        Me.BtnJOG_7.UseVisualStyleBackColor = False
        '
        'BtnJOG_4
        '
        Me.BtnJOG_4.AllowDrop = True
        Me.BtnJOG_4.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_4.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_4.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_4, "BtnJOG_4")
        Me.BtnJOG_4.Name = "BtnJOG_4"
        Me.BtnJOG_4.TabStop = False
        Me.BtnJOG_4.UseVisualStyleBackColor = False
        '
        'BtnHI
        '
        resources.ApplyResources(Me.BtnHI, "BtnHI")
        Me.BtnHI.BackColor = System.Drawing.SystemColors.Control
        Me.BtnHI.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnHI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnHI.Name = "BtnHI"
        Me.BtnHI.TabStop = False
        Me.BtnHI.UseVisualStyleBackColor = False
        '
        'BtnJOG_3
        '
        Me.BtnJOG_3.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_3.CausesValidation = False
        Me.BtnJOG_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_3.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_3, "BtnJOG_3")
        Me.BtnJOG_3.Name = "BtnJOG_3"
        Me.BtnJOG_3.TabStop = False
        Me.BtnJOG_3.UseVisualStyleBackColor = False
        '
        'BtnJOG_2
        '
        Me.BtnJOG_2.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_2.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_2, "BtnJOG_2")
        Me.BtnJOG_2.Name = "BtnJOG_2"
        Me.BtnJOG_2.TabStop = False
        Me.BtnJOG_2.UseVisualStyleBackColor = False
        '
        'BtnJOG_1
        '
        Me.BtnJOG_1.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.BtnJOG_1.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_1.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_1, "BtnJOG_1")
        Me.BtnJOG_1.Name = "BtnJOG_1"
        Me.BtnJOG_1.TabStop = False
        Me.BtnJOG_1.UseVisualStyleBackColor = False
        '
        'BtnJOG_0
        '
        Me.BtnJOG_0.BackColor = System.Drawing.SystemColors.Control
        Me.BtnJOG_0.Cursor = System.Windows.Forms.Cursors.Default
        Me.BtnJOG_0.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.BtnJOG_0, "BtnJOG_0")
        Me.BtnJOG_0.Name = "BtnJOG_0"
        Me.BtnJOG_0.TabStop = False
        Me.BtnJOG_0.UseVisualStyleBackColor = False
        '
        'BtnZ
        '
        Me.BtnZ.BackColor = System.Drawing.SystemColors.Control
        Me.BtnZ.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnZ, "BtnZ")
        Me.BtnZ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnZ.Name = "BtnZ"
        Me.BtnZ.TabStop = False
        Me.BtnZ.UseVisualStyleBackColor = False
        '
        'BtnRESET
        '
        Me.BtnRESET.BackColor = System.Drawing.SystemColors.Control
        Me.BtnRESET.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnRESET, "BtnRESET")
        Me.BtnRESET.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnRESET.Name = "BtnRESET"
        Me.BtnRESET.TabStop = False
        Me.BtnRESET.UseVisualStyleBackColor = False
        '
        'BtnSTART
        '
        Me.BtnSTART.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.BtnSTART.BackColor = System.Drawing.SystemColors.Control
        Me.BtnSTART.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnSTART, "BtnSTART")
        Me.BtnSTART.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnSTART.Name = "BtnSTART"
        Me.BtnSTART.TabStop = False
        Me.BtnSTART.UseVisualStyleBackColor = False
        '
        'BtnHALT
        '
        Me.BtnHALT.BackColor = System.Drawing.SystemColors.Control
        Me.BtnHALT.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.BtnHALT, "BtnHALT")
        Me.BtnHALT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BtnHALT.Name = "BtnHALT"
        Me.BtnHALT.TabStop = False
        Me.BtnHALT.UseVisualStyleBackColor = False
        '
        'GrpPithPanel
        '
        Me.GrpPithPanel.BackColor = System.Drawing.SystemColors.Control
        Me.GrpPithPanel.Controls.Add(Me.TBarPause)
        Me.GrpPithPanel.Controls.Add(Me.TBarHiPitch)
        Me.GrpPithPanel.Controls.Add(Me.TBarLowPitch)
        Me.GrpPithPanel.Controls.Add(Me.LblTchMoval2)
        Me.GrpPithPanel.Controls.Add(Me.LblTchMoval1)
        Me.GrpPithPanel.Controls.Add(Me.LblTchMoval0)
        Me.GrpPithPanel.Controls.Add(Me.LblPitch2)
        Me.GrpPithPanel.Controls.Add(Me.LblPitch1)
        Me.GrpPithPanel.Controls.Add(Me.LblPitch0)
        resources.ApplyResources(Me.GrpPithPanel, "GrpPithPanel")
        Me.GrpPithPanel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GrpPithPanel.Name = "GrpPithPanel"
        Me.GrpPithPanel.TabStop = False
        '
        'TBarPause
        '
        resources.ApplyResources(Me.TBarPause, "TBarPause")
        Me.TBarPause.Name = "TBarPause"
        Me.TBarPause.TabStop = False
        '
        'TBarHiPitch
        '
        Me.TBarHiPitch.AllowDrop = True
        resources.ApplyResources(Me.TBarHiPitch, "TBarHiPitch")
        Me.TBarHiPitch.Name = "TBarHiPitch"
        Me.TBarHiPitch.TabStop = False
        '
        'TBarLowPitch
        '
        resources.ApplyResources(Me.TBarLowPitch, "TBarLowPitch")
        Me.TBarLowPitch.Name = "TBarLowPitch"
        Me.TBarLowPitch.TabStop = False
        '
        'LblTchMoval2
        '
        Me.LblTchMoval2.BackColor = System.Drawing.Color.Transparent
        Me.LblTchMoval2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblTchMoval2, "LblTchMoval2")
        Me.LblTchMoval2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblTchMoval2.Name = "LblTchMoval2"
        '
        'LblTchMoval1
        '
        Me.LblTchMoval1.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.LblTchMoval1.BackColor = System.Drawing.Color.Transparent
        Me.LblTchMoval1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblTchMoval1, "LblTchMoval1")
        Me.LblTchMoval1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblTchMoval1.Name = "LblTchMoval1"
        '
        'LblTchMoval0
        '
        Me.LblTchMoval0.AllowDrop = True
        Me.LblTchMoval0.BackColor = System.Drawing.Color.Transparent
        Me.LblTchMoval0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblTchMoval0, "LblTchMoval0")
        Me.LblTchMoval0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblTchMoval0.Name = "LblTchMoval0"
        '
        'LblPitch2
        '
        Me.LblPitch2.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.LblPitch2, "LblPitch2")
        Me.LblPitch2.BackColor = System.Drawing.SystemColors.Control
        Me.LblPitch2.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblPitch2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblPitch2.Name = "LblPitch2"
        '
        'LblPitch1
        '
        resources.ApplyResources(Me.LblPitch1, "LblPitch1")
        Me.LblPitch1.BackColor = System.Drawing.SystemColors.Control
        Me.LblPitch1.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblPitch1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblPitch1.Name = "LblPitch1"
        '
        'LblPitch0
        '
        Me.LblPitch0.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.LblPitch0, "LblPitch0")
        Me.LblPitch0.BackColor = System.Drawing.SystemColors.Control
        Me.LblPitch0.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblPitch0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblPitch0.Name = "LblPitch0"
        '
        'GrpMain
        '
        Me.GrpMain.Controls.Add(Me.GrpFram_2)
        Me.GrpMain.Controls.Add(Me.lblInfo)
        Me.GrpMain.Controls.Add(Me.GrpFram_1)
        Me.GrpMain.Controls.Add(Me.lblTitle2)
        Me.GrpMain.Controls.Add(Me.lblTitle)
        Me.GrpMain.Controls.Add(Me.GrpFram_0)
        resources.ApplyResources(Me.GrpMain, "GrpMain")
        Me.GrpMain.Name = "GrpMain"
        Me.GrpMain.TabStop = False
        '
        'GrpFram_2
        '
        Me.GrpFram_2.Controls.Add(Me.GridView)
        Me.GrpFram_2.Controls.Add(Me.LblResult_2)
        Me.GrpFram_2.Controls.Add(Me.LblResult_1)
        Me.GrpFram_2.Controls.Add(Me.LblResult_0)
        Me.GrpFram_2.Controls.Add(Me.LblDisp_10)
        Me.GrpFram_2.Controls.Add(Me.LblDisp_9)
        Me.GrpFram_2.Controls.Add(Me.LblDisp_11)
        Me.GrpFram_2.Controls.Add(Me.LblDisp_8)
        resources.ApplyResources(Me.GrpFram_2, "GrpFram_2")
        Me.GrpFram_2.Name = "GrpFram_2"
        Me.GrpFram_2.TabStop = False
        '
        'GridView
        '
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        resources.ApplyResources(Me.GridView, "GridView")
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.RowTemplate.Height = 21
        Me.GridView.TabStop = False
        '
        'LblResult_2
        '
        Me.LblResult_2.BackColor = System.Drawing.Color.White
        Me.LblResult_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblResult_2, "LblResult_2")
        Me.LblResult_2.Name = "LblResult_2"
        '
        'LblResult_1
        '
        Me.LblResult_1.BackColor = System.Drawing.Color.White
        Me.LblResult_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblResult_1, "LblResult_1")
        Me.LblResult_1.Name = "LblResult_1"
        '
        'LblResult_0
        '
        Me.LblResult_0.BackColor = System.Drawing.Color.White
        Me.LblResult_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblResult_0, "LblResult_0")
        Me.LblResult_0.Name = "LblResult_0"
        '
        'LblDisp_10
        '
        resources.ApplyResources(Me.LblDisp_10, "LblDisp_10")
        Me.LblDisp_10.Name = "LblDisp_10"
        '
        'LblDisp_9
        '
        resources.ApplyResources(Me.LblDisp_9, "LblDisp_9")
        Me.LblDisp_9.Name = "LblDisp_9"
        '
        'LblDisp_11
        '
        resources.ApplyResources(Me.LblDisp_11, "LblDisp_11")
        Me.LblDisp_11.Name = "LblDisp_11"
        '
        'LblDisp_8
        '
        resources.ApplyResources(Me.LblDisp_8, "LblDisp_8")
        Me.LblDisp_8.Name = "LblDisp_8"
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblInfo, "lblInfo")
        Me.lblInfo.ForeColor = System.Drawing.Color.White
        Me.lblInfo.Name = "lblInfo"
        '
        'GrpFram_1
        '
        Me.GrpFram_1.Controls.Add(Me.LblDisp_7)
        Me.GrpFram_1.Controls.Add(Me.LblDisp_6)
        Me.GrpFram_1.Controls.Add(Me.TxtPos2Y)
        Me.GrpFram_1.Controls.Add(Me.TxtPos2X)
        Me.GrpFram_1.Controls.Add(Me.LblDisp_3)
        Me.GrpFram_1.Controls.Add(Me.LblDisp_2)
        resources.ApplyResources(Me.GrpFram_1, "GrpFram_1")
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
        Me.TxtPos2Y.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.TxtPos2Y, "TxtPos2Y")
        Me.TxtPos2Y.Name = "TxtPos2Y"
        Me.TxtPos2Y.ReadOnly = True
        Me.TxtPos2Y.TabStop = False
        '
        'TxtPos2X
        '
        Me.TxtPos2X.AcceptsTab = True
        Me.TxtPos2X.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.TxtPos2X, "TxtPos2X")
        Me.TxtPos2X.Name = "TxtPos2X"
        Me.TxtPos2X.ReadOnly = True
        Me.TxtPos2X.TabStop = False
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
        'lblTitle2
        '
        Me.lblTitle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblTitle2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblTitle2, "lblTitle2")
        Me.lblTitle2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle2.Name = "lblTitle2"
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblTitle, "lblTitle")
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Name = "lblTitle"
        '
        'GrpFram_0
        '
        Me.GrpFram_0.Controls.Add(Me.LblDisp_5)
        Me.GrpFram_0.Controls.Add(Me.LblDisp_4)
        Me.GrpFram_0.Controls.Add(Me.TxtPosY)
        Me.GrpFram_0.Controls.Add(Me.TxtPosX)
        Me.GrpFram_0.Controls.Add(Me.LblDisp_1)
        Me.GrpFram_0.Controls.Add(Me.LblDisp_0)
        resources.ApplyResources(Me.GrpFram_0, "GrpFram_0")
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
        Me.TxtPosY.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.TxtPosY, "TxtPosY")
        Me.TxtPosY.Name = "TxtPosY"
        Me.TxtPosY.ReadOnly = True
        Me.TxtPosY.TabStop = False
        '
        'TxtPosX
        '
        Me.TxtPosX.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.TxtPosX, "TxtPosX")
        Me.TxtPosX.Name = "TxtPosX"
        Me.TxtPosX.ReadOnly = True
        Me.TxtPosX.TabStop = False
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
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'frmTxTyTeach
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpMain)
        Me.Controls.Add(Me.GrpArrow)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTxTyTeach"
        Me.ShowInTaskbar = False
        Me.GrpArrow.ResumeLayout(False)
        Me.GrpArrow.PerformLayout()
        Me.GrpPithPanel.ResumeLayout(False)
        Me.GrpPithPanel.PerformLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpMain.ResumeLayout(False)
        Me.GrpFram_2.ResumeLayout(False)
        Me.GrpFram_2.PerformLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpFram_1.ResumeLayout(False)
        Me.GrpFram_1.PerformLayout()
        Me.GrpFram_0.ResumeLayout(False)
        Me.GrpFram_0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpArrow As System.Windows.Forms.GroupBox
    Public WithEvents BtnJOG_6 As System.Windows.Forms.Button
    Public WithEvents BtnJOG_5 As System.Windows.Forms.Button
    Public WithEvents BtnJOG_7 As System.Windows.Forms.Button
    Public WithEvents BtnJOG_4 As System.Windows.Forms.Button
    Public WithEvents BtnHI As System.Windows.Forms.Button
    Public WithEvents BtnJOG_3 As System.Windows.Forms.Button
    Public WithEvents BtnJOG_2 As System.Windows.Forms.Button
    Public WithEvents BtnJOG_1 As System.Windows.Forms.Button
    Public WithEvents BtnJOG_0 As System.Windows.Forms.Button
    Public WithEvents BtnZ As System.Windows.Forms.Button
    Public WithEvents BtnRESET As System.Windows.Forms.Button
    Public WithEvents BtnSTART As System.Windows.Forms.Button
    Public WithEvents BtnHALT As System.Windows.Forms.Button
    Public WithEvents GrpPithPanel As System.Windows.Forms.GroupBox
    Friend WithEvents TBarPause As System.Windows.Forms.TrackBar
    Friend WithEvents TBarHiPitch As System.Windows.Forms.TrackBar
    Friend WithEvents TBarLowPitch As System.Windows.Forms.TrackBar
    Public WithEvents LblTchMoval2 As System.Windows.Forms.Label
    Public WithEvents LblTchMoval1 As System.Windows.Forms.Label
    Public WithEvents LblTchMoval0 As System.Windows.Forms.Label
    Public WithEvents LblPitch2 As System.Windows.Forms.Label
    Public WithEvents LblPitch1 As System.Windows.Forms.Label
    Public WithEvents LblPitch0 As System.Windows.Forms.Label
    Friend WithEvents GrpMain As System.Windows.Forms.GroupBox
    Friend WithEvents GrpFram_0 As System.Windows.Forms.GroupBox
    Friend WithEvents LblDisp_5 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_4 As System.Windows.Forms.Label
    Friend WithEvents TxtPosY As System.Windows.Forms.TextBox
    Friend WithEvents TxtPosX As System.Windows.Forms.TextBox
    Friend WithEvents LblDisp_1 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_0 As System.Windows.Forms.Label
    Public WithEvents lblTitle2 As System.Windows.Forms.Label
    Friend WithEvents GrpFram_2 As System.Windows.Forms.GroupBox
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents LblResult_2 As System.Windows.Forms.Label
    Friend WithEvents LblResult_1 As System.Windows.Forms.Label
    Friend WithEvents LblResult_0 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_10 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_9 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_11 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_8 As System.Windows.Forms.Label
    Public WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents GrpFram_1 As System.Windows.Forms.GroupBox
    Friend WithEvents LblDisp_7 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_6 As System.Windows.Forms.Label
    Friend WithEvents TxtPos2Y As System.Windows.Forms.TextBox
    Friend WithEvents TxtPos2X As System.Windows.Forms.TextBox
    Friend WithEvents LblDisp_3 As System.Windows.Forms.Label
    Friend WithEvents LblDisp_2 As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
#End Region
End Class