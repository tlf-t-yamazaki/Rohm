'===============================================================================
'   Description  : ���_���A��ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================

Imports LaserFront.Trimmer.DllSystem    'V4.4.0.0-0
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Public Class FrmReset

#Region "�y�ϐ���`�z"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '-------------------------------------------------------------------------------
    '   �����v ON/OFF����p�����v�ԍ�(�R���\�[������)
    '-------------------------------------------------------------------------------
    Public Const LAMP_START As Short = 0                            ' START�����v
    Public Const LAMP_RESET As Short = 1                            ' RESET�����v
    Public Const LAMP_PRB As Short = 2                              ' Z�����v
    Public Const LAMP_HALT As Short = 5                             ' HALT�����v
    Public Const LATCH_CLR As Short = 11                            ' B11 : �J�o�[�J���b�`�N���A

    '-------------------------------------------------------------------------------
    '   �C���^�[���b�N���
    '-------------------------------------------------------------------------------
    '----- �C���^�[���b�N��� -----
    Public Const INTERLOCK_STS_DISABLE_NO As Integer = 0            ' �C���^�[���b�N��ԁi�����Ȃ��j
    Public Const INTERLOCK_STS_DISABLE_PART As Integer = 1          ' �C���^�[���b�N�ꕔ�����i�X�e�[�W����\�j
    Public Const INTERLOCK_STS_DISABLE_FULL As Integer = 2          ' �C���^�[���b�N�S����

    '----- �A�N�`���G�[�^���̓r�b�g -----
    Public Const BIT_SLIDE_COVER_OPEN As UShort = &H1               ' �X���C�h�J�o�[�J(=1)
    Public Const BIT_SLIDE_COVER_CLOSE As UShort = &H2              ' �X���C�h�J�o�[��(=1)
    Public Const BIT_SLIDE_COVER_MOVING As UShort = &H4             ' �X���C�h�J�o�[���쒆(=1)
    Public Const BIT_SOURCE_AIR_CHECK As UShort = &H8               ' �������G�A�[�F0/1=�ُ�/����
    Public Const BIT_MAIN_COVER_OPENCLOSE As UShort = &H10          ' �Œ�J�o�[�F0/1=�J/��
    Public Const BIT_COVER_OPEN_RATCH As UShort = &H20              ' �J�o�[�J���b�`
    Public Const BIT_INTERLOCK_NO1_RELEASE As UShort = &H100        ' �C���^�[���b�N����1�F0/1=����/�L��
    Public Const BIT_INTERLOCK_NO2_RELEASE As UShort = &H200        ' �C���^�[���b�N����2�F0/1=����/�L��
    Public Const BIT_EMERGENCY_STATUS_ONOFF As UShort = &H400       ' ����~��ԁF0/1=�ُ�/���� (������~��H/W��������̂ŕԂ��ė��Ȃ�)

    '-------------------------------------------------------------------------------
    '   ���̑�
    '-------------------------------------------------------------------------------
    '----- Form�̕�/���� -----
    Private Const WIDTH_NOMAL As Integer = 570                      ' Form�̕�
    Private Const WEIGHT_NOMAL As Integer = 203                     ' Form�̍���(�ʏ탂�[�h)
    'Private Const WEIGHT_LDALM As Integer = 460                    ' Form�̍���(���[�_�A���[�����[�h)
    Private Const WEIGHT_LDALM As Integer = 203 + 129 + 460         ' Form�̍���(���[�_�A���[�����[�h) ###161
    Private Const WEIGHT_LDALM2 As Integer = 203 + 129 + 2          ' Form�̍���(�����{�^���\�����[�h) ###161

    Private stSzNML As System.Drawing.Size = New System.Drawing.Size(WIDTH_NOMAL, WEIGHT_NOMAL)
    Private stSzLDE As System.Drawing.Size = New System.Drawing.Size(WIDTH_NOMAL, WEIGHT_LDALM)
    Private stSzLDE2 As System.Drawing.Size = New System.Drawing.Size(WIDTH_NOMAL, WEIGHT_LDALM2) ' ###161

    '----- Cancel�{�^���\���ʒu -----                               '###073
    Private LocCanBtnOrg As System.Drawing.Point = New System.Drawing.Point(215, 163)
    Private LocCanBtn2 As System.Drawing.Point = New System.Drawing.Point(303, 163)

    '----- �ϐ���` -----
    Private mExitFlag As Integer                                    ' �I���t���O
    Private gMode As Integer                                        ' �������[�h
    'Private ObjSys As Object                                        ' OcxSystem�I�u�W�F�N�g
    Private ObjSys As SystemNET                                     ' OcxSystem�I�u�W�F�N�g           'V4.4.0.0-0

    '----- ���[�_�A���[����� -----
    Private AlarmCount As Integer
    Private strLoaderAlarm(LALARM_COUNT) As String                  ' �A���[��������
    Private strLoaderAlarmInfo(LALARM_COUNT) As String              ' �A���[�����1
    Private strLoaderAlarmExec(LALARM_COUNT) As String              ' �A���[�����(�΍�)

    '----- �w�胁�b�Z�[�W�\���p -----  ###089
    Private Const MSGARY_NO As Integer = 3                          ' �\�����b�Z�[�W�̍ő吔
    Private DspWaitKey As Integer                                   ' WaitKey 
    Private DspBtnDsp As Boolean                                    ' �{�^���\������/���Ȃ�
    Private strMsgAry(MSGARY_NO) As String                          ' �\�����b�Z�[�W�P�|�R
    'V6.0.0.0�Q    Private ColColAry(MSGARY_NO) As Object                          ' ���b�Z�[�W�F�P�|�R
    Private ColColAry(MSGARY_NO) As Color                          ' ���b�Z�[�W�F�P�|�R              'V6.0.0.0�Q

#End Region

#Region "�y���\�b�h��`�z"
#Region "�I�����ʂ�Ԃ�"
    '''=========================================================================
    ''' <summary>�I�����ʂ�Ԃ�</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public ReadOnly Property sGetReturn() As Integer
        Get
            Return (mExitFlag)
        End Get
    End Property
#End Region

#Region "ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����"
    '''=========================================================================
    ''' <summary>ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����</summary>
    ''' <param name="Owner">    (INP)���g�p</param>
    ''' <param name="iGmode">   (INP)�������[�h</param>
    ''' <param name="ObjSystem">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As SystemNET) 'V4.4.0.0-0
        'Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As Object)

        Dim strMSG As String

        Try
            ' ��������
            Console.WriteLine("�p�����[�^ = " + gMode.ToString)
            mExitFlag = -1                                          ' �I���t���O = ������
            gMode = iGmode                                          ' �������[�h
            ObjSys = ObjSystem                                      ' OcxSystem�I�u�W�F�N�g
            LblCaption.Text = ""
            Label1.Text = ""
            Label2.Text = ""
            Call SetControlName()                                   ' ###186 �{�^��������ݒ肷��(���{��/�p��)

            ' ��ʕ\��
            Me.Size = stSzNML                                       ' Form�̕�/������ʏ탂�[�h�p�ɂ���
            Me.ShowDialog()                                         ' ��ʕ\��
            Me.BringToFront()                                       ' �őO�ʂɕ\�� 
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����(�w�胁�b�Z�[�W�\���p)"
    '''=========================================================================
    ''' <summary>ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����(�w�胁�b�Z�[�W�\���p) ###089</summary>
    ''' <param name="Owner">    (INP)���g�p</param>
    ''' <param name="iGmode">   (INP)�������[�h</param>
    ''' <param name="ObjSystem">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="MsgAry">   (INP)�\�����b�Z�[�W�P�|�R</param>
    ''' <param name="ColAry">   (INP)���b�Z�[�W�F�P�|�R</param>
    ''' <param name="Md">       (INP)cFRS_ERR_START                = START�L�[�����҂�
    '''                              cFRS_ERR_RST                  = RESET�L�[�����҂�
    '''                              cFRS_ERR_START + cFRS_ERR_RST = START/RESET�L�[�����҂�</param>
    ''' <param name="BtnDsp">   (INP)�{�^���\������/���Ȃ�</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As SystemNET, _
                                    ByVal MsgAry() As String, ByVal ColAry() As Color, ByVal Md As Integer, ByVal BtnDsp As Boolean) 'V6.0.0.0�Q
        'Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal iGmode As Integer, ByVal ObjSystem As Object, _
        '                                ByVal MsgAry() As String, ByVal ColAry() As Object, ByVal Md As Integer, ByVal BtnDsp As Boolean)

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ��������
            Console.WriteLine("�p�����[�^ = " + gMode.ToString)
            mExitFlag = -1                                              ' �I���t���O = ������
            gMode = iGmode                                              ' �������[�h
            ObjSys = ObjSystem                                          ' OcxSystem�I�u�W�F�N�g
            LblCaption.Text = ""
            Label1.Text = ""
            Label2.Text = ""
            Call SetControlName()                                       ' ###186 �{�^��������ݒ肷��(���{��/�p��)

            ' �p�����[�^���擾����
            For Idx = 0 To (MSGARY_NO - 1)
                strMsgAry(Idx) = ""
                ColColAry(Idx) = System.Drawing.SystemColors.ControlText
            Next Idx
            DspWaitKey = Md                                             ' WaitKey
            DspBtnDsp = BtnDsp                                          ' �{�^���\������/���Ȃ�

            ' �\�����b�Z�[�W�P�|�R
            For Idx = 0 To (MsgAry.Length - 1)
                If (Idx > (MSGARY_NO - 1)) Then Exit For
                strMsgAry(Idx) = MsgAry(Idx)
            Next Idx

            ' ���b�Z�[�W�F�P�|�R
            For Idx = 0 To (ColAry.Length - 1)
                If (Idx > (MSGARY_NO - 1)) Then Exit For
                ColColAry(Idx) = ColAry(Idx)
            Next Idx

            '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
            ' MG1 Down�{�^����\������
            BtnMg1Down.Visible = False
            If (strMsgAry(0) = MSG_LOADER_50) Then
                BtnMg1Down.Visible = True
            End If
            '----- V4.11.0.0�E�� -----

            ' ��ʕ\��
            Me.Size = stSzNML                                           ' Form�̕�/������ʏ탂�[�h�p�ɂ���
            Me.ShowDialog()                                             ' ��ʕ\��
            Me.BringToFront()                                           ' �őO�ʂɕ\�� 
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###186�� -----
#Region "�{�^��������ݒ肷��(���{��/�p��)"
    '''=========================================================================
    ''' <summary>�{�^��������ݒ肷��(���{��/�p��)</summary>
    '''=========================================================================
    Private Sub SetControlName()

        Dim strMSG As String

        Try
            ' �{�^������ݒ肷��(���{��/�p��)
            'BtnTableClampOff.Text = MSG_LOADER_24                       '"�ڕ���N�����v����" �{�^��
            'BtnTableVacumeOff.Text = MSG_LOADER_25                      '"�ڕ���z������" �{�^��
            'BtnHandVacumeOff.Text = MSG_LOADER_26                       '"�n���h�z������" �{�^��
            'BtnHandVacumeOff.Text = MSG_LOADER_27                       '"�����n���h�z������" �{�^�� 
            'BtnHand2VacumeOff.Text = MSG_LOADER_28                      '"���[�n���h�z������" �{�^�� 

            ' ���x������ݒ肷��(���{��/�p��)
            '"���u���Ɋ���c���Ă���ꍇ��", "��菜���Ă�������"
            LblMSG.Text = MSG_LOADER_29 + MSG_LOADER_18

            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    GrpLdAlarm.Text = "�A���[�����X�g"
            'Else
            '    GrpLdAlarm.Text = "Alarm List"
            'End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.SetControlName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###186�� -----
#Region "�t�H�[�����\�����ꂽ���̏���"
    '''=========================================================================
    ''' <summary>�t�H�[�����\�����ꂽ���̏���</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmReset_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' ��ʏ������C��
            r = FrmReset_Main(gMode)
            mExitFlag = r                                           ' mExitFlag�ɖ߂�l��ݒ肷�� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.FrmReset_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                  ' �t�H�[�������
    End Sub
#End Region

#Region "Cancel(or OK)�{�^������������"
    '''=========================================================================
    ''' <summary>Cancel(or OK)�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            'V4.9.0.0�@
            '            If (BtnCancel.Text = "Cancel") Then
#If START_KEY_SOFT Then
            If (BtnCancel.Text = "Cancel") Or (BtnCancel.Text = "CANCEL") Or (BtnCancel.Text = "RESET") Or (BtnCancel.Text = "���~") Then
#Else
            If (BtnCancel.Text = "Cancel") Or (BtnCancel.Text = "���~") Then
#End If
                mExitFlag = cFRS_ERR_RST
            Else
                mExitFlag = cFRS_ERR_START
            End If
            '----- V6.0.3.0_27�� -----
            If (BtnCancel.Text = MSG_SPRASH58) Then                     ' "�p��"  ?
                mExitFlag = cFRS_ERR_RST
            End If
            '----- V6.0.3.0_27�� -----

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call RESET_SWITCH_ON()
            End If
#End If
            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "OK�{�^������������"
    '''=========================================================================
    ''' <summary>OK�{�^������������ ###073</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click
        Dim strMSG As String

        Try
            mExitFlag = cFRS_ERR_START
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call START_SWITCH_ON()
            End If
#End If
            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###161�� -----
#Region "�ڕ���N�����v�����{�^��������"
    '''=========================================================================
    ''' <summary>�ڕ���N�����v�����{�^��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnTableClampOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTableClampOff.Click

        Dim r As Integer                                                    ' V1.23.0.0�I
        Dim strMSG As String

        Try
            ' �A���[�����Z�b�g���o 
            Call W_RESET()

            '----- V1.23.0.0�I�� -----
            ' �����^�]���f�M�����o
            r = Send_AutoStopToLoader()
            '----- V1.23.0.0�I�� -----

            ' ���[�_�蓮���[�h�ؑւ�
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ���[�_�o��(ON=�Ȃ�, OFF=����)

            ' �N�����v�n�e�e
            'Call W_CLMP_ONOFF()                                        ' �N�����v�n�e�e�M�����o V1.16.0.0�D
            Call W_CLMP_ONOFF(0)                                        ' �N�����v�n�e�e�M�����o V1.16.0.0�D

            '' �A���[�����Z�b�g���o
            'Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormReset.BtnTableClampOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ڕ���z�������{�^��������"
    '''=========================================================================
    ''' <summary>�ڕ���z�������{�^��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnTableVacumeOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTableVacumeOff.Click

        Dim r As Integer                                                    ' V1.23.0.0�I
        Dim strMSG As String

        Try
            ' �A���[�����Z�b�g���o 
            Call W_RESET()

            '----- V1.23.0.0�I�� -----
            ' �����^�]���f�M�����o
            r = Send_AutoStopToLoader()
            '----- V1.23.0.0�I�� -----

            ' ���[�_�蓮���[�h�ؑւ�
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ���[�_�o��(ON=�Ȃ�, OFF=����)

            ' �z���n�e�e
            'Call W_VACUME_ONOFF()                                      ' �z���n�e�e�M�����o V1.18.0.0�N
            Call W_VACUME_ONOFF(0)                                      ' �z���n�e�e�M�����o V1.18.0.0�N

            '' �A���[�����Z�b�g���o
            'Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormReset.BtnTableVacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�����n���h�z�������{�^��������"
    '''=========================================================================
    ''' <summary>�����n���h�z�������{�^��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHandVacumeOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHandVacumeOff.Click

        Dim strMSG As String

        Try
            ' �A���[�����Z�b�g���o 
            Call W_RESET()

            ' ���[�_�蓮���[�h�ؑւ�
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ���[�_�o��(ON=�Ȃ�, OFF=����)

            ' �n���h�P�z���n�e�e
            Call W_HAND1_VACUME()                                       ' �n���h�P�z���n�e�e

            '' �A���[�����Z�b�g���o
            'Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormReset.BtnHandVacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���[�n���h�z�������{�^��������"
    '''=========================================================================
    ''' <summary>���[�n���h�z�������{�^��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHand2VacumeOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHand2VacumeOff.Click

        Dim strMSG As String

        Try
            ' �A���[�����Z�b�g���o 
            Call W_RESET()

            ' ���[�_�蓮���[�h�ؑւ�
            Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ���[�_�o��(ON=�Ȃ�, OFF=����)

            ' �n���h�Q�z���n�e�e
            Call W_HAND2_VACUME()                                       ' �n���h�Q�z���n�e�e

            '' �A���[�����Z�b�g���o
            'Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormReset.BtnHand2VacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###161�� -----
    '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
#Region "MG1 MouseDown����"
    '''=========================================================================
    ''' <summary>MG1 MouseDown����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMg1Down_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnMg1Down.MouseDown

        Dim Mg1 As Integer = 1
        Dim strMSG As String

        Try
            ' �}�K�W�����ړ�����
            Call MGMoveJog(Mg1, MG_DOWN)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormReset.BtnMg1Down_MouseDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "MG1 MouseUp����"
    '''=========================================================================
    ''' <summary>MG1 MouseUp����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMg1Down_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnMg1Down.MouseUp

        Dim strMSG As String

        Try
            ' �}�K�W���㉺�����~
            Call MGStopJog()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormReset.BtnMg1Down_MouseUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0�E�� -----
#Region "��ʏ������C��"
    '''=========================================================================
    ''' <summary>��ʏ������C��</summary>
    ''' <param name="gMode">(INP)�������[�h</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function FrmReset_Main(ByVal gMode As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String
        Dim InitError As Integer = 0        'V5.0.0.1-21

        Try
            '-------------------------------------------------------------------
            '   �������[�h�ɑΉ����鏈�����s��
            '-------------------------------------------------------------------
            Select Case gMode
                '   '-----------------------------------------------------------
                '   '   ���_���A����
                '   '-----------------------------------------------------------
                Case cGMODE_ORG                                                 ' ���_���A����
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_StartResetButtonDispOn()
                    End If
#End If
                    Call ObjSys.SetOrgFlg(True)                                 ' ���_���A�������t���O��ON�ɐݒ肷��
                    r = Sub_OriginBack()                                        ' ���_���A����
                    Call ObjSys.SetOrgFlg(False)                                ' ���_���A�������t���O��OFF�ɐݒ肷��

                    'V5.0.0.1-21
                    '���������̃^�C���A�E�g�A���[�_�G���[�̓\�t�g�I���ɂ���
                    InitError = r
                    'V5.0.0.1-21

                    ' INtime���G���[
                    If (System.Math.Abs(r) >= ERR_INTIME_BASE) Then             ' INtime���G���[ ?
                        gMode = System.Math.Abs(r)                              ' gMode = ���b�Z�[�W�ԍ�
                        GoTo STP_INTRIM                                         ' ���b�Z�[�W�\����
                    End If
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_StartResetButtonDispOff()
                    End If
#End If
                    ' ���ɉ��L�̃G���[�ŕԂ�
                    Select Case (r)
                        Case cFRS_ERR_EMG                                       ' ����~
                            GoTo STP_EMERGENCY                                  ' ����~���b�Z�[�W�\��

                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ���[�_�A���[�����b�Z�[�W�\��

                        Case cFRS_ERR_DUST                                      ' �W�o�@�ُ팟�o
                            GoTo STP_ARMDUST                                    ' �W�o�@�A���[�����b�Z�[�W�\��

                        Case cFRS_ERR_AIR                                       ' �G�A�[���G���[���o
                            GoTo STP_AIRVALVE                                   ' �G�A�[���ቺ���o���b�Z�[�W�\��

                        Case cFRS_ERR_MVC                                       ' Ͻ������މ�H��ԃG���[���o
                            ' (���b�Z�[�W�\���ς�)

                        Case cFRS_TO_SCVR_ON                                    ' �^�C���A�E�g(�ײ�޶�ް�į�߰�s�҂�)
                            ' (���b�Z�[�W�\���ς�)

                        Case cFRS_TO_SCVR_OP                                    ' �^�C���A�E�g(�X���C�h�J�o�[�J�҂�)
                            ' (���b�Z�[�W�\���ς�)
                    End Select

                    '-----------------------------------------------------------
                    '   ���[�_���_���A
                    '-----------------------------------------------------------
                Case cGMODE_LDR_ORG
                    ' "���[�_���_���A��", "", ""
                    Call Sub_SetMessage(MSG_SPRASH24, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()
                    r = Sub_Loader_OrgBack(cGMODE_LDR_ORG)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' ����~���b�Z�[�W�\��
                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ���[�_�A���[�����b�Z�[�W�\��
                    End Select

                    '-----------------------------------------------------------
                    '   �A��HI-NG�G���[������
                    '-----------------------------------------------------------
                Case cGMODE_ERR_HING                                            ' �A��HI-NG�G���[��
                    r = Sub_TrimError(cGMODE_ERR_HING)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' ����~���b�Z�[�W�\��
                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ���[�_�A���[�����b�Z�[�W�\��
                        Case Else
                            r = cFRS_ERR_HING                                   ' Return�l = �A��NG-HIGH��G���[����
                    End Select

                    '-----------------------------------------------------------
                    '   �ăv���[�r���O���s��
                    '-----------------------------------------------------------
                Case cGMODE_ERR_REPROBE                                         ' �ăv���[�r���O���s��
                    r = Sub_TrimError(cGMODE_ERR_REPROBE)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' ����~���b�Z�[�W�\��
                        Case cFRS_ERR_LDR, cFRS_ERR_LDR1, cFRS_ERR_LDR2, cFRS_ERR_LDR3, cFRS_ERR_LDRTO
                            GoTo STP_LDRALARM                                   ' ���[�_�A���[�����b�Z�[�W�\��
                        Case Else
                            r = cFRS_ERR_REPROB                                 ' Return�l = �ăv���[�r���O���s
                    End Select

                    '----- V6.0.3.0�S�� -----
                    '-----------------------------------------------------------
                    '   �����J�b�g�I�t����Ɏ��s�����Ƃ��̃G���[���b�Z�[�W
                    '-----------------------------------------------------------
                Case cGMODE_ERR_CUTOFF_TURNING                                  '�����J�b�g�I�t�����Ɏ��s
                    r = Sub_CutOffTurnError(cGMODE_ERR_CUTOFF_TURNING)
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' ����~���b�Z�[�W�\��
                        Case Else
                    End Select
                    '----- V6.0.3.0�S�� -----

                    '-----------------------------------------------------------
                    '   ���[�_�A���[��������
                    '-----------------------------------------------------------
                Case cGMODE_LDR_ALARM                                           ' ���[�_�A���[��������
STP_LDRALARM:
                    r = Sub_LdrAlarm(cGMODE_LDR_ALARM)
                    'V5.0.0.1-21
                    If r = 0 Then
                        r = InitError
                    End If
                    'V5.0.0.1-21

                    '-----------------------------------------------------------
                    '   ����~���b�Z�[�W�\��
                    '-----------------------------------------------------------
                Case cGMODE_EMG
STP_EMERGENCY:
                    bEmergencyOccurs = True 'V1.25.0.5�A
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    giTrimErr = giTrimErr Or &H10                   ' ��ϰ �װ �׸�(����~)
                    'V5.0.0.9�M ��
                    '�@r = ObjSys.SetSignalTower(0, &HFFFF)            ' ������ܰ����(On=0, Off=�S�ޯ�)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M ��
                    Call EXTOUT1(0, &HFFFF)                         ' EXTBIT (On=0, Off=�S�r�b�g)
                    Call EXTOUT2(0, &HFFFF)                         ' EXTBIT2(On=0, Off=�S�r�b�g)
                    r = Sub_DispEmergencyMsg()                      ' ����~���b�Z�[�W�\��

                    '-----------------------------------------------------------
                    '   �W�o�@�ُ탁�b�Z�[�W�\��
                    '-----------------------------------------------------------
                Case cGMODE_ERR_DUST
STP_ARMDUST:
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    Call LAMP_CTRL(LAMP_START, False)               ' START���ߏ���
                    ' ���b�Z�[�W�\��
                    ' "�W�o�@�ُ킪�������܂���", "RESET�L�[�������ƃv���O�������I�����܂�", ""
                    Call Sub_SetMessage(MSG_SPRASH17, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' ----- V1.18.0.0�B�� -----
                    ' ����p�A���[���J�n����ݒ肷��(���[���a����)
                    ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
                    Call SetAlmStartTime()
                    ' �A���[���t�@�C���ɃA���[���f�[�^����������
                    Call WriteAlarmData(gFPATH_QR_ALARM, MSG_SPRASH17, stPRT_ROHM.AlarmST_time, cFRS_ERR_DUST)
                    ' ----- V1.18.0.0�B�� -----

                    ' ���b�Z�[�W�\������RESET�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_RST)
                    ' ----- V1.18.0.0�B�� -----
                    ' �A���[����~����ݒ肷��(���[���a����)
                    Call SetAlmEndTime()
                    ' ----- V1.18.0.0�B�� -----
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    r = cFRS_ERR_DUST                                           ' Return�l = �W�o�@�ُ팟�o

                    '-----------------------------------------------------------
                    '   �G�A�[���ቺ���o���b�Z�[�W�\��
                    '-----------------------------------------------------------
                Case cGMODE_ERR_AIR
STP_AIRVALVE:
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    Call LAMP_CTRL(LAMP_START, False)                   ' START���ߏ���
                    ' ���b�Z�[�W�\��
                    ' "�G�A�[���ቺ���o", "RESET�L�[�������ƃv���O�������I�����܂�", ""
                    Call Sub_SetMessage(MSG_SPRASH12, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' RESET�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_RST)
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    r = cFRS_ERR_AIR                                            ' Return�l = �G�A�[���G���[���o

                    '-----------------------------------------------------------
                    '   ���[�_�ʐM�^�C���A�E�g���b�Z�[�W�\��
                    '-----------------------------------------------------------
                Case cGMODE_LDR_TMOUT
STP_LDTOUT:
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    Call LAMP_CTRL(LAMP_START, False)                   ' START���ߏ���

                    '----- ###205�� -----
                    ' �V�O�i���^���[��ԓ_��+�u�U�[ON����
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                             ' �W��(�ԓ_��+�u�U�[ON)
                            'V5.0.0.9�M �� V6.0.3.0�G
                            ' Call ObjSys.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                            'V5.0.0.9�M �� V6.0.3.0�G 

                        Case SIGTOWR_SPCIAL                             ' ����(�ԓ_��+�u�U�[�P)
                            'r = ObjSys.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
                    End Select
                    '----- ###205�� -----

                    ' ���b�Z�[�W�\��
                    ' "���[�_�ʐM�^�C���A�E�g�G���[", "RESET�L�[�������Ə������I�����܂�", ""
                    Call Sub_SetMessage(MSG_SPRASH18, MSG_SPRASH33, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' RESET�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_RST)
                    '----- ###205�� -----
                    ' �V�O�i���^���[��ԓ_��+�u�U�[OFF����
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                             ' �W��(�ԓ_��+�u�U�[OFF)
                            'V5.0.0.9�M �� V6.0.3.0�G
                            ' Call ObjSys.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                            'V5.0.0.9�M �� V6.0.3.0�G

                        Case SIGTOWR_SPCIAL                             ' ����(�ԓ_��+�u�U�[�P OFF)
                            'r = ObjSys.SetSignalTower(0, EXTOUT_RED_BLK Or EXTOUT_BZ1_ON)
                    End Select
                    '----- ###205�� -----
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    r = cFRS_ERR_LDRTO                                          ' Return�l = ���[�_�ʐM�^�C���A�E�g

                    '-----------------------------------------------------------
                    '   �����^�]�J�n(START�������҂�)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_START
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_StartResetButtonDispOn()
                    End If
#End If
                    ' ���b�Z�[�W�\��
                    ' "START�L�[�������Ǝ����^�]���J�n���܂�", "", ""
                    Call Sub_SetMessage(MSG_SPRASH22, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' START�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return�l = cFRS_ERR_START(START�L�[����)/cFRS_ERR_RST(RESET�L�[����)

                    '-----------------------------------------------------------
                    '   �����^�]�I��(START�������҂�)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_END

                    Dim DSP_MSG As String

                    ' ���b�Z�[�W�\��
                    BtnCancel.Visible = True                                    ' Cancel(OK)�{�^���\��
                    BtnCancel.Text = "OK"
                    DSP_MSG = MSG_frmLimit_07

#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        DSP_MSG = MSG_frmLimit_08
                    End If
#End If

                    ' "�����^�]�I��", "START�L�[���������AOK�{�^���������ĉ������B", ""
                    Call Sub_SetMessage(MSG_LOADER_15, DSP_MSG, "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()

                    ' START�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_START)
                    BtnCancel.Visible = False                                   ' Cancel(OK)�{�^����\��
                    BtnCancel.Text = "Cancel"
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return�l = cFRS_ERR_START(START�L�[����)

                    '-----------------------------------------------------------
                    '   �c���菜�����b�Z�[�W(START�������҂�)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_WKREMOVE, cGMODE_LDR_RSTAUTO, cGMODE_LDR_WKREMOVE2   ' ###175 ###124
                    ' ���b�Z�[�W�\��
                    BtnCancel.Visible = True                                    ' Cancel(OK)�{�^���\��
                    BtnCancel.Text = "OK"

                    '----- ###161�� -----
                    Me.Size = stSzLDE2                                          ' Form�̕�/�����������{�^���\�����[�h�p�ɂ���
                    Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
                    '----- ###161�� -----

                    '----- V1.18.0.1�G�� -----
                    ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
                    r = EL_Lock_OnOff(EX_LOK_MD_OFF)
                    If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                        Return (cFRS_ERR_RST)
                    End If
                    If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                        GoTo STP_EMERGENCY                                      ' ����~���b�Z�[�W�\����
                    End If
                    If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                        Return (r)
                    End If
                    '----- V1.18.0.1�G�� -----

                    '----- ###124�� -----
                    If (gMode = cGMODE_LDR_WKREMOVE) Then
                        ' "�ڕ����Ɋ���c���Ă���ꍇ��", "��菜���Ă�������", "START�L�[����OK�{�^�������Ō��_���A���܂��B"
                        Call Sub_SetMessage(MSG_LOADER_17, MSG_LOADER_18, MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText)

                        '' "�ڕ����Ɋ���c���Ă���ꍇ��", "��菜���Ă�������", "START�L�[���������AOK�{�^���������ĉ������B"
                        'Call Sub_SetMessage(MSG_LOADER_17, MSG_LOADER_18, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText)

                        '----- ###175�� -----
                    ElseIf (gMode = cGMODE_LDR_WKREMOVE2) Then
                        ' "���u���Ɋ���c���Ă���ꍇ��", "��菜���Ă�������", "OK�{�^�������ŃA�v���P�[�V�������I�����܂�"
                        Call Sub_SetMessage(MSG_LOADER_29, MSG_LOADER_18, MSG_LOADER_31, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText)
                        '----- ###175�� -----

                    Else
                        '' "�����^�]�𒆎~���܂�", "", "START�L�[����OK�{�^�������Ō��_���A���܂��B"
                        'Call Sub_SetMessage(MSG_LOADER_23, "", MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                        ' ###161 "�����^�]�𒆎~���܂�", "➑̃J�o�[�����", "START�L�[����OK�{�^�������Ō��_���A���܂��B"
                        Call Sub_SetMessage(MSG_LOADER_23, MSG_SPRASH36, MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    End If
                    '----- ###124�� -----
                    Me.Refresh()

                    ' START�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_START)
                    BtnCancel.Visible = False                                   ' Cancel(OK)�{�^����\��
                    BtnCancel.Text = "Cancel"
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return�l = cFRS_ERR_START(START�L�[����)
                    '----- ###161�� -----
                    If (gMode = cGMODE_LDR_WKREMOVE2) Then                      ' cGMODE_LDR_WKREMOVE2��➑̃J�o�[���m�F�Ȃ� ###175
                        Return (r)
                    End If

                    ' ➑̃J�o�[���m�F����
                    Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
                    Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
                    Me.Size = stSzNML                                           ' Form�̕�/������W�����[�h�p�ɂ���
                    Me.Visible = False                                          ' ���b�Z�[�W�\�������� 
                    r = Sub_CoverCheck()
                    If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                        GoTo STP_EMERGENCY                                      ' ����~���b�Z�[�W�\����
                    End If
                    '----- V1.18.0.1�G�� -----
                    ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
                    r = EL_Lock_OnOff(EX_LOK_MD_ON)
                    If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                        Return (cFRS_ERR_RST)
                    End If
                    If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                        GoTo STP_EMERGENCY                                      ' ����~���b�Z�[�W�\����
                    End If
                    If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                        Return (r)
                    End If
                    '----- V1.18.0.1�G�� -----

                    r = cFRS_ERR_START
                    '----- ###161�� -----

                    '----- ###188�� -----
                    '-----------------------------------------------------------
                    '   �X�e�[�W�����_�ɖ߂�(�c���菜������)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_STAGE_ORG
                    ' "�X�e�[�W���_�ړ���", "", ""
                    Call Sub_SetMessage(MSG_SPRASH38, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                    Me.Refresh()
                    r = Sub_XY_OrgBack()
                    Select Case (r)
                        Case cFRS_ERR_EMG
                            GoTo STP_EMERGENCY                                  ' ����~���b�Z�[�W�\��
                    End Select
                    '----- ###188�� -----
                    '----- V1.18.0.0�H�� -----
                    '-----------------------------------------------------------
                    '   �}�K�W���������b�Z�[�W(START�������҂�)
                    '-----------------------------------------------------------
                Case cGMODE_LDR_MAGAGINE_EXCHG
                    r = Sub_MagazineExchange()                                  ' �}�K�W���������b�Z�[�W�\��
                    If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                        GoTo STP_EMERGENCY                                      ' ����~���b�Z�[�W�\����
                    End If
                    '----- V1.18.0.0�H�� -----
                    'V4.9.0.0�@��
                Case cGMODE_ERR_RATE_NG        '�Ǖi���������Ȃ����ꍇ�̃G���[�\�����
                    r = Sub_NgRateAlarm()

                Case cGMODE_ERR_TOTAL_CLEAR    '�W�v���N���A���邩�̊m�F���b�Z�[�W
                    r = Sub_QuestTotalClear()
                    'V4.9.0.0�@��

                    '-----------------------------------------------------------
                    '   �g���~���O���̽ײ�޶�ް�J/➑̶�ް�J���b�Z�[�W�\��(START�������҂�)
                    '-----------------------------------------------------------
                Case cGMODE_SCVR_OPN, cGMODE_CVR_OPN
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call Sub_ResetButtonDispOn()
                    End If
#End If
                    r = Sub_CvrOpen(gMode)                                      ' �ײ�޶�ް�J/➑̶�ް�Jү���ޕ\��
                    If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                        GoTo STP_EMERGENCY                                      ' ����~���b�Z�[�W�\����
                    End If                                                      ' �����ݸޒ���➑̶�ް�J�͌��_���A�͍s��Ȃ�

                    '-----------------------------------------------------------
                    '   �w�胁�b�Z�[�W�\��(START�L�[/RESET�L�[�����҂�) ###089
                    '-----------------------------------------------------------
                Case cGMODE_MSG_DSP
                    ' OK/Cancel�{�^���\���ݒ�
                    BtnCancel.Visible = False                                   ' Cancel�{�^����\��
                    BtnOK.Visible = False                                       ' OK�{�^����\��
                    If (DspBtnDsp = True) Then                                  ' �{�^���\������ ?
                        If (DspWaitKey = (cFRS_ERR_START + cFRS_ERR_RST)) Then  ' OK/Cancel�{�^����\������ ?
                            BtnCancel.Location = LocCanBtn2                     ' Cancel�{�^���\���ʒu���E�ɂ��炷 
                            BtnCancel.Visible = True                            ' Cancel�{�^���\��
                            BtnOK.Visible = True                                ' OK�{�^���\��
                        End If
                        If (DspWaitKey = cFRS_ERR_RST) Then                     ' Cancel�{�^����\������ ?
                            BtnCancel.Text = "Cancel"
                            BtnCancel.Visible = True                            ' Cancel�{�^���\��
                        End If
                        If (DspWaitKey = cFRS_ERR_START) Then                   ' OK�{�^����\������ ?
                            BtnCancel.Text = "OK"
                            BtnCancel.Visible = True                            ' Cancel(OK)�{�^���\��
                        End If
                        'V4.11.0.0�J
                        If (DspWaitKey = cFRS_ERR_BTN_START) Then               ' OK�{�^����\������ ?
                            BtnCancel.Text = "START"
                            BtnCancel.Visible = True                            ' Cancel(OK)�{�^���\��
                        End If
                        If (DspWaitKey = cFRS_ERR_BTN_START + cFRS_ERR_RST) Then                   ' OK�{�^����\������ ?
                            BtnCancel.Location = LocCanBtn2                     ' Cancel�{�^���\���ʒu���E�ɂ��炷 
                            BtnCancel.Visible = True                            ' Cancel�{�^���\��
                            BtnCancel.Text = "CANCEL"
                            BtnOK.Text = "START"                                ' OK�{�^���\��
                            BtnOK.Visible = True                                ' OK�{�^���\��
                        End If
                        'V4.11.0.0�J

                    End If

                    ' �w�胁�b�Z�[�W��\������
                    Call Sub_SetMessage(strMsgAry(0), strMsgAry(1), strMsgAry(2), ColColAry(0), ColColAry(1), ColColAry(2))
                    Me.Refresh()

                    ' START�L�[/RESET�L�[�����҂�
                    r = Sub_WaitStartRestKey(DspWaitKey)
                    BtnCancel.Visible = False                                   ' Cancel�{�^����\��
                    BtnOK.Visible = False                                       ' OK�{�^����\��
                    BtnCancel.Text = "Cancel"
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '                                                           ' Return�l = cFRS_ERR_START(START�L�[����)

                    ''V4.0.0.0-83
                    ' �X�e�[�WHome�ʒu�ړ�
                Case cGMODE_STAGE_HOMEMOVE
                    'V4.0.0.0-83��
                    LblCaption.Text = MSG_SPRASH47
                    Me.Refresh()
                    r = SubHomeMove()
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    'V4.0.0.0-83

                    '----- V6.0.3.0_27�� -----
                    '-----------------------------------------------------------
                    '   �����^�]�{�^�����������Ƃ��ɐV�K���p���̑I�����s�� 
                    '-----------------------------------------------------------
                Case cGMODE_QUEST_NEW_CONTINUE

                    BtnCancel.Visible = True                                    ' Cancel�{�^���\��
                    BtnOK.Visible = True                                        ' OK�{�^���\��
                    BtnCancel.Location = LocCanBtn2                             ' Cancel�{�^���\���ʒu���E�ɂ��炷 

                    ' ���b�Z�[�W�\��
                    ' "�V�K�FSTART�{�^���A�p���FRESET�{�^��", "", ""
                    Call Sub_SetMessage(MSG_SPRASH74, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)

                    Dim save_OKBtn As String = BtnOK.Text
                    Dim save_CancelBtn As String = BtnCancel.Text

                    BtnOK.Text = MSG_SPRASH75                                   ' "�V�K" 
                    BtnCancel.Text = MSG_SPRASH76                               ' "�p��" 
                    Me.Refresh()

                    ' START�L�[�����҂�
                    r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)

                    BtnOK.Text = save_OKBtn
                    BtnCancel.Text = save_CancelBtn

                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY
                    '----- V6.0.3.0_27�� -----

                    '-----------------------------------------------------------
                    '   INtime������G���[�߂肵���G���[���b�Z�[�W��\������
                    '-----------------------------------------------------------
                Case Else
STP_INTRIM:
                    ' INtime���G���[��
                    If (gMode >= ERR_INTIME_BASE) Then                          ' INtime���G���[ ?
                        '   ' �\�t�g���~�b�g�G���[�̏ꍇ
                        If (ObjSys.IsSoftLimitCode(gMode)) Then                 ' �\�t�g���~�b�g�G���[
                            'r = Sub_ErrSoftLimit(gMode, giTrimErr)              ' ���b�Z�[�W�\��&START�L�[�����҂� ' ###008 

                            ' �\�t�g���~�b�g�G���[�ȊO�̏ꍇ
                        Else
                            ' �V�O�i���^���[3�F���䂠��(����) ?
                            If (gSysPrm.stIOC.giSigTwr2Flag = 1) Then
                                ' �V�O�i���^���[�R�F����(�ԓ_��) (EXTOUT(OnBit, OffBit))
                                Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glSigTwr2_Out_Adr, gSysPrm.stIOC.glSigTwr2_Red_Blnk, _
                                                           gSysPrm.stIOC.glSigTwr2_Red_On Or gSysPrm.stIOC.glSigTwr2_Yellow_On Or gSysPrm.stIOC.glSigTwr2_Yellow_Blnk)
                                ' �u�U�[���䂠��(����) ?
                                If (gSysPrm.stIOC.giBuzerCtrlFlag = 1) Then
                                    ' �u�U�[��2(�s�`�s�b�s) (EXTOUT(OnBit, OffBit))
                                    Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glBuzerCtrl_Out_Adr, gSysPrm.stIOC.glBuzerCtrl_Out2, gSysPrm.stIOC.glBuzerCtrl_Out1)
                                End If
                            End If

                            ' ���b�Z�[�W�\�� & START�L�[�����҂�
                            'r = Sub_ErrAxis(System.Math.Abs(r), giTrimErr)     ' ###008 
                        End If

                    End If

                    'If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY              ' ###008 

                    ' ���b�Z�[�W�\�� & START�L�[�����҂�                        ' ###008 
                    r = Sub_ErrAxis(gMode, giTrimErr)
                    If (r = cFRS_ERR_EMG) Then                                  ' ����~���o ?
                        GoTo STP_EMERGENCY                                      ' ����~���b�Z�[�W�\����
                    End If

                    r = -1 * gMode                                              ' Return�l = = gMode(-xxx�Ŗ߂�)

            End Select

            ' �I������
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.FrmReset_Main() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "���_���A����"
    '''=========================================================================
    ''' <summary>���_���A����</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_OriginBack() As Integer

        Dim r As Integer
        Dim bR As Boolean
        Dim sts As Long
        Dim InterlockSts As Integer
        Dim strMSG As String
        Dim lData As Long

        Try
            ' �V�O�i���^���[����(On=���_���A��,Off=�S�ޯ�) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(���_���A��(�Γ_��))
                    'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͖��_��)
                    ' r = ObjSys.SetSignalTower(SIGOUT_GRN_BLK, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ZRN)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(���_���A��(���F�_��))
                    r = ObjSys.SetSignalTower(EXTOUT_YLW_BLK, &HFFFF)
            End Select

            ' ���[�_�ʐM�`�F�b�N
            r = W_Read(LOFS_W110, lData)                               ' ���[�_�A���[����Ԏ擾(W110.08-10)
            If (r <> 0) Then
                r = cFRS_ERR_LDRTO
                GoTo STP_END
            End If

            ' �z��,�N�����v���䓙
            Call ObjSys.AutoLoaderFlgReset()                            ' ���۰�ް�׸�ؾ��
            Call ZSLCOVEROPEN(0)                                        ' �ײ�޶�ް����������OFF
            Call ZSLCOVERCLOSE(0)                                       ' �ײ�޶�ް�۰�������OFF
            ' �N�����v�y�уo�L���[��OFF
            If (bFgAutoMode = False) Then                               ' �����^�]���̓N�����v�y�уo�L���[��OFF���Ȃ� ###107
                r = ObjSys.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
            End If

            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY '         ' ����~�Ȃ�EMERGENCY��
                GoTo STP_END                                            ' ���̑��̃G���[���^�[��
            End If

            ' ����~ ?
            If (ObjSys.EmergencySwCheck() = True) Then
STP_EMERGENCY:
                r = cFRS_ERR_EMG                                        ' Return�l = ����~���o
                GoTo STP_END
            End If

            ' �}�X�^�[�o���u��H��ԃ`�F�b�N(����)
            r = ObjSys.Master_Valve_Check(gSysPrm)                      ' �}�X�^�[�o���u�`�F�b�N
            If (r = 2) Then                                             ' �G���[���o��Cancel ?
                r = cFRS_ERR_MVC                                        ' Return�l = Ͻ������މ�H��ԃG���[���o
                GoTo STP_END
            End If

            ' �G�A�[���`�F�b�N
            bR = ObjSys.Air_Valve_Check(gSysPrm)
            If (bR = False) Then                                        ' �G�A�[���G���[ ?
                r = cFRS_ERR_AIR                                        ' Return�l = �G�A�[���G���[���o
                GoTo STP_END
            End If

            ' ➑̃J�o�[/�X���C�h�J�o�[�`�F�b�N
STP_COVEROPEN:
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            r = CoverCheck(0, True)                                     ' ➑̃J�o�[/�X���C�h�J�o�[�`�F�b�N(RESET�L�[�����w��, ���_���A������)
            If (r = cFRS_ERR_RST) Then GoTo STP_COVEROPEN '             ' RESET �L�[�����H
            If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY '             ' ����~�Ȃ�EMERGENCY��
            If (r <> cFRS_NORMAL) Then                                  ' ���̑��̃G���[ ? 
                GoTo STP_END                                            ' ���̑��̃G���[���^�[��
            End If

            '���_���A����J�n����
            ' "START�L�[�������Ă�������", "", ""
            Call Sub_SetMessage(MSG_SPRASH4, "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
            Me.Refresh()

#If cOFFLINEcDEBUG Then                                                 ' OffLine���ޯ��ON ?
            MsgBox("START SW CHECK", vbOKOnly, "DEBUG")
#Else
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            Call LAMP_CTRL(LAMP_START, False)                           ' START����OFF

            ' START�L�[�����҂� ���� �ײ�޶�ް/➑̶�ް�۰������
            ' START�L�[�����҂�(�C���^�[�b�N������)
            r = ORG_INTERLOCK_CHECK(InterlockSts, sts)                  ' �C���^�[���b�N��Ԏ擾
            If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Or _
                (gSysPrm.stSPF.giWithStartSw = 1) Or _
                (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                Call ORG_STARTRESET_SWWAIT(sts)                         ' START�L�[�����҂�
                If (sts <> cFRS_ERR_START) Then                         ' START�L�[�����ȊO ? 
                    Select Case sts
                        Case ERR_OPN_CVR                                ' ➑̃J�o�[�J���o
                            GoTo STP_COVEROPEN
                        Case ERR_EMGSWCH                                ' ����~
                            GoTo STP_EMERGENCY
                        Case ERR_OPN_SCVR                               ' �X���C�h�J�o�[�J���o
                            GoTo STP_COVEROPEN
                        Case Else
                            r = sts                                     ' INtime�������Return�l�ݒ�
                            GoTo STP_END
                    End Select
                End If
            End If

            ' ���b�`����
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            Call COVERLATCH_CLEAR()                                     ' ��ް�Jׯ��ر
#End If
            ' "���_���A��"
            'V4.0.0.0-83��
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                LblCaption.Text = MSG_SPRASH24
            Else
                LblCaption.Text = MSG_SPRASH0
            End If
            'V4.0.0.0-83��
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call Sub_StartResetButtonDispOff()                          ' �X�^�[�g�{�^��OFF
            End If
#End If

            'V5.0.0.1-24
            ' ➑̃J�o�[���m�F����
            r = Sub_CoverCheck()
            'V5.0.0.1-24

            Me.Refresh()
            Call LAMP_CTRL(LAMP_START, True)                            ' START����ON

            ' ���[�_���_���A���� 
            r = Sub_Loader_OrgBack(cGMODE_ORG)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                GoTo STP_END
            End If

            ' �V�O�i���^���[����(On=���_���A��,Off=�S�ޯ�) ###117
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(���_���A��(�Γ_��))
                    'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͖��_��)
                    ' r = ObjSys.SetSignalTower(SIGOUT_GRN_BLK, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ZRN)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(���_���A��(���F�_��))
                    r = ObjSys.SetSignalTower(EXTOUT_YLW_BLK, &HFFFF)
            End Select
            'V4.0.0.0-83��
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                LblCaption.Text = MSG_SPRASH46
                Me.Refresh()
            End If
            'V4.0.0.0-83��

            ''V6.0.1.022���ʏ�̑��x��]������B 
            SetXYStageSpeed(StageSpeed.NormalSpeed)
            ''V6.0.1.022��

            ' XYZ�Ǝ�������
            r = ObjSys.EX_SYSINIT(gSysPrm, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet)
            Call ObjSys.SetOrgFlg(False)                                ' ���_���A�������t���O��OFF�ɐݒ肷��
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? (�����b�Z�[�W�͕\����)
                If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY ' ����~�Ȃ�EMERGENCY��
                If (r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Then GoTo STP_COVEROPEN
                GoTo STP_END                                            ' ���̑��̃G���[���^�[�� 
            End If

            ' XYZ�ړ����x�ؑւ�
            r = ORG_INTERLOCK_CHECK(InterlockSts, sts)                  ' �C���^�[���b�N��Ԏ擾
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' �C���^�[���b�N���(�����Ȃ�)�łȂ� ? ###108
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed
            Else
                Call ZSELXYZSPD(0)                                      ' XYZ Normal speed
            End If

            ' ���[�^���[�A�b�e�l�[�^����  ��INtime�Ƃ̓p�����[�^�������قȂ�̂Œ���
            If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then                       ' RMCTRL2�Ή� ?
                Call LATTSET(gSysPrm.stRAT.giAttFix, gSysPrm.stRAT.giAttRot)
            End If
            If ObjSys.SlideCoverCheck() Then GoTo STP_EMERGENCY '       ' �ײ�޶�ް�۰�ނłȂ� ?

            ' �␳�l���ݒ�
            Call ObjSys.SetRangeCorrectionValue(gSysPrm)                ' �����W�␳�l�ݒ�
            Call PROCPOWER(gSysPrm.stSPF.giProcPower2)                  ' ��d���l�؂�ւ�(250mA, 500mA)
            Call ZBPLOGICALCOORD(gSysPrm.stDEV.giBpDirXy)               ' BP���W�n�i�ی��j�̐ݒ�
            Call ObjSys.SetBpLinearityData(gSysPrm)                     ' BP���j�A���e�B�[�␳�f�[�^�ݒ�

            ' �ײ�޶�ް���������
            Call ZSLCOVEROPEN(0)                                        ' �ײ�޶�ް����������OFF
            Call ZSLCOVERCLOSE(0)                                       ' �ײ�޶�ް�۰�������OFF
            ' �z��OFF
            Call ObjSys.AbsVaccume(gSysPrm, 0, giAppMode, giTrimErr)

            ' ����SW�����҂��̏ꍇ�ͽײ�޶�ް��������݂��Ȃ�(��߼��)
            If ((gSysPrm.stSPF.giWithStartSw = 0) And _
                 (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432)) Then    ' �X�^�[�g�X�C�b�`�҂��łȂ��ASL432�n�̏ꍇ�B
                r = ObjSys.Z_COPEN(gSysPrm, 0, giTrimErr, False)        ' �ײ�޶�ް���������
                If (r <> 0) Then                                        ' �G���[ ?
                    If (r = cFRS_ERR_EMG) Then GoTo STP_EMERGENCY '     ' ����~�Ȃ�EMERGENCY��
                    GoTo STP_END                                        ' Return�l = ��ѱ��(�ײ�޶�ް�J��/�ײ�޶�ް�į�߰�s��)
                End If
            End If

            ' ���ߐݒ�
            Call LAMP_CTRL(LAMP_START, False)                           ' START����OFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET����OFF
            Call LAMP_CTRL(LAMP_PRB, False)                             ' PRB����OFF
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            If ObjSys.InterLockCheck() Then GoTo STP_EMERGENCY '        ' ����~ or ➑̶�ް�J ?

            ' ���_���A�I������
            giTrimErr = 0                                               ' ��ϰ �װ �׸ޏ�����
            ' �V�O�i���^���[����(On=���_���A��,Off=�S�ޯ�) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(���_��)
                    'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͖��_��)
                    '  r = ObjSys.SetSignalTower(0, &HEFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͖��_��)

                Case SIGTOWR_SPCIAL                                     ' ����(���F�_��)
                    r = ObjSys.SetSignalTower(EXTOUT_YLW_ON, &HEFFF)
            End Select

            gbInitialized = True                                        ' ���_���A��
            r = cFRS_NORMAL                                             ' Return�l = ���� 
STP_END:
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_OriginBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[
        End Try
    End Function
#End Region

#Region "���[�_�A���[������������"
    '''=========================================================================
    ''' <summary>���[�_�A���[������������</summary>
    ''' <param name="Mode">(INP)�������[�h</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_LdrAlarm(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim rtCode As Integer
        Dim strMSG As String
        Dim rtCodelock As Integer 'V4.1.0.0�J 

        Try
            ' ���샍�O�o��
            Call LAMP_CTRL(LAMP_START, False)                           ' START�����vOFF
            Call ObjSys.OperationLogging(gSysPrm, "ALARM", "LOADER")    ' ���샍�O�o��
            Call Sub_SetMessage("", "", "", System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)

            ' ���[�_���M(ON=�Ȃ�, OFF=���[�_���_���A+�g���~���O�m�f+�p�^�[���F���m�f)
            'Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG + LOUT_PTN_NG)   '###070
            Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG)                  '###070

            ' ���[�_�A���[���`�F�b�N(False=�A���[�����X�g�͕\�����Ȃ�)
            rtCode = Loader_AlarmCheck(ObjSys, False, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

            ' ���[�_�A���[������ݒ肷��
            Me.Size = stSzLDE                                           ' Form�̕�/���������[�_�A���[�����[�h�p�ɂ���
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ��� ###163
            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
            'V4.1.0.0�J��
            ' rtCode = EL_Lock_OnOff(EX_LOK_MD_OFF)
            rtCodelock = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (rtCodelock = cFRS_TO_EXLOCK) Then                           ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                rtCode = cFRS_ERR_RST
                GoTo STP_END                                            ' �I��������
            End If
            'V4.1.0.0�J��
            If (rtCode < cFRS_NORMAL) Then                              ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_END                                            ' �A�v���P�[�V���������I��
            End If
            '----- V1.18.0.1�G�� -----
            Call SetAlarmList(rtCode, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

            ' ���b�Z�[�W�\�� '###073
            'If (rtCode = cFRS_ERR_LDR3) Then                            ' ���[�_�A���[�����o(�y�̏�) ?
            If (rtCode = cFRS_ERR_LDR3) Or (rtCode = cFRS_ERR_LDR2) Then ' ���[�_�A���[��/���o(�y�̏�, �T�C�N����~) ?
                ' OK/Cancel�{�^���\��
                BtnCancel.Location = LocCanBtn2                         ' Cancel�{�^���\���ʒu���E�ɂ��炷 
                BtnCancel.Visible = True                                ' Cancel�{�^���\��
                BtnOK.Visible = True                                    ' OK�{�^���\��
                ' "���[�_�G���[", "START�L�[�F�������s�CRESET�L�[�F�����I��", ""
                Call Sub_SetMessage(MSG_LOADER_05, MSG_SPRASH35, "", System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlText)

            Else                                                        ' ���[�_�A���[�����o(�y�̏�ȊO) 
                ' Cancel�{�^���\��
                BtnCancel.Visible = True                                ' Cancel�{�^���\��
                ' "���[�_�G���[", "RESET�L�[�������Ə������I�����܂�", ""
                Call Sub_SetMessage(MSG_LOADER_05, MSG_SPRASH33, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
            End If
            Me.Refresh()

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()
            ' �A���[���t�@�C���ɃA���[���f�[�^����������
            Call WriteAlarmData(gFPATH_QR_ALARM, strLoaderAlarm(0), stPRT_ROHM.AlarmST_time, rtCode)
            ' ----- V1.18.0.0�B�� -----

            ' ���b�Z�[�W�\������START/RESET�������҂� '###073
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call Sub_ResetButtonDispOn()
            End If
#End If
            r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)     ' START/RESET�������҂�
            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----
            If (r = cFRS_ERR_EMG) Then                                  ' ����~���o ?
                rtCode = cFRS_ERR_EMG                                   ' Return�l = ����~���o
                GoTo STP_END                                            ' ����~���b�Z�[�W�\����
            ElseIf (r = cFRS_ERR_RST) Then
                'If (rtCode = cFRS_ERR_LDR3) Then                       ' �y�̏��RESET�������Ȃ�
                If (rtCode = cFRS_ERR_LDR3) Or (rtCode = cFRS_ERR_LDR2) Then ' ###196 �y�̏�,�T�C�N����~ ��RESET�������Ȃ�
                    rtCode = cFRS_ERR_RST                               ' Return�l = RESET�������ŕԂ�
                End If
            End If

            '----- ###163�� -----
            ' ➑̃J�o�[���m�F����
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Me.Size = stSzNML                                           ' Form�̕�/������W�����[�h�p�ɂ���
            Me.Visible = False                                          ' ���b�Z�[�W�\�������� 
            r = Sub_CoverCheck()
            If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                rtCode = cFRS_ERR_EMG                                   ' Return�l = ����~���o
                GoTo STP_END                                            ' ����~���b�Z�[�W�\����
            End If
            '----- ###163�� -----
            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
            rtCode = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (rtCode = cFRS_TO_EXLOCK) Then                           ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                rtCode = cFRS_ERR_RST
                GoTo STP_END                                            ' �I��������
            End If
            If (rtCode < cFRS_NORMAL) Then                              ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_END                                            ' �A�v���P�[�V���������I��
            End If
            '----- V1.18.0.1�G�� -----
STP_END:
            BtnCancel.Location = LocCanBtnOrg                           ' Cancel�{�^���\���ʒu��߂� 
            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancel�{�^����\��
            BtnOK.Visible = False                                       ' OK�{�^����\��

            Me.Size = stSzNML                                           ' Form�̕�/������ʏ탂�[�h�p�ɂ���
            Return (rtCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_LdrAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "���[�_�A���[������ݒ肷��"
    '''=========================================================================
    ''' <summary>���[�_�A���[������ݒ肷��</summary>
    ''' <param name="AlarmKind">         (INP)�A���[�����(�S��~�ُ�, �T�C�N����~, �y�̏�, �A���[������)</param>
    ''' <param name="AlarmCount">        (INP)�����A���[����</param>
    ''' <param name="strLoaderAlarm">    (INP)�A���[��������</param>
    ''' <param name="strLoaderAlarmInfo">(INP)�A���[�����1(�����g�p)</param>
    ''' <param name="strLoaderAlarmExec">(INP)�A���[�����(�΍�)</param>
    '''=========================================================================
    Private Sub SetAlarmList(ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
                             ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String)

        Dim i As Integer
        Dim strMSG As String = ""

        Try
            ' ���[�_�A���[�����X�g �O���[�v�{�b�N�X�̃e�L�X�g��ݒ肷��
            Select Case AlarmKind
                Case cFRS_NORMAL                                                    ' �A���[���Ȃ�
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & "---" & ")"             ' ���[�_�A���[�����X�g(---)

                Case cFRS_ERR_LDR1 ' �S��~�ُ픭����
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH19 & ")"      ' ���[�_�A���[�����X�g(�S��~�ُ픭����)

                Case cFRS_ERR_LDR2 ' ���ْ�~�ُ픭����
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH20 & ")"      ' ���[�_�A���[�����X�g(�T�C�N����~�ُ픭����)

                Case cFRS_ERR_LDR3  ' �y�̏ᔭ����
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH21 & ")"      ' ���[�_�A���[�����X�g(�y�̏ᔭ����)

                Case cFRS_ERR_PLC   ' PLC�X�e�[�^�X�ُ�
                    GrpLdAlarm.Text = MSG_LOADER_16 & "(" & MSG_SPRASH30 & ")"      ' ���[�_�A���[�����X�g(PLC�X�e�[�^�X�ُ�)
            End Select

            '----- V6.1.1.0�L�� -----
            If (giAlmTimeDsp = 1) Then                                              ' ���[�_�A���[�����̎��ԕ\���̗L��(0=�\���Ȃ�, 1=�\������)�@
                Call Get_NowYYMMDDHHMMSS(strMSG)
                GrpLdAlarm.Text = GrpLdAlarm.Text + " " + strMSG
            End If
            '----- V6.1.1.0�L�� -----

            ' ���[�_�A���[�������X�g�{�b�N�X�ɐݒ肷��
            Call ListAlarm.Items.Clear()
            'txtInfo.Text = ""
            TxtExec.Text = ""
            For i = 0 To (AlarmCount - 1)
                ListAlarm.Items.Add(strLoaderAlarm(i))                              ' �A���[��������
            Next
            If (AlarmCount <= 0) Then Exit Sub

            ' �A���[�����(�΍�)���e�L�X�g�{�b�N�X�ɐݒ肷��(�ŏ��͐擪��\��)
            ListAlarm.SelectedIndex = 0
            If (AlarmCount > 0) Then
                'txtInfo.Text = strLoaderAlarmInfo(0)        
                TxtExec.Text = strLoaderAlarmExec(0)
            End If
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.SetAlarmList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���[�_�A���[�����X�g�{�b�N�X�N���b�N�C�x���g����"
    '''=========================================================================
    ''' <summary>���[�_�A���[�����X�g�{�b�N�X�N���b�N�C�x���g����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListAlarm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListAlarm.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ���X�g�{�b�N�X�őI�����ꂽ���[�_�A���[���ɑΉ�����A���[�����(�΍�)���e�L�X�g�{�b�N�X�ɐݒ肷��
            Idx = ListAlarm.SelectedIndex
            'txtInfo.Text = strLoaderAlarmInfo(Idx)
            TxtExec.Text = strLoaderAlarmExec(Idx)                          ' �A���[�����(�΍�)���e�L�X�g�{�b�N�X�ɐݒ肷��
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.ListAlarm_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���}�G���[����������"
    '''=========================================================================
    ''' <summary>�g���}�G���[����������</summary>
    ''' <param name="Mode">(INP)�������[�h</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_TrimError(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim ErrCode As Short                                            ' V1.18.0.0�B
        Dim strMSG As String
        Dim strMSG2 As String

        Try
            ' ���샍�O�o��
            If (Mode = cGMODE_ERR_HING) Then
                strMSG = "ALARM"
                strMSG2 = "HI-NG"
            Else
                strMSG = "ALARM"
                strMSG2 = "RE-PROBING-NG"
            End If
            Call ObjSys.OperationLogging(gSysPrm, strMSG, strMSG2)

            '----- ###216�� -----
            ''----- ###181�� -----
            'If (bFgAutoMode = True) Then                                   ' �����^�] ?
            '    ' ���[�_���M(ON=�Ȃ�, OFF=���[�_���_���A+�g���~���O�m�f+�p�^�[���F���m�f)
            '    'Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG + LOUT_PTN_NG)   '###070
            '    Call SetLoaderIO(&H0, LOUT_ORG_BACK + LOUT_TRM_NG)                  '###070
            'End If
            ''----- ###181�� -----
            '----- ###216�� -----

            ' �V�O�i���^���[����(On=�ُ�+�޻ް1, Off=�S�ޯ�) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(�ԓ_��+�u�U�[�P)
                    'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͐ԓ_�Ł{�u�U�[�n�m)
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(�ԓ_��+�u�U�[�P)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            ' ���b�Z�[�W�\��
            If (Mode = cGMODE_ERR_HING) Then
                ErrCode = cFRS_ERR_HING                                 ' V1.18.0.0�B
                strMSG = MSG_SPRASH13                                   ' "�A��NG-HI�G���["
                strMSG2 = MSG_frmLimit_07                               ' "START�L�[��OK�{�^���������Ă�������"
            Else
                ErrCode = cFRS_ERR_REPROB                               ' V1.18.0.0�B
                strMSG = MSG_SPRASH14                                   ' "�ăv���[�r���O���s"
                strMSG2 = MSG_frmLimit_07                               ' "START�L�[��OK�{�^���������Ă�������"
            End If
            Call Sub_SetMessage(strMSG, strMSG2, "", System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
            Me.Refresh()
            BtnCancel.Text = "OK"
            BtnCancel.Visible = True                                    ' OK�{�^���\��

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()
            ' �A���[���t�@�C���ɃA���[���f�[�^����������
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, ErrCode)
            ' ----- V1.18.0.0�B�� -----

            ' ���b�Z�[�W�\������START�L�[�����҂�(OK�{�^�����L��)
            r = Sub_WaitStartRestKey(cFRS_ERR_START)                    ' START�������҂�
            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----
            If (r = cFRS_ERR_EMG) Then                                  ' ����~���o ?
                GoTo STP_END                                            ' ����~���b�Z�[�W�\����
            End If

            '----- ###181�� -----
            ' �V�O�i���^���[����(On=�Ȃ�, Off=�ُ�+�޻ް1) 
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��
                    'V5.0.0.9�M �� V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����
                    'r = Form1.System1.SetSignalTower(0, EXTOUT_RED_BLK Or EXTOUT_BZ1_ON)
            End Select

            '----- ###216�� -----
            '' ���[�_�p�g���}�G���[����������(SL432R�n��NOP) 
            'If (bFgAutoMode = True) Then                                   ' �����^�] ?
            '    r = Sub_Loader_TrimError()
            '    If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
            '        GoTo STP_END
            '    End If
            'End If
            '----- ###216�� -----
            '----- ###181�� -----
STP_END:
            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancel�{�^����\��
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_TrimError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�g���~���O���̽ײ�޶�ް�J/➑̶�ް�J���b�Z�[�W�\������"
    '''=========================================================================
    ''' <summary>�g���~���O���̽ײ�޶�ް�J/➑̶�ް�J���b�Z�[�W�\��</summary>
    ''' <param name="Mode">(INP)�������[�h</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_CvrOpen(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim rtnCode As Integer
        Dim strMSG As String

        Try
            ' �V�O�i���^���[����(On=�ُ�+�޻ް1, Off=�S�ޯ�) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(�ԓ_��+�u�U�[�P)
                    'V5.0.0.9�M �� V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M �� V6.0.3.0�G
                Case SIGTOWR_SPCIAL                                     ' ����(�ԓ_��+�u�U�[�P)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            Call LAMP_CTRL(LAMP_START, False)                           ' START����OFF

            ' ���b�Z�[�W�\��
            If (gMode = cGMODE_CVR_OPN) Then                            ' ➑̶�ް�J ?
                ' "➑̃J�o�[���J���܂���", "RESET�L�[�������ƃv���O�������I�����܂�", ""
                strMSG = MSG_SPRASH27                                   ' V1.18.0.0�B
                Call Sub_SetMessage(MSG_SPRASH27, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                rtnCode = cFRS_ERR_CVR                                  ' Return�l = ➑̃J�o�[�J���o
            Else
                ' "�X���C�h�J�o�[���J���܂���", "RESET�L�[�������ƃv���O�������I�����܂�", ""
                strMSG = MSG_SPRASH28                                   ' V1.18.0.0�B
                Call Sub_SetMessage(MSG_SPRASH28, MSG_SPRASH16, "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                rtnCode = cFRS_ERR_SCVR                                 ' Return�l = �X���C�h�J�o�[�J���o
            End If
            Me.Refresh()

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()
            ' �A���[���t�@�C���ɃA���[���f�[�^����������
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, rtnCode)
            ' ----- V1.18.0.0�B�� -----

            ' ���b�Z�[�W�\������RESET�������҂�
            r = Sub_WaitStartRestKey(cFRS_ERR_RST)                      ' RESET�������҂�
            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----
            If (r = cFRS_ERR_EMG) Then                                  ' ����~���o ?
                rtnCode = cFRS_ERR_EMG
                GoTo STP_END                                            ' ����~���b�Z�[�W�\����
            End If

STP_END:
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_CvrOpen() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region
    '----- V1.18.0.0�H�� -----
#Region "�}�K�W���������b�Z�[�W�\������(���[���a����)"
    '''=========================================================================
    ''' <summary>�}�K�W���������b�Z�[�W�\��(���[���a����)</summary>
    ''' <returns>cFRS_ERR_START  = START�L�[����OK�{�^������
    '''          cFRS_ERR_RST    = RESET�L�[����Cancel�{�^������
    '''          ��L�ȊO        = �G���[</returns>
    '''=========================================================================
    Private Function Sub_MagazineExchange() As Integer

        Dim Md As Integer = 0
        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' OK/Cancel�{�^���\���ݒ�
            BtnCancel.Location = LocCanBtn2                             ' Cancel�{�^���\���ʒu���E�ɂ��炷 
            BtnCancel.Visible = True                                    ' Cancel�{�^���\��
            BtnOK.Visible = True                                        ' OK�{�^���\��
            '----- V4.0.0.0�R�� -----
            ' SL436R�̃��[���a�����ȊO��OK�{�^���͕\������
            'BtnOK.Enabled = False                                      ' OK�{�^���񊈐���
            BtnOK.Enabled = True                                        ' OK�{�^���񊈐���
            If ((gSysPrm.stCTM.giSPECIAL = customROHM) And (giMachineKd <> MACHINE_KD_RS)) Then
                BtnOK.Enabled = False                                    ' OK�{�^���񊈐���
            End If
            '----- V4.0.0.0�R�� -----

            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����(SL436R/SL436S))
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                mExitFlag = cFRS_ERR_RST
                GoTo STP_EXIT                                           ' �I��������
            End If
            If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                mExitFlag = r
                GoTo STP_EXIT                                           ' �A�v���P�[�V���������I��
            End If
            '----- V1.18.0.1�G�� -----
            '----- V4.0.0.0-25�� -----
            ' �}�K�W���I�����̃V�O�i���^���[����
            If (giMachineKd = MACHINE_KD_RS) Then
                ' �V�O�i���^���[����(On=�Γ_��+�u�U�[�P, Off=�S�ޯ�) SL436S��
                'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͗Γ_�Ł{�u�U�[�n�m)
                ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)
                'V5.0.0.9�M �� V6.0.3.0�G

            Else
                ' �V�O�i���^���[����(On=�ԓ_��+�u�U�[�P, Off=�S�ޯ�) SL436R��
                'Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)     'V6.1.1.0�C
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)               'V6.1.1.0�C
            End If

            '' �V�O�i���^���[����(On=�ԓ_��+�u�U�[�P, Off=�S�ޯ�)
            'Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            '----- V4.0.0.0-25�� -----

            ' ���b�Z�[�W�\��
            ' "�}�K�W���I��", "OK�{�^�������Ŏ����^�]�𑱍s���܂�", "Cancel�{�^�������Ŏ����^�]���I�����܂�"
            Call Sub_SetMessage(MSG_LOADER_44, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Me.Refresh()
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            mExitFlag = 0
            Md = 0

            '-------------------------------------------------------------------
            '   �}�K�W�������m�F
            '-------------------------------------------------------------------
            Do
                ' �}�K�W�������m�F�̂��߁A�}�K�W��2�����`�F�b�N��A�}�K�W��2�L���\�`�F�b�N���s��
                If (BtnOK.Enabled = False) Then                         ' OK�{�^�����������Ȃ�}�K�W����/�L�`�F�b�N�͍s��Ȃ�
                    r = Sub_MagazineExitCheck(Md)                       ' �}�K�W����/�L�`�F�b�N
                    If (r = cFRS_NORMAL) Then
                        If (Md = 0) Then                                ' �}�K�W�������`�F�b�NOK�Ȃ� 
                            Md = 1                                      ' �}�K�W��2�L�`�F�b�N���s��
                        Else                                            ' �}�K�W��2�L���\�`�F�b�NOK�Ȃ�  
                            BtnOK.Enabled = True                        ' OK�{�^��������������
                        End If
                    End If
                End If

                ' START/RESET�L�[�����҂�
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW�����`�F�b�N
                If (r = cFRS_ERR_EMG) Then
                    mExitFlag = cFRS_ERR_EMG
                    GoTo STP_EXIT
                End If

                ' RESET�L�[���� ?
                If (sts = cFRS_ERR_RST) Then                            ' RESET�L�[����Cancel�{�^������ ? 
                    mExitFlag = cFRS_ERR_RST                            ' Retuen�l = RESET�L�[����Cancel�{�^������
                    Exit Do                                             ' ➑̃J�o�[�m�F��
                End If

                ' START�L�[���� ?
                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' START�L�[����OK�{�^������ ? V1.18.0.7�A
                    mExitFlag = cFRS_ERR_START                          ' Retuen�l = START�L�[����OK�{�^������
                    Exit Do                                             ' ➑̃J�o�[�m�F��
                End If

                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                       ' ����~ ?
                    mExitFlag = cFRS_ERR_EMG                            ' Retuen�l = ����~
                    GoTo STP_EXIT                                       ' ����~���b�Z�[�W�\����
                End If
                Me.BringToFront()                                       ' �őO�ʂɕ\�� 

            Loop While (mExitFlag = 0)

            '----- V6.0.3.0_21�� -----           ��
            ' �����^�]�I�����I�����ꂽ�ꍇ�A����������Ə���������̍��ɂ��I�����Ċm�F����
            If ((mExitFlag = cFRS_ERR_RST) AndAlso (FormLotEnd.DoConfirm)) Then
                Using frm As New FormLotEnd(gSysPrm.stTMN.giMsgTyp)
                    Me.Enabled = False
                    frm.Show(Me)
                    mExitFlag = frm.Execute(ObjSys)
                    frm.Close()
                    Me.Enabled = True
                    If (cFRS_ERR_EMG = mExitFlag) Then
                        GoTo STP_EXIT                                   ' ����~���b�Z�[�W�\����
                    End If
                End Using
            End If
            '----- V6.0.3.0_21�� -----          ��

            ' �V�O�i���^���[����(On=�Ȃ�, Off=�ԓ_��+�u�U�[�P)
            'V5.0.0.9�M �� V6.0.3.0�G
            'Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)                      ' V4.0.0.0-25
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON Or SIGOUT_GRN_BLK)     ' V4.0.0.0-25
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)
            'V5.0.0.9�M �� V6.0.3.0�G

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
            ' ➑̃J�o�[���m�F����
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Me.Size = stSzNML                                           ' Form�̕�/������W�����[�h�p�ɂ���
            Me.Visible = False                                          ' ���b�Z�[�W�\�������� 
            r = Sub_CoverCheck()                                        ' ➑̃J�o�[���m�F����
            If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                mExitFlag = cFRS_ERR_EMG                                ' Retuen�l = ����~
                GoTo STP_EXIT                                           ' ����~���b�Z�[�W�\����
            End If

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����(���[���a����(SL436R/SL436S))
            r = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                mExitFlag = cFRS_ERR_RST
                GoTo STP_EXIT                                           ' �I��������
            End If
            If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                mExitFlag = r
                GoTo STP_EXIT                                           ' �A�v���P�[�V���������I��
            End If
            '----- V1.18.0.1�G�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_MagazineExchange() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExitFlag = cERR_TRAP                                       ' Retuen�l = �ׯ�ߴװ����
        End Try

STP_EXIT:
        BtnCancel.Visible = False                                       ' Cancel�{�^����\��
        BtnOK.Visible = False                                           ' OK�{�^����\��
        BtnCancel.Text = "Cancel"
        Return (mExitFlag)
    End Function
#End Region
    '----- V1.18.0.0�H�� -----
    '----- ###188�� -----
#Region "�X�e�[�W�����_�ɖ߂�(�c���菜������)"
    '''=========================================================================
    ''' <summary>�X�e�[�W�����_�ɖ߂�(�c���菜������)</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_XY_OrgBack() As Integer

        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' �n���h���㏸������(�n���h���������Ă���ꍇ)
            W_HAND1_UP()                                                ' �����n���h
            W_HAND2_UP()                                                ' ���[�n���h

            ' �X�e�[�W�����_�ɖ߂�(XYZ�Ǝ�������)
            r = Form1.System1.EX_SYSINIT(gSysPrm, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? (�����b�Z�[�W�͕\����)
                Return (r)
            End If

            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_XY_OrgBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region
    '----- ###188�� -----
#Region "�t�H�[���ɕ\�����郁�b�Z�[�W��ݒ肷��"
    '''=========================================================================
    ''' <summary>�t�H�[���ɕ\�����郁�b�Z�[�W��ݒ肷��</summary>
    ''' <param name="strMSG1">(INP)LblCaption�ɕ\�����镶����</param>
    ''' <param name="strMSG2">(INP)Label1�ɕ\�����镶����</param>
    ''' <param name="strMSG3">(INP)Label2�ɕ\�����镶����</param>
    ''' <param name="Color1"> (INP)LblCaption�̕����̐F</param>
    ''' <param name="Color2"> (INP)Label1�̕����̐F</param>
    ''' <param name="Color3"> (INP)Label2�̕����̐F</param>
    '''=========================================================================
    Private Sub Sub_SetMessage(ByVal strMSG1 As String, ByVal strMSG2 As String, ByVal strMSG3 As String, _
                                 ByVal Color1 As Color, ByVal Color2 As Color, ByVal Color3 As Color)           'V6.0.0.0�Q
        'Private Sub Sub_SetMessage(ByVal strMSG1 As String, ByVal strMSG2 As String, ByVal strMSG3 As String, _
        '                             ByVal Color1 As Object, ByVal Color2 As Object, ByVal Color3 As Object)

        Dim strMSG As String

        Try
            ' ���b�Z�[�W�ݒ�
            LblCaption.ForeColor = Color1
            Label1.ForeColor = Color2
            Label2.ForeColor = Color3
            LblCaption.Text = strMSG1
            Label1.Text = strMSG2
            Label2.Text = strMSG3
            'V5.0.0.4�@�����b�Z�[�W�����s�����ĕ\�����鎞�������B��Ȃ��l�ɃV�t�g������B
            If Label1.Text.Contains(ControlChars.CrLf) Then
                Label1.Location = New System.Drawing.Point(Label1.Location.X, Label1.Location.Y - 15)
                Label1.Size = New System.Drawing.Size(Label1.Size.Width, Label1.Size.Height + 35)
                '                Label2.Location = New System.Drawing.Point(Label2.Location.X, Label2.Location.Y)
            End If
            If Label2.Text.Contains(ControlChars.CrLf) Then
                Label1.Location = New System.Drawing.Point(Label1.Location.X, Label1.Location.Y - 5)
                Label2.Location = New System.Drawing.Point(Label2.Location.X, Label2.Location.Y - 20)
                Label2.Size = New System.Drawing.Size(Label2.Size.Width, Label2.Size.Height + 35)
            End If
            'V5.0.0.4�@��
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_SetMessage() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Start/Reset�L�[�����҂����ٰ��"
    '''=========================================================================
    ''' <summary>Start/Reset�L�[�����҂����ٰ��</summary>
    ''' <param name="Md">(INP)cFRS_ERR_START                = START�L�[�����҂�
    '''                       cFRS_ERR_RST                  = RESET�L�[�����҂�
    '''                       cFRS_ERR_START + cFRS_ERR_RST = START/RESET�L�[�����҂�
    ''' </param>
    ''' <returns>cFRS_ERR_START = START�L�[����
    '''          cFRS_ERR_RST   = RESET�L�[����
    '''          ��L�ȊO=�G���[
    ''' </returns>
    '''=========================================================================
    Private Function Sub_WaitStartRestKey(ByVal Md As Integer) As Integer

        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            ' �p�����[�^�`�F�b�N
            If (Md = 0) Then
                Return (-1 * ERR_CMD_PRM)                           ' �p�����[�^�G���[
            End If

#If cOFFLINEcDEBUG Then                                             ' OffLine���ޯ��ON ?(��FormReset���őO�ʕ\���Ȃ̂ŉ��L�̂悤�ɂ��Ȃ���MsgBox���őO�ʕ\������Ȃ�)
            Dim Dr As System.Windows.Forms.DialogResult
            Dr = MessageBox.Show("START SW CHECK", "Debug", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If (Dr = System.Windows.Forms.DialogResult.OK) Then
                mExitFlag = cFRS_ERR_START                          ' Return�l = START�L�[����
            Else
                mExitFlag = cFRS_ERR_RST                            ' Return�l = RESET�L�[����
            End If
            Return (mExitFlag)
#End If

            ' START/RESET�L�[�����҂�(Ok/Cancel�{�^�����L��)
            Call ZCONRST()                                          ' �R���\�[���L�[���b�`����
            mExitFlag = 0
            Do
                r = STARTRESET_SWCHECK(False, sts)                  ' START/RESET SW�����`�F�b�N
                If (sts = cFRS_ERR_RST) And ((Md = cFRS_ERR_RST) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_RST                        ' ExitFlag = Cancel(RESET�L�[)
                ElseIf (sts = cFRS_ERR_START) And ((Md = cFRS_ERR_START) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_START                      ' ExitFlag = OK(START�L�[)
                End If

                System.Windows.Forms.Application.DoEvents()         ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(1)               ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                   ' ����~ ?
                    mExitFlag = cFRS_ERR_EMG                        ' Return�l = ����~���o
                End If
                Me.BringToFront()                                   ' �őO�ʂɕ\�� 
            Loop While (mExitFlag = 0)

            Return (mExitFlag)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_WaitRestKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "����~���b�Z�[�W�\��"
    '''=========================================================================
    ''' <summary>����~���b�Z�[�W�\��</summary>
    ''' <returns></returns>
    '''=========================================================================
    Private Function Sub_DispEmergencyMsg() As Integer

        Dim strMSG As String

        Try
            ' �V�O�i���^���[3�F���䂠��(����) ?
            If (gSysPrm.stIOC.giSigTwr2Flag = 1) Then
                ' �V�O�i���^���[�R�F����(�ԓ_��) (EXTOUT(OnBit, OffBit))
                Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glSigTwr2_Out_Adr, gSysPrm.stIOC.glSigTwr2_Red_Blnk, _
                                           gSysPrm.stIOC.glSigTwr2_Red_On Or gSysPrm.stIOC.glSigTwr2_Yellow_On Or gSysPrm.stIOC.glSigTwr2_Yellow_Blnk)
                ' �u�U�[���䂠��(����) ?
                If (gSysPrm.stIOC.giBuzerCtrlFlag = 1) Then
                    ' �u�U�[��2(�s�`�s�b�s) (EXTOUT(OnBit, OffBit))
                    Call ObjSys.EXTIO_Out_Sub(gSysPrm.stIOC.glBuzerCtrl_Out_Adr, gSysPrm.stIOC.glBuzerCtrl_Out2, gSysPrm.stIOC.glBuzerCtrl_Out1)
                End If
            End If

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()
            ' �A���[���t�@�C���ɃA���[���f�[�^����������(����~)
            Call WriteAlarmData(gFPATH_QR_ALARM, MSG_SPRASH6, stPRT_ROHM.AlarmST_time, cFRS_ERR_EMG)
            ' ----- V1.18.0.0�B�� -----

            ' ���b�Z�[�W�\��
            ' "����~���܂���", "Cancel�{�^�������Ńv���O�������I�����܂�", ""
            ''V4.0.0.0-71            Call Sub_SetMessage(MSG_SPRASH6, "", MSG_SPRASH17, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
            Call Sub_SetMessage(MSG_SPRASH6, MSG_SPRASH45, MSG_SPRASH17, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
            Me.Refresh()
            mExitFlag = cFRS_NORMAL                                     ' ExitFlg = ������
            BtnCancel.Visible = True                                    ' Cancel�{�^���\��

            ' Cancel�{�^���̉�����҂�
            Do
                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While (mExitFlag = cFRS_NORMAL)
            BtnCancel.Visible = False                                   ' Cancel�{�^����\��

            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----

            Return (cFRS_ERR_EMG)                                       ' Retuen�l = ����~

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_DispEmergencyMsg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "���~�b�g�Z���T�[& ���G���[ & �^�C���A�E�g��"
    '''=========================================================================
    ''' <summary>���~�b�g�Z���T�[/���G���[/�^�C���A�E�g��
    '''          INtime������̃G���[���b�Z�[�W��\������</summary>
    ''' <param name="gMode">    (INP)�G���[�ԍ�</param>
    ''' <param name="giTrimErr">(INP)�g���}�G���[�t���O(0=��ϰ�װ�łȂ�, 0�ȊO=��ϰ�װ)</param>
    ''' <returns>gMode</returns>
    '''=========================================================================
    Private Function Sub_ErrAxis(ByVal gMode As Integer, ByVal giTrimErr As Integer) As Integer

        Dim bMsgDspMode As Boolean
        Dim r As Integer
        Dim Md As Integer
        Dim strMSG As String

        Try
            ' ��������
            r = gMode                                                   ' Return�l�ݒ�
            Call LAMP_CTRL(LAMP_START, False)                           ' STATR����OFF

            ' �V�O�i���^���[����(On=�ُ�+�޻ް1, Off=�S�ޯ�) ###007
            If (ObjSys.IsSoftLimitCode(gMode)) Then                     ' �\�t�g���~�b�g�G���[ ?
                ' ������ܰ����Ȃ�
            Else
                ' ������ܰ����(On=�ُ�+�޻ް1, Off=�S�ޯ�)
                'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͐ԓ_�Ł{�u�U�[�n�m)
                ' r = ObjSys.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF) ' �W��(�ԓ_��+�u�U�[�P) ###007
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                'V5.0.0.9�M �� V6.0.3.0�G
            End If

            ObjSys.GetMsgDispMode(bMsgDspMode)
            If (bMsgDspMode = False) Then Exit Function '               ' ���b�Z�[�W�\���Ȃ��Ȃ�NOP

            '---------------------------------------------------------------------------
            '   ���b�Z�[�W�ݒ� ###008
            '---------------------------------------------------------------------------
            strMSG = ""
            Select Case (gMode)
                Case ERR_SOFTLIMIT_X                                    ' X���\�t�g���~�b�g
                    strMSG = MSG_frmLimit_03                            ' "X�����~�b�g"
                Case ERR_SOFTLIMIT_Y                                    ' Y���\�t�g���~�b�g
                    strMSG = MSG_frmLimit_04                            ' "Y�����~�b�g"
                Case ERR_SOFTLIMIT_Z                                    ' Z���\�t�g���~�b�g
                    strMSG = MSG_frmLimit_05                            ' "Z�����~�b�g
                Case ERR_SOFTLIMIT_Z2                                   ' Z2���\�t�g���~�b�g
                    strMSG = MSG_frmLimit_06                            ' "Z2�����~�b�g"
                Case ERR_BP_XLIMIT                                      ' BP X���\�t�g���~�b�g�G���[
                    strMSG = MSG_frmLimit_01                            ' "BP���~�b�g"
                Case ERR_BP_YLIMIT                                      ' BP Y���\�t�g���~�b�g�G���[
                    strMSG = MSG_frmLimit_01                            ' "BP���~�b�g"

                Case ERR_BP_LIMITOVER                                   ' BP�ړ������ݒ�F���~�b�g�I�[�o�[
                    strMSG = MSG_BP_LIMITOVER                           ' "BP�ړ������ݒ胊�~�b�g�I�[�o�["
                Case ERR_BP_HARD_LIMITOVER_LO                           ' BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ŏ����͈͈ȉ��j
                    strMSG = MSG_BP_HARD_LIMITOVER_LO                   ' "BP���~�b�g�I�[�o�[�i�ŏ����͈͈ȉ��j"
                Case ERR_BP_HARD_LIMITOVER_HI                           ' BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ő���͈͈ȏ�j
                    strMSG = MSG_BP_HARD_LIMITOVER_HI                   ' "BP���~�b�g�I�[�o�[�i�ő���͈͈ȏ�j"
                Case ERR_BP_SOFT_LIMITOVER                              ' �\�t�g���͈̓I�[�o�[
                    strMSG = MSG_BP_SOFT_LIMITOVER                      ' "BP�\�t�g���͈̓I�[�o�["
                Case ERR_BP_BSIZE_OVER                                  ' �u���b�N�T�C�Y�ݒ�I�[�o�[�i�\�t�g���͈̓I�[�o�[�j
                    strMSG = MSG_BP_BSIZE_OVER                          ' "�u���b�N�T�C�Y�ݒ�I�[�o�[�i�\�t�g���͈͊O�j"
                Case ERR_BP_MOVE_TIMEOUT
                    strMSG = MSG_BP_MOVE_TIMEOUT                        ' "BP �^�C���A�E�g"
                Case ERR_BP_GRV_ALARM_X
                    strMSG = MSG_BP_GRV_ALARM_X                         ' "�K���o�m�A���[��X"
                Case ERR_BP_GRV_ALARM_Y
                    strMSG = MSG_BP_GRV_ALARM_Y                         ' "�K���o�m�A���[��Y"

                Case ERR_SRV_ALM
                    strMSG = MSG_SRV_ALM                                ' "�T�[�{�A���[��"
                Case ERR_AXS_LIM_X                                      ' X�����~�b�g���o
                    strMSG = MSG_AXS_LIM_X
                Case ERR_AXS_LIM_Y                                      ' Y�����~�b�g���o
                    strMSG = MSG_AXS_LIM_Y
                Case ERR_AXS_LIM_Z                                      ' Z�����~�b�g���o
                    strMSG = MSG_AXS_LIM_Z
                Case ERR_AXS_LIM_T                                      ' �Ǝ����~�b�g���o
                    strMSG = MSG_AXS_LIM_T
                Case ERR_AXS_LIM_ATT                                    ' ATT�����~�b�g���o
                    strMSG = MSG_AXS_LIM_ATT
                Case ERR_AXS_LIM_Z2                                     ' Z2�����~�b�g���o
                    strMSG = MSG_AXS_LIM_Z2
                Case ERR_OPN_CVR                                        ' ➑̃J�o�[�J���o
                    strMSG = MSG_OPN_CVR
                Case ERR_OPN_SCVR                                       ' �X���C�h�J�o�[�J���o
                    strMSG = MSG_OPN_SCVR
                Case ERR_OPN_CVRLTC                                     ' �J�o�[�J���b�`���o
                    strMSG = MSG_OPN_CVRLTC

                Case ERR_AXIS_X_SERVO_ALM
                    strMSG = MSG_AXIS_X_SERVO_ALM                       ' "X���T�[�{�A���[��"
                Case ERR_AXIS_Y_SERVO_ALM
                    strMSG = MSG_AXIS_Y_SERVO_ALM                       ' "Y���T�[�{�A���[��"
                Case ERR_AXIS_Z_SERVO_ALM
                    strMSG = MSG_AXIS_Z_SERVO_ALM                       ' "Z���T�[�{�A���[��"
                Case ERR_AXIS_T_SERVO_ALM
                    strMSG = MSG_AXIS_T_SERVO_ALM                       ' "�Ǝ��T�[�{�A���[��"
                Case ERR_TIMEOUT_AXIS_X
                    strMSG = MSG_TIMEOUT_AXIS_X                         ' "X���^�C���A�E�g�G���["
                Case ERR_TIMEOUT_AXIS_Y
                    strMSG = MSG_TIMEOUT_AXIS_Y                         ' "Y���^�C���A�E�g�G���["
                Case ERR_TIMEOUT_AXIS_Z
                    strMSG = MSG_TIMEOUT_AXIS_Z                         ' "Z���^�C���A�E�g�G���["
                Case ERR_TIMEOUT_AXIS_T
                    strMSG = MSG_TIMEOUT_AXIS_T                         ' "�Ǝ��^�C���A�E�g�G���["
                Case ERR_TIMEOUT_AXIS_Z2
                    strMSG = MSG_TIMEOUT_AXIS_Z2                        ' "Z2���^�C���A�E�g�G���["
                Case ERR_TIMEOUT_ATT
                    strMSG = MSG_TIMEOUT_ATT                            ' "���[�^���A�b�e�l�[�^�^�C���A�E�g�G���["
                Case ERR_TIMEOUT_AXIS_XY
                    strMSG = MSG_TIMEOUT_AXIS_XY                        ' "XY���^�C���A�E�g�G���["

                Case ERR_STG_SOFTLMT_PLUS                               ' �v���X���~�b�g�I�[�o�[
                    strMSG = MSG_STG_SOFTLMT_PLUS                       ' "���v���X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_MINUS                              ' �}�C�i�X���~�b�g�I�[�o�[
                    strMSG = MSG_STG_SOFTLMT_MINUS                      ' "���}�C�i�X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_PLUS_AXIS_X
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_X                ' "X���v���X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_PLUS_AXIS_Y
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_Y                ' "Y���v���X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_PLUS_AXIS_Z
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_Z                ' "Z���v���X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_PLUS_AXIS_T
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_T                ' "�Ǝ��v���X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_PLUS_AXIS_Z2
                    strMSG = MSG_STG_SOFTLMT_PLUS_AXIS_Z2               ' "Z2���v���X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_MINUS_AXIS_X
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_X               ' "X���}�C�i�X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_MINUS_AXIS_Y
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_Y               ' "Y���}�C�i�X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_MINUS_AXIS_Z
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_Z               ' "ZZ���}�C�i�X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_MINUS_AXIS_T
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_T               ' "�Ǝ��}�C�i�X���~�b�g�I�[�o�["
                Case ERR_STG_SOFTLMT_MINUS_AXIS_Z2
                    strMSG = MSG_STG_SOFTLMT_MINUS_AXIS_Z2              ' "Z2���}�C�i�X���~�b�g�I�[�o�["

                Case ERR_OPN_CVR                                        ' ➑̃J�o�[�J���o
                    strMSG = MSG_SPRASH27                               ' "➑̃J�o�[���J���܂���"
                Case ERR_OPN_SCVR                                       ' �X���C�h�J�o�[�J���o
                    strMSG = MSG_SPRASH28                               ' "�X���C�h�J�o�[���J���܂���"
                Case ERR_OPN_CVRLTC                                     ' �J�o�[�J���b�`���o
                    strMSG = MSG_SPRASH34                               ' "➑̃J�o�[�܂��̓X���C�h�J�o�[���J���܂���"
                Case ERR_LSR_STATUS_STANBY
                    strMSG = MSG_INTIME_ERROR + " 833:LASER IS NOT READY"
                Case Else
                    ' INtime���G���[(Code=xxxx)
                    strMSG = MSG_INTIME_ERROR + "(Code=" + gMode.ToString("0") + ")"

            End Select

            '---------------------------------------------------------------------------
            '   ���b�Z�[�W�\��
            '---------------------------------------------------------------------------
            If (ObjSys.IsSoftLimitCode(gMode)) Then                     ' �\�t�g���~�b�g�G���[ ?
                Call LAMP_CTRL(LAMP_START, 1)                           ' START����ON
                Call LAMP_CTRL(LAMP_RESET, 0)                           ' RESET����OFF

                ' "���b�Z�[�W","","START�L�[��OK�{�^���������Ă�������"
                Call Sub_SetMessage(strMSG, "", MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
                Me.Refresh()
                BtnCancel.Text = "OK"
                BtnCancel.Visible = True                                ' OK�{�^���\��
                Md = cFRS_ERR_START                                     ' Md = START�L�[�����҂�
            Else
                Call LAMP_CTRL(LAMP_START, 0)                           ' START����OFF
                Call LAMP_CTRL(LAMP_RESET, 1)                           ' RESET����ON

                ' "���b�Z�[�W","","RESET�L�[�������ƃv���O�������I�����܂�"
#If START_KEY_SOFT Then
                If gbStartKeySoft Then
                    Call Sub_ResetButtonDispOn()
                End If
#End If
                Call Sub_SetMessage(strMSG, "", MSG_SPRASH16, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
                Me.Refresh()
                BtnCancel.Visible = True                                ' Cancel�{�^���\��
                Md = cFRS_ERR_RST                                       ' Md = RESET�L�[�����҂�
            End If

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()
            ' �A���[���t�@�C���ɃA���[���f�[�^����������
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, gMode)
            ' ----- V1.18.0.0�B�� -----

            ' ���b�Z�[�W�\������START(RESET)�L�[�����҂�(OK(Cancel)�{�^�����L��) ###008
            r = Sub_WaitStartRestKey(Md)
            If (r <> Md) Then
                gMode = r                                               ' Return�l�ݒ�
            End If

            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancel�{�^����\��

            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----

            If (ObjSys.IsSoftLimitCode(gMode)) Then                     ' �\�t�g���~�b�g�G���[ ?
                Call LAMP_CTRL(LAMP_RESET, True)                        ' RESET���� ON
            End If

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_ErrAxis() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "➑̃J�o�[��/�X���C�h�J�o�[�`�F�b�N����"
    '''=========================================================================
    ''' <summary>➑̃J�o�[��/�X���C�h�J�o�[�`�F�b�N����</summary>
    ''' <param name="iRESETiARBORT">(INP)1=Reset SW�L��</param>
    ''' <param name="bOrgFlg">      (INP)True=���_���A������, False=���_���A�������ȊO</param>
    ''' <returns>0 = ����, -1 = ����~, 2 = RESET �L�[����, ���̑�</returns>
    '''========================================================================
    Public Function CoverCheck(ByVal iRESETiARBORT As Integer, ByVal bOrgFlg As Boolean) As Long

        Dim sw As Long = 0
        Dim sw1 As Long = 0
        Dim fclamp As Boolean = False
        Dim Flg As Boolean = False
        Dim r As Long = 0
        Dim InterlockSts As Integer = 0
        Dim sldcvrSts As Long = 0
        Dim strMSG As String
        Dim coverSts As Long = 0                                           ' ###101

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
#If cOFFLINEcDEBUG Then                                                 ' OffLine���ޯ��ON ?
            Return (cFRS_NORMAL)                                        ' Return�l = ����
#End If
            ' �C���^�[���b�N�����Ȃ�NOP
            If (bOrgFlg = True) Then                                    ' ���_���A������ ?
                r = ORG_INTERLOCK_CHECK(InterlockSts, sw)               ' �C���^�[���b�N��Ԏ擾
            Else
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' �C���^�[���b�N��Ԏ擾
            End If
            'If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Then        ' �C���^�[���b�N�S���� ?    ###101
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' �C���^�[���b�N���łȂ� ?  ###101
                Return (cFRS_NORMAL)                                    ' Return�l = ����
            End If

            '---------------------------------------------------------------------------
            '   ➑̃J�o�[�`�F�b�N
            '---------------------------------------------------------------------------
STP_COVEROPEN:
            Flg = False                                                 ' Flg = ���b�Z�[�W���\��
            '            Do                                             ' ###101
            System.Windows.Forms.Application.DoEvents()

            ' �C���^�[���b�N��Ԏ擾
            If (bOrgFlg = True) Then                                    ' ���_���A������ ?
                r = ORG_INTERLOCK_CHECK(InterlockSts, sw)
            Else
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' �C���^�[���b�N��Ԏ擾
            End If
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' �C���^�[���b�N���łȂ� ?  ###101
                Return (cFRS_NORMAL)                                    ' Return�l = ����           ###101
            End If

            '----- ###101(�ύX ��������) -----
            ' �J�o�[�J�`�F�b�N
            r = COVER_CHECK(coverSts)
            If (r <> cFRS_NORMAL) Then
                Return (r)
            End If

            ' ➑̃J�o�[�łȂ���΃G���[�߂肷��
            If (coverSts <> 1) Then
                Return (ERR_OPN_CVR)                                    ' Return�l = ➑̃J�o�[�J���o
            End If

            '    ' �߂�l�̔���
            '    If (r = ERR_OPN_CVRLTC) Then
            '        If (bOrgFlg <> True) Then
            '            Return (r)                                  ' Return�l = �J�o�[�J���b�`
            '        End If
            '        If (Flg = True) Then
            '            Flg = False
            '            '�{�`�F�b�N���[�v�ɂāA��x�J�o�[���J������̓��b�`�N���A
            '            Call COVERLATCH_CLEAR()
            '        End If
            '    ElseIf (r = ERR_OPN_CVR) Then
            '        If (Flg = False) Then                           ' ���b�Z�[�W�\���ς� ?
            '            Flg = True                                  ' Flg = ���b�Z�[�W�\���ς�
            '            LblCaption.ForeColor = System.Drawing.SystemColors.ControlText
            '            Label2.ForeColor = System.Drawing.SystemColors.ControlText
            '            LblCaption.Text = MSG_SPRASH10              ' "➑̃J�o�[����Ă�������"
            '            Label2.Text = ""
            '            Me.Refresh()
            '        End If
            '    ElseIf (r = cFRS_NORMAL) Then
            '        If (sw And BIT_MAIN_COVER_OPENCLOSE) Then           ' �J�o�[�N���[�Y
            '            Exit Do
            '        End If
            '    Else                                                ' �ȏ㔭����
            '        Return (r)                                      ' Return�l = INTRIM����̃G���[�R�[�h
            '    End If
            '    'If (r = cFRS_ERR_CVR) Then                          ' ➑̃J�o�[�J���o ?
            '    '    If (Flg = False) Then                           ' ���b�Z�[�W�\���ς� ?
            '    '        Flg = True                                  ' Flg = ���b�Z�[�W�\���ς�
            '    '        LblCaption.ForeColor = System.Drawing.SystemColors.ControlText
            '    '        Label2.ForeColor = System.Drawing.SystemColors.ControlText
            '    '        LblCaption.Text = MSG_SPRASH10              ' "➑̃J�o�[����Ă�������"
            '    '        Label2.Text = ""
            '    '        Me.Refresh()
            '    '    End If
            '    'End If
            'Loop While (1)
            '----- ###101(�ύX �����܂�) -----

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
            Return (cFRS_NORMAL)                                    ' Return�l = ����
            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.CoverCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        CoverCheck = cERR_TRAP                                      ' Retuen�l = �ׯ�ߴװ����
    End Function
#End Region

#Region "➑̃J�o�[���m�F����"
    '''=========================================================================
    ''' <summary>➑̃J�o�[���m�F���� ###161</summary>
    ''' <returns>cFRS_NORMAL = ➑̃J�o�[��
    '''          ��L�ȊO  = �G���[</returns>
    '''=========================================================================
    Public Function Sub_CoverCheck() As Integer ' ###185

        Dim sw As Long = 0
        Dim coverSts As Long = 0
        Dim InterlockSts As Integer = 0
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' �C���^�[���b�N�����Ȃ�NOP
            r = INTERLOCK_CHECK(InterlockSts, sw)                       ' �C���^�[���b�N��Ԏ擾
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' �C���^�[���b�N���łȂ� ?
                Return (cFRS_NORMAL)                                    ' Return�l = ����
            End If

            Do
                System.Threading.Thread.Sleep(100)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()
                '----- ###190�� -----
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' �C���^�[���b�N��Ԏ擾
                If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then      ' �C���^�[���b�N���łȂ� ?
                    Return (cFRS_NORMAL)                                ' Return�l = ����
                End If
                '----- ###190�� -----

                ' ➑̃J�o�[���m�F����
                r = COVER_CHECK(coverSts)
                If (r <> cFRS_NORMAL) Then
                    Return (r)                                          ' ����~���̃G���[
                End If
                ' ➑̃J�o�[�Ȃ琳��߂肷��
                If (coverSts = 1) Then Exit Do

                ' "➑̃J�o�[�����","","START�L�[���������AOK�{�^���������ĉ������B"
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH36, "", MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                ' ����~���̃G���[�Ȃ�A�v�������I����(�G���[���b�Z�[�W�͕\���ς�) 
                If (r < cFRS_NORMAL) Then Return (r)
                If (r = cFRS_ERR_START) Then
                    ''V5.0.0.1-27
                    Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
                End If
                Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A

            Loop While (1)

            Return (cFRS_NORMAL)                                        ' Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmReset.Sub_CoverCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[ 
        End Try
    End Function
#End Region

    ''' <summary>
    '''  'Home�ʒu�ړ�
    ''' </summary>
    ''' <remarks></remarks>
    Public Function SubHomeMove() As Integer
        Dim r As Integer

        'V5.0.0.6�A        r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
        r = Form1.System1.EX_SMOVE2(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
        If (r < cFRS_NORMAL) Then                           ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
            SubHomeMove = r                                 ' Return�l�ݒ�
            Exit Function
        End If

    End Function

#End Region

    ''' <summary>
    ''' �R�ڂ̃{�^�����������Ƃ��̏���    'V4.9.0.0�@
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnOther_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOther.Click
        Dim strMSG As String

        Try
            mExitFlag = cFRS_ERR_OTHER

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.BtnOther_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

#Region "�m�f���ɂ��\������"
    '''=========================================================================
    ''' <summary>�m�f���ɂ��\�������\��</summary>
    ''' <returns>cFRS_ERR_START  = START�L�[����OK�{�^������
    '''          cFRS_ERR_RST    = RESET�L�[����Cancel�{�^������
    '''          cFRS_ERR_OTHER  = OTHER�{�^������
    '''          ��L�ȊO      = �G���[</returns>
    '''=========================================================================
    Private Function Sub_NgRateAlarm() As Integer

        'V5.0.0.9�P        Dim Md As Integer = 0
        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' OK/Cancel�{�^���\���ݒ�
            BtnCancel.Location = LocCanBtn2                             ' Cancel�{�^���\���ʒu���E�ɂ��炷 
            BtnCancel.Visible = True                                    ' Cancel�{�^���\��
            BtnOK.Visible = True                                        ' OK�{�^���\��
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    BtnOK.Text = "���s"                                         ' �{�^���\���u���s�v
            '    BtnCancel.Text = "���~"                                     ' �{�^���\���u���~�v
            '    BtnOther.Text = "�N���[�j���O"                              ' �{�^���\���u�N���[�j���O�v
            'Else
            '    BtnOK.Text = "CONTINUE"                                         ' �{�^���\���u���s�v
            '    BtnCancel.Text = "Cancel"                                     ' �{�^���\���u���~�v
            '    BtnOther.Text = "CLEANING"                              ' �{�^���\���u�N���[�j���O�v
            'End If

            BtnOK.Text = MSG_SPRASH59                                         ' �{�^���\���u���s�v
            BtnCancel.Text = MSG_SPRASH60                                     ' �{�^���\���u���~�v
            BtnOther.Text = MSG_SPRASH61                              ' �{�^���\���u�N���[�j���O�v

            BtnOther.Visible = True                                     ' Other�{�^���\��
            BtnOther.Visible = False
            BtnOK.Left = 50
            BtnOther.Left = BtnOK.Left + BtnOK.Width + 40                            ' �{�^���\���u�N���[�j���O�v
            BtnCancel.Left = BtnOther.Left + BtnOther.Width + 40

            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���

            'V5.0.0.9�M ��
            '' �}�K�W���I�����̃V�O�i���^���[����
            'If (giMachineKd = MACHINE_KD_RS) Then
            '    ' �V�O�i���^���[����(On=�Γ_��+�u�U�[�P, Off=�S�ޯ�) SL436S��
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)

            'Else
            '    ' �V�O�i���^���[����(On=�ԓ_��+�u�U�[�P, Off=�S�ޯ�) SL436R��
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            'End If
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            'V5.0.0.9�M ��

            Me.Top = 800
            Me.Left = Me.Left
            Me.Refresh()
            ' ���b�Z�[�W�\��
            strMSG = MSG_SPRASH52
            If NGJudgeResult = 1 Then
                strMSG = strMSG + " (Yield)"
            ElseIf NGJudgeResult = 2 Then
                strMSG = strMSG + " (OpenNG)"
            ElseIf NGJudgeResult = 4 Then
                strMSG = strMSG + " (Sub-IT-HI)"
            ElseIf NGJudgeResult = 5 Then
                strMSG = strMSG + " (Lot-IT-HI)"
            ElseIf NGJudgeResult = 7 Then
                strMSG = strMSG + " (Sub-IT-LO)"
            ElseIf NGJudgeResult = 8 Then
                strMSG = strMSG + " (Lot-IT-LO)"
            ElseIf NGJudgeResult = 10 Then
                strMSG = strMSG + " (Sub-FT-HI)"
            ElseIf NGJudgeResult = 11 Then
                strMSG = strMSG + " (LOT-FT-HI)"
            ElseIf NGJudgeResult = 13 Then
                strMSG = strMSG + " (Sub-FT-LO)"
            ElseIf NGJudgeResult = 14 Then
                strMSG = strMSG + " (LOT-FT-LO)"
            End If
            ' "�m�f���ɂ���~", "OK�{�^�������Ŏ����^�]�𑱍s���܂�", "Cancel�{�^�������Ŏ����^�]���I�����܂�"
            Call Sub_SetMessage(strMSG, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Me.Refresh()
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            mExitFlag = 0
            'V5.0.0.9�P            Md = 0

            '-------------------------------------------------------------------
            '   �{�^���҂�����
            '-------------------------------------------------------------------
            Dim md As Short = Globals_Renamed.giAppMode                 'V5.0.0.9�P
            Globals_Renamed.giAppMode = APP_MODE_IDLE                   'V5.0.0.9�P
            Do

                ' START/RESET�L�[�����҂�
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW�����`�F�b�N
                If (r = cFRS_ERR_EMG) Then
                    mExitFlag = cFRS_ERR_EMG
                    GoTo STP_EXIT
                End If

                ' RESET�L�[���� ?
                If (sts = cFRS_ERR_RST) Then                            ' RESET�L�[����Cancel�{�^������ ? 
                    mExitFlag = cFRS_ERR_RST                            ' Retuen�l = RESET�L�[����Cancel�{�^������
                    Exit Do                                             ' ➑̃J�o�[�m�F��
                End If

                ' START�L�[���� ?
                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' START�L�[����OK�{�^������ ? V1.18.0.7�A
                    mExitFlag = cFRS_ERR_START                          ' Retuen�l = START�L�[����OK�{�^������
                    Exit Do                                             ' ➑̃J�o�[�m�F��
                End If

                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                       ' ����~ ?
                    mExitFlag = cFRS_ERR_EMG                            ' Retuen�l = ����~
                    GoTo STP_EXIT                                       ' ����~���b�Z�[�W�\����
                End If
                Me.BringToFront()                                       ' �őO�ʂɕ\�� 

            Loop While (mExitFlag = 0)
            Globals_Renamed.giAppMode = md                              'V5.0.0.9�P

            ' �V�O�i���^���[����(On=�Ȃ�, Off=�ԓ_��+�u�U�[�P)
            'V5.0.0.9�M ��
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)     ' V4.0.0.0-25
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
            'V5.0.0.9�M ��

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
            ' ➑̃J�o�[���m�F����
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Me.Size = stSzNML                                           ' Form�̕�/������W�����[�h�p�ɂ���
            Me.Visible = False                                          ' ���b�Z�[�W�\�������� 
            r = Sub_CoverCheck()                                        ' ➑̃J�o�[���m�F����
            If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                mExitFlag = cFRS_ERR_EMG                                ' Retuen�l = ����~
                GoTo STP_EXIT                                           ' ����~���b�Z�[�W�\����
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_NgRateAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExitFlag = cERR_TRAP                                       ' Retuen�l = �ׯ�ߴװ����
        End Try

STP_EXIT:
        BtnCancel.Visible = False                                       ' Cancel�{�^����\��
        BtnOK.Visible = False                                           ' OK�{�^����\��
        BtnCancel.Text = "Cancel"
        Return (mExitFlag)
    End Function
#End Region

#Region "�W�v���e���N���A���邩�̊m�F���b�Z�[�W"
    '''=========================================================================
    ''' <summary>�W�v���e���N���A���邩�̊m�F���b�Z�[�W</summary>
    ''' <returns>cFRS_ERR_START  = START�L�[����OK�{�^������
    '''          cFRS_ERR_RST    = RESET�L�[����Cancel�{�^������
    '''          ��L�ȊO      = �G���[</returns>
    '''=========================================================================
    Private Function Sub_QuestTotalClear() As Integer

        Dim Md As Integer = 0
        Dim sts As Long = 0
        Dim r As Long = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' OK/Cancel�{�^���\���ݒ�
            BtnCancel.Location = LocCanBtn2                             ' Cancel�{�^���\���ʒu���E�ɂ��炷 
            BtnCancel.Visible = False                                   ' Cancel�{�^���\��
            BtnOK.Visible = True                                        ' OK�{�^���\��
            BtnOK.Text = MSG_SPRASH54                                   ' �{�^���\���u�N���A�v

            BtnOther.Size = New Point(BtnOK.Size.Width, BtnOK.Size.Height)

            BtnOther.Visible = True                                     ' Other�{�^���\��
            BtnOther.Text = MSG_SPRASH55                                ' �{�^���\���u�N���A���Ȃ��v
            BtnOK.Left = 100
            BtnOther.Left = BtnOK.Left + BtnOK.Width + 100


            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���

            ' �}�K�W���I�����̃V�O�i���^���[����
            'V5.0.0.9�M ��
            'If (giMachineKd = MACHINE_KD_RS) Then
            '    ' �V�O�i���^���[����(On=�Γ_��+�u�U�[�P, Off=�S�ޯ�) SL436S��
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            'Else
            '    ' �V�O�i���^���[����(On=�ԓ_��+�u�U�[�P, Off=�S�ޯ�) SL436R��
            '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            'End If
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            'V5.0.0.9�M ��


            ' ���b�Z�[�W�\��
            ' "�W�v���N���A���܂����H", " ", " "
            Call Sub_SetMessage("", MSG_SPRASH56, MSG_SPRASH57, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Me.Refresh()
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            mExitFlag = 0
            Md = 0

            '-------------------------------------------------------------------
            '   �{�^���҂�����
            '-------------------------------------------------------------------

            Do

                ' START/RESET�L�[�����҂�
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW�����`�F�b�N
                If (r = cFRS_ERR_EMG) Then
                    mExitFlag = cFRS_ERR_EMG
                    GoTo STP_EXIT
                End If

                ' RESET�L�[���� ?
                If (sts = cFRS_ERR_RST) Then                            ' RESET�L�[����Cancel�{�^������ ? 
                    mExitFlag = cFRS_ERR_OTHER                            ' Retuen�l = RESET�L�[����Cancel�{�^������
                    Exit Do                                             ' ➑̃J�o�[�m�F��
                End If

                ' START�L�[���� ?
                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' START�L�[����OK�{�^������ ? V1.18.0.7�A
                    mExitFlag = cFRS_ERR_START                          ' Retuen�l = START�L�[����OK�{�^������
                    Exit Do                                             ' ➑̃J�o�[�m�F��
                End If

                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then                       ' ����~ ?
                    mExitFlag = cFRS_ERR_EMG                            ' Retuen�l = ����~
                    GoTo STP_EXIT                                       ' ����~���b�Z�[�W�\����
                End If
                Me.BringToFront()                                       ' �őO�ʂɕ\�� 

            Loop While (mExitFlag = 0)

            ' �V�O�i���^���[����(On=�Ȃ�, Off=�ԓ_��+�u�U�[�P)
            'V5.0.0.9�M ��
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)     ' V4.0.0.0-25
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            'V5.0.0.9�M ��

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
            ' ➑̃J�o�[���m�F����
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Me.Size = stSzNML                                           ' Form�̕�/������W�����[�h�p�ɂ���
            Me.Visible = False                                          ' ���b�Z�[�W�\�������� 
            r = Sub_CoverCheck()                                        ' ➑̃J�o�[���m�F����
            If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
                mExitFlag = cFRS_ERR_EMG                                ' Retuen�l = ����~
                GoTo STP_EXIT                                           ' ����~���b�Z�[�W�\����
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_NgRateAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExitFlag = cERR_TRAP                                       ' Retuen�l = �ׯ�ߴװ����
        End Try

STP_EXIT:
        BtnCancel.Visible = False                                       ' Cancel�{�^����\��
        BtnOK.Visible = False                                           ' OK�{�^����\��
        BtnCancel.Text = "Cancel"
        Return (mExitFlag)
    End Function
#End Region
    '----- V6.0.3.0�S�� -----
#Region "�J�b�g�I�t�����ŃG���[����������"
    '''=========================================================================
    ''' <summary>�J�b�g�I�t�����ŃG���[����������</summary>
    ''' <param name="Mode">(INP)�������[�h</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_CutOffTurnError(ByVal Mode As Integer) As Integer

        Dim r As Integer
        Dim ErrCode As Short                                            ' V1.18.0.0�B
        Dim strMSG As String
        Dim strMSG2 As String

        Try
            strMSG = "ALARM"
            strMSG2 = "CutOff"
            Call ObjSys.OperationLogging(gSysPrm, strMSG, strMSG2)

            ' �V�O�i���^���[����(On=�ُ�+�޻ް1, Off=�S�ޯ�) 
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(�ԓ_��+�u�U�[�P)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)

                Case SIGTOWR_SPCIAL                                     ' ����(�ԓ_��+�u�U�[�P)

            End Select

            ErrCode = cFRS_ERR_REPROB                                   '
            strMSG = MSG_SPRASH70                                       ' "�J�b�g�I�t�����Ɏ��s���܂����B" 'V6.0.3.0�S
            strMSG2 = MSG_SPRASH71                                      ' "START:�Ď��s�ARESET:���b�g���f" 'V6.0.3.0�S

            Call Sub_SetMessage(strMSG, strMSG2, "", System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText, System.Drawing.SystemColors.ControlText)
            Me.Refresh()
            'BtnCancel.Text = "START"
            'BtnCancel.Visible = True                                    ' OK�{�^���\��

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()
            ' �A���[���t�@�C���ɃA���[���f�[�^����������
            Call WriteAlarmData(gFPATH_QR_ALARM, strMSG, stPRT_ROHM.AlarmST_time, ErrCode)
            ' ----- V1.18.0.0�B�� -----

            ' ���b�Z�[�W�\������START�L�[�����҂�(OK�{�^�����L��)
            r = Sub_WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST)     ' START�������҂�
            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----
            If (r = cFRS_ERR_EMG) Then                                  ' ����~���o ?
                GoTo STP_END                                            ' ����~���b�Z�[�W�\����
            End If

            '----- ###181�� -----
            ' �V�O�i���^���[����(On=�Ȃ�, Off=�ُ�+�޻ް1) 
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(�ԓ_��+�u�U�[�P)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)

                Case SIGTOWR_SPCIAL                                     ' ����(�ԓ_��+�u�U�[�P)

            End Select

STP_END:
            BtnCancel.Text = "Cancel"
            BtnCancel.Visible = False                                   ' Cancel�{�^����\��
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_CutOffTurnError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region
    '----- V6.0.3.0�S�� -----

#If START_KEY_SOFT Then

#Region "�{�^���\���A��\������"

    Private Sub Sub_StartResetButtonDispOn()
        Try
            Sub_StartResetButtonDispOff()
            BtnCancel.Location = LocCanBtn2                     ' Cancel�{�^���\���ʒu���E�ɂ��炷 
            BtnCancel.Enabled = True                            ' Cancel�{�^���\��
            BtnCancel.Visible = True                            ' Cancel�{�^���\��
            BtnOK.Enabled = True                                ' OK�{�^���\��
            BtnOK.Visible = True                                ' OK�{�^���\��
            BtnOther.Enabled = False                            ' Other�{�^����\��
            BtnOther.Visible = False                            ' Other�{�^����\��
            BtnOK.Text = "START"                                ' OK�{�^���\��
            BtnCancel.Text = "RESET"                            ' Cancel�{�^���\��
            BtnOK.Size = New System.Drawing.Size(130, 40)
            BtnCancel.Size = New System.Drawing.Size(130, 40)
        Catch ex As Exception
            MsgBox("FrmReset.Sub_StartResetButtonDispOn() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub Sub_StartButtonDispOn()
        Try
            Sub_StartResetButtonDispOff()
            BtnOK.Enabled = True                                ' OK�{�^���\��
            BtnOK.Visible = True                                ' OK�{�^���\��
            BtnOK.Text = "START"                                ' OK�{�^���\��
            BtnOK.Size = New System.Drawing.Size(130, 40)
        Catch ex As Exception
            MsgBox("FrmReset.Sub_StartButtonDispOn() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub Sub_ResetButtonDispOn()
        Try
            Sub_StartResetButtonDispOff()
            BtnCancel.Location = LocCanBtnOrg                   ' Cancel�{�^���\���ʒu���E�ɂ��炷 
            BtnCancel.Enabled = True                            ' Cancel�{�^���\��
            BtnCancel.Visible = True                            ' Cancel�{�^���\��
            BtnCancel.Text = "RESET"                            ' Cancel�{�^���\��
            BtnCancel.Size = New System.Drawing.Size(130, 40)
        Catch ex As Exception
            MsgBox("FrmReset.Sub_ResetButtonDispOn() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub Sub_StartResetButtonDispOff()
        Try
            BtnCancel.Enabled = False                           ' Cancel�{�^���\��
            BtnCancel.Visible = False                           ' Cancel�{�^���\��
            BtnOK.Enabled = False                               ' OK�{�^���\��
            BtnOK.Visible = False                               ' OK�{�^���\��
            'BtnOther.Enabled = False                            ' Other�{�^����\��
            BtnOther.Visible = False                            ' Other�{�^����\��
        Catch ex As Exception
            MsgBox("FrmReset.Sub_StartResetButtonOff() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#End If

    '#Region "�m�f���ɂ��\������"
    '    '''=========================================================================
    '    ''' <summary>�m�f���ɂ��\�������\��</summary>
    '    ''' <returns>cFRS_ERR_START  = START�L�[����OK�{�^������
    '    '''          cFRS_ERR_RST    = RESET�L�[����Cancel�{�^������
    '    '''          cFRS_ERR_OTHER  = OTHER�{�^������
    '    '''          ��L�ȊO      = �G���[</returns>
    '    '''=========================================================================
    '    Private Function Sub_NgCountAlarm() As Integer

    '        Dim sts As Long = 0
    '        Dim r As Long = 0
    '        Dim strMSG As String

    '        Try
    '            '-------------------------------------------------------------------
    '            '   ��������
    '            '-------------------------------------------------------------------
    '            ' OK/Cancel�{�^���\���ݒ�
    '            BtnCancel.Location = LocCanBtn2                             ' Cancel�{�^���\���ʒu���E�ɂ��炷 
    '            BtnCancel.Visible = True                                    ' Cancel�{�^���\��
    '            BtnOK.Visible = True                                        ' OK�{�^���\��

    '            'BtnOK.Text = MSG_SPRASH59                                         ' �{�^���\���u���s�v
    '            'BtnCancel.Text = MSG_SPRASH60                                     ' �{�^���\���u���~�v
    '            'BtnOther.Text = MSG_SPRASH61                              ' �{�^���\���u�N���[�j���O�v

    '            'BtnOther.Visible = True                                     ' Other�{�^���\��
    '            'BtnOther.Visible = False
    '            BtnOK.Left = 50
    '            BtnOther.Left = BtnOK.Left + BtnOK.Width + 40                            ' �{�^���\���u�N���[�j���O�v
    '            BtnCancel.Left = BtnOther.Left + BtnOther.Width + 40

    '            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���

    '            'V5.0.0.9�M ��
    '            '' �}�K�W���I�����̃V�O�i���^���[����
    '            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
    '            'V5.0.0.9�M ��

    '            Me.Top = 800
    '            Me.Left = Me.Left
    '            Me.Refresh()

    '            ' ���b�Z�[�W�\��
    '            ' "�m�f���ɂ���~", "OK�{�^�������Ŏ����^�]�𑱍s���܂�", "Cancel�{�^�������Ŏ����^�]���I�����܂�"
    '            Call Sub_SetMessage(strMSG, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
    '            Me.Refresh()
    '            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
    '            mExitFlag = 0
    '            'V5.0.0.9�P            Md = 0

    '            '-------------------------------------------------------------------
    '            '   �{�^���҂�����
    '            '-------------------------------------------------------------------
    '            Dim md As Short = Globals_Renamed.giAppMode                 'V5.0.0.9�P
    '            Globals_Renamed.giAppMode = APP_MODE_IDLE                   'V5.0.0.9�P
    '            Do

    '                ' START/RESET�L�[�����҂�
    '                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW�����`�F�b�N
    '                If (r = cFRS_ERR_EMG) Then
    '                    mExitFlag = cFRS_ERR_EMG
    '                    GoTo STP_EXIT
    '                End If

    '                ' RESET�L�[���� ?
    '                If (sts = cFRS_ERR_RST) Then                            ' RESET�L�[����Cancel�{�^������ ? 
    '                    mExitFlag = cFRS_ERR_RST                            ' Retuen�l = RESET�L�[����Cancel�{�^������
    '                    Exit Do                                             ' ➑̃J�o�[�m�F��
    '                End If

    '                ' START�L�[���� ?
    '                If ((BtnOK.Enabled = True) And (sts = cFRS_ERR_START)) Then ' START�L�[����OK�{�^������ ? V1.18.0.7�A
    '                    mExitFlag = cFRS_ERR_START                          ' Retuen�l = START�L�[����OK�{�^������
    '                    Exit Do                                             ' ➑̃J�o�[�m�F��
    '                End If

    '                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
    '                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)
    '                If ObjSys.EmergencySwCheck() Then                       ' ����~ ?
    '                    mExitFlag = cFRS_ERR_EMG                            ' Retuen�l = ����~
    '                    GoTo STP_EXIT                                       ' ����~���b�Z�[�W�\����
    '                End If
    '                Me.BringToFront()                                       ' �őO�ʂɕ\�� 

    '            Loop While (mExitFlag = 0)
    '            Globals_Renamed.giAppMode = md                              'V5.0.0.9�P

    '            ' �V�O�i���^���[����(On=�Ȃ�, Off=�ԓ_��+�u�U�[�P)
    '            'V5.0.0.9�M ��
    '            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
    '            'V5.0.0.9�M ��

    '            '-------------------------------------------------------------------
    '            '   �I������
    '            '-------------------------------------------------------------------
    '            ' ➑̃J�o�[���m�F����
    '            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
    '            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
    '            Me.Size = stSzNML                                           ' Form�̕�/������W�����[�h�p�ɂ���
    '            Me.Visible = False                                          ' ���b�Z�[�W�\�������� 
    '            r = Sub_CoverCheck()                                        ' ➑̃J�o�[���m�F����
    '            If (r = cFRS_ERR_EMG) Then                                  ' ����~ ?
    '                mExitFlag = cFRS_ERR_EMG                                ' Retuen�l = ����~
    '                GoTo STP_EXIT                                           ' ����~���b�Z�[�W�\����
    '            End If

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            strMSG = "FrmReset.Sub_NgRateAlarm() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '            mExitFlag = cERR_TRAP                                       ' Retuen�l = �ׯ�ߴװ����
    '        End Try

    'STP_EXIT:
    '        BtnCancel.Visible = False                                       ' Cancel�{�^����\��
    '        BtnOK.Visible = False                                           ' OK�{�^����\��
    '        BtnCancel.Text = "Cancel"
    '        Return (mExitFlag)
    '    End Function
    '#End Region

End Class

'=============================== END OF FILE ===============================