<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmWait
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmWait))
        Me.Grp1 = New System.Windows.Forms.GroupBox()
        Me.MessageLabel2 = New System.Windows.Forms.Label()
        Me.MessageLabel1 = New System.Windows.Forms.Label()
        Me.Grp1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grp1
        '
        resources.ApplyResources(Me.Grp1, "Grp1")
        Me.Grp1.Controls.Add(Me.MessageLabel2)
        Me.Grp1.Controls.Add(Me.MessageLabel1)
        Me.Grp1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Grp1.Name = "Grp1"
        Me.Grp1.TabStop = False
        '
        'MessageLabel2
        '
        resources.ApplyResources(Me.MessageLabel2, "MessageLabel2")
        Me.MessageLabel2.ForeColor = System.Drawing.Color.Red
        Me.MessageLabel2.Name = "MessageLabel2"
        '
        'MessageLabel1
        '
        resources.ApplyResources(Me.MessageLabel1, "MessageLabel1")
        Me.MessageLabel1.ForeColor = System.Drawing.Color.Blue
        Me.MessageLabel1.Name = "MessageLabel1"
        '
        'FrmWait
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlBox = False
        Me.Controls.Add(Me.Grp1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmWait"
        Me.TopMost = True
        Me.Grp1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grp1 As System.Windows.Forms.GroupBox
    Friend WithEvents MessageLabel2 As System.Windows.Forms.Label
    Friend WithEvents MessageLabel1 As System.Windows.Forms.Label
End Class
