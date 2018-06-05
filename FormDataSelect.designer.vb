<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDataSelect
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDataSelect))
        Me.ListFile = New System.Windows.Forms.ListBox()
        Me.GrpMode = New System.Windows.Forms.GroupBox()
        Me.BtnMdEndless = New System.Windows.Forms.RadioButton()
        Me.BtnMdLot = New System.Windows.Forms.RadioButton()
        Me.BtnMdMagazine = New System.Windows.Forms.RadioButton()
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
        Me.GrpMode.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListFile
        '
        resources.ApplyResources(Me.ListFile, "ListFile")
        Me.ListFile.FormattingEnabled = True
        Me.ListFile.Name = "ListFile"
        '
        'GrpMode
        '
        resources.ApplyResources(Me.GrpMode, "GrpMode")
        Me.GrpMode.Controls.Add(Me.BtnMdEndless)
        Me.GrpMode.Controls.Add(Me.BtnMdLot)
        Me.GrpMode.Controls.Add(Me.BtnMdMagazine)
        Me.GrpMode.Name = "GrpMode"
        Me.GrpMode.TabStop = False
        '
        'BtnMdEndless
        '
        resources.ApplyResources(Me.BtnMdEndless, "BtnMdEndless")
        Me.BtnMdEndless.Checked = True
        Me.BtnMdEndless.Name = "BtnMdEndless"
        Me.BtnMdEndless.TabStop = True
        Me.BtnMdEndless.UseVisualStyleBackColor = True
        '
        'BtnMdLot
        '
        resources.ApplyResources(Me.BtnMdLot, "BtnMdLot")
        Me.BtnMdLot.Name = "BtnMdLot"
        Me.BtnMdLot.UseVisualStyleBackColor = True
        '
        'BtnMdMagazine
        '
        resources.ApplyResources(Me.BtnMdMagazine, "BtnMdMagazine")
        Me.BtnMdMagazine.Name = "BtnMdMagazine"
        Me.BtnMdMagazine.UseVisualStyleBackColor = True
        '
        'BtnSelect
        '
        resources.ApplyResources(Me.BtnSelect, "BtnSelect")
        Me.BtnSelect.Name = "BtnSelect"
        Me.BtnSelect.UseVisualStyleBackColor = True
        '
        'ListList
        '
        resources.ApplyResources(Me.ListList, "ListList")
        Me.ListList.FormattingEnabled = True
        Me.ListList.Name = "ListList"
        '
        'BtnOK
        '
        resources.ApplyResources(Me.BtnOK, "BtnOK")
        Me.BtnOK.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.UseVisualStyleBackColor = False
        '
        'BtnCancel
        '
        resources.ApplyResources(Me.BtnCancel, "BtnCancel")
        Me.BtnCancel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'BtnUp
        '
        resources.ApplyResources(Me.BtnUp, "BtnUp")
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'BtnDown
        '
        resources.ApplyResources(Me.BtnDown, "BtnDown")
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        resources.ApplyResources(Me.BtnDelete, "BtnDelete")
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        resources.ApplyResources(Me.BtnClear, "BtnClear")
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'LblDataFile
        '
        resources.ApplyResources(Me.LblDataFile, "LblDataFile")
        Me.LblDataFile.Name = "LblDataFile"
        '
        'LblListList
        '
        resources.ApplyResources(Me.LblListList, "LblListList")
        Me.LblListList.Name = "LblListList"
        '
        'LblFullPath
        '
        resources.ApplyResources(Me.LblFullPath, "LblFullPath")
        Me.LblFullPath.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LblFullPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblFullPath.Name = "LblFullPath"
        '
        'DrvListBox
        '
        resources.ApplyResources(Me.DrvListBox, "DrvListBox")
        Me.DrvListBox.FormattingEnabled = True
        Me.DrvListBox.Name = "DrvListBox"
        '
        'DirListBox
        '
        resources.ApplyResources(Me.DirListBox, "DirListBox")
        Me.DirListBox.FormattingEnabled = True
        Me.DirListBox.Name = "DirListBox"
        '
        'FileLstBox
        '
        resources.ApplyResources(Me.FileLstBox, "FileLstBox")
        Me.FileLstBox.FormattingEnabled = True
        Me.FileLstBox.Name = "FileLstBox"
        Me.FileLstBox.Pattern = "*.*"
        '
        'FormDataSelect
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ControlBox = False
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
        Me.Controls.Add(Me.GrpMode)
        Me.Controls.Add(Me.ListFile)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormDataSelect"
        Me.TopMost = True
        Me.GrpMode.ResumeLayout(False)
        Me.GrpMode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListFile As System.Windows.Forms.ListBox
    Friend WithEvents GrpMode As System.Windows.Forms.GroupBox
    Friend WithEvents BtnMdMagazine As System.Windows.Forms.RadioButton
    Friend WithEvents BtnMdLot As System.Windows.Forms.RadioButton
    Friend WithEvents BtnMdEndless As System.Windows.Forms.RadioButton
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
End Class
