'===============================================================================
'   Description  : �J�b�g�ʒu�␳��ʏ����yCHIP/NET �O���J�����z
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class FrmCutPosCorrect
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

#Region "�v���C�x�[�g�萔/�ϐ���`"
    '===========================================================================
    '   �萔/�ϐ���`
    '===========================================================================
    '----- �ϐ���` -----
    Private stJOG As JOG_PARAM                                          ' �����(JOG����)�p�p�����[�^
    Private mExit_flg As Short                                          ' �I���t���O
    Private dblTchMoval(3) As Double                                    ' �s�b�`�ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))

    '----- �J�b�g�ʒu�␳�p -----
    Private iMaxDataNum As Short                                        ' �����Ώۃf�[�^��
    Private iResistorNumber(MaxCntResist + 1) As Short                  ' ��R�ԍ�
    Private pfStartPos(2, MaxCntResist + 1) As Double                   ' �e�B�[�`���O�e�[�u��(X,Y �ő��R��(1 ORG))
    Private pfStartPosTeachPoint(2, MaxCntResist + 1) As Double         ' �e�B�[�`���O�e�[�u���O��␳�l�ۑ��p(X,Y �ő��R��(1 ORG))V5.0.0.6�I
#End Region

    '========================================================================================
    '   �e����������
    '========================================================================================
#Region "�I�����ʂ�Ԃ�"
    ' '''=========================================================================
    ' '''<summary>�I�����ʂ�Ԃ�</summary>
    ' '''<returns>cFRS_NORMAL   = ����
    ' '''         cFRS_ERR_RST  = Cancel(RESET��)
    ' '''         -1�ȉ�        = �G���[</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0�J
    '    'V6.0.0.0�J    Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        If (mExit_flg = cFRS_ERR_START) Then
    '            sGetReturn = cFRS_NORMAL
    '        Else
    '            sGetReturn = mExit_flg
    '        End If
    '    End Get
    'End Property
#End Region

#Region "̫�я�����������"
    '''=========================================================================
    '''<summary>̫�я�����������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        'V6.0.0.0�K                  ��
        stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                     BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
        'V6.0.0.0�K                  ��

    End Sub
#End Region

#Region "Form FormClosed������"
    '''=========================================================================
    '''<summary>Form Closed������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CutPosCorrect_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub
#End Region

#Region "Form Load������"
    '''=========================================================================
    '''<summary>Form Load������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CutPosCorrect_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ��������
            Call SetMessages()                                          ' ���x������ݒ肷��(���{��/�p��)
            mExit_flg = -1                                              ' �I���t���O = ������

            ' ����޳����۰قɏd�˂�
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' �g���~���O�f�[�^���K�v�ȃp�����[�^���擾����
            Call GetTrimData()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.FrmCutPosCorrect_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���x������ݒ肷��(���{��/�p��)"
    '''=========================================================================
    '''<summary>���x������ݒ肷��(���{��/�p��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetMessages()

        'Dim intChipNum As Short
        Dim strMSG As String

        Try
            ' ���x������ݒ肷��(���{��/�p��)
            'Call PrepareMessagesCutPosCorrect_Calibration(gSysPrm.stTMN.giMsgTyp)
            'Me.frmTitle.Text = FRM_CUT_POS_CORRECT_TITLE                                    ' "�J�b�g�ʒu�␳"
            'Me.lblGroupResistorTitle.Text = LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER       ' "�O���[�v����R��"
            'Me.lblCutCorrectOffsetXTitle.Text = LBL_CUT_POS_CORRECT_OFFSET_X                ' "�J�b�g�ʒu�␳�e�[�u���I�t�Z�b�g�w[mm]"
            'Me.lblCutCorrectOffsetYTitle.Text = LBL_CUT_POS_CORRECT_OFFSET_Y                ' "�J�b�g�ʒu�␳�e�[�u���I�t�Z�b�g�x[mm]"
            'V1.25.0.0�K'V5.0.0.6�J��
            If iExcamCutBlockNo_X > 0 Then
                strMSG = LBL_CUT_POS_CORRECT_BLOCK_NO_X + "(1-" + iExcamCutBlockNo_X.ToString("0") + ")"
            Else
                'V1.25.0.0�K'V5.0.0.6�J��
                strMSG = LBL_CUT_POS_CORRECT_BLOCK_NO_X + "(1-" + typPlateInfo.intBlockCntXDir.ToString("0") + ")"
            End If      'V1.25.0.0�K'V5.0.0.6�J
            Me.lblBlockNoX.Text = strMSG                                                    ' "�u���b�NNo X��(1�`99�j"
            strMSG = LBL_CUT_POS_CORRECT_BLOCK_NO_Y + "(1-" + typPlateInfo.intBlockCntYDir.ToString("0") + ")"
            Me.lblBlockNoY.Text = strMSG                                                    ' "�u���b�NNo Y��(1�`99�j"

            ' �\���f�[�^�ɏ����l��ݒ肷��
            'Call GetChipNum(intChipNum)
            'Me.lblGroupResistor.Text = intChipNum.ToString("0")                             ' �O���[�v����R��
            Me.lblGroupResistor.Text = typPlateInfo.intResistCntInBlock.ToString("0")       ' �u���b�N����R�� 
            Me.lblCutCorrectOffsetX.Text = typPlateInfo.dblCutPosiReviseOffsetXDir.ToString("##0.0000")     ' ��Ĉʒu�␳ð��ٵ̾��X
            Me.lblCutCorrectOffsetY.Text = typPlateInfo.dblCutPosiReviseOffsetYDir.ToString("##0.0000")     ' ��Ĉʒu�␳ð��ٵ̾��Y
            Me.txtBlockNoX.Text = "1"                                                       ' �u���b�N�ԍ�(X��)
            Me.txtBlockNoY.Text = "1"                                                       ' �u���b�N�ԍ�(Y��)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.SetMessages() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Activated������"
#If False Then                          'V6.0.0.0�L Execute()�ł����Ȃ�
    '''=========================================================================
    '''<summary>Form Activated������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CutPosCorrect_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' �J�b�g�ʒu�␳���s
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                               ' �I���t���O = 0
            mExit_flg = CutPosCorrectMain()                             ' �J�b�g�ʒu�␳���s

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCorrect_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return�l = ��O�G���[
        End Try
        Me.Close()

    End Sub
#End If
#End Region

#Region "���C���������s"
    ''' <summary>���C���������s</summary>
    ''' <returns>���s����</returns>
    ''' <remarks>'V6.0.0.0�L</remarks>
    Public Function Execute() As Integer Implements ICommonMethods.Execute
        Try
            ' �J�b�g�ʒu�␳���s
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                               ' �I���t���O = 0
            mExit_flg = CutPosCorrectMain()                             ' �J�b�g�ʒu�␳���s

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "FrmCutPosCorrect.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return�l = ��O�G���[
        End Try

        Dim ret As Integer
        If (mExit_flg = cFRS_ERR_START) Then    ' sGetReturn ��荞��   'V6.0.0.0�L
            ret = cFRS_NORMAL
        Else
            ret = mExit_flg
        End If

        Me.Close()
        Return ret

    End Function
#End Region

    '========================================================================================
    '   �{�^������������
    '========================================================================================
#Region "OK�{�^������������"
    '''=========================================================================
    '''<summary>OK�{�^������������</summary>
    '''<remarks>�蓮�e�B�[�`���O�����p</remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        stJOG.Flg = cFRS_NORMAL                                        ' �����ݒ肷��
    End Sub
#End Region

#Region "Cancel�{�^������������"
    '''=========================================================================
    '''<summary>Cancel�{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        stJOG.Flg = cFRS_ERR_RST                                        ' Cancel(RESET��)
    End Sub
#End Region

    '========================================================================================
    '   �J�b�g�ʒu�␳����
    '========================================================================================
#Region "�J�b�g�ʒu�␳���C������"
    '''=========================================================================
    '''<summary>�J�b�g�ʒu�␳���C������</summary>
    '''<returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST  =Cancel(RESET��)
    '''         -1�ȉ�        =�G���[</returns>
    '''=========================================================================
    Private Function CutPosCorrectMain() As Integer

        Dim mdBSx As Double                                                     ' ��ۯ�����X
        Dim mdBSy As Double                                                     ' ��ۯ�����Y
        Dim BlkNumX As Integer
        Dim BlkNumY As Integer
        Dim StepNum As Integer
        Dim r As Integer
        Dim rtn As Integer
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            BlkNumX = 1                                                         ' �u���b�N�ԍ�X,Y������
            BlkNumY = 1
            txtBlockNoX.Text = BlkNumX.ToString("0")
            txtBlockNoY.Text = BlkNumY.ToString("0")
            Call CalcBlockSize(mdBSx, mdBSy)                                    ' �u���b�N�T�C�YXY�ݒ�
            Call LAMP_CTRL(LAMP_START, True)                                    ' START�����vON
            Call LAMP_CTRL(LAMP_RESET, True)                                    ' RESET�����vON

            ' �����(JOG����)�p�p�����[�^������������
            stJOG.Md = MODE_KEY                                                 ' ���[�h(�L�[���͑҂����[�h)
            stJOG.Md2 = MD2_BUTN                                                ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START                     ' �L�[�̗L��(1)/����(0)�w��
            stJOG.PosX = 0.0                                                    ' BP X�ʒu/XYð��� X�ʒu
            stJOG.PosY = 0.0                                                    ' BP Y�ʒu/XYð��� Y�ʒu
            stJOG.BpOffX = typPlateInfo.dblBpOffSetXDir                         ' BP�̾��X 
            stJOG.BpOffY = typPlateInfo.dblBpOffSetYDir                         ' BP�̾��Y 
            stJOG.BszX = mdBSx                                                  ' ��ۯ�����X 
            stJOG.BszY = mdBSy                                                  ' ��ۯ�����Y
            'V4.7.0.0�I            stJOG.TextX = fgrdPoints                                            ' BP�ʒu�\���p�O���b�h
            '            stJOG.TextX = dgvPoints                                             ' BP�ʒu�\���p�O���b�h
            'V6.0.0.0�J            stJOG.TextX = 0                                                     ' BP�ʒu�\���p�O���b�h
            stJOG.TextX = Nothing                                               ' BP�ʒu�\���p�O���b�h   'V6.0.0.0�J
            'V6.0.0.0�J            stJOG.TextY = 0                                                     ' 
            stJOG.TextY = Nothing                                               '   'V6.0.0.0�J
            stJOG.cgX = 0.0                                                     ' �ړ���X 
            stJOG.cgY = 0.0                                                     ' �ړ���Y 
            stJOG.BtnHI = BtnHI                                                 ' HI�{�^��
            stJOG.BtnZ = BtnZ                                                   ' Z�{�^��
            stJOG.BtnSTART = BtnSTART                                           ' START�{�^��
            stJOG.BtnHALT = BtnHALT                                             ' HALT�{�^��
            stJOG.BtnRESET = BtnRESET                                           ' RESET�{�^��
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()                                                     ' �t�H�[�J�X��ݒ肷��(�e���L�[���͗p) 

            '---------------------------------------------------------------------------
            '   �u���b�N�ԍ����͏���
            '---------------------------------------------------------------------------
STP_BLKINP:
            ' �K�C�_���X���b�Z�[�W�\��
            Me.lblInfo.Text = MSG_CUT_POS_CORRECT_014                           ' "�u���b�N�ԍ�����͌�A[START]�L�[�������Ă�������"
            StepNum = 0                                                         ' StepNum = �u���b�N�ԍ�����
            Call SetTxtBlkNumBackColor(True)                                    ' �u���b�N�ԍ��e�L�X�g�{�b�N�X�̊�����

            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21
            ' �R���\�[������(JOG��ʂ�START/RESET�L�[���͑҂�)
            r = WaitJogStartResetKey(stJOG, CONSOLE_SW_RESET + CONSOLE_SW_START)
            If (r < cFRS_NORMAL) Then                                           ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            ' RESET�L�[�Ȃ�I���m�F���b�Z�[�W�\����
            If (r = cFRS_ERR_RST) Then                                          ' RESET�L�[���� ?
                Call LAMP_CTRL(LAMP_RESET, True)                                ' RESET�����vON
                GoTo STP_ENDMSG                                                 ' �I���m�F���b�Z�[�W�\����

                ' START�L�[�Ȃ�u���b�N�Ԃ���͂���
            Else
                r = InputBlkNum(BlkNumX, BlkNumY)
                If (r <> cFRS_NORMAL) Then GoTo STP_BLKINP '                    ' �G���[�Ȃ�u���b�N�ԍ����͂�

            End If
            Call SetTxtBlkNumBackColor(False)                                   ' �u���b�N�ԍ��e�L�X�g�{�b�N�X�̔񊈐���
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21

            '---------------------------------------------------------------------------
            '   �w��̃u���b�N�ԍ��ʒu�Ƀe�[�u�����ړ�����(�����J������)
            '---------------------------------------------------------------------------
            ' �e�[�u�����w��u���b�N�ʒu�Ɉړ�����(�����J������)
            r = XYTableMoveBlock(0, 1, 1, BlkNumX, BlkNumY)
            If (r <> cFRS_NORMAL) Then                                          ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                If (Form1.System1.IsSoftLimitXY(r) = False) Then
                    Return (r)
                End If
            End If

            '-------------------------------------------------------------------
            '   �w��u���b�N�̑S�Ă̒�R�ɏ\���J�b�g���s��(�����J������)
            '-------------------------------------------------------------------
            StepNum = 1                                                         ' StepNum = �\���J�b�g
            r = CutPosCrossCut(typPlateInfo)
            If (r = cFRS_ERR_RST) Then
                ' �u���b�N�ԍ����͎w��܂ŏ�����߂�
                GoTo STP_BLKINP
            ElseIf (r <> cFRS_NORMAL) Then                                          ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If

            '-------------------------------------------------------------------
            '   �\���J�b�g������R�̃p�^�[���F��(�O���J����)���s������ʂ��Z�o����
            '-------------------------------------------------------------------
            StepNum = 2                                                         ' StepNum = ����ʎZ�o
            r = CutPosCorrect(typPlateInfo, BlkNumX, BlkNumY)
            GoTo STP_END                                                        ' �����I�� 

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W��\������
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                        '�u�������v�I���Ȃ�e�B�[�`���O�ɖ߂�
                If (StepNum = 0) Then                                           ' StepNum = �u���b�N�ԍ����� ?
                    GoTo STP_BLKINP                                             ' �u���b�N�ԍ����͂�
                End If
            End If

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
STP_END:
            Call ZCONRST()                                                      ' ���b�`����
            Call LAMP_CTRL(LAMP_RESET, False)                                   ' RESET�����vOFF
            Call LAMP_CTRL(LAMP_Z, False)                                       ' PRB�����vOFF
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCorrectMain() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[

        Finally
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21
        End Try
    End Function
#End Region

#Region "�w��u���b�N�̑S�Ă̒�R�ɏ\���J�b�g���s��"
    '''=========================================================================
    ''' <summary>�w��u���b�N�̑S�Ă̒�R�ɏ\���J�b�g���s��</summary>
    ''' <param name="stPLT">  (INP)�v���[�g�f�[�^</param>
    ''' <returns>cFRS_NORMAL  = ����
    '''          cFRS_ERR_RST = RESET(Cancel)�L�[
    '''          ��L�ȊO     = �G���[</returns>
    '''=========================================================================
    Private Function CutPosCrossCut(ByRef stPLT As PlateInfo) As Integer

        Dim r As Integer
        Dim rtn As Integer
        Dim StepNum As Integer
        Dim SwitchChk As Integer = 0
        Dim PosX As Double
        Dim PosY As Double
        Dim dQrate As Double
        Dim CondNum As Integer
        Dim ObjProc As Process = Nothing
        Dim bFlg As Boolean = False
        Dim bHALT As Boolean = False
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                    ' �J�����ؑ�(�����J����)
#End If
            ' ������ V3.1.0.0�A 2014/12/01
            'r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0) ' �摜�\���v���O�������N������
            'V6.0.0.0�D            r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0) ' �摜�\���v���O�������N������
            ' ������ V3.1.0.0�A 2014/12/01
            SendCrossLineMsgToDispGazou()                               ' V6.1.4.18�@

            StepNum = 0                                                 ' StepNum = ���H��������
            stJOG.CurrentNo = 1                                         ' �������̍s
            Call BpMoveOrigin_Ex()
STP_INPCND:
            ' ���H��������͂���(FL��)
            CondNum = 0                                                 ' ���H�����ԍ� 
            dQrate = stPLT.dblCutPosiReviseCutQRate                     ' Qڰ� = �J�b�g�ʒu�␳�pQ���[�g 
            r = Sub_FlCond(CondNum, dQrate, Me)
            If (r <> cFRS_NORMAL) Then
                'If (r = cFRS_ERR_RST) Then
                '    Return r '                                          ' �O�̏�Ԃɖ߂�
                'End If
                GoTo STP_END
            End If
            Me.Refresh()                                                ' �����H�������͉�ʕ\������������
            StepNum = 1                                                 ' StepNum = �\���J�b�g

            '---------------------------------------------------------------------------
            '   �S�Ă̒�R�ɏ\���J�b�g���s��
            '---------------------------------------------------------------------------
STP_CROSCUT:
            stJOG.Flg = -1
            Do
                ' BP�ړ�
                'V5.0.0.6�I���O���J�����J�b�g�ʒu�␳�ʂ̉��Z
                If giTeachpointUse = 1 Then
                    PosX = pfStartPos(0, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetXDir + pfStartPosTeachPoint(0, stJOG.CurrentNo)  ' è��ݸ��߲��X
                    PosY = pfStartPos(1, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetYDir + pfStartPosTeachPoint(1, stJOG.CurrentNo)  ' è��ݸ��߲��Y
                Else
                    'V5.0.0.6�I��
                    PosX = pfStartPos(0, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetXDir
                    PosY = pfStartPos(1, stJOG.CurrentNo) + typPlateInfo.dblCutPosiReviseOffsetYDir
                End If                                                  'V5.0.0.6�I
                r = Form1.System1.EX_MOVE(gSysPrm, PosX, PosY, 1)       ' BP��Βl�ړ�
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    GoTo STP_END
                End If

                ' �K�C�_���X���b�Z�[�W�\��
                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_004               '  "�y�x���z" & "[START]�F�\���J�b�g���s�C[RESET]�F���~"
                Call Set_BackColor(stJOG.CurrentNo)                     ' �O���b�h�̌��ݍs�̔w�i�����F�ɂ���

                ' HALT SW�`�F�b�N
                r = HALT_SWCHECK(SwitchChk)
                If (SwitchChk <> 0) Then                                ' HALT SW�������ꂽ ?
                    Console.WriteLine("HALT SW ON")
                    If (bHALT = False) Then                             ' HALT SW��� = OFF�Ȃ�
                        bHALT = True                                    ' HALT SW��Ԃ�ON�ɂ���
                        Call LAMP_CTRL(LAMP_HALT, True)                 ' HALT�����vON
                    Else                                                ' HALT SW��� = ON�Ȃ�
                        bHALT = False                                   ' HALT SW��Ԃ�OFF�ɂ���
                        Call LAMP_CTRL(LAMP_HALT, False)                ' HALT�����vOFF
                    End If
                End If

                ' �R���\�[������(JOG��ʂ�START/RESET/HALT�L�[���͑҂�)
                If (bFlg = False) Or (bHALT = True) Then                ' ���񖔂�HALT SW��ON�̎���START/RESET�L�[���͑҂�
                    bFlg = True
STP_INPCONS:
                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21

                    ' �R���\�[������(JOG��ʂ�START/RESET/HALT�L�[���͑҂�)
                    r = WaitJogStartResetKey(stJOG, CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT)
                    If (r < cFRS_NORMAL) Then                           ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                        GoTo STP_END
                    End If
                    ' RESET�L�[�Ȃ�I���m�F���b�Z�[�W�\����
                    If (r = cFRS_ERR_RST) Then                          ' RESET�L�[���� ?
                        Call LAMP_CTRL(LAMP_RESET, True)                ' RESET�����vON
                        GoTo STP_ENDMSG                                 ' �I���m�F���b�Z�[�W�\����

                    ElseIf (r = cFRS_ERR_HALT) Then                     ' HALT�L�[���� ?
                        If (bHALT = False) Then                         ' HALT SW��� = OFF�Ȃ�
                            bHALT = True                                ' HALT SW��Ԃ�ON�ɂ���
                            Call LAMP_CTRL(LAMP_HALT, True)             ' HALT�����vON
                        Else                                            ' HALT SW��� = ON�Ȃ�
                            bHALT = False                               ' HALT SW��Ԃ�OFF�ɂ���
                            Call LAMP_CTRL(LAMP_HALT, False)            ' HALT�����vOFF
                        End If
                        GoTo STP_INPCONS                                ' �R���\�[������(JOG��ʂ�START/RESET/HALT�L�[���͑҂�)��
                    End If

                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21
                End If

                ' �\���J�b�g�����s����
                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_005               ' "�\���J�b�g���s��" & vbCrLf & "[HALT]:�ꎞ��~"
                Me.lblInfo.Refresh()
                r = CrossCutExec(PosX, PosY, CondNum, dQrate, stPLT.dblCutPosiReviseCutLength, stPLT.dblCutPosiReviseCutSpeed)
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    GoTo STP_END
                End If

                ' �\���J�b�g���BP���J�b�g�ʒu�����֖߂�
                r = Form1.System1.EX_MOVE(gSysPrm, PosX, PosY, 1)       ' BP��Βl�ړ�
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    GoTo STP_END
                End If

                ' �V�X�e���G���[�`�F�b�N
                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)         ' �V�X�e���G���[�`�F�b�N
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1)                   ' Wait(ms)

                ' ���̍s��
                Call Reset_BackColor(stJOG.CurrentNo)                   ' �O���b�h�̌��ݍs�̔w�i�F�����ɖ߂�
                stJOG.CurrentNo = stJOG.CurrentNo + 1                   ' �������̍s + 1
                If (stJOG.CurrentNo > iMaxDataNum) Then                 ' �����Ώۃf�[�^�����\���J�b�g���� ?
                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_011           ' "�\���J�b�g�I��
                    r = cFRS_NORMAL                                     ' ����I��
                    GoTo STP_END
                End If
                If (stJOG.CurrentNo <= 5) Then                          ' 5�s�ڈȍ~�͎�����۰�
                    Call Set_TopRow(1)                                  ' �O���b�h�̎w��s��擪�ɂ��� 
                Else
                    Call Set_TopRow(stJOG.CurrentNo - 5)                ' �O���b�h�̎w��s��擪�ɂ��� 
                End If

            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK(START)/RESET(Cancel)���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                If (r = cFRS_ERR_RST) Then                              ' RESET(Cancel)���݉����Ȃ�I���m�F���b�Z�[�W�\����
                    GoTo STP_ENDMSG
                End If
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W��\������
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                '�u�������v�I���Ȃ珈�����p������
                ' HALT SW��Ԃ�ON�Ȃ�HALT�����vON�Ƃ��� (��TrmMsgBox()�Ń��b�`����&HALT�����v��OFF�����)
                If (bHALT = True) Then
                    Call LAMP_CTRL(LAMP_HALT, True)
                End If
                If (StepNum = 0) Then                                   ' StepNum = ���H�������� ?
                    GoTo STP_INPCND                                     ' ���H�����ԍ����͂�
                Else
                    GoTo STP_CROSCUT                                    ' �\���J�b�g���s��
                End If
            End If

            ' �I������ 
STP_END:
            Call Reset_AllBackColor(iMaxDataNum)                        'V4.7.0.0�I
            'V6.0.0.0�D            Call End_GazouProc(ObjProc)                                 ' �摜�\���v���O�������I������
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALT�����vOFF
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[

        Finally                         'V6.0.0.0-21
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)
        End Try
    End Function
#End Region

#Region "�\���J�b�g������R�̃p�^�[���F��(�O���J����)���s���A����ʂ��Z�o����"
    '''=========================================================================
    ''' <summary>�\���J�b�g������R�̃p�^�[���F��(�O���J����)���s���A����ʂ��Z�o����</summary>
    ''' <param name="stPLT">  (INP)�v���[�g�f�[�^</param>
    ''' <param name="iBlockX"></param>
    ''' <param name="iBlockY"></param>
    ''' <returns>cFRS_NORMAL  = ����
    '''          cFRS_ERR_RST = RESET(Cancel)�L�[
    '''          ��L�ȊO     = �G���[</returns>
    '''=========================================================================
    Private Function CutPosCorrect(ByRef stPLT As PlateInfo, ByRef iBlockX As Short, ByRef iBlockY As Short) As Integer

        Dim Idx As Integer
        Dim EndNo As Integer
        Dim r As Integer
        Dim rtn As Integer
        Dim StepNum As Integer
        Dim intGroup As Short
        Dim intTemp As Short
        Dim dblGapX(MaxCntResist + 1) As Double                         ' �����X(�ő��R��(1 ORG))
        Dim dblGapY(MaxCntResist + 1) As Double                         ' �����Y(�ő��R��(1 ORG))
        Dim ObjProc As Process = Nothing
        Dim fcoeff As Double                                            ' ���֒l
        Dim crx As Double                                               ' �����X(���[�N)
        Dim cry As Double                                               ' �����Y(���[�N)
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            Call gparModules.CrossLineDispOff()    'V5.0.0.6�K
            StepNum = 0                                                 ' StepNum = �R���\�[������(START/RESET�L�[���͑҂�)
            stJOG.CurrentNo = 1                                         ' �������̍s
            intGroup = stPLT.intCutPosiReviseGroupNo                    ' �����ϯ�ݸ޸�ٰ�ߔԍ�
            intTemp = stPLT.intCutPosiRevisePtnNo                       ' �����ϯ�ݸ�����ڰĔԍ�

            For Idx = 1 To MaxCntResist                                 ' ����ʏ�����
                dblGapX(Idx) = 0.0                                      ' �����X
                dblGapY(Idx) = 0.0                                      ' �����Y
            Next Idx

            Call Form1.System1.EX_BPOFF(gSysPrm, 0.0#, 0.0#)            ' BP�I�t�Z�b�g(�O����ׂł�BP�͎g�p�ł��Ȃ��̂ŏ��������Ă���)
            r = Form1.System1.EX_MOVE(gSysPrm, 0.0#, 0.0#, 1)           ' �ŏ��̶�Ĉʒu�����ֈړ�
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If

            ' �e�[�u�����O���J�������Ɉړ�
            r = XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, stJOG.CurrentNo)
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                GoTo STP_END
            End If

            Call Reset_AllBackColor(iMaxDataNum)                        ' �O���b�h�̑S�s�̔w�i�F�����ɖ߂�
            Call Set_TopRow(stJOG.CurrentNo)                            ' �O���b�h�̎w��s��擪�ɂ��� 

#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)                    ' �J�����ؑ�(�O���J����)
#End If
            ' �摜�\���v���O�������N������(�O���J����)
            'r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 1)

STP_INPCONS:
            ' �K�C�_���X���b�Z�[�W�\��
            Me.lblInfo.Text = MSG_CUT_POS_CORRECT_007                   ' "[START]�F�摜�F�����s" & vbCrLf & "[RESET]�F���~"
            StepNum = 0                                                 ' StepNum = �u���b�N�ԍ�����

            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21

            ' �R���\�[������(START/RESET�L�[���͑҂�)
            r = WaitJogStartResetKey(stJOG, CONSOLE_SW_RESET + CONSOLE_SW_START)
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                GoTo STP_END
            End If
            ' RESET�L�[�Ȃ�I���m�F���b�Z�[�W�\����
            If (r = cFRS_ERR_RST) Then                                  ' RESET�L�[���� ?
                Call LAMP_CTRL(LAMP_RESET, True)                        ' RESET�����vON
                GoTo STP_ENDMSG                                         ' �I���m�F���b�Z�[�W�\����
            End If
            StepNum = 1                                                 ' StepNum = �p�^�[���F�����s

            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21
            System.Windows.Forms.Application.DoEvents()                 ' ���b�Z�[�W�|���v

            '---------------------------------------------------------------------------
            '   �p�^�[���F��(�O���J����)�����s���A����ʂ��Z�o����
            '---------------------------------------------------------------------------
STP_PTNRECOG:
            stJOG.Flg = -1
            Do
                ' �K�C�_���X���b�Z�[�W�\��
                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_016               ' "�摜�F�����s���s��"
                Me.lblInfo.Refresh()
                Call Set_BackColor(stJOG.CurrentNo)                     ' �O���b�h�̎w��s�̔w�i�F�����F�ɂ��� 

                ' �e�[�u�����O���J�������Ɉړ�
                r = XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, stJOG.CurrentNo)
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1000)                ' �e�[�u���ړ���摜����҂�����(ms)

                ' �O���J�����Ńp�^�[���F�����s��
                r = Sub_PatternMatching(intGroup, intTemp, crx, cry, fcoeff)
                If (r = cFRS_NORMAL) Then                               ' ���� ? 
                    'crx = dblGapX(stJOG.CurrentNo)                     '###152
                    'cry = dblGapY(stJOG.CurrentNo)                     '###152
                    dblGapX(stJOG.CurrentNo) = crx
                    dblGapY(stJOG.CurrentNo) = cry
                Else
                    If (r = cFRS_ERR_PT2) Then                          ' �摜�}�b�`���O�G���[(臒l�G���[) ?
                        '"�摜�}�b�`���O�G���[ (���֌W��=x.xxx)" + "���X�C�b�`:�ʒu����, [START]:����, [HALT]:�O��"
                        strMSG = MSG_CUT_POS_CORRECT_013 + " (" + MSG_CUT_POS_CORRECT_018 + fcoeff.ToString("0.000") + ")" + vbCrLf + MSG_CUT_POS_CORRECT_017
                    Else                                                ' �p�^�[���F���G���[(�p�^�[����������Ȃ�����)
                        '"�摜��������܂���" + "���X�C�b�`:�ʒu����, [START]:����, [HALT]:�O��"
                        strMSG = MSG_CUT_POS_CORRECT_019 + vbCrLf + MSG_CUT_POS_CORRECT_017
                    End If
                    Me.lblInfo.Text = strMSG                            ' "�摜�}�b�`���O�G���[" + "���X�C�b�`:�ʒu����, [START]:����, [HALT]:�O��"
                    Me.lblInfo.Refresh()
                    EndNo = stJOG.CurrentNo                             ' �ŏI�s = �������̍s�ԍ�(�������̍s�ԍ��ȑO�܂ł̎蓮�e�B�[�`���O�������s��)  
STP_MANUAL:
                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21

                    ' �蓮�e�B�[�`���O����
                    r = ManualTeach(stPLT, iBlockX, iBlockY, stJOG.CurrentNo, EndNo, dblGapX, dblGapY)
                    If (r <> cFRS_NORMAL) Then                          ' ����ȊO�Ȃ�I��
                        GoTo STP_END
                    Else                                                ' ���탊�^�[���� 
                        If (StepNum = 2) Then                           ' StepNum = �蓮�e�B�[�`���O�����Ȃ�u�ۑ��m�F���b�Z�[�W�v�\����
                            GoTo STP_EXITMSG
                        End If
                    End If

                    Form1.Instance.VideoLibrary1.SetTrackBarEnabled(False)      'V6.0.0.0-21
                End If

                ' ����ʂ�\������
                Call DispGapXY(stJOG.CurrentNo, dblGapX(stJOG.CurrentNo), dblGapY(stJOG.CurrentNo))

                ' ���̍s��
                Call Reset_BackColor(stJOG.CurrentNo)                   ' �O���b�h�̎w��s�̔w�i�F�����ɖ߂�
                stJOG.CurrentNo = stJOG.CurrentNo + 1                   ' �������̍s + 1
                ' �ŏI�s�̏ꍇ
                If (stJOG.CurrentNo > iMaxDataNum) Then                 ' �����Ώۃf�[�^�����摜�F������ ?
                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_012           ' "�摜�F���I��"
                    stJOG.CurrentNo = stJOG.CurrentNo - 1               ' �������̍s - 1
STP_EXITMSG:
                    ' "���̏���ۑ����đO�̉�ʂɖ߂�܂��B��낵���ł����H"�@
                    r = Form1.System1.TrmMsgBox(gSysPrm, MSG_106, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
                    If (r = cFRS_ERR_RST) Then                          '�u�������v�I���Ȃ珈�����p������
                        StepNum = 2                                     ' StepNum = �蓮�e�B�[�`���O
                        EndNo = iMaxDataNum                             ' �ŏI�s = �����Ώۃf�[�^�� 
                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_017       ' "���X�C�b�`:�ʒu����, [START]:����, [HALT]:�O��"
                        GoTo STP_MANUAL                                 ' �蓮�e�B�[�`���O������
                    End If

                    ' �u�͂��v�I���Ȃ�f�[�^�X�V���ďI��
                    Call UpdateData(dblGapX, dblGapY)                   ' �f�[�^�X�V
                    GoTo STP_END                                        ' ����I�� 
                End If

                ' �ŏI�s�ȊO�̏ꍇ
                If (stJOG.CurrentNo <= 5) Then                          ' 5�s�ڈȍ~�̓O���b�h�����X�N���[��
                    Call Set_TopRow(1)                                  ' �O���b�h�̎w��s��擪�ɂ��� 
                Else
                    Call Set_TopRow(stJOG.CurrentNo - 5)                ' �O���b�h�̎w��s��擪�ɂ��� 
                End If

                ' �V�X�e���G���[�`�F�b�N
                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)         ' �V�X�e���G���[�`�F�b�N
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1)                   ' Wait(ms)

            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK(START)/RESET(Cancel)���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                If (r = cFRS_ERR_RST) Then                              ' RESET(Cancel)���݉����Ȃ�I���m�F���b�Z�[�W�\����
                    GoTo STP_ENDMSG
                End If
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W��\������
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                '�u�������v�I���Ȃ珈�����p������
                If (StepNum = 0) Then                                   ' StepNum = �R���\�[������(START/RESET�L�[���͑҂�) ?
                    GoTo STP_INPCONS                                    ' �R���\�[������(START/RESET�L�[���͑҂�)��
                ElseIf (StepNum = 1) Then                               ' StepNum = �p�^�[���F�����s ?
                    GoTo STP_PTNRECOG                                   ' �p�^�[���F�����s��
                Else
                    GoTo STP_MANUAL                                     ' �蓮�e�B�[�`���O������
                End If
            End If

            ' �I������
STP_END:
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                    ' �J�����ؑ�(�����J����)
#End If
            'Call End_GazouProc(ObjProc)                                 ' �摜�\���v���O�������I������
            Call Form1.VideoLibrary1.PatternDisp(False)                 ' �p�^�[���}�b�`���O���̌����͈͘g(���F�g�ƐF�g)���\���Ƃ���
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CutPosCorrect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[

        Finally
            Form1.Instance.VideoLibrary1.SetTrackBarEnabled(True)       'V6.0.0.0-21
        End Try
    End Function
#End Region

#Region "�蓮�e�B�[�`���O����"
    '''=========================================================================
    ''' <summary>�蓮�e�B�[�`���O����</summary>
    ''' <param name="stPLT">      (INP)�v���[�g�f�[�^</param>
    ''' <param name="BlkX">       (INP)�u���b�N�ԍ�X</param>
    ''' <param name="BlkY">       (INP)�u���b�N�ԍ�Y</param>
    ''' <param name="piCurrentNo">(INP)�������̍s�ԍ�</param>
    ''' <param name="EndNo">      (INP)�ŏI�s�ԍ�</param>
    ''' <param name="dblGapX">    (I/O)�����X</param>
    ''' <param name="dblGapY">    (I/O)�����Y</param>
    ''' <returns>cFRS_NORMAL    = ����
    '''          cFRS_ERR_RST   = Cancel(RESET��)
    '''          -1�ȉ�         = �G���[</returns>
    '''=========================================================================
    Private Function ManualTeach(ByRef stPLT As PlateInfo, ByVal BlkX As Integer, ByVal BlkY As Integer, ByRef piCurrentNo As Integer, ByVal EndNo As Integer, ByRef dblGapX() As Double, ByRef dblGapY() As Double) As Integer

        Dim mdBSx As Double                                                     ' ��ۯ�����X
        Dim mdBSy As Double                                                     ' ��ۯ�����Y
        Dim X As Double
        Dim Y As Double
        Dim POSX As Double
        Dim POSY As Double
        Dim r As Integer
        Dim rtn As Integer
        Dim strMSG As String

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            Call Reset_AllBackColor(iMaxDataNum)                        ' �O���b�h�̑S�s�̔w�i�F�����ɖ߂�
            Call Set_TopRow(stJOG.CurrentNo)                            ' �O���b�h�̎w��s��擪�ɂ��� 
            cmdOK.Visible = True                                        ' OK�{�^���\��
            Call CalcBlockSize(mdBSx, mdBSy)                            ' �u���b�N�T�C�YXY�ݒ�

            ' �����(JOG����)�p�p�����[�^������������
            'Call ZGETPHPOS(POSX, POSY)                                  ' X,Y�e�[�u���ʒu�擾 V1.14.0.0�F
            Call ZGETPHPOS2(POSX, POSY)                                  ' X,Y�e�[�u���ʒu�擾 V1.14.0.0�F
            stJOG.Md = MODE_STG                                         ' ���[�h(XY�e�[�u�����[�h)
            stJOG.Md2 = MD2_BUTN                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT   ' �L�[�̗L��(1)/����(0)�w��
            stJOG.PosX = POSX                                           ' �e�[�u��X���W
            stJOG.PosY = POSY                                           ' �e�[�u��Y���W
            stJOG.BpOffX = stPLT.dblBpOffSetXDir                        ' BP�̾��X 
            stJOG.BpOffY = stPLT.dblBpOffSetYDir                        ' BP�̾��Y 
            stJOG.BszX = mdBSx                                          ' ��ۯ�����X 
            stJOG.BszY = mdBSy                                          ' ��ۯ�����Y
            'V4.7.0.0�I            stJOG.TextX = fgrdPoints                                    ' �����X�\���p���x��(�O���b�h)
            stJOG.TextX = dgvPoints                                     ' �����X�\���p���x��(�O���b�h)
            'V6.0.0.0�J            stJOG.TextY = 0                                             ' �����Y�\���p���x��(���g�p)
            stJOG.TextY = Nothing                                       ' �����Y�\���p���x��(���g�p)   'V6.0.0.0�J
            stJOG.cgX = dblGapX(piCurrentNo)                            ' �ړ���X(�����X)
            stJOG.cgY = dblGapY(piCurrentNo)                            ' �ړ���Y(�����Y)
            stJOG.BtnHI = BtnHI                                         ' HI�{�^��
            stJOG.BtnZ = BtnZ                                           ' Z�{�^��
            stJOG.BtnSTART = BtnSTART                                   ' START�{�^��
            stJOG.BtnHALT = BtnHALT                                     ' HALT�{�^��
            stJOG.BtnRESET = BtnRESET                                   ' RESET�{�^��
            stJOG.CurrentNo = piCurrentNo                               ' �������̍s�ԍ�
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()                                             ' �t�H�[�J�X��ݒ肷��(�e���L�[���͗p) 

            '---------------------------------------------------------------------------
            '   �J�b�g�X�^�[�g�ʒu�e�B�[�`���O����
            '---------------------------------------------------------------------------
STP_TEACH:
            stJOG.Flg = -1
            Do
                Call Set_BackColor(stJOG.CurrentNo)                     ' �O���b�h�̎w��s�̔w�i�F�����F�ɂ��� 

                ' �e�[�u����Βl�ړ�(�O���J������)
                r = XYTableMoveBlockFirst(1, 1, 1, BlkX, BlkY, stJOG.CurrentNo)
                If (r <> cFRS_NORMAL) Then                              ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    GoTo STP_END
                End If
                Call System.Threading.Thread.Sleep(1000)                ' �e�[�u���ړ���摜����҂�����(ms)

                '-------------------------------------------------------------------
                '   JOG�����ʏ���
                '-------------------------------------------------------------------
                'Call ZGETPHPOS(POSX, POSY)                              ' X,Y�e�[�u���ʒu�擾 V1.14.0.0�F
                Call ZGETPHPOS2(POSX, POSY)                              ' X,Y�e�[�u���ʒu�擾 V1.14.0.0�F
                'V5.0.0.6�S ADD START��
                If dblGapX(stJOG.CurrentNo) <> 0.0 Or dblGapX(stJOG.CurrentNo) <> 0.0 Then
                    POSX = POSX + dblGapX(stJOG.CurrentNo)
                    POSY = POSY + dblGapY(stJOG.CurrentNo)
                    r = Form1.System1.XYtableMove(gSysPrm, POSX, POSY)
                    If (r <> cFRS_NORMAL) Then                      ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                        If (Form1.System1.IsSoftLimitXY(r) = False) Then
                            GoTo STP_END
                        End If
                    End If
                End If
                'V5.0.0.6�S ADD END��
                stJOG.PosX = POSX                                       ' �e�[�u��X���W
                stJOG.PosY = POSY                                       ' �e�[�u��Y���W
                stJOG.cgX = dblGapX(stJOG.CurrentNo)                    ' �ړ���X(�����X)
                stJOG.cgY = dblGapY(stJOG.CurrentNo)                    ' �ړ���Y(�����Y)
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    GoTo STP_END
                End If
                ' START�L�[�����ȊO�Ȃ猻�݂̃e�[�u���ʒu(BP�ʒu)���擾����
                If (r <> cFRS_ERR_START) Then
                    X = stJOG.cgX                                       ' �ړ���X(�����X)
                    Y = stJOG.cgY                                       ' �ړ���Y(�����Y)
                End If

                '-----------------------------------------------------------------------
                '   HALT�L�[����������(�P�O�Ɉړ�)
                '-----------------------------------------------------------------------
                If (r = cFRS_ERR_HALT) Then                             ' HALT�L�[���� ?
                    ' �擪�s�łȂ���ΑO�s��
                    If (stJOG.CurrentNo > 1) Then                       ' �擪�s�Ȃ�NOP
                        Call Reset_BackColor(stJOG.CurrentNo)           ' �O���b�h�̎w��s�̔w�i�F�����ɖ߂�
                        stJOG.CurrentNo = stJOG.CurrentNo - 1           ' �s�ԍ� - 1
                        If (stJOG.CurrentNo <= 5) Then
                            Call Set_TopRow(1)
                        Else
                            Call Set_TopRow(stJOG.CurrentNo - 5)
                        End If

                        ' �e�[�u����Βl�ړ�
                        X = pfStartPos(1, stJOG.CurrentNo)
                        Y = pfStartPos(2, stJOG.CurrentNo)
                        r = Form1.System1.XYtableMove(gSysPrm, POSX + X, POSY + Y)
                        If (r <> cFRS_NORMAL) Then                      ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                            If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                GoTo STP_END
                            End If
                        End If
                        Call System.Threading.Thread.Sleep(1000)        ' �e�[�u���ړ���摜����҂�����(ms)
                    End If

                    '-----------------------------------------------------------------------
                    '   RESET�L�[����������(è��ݸ�Cancel�I��)
                    '-----------------------------------------------------------------------
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET�L�[���� ?
                    Call LAMP_CTRL(LAMP_RESET, True)                    ' RESET�����vON
                    GoTo STP_ENDMSG                                     ' �I���m�F���b�Z�[�W�\����

                    '-----------------------------------------------------------------------
                    '  START�L�[����������(���s�ֈړ�)
                    '-----------------------------------------------------------------------
                ElseIf (r = cFRS_ERR_START) Then                        ' START�L�[���� ?
                    ' ���݂̃e�[�u���ʒu(BP�ʒu)���擾����
                    Call System.Threading.Thread.Sleep(1000)            ' Wait(ms)
                    dblGapX(stJOG.CurrentNo) = stJOG.cgX                ' �ړ���X(�����X)
                    dblGapY(stJOG.CurrentNo) = stJOG.cgY                ' �ړ���Y(�����Y)
                    X = dblGapX(stJOG.CurrentNo)
                    Y = dblGapY(stJOG.CurrentNo)
                    Call DispGapXY(stJOG.CurrentNo, X, Y)               'V4.7.0.0�I �����X,Y��\������
                    ' �ŏI�s ?
                    If (stJOG.CurrentNo + 1 > EndNo) Then
                        r = cFRS_NORMAL                                 ' �ŏI�s�Ȃ琳�탊�^�[�� 
                        GoTo STP_END

                        ' �ŏI�s�łȂ���Ύ��s��
                    Else
                        Call Reset_BackColor(stJOG.CurrentNo)           ' �O���b�h�̎w��s�̔w�i�F�����ɖ߂�
                        stJOG.CurrentNo = stJOG.CurrentNo + 1
                        If (stJOG.CurrentNo <= 5) Then
                            Call Set_TopRow(1)
                        Else
                            Call Set_TopRow(stJOG.CurrentNo - 5)
                        End If

                        X = dblGapX(stJOG.CurrentNo)
                        Y = dblGapY(stJOG.CurrentNo)

                        ' �e�[�u����Βl�ړ�(�ŏI��R)
                        'V5.0.0.6�S�R�����g��
                        'r = Form1.System1.XYtableMove(gSysPrm, POSX + X, POSY + Y)
                        'If (r <> cFRS_NORMAL) Then                      ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                        '    If (Form1.System1.IsSoftLimitXY(r) = False) Then
                        '        GoTo STP_END
                        '    End If
                        'End If
                        'V5.0.0.6�S�R�����g��
                        Call System.Threading.Thread.Sleep(1000)        ' �e�[�u���ړ���摜����҂�����(ms)
                    End If
                    Call ZCONRST()                                      ' ���b�`����
                    'V4.7.0.0�I Call DispGapXY(stJOG.CurrentNo, X, Y)               ' �����X,Y��\������

                End If

                ' ���b�Z�[�W�|���v
                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(ms)

                ' ����~���`�F�b�N
                r = Form1.System1.SysErrChk_ForVBNET(APP_MODE_TEACH)
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_END
                End If
            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK/RESET(Cancel)���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                stJOG.Flg = -1                                          ' �e��ʂׂ̈�FLG������������ 
                If (r = cFRS_NORMAL) Then                               ' OK�{�^�������Ȃ琳��I��
                    dblGapX(stJOG.CurrentNo) = stJOG.cgX                ' �ړ���X(�����X)
                    dblGapY(stJOG.CurrentNo) = stJOG.cgY                ' �ړ���Y(�����Y)
                    GoTo STP_END
                End If
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W��\������
            '-------------------------------------------------------------------
STP_ENDMSG:
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CUT_POS_CORRECT_START)
            If (rtn = cFRS_ERR_RST) Then                                '�u�������v�I���Ȃ�e�B�[�`���O�ɖ߂�
                GoTo STP_TEACH                                          ' �J�b�g�X�^�[�g�ʒu�e�B�[�`���O��
            End If

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
STP_END:
            cmdOK.Visible = False                                       ' OK�{�^����\��
            Call ZCONRST()                                              ' ���b�`����
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET�����vOFF
            Call LAMP_CTRL(LAMP_Z, False)                               ' PRB�����vOFF
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.ManualTeach() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�R���\�[������(JOG��ʂ̃L�[����(START/RESET��)�҂�)"
    '''=========================================================================
    ''' <summary>�R���\�[������(JOG��ʂ̃L�[����(START/RESET��)�҂�)</summary>
    ''' <param name="stJOG"> (INP)�����(JOG����)�p�p�����[�^</param>
    ''' <param name="KeyOpt">(INP)���͑҂�����L�[</param>
    ''' <returns>cFRS_ERR_START = START(OK)�L�[
    '''          cFRS_ERR_RST   = RESET(Cancel)�L�[
    '''          cFRS_ERR_HALT  = HALT�L�[
    '''          ��L�ȊO       = �G���[</returns>
    '''=========================================================================
    Private Function WaitJogStartResetKey(ByRef stJOG As JOG_PARAM, ByVal KeyOpt As UShort) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �R���\�[������(JOG��ʂ̃L�[���͑҂�)
            Call ZCONRST()                                              ' ���b�`����
            Call LAMP_CTRL(LAMP_START, True)                                    ' START�����vON
            Call LAMP_CTRL(LAMP_RESET, True)                                    ' RESET�����vON
            stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
            stJOG.Opt = KeyOpt                                          ' �L�[�̗L��(1)/����(0)�w��
            'V6.0.0.0�J            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then                                  ' RESET�L�[���� ?
                Call LAMP_CTRL(LAMP_RESET, True)                        ' RESET�����vON
                Return (r)                                              ' Return�l = RESET(Cancel)�L�[
            End If
            Return (r)                                                  ' Return�l = START(OK)�L�[��

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.WaitJogStartResetKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

    '========================================================================================
    '   �g���~���O�f�[�^�̓��o�͏���
    '========================================================================================
#Region "�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����"
    '''=========================================================================
    '''<summary>�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub GetTrimData()

        Dim i As Short
        Dim intChipNum As Short
        Dim intRegNum As Short
        Dim dblX As Double
        Dim dblY As Double
        Dim strMSG As String
        Dim bRtn As Boolean         'V5.0.0.6�I

        Try
            ' �����ϐ�������
            For i = 1 To MaxCntResist
                iResistorNumber(i) = 0                                  ' ��R�ԍ�
                pfStartPos(0, i) = 0.0                                  ' è��ݸ��߲��X
                pfStartPos(1, i) = 0.0                                  ' è��ݸ��߲��Y
            Next

            ' �e�B�[�`���O�e�[�u���Ƀe�[�u�����W��ݒ肷��
            iMaxDataNum = 0                                             ' �����Ώۃf�[�^�� = 0 
            'Call GetChipNum(intChipNum)                                 ' ��ٰ�ߓ���R���擾
            intChipNum = typPlateInfo.intResistCntInBlock               ' �u���b�N����R���擾
            For i = 1 To intChipNum                                     ' ��R�����J��Ԃ�
                If (GetRegNum(i, intRegNum)) Then                       ' i�Ԗڂ̒�R�ԍ��擾
                    If ((1 <= intRegNum) And (MARKING_RESNO_SET > intRegNum)) Then   ' ϰ�ݸނ͑ΏۊO
                        ' ��R�ԍ��̍ŏ��̃J�b�g�f�[�^�̃X�^�[�g�|�C���gX,Y���擾����
                        'V5.0.0.6�I            If (GetCutTeachPoint(intRegNum, 1, dblX, dblY)) Then
                        'V4.7.0.0�I             If (GetCutStartPoint(intRegNum, 1, dblX, dblY)) Then
                        'V5.0.0.6�I��
                        If giTeachpointUse = 0 Then                     ' KOA EW��
                            bRtn = GetCutTeachPoint(intRegNum, 1, dblX, dblY)
                        Else
                            bRtn = GetCutStartPoint(intRegNum, 1, dblX, dblY)
                        End If
                        'V5.0.0.6�I��
                        If bRtn Then
                            iMaxDataNum = iMaxDataNum + 1               ' �����Ώۃf�[�^��+1
                            iResistorNumber(iMaxDataNum) = intRegNum    ' ��R�ԍ�
                            pfStartPos(0, iMaxDataNum) = dblX           ' è��ݸ��߲��X
                            pfStartPos(1, iMaxDataNum) = dblY           ' è��ݸ��߲��Y

                            'V5.0.0.6�I���O���J�����J�b�g�ʒu�␳�ʂ̉��Z
                            pfStartPosTeachPoint(0, iMaxDataNum) = 0.0          ' ������
                            pfStartPosTeachPoint(1, iMaxDataNum) = 0.0
                            If giTeachpointUse = 1 Then
                                bRtn = GetCutTeachPoint(intRegNum, 1, dblX, dblY)
                                If bRtn Then    '�J�b�g�ʒu�␳�l���e�B�[�`���O�|�C���g�Ɋi�[�����ꍇ�őO�̕␳�l��ǂݏo���ĕۑ����Ă���
                                    pfStartPosTeachPoint(0, iMaxDataNum) = dblX         ' è��ݸ��߲��X
                                    pfStartPosTeachPoint(1, iMaxDataNum) = dblY         ' è��ݸ��߲��Y
                                Else
                                End If
                            End If
                            'V5.0.0.6�I��
                        End If
                    End If
                End If
            Next

            ' �\���p�O���b�h�̏�����
            Call GridInitialize()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.GetTrimData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�f�[�^�X�V(����ʂ��X�^�[�g�|�C���g�ɔ��f)"
    '''=========================================================================
    ''' <summary>�g���~���O�f�[�^�X�V(����ʂ�����߲�Ăɔ��f)</summary>
    ''' <param name="dblGapX">(INP)�����X</param>
    ''' <param name="dblGapY">(INP)�����Y</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub UpdateData(ByRef dblGapX() As Double, ByRef dblGapY() As Double)

        Dim intChipNum As Short
        Dim bRetc As Boolean = False
        Dim i As Short
        Dim j As Short
        Dim intRegNum As Short
        Dim intCutNum As Short
        Dim dblX As Double
        Dim dblY As Double
        Dim strMSG As String
        Dim dBpSizeX As Double, dBpSizeY As Double  'V4.7.0.0�I 
        Dim dGapX As Double, dGapY As Double, dStartX As Double, dStartY As Double, dTeachX As Double, dTeachY As Double       'V5.0.0.6�I

        Try
            'V4.7.0.0�I ADD START��
            If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                dBpSizeX = 60.0
                dBpSizeY = 60.0
            ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                dBpSizeX = 60.0
                dBpSizeY = 30.0
            ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then    'V4.3.0.0�A
                dBpSizeX = 90.0
                dBpSizeY = 90.0
            Else
                dBpSizeX = 80.0
                dBpSizeY = 80.0
            End If
            'V4.7.0.0�I ADD START��
            'Call GetChipNum(intChipNum)
            intChipNum = typPlateInfo.intResistCntInBlock               ' �u���b�N����R�� 
            For i = 1 To intChipNum                                     ' ��R������߰
                If (GetRegNum(i, intRegNum)) Then                       ' i�Ԗڂ̒�R�ԍ��擾
                    If ((1 <= intRegNum) And (999 >= intRegNum)) Then   ' ϰ�ݸނ͑ΏۊO
                        If (GetRegCutNum(intRegNum, intCutNum)) Then    ' ��Đ��擾
                            For j = 1 To intCutNum
                                'V5.0.0.6�I��
                                If giTeachpointUse = 1 Then             ' �␳�ʂ��e�B�[�`���O�|�C���g�Ɋi�[���鏈���̏ꍇ
                                    If (GetCutStartPoint(intRegNum, j, dStartX, dStartY)) Then ' ��R�ԍ��̍ŏ��̶���ް�
                                        If (GetCutTeachPoint(intRegNum, j, dTeachX, dTeachY)) Then
                                            dGapX = dblGapX(i) * -1.0 + dTeachX
                                            dGapY = dblGapY(i) * -1.0 + dTeachY
                                            dblX = dStartX + dGapX            ' �X�^�[�g�|�C���g = �J�b�g�ʒu + �����X
                                            dblY = dStartY + dGapY
                                        Else
                                            MsgBox("FrmCutPosCorrect.UpdateData.GetCutTeachPoint ERROR REG=[" + intRegNum.ToString() + "] CUT=[" + j.ToString() + "]")
                                        End If

                                        If (-dBpSizeX > dblX) Then
                                            dblX = -dBpSizeX  ' �����Ă���ꍇ�͍ő�l�A�ŏ��l�ɕ␳
                                            dGapX = dblX - dStartX
                                            bRetc = True
                                        End If
                                        If (dBpSizeX < dblX) Then
                                            dblX = dBpSizeX
                                            dGapX = dblX - dStartX
                                            bRetc = True
                                        End If
                                        If (-dBpSizeY > dblY) Then
                                            dblY = -dBpSizeY
                                            dGapY = dblY - dStartY
                                            bRetc = True
                                        End If
                                        If (dBpSizeY < dblY) Then
                                            dblY = dBpSizeY
                                            dGapY = dblY - dStartY
                                            bRetc = True
                                        End If
                                        ' �X�^�[�g�|�C���g������ʂōX�V����
                                        Call SetCutTeachPoint(intRegNum, j, dGapX, dGapY)
                                        Console.WriteLine("S " & intRegNum & " " & j & " " & dGapX & " " & dGapY)
                                    Else
                                        MsgBox("FrmCutPosCorrect.UpdateData.GetCutStartPoint ERROR REG=[" + intRegNum.ToString() + "] CUT=[" + j.ToString() + "]")
                                    End If
                                Else
                                    'V5.0.0.6�I��
                                    'If (GetCutTeachPoint(intRegNum, j, dblX, dblY)) Then ' ��R�ԍ��̍ŏ��̶���ް�
                                    If (GetCutStartPoint(intRegNum, j, dblX, dblY)) Then
                                        'V4.7.0.0�I                                If (GetCutStartPoint(intRegNum, j, dblX, dblY)) Then
                                        dblX = dblX + dblGapX(i)            ' �X�^�[�g�|�C���g = �J�b�g�ʒu + �����X
                                        dblY = dblY + dblGapY(i)
                                        dblX = RoundOff(dblX)               ' �l�̌ܓ�
                                        dblY = RoundOff(dblY)

                                        ' �����������
                                        'V4.7.0.0�I ADD START��
                                        If (-dBpSizeX > dblX) Then
                                            dblX = -dBpSizeX  ' �����Ă���ꍇ�͍ő�l�A�ŏ��l�ɕ␳
                                            bRetc = True
                                        End If
                                        If (dBpSizeX < dblX) Then
                                            dblX = dBpSizeX
                                            bRetc = True
                                        End If
                                        If (-dBpSizeY > dblY) Then
                                            dblY = -dBpSizeY
                                            bRetc = True
                                        End If
                                        If (dBpSizeY < dblY) Then
                                            dblY = dBpSizeY
                                            bRetc = True
                                        End If
                                        'V4.7.0.0�I �R�����g START��

                                        'V4.7.0.0�I ADD START��
                                        'If (-gSysPrm.stDEV.giBpSize > dblX) Then
                                        '    dblX = -gSysPrm.stDEV.giBpSize  ' �����Ă���ꍇ�͍ő�l�A�ŏ��l�ɕ␳
                                        '    bRetc = True
                                        'End If
                                        'If (gSysPrm.stDEV.giBpSize < dblX) Then
                                        '    dblX = gSysPrm.stDEV.giBpSize
                                        '    bRetc = True
                                        'End If
                                        'If (-gSysPrm.stDEV.giBpSize > dblY) Then
                                        '    dblY = -gSysPrm.stDEV.giBpSize
                                        '    bRetc = True
                                        'End If
                                        'If (gSysPrm.stDEV.giBpSize < dblY) Then
                                        '    dblY = gSysPrm.stDEV.giBpSize
                                        '    bRetc = True
                                        'End If
                                        'V4.7.0.0�I �R�����g START��

                                        ' �X�^�[�g�|�C���g���X�V����
                                        Call SetCutStartPoint(intRegNum, j, dblX, dblY)
                                        Console.WriteLine("S " & intRegNum & " " & j & " " & dblX & " " & dblY)
                                    End If
                                    'V4.7.0.0�I ADD START��
                                    If (GetCutTeachPoint(intRegNum, j, dblX, dblY)) Then ' ��R�ԍ��̍ŏ��̶���ް�
                                        'V4.7.0.0�I                                If (GetCutStartPoint(intRegNum, j, dblX, dblY)) Then
                                        dblX = dblX + dblGapX(i)            ' �X�^�[�g�|�C���g = �J�b�g�ʒu + �����X
                                        dblY = dblY + dblGapY(i)
                                        dblX = RoundOff(dblX)               ' �l�̌ܓ�
                                        dblY = RoundOff(dblY)

                                        If (-dBpSizeX > dblX) Then
                                            dblX = -dBpSizeX  ' �����Ă���ꍇ�͍ő�l�A�ŏ��l�ɕ␳
                                            bRetc = True
                                        End If
                                        If (dBpSizeX < dblX) Then
                                            dblX = dBpSizeX
                                            bRetc = True
                                        End If
                                        If (-dBpSizeY > dblY) Then
                                            dblY = -dBpSizeY
                                            bRetc = True
                                        End If
                                        If (dBpSizeY < dblY) Then
                                            dblY = dBpSizeY
                                            bRetc = True
                                        End If
                                        ' �X�^�[�g�|�C���g���X�V����
                                        Call SetCutTeachPoint(intRegNum, j, dblX, dblY)
                                        Console.WriteLine("S " & intRegNum & " " & j & " " & dblX & " " & dblY)
                                    End If
                                    'V4.7.0.0�I ADD START��
                                End If  'V5.0.0.6�I
                            Next
                        End If
                    End If
                End If
            Next

            If (bRetc) Then
                ' "�␳�l�X�V�G���[" & vbCrLf & "�X�V���ʂ��͈͂𒴂��Ă��܂��B" & vbCrLf & "�ő�l�A�ŏ��l�ɕ␳����܂��B"
                MsgBox(MSG_CUT_POS_CORRECT_010, MsgBoxStyle.OkOnly)
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.UpdateData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   �u���b�N�ԍ�����
    '========================================================================================
#Region "�u���b�N�ԍ��e�L�X�g�{�b�N�X�̊�����/�񊈐���"
    '''=========================================================================
    ''' <summary>�u���b�N�ԍ��e�L�X�g�{�b�N�X�̊�����/�񊈐���</summary>
    ''' <param name="mode">(INP)True=������, False=�񊈐���</param>
    '''=========================================================================
    Private Sub SetTxtBlkNumBackColor(ByVal mode As Boolean)

        If (mode = True) Then
            txtBlockNoX.Enabled = True
            txtBlockNoX.BackColor = System.Drawing.Color.Yellow
            txtBlockNoY.Enabled = True
            txtBlockNoY.BackColor = System.Drawing.Color.Yellow
        Else
            txtBlockNoX.Enabled = False
            txtBlockNoX.BackColor = System.Drawing.SystemColors.Control
            txtBlockNoY.Enabled = False
            txtBlockNoY.BackColor = System.Drawing.SystemColors.Control
        End If

    End Sub
#End Region

#Region "�u���b�N�ԍ�(X��)�ύX��"
    '''=========================================================================
    '''<summary>�u���b�N�ԍ�(X��)�ύX��</summary>
    '''<remarks>txtBlockNoX.TextChanged �́A�t�H�[�������������ꂽ�Ƃ��ɔ������܂��B</remarks>
    '''=========================================================================
    Private Sub txtBlockNoX_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBlockNoX.TextChanged

        If (Not IsNumeric(Me.txtBlockNoX.Text)) Then                    ' �����ȊO�Ȃ�
            Me.txtBlockNoX.Text = "1"
        End If

    End Sub
#End Region

#Region "�u���b�N�ԍ�(Y��)�ύX��"
    '''=========================================================================
    '''<summary>�u���b�N�ԍ�(Y��)�ύX��</summary>
    '''<remarks>txtBlockNoY.TextChanged �́A�t�H�[�������������ꂽ�Ƃ��ɔ������܂��B</remarks>
    '''=========================================================================
    Private Sub txtBlockNoY_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtBlockNoY.TextChanged

        If (Not IsNumeric(Me.txtBlockNoX.Text)) Then                    ' �����ȊO�Ȃ�
            Me.txtBlockNoX.Text = "1"
        End If

    End Sub
#End Region

#Region "�u���b�N�ԍ�����"
    '''=========================================================================
    ''' <summary>�u���b�N�ԍ�����</summary>
    ''' <param name="BlkNumX">(OUT)�u���b�N�ԍ�X</param>
    ''' <param name="BlkNumY">(OUT)�u���b�N�ԍ�Y</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function InputBlkNum(ByRef BlkNumX As Integer, ByRef BlkNumY As Integer) As Integer

        Dim Blk As Integer
        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' �u���b�N�ԍ��͈̓`�F�b�N
            Idx = 0
            Blk = CInt(txtBlockNoX.Text)
            If (Blk < 1) Or (Blk > typPlateInfo.intBlockCntXDir) Then GoTo STP_ERR
            'V1.25.0.0�K'V5.0.0.6�J��
            If iExcamCutBlockNo_X > 0 Then
                If (Blk < 1) Or (Blk > iExcamCutBlockNo_X) Then GoTo STP_ERR
            End If
            'V1.25.0.0�K'V5.0.0.6�J��
            Idx = 1
            Blk = CInt(txtBlockNoY.Text)
            If (Blk < 1) Or (Blk > typPlateInfo.intBlockCntYDir) Then GoTo STP_ERR

            ' �u���b�N�ԍ���Ԃ�
            BlkNumX = CInt(txtBlockNoX.Text)
            BlkNumY = CInt(txtBlockNoY.Text)
            Return (cFRS_NORMAL)

STP_ERR:
            ' "�u���b�N�ԍ����̓G���["
            'Call Form1.System1.TrmMsgBox(gSysPrm, MSG_CUT_POS_CORRECT_015, vbOKOnly, FRM_CUT_POS_CORRECT_TITLE)
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_CUT_POS_CORRECT_015, vbOKOnly, Me.frmTitle.Text)
            If (Idx = 0) Then
                txtBlockNoX.Focus()
            Else
                txtBlockNoY.Focus()
            End If
            Return (-1)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.InputBlkNum() TRAP ERROR = " + ex.Message
            GoTo STP_ERR
        End Try
    End Function
#End Region

#Region "�X�e�b�v�C���^�[�o��(X�܂���Y����)�����߂�"
    '''=========================================================================
    ''' <summary>�X�e�b�v�C���^�[�o��(X�܂���Y����)�����߂�</summary>
    ''' <param name="stPLT"> (INP)�v���[�g�f�[�^</param>
    ''' <param name="stSTP"> (INP)�X�e�b�v�f�[�^</param>
    ''' <param name="BlkNum">(INP)�u���b�N�ԍ�XorY</param>
    ''' <param name="Intval">(OUT)�X�e�b�v�C���^�[�o��</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function CalcStepIntervel(ByRef stPLT As PlateInfo, ByRef stSTP() As StepInfo, ByVal BlkNum As Integer, ByRef Intval As Double) As Double

        Dim intStpMax As Integer
        Dim intBMax As Integer
        Dim IDX As Integer
        Dim strMSG As String

        Try
            ' �O���[�v��(�X�e�b�v��)�擾
            If (stPLT.intResistDir = 0) Then                                    ' ��R���ѕ��� = X����
                intStpMax = stPLT.intGroupCntInBlockXBp                         ' �u���b�N��BP�O���[�v��(X�����j
            Else
                intStpMax = stPLT.intGroupCntInBlockYStage                      ' �u���b�N��Stage�O���[�v��(Y�����j
            End If

            ' �X�e�b�v�C���^�[�o��(X�܂���Y����)�����߂�
            Intval = 0
            intBMax = 0
            For IDX = 1 To intStpMax - 1
                intBMax = intBMax + stSTP(IDX).intSP2                           ' �u���b�N��
                If (BlkNum <= intBMax) Then
                    Exit For
                End If
                Intval = Intval + stSTP(IDX).dblSP3                             ' �X�e�b�v�ԃC���^�[�o��
            Next IDX
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.CalcStepIntervel() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�w��u���b�N�̒�R�擪�e�B�[�`���O�ʒu�փe�[�u�����ړ�����"
    '''=========================================================================
    '''<summary>�w��u���b�N�̒�R�擪�e�B�[�`���O�ʒu�փe�[�u�����ړ�����</summary>
    '''<param name="intCamera">(INP)��׎��(0:������� 1:�O�����)</param>
    '''<param name="iXPlate">(INP)XPlateNo</param> 
    '''<param name="iYPlate">(INP)YPlateNo</param>  
    '''<param name="iXBlock">(INP)XBlockNo</param> 
    '''<param name="iYBlock">(INP)YBlockNo</param> 
    '''<param name="iRegNo"> (INP)��R�ԍ�</param>   
    '''<remarks>BP Offset �� ��Ĉʒu�␳ð��ٵ̾�Ă��l��</remarks>
    '''=========================================================================
    Private Function XYTableMoveBlockFirst(ByRef intCamera As Short, ByRef iXPlate As Short, ByRef iYPlate As Short, ByRef iXBlock As Short, ByRef iYBlock As Short, ByRef iRegNo As Short) As Integer

        Dim r As Integer
        Dim dblX As Double
        Dim dblY As Double
        Dim dblRotX As Double
        Dim dblRotY As Double
        Dim dblPSX As Double
        Dim dblPSY As Double
        Dim dblBsoX As Double
        Dim dblBsoY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim intCDir As Short
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim dblTOffsX As Double
        Dim dblTOffsY As Double
        Dim dblStepInterval As Double
        Dim dblExtTablePosOffsX As Double
        Dim dblExtTablePosOffsY As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim dblBpOffsetX As Double
        Dim dblBpOffsetY As Double
        Dim dblCutPosCorrectOffsetX As Double
        Dim dblCutPosCorrectOffsetY As Double
        Dim strMSG As String

        Try
            dblRotX = 0
            dblRotY = 0

            ' ����߼޼��X,Y�擾
            dblTrimPosX = gSysPrm.stDEV.gfTrimX
            dblTrimPosY = gSysPrm.stDEV.gfTrimY

            ' ð��وʒu�̾��X,Y�̎擾
            dblTOffsX = typPlateInfo.dblTableOffsetXDir
            dblTOffsY = typPlateInfo.dblTableOffsetYDir

            Call CalcBlockSize(dblBSX, dblBSY) ' ��ۯ����ގZ�o

            ' BP�ʒu�̾��X,Y�ݒ�
            dblBpOffsetX = typPlateInfo.dblBpOffSetXDir
            dblBpOffsetY = typPlateInfo.dblBpOffSetYDir
            ' ��Ĉʒu�␳�̾��X,Y
            dblCutPosCorrectOffsetX = typPlateInfo.dblCutPosiReviseOffsetXDir
            dblCutPosCorrectOffsetY = typPlateInfo.dblCutPosiReviseOffsetYDir

            ' ��ۯ����޵̾�ĎZ�o(��ۯ�����/2 ��ۯ��̏ی���XY�Ƃ���1 ð��ق̏ی���1)
            dblBsoX = (dblBSX / 2) * 1 * 1                  ' Table.BDirX * Table.dir
            dblBsoY = (dblBSY / 2) * 1                      ' Table.BDirY;

            ' �ƕ␳��ѵ̾��X,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
            ' 0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
            ' ���ݸވʒu���W (+or-) ��]���a + ð��ٵ̾�� (+or-) ��ۯ����޵̾�� + ð��ٕ␳��
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 1 ' x��, y��
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 2 ' x��, y��
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

                Case 3 ' x��, y��
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y
            End Select

            If (1 = intCamera) Then                         ' �O����׈ʒu���Z ?
                dblExtTablePosOffsX = gSysPrm.stDEV.gfExCmX ' Externla Camera Offset X(mm)
                dblExtTablePosOffsY = gSysPrm.stDEV.gfExCmY ' Externla Camera Offset Y(mm)
                dblX = dblX + dblExtTablePosOffsX
                dblY = dblY + dblExtTablePosOffsY
            End If

            ' �ï�ߊԊu�̎Z�o
            intCDir = typPlateInfo.intResistDir               ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
            If intCDir = 0 Then ' X����
                dblStepInterval = CalcStepInterval(iYBlock) ' �ï�߲�����َZ�o(Y��)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' ð���Y�������]�Ȃ�
                    dblY = dblY + dblStepInterval
                Else ' ð���Y�������]
                    dblY = dblY - dblStepInterval
                End If
            Else ' Y����
                dblStepInterval = CalcStepInterval(iXBlock) ' �ï�߲�����َZ�o(X��)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' ð���X�������]�Ȃ�
                    dblX = dblX + dblStepInterval
                Else ' ð���X�������]
                    dblX = dblX - dblStepInterval
                End If
            End If

            ' ��ڰ�/��ۯ��ʒu�̑��΍��W�v�Z
            dblPSX = 0.0 : dblPSY = 0.0                         ' ��ڰĻ��ގ擾(0�Œ�)

            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX + pfStartPos(0, iRegNo) + dblBpOffsetX + dblCutPosCorrectOffsetX - (dblBSX / 2)
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY + pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY - (dblBSY / 2)

                Case 1 ' x��, y��
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX - pfStartPos(0, iRegNo) - dblBpOffsetX - dblCutPosCorrectOffsetX + (dblBSX / 2)
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY + pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY - (dblBSY / 2)

                Case 2 ' x��, y��
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX + pfStartPos(0, iRegNo) + dblBpOffsetX + dblCutPosCorrectOffsetX - (dblBSX / 2)
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY - pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY + (dblBSY / 2)

                Case 3 ' x��, y��
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblX = dblX - pfStartPos(0, iRegNo) + dblBpOffsetX + dblCutPosCorrectOffsetX + (dblBSX / 2)
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))
                    dblY = dblY - pfStartPos(1, iRegNo) + dblBpOffsetY + dblCutPosCorrectOffsetY + (dblBSY / 2)
            End Select

            ' �w����ڰ�/��ۯ��ʒu�Ɉړ�
            r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.XYTableMoveBlockFirst() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

    '========================================================================================
    '   �O���b�h����
    '========================================================================================
#Region "�X�^�[�g�|�C���g�O���b�h�̏�����"
    '''=========================================================================
    '''<summary>�X�^�[�g�|�C���g�O���b�h�̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub GridInitialize()

        Dim strHead(6) As String
        Dim i As Short
        Dim intChipNum As Short
        Dim strMSG As String

        Try

            'Call GetChipNum(intChipNum)                                 ' ��ٰ�ߓ���R��
            intChipNum = typPlateInfo.intResistCntInBlock               ' �u���b�N����R�� 
            strHead(0) = ""
            strHead(1) = "R No"
            strHead(2) = FrmCutPosCorrect_001
            strHead(3) = FrmCutPosCorrect_002
            strHead(4) = FrmCutPosCorrect_003
            strHead(5) = FrmCutPosCorrect_004
            'If (0 = gSysPrm.stTMN.giMsgTyp) Then
            '    strHead(2) = "è��ݸ�X"
            '    strHead(3) = "è��ݸ�Y"
            '    strHead(4) = "�����X "
            '    strHead(5) = "�����Y "
            '    'Me.cmdBlockMove.Text = "�u���b�N�ړ�"
            'Else
            '    strHead(2) = "Teaching X"
            '    strHead(3) = "Teaching Y"
            '    strHead(4) = " Gap X  "
            '    strHead(5) = " Gap Y  "
            '    'Me.cmdBlockMove.Text = "Block Move"
            'End If

            'For i = 0 To strHead.Length - 1 Step 1
            For i = 0 To 4 Step 1
                dgvPoints.Columns(i).HeaderText = strHead(i + 1)    ' V4.0.0.0�G
            Next i

            For i = 1 To 5                                              ' ���o���쐬
                strHead(0) = strHead(0) & strHead(i)
                If (i <> 5) Then
                    strHead(0) = strHead(0) & "|"
                End If
            Next

            'UPGRADE_ISSUE: �萔 vbTwips �̓A�b�v�O���[�h����܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' ���N���b�N���Ă��������B
            'UPGRADE_ISSUE: Form �v���p�e�B CutPosCorrect.ScaleMode �̓T�|�[�g����܂���B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="8027179A-CB3B-45C0-9863-FAA1AF983B59"' ���N���b�N���Ă��������B
            'Me.ScaleMode = vbTwips
            'V4.7.0.0�I �R�����g�A�E�g�J�n��
            'With Me.fgrdPoints
            '    .FormatString = strHead(0)

            '    .Rows = intChipNum + 1                                  ' ��د�ލs��(+1:���ٍs)
            '    .Row = 1                                                ' ���وʒu
            '    .Col = 2
            '    'UPGRADE_ISSUE: VBControlExtender ���\�b�h fgrdPoints.FillStyle �̓A�b�v�O���[�h����܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' ���N���b�N���Ă��������B
            '    .FillStyle = MSFlexGridLib.FillStyleSettings.flexFillSingle
            '    For i = 0 To 4                                          ' �ް������͉E��
            '        .set_ColAlignment(i, MSFlexGridLib.AlignmentSettings.flexAlignRightCenter)
            '    Next

            '    .Row = 0
            '    For i = 0 To 4                                          ' ���ٕ����̂ݒ����悹
            '        .Col = i
            '        .CellAlignment = MSFlexGridLib.AlignmentSettings.flexAlignCenterCenter
            '    Next

            '    For i = 1 To intChipNum
            '        If (1 <= CInt(iResistorNumber(i))) AndAlso (MARKING_RESNO_SET > CInt(iResistorNumber(i))) Then
            '            .set_TextMatrix(i, 0, CShort(iResistorNumber(i)))               ' ��R�ԍ�
            '            .set_TextMatrix(i, 1, pfStartPos(0, i).ToString("##0.0000"))    ' è��ݸ��߲��X
            '            .set_TextMatrix(i, 2, pfStartPos(1, i).ToString("##0.0000"))    ' è��ݸ��߲��Y
            '            .set_TextMatrix(i, 3, "")                                       ' �����X
            '            .set_TextMatrix(i, 4, "")                                       ' �����Y
            '        End If
            '    Next
            'End With
            'V4.7.0.0�I �R�����g�A�E�g�I����

            With dgvPoints              ' V4.0.0.0�G
                .SuspendLayout()
                .Rows.Add(intChipNum)
                For i = 1 To intChipNum Step 1
                    With .Rows(i - 1)
                        If (1 <= CInt(iResistorNumber(i))) AndAlso _
                            (MARKING_RESNO_SET > CInt(iResistorNumber(i))) Then
                            .Cells(0).Value = iResistorNumber(i)                      ' ��R�ԍ�
                            .Cells(1).Value = pfStartPos(0, i).ToString("##0.0000")   ' è��ݸ��߲��X
                            .Cells(2).Value = pfStartPos(1, i).ToString("##0.0000")   ' è��ݸ��߲��Y
                        End If
                    End With
                Next i
                .ResumeLayout()
            End With

            Call Set_TopRow(1)                                          ' �O���b�h��1�s�ڂ�擪�ɂ��� 

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.GridInitialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�|�C���g�O���b�h�̎w��s��擪�ɂ���"
    '''=========================================================================
    '''<summary>�����߲�ĸ�د�ނ̎w��s��擪�ɂ���</summary>
    '''<param name="iRow">(INP)�s�ԍ�(1-)</param>
    '''=========================================================================
    Private Sub Set_TopRow(ByVal iRow As Integer)

        Dim strMSG As String

        Try
            'V4.7.0.0�I Me.fgrdPoints.TopRow = iRow

            dgvPoints.FirstDisplayedScrollingRowIndex = (iRow - 1) ' V4.0.0.0�G

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Set_TopRow() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�����F�ɂ���"
    '''=========================================================================
    '''<summary>�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�����F�ɂ���</summary>
    '''<param name="iRow">(INP)�s�ԍ�(1-)</param>
    '''=========================================================================
    Private Sub Set_BackColor(ByVal iRow As Integer)

        Dim strMSG As String

        Try
            'V4.7.0.0�I �R�����g�A�E�g�J�n��
            'With Me.fgrdPoints
            '    ' �w�i�F�����F�ɐݒ肷��
            '    .Row = iRow                                                 ' Row = �w��s 
            '    .Col = 1 : .CellBackColor = System.Drawing.Color.Yellow
            '    .Col = 2 : .CellBackColor = System.Drawing.Color.Yellow
            '    .Col = 3 : .CellBackColor = System.Drawing.Color.Yellow
            '    .Col = 4 : .CellBackColor = System.Drawing.Color.Yellow
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0�I �R�����g�A�E�g�I����

            With dgvPoints
                .SuspendLayout()
                With .Rows(iRow - 1)   ' V4.0.0.0�G
                    ' �w�i�F�����F�ɐݒ肷��
                    .Cells(1).Style.BackColor = Color.Yellow
                    .Cells(2).Style.BackColor = Color.Yellow
                    .Cells(3).Style.BackColor = Color.Yellow
                    .Cells(4).Style.BackColor = Color.Yellow
                End With
                .ResumeLayout()
                .Refresh()
            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Set_BackColor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�|�C���g�O���b�h�̑S�s�̔w�i�F�����ɖ߂�"
    '''=========================================================================
    '''<summary>�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�F�����ɖ߂�</summary>
    '''<param name="DatCount">(INP)�f�[�^��</param>
    '''=========================================================================
    Private Sub Reset_AllBackColor(ByVal DatCount As Integer)

        'V4.7.0.0�I        Dim Count As Integer
        Dim strMSG As String

        Try
            'V4.7.0.0�I �R�����g�A�E�g�J�n��
            'With Me.fgrdPoints
            '    For Count = 1 To DatCount
            '        .Row = Count                                        ' Row = �w��s
            '        .Col = 1 : .CellBackColor = System.Drawing.Color.White
            '        .Col = 2 : .CellBackColor = System.Drawing.Color.White
            '        .Col = 3 : .CellBackColor = System.Drawing.Color.White
            '        .Col = 4 : .CellBackColor = System.Drawing.Color.White
            '    Next Count
            'End With
            'V4.7.0.0�I �R�����g�A�E�g�I����

            With dgvPoints              ' V4.0.0.0�G
                .SuspendLayout()
                For i As Integer = 0 To (.Rows.Count - 1) Step 1
                    .Rows(i).Cells(1).Style.BackColor = SystemColors.Window
                    .Rows(i).Cells(2).Style.BackColor = SystemColors.Window
                    .Rows(i).Cells(3).Style.BackColor = SystemColors.Window
                    .Rows(i).Cells(4).Style.BackColor = SystemColors.Window
                Next i
                .ResumeLayout()
                .Refresh()
            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Reset_AllBackColor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�F�����ɖ߂�"
    '''=========================================================================
    '''<summary>�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�F�����ɖ߂�</summary>
    '''<param name="iRow">(INP)�s�ԍ�(1-)</param>
    '''=========================================================================
    Private Sub Reset_BackColor(ByVal iRow As Integer)

        Dim strMSG As String

        Try
            'V4.7.0.0�I �R�����g�A�E�g�J�n��
            'With Me.fgrdPoints
            '    .Row = iRow                                             ' Row = �w��s
            '    .Col = 1 : .CellBackColor = System.Drawing.Color.White
            '    .Col = 2 : .CellBackColor = System.Drawing.Color.White
            '    .Col = 3 : .CellBackColor = System.Drawing.Color.White
            '    .Col = 4 : .CellBackColor = System.Drawing.Color.White
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0�I �R�����g�A�E�g�I����

            With dgvPoints              ' V4.0.0.0�G
                .SuspendLayout()
                With .Rows(iRow - 1)
                    .Cells(1).Style.BackColor = SystemColors.Window
                    .Cells(2).Style.BackColor = SystemColors.Window
                    .Cells(3).Style.BackColor = SystemColors.Window
                    .Cells(4).Style.BackColor = SystemColors.Window
                End With
                .ResumeLayout()
                .Refresh()
            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.Reset_BackColor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�|�C���g�O���b�h�̂����X,Y�\��������������"
    '''=========================================================================
    '''<summary>�X�^�[�g�|�C���g�O���b�h�̂����X,Y�\��������������</summary>
    '''<param name="num">(INP)�Ώۃf�[�^��(1-)</param>
    '''=========================================================================
    Private Sub DispGapXYInit(ByVal num As Integer)

        'V4.7.0.0�I        Dim Idx As Integer
        Dim strMSG As String

        Try
            'V4.7.0.0�I �R�����g�A�E�g�J�n��
            'With Me.fgrdPoints
            '    For Idx = 1 To num
            '        .set_TextMatrix(Idx, 3, "")                         ' �����X
            '        .set_TextMatrix(Idx, 4, "")                         ' �����Y
            '    Next Idx
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0�I �R�����g�A�E�g�I����

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.DispGapXYInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�|�C���g�O���b�h�̎w��s�ɂ����X,Y��\������"
    '''=========================================================================
    '''<summary>�X�^�[�g�|�C���g�O���b�h�̎w��s�ɂ����X,Y��\������</summary>
    '''<param name="row">(INP)�s�ԍ�(1-)</param>
    ''' <param name="X"> (INP)�����X</param>
    ''' <param name="Y"> (INP)�����Y</param>
    '''=========================================================================
    Private Sub DispGapXY(ByVal row As Integer, ByVal X As Double, ByVal Y As Double)

        Dim strMSG As String

        Try
            'V4.7.0.0�I �R�����g�A�E�g�J�n��
            'With Me.fgrdPoints
            '    .set_TextMatrix(row, 3, X.ToString("##0.0000"))                     ' �����X
            '    .set_TextMatrix(row, 4, Y.ToString("##0.0000"))                     ' �����Y
            'End With
            'Me.fgrdPoints.Refresh()
            'V4.7.0.0�I �R�����g�A�E�g�I����

            With dgvPoints.Rows(row - 1)
                .Cells(3).Value = X.ToString("##0.0000")                ' �����X
                .Cells(4).Value = Y.ToString("##0.0000")                ' �����Y
            End With
            dgvPoints.Refresh()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCutPosCorrect.DispGapXY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "DataGridView���I����Ԃ̐F�ɂȂ�̂��L�����Z������"
    '''=========================================================================
    ''' <summary>DataGridView���I����Ԃ̐F�ɂȂ�̂��L�����Z������</summary>
    '''=========================================================================
    Private Sub dgvPoints_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvPoints.SelectionChanged
        dgvPoints.Rows(dgvPoints.CurrentRow.Index).Selected = False
    End Sub
#End Region

    '========================================================================================
    '   �{�^������������(�i�n�f������)
    '========================================================================================
#Region "HALT�{�^������������"
    '''=========================================================================
    '''<summary>HALT�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHALT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHALT.Click
        Call SubBtnHALT_Click()
    End Sub
#End Region

#Region "START�{�^������������"
    '''=========================================================================
    '''<summary>START�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSTART.Click
        Call SubBtnSTART_Click()
    End Sub
#End Region

#Region "RESET�{�^������������"
    '''=========================================================================
    '''<summary>RESET�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        Call SubBtnRESET_Click()
    End Sub
#End Region

#Region "Z�{�^������������"
    '''=========================================================================
    '''<summary>RESET�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnZ.Click
        Call SubBtnZ_Click(stJOG)
    End Sub
#End Region

#Region "HI�{�^������������"
    '''=========================================================================
    '''<summary>HI�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHI.Click
        Call SubBtnHI_Click(stJOG)
    End Sub
#End Region

#Region "���{�^��������"
    '''=========================================================================
    '''<summary>���{�^��������</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnJOG_0_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseDown
        Call SubBtnJOG_0_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +Y ON
    End Sub
    Private Sub BtnJOG_0_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseUp
        Call SubBtnJOG_0_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +Y OFF
    End Sub

    Private Sub BtnJOG_1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseDown
        Call SubBtnJOG_1_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -Y ON
    End Sub
    Private Sub BtnJOG_1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseUp
        Call SubBtnJOG_1_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -Y OFF
    End Sub

    Private Sub BtnJOG_2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseDown
        Call SubBtnJOG_2_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X ON
    End Sub
    Private Sub BtnJOG_2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseUp
        Call SubBtnJOG_2_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X OFF
    End Sub

    Private Sub BtnJOG_3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseDown
        Call SubBtnJOG_3_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X ON
    End Sub
    Private Sub BtnJOG_3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseUp
        Call SubBtnJOG_3_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X OFF
    End Sub

    Private Sub BtnJOG_4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseDown
        Call SubBtnJOG_4_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X -Y ON
    End Sub
    Private Sub BtnJOG_4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseUp
        Call SubBtnJOG_4_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X -Y OFF
    End Sub

    Private Sub BtnJOG_5_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseDown
        Call SubBtnJOG_5_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X -Y ON
    End Sub
    Private Sub BtnJOG_5_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseUp
        Call SubBtnJOG_5_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X -Y OFF
    End Sub

    Private Sub BtnJOG_6_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseDown
        Call SubBtnJOG_6_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X +Y ON
    End Sub
    Private Sub BtnJOG_6_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseUp
        Call SubBtnJOG_6_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
    End Sub

    Private Sub BtnJOG_7_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseDown
        Call SubBtnJOG_7_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X +Y ON
    End Sub
    Private Sub BtnJOG_7_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseUp
        Call SubBtnJOG_7_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X +Y OFF
    End Sub
#End Region

    '========================================================================================
    '   �e���L�[���͏���(�i�n�f������)
    '========================================================================================
#Region "�L�[�_�E��������"
    '''=========================================================================
    '''<summary>�L�[�_�E��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmCutPosCorrect_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J
        Console.WriteLine("FrmCutPosCorrect.FrmCutPosCorrect_KeyDown()")

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V6.0.0.0�K        Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0�K
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5�� (KeyCode = 101(&H65)
            'V6.0.0.0�J            Call BtnHI_Click(sender, e)                                 ' HI�{�^�� ON/OFF
            BtnHI_Click(BtnHI, e)                                 ' HI�{�^�� ON/OFF    'V6.0.0.0�J
        End If
        'V6.0.0.0�J        Call Me.Focus()

    End Sub
#End Region

#Region "�L�[�A�b�v������"
    '''=========================================================================
    '''<summary>�L�[�A�b�v������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmCutPosCorrect_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J
        Console.WriteLine("FrmCutPosCorrect.FrmCutPosCorrect_KeyUp()")
        ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
        'V6.0.0.0�K        Call Sub_10KeyUp(KeyCode)
        Sub_10KeyUp(KeyCode, stJOG)                   'V6.0.0.0�K
        'V6.0.0.0�K        Call Me.Focus()

    End Sub

#End Region

#Region "�J�����摜�N���b�N�ʒu���摜�Z���^�[�Ɉړ����鏈��"
    ''' <summary>�J�����摜�N���b�N�ʒu���摜�Z���^�[�Ɉړ����鏈��</summary>
    ''' <param name="distanceX">�摜�Z���^�[����̋���X</param>
    ''' <param name="distanceY">�摜�Z���^�[����̋���Y</param>
    ''' <remarks>'V6.0.0.0�J</remarks>
    Public Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal) _
        Implements ICommonMethods.MoveToCenter

        Globals_Renamed.MoveToCenter(distanceX, distanceY, stJOG)
    End Sub
#End Region

    '========================================================================================
    '   �g���b�N�o�[����(�i�n�f������)
    '========================================================================================
#Region "�g���b�N�o�[�̃X���C�_�[�ړ��C�x���g"
    '''=========================================================================
    ''' <summary>�g���b�N�o�[�̃X���C�_�[�ړ��C�x���g</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub TBarLowPitch_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBarLowPitch.Scroll
        Call SetSliderPitch(IDX_PIT, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
    End Sub

    Private Sub TBarHiPitch_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBarHiPitch.Scroll
        Call SetSliderPitch(IDX_HPT, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
    End Sub

    Private Sub TBarPause_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBarPause.Scroll
        Call SetSliderPitch(IDX_PAU, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
    End Sub
#End Region


    '--- ����ȉ��͍폜

    '#Region "��ۯ��ړ����݉���������"
    '    '''=========================================================================
    '    '''<summary>��ۯ��ړ����݉���������</summary>
    '    '''<remarks></remarks>
    '    '''=========================================================================
    '    Private Sub cmdBlockMove_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

    '        Dim iXBlock As Short
    '        Dim iYBlock As Short
    '        Dim iRetc As Short
    '        Dim r As Short

    '        Me.cmdBlockMove.Enabled = False
    '        Me.cmdExit.Enabled = False
    '        iXBlock = CShort(Me.txtBlockNoX.Text)
    '        iYBlock = CShort(Me.txtBlockNoY.Text)

    '        ' ��ۯ���X,Y�Ƃ��ɂP�`�X�X
    '        If ((1 <= iXBlock) And (99 >= iXBlock)) And ((1 <= iYBlock) And (99 >= iYBlock)) Then
    '            iRetc = 1
    '            If (1 = iRetc) Then ' START�Ȃ玟����

    '                iRetc = 0

    '                'gbfReset_flg = 1 '(START�v���̗L��)

    '                '�Ǝ��␳���s
    '                r = CorrectTheta()
    '                If (r = cFRS_ERR_RST) Then
    '                    iRetc = 0
    '                End If

    '                If gbRotCorrectCancel = 0 Then
    '                    iRetc = 1 'ok
    '                Else
    '                    'errore!
    '                    iRetc = 0 'error
    '                End If
    '#If VIDEO_CAPTURE = 0 Then
    '                Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                         ' �J�����ؑ�(�������)
    '#End If

    '                If (1 = iRetc) Then
    '                    pfCurrentNo = 1 ' ������R�ԍ�
    '                    Call Set_BackColor(pfCurrentNo) ' �擪�s

    '                    iRetc = XYTableMoveBlock(0, 1, 1, iXBlock, iYBlock) ' �w����ۯ��̐擪��R�擪è��ݸ��߲�Ăֈړ�

    '                    If (iRetc) Then
    '                        'iRetc = CutPosCorrectCrossExec() ' ��Ĉʒu�␳���s(�\�����)
    '                        If (1 = iRetc) Then
    '                            'iRetc = CutPosCorrectExec(iXBlock, iYBlock) ' ��Ĉʒu�␳���s(�摜�F��)
    '                        End If
    '                    End If
    '                End If
    '            End If
    '            If (1 = iRetc) Or (0 = iRetc) Then ' ����I��orCancel�͌��_���A
    '                r = sResetTrim() ' ���_���A(�����Ɉړ�)
    '            End If

    '            ''''        '�޷���/����ߐ���(OFF)
    '            ''''        Call Form1.VacClampCtrl(False)

    '            If (1 = iRetc) Then ' ����I��
    '                Me.cmdBlockMove.Enabled = True
    '                Me.cmdExit.Enabled = True
    '                Me.Close()
    '            ElseIf (0 = iRetc) Then  ' RESET�ͷ�ݾ�
    '                'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                Call LAMP_CTRL(LAMP_RESET, True)
    '                Call LAMP_CTRL(LAMP_RESET, False)
    '                Me.cmdBlockMove.Enabled = True
    '                Me.cmdExit.Enabled = True
    '                Me.Close()
    '            ElseIf (-1 = iRetc) Then  ' ����~��̫�я���
    '                Me.cmdBlockMove.Enabled = True
    '                Me.cmdExit.Enabled = True
    '                Me.Close()
    '            End If
    '        Else
    '            MsgBox(MSG_CUT_POS_CORRECT_001, MsgBoxStyle.OkOnly, "Attention")
    '            Me.cmdBlockMove.Enabled = True
    '            Me.cmdExit.Enabled = True
    '        End If
    '    End Sub
    '#End Region

    '#Region "�w����ۯ��̒����ֈړ�"
    '    '''=========================================================================
    '    '''<summary>�w����ۯ��̒����ֈړ�</summary>
    '    '''<param name="intCamera">(INP)��׎��(0:������� 1:�O�����)</param>
    '    '''<param name="iXPlate">(INP)XPlateNo</param> 
    '    '''<param name="iYPlate">(INP)YPlateNo</param>  
    '    '''<param name="iXBlock">(INP)XBlockNo</param> 
    '    '''<param name="iYBlock">(INP)YBlockNo</param>   
    '    '''<remarks>�\����Ĉʒu��è��ݸ��߲�Ă�����ڰ��ް�
    '    '''         ��PP47�̒l�����ꂽ�Ƃ��낪���S�ƂȂ�
    '    '''         �������ڰĂ��w�肵�Ă��Ӗ��Ȃ�</remarks>
    '    '''=========================================================================
    '    Private Function XYTableMoveBlock(ByRef intCamera As Short, ByRef iXPlate As Short, ByRef iYPlate As Short, ByRef iXBlock As Short, ByRef iYBlock As Short) As Boolean

    '        Dim bRetc As Boolean
    '        Dim dblX As Double
    '        Dim dblY As Double
    '        Dim dblRotX As Double
    '        Dim dblRotY As Double
    '        Dim dblPSX As Double
    '        Dim dblPSY As Double
    '        Dim dblBsoX As Double
    '        Dim dblBsoY As Double
    '        Dim dblBSX As Double
    '        Dim dblBSY As Double
    '        Dim intTableType As Short
    '        Dim intCDir As Short
    '        Dim dblTrimPosX As Double
    '        Dim dblTrimPosY As Double
    '        Dim dblTOffsX As Double
    '        Dim dblTOffsY As Double
    '        Dim dblStepInterval As Double
    '        Dim dblExtTablePosOffsX As Double
    '        Dim dblExtTablePosOffsY As Double
    '        Dim Del_x As Double
    '        Dim Del_y As Double

    '        bRetc = False
    '        dblRotX = 0
    '        dblRotY = 0

    '        ' ����߼޼��X,Y�擾
    '        ''''(2010/11/16) ����m�F�㉺�L�R�����g�͍폜
    '        'dblTrimPosX = gStartX
    '        'dblTrimPosY = gStartY
    '        dblTrimPosX = gSysPrm.stDEV.gfTrimX
    '        dblTrimPosY = gSysPrm.stDEV.gfTrimY

    '        ' ð��وʒu�̾�Ă̎擾
    '        dblTOffsX = typPlateInfo.dblTableOffsetXDir : dblTOffsY = typPlateInfo.dblTableOffsetYDir

    '        Call CalcBlockSize(dblBSX, dblBSY) ' ��ۯ����ގZ�o
    '        intTableType = gSysPrm.stDEV.giXYtbl

    '        ' ��ۯ����޵̾�ĎZ�o�@��ۯ�����/2 ��ۯ��̏ی���XY�Ƃ���1 ð��ق̏ی���1
    '        dblBsoX = (dblBSX / 2.0#) * 1 * 1 ' Table.BDirX * Table.dir
    '        dblBsoY = (dblBSY / 2) * 1 ' Table.BDirY;
    '        ' �ƕ␳��ѵ̾��X,Y
    '        Del_x = gfCorrectPosX
    '        Del_y = gfCorrectPosY

    '        ' giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
    '        ' 0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
    '        ' ���ݸވʒu���W (+or-) ��]���a + ð��ٵ̾�� (+or-) ��ۯ����޵̾�� + ð��ٕ␳��
    '        Select Case gSysPrm.stDEV.giBpDirXy

    '            Case 0 ' x��, y��
    '                dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
    '                dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

    '            Case 1 ' x��, y��
    '                dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
    '                dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

    '            Case 2 ' x��, y��
    '                dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
    '                dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

    '            Case 3 ' x��, y��
    '                dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
    '                dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

    '        End Select

    '        If (1 = intCamera) Then                         ' �O����׈ʒu���Z ?
    '            dblExtTablePosOffsX = gSysPrm.stDEV.gfExCmX ' Externla Camera Offset X(mm)
    '            dblExtTablePosOffsY = gSysPrm.stDEV.gfExCmY ' Externla Camera Offset Y(mm)
    '            dblX = dblX + dblExtTablePosOffsX
    '            dblY = dblY + dblExtTablePosOffsY
    '        End If

    '        ' �ï�ߊԊu�̎Z�o
    '        intCDir = typPlateInfo.intResistDir             ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
    '        If intCDir = 0 Then                             ' X����
    '            dblStepInterval = CalcStepInterval(iYBlock) ' �ï�߲�����َZ�o(Y��)
    '            If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' ð���Y�������]�Ȃ�
    '                dblY = dblY + dblStepInterval
    '            Else                                        ' ð���Y�������]
    '                dblY = dblY - dblStepInterval
    '            End If
    '        Else                                            ' Y����
    '            dblStepInterval = CalcStepInterval(iXBlock) ' �ï�߲�����َZ�o(X��)
    '            If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' ð���X�������]�Ȃ�
    '                dblX = dblX + dblStepInterval
    '            Else ' ð���X�������]
    '                dblX = dblX - dblStepInterval
    '            End If
    '        End If

    '        '��ڰ�/��ۯ��ʒu�̑��΍��W�v�Z
    '        dblPSX = 0.0 : dblPSY = 0.0                         ' ��ڰĻ��ގ擾(0�Œ�)

    '        Select Case gSysPrm.stDEV.giBpDirXy

    '            Case 0 ' x��, y��
    '                dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '            Case 1 ' x��, y��
    '                dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '            Case 2 ' x��, y��
    '                dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '            Case 3 ' x��, y��
    '                dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
    '                dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

    '        End Select

    '        '�w����ڰ�/��ۯ��ʒu�Ɉړ�
    '        If (0 = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)) Then
    '            bRetc = True
    '        Else
    '            bRetc = False
    '        End If

    '        XYTableMoveBlock = bRetc

    '    End Function
    '#End Region

    '#Region "�w����ۯ��̒�R�S�Ăɏ\����Ă��s��"
    '    '''=========================================================================
    '    '''<summary>�w����ۯ��̒�R�S�Ăɏ\����Ă��s��</summary>
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function CutPosCorrectCrossExec() As Short

    '        Dim iRetc As Short
    '        Dim r As Short
    '        Dim X As Double
    '        Dim y As Double
    '        Dim intChipNum As Short
    '        Dim bFgLoop As Boolean
    '        Dim dblCutPosCorrectOffsetX As Double
    '        Dim dblCutPosCorrectOffsetY As Double
    '        Dim errChk As Integer

    '        Call GetChipNum(intChipNum) ' ��ٰ�ߓ���R��
    '        ' ��Ĉʒu�␳�̾��X,Y
    '        dblCutPosCorrectOffsetX = typPlateInfo.dblCutPosiReviseOffsetXDir
    '        dblCutPosCorrectOffsetY = typPlateInfo.dblCutPosiReviseOffsetYDir

    '        Call ZCONRST() ' ���b�`����
    '        Call BpMoveOrigin_Ex()
    '        '    Call BpMoveOrigin                               ' BP����ۯ��E��Ɉړ�(���_�ݒ�)
    '        '    Call BpMove(0, 0, 1)
    '        '    Call Form1.System1.EX_MOVE(gSysPrm, 0, 0, 1)
    '        ' �擪���W�擾
    '        X = pfStartPos(0, pfCurrentNo) + dblCutPosCorrectOffsetX
    '        y = pfStartPos(1, pfCurrentNo) + dblCutPosCorrectOffsetY
    '        '    Call BpMove(x, y, 1)                            ' �ŏ��̶�Ĉʒu�����ֈړ�
    '        Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1)

    '        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_004 ' WARNING & vbCrLf & [START]Key Cross Cut & vbCrLf & [RESET]Key CANCEL
    '        '    iRetc = InputADVRESETKey()                      ' ������
    '        iRetc = Form1.System1.AdvResetSw() ' ������
    '        If (iRetc = 1) Then
    '            ' START
    '        ElseIf (iRetc = 2) Then
    '            ' RESET
    '        ElseIf (-1 = iRetc) Then
    '            GoTo EMERGENCY ' EMERGENCY
    '        End If
    '        If (1 = iRetc) Then ' START
    '            iRetc = 0
    '            bFgLoop = True
    '            Do
    '                System.Windows.Forms.Application.DoEvents() ' ���b�Z�[�W�|���v
    '                '            If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY
    '                ' �V�X�e���G���[�`�F�b�N
    '                errChk = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
    '                If cFRS_NORMAL <> errChk Then GoTo EMERGENCY

    '                '            Call BpMove(x, y, 1)                    ' ��đO�͈ʒu�����ֈړ�
    '                Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1) ' ��đO�͈ʒu�����ֈړ�

    '                ' ADJ���L��Ȃ�҂�
    '                r = 1
    '                Call ZCONRST()
    '                If Form1.System1.AdjReqSw() Then
    '                    If pfCurrentNo < intChipNum Then ' ��ٰ�ߓ���R����
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_006 ' [START]�\���J�b�g���s" & vbCrLf & "[RESET]���~" & vbCrLf & "[ADJ]�ꎞ��~����"
    '                    Else
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_011 ' �\���J�b�g�I��"
    '                    End If

    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, True)
    '                    Call LAMP_CTRL(LAMP_HALT, True)
    '                    Do
    '                        System.Windows.Forms.Application.DoEvents()
    '                        '                    ' Emergency check
    '                        '                    If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY
    '                        ' �V�X�e���G���[�`�F�b�N
    '                        errChk = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
    '                        If cFRS_NORMAL <> errChk Then GoTo EMERGENCY

    '                        r = Form1.System1.AdvResetSw()

    '#If cOFFLINEcDEBUG = 1 Then
    '                        If (MsgBoxResult.Ok = MsgBox("OK=START CANCEL=RESET", MsgBoxStyle.OkCancel)) Then
    '                            r = cSTS_STARTSW_ON 
    '                        Else
    '                            r = cSTS_RESETSW_ON 
    '                        End If
    '#End If

    '                        'If r = 1 Or r = 3 Then ' START(ADV)
    '                        If r = cSTS_STARTSW_ON Then ' START(ADV)
    '                            Exit Do
    '                        End If
    '                        If r = cSTS_RESETSW_ON Then ' RESET
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                            Call LAMP_CTRL(LAMP_RESET, True)
    '                            '                        mMsgbox.Label1.Caption = "OK:[START] CANCEL:[HALT]"
    '                            If (ExitCheck("OK:[START(ADV)] CANCEL:[HALT]")) Then ' �I���m�F
    '                                iRetc = 0
    '                                bFgLoop = False
    '                            Else
    '                                r = 0
    '                            End If
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                            Call LAMP_CTRL(LAMP_RESET, False)
    '                        End If
    '                    Loop While (0 = r)
    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, False)
    '                    Call LAMP_CTRL(LAMP_HALT, False)
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then ' START(ADV)
    '                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_005 ' �\���J�b�g���s��" & vbCrLf & "[ADJ]�ꎞ��~"

    '                    ' �\�����
    '                    Call CrossCutExec(X, y)

    '                    '                Call BpMove(x, y, 1)                ' ��Č�͈ʒu�����ֈړ�
    '                    Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1) ' ��Č�͈ʒu�����ֈړ�
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then
    '                    If pfCurrentNo < intChipNum Then ' ��ٰ�ߓ���R����
    '                        pfCurrentNo = pfCurrentNo + 1
    '                        If pfCurrentNo <= 5 Then ' 5�ڈȍ~�͎�����۰�
    '                            Call Set_TopRow(1) ' �O���b�h�̎w��s��擪�ɂ��� 
    '                        Else
    '                            Call Set_TopRow(pfCurrentNo - 5) ' �O���b�h�̎w��s��擪�ɂ��� 
    '                        End If
    '                        Call Set_BackColor(pfCurrentNo) ' ���̒�R�̔w�i�F�ύX
    '                        ' ���̍��W�擾
    '                        X = pfStartPos(0, pfCurrentNo) + dblCutPosCorrectOffsetX
    '                        y = pfStartPos(1, pfCurrentNo) + dblCutPosCorrectOffsetY
    '                        '                    Call BpMove(x, y, 1)                ' ���̶�Ĉʒu�ֈړ�
    '                        Call Form1.System1.EX_MOVE(gSysPrm, X, y, 1) ' ���̶�Ĉʒu�ֈړ�
    '                    Else
    '                        iRetc = 1 ' ����I��
    '                        bFgLoop = False ' ��ďI��
    '                    End If
    '                End If

    '            Loop While bFgLoop = True
    '        End If
    'EXIT_LOOP:

    '        Call ZCONRST()
    '        CutPosCorrectCrossExec = iRetc
    '        Exit Function

    'EMERGENCY:
    '        Call ZCONRST()

    '        '    gMode = 5
    '        '    frmReset.Show vbModal
    '        '    r = Form1.System1.Form_Reset(cGMODE_EMG, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, LoadDTFlag)
    '        iRetc = -1
    '        If (errChk <= cFRS_ERR_EMG) Then
    '            '�����I��
    '            Call Form1.AppEndDataSave()
    '            Call Form1.AplicationForcedEnding()
    '        End If
    '    End Function
    '#End Region

    '#Region "����ݔF��"
    '    '''=========================================================================
    '    '''<summary>����ݔF��</summary>
    '    '''<param name="iGroup">(INP)��ٰ�ߔԍ�</param>
    '    '''<param name="iTemp">(INP)����ڰĔԍ�</param> 
    '    '''<param name="GapX"> (INP) </param>
    '    '''<param name="GapY"> (INP) </param>  
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function PatternRecognition(ByRef iGroup As Short, ByRef iTemp As Short, ByRef GapX As Double, ByRef GapY As Double) As Boolean
    '        ' Video.ocx���g�p����悤�ɕύX 
    '        '        Dim bRetc As Boolean
    '        '        Dim dblPixel2umX As Double
    '        '        Dim dblPixel2umY As Double

    '        '        Call GetPixel2um(dblPixel2umX, dblPixel2umY)        ' �߸�ْlX,Y

    '        '        bRetc = False
    '        '#If VIDEO_CAPTURE = 0 Then
    '        '        Dim r As Short
    '        '        Dim fcoeff As Double
    '        '        Dim lTblCnt As Integer

    '        '		Call PatternCrossLine(False)                        ' �\���\������
    '        '		' �ق��̐ݒ��Form1�ŏ����ς�
    '        '		Call MvcPt2_SetShrink(3)                            ' �V�������N��
    '        '		Call MvcPt2_SetSrchOffset(4, 4)                     ' �I�t�Z�b�g
    '        '		Call MvcPt2_SetCorrThresh(PATTERN_TRESH_INITVAL)     ' �������l�̍Đݒ�
    '        '		Call PatternAreaDisp(False)

    '        '        'Form1.picGazou.AutoRedraw = True
    '        '        Call Mvc10_PaintDIB(Form1.picGazou.Handle, mtDest, mlHSKDib, mtSrc)
    '        '        'Form1.picGazou.AutoRedraw = False

    '        '		' ����ڰĈꊇ�ǂݍ���
    '        '		If ReadTemplateGourp(iGroup) Then
    '        '		Call MvcPt2_SetTempNo(iTemp)                ' �J�����g�̃e���v���[�g�ԍ����w��
    '        '		'�����͈͐ݒ�
    '        '		Call MvcPt2_SetSrchWndRect(gtSearch.tDest)
    '        '		r = RunPatternMaching(iTemp, True, fcoeff)  ' �����ϯ�ݸގ��s

    '        '		If r = 0 Then                               ' ϯ�ݸ�0��
    '        '                Me.lblInfo.Text = MSG_CUT_POS_CORRECT_013
    '        '		GapX = 0
    '        '		GapY = 0
    '        '		bRetc = False
    '        '		Else
    '        '		lTblCnt = 1                             ' ����ʎZ�o�A�i�[

    '        '		Select Case gSysPrm.Stdev.giBpDirXy

    '        '		Case 0      ' x��, y��
    '        '		GapX = (RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = -(RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		Case 1      ' x��, y��
    '        '		GapX = -(RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = -(RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		Case 2      ' x��, y��
    '        '		GapX = (RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = (RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		Case 3      ' x��, y��
    '        '		GapX = -(RoundOff((gpoResult(lTblCnt).X - 320#) * (dblPixel2umX / 1000#)))
    '        '		GapY = (RoundOff((gpoResult(lTblCnt).y - 238#) * (dblPixel2umY / 1000#)))

    '        '		End Select

    '        '		bRetc = True
    '        '		End If
    '        '		End If
    '        '		Call PatternCrossLine(True)                     ' �\���\��
    '        '#End If
    '        '        PatternRecognition = bRetc

    '    End Function
    '#End Region

    '#Region "�\����Ă��O����ׂŏ���"
    '    '''=========================================================================
    '    '''<summary>�\����Ă��O����ׂŏ���</summary>
    '    '''<param name="iBlockX">(INP)</param>
    '    '''<param name="iBlockY">(INP)</param> 
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function CutPosCorrectExec(ByRef iBlockX As Short, ByRef iBlockY As Short) As Short

    '        Dim bRetc As Boolean
    '        Dim iRetc As Short
    '        Dim lCon As Integer
    '        Dim r As Short
    '        Dim GapX As Double
    '        Dim GapY As Double
    '        Dim bFgLoop As Boolean
    '        Dim bFgLoop2 As Boolean
    '        Dim intChipNum As Short
    '        Dim dblChipSizeX As Double
    '        Dim dblChipSizeY As Double
    '        Dim intGroup As Short
    '        Dim intTemp As Short
    '        Dim dblBpOffsetX As Double
    '        Dim dblBpOffsetY As Double
    '        Dim dblCutPosCorrectOffsetX As Double
    '        Dim dblCutPosCorrectOffsetY As Double

    '        Call GetChipNum(intChipNum) ' ��ٰ�ߓ���R��

    '        ' ���߻���
    '        dblChipSizeX = typPlateInfo.dblChipSizeXDir
    '        dblChipSizeY = typPlateInfo.dblChipSizeYDir

    '        ' BP�ʒu�̾��X,Y�ݒ�
    '        dblBpOffsetX = typPlateInfo.dblBpOffSetXDir
    '        dblBpOffsetY = typPlateInfo.dblBpOffSetYDir

    '        ' ��Ĉʒu�␳�̾��X,Y
    '        dblCutPosCorrectOffsetX = typPlateInfo.dblCutPosiReviseOffsetXDir
    '        dblCutPosCorrectOffsetY = typPlateInfo.dblCutPosiReviseOffsetYDir
    '        ' �����ϯ�ݸ޸�ٰ�ߔԍ�/����ڰĔԍ�
    '        intGroup = typPlateInfo.intCutPosiReviseGroupNo
    '        intTemp = typPlateInfo.intCutPosiRevisePtnNo

    '        Call ZCONRST() ' ���b�`����

    '        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_007 ' [START]�摜�F�����s" & vbCrLf & "[RESET]���~"

    '        '    iRetc = InputADVRESETKey()                      ' ������
    '        iRetc = Form1.System1.AdvResetSw() ' ������
    '        If (iRetc = 1) Then
    '            ' ADV
    '        ElseIf (iRetc = 2) Then
    '            ' RESET
    '        ElseIf (-1 = iRetc) Then
    '            GoTo EMERGENCY ' EMERGENCY
    '        End If

    '        If (1 = iRetc) Then ' START

    '            pfCurrentNo = 1 ' �摜�������n�߂�̂Ő擪�֖߂�

    '            Call Set_TopRow(pfCurrentNo) ' �O���b�h�̎w��s��擪�ɂ��� 
    '            Call Set_BackColor(pfCurrentNo)
    '            ' BP�̾�Ăɶ�Ĉʒu�␳�̾�Đݒ�
    '            '        Call BpOffset(0, 0)                         ' �O����ׂł�BP�͎g�p�ł��Ȃ��̂�
    '            '        Call BpMove(0, 0, 1)                        ' ���������Ă���
    '            Call Form1.System1.EX_BPOFF(gSysPrm, 0.0#, 0.0#) ' BP�̾�Ăɶ�Ĉʒu�␳�̾�Đݒ�
    '            Call Form1.System1.EX_MOVE(gSysPrm, 0.0#, 0.0#, 1) ' �ŏ��̶�Ĉʒu�����ֈړ�

    '#If VIDEO_CAPTURE = 0 Then
    '            Call Form1.VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)                     ' �J�����ؑ�(�O�����)
    '#End If
    '            ' �O����ׂֈړ�
    '            Call XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, pfCurrentNo)
    '            Call System.Threading.Thread.Sleep(1000)                       ' �e�[�u���ړ���摜����҂�����(ms)
    '            iRetc = 0
    '            bFgLoop = True
    '            Do
    '                System.Windows.Forms.Application.DoEvents() ' ���b�Z�[�W�|���v
    '                If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY

    '                ' ADJ���L��Ȃ�҂�
    '                r = 1
    '                Call ZCONRST() ' ���b�`����
    '                If Form1.System1.AdjReqSw() Then
    '                    If pfCurrentNo < intChipNum Then ' ��ٰ�ߓ���R����
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_009 ' [START]�摜�F�����s" & vbCrLf & "[RESET]���~" & vbCrLf & "[ADJ]�ꎞ��~����"
    '                    Else
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_012 ' �摜�F���I��
    '                    End If

    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, True)
    '                    Call LAMP_CTRL(LAMP_HALT, True)
    '                    r = 0
    '                    Do
    '                        Call ZCONRST() ' ���b�`����
    '                        System.Windows.Forms.Application.DoEvents()
    '                        ' Emergency check
    '                        If Form1.System1.EmergencySwCheck() Then GoTo EMERGENCY

    '#If cOFFLINEcDEBUG = 1 Then
    '                        lCon = MsgBox("YES=START NO=HALT CANCEL=RESET", MsgBoxStyle.YesNoCancel)
    '                        If (MsgBoxResult.Yes = lCon) Then
    '                            lCon = &H4S
    '                        ElseIf (MsgBoxResult.No = lCon) Then
    '                            lCon = &H2S
    '                        Else
    '                            lCon = &H8S
    '                        End If
    '#Else
    '                        '                        lCon = ConsoleInput         ' ������
    '                        Call ZINPSTS(GET_CONSOLE_INPUT, lCon)
    '#End If

    '                        If ((lCon And &H4S) = &H4S) Then ' ADV
    '                            r = cSTS_STARTSW_ON
    '                            Exit Do ' ���s
    '                        ElseIf ((lCon And &H8S) = &H8S) Then  ' RESET
    '                            r = cSTS_RESETSW_ON
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                            Call LAMP_CTRL(LAMP_RESET, True)
    '                            '                        mMsgbox.Label1.Caption = "OK:[ADV] CANCEL:[HALT]"
    '                            If (ExitCheck("OK:[START] CANCEL:[HALT]")) Then ' �I���m�F
    '                                iRetc = 0
    '                                bFgLoop = False
    '                            Else
    '                                r = 0
    '                            End If
    '                            'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                            Call LAMP_CTRL(LAMP_RESET, False)
    '                        ElseIf ((lCon And &H2S) = &H2S) Then  ' HALT
    '                            r = cSTS_HALTSW_ON
    '                        End If
    '                    Loop While (0 = r)
    '                    'Call Form1.System1.sLampOnOff(LAMP_HALT, False)
    '                    Call LAMP_CTRL(LAMP_HALT, False)
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then
    '                    Me.lblInfo.Text = MSG_CUT_POS_CORRECT_008 ' �摜�F�����s���s��" & vbCrLf & "[ADJ]�ꎞ��~"

    '                    ' �摜�F��
    '                    GapX = 0
    '                    GapY = 0
    '                    bRetc = PatternRecognition(intGroup, intTemp, GapX, GapY)

    '                    If (Not bRetc) Then
    '                        bFgLoop2 = True
    '                        Me.lblInfo.Text = MSG_CUT_POS_CORRECT_013
    '                        Do
    '                            lCon = ExtTableJogX(GapX, GapY) ' �蓮Ӱ�ޓ˓�
    '                            If ((lCon And &H4S) = &H4S) Then ' ADV
    '                                r = cSTS_STARTSW_ON
    '                                bRetc = True
    '                                bFgLoop2 = False
    '                            ElseIf ((lCon And &H8S) = &H8S) Then  ' RESET
    '                                r = cSTS_RESETSW_ON
    '                                'Call Form1.System1.sLampOnOff(LAMP_RESET, True)
    '                                Call LAMP_CTRL(LAMP_RESET, True)
    '                                '                            mMsgbox.Label1.Caption = "OK:[ADV] CANCEL:[HALT]"
    '                                If (ExitCheck("OK:[START] CANCEL:[HALT]")) Then ' �I���m�F
    '                                    iRetc = 0
    '                                    bFgLoop2 = False
    '                                    bFgLoop = False
    '                                End If
    '                                'Call Form1.System1.sLampOnOff(LAMP_RESET, False)
    '                                Call LAMP_CTRL(LAMP_RESET, False)
    '                            ElseIf ((lCon And &H2S) = &H2S) Then  ' HALT
    '                                bFgLoop2 = False
    '                                r = cSTS_HALTSW_ON
    '                            End If
    '                        Loop While (bFgLoop2)
    '                    End If
    '                    If (bRetc) Then
    '                        dblGapX(pfCurrentNo) = GapX ' �����X
    '                        dblGapY(pfCurrentNo) = GapY ' �����Y
    '                        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 3, GapX.ToString("0.0000"))
    '                        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 4, GapY.ToString("0.0000"))
    '                    End If
    '                End If

    '                If (cSTS_STARTSW_ON = r) Then
    '                    If pfCurrentNo < intChipNum Then        ' ��ٰ�ߓ���R����
    '                        pfCurrentNo = pfCurrentNo + 1
    '                        If pfCurrentNo <= 5 Then            ' 5�ڈȍ~�͎�����۰�
    '                            Call Set_TopRow(1)                          ' �O���b�h�̎w��s��擪�ɂ��� 
    '                        Else
    '                            Call Set_TopRow(pfCurrentNo - 5) ' �O���b�h�̎w��s��擪�ɂ��� 
    '                        End If
    '                        Call Set_BackColor(pfCurrentNo)  ' ���̒�R�̔w�i�F�ύX
    '                        Call XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, pfCurrentNo)
    '                        Call System.Threading.Thread.Sleep(1000) ' �e�[�u���ړ���摜����҂�����(ms)
    '                    Else
    '                        pfCurrentNo = pfCurrentNo + 1
    '                        Call ZCONRST() ' ���b�`����
    '                        '                    mMsgbox.Label1.Caption = MSG_106    ' ���̏���ۑ����đO�̉�ʂɖ߂�܂��B��낵���ł����H"
    '                        If (ExitCheck(MSG_106)) Then ' �I���m�F
    '                            UpdatePoints() ' �e�����߲�Ăɔ��f
    '                            iRetc = 1 ' ����I��
    '                            bFgLoop = False ' �␳�I��
    '                        Else
    '                            r = cSTS_HALTSW_ON  ' ��O��
    '                        End If
    '                    End If
    '                End If
    '                If (cSTS_HALTSW_ON = r) Then
    '                    If pfCurrentNo > 1 Then
    '                        pfCurrentNo = pfCurrentNo - 1
    '                        If pfCurrentNo <= 5 Then
    '                            Call Set_TopRow(1) ' �O���b�h�̎w��s��擪�ɂ��� 
    '                        Else
    '                            Call Set_TopRow(pfCurrentNo - 5) ' �O���b�h�̎w��s��擪�ɂ��� 
    '                        End If
    '                        Call Set_BackColor(pfCurrentNo)
    '                        Call XYTableMoveBlockFirst(1, 1, 1, iBlockX, iBlockY, pfCurrentNo)
    '                        Call System.Threading.Thread.Sleep(1000)           ' �e�[�u���ړ���摜����҂�����(ms)
    '                    End If
    '                End If
    '            Loop While (bFgLoop)
    '        End If
    '        Call ZCONRST()

    '#If VIDEO_CAPTURE = 0 Then
    '        Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)       ' �J�����ؑ�(�������)
    '        Call PatternCrossLine(False)
    '        Call PatternAreaDisp(False)
    '#End If

    '        CutPosCorrectExec = iRetc
    '        Exit Function

    'EMERGENCY:
    '        Call ZCONRST()

    '        '    gMode = 5
    '        '    frmReset.Show vbModal
    '        r = Form1.System1.Form_Reset(cGMODE_EMG, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, LoadDTFlag)
    '        iRetc = -1
    '        If (r <= cFRS_ERR_EMG) Then
    '            '�����I��
    '            Call Form1.AppEndDataSave()
    '            Call Form1.AplicationForcedEnding()
    '        End If

    '#If VIDEO_CAPTURE = 0 Then
    '        Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)       ' �J�����ؑ�(�������)
    '        Call PatternCrossLine(False)
    '        Call PatternAreaDisp(False)
    '#End If

    '        CutPosCorrectExec = iRetc

    '    End Function
    '#End Region

    '#Region "�\����Ă��s��"
    '    '''=========================================================================
    '    '''<summary>�\����Ă��s��</summary>
    '    '''<param name="dx">(INP)BP���WX</param>
    '    '''<param name="dy">(INP)BP���WY</param> 
    '    '''<remarks>�\����Ă̒�����BP���ړ����Ă�������</remarks>
    '    '''=========================================================================
    '    Private Function CrossCutExec(ByRef dx As Double, ByRef dy As Double) As Short

    '        Dim dblCutLength As Double
    '        Dim dblCutSpeed As Double
    '        Dim dblCutQRate As Double
    '        Dim intXDir As Short
    '        Dim intYDir As Short
    '        Dim d As Double

    '        dblCutLength = typPlateInfo.dblCutPosiReviseCutLength   ' ��Ē�
    '        dblCutSpeed = typPlateInfo.dblCutPosiReviseCutSpeed     ' ��đ��x
    '        dblCutQRate = typPlateInfo.dblCutPosiReviseCutQRate     ' ���Qڰ�

    '        If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' X�������]�Ȃ�
    '            intXDir = 1
    '        Else ' X�������]
    '            intXDir = 0
    '        End If

    '        Call QRATE(dblCutQRate)

    '        '    Call BpMove(dx - (dblCutLength / 2), dy, 1)     ' X���n�_��
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx - (dblCutLength / 2), dy, 1) ' X���n�_��
    '        ' LASER ON
    '        Call LASERON()
    '        Call cut(dblCutLength, intXDir, dblCutSpeed) ' X�����
    '        ' LASER OFF
    '        Call LASEROFF()
    '        '    Call BpMove(dx, dy, 1)      ' ���S�֖߂�
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx, dy, 1) ' ���S�֖߂�
    '        Call System.Threading.Thread.Sleep(500)                   ' Wait(500ms)

    '        If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' Y�������]�Ȃ�
    '            intYDir = 3
    '            d = 1.0#
    '        Else ' Y�������]
    '            intYDir = 3
    '            d = -1.0#
    '        End If

    '        '    Call BpMove(dx, (dy - (dblCutLength / 2)) * d, 1)   ' Y���n�_��
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx, (dy - (dblCutLength / 2)) * d, 1) ' Y���n�_��

    '        ' LASER ON
    '        Call LASERON()
    '        Call cut(dblCutLength, intYDir, dblCutSpeed) ' Y�����
    '        ' LASER OFF
    '        Call LASEROFF()

    '        '    Call BpMove(dx, dy, 1)  ' ���S�֖߂�
    '        Call Form1.System1.EX_MOVE(gSysPrm, dx, dy, 1) ' ���S�֖߂�
    '        Call System.Threading.Thread.Sleep(500) ' 500ms Wait

    '    End Function
    '#End Region


    '#Region "���SW�ɂ��e�[�u������(�O���J������)"
    '    '''=========================================================================
    '    '''<summary>���SW�ɂ��e�[�u������(�O���J������)</summary>
    '    '''<param name="dblGapX">(INP)�e�[�u�����WX</param>
    '    '''<param name="dblGapY">(INP)�e�[�u�����WY</param> 
    '    '''<returns> </returns>
    '    '''=========================================================================
    '    Private Function ExtTableJogX(ByRef dblGapX As Double, ByRef dblGapY As Double) As Short

    '        Dim Dspx As Double
    '        Dim Dspy As Double
    '        Dim mPIT As Double '�ړ��������
    '        Dim mvx As Double
    '        Dim mvy As Double
    '        Dim rslt As Integer
    '        Dim cin As Integer

    '        'inisialize
    '        mvx = 0 : mvy = 0
    '        Dspx = dblGapX : Dspy = dblGapY
    '        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 3, Dspx.ToString("0.0000"))
    '        Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 4, Dspy.ToString("0.0000"))

    '        'HALT/RESET/START�����������܂�ٰ��
    '        Do
    '            If Form1.System1.EmergencySwCheck() Then
    '                Call ZCONRST() 'ׯ�����
    '                rslt = &H10S
    '                Exit Do
    '            End If

    '            System.Windows.Forms.Application.DoEvents()

    '            ' �ݿ�ٓ���
    '#If cOFFLINEcDEBUG Then
    '            'Me.Framedebug.Visible = True                '���ޯ�ޗp
    '#Else
    '            '            AppInfo.cmdNo = CMD_Z_INPSTS '�ݿ�ٓ���
    '            '            Call VBINtimeSendMessage(1, AppInfo, ResInfo)
    '            Call ZINPSTS(GET_CONSOLE_INPUT, cin)
    '#End If

    '            'key check!
    '            If cin And &H1E00S Then '���SW
    '                '�ړ��������
    '                If cin And &H100S Then
    '                    mPIT = gSysPrm.stSYP.gStageHighPIT 'High speed
    '                Else
    '                    mPIT = gSysPrm.stSYP.gPIT 'Normal
    '                End If
    '                '�������!
    '                If cin And &H200S Then '��
    '                    mvx = mPIT
    '                    mvy = 0
    '                ElseIf cin And &H400S Then  '��
    '                    mvx = -1 * mPIT
    '                    mvy = 0
    '                ElseIf cin And &H800S Then  '��
    '                    mvx = 0
    '                    mvy = mPIT
    '                ElseIf cin And &H1000S Then  '��
    '                    mvx = 0
    '                    mvy = -1 * mPIT
    '                End If

    '                'ð��ّ��Έړ�
    '                '            Call XYtableRelMove(mvx, mvy)
    '                Call Form1.System1.XYtableRelMove(gSysPrm, mvx, mvy)
    '                Call ZWAIT(gSysPrm.stSYP.gPitPause)
    '                cin = 0

    '                'XY���ۂ̍l��(2004/12/02)
    '                Select Case gSysPrm.stDEV.giBpDirXy
    '                    Case 0 ' x��, y��
    '                        mvx = -mvx
    '                        mvy = -mvy
    '                    Case 1 ' x��, y��
    '                        mvx = mvx
    '                        mvy = -mvy
    '                    Case 2 ' x��, y��
    '                        mvx = -mvx
    '                        mvy = mvy
    '                    Case 3 ' x��, y��
    '                        mvx = mvx
    '                        mvy = mvy
    '                End Select

    '                '���W�\��
    '                Dspx = Dspx + mvx
    '                Dspy = Dspy + mvy
    '                Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 3, Dspx.ToString("0.0000"))
    '                Me.fgrdPoints.set_TextMatrix(pfCurrentNo, 4, Dspy.ToString("0.0000"))
    '            ElseIf cin And &H4S Then  'START(ADV) SW
    '                dblGapX = Dspx
    '                dblGapY = Dspy
    '                Call ZCONRST() 'ׯ�����
    '                rslt = &H4S
    '                Exit Do
    '            ElseIf cin And &H8S Then  'RESET SW
    '                Call ZCONRST() 'ׯ�����
    '                rslt = &H8S
    '                Exit Do
    '            ElseIf cin And &H2S Then  'HALT SW
    '                Call ZCONRST() 'ׯ�����
    '                rslt = &H2S
    '                Exit Do
    '            End If
    '        Loop
    '        ExtTableJogX = rslt
    '        cin = 0

    '        ' ���ޯ�ޗp
    '#If cOFFLINEcDEBUG Then
    '        'Me.Framedebug.Visible = False
    '#End If

    '    End Function
    '#End Region

    '#Region "�I���m�F"
    '    '''=========================================================================
    '    '''<summary>�I���m�F</summary>
    '    '''<returns>TRUE:�I��, FALSE:��ݾ�</returns>
    '    '''=========================================================================
    '    Private Function ExitCheck(ByRef strMSG As String) As Boolean
    '        ExitFlag = False
    '        '    mMsgbox.Show vbModal
    '        '    ExitCheck = mMsgbox.sGetReturn
    '        ExitCheck = Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkCancel, FRM_CUT_POS_CORRECT_TITLE)

    '    End Function
    '#End Region

    '#Region "�w��̃u���b�N�ԍ�����e�[�u�����W(�O���J������)�����߂�"
    '    '''=========================================================================
    '    ''' <summary>�w��̃u���b�N�ԍ�����e�[�u�����W(�O���J������)�����߂�</summary>
    '    ''' <param name="stPLT">(INP)�v���[�g�f�[�^</param>
    '    ''' <param name="BlX">  (INP)�u���b�N�ԍ�X</param>
    '    ''' <param name="BlY">  (INP)�u���b�N�ԍ�Y</param>
    '    ''' <param name="POSX"> (OUT)�e�[�u�����WX</param>
    '    ''' <param name="POSY"> (OUT)�e�[�u�����WY</param>
    '    ''' <remarks></remarks>
    '    '''=========================================================================
    '    Private Sub GetTablePos(ByRef stPLT As PlateInfo, ByVal BlX As Integer, ByVal BlY As Integer, ByRef POSX As Double, ByRef POSY As Double)

    '        Dim CSx As Double, CSy As Double                                    ' �`�b�v�T�C�Y
    '        Dim OfsX As Double, OfsY As Double                                  ' �e�[�u���ʒu�I�t�Z�b�g
    '        Dim BpOfsX As Double, BpOfsY As Double                              ' �a�o�I�t�Z�b�g
    '        Dim Del_x As Double, Del_y As Double                                ' �g�����|�W�V�����␳�l
    '        Dim dblStepIntervel As Double
    '        Dim strMSG As String

    '        Try

    '            ' �e�[�u���ʒu�I�t�Z�b�g
    '            OfsX = stPLT.dblTableOffsetXDir
    '            OfsY = stPLT.dblTableOffsetYDir
    '            ' �g�����|�W�V�����␳�l(�ƕ␳����XYð��ق����)
    '            Del_x = gfCorrectPosX
    '            Del_y = gfCorrectPosY
    '            ' �a�o�I�t�Z�b�g
    '            BpOfsX = stPLT.dblBpOffSetXDir
    '            BpOfsY = stPLT.dblBpOffSetYDir
    '            ' �`�b�v�T�C�Y
    '            CSx = stPLT.dblChipSizeXDir
    '            CSy = stPLT.dblChipSizeYDir

    '            ' ���ݸވʒu (+or-) ��]���a + ð��ٵ̾�� + ð��ٕ␳�� + �O����׵̾��
    '            Select Case gSysPrm.stDEV.giBpDirXy
    '                Case 0      ' x��, y��
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX + BpOfsX
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY + BpOfsY

    '                Case 1      ' x��, y��
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX - BpOfsX      '�@�����̏ꍇ�AX�������t�ɂȂ邽�߁A���Z
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY + BpOfsY

    '                Case 2      ' x��, y��
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX + BpOfsX
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY - BpOfsY      ' �E����̏ꍇ�AY�������t�ɂȂ邽�߁A���Z

    '                Case 3      ' x��, y��
    '                    POSX = gSysPrm.stDEV.gfTrimX + OfsX + Del_x + gSysPrm.stDEV.gfExCmX - BpOfsX      ' ������̏ꍇ�AX�������t�ɂȂ邽�߁A���Z
    '                    POSY = gSysPrm.stDEV.gfTrimY + OfsY + Del_y + gSysPrm.stDEV.gfExCmY - BpOfsY      ' ������̏ꍇ�AY�������t�ɂȂ邽�߁A���Z
    '            End Select

    '            ' �ï�߲�����َZ�o(Y��)(��R���ѕ��� = X�����̏ꍇ)
    '            If (stPLT.intResistDir = 0) Then                                        ' ��R���ѕ��� = X����
    '                Call CalcStepIntervel(typPlateInfo, typStepInfoArray, BlY, dblStepIntervel)
    '                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then  ' ð���Y�������]�Ȃ� ?
    '                    POSY = POSY + (CSy * (BlY - 1))
    '                    POSY = POSY + dblStepIntervel
    '                Else                                                                ' ð���Y�������]
    '                    POSY = POSY - (CSy * (BlY - 1))
    '                    POSY = POSY - dblStepIntervel
    '                End If

    '                ' �ï�߲�����َZ�o(X��)(��R���ѕ��� = Y�����̏ꍇ)
    '            Else                                                                    ' ��R���ѕ��� = Y����
    '                Call CalcStepIntervel(typPlateInfo, typStepInfoArray, BlX, dblStepIntervel)
    '                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then  ' ð���X�������]�Ȃ�
    '                    POSX = POSX + (CSx * (BlX - 1))
    '                    POSX = POSX + dblStepIntervel
    '                Else                                                                ' ð���X�������]
    '                    POSX = POSX - (CSx * (BlX - 1))
    '                    POSX = POSX - dblStepIntervel
    '                End If
    '            End If

    '            Exit Sub

    '            ' �g���b�v�G���[������
    '        Catch ex As Exception
    '            strMSG = "FrmCutPosCorrect.GetTablePos() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region
    '#Region "�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�����F�ɂ���"
    '    '''=========================================================================
    '    '''<summary>�X�^�[�g�|�C���g�O���b�h�̎w��s�̔w�i�����F�ɂ���</summary>
    '    '''<param name="iCol">(INP)�s�ԍ�(1-)</param>
    '    '''=========================================================================
    '    Private Sub SetBackColor(ByVal iCol As Short)

    '        Static iPrevColumn As Integer
    '        Dim strMSG As String

    '        Try
    '            If IsNothing(iPrevColumn) Then                  ' �ȑO�I������Ă�����
    '                iPrevColumn = 0
    '            End If

    '            With Me.fgrdPoints
    '                If iPrevColumn >= 0 Then                    ' �I������Ă����獕�ɂ���
    '                    If .Rows > iPrevColumn Then
    '                        .Row = iPrevColumn
    '                        .Col = 1 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                        .Col = 2 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                        .Col = 3 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                        .Col = 4 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(0)
    '                    End If
    '                End If
    '                .Row = iCol                                 ' ���F�ɐݒ�
    '                .Col = 1 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '                .Col = 2 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '                .Col = 3 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '                .Col = 4 : .CellBackColor = System.Drawing.ColorTranslator.FromOle(QBColor(14))
    '            End With
    '            iPrevColumn = iCol

    '            ' �g���b�v�G���[������
    '        Catch ex As Exception
    '            strMSG = "FrmCutPosCorrect.SetBackColor() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region

End Class