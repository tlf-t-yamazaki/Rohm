<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCutOffEsPointEnter
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
        Me.components = New System.ComponentModel.Container()
        Me.pnlFirstResData = New System.Windows.Forms.Panel()
        Me.lblFrdNom = New System.Windows.Forms.Label()
        Me.lblFrdNomVal = New System.Windows.Forms.Label()
        Me.tlpFirstResData = New System.Windows.Forms.TableLayoutPanel()
        Me.lblFrdE1 = New System.Windows.Forms.TextBox()
        Me.lblFrdE5 = New System.Windows.Forms.TextBox()
        Me.lblFrdC1 = New System.Windows.Forms.TextBox()
        Me.lblFrdC2 = New System.Windows.Forms.TextBox()
        Me.lblFrdC3 = New System.Windows.Forms.TextBox()
        Me.lblFrdC4 = New System.Windows.Forms.TextBox()
        Me.lblFrdC5 = New System.Windows.Forms.TextBox()
        Me.lblFrdC6 = New System.Windows.Forms.TextBox()
        Me.lblFrdE2 = New System.Windows.Forms.TextBox()
        Me.lblFrdE3 = New System.Windows.Forms.TextBox()
        Me.lblFrdE4 = New System.Windows.Forms.TextBox()
        Me.lblFrdE6 = New System.Windows.Forms.TextBox()
        Me.lblFrdCutOff = New System.Windows.Forms.Label()
        Me.lblFrdESPoint = New System.Windows.Forms.Label()
        Me.lblFrd_1 = New System.Windows.Forms.Label()
        Me.lblFrd_2 = New System.Windows.Forms.Label()
        Me.lblFrd_3 = New System.Windows.Forms.Label()
        Me.lblFrd_4 = New System.Windows.Forms.Label()
        Me.lblFrd_5 = New System.Windows.Forms.Label()
        Me.lblFrd_6 = New System.Windows.Forms.Label()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.EntryButton = New System.Windows.Forms.Button()
        Me.LabelErrorMessage = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlFirstResData.SuspendLayout()
        Me.tlpFirstResData.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlFirstResData
        '
        Me.pnlFirstResData.Controls.Add(Me.lblFrdNom)
        Me.pnlFirstResData.Controls.Add(Me.lblFrdNomVal)
        Me.pnlFirstResData.Controls.Add(Me.tlpFirstResData)
        Me.pnlFirstResData.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.pnlFirstResData.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.pnlFirstResData.Location = New System.Drawing.Point(46, 37)
        Me.pnlFirstResData.Name = "pnlFirstResData"
        Me.pnlFirstResData.Size = New System.Drawing.Size(412, 300)
        Me.pnlFirstResData.TabIndex = 400
        '
        'lblFrdNom
        '
        Me.lblFrdNom.Location = New System.Drawing.Point(25, 10)
        Me.lblFrdNom.Name = "lblFrdNom"
        Me.lblFrdNom.Size = New System.Drawing.Size(80, 20)
        Me.lblFrdNom.TabIndex = 1
        Me.lblFrdNom.Text = "目標値："
        Me.lblFrdNom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFrdNomVal
        '
        Me.lblFrdNomVal.Location = New System.Drawing.Point(110, 10)
        Me.lblFrdNomVal.Name = "lblFrdNomVal"
        Me.lblFrdNomVal.Size = New System.Drawing.Size(191, 20)
        Me.lblFrdNomVal.TabIndex = 2
        Me.lblFrdNomVal.Text = "xxxxx.xxxxx"
        Me.lblFrdNomVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tlpFirstResData
        '
        Me.tlpFirstResData.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.tlpFirstResData.ColumnCount = 3
        Me.tlpFirstResData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.tlpFirstResData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85714!))
        Me.tlpFirstResData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85714!))
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE1, 2, 1)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE5, 2, 5)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC1, 1, 1)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC2, 1, 2)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC3, 1, 3)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC4, 1, 4)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC5, 1, 5)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdC6, 1, 6)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE2, 2, 2)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE3, 2, 3)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE4, 2, 4)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdE6, 2, 6)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdCutOff, 1, 0)
        Me.tlpFirstResData.Controls.Add(Me.lblFrdESPoint, 2, 0)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_1, 0, 1)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_2, 0, 2)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_3, 0, 3)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_4, 0, 4)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_5, 0, 5)
        Me.tlpFirstResData.Controls.Add(Me.lblFrd_6, 0, 6)
        Me.tlpFirstResData.Location = New System.Drawing.Point(13, 57)
        Me.tlpFirstResData.Name = "tlpFirstResData"
        Me.tlpFirstResData.RowCount = 7
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.0!))
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.0!))
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.0!))
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.0!))
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.0!))
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.0!))
        Me.tlpFirstResData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.0!))
        Me.tlpFirstResData.Size = New System.Drawing.Size(396, 227)
        Me.tlpFirstResData.TabIndex = 0
        '
        'lblFrdE1
        '
        Me.lblFrdE1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdE1.Location = New System.Drawing.Point(239, 42)
        Me.lblFrdE1.MaxLength = 8
        Me.lblFrdE1.Name = "lblFrdE1"
        Me.lblFrdE1.Size = New System.Drawing.Size(154, 22)
        Me.lblFrdE1.TabIndex = 2
        Me.lblFrdE1.Text = "lblFrdE1"
        Me.lblFrdE1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdE5
        '
        Me.lblFrdE5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdE5.Location = New System.Drawing.Point(239, 166)
        Me.lblFrdE5.MaxLength = 8
        Me.lblFrdE5.Name = "lblFrdE5"
        Me.lblFrdE5.Size = New System.Drawing.Size(154, 22)
        Me.lblFrdE5.TabIndex = 10
        Me.lblFrdE5.Text = "lblFrdE5"
        Me.lblFrdE5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdC1
        '
        Me.lblFrdC1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdC1.Location = New System.Drawing.Point(105, 42)
        Me.lblFrdC1.MaxLength = 8
        Me.lblFrdC1.Name = "lblFrdC1"
        Me.lblFrdC1.Size = New System.Drawing.Size(117, 22)
        Me.lblFrdC1.TabIndex = 1
        Me.lblFrdC1.Text = "lblFrdC1"
        Me.lblFrdC1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdC2
        '
        Me.lblFrdC2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdC2.Location = New System.Drawing.Point(105, 73)
        Me.lblFrdC2.MaxLength = 8
        Me.lblFrdC2.Name = "lblFrdC2"
        Me.lblFrdC2.Size = New System.Drawing.Size(117, 22)
        Me.lblFrdC2.TabIndex = 3
        Me.lblFrdC2.Text = "lblFrdC2"
        Me.lblFrdC2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdC3
        '
        Me.lblFrdC3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdC3.Location = New System.Drawing.Point(105, 104)
        Me.lblFrdC3.MaxLength = 8
        Me.lblFrdC3.Name = "lblFrdC3"
        Me.lblFrdC3.Size = New System.Drawing.Size(117, 22)
        Me.lblFrdC3.TabIndex = 5
        Me.lblFrdC3.Text = "lblFrdC3"
        Me.lblFrdC3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdC4
        '
        Me.lblFrdC4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdC4.Location = New System.Drawing.Point(105, 135)
        Me.lblFrdC4.MaxLength = 8
        Me.lblFrdC4.Name = "lblFrdC4"
        Me.lblFrdC4.Size = New System.Drawing.Size(117, 22)
        Me.lblFrdC4.TabIndex = 7
        Me.lblFrdC4.Text = "lblFrdC4"
        Me.lblFrdC4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdC5
        '
        Me.lblFrdC5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdC5.Location = New System.Drawing.Point(105, 166)
        Me.lblFrdC5.MaxLength = 8
        Me.lblFrdC5.Name = "lblFrdC5"
        Me.lblFrdC5.Size = New System.Drawing.Size(117, 22)
        Me.lblFrdC5.TabIndex = 9
        Me.lblFrdC5.Text = "lblFrdC5"
        Me.lblFrdC5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdC6
        '
        Me.lblFrdC6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdC6.Location = New System.Drawing.Point(105, 202)
        Me.lblFrdC6.MaxLength = 8
        Me.lblFrdC6.Name = "lblFrdC6"
        Me.lblFrdC6.Size = New System.Drawing.Size(117, 22)
        Me.lblFrdC6.TabIndex = 11
        Me.lblFrdC6.Text = "lblFrdC6"
        Me.lblFrdC6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdE2
        '
        Me.lblFrdE2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdE2.Location = New System.Drawing.Point(239, 73)
        Me.lblFrdE2.MaxLength = 8
        Me.lblFrdE2.Name = "lblFrdE2"
        Me.lblFrdE2.Size = New System.Drawing.Size(154, 22)
        Me.lblFrdE2.TabIndex = 4
        Me.lblFrdE2.Text = "lblFrdE2"
        Me.lblFrdE2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdE3
        '
        Me.lblFrdE3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdE3.Location = New System.Drawing.Point(239, 104)
        Me.lblFrdE3.MaxLength = 8
        Me.lblFrdE3.Name = "lblFrdE3"
        Me.lblFrdE3.Size = New System.Drawing.Size(154, 22)
        Me.lblFrdE3.TabIndex = 6
        Me.lblFrdE3.Text = "lblFrdE3"
        Me.lblFrdE3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdE4
        '
        Me.lblFrdE4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdE4.Location = New System.Drawing.Point(239, 135)
        Me.lblFrdE4.MaxLength = 8
        Me.lblFrdE4.Name = "lblFrdE4"
        Me.lblFrdE4.Size = New System.Drawing.Size(154, 22)
        Me.lblFrdE4.TabIndex = 8
        Me.lblFrdE4.Text = "lblFrdE4"
        Me.lblFrdE4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdE6
        '
        Me.lblFrdE6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFrdE6.Location = New System.Drawing.Point(239, 202)
        Me.lblFrdE6.MaxLength = 8
        Me.lblFrdE6.Name = "lblFrdE6"
        Me.lblFrdE6.Size = New System.Drawing.Size(154, 22)
        Me.lblFrdE6.TabIndex = 12
        Me.lblFrdE6.Text = "lblFrdE6"
        Me.lblFrdE6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblFrdCutOff
        '
        Me.lblFrdCutOff.AutoSize = True
        Me.lblFrdCutOff.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrdCutOff.Location = New System.Drawing.Point(59, 3)
        Me.lblFrdCutOff.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrdCutOff.Name = "lblFrdCutOff"
        Me.lblFrdCutOff.Size = New System.Drawing.Size(163, 30)
        Me.lblFrdCutOff.TabIndex = 0
        Me.lblFrdCutOff.Text = "カットオフ（％）"
        Me.lblFrdCutOff.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrdESPoint
        '
        Me.lblFrdESPoint.AutoSize = True
        Me.lblFrdESPoint.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrdESPoint.Location = New System.Drawing.Point(228, 3)
        Me.lblFrdESPoint.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrdESPoint.Name = "lblFrdESPoint"
        Me.lblFrdESPoint.Size = New System.Drawing.Size(165, 30)
        Me.lblFrdESPoint.TabIndex = 1
        Me.lblFrdESPoint.Text = "ＥＳポイント（％）"
        Me.lblFrdESPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrd_1
        '
        Me.lblFrd_1.AutoSize = True
        Me.lblFrd_1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrd_1.Location = New System.Drawing.Point(3, 39)
        Me.lblFrd_1.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrd_1.Name = "lblFrd_1"
        Me.lblFrd_1.Size = New System.Drawing.Size(50, 25)
        Me.lblFrd_1.TabIndex = 2
        Me.lblFrd_1.Text = "１．"
        Me.lblFrd_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrd_2
        '
        Me.lblFrd_2.AutoSize = True
        Me.lblFrd_2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrd_2.Location = New System.Drawing.Point(3, 70)
        Me.lblFrd_2.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrd_2.Name = "lblFrd_2"
        Me.lblFrd_2.Size = New System.Drawing.Size(50, 25)
        Me.lblFrd_2.TabIndex = 5
        Me.lblFrd_2.Text = "２．"
        Me.lblFrd_2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrd_3
        '
        Me.lblFrd_3.AutoSize = True
        Me.lblFrd_3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrd_3.Location = New System.Drawing.Point(3, 101)
        Me.lblFrd_3.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrd_3.Name = "lblFrd_3"
        Me.lblFrd_3.Size = New System.Drawing.Size(50, 25)
        Me.lblFrd_3.TabIndex = 8
        Me.lblFrd_3.Text = "３．"
        Me.lblFrd_3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrd_4
        '
        Me.lblFrd_4.AutoSize = True
        Me.lblFrd_4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrd_4.Location = New System.Drawing.Point(3, 132)
        Me.lblFrd_4.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrd_4.Name = "lblFrd_4"
        Me.lblFrd_4.Size = New System.Drawing.Size(50, 25)
        Me.lblFrd_4.TabIndex = 11
        Me.lblFrd_4.Text = "４．"
        Me.lblFrd_4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrd_5
        '
        Me.lblFrd_5.AutoSize = True
        Me.lblFrd_5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrd_5.Location = New System.Drawing.Point(3, 163)
        Me.lblFrd_5.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrd_5.Name = "lblFrd_5"
        Me.lblFrd_5.Size = New System.Drawing.Size(50, 25)
        Me.lblFrd_5.TabIndex = 14
        Me.lblFrd_5.Text = "５．"
        Me.lblFrd_5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFrd_6
        '
        Me.lblFrd_6.AutoSize = True
        Me.lblFrd_6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFrd_6.Location = New System.Drawing.Point(3, 194)
        Me.lblFrd_6.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFrd_6.Name = "lblFrd_6"
        Me.lblFrd_6.Size = New System.Drawing.Size(50, 30)
        Me.lblFrd_6.TabIndex = 17
        Me.lblFrd_6.Text = "６．"
        Me.lblFrd_6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OKButton
        '
        Me.OKButton.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.OKButton.Location = New System.Drawing.Point(34, 415)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(124, 51)
        Me.OKButton.TabIndex = 13
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.CancelButton.Location = New System.Drawing.Point(370, 415)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(124, 51)
        Me.CancelButton.TabIndex = 15
        Me.CancelButton.Text = "CANCEL"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'EntryButton
        '
        Me.EntryButton.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.EntryButton.Location = New System.Drawing.Point(202, 415)
        Me.EntryButton.Name = "EntryButton"
        Me.EntryButton.Size = New System.Drawing.Size(124, 51)
        Me.EntryButton.TabIndex = 14
        Me.EntryButton.Text = "ENTRY"
        Me.EntryButton.UseVisualStyleBackColor = True
        '
        'LabelErrorMessage
        '
        Me.LabelErrorMessage.AutoSize = True
        Me.LabelErrorMessage.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelErrorMessage.ForeColor = System.Drawing.Color.Red
        Me.LabelErrorMessage.Location = New System.Drawing.Point(24, 363)
        Me.LabelErrorMessage.Name = "LabelErrorMessage"
        Me.LabelErrorMessage.Size = New System.Drawing.Size(476, 19)
        Me.LabelErrorMessage.TabIndex = 401
        Me.LabelErrorMessage.Text = "下限値[-99.9999]以下の値が設定されています。"
        '
        'Timer1
        '
        '
        'FormCutOffEsPointEnter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(527, 498)
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelErrorMessage)
        Me.Controls.Add(Me.EntryButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.pnlFirstResData)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormCutOffEsPointEnter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "カットオフ・ＥＳポイント設定"
        Me.pnlFirstResData.ResumeLayout(False)
        Me.tlpFirstResData.ResumeLayout(False)
        Me.tlpFirstResData.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlFirstResData As Panel
    Private WithEvents tlpFirstResData As TableLayoutPanel
    Private WithEvents lblFrdCutOff As Label
    Private WithEvents lblFrdESPoint As Label
    Private WithEvents lblFrd_1 As Label
    Private WithEvents lblFrd_2 As Label
    Private WithEvents lblFrd_3 As Label
    Private WithEvents lblFrd_4 As Label
    Private WithEvents lblFrd_5 As Label
    Private WithEvents lblFrd_6 As Label
    Private WithEvents lblFrdNomVal As Label
    Private WithEvents lblFrdNom As Label
    Friend WithEvents lblFrdC1 As TextBox
    Friend WithEvents lblFrdE5 As TextBox
    Friend WithEvents lblFrdC2 As TextBox
    Friend WithEvents lblFrdC3 As TextBox
    Friend WithEvents lblFrdC4 As TextBox
    Friend WithEvents lblFrdC5 As TextBox
    Friend WithEvents lblFrdC6 As TextBox
    Friend WithEvents lblFrdE1 As TextBox
    Friend WithEvents lblFrdE2 As TextBox
    Friend WithEvents lblFrdE3 As TextBox
    Friend WithEvents lblFrdE4 As TextBox
    Friend WithEvents lblFrdE6 As TextBox
    Friend WithEvents OKButton As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents EntryButton As Button
    Friend WithEvents LabelErrorMessage As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
