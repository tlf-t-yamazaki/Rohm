'==============================================================================
'   Description : シンプルトリマ用モジュールファイル
'                 ブロック単位のデータ表示のブロック番号指定フォーム処理
'
'   V2.0.0.0⑩
'
'　 2014/07/22 First Written by N.Arata(OLFT) 
'
'==============================================================================
Public Class FrmBlockDisp
    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        CmbSelBlockNo.Items.Clear()

        Dim sList(0) As String
        ReDim sList(TrimData.GetBlockNumber() - 1)

        For Cnt As Integer = 1 To TrimData.GetBlockNumber()
            sList(Cnt - 1) = Cnt.ToString()
        Next
        CmbSelBlockNo.Items.AddRange(sList)
        'V4.0.0.0-73　↓
        If sList.Length <> 0 Then
            CmbSelBlockNo.SelectedIndex = 0
        End If
        'V4.0.0.0-73　↑
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        'V4.0.0.0-73　↓
        If CmbSelBlockNo.Text <> "" Then
            Call SimpleTrimmer.ResistorDataDisplay(True, Integer.Parse(CmbSelBlockNo.Text), 1)
        End If
        'V4.0.0.0-73　↑
        Me.Close()
    End Sub

    Private Sub EndButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'V4.0.0.0-73　↓
        Dim tmpBlockNo As Integer
        tmpBlockNo = 0
        If CmbSelBlockNo.Text <> "" Then
            tmpBlockNo = Integer.Parse(CmbSelBlockNo.Text)
        End If
        '        Call SimpleTrimmer.ResistorDataDisplay(False, Integer.Parse(CmbSelBlockNo.Text), 1)
        Call SimpleTrimmer.ResistorDataDisplay(False, tmpBlockNo, 1)
        'V4.0.0.0-73　↑
        Me.Close()
    End Sub
End Class