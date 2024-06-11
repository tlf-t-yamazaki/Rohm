'===============================================================================
'   Description  : �L�����u���[�V�������s����
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0
Imports LaserFront.Trimmer.DefWin32Fnc              'V6.1.4.2�@

Friend Class FrmCalibration
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

#Region "�v���C�x�[�g�ϐ���`"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- �ϐ���` -----
    Private stJOG As JOG_PARAM                                          ' �����(JOG����)�p�p�����[�^
    Private mExit_flg As Short                                          ' �I���t���O

    '----- ���̑� -----
    Private dblTchMoval(3) As Double                                    ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))
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
    '    'V6.0.0.0�J        Public ReadOnly Property sGetReturn() As Short
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

#Region "Form Closed������"
    '''=========================================================================
    '''<summary>Form Closed������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub FrmCalibration_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

    End Sub
#End Region

#Region "̫��۰�ގ�����"
    '''=========================================================================
    '''<summary>̫��۰�ގ�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub FrmCalibration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ��������
            'Call SetMessages()                                          ' ���x������ݒ肷��(���{��/�p��)
            mExit_flg = -1                                              ' �I���t���O = ������

            ' ����޳����۰قɏd�˂�
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' �g���~���O�f�[�^���\�����ڂ�ݒ肷��
            Call SetTrimData()

            Me.TopMost = True      ''V3.0.0.0�J    'V6.0.0.0�L Activate����ړ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.FrmCalibration_Load() TRAP ERROR = " + ex.Message
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

        Dim strMSG As String

        Try
            ' ���x������ݒ肷��(���{��/�p��)
            'Call PrepareMessagesCutPosCorrect_Calibration(gSysPrm.stTMN.giMsgTyp)          'V1.20.0.0�G
            'Me.frmTitle.Text = FRM_CALIBRATION_TITLE                                        ' �����ڰ���
            'Me.lblStandardCoordinates1XTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES1X  ' �����ڰ��݊���W1X
            'Me.lblStandardCoordinates1YTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES1Y  ' �����ڰ��݊���W1Y
            'Me.lblStandardCoordinates2XTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES2X  ' �����ڰ��݊���W2X
            'Me.lblStandardCoordinates2YTitle.Text = LBL_CALIBRATION_STANDERD_COORDINATES2Y  ' �����ڰ��݊���W2Y
            'Me.lblTableOffsetXTitle.Text = LBL_CALIBRATION_TABLE_OFFSETX                    ' �����ڰ���ð��ٵ̾��X
            'Me.lblTableOffsetYTitle.Text = LBL_CALIBRATION_TABLE_OFFSETY                    ' �����ڰ���ð��ٵ̾��Y

            'Me.lblGap1XTitle.Text = LBL_CALIBRATION_GAP1X                                   ' �����ڰ��݂����1X
            'Me.lblGap1YTitle.Text = LBL_CALIBRATION_GAP1Y                                   ' �����ڰ��݂����1Y
            'Me.lblGap2XTitle.Text = LBL_CALIBRATION_GAP2X                                   ' �����ڰ��݂����2X
            'Me.lblGap2YTitle.Text = LBL_CALIBRATION_GAP2Y                                   ' �����ڰ��݂����2Y

            'Me.lblGainXTitle.Text = LBL_CALIBRATION_GAINX                                   ' �����ڰ��ݹ޲ݕ␳�W��X
            'Me.lblGainYTitle.Text = LBL_CALIBRATION_GAINY                                   ' �����ڰ��ݹ޲ݕ␳�W��Y
            'Me.lblOffsetXTitle.Text = LBL_CALIBRATION_OFFSETX                               ' �����ڰ��ݵ̾�ĕ␳��X
            'Me.lblOffsetYTitle.Text = LBL_CALIBRATION_OFFSETY                               ' �����ڰ��ݵ̾�ĕ␳��Y

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.SetMessages() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�f�[�^���\�����ڂ�ݒ肷��"
    '''=========================================================================
    '''<summary>�g���~���O�f�[�^���\�����ڂ�ݒ肷��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim dblStanderd1X As Double                                     ' �����ڰ��݊���W1X
        Dim dblStanderd1Y As Double                                     ' �����ڰ��݊���W1Y
        Dim dblStanderd2X As Double                                     ' �����ڰ��݊���W2X
        Dim dblStanderd2Y As Double                                     ' �����ڰ��݊���W2Y
        Dim dblTableOffsetX As Double                                   ' �����ڰ���ð��ٵ̾��X
        Dim dblTableOffsetY As Double                                   ' �����ڰ���ð��ٵ̾��Y
        Dim strMSG As String

        Try
            ' �\�����ڐݒ�
            dblStanderd1X = typPlateInfo.dblCaribBaseCordnt1XDir        ' �����ڰ��݊���W1
            dblStanderd1Y = typPlateInfo.dblCaribBaseCordnt1YDir
            dblStanderd2X = typPlateInfo.dblCaribBaseCordnt2XDir        ' �����ڰ��݊���W2
            dblStanderd2Y = typPlateInfo.dblCaribBaseCordnt2YDir

            Me.lblStandardCoordinates1X.Text = dblStanderd1X.ToString("##0.0000") ' �����ڰ��݊���W1X
            Me.lblStandardCoordinates1Y.Text = dblStanderd1Y.ToString("##0.0000") ' �����ڰ��݊���W1Y
            Me.lblStandardCoordinates2X.Text = dblStanderd2X.ToString("##0.0000") ' �����ڰ��݊���W2X
            Me.lblStandardCoordinates2Y.Text = dblStanderd2Y.ToString("##0.0000") ' �����ڰ��݊���W2Y

            ' �����ڰ���ð��ٵ̾��X,Y
            dblTableOffsetX = typPlateInfo.dblCaribTableOffsetXDir
            dblTableOffsetY = typPlateInfo.dblCaribTableOffsetYDir
            Me.lblTableOffsetX.Text = dblTableOffsetX.ToString("##0.0000") ' �����ڰ���ð��ٵ̾��X
            Me.lblTableOffsetY.Text = dblTableOffsetY.ToString("##0.0000") ' �����ڰ���ð��ٵ̾��Y

            Me.lblGap1X.Text = ""                                       ' �����1X
            Me.lblGap1Y.Text = ""                                       ' �����1Y
            Me.lblGap2X.Text = ""                                       ' �����2X
            Me.lblGap2Y.Text = ""                                       ' �����2Y

            Me.lblGainX.Text = ""                                       ' �޲ݕ␳�W��X
            Me.lblGainY.Text = ""                                       ' �޲ݕ␳�W��Y
            Me.lblOffsetX.Text = ""                                     ' �̾�ĕ␳��X
            Me.lblOffsetY.Text = ""                                     ' �̾�ĕ␳��Y

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.SetTrimData() TRAP ERROR = " + ex.Message
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
    Private Sub FrmCalibration_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' �L�����u���[�V�������s
            If (mExit_flg <> -1) Then Exit Sub

            'V6.0.0.0�L            Me.TopMost = True      ''V3.0.0.0�J    'V6.0.0.0�L Load�Ɉړ�

            mExit_flg = 0                                               ' �I���t���O = 0
            mExit_flg = CalibrationMain()                               ' �L�����u���[�V�������s

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.FrmCalibration_Activated() TRAP ERROR = " + ex.Message
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
            ' �L�����u���[�V�������s
            If (mExit_flg <> -1) Then Exit Function

            'V6.0.0.0�L            Me.TopMost = True      ''V3.0.0.0�J    'V6.0.0.0�L Load�Ɉړ�

            mExit_flg = 0                                               ' �I���t���O = 0
            mExit_flg = CalibrationMain()                               ' �L�����u���[�V�������s

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "FrmCalibration.Execute() TRAP ERROR = " & ex.Message
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
    '   �L�����u���[�V�������s����
    '========================================================================================
#Region "�L�����u���[�V�������s���C������"
    '''=========================================================================
    '''<summary>�L�����u���[�V�������s���C������</summary>
    '''<returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST  =Cancel(RESET��)
    '''         -1�ȉ�        =�G���[</returns>
    '''=========================================================================
    Private Function CalibrationMain() As Integer

        Dim mdBSx As Double                                             ' ��ۯ�����X
        Dim mdBSy As Double                                             ' ��ۯ�����Y
        'Dim dblStanderd1X As Double                                    ' V3.0.0.0�E(V1.22.0.0�F)
        'Dim dblStanderd1Y As Double
        'Dim dblStanderd2X As Double
        'Dim dblStanderd2Y As Double
        Dim dblGainX As Double                                          ' �޲ݕ␳�W��
        Dim dblGainY As Double
        Dim dblOffsetX As Double                                        ' �̾�ĕ␳�W��
        Dim dblOffsetY As Double
        Dim dQrate As Double
        Dim CondNum As Integer
        Dim r As Integer
        Dim rtn As Integer
        Dim StpNum As Integer
        Dim ObjProc As Process = Nothing
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' ����W�擾
            '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
            If (typPlateInfo.intResistDir = 0) Then                     ' ��R(����)���ѕ���(0:X, 1:Y)
                mdBSx = 80.0                                            ' �u���b�N�T�C�Y��80*0�Ƃ��� 
                mdBSy = 0.0
                '----- V1.24.0.0�@�� -----
                If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                    mdBSx = 60.0                                        ' �u���b�N�T�C�Y��60*0�Ƃ��� 
                ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then
                    mdBSx = 90.0                                        ' �u���b�N�T�C�Y��90*0�Ƃ���   'V4.4.0.0�@ 
                End If
                '----- V1.24.0.0�@�� -----
            Else
                mdBSx = 0.0                                            ' �u���b�N�T�C�Y��0*80�Ƃ��� 
                mdBSy = 80.0
                '----- V1.24.0.0�@�� -----
                If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                    mdBSy = 60.0                                        ' �u���b�N�T�C�Y��0*60�Ƃ���
                ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then
                    mdBSy = 90.0                                        ' �u���b�N�T�C�Y��0*90�Ƃ���   'V4.4.0.0�@
                End If
                '----- V1.24.0.0�@�� -----
            End If

            'dblStanderd1X = typPlateInfo.dblCaribBaseCordnt1XDir        ' �����ڰ��݊���W1
            'dblStanderd1Y = typPlateInfo.dblCaribBaseCordnt1YDir
            'dblStanderd2X = typPlateInfo.dblCaribBaseCordnt2XDir        ' �����ڰ��݊���W2
            'dblStanderd2Y = typPlateInfo.dblCaribBaseCordnt2YDir
            ''----- V1.14.0.0�F�� -----
            ''----- V1.20.0.1�A�� -----
            ''mdBSx = 80.0                                                ' �u���b�N�T�C�Y��80�p�Ƃ��� 
            ''mdBSy = 80.0
            '' �u���b�N�T�C�Y�͂��̂܂܂Ƃ���(Move_Trimposition()�Ńu���b�N�T�C�Y��BP�I�t�Z�b�g(0,0)�͐ݒ�ς�) 
            'mdBSx = typPlateInfo.dblBlockSizeXDir
            'mdBSy = typPlateInfo.dblBlockSizeYDir
            '----- V3.0.0.0�E(V1.22.0.0�F)�� -----

            ' BP������W1�Ɉړ�����
            'r = Form1.System1.EX_MOVE(gSysPrm, dblStanderd1X + typPlateInfo.dblCaribTableOffsetXDir, dblStanderd1Y + typPlateInfo.dblCaribTableOffsetYDir, 1)
            r = CaribBpMove(0, stJOG.PosX, stJOG.PosY)                  ' ��stJOG.PosX,Y�̓_�~�[��
            '----- V1.20.0.1�A�� -----
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return�l = �G���[
            End If

            'Call CalcBlockSize(mdBSx, mdBSy)                            ' �u���b�N�T�C�YXY�ݒ�
            'Call BpMoveOrigin_Ex()                                      ' BP����ۯ��̉E��Ɉړ�����
            'r = Form1.System1.EX_MOVE(gSysPrm, 0.0, 0.0, 1)
            'If (r < cFRS_NORMAL) Then
            '    Return (r)                                              ' Return�l = �G���[
            'End If
            '----- V1.14.0.0�F�� -----

            ' �����(XYð��ق�JOG����)�p�p�����[�^������������
            stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
            stJOG.Md2 = MD2_BUTN                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
            stJOG.PosX = 0.0                                            ' BP X�ʒu/XYð��� X�ʒu
            stJOG.PosY = 0.0                                            ' BP Y�ʒu/XYð��� Y�ʒu
            '----- V1.14.0.0�F�� -----
            stJOG.BpOffX = 0.0                                          ' BP�̾��X = �����ڰ���ð��ٵ̾��X 
            stJOG.BpOffY = 0.0                                          ' BP�̾��Y = �����ڰ���ð��ٵ̾��Y
            'stJOG.BpOffX = typPlateInfo.dblBpOffSetXDir                 ' BP�̾��X 
            'stJOG.BpOffY = typPlateInfo.dblBpOffSetYDir                 ' BP�̾��Y 
            '----- V1.14.0.0�F�� -----
            stJOG.BszX = mdBSx                                          ' ��ۯ�����X 
            stJOG.BszY = mdBSy                                          ' ��ۯ�����Y
            stJOG.TextX = lblGap1X                                      ' X�ʒu�\���p���x��
            stJOG.TextY = lblGap1Y                                      ' Y�ʒu�\���p���x��
            stJOG.cgX = 0.0                                             ' �ړ���X 
            stJOG.cgY = 0.0                                             ' �ړ���Y 
            stJOG.BtnHI = BtnHI                                         ' HI�{�^��
            stJOG.BtnZ = BtnZ                                           ' Z�{�^��
            stJOG.BtnSTART = BtnSTART                                   ' START�{�^��
            stJOG.BtnHALT = BtnHALT                                     ' HALT�{�^��
            stJOG.BtnRESET = BtnRESET                                   ' RESET�{�^��
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()                                             ' �t�H�[�J�X��ݒ肷��(�e���L�[���͗p) 

STP_START:
            '-------------------------------------------------------------------
            '   �L�����u���[�V�������s�̂��߂ɏ\���J�b�g���s��
            '-------------------------------------------------------------------
            ' �V�X�e���G���[�`�F�b�N
            r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
            If (r <> cFRS_NORMAL) Then
                Return (r)
            End If

            ' �����J�����ɐؑւ���
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
#End If
            '' ����W1,2�̒����ֈړ�(�����J����)   �s�v ??
            'r = XYTableMoveCenter(dblStanderd1X, dblStanderd1Y, dblStanderd2X, dblStanderd2Y)
            'If (r <> cFRS_NORMAL) Then
            '    GoTo STP_END
            'End If
            SendCrossLineMsgToDispGazou()                               ' V6.1.4.18�@

            ' �R���\�[������(START/RESET�L�[���͑҂�)
            Call ZCONRST()                                              ' ���b�`����
            StpNum = 0                                                  ' StpNum = 0
            Me.lblInfo.Text = MSG_CALIBRATION_001                       ' �L�����u���[�V���������s���܂��B" & vbCrLf & "[START]�������Ă��������B"
            stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
            'V6.0.0.0�J            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me) 'V6.0.0.0�J
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����

            ' ���H��������͂���(FL��)
            '----- V1.14.0.0�G�� -----
            CondNum = typPlateInfo.intCaribCutCondNo                    ' ���H�����ԍ�
            'CondNum = 0                                                 ' ���H�����ԍ�
            '----- V1.14.0.0�G�� -----
            dQrate = typPlateInfo.dblCaribCutQRate                      ' �����ڰ���ڰ��Qڰ� 
            r = Sub_FlCond(CondNum, dQrate, Me)
            If (r <> cFRS_NORMAL) Then
                If (r = cFRS_ERR_RST) Then GoTo STP_START '             ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����
                Return (r)
            End If
            Me.Refresh()                                                ' �����H�������͉�ʕ\������������

            ' Intime����BP�L�����u���[�V�����l�𑗐M����(�Q�C���I�t�Z�b�g�̃N���A)
            dblGainX = 1
            dblGainY = 1
            dblOffsetX = 0
            dblOffsetY = 0
            r = BP_CALIBRATION(dblGainX, dblGainY, dblOffsetX, dblOffsetY)
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If

            ' �摜�\���v���O�������N������
            'V6.0.0.0�D            r = Exec_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)

            ' �L�����u���[�V�������s�̂��߂Ɋ���W1�Ɗ���W2�ɏ\���J�b�g���s��
            r = CalibrationCrossCut(typPlateInfo, CondNum, dQrate)      ' �\���J�b�g���s
            'V6.0.0.0�D            Call End_GazouProc(ObjProc)                                 ' �摜�\���v���O�������I������
            If (r <> cFRS_NORMAL) Then
                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����
                Return (r)                                              ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
            End If

STP_RETRY:
            '-------------------------------------------------------------------
            '   �O���J�����Ńp�^�[���F�����s���A����ʂ��Z�o����
            '-------------------------------------------------------------------
            ' �R���\�[������(START/RESET�L�[���͑҂�)
            StpNum = 1                                                  ' StpNum = 1
            Me.lblInfo.Text = MSG_CALIBRATION_002                       ' �O���J�����ł���ʂ����o���܂��B[START]�������Ă��������B[RESET]�F���~"
            'V6.0.0.0�J            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����
            Call gparModules.CrossLineDispOff()    'V4.10.0.0�B
            ' �L�����u���[�V��������(�O���J�����Ńp�^�[���F�����s���A����ʂ��Z�o����)
            r = CalibrationExec(typPlateInfo)
            If (r <> cFRS_NORMAL) Then
                ' �����J�����ɐؑւ���
#If VIDEO_CAPTURE = 0 Then
                Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
#End If
            End If

            Call Form1.VideoLibrary1.PatternDisp(False)                 ' �p�^�[���}�b�`���O���̌����͈͘g(���F�g�ƐF�g)���\���Ƃ���@###094

            Return (r)                                                  ' �L�����u���[�V���������I�� 

STP_END:
            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W��\������
            '-------------------------------------------------------------------
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H
            Call Form1.VideoLibrary1.PatternDisp(False)                 ' �p�^�[���}�b�`���O���̌����͈͘g(���F�g�ƐF�g)���\���Ƃ���@###094
            If gbAutoCalibration Then                                   'V6.1.4.2�@[�����L�����u���[�V�����␳���s]
                r = cFRS_NORMAL                                         'V6.1.4.2�@
            Else                                                        'V6.1.4.2�@
                rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
            End If                                                      'V6.1.4.2�@
            If (rtn = cFRS_ERR_RST) Then                                '�u�������v�I���Ȃ珈�����p������
                If (StpNum = 0) Then GoTo STP_START '                   ' �\���J�b�g������
                GoTo STP_RETRY                                          ' �p�^�[���F��������
            End If

            ' �����J�����ɐؑւ���
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
#End If
            Return (r)                                                  ' �L�����u���[�V���������I��(RESET(Cancel)�L�[)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.CalibrationMain() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�L�����u���[�V�����p�\���J�b�g����(����W1�Ɗ���W2�ɏ\���J�b�g���s��)"
    '''=========================================================================
    ''' <summary>�L�����u���[�V�����p�\���J�b�g����(����W1�Ɗ���W2�ɏ\���J�b�g���s��)</summary>
    ''' <param name="stPLT">  (INP)�v���[�g�f�[�^</param>
    ''' <param name="CondNum">(INP)���H�����ԍ�(FL�p)</param>
    ''' <param name="dQrate"> (INP)Q���[�g(KHz)</param>
    ''' <returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST  =Cancel(RESET��)
    '''         -1�ȉ�        =�G���[</returns>
    ''' <remarks>V1.14.0.0�F�őO�ʉ���A###104�A###105�̏C���͍폜</remarks>
    '''=========================================================================
    Private Function CalibrationCrossCut(ByRef stPLT As PlateInfo, ByVal CondNum As Integer, ByVal dQrate As Double) As Integer

        Dim r As Integer
        Dim Count As Integer
        Dim fPosOffX As Double
        Dim fPosOffY As Double
        Dim dblBpPosX As Double
        Dim dblBpPosY As Double
        Dim dblStdX(2) As Double
        Dim dblStdY(2) As Double
        Dim strMSG As String

        Try
            ' ��������
            fPosOffX = stPLT.dblCaribTableOffsetXDir                        ' �����ڰ���ð��ٵ̾��X
            fPosOffY = stPLT.dblCaribTableOffsetYDir                        ' �����ڰ���ð��ٵ̾��Y
            dblStdX(0) = stPLT.dblCaribBaseCordnt1XDir                      ' �����ڰ��݊���W1X
            dblStdY(0) = stPLT.dblCaribBaseCordnt1YDir                      ' �����ڰ��݊���W1Y
            dblStdX(1) = stPLT.dblCaribBaseCordnt2XDir                      ' �����ڰ��݊���W2X
            dblStdY(1) = stPLT.dblCaribBaseCordnt2YDir                      ' �����ڰ��݊���W2Y

            ' ����W�P�Ɗ���W�Q�ɏ\���J�b�g���s��
            For Count = 0 To 1
                ' "�y�\���J�b�g���[�h(����Wx)�z" + "�y�x���z[START�L�[]: �\���J�b�g���s , [RESET�L�[]: ���~"
                If (Count = 0) Then
                    strMSG = LBL_CALIBRATION_001 + vbCrLf
                Else
                    strMSG = LBL_CALIBRATION_002 + vbCrLf
                End If
                lblInfo.Text = strMSG + MSG_CUT_POS_CORRECT_004
                lblInfo.Refresh()

                ' BP�̃J�b�g�ʒu�����̍��W�����߂�(X���W���J�b�g����1/2�߂�)
                'dblBpPosX = dblStdX(Count) - (stPLT.dblCaribCutLength / 2) + fPosOffX
                dblBpPosX = dblStdX(Count) + fPosOffX
                dblBpPosY = dblStdY(Count) + fPosOffY

                ' BP���\���J�b�g�ʒu�̒����ֈړ����� 
                'r = Form1.System1.EX_MOVE(gSysPrm, dblBpPosX, dblBpPosY, 1) ' BP��Βl�ړ� V1.20.0.1�A
                r = CaribBpMove(Count, dblBpPosX, dblBpPosY)                ' V1.20.0.1�A
                If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?   
                    GoTo STP_END
                End If
                Call gparModules.DispCrossLine(0, 0)    'V5.0.0.6�K
                ' �\���J�b�g�����s����(����W�P�܂��͊���W�Q)
                r = Sub_CalibrationCrossCut(dblBpPosX, dblBpPosY, CondNum, dQrate, stPLT.dblCaribCutLength, stPLT.dblCaribCutSpeed)
                If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                    If (r = cFRS_ERR_RST) Then
                        Count = Count - 1
                        Continue For
                    ElseIf r < cFRS_NORMAL Then ' V3.0.0.0�B ADD START
                        Return (r)              ' V3.0.0.0�B �\�t�g�����I�����x���̃G���[
                    End If                      ' V3.0.0.0�B
                End If
            Next Count

            ' �I������
STP_END:
            Return (r)                                                      ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.CalibrationCrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                              ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�L�����u���[�V�����p�\���J�b�g�T�u���[�`��"
    '''=========================================================================
    '''<summary>�L�����u���[�V�����p�\���J�b�g�T�u���[�`��</summary>
    ''' <param name="dblBpPosX">   (INP)�J�b�g�ʒuX</param>
    ''' <param name="dblBpPosY">   (INP)�J�b�g�ʒuY</param>
    ''' <param name="CondNum">     (INP)���H�����ԍ�(FL�p)</param>
    ''' <param name="dQrate">      (INP)Q���[�g(KHz)</param>
    ''' <param name="dblCutLength">(INP)�J�b�g��</param>
    ''' <param name="dblCutSpeed"> (INP)�J�b�g���x</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    ''' <remarks>�\����Ă̒�����BP���ړ����Ă�������</remarks>
    '''=========================================================================
    Private Function Sub_CalibrationCrossCut(ByVal dblBpPosX As Double, ByVal dblBpPosY As Double, ByVal CondNum As Integer,
                                             ByVal dQrate As Double, ByVal dblCutLength As Double, ByVal dblCutSpeed As Double) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �R���\�[������(START/RESET�L�[���͑҂�)
            stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
            'V6.0.0.0�J            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            If (r = cFRS_ERR_RST) Then
                ' ###078  �@�� 
                ' Reset�������ꂽ�ꍇ�ɁA�{����Reset���邩�̃��b�Z�[�W��\������B
                Me.lblInfo.Text = MSG_CALIBRATION_008                       ' "Reset���܂��B [Start]:Reset���s , [Reset]:Cancel���s "     
                stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                ' ###078  �@�� 
                Return (r)
            End If

            ' �\���J�b�g�����s����
            lblInfo.Text = MSG_CUT_POS_CORRECT_005                      ' "�\���J�b�g���s��"
            r = CrossCutExec(dblBpPosX, dblBpPosY, CondNum, dQrate, dblCutLength, dblCutSpeed)
            lblInfo.Text = ""
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.Sub_CalibrationCrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�L�����u���[�V��������(�O���J�����Ńp�^�[���F�����s���A����ʂ��Z�o����)"
    '''=========================================================================
    ''' <summary>�L�����u���[�V��������(�O���J�����Ńp�^�[���F�����s���A����ʂ��Z�o����)</summary>
    ''' <param name="stPLT">(INP)�v���[�g�f�[�^</param>
    ''' <returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST  =Cancel(RESET��)
    '''         -1�ȉ�        =�G���[</returns>
    '''=========================================================================
    Private Function CalibrationExec(ByRef stPLT As PlateInfo) As Integer

        Dim r As Integer
        Dim rtn As Integer
        Dim Count As Integer
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim dblCalSizeX As Double
        Dim dblCalSizeY As Double
        Dim PosX(2) As Double
        Dim PosY(2) As Double
        Dim GapX(2) As Double
        Dim GapY(2) As Double
        Dim GrpNo(2) As Short
        Dim TmpNo(2) As Short
        Dim LblGapX(2) As System.Windows.Forms.Label
        Dim LblGapY(2) As System.Windows.Forms.Label
        Dim fStgPosX As Double
        Dim fStgPosY As Double
        Dim fCaribOfsX As Double                                        ' V3.0.0.0�E(V1.22.0.0�F)
        Dim fCaribOfsY As Double                                        ' V3.0.0.0�E(V1.22.0.0�F)

        Dim dblGainX As Double                                          ' �޲ݕ␳�W��
        Dim dblGainY As Double
        Dim dblOffsetX As Double                                        ' �̾�ĕ␳�W��
        Dim dblOffsetY As Double
        Dim fcoeff As Double                                            ' ���֒l
        Dim strMSG As String = Nothing

        Try
            '-------------------------------------------------------------------
            '   �������� 
            '-------------------------------------------------------------------
            Call CalcBlockSize(dblBSX, dblBSY)                          ' ��ۯ����ގZ�o

            ' ����W1�Ɗ���W2�̒��ԍ��W�����߂�
            dblCalSizeX = System.Math.Abs(stPLT.dblCaribBaseCordnt2XDir - stPLT.dblCaribBaseCordnt1XDir)
            dblCalSizeY = System.Math.Abs(stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir)

            Call Form1.System1.EX_BPOFF(gSysPrm, 0.0#, 0.0#)            ' BP�̾�Ăɶ�Ĉʒu�␳�̾�Đݒ�
            Call Form1.System1.EX_MOVE(gSysPrm, 0.0#, 0.0#, 1)          ' �ŏ��̶�Ĉʒu�����ֈړ�

            ' �\���J�b�g���W�擾
            PosX(0) = stPLT.dblCaribBaseCordnt1XDir                     ' ����W�P
            PosY(0) = stPLT.dblCaribBaseCordnt1YDir
            '----- V1.14.0.0�F�� -----
            ' ����W�Q
            PosX(1) = stPLT.dblCaribBaseCordnt2XDir
            PosY(1) = stPLT.dblCaribBaseCordnt2YDir
            'If (stPLT.intResistDir = 0) Then                            ' ���ߕ��ѕ�����X���� ?
            '    PosX(1) = stPLT.dblCaribBaseCordnt2XDir                 ' ����W�Q
            '    PosY(1) = stPLT.dblCaribBaseCordnt1YDir
            'Else
            '    PosX(1) = stPLT.dblCaribBaseCordnt1XDir
            '    PosY(1) = stPLT.dblCaribBaseCordnt2YDir
            'End If
            '----- V1.14.0.0�F�� -----
            GrpNo(0) = stPLT.intCaribPtnNo1GroupNo                      ' �O���[�v�ԍ�1
            TmpNo(0) = stPLT.intCaribPtnNo1                             ' �p�^�[���ԍ�1
            GrpNo(1) = stPLT.intCaribPtnNo2GroupNo                      ' �O���[�v�ԍ�2
            TmpNo(1) = stPLT.intCaribPtnNo2                             ' �p�^�[���ԍ�2
            GapX(0) = 0.0                                               ' �����1������
            GapY(0) = 0.0
            GapX(1) = 0.0                                               ' �����2������
            GapY(1) = 0.0
            LblGapX(0) = lblGap1X                                       ' �����1X�\���p���x��
            LblGapY(0) = lblGap1Y                                       ' �����1Y�\���p���x��
            LblGapX(1) = lblGap2X                                       ' �����2X�\���p���x��
            LblGapY(1) = lblGap2Y                                       ' �����2Y�\���p���x��

            ' �O���J�����ɐؑւ���
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)
#End If

STP_RETRY:
            '-------------------------------------------------------------------
            '   ����W�P�Ɗ���W�Q�̃p�^�[���}�b�`���O���s������ʂ��Z�o����
            '-------------------------------------------------------------------
            For Count = 0 To 1                                          ' ����W�P�Ɗ���W�Q�̕��A�J��Ԃ� 
                ' �K�C�_���X���b�Z�[�W�\��
                If (Count = 0) Then
                    strMSG = LBL_CALIBRATION_003 + vbCrLf
                Else
                    strMSG = LBL_CALIBRATION_004 + vbCrLf
                End If
                lblInfo.Text = strMSG + MSG_CALIBRATION_003             ' "�y�摜�F�����[�h(����Wx)�z" + "[START]: �摜�F�����s , [RESET]: ���~"
                lblInfo.Refresh()

                'V6.0.0.0-28                ' �ړ��O��ɉ摜�X�V�����͒�~����B
                'V6.0.0.0-28                Call Form1.VideoLibrary1.VideoStop()

                '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0 ' �E��(x��, y��)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir
                    Case 1 ' ����(x��, y��)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir * -1
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir
                    Case 2 ' �E��(x��, y��)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir * -1
                    Case 3 ' ����(x��, y��)
                        fCaribOfsX = typPlateInfo.dblCaribTableOffsetXDir * -1
                        fCaribOfsY = typPlateInfo.dblCaribTableOffsetYDir * -1
                End Select
                'V4.7.1.0�@�� PATTERN_CORRECT_MODE�i=0:BP/XYð��قƂ��������j���[�h�̎��́A�O���J�����ŃI�t�Z�b�g�����Z���Ă͑ʖ�
                If gSysPrm.stSPF.giPatternCorectMode = 0 Then
                    fCaribOfsX = 0.0
                    fCaribOfsY = 0.0
                End If
                '                V4.7.1.0�@��
                ' ����W�P�܂��͂Q�փe�[�u�����Βl�ړ�����(�O���J������)            ''###106
                r = XYTableMoveExt(PosX(Count) + fCaribOfsX, PosY(Count) + fCaribOfsY)
                'r = XYTableMoveExt(PosX(Count) + stPLT.dblCaribTableOffsetXDir, PosY(Count) + stPLT.dblCaribTableOffsetYDir)
                ' r = XYTableMoveExt(PosX(Count) + stPLT.dblCaribTableOffsetXDir + typPlateInfo.dblTableOffsetXDir,PosY(Count) + stPLT.dblCaribTableOffsetYDir + typPlateInfo.dblTableOffsetYDir)
                '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
                If (r <> cFRS_NORMAL) Then
                    Return (r)
                End If

                ' �e�[�u�����莞�� Wait
                If (gPrevInterlockSw = 0) Then
                    Call System.Threading.Thread.Sleep(100)             ' �ʏ퓮�쎞��ð��و��莞�� Wait(msec)
                Else
                    Call System.Threading.Thread.Sleep(200)             ' �ᑬ���쎞��ð��و��莞�� Wait(msec)
                End If

                'V6.0.0.0-28                ' �ړ��O��ɉ摜�X�V�����͍ĊJ����B
                'V6.0.0.0-28                Call Form1.VideoLibrary1.VideoStart()

                ' �R���\�[������(START/RESET�L�[���͑҂�)
                Call ZCONRST()                                          ' ���b�`����
                stJOG.Md = MODE_KEY                                     ' ���[�h(�L�[���͑҂����[�h)
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START         ' �L�[�̗L��(1)/����(0)�w��
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)      'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    Return (r)
                End If
                '                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����
                '###095
                If (r = cFRS_ERR_RST) Then
                    ' Reset�������ꂽ�ꍇ�ɁA�{����Reset���邩�̃��b�Z�[�W��\������B
                    Me.lblInfo.Text = MSG_CALIBRATION_008                       ' "Reset���܂��B [Start]:Reset���s , [Reset]:Cancel���s "     
                    stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
                    stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
                    'V6.0.0.0�J                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                    If (r = cFRS_ERR_START) Then
                        GoTo STP_END '               ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����
                    ElseIf (r = cFRS_ERR_RST) Then
                        Count = Count - 1
                        Continue For
                    End If
                End If
                ' ###095  �@�� 

                ' �O���J�����Ńp�^�[���F�����s��
                r = Sub_PatternMatching(GrpNo(Count), TmpNo(Count), GapX(Count), GapY(Count), fcoeff)
                If (r <> cFRS_NORMAL) Then
                    If (r = cFRS_ERR_PTN) Or (r = cFRS_ERR_PT2) Then
                        '"�摜�}�b�`���O�G���[" "�����𑱂���ꍇ��[OK]���A���~����ꍇ��[Cancel]�������ĉ������B"
                        '----- V1.14.0.0�F�� -----
                        GapX(Count) = 0.0
                        GapY(Count) = 0.0
                        If (r = cFRS_ERR_PT2) Then
                            strMSG = MSG_CALIBRATION_007 + " (Thresh =" + fcoeff.ToString("0.00") + ")"
                        Else
                            strMSG = MSG_CALIBRATION_007
                        End If
                        'V6.1.4.2�@��
                        If gbAutoCalibration Then
                            strMSG = "�����L�����u���[�V�����␳ �p�^�[���}�b�`���O�G���[[" & r.ToString & "] Thresh =[" & fcoeff.ToString("0.00") & "]"
                            Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                            Form1.Z_PRINT(strMSG)
                            gbAutoCalibrationResult = False
                            r = cFRS_ERR_RST
                            GoTo STP_EXIT
                        End If
                        'V6.1.4.2�@��
                        rtn = Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
                        'rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_CALIBRATION_007, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
                        '----- V1.14.0.0�F�� -----
                        If (rtn = cFRS_ERR_RST) Then                    '�u�������v�I���Ȃ�I��
                            ' ###077 ��
                            r = cFRS_NORMAL
                            GoTo STP_END
                            ' ###077 ��
                        End If
                    Else
                        Return (r)
                    End If
                End If

                '-------------------------------------------------------------------
                '   ���L�[�ɂ��e�[�u������(�O���J������)�ł����1,2�𒲐�����
                '-------------------------------------------------------------------
                ' �K�C�_���X���b�Z�[�W�\��
                If (Count = 0) Then
                    strMSG = MSG_CALIBRATION_004                        ' "�O���J�����ł���ʂ����o���܂��B(����W�P)" & vbCrLf & "[START]�F����C[RESET]�F���~"
                Else
                    strMSG = MSG_CALIBRATION_005                        ' "�O���J�����ł���ʂ����o���܂��B(����W�Q)" & vbCrLf & "[START]�F����C[RESET]�F���~"
                End If
                lblInfo.Text = strMSG
                lblInfo.Refresh()

                'Call ZGETPHPOS(fStgPosX, fStgPosY)                      ' X,Y�e�[�u���ʒu�擾 V1.14.0.0�F
                Call ZGETPHPOS2(fStgPosX, fStgPosY)                      ' X,Y�e�[�u���ʒu�擾 V1.14.0.0�F
                stJOG.Md = MODE_STG                                     ' ���[�h(XY�e�[�u�����[�h)
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START         ' �L�[�̗L��(1)/����(0)�w��
                stJOG.PosX = fStgPosX                                   ' �e�[�u��X���W
                stJOG.PosY = fStgPosY                                   ' �e�[�u��Y���W
                stJOG.cgX = GapX(Count)                                 ' �ړ���X(�����X)
                stJOG.cgY = GapY(Count)                                 ' �ړ���Y(�����Y)
                stJOG.TextX = LblGapX(Count)                            ' �����X�\���p���x��
                stJOG.TextY = LblGapY(Count)                            ' �����Y�\���p���x��
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)      'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    Return (r)
                End If
                '###095
                If (r = cFRS_ERR_RST) Then
                    ' Reset�������ꂽ�ꍇ�ɁA�{����Reset���邩�̃��b�Z�[�W��\������B
                    Me.lblInfo.Text = MSG_CALIBRATION_008                       ' "Reset���܂��B [Start]:Reset���s , [Reset]:Cancel���s "     
                    stJOG.Md = MODE_KEY                                         ' ���[�h(�L�[���͑҂����[�h)
                    stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
                    'V6.0.0.0�J                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                    r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                    If (r = cFRS_ERR_START) Then
                        GoTo STP_END '               ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����
                    ElseIf (r = cFRS_ERR_RST) Then
                        Count = Count - 1
                        Continue For
                    End If
                End If
                ' ###095  �@�� 
                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' RESET�L�[�����Ȃ�I���m�F���b�Z�[�W�\����

                ' ����ʂ�ޔ�����
                GapX(Count) = stJOG.cgX                                 ' �ړ���X(�����X)
                GapY(Count) = stJOG.cgY                                 ' �ړ���Y(�����Y)

            Next Count

            '-------------------------------------------------------------------
            '   �Q�C���ƃI�t�Z�b�g���Z�o����
            '-------------------------------------------------------------------
            ' �Q�C���ƃI�t�Z�b�g���Z�o����
            Call CalcGainOffset(stPLT, GapX(0), GapY(0), GapX(1), GapY(1), dblGainX, dblGainY, dblOffsetX, dblOffsetY)
            'V6.1.4.2�@��
            If gbAutoCalibration Then
                Dim GapLimit As Double = Double.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CARIBRATION_RECOG_LIMIT", "C:\TRIM\tky.ini", "0"))
                For i As Integer = 0 To 1
                    If System.Math.Abs(GapX(i)) > GapLimit Then
                        strMSG = "�����L�����u���[�V�����␳�@���~�b�g�G���[ X[" & (i + 1).ToString("0") & "] OFFSET LIMIT ERROR=[" & GapX(i).ToString("0.000") & "]>[" & GapLimit.ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        Form1.Z_PRINT(strMSG)
                        gbAutoCalibrationResult = False
                        r = cFRS_ERR_RST
                        strMSG = "���~�b�g�G���[ GapX(0)=[" & GapX(0).ToString("0.000") & "] GapY(0)=[" & GapY(0).ToString("0.000") & "] GapX(1)=[" & GapX(1).ToString("0.000") & "] GapY(1)=[" & GapY(1).ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        GoTo STP_EXIT
                    End If
                    If System.Math.Abs(GapY(i)) > GapLimit Then
                        strMSG = "�����L�����u���[�V�����␳�@���~�b�g�G���[ Y[" & (i + 1).ToString("0") & "] OFFSET LIMIT ERROR=[" & GapY(i).ToString("0.000") & "]>[" & GapLimit.ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        Form1.Z_PRINT(strMSG)
                        gbAutoCalibrationResult = False
                        r = cFRS_ERR_RST
                        strMSG = "���~�b�g�G���[ GapX(0)=[" & GapX(0).ToString("0.000") & "] GapY(0)=[" & GapY(0).ToString("0.000") & "] GapX(1)=[" & GapX(1).ToString("0.000") & "] GapY(1)=[" & GapY(1).ToString("0.000") & "]"
                        Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                        GoTo STP_EXIT
                    End If
                Next
            End If
            'V6.1.4.2�@��

            ' �Q�C���ƃI�t�Z�b�g��\������ 
            lblGainX.Text = dblGainX.ToString("0.0000")
            lblGainY.Text = dblGainY.ToString("0.0000")
            lblOffsetX.Text = dblOffsetX.ToString("0.0000")
            lblOffsetY.Text = dblOffsetY.ToString("0.0000")

            If gbAutoCalibration Then                                   'V6.1.4.2�@[�����L�����u���[�V�����␳���s]
                frmTitle.Refresh()                                      'V6.1.4.2�@
                r = cFRS_NORMAL                                         'V6.1.4.2�@
            Else                                                        'V6.1.4.2�@
                '"�L�����u���[�V�������I�����܂��B" & "�f�[�^��ێ�����ꍇ��[OK]���A�f�[�^��ێ����Ȃ��ꍇ��[Cancel]�������ĉ������B"
                rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_CALIBRATION_006, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
            End If                                                      'V6.1.4.2�@
            If (rtn = cFRS_ERR_RST) Then                                '�u�������v�I���Ȃ珈�����I������
                GoTo STP_EXIT
            End If

            ' Intime����BP�L�����u���[�V�����l�𑗐M����
            'V1.20.0.0�F��
            dblGainX = dblGainX * dblCalibHoseiX
            'V1.20.0.0�F��
            r = BP_CALIBRATION(dblGainX, dblGainY, dblOffsetX, dblOffsetY)
            'V6.1.4.2�@��
            If r <> cFRS_NORMAL Then
                gbAutoCalibrationResult = False
            Else
                strMSG = "CalGainX=[" & dblGainX.ToString("0.0000") & "] CalGainY=[" & dblGainY.ToString("0.0000") & "] CalOffX=[" & dblOffsetX.ToString("0.0000") & "] CalOffY=[" & dblOffsetY.ToString("0.0000") & "]"
                Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                Form1.Z_PRINT(strMSG)
            End If
            'V6.1.4.2�@��
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            Dim OffsetLimit As Double = Double.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CARIBRATION_OFFSET_LIMIT", "C:\TRIM\tky.ini", "0"))
            If dblOffsetY > OffsetLimit Or dblOffsetX > OffsetLimit Then
                ' '' ''If System.Math.Abs(dblOffsetY) > OffsetLimit Then
                ' '' ''    strMSG = "�I�t�Z�b�g�␳�ʂx=[" & dblOffsetY.ToString("0.000") & "]��[" & OffsetLimit.ToString("0.000") & "](mm)�𒴂��܂����B"
                ' '' ''    Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                ' '' ''    Form1.Z_PRINT(strMSG)
                ' '' ''    Form1.LabelAutoCalibLimit.Text = strMSG
                ' '' ''    Form1.LabelAutoCalibLimit.Visible = True
                ' '' ''End If
                If System.Math.Abs(dblOffsetX) > OffsetLimit Then
                    strMSG = "�I�t�Z�b�g�␳�ʂw=[" & dblOffsetX.ToString("0.000") & "]��[" & OffsetLimit.ToString("0.000") & "](mm)�𒴂��܂����B"
                    Call Form1.System1.OperationLogging(gSysPrm, strMSG, "CALIBRATION")
                    Form1.Z_PRINT(strMSG)
                    Form1.LabelAutoCalibLimit.Text = strMSG
                    Form1.LabelAutoCalibLimit.Visible = True
                End If

            Else
                Form1.LabelAutoCalibLimit.Visible = False
            End If



            GoTo STP_EXIT                                               ' �����I���� 

STP_END:
            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W��\������
            '-------------------------------------------------------------------
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            If gbAutoCalibration Then                                   'V6.1.4.2�@[�����L�����u���[�V�����␳���s]
                r = cFRS_NORMAL                                         'V6.1.4.2�@
            Else                                                        'V6.1.4.2�@
                rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_CALIBRATION_START)
            End If                                                      'V6.1.4.2�@
            If (rtn = cFRS_ERR_RST) Then                                '�u�������v�I���Ȃ珈�����p������
                GoTo STP_RETRY
            End If

STP_EXIT:
            ' �����J�����ɐؑւ���
#If VIDEO_CAPTURE = 0 Then
            Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                    ' �����J�����ɐؑւ���
#End If
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.CalibrationExec() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�O���J�����̎w����W�փe�[�u�����Βl�ړ�����"
    '''=========================================================================
    '''<summary>�O���J�����̊���W��XY�e�[�u�����ړ�����</summary>
    ''' <param name="dblStdX">(INP)����WX�ʒu</param>
    ''' <param name="dblStdY">(INP)����WY�ʒu</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function XYTableMoveExt(ByVal dblStdX As Double, ByVal dblStdY As Double) As Integer

        Dim r As Integer
        Dim dblX As Double
        Dim dblY As Double
        Dim dblBPX As Double
        Dim dblBPY As Double
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim strMSG As String

        Try
            ' ��������
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                         ' �g�����|�W�V����X                             '
            dblTrimPosY = gSysPrm.stDEV.gfTrimY                         ' �g�����|�W�V����Y
            Del_x = gfCorrectPosX                                       ' �g�����|�W�V�����␳�lX(�ƕ␳���s���ɐݒ�) 
            Del_y = gfCorrectPosY                                       ' �g�����|�W�V�����␳�lY(�ƕ␳���s���ɐݒ�) 
            dblBPX = dblStdX                                            ' ����WX�ʒu
            dblBPY = dblStdY                                            ' ����WY�ʒu

            ' ���ݸވʒu���W + �����ڰ���ð��ٵ̾�� + �␳�ʒu  (+or-) ð��ٕ␳��
            Select Case (gSysPrm.stDEV.giBpDirXy)
                Case 0      ' x��, y��
                    dblX = dblTrimPosX + Del_x + dblBPX
                    dblY = dblTrimPosY + Del_y + dblBPY
                Case 1      ' x��, y��
                    dblX = dblTrimPosX + Del_x - dblBPX
                    dblY = dblTrimPosY + Del_y + dblBPY
                Case 2      ' x��, y��
                    dblX = dblTrimPosX + Del_x + dblBPX
                    dblY = dblTrimPosY + Del_y - dblBPY
                Case 3      ' x��, y��
                    dblX = dblTrimPosX + Del_x - dblBPX
                    dblY = dblTrimPosY + Del_y - dblBPY
            End Select

            ' �w�x�e�[�u���ړ�
            dblX = dblX + gSysPrm.stDEV.gfExCmX                         ' �O���J�����I�t�Z�b�g�ʒu�����Z
            dblY = dblY + gSysPrm.stDEV.gfExCmY
            r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)          ' �w�x�e�[�u���ړ�(��Βl)
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.XYTableMoveExt() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- V1.20.0.1�A�� -----
#Region "����W�ʒu1���͊���W�ʒu2��BP���ړ�����"
    '''=========================================================================
    ''' <summary>����W�ʒu1���͊���W�ʒu2��BP���ړ�����</summary>
    ''' <param name="Idx">   (INP)0=����W�ʒu1, 1=����W�ʒu2</param>
    ''' <param name="dblBPX">(OUT)����W�ʒuX</param>
    ''' <param name="dblBPY">(OUT)����W�ʒuY</param>
    ''' <returns>cFRS_NORMAL = ����
    '''          ��L�ȊO  = �G���[</returns>
    '''=========================================================================
    Private Function CaribBpMove(ByVal Idx As Integer, ByRef dblBPX As Double, ByRef dblBPY As Double) As Integer

        Dim r As Integer
        Dim BpOffX As Double = 0.0
        Dim BpOffY As Double = 0.0
        Dim strMSG As String
        Dim dblOffsetXDir As Double 'V4.7.1.0�@�����ڰ���ð��ٵ̾��X
        Dim dblOffsetYDir As Double 'V4.7.1.0�@�����ڰ���ð��ٵ̾��Y

        'V4.7.1.0�@�� PATTERN_CORRECT_MODE�i=0:BP/XYð��قƂ��������j���[�h�̎��́A�O���J�����ŃI�t�Z�b�g�����Z���Ă͑ʖ�
        If gSysPrm.stSPF.giPatternCorectMode = 0 Then
            dblOffsetXDir = typPlateInfo.dblCaribTableOffsetXDir
            dblOffsetYDir = typPlateInfo.dblCaribTableOffsetYDir
        Else
            dblOffsetXDir = 0.0
            dblOffsetYDir = 0.0
        End If
        'V4.7.1.0�@�� 

        Try
            ' ����W1,2��BP���W�����߂�
            '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
            If (Idx = 0) Then                                           ' ����W�ʒu1 ?
                If (typPlateInfo.intResistDir = 0) Then                 ' ��R(����)���ѕ���(0:X, 1:Y)
                    dblBPX = typPlateInfo.dblCaribBaseCordnt1XDir
                    dblBPY = 0.0
                Else
                    dblBPX = 0.0
                    dblBPY = typPlateInfo.dblCaribBaseCordnt1YDir
                End If

            Else                                                        ' ����W�ʒu2
                If (typPlateInfo.intResistDir = 0) Then                 ' ��R(����)���ѕ���(0:X, 1:Y)
                    dblBPX = typPlateInfo.dblCaribBaseCordnt2XDir
                    dblBPY = 0.0
                    BpOffX = 0.0
                    BpOffY = typPlateInfo.dblCaribBaseCordnt1YDir - typPlateInfo.dblCaribBaseCordnt2YDir
                Else
                    dblBPX = 0.0
                    dblBPY = typPlateInfo.dblCaribBaseCordnt2YDir
                    BpOffX = typPlateInfo.dblCaribBaseCordnt1XDir - typPlateInfo.dblCaribBaseCordnt2XDir
                    BpOffY = 0.0
                End If
            End If
            r = BPOFF(BpOffX, BpOffY)

            'If (Idx = 0) Then
            '    dblBPX = typPlateInfo.dblCaribBaseCordnt1XDir
            '    dblBPY = typPlateInfo.dblCaribBaseCordnt1YDir
            'Else
            '    dblBPX = typPlateInfo.dblCaribBaseCordnt2XDir
            '    dblBPY = typPlateInfo.dblCaribBaseCordnt2YDir
            'End If
            '----- V3.0.0.0�E(V1.22.0.0�F)�� -----

            ' BP�ɃL�����u���[�V�����e�[�u���I�t�Z�b�g�����Z(�e�[�u��(x��, y��)�Ƃ͔��Ε���)
            '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
            'V4.7.1.0�@���R�����g�A�E�g
            'Select Case (gSysPrm.stDEV.giBpDirXy)
            '    Case 0 ' �E��(x��, y��)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' ��R(����)���ѕ���(0:X, 1:Y)
            '            dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        End If
            '    Case 1 ' ����(x��, y��)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' ��R(����)���ѕ���(0:X, 1:Y)
            '            dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        End If
            '    Case 2 ' �E��(x��, y��)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' ��R(����)���ѕ���(0:X, 1:Y)
            '            dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        End If
            '    Case 3 ' ����(x��, y��)
            '        If (typPlateInfo.intResistDir = 0) Then                 ' ��R(����)���ѕ���(0:X, 1:Y)
            '            dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            'dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        Else
            '            'dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '            dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        End If

            '        'Case 0 ' �E��(x��, y��)
            '        '    dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        'Case 1 ' ����(x��, y��)
            '        '    dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY + typPlateInfo.dblCaribTableOffsetYDir
            '        'Case 2 ' �E��(x��, y��)
            '        '    dblBPX = dblBPX + typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            '        'Case 3 ' ����(x��, y��)
            '        '    dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            '        '    dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            'End Select
            'V4.7.1.0�@���R�����g�A�E�g
            '----- V3.0.0.0�E(V1.22.0.0�F)�� -----

            ' BP���\���J�b�g�ʒu�̒����ֈړ�����(��Βl�ړ�)
            'V4.7.1.0�@��
            dblBPX = dblBPX - typPlateInfo.dblCaribTableOffsetXDir
            dblBPY = dblBPY - typPlateInfo.dblCaribTableOffsetYDir
            'V4.7.1.0�@��
            r = Form1.System1.EX_MOVE(gSysPrm, dblBPX, dblBPY, 1)

            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.CaribBpMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- V1.20.0.1�A�� -----
#Region "�Q�C���ƃI�t�Z�b�g���Z�o����(�l�̌ܓ��t��)"
    '''=========================================================================
    ''' <summary>�Q�C���ƃI�t�Z�b�g���Z�o����(�l�̌ܓ��t��)</summary>
    ''' <param name="stPLT">     (INP)�v���[�g�f�[�^</param>
    ''' <param name="dblGap1X">  (INP)�����1X</param>
    ''' <param name="dblGap1Y">  (INP)�����1Y</param>
    ''' <param name="dblGap2X">  (INP)�����2X</param>
    ''' <param name="dblGap2Y">  (INP)�����2Y</param>
    ''' <param name="dblGainX">  (OUT)�Q�C��X�␳�W��</param>
    ''' <param name="dblGainY">  (OUT)�Q�C��Y�␳�W��</param>
    ''' <param name="dblOffsetX">(OUT)�I�t�Z�b�gX�␳�W��</param>
    ''' <param name="dblOffsetY">(OUT)�I�t�Z�b�gY�␳�W��</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub CalcGainOffset(ByRef stPLT As PlateInfo, ByRef dblGap1X As Double, ByRef dblGap1Y As Double, ByRef dblGap2X As Double, ByRef dblGap2Y As Double, _
                               ByRef dblGainX As Double, ByRef dblGainY As Double, ByRef dblOffsetX As Double, ByRef dblOffsetY As Double)

        Dim r As Integer
        'Dim dblTOffsX As Double
        'Dim dblTOffsY As Double
        'Dim dblOf1 As Double
        'Dim dblOf2 As Double
        Dim dblNumerator As Double                                      ' ���q
        Dim dblDenominator As Double                                    ' ����
        Dim strMSG As String
        Dim dblCurGain(1) As Double
        Dim dblCurCalibOff(1) As Double

        Try
            ' INTRIM�ɐݒ肳��Ă��錻��̃L�����u���[�V�����␮�l���擾
            r = BP_GET_CALIBDATA(dblCurGain(0), dblCurGain(1), dblCurCalibOff(0), dblCurCalibOff(1))

            If (stPLT.intResistDir = 0) Then                            ' �`�b�v���ѕ��� = X���� ?
                ' �Q�C��X
                dblNumerator = stPLT.dblCaribBaseCordnt2XDir - stPLT.dblCaribBaseCordnt1XDir
                dblDenominator = (stPLT.dblCaribBaseCordnt2XDir + dblGap2X) - (stPLT.dblCaribBaseCordnt1XDir + dblGap1X)
                '----- V1.14.0.0�F�� -----
                If (0.0# = dblDenominator) Then                         ' ���� 0?
                    dblDenominator = 1.0#
                End If
                '----- V1.14.0.0�F�� -----
                ' �Q�C���␮�l�����ɖ߂��B
                dblDenominator = dblDenominator / dblCurGain(0)
                ' �␮�l
                'V4.7.0.0�O                dblGainX = RoundOff(dblDenominator / dblNumerator)
                dblGainX = RoundOff(dblNumerator / dblDenominator)  'V4.7.0.0�O

                ' �Q�C��Y
                dblGainY = 1.0#

                ' '' '' '' �I�t�Z�b�gX
                '' '' ''dblOf1 = (stPLT.dblCaribBaseCordnt1XDir + dblGap1X) * stPLT.dblCaribBaseCordnt2XDir
                '' '' ''dblOf2 = (stPLT.dblCaribBaseCordnt2XDir + dblGap2X) * stPLT.dblCaribBaseCordnt2XDir
                '' '' ''dblOffsetX = RoundOff((dblOf1 - dblOf2) / dblNumerator)

                ' '' '' '' �I�t�Z�b�gY
                '' '' ''dblOffsetY = RoundOff((-dblGap1Y - dblGap2Y) / 2)
            Else
                '(2012/02/23)
                '   �Ȃ��L�����u���[�V�����̌v�Z�ɂ����āAXY�Ōv�Z���@���قȂ邩�s���B(minato)

                ' �Q�C��X
                dblGainX = 1.0#

                ' �Q�C��Y
                dblNumerator = stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir
                '----- V1.14.0.0�F�� -----
                If (gSysPrm.stDEV.giBpDirXy = 0) Or (gSysPrm.stDEV.giBpDirXy = 1) Then
                    dblGap1Y = dblGap1Y * -1
                    dblGap2Y = dblGap2Y * -1
                    '    dblDenominator = (stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir) - (dblGap2Y * -1 - dblGap1Y * -1)
                    'Else
                    '    dblDenominator = (stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir) - (dblGap2Y - dblGap1Y)
                End If
                dblDenominator = (stPLT.dblCaribBaseCordnt2YDir - stPLT.dblCaribBaseCordnt1YDir) - (dblGap2Y - dblGap1Y)
                '----- V1.14.0.0�F�� -----
                If (0.0# = dblDenominator) Then                         ' ���� 0?
                    dblDenominator = 1.0#
                End If
                ' �Q�C���␮�l�����ɖ߂��B
                dblDenominator = dblDenominator / dblCurGain(1)
                ' �␮�l
                dblGainY = RoundOff(dblNumerator / dblDenominator)

                '' '' '' �I�t�Z�b�gX
                ' '' ''dblOffsetX = RoundOff((-dblGap1X - dblGap2X) / 2)

                '' '' '' �I�t�Z�b�gY
                ' '' ''dblOf1 = (stPLT.dblCaribBaseCordnt1YDir + dblTOffsY) * -dblGap2Y * dblGainY
                ' '' ''dblOf2 = (stPLT.dblCaribBaseCordnt2YDir + dblTOffsY) * -dblGap1Y * dblGainY
                ' '' ''dblOffsetY = RoundOff((dblOf1 - dblOf2) / dblNumerator)
            End If

            '�I�t�Z�b�g�l�̕␳
            '   �I�t�Z�b�g�͊1�̊O���J�����̃Y���ʂ�BP�I�t�Z�b�g�A�L�����u���[�V�����I�t�Z�b�g�֔��f 
            dblOffsetX = dblCurCalibOff(0) + (dblGap1X * -1)   'V1.20.0.0�F 'V4.7.0.0�O�R�����g����
            'dblOffsetX = 0.0                                    'V1.20.0.0�F
            dblOffsetY = dblCurCalibOff(1) + (dblGap1Y * -1)   'V1.20.0.0�F 'V4.7.0.0�O�R�����g����
            'dblOffsetY = 0.0                                    'V1.20.0.0�F

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "FrmCalibration.CalcGainOffset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
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
        'Call SubBtnSTART_Click()                                       'V1.20.0.1�B
    End Sub
    Private Sub BtnSTART_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnSTART.MouseUp
        Call SubBtnSTART_Click()                                        'V1.20.0.1�B
    End Sub
#End Region

#Region "RESET�{�^������������"
    '''=========================================================================
    '''<summary>RESET�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        'Call SubBtnRESET_Click()                                       'V1.20.0.1�B
    End Sub
    Private Sub BtnRESET_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnRESET.MouseUp
        Call SubBtnRESET_Click()                                        'V1.20.0.1�B
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
    Private Sub FrmCalibration_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J 
        Console.WriteLine("FrmCalibration.FrmCalibration_KeyDown()")

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V6.0.0.0�K       'Call Sub_10KeyDown(KeyCode)
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
    Private Sub FrmCalibration_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J
        Console.WriteLine("FrmCalibration.FrmCalibration_KeyUp()")
        ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
        'V6.0.0.0�K        Call Sub_10KeyUp(KeyCode)
        Sub_10KeyUp(KeyCode, stJOG)               'V6.0.0.0�K
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

End Class