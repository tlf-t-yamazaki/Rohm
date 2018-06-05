<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormLotEnd
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LabelLotInput = New System.Windows.Forms.Label()
        Me.ButtonContinue = New System.Windows.Forms.Button()
        Me.ButtonEnd = New System.Windows.Forms.Button()
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelMessage = New System.Windows.Forms.Label()
        Me.LabelAlbNum = New System.Windows.Forms.Label()
        Me.LabelAllowable = New System.Windows.Forms.Label()
        Me.LabelProcNum = New System.Windows.Forms.Label()
        Me.LabelInNum = New System.Windows.Forms.Label()
        Me.LabelProcessed = New System.Windows.Forms.Label()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelLotInput
        '
        Me.TableLayoutPanelMain.SetColumnSpan(Me.LabelLotInput, 2)
        Me.LabelLotInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelLotInput.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelLotInput.Location = New System.Drawing.Point(83, 83)
        Me.LabelLotInput.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelLotInput.Name = "LabelLotInput"
        Me.LabelLotInput.Size = New System.Drawing.Size(220, 24)
        Me.LabelLotInput.TabIndex = 9
        Me.LabelLotInput.Text = "ロット投入基板枚数"
        Me.LabelLotInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonContinue
        '
        Me.ButtonContinue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonContinue.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ButtonContinue.Location = New System.Drawing.Point(83, 193)
        Me.ButtonContinue.Name = "ButtonContinue"
        Me.ButtonContinue.Size = New System.Drawing.Size(180, 44)
        Me.ButtonContinue.TabIndex = 11
        Me.ButtonContinue.TabStop = False
        Me.ButtonContinue.Text = "続行"
        Me.ButtonContinue.UseVisualStyleBackColor = True
        '
        'ButtonEnd
        '
        Me.ButtonEnd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonEnd.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ButtonEnd.Location = New System.Drawing.Point(309, 193)
        Me.ButtonEnd.Name = "ButtonEnd"
        Me.ButtonEnd.Size = New System.Drawing.Size(180, 44)
        Me.ButtonEnd.TabIndex = 12
        Me.ButtonEnd.TabStop = False
        Me.ButtonEnd.Text = "終了"
        Me.ButtonEnd.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 5
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelMessage, 1, 1)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelAlbNum, 3, 4)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelAllowable, 1, 4)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelProcNum, 3, 3)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelInNum, 3, 2)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelProcessed, 1, 3)
        Me.TableLayoutPanelMain.Controls.Add(Me.ButtonContinue, 1, 6)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelLotInput, 1, 2)
        Me.TableLayoutPanelMain.Controls.Add(Me.ButtonEnd, 3, 6)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 8
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(572, 250)
        Me.TableLayoutPanelMain.TabIndex = 13
        '
        'LabelMessage
        '
        Me.LabelMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TableLayoutPanelMain.SetColumnSpan(Me.LabelMessage, 3)
        Me.LabelMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelMessage.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelMessage.Location = New System.Drawing.Point(83, 3)
        Me.LabelMessage.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelMessage.Name = "LabelMessage"
        Me.LabelMessage.Size = New System.Drawing.Size(406, 74)
        Me.LabelMessage.TabIndex = 18
        Me.LabelMessage.Text = "処理基板枚数が不足しています。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "自動運転を終了しますか？"
        Me.LabelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelAlbNum
        '
        Me.LabelAlbNum.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelAlbNum.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelAlbNum.Location = New System.Drawing.Point(309, 143)
        Me.LabelAlbNum.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelAlbNum.Name = "LabelAlbNum"
        Me.LabelAlbNum.Size = New System.Drawing.Size(180, 24)
        Me.LabelAlbNum.TabIndex = 17
        Me.LabelAlbNum.Text = "{2}"
        Me.LabelAlbNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelAllowable
        '
        Me.TableLayoutPanelMain.SetColumnSpan(Me.LabelAllowable, 2)
        Me.LabelAllowable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelAllowable.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelAllowable.Location = New System.Drawing.Point(83, 143)
        Me.LabelAllowable.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelAllowable.Name = "LabelAllowable"
        Me.LabelAllowable.Size = New System.Drawing.Size(220, 24)
        Me.LabelAllowable.TabIndex = 16
        Me.LabelAllowable.Text = "許容基板枚数"
        Me.LabelAllowable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelProcNum
        '
        Me.LabelProcNum.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelProcNum.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelProcNum.Location = New System.Drawing.Point(309, 113)
        Me.LabelProcNum.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelProcNum.Name = "LabelProcNum"
        Me.LabelProcNum.Size = New System.Drawing.Size(180, 24)
        Me.LabelProcNum.TabIndex = 15
        Me.LabelProcNum.Text = "{1}"
        Me.LabelProcNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelInNum
        '
        Me.LabelInNum.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelInNum.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelInNum.Location = New System.Drawing.Point(309, 83)
        Me.LabelInNum.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelInNum.Name = "LabelInNum"
        Me.LabelInNum.Size = New System.Drawing.Size(180, 24)
        Me.LabelInNum.TabIndex = 14
        Me.LabelInNum.Text = "{0}"
        Me.LabelInNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelProcessed
        '
        Me.TableLayoutPanelMain.SetColumnSpan(Me.LabelProcessed, 2)
        Me.LabelProcessed.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelProcessed.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelProcessed.Location = New System.Drawing.Point(83, 113)
        Me.LabelProcessed.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelProcessed.Name = "LabelProcessed"
        Me.LabelProcessed.Size = New System.Drawing.Size(220, 24)
        Me.LabelProcessed.TabIndex = 13
        Me.LabelProcessed.Text = "処理基板枚数"
        Me.LabelProcessed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FormLotEnd
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(572, 250)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanelMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormLotEnd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LOT END"
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents LabelLotInput As System.Windows.Forms.Label
    Private WithEvents TableLayoutPanelMain As System.Windows.Forms.TableLayoutPanel
    Private WithEvents LabelMessage As System.Windows.Forms.Label
    Private WithEvents LabelAlbNum As System.Windows.Forms.Label
    Private WithEvents LabelAllowable As System.Windows.Forms.Label
    Private WithEvents LabelProcNum As System.Windows.Forms.Label
    Private WithEvents LabelInNum As System.Windows.Forms.Label
    Private WithEvents LabelProcessed As System.Windows.Forms.Label
    Private WithEvents ButtonContinue As System.Windows.Forms.Button
    Private WithEvents ButtonEnd As System.Windows.Forms.Button
End Class
