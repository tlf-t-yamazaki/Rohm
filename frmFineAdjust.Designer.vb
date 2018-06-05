<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFineAdjust

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFineAdjust))
        Me.LblDIGSW_HI = New System.Windows.Forms.Label()
        Me.CbDigSwL = New System.Windows.Forms.ComboBox()
        Me.CbDigSwH = New System.Windows.Forms.ComboBox()
        Me.LblDIGSW = New System.Windows.Forms.Label()
        Me.btnTrimming = New System.Windows.Forms.Button()
        Me.grpBpOff = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtBpOffY = New System.Windows.Forms.TextBox()
        Me.txtBpOffX = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnBlkPrvMove = New System.Windows.Forms.Button()
        Me.btnBlkNextMove = New System.Windows.Forms.Button()
        Me.BlockMove = New System.Windows.Forms.GroupBox()
        Me.tlpBlockMove = New System.Windows.Forms.TableLayoutPanel()
        Me.cmbBlockMoveX = New System.Windows.Forms.ComboBox()
        Me.cmbBlockMoveY = New System.Windows.Forms.ComboBox()
        Me.lblBlockMoveX = New System.Windows.Forms.Label()
        Me.lblBlockMoveY = New System.Windows.Forms.Label()
        Me.BtnADJ = New System.Windows.Forms.Button()
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
        Me.TmKeyCheck = New System.Windows.Forms.Timer(Me.components)
        Me.BtnTenKey = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.GrpDistribute = New System.Windows.Forms.GroupBox()
        Me.chkDistributeOnOff = New System.Windows.Forms.CheckBox()
        Me.btnCounterClear = New System.Windows.Forms.Button()
        Me.BtnLaser = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblBlockNo = New System.Windows.Forms.Label()
        Me.grpBpOff.SuspendLayout()
        Me.BlockMove.SuspendLayout()
        Me.tlpBlockMove.SuspendLayout()
        Me.GrpArrow.SuspendLayout()
        Me.GrpPithPanel.SuspendLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpDistribute.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblDIGSW_HI
        '
        Me.LblDIGSW_HI.BackColor = System.Drawing.SystemColors.Control
        Me.LblDIGSW_HI.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblDIGSW_HI, "LblDIGSW_HI")
        Me.LblDIGSW_HI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDIGSW_HI.Name = "LblDIGSW_HI"
        '
        'CbDigSwL
        '
        Me.CbDigSwL.BackColor = System.Drawing.Color.PeachPuff
        Me.CbDigSwL.DisplayMember = "0"
        Me.CbDigSwL.DropDownHeight = 200
        Me.CbDigSwL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.CbDigSwL, "CbDigSwL")
        Me.CbDigSwL.FormattingEnabled = True
        Me.CbDigSwL.Items.AddRange(New Object() {resources.GetString("CbDigSwL.Items"), resources.GetString("CbDigSwL.Items1"), resources.GetString("CbDigSwL.Items2"), resources.GetString("CbDigSwL.Items3"), resources.GetString("CbDigSwL.Items4"), resources.GetString("CbDigSwL.Items5"), resources.GetString("CbDigSwL.Items6")})
        Me.CbDigSwL.Name = "CbDigSwL"
        '
        'CbDigSwH
        '
        Me.CbDigSwH.BackColor = System.Drawing.Color.PeachPuff
        Me.CbDigSwH.DropDownHeight = 80
        Me.CbDigSwH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.CbDigSwH, "CbDigSwH")
        Me.CbDigSwH.FormattingEnabled = True
        Me.CbDigSwH.Items.AddRange(New Object() {resources.GetString("CbDigSwH.Items"), resources.GetString("CbDigSwH.Items1"), resources.GetString("CbDigSwH.Items2")})
        Me.CbDigSwH.Name = "CbDigSwH"
        '
        'LblDIGSW
        '
        Me.LblDIGSW.BackColor = System.Drawing.SystemColors.Control
        Me.LblDIGSW.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LblDIGSW, "LblDIGSW")
        Me.LblDIGSW.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDIGSW.Name = "LblDIGSW"
        '
        'btnTrimming
        '
        Me.btnTrimming.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnTrimming.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnTrimming, "btnTrimming")
        Me.btnTrimming.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnTrimming.Name = "btnTrimming"
        Me.btnTrimming.UseVisualStyleBackColor = False
        '
        'grpBpOff
        '
        Me.grpBpOff.Controls.Add(Me.Label2)
        Me.grpBpOff.Controls.Add(Me.Label1)
        Me.grpBpOff.Controls.Add(Me.txtBpOffY)
        Me.grpBpOff.Controls.Add(Me.txtBpOffX)
        resources.ApplyResources(Me.grpBpOff, "grpBpOff")
        Me.grpBpOff.Name = "grpBpOff"
        Me.grpBpOff.TabStop = False
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'txtBpOffY
        '
        resources.ApplyResources(Me.txtBpOffY, "txtBpOffY")
        Me.txtBpOffY.Name = "txtBpOffY"
        Me.txtBpOffY.ReadOnly = True
        Me.txtBpOffY.TabStop = False
        '
        'txtBpOffX
        '
        resources.ApplyResources(Me.txtBpOffX, "txtBpOffX")
        Me.txtBpOffX.Name = "txtBpOffX"
        Me.txtBpOffX.ReadOnly = True
        Me.txtBpOffX.TabStop = False
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'btnBlkPrvMove
        '
        Me.btnBlkPrvMove.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.tlpBlockMove.SetColumnSpan(Me.btnBlkPrvMove, 2)
        resources.ApplyResources(Me.btnBlkPrvMove, "btnBlkPrvMove")
        Me.btnBlkPrvMove.Name = "btnBlkPrvMove"
        Me.btnBlkPrvMove.UseVisualStyleBackColor = False
        '
        'btnBlkNextMove
        '
        Me.btnBlkNextMove.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.tlpBlockMove.SetColumnSpan(Me.btnBlkNextMove, 2)
        resources.ApplyResources(Me.btnBlkNextMove, "btnBlkNextMove")
        Me.btnBlkNextMove.Name = "btnBlkNextMove"
        Me.btnBlkNextMove.UseVisualStyleBackColor = False
        '
        'BlockMove
        '
        resources.ApplyResources(Me.BlockMove, "BlockMove")
        Me.BlockMove.Controls.Add(Me.tlpBlockMove)
        Me.BlockMove.Name = "BlockMove"
        Me.BlockMove.TabStop = False
        '
        'tlpBlockMove
        '
        resources.ApplyResources(Me.tlpBlockMove, "tlpBlockMove")
        Me.tlpBlockMove.Controls.Add(Me.btnBlkPrvMove, 0, 0)
        Me.tlpBlockMove.Controls.Add(Me.btnBlkNextMove, 2, 0)
        Me.tlpBlockMove.Controls.Add(Me.cmbBlockMoveX, 1, 1)
        Me.tlpBlockMove.Controls.Add(Me.cmbBlockMoveY, 3, 1)
        Me.tlpBlockMove.Controls.Add(Me.lblBlockMoveX, 0, 1)
        Me.tlpBlockMove.Controls.Add(Me.lblBlockMoveY, 2, 1)
        Me.tlpBlockMove.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize
        Me.tlpBlockMove.Name = "tlpBlockMove"
        '
        'cmbBlockMoveX
        '
        Me.cmbBlockMoveX.BackColor = System.Drawing.Color.Cyan
        resources.ApplyResources(Me.cmbBlockMoveX, "cmbBlockMoveX")
        Me.cmbBlockMoveX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBlockMoveX.Name = "cmbBlockMoveX"
        '
        'cmbBlockMoveY
        '
        Me.cmbBlockMoveY.BackColor = System.Drawing.Color.Cyan
        resources.ApplyResources(Me.cmbBlockMoveY, "cmbBlockMoveY")
        Me.cmbBlockMoveY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBlockMoveY.Name = "cmbBlockMoveY"
        '
        'lblBlockMoveX
        '
        resources.ApplyResources(Me.lblBlockMoveX, "lblBlockMoveX")
        Me.lblBlockMoveX.Name = "lblBlockMoveX"
        '
        'lblBlockMoveY
        '
        resources.ApplyResources(Me.lblBlockMoveY, "lblBlockMoveY")
        Me.lblBlockMoveY.Name = "lblBlockMoveY"
        '
        'BtnADJ
        '
        resources.ApplyResources(Me.BtnADJ, "BtnADJ")
        Me.BtnADJ.Name = "BtnADJ"
        Me.BtnADJ.UseVisualStyleBackColor = True
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
        'TmKeyCheck
        '
        Me.TmKeyCheck.Interval = 3
        '
        'BtnTenKey
        '
        Me.BtnTenKey.BackColor = System.Drawing.Color.Pink
        resources.ApplyResources(Me.BtnTenKey, "BtnTenKey")
        Me.BtnTenKey.Name = "BtnTenKey"
        Me.BtnTenKey.UseVisualStyleBackColor = False
        '
        'BtnEdit
        '
        resources.ApplyResources(Me.BtnEdit, "BtnEdit")
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'GrpDistribute
        '
        Me.GrpDistribute.Controls.Add(Me.chkDistributeOnOff)
        Me.GrpDistribute.Controls.Add(Me.btnCounterClear)
        resources.ApplyResources(Me.GrpDistribute, "GrpDistribute")
        Me.GrpDistribute.Name = "GrpDistribute"
        Me.GrpDistribute.TabStop = False
        '
        'chkDistributeOnOff
        '
        resources.ApplyResources(Me.chkDistributeOnOff, "chkDistributeOnOff")
        Me.chkDistributeOnOff.Name = "chkDistributeOnOff"
        Me.chkDistributeOnOff.UseVisualStyleBackColor = True
        '
        'btnCounterClear
        '
        Me.btnCounterClear.BackColor = System.Drawing.Color.White
        Me.btnCounterClear.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnCounterClear, "btnCounterClear")
        Me.btnCounterClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCounterClear.Name = "btnCounterClear"
        Me.btnCounterClear.UseVisualStyleBackColor = False
        '
        'BtnLaser
        '
        Me.BtnLaser.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.BtnLaser, "BtnLaser")
        Me.BtnLaser.Name = "BtnLaser"
        Me.BtnLaser.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Name = "btnClose"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblBlockNo
        '
        resources.ApplyResources(Me.lblBlockNo, "lblBlockNo")
        Me.lblBlockNo.Name = "lblBlockNo"
        '
        'frmFineAdjust
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.lblBlockNo)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.BtnLaser)
        Me.Controls.Add(Me.GrpDistribute)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.BtnTenKey)
        Me.Controls.Add(Me.GrpArrow)
        Me.Controls.Add(Me.BtnADJ)
        Me.Controls.Add(Me.BlockMove)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.grpBpOff)
        Me.Controls.Add(Me.LblDIGSW_HI)
        Me.Controls.Add(Me.CbDigSwL)
        Me.Controls.Add(Me.CbDigSwH)
        Me.Controls.Add(Me.LblDIGSW)
        Me.Controls.Add(Me.btnTrimming)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmFineAdjust"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.grpBpOff.ResumeLayout(False)
        Me.grpBpOff.PerformLayout()
        Me.BlockMove.ResumeLayout(False)
        Me.BlockMove.PerformLayout()
        Me.tlpBlockMove.ResumeLayout(False)
        Me.GrpArrow.ResumeLayout(False)
        Me.GrpArrow.PerformLayout()
        Me.GrpPithPanel.ResumeLayout(False)
        Me.GrpPithPanel.PerformLayout()
        CType(Me.TBarPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarHiPitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TBarLowPitch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpDistribute.ResumeLayout(False)
        Me.GrpDistribute.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents LblDIGSW_HI As System.Windows.Forms.Label
    Friend WithEvents CbDigSwL As System.Windows.Forms.ComboBox
    Friend WithEvents CbDigSwH As System.Windows.Forms.ComboBox
    Public WithEvents LblDIGSW As System.Windows.Forms.Label
    Public WithEvents btnTrimming As System.Windows.Forms.Button
    Friend WithEvents grpBpOff As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtBpOffY As System.Windows.Forms.TextBox
    Friend WithEvents txtBpOffX As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnBlkPrvMove As System.Windows.Forms.Button
    Friend WithEvents btnBlkNextMove As System.Windows.Forms.Button
    Friend WithEvents BlockMove As System.Windows.Forms.GroupBox
    Friend WithEvents BtnADJ As System.Windows.Forms.Button
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
    Friend WithEvents TmKeyCheck As System.Windows.Forms.Timer
    Friend WithEvents BtnTenKey As System.Windows.Forms.Button
    Friend WithEvents BtnEdit As System.Windows.Forms.Button
    Friend WithEvents GrpDistribute As System.Windows.Forms.GroupBox
    Public WithEvents btnCounterClear As System.Windows.Forms.Button
    Friend WithEvents chkDistributeOnOff As System.Windows.Forms.CheckBox
    Friend WithEvents BtnLaser As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblBlockNo As System.Windows.Forms.Label
    Private WithEvents tlpBlockMove As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cmbBlockMoveX As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBlockMoveY As System.Windows.Forms.ComboBox
    Friend WithEvents lblBlockMoveX As System.Windows.Forms.Label
    Friend WithEvents lblBlockMoveY As System.Windows.Forms.Label
End Class
