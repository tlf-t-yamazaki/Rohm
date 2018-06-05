Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TrimClassLibrary                'V6.0.0.0�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Public Class MainModules
    : Implements IMainModules           'V6.0.0.0�@

    '----- V1.13.0.0�B�� -----
    '----- �I�[�g�v���[�u���s�p�\���� -----
    Public Structure AutoProbe_Info                         ' �I�[�g�v���[�u���s�p�\���̌`����`
        Dim intAProbeGroupNo1 As Short                      ' �p�^�[��1(����)�p�O���[�v�ԍ�
        Dim intAProbePtnNo1 As Short                        ' �p�^�[��1(����)�p�p�^�[���ԍ�
        Dim intAProbeGroupNo2 As Short                      ' �p�^�[��2(���)�p�O���[�v�ԍ�
        Dim intAProbePtnNo2 As Short                        ' �p�^�[��2(���)�p�p�^�[���ԍ�
        Dim dblAProbeBpPosX As Double                       ' �p�^�[��1(����)�pBP�ʒuX
        Dim dblAProbeBpPosY As Double                       ' �p�^�[��1(����)�pBP�ʒuY
        Dim dblAProbeStgPosX As Double                      ' �p�^�[��2(���)�p�X�e�[�W�I�t�Z�b�g�ʒuX
        Dim dblAProbeStgPosY As Double                      ' �p�^�[��2(����)�p�X�e�[�W�I�t�Z�b�g�ʒuY
        Dim intAProbeStepCount As Short                     ' �X�e�b�v���s�p�X�e�b�v��
        Dim intAProbeStepCount2 As Short                    ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v��
        Dim dblAProbePitch As Double                        ' �X�e�b�v���s�p�X�e�b�v�s�b�`
        Dim dblAProbePitch2 As Double                       ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v�s�b�`
    End Structure
    Public stAPRB As AutoProbe_Info
    '----- V1.13.0.0�B�� -----

    '----- V1.23.0.0�F�� -----
    ' �v���[�u�`�F�b�N�@�\�p�g���~���O���ʍ\���̌`����`
    Public Structure ProbeChk_Info
        Dim FinalTest(,) As Double

        '�\���̂̏�����
        Public Sub Initialize(ByVal RegCount As Integer)
            ReDim FinalTest(0 To 2, 0 To RegCount)          ' �� ��R����+1�̗̈�m�ۂ��� 
        End Sub
    End Structure

    ' ���̑�
    Public stPrbChk As ProbeChk_Info = Nothing              ' �v���[�u�`�F�b�N�@�\�p�g���~���O���ʍ\���� 
    Public strPrbChkLogFile As String = ""                  ' �v���[�u�`�F�b�N���O�t�@�C����
    Public strPrbChkLogDate As String                       ' ���O���t(YYYY/MM/DDhh:mm:ss) 
    Public strPrbChkFileLog As String                       ' �t�@�C���o�͗p���O�G���A 
    Public strPrbChkDispLog As String                       ' �\���p���O�G���A 

    '----- V1.23.0.0�F�� -----

#Region "�K�x�[�W�R���N�^�Ƀ��������J��������"
    '''=========================================================================
    '''<summary>�K�x�[�W�R���N�^�Ƀ��������J��������</summary>
    '''=========================================================================
    Public Sub ReleaseMemory()
        GC.Collect()
    End Sub


#End Region

#Region "�A�v���P�[�V������ʂ�Ԃ�(OCX�p)"
    '''=========================================================================
    ''' <summary>�A�v���P�[�V������ʂ�Ԃ�</summary>
    ''' <param name="AppKind">0=TKY, 1=CHIP, 2=NET</param>
    '''=========================================================================
    Public Sub GetAppKind(ByRef AppKind As Short) Implements IMainModules.GetAppKind    'V6.0.0.0�@
        AppKind = gTkyKnd
    End Sub
#End Region

#Region "��R(�`�b�v)���ѕ�����Ԃ�(OCX�p)"
    '''=========================================================================
    ''' <summary>��R(�`�b�v)���ѕ�����Ԃ�</summary>
    ''' <param name="ResistDir">0=X����, 1=Y����</param>
    '''=========================================================================
    Public Sub GetResistDir(ByRef ResistDir As Short) Implements IMainModules.GetResistDir  'V6.0.0.0�@
        ResistDir = typPlateInfo.intResistDir
    End Sub
#End Region

#Region "�v���[�g���u���b�N��X�����AY�����̊J�n�ʒu�Z�o(OCX�p)"
    '''=========================================================================
    ''' <summary>�v���[�g���u���b�N��X�����AY�����̊J�n�ʒu�Z�o</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Call_CalcBlockXYStartPos() As Integer _
        Implements IMainModules.Call_CalcBlockXYStartPos  'V6.0.0.0�@

        Dim r As Integer

        r = CalcBlockXYStartPos()
        Return (r)

    End Function
#End Region

#Region "�w��u���b�NXY����X�e�[�W�ʒuXY���擾���e�[�u���ړ�����(OCX�p)"
    '''=========================================================================
    ''' <summary>�w��u���b�NXY����X�e�[�W�ʒuXY���擾���e�[�u���ړ�����</summary>
    ''' <param name="xBlockNo">(INP)�u���b�N�ԍ�X</param>
    ''' <param name="yBlockNo">(INP)�u���b�N�ԍ�Y</param>
    ''' <param name="OffSetX"> (INP)�I�t�Z�b�gX</param>
    ''' <param name="OffSetY"> (INP)�I�t�Z�b�gY</param>
    ''' <param name="stgx">    (OUT)�X�e�[�W�ʒuX</param>
    ''' <param name="stgy">    (OUT)�X�e�[�W�ʒuY</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>Call_CalcBlockXYStartPos()�̌��Call���鎖</remarks>
    '''=========================================================================
    Public Function Call_GetTargetStagePosByXY(ByVal xBlockNo As Integer, ByVal yBlockNo As Integer,
                                               ByVal OffSetX As Double, ByVal OffSetY As Double,
                                               ByRef stgx As Double, ByRef stgy As Double) As Integer _
                                           Implements IMainModules.Call_GetTargetStagePosByXY       'V6.0.0.0�@

        Dim r As Integer
        Dim PosX As Double
        Dim PosY As Double
        Dim AddSubPosX As Double = 0.0                                  ' V1.22.0.0�E
        Dim AddSubPosY As Double = 0.0                                  ' V1.22.0.0�E

        '----- ###249�� -----
        r = GetTargetStagePosByXY(xBlockNo, yBlockNo, stgx, stgy)
        '' �X�e�[�W�ړ�
        'Form1.System1.EX_START(gSysPrm, stgx + OffSetX, stgy + OffSetY, 0)

        '----- V1.22.0.0�E�� -----
        ' �X�e�b�v�I�t�Z�b�g�𔽉f 
        '----- V1.20.0.0�C�� -----
        r = GetStepOffSetPos(1, 1, AddSubPosX, AddSubPosY, xBlockNo, yBlockNo)
        PosX = stgx + OffSetX + AddSubPosX
        PosY = stgy + OffSetY + AddSubPosY

        ' ��EX_START()��BP��R�[�i�[���l�����Ă���̂ŉ��L�͕s�v(�O���J�����e�B�[�`���O�͕K�v)
        ' BP��R�[�i�[���l��
        If (giAppMode = APP_MODE_EXCAM_R1TEACH) Or (giAppMode = APP_MODE_EXCAM_TEACH) Then
            Select Case gSysPrm.stDEV.giBpDirXy                         ' V1.22.0.0�E
                Case 0 ' �E��(x��, y��)
                    PosX = stgx + OffSetX + AddSubPosX
                    PosY = stgy + OffSetY + AddSubPosY
                Case 1 ' ����(x��, y��)
                    PosX = stgx + (OffSetX * (-1)) + AddSubPosX
                    PosY = stgy + OffSetY + AddSubPosY
                Case 2 ' �E��(x��, y��)
                    PosX = stgx + OffSetX + AddSubPosX
                    PosY = stgy + (OffSetY * (-1)) + AddSubPosY
                Case 3 ' ����(x��, y��)
                    PosX = stgx + (OffSetX * (-1)) + AddSubPosX
                    PosY = stgy + (OffSetY * (-1)) + AddSubPosY
            End Select

            '----- V1.24.0.0�@�� -----
            If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                PosX = PosX - BSZ_6060_OFSX
                PosY = PosY - BSZ_6060_OFSY
            End If
            '----- V1.24.0.0�@�� -----
        End If
        '----- V1.20.0.0�C�� -----
        '----- V1.22.0.0�E�� -----
        '----- V4.0.0.0-40�� -----
        If (giMachineKd = MACHINE_KD_RS) Then
            ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
            ''----- V2.0.0.0�H�� -----
            If (giStageYOrg = STGY_ORG_UP) Then
                PosY = PosY
            Else
                PosY = PosY ' (typPlateInfo.dblBlockSizeYDir / 2)
            End If

            'If (giMachineKd = MACHINE_KD_RS) Then
            '    PosY = PosY + (typPlateInfo.dblBlockSizeYDir / 2)
            'End If
            ''----- V2.0.0.0�H�� -----
        End If
        '----- V4.0.0.0-40�� -----

        ' �X�e�[�W�ړ�
        r = Form1.System1.EX_START(gSysPrm, PosX, PosY, 0)
        'V1.19.0.0-31 ADD START
        If (r <> cFRS_NORMAL) Then                                      ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
            ' �����I��
            Call Form1.AppEndDataSave()
            Call Form1.AplicationForcedEnding()
        End If
        'V1.19.0.0-31 ADD END
        '----- ###249�� -----
        Return (r)

    End Function
#End Region

#Region "(Teaching����)���C����ʏ�̃N���X���C���̕\���ʒu��ύX����"
    '''=========================================================================
    '''<summary>���C����ʏ�̃N���X���C���̕\���ʒu��ύX����</summary>
    '''=========================================================================
    Public Sub SetCrossLinePos(ByVal xPos As Integer, ByVal yPos As Integer)
        ' �N���X���C���ʒu��ݒ肷�� 
        'V6.0.0.0�C        Form1.Picture1.Top = xPos + Form1.VideoLibrary1.Location.Y
        'V6.0.0.0�C        Form1.Picture2.Left = yPos + Form1.VideoLibrary1.Location.X

        ' ��ʂ̍ĕ`��
        'V6.0.0.0�C        Form1.Refresh()
        Form1.Instance.VideoLibrary1.SetCrossLineCenter(yPos, xPos)     'V6.0.0.0�C

    End Sub
#End Region

#Region "(Teaching����)�}�[�L���O�G���A�\��"
    '''=========================================================================
    '''<summary>���C����ʏ�̃}�[�L���O�G���A�̎l�p��\��/��\������</summary>
    '''=========================================================================
    Public Sub DisplayMarkingArea(ByVal bDisp As Boolean, ByVal xPos As Integer, ByVal yPos As Integer,
                                        ByVal width As Integer, ByVal height As Integer)
#If False Then              'V6.0.0.0�C
        If (bDisp = True) Then
            '�w����W��ݒ肵�}�[�L���O�G���A��\������B
            Form1.MarkingAreaLeft.Left = xPos + Form1.VideoLibrary1.Location.X
            Form1.MarkingAreaLeft.Top = yPos + Form1.VideoLibrary1.Location.Y
            Form1.MarkingAreaLeft.Height = height
            Form1.MarkingAreaUpper.Left = xPos + Form1.VideoLibrary1.Location.X
            Form1.MarkingAreaUpper.Top = yPos + Form1.VideoLibrary1.Location.Y
            Form1.MarkingAreaUpper.Width = width
            Form1.MarkingAreaRight.Left = xPos + Form1.VideoLibrary1.Location.X + width
            Form1.MarkingAreaRight.Top = yPos + Form1.VideoLibrary1.Location.Y
            Form1.MarkingAreaRight.Height = height
            Form1.MarkingAreaLow.Left = xPos + Form1.VideoLibrary1.Location.X
            Form1.MarkingAreaLow.Top = yPos + Form1.VideoLibrary1.Location.Y + height
            Form1.MarkingAreaLow.Width = width

            '���C���̕\��
            Form1.MarkingAreaLeft.Visible = True
            Form1.MarkingAreaLow.Visible = True
            Form1.MarkingAreaRight.Visible = True
            Form1.MarkingAreaUpper.Visible = True
        Else
            '���C���̔�\��
            Form1.MarkingAreaLeft.Visible = False
            Form1.MarkingAreaLow.Visible = False
            Form1.MarkingAreaRight.Visible = False
            Form1.MarkingAreaUpper.Visible = False
        End If
        ' ��ʂ̃��t���b�V��
        Form1.Refresh()
#Else
        Form1.Instance.VideoLibrary1.SetMarkingArea(bDisp, xPos, yPos, width, height)
#End If
    End Sub
#End Region

    'V5.0.0.6�K��
#Region "�N���X���C����\��"
    Public Sub CrossLineDispOff() Implements IMainModules.CrossLineDispOff      'V6.0.0.0�@
        Try
#If False Then      'V6.0.0.0�C
            Form1.CrosLineX.Visible = False
            Form1.CrosLineY.Visible = False
            Form1.CrosLineX.Refresh()
            Form1.CrosLineY.Refresh()
            Call Form1.VideoLibrary1.Refresh()
#End If
            Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)     'V6.0.0.0�C

        Catch ex As Exception
            MsgBox("MainModules.CrossLineDispOff() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region
    'V5.0.0.6�K��

#Region "(Teaching-Jog����)�␳�N���X���C���\������"
    '''=========================================================================
    ''' <summary>(Teaching-Jog����)�␳�N���X���C���\������</summary>
    ''' <param name="xPos">(INP)BP�ʒuX(mm)</param>
    ''' <param name="yPos">(INP)BP�ʒuY(mm)</param>
    '''=========================================================================
    Public Sub DispCrossLine(ByVal xPos As Double, ByVal yPos As Double) _
        Implements IMainModules.DispCrossLine                           'V6.0.0.0�@

        Dim strMSG As String

        Try
            '----- ###232�� -----
            '�N���X���C���␳�������Ăяo��
            ObjCrossLine.CrossLineDispXY(xPos, yPos)

            ''�N���X���C���␳�������Ăяo��
            'gstCLC.x = xPos                        ' BP�ʒuX(mm)
            'gstCLC.y = yPos                        ' BP�ʒuY(mm)
            'Call CrossLineCorrect(gstCLC)          ' �␳�N���X���C���\��
            '----- ###232�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.DispCrossLine() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�B�� -----
#Region "�A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��(���[���a����)(OCX�p)"
    '''=========================================================================
    ''' <summary>�A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��</summary>
    ''' <param name="strDAT">  (INP)�G���[���b�Z�[�W</param>
    ''' <param name="ErrCode"> (INP)�G���[�R�[�h</param>
    ''' <remarks>���[���a����
    '''          OcxSystem����Call�����</remarks>
    '''=========================================================================
    Public Sub Call_SetAlmStartTime(ByVal strDAT As String, ByVal ErrCode As Short) _
        Implements IMainModules.Call_SetAlmStartTime                    'V6.0.0.0�@

        Dim strMSG As String

        Try
            ' ��������
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP

            ' ����p�A���[���J�n����ݒ肷��(���[���a����)
            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            Call SetAlmStartTime()

            ' �A���[���t�@�C���ɃA���[���f�[�^����������
            Call WriteAlarmData(gFPATH_QR_ALARM, strDAT, stPRT_ROHM.AlarmST_time, ErrCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Call_SetAlmStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�A���[����~����ݒ肷��(���[���a����)(OCX�p)"
    '''=========================================================================
    ''' <summary>�A���[����~����ݒ肷��</summary>
    ''' <remarks>���[���a����
    '''          OcxSystem����Call�����</remarks>
    '''=========================================================================
    Public Sub Call_SetAlmEndTime() Implements IMainModules.Call_SetAlmEndTime  'V6.0.0.0�@

        Dim strMSG As String

        Try
            ' ��������
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP

            ' ����p�A���[����~�I�����Ԃ�ݒ肷��(���[���a����)
            Call SetAlmEndTime()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Call_SetAlmEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '----- V1.18.0.0�B�� -----
    '----- V1.13.0.0�B�� -----
    '===========================================================================
    '   �I�[�g�v���[�u�p���\�b�h��`
    '===========================================================================
#Region "�I�[�g�v���[�u�p�}�g���b�N�X��ʕ\���T�u���[�`��"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u�p�}�g���b�N�X��ʕ\���T�u���[�`��</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Sub_CallFrmMatrix() As Integer Implements IMainModules.Sub_CallFrmMatrix    'V6.0.0.0�@

        Dim r As Integer
        Dim StgOfsX As Double
        Dim StgOfsY As Double
        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            ' giAppmode = �I�[�g�v���[�u���s���[�h�Ȃ烍�O�\�����\������
            If (giAppMode = APP_MODE_APROBEEXE) Then
                Form1.txtLog.Visible = True
            End If

            ' �}�g���b�N�X��ʕ\��
            r = ShowMatrixDialog(Me)

            ' �␳���ʂ̎擾
            r = GetMatrixReturn(StgOfsX, StgOfsY)
            If (r = cFRS_NORMAL) Then
                If (giAppMode = APP_MODE_PROBE) Then                    ' �v���[�u�R�}���h���̓I�t�Z�b�g�X�V���Ȃ�

                Else
                    gfStgOfsX = StgOfsX                                 ' XY�e�[�u���I�t�Z�b�gX(mm)
                    gfStgOfsY = StgOfsY                                 ' XY�e�[�u���I�t�Z�b�gY(mm)
                End If
            End If

            ' �A�v�������I��(�p�^�[���F���G���[�ȊO)
            If (r < cFRS_NORMAL) And ((r > cFRS_ERR_PTN) Or (r < cFRS_ERR_PT2)) Then
                Call Form1.AppEndDataSave()
                Call Form1.AplicationForcedEnding()                     ' �A�v�������I��
            End If
            Return (r)                                                  ' Return

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Sub_CallFrmRset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�I�[�g�v���[�u���s�p�\���̂ɃX�e�b�v���s�p�p�����[�^��ݒ肷��(OCX�p)"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u���s�p�\���̂ɃX�e�b�v���s�p�p�����[�^��ݒ肷��</summary>
    ''' <param name="StepCount">  (INP)�X�e�b�v��</param>
    ''' <param name="ProbePitch"> (INP)�X�e�b�v�s�b�`</param>
    ''' <param name="StepCount2"> (INP)�J�Ԃ��X�e�b�v��</param>
    ''' <param name="ProbePitch2">(INP)�J�Ԃ��X�e�b�v�s�b�`</param>
    '''=========================================================================
    Public Sub SetAPBPrm_Step(ByVal StepCount As Short, ByVal ProbePitch As Double, ByVal StepCount2 As Short, ByVal ProbePitch2 As Double) _
        Implements IMainModules.SetAPBPrm_Step                          'V6.0.0.0�@

        Dim strMSG As String

        Try
            stAPRB.intAProbeStepCount = StepCount
            stAPRB.dblAProbePitch = ProbePitch
            stAPRB.intAProbeStepCount2 = StepCount2
            stAPRB.dblAProbePitch2 = ProbePitch2

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Sub_CallFrmRset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�I�[�g�v���[�u���s�p�\���̂ɃX�e�b�v���s�p�p�^�[���p�����[�^��ݒ肷��(OCX�p)"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u���s�p�\���̂ɃX�e�b�v���s�p�p�^�[���p�����[�^��ݒ肷��</summary>
    ''' <param name="GroupNo1">(INP)�p�^�[��1(����)�p�O���[�v�ԍ�</param>
    ''' <param name="PtnNo1">  (INP)�p�^�[��1(����)�p�p�^�[���ԍ�</param>
    ''' <param name="GroupNo2">(INP)�p�^�[��2(���)�p�O���[�v�ԍ�</param>
    ''' <param name="PtnNo2">  (INP)�p�^�[��2(���)�p�p�^�[���ԍ�</param>
    ''' <param name="BpPosX">  (INP)�p�^�[��1(����)�pBP�ʒuX</param>
    ''' <param name="BpPosY">  (INP)�p�^�[��1(����)�pBP�ʒuY</param>
    ''' <param name="StgPosX"> (INP)�p�^�[��2(���)�p�X�e�[�W�I�t�Z�b�g�ʒuX</param>
    ''' <param name="StgPosY"> (INP)�p�^�[��2(���)�p�X�e�[�W�I�t�Z�b�g�ʒuY</param>
    '''=========================================================================
    Public Sub SetAPBPrm_Ptn(ByVal GroupNo1 As Short, ByVal PtnNo1 As Double, ByVal GroupNo2 As Short, ByVal PtnNo2 As Double,
                             ByVal BpPosX As Double, ByVal BpPosY As Double, ByVal StgPosX As Double, ByVal StgPosY As Double)

        Dim strMSG As String

        Try
            stAPRB.intAProbeGroupNo1 = GroupNo1
            stAPRB.intAProbePtnNo1 = PtnNo1
            stAPRB.intAProbeGroupNo2 = GroupNo2
            stAPRB.intAProbePtnNo2 = PtnNo2
            stAPRB.dblAProbeBpPosX = BpPosX
            stAPRB.dblAProbeBpPosY = BpPosY
            stAPRB.dblAProbeStgPosX = StgPosX
            stAPRB.dblAProbeStgPosY = StgPosY

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.SetAPBPrm_Ptn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0�B�� -----
    '----- V1.16.0.0�O�� -----
    '===========================================================================
    '   OCX����̃I�[�g���[�_�V���A���ʐM�p���\�b�h�Ăяo��(SL436�p)
    '===========================================================================
#Region "�z���m�F�X�e�[�^�X��Ԃ�(SL436R�p)"
    '''=========================================================================
    ''' <summary>�z���m�F�X�e�[�^�X��Ԃ�(SL436R�p)</summary>
    ''' <param name="Sts">(OUT)�X�e�[�^�X(1:�z���m�F, 0:�z�����m�F)</param>
    '''=========================================================================
    Public Sub Call_GetVacumeStatus(ByRef Sts As Integer) _
        Implements IMainModules.Call_GetVacumeStatus                    'V6.0.0.0�@

        Dim strMSG As String

        Try
            Call GetVacumeStatus(Sts)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Call_GetVacumeStatus() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.16.0.0�O�� -----
    '===========================================================================
    ' 'V3.0.0.0�B �r�f�I�X�^�[�g��~����
    '===========================================================================
#Region "Video"""
    ''' <summary>
    ''' �r�f�I�̃X�^�[�g����
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VideoStart() Implements IMainModules.VideoStart 'V6.0.0.0�@
        'V6.0.0.0-28        Try
        'V6.0.0.0-28            Call Form1.Instance.VideoLibrary1.VideoStart()
        'V6.0.0.0-28        Catch ex As Exception
        'V6.0.0.0-28            MsgBox("MainModules.VideoStart() TRAP ERROR = " + ex.Message)
        'V6.0.0.0-28        End Try
    End Sub
    ''' <summary>
    ''' �r�f�I�̃X�g�b�v����
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VideoStop() Implements IMainModules.VideoStop 'V6.0.0.0�@
        'V6.0.0.0-28        Try
        'V6.0.0.0-28        Call Form1.Instance.VideoLibrary1.VideoStop()
        'V6.0.0.0-28        Catch ex As Exception
        'V6.0.0.0-28            MsgBox("MainModules.VideoStop() TRAP ERROR = " + ex.Message)
        'V6.0.0.0-28        End Try
    End Sub

    ''' <summary>DllVideo.VideoLibrary�̔{�������g���b�N�o�[�\����Ԃ�ݒ肷��</summary>
    ''' <remarks>'V6.0.1.0�D</remarks>
    Public Sub SetVideoTrackBar(ByVal visible As Boolean, ByVal enabled As Boolean) Implements IMainModules.SetVideoTrackBar
        Form1.Instance.VideoLibrary1.SetTrackBar(visible, enabled)
    End Sub

    ''' <summary><para>�\������JOG�𐧌䂷��KeyDown,KeyUp���̏��������C���t�H�[���ɁA</para>
    ''' <para>�J�����摜MouseClick���̏�����DllVideo�ɐݒ肷��</para></summary>
    ''' <remarks>'V6.0.1.0�E</remarks>
    Public Sub SetActiveJogMethod(ByVal keyDown As Action(Of KeyEventArgs),
                                  ByVal keyUp As Action(Of KeyEventArgs),
                                  ByVal moveToCenter As Action(Of Decimal, Decimal)) Implements IMainModules.SetActiveJogMethod

        Form1.Instance.SetActiveJogMethod(keyDown, keyUp, moveToCenter)
    End Sub
#End Region
    '----- V2.0.0.0�A�� -----
#Region "���[�_�A���[���`�F�b�N(�����^�]�ȊO��(SL436RS�p))"
    '''=========================================================================
    ''' <summary>���[�_�A���[���`�F�b�N(�����^�]�ȊO��(SL436RS�p))</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Call_Loader_AlarmCheck_ManualMode() As Integer _
        Implements IMainModules.Call_Loader_AlarmCheck_ManualMode       'V6.0.0.0�@

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���[�_�A���[���`�F�b�N(�����^�]���ȊO)
            r = Loader_AlarmCheck_ManualMode(Form1.System1)             ' �G���[���̓��b�Z�[�W�\����
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Call_Loader_AlarmCheck_ManualMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V2.0.0.0�A�� -----
    '----- V1.23.0.0�F�� -----
    '===========================================================================
    '   �v���[�u�`�F�b�N�p���\�b�h��`(�I�v�V����)
    '===========================================================================
#Region "�v���[�u�`�F�b�N���C������(�v���[�u�`�F�b�N�@�\)"
    '''=========================================================================
    ''' <summary>�v���[�u�`�F�b�N���C������(�v���[�u�`�F�b�N�@�\)</summary>
    ''' <param name="digL">      (INP)Dig-SW Low</param>
    ''' <param name="PltCounter">(INP)��J�E���^</param>
    ''' <param name="ChkCounter">(I/O)�`�F�b�N�J�E���^</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function ProbeCheck(ByVal digL As Integer, ByVal PltCounter As Integer, ByRef ChkCounter As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim ChkBlkX As Integer
        Dim ChkBlkY As Integer
        Dim OffSetX As Double = 0.0
        Dim OffSetY As Double = 0.0
        Dim StgPosX As Double = 0.0
        Dim StgPosY As Double = 0.0
        Dim strMSG As String

        Try
            ' �v���[�u�`�F�b�N�@�\�Ȃ����̓`�F�b�N�����̎w��Ȃ����͎����^�]��(SL436R)�łȂ��Ȃ�NOP
            If ((giProbeCheck = 0) Or (typPlateInfo.intPrbChkPlt = 0) Or (bFgAutoMode <> True)) Then
                Return (cFRS_NORMAL)
            End If

            ' x0, x1, x2�ȊO�Ȃ�NOP
            If (digL > 2) Then
                Return (cFRS_NORMAL)
            End If

            ' ��J�E���^�ƃ`�F�b�N�J�E���^���������Ȃ����NOP
            If (PltCounter <> ChkCounter) Then
                Return (cFRS_NORMAL)
            End If
            ChkCounter = ChkCounter + typPlateInfo.intPrbChkPlt         ' �`�F�b�N�J�E���^�X�V

            ' �w��̃u���b�N�ʒu�ɃX�e�[�W���ړ�����
            If (typPlateInfo.intResistDir = 0) Then                     ' ��R���ѕ��� = X���� ?
                ChkBlkX = 1                                             ' �u���b�N�ԍ�X = 1
                ChkBlkY = typPlateInfo.intPrbChkBlk                     ' �u���b�N�ԍ�Y = �`�F�b�N�u���b�N�ԍ�
            Else
                ChkBlkX = typPlateInfo.intPrbChkBlk                     ' �u���b�N�ԍ�X = �`�F�b�N�u���b�N�ԍ�
                ChkBlkY = 1                                             ' �u���b�N�ԍ�Y = 1
            End If
            r = Call_GetTargetStagePosByXY(ChkBlkX, ChkBlkY, OffSetX, OffSetY, StgPosX, StgPosY)
            If (r <> cFRS_NORMAL) Then
                Return (r)
            End If

            ' �v���[�u�`�F�b�N�����s����
            rtn = Sub_ProbeCheck(PltCounter, ChkBlkX, ChkBlkY, typPlateInfo.dblPrbTestLimit, strPrbChkFileLog, strPrbChkDispLog)

            ' ���O���o�͂���(��ʂƃt�@�C��)
            If (strPrbChkDispLog <> "") Then
                Call Sub_ProbeCheckLogOut(strPrbChkFileLog)             ' ���O�t�@�C���ɏo�͂��� 
                If (rtn = cFRS_FNG_PROBCHK) Then                        ' ���O��ʕ\���̓v���[�u�`�F�b�N�G���[���̂ݕ\������ 
                    Call Form1.Z_PRINT(strPrbChkDispLog)                ' ���O��\������(�\�����[�h�ɂ�����炸)
                End If
            End If

            ' �v���[�u�`�F�b�N���s���ʂ��`�F�b�N����
            If (rtn = cFRS_FNG_PROBCHK) Then                            ' �v���[�u�`�F�b�N�G���[�Ȃ烁�b�Z�[�W�\��(START�L�[�����҂�)
                ' �V�O�i���^���[����(On=�ԓ_��+�u�U�[, Off=�S�ޯ�)
                'V5.0.0.9�M �� V6.0.3.0�G
                ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                'V5.0.0.9�M �� V6.0.3.0�G

                ' ���b�Z�[�W�\�� "�v���[�u�`�F�b�N�G���[","START�L�[����OK�{�^��������","�����^�]�𒆎~���܂�"
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                        MSG_LOADER_47, MSG_LOADER_38, MSG_LOADER_23, System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)

                ' �V�O�i���^���[����(On=�Ȃ�, Off=�ԓ_��+�u�U�[)
                'V5.0.0.9�M �� V6.0.3.0�G
                ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                'V5.0.0.9�M �� V6.0.3.0�G

                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                Return (cFRS_ERR_RST)                                   ' �v���[�u�`�F�b�N�G���[�Ȃ�Return�l = Cancel(RESET�L�[����) 
            End If

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.ProbeCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region

#Region "�v���[�u�`�F�b�N����(�v���[�u�`�F�b�N�@�\)"
    '''=========================================================================
    ''' <summary>�v���[�u�`�F�b�N����(�v���[�u�`�F�b�N�@�\)</summary>
    ''' <param name="Plt">   (INP)��ԍ�</param>
    ''' <param name="BlkX">  (INP)�u���b�N�ԍ�X</param>
    ''' <param name="BlkY">  (INP)�u���b�N�ԍ�Y</param>
    ''' <param name="Limit"> (INP)�덷�}%</param>
    ''' <param name="strLOG">(OUT)�t�@�C���o�͗p���O�G���A</param>
    ''' <param name="strDSP">(OUT)�\���p���O�G���A</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>�EPROBE.OCX�����Call�����
    '''          �E�X�e�[�W�͎w��u���b�N�̐��K�ʒu�ɂ��鎖</remarks> 
    '''=========================================================================
    Public Function Sub_ProbeCheck(ByVal Plt As Integer, ByVal BlkX As Integer, ByVal BlkY As Integer,
                                   ByVal Limit As Double, ByRef strLOG As String, ByRef strDSP As String) As Integer _
                               Implements IMainModules.Sub_ProbeCheck   'V6.0.0.0�@
        Dim bFlg As Boolean
        Dim PosX As Double = 0.0
        Dim PosY As Double = 0.0
        Dim StgOfsX As Double = 0.0
        Dim StgOfsY As Double = 0.0
        Dim BpOfsX As Double = 0.0
        Dim BpOfsY As Double = 0.0
        'Dim Delay As Integer
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim Count As Integer
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' ���ݍ��W�ޔ� 
            Call ZGETPHPOS2(PosX, PosY)

            ' �f�B���C�g�����Ȃ�m�[�}�����[�h�Ńv���[�g�f�[�^�𑗐M�ג���
            ' ���f�B���C�g�����Q��x0,x1���[�h���̂ݗL���Ȃ̂ŉ��L�͕s�v
            'Delay = typPlateInfo.intDelayTrim
            'If (Delay <> 0) Then
            '    typPlateInfo.intDelayTrim = 0
            '    r = SendTrimDataPlate(gTkyKnd, typPlateInfo.intResistCntInBlock, BpOfsX, BpOfsY)
            '    If (r <> cFRS_NORMAL) Then GoTo STP_ERR2
            'End If

            ' �v���[�u�`�F�b�N�@�\�p�g���~���O���ʍ\���̏�����
            stPrbChk.Initialize(gRegistorCnt)

            '-------------------------------------------------------------------
            '   �w��̃u���b�N�ŉ��L�R��̑�����s���A����l�̃o���c�L���덷�ȓ����`�F�b�N����
            '   �P��� - ���K�̈ʒu�ő��肷��
            '   �Q��� - ���K�̈ʒu����P�`�b�v���A�X�e�[�W���E�ɂÂ炵�đ��肷��
            '   �R��� - ���K�̈ʒu����P�`�b�v���A�X�e�[�W�����ɂÂ炵�đ��肷��
            '-------------------------------------------------------------------
            For Count = 1 To 3
                ' Z��ON�ʒu�Ɉړ�����
                r = Sub_ZOnOff(1)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' �G���[�Ȃ�APP�I��(���b�Z�[�W�\���͍ς�) 

                ' �P�u���b�N����������s����(x2���[�h)
                r = TRIMBLOCK(2, gSysPrm.stDEV.giPower_Cyc, 0, 0, 0)
                bFlg = IS_CV_OverLoadErrorCode(r)                       ' ����΂�����o/�I�[�o���[�h���o�`�F�b�N
                If (bFlg = False) Then                                  ' ����΂�����o/�I�[�o���[�h���o�Ȃ烁�b�Z�[�W�\�����Ȃ� 
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' ���̑��̃G���[�Ȃ烁�b�Z�[�W��\������
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT '       ' �G���[�Ȃ�APP�I��(���b�Z�[�W�\���͍ς�)  
                End If

                '' ����̔��茋�ʂ̎擾���遨gwTrimResult()
                'Call TrimLoggingResult_Get(RSLTTYP_TRIMJUDGE, 0)

                ' �t�@�C�i���e�X�g���茋��(����l)���擾���遨gfFinalTest()
                Call TrimLoggingResult_Get(RSLTTYP_FINAL_TEST, 0)

                ' ���茋�ʂ��`�F�b�N�����ʂ�ҏW����
                r = Sub_ProbeResultCheck(Count, gfFinalTest, Plt, BlkX, BlkY, Limit, strLOG, strDSP)
                If (r <> cFRS_NORMAL) Then
                    rtn = r                                             ' Return�l = �v���[�u�`�F�b�N�G���[ 
                End If

                ' Z��ҋ@�ʒu�Ɉړ�����
                r = Sub_ZOnOff(0)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' �G���[�Ȃ�APP�I��(���b�Z�[�W�\���͍ς�)  
                If (Count = 3) Then Exit For

                ' �X�e�[�W�ړ��I�t�Z�b�g��ݒ肷��
                If (typPlateInfo.intResistDir = 0) Then                 ' ���ߕ��т�X���� ?
                    If (Count = 1) Then
                        StgOfsX = typPlateInfo.dblChipSizeXDir
                    Else
                        StgOfsX = typPlateInfo.dblChipSizeXDir * -1
                    End If
                Else
                    If (Count = 1) Then
                        StgOfsY = typPlateInfo.dblChipSizeYDir
                    Else
                        StgOfsY = typPlateInfo.dblChipSizeYDir * -1
                    End If
                End If

                ' �X�e�[�W���P�`�b�v���E(��)�܂��͍�(��)�Ɉړ�����
                r = Form1.System1.EX_SMOVE2(gSysPrm, PosX + StgOfsX, PosY + StgOfsY)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' �G���[�Ȃ�APP�I��(���b�Z�[�W�\���͍ς�)  

            Next Count

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
            ' BP�I�t�Z�b�g�ʒu�ֈړ�(�v���[�u�R�}���h��)
            If (giAppMode = APP_MODE_PROBE) Then                        ' �v���[�u�R�}���h��
                r = Form1.System1.BPMOVE(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, 0, 0, 1)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '          ' �G���[�Ȃ�APP�I��(���b�Z�[�W�\���͍ς�)  
            End If

            ' �X�e�[�W�𐳋K�̈ʒu�ֈړ�����
            r = Form1.System1.EX_SMOVE2(gSysPrm, PosX, PosY)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '              ' �G���[�Ȃ�APP�I��(���b�Z�[�W�\���͍ς�)  

            ' �v���[�g�f�[�^�����ɖ߂�
            ' ���f�B���C�g�����Q��x0,x1���[�h���̂ݗL���Ȃ̂ŉ��L�͕s�v
            'If (Delay <> 0) Then
            '    typPlateInfo.intDelayTrim = Delay
            '    ' �g���~���O�f�[�^�𑗐M���Ȃ���(�v���[�g�f�[�^���M�ŃJ�b�g�f�[�^������������邽��)
            '    r = SendTrimData()
            '    If (r <> cFRS_NORMAL) Then GoTo STP_ERR2
            'End If

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Sub_ProbeCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try

STP_ERR_EXIT:
        ' �A�v�������I��
        Call Form1.AppEndDataSave()
        Call Form1.AplicationForcedEnding()                             ' �A�v�������I��
        Return (r)                                                      ' Return

STP_ERR2:
        ' "�g���~���O�f�[�^�̐ݒ�Ɏ��s���܂����B" & vbCrLf & "�g���~���O�f�[�^�ɖ�肪�Ȃ����m�F���Ă��������B"
        Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
        Return (cFRS_ERR_TRIM)                                          ' Return�l = �g���}�G���[
    End Function
#End Region

#Region "���茋�ʂ��`�F�b�N�����ʂ�ҏW���ĕԂ�(�v���[�u�`�F�b�N�@�\)"
    '''=========================================================================
    ''' <summary>���茋�ʂ��`�F�b�N�����ʂ�ҏW���ĕԂ�(�v���[�u�`�F�b�N�@�\)</summary>
    ''' <param name="Count">      (INP)�����(1-3)</param>
    ''' <param name="gfFinalTest">(INP)����l�̔z��</param>
    ''' <param name="Plt">        (INP)��ԍ�</param>
    ''' <param name="BlkX">       (INP)�u���b�N�ԍ�X</param>
    ''' <param name="BlkY">       (INP)�u���b�N�ԍ�Y</param>
    ''' <param name="Limit">      (INP)�덷�}%</param>
    ''' <param name="strLOG">     (OUT)�t�@�C���o�͗p���O�G���A</param>
    ''' <param name="strDSP">     (OUT)�\���p���O�G���A</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Sub_ProbeResultCheck(ByVal Count As Integer, ByRef gfFinalTest() As Double, ByVal Plt As Integer,
                    ByVal BlkX As Integer, ByVal BlkY As Integer, ByVal Limit As Double, ByRef strLOG As String, ByRef strDSP As String) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim Rn As Integer
        Dim GetRn As Integer
        Dim SetRn As Integer
        Dim LimitLo As Double
        Dim LimitHi As Double
        Dim dblDAT(0 To 2) As Double
        Dim dblDEV(0 To 1) As Double
        Dim strDAT(0 To 2) As String
        Dim strDEV As String
        Dim strRESULT As String
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' ���o����ݒ肷��
            If (Count = 1) Then
                strLOG = ""                                             ' �t�@�C���o�͗p���O�G���A�N���A
                strDSP = ""                                             ' �\���p���O�G���A�N���A

                '----- �t�@�C���o�͗p���O -----
                ' ���t(YYYY/MM/DD HH:mm:ss) 
                If (giAppMode <> APP_MODE_PROBE) Then                   ' �v���[�u�R�}���h�ȊO 
                    strLOG = Today.ToString("yyyy/MM/dd")
                    strLOG = strLOG + " " + TimeOfDay.ToString("HH:mm:ss") + vbCrLf

                    ' "Plate = 9 Block X=999 Y=999"
                    strLOG = strLOG + "Plate = " + Plt.ToString("0") + " Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strLOG = strLOG + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf

                Else                                                    ' �v���[�u�R�}���h�̏ꍇ 
                    ' "Block X=999 Y=999"
                    strLOG = strLOG + "Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strLOG = strLOG + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf
                End If

                '----- �\���p���O -----
                If (giAppMode <> APP_MODE_PROBE) Then                   ' �v���[�u�R�}���h�ȊO 
                    strDSP = "=== Probe Check ===" + vbCrLf
                    ' "Plate = 9 Block X=999 Y=999"
                    strDSP = strDSP + "Plate = " + Plt.ToString("0") + " Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strDSP = strDSP + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf
                Else
                    ' "Block X=999 Y=999"
                    strDSP = strDSP + "Block X=" + BlkX.ToString("000") + " Y=" + BlkY.ToString("000") + vbCrLf
                    ' "RNo      MEAS1       MEAS2                MEAS3      DEVIAT(%)         RESULT"
                    strDSP = strDSP + "RNo".PadLeft(4) + "MEAS1  ".PadLeft(17) + "MEAS2  ".PadLeft(17) +
                                   "MEAS3  ".PadLeft(17) + "     DEVIAT(%)  " + "   RESULT" + vbCrLf
                End If
#If cOFFLINEcDEBUG Then
                'Console.WriteLine(strLOG)                               ' For Debug 
                'Console.WriteLine(strDSP)                               ' For Debug 
#End If
            End If

            ' �擾�Ɛݒ�f�[�^�C���f�b�N�X��ݒ肷��
            If (Count = 1) Then                                         ' 1��ڂ͐��K�̈ʒu 
                GetRn = 0
                SetRn = 0
            ElseIf (Count = 2) Then                                     ' 2��ڂ�R1�̌��ʂ�R2����ݒ肷�� 
                GetRn = 0
                SetRn = 1
            Else                                                        ' 3��ڂ�R2�̌��ʂ�R1����ݒ肷��
                GetRn = 1
                SetRn = 0
            End If

            ' ����l���v���[�u�`�F�b�N�@�\�p�g���~���O���ʃf�[�^�ɐݒ肷��(�����W�I�[�p�́u9999999999.9999�v�ŋA��)
            For GetRn = GetRn To gRegistorCnt - 1
                If (typResistorInfoArray(GetRn).intResNo >= 1000) Then Continue For
                stPrbChk.FinalTest(Count - 1, SetRn) = gfFinalTest(GetRn)
                SetRn = SetRn + 1
            Next GetRn

            ' �R��ڂ̑���łȂ����Return
            If (Count < 3) Then Return (cFRS_NORMAL)

            '-------------------------------------------------------------------
            '   ���茋�ʂ�ҏW����
            '-------------------------------------------------------------------
            ' ���茋�ʂ��R�����ҏW����
            For Rn = 0 To gRegistorCnt - 1
                ' �P��ڂ̑���f�[�^
                dblDAT(0) = stPrbChk.FinalTest(0, Rn)
                ' 17�����E�l�O����
                strDAT(0) = String.Format("{0,17}", stPrbChk.FinalTest(0, Rn).ToString(gsEDIT_DIGITNUM))

                ' �Q��ڂ̑���f�[�^(�擪��R�͂Ȃ�)  
                If (Rn = 0) Then                                        ' �擪��R ? 
                    dblDAT(1) = stPrbChk.FinalTest(0, Rn)              ' �ŏ��̑���l���Q��ڂ̑���f�[�^�Ƃ���
                    strDAT(1) = String.Format("{0,17}", stPrbChk.FinalTest(2, Rn).ToString(gsEDIT_DIGITNUM))
                Else
                    dblDAT(1) = stPrbChk.FinalTest(1, Rn)
                    strDAT(1) = String.Format("{0,17}", stPrbChk.FinalTest(1, Rn).ToString(gsEDIT_DIGITNUM))
                End If

                ' �R��ڂ̑���f�[�^(�ŏI��R�͂Ȃ�) 
                If (Rn = (gRegistorCnt - 1)) Then                       ' �ŏI��R ? 
                    dblDAT(2) = stPrbChk.FinalTest(0, Rn)               ' �ŏ��̑���l���R��ڂ̑���f�[�^�Ƃ���
                Else
                    dblDAT(2) = stPrbChk.FinalTest(2, Rn)
                    strDAT(2) = String.Format("{0,17}", stPrbChk.FinalTest(2, Rn).ToString(gsEDIT_DIGITNUM))
                End If

                If (Rn = 0) Then                                        ' �擪��R ? 
                    strDAT(1) = "".PadLeft(17)                          ' �Q��ڂ̑���f�[�^�̕\���͂Ȃ�
                End If
                If (Rn = (gRegistorCnt - 1)) Then                       ' �ŏI��R ?
                    strDAT(2) = "".PadLeft(17)                          ' �R��ڂ̑���f�[�^�̕\���͂Ȃ�
                End If

                ' dblDAT(1)�ɒ��Ԓl��ݒ肷��(dblDAT(0)=�ŏ��l, dblDAT(2)=�ő�l)
                If (dblDAT(0) > dblDAT(1)) Then SwapDouble(dblDAT(0), dblDAT(1))
                If (dblDAT(0) > dblDAT(2)) Then SwapDouble(dblDAT(0), dblDAT(2))
                If (dblDAT(1) > dblDAT(2)) Then SwapDouble(dblDAT(1), dblDAT(2))

                ' �덷�����߂�
                If (dblDAT(1) = 0) Or
                  ((dblDAT(1) = 9999999999.9999) And ((dblDAT(0) <> 9999999999.9999) And (dblDAT(1) <> 9999999999.9999) And (dblDAT(2) <> 9999999999.9999))) Then
                    dblDEV(0) = 999.999
                    dblDEV(1) = 999.999
                Else
                    dblDEV(0) = (dblDAT(0) / dblDAT(1) - 1.0) * 100     ' (�ŏ��l/���Ԓl - 1) * 100
                    dblDEV(1) = (dblDAT(2) / dblDAT(1) - 1.0) * 100     ' (�ő�l/���Ԓl - 1) * 100
                End If

                If (dblDEV(0) > 999.999) Then
                    dblDEV(0) = 999.999
                End If
                If (dblDEV(0) < -999.999) Then
                    dblDEV(0) = -999.999
                End If
                If (dblDEV(1) > 999.999) Then
                    dblDEV(1) = 999.999
                End If
                If (dblDEV(1) < -999.999) Then
                    dblDEV(1) = -999.999
                End If

                ' �덷(�\���p)�����߂�
                strDEV = dblDEV(0).ToString("0.000").PadLeft(8) + " " + dblDEV(1).ToString("0.000").PadLeft(8)

                ' ����l�̃o���c�L���덷�ȓ����`�F�b�N����
                LimitLo = Math.Abs(Limit) * -1
                LimitHi = Math.Abs(Limit)
                strRESULT = "OK"
                If ((dblDEV(0) < LimitLo) Or (dblDEV(1) > LimitHi)) Then
                    strRESULT = "NG"
                    r = cFRS_FNG_PROBCHK                                ' Return�l = �v���[�u�`�F�b�N�G���[
                End If

                '���茋�ʂ��t�H�[�}�b�g�ϊ�����������\�z(16�������l��)
                ' �t�@�C���o�͗p���O
                ' "RNo      MEAS1       MEAS2         MEAS3          DEVIAT(%)     RESULT"
                strLOG = strLOG + typResistorInfoArray(Rn + 1).intResNo.ToString("0").PadLeft(4) +
                         strDAT(0) + strDAT(1) + strDAT(2) + strDEV + strRESULT.PadLeft(8) + vbCrLf
                Console.WriteLine(strLOG)                               ' For Debug 

                ' �\���p���O
                ' "RNo      MEAS1       MEAS2         MEAS3          DEVIAT(%)     RESULT"
                strDSP = strDSP + typResistorInfoArray(Rn + 1).intResNo.ToString("0").PadLeft(4) +
                         strDAT(0) + strDAT(1) + strDAT(2) + strDEV + strRESULT.PadLeft(8) + vbCrLf
#If cOFFLINEcDEBUG Then
               Console.WriteLine(strDSP)                               ' For Debug 
#End If
            Next Rn

            Return (r)                                                  ' Return

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Sub_ProbeResultCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region

#Region "�v���[�u�`�F�b�N���O�t�@�C������ݒ肷��(�v���[�u�`�F�b�N�@�\)"
    '''=========================================================================
    ''' <summary>�v���[�u�`�F�b�N���O�t�@�C������ݒ肷��</summary>
    ''' <param name="strDatPath">(INP)�g���~���O�f�[�^�t�@�C����</param>
    '''=========================================================================
    Public Sub Gen_ProbeCheckLogFile(ByRef strDatPath As String)

        Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim strFileName As String
        Dim strMSG As String

        Try
            ' �v���[�u�`�F�b�N�@�\�Ȃ����̓`�F�b�N�����̎w��Ȃ����͎����^�]��(SL436R)�łȂ��Ȃ�NOP
            If ((giProbeCheck = 0) Or (typPlateInfo.intPrbChkPlt = 0) Or (bFgAutoMode <> True)) Then
                Return
            End If

            ' ���O�t�@�C������ݒ肷��
            ' ���uProbeCheck_�g���~���O�f�[�^�t�@�C���� + ����(yyyymmddhhmmss).log�v
            strPrbChkLogDate = Today.ToString("yyyyMMdd") + TimeOfDay.ToString("HHmmss")
            strFileName = GetFileNameNonExtension(strDatPath)           ' ���ݸ��ް�̧�ٖ��̊g���q�𔲂������t�@�C���������o���ĕԂ�
            strPrbChkLogFile = gSysPrm.stLOG.gsLoggingDir + "ProbeCheck_" + strFileName + strPrbChkLogDate + ".log"

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Gen_ProbeCheckLogFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�v���[�u�`�F�b�N���O�o�͏���(�v���[�u�`�F�b�N�@�\)"
    '''=========================================================================
    ''' <summary>�v���[�u�`�F�b�N���O�o�͏���</summary>
    ''' <param name="strLOG">    (INP)���O�f�[�^</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Sub_ProbeCheckLogOut(ByRef strLOG As String) As Integer

        Dim hFStream As System.IO.FileStream
        'Dim writer As System.IO.StreamWriter
        'Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        'Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim strMSG As String

        Try
            ' ���O�t�@�C���̑��݃`�F�b�N
            If (System.IO.File.Exists(strPrbChkLogFile) = False) Then
                ' �t�@�C�������݂��Ȃ���΋�t�@�C���𐶐�����
                hFStream = System.IO.File.Create(strPrbChkLogFile)
                If (hFStream Is Nothing) Then
                    '���O�t�@�C���̏����݂ŃG���[���������܂��� File=xxxxxxxxxxxx"
                    strMSG = MSG_LOGERROR + " File=" + strPrbChkLogFile
                    Call Form1.Z_PRINT(strMSG)
                    Return (cFRS_FIOERR_OUT)                            ' Return�l = cFRS_FIOERR_OUT
                Else
                    hFStream.Close()
                End If
            End If

            ' ���O�t�@�C���I�[�v��
            '                                                           ' false = �㏑��(true = �ǉ�)
            'writer = New System.IO.StreamWriter(strPrbChkLogFile, True, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using writer As New StreamWriter(strPrbChkLogFile, True, Encoding.UTF8)     'V4.4.0.0-0
                ' ���O�f�[�^����������
                writer.WriteLine(strLOG)                                    ' ��"\r\n"�t���@�P���ɕ�������������ނɂ́AWrite() ���g�p����B

                '' �I������
                'writer.Close()                                              ' ���O�t�@�C���N���[�Y
            End Using

            Return (cFRS_NORMAL)                                        ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "MainModules.Sub_ProbeCheckLogOut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region


#Region "DLL����̃}�K�W���̒�~" 'V5.0.0.1-26
    Public Sub sub_MGStopJog() Implements IMainModules.sub_MGStopJog    'V6.0.0.0�@
        MGStopJog()
    End Sub
#End Region


#Region "Swap(Double)"
    '''=========================================================================
    ''' <summary>Swap(Double)</summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <remarks>a > b�Ȃ�a��b�����ւ���</remarks>
    '''=========================================================================
    Private Sub SwapDouble(ByRef a As Double, ByRef b As Double)

        Dim c As Double

        If (a > b) Then
            c = a
            a = b
            b = c
        End If

    End Sub
#End Region
    '----- V1.23.0.0�F�� -----
End Class
