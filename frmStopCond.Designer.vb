<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStopCond
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtOverRange = New System.Windows.Forms.TextBox()
        Me.CheckOverRange = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtYield = New System.Windows.Forms.TextBox()
        Me.CheckYeild = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GrpFinal = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFTLO = New System.Windows.Forms.TextBox()
        Me.CheckFTLo = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFTHI = New System.Windows.Forms.TextBox()
        Me.CheckFTHi = New System.Windows.Forms.CheckBox()
        Me.RadioLot = New System.Windows.Forms.RadioButton()
        Me.GrpInital = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtITLO = New System.Windows.Forms.TextBox()
        Me.CheckITLo = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtITHI = New System.Windows.Forms.TextBox()
        Me.CheckITHi = New System.Windows.Forms.CheckBox()
        Me.RadioPlate = New System.Windows.Forms.RadioButton()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.CheckBlock = New System.Windows.Forms.CheckBox()
        Me.CheckPlate = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GrpFinal.SuspendLayout()
        Me.GrpInital.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtOverRange)
        Me.GroupBox1.Controls.Add(Me.CheckOverRange)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtYield)
        Me.GroupBox1.Controls.Add(Me.CheckYeild)
        Me.GroupBox1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(22, 52)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(299, 84)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Total"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.Location = New System.Drawing.Point(214, 53)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 15)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "% (Over)"
        '
        'txtOverRange
        '
        Me.txtOverRange.Location = New System.Drawing.Point(122, 50)
        Me.txtOverRange.Name = "txtOverRange"
        Me.txtOverRange.Size = New System.Drawing.Size(81, 23)
        Me.txtOverRange.TabIndex = 17
        '
        'CheckOverRange
        '
        Me.CheckOverRange.AutoSize = True
        Me.CheckOverRange.Location = New System.Drawing.Point(33, 52)
        Me.CheckOverRange.Name = "CheckOverRange"
        Me.CheckOverRange.Size = New System.Drawing.Size(89, 20)
        Me.CheckOverRange.TabIndex = 16
        Me.CheckOverRange.Text = "OpenNG"
        Me.CheckOverRange.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.Location = New System.Drawing.Point(214, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 15)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "% (Under)"
        '
        'txtYield
        '
        Me.txtYield.Location = New System.Drawing.Point(122, 23)
        Me.txtYield.Name = "txtYield"
        Me.txtYield.Size = New System.Drawing.Size(81, 23)
        Me.txtYield.TabIndex = 14
        '
        'CheckYeild
        '
        Me.CheckYeild.AutoSize = True
        Me.CheckYeild.Location = New System.Drawing.Point(33, 23)
        Me.CheckYeild.Name = "CheckYeild"
        Me.CheckYeild.Size = New System.Drawing.Size(63, 20)
        Me.CheckYeild.TabIndex = 13
        Me.CheckYeild.Text = "Yield"
        Me.CheckYeild.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GrpFinal)
        Me.GroupBox2.Controls.Add(Me.RadioLot)
        Me.GroupBox2.Controls.Add(Me.GrpInital)
        Me.GroupBox2.Controls.Add(Me.RadioPlate)
        Me.GroupBox2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(22, 143)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(299, 238)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Unit"
        '
        'GrpFinal
        '
        Me.GrpFinal.Controls.Add(Me.Label3)
        Me.GrpFinal.Controls.Add(Me.txtFTLO)
        Me.GrpFinal.Controls.Add(Me.CheckFTLo)
        Me.GrpFinal.Controls.Add(Me.Label4)
        Me.GrpFinal.Controls.Add(Me.txtFTHI)
        Me.GrpFinal.Controls.Add(Me.CheckFTHi)
        Me.GrpFinal.Location = New System.Drawing.Point(12, 140)
        Me.GrpFinal.Name = "GrpFinal"
        Me.GrpFinal.Size = New System.Drawing.Size(274, 86)
        Me.GrpFinal.TabIndex = 6
        Me.GrpFinal.TabStop = False
        Me.GrpFinal.Text = "Final"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(194, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "% (Over)"
        '
        'txtFTLO
        '
        Me.txtFTLO.Location = New System.Drawing.Point(95, 51)
        Me.txtFTLO.Name = "txtFTLO"
        Me.txtFTLO.Size = New System.Drawing.Size(81, 23)
        Me.txtFTLO.TabIndex = 4
        '
        'CheckFTLo
        '
        Me.CheckFTLo.AutoSize = True
        Me.CheckFTLo.Location = New System.Drawing.Point(26, 53)
        Me.CheckFTLo.Name = "CheckFTLo"
        Me.CheckFTLo.Size = New System.Drawing.Size(49, 20)
        Me.CheckFTLo.TabIndex = 3
        Me.CheckFTLo.Text = "LO"
        Me.CheckFTLo.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(194, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 16)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "% (Over)"
        '
        'txtFTHI
        '
        Me.txtFTHI.Location = New System.Drawing.Point(95, 22)
        Me.txtFTHI.Name = "txtFTHI"
        Me.txtFTHI.Size = New System.Drawing.Size(81, 23)
        Me.txtFTHI.TabIndex = 1
        '
        'CheckFTHi
        '
        Me.CheckFTHi.AutoSize = True
        Me.CheckFTHi.Location = New System.Drawing.Point(26, 24)
        Me.CheckFTHi.Name = "CheckFTHi"
        Me.CheckFTHi.Size = New System.Drawing.Size(43, 20)
        Me.CheckFTHi.TabIndex = 0
        Me.CheckFTHi.Text = "HI"
        Me.CheckFTHi.UseVisualStyleBackColor = True
        '
        'RadioLot
        '
        Me.RadioLot.AutoSize = True
        Me.RadioLot.Location = New System.Drawing.Point(51, 22)
        Me.RadioLot.Name = "RadioLot"
        Me.RadioLot.Size = New System.Drawing.Size(52, 20)
        Me.RadioLot.TabIndex = 19
        Me.RadioLot.TabStop = True
        Me.RadioLot.Text = "Lot"
        Me.RadioLot.UseVisualStyleBackColor = True
        '
        'GrpInital
        '
        Me.GrpInital.Controls.Add(Me.Label2)
        Me.GrpInital.Controls.Add(Me.txtITLO)
        Me.GrpInital.Controls.Add(Me.CheckITLo)
        Me.GrpInital.Controls.Add(Me.Label1)
        Me.GrpInital.Controls.Add(Me.txtITHI)
        Me.GrpInital.Controls.Add(Me.CheckITHi)
        Me.GrpInital.Location = New System.Drawing.Point(12, 49)
        Me.GrpInital.Name = "GrpInital"
        Me.GrpInital.Size = New System.Drawing.Size(274, 85)
        Me.GrpInital.TabIndex = 1
        Me.GrpInital.TabStop = False
        Me.GrpInital.Text = "Initial"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(194, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "% (Over)"
        '
        'txtITLO
        '
        Me.txtITLO.Location = New System.Drawing.Point(95, 51)
        Me.txtITLO.Name = "txtITLO"
        Me.txtITLO.Size = New System.Drawing.Size(81, 23)
        Me.txtITLO.TabIndex = 4
        '
        'CheckITLo
        '
        Me.CheckITLo.AutoSize = True
        Me.CheckITLo.Location = New System.Drawing.Point(26, 50)
        Me.CheckITLo.Name = "CheckITLo"
        Me.CheckITLo.Size = New System.Drawing.Size(49, 20)
        Me.CheckITLo.TabIndex = 3
        Me.CheckITLo.Text = "LO"
        Me.CheckITLo.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(194, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "% (Over)"
        '
        'txtITHI
        '
        Me.txtITHI.Location = New System.Drawing.Point(95, 22)
        Me.txtITHI.Name = "txtITHI"
        Me.txtITHI.Size = New System.Drawing.Size(81, 23)
        Me.txtITHI.TabIndex = 1
        '
        'CheckITHi
        '
        Me.CheckITHi.AutoSize = True
        Me.CheckITHi.Location = New System.Drawing.Point(26, 24)
        Me.CheckITHi.Name = "CheckITHi"
        Me.CheckITHi.Size = New System.Drawing.Size(43, 20)
        Me.CheckITHi.TabIndex = 0
        Me.CheckITHi.Text = "HI"
        Me.CheckITHi.UseVisualStyleBackColor = True
        '
        'RadioPlate
        '
        Me.RadioPlate.AutoSize = True
        Me.RadioPlate.Location = New System.Drawing.Point(147, 22)
        Me.RadioPlate.Name = "RadioPlate"
        Me.RadioPlate.Size = New System.Drawing.Size(102, 20)
        Me.RadioPlate.TabIndex = 20
        Me.RadioPlate.TabStop = True
        Me.RadioPlate.Text = "Substrate"
        Me.RadioPlate.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(49, 405)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(106, 40)
        Me.btnUpdate.TabIndex = 2
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(197, 405)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(106, 40)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'CheckBlock
        '
        Me.CheckBlock.AutoSize = True
        Me.CheckBlock.Location = New System.Drawing.Point(155, 18)
        Me.CheckBlock.Name = "CheckBlock"
        Me.CheckBlock.Size = New System.Drawing.Size(68, 20)
        Me.CheckBlock.TabIndex = 20
        Me.CheckBlock.Text = "Block"
        Me.CheckBlock.UseVisualStyleBackColor = True
        '
        'CheckPlate
        '
        Me.CheckPlate.AutoSize = True
        Me.CheckPlate.Location = New System.Drawing.Point(28, 18)
        Me.CheckPlate.Name = "CheckPlate"
        Me.CheckPlate.Size = New System.Drawing.Size(103, 20)
        Me.CheckPlate.TabIndex = 19
        Me.CheckPlate.Text = "Substrate"
        Me.CheckPlate.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CheckPlate)
        Me.GroupBox3.Controls.Add(Me.CheckBlock)
        Me.GroupBox3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(27, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(294, 42)
        Me.GroupBox3.TabIndex = 21
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "JudgeTimming"
        '
        'frmStopCond
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 457)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmStopCond"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lot Stop Condition "
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GrpFinal.ResumeLayout(False)
        Me.GrpFinal.PerformLayout()
        Me.GrpInital.ResumeLayout(False)
        Me.GrpInital.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GrpFinal As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFTLO As System.Windows.Forms.TextBox
    Friend WithEvents CheckFTLo As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFTHI As System.Windows.Forms.TextBox
    Friend WithEvents CheckFTHi As System.Windows.Forms.CheckBox
    Friend WithEvents GrpInital As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtITLO As System.Windows.Forms.TextBox
    Friend WithEvents CheckITLo As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtITHI As System.Windows.Forms.TextBox
    Friend WithEvents CheckITHi As System.Windows.Forms.CheckBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtOverRange As System.Windows.Forms.TextBox
    Friend WithEvents CheckOverRange As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtYield As System.Windows.Forms.TextBox
    Friend WithEvents CheckYeild As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBlock As System.Windows.Forms.CheckBox
    Friend WithEvents CheckPlate As System.Windows.Forms.CheckBox
    Friend WithEvents RadioPlate As System.Windows.Forms.RadioButton
    Friend WithEvents RadioLot As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
End Class
