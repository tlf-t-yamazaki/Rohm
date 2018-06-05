'===============================================================================
'   Description  : �h�c���[�_�[�e�B�[�`���O��ʏ��� V1.13.0.0�E
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'�@frmTxTyTeach.vb���x�[�X�ɉ���
'
'  2013/11/07  Written by N.Arata
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmIDReaderTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, _
         ByVal hWndInsertAfter As IntPtr, ByVal x As Integer, ByVal y As Integer, _
             ByVal width As Integer, ByVal height As Integer, _
         ByVal flags As Integer) As UInt32
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function _
      SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function

#Region "�v���C�x�[�g�ϐ���`"
    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer) As Integer

    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- �I�u�W�F�N�g��` -----
    Private stJOG As JOG_PARAM                                      ' �����(BP��JOG����)�p�p�����[�^
    Private mExit_flg As Short                                      ' �h�c���[�_�[�e�B�[�`���O����

    '----- ���̑� -----
    Private dblTchMoval(3) As Double                                ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))


    '----- �g���~���O�p�����[�^ -----
    Private dblIDReadPos1X As Double                        ' IDذ�ޓǂݎ���߼޼�� 1X
    Private dblIDReadPos1Y As Double                        ' IDذ�ޓǂݎ���߼޼�� 1Y
    Private dblIDReadPos2X As Double                        ' IDذ�ޓǂݎ���߼޼�� 2X
    Private dblIDReadPos2Y As Double                        ' IDذ�ޓǂݎ���߼޼�� 2Y

    Public ObjOmronIDReader As New TrimClassLibrary.OmronIDReader

    Private TouchFinderProcessId As Integer = 0

    Private IdReaderPos As Integer = 1
#End Region

#Region "�I�����ʂ�Ԃ�"
    ' '''=========================================================================
    ' '''�@<summary>�I�����ʂ�Ԃ�</summary>
    ' '''�@<returns>cFRS_ERR_START = OK(START��)
    ' '''  �@       cFRS_ERR_RST   = Cancel(RESET��)
    ' '''    �@     cFRS_TxTy      = TX2(Teach)/TY2����
    ' '''      �@   -1�ȉ�         = �G���[</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0�J
    '    'V6.0.0.0�J    Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        sGetReturn = mExit_flg
    '    End Get
    'End Property
#End Region

    '=========================================================================
    '   �t�H�[���̏�����/�I������
    '=========================================================================
#Region "Form Initialize������"
    '''=========================================================================
    '''<summary>Form Initialize������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        Dim strMSG As String

        Try
            'V6.0.0.0�K                  ��
            stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                         BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            'V6.0.0.0�K                  ��

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Form_Initialize_Renamed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form �I��������"
    '''=========================================================================
    '''<summary>Form �I��������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmIDReaderTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim strMSG As String

        Try
            If Not TouchFinderProcess.HasExited Then    ' �^�b�`�t�@�C���_�[���N�������܂܂Ȃ�I��������B
                TouchFinderProcess.Kill()
            End If

            ObjOmronIDReader = Nothing

        Catch ex As System.InvalidOperationException
            ' �h���̃I�u�W�F�N�g�Ɋ֘A�t�����Ă���v���Z�X�͂���܂���B�h�͉������Ȃ�
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.frmIDReaderTeach_FormClosed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Load������"
    '''=========================================================================
    '''<summary>Form Load������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmIDReaderTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ��������
            Call SetDispMsg()                                       ' ���x������ݒ肷��(���{��/�p��)
            mExit_flg = -1                                          ' �I���t���O = ������

            ' ����޳����۰قɏd�˂�
            Me.Top = Form1.VideoLibrary1.Top - Me.lblTitle2.Top
            Me.Left = Form1.VideoLibrary1.Right

            ' �g���~���O�f�[�^���K�v�ȃp�����[�^���擾����
            Call SetTrimData()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.frmIDReaderTeach_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���x������ݒ肷��(���{��/�p��)"
    '''=========================================================================
    '''<summary>���x������ݒ肷��(���{��/�p��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetDispMsg()

        Dim strMSG As String

        Try
            ' ���x������ݒ肷��
            With Me
                '.lblTitle.Text = LBL_IDREADER_TEACH_01                  ' ����
                .lblTitle2.Text = ""

                ' �h�c���[�_�[�e�B�[�`���O����
                ' frame
                '.GrpFram_0.Text = LBL_IDREADER_TEACH_02                 ' ��P�h�c�ǂݎ��ʒu
                '.GrpFram_1.Text = LBL_IDREADER_TEACH_03                 ' ��Q�h�c�ǂݎ��ʒu
                '.GrpFram_2.Text = LBL_IDREADER_TEACH_04                 ' �h�c���[�_�[
                ' button
                '.cmdOK.Text = "OK"
                '.cmdCancel.Text = CMD_CANCEL
            End With

            ' �\���֘A������
            Call InitDisp()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.SetDispMsg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�\���֘A������"
    '''=========================================================================
    '''<summary>�\���֘A������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitDisp()

        Dim strMSG As String

        Try
            ' ���W�\���N���A
            With Me
                .TxtPosX.Text = ""                                ' ��1�h�c���[�_�[�ʒuXY
                .TxtPosY.Text = ""
                .TxtPos2X.Text = ""                               ' ��2�h�c���[�_�[�ʒuXY
                .TxtPos2Y.Text = ""

            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.InitDisp() TRAP ERROR = " + ex.Message
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
    Private Sub frmIDReaderTeach_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' �h�c���[�_�[�e�B�[�`���O�J�n
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                           ' �I���t���O = 0
            mExit_flg = IdReaderMainProc()                              ' �s�w�܂��͂s�x�e�B�[�`���O�J�n

            ' �␳�N���X���C�����\���Ƃ���
            'V6.0.0.0�C            Form1.CrosLineX.Visible = False
            'V6.0.0.0�C            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.frmIDReaderTeach_Activated() TRAP ERROR = " + ex.Message
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
            ' �h�c���[�_�[�e�B�[�`���O�J�n
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                           ' �I���t���O = 0
            mExit_flg = IdReaderMainProc()                              ' �s�w�܂��͂s�x�e�B�[�`���O�J�n

            ' �␳�N���X���C�����\���Ƃ���
            'V6.0.0.0�C            Form1.CrosLineX.Visible = False
            'V6.0.0.0�C            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "frmIDReaderTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                   ' Return�l = ��O�G���[
        End Try

        Me.Close()
        Return mExit_flg                ' sGetReturn ��荞��   'V6.0.0.0�L

    End Function
#End Region

#Region "OK���݉���������"
    '''=========================================================================
    '''<summary>OK���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click

        stJOG.Flg = cFRS_ERR_START                                      ' OK(START��)

    End Sub
#End Region

#Region "��ݾ����݉���������"
    '''=========================================================================
    '''<summary>��ݾ����݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        stJOG.Flg = cFRS_ERR_RST                           ' Cancel(RESET��)

    End Sub
#End Region

#Region "�s�w�܂��͂s�x�e�B�[�`���O�̃��C������"
    '''=========================================================================
    '''<summary>�s�w�܂��͂s�x�e�B�[�`���O�̃��C������"</summary>
    '''<returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST  =Cancel(RESET��)
    '''         cFRS_TxTy     =TX2/TY2����
    '''         -1�ȉ�        =�G���[</returns>
    '''=========================================================================
    Private Function IdReaderMainProc() As Short

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

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            mdBpOffx = typPlateInfo.dblBpOffSetXDir                     ' BP�ʒu�̾��X,Y�ݒ�
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            dStepOffx = typPlateInfo.dblStepOffsetXDir                  ' �ï�ߵ̾�ė�X,Y(TYè���p)
            dStepOffy = typPlateInfo.dblStepOffsetYDir
            Call CalcBlockSize(mdBSx, mdBSy)                            ' �u���b�N�T�C�YXY�ݒ�


            Call InitDisp()                                             ' ���W�\���N���A
            r = Form1.System1.EX_MOVE(gSysPrm, 0.0, 0.0, 1)             ' BP��( 0.0 , 0.0 )�Ɉړ�

            ' �����(BP��JOG����)�p�p�����[�^������������
            stJOG.Md = MODE_STG                                     ' ���[�h(0:XY�e�[�u���ړ�)
            stJOG.Md2 = MD2_BUTN                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            '                                                           ' �L�[�̗L��(1)/����(0)�w��
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT
            stJOG.PosX = KJPosX                                         ' BP X�ʒu/XYð��� X�ʒu
            stJOG.PosY = KJPosY                                         ' BP Y�ʒu/XYð��� Y�ʒu
            stJOG.BpOffX = mdAdjx + mdBpOffx                            ' BP�̾��X 
            stJOG.BpOffY = mdAdjy + mdBpOffy                            ' BP�̾��Y 
            stJOG.BszX = mdBSx                                          ' ��ۯ�����X 
            stJOG.BszY = mdBSy                                          ' ��ۯ�����Y
            stJOG.TextX = TxtPosX                                       ' BP X�ʒu/XYð��� X�ʒu�\���p÷���ޯ��
            stJOG.TextY = TxtPosY                                       ' BP Y�ʒu/XYð��� Y�ʒu�\���p÷���ޯ��
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

            '-------------------------------------------------------------------
            '   �h�c���[�_�[�̃e�B�[�`���O����
            '-------------------------------------------------------------------
            r = Sub_Jog1()
            Timer1.Enabled = False
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
                Call SetTrimParameter()
                gCmpTrimDataFlg = 1                                     ' �f�[�^�X�V�t���O = 1(�X�V����)
            End If
            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.TxTyMainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�h�c���[�_�[�ʒu�e�B�[�`���O����"

    '''=========================================================================
    ''' <summary>
    ''' �h�c���[�_�[�ʒu�e�B�[�`���O����
    ''' </summary>
    ''' <returns>cFRS_ERR_START = OK(START��)����
    '''          cFRS_ERR_RST   = Cancel(RESET��)����
    '''          cFRS_NORMAL    = ����(�O���[�v�ԃC���^�[�o��������)
    '''          -1�ȉ�         = �G���[</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function Sub_Jog1() As Short

        Dim PosX As Double                                              ' ���݂�BP�ʒuX/ð��وړ����WX(��Βl)
        Dim PosY As Double                                              ' ���݂�BP�ʒuY/ð��وړ����WY(��Βl)
        Dim OffPosX As Double                                              ' ���݂�BP�ʒuX/ð��وړ����WX(��Βl)
        Dim OffPosY As Double                                              ' ���݂�BP�ʒuY/ð��وړ����WY(��Βl)
        Dim iPos As Short
        Dim r As Short
        Dim rtn As Short                                                ' �ߒl 
        Dim strMSG As String
        Dim DirX As Integer = 1, DirY As Integer = 1

        Try
            ' ��������
            Timer1.Enabled = True
            iPos = 0                                                    ' iPos = ��1�h�c���[�_�[�ʒu(0:��1�h�c���[�_�[�ʒu�ʒu, 1:��2�h�c���[�_�[�ʒu�ʒu)
            IdReaderPos = iPos + 1
            Call ObjOmronIDReader.SetIDReaderNo(IdReaderPos)
            TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)

            Me.lblTitle2.Text = LBL_IDREADER_TEACH_01

            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸�
            Me.GrpFram_1.Visible = True                                 ' ����_�\��(Sub_Jog3()�Ŕ�\���ɂ��邽��)
            Call InitDisp()                                             ' ���W�\���N���A

STP_RETRY:
            Call Me.Focus()
            Do
                ObjOmronIDReader.GetIDReaderPotision(gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, typPlateInfo.dblTableOffsetXDir, typPlateInfo.dblTableOffsetYDir, gfCorrectPosX, gfCorrectPosY, OffPosX, OffPosY)
                If (iPos = 0) Then                                  ' ��1�h�c���[�_�[�ʒu
                    PosX = dblIDReadPos1X * DirX + OffPosX
                    PosY = dblIDReadPos1Y * DirY + OffPosY
                Else                                                ' ��2�h�c���[�_�[�ʒu
                    PosX = dblIDReadPos2X * DirX + OffPosX
                    PosY = dblIDReadPos2Y * DirY + OffPosY
                End If
                r = Form1.System1.XYtableMove(gSysPrm, PosX, PosY)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Return (r)                                          ' �G���[���^�[��
                End If

                ' �\�����b�Z�[�W����ݒ肷�� 
                If (iPos = 0) Then                                      ' ��1�h�c���[�_�[�ʒu ? 
                    '"��1�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
                    lblInfo.Text = INFO_MSG19 & (iPos + 1).ToString(0) & LBL_IDREADER_TEACH_04 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPosX                               ' ��1�h�c���[�_�[�ʒu���WX�ʒu�\���p÷���ޯ��
                    stJOG.TextY = TxtPosY                               ' ��1�h�c���[�_�[�ʒu���WY�ʒu�\���p÷���ޯ��
                    TxtPosX.Enabled = True
                    TxtPosY.Enabled = True
                Else                                                    ' ��2�h�c���[�_�[�ʒu 
                    '"��n�O���[�v�A�ŏI��R��ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" & vbCrLf & "[HALT]�łP�O�̏����ɖ߂�܂��B"
                    lblInfo.Text = INFO_MSG19 & (iPos + 1).ToString(0) & LBL_IDREADER_TEACH_04 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPos2X                              ' ��2�h�c���[�_�[�ʒu���WX�ʒu�\���p÷���ޯ��
                    stJOG.TextY = TxtPos2Y                              ' ��2�h�c���[�_�[�ʒu���WY�ʒu�\���p÷���ޯ��
                    TxtPos2X.Enabled = True
                    TxtPos2Y.Enabled = True
                End If

                ' ��1�h�c���[�_�[�ʒu/��2�h�c���[�_�[�ʒu�̃e�B�[�`���O"����
                stJOG.PosX = PosX                                       ' BP X�܂���XYð��� X��Έʒu
                stJOG.PosY = PosY                                       ' BP Y�܂���XYð��� Y��Έʒu
                stJOG.Flg = -1                                          ' �e��ʂ�OK/Cancel���݉����׸�
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�I��
                    Return (r)
                End If

                ' �R���\�[���L�[�`�F�b�N
                If (r = cFRS_ERR_START) Then                            ' START SW���� ?
                    ' �X�e�b�v�I�t�Z�b�g�ʒu�X�V(��1�h�c���[�_�[�ʒu/��2�h�c���[�_�[�ʒu)
                    TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)
                    If (iPos = 0) Then                                  ' ��1�h�c���[�_�[�ʒu
                        dblIDReadPos1X = dblIDReadPos1X + stJOG.cgX * DirX
                        dblIDReadPos1Y = dblIDReadPos1Y + stJOG.cgY * DirY
                    Else                                                ' ��2�h�c���[�_�[�ʒu
                        dblIDReadPos2X = dblIDReadPos2X + stJOG.cgX * DirX
                        dblIDReadPos2Y = dblIDReadPos2Y + stJOG.cgY * DirY
                    End If

                    If (iPos >= 1) Then                                 ' ��1�h�c���[�_�[�ʒu/��2�h�c���[�_�[�ʒu��è��ݸޏI�� ?
                        r = cFRS_NORMAL                             ' Return�l = �O���[�v�ԃC���^�[�o�������� 
                        Exit Do
                    Else
                        If (stJOG.Flg = -1) Then
                            iPos = 1                                    ' iPos = ��2�h�c���[�_�[�ʒu��è��ݸޏ���
                            IdReaderPos = iPos + 1
                            Call ObjOmronIDReader.SetIDReaderNo(IdReaderPos)
                        End If
                    End If

                    ' HALT SW�������͂P�O�̃e�B�[�`���O�֖߂�
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW���� ?
                    If (iPos = 0) Then                                  ' ��1�h�c���[�_�[�ʒu�̃e�B�[�`���O�Ȃ珈�����s

                    Else                                                ' ��2�h�c���[�_�[�ʒu�Ȃ��1�h�c���[�_�[�ʒu�̃e�B�[�`���O��
                        iPos = 0                                        ' iPos = ��1�h�c���[�_�[�ʒu
                        IdReaderPos = iPos + 1
                        Call ObjOmronIDReader.SetIDReaderNo(IdReaderPos)
                    End If

                    '  RESET SW�������͏I���m�F���b�Z�[�W�\����
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW���� ?
                    Exit Do
                End If

                If System.Windows.Forms.Form.ActiveForm IsNot Nothing Then
                    If System.Windows.Forms.Form.ActiveForm.Text <> "IDREADERTEACH" Then
                        Call ClearInpKey()
                    End If
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
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, LBL_IDREADER_TEACH_01)

            ' Cancel(RESET��)�������͏������p������
            If (rtn = cFRS_ERR_RST) Then
                GoTo STP_RETRY                                          ' �����p����
            End If

STP_END:
            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.Sub_Jog1() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

    '=========================================================================
    '   �g���~���O�p�����[�^�̎擾/�X�V����
    '=========================================================================
#Region "�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����(�h�c���[�_�[�e�B�[�`���O)"
    '''=========================================================================
    '''<summary>�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����</summary>
    '''<remarks>�e�C���^�[�o���f�[�^���擾����</remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim strMSG As String

        Try
            '---------------------------------------------------------------
            '   �h�c���[�h�ǂݎ��|�W�V�������擾����B
            '---------------------------------------------------------------
            dblIDReadPos1X = typPlateInfo.dblIDReadPos1X
            dblIDReadPos1Y = typPlateInfo.dblIDReadPos1Y
            dblIDReadPos2X = typPlateInfo.dblIDReadPos2X
            dblIDReadPos2Y = typPlateInfo.dblIDReadPos2Y

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.SetTrimData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�p�����[�^�X�V(�h�c���[�_�[�e�B�[�`���O)"
    '''=========================================================================
    ''' <summary>
    '''�g���~���O�p�����[�^�X�V
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimParameter()

        Dim OffSet As Double = 0.0
        Dim strMSG As String

        Try
            '---------------------------------------------------------------
            '   �g���~���O�p�����[�^�X�V(�s�x�e�B�[�`���O��)
            '---------------------------------------------------------------
            typPlateInfo.dblIDReadPos1X = dblIDReadPos1X
            typPlateInfo.dblIDReadPos1Y = dblIDReadPos1Y
            typPlateInfo.dblIDReadPos2X = dblIDReadPos2X
            typPlateInfo.dblIDReadPos2Y = dblIDReadPos2Y

            Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmIDReaderTeach.SetTrimParameter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   �{�^������������
    '========================================================================================
#Region "HALT�{�^������������"
    '''=========================================================================
    '''<summary>HALT�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHALT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHALT.Click
        'Call SubBtnHALT_Click() ' '###265
    End Sub
#End Region

#Region "START�{�^������������"
    '''=========================================================================
    '''<summary>START�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSTART.Click
        'Call SubBtnSTART_Click() ' '###265
    End Sub
#End Region

#Region "RESET�{�^������������"
    '''=========================================================================
    '''<summary>RESET�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        'Call SubBtnRESET_Click() '###265
    End Sub
#End Region

    Private Sub BtnSTART_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnSTART.MouseDown
        Call SubBtnSTART_Click() ' '###265
    End Sub

    Private Sub BtnHALT_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnHALT.MouseDown
        Call SubBtnHALT_Click() ' '###265
    End Sub

    Private Sub BtnRESET_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnRESET.MouseDown
        Call SubBtnRESET_Click() ' '###265
    End Sub


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
        'Call SubBtnHI_Click(stJOG) ' V1.13.0.0�M
    End Sub
#End Region
    Private Sub BtnHI_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnHI.MouseDown
        Call SubBtnHI_Click(stJOG) '###265
    End Sub

#Region "���{�^��������"
    '''=========================================================================
    '''<summary>���{�^��������</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnJOG_0_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_0_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +Y ON
    End Sub
    Private Sub BtnJOG_0_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_0.MouseUp
        Call SubBtnJOG_0_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_1_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -Y ON
    End Sub
    Private Sub BtnJOG_1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_1.MouseUp
        Call SubBtnJOG_1_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -Y OFF
        Timer1.Enabled = True '###228
    End Sub
    Private Sub BtnJOG_2_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_2_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X ON
    End Sub
    Private Sub BtnJOG_2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_2.MouseUp
        Call SubBtnJOG_2_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_3_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X ON
    End Sub
    Private Sub BtnJOG_3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_3.MouseUp
        Call SubBtnJOG_3_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_4_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X -Y ON
    End Sub
    Private Sub BtnJOG_4_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_4.MouseUp
        Call SubBtnJOG_4_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X -Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_5_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_5_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X -Y ON
    End Sub
    Private Sub BtnJOG_5_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_5.MouseUp
        Call SubBtnJOG_5_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X -Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_6_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_6_MouseDown(stJOG) 'V6.0.0.0-22                                    ' +X +Y ON
    End Sub
    Private Sub BtnJOG_6_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_6.MouseUp
        Call SubBtnJOG_6_MouseUp(stJOG) 'V6.0.0.0-22                                      ' +X +Y OFF
        Timer1.Enabled = True '###228
    End Sub

    Private Sub BtnJOG_7_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseDown
        Timer1.Enabled = False '###228
        Call SubBtnJOG_7_MouseDown(stJOG) 'V6.0.0.0-22                                    ' -X +Y ON
    End Sub
    Private Sub BtnJOG_7_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BtnJOG_7.MouseUp
        Call SubBtnJOG_7_MouseUp(stJOG) 'V6.0.0.0-22                                      ' -X +Y OFF
        Timer1.Enabled = True '###228
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
    Private Sub frmIDReaderTeach_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J
        Console.WriteLine("frmIDReaderTeach.frmIDReaderTeach_KeyDown()")

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V6.0.0.0�K        GrpMain.Enabled = False                                         ' ###085
        'V6.0.0.0�K       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0�K
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5�� (KeyCode = 101(&H65)
            Call SubBtnHI_Click(stJOG)                                  ' V1.13.0.0�M
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
    Private Sub frmIDReaderTeach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J

        Console.WriteLine("frmIDReaderTeach.frmIDReaderTeach_KeyKeyUp()")
        ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
        'V6.0.0.0�K        GrpMain.Enabled = True                                          ' ###085
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
        Try
            Timer1.Enabled = False

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
                    'V6.0.0.0�K                    GrpMain.Enabled = True
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
            strMSG = "frmIDReaderTeach.Timer1_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
#End If
    End Sub
#End Region

#Region "�h�c���[�_�[�ǂݍ��ݏ���"
    '''=========================================================================
    ''' <summary>
    ''' �h�c���[�_�[�ǂݍ��ݏ���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnRead_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRead.MouseDown
        '   End Sub
        '    Private Sub btnRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRead.Click
        Dim cData As String = ""
        Try

            If Not ObjOmronIDReader.IDRead(IdReaderPos, cData) Then
                ' �G���[����
            End If

            TextIdReadData.Text = cData
        Catch ex As Exception
            MsgBox("frmIDReaderTeach.btnRead_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region

#Region "�I���������h�c���[�_�[�p�c�[���^�b�`�t�@�C���_�[�̋N��"
    '''=========================================================================
    ''' <summary>
    ''' �I���������h�c���[�_�[�p�c�[���^�b�`�t�@�C���_�[�̋N��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnTouchFinder_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnTouchFinder.MouseDown
        'End Sub
        'Private Sub btnTouchFinder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTouchFinder.Click
        'Const SWP_NOSIZE As Integer = &H1
        'Const SWP_NOMOVE As Integer = &H2
        'Const SWP_NOACTIVATE As Integer = &H10
        'Const SWP_SHOWWINDOW As Integer = &H40
        Try

            'Me.TopMost = False

            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("TouchFinder")
            If 0 < ps.Length Then
                If TouchFinderProcessId = ps(0).Id Then
                    Microsoft.VisualBasic.Interaction.AppActivate(ps(0).Id)
                End If
                Exit Sub
            End If

            TouchFinderProcess.Start()
            'Dim pName As String = TouchFinderProcess.ProcessName()
            TouchFinderProcessId = TouchFinderProcess.Id

            'SetWindowPos(TouchFinderProcess.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, _
            '        'SWP_NOACTIVATE Or SWP_NOMOVE Or SWP_NOSIZE Or SWP_SHOWWINDOW)


            'Me.TopMost = False
        Catch ex As Exception
            MsgBox("frmIDReaderTeach.btnTouchFinder_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region
    Public Sub ToucFinderUp()
        Try

            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("TouchFinder")
            If 0 < ps.Length Then
                ' If TouchFinderProcessId = ps(0).Id Then
                Microsoft.VisualBasic.Interaction.AppActivate(ps(0).Id)
                'End If
            End If
            Exit Sub
        Catch ex As Exception
            ' �G���[�͕\�����Ȃ�            MsgBox("frmIDReaderTeach.ToucFinderUp() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub btnTouchFinder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTouchFinder.Click

    End Sub
End Class

'=============================== END OF FILE ===============================