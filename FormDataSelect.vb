'===============================================================================
'   Description  : �f�[�^�I����ʏ���(�����^�]�p)
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Public Class FormDataSelect
#Region "�y�ϐ���`�z"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    Private Const DATA_DIR_PATH As String = "C:\TRIMDATA\DATA"          ' �f�[�^�t�@�C���t�H���_(����l)

    '----- ���샂�[�h�̍ő�t�@�C���� -----
    Private Const COUNT_MAGAZINE As Integer = 3                         ' �}�K�W�����[�h(�ő�3�̋����}�K�W��)
    Private Const COUNT_LOT As Integer = 30                             ' ���b�g���[�h(�ő�3�̋����}�K�W���~10���b�g)
    Private Const COUNT_ENDLESS As Integer = 1                          ' �G���h���X���[�h
    Private Const COUNT_MAX As Integer = 30                             ' �ő�t�@�C����

    '----- �ϐ���` -----
    Private mExitFlag As Integer                                        ' �I���t���O

#End Region

#Region "�y���\�b�h��`�z"
#Region "�I�����ʂ�Ԃ�"
    '''=========================================================================
    ''' <summary>�I�����ʂ�Ԃ�</summary>
    ''' <returns>cFRS_ERR_START = OK�{�^������
    '''          cFRS_ERR_RST   = Cancel�{�^������</returns>
    '''=========================================================================
    Public Function sGetReturn() As Integer

        Dim strMSG As String

        Try
            Return (mExitFlag)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.sGetReturn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����"
    '''=========================================================================
    ''' <summary>ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����</summary>
    ''' <param name="Owner">(INP)���g�p</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window)

        Dim strMSG As String

        Try
            ' ��������
            mExitFlag = -1                                              ' �I���t���O = ������

            ' ��ʕ\��
            Me.ShowDialog()                                             ' ��ʕ\��
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Load������"
    '''=========================================================================
    ''' <summary>Form_Load������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FormDataSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String
        Dim lSts As Long = 0                                            ' ###206
        Dim r As Integer = 0                                            ' ###206

        Try
            ' �t�H�[���� 
            'Me.Text = MSG_AUTO_14                                       ' "�f�[�^�I��"

            ' ���샂�[�h�I�����W�I�{�^��
            'GrpMode.Text = MSG_AUTO_01                                  ' "���샂�[�h"
            'BtnMdMagazine.Text = MSG_AUTO_02                            ' "�}�K�W�����[�h"
            'BtnMdLot.Text = MSG_AUTO_03                                 ' "���b�g���[�h"
            'BtnMdEndless.Text = MSG_AUTO_04                             ' "�G���h���X���[�h"
            ' ���x�����E�{�^���� 
            'LblDataFile.Text = MSG_AUTO_05                              ' "�f�[�^�t�@�C��"
            'LblListList.Text = MSG_AUTO_06                              ' "�o�^�ς݃f�[�^�t�@�C��"
            'BtnUp.Text = MSG_AUTO_07                                    ' "���X�g��1���"
            'BtnDown.Text = MSG_AUTO_08                                  ' "���X�g��1����"
            'BtnDelete.Text = MSG_AUTO_09                                ' "���X�g����폜"
            'BtnClear.Text = MSG_AUTO_10                                 ' "���X�g���N���A"
            'BtnSelect.Text = MSG_AUTO_11                                ' "���o�^��"
            'BtnOK.Text = MSG_AUTO_12                                    ' "OK"
            'BtnCancel.Text = MSG_AUTO_13                                ' "�L�����Z��"

            ' ���샂�[�h�I�����W�I�{�^��������
            '----- ###206�� -----
            r = H_Read(LOFS_H00, lSts)                                  ' �����ݒ��Ԏ擾(H0.00-H0.15)
            If (lSts And LHST_MAGAZINE) Then                            ' 4�}�K�W�� ?
                BtnMdEndless.Visible = True                             ' �G���h���X���[�h�\�� 
                BtnMdMagazine.Visible = True                            ' �}�K�W�����[�h�\�� 
                BtnMdLot.Visible = False                                ' ���b�g���[�h��\�� 
                BtnMdEndless.Checked = True                             ' �G���h���X���[�h�Ƀ`�F�b�N�}�[�N��ݒ肷��
            Else
                BtnMdEndless.Visible = True                             ' �G���h���X���[�h�\�� 
                BtnMdMagazine.Visible = False                           ' �}�K�W�����[�h��\�� 
                BtnMdLot.Visible = False                                ' ���b�g���[�h��\�� 
                BtnMdEndless.Checked = True                             ' �G���h���X���[�h�Ƀ`�F�b�N�}�[�N��ݒ肷��
            End If
            'If (giActMode = MODE_MAGAZINE) Then
            '    BtnMdMagazine.Checked = True                            ' �}�K�W�����[�h
            'ElseIf (giActMode = MODE_LOT) Then
            '    BtnMdLot.Checked = True                                 ' ���b�g���[�h
            'Else
            '    BtnMdEndless.Checked = True                             ' �G���h���X���[�h
            'End If
            '----- ###206�� -----

            ' ���X�g�{�b�N�X�N���A
            Call ListList.Items.Clear()                                 ' �u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�N���A

            '----- V1.18.0.0�G�� -----
            ' �o�^�ς݃f�[�^�t�@�C�����X�g�{�b�N�X�Ƀ��[�h�ς݃f�[�^����\������(�I�v�V����)
            If (giAutoModeDataSelect = 1) And (gStrTrimFileName <> "") Then ' ���[�h�ς݃f�[�^����\������ ?
                Call Sub_GetFileFolder(gStrTrimFileName, DrvListBox.Drive, DirListBox.Path)
                MakeFileList()                                              ' ���ʏ��DirListBox_Change()�C�x���g����������̂ŕs�v����
                Call AddFileNameToListList(gStrTrimFileName)
            Else
                ' �u�f�[�^�t�@�C���v���X�g�{�b�N�X�ɓ��t�t���t�@�C������\������
                DrvListBox.Drive = "C:"                                     ' �h���C�u 
                DirListBox.Path = DATA_DIR_PATH                             ' �f�B���N�g�����X�g�{�b�N�X����l
                MakeFileList()                                              ' ���ʏ��DirListBox_Change()�C�x���g����������̂ŕs�v����
                '                                                           ' �J�����g��"C:\TRIMDATA\DATA"���Ɣ������Ȃ��̂ŕK�v
            End If
            '----- V1.18.0.0�G�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.FormDataSelect_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   �{�^���������̏���
    '========================================================================================
#Region "OK�{�^������������"
    '''=========================================================================
    ''' <summary>OK�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        Dim r As Integer
        Dim Count As Integer
        Dim Idx As Integer
        Dim strDAT As String
        Dim strMSG As String = ""

        Try
            ' �I�����X�g1�ȏ�L�肩�`�F�b�N���� ?
            If (ListList.Items.Count < 1) Then
                '"�f�[�^�t�@�C����I�����Ă��������B"
                Call MsgBox(MSG_AUTO_18, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            ' �G���h���X���[�h�őI�����X�g2�ȏ�L��Ȃ�G���[�Ƃ���
            If (BtnMdEndless.Checked = True) And (ListList.Items.Count > 1) Then
                '"�G���h���X���[�h���͕����̃f�[�^�t�@�C���͑I���ł��܂���B"
                Call MsgBox(MSG_AUTO_17, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            ' �I���f�[�^�ɑΉ�������H�����t�@�C�������݂��邩�`�F�b�N����(FL��)
            For Idx = 0 To ListList.Items.Count - 1
                strDAT = FileLstBox.Path + "\" + ListList.Items(Idx)
                r = GetFLCndFileName(strDAT, strMSG, True)              ' ���݃`�F�b�N 
                If (r <> SerialErrorCode.rRS_OK) Then                   ' ���H�����t�@�C�������݂��Ȃ� ?
                    ' "���H�����t�@�C�������݂��܂���B(���H�����t�@�C����)"
                    strMSG = MSG_AUTO_20 + "(" + strMSG + ")"
                    Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                    ListList.SelectedIndex = Idx
                    Call ListList_Click(sender, e)                      ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��
                    Exit Sub
                End If
            Next Idx

            ' �A���^�]�p�̃f�[�^�t�@�C�����ƃf�[�^�t�@�C�����z����O���[�o���̈�ɐݒ肷��
            giAutoDataFileNum = ListList.Items.Count                    ' �f�[�^�t�@�C����
            Count = giAutoDataFileNum
            If (Count < COUNT_MAX) Then
                Count = COUNT_MAX
            End If
            ReDim gsAutoDataFileFullPath(Count - 1)
            For Idx = 0 To giAutoDataFileNum - 1                        ' �f�[�^�t�@�C����
                gsAutoDataFileFullPath(Idx) = FileLstBox.Path + "\" + ListList.Items(Idx)
            Next

            ' ���샂�[�h�I�����W�I�{�^�����A���^�]���샂�[�h��ݒ肷��
            If (BtnMdMagazine.Checked = True) Then
                giActMode = MODE_MAGAZINE                               ' �}�K�W�����[�h
            ElseIf (BtnMdLot.Checked = True) Then
                giActMode = MODE_LOT                                    ' ���b�g���[�h
            Else
                giActMode = MODE_ENDLESS                                ' �G���h���X���[�h
            End If

            ' ���샂�[�h�ɂ�莩���^�]����t�@�C�������Đݒ肷��
            Call Sub_AddFileName(giActMode)

            mExitFlag = cFRS_ERR_START                                  ' Return�l = OK�{�^������ 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' �t�H�[�������
    End Sub
#End Region

#Region "���샂�[�h�ɂ�莩���^�]����t�@�C�������Đݒ肷��"
    '''=========================================================================
    ''' <summary>���샂�[�h�ɂ�莩���^�]����t�@�C�������Đݒ肷��</summary>
    ''' <param name="ActMode">(INP)���샂�[�h</param>
    '''=========================================================================
    Private Sub Sub_AddFileName(ByVal ActMode As Integer)

        Dim Count As Integer
        Dim Idx As Integer
        Dim strFNAM As String
        Dim strMSG As String

        Try
            ' ���샂�[�h�ɂ�莩���^�]����t�@�C�������Đݒ肷��
            If (ActMode = MODE_MAGAZINE) Then
                Count = COUNT_MAGAZINE                                  ' �}�K�W�����[�h
            ElseIf (ActMode = MODE_LOT) Then
                Count = COUNT_LOT                                       ' ���b�g���[�h
            Else
                Count = COUNT_ENDLESS                                   ' �G���h���X���[�h
            End If

            ' �t�@�C����������Ȃ����A�Ō�̃f�[�^�t�@�C������ݒ肷��
            Idx = giAutoDataFileNum - 1                                 ' ��ʎw��̃f�[�^�t�@�C����
            strFNAM = gsAutoDataFileFullPath(Idx)                       ' ��ʎw��̍Ō�̃f�[�^�t�@�C���� 
            For Idx = giAutoDataFileNum To Count - 1
                gsAutoDataFileFullPath(Idx) = strFNAM
            Next Idx
            giAutoDataFileNum = Count

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.Sub_AddFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Cancel�{�^������������"
    '''=========================================================================
    ''' <summary>Cancel�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            mExitFlag = cFRS_ERR_RST                                    ' Return�l = Cancel�{�^������

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' �t�H�[�������
    End Sub
#End Region

#Region "�u���X�g�̂P��ցv�{�^������������"
    '''=========================================================================
    ''' <summary>�u���X�g�̂P��ցv�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUp.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' �P��̃C���f�b�N�X���w�肷��
            Idx = ListList.SelectedIndex
            If (Idx <= 0) Then Exit Sub
            ListList.SelectedIndex = Idx - 1

            ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��
            Call ListList_Click(sender, e)

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnUp_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u���X�g�̂P���ցv�{�^������������"
    '''=========================================================================
    ''' <summary>�u���X�g�̂P���ցv�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDown.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' �P���̃C���f�b�N�X���w�肷��
            Idx = ListList.SelectedIndex + 1
            If (Idx >= ListList.Items.Count) Then Exit Sub
            ListList.SelectedIndex = Idx

            ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��
            Call ListList_Click(sender, e)

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnDown_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u���X�g����폜�v�{�^������������"
    '''=========================================================================
    ''' <summary>�u���X�g����폜�v�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' �u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X����1�폜����
            Idx = ListList.SelectedIndex
            If (Idx < 0) Then Exit Sub
            ListList.Items.RemoveAt(Idx)                                  '�u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X����1���ڍ폜(��Remove()�͕�����w��)

            ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��(�폜�̂P�O�̃f�[�^��I����ԂƂ���)
            If (Idx > 0) Then
                Idx = Idx - 1
                ListList.SelectedIndex = Idx
            End If
            Call ListList_Click(sender, e)

            ' �G���h���X���[�h����
            Call DspEndless()

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnDelete_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u���X�g���N���A�v�{�^������������"
    '''=========================================================================
    ''' <summary>�u���X�g���N���A�v�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>�u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X����S�č폜</remarks>
    '''=========================================================================
    Private Sub BtnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClear.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �폜�m�F���b�Z�[�W�\��
            If (ListList.Items.Count < 1) Then Exit Sub '               ' �o�^���X�g�Ȃ��Ȃ�NOP 
            ' "�o�^���X�g��S�č폜���܂��B" & vbCrLf & "��낵���ł����H"
            strMSG = MSG_AUTO_15 & vbCrLf & MSG_AUTO_16
            r = MsgBox(strMSG, MsgBoxStyle.OkCancel, "")
            If (r <> MsgBoxResult.Ok) Then Exit Sub

            ' �u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�N���A
            Call ListList.Items.Clear()                                 '�u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�N���A

            ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��(�N���A����)
            Call ListList_Click(sender, e)

            ' �G���h���X���[�h����
            Call DspEndless()

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnClear_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�o�^�{�^������������"
    '''=========================================================================
    ''' <summary>�o�^�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelect.Click

        Dim Idx As Integer
        Dim Sz As Integer
        Dim Pos As Integer
        Dim strDAT As String
        Dim strMSG As String

        Try
            '�u�f�[�^�t�@�C���v���X�g�{�b�N�X�C���f�b�N�X�����Ȃ�NOP
            Idx = ListFile.SelectedIndex
            If (Idx < 0) Then Exit Sub
            ' �G���h���X���[�h�őI�����X�g1�ȏ�L��Ȃ�NOP
            If (BtnMdEndless.Checked = True) And (ListList.Items.Count >= 1) Then Exit Sub ' �G���h���X���[�h ?

            ' �w��̃f�[�^�t�@�C�������u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�ɒǉ�����
            strDAT = ListFile.Items(Idx)                                ' ���t�����t���t�@�C�����Ȃ̂Ńt�@�C�����̂ݎ��o�� 
            Sz = strDAT.Length
            Pos = strDAT.LastIndexOf(" ")
            If (Pos = -1) Then Exit Sub
            Pos = 19                                                    ' Pos = 19("YYYY/MM/DD HH:MM:ss ") ###225
            strDAT = strDAT.Substring(Pos + 1, Sz - Pos - 1)
            Idx = ListList.Items.Count
            Call ListList.Items.Add(strDAT)
            ListList.SelectedIndex = Idx

            ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��
            Call ListList_Click(sender, e)

            ' �G���h���X���[�h����
            Call DspEndless()

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnSelect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   ���X�g�{�b�N�X�̃N���b�N�C�x���g����
    '========================================================================================
#Region "�u�f�[�^�t�@�C���v���X�g�{�b�N�X�_�u���N���b�N�C�x���g����"
    '''=========================================================================
    ''' <summary>�u�f�[�^�t�@�C���v���X�g�{�b�N�X�_�u���N���b�N�C�x���g����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListFile_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListFile.DoubleClick
        Dim strMSG As String

        Try
            ' �o�^�{�^��������������
            Call BtnSelect_Click(sender, e)
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.ListFile_DoubleClick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�N���b�N�C�x���g����"
    '''=========================================================================
    ''' <summary>�u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�N���b�N�C�x���g����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListList.Click

        Dim Idx As Integer
        'Dim strDAT As String
        Dim strMSG As String

        Try
            ' �u�o�^�ς݃f�[�^�t�@�C���v���X�g�{�b�N�X�őI�����ꂽ�f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��
            Idx = ListList.SelectedIndex
            If (Idx < 0) Then
                LblFullPath.Text = ""
            Else
                LblFullPath.Text = FileLstBox.Path + "\" + ListList.Items(Idx)
            End If
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.ListList_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u�h���C�u���X�g�{�b�N�X�v�̃N���b�N�C�x���g����"
    '''=========================================================================
    ''' <summary>�u�h���C�u���X�g�{�b�N�X�v�̃N���b�N�C�x���g����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>DriveListBox�͔�W���R���g���[���Ȃ̂Ńc�[���{�b�N�X�ɒǉ�����K�v�L��</remarks>
    '''=========================================================================
    Private Sub DrvListBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrvListBox.Click

        Dim strMSG As String

        Try
            ' �f�B���N�g�����X�g�{�b�N�X�̑I���h���C�u��ύX����
            DirListBox.Path = DrvListBox.Drive
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = ex.Message
            MsgBox(strMSG)
            DrvListBox.Drive = "C:"
        End Try
    End Sub
#End Region

#Region "�u�f�B���N�g�����X�g�{�b�N�X�v�̕ύX������"
    '''=========================================================================
    ''' <summary>�u�f�B���N�g�����X�g�{�b�N�X�v�̕ύX������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>DirListBox�͔�W���R���g���[���Ȃ̂Ńc�[���{�b�N�X�ɒǉ�����K�v�L��</remarks>
    '''=========================================================================
    Private Sub DirListBox_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DirListBox.Change

        Dim strMSG As String

        Try
            ' �I���f�B���N�g����ύX����(FileLstBox�͍�Ɨp��Dummy)
            FileLstBox.Path = DirListBox.Path

            ' �u�f�[�^�t�@�C���v���X�g�{�b�N�X�ɓ��t�����t���t�@�C������\������
            MakeFileList()
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.DirListBox_Change() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   ���샂�[�h���W�I�{�^���ύX���̏���
    '========================================================================================
#Region "�}�K�W�����[�h�I������"
    '''=========================================================================
    ''' <summary>�}�K�W�����[�h�I������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdMagazine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMdMagazine.CheckedChanged
        Call DspEndless()
    End Sub
#End Region

#Region "���b�g���[�h�I������"
    '''=========================================================================
    ''' <summary>���b�g���[�h�I������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdLot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMdLot.CheckedChanged
        Call DspEndless()
    End Sub
#End Region

#Region "�G���h���X���[�h�I������"
    '''=========================================================================
    ''' <summary>�G���h���X���[�h�I������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdEndless_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMdEndless.CheckedChanged
        Call DspEndless()
    End Sub
#End Region

    '========================================================================================
    '   ���ʊ֐���`
    '========================================================================================
#Region "�u�f�[�^�t�@�C���v���X�g�{�b�N�X�ɓ��t�����t���t�@�C������\������"
    '''=========================================================================
    ''' <summary>�u�f�[�^�t�@�C���v���X�g�{�b�N�X�ɓ��t�����t���t�@�C������\������</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub MakeFileList()

        Dim Count As Integer
        Dim i As Integer
        Dim Sz As Integer
        Dim strWK As String
        Dim strDAT As String
        Dim strMSG As String

        Try
            ' �u�f�[�^�t�@�C���v���X�g�{�b�N�X�ɓ��t�����t���t�@�C������\������
            Call ListFile.Items.Clear()                                                 '�u�f�[�^�t�@�C���v���X�g�{�b�N�X�N���A
            Count = FileLstBox.Items.Count                                              ' �t�@�C���̐� 
            For i = 0 To (Count - 1)
                ' �t�@�C���g���q��ݒ�
                If (gTkyKnd = KND_TKY) Then                                             ' TKY�̏ꍇ(�g�������l������TKY���T�|�[�g����)
                    strWK = ".tdt"
                ElseIf (gTkyKnd = KND_CHIP) Then                                        ' CHIP�̏ꍇ
                    strWK = ".tdc"
                Else
                    strWK = ".tdn"                                                      ' NET�̏ꍇ
                End If
                '----- V4.0.0.0�H�� -----
                If (giMachineKd = MACHINE_KD_RS) Then                                   ' SL436S ?
                    If (gTkyKnd = KND_TKY) Then
                        strWK = ".tdts"
                    ElseIf (gTkyKnd = KND_CHIP) Then
                        strWK = ".tdcs"
                    Else
                        strWK = ".tdns"
                    End If
                End If
                '----- V4.0.0.0�H�� -----

                ' �Ώۂ̊g���q�łȂ����SKIP
                strDAT = FileLstBox.Items(i)
                Sz = strDAT.Length
                '-----V4.0.0.0�H�� -----
                If (giMachineKd = MACHINE_KD_RS) Then                                   ' SL436S ? 
                    If (Sz < 5) Then GoTo STP_NEXT
                    strDAT = strDAT.Substring(Sz - 5, 5)                                ' �g���q�����o��
                Else
                    If (Sz < 4) Then GoTo STP_NEXT
                    strDAT = strDAT.Substring(Sz - 4, 4)                                ' �g���q�����o��
                End If
                'If (Sz < 4) Then GoTo STP_NEXT
                'strDAT = strDAT.Substring(Sz - 4, 4)                                   ' �g���q�����o��
                '----- V4.0.0.0�H�� -----
                If (strDAT <> strWK) Then GoTo STP_NEXT '                               ' �Ώۂ̊g���q�łȂ����SKIP

                ' ���t�����t���t�@�C�����X�g�쐬
                strDAT = FileDateTime(FileLstBox.Path & "\" & FileLstBox.Items(i))
                Dim Dt As DateTime = DateTime.Parse(strDAT)
                strDAT = Dt.ToString("yyyy/MM/dd HH:mm:ss") + " " + FileLstBox.Items(i) ' ���t�����̒��������킹�� 
                Call ListFile.Items.Add(strDAT)                                         ' ���t�����t���t�@�C������\������
STP_NEXT:
            Next i
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.MakeFileList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�G���h���X���[�h����"
    '''=========================================================================
    ''' <summary>�G���h���X���[�h����</summary>
    ''' <remarks>�G���h���X���[�h���̓f�[�^�t�@�C�����P�����I���ł��Ȃ�</remarks>
    '''=========================================================================
    Private Sub DspEndless()

        Dim strMSG As String

        Try
            ' �G���h���X���[�h�őI�����X�g1�ȏ�L��Ȃ牺�L�̃{�^������񊈐����ɂ���
            If (BtnMdEndless.Checked = True) And (ListList.Items.Count >= 1) Then
                ListFile.Enabled = False                                ' �f�[�^�t�@�C�����X�g�{�b�N�X�񊈐���
                BtnSelect.Enabled = False                               ' �o�^�{�^���񊈐��� 
                BtnUp.Enabled = False                                   '�u���X�g�̂P��ցv�{�^���񊈐���
                BtnDown.Enabled = False                                 '�u���X�g�̂P���ցv�{�^���񊈐���
            Else
                ListFile.Enabled = True                                 ' �f�[�^�t�@�C�����X�g�{�b�N�X������
                BtnSelect.Enabled = True                                ' �o�^�{�^�������� 
                BtnUp.Enabled = True                                    '�u���X�g�̂P��ցv�{�^��������
                BtnDown.Enabled = True                                  '�u���X�g�̂P���ցv�{�^��������
            End If

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.DspEndless() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�G�� -----
#Region "�o�^�ς݃f�[�^�t�@�C�����X�g�{�b�N�X�Ƀ��[�h�ς݃f�[�^����\������(�I�v�V����)"
    '''=========================================================================
    ''' <summary>�o�^�ς݃f�[�^�t�@�C�����X�g�{�b�N�X�Ƀ��[�h�ς݃f�[�^����\������(�I�v�V����)</summary>
    ''' <param name="FileName">(INP)�\������t�@�C����</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub AddFileNameToListList(ByRef FileName As String)

        Dim strDAT As String = ""
        Dim strMSG As String

        Try
            ' �t�@�C�����Ȃ��Ȃ�NOP
            If (FileName = "") Then Return

            ' �o�^�ς݃f�[�^�t�@�C�����X�g�{�b�N�X�Ƀ��[�h�ς݃f�[�^����\������
            Sub_GetFileName(FileName, strDAT)                           ' �t�@�C�����݂̂����o�� 
            Call ListList.Items.Add(strDAT)
            ListList.SelectedIndex = 0

            ' �f�[�^�t�@�C�������t���p�X�Ń��x���e�L�X�g�{�b�N�X�ɐݒ肷��
            LblFullPath.Text = FileName

            ' �G���h���X���[�h����
            Call DspEndless()
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormDataSelect.AddFileNameToListList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�G�� -----
#End Region

End Class