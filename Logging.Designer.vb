<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Logging
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
    Public WithEvents CmdLogTyp As System.Windows.Forms.Button
	Public comDlgSave As System.Windows.Forms.SaveFileDialog
	Public WithEvents Check2 As System.Windows.Forms.CheckBox
	Public WithEvents chkResultAdd As System.Windows.Forms.CheckBox
	Public WithEvents Combo2 As System.Windows.Forms.ComboBox
	Public WithEvents chkLogFinal As System.Windows.Forms.CheckBox
	Public WithEvents chkLogInitial As System.Windows.Forms.CheckBox
    Public WithEvents Command3 As System.Windows.Forms.Button
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents Check1 As System.Windows.Forms.CheckBox
    Public WithEvents lblLogBaseTitle As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents Command2 As System.Windows.Forms.Button
	Public WithEvents Command1 As System.Windows.Forms.Button
    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更できます。
    'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Logging))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.CmdLogTyp = New System.Windows.Forms.Button()
        Me.Check2 = New System.Windows.Forms.CheckBox()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.lblLogBaseTitle = New System.Windows.Forms.Label()
        Me.LblLogType = New System.Windows.Forms.Label()
        Me.LblLotNo = New System.Windows.Forms.Label()
        Me.UDLotNum = New System.Windows.Forms.NumericUpDown()
        Me.chkResultAdd = New System.Windows.Forms.CheckBox()
        Me.Combo2 = New System.Windows.Forms.ComboBox()
        Me.chkLogFinal = New System.Windows.Forms.CheckBox()
        Me.chkLogInitial = New System.Windows.Forms.CheckBox()
        Me.Command3 = New System.Windows.Forms.Button()
        Me.Text1 = New System.Windows.Forms.TextBox()
        Me.Check1 = New System.Windows.Forms.CheckBox()
        Me.Command2 = New System.Windows.Forms.Button()
        Me.Command1 = New System.Windows.Forms.Button()
        Me.chkOptDisp = New System.Windows.Forms.CheckBox()
        Me.comDlgSave = New System.Windows.Forms.SaveFileDialog()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DataGridViewMeasureLog = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTrimmingLog = New System.Windows.Forms.DataGridView()
        Me.btnSetLogTarget = New System.Windows.Forms.Button()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Frame1.SuspendLayout()
        CType(Me.UDLotNum, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridViewMeasureLog, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewTrimmingLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CmdLogTyp
        '
        Me.CmdLogTyp.BackColor = System.Drawing.Color.Green
        Me.CmdLogTyp.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdLogTyp.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.CmdLogTyp, "CmdLogTyp")
        Me.CmdLogTyp.Name = "CmdLogTyp"
        Me.CmdLogTyp.UseVisualStyleBackColor = False
        '
        'Check2
        '
        Me.Check2.BackColor = System.Drawing.SystemColors.Control
        Me.Check2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check2.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Check2, "Check2")
        Me.Check2.Name = "Check2"
        Me.Check2.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.lblLogBaseTitle)
        Me.Frame1.Controls.Add(Me.LblLogType)
        Me.Frame1.Controls.Add(Me.LblLotNo)
        Me.Frame1.Controls.Add(Me.UDLotNum)
        Me.Frame1.Controls.Add(Me.Check2)
        Me.Frame1.Controls.Add(Me.chkResultAdd)
        Me.Frame1.Controls.Add(Me.Combo2)
        Me.Frame1.Controls.Add(Me.chkLogFinal)
        Me.Frame1.Controls.Add(Me.chkLogInitial)
        Me.Frame1.Controls.Add(Me.Command3)
        Me.Frame1.Controls.Add(Me.Text1)
        Me.Frame1.Controls.Add(Me.Check1)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'lblLogBaseTitle
        '
        Me.lblLogBaseTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblLogBaseTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLogBaseTitle.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.lblLogBaseTitle, "lblLogBaseTitle")
        Me.lblLogBaseTitle.Name = "lblLogBaseTitle"
        '
        'LblLogType
        '
        resources.ApplyResources(Me.LblLogType, "LblLogType")
        Me.LblLogType.Name = "LblLogType"
        '
        'LblLotNo
        '
        resources.ApplyResources(Me.LblLotNo, "LblLotNo")
        Me.LblLotNo.Name = "LblLotNo"
        '
        'UDLotNum
        '
        resources.ApplyResources(Me.UDLotNum, "UDLotNum")
        Me.UDLotNum.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.UDLotNum.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.UDLotNum.Name = "UDLotNum"
        Me.UDLotNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'chkResultAdd
        '
        Me.chkResultAdd.BackColor = System.Drawing.SystemColors.Control
        Me.chkResultAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkResultAdd.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.chkResultAdd, "chkResultAdd")
        Me.chkResultAdd.Name = "chkResultAdd"
        Me.chkResultAdd.UseVisualStyleBackColor = False
        '
        'Combo2
        '
        Me.Combo2.BackColor = System.Drawing.SystemColors.Window
        Me.Combo2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Combo2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo2.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.Combo2, "Combo2")
        Me.Combo2.Name = "Combo2"
        '
        'chkLogFinal
        '
        Me.chkLogFinal.BackColor = System.Drawing.SystemColors.Control
        Me.chkLogFinal.Checked = True
        Me.chkLogFinal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLogFinal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLogFinal.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.chkLogFinal, "chkLogFinal")
        Me.chkLogFinal.Name = "chkLogFinal"
        Me.chkLogFinal.UseVisualStyleBackColor = False
        '
        'chkLogInitial
        '
        Me.chkLogInitial.BackColor = System.Drawing.SystemColors.Control
        Me.chkLogInitial.Checked = True
        Me.chkLogInitial.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLogInitial.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLogInitial.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.chkLogInitial, "chkLogInitial")
        Me.chkLogInitial.Name = "chkLogInitial"
        Me.chkLogInitial.UseVisualStyleBackColor = False
        '
        'Command3
        '
        Me.Command3.BackColor = System.Drawing.SystemColors.Control
        Me.Command3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command3.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Command3, "Command3")
        Me.Command3.Name = "Command3"
        Me.Command3.UseVisualStyleBackColor = False
        '
        'Text1
        '
        Me.Text1.AcceptsReturn = True
        Me.Text1.BackColor = System.Drawing.SystemColors.Control
        Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.Text1, "Text1")
        Me.Text1.Name = "Text1"
        Me.Text1.ReadOnly = True
        '
        'Check1
        '
        Me.Check1.BackColor = System.Drawing.SystemColors.Control
        Me.Check1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check1.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Check1, "Check1")
        Me.Check1.Name = "Check1"
        Me.Check1.UseVisualStyleBackColor = False
        '
        'Command2
        '
        Me.Command2.BackColor = System.Drawing.SystemColors.Control
        Me.Command2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command2.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Command2, "Command2")
        Me.Command2.Name = "Command2"
        Me.Command2.UseVisualStyleBackColor = False
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Me.Command1, "Command1")
        Me.Command1.Name = "Command1"
        Me.Command1.UseVisualStyleBackColor = False
        '
        'chkOptDisp
        '
        resources.ApplyResources(Me.chkOptDisp, "chkOptDisp")
        Me.chkOptDisp.Name = "chkOptDisp"
        Me.chkOptDisp.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DataGridViewMeasureLog)
        Me.GroupBox2.Controls.Add(Me.DataGridViewTrimmingLog)
        Me.GroupBox2.Controls.Add(Me.btnSetLogTarget)
        Me.GroupBox2.Controls.Add(Me.Label26)
        Me.GroupBox2.Controls.Add(Me.Label25)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'DataGridViewMeasureLog
        '
        Me.DataGridViewMeasureLog.AllowUserToAddRows = False
        Me.DataGridViewMeasureLog.AllowUserToDeleteRows = False
        Me.DataGridViewMeasureLog.AllowUserToResizeColumns = False
        Me.DataGridViewMeasureLog.AllowUserToResizeRows = False
        Me.DataGridViewMeasureLog.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridViewMeasureLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewMeasureLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        resources.ApplyResources(Me.DataGridViewMeasureLog, "DataGridViewMeasureLog")
        Me.DataGridViewMeasureLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewMeasureLog.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewMeasureLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridViewMeasureLog.EnableHeadersVisualStyles = False
        Me.DataGridViewMeasureLog.MultiSelect = False
        Me.DataGridViewMeasureLog.Name = "DataGridViewMeasureLog"
        Me.DataGridViewMeasureLog.RowHeadersVisible = False
        Me.DataGridViewMeasureLog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridViewMeasureLog.RowTemplate.Height = 20
        Me.DataGridViewMeasureLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewMeasureLog.ShowCellErrors = False
        Me.DataGridViewMeasureLog.ShowEditingIcon = False
        Me.DataGridViewMeasureLog.ShowRowErrors = False
        '
        'DataGridViewTrimmingLog
        '
        Me.DataGridViewTrimmingLog.AllowUserToAddRows = False
        Me.DataGridViewTrimmingLog.AllowUserToDeleteRows = False
        Me.DataGridViewTrimmingLog.AllowUserToResizeColumns = False
        Me.DataGridViewTrimmingLog.AllowUserToResizeRows = False
        Me.DataGridViewTrimmingLog.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridViewTrimmingLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTrimmingLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        resources.ApplyResources(Me.DataGridViewTrimmingLog, "DataGridViewTrimmingLog")
        Me.DataGridViewTrimmingLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTrimmingLog.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTrimmingLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridViewTrimmingLog.EnableHeadersVisualStyles = False
        Me.DataGridViewTrimmingLog.MultiSelect = False
        Me.DataGridViewTrimmingLog.Name = "DataGridViewTrimmingLog"
        Me.DataGridViewTrimmingLog.RowHeadersVisible = False
        Me.DataGridViewTrimmingLog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridViewTrimmingLog.RowTemplate.Height = 20
        Me.DataGridViewTrimmingLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewTrimmingLog.ShowCellErrors = False
        Me.DataGridViewTrimmingLog.ShowEditingIcon = False
        Me.DataGridViewTrimmingLog.ShowRowErrors = False
        '
        'btnSetLogTarget
        '
        resources.ApplyResources(Me.btnSetLogTarget, "btnSetLogTarget")
        Me.btnSetLogTarget.Name = "btnSetLogTarget"
        Me.btnSetLogTarget.UseVisualStyleBackColor = True
        '
        'Label26
        '
        resources.ApplyResources(Me.Label26, "Label26")
        Me.Label26.Name = "Label26"
        '
        'Label25
        '
        resources.ApplyResources(Me.Label25, "Label25")
        Me.Label25.Name = "Label25"
        '
        'Logging
        '
        Me.AcceptButton = Me.Command2
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.chkOptDisp)
        Me.Controls.Add(Me.CmdLogTyp)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.Command2)
        Me.Controls.Add(Me.Command1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Logging"
        Me.ShowInTaskbar = False
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.UDLotNum, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.DataGridViewMeasureLog, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewTrimmingLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UDLotNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents LblLotNo As System.Windows.Forms.Label
    Friend WithEvents LblLogType As System.Windows.Forms.Label
    Friend WithEvents chkOptDisp As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox2 As GroupBox
    Private WithEvents DataGridViewMeasureLog As DataGridView
    Private WithEvents DataGridViewTrimmingLog As DataGridView
    Friend WithEvents btnSetLogTarget As Button
    Friend WithEvents Label26 As Label
    Friend WithEvents Label25 As Label
#End Region
End Class