<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFlCond
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmFlCond))
        Me.GrpFlCond = New System.Windows.Forms.GroupBox()
        Me.CmbSelCnd = New System.Windows.Forms.ComboBox()
        Me.LblCndCurVal = New System.Windows.Forms.Label()
        Me.LblCndStegVal = New System.Windows.Forms.Label()
        Me.LblCndQrateVal = New System.Windows.Forms.Label()
        Me.LblCndCur = New System.Windows.Forms.Label()
        Me.LblCndSteg = New System.Windows.Forms.Label()
        Me.LblCndQrate = New System.Windows.Forms.Label()
        Me.LblSelCnd = New System.Windows.Forms.Label()
        Me.LblMSG = New System.Windows.Forms.Label()
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.GrpFlCond.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpFlCond
        '
        Me.GrpFlCond.Controls.Add(Me.CmbSelCnd)
        Me.GrpFlCond.Controls.Add(Me.LblCndCurVal)
        Me.GrpFlCond.Controls.Add(Me.LblCndStegVal)
        Me.GrpFlCond.Controls.Add(Me.LblCndQrateVal)
        Me.GrpFlCond.Controls.Add(Me.LblCndCur)
        Me.GrpFlCond.Controls.Add(Me.LblCndSteg)
        Me.GrpFlCond.Controls.Add(Me.LblCndQrate)
        Me.GrpFlCond.Controls.Add(Me.LblSelCnd)
        Me.GrpFlCond.Controls.Add(Me.LblMSG)
        resources.ApplyResources(Me.GrpFlCond, "GrpFlCond")
        Me.GrpFlCond.Name = "GrpFlCond"
        Me.GrpFlCond.TabStop = False
        '
        'CmbSelCnd
        '
        Me.CmbSelCnd.FormattingEnabled = True
        resources.ApplyResources(Me.CmbSelCnd, "CmbSelCnd")
        Me.CmbSelCnd.Name = "CmbSelCnd"
        '
        'LblCndCurVal
        '
        Me.LblCndCurVal.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.LblCndCurVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblCndCurVal, "LblCndCurVal")
        Me.LblCndCurVal.Name = "LblCndCurVal"
        '
        'LblCndStegVal
        '
        Me.LblCndStegVal.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.LblCndStegVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblCndStegVal, "LblCndStegVal")
        Me.LblCndStegVal.Name = "LblCndStegVal"
        '
        'LblCndQrateVal
        '
        Me.LblCndQrateVal.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.LblCndQrateVal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.LblCndQrateVal, "LblCndQrateVal")
        Me.LblCndQrateVal.Name = "LblCndQrateVal"
        '
        'LblCndCur
        '
        Me.LblCndCur.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.LblCndCur, "LblCndCur")
        Me.LblCndCur.Name = "LblCndCur"
        '
        'LblCndSteg
        '
        resources.ApplyResources(Me.LblCndSteg, "LblCndSteg")
        Me.LblCndSteg.Name = "LblCndSteg"
        '
        'LblCndQrate
        '
        resources.ApplyResources(Me.LblCndQrate, "LblCndQrate")
        Me.LblCndQrate.Name = "LblCndQrate"
        '
        'LblSelCnd
        '
        resources.ApplyResources(Me.LblSelCnd, "LblSelCnd")
        Me.LblSelCnd.Name = "LblSelCnd"
        '
        'LblMSG
        '
        Me.LblMSG.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        resources.ApplyResources(Me.LblMSG, "LblMSG")
        Me.LblMSG.Name = "LblMSG"
        '
        'BtnOK
        '
        Me.BtnOK.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        resources.ApplyResources(Me.BtnOK, "BtnOK")
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.UseVisualStyleBackColor = False
        '
        'BtnCancel
        '
        Me.BtnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.BtnCancel.BackColor = System.Drawing.SystemColors.ControlDark
        resources.ApplyResources(Me.BtnCancel, "BtnCancel")
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'FrmFlCond
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnOK)
        Me.Controls.Add(Me.GrpFlCond)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FrmFlCond"
        Me.ShowIcon = False
        Me.GrpFlCond.ResumeLayout(False)
        Me.GrpFlCond.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpFlCond As System.Windows.Forms.GroupBox
    Friend WithEvents LblMSG As System.Windows.Forms.Label
    Friend WithEvents BtnOK As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents LblCndCur As System.Windows.Forms.Label
    Friend WithEvents LblCndSteg As System.Windows.Forms.Label
    Friend WithEvents LblCndQrate As System.Windows.Forms.Label
    Friend WithEvents LblSelCnd As System.Windows.Forms.Label
    Friend WithEvents LblCndCurVal As System.Windows.Forms.Label
    Friend WithEvents LblCndStegVal As System.Windows.Forms.Label
    Friend WithEvents LblCndQrateVal As System.Windows.Forms.Label
    Friend WithEvents CmbSelCnd As System.Windows.Forms.ComboBox
End Class
