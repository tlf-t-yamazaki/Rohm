'===============================================================================
'   Description  : �g���~���O���s���ꎞ��~����
'
'   Copyright(C) : OMRON LASERFRONT INC. 2012
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DllSysPrm.SysParam
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmFineAdjust
    Inherits System.Windows.Forms.Form
    Implements ICommonMethods              'V6.0.0.0�J

    '========================================================================================
    '   �萔�E�ϐ���`
    '========================================================================================
#Region "�萔�E�ϐ���`"
    '===========================================================================
    '   �萔��`
    '===========================================================================
    Public Const MOVE_NEXT As Integer = 0
    Public Const MOVE_NOT As Integer = 1

    '----- �������[�h -----
    Private Const MD_INI As Integer = 0                                 ' �����G���g�����[�h
    Private Const MD_CHK As Integer = 1                                 ' �p���G���g�����[�h

    '===========================================================================
    '   �����o�ϐ���`
    '===========================================================================
    Private m_BlockSizeX As Double
    Private m_BlockSizeY As Double
    Private m_bpOffX As Double
    Private m_bpOffY As Double
    Private m_sysPrm As SYSPARAM_PARAM
    Private stJOG As JOG_PARAM                                          ' �����(BP��JOG����)�p�p�����[�^ '###046(Globals.vb�̋��ʊ֐����g�p)
    Private dblTchMoval(3) As Double                                    ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))
    Private mExit_flg As Short                                          ' ����
    Private mMd As Integer = MD_INI                                     ' �������[�h
    Private m_TenKeyFlg As Boolean = False                              ' ###139
    Private m_LaserOnOffFlag As Boolean = False                         ' ###237

    Private _procBlockSelected As Boolean                               'V4.12.2.0�B
#End Region

    '========================================================================================
    '   ���\�b�h��`
    '========================================================================================
#Region "�����l�ݒ菈��"
    '''=========================================================================
    ''' <summary>�����l�ݒ菈��</summary>
    ''' <param name="SysPrm"></param>
    ''' <param name="digL"></param>
    ''' <param name="digH"></param>
    ''' <param name="curPltNo"></param>
    ''' <param name="curBlkNo"></param>
    '''=========================================================================
    Public Sub SetInitialData(ByRef SysPrm As SYSPARAM_PARAM,
                              ByVal digL As Integer, ByVal digH As Integer,
                              ByVal curPltNo As Integer, ByVal curBlkNo As Integer,
                              ByVal curPltNoX As Integer, ByVal curPltNoY As Integer,
                              ByVal curBlkNoX As Integer, ByVal curBlkNoY As Integer)   '#4.12.2.0�E
        '#4.12.2.0�E    Public Sub SetInitialData(ByRef SysPrm As SYSPARAM_PARAM,
        '#4.12.2.0�E                        ByVal digL As Integer, ByVal digH As Integer,
        '#4.12.2.0�E                        ByRef curPltNo As Integer, ByRef curBlkNo As Integer)

        Try
            CbDigSwH.SelectedIndex = digH
            CbDigSwL.SelectedIndex = digL
            gCurBlockNo = curBlkNo
            gCurPlateNo = curPltNo
            '#4.12.2.0�E                 ��
            Globals_Renamed.gCurPlateNoX = curPltNoX
            Globals_Renamed.gCurPlateNoY = curPltNoY
            Globals_Renamed.gCurBlockNoX = curBlkNoX
            Globals_Renamed.gCurBlockNoY = curBlkNoY
            '#4.12.2.0�E                 ��
            m_sysPrm = SysPrm
            'gFrmEndStatus = cFRS_NORMAL

            If (gbChkboxHalt = True) Then                                       '###009
                BtnADJ.Text = "ADJ ON"                                          '###009
                BtnADJ.BackColor = System.Drawing.Color.Yellow                  '###009
            Else                                                                '###009
                BtnADJ.Text = "ADJ OFF"                                         '###009
                BtnADJ.BackColor = System.Drawing.SystemColors.Control          '###009
            End If                                                              '###009

            ' ���x�����ݒ�(���{��/�p��)
            'BtnEdit.Text = LBL_FINEADJ_001                                      ' "�f�[�^�ҏW" ###014
            '-----###204 -----
            'Me.Label3.Text = LBL_FINEADJ_002                                    ' "����" 
            'CbDigSwH.Items(0) = LBL_FINEADJ_003
            'CbDigSwH.Items(1) = LBL_FINEADJ_004
            'CbDigSwH.Items(2) = LBL_FINEADJ_005
            '-----###204 -----
            '----- ###268�� -----
            '�uTen Key On/Off�v�{�^���̏����l���V�X�p�����ݒ肷��
            If (giTenKey_Btn = 0) Then                                          ' �ꎞ��~��ʂł́uTen Key On/Off�v�{�^���̏����l(0:ON(����l), 1:OFF)
                gbTenKeyFlg = True
                BtnTenKey.Text = "Ten Key On"
                BtnTenKey.BackColor = System.Drawing.Color.Pink
                'V6.0.1.0�J                Form1.SetTrimMapVisible(False)                          'V6.0.1.0�I
            Else
                gbTenKeyFlg = False
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control
            End If
            'V6.0.1.0�J            Form1.SetMapOnOffButtonEnabled(Not gbTenKeyFlg)             'V4.12.2.0�@

            'gbTenKeyFlg = True                                                 ' �uTen Key On�v��� ###242
            '----- ###268�� -----
            '----- ###269�� -----
            ' �ꎞ��~��ʂł̃V�X�p���uBP�I�t�Z�b�g��������/���Ȃ��v�w��ɂ����{�^������ݒ肷��
            Call Sub_SetBtnArrowEnable()
            '----- ###269�� -----

            '----- V6.0.3.0_50�� -----
            stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
                                         BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            '----- V6.0.3.0_50�� -----

            'for�@��R����
            '�ڕW�l
            '�J�b�g�I�t
            '�X�s�[�h
            '���H�����ԍ�
            'next
            'txtExCamPosX.Text = m_sysPrm.stDEV.gfExCmX.ToString
#If False Then  'V6.0.1.3�@
            _procBlockSelected = (0 < Globals_Renamed.GetProcBlockCount())  'V4.12.2.0�B
            Form1.SetMapBorder(gCurBlockNo, Color.Black)                    'V4.12.2.0�@
#Else
            ' �v���[�g�Ή�    'V6.0.1.3�@
            'If (Form1.ChkMapEnable()) Then
            _procBlockSelected = Form1.TrimMapSelected
                If (_procBlockSelected) Then
                    Form1.GetNextSelectBlock(gCurPlateNo, gCurBlockNo, gCurPlateNo, gCurBlockNo)
                End If
                Form1.SetMapBorder(gCurPlateNo, gCurBlockNo, Color.Black)

            'End If
#End If
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.Form_Initialize_Renamed() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub
#End Region
    '-----###269��-----
#Region "���{�^����������/�񊈐�������"
    '''=========================================================================
    ''' <summary>���{�^����������/�񊈐�������</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub Sub_SetBtnArrowEnable()

        Dim bFlg As Boolean
        Dim strMsg As String

        Try
            '  �ꎞ��~��ʂł̃V�X�p���uBP�I�t�Z�b�g��������/���Ȃ��v�w��ɂ����{�^������ݒ肷��
            If (giBpAdj_HALT = 0) Then                                          ' BP�I�t�Z�b�g�������� ?
                bFlg = True

                Form1.Instance.SetActiveJogMethod(AddressOf Me.JogKeyDown,
                                                  AddressOf Me.JogKeyUp,
                                                  AddressOf Me.MoveToCenter)    'V6.0.0.0�I
            Else                                                                ' BP�I�t�Z�b�g�������Ȃ�
                bFlg = False
                gbTenKeyFlg = False
                BtnTenKey.Enabled = False                                       '�uTen Key Off�v�{�^���񊈐���
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control

                Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)    'V6.0.0.0�I
            End If

            ' ���{�^��������/�񊈐���
            BtnJOG_0.Enabled = bFlg
            BtnJOG_1.Enabled = bFlg
            BtnJOG_2.Enabled = bFlg
            BtnJOG_3.Enabled = bFlg
            BtnJOG_4.Enabled = bFlg
            BtnJOG_5.Enabled = bFlg
            BtnJOG_6.Enabled = bFlg
            BtnJOG_7.Enabled = bFlg
            BtnHI.Enabled = bFlg

            ' Moving Pitch������/�񊈐���
            GrpPithPanel.Enabled = bFlg

        Catch ex As Exception
            strMsg = "frmFineAdjust.Sub_SetBtnArrowEnable() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- ###269��-----
    '----- ###260��-----
#Region "�^�C�}�[��~"
    '''=========================================================================
    ''' <summary>�^�C�}�[��~</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function Sub_StopTimer() As Integer

        TmKeyCheck.Enabled = False

    End Function
#End Region
    '----- ###260��-----

#Region "�^�C�}�[�J�n"
    '''=========================================================================
    ''' <summary>�^�C�}�[�J�n</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function Sub_StartTimer() As Integer

        TmKeyCheck.Enabled = True

    End Function
#End Region

#Region "�X�e�[�W�|�W�V�����擾����"
    '''=========================================================================
    ''' <summary>�X�e�[�W�|�W�V�����擾�����i���s��Ɏ擾�j</summary>
    '''=========================================================================
    Public Sub GetStagePosInfo(ByRef pltNo As Integer, ByRef blkNo As Integer,
                               ByRef pltNoX As Integer, ByRef pltNoY As Integer,
                               ByRef blkNoX As Integer, ByRef blkNoY As Integer)    '#4.12.2.0�E
        '#4.12.2.0�E    Public Sub GetStagePosInfo(ByRef pltNo As Integer, ByRef blkNo As Integer)
        Try
            pltNo = gCurPlateNo
            blkNo = gCurBlockNo
            '#4.12.2.0�E                 ��
            pltNoX = Globals_Renamed.gCurPlateNoX
            pltNoY = Globals_Renamed.gCurPlateNoY
            blkNoX = Globals_Renamed.gCurBlockNoX
            blkNoY = Globals_Renamed.gCurBlockNoY
            '#4.12.2.0�E                 ��
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.GetStagePosInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�I���߂�l�擾����"
    '''=========================================================================
    ''' <summary>�I���߂�l�擾�����i���s��Ɏ擾�j</summary>
    '''=========================================================================
    Public Function GetReturnVal() As Integer
        Try
            Return (mExit_flg)

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.GetReturnVal() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

    '========================================================================================
    '   ��ʏ���
    '========================================================================================
#Region "�t�H�[�����[�h����"

    'V6.0.0.0�J    Private Sub frmFineAdjust_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
    'V6.0.0.0�JDim r As Integer                                                ' ###237
    'V6.0.0.0�J        r = r

    'V6.0.0.0�J    End Sub

    '''=========================================================================
    ''' <summary>�t�H�[�����[�h����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim stPos As System.Drawing.Point
        Dim stGetPos As System.Drawing.Point
        Dim r As Integer                                                ' ###237
        Dim strMsg As String
        Dim stSize As System.Drawing.Size

        Try
            ' �\���ʒu�̒���
            stPos = Form1.Text4.PointToScreen(stGetPos)
            stPos.X = stPos.X - 2
            stPos.Y = stPos.Y - 2
            Me.Location = stPos

            ' BpOffset�̌��ݒl�ݒ�
            GetBpOffset(m_bpOffX, m_bpOffY)
            txtBpOffX.Text = m_bpOffX.ToString
            txtBpOffY.Text = m_bpOffY.ToString

            ' BlockSize�̌��ݒl�擾
            GetBlockSize(m_BlockSizeX, m_BlockSizeY)

            '----- ###139�� -----
            'V6.0.2.0�A��
            '' ���C����ʂ́u���Y�O���t�\��/��\���{�^���v���瓖��ʂ́u���Y�O���t�\��/��\���{�^���v��ݒ肷��
            'If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
            '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
            '    chkDistributeOnOff.Checked = Form1.chkDistributeOnOff.Checked
            '    'V4.0.0.0�K
            '    If gKeiTyp <> KEY_TYPE_RS Then
            '        GrpDistribute.Visible = True                        '�u���Y�O���t�{�^���v�\��
            '    Else
            '        GrpDistribute.Visible = False                        '�u���Y�O���t�{�^���v�\��
            '    End If
            '    'V4.0.0.0�K
            'Else
            '    GrpDistribute.Visible = False                       '�u���Y�O���t�{�^���v��\��
            'End If
            'V6.0.2.0�A��
            '----- ###139�� -----

            '----- ###237�� -----
            ' ���H�����ԍ���ݒ肷��(FL��)
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                Call QRATE(stCND.Freq(ADJ_CND_NUM))                     ' Q���[�g�ݒ�(KHz)
                r = FLSET(FLMD_CNDSET, ADJ_CND_NUM)                     ' ���H�����ԍ��ݒ�(�ꎞ��~��ʗp)
            Else
                Call QRATE(gSysPrm.stDEV.gfLaserQrate)                  ' Q���[�g�ݒ�(KHz) �����[�U�����pQ���[�g��ݒ�
            End If
            '----- ###237�� -----

            '----- V1.23.0.0�E�� -----
            ' �g���~���O�f�[�^�Łu��f�B���C�v�w�莞��
            ' �uBlock Move�v�u�f�[�^�ҏW�v�uMoveMode�v�͖����Ƃ���
            If (typPlateInfo.intDelayTrim = 2) Then                     ' ��f�B���C(�f�B���C�g�����Q) ?
                BlockMove.Enabled = False
                BtnEdit.Enabled = False
                CbDigSwL.Enabled = False
            Else
                BlockMove.Enabled = True
                BtnEdit.Enabled = True
                CbDigSwL.Enabled = True
            End If
            '----- V1.23.0.0�E�� -----

            'V4.0.0.0-84��
            If gSysPrm.stTMN.giKeiTyp = KEY_TYPE_RS Then
                btnClose.Visible = True
                BtnSTART.Visible = True
                stSize.Height = Me.Height + 100
                stSize.Width = Me.Width
                Me.Size = stSize
            Else
                btnClose.Visible = False
                BtnSTART.Visible = False
            End If
            'V4.0.0.0-84��

            'V5.0.0.9�O                  ��
            If (1 < giStartBlkAss) Then
                Dim x, y As Integer : Form1.Get_StartBlkNum(x, y)

                ' �g���~���O�J�n�u���b�N�ԍ�X�R���{�{�b�N�X�̐ݒ�
                With Me.cmbBlockMoveX
                    .BeginUpdate()
                    .Items.Clear()
                    For Idx As Integer = 1 To typPlateInfo.intBlockCntXDir
                        .Items.Add(Idx.ToString(0).PadLeft(3))
                    Next Idx
                    .SelectedIndex = (x - 1)
                    .EndUpdate()
                End With

                ' �g���~���O�J�n�u���b�N�ԍ�Y�R���{�{�b�N�X�̐ݒ�
                With Me.cmbBlockMoveY
                    .BeginUpdate()
                    .Items.Clear()
                    For Idx As Integer = 1 To typPlateInfo.intBlockCntYDir
                        .Items.Add(Idx.ToString(0).PadLeft(3))
                    Next Idx
                    .SelectedIndex = (y - 1)
                    .EndUpdate()
                End With
            Else
                Me.lblBlockMoveX.Visible = False
                Me.cmbBlockMoveX.Enabled = False
                Me.cmbBlockMoveX.Visible = False

                Me.lblBlockMoveY.Visible = False
                Me.cmbBlockMoveY.Enabled = False
                Me.cmbBlockMoveY.Visible = False
            End If
            'V5.0.0.9�O                  ��

            ' �t�H�[�J�X�̐ݒ�(����ɂ���ăe���L�[�̃C�x���g���擾�ł���)
            Me.KeyPreview = True
            Me.Activate()                                               ' ###046

        Catch ex As Exception
            strMsg = "frmFineAdjust.frmFineAdjust_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�t�H�[�����\�����ꂽ���̏���"
    '''=========================================================================
    ''' <summary>�t�H�[�����\�����ꂽ���̏���</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            '----- V6.0.3.0_50�� -----
            'V6.0.0.0�K                  ��
            ' Form.Load() �� Show()����ShowDialog()���Ƃň�x�E�s�x�Ɣ����󋵂��قȂ邽�߁A�Ă΂���ɉe�����󂯂Ȃ�Shown()�ł����Ȃ�
            'stJOG.TenKey = New Button() {BtnJOG_0, BtnJOG_1, BtnJOG_2, BtnJOG_3,
            '                             BtnJOG_4, BtnJOG_5, BtnJOG_6, BtnJOG_7, BtnHI}
            'V6.0.0.0�K                  ��
            '----- V6.0.3.0_50�� -----

            'V6.0.2.0�A��
            ' ���C����ʂ́u���Y�O���t�\��/��\���{�^���v���瓖��ʂ́u���Y�O���t�\��/��\���{�^���v��ݒ肷��
            If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                chkDistributeOnOff.Checked = Form1.chkDistributeOnOff.Checked
                'V4.0.0.0�K
                If gKeiTyp <> KEY_TYPE_RS Then
                    GrpDistribute.Visible = True                        '�u���Y�O���t�{�^���v�\��
                Else
                    GrpDistribute.Visible = False                        '�u���Y�O���t�{�^���v�\��
                End If
                'V4.0.0.0�K
            Else
                GrpDistribute.Visible = False                       '�u���Y�O���t�{�^���v��\��
            End If
            'V6.0.2.0�A��

            ' �ꎞ��~��ʏ������C����Call����
            mExit_flg = 0                                               ' �I���t���O = 0
            Call ZCONRST()                                              ' �ݿ�ٷ�ׯ�����
            TmKeyCheck.Interval = 10
            TmKeyCheck.Enabled = True                                   ' �^�C�}�[�J�n
            lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0�D
            gfrmAdjustDisp = 1                                           'V5.0.0.1-29
            Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmFineAdjust.frmFineAdjust_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return�l = ��O�G���[
        End Try

        gbExitFlg = True                                                ' �I���t���OON
        Call LASEROFF()                                                 ' ###237
        gfrmAdjustDisp = 0                                           'V5.0.0.1-29
        Me.Close()
    End Sub
#End Region

#Region "���C���������s"
    ''' <summary>���C���������s</summary>
    ''' <returns>���s����</returns>
    ''' <remarks>'V6.0.0.0�L</remarks>
    Public Function Execute() As Integer Implements ICommonMethods.Execute
        ' DO NOTHING
    End Function
#End Region

#Region "�L�[���̓`�F�b�N�^�C�}�[����"
    '''=========================================================================
    ''' <summary>�L�[���̓`�F�b�N�^�C�}�[����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub TmKeyCheck_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TmKeyCheck.Tick

        Dim r As Short
        Dim strMSG As String

        Try
            ' �L�[���̓`�F�b�N����
            TmKeyCheck.Enabled = False                                  ' �^�C�}�[��~
            ' �t�H�[�����\������Ă��Ȃ��Ƃ��ɂ́A�^�C�}�[�͉������Ȃ�(��ʂ͂Ȃ��̂Ƀ^�C�}�[�������Ă��Ȃ����Ƃ�����)
            If (IsNothing(gObjADJ) = False) Then
                If (gObjADJ.Visible = False) Then
                    Return
                End If
            End If

            r = MainProc(mMd)                                           ' �ꎞ��~��ʏ���
            If (r = cFRS_NORMAL) Then                                   ' ����߂� 
                TmKeyCheck.Enabled = True                               ' �^�C�}�[�J�n
                Return
            End If

            '----- ###219�� -----
            ' Z �L�[�����Ȃ� Z On/OFF���� 
            If (r = cFRS_ERR_Z) Then                                    ' Z SW���� ?
                If (stJOG.bZ = True) Then                               ' Z ON ? 
                    r = PROBON()
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ烁�b�Z�[�W��\������
                    If (r <> cFRS_NORMAL) Then
                        mExit_flg = r                                   ' �G���[���^�[�� 
                        Return
                    End If
                    '----- V1.13.0.0�C�� -----
                    ' Z2�����ړ�����B
                    If IsUnderProbe() Then
                        r = Z2move(Z2ON)                                ' Z2������w��
                        If (r <> cFRS_NORMAL) Then
                            mExit_flg = r                               ' �G���[���^�[�� 
                            Return
                        End If
                        Call ZSTOPSTS2()                                ' Z2�������~�҂�
                    End If
                    '----- V1.13.0.0�C�� -----

                Else                                                    ' Z OFF
                    '----- V1.13.0.0�C�� -----
                    ' Z2��ҋ@�ʒu�Ɉړ�
                    If IsUnderProbe() Then                              ' �����v���[�u�L��̏ꍇ
                        r = Z2move(Z2OFF)                               ' Z2��DOWN�ʒu�Ɉړ�
                        If (r <> cFRS_NORMAL) Then                      ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                            mExit_flg = r                               ' �G���[���^�[�� 
                            Return
                        End If
                        Call ZSTOPSTS2()
                    End If
                    '----- V1.13.0.0�C�� -----
                    r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)         ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ���
                    ' �G���[�Ȃ烁�b�Z�[�W��\�����ăG���[���^�[��
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ烁�b�Z�[�W��\������
                    If (r <> cFRS_NORMAL) Then
                        mExit_flg = r                                   ' �G���[���^�[�� 
                        Return
                    End If
                End If

                ' Z�����v�̓_��/����
                If (stJOG.bZ = True) Then
                    Call LAMP_CTRL(LAMP_Z, True)
                Else
                    Call LAMP_CTRL(LAMP_Z, False)
                End If

                TmKeyCheck.Enabled = True                               ' �^�C�}�[�J�n
                Return
            End If
            '----- ###219�� -----

            ' START/RESET�L�[�����܂��̓G���[�Ȃ�I��
            If (r = cFRS_ERR_START) Then r = cFRS_NORMAL

            mExit_flg = r

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmFineAdjust.TmKeyCheck_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            mExit_flg = cERR_TRAP                                       ' Return�l = ��O�G���[
        End Try

        gbExitFlg = True                                                ' �I���t���OON
        Call LASEROFF()                                                 ' ###237

        'V5.0.0.1�@��
        If gSysPrm.stTMN.giKeiTyp = KEY_TYPE_RS Then
            'V5.0.0.9�O            If (giStartBlkAss = 1) Then
            If (giStartBlkAss <> 0) Then    'V5.0.0.9�O
                Call Form1.Set_StartBlkNum(gCurBlockNo, 1)
            End If
        End If
        'V5.0.0.1�@��

        gfrmAdjustDisp = 0                                           'V5.0.0.1-29
        Me.Close()
    End Sub
#End Region

#Region "���C������"
    '''=========================================================================
    ''' <summary>���C������"</summary>
    ''' <param name="Md">(I/O)�������[�h
    ''' �@�@�@�@�@�@�@�@�@�@�@MD_INI=�����G���g��, MD_CHK=�p���G���g��</param>
    ''' <returns>cFRS_NORMAL   = OK(START��)
    '''          cFRS_ERR_RST  = Cancel(RESET��)
    '''          -1�ȉ�        = �G���[</returns>
    '''=========================================================================
    Private Function MainProc(ByRef Md As Integer) As Short

        Dim mdAdjx As Double = 0.0                                      ' ��ެ�ĈʒuX(���g�p)
        Dim mdAdjy As Double = 0.0                                      ' ��ެ�ĈʒuY(���g�p)
        Dim r As Short
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            If (Md = MD_INI) Then                                       ' �����G���g��
                ' JOG�p�����[�^�ݒ� 
                stJOG.Md = MODE_BP                                      ' ���[�h(1:BP�ړ�)
                stJOG.Md2 = MD2_BUTN                                    ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
                '                                                       ' �L�[�̗L��(1)/����(0)�w��
                'stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START
                stJOG.Opt = CONSOLE_SW_RESET + CONSOLE_SW_START + CONSOLE_SW_ZSW ' ###219
                stJOG.PosX = 0.0                                        ' BP X�ʒu(BP�̾��X)
                stJOG.PosY = 0.0                                        ' BP Y�ʒu(BP�̾��Y)
                stJOG.BpOffX = mdAdjx + m_bpOffX                        ' BP�̾��X 
                stJOG.BpOffY = mdAdjy + m_bpOffY                        ' BP�̾��Y 
                stJOG.BszX = m_BlockSizeX                               ' ��ۯ�����X 
                stJOG.BszY = m_BlockSizeY                               ' ��ۯ�����Y
                txtBpOffX.ShortcutsEnabled = False                      ' ###047 �E�N���b�N���j���[��\�����Ȃ� 
                txtBpOffY.ShortcutsEnabled = False                      '  
                stJOG.TextX = txtBpOffX                                 ' BP X�ʒu�\���p÷���ޯ��
                stJOG.TextY = txtBpOffY                                 ' BP Y�ʒu�\���p÷���ޯ��
                stJOG.cgX = m_bpOffX                                    ' �ړ���X (BP�̾��X)
                stJOG.cgY = m_bpOffY                                    ' �ړ���Y (BP�̾��Y)
                stJOG.BtnHI = BtnHI                                     ' HI�{�^��
                stJOG.BtnZ = BtnZ                                       ' Z�{�^��
                stJOG.BtnSTART = BtnSTART                               ' START�{�^��
                stJOG.BtnRESET = BtnRESET                               ' RESET�{�^��
                stJOG.BtnHALT = BtnHALT                                 ' HALT�{�^��
                Call JogEzInit(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
                stJOG.Flg = -1                                          ' �e��ʂ�OK/Cancel���݉����׸�
                Md = MD_CHK
                stJOG.bZ = False                                        ' Jog��Z�L�[��� = Z Off ###219
                Call LAMP_CTRL(LAMP_Z, False)                           ' ###219 
            End If

STP_RETRY:
            'Call Me.Focus()                                            ' �� ��������ƃe���L�[��KeyUp/KeyDown�C�x���g�������Ă��Ȃ��Ȃ�

            ' ����~���`�F�b�N
            r = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
            If (r <> cFRS_NORMAL) Then                                  ' ����~�����o ?
                Return (r)
            End If

            '----- ###209�� -----
            ' �J�o�[���m�F����(SL436R���Ŏ蓮���[�h��)
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (bFgAutoMode = False) Then
                Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A
                r = FrmReset.Sub_CoverCheck()
                If (r < cFRS_NORMAL) Then                               ' ����~�����o ?
                    Return (r)
                End If
            ElseIf gKeiTyp = KEY_TYPE_RS Then   'SL436S�̏ꍇ��BP������ʂ��\������Ă�����`�F�b�N����
                'Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A
                'r = FrmReset.Sub_CoverCheck()
                'If (r < cFRS_NORMAL) Then                               ' ����~�����o ?
                '    Return (r)
                'End If
            End If
            '----- ###209�� -----

            ' �R���\�[���L�[���̓��͑҂�
            'stJOG.Flg = -1                                             ' �e��ʂ�OK/Cancel���݉����׸�
            r = JogEzMove_Ex(stJOG, gSysPrm, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)
            If (r < cFRS_NORMAL) Then                                   ' �G���[�Ȃ�I��
                Return (r)
            End If

            ' �R���\�[���L�[�`�F�b�N
            If (r = cFRS_ERR_START) Then                                ' START SW���� ?
                ' DIG-SW�ݒ�
                Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex)
                ' BP�I�t�Z�b�g�X�V(�^�C�~���O�ɂ���ċ󔒂œ����Ă���ꍇ�g���b�v�G���[�ƂȂ�̂Ń`�F�b�N���� ###014)
                If (txtBpOffX.Text <> "") And (txtBpOffY.Text <> "") Then
                    Call SetBpOffset(Double.Parse(txtBpOffX.Text), Double.Parse(txtBpOffY.Text))
                End If
                Return (cFRS_ERR_START)

            ElseIf (r = cFRS_ERR_RST) Then                              ' RESET SW���� ?
                Return (cFRS_ERR_RST)

                '----- ###219�� -----
            ElseIf (r = cFRS_ERR_Z) Then                                ' Z SW���� ?
                Return (cFRS_ERR_Z)
                '----- ###219�� -----
            End If

            'Loop While (stJOG.Flg = -1)

            '' ����ʂ���OK/Cancel���݉����Ȃ�r�ɖߒl��ݒ肷��
            'If (stJOG.Flg <> -1) Then
            '    r = stJOG.Flg
            'End If

STP_END:
            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmFineAdjust.MainProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

    '========================================================================================
    '   ���C����ʂ̃{�^������������
    '========================================================================================
#Region "ADJ���݉���������"
    '''=========================================================================
    ''' <summary>ADJ���݉��������� ###009</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnADJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnADJ.Click
        Dim strMSG As String

        Try
            If (BtnADJ.Text = "ADJ OFF") Then
                gbChkboxHalt = True
                BtnADJ.Text = "ADJ ON"
                BtnADJ.BackColor = System.Drawing.Color.Yellow
            Else
                gbChkboxHalt = False
                BtnADJ.Text = "ADJ OFF"
                BtnADJ.BackColor = System.Drawing.SystemColors.Control
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.BtnADJ_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�O�u���b�N�ړ�����(Prev�{�^��������)"
    '''=========================================================================
    ''' <summary>�O�u���b�N�ړ�����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub btnBlkPrvMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlkPrvMove.Click

        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer

        Try
            ' �擪�v���[�g�A�擪�u���b�N�ł���Έړ����Ȃ�
            TmKeyCheck.Enabled = False                                  ' �^�C�}�[��~
            If (gCurPlateNo <= 1) And (gCurBlockNo <= 1) Then
                GoTo STP_END
            ElseIf (gCurBlockNo <= 1) Then
                ' �擪�u���b�N�ł���Έ�O�̃v���[�g�̍ŏI�u���b�N��
                workPlateNo = gCurPlateNo - 1
                workBlockNo = GetBlockCnt()
            Else
                ' �u���b�N��1�O�Ɉړ�����
                workBlockNo = gCurBlockNo - 1
                workPlateNo = gCurPlateNo
            End If

            'V4.12.2.0�B             ��
            If (_procBlockSelected) Then
#If Not True Then   'V6.0.1.3�@
                workBlockNo = Globals_Renamed.GetPrevProcBlock(workBlockNo)
                If (workBlockNo < 1) Then
                    If (1 < gCurPlateNo) Then
                        ' �擪�I���u���b�N�ł���Έ�O�̃v���[�g�̍ŏI�I���u���b�N��
                        workPlateNo = gCurPlateNo - 1
                        workBlockNo = Globals_Renamed.GetPrevProcBlock(GetBlockCnt())
                    Else
                        ' �擪�v���[�g�A�擪�I���u���b�N�ł���Έړ����Ȃ�
                        GoTo STP_END
                    End If
                End If
#Else
                ' �v���[�g�Ή�    'V6.0.1.3�@
                Dim hasPrev As Boolean = Form1.GetPrevSelectBlock(
                    workPlateNo, workBlockNo, workPlateNo, workBlockNo)

                If (False = hasPrev) Then
                    ' �擪�v���[�g�A�擪�I���u���b�N�ł���Έړ����Ȃ�
                    GoTo STP_END
                End If
#End If
            End If
            'V4.12.2.0�B             ��

            ' �X�e�[�W�ړ�
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = MOVE_NEXT) Then
                ' �ړ���̃v���[�g�ԍ��A�u���b�N�ԍ���ۑ�����
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0�D
                'V4.1.0.0�Q
                If gKeiTyp = KEY_TYPE_RS Then
                    SetNowBlockDspNum(gCurBlockNo)
                End If
                'V4.1.0.0�Q

                'V5.0.0.9�O              ��
                Dim x, y As Integer : Form1.Get_StartBlkNum(x, y)
                Me.cmbBlockMoveX.SelectedIndex = (x - 1)
                Me.cmbBlockMoveY.SelectedIndex = (y - 1)
                'V5.0.0.9�O              ��

                'V6.0.1.3�@                Form1.SetMapBorder(gCurBlockNo, Color.Black)            'V4.12.2.0�@
                Form1.SetMapBorder(gCurPlateNo, gCurBlockNo, Color.Black)   'V6.0.1.3�@
            End If
STP_END:
            TmKeyCheck.Enabled = True                                   ' �^�C�}�[�J�n

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.btnBlkPrvMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "���u���b�N�ړ�����(Next�{�^��������)"
    '''=========================================================================
    ''' <summary>���u���b�N�ړ�����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub btnBlkNextMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlkNextMove.Click

        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer
        Dim plateCnt As Integer
        Dim blockCnt As Integer

        Try
            ' �v���[�g���A�u���b�N�����擾����
            TmKeyCheck.Enabled = False                                  ' �^�C�}�[��~
            plateCnt = GetPlateCnt()
            blockCnt = GetBlockCnt()

            If (gCurPlateNo >= plateCnt) And (gCurBlockNo >= blockCnt) Then
                ' �ŏI�v���[�g�A�ŏI�u���b�N�ł���Έړ����Ȃ�
                GoTo STP_END
            ElseIf (gCurBlockNo >= blockCnt) Then
                ' �ŏI�u���b�N�ł���΁A���̃v���[�g�̐擪��
                workPlateNo = gCurPlateNo + 1
                workBlockNo = 1
            Else
                ' ���̃u���b�N�ֈړ�����
                workPlateNo = gCurPlateNo
                workBlockNo = gCurBlockNo + 1
            End If

            'V4.12.2.0�B             ��
            If (_procBlockSelected) Then
#If False Then  'V6.0.1.3�@
                workBlockNo = Globals_Renamed.GetNextProcBlock(workBlockNo)
                If (blockCnt < workBlockNo) Then
                    If (gCurPlateNo < plateCnt) Then
                        ' �ŏI�I���u���b�N�ł���΁A���̃v���[�g�̐擪�I���u���b�N��
                        workPlateNo = gCurPlateNo + 1
                        workBlockNo = Globals_Renamed.GetNextProcBlock(1)
                    Else
                        ' �ŏI�v���[�g�A�ŏI�I���u���b�N�ł���Έړ����Ȃ�
                        GoTo STP_END
                    End If
                End If
#Else
                Dim hasNext As Boolean =
                    Form1.GetNextSelectBlock(workPlateNo, workBlockNo, workPlateNo, workBlockNo)

                If (False = hasNext) Then
                    ' �ŏI�v���[�g�A�ŏI�I���u���b�N�ł���Έړ����Ȃ�
                    GoTo STP_END
                End If
#End If
            End If
            'V4.12.2.0�B             ��

            ' �X�e�[�W�ړ�
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = MOVE_NEXT) Then
                ' �ړ���̃v���[�g�ԍ��A�u���b�N�ԍ���ۑ�����
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0�D
                'V4.0.0.0�L��
                If gKeiTyp = KEY_TYPE_RS Then
                    'V5.0.0.9�O                    SetBlockDisplayNumber(gCurBlockNo)
                    If (1 = giStartBlkAss) Then SetBlockDisplayNumber(gCurBlockNo) 'V5.0.0.9�O
                    SetNowBlockDspNum(gCurBlockNo)                           'V4.1.0.0�Q
                End If
                'V4.0.0.0�L��

                'V5.0.0.9�O              ��
                Dim x, y As Integer : Form1.Get_StartBlkNum(x, y)
                Me.cmbBlockMoveX.SelectedIndex = (x - 1)
                Me.cmbBlockMoveY.SelectedIndex = (y - 1)
                'V5.0.0.9�O              ��

                'V6.0.1.3�@                Form1.SetMapBorder(gCurBlockNo, Color.Black)            'V4.12.2.0�@
                Form1.SetMapBorder(gCurPlateNo, gCurBlockNo, Color.Black)       'V6.0.1.3�@
            End If
STP_END:
            TmKeyCheck.Enabled = True                                   ' �^�C�}�[�J�n

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.btnBlkNextMove_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region ""
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V5.0.0.9�O</remarks>
    Private Sub cmbBlockMoveXY_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbBlockMoveY.SelectionChangeCommitted, cmbBlockMoveX.SelectionChangeCommitted
        Try
            TmKeyCheck.Enabled = False                                  ' �^�C�}�[��~

            Dim x As Integer = Me.cmbBlockMoveX.SelectedIndex + 1
            Dim y As Integer = Me.cmbBlockMoveY.SelectedIndex + 1
            Dim workBlockNo As Integer = basTrimming.GetProcessingOrder(x, y)

            ' �X�e�[�W�ړ�
            Dim intRet As Integer = basTrimming.MoveTargetStagePos(Globals_Renamed.gCurPlateNo, workBlockNo)
            If (intRet = frmFineAdjust.MOVE_NEXT) Then
                ' �ړ���̃u���b�N�ԍ���ۑ�����
                Globals_Renamed.gCurBlockNo = workBlockNo
                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")

                If (Globals_Renamed.gKeiTyp = Globals_Renamed.KEY_TYPE_RS) Then
                    SimpleTrimmer.SetNowBlockDspNum(gCurBlockNo)
                End If
            End If

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.cmbBlockMoveXY_SelectionChangeCommitted() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMsg)
        Finally
            TmKeyCheck.Enabled = True                                   ' �^�C�}�[�J�n
        End Try
    End Sub
#End Region

#Region "Ten Key On/Off�{�^������������"
    '''=========================================================================
    ''' <summary>Ten Key On/Off�{�^������������ ###057</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnTenKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTenKey.Click

        'Dim InpKey As UShort
        Dim strMSG As String

        Try
            Call SubBtnTenKey_Click()                                   ' ###139

            '' InpKey��HI SW�ȊO��OFF����' ###139
            'GetInpKey(InpKey)
            'If (InpKey And cBIT_HI) Then                                ' HI SW ON ?
            '    InpKey = cBIT_HI
            'Else
            '    InpKey = 0
            'End If
            'PutInpKey(InpKey)

            '' Ten Key On/Off�{�^���ݒ�
            'If (BtnTenKey.Text = "Ten Key Off") Then
            '    gbTenKeyFlg = True
            '    BtnTenKey.Text = "Ten Key On"
            '    BtnTenKey.BackColor = System.Drawing.Color.Pink
            'Else
            '    gbTenKeyFlg = False
            '    BtnTenKey.Text = "Ten Key Off"
            '    BtnTenKey.BackColor = System.Drawing.SystemColors.Control
            'End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.BtnTenKey_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Ten Key On/Off�{�^������������"
    '''=========================================================================
    ''' <summary>Ten Key On/Off�{�^������������ ###139</summary>
    '''=========================================================================
    Private Sub SubBtnTenKey_Click()

        Dim InpKey As UShort
        Dim strMSG As String

        Try
            ' InpKey��HI SW�ȊO��OFF����
            GetInpKey(InpKey)
            If (InpKey And cBIT_HI) Then                                ' HI SW ON ?
                InpKey = cBIT_HI
            Else
                InpKey = 0
            End If
            PutInpKey(InpKey)

            ' Ten Key On/Off�{�^���ݒ�
            If (BtnTenKey.Text = "Ten Key Off") Then
                gbTenKeyFlg = True
                BtnTenKey.Text = "Ten Key On"
                BtnTenKey.BackColor = System.Drawing.Color.Pink
                'V6.0.1.0�J                Form1.SetTrimMapVisible(False)                          'V6.0.1.0�I
            Else
                gbTenKeyFlg = False
                BtnTenKey.Text = "Ten Key Off"
                BtnTenKey.BackColor = System.Drawing.SystemColors.Control
            End If
            Sub_10KeyUp(Keys.None, stJOG)       'V6.0.1.0�B
            'V6.0.1.0�J            Form1.SetMapOnOffButtonEnabled(Not gbTenKeyFlg)             'V4.12.2.0�@

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.SubBtnTenKey_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�f�[�^�ҏW�{�^������������"
    '''=========================================================================
    ''' <summary>�f�[�^�ҏW�{�^������������ ###063</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            GrpArrow.Visible = False                                    ' JOG�R���g���[�����\���ɂ��� 
            Me.Hide()                                                   ' �ꎞ��~��ʂ��\���Ƃ��� 

            ' �f�[�^�ҏW�v���O�������N������(�ꎞ��~���[�h)
            r = Form1.ExecEditProgram(1)                                ' ����~���̃G���[��������APP�I������̂Ŗ߂��Ă��Ȃ� 
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`���� ###226
            If (r <> cFRS_NORMAL) Then
                GoTo STP_ERR                                            ' �ꎞ��~��ʏ����I����
            End If

            ' �v���[�g�̃X�^�[�g�|�W�V�����ݒ�                          ' ###079 File_Read()��Call�����gBlkStagePosX,Y()���N���A�����̂ōĐݒ肷��
            Call CalcPlateXYStartPos()
            ' �u���b�N�̃X�^�[�g�|�W�V�����ݒ�                          ' ###079
            Call CalcBlockXYStartPos()

            ' �g���~���O�f�[�^��INtime���ɑ��M���� ###087
            Call TRIMEND()                                              ' INtime���̃��������
            '----- ###257�� -----
            ' FL�����猻�݂̉��H��������M����
            r = TrimCondInfoRcv(stCND)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                GoTo STP_ERR                                            ' �ꎞ��~��ʏ����I����
            End If
            '----- ###257�� -----
            r = SendTrimData()                                          ' �g���~���O�f�[�^��INtime���ɑ��M����
            If (r <> cFRS_NORMAL) Then
                ' "�g���~���O�f�[�^�̐ݒ�Ɏ��s���܂����B" & vbCrLf & "�g���~���O�f�[�^�ɖ�肪�Ȃ����m�F���Ă��������B"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                GoTo STP_ERR                                            ' �ꎞ��~��ʏ����I����
            End If

            'V6.0.1.0�Q��
            '�J�b�g�ʒu�␳�p�����[�^�̏������F�ꎞ��~���̕ҏW�ŕύX���ꂽ�Ƃ��ɑΉ��ł���悤��
            '---------------------------------------------------------------------------
            '   �J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u����ݒ肷��yTKY�p�z
            '   ��TKY���̂ݗL������CHIP/NET�����e�[�u���ݒ�(������)�͂���
            '---------------------------------------------------------------------------
            giCutPosRNum = CutPosCorrectInit(gRegistorCnt, stCutPos)
            'V6.0.1.0�Q��


            ' �㏈��
            GrpArrow.Visible = True                                     ' JOG�R���g���[����\������ 

            Call ZCONRST()                                              'V4.7.0.0�Q�ݿ�ٷ�ׯ�����
            TmKeyCheck.Interval = 10                                    'V4.7.0.0�Q
            TmKeyCheck.Enabled = True                                   'V4.7.0.0�Q�@�^�C�}�[�J�n

            Me.Show()                                                   ' �ꎞ��~��ʂ�\������ 
            Return

            ' �G���[������
STP_ERR:
            mExit_flg = cFRS_ERR_RST                                    ' Return�l = Cancel(RESET��)  
            gbExitFlg = True                                            ' �I���t���OON
            gfrmAdjustDisp = 0                                           'V5.0.0.1-29
            Me.Close()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmFineAdjust.BtnEdit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "CLR�{�^������������"
    '''=========================================================================
    ''' <summary>CLR�{�^������������ ###139</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub btnCounterClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCounterClear.Click

        Dim r As Short
        Dim strMSG As String

        Try
            ' �݌v���N���A���Ă���낵���ł����H
            r = MsgBox(MSG_108, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)
            If (r = MsgBoxResult.Yes) Then
                Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
                Call Form1.ClearCounter(1)                              ' ���Y�Ǘ��f�[�^�̃N���A
                Call ClrTrimPrnData()                                   ' ���ݸތ��ʈ�����ڂ��ް���ر����(���[���a����) V1.18.0.0�B

                ' ���v�\����ON�̏ꍇ�A�\�����X�V����
                If Form1.chkDistributeOnOff.Checked = True Then
                    gObjFrmDistribute.RedrawGraph()
                End If
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmFineAdjust.btnCounterClear_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�O���t�\��/��\���{�^������������"
    '''=========================================================================
    ''' <summary>�O���t�\��/��\���{�^������������ ###139</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub chkDistributeOnOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDistributeOnOff.CheckedChanged

        Dim strMSG As String

        Try
            ' ���v�O���t�\����ON/OFF
            If chkDistributeOnOff.Checked = True Then
                ' ���v�O���t�\����ON
                Form1.chkDistributeOnOff.Checked = True                 ' �{�^�� =�u���Y�O���t�\���v 
                Form1.changefrmDistStatus(1)                            ' �O���t�\�� 
                gObjFrmDistribute.RedrawGraph()                         ' �\�����X�V����

                '�{�^���\���̕ύX
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    Form1.chkDistributeOnOff.Text = "���Y�O���t�@��\��"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'Else
                '    Form1.chkDistributeOnOff.Text = "Distribute OFF"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'End If
                Form1.chkDistributeOnOff.Text = frmDistribution_004
                chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text

                ' ���{�^���񊈐���
                Call SetBtnArowEnable(False)

                '----- ###150�� -----
                ' ���v�\�����͓��v�\����̃{�^����L���ɂ���
                gObjFrmDistribute.cmdGraphSave.Enabled = True
                gObjFrmDistribute.cmdInitial.Enabled = True
                gObjFrmDistribute.cmdFinal.Enabled = True
                '----- ###150�� -----

            Else
                ' ���v�O���t�\����OFF
                Form1.chkDistributeOnOff.Checked = False            ' �{�^�� =�u���Y�O���t�\���v 
                Form1.changefrmDistStatus(0)                        ' �O���t��\�� 

                ' �{�^���\���̕ύX
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    Form1.chkDistributeOnOff.Text = "���Y�O���t�@�\��"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'Else
                '    Form1.chkDistributeOnOff.Text = "Distribute ON"
                '    chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text
                'End If
                Form1.chkDistributeOnOff.Text = frmDistribution_003
                chkDistributeOnOff.Text = Form1.chkDistributeOnOff.Text

                ' ���{�^��������
                Call SetBtnArowEnable(True)
                Call Sub_SetBtnArrowEnable()                        ' V1.16.0.0�F

            End If

        Catch ex As Exception
            strMSG = "frmFineAdjust.chkDistributeOnOff_CheckedChanged() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###237�� -----
#Region "LASER�{�^������������"
    '''=========================================================================
    ''' <summary>LASER�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnLaser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnLaser.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' LASER�ˏo�\/�s�̐؂�ւ�
            If (BtnLaser.BackColor = System.Drawing.SystemColors.Control) Then
                ' LASER�ˏo�\�Ƃ���
                BtnLaser.BackColor = System.Drawing.Color.OrangeRed
            Else
                ' LASER�ˏo�s�Ƃ���
                BtnLaser.BackColor = System.Drawing.SystemColors.Control
                r = LASEROFF()
                m_LaserOnOffFlag = False
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "frmFineAdjust.BtnLaser_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###237�� -----

    '    '========================================================================================
    '    '   ���ʊ֐�
    '    '========================================================================================
    '#Region "�X�e�[�W�ړ�����"
    '    '''=========================================================================
    '    ''' <summary>�X�e�[�W�ړ�����</summary>
    '    ''' <param name="pltNo"></param>
    '    ''' <param name="blkNo"></param>
    '    ''' <returns></returns>
    '    '''=========================================================================
    '    Public Function MoveTargetStagePos(ByVal pltNo As Integer, ByVal blkNo As Integer) As Integer
    '        '    Private Function MoveTargetStagePos(ByVal pltNo As Integer, ByVal blkNo As Integer) As Integer

    '        Dim intRet As Integer
    '        Dim nextStgX As Double
    '        Dim nextStgY As Double
    '        Dim dispPltX As Integer
    '        Dim dispPltY As Integer
    '        Dim dispBlkX As Integer
    '        Dim dispBlkY As Integer
    '        'Dim retBlkNoX As Integer
    '        'Dim retBlkNoY As Integer
    '        Dim dispCurStgGrpNoX As Integer
    '        Dim dispCurStgGrpNoY As Integer
    '        Dim dispCurBlkNoX As Integer
    '        Dim dispCurBlkNoY As Integer
    '        Dim dispCurPltNoX As Integer
    '        Dim dispCurPltNoY As Integer
    '        Dim StgX As Double = 0.0 ' V4.0.0.0-40
    '        Dim StgY As Double = 0.0 ' V4.0.0.0-40

    '        Try
    '            MoveTargetStagePos = MOVE_NEXT
    '            intRet = GetTargetStagePos(pltNo, blkNo, nextStgX, nextStgY, dispPltX, dispPltY, dispBlkX, dispBlkY)
    '            If intRet = BLOCK_END Then
    '                ' �������Ȃ��ŏI��
    '                MoveTargetStagePos = MOVE_NOT
    '                Exit Function
    '            ElseIf intRet = PLATE_BLOCK_END Then
    '                ' �������Ȃ��ŏI��
    '                MoveTargetStagePos = MOVE_NOT
    '                Exit Function
    '            End If

    '            '---------------------------------------------------------------------
    '            '   �\���p�e�|�W�V�����̔ԍ���ݒ�i�v���[�g/�X�e�[�W�O���[�v/�u���b�N�j
    '            '---------------------------------------------------------------------
    '            Dim bRet As Boolean
    '            bRet = GetDisplayPosInfo(dispBlkX, dispBlkY, _
    '                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)

    '            '---------------------------------------------------------------------
    '            '   ���O�\��������̐ݒ�
    '            '---------------------------------------------------------------------
    '            dispCurPltNoX = dispPltX : dispCurPltNoY = dispPltY         '###056
    '            Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY, _
    '                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
    '            ' �X�e�[�W�̓���
    '            '----- V1.13.0.0�B�� -----
    '            ' �L�k�␳�p�p�����[�^�̐ݒ�
    '            GetShinsyukuData(dispBlkX, dispBlkY, nextStgX, nextStgY)
    '            '----- V2.0.0.0�H�� -----
    '            If (giMachineKd = MACHINE_KD_RS) Then
    '                '----- V4.0.0.0-40�� -----
    '                ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
    '                '��
    '                'V4.6.0.0�C�@If (giStageYOrg = STGY_ORG_UP) Then
    '                StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
    '                StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
    '                'V4.6.0.0�C�@Else
    '                'V4.6.0.0�C�@StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
    '                'V4.6.0.0�C�@StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY ' + (typPlateInfo.dblBlockSizeYDir / 2)
    '                'V4.6.0.0�C�@End If
    '                'V4.6.0.0�C�@��
    '                intRet = Form1.System1.EX_START(gSysPrm, StgX, StgY, 0)

    '                'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
    '                '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
    '                '----- V4.0.0.0-40�� -----
    '            Else
    '                intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
    '                                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
    '            End If
    '            'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
    '            '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
    '            '----- V2.0.0.0�H�� ----
    '            '----- V1.13.0.0�B�� -----

    '        Catch ex As Exception
    '            Dim strMsg As String
    '            strMsg = "frmFineAdjust.btnTrimming_Click() TRAP ERROR = " + ex.Message
    '            MsgBox(strMsg)
    '        End Try
    '    End Function
    '#End Region

#Region "���{�^��������/�񊈐���"
    '''=========================================================================
    ''' <summary>���{�^��������/�񊈐��� ###139</summary>
    ''' <param name="OnOff"></param>
    '''=========================================================================
    Private Sub SetBtnArowEnable(ByVal OnOff As Boolean)

        Dim strMSG As String

        Try
            ' ���{�^��������/�񊈐���
            BtnJOG_0.Enabled = OnOff
            BtnJOG_1.Enabled = OnOff
            BtnJOG_2.Enabled = OnOff
            BtnJOG_3.Enabled = OnOff
            BtnJOG_4.Enabled = OnOff
            BtnJOG_5.Enabled = OnOff
            BtnJOG_6.Enabled = OnOff
            BtnJOG_7.Enabled = OnOff
            BtnHI.Enabled = OnOff

            ' Ten Key�{�^��������/�񊈐���
            BtnTenKey.Enabled = OnOff

            ' Ten Key�{�^����On/Off�ɂ���
            If (OnOff = False) Then
                ' ���{�^���񊈐����Ȃ�Ten Key�{�^����Off�ɂ��ăe���L�[���͂�s�Ƃ���
                If (BtnTenKey.Text = "Ten Key On") Then
                    m_TenKeyFlg = True
                    Call SubBtnTenKey_Click()
                End If

                Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)  'V6.0.0.0�I

            Else
                ' Ten Key�{�^����Off�ɂ����ꍇ��Ten Key�{�^����On�ɂ��ăe���L�[���͂��Ƃ���
                If (m_TenKeyFlg = True) Then
                    m_TenKeyFlg = False
                    Call SubBtnTenKey_Click()
                End If
            End If
            Sub_10KeyUp(Keys.None, stJOG)                               'V6.0.1.0�A

        Catch ex As Exception
            strMSG = "frmFineAdjust.SetBtnArowEnable() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '===============================================================================
    '   Description  : �i�n�f�����ʏ���
    '
    '   Copyright(C) : OMRON LASERFRONT INC. 2012
    '
    '===============================================================================
    '========================================================================================
    '   �{�^������������
    '========================================================================================
#Region "RESET�{�^������������"
    '''=========================================================================
    ''' <summary>RESET�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnRESET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRESET.Click
        mExit_flg = cFRS_ERR_RST                                        ' Return�l = Cancel(RESET��)  
        gbExitFlg = True                                                ' �I���t���OON
        gfrmAdjustDisp = 0                  'V5.0.0.1-29
        Me.Close()
    End Sub
#End Region

#Region "START�{�^������������"
    '''=========================================================================
    ''' <summary>START�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSTART.Click
        Dim r As Integer

        If (gKeiTyp = KEY_TYPE_RS) Then
            r = GetLaserOffIO(True) 'V5.0.0.1�K
            If r = 1 Then
                ''V5.0.0.1�H��
                r = cFRS_NORMAL
                Call ZCONRST()
                ''V5.0.0.1�H��
            Else
                mExit_flg = cFRS_ERR_START                                        ' Return�l = START 
                gbExitFlg = True                                                ' �I���t���OON
                gfrmAdjustDisp = 0                  'V5.0.0.1-29
                Me.Close()
            End If
        Else
            mExit_flg = cFRS_ERR_START                                        ' Return�l = START 
            gbExitFlg = True                                                ' �I���t���OON
            gfrmAdjustDisp = 0                  'V5.0.0.1-29
            Me.Close()
        End If

    End Sub
#End Region


#Region "HI�{�^������������"
    '''=========================================================================
    ''' <summary>HI�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnHI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHI.Click
        Call SubBtnHI_Click(stJOG)
    End Sub
#End Region

#Region "���{�^���̃}�E�X�N���b�N������"
    '''=========================================================================
    ''' <summary>���{�^���̃}�E�X�N���b�N������</summary>
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
    '----- ###219 -----
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
    '----- ###219 -----

    '========================================================================================
    '   �e���L�[���͏���
    '========================================================================================
#Region "�L�[�_�E��������"
    '''=========================================================================
    ''' <summary>�L�[�_�E��������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown 'V6.0.0.0�J
        Me.JogKeyDown(e)                'V6.0.0.0�J
    End Sub

    Public Sub JogKeyDown(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyDown    'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0�J
        Dim r As Integer

        '----- ###237�� -----
        ' LASER�ˏo�\�Łu*�L�[�v�����Ȃ�LASER�ˏo����()
        If (BtnLaser.BackColor = System.Drawing.Color.OrangeRed) And (KeyCode = System.Windows.Forms.Keys.Multiply) Then
            ' ���[�UON
            If (m_LaserOnOffFlag = False) Then
                ' DIG-SW�ݒ� 
                Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex) 'V5.0.0.1�K

                ''V4.0.0.0-86
                r = GetLaserOffIO(False)
                If r = 1 Then
                    Me.ShowInTaskbar = False 'V5.0.0.1�K
                    Me.Activate()  'V5.0.0.1�K
                    'frmFineAdjust_KeyUp(sender, e)

                    Exit Sub
                End If
                ''V4.0.0.0-86
                Call LASERON()
                m_LaserOnOffFlag = True
                Console.WriteLine("frmFineAdjust_KeyDown() Laser On")
            End If
        End If
        '----- ###237�� -----

        ' �e���L�[���̓t���O��OFF�Ȃ�NOP ###057
        If (gbTenKeyFlg = False) Then Exit Sub

        ' �e���L�[�_�E���Ȃ�InpKey�Ƀe���L�[�R�[�h��ݒ肷��
        'V6.0.0.0�K       'Call Sub_10KeyDown(KeyCode)
        Sub_10KeyDown(KeyCode, stJOG)             'V6.0.0.0�K
        If (KeyCode = System.Windows.Forms.Keys.NumPad5) Then       ' 5�� (KeyCode = 101(&H65)
            'Call BtnHI_Click(sender, e)                             ' HI�{�^�� ON/OFF
            Call BtnHI_Click(BtnHI, e)                              ' HI�{�^�� ON/OFF     'V6.0.0.0�I
        End If
        'Call Me.Focus()

    End Sub
#End Region

#Region "�L�[�A�b�v������"
    '''=========================================================================
    ''' <summary>�L�[�A�b�v������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub frmFineAdjust_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyUp 'V6.0.0.0�J
        Me.JogKeyUp(e)                  'V6.0.0.0�J
    End Sub

    Public Sub JogKeyUp(ByVal e As KeyEventArgs) Implements ICommonMethods.JogKeyUp        'V6.0.0.0�J
        'V6.0.0.0�J        Dim KeyCode As Short = e.KeyCode
        Dim KeyCode As Keys = e.KeyCode             'V6.0.0.0�J

        '----- ###237�� -----
        ' LASER Off����
        If (m_LaserOnOffFlag = True) Then
            Call LASEROFF()
            m_LaserOnOffFlag = False
            Console.WriteLine("frmFineAdjust_KeyUp() Laser Off")
        End If
        '----- ###237�� -----

        ' �e���L�[���̓t���O��OFF�Ȃ�NOP ###057
        'V6.0.1.0�B        If (gbTenKeyFlg = False) Then Exit Sub
        If (False = gbTenKeyFlg) Then       'V6.0.1.0�B
            Sub_10KeyUp(Keys.None, stJOG)   'V6.0.1.0�B
        Else
            ' �e���L�[�A�b�v�Ȃ�InpKey�̃e���L�[�R�[�h��OFF����
            'V6.0.0.0�K        Call Sub_10KeyUp(KeyCode)
            Sub_10KeyUp(KeyCode, stJOG)                   'V6.0.0.0�K
            'Call Me.Focus()
        End If

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

    'V4.0.0.0-84
    ''' <summary>
    ''' �ꎞ��~��ʂ�Close�{�^�����������Ƃ��̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

        If gKeiTyp = KEY_TYPE_RS Then
            ' DIG-SW�ݒ�  // 'V4.1.0.0�M
            Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex)

            'V4.8.0.1�A��
            ' BP�I�t�Z�b�g�X�V(�^�C�~���O�ɂ���ċ󔒂œ����Ă���ꍇ�g���b�v�G���[�ƂȂ�̂Ń`�F�b�N���� ###014)
            If (txtBpOffX.Text <> "") And (txtBpOffY.Text <> "") Then
                Call SetBpOffset(Double.Parse(txtBpOffX.Text), Double.Parse(txtBpOffY.Text))
            End If
            'V4.8.0.1�A��

            SetADJButton()      'V4.1.0.0�N

            ' �f�[�^�\�����\���ɂ���
            Call Form1.SetDataDisplayOn()
            GroupBoxVisibleChange(True)
            SetSimpleVideoSize()
            Form1.TimerAdjust.Enabled = True

            'V5.0.0.9�O                  ��
            'V5.0.0.9�O            Call SimpleTrimmer.ResistorDataDisp(True, 0, 1)
            Dim blkNo As Integer = TrimData.GetBlockNumber()
            If (0 = blkNo) Then blkNo = 1
            SimpleTrimmer.ResistorDataDisp(True, blkNo, 1)
            'V5.0.0.9�O                  ��

            Call Sub_StopTimer()                                ' ###260
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ���\������
            'V5.0.0.9�O            If (giStartBlkAss = 1) Then                             ' �g���~���O�J�n�u���b�N�ԍ��w��̗L��/����(0=����, 1=�L��)�@
            If (giStartBlkAss <> 0) Then                             ' �g���~���O�J�n�u���b�N�ԍ��w��̗L��/����(0=����, 1=�L��)    'V5.0.0.9�O
                Form1.GrpStartBlk.Visible = True
                'V5.0.0.1�@��
                'V5.0.0.9�O                Call Form1.Set_StartBlkNum(gCurBlockNo, 1)
                If (1 = giStartBlkAss) Then Form1.Set_StartBlkNum(gCurBlockNo, 1) 'V5.0.0.9�O
                SetNowBlockDspNum(gCurBlockNo)
                'V5.0.0.1�@��
            End If
            '----- V4.11.0.0�D�� -----

            gfrmAdjustDisp = 0                  'V5.0.0.1-29
            Close()

        End If
    End Sub

    'V5.0.0.1�K
    Private Sub CbDigSwL_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbDigSwL.SelectedIndexChanged

        ' DIG-SW�ݒ�  
        Call Form1.SetMoveMode(CbDigSwL.SelectedIndex, CbDigSwH.SelectedIndex)

    End Sub

    Private Sub lblBlockNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBlockNo.Click
        lblBlockNo = lblBlockNo
    End Sub


    Private Sub frmFineAdjust_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        lblBlockNo = lblBlockNo
    End Sub

    'V6.0.0.0�S    Private Sub frmFineAdjust_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

    'V6.0.0.0�S    End Sub

    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0�S</remarks>
    Private Sub frmFineAdjust_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)

        ' �Ō�ɕ\�������u���b�N�̘g��������   'V4.12.2.0�@
        Form1.ClearMapBorder()

        'V6.0.1.0�J        ' MAP ON/OFF �{�^�����\���ɂ���     'V4.12.2.0�@
        'V6.0.1.0�J        Form1.SetMapOnOffButtonEnabled(False)
    End Sub

End Class