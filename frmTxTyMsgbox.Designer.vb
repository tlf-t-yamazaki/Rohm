<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTxTyMsgbox
#Region "Windows フォーム デザイナによって生成されたコード "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'この呼び出しは、Windows フォーム デザイナで必要です。
		InitializeComponent()
		Form_Initialize_renamed()
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
    Public WithEvents cmdOKTxTy As System.Windows.Forms.Button
	Public WithEvents cmdCAN As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
	'Windows フォーム デザイナを使って変更できます。
	'コード エディタを使用して、変更しないでください。
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTxTyMsgbox))
        Me.cmdOKTxTy = New System.Windows.Forms.Button()
        Me.cmdCAN = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdOKTxTy
        '
        resources.ApplyResources(Me.cmdOKTxTy, "cmdOKTxTy")
        Me.cmdOKTxTy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOKTxTy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOKTxTy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOKTxTy.Name = "cmdOKTxTy"
        Me.cmdOKTxTy.UseVisualStyleBackColor = False
        '
        'cmdCAN
        '
        resources.ApplyResources(Me.cmdCAN, "cmdCAN")
        Me.cmdCAN.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCAN.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCAN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCAN.Name = "cmdCAN"
        Me.cmdCAN.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        resources.ApplyResources(Me.cmdOK, "cmdOK")
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'frmTxTyMsgbox
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.cmdOKTxTy)
        Me.Controls.Add(Me.cmdCAN)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTxTyMsgbox"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class