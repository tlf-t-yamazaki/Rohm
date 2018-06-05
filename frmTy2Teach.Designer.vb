<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTy2Teach
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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTy2Teach))
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
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
        Me.LblInterval = New System.Windows.Forms.Label()
        Me.GrpTyTeach_2 = New System.Windows.Forms.GroupBox()
        Me.FGridTy2Teach = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.dgvTy2Teach = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GrpTyTeach_1 = New System.Windows.Forms.GroupBox()
        Me.Lbl_03 = New System.Windows.Forms.Label()
        Me.Lbl_01 = New System.Windows.Forms.Label()
        Me.TxtPosY = New System.Windows.Forms.TextBox()
        Me.TxtPosX = New System.Windows.Forms.TextBox()
        Me.Lbl_02 = New System.Windows.Forms.Label()
        Me.Lbl_00 = New System.Windows.Forms.Label()
        Me.lblTyTitle = New System.Windows.Forms.Label()
        Me.lblTyInfo = New System.Windows.Forms.Label()
        Me.GrpArrow.SuspendLayout()
        Me.GrpPithPanel.SuspendLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpTyTeach_2.SuspendLayout()
        CType(Me.FGridTy2Teach, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvTy2Teach, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpTyTeach_1.SuspendLayout()
        Me.SuspendLayout()
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
        'LblInterval
        '
        resources.ApplyResources(Me.LblInterval, "LblInterval")
        Me.LblInterval.Name = "LblInterval"
        '
        'GrpTyTeach_2
        '
        Me.GrpTyTeach_2.Controls.Add(Me.FGridTy2Teach)
        Me.GrpTyTeach_2.Controls.Add(Me.dgvTy2Teach)
        Me.GrpTyTeach_2.Controls.Add(Me.GrpTyTeach_1)
        Me.GrpTyTeach_2.Controls.Add(Me.lblTyTitle)
        Me.GrpTyTeach_2.Controls.Add(Me.lblTyInfo)
        Me.GrpTyTeach_2.Controls.Add(Me.LblInterval)
        resources.ApplyResources(Me.GrpTyTeach_2, "GrpTyTeach_2")
        Me.GrpTyTeach_2.Name = "GrpTyTeach_2"
        Me.GrpTyTeach_2.TabStop = False
        '
        'FGridTy2Teach
        '
        resources.ApplyResources(Me.FGridTy2Teach, "FGridTy2Teach")
        Me.FGridTy2Teach.Name = "FGridTy2Teach"
        Me.FGridTy2Teach.OcxState = CType(resources.GetObject("FGridTy2Teach.OcxState"), System.Windows.Forms.AxHost.State)
        '
        'dgvTy2Teach
        '
        Me.dgvTy2Teach.AllowUserToAddRows = False
        Me.dgvTy2Teach.AllowUserToDeleteRows = False
        Me.dgvTy2Teach.AllowUserToResizeColumns = False
        Me.dgvTy2Teach.AllowUserToResizeRows = False
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvTy2Teach.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvTy2Teach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTy2Teach.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        resources.ApplyResources(Me.dgvTy2Teach, "dgvTy2Teach")
        Me.dgvTy2Teach.MultiSelect = False
        Me.dgvTy2Teach.Name = "dgvTy2Teach"
        Me.dgvTy2Teach.ReadOnly = True
        Me.dgvTy2Teach.RowHeadersVisible = False
        Me.dgvTy2Teach.RowTemplate.Height = 21
        Me.dgvTy2Teach.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTy2Teach.ShowCellErrors = False
        Me.dgvTy2Teach.ShowCellToolTips = False
        Me.dgvTy2Teach.ShowEditingIcon = False
        Me.dgvTy2Teach.ShowRowErrors = False
        '
        'Column1
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle5
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
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column2.Frozen = True
        resources.ApplyResources(Me.Column2, "Column2")
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'GrpTyTeach_1
        '
        Me.GrpTyTeach_1.Controls.Add(Me.Lbl_03)
        Me.GrpTyTeach_1.Controls.Add(Me.Lbl_01)
        Me.GrpTyTeach_1.Controls.Add(Me.TxtPosY)
        Me.GrpTyTeach_1.Controls.Add(Me.TxtPosX)
        Me.GrpTyTeach_1.Controls.Add(Me.Lbl_02)
        Me.GrpTyTeach_1.Controls.Add(Me.Lbl_00)
        resources.ApplyResources(Me.GrpTyTeach_1, "GrpTyTeach_1")
        Me.GrpTyTeach_1.Name = "GrpTyTeach_1"
        Me.GrpTyTeach_1.TabStop = False
        '
        'Lbl_03
        '
        resources.ApplyResources(Me.Lbl_03, "Lbl_03")
        Me.Lbl_03.Name = "Lbl_03"
        '
        'Lbl_01
        '
        resources.ApplyResources(Me.Lbl_01, "Lbl_01")
        Me.Lbl_01.Name = "Lbl_01"
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
        'Lbl_02
        '
        resources.ApplyResources(Me.Lbl_02, "Lbl_02")
        Me.Lbl_02.Name = "Lbl_02"
        '
        'Lbl_00
        '
        resources.ApplyResources(Me.Lbl_00, "Lbl_00")
        Me.Lbl_00.Name = "Lbl_00"
        '
        'lblTyTitle
        '
        resources.ApplyResources(Me.lblTyTitle, "lblTyTitle")
        Me.lblTyTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTyTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTyTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTyTitle.Name = "lblTyTitle"
        '
        'lblTyInfo
        '
        Me.lblTyInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTyInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTyInfo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblTyInfo, "lblTyInfo")
        Me.lblTyInfo.ForeColor = System.Drawing.Color.White
        Me.lblTyInfo.Name = "lblTyInfo"
        '
        'frmTy2Teach
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpArrow)
        Me.Controls.Add(Me.GrpTyTeach_2)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTy2Teach"
        Me.ShowInTaskbar = False
        Me.GrpArrow.ResumeLayout(False)
        Me.GrpArrow.PerformLayout()
        Me.GrpPithPanel.ResumeLayout(False)
        Me.GrpPithPanel.PerformLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpTyTeach_2.ResumeLayout(False)
        Me.GrpTyTeach_2.PerformLayout()
        CType(Me.FGridTy2Teach, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvTy2Teach, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpTyTeach_1.ResumeLayout(False)
        Me.GrpTyTeach_1.PerformLayout()
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
    Public WithEvents FGridTy2Teach As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents LblInterval As System.Windows.Forms.Label
    Public WithEvents GrpTyTeach_2 As System.Windows.Forms.GroupBox
    Friend WithEvents GrpTyTeach_1 As System.Windows.Forms.GroupBox
    Friend WithEvents Lbl_03 As System.Windows.Forms.Label
    Friend WithEvents Lbl_01 As System.Windows.Forms.Label
    Friend WithEvents TxtPosY As System.Windows.Forms.TextBox
    Friend WithEvents TxtPosX As System.Windows.Forms.TextBox
    Friend WithEvents Lbl_02 As System.Windows.Forms.Label
    Friend WithEvents Lbl_00 As System.Windows.Forms.Label
    Public WithEvents lblTyTitle As System.Windows.Forms.Label
    Public WithEvents lblTyInfo As System.Windows.Forms.Label
    Private WithEvents dgvTy2Teach As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class