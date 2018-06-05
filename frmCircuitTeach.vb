'===============================================================================
'   Description  : �T�[�L�b�g�e�B�[�`���O��ʏ���(NET�p)
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0
'Imports VB = Microsoft.VisualBasic

Friend Class frmCircuitTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

#Region "�v���C�x�[�g�ϐ���`"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- �ϐ���` -----
    Private stJOG As JOG_PARAM                                          ' �����(BP��JOG����)�p�p�����[�^
    Private mExit_flg As Short                                          ' �I���t���O
    Private Dspx As Double
    Private Dspy As Double                                              ' ���W�\���p
    Private cpCirAxisInfoArray(MaxCntCircuit + 1) As CirAxisInfo        ' ����č��W

    '----- ���̑� -----
    Private dblTchMoval(3) As Double                                    ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))
#End Region

#Region "�I�����ʂ�Ԃ�"
    ' '''=========================================================================
    ' '''<summary>�I�����ʂ�Ԃ�</summary>
    ' '''<returns>cFRS_ERR_START=OK(START��)
    ' '''         cFRS_ERR_RST=Cancel(RESET��)
    ' '''         -1�ȉ�      =�G���[</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0�J
    '    'V6.0.0.0�J       'Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        sGetReturn = mExit_flg
    '    End Get
    'End Property
#End Region

#Region "̫�я�����������"
    '''=========================================================================
    '''<summary>̫�я�����������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()
        MessageBox.Show(Me.GetType().Name & " �ł́AFlexGrid�iFGridCir�j��" & Environment.NewLine & _
                        "DataGridView�idgvCir�j�ɒu�������Ă��܂���" & Environment.NewLine & _
                        "���@�ł̃f�o�b�O�������Ȃ��Ă��܂���B")

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
    Private Sub frmCircuitTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub
#End Region

#Region "̫��۰�ގ�����"
    '''=========================================================================
    '''<summary>̫��۰�ގ�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmCircuitTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim i As Short

        ' ��������
        Call CirSetMessage()                                            ' ���x������ݒ肷��(���{��/�p��)
        mExit_flg = -1                                                  ' �I���t���O = ������

        ' ����޳����۰قɏd�˂�
        Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
        Me.Left = Form1.Text4.Left

        lblCirInfo.Text = ""
        TxtPosX.Text = ""
        TxtPosY.Text = ""

        ' ����č��W�ް����ޯ�����
        For i = 1 To MaxCntCircuit
            cpCirAxisInfoArray(i) = typCirAxisInfoArray(i)
        Next i

        ' �ڷ���ٸ�د�ޏ�����
        Call CirGridInitialize()

    End Sub
#End Region

#Region "���x������ݒ肷��(���{��/�p��)"
    '''=========================================================================
    '''<summary>���b�Z�[�W�ݒ�</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CirSetMessage()

        'If gSysPrm.stTMN.giMsgTyp = 0 Then
        '    lblTitle.Text = "�T�[�L�b�g�e�B�[�`���O"
        '    GrpCir1.Text = "�e�B�[�`���O�|�C���g"
        '    cmdCancel.Text = "Cancel"
        '    cmdOK.Text = "OK"
        '    lblCirInfo.Text = ""

        'Else
        '    lblTitle.Text = "Circuit Standard Position Teaching"
        '    GrpCir1.Text = "Teaching Point"
        '    cmdCancel.Text = "Cancel"
        '    cmdOK.Text = "OK"
        '    lblCirInfo.Text = ""
        'End If
        lblCirInfo.Text = ""

        ' ���x��
        'LblCir1X.Text = LBL_Ex_Cam_01                                   '"�w��"
        'LblCir1Y.Text = LBL_Ex_Cam_02                                   '"�x��"

    End Sub
#End Region

#Region "Form Activated������"
#If False Then                          'V6.0.0.0�L Execute()�ł����Ȃ�
    '''=========================================================================
    '''<summary>Form Activated������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmCircuitTeach_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' TY2è��ݸފJ�n
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                               ' �I���t���O = 0
            mExit_flg = CirMain()                                       ' �����è��ݸފJ�n

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmCircuitTeach.frmCircuitTeach_Activated() TRAP ERROR = " + ex.Message
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
            ' TY2è��ݸފJ�n
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                               ' �I���t���O = 0
            mExit_flg = CirMain()                                       ' �����è��ݸފJ�n

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "frmCircuitTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return�l = ��O�G���[
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

#Region "�T�[�L�b�g�e�B�[�`���O����"
    '''=========================================================================
    '''<summary>�����è��ݸ�Ҳ�</summary>
    '''<returns>0=OK, 1=NG</returns>
    '''=========================================================================
    Private Function CirMain() As Short

        Dim Cdir As Short                                               ' ���ߕ��ѕ���
        Dim CirNum As Short                                             ' 1��ٰ�ߓ໰��Đ�
        Dim Tofx As Double                                              ' ð��وʒu�̾��
        Dim Tofy As Double
        Dim Bofx As Double                                              ' �ް��߼޼�ňʒu�̾��
        Dim Bofy As Double
        Dim ADJX As Double                                              ' ��ެ���߲�Ĉʒu�̾��
        Dim ADJY As Double
        Dim BSX As Double                                               ' ��ۯ� ����
        Dim BSY As Double

        Dim CNO As Short                                                ' ����Đ�����
        Dim i As Short                                                  ' �����
        Dim rtn As Short                                                ' CirMain�ߒl 
        Dim r As Short
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �e���ް��擾
            rtn = 0                                                     ' CirMain�ߒl 
            Cdir = typPlateInfo.intResistDir                            ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
            CirNum = typPlateInfo.intCircuitCntInBlock                  ' 1��ۯ��໰��Đ�
            ' ð��وʒu�̾��X,Y
            Tofx = typPlateInfo.dblTableOffsetXDir
            Tofy = typPlateInfo.dblTableOffsetYDir
            ' BP�ʒu�̾��X,Y
            Bofx = typPlateInfo.dblBpOffSetXDir
            Bofy = typPlateInfo.dblBpOffSetYDir
            ' ��ެ���߲�Ĉʒu�̾��X,Y
            ADJX = typPlateInfo.dblAdjOffSetXDir
            ADJY = typPlateInfo.dblAdjOffSetYDir
            ' ��ۯ�����X,Y
            Call CalcBlockSize(BSX, BSY)

            ' �擪��ۯ���XYð��وړ�
            'r = XYTableMoveBlock(0, 1, 1, 1, 1)
            ' BP���I�t�Z�b�g�ʒu�Ɉړ�����
            Call BpMoveOrigin_Ex()
            r = Form1.System1.EX_MOVE(gSysPrm, ADJX, ADJY, 1)
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' �G���[���^�[��
            End If

            ' �����(XYð��ق�JOG����)�p�p�����[�^������������
            stJOG.Md = MODE_BP                                          ' ���[�h(1:BP�ړ�)
            stJOG.Md2 = MD2_BUTN                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_HALT ' �L�[�̗L��(1)/����(0)�w��
            stJOG.PosX = 0.0                                            ' BP X�ʒu/XYð��� X�ʒu
            stJOG.PosY = 0.0                                            ' BP Y�ʒu/XYð��� Y�ʒu
            stJOG.BpOffX = ADJX + Bofx                                  ' BP�̾��X 
            stJOG.BpOffY = ADJY + Bofy                                  ' BP�̾��Y 
            stJOG.BszX = BSX                                            ' ��ۯ�����X 
            stJOG.BszY = BSY                                            ' ��ۯ�����Y
            stJOG.TextX = TxtPosX                                       ' XYð��� X�ʒu�\���p÷���ޯ��
            stJOG.TextY = TxtPosY                                       ' XYð��� Y�ʒu�\���p÷���ޯ��
            stJOG.cgX = 0.0                                             ' �ړ���X 
            stJOG.cgY = 0.0                                             ' �ړ���Y 
            stJOG.BtnHI = BtnHI                                         ' HI�{�^��
            stJOG.BtnZ = BtnZ                                           ' Z�{�^��
            stJOG.BtnSTART = BtnSTART                                   ' START�{�^��
            stJOG.BtnHALT = BtnHALT                                     ' HALT�{�^��
            stJOG.BtnRESET = BtnRESET                                   ' RESET�{�^��
            Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            Call Me.Focus()
            CNO = 1                                                     ' ����Đ�����

STP_RETRY:
            '-------------------------------------------------------------------
            '   �P�u���b�N���̃T�[�L�b�g���W�̃e�B�[�`���O�������s��
            '-------------------------------------------------------------------
            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸ޏ�����
            stJOG.Md = MODE_BP                                          ' ���[�h(1:BP�ړ�)

            Do
                ' �T�[�L�b�g�����̏��������H
                System.Windows.Forms.Application.DoEvents()
                If (CNO > CirNum) Then                                  ' �T�[�L�b�g����ٰ�� ?
                    Exit Do
                End If

                ' ��n����Ă�BP���W�擾
                With cpCirAxisInfoArray(CNO)
                    Dspx = .dblCaP2                                     ' ����Ċ���WX
                    Dspy = .dblCaP3                                     ' ����Ċ���WY
                End With
                ' ��n����č��WXY��BP��Βl�ړ�
                r = Form1.System1.EX_MOVE(gSysPrm, Dspx, Dspy, 1)
                If (r < cFRS_NORMAL) Then
                    Return (r)                                          ' �G���[���^�[��
                End If

                ' ��د�ޕ\��(�I��)
                Call FlxGridSet(CNO, 1, Dspx, 1)
                Call FlxGridSet(CNO, 2, Dspy, 1)

                ' �e�B�[�`���O����
                lblCirInfo.Text = CIRTEACH_MSG01                        ' "�T�[�L�b�g��_�w�x�����킹�Ă���START��L�[�������ĉ�����"
                stJOG.PosX = Dspx                                       ' BP X�ʒu
                stJOG.PosY = Dspy                                       ' BP Y�ʒu
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J 

                ' ��د�ޕ\��(��I��)
                Call FlxGridSet(CNO, 1, stJOG.PosX, 0)
                Call FlxGridSet(CNO, 2, stJOG.PosY, 0)

                ' �e�B�[�`���O�����̖ߒl���`�F�b�N����
                If (r < cFRS_NORMAL) Then                               ' �G���[ ?
                    Return (r)                                          ' �G���[���^�[��
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW ?
                    GoTo STP_END                                        ' �I���m�F���b�Z�[�W�\����
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW ?
                    ' ��د�ޑ���(��I��)
                    Call FlxGridSet(CNO, 1, cpCirAxisInfoArray(CNO).dblCaP2, 0)
                    Call FlxGridSet(CNO, 2, cpCirAxisInfoArray(CNO).dblCaP3, 0)
                    GoTo STP_HALT                                       ' �P�O�̻���Ăɖ߂�

                ElseIf (r = cFRS_ERR_START) Then                        ' START SW ?
                    ' ����Ċ���W�X�V
                    With cpCirAxisInfoArray(CNO)
                        .dblCaP2 = stJOG.PosX                           ' ����Ċ���WX
                        .dblCaP3 = stJOG.PosY                           ' ����Ċ���WY
                    End With
                    CNO = CNO + 1                                       ' ����Đ������X�V
                End If

            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK(START)/RESET(Cancel)���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                GoTo STP_END                                            '�I���m�F���b�Z�[�W�\����
            End If

            '-------------------------------------------------------------------
            '   START�L�[/RESET/HALT�L�[�����҂�
            '-------------------------------------------------------------------
            ' "[START]�L�[�������ĉ����� [HALT]�L�[�łP�O�̃f�[�^�ɖ߂�܂�"
            lblCirInfo.Text = INFO_MSG10
            stJOG.Md = MODE_KEY                                         ' ���[�h=�L�[���͑҂����[�h
            stJOG.Flg = -1
            Do
                ' START�L�[/RESET/HALT�L�[�����҂�
                'V6.0.0.0�J                 r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r = cFRS_ERR_START) Then                            ' OK(START�L�[) ?
                    Exit Do
                ElseIf (r = cFRS_ERR_RST) Then                          ' Cancel(RESET��) ?
                    Exit Do
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT�� ?
                    ' ��د�ޑ���(��I��)
                    Call FlxGridSet(CirNum, 1, cpCirAxisInfoArray(CirNum).dblCaP2, 0)
                    Call FlxGridSet(CirNum, 2, cpCirAxisInfoArray(CirNum).dblCaP3, 0)
                    Exit Do
                Else                                                    ' ���̑��̃G���[ 
                    Return (r)
                End If
            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK(START)/RESET(Cancel)���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                GoTo STP_END                                            '�I���m�F���b�Z�[�W�\����
            End If

            '-------------------------------------------------------------------
            '   HALT���������͂P�O�̻���Ăɖ߂�
            '-------------------------------------------------------------------
            If (r = cFRS_ERR_HALT) Then                                 ' HALT�� ?
STP_HALT:
                CNO = CNO - 1                                           ' ����Đ�����
                If CNO <= 0 Then CNO = 1
                ' ��د�ޑ���(�O��)
                Call FlxGridSet(CNO, 1, cpCirAxisInfoArray(CNO).dblCaP2, 1)
                Call FlxGridSet(CNO, 2, cpCirAxisInfoArray(CNO).dblCaP3, 1)
                GoTo STP_RETRY
            End If

STP_END:
            '-------------------------------------------------------------------
            '   �I���m�F���g���~���O�f�[�^�X�V
            '-------------------------------------------------------------------
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_FUNC30)
            If (rtn = cFRS_ERR_RST) Then
                '�u�������v�I���Ȃ�P�O�̻���Ăɖ߂�
                ' ��د�ޑ���(�O��)
                'If (CNO >= FGridCir.Rows - 1) Then CNO = FGridCir.Rows - 1
                If (dgvCir.CurrentRow.Index < CNO) Then CNO = dgvCir.CurrentRow.Index ' V4.0.0.0�G
                Call FlxGridSet(CNO, 1, cpCirAxisInfoArray(CNO).dblCaP2, 0)
                Call FlxGridSet(CNO, 2, cpCirAxisInfoArray(CNO).dblCaP3, 0)
                CNO = CNO + 1
                GoTo STP_HALT
            Else
                ' OK����(START��)�������Łu�͂��v�I���łȂ�f�[�^�X�V���ďI��
                If (r = cFRS_ERR_START) Then                            ' OK����(START��)������ ?
                    ' ����č��W�X�V
                    For i = 1 To MaxCntCircuit
                        typCirAxisInfoArray(i) = cpCirAxisInfoArray(i)
                    Next i
                    gCmpTrimDataFlg = 1                                 ' �f�[�^�X�V�t���O = 1(�X�V����)
                End If
            End If

            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmCircuitTeach.CirMain() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�ڷ���ٸ�د�ޏ�����"
    '''=========================================================================
    '''<summary>�ڷ���ٸ�د�ޏ�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CirGridInitialize()

        'Dim TOP_TITLE As String                             ' ����
        'Dim TOP_TITLE_() As String                          ' ����    V4.0.0.0�G
        Dim i As Short                                      ' ����
        Dim CirNum As Short                                 ' ����Đ�
        Dim BNX As Short                                    ' ��ۯ���X
        Dim BNY As Short                                    ' ��ۯ���Y
        Dim Cdir As Short                                   ' ���ߕ��ѕ���
        Dim num As Short
        Dim strMSG As String

        Try
            ' ����
            'TOP_TITLE = ""
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    TOP_TITLE = " ����Ĕԍ� " & "|" & "����W X" & "|" & "����W Y"
            '    TOP_TITLE_ = New String() {" ����Ĕԍ� ", "����W X", "����W Y"}
            'ElseIf gSysPrm.stTMN.giMsgTyp = 1 Then
            '    TOP_TITLE = "CIRCUIT No." & "|" & "POSITION X" & "|" & "POSITION Y"
            '    TOP_TITLE_ = New String() {"CIRCUIT No.", "POSITION X", "POSITION Y"}
            'Else
            '    TOP_TITLE_ = New String() {String.Empty, String.Empty, String.Empty}
            'End If

            ' �ް��擾
            CirNum = typPlateInfo.intCircuitCntInBlock          ' 1��ۯ��໰��Đ�
            BNX = typPlateInfo.intGroupCntInBlockXBp            ' �O���[�v��(��ۯ���)X,Y
            BNY = typPlateInfo.intGroupCntInBlockYStage
            Cdir = typPlateInfo.intResistDir                    ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
            If Cdir = 0 Then
                num = CirNum * BNX
            ElseIf Cdir = 0 Then
                num = CirNum * BNY
            End If

            'With FGridCir
            '    .FormatString = TOP_TITLE                       ' ����
            '    .Cols = 3                                       ' �ő��
            '    .FixedCols = 1                                  ' �Œ��
            '    .Rows = CirNum + 1                              ' �ő�s��
            '    .FixedRows = 1                                  ' �Œ�s��

            '    ' ������z�u
            '    For i = 1 To CirNum
            '        .Row = i
            '        .Col = 1
            '        .CellAlignment = MSFlexGridLib.AlignmentSettings.flexAlignRightCenter
            '    Next

            'End With

            '' �����ް���ݒ�
            'For i = 1 To CirNum
            '    With typCirAxisInfoArray(i)
            '        'FGridCir.set_TextMatrix(i, 0, .intCaP1.ToString("      0")) ' �ԍ�
            '        FGridCir.set_TextMatrix(i, 0, i.ToString("      0"))        ' �ԍ�
            '        FGridCir.set_TextMatrix(i, 1, .dblCaP2.ToString("0.0000"))  ' ���WX
            '        FGridCir.set_TextMatrix(i, 2, .dblCaP3.ToString("0.0000"))  ' ���WY
            '    End With
            'Next i

            With dgvCir                         ' V4.0.0.0�G
                '.Columns(0).HeaderText = TOP_TITLE_(0)
                '.Columns(1).HeaderText = TOP_TITLE_(1)
                '.Columns(2).HeaderText = TOP_TITLE_(2)
                .Rows.Add(CirNum)
            End With

            ' �����ް���ݒ�
            For i = 0 To (CirNum - 1) Step 1    ' V4.0.0.0�G
                With typCirAxisInfoArray(i + 1)
                    dgvCir(0, i).Value = (i + 1).ToString()          ' �ԍ�
                    dgvCir(1, i).Value = .dblCaP2.ToString("0.0000") ' ���WX
                    dgvCir(2, i).Value = .dblCaP3.ToString("0.0000") ' ���WY
                End With
            Next i

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmCircuitTeach.CirGridInitialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ڷ���ٸ�د�ނ֕\��"
    '''=========================================================================
    '''<summary>�ڷ���ٸ�د�ނ֕\��</summary>
    '''<param name="r">      (INP)�s</param>
    '''<param name="c">      (INP)��</param>
    '''<param name="dblDATA">(INP)�o�̓f�[�^</param> 
    '''<param name="col">    (INP)</param>  
    '''=========================================================================
    Public Sub FlxGridSet(ByVal r As Integer, ByVal c As Integer, ByVal dblDATA As Double, ByVal col As Integer)

        'FGridCir.set_TextMatrix(r, c, dblDATA.ToString("0.0000"))
        dgvCir(c, r - 1).Value = dblDATA.ToString("0.0000")             ' V4.0.0.0�G
        Call FlxGridColSet(r, c, col)

    End Sub
#End Region

#Region "�ڷ���ٸ�د���ޯ��װ�ύX"
    '''=========================================================================
    '''<summary>�ڷ���ٸ�د���ޯ��װ�ύX</summary>
    '''<param name="rr">  (INP)�s</param>
    '''<param name="CC">  (INP)��</param>
    '''<param name="MODE">(INP) �Ώۂ̃J�b�g�ԍ�</param> 
    '''=========================================================================
    Public Sub FlxGridColSet(ByVal rr As Integer, ByVal CC As Integer, ByVal MODE As Integer)

        'With FGridCir
        '    .Col = CC                                   ' ��
        '    .Row = rr                                   ' �s
        '    If MODE = 0 Then                            ' Normal ?
        '        .CellBackColor = System.Drawing.Color.White
        '    Else
        '        '.CellBackColor = System.Drawing.Color.LightCyan
        '        .CellBackColor = System.Drawing.Color.Yellow
        '        If rr <= 5 Then
        '            .TopRow = 1
        '        Else
        '            .TopRow = rr - 5
        '        End If
        '    End If
        'End With

        With dgvCir(CC, rr - 1)         ' V4.0.0.0�G
            If (0 = MODE) Then
                .Style.BackColor = SystemColors.Window
            Else
                .Style.BackColor = Color.Yellow
            End If
        End With

        If (rr < 5) Then                ' V4.0.0.0�G
            dgvCir.FirstDisplayedScrollingRowIndex = 0
        Else
            dgvCir.FirstDisplayedScrollingRowIndex = (rr - 4)
        End If

    End Sub
#End Region

#Region "DataGridView���I����Ԃ̐F�ɂȂ�̂��L�����Z������"
    '''=========================================================================
    ''' <summary>DataGridView���I����Ԃ̐F�ɂȂ�̂��L�����Z������</summary>
    '''=========================================================================
    Private Sub dgvCir_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvCir.SelectionChanged
        dgvCir.Rows(dgvCir.CurrentRow.Index).Selected = False ' V4.0.0.0�G
    End Sub
#End Region

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
    '   �e���L�[���͏���
    '========================================================================================
#Region "�L�[�_�E��������"
    '''=========================================================================
    '''<summary>�L�[�_�E��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmTy2Teach_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                     'V6.0.0.0�J
        Console.WriteLine("frmCircuitTeach.frmTy2Teach_KeyDown()")

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
    Private Sub frmTy2Teach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0�J
        Console.WriteLine("frmCircuitTeach.frmTy2Teach_KeyKeyUp()")
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

End Class