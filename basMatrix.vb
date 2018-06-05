'===============================================================================
'   Description  : �I�[�g�v���[�u�p�}�g���b�N�X��ʏ���
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'===============================================================================

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module basMatrix
#Region "�y�ϐ���`�z"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- ���x���R���g���[���̕\���T�C�Y -----
    Private Const LBLSZ_20 As Integer = 20
    Private Const LBLSZ_30 As Integer = 30
    Private Const LBLSZ_40 As Integer = 40

    '----- ���x���R���g���[���̃o�b�N�J���[ -----
    Private Const LBL_OK As Integer = 0
    Private Const LBL_NG As Integer = 1

    '----- �}�g���b�N�X�̃u���b�N�� -----
    Private Const MAX_MATRIX As Integer = 7

    '----- �ϐ���` -----
    Private mExitFlag As Integer                                ' �I���t���O(Return�l)
    Private gBpPosX As Double = 0.0                             ' BP�ʒuX
    Private gBpPosY As Double = 0.0                             ' BP�ʒuY
    Private g1stPosX As Double = 0.0                            ' ���S���WX
    Private g1stPosY As Double = 0.0                            ' ���S���WY
    Private gStgOfsX As Double = 0.0                            ' �X�e�[�W�I�t�Z�b�gX
    Private gStgOfsY As Double = 0.0                            ' �X�e�[�W�I�t�Z�b�gY

    Private giBlkCnt As Integer                                 ' �X�e�b�v�u���b�N��

    Private ObjMain As Object = Nothing                         ' �ďo���̃I�u�W�F�N�g
    Private LabelAry(,) As System.Windows.Forms.Label = Nothing ' �}�g���b�N�X�\���p���x���z�� 
    Private StgPosAryX(MAX_MATRIX) As Double                    ' �X�e�[�W���WX�z�� 
    Private StgPosAryY(MAX_MATRIX) As Double                    ' �X�e�[�W���WY�z�� 
    Private SerchAryX(MAX_MATRIX * MAX_MATRIX) As Integer       ' �u���b�N�ԍ�X�z��(�����p) 
    Private SerchAryY(MAX_MATRIX * MAX_MATRIX) As Integer       ' �u���b�N�ԍ�Y�z��(�����p) 

    '----- �I�[�g�v���[�u���s�p�\����(��`��MainModules.vb) -----
    'Public Structure AutoProbe_Info                             ' �I�[�g�v���[�u���s�p�\���̌`����`
    '    Dim intAProbeGroupNo1 As Short                          ' �p�^�[��1(����)�p�O���[�v�ԍ�
    '    Dim intAProbePtnNo1 As Short                            ' �p�^�[��1(����)�p�p�^�[���ԍ�
    '    Dim intAProbeGroupNo2 As Short                          ' �p�^�[��2(���)�p�O���[�v�ԍ�
    '    Dim intAProbePtnNo2 As Short                            ' �p�^�[��2(���)�p�p�^�[���ԍ�
    '    Dim dblAProbeBpPosX As Double                           ' �p�^�[��1(����)�pBP�ʒuX
    '    Dim dblAProbeBpPosY As Double                           ' �p�^�[��1(����)�pBP�ʒuY
    '    Dim dblAProbeStgPosX As Double                          ' �p�^�[��2(���)�p�X�e�[�W�I�t�Z�b�g�ʒuX
    '    Dim dblAProbeStgPosY As Double                          ' �p�^�[��1(����)�p�X�e�[�W�I�t�Z�b�g�ʒuY
    '    Dim intAProbeStepCount As Short                         ' �X�e�b�v���s�p�X�e�b�v��
    '    Dim intAProbeStepCount2 As Short                        ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v��
    '    Dim dblAProbePitch As Double                            ' �X�e�b�v���s�p�X�e�b�v�s�b�`
    '    Dim dblAProbePitch2 As Double                           ' �X�e�b�v���s�p�J��Ԃ��X�e�b�v�s�b�`
    'End Structure
#End Region

#Region "�y���\�b�h��`�z"
#Region "�I�����ʂ�Ԃ�"
    '''=========================================================================
    ''' <summary>�I�����ʂ�Ԃ�</summary>
    ''' <param name="OfsX">(OUT)�X�e�[�W�I�t�Z�b�gX</param>
    ''' <param name="OfsY">(OUT)�X�e�[�W�I�t�Z�b�gY</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function GetMatrixReturn(ByRef OfsX As Double, ByRef OfsY As Double) As Integer

        Dim strMSG As String

        Try
            ' �␳�ʒu��Ԃ�
            OfsX = gStgOfsX
            OfsY = gStgOfsY

            ' mExitFlag=OK(START��)�Ȃ�ߒl=����Ƃ��� 
            If (mExitFlag = cFRS_ERR_START) Then mExitFlag = cFRS_NORMAL
            Return (mExitFlag)
            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.GetMatrixReturn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����"
    '''=========================================================================
    ''' <summary>ShowDialog���\�b�h�ɓƎ��̈�����ǉ�����</summary>
    ''' <param name="Obj">   (INP)�ďo���̃I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function ShowMatrixDialog(ByRef Obj As Object) As Integer

        Dim stPos As System.Drawing.Point
        Dim stGetPos As System.Drawing.Point
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' ��������
            ObjMain = Obj
            mExitFlag = -1                                              ' �I���t���O = ������

            ' �\���ʒu�̒���
            stPos = Form1.chkDistributeOnOff.PointToScreen(stGetPos)
            stPos.X = stPos.X
            stPos.Y = stPos.Y - 20
            Form1.GrpMatrix.Location = stPos                            ' StartPosition�v���p�e�B�� = Manual�ɂ��Ȃ��ƌ����Ȃ� ?

            ' �}�g���b�N�X����
            giBlkCnt = ObjMain.stAPRB.intAProbeStepCount * 2 + 1        ' �u���b�N�� = �X�e�b�v�� * 2 + 1 
            Call MatrixGenerate(giBlkCnt, ObjMain.stAPRB.intAProbeStepCount)

            ' �u���b�N�ԍ��z�������������
            Call InitSerchAry(giBlkCnt, ObjMain.stAPRB.intAProbeStepCount)

            ' ��ʕ\��
            Form1.GrpMatrix.Visible = True                              ' �}�g���b�N�X��ʕ\��
            Form1.GrpMatrix.BringToFront()                              ' �őO�ʂɕ\��

            ' ��ʏ������C��
            r = FrmMatrix_Main()
            mExitFlag = r                                               ' mExitFlag�ɖ߂�l��ݒ肷�� 
            Return (r)

            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.ShowMatrixDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "��ʏ������C��"
    '''=========================================================================
    ''' <summary>��ʏ������C��</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function FrmMatrix_Main() As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        'Dim RptCnt As Integer
        Dim StgOfsX As Double
        Dim StgOfsY As Double
        Dim StgPosX As Double
        Dim StgPosY As Double
        Dim SvStgPosX As Double
        Dim SvStgPosY As Double
        Dim SvBpPosX As Double
        Dim SvBpPosY As Double
        Dim fCorrectX As Double
        Dim fCorrectY As Double
        Dim fCoeff As Double = 0.0                                      ' ���֒l
        'Dim RptPit As Double
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �}�g���b�N�X�\��
            Form1.GrpMatrix.Refresh()

            ' ���ݍ��W�ޔ� 
            Call ZGETBPPOS(SvBpPosX, SvBpPosY)
            Call ZGETPHPOS2(SvStgPosX, SvStgPosY)

            ' �v���[�u�R�}���h���̓X�e�b�v������s��
            If (giAppMode = APP_MODE_PROBE) Then
                StgPosX = SvStgPosX
                StgPosY = SvStgPosY
                GoTo STP_STPMEAS
            End If

            ' �u���b�N�T�C�Y�ݒ�
            r = Form1.System1.EX_BSIZE(gSysPrm, typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            ' BP�I�t�Z�b�g�ݒ�(���u���b�N�T�C�Y��ݒ肷���BP�I�t�Z�b�g��INtime���ŏ����������)
            r = Form1.System1.EX_BPOFF(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If

            ' BP�ʒu�Ƀp�^�[��1(����)�pBP�ʒu��ݒ肷��
            gBpPosX = ObjMain.stAPRB.dblAProbeBpPosX
            gBpPosY = ObjMain.stAPRB.dblAProbeBpPosY

            '-------------------------------------------------------------------
            '   �p�^�[���F��(����)�����s����
            '-------------------------------------------------------------------
            ' XY�e�[�u�������_�ֈړ�����
            r = Form1.System1.EX_SMOVE2(gSysPrm, 0.0, 0.0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If

            ' Z2��ON�ʒu�Ɉړ�����
            If IsUnderProbe() Then                                      ' Z2�L�� ? 
                r = Z2move(Z2ON)                                        ' Z2��ON�ʒu�Ɉړ�����
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If
                Call ZSTOPSTS2()                                        ' Z2�������~�҂�
            End If
            Call System.Threading.Thread.Sleep(300)                     ' Wait(msec)

            ' ����J�����ŉ����t���[�u��̃p�^�[���F�������s����
            r = Sub_PatternMatching(ObjMain.stAPRB.intAProbeGroupNo1, ObjMain.stAPRB.intAProbePtnNo1, gBpPosX, gBpPosY, fCorrectX, fCorrectY, fCoeff)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? ���p�^�[���F���G���[�ɂȂ�ƃu���b�N�T�C�Y��0�ƂȂ��Ă���̂Œ���
                GoTo STP_EXIT
            End If

            '-------------------------------------------------------------------
            '   �p�^�[���F��(���)�����s����
            '-------------------------------------------------------------------
            ' Z2��OFF�ʒu�Ɉړ�����
            If IsUnderProbe() Then                                      ' Z2�L�� ? 
                r = Z2move(Z2OFF)                                       ' Z2��OFF�ʒu�Ɉړ�����
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If
                Call ZSTOPSTS2()                                        ' Z2�������~�҂�
            End If

            ' ����ʕ�BP�ʒu��␳����(fCorrectX,Y�͊�R�[�i�[���l���ς�)
            gBpPosX = gBpPosX + fCorrectX
            gBpPosY = gBpPosY + fCorrectY

            ' �X�e�[�W���W�����߂�
            Call GetAProbStagePosition(StgOfsX, StgOfsY)

            ' XY�e�[�u�����p�^�[���F���ʒu�ֈړ�(�g�����ʒu�{�X�e�[�W�I�t�Z�b�g�{���␳�l+����p�I�t�Z�b�g)
            'V6.0.0.0-28            Call Form1.VideoLibrary1.VideoStop()                        ' �X�e�[�W�ړ��O�Ƀr�f�I�̍X�V��������U��~
            r = Form1.System1.EX_SMOVE2(gSysPrm, StgOfsX, StgOfsY)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                Return (r)
            End If
            Call System.Threading.Thread.Sleep(300)                     ' Wait(msec)
            'V6.0.0.0-28            Call Form1.VideoLibrary1.VideoStart()                       ' �X�e�[�W�ړ���Ƀr�f�I�̍X�V�������Ď��{

            ' ����J�����Ŋ��̃p�^�[���F�������s����(����ʁ�fCorrectX, fCorrectY)
            r = Sub_PatternMatching(ObjMain.stAPRB.intAProbeGroupNo2, ObjMain.stAPRB.intAProbePtnNo2, gBpPosX, gBpPosY, fCorrectX, fCorrectY, fCoeff)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                Return (r)
            End If

            ' �摜�\���v���O�������N������
            ' ������ V3.1.0.0�A 2014/12/01
            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
            'V6.0.0.0�D            r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
            ' ������ V3.1.0.0�A 2014/12/01

            ' �X�e�[�W���W�����߂�(����ʒu)
            Call GetAProbStageMeasPosition(StgOfsX, StgOfsY)

            ' ����ʕ��X�e�[�W���ړ�����
            'fCorrectX = fCorrectX * -1                                  ' BP�łȂ��X�e�[�W�ړ��Ȃ̂ŕ����𔽓]����
            fCorrectX = fCorrectX
            fCorrectY = fCorrectY * -1
            r = Form1.System1.EX_SMOVE2(gSysPrm, StgOfsX + fCorrectX, StgOfsY + fCorrectY)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                rtn = r
                GoTo STP_ERR_EXIT
            End If
            Call ZGETPHPOS2(StgPosX, StgPosY)                           ' ���S���W�ޔ� 

            '-------------------------------------------------------------------
            '   �X�e�b�v������s��
            '-------------------------------------------------------------------
STP_STPMEAS:
            'RptPit = ObjMain.stAPRB.dblAProbePitch2                     ' �X�e�b�v����J�Ԃ��X�e�b�v�s�b�`
            'For RptCnt = 1 To ObjMain.stAPRB.intAProbeStepCount2        ' �X�e�b�v����J�Ԃ��X�e�b�v�񐔕��J�Ԃ�
            ' �}�g���b�N�X�\��������
            Call MatrixInit(giBlkCnt)

            ' �X�e�b�v�񐔕���R������s��
            r = StepMeasure(giBlkCnt, StgPosX, StgPosY, ObjMain.stAPRB.dblAProbePitch)
            If (r = cFRS_ERR_RST) Then GoTo STP_EXIT '                  ' Cancel(RESET�L�[)
            If (r < cFRS_NORMAL) Then                                   ' ����~�� ?
                rtn = r
                GoTo STP_ERR_EXIT
            End If

            'Next RptCnt

            ' ���S���W�ɋ߂��A����OK�̃X�e�[�W�I�t�Z�b�g����������
            gStgOfsX = 0.0                                               ' �X�e�[�W�I�t�Z�b�g������
            gStgOfsY = 0.0
            If (r = cFRS_NORMAL) Then                                   ' ����OK ? 
                ' �X�e�[�W�I�t�Z�b�g����������
                Call SerchStageOffset(giBlkCnt, gStgOfsX, gStgOfsY)

                ' �I�[�g�v���[�u���s�R�}���h���Ȃ�␳�l��\������
                If (giAppMode = APP_MODE_APROBEEXE) Then
                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    '    strMSG = "�I�[�g�v���[�u���s �␳�lX=" + gStgOfsX.ToString("0.0000") + ", �␳�lY=" + gStgOfsY.ToString("0.0000")
                    'Else
                    '    strMSG = "Auto Prob Execute. X=" + gStgOfsX.ToString("0.0000") + ", Y=" + gStgOfsY.ToString("0.0000")
                    'End If
                    strMSG = basMatrix_001 & gStgOfsX.ToString("0.0000") & basMatrix_002 & gStgOfsY.ToString("0.0000")
                    Call Form1.Z_PRINT(strMSG)
                End If
            End If

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
            ' �摜�\���v���O�������I������
            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)
            Return (cFRS_NORMAL)

STP_EXIT:
            ' �p�^�[���F���G���[���A�A�v�������I�����Ȃ��ꍇ�͂w�x�X�e�[�W�����̈ʒu�Ɉړ�����
            rtn = r                                                     ' Return�l�ޔ�
            If (r >= cFRS_NORMAL) Or ((r >= cFRS_ERR_PT2) And (r <= cFRS_ERR_PTN)) Then
                ' Z, Z2��Off�ʒu�Ɉړ�����
                r = Sub_ZOnOff(0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? 
                    rtn = r
                    GoTo STP_ERR_EXIT
                End If

                ' BP�����̈ʒu�Ɉړ�����(��Έړ�)
                r = Form1.System1.EX_MOVE(gSysPrm, SvBpPosX, SvBpPosY, 1)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                    rtn = r
                    GoTo STP_ERR_EXIT
                End If

                ' �w�x�X�e�[�W�����̈ʒu�Ɉړ�����(��Έړ�)
                r = Form1.System1.EX_SMOVE2(gSysPrm, SvStgPosX, SvStgPosY)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? 
                    rtn = r
                    GoTo STP_ERR_EXIT
                End If
            End If

STP_ERR_EXIT:
            ' �摜�\���v���O�������I������
            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)
            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.FrmMatrix_Main() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "�}�g���b�N�X����"
    '''=========================================================================
    ''' <summary>�}�g���b�N�X����</summary>
    ''' <param name="BlkCnt">  (INP)�u���b�N��</param>
    ''' <param name="StpCount">(INP)�X�e�b�v��</param>
    '''=========================================================================
    Private Sub MatrixGenerate(ByVal BlkCnt As Integer, ByVal StpCount As Integer)

        Dim BlkX As Integer
        Dim BlkY As Integer
        Dim LblSz As Integer
        Dim strMSG As String

        Try
            ' ���x���R���g���[���̕\���T�C�Y��ݒ肷��
            Select Case (StpCount)
                Case 3
                    LblSz = LBLSZ_20
                Case 2
                    LblSz = LBLSZ_30
                Case Else
                    LblSz = LBLSZ_40
            End Select

            ' ���x���R���g���[���z��̍쐬�iX�u���b�N,Y�u���b�N�j
            If (LabelAry Is Nothing = False) Then
                BlkX = LabelAry.GetLength(0)
                BlkY = LabelAry.GetLength(1)
                For BlkX = 0 To (LabelAry.GetLength(0) - 1)
                    For BlkY = 0 To (LabelAry.GetLength(1) - 1)
                        Form1.GrpMatrix.Controls.Remove(LabelAry(BlkX, BlkY))
                    Next BlkY
                Next BlkX
            End If
            LabelAry = New System.Windows.Forms.Label(BlkCnt, BlkCnt) {}

            ' ���x���R���g���[���̃C���X�^���X�쐬���A�v���p�e�B��ݒ肷��
            For BlkX = 1 To BlkCnt
                For BlkY = 1 To BlkCnt
                    '�C���X�^���X�쐬
                    LabelAry(BlkX, BlkY) = New System.Windows.Forms.Label
                    '�v���p�e�B�ݒ�
                    LabelAry(BlkX, BlkY).Name = "LblMatrix" + BlkX.ToString("00") + BlkY.ToString("00")
                    LabelAry(BlkX, BlkY).BorderStyle = BorderStyle.Fixed3D
                    LabelAry(BlkX, BlkY).Size = New Size(LblSz, LblSz)
                    LabelAry(BlkX, BlkY).Location = New Point(BlkX * LblSz, BlkY * LblSz)
                    LabelAry(BlkX, BlkY).BackColor = Color.White
                    ' �t�H�[���ɃR���g���[����ǉ�
                    Form1.GrpMatrix.Controls.Add(LabelAry(BlkX, BlkY))
                Next BlkY
            Next BlkX

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.MatrixGenerate() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�}�g���b�N�X�\��������"
    '''=========================================================================
    ''' <summary>�}�g���b�N�X�\��������</summary>
    ''' <param name="BlkCnt">(INP)�u���b�N��</param>
    '''=========================================================================
    Private Sub MatrixInit(ByVal BlkCnt As Integer)

        Dim BlkX As Integer
        Dim BlkY As Integer
        Dim strMSG As String

        Try
            ' �}�g���b�N�X�\��������
            For BlkX = 1 To BlkCnt
                For BlkY = 1 To BlkCnt
                    LabelAry(BlkX, BlkY).BackColor = Color.White
                Next BlkY
            Next BlkX
            Form1.GrpMatrix.Refresh()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.MatrixInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�}�g���b�N�X�̃o�b�N�J���[��ݒ肷��"
    '''=========================================================================
    ''' <summary>�}�g���b�N�X�̃o�b�N�J���[��ݒ肷��</summary>
    ''' <param name="BlkX">(INP)�}�g���b�N�X�̃u���b�NX</param>
    ''' <param name="BlkY">(INP)�}�g���b�N�X�̃u���b�NY</param>
    ''' <param name="OkNg">(INP)0=LBL_OK, 1=LBL_NG</param>
    '''=========================================================================
    Private Sub MatrixDisp(ByVal BlkX As Integer, ByVal BlkY As Integer, ByVal OkNg As Integer)

        Dim strMSG As String

        Try
            ' �}�g���b�N�X�̃o�b�N�J���[��ݒ肷��
            If (OkNg = LBL_OK) Then
                LabelAry(BlkX, BlkY).BackColor = Color.LimeGreen
            Else
                LabelAry(BlkX, BlkY).BackColor = Color.Red
            End If
            Form1.GrpMatrix.Refresh()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.MatrixDisp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�p�^�[���}�b�`���O�ɂ��P��R���̈ʒu�␳�����߂�"
    '''=========================================================================
    ''' <summary>�p�^�[���}�b�`���O�ɂ��P��R���̈ʒu�␳�����߂�</summary>
    ''' <param name="Grp">      (INP)�O���[�v�ԍ�</param>
    ''' <param name="Ptn">      (INP)�p�^�[���ԍ�</param>
    ''' <param name="fCorrectX">(OUT)�����X</param> 
    ''' <param name="fCorrectY">(OUT)�����Y</param> 
    ''' <param name="Coeff">    (OUT)��v�x</param> 
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Sub_PatternMatching(ByVal Grp As Short, ByVal Ptn As Short, _
                                         ByVal BpX As Double, ByVal BpY As Double, _
                                         ByRef fCorrectX As Double, ByRef fCorrectY As Double, ByRef Coeff As Double) As Integer

        Dim ret As Short = cFRS_NORMAL
        Dim crx As Double = 0.0                                         ' �����X
        Dim cry As Double = 0.0                                         ' �����Y
        Dim fCoeff As Double = 0.0                                      ' ���֒l
        Dim Thresh As Double = 0.0                                      ' 臒l
        Dim r As Integer = cFRS_NORMAL                                  ' �֐��l
        Dim strMSG As String

        Try
#If VIDEO_CAPTURE = 1 Then
        fCorrectX = 0.0
        fCorrectY = 0.0
        Return (cFRS_NORMAL)   
#End If
            ' �p�^�[���}�b�`���O���̃e���v���[�g�O���[�v�ԍ���ݒ肷��(������ƒx���Ȃ�)
            'If (giTempGrpNo <> Grp) Then                                ' �e���v���[�g�O���[�v�ԍ����ς���� ?
            '    giTempGrpNo = Grp                                       ' ���݂̃e���v���[�g�O���[�v�ԍ���ޔ�
            '    Form1.VideoLibrary1.SelectTemplateGroup(giTempGrpNo)    ' �e���v���[�g�O���[�v�ԍ��ݒ�
            'End If
            giTempGrpNo = Grp                                       ' ���݂̃e���v���[�g�O���[�v�ԍ���ޔ�
            Form1.VideoLibrary1.SelectTemplateGroup(giTempGrpNo)    ' �e���v���[�g�O���[�v�ԍ��ݒ�

            ' 臒l�擾
            Thresh = gDllSysprmSysParam_definst.GetPtnMatchThresh(giTempGrpNo, Ptn)

            ' �p�[�^�[���ʒuXY��BP�ړ�(��Βl)
            r = Form1.System1.EX_MOVE(gSysPrm, BpX, BpY, 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            Form1.System1.WAIT(0.3)                                     ' Wait(Sec)
            Call Form1.VideoLibrary1.PatternDisp(True)                  ' �����͈͘g�\�� 

            ' �p�^�[���}�b�`���O���s��(Video.ocx���g�p)
            ret = Form1.VideoLibrary1.PatternMatching_EX(Ptn, 0, True, crx, cry, fCoeff)
            If (ret <> cFRS_NORMAL) Then
                r = cFRS_ERR_PTN                                        ' RETURN�l = �p�^�[���}�b�`�B���O�G���[
            ElseIf (fCoeff < Thresh) Then                               ' ��v�x
                r = cFRS_ERR_PTN                                        ' RETURN�l = �p�^�[���}�b�`�B���O�G���[
            Else
                ' �}�b�`�����p�^�[���̑���ʒu���炸��ʂ����߂�
                fCorrectX = crx
                fCorrectY = cry
                Coeff = fCoeff                                          ' ��v�x
                r = cFRS_NORMAL                                         ' Return�l = ����
            End If

            ' �㏈��
            Debug.Print("X=" & fCorrectX.ToString("0.0000") & " Y=" & fCorrectY.ToString("0.0000"))
            Call Form1.VideoLibrary1.PatternDisp(False)                 ' �����͈͘g��\�� 

            ' �p�^�[���F���G���[�Ȃ烁�b�Z�[�W�\��(START�L�[�����҂�)
            If (r <> cFRS_NORMAL) Then                                  ' �p�^�[���F���G���[ ? 
                '   ' ���[�_���������[�h���̓��O�\����Ƀ��b�Z�[�W�\������
                If (giHostMode = cHOSTcMODEcAUTO) Then
                    ' "�p�^�[���F���G���[ Group No.=x, Pattern No.=x"
                    strMSG = MSG_127 + " Group No.=" + giTempGrpNo.ToString("0") + ", Pattern No.=" + Ptn.ToString("0")
                    Call Form1.Z_PRINT(strMSG)

                    ' �蓮���[�h���̓��b�Z�[�W�\������START�L�[�����҂�
                Else
                    ' "�p�^�[���F���G���[", "Group No.=x, Pattern No.=x"
                    strMSG = "Group No.=" + giTempGrpNo.ToString("0") + ", Pattern No.=" + Ptn.ToString("0")
                    ret = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                            MSG_127, strMSG, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (ret < cFRS_NORMAL) Then Return (ret) '          ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                End If

            End If

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.CutPosPatternMatching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�X�e�b�v�񐔕���R������s��"
    '''=========================================================================
    ''' <summary>�X�e�b�v�񐔕���R������s��</summary>
    ''' <param name="BlkCnt">(INP)�u���b�N��</param>
    ''' <param name="CtPosX">(INP)���S���WX</param>
    ''' <param name="CtPosY">(INP)���S���WY</param>
    ''' <param name="Pitch"> (INP)�ړ��s�b�`(mm)</param>
    ''' <returns>0    = ����(����OK)
    '''          1    = �S��R����NG
    '''          0�ȉ�= �G���[</returns>
    '''=========================================================================
    Private Function StepMeasure(ByVal BlkCnt As Integer, ByVal CtPosX As Double, ByVal CtPosY As Double, ByVal Pitch As Double) As Integer

        Dim bFlg As Boolean = False
        Dim r As Integer
        Dim rtn As Integer = cFRS_NORMAL
        Dim BlkX As Integer
        Dim BlkY As Integer
        Dim EndY As Integer
        Dim StepY As Integer
        Dim sts As Long = 0
        Dim StgPosX As Double
        Dim StgPosY As Double
        Dim StartPosX As Double
        Dim StartPosY As Double
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' ����ʒu�|�W�V�����Z�o
            'If (giAppMode = APP_MODE_PROBE) Then                        ' �v���[�u�R�}���h���͌��ݍ��W����J�n����
            '    StgPosX = CtPosX
            '    StgPosY = CtPosY
            'Else                                                        ' ����ʒu�|�W�V�����Z�o
            '    StgPosX = CtPosX + Pitch * ((BlkCnt - 1) / 2)
            '    StgPosY = CtPosY - Pitch * ((BlkCnt - 1) / 2)
            'End If
            StgPosX = CtPosX + Pitch * ((BlkCnt - 1) / 2)
            StgPosY = CtPosY - Pitch * ((BlkCnt - 1) / 2)

            strMSG = StgPosX.ToString("0.0000")
            StgPosX = Double.Parse(strMSG)
            strMSG = StgPosY.ToString("0.0000")
            StgPosY = Double.Parse(strMSG)
            StartPosX = StgPosX                                         ' �J�n���WXY��ޔ� 
            StartPosY = StgPosY

            ' �摜�\���v���O�������N������(���N����)
            'V6.0.0.0�D            If (ObjGazou Is Nothing) Then
            ' ������ V3.1.0.0�A 2014/12/01
            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
            'V6.0.0.0�D                r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
            ' ������ V3.1.0.0�A 2014/12/01
            'V6.0.0.0�D            End If

            ' �X�e�b�v�񐔕���R������s��
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            For BlkX = 1 To BlkCnt                                      ' X�u���b�N�����J��Ԃ�
                ' �X�e�[�W�̈ړ����WX��ݒ肷��
                StgPosX = StartPosX - Pitch * (BlkX - 1)                ' X�͏�Ɂ������ֈړ����� 

                ' Y�����̃X�e�b�v������ݒ肷��
                If ((BlkX Mod 2) <> 0) Then                             ' X����u���b�N�Ȃ�Y�́����� 
                    BlkY = 1
                    EndY = BlkCnt
                    StepY = 1
                Else                                                    ' X�������u���b�N�Ȃ�Y�́����� 
                    BlkY = BlkCnt
                    EndY = 1
                    StepY = -1
                End If

                ' Y�u���b�N�����J��Ԃ� 
                For BlkY = BlkY To EndY Step StepY                      ' Y�́��������́������ֈړ����� 
                    ' �V�X�e���G���[�`�F�b�N
                    r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                    If (r <> cFRS_NORMAL) Then                          ' ����~�� ?
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If

                    ' HALT�L�[�����`�F�b�N
                    Call HALT_SWCHECK(sts)
                    If (sts = cSTS_HALTSW_ON) Then
                        ' Z,Z2��Off�ʒu�Ɉړ�����
                        r = Sub_ZOnOff(0)
                        If (r <> cFRS_NORMAL) Then                      ' �G���[ ? 
                            rtn = r
                            GoTo STP_ERR_EXIT
                        End If

                        ' �����v����
                        Call LAMP_CTRL(LAMP_START, True)                ' START�����vON
                        Call LAMP_CTRL(LAMP_RESET, True)                ' RESET�����vON

                        ' ���b�Z�[�W�\��(START�L�[/RESET�L�[�����҂�)
                        '  "�ꎞ��~���ł�" "START�L�[�F�������s�CRESET�L�[�F�����I��" 
                        r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                                MSG_SPRASH39, MSG_SPRASH35, "", System.Drawing.Color.Blue, System.Drawing.Color.Black, System.Drawing.Color.Black)
                        If (r < cFRS_NORMAL) Then                       ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)               
                            rtn = r
                            GoTo STP_ERR_EXIT
                        ElseIf (r = cFRS_ERR_RST) Then
                            rtn = cFRS_ERR_RST
                            Call ZCONRST()                              ' �R���\�[���L�[���b�`����
                            GoTo STP_EXIT
                        End If

                        ' �����v����
                        Call LAMP_CTRL(LAMP_START, False)               ' START�����vOFF
                        Call LAMP_CTRL(LAMP_RESET, False)               ' RESET�����vOFF
                        Call ZCONRST()                                  ' �R���\�[���L�[���b�`����
                        Form1.Refresh()                                 ' ���b�Z�[�WForm�������� 
                    End If

                    ' Z,Z2��Off�ʒu�Ɉړ�����
                    r = Sub_ZOnOff(0)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If

                    ' �w�x�X�e�[�W�̈ړ����WY��ݒ肷��
                    StgPosY = StartPosY + Pitch * (BlkY - 1)

                    ' �w�x�X�e�[�W���ړ�����(��Έړ�)
                    Console.WriteLine("BlkX=" + BlkX.ToString("0") + ", BlkY=" + BlkY.ToString("0"))
                    Console.WriteLine("StgPosX=" + StgPosX.ToString("0.0000") + ", StgPosY=" + StgPosY.ToString("0.0000"))
                    r = Form1.System1.EX_SMOVE2(gSysPrm, StgPosX, StgPosY)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If
                    System.Threading.Thread.Sleep(50)                   ' Wait(ms)

                    ' Z,Z2��On�ʒu�Ɉړ�����
                    r = Sub_ZOnOff(1)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If

                    ' �S��R������s��
                    r = MeasureAllResistors()
                    If (r < cFRS_NORMAL) Then                           ' �G���[ ? 
                        rtn = r
                        GoTo STP_ERR_EXIT
                    End If
                    If (bFlg = False) And (r = cFRS_NORMAL) Then
                        bFlg = True                                     ' bFlg = ����OK 
                    End If

                    ' �}�g���b�N�X�̃o�b�N�J���[��ݒ肷��(����OK=��, ����NG=��)
                    Call MatrixDisp(BlkX, BlkY, r)

                    ' �X�e�[�W���W�I�t�Z�b�gX,Y�ޔ�
                    strMSG = (StgPosX - CtPosX).ToString("0.0000")      ' ���S���W����̃I�t�Z�b�g�l��ݒ� 
                    StgPosAryX(BlkX) = Double.Parse(strMSG)
                    strMSG = (StgPosY - CtPosY).ToString("0.0000")
                    StgPosAryY(BlkY) = Double.Parse(strMSG)

                Next BlkY

            Next BlkX

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
STP_EXIT:
            ' Z,Z2��Off�ʒu�Ɉړ�����
            r = Sub_ZOnOff(0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                rtn = r
                GoTo STP_ERR_EXIT
            End If

            ' �w�x�X�e�[�W�����̈ʒu(���S)�Ɉړ�����(��Έړ�)
            r = Form1.System1.EX_SMOVE2(gSysPrm, CtPosX, CtPosY)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                rtn = r
                GoTo STP_ERR_EXIT
            End If
            System.Threading.Thread.Sleep(100)                          ' Wait(ms)

            ' �߂�l��ݒ�
            If (rtn <> cFRS_ERR_RST) Then
                If (bFlg = True) Then
                    rtn = cFRS_NORMAL                                   ' Return�l = ����OK�ƂȂ�����R������
                Else
                    rtn = 1                                             ' Return�l = �S��R����NG
                End If
            End If

STP_ERR_EXIT:
            ' �摜�\���v���O�������I������
            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.StepMeasure() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�S��R������s��"
    '''=========================================================================
    ''' <summary>�S��R������s��</summary>
    ''' <returns>0=����OK
    '''          1=����NG 
    '''          0,1�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function MeasureAllResistors() As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim Rn As Integer
        Dim Hp As Integer
        Dim Lp As Integer
        Dim Ag1 As Integer
        Dim Ag2 As Integer
        Dim Ag3 As Integer
        Dim Ag4 As Integer
        Dim Ag5 As Integer
        Dim Slp As Integer
        Dim RangeType As Integer                                        ' �����W�ݒ�^�C�v�i0:�I�[�g�����W�A1:�Œ背���W-�ڕW�l�w��A2:�Œ背���W-�����W�ԍ��w��j
        Dim TarGetVal As Double
        Dim MeasVal As Double
        Dim gfInitH As Double
        Dim gfInitL As Double
        Dim strMSG As String

        Try
            ' �S��R����(�Œ背���W-�ڕW�l�w��)
            For Rn = 1 To gRegistorCnt                                  ' ��R�����ݒ肷��

                ' �V�X�e���G���[�`�F�b�N
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' ����~�� ?
                    Return (r)
                End If

                ' �}�[�L���O�p��R�ȊO ?
                If (typResistorInfoArray(Rn).intResNo < 1000) Then
                    ' �v���[�u�ԍ�(H,L,G1�`G5)
                    Hp = typResistorInfoArray(Rn).intProbHiNo
                    Lp = typResistorInfoArray(Rn).intProbLoNo
                    Ag1 = typResistorInfoArray(Rn).intProbAGNo1
                    Ag2 = typResistorInfoArray(Rn).intProbAGNo2
                    Ag3 = typResistorInfoArray(Rn).intProbAGNo3
                    Ag4 = typResistorInfoArray(Rn).intProbAGNo4
                    Ag5 = typResistorInfoArray(Rn).intProbAGNo5
                    ' �ڕW�l
                    TarGetVal = typResistorInfoArray(Rn).dblTrimTargetVal
                    ' �d���ω��X���[�v(1:+�X���[�v, 2:-�X���[�v, 4:��R)
                    Slp = typResistorInfoArray(Rn).intSlope
                    If (typResistorInfoArray(Rn).intResMeasMode = 0) Then ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��)
                        Slp = 4
                    End If

                    ' ��R/�d��������s��
                    'RangeType = 1                                       ' �Œ背���W-�ڕW�l�w��
                    RangeType = 0                                       ' �I�[�g�����W
                    If (Slp = 4) Then                                   ' �d���ω��۰�� = 4(R) ?
                        ' ��R����
                        r = MFSET_EX("R", TarGetVal)
                        Call MSCAN(Hp, Lp, Ag1, Ag2, Ag3, Ag4, Ag5)     ' �X�L���i�[�ԍ��ݒ�
                        r = MEASURE(0, RangeType, 0, TarGetVal, 0, MeasVal)
                    ElseIf (Slp = 1) Or (Slp = 2) Then
                        ' �d������ 
                        r = MFSET_EX("V", TarGetVal)
                        Call MSCAN(Hp, Lp, Ag1, Ag2, Ag3, Ag4, Ag5)     ' �X�L���i�[�ԍ��ݒ�
                        r = MEASURE(1, RangeType, 0, TarGetVal, 0, MeasVal)
                    End If
                    ' ����G���[
                    If ((r <> cFRS_NORMAL) And (r <> ERR_MEAS_SPAN_SHORT) And (r <> ERR_MEAS_SPAN_OVER)) Then
                        Return (r)                                      ' Return�l = �G���[
                    End If

                    ' ����l���Z�o
                    gfInitH = TarGetVal * (1.0# + typResistorInfoArray(Rn).dblInitTest_HighLimit / 100.0#)
                    gfInitL = TarGetVal * (1.0# + typResistorInfoArray(Rn).dblInitTest_LowLimit / 100.0#)

                    ' �C�j�V����High OR �C�j�V����Low ����
                    If (MeasVal > gfInitH) Or (MeasVal < gfInitL) Then
                        Return (1)                                      ' Return�l = NG
                    End If
                End If
            Next Rn

            ' �I������
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.MeasureAllResistors() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "���S���W�ɋ߂��A����OK�̃X�e�[�W�I�t�Z�b�g����������"
    '''=========================================================================
    ''' <summary>���S���W�ɋ߂��A����OK�̃X�e�[�W�I�t�Z�b�g����������</summary>
    ''' <param name="BlkCnt"> (INP)�u���b�N��</param>
    ''' <param name="StgOfsX">(OUT)�X�e�[�W�I�t�Z�b�gX</param>
    ''' <param name="StgOfsY">(OUT)�X�e�[�W�I�t�Z�b�gY</param>
    '''=========================================================================
    Private Sub SerchStageOffset(ByVal BlkCnt As Integer, ByRef StgOfsX As Double, ByRef StgOfsY As Double)

        Dim Count As Integer
        Dim BlkX As Integer
        Dim BlkY As Integer

        Dim strMSG As String

        Try
            ' ��������
            StgOfsX = 0.0                                               ' �X�e�[�W�I�t�Z�b�g������
            StgOfsY = 0.0

            ' ���S���W����Q������Ɍ�������
            For Count = 1 To BlkCnt * BlkCnt                            ' �X�e�b�v�u���b�N�����J��Ԃ�
                ' �u���b�N�ԍ�X,Y�����߂�
                Call GetBlkNum(Count, BlkX, BlkY)
                ' ����OK�ƂȂ����u���b�N�ʒu����������
                If (LabelAry(BlkX, BlkY).BackColor = Color.LimeGreen) Then
                    StgOfsX = StgPosAryX(BlkX)
                    StgOfsY = StgPosAryY(BlkY)
                    Console.WriteLine("SerchStageOffset() BlkX=" + BlkX.ToString("0") + ", BlkY=" + BlkY.ToString("0") + ", OfsX=" + StgOfsX.ToString("0.0000") + ", OfsY=" + StgOfsY.ToString("0.0000"))
                    Exit Sub
                End If
            Next Count

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.SerchStageOffset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u���b�N�ԍ��z�������������"
    '''=========================================================================
    ''' <summary>�u���b�N�ԍ��z�������������</summary>
    ''' <param name="BlkCnt">  (INP)�u���b�N��</param>
    ''' <param name="StpCount">(INP)�X�e�b�v��</param>
    ''' <remarks>�����S����Q������Ɍ�������ׂ̔z��B
    ''' �@�@�@�@�@ �v�Z�ŋ��߂�悤�Ɏ��Ԃ�����ΕύX����</remarks>
    '''=========================================================================
    Private Sub InitSerchAry(ByVal BlkCnt As Integer, ByVal StpCount As Integer)

        Dim strMSG As String

        Try
            Select Case (StpCount)
                Case 3
                    SerchAryX(1) = 4                                    ' 1�@ X=4,Y=4
                    SerchAryY(1) = 4
                    SerchAryX(2) = 5                                    ' 2�@ X=5,Y=4
                    SerchAryY(2) = 4
                    SerchAryX(3) = 5                                    ' 3�@ X=5,Y=3
                    SerchAryY(3) = 3
                    SerchAryX(4) = 4                                    ' 4�@ X=4,Y=3
                    SerchAryY(4) = 3
                    SerchAryX(5) = 3                                    ' 5�@ X=3,Y=3
                    SerchAryY(5) = 3
                    SerchAryX(6) = 3                                    ' 6�@ X=3,Y=4
                    SerchAryY(6) = 4
                    SerchAryX(7) = 3                                    ' 7�@ X=3,Y=5
                    SerchAryY(7) = 5
                    SerchAryX(8) = 4                                    ' 8�@ X=4,Y=5
                    SerchAryY(8) = 5
                    SerchAryX(9) = 5                                    ' 9�@ X=5,Y=5
                    SerchAryY(9) = 5
                    SerchAryX(10) = 6                                    '10�@ X=6,Y=5
                    SerchAryY(10) = 5
                    SerchAryX(11) = 6                                    '11�@ X=6,Y=4
                    SerchAryY(11) = 4
                    SerchAryX(12) = 6                                    '12�@ X=6,Y=3
                    SerchAryY(12) = 3
                    SerchAryX(13) = 6                                    '13�@ X=6,Y=2
                    SerchAryY(13) = 2
                    SerchAryX(14) = 5                                    '14�@ X=5,Y=2
                    SerchAryY(14) = 2
                    SerchAryX(15) = 4                                    '15�@ X=4,Y=2
                    SerchAryY(15) = 2
                    SerchAryX(16) = 3                                    '16�@ X=3,Y=2
                    SerchAryY(16) = 2
                    SerchAryX(17) = 2                                    '17�@ X=2,Y=2
                    SerchAryY(17) = 2
                    SerchAryX(18) = 2                                    '18�@ X=2,Y=3
                    SerchAryY(18) = 3
                    SerchAryX(19) = 2                                    '19�@ X=2,Y=4
                    SerchAryY(19) = 4
                    SerchAryX(20) = 2                                    '20�@ X=2,Y=5
                    SerchAryY(20) = 5
                    SerchAryX(21) = 2                                    '21�@ X=2,Y=6
                    SerchAryY(21) = 6
                    SerchAryX(22) = 3                                    '22�@ X=3,Y=6
                    SerchAryY(22) = 6
                    SerchAryX(23) = 4                                    '23�@ X=4,Y=6
                    SerchAryY(23) = 6
                    SerchAryX(24) = 5                                    '24�@ X=5,Y=6
                    SerchAryY(24) = 6
                    SerchAryX(25) = 6                                    '25�@ X=6,Y=6
                    SerchAryY(25) = 6
                    SerchAryX(26) = 7                                    ' 26�@ X=7,Y=6
                    SerchAryY(26) = 6
                    SerchAryX(27) = 7                                    ' 27�@ X=7,Y=5
                    SerchAryY(27) = 5
                    SerchAryX(28) = 7                                    ' 28�@ X=7,Y=4
                    SerchAryY(28) = 4
                    SerchAryX(29) = 7                                    ' 29�@ X=7,Y=3
                    SerchAryY(29) = 3
                    SerchAryX(30) = 7                                    ' 30�@ X=7,Y=2
                    SerchAryY(30) = 2
                    SerchAryX(31) = 7                                    ' 31�@ X=7,Y=1
                    SerchAryY(31) = 1
                    SerchAryX(32) = 6                                    ' 32�@ X=6,Y=1
                    SerchAryY(32) = 1
                    SerchAryX(33) = 5                                    ' 33�@ X=5,Y=1
                    SerchAryY(33) = 1
                    SerchAryX(34) = 4                                    ' 34�@ X=4,Y=1
                    SerchAryY(34) = 1
                    SerchAryX(35) = 3                                    '35�@ X=3,Y=1
                    SerchAryY(35) = 1
                    SerchAryX(36) = 2                                    '36�@ X=2,Y=1
                    SerchAryY(36) = 1
                    SerchAryX(37) = 1                                    '37�@ X=1,Y=1
                    SerchAryY(37) = 1
                    SerchAryX(38) = 1                                    '38�@ X=1,Y=2
                    SerchAryY(38) = 2
                    SerchAryX(39) = 1                                    '39�@ X=1,Y=3
                    SerchAryY(39) = 3
                    SerchAryX(40) = 1                                    '40�@ X=1,Y=4
                    SerchAryY(40) = 4
                    SerchAryX(41) = 1                                    '41�@ X=1,Y=5
                    SerchAryY(41) = 5
                    SerchAryX(42) = 1                                    '42�@ X=1,Y=6
                    SerchAryY(42) = 6
                    SerchAryX(43) = 1                                    '43�@ X=1,Y=7
                    SerchAryY(43) = 7
                    SerchAryX(44) = 2                                    '44�@ X=2,Y=7
                    SerchAryY(44) = 7
                    SerchAryX(45) = 3                                    '45�@ X=3,Y=7
                    SerchAryY(45) = 7
                    SerchAryX(46) = 4                                    '46�@ X=4,Y=7
                    SerchAryY(46) = 7
                    SerchAryX(47) = 5                                    '47�@ X=5,Y=7
                    SerchAryY(47) = 7
                    SerchAryX(48) = 6                                    '48�@ X=6,Y=7
                    SerchAryY(48) = 7
                    SerchAryX(49) = 7                                    '49�@ X=7,Y=7
                    SerchAryY(49) = 7

                Case 2
                    SerchAryX(1) = 3                                    ' 1�@ X=3,Y=3
                    SerchAryY(1) = 3
                    SerchAryX(2) = 4                                    ' 2�@ X=4,Y=3
                    SerchAryY(2) = 3
                    SerchAryX(3) = 4                                    ' 3�@ X=4,Y=2
                    SerchAryY(3) = 2
                    SerchAryX(4) = 3                                    ' 4�@ X=3,Y=2
                    SerchAryY(4) = 2
                    SerchAryX(5) = 2                                    ' 5�@ X=2,Y=2
                    SerchAryY(5) = 2
                    SerchAryX(6) = 2                                    ' 6�@ X=2,Y=3
                    SerchAryY(6) = 3
                    SerchAryX(7) = 2                                    ' 7�@ X=2,Y=4
                    SerchAryY(7) = 4
                    SerchAryX(8) = 3                                    ' 8�@ X=3,Y=4
                    SerchAryY(8) = 4
                    SerchAryX(9) = 4                                    ' 9�@ X=4,Y=4
                    SerchAryY(9) = 4
                    SerchAryX(10) = 5                                    '10�@ X=5,Y=4
                    SerchAryY(10) = 4
                    SerchAryX(11) = 5                                    '11�@ X=5,Y=3
                    SerchAryY(11) = 3
                    SerchAryX(12) = 5                                    '12�@ X=5,Y=2
                    SerchAryY(12) = 2
                    SerchAryX(13) = 5                                    '13�@ X=5,Y=1
                    SerchAryY(13) = 1
                    SerchAryX(14) = 4                                    '14�@ X=4,Y=1
                    SerchAryY(14) = 1
                    SerchAryX(15) = 3                                    '15�@ X=3,Y=1
                    SerchAryY(15) = 1
                    SerchAryX(16) = 2                                    '16�@ X=2,Y=1
                    SerchAryY(16) = 1
                    SerchAryX(17) = 1                                    '17�@ X=1,Y=1
                    SerchAryY(17) = 1
                    SerchAryX(18) = 1                                    '18�@ X=1,Y=2
                    SerchAryY(18) = 2
                    SerchAryX(19) = 1                                    '19�@ X=1,Y=3
                    SerchAryY(19) = 3
                    SerchAryX(20) = 1                                    '20�@ X=1,Y=4
                    SerchAryY(20) = 4
                    SerchAryX(21) = 1                                    '21�@ X=1,Y=5
                    SerchAryY(21) = 5
                    SerchAryX(22) = 2                                    '22�@ X=2,Y=5
                    SerchAryY(22) = 5
                    SerchAryX(23) = 3                                    '23�@ X=3,Y=5
                    SerchAryY(23) = 5
                    SerchAryX(24) = 4                                    '24�@ X=4,Y=5
                    SerchAryY(24) = 5
                    SerchAryX(25) = 5                                    '25�@ X=5,Y=5
                    SerchAryY(25) = 5

                Case Else
                    SerchAryX(1) = 2                                    ' 1�@ X=2,Y=2
                    SerchAryY(1) = 2
                    SerchAryX(2) = 3                                    ' 2�@ X=3,Y=2
                    SerchAryY(2) = 2
                    SerchAryX(3) = 3                                    ' 3�@ X=3,Y=1
                    SerchAryY(3) = 1
                    SerchAryX(4) = 2                                    ' 4�@ X=2,Y=1
                    SerchAryY(4) = 1
                    SerchAryX(5) = 1                                    ' 5�@ X=1,Y=1
                    SerchAryY(5) = 1
                    SerchAryX(6) = 1                                    ' 6�@ X=1,Y=2
                    SerchAryY(6) = 2
                    SerchAryX(7) = 1                                    ' 7�@ X=1,Y=3
                    SerchAryY(7) = 3
                    SerchAryX(8) = 2                                    ' 8�@ X=2,Y=3
                    SerchAryY(8) = 3
                    SerchAryX(9) = 3                                    ' 9�@ X=3,Y=3
                    SerchAryY(9) = 3
            End Select

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.InitSerchAry() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�u���b�N�ԍ�X,Y��Ԃ�"
    '''=========================================================================
    ''' <summary>�u���b�N�ԍ�X,Y��Ԃ�</summary>
    ''' <param name="BlkNum">(INP)�u���b�N�ԍ�</param>
    ''' <param name="BlkX">  (OUT)�u���b�N�ԍ�X</param>
    ''' <param name="BlkY">  (OUT)�u���b�N�ԍ�Y</param>
    '''=========================================================================
    Public Sub GetBlkNum(ByVal BlkNum As Integer, ByRef BlkX As Integer, ByRef BlkY As Integer) 'V1.20.0.0�H

        Dim strMSG As String

        Try
            BlkX = SerchAryX(BlkNum)
            BlkY = SerchAryY(BlkNum)

            Console.WriteLine("GetBlkNum() BlkNum=" + BlkNum.ToString("0") + " X=" + BlkX.ToString("0") + ", Y=" + BlkY.ToString("0"))

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.GetNextBlkNum() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�e�b�v�����ƕ������玟�̕␳�ʒu(XY�e�[�u�����W)��Ԃ�"
    '''=========================================================================
    ''' <summary>�X�e�b�v�����ƕ������玟�̕␳�ʒu(XY�e�[�u�����W)��Ԃ�</summary>
    ''' <param name="InpPosX">(INP)�{���̕␳�ʒuX</param>
    ''' <param name="InpPosY">(INP)�{���̕␳�ʒuY</param>
    ''' <param name="Pitch">  (INP)�r�b�`</param>
    ''' <param name="TStep">  (INP)�g�[�^���X�e�b�v����</param>
    ''' <param name="DIR">    (I/O)�X�e�b�v����(1=0��(��), 2=45��, 3=90��(��), 4=135��,
    '''                                         5=180��(��), 6=225��, 7=270��(��), 8=315��)</param>
    ''' <param name="OutPosX">(OUT)���␳�ʒuX</param>
    ''' <param name="OutPosY">(OUT)���␳�ʒuX</param>
    '''=========================================================================
    Private Sub GetNextPos(ByVal InpPosX As Double, ByVal InpPosY As Double, ByVal Pitch As Double, ByVal TStep As Double, ByRef DIR As Integer, ByRef OutPosX As Double, ByRef OutPosY As Double)

        Dim strMSG As String

        Try
            Select Case (DIR)
                Case 1      ' �X�e�b�v����(1=��)
                    OutPosX = InpPosX + TStep                               ' X = �{���̕␳�ʒux + �g�[�^���X�e�b�v����
                    OutPosY = InpPosY                                       ' Y = �{���̕␳�ʒuy
                    DIR = 2

                Case 2      ' �X�e�b�v����(2=45��)
                    If (OutPosY + Pitch > InpPosY + TStep) Then             ' Y�����͖{���̕␳�ʒuy + �g�[�^���X�e�b�v�����𒴂���܂ŃX�e�b�v����
                        If (OutPosX - Pitch <= InpPosX) Then                ' X�����͖{���̍��W�܂�-�X�e�b�v����
                            DIR = 3
                            GoTo STP_DIR3
                        Else                                                ' X���W�����X�e�b�v������ -�X�e�b�v����
                            OutPosX = OutPosX - Pitch                       ' X = �O��̕␳�ʒuy - �X�e�b�v����
                            '                                               ' Y = �O��̕␳�ʒuy
                        End If
                    Else                                                    ' Y���W�����X�e�b�v������ +�X�e�b�v����
                        '                                                   ' X = �O��̕␳�ʒux
                        OutPosY = OutPosY + Pitch                           ' Y = �O��̕␳�ʒuy + �X�e�b�v����
                    End If

                Case 3      ' �X�e�b�v����(3=��)
STP_DIR3:
                    OutPosX = InpPosX                                       ' X = �{���̕␳�ʒux
                    OutPosY = InpPosY + TStep                               ' Y = �{���̕␳�ʒuy + �g�[�^���X�e�b�v����
                    DIR = 4

                Case 4      ' �X�e�b�v����(4=135��)
                    If (OutPosX - Pitch < InpPosX - TStep) Then             ' X�����͖{���̕␳�ʒux - �g�[�^���X�e�b�v�����𒴂���܂ŃX�e�b�v����
                        If (OutPosY - Pitch <= InpPosY) Then                ' Y�����͖{���̍��W�܂�-�X�e�b�v����
                            DIR = 5
                            GoTo STP_DIR5
                        Else                                                ' Y���W�����X�e�b�v������ -�X�e�b�v����
                            '                                               ' X = �O��̕␳�ʒux
                            OutPosY = OutPosY - Pitch                       ' Y = �O��̕␳�ʒuy - �X�e�b�v����
                        End If
                    Else                                                    ' X���W�����X�e�b�v������-����
                        OutPosX = OutPosX - Pitch                           ' X = �O��̕␳�ʒux -  �X�e�b�v����
                        '                                                   ' Y = �O��̕␳�ʒuy
                    End If

                Case 5      ' �X�e�b�v����(5=��)
STP_DIR5:
                    OutPosX = InpPosX - TStep                               ' X = �{���̕␳�ʒux - �g�[�^���X�e�b�v����
                    OutPosY = InpPosY                                       ' Y = �{���̕␳�ʒuy
                    DIR = 6

                Case 6      ' �X�e�b�v����(6=225��)
                    If (OutPosY - Pitch < InpPosY - TStep) Then             ' Y�����͖{���̕␳�ʒuy - �g�[�^���X�e�b�v�����𒴂���܂ŃX�e�b�v����
                        If (OutPosX + Pitch >= InpPosX) Then                ' X�����͖{���̍��W�܂�+�X�e�b�v����
                            DIR = 7
                            GoTo STP_DIR7
                        Else                                                ' X���W�����X�e�b�v������ +�X�e�b�v����
                            OutPosX = OutPosX + Pitch                       ' X = �O��̕␳�ʒux + �X�e�b�v����
                            '                                               ' Y = �O��̕␳�ʒuy
                        End If
                    Else                                                    ' X���W�����X�e�b�v������-����
                        '                                                   ' X = �O��̕␳�ʒux
                        OutPosY = OutPosY - Pitch                           ' Y = �O��̕␳�ʒuy - �g�[�^���X�e�b�v����
                    End If

                Case 7      ' �X�e�b�v����(7=��)
STP_DIR7:
                    OutPosX = InpPosX                                       ' X = �{���̕␳�ʒux
                    OutPosY = InpPosY - TStep                               ' Y = �{���̕␳�ʒuy - �g�[�^���X�e�b�v����
                    DIR = 8

                Case Else   ' �X�e�b�v����(8=315��)
                    If (OutPosX + Pitch > InpPosX + TStep) Then             ' X�����͖{���̕␳�ʒuy + �g�[�^���X�e�b�v�����𒴂���܂ŃX�e�b�v����
                        If (OutPosY + Pitch >= InpPosY) Then                ' Y�����͖{���̍��W�܂�+�X�e�b�v����
                            DIR = 9                                         ' 1�����T�[�`�����̂ŃX�e�b�v������������
                        Else                                                ' X���W�����X�e�b�v������ +�X�e�b�v����
                            '                                               ' X = �O��̕␳�ʒux
                            OutPosY = OutPosY + Pitch                       ' Y = �O��̕␳�ʒuy + �X�e�b�v����
                        End If
                    Else                                                    ' X���W�����X�e�b�v������ +�X�e�b�v����
                        OutPosX = OutPosX + Pitch                           ' X = �O��̕␳�ʒux + �X�e�b�v����
                        '                                                   ' Y = �O��̕␳�ʒuy
                    End If
            End Select

            Console.WriteLine("GetThetaPos Return (OutPosX, OutPosY = " + Format(OutPosX, "0.0000") + ", " + Format(OutPosY, "0.0000") + ")")
            Console.WriteLine(" Step, Dir = " + Format(TStep, "0.0000") + ", " + Format(DIR, "0"))

STP_END:
            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmMatrix.GetNextPos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Z,Z2��On�܂���Off����"
    '''=========================================================================
    ''' <summary>Z,Z2��On�܂���Off����</summary>
    ''' <param name="OnOff">(INP)�u���b�N��</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Sub_ZOnOff(ByVal OnOff As Integer) As Integer 'V1.23.0.0�F

        Dim r As Integer
        Dim zStepPos As Double
        Dim strMSG As String

        Try
            ' Z�Ȃ��Ȃ�NOP
            If (gSysPrm.stDEV.giPrbTyp = 0) Then
                Return (cFRS_NORMAL)
            End If

            If (OnOff) Then
                '-------------------------------------------------------------------
                '   Z,Z2��On�ʒu�Ɉړ�����
                '-------------------------------------------------------------------
                ' Z��ON�ʒu�Ɉړ�����
                r = ZZMOVE(typPlateInfo.dblZOffSet, 1)
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                Else
                    Call LAMP_CTRL(LAMP_Z, True)                        ' Z�����vON
                End If

                ' Z2��ON�ʒu�Ɉړ�����
                If IsUnderProbe() Then                                  ' Z2�L�� ? 
                    r = Z2move(Z2ON)                                    ' Z2��ON�ʒu�Ɉړ�����
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                        Return (r)
                    End If
                    Call ZSTOPSTS2()                                    ' Z2�������~�҂�
                End If

                '-------------------------------------------------------------------
                '   Z,Z2��Off�ʒu�Ɉړ�����
                '-------------------------------------------------------------------
            Else
                ' Z2��OFF�ʒu�Ɉړ�����
                If IsUnderProbe() Then                                  ' Z2�L�� ? 
                    r = Z2move(Z2OFF)                                   ' Z2��OFF�ʒu�Ɉړ�����
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                        Return (r)
                    End If
                    Call ZSTOPSTS2()                                    ' Z2�������~�҂�
                End If

                ' Z���ï��&��߰Ĉʒu = ZON�ʒu - �ï�ߏ㏸���� 
                zStepPos = typPlateInfo.dblZOffSet - typPlateInfo.dblZStepUpDist
                ' �ï��&��߰Ĉʒu���ҋ@�ʒu��菬�����ꍇ�́A�ҋ@�ʒu��ï��&��߰Ĉʒu�Ƃ���
                If (zStepPos < typPlateInfo.dblZWaitOffset) Then zStepPos = typPlateInfo.dblZWaitOffset

                ' Z��ï��&��߰Ĉʒu�Ɉړ�(�㏸�ʒu)
                r = PROBOFF_EX(zStepPos)                                ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ���
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                Else
                    Call LAMP_CTRL(LAMP_Z, False)                       ' Z�����vOFF
                End If

            End If

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.Sub_ZOnOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�X�e�[�W���W�����߂�"
    '''=========================================================================
    ''' <summary>�X�e�[�W���W�����߂�</summary>
    ''' <param name="StgPosX">(INP)�}�g���b�N�X�̃u���b�NX</param>
    ''' <param name="StgPosY">(INP)�}�g���b�N�X�̃u���b�NY</param>
    '''=========================================================================
    Private Sub GetAProbStagePosition(ByRef StgPosX As Double, ByRef StgPosY As Double)

        Dim strMSG As String

        Try

            With typPlateInfo
                ' XY�e�[�u���ʒu = �g�����ʒu + �X�e�[�W�I�t�Z�b�g + ���␳�̂����
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY + gfCorrectPosY

                    Case 1 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY + gfCorrectPosY

                    Case 2 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY) + gfCorrectPosY

                    Case 3 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + ObjMain.stAPRB.dblAProbeStgPosX) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + ObjMain.stAPRB.dblAProbeStgPosY) + gfCorrectPosY
                End Select
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.GetAProbStagePosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�e�[�W���W(����ʒu)�����߂�"
    '''=========================================================================
    ''' <summary>�X�e�[�W���W(����ʒu)�����߂�</summary>
    ''' <param name="StgPosX">(INP)�}�g���b�N�X�̃u���b�NX</param>
    ''' <param name="StgPosY">(INP)�}�g���b�N�X�̃u���b�NY</param>
    '''=========================================================================
    Private Sub GetAProbStageMeasPosition(ByRef StgPosX As Double, ByRef StgPosY As Double)

        Dim strMSG As String

        Try

            With typPlateInfo
                ' XY�e�[�u���ʒu = �g�����ʒu + �X�e�[�W�I�t�Z�b�g + ���␳�̂����
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + gfCorrectPosY

                    Case 1 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2)) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY + .dblTableOffsetYDir + (.dblBlockSizeYDir / 2) + gfCorrectPosY

                    Case 2 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX + .dblTableOffsetXDir + (.dblBlockSizeXDir / 2) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2)) + gfCorrectPosY

                    Case 3 ' x��, y��
                        StgPosX = gSysPrm.stDEV.gfTrimX - (.dblTableOffsetXDir + (.dblBlockSizeXDir / 2)) + gfCorrectPosX
                        StgPosY = gSysPrm.stDEV.gfTrimY - (.dblTableOffsetYDir + (.dblBlockSizeYDir / 2)) + gfCorrectPosY
                End Select
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basMatrix.GetAProbStageMeasPosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region

End Module
