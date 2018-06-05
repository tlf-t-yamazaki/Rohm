<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class FrmCutPosCorrect
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
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents txtBlockNoY As System.Windows.Forms.TextBox
    Public WithEvents txtBlockNoX As System.Windows.Forms.TextBox
    Public WithEvents fgrdPoints As AxMSFlexGridLib.AxMSFlexGrid
    Public WithEvents lblCutCorrectOffsetY As System.Windows.Forms.Label
    Public WithEvents lblCutCorrectOffsetYTitle As System.Windows.Forms.Label
    Public WithEvents lblCutCorrectOffsetX As System.Windows.Forms.Label
    Public WithEvents lblCutCorrectOffsetXTitle As System.Windows.Forms.Label
    Public WithEvents lblGroupResistor As System.Windows.Forms.Label
    Public WithEvents lblGroupResistorTitle As System.Windows.Forms.Label
    Public WithEvents lblBlockNoY As System.Windows.Forms.Label
    Public WithEvents lblBlockNoX As System.Windows.Forms.Label
    Public WithEvents frmTitle As System.Windows.Forms.GroupBox
    Public WithEvents lblInfo As System.Windows.Forms.Label
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCutPosCorrect))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.frmTitle = New System.Windows.Forms.GroupBox()
        Me.dgvPoints = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtBlockNoY = New System.Windows.Forms.TextBox()
        Me.txtBlockNoX = New System.Windows.Forms.TextBox()
        Me.lblCutCorrectOffsetY = New System.Windows.Forms.Label()
        Me.lblCutCorrectOffsetYTitle = New System.Windows.Forms.Label()
        Me.lblCutCorrectOffsetX = New System.Windows.Forms.Label()
        Me.lblCutCorrectOffsetXTitle = New System.Windows.Forms.Label()
        Me.lblGroupResistor = New System.Windows.Forms.Label()
        Me.lblGroupResistorTitle = New System.Windows.Forms.Label()
        Me.lblBlockNoY = New System.Windows.Forms.Label()
        Me.lblBlockNoX = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.GrpArrow = New System.Windows.Forms.GroupBox()
        Me.fgrdPoints = New AxMSFlexGridLib.AxMSFlexGrid()
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
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.frmTitle.SuspendLayout()
        CType(Me.dgvPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpArrow.SuspendLayout()
        CType(Me.fgrdPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpPithPanel.SuspendLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.ControlDark
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'frmTitle
        '
        Me.frmTitle.BackColor = System.Drawing.SystemColors.Control
        Me.frmTitle.Controls.Add(Me.dgvPoints)
        Me.frmTitle.Controls.Add(Me.txtBlockNoY)
        Me.frmTitle.Controls.Add(Me.txtBlockNoX)
        Me.frmTitle.Controls.Add(Me.lblCutCorrectOffsetY)
        Me.frmTitle.Controls.Add(Me.lblCutCorrectOffsetYTitle)
        Me.frmTitle.Controls.Add(Me.lblCutCorrectOffsetX)
        Me.frmTitle.Controls.Add(Me.lblCutCorrectOffsetXTitle)
        Me.frmTitle.Controls.Add(Me.lblGroupResistor)
        Me.frmTitle.Controls.Add(Me.lblGroupResistorTitle)
        Me.frmTitle.Controls.Add(Me.lblBlockNoY)
        Me.frmTitle.Controls.Add(Me.lblBlockNoX)
        Me.frmTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.frmTitle, "frmTitle")
        Me.frmTitle.Name = "frmTitle"
        Me.frmTitle.TabStop = False
        '
        'dgvPoints
        '
        Me.dgvPoints.AllowUserToAddRows = False
        Me.dgvPoints.AllowUserToDeleteRows = False
        Me.dgvPoints.AllowUserToResizeColumns = False
        Me.dgvPoints.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPoints.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPoints.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5})
        resources.ApplyResources(Me.dgvPoints, "dgvPoints")
        Me.dgvPoints.MultiSelect = False
        Me.dgvPoints.Name = "dgvPoints"
        Me.dgvPoints.ReadOnly = True
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPoints.RowHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgvPoints.RowHeadersVisible = False
        Me.dgvPoints.RowTemplate.Height = 20
        Me.dgvPoints.RowTemplate.ReadOnly = True
        Me.dgvPoints.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPoints.ShowCellErrors = False
        Me.dgvPoints.ShowCellToolTips = False
        Me.dgvPoints.ShowEditingIcon = False
        Me.dgvPoints.ShowRowErrors = False
        Me.dgvPoints.TabStop = False
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.DividerWidth = 1
        Me.Column1.Frozen = True
        resources.ApplyResources(Me.Column1, "Column1")
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.Frozen = True
        resources.ApplyResources(Me.Column2, "Column2")
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column3
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column3.Frozen = True
        resources.ApplyResources(Me.Column3, "Column3")
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column4
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column4.Frozen = True
        resources.ApplyResources(Me.Column4, "Column4")
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column5
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column5.Frozen = True
        resources.ApplyResources(Me.Column5, "Column5")
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'txtBlockNoY
        '
        Me.txtBlockNoY.AcceptsReturn = True
        Me.txtBlockNoY.BackColor = System.Drawing.SystemColors.Window
        Me.txtBlockNoY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBlockNoY.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.txtBlockNoY, "txtBlockNoY")
        Me.txtBlockNoY.Name = "txtBlockNoY"
        '
        'txtBlockNoX
        '
        Me.txtBlockNoX.AcceptsReturn = True
        Me.txtBlockNoX.BackColor = System.Drawing.SystemColors.Window
        Me.txtBlockNoX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBlockNoX.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.txtBlockNoX, "txtBlockNoX")
        Me.txtBlockNoX.Name = "txtBlockNoX"
        '
        'lblCutCorrectOffsetY
        '
        Me.lblCutCorrectOffsetY.BackColor = System.Drawing.SystemColors.Control
        Me.lblCutCorrectOffsetY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCutCorrectOffsetY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCutCorrectOffsetY.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblCutCorrectOffsetY, "lblCutCorrectOffsetY")
        Me.lblCutCorrectOffsetY.Name = "lblCutCorrectOffsetY"
        '
        'lblCutCorrectOffsetYTitle
        '
        Me.lblCutCorrectOffsetYTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblCutCorrectOffsetYTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCutCorrectOffsetYTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblCutCorrectOffsetYTitle, "lblCutCorrectOffsetYTitle")
        Me.lblCutCorrectOffsetYTitle.Name = "lblCutCorrectOffsetYTitle"
        '
        'lblCutCorrectOffsetX
        '
        Me.lblCutCorrectOffsetX.BackColor = System.Drawing.SystemColors.Control
        Me.lblCutCorrectOffsetX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCutCorrectOffsetX.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCutCorrectOffsetX.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblCutCorrectOffsetX, "lblCutCorrectOffsetX")
        Me.lblCutCorrectOffsetX.Name = "lblCutCorrectOffsetX"
        '
        'lblCutCorrectOffsetXTitle
        '
        Me.lblCutCorrectOffsetXTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblCutCorrectOffsetXTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCutCorrectOffsetXTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblCutCorrectOffsetXTitle, "lblCutCorrectOffsetXTitle")
        Me.lblCutCorrectOffsetXTitle.Name = "lblCutCorrectOffsetXTitle"
        '
        'lblGroupResistor
        '
        Me.lblGroupResistor.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.lblGroupResistor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblGroupResistor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroupResistor.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGroupResistor, "lblGroupResistor")
        Me.lblGroupResistor.Name = "lblGroupResistor"
        '
        'lblGroupResistorTitle
        '
        Me.lblGroupResistorTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblGroupResistorTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGroupResistorTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblGroupResistorTitle, "lblGroupResistorTitle")
        Me.lblGroupResistorTitle.Name = "lblGroupResistorTitle"
        '
        'lblBlockNoY
        '
        Me.lblBlockNoY.BackColor = System.Drawing.SystemColors.Control
        Me.lblBlockNoY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBlockNoY.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblBlockNoY, "lblBlockNoY")
        Me.lblBlockNoY.Name = "lblBlockNoY"
        '
        'lblBlockNoX
        '
        Me.lblBlockNoX.BackColor = System.Drawing.SystemColors.Control
        Me.lblBlockNoX.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBlockNoX.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblBlockNoX, "lblBlockNoX")
        Me.lblBlockNoX.Name = "lblBlockNoX"
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
        Me.GrpArrow.Controls.Add(Me.fgrdPoints)
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
        'fgrdPoints
        '
        resources.ApplyResources(Me.fgrdPoints, "fgrdPoints")
        Me.fgrdPoints.Name = "fgrdPoints"
        Me.fgrdPoints.OcxState = CType(resources.GetObject("fgrdPoints.OcxState"), System.Windows.Forms.AxHost.State)
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
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdOK, "cmdOK")
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'FrmCutPosCorrect
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GrpArrow)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.frmTitle)
        Me.Controls.Add(Me.lblInfo)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmCutPosCorrect"
        Me.ShowInTaskbar = False
        Me.frmTitle.ResumeLayout(False)
        Me.frmTitle.PerformLayout()
        CType(Me.dgvPoints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpArrow.ResumeLayout(False)
        Me.GrpArrow.PerformLayout()
        CType(Me.fgrdPoints, System.ComponentModel.ISupportInitialize).EndInit()
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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents dgvPoints As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class