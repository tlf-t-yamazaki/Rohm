<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProbeCleaning

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProbeCleaning))
        Me.GrpArrow = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbMoveMode = New System.Windows.Forms.ComboBox()
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
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.btnZRegMove = New System.Windows.Forms.Button()
        Me.btnStageRegPosMove = New System.Windows.Forms.Button()
        Me.GrpMain = New System.Windows.Forms.GroupBox()
        Me.LblMsg = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtOption1_X = New System.Windows.Forms.TextBox()
        Me.LblOption2_2 = New System.Windows.Forms.Label()
        Me.LblOption2_1 = New System.Windows.Forms.Label()
        Me.txtOption2_Y = New System.Windows.Forms.TextBox()
        Me.txtOption1_Y = New System.Windows.Forms.TextBox()
        Me.txtOption2_X = New System.Windows.Forms.TextBox()
        Me.LblOption1_2 = New System.Windows.Forms.Label()
        Me.LblOption1_1 = New System.Windows.Forms.Label()
        Me.btnCleaningStart = New System.Windows.Forms.Button()
        Me.txtProbingCnt = New System.Windows.Forms.TextBox()
        Me.txtContactCount = New System.Windows.Forms.TextBox()
        Me.LblRemark01 = New System.Windows.Forms.Label()
        Me.LblProbingCnt = New System.Windows.Forms.Label()
        Me.LblContactCount = New System.Windows.Forms.Label()
        Me.grpBpOff = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtRegPosX = New System.Windows.Forms.TextBox()
        Me.txtRegPosZ = New System.Windows.Forms.TextBox()
        Me.txtRegPosY = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtStagePosX = New System.Windows.Forms.TextBox()
        Me.txtZPos = New System.Windows.Forms.TextBox()
        Me.txtStagePosY = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblHead01 = New System.Windows.Forms.Label()
        Me.BtnTenKey = New System.Windows.Forms.Button()
        Me.GrpArrow.SuspendLayout()
        Me.GrpPithPanel.SuspendLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpMain.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.grpBpOff.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpArrow
        '
        Me.GrpArrow.Controls.Add(Me.Label8)
        Me.GrpArrow.Controls.Add(Me.cmbMoveMode)
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
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'cmbMoveMode
        '
        Me.cmbMoveMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbMoveMode, "cmbMoveMode")
        Me.cmbMoveMode.FormattingEnabled = True
        Me.cmbMoveMode.Items.AddRange(New Object() {resources.GetString("cmbMoveMode.Items"), resources.GetString("cmbMoveMode.Items1")})
        Me.cmbMoveMode.Name = "cmbMoveMode"
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
        'Timer1
        '
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
        'btnZRegMove
        '
        resources.ApplyResources(Me.btnZRegMove, "btnZRegMove")
        Me.btnZRegMove.Name = "btnZRegMove"
        Me.btnZRegMove.UseVisualStyleBackColor = True
        '
        'btnStageRegPosMove
        '
        resources.ApplyResources(Me.btnStageRegPosMove, "btnStageRegPosMove")
        Me.btnStageRegPosMove.Name = "btnStageRegPosMove"
        Me.btnStageRegPosMove.UseVisualStyleBackColor = True
        '
        'GrpMain
        '
        Me.GrpMain.Controls.Add(Me.LblMsg)
        Me.GrpMain.Controls.Add(Me.GroupBox3)
        Me.GrpMain.Controls.Add(Me.grpBpOff)
        Me.GrpMain.Controls.Add(Me.LblHead01)
        resources.ApplyResources(Me.GrpMain, "GrpMain")
        Me.GrpMain.Name = "GrpMain"
        Me.GrpMain.TabStop = False
        '
        'LblMsg
        '
        Me.LblMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LblMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.LblMsg, "LblMsg")
        Me.LblMsg.Name = "LblMsg"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtOption1_X)
        Me.GroupBox3.Controls.Add(Me.LblOption2_2)
        Me.GroupBox3.Controls.Add(Me.LblOption2_1)
        Me.GroupBox3.Controls.Add(Me.txtOption2_Y)
        Me.GroupBox3.Controls.Add(Me.txtOption1_Y)
        Me.GroupBox3.Controls.Add(Me.txtOption2_X)
        Me.GroupBox3.Controls.Add(Me.LblOption1_2)
        Me.GroupBox3.Controls.Add(Me.LblOption1_1)
        Me.GroupBox3.Controls.Add(Me.btnCleaningStart)
        Me.GroupBox3.Controls.Add(Me.txtProbingCnt)
        Me.GroupBox3.Controls.Add(Me.txtContactCount)
        Me.GroupBox3.Controls.Add(Me.LblRemark01)
        Me.GroupBox3.Controls.Add(Me.LblProbingCnt)
        Me.GroupBox3.Controls.Add(Me.LblContactCount)
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        '
        'txtOption1_X
        '
        resources.ApplyResources(Me.txtOption1_X, "txtOption1_X")
        Me.txtOption1_X.Name = "txtOption1_X"
        '
        'LblOption2_2
        '
        resources.ApplyResources(Me.LblOption2_2, "LblOption2_2")
        Me.LblOption2_2.Name = "LblOption2_2"
        '
        'LblOption2_1
        '
        resources.ApplyResources(Me.LblOption2_1, "LblOption2_1")
        Me.LblOption2_1.Name = "LblOption2_1"
        '
        'txtOption2_Y
        '
        resources.ApplyResources(Me.txtOption2_Y, "txtOption2_Y")
        Me.txtOption2_Y.Name = "txtOption2_Y"
        '
        'txtOption1_Y
        '
        resources.ApplyResources(Me.txtOption1_Y, "txtOption1_Y")
        Me.txtOption1_Y.Name = "txtOption1_Y"
        '
        'txtOption2_X
        '
        resources.ApplyResources(Me.txtOption2_X, "txtOption2_X")
        Me.txtOption2_X.Name = "txtOption2_X"
        '
        'LblOption1_2
        '
        resources.ApplyResources(Me.LblOption1_2, "LblOption1_2")
        Me.LblOption1_2.Name = "LblOption1_2"
        '
        'LblOption1_1
        '
        resources.ApplyResources(Me.LblOption1_1, "LblOption1_1")
        Me.LblOption1_1.Name = "LblOption1_1"
        '
        'btnCleaningStart
        '
        resources.ApplyResources(Me.btnCleaningStart, "btnCleaningStart")
        Me.btnCleaningStart.Name = "btnCleaningStart"
        Me.btnCleaningStart.UseVisualStyleBackColor = True
        '
        'txtProbingCnt
        '
        resources.ApplyResources(Me.txtProbingCnt, "txtProbingCnt")
        Me.txtProbingCnt.Name = "txtProbingCnt"
        '
        'txtContactCount
        '
        resources.ApplyResources(Me.txtContactCount, "txtContactCount")
        Me.txtContactCount.Name = "txtContactCount"
        '
        'LblRemark01
        '
        resources.ApplyResources(Me.LblRemark01, "LblRemark01")
        Me.LblRemark01.Name = "LblRemark01"
        '
        'LblProbingCnt
        '
        resources.ApplyResources(Me.LblProbingCnt, "LblProbingCnt")
        Me.LblProbingCnt.Name = "LblProbingCnt"
        '
        'LblContactCount
        '
        resources.ApplyResources(Me.LblContactCount, "LblContactCount")
        Me.LblContactCount.Name = "LblContactCount"
        '
        'grpBpOff
        '
        Me.grpBpOff.Controls.Add(Me.GroupBox2)
        Me.grpBpOff.Controls.Add(Me.GroupBox1)
        resources.ApplyResources(Me.grpBpOff, "grpBpOff")
        Me.grpBpOff.Name = "grpBpOff"
        Me.grpBpOff.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtRegPosX)
        Me.GroupBox2.Controls.Add(Me.txtRegPosZ)
        Me.GroupBox2.Controls.Add(Me.txtRegPosY)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label7)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'txtRegPosX
        '
        resources.ApplyResources(Me.txtRegPosX, "txtRegPosX")
        Me.txtRegPosX.Name = "txtRegPosX"
        Me.txtRegPosX.ReadOnly = True
        Me.txtRegPosX.TabStop = False
        '
        'txtRegPosZ
        '
        resources.ApplyResources(Me.txtRegPosZ, "txtRegPosZ")
        Me.txtRegPosZ.Name = "txtRegPosZ"
        Me.txtRegPosZ.ReadOnly = True
        Me.txtRegPosZ.TabStop = False
        '
        'txtRegPosY
        '
        resources.ApplyResources(Me.txtRegPosY, "txtRegPosY")
        Me.txtRegPosY.Name = "txtRegPosY"
        Me.txtRegPosY.ReadOnly = True
        Me.txtRegPosY.TabStop = False
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtStagePosX)
        Me.GroupBox1.Controls.Add(Me.txtZPos)
        Me.GroupBox1.Controls.Add(Me.txtStagePosY)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'txtStagePosX
        '
        resources.ApplyResources(Me.txtStagePosX, "txtStagePosX")
        Me.txtStagePosX.Name = "txtStagePosX"
        Me.txtStagePosX.ReadOnly = True
        Me.txtStagePosX.TabStop = False
        '
        'txtZPos
        '
        resources.ApplyResources(Me.txtZPos, "txtZPos")
        Me.txtZPos.Name = "txtZPos"
        Me.txtZPos.ReadOnly = True
        Me.txtZPos.TabStop = False
        '
        'txtStagePosY
        '
        resources.ApplyResources(Me.txtStagePosY, "txtStagePosY")
        Me.txtStagePosY.Name = "txtStagePosY"
        Me.txtStagePosY.ReadOnly = True
        Me.txtStagePosY.TabStop = False
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'LblHead01
        '
        resources.ApplyResources(Me.LblHead01, "LblHead01")
        Me.LblHead01.Name = "LblHead01"
        '
        'BtnTenKey
        '
        Me.BtnTenKey.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.BtnTenKey, "BtnTenKey")
        Me.BtnTenKey.Name = "BtnTenKey"
        Me.BtnTenKey.UseVisualStyleBackColor = False
        '
        'frmProbeCleaning
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.BtnTenKey)
        Me.Controls.Add(Me.GrpMain)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.btnZRegMove)
        Me.Controls.Add(Me.btnStageRegPosMove)
        Me.Controls.Add(Me.GrpArrow)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmProbeCleaning"
        Me.ShowInTaskbar = False
        Me.GrpArrow.ResumeLayout(False)
        Me.GrpArrow.PerformLayout()
        Me.GrpPithPanel.ResumeLayout(False)
        Me.GrpPithPanel.PerformLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpMain.ResumeLayout(False)
        Me.GrpMain.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.grpBpOff.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbMoveMode As System.Windows.Forms.ComboBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents btnZRegMove As System.Windows.Forms.Button
    Friend WithEvents btnStageRegPosMove As System.Windows.Forms.Button
    Friend WithEvents GrpMain As System.Windows.Forms.GroupBox
    Friend WithEvents LblMsg As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCleaningStart As System.Windows.Forms.Button
    Friend WithEvents txtProbingCnt As System.Windows.Forms.TextBox
    Friend WithEvents txtContactCount As System.Windows.Forms.TextBox
    Friend WithEvents LblRemark01 As System.Windows.Forms.Label
    Friend WithEvents LblProbingCnt As System.Windows.Forms.Label
    Friend WithEvents LblContactCount As System.Windows.Forms.Label
    Friend WithEvents grpBpOff As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtRegPosX As System.Windows.Forms.TextBox
    Friend WithEvents txtRegPosZ As System.Windows.Forms.TextBox
    Friend WithEvents txtRegPosY As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtStagePosX As System.Windows.Forms.TextBox
    Friend WithEvents txtZPos As System.Windows.Forms.TextBox
    Friend WithEvents txtStagePosY As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LblHead01 As System.Windows.Forms.Label
    Friend WithEvents LblOption2_2 As System.Windows.Forms.Label
    Friend WithEvents LblOption2_1 As System.Windows.Forms.Label
    Friend WithEvents txtOption2_Y As System.Windows.Forms.TextBox
    Friend WithEvents txtOption1_Y As System.Windows.Forms.TextBox
    Friend WithEvents txtOption2_X As System.Windows.Forms.TextBox
    Friend WithEvents LblOption1_2 As System.Windows.Forms.Label
    Friend WithEvents LblOption1_1 As System.Windows.Forms.Label
    Friend WithEvents txtOption1_X As System.Windows.Forms.TextBox
    Friend WithEvents BtnTenKey As System.Windows.Forms.Button
End Class
