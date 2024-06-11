'===============================================================================
'   Description  : ���[�_�A���[����ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DllSystem        'V6.0.0.0�Q
Imports TKY_ALL_SL432HW.My.Resources        'V4.4.0.0-0

Public Class FormLoaderAlarm

#Region "�y�ϐ���`�z"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    Private mExitFlag As Integer                                        ' �I���t���O
    Private mMode As Short                                              ' Start/Reset�L�[ ###074
    Private mAlarmKind As Integer                                       ' �A���[����� ###144
    'V6.0.0.0�Q    Private ObjSys As Object                                            ' OcxSystem�I�u�W�F�N�g

    Private mAlarmLevel As Integer                                      ' �A���[�����x���ۑ��p        ''V5.0.0.7�@

    '----- ���[�_�A���[����� -----
    'Private AlarmKind As Integer                                        ' �A���[�����(�S��~�ُ�, �T�C�N����~, �y�̏�, �A���[������)
    'Private AlarmCount As Integer                                       ' �����A���[���� 
    'Private strLoaderAlarm(LALARM_COUNT) As String                      ' �A���[��������
    'Private strLoaderAlarmInfo(LALARM_COUNT) As String                  ' �A���[�����1
    Private strLoaderAlarmExec(LALARM_COUNT) As String                  ' �A���[�����(�΍�)

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
    ''' <param name="Owner">             (INP)���g�p</param>
    ''' <param name="ObjSystem">         (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="AlarmKind">         (INP)�A���[�����(�S��~�ُ�, �T�C�N����~, �y�̏�, �A���[������)</param>
    ''' <param name="AlarmCount">        (INP)�����A���[����</param>
    ''' <param name="strLoaderAlarm">    (INP)�A���[��������</param>
    ''' <param name="strLoaderAlarmInfo">(INP)�A���[�����1(�����g�p)</param>
    ''' <param name="pstrLoaderAlarmExec">(INP)�A���[�����(�΍�)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal ObjSystem As SystemNET, ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
                                    ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef pstrLoaderAlarmExec() As String)
        'Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window, ByVal ObjSystem As Object, ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
        '                                ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef pstrLoaderAlarmExec() As String)

        Dim strMSG As String

        Try
            ' ��������
            mExitFlag = -1                                              ' �I���t���O = ������
            'V6.0.0.0�Q            ObjSys = ObjSystem                                          ' OcxSystem�I�u�W�F�N�g
            strLoaderAlarmExec = pstrLoaderAlarmExec                    ' �A���[�����(�΍�)��ޔ�����(�N���b�N�C�x���g�����Ŏg�p���邽��) 

            mAlarmKind = AlarmKind                                      ' ###144
            '----- ###196�� -----
            ' �u�y�̏ᔭ�����v�܂��́u�T�C�N����~�v�Ȃ�Cancel�{�^����\������
            'If (AlarmKind = cFRS_ERR_LDR3) Then                         ' �y�̏ᔭ�����Ȃ�Cancel�{�^����\������ ###073
            If (AlarmKind = cFRS_ERR_LDR3) Or (AlarmKind = cFRS_ERR_LDR2) Then
                BtnCancel.Visible = True                                ' Cancel�{�^���\��
                mMode = cFRS_ERR_START + cFRS_ERR_RST                   ' START/RESET�L�[�����҂� ###074
            Else
                BtnCancel.Visible = False                               ' Cancel�{�^����\��
                mMode = cFRS_ERR_START                                  ' START�L�[�����҂� ###074
            End If
            '----- ###196�� -----
            ' ���[�_�A���[������ݒ肷��
            Call SetAlarmList(AlarmKind, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

            ' ----- V1.18.0.0�B�� -----
            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            If (AlarmKind <> cFRS_NORMAL) Then
                ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
                Call SetAlmStartTime()
                ' �A���[���t�@�C���ɃA���[���f�[�^����������
                Call WriteAlarmData(gFPATH_QR_ALARM, strLoaderAlarm(0), stPRT_ROHM.AlarmST_time, AlarmKind)
            End If
            ' ----- V1.18.0.0�B�� -----

            ' ��ʕ\��
            Me.ShowDialog()                                             ' ��ʕ\��
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.ShowDialog() TRAP ERROR = " + ex.Message
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
    Private Sub FormLoaderAlarm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ���x�����ݒ�
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    LblAlarmList.Text = "�A���[�����X�g"
            '    LblExec.Text = "�΍�"
            'Else
            '    LblAlarmList.Text = "Alarm List"
            '    LblExec.Text = "Measures"
            'End If
            '----- ###158�� -----
            '"���u���Ɋ���c���Ă���ꍇ��", "��菜���Ă�������"
            LblMSG.Text = MSG_LOADER_29 & MSG_LOADER_18
            '----- ###158�� -----

            '----- ###144�� -----
            ' �{�^�����ݒ�
            'BtnTableClampOff.Text = MSG_LOADER_24                           '"�ڕ���N�����v����" �{�^��
            'BtnTableVacumeOff.Text = MSG_LOADER_25                          '"�ڕ���z������" �{�^��
            'BtnHandVacumeOff.Text = MSG_LOADER_26                           '"�n���h�z������" �{�^��
            'BtnHandVacumeOff.Text = MSG_LOADER_27                           '"�����n���h�z������" �{�^�� ###158
            'BtnHand2VacumeOff.Text = MSG_LOADER_28                          '"���[�n���h�z������" �{�^�� ###158

            ' �{�^���\��/��\���ݒ�
            '----- ###196�� -----
            'If (mAlarmKind = cFRS_ERR_LDR3) Then                            ' �y�̏ᔭ�����Ȃ�
            If (mAlarmKind = cFRS_ERR_LDR3) Or (mAlarmKind = cFRS_ERR_LDR2) Then
                BtnTableVacumeOff.Visible = False                           ' �ڕ���z�������{�^����\��
                BtnTableClampOff.Visible = False                            ' �ڕ���N�����v�����{�^����\��
                BtnHandVacumeOff.Visible = False                            ' �����n���h�z��������\��
                BtnHand2VacumeOff.Visible = False                           ' ���[�n���h�z��������\�� ###158
                LblMSG.Visible = False                                      ' ###158
            Else
                BtnTableVacumeOff.Visible = True                            ' �ڕ���z�������{�^���\��
                BtnTableClampOff.Visible = True                             ' �ڕ���N�����v�����{�^���\��
                BtnHandVacumeOff.Visible = True                             ' �����n���h�z�������{�^���\��
                BtnHand2VacumeOff.Visible = True                            ' ���[�n���h�z�������{�^���\�� ###158
                LblMSG.Visible = True                                       ' ###158
            End If
            '----- ###144�� -----
            '----- ###196�� -----

            '----- V1.18.0.6�A�� -----
            ' �t�H�[���̕\���ʒu��ݒ肷�� ###158 ###074
            'Me.Location = Form1.chkDistributeOnOff.Location
            'Me.Location = Form1.GrpNgBox.Location
            Dim Pos As System.Drawing.Point
            Pos.Y = Form1.Size.Height - Me.Size.Height
            Pos.X = 0
            Me.Location = Pos
            '----- V1.18.0.6�A�� -----

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.FormLoaderAlarm_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Shown������"
    '''=========================================================================
    ''' <summary>Form_Shown������ ###074</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FormLoaderAlarm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Dim strMSG As String

        Try
            ' Start/Reset�L�[�����҂�
            mExitFlag = Sub_WaitStartRestKey(mMode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.FormLoaderAlarm_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' �t�H�[�������
    End Sub
#End Region

#Region "Start/Reset�L�[�����҂����ٰ��"
    '''=========================================================================
    ''' <summary>Start/Reset�L�[�����҂����ٰ�� ###074</summary>
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
        Dim OutBit As Integer = 0                                   ' ###208
        Dim lData As Long                                           'V5.0.0.7�@
        Dim AlarmCount As Integer = 0                               'V5.0.0.7�@
        Dim strLoaderAlarm(128) As String                           'V5.0.0.7�@
        Dim strLoaderAlarmInfo(128) As String                       'V5.0.0.7�@
        Dim pstrLoaderAlarmExec(128) As String                      'V5.0.0.7�@
        Dim iData(2) As UShort                                      'V5.0.0.7�@

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
            mExitFlag = -1
            '----- ###208 -----
            Call ZATLDRED(OutBit)
            Call SetLoaderIO(&H0, LOUT_SUPLY)
            '----- ###208 -----

            Do
                r = STARTRESET_SWCHECK(False, sts)                  ' START/RESET SW�����`�F�b�N
                If (sts = cFRS_ERR_RST) And ((Md = cFRS_ERR_RST) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_RST                        ' ExitFlag = Cancel(RESET�L�[)
                ElseIf (sts = cFRS_ERR_START) And ((Md = cFRS_ERR_START) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    mExitFlag = cFRS_ERR_START                      ' ExitFlag = OK(START�L�[)
                End If

                System.Windows.Forms.Application.DoEvents()         ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(250)             ' Wait(msec)
                If Form1.System1.EmergencySwCheck() Then            ' ����~ ?
                    mExitFlag = cFRS_ERR_EMG                        ' Return�l = ����~���o
                End If
                ''V5.0.0.7�@��
                ' �y�̏ᔭ�����ɏd�̏Ⴊ���������ꍇ�ɂ́A�d�̏��D��Ƃ��ĕ\������ 
                If mMode = (cFRS_ERR_START + cFRS_ERR_RST) Then       '�y�̏�̏ꍇ�FSTART,RESET�҂�

                    ' ���[�_���A���[����Ԃ��`�F�b�N����
                    r = W_Read(LOFS_W110, lData)                            ' ���[�_�A���[����Ԏ擾(W110.08-10)v
                    iData(0) = lData
                    If (lData And LARM_ARM3) Then                        ' �S��~�ُ픭����
                        BtnCancel.Visible = False                               ' Cancel�{�^����\��
                        mMode = cFRS_ERR_START                                  ' START�L�[�����҂� ###074
                        If (lData <> cFRS_NORMAL) Then                          ' �A���[������ ?
                            r = W_Read(LOFS_W115, lData)                        ' ���[�_�A���[���ڍ׎擾(W115.00-W115.15(���s�s��))
                            iData(0) = lData
                            r = W_Read(LOFS_W116, lData)                        ' ���[�_�A���[���ڍ׎擾(W116.00-W116.15(���s��))
                            iData(1) = lData
                            SetAlarmLevel(cFRS_ERR_LDR1)
                            ' ���[�_�A���[�����b�Z�[�W���쐬����(AlmCount = �����A���[����)
                            AlarmCount = Loader_MakeAlarmStrings(iData, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)

                            ' ���[�_�A���[������ݒ肷��
                            Call SetAlarmList(cFRS_ERR_LDR1, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        End If
                    End If

                End If
                ''V5.0.0.7�@��

                Me.BringToFront()                                   ' �őO�ʂɕ\�� 
            Loop While (mExitFlag = -1)

            '----- ###208 -----
            If (OutBit And LOUT_SUPLY) Then
                Call SetLoaderIO(LOUT_SUPLY, &H0)
            End If
            '----- ###208 -----

            Return (mExitFlag)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmReset.Sub_WaitRestKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "OK�{�^��������"
    '''=========================================================================
    ''' <summary>OK�{�^��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        Dim strMSG As String

        Try
            ' �A���[�����Z�b�g���o 
            Call W_RESET()                                              ' ###172

            mExitFlag = cFRS_NORMAL
            Me.Close()                                                  ' �t�H�[�������

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Cancel�{�^��������"
    '''=========================================================================
    ''' <summary>Cancel�{�^�������� ###073</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            '----- ###144�� -----
            ' �ڕ��䃏�[�N/�n���h���[�N�����{�^����\�����āASTART�L�[�����҂��Ƃ���
            BtnTableVacumeOff.Visible = True                            ' �ڕ���z�������{�^���\��
            BtnTableClampOff.Visible = True                             ' �ڕ���N�����v�����{�^���\��
            BtnHandVacumeOff.Visible = True                             ' �����n���h�z�������{�^���\��
            BtnHand2VacumeOff.Visible = True                            ' ���[�n���h�z�������{�^���\�� ###158
            LblMSG.Visible = True                                       ' "���u���Ɋ���c���Ă���ꍇ��", "��菜���Ă�������" ###158
            BtnCancel.Visible = False                                   ' Cancel�{�^����\��

            mExitFlag = Sub_WaitStartRestKey(cFRS_ERR_START)            ' START�L�[�����҂�
            If (mExitFlag = cFRS_NORMAL) Then                           ' START�L�[�����Ȃ�
                mExitFlag = cFRS_ERR_RST                                ' Retuen�l = Cancel�{�^������
            End If
            'mExitFlag = cFRS_ERR_RST
            '----- ###144�� -----

            ''V6.0.5.0�F��'V4.12.2.2�B
            'START�L�[�������ꂽ��L�����Z�����������s����
            If (mExitFlag = cFRS_ERR_START) Then                           ' START�L�[�����Ȃ�
                mExitFlag = cFRS_ERR_RST                                ' Retuen�l = Cancel�{�^������
            End If
            ''V6.0.5.0�F��'V4.12.2.2�B

            Me.Close()                                                  ' �t�H�[�������

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�����n���h�z�������{�^��������"
    '''=========================================================================
    ''' <summary>�n���h�z�������{�^�������� ###158 ###144</summary>
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

            ' �n���h�P/�n���h�Q�z���n�e�e
            Call W_HAND1_VACUME()                                       ' �n���h�P�z���n�e�e
            'Call W_HAND2_VACUME()                                      ' �n���h�Q�z���n�e�e ###158

            ' �A���[�����Z�b�g���o ###147
            Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnFreeHand_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���[�n���h�z�������{�^��������"
    '''=========================================================================
    ''' <summary>���[�n���h�z�������{�^�������� ###158</summary>
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

            ' �A���[�����Z�b�g���o
            Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnHand2VacumeOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ڕ���N�����v�����{�^��������"
    '''=========================================================================
    ''' <summary>�ڕ���N�����v�����{�^�������� ###144</summary>
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

            ' �A���[�����Z�b�g���o ###147
            Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnFreeTable_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ڕ���z�������{�^��������"
    '''=========================================================================
    ''' <summary>�ڕ���z�������{�^�������� ###144</summary>
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

            ' �A���[�����Z�b�g���o ###147
            Call W_RESET()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnFreeTable_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Buzzer Off�{�^��������(�^�b�`�p�l���p)"
    '''=========================================================================
    ''' <summary>Buzzer Off�{�^�������� ###073</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnBZOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBZOff.Click

        Dim strMSG As String

        Try
            ' �u�U�[OFF
            Call W_BzOff()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.BtnBZOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
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
            ' �A���[����ʂ�ݒ肷��
            Select Case AlarmKind
                Case cFRS_NORMAL                                                    ' �A���[���Ȃ�
                    LblKind.Text = MSG_LOADER_16 & "(" & "---" & ")"                ' ���[�_�A���[�����X�g(---)

                Case cFRS_ERR_LDR1 ' �S��~�ُ픭����
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH19 & ")"         ' ���[�_�A���[�����X�g(�S��~�ُ픭����)

                Case cFRS_ERR_LDR2 ' ���ْ�~�ُ픭����
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH20 & ")"         ' ���[�_�A���[�����X�g(�T�C�N����~�ُ픭����)

                Case cFRS_ERR_LDR3  ' �y�̏ᔭ����
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH21 & ")"         ' ���[�_�A���[�����X�g(�y�̏ᔭ����)

                Case cFRS_ERR_PLC   ' PLC�X�e�[�^�X�ُ�
                    LblKind.Text = MSG_LOADER_16 & "(" & MSG_SPRASH30 & ")"         ' ���[�_�A���[�����X�g(PLC�X�e�[�^�X�ُ�)
            End Select

            '----- V6.1.1.0�L�� -----
            If (giAlmTimeDsp = 1) Then                                              ' ���[�_�A���[�����̎��ԕ\���̗L��(0=�\���Ȃ�, 1=�\������)�@
                Call Get_NowYYMMDDHHMMSS(strMSG)
                LblKind.Text = LblKind.Text + " " + strMSG
            End If
            '----- V6.1.1.0�L�� -----

            ' ���[�_�A���[�����X�g��ݒ肷��
            Call ListAlarm.Items.Clear()
            'txtInfo.Text = ""
            TxtExec.Text = ""
            For i = 0 To (AlarmCount - 1)
                ListAlarm.Items.Add(strLoaderAlarm(i))                              ' �A���[��������
            Next
            'ListAlarm.SelectedIndex = 0

            ' �ŏ��͐擪��\��
            If (AlarmCount > 0) Then
                'txtInfo.Text = strLoaderAlarmInfo(0)        
                TxtExec.Text = strLoaderAlarmExec(0)
            End If
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FormLoaderAlarm.SetAlarmList() TRAP ERROR = " + ex.Message
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
            strMSG = "FormLoaderAlarm.ListAlarm_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�A���[�����x���̕ۑ�"
    ''' <summary>
    ''' �A���[�����x���̕ۑ� 'V5.0.0.7�@
    ''' </summary>
    ''' <param name="AlarmLevel"></param>
    ''' <remarks></remarks>
    Public Sub SetAlarmLevel(ByVal AlarmLevel As Integer)

        mAlarmLevel = AlarmLevel

    End Sub

#End Region

#Region "�A���[�����x���̎擾"
    ''' <summary>
    ''' �A���[�����x���̎擾 'V5.0.0.7�@
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAlarmLevel() As Integer

        Return (mAlarmLevel)

    End Function


#End Region

#End Region

End Class