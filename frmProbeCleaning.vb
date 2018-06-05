'===============================================================================
'   Description  : �v���[�u�N���[�j���O��ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2015
'
'===============================================================================
Option Strict Off
Option Explicit On
'
Imports LaserFront.Trimmer.DefWin32Fnc                                  'V6.0.0.1�A
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager                         'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources                                    'V4.4.0.0-0

Public Class frmProbeCleaning
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods                                           'V6.0.0.0�J

    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer) As Integer 'V4.0.0.0-53

#Region "�y�萔�E�ϐ���`�z"
    '===========================================================================
    '   �萔�E�ϐ���`
    '===========================================================================
    Private stJOG As JOG_PARAM                                          ' �����(JOG����)�p�p�����[�^
    Private dblTchMoval(3) As Double                                    ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))
    'V6.0.0.1�A                          ��                               Public -> Private, Const -> ReadOnly
    Private gdblCleaningPosX As Double
    Private gdblCleaningPosY As Double
    'V6.0.0.0�J    Public gdblCleaningPosZ As Double
    Private Shared gdblCleaningPosZ As Double       'V6.0.0.0�J
    'V6.0.0.0�J    Public ProbeCleaningCnt As Integer
    Public Shared ProbeCleaningCnt As Integer       'V6.0.0.0�J
    Private mExit_flg As Integer

    Private Const CLEANING_OFFSET As Integer = 1
    '----- V4.0.0.0-30�� -----
    ' �v���[�u�ڐG��
    Private ReadOnly CONTACT_MIN As Integer = 0
    Private ReadOnly CONTACT_MAX As Integer = 99
    ' �N���[�j���O�������s�Ԋu
    Private ReadOnly PROBING_MIN As Integer = 0
    Private ReadOnly PROBING_MAX As Integer = 32767
    '----- V4.0.0.0-30�� -----
    'V4.10.0.0�H��
    Private ReadOnly DISTANCE_MIN As Double = 0.0
    Private ReadOnly DISTANCE_MAX As Double = 10.0
    Private ReadOnly CLEANINGOFFSET_MIN As Double = -10.0
    Private ReadOnly CLEANINGOFFSET_MAX As Double = 10.0
    Private ReadOnly PITCH_MIN As Double = 0.0
    Private ReadOnly PITCH_MAX As Double = 1.0
    Private ReadOnly MOVECNT_MIN As Integer = 0
    Private ReadOnly MOVECNT_MAX As Integer = 99
    Dim savedblPrbDistance As Double                                    ' �v���[�u�ԋ����imm�j
    Dim savedblPrbCleaningOffset As Double                              ' �N���[�j���O�I�t�Z�b�g(mm)
    Dim savedblPrbCleanStagePitchX As Double                            ' �X�e�[�W����s�b�`
    Dim savedblPrbCleanStagePitchY As Double                            ' �X�e�[�W����s�b�`
    Dim saveintPrbCleanStageCountX As Integer                           ' �X�e�[�W�����
    Dim saveintPrbCleanStageCountY As Integer                           ' �X�e�[�W�����
    'V4.10.0.0�H��

    Private _tenKeyON As Boolean                                        'V6.0.0.0�O  'V6.0.0.1�A _tenKeyFlg -> _tenKeyON
    'V6.0.0.1�A                          ��
#End Region

#Region "�y���\�b�h��`�z"

#Region "�R���X�g���N�^"
    ''' <summary>�R���X�g���N�^</summary>
    ''' <param name="dblPrbCleanPosX">typPlateInfo.dblPrbCleanPosX</param>
    ''' <param name="dblPrbCleanPosY">typPlateInfo.dblPrbCleanPosY</param>
    ''' <remarks>'V6.0.0.0�J</remarks>
    Public Sub New(ByVal dblPrbCleanPosX As Double, ByVal dblPrbCleanPosY As Double)

        ' ���̌Ăяo���̓f�U�C�i�[�ŕK�v�ł��B
        InitializeComponent()

        ' InitializeComponent() �Ăяo���̌�ŏ�������ǉ����܂��B

        gdblCleaningPosX = dblPrbCleanPosX
        gdblCleaningPosY = dblPrbCleanPosY

        'V6.0.0.1�A                      ��
        CONTACT_MIN = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_CONTACT_MIN", TKY_INI, "0"))
        CONTACT_MAX = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_CONTACT_MAX", TKY_INI, "99"))
        PROBING_MIN = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PROBING_MIN", TKY_INI, "0"))
        PROBING_MAX = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PROBING_MAX", TKY_INI, "32767"))
        DISTANCE_MIN = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_DISTANCE_MIN", TKY_INI, "0.0"))
        DISTANCE_MAX = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_DISTANCE_MAX", TKY_INI, "10.0"))
        CLEANINGOFFSET_MIN = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_OFFSET_MIN", TKY_INI, "-10.0"))
        CLEANINGOFFSET_MAX = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_OFFSET_MAX", TKY_INI, "10.0"))
        PITCH_MIN = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PITCH_MIN", TKY_INI, "0.0"))
        PITCH_MAX = Double.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_PITCH_MAX", TKY_INI, "1.0"))
        MOVECNT_MIN = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_MOVECNT_MIN", TKY_INI, "0"))
        MOVECNT_MAX = Integer.Parse(GetPrivateProfileString_S("MIN_MAX", "PBCLN_MOVECNT_MAX", TKY_INI, "99"))
        'V6.0.0.1�A                      ��
    End Sub
#End Region
    '========================================================================================
    '   ��ʏ���
    '========================================================================================
#Region "�t�H�[�����\�����ꂽ�Ƃ��̃��C������"
#If False Then                          'V6.0.0.0�L Execute()�ł����Ȃ�
    '''=========================================================================
    ''' <summary>
    ''' �t�H�[�����\�����ꂽ�Ƃ��̃��C������ 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub frmProbeCleaning_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim strMSG As String

        Try
            ' �e�B�[�`���O�J�n
            If (mExit_flg <> -1) Then
                Me.GrpMain.Enabled = True 'V4.0.0.0-80
                Exit Sub
            End If

            mExit_flg = 0                                           ' �I���t���O = 0
            mExit_flg = ProbCleanMainProc()                              ' �s�w�܂��͂s�x�e�B�[�`���O�J�n

            ' �␳�N���X���C�����\���Ƃ���
            'V6.0.0.0�C            Form1.CrosLineX.Visible = False
            'V6.0.0.0�C            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTeach.frmTxTeach_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                   ' Return�l = ��O�G���[
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
            ' �e�B�[�`���O�J�n
            If (mExit_flg <> -1) Then
                'V6.0.0.0�K                Me.GrpMain.Enabled = True 'V4.0.0.0-80
                Exit Function
            End If

            _tenKeyON = False           'V6.0.0.1�A
            mExit_flg = 0                                           ' �I���t���O = 0
            mExit_flg = ProbCleanMainProc()                              ' �s�w�܂��͂s�x�e�B�[�`���O�J�n

            ' �␳�N���X���C�����\���Ƃ���
            'V6.0.0.0�C            Form1.CrosLineX.Visible = False
            'V6.0.0.0�C            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "frmTxTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                   ' Return�l = ��O�G���[
        End Try

        Me.Close()
        Return cFRS_NORMAL              ' sGetReturn ��荞��   'V6.0.0.0�L

    End Function
#End Region

#Region "�v���[�u�N���[�j���O�@�\��ʕ\��"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�@�\��ʕ\�� 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub frmProbeCleaning_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim stPos As System.Drawing.Point
        Dim stGetPos As System.Drawing.Point

        '�X�^�[�g�X�C�b�`���b�`�N���A   
        Call ZCONRST()

        giJogButtonEnable = 0 'V4.0.0.0-78

        mExit_flg = -1
        ' �\���ʒu�̒���
        stPos = Form1.Text4.PointToScreen(stGetPos)
        stPos.X = stPos.X - 2
        stPos.Y = stPos.Y - 2
        Me.Location = stPos
        Me.KeyPreview = True 'V4.0.0.0-53

    End Sub

    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0�K</remarks>
    Private Sub frmProbeCleaning_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown

        ' Form.Load() �� Show()����ShowDialog()���Ƃň�x�E�s�x�Ɣ����󋵂��قȂ邽�߁A�Ă΂���ɉe�����󂯂Ȃ�Shown()�ł����Ȃ�
        stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                     BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
    End Sub
#End Region

#Region "�X�e�[�W�o�^�ʒu�ւ̈ړ��{�^��"
    '''=========================================================================
    ''' <summary>
    ''' �o�^�ʒu�ւ̈ړ��{�^�� 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnStageRegPosMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim PosX As Double
        Dim PosY As Double
        Dim r As Integer
        Dim rtnCode As Integer
        Dim strMSG As String

        Try

            PosX = CDbl(txtRegPosX.Text)
            PosY = CDbl(txtRegPosY.Text)

            r = SMOVE2(PosX, PosY)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' ���b�Z�[�W�\��
            End If
        Catch ex As Exception
            strMSG = "frmProbeCleaning.btnStageRegPosMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return
        End Try
    End Sub
#End Region

#Region "�o�^���Ă���Z�����W�ւ̈ړ��{�^��"
    '''=========================================================================
    ''' <summary>
    ''' �o�^���Ă���Z�����W�ւ̈ړ� 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnZRegMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim PosZ As Double
        Dim ret As Integer
        Dim strMSG As String

        Try

            PosZ = CDbl(txtRegPosZ.Text)

            ' Z��ҋ@�ʒu�ֈړ�����
            ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' �G���[��EX_MOVE���ŕ\���ς�
            If (ret <> cFRS_NORMAL) Then                                  ' �G���[ ?

            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.btnZRegMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return
        End Try

    End Sub
#End Region

#Region "�v���[�u�N���[�j���O�@�\���C�����[�v����"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�@�\���C�����[�v���� 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function ProbCleanMainProc() As Integer

        Dim GrpNum As Short                                 ' ��ٰ�ߐ�
        Dim RnBn As Short                                   ' �`�b�v��(��R��)(TX��), �u���b�N��(TY��)
        Dim GrpCnt As Short                                 ' ��ٰ�ߐ�����
        Dim ChipSize As Double                              ' �␳������߻���
        Dim tmpChipSize As Double                           ' ���߻��ޑޔ���
        Dim CSPoint(1) As Double                            ' ���߻��ގZ�o�p(0:��1��_, 1:��2��_)
        Dim mdBpOffx As Double                              ' BP�ʒu�̾��X
        Dim mdBpOffy As Double                              ' BP�ʒu�̾��Y
        Dim mdAdjx As Double                                ' ��ެ�ĈʒuX(���g�p)
        Dim mdAdjy As Double                                ' ��ެ�ĈʒuY(���g�p)
        Dim dStepOffx As Double                             ' �ï�ߵ̾�ė�X
        Dim dStepOffy As Double                             ' �ï�ߵ̾�ė�Y
        Dim mdBSx As Double                                 ' ��ۯ�����X
        Dim mdBSy As Double                                 ' ��ۯ�����Y
        Dim KJPosX As Double                                ' �擪�u���b�N�̐擪��ʒu(BP�ʒuX/ð��وړ����WX)
        Dim KJPosY As Double                                ' �擪�u���b�N�̐擪��ʒu(BP�ʒuY/ð��وړ����WY)
        Dim r As Short
        Dim StepNum As Integer = 0
        Dim strMSG As String
        Dim wkContactCount As Integer = 0                               ' V4.0.0.0-30
        Dim wkProbingCount As Integer = 0                               ' V4.0.0.0-30
        Dim br As Boolean = True                                        ' V4.0.0.0-30

        Try

            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' JOG�p�����[�^�ݒ� 
            stJOG.Md = MODE_STG                                     ' ���[�h(STAGE�ړ�)
            stJOG.Md2 = MD2_BUTN                                    ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            '                                                       ' �L�[�̗L��(1)/����(0)�w��
            'stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_ZSW
            stJOG.PosX = 0.0                                        ' BP X�ʒu(BP�̾��X)
            stJOG.PosY = 0.0                                        ' BP Y�ʒu(BP�̾��Y)
            stJOG.BpOffX = 0                                        ' BP�̾��X 
            stJOG.BpOffY = 0                                        ' BP�̾��Y 
            stJOG.BszX = 0                                          ' ��ۯ�����X 
            stJOG.BszY = 0                                          ' ��ۯ�����Y
            stJOG.TextX = txtStagePosX                              ' BP X�ʒu�\���p÷���ޯ��
            stJOG.TextY = txtStagePosY                              ' BP Y�ʒu�\���p÷���ޯ��
            stJOG.cgX = 0                                           ' �ړ���X (BP�̾��X)
            stJOG.cgY = 0                                           ' �ړ���Y (BP�̾��Y)
            stJOG.BtnHI = BtnHI                                     ' HI�{�^��
            stJOG.BtnZ = BtnZ                                       ' Z�{�^��
            stJOG.BtnSTART = BtnSTART                               ' START�{�^��
            stJOG.BtnRESET = BtnRESET                               ' RESET�{�^��
            stJOG.BtnHALT = BtnHALT                                 ' HALT�{�^��
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            stJOG.Flg = -1                                          ' �e��ʂ�OK/Cancel���݉����׸�
            stJOG.bZ = False                                        ' Jog��Z�L�[��� = Z Off
            Call LAMP_CTRL(LAMP_Z, False)

            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �O���[�v��, �u���b�N��(TY��)�܂��̓`�b�v��(��R��)(TX��), �`�b�v�T�C�Y���擾����
            r = GetChipNumAndSize(giAppMode, GrpNum, RnBn, ChipSize)
            tmpChipSize = ChipSize                                      ' �`�b�v�T�C�Y�ޔ�
            GrpCnt = 0                                                  ' ��ٰ�ߐ�����
            mdAdjx = 0.0 : mdAdjy = 0.0                                 ' ��ެ�ĈʒuX(���g�p)
            mdBpOffx = typPlateInfo.dblBpOffSetXDir                     ' BP�ʒu�̾��X,Y�ݒ�
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            dStepOffx = typPlateInfo.dblStepOffsetXDir                  ' �ï�ߵ̾�ė�X,Y(TYè���p)
            dStepOffy = typPlateInfo.dblStepOffsetYDir
            Call CalcBlockSize(mdBSx, mdBSy)                            ' �u���b�N�T�C�YXY�ݒ�
            '----- V2.0.0.0_30�� -----
            'V4.10.0.0�H            txtRegPosX.Text = CStr(typPlateInfo.dblPrbCleanPosX)
            'V4.10.0.0�H            txtRegPosY.Text = CStr(typPlateInfo.dblPrbCleanPosY)
            'V4.10.0.0�H            txtRegPosZ.Text = CStr(typPlateInfo.dblPrbCleanPosZ)
            txtRegPosX.Text = typPlateInfo.dblPrbCleanPosX.ToString("0.0000")
            txtRegPosY.Text = typPlateInfo.dblPrbCleanPosY.ToString("0.0000")
            txtRegPosZ.Text = typPlateInfo.dblPrbCleanPosZ.ToString("0.000")
            txtProbingCnt.Text = CStr(typPlateInfo.intPrbCleanAutoSubCount) ' �v���[�r���O�Ԋu
            txtContactCount.Text = CStr(typPlateInfo.intPrbCleanUpDwCount)  ' �v���[�r���O�� 
            '----- V2.0.0.0_30�� ----
            'V4.10.0.0�H��
            savedblPrbDistance = typPlateInfo.dblPrbDistance                ' �v���[�u�ԋ����imm�j
            savedblPrbCleaningOffset = typPlateInfo.dblPrbCleaningOffset    ' �N���[�j���O�I�t�Z�b�g(mm)
            savedblPrbCleanStagePitchX = typPlateInfo.dblPrbCleanStagePitchX    '�X�e�[�W����s�b�`
            savedblPrbCleanStagePitchY = typPlateInfo.dblPrbCleanStagePitchY    '�X�e�[�W����s�b�`
            saveintPrbCleanStageCountX = typPlateInfo.intPrbCleanStageCountX    '�X�e�[�W�����
            saveintPrbCleanStageCountY = typPlateInfo.intPrbCleanStageCountY    '�X�e�[�W�����
            If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then               ' = 1 �v���[�u�A�b�v�_�E��
                txtOption1_Y.Enabled = False
                txtOption1_Y.Visible = False
                txtOption2_Y.Enabled = False
                txtOption2_Y.Visible = False
                LblOption2_1.Visible = False
                LblOption2_2.Visible = False
                LblOption1_1.Visible = True
                LblOption1_2.Visible = True
                'V6.0.0.1�A                txtOption2_X.Text = CStr(typPlateInfo.dblPrbCleaningOffset)     ' �N���[�j���O�I�t�Z�b�g(mm)
                'V6.0.0.1�A                txtOption1_X.Text = CStr(typPlateInfo.dblPrbDistance)           ' �v���[�u�ԋ����imm�j
                txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.000")       ' �v���[�u�ԋ����imm�j
                txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.000") ' �N���[�j���O�I�t�Z�b�g(mm)
            Else                                ' = 2 �X�e�[�W�ړ�
                txtOption1_Y.Enabled = True
                txtOption1_Y.Visible = True
                txtOption2_Y.Enabled = True
                txtOption2_Y.Visible = True
                LblOption2_1.Visible = True
                LblOption2_2.Visible = True
                LblOption1_1.Visible = False
                LblOption1_2.Visible = False
                'V6.0.0.1�A                txtOption1_X.Text = CStr(typPlateInfo.dblPrbCleanStagePitchX)       '�X�e�[�W����s�b�`�w
                'V6.0.0.1�A                txtOption1_Y.Text = CStr(typPlateInfo.dblPrbCleanStagePitchY)       '�X�e�[�W����s�b�`�x
                'V6.0.0.1�A                txtOption2_X.Text = CStr(typPlateInfo.intPrbCleanStageCountX)       '�X�e�[�W����s�b�`�w
                'V6.0.0.1�A                txtOption2_Y.Text = CStr(typPlateInfo.intPrbCleanStageCountY)       '�X�e�[�W����s�b�`�x
                txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.000")       '�X�e�[�W����s�b�`�w
                txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.000")       '�X�e�[�W����s�b�`�x
                txtOption2_X.Text = typPlateInfo.intPrbCleanStageCountX.ToString("0.000")       '�X�e�[�W����s�b�`�w
                txtOption2_Y.Text = typPlateInfo.intPrbCleanStageCountY.ToString("0.000")       '�X�e�[�W����s�b�`�x
            End If
            'V4.10.0.0�H��
            '----- V4.0.0.0-53�� -----
            Me.txtStagePosX.Text = txtRegPosX.Text
            Me.txtStagePosY.Text = txtRegPosY.Text
            Me.txtZPos.Text = txtRegPosZ.Text
            CleaningPosZ = typPlateInfo.dblPrbCleanPosZ
            '----- V4.0.0.0-53�� -----
STP_RETRY:
            'Call InitDisp()                                             ' ���W�\���N���A
            ChipSize = tmpChipSize                                      ' �`�b�v�T�C�Y�ݒ�

            r = Form1.System1.BpCenter(gSysPrm)                         ' BP�Z���^�[�ֈړ�
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return�l = �G���[
            End If

            ' �����(BP��JOG����)�p�p�����[�^������������
            stJOG.Md = MODE_STG                                         ' ���[�h(0:XY�e�[�u���ړ�)

            stJOG.Md2 = MD2_BUTN                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            '                                                           ' �L�[�̗L��(1)/����(0)�w��
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT
            stJOG.PosX = KJPosX                                         ' BP X�ʒu/XYð��� X�ʒu
            stJOG.PosY = KJPosY                                         ' BP Y�ʒu/XYð��� Y�ʒu
            stJOG.BpOffX = mdAdjx + mdBpOffx                            ' BP�̾��X 
            stJOG.BpOffY = mdAdjy + mdBpOffy                            ' BP�̾��Y 
            stJOG.BszX = mdBSx                                          ' ��ۯ�����X 
            stJOG.BszY = mdBSy                                          ' ��ۯ�����Y
            stJOG.TextX = txtStagePosX                                  ' BP X�ʒu/XYð��� X�ʒu�\���p÷���ޯ��
            stJOG.TextY = txtStagePosY                                  ' BP Y�ʒu/XYð��� Y�ʒu�\���p÷���ޯ��
            stJOG.cgX = 0.0                                             ' �ړ���X 
            stJOG.cgY = 0.0                                             ' �ړ���Y 
            stJOG.BtnHI = BtnHI                                         ' HI�{�^��
            stJOG.BtnZ = BtnZ                                           ' Z�{�^��
            stJOG.BtnSTART = BtnSTART                                   ' START�{�^��
            stJOG.BtnHALT = BtnHALT                                     ' HALT�{�^��
            stJOG.BtnRESET = BtnRESET                                   ' RESET�{�^��
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸�
            Call Me.Focus()

STP_CHIPSIZE:
            '-------------------------------------------------------------------
            '   �`�b�v�T�C�Y�w(TX��)�܂��̓`�b�v�T�C�Y�x(TY��)�̃e�B�[�`���O����
            '   ��CSPoint(0:��1��_, 1:��2��_)��BP�܂��̓e�[�u���ʒu��ݒ肷��
            '-------------------------------------------------------------------
            ' �`�b�v�T�C�Y�̃e�B�[�`���O����
            r = Sub_Jog1()
            Timer1.Enabled = False                                      ' V4.0.0.0-53
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��
                Return (r)
            End If
            If (r <> cFRS_NORMAL) Then                                  ' �����p���ȊO(RESET SW(Cancel����)����/OK�{�^������)�Ȃ�g���~���O�f�[�^�X�V��
                GoTo STP_END
            End If

STP_END:
            '-------------------------------------------------------------------
            '   �g���~���O�f�[�^�X�V
            '-------------------------------------------------------------------
            If (r <> cFRS_ERR_RST) Then                                 ' Cancel(RESET SW)�����ȊO ?
                '----- V4.0.0.0-30�� -----
                ' ���̓f�[�^�`�F�b�N(�v���[�u�ڐG��)
                br = DataCheck_Int(txtContactCount.Text, LblContactCount.Text, CONTACT_MIN, CONTACT_MAX, wkContactCount)
                If (br = False) Then
                    txtContactCount.Text = CStr(typPlateInfo.intPrbCleanUpDwCount)
                    txtContactCount.Focus()
                    GoTo STP_CHIPSIZE
                End If
                ' ���̓f�[�^�`�F�b�N(�N���[�j���O�������s�Ԋu)
                br = DataCheck_Int(txtProbingCnt.Text, LblProbingCnt.Text, PROBING_MIN, PROBING_MAX, wkProbingCount)
                If (br = False) Then
                    txtProbingCnt.Text = CStr(typPlateInfo.intPrbCleanAutoSubCount)
                    txtProbingCnt.Focus()
                    GoTo STP_CHIPSIZE
                End If
                '----- V4.0.0.0-30�� -----
                'V4.10.0.0�H��
                If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then               ' = 1 �v���[�u�A�b�v�_�E��
                    ' �v���[�u�ԋ����imm�j
                    br = DataCheck_Double(txtOption1_X.Text, LblOption1_1.Text, DISTANCE_MIN, DISTANCE_MAX, typPlateInfo.dblPrbDistance)
                    txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.0")
                    If (br = False) Then
                        txtOption1_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    ' �N���[�j���O�I�t�Z�b�g(mm)
                    br = DataCheck_Double(txtOption2_X.Text, LblOption1_2.Text, CLEANINGOFFSET_MIN, CLEANINGOFFSET_MAX, typPlateInfo.dblPrbCleaningOffset)
                    txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.0")
                    If (br = False) Then
                        txtOption2_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                Else                                ' = 2 �X�e�[�W�ړ�
                    '�X�e�[�W����s�b�`
                    br = DataCheck_Double(txtOption1_X.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchX)
                    txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.0")
                    If (br = False) Then
                        txtOption1_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    br = DataCheck_Double(txtOption1_Y.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchY)
                    txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.0")
                    If (br = False) Then
                        txtOption1_Y.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    '�X�e�[�W�����
                    br = DataCheck_Int(txtOption2_X.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountX)
                    txtOption2_X.Text = typPlateInfo.intPrbCleanStageCountX.ToString("0")
                    If (br = False) Then
                        txtOption2_X.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                    br = DataCheck_Int(txtOption2_Y.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountY)
                    txtOption2_Y.Text = typPlateInfo.intPrbCleanStageCountY.ToString("0")
                    If (br = False) Then
                        txtOption2_Y.Focus()
                        GoTo STP_CHIPSIZE
                    End If
                End If
                'V4.10.0.0�H��
                '----- V2.0.0.0_30�� -----
                ' �g���~���O�f�[�^�X�V
                typPlateInfo.dblPrbCleanPosX = gdblCleaningPosX
                typPlateInfo.dblPrbCleanPosY = gdblCleaningPosY
                typPlateInfo.dblPrbCleanPosZ = gdblCleaningPosZ
                typPlateInfo.intPrbCleanUpDwCount = wkContactCount       ' �v���[�r���O��
                typPlateInfo.intPrbCleanAutoSubCount = wkProbingCount    ' �v���[�r���O�Ԋu
                'typPlateInfo.intPrbCleanUpDwCount = CLng(txtContactCount.Text)       ' �v���[�r���O��
                'typPlateInfo.intPrbCleanAutoSubCount = CInt(txtProbingCnt.Text)      ' �v���[�r���O�Ԋu
                '----- V2.0.0.0_30�� -----
                gCmpTrimDataFlg = 1                                     ' �f�[�^�X�V�t���O = 1(�X�V����)
            End If
            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.TxTyMainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "�v���[�u�e�B�[�`���OJog���쏈��"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�e�B�[�`���OJog���쏈��
    ''' </summary>
    ''' <returns>cFRS_ERR_START = OK(START��)����
    '''          cFRS_ERR_RST   = Cancel(RESET��)����
    '''          cFRS_NORMAL    = ����(�O���[�v�ԃC���^�[�o��������)
    '''          -1�ȉ�         = �G���[</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function Sub_Jog1() As Short

        Dim r As Short
        Dim rtn As Short                                                ' �ߒl 
        Dim strMSG As String
        Dim DirX As Integer = 1, DirY As Integer = 1
        'V6.0.0.0�J        Dim stJogTextZ As Object

        Try
            ' ��������
            'Me.LblHead01.Text = LBL_PROBECLEANING_TEACH                 ' "�v���[�u�N���[�j���O�ʒu�e�B�[�`���O" V2.0.0.0�P
            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸�
            Timer1.Enabled = True                                       ' V4.0.0.0-53

            'V4.3.0.0�B
            globaldblCleaningPosX = typPlateInfo.dblPrbCleanPosX
            globaldblCleaningPosY = typPlateInfo.dblPrbCleanPosY
            'V4.3.0.0�B

            ' ���݂̓o�^�ʒu�փX�e�[�W�̈ړ����s���B 
            r = MoveCleaningPosXY(0.0, 0.0)                             'V4.10.0.0�H  OffsetX,Y�ǉ�
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)                                              ' �G���[���^�[��
            End If
            cmbMoveMode.SelectedIndex = 0                               ' �ړ����[�h = �X�e�[�W V4.0.0.0-53

STP_RETRY:
            Call Me.Focus()
            'cmbMoveMode.SelectedIndex = 0                               ' �ړ����[�h = �X�e�[�W V4.0.0.0-53

            ' �e�B�[�`���O"����
            stJOG.PosX = gdblCleaningPosX                               ' BP X�܂���XYð��� X��Έʒu
            stJOG.PosY = gdblCleaningPosY                               ' BP Y�܂���XYð��� Y��Έʒu

            stJOG.TextX = txtStagePosX                                  ' ���WX�ʒu�\���p÷���ޯ��
            stJOG.TextY = txtStagePosY                                  ' ���WY�ʒu�\���p÷���ޯ��
            Me.txtZPos.Text = CleaningPosZ.ToString("0.000")            ' V4.0.0.0-53
            'V6.0.0.0�J            stJogTextZ = txtZPos                                        ' Z���W�\���p V4.0.0.0-53

            '' �\�����b�Z�[�W����ݒ肷�� 'V4.0.0.0-30
            ''"�v���[�u�N���[�j���O�̃X�e�[�W�ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
            'LblMsg.Text = MSG_160 + vbCrLf + MSG_161 'V4.0.0.0-30

            'V6.0.0.0�O            If (gbTenKeyFlg) Then
            If (_tenKeyON) Then         'V6.0.0.0�O
                Form1.Instance.SetActiveJogMethod(AddressOf Me.JogKeyDown,
                                                  AddressOf Me.JogKeyUp,
                                                  AddressOf Me.MoveToCenter)    'V6.0.0.0�J
            Else
                Form1.Instance.SetActiveJogMethod(Nothing,
                                                  Nothing,
                                                  AddressOf Me.MoveToCenter)    'V6.0.0.0�J
            End If

            Do
                'V6.0.0.0�J                r = JogEzMoveWithZ(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, stJogTextZ)
                r = JogEzMoveWithZ(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause,
                                   LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval,
                                   AddressOf DispProbePos)              'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�I��
                    Return (r)
                End If

                ' �R���\�[���L�[�`�F�b�N
                If (r = cFRS_ERR_START) Then                            ' START SW���� 
                    'V4.0.0.0-80
                    'gdblCleaningPosX = gdblCleaningPosX + stJOG.cgX
                    'gdblCleaningPosY = gdblCleaningPosY + stJOG.cgY
                    'gdblCleaningPosZ = CleaningPosZ
                    'V4.0.0.0-80
                    Exit Do

                    ' HALT SW�������͂P�O�̃e�B�[�`���O�֖߂�
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW���� ?

                    '  RESET SW�������͏I���m�F���b�Z�[�W�\����
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW���� ?
                    'V4.10.0.0�H��
                    typPlateInfo.dblPrbDistance = savedblPrbDistance                ' �v���[�u�ԋ����imm�j
                    typPlateInfo.dblPrbCleaningOffset = savedblPrbCleaningOffset   ' �N���[�j���O�I�t�Z�b�g(mm)
                    typPlateInfo.dblPrbCleanStagePitchX = savedblPrbCleanStagePitchX '�X�e�[�W����s�b�`
                    typPlateInfo.dblPrbCleanStagePitchY = savedblPrbCleanStagePitchY '�X�e�[�W����s�b�`
                    typPlateInfo.intPrbCleanStageCountX = saveintPrbCleanStageCountX '�X�e�[�W�����
                    typPlateInfo.intPrbCleanStageCountY = saveintPrbCleanStageCountY '�X�e�[�W�����
                    'V4.10.0.0�H��
                    Exit Do

                    ' Z�����~�b�g�G���[���Ɏ���On������Ԃ̂܂܃{�^���̕\����[Z Off]�ƂȂ�
                    ' �{�^����Enabled��False�ƂȂ��Ă��܂����߃t���O�̐ݒ��ǉ�
                ElseIf (r = cFRS_ERR_Z) Then        'V6.0.0.0�R
                    stJOG.bZ = (Not stJOG.bZ)
                    'V6.0.0.1�A          ��
                    Double.TryParse(txtStagePosX.Text, gdblCleaningPosX)
                    Double.TryParse(txtStagePosY.Text, gdblCleaningPosY)
                    stJOG.PosX = gdblCleaningPosX                       ' XYð��� X��Έʒu
                    stJOG.PosY = gdblCleaningPosY                       ' XYð��� Y��Έʒu
                    If (MODE_STG = iGlobalJogMode) Then
                        stJOG.Opt = stJOG.Opt And (Not CONSOLE_SW_ZSW)
                    End If
                    'V6.0.0.1�A          ��
                End If

                Call ZCONRST()                                          ' �ݿ�ٷ�ׯ����� 
            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK/Cancel���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W�\�� 
            '-------------------------------------------------------------------
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, Me.LblHead01.Text)
            'V4.3.0.0�B
            '----- V4.0.0.0-53�� -----
            'If (iGlobalJogMode = MODE_STG) Then
            'V6.0.0.1�A            gdblCleaningPosX = gdblCleaningPosX + stJOG.cgX
            'V6.0.0.1�A            gdblCleaningPosY = gdblCleaningPosY + stJOG.cgY
            Double.TryParse(txtStagePosX.Text, gdblCleaningPosX)        'V6.0.0.1�A
            Double.TryParse(txtStagePosY.Text, gdblCleaningPosY)        'V6.0.0.1�A
            'Else
            gdblCleaningPosZ = CleaningPosZ
            'End If
            '----- V4.0.0.0-53�� -----
            'V4.3.0.0�B
            ' Cancel(RESET��)�������͏������p������
            If (rtn = cFRS_ERR_RST) Then
                'V4.3.0.0�B
                '----- V4.0.0.0-53�� -----
                'If (iGlobalJogMode = MODE_STG) Then
                '    gdblCleaningPosX = gdblCleaningPosX + stJOG.cgX
                '    gdblCleaningPosY = gdblCleaningPosY + stJOG.cgY
                'Else
                '    gdblCleaningPosZ = CleaningPosZ
                'End If
                '----- V4.0.0.0-53�� -----
                'V4.3.0.0�B
                stJOG.bZ = False                                        'V6.0.0.1�A
                stJOG.Opt = stJOG.Opt And (Not CONSOLE_SW_ZSW)          'V6.0.0.1�A
                cmbMoveMode.SelectedIndex = 0                           'V4.10.0.0�H
                GoTo STP_RETRY                                          ' �����p����
            End If

STP_END:
            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Sub_Jog1() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[

        Finally                         'V6.0.0.0�J
            Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)
        End Try
    End Function
#End Region

#Region "�v���[�u�N���[�j���O�ʒu�o�^��ʂł̓��샂�[�h�ύX"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�ʒu�o�^��ʂł̓��샂�[�h�ύX
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmbMoveMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMoveMode.SelectedIndexChanged
        Dim r As Integer

        If cmbMoveMode.SelectedIndex = 0 Then
            iGlobalJogMode = MODE_STG
            txtZPos.BackColor = System.Drawing.SystemColors.Control
            txtStagePosX.BackColor = System.Drawing.Color.Yellow 'V4.0.0.0-53
            txtStagePosY.BackColor = System.Drawing.Color.Yellow 'V4.0.0.0-53
            '----- V4.0.0.0-30�� -----
            '"�v���[�u�N���[�j���O�̃X�e�[�W�ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
            LblMsg.Text = MSG_160 + vbCrLf + MSG_161
            '----- V4.0.0.0-30�� -----
            'V4.0.0.0-78
            r = MoveOrgPosZ()
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' ���b�Z�[�W�\��
            End If
            'V6.0.0.1�A            BtnZ.BackColor = Color.LightGray
            BtnZ.BackColor = SystemColors.Control                       'V6.0.0.1�A
            giJogButtonEnable = 0
            BtnZ.Enabled = False
            'V4.0.0.0-78

            BtnZ.Text = "Z Off"                                         'V6.0.0.0�R
            'V6.0.0.1�A            stJOG.Opt = stJOG.Opt Or (Not CONSOLE_SW_ZSW)               'V6.0.0.0�R
            stJOG.bZ = False                                            'V6.0.0.1�A
        Else
            iGlobalJogMode = MODE_Z
            txtZPos.BackColor = System.Drawing.Color.Yellow 'V4.0.0.0-53
            txtStagePosX.BackColor = System.Drawing.SystemColors.Control
            txtStagePosY.BackColor = System.Drawing.SystemColors.Control
            '----- V4.0.0.0-30�� -----
            '"�v���[�u�N���[�j���O�̃v���[�u�ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
            LblMsg.Text = MSG_162 + vbCrLf + MSG_161
            '----- V4.0.0.0-30�� -----
            BtnZ.Enabled = True 'V4.0.0.0-78
            stJOG.Opt = (stJOG.Opt Or CONSOLE_SW_ZSW)                   'V6.0.0.0�R
        End If

    End Sub
#End Region

#Region "�v���[�u�N���[�j���O�̎��s�{�^��"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�̎��s�{�^��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnCleaningStart_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnCleaningStart.MouseUp

        Dim strMSG As String
        Dim wkContactCount As Integer = 0                               ' V4.0.0.0-30
        Dim wkProbingCount As Integer = 0                               ' V4.0.0.0-30
        Dim br As Boolean = True                                        ' V4.0.0.0-30
        Dim Ret As Integer

        Try
            'V6.0.0.1�A                  ��
            btnCleaningStart.Parent.Select()
            btnCleaningStart.Enabled = False
            cmdCancel.Enabled = False
            cmdOK.Enabled = False
            GrpArrow.Enabled = False
            BtnTenKey.Enabled = False
            Form1.Instance.VideoLibrary1.SetTrackBar(True, False)       'V6.0.1.0�F
            Application.DoEvents()
            'V6.0.0.1�A                  ��

            '----- V4.0.0.0-30�� -----
            ' ���̓f�[�^�`�F�b�N(�v���[�u�ڐG��)
            br = DataCheck_Int(txtContactCount.Text, LblContactCount.Text, CONTACT_MIN, CONTACT_MAX, wkContactCount)
            If (br = False) Then
                txtContactCount.Text = CStr(typPlateInfo.intPrbCleanUpDwCount)
                txtContactCount.Focus()
                Return
            End If
            ' ���̓f�[�^�`�F�b�N(�N���[�j���O�������s�Ԋu)
            br = DataCheck_Int(txtProbingCnt.Text, LblProbingCnt.Text, PROBING_MIN, PROBING_MAX, wkProbingCount)
            If (br = False) Then
                txtProbingCnt.Text = CStr(typPlateInfo.intPrbCleanAutoSubCount)
                txtProbingCnt.Focus()
                Return
            End If

            ProbeCleaningCnt = wkContactCount
            'ProbeCleaningCnt = CLng(txtContactCount.Text)
            '----- V4.0.0.0-30�� -----
            'V4.10.0.0�H��
            If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then               ' = 1 �v���[�u�A�b�v�_�E��
                ' �v���[�u�ԋ����imm�j
                br = DataCheck_Double(txtOption1_X.Text, LblOption1_1.Text, DISTANCE_MIN, DISTANCE_MAX, typPlateInfo.dblPrbDistance)
                'V5.0.0.8�D                txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.0")
                txtOption1_X.Text = typPlateInfo.dblPrbDistance.ToString("0.000")
                If (br = False) Then
                    txtOption1_X.Focus()
                    Return
                End If
                ' �N���[�j���O�I�t�Z�b�g(mm)
                br = DataCheck_Double(txtOption2_X.Text, LblOption1_2.Text, CLEANINGOFFSET_MIN, CLEANINGOFFSET_MAX, typPlateInfo.dblPrbCleaningOffset)
                'V5.0.0.8�D                txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.0")
                txtOption2_X.Text = typPlateInfo.dblPrbCleaningOffset.ToString("0.000")
                If (br = False) Then
                    txtOption2_X.Focus()
                    Return
                End If
            Else                                ' = 2 �X�e�[�W�ړ�
                '�X�e�[�W����s�b�`
                br = DataCheck_Double(txtOption1_X.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchX)
                'V5.0.0.8�D                txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.0")
                txtOption1_X.Text = typPlateInfo.dblPrbCleanStagePitchX.ToString("0.000")
                If (br = False) Then
                    txtOption1_X.Focus()
                    Return
                End If
                br = DataCheck_Double(txtOption1_Y.Text, LblOption2_1.Text, PITCH_MIN, PITCH_MAX, typPlateInfo.dblPrbCleanStagePitchY)
                'V5.0.0.8�D                txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.0")
                txtOption1_Y.Text = typPlateInfo.dblPrbCleanStagePitchY.ToString("0.000")
                If (br = False) Then
                    txtOption1_Y.Focus()
                    Return
                End If
                '�X�e�[�W�����
                br = DataCheck_Int(txtOption2_X.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountX)
                txtOption2_X.Text = typPlateInfo.intPrbCleanStageCountX.ToString("0")
                If (br = False) Then
                    txtOption2_X.Focus()
                    Return
                End If
                br = DataCheck_Int(txtOption2_Y.Text, LblOption2_2.Text, MOVECNT_MIN, MOVECNT_MAX, typPlateInfo.intPrbCleanStageCountY)
                txtOption2_Y.Text = typPlateInfo.intPrbCleanStageCountY.ToString("0")
                If (br = False) Then
                    txtOption2_Y.Focus()
                    Return
                End If
            End If
            'V4.10.0.0�H��
            'V4.3.0.0�B
            If (Double.TryParse(Me.txtStagePosX.Text, globaldblCleaningPosX) = False) Then
                strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() X-Pos ERROR = "
                MsgBox(strMSG)
                Return                  'V6.0.0.1�A
            End If
            If (Double.TryParse(Me.txtStagePosY.Text, globaldblCleaningPosY) = False) Then
                strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() Y-Pos ERROR = "
                MsgBox(strMSG)
                Return                  'V6.0.0.1�A
            End If
            If (Double.TryParse(Me.txtZPos.Text, globaldblCleaningPosZ) = False) Then
                strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() Y-Pos ERROR = "
                MsgBox(strMSG)
                Return                  'V6.0.0.1�A
            End If

            'V4.3.0.0�B

            Ret = ProbeCleaningStart()
            'V6.0.0.1�A                  ��
            ' �v���[�u�N���[�j���O�J�n�ʒu�ɃX�e�[�W��߂�
            Ret = MoveCleaningPosXY(0.0, 0.0)
            giJogButtonEnable = 0
            stJOG.bZ = False
            'V6.0.0.1�A                  ��
            Return

        Catch ex As Exception
            strMSG = "frmProbeCleaning.btnCleaningStart_MouseUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)

        Finally                         'V6.0.0.1�A
            Application.DoEvents()
            Form1.Instance.VideoLibrary1.SetTrackBar(True, True)        'V6.0.1.0�F
            BtnTenKey.Enabled = True
            GrpArrow.Enabled = True
            cmdOK.Enabled = True
            cmdCancel.Enabled = True
            btnCleaningStart.Enabled = True
        End Try

    End Sub
#End Region

#Region "�f�[�^�`�F�b�N(Integer�^)"
    '''=========================================================================
    ''' <summary>�f�[�^�`�F�b�N(Integer�^) V4.0.0.0-30</summary>
    ''' <param name="strVal">(INP)���̓f�[�^</param>
    ''' <param name="strLbl">(INP)���̓f�[�^�̃��x����</param>
    ''' <param name="MinVal">(INP)�ŏ��l</param>
    ''' <param name="MaxVal">(INP)�ő�l</param>
    ''' <param name="Val">   (OUT)�`�F�b�N��̃f�[�^��Ԃ�</param>
    ''' <returns>True=����, False=�G���[</returns>
    '''=========================================================================
    Public Function DataCheck_Int(ByRef strVal As String, ByRef strLbl As String, ByVal MinVal As Integer, ByVal MaxVal As Integer, ByRef Val As Integer) As Boolean

        Dim strMSG As String = ""
        Dim wkVal As Integer = 0

        Try
            ' ���̓f�[�^�`�F�b�N
            If (Integer.TryParse(strVal, wkVal) = False) OrElse _
                (MinVal > wkVal) OrElse (MaxVal < wkVal) Then
                GoTo STP_ERR1
            End If

            ' �`�F�b�N��̃f�[�^��Ԃ�
            Val = wkVal
            Return (True)

            ' �f�[�^�`�F�b�N�G���[
STP_ERR1:
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    strMSG = "���l��" & MinVal.ToString("0") & "�`" & MaxVal.ToString("0") & "�͈̔͂œ��͂��ĉ������B" + " (" + strLbl + ")"
            'Else
            '    strMSG = "Please specify a value within the limits of " & MinVal.ToString("0") & " and " & MaxVal.ToString("0") + " (" + strLbl + ")"
            'End If
            strMSG = String.Format(frmProbeCleaning_001, MinVal.ToString("0"), MaxVal.ToString("0"), strLbl)
            MsgBox(strMSG)
            Return (False)

        Catch ex As Exception
            strMSG = "frmProbeCleaning.DataCheck_Int() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (False)
        End Try
    End Function
#End Region

    'V4.10.0.0�H��
#Region "�f�[�^�`�F�b�N(Double�^)"
    '''=========================================================================
    ''' <summary>�f�[�^�`�F�b�N(Double�^) V4.0.0.0-30</summary>
    ''' <param name="strVal">(INP)���̓f�[�^</param>
    ''' <param name="strLbl">(INP)���̓f�[�^�̃��x����</param>
    ''' <param name="MinVal">(INP)�ŏ��l</param>
    ''' <param name="MaxVal">(INP)�ő�l</param>
    ''' <param name="Val">   (OUT)�`�F�b�N��̃f�[�^��Ԃ�</param>
    ''' <returns>True=����, False=�G���[</returns>
    '''=========================================================================
    Public Function DataCheck_Double(ByRef strVal As String, ByRef strLbl As String, ByVal MinVal As Double, ByVal MaxVal As Double, ByRef Val As Double) As Boolean

        Dim strMSG As String = ""
        Dim wkVal As Double = 0

        Try
            ' ���̓f�[�^�`�F�b�N
            If (Double.TryParse(strVal, wkVal) = False) OrElse _
                (MinVal > wkVal) OrElse (MaxVal < wkVal) Then
                GoTo STP_ERR1
            End If

            ' �`�F�b�N��̃f�[�^��Ԃ�
            Val = wkVal
            Return (True)

            ' �f�[�^�`�F�b�N�G���[
STP_ERR1:
            strMSG = String.Format(frmProbeCleaning_001, MinVal.ToString("0.0"), MaxVal.ToString("0.0"), strLbl)
            MsgBox(strMSG)
            Return (False)

        Catch ex As Exception
            strMSG = "frmProbeCleaning.DataCheck_Double() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (False)
        End Try
    End Function
#End Region

#Region "�v���[�u�N���[�j���O�@�\�̎��s"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�̉��~�㏸�̎��s 
    ''' </summary>
    ''' <param name="CountTimes">�_�E���A�b�v�̉�</param>
    ''' <param name="OrgX">�X�e�[�W�̌��݈ʒu�w</param>
    ''' <param name="OrgY">�X�e�[�W�̌��݈ʒu�x</param>
    ''' <param name="TimesX">�w�����ɃX�e�b�v�����</param>
    ''' <param name="TimesY">�x�����ɃX�e�b�v�����</param>
    ''' <param name="MoveX">�w�����ɃX�e�b�v���鋗��</param>
    ''' <param name="MoveY">�x�����ɃX�e�b�v���鋗��</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Shared Function ProbeDownAndUp(ByVal CountTimes As Integer, ByVal OrgX As Double, ByVal OrgY As Double, ByVal TimesX As Integer, ByVal TimesY As Integer, ByVal MoveX As Double, ByVal MoveY As Double) As Integer      'V6.0.0.0�J
        'V6.0.0.0�J    Public Function ProbeDownAndUp(ByVal CountTimes As Integer, ByVal OrgX As Double, ByVal OrgY As Double, ByVal TimesX As Integer, ByVal TimesY As Integer, ByVal MoveX As Double, ByVal MoveY As Double) As Integer

        Dim Ret As Integer
        Dim strMSG As String
        Dim i As Integer
        Dim CntX As Integer, CntY As Integer

        Try

            If TimesX > 0 Or TimesY > 0 Then
                Ret = PROBOFF_EX(globaldblCleaningPosZ)
                If Ret <> 0 Then
                    Return (Ret)
                End If
            End If
            ' �w��񐔃v���[�u�̏㉺���J��Ԃ� 
            For i = 1 To CountTimes
                ' �v���[�u�N���[�j���O�ʒu��Z���ʒu�ݒ� 
                Ret = MoveCleaningPosZ()                                    ' �v���[�u�N���[�j���O�ʒu��Z���ʒu�ݒ� 'V4.3.0.0�C
                If Ret <> 0 Then
                    Return (Ret)
                End If
                Application.DoEvents()                                          'V6.0.0.1�A
                Call System.Threading.Thread.Sleep(500)

                CntX = 0
                CntY = 0
                Do While (MoveX > 0.0 And CntX < TimesX) Or (MoveY > 0.0 And CntY < TimesY)
                    If CntX < TimesX Then                                   ' �w�����X�e�[�W����s�b�`�ֈړ�
                        Ret = MoveCleaningPosXY(MoveX, 0.0)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                        Application.DoEvents()                                  'V6.0.0.1�A
                        Call System.Threading.Thread.Sleep(300)
                    End If

                    If CntY < TimesY Then                                   ' �x�����X�e�[�W����s�b�`�ֈړ�
                        Ret = MoveCleaningPosXY(0.0, MoveY)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                        Application.DoEvents()                                  'V6.0.0.1�A
                        Call System.Threading.Thread.Sleep(300)
                    End If

                    Ret = MoveCleaningPosXY(OrgX, OrgY)                     ' ���̈ʒu�Ɉړ�
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                    Application.DoEvents()                                      'V6.0.0.1�A

                    CntX = CntX + 1
                    CntY = CntY + 1
                Loop

                Ret = MoveOrgWaitPosZ()                                     ' ���_�ʒu��Z���ʒu�ݒ� 
                If Ret <> 0 Then
                    Return (Ret)
                End If
                Application.DoEvents()                                          'V6.0.0.1�A
                Call System.Threading.Thread.Sleep(500)

            Next i

            If TimesX > 0 Or TimesY > 0 Then
                Ret = PROBOFF_EX(typPlateInfo.dblZWaitOffset)
                If Ret <> 0 Then
                    Return (Ret)
                End If
            End If
        Catch ex As Exception
            strMSG = "frmProbeCleaning.ProbeDownAndUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region
    'V4.10.0.0�H��

#Region "�v���[�u�N���[�j���O�@�\�̎��s"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�@�\�̎��s 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Shared Function ProbeCleaningStart() As Integer          'V6.0.0.0�J
        'V6.0.0.0�J    Public Function ProbeCleaningStart() As Integer

        Dim Ret As Integer
        Dim strMSG As String
        Dim PosX As Double, PosY As Double, MoveX As Double, MoveY As Double

        Try
            'V4.10.0.0�H �X�V��
            MoveX = 0.0
            MoveY = 0.0
            ' �v���[�u�N���[�j���O�ʒu�փX�e�[�W�̈ړ�
            Ret = MoveCleaningPosXY(MoveX, MoveY)
            If Ret <> 0 Then
                Return (Ret)
            End If

            If Form1.stFNC(F_PROBE_CLEANING).iDEF = 1 Then   ' = 1 �A�b�v�_�E�����[�h
                If typPlateInfo.intResistDir = 0 Then   ' ��R(����)���ѕ���(0:X, 1:Y)
                    PosX = 0.0
                    PosY = 1.0
                Else
                    PosX = 1.0
                    PosY = 0.0
                End If
                ' �ŏ��̃v���[�u�s��
                ' ���K�̈ʒu�ŃA�b�v�_�E��
                Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                If Ret <> 0 Then
                    Return (Ret)
                End If

                ' �N���[�j���O�I�t�Z�b�g���X�e�[�W�ړ�
                If typPlateInfo.dblPrbCleaningOffset <> 0.0 Then
                    MoveX = PosX * typPlateInfo.dblPrbCleaningOffset
                    MoveY = PosY * typPlateInfo.dblPrbCleaningOffset
                    Ret = MoveCleaningPosXY(MoveX, MoveY)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                    Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                End If

                ' �Q�Ԗڂ̃v���[�u�s��
                If typPlateInfo.dblPrbDistance > 0.0 Then
                    MoveX = PosX * typPlateInfo.dblPrbDistance
                    MoveY = PosY * typPlateInfo.dblPrbDistance
                    Ret = MoveCleaningPosXY(MoveX, MoveY)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If
                    Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                    If Ret <> 0 Then
                        Return (Ret)
                    End If

                    ' �N���[�j���O�I�t�Z�b�g���X�e�[�W�ړ�
                    If typPlateInfo.dblPrbCleaningOffset <> 0.0 Then
                        MoveX = PosX * (typPlateInfo.dblPrbCleaningOffset + typPlateInfo.dblPrbDistance)
                        MoveY = PosY * (typPlateInfo.dblPrbCleaningOffset + typPlateInfo.dblPrbDistance)
                        Ret = MoveCleaningPosXY(MoveX, MoveY)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                        Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, 0, 0, 0.0, 0.0)
                        If Ret <> 0 Then
                            Return (Ret)
                        End If
                    End If
                End If          ' ���Q�Ԗڂ̃v���[�u�s��

            Else                    ' = 2 �X�e�[�W�ړ�
                Ret = ProbeDownAndUp(ProbeCleaningCnt, MoveX, MoveY, typPlateInfo.intPrbCleanStageCountX, typPlateInfo.intPrbCleanStageCountY, typPlateInfo.dblPrbCleanStagePitchX, typPlateInfo.dblPrbCleanStagePitchY)
                If Ret <> 0 Then
                    Return (Ret)
                End If
            End If
            'V4.10.0.0�H �X�V��

            'V4.10.0.0�H�R�����g����
            ' �v���[�u�N���[�j���O�ʒu�փX�e�[�W�̈ړ�
            'Ret = MoveCleaningPosXY()
            'If Ret <> 0 Then
            '    Return (Ret)
            'End If

            ' �w��񐔃v���[�u�̏㉺���J��Ԃ� 
            'For i = 0 To ProbeCleaningCnt - 1
            '    '                For i = 0 To typPlateInfo.intPrbCleanUpDwCount - 1
            '    Call System.Threading.Thread.Sleep(500)                 ' �v���[�u�N���[�j���O�ʒu��Z���ʒu�ݒ� 'V4.3.0.0�C
            '    ' �v���[�u�N���[�j���O�ʒu��Z���ʒu�ݒ� 
            '    Ret = MoveCleaningPosZ()
            '    If Ret <> 0 Then
            '        Return (Ret)
            '    End If
            '    Call System.Threading.Thread.Sleep(500)                 ' �v���[�u�N���[�j���O�ʒu��Z���ʒu�ݒ� 
            '    Ret = MoveOrgWaitPosZ()
            '    If Ret <> 0 Then
            '        Return (Ret)
            '    End If

            'Next i
            'V4.10.0.0�H�R�����g����

        Catch ex As Exception
            strMSG = "frmProbeCleaning.ProbeCleaningStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "�X�e�[�W���v���[�u�N���[�j���O�ʒu�ֈړ�"
    '''=========================================================================
    ''' <summary>
    ''' �X�e�[�W���v���[�u�N���[�j���O�ʒu�ֈړ� 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    '''V4.10.0.0�H    Public Function MoveCleaningPosXY() As Integer
    Private Shared Function MoveCleaningPosXY(ByVal OffsetX As Double, ByVal OffsetY As Double) As Integer   'V6.0.0.0�J
        'V6.0.0.0�J        Public Function MoveCleaningPosXY(ByVal OffsetX As Double, ByVal OffsetY As Double) As Integer
        Dim PosX As Double
        Dim PosY As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            '----- V2.0.0.0_30�� -----
            'PosX = typPlateInfo.dblPrbCleanPosX
            'PosY = typPlateInfo.dblPrbCleanPosY
            'V4.10.0.0�H            PosX = globaldblCleaningPosX
            'V4.10.0.0�H            PosY = globaldblCleaningPosY
            PosX = globaldblCleaningPosX + OffsetX      'V4.10.0.0�H
            PosY = globaldblCleaningPosY + OffsetY      'V4.10.0.0�H
            '----- V2.0.0.0_30�� -----
            '----- V4.0.0.0-53�� -----
            'If (Double.TryParse(Me.txtStagePosX.Text, PosX) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            'If (Double.TryParse(Me.txtStagePosY.Text, PosY) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            '----- V4.0.0.0-53�� -----
            Ret = Form1.System1.XYtableMove(gSysPrm, PosX, PosY)
            If (Ret <> cFRS_NORMAL) Then                              ' �G���[ ?
                Return (Ret)                                          ' �G���[���^�[��
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosXY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "�y�����v���[�u�N���[�j���O�ʒu�ֈړ�"
    '''=========================================================================
    ''' <summary>
    ''' �y�����v���[�u�N���[�j���O�ʒu�ֈړ� 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Shared Function MoveCleaningPosZ() As Integer    'V6.0.0.0�J
        'V6.0.0.0�J    Public Function MoveCleaningPosZ() As Integer

        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            '----- V2.0.0.0_30�� -----
            PosZ = globaldblCleaningPosZ
            'V4.3.0.0�B PosZ = typPlateInfo.dblPrbCleanPosZ
            '----- V2.0.0.0_30�� -----
            '----- V4.0.0.0-53�� -----
            'If (Double.TryParse(Me.txtZPos.Text, PosZ) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            '----- V4.0.0.0-53�� -----

            ' Z���v���[�u�N���[�j���O�ʒu�ֈړ�����
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' �G���[��EX_MOVE���ŕ\���ς�
            If (Ret <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "�y�������_�ʒu�ֈړ�"
    '''=========================================================================
    ''' <summary>
    ''' �y�������_�ʒu�ֈړ� 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function MoveOrgPosZ() As Integer
        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            PosZ = 0
            ' Z��ҋ@�ʒu�ֈړ�����
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' �G���[��EX_MOVE���ŕ\���ς�
            If (Ret <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function

    '''=========================================================================
    ''' <summary>
    ''' �y�������_�ʒu�ړ��܂��͌��݈ʒu���1mm��ɏグ�� 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Shared Function MoveOrgWaitPosZ() As Integer        'V6.0.0.0�J
        'V6.0.0.0�J    Public Function MoveOrgWaitPosZ() As Integer
        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            If (gdblCleaningPosZ > CLEANING_OFFSET) Then
                'V4.10.0.0�H ���̃v���O�����ł�gdblCleaningPosZ�̒l�͏�ɂO�Ȃ̂ł��̏������s���邱�Ƃ͖����B�d�l���s���B
                PosZ = gdblCleaningPosZ - CLEANING_OFFSET
            Else
                PosZ = 0
            End If
            ' Z��ҋ@�ʒu�ֈړ�����
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' �G���[��EX_MOVE���ŕ\���ς�
            If (Ret <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

#Region "�v���[�u�N���[�j���O�ʒu��ۑ����ďI��"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�ʒu��ۑ����ďI��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        stJOG.Flg = cFRS_ERR_START                                      ' OK(START��)

    End Sub
#End Region

#Region "�v���[�u�N���[�j���O�ʒu��ۑ������ɏI��"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�ʒu��ۑ������ɏI�� 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        stJOG.Flg = cFRS_ERR_RST                                      ' OK(START��)

    End Sub
#End Region

#Region "�����l�̐ݒ�"
#If False Then                          'V6.0.0.0�J �R���X�g���N�^�̈����Őݒ肷��
    '''=========================================================================
    ''' <summary>
    ''' �����l�̐ݒ�  
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function sSetInitVal() As Integer

        Dim strMSG As String

        Try
            '----- V2.0.0.0_30�� -----
            gdblCleaningPosX = typPlateInfo.dblPrbCleanPosX     'V6.0.0.0�J �R���X�g���N�^�̈����Őݒ肷��
            gdblCleaningPosY = typPlateInfo.dblPrbCleanPosY     'V6.0.0.0�J �R���X�g���N�^�̈����Őݒ肷��
            'gdblCleaningPosX = globaldblCleaningPosX
            'gdblCleaningPosY = globaldblCleaningPosY
            '----- V2.0.0.0_30�� -----

            '----- V4.0.0.0-29�� -----
            ' ���x������ݒ肷��(���{��E�p��)
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    ' �O���[�v��
            '    grpBpOff.Text = "�ʒu���"
            '    GroupBox1.Text = "���݈ʒu"
            '    GroupBox2.Text = "�o�^�ʒu"
            '    GroupBox3.Text = "�v���[�u�N���[�j���O�p�����[�^"

            '    ' ���x��
            '    LblContactCount.Text = "�v���[�u�ڐG��"
            '    LblProbingCnt.Text = "�N���[�j���O�������s�Ԋu"
            '    LblRemark01.Text = "(0:�N���[�j���O����)"

            '    ' �{�^��
            '    btnCleaningStart.Text = "�N���[�j���O���s"
            '    btnStageRegPosMove.Text = "�X�e�[�W�o�^�ʒu�ړ�"
            '    btnZRegMove.Text = "Z���o�^�ʒu�ړ�"

            'Else
            '    ' �O���[�v��
            '    grpBpOff.Text = "POSITION INFORMATION"
            '    GroupBox1.Text = "CURRENT POSITION"
            '    GroupBox2.Text = "RESISTRATION POSITION"
            '    GroupBox3.Text = "PROBE CLEANING PARAMETER"

            '    ' ���x��
            '    LblContactCount.Text = "UP AND DOWN COUNT"
            '    LblProbingCnt.Text = "EXECUTION NUMBER"
            '    LblRemark01.Text = "(0:NO EXECUTE)"

            '    ' �{�^��
            '    btnCleaningStart.Text = "CLEANING START"
            '    btnStageRegPosMove.Text = "MOVE STAGE POSITION"
            '    btnZRegMove.Text = "MOVE Z POSITION"
            'End If

            '----- V4.0.0.0-29�� -----

            Return cFRS_NORMAL

        Catch ex As Exception
            strMSG = "frmProbeCleaning.sSetInitVal() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End If
#End Region

#Region "���s���ʂ̎擾"
    ' '''=========================================================================
    ' ''' <summary>
    ' ''' ���s���ʂ̎擾 
    ' ''' </summary>
    ' ''' <remarks></remarks>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0�J
    '    'V6.0.0.0�J        Public Function sGetReturn() As Integer
    '    Get
    '        Return cFRS_NORMAL
    '    End Get
    'End Property
#End Region

#Region "�v���[�u�N���[�j���O�ʒu�̍X�V"
    '''=========================================================================
    ''' <summary>
    ''' �v���[�u�N���[�j���O�ʒu�̍X�V 
    ''' </summary>
    ''' <param name="CleaningPosZ"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub DispProbePos(ByVal CleaningPosZ As Double)                 ' �ʒu�\��(Z)
        Dim strMSG As String

        Try
            Me.txtZPos.Text = CleaningPosZ.ToString("0.000")            ' V4.0.0.0-53
            Me.txtZPos.BackColor = System.Drawing.Color.White

            Me.Refresh()

        Catch ex As Exception
            strMSG = "frmProbeCleaning.DispProbePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return
        End Try

    End Sub
#End Region

    '----- V4.0.0.0-53�� -----
    '========================================================================================
    '   �{�^������������
    '========================================================================================
#Region "OK���݉���������"
    '''=========================================================================
    ''' <summary>OK���݉���������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdOK.MouseUp  'V1.16.0.0�L

        stJOG.Flg = cFRS_ERR_START                                      ' OK(START��)

    End Sub
#End Region

#Region "��ݾ����݉���������"
    '''=========================================================================
    ''' <summary>��ݾ����݉���������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdCancel.MouseUp 'V1.16.0.0�L

        stJOG.Flg = cFRS_ERR_RST                           ' Cancel(RESET��)

    End Sub
#End Region

#Region "HI�{�^������������"
    '''=========================================================================
    '''<summary>HI�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHI_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnHI.MouseDown
        Call SubBtnHI_Click(stJOG)
    End Sub
#End Region

#Region "���{�^���̃}�E�X�N���b�N������"
    '''=========================================================================
    ''' <summary>���{�^���̃}�E�X�N���b�N������</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnJOG_0_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_0_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +Y ON
    End Sub
    Private Sub BtnJOG_0_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseUp
        Call SubBtnJOG_0_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_1_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -Y ON
    End Sub
    Private Sub BtnJOG_1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseUp
        Call SubBtnJOG_1_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -Y OFF
        Timer1.Enabled = True
    End Sub
    Private Sub BtnJOG_2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_2_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X ON
    End Sub
    Private Sub BtnJOG_2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseUp
        Call SubBtnJOG_2_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_3_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X ON
    End Sub
    Private Sub BtnJOG_3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseUp
        Call SubBtnJOG_3_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_4_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X -Y ON
    End Sub
    Private Sub BtnJOG_4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseUp
        Call SubBtnJOG_4_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X -Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_5_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_5_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X -Y ON
    End Sub
    Private Sub BtnJOG_5_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseUp
        Call SubBtnJOG_5_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X -Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_6_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_6_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X +Y ON
    End Sub
    Private Sub BtnJOG_6_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseUp
        Call SubBtnJOG_6_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
        Timer1.Enabled = True
    End Sub

    Private Sub BtnJOG_7_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseDown
        Timer1.Enabled = False
        Call SubBtnJOG_7_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X +Y ON
    End Sub
    Private Sub BtnJOG_7_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseUp
        Call SubBtnJOG_7_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
        Timer1.Enabled = True
    End Sub
#End Region

#Region "���[�h�ύX��"
    '''=========================================================================
    ''' <summary>���[�h�ύX��</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub cmbMoveMode_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmbMoveMode.MouseDown
        Timer1.Enabled = False
    End Sub

    Private Sub cmbMoveMode_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMoveMode.SelectionChangeCommitted
        Timer1.Enabled = True
    End Sub
#End Region
    '========================================================================================
    '   �e���L�[���͏���
    '========================================================================================
#Region "�L�[�_�E��������"
    '''=========================================================================
    '''<summary>�L�[�_�E��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmProbeCleaning_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(e As KeyEventArgs) Implements ICommonMethods.JogKeyDown   'V6.0.0.0�J
        If (False = _tenKeyON) Then Return 'V6.0.0.1�A
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J
        Console.WriteLine("frmProbeCleaning.frmProbeCleaning_KeyDown()")

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V4.10.0.0�H        GrpMain.Enabled = False
        'V6.0.0.0�K       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0�K
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5�� (KeyCode = 101(&H65)
            Call SubBtnHI_Click(stJOG)                                  ' HI�{�^�� ON/OFF
        End If
        'V4.10.0.0�H        Call Me.Focus()

    End Sub
#End Region

#Region "�L�[�A�b�v������"
    '''=========================================================================
    '''<summary>�L�[�A�b�v������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmProbeCleaning_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        JogKeyUp(e)                     'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(e As KeyEventArgs) Implements ICommonMethods.JogKeyUp       'V6.0.0.0�J
        If (False = _tenKeyON) Then Return 'V6.0.0.1�A
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J

        Console.WriteLine("frmProbeCleaning.frmProbeCleaning_KeyKeyUp()")
        ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
        'V4.10.0.0�H        GrpMain.Enabled = True
        'V6.0.0.0�K        Call Sub_10KeyUp(KeyCode)
        Sub_10KeyUp(KeyCode, stJOG)                   'V6.0.0.0�K
        'V4.10.0.0�H        Call Me.Focus()

    End Sub
#End Region

#Region "�J�����摜�N���b�N�ʒu���Z���^�[�Ɉړ����鏈��"
    ''' <summary>�J�����摜�N���b�N�ʒu���Z���^�[�Ɉړ����鏈��</summary>
    ''' <param name="distanceX">�摜�Z���^�[����̋���X</param>
    ''' <param name="distanceY">�摜�Z���^�[����̋���Y</param>
    ''' <remarks>'V6.0.0.0�J</remarks>
    Public Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal) _
        Implements ICommonMethods.MoveToCenter

        Globals_Renamed.MoveToCenter(distanceX, distanceY, stJOG)
    End Sub
#End Region

    '========================================================================================
    '   �g���b�N�o�[����
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

#Region "�L�[�X�e�[�^�X�擾"
    '''=========================================================================
    ''' <summary>�L�[�X�e�[�^�X�擾</summary>
    ''' <param name="Code">(INP)�L�[�R�[�h</param>
    ''' <returns></returns>
    '''=========================================================================
    Private Function Sub_GetKeyState(ByVal Code As Integer) As Integer

        Dim keyState As Integer

        keyState = GetKeyState(Code)
        Return (keyState)
    End Function
#End Region

#Region "�L�[�X�e�[�^�X�`�F�b�N�^�C�}�["
    '''=========================================================================
    ''' <summary>�L�[�X�e�[�^�X�`�F�b�N�^�C�}�[</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
#If False Then                          'V6.0.0.0-22
        Dim KeyCode As Integer
        Dim keyState As Integer
        Dim Count As Integer
        Dim strMSG As String
        Dim hit As Integer
        Try
            Timer1.Enabled = False
            hit = 1

            KeyCode = System.Windows.Forms.Keys.NumPad1
            For Count = 1 To 9                                              ' 1-9�܂ŌJ��Ԃ� 
                keyState = Sub_GetKeyState(KeyCode)
                If (keyState < 0) Then
                    'Call Sub_10KeyDown(KeyCode)
                    'If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then   ' 5�� (KeyCode = 101(&H65)
                    '    Call BtnHI_Click(sender, e)                         ' HI�{�^�� ON/OFF
                    'End If
                Else
                    ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
                    'V6.0.0.0�K                    grpBpOff.Enabled = True
                    'V6.0.0.0�K        Call Sub_10KeyUp(KeyCode)
                    Sub_10KeyUp(KeyCode, buttons)                   'V6.0.0.0�K
                    'V6.0.0.0�K                    Call Me.Focus()
                End If
                KeyCode = KeyCode + 1
            Next

            'strMSG = "Keys.NumPad9=" + keyState.ToString("")
            'Console.WriteLine(strMSG)
            'MsgBox(strMSG, MsgBoxStyle.OkOnly)

            Timer1.Enabled = True

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.Timer1_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
#End If
    End Sub
#End Region
    '----- V4.0.0.0-53�� -----
#End Region

    ''' <summary>
    ''' �v���[�u�e�B�[�`���O��ʂ�Z�{�^���������ꂽ�Ƃ��̏��� �@'V4.0.0.0-78
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnZ_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnZ.MouseDown
        Dim Ret As Integer

        If cmbMoveMode.SelectedIndex = 0 Then
            'XY���[�h�̂Ƃ��͖���
            giJogButtonEnable = 0
        Else
            If BtnZ.BackColor = Color.Yellow Then
                giJogButtonEnable = 0
                'V6.0.0.1�A                BtnZ.BackColor = Color.LightGray
                BtnZ.BackColor = SystemColors.Control                   'V6.0.0.1�A
                Ret = MoveOrgPosZ()
                If Ret <> 0 Then
                    MsgBox("Z Axis Org move Error. ")
                End If
                BtnZ.Text = "Z Off"     'V4.10.0.0�H
                stJOG.bZ = False        'V6.0.0.0�R
            Else
                giJogButtonEnable = 1
                'V4.0.0.0-78
                BtnZ.BackColor = Color.Yellow
                ' �v���[�u�N���[�j���O�ʒu��Z���ʒu�ݒ� 
                'Ret = MoveCleaningPosZ()
                Ret = MoveTempCleaningPosZ()
                If Ret <> 0 Then
                    MsgBox("Z Axis move Error. ")
                End If
                BtnZ.Text = "Z On"      'V4.10.0.0�H
                stJOG.bZ = True         'V6.0.0.0�R
            End If
        End If
    End Sub



#Region "�y�����ꎞ�I�ȃv���[�u�N���[�j���O�ʒu�ֈړ�"
    '''=========================================================================
    ''' <summary>
    ''' �y�����ꎞ�I�ȃv���[�u�N���[�j���O�ʒu�ֈړ� 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function MoveTempCleaningPosZ() As Integer

        Dim PosZ As Double
        Dim Ret As Integer
        Dim strMSG As String

        Try
            '----- V2.0.0.0_30�� -----
            'PosZ = globaldblCleaningPosZ
            PosZ = CleaningPosZ
            '----- V2.0.0.0_30�� -----
            '----- V4.0.0.0-53�� -----
            'If (Double.TryParse(Me.txtZPos.Text, PosZ) = False) Then
            '    Return (cFRS_NORMAL)
            'End If
            '----- V4.0.0.0-53�� -----

            ' Z���v���[�u�N���[�j���O�ʒu�ֈړ�����
            Ret = EX_ZMOVE(PosZ, MOVE_ABSOLUTE)
            ' �G���[��EX_MOVE���ŕ\���ς�
            If (Ret <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (Ret)
            End If

        Catch ex As Exception
            strMSG = "frmProbeCleaning.MoveCleaningPosZ() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

    End Function
#End Region

    'V4.10.0.0�H��
#Region "�e���L�[�n�m���̏���"
    Private Sub BtnTenKey_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnTenKey.MouseUp
        'Dim InpKey As UShort
        Dim strMSG As String

        Try
            ' InpKey��HI SW�ȊO��OFF����
            'GetInpKey(InpKey)
            'If (InpKey And cBIT_HI) Then                                ' HI SW ON ?
            '    InpKey = cBIT_HI
            'Else
            '    InpKey = 0
            'End If
            'PutInpKey(InpKey)

            ' Ten Key On/Off�{�^���ݒ�
            If (BtnTenKey.Text = "Ten Key Off") Then
                'V6.0.0.0�O                gbTenKeyFlg = True
                _tenKeyON = True       'V6.0.0.0�O
                BtnTenKey.Text = "Ten Key On"
                BtnTenKey.BackColor = System.Drawing.Color.Pink
                AddHandler Me.KeyDown, AddressOf frmProbeCleaning_KeyDown
                AddHandler Me.KeyUp, AddressOf frmProbeCleaning_KeyUp
                Form1.Instance.SetActiveJogMethod(AddressOf Me.JogKeyDown,
                                                  AddressOf Me.JogKeyUp,
                                                  AddressOf Me.MoveToCenter)  'V6.0.0.0�J
                GroupBox3.Enabled = False
            Else
                'V6.0.0.0�O                gbTenKeyFlg = False
                _tenKeyON = False      'V6.0.0.0�O
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control
                RemoveHandler Me.KeyDown, AddressOf frmProbeCleaning_KeyDown
                RemoveHandler Me.KeyUp, AddressOf frmProbeCleaning_KeyUp
                Form1.Instance.SetActiveJogMethod(Nothing,
                                                  Nothing,
                                                  AddressOf Me.MoveToCenter)  'V6.0.0.0�J
                GroupBox3.Enabled = True
            End If
            Sub_10KeyUp(Keys.None, stJOG)                               'V6.0.1.0�A

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.SubBtnTenKey_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region
    'V4.10.0.0�H��
End Class

