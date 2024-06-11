<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCutOffEnter
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
        Me.EntryButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.LabelErrorMessage = New System.Windows.Forms.Label()
        Me.pnlFirstResDataNET = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBoxResNo = New System.Windows.Forms.ComboBox()
        Me.lblNETNomVal = New System.Windows.Forms.Label()
        Me.lblNETNom = New System.Windows.Forms.Label()
        Me.tlpFirstResDataNET = New System.Windows.Forms.TableLayoutPanel()
        Me.ResCut10 = New System.Windows.Forms.TextBox()
        Me.ResCut9 = New System.Windows.Forms.TextBox()
        Me.ResCut8 = New System.Windows.Forms.TextBox()
        Me.ResCut7 = New System.Windows.Forms.TextBox()
        Me.ResCut6 = New System.Windows.Forms.TextBox()
        Me.ResCut5 = New System.Windows.Forms.TextBox()
        Me.ResCut4 = New System.Windows.Forms.TextBox()
        Me.ResCut3 = New System.Windows.Forms.TextBox()
        Me.ResCut2 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.ResCut1 = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlFirstResDataNET.SuspendLayout()
        Me.tlpFirstResDataNET.SuspendLayout()
        Me.SuspendLayout()
        '
        'EntryButton
        '
        Me.EntryButton.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.EntryButton.Location = New System.Drawing.Point(198, 568)
        Me.EntryButton.Name = "EntryButton"
        Me.EntryButton.Size = New System.Drawing.Size(124, 51)
        Me.EntryButton.TabIndex = 22
        Me.EntryButton.Text = "ENTRY"
        Me.EntryButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.CancelButton.Location = New System.Drawing.Point(366, 568)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(124, 51)
        Me.CancelButton.TabIndex = 23
        Me.CancelButton.Text = "CANCEL"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'OKButton
        '
        Me.OKButton.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.OKButton.Location = New System.Drawing.Point(30, 568)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(124, 51)
        Me.OKButton.TabIndex = 21
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'LabelErrorMessage
        '
        Me.LabelErrorMessage.AutoSize = True
        Me.LabelErrorMessage.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LabelErrorMessage.ForeColor = System.Drawing.Color.Red
        Me.LabelErrorMessage.Location = New System.Drawing.Point(26, 511)
        Me.LabelErrorMessage.Name = "LabelErrorMessage"
        Me.LabelErrorMessage.Size = New System.Drawing.Size(476, 19)
        Me.LabelErrorMessage.TabIndex = 402
        Me.LabelErrorMessage.Text = "下限値[-99.9999]以下の値が設定されています。"
        '
        'pnlFirstResDataNET
        '
        Me.pnlFirstResDataNET.Controls.Add(Me.Label1)
        Me.pnlFirstResDataNET.Controls.Add(Me.ComboBoxResNo)
        Me.pnlFirstResDataNET.Controls.Add(Me.lblNETNomVal)
        Me.pnlFirstResDataNET.Controls.Add(Me.lblNETNom)
        Me.pnlFirstResDataNET.Controls.Add(Me.tlpFirstResDataNET)
        Me.pnlFirstResDataNET.Location = New System.Drawing.Point(101, 33)
        Me.pnlFirstResDataNET.Name = "pnlFirstResDataNET"
        Me.pnlFirstResDataNET.Size = New System.Drawing.Size(360, 419)
        Me.pnlFirstResDataNET.TabIndex = 404
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(10, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 20)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "抵抗番号："
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboBoxResNo
        '
        Me.ComboBoxResNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ComboBoxResNo.FormattingEnabled = True
        Me.ComboBoxResNo.Location = New System.Drawing.Point(111, 11)
        Me.ComboBoxResNo.MaxLength = 3
        Me.ComboBoxResNo.Name = "ComboBoxResNo"
        Me.ComboBoxResNo.Size = New System.Drawing.Size(70, 29)
        Me.ComboBoxResNo.TabIndex = 1
        '
        'lblNETNomVal
        '
        Me.lblNETNomVal.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblNETNomVal.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblNETNomVal.Location = New System.Drawing.Point(86, 52)
        Me.lblNETNomVal.Name = "lblNETNomVal"
        Me.lblNETNomVal.Size = New System.Drawing.Size(270, 20)
        Me.lblNETNomVal.TabIndex = 4
        Me.lblNETNomVal.Text = "xxxxx.xxxxx"
        Me.lblNETNomVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNETNom
        '
        Me.lblNETNom.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblNETNom.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblNETNom.Location = New System.Drawing.Point(10, 52)
        Me.lblNETNom.Name = "lblNETNom"
        Me.lblNETNom.Size = New System.Drawing.Size(75, 20)
        Me.lblNETNom.TabIndex = 3
        Me.lblNETNom.Text = "目標値："
        Me.lblNETNom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tlpFirstResDataNET
        '
        Me.tlpFirstResDataNET.ColumnCount = 2
        Me.tlpFirstResDataNET.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.tlpFirstResDataNET.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300.0!))
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut10, 1, 10)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut9, 1, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut8, 1, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut7, 1, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut6, 1, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut5, 1, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut4, 1, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut3, 1, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut2, 1, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label8, 0, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label9, 0, 1)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label10, 0, 2)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label11, 0, 3)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label12, 0, 4)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label13, 0, 5)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label19, 0, 6)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label20, 0, 7)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label21, 0, 8)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label22, 0, 9)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label23, 0, 10)
        Me.tlpFirstResDataNET.Controls.Add(Me.Label24, 1, 0)
        Me.tlpFirstResDataNET.Controls.Add(Me.ResCut1, 1, 1)
        Me.tlpFirstResDataNET.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tlpFirstResDataNET.Location = New System.Drawing.Point(0, 86)
        Me.tlpFirstResDataNET.Name = "tlpFirstResDataNET"
        Me.tlpFirstResDataNET.RowCount = 11
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tlpFirstResDataNET.Size = New System.Drawing.Size(360, 333)
        Me.tlpFirstResDataNET.TabIndex = 2
        '
        'ResCut10
        '
        Me.ResCut10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut10.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut10.Location = New System.Drawing.Point(70, 303)
        Me.ResCut10.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut10.Name = "ResCut10"
        Me.ResCut10.Size = New System.Drawing.Size(134, 22)
        Me.ResCut10.TabIndex = 12
        Me.ResCut10.Text = "----"
        Me.ResCut10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut9
        '
        Me.ResCut9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut9.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut9.Location = New System.Drawing.Point(70, 273)
        Me.ResCut9.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut9.Name = "ResCut9"
        Me.ResCut9.Size = New System.Drawing.Size(134, 22)
        Me.ResCut9.TabIndex = 11
        Me.ResCut9.Text = "----"
        Me.ResCut9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut8
        '
        Me.ResCut8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut8.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut8.Location = New System.Drawing.Point(70, 243)
        Me.ResCut8.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut8.Name = "ResCut8"
        Me.ResCut8.Size = New System.Drawing.Size(134, 22)
        Me.ResCut8.TabIndex = 10
        Me.ResCut8.Text = "----"
        Me.ResCut8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut7
        '
        Me.ResCut7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut7.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut7.Location = New System.Drawing.Point(70, 213)
        Me.ResCut7.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut7.Name = "ResCut7"
        Me.ResCut7.Size = New System.Drawing.Size(134, 22)
        Me.ResCut7.TabIndex = 9
        Me.ResCut7.Text = "----"
        Me.ResCut7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut6
        '
        Me.ResCut6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut6.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut6.Location = New System.Drawing.Point(70, 183)
        Me.ResCut6.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut6.Name = "ResCut6"
        Me.ResCut6.Size = New System.Drawing.Size(134, 22)
        Me.ResCut6.TabIndex = 8
        Me.ResCut6.Text = "----"
        Me.ResCut6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut5
        '
        Me.ResCut5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut5.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut5.Location = New System.Drawing.Point(70, 153)
        Me.ResCut5.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut5.Name = "ResCut5"
        Me.ResCut5.Size = New System.Drawing.Size(134, 22)
        Me.ResCut5.TabIndex = 7
        Me.ResCut5.Text = "----"
        Me.ResCut5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut4
        '
        Me.ResCut4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut4.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut4.Location = New System.Drawing.Point(70, 123)
        Me.ResCut4.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut4.Name = "ResCut4"
        Me.ResCut4.Size = New System.Drawing.Size(134, 22)
        Me.ResCut4.TabIndex = 6
        Me.ResCut4.Text = "----"
        Me.ResCut4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut3
        '
        Me.ResCut3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut3.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut3.Location = New System.Drawing.Point(70, 93)
        Me.ResCut3.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut3.Name = "ResCut3"
        Me.ResCut3.Size = New System.Drawing.Size(134, 22)
        Me.ResCut3.TabIndex = 5
        Me.ResCut3.Text = "----"
        Me.ResCut3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResCut2
        '
        Me.ResCut2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut2.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut2.Location = New System.Drawing.Point(70, 63)
        Me.ResCut2.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut2.Name = "ResCut2"
        Me.ResCut2.Size = New System.Drawing.Size(134, 22)
        Me.ResCut2.TabIndex = 4
        Me.ResCut2.Text = "----"
        Me.ResCut2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label8.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label8.Location = New System.Drawing.Point(3, 3)
        Me.Label8.Margin = New System.Windows.Forms.Padding(3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(0, 12)
        Me.Label8.TabIndex = 0
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(3, 33)
        Me.Label9.Margin = New System.Windows.Forms.Padding(3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 24)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "1."
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label10.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label10.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label10.Location = New System.Drawing.Point(3, 63)
        Me.Label10.Margin = New System.Windows.Forms.Padding(3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(54, 24)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "2."
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label11.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label11.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label11.Location = New System.Drawing.Point(3, 93)
        Me.Label11.Margin = New System.Windows.Forms.Padding(3)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(54, 24)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "3."
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label12.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label12.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label12.Location = New System.Drawing.Point(3, 123)
        Me.Label12.Margin = New System.Windows.Forms.Padding(3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(54, 24)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "4."
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label13.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label13.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label13.Location = New System.Drawing.Point(3, 153)
        Me.Label13.Margin = New System.Windows.Forms.Padding(3)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(54, 24)
        Me.Label13.TabIndex = 5
        Me.Label13.Text = "5."
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label19.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label19.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label19.Location = New System.Drawing.Point(3, 183)
        Me.Label19.Margin = New System.Windows.Forms.Padding(3)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(54, 24)
        Me.Label19.TabIndex = 6
        Me.Label19.Text = "6."
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label20.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label20.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label20.Location = New System.Drawing.Point(3, 213)
        Me.Label20.Margin = New System.Windows.Forms.Padding(3)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(54, 24)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "7."
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label21.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label21.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label21.Location = New System.Drawing.Point(3, 243)
        Me.Label21.Margin = New System.Windows.Forms.Padding(3)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(54, 24)
        Me.Label21.TabIndex = 8
        Me.Label21.Text = "8."
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label22.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label22.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label22.Location = New System.Drawing.Point(3, 273)
        Me.Label22.Margin = New System.Windows.Forms.Padding(3)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(54, 24)
        Me.Label22.TabIndex = 9
        Me.Label22.Text = "9."
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label23.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label23.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label23.Location = New System.Drawing.Point(3, 303)
        Me.Label23.Margin = New System.Windows.Forms.Padding(3)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(54, 27)
        Me.Label23.TabIndex = 10
        Me.Label23.Text = "10."
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label24.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label24.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label24.Location = New System.Drawing.Point(90, 3)
        Me.Label24.Margin = New System.Windows.Forms.Padding(30, 3, 3, 3)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(267, 24)
        Me.Label24.TabIndex = 11
        Me.Label24.Text = "カットオフ"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ResCut1
        '
        Me.ResCut1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResCut1.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ResCut1.Location = New System.Drawing.Point(70, 33)
        Me.ResCut1.Margin = New System.Windows.Forms.Padding(10, 3, 156, 3)
        Me.ResCut1.Name = "ResCut1"
        Me.ResCut1.Size = New System.Drawing.Size(134, 22)
        Me.ResCut1.TabIndex = 3
        Me.ResCut1.Text = "----"
        Me.ResCut1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Timer1
        '
        '
        'FormCutOffEnter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 671)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlFirstResDataNET)
        Me.Controls.Add(Me.LabelErrorMessage)
        Me.Controls.Add(Me.EntryButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.OKButton)
        Me.Name = "FormCutOffEnter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FormCutOffEnter"
        Me.pnlFirstResDataNET.ResumeLayout(False)
        Me.tlpFirstResDataNET.ResumeLayout(False)
        Me.tlpFirstResDataNET.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EntryButton As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents OKButton As Button
    Friend WithEvents LabelErrorMessage As Label
    Friend WithEvents pnlFirstResDataNET As Panel
    Private WithEvents Label1 As Label
    Friend WithEvents ComboBoxResNo As ComboBox
    Private WithEvents lblNETNomVal As Label
    Private WithEvents lblNETNom As Label
    Friend WithEvents tlpFirstResDataNET As TableLayoutPanel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents ResCut1 As TextBox
    Friend WithEvents ResCut10 As TextBox
    Friend WithEvents ResCut9 As TextBox
    Friend WithEvents ResCut8 As TextBox
    Friend WithEvents ResCut7 As TextBox
    Friend WithEvents ResCut6 As TextBox
    Friend WithEvents ResCut5 As TextBox
    Friend WithEvents ResCut4 As TextBox
    Friend WithEvents ResCut3 As TextBox
    Friend WithEvents ResCut2 As TextBox
    Friend WithEvents Timer1 As Timer
End Class
