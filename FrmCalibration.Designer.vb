<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class FrmCalibration
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
    Public WithEvents lblOffsetY As System.Windows.Forms.Label
    Public WithEvents lblOffsetX As System.Windows.Forms.Label
    Public WithEvents lblGainY As System.Windows.Forms.Label
    Public WithEvents lblGainX As System.Windows.Forms.Label
    Public WithEvents lblOffsetYTitle As System.Windows.Forms.Label
    Public WithEvents lblOffsetXTitle As System.Windows.Forms.Label
    Public WithEvents lblGainYTitle As System.Windows.Forms.Label
    Public WithEvents lblGainXTitle As System.Windows.Forms.Label
    Public WithEvents Line1 As System.Windows.Forms.Label
    Public WithEvents lblGap2Y As System.Windows.Forms.Label
    Public WithEvents lblGap2X As System.Windows.Forms.Label
    Public WithEvents lblGap1Y As System.Windows.Forms.Label
    Public WithEvents lblGap1X As System.Windows.Forms.Label
    Public WithEvents lblGap2YTitle As System.Windows.Forms.Label
    Public WithEvents lblGap2XTitle As System.Windows.Forms.Label
    Public WithEvents lblGap1YTitle As System.Windows.Forms.Label
    Public WithEvents lblGap1XTitle As System.Windows.Forms.Label
    Public WithEvents lblTableOffsetY As System.Windows.Forms.Label
    Public WithEvents lblTableOffsetX As System.Windows.Forms.Label
    Public WithEvents lblTableOffsetYTitle As System.Windows.Forms.Label
    Public WithEvents lblTableOffsetXTitle As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates2Y As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates2X As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates1Y As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates2YTitle As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates2XTitle As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates1YTitle As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates1X As System.Windows.Forms.Label
    Public WithEvents lblStandardCoordinates1XTitle As System.Windows.Forms.Label
    Public WithEvents frmTitle As System.Windows.Forms.GroupBox
    Public WithEvents lblInfo As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCalibration))
        Me.frmTitle = New System.Windows.Forms.GroupBox()
        Me.lblOffsetY = New System.Windows.Forms.Label()
        Me.lblOffsetX = New System.Windows.Forms.Label()
        Me.lblGainY = New System.Windows.Forms.Label()
        Me.lblGainX = New System.Windows.Forms.Label()
        Me.lblOffsetYTitle = New System.Windows.Forms.Label()
        Me.lblOffsetXTitle = New System.Windows.Forms.Label()
        Me.lblGainYTitle = New System.Windows.Forms.Label()
        Me.lblGainXTitle = New System.Windows.Forms.Label()
        Me.Line1 = New System.Windows.Forms.Label()
        Me.lblGap2Y = New System.Windows.Forms.Label()
        Me.lblGap2X = New System.Windows.Forms.Label()
        Me.lblGap1Y = New System.Windows.Forms.Label()
        Me.lblGap1X = New System.Windows.Forms.Label()
        Me.lblGap2YTitle = New System.Windows.Forms.Label()
        Me.lblGap2XTitle = New System.Windows.Forms.Label()
        Me.lblGap1YTitle = New System.Windows.Forms.Label()
        Me.lblGap1XTitle = New System.Windows.Forms.Label()
        Me.lblTableOffsetY = New System.Windows.Forms.Label()
        Me.lblTableOffsetX = New System.Windows.Forms.Label()
        Me.lblTableOffsetYTitle = New System.Windows.Forms.Label()
        Me.lblTableOffsetXTitle = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates2Y = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates2X = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates1Y = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates2YTitle = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates2XTitle = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates1YTitle = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates1X = New System.Windows.Forms.Label()
        Me.lblStandardCoordinates1XTitle = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
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
        Me.frmTitle.SuspendLayout()
        Me.GrpArrow.SuspendLayout()
        Me.GrpPithPanel.SuspendLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'frmTitle
        '
        Me.frmTitle.BackColor = System.Drawing.SystemColors.Control
        Me.frmTitle.Controls.Add(Me.lblOffsetY)
        Me.frmTitle.Controls.Add(Me.lblOffsetX)
        Me.frmTitle.Controls.Add(Me.lblGainY)
        Me.frmTitle.Controls.Add(Me.lblGainX)
        Me.frmTitle.Controls.Add(Me.lblOffsetYTitle)
        Me.frmTitle.Controls.Add(Me.lblOffsetXTitle)
        Me.frmTitle.Controls.Add(Me.lblGainYTitle)
        Me.frmTitle.Controls.Add(Me.lblGainXTitle)
        Me.frmTitle.Controls.Add(Me.Line1)
        Me.frmTitle.Controls.Add(Me.lblGap2Y)
        Me.frmTitle.Controls.Add(Me.lblGap2X)
        Me.frmTitle.Controls.Add(Me.lblGap1Y)
        Me.frmTitle.Controls.Add(Me.lblGap1X)
        Me.frmTitle.Controls.Add(Me.lblGap2YTitle)
        Me.frmTitle.Controls.Add(Me.lblGap2XTitle)
        Me.frmTitle.Controls.Add(Me.lblGap1YTitle)
        Me.frmTitle.Controls.Add(Me.lblGap1XTitle)
        Me.frmTitle.Controls.Add(Me.lblTableOffsetY)
        Me.frmTitle.Controls.Add(Me.lblTableOffsetX)
        Me.frmTitle.Controls.Add(Me.lblTableOffsetYTitle)
        Me.frmTitle.Controls.Add(Me.lblTableOffsetXTitle)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates2Y)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates2X)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates1Y)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates2YTitle)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates2XTitle)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates1YTitle)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates1X)
        Me.frmTitle.Controls.Add(Me.lblStandardCoordinates1XTitle)
        Me.frmTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.frmTitle, "frmTitle")
        Me.frmTitle.Name = "frmTitle"
        Me.frmTitle.TabStop = False
        '
        'lblOffsetY
        '
        Me.lblOffsetY.BackColor = System.Drawing.SystemColors.Control
        Me.lblOffsetY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblOffsetY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffsetY.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblOffsetY, "lblOffsetY")
        Me.lblOffsetY.Name = "lblOffsetY"
        '
        'lblOffsetX
        '
        Me.lblOffsetX.BackColor = System.Drawing.SystemColors.Control
        Me.lblOffsetX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblOffsetX.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffsetX.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblOffsetX, "lblOffsetX")
        Me.lblOffsetX.Name = "lblOffsetX"
        '
        'lblGainY
        '
        Me.lblGainY.BackColor = System.Drawing.SystemColors.Control
        Me.lblGainY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGainY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainY.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGainY, "lblGainY")
        Me.lblGainY.Name = "lblGainY"
        '
        'lblGainX
        '
        Me.lblGainX.BackColor = System.Drawing.SystemColors.Control
        Me.lblGainX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGainX.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainX.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGainX, "lblGainX")
        Me.lblGainX.Name = "lblGainX"
        '
        'lblOffsetYTitle
        '
        Me.lblOffsetYTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblOffsetYTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffsetYTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblOffsetYTitle, "lblOffsetYTitle")
        Me.lblOffsetYTitle.Name = "lblOffsetYTitle"
        '
        'lblOffsetXTitle
        '
        Me.lblOffsetXTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblOffsetXTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffsetXTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblOffsetXTitle, "lblOffsetXTitle")
        Me.lblOffsetXTitle.Name = "lblOffsetXTitle"
        '
        'lblGainYTitle
        '
        Me.lblGainYTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGainYTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainYTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGainYTitle, "lblGainYTitle")
        Me.lblGainYTitle.Name = "lblGainYTitle"
        '
        'lblGainXTitle
        '
        Me.lblGainXTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGainXTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainXTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGainXTitle, "lblGainXTitle")
        Me.lblGainXTitle.Name = "lblGainXTitle"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.Line1, "Line1")
        Me.Line1.Name = "Line1"
        '
        'lblGap2Y
        '
        Me.lblGap2Y.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap2Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGap2Y.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap2Y.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap2Y, "lblGap2Y")
        Me.lblGap2Y.Name = "lblGap2Y"
        '
        'lblGap2X
        '
        Me.lblGap2X.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap2X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGap2X.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap2X.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap2X, "lblGap2X")
        Me.lblGap2X.Name = "lblGap2X"
        '
        'lblGap1Y
        '
        Me.lblGap1Y.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap1Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGap1Y.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap1Y.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap1Y, "lblGap1Y")
        Me.lblGap1Y.Name = "lblGap1Y"
        '
        'lblGap1X
        '
        Me.lblGap1X.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap1X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGap1X.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap1X.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap1X, "lblGap1X")
        Me.lblGap1X.Name = "lblGap1X"
        '
        'lblGap2YTitle
        '
        Me.lblGap2YTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap2YTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap2YTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap2YTitle, "lblGap2YTitle")
        Me.lblGap2YTitle.Name = "lblGap2YTitle"
        '
        'lblGap2XTitle
        '
        Me.lblGap2XTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap2XTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap2XTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap2XTitle, "lblGap2XTitle")
        Me.lblGap2XTitle.Name = "lblGap2XTitle"
        '
        'lblGap1YTitle
        '
        Me.lblGap1YTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap1YTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap1YTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap1YTitle, "lblGap1YTitle")
        Me.lblGap1YTitle.Name = "lblGap1YTitle"
        '
        'lblGap1XTitle
        '
        Me.lblGap1XTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGap1XTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGap1XTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGap1XTitle, "lblGap1XTitle")
        Me.lblGap1XTitle.Name = "lblGap1XTitle"
        '
        'lblTableOffsetY
        '
        Me.lblTableOffsetY.BackColor = System.Drawing.SystemColors.Control
        Me.lblTableOffsetY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTableOffsetY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTableOffsetY.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblTableOffsetY, "lblTableOffsetY")
        Me.lblTableOffsetY.Name = "lblTableOffsetY"
        '
        'lblTableOffsetX
        '
        Me.lblTableOffsetX.BackColor = System.Drawing.SystemColors.Control
        Me.lblTableOffsetX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTableOffsetX.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTableOffsetX.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblTableOffsetX, "lblTableOffsetX")
        Me.lblTableOffsetX.Name = "lblTableOffsetX"
        '
        'lblTableOffsetYTitle
        '
        Me.lblTableOffsetYTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTableOffsetYTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTableOffsetYTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblTableOffsetYTitle, "lblTableOffsetYTitle")
        Me.lblTableOffsetYTitle.Name = "lblTableOffsetYTitle"
        '
        'lblTableOffsetXTitle
        '
        Me.lblTableOffsetXTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTableOffsetXTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTableOffsetXTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblTableOffsetXTitle, "lblTableOffsetXTitle")
        Me.lblTableOffsetXTitle.Name = "lblTableOffsetXTitle"
        '
        'lblStandardCoordinates2Y
        '
        Me.lblStandardCoordinates2Y.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates2Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStandardCoordinates2Y.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates2Y.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates2Y, "lblStandardCoordinates2Y")
        Me.lblStandardCoordinates2Y.Name = "lblStandardCoordinates2Y"
        '
        'lblStandardCoordinates2X
        '
        Me.lblStandardCoordinates2X.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates2X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStandardCoordinates2X.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates2X.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates2X, "lblStandardCoordinates2X")
        Me.lblStandardCoordinates2X.Name = "lblStandardCoordinates2X"
        '
        'lblStandardCoordinates1Y
        '
        Me.lblStandardCoordinates1Y.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates1Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStandardCoordinates1Y.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates1Y.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates1Y, "lblStandardCoordinates1Y")
        Me.lblStandardCoordinates1Y.Name = "lblStandardCoordinates1Y"
        '
        'lblStandardCoordinates2YTitle
        '
        Me.lblStandardCoordinates2YTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates2YTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates2YTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates2YTitle, "lblStandardCoordinates2YTitle")
        Me.lblStandardCoordinates2YTitle.Name = "lblStandardCoordinates2YTitle"
        '
        'lblStandardCoordinates2XTitle
        '
        Me.lblStandardCoordinates2XTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates2XTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates2XTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates2XTitle, "lblStandardCoordinates2XTitle")
        Me.lblStandardCoordinates2XTitle.Name = "lblStandardCoordinates2XTitle"
        '
        'lblStandardCoordinates1YTitle
        '
        Me.lblStandardCoordinates1YTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates1YTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates1YTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates1YTitle, "lblStandardCoordinates1YTitle")
        Me.lblStandardCoordinates1YTitle.Name = "lblStandardCoordinates1YTitle"
        '
        'lblStandardCoordinates1X
        '
        Me.lblStandardCoordinates1X.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates1X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStandardCoordinates1X.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates1X.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates1X, "lblStandardCoordinates1X")
        Me.lblStandardCoordinates1X.Name = "lblStandardCoordinates1X"
        '
        'lblStandardCoordinates1XTitle
        '
        Me.lblStandardCoordinates1XTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblStandardCoordinates1XTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandardCoordinates1XTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblStandardCoordinates1XTitle, "lblStandardCoordinates1XTitle")
        Me.lblStandardCoordinates1XTitle.Name = "lblStandardCoordinates1XTitle"
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.SystemColors.Info
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblInfo, "lblInfo")
        Me.lblInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInfo.Name = "lblInfo"
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
        'FrmCalibration
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.GrpArrow)
        Me.Controls.Add(Me.frmTitle)
        Me.Controls.Add(Me.lblInfo)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmCalibration"
        Me.ShowInTaskbar = False
        Me.frmTitle.ResumeLayout(False)
        Me.GrpArrow.ResumeLayout(False)
        Me.GrpArrow.PerformLayout()
        Me.GrpPithPanel.ResumeLayout(False)
        Me.GrpPithPanel.PerformLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).EndInit()
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
#End Region
End Class