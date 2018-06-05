<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmDistribution
#Region "Windows フォーム デザイナによって生成されたコード "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'この呼び出しは、Windows フォーム デザイナで必要です。
		InitializeComponent()
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
    Public WithEvents cmdGraphSave As System.Windows.Forms.Button
	Public WithEvents cmdFinal As System.Windows.Forms.Button
	Public WithEvents cmdInitial As System.Windows.Forms.Button
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDistribution))
        Me.cmdGraphSave = New System.Windows.Forms.Button()
        Me.cmdFinal = New System.Windows.Forms.Button()
        Me.cmdInitial = New System.Windows.Forms.Button()
        Me.lblRegistUnit = New System.Windows.Forms.Label()
        Me.lblRegistTitle = New System.Windows.Forms.Label()
        Me.lblGraphUnit = New System.Windows.Forms.Label()
        Me.lblGraphAccumulationTitle = New System.Windows.Forms.Label()
        Me.Line1 = New System.Windows.Forms.Label()
        Me.Line2 = New System.Windows.Forms.Label()
        Me.Line3 = New System.Windows.Forms.Label()
        Me.LblRegN_00 = New System.Windows.Forms.Label()
        Me.LblRegN_01 = New System.Windows.Forms.Label()
        Me.LblRegN_02 = New System.Windows.Forms.Label()
        Me.LblRegN_03 = New System.Windows.Forms.Label()
        Me.LblRegN_04 = New System.Windows.Forms.Label()
        Me.LblRegN_05 = New System.Windows.Forms.Label()
        Me.LblRegN_06 = New System.Windows.Forms.Label()
        Me.LblRegN_07 = New System.Windows.Forms.Label()
        Me.LblRegN_08 = New System.Windows.Forms.Label()
        Me.LblRegN_09 = New System.Windows.Forms.Label()
        Me.LblRegN_10 = New System.Windows.Forms.Label()
        Me.LblRegN_11 = New System.Windows.Forms.Label()
        Me.LblGrpPer_00 = New System.Windows.Forms.Label()
        Me.LblGrpPer_01 = New System.Windows.Forms.Label()
        Me.LblGrpPer_02 = New System.Windows.Forms.Label()
        Me.LblGrpPer_03 = New System.Windows.Forms.Label()
        Me.LblGrpPer_04 = New System.Windows.Forms.Label()
        Me.LblGrpPer_05 = New System.Windows.Forms.Label()
        Me.LblGrpPer_06 = New System.Windows.Forms.Label()
        Me.LblGrpPer_07 = New System.Windows.Forms.Label()
        Me.LblGrpPer_08 = New System.Windows.Forms.Label()
        Me.LblGrpPer_09 = New System.Windows.Forms.Label()
        Me.LblGrpPer_10 = New System.Windows.Forms.Label()
        Me.LblGrpPer_11 = New System.Windows.Forms.Label()
        Me.LblShpGrp_00 = New System.Windows.Forms.Label()
        Me.LblShpGrp_01 = New System.Windows.Forms.Label()
        Me.LblShpGrp_02 = New System.Windows.Forms.Label()
        Me.LblShpGrp_03 = New System.Windows.Forms.Label()
        Me.LblShpGrp_04 = New System.Windows.Forms.Label()
        Me.LblShpGrp_05 = New System.Windows.Forms.Label()
        Me.LblShpGrp_06 = New System.Windows.Forms.Label()
        Me.LblShpGrp_07 = New System.Windows.Forms.Label()
        Me.LblShpGrp_08 = New System.Windows.Forms.Label()
        Me.LblShpGrp_09 = New System.Windows.Forms.Label()
        Me.LblShpGrp_10 = New System.Windows.Forms.Label()
        Me.LblShpGrp_11 = New System.Windows.Forms.Label()
        Me.lblMinValue = New System.Windows.Forms.Label()
        Me.lblMaxValue = New System.Windows.Forms.Label()
        Me.lblAverageValue = New System.Windows.Forms.Label()
        Me.lblDeviationValue = New System.Windows.Forms.Label()
        Me.lblNgChip = New System.Windows.Forms.Label()
        Me.lblGoodChip = New System.Windows.Forms.Label()
        Me.lblDeviation = New System.Windows.Forms.Label()
        Me.lblAverage = New System.Windows.Forms.Label()
        Me.lblMaxTitle = New System.Windows.Forms.Label()
        Me.lblMinTitle = New System.Windows.Forms.Label()
        Me.lblNgTitle = New System.Windows.Forms.Label()
        Me.lblGoodTitle = New System.Windows.Forms.Label()
        Me.picGraphAccumulation = New System.Windows.Forms.Panel()
        Me.picGraphAccumulation.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdGraphSave
        '
        resources.ApplyResources(Me.cmdGraphSave, "cmdGraphSave")
        Me.cmdGraphSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGraphSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGraphSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGraphSave.Name = "cmdGraphSave"
        Me.cmdGraphSave.UseVisualStyleBackColor = False
        '
        'cmdFinal
        '
        resources.ApplyResources(Me.cmdFinal, "cmdFinal")
        Me.cmdFinal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFinal.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFinal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFinal.Name = "cmdFinal"
        Me.cmdFinal.UseVisualStyleBackColor = False
        '
        'cmdInitial
        '
        resources.ApplyResources(Me.cmdInitial, "cmdInitial")
        Me.cmdInitial.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInitial.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInitial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInitial.Name = "cmdInitial"
        Me.cmdInitial.UseVisualStyleBackColor = False
        '
        'lblRegistUnit
        '
        resources.ApplyResources(Me.lblRegistUnit, "lblRegistUnit")
        Me.lblRegistUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblRegistUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegistUnit.ForeColor = System.Drawing.Color.Lime
        Me.lblRegistUnit.Name = "lblRegistUnit"
        '
        'lblRegistTitle
        '
        resources.ApplyResources(Me.lblRegistTitle, "lblRegistTitle")
        Me.lblRegistTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblRegistTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegistTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblRegistTitle.Name = "lblRegistTitle"
        '
        'lblGraphUnit
        '
        resources.ApplyResources(Me.lblGraphUnit, "lblGraphUnit")
        Me.lblGraphUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblGraphUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGraphUnit.ForeColor = System.Drawing.Color.Lime
        Me.lblGraphUnit.Name = "lblGraphUnit"
        '
        'lblGraphAccumulationTitle
        '
        resources.ApplyResources(Me.lblGraphAccumulationTitle, "lblGraphAccumulationTitle")
        Me.lblGraphAccumulationTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblGraphAccumulationTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGraphAccumulationTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblGraphAccumulationTitle.Name = "lblGraphAccumulationTitle"
        '
        'Line1
        '
        resources.ApplyResources(Me.Line1, "Line1")
        Me.Line1.BackColor = System.Drawing.Color.Green
        Me.Line1.Name = "Line1"
        '
        'Line2
        '
        resources.ApplyResources(Me.Line2, "Line2")
        Me.Line2.BackColor = System.Drawing.Color.Green
        Me.Line2.Name = "Line2"
        '
        'Line3
        '
        resources.ApplyResources(Me.Line3, "Line3")
        Me.Line3.BackColor = System.Drawing.Color.Green
        Me.Line3.Name = "Line3"
        '
        'LblRegN_00
        '
        resources.ApplyResources(Me.LblRegN_00, "LblRegN_00")
        Me.LblRegN_00.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_00.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_00.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_00.Name = "LblRegN_00"
        '
        'LblRegN_01
        '
        resources.ApplyResources(Me.LblRegN_01, "LblRegN_01")
        Me.LblRegN_01.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_01.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_01.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_01.Name = "LblRegN_01"
        '
        'LblRegN_02
        '
        resources.ApplyResources(Me.LblRegN_02, "LblRegN_02")
        Me.LblRegN_02.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_02.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_02.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_02.Name = "LblRegN_02"
        '
        'LblRegN_03
        '
        resources.ApplyResources(Me.LblRegN_03, "LblRegN_03")
        Me.LblRegN_03.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_03.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_03.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_03.Name = "LblRegN_03"
        '
        'LblRegN_04
        '
        resources.ApplyResources(Me.LblRegN_04, "LblRegN_04")
        Me.LblRegN_04.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_04.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_04.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_04.Name = "LblRegN_04"
        '
        'LblRegN_05
        '
        resources.ApplyResources(Me.LblRegN_05, "LblRegN_05")
        Me.LblRegN_05.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_05.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_05.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_05.Name = "LblRegN_05"
        '
        'LblRegN_06
        '
        resources.ApplyResources(Me.LblRegN_06, "LblRegN_06")
        Me.LblRegN_06.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_06.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_06.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_06.Name = "LblRegN_06"
        '
        'LblRegN_07
        '
        resources.ApplyResources(Me.LblRegN_07, "LblRegN_07")
        Me.LblRegN_07.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_07.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_07.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_07.Name = "LblRegN_07"
        '
        'LblRegN_08
        '
        resources.ApplyResources(Me.LblRegN_08, "LblRegN_08")
        Me.LblRegN_08.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_08.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_08.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_08.Name = "LblRegN_08"
        '
        'LblRegN_09
        '
        resources.ApplyResources(Me.LblRegN_09, "LblRegN_09")
        Me.LblRegN_09.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_09.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_09.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_09.Name = "LblRegN_09"
        '
        'LblRegN_10
        '
        resources.ApplyResources(Me.LblRegN_10, "LblRegN_10")
        Me.LblRegN_10.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_10.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_10.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_10.Name = "LblRegN_10"
        '
        'LblRegN_11
        '
        resources.ApplyResources(Me.LblRegN_11, "LblRegN_11")
        Me.LblRegN_11.BackColor = System.Drawing.Color.Transparent
        Me.LblRegN_11.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblRegN_11.ForeColor = System.Drawing.Color.Lime
        Me.LblRegN_11.Name = "LblRegN_11"
        '
        'LblGrpPer_00
        '
        resources.ApplyResources(Me.LblGrpPer_00, "LblGrpPer_00")
        Me.LblGrpPer_00.BackColor = System.Drawing.Color.Black
        Me.LblGrpPer_00.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_00.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_00.Name = "LblGrpPer_00"
        '
        'LblGrpPer_01
        '
        resources.ApplyResources(Me.LblGrpPer_01, "LblGrpPer_01")
        Me.LblGrpPer_01.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_01.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_01.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_01.Name = "LblGrpPer_01"
        '
        'LblGrpPer_02
        '
        resources.ApplyResources(Me.LblGrpPer_02, "LblGrpPer_02")
        Me.LblGrpPer_02.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_02.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_02.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_02.Name = "LblGrpPer_02"
        '
        'LblGrpPer_03
        '
        resources.ApplyResources(Me.LblGrpPer_03, "LblGrpPer_03")
        Me.LblGrpPer_03.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_03.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_03.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_03.Name = "LblGrpPer_03"
        '
        'LblGrpPer_04
        '
        resources.ApplyResources(Me.LblGrpPer_04, "LblGrpPer_04")
        Me.LblGrpPer_04.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_04.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_04.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_04.Name = "LblGrpPer_04"
        '
        'LblGrpPer_05
        '
        resources.ApplyResources(Me.LblGrpPer_05, "LblGrpPer_05")
        Me.LblGrpPer_05.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_05.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_05.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_05.Name = "LblGrpPer_05"
        '
        'LblGrpPer_06
        '
        resources.ApplyResources(Me.LblGrpPer_06, "LblGrpPer_06")
        Me.LblGrpPer_06.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_06.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_06.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_06.Name = "LblGrpPer_06"
        '
        'LblGrpPer_07
        '
        resources.ApplyResources(Me.LblGrpPer_07, "LblGrpPer_07")
        Me.LblGrpPer_07.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_07.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_07.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_07.Name = "LblGrpPer_07"
        '
        'LblGrpPer_08
        '
        resources.ApplyResources(Me.LblGrpPer_08, "LblGrpPer_08")
        Me.LblGrpPer_08.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_08.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_08.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_08.Name = "LblGrpPer_08"
        '
        'LblGrpPer_09
        '
        resources.ApplyResources(Me.LblGrpPer_09, "LblGrpPer_09")
        Me.LblGrpPer_09.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_09.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_09.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_09.Name = "LblGrpPer_09"
        '
        'LblGrpPer_10
        '
        resources.ApplyResources(Me.LblGrpPer_10, "LblGrpPer_10")
        Me.LblGrpPer_10.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_10.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_10.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_10.Name = "LblGrpPer_10"
        '
        'LblGrpPer_11
        '
        resources.ApplyResources(Me.LblGrpPer_11, "LblGrpPer_11")
        Me.LblGrpPer_11.BackColor = System.Drawing.Color.Transparent
        Me.LblGrpPer_11.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblGrpPer_11.ForeColor = System.Drawing.Color.Lime
        Me.LblGrpPer_11.Name = "LblGrpPer_11"
        '
        'LblShpGrp_00
        '
        resources.ApplyResources(Me.LblShpGrp_00, "LblShpGrp_00")
        Me.LblShpGrp_00.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_00.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_00.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_00.Name = "LblShpGrp_00"
        '
        'LblShpGrp_01
        '
        resources.ApplyResources(Me.LblShpGrp_01, "LblShpGrp_01")
        Me.LblShpGrp_01.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_01.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_01.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_01.Name = "LblShpGrp_01"
        '
        'LblShpGrp_02
        '
        resources.ApplyResources(Me.LblShpGrp_02, "LblShpGrp_02")
        Me.LblShpGrp_02.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_02.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_02.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_02.Name = "LblShpGrp_02"
        '
        'LblShpGrp_03
        '
        resources.ApplyResources(Me.LblShpGrp_03, "LblShpGrp_03")
        Me.LblShpGrp_03.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_03.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_03.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_03.Name = "LblShpGrp_03"
        '
        'LblShpGrp_04
        '
        resources.ApplyResources(Me.LblShpGrp_04, "LblShpGrp_04")
        Me.LblShpGrp_04.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_04.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_04.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_04.Name = "LblShpGrp_04"
        '
        'LblShpGrp_05
        '
        resources.ApplyResources(Me.LblShpGrp_05, "LblShpGrp_05")
        Me.LblShpGrp_05.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_05.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_05.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_05.Name = "LblShpGrp_05"
        '
        'LblShpGrp_06
        '
        resources.ApplyResources(Me.LblShpGrp_06, "LblShpGrp_06")
        Me.LblShpGrp_06.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_06.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_06.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_06.Name = "LblShpGrp_06"
        '
        'LblShpGrp_07
        '
        resources.ApplyResources(Me.LblShpGrp_07, "LblShpGrp_07")
        Me.LblShpGrp_07.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_07.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_07.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_07.Name = "LblShpGrp_07"
        '
        'LblShpGrp_08
        '
        resources.ApplyResources(Me.LblShpGrp_08, "LblShpGrp_08")
        Me.LblShpGrp_08.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_08.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_08.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_08.Name = "LblShpGrp_08"
        '
        'LblShpGrp_09
        '
        resources.ApplyResources(Me.LblShpGrp_09, "LblShpGrp_09")
        Me.LblShpGrp_09.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_09.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_09.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_09.Name = "LblShpGrp_09"
        '
        'LblShpGrp_10
        '
        resources.ApplyResources(Me.LblShpGrp_10, "LblShpGrp_10")
        Me.LblShpGrp_10.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_10.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_10.Name = "LblShpGrp_10"
        '
        'LblShpGrp_11
        '
        resources.ApplyResources(Me.LblShpGrp_11, "LblShpGrp_11")
        Me.LblShpGrp_11.BackColor = System.Drawing.Color.Cyan
        Me.LblShpGrp_11.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblShpGrp_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblShpGrp_11.Name = "LblShpGrp_11"
        '
        'lblMinValue
        '
        resources.ApplyResources(Me.lblMinValue, "lblMinValue")
        Me.lblMinValue.BackColor = System.Drawing.Color.Transparent
        Me.lblMinValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinValue.ForeColor = System.Drawing.Color.Lime
        Me.lblMinValue.Name = "lblMinValue"
        '
        'lblMaxValue
        '
        resources.ApplyResources(Me.lblMaxValue, "lblMaxValue")
        Me.lblMaxValue.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxValue.ForeColor = System.Drawing.Color.Lime
        Me.lblMaxValue.Name = "lblMaxValue"
        '
        'lblAverageValue
        '
        resources.ApplyResources(Me.lblAverageValue, "lblAverageValue")
        Me.lblAverageValue.BackColor = System.Drawing.Color.Transparent
        Me.lblAverageValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAverageValue.ForeColor = System.Drawing.Color.Lime
        Me.lblAverageValue.Name = "lblAverageValue"
        '
        'lblDeviationValue
        '
        resources.ApplyResources(Me.lblDeviationValue, "lblDeviationValue")
        Me.lblDeviationValue.BackColor = System.Drawing.Color.Transparent
        Me.lblDeviationValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeviationValue.ForeColor = System.Drawing.Color.Lime
        Me.lblDeviationValue.Name = "lblDeviationValue"
        '
        'lblNgChip
        '
        resources.ApplyResources(Me.lblNgChip, "lblNgChip")
        Me.lblNgChip.BackColor = System.Drawing.Color.Transparent
        Me.lblNgChip.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNgChip.ForeColor = System.Drawing.Color.Lime
        Me.lblNgChip.Name = "lblNgChip"
        '
        'lblGoodChip
        '
        resources.ApplyResources(Me.lblGoodChip, "lblGoodChip")
        Me.lblGoodChip.BackColor = System.Drawing.Color.Transparent
        Me.lblGoodChip.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGoodChip.ForeColor = System.Drawing.Color.Lime
        Me.lblGoodChip.Name = "lblGoodChip"
        '
        'lblDeviation
        '
        resources.ApplyResources(Me.lblDeviation, "lblDeviation")
        Me.lblDeviation.BackColor = System.Drawing.Color.Transparent
        Me.lblDeviation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeviation.ForeColor = System.Drawing.Color.Lime
        Me.lblDeviation.Name = "lblDeviation"
        '
        'lblAverage
        '
        resources.ApplyResources(Me.lblAverage, "lblAverage")
        Me.lblAverage.BackColor = System.Drawing.Color.Transparent
        Me.lblAverage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAverage.ForeColor = System.Drawing.Color.Lime
        Me.lblAverage.Name = "lblAverage"
        '
        'lblMaxTitle
        '
        resources.ApplyResources(Me.lblMaxTitle, "lblMaxTitle")
        Me.lblMaxTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblMaxTitle.Name = "lblMaxTitle"
        '
        'lblMinTitle
        '
        resources.ApplyResources(Me.lblMinTitle, "lblMinTitle")
        Me.lblMinTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblMinTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblMinTitle.Name = "lblMinTitle"
        '
        'lblNgTitle
        '
        resources.ApplyResources(Me.lblNgTitle, "lblNgTitle")
        Me.lblNgTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblNgTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNgTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblNgTitle.Name = "lblNgTitle"
        '
        'lblGoodTitle
        '
        resources.ApplyResources(Me.lblGoodTitle, "lblGoodTitle")
        Me.lblGoodTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblGoodTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGoodTitle.ForeColor = System.Drawing.Color.Lime
        Me.lblGoodTitle.Name = "lblGoodTitle"
        '
        'picGraphAccumulation
        '
        resources.ApplyResources(Me.picGraphAccumulation, "picGraphAccumulation")
        Me.picGraphAccumulation.BackColor = System.Drawing.Color.Black
        Me.picGraphAccumulation.Controls.Add(Me.cmdFinal)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_00)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_00)
        Me.picGraphAccumulation.Controls.Add(Me.cmdGraphSave)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_00)
        Me.picGraphAccumulation.Controls.Add(Me.cmdInitial)
        Me.picGraphAccumulation.Controls.Add(Me.lblGoodTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblNgTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblMinTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblMaxTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblAverage)
        Me.picGraphAccumulation.Controls.Add(Me.lblDeviation)
        Me.picGraphAccumulation.Controls.Add(Me.lblGoodChip)
        Me.picGraphAccumulation.Controls.Add(Me.lblNgChip)
        Me.picGraphAccumulation.Controls.Add(Me.lblDeviationValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblAverageValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblMaxValue)
        Me.picGraphAccumulation.Controls.Add(Me.lblMinValue)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_11)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_10)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_09)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_08)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_07)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_06)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_05)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_04)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_03)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_02)
        Me.picGraphAccumulation.Controls.Add(Me.LblShpGrp_01)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_11)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_10)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_09)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_08)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_07)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_06)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_05)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_04)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_03)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_02)
        Me.picGraphAccumulation.Controls.Add(Me.LblGrpPer_01)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_11)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_10)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_09)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_08)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_07)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_06)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_05)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_04)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_03)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_02)
        Me.picGraphAccumulation.Controls.Add(Me.LblRegN_01)
        Me.picGraphAccumulation.Controls.Add(Me.Line3)
        Me.picGraphAccumulation.Controls.Add(Me.Line2)
        Me.picGraphAccumulation.Controls.Add(Me.Line1)
        Me.picGraphAccumulation.Controls.Add(Me.lblGraphUnit)
        Me.picGraphAccumulation.Controls.Add(Me.lblRegistTitle)
        Me.picGraphAccumulation.Controls.Add(Me.lblRegistUnit)
        Me.picGraphAccumulation.Controls.Add(Me.lblGraphAccumulationTitle)
        Me.picGraphAccumulation.Cursor = System.Windows.Forms.Cursors.Default
        Me.picGraphAccumulation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.picGraphAccumulation.Name = "picGraphAccumulation"
        Me.picGraphAccumulation.TabStop = True
        '
        'frmDistribution
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ControlBox = False
        Me.Controls.Add(Me.picGraphAccumulation)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDistribution"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.picGraphAccumulation.ResumeLayout(False)
        Me.picGraphAccumulation.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lblRegistUnit As System.Windows.Forms.Label
    Public WithEvents lblRegistTitle As System.Windows.Forms.Label
    Public WithEvents lblGraphUnit As System.Windows.Forms.Label
    Public WithEvents lblGraphAccumulationTitle As System.Windows.Forms.Label
    Public WithEvents Line1 As System.Windows.Forms.Label
    Public WithEvents Line2 As System.Windows.Forms.Label
    Public WithEvents Line3 As System.Windows.Forms.Label
    Public WithEvents LblRegN_00 As System.Windows.Forms.Label
    Public WithEvents LblRegN_01 As System.Windows.Forms.Label
    Public WithEvents LblRegN_02 As System.Windows.Forms.Label
    Public WithEvents LblRegN_03 As System.Windows.Forms.Label
    Public WithEvents LblRegN_04 As System.Windows.Forms.Label
    Public WithEvents LblRegN_05 As System.Windows.Forms.Label
    Public WithEvents LblRegN_06 As System.Windows.Forms.Label
    Public WithEvents LblRegN_07 As System.Windows.Forms.Label
    Public WithEvents LblRegN_08 As System.Windows.Forms.Label
    Public WithEvents LblRegN_09 As System.Windows.Forms.Label
    Public WithEvents LblRegN_10 As System.Windows.Forms.Label
    Public WithEvents LblRegN_11 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_00 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_01 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_02 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_03 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_04 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_05 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_06 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_07 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_08 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_09 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_10 As System.Windows.Forms.Label
    Public WithEvents LblGrpPer_11 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_00 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_01 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_02 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_03 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_04 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_05 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_06 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_07 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_08 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_09 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_10 As System.Windows.Forms.Label
    Public WithEvents LblShpGrp_11 As System.Windows.Forms.Label
    Public WithEvents lblMinValue As System.Windows.Forms.Label
    Public WithEvents lblMaxValue As System.Windows.Forms.Label
    Public WithEvents lblAverageValue As System.Windows.Forms.Label
    Public WithEvents lblDeviationValue As System.Windows.Forms.Label
    Public WithEvents lblNgChip As System.Windows.Forms.Label
    Public WithEvents lblGoodChip As System.Windows.Forms.Label
    Public WithEvents lblDeviation As System.Windows.Forms.Label
    Public WithEvents lblAverage As System.Windows.Forms.Label
    Public WithEvents lblMaxTitle As System.Windows.Forms.Label
    Public WithEvents lblMinTitle As System.Windows.Forms.Label
    Public WithEvents lblNgTitle As System.Windows.Forms.Label
    Public WithEvents lblGoodTitle As System.Windows.Forms.Label
    Public WithEvents picGraphAccumulation As System.Windows.Forms.Panel
#End Region
End Class