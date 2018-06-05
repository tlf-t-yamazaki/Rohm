'===============================================================================
'   Description  : TY2�e�B�[�`���O��ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmTy2Teach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

#Region "�v���C�x�[�g�萔/�ϐ���`"
    '===========================================================================
    '   �萔/�ϐ���`
    '===========================================================================
    '----- �I�u�W�F�N�g��` -----
    Private stJOG As JOG_PARAM                              ' �����(BP��JOG����)�p�p�����[�^
    Private mExit_flg As Short                              ' �I���t���O

    Private EffBN As Short                                  ' �L����ۯ���
    Private EffGN As Short                                  ' �L����ٰ�ߐ�
    Private TBLx As Double                                  ' ð��وړ����WX(��Βl)
    Private TBLy As Double                                  ' ð��وړ����WY(��Βl)
    Private piPositionNum As Short                          ' �ï�ߕ�ð��ق̴��ؐ�-1
    Private pfStartPos(MaxCntResist) As Double              ' �ï�ߕ�ð���(1�ؼ��)
    Private pfSaveStartPos(1, MaxCntResist) As Double       ' ���ĈʒuX(0),Y(1)ð���(1�ؼ��)
    Private pfIntvalAry(MaxCntResist) As Double             ' �����e�[�u��(1�ؼ��) ####119

    '----- �g���~���O�f�[�^ -----
    Private mdTbOffx As Double                              ' ð��وʒu�̾��X
    Private mdTbOffy As Double                              ' ð��وʒu�̾��Y
    Private mdBpOffx As Double                              ' BP�ʒu�̾��X
    Private mdBpOffy As Double                              ' BP�ʒu�̾��Y
    Private mdAdjx As Double                                ' ��ެ�Ĉʒu�̾��X
    Private mdAdjy As Double                                ' ��ެ�Ĉʒu�̾��Y
    Private miBNx As Short                                  ' ��ۯ���X
    Private miBNy As Short                                  ' ��ۯ���Y
    Private miCdir As Short                                 ' ���ߕ��ѕ���
    Private miGNx As Short                                  ' ��ٰ�ߐ�X
    Private miGNy As Short                                  ' ��ٰ�ߐ�Y
    Private mdBSx As Double                                 ' ��ۯ�����X
    Private mdBSy As Double                                 ' ��ۯ�����Y
    Private mdSpx As Double                                 ' �����߲��X
    Private mdSpy As Double                                 ' �����߲��Y
    Private mdStageX As Double                              ' �ð�ވʒuX
    Private mdStageY As Double                              ' �ð�ވʒuY

    '----- ���̑� -----
    Private dblTchMoval(3) As Double                            ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))
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
    Private Sub Ty2Teach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed


    End Sub
#End Region

#Region "Form Load������"
    '''=========================================================================
    '''<summary>Form Load������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Ty2Teach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        ' ��������
        Call TySetMessages()                                ' ���x������ݒ肷��(���{��/�p��)
        mExit_flg = -1                                      ' �I���t���O = ������

        ' ����޳����۰قɏd�˂�
        Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
        Me.Left = Form1.Text4.Left

        ' �g���~���O�f�[�^���K�v�ȃp�����[�^���擾����
        Call SetTrimData()

        ' �u���b�NXY�̃X�^�[�g�|�W�V������gBlkStagePosX()/gBlkStagePosY()�ɐݒ肷�� ###119
        Call CalcBlockXYStartPos()

    End Sub
#End Region

#Region "���x������ݒ肷��(���{��/�p��)"
    '''=========================================================================
    '''<summary>���x������ݒ肷��(���{��/�p��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TySetMessages()

        'Me.lblTyTitle.Text = TITLE_TY2                                 ' ����="�X�e�b�v�T�C�Y(TY2)�e�B�[�`���O"
        'Me.GrpTyTeach_1.Text = FRAME_TY2_01                             ' "�e�B�[�`���O�|�C���g" 
        'Me.GrpTyTeach_2.Text = LBL_TXTY_TEACH_03                       ' "�␳��" '###086
        'Me.GrpTyTeach_2.Text = TITLE_TY2                                ' ����="�X�e�b�v�T�C�Y(TY2)�e�B�[�`���O" '###086
        Me.LblInterval.Text = LBL_TXTY_TEACH_11                         ' "�X�e�b�v�C���^�[�o��"
        'Me.cmdCancel.Text = CMD_CANCEL                                  ' "�L�����Z�� (&Q)"
        Me.lblTyInfo.Text = ""

    End Sub
#End Region

#Region "Form Activated������"
#If False Then                          'V6.0.0.0�L Execute()�ł����Ȃ�
    '''=========================================================================
    '''<summary>Form Activated������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Ty2Teach_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' TY2è��ݸފJ�n
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                   ' �I���t���O = 0
            mExit_flg = Ty2Main()                           ' TY2è��ݸފJ�n

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTy2Teach.Ty2Teach_Activated() TRAP ERROR = " + ex.Message
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
            ' TY2è��ݸފJ�n
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                   ' �I���t���O = 0
            mExit_flg = Ty2Main()                           ' TY2è��ݸފJ�n

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim strMSG As String = "frmTy2Teach.Execute() TRAP ERROR = " & ex.Message
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

#Region "TY2�e�B�[�`���O����"
    '''=========================================================================
    '''<summary>TY2�e�B�[�`���O����</summary>
    '''<returns>cFRS_ERR_START=OK(START��)
    '''         cFRS_ERR_RST=Cancel(RESET��)
    '''         -1�ȉ�      =�G���[</returns>
    '''=========================================================================
    Private Function Ty2Main() As Short

        Dim BlockCnt As Short                                           ' ��ۯ�������
        Dim i As Short
        Dim r As Short
        Dim rtn As Short                                                ' TyMainProc�ߒl 
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �ï�ߕ�ð��ٍ쐬
            rtn = 0                                                     ' TyMain�ߒl 
            Call TyTeachingInitialize()

            ' �ï�߸�د�ނ̏�����
            Call GridInitialize()

            ' ���ߕ���������ۯ������擾
            If miCdir = 0 Then
                ' ���ߕ���X����:��ۯ���Y/��ٰ�ߐ�Y���
                EffBN = miBNy                                           ' ��ۯ���Y
                EffGN = miGNy                                           ' ��ٰ�ߐ�Y
            Else
                ' ���ߕ���Y����:��ۯ���X/��ٰ�ߐ�X���
                EffBN = miBNx                                           ' ��ۯ���X
                EffGN = miGNx                                           ' ��ٰ�ߐ�X
            End If

            ' �擪��ۯ��̐擪��ʒu���擾����
            Call BpMoveOrigin_Ex()                                      ' BP����ۯ��̉E��Ɉړ�����
            r = Form1.System1.EX_MOVE(gSysPrm, mdSpx, mdSpy, 1)
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return�l = �G���[
            End If

            ' �����(XYð��ق�JOG����)�p�p�����[�^������������
            stJOG.Md = MODE_STG                                         ' ���[�h(0:XY�e�[�u���ړ�)
            stJOG.Md2 = MD2_BUTN                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
            stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START             ' �L�[�̗L��(1)/����(0)�w��
            stJOG.PosX = 0.0                                            ' BP X�ʒu/XYð��� X�ʒu
            stJOG.PosY = 0.0                                            ' BP Y�ʒu/XYð��� Y�ʒu
            stJOG.BpOffX = mdAdjx + mdBpOffx                            ' BP�̾��X 
            stJOG.BpOffY = mdAdjy + mdBpOffy                            ' BP�̾��Y 
            stJOG.BszX = mdBSx                                          ' ��ۯ�����X 
            stJOG.BszY = mdBSy                                          ' ��ۯ�����Y
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
            Me.KeyPreview = True
            Call Me.Focus()

STP_RETRY:
            '-------------------------------------------------------------------
            '   �x�����̊e�u���b�N�̃X�e�b�v���̃e�B�[�`���O�������s��
            '-------------------------------------------------------------------
            BlockCnt = 1                                                ' ��ۯ�������
            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸ޏ�����
            stJOG.Md = MODE_STG                                         ' ���[�h(0:XY�e�[�u���ړ�)

            Do
                ' ��ٰ�ߐ����̏��������H
                If (BlockCnt > EffBN) Then
                    Exit Do
                End If

                ' ���W�\���N���A
                Me.TxtPosX.Text = ""
                Me.TxtPosY.Text = ""

                ' XYð��وړ�(��[BlockCnt]��ۯ�)
                r = XYTableMoveSetBlock(BlockCnt)                       ' ð��وʒu �� TBLx, TBLy
                If (r < cFRS_NORMAL) Then
                    Return (r)                                          ' �G���[���^�[��
                End If

                If (BlockCnt = EffBN) Then
                    '"�ŏI"+"�u���b�N�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ��F[���]  ����F[START]  ���f�F[RESET]" 
                    Me.lblTyInfo.Text = INFO_MSG26 & INFO_MSG25 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                Else
                    '"��"BlockCnt"�u���b�N�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ��F[���]  ����F[START]  ���f�F[RESET]" 
                    Me.lblTyInfo.Text = INFO_MSG24 & BlockCnt & INFO_MSG25 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                End If

                ' �e�B�[�`���O����
                stJOG.PosX = TBLx                                       ' XYð��� X�ʒu
                stJOG.PosY = TBLy                                       ' XYð��� Y�ʒu
                TxtPosX.Enabled = True
                TxtPosY.Enabled = True
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�I��
                    Return (r)
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW ?
                    GoTo STP_END                                        ' �I���m�F���b�Z�[�W�\����
                ElseIf (r = cFRS_ERR_START) Then                        ' START SW ?
                    ' ��[BlockCnt]��ۯ���XYð��ِ�Βl���W�X�V ###119
                    If (BlockCnt > 1) Then
                        Call SetStartPosTbl(BlockCnt)                   ' �����e�[�u���X�V
                        ' �O���b�h��1�O�̒l��ݒ肷��
                        Me.FGridTy2Teach.set_TextMatrix(BlockCnt - 1, 1, (pfStartPos(BlockCnt - 1) + pfIntvalAry(BlockCnt - 1)).ToString("0.0000 "))
                        dgvTy2Teach(1, BlockCnt - 2).Value = (pfStartPos(BlockCnt - 1) + pfIntvalAry(BlockCnt - 1)).ToString("0.0000 ")     ' V4.0.0.0�G
                        'If (BlockCnt = EffBN) Then
                        '    ' �ŏI�u���b�N�̏ꍇ
                        '    Call SetStartPosTbl(BlockCnt)               ' �X�e�b�v���e�[�u���X�V
                        '    ' �O���b�h�ɒl��ݒ肷��B
                        '    Me.FGridTy2Teach.set_TextMatrix(BlockCnt - 1, 1, pfStartPos(BlockCnt - 1).ToString("0.0000 "))
                        'Else
                        '    ' �ŏI�u���b�N�łȂ��ꍇ
                        '    Call SetStartPosTbl(BlockCnt)               ' �����e�[�u���X�V
                        '    ' �O���b�h�ɒl��ݒ肷��
                        '    Me.FGridTy2Teach.set_TextMatrix(BlockCnt, 1, (pfStartPos(BlockCnt) + pfIntvalAry(BlockCnt)).ToString("0.0000 "))
                        'End If
                    End If

                    ' �O���b�h�̐擪�s�ԍ���ݒ肷��
                    If BlockCnt <= 5 Then
                        Call SetTopRow(1)
                    Else
                        Call SetTopRow(BlockCnt - 5)
                    End If

                    ' ��ۯ��������X�V
                    BlockCnt = BlockCnt + 1
                End If

            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK(START)/RESET(Cancel)���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
                If (r = cFRS_ERR_RST) Then                              ' RESET(Cancel)���݉����Ȃ�I���m�F���b�Z�[�W�\����
                    GoTo STP_END
                End If
            End If

            '-------------------------------------------------------------------
            '   START�L�[/RESET�L�[�����҂�
            '-------------------------------------------------------------------
            lblTyInfo.Text = INFO_REC                                   ' "�o�^:[START] �L�����Z��:[RESET]"
            stJOG.Md = MODE_KEY                                         ' ���[�h=�L�[���͑҂����[�h
            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸ޏ�����
            Do
                ' START��/RESET�� �����҂�
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r = cFRS_ERR_START) Then                            ' OK(START�L�[) ?
                    r = cFRS_ERR_START
                    Exit Do
                ElseIf (r = cFRS_ERR_RST) Then                          ' Cancel(RESET��) ?
                    r = cFRS_ERR_RST
                    Exit Do
                Else                                                    ' ���̑��̃G���[ 
                    Return (r)
                End If
            Loop
STP_END:
            '-------------------------------------------------------------------
            '   �I���m�F���g���~���O�f�[�^�X�V
            '-------------------------------------------------------------------
            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@
            rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, MSG_OPLOG_TY2_START)
            If (rtn = cFRS_ERR_START) Then
                '�u�͂��v�I���Ȃ��ް��X�V���ďI��
                If (r = cFRS_ERR_START) Then                            ' OK�{�^�������� ?
                    Call sUpdateStartPosition()                         ' �f�[�^�X�V
                    gCmpTrimDataFlg = 1                                 ' �f�[�^�X�V�t���O = 1(�X�V����)
                End If
            Else
                '�u�������v�I���Ȃ�擪�̏����ɖ߂�
                ' �␳�l�N���A
                For i = 1 To Me.FGridTy2Teach.Rows - 1
                    Me.FGridTy2Teach.set_TextMatrix(i, 1, "")
                Next i
                For i = 0 To (dgvTy2Teach.Rows.Count - 1) Step 1        ' V4.0.0.0�G
                    dgvTy2Teach(1, i).Value = String.Empty
                Next i
                ' �e�B�[�`���O�����f�[�^���N���A
                Call sDefSetData()                                      ' �X�e�b�v�����̃O���b�h�l���g���~���O�f�[�^���\������
                Call SetTopRow(1)
                GoTo STP_RETRY
            End If

            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTy2Teach.Ty2Main() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�w��u���b�N�̃X�e�b�v���e�[�u�����X�V����"
    '''=========================================================================
    ''' <summary>�w��u���b�N�̃X�e�b�v���e�[�u�����X�V����</summary>
    ''' <param name="bn">   (INP)�u���b�N�ԍ�(1 ORG)</param>
    '''=========================================================================
    Private Sub SetStartPosTbl(ByVal bn As Short)

        Dim strMSG As String
        Dim Idx As Short
        Dim wkDBL As Double = 0.0

        Try
            '------ ###119�� -----
            ' �u���b�N�ԍ����P�ȏ�̎��ɐݒ肷��
            If (bn <= 1) Then Exit Sub

            ' �e�B�[�`���O���W�̍��������߂�
            If (miCdir = 0) Then                                        ' �`�b�v����X����
                wkDBL = CDbl(Me.TxtPosY.Text) - pfSaveStartPos(1, bn - 1)
                pfIntvalAry(bn - 1) = wkDBL - pfStartPos(bn - 1)

            Else
                'pfIntvalAry(bn - 1) = pfSaveStartPos(0, bn) - CDbl(Me.TxtPosX.Text)
                wkDBL = CDbl(Me.TxtPosX.Text) - pfSaveStartPos(0, bn - 1)
                pfIntvalAry(bn - 1) = wkDBL - pfStartPos(bn - 1)
            End If

            ' ������4���ɂ���(�S�~������ꍇ�������)
            strMSG = pfIntvalAry(bn - 1).ToString("0.0000")
            pfIntvalAry(bn - 1) = Double.Parse(strMSG)

            ' �e�B�[�`���O���W�z����X�V����
            For Idx = bn To MaxTy2                                  ' �u���b�N�����J��Ԃ� 
                If (miCdir = 0) Then                                    ' �`�b�v���ѕ��� = X�����̏ꍇ
                    pfSaveStartPos(1, Idx) = pfSaveStartPos(1, Idx) + pfIntvalAry(bn - 1)
                Else
                    pfSaveStartPos(0, Idx) = pfSaveStartPos(0, Idx) + pfIntvalAry(bn - 1)
                End If
            Next

            '' �u���b�N�ԍ����P�ȏ�̎��ɐݒ肷��
            'If (bn <= 1) Then Exit Sub

            'If (miCdir = 0) Then                            ' ���ߕ���X����
            '    pfStartPos(bn - 1) = pfSaveStartPos(1, bn) - pfSaveStartPos(1, bn - 1)
            '    Select Case gSysPrm.stDEV.giBpDirXy
            '        Case 0 ' x��, y��
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 1 ' x��, y��
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 2 ' x��, y��
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '        Case 3 ' x��, y��
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '    End Select
            'Else                                            ' ���ߕ���Y����
            '    pfStartPos(bn - 1) = pfSaveStartPos(0, bn) - pfSaveStartPos(0, bn - 1)
            '    Select Case gSysPrm.stDEV.giBpDirXy
            '        Case 0 ' x��, y��
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 1 ' x��, y��
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '        Case 2 ' x��, y��
            '            pfStartPos(bn - 1) = pfStartPos(bn - 1)
            '        Case 3 ' x��, y��
            '            pfStartPos(bn - 1) = -pfStartPos(bn - 1)
            '    End Select
            'End If
            '------ ###119�� -----

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTy2Teach.SetStartPosTbl() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����"
    '''=========================================================================
    ''' <summary>�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim strMSG As String = ""

        ' ð��وʒu�̾��X,Y�̎擾
        mdTbOffx = typPlateInfo.dblTableOffsetXDir
        mdTbOffy = typPlateInfo.dblTableOffsetYDir
        ' BP�ʒu�̾��X,Y�ݒ�
        mdBpOffx = typPlateInfo.dblBpOffSetXDir
        mdBpOffy = typPlateInfo.dblBpOffSetYDir
        ' ��ެ���߲�Ĉʒu�̾��X,Y�ݒ�
        mdAdjx = typPlateInfo.dblAdjOffSetXDir
        mdAdjy = typPlateInfo.dblAdjOffSetYDir
        ' ��ۯ���X,Y
        miBNx = typPlateInfo.intBlockCntXDir
        miBNy = typPlateInfo.intBlockCntYDir
        miCdir = typPlateInfo.intResistDir                  ' �`�b�v���ѕ����擾(CHIP-NET�̂�)

        miGNx = typPlateInfo.intGroupCntInBlockXBp          ' �O���[�v��X,Y
        miGNy = typPlateInfo.intGroupCntInBlockYStage

        Call CalcBlockSize(mdBSx, mdBSy)                    ' ��ۯ�����
        Call GetCutStartPoint(1, 1, mdSpx, mdSpy)           ' �����߲��

        mdStageX = gSysPrm.stDEV.gfTrimX + mdTbOffx + gfCorrectPosX
        mdStageY = gSysPrm.stDEV.gfTrimY + mdTbOffy + gfCorrectPosY

        Select Case gSysPrm.stDEV.giBpDirXy
            Case 0 ' x��, y��
                mdStageX = mdStageX + (mdBSx / 2)
                mdStageY = mdStageY + (mdBSy / 2)
            Case 1 ' x��, y��
                mdStageX = mdStageX - (mdBSx / 2)
                mdStageY = mdStageY + (mdBSy / 2)
            Case 2 ' x��, y��
                mdStageX = mdStageX + (mdBSx / 2)
                mdStageY = mdStageY - (mdBSy / 2)
            Case 3 ' x��, y��
                mdStageX = mdStageX - (mdBSx / 2)
                mdStageY = mdStageY - (mdBSy / 2)
        End Select

        ' ������4���ɂ���(�S�~������ꍇ�������) ###119
        strMSG = mdStageX.ToString("0.0000")
        mdStageX = Double.Parse(strMSG)
        strMSG = mdStageY.ToString("0.0000")
        mdStageY = Double.Parse(strMSG)

    End Sub
#End Region

#Region "�g���~���O�f�[�^���X�V����"
    '''=========================================================================
    '''<summary>�g���~���O�f�[�^���X�V����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub sUpdateStartPosition()

        Dim intForCnt As Short

        ' TY2�f�[�^�̃X�e�b�v�������X�V����
        For intForCnt = 1 To (MaxTy2 - 1)                               ' �u���b�N�����ݒ肷�� 
            ' TY2�f�[�^�̃X�e�b�v�����ɂ̓C���^�[�o���l(�`�b�v�T�C�Y������������)��ݒ肷�� ###119
            'typTy2InfoArray(intForCnt).dblTy22 = pfStartPos(intForCnt)
            typTy2InfoArray(intForCnt).dblTy22 = typTy2InfoArray(intForCnt).dblTy22 + pfIntvalAry(intForCnt)
        Next

        ' �u���b�N��X�����AY�����̊J�n�ʒu���Đݒ肷�� ###119
        Call CalcBlockXYStartPos()

    End Sub
#End Region

#Region "�ï�ߕ�ð��ٍ쐬"
    '''=========================================================================
    ''' <summary>�ï�ߕ�ð��ٍ쐬</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TyTeachingInitialize()

        'Dim i As Short
        'Dim rn As Short

        '----- ###119�� -----
        ' �X�e�b�v�����z��(pfStartPos())��ݒ肷�� 
        Call SetStartPosAry()
        piPositionNum = MaxTy2                                          ' �ï�ߕ�ð��ق̴��ؐ� 

        ' �e�B�[�`���O���W�z��(pfSaveStartPos())�ɑS�u���b�N��XY���W��ݒ肷��
        Call SetSaveStartPosAry()

        '' �X�e�b�v��ð��ٍ쐬
        'piPositionNum = 0
        'For i = 1 To MaxTy2
        '    rn = typTy2InfoArray(i).intTy21                             ' �u���b�N�ԍ�
        '    If rn < 1 Then Exit For
        '    piPositionNum = piPositionNum + 1
        '    ' �X�e�b�v����
        '    pfStartPos(piPositionNum) = typTy2InfoArray(i).dblTy22      ' �X�e�b�v����
        'Next
        'piPositionNum = piPositionNum - 1
        '----- ###119�� -----

    End Sub
#End Region

#Region "��ۯ��ړ�(ð��وړ�)"
    '''=========================================================================
    ''' <summary>��ۯ��ړ�(ð��وړ�)</summary>
    ''' <param name="intBlockNum">(INP)�u���b�N�ԍ�(1 ORG)</param>
    '''=========================================================================
    Private Function XYTableMoveSetBlock(ByRef intBlockNum As Short) As Short

        'Dim X As Double
        'Dim Y As Double
        'Dim intForCnt As Short
        'Dim dblInterval As Double
        Dim strMSG As String
        Dim r As Short

        Try
            '------ ###119�� -----
            ' XY�e�[�u�����w��̃u���b�N�ʒu�Ɉړ�����
            TBLx = pfSaveStartPos(0, intBlockNum)                       ' �e�[�u���ʒuX,Y���O���[�o����ɐݒ肷��
            TBLy = pfSaveStartPos(1, intBlockNum)
            r = Form1.System1.XYtableMove(gSysPrm, TBLx, TBLy)          ' �e�[�u���ړ�(��Βl)
            Return (r)                                                  ' Return�l�ݒ�

            '' ���̃u���b�N��XY�ړ�
            'dblInterval = 0.0
            'Select Case gSysPrm.stDEV.giBpDirXy
            '    Case 0 ' x��, y��
            '        ' ���ߕ������������s���B
            '        If (0 = miCdir) Then                        ' X����
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY + dblInterval
            '        Else                                        ' Y����
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX + dblInterval
            '            Y = mdStageY
            '        End If

            '    Case 1 ' x��, y��
            '        ' ���ߕ������������s���B
            '        If (0 = miCdir) Then                        ' X����
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY + dblInterval
            '        Else                                        ' Y����
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX - dblInterval
            '            Y = mdStageY
            '        End If

            '    Case 2 ' x��, y��
            '        ' ���ߕ������������s���B
            '        If (0 = miCdir) Then                        ' X����
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY - dblInterval
            '        Else                                        ' Y����
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX + dblInterval
            '            Y = mdStageY
            '        End If
            '    Case 3 ' x��, y��
            '        ' ���ߕ������������s���B
            '        If (0 = miCdir) Then                        ' X����
            '            X = mdStageX
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            Y = mdStageY - dblInterval
            '        Else                                        ' Y����
            '            For intForCnt = 1 To intBlockNum
            '                dblInterval = dblInterval + pfStartPos(intForCnt)
            '            Next
            '            X = mdStageX - dblInterval
            '            Y = mdStageY
            '        End If
            'End Select

            'TBLx = X
            'TBLy = Y
            'r = Form1.System1.XYtableMove(gSysPrm, TBLx, TBLy)
            'Return (r)                                      ' Return�l�ݒ�
            '------ ###119�� -----

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTy2Teach.XYTableMoveSetBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "��د�ނ̏�����"
    '''=========================================================================
    '''<summary>��د�ނ̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub GridInitialize()

        Dim head(4) As String
        Dim strHead As String
        Dim intForCntRows As Short
        Dim maxRows As Short

        If (0 = gSysPrm.stTMN.giMsgTyp) Then    'V4.4.0.0-0 FlexGrid�폜���ɍ폜����
            head(0) = "��ۯ��ԍ�"                                 ' ��ۯ��ԍ�
            head(1) = "�ï�ߋ���"                                 ' �ï�ߋ���
        Else
            head(0) = "BLOCK NO."                                 ' ��ۯ��ԍ�
            head(1) = "Step Interval"                                 ' �ï�ߋ���
        End If

        ' ���o��
        'head(0) = LBL_TY2_1                                 ' ��ۯ��ԍ�
        'head(1) = LBL_TY2_2                                 ' �ï�ߋ���
        strHead = head(0) & "|" & head(1)

        ' �O���b�h�̍ő�s���ݒ�
        If piPositionNum < 25 Then                          ' �ï�ߕ�ð��ق̴��ؐ� 
            maxRows = 25
        Else
            maxRows = piPositionNum + 1
        End If

        ' �O���b�h�̏����ݒ�
        With Me.FGridTy2Teach
            .FormatString = strHead                         ' ���o��
            .Cols = 2                                       ' �ő��
            .FixedCols = 1                                  ' �Œ��
            .Rows = maxRows                                 ' �ő�s��
            .FixedRows = 1                                  ' �Œ�s��

            For intForCntRows = 1 To maxRows - 1
                .Row = intForCntRows
                .Col = 1
                .CellAlignment = MSFlexGridLib.AlignmentSettings.flexAlignRightCenter
            Next

            ' �u���b�N�ԍ�/�ï�ߕ��̏����l�\��
            For intForCntRows = 1 To piPositionNum
                .set_TextMatrix(intForCntRows, 0, CStr(intForCntRows))
                .set_TextMatrix(intForCntRows, 1, pfStartPos(intForCntRows).ToString("0.0000 "))
            Next intForCntRows

        End With

        ' �O���b�h�̏����ݒ�
        With dgvTy2Teach                ' V4.0.0.0�G
            .SuspendLayout()
            '.Columns(0).HeaderText = head(0)
            '.Columns(1).HeaderText = head(1)
            .Rows.Add(maxRows - 1)

            ' �u���b�N�ԍ�/�ï�ߕ��̏����l�\��
            For i As Integer = 1 To piPositionNum Step 1
                With .Rows(i - 1)
                    .Cells(0).Value = i.ToString()
                    .Cells(1).Value = pfStartPos(i).ToString("0.0000 ")
                End With
            Next i
            .ResumeLayout()
        End With

        Call SetTopRow(1)                                   ' �O���b�h�̐擪�s�ԍ���ݒ肷��

    End Sub
#End Region

#Region "�O���b�h�̐擪�s�ԍ���ݒ肷��"
    '''=========================================================================
    '''<summary>�O���b�h�̐擪�s�ԍ���ݒ肷��</summary>
    '''<param name="iRow">(INP)�s�ԍ�</param>
    '''=========================================================================
    Private Sub SetTopRow(ByVal iRow As Integer)

        With Me.FGridTy2Teach
            .TopRow = iRow
        End With

        dgvTy2Teach.FirstDisplayedScrollingRowIndex = (iRow - 1) ' V4.0.0.0�G

    End Sub
#End Region

#Region "�O���b�h�̎w��s�̔w�i�F�����F�ɂ���"
    '''=========================================================================
    '''<summary>�w��s�̔w�i�F�����F�ɂ���</summary>
    '''<param name="iRow">(INP)�s</param>
    '''=========================================================================
    Private Sub SetBackColor(ByVal iRow As Integer)

        Static iPrevColumn As Integer = 0

        'If IsNothing(iPrevColumn) Then
        '    iPrevColumn = 0
        'End If

        With Me.FGridTy2Teach

            If iPrevColumn >= 0 Then
                If .Rows > iPrevColumn Then
                    .Row = iPrevColumn
                    .Col = 1
                    .CellBackColor = System.Drawing.Color.White
                    .Col = 2
                    .CellBackColor = System.Drawing.Color.White
                End If
            End If

            .Row = iRow
            .Col = 1
            .CellBackColor = System.Drawing.Color.Yellow
            .Col = 2
            .CellBackColor = System.Drawing.Color.Yellow

        End With

        With dgvTy2Teach                ' V4.0.0.0�G
            .SuspendLayout()
            If (0 < iPrevColumn) Then
                If (iPrevColumn < .CurrentRow.Index) Then
                    With .Rows(iPrevColumn - 1)
                        '.Cells(0).Style.BackColor = SystemColors.Window
                        .Cells(1).Style.BackColor = SystemColors.Window
                    End With
                End If
            End If
            With .Rows(iRow - 1)
                '.Cells(0).Style.BackColor = Color.Yellow
                .Cells(1).Style.BackColor = Color.Yellow
            End With
            .ResumeLayout()
        End With

        iPrevColumn = iRow

    End Sub
#End Region

#Region "�O���b�h�̒l���X�V����"
    '''=========================================================================
    '''<summary>�X�e�b�v�����̃O���b�h�̒l���X�V����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub sUpdateGridData()

        Dim intForCnt As Integer

        ' �X�e�b�v�����̃O���b�h�̒l���X�V����
        For intForCnt = 1 To MaxTy2 - 1
            Me.FGridTy2Teach.set_TextMatrix(intForCnt, 1, pfStartPos(intForCnt).ToString("0.0000 "))

            dgvTy2Teach(1, intForCnt - 1).Value = pfStartPos(intForCnt).ToString("0.0000 ")         ' V4.0.0.0�G
        Next

    End Sub
#End Region

#Region "�X�e�b�v�����̃O���b�h�l���g���~���O�f�[�^���\������"
    '''=========================================================================
    '''<summary>�X�e�b�v�����̃O���b�h�l���g���~���O�f�[�^���\������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub sDefSetData()

        Dim intForCnt As Integer

        ' �X�e�b�v�����z��(pfStartPos())��ݒ肷�� ###119
        Call SetStartPosAry()

        ' �e�B�[�`���O���W�z��(pfSaveStartPos())�ɑS�u���b�N��XY���W��ݒ肷�� ###119
        Call SetSaveStartPosAry()

        ' �X�e�b�v�����̃O���b�h�̒l��\������
        'For intForCnt = 1 To MaxTy2                                     ' �u���b�N�����ݒ肷��  
        For intForCnt = 1 To MaxTy2 - 1                                 ' �u���b�N�����ݒ肷��        V4.0.0.0�G
            'pfStartPos(intForCnt) = typTy2InfoArray(intForCnt).dblTy22 ' ###119 
            Me.FGridTy2Teach.set_TextMatrix(intForCnt, 1, pfStartPos(intForCnt).ToString("0.0000 "))

            dgvTy2Teach(1, intForCnt - 1).Value = pfStartPos(intForCnt).ToString("0.0000 ")         ' V4.0.0.0�G
        Next

    End Sub
#End Region

#Region "�X�e�b�v�����z��(pfStartPos())��ݒ肷��"
    '''=========================================================================
    ''' <summary>�X�e�b�v�����z��(pfStartPos())��ݒ肷�� ###119</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetStartPosAry()

        Dim Idx As Short
        Dim strMSG As String

        ' �X�e�b�v�����z��(pfStartPos())��gBlkStagePosX()/gBlkStagePosY()����ݒ肷��
        For Idx = 1 To MaxTy2 - 1                                       ' �u���b�N�����ݒ肷�� 
            If (miCdir = 0) Then                                        ' �`�b�v���ѕ��� = X�����̏ꍇ
                pfStartPos(Idx) = gBlkStagePosY(Idx) - gBlkStagePosY(Idx - 1)
            Else                                                        ' �`�b�v���ѕ��� = Y�����̏ꍇ
                pfStartPos(Idx) = gBlkStagePosX(Idx) - gBlkStagePosX(Idx - 1)
            End If

            ' ������4���ɂ���(�S�~������ꍇ�������)
            strMSG = pfStartPos(Idx).ToString("0.0000")
            pfStartPos(Idx) = Double.Parse(strMSG)
        Next Idx

        ' �X�e�b�v�����z��(�ŏI�u���b�N)�͏���������
        pfStartPos(MaxTy2) = 0.0

    End Sub
#End Region

#Region "�e�B�[�`���O���W�z��(pfSaveStartPos())��ݒ肷��"
    '''=========================================================================
    ''' <summary>�e�B�[�`���O���W�z��(pfSaveStartPos())��ݒ肷�� ###119</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetSaveStartPosAry()

        Dim Idx As Short

        ' �e�B�[�`���O���W�z��ɑS�u���b�N��XY���W��ݒ肷��
        For Idx = 0 To (MaxTy2 - 1)                                     ' �u���b�N�����J��Ԃ� 
            If (miCdir = 0) Then                                        ' �`�b�v���ѕ��� = X�����̏ꍇ
                pfSaveStartPos(0, Idx + 1) = mdStageX
                pfSaveStartPos(1, Idx + 1) = mdStageY + gBlkStagePosY(Idx)
            Else
                pfSaveStartPos(0, Idx + 1) = mdStageX + gBlkStagePosX(Idx)
                pfSaveStartPos(1, Idx + 1) = mdStageY
            End If

            ' �����e�[�u��(1�ؼ��)������
            pfIntvalAry(Idx) = 0.0
        Next Idx
        pfIntvalAry(MaxTy2) = 0.0

        '' �e�B�[�`���O���W�z��0�ɑ�1�u���b�N��XY���W��ݒ肷��
        'pfSaveStartPos(0, 0) = pfSaveStartPos(0, 1)
        'pfSaveStartPos(1, 0) = pfSaveStartPos(1, 1)

    End Sub
#End Region

#Region "DataGridView���I����Ԃ̐F�ɂȂ�̂��L�����Z������"
    '''=========================================================================
    ''' <summary>DataGridView���I����Ԃ̐F�ɂȂ�̂��L�����Z������</summary>
    '''=========================================================================
    Private Sub dgvTy2Teach_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dgvTy2Teach.SelectionChanged
        dgvTy2Teach.Rows(dgvTy2Teach.CurrentRow.Index).Selected = False ' V4.0.0.0�G
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
    Private Sub frmTy2Teach_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0�J
        Console.WriteLine("frmTy2Teach.frmTy2Teach_KeyDown()")

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V6.0.0.0�K        GrpTyTeach_2.Enabled = False                                    ' ###086
        'V6.0.0.0�K       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0�K
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5�� (KeyCode = 101(&H65)
            'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
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
        Console.WriteLine("frmTy2Teach.frmTy2Teach_KeyKeyUp()")
        ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
        'V6.0.0.0�K        GrpTyTeach_2.Enabled = True                                     ' ###086
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

End Class