<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDataSelect2
    Inherits System.Windows.Forms.Form

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
        Me.ListFile = New System.Windows.Forms.ListBox()
        Me.BtnSelect = New System.Windows.Forms.Button()
        Me.ListList = New System.Windows.Forms.ListBox()
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnUp = New System.Windows.Forms.Button()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.LblDataFile = New System.Windows.Forms.Label()
        Me.LblListList = New System.Windows.Forms.Label()
        Me.LblFullPath = New System.Windows.Forms.Label()
        Me.DrvListBox = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox()
        Me.DirListBox = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox()
        Me.FileLstBox = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdLotInfo = New System.Windows.Forms.Button()
        Me.PanelComboBoxBorder = New System.Windows.Forms.Panel()
        Me.ComboBoxOperatorList = New TKY_ALL_SL432HW.ComboBoxRightAlign()
        Me.ToolTipOperator = New System.Windows.Forms.ToolTip(Me.components)
        Me.PanelComboBoxBorder.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListFile
        '
        Me.ListFile.FormattingEnabled = True
        Me.ListFile.HorizontalScrollbar = True
        Me.ListFile.ItemHeight = 16
        Me.ListFile.Location = New System.Drawing.Point(221, 29)
        Me.ListFile.Name = "ListFile"
        Me.ListFile.Size = New System.Drawing.Size(583, 244)
        Me.ListFile.TabIndex = 2
        '
        'BtnSelect
        '
        Me.BtnSelect.Location = New System.Drawing.Point(441, 287)
        Me.BtnSelect.Name = "BtnSelect"
        Me.BtnSelect.Size = New System.Drawing.Size(120, 34)
        Me.BtnSelect.TabIndex = 3
        Me.BtnSelect.Text = "↓ 登録 ↓"
        Me.BtnSelect.UseVisualStyleBackColor = True
        '
        'ListList
        '
        Me.ListList.FormattingEnabled = True
        Me.ListList.HorizontalScrollbar = True
        Me.ListList.ItemHeight = 16
        Me.ListList.Location = New System.Drawing.Point(221, 345)
        Me.ListList.Name = "ListList"
        Me.ListList.Size = New System.Drawing.Size(583, 244)
        Me.ListList.TabIndex = 4
        '
        'BtnOK
        '
        Me.BtnOK.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.BtnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOK.Location = New System.Drawing.Point(972, 392)
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.Size = New System.Drawing.Size(120, 34)
        Me.BtnOK.TabIndex = 11
        Me.BtnOK.Text = "OK"
        Me.BtnOK.UseVisualStyleBackColor = False
        '
        'BtnCancel
        '
        Me.BtnCancel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(972, 432)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(120, 34)
        Me.BtnCancel.TabIndex = 12
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'BtnUp
        '
        Me.BtnUp.Location = New System.Drawing.Point(829, 174)
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.Size = New System.Drawing.Size(263, 34)
        Me.BtnUp.TabIndex = 7
        Me.BtnUp.Text = "リストの１つ上へ"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'BtnDown
        '
        Me.BtnDown.Location = New System.Drawing.Point(829, 214)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(263, 34)
        Me.BtnDown.TabIndex = 8
        Me.BtnDown.Text = "リストの１つ下へ"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Location = New System.Drawing.Point(829, 287)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(263, 34)
        Me.BtnDelete.TabIndex = 9
        Me.BtnDelete.Text = "リストから削除"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Location = New System.Drawing.Point(829, 327)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(263, 34)
        Me.BtnClear.TabIndex = 10
        Me.BtnClear.Text = "リストをクリア"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'LblDataFile
        '
        Me.LblDataFile.AutoSize = True
        Me.LblDataFile.Location = New System.Drawing.Point(218, 8)
        Me.LblDataFile.Name = "LblDataFile"
        Me.LblDataFile.Size = New System.Drawing.Size(120, 16)
        Me.LblDataFile.TabIndex = 16
        Me.LblDataFile.Text = "データファイル"
        '
        'LblListList
        '
        Me.LblListList.AutoSize = True
        Me.LblListList.Location = New System.Drawing.Point(224, 317)
        Me.LblListList.Name = "LblListList"
        Me.LblListList.Size = New System.Drawing.Size(184, 16)
        Me.LblListList.TabIndex = 17
        Me.LblListList.Text = "登録済みデータファイル"
        '
        'LblFullPath
        '
        Me.LblFullPath.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LblFullPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblFullPath.Location = New System.Drawing.Point(8, 614)
        Me.LblFullPath.Name = "LblFullPath"
        Me.LblFullPath.Size = New System.Drawing.Size(1103, 20)
        Me.LblFullPath.TabIndex = 12
        '
        'DrvListBox
        '
        Me.DrvListBox.FormattingEnabled = True
        Me.DrvListBox.Location = New System.Drawing.Point(8, 30)
        Me.DrvListBox.Name = "DrvListBox"
        Me.DrvListBox.Size = New System.Drawing.Size(180, 24)
        Me.DrvListBox.TabIndex = 0
        '
        'DirListBox
        '
        Me.DirListBox.FormattingEnabled = True
        Me.DirListBox.HorizontalScrollbar = True
        Me.DirListBox.IntegralHeight = False
        Me.DirListBox.Location = New System.Drawing.Point(8, 69)
        Me.DirListBox.Name = "DirListBox"
        Me.DirListBox.Size = New System.Drawing.Size(192, 520)
        Me.DirListBox.TabIndex = 1
        '
        'FileLstBox
        '
        Me.FileLstBox.FormattingEnabled = True
        Me.FileLstBox.Location = New System.Drawing.Point(656, 4)
        Me.FileLstBox.Name = "FileLstBox"
        Me.FileLstBox.Pattern = "*.*"
        Me.FileLstBox.Size = New System.Drawing.Size(138, 20)
        Me.FileLstBox.TabIndex = 21
        Me.FileLstBox.Visible = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(830, 94)
        Me.cmdEdit.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(263, 34)
        Me.cmdEdit.TabIndex = 6
        Me.cmdEdit.Text = "編集"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdLotInfo
        '
        Me.cmdLotInfo.BackColor = System.Drawing.Color.Cyan
        Me.cmdLotInfo.Enabled = False
        Me.cmdLotInfo.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Bold)
        Me.cmdLotInfo.Location = New System.Drawing.Point(830, 34)
        Me.cmdLotInfo.Name = "cmdLotInfo"
        Me.cmdLotInfo.Size = New System.Drawing.Size(263, 34)
        Me.cmdLotInfo.TabIndex = 5
        Me.cmdLotInfo.Text = "データ設定"
        Me.cmdLotInfo.UseVisualStyleBackColor = False
        Me.cmdLotInfo.Visible = False
        '
        'PanelComboBoxBorder
        '
        Me.PanelComboBoxBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelComboBoxBorder.Controls.Add(Me.ComboBoxOperatorList)
        Me.PanelComboBoxBorder.Location = New System.Drawing.Point(830, 29)
        Me.PanelComboBoxBorder.Name = "PanelComboBoxBorder"
        Me.PanelComboBoxBorder.Size = New System.Drawing.Size(263, 29)
        Me.PanelComboBoxBorder.TabIndex = 5
        '
        'ComboBoxOperatorList
        '
        Me.ComboBoxOperatorList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBoxOperatorList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxOperatorList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxOperatorList.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ComboBoxOperatorList.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.ComboBoxOperatorList.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxOperatorList.Name = "ComboBoxOperatorList"
        Me.ComboBoxOperatorList.Size = New System.Drawing.Size(261, 27)
        Me.ComboBoxOperatorList.TabIndex = 0
        '
        'FormDataSelect
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1123, 643)
        Me.ControlBox = False
        Me.Controls.Add(Me.PanelComboBoxBorder)
        Me.Controls.Add(Me.cmdLotInfo)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.FileLstBox)
        Me.Controls.Add(Me.DirListBox)
        Me.Controls.Add(Me.DrvListBox)
        Me.Controls.Add(Me.LblFullPath)
        Me.Controls.Add(Me.LblListList)
        Me.Controls.Add(Me.LblDataFile)
        Me.Controls.Add(Me.BtnClear)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.BtnUp)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnOK)
        Me.Controls.Add(Me.ListList)
        Me.Controls.Add(Me.BtnSelect)
        Me.Controls.Add(Me.ListFile)
        Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormDataSelect2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "データ登録"
        Me.TopMost = True
        Me.PanelComboBoxBorder.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListFile As System.Windows.Forms.ListBox
    Friend WithEvents BtnSelect As System.Windows.Forms.Button
    Friend WithEvents ListList As System.Windows.Forms.ListBox
    Friend WithEvents BtnOK As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnUp As System.Windows.Forms.Button
    Friend WithEvents BtnDown As System.Windows.Forms.Button
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents BtnClear As System.Windows.Forms.Button
    Friend WithEvents LblDataFile As System.Windows.Forms.Label
    Friend WithEvents LblListList As System.Windows.Forms.Label
    Friend WithEvents LblFullPath As System.Windows.Forms.Label
    Friend WithEvents DrvListBox As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
    Friend WithEvents DirListBox As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
    Friend WithEvents FileLstBox As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Friend WithEvents cmdLotInfo As System.Windows.Forms.Button
    Private WithEvents ComboBoxOperatorList As ComboBoxRightAlign
    Private WithEvents PanelComboBoxBorder As Panel
    Friend WithEvents ToolTipOperator As ToolTip
End Class
