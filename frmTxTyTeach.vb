'===============================================================================
'   Description  : �s�w/�s�x�e�B�[�`���O��ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmTxTyTeach
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

#Region "�v���C�x�[�g�ϐ���`"
    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Integer) As Integer '###228

    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- �I�u�W�F�N�g��` -----
    Private objTxTyMsgbox As frmTxTyMsgbox                          ' TX/TYè�ݸޏI���m�F��ʵ�޼ު�� 
    Private stJOG As JOG_PARAM                                      ' �����(BP��JOG����)�p�p�����[�^
    Private mExit_flg As Short                                      ' �s�w/�s�x�e�B�[�`���O����

    '----- �f�[�^�O���b�h�r���[�p -----
    Private RowAry() As DataGridViewRow                             ' Row(�s)�I�u�W�F�N�g�z��
    Private ColAry() As DataGridViewColumn                          ' Col(��)�I�u�W�F�N�g�z��

    Private miStepBlock(MaxCntStep) As Short                        ' �ύX�O����ۯ���
    Private mdStepInterval(MaxCntStep) As Double                    ' �ύX�O�̽ï�ߊԲ������
    'Private tmpStepInterval(MaxCntStep) As Double                   ' �ύX��̽ï�ߊԲ������
    Private ZRPosX(1 + (MaxCntStep * 2)) As Double                  ' �����X(0:��1��_�@1:��2��_)
    Private ZRPosY(1 + (MaxCntStep * 2)) As Double                  ' �����Y(0:��1��_�@1:��2��_)

    '----- �X�e�b�v�I�t�Z�b�g�ʎZ�o��(TX/TY����) -----              ' ###091
    Private StepOffSetX(2) As Double                                ' �I�t�Z�b�g�ʒuX(0:��1��_�@1:��2��_)
    Private StepOffSetY(2) As Double                                ' �I�t�Z�b�g�ʒuX(0:��1��_�@1:��2��_)

    'Private mdBlockCnt(MaxCntResist) As Double                      ' �ύX�O�̃u���b�N��
    'Private mdStepBlockX(MaxCntResist) As Double                    ' �ύX�O�̃u���b�N�C���^�[�o��X
    'Private mdStepBlockY(MaxCntResist) As Double                    ' �ύX�O�̃u���b�N�C���^�[�o��Y
    'Private tmpStepBlockX(MaxCntResist) As Double                   ' �ύX��̃u���b�N�C���^�[�o��X
    'Private tmpStepBlockY(MaxCntResist) As Double                   ' �ύX��̃u���b�N�C���^�[�o��Y

    '----- ���̑� -----
    Private dblTchMoval(3) As Double                                ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))

    Dim dblSaveTXChipsizeRelation As Double                         ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
    Dim dblSaveBpGrpItv As Double                                   ' �O�̃C���^�[�o���̕ۑ� 'V4.5.1.0�N

    ' �ȉ��͖��g�p�̂��ߍ폜 ?
    'Private ChipSize As Double                          ' �␳������߻���
    'Private Ino As Short                                ' �������̲������No
    'Private TBLx As Double                                          ' ð��وړ����WX(��Βl)
    'Private TBLy As Double                                          ' ð��وړ����WY(��Βl)
    'Private KJPosX As Double                                        ' ��ʒuX(��1��ۯ��̐擪��ʒu)
    'Private KJPosY As Double                                        ' ��ʒuY(��1��ۯ��̐擪��ʒu)
    'Private Pos2ndX As Double                                       ' ��ʒuX(��1��ۯ��̍ŏI��ʒu)
    'Private Pos2ndY As Double                                       ' ��ʒuY(��1��ۯ��̍ŏI��ʒu)
    'Private EffBN As Short                              ' �L����ۯ���
    'Private EffGN As Short                              ' �L����ٰ�ߐ�

    '----- �g���~���O�p�����[�^ -----
    'Private mdTbOffx As Double                          ' ð��وʒu�̾��X
    'Private mdTbOffy As Double                          ' ð��وʒu�̾��Y
    'Private mdBpOffx As Double                          ' BP�ʒu�̾��X
    'Private mdBpOffy As Double                          ' BP�ʒu�̾��Y
    'Private mdAdjx As Double                            ' ��ެ�Ĉʒu�̾��X
    'Private mdAdjy As Double                            ' ��ެ�Ĉʒu�̾��Y
    'Private miBNx As Short                              ' ��ۯ���X
    'Private miBNy As Short                              ' ��ۯ���Y
    'Private miCdir As Short                             ' ���ߕ��ѕ���
    'Private miChpNum As Short                           ' ���ߐ�
    'Private mdCSx As Double                             ' ���߻���X
    'Private mdCSy As Double                             ' ���߻���Y
    'Private miGNx As Short                              ' ��ٰ�ߐ�X
    'Private miGNy As Short                              ' ��ٰ�ߐ�Y
    'Private mdBSx As Double                             ' ��ۯ�����X
    'Private mdBSy As Double                             ' ��ۯ�����Y
    'Private dStepOffx As Double                         ' �ï�ߵ̾�ė�X
    'Private dStepOffy As Double                         ' �ï�ߵ̾�ė�Y
    'Private miPP30 As Short                             ' �␳Ӱ��
    'Private miPP31 As Short                             ' �␳���@
    'Private mdSpx As Double                             ' �����߲��X
    'Private mdSpy As Double                             ' �����߲��Y

#End Region

#Region "�I�����ʂ�Ԃ�"
    ' '''=========================================================================
    ' '''�@<summary>�I�����ʂ�Ԃ�</summary>
    ' '''�@<returns>cFRS_ERR_START = OK(START��)
    ' '''  �@       cFRS_ERR_RST   = Cancel(RESET��)
    ' '''    �@     cFRS_TxTy      = TX2(Teach)/TY2����
    ' '''      �@   -1�ȉ�         = �G���[</returns>
    ' '''=========================================================================
    'Public ReadOnly Property sGetReturn() As Integer Implements ICommonMethods.sGetReturn
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
            ' �f�[�^�O���b�h�r���[�p�I�u�W�F�N�g�z�񏉊���
            RowAry = Nothing                                        ' Row(�s)�I�u�W�F�N�g�z��
            ColAry = Nothing                                        ' Col(��)�I�u�W�F�N�g�z��

            'V6.0.0.0�K                  ��
            stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                         BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            'V6.0.0.0�K                  ��

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.Form_Initialize_Renamed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form �I��������"
    '''=========================================================================
    '''<summary>Form �I��������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTxTeach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim strMSG As String

        Try
            ' �f�[�^�O���b�h�r���[�I�u�W�F�N�g�̊J��
            Call TermGridView(RowAry, ColAry)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.frmTxTeach_FormClosed() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form Load������"
    '''=========================================================================
    '''<summary>Form Load������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmTxTeach_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String

        Try
            ' ��������
            Call SetDispMsg()                                       ' ���x������ݒ肷��(���{��/�p��)
            mExit_flg = -1                                          ' �I���t���O = ������

            ' ����޳����۰قɏd�˂�
            Me.Top = Form1.Text4.Top + DISPOFF_SUBFORM_TOP
            Me.Left = Form1.Text4.Left

            ' �g���~���O�f�[�^���K�v�ȃp�����[�^���擾����
            Call SetTrimData()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.frmTxTeach_Load() TRAP ERROR = " + ex.Message
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
                If (giAppMode = APP_MODE_TX) Then
                    ' �s�w�e�B�[�`���O��
                    ' label
                    .lblTitle.Text = TITLE_TX                       ' ����
                    '.lblTitle2.Text = ""
                    '.LblDisp_8.Text = LBL_TXTY_TEACH_04             ' �␳�䗦
                    '.LblDisp_9.Text = LBL_TXTY_TEACH_05             ' �`�b�v�T�C�Y(mm)
                    .LblDisp_10.Text = LBL_TXTY_TEACH_09            ' �O���[�v�C���^�[�o��
                Else
                    ' �s�x�e�B�[�`���O��
                    ' label
                    .lblTitle.Text = TITLE_TY                       ' ����
                    '.lblTitle2.Text = ""
                    '.LblDisp_8.Text = LBL_TXTY_TEACH_04             ' �␳�䗦
                    '.LblDisp_9.Text = LBL_TXTY_TEACH_05             ' �`�b�v�T�C�Y(mm)
                    .LblDisp_10.Text = LBL_TXTY_TEACH_11            ' �X�e�b�v�C���^�[�o��
                End If

                .lblTitle2.Text = ""

                ' �s�w/�s�x�e�B�[�`���O����
                ' frame
                '.GrpFram_0.Text = LBL_TXTY_TEACH_12                 ' ��P��_
                '.GrpFram_1.Text = LBL_TXTY_TEACH_13                 ' ��Q��_
                '.GrpFram_2.Text = LBL_TXTY_TEACH_03                 ' �␳��
                ' button
                '.cmdOK.Text = "OK"
                '.cmdCancel.Text = CMD_CANCEL
            End With

            ' �\���֘A������
            Call InitDisp()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.SetDispMsg() TRAP ERROR = " + ex.Message
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
                Me.TxtPosX.Text = ""                                ' ��1��_XY
                Me.TxtPosY.Text = ""
                Me.TxtPos2X.Text = ""                               ' ��2��_XY
                Me.TxtPos2Y.Text = ""

                Me.LblResult_0.Text = ""                            ' �␳�䗦
                Me.LblResult_1.Text = ""                            ' �`�b�v�T�C�Y
                Me.LblResult_2.Text = ""                            ' �`�b�v�T�C�Y(�␳��)
            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.InitDisp() TRAP ERROR = " + ex.Message
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
    Private Sub frmTxTeach_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim strMSG As String

        Try
            ' �s�w/�s�x�e�B�[�`���O�J�n
            If (mExit_flg <> -1) Then Exit Sub
            mExit_flg = 0                                           ' �I���t���O = 0
            mExit_flg = TxTyMainProc()                              ' �s�w�܂��͂s�x�e�B�[�`���O�J�n

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
            ' �s�w/�s�x�e�B�[�`���O�J�n
            If (mExit_flg <> -1) Then Exit Function
            mExit_flg = 0                                           ' �I���t���O = 0
            mExit_flg = TxTyMainProc()                              ' �s�w�܂��͂s�x�e�B�[�`���O�J�n

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
        Return mExit_flg                ' sGetReturn ��荞��   'V6.0.0.0�L

    End Function
#End Region

    '=========================================================================
    '   �s�w�܂��͂s�x�e�B�[�`���O����
    '=========================================================================
#Region "�I���m�F���b�Z�[�W�\������"
    '''=========================================================================
    '''<summary>�I���m�F���b�Z�[�W�\������</summary>
    '''<remarks>Exit_flg = cFRS_ERR_START(OK(START��))
    '''                    cFRS_ERR_RST(Cancel(RESET��))
    '''                    cFRS_TxTy(TX2/TY2����)</remarks>
    '''=========================================================================
    Private Sub Disp_TxTyMsgBox(ByRef Exit_flg As Short)

        Dim strMSG As String

        Try
            'If (giAppMode = APP_MODE_TX) Then
            '    MSG_EXECUTE_TXTYLABEL = "Teach"
            'Else
            '    MSG_EXECUTE_TXTYLABEL = "TY2"
            'End If
            objTxTyMsgbox = New frmTxTyMsgbox()                 ' TX/TYè�ݸޏI���m�F��ʵ�޼ު�Đ��� 

            ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H"
            objTxTyMsgbox.ShowDialog()                          ' �I���m�F���b�Z�[�W�\�� 
            Exit_flg = objTxTyMsgbox.sGetReturn()               ' Exit_flg = �ݒ�

            Call objTxTyMsgbox.Close()                          ' TX/TYè�ݸޏI���m�F��ʵ�޼ު�ĊJ��
            Call objTxTyMsgbox.Dispose()                        ' ���\�[�X�J��
            objTxTyMsgbox = Nothing

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.Disp_TxTyMsgBox() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "OK���݉���������"
    '''=========================================================================
    ''' <summary>OK���݉���������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdOK_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cmdOK.MouseUp  'V1.16.0.0�L
        'Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

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
        'Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

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
    Private Function TxTyMainProc() As Short

        Dim i As Short
        'Dim rtn As Short                                    ' TxTyMainProc�ߒl 
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
        Dim mdSpx As Double                                 ' �����߲��X
        Dim mdSpy As Double                                 ' �����߲��Y
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
            ' �O���[�v��, �u���b�N��(TY��)�܂��̓`�b�v��(��R��)(TX��), �`�b�v�T�C�Y���擾����
            Me.cmdOK.Enabled = False                                    ' ��Q��֍s���܂ŗL���Ƃ��Ȃ�'V3.0.0.0�B
            r = GetChipNumAndSize(giAppMode, GrpNum, RnBn, ChipSize)
            tmpChipSize = ChipSize                                      ' �`�b�v�T�C�Y�ޔ�
            GrpCnt = 0                                                  ' ��ٰ�ߐ�����
            mdAdjx = 0.0 : mdAdjy = 0.0                                 ' ��ެ�ĈʒuX(���g�p)
            mdBpOffx = typPlateInfo.dblBpOffSetXDir                     ' BP�ʒu�̾��X,Y�ݒ�
            mdBpOffy = typPlateInfo.dblBpOffSetYDir
            dStepOffx = typPlateInfo.dblStepOffsetXDir                  ' �ï�ߵ̾�ė�X,Y(TYè���p)
            dStepOffy = typPlateInfo.dblStepOffsetYDir
            Call CalcBlockSize(mdBSx, mdBSy)                            ' �u���b�N�T�C�YXY�ݒ�

            ' �f�[�^�O���b�h�r���[�I�u�W�F�N�g�̐���
            Call InitGridView(GridView, GrpNum, RowAry, ColAry)         ' �O���[�v���2��ȏ�̎��ɸ�د�ޕ\������

STP_RETRY:
            ' ���W�\�����N���A
            For i = 0 To 1 + MaxCntStep                                 ' �O���[�v�C���^�[�o��������
                ZRPosX(i) = 0.0#
                ZRPosY(i) = 0.0#
            Next i
            For i = 0 To 1                                              ' �X�e�b�v�I�t�Z�b�g�ʎZ�o�揉���� ###091
                StepOffSetX(i) = 0.0#
                StepOffSetY(i) = 0.0#
            Next i

            Call InitDisp()                                             ' ���W�\���N���A
            Call ClearGridView(RowAry)                                  ' �f�[�^�O���b�h�r���[�̃C���^�[�o���\�����N���A����
            ChipSize = tmpChipSize                                      ' �`�b�v�T�C�Y�ݒ�

            ' BP���1��ۯ��A��1��R�����߲�ĂɈړ�����
            If (giAppMode = APP_MODE_TY) Then
                Call BpMoveOrigin_Ex()                                  ' BP����ۯ��̉E��Ɉړ�����
                Call XYTableMoveTopBlock(KJPosX, KJPosY)                ' �擪��ۯ��̐擪��e�[�u���ʒuX,Y���擾����
            End If
            Call GetCutStartPoint(1, 1, mdSpx, mdSpy)                   ' ����R�̑��J�b�g�̽����߲�Ă��擾
            r = Form1.System1.EX_MOVE(gSysPrm, mdSpx, mdSpy, 1)         ' BP�����R�̑��J�b�g�̽����߲�ĂɈړ�
            If (r < cFRS_NORMAL) Then
                Return (r)                                              ' Return�l = �G���[
            End If
            If (giAppMode = APP_MODE_TX) Then
                KJPosX = mdSpx                                          ' KJPosX = ��1��ۯ��̐擪�BP�ʒuX
                KJPosY = mdSpy                                          ' KJPosY = ��1��ۯ��̐擪�BP�ʒuY
            End If

            ' �����(BP��JOG����)�p�p�����[�^������������
            If (giAppMode = APP_MODE_TX) Then                           ' TX ? 
                stJOG.Md = MODE_BP                                      ' ���[�h(1:BP�ړ�)
            Else
                stJOG.Md = MODE_STG                                     ' ���[�h(0:XY�e�[�u���ړ�)
                Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C
            End If
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

STP_CHIPSIZE:
            '-------------------------------------------------------------------
            '   �`�b�v�T�C�Y�w(TX��)�܂��̓`�b�v�T�C�Y�x(TY��)�̃e�B�[�`���O����
            '   ��CSPoint(0:��1��_, 1:��2��_)��BP�܂��̓e�[�u���ʒu��ݒ肷��
            '-------------------------------------------------------------------
            ' �`�b�v�T�C�Y�̃e�B�[�`���O����
            r = Sub_Jog1(RnBn, ChipSize, KJPosX, KJPosY, GrpNum, GrpCnt, CSPoint)
            Timer1.Enabled = False                                      ' ###228
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��
                Return (r)
            End If
            If (r <> cFRS_NORMAL) Then                                  ' �����p���ȊO(RESET SW(Cancel����)����/OK�{�^������)�Ȃ�g���~���O�f�[�^�X�V��
                GoTo STP_END
            End If

STP_INTVAL:
            '-------------------------------------------------------------------
            '   �a�o�O���[�v(�T�[�L�b�g)�Ԋu(TX��)�܂���
            '   �X�e�[�W�O���[�v�Ԋu(TY��)�̃e�B�[�`���O����
            '     mdStepInterval()�ɃC���^�[�o���l��ݒ肷��
            '-------------------------------------------------------------------
            ' �C���^�[�o���̃e�B�[�`���O����
            r = Sub_Jog2(RnBn, ChipSize, KJPosX, KJPosY, GrpNum, GrpCnt, CSPoint)
            Timer1.Enabled = False                                      ' ###228
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��
                Return (r)
            End If
            If (r = cFRS_ERR_HALT) Then                                 ' HALT SW�����Ȃ�`�b�v�T�C�Y�e�B�[�`���O������
                GoTo STP_CHIPSIZE
            End If
            If (r <> cFRS_NORMAL) Then                                  ' �����p���ȊO(RESET SW(Cancel����)����/OK�{�^������)�Ȃ�g���~���O�f�[�^�X�V��
                GoTo STP_END
            End If


            '###121
            '-------------------------------------------------------------------
            '   �X�e�b�v�I�t�Z�b�g�ʂ̎Z�o(�s�x�e�B�[�`���O��)
            '   TY�e�B�[�`���O�ŏI�u���b�N�e�B�[�`���O����X�����̂���ʂ��X�e�b�v�I�t�Z�b�g�ƂȂ�
            '-------------------------------------------------------------------
            ' �X�e�b�v�I�t�Z�b�g�ʂ̃e�B�[�`���O����

            'Nop Nop Nop    2012.11.20 kami del
            'r = Sub_Jog3(RnBn, ChipSize, KJPosX, KJPosY, GrpNum, GrpCnt, CSPoint, dStepOffx, dStepOffy)
            'If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�G���[���^�[��
            '    Return (r)
            'End If
            'If (r = cFRS_ERR_HALT) Then                                 ' HALT SW�����Ȃ�X�e�[�W�O���[�v�Ԋu�̃e�B�[�`���O������
            '    GoTo STP_INTVAL
            'End If
            'Nop Nop Nop    2012.11.20 kami del
            '###121


STP_END:
            '-------------------------------------------------------------------
            '   �g���~���O�f�[�^�X�V
            '-------------------------------------------------------------------
            If (r <> cFRS_ERR_RST) Then                                 ' Cancel(RESET SW)�����ȊO ?
                Call SetTrimParameter(ChipSize, GrpCnt)
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

#Region "�`�b�v�T�C�Y�w�܂��̓`�b�v�T�C�Y�x�̃e�B�[�`���O����"
    '''=========================================================================
    ''' <summary>�`�b�v�T�C�Y�w�܂��̓`�b�v�T�C�Y�x�̃e�B�[�`���O����</summary>
    ''' <param name="RnBn">    (INP)�`�b�v��(��R��)(TX��), �u���b�N��(TY��)</param>
    ''' <param name="ChipSize">(I/O)�`�b�v�T�C�Y</param>
    ''' <param name="KJPosX">  (INP)�擪�u���b�N�̐擪��ʒuX</param>
    ''' <param name="KJPosY">  (INP)�擪�u���b�N�̐擪��ʒuY</param>
    ''' <param name="GrpNum">  (INP)�O���[�v��</param>
    ''' <param name="GrpCnt">  (OUT)�O���[�v�J�E���^</param>
    ''' <param name="CSPoint"> (OUT)���߻��ގZ�o�p(0:��1��_, 1:��2��_)</param>
    ''' <returns>cFRS_ERR_START = OK(START��)����
    '''          cFRS_ERR_RST   = Cancel(RESET��)����
    '''          cFRS_NORMAL    = ����(�O���[�v�ԃC���^�[�o��������)
    '''          -1�ȉ�         = �G���[</returns>
    '''=========================================================================
    Private Function Sub_Jog1(ByVal RnBn As Short, ByRef ChipSize As Double, ByVal KJPosX As Double, ByVal KJPosY As Double, _
                              ByVal GrpNum As Short, ByRef GrpCnt As Short, ByRef CSPoint() As Double) As Short

        Dim PosX As Double                                              ' ���݂�BP�ʒuX/ð��وړ����WX(��Βl)
        Dim PosY As Double                                              ' ���݂�BP�ʒuY/ð��وړ����WY(��Βl)
        Dim WkChipSize As Double
        Dim iPos As Short
        Dim r As Short
        Dim rtn As Short                                                ' �ߒl 
        Dim strMSG As String

        Try
            ' ��������
            Timer1.Enabled = True                                       ' ###228
            iPos = 0                                                    ' iPos = ��1��_(0:��1��_�ʒu, 1:��2��_�ʒu)
            GrpCnt = 1                                                  ' ��ٰ�ߐ�����
            WkChipSize = ChipSize                                       ' �`�b�v�T�C�Y�ޔ� 

            If (giAppMode = APP_MODE_TX) Then                           ' �^�C�g���ݒ� ###084
                Me.lblTitle2.Text = INFO_MSG13 + INFO_MSG32             ' �^�C�g�� = "�`�b�v�T�C�Y�@�e�B�[�`���O (TX)"
            Else
                Me.lblTitle2.Text = INFO_MSG13 + INFO_MSG33             ' �^�C�g�� = "�`�b�v�T�C�Y�@�e�B�[�`���O (TY)"
            End If
            'Me.lblTitle2.Text = INFO_MSG13                             ' �^�C�g�� = "�`�b�v�T�C�Y�@�e�B�[�`���O"

            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸�
            Me.GrpFram_1.Visible = True                                 ' ����_�\��(Sub_Jog3()�Ŕ�\���ɂ��邽��)
            Call InitDisp()                                             ' ���W�\���N���A
            Call ClearGridView(RowAry)                                  ' �f�[�^�O���b�h�r���[�̃C���^�[�o���\�����N���A����
            If (giAppMode = APP_MODE_TY) Then                           ' �s�x�e�B�[�`���O���̓u���b�N�����X�e�[�W�O���[�v���u���b�N���Ƃ���
                If (typPlateInfo.intResistDir = 0) Then                 ' �`�b�v���т�X���� ?
                    RnBn = typPlateInfo.intBlkCntInStgGrpY              ' �u���b�N�� = �X�e�[�W�O���[�v���u���b�N��Y 
                Else
                    RnBn = typPlateInfo.intBlkCntInStgGrpX              ' �u���b�N�� = �X�e�[�W�O���[�v���u���b�N��X
                End If
            End If

STP_RETRY:
            Call Me.Focus()
            Do
                ' BP�ړ��܂���XYð��وړ�(��1��ٰ�ߑ�1��ʒu/��2��ʒu)
                If (giAppMode = APP_MODE_TX) Then                       ' �s�w�e�B�[�`���O��
                    ' BP���1��ʒu�܂��͑�2��ʒu�ړ�����
                    r = XYBPMoveSetBlock(iPos, iPos, GrpCnt, RnBn, ChipSize, KJPosX, KJPosY, PosX, PosY, GrpNum)     'V6.1.4.9�A�@�T�[�L�b�g��GrpNum�ǉ�
                Else                                                    ' �s�x�e�B�[�`���O��
                    ' XYð��ق��1��ʒu�܂��͑�2��ʒu�ړ�����
                    r = XYTableMoveSetBlock(iPos, iPos, GrpCnt, RnBn, ChipSize, KJPosX, KJPosY, PosX, PosY)
                End If
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Return (r)                                          ' �G���[���^�[��
                End If

                ' �\�����b�Z�[�W����ݒ肷�� 
                If (iPos = 0) Then                                      ' ��1��_ ? 
                    '"��1�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
                    lblInfo.Text = INFO_MSG18 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPosX                               ' ��1��_���WX�ʒu�\���p÷���ޯ��
                    stJOG.TextY = TxtPosY                               ' ��1��_���WY�ʒu�\���p÷���ޯ��
                    TxtPosX.Enabled = True
                    TxtPosY.Enabled = True
                Else                                                    ' ��2��_ 
                    '"��n�O���[�v�A�ŏI��R��ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" & vbCrLf & "[HALT]�łP�O�̏����ɖ߂�܂��B"
                    lblInfo.Text = INFO_MSG19 & iPos & INFO_MSG20 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPos2X                              ' ��2��_���WX�ʒu�\���p÷���ޯ��
                    stJOG.TextY = TxtPos2Y                              ' ��2��_���WY�ʒu�\���p÷���ޯ��
                    TxtPos2X.Enabled = True
                    TxtPos2Y.Enabled = True
                    Me.cmdOK.Enabled = True                             ' ��Q��֍s���܂ŗL���Ƃ��Ȃ�'V3.0.0.0�B
                End If

                ' ��1��_/��2��_�̃e�B�[�`���O"����
                stJOG.PosX = PosX                                       ' BP X�܂���XYð��� X��Έʒu
                stJOG.PosY = PosY                                       ' BP Y�܂���XYð��� Y��Έʒu
                stJOG.Flg = -1                                          ' �e��ʂ�OK/Cancel���݉����׸�
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�I��
                    Return (r)
                End If

                ' ��1��_/��2��_�̃I�t�Z�b�g�l�X�V
                ZRPosX(iPos) = ZRPosX(iPos) + stJOG.cgX
                ZRPosY(iPos) = ZRPosY(iPos) + stJOG.cgY
                ' V1.16.0.1�@ ADD START-----------------------------------------------------
                If (iPos = 0) Then                                      ' ��1��_ ? 
                    If (typPlateInfo.intResistDir = 0) Then             ' ���ߕ��т�X���� ?
                        If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                            ZRPosY(1) = ZRPosY(iPos)
                        End If
                    Else
                        If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                            ZRPosX(1) = ZRPosX(iPos)
                        End If
                    End If
                End If
                ' V1.16.0.1�@ ADD END  -----------------------------------------------------

                ' �R���\�[���L�[�`�F�b�N
                If (r = cFRS_ERR_START) Then                            ' START SW���� ?
                    ' ��1��_/��2��_�̍��W�X�V
                    If (typPlateInfo.intResistDir = 0) Then             ' ���ߕ��т�X���� ?
                        If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                            CSPoint(iPos) = stJOG.PosX                  ' CSPoint(0:��1��_�ʒu, 1:��2��_�ʒu)�ɐݒ肷��
                        Else                                            ' �s�x�e�B�[�`���O��
                            CSPoint(iPos) = stJOG.PosY
                        End If
                    Else
                        If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                            CSPoint(iPos) = stJOG.PosY                  ' CSPoint(0:��1��_�ʒu, 1:��2��_�ʒu)�ɐݒ肷��
                        Else                                            ' �s�x�e�B�[�`���O��
                            CSPoint(iPos) = stJOG.PosX
                        End If
                    End If

                    ' �X�e�b�v�I�t�Z�b�g�ʒu�X�V(��1��_/��2��_) ###091
                    If (iPos = 0) Then                                  ' ��1��_
                        StepOffSetX(0) = Double.Parse(TxtPosX.Text)
                        StepOffSetY(0) = Double.Parse(TxtPosY.Text)
                    Else                                                ' ��2��_
                        StepOffSetX(1) = Double.Parse(TxtPos2X.Text)
                        StepOffSetY(1) = Double.Parse(TxtPos2Y.Text)
                    End If

                    If (iPos >= 1) Then                                 ' ��1��_/��2��_��è��ݸޏI�� ?
                        If (GrpNum <= 1) Then                           ' �O���[�v�� <= 1 ? 
                            r = cFRS_ERR_START                          ' Return�l = OK(START SW)���� 
                        Else
                            r = cFRS_NORMAL                             ' Return�l = �O���[�v�ԃC���^�[�o�������� 
                        End If
                        r = cFRS_ERR_START                              'V6.1.4.9�A
                        Exit Do
                    Else
                        If (stJOG.Flg = -1) Then
                            iPos = 1                                    ' iPos = ��2��_��è��ݸޏ���
                        End If
                    End If

                    ' HALT SW�������͂P�O�̃e�B�[�`���O�֖߂�
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW���� ?
                    If (iPos = 0) Then                                  ' ��1��_�̃e�B�[�`���O�Ȃ珈�����s

                    Else                                                ' ��2��_�Ȃ��1��_�̃e�B�[�`���O��
                        ' ��2��_�̍��W�X�V 'V3.0.0.0�B���@HALT�L�[�ő�P��_�ɖ߂��Ă���OK������ꍇ�ׂ̈ɑ�Q��_�̒l���i�[���Ă����B
                        If (typPlateInfo.intResistDir = 0) Then             ' ���ߕ��т�X���� ?
                            If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                                CSPoint(iPos) = stJOG.PosX                  ' CSPoint(0:��1��_�ʒu, 1:��2��_�ʒu)�ɐݒ肷��
                            Else                                            ' �s�x�e�B�[�`���O��
                                CSPoint(iPos) = stJOG.PosY
                            End If
                        Else
                            If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                                CSPoint(iPos) = stJOG.PosY                  ' CSPoint(0:��1��_�ʒu, 1:��2��_�ʒu)�ɐݒ肷��
                            Else                                            ' �s�x�e�B�[�`���O��
                                CSPoint(iPos) = stJOG.PosX
                            End If
                        End If
                        StepOffSetX(1) = Double.Parse(TxtPos2X.Text)
                        StepOffSetY(1) = Double.Parse(TxtPos2Y.Text)
                        ' ��2��_�̍��W�X�V 'V3.0.0.0�B��
                        iPos = 0                                        ' iPos = ��1��_
                    End If

                    '  RESET SW�������͏I���m�F���b�Z�[�W�\����
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW���� ?
                    Exit Do
                End If

                Call ZCONRST()                                          ' �ݿ�ٷ�ׯ����� 
            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK/Cancel���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            '-------------------------------------------------------------------
            '   �`�b�v�T�C�Y���Z�o����
            '-------------------------------------------------------------------
            'V3.0.0.0�B            If ((r <> cFRS_ERR_RST) And (iPos >= 1)) Then               ' OK(START��)�����ő�2��_��è��ݸޏI�� ?
            If ((r <> cFRS_ERR_RST) And (Me.cmdOK.Enabled = True)) Then ' OK(START��)�����ő�2��_��è��ݸޏI�� ?
                '  �s�w�e�B�[�`���O�� �� �`�b�v�T�C�Y = (��2��_-��1��_) / (�`�b�v��-1)) 
                '  �s�x�e�B�[�`���O�� �� �`�b�v�T�C�Y = (��2��_-��1��_) / (�u���b�N��-1)) 
                dblSaveTXChipsizeRelation = CSPoint(1) - CSPoint(0)                ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
                WkChipSize = System.Math.Abs(CDbl((CSPoint(1) - CSPoint(0)) / (RnBn - 1)))
                'V6.1.4.9�A��
                If gTkyKnd = KND_NET And (giAppMode = APP_MODE_TX) Then                       'V4.5.1.1�@
                    WkChipSize = System.Math.Abs(CDbl((CSPoint(1) - CSPoint(0)) / CDbl(RnBn * (GrpNum - 1))))
                End If                                          'V4.5.1.1�@
                'V6.1.4.9�A��
                strMSG = WkChipSize.ToString("0.0000")
                WkChipSize = Double.Parse(strMSG)
                Call DispChipSize(WkChipSize)                           ' �X�V�O/�X�V��̃`�b�v�T�C�Y��\������
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W�\�� 
            '-------------------------------------------------------------------
            ' �I���m�F���b�Z�[�W�\�� 
            If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESET��)������ ?
                ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@
                If (giAppMode = APP_MODE_TX) Then                           ' TX ? 
                    rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TX)
                Else
                    rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TY)
                End If
            ElseIf (r = cFRS_ERR_START) Then                            ' OK�{�^��������
                Call Disp_TxTyMsgBox(rtn)                               ' �I���m�F���b�Z�[�W�\�� 
            Else                                                        ' START SW�����ő�1��_/��2��_��è��ݸޏI������
                GoTo STP_END                                            ' �I���m�F���b�Z�[�W�\���͕\�������`�b�v�T�C�Y�X�V�㏈���p����
            End If

            ' Cancel(RESET��)�������͏������p������
            If (rtn = cFRS_ERR_RST) Then
                Me.LblResult_0.Text = ""                                ' �␳�䗦
                Me.LblResult_1.Text = ""                                ' �`�b�v�T�C�Y
                Me.LblResult_2.Text = ""                                ' �`�b�v�T�C�Y(�␳��)
                GoTo STP_RETRY                                          ' �����p����
            End If

            ' TX2(Teach)�܂���TY2����
            If (rtn = cFRS_TxTy) Then
                r = rtn
            End If

STP_END:
            ' �`�b�v�T�C�Y�X�V
            If (r <> cFRS_ERR_RST) Then                                 ' Cancel(RESET��)�����ȊO ?
                ChipSize = WkChipSize                                   ' �`�b�v�T�C�Y�X�V
            End If
            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.Sub_Jog1() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�a�o�O���[�v(�T�[�L�b�g)�Ԋu(TX��)�܂��̓X�e�[�W�O���[�v�Ԋu(TY��)�̃e�B�[�`���O����"
    '''=========================================================================
    ''' <summary>�a�o�O���[�v(�T�[�L�b�g)�Ԋu(TX��)�܂��̓X�e�[�W�O���[�v�Ԋu(TY��)�̃e�B�[�`���O����</summary>
    ''' <param name="RnBn">    (INP)�`�b�v��(��R��)(TX��), �u���b�N��(TY��)</param>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <param name="KJPosX">  (INP)�擪�u���b�N�̐擪��ʒuX</param>
    ''' <param name="KJPosY">  (INP)�擪�u���b�N�̐擪��ʒuY</param>
    ''' <param name="GrpNum">  (INP)�O���[�v��</param>
    ''' <param name="GrpCnt">  (I/O)�O���[�v���J�E���^</param>
    ''' <param name="CSPoint"> (OUT)�`�b�v�T�C�Y�Z�o�p(0:��1��_, 1:��2��_)</param>
    ''' <returns>cFRS_ERR_START = OK(START��)����
    '''          cFRS_ERR_RST   = Cancel(RESET��)����
    '''          cFRS_ERR_HALT  = HALT������(�`�b�v�T�C�Y�e�B�[�`���O������)
    '''          cFRS_NORMAL    = ����(�X�e�b�v�I�t�Z�b�g�ʂ̎Z�o��)
    '''          -1�ȉ�         = �G���[</returns>
    ''' <remarks>�C���^�[�o���l��mdStepInterval()�ɐݒ�</remarks>
    '''=========================================================================
    Private Function Sub_Jog2(ByVal RnBn As Short, ByVal ChipSize As Double, ByVal KJPosX As Double, ByVal KJPosY As Double, _
                              ByVal GrpNum As Short, ByRef GrpCnt As Short, ByRef CSPoint() As Double) As Short

        Dim PosX As Double                                              ' ���݂�BP�ʒuX/ð��وړ����WX(��Βl)
        Dim PosY As Double                                              ' ���݂�BP�ʒuY/ð��وړ����WY(��Βl)
        Dim iPos As Short
        Dim iFlg As Short                                               ' iFlg:2=��n��ٰ��,�ŏI��ʒu��è��ݸ�, 3=��n+1��ٰ��,�擪��ʒu��è��ݸ� 
        Dim r As Short
        Dim rtn As Short                                                ' �ߒl 
        Dim strMSG As String

        Try
            ' �O���[�v���1��ȉ��Ȃ�NOP
            If (GrpNum <= 1) Then
                Return (cFRS_ERR_START)
            End If

            ' ��������
            GrpCnt = 1                                                  ' �O���[�v���J�E���^������
            iFlg = 2                                                    ' iFlg = ��n��ٰ��,�ŏI��ʒu��è��ݸ�
            Me.GrpFram_1.Visible = True                                 ' ����_�\��(Sub_Jog3()�Ŕ�\���ɂ��邽��)
            stJOG.Flg = -1                                              ' �e��ʂ�OK/Cancel���݉����׸�
            Me.TxtPosX.Text = ""                                        ' ���W�\��(�ر)��1��_XY
            Me.TxtPosY.Text = ""
            Me.TxtPos2X.Text = ""                                       ' ���W�\��(�ر)��2��_XY
            Me.TxtPos2Y.Text = ""
            Call ClearGridView(RowAry)                                  ' �f�[�^�O���b�h�r���[�̃C���^�[�o���\�����N���A����

            If (giAppMode = APP_MODE_TX) Then                           ' �s�w�e�B�[�`���O ? 
                GrpNum = 2                                              ' BP�O���[�v��(�T�[�L�b�g��)�͂Q�Ƃ��ď������� 

            Else                                                        ' �s�x�e�B�[�`���O��
                GrpNum = 2                                              ' �X�e�[�W�O���[�v���͂Q�Ƃ��ď�������
                If (typPlateInfo.intResistDir = 0) Then                 ' �`�b�v���т�X���� ?
                    RnBn = typPlateInfo.intBlkCntInStgGrpY              ' �u���b�N�� = �X�e�[�W�O���[�v���u���b�N��Y 
                Else
                    RnBn = typPlateInfo.intBlkCntInStgGrpX              ' �u���b�N�� = �X�e�[�W�O���[�v���u���b�N��X
                End If
            End If

            ' �^�C�g�����b�Z�[�W�̐ݒ� 
            If (giAppMode = APP_MODE_TX) Then                           ' �s�w�e�B�[�`���O��
                If (gTkyKnd = KND_CHIP) Then
                    Me.lblTitle2.Text = INFO_MSG23                      ' �^�C�g�� = "�a�o�O���[�v�Ԋu�e�B�[�`���O"
                Else
                    Me.lblTitle2.Text = INFO_MSG30                      ' �^�C�g�� = "�T�[�L�b�g�Ԋu�e�B�[�`���O"
                End If
            Else                                                        ' �s�x�e�B�[�`���O��
                Me.lblTitle2.Text = INFO_MSG14                          ' �^�C�g�� = "�X�e�[�W�O���[�v�Ԋu�e�B�[�`���O"
            End If

            ' �O���[�v�����܂��̓X�e�[�W�O���[�v�����̃C���^�[�o�����e�B�[�`���O
            Timer1.Enabled = True                                       ' ###228
            Do
                '�O���[�v��(�T�[�L�b�g��)���܂��̓X�e�b�v�����̏��������H
                If (GrpCnt >= GrpNum) Then                              ' �C���^�[�o���e�B�[�`���O�I�� ?
                    ' �s�x�e�B�[�`���O���Ńv���[�g�f�[�^�̃X�e�b�v�I�t�Z�b�g��X,Y�w��Ȃ��̏ꍇ�̓X�e�b�v�I�t�Z�b�g�ʂ̎Z�o������
                    If ((giAppMode = APP_MODE_TY) And (typPlateInfo.dblStepOffsetXDir = 0) And (typPlateInfo.dblStepOffsetYDir = 0)) Then
                        r = cFRS_NORMAL                                 ' Return�l = �X�e�b�v�I�t�Z�b�g�ʂ̎Z�o������ 
                    Else
                        r = cFRS_ERR_START                              ' Return�l = OK(START SW)���� 
                    End If
                    Exit Do
                End If

STP_RETRY:
                ' ��(GrpCnt)��ٰ��,�ŏI��_��è��ݸ�
                If (iFlg = 2) Then                                      ' iFlg(2=��n��ٰ��,�ŏI��ʒu��è��ݸ�)?
                    '"��" n "�O���[�v�A�ŏI�[�ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
                    lblInfo.Text = INFO_MSG19 & GrpCnt & INFO_MSG28 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPosX                               ' ��1��_���WX�ʒu�\���p÷���ޯ��
                    stJOG.TextY = TxtPosY                               ' ��1��_���WY�ʒu�\���p÷���ޯ��
                    iPos = 2 * GrpCnt
                Else
                    '"��" n+1 "�O���[�v�A�Ő�[�ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]" "
                    lblInfo.Text = INFO_MSG19 & GrpCnt + 1 & INFO_MSG29 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17
                    stJOG.TextX = TxtPos2X                              ' ��2��_���WX�ʒu�\���p÷���ޯ��
                    stJOG.TextY = TxtPos2Y                              ' ��2��_���WY�ʒu�\���p÷���ޯ��
                    iPos = (2 * GrpCnt) + 1
                End If

                ' BP�ړ��܂���XY�e�[�u���ړ�(��[GrpCnt]��ٰ��,�ŏI��ʒu/��[GrpCnt+1]��ٰ��,�擪��ʒu)
                If (giAppMode = APP_MODE_TX) Then                       ' �s�w�e�B�[�`���O��
                    ' BP���[GrpCnt]��ٰ�߂̍ŏI��ʒu�܂��͑�[GrpCnt+1]�̐擪��ʒu�Ɉړ�����
                    r = XYBPMoveSetBlock(iFlg, iPos, GrpCnt, RnBn, ChipSize, KJPosX, KJPosY, PosX, PosY, GrpNum)     'V6.1.4.9�A�@�T�[�L�b�g��GrpNum�ǉ�
                Else                                                    ' �s�x�e�B�[�`���O��
                    ' XY�e�[�u�����[GrpCnt]��ٰ�߂̍ŏI��ʒu�܂��͑�[GrpCnt+1]�̐擪��ʒu�Ɉړ�����
                    r = XYTableMoveSetBlock(iFlg, iPos, GrpCnt, RnBn, ChipSize, KJPosX, KJPosY, PosX, PosY)
                End If
                If (r <> cFRS_NORMAL) Then
                    Return (r)                                          ' �G���[���^�[��
                End If

                ' �e�B�[�`���O����
                stJOG.PosX = PosX                                       ' BP X�܂���XYð��� X��Έʒu
                stJOG.PosY = PosY                                       ' BP Y�܂���XYð��� X��Έʒu
                stJOG.Flg = -1                                          ' �e��ʂ�OK/Cancel���݉����׸�
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�I��
                    Return (r)
                End If

                ' �ŏI��ʒu/�擪��ʒu�̐�Βl���W�X�V
                ZRPosX(iPos) = ZRPosX(iPos) + stJOG.cgX
                ZRPosY(iPos) = ZRPosY(iPos) + stJOG.cgY
                ' V1.16.0.1�@ ADD START-----------------------------------------------------
                If (iPos = (2 * GrpCnt)) Then                           ' ��1��_ ? 
                    If (typPlateInfo.intResistDir = 0) Then             ' ���ߕ��т�X���� ?
                        If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                            ZRPosY((2 * GrpCnt) + 1) = ZRPosY(iPos)
                        End If
                    Else
                        If (giAppMode = APP_MODE_TX) Then               ' �s�w�e�B�[�`���O��
                            ZRPosX((2 * GrpCnt) + 1) = ZRPosX(iPos)
                        End If
                    End If
                End If
                ' V1.16.0.1�@ ADD END  -----------------------------------------------------

                ' �R���\�[���L�[�`�F�b�N
                If (r = cFRS_ERR_START) Then                            ' START SW���� ?
                    ' �C���^�[�o���l�Z�o
                    If (iFlg = 3) Then                                  ' ��n+1��ٰ��,�擪��ʒu��è��ݸ� ?
                        ' ��n+1��ٰ��,�擪��ʒu�̍��W�X�V
                        If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��т�X���� ?
                            If (giAppMode = APP_MODE_TX) Then           ' �s�w�e�B�[�`���O��
                                CSPoint(1) = stJOG.PosX                 ' CSPoint(1:��n+1��ٰ��,�擪��ʒu)�ɐݒ肷��
                            Else                                        ' �s�x�e�B�[�`���O��
                                CSPoint(1) = stJOG.PosY
                            End If
                        Else
                            If (giAppMode = APP_MODE_TX) Then           ' �s�w�e�B�[�`���O��
                                CSPoint(1) = stJOG.PosY
                            Else                                        ' �s�x�e�B�[�`���O��
                                CSPoint(1) = stJOG.PosX                 ' CSPoint(1:��n+1��ٰ��,�擪��ʒu)�ɐݒ肷��
                            End If
                        End If
                        ' �C���^�[�o���l���Z�o (������ْl = ��2��_-��1��_)
                        '                        mdStepInterval(GrpCnt) = CDbl(CSPoint(1) - CSPoint(0))
                        'V4.5.1.0�L                        mdStepInterval(1) = CDbl(CSPoint(1) - CSPoint(0))
                        mdStepInterval(1) = CDbl(CSPoint(1) - CSPoint(0) - ChipSize)   'V4.5.1.0�L
                        'V6.1.4.9�A��
                        If gTkyKnd = KND_NET Then
                            mdStepInterval(1) = CDbl(CSPoint(1) - CSPoint(0) - (ChipSize * RnBn))
                        End If
                        'V6.1.4.9�A��
                        iFlg = 2                                        ' iFlg = ��n��ٰ�߁A�ŏI��ʒu
                        GrpCnt = GrpCnt + 1                             ' �O���[�vߐ��J�E���^�X�V
                        If (GrpCnt < GrpNum) Then                       ' �ŏI�O���[�v�łȂ� ?
                            ' ���W�\��(�ر)
                            Me.TxtPosX.Text = ""                        ' ��1��_XY
                            Me.TxtPosY.Text = ""
                            Me.TxtPos2X.Text = ""                       ' ��2��_XY
                            Me.TxtPos2Y.Text = ""
                        End If
                        Exit Do                                         '   ###121

                    Else
                        ' ��n��ٰ��,�ŏI��ʒu�̍��W�X�V
                        If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��т�X���� ?
                            If (giAppMode = APP_MODE_TX) Then           ' �s�w�e�B�[�`���O��
                                CSPoint(0) = stJOG.PosX                 ' CSPoint(0:��n��ٰ��,�ŏI��ʒu)�ɐݒ肷��
                            Else                                        ' �s�x�e�B�[�`���O��
                                CSPoint(0) = stJOG.PosY
                            End If
                        Else
                            If (giAppMode = APP_MODE_TX) Then           ' �s�w�e�B�[�`���O��
                                CSPoint(0) = stJOG.PosY                 ' CSPoint(0:��n��ٰ��,�ŏI��ʒu)�ɐݒ肷��
                            Else                                        ' �s�x�e�B�[�`���O��
                                CSPoint(0) = stJOG.PosX
                            End If
                        End If
                        iFlg = 3                                        ' ��n+1��ٰ��,�擪��ʒu
                    End If

                    ' HALT SW�������͂P�O�̃e�B�[�`���O�֖߂�
                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW���� ?
                    If (iFlg = 2) Then                                  ' ��n��ٰ�ߍŏI��ʒu�̃e�B�[�`���O ?
                        If (GrpCnt <= 1) Then                           ' �ŏ��̃O���[�v�Ȃ�
                            Return (r)                                  ' �`�b�v�T�C�Y�e�B�[�`���O�����֖߂�
                        Else
                            GrpCnt = GrpCnt - 1                         ' �O���[�v�J�E���^ -= 1 
                            iFlg = 3                                    ' iFlg = ��n+1��ٰ��,�擪��ʒu��è��ݸނ�
                        End If
                    Else                                                ' ��n+1��ٰ�ߐ擪��ʒu�̃e�B�[�`���O
                        If (GrpCnt <= 1) Then
                            iFlg = 2                                    ' iFlg = ��n��ٰ��,�ŏI��ʒu��è��ݸނ�
                        End If
                    End If

                    '  RESET SW�������͏I���m�F���b�Z�[�W�\����
                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW���� ?
                    Exit Do
                End If
            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK/Cancel���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            '-------------------------------------------------------------------
            '   �O���[�v�ԃC���^�[�o���l(�␳�O/�␳��)��\������
            '-------------------------------------------------------------------
            If (r <> cFRS_ERR_RST) Then                                 ' OK(START��)�����ő�2��_��è��ݸޏI�� ?
                Call DispGridView(RowAry, ChipSize)                     ' �O���[�v�ԃC���^�[�o���l(�␳�O/�␳��)��\������
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W�\�� 
            '-------------------------------------------------------------------
            ' �I���m�F���b�Z�[�W�\�� 
            If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESET��)������ ?
                ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@
                If (giAppMode = APP_MODE_TX) Then                           ' TX ? 
                    rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TX)
                Else
                    rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TY)
                End If
            ElseIf (r = cFRS_ERR_START) Then                            ' OK�{�^��������
                Call Disp_TxTyMsgBox(rtn)                               ' �I���m�F���b�Z�[�W�\�� 
            Else                                                        ' START SW�����ő�1��_/��2��_��è��ݸޏI������
                Return (r)                                              ' �I���m�F���b�Z�[�W�\���͕\�����������p����
            End If

            ' Cancel(RESET��)�������͏������p������                     ' ###080��
            If (rtn = cFRS_ERR_RST) Then

                'If (GrpCnt >= GrpNum) Then                              ' �C���^�[�o���e�B�[�`���O�I�� ?
                '    GrpCnt = GrpCnt - 1                                 ' �ŏI�O���[�v�ԍ��ɂ��� 
                '    iFlg = 3                                            ' ��n+1��ٰ��,�擪��ʒu
                'End If

                If (GrpCnt < GrpNum) Then                               ' ###080 �C���^�[�o���e�B�[�`���O�I�� ?
                    iFlg = 2                                            ' ��n+1��ٰ��,�擪��ʒu
                Else
                    GrpCnt = GrpCnt - 1                                 ' �ŏI�O���[�v�ԍ��ɂ��� 
                    iFlg = 3
                End If

                GoTo STP_RETRY                                          ' �����p����
            End If                                                      ' ###080��

            ' TX2(Teach)�܂���TY2����
            If (rtn = cFRS_TxTy) Then
                r = rtn
            End If

            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.Sub_Jog2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�X�e�b�v�I�t�Z�b�g�ʂ̎Z�o(�s�x�e�B�[�`���O��)"
    '''=========================================================================
    ''' <summary>�X�e�b�v�I�t�Z�b�g�ʂ̎Z�o(�s�x�e�B�[�`���O��)</summary>
    ''' <param name="RnBn">     (INP)�u���b�N��</param>
    ''' <param name="ChipSize"> (INP)�`�b�v�T�C�Y</param>
    ''' <param name="KJPosX">   (INP)�擪�u���b�N�̐擪��ʒuX</param>
    ''' <param name="KJPosY">   (INP)�擪�u���b�N�̐擪��ʒuY</param>
    ''' <param name="GrpNum">   (INP)��ٰ�ߐ�</param>
    ''' <param name="GrpCnt">   (INP)��ٰ�ߐ�����</param>
    ''' <param name="CSPoint">  (OUT)���߻��ގZ�o�p(0:��1��_, 1:��2��_)</param>
    ''' <param name="dStepOffx">(INP)�ï�ߵ̾�ė�X</param>
    ''' <param name="dStepOffy">(INP)�ï�ߵ̾�ė�Y</param>
    ''' <returns>cFRS_ERR_ADV  = OK(START��)
    '''          cFRS_ERR_RST  = Cancel(RESET��)
    '''          cFRS_ERR_HALT = HALT������(�X�e�[�W�O���[�v�Ԋu�̃e�B�[�`���O������)
    '''          -1�ȉ�        = �G���[</returns>
    ''' <remarks>�ETY�e�B�[�`���O�ŏI�u���b�N�e�B�[�`���O����X�����̂���ʂ��X�e�b�v�I�t�Z�b�g�ƂȂ�
    '''          �E�v���[�g�f�[�^�̃X�e�b�v�I�t�Z�b�g��XY���0��̎��̂ݏ������s��
    ''' </remarks>
    '''=========================================================================
    Private Function Sub_Jog3(ByVal RnBn As Short, ByVal ChipSize As Double, ByVal KJPosX As Double, ByVal KJPosY As Double, _
                              ByVal GrpNum As Short, ByVal GrpCnt As Short, ByRef CSPoint() As Double, ByVal dStepOffx As Double, ByVal dStepOffy As Double) As Short

        Dim PosX As Double                                              ' ���݂�BP�ʒuX/ð��وړ����WX(��Βl)
        Dim PosY As Double                                              ' ���݂�BP�ʒuY/ð��وړ����WY(��Βl)
        Dim iPos As Short
        Dim iFlg As Short                                               ' iFlg:2=��n��ٰ��,�ŏI��ʒu��è��ݸ�, 3=��n+1��ٰ��,�擪��ʒu��è��ݸ� 
        Dim r As Short
        Dim rtn As Short                                                ' �ߒl 
        Dim strMSG As String

        Try
            ' �s�x�e�B�[�`���O�łȂ����NOP
            If (giAppMode <> APP_MODE_TY) Then
                Return (cFRS_ERR_START)
            End If

            ' ��ٰ�ߐ��1��ȉ��Ȃ�NOP
            If (GrpNum <= 1) Then
                Return (cFRS_ERR_START)
            End If

            ' �v���[�g�f�[�^�̃X�e�b�v�I�t�Z�b�g��XY���0��Ȃ珈������
            If (dStepOffx <> 0) And (dStepOffy <> 0) Then
                Return (cFRS_ERR_START)
            End If

            ' �ŏI��ٰ�ߍŏI��_��è��ݸ�
            Me.lblTitle2.Text = INFO_MSG15                              '"�X�e�b�v�I�t�Z�b�g�ʁ@�e�B�[�`���O"
            iFlg = 4                                                    ' iFlg = ��n��ٰ��,�ŏI�u���b�N��ʒu��è��ݸ�
            iPos = 2 * GrpCnt
            ' XY�e�[�u���ړ�(�ŏI��ٰ�߁A�ŏI��ʒu)
            r = XYTableMoveSetBlock(iFlg, iPos, GrpCnt, RnBn, ChipSize, KJPosX, KJPosY, PosX, PosY)
            ' ���W�\��
            Me.TxtPosX.Text = ""                                        ' ��1��_XY
            Me.TxtPosY.Text = ""

            Me.GrpFram_1.Visible = False                                ' ����_��\�� 
            '"�X�e�b�v�I�t�Z�b�g�ʒu�̃e�B�[�`���O"+"��ʒu�����킹�ĉ������B"+"�ړ�:[���]  ����:[START]  ���f:[RESET]"
            lblInfo.Text = INFO_MSG31 & vbCrLf & INFO_MSG16 & vbCrLf & INFO_MSG17

STP_RETRY:
            ' �e�B�[�`���O����
            Do
                stJOG.PosX = PosX                                       ' XYð��� X��Έʒu
                stJOG.PosY = PosY                                       ' XYð��� Y��Έʒu
                stJOG.TextX = TxtPosX                                   ' XYð��� X�ʒu�\���p÷���ޯ��
                stJOG.TextY = TxtPosY                                   ' XYð��� Y�ʒu�\���p÷���ޯ��
                stJOG.Flg = -1                                          ' �e��ʂ�OK/Cancel���݉����׸�
                'V6.0.0.0�J                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                r = JogEzMove(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval, Me)  'V6.0.0.0�J
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�I��
                    Return (r)
                End If

                ' �ŏI�O���[�v�A�ŏI��R�ʒu�̐�Βl���W�X�V
                ZRPosX(iPos) = ZRPosX(iPos) + stJOG.cgX
                ZRPosY(iPos) = ZRPosY(iPos) + stJOG.cgY

                ' �R���\�[���L�[�`�F�b�N
                If (r = cFRS_ERR_START) Then                            ' START SW���� ?
                    ' �ŏI�O���[�v�A�ŏI��R�ʒu�̍��W�X�V
                    If (typPlateInfo.intResistDir = 0) Then             ' ���ߕ��т�X���� ?
                        CSPoint(1) = stJOG.PosY
                    Else
                        CSPoint(1) = stJOG.PosX
                    End If

                    ' �X�e�b�v�I�t�Z�b�g�ʒu�X�V(��2��_) ###091
                    StepOffSetX(1) = Double.Parse(TxtPosX.Text)
                    StepOffSetY(1) = Double.Parse(TxtPosY.Text)

                    Exit Do

                ElseIf (r = cFRS_ERR_HALT) Then                         ' HALT SW���� ?
                    Return (r)                                          ' �X�e�[�W�O���[�v�Ԋu�̃e�B�[�`���O�����֖߂�

                ElseIf (r = cFRS_ERR_RST) Then                          ' RESET SW���� ?
                    Exit Do
                End If

            Loop While (stJOG.Flg = -1)

            ' ����ʂ���OK/Cancel���݉����Ȃ�r�ɖߒl��ݒ肷��
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            '-------------------------------------------------------------------
            '   �I���m�F���b�Z�[�W�\�� 
            '-------------------------------------------------------------------
            ' �I���m�F���b�Z�[�W�\�� 
            If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESET��)������ ?
                ' "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@
                If (giAppMode = APP_MODE_TX) Then                           ' TX ? 
                    rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TX)
                Else
                    rtn = Form1.System1.TrmMsgBox(gSysPrm, MSG_105, MsgBoxStyle.OkCancel, TITLE_TY)
                End If
            ElseIf (r = cFRS_ERR_START) Then                            ' OK�{�^��������
                Call Disp_TxTyMsgBox(rtn)                               ' �I���m�F���b�Z�[�W�\�� 
            Else                                                        ' START SW�����ő�1��_/��2��_��è��ݸޏI������
                Return (r)                                              ' �I���m�F���b�Z�[�W�\���͕\�����������p����
            End If

            ' Cancel(RESET��)�������͏������p������
            If (rtn = cFRS_ERR_RST) Then
                If (GrpCnt >= GrpNum) Then                              ' �C���^�[�o���e�B�[�`���O�I�� ?
                    GrpCnt = GrpCnt - 1                                 ' �ŏI�O���[�v�ԍ��ɂ��� 
                    iFlg = 3                                            ' ��n+1��ٰ��,�擪��ʒu
                End If
                GoTo STP_RETRY                                          ' �����p����
            End If

            ' TX2(Teach)�܂���TY2����
            If (rtn = cFRS_TxTy) Then
                r = rtn
            End If

            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.Sub_Jog3() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

    '=========================================================================
    '   �g���~���O�p�����[�^�̎擾/�X�V����
    '=========================================================================
#Region "�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����(�s�w/�s�x�e�B�[�`���O)"
    '''=========================================================================
    '''<summary>�g���~���O�f�[�^���K�v�ȃp�����[�^���擾����</summary>
    '''<remarks>�e�C���^�[�o���f�[�^���擾����</remarks>
    '''=========================================================================
    Private Sub SetTrimData()

        Dim mdCSx As Double                                                     ' �`�b�v�T�C�YX
        Dim mdCSy As Double                                                     ' �`�b�v�T�C�YY
        Dim i As Short
        Dim strMSG As String

        Try
            '---------------------------------------------------------------
            '   �`�b�v�T�C�YX,Y���擾����
            '---------------------------------------------------------------
            mdCSx = typPlateInfo.dblChipSizeXDir                                ' �`�b�v�T�C�YX,Y(CHIP/NET����)
            mdCSy = typPlateInfo.dblChipSizeYDir
            mdStepInterval(0) = 0.0#

            '---------------------------------------------------------------
            '   �e�C���^�[�o���f�[�^���擾����(�s�w�e�B�[�`���O��)
            '---------------------------------------------------------------
            ' �v���[�g�f�[�^��BP�O���[�v�Ԋu(�T�[�L�b�g�Ԋu)���擾����
            If (giAppMode = APP_MODE_TX) Then                                   ' �s�x�e�B�[�`���O ?
                For i = 1 To MaxCntStep
                    miStepBlock(i) = typPlateInfo.intResistCntInGroup           ' 1�O���[�v(1�T�[�L�b�g)����R��
                    mdStepInterval(i) = typPlateInfo.dblBpGrpItv                ' BP�O���[�v(�T�[�L�b�g)�Ԋu
                Next i
            End If

            '---------------------------------------------------------------
            '   �e�C���^�[�o���f�[�^���擾����(�s�x�e�B�[�`���O��)
            '---------------------------------------------------------------
            ' �v���[�g�f�[�^����u���b�N���ƃC���^�[�o���l���擾����(�X�e�b�v�f�[�^�\���͖̂��g�p)
            If (giAppMode = APP_MODE_TY) Then                                   ' �s�x�e�B�[�`���O ?
                For i = 1 To MaxCntStep
                    If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���ѕ��� = X���� ?
                        miStepBlock(i) = typPlateInfo.intBlockCntYDir           ' �u���b�N��Y
                        mdStepInterval(i) = typPlateInfo.dblStgGrpItvY          ' Y�����X�e�[�W�O���[�v�Ԋu
                    Else
                        miStepBlock(i) = typPlateInfo.intBlockCntXDir           ' �u���b�N��X
                        mdStepInterval(i) = typPlateInfo.dblStgGrpItvX          ' X�����X�e�[�W�O���[�v�Ԋu
                    End If
                Next i
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTeach.SetTrimData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�p�����[�^�X�V(�s�w/�s�x�e�B�[�`���O)"
    '''=========================================================================
    '''<summary>�g���~���O�p�����[�^�X�V</summary>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <param name="GrpCnt">  (INP)��ٰ�ߐ�����</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimParameter(ByVal ChipSize As Double, ByVal GrpCnt As Short)

        Dim OffSet As Double = 0.0                                      ' ###091
        Dim strMSG As String
        Dim dBeforeChipSize As Double                                   'V4.5.1.0�L
        Dim dblBeforeOffSet As Double                                   'V4.5.1.0�N

        Try
            '---------------------------------------------------------------
            '   �g���~���O�p�����[�^�X�V(�s�x�e�B�[�`���O��)
            '---------------------------------------------------------------
            If (giAppMode = APP_MODE_TY) Then
                Call SetTrimParamToGlobalArea_TY(ChipSize, GrpCnt)
                Return
            End If

            '---------------------------------------------------------------
            '   �g���~���O�p�����[�^�X�V(�s�w�e�B�[�`���O��)
            '---------------------------------------------------------------
            ' �`�b�v�T�C�Y���X�V����
            If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���ѕ��� = X���� ?
                dBeforeChipSize = typPlateInfo.dblChipSizeXDir          'V4.5.1.0�L
                typPlateInfo.dblChipSizeXDir = ChipSize
            Else
                dBeforeChipSize = typPlateInfo.dblChipSizeYDir          'V4.5.1.0�L
                typPlateInfo.dblChipSizeYDir = ChipSize
            End If

            ' �C���^�[�o���f�[�^���X�V����
            If (RowAry Is Nothing = False) Then
                dblSaveBpGrpItv = typPlateInfo.dblBpGrpItv              ' �O�̃C���^�[�o���̕ۑ� 'V4.5.1.0�N
                typPlateInfo.dblBpGrpItv = mdStepInterval(1)            ' BP�O���[�v(�T�[�L�b�g)�Ԋu
            End If

            dblBeforeOffSet = typPlateInfo.dblTXChipsizeRelationY       'V4.5.1.0�N�O��̇�Y��ۑ�

            ' �X�e�b�v�I�t�Z�b�g��(BP�I�t�Z�b�g��)�����߂� ###091
            ' (TX�e�B�[�`���O�ŏI�u���b�N�e�B�[�`���O����Y�����̂���ʂ��I�t�Z�b�g�ƂȂ�)
            If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���ѕ��� = X���� ?
                OffSet = StepOffSetY(1) - StepOffSetY(0)                ' �I�t�Z�b�g�ʒuY(0:��1��_�@1:��2��_)
                'V4.5.1.0�N��
                typPlateInfo.dblTXChipsizeRelationX = dblSaveTXChipsizeRelation         ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
                typPlateInfo.dblTXChipsizeRelationY = StepOffSetY(1) - StepOffSetY(0)   ' �␳�ʒu�P�ƂQ�̑��Βl�x 'V4.5.1.0�N
                'V4.5.1.0�N��
            Else
                OffSet = StepOffSetX(1) - StepOffSetX(0)                ' �I�t�Z�b�g�ʒuX(0:��1��_�@1:��2��_)
                'V4.5.1.0�N��
                typPlateInfo.dblTXChipsizeRelationX = StepOffSetX(1) - StepOffSetX(0)   ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
                typPlateInfo.dblTXChipsizeRelationY = dblSaveTXChipsizeRelation         ' �␳�ʒu�P�ƂQ�̑��Βl�x 'V4.5.1.0�N
                'V4.5.1.0�N��
            End If

            ' �g���~���O�p�����[�^(�J�b�g�ʒuXY, �u���b�N�T�C�Y)���X�V����
            'Call SetTrimParamToGlobalArea(ChipSize)                    '###090
            'V4.5.1.0�L            Call SetTrimParamToGlobalArea(ChipSize, OffSet)             '###090
            Call SetTrimParamToGlobalArea(dBeforeChipSize, ChipSize, OffSet, dblBeforeOffSet)         '###090 'V4.5.1.0�L dBeforeChipSize ADD

            Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.SetTrimParameter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�p�����[�^(�J�b�g�ʒuXY, �u���b�N�T�C�Y)�X�V(�s�w�e�B�[�`���O)�yCHIP/NET�p�z"
    '''=========================================================================
    ''' <summary>�g���~���O�p�����[�^(�J�b�g�ʒuXY, �u���b�N�T�C�Y)(�s�w�e�B�[�`���O)�yCHIP/NET�p�z</summary>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <param name="OffSet">  (INP)�X�e�b�v�I�t�Z�b�g��(BP�I�t�Z�b�g��) ###090</param>
    ''' <returns>0=����, 0�ȊO:�G���[</returns>
    '''=========================================================================
    Private Function SetTrimParamToGlobalArea(ByVal BeforChipSize As Double, ByVal ChipSize As Double, ByVal OffSet As Double, ByVal dblBeforeOffSet As Double) As Short 'V4.5.1.0�L ADD BeforChipSize ,dblBeforeOffSet
        'Private Function SetTrimParamToGlobalArea(ByVal ChipSize As Double) As Short

        'V6.1.4.9�A        Dim dTeachPos(MaxCntCut) As Double
        'V6.1.4.9�A        Dim dStartPos(MaxCntCut) As Double
        Dim dTeachPos(256, MaxCntCut) As Double         'V6.1.4.9�A  �P�T�[�L�b�g���ő��R���́A256��
        Dim dStartPos(256, MaxCntCut) As Double         'V6.1.4.9�A  
        Dim dAddGrpInt As Double
        Dim iChpNum As Short
        Dim iChpCnt As Short
        Dim iCutNum As Short
        Dim iCutCnt As Short
        Dim strMSG As String
        Dim OffSetReg As Double                                         ' ###090
        Dim WkDbl As Double                                             ' ###264
        Dim cin As Integer                                              ' ###090
        Dim r As Integer                                                ' ###090
        Dim bOfsFlg As Boolean = False                                  ' ###090
        Dim DmyX As Double = 0.0                                        ' ###090
        Dim DmyY As Double = 0.0                                        ' ###090
        Dim mPIT As Double = 0.0                                        ' ###090
        Dim dDiffChipSize As Double = ChipSize - BeforChipSize          'V4.5.1.0�L
        Dim iCircuit As Integer                                         'V4.5.1.0�L
        Dim bCircuit As Boolean                                         'V4.5.1.0�L
        Dim iChipInCircuit As Integer                                   'V4.5.1.0�L �T�[�L�b�g���̒�R�ԍ�
        Dim iCirCuitCnt As Integer                                      'V6.1.4.9�A �u���b�N���̃T�[�L�b�g�ԍ�
        Dim iMarkingNo As Integer = 0                                   'V6.1.4.9�A
        'V6.1.4.9�A        Dim dDistance As Double                                         'V4.5.1.0�L

        Try
            ' ��������
            iCirCuitCnt = typPlateInfo.intResistCntInGroup              'V6.1.4.9�A
            'V4.5.1.0�L��
            ' �O���[�v�ԍ� = ��R�ԍ� / 1�O���[�v(1�T�[�L�b�g)����R��
            If (gTkyKnd = KND_NET) And typPlateInfo.intResistCntInGroup > 1 Then
                bCircuit = True
            Else
                bCircuit = False
            End If
            'V4.5.1.0�L��
            iChpNum = typPlateInfo.intResistCntInBlock                  ' �u���b�N����R��(�}�[�L���O�p��R�܂�)
            'V4.5.1.0�L��
            If bCircuit Then
                'V6.1.4.9�A                r = GetCutPosOffset(OffSet, typPlateInfo.intResistCntInGroup, OffSetReg, cin)        ' BP�I�t�Z�b�g�ʂ���P��R������̃I�t�Z�b�g�ʂ����߂�
                r = GetCutPosOffset(OffSet, typPlateInfo.intGroupCntInBlockXBp, OffSetReg, cin)        ' BP�I�t�Z�b�g�ʂ���P�T�[�L�b�g�T�C�Y������̃I�t�Z�b�g�ʂ����߂�
            Else
                'V4.5.1.0�L��
                r = GetCutPosOffset(OffSet, iChpNum, OffSetReg, cin)        ' BP�I�t�Z�b�g�ʂ���P��R������̃I�t�Z�b�g�ʂ����߂� ###090
            End If                                      'V4.5.1.0�L
            If (r = cFRS_NORMAL) Then bOfsFlg = True '                  ' bOfsFlg = BP�I�t�Z�b�g�̉��Z�����s���� ###090
            mPIT = 0.0                                                  ' �s�b�`������ ###090

            ' �g���~���O�p�����[�^(�J�b�g�ʒuXY)���X�V����
            For iChpCnt = 1 To iChpNum                                  ' �`�b�v��(��R��)���ݒ肷��
                'V6.1.4.9�A��
                If bCircuit Then
                    mPIT = OffSetReg * (typResistorInfoArray(iChpCnt).intCircuitGrp - 1)
                End If
                If iMarkingNo = 0 And typResistorInfoArray(iChpCnt).intResNo > 1000 Then
                    iMarkingNo = iChpCnt                                ' �}�[�L���O�f�[�^�̐擪
                End If
                'V6.1.4.9�A��
                iCutNum = typResistorInfoArray(iChpCnt).intCutCount     ' �J�b�g��
                'V4.5.1.0�L��
                iCircuit = iChpCnt \ typPlateInfo.intResistCntInGroup
                If ((iChpCnt Mod typPlateInfo.intResistCntInGroup) = 0) Then
                    iCircuit = iCircuit - 1                                         ' �]��0�Ȃ�O���[�v�ԍ�-1          
                End If
                iChipInCircuit = iChpCnt Mod typPlateInfo.intResistCntInGroup
                If (iChipInCircuit = 0) Then
                    iChipInCircuit = typPlateInfo.intResistCntInGroup               ' �]��0�Ȃ�O���[�v�ԍ��i�T�[�L�b�g���Ō�̒�R�j          
                End If
                'V4.5.1.0�L��
                dAddGrpInt = AddGrpInterval(iChpCnt)                    ' BP�O���[�v(�T�[�L�b�g)�Ԋu�擾

                ' �J�b�g�����ݒ肷��
                For iCutCnt = 1 To iCutNum

                    ' ��1��R�̏ꍇ
                    'V6.1.4.9�A                    If iChpCnt = 1 Then
                    If iChpCnt = 1 Or (bCircuit And iChpCnt <= iCirCuitCnt) Then             'V6.1.4.9�A�T�[�L�b�g����R�����l��
                        If (typPlateInfo.intResistDir = 0) Then         ' �`�b�v���ѕ��� = X���� ?
                            ' �����߲�Ă��擾
                            dStartPos(iChpCnt, iCutCnt) = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX
                            ' è��ݸ��߲�Ă��擾
                            dTeachPos(iChpCnt, iCutCnt) = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointX

                            ' è��ݸ��߲�Ă�����߲�Ăɺ�߰    
                            If (gSysPrm.stCTM.giTEACH_P = 2) Then       '  è��ݸ��߲�Ă�����߲�Ăɺ�߰ ?
                                dStartPos(iChpCnt, iCutCnt) = dTeachPos(iChpCnt, iCutCnt) '
                                typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = dStartPos(iChpCnt, iCutCnt)
                            End If
                        Else                                            ' �`�b�v���ѕ��� = Y�����̏ꍇ 
                            ' �����߲�Ă��擾
                            dStartPos(iChpCnt, iCutCnt) = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY
                            ' è��ݸ��߲�Ă��擾
                            dTeachPos(iChpCnt, iCutCnt) = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointY

                            ' è��ݸ��߲�Ă�����߲�Ăɺ�߰ 
                            If (gSysPrm.stCTM.giTEACH_P = 2) Then       ' è��ݸ��߲�Ă�����߲�Ăɺ�߰ ?
                                dStartPos(iChpCnt, iCutCnt) = dTeachPos(iChpCnt, iCutCnt) '
                                typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = dStartPos(iChpCnt, iCutCnt)
                            End If
                        End If

                        ' ��1��R�ȊO�̏ꍇ
                    Else
                        If (typPlateInfo.intResistDir = 0) Then         ' �`�b�v���ѕ��� = X���� ?
                            ' �����߲�čX�V
                            'V4.5.1.0�L��
                            If bCircuit Then
                                If iMarkingNo > 0 Then                  ' �}�[�L���O��
                                    'If iChpCnt > iMarkingNo Then
                                    '    iCircuit = typResistorInfoArray(iChpCnt).intCircuitGrp - 1
                                    '    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = typResistorInfoArray(iMarkingNo).ArrCut(iCutCnt).dblStartPointX + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt
                                    '    ' è��ݸ��߲�Ă֔��f
                                    '    If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                    '        ' è��ݸ��߲��
                                    '        typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointX = typResistorInfoArray(iMarkingNo).ArrCut(iCutCnt).dblTeachPointX + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt
                                    '    End If
                                    '    ' �J�b�g�ʒuX,Y�ɃI�t�Z�b�g�ʂ����Z����(BP��R�[�i�[���l��) ###090
                                    '    If (bOfsFlg = True) Then
                                    '        ' ��n��R�̑�n�J�b�g�ʒuy�ɃI�t�Z�b�g�ʂ����Z����
                                    '        WkDbl = typResistorInfoArray(iChipInCircuit).ArrCut(iCutCnt).dblStartPointY
                                    '        Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX, WkDbl, gSysPrm.stDEV.giBpDirXy)
                                    '        typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = WkDbl
                                    '    End If
                                    'End If
                                    Continue For
                                End If
                                typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = dStartPos(iChipInCircuit, iCutCnt) + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt

                                ' è��ݸ��߲�Ă֔��f
                                If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                    ' è��ݸ��߲��
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointX = dTeachPos(iChipInCircuit, iCutCnt) + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt
                                End If

                                ' �J�b�g�ʒuX,Y�ɃI�t�Z�b�g�ʂ����Z����(BP��R�[�i�[���l��) ###090
                                If (bOfsFlg = True) Then
                                    ' ��n��R�̑�n�J�b�g�ʒuy�ɃI�t�Z�b�g�ʂ����Z����
                                    WkDbl = typResistorInfoArray(iChipInCircuit).ArrCut(iCutCnt).dblStartPointY
                                    Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX, WkDbl, gSysPrm.stDEV.giBpDirXy)
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = WkDbl
                                End If

                                'V6.1.4.9�A         typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX + (dDiffChipSize * (iChpCnt - 1)) + dAddGrpInt - (iCircuit * dblSaveBpGrpItv)
                                'V6.1.4.9�A                                If dblBeforeOffSet <> 0.0 Then
                                'V6.1.4.9�A                                    dDistance = BeforChipSize * (iChpCnt - 1) + (iCircuit * dblSaveBpGrpItv)
                                'V6.1.4.9�A                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY - dblBeforeOffSet * dDistance / ((typPlateInfo.intResistCntInGroup - 1) * BeforChipSize)
                                'V6.1.4.9�A                                End If
                                'V6.1.4.9�A                                If OffSet <> 0.0 Then
                                'V6.1.4.9�A                                    dDistance = ChipSize * (iChpCnt - 1) + dAddGrpInt
                                'V6.1.4.9�A                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY + OffSet * dDistance / ((typPlateInfo.intResistCntInGroup - 1) * ChipSize)
                                'V6.1.4.9�A                                End If
                                'V6.1.4.9�A                                '----- V6.1.4.0_48�� ----- 
                                'V6.1.4.9�A                                ' è��ݸ��߲�Ă֔��f
                                'V6.1.4.9�A                                If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                'V6.1.4.9�A                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointX = dTeachPos(iCutCnt) + (ChipSize * (iChpCnt - 1)) + dAddGrpInt
                                'V6.1.4.9�A                                End If
                                'V6.1.4.9�A                                '----- V6.1.4.0_48�� ----- 
                            Else
                                'V4.5.1.0�L��
                                typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = dStartPos(1, iCutCnt) + (ChipSize * (iChpCnt - 1)) + dAddGrpInt

                                ' è��ݸ��߲�Ă֔��f
                                If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                    ' è��ݸ��߲��
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointX = dTeachPos(1, iCutCnt) + (ChipSize * (iChpCnt - 1)) + dAddGrpInt
                                End If

                                ' �J�b�g�ʒuX,Y�ɃI�t�Z�b�g�ʂ����Z����(BP��R�[�i�[���l��) ###090
                                If (bOfsFlg = True) Then
                                    '----- ###264�� -----
                                    ' ��n��R�̑�n�J�b�g�ʒuy�ɃI�t�Z�b�g�ʂ����Z����
                                    WkDbl = typResistorInfoArray(1).ArrCut(iCutCnt).dblStartPointY
                                    Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX, WkDbl, gSysPrm.stDEV.giBpDirXy)
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = WkDbl
                                    'Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY, gSysPrm.stDEV.giBpDirXy)
                                    '----- ###264�� -----
                                End If
                            End If                                      'V4.5.1.0�L

                            ' �߼޼��ݸނȂ�IX���,�߼޼��ݸނȂ�ST��Ă͎g�p�ł��Ȃ��̂ŏ�����
                            Select Case typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).strCutType
                                Case CNS_CUTP_IX2, CNS_CUTP_ST2
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = 0.0
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointX = 0.0
                            End Select
                        Else                                            ' �`�b�v���ѕ��� = Y�����̏ꍇ 
                            ' �����߲��
                            'V4.5.1.0�L��
                            If bCircuit Then
                                If iMarkingNo > 0 Then                  ' �}�[�L���O��
                                    'If iChpCnt > iMarkingNo Then
                                    '    iCircuit = typResistorInfoArray(iChpCnt).intCircuitGrp - 1
                                    '    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = typResistorInfoArray(iMarkingNo).ArrCut(iCutCnt).dblStartPointY + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt

                                    '    ' è��ݸ��߲�Ă֔��f
                                    '    If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                    '        ' è��ݸ��߲��
                                    '        typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointY = typResistorInfoArray(iMarkingNo).ArrCut(iCutCnt).dblTeachPointY + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt
                                    '    End If

                                    'End If
                                    'If (bOfsFlg = True) Then
                                    '    ' ��n��R�̑�n�J�b�g�ʒux�ɃI�t�Z�b�g�ʂ����Z����
                                    '    WkDbl = typResistorInfoArray(iChipInCircuit).ArrCut(iCutCnt).dblStartPointX
                                    '    Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, WkDbl, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY, gSysPrm.stDEV.giBpDirXy)
                                    '    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = WkDbl
                                    'End If
                                    Continue For
                                End If
                                typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = dStartPos(iChipInCircuit, iCutCnt) + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt

                                ' è��ݸ��߲�Ă֔��f
                                If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                    ' è��ݸ��߲��
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointY = dTeachPos(iChipInCircuit, iCutCnt) + (ChipSize * iCirCuitCnt * iCircuit) + dAddGrpInt
                                End If
                                If (bOfsFlg = True) Then
                                    ' ��n��R�̑�n�J�b�g�ʒux�ɃI�t�Z�b�g�ʂ����Z����
                                    WkDbl = typResistorInfoArray(iChipInCircuit).ArrCut(iCutCnt).dblStartPointX
                                    Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, WkDbl, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY, gSysPrm.stDEV.giBpDirXy)
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = WkDbl
                                End If
                                'V6.1.4.9�A�@CHIP�Ɠ����ɂ���B
                                'V6.1.4.9�A   typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY + (dDiffChipSize * (iChpCnt - 1)) + dAddGrpInt - (iCircuit * dblSaveBpGrpItv)
                                'V6.1.4.9�A                                If OffSet <> 0.0 Then
                                'V6.1.4.9�A                                    If dblBeforeOffSet <> 0.0 Then
                                'V6.1.4.9�A                                        dDistance = BeforChipSize * (iChpCnt - 1) + (iCircuit * dblSaveBpGrpItv)
                                'V6.1.4.9�A                                        typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX - dblBeforeOffSet * dDistance / ((typPlateInfo.intResistCntInGroup - 1) * BeforChipSize)
                                'V6.1.4.9�A                                    End If
                                'V6.1.4.9�A                                    If OffSet <> 0.0 Then
                                'V6.1.4.9�A                                        dDistance = ChipSize * (iChpCnt - 1) + dAddGrpInt
                                'V6.1.4.9�A                                        typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX + OffSet * dDistance / ((typPlateInfo.intResistCntInGroup - 1) * ChipSize)
                                'V6.1.4.9�A                                    End If
                                'V6.1.4.9�A                                End If
                            Else
                                'V4.5.1.0�L��
                                typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = dStartPos(1, iCutCnt) + (ChipSize * (iChpCnt - 1)) + dAddGrpInt

                                ' è��ݸ��߲�Ă֔��f
                                If (gSysPrm.stCTM.giTEACH_P = 1) Or (gSysPrm.stCTM.giTEACH_P = 2) Then
                                    ' è��ݸ��߲��
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointY = dTeachPos(1, iCutCnt) + (ChipSize * (iChpCnt - 1)) + dAddGrpInt
                                End If

                                If (bOfsFlg = True) Then
                                    '----- ###264�� -----
                                    ' ��n��R�̑�n�J�b�g�ʒux�ɃI�t�Z�b�g�ʂ����Z����
                                    WkDbl = typResistorInfoArray(1).ArrCut(iCutCnt).dblStartPointX
                                    Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, WkDbl, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY, gSysPrm.stDEV.giBpDirXy)
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX = WkDbl
                                    'Call Form1.Utility1.GetBPmovePitch(cin, DmyX, DmyY, mPIT, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointX, typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY, gSysPrm.stDEV.giBpDirXy)
                                    '----- ###264�� -----
                                End If
                            End If                                      'V4.5.1.0�L

                            ' �߼޼��ݸނȂ�IX���,�߼޼��ݸނȂ�ST��Ă͎g�p�ł��Ȃ��̂ŏ�����
                            Select Case typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).strCutType
                                Case CNS_CUTP_IX2, CNS_CUTP_ST2
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblStartPointY = 0.0
                                    typResistorInfoArray(iChpCnt).ArrCut(iCutCnt).dblTeachPointY = 0.0
                            End Select
                        End If

                    End If
                Next iCutCnt                                            ' ���J�b�g��

                ' �s�b�`�X�V ###090
                mPIT = mPIT + OffSetReg                                 ' �s�b�` = �s�b�` + �P��R������̃I�t�Z�b�g�� 

            Next iChpCnt                                                ' ����R��

            ' �v���b�N�T�C�Y��ݒ肷��
            Call CalcBlockSize(typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)

            Return (cFRS_NORMAL)                                        ' Return�l = ����

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.SetTrimParamToGlobalArea() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "BP�I�t�Z�b�g�ʂ���P��R������̃I�t�Z�b�g�ʂ����߂�"
    '''=========================================================================
    ''' <summary>BP�I�t�Z�b�g�ʂ���P��R������̃I�t�Z�b�g�ʂ����߂� ###090</summary>
    ''' <param name="OffSet">       (INP)�I�t�Z�b�g��</param>
    ''' <param name="registorCnt">  (INP)��R��</param>
    ''' <param name="OffSetReg">    (OUT)�P��R������̃I�t�Z�b�g��</param>
    ''' <param name="cin">          (OUT)BP�ړ�����(�R���\�[���̖��L�[�R�[�h)</param>
    ''' <returns>cFRS_NORMAL  =  �I�t�Z�b�g�ʂ�ݒ肵��
    '''          cFRS_ERR_RST =  �I�t�Z�b�g�ʂ�ݒ肵�Ȃ� </returns>
    '''=========================================================================
    Private Function GetCutPosOffset(ByVal OffSet As Double, ByVal registorCnt As Integer, ByRef OffSetReg As Double, ByRef cin As Integer) As Integer

        Dim strMSG As String

        Try
            '------------------------------------------------------------------------
            '   ��������
            '------------------------------------------------------------------------
            '----- V1.20.0.1�@�� -----
            'If (OffSet = 0.0) Then Return (cFRS_ERR_RST) '              ' �I�t�Z�b�g�ʂ�0�Ȃ�NOP 
            ' �I�t�Z�b�g�ʂ�0�̏ꍇ��Return�l = cFRS_NORMAL(�I�t�Z�b�g�ʂ�ݒ肵��)�ŕԂ�
            If (OffSet = 0.0) Then
                OffSetReg = 0.0                                         ' �P��R������̃I�t�Z�b�g�� = 0.0 
                Return (cFRS_NORMAL)                                    ' Return�l = �I�t�Z�b�g�ʂ�ݒ肵��
            End If
            '----- V1.20.0.1�@�� -----

            If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���ѕ��� = X�����̏ꍇ
                ' BP�ړ����������߂�
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0, 1                                           ' y��
                        If (OffSet > 0.0) Then
                            cin = &H1000                                ' BP�ړ����� =��(+Y)
                        Else
                            cin = &H800                                 ' BP�ړ����� =��(-Y)
                        End If
                    Case 2, 3                                           ' y��
                        If (OffSet > 0.0) Then
                            cin = &H800                                 ' BP�ړ����� =��(+Y)
                        Else
                            cin = &H1000                                ' BP�ړ����� =��(-Y)
                        End If
                End Select

            ElseIf (typPlateInfo.intResistDir = 1) Then                 ' �`�b�v���ѕ��� = Y�����̏ꍇ
                ' BP�ړ����������߂�
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0, 2                                           ' x��
                        If (OffSet > 0.0) Then
                            'cin = &H200                                 ' BP�ړ����� =��(+X)
                            cin = &H400                                 ' BP�ړ����� =��(-X) V1.14.0.0�B
                        Else
                            'cin = &H400                                 ' BP�ړ����� =��(-X)
                            cin = &H200                                 ' BP�ړ����� =��(+X) V1.14.0.0�B
                        End If
                    Case 1, 4                                           ' x��
                        If (OffSet > 0.0) Then
                            'cin = &H400                                 ' BP�ړ����� =��(+X)
                            cin = &H200                                 ' BP�ړ����� =��(+X) V1.14.0.0�B
                        Else
                            'cin = &H200                                 ' BP�ړ����� =��(-X)
                            cin = &H400                                 ' BP�ړ����� =��(-X) V1.14.0.0�B
                        End If
                End Select
            End If

            ' �P��R������̃I�t�Z�b�g��(OffSetReg)�����߂�
            OffSetReg = Math.Abs(OffSet / (registorCnt - 1))            ' �P��R������̃I�t�Z�b�g�� = �I�t�Z�b�g�� / ��R�� - 1 

            Return (cFRS_NORMAL)                                        ' Return�l = �I�t�Z�b�g�ʂ�ݒ肵��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "frmTxTyTeach.GetCutPosOffset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_ERR_RST)
        End Try
    End Function
#End Region

#Region "�g���~���O�p�����[�^�X�V����(�s�x�e�B�[�`���O)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�g���~���O�p�����[�^�X�V����(�s�x�e�B�[�`���O)�yCHIP/NET�p�z</summary>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <param name="GrpCnt">  (INP)��ٰ�ߐ�����</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetTrimParamToGlobalArea_TY(ByVal ChipSize As Double, ByVal GrpCnt As Short)

        Dim lRow As Integer
        Dim RowMax As Integer
        'Dim iPos As Integer
        'Dim SpOff As Double
        Dim dData As Double
        Dim strMSG As String

        Try
            ' �`�b�v�T�C�Y���X�V����
            If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���ѕ��� = X���� ?
                typPlateInfo.dblChipSizeYDir = ChipSize
            Else
                typPlateInfo.dblChipSizeXDir = ChipSize
            End If

            ' �v���b�N�T�C�Y��ݒ肷�� ###113
            Call CalcBlockSize(typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)

            ' �X�e�[�W�O���[�v�Ԋu���X�V����
            If (RowAry Is Nothing = False) Then
                RowMax = RowAry.Length
                If (RowMax >= 1) Then
                    lRow = 1
                    If (typPlateInfo.intResistDir = 0) Then                 ' �`�b�v���ѕ��� = X���� ?
                        typPlateInfo.dblStgGrpItvY = mdStepInterval(lRow)   ' Y�����X�e�[�W�O���[�v�Ԋu
                    Else
                        typPlateInfo.dblStgGrpItvX = mdStepInterval(lRow)   ' X�����X�e�[�W�O���[�v�Ԋu
                    End If
                End If
            End If

            ' �X�e�b�v�I�t�Z�b�g�ʂ�ݒ肷��
            ' (TY�e�B�[�`���O�ŏI�u���b�N�e�B�[�`���O����X�����̂���ʂ��X�e�b�v�I�t�Z�b�g�ƂȂ�) ###091
            If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���ѕ��� = X���� ?
                dData = StepOffSetX(1) - StepOffSetX(0)                 ' �I�t�Z�b�g�ʒuY(0:��1��_�@1:��2��_)
            Else
                dData = StepOffSetY(1) - StepOffSetY(0)                 ' �I�t�Z�b�g�ʒuX(0:��1��_�@1:��2��_)
            End If
            'dData = 0.0#
            'iPos = 2 * GrpCnt
            'For lRow = 0 To iPos
            '    If (typPlateInfo.intResistDir = 0) Then                 ' �`�b�v���ѕ��� = X���� ?
            '        dData = dData + ZRPosX(lRow)                        ' ZRPosX(�����X(0:��1��_, 1:��2��_...))
            '    Else
            '        dData = dData + ZRPosY(lRow)                        ' ZRPosY(�����Y(0:��1��_, 1:��2��_...))
            '    End If
            'Next lRow

            ' �v���[�g�f�[�^�̃X�e�b�v�I�t�Z�b�g�ʂ��X�V����(�X�e�b�v�I�t�Z�b�g�ʂ�0�̏ꍇ�̂ݐݒ肷��) 
            'If ((typPlateInfo.dblStepOffsetXDir = 0) And (typPlateInfo.dblStepOffsetYDir = 0)) Then '###263
            If (typPlateInfo.intResistDir = 0) Then                 ' �`�b�v���ѕ��� = X���� ?
                ' ���ݸވʒu���W + �����ڰ���ð��ٵ̾�� + �␳�ʒu  (+or-) ð��ٕ␳��
                '###249 ��
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0, 2      ' x��, y��
                        typPlateInfo.dblStepOffsetXDir = CDbl(dData.ToString("0.0000"))
                    Case 1, 3      ' x��, y��
                        typPlateInfo.dblStepOffsetXDir = -1 * CDbl(dData.ToString("0.0000"))
                End Select
                '###249 ��                   typPlateInfo.dblStepOffsetXDir = CDbl(dData.ToString("0.0000"))
                'SpOff = (dData - ZRPosX(0)) / (typPlateInfo.intBlockCntYDir - 1)
                'typPlateInfo.dblStepOffsetXDir = CDbl(SpOff.ToString("0.0000"))
            Else
                '###249 ��
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0, 1      ' y��
                        typPlateInfo.dblStepOffsetYDir = CDbl(dData.ToString("0.0000"))
                    Case 2, 3      ' y��
                        typPlateInfo.dblStepOffsetYDir = -1 * CDbl(dData.ToString("0.0000"))
                End Select
                '###249 ��                   typPlateInfo.dblStepOffsetYDir = CDbl(dData.ToString("0.0000"))
                'SpOff = (dData - ZRPosY(0)) / (typPlateInfo.intBlockCntXDir - 1)
                'typPlateInfo.dblStepOffsetYDir = CDbl(SpOff.ToString("0.0000"))
            End If
            'End If '###263

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.SetTrimParamToGlobalArea_TY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�w���R�ɑΉ�����BP�O���[�v(�T�[�L�b�g)�Ԋu�擾"
    '''=========================================================================
    '''<summary>�w���R�ɑΉ�����BP�O���[�v(�T�[�L�b�g)�Ԋu�擾</summary>
    '''<param name="Rno">(INP)��R�f�[�^�C���f�b�N�X</param>
    '''<returns>BP�O���[�v(�T�[�L�b�g)�Ԋu</returns>
    '''=========================================================================
    Private Function AddGrpInterval(ByRef Rno As Short) As Double

        Dim iGrCnt As Integer
        Dim dRet As Double                                              ' �߂�l
        Dim strMSG As String

        Try
            ' BP�O���[�v(�T�[�L�b�g)����1�Ȃ�0��Ԃ� 
            If (typPlateInfo.intGroupCntInBlockXBp <= 1) Then
                Return (0.0)
            End If

            ' �O���[�v�ԍ� = ��R�ԍ� / 1�O���[�v(1�T�[�L�b�g)����R��
            iGrCnt = Rno \ typPlateInfo.intResistCntInGroup
            If ((Rno Mod typPlateInfo.intResistCntInGroup) = 0) Then
                iGrCnt = iGrCnt - 1                                     ' �]��0�Ȃ�O���[�v�ԍ�-1 
            End If

            ' �C���^�[�o�� = �O���[�v�ԍ� * BP�O���[�v(�T�[�L�b�g)�Ԋu
            dRet = iGrCnt * typPlateInfo.dblBpGrpItv
            Return (dRet)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.AddGrpInterval() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (0.0)
        End Try
    End Function
#End Region

    '=========================================================================
    '   �f�[�^�O���b�h�r���[�̕\������
    '=========================================================================
#Region "�f�[�^�O���b�h�r���[�I�u�W�F�N�g�̐���"
    '''=========================================================================
    '''<summary>�f�[�^�O���b�h�r���[�I�u�W�F�N�g�̐���</summary>
    ''' <param name="ObjGrid">(I/O)�f�[�^�O���b�h�r���[�I�u�W�F�N�g</param>
    ''' <param name="GrpNum"> (INP)�O���[�v��</param>
    ''' <param name="RowAry"> (OUT)Row(�s)�I�u�W�F�N�g�z��I�u�W�F�N�g</param>
    ''' <param name="ColAry"> (OUT)Col(��)�I�u�W�F�N�g�z��I�u�W�F�N�g</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitGridView(ByVal ObjGrid As DataGridView, ByVal GrpNum As Short, ByRef RowAry() As DataGridViewRow, ByRef ColAry() As DataGridViewColumn)

        Dim RowIdx As Integer
        Dim ColIdx As Integer
        Dim RowCount As Integer
        Dim ColCount As Integer
        Dim strMSG As String

        Try
            ' �O���b�h�̕\��/��\����ݒ肷��
            'V6.1.4.9�A            If (GrpNum <= 1) Then                                   ' ��ٰ�ߐ�1�ȉ��Ȃ��\��
            LblDisp_10.Visible = False                          ' ���x����\��
                GridView.Visible = False                            ' �ڷ���ٸ�د�ޔ�\��
                Exit Sub
            'V6.1.4.9�A            End If

            ' Row(�s)/Col(��)�I�u�W�F�N�g�z�񐶐�
            RowCount = GrpNum - 1                                   ' �ő�s�� = ��ٰ�ߐ�(0 ORG)
            ColCount = 3                                            ' �ő�� = 3
            RowAry = New DataGridViewRow(RowCount) {}               ' Row(�s)�I�u�W�F�N�g�z�񐶐�
            ColAry = New DataGridViewColumn(ColCount) {}            ' Col(��)�I�u�W�F�N�g�z�񐶐�

            ' �O���b�h������ݒ肷��
            ObjGrid.ReadOnly = True                                 ' �ҏW�֎~
            ObjGrid.ColumnHeadersVisible = True                     ' ��w�b�_�[�\��
            'ObjGrid.RowHeadersVisible = True                        ' �s�w�b�_�[�\��
            ObjGrid.RowHeadersVisible = False                       ' �s�w�b�_�[��\��
            ObjGrid.AutoGenerateColumns = False                     ' �񂪎����I�ɍ쐬����Ȃ��悤�ɂ���
            ObjGrid.AllowUserToAddRows = False                      ' �V�����s��ǉ��ł��Ȃ��悤�ɂ��� 
            ObjGrid.AllowUserToResizeColumns = False                ' ��̕���ύX�ł��Ȃ��悤�ɂ���
            ObjGrid.AllowUserToResizeRows = False                   ' �s�̍�����ύX�ł��Ȃ��悤�ɂ���
            '                                                       ' �w�b�_�[�̗�̕�/�s�̍�����ύX�ł��Ȃ��悤�ɂ���
            ObjGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            ObjGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing

            ' ��w�b�_�[��ݒ肷��
            For ColIdx = 0 To (ColCount - 1)
                ' Col(��)�I�u�W�F�N�g���쐬����
                Dim ObjCol As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
                ObjGrid.Columns.Add(ObjCol)                         ' Col(��)��ǉ�����
                ColAry(ColIdx) = ObjCol                             ' Col(��)�I�u�W�F�N�g�z��ɐݒ�
                Select Case (ColIdx)
                    Case 0                                          ' ��w�b�_�[�ݒ�(0�� �񌩏o�� = �Ȃ�)
                        ObjGrid.Columns(ColIdx).HeaderText = ""
                    Case 1                                          ' ��w�b�_�[�ݒ�(1�� �񌩏o�� = "�␳�O")
                        ObjGrid.Columns(ColIdx).HeaderText = LBL_TXTY_TEACH_07
                    Case Else                                       ' ��w�b�_�[�ݒ�(2�� �񌩏o�� = "�␳��")
                        ObjGrid.Columns(ColIdx).HeaderText = LBL_TXTY_TEACH_08
                End Select

                ObjGrid.Columns(ColIdx).Name = "Col" + ColIdx.ToString()                        ' ��̖��O 
                ObjGrid.Columns(ColIdx).Width = Len(ObjGrid.Columns(ColIdx).HeaderText) + 120   ' ��̕� 
                '                                                                               ' �w�b�_�[�e�L�X�g�̔z�u���㉺���E�Ƃ������ɂ��� 
                ObjGrid.Columns(ColIdx).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                '                                                                               ' ��̃e�L�X�g�̔z�u���E�l�ɂ��� 
                ObjGrid.Columns(ColIdx).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next ColIdx

            ' ���ׂĂ̗�̕��ёւ����֎~����
            For Each Col As DataGridViewColumn In ObjGrid.Columns
                Col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next Col

            ' �s����ݒ肷��
            For RowIdx = 0 To (RowCount - 1)
                Dim ObjRow As DataGridViewRow = New DataGridViewRow()   ' Row(�s)�I�u�W�F�N�g�쐬
                RowAry(RowIdx) = ObjRow                                 ' Row(�s)�I�u�W�F�N�g�z��ɐݒ�
                RowAry(RowIdx).CreateCells(ObjGrid)                     ' Col(��)�����擾����

                ' �s(�w�b�_�[�O���[�v1-2, �O���[�v2-3 .....)��Row(�s)�I�u�W�F�N�g�ɐݒ肷��
                strMSG = LBL_TXTY_TEACH_14 + " " + (RowIdx + 1).ToString("0") + "-" + (RowIdx + 2).ToString("0")
                RowAry(RowIdx).Cells(0).Value = strMSG
                ObjGrid.Rows.Add(RowAry(RowIdx))                        ' �O���b�h��Row(�s)�v���p�e�B�ɃZ�b�g
                ObjGrid.Rows(RowIdx).HeaderCell.Value = ""              ' �s(�w�b�_�[�O���[�v1-2, �O���[�v2-3 .....)
                '                                                       ' �e�L�X�g�̔z�u���E�l�߂ɂ��� 
                ObjGrid.Rows(RowIdx).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            Next RowIdx

            ' �Z���̂��ׂĂ̑I��������
            ObjGrid.ClearSelection()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTeach.InitGridView() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�f�[�^�O���b�h�r���[�I�u�W�F�N�g�̊J��"
    '''=========================================================================
    ''' <summary>�f�[�^�O���b�h�r���[�I�u�W�F�N�g�̊J��</summary>
    ''' <param name="RowAry"> (I/O)Row(�s)�I�u�W�F�N�g�z��I�u�W�F�N�g</param>
    ''' <param name="ColAry"> (I/O)Col(��)�I�u�W�F�N�g�z��I�u�W�F�N�g</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TermGridView(ByRef RowAry() As DataGridViewRow, ByRef ColAry() As DataGridViewColumn)

        Dim Idx As Integer
        Dim Count As Integer
        Dim strMSG As String

        Try
            ' Row(�s)�I�u�W�F�N�g�z��J��
            If (RowAry Is Nothing = False) Then
                Count = RowAry.Length
                For Idx = 0 To (Count - 1)
                    If (RowAry(Idx) Is Nothing = False) Then
                        RowAry(Idx).Dispose()                       ' ���\�[�X�J�� 
                    End If
                Next Idx
                RowAry = Nothing
            End If

            ' Col(��)�I�u�W�F�N�g�z��J��
            If (ColAry Is Nothing = False) Then
                Count = ColAry.Length
                For Idx = 0 To (Count - 1)
                    If (ColAry(Idx) Is Nothing = False) Then
                        ColAry(Idx).Dispose()                       ' ���\�[�X�J�� 
                    End If
                Next Idx
                ColAry = Nothing
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.TermGridView() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�f�[�^�O���b�h�r���[�̃C���^�[�o���\�����N���A����"
    '''=========================================================================
    '''<summary>�f�[�^�O���b�h�r���[�̃C���^�[�o���\�����N���A����</summary>
    ''' <param name="RowAry"> (I/O)Row(�s)�I�u�W�F�N�g�z��I�u�W�F�N�g</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub ClearGridView(ByVal RowAry() As DataGridViewRow)

        Dim RowIdx As Integer
        Dim RowMax As Integer
        Dim strMSG As String

        Try
            If (RowAry Is Nothing = True) Then Exit Sub
            RowMax = RowAry.Length
            For RowIdx = 0 To (RowMax - 1)
                If (RowAry(RowIdx) Is Nothing = False) Then
                    RowAry(RowIdx).Cells(1).Value = ""                  ' �␳�O�C���^�[�o���\���N���A
                    RowAry(RowIdx).Cells(2).Value = ""                  ' �␳��C���^�[�o���\���N���A
                End If
            Next RowIdx

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.ClearGridView() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�f�[�^�O���b�h�r���[�փC���^�[�o���l(�␳�O/�␳��)��\������"
    '''=========================================================================
    ''' <summary>�f�[�^�O���b�h�r���[�փC���^�[�o���l��\������</summary>
    ''' <param name="RowAry">  (I/O)Row(�s)�I�u�W�F�N�g�z��I�u�W�F�N�g</param>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub DispGridView(ByVal RowAry() As DataGridViewRow, ByVal ChipSize As Double)

        Dim RowIdx As Integer
        Dim RowMax As Integer
        Dim StgGrpItv As Double
        Dim strMSG As String

        Try
            ' Row(�s)�I�u�W�F�N�g�z��Ȃ��Ȃ�NOP
            If (RowAry Is Nothing = True) Then Exit Sub

            ' ��2��_�\���Ȃ��Ȃ�NOP
            If (TxtPos2X.Text = "") Then Exit Sub

            ' �f�[�^�O���b�h�r���[�փC���^�[�o���l��\������
            RowMax = RowAry.Length - 2
            For RowIdx = 0 To RowMax                                    ' �O���b�h�s�����J��Ԃ� 
                If (giAppMode = APP_MODE_TX) Then                       ' �s�w�e�B�[�`���O��(CHIP/NET����)
                    ' �␳�O�C���^�[�o��(�a�o�O���[�v(�T�[�L�b�g)�Ԋu)�\��
                    RowAry(RowIdx).Cells(1).Value = typPlateInfo.dblBpGrpItv.ToString("0.0000")
                    ' �␳��C���^�[�o��(�a�o�O���[�v(�T�[�L�b�g)�Ԋu)�\��
                    'RowAry(RowIdx).Cells(2).Value = mdStepInterval(RowIdx + 1).ToString("0.0000")
                    RowAry(RowIdx).Cells(2).Value = mdStepInterval(1).ToString("0.0000")                ' ###081

                Else                                                    ' �s�x�e�B�[�`���O��
                    ' �␳�O�C���^�[�o���\��
                    If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���ѕ��� = X���� ?
                        StgGrpItv = typPlateInfo.dblStgGrpItvY          ' Y�����X�e�[�W�O���[�v�Ԋu
                    Else
                        StgGrpItv = typPlateInfo.dblStgGrpItvX          ' X�����X�e�[�W�O���[�v�Ԋu
                    End If
                    RowAry(RowIdx).Cells(1).Value = StgGrpItv.ToString("0.0000")
                    ' �␳��C���^�[�o���\��
                    'RowAry(RowIdx).Cells(2).Value = mdStepInterval(RowIdx + 1).ToString("0.0000")
                    RowAry(RowIdx).Cells(2).Value = mdStepInterval(1).ToString("0.0000")                ' ###081
                End If
            Next RowIdx

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.DispGridView() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '=========================================================================
    '   ���̑����ʊ֐�
    '=========================================================================
#Region "�X�V�O/�X�V��̃`�b�v�T�C�Y��\������"
    '''=========================================================================
    '''<summary>�X�V�O/�X�V��̃`�b�v�T�C�Y��\������</summary>
    ''' <param name="ChipSize">(INP)�X�V��`�b�v�T�C�Y</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub DispChipSize(ByVal ChipSize As Double)

        Dim mdCSx As Double                                         ' �X�V�O�̃`�b�v�T�C�YX
        Dim mdCSy As Double                                         ' �X�V�O�̃`�b�v�T�C�YY
        Dim strMSG As String

        Try
            ' �X�V�O�̃`�b�v�T�C�Y���擾����(CHIP/NET����)
            mdCSx = typPlateInfo.dblChipSizeXDir
            mdCSy = typPlateInfo.dblChipSizeYDir

            ' �X�V�O/�X�V��̃`�b�v�T�C�Y��\������
            If (giAppMode = APP_MODE_TX) Then
                ' �s�w�e�B�[�`���O��
                With Me
                    If (typPlateInfo.intResistDir = 0) Then         ' �`�b�v���т�X���� ?
                        ' �`�b�v�T�C�YX(��)
                        .LblResult_1.Text = mdCSx.ToString("0.0000")
                        ' �␳�䗦
                        .LblResult_0.Text = System.Math.Abs(ChipSize / mdCSx).ToString("0.0000")
                    Else
                        ' �`�b�v�T�C�YY(��)
                        .LblResult_1.Text = mdCSy.ToString("0.0000")
                        ' �␳�䗦
                        .LblResult_0.Text = System.Math.Abs(ChipSize / mdCSy).ToString("0.0000")
                    End If
                    ' �`�b�v�T�C�Y(�V)
                    .LblResult_2.Text = ChipSize.ToString("0.0000")
                End With
            Else
                ' �s�x�e�B�[�`���O��
                With Me
                    If (typPlateInfo.intResistDir = 0) Then         ' �`�b�v���т�X���� ?
                        ' �`�b�v�T�C�YX(��)
                        .LblResult_1.Text = mdCSy.ToString("0.0000")
                        ' �␳�䗦
                        .LblResult_0.Text = System.Math.Abs(ChipSize / mdCSy).ToString("0.0000")
                    Else
                        ' �`�b�v�T�C�YY(��)
                        .LblResult_1.Text = mdCSx.ToString("0.0000")
                        ' �␳�䗦
                        .LblResult_0.Text = System.Math.Abs(ChipSize / mdCSx).ToString("0.0000")
                    End If
                    ' �`�b�v�T�C�Y(�V)
                    .LblResult_2.Text = ChipSize.ToString("0.0000")
                End With
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.DispChipSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u���b�N�ړ�(BP�ړ�)�yTX�e�B�[�`���O�p�z"
    '''=========================================================================
    ''' <summary>�u���b�N�ړ�(BP�ړ�)�yTX�e�B�[�`���O�p�z</summary>
    ''' <param name="MODE">    (INP)0=��1��ٰ�ߑ�1��ʒu , 1=��1��ٰ�ߍŏI��ʒu
    '''                             2=��n��ٰ�ߍŏI��ʒu, 3=��n+1��ٰ�߁A�擪��ʒu�u</param>
    ''' <param name="iPos">    (INP)�����X,Yð���(ZRPosX(),ZRPosY())���ޯ��</param>
    ''' <param name="Ino">     (INP)�������̸�ٰ�ߔԍ�</param>
    ''' <param name="ChipNum"> (INP)�`�b�v��</param>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <param name="KJPosX">  (INP)�擪�u���b�N�̐擪��ʒuX</param>
    ''' <param name="KJPosY">  (INP)�擪�u���b�N�̐擪��ʒuy</param>
    ''' <param name="TBLx">    (OUT)BP�ʒuX</param>
    ''' <param name="TBLy">    (OUT)BP�ʒuy</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function XYBPMoveSetBlock(ByRef MODE As Short, ByRef iPos As Short, ByVal Ino As Short, ByVal ChipNum As Short,
                                      ByVal ChipSize As Double, ByVal KJPosX As Double, ByVal KJPosY As Double,
                                      ByRef TBLx As Double, ByRef TBLy As Double, ByRef GrpNum As Short) As Short  'V6.1.4.9�A�@�T�[�L�b�g��GrpNum�ǉ�

        'Dim i As Short
        'Dim iBlock As Short
        'Dim dIntv As Double
        'Dim ZZx As Double
        'Dim ZZy As Double
        Dim strMSG As String
        Dim r As Short
        'Dim Gn As Integer
        'Dim GrpSz As Double

        Try
            Select Case MODE
                Case 0                                              ' ��1��ٰ�߁A��1��ʒu
                    ' ��1��ۯ��̑���R�̑��J�b�g�ʒu + ����� �̈ʒu�����߂�
                    TBLx = KJPosX + ZRPosX(0)                       ' ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X
                    TBLy = KJPosY + ZRPosY(0)                       ' ��1��ۯ��̐擪��ʒuY + ��1��_�̂����Y

                Case 1                                              ' ��1��ٰ�߁A�ŏI��ʒu
                    If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��� X���� ?
                        ' ���߻��� * ���ߐ�-1 + ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X �̈ʒu�����߂�
                        TBLx = (ChipSize * (ChipNum - 1)) + KJPosX + ZRPosX(1)
                        TBLy = KJPosY + ZRPosY(1)
                    Else
                        TBLx = KJPosX + ZRPosX(1)
                        TBLy = (ChipSize * (ChipNum - 1)) + KJPosY + ZRPosY(1)
                    End If
                    'V6.1.4.9�A��
                    If gTkyKnd = KND_NET Then                       'V4.5.1.1�@
                        If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��� X���� ?
                            ' ���߻��� * ���ߐ�-1 + ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X �̈ʒu�����߂�
                            TBLx = (ChipSize * (ChipNum)) * (GrpNum - 1) + KJPosX + ZRPosX(1)
                            TBLy = KJPosY + ZRPosY(1)
                        Else
                            TBLx = KJPosX + ZRPosX(1)
                            TBLy = (ChipSize * (ChipNum)) * (GrpNum - 1) + KJPosY + ZRPosY(1)
                        End If
                    End If                                          'V4.5.1.1�@
                    'V6.1.4.9�A��
'V6.1.4.9�A                   If gTkyKnd = KND_NET Then                       'V4.5.1.1�@
'V6.1.4.9�A                        'V4.5.1.0�N��
'V6.1.4.9�A                        If (typPlateInfo.dblTXChipsizeRelationY <> 0.0 And typPlateInfo.intResistDir = 0) Then         ' ���ߕ��� X���� ?
'V6.1.4.9�A                            ' ���߻��� * ���ߐ�-1 + ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X �̈ʒu�����߂�
'V6.1.4.9�A                            TBLx = typPlateInfo.dblTXChipsizeRelationX + KJPosX + ZRPosX(0)
'V6.1.4.9�A                            'V6.1.4.9�A                            TBLy = typPlateInfo.dblTXChipsizeRelationY + ZRPosY(0)
'V6.1.4.9�A                            TBLy = typPlateInfo.dblTXChipsizeRelationY + KJPosY + ZRPosY(0)          'V6.1.4.9�A
'V6.1.4.9�A                        End If
'V6.1.4.9�A                        If (typPlateInfo.dblTXChipsizeRelationX <> 0.0 And typPlateInfo.intResistDir <> 0) Then         ' ���ߕ��� Y���� ?
'V6.1.4.9�A                            'V6.1.4.9�A                            TBLx = typPlateInfo.dblTXChipsizeRelationX + ZRPosX(0)
'V6.1.4.9�A                            TBLx = typPlateInfo.dblTXChipsizeRelationX + KJPosX + ZRPosX(0)          'V6.1.4.9�A
'V6.1.4.9�A                            TBLy = typPlateInfo.dblTXChipsizeRelationY + KJPosY + ZRPosY(0)
'V6.1.4.9�A                        End If
'V6.1.4.9�A                        'V4.5.1.0�N��
'V6.1.4.9�A                    End If                                          'V4.5.1.1�@

                Case 2                                              ' ��n��ٰ�߁A�ŏI��ʒu
                    If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��� X���� ?
                        ' ���߻��� * ���ߐ� + ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X �̈ʒu�����߂�
                        'V4.5.1.0�L                        TBLx = (ChipSize * ChipNum) + KJPosX + ZRPosX(MODE)
                        'V4.5.1.0�L                        TBLx = (ChipSize * (ChipNum - 1)) + KJPosX + ZRPosX(MODE) 'V4.5.1.0�L
                        'V4.5.1.0�L                        TBLy = KJPosY + ZRPosY(MODE)
                        TBLx = StepOffSetX(1)               'V4.5.1.0�L
                        TBLy = StepOffSetY(1)               'V4.5.1.0�L
                    Else
                        'V4.5.1.0�L                        TBLx = KJPosX + ZRPosX(MODE)
                        'V4.5.1.0�L                        TBLy = (ChipSize * ChipNum) + KJPosY + ZRPosX(MODE)
                        TBLx = StepOffSetX(1)               'V4.5.1.0�L
                        TBLy = StepOffSetY(1)               'V4.5.1.0�L
                    End If

                Case 3                                              ' ��n+1��ٰ�߁A�擪��ʒu
                    If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��� X���� ?
                        ' ���߻��� * ���ߐ� + ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X �̈ʒu�����߂�
                        'V4.5.1.0�L                        TBLx = (ChipSize * ChipNum) + KJPosX + ZRPosX(MODE) + mdStepInterval(MODE)
                        'V4.5.1.0�L                        TBLy = KJPosY + ZRPosY(MODE)
                        TBLx = StepOffSetX(1) + ChipSize    'V4.5.1.0�L
                        TBLy = StepOffSetY(1)               'V4.5.1.0�L
                    Else
                        'V4.5.1.0�L                        TBLx = KJPosX + ZRPosX(MODE)
                        'V4.5.1.0�L                        TBLy = (ChipSize * ChipNum) + KJPosY + ZRPosX(MODE) + mdStepInterval(MODE)
                        TBLx = StepOffSetX(1)               'V4.5.1.0�L
                        TBLy = StepOffSetY(1) + ChipSize    'V4.5.1.0�L
                    End If
                    'V6.1.4.9�A��
                    If gTkyKnd = KND_NET Then                       'V4.5.1.1�@
                        If (typPlateInfo.intResistDir = 0) Then         ' ���ߕ��� X���� ?
                            ' ���߻��� * ���ߐ� + ��1��ۯ��̐擪��ʒuX + ��1��_�̂����X �̈ʒu�����߂�
                            TBLx = StepOffSetX(1) + (ChipSize * ChipNum)
                            TBLy = StepOffSetY(1)
                        Else
                            TBLx = StepOffSetX(1)
                            TBLy = StepOffSetY(1) + (ChipSize * ChipNum)
                        End If
                    End If                                          'V4.5.1.1�@
                    'V6.1.4.9�A��
            End Select

            ' BP�ړ�(��Βl�w��)
            r = Form1.System1.EX_MOVE(gSysPrm, TBLx, TBLy, 1)
            Return (r)                                      ' Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTeach.XYBPMoveSetBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return�l = ��O�G���[ 
        End Try

    End Function
#End Region

#Region "�擪�u���b�N�̐擪��ʒu��Ԃ�"
    '''=========================================================================
    ''' <summary>�擪�u���b�N�̐擪��ʒu��Ԃ�</summary>
    ''' <param name="PosX">(OUT)���WX</param>
    ''' <param name="PosY">(OUT)���WY</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function XYTableMoveTopBlock(ByRef PosX As Double, ByRef PosY As Double) As Integer

        Dim dblTrimPosX As Double                                   ' ����߼޼��X
        Dim dblTrimPosY As Double                                   ' ����߼޼��Y
        Dim dblBSX As Double                                        ' ��ۯ�����X
        Dim dblBSY As Double                                        ' ��ۯ�����Y
        Dim dblBsoX As Double                                       ' ��ۯ����޵̾��X
        Dim dblBsoY As Double                                       ' ��ۯ����޵̾��Y
        Dim Del_x As Double                                         ' �ƕ␳��X
        Dim Del_y As Double                                         ' �ƕ␳��Y
        Dim dblRotX As Double                                       ' ��]���aX
        Dim dblRotY As Double                                       ' ��]���aY
        Dim dblX As Double                                          ' �ړ����WX
        Dim dblY As Double                                          ' �ړ����WY
        Dim mdTbOffx As Double                                      ' ð��وʒu�̾��X
        Dim mdTbOffy As Double                                      ' ð��وʒu�̾��Y

        Dim strMSG As String

        Try
            ' ����߼޼��X,Y�擾
            ''''(2010/11/16) ����m�F�㉺�L�R�����g�͍폜
            'dblTrimPosX = gStartX                                  ' ����߼޼��X,Y�擾
            'dblTrimPosY = gStartY
            dblRotX = 0
            dblRotY = 0
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                     ' ����߼޼��X,Y�擾
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            mdTbOffx = typPlateInfo.dblTableOffsetXDir              ' ð��وʒu�̾��X,Y�̎擾
            mdTbOffy = typPlateInfo.dblTableOffsetYDir
            Call CalcBlockSize(dblBSX, dblBSY)                      ' ��ۯ����ގZ�o

            ' ��ۯ����޵̾�ĎZ�o(��ۯ�����/2 ��ۯ��̏ی���XY�Ƃ���1 ð��ق̏ی���1)
            dblBsoX = (dblBSX / 2.0#) * 1 * 1
            dblBsoY = (dblBSY / 2.0#) * 1
            '----- V4.0.0.0-40�� -----
            ' SL36S���̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
            If (giMachineKd = MACHINE_KD_RS) Then
                ''V4.6.0.0�D                If (giStageYOrg = STGY_ORG_UP) Then
                dblBsoY = 0
                ''V4.6.0.0�D            End If
            End If
            '----- V4.0.0.0-40�� -----

            ' �ƕ␳��ѵ̾��X,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
            ' 0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
            ' ���ݸވʒu���W (+or-) ��]���a + ð��ٵ̾�� (+or-) ��ۯ����޵̾�� + ð��ٕ␳��
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    dblX = dblTrimPosX + dblRotX + mdTbOffx + dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + mdTbOffy + dblBsoY + Del_y
                Case 1 ' x��, y��
                    ''###249                    dblX = dblTrimPosX - dblRotX + mdTbOffx - dblBsoX + Del_x
                    'dblX = dblTrimPosX - dblRotX - mdTbOffx - dblBsoX + Del_x
                    dblX = dblTrimPosX - dblRotX - mdTbOffx - dblBsoX - Del_x
                    dblY = dblTrimPosY + dblRotY + mdTbOffy + dblBsoY + Del_y
                Case 2 ' x��, y��
                    dblX = dblTrimPosX + dblRotX + mdTbOffx + dblBsoX + Del_x
                    'dblY = dblTrimPosY - dblRotY + mdTbOffy - dblBsoY - Del_y ' V1.20.0.0�D
                    dblY = dblTrimPosY - dblRotY - mdTbOffy - dblBsoY - Del_y  ' V1.20.0.0�D
                Case 3 ' x��, y��
                    ''###249                    dblX = dblTrimPosX - dblRotX + mdTbOffx - dblBsoX + Del_x
                    'dblX = dblTrimPosX - dblRotX - mdTbOffx - dblBsoX + Del_x
                    dblX = dblTrimPosX - dblRotX - mdTbOffx - dblBsoX + Del_x
                    'dblY = dblTrimPosY - dblRotY + mdTbOffy - dblBsoY + Del_y
                    'dblY = dblTrimPosY - dblRotY + mdTbOffy - dblBsoY - Del_y' V1.20.0.0�D
                    dblY = dblTrimPosY - dblRotY - mdTbOffy - dblBsoY - Del_y ' V1.20.0.0�D
            End Select

            PosX = dblX
            PosY = dblY
            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.XYTableMoveTopBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[ 
        End Try
    End Function
#End Region

#Region "�u���b�N�ړ�(XY�e�[�u���ړ�)�yTY�e�B�[�`���O�p�z"
    '''=========================================================================
    ''' <summary>�u���b�N�ړ�(XY�e�[�u���ړ�)�yTY�e�B�[�`���O�p�z</summary>
    ''' <param name="MODE">    (INP)0=��1��ٰ�ߑ�1��ʒu , 1=��1��ٰ�ߍŏI��ʒu
    '''                             2=��1��ٰ�ߍŏI��ʒu, 3=��2��ٰ�߁A�擪��ʒu
    '''                             4=�ŏI�u���b�N��ʒu</param>
    ''' <param name="iPos">    (INP)�����X,Yð���(ZRPosX(),ZRPosY())���ޯ��
    ''' �@�@�@�@�@�@�@�@�@�@�@�@�@�@0=��1��ٰ�ߑ�1��ʒu , 1=��1��ٰ�ߍŏI��ʒu
    ''' �@�@�@�@�@�@�@�@�@�@�@�@�@�@2=��1��ٰ�ߍŏI��ʒu, 3=��2��ٰ�ߐ擪��ʒu
    ''' </param>
    ''' <param name="Ino">     (INP)�������̲������No</param>
    ''' <param name="Bn">      (INP)�u���b�N��</param>
    ''' <param name="ChipSize">(INP)�`�b�v�T�C�Y</param>
    ''' <param name="KJPosX">  (INP)�擪�u���b�N�̐擪��e�[�u���ʒuX</param>
    ''' <param name="KJPosY">  (INP)�擪�u���b�N�̐擪��e�[�u���ʒuy</param>
    ''' <param name="TBLx">    (OUT)�e�[�u���ʒuX</param>
    ''' <param name="TBLy">    (OUT)�e�[�u���ʒuy</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function XYTableMoveSetBlock(ByRef MODE As Short, ByVal iPos As Short, ByVal Ino As Short, ByVal Bn As Short, _
                                         ByVal ChipSize As Double, ByVal KJPosX As Double, ByVal KJPosY As Double, _
                                         ByRef TBLx As Double, ByRef TBLy As Double) As Short

        Dim wkx As Double
        Dim wky As Double
        Dim i As Short
        Dim iBlock As Short = 0
        Dim dIntv As Double = 0
        Dim ZZx As Double
        Dim ZZy As Double
        Dim r As Short
        Dim strMSG As String

        Try
            Select Case MODE
                Case 0                                                  ' ��1��ٰ�߁A��1��ʒu(�`�b�v�T�C�Y�e�B�[�`���O�p)
                    ' �e�[�u���ʒu���1��ٰ�ߑ�1��ʒu�ɐݒ肷��
                    TBLx = KJPosX + ZRPosX(0)                           ' X = ��1��ʒuX + ��1��ʒu�����X
                    TBLy = KJPosY + ZRPosY(0)                           ' Y = ��1��ʒuY + ��1��ʒu�����Y

                Case 1                                                  ' ��1��ٰ�߁A�ŏI��ʒu(�`�b�v�T�C�Y�e�B�[�`���O�p)
                    ' �e�[�u���ʒu���1��ٰ�ߍŏI�u���b�N�ʒu�ɐݒ肷��
                    If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���т�X���� ?
                        wkx = ZRPosX(0)                                 ' X = ��1��ʒu�̂����X 
                        wky = (ChipSize * (Bn - 1)) + ZRPosY(0)         ' Y = �`�b�v�T�C�Y*(�u���b�N��-1) + ��1��ʒu�̂����Y
                    Else
                        wkx = (ChipSize * (Bn - 1)) + ZRPosX(0)         ' X = �`�b�v�T�C�Y*(�u���b�N��-1) + ��1��ʒu�̂����X
                        wky = ZRPosY(0)                                 ' Y = ��1��ʒu�̂����Y 
                    End If

                    ' �e�[�u���ړ��ʒu�ݒ�
                    Call Sub_TblPosXY(wkx, wky)
                    TBLx = KJPosX + wkx + ZRPosX(iPos)                  ' X = ��1��ʒuX + �v�Z�lX + ���݂̂����X
                    TBLy = KJPosY + wky + ZRPosY(iPos)                  ' Y = ��1��ʒuY + �v�Z�lY + ���݂̂����Y

                Case 2                                                  ' ��1��ٰ�߁A�ŏI��ʒu(�X�e�[�W�O���[�v�Ԋu�e�B�[�`���O�p)
                    ' �e�[�u���ʒu���1��ٰ�߂̍ŏI�u���b�N�ʒu�ɐݒ肷��
                    For i = 0 To iPos - 1                               ' ����ʂ����Z����
                        ZZx = ZZx + ZRPosX(i)
                        ZZy = ZZy + ZRPosY(i)
                    Next i

                    If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���т�X���� ?
                        wkx = ZZx                                       ' X = ��1��ʒu�̂����X 
                        wky = (ChipSize * Bn) + ZRPosY(0)               ' Y = �`�b�v�T�C�Y*(�u���b�N��-1) + ��1��ʒu�̂����Y
                    Else
                        wkx = (ChipSize * Bn) + ZRPosX(0)               ' X = �`�b�v�T�C�Y*(�u���b�N��-1) + ��1��ʒu�̂����X
                        wky = ZZy                                       ' Y = ��1��ʒu�̂����Y 
                    End If

                    ' �e�[�u���ړ��ʒu�ݒ�
                    Call Sub_TblPosXY(wkx, wky)
                    TBLx = KJPosX + wkx + ZRPosX(iPos)                  ' X = ��1��ʒuX + �v�Z�lX + ���݂̂����X
                    TBLy = KJPosY + wky + ZRPosY(iPos)                  ' Y = ��1��ʒuY + �v�Z�lY + ���݂̂����Y

                Case 3                                                  ' ��2��ٰ�߁A�擪��ʒu(�X�e�[�W�O���[�v�Ԋu�e�B�[�`���O�p)
                    ' �e�[�u���ʒu���2��ٰ�߂̐擪�u���b�N�ʒu�ɐݒ肷��
                    For i = 0 To iPos - 1                               ' ����ʂ����Z����
                        ZZx = ZZx + ZRPosX(i)
                        ZZy = ZZy + ZRPosY(i)
                    Next i
                    dIntv = mdStepInterval(1)                           ' �X�e�[�W�O���[�v�Ԋu 

                    If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���т�X���� ?
                        wkx = ZZx                                       ' X = ��1��ʒu�̂����X 
                        wky = (ChipSize * Bn) + dIntv + ZRPosY(0)       ' Y = �`�b�v�T�C�Y*�u���b�N�� + �O���[�v�Ԋu + ��1��ʒu�̂����Y
                    Else
                        wkx = (ChipSize * Bn) + dIntv + ZRPosX(0)       ' X = �`�b�v�T�C�Y*�u���b�N�� + �O���[�v�Ԋu + ��1��ʒu�̂����X
                        wky = ZZy                                       ' Y = ��1��ʒu�̂����Y 
                    End If

                    ' �e�[�u���ړ��ʒu�ݒ�
                    Call Sub_TblPosXY(wkx, wky)
                    TBLx = KJPosX + wkx + ZRPosX(iPos)                  ' X = ��1��ʒuX + �v�Z�lX + ���݂̂����X
                    TBLy = KJPosY + wky + ZRPosY(iPos)                  ' Y = ��1��ʒuY + �v�Z�lY + ���݂̂����Y

                Case 4                                                  ' �X�e�b�v�I�t�Z�b�g�e�B�[�`���O�p
                    ' �e�[�u���ʒu���ŏI�u���b�N�ʒu�ɐݒ肷��
                    For i = 0 To iPos - 1                               ' ����ʂ����Z����
                        ZZx = ZZx + ZRPosX(i)
                        ZZy = ZZy + ZRPosY(i)
                    Next i

                    ' �X�e�[�W�O���[�v�Ԋu = (�X�e�[�W�O���[�v�� -1) * �X�e�[�W�O���[�v�Ԋu
                    dIntv = (typPlateInfo.intGroupCntInBlockYStage - 1) * mdStepInterval(1)

                    If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���т�X���� ?
                        wkx = ZZx                                       ' X = ��1��ʒu�̂����X 
                        wky = (ChipSize * Bn) + dIntv + ZRPosY(0)       ' Y = �`�b�v�T�C�Y*�u���b�N�� + �O���[�v�Ԋu + ��1��ʒu�̂����Y
                    Else
                        wkx = (ChipSize * Bn) + dIntv + ZRPosX(0)       ' X = �`�b�v�T�C�Y*�u���b�N�� + �O���[�v�Ԋu + ��1��ʒu�̂����X
                        wky = ZZy                                       ' Y = ��1��ʒu�̂����Y 
                    End If

                    ' �e�[�u���ړ��ʒu�ݒ�
                    Call Sub_TblPosXY(wkx, wky)
                    TBLx = KJPosX + wkx + ZRPosX(iPos)                  ' X = ��1��ʒuX + �v�Z�lX + ���݂̂����X
                    TBLy = KJPosY + wky + ZRPosY(iPos)                  ' Y = ��1��ʒuY + �v�Z�lY + ���݂̂����Y

            End Select

            ' �e�[�u���ړ�
            r = Form1.System1.XYtableMove(gSysPrm, TBLx, TBLy)
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.XYTableMoveSetBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[ 
        End Try
    End Function
#End Region

#Region "�e�[�u���ʒu�v�Z�T�u���[�`��"
    '''=========================================================================
    ''' <summary>�e�[�u���ʒu�v�Z�T�u���[�`��</summary>
    ''' <param name="wkx">    (I/O)�e�[�u���ʒuX</param>
    ''' <param name="wky">    (I/O)�e�[�u���ʒuy</param>
    '''=========================================================================
    Private Sub Sub_TblPosXY(ByRef wkx As Double, ByRef wky As Double)

        Dim strMSG As String

        Try

            If (typPlateInfo.intResistDir = 0) Then                     ' �`�b�v���т�X���� ?
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 'x��, y��
                        wky = wky
                    Case 1 'x��, y��
                        wky = wky
                    Case 2 'x��, y��
                        wky = -wky
                    Case 3 'x��, y��
                        wky = -wky
                End Select
            Else
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 'x��, y��
                        wkx = wkx
                    Case 1 'x��, y��
                        wkx = -wkx
                    Case 2 'x��, y��
                        wkx = wkx
                    Case 3 'x��, y��
                        wkx = -wkx
                End Select
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmTxTyTeach.XYTableMoveSetBlock() TRAP ERROR = " + ex.Message
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
        'Call SubBtnHI_Click(stJOG) ' V1.13.0.0�M  ###265
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
    Private Sub frmTxTyTeach_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        JogKeyDown(e)                   'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode                 'V6.0.0.0�J
        Console.WriteLine("frmTxTyTeach.frmTxTyTeach_KeyDown()")

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V6.0.0.0�K        GrpMain.Enabled = False                                         ' ###085
        'V6.0.0.0�K       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0�K
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then           ' 5�� (KeyCode = 101(&H65)
            'Call BtnHI_Click(sender, e)                                ' V1.13.0.0�M HI�{�^�� ON/OFF
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
    Private Sub frmTxTyTeach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        JogKeyUp(e)                     'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0�J

        Console.WriteLine("frmTxTyTeach.frmTxTyTeach_KeyKeyUp()")
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

    '----- ###228�� -----
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
            strMSG = "frmTxTyTeach.Timer1_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
#End If
    End Sub
#End Region
    '----- ###228�� -----

End Class
