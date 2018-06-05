'==============================================================================
'   Description : �V���v���g���}�p���W���[���t�@�C��
'                 �u���b�N�P�ʂ̃f�[�^�\���̃u���b�N�ԍ��w��t�H�[������
'
'   V2.0.0.0�I
'
'�@ 2014/07/22 First Written by N.Arata(OLFT) 
'
'==============================================================================
Public Class FrmBlockDisp
    Public Sub New()

        ' ���̌Ăяo���́AWindows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()

        ' InitializeComponent() �Ăяo���̌�ŏ�������ǉ����܂��B
        CmbSelBlockNo.Items.Clear()

        Dim sList(0) As String
        ReDim sList(TrimData.GetBlockNumber() - 1)

        For Cnt As Integer = 1 To TrimData.GetBlockNumber()
            sList(Cnt - 1) = Cnt.ToString()
        Next
        CmbSelBlockNo.Items.AddRange(sList)
        'V4.0.0.0-73�@��
        If sList.Length <> 0 Then
            CmbSelBlockNo.SelectedIndex = 0
        End If
        'V4.0.0.0-73�@��
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        'V4.0.0.0-73�@��
        If CmbSelBlockNo.Text <> "" Then
            Call SimpleTrimmer.ResistorDataDisplay(True, Integer.Parse(CmbSelBlockNo.Text), 1)
        End If
        'V4.0.0.0-73�@��
        Me.Close()
    End Sub

    Private Sub EndButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'V4.0.0.0-73�@��
        Dim tmpBlockNo As Integer
        tmpBlockNo = 0
        If CmbSelBlockNo.Text <> "" Then
            tmpBlockNo = Integer.Parse(CmbSelBlockNo.Text)
        End If
        '        Call SimpleTrimmer.ResistorDataDisplay(False, Integer.Parse(CmbSelBlockNo.Text), 1)
        Call SimpleTrimmer.ResistorDataDisplay(False, tmpBlockNo, 1)
        'V4.0.0.0-73�@��
        Me.Close()
    End Sub
End Class