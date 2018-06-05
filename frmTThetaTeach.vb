'===============================================================================
'   Description  : �s�ƃe�B�[�`���O��ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmTThetaTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

#Region "�v���C�x�[�g�ϐ���`"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    Private stJOG As JOG_PARAM                              ' �����(BP��JOG����)�p�p�����[�^
    Private mExit_flg As Short                              ' �I���t���O
    'Private objArrow As FrmArrow                        ' ����ʵ�޼ު�� 
    'Private stBPMV As FrmArrow.JOG_PARAM                ' �����(BP��JOG����)�p�p�����[�^

    '----- �g���~���O�p�����[�^ -----
    Private mdBpOffx As Double                          ' BP�ʒu�̾��X
    Private mdBpOffy As Double                          ' BP�ʒu�̾��Y
    Private mdAdjx As Double                            ' ��ެ�Ĉʒu�̾��X
    Private mdAdjy As Double                            ' ��ެ�Ĉʒu�̾��Y
    Private miBNx As Short                              ' ��ۯ���X
    Private miBNy As Short                              ' ��ۯ���Y
    Private miCdir As Short                             ' ���ߕ��ѕ���
    Private miChpNum As Short                           ' ���ߐ�
    Private miGNx As Short                              ' ��ٰ�ߐ�X
    Private miGNy As Short                              ' ��ٰ�ߐ�Y
    Private mdBSx As Double                             ' ��ۯ�����X
    Private mdBSy As Double                             ' ��ۯ�����Y
    Private mdTThetaOff As Double                       ' T�Ƶ̾��
    Private mdTThetaPos1X As Double                     ' T�Ɗ�ʒu1X
    Private mdTThetaPos1Y As Double                     ' T�Ɗ�ʒu1Y
    Private mdTThetaPos2X As Double                     ' T�Ɗ�ʒu2X
    Private mdTThetaPos2Y As Double                     ' T�Ɗ�ʒu2Y

#End Region

#Region "�I�����ʂ�Ԃ�"
    ' '''=========================================================================
    ' '''<summary>�I�����ʂ�Ԃ�</summary>
    ' '''<returns>cFRS_ERR_START=OK(START��)
    ' '''         cFRS_ERR_RST  =Cancel(RESET��)
    ' '''         -1�ȉ�        =�G���[</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn  'V6.0.0.0�J
    '    'V6.0.0.0�J    Public ReadOnly Property sGetReturn() As Short
    '    Get
    '        sGetReturn = mExit_flg
    '    End Get
    'End Property
#End Region

#Region "Form Initialize������"
    '''=========================================================================
    '''<summary>Form Initialize������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        'objArrow = New FrmArrow()                           ' ����ʵ�޼ު�Đ���

    End Sub
#End Region

#Region "Form �I��������"
    '''=========================================================================
    '''<summary>Form �I��������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTThetaTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim strMSG As String

        Try
            '' �I�u�W�F�N�g�J��
            'Call objArrow.Close()                               ' ����ʃI�u�W�F�N�g�J��
            'Call objArrow.Dispose()                             ' ����ʃ��\�[�X�J��
            'objArrow = Nothing

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTThetaTeach.frmTThetaTeach_FormClosed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Load������"
    '''=========================================================================
    '''<summary>Form Load������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTThetaTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ��������
            Call SetDispMsg()                                   ' ���x������ݒ肷��(���{��/�p��)
            mExit_flg = -1                                      ' �I���t���O = ������

            ' ����޳����۰قɏd�˂�
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' �g���~���O�f�[�^���K�v�ȃp�����[�^���擾����
            Call SetTrimData()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTThetaTeach.frmTThetaTeach_Load() TRAP ERROR = " + ex.Message
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

        With Me
            ' label
            '.lblTitle2.Text = TITLE_T_Theta                 ' ����="T�ƃe�B�[�`���O"
            '.LblDisp_9.Text = LBL_TXTY_TEACH_15             ' �p�x�␳(deg)
            ' frame
            '.GrpFram_0.Text = LBL_TXTY_TEACH_12             ' ��P��_
            '.GrpFram_1.Text = LBL_TXTY_TEACH_13             ' ��Q��_
            '.GrpFram_2.Text = LBL_TXTY_TEACH_03             ' �␳��
            ' button
            '.cmdOK.Text = "OK"
            '.cmdCancel.Text = CMD_CANCEL
        End With

        Call InitDisp()

    End Sub
#End Region

#Region "�\���֘A������"
    '''=========================================================================
    '''<summary>�\���֘A������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitDisp()

        With Me
            .TxtPosX.Text = ""
            .TxtPosY.Text = ""
            .TxtPos2X.Text = ""
            .TxtPos2Y.Text = ""
            .LblResult_1.Text = ""
            .LblResult_2.Text = ""
        End With

    End Sub
#End Region

#Region "�K�v�����ݸ����Ұ����擾"
    '''=========================================================================
    '''<summary>�K�v�����ݸ����Ұ����擾</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim strMSG As String

        Try
            ' �K�v�����ݸ����Ұ����擾
            mdBpOffx = typPlateInfo.dblBpOffSetXDir             ' BP�ʒu�̾��X,Y�ݒ�
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            mdAdjx = typPlateInfo.dblAdjOffSetXDir              ' ��ެ���߲�Ĉʒu�̾��X,Y�ݒ�
            mdAdjy = typPlateInfo.dblAdjOffSetYDir
            miBNx = typPlateInfo.intBlockCntXDir                ' ��ۯ���X,Y
            miBNy = typPlateInfo.intBlockCntYDir
            miCdir = typPlateInfo.intResistDir                  ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
            'Call GetChipNum(miChpNum)                           ' ���ߐ�
            miChpNum = typPlateInfo.intResistCntInBlock         ' �u���b�N����R��
            miGNx = typPlateInfo.intGroupCntInBlockXBp          ' �O���[�v��X,Y
            miGNy = typPlateInfo.intGroupCntInBlockYStage
            Call CalcBlockSize(mdBSx, mdBSy)                    ' ��ۯ�����X,Y
            mdTThetaOff = typPlateInfo.dblTThetaOffset          ' T�Ƶ̾��
            mdTThetaPos1X = typPlateInfo.dblTThetaBase1XDir     ' T�Ɗ�ʒu1XY
            mdTThetaPos1Y = typPlateInfo.dblTThetaBase1YDir
            mdTThetaPos2X = typPlateInfo.dblTThetaBase2XDir     ' T�Ɗ�ʒu2XY
            mdTThetaPos2Y = typPlateInfo.dblTThetaBase2YDir

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTThetaTeach.SetTrimData() TRAP ERROR = " + ex.Message
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
    Private Sub frmTThetaTeach_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim strMSG As String

        Try
            ' T��è��ݸފJ�n
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                       ' �I���t���O = 0
            mExit_flg = T_ThetaMainProc()                       ' T��è��ݸފJ�n

            ' �␳�N���X���C�����\���Ƃ���
            'V6.0.0.0�C            Form1.CrosLineX.Visible = False
            'V6.0.0.0�C            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTThetaTeach.frmTThetaTeach_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                           ' Return�l = ��O�G���[
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
            ' T��è��ݸފJ�n
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                       ' �I���t���O = 0
            mExit_flg = T_ThetaMainProc()                       ' T��è��ݸފJ�n

            ' �␳�N���X���C�����\���Ƃ���
            'V6.0.0.0�C            Form1.CrosLineX.Visible = False
            'V6.0.0.0�C            Form1.CrosLineY.Visible = False
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "frmTThetaTeach.Execute() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                           ' Return�l = ��O�G���[
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

        'stBPMV.Flg = cFRS_ERR_START                           ' OK(START��)

    End Sub
#End Region

#Region "��ݾ����݉���������"
    '''=========================================================================
    '''<summary>��ݾ����݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        'stBPMV.Flg = cFRS_ERR_RST                           ' Cancel(RESET��)

    End Sub
#End Region

#Region "�s�ƃe�B�[�`���O����"
    '''=========================================================================
    '''<summary>�s�ƃe�B�[�`���O����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function T_ThetaMainProc() As Short

        Dim i As Short
        Dim POSX As Double                                  ' ��ʒuX
        Dim POSY As Double                                  ' ��ʒuX
        Dim workTThetaOff As Double                         ' T�Ƶ̾��
        Dim EffBN As Short                                  ' �L����ۯ���
        Dim EffGN As Short                                  ' �L����ٰ�ߐ�
        Dim iPos As Short                                   ' �\���ʒu
        Dim ZRPosX(1) As Double                             ' �����X(0:��1��_, 1:��2��_)
        Dim ZRPosY(1) As Double                             ' �����Y(0:��1��_, 1:��2��_)
        Dim CSPointX(1) As Double                           ' PosX�Z�o�p(0:��1��_�@1:��2��_)
        Dim CSPointY(1) As Double                           ' PosY�Z�o�p(0:��1��_�@1:��2��_)
        Dim tmpTThetaOff As Double                      ' ��T�ƃI�t�Z�b�g

        Dim TBLx As Double                                  ' ð��وړ����WX(��Βl)
        Dim TBLy As Double                                  ' ð��وړ����WY(��Βl)

        Dim rtn As Short                                    ' �ߒl 
        Dim r As Short
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' ���ߕ��������R�����擾
            If miCdir = 0 Then                              ' ���ߕ���X����
                ' ��R��X/��ٰ�ߐ�X���
                If miGNx = 1 Then
                    EffBN = miChpNum                        ' ��R�����
                Else
                    EffBN = typGrpInfoArray(1).intGP2       ' ��ٰ���ް��̒�R��(No.1)���
                End If
                EffGN = miGNx
            Else                                            ' ���ߕ���Y����
                ' ��R��Y/��ٰ�ߐ�Y���
                If miGNy = 1 Then
                    EffBN = miChpNum                        ' ��R�����
                Else
                    EffBN = typGrpInfoArray(1).intGP2       ' ��ٰ���ް��̒�R��(No.1)���
                End If
                EffGN = miGNy
            End If
            tmpTThetaOff = mdTThetaOff                      ' HALT�����p�ɽį�

            ' ��R�����1��̏ꍇ�͏����ł��Ȃ��ׁA�װ�Ƃ���
            If EffBN <= 1 Then
                ' ��R�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I
                MsgBox(ERR_TXNUM_E, MsgBoxStyle.Exclamation, "")
                Return (cFRS_ERR_RST)                       ' Return�l = Cancel(RESET��)
            End If

            ' ����ʏ�����
            For i = 0 To 1
                ZRPosX(i) = 0.0#
                ZRPosY(i) = 0.0#
            Next i

            ' BP���1��ۯ��A��1��R�����߲�ĂɈړ�����
            Call BpMoveOrigin_Ex()                          ' BP����ۯ��E��Ɉړ�(���_�ݒ�)

            ' �����(BP��JOG����)�p�p�����[�^������������
            'stBPMV.Md = MODE_BP                             ' ���[�h(1:BP�ړ�)
            'stBPMV.Md2 = 0                                  ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            'stBPMV.Opt = 0                                  ' �I�v�V����(0:HALT�L�[����, 1:HALT�L�[�L��)
            'stBPMV.BpOffX = mdAdjx + mdBpOffx               ' BP�̾��X 
            'stBPMV.BpOffY = mdAdjy + mdBpOffy               ' BP�̾��Y 
            'stBPMV.BszX = mdBSx                             ' ��ۯ�����X 
            'stBPMV.BszY = mdBSy                             ' ��ۯ�����Y
            'stBPMV.TextX = TxtPosX                          ' BP X�ʒu�\���p÷���ޯ��
            'stBPMV.TextY = TxtPosY                          ' BP Y�ʒu�\���p÷���ޯ��
            'objArrow.Show(Me)                               ' ����ʕ\�� 

STP_RETRY:
            '-------------------------------------------------------------------
            '   �s�ƃe�B�[�`���O����
            '-------------------------------------------------------------------
            ' ��1��_/��2��_��è��ݸ�
            Call InitDisp()                                 ' ���W�\��(�ر)
            iPos = 0                                        ' iPos = ��1��_
            stJOG.Flg = -1                                  ' �e��ʂ�OK/Cancel���݉����׸�

            Do
                ' �\�����b�Z�[�W����ݒ肷�� 
                If (iPos = 0) Then                          ' ��1��_ ? 
                    '"��1�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
                    lblInfo.Text = INFO_MSG18 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    'stBPMV.TextX = TxtPosX                  ' ��1��_���WX�ʒu�\���p÷���ޯ��
                    'stBPMV.TextY = TxtPosY                  ' ��1��_���WY�ʒu�\���p÷���ޯ��
                    TxtPosX.Enabled = True
                    TxtPosY.Enabled = True
                    TBLx = mdTThetaPos1X                    ' ��1��_���WX
                    TBLy = mdTThetaPos1Y                    ' ��1��_���WY
                Else
                    '"��n�O���[�v�A�ŏI��R��ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" & vbCrLf & "[HALT]�łP�O�̏����ɖ߂�܂��B"
                    lblInfo.Text = INFO_MSG19 & iPos & INFO_MSG20 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    'stBPMV.TextX = TxtPos2X                 ' ��2��_���WX�ʒu�\���p÷���ޯ��
                    'stBPMV.TextY = TxtPos2Y                 ' ��2��_���WY�ʒu�\���p÷���ޯ��
                    TxtPos2X.Enabled = True
                    TxtPos2Y.Enabled = True
                    TBLx = mdTThetaPos2X                    ' ��2��_���WX
                    TBLy = mdTThetaPos2Y                    ' ��2��_���WY
                End If

                ' BP��Βl�ړ�(��1��ʒu/��2��ʒu)
                r = Form1.System1.EX_MOVE(gSysPrm, TBLx, TBLy, 1)
                If (r <> cFRS_NORMAL) Then
                    Return (r)                              ' �G���[���^�[��
                End If

                ' Cancel���݂�̫�����ݒ肷��
                Me.cmdCancel.Focus()

                ' ��1��_/��2��_��è��ݸޏ���
                'stBPMV.PosX = TBLx + ZRPosX(iPos)           ' BP X��Έʒu
                'stBPMV.PosY = TBLy + ZRPosY(iPos)           ' BP Y��Έʒu
                'stBPMV.Flg = 0                              ' �e��ʂ�OK/Cancel���݉����׸�
                'r = objArrow.JogEzMove(stBPMV)              ' è��ݸޏ���

                ' �ð�� ����(�ݿ�ٷ�)
                If (r = cFRS_ERR_START) Then                  ' START SW ?
                    ' ��1��_/��2��_�̐�Βl���W�X�V
                    'CSPointX(iPos) = stBPMV.PosX
                    'CSPointY(iPos) = stBPMV.PosY
                    ' ��1��_/��2��_�̐�Βl���W�X�V
                    'ZRPosX(iPos) = ZRPosX(iPos) + stBPMV.cgX
                    'ZRPosY(iPos) = ZRPosY(iPos) + stBPMV.cgY

                    If (iPos >= 1) Then                     ' ��1��_/��2��_��è��ݸޏI�� ?
                        Exit Do
                    Else
                        iPos = 1                            ' iPos = ��2��_��è��ݸޏ���
                    End If

                ElseIf (r = cFRS_ERR_RST) Then              ' RESET SW ?
                    GoTo STP_END                            ' �I���m�F���b�Z�[�W�\����
                ElseIf (r < cFRS_NORMAL) Then
                    Return (r)                              ' �G���[���^�[��
                End If
                Call ZCONRST()                              ' �ݿ�ٷ�ׯ����� 
            Loop While (stJOG.Flg = -1)

            ' Cancel���݉����Ȃ�I���m�F���b�Z�[�W�\����
            'If (stBPMV.Flg = cFRS_ERR_RST) Then
            '    r = cFRS_ERR_RST
            '    GoTo STP_END
            'End If

            ' PosXY�̋������Z�o���AT�Ɗp�x���v�Z
            POSX = System.Math.Abs(CDbl(CSPointX(1) - CSPointX(0)))
            POSY = (CDbl(CSPointY(1) - CSPointY(0)))
            ' �Ƃ̉�]�����͋t�ɂȂ邽�ߔ��]������
            workTThetaOff = -(System.Math.Atan(POSY / POSX) * (180 / 3.141592))
            mdTThetaOff = mdTThetaOff + workTThetaOff

            '-------------------------------------------------------------------
            '   ���ʂ�\������
            '-------------------------------------------------------------------
            With Me
                ' �␳�p�x(�␳�O/�␳��)
                LblResult_1.Text = tmpTThetaOff.ToString("0.00000")
                LblResult_2.Text = mdTThetaOff.ToString("0.00000")
            End With
            r = cFRS_ERR_START                                ' OK(START��) 

STP_END:
            '-------------------------------------------------------------------
            '   �I���m�F�����ݸ��ް��X�V
            '-------------------------------------------------------------------
            ' �I���m�F���b�Z�[�W�\�� 
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TX)
            If (rtn = cFRS_ERR_START) Then
                '�u�͂��v�I���Ȃ��ް��X�V���ďI��
                If (r = cFRS_ERR_START) Then                  ' OK�{�^�������� ?
                    ' T�Ƶ̾�čX�V
                    typPlateInfo.dblTThetaOffset = mdTThetaOff
                    gCmpTrimDataFlg = 1                     ' �f�[�^�X�V�t���O = 1(�X�V����)
                End If
            Else
                '�u�������v�I���Ȃ�擪�̏����ɖ߂�
                ' è��ݸނ����ް���ر
                mdTThetaOff = tmpTThetaOff
                For i = 0 To 1
                    ZRPosX(i) = 0.0#
                    ZRPosY(i) = 0.0#
                Next i
                GoTo STP_RETRY
            End If

            Return (rtn)                                    ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTThetaTeach.T_ThetaMainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "�C���^�[�t�F�[�X����(���̃t�H�[���ł�NOP)"
    ''' <summary></summary>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0�J</remarks>
    Public Sub JogKeyDown(e As KeyEventArgs) Implements ICommonMethods.JogKeyDown
        ' DO NOTHING
    End Sub

    ''' <summary></summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub JogKeyUp(e As KeyEventArgs) Implements ICommonMethods.JogKeyUp
        ' DO NOTHING
    End Sub

    ''' <summary></summary>
    ''' <param name="distanceX"></param>
    ''' <param name="distanceY"></param>
    ''' <remarks>'V6.0.0.0�J</remarks>
    Public Sub MoveToCenter(distanceX As Decimal, distanceY As Decimal) _
        Implements ICommonMethods.MoveToCenter
        ' DO NOTHING
    End Sub
#End Region

End Class