Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Windows.Forms

Public Class CustomTkyLabel
    Inherits System.Windows.Forms.Label

#Region "BorderColor"
    Private _borderColor As Color = SystemColors.ControlText
    <EditorBrowsable(EditorBrowsableState.Always), Browsable(True), _
      DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), _
      DefaultValue(GetType(Color), "ControlText"), Category("カスタム"), _
      Description("境界線の色です。BorderStyleプロパティがFixedSingleの場合のみ有効です。")> _
      Public Property BorderColor() As Color
        Get
            Return Me._borderColor
        End Get
        Set(ByVal value As Color)
            Me._borderColor = value
            Me.Invalidate()
        End Set
    End Property
#End Region

#Region "BorderWidth"
    Private _borderWidth As Integer = 1
    <EditorBrowsable(EditorBrowsableState.Always), Browsable(True), _
      DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), _
      DefaultValue(GetType(Integer)), Category("カスタム"), _
      Description("境界線の太さです。BorderStyleプロパティがFixedSingleの場合のみ有効です。")> _
    Public Property BorderWidth() As Integer
        Get
            Return Me._borderWidth
        End Get
        Set(ByVal value As Integer)
            Me._borderWidth = value
            Me.Invalidate()
            Me.Padding = New Padding(value + 1)
            If (0 = value) Then
                Me.BorderStyle = System.Windows.Forms.BorderStyle.None
            Else
                Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            End If
        End Set
    End Property
#End Region

    Private Const WM_PAINT As Integer = &HF

    Public Sub New()
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Me.Padding = New Padding(2)
        Me.TextAlign = ContentAlignment.MiddleCenter

    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_PAINT
                MyBase.WndProc(m)
                Call DrawRectangle()
            Case Else
                MyBase.WndProc(m)
        End Select

    End Sub

    Protected Overridable Sub DrawRectangle()
        If Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle Then
            Dim g As Graphics = Me.CreateGraphics()
            Dim rect As Rectangle = Me.ClientRectangle
            Dim LinePen As New Pen(Me.BorderColor, _borderWidth)
            Dim x As Integer = (rect.X - 1)
            Dim y As Integer = (rect.Y - 1)
            Dim w As Integer = (rect.Width - _borderWidth + 2)
            Dim h As Integer = (rect.Height - _borderWidth + 2)

            If (0 = (_borderWidth Mod 2)) Then
                x = CInt(x + _borderWidth / 2)
                y = CInt(y + _borderWidth / 2)
            Else
                x = CInt(x + (_borderWidth - 1) / 2)
                y = CInt(y + (_borderWidth - 1) / 2)
            End If

            Try
                g.DrawRectangle(LinePen, x, y, w, h)

            Finally
                g.Dispose()
                LinePen.Dispose()
            End Try
        End If

    End Sub

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CustomTkyLabel))
        Me.SuspendLayout()
        '
        'CustomTkyLabel
        '
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me, "$this")
        Me.ResumeLayout(False)

    End Sub

End Class

