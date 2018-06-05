'===============================================================================
'   Description  : �g���~���O����
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Globalization                        'V4.0.0.0-65
Imports System.IO                                   'V4.4.0.0-0
Imports System.Reflection                           'V4.0.0.0-65
Imports System.Runtime.InteropServices
'V6.0.0.0�A  Imports System.Runtime.Remoting                     'V3.0.0.0�D
'V6.0.0.0�A  Imports System.Runtime.Remoting.Channels            'V3.0.0.0�D
'V6.0.0.0�A  Imports System.Runtime.Remoting.Channels.Ipc        'V3.0.0.0�D
Imports System.Text                                 'V4.0.0.0-65
Imports System.Threading                            'V4.0.0.0-65
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources                'V4.4.0.0-0
Imports TrimClassLibrary    'V3.0.0.0�D      'V3.0.0.0�D  
Imports TrimControlLibrary              '#4.12.2.0�@

Module basTrimming
#Region "�萔/�ϐ���`"
    '===========================================================================
    '   �萔/�ϐ���`
    '===========================================================================
    '----- �g���~���O���� -----
    Public Const TRIM_RESULT_NOTDO As Short = 0                         ' �����{
    Public Const TRIM_RESULT_OK As Short = 1                            ' OK
    Public Const TRIM_RESULT_IT_NG As Short = 2                         ' ITNG(���[�N ��VB���ւ͕Ԃ�Ȃ�)
    Public Const TRIM_RESULT_FT_NG As Short = 3                         ' FTNG(���[�N ��VB���ւ͕Ԃ�Ȃ�)
    Public Const TRIM_RESULT_SKIP As Short = 4                          ' SKIP
    Public Const TRIM_RESULT_RATIO As Short = 5                         ' RATIO 
    Public Const TRIM_RESULT_IT_HING As Short = 6                       ' ITHI NG
    Public Const TRIM_RESULT_IT_LONG As Short = 7                       ' ITLO NG
    Public Const TRIM_RESULT_FT_HING As Short = 8                       ' FTHI NG 
    Public Const TRIM_RESULT_FT_LONG As Short = 9                       ' FTLO NG
    Public Const TRIM_RESULT_OVERRANGE As Short = 10                    ' �����W�I�[�o
    '                                                                   ' 4�[�q�����
    Public Const TRIM_RESULT_IT_OK As Short = 12                        ' IT�e�X�gOK
    Public Const TRIM_RESULT_PATTERNNG As Short = 13                    ' �J�b�g�ʒu�␳�p�^�[���F��NG�̂���SKIP
    Public Const TRIM_RESULT_TRIM_OK As Short = 14                      ' TRIM OK
    Public Const TRIM_RESULT_IKEI_SKIP As Short = 15                    ' �ٌ`�ʕt���ɂ��SKIP 
    '----- V1.13.0.0�J�� -----
    Public Const TRIM_RESULT_CVERR As Short = 16                        ' ����΂�����o
    Public Const TRIM_RESULT_OVERLOAD As Short = 17                     ' �I�[�o���[�h���o
    Public Const TRIM_RESULT_REPROBING As Short = 18                    ' �ăv���[�r���O�΂�����o(�o�M�a)
    '----- V1.13.0.0�J�� -----
    Public Const TRIM_RESULT_ES2ERR As Short = 19                       ' ES2 �G���[ V1.14.0.1�@
    Public Const TRIM_RESULT_OPENCHK_NG As Short = 20                   ' �I�[�v���`�F�b�N�G���[
    Public Const TRIM_RESULT_SHORTCHK_NG As Short = 21                  ' �V���[�g�`�F�b�N�G���[

    'V1.20.0.1�@
    Public Const TRIM_RESULT_MIDIUM_CUT_NG As Short = 22                ' �r���؂茟�o�G���[ 
    'V1.20.0.1�@
    '----- NG Judge -----
    Public Const CONTINUES_NG_HI As Integer = 1                         ' �A��NG-HIGH�װ����
    Public Const CONTINUES_NG_LO As Integer = 2

    '----- �摜�\���v���O�����̋N���p -----
    'V6.0.0.0�D    Public Const DISPGAZOU_SMALL_PATH As String = "C:\TRIM\DispGazouSmall.exe"     ' �摜�\���v���O������
    'Public Const DISPGAZOU_PATH As String = "C:\TRIM\DispGazou.exe"     ' �摜�\���v���O������ V4.3.0.0�B
    'V6.0.0.0�D    Public Const DISPGAZOU_PATH As String = "C:\TRIM\DispGazouSmall.exe" ' V4.3.0.0�B
    'V6.0.0.0�D    Public Const DISPGAZOU_WRK As String = "C:\\TRIM"                   ' ��ƃt�H���_

    'Private ObjGazou As Process = Nothing                              ' Process�I�u�W�F�N�g ###156
    'V6.0.0.0�D    Public ObjGazou As Process = Nothing                                ' Process�I�u�W�F�N�g ###156

    '----- FL�����f�t�H���g�ݒ�t�@�C�� ----
    Public Const DEF_FLPRM_SETFILEPATH As String = "c:\TRIM\"           'FL�����f�t�H���g�ݒ�t�@�C��
    Public Const DEF_FLPRM_SETFILENAME As String = "c:\TRIM\defaultFlParamSet.xml"    'FL�����f�t�H���g�ݒ�t�@�C��

    Private Const NEXT_BLOCK As Integer = -1                            ' ADJ�����Ŏg�p�B���̃X�e�b�v�ֈړ�����ꍇ�B
    Private Const PREV_BLOCK As Integer = -2                            ' ADJ�����Ŏg�p�B��O�̃X�e�b�v�ֈړ�����ꍇ�B

    'Private gstrResult(20) As String                                   ' ���ݸތ��� ###248
    Private gstrResult(MAX_RESULT_NUM) As String                        ' ���ݸތ��� ###248

    'Public glTchBPTeachMoval(2) As Double                              ' �X���C�_�[�o�[�̒l�ۑ��̈�

    Public globalLogFileName As String = "tmpLogFile.Log"                  'V1.13.0.0�K
    '----- �萔��` -----
    Public giMarkingMode As Short                                       ' 1:NG�}�[�L���O����

    '----- �v���[�u�`�F�b�N�@�\�p -----
    Private m_PlateCounter As Integer = 0                               ' ������J�E���^ V1.23.0.0�F
    Private m_ChkCounter As Integer = 0                                 ' ��`�F�b�N�J�E���^ V1.23.0.0�F
    '----- V4.11.0.0�B�� (WALSIN�aSL436S�Ή�) -----
    ' �I�[�g�p���[���s�@�\�p
    Private m_PwrChkCounter As Integer = 0                              ' ��`�F�b�N�J�E���^
    Private m_TimeAss As TimeSpan = New TimeSpan(0, 0, 0)               ' �w�莞��(��)
    Private m_TimeSpan As TimeSpan = New TimeSpan(0, 0, 0)              ' �o�ߎ���(��)
    Private m_TimeSTart As DateTime                                     ' �J�n����
    Private m_TimeNow As DateTime                                       ' ���ݎ���

    '----- AutoLaserPowerADJ()�̏������[�h
    Private Const MD_INI As Integer = 0                                 ' �������[�h(stPWR(���H�����ԍ��z��)������������)
    Private Const MD_ADJ As Integer = 1                                 ' �������[�h(stPWR(���H�����ԍ��z��)�����������Ȃ�)
    '----- V4.11.0.0�B�� -----

    '----- ��f�B���C(�f�B���C�g�����Q)�p -----
    Public intGetCutCnt As Integer = 1                                  ' �J�b�g��(�擪�u���b�N�`�ŏI�u���b�N�܂Ńu���b�N�ړ����邽�߂̃��[�v��)
    Public m_blnDelayCheck As Boolean = False                           ' �f�B���C�g�����Q���s��(True), �s��Ȃ�(False)
    Private m_blnDelayFirstCut As Boolean                               ' ��1�J�b�g�t���O
    Private m_blnDelayLastCut As Boolean                                ' �ŏI�J�b�g�t���O
    Private m_intDelayBlockIndex As Short                               ' ���݂̃u���b�N�ԍ�(�J�b�g�ԍ�)

    Private m_intNgCount As Short                                       ' NG�� 
    Private m_lngRetcTrim As Integer

    'Private pfPP36x As Double                                          ' �s�N�Z���������BP����
    'Private pfPP36y As Double
    'Private piPP37_3 As Short
    'Private pfPosition(1, 512) As Double                               ' �J�b�g�␳�|�W�V����XY
    'Private piCutCorPtn(512) As Short                                  ' �J�b�g�␳�̃p�^�[���ԍ�
    'Private pbCutCorDisp(512) As Boolean                               ' �J�b�g�␳�@�p�^�[���}�b�`�\��

    ' ���M���O�֌W
    Private strLogFileNameDate As String                                ' ۸�̧�ق�̧�ٖ����t����
    'Private Const cTRIMLOGcSECTNAME As String = "TRIMMODE_FILELOG_DATA_SET"
    'Private Const cMEASLOGcSECTNAME As String = "MEASMODE_FILELOG_DATA_SET"
    'Private m_TrimlogFileFormat(20) As Integer                         ' �g���~���O��-���O�t�@�C���ւ̏o�͑Ώہ����Ԑݒ�ϐ�
    'Private m_MeaslogFileFormat(20) As Integer                         ' ���莞-���O�t�@�C���ւ̏o�͑Ώہ����Ԑݒ�ϐ�

    ''----- ���O�Ώے萔 -----
    'Private Const LOGTAR_DATE As Short = 1
    'Private Const LOGTAR_LOTNO As Short = 2
    'Private Const LOGTAR_CIRCUIT As Short = 3
    'Private Const LOGTAR_RESISTOR As Short = 4
    'Private Const LOGTAR_JUDGE As Short = 5
    'Private Const LOGTAR_TARGET As Short = 6
    'Private Const LOGTAR_INITIAL As Short = 7
    'Private Const LOGTAR_FINAL As Short = 8
    'Private Const LOGTAR_DEVIATION As Short = 9
    'Private Const LOGTAR_UCUTPRMNO As Short = 10
    'Private Const LOGTAR_END As Short = 99

    '----- ���Y�Ǘ���� -----
    Public m_lCircuitNgTotal As Integer                                 ' �s�ǃT�[�L�b�g��
    Public m_lCircuitGoodTotal As Integer                               ' �Ǖi�T�[�L�b�g��
    Public m_lPlateCount As Integer                                     ' �v���[�g������
    Public m_lGoodCount As Integer                                      ' �Ǖi��R��
    Public m_lNgCount As Integer                                        ' �s�ǒ�R��
    Public m_lITHINGCount As Integer                                    ' IT HI NG��
    Public m_lITLONGCount As Integer                                    ' IT LO NG��
    Public m_lFTHINGCount As Integer                                    ' FT HI NG��
    Public m_lFTLONGCount As Integer                                    ' FT LO NG��
    Public m_lITOVERCount As Integer                                    ' IT���ް�ݼސ�

    ' NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��) ###142 
    Public m_NG_RES_Count As Integer                                    ' NG��R��

    Public m_NgCountInPlate As Integer                                    ' NG��R���J�E���g�p 'V4.5.0.5�@ 'V6.0.5.0�C
    Public TotalAverageFT As Double                                       ' ###154
    Public TotalDeviationFT As Double                                     ' ###154
    Public TotalAverageIT As Double                                       ' ###154
    Public TotalDeviationIT As Double                                     ' ###154
    Public TotalAverageDebug As Double                                    ' ###154
    Public TotalDeviationDebug As Double                                  ' ###154
    Public TotalSum2FT As Double                                          ' ###154
    Public TotalSum2IT As Double                                          ' ###154

    '----- V6.0.3.0_26�� -----
    Public TotalAverageFTValue As Double
    Public TotalFTValue As Double
    Public TotalCntTrimming As Long
    '----- V6.0.3.0_26�� -----

    'UPGRADE_WARNING: �z�� gwCircuitNgCount �̉����� 1 ���� 0 �ɕύX����܂����B
    Private gwCircuitNgCount(48) As Short                               '  
    Private gfCircuitX(48) As Double                                    ' �T�[�L�b�g���W X 
    Private gfCircuitY(48) As Double                                    ' �T�[�L�b�g���W Y 

    '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) -----
    ' �ꎞ��~���ԏW�v�p�\����
    Public Structure PauseTime_Data_Info
        Dim StartTime As DateTime                                       ' �ꎞ��~�J�n����(YYYY/MM/DD MM:DD:SS)
        Dim EndTime As DateTime                                         ' �ꎞ��~�I������
        Dim PauseTime As TimeSpan                                       ' �ꎞ��~����
        Dim TotalTime As TimeSpan                                       ' �ꎞ��~�g�[�^������
    End Structure
    Public StPauseTime As PauseTime_Data_Info
    Public m_blnElapsedTime As Boolean = False                          ' �o�ߎ��Ԃ�\������(True), ���Ȃ�(False)
    '----- V4.11.0.0�C�� -----

    '----- V1.22.0.0�C�� -----
    '----- �T�}���[���M���O�p(�V�i�W�[�a�Ή�) -----
    ' �T�}���[���M���O�̒�R�ڍ׍��ڌ`����`
    Public Structure SummaryLog_SubData
        Dim lngItLow As Long                                            ' IT LO NG��R��
        Dim lngItHigh As Long                                           ' IT HI NG��R��
        Dim lngItTotal As Long                                          ' IT NG��R�� =  
        Dim lngFtLow As Long                                            ' FT LO NG��R��
        Dim lngFtHight As Long                                          ' FT HI NG��R��
        Dim lngFtTotal As Long                                          ' IT NG��R�� 
        Dim lngOpen As Long                                             ' �I�[�v���`�F�b�N�G���[��R��
    End Structure

    ' �T�}���[���M���O���`����`
    Public Structure SummaryLog
        Dim strStartTime As String                                      ' �T�}���[�J�n����
        Dim strEndTime As String                                        ' �T�}���[�I������
        Dim stTotalSubData As SummaryLog_SubData                        ' �T�}���[���M���O�̏ڍ׍���(�g�[�^��)
        <VBFixedArray(1000)> Dim RegAry() As SummaryLog_SubData         ' �T�}���[���M���O�̏ڍ׍���(��R��1-999)

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim RegAry(1000)
        End Sub
    End Structure
    Public stSummaryLog As SummaryLog                                   ' �T�}���[���M���O�o�̓f�[�^
    '----- V1.22.0.0�C�� -----

    '----- V6.1.1.0�B�� (SL436R�p�I�v�V����)-----
    ' �����^�]�J�n/�I�����ԕ\���p
    Public cStartTime As DateTime
    Public cEndTime As DateTime
    '----- V6.1.1.0�B�� -----

    '----- ���̑� -----
    Private bIniFlg As Integer = 0                                      ' �����t���O(0=����, 1= �g���~���O��, 2=�I��) ###156
    Private bIniPwrFlg As Boolean = False                               ' V4.11.0.0�B
    Public IDReadName As String                                         ' ID�ǂݍ��݌���        'V1.13.0.0�K
    Public LogFileSaveName As String                                    ' Log�t�@�C����         'V1.13.0.0�K

    Public Const WM_COPYDATA As Int32 = &H4A
    Public Const WM_USER As Int32 = &H400
    Public Const WM_APP As Int32 = &H8000

    'COPYDATASTRUCT�\���� 
    Public Structure COPYDATASTRUCT
        Public dwData As Int32   '���M����32�r�b�g�l
        Public cbData As Int32        'lpData�̃o�C�g��
        Public lpData As String     '���M����f�[�^�ւ̃|�C���^(0���\)
    End Structure

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function FindWindow( _
         ByVal lpClassName As String, _
         ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function SendMessage( _
                           ByVal hWnd As IntPtr, _
                           ByVal wMsg As Int32, _
                           ByVal wParam As Int32, _
                           ByVal lParam As Int32) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function SendNotifyMessage( _
                           ByVal hWnd As IntPtr, _
                           ByVal wMsg As Int32, _
                           ByVal wParam As Int32, _
                           ByVal lParam As Int32) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function SendMessage(
                            ByVal hWnd As IntPtr,
                            ByVal wMsg As Int32,
                            ByVal wParam As Int32,
                            ByRef lParam As COPYDATASTRUCT) As Integer
    End Function
    '#4.12.2.0�C            ��
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="SendMessage")>
    Public Function SendMessageString(ByVal hWnd As IntPtr,
                                      ByVal wMsg As UInt32,
                                      ByVal wParam As Int32,
                                      <[In], MarshalAs(UnmanagedType.LPWStr)>
                                      lParam As String) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="SendMessage")>
    Public Function SendMessageStringBuilder(ByVal hWnd As IntPtr,
                                             ByVal wMsg As UInt32,
                                             ByVal wParam As Int32,
                                             <[In], MarshalAs(UnmanagedType.LPWStr)>
                                             ByVal lParam As StringBuilder) As Integer
    End Function
    '#4.12.2.0�C            ��
#End Region

#Region "�g���~���O���ʕۑ��̈揉����"
    Public Sub ClearTrimResult()
        m_lPlateCount = 0                                               ' �v���[�g������
        m_lGoodCount = 0                                                ' �Ǖi��R��
        m_lNgCount = 0                                                  ' �s�ǒ�R��
        m_lCircuitNgTotal = 0                                           ' �s�ǃT�[�L�b�g��
        m_lCircuitGoodTotal = 0                                         ' �Ǖi�T�[�L�b�g��

        m_lITHINGCount = 0
        m_lITLONGCount = 0
        m_lFTHINGCount = 0
        m_lFTLONGCount = 0
        m_lITOVERCount = 0
    End Sub
#End Region

#Region "�������[�U�p���[��������"
    '''=========================================================================
    '''<summary>�������[�U�p���[��������</summary>
    '''<param name="Mode">(INP)�������[�h V4.11.0.0�B
    '''                        MD_INI = �������[�h(stPWR(���H�����ԍ��z��)������������)
    '''                        MD_ADJ = �������[�h(stPWR(���H�����ԍ��z��)�����������Ȃ�)
    '''                      </param>
    '''<remarks>�������[�U�p���[�̒����������s�B</remarks>
    '''<returns>cFRS_NORMAL  = ����
    '''         cFRS_ERR_RST = Cancel(RESET��)
    '''         ��L�ȊO �@�@= ����~���o���̃G���[</returns> 
    '''=========================================================================
    Public Function AutoLaserPowerADJ(ByVal Mode As Integer) As Short
        'Public Function AutoLaserPowerADJ() As Short

        Dim strMsg As String
        Dim r As Integer
        Dim iCurr As Long
        Dim iCurrOfs As Long
        Dim dMeasPower As Double
        Dim dFullPower As Double
        Dim AdjustTarget As Double                                      ' ###066
        Dim AdjustLevel As Double                                       ' ###066
        Dim CndNum As Integer                                           ' ###066
        Dim bIniFlg As Boolean = True                                   ' V4.0.0.0-87

        Try
            With typPlateInfo
                stPRT_ROHM.LaserPower = ""                              ' V1.18.0.0�B
                strMsg = "" 'V4.0.0.0-72
                ' �p���[���[�^�̃f�[�^�擾�^�C�v���u�h�^�n�ǎ��v/�u�t�r�a�v�łȂ����NOP(���̂܂ܔ�����)
                If (gSysPrm.stIOC.giPM_DataTp = PM_DTTYPE_NONE) Then
                    Return (cFRS_NORMAL)
                End If

                '----- V1.18.0.4�@�� -----
                ' Power Ctrl ON/OFF�{�^����\������(�I�v�V����)�Ȃ�uPower Ctrl ON/OFF�v�{�^����D�悷��(���[���a����)
                If (giBtnPwrCtrl = 1) Then                              '�uPower Ctrl ON/OFF�v�{�^���L�� ?
                    If (Form1.BtnPowerOnOff.Text = "Power Ctrl ON") Then
                        GoTo STP_EXEC                                   ' �uPower Ctrl ON�v�Ȃ�p���[���������s����
                    Else
                        GoTo STP_NO_EXEC                                ' �uPower Ctrl OFF�v�Ȃ� �p���[���������s���Ȃ�
                    End If
                    'Else                                               'V1.18.0.2�@
                    '    GoTo STP_NO_EXEC                               'V1.18.0.2�@
                End If
                '----- V1.18.0.4�@�� -----

                ' �p���[���������s���Ȃ��ꍇ�͂��̂܂ܔ�����
                If (.intPowerAdjustMode <> 1) Then                      ' �p���[�������s�t���O = ���s���Ȃ� ?
                    '----- V1.18.0.0�B�� -----
STP_NO_EXEC:        ' V1.18.0.4�@
                    ' ����p���H�ʏo�̓p���[�ݒ�(���[���a����)
                    If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                        If (stPRT_ROHM.LaserPower = "") Then
                            stPRT_ROHM.LaserPower = dMeasPower.ToString("-.--") + "W"
                        End If
                    End If
                    '----- V1.18.0.0�B�� -----
                    ' �p���[���������s���Ȃ��ꍇ�͂��̂܂ܔ�����
                    Return (cFRS_NORMAL)
                End If

STP_EXEC:       ' V1.18.0.4�@
                ' Z�����_�ֈړ�
                r = EX_ZMOVE(0, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                    Return (r)                                          ' Return�l�ݒ� 
                End If

                '---------------------------------------------------------------
                '   �����p���[�������s
                '---------------------------------------------------------------
                '----- V1.18.0.1�F�� -----
                ' ���[�U�Ǝ˒��̃V�O�i���^���[(���F)�_��(���[���a����)
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ���[���a���� ? 
                    Call EXTOUT1(EXTOUT_EX_YLW_ON, 0)
                End If
                '----- V1.18.0.1�F�� -----

                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                    '-----------------------------------------------------------
                    '   FL�� ###066
                    '-----------------------------------------------------------
                    'r = Form1.System1.Form_FLAutoLaser(gSysPrm, .intPowerAdjustCondNo, .dblPowerAdjustTarget, .dblPowerAdjustToleLevel, _
                    '                                   iCurr, iCurrOfs, dMeasPower, dFullPower)
                    '----- V4.11.0.0�B�� (WALSIN�aSL436S�Ή�) -----
                    ' �p���[����������H�����ԍ��z��ɗL��/������ݒ肷��
                    'r = SetAutoPowerCndNumAry(stPWR)
                    If (Mode = MD_INI) Then                                 ' �������[�h���ɐݒ肷�� 
                        r = SetAutoPowerCndNumAry(stPWR)
                    End If
                    '----- V4.11.0.0�B�� -----

                    ' �J�b�g�Ɏg�p������H�����ԍ��̃p���[�������s�� 
                    For CndNum = 0 To (MAX_BANK_NUM - 1)
                        If (stPWR.CndNumAry(CndNum) = 1) Then               ' ���H�����͗L�� ?
                            AdjustTarget = stPWR.AdjustTargetAry(CndNum)    ' �ڕW�p���[�l(W)
                            AdjustLevel = stPWR.AdjustLevelAry(CndNum)      ' �������e�͈�(�}W)

                            ' ���b�Z�[�W�\��("�p���[�����J�n"+ " ���H�����ԍ�xx")
                            strMsg = MSG_AUTOPOWER_01 + " " + MSG_AUTOPOWER_02 + CndNum.ToString("00")
                            Call Form1.Z_PRINT(strMsg)

                            ' �p���[�������s��
                            r = Form1.System1.Form_FLAutoLaser(gSysPrm, CndNum, AdjustTarget, AdjustLevel, iCurr, iCurrOfs, dMeasPower, dFullPower)
                            '----- ###177�� -----
                            If (r < cFRS_NORMAL) Then
                                ' �G���[���b�Z�[�W�\��
                                r = Form1.System1.Form_AxisErrMsgDisp(System.Math.Abs(r))
                                Return (r)
                            End If
                            '----- ###177�� -----

                            ' �������ʂ����C����ʂɕ\������
                            If (r = cFRS_NORMAL) Then                   ' ����I�� ? 
                                ' ���b�Z�[�W�\��("���[�U�p���[�ݒ�l"+" = xx.xxW, " + "�d���l=" + "xxxmA")
                                strMsg = MSG_AUTOPOWER_03 + "= " + dMeasPower.ToString("0.00") + "W, "
                                strMsg = strMsg + MSG_AUTOPOWER_04 + "= " + iCurr.ToString("0") + "mA"
                                Call Form1.Z_PRINT(strMsg)

                                '----- V4.0.0.0-58�� -----
                                ' �I�[�g�p���[������̓d���l���u���H�����\���́v�ɔ��f����(SL436S��)
                                If (giMachineKd = MACHINE_KD_RS) Then
                                    stCND.Curr(CndNum) = iCurr          ' �g���}�[���H�����\���̂̓d���l�X�V
                                End If
                                '----- V4.0.0.0-58�� -----

                                '----- V1.18.0.0�B�� -----
                                ' ����p���H�ʏo�̓p���[�ݒ�(���[���a����)
                                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                                    If (stPRT_ROHM.LaserPower = "") Or (stPRT_ROHM.LaserPower = "        ---") Then
                                        stPRT_ROHM.LaserPower = dMeasPower.ToString("0.00") + "W"
                                    End If
                                End If
                                '----- V1.18.0.0�B�� -----
                                '----- V4.0.0.0-87�� -----
                                If (giMachineKd = MACHINE_KD_RS) And (bIniFlg = True) Then
                                    TrimData.SetLaserPower(dMeasPower)
                                End If
                                bIniFlg = False
                                '----- V4.0.0.0-87�� -----
                            Else
                                ' ���b�Z�[�W�\��("�p���[����������")
                                strMsg = MSG_AUTOPOWER_05
                                Call Form1.Z_PRINT(strMsg)
                                '----- V4.0.0.0-87�� -----
                                If (giMachineKd = MACHINE_KD_RS) Then
                                    TrimData.SetLaserPower(0.0)
                                End If
                                '----- V4.0.0.0-87�� -----
                                Exit For                                ' �����I��
                            End If
                        End If
                    Next CndNum

                    '----- V4.0.0.0-58�� -----
                    If strMsg <> MSG_AUTOPOWER_05 Then  'V4.0.0.0-72
                        ' �I�[�g�p���[������̓d���l���g���~���O�f�[�^�ɔ��f����(SL436S��)
                        If (giMachineKd = MACHINE_KD_RS) Then
                            r = SetAutoPowerCurrData(stCND)
                            gCmpTrimDataFlg = 1                            ' �f�[�^�X�V�t���O = 1(�X�V����)
                        End If
                        '----- V4.0.0.0-58�� -----
                    End If                              'V4.0.0.0-72

                Else
                    '-----------------------------------------------------------
                    '   FL�ȊO�̏ꍇ
                    '-----------------------------------------------------------
                    If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then                                  'V5.0.0.6�G RMCTRL2�Ή� ?
                        Call ATTRESET()                                                     'V1.25.0.0�M
                    End If                                                                  'V5.0.0.6�G
                    r = Form1.System1.Form_AutoLaser(gSysPrm, .dblPowerAdjustQRate,
                                        .dblPowerAdjustTarget, .dblPowerAdjustToleLevel)
                    '----- V1.16.0.0�G�� -----
                    ' �������ʂ����C����ʂɕ\������
                    If (r = cFRS_NORMAL) Then                           ' ����I�� ? 
                        ' ���b�Z�[�W�\��("�p���[��������I��")
                        strMsg = MSG_AUTOPOWER_06
                        Call Form1.Z_PRINT(strMsg)
                        'V4.4.0.0�D
                        '---------------------------------------------------------------------------
                        '   ���[�U�[�����㏈��
                        '---------------------------------------------------------------------------
                        ' RMCTRL2 >=2 �̏ꍇ�A ���������V�X�p�����ĕ\��("������ = 99.9%")  '###026
                        Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)          '###029
                        If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then
                            strMsg = LBL_ATT + "  " + CDbl(gSysPrm.stRAT.gfAttRate).ToString("##0.0") + " %"
                            Form1.LblRotAtt.Text = strMsg
                        End If
                        'V4.4.0.0�D
                    Else
                        ' ���b�Z�[�W�\��("�p���[����������")
                        strMsg = MSG_AUTOPOWER_05
                        Call Form1.Z_PRINT(strMsg)
                    End If
                    '----- V1.16.0.0�G�� -----
                End If

                System.Windows.Forms.Application.DoEvents()
            End With

            '----- V1.18.0.1�F�� -----
            ' ���[�U�Ǝ˒��̃V�O�i���^���[(���F)����(���[���a����)
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ���[���a���� ? 
                Call EXTOUT1(0, EXTOUT_EX_YLW_ON)
            End If
            '----- V1.18.0.1�F�� -----

            Return (r)                                                  ' Return�l�ݒ� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "basTrimming.AutoLaserPowerADJ() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try
    End Function
#End Region
    '----- V4.11.0.0�B�� (WALSIN�aSL436S�Ή�) -----
#Region "�I�[�g�p���[�������s(������w��/����(��)�w��)FL�p(�����^�]���L��)"
    '''=========================================================================
    ''' <summary>�I�[�g�p���[�������s(������w��/����(��)�w��)</summary>
    ''' <param name="digL">       (INP)Dig-SW Low</param>
    ''' <param name="ChkCounter"> (I/O)�`�F�b�N�J�E���^</param>
    ''' <param name="StartTime">  (I/O)�J�n����</param>
    ''' <param name="NowTimeSpan">(I/O)�o�ߎ���</param>
    ''' <param name="AssTimeSpan">(INP)�w�莞��(��)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>��P�ʂŎ��s����B�Ⴆ�΃u���b�N�̓r���Ŏw�莞�Ԃ����Ă����̊��
    '''          ���I�������Ɏ��s����</remarks>
    '''=========================================================================
    Public Function AutoPowerExeByOption(ByVal digL As Integer, ByRef ChkCounter As Integer, ByRef StartTime As DateTime, ByRef NowTimeSpan As TimeSpan, ByRef AssTimeSpan As TimeSpan) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim iCurr As Long
        Dim CndNum As Integer
        Dim dblMeasure As Double
        Dim AdjustTarget As Double
        Dim AdjustLevel As Double
        Dim dblHi As Double
        Dim dblLo As Double
        Dim bPowerADJ As Boolean = False
        Dim strMSG As String
        Dim ts As TimeSpan
        Dim Flg As Integer = 0

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �����^�]�łȂ����NOP
            If (bFgAutoMode = False) Then Return (cFRS_NORMAL)

            ' FL�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)

            ' �p���[�������s�t���O = ���s���Ȃ��Ȃ�NOP
            If (typPlateInfo.intPowerAdjustMode <> 1) Then Return (cFRS_NORMAL)

            ' �p���[���[�^�̃f�[�^�擾�^�C�v���u�h�^�n�ǎ��v/�u�t�r�a�v�łȂ����NOP
            If (gSysPrm.stIOC.giPM_DataTp = PM_DTTYPE_NONE) Then Return (cFRS_NORMAL)

            ' �I�[�g�p���[�u������w��v�@�\�Ȃ����́u����(��)�w��v�@�\�Ȃ��Ȃ�NOP
            If (giPwrChkPltNum = 0) And (giPwrChkTime = 0) Then Return (cFRS_NORMAL)

            '-------------------------------------------------------------------
            '   ������w�莞
            '-------------------------------------------------------------------
            If (giPwrChkPltNum = 1) Then                                '�u������w��v�@�\���� ?
                If (typPlateInfo.intPwrChkPltNum = 0) Then              ' ������w��Ȃ� ?
                    If (giPwrChkTime = 0) Then                          '�u����(��)�w��v�@�\�Ȃ� ?
                        Return (cFRS_NORMAL)
                    End If
                Else
                    ' �g���~���O���[�h��x0,x1,x5�ȊO�Ȃ�
                    ' �`�F�b�N�J�E���^�ɍő�l�����Ĕ�����(x0,x1,x5���[�h�ɕς�������Ɏ��s���邽��)
                    If (digL <> 0) And (digL <> 1) And (digL <> 5) Then
                        ChkCounter = typPlateInfo.intPwrChkPltNum + 1
                        Return (cFRS_NORMAL)
                    End If
                    ' �`�F�b�N�J�E���^ > �`�F�b�N���� ?
                    '                    If (ChkCounter <= typPlateInfo.intPwrChkPltNum) Then
                    If (ChkCounter < typPlateInfo.intPwrChkPltNum) Then
                        Return (cFRS_NORMAL)
                    End If
                    Flg = &H1                                           ' Flg = ������w�� 
                End If
            End If

            '-------------------------------------------------------------------
            '   ����(��)�w�莞
            '-------------------------------------------------------------------
            '�u����(��)�w��v�@�\�Ȃ�/�u����(��)�w��Ȃ��v�Ȃ�NOP
            If (giPwrChkTime = 0) And ((Flg And &H1) = 0) Then Return (cFRS_NORMAL)
            If (typPlateInfo.intPwrChkTime = 0) And ((Flg And &H1) = 0) Then Return (cFRS_NORMAL)

            ' �g���~���O���[�h��x0,x1,x5�ȊO�Ȃ�
            ' �o�ߎ���(��)�ɍő�l�����Ĕ�����(x0,x1,x5���[�h�ɕς�������Ɏ��s���邽��)
            If (digL <> 0) And (digL <> 1) And (digL <> 5) Then
                ts = New TimeSpan(0, typPlateInfo.intPwrChkTime + 1, 0)
                NowTimeSpan = ts
                Return (cFRS_NORMAL)
            End If

            ' �o�ߎ���(��) > �w�莞��(��) ?
            If (NowTimeSpan <= AssTimeSpan) Then
                If ((Flg And &H1) = 0) Then
                    Return (cFRS_NORMAL)
                End If
            Else
                Flg = Flg Or &H2                                        ' Flg = ����(��)�w�� 
            End If

            '-------------------------------------------------------------------
            '   ���݂̃��[�U�p���[�𑪒肷��
            '-------------------------------------------------------------------
STP_CHK:
            ' Z�����_�ֈړ�
            r = EX_ZMOVE(0, MOVE_ABSOLUTE)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                Return (r)                                              ' Return�l�ݒ� 
            End If

            ' �p���[����������H�����ԍ��z��ɗL��/������ݒ肷��
            r = SetAutoPowerCndNumAry(stPWR)

            ' �J�b�g�Ɏg�p������H�����ԍ��̃p���[�������s�� 
            For CndNum = 0 To (MAX_BANK_NUM - 1)                        ' �ő���H���������J��Ԃ�
                If (stPWR.CndNumAry(CndNum) = 1) Then                   ' ���H�����͗L�� ?

                    ' ���e�͈�[W](Hight/Low)��ݒ肷��
                    AdjustTarget = stPWR.AdjustTargetAry(CndNum)        ' �ڕW�p���[�l(W)
                    AdjustLevel = stPWR.AdjustLevelAry(CndNum)          ' �������e�͈�(�}W)
                    dblHi = AdjustTarget + AdjustLevel                  ' ���e�͈�[W](Hight)
                    dblLo = AdjustTarget - AdjustLevel                  ' ���e�͈�[W](Low)

                    ' �p���[������s��
                    r = Form1.System1.Form_GetLaserPowerFL(gSysPrm, CndNum, iCurr, dblMeasure)
                    If (r < cFRS_NORMAL) Then                           ' �G���[ ? 
                        rtn = r                                         ' ����~���̃G���[�Ȃ烊�^�[��(�G���[���b�Z�[�W�͕\����)
                        GoTo STP_END
                    End If
                    If (r = cFRS_ERR_RST) Then                          ' Cancel(RESET��) ?
                        rtn = r                                         ' �Ȃ烊�^�[��
                        GoTo STP_END
                    End If

                    ' ���茋�ʂ����C����ʂɕ\������
                    ' ���b�Z�[�W�\��("���[�U�p���[����l = "+" ���H�����ԍ�xx"+" = xx.xxW, " + "�d���l=" + "xxxmA")
                    strMSG = MSG_AUTOPOWER_03 + "= " + MSG_AUTOPOWER_02 + CndNum.ToString("00")
                    strMSG = strMSG + ", " + dblMeasure.ToString("0.00") + "W, "
                    strMSG = strMSG + MSG_AUTOPOWER_04 + "= " + iCurr.ToString("0") + "mA"
                    Call Form1.Z_PRINT(strMSG)

                    ' ����l�͋��e�͈͓� ?
                    If (dblMeasure <= dblHi) And (dblMeasure >= dblLo) Then
                        stPWR.CndNumAry(CndNum) = 0                     ' ���e�͈͓��Ȃ�p���[�������Ȃ��悤�Ɂu���H�����𖳌��v�Ƃ���
                        Continue For
                    Else
                        bPowerADJ = True                                ' ���e�͈͊O�Ȃ�u�p���[�������s�t���O�v��ON�ɂ���
                    End If
                End If
            Next CndNum

            '-------------------------------------------------------------------
            '   ���[�U�p���[�������s��(����p���[���͈͊O�̂��̂̂�)
            '-------------------------------------------------------------------
            If (bPowerADJ = True) Then                                      '�u�p���[�������s�t���O�vON ?
                r = AutoLaserPowerADJ(MD_ADJ)                               ' ���[�U�p���[�������s(�������[�h)
                If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESET��) ?
                    rtn = cFRS_ERR_RST                                      ' Return�l = Cancel(RESET��)
                ElseIf (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                    Return (r)                                              ' ����~���̃G���[�Ȃ�A�v�������I����
                End If
            End If

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
STP_END:
            ' ������w��Ȃ�`�F�b�N�J�E���^������������������
            If ((Flg And &H1) = &H1) Then
                ChkCounter = 0
            End If

            ' ����(��)�w��Ȃ�J�n����/�o�ߎ��Ԃ�����������
            If ((Flg And &H2) = &H2) Then
                StartTime = DateTime.Now
                NowTimeSpan = New TimeSpan(0, 0, 0)
            End If

            ' �u���b�N�T�C�Y���Đݒ肷��(Form_GetLaserPowerFL()�ŕύX������)
            r = Form1.System1.EX_BSIZE(gSysPrm, typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)                                              ' �A�v�������I����
            End If
            ' BP�I�t�Z�b�g�ݒ�(���u���b�N�T�C�Y��ݒ肷���BP�I�t�Z�b�g��INtime���ŏ�����������)
            r = Form1.System1.EX_BPOFF(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)                                              ' �A�v�������I����
            End If

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.AutoPowerExeCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V4.11.0.0�B�� -----
    '----- V4.0.0.0-58�� -----
#Region "�I�[�g�p���[������̓d���l���g���~���O�f�[�^�ɔ��f����(SL436S��)"
    '''=========================================================================
    ''' <summary>�I�[�g�p���[������̓d���l���g���~���O�f�[�^�ɔ��f����</summary>
    ''' <param name="stCND">(INP)FL�p�g���}�[���H������� ���z���0�I���W��</param>
    ''' <remarks></remarks>
    ''' <returns>cFRS_NORMAL  = ����
    '''          ��L�ȊO �@�@= �G���[</returns> 
    '''=========================================================================
    Private Function SetAutoPowerCurrData(ByRef stCND As TrimCondInfo) As Integer

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   ��������
            '------------------------------------------------------------------
            ' FL�łȂ��܂���SL436S�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            '------------------------------------------------------------------
            '   �g���~���O�f�[�^�̓d���l���X�V����
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' �P�u���b�N����R�����`�F�b�N���� 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' ��R���J�b�g�����`�F�b�N����
                    ' �J�b�g�^�C�v�擾
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

                    ' ���H����1�͑S�J�b�g�������ɃI�[�g�p���[������̓d���l��ݒ肷��
                    CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1)
                    typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1) = stCND.Curr(CndNum)

                    ' ���H����2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
                    ' HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = DataManager.CNS_CUTP_L) Or (strCutType = DataManager.CNS_CUTP_NL) Or
                       (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Or
                       (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then
                        ' ���H����2
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2) = stCND.Curr(CndNum)
                    End If

                    ' ���H����3��HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then
                        ' ���H����3
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L3) = stCND.Curr(CndNum)
                    End If

                    ' ���H����4�͌���͖��g�p(�\��)

                    ' ���H����5�`8�̓��^�[��/���g���[�X�p 
                    ' ���H����5(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)��
                    If (strCutType = DataManager.CNS_CUTP_STr) Or (strCutType = DataManager.CNS_CUTP_STt) Or
                       (strCutType = DataManager.CNS_CUTP_NSTr) Or (strCutType = DataManager.CNS_CUTP_NSTt) Then
                        ' ���H����5�̏����ԍ����J�b�g�f�[�^���ݒ肷��
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET) = stCND.Curr(CndNum)
                    End If

                    ' ���H����5,6(L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)��
                    If (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Then
                        ' ���H����5�̏����ԍ����J�b�g�f�[�^���ݒ肷��
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET) = stCND.Curr(CndNum)

                        ' ���H����6�̏����ԍ����J�b�g�f�[�^�̉��H����4���ݒ肷��
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2_RET) = stCND.Curr(CndNum)
                    End If

                    ' ���H����7,8�͌���͖��g�p(�\��)

                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return�l�ݒ� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "basTrimming.SetAutoPowerCurrData() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try
    End Function
#End Region
    '----- V4.0.0.0-58�� -----
#Region "�p���[����������H�����ԍ��z��ɗL��/������ݒ肷��"
    '''=========================================================================
    ''' <summary>�p���[����������H�����ԍ��z��ɗL��/������ݒ肷�� ###066</summary>
    ''' <param name="stPWR">(OUT)FL�p�p���[�������
    '''                              ���z���0�I���W��</param>
    ''' <remarks>�������[�U�p���[�̒����������s�p</remarks>
    ''' <returns>cFRS_NORMAL  = ����
    '''          ��L�ȊO �@�@= �G���[</returns> 
    '''=========================================================================
    Private Function SetAutoPowerCndNumAry(ByRef stPWR As POWER_ADJUST_INFO) As Short

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   ��������
            '------------------------------------------------------------------
            ' FL�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)

            ' �g���}�[���H�����\����(FL�p)������ 
            stPWR.Initialize()

            '------------------------------------------------------------------
            '   ���H�����ԍ��z���ݒ肷��
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' �P�u���b�N����R�����`�F�b�N���� 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' ��R���J�b�g�����`�F�b�N����
                    ' �J�b�g�^�C�v�擾
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()
#If False Then
                    ' ���H����1�͑S�J�b�g�������ɐݒ肷��
                    CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1)
                    If (stPWR.CndNumAry(CndNum) = 0) Then               ' ���� ? 
                        stPWR.CndNumAry(CndNum) = 1                     ' �L���ɐݒ�
                        ' �ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                        stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1)
                        stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L1)
                    End If

                    ' ���H����2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
                    ' HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = DataManager.CNS_CUTP_L) Or (strCutType = DataManager.CNS_CUTP_NL) Or _
                       (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or _
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Or _
                       (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then ' V1.22.0.0�@
                        ' ���H����2
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2)
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' ���� ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' �L���ɐݒ�
                            ' �ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2)
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L2)
                        End If
                    End If

                    ' ���H����3��HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then ' V1.22.0.0�@
                        ' ���H����3
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3)
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' ���� ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' �L���ɐݒ�
                            ' �ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L3)
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L3)
                        End If
                    End If

                    ' ���H����4�͌���͖��g�p(�\��)

                    ' ���H����5�`8�̓��^�[��/���g���[�X�p 
                    ' ���H����5(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)��
                    If (strCutType = DataManager.CNS_CUTP_STr) Or (strCutType = DataManager.CNS_CUTP_STt) Or _
                       (strCutType = DataManager.CNS_CUTP_NSTr) Or (strCutType = DataManager.CNS_CUTP_NSTt) Then
                        ' ���H����5�̏����ԍ����J�b�g�f�[�^�̉��H����2���ݒ肷��
                        'CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(1)                 'V4.0.0.0-58
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)     'V4.0.0.0-58
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' ���� ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' �L���ɐݒ�
                            ' �ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                            'stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(1)'V4.0.0.0-58
                            'stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(1)'V4.0.0.0-58
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET) 'V4.0.0.0-58
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L1_RET) 'V4.0.0.0-58
                        End If
                    End If

                    ' ���H����5,6(L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)��
                    If (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or _
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Then
                        ' ���H����5�̏����ԍ����J�b�g�f�[�^�̉��H����3���ݒ肷��
                        'CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(2)                 'V4.0.0.0-58
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)     'V4.0.0.0-58
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' ���� ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' �L���ɐݒ�
                            ' �ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                            'stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(2)                'V4.0.0.0-58
                            'stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(2)              'V4.0.0.0-58
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET)    'V4.0.0.0-58
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L1_RET)  'V4.0.0.0-58
                        End If
                        ' ���H����6�̏����ԍ����J�b�g�f�[�^�̉��H����4���ݒ肷��
                        'CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(3)             'V4.0.0.0-58
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET) 'V4.0.0.0-58
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' ���� ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' �L���ɐݒ�
                            ' �ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                            'stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(3)                'V4.0.0.0-58
                            'stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(3)              'V4.0.0.0-58
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2_RET)    'V4.0.0.0-58
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L2_RET)  'V4.0.0.0-58
                        End If
                    End If

                    ' ���H����7,8�͌���͖��g�p(�\��)
#Else
                    For cndIdx As Integer = 0 To (DataManager.MaxCndNum - 1) Step 1                 '#5.0.0.8�@
                        Dim arrIdx As Integer = DataManager.GetArrCutFLIndex(cndIdx, strCutType)    '#5.0.0.8�@
                        If (0 <= arrIdx) Then
                            With typResistorInfoArray(Rn).ArrCut(Cn)
                                CndNum = .CndNum(arrIdx)                ' ���H�����ԍ�
                                If (0 = stPWR.CndNumAry(CndNum)) Then   ' �ݒ肳��Ă��Ȃ� ?
                                    stPWR.CndNumAry(CndNum) = 1         ' �ݒ�ς݂Ƃ���
                                    ' �w����H�����ԍ��̖ڕW�p���[�l(W)�ƒ������e�͈�(�}W)��ݒ肷��
                                    stPWR.AdjustTargetAry(CndNum) = .dblPowerAdjustTarget(arrIdx)
                                    stPWR.AdjustLevelAry(CndNum) = .dblPowerAdjustToleLevel(arrIdx)
                                End If
                            End With
                        End If
                    Next cndIdx
#End If
                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return�l�ݒ� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "basTrimming.SetAutoPowerCndNumAry() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try
    End Function
#End Region

#Region "Trimming���s��-HALT����������"
    '''=========================================================================
    ''' <summary>�g���~���O���s����HALT�X�C�b�`����i�ꎞ��~����j</summary>
    ''' <param name="curPltNo"></param>
    ''' <param name="curBlkNo"></param>
    ''' <param name="curPltNoX"></param>
    ''' <param name="curPltNoY"></param>
    ''' <param name="curBlkNoX"></param>
    ''' <param name="curBlkNoY"></param>
    ''' <param name="bFgAutoMode"></param>
    ''' <remarks>HALT�X�C�b�`�������͈ꎞ�������~���A
    '''          BP�I�t�Z�b�g�𒲐��ł���悤�ɂ���B
    '''          TKYCHIP�ł́A��R��J�b�g�̏������ύX�ł���悤�ɂ���B</remarks>
    ''' <returns>-1:���̃X�e�b�v�ֈړ�</returns> 
    '''=========================================================================
    Public Function HaltSwOnMove(ByRef curPltNo As Integer, ByRef curBlkNo As Integer,
                                 ByRef curPltNoX As Integer, ByRef curPltNoY As Integer,
                                 ByRef curBlkNoX As Integer, ByRef curBlkNoY As Integer,
                                 ByVal bFgAutoMode As Boolean) As Integer                   '#4.12.2.0�E
        '#4.12.2.0�E    Public Function HaltSwOnMove(ByRef curPltNo As Integer, ByRef curBlkNo As Integer, ByVal bFgAutoMode As Boolean) As Integer

        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        'Dim objForm As Object                                                  '###053
        Dim r As Integer                                                        '###054
        Dim swSts As Integer
        Dim RtnCode As Integer = cFRS_NORMAL                                    '###073 
        Dim AlarmCount As Integer                                               '###073 
        Dim strLoaderAlarm(LALARM_COUNT) As String
        Dim strLoaderAlarmInfo(LALARM_COUNT) As String
        Dim strLoaderAlarmExec(LALARM_COUNT) As String
        Dim coverSts As Long                                                    '###213
        Dim rtn As Integer
        Dim xPos As Double                                                      'V4.8.0.1�B
        Dim yPos As Double                                                      'V4.8.0.1�B

        Try

            ''V4.12.2.2�E��'V6.0.5.0�H
            GraphDispSet()
            ''V4.12.2.2�E��'V6.0.5.0�H

            ' HALT SW�ǂݍ���
            Call HALT_SWCHECK(swSts)
            'If (swSts = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Then          '###255 ###009
            If (swSts = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Or (gbHaltSW = True) Then           '###255
                gbHaltSW = False                                                ' ###255
                '----- V4.0.0.0�R�� -----
                ' SL436S���͎����^�]���́uADJ ON�v�ȊO�͈ꎞ��~��ʂ�\�����Ȃ�(HALT�̓T�C�N����~)(���[���a����)
                'V5.0.0.4�@     If (bFgAutoMode = True) And (gbChkboxHalt <> True) And (giMachineKd = MACHINE_KD_RS) And (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                If (bFgAutoMode = True) And (gbChkboxHalt <> True) And (gbCycleStop Or ((giMachineKd = MACHINE_KD_RS) And (gSysPrm.stCTM.giSPECIAL = customROHM))) Then
                    Call LAMP_CTRL(LAMP_HALT, True)                             ' HALT�����vON
                    bFgCyclStp = True
                    '----- V6.0.3.0�G�� -----
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_CYCLE_STOP)
                    '----- V6.0.3.0�G�� -----
                    '----- V6.0.3.0_37�� -----
                    If gVacuumIO = 1 Then
                        'V4.11.0.0�L �N�����v�y�уo�L���[��OFF
                        If gKeiTyp = KEY_TYPE_RS Then
                            r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                        End If
                        'V4.11.0.0�L
                    End If
                    '----- V6.0.3.0_37�� -----
                    Form1.btnCycleStop.Text = "CYCLE STOP ON"                   ' V5.0.0.4�@
                    Form1.btnCycleStop.BackColor = System.Drawing.Color.Yellow  ' V5.0.0.4�@
                    Return (cFRS_NORMAL)
                End If
                '----- V4.0.0.0�R�� -----
                '----- ###214�� ----
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)                 ' �V�X�e���G���[�`�F�b�N
                If (r <> cFRS_NORMAL) Then
                    Return (r)
                End If
                '----- ###214�� ----

                ' ���[�_���~���� 
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R�n ? 
                    ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))(SL436R�p)
                    If (bFgAutoMode = True) Then                                ' ���[�_�������[�h? ###001(���L���o�͂���ƃN�����v�E�z��OFF����)
                        'Call SetLoaderIO(&H0, LOUT_SUPLY)                       ' ���[�_�͌��T�C�N���I�����_(���[�I���)�Œ�~����
                        Call SetLoaderIO(LOUT_STOP, &H0)                        ' ���[�_�o��(ON=�g���}��~��, OFF=�Ȃ�) ###073
                    End If
                    'V4.8.0.1�B��
                    ' �␳�N���X���C����\������
                    If gMachineType = MACHINE_TYPE_436S Then
                        Call ZGETBPPOS(xPos, yPos)
                        ObjCrossLine.CrossLineDispXY(xPos, yPos)
                    End If
                    'V4.8.0.1�B��
                Else
                    ' ���[�_�o��(ON=���쒆, OFF=�Ȃ�)                           ' 432R�n
                    Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                    ' �g���}���ꎞ��~��ԂƂ��� '###035
                End If

                ' �A�v�����[�h���u�ꎞ��~��ʁv�ɂ��� ###088
                giAppMode = APP_MODE_FINEADJ
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436�n�̏ꍇ��
                    Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
                    '----- V1.18.0.1�G�� -----
                    ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
                    RtnCode = EL_Lock_OnOff(EX_LOK_MD_OFF)
                    If (RtnCode = cFRS_TO_EXLOCK) Then '                        ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                        RtnCode = cFRS_ERR_RST
                        GoTo STP_END                                            ' �����I���� 
                    End If
                    If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                        Return (r)                                              ' �A�v���P�[�V���������I��
                    End If
                    RtnCode = cFRS_NORMAL
                    '----- V1.18.0.1�G�� -----
                End If

                ' CMOS��؂��Windows�����Ԃ�ݒ肷��
                'V4.3.0.0�E�@Call GETSETTIME()
                ' �����vON
                Call LAMP_CTRL(LAMP_START, True)  ' START�����vON 
                Call LAMP_CTRL(LAMP_RESET, True)  ' RESET�����vON 

                '�f�W�^���X�C�b�`�̒l�擾
                Call Form1.GetMoveMode(digL, digH, digSW)

                'Version�{�^�����̖�����
                Form1.mnuHelpAbout.Enabled = False

                ' �}�K�W���㉺����@����
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R�n ? ###182
                    'V4.0.0.0�P��
                    If gKeiTyp <> KEY_TYPE_RS Then
                        Form1.GroupBox1.Visible = True
                    End If
                    'V4.0.0.0�P��
                End If

                '���O�\���G���A�̈ړ�
                Dim orgSize As System.Drawing.Size
                Dim orgLocation As System.Drawing.Point
                Dim setSize As System.Drawing.Size

                ' Distribute�{�^�����\����
                Form1.chkDistributeOnOff.Visible = False
                Form1.GrpNgBox.Visible = False                                '###149

                ' V4.11.0.0�O��
                '                Form1.BtnSubstrateSet.Enabled = False 'V4.11.0.0�J
                If gbLastSubstrateSet = False And (bFgAutoMode = True) Then
                    Form1.BtnSubstrateSet.Enabled = True
                Else
                    Form1.BtnSubstrateSet.Enabled = False
                End If
                ' V4.11.0.0�O��

                ''V5.0.0.1�C��
                If giNgStop = 1 Then
                    btnJudgeEnable(True)
                End If
                ''V5.0.0.1�C��

                '���̗̈��ۑ�
                orgSize = Form1.txtLog.Size
                orgLocation = Form1.txtLog.Location

                If gKeiTyp <> KEY_TYPE_RS Then
                    'V6.0.0.0�E                  ��
                    Form1.Instance.frmHistoryData.Visible = False
                    If (Form1.Instance.VideoLibrary1.SetTrackBarVisible(True)) Then
                        Form1.Instance.txtLog.Location =
                            Point.Add(Form1.Instance.HistoryDataLocation, New Size(0, 30))
                        setSize.Height = 420
                    Else    'V6.0.0.0�E                  ��
                        Form1.txtLog.Location = Form1.Instance.HistoryDataLocation
                        setSize.Height = 450
                    End If
                    setSize.Width = Form1.Instance.HistoryDataSize.Width
                    Form1.txtLog.Size = setSize
                    Form1.SetTrimMapVisible(Form1.MapOn)                'V6.0.1.0�J
                End If

                '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
                ' WALSIN�a�͔�\���ɂ��Ȃ�
                'Form1.GrpQrCode.Visible = False                                ' QR���ޏ����\�� V1.18.0.0�A
                ' ���[���a���� ? 
                'V5.0.0.9�R                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                'V5.0.0.9�S                If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.Taiyo = BarCode_Data.Type) Then 'V5.0.0.9�R
                If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.Walsin <> BarCode_Data.Type) Then 'V5.0.0.9�R
                    Form1.GrpQrCode.Visible = False                             ' QR���ޏ����\�� V1.18.0.0�A
                End If
                '----- V4.11.0.0�A�� -----

                '���C����ʂ̃X�N���[���o�[�͖����ɂȂ邽�߁A
                '�\���ʒu���ŉ��w�֐ݒ肵����
                'Form1.txtLog.ScrollBars = ScrollBars.None                      ' �X�N���[���o�[�͕\������ ###014
                '#4.12.2.0�C                Form1.txtLog.ScrollToCaret()                                    ' ���݂̃J���b�g�ʒu�܂ŃX�N���[������(��ԉ��܂ŕ\������) 
                Form1.TxtLogScrollToCaret() '#4.12.2.0�C

                '----- V1.18.0.7�@�� -----
                ' SL436R���Ŏ����^�]���̈ꎞ��~���́A�V�O�i���^���[�̗Γ_��(�����^�]��)�͏���
                If ((bFgAutoMode = True) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436)) Then
                    ' �V�O�i���^���[����(On=�Ȃ�, Off=�����^�]��(�Γ_��))
                    'V5.0.0.9�M �� V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_GRN_ON)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_IDLE)
                    'V5.0.0.9�M �� V6.0.3.0�G 

                End If
                '----- V1.18.0.7�@�� -----

                '--------------------------------------------------------------
                '   �ꎞ��~�p�������t�H�[���̕\��
                '--------------------------------------------------------------
                gbExitFlg = False                                               '###014
                'objForm = New frmFineAdjust()                                  '###053
                gObjADJ = New frmFineAdjust()                                   '###053

                'frmFineAdjust.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo)'###014
                'Call frmFineAdjust.Focus() ' ###014
                'Call frmFineAdjust.ShowDialog()' ###014

                '----- ###053(�ύX)�������� -----
                'Call objForm.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo)
                'Call objForm.Focus()                                           ' ###014
                'Call objForm.Show()                                            ' ###014

                If gKeiTyp = KEY_TYPE_RS Then
                    ' �摜�\���v���O�������I������
                    'V6.0.0.0�D                    r = End_SmallGazouProc(ObjGazou)
                    GroupBoxEnableChange(True)
                    Form1.CbDigSwH.Enabled = True                  ' DisplayMode�R���{�{�b�N�X���X�g
                    Form1.CbDigSwL.Enabled = True                  ' MoveMode�R���{�{�b�N�X���X�g
                    'V4.0.0.0�A
                    '�X�^�[�g�X�C�b�`���b�`�N���A   
                    Call ZCONRST()
                    'V4.0.0.0�A
                    'V4.11.0.0�H
                    GrpStartBlkPartEnable()
                    'V4.11.0.0�H

                    'V6.0.0.0�D                Else
                    ' �摜�\���v���O�������I������ '###054
                    'V6.0.0.0�D                    Call End_GazouProc(ObjGazou)
                End If
                Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(False)   'V6.0.0.0-26

                '---------------------------------------------------------------
                '   �ꎞ��~�p�������t�H�[����\������
                '---------------------------------------------------------------

                '#4.12.2.0�E                Call gObjADJ.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo)
                Call gObjADJ.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo,
                                            curPltNoX, curPltNoY, curBlkNoX, curBlkNoY)     '#4.12.2.0�E
                If gKeiTyp <> KEY_TYPE_RS Then

                    Form1.SetMapOnOffButtonEnabled(True)                        ' MAP ON/OFF �{�^����L���ɂ���  'V4.12.2.0�@

                    ' SL432R/SL436R�̏ꍇ
                    Call gObjADJ.Focus()
                    Call gObjADJ.Show()
                    '----- ###053(�ύX)�����܂� -----
                Else
                    ' SL436S�̏ꍇ
                    BPAdjustButton.Visible = True
                    '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
                    ' �ꎞ��~�J�n���Ԃ�ݒ肷��(�I�v�V����)
                    m_blnElapsedTime = True                             ' �o�ߎ��Ԃ�\������
                    Call Set_PauseStartTime(StPauseTime)
                    '----- V4.11.0.0�C�� -----
                    Form1.TimerAdjust.Enabled = True
                    'V4.0.0.0-82        ' 
                    BlockNextButton.Enabled = True
                    BlockRvsButton.Enabled = True
                    BlockMainButton.Enabled = True
                    'V4.0.0.0-82        ' 
                    DataEditButton.Visible = True 'V4.0.0.0-84 
                    '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
                    ' �g���~���O�J�n�u���b�N�ԍ��w��(�蓮���[�h��(�I�v�V����))������
                    'V5.0.0.9�O                    If (giStartBlkAss = 1) And (bFgAutoMode = False) Then
                    If (giStartBlkAss <> 0) AndAlso (bFgAutoMode = False) Then  'V5.0.0.9�O
                        Call Form1.Set_StartBlkNum_Enabled(True)
                    End If
                    '----- V4.11.0.0�D�� -----
                End If

                ' �ꎞ��~�p�������t�H�[���̏I����҂� ###014
                Do
                    System.Threading.Thread.Sleep(100)                          ' Wait(ms)
                    System.Windows.Forms.Application.DoEvents()

                    'V4.8.0.1�B��
                    ' �␳�N���X���C����\������
                    If gMachineType = MACHINE_TYPE_436S Then
                        Call ZGETBPPOS(xPos, yPos)
                        ObjCrossLine.CrossLineDispXY(xPos, yPos)
                    End If
                    'V4.8.0.1�B��
                    ' �C���^�[���b�N��Ԃ̕\������у��[�_�֒ʒm(SL436R) ###162
                    r = Form1.DispInterLockSts()
                    '----- ###213�� -----
                    ' �C���^�[���b�N�S����/�ꕔ�����ŁA�J�o�[�ُ͈�Ƃ���(SL436R��) 
                    If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                        'V4.0.0.0�M    If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gKeiTyp <> KEY_TYPE_RS) Then  'V4.0.0.0�M

                        r = COVER_CHECK(coverSts)                           ' �Œ�J�o�[��Ԏ擾(0=�Œ�J�o�[�J, 1=�Œ�J�o�[��))
                        If (coverSts = 1) Then                              ' �Œ�J�o�[�� ?
                            ' �n�[�h�E�F�A�G���[(�J�o�[�X�C�b�`�I��)���b�Z�[�W�\��
                            Call Form1.System1.Form_Reset(cGMODE_ERR_HW, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                            RtnCode = cFRS_ERR_HW                           ' Return�l = �n�[�h�E�F�A�G���[���o
                            Return (RtnCode)                                ' �A�v�������I��
                        End If
                    End If
                    '----- ###213�� -----

                    ' ���[�_�A���[���`�F�b�N(���[�_�������[�h��)(SL436R) ###073
                    If (bFgAutoMode = True) And (giErrLoader <> cFRS_ERR_RST) Then
                        If (gfrmAdjustDisp = 1) Then
                            gObjADJ.Sub_StopTimer()
                        End If
                        'V6.0.0.0�Q                        r = Loader_AlarmCheck(gSysPrm, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        r = Loader_AlarmCheck(Nothing, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec) 'V6.0.0.0�Q
                        If (r <> cFRS_NORMAL) Then
                            'V5.0.0.1�L��
                            If gKeiTyp = KEY_TYPE_RS Then
                                Form1.BtnSubstrateSet.Enabled = False                       ' ������{�^���񊈐���
                            End If
                            'V5.0.0.1�L��
                            giErrLoader = r                                     ' ���[�_�A���[�����oON
                            RtnCode = r                                         ' Return�l�ݒ� 
                            Call W_RESET()                                      ' �A���[�����Z�b�g���o 
                            'V5.0.0.1�O
                            If (r <> cFRS_ERR_RST) Then
                                Call W_START()                                      ' �X�^�[�g�M�����o 
                            End If
                            'V5.0.0.1�O
                        End If
                        If (gfrmAdjustDisp = 1) Then
                            gObjADJ.Sub_StartTimer()
                        End If

                    End If
                    ''V5.0.0.1-28
                    If (bFgAutoMode = False) Then
                        '----- ###209�� -----
                        ' �J�o�[���m�F����(SL436R���Ŏ蓮���[�h��)
                        If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (bFgAutoMode = False) Then
                            Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A
                            r = FrmReset.Sub_CoverCheck()
                            If (r < cFRS_NORMAL) Then                               ' ����~�����o ?
                                RtnCode = r                                     ' Return�l = �n�[�h�E�F�A�G���[���o
                                Return (RtnCode)                                ' �A�v�������I��
                            End If
                        End If
                        '----- ###209�� -----
                    End If
                    ''V5.0.0.1-28

                    'V4.0.0.0�@�@��
                    ' ����~���`�F�b�N(�g���}���u�A�C�h����)
                    r = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
                    If (r <> cFRS_NORMAL) Then                          ' ����~�����o ?
                        RtnCode = r                                     ' Return�l = �n�[�h�E�F�A�G���[���o
                        Return (RtnCode)                                ' �A�v�������I��
                    End If
                    'V4.0.0.0�@�@��

                Loop While (gbExitFlg = False)

                '----- ###232��(�\����DllTrimClassLiblary�ŕ\�������) -----
                ' �␳��N���X���C��X,Y��\��
                'V6.0.0.0�C                Form1.CrosLineX.Visible = False
                'V6.0.0.0�C                Form1.CrosLineY.Visible = False
                Form1.VideoLibrary1.SetCorrCrossVisible(False)          'V6.0.0.0�C
                '----- ###232�� -----
                If gKeiTyp = KEY_TYPE_RS Then
                    BPAdjustButton.Visible = False
                    Form1.TimerAdjust.Enabled = False
                    '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
                    ' �ꎞ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����(�I�v�V����)
                    Call Set_PauseTotalTime(StPauseTime)
                    ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��(�I�v�V����)
                    TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
                    '----- V4.11.0.0�C�� -----
                    SetSimpleVideoSize()
                    GroupBoxEnableChange(False)
                    ' �f�[�^�\�����\���ɂ���
                    Call Form1.SetDataDisplayOn()                           'V4.0.0.0�J
                    GroupBoxVisibleChange(True)                             'V4.0.0.0�J
                    'V4.0.0.0-82        ' 
                    BlockNextButton.Enabled = False
                    BlockRvsButton.Enabled = False
                    BlockMainButton.Enabled = False
                    'V4.0.0.0-82        ' 
                    DataEditButton.Visible = False 'V4.0.0.0-84 
                    '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
                    ' �g���~���O�J�n�u���b�N�ԍ��w��(�蓮���[�h��(�I�v�V����))�񊈐���
                    'V5.0.0.9�O                    If (giStartBlkAss = 1) And (bFgAutoMode = False) Then
                    If (giStartBlkAss <> 0) AndAlso (bFgAutoMode = False) Then  'V5.0.0.9�O
                        Call Form1.Set_StartBlkNum_Enabled(False)
                    End If
                    '----- V4.11.0.0�D�� -----
                    'V4.11.0.0�H
                    Form1.btnNEXT.Enabled = False
                    Form1.btnPREV.Enabled = False
                    'V4.11.0.0�H
                    'V5.0.0.1�M��
                    If (giSubstrateInvBtn = 1) Then                         ' �ꎞ��~��ʂł́u������v�{�^���̗L�� ?�@
                        If giErrLoader <> cFRS_NORMAL Then
                            RtnCode = giErrLoader                                         ' Return�l�ݒ� 
                        End If
                    End If
                    'V5.0.0.1�M��
                End If

                ''V5.0.0.1�C��
                If giNgStop = 1 Then
                    btnJudgeEnable(False)
                End If
                ''V5.0.0.1�C��
                '----- ###141�� -----
                ' ���[�_�A���[�����o(�y�̏�)�ő��s�w��Ȃ烊�^�[���l�ɐ����ݒ肷��
                'If (RtnCode = cFRS_ERR_LDR3) Then                              ' ###196 ���[�_�A���[�����o(�y�̏�) ?
                If (RtnCode = cFRS_ERR_LDR3) Or (RtnCode = cFRS_ERR_LDR2) Then  ' ###196 ���[�_�A���[�����o(�y�̏�, �T�C�N����~) ?
                    giErrLoader = cFRS_NORMAL
                    RtnCode = cFRS_NORMAL
                End If
                '----- ###141�� -----

                '----- V1.18.0.6�@�� -----
                ' SL436R���Ŏ����^�]���̓V�O�i���^���[����(�����^�]��(�Γ_��))���s��
                If ((bFgAutoMode = True) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436)) Then
                    ' �V�O�i���^���[����(On=�����^�]��(�Γ_��),Off=�S�ޯ�)
                    'V5.0.0.9�M �� V6.0.3.0�G
                    'Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                    'V5.0.0.9�M �� V6.0.3.0�G
                End If
                '----- V1.18.0.6�@�� -----
                '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
                ' �g���~���O�J�n�u���b�N�ԍ���\������
                'V5.0.0.9�O                If (giStartBlkAss = 1) Then                             ' �g���~���O�J�n�u���b�N�ԍ��w��̗L��/����(0=����, 1=�L��)�@
                If (giStartBlkAss <> 0) AndAlso (gKeiTyp = KEY_TYPE_RS) Then    ' �g���~���O�J�n�u���b�N�ԍ��w��̗L��/����(0=����, 1=�L��)     'V5.0.0.9�O
                    Form1.GrpStartBlk.Visible = True
                    'V5.0.0.1�@��
                    SetNowBlockDspNum(gCurBlockNo)
                    'V5.0.0.1�@��
                End If
                '----- V4.11.0.0�D�� -----

                ' �����vOFF
                Call LAMP_CTRL(LAMP_START, False)                               ' START�����vOFF 
                Call LAMP_CTRL(LAMP_RESET, False)                               ' RESET�����vOFF 

                '----- ###150�� -----
                ' ���v�\�����͓��v�\����̃{�^���𖳌��ɂ���(CHIP/NET��)
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then             ' ###157 (CHIP/NET��)
                    gObjFrmDistribute.cmdGraphSave.Enabled = False
                    gObjFrmDistribute.cmdInitial.Enabled = False
                    gObjFrmDistribute.cmdFinal.Enabled = False
                End If
                '----- ###150�� -----

                ' �}�K�W���㉺����@����
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R�n ? ###182
                    Form1.GroupBox1.Visible = False
                End If

                'V4.11.0.0�J��
                If bFgAutoMode = True Then
                    'V4.11.0.0�O��
                    '                    Form1.BtnSubstrateSet.Enabled = True
                    Form1.BtnSubstrateSet.Enabled = False
                    'V4.11.0.0�O��
                End If
                'V4.11.0.0�J��
#If False Then                          'V6.0.0.0�D
                ' �摜�\���v���O�������N������ '###054
                If gKeiTyp = KEY_TYPE_RS Then
                    ' ������ V3.1.0.0�A 2014/12/01
                    'r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                    r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 1)
                    ' ������ V3.1.0.0�A 2014/12/01
                    '                    r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                Else
                    If (Form1.chkDistributeOnOff.Checked = False) Then              ' ���v��ʔ�\�����ɋN������ ###116
                        ' ������ V3.1.0.0�A 2014/12/01
                        'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                        r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                        ' ������ V3.1.0.0�A 2014/12/01
                    End If

                End If
#End If
                ' ���O�\���G���A�����ɖ߂�
                '���̗̈��ۑ�
                Form1.txtLog.Location = orgLocation
                Form1.txtLog.Size = orgSize
                'Form1.txtLog.ScrollBars = ScrollBars.Vertical
                Form1.txtLog.ScrollBars = ScrollBars.Both                       ' ###010 
                '#4.12.2.0�C                Form1.txtLog.ScrollToCaret()
                Form1.TxtLogScrollToCaret()     '#4.12.2.0�C

                'V6.0.0.0�E                  ��
                If gKeiTyp <> KEY_TYPE_RS Then
                    Form1.Instance.VideoLibrary1.SetTrackBarVisible(False)
                    Form1.Instance.frmHistoryData.Visible = True
                    Form1.MoveHistoryDataLocation(True)                     'V6.0.1.0�J

                    Form1.SetMapOnOffButtonEnabled(False)                        ' MAP ON/OFF �{�^����L���ɂ���  'V4.12.2.0�@
                    Form1.SetTrimMapVisible(True)                       'V6.0.1.0�J
                End If
                'V6.0.0.0�E                  ��

                '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
                '----- V1.18.0.0�A�� -----
                ' QR���ޏ��/�o�[�R�[�h���(���z��)/�o�[�R�[�h���(WALSIN)��\�� V1.23.0.0�@
                'V5.0.0.9�N                If (gSysPrm.stCTM.giSPECIAL = customROHM) Or (gSysPrm.stCTM.giSPECIAL = customTAIYO) Or (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
                'If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse
                '    (BarcodeType.Taiyo = BarCode_Data.Type) OrElse
                '    (BarcodeType.Walsin = BarCode_Data.Type) Then ' V5.0.0.9�N
                If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.None <> BarCode_Data.Type) Then 'V5.0.0.9�S
                    Form1.GrpQrCode.Visible = True
                End If
                '----- V1.18.0.0�A�� -----
                '----- V4.11.0.0�A�� -----

                ' �f�[�^�擾
                'objForm.GetStagePosInfo(curPltNo, curBlkNo)                        '###053
                'HaltSwOnMove = objForm.GetReturnVal()                              '###053
                '#4.12.2.0�E                gObjADJ.GetStagePosInfo(curPltNo, curBlkNo)                         '###053
                gObjADJ.GetStagePosInfo(curPltNo, curBlkNo, curPltNoX, curPltNoY, curBlkNoX, curBlkNoY) '#4.12.2.0�E
                r = gObjADJ.GetReturnVal()                                          '###053

                ' ���C�����̃^�C�}�[��RESET�{�^�������o�����ꍇ�̏���
                'V4.10.0.0�I                If gKeiTyp = KEY_TYPE_RS Then
                If (gMachineType = MACHINE_TYPE_436S) Then
                    'V4.9.0.0�@��
                    ' �����l�A�؂�ւ��|�C���g�A�^�[���|�C���g�̃e�[�u����]������B 
                    SendTrimDataCutPoint()
                    'V4.9.0.0�@��

                    rtn = Form1.GetResultAdjust()
                    If (rtn = cFRS_ERR_RST) Then
                        r = rtn
                    End If
                End If
                If (RtnCode = cFRS_NORMAL) Then                                     '###073 
                    RtnCode = r
                End If

                'V4.1.0.0�N��
                'If (gbChkboxHalt = True) Then                                       '###009
                '    Form1.BtnADJ.Text = "ADJ ON"                                    '###009
                '    Form1.BtnADJ.BackColor = System.Drawing.Color.Yellow            '###009
                'Else                                                                '###009
                '    Form1.BtnADJ.Text = "ADJ OFF"                                   '###009
                '    Form1.BtnADJ.BackColor = System.Drawing.SystemColors.Control    '###009
                'End If                                                              '###009
                SetADJButton()
                'V4.1.0.0�N��

                ' Version�{�^�����̗L����
                Form1.mnuHelpAbout.Enabled = True
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then               ' ###149
                    'Form1.GrpNgBox.Visible = True
                    Form1.GrpNgBox.Visible = False                                  '###195
                End If

                ' ��������ʃI�u�W�F�N�g�J��
                '----- ###053(�ύX)�������� -----
                ''Call frmFineAdjust.Close()                                    ' ###014
                'If (objForm Is Nothing = False) Then
                '    Call objForm.Close()                                       ' �I�u�W�F�N�g�J��
                '    Call objForm.Dispose()                                     ' ���\�[�X�J��
                'End If
                If (gObjADJ Is Nothing = False) Then
                    Call gObjADJ.Sub_StopTimer()                                ' ###260
                    Call gObjADJ.Close()                                        ' �I�u�W�F�N�g�J��
                    Call gObjADJ.Dispose()                                      ' ���\�[�X�J��
                    gObjADJ = Nothing
                End If
                '----- ###053(�ύX)�����܂� -----
                Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(True)    'V6.0.0.0-26

                ' �\���̍X�V
                Form1.Refresh()

                '----- ###088�� -----
STP_END:        ' V1.18.0.1�G
                ' SL436R����➑̃J�o�[���m�F����
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R�n ?
                    Do
                        System.Threading.Thread.Sleep(100)                      ' Wait(ms)
                        System.Windows.Forms.Application.DoEvents()

                        ' ➑̃J�o�[���m�F����
                        r = FrmReset.CoverCheck(0, False)                       ' ➑̃J�o�[�`�F�b�N(RESET�L�[�����w��, ���_���A�������ȊO)
                        If (r = cFRS_NORMAL) Then Exit Do
                        If (r <> ERR_OPN_CVR) Then Return (r) '                 ' ����~���̃G���[

                        ' "➑̃J�o�[�����","","START�L�[���������AOK�{�^���������ĉ������B"
                        r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                                MSG_SPRASH36, "", MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        ' ����~���̃G���[�Ȃ�A�v�������I����(�G���[���b�Z�[�W�͕\���ς�) 
                        'If (r < cFRS_NORMAL) Then Return (RtnCode)             ' ###193
                        If (r < cFRS_NORMAL) Then Return (r) '                  ' ###193
                        Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A

                    Loop While (1)
                End If
                '----- ###088�� -----
                '----- ###193�� -----
                ' RESET�L�[/�G���[�Ȃ�Return(###088�̑O�����Ɉړ�)
                If (RtnCode = cFRS_ERR_RST) Or (RtnCode <> cFRS_NORMAL) Then    ' ###073
                    If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436�n�̏ꍇ�́u�Œ�J�o�[�J�`�F�b�N����v�ɂ���###133
                        Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A
                        Call COVERCHK_ONOFF(COVER_CHECK_ON)                     ' �u�Œ�J�o�[�J�`�F�b�N����v
                    End If
                    Return (RtnCode)
                End If
                '----- ###193�� -----

                ' ���[�_���ĊJ����
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R�n ? 
                    ' ���[�_�o��(ON=��v��(�A���^�]�J�n), OFF=�Ȃ�)(SL436R�p)
                    If (bFgAutoMode = True) Then                                ' ���[�_�������[�h? ###001(���L���o�͂���ƃN�����v�E�z��OFF����)
                        'Call SetLoaderIO(LOUT_SUPLY, &H0)                       ' �A���^�]�ĊJ
                        Call SetLoaderIO(&H0, LOUT_STOP)                        ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}��~��) ###07
                    End If
                Else
                    ' ���[�_�o��(ON=�^�]��, OFF=�Ȃ�)                           ' 432R�n
                    Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                    ' �g���}���^�]���Ƃ��� ###035
                End If

                ' �A�v�����[�h���u�g���~���O���v�ɖ߂� ###088
                giAppMode = APP_MODE_TRIM
                Call COVERCHK_ONOFF(COVER_CHECK_ON)                             ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���(SL436R��)
                Call COVERLATCH_CLEAR()                                         ' �J�o�[�J���b�`�̃N���A

                '----- V1.18.0.1�G�� -----
                ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
                If (bFgAutoMode = True) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    RtnCode = EL_Lock_OnOff(EX_LOK_MD_ON)
                    If (RtnCode = cFRS_TO_EXLOCK) Then RtnCode = cFRS_ERR_RST ' ' �O�ʔ����b�N�^�C���A�E�g�Ȃ�߂�l���uRESET�v�ɂ���
                End If
                '----- V1.18.0.1�G�� -----
            End If

            ''V4.12.2.2�E��'V6.0.5.0�H
            GraphDispSet()
            ''V4.12.2.2�E��'V6.0.5.0�H
            Return (RtnCode)                                                    '###073 

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.HaltSwOnMove() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)

        Finally                         'V6.0.0.0�I
            Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)
        End Try

    End Function
#End Region
    '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) -----
#Region "�ꎞ��~�J�n���Ԃ�ݒ肷��"
    '''=========================================================================
    ''' <summary>�ꎞ��~�J�n���Ԃ�ݒ肷��</summary>
    ''' <param name="StPauseTime">(OUT)�ꎞ��~���ԏW�v�p�\����</param>
    '''=========================================================================
    Public Sub Set_PauseStartTime(ByRef StPauseTime As PauseTime_Data_Info)

        Dim strMsg As String

        Try
            StPauseTime.StartTime = DateTime.Now
            StPauseTime.EndTime = StPauseTime.StartTime

        Catch ex As Exception
            strMsg = "basTrimming.Set_PauseStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�ꎞ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����"
    '''=========================================================================
    ''' <summary>�ꎞ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����</summary>
    ''' <param name="StPauseTime">(OUT)�ꎞ��~���ԏW�v�p�\����</param>
    '''=========================================================================
    Public Sub Set_PauseTotalTime(ByRef StPauseTime As PauseTime_Data_Info)

        Dim strMsg As String

        Try
            StPauseTime.EndTime = DateTime.Now                                      ' �ꎞ��~�I������
            StPauseTime.PauseTime = StPauseTime.EndTime - StPauseTime.StartTime     ' �ꎞ��~����
            StPauseTime.TotalTime = StPauseTime.TotalTime + StPauseTime.PauseTime   ' �ꎞ��~�g�[�^������

        Catch ex As Exception
            strMsg = "basTrimming.Set_PauseTotalTime() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0�C�� -----
#Region "���z�}�\������"
    '''=========================================================================
    ''' <summary>���z�}�̕\������</summary>
    ''' <remarks>���z�}�\���{�^�������Ȃ番�z�}��\������</remarks>
    '''=========================================================================
    Public Sub DisplayDistribute()
        Dim strMsg As String

        Try
            ' ���̍X�V
            'If (bFgfrmDistribution) Then    ' ���Y���̕\���׸�ON ?
            If (Form1.chkDistributeOnOff.Checked = True) Then    ' ���Y���̕\���׸�ON ?
                gObjFrmDistribute.RedrawGraph()
            End If
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            strMsg = "basTrimming.DisplayDistribute() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub

#End Region

#Region "�J�n���O�\������"
    '''=========================================================================
    ''' <summary>�J�n���O�\��</summary>
    ''' <param name="plateNoX"></param>
    ''' <param name="plateNoY"></param>
    ''' <param name="StageGrpNoX"></param>
    ''' <param name="StageGrpNoY"></param>
    ''' <param name="blockNoX"></param>
    ''' <param name="blockNoY"></param>
    ''' <remarks>�����J�n���̎��s�ӏ��̏���\������B</remarks>
    '''=========================================================================
    Public Sub DisplayStartLog(ByVal plateNoX As Integer, ByVal plateNoY As Integer,
                ByVal StageGrpNoX As Integer, ByVal StageGrpNoY As Integer,
                ByVal blockNoX As Integer, ByVal blockNoY As Integer)
        Dim bDispLogWrite As Boolean
        Dim strLOG As String
        Dim strMsg As String
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        'Dim TotalBlk As Integer                                         ' V4.0.0.0�R


        Try
            SimpleTrimmer.TrimData.SetBlockXYNumber(blockNoX, blockNoY)       'V2.0.0.0�I
            bDispLogWrite = False

            ' �޼�SW��2���ڂ���������B
            Call Form1.GetMoveMode(digL, digH, digSW)
            Select Case digH
                'Select Case gDigH
                'Case 0, 1
                Case 0                                          '###217
                    'Case 0, 1
                    '    ' �޼�SW��1���ڂ���������B
                    '    Select Case digL
                    '        'Select Case gDigL
                    '        Case 0, 1, 2
                    '            ' �\�����Ȃ�
                    '            Exit Sub
                    '        Case Else
                    '            ' ���O��ʂɕ������\������
                    '            bDispLogWrite = True
                    '    End Select
                Case Else
                    ' ���O��ʂɕ������\������
                    bDispLogWrite = True
            End Select

            ' ���O�o�͕�����̍\�z�Əo��
            If (bDispLogWrite = True) Then
                ''V4.4.0.0�A��
                ''----- V4.0.0.0�R�� -----
                '' �����^�]���̍ŏI�u���b�N����" �� Last Block ��"��\������(�T�C�N����~�̂���)
                '' �����[���a�����@SL436R�݂̂�SL436S�͂��Ȃ�
                'If (gSysPrm.stCTM.giSPECIAL = customROHM) And (giMachineKd <> MACHINE_KD_RS) Then
                '    TotalBlk = typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir
                '    If (bFgAutoMode = True) And (TotalBlk <= (blockNoX * blockNoY)) Then
                '        strLOG = "������ Last Block ������"
                '        Call Form1.Z_PRINT(strLOG)
                '    End If
                'End If
                ''----- V4.0.0.0�R�� -----
                ''V4.4.0.0�A��
                ''V4.4.0.0�A��
                'V4.0.0.0-63 ��
                If (giMachineKd <> MACHINE_KD_RS) Then
                    strLOG = "--- Plate X=" + plateNoX.ToString("000") + " Y=" + plateNoY.ToString("000")
                    strLOG = strLOG + "--- StageGroup X=" & StageGrpNoX.ToString("000") + " Y=" & StageGrpNoY.ToString("000")

                    '�@'V4.5.0.0�A �X�e�b�v�����s�[�g�Ȃ��̏ꍇ�ɂ́A�u���b�N�ԍ��͏�ɂP�Ƃ��ĕ\������ 
                    If (typPlateInfo.intDirStepRepeat = STEP_RPT_NON) Then
                        strLOG = strLOG + " Block X=" + "001" + " Y=" + "001" + " CntX=" + blockNoX.ToString("000") + " CntY=" + blockNoY.ToString("000")
                    Else
                        strLOG = strLOG + " Block X=" + blockNoX.ToString("000") + " Y=" + blockNoY.ToString("000")
                    End If
                    '�@'V4.5.0.0�A �X�e�b�v�����s�[�g�Ȃ��̏ꍇ�ɂ́A�u���b�N�ԍ��͏�ɂP�Ƃ��ĕ\������ 

                    ''''�����̌������K�v�B
                    'If StepDir = 1 Then
                    '    strLOG = strLOG & " Block X=" & (LogBlkData + XY(1) + 1).ToString("000") & " Y=" & (XY(2) + 1).ToString("000") '(TXT�d�l�ύX)
                    'Else
                    '    strLOG = strLOG & " Block X=" & (XY(1) + 1).ToString("000") & " Y=" & (LogBlkData + XY(2) + 1).ToString("000") '(TXT�d�l�ύX)
                    'End If
                    'strLOG = strLOG & vbCrLf
                    '#4.12.2.0�C                    strLOG = strLOG

                    ' ���O��ʂɕ������\������
                    Call Form1.Z_PRINT(strLOG)
                End If
                'V4.0.0.0-63 ��
                ''V4.4.0.0�A��
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "basTrimming.DisplayStartLog() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�f�o�b�O�p�`�F�b�N�֐�"
    '''=========================================================================
    '''<summary>�f�o�b�O���Ɏ��s����֐��B�����m�F���������͕s���B</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub DebugCheck()
#If cOFFLINEcDEBUG Then
        Dim i As Integer
        Dim strMsg As Integer

        Try
            For i = 1 To MaxCntResist - 1                       ' 1-999
                gwTrimResult(i) = 1
                ' (��R�ް�)�ڕW�l�w�� = ��Βl�ł��邩��������B
                If typResistorInfoArray(i).intTargetValType = TARGET_TYPE_ABSOLUTE Then
                    ' IT ��R�l��ݒ肷��B
                    gfInitialTest(i) = typResistorInfoArray(i).dblTrimTargetVal * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                    ' FT��R�l��ݒ肷��B
                    gfFinalTest(i) = typResistorInfoArray(i).dblTrimTargetVal * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                Else
                    gfInitialTest(i) = 1000.0# * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                    gfFinalTest(i) = 1000.0# * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                End If
            Next
        Catch ex As Exception
            strMsg = "basTrimming.DebugCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
#End If
    End Sub
#End Region

#Region "NG����臒l�̎Z�o"
    '''=========================================================================
    ''' <summary>NG����臒l�̎Z�o</summary>
    ''' <param name="ngJudgeUnit">       (OUT)NG����P��(0:BLOCK, 1:PLATE)</param>
    ''' <param name="ngJudgeResCntInBlk">(OUT)NG��������{�����R��(�u���b�N�P�ʂł�NG����)</param>
    ''' <param name="ngJudgeCntInPlt">   (OUT)NG��������{�����R��(�v���[�g�P�ʂł�NG����)</param>
    '''=========================================================================
    Public Sub CalcNgJudegeResCnt(ByRef ngJudgeUnit As Integer, ByRef ngJudgeResCntInBlk As Integer, ByRef ngJudgeCntInPlt As Integer)

        Dim iNgJudgeRate As Integer
        Dim iResistor As Integer
        Dim i As Integer

        '-----------------------------------------------------------
        '   NG����臒l�̎Z�o
        '-----------------------------------------------------------
        With typPlateInfo
            ngJudgeUnit = .intNgJudgeUnit           ' NG����P�ʎ擾(0:BLOCK, 1:PLATE)
            'ngJudgeUnit = ngJudgeUnit <> 0         '����r���Z�̈Ӗ����s���B�B�B ###142
            iNgJudgeRate = .intNgJudgeLevel         ' NG JUDGEMENT RATE 0-100%

            ' �P�u���b�N�̒�R�����J�E���g
            iResistor = 0

            'V5.0.0.6�D��
            'If (gTkyKnd = KND_TKY)  Then                                   'V6.0.1.0�N
            If (gTkyKnd = KND_TKY) AndAlso giNGCountInPlate = 0 Then        'V6.0.1.0�N
                iResistor = .intCircuitCntInBlock
            ElseIf (gTkyKnd = KND_NET) Then
                iResistor = .intGroupCntInBlockXBp
            Else
                'V5.0.0.6�D��
                ' ��R�����������s�Ȃ��B(TKY_CHIP)
                For i = 1 To gRegistorCnt
                    ' ��R�ԍ�<1000�ł��邩�`�F�b�N
                    If typResistorInfoArray(i).intResNo < 1000 Then
                        ' �P�u���b�N�̒�R���̃J�E���g
                        iResistor = iResistor + 1
                    End If
                Next
            End If
            'V5.0.0.6�D��

            ' �u���b�N�P�ʂł�NG����臒l
            ' �u�u���b�N���̒�R��xNG���藦�v���NG��������{�����R�����Z�o
            ngJudgeResCntInBlk = iResistor * (iNgJudgeRate / 100.0#)

            ' �v���[�g�P�ʂł�NG����臒l
            ' �u�P�v���[�g�̑���R���i�P�u���b�N�̒�R�� �~ ���u���b�N���j��NG���藦�v���
            ' NG������{�����R�����Z�o()
            ' lResistorPlateTotalCount = iResistor * bxy(1) * bxy(2) * (iNgJudgeRate / 100.0#)
            '#4.12.2.0�F ngJudgeCntInPlt = iResistor * .intBlockCntXDir * .intBlockCntYDir * (iNgJudgeRate / 100.0#)
            ngJudgeCntInPlt = iResistor * CInt(.intBlockCntXDir) * CInt(.intBlockCntYDir) * (iNgJudgeRate / 100.0#) '#4.12.2.0�F

            'V6.0.1.0�N��
            If giNGCountInPlate <> 0 Then
                ngJudgeResCntInBlk = iNgJudgeRate
                ngJudgeCntInPlt = iNgJudgeRate
            End If
            'V6.0.1.0�N��

            '----- 60.1.1.0�H�� -----
            ' NG������%�łȂ�NG���̏ꍇ(�I�v�V����)
            If (giNgCountAss = 1) Then
                ' NG����P�ʂ��u���b�N�P�ʂ̏ꍇ  ��R�� = NG��R��
                ngJudgeResCntInBlk = iNgJudgeRate
                '  NG����P�ʂ��v���[�g�P�ʂ̏ꍇ ��R�� = NG��R��
                ngJudgeCntInPlt = iNgJudgeRate
            End If
            '----- 60.1.1.0�H�� -----
        End With
        '-----------------------------------------------------------
    End Sub
#End Region

#Region "�G���[�R�[�h�ɂ��p������\�����肷��"
    '''=========================================================================
    ''' <summary>�G���[�R�[�h�ɂ��p������\�����肷��</summary>
    ''' <param name="errCode"></param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function IsAliveErrorCode(ByVal errCode As Integer) As Boolean
        Dim absCode As Integer
        Dim ret As Boolean

        Try
            ret = True
            absCode = System.Math.Abs(errCode)

            Select Case (absCode)
                Case ERR_CMD_NOTSPT, ERR_CMD_PRM, ERR_CMD_LIM_L,
                    ERR_CMD_LIM_U, ERR_RT2WIN_SEND, ERR_RT2WIN_RECV,
                    ERR_WIN2RT_SEND, ERR_WIN2RT_RECV, ERR_SYS_BADPOINTER,
                    ERR_SYS_FREE_MEMORY, ERR_SYS_ALLOC_MEMORY, ERR_CALC_OVERFLOW,
                    ERR_INTIME_NOTMOVE, ERR_BP_MOVE_TIMEOUT, ERR_BP_GRV_ALARM_X, ERR_BP_GRV_ALARM_Y,
                    ERR_BP_GRVMOVE_HARDERR, ERR_LSR_STATUS_STANBY, ERR_LSR_STATUS_OSCERR,
                    ERR_OPN_CVR, ERR_OPN_SCVR, ERR_OPN_CVRLTC
                    ret = False
                Case Else
                    ret = True
            End Select

            Return (ret)
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.IsAliveErrorCode() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (False)
        End Try

    End Function
#End Region

#Region "�g���~���O�u���b�N�̃��[�v����"
    '''=========================================================================
    ''' <summary>�g���~���O�u���b�N�̃��[�v����</summary>
    ''' <param name="stgx"></param>
    ''' <param name="stgy"></param>
    ''' <param name="zStepPos"></param>
    ''' <param name="zWaitPos"></param>
    ''' <param name="zOnPos"></param>
    ''' <param name="bpOffX"></param>
    ''' <param name="bpOffY"></param>
    ''' <param name="blkSizeX"></param>
    ''' <param name="blkSizeY"></param>
    ''' <param name="digH"></param>
    ''' <param name="digL"></param>
    ''' <param name="bLoaderNg"></param>
    ''' <param name="strLogDataBuffer"></param>
    ''' <param name="bFgAutoMode"></param>
    ''' <remarks>�u���b�N�P�ʂ̃g���~���O�̃��[�v����
    '''          �v���[�g���E�u���b�N�������̊֐����ŏ��������[�v�����B
    ''' </remarks>
    ''' <returns></returns> 
    '''=========================================================================
    Public Function TrimmingBlockLoop(ByVal stgx As Double, ByVal stgy As Double,
                                      ByVal zStepPos As Double, ByVal zWaitPos As Double, ByVal zOnPos As Double,
                                      ByVal bpOffX As Double, ByVal bpOffY As Double, ByVal blkSizeX As Double, ByVal blkSizeY As Double,
                                      ByVal digH As Integer, ByVal digL As Integer,
                                      ByRef bLoaderNg As Boolean, ByVal strLogDataBuffer As StringBuilder, ByVal bFgAutoMode As Boolean) As Integer
        '#4.12.2.0�C    Public Function TrimmingBlockLoop(ByVal stgx As Double, ByVal stgy As Double, _
        '#4.12.2.0�C                                      ByVal zStepPos As Double, ByVal zWaitPos As Double, ByVal zOnPos As Double, _
        '#4.12.2.0�C                                      ByVal bpOffX As Double, ByVal bpOffY As Double, ByVal blkSizeX As Double, ByVal blkSizeY As Double, _
        '#4.12.2.0�C                                      ByVal digH As Integer, ByVal digL As Integer, _
        '#4.12.2.0�C                                      ByRef bLoaderNg As Boolean, ByRef strLogDataBuffer As String, ByVal bFgAutoMode As Boolean) As Integer

        Dim currentPlateNo As Integer
        Dim currentBlockNo As Integer

        Dim retBlkNoX As Integer
        Dim retBlkNoY As Integer
        Dim dispCurPltNoX As Integer
        Dim dispCurPltNoY As Integer
        Dim dispCurStgGrpNoX As Integer
        Dim dispCurStgGrpNoY As Integer
        Dim dispCurBlkNoX As Integer
        Dim dispCurBlkNoY As Integer
        Dim nextStagePosX As Double
        Dim nextStagePosY As Double
        Dim r As Integer
        Dim ngJudgeUnit As Integer
        Dim ngJudgeResCntInBlk As Integer
        Dim ngJudgeResCntInPlt As Integer
        Dim ngCount As Integer
        Dim iDummy As Integer
        'Dim curMode As Integer
        'Dim blnCheckPlate As Boolean                                   ' �l�������ݒ肳��Ȃ��B�K�v������  ###093
        Dim bFlgInit As Boolean = True                                  ' ###045
        Dim FlErrCode As Integer
        Dim strMsg As String
        Dim contNgCountError As Integer = 0                             ' �A��HI-NG�����t���O������ ###129
        Dim Sw As Long                                                  ' ###255
        Dim i As Long                                                   ' ���g���C�񐔃J�E���g�p 'V2.0.0.0�M 
        Dim strLOG As String = ""                                       ' V1.23.0.0�F 
        Dim ret1 As Integer    'V6.0.5.0�C
        '@@@888 test ���Ԍv���p
        '        Dim StopWatch As New System.Diagnostics.Stopwatch
        '       Dim sw_save(100) As TimeSpan
        'Dim strStopwatch As String '@@@888


        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            r = cFRS_NORMAL
            ngCount = 0
            currentPlateNo = 1
            'V5.0.0.9�O            currentBlockNo = 1    �� GetCurrentBlockNo() �Őݒ肷��

            dispCurPltNoX = 1
            dispCurPltNoY = 1
            dispCurStgGrpNoX = 1
            dispCurStgGrpNoY = 1
            dispCurBlkNoX = 1
            dispCurBlkNoY = 1
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ�XY���擾����(�蓮�^�]���̂ݗL��)
            Call Form1.Get_StartBlkNum(dispCurBlkNoX, dispCurBlkNoY)
            'V5.0.0.9�O            currentBlockNo = dispCurBlkNoX * dispCurBlkNoY
            currentBlockNo = GetProcessingOrder(dispCurBlkNoX, dispCurBlkNoY)    'V5.0.0.9�O
            '----- V4.11.0.0�D�� -----

            '----- V1.23.0.0�E�� -----
            ' ��f�B���C(�f�B���C�g�����Q)���̍ŏI�u���b�N�ԍ���ݒ肷��
            If (m_blnDelayCheck) Then                                   ' ��f�B���C ? 
                m_intDelayBlockIndex = 1                                ' ���݂̃u���b�N�ԍ�(�J�b�g�ԍ�)������
                m_blnDelayLastCut = False                               ' �ŏI�J�b�g�t���OOFF
            End If
            If (digL > 2) Then                                          ' ��f�B���C(�f�B���C�g�����Q)��
                m_blnDelayCheck = False                                 ' x0,x1���̂ݗL���Ƃ���
            End If
            '----- V1.23.0.0�E�� -----
            TrimmingBlockLoop = cFRS_NORMAL

            '----- ###255��-----
            ' HALT SW�ǂݍ���
            gbHaltSW = False
            bFgCyclStp = False                                          ' �T�C�N����~�t���OOFF V4.0.0.0�R
            Form1.btnCycleStop.Text = "CYCLE STOP OFF"                          'V5.0.0.4�@
            Form1.btnCycleStop.BackColor = System.Drawing.SystemColors.Control  'V5.0.0.4�@
            Call HALT_SWCHECK(Sw)
            If (Sw = cSTS_HALTSW_ON) Then
                gbHaltSW = True                                         ' HALT SW��ԑޔ�
            End If
            '----- ###255��-----
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`���� ###042

            '=====================================
            ' NG����臒l�̒�R�����擾
            '=====================================
            Call CalcNgJudegeResCnt(ngJudgeUnit, ngJudgeResCntInBlk, ngJudgeResCntInPlt)

            ' V4.5.0.5�@ NG���J�E���g�p  'V4.12.2.0�H�@'V6.0.5.0�C
            m_NgCountInPlate = 0

            '-------------------------------------------------------------------
            '   1���(�u���b�N����)�ȉ����J��Ԃ�(Loop�@)
            '-------------------------------------------------------------------
            Do
                '@@@888 StopWatch.Restart()
                'Erase tDelayTrimNgCnt                                   ' �ިڲ���2�pNG�����p�z�� V1.23.0.0�E
                '----- ###142�� -----
                ' �u���b�N�P�ʂł�NG������s���Ȃ�NG����pNG��R��������������
                If (ngJudgeUnit = 0) Then                               ' NG���� = �u���b�N�P�� ?
                    '----- V1.23.0.0�E�� -----
                    ' ��f�B���C(�f�B���C�g�����Q)���͍ŏI�J�b�g���ɏ���������
                    If ((m_blnDelayCheck = False) Or ((m_blnDelayCheck = True) And (m_blnDelayLastCut = True))) Then
                        'V6.0.1.0�N��
                        'NG���ł̔���Ńu���b�N�P�ʂ��I������Ă���ꍇ
                        If giNGCountInPlate = 1 And ngJudgeUnit = 0 Then
                        Else
                            m_NG_RES_Count = 0                              ' NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��)������ 
                        End If
                        'V6.0.1.0�N��
                    End If
                    'm_NG_RES_Count = 0                                 ' NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��)������ 
                    '----- V1.23.0.0�E�� -----
                End If
                '----- ###142�� -----

                '---------------------------------------------------------------
                ' BP�I�t�Z�b�g�ʒu�ֈړ� 
                ' ###102(EX_START()�̌�Ɉړ�,BP�ړ�������50mm�Ŗ�6msec�������
                '        �X�e�[�W�̈ړ���̃|�[�Y�Ƒ��E����)
                '---------------------------------------------------------------
                'r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, 0, 0, 1)
                'If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                '    Return (r)
                'End If

#If cOFFLINEcDEBUG = 0 Then
                ' ����~���`�F�b�N
                r = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' ����~�����o ?
                    TrimmingBlockLoop = r
                    Exit Function
                End If
#End If
                'V1.23.0.0�E(�폜)

                ' �G�A�[���G���[���o ?
                If Form1.System1.Air_Valve_Check(gSysPrm) = False Then
                    GoTo STP_ERR_AIR
                End If

                '---------------------------------------------------------------------
                '�@�Ώۃu���b�N�̃X�e�[�W�ʒu���擾
                '---------------------------------------------------------------------
                r = GetTargetStagePos(currentPlateNo, currentBlockNo, nextStagePosX, nextStagePosY,
                                    dispCurPltNoX, dispCurPltNoY, retBlkNoX, retBlkNoY)
                ' �S�u���b�N�I�� ?
                If r = BLOCK_END Then                                   ' �u���b�N�I�� ?
                    '----- V1.23.0.0�E�� -----
                    ' ��f�B���C(�f�B���C�g�����Q)��
                    If (m_blnDelayCheck) Then                           ' ��f�B���C ? 
                        m_intDelayBlockIndex = m_intDelayBlockIndex + 1 ' ���݂̃u���b�N�ԍ�(�J�b�g�ԍ�)�X�V
                        If (m_intDelayBlockIndex >= intGetCutCnt) Then  ' �ŏI�J�b�g�Ȃ�
                            m_blnDelayLastCut = True                    ' �ŏI�J�b�g�t���OON
                        End If
                        If (m_intDelayBlockIndex <= intGetCutCnt) Then  ' �J�b�g�����u���b�N�ړ� ? 
                            currentBlockNo = 1                          ' ���ĂȂ��Ȃ�擪�u���b�N 
                            Continue Do                                 ' ����ēx���s����
                        Else
                            ' ��f�B���C�̑S�u���b�N�I����
                            m_blnDelayLastCut = True                    ' �ŏI�J�b�g�t���OON
                            m_intDelayBlockIndex = 1                    ' ���݂̃u���b�N�ԍ�(�J�b�g�ԍ�)������  
                            currentBlockNo = 0                          ' �u���b�N�ԍ�������
                            currentPlateNo = currentPlateNo + 1         ' �v���[�g�ԍ��X�V
                        End If
                    End If
                    '----- V1.23.0.0�E�� -----

                    ' �v���[�g�P�ʂ�NG����̏ꍇ�����Ŕ�������s����B
                    If (ngJudgeUnit <> 0) Then                          ' NG���� = �v���[�g�P�� ?
                        ' NG�� > NG��������{�����R��(�v���[�g�P�ʂł�NG����)
                        'If (ngCount > ngJudgeResCntInPlt) Then         ' ###142
                        '                        If (m_NG_RES_Count > ngJudgeResCntInPlt) Then   ' ###142
                        ''V6.1.1.0�J��                        If ((m_NG_RES_Count > ngJudgeResCntInPlt) And ngJudgeResCntInPlt <> 0) Or
                        If ((m_NG_RES_Count >= ngJudgeResCntInPlt) And ngJudgeResCntInPlt <> 0) Or
                             (((m_NG_RES_Count >= ngJudgeResCntInPlt) And ngJudgeResCntInPlt <> 0) And (giNGCountInPlate = 1)) Then
                            'V6.1.1.0�J��
                            bLoaderNg = True                            ' �g���~���ONG�t���OON(�u���b�N�P��/�v���[�g�P�ʂł�NG����NG��R���𒴂���)
                            '----- V1.18.0.0�B���폜(�����ł̃J�E���g��NG�u���b�N���ƂȂ�) -----
                            '''' CHIP�̂݁F�i090702�Fminato)
                            'If (gTkyKnd = KND_CHIP) Then
                            '   If gSysPrm.stDEV.rPrnOut_flg = True Then    ' ������� ? (ROHM�a�d�l)
                            '       stPRT_ROHM.Tol_NG_Sheet = stPRT_ROHM.Tol_NG_Sheet + 1 
                            '   End If
                            'End If
                            '----- V1.18.0.0�B�� -----
                            'V6.0.1.0�N��
                            If giNGCountInPlate = 1 Then
                                JudgePlateNGCount(ngJudgeResCntInPlt, m_NG_RES_Count)
                            End If
                            'V6.0.1.0�N��

                        End If
                        m_NG_RES_Count = 0                              ' NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��)������ ###142 
                    End If

                    ' �S�u���b�N�I���Ȃ�u���b�N�ԍ������������A���̃v���[�g��ǂݏo��
                    currentBlockNo = 1
                    currentPlateNo = currentPlateNo + 1
                    Continue Do

                    ' �v���[�g���u���b�N���I�� ?
                ElseIf r = PLATE_BLOCK_END Then
                    'V4.5.0.5�@��'V4.12.2.0�H�@'V6.0.5.0�C��
                    ''V4.12.2.2�F��                    ret1 = JudgePlateNGRate()
                    ret1 = JudgePlateNGRate(bFgAutoMode)
                    ''V4.12.2.2�F��
                    If (ret1 = cFRS_ERR_START) Then
                        ' ���b�g���s
                        ' ����:NG�r�o���ă��b�g�͑��s 
                        bLoaderNg = True                                ' �g���~���ONG�t���OON(�u���b�N�P��/�v���[�g�P�ʂł�NG����NG��R���𒴂���)
                    ElseIf (ret1 = cFRS_ERR_RST) Then
                        ' ���b�g���fRESET����
                        r = cFRS_ERR_RST
                        ' �ُ�
                        TrimmingBlockLoop = r
                        Exit Function
                    Else
                        '�`�F�b�N�Ȃ��ŉ������Ȃ��ꍇ 

                    End If
                    'V4.5.0.5�@��'V4.12.2.0�H�@'V6.0.5.0�C��
                    ' �v���[�g���u���b�N���I�������ꍇ�͎��ցB
                    Exit Do

                ElseIf r <> cFRS_NORMAL Then
                    ' �p�����[�^�G���[�ŕԂ��B
                    TrimmingBlockLoop = r
                    Exit Function
                End If

                ' �L�k�␳�p�p�����[�^�̐ݒ�
                GetShinsyukuData(retBlkNoX, retBlkNoY, nextStagePosX, nextStagePosY)

                '---------------------------------------------------------------------
                ' �\���p�e�|�W�V�����̔ԍ���ݒ�i�v���[�g/�X�e�[�W�O���[�v/�u���b�N�j
                '---------------------------------------------------------------------
                Dim bRet As Boolean
                bRet = GetDisplayPosInfo(retBlkNoX, retBlkNoY,
                                dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)

                '===============================================================
                '   ���O�\��������̐ݒ�(���o���̕\��)
                '   --- Plate X=xxx Y=xxx--- StageGroup X= xxx Y= xxx Block X=xxx Y=xxx
                '===============================================================
                '----- V1.23.0.0�E�� -----
                ' ��f�B���C(�f�B���C�g�����Q)���͍ŏI�J�b�g���ɕ\������
                If ((m_blnDelayCheck = False) Or ((m_blnDelayCheck = True) And (m_blnDelayLastCut = True))) Then
                    Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY,
                                    dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
                Else
                    ' ��f�B���C(�f�B���C�g�����Q)�̍ŏI�J�b�g�ȊO���̓u���b�N�ԍ��ƃJ�b�g�ԍ���\������

                    If (digH <> 0) Then                                         ' DigSW-H = �\���Ȃ��ȊO ? 
                        strMsg = "- Block X=" + dispCurBlkNoX.ToString("000") + " Y=" + dispCurBlkNoY.ToString("000") + " Cut =" + m_intDelayBlockIndex.ToString("0")
                        Call Form1.Z_PRINT(strMsg)                              ' ���O��ʂɕ\������ 
                    End If

                End If

                System.Windows.Forms.Application.DoEvents()             '###009

                'Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY, _
                '                dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
                '----- V1.23.0.0�E�� -----

                '---------------------------------------------------------------------
                ' �X�e�[�W�̈ړ�
                '   EX_SMOVE2�ł͂Ȃ��ASTART�ɂē��삳����B
                '   stgx,stgy�̓X�e�[�W�̃I�t�Z�b�g�ƕ␳�l�̉��Z
                '---------------------------------------------------------------------
                ' Z��STEP�ʒu�ֈړ�����(START()�̒��ł���Ă���̂ŏ���̂ݎ��s) '###045
                If (bFlgInit = True) Then
                    'bFlgInit = False                                   ' V2.0.0.0_29
                    SETZOFFPOS(-1) 'V6.0.2.0�@
                    'r = PROBOFF_EX(zStepPos)                           ' EX_START��ZOFF�ʒu��zStepPos�Ƃ���
                    r = PROBOFF_EX(zWaitPos)                            ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ��� ###058
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                        Return (r)
                    End If

                    ''V6.0.1.022�� Step&Repeat�̑��x��]������B 
                    SetXYStageSpeed(StageSpeed.StepRepeatSpeed)
                    ''V6.0.1.022��

                End If

                '@@@888 test ���Ԍv���p
                'StopWatch.Restart()

                ' �X�e�[�W�̈ړ�
                r = Form1.System1.EX_START(gSysPrm, stgx + nextStagePosX, stgy + nextStagePosY, 0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If

                '@@@888 test ���Ԍv���p
                'StopWatch.Stop()
                'sw_save(currentBlockNo) = StopWatch.Elapsed()

                '----- V2.0.0.0_29�� -----
                ' �X�e�b�v�����s�[�g����X,Y�����x��؂�ւ���(SL436S��)
                'V4.4.0.0�E
                'If (giMachineKd = MACHINE_KD_RS) And (bFlgInit = True) Then
                If (bFlgInit = True) Then
                    bFlgInit = False

                    If (giMachineKd = MACHINE_KD_RS) Then
                        'V4.1.0.0�A ---
                        ' �g���~���O���[�h��Step&Repeat�̂Ƃ��ɂ́AZ���𓮍삳���Ȃ�
                        'V5.0.0.6�@  If digL <> TRIM_MODE_STEP_AND_REPEAT Then
                        If digL <> TRIM_MODE_STPRPT Then
                            ' Z��STEP�ʒu�ֈړ�(SL436S���͂Q�i�K(STEP�ʒu��ON�ʒu)��ON����)
                            r = EX_ZMOVE(zStepPos, MOVE_ABSOLUTE)               ' Z��STEP�ʒu�ֈړ� V2.0.0.0�O
                            If (r <> cFRS_NORMAL) Then                          ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                                Return (r)
                            End If
                            Call System.Threading.Thread.Sleep(100)             ' Wait(msec)
                        End If
                        'V4.1.0.0�A ---

                        ''V6.0.1.022���W���ɂ��邽��SL436S�̏�������o��
                        '' �X�e�b�v�����s�[�g����X,Y�����x��؂�ւ���
                        'r = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, 1).FL, stPclAxisPrm(AXIS_X, 1).FH, stPclAxisPrm(AXIS_X, 1).DrvRat, stPclAxisPrm(AXIS_X, 1).Magnif)
                        ''V4.1.0.0�L �ʐM�G���[�ȊO�����Ȃ��̂ɂQ����ʂɌ���K�v�͂Ȃ�                   r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                        ''V4.1.0.0�L                   If (r <> cFRS_NORMAL) Then Return (r)
                        'r = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, 1).FL, stPclAxisPrm(AXIS_Y, 1).FH, stPclAxisPrm(AXIS_Y, 1).DrvRat, stPclAxisPrm(AXIS_Y, 1).Magnif)
                        'r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                        'If (r <> cFRS_NORMAL) Then Return (r)
                    End If
                    '----- V2.0.0.0_29�� -----
                End If

                '---------------------------------------------------------------
                '   �v���[�u�`�F�b�N����(x0,x1,x2���[�h) ���I�v�V�����@�\ V1.23.0.0�F
                '---------------------------------------------------------------
                r = gparModules.ProbeCheck(digL, m_PlateCounter, m_ChkCounter)
                If (r <> cFRS_NORMAL) Then                              ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)
                    Return (r)                                          ' �v���[�u�`�F�b�N�G���[�Ȃ�Return�l = Cancel(RESET�L�[����)�Ŗ߂� 
                End If

                '---------------------------------------------------------------
                '   BP�I�t�Z�b�g�ʒu�ֈړ� ###102
                '---------------------------------------------------------------
                r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, 0, 0, 1)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                    Return (r)
                End If

                '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
                ' �g���~���O�J�n�u���b�N�ԍ�XY���X�V����
                Call Form1.Set_StartBlkNum(dispCurBlkNoX, dispCurBlkNoY)
                '----- V4.11.0.0�D�� -----

                ''----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
                '' ������{�^�������`�F�b�N(�����^�]��)
                'If (gbChkSubstrateSet = True) And (bFgAutoMode = True) And (gMachineType = MACHINE_TYPE_436S) Then
                '    ' ���������
                '    r = SubstrateSet_Proc(Form1.System1)
                '    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                '        Return (r)
                '    End If
                'End If
                'If (gbChkSubstrateSet = True) Then
                '    ' ������{�^���̔w�i�F���D�F�ɂ���
                '    gbChkSubstrateSet = False                           ' ������t���OOFF
                '    Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
                'End If
                '----- V4.11.0.0�E�� -----

                '=======================================================
                ' HALT�����֕ύX
                '=======================================================
                '#4.12.2.0�E                r = HaltSwOnMove(currentPlateNo, currentBlockNo, bFgAutoMode)
                r = HaltSwOnMove(currentPlateNo, currentBlockNo, dispCurPltNoX,
                                 dispCurPltNoY, dispCurBlkNoX, dispCurBlkNoY, bFgAutoMode)  '#4.12.2.0�E
                ' ����/START�L�[/RESET�L�[�����ȊO�̓G���[���^�[������ '###032
                If (r <> cFRS_NORMAL) And (r <> cFRS_ERR_START) Then
                    If (r = cFRS_ERR_RST) Then
                        TrimmingBlockLoop = r
                        Exit Do
                    Else
                        Return (r)
                    End If
                End If

                '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
                ' �g���~���O�J�n�u���b�N�ԍ�XY���擾����(�蓮�^�]���̂ݗL��(�I�v�V����))
                Dim wkBlkX As Integer = dispCurBlkNoX
                Dim wkBlkY As Integer = dispCurBlkNoY
                Call Form1.Get_StartBlkNum(wkBlkX, wkBlkY)
                ' �u���b�N�ԍ����ς�������`�F�b�N����
                If (wkBlkX <> dispCurBlkNoX) Or (wkBlkY <> dispCurBlkNoY) Then
                    dispCurBlkNoX = wkBlkX
                    dispCurBlkNoY = wkBlkY
                    'V5.0.0.9�O                    currentBlockNo = dispCurBlkNoX * dispCurBlkNoY
                    currentBlockNo = GetProcessingOrder(dispCurBlkNoX, dispCurBlkNoY)   'V5.0.0.9�O
                    '�@�w��u���b�N�̃X�e�[�W�ʒu���擾
                    r = GetTargetStagePos(currentPlateNo, currentBlockNo, nextStagePosX, nextStagePosY,
                                        dispCurPltNoX, dispCurPltNoY, retBlkNoX, retBlkNoY)
                    ' �X�e�[�W�̈ړ�
                    r = Form1.System1.EX_START(gSysPrm, stgx + nextStagePosX, stgy + nextStagePosY, 0)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                        Return (r)
                    End If
                End If
                '----- V4.11.0.0�D�� -----

                '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
                '' ������{�^�������`�F�b�N(�����^�]��)
                'If (gbChkSubstrateSet = True) And (bFgAutoMode = True) And (gMachineType = MACHINE_TYPE_436S) Then
                '    ' ���������
                '    r = SubstrateSet_Proc(Form1.System1)
                '    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                '        Return (r)
                '    End If
                'End If
                'If (gbChkSubstrateSet = True) Then
                '    ' ������{�^���̔w�i�F���D�F�ɂ���
                '    gbChkSubstrateSet = False                           ' ������t���OOFF
                '    Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
                'End If
                '----- V4.11.0.0�E�� -----

                ' �f�W�X�C�b�`�̓ǎ��
                Call Form1.GetMoveMode(digL, digH, 0)
                '���ł���Ă���̂ł����ł͂��Ȃ�'V4.1.0.0�L��
                'If gKeiTyp = KEY_TYPE_RS Then                                   'V3.1.0.0�D �V���v���g���}�̏ꍇ
                '    '�^�C�~���O�ύX
                '    SimpleTrimmer.TrimData.SetBlockNumber(currentBlockNo)    'V2.0.0.0�I ���݂̃u���b�N�ԍ��̕ۑ�
                '    ''V4.0.0.0-82        ' 
                '    'BlockNextButton.Enabled = False
                '    'BlockRvsButton.Enabled = False
                '    'BlockMainButton.Enabled = False
                'End If
                '���ł���Ă���̂ł����ł͂��Ȃ�'V4.1.0.0�L��
                'V4.0.0.0-82    
                'If r = cFRS_ERR_RST Then'###032
                '    TrimmingBlockLoop = r
                '    Exit Do
                'ElseIf (r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Or (r = cFRS_ERR_LATCH) Or (r = cFRS_ERR_EMG) Then
                '    Return (r)
                'Else
                '    ' �f�W�X�C�b�`�̓ǎ��
                '    Call Form1.GetMoveMode(digL, digH, 0)
                'End If

                'V4.12.2.0�@             ��
                If (digL < 3) Then
                    'V6.0.1.3�@                    Form1.SetMapColor(currentBlockNo, TrimMap.ColorStart)       ' ���H���u���b�N�̔w�i�F�ݒ�
                    Form1.SetMapColor(currentPlateNo, currentBlockNo, TrimMap.ColorStart)   ' �v���[�g�Ή�    'V6.0.1.3�@
                Else
                    'V6.0.1.3�@                    Form1.SetMapBorder(currentBlockNo, Color.Black)
                    Form1.SetMapBorder(currentPlateNo, currentBlockNo, Color.Black)         ' �v���[�g�Ή�    'V6.0.1.3�@
                End If
                'V4.12.2.0�@             ��

                '=======================================================
                '   Z�v���[�u��ON�ʒu�Ɉړ�
                '=======================================================
                ' �޼�SW��1���ڂ�5�ȉ��ł��邩��������B(x0,x1,x2,x3,x4,x5)
                'If (gDigL <= 5) Then
                If (digL <= 3) Then
                    '���̌Ăѕ��������PROP_SET�Őݒ肵���v���[�u��ON/OFF�ʒu���N���A�����B
                    '�����s�������A��������OcxSystem��IF�g�p���~�߂ĉ��������B
                    'r = Form1.System1.EX_PROBON(gSysPrm)
                    'r = PROBON()
                    'r = EX_ZMOVE(typPlateInfo.dblZOffSet, MOVE_ABSOLUTE)

                    ' �o�C�A�XON(FL��x0,x1���[�h��)�@###043 ###017
                    'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And (digL <= 1) Then
                    ' �o�C�A�XON(CHIP����FL��x0,x1���[�h��)�@###043
                    If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And (gTkyKnd = KND_CHIP) And (digL <= 1) Then
                        '----- ###192�� -----
                        Call QRATE(stCND.Freq(MINCUR_CND_NUM))          ' Q���[�g�ݒ�(KHz)
                        r = FLSET(FLMD_CNDSET, MINCUR_CND_NUM)          ' ���H�����ԍ��ݒ�(�ŏ��d���l)
                        If (r <> cFRS_NORMAL) Then                      ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                            Return (r)
                        End If
                        '----- ###192�� -----
                        r = FLSET(FLMD_BIAS_ON, 0)                      ' �o�C�A�XON
                        If (r <> cFRS_NORMAL) Then                      ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                            Return (r)
                        End If
                    End If

                    ' V2.0.0.0�M��
                    glProbeRetryCount = typPlateInfo.intPrbRetryCount   ' �v���[�u���g���C��(0=���g���C�Ȃ�)
                    For i = 0 To (glProbeRetryCount)

                        '###018 r = EX_ZMOVE(zOnPos, MOVE_ABSOLUTE)
                        r = PROBON()                                        ' ###018
                        r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' ###140

                        ' 'V1.13.0.0�C  ��
                        ' Z2�����ړ�����B
                        If IsUnderProbe() Then
                            r = Z2move(Z2ON)                                ' Z2������w��
                            If (r <> cFRS_NORMAL) Then                      ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                                Return (r)
                            End If
                            Call ZSTOPSTS2()                                ' Z2�������~�҂�
                        End If
                        ' ''V1.13.0.0�C  ��
                        If i < (glProbeRetryCount) Then ' �ăv���[�r���O����Ƃ��ɂ͈�x�㏸������ 
                            ' 'V1.13.0.0�C  ��
                            ' Z2��DOWN�ʒu�Ɉړ�����
                            If IsUnderProbe() Then                          ' �����v���[�u�L��̏ꍇ
                                r = Z2move(Z2STEP)                          ' �y�Q���X�e�b�v�ړ����͉����v���[�u�X�e�b�v���~�����������~����B
                                If (r <> cFRS_NORMAL) Then                  ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                                    TrimmingBlockLoop = r
                                    Exit Do
                                End If
                                Call ZSTOPSTS2()
                            End If
                            ' 'V1.13.0.0�C  ��

                            ' Z��ï��&��߰Ĉʒu�Ɉړ�(�㏸�ʒu)
                            r = PROBOFF_EX(zStepPos)                        ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ��� ###058
                            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                            If (r <> cFRS_NORMAL) Then                      ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                                TrimmingBlockLoop = r
                                Exit Do
                            Else
                                'Call LAMP_CTRL(LAMP_Z, False)               ' Z�����vOFF
                            End If
                        End If

                    Next i
                    'V2.0.0.0�M��

                Else
                    '----- V3.0.0.1�@�� -----
                    ' x5���[�h�̏ꍇ��Z���w��ʒu�Ɉړ�����(0=OFF�ʒu, 1=STEP�ʒu, 2=ON�ʒu)�@���I�v�V����
                    If ((digL = 5) And (giFullCutZpos <> 0)) Then
                        If (giFullCutZpos = 1) Then                             ' Z�ʒu�w��=STEP�ʒu�w��
                            ' Z��STEP�ʒu�ֈړ�(Z�ʒu�w��=STEP�ʒu�̏ꍇ)
                            r = EX_ZMOVE(zStepPos, MOVE_ABSOLUTE)               ' �G���[�Ȃ烁�b�Z�[�W�\���ς�
                        Else                                                    ' Z�ʒu�w��=ON�ʒu�w��
                            ' Z��ON�ʒu�ֈړ�(Z�ʒu�w��=ON�ʒu�̏ꍇ)
                            r = PROBON()
                            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ烁�b�Z�[�W��\������
                        End If
                    Else
                        '----- 'V4.5.0.0�C�� -----
                        ' Z��ҋ@�ʒu�ֈړ�����
                        r = EX_ZMOVE(zWaitPos, MOVE_ABSOLUTE)
                        If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                            Return (r)                                              ' Return�l�ݒ�
                        End If
                        '----- 'V4.5.0.0�C�� -----
                        ' Z��ҋ@�ʒu�ֈړ�(x5���[�h�ȊO�܂���x5���[�h��Z�ʒu�w��Ȃ��̏ꍇ)
                        r = PROBOFF_EX(zWaitPos)                                ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ���
                        r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)       ' �G���[�Ȃ烁�b�Z�[�W��\������
                    End If

                    '' x4�ȏ�̏ꍇ��Z��ҋ@�ʒu�ֈړ�����
                    ''r = EX_ZMOVE(typPlateInfo.dblZWaitOffset, MOVE_ABSOLUTE)
                    'r = PROBOFF_EX(zWaitPos)                            ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ��� ###171
                    'r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ烁�b�Z�[�W��\������ ###212
                    '----- V3.0.0.1�@�� -----
                End If
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If
                If (digL <= 3) Then
                    Call LAMP_CTRL(LAMP_Z, True)                        ' Z�����vON 
                End If

                ''@@@888
                'StopWatch.Stop()
                'sw_save(currentBlockNo) = StopWatch.Elapsed()

                'V4.2.0.0�@
                If gKeiTyp = KEY_TYPE_RS Then                                   ' �V���v���g���}�̏ꍇ
                    SetNowBlockDspNum(currentBlockNo + 1)
                End If
                'V4.2.0.0�@

                ' V5.0.0.6�N ��
                ' �u���b�N���Ƀg���}��~������xON����OFF����B���[�_�ł̍H���ԃ^�C���A�E�g������邽�߂̈ꎞ�I�ȏ��� 
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R�n ? 
                    ' ���[�_�o��(ON=��v��(�A���^�]�J�n), OFF=�Ȃ�)(SL436R�p)
                    If (bFgAutoMode = True) Then                                ' ���[�_�������[�h? ###001(���L���o�͂���ƃN�����v�E�z��OFF����)
                        '                            Call SetLoaderIO(LOUT_STOP, &H0)                        ' ���[�_�o��(ON=�g���}��~��) 
                        '                            Sleep(20)
                        Call SetLoaderIO(&H0, LOUT_STOP)                        ' ���[�_�o��(OFF=�g���}���쒆) 
                    End If
                End If
                ' V5.0.0.6�N ��

                ' �޼�SW��1���ڂ�5�ȉ��ł��邩��������B(x0,x1,x2,x3,x4,x5)
                If (digL <= 5) Then
                    ' �g���~���O���ʎ擾�G���A������������B
                    Init_TrimResultData()

                    If IsCutPosCorrect() Then       'V3.0.0.0�B ADD �J�b�g�ʒu�␳�w�肪�����ꍇ�͏������Ȃ��B
                        ' TKY�ɂ����ẮA�J�b�g�ʒu�␳�����{����
#If False Then                          'V6.0.0.0�D
                        If (bIniFlg <> 0) Then
                            End_GazouProc(ObjGazou)
                        End If
#End If
                        Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(False)   'V6.0.0.0-26
                        r = DoCutPosCorrect(digL, gRegistorCnt, stCutPos)
                        Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(True)    'V6.0.0.0-26
#If False Then                          'V6.0.0.0�D
                        '---------------------------------------------------------------------------
                        '   �摜�\���v���O�������N������
                        '---------------------------------------------------------------------------
                        If (bIniFlg <> 0) And (Form1.chkDistributeOnOff.Checked = False) Then
                            ' ������ V3.1.0.0�A 2014/12/01
                            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                            r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                            ' ������ V3.1.0.0�A 2014/12/01
                        End If
#End If
                        ''V6.0.1.0�S��    
                    Else
                        DoCutPosCorrectClr(gRegistorCnt)
                        ''V6.0.1.0�S��
                    End If

                    '----- ###211�� -----
                    '-----------------------------------------------------------
                    ' �u���b�N�P�ʂ̃g���~���O�������̓|�W�V�����`�F�b�N����
                    '-----------------------------------------------------------
                    If gKeiTyp = KEY_TYPE_RS Then                                   'V3.1.0.0�D �V���v���g���}�̏ꍇ
                        SimpleTrimmer.BlockStart()                                  'V3.1.0.0�D �u���b�N�X�^�[�g������
                        SimpleTrimmer.TrimData.SetBlockNumber(currentBlockNo)       'V2.0.0.0�I ���݂̃u���b�N�ԍ��̕ۑ�
                        BlockNoLabel.Text = currentBlockNo                          'V4.0.0.0�L
                        'SetNowBlockDspNum(currentBlockNo + 1)                           'V4.1.0.0�Q'V4.2.0.0�@
                    End If                                                          'V3.1.0.0�D

                    If (digL <> 4) Then
                        ' �u���b�N�P�ʂ̃g���~���O����(x0,x1,x2,x3,x5���[�h��)
                        r = TrimBlockExe(digL, gSysPrm.stDEV.giPower_Cyc, m_intDelayBlockIndex, 0) ' V1.23.0.0�E
                    Else
                        ' �|�W�V�����`�F�b�N����(x4���[�h��)
                        r = TrimPositionCheck(digH, digL, bpOffX, bpOffY, blkSizeX, blkSizeY)
                        '----- ###232��(�\����DllTrimClassLiblary�ŕ\�������) -----
                        ' �␳��N���X���C��X,Y��\��
                        'V6.0.0.0�C                        Form1.CrosLineX.Visible = False
                        'V6.0.0.0�C                        Form1.CrosLineY.Visible = False
                        Form1.VideoLibrary1.SetCorrCrossVisible(False)          'V6.0.0.0�C
                        '----- ###232�� -----
                        If (r >= cFRS_NORMAL) Then                      ' RESET�L�[�����Ȃ�Return�l�𐳏�Ƃ���
                            r = cFRS_NORMAL
                        Else
                            Return (r)
                        End If
                    End If

                    '----- V4.0.0.0�R�� -----
                    ' �����^�]���̍ŏI�u���b�N��HALT SW���������ꂽ�ꍇ�́u�T�C�N����~�v������
                    ' �s�����߁u�T�C�N����~�t���O�v��ON����
                    'V5.0.0.4�@         If (gSysPrm.stCTM.giSPECIAL = customROHM) Then      ' ���[���a����(SL436R/SL436S) ?
                    If gbCycleStop Or (gSysPrm.stCTM.giSPECIAL = customROHM) Then      ' ���[���a����(SL436R/SL436S) ?
                        '#4.12.2.0�F                        If (bFgAutoMode = True) And (currentBlockNo >= (typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir)) Then
                        If (bFgAutoMode = True) AndAlso
                            (currentBlockNo >= (CInt(typPlateInfo.intBlockCntXDir) * CInt(typPlateInfo.intBlockCntYDir))) Then '#4.12.2.0�F 
                            Call HALT_SWCHECK(Sw)
                            If (Sw = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Or (gbHaltSW = True) Then
                                bFgCyclStp = True                       ' �T�C�N����~�t���OON
                                Call LAMP_CTRL(LAMP_HALT, True)             ' HALT�����vON
                            End If
                            gbHaltSW = False
                            Call ZCONRST()                              ' �R���\�[���L�[���b�`����
                        End If
                    End If
                End If
                '----- V4.0.0.0�R�� -----

                '' �u���b�N�P�ʂ̃g���~���O����(x0,x1,x2,x3,x5)
                'r = TrimBlockExe(digL, gSysPrm.stDEV.giPower_Cyc, 0, 0)
                '----- ###211�� -----
                If r <> cFRS_NORMAL Then
                    If r = cFRS_ERR_EMG Then            'V1.25.0.5�A
                        bEmergencyOccurs = True         'V1.25.0.5�A
                    End If                              'V1.25.0.5�A
                    If r = 1 Then
                        TrimmingBlockLoop = cGMODE_EMG
                        Exit Do
                    ElseIf r = 3 Then
                        Call LAMP_CTRL(LAMP_HALT, False)            ' HALT�����vOFF
                        Call LAMP_CTRL(LAMP_RESET, True)            ' RESET�����vON
                        Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMRES, "")
                        Call Form1.Z_PRINT("RESET SW")
                        TrimmingBlockLoop = cFRS_ERR_RST
                        Exit Do
                        '(2011/06/15)
                        '   ���L�̃G���[�R�[�h�͕Ԃ��Ă��Ȃ�()
                        '   �K�v�Ȃ��INTRIM�����C��
                        'ElseIf r = 4 Then
                        '    Call Form1.Z_PRINT("HOST ERR")
                        '    GoTo EXIT_LOOP
                    Else
                        ' FL�G���[�Ȃ�G���[����INtime���ɑ��M����(���O�o�͗p)
                        If ((Math.Abs(r) = ERR_LSR_STATUS_OSCERR) Or (Math.Abs(r) = ERR_LSR_STATUS_OSCERR)) Then
                            FlErrCode = 0
                            Call ReceiveErrInfo(FlErrCode)          ' FL������G���[������M����
                            If (FlErrCode <> 0) Then
                                Call SET_FL_ERRLOG(FlErrCode)       ' �G���[����INtime���ɑ��M����
                            End If
                        End If
                        '----- V1.13.0.0�J�� -----
                        ' ����΂�����o/�I�[�o���[�h���o�`�F�b�N
                        r = CV_OverLoadErrorCheck(r)
                        If (r = cFRS_ERR_RST) Then                  ' ����΂�����o/�I�[�o���[�h���o�ŏ����Ő؎w��
                            TrimmingBlockLoop = cFRS_ERR_RST
                            Exit Do
                        ElseIf (r = cFRS_ERR_START) Then            ' ����΂�����o/�I�[�o���[�h���o�ŏ������s�w��
                            GoTo STP_NEXT
                        End If
                        '----- V1.13.0.0�J�� -----

                        TrimmingBlockLoop = Math.Abs(r) * -1        ' Return�l���|�ϊ� 
                        Exit Do
                    End If
                End If

STP_NEXT:       '                                                   ' V1.13.0.0�J
                '---------------------------------------------------------------
                '   Z���X�e�b�v&���s�[�g�ʒu�Ɉړ�����
                '---------------------------------------------------------------
                ' �v���[�g�̃`�F�b�N�E�\���X�V
                'blnCheckPlate = CheckPlate(blnCheckPlate, digL)    ' ###093

                '----- V3.0.0.1�@�� -----
                'If (digL <= 3) Then
                ' x0,x1,x2,x3���[�h�����x5���[�h��Z�̈ʒu�w��L��(�I�v�V����)�̏ꍇ
                If ((digL <= 3) Or ((digL = 5) And (giFullCutZpos <> 0))) Then
                    '----- V3.0.0.1�@�� -----
                    ' 'V1.13.0.0�C  ��
                    ' Z2��DOWN�ʒu�Ɉړ�����
                    If IsUnderProbe() Then                          ' �����v���[�u�L��̏ꍇ
                        r = Z2move(Z2STEP)                          ' �y�Q���X�e�b�v�ړ����͉����v���[�u�X�e�b�v���~�����������~����B
                        If (r <> cFRS_NORMAL) Then                  ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                            TrimmingBlockLoop = r
                            Exit Do
                        End If
                        Call ZSTOPSTS2()
                    End If
                    ' 'V1.13.0.0�C  ��

                    ' Z��ï��&��߰Ĉʒu�Ɉړ�(�㏸�ʒu)
                    r = PROBOFF_EX(zStepPos)                        ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ��� ###058
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    'r = EX_ZMOVE(zStepPos, MOVE_ABSOLUTE)          ' ###058
                    If (r <> cFRS_NORMAL) Then                      ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                        TrimmingBlockLoop = r
                        Exit Do
                    Else
                        Call LAMP_CTRL(LAMP_Z, False)               ' Z�����vOFF
                    End If
                End If

                ' ����~���݂̊m�F
                If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

                '---------------------------------------------------------------
                '   �g���~���O���ʂ̕\��/���O�t�@�C���̏o�͂��s��
                '---------------------------------------------------------------
                '----- V1.23.0.0�E�� -----
STP_LOG_OUT:
                ' ��f�B���C(�f�B���C�g�����Q)��
                If (m_blnDelayCheck) Then                               ' ��f�B���C ?
                    If (digL < 2) Then                                  ' x0,x1���[�h�� 
                        ' �g���~���O���ʂ��擾���� ��gwTrimResult()
                        Call TrimLoggingResult_Get(RSLTTYP_TRIMJUDGE, 0)

                        ' �C�j�V��������l���擾����(��P�J�b�g��)
                        If (m_intDelayBlockIndex = 1) Then              ' ��P�J�b�g ?
                            ' NG�����p�\���̂̏�����
                            Call SetDefaultData(dispCurBlkNoX, dispCurBlkNoY)
                            ' �C�j�V��������l���擾���遨gfInitialTest()
                            Call TrimLoggingResult_Get(RSLTTYP_INTIAL_TEST, 0)
                            ' �C�j�V��������l�ƌ��ʂ�stDelay2.NgAry()�ɕێ����Ă���
                            Call TrimLogging_NgCont_CHIP(digL, dispCurBlkNoX, dispCurBlkNoY)
                        End If

                        ' �ŏI�u���b�N�ȊO�̓g���~���O���ʂ̕\��/���O�t�@�C���̏o�͍͂s��Ȃ�
                        If (m_blnDelayLastCut = False) Then             ' �ŏI�u���b�N�ȊO�Ȃ�
                            GoTo STP_NEXT_BLOCK                         ' �g���~���O���ʂ̕\��/���O�t�@�C���o�͂��Ȃ��Ŏ��u���b�N��
                        End If
                    Else
                        m_blnDelayCheck = False                         ' x3�ȏ�Ȃ��f�B���C�͎~�߂�
                    End If
                End If
                '----- V1.23.0.0�E�� -----

                m_intNgCount = ngCount
                ' �\�����O�̍X�V/�o�̓��O�̃f�[�^�ۑ����s�Ȃ��B
                Call TrimLogging_Main(digH, digL, r, dispCurPltNoX, dispCurPltNoY, dispCurBlkNoX, dispCurBlkNoY,
                                   strLogDataBuffer, contNgCountError)


                'V2.0.0.0�I ADD START ��R�f�[�^�̕\��
                If gKeiTyp = KEY_TYPE_RS Then
                    m_blnElapsedTime = True                             ' �o�ߎ��Ԃ�\������ V4.11.0.0�C
                    Call SimpleTrimmer.ResistorDataDisp(True, 0, 1)
                    SimpleTrimmer.ElapsedTimeUpdate()       'V2.0.0.0�I �o�ߎ��Ԃ̍X�V(�P�u���b�N�I����)
                End If
                'V2.0.0.0�I ADD END ��R�f�[�^�̕\��

                '----- V6.0.3.0�F�� (�J�b�g�I�t�����@�\) -----
                ' �����u���b�N�����O�łȂ��Ƃ��Ɏ��s
                If gAdjustCutoffFunction = 1 And (giMachineKd = MACHINE_KD_RS) Then
                    If digL <= TRIM_MODE_TRFT Then
                        If gAdjustCutoffCount = 0 Then
                            '�ŏ��̂P�u���b�N�ڂ͖ڕW�l�̕ۑ��̂�
                            InitCalCutoff()
                        End If
                        If (stCutOffAdjust.dblAdjustCutOff_Exec <> AdjustStat.ADJUST_ALREADY) And (stCutOffAdjust.dblAdjustCutOff_Exec <> AdjustStat.ADJUST_FINISHED) Then
                            If 0 < typPlateInfo.intAdjustBlockCnt Then
                                '�����J�E���^�������u���b�N�ȉ��̏ꍇ���s
                                If gAdjustCutoffCount < typPlateInfo.intAdjustBlockCnt Then
                                    If stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC Then
                                        ' �J�b�g�I�t�̌v�Z
                                        CalcCutOffVal()
                                    End If

                                    gAdjustCutoffCount = gAdjustCutoffCount + 1
                                Else
                                    '----- V6.0.3.0�S�� -----
                                    ' �����񐔂��I�[�o�[�������A�������ł��Ă��Ȃ��ꍇ
                                    If (stCutOffAdjust.dblAdjustCutOff_Exec <> AdjustStat.ADJUST_ALREADY) Then
                                        ' ��ʕ\�����čēx���s���A���b�g�I������I��������B
                                        r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_CUTOFF_TURNING) ' START/RESET�L�[�����҂���ʕ\��
                                        If r = cFRS_ERR_START Then
                                            ' �ēx�J�b�g�I�t�������J��Ԃ� 
                                            gAdjustCutoffCount = 0
                                        Else
                                            ' ���b�g�I�� 
                                            TrimmingBlockLoop = r
                                            Exit Do
                                        End If
                                    End If
                                    '----- V6.0.3.0�S�� -----
                                End If
                            End If
                        End If
                    End If
                End If
                '----- V6.0.3.0�F�� -----

                '----- V1.22.0.0�C�� -----
                ' �T�}���[���M���O�p�f�[�^�ݒ菈��(�t�@�C���o�͂͂����ł͂��Ȃ�)
                Call SummaryLoggingDataSet(stSummaryLog, digH, digL)
                '----- V1.22.0.0�C�� -----

                'V1.14.0.0�@
                If (gESLog_flg = True) Then     'V1.14.0.0�@
                    Call TrimLoggingResult_ES()
                    Call LoggingWrite_ES()
                End If

                '----- V1.18.0.0�E�� -----
                ' IX2���O���o�͂���(�I�v�V����)
                If (digL <= 1) Then                                     ' x0.x1���[�h ? 
                    Call TrimLoggingResult_Index2()                     ' ���茋�ʂ̎擾 
                    Call LoggingWrite_Index2()                          ' IX2���O�o��
                End If
                '----- V1.18.0.0�E�� -----

                ''�\���e�L�X�g�{�b�N�X���ŉ��w�Ɉړ�
                'Form1.txtLog.ScrollToCaret()

                '-------------------------------------------------------
                '   �A��NG-HIGH�װ����������(CHIP/NET��)
                '-------------------------------------------------------
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then
                    If (contNgCountError = CONTINUES_NG_HI) Then        ' �A��NG-HIGH�װ���� ?
                        ' �A��HI-NG�G���[���b�Z�[�W�\��(SL432R/SL436R����)
                        r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_HING) ' START�L�[�����҂���ʕ\��

                        ' �A��HI-NG�����ޯ̧������ ###181
                        Call ClearNgHiCount()
                        contNgCountError = 0

                        If (r) Then r = cFRS_ERR_RST '                  ' Return�l = �A��NG-HIGH��G���[�Ȃ�Cancel(RESET��)�ŕԂ� ###129
                        TrimmingBlockLoop = r                           ' Return�l = ����~��  
                        Exit Do
                    End If
                End If

                'V4.9.0.0�@�� 'High,Low�̗��������Ȃ����ꍇ�̒�~��ʗp
                'V4.10.0.0�I                If gKeiTyp = KEY_TYPE_RS Then                                   ' �V���v���g���}�̏ꍇ
                If (gMachineType = MACHINE_TYPE_436S) Then                      ' �V���v���g���}�̏ꍇ
                    'V4.9.0.0�@
                    If giNgStop = 1 Then
                        If (bFgAutoMode = True) And (JudgeNgRate.CheckTimmingBlock = True) Then
                            r = sub_JudgeLotStop()
                            If r = cFRS_ERR_RST Then
                                ' �����͉�ʂŒ��f�������ꂽ�Ƃ��̂�
                                r = cFRS_NORMAL '                  ' Return�l = �A��NG-HIGH��G���[�Ȃ�Cancel(RESET��)�ŕԂ� ###129
                                TrimmingBlockLoop = r                           ' Return�l = ����~��  
                                bLoaderNg = True
                                Exit Do
                                'V4.11.0.0�G��
                            ElseIf r = cFRS_ERR_LDRTO Then
                                TrimmingBlockLoop = r                           ' Return�l = ����~��  
                                Exit Do
                            ElseIf r = cFRS_ERR_LDR Then
                                TrimmingBlockLoop = r                           ' Return�l = ����~��  
                                Exit Do
                                'V4.11.0.0�G��
                            End If
                        End If
                    End If
                End If
                'V4.9.0.0�@��

                ngCount = m_intNgCount

                'Call Form1.System1.D_DREAD2_EX(iDummy, gDigH, iDummy, gDigL, gDigH)
                Call Form1.GetMoveMode(iDummy, digH, iDummy)
                lOkChip = m_lGoodCount                                   ' OK��
                lNgChip = m_lNgCount                                     ' NG��

                '-------------------------------------------------------
                '   ���z�}�\������(CHIP/NET��)
                '-------------------------------------------------------
                'If (gTkyKnd = KND_CHIP) Then
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then     ' ###139
                    'V4.0.0.0�K��
                    If gKeiTyp <> KEY_TYPE_RS Then
                        Call DisplayDistribute()
                    Else
                        gObjFrmDistribute.RedrawGraph()
                    End If
                End If

                ' �u���b�N�P�ʂł�NG������s
                If (ngJudgeUnit = 0) Then                               ' NG���� = �u���b�N�P�� ?
                    ' NG�� > NG��������{�����R��(�u���b�N�P�ʂł�NG����)
                    'If (ngCount > ngJudgeResCntInBlk) Then             ' ###142
                    '�C�R�[�����Ȃ��ƂP�O�O���̂Ƃ�����Ȃ� If (m_NG_RES_Count > ngJudgeResCntInBlk) Then       ' ###142
                    ''V4.1.0.0�O                    If (m_NG_RES_Count >= ngJudgeResCntInBlk) Then       ' ###142�@'V4.0.0.0-64
                    If ((m_NG_RES_Count >= ngJudgeResCntInBlk) And (ngJudgeResCntInBlk <> 0)) Then  'V4.1.0.0�O     ' ###142�@'V4.0.0.0-64
                        bLoaderNg = True                                ' �g���~���ONG�t���OON(�u���b�N�P��/�v���[�g�P�ʂł�NG����NG��R���𒴂���)
                        'V6.0.1.0�N��
                        If giNGCountInPlate = 1 Then
                            r = JudgePlateNGCount(ngJudgeResCntInBlk, m_NG_RES_Count)
                            r = cFRS_ERR_RST '                             ' Return�l = NG���ɂ���~�ł̒��f������
                            TrimmingBlockLoop = r                           ' Return�l = ����~��  
                            Exit Do
                        End If
                        'V6.0.1.0�N��

                        ' ���b�g���̕s�Ǌ���J�E���g(CHIP�̂�)
                        '----- V1.18.0.0�B�� �폜 -----
                        'If (gTkyKnd = KND_CHIP) Then
                        '   If gSysPrm.stDEV.rPrnOut_flg = True Then    ' ������� ? (ROHM�a�d�l)
                        '       stPRT_ROHM.Tol_NG_Sheet = stPRT_ROHM.Tol_NG_Sheet + 1 
                        '   End If
                        'End If
                        '----- V1.18.0.0�B�� -----
                    End If
                End If

STP_NEXT_BLOCK:  '                                                       ' V1.23.0.0�E

                ' V5.0.0.6�N ��
                ' �u���b�N���Ƀg���}��~������xON����OFF����B���[�_�ł̍H���ԃ^�C���A�E�g������邽�߂̈ꎞ�I�ȏ��� 
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R�n ? 
                    ' ���[�_�o��(ON=��v��(�A���^�]�J�n), OFF=�Ȃ�)(SL436R�p)
                    If (bFgAutoMode = True) Then                                ' ���[�_�������[�h? ###001(���L���o�͂���ƃN�����v�E�z��OFF����)
                        Call SetLoaderIO(LOUT_STOP, &H0)                        ' ���[�_�o��(ON=�g���}��~��) 
                        '                        Sleep(20)
                        '                        Call SetLoaderIO(&H0, LOUT_STOP)                        ' ���[�_�o��(OFF=�g���}���쒆) 
                    End If
                End If
                ' V5.0.0.6�N ��

                'StopWatch.Stop()
                'sw_save(currentBlockNo) = StopWatch.Elapsed()

                ' ���u���b�N�̎��s
                currentBlockNo = currentBlockNo + 1
                System.Windows.Forms.Application.DoEvents()             '###009

            Loop

            '���Ԍv���p @@@888
            'For i = 1 To typPlateInfo.intBlockCntXDir
            '    strStopwatch = CStr(sw_save(i).Milliseconds) + vbCrLf
            '    MakeTmpLogFile("C:\TRIMDATA\LOG\timeCheck.log", strStopwatch)
            'Next

            ''V1.13.0.0�C  ��
            ' Z2��ҋ@�ʒu�Ɉړ�
            If IsUnderProbe() Then                                      ' �����v���[�u�L��̏ꍇ
                r = Z2move(Z2OFF)                                       ' Z2��DOWN�ʒu�Ɉړ�
                If (r <> cFRS_NORMAL) Then                              ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    Return (r)
                End If
                Call ZSTOPSTS2()
            End If
            ''V1.13.0.0�C  ��

            '---- ###261�� -----
            ' Z��ҋ@�ʒu�Ɉړ�
            r = PROBOFF_EX(zWaitPos)                                    ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ��� 
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            Else
                ' Z�����v�̏���
                Call LAMP_CTRL(LAMP_Z, False)
            End If

            ''If (r <> cFRS_NORMAL) Then
            '' Z��ҋ@�ʒu�Ɉړ�
            ''r = EX_ZMOVE(typPlateInfo.dblZWaitOffset, MOVE_ABSOLUTE)
            'r = EX_ZMOVE(zWaitPos, MOVE_ABSOLUTE)
            'If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? (���b�Z�[�W�͕\���ς�)
            '    Return (r)
            'Else
            '    ' Z�����v�̏���
            '    Call LAMP_CTRL(LAMP_Z, False)
            'End If
            '---- ###261��----

            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �S�u���b�N�I���̓g���~���O�J�n�u���b�N�ԍ�XY��1,1�Ƃ���
            'V5.0.0.9�O            If (dispCurBlkNoX >= typPlateInfo.intBlockCntXDir) And (dispCurBlkNoY >= typPlateInfo.intBlockCntYDir) Then
            If (1 = currentBlockNo) Then 'V5.0.0.9�O
                Call Form1.Set_StartBlkComb1St()
            End If
            '----- V4.11.0.0�D�� -----

            Exit Function
            'End If

            '---------------------------------
            ' BP�I�t�Z�b�g�ʒu�ֈړ�
            '---------------------------------
            'r = Form1.System1.BPMOVE(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
            '                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
            '                0, 0, 1)
            r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, 0, 0, 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                Return (r)
            End If

            ' �޼�SW��1���ڂ�3�ȉ��ł��邩��������B(x0,x1,x2,x3)
            'blnCheckPlate = CheckPlate(blnCheckPlate, gDigL) ###093

            Exit Function

            '---------------------------------------------------------------------
            '�@�G���[���o��
            '---------------------------------------------------------------------
            ' ����~���o�� 
STP_EMERGENCY:
            r = Sub_CallFrmRset(Form1.System1, cGMODE_EMG)              ' ����~���b�Z�[�W�\��
            Return (cFRS_ERR_EMG)

            ' �G�A�[���G���[���o�� 
STP_ERR_AIR:
            r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_AIR)          ' �G�A�[���G���[���o���b�Z�[�W�\��
            Return (r)                                                  ' Return�l = �G�A�[���G���[���o��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "basTrimming.TrimmingBlockLoop() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���Ԗڂɉ��H����u���b�N�ł��邩���擾����"
    ''' <summary>���Ԗڂɉ��H����u���b�N�ł��邩���擾����</summary>
    ''' <param name="blockPosX">�u���b�N�ʒu�w</param>
    ''' <param name="blockPosY">�u���b�N�ʒu�x</param>
    ''' <returns>���H���鏇��</returns>
    ''' <remarks>'V5.0.0.9�O</remarks>
    Friend Function GetProcessingOrder(ByVal blockPosX As Integer, ByVal blockPosY As Integer) As Integer
        Dim ret As Integer

        Select Case (giStartBlkAss)
            Case 1                      ' ����Walsin�d�l
                ret = blockPosX * blockPosY

            Case 2                      ' 2016�����J�� �ĊJ�u���b�N�ʒu�w��
                ' �X�e�b�v&(���s�[�g����)
                With typPlateInfo
                    Select Case .intDirStepRepeat
                        Case STEP_RPT_Y, STEP_RPT_CHIPXSTPY, STEP_RPT_NON
                            If (0 = (blockPosX Mod 2)) Then
                                ret = (.intBlockCntYDir * (blockPosX - 1)) + (.intBlockCntYDir - blockPosY + 1)
                            Else
                                ret = (.intBlockCntYDir * blockPosX) - (.intBlockCntYDir - blockPosY)
                            End If

                        Case STEP_RPT_X, STEP_RPT_CHIPYSTPX
                            If (0 = (blockPosY Mod 2)) Then
                                ret = (.intBlockCntXDir * (blockPosY - 1)) + (.intBlockCntXDir - blockPosX + 1)
                            Else
                                ret = (.intBlockCntXDir * blockPosY) - (.intBlockCntXDir - blockPosX)
                            End If

                        Case Else ' �ʂ�Ȃ�
                            ret = 1
                    End Select
                End With

            Case Else                   ' �ĊJ�u���b�N�ʒu�w��@�\�Ȃ��A�擪�u���b�N����J�n����
                ret = 1
        End Select

        Return ret

    End Function
#End Region

    '----- ###211�� -----
#Region "�|�W�V�����`�F�b�N����(x4)"
    '''=========================================================================
    ''' <summary>�|�W�V�����`�F�b�N����(x4)</summary>
    ''' <param name="digH">    (INP)DigSW High</param>
    ''' <param name="digL">    (INP)DigSW Low</param>
    ''' <param name="bpOffX">  (INP)BP�I�t�Z�b�gX</param>
    ''' <param name="bpOffY">  (INP)BP�I�t�Z�b�gY</param>
    ''' <param name="blkSizeX">(INP)�u���b�N�T�C�YX</param>
    ''' <param name="blkSizeY">(INP)�u���b�N�T�C�YY</param>
    ''' <returns>cFRS_NORMAL    = ����
    '''          cFRS_ERR_RST   = RESET�L�[����
    '''          ��L�ȊO=�G���[
    ''' </returns>
    ''' <remarks>�G���[�������̃��b�Z�[�W�͕\���ς�</remarks>
    '''=========================================================================
    Public Function TrimPositionCheck(ByVal digH As Integer, ByVal digL As Integer, ByVal bpOffX As Double, ByVal bpOffY As Double, ByVal blkSizeX As Double, ByVal blkSizeY As Double) As Integer

        Dim Rn As Integer = 0
        Dim Cn As Integer = 1                                           ' ���J�b�g 
        Dim Rtn As Integer = cFRS_NORMAL
        Dim r As Integer
        Dim PosX As Double = 0.0
        Dim PosY As Double = 0.0
        Dim strMSG As String
        Dim bZ As Boolean = False                                       ' ###220 
        Dim xPos As Double = 0.0                                        ' ###232
        Dim yPos As Double = 0.0                                        ' ###232

        Try
            ' ��������
            Call LAMP_CTRL(LAMP_START, True)                            ' START�����vON 
            Call LAMP_CTRL(LAMP_RESET, True)                            ' RESET�����vON 
            Call LAMP_CTRL(LAMP_Z, False)                               ' ###220 

            ' �摜�\���v���O�������I������ '###232
            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)

            ' BP�̃|�W�V�����`�F�b�N���s��
            For Rn = 1 To gRegistorCnt                                  ' ��R�����J��Ԃ� 
                ' ��R�̑��J�b�g�ʒuXY���擾����
                PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                'V5.0.0.6�I���O���J�����J�b�g�ʒu�␳�ʂ̉��Z
                If giTeachpointUse = 1 Then
                    If (Not GetCutStartPointAddTeachPoint(Rn, Cn, PosX, PosY)) Then
                        PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                        PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                        Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + Rn.ToString() + "] CUT=[" + Cn.ToString() + "]")
                    End If
                End If
                'V5.0.0.6�I��
                '----- ###245 -----
                ' �p�^�[���F�����ʂ�OK�Ȃ炸��ʂ����Z����
                If (gStCutPosCorrData.corrResult(Rn - 1) = 1) Then
                    PosX = PosX + gStCutPosCorrData.corrPosX(Rn - 1)
                    PosY = PosY + gStCutPosCorrData.corrPosY(Rn - 1)
                End If
                '----- ###245 -----

                ' BP���R�̑��J�b�g�ʒu�Ɉړ�����(��Βl�ړ�)
                r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, PosX, PosY, 1)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                    Rtn = r
                    Exit For
                End If

                '----- ###232�� -----
                ' �␳�N���X���C����\������
                Call ZGETBPPOS(xPos, yPos)
                ObjCrossLine.CrossLineDispXY(xPos, yPos)
                '----- ###232�� -----

                ' ���O�G���A�Ƀ��b�Z�[�W�\��
                If (digH = 2) Then                                      ' DigSW High = �S�ĕ\�� ? 
                    strMSG = "[Position Check Rn=" + typResistorInfoArray(Rn).intResNo.ToString("0") + "] START SW:NEXT, RESET SW:CANCEL"
                    Call Form1.Z_PRINT(strMSG)
                End If

STP_KEY_WAIT:   '                                                       ' ###220 
                ' START/RESET�L�[�����҂�(Z�L�[�����`�F�b�N����)
                r = WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST, True)
                If (r = cFRS_ERR_RST) Then                              ' RESET�L�[�����Ȃ�Return
                    Rtn = cFRS_ERR_RST
                    Exit For
                End If
                If (r < cFRS_NORMAL) Then                               ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                    Rtn = r
                    Exit For
                End If
                '----- ###220�� -----
                If (r = cFRS_ERR_Z) Then                                ' Z�L�[���� ?
                    If (bZ = False) Then                                ' Z OFF�Ȃ�Z ON
                        r = PROBON()
                    Else                                                ' Z ON�Ȃ� Z OFF
                        r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)     ' EX_START��ZOFF�ʒu��zWaitPos�Ƃ���
                    End If
                    ' �G���[�Ȃ烁�b�Z�[�W��\�����ăG���[���^�[��
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ烁�b�Z�[�W��\������
                    If (r <> cFRS_NORMAL) Then
                        Return (r)                                      ' �G���[���^�[�� 
                    End If

                    ' Z�����v�̓_��/����
                    If (bZ = False) Then
                        Call LAMP_CTRL(LAMP_Z, True)
                        bZ = True
                    Else
                        Call LAMP_CTRL(LAMP_Z, False)
                        bZ = False
                    End If
                    GoTo STP_KEY_WAIT                                   ' START/RESET�L�[�����҂��� 
                End If
                '----- ###220�� -----
            Next Rn                                                     ' START�L�[�����Ȃ玟��R�� 
#If False Then                          'V6.0.0.0�D
            '----- ###232�� -----
            ' �摜�\���v���O�������N������
            If gKeiTyp = KEY_TYPE_RS Then
                ' ������ V3.1.0.0�A 2014/12/01
                'r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 1)
                ' ������ V3.1.0.0�A 2014/12/01
            Else

                If (Form1.chkDistributeOnOff.Checked = False) Then          ' ���v��ʔ�\�����ɋN������
                    ' ������ V3.1.0.0�A 2014/12/01
                    'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                    r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                    ' ������ V3.1.0.0�A 2014/12/01
                End If
                '----- ###232�� -----
            End If
#End If
            Return (Rtn)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.TrimPositionCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###211�� -----
#Region "�g���~���O����"
    '''=========================================================================
    '''<summary>�g���~���O����</summary>
    '''<remarks>�u���b�N�P�ʂ̏�����INTRTM���ŏ��������B
    '''         VB�n�̃��[�v�ł́A
    '''         �v���[�g�A�O���[�v�A�u���b�N�̃��[�v���������s�����B
    '''         ���s����郋�[�v�́ANET,CHIP,TKY�ňقȂ�B  </remarks>
    '''<returns>cFRS_NORMAL = ����, ���L�ȊO = �G���[ </returns> 
    '''=========================================================================
    Public Function Trimming() As Short
        '#4.12.2.0�C        Dim strLogDataBuffer As String
        Dim strLogDataBuffer As StringBuilder               '#4.12.2.0�C
        Dim stgx As Double
        Dim stgy As Double
        Dim r As Integer
        Dim lRet As Long                                                  ' 'V2.0.0.0�I Return�l
        Dim rtnCode As Short = cFRS_NORMAL                              ' Return�l 
        Dim zStepPos As Double                                          ' Z���ï��&��߰Ĉʒu
        Dim strLOG As String                                            ' ���O�\��������
        Dim bLoaderNG As Boolean                                        ' �g���~���ONG�t���O(�u���b�N�P��/�v���[�g�P�ʂł�NG����NG��R���𒴂���)

        '----- CHIP�Ŏg�p -----
        Dim blnRotTheta As Boolean                                      ' ۯĖ��̃ƕ␳�L��(True=����, False=�Ȃ�)

        ' ���ʂŖ��O���Đݒ�
        Dim plate(2) As Short                                           ' �v���[�g
        Dim block(2) As Short                                           ' �u���b�N

        '----- �����^�]�p(SL436R�p) -----
        'Dim bFgAutoMode As Boolean = False                              ' ���[�_�������[�h�t���O ###107
        Dim bFgAutoMode_BK As Boolean                                   ' ���[�_�������[�h�t���O(�ޔ�)
        'Dim bIniFlg As Integer = 0                                     ' �����t���O(0=����, 1= �g���~���O��, 2=�I��) ###156
        Dim bFgLot As Boolean = False                                   ' ���b�g�ؑւ��v���t���O
        Dim bFgMagagin As Boolean = False                               ' �}�K�W���I���t���O�
        Dim bFgAllMagagin As Boolean = False                            ' �S�}�K�W���I���t���O�
        Dim FileNum As Integer                                          ' �t�@�C���J�E���^ 
        Dim strFNAM As String = ""                                      ' �������̃t�@�C����
        Dim SBLoader As Short                                           ' �������̊�i��ԍ�(-1�����l) ###089

        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        Dim strMSG As String
        Dim InterlockSts As Integer                                     ' ###006
        Dim Sw As Long                                                  ' ###006
        Dim blnCheckPlate As Boolean = False                            ' ###093
        Dim tmpLogFileName As String = "tmpLogFilename.Log"
        '----- V2.0.0.0_29�� -----
        Dim i As Integer
        Dim Axis As Integer
        Dim strSECT As String
        Dim strKEY As String
        '----- V2.0.0.0_29�� -----
        Dim TPInit As TimeSpan = New TimeSpan(0, 0, 0, 0)               ' V4.11.0.0�C
        Dim WkBlkX As Integer = 1                                       ' V4.11.0.0�D
        Dim WkBlkY As Integer = 1                                       ' V4.11.0.0�D

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            bFgAutoMode = False                                         ' V1.24.0.0�B
            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) -----
            StPauseTime.PauseTime = TPInit                              ' �ꎞ��~���ԏ����� 
            StPauseTime.TotalTime = TPInit
            ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��(�I�v�V����)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C�� -----
            Call SetTrimStartTime()                                     ' V1.18.0.0�B �g���~���O�J�n���Ԃ�ݒ肷��(���[���a����)
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ���񊈐�������
            Call Form1.Set_StartBlkNum_Enabled(False)
            '----- V4.11.0.0�D�� -----

            '----- V2.0.0.0_29�� -----
            'V4.10.0.0�I            If (gKeiTyp = KEY_TYPE_RS) Then
            'V6.0.1.022            If (gMachineType = MACHINE_TYPE_436S) Then
            ' X,Y��PCL�p�����[�^(SL436S�p)���V�X�p�����ݒ肷�遨���x�؂�ւ���W���Ƃ��� 
            For Axis = 0 To 1
                strSECT = "PCL_PRM_" + Axis.ToString("0")
                For i = 0 To 1
                    strKEY = "FL_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).FL = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                    strKEY = "FH_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).FH = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                    strKEY = "DRVRAT_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).DrvRat = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                    strKEY = "MAGNIF_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).Magnif = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                Next i
            Next Axis

            '----- 'V4.9.0.0�B V2.0.0.0_29�� -----
            ' X,Y�����x��ʏ푬�x�ɐ؂�ւ���
            Dim r2 As Integer
            r2 = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, 0).FL, stPclAxisPrm(AXIS_X, 0).FH, stPclAxisPrm(AXIS_X, 0).DrvRat, stPclAxisPrm(AXIS_X, 0).Magnif)
            'V4.1.0.0�L �ʐM�G���[�ȊO�����Ȃ��̂ɂQ����ʂɌ���K�v�͂Ȃ�          r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' �G���[�Ȃ�A�v�������I��(���b�Z�[�W�\���ς�)
            'V4.1.0.0�LIf (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            r2 = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, 0).FL, stPclAxisPrm(AXIS_Y, 0).FH, stPclAxisPrm(AXIS_Y, 0).DrvRat, stPclAxisPrm(AXIS_Y, 0).Magnif)
            r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' �G���[�Ȃ�A�v�������I��(���b�Z�[�W�\���ς�)
            If (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            '----- 'V4.9.0.0�B V2.0.0.0_29�� -----
            'V6.0.1.022
            If (gMachineType = MACHINE_TYPE_436S) Then
                SimpleTrimmer.BlockNoBtnVisible(False)           'V4.1.0.0�Q
                '----- 'V4.9.0.0�B V2.0.0.0_29�� -----

                'V4.6.0.0�A��
                If giTimeLotOnly = 1 Then
                    TrimData.SetLotChange()
                End If
                'V4.6.0.0�A��
                Clear113Bit() 'V4.11.0.0�G

            End If
            '----- V2.0.0.0_29�� -----

STP_CONTINUE:  '                                                        ' V1.18.0.0�B �����^�]�p�����̃G���g���|�C���g
            ' �׸ޓ�������
            giTrimErr = 0                                               ' ��ϰ �װ �׸ޏ�����
            strLOG = ""
            '#4.12.2.0�C            strLogDataBuffer = ""                                       ' ���M���O�o�b�t�@�[������ 
            strLogDataBuffer = New StringBuilder(64)                    ' ���M���O�o�b�t�@�[������            '#4.12.2.0�C
            giErrLoader = 0                                             ' ���[�_�A���[�����o(0:�����o 0�ȊO:�G���[�R�[�h) ###073
            bIniFlg = 0                                                 ' �����t���O(0=����) ###156
            bFgCyclStp = False                                          ' �T�C�N����~�t���O V4.0.0.0�R
            Form1.btnCycleStop.Text = "CYCLE STOP OFF"                          'V5.0.0.4�@
            Form1.btnCycleStop.BackColor = System.Drawing.SystemColors.Control  'V5.0.0.4�@
            m_PlateCounter = 0                                          ' �v���[�u�`�F�b�N�@�\�p������J�E���^ V1.23.0.0�F
            m_ChkCounter = 1                                            ' �v���[�u�`�F�b�N�@�\�p��`�F�b�N�J�E���^ V1.23.0.0�F
            '----- V4.11.0.0�B�� (WALSIN�aSL436S�Ή�) -----
            m_PwrChkCounter = 0                                         ' �I�[�g�p���[�`�F�b�N�p��`�F�b�N�J�E���^
            m_TimeSpan = New TimeSpan(0, 0, 0)                          ' �o�ߎ���(��)
            m_TimeAss = New TimeSpan(0, 0, 0)                           ' �w�莞��(��)
            '----- V4.11.0.0�B�� -----
            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) -----
            StPauseTime.PauseTime = New TimeSpan(0, 0, 0)
            StPauseTime.TotalTime = New TimeSpan(0, 0, 0)
            '----- V4.11.0.0�C�� -----
            bFgMagagin = False                                          'V4.0.0.0-74
            ' NG���莞�����p
            '###130 giNgBoxCounter = 0                                  ' NG�r�oBOX�̎��[�����J�E���^�[������           ###089
            m_NG_RES_Count = 0                                          ' ###142 NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��)������
            Call ClearNGTrayCount()
            If gMachineType = MACHINE_TYPE_436R Or gMachineType = MACHINE_TYPE_436S Then    'V4.10.0.0�H�������ǉ�
                gProbeCleaningCounter = 0
            End If

            '----- V6.0.3.0�S�� (�J�b�g�I�t�����@�\�p) -----
            gAdjustCutoffCount = 0                                      ' �J�b�g�I�t�����p�J�E���^�[     
            stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_DISABLE
            Form1.lblCutOff.Visible = False
            '----- V6.0.3.0�S�� -----

            m_lTrimResult = cFRS_NORMAL                                 ' ��P�ʂ̃g���~���O����(SL436R�p) = ����     ###089
            bLoaderNG = False                                           ' �g���~���ONG�t���OOFF(�u���b�N�P��/�v���[�g�P�ʂł�NG����NG��R���𒴂���)
            Form1.txtLog.ShortcutsEnabled = False                       ' ###083 �E�N���b�N���j���[��\�����Ȃ� 

            gbLastSubstrateSet = False                                  ' V4.11.0.0�O

            '----- V4.3.0.0�A�� -----
            ' ���L��SL436S���̂ݍs��
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ?
                'V4.0.0.0-82        ' 
                BlockNextButton.Enabled = False
                BlockRvsButton.Enabled = False
                BlockMainButton.Enabled = False
                'V4.0.0.0-82        ' 
            End If
            '----- V4.3.0.0�A�� -----

            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(False)
            End If
            ''V5.0.0.1�C��
            ' �����^�]�J�n�O�Ƀ��[�_���ɑO�񑗐M���̕s�v�r�b�g��OFF����(SL436R�Ŏ����^�]���̂ݑ��M(�N�����v�y�уo�L���[��OFF�����)) ###118
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gbFgAutoOperation = True) Then
                ' ���[�_�[�o��(ON=�Ȃ�, OFF=��v��+�g���~���O�m�f+�����v��+�m�f��r�o�v��)
                '###148                Call SetLoaderIO(&H0, LOUT_SUPLY + LOUT_TRM_NG + LOUT_REQ_COLECT + LOUT_NG_DISCHRAGE)
                Call SetLoaderIO(&H0, LOUT_SUPLY + LOUT_TRM_NG + LOUT_REQ_COLECT + LOUT_NG_DISCHRAGE + LOUT_PROC_CONTINUE)

            End If
            ' 'V1.13.0.0�K�@��
            If typPlateInfo.intIDReaderUse = 1 Then
                ' ID���[�_�L�Ȃ珑�����ݗp�̃t�@�C�����N���A
                globalLogFileName = gSysPrm.stLOG.gsLoggingDir + tmpLogFileName
                Call ClearTmpLogFile(globalLogFileName)
            End If
            ' 'V1.13.0.0�K�@��

            '----- V1.14.0.0�A�� -----
            ' INtime���֌Œ�ATT���(���[�U�R�}���h�Œ��������l)�𑗐M����
            lRet = SetFixAttInfo(stPWR_LSR.AttInfoAry(0))
            '            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, lRet, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                GoTo STP_ERR_EXIT                                       ' �A�v�������I����
            End If
            '----- V1.14.0.0�A�� -----

            ' �C���^�[���b�N��Ԃ̕\��/��\�� ###108
            Call Form1.DispInterLockSts()

            '---------------------------------------------------------------------------
            '   �V�O�i���^���[����(�����^�]��)����у��[�_�����^�]�ؑւ�(SL436R��)
            '---------------------------------------------------------------------------
            Call ClearBefAlarm()                                        ' ���[�_�A���[�����ޔ���N���A(SL436R �����^�]�p)
            r = Loader_ChangeMode(Form1.System1, bFgAutoMode, MODE_AUTO)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                rtnCode = r                                             ' Return�l�ݒ� 
                If (r = cFRS_ERR_RST) Then                              ' RESET�L�[(SL436R �����^�]��) ?
                    GoTo STP_TRIM_END                                   ' �I��������
                ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                    giErrLoader = r                                     ' ���[�_�A���[�����o ###073
                    GoTo STP_TRIM_END                                   ' �I��������
                ElseIf (r = cFRS_ERR_LDRTO) Then                        ' ���[�_�ʐM�^�C���A�E�g  ?
                    giErrLoader = r                                     ' ���[�_�A���[�����o ###073
                    GoTo STP_TRIM_END                                   ' �I��������
                Else                                                    ' ���̑��ُ̈�I�����x���̃G���[ 
                    GoTo STP_ERR_EXIT                                   ' �A�v�������I����
                End If
            End If

            ' ���[�_�������[�h�ڍs�������`�F�b�N����(SL436R �����^�]���[�h��)
            bFgAutoMode_BK = bFgAutoMode                                ' ���[�_�蓮/�������[�h�ޔ�
            If (gbFgAutoOperation) And (bFgAutoMode = False) Then       ' ���[�_�������[�h�ڍs���� �H
                ' "���[�_�[�����^�]�̋N�����ł��܂���ł����B" & vbCrLf & "�����𑱍s���܂����H"
                If (vbNo = MsgBox(MSG_TRIM_02 & vbCrLf & MSG_TRIM_03, vbYesNo)) Then
                    rtnCode = cFRS_ERR_TRIM                             ' Return�l = �g���}�G���[
                    GoTo STP_EXIT                                       ' �I��������
                End If
            End If
            '----- V6.1.1.0�B��(IAM�a�I�v�V����) -----
            ' �����^�]�J�n���Ԃ��擾����(SL436R��)
            If ((gMachineType = MACHINE_TYPE_436R) And (giDispEndTime = 1)) Then
                If (bFgAutoMode = True) Then                            ' �����^�] ? 
                    Call DispTrimStartTime(cStartTime)
                End If
            End If
            '----- V6.1.1.0�B�� -----

            '----- ###178�� -----
            '-----------------------------------------------------------------------
            '   �C���^�[���b�N�X�e�[�^�X�Ď��^�C�}�[�J�n(�����^�]�� SL436R)
            '-----------------------------------------------------------------------
            If (bFgAutoMode = True) Then                                ' �����^�] ? 
                Form1.TimerInterLockSts.Interval = 300                  ' �Ď��^�C�}�[�l(msec)
                Form1.TimerInterLockSts.Enabled = True                  ' �Ď��^�C�}�[�J�n
            End If
            '----- ###178�� -----

            '---------------------------------------------------------------------------
            '   �g���~���O�����s����
            '   �����^�]�ɑΉ����邽�ߘA���^�]�o�^�f�[�^�t�@�C���������[�v����
            '---------------------------------------------------------------------------
            giAppMode = APP_MODE_TRIM                                   ' �A�v�����[�h = �g���~���O��
            Call LAMP_CTRL(LAMP_START, True)                            ' START�����vON

            ' �蓮�^�]���̓��[�v�񐔂�1��ݒ肷��
            If (gbFgAutoOperation = False) Then                         ' �蓮�^�]�Ȃ�΃t�@�C����1�Ń��[�v����
                giAutoDataFileNum = 1
            Else
                stPRT_ROHM.bAutoMode = True                             '�@�����^�]�ݒ�(���[���a����)�@V1.18.0.0�B
            End If

            ' �����^�]�ɑΉ����邽�ߘA���^�]�o�^�f�[�^�t�@�C���������[�v����
            For FileNum = 0 To giAutoDataFileNum - 1
                '------------------------------------------------------------------------
                '   �t�@�C�����[�h����(SL436R �����^�]��(�����^�]�ȊO�̓��[�h�ς�))
                '------------------------------------------------------------------------
                If (gbFgAutoOperation = True) Then                      ' �����^�]���[�h�Ȃ�t�@�C�����[�h����
                    Call Form1.Z_CLS()                                  ' �f�[�^���[�h�Ń��O��ʃN���A ###013
                    gDspCounter = 0                                     ' ���O��ʕ\��������J�E���^�N���A ###013
                    m_lTrimResult = cFRS_NORMAL                         ' ��P�ʂ̃g���~���O����(SL436R�p) = ���� ###089
                    If (strFNAM = gsAutoDataFileFullPath(FileNum)) Then ' �������̃t�@�C�������O��Ɠ����Ȃ烍�[�h���Ȃ� 
                        GoTo STP_SKIP_FILE_LOAD
                    End If
                    FormLotEnd.Processed = 0                            ' ����������N���A             ' V6.0.3.0_21
                    r = Form1.Sub_FileLoad(gsAutoDataFileFullPath(FileNum))
                    If (r <> cFRS_NORMAL) Then                          ' �t�@�C�����[�h�G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                        rtnCode = r                                     ' Return�l�ݒ� 
                        GoTo STP_TRIM_ERR                               ' �g���}�G���[�M�����M��
                    End If
                    strFNAM = gsAutoDataFileFullPath(FileNum)           ' �������̃t�@�C�����ޔ�
                    gCmpTrimDataFlg = 0                                 ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�) ###223
                    stPRT_ROHM.bAutoMode = True                         ' �����^�]�ݒ�(���[���a����)�@V1.18.0.0�B

                    ' ��i��ԍ���ޔ����� ###089
                    If (FileNum <> 0) Then                              ' �ŏ��̃f�[�^�t�@�C���ȊO ? 
                        ' ��i��ԍ����ς�����ꍇ��NG�r�oBOX�̎��[�����J�E���^�[��0�ȏ�̎���
                        ' ���b�Z�[�W��\������
                        If (SBLoader <> (typPlateInfo.intWorkSetByLoader - 1)) Then
                            If (giNgBoxCounter > 0) Then                ' NG�r�oBOX�̎��[�����J�E���^�[ > 0 ?
                                giAppMode = APP_MODE_LDR_ALRM           ' �A�v�����[�h = ���[�_�A���[�����(�J�o�[�J�̃G���[�Ƃ��Ȃ���)
                                Call COVERCHK_ONOFF(COVER_CHECK_OFF)    ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ��� ###088

                                '----- V1.18.0.1�G�� -----
                                ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
                                r = EL_Lock_OnOff(EX_LOK_MD_OFF)
                                If (r = cFRS_TO_EXLOCK) Then            ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                                    rtnCode = cFRS_ERR_RST
                                    GoTo STP_TRIM_END                   ' �I��������
                                End If
                                If (r < cFRS_NORMAL) Then               ' �ُ�I�����x���̃G���[ ? 
                                    rtnCode = r
                                    GoTo STP_ERR_EXIT                   ' �A�v���P�[�V���������I��
                                End If
                                '----- V1.18.0.1�G�� -----

                                ''"��i�킪�ς��܂���","�m�f�r�o�{�b�N�X����m�f�����菜���Ă���","START�L�[���������AOK�{�^���������ĉ������B" V6.1.1.0�I
                                ' "��i�킪�ς��܂���","�m�f�r�o�{�b�N�X����m�f�����菜����","START�L�[���������AOK�{�^���������ĉ������B"     V6.1.1.0�I
                                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                                        MSG_LOADER_19, MSG_LOADER_20, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                                If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' ����~���̃G���[�Ȃ�A�v�������I����(�G���[���b�Z�[�W�͕\���ς�) 

                                giAppMode = APP_MODE_TRIM               ' �A�v�����[�h = �g���~���O��
                                Call COVERLATCH_CLEAR()                 ' �J�o�[�J���b�`�̃N���A ###088
                                Call COVERCHK_ONOFF(COVER_CHECK_ON)     ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ��� ###088
                                '----- V1.18.0.1�G�� -----
                                ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
                                r = EL_Lock_OnOff(EX_LOK_MD_ON)
                                If (r = cFRS_TO_EXLOCK) Then            ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                                    rtnCode = cFRS_ERR_RST
                                    GoTo STP_TRIM_END                   ' �I��������
                                End If
                                If (r < cFRS_NORMAL) Then               ' �ُ�I�����x���̃G���[ ? 
                                    rtnCode = r
                                    GoTo STP_ERR_EXIT                   ' �A�v���P�[�V���������I��
                                End If
                                '----- V1.18.0.1�G�� -----
                            End If
                            ' ��i��ԍ����ς������uNG�r�oBOX�̎��[�����J�E���^�[�v������������
                            '###130 giNgBoxCounter = 0                  ' �uNG�r�oBOX�̎��[�����J�E���^�[�v��������
                            Call ClearNGTrayCount()
                        End If
                    End If
                    SBLoader = typPlateInfo.intWorkSetByLoader - 1      ' ��i��ԍ��ޔ�(-1�����l)
                End If

                '---------------------------------------------------------------------------
                '   �g���~���O���s�O�`�F�b�N
                '---------------------------------------------------------------------------
                ' �f�[�^���[�h�`�F�b�N
                If (gLoadDTFlag = False) Then                            ' �f�[�^�����[�h ?
                    ' "�g���~���O�p�����[�^�f�[�^�����[�h���邩�V�K�쐬���Ă�������"
                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    rtnCode = cFRS_ERR_TRIM                             ' Return�l = �g���}�G���[
                    GoTo STP_TRIM_ERR                                   ' �g���}�G���[�M�����M��
                End If

                ' �e�k���ɉ��H�������ݒ肳��Ă��邩�`�F�b�N����(�t�@�C�o�[���[�U��) 
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then       ' FL(̧��ްڰ��) ?
                    '' �V���A���|�[�g����g���}�[���H�������ʂɎ�M����
                    'r = RsReceiveBankChkALL(cTIMEOUT)
                    'If (r <> SerialErrorCode.rRS_OK) Then               ' �G���[ ? 
                    '    If (r = SerialErrorCode.rRS_FLCND_NONE) Then
                    '        strMSG = MSG_140                            '"�e�k���̉��H�����̐ݒ肪����܂���B�ēx�f�[�^�����[�h���邩�A�ҏW��ʂ�����H�����̐ݒ���s���Ă��������B"
                    '    Else
                    '        strMSG = MSG_141                            '"�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
                    '    End If
                    '    Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    '    rtnCode = cFRS_ERR_TRIM                         ' Return�l = �g���}�G���[
                    '    GoTo STP_TRIM_ERR                               ' �g���}�G���[�M�����M��
                    'End If

                    'r = TrimCondInfoRcv(stCND)                          ' FL�����猻�݂̉��H��������M����
                    'If (r <> SerialErrorCode.rRS_OK) Then               ' �G���[ ? 
                    '    If (r = SerialErrorCode.rRS_FLCND_NONE) Then
                    '        strMSG = MSG_140                            '"�e�k���̉��H�����̐ݒ肪����܂���B�ēx�f�[�^�����[�h���邩�A�ҏW��ʂ�����H�����̐ݒ���s���Ă��������B"
                    '    Else
                    '        strMSG = MSG_141                            '"�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
                    '    End If
                    '    Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    '    rtnCode = cFRS_ERR_TRIM                         ' Return�l = �g���}�G���[
                    '    GoTo STP_TRIM_ERR                               ' �g���}�G���[�M�����M��
                    'End If
                End If

STP_SKIP_FILE_LOAD:
                ' �f�W�X�C�b�`�̓ǎ��
                Call Form1.GetMoveMode(digL, digH, digSW)
                If (digL > 6) Then
                    ' "���샂�[�h�i�f�W�X�C�b�`�j�̐ݒ�� x0�`x6 �ɂ��Ă�������"
                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_25, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    rtnCode = cFRS_ERR_TRIM                             ' Return�l = �g���}�G���[
                    GoTo STP_TRIM_ERR                                   ' �g���}�G���[�M�����M��
                End If

                '---------------------------------------------------------------------------
                '   �e��p�����[�^�擾
                '---------------------------------------------------------------------------
                With typPlateInfo
                    ' �V�X�e���ϐ��ݒ�(�v���[�uON/OFF�ʒu)
                    Call PROP_SET(.dblZOffSet, .dblZWaitOffset,
                            gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, gSysPrm.stDEV.gfSmaxX, gSysPrm.stDEV.gfSmaxY)

                    '���̌Ăѕ�������Ə�Őݒ肵���v���[�u��ON/OFF�ʒu���N���A������B
                    '�����s�������A��������OcxSystem��IF�g�p���~�߂ĉ��������B
                    'r = Form1.System1.EX_PROBON(gSysPrm)

                    ' NET�̂݁F2009/07/24 �Ȃ��R�R�ŃZ�b�g���Ă���̂����Ԃ��������璲��
                    gwCircuitCount = .intCircuitCntInBlock              ' 1��ۯ��໰��Đ�(TKY) 
                    If (gTkyKnd = KND_NET) Then                         ' ###164 
                        gwCircuitCount = .intGroupCntInBlockXBp         ' ###164 1��ۯ��໰��Đ�(NET) 
                    End If                                              ' ###164 

                    '----- V1.14.0.0�D�� -----
                    '' LED�̐ݒ�(�I�v�V����)
                    'stATLD.iLED = .intLedCtrl                           ' LED����(TKY-CHIP�̂�)
                    'If gSysPrm.stSPF.giLedOnOff = 1 Then
                    '    Call Form1.System1.Set_Led(0, stATLD.iLED, 0)   ' �o�b�N���C�g�Ɩ��n�m�^�n�e�e(LED����)
                    'End If
                    '----- V1.14.0.0�D�� -----

                End With ' typPlateInfo

                '----- V1.23.0.0�E�� -----
                ' ���L�𕜊�
                '*****************************************************************************
                '(2010/11/09)�@DelayTrim2�͈ꎞ�I�ɃR�����g�A�E�g
                '*****************************************************************************
                '==================================
                ' CHIP�̂݁F�i090414�Fminato)
                '==================================
                If (gTkyKnd = KND_CHIP) Then
                    ' �ިڲ��������׸ނ�����������B
                    m_blnDelayCheck = False
                    m_blnDelayLastCut = False
                    m_blnDelayFirstCut = False
                    intGetCutCnt = 1

                    ' �ިڲ���2���s�ł��邩��������B
                    If (typPlateInfo.intDelayTrim = 2) Then
                        ' ��Đ����S�ē����ł��邩/ڼ�Ӱ�ނ����݂��Ȃ�����������B
                        m_blnDelayCheck = DelayTrimCheck(intGetCutCnt)
                        ' �ިڲ���2���s����NG�̏ꍇ�ɂͶ�Đ���1�ɐݒ肷��B
                        If Not m_blnDelayCheck Then
                            intGetCutCnt = 1
                        End If
                    Else
                        m_blnDelayLastCut = True
                    End If

                    ' �ިڲ���2�p�\���̂̏�����
                    Init_stDelay2(typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir, gRegistorCnt)
                End If
                '----- V1.23.0.0�E�� -----

COVER_CLOSE_RETRY:  'V4.7.0.0�P
                '-----------------------------------------------------------------------
                '   �X���C�h�J�o�[���N���[�Y����(�蓮/����) (SL432R��)
                '-----------------------------------------------------------------------
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then   ' SL432R�n ? 
                    If (gSysPrm.stSPF.giWithStartSw = 1) And (giHostMode <> cHOSTcMODEcAUTO) Then
                        ' �X���C�h�J�o�[�����N���[�Y���Ȃ� (����SW�����҂�(�I�v�V����) �Ń��[�_�����^�]���łȂ��ꍇ)
                        r = Form1.System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, True)
                        Form1.Refresh()                 'V1.13.0.0�L
                        If (r = cFRS_ERR_START) Then r = cFRS_NORMAL ' START SW�����Ȃ�Return�l = ����

                    Else
                        'V4.7.0.0�P �z���G���[�ƃN�����v�G���[����ʂŏ��������giAppMode����iAppMode�֕ύX��
                        Dim iAppMode As Short
                        If giHostMode = cHOSTcMODEcAUTO And giAppMode = APP_MODE_TRIM Then
                            iAppMode = APP_MODE_TRIM_AUTO
                        Else
                            iAppMode = giAppMode
                        End If
                        'V4.7.0.0�P �z���G���[�ƃN�����v�G���[����ʂŏ�������ׁ�
                        ' �X���C�h�J�o�[�������N���[�Y����
                        ' XY_SLIDE�������� ? (0:OFFLINE, 1:ONLINE, 2:SLIDE COVER+XY�ړ�)
                        If gSysPrm.stTMN.giOnline = TYPE_MANUAL Then        ' XY_SLIDE�������� 
                            r = Form1.System1.Z_CCLOSE(gSysPrm, giAppMode, 0, True, gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY) '###134
                        End If
                        If gSysPrm.stTMN.giOnline = TYPE_ONLINE Then        ' XY_SLIDE�ʏ퓮��
                            r = Form1.System1.Z_CCLOSE(gSysPrm, giAppMode, 0, False, stgx, stgy)
                        End If
                        'V4.7.0.0�P �z���G���[�ƃN�����v�G���[����ʂŏ��������giAppMode����iAppMode�֕ύX��
                        If (r = cFRS_TO_CLAMP_ON) Or (r = cFRS_VACCUME_ERROR) Then
                            'V5.0.0.9�M ��
                            ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF) ' �W��((�ԓ_��+�u�U�[�P))
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                            'V5.0.0.9�M ��

                            strMSG = "�G���["
                            If (r = cFRS_TO_CLAMP_ON) Then
                                strMSG = "�N�����v���m�F���Ă�������"        '�N�����v�G���[
                            ElseIf (r = cFRS_VACCUME_ERROR) Then
                                strMSG = "�z�����m�F���Ă�������"          ' �z���G���[
                            End If
                            r = Form1.System1.Form_MsgDispStartReset(strMSG, "RESET�L�[�ŏI�����܂��B", &HFF, &HFF0000)
                            Call ZCONRST()                                                  ' ���b�`����

                            'V5.0.0.9�M ��
                            ' Call Form1.System1.SetSignalTower(0, &HFFFF)   ' �V�O�i���^���[���䏉����(On=0, Off=�S�ޯĵ�)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                            'V5.0.0.9�M ��

                            If (r = cFRS_ERR_START) Then
                                GoTo COVER_CLOSE_RETRY
                            ElseIf (r = cFRS_ERR_RST) Then
                                'frmAutoObj.SetAutoOpeCancel()
                                GoTo STP_TRIM_END
                            Else
                                GoTo STP_ERR_EXIT                               ' ����~���̃G���[�Ȃ�A�v�������I����
                            End If
                        End If
                        If giHostMode = cHOSTcMODEcAUTO Then
                            'V5.0.0.9�M ��
                            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)    ' �V�O�i���^���[����(On=�����^�]��(�Γ_��),Off=�S�ޯ�)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                            'V5.0.0.9�M ��

                        End If
                        'V4.7.0.0�P �z���G���[�ƃN�����v�G���[����ʂŏ�������ׁ�

                    End If
                Else
                    ' �N�����v�y�уo�L���[��ON(SL436R��) '###001
                    If (bFgAutoMode = False) Then                   ' �����^�]���̓N�����v�y�уo�L���[��ON���Ȃ� ###107
                        ' �ǉ��������ǁA������ƒx��������̂Ō��ɖ߂�   System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)     'V5.0.0.8�E
                        r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 1, giAppMode, giTrimErr)
                    End If
                End If

                If (r = cFRS_ERR_RST) Then                          ' Cancel(RESET��) ?
                    rtnCode = cFRS_ERR_RST                          ' Return�l = Cancel(RESET��)
                    GoTo STP_TRIM_END                               ' �I��������
                ElseIf ((r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Or (r = cFRS_ERR_LATCH)) Then
                    GoTo STP_TRIM_END                               ' �J�o�[�J�G���[�Ȃ�I��������
                ElseIf (r <> cFRS_NORMAL) Then                      ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                    GoTo STP_ERR_EXIT                               ' ����~���̃G���[�Ȃ�A�v�������I����
                End If

                'V5.0.0.6�@��
                r = basTrimming.CheckControllerInterlock()       ' �O���R���g���[���`�F�b�N�i�����ŃV�X�p���@�\�L���m�F�j
                If (r = (cFRS_ERR_EMG)) Then
                    GoTo STP_ERR_EXIT                               ' ����~���̃G���[�Ȃ�A�v�������I����
                ElseIf (r = cFRS_ERR_RST) Then
                    rtnCode = cFRS_ERR_RST                          ' Return�l = Cancel(RESET��)
                    GoTo STP_TRIM_END                               ' �I��������
                End If
                'V5.0.0.6�@��

                ' ���O��ʕ\���N���A ###013
                gDspCounter = gDspCounter + 1                       ' ���O��ʕ\��������J�E���^�X�V
                '#4.12.2.0�C                If (gDspCounter > gDspClsCount) Then                ' �J�E���^ > ���O��ʕ\���N���A�����
                If (gDspCounter > gDspClsCount) OrElse (Form1.TxtLogLengthLimit()) Then '#4.12.2.0�C
                    ' �J�E���^ > ���O��ʕ\���N���A����� OrElse �����������ȏ�
                    Call Form1.Z_CLS()                              ' ���O��ʃN���A
                    gDspCounter = 1                                 ' ���O��ʕ\��������J�E���^�Đݒ�
                End If
                Form1.ShowSelectedMap(False)                        ' �I���ς݉��H�Ώۃu���b�N��ύX�s�ŕ\������ 'V4.12.2.0�A
                'V6.0.1.0�J
                Form1.SetMapOnOffButtonEnabled(False)               ' MAP ON/OFF �{�^���𖳌��ɂ���              'V4.12.2.0�@

                'V6.0.1.0�P��
                ' �l�`�o�{�^���̑I����Ԃɂ���āA�\����؂�ւ��� 
                '                Form1.SetTrimMapVisible(True)                           'V6.0.1.0�J
                Form1.SetTrimMapVisible(Form1.MapOn)                'V6.0.1.0�J
                'V6.0.1.0�P��
                Form1.MoveHistoryDataLocation(True)                     'V6.0.1.0�J
                '---------------------------------------------------------------------------
                '   �������[�U�p���[�������s
                '---------------------------------------------------------------------------
                'V5.0.0.4�F         If (digL < 2) Or (digL = 5) Then                        ' ���ݸ�Ӱ�� = x0,x1,x5 ?
                If (digL < 2) Or (digL = 5) Or (digL = 2 And typPlateInfo.intNGMark <> 0) Then     ' ���ݸ�Ӱ�� = x0,x1,x5 ?
                    bIniPwrFlg = True                                   ' V4.11.0.0�B
                    r = AutoLaserPowerADJ(MD_INI)                       ' ���[�U�p���[�������s�@V4.11.0.0�B
                    If (r = cFRS_ERR_RST) Then                          ' Cancel(RESET��) ?
                        rtnCode = cFRS_ERR_RST                          ' Return�l = Cancel(RESET��)
                        GoTo STP_TRIM_END                               ' �I�������� 
                    ElseIf (r <> cFRS_NORMAL) Then                      ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                        GoTo STP_ERR_EXIT                               ' ����~���̃G���[�Ȃ�A�v�������I����
                    End If
                End If
                '----- V4.11.0.0�B�� (WALSIN�aSL436S�Ή�) -----
                ' �u����(��)�w��v�@�\����Ŏ���(��)�w��v���� ?
                If (giPwrChkTime = 1) And (typPlateInfo.intPwrChkTime <> 0) Then
                    m_TimeSTart = DateTime.Now
                    m_TimeAss = New TimeSpan(0, typPlateInfo.intPwrChkTime, 0)
                End If
                '----- V4.11.0.0�B�� -----

                '---------------------------------------------------------------------------
                '   �g���~���O�O���� (SL436R�n��)
                '    �i��f�[�^���M(STB)
                '    (�`�b�v�T�C�YXY, �A���g���~���O�m�f�������,�g���~���O�m�f�J�E���^(���)
                '    �������r�o�J�E���^(���)���[�_�ɑ��M���遦���󏈗��Ȃ�)
                '---------------------------------------------------------------------------
                r = Loader_TrimPreStart(Form1.System1, bFgAutoMode, typPlateInfo.dblChipSizeXDir, typPlateInfo.dblChipSizeYDir,
                                             typPlateInfo.intTrimNgCount, typPlateInfo.intMaxTrimNgCount, typPlateInfo.intMaxBreakDischargeCount)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                    GoTo STP_ERR_EXIT                                   ' ����~���̃G���[�Ȃ�A�v�������I����
                End If

                '----- V6.0.3.0_31�� -----
                If (gbFgAutoOperation = True) Then
                    gbAutoOperating = True                              ' �����^�]������s�t���O�ݒ�@
                End If
                '----- V6.0.3.0_31�� -----

                '---------------------------------------------------------------
                '   ���[�_�֊�v��(�����/�����v���M��)�𑗐M���A
                '   ���[�_����̃g���~���O�X�^�[�g�M����҂�(SL436R�n��)
                '---------------------------------------------------------------
                Do
                    '----- V2.0.0.0�K�� -----
                    If gKeiTyp = KEY_TYPE_RS Then
                        ' �Ƃ����_�ɖ߂�(SL436R/SL436S��) ���␳���[�h=1(�蓮),�␳���@=1(1��̂�)���͑O��̈ʒu�։�]������K�v����(������)
                        If (bFgAutoMode = True) Then
                            If (gSysPrm.stDEV.giTheta <> 0) Then        ' �Ƃ��� ?
                                Call ROUND4(0.0)
                            End If
                        End If
                    End If
                    '----- V2.0.0.0�K�� -----

                    ' ���[�_�֊�v��(�����/�����v���M��)�𑗐M���A�g���~���O�X�^�[�g�M����҂�(SL436R�n��)
                    r = Loader_WaitTrimStart(Form1.System1, bFgAutoMode, m_lTrimResult, bFgMagagin, bFgAllMagagin, bFgLot, bIniFlg)
                    'V4.9.0.0�@��
                    If (gMachineType = MACHINE_TYPE_436S) Then
                        ' W113.02��OFF����
                        SetLotStopBit(0)
                    End If
                    'V4.9.0.0�@��
                    Call Form1.System1.AutoLoaderFlgReset()                     ' �I�[�g���[�_�@�t���O���Z�b�g ###099
                    If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                        rtnCode = r                                             ' Return�l�ݒ� 
                        'V5.0.0.9�M �� V6.0.3.0�G ��~�ڍs��ԁi�����^�]�ŃA���[�����b�Z�[�W��OK�����Ă����~��ԂɈڍs����܂ł̊�)
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_WAIT_IDLE)
                        'V5.0.0.9�M �� V6.0.3.0�G
                        If (r = cFRS_ERR_RST) Then                              ' RESET�L�[(SL436R �����^�]��) ?
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                            GoTo STP_TRIM_END                                   ' �I��������
                        ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                            '                                                   ' ���[�_�A���[�����o(SL436R �����^�]��) ? ###073
                            giErrLoader = r                                     ' ���[�_�A���[�����o ###073
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                            Call W_RESET()                                      ' �A���[�����Z�b�g 
                            GoTo STP_TRIM_END                                   ' �I��������
                        ElseIf (r = cFRS_ERR_LDRTO) Then                        ' ���[�_�ʐM�^�C���A�E�g  ?
                            giErrLoader = r                                     ' ���[�_�A���[�����o ###073
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                            GoTo STP_TRIM_END                                   ' �I��������
                        ElseIf (r = cFRS_ERR_LOTEND) Then                       ' LotEnd 
                            '----- V4.0.0.0�R�� -----
                            'V2.0.0.0_21�@��
                            ' giErrLoader = r                                   ' ���[�_�A���[�����o V1.16.0.0�H
                            giErrLoader = cFRS_NORMAL                           ' ���b�g�I���͐���Ƃ��� V1.16.0.0�H
                            rtnCode = cFRS_NORMAL
                            '----- V4.0.0.0�R�� -----
                            bFgAllMagagin = True
                            'V2.0.0.0_21�@��
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                            GoTo STP_TRIM_END                                   ' �I��������
                        Else                                                    ' ���̑��ُ̈�I�����x���̃G���[ 
                            GoTo STP_ERR_EXIT                                   ' �A�v�������I����
                        End If
                    End If

                    '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
                    bAllMagazineFinFlag = bFgMagagin
                    ' �ŏI����́u������{�^���v�������Ȃ��悤�ɂ���(�����^�]��)
                    If (gKeiTyp = KEY_TYPE_RS) And (bFgAutoMode = True) Then
                        Clear113Bit() 'V4.11.0.0�G

                        If (bFgMagagin) Then
                            Form1.BtnSubstrateSet.Enabled = False       '�u������{�^���v�񊈐���
                            gbChkSubstrateSet = False                   ' ������t���OOFF
                            Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
                            gbLastSubstrateSet = True                                   ' V4.11.0.0�O

                        Else
                            'V4.11.0.0�O Form1.BtnSubstrateSet.Enabled = True     'V4.11.0.0�O   '�u������{�^���v������
                            Form1.BtnSubstrateSet.Enabled = False     'V4.11.0.0�O   '�u������{�^���v������
                        End If
                    End If
                    '----- V4.11.0.0�E�� -----

                    ' �v���[�u�N���[�j���O�J�E���^�X�V
                    'V6.0.0.1�C                    If (typPlateInfo.intPrbCleanAutoSubCount <> 0) Then
                    If (typPlateInfo.intPrbCleanAutoSubCount <> 0) AndAlso (1 <= Form1.Instance.stFNC(F_PROBE_CLEANING).iDEF) Then 'V6.0.0.1�C
                        If (digL < 4) Then
                            gProbeCleaningCounter = gProbeCleaningCounter + 1
                            If typPlateInfo.intPrbCleanAutoSubCount <= gProbeCleaningCounter Then
                                'V4.3.0.0�B
                                globaldblCleaningPosX = typPlateInfo.dblPrbCleanPosX
                                globaldblCleaningPosY = typPlateInfo.dblPrbCleanPosY
                                globaldblCleaningPosZ = typPlateInfo.dblPrbCleanPosZ
                                frmProbeCleaning.ProbeCleaningCnt = typPlateInfo.intPrbCleanUpDwCount
                                'V4.3.0.0�B
                                frmProbeCleaning.ProbeCleaningStart()
                                gProbeCleaningCounter = 0
                            End If
                        End If
                    End If

                    '----- V4.11.0.0�B�� (WALSIN�aSL436S�Ή�) -----
                    '-----------------------------------------------------------------
                    ' �I�[�g�p���[�������s(������w��/����(��)�w��)FL�p(�����^�]��)
                    '-----------------------------------------------------------------
                    Call Form1.GetMoveMode(digL, digH, digSW)           ' �f�W�X�C�b�`�̓ǎ��
                    ' �I�[�g�p���[�u������w��v�@�\���薔�́u����(��)�w��v�@�\����̏ꍇ
                    If (giPwrChkPltNum = 1) Or (giPwrChkTime = 1) Then
                        m_PwrChkCounter = m_PwrChkCounter + 1           ' �`�F�b�N�J�E���^�X�V
                        m_TimeNow = DateTime.Now
                        m_TimeSpan = m_TimeNow - m_TimeSTart            ' �o�ߎ��ԍX�V 

                        If (bIniPwrFlg = True) Then                     ' ����͏�L�Ŏ��{���Ă���̂ł��Ȃ�
                            bIniPwrFlg = False
                        Else
                            ' �I�[�g�p���[�������s(������w��/����(��)�w��)FL�p
                            r = AutoPowerExeByOption(digL, m_PwrChkCounter, m_TimeSTart, m_TimeSpan, m_TimeAss)
                            If (r <> cFRS_NORMAL) Then                  ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                                rtnCode = r                             ' Return�l�ݒ� 
                                If (r = cFRS_ERR_RST) Then              ' RESET�L�[(SL436R �����^�]��) ?
                                    Call SetLoaderIO(&H0, LOUT_SUPLY)   ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                                    GoTo STP_TRIM_END                   ' �I��������
                                Else                                    ' ���̑��ُ̈�I�����x���̃G���[ 
                                    GoTo STP_ERR_EXIT                   ' �A�v�������I����
                                End If
                            End If
                        End If
                    End If
                    '----- V4.11.0.0�B�� -----

                    '---------------------------------------------------------------------------
                    '   �ƕ␳����
                    '---------------------------------------------------------------------------
                    '----- ###172�� -----
                    ' �O��͎������[�h�Ńp�^�[���F���G���[�Ȃ珈���I��/���s���b�Z�[�W��\������
                    ' (HALT SW���� ���� ADJ�{�^��ON��)
                    If (bFgAutoMode = True) And (m_lTrimResult = cFRS_ERR_PTN) Then
                        ' HALT SW�ǂݍ���
                        Call HALT_SWCHECK(Sw)
                        ' HALT SW���� ���� ADJ�{�^��ON ?
                        If (Sw = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Then
                            Call ZCONRST()                              ' ���b�`����

                            ' "�����^�]��~��","","START�L�[�F�������s�CRESET�L�[�F�����I��"
                            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                                    MSG_LOADER_30, "", MSG_SPRASH35, System.Drawing.Color.Black, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                            If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' ����~���̃G���[�Ȃ�A�v�������I����(�G���[���b�Z�[�W�͕\���ς�) 
                            If (r = cFRS_ERR_RST) Then                  ' RESET�L�[�����Ȃ珈���I���� 
                                rtnCode = cFRS_ERR_RST                  ' Return = Cancel(RESET�L�[) 
                                Call W_RESET()                          ' �A���[�����Z�b�g 
                                Call SetLoaderIO(&H0, LOUT_AUTO)        ' ���[�_�蓮���[�h�ؑւ�(���[�_�o��(ON=�Ȃ�, OFF=����))
                                'r = DspNGTrayChk()                      ' NG�g���C�ւ̔r�o�����M����OFF��҂�
                                GoTo STP_TRIM_END
                            End If
                        End If
                    End If
                    '----- ###172�� -----

                    blnRotTheta = False                                 ' ۯĖ��̃ƕ␳ = �Ȃ�
                    gfCorrectPosX = 0.0                                 ' �␳�l������ 
                    gfCorrectPosY = 0.0
                    gdCorrectTheta = 0.0                                'V5.0.0.9�H
                    If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then           ' SL432R�n ? 
                        If (gTkyKnd = KND_CHIP) Then                    ' CHIP�̂�
                            ' ۯĖ��̃ƕ␳����(�V�X�p��)�ŕ␳���[�h=0(����)�̎��ɍs��
                            If (gSysPrm.stSPF.giRotTheta = 1) And (typPlateInfo.intReviseMode = 0) Then
                                '' ۰�ްI/O����Bit4���������s���B
                                'Call GetLoaderIO(lWork)                 ' ۰�ްIO����
                                'If (lWork And &H10S) Then               ' ۯĐؑփƕ␳����H���@����BIT�͂Ȃ�
                                '    blnRotTheta = True                  ' ۯĖ��̃ƕ␳ = ����
                                'End If
                            End If
                        End If
                    End If

                    ' ���t�A���C�����g�����s���邩�H   'V5.0.0.9�I
                    Dim doRough As Boolean = (True = bFgAutoMode) AndAlso
                        (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) AndAlso
                        (0 <> typPlateInfo.intReviseExecRgh) AndAlso (0 < giClampLessRoughCount)

                    Form1.Instance.txtLog.Visible = False   'V6.0.0.0-27

                    ' ۯĖ��̃ƕ␳����ł��邩��������B
                    r = cFRS_NORMAL
                    If (blnRotTheta) Then                               ' ۯĖ��̃ƕ␳���� ?(CHIP�̂ݎg�p����t���O)
                        ' �ƕ␳�̎��s
                        'r = DoCorrectPos(typPlateInfo)
                        r = DoCorrectPos(typPlateInfo, True, doRough)   'V5.0.0.9�I
                    Else
                        ' �␳Ӱ��=1(�蓮)�ŕ␳���@=0(�␳�Ȃ�)�ł��邩��������B
                        'If (typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) Then
                        If (typPlateInfo.intReviseMode = 1) AndAlso (typPlateInfo.intManualReviseType = 0) Then
                            If (gSysPrm.stDEV.giTheta <> 0) Then        ' �Ƃ��� ?
                                If (False = doRough) Then               'V5.0.0.9�I
                                    '----- V1.19.0.0-32   �� -----
                                    gfCorrectPosX = 0.0                 ' �␳�l������ 
                                    gfCorrectPosY = 0.0
                                    '----- V1.19.0.0-32   �� -----
                                    gdCorrectTheta = 0.0                'V5.0.0.9�H
                                Else
                                    ' ���t�A���C�����g���s�A�ƕ␳�͎��s���Ȃ�
                                    r = DoCorrectPos(typPlateInfo, False, doRough) 'V5.0.0.9�I
                                End If

                                ' �Ƃ��w��p�x��]������
                                'Call ROUND4(typPlateInfo.dblRotateTheta) ' �ƍڕ���̐�Ίp�x�w���]'###037
                                Call ROUND4(typPlateInfo.dblRotateTheta + gdCorrectTheta) ' �ƍڕ���̐�Ίp�x�w���] 'V5.0.0.9�I
                            End If

                            'V5.0.0.9�I              ��
                            '    ' ۯĖ��̃ƕ␳�Ȃ��ŁA�␳���[�h=1(�蓮),�␳���@=1(1��̂�),�V�[�^�␳���s�t���O(���s����)�ȊO
                            'ElseIf (gSysPrm.stSPF.giRotTheta = 0) And Not (typPlateInfo.intReviseMode = 1 And typPlateInfo.intManualReviseType = 1 And gManualThetaCorrection = False) Then
                            '    ' �ƕ␳�̎��s
                            '    r = DoCorrectPos(typPlateInfo)
                        Else
                            ' ۯĖ��̃ƕ␳�Ȃ��ŁA�␳���[�h=1(�蓮),�␳���@=1(1��̂�),�V�[�^�␳���s�t���O(���s����)�ȊO
                            Dim doAlign As Boolean = (gSysPrm.stSPF.giRotTheta = 0) AndAlso
                                 Not ((typPlateInfo.intReviseMode = 1) AndAlso
                                 (typPlateInfo.intManualReviseType = 1) AndAlso
                                 (gManualThetaCorrection = False))

                            ' �ƕ␳�̎��s
                            r = DoCorrectPos(typPlateInfo, doAlign, doRough)
                        End If
                        'V5.0.0.9�I                  ��

                    End If
                    Form1.Instance.txtLog.Visible = True    'V6.0.0.0-27

                    ' �ƕ␳���ʂ��`�F�b�N����
                    If (r = cFRS_NORMAL) Then                           ' ���� ?

                    ElseIf (r = cFRS_ERR_RST) Then                      ' Cancel(RESET��) ?
                        GoTo STP_TRIM_END                               ' �I�������� 
                    ElseIf (r = cFRS_ERR_PTN) Then                      ' �p�^�[���F���G���[ ?(���O��ʂɃ��b�Z�[�W��\���ς�)
                        '----- V1.18.0.0�B�� .Edg_Fail�͖��g�p�̂��ߍ폜 -----
                        'If (gTkyKnd = KND_CHIP) Then                    ' CHIP�̂�(���ݸތ��ʈ��)
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Edg_Fail = stPRT_ROHM.Edg_Fail + 1 ' ۯĒ��̔F���s�Ǌ����
                        '    End If
                        'End If                                          ' ###028
                        '----- V1.18.0.0�B�� -----

                        ' ���[�_�o��(ON=�p�^�[���F���m�f, OFF=�Ȃ�) SL432R��
                        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then   ' SL432R�n ? 
                            ' ���[�_�[�o��(ON=����ݔF��NG,OFF=�Ȃ�)
                            Call SetLoaderIO(COM_STS_PTN_NG, &H0)       ' ���[�_�o��(ON=�p�^�[���F���m�f, OFF=�Ȃ�) '###035
                            rtnCode = cFRS_ERR_PTN                      ' V6.1.1.0�@
                            GoTo STP_TRIM_END                           ' �I�������� 
                        End If
                        m_lTrimResult = cFRS_ERR_PTN                    ' ��P�ʂ̃g���~���O����(SL436R�p) = �p�^�[���F���G���[
                        '----- ###146�� -----
                        'GoTo STP_NEXT_FILE                             ' ���̃t�@�C���̃��[�h������(�����^�]��)
                        If (bIniFlg = 0) Then
                            bIniFlg = 1                                 ' �t���O = 1(�g���~���O��)
                        End If
                        GoTo STP_PTN_NG
                        '----- ###146�� -----
                    Else
                        GoTo STP_ERR_EXIT                               ' ����~���̃G���[�Ȃ�A�v�������I����
                    End If
                    gManualThetaCorrection = False                      ' �V�[�^�␳���s�t���O = False(�V�[�^�␳�����s����)

                    ' �g���~���O���s���͎擾�摜�f�[�^�T�C�Y�𐧌�����(��{����1�{�ȏ�̏ꍇ�̂�)
                    Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(True)    'V6.0.0.0-26

                    '�@�L�k�␳     V1.13.0.0�D��
                    '�u���b�N�̃X�^�[�g�|�W�V������TY2�̕␳�l�͂����ő������ޕ����Ō���
                    Call CalcBlockXYStartPos()
                    r = ShinsyukuHoseiMain()
                    If (r = cFRS_NORMAL) Then                           ' ���� ?

                    ElseIf (r = cFRS_ERR_RST) Then                      ' Cancel(RESET��) ?
                        GoTo STP_TRIM_END                               ' �I�������� 
                    ElseIf (r = -1) Then
                        m_lTrimResult = cFRS_ERR_PTN                    ' ��P�ʂ̃g���~���O����(SL436R�p) = �p�^�[���F���G���[
                        GoTo STP_PTN_NG
                    ElseIf (r = cFRS_ERR_PTN) Then                      ' �p�^�[���F���G���[ ?(���O��ʂɃ��b�Z�[�W��\���ς�)
                        '----- V1.18.0.0�B�� .Edg_Fail�͖��g�p�̂��ߍ폜 -----
                        'If (gTkyKnd = KND_CHIP) Then                    ' CHIP�̂�(���ݸތ��ʈ��)
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Edg_Fail = stPRT_ROHM.Edg_Fail + 1           ' ۯĒ��̔F���s�Ǌ���� V1.18.0.0�B
                        '    End If
                        'End If                                          ' ###028
                        '----- V1.18.0.0�B�� -----

                        ' ���[�_�o��(ON=�p�^�[���F���m�f, OFF=�Ȃ�) SL432R��
                        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then   ' SL432R�n ? 
                            ' ���[�_�[�o��(ON=����ݔF��NG,OFF=�Ȃ�)
                            Call SetLoaderIO(COM_STS_PTN_NG, &H0)       ' ���[�_�o��(ON=�p�^�[���F���m�f, OFF=�Ȃ�) '###035
                            GoTo STP_TRIM_END                           ' �I�������� 
                        End If
                        m_lTrimResult = cFRS_ERR_PTN                    ' ��P�ʂ̃g���~���O����(SL436R�p) = �p�^�[���F���G���[
                        '----- ###146�� -----
                        'GoTo STP_NEXT_FILE                             ' ���̃t�@�C���̃��[�h������(�����^�]��)
                        If (bIniFlg = 0) Then
                            bIniFlg = 1                                 ' �t���O = 1(�g���~���O��)
                        End If
                        GoTo STP_PTN_NG
                        '----- ###146�� -----
                    Else
                        Call Form1.Z_PRINT("Thresh hold length over")
                        GoTo STP_TRIM_END                               ' ����~���̃G���[�Ȃ�A�v�������I����
                    End If
                    'V1.13.0.0�D��

                    '---------------------------------------------------------------------------
                    '   �摜�\���v���O�������N������
                    '---------------------------------------------------------------------------
                    If (bIniFlg = 0) Then                               ' ����̂݋N������
                        bIniFlg = 1
                        ' ���v��ʔ�\�����͋N��
                        If gKeiTyp = KEY_TYPE_RS Then
                            ' �摜�\���v���O�������N������
                            '                            r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                            ' ������ V3.1.0.0�A 2014/12/01
                            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                            'V6.0.0.0�D                            r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 1)
                            ' ������ V3.1.0.0�A 2014/12/01
                        ElseIf (Form1.chkDistributeOnOff.Checked = False) Then
                            ' ������ V3.1.0.0�A 2014/12/01
                            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                            'V6.0.0.0�D                            r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                            ' ������ V3.1.0.0�A 2014/12/01
                        Else
                            ' ���v�\�����͓��v�\����̃{�^���𖳌��ɂ���
                            gObjFrmDistribute.cmdGraphSave.Enabled = False
                            gObjFrmDistribute.cmdInitial.Enabled = False
                            gObjFrmDistribute.cmdFinal.Enabled = False
                        End If
                    End If

                    '---------------------------------------------------------------------------
                    '   �g���~���O�f�[�^��INtime���ɑ��M����
                    '---------------------------------------------------------------------------
                    r = SendTrimData()
                    If (r <> cFRS_NORMAL) Then
                        ' "�g���~���O�f�[�^�̐ݒ�Ɏ��s���܂����B" & vbCrLf & "�g���~���O�f�[�^�ɖ�肪�Ȃ����m�F���Ă��������B"
                        Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                        rtnCode = cFRS_ERR_TRIM                         ' Return�l = �g���}�G���[
                        'GoTo STP_TRIM_ERR                              ' �g���}�G���[�M�����M�� V1.24.0.0�A
                        GoTo STP_TRIM_END                               ' ' �I��������           V1.24.0.0�A
                    End If

                    '---------------------------------------------------------------------------
                    '   �J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u����ݒ肷��yTKY�p�z
                    '   ��TKY���̂ݗL������CHIP/NET�����e�[�u���ݒ�(������)�͂���
                    '---------------------------------------------------------------------------
                    giCutPosRNum = CutPosCorrectInit(gRegistorCnt, stCutPos)

                    '' �A��HI-NG�����ޯ̧������ ###129
                    'For r = 1 To gRegistorCnt
                    '    iNgHiCount(r) = 0
                    'Next r
                    ' �A��HI-NG�����ޯ̧������ ###181
                    Call ClearNgHiCount()

                    '---------------------------------------------------------------------------
                    '   ���O��ʂɊJ�n���t������\������
                    '---------------------------------------------------------------------------
                    '----- V1.22.0.0�C�� -----
                    'strMSG = CStr(Today) & " " & CStr(TimeOfDay)
                    strMSG = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")       'V4.4.0.0-0
                    If (stSummaryLog.strStartTime = "") Then
                        stSummaryLog.strStartTime = strMSG              ' �T�}�����O�J�n���� 
                    End If
                    Call Form1.Z_PRINT("START " & strMSG)
                    'Call Form1.Z_PRINT("START " & CStr(Today) & " " & CStr(TimeOfDay))
                    '----- V1.22.0.0�C�� -----

                    ' ۸ޕ\���J�n�׸ނ���������B
                    If gSysPrm.stLOG.giLoggingMode = 1 Then
                        '' '' '''�X���b�h�̏����Ƃ��đ��x�������������B�������A���O�f�[�^�̕ۑ����@�̌����K�v
                        '' '' ''Dim t As New System.Threading.Thread( _
                        '' '' ''        New System.Threading.ThreadStart( _
                        '' '' ''         AddressOf TrimLogging_Start2))
                        '' '' ''t.Start()
                        Call TrimLogging_Start2()                       ' ���M���O�J�n
                    End If

                    '(2011/06/21)
                    '   �����ł̓e�[�u���𓮍삳�����ATrimmingBlockLoop�ŃX�e�[�W�𓮍삳����B
                    '   ���̏ꏊ�ł́A�X�e�[�W�I�t�Z�b�g�ƕ␳�l�̉��Z�݂̂��s���B
                    '   �f�o�b�O/�e�X�g�ɂĕK�v�����������A�K�v�ł���΂����ł̓���Ƃ���B
                    ' X�J�n�ʒu�{�I�t�Z�b�g�{�␳�l
                    'stgx = typPlateInfo.dblTableOffsetXDir + gfCorrectPosX             'V1.13.0.0�B
                    stgx = typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX  'V1.13.0.0�B
                    ' Y�J�n�ʒu�{�I�t�Z�b�g�{�␳�l
                    'stgy = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY             'V1.13.0.0�B
                    stgy = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY  'V1.13.0.0�B

                    '----- V4.0.0.0-40�� -----
                    ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
                    Sub_GetStageYPosistion(stgy)

                    ''----- V2.0.0.0�H�� -----
                    ''If (giMachineKd = MACHINE_KD_RS) Then
                    ''    stgy = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2) ' V4.0.0.0-40
                    ''End If
                    ''----- V2.0.0.0�H�� -----
                    '----- V4.0.0.0-40�� -----

                    ' ����~���݂̊m�F
                    If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

                    '---------------------------------------------------------------------------
                    '   BP�ʒu�̐ݒ�
                    '---------------------------------------------------------------------------
                    ' �u���b�N�T�C�Y�ABP�ʒu�̾��X,Y�ݒ�
                    Call BSIZE(typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)
                    Call BPOFF(typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)

                    ' ����~���݂̊m�F
                    If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

                    Call DebugCheck()

                    '---------------------------------------------------------------------------
                    '   �X�e�[�W�ʒu�̎Z�o
                    '---------------------------------------------------------------------------
                    '�v���[�g�̃X�^�[�g�|�W�V����
                    Call CalcPlateXYStartPos()
                    '�u���b�N�̃X�^�[�g�|�W�V������TY2�̕␳�l�͂����ő������ޕ����Ō���
                    Call CalcBlockXYStartPos()

                    '---------------------------------------------------------------------------
                    '   Z�ҋ@�ʒu���Z�o����
                    '---------------------------------------------------------------------------
                    ' Z���ï��&��߰Ĉʒu = ZON�ʒu - �ï�ߏ㏸���� 
                    zStepPos = typPlateInfo.dblZOffSet - typPlateInfo.dblZStepUpDist
                    ' �ï��&��߰Ĉʒu���ҋ@�ʒu��菬�����ꍇ�́A�ҋ@�ʒu��ï��&��߰Ĉʒu�Ƃ���
                    If (zStepPos < typPlateInfo.dblZWaitOffset) Then zStepPos = typPlateInfo.dblZWaitOffset

                    '=======================================================
                    '   �g���~���O�u���b�N���[�v(1����̃g���~���O�������s��)
                    '   ���e�[�u���ړ���Z ON/OFF�͂��̒��ōs���Ă���
                    '=======================================================
                    If gKeiTyp = KEY_TYPE_RS Then                       'V2.0.0.0�I �V���v���g���}�̏ꍇ
                        SimpleTrimmer.TrimmingStart()                   'V2.0.0.0�I �X�^�[�g������
                        SimpleTrimmer.BlockNoBtnVisible(True)           'V4.1.0.0�Q
                        '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
                        'SetNowBlockDspNum(1)                            'V4.1.0.0�Q
                        ' �g���~���O�J�n�u���b�N�ԍ�XY���擾����(�蓮�^�]���̂ݗL��)
                        Call Form1.Get_StartBlkNum(WkBlkX, WkBlkY)
                        WkBlkX = WkBlkX * WkBlkY
                        SetNowBlockDspNum(WkBlkX)                       ' �u���b�N�ԍ��\��
                        '----- V4.11.0.0�D�� -----
                    End If                                              'V2.0.0.0�I

                    m_lTrimResult = cFRS_NORMAL                         ' ��P�ʂ̃g���~���O����(SL436�p) = ���� '###089
                    m_PlateCounter = m_PlateCounter + 1                 ' �v���[�u�`�F�b�N�@�\�p������J�E���^�X�V V1.23.0.0�F
                    r = TrimmingBlockLoop(stgx, stgy,
                                              zStepPos, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet,
                                              typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir,
                                              typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir,
                                              digH, digL, bLoaderNG, strLogDataBuffer, bFgAutoMode)
                    '-----  ###173 -----
                    ''V6.0.1.022���ʏ�̑��x��]������B 
                    '----- V2.0.0.0_29�� -----
                    ' X,Y�����x��ʏ푬�x�ɐ؂�ւ���(SL436S��)
                    'If (giMachineKd = MACHINE_KD_RS) Then
                    'Dim r2 As Integer
                    'r2 = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, 0).FL, stPclAxisPrm(AXIS_X, 0).FH, stPclAxisPrm(AXIS_X, 0).DrvRat, stPclAxisPrm(AXIS_X, 0).Magnif)
                    ''V4.1.0.0�L �ʐM�G���[�ȊO�����Ȃ��̂ɂQ����ʂɌ���K�v�͂Ȃ�          r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' �G���[�Ȃ�A�v�������I��(���b�Z�[�W�\���ς�)
                    ''V4.1.0.0�LIf (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
                    'r2 = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, 0).FL, stPclAxisPrm(AXIS_Y, 0).FH, stPclAxisPrm(AXIS_Y, 0).DrvRat, stPclAxisPrm(AXIS_Y, 0).Magnif)
                    'r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' �G���[�Ȃ�A�v�������I��(���b�Z�[�W�\���ς�)
                    'If (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
                    '    SimpleTrimmer.BlockNoBtnVisible(False)           'V4.1.0.0�Q
                    'End If
                    ''V6.0.1.022���ʏ�̑��x��]������B 
                    SetXYStageSpeed(StageSpeed.NormalSpeed)
                    ''V6.0.1.022��

                    '----- V2.0.0.0_29�� -----
                    If (r = cFRS_NORMAL) Then
                        Call DspNGTrayChk()                             ' NG�g���C�ւ̔r�o�����M����OFF��҂�
                        'Call SimpleTrimmer.SetPlateEnd()                ' V2.0.0.0�I ��I����Ԃ̐ݒ�
                        'If gKeiTyp = KEY_TYPE_RS Then                   ' V2.0.0.0�I �V���v���g���}�̏ꍇ
                        '    SimpleTrimmer.PlateTrimmingEnd()            ' V2.0.0.0�I �P�������̏���
                        'End If                                          ' V2.0.0.0�I
                    End If
                    'V4.3.0.0�F�^�C�~���O�ύX
                    ''����I���łȂ��Ƃ����u���b�N�f�[�^��\�������� 
                    If gKeiTyp = KEY_TYPE_RS Then                       ' V2.0.0.0�I �V���v���g���}�̏ꍇ
                        SimpleTrimmer.BlockNoBtnVisible(False)          ' V4.1.0.0�Q
                        Call SimpleTrimmer.SetPlateEnd()                ' V2.0.0.0�I ��I����Ԃ̐ݒ�
                        SimpleTrimmer.PlateTrimmingEnd()                ' V2.0.0.0�I �P�������̏���
                    End If                                              ' V2.0.0.0�I
                    ''----- ###173�� -----
                    'V4.3.0.0�F�^�C�~���O�ύX
                    '----- ###142�� -----
                    ' NG��R����NG����(0-100%)�𒴂�����A��P�ʂ̃g���~���O���ʂɃg���~���ONG��ݒ肷��(SL436R�p)
                    If (bLoaderNG = True) Then                          ' NG��R����NG����(0-100%)�𒴂��� ?
                        m_lTrimResult = cFRS_TRIM_NG                    ' ��P�ʂ̃g���~���O���� = �g���~���ONG
                    End If
                    '----- ###142�� -----

                    '----- V1.22.0.0�C�� -----
                    ' �T�}���[���O�t�@�C���o��(x0,x1,x2���[�h��)
                    'strMSG = CStr(Today) & " " & CStr(TimeOfDay)
                    strMSG = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")       'V4.4.0.0-0
                    stSummaryLog.strEndTime = strMSG
                    SummaryLoggingWrite(digH, digL)
                    '----- V1.22.0.0�C�� -----

                    '----- V6.0.3.0�F��(�J�b�g�I�t�����@�\) -----
                    If gAdjustCutoffFunction = 1 And (giMachineKd = MACHINE_KD_RS) And (bFgAutoMode = True) Then ' V6.0.3.0�S
                        ' �������A�܂��͒����ς݁A�u���b�N������������̏ꍇ�̓t�@�C���ۑ�����
                        If stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC OrElse stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_ALREADY Then
                            '-----  V6.0.3.0_24 -----
                            Dim rt2 As Integer
                            stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_FINISHED
                            rt2 = File_Save(typPlateInfo.strDataName)    ' �g�����f�[�^������
                            If rt2 <> cFRS_NORMAL Then
                                '----- V6.0.3.0_24�� -----
                                MsgBox("�t�@�C���̕ۑ��Ɏ��s���܂����B" + typPlateInfo.strDataName)
                            End If
                        End If
                    End If
                    '----- V6.0.3.0�F�� -----

                    ' ���M���O�X�^�[�g�t���O���`�F�b�N����B
                    If gSysPrm.stLOG.giLoggingMode = 1 Then             ' ���O�o�͂��� ?
                        '#4.12.2.0�C                        Call TrimLogging_Write(strLogDataBuffer)        ' ���O�o�� 
                        TrimLogging_Write(strLogDataBuffer.ToString())  ' ���O�o��      '#4.12.2.0�C 
                        '#4.12.2.0�C                        strLogDataBuffer = ""                           ' ���M���O�o�b�t�@�[������ 
                        strLogDataBuffer.Length = 0                     ' ���M���O�o�b�t�@�[������'#4.12.2.0�C 
                    End If

                    ' �g���~���O���ʂ��`�F�b�N���� 
                    If (r = cFRS_ERR_EMG) Then
                        GoTo STP_ERR_EXIT                               ' �A�v�������I��
                    ElseIf (r = cFRS_ERR_AIR) Then
                        GoTo STP_ERR_EXIT                               ' �A�v�������I��
                    ElseIf (r = cFRS_NORMAL) Or (r = cFRS_ERR_START) Then
                        ' ����I��- ���O��ʂɏI�����t������\������
                        'Call Form1.Z_PRINT("END " & CStr(Today) & " " & CStr(TimeOfDay) & vbCrLf)  ' V1.22.0.0�C
                        Call Form1.Z_PRINT("END " & strMSG & vbCrLf)                             ' V1.22.0.0�C
                    ElseIf (r = cFRS_ERR_RST) Then
                        'Call Form1.Z_PRINT("RESET END " & CStr(Today) & " " & CStr(TimeOfDay) & vbCrLf)' V1.22.0.0�C
                        Call Form1.Z_PRINT("RESET END " & strMSG & vbCrLf)                           ' V1.22.0.0�C
                        rtnCode = cFRS_ERR_RST                          ' Return�l = Cancel(RESET��)
                        GoTo STP_TRIM_END                               ' �I��������
                        '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
                        'ElseIf (r = cFRS_ERR_LDR3) Then                     ' ���[�_�A���[��(�y�̏�)(SL436R��)�Ȃ珈�����s ###073
                        '    '----- ###196�� -----
                        '    'ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDRTO) Then
                        '    '    GoTo STP_TRIM_END                          ' ���[�_�A���[��(�y�̏�ȊO)�͏I��������(SL436R��) ###073
                        'ElseIf (r = cFRS_ERR_LDR2) Then                     ' ���[�_�A���[��(�T�C�N����~)(SL436R��)�Ȃ珈�����s

                        'ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDRTO) Then
                        '    GoTo STP_TRIM_END                               ' ���[�_�A���[��(�y�̏�,�T�C�N����~�ȊO)�͏I��������(SL436R��) ###073
                        '    '----- ###196�� -----
                    ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                        '                                               ' ���[�_�A���[�����o(SL436R �����^�]��) ? 
                        giErrLoader = r                                 ' ���[�_�A���[�����o
                        Call SetLoaderIO(&H0, LOUT_SUPLY)               ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                        Call W_RESET()                                  ' �A���[�����Z�b�g 
                        GoTo STP_TRIM_END                               ' �I��������
                    ElseIf (r = cFRS_ERR_LDRTO) Then                    ' ���[�_�ʐM�^�C���A�E�g  ?
                        giErrLoader = r                                 ' ���[�_�A���[�����o
                        Call SetLoaderIO(&H0, LOUT_SUPLY)               ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                        GoTo STP_TRIM_END                               ' �I��������
                    ElseIf (r = cFRS_ERR_LOTEND) Then                   ' LotEnd 
                        giErrLoader = cFRS_NORMAL                       ' ���b�g�I���͐���Ƃ���
                        rtnCode = cFRS_NORMAL
                        bFgAllMagagin = True
                        Call SetLoaderIO(&H0, LOUT_SUPLY)               ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n))
                        GoTo STP_TRIM_END                               ' �I��������
                        '----- V4.11.0.0�E�� -----
                        '----- ###229�� -----
                        ' GPIB�n�G���[�Ȃ�A�v�������I�����Ȃ��ŏI��������
                    ElseIf (System.Math.Abs(r) >= ERR_GPIB_PARAM) And (System.Math.Abs(r) <= ERR_GPIB_EXEC) Then
                        Call Form1.Z_PRINT("GPIB ERROR " & vbCrLf)
                        GoTo STP_TRIM_END                               ' �I��������
                        '----- ###229�� -----
                    Else
                        GoTo STP_ERR_EXIT                               ' �A�v�������I��'###032
                    End If

STP_PTN_NG:         '                                                   ' ###146
                    ' �f�W�X�C�b�`�̓ǎ�� V6.0.3.0�E
                    Call Form1.GetMoveMode(digL, digH, digSW)

                    ' ������̕\�� ###093
                    blnCheckPlate = CheckPlate(False, digL)             ' ###145 (blnCheckPlate��False�łȂ��Ɗ�������J�E���g����Ȃ�) 

                    ' �S�}�K�W���I���`�F�b�N(SL436R��)
                    If (bFgAllMagagin = True) Then                      ' �S�}�K�W���I���H
                        ' ���[�_�֍ŏI��̎�o�v���𑗐M����
                        bIniFlg = 3
                        r = Loader_WaitTrimStart(Form1.System1, bFgAutoMode, m_lTrimResult, bFgMagagin, bFgAllMagagin, bFgLot, bIniFlg)
                        '----- V4.0.0.0�R�� -----
                        ' LotEnd�Ȃ�Return�l=����Ƃ��� 
                        If (r = cFRS_ERR_LOTEND) Then
                            r = cFRS_NORMAL
                        End If
                        '----- V4.0.0.0�R�� -----
                        Call Form1.System1.AutoLoaderFlgReset()         ' �I�[�g���[�_�@�t���O���Z�b�g ###099
                        If (r <> cFRS_NORMAL) Then                      ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                            rtnCode = r                                 ' Return�l�ݒ� 
                            If (r = cFRS_ERR_RST) Then                  ' RESET�L�[(SL436R �����^�]��) ?
                                GoTo STP_TRIM_END                       ' �I��������
                            ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                                '                                       ' ���[�_�A���[�����o(SL436R �����^�]��) ? ###073
                                giErrLoader = r                         ' ���[�_�A���[�����o ###073
                                Call W_RESET()                          ' �A���[�����Z�b�g 
                                GoTo STP_TRIM_END                       ' �I��������
                            ElseIf (r = cFRS_ERR_LDRTO) Then            ' ���[�_�ʐM�^�C���A�E�g  ?
                                giErrLoader = r                         ' ���[�_�A���[�����o V1.16.0.0�H
                                GoTo STP_TRIM_END                       ' �I��������
                            Else                                        ' ���̑��ُ̈�I�����x���̃G���[ 
                                GoTo STP_ERR_EXIT                       ' �A�v�������I����
                            End If
                        End If

                        '----- V1.23.0.0�D�� -----
                        ' �}�K�W����������܂ő҂�
                        r = Sub_MagazineDownCheck(Form1.System1)
                        'Call System.Threading.Thread.Sleep(10 * 1000)  ' Wait(msec)���������͑S�}�K�W�������ɂȂ�܂ő҂� V1.23.0.0�D 
                        '----- V1.23.0.0�D�� -----
                        Exit For                                        ' �I��������(�t�@�C�����[�h�̃��[�v�𔲂���)
                    End If

                    ' �}�K�W���I���`�F�b�N(SL436R��) ���G���h���X���[�h����1�t�@�C���őS�}�K�W������������ׁA����
                    If (giActMode = MODE_MAGAZINE) And (bFgMagagin) Then ' �}�K�W�����[�h�� �}�K�W���I���H
                        Exit Do                                         ' �g���~���O�̃��[�v�𔲂��Ď��t�@�C���̃��[�h������
                    End If

                    ' ���b�g�ؑւ��`�F�b�N(SL436R��)
                    If (giActMode = MODE_LOT) And (bFgLot) Then         ' ���b�g���[�h�� ���b�g�ؑւ��v������H
                        Exit Do                                         ' �g���~���O�̃��[�v�𔲂��Ď��t�@�C���̃��[�h������
                    End If

                    ' �g���~���ONG ?
                    If (bFgAutoMode) And (bLoaderNG = True) Then        ' �g���~���ONG ?
                        bLoaderNG = False                               ' �g���~���ONG�t���OOFF
                    End If

                    '----- ###153�� -----
                    ' ���O��ʕ\���N���A(�����^�]��) 
                    If (bFgAutoMode) Then                               ' �����^�] ? 
                        gDspCounter = gDspCounter + 1                   ' ���O��ʕ\��������J�E���^�X�V
                        FormLotEnd.Processed += 1                       ' ���������                ' V6.0.3.0_21
                        If (gDspCounter > gDspClsCount) Then            ' �J�E���^ > ���O��ʕ\���N���A�����
                            Call Form1.Z_CLS()                          ' ���O��ʃN���A
                            gDspCounter = 1                             ' ���O��ʕ\��������J�E���^�Đݒ�
                        End If
                    End If
                    '----- ###153�� -----

                Loop While (bFgAutoMode)                                ' �����^�]���͉��(���[�_�֊�v���𑗐M���A���[�_����̃g���~���O�X�^�[�g�M���҂���)


                ' INtime���̃��[�N���������
                Call TRIMEND()

                ' ���Ԃ��Đݒ�
                'V4.3.0.0�E�@Call GETSETTIME()                                       ' CMOS��؂��Windows�����Ԃ�ݒ肷��

                ' �z�������������(CHIP�̂�)
                'Erase tDelayTrimNgCnt                                  'V1.23.0.0�E
STP_NEXT_FILE:
            Next FileNum                                                ' ���̃t�@�C���̃��[�h������(�����^�]��)

STP_TRIM_END:
            ' INtime���̃��[�N���������
            Call TRIMEND()

            If gKeiTyp = KEY_TYPE_RS Then
                GroupBoxEnableChange(True)
                Clear113Bit() 'V4.11.0.0�G
                'V4.11.0.0�J
                If IsNothing(gObjMSG) <> True Then
                    gObjMSG.MsgClose()
                    gObjMSG = Nothing
                End If
                'V4.11.0.0�J

            End If

            '--------------------------------------------------------------------------
            '   Z��ҋ@�ʒu�ֈړ�����
            '--------------------------------------------------------------------------
            r = Form1.System1.EX_PROBOFF(gSysPrm)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '              ' �G���[�Ȃ�A�v�������I����(���G���[���b�Z�[�W�͕\���ς�) 

            ' ����~���݂̊m�F
            If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

            ' 'V1.13.0.0�K�@��
            '            If typPlateInfo.intIDReaderUse = 1 Then
            If (typPlateInfo.intIDReaderUse = 1) And (gSysPrm.stLOG.giLoggingMode = 1) Then  ''V1.13.0.1�B
                Dim DirX As Integer = 1, DirY As Integer = 1
                Dim OffPosX As Double, OffPosY As Double
                Dim IdData As String = ""
                TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)
                frmIDReaderTeach.ObjOmronIDReader.GetIDReaderPotision(gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, typPlateInfo.dblTableOffsetXDir, typPlateInfo.dblTableOffsetYDir, gfCorrectPosX, gfCorrectPosY, OffPosX, OffPosY)
                OffPosX = typPlateInfo.dblIDReadPos1X * DirX + OffPosX
                OffPosY = typPlateInfo.dblIDReadPos1Y * DirY + OffPosY
                r = Form1.System1.EX_SMOVE2(gSysPrm, OffPosX, OffPosY)
                If (r < cFRS_NORMAL) Then                               ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                    GoTo STP_ERR_EXIT                                   ' �A�v�������I����
                End If
                If frmIDReaderTeach.ObjOmronIDReader.IDRead(1, IdData) Then
                    IDReadName = IdData
                    r = IDReadName.Length - 1
                    strMSG = IDReadName.Substring(0, r)
                    strMSG = gSysPrm.stLOG.gsLoggingDir + strMSG + ".log"
                    FileCopy(globalLogFileName, strMSG)
                Else
                    ''V1.13.0.1�A
                    strMSG = Today.ToString("yyyyMMdd") & "_" & TimeOfDay.ToString("HHmmss")
                    strMSG = gSysPrm.stLOG.gsLoggingDir + strMSG + ".log"
                    FileCopy(globalLogFileName, strMSG)
                    ''V1.13.0.1�A
                    IDReadName = ""
                End If
            End If
            ' 'V1.13.0.0�K�@��

            '###161 �X�e�[�W���_�ړ��̌�Ɉړ�
            '' �c���菜�����b�Z�[�W�\��(�����^�]��Cancel(RESET��)������)���̓��[�_�A���[�����o�� ###124
            'If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
            '    If (rtnCode = cFRS_ERR_RST) Then
            '        ' �����^�]���~���b�Z�[�W�\��
            '        r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_RSTAUTO)  ' START�L�[�����҂���ʕ\��
            '    Else
            '        ' �c���菜�����b�Z�[�W�\��
            '        r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_WKREMOVE) ' START�L�[�����҂���ʕ\��
            '    End If
            '    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            'End If

            '----- ###222�� -----
            '--------------------------------------------------------------------------
            '   �X�e�[�W���_�ړ����n���h���������Ă���ꍇ�n���h���㏸������(SL436R�����^�]�ŃA���[��������)
            '--------------------------------------------------------------------------
            If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
                W_HAND1_UP()                                            ' �����n���h
                W_HAND2_UP()                                            ' ���[�n���h
            End If
            '----- ###222�� -----

            '--------------------------------------------------------------------------
            '   �X�e�[�W���_�ړ�
            '--------------------------------------------------------------------------
            Dim bInitStage As Boolean
            bInitStage = False

            ' XY_SLIDE��������w�莞
            If gSysPrm.stTMN.giOnline = 2 Then
                '----- V1.22.0.0�I�� -----
                r = INTERLOCK_CHECK(InterlockSts, Sw)                   ' �C���^�[���b�N��Ԏ擾
                ' �����^�]�ŃC���^���b�N�������܂��͎蓮�^�]���͌��_�ړ�FLG�𗧂Ă�
                If ((giHostMode = cHOSTcMODEcAUTO) And (InterlockSts <> INTERLOCK_STS_DISABLE_NO)) Or (giHostMode = cHOSTcMODEcMANUAL) Then
                    ' �X�e�[�W���_�ړ�
                    bInitStage = True
                End If

                '' XY_SLIDE��������  ' InterLockSwRead()���T�|�[�g�̂��ߏC���K�v
                'If giHostMode = cHOSTcMODEcAUTO And (Form1.System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) <> 0 Or giHostMode = cHOSTcMODEcMANUAL Then
                '    ' �X�e�[�W���_�ړ�
                '    bInitStage = True
                'End If
                '----- V1.22.0.0�I�� -----
            ElseIf gSysPrm.stTMN.giOnline = 1 Then                      ' XY_SLIDE��������
                ' �X�e�[�W���_�ړ�
                bInitStage = True
            End If

            ' ���̏��������܂��������A2�ӏ��L�q���͗ǂ��H
            If True = bInitStage Then
                If r <> cGMODE_LDR_MNL Then
                    '----- V1.18.0.0�H�� -----
                    ' SL436R�����^�]���Ŏ����^�]���s�w���҂��̏ꍇ�͂����ł�XY�X�e�[�W�̌��_�ړ����Ȃ�
                    ' 'V2.0.0.0�S                   If ((gSysPrm.stCTM.giSPECIAL = customROHM) And (bFgAutoMode = True) And (giAutoModeContinue = 1) And _
                    If ((bFgAutoMode = True) And (giAutoModeContinue = 1) And (giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Then  'V2.0.0.0�S

                    Else
                        ' XY�X�e�[�W�̌��_�ړ�(��񌴓_�ʒu�ɃX�e�[�W���ړ�����)
                        '----- V2.0.0.0�G�� -----
                        r = Form1.System1.XYtableMove(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                        'V5.0.0.6�A                        r = Form1.System1.XYtableMove(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                        'r = Form1.System1.XYtableMove(gSysPrm, 0.0#, 0.0#)
                        '----- V2.0.0.0�G�� ----
                        If (r <> cFRS_NORMAL) Then                          ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                            GoTo STP_ERR_EXIT                               ' �A�v�������I����
                        End If
                    End If

                    '' XY�X�e�[�W�̌��_�ړ�
                    'r = Form1.System1.XYtableMove(gSysPrm, 0.0#, 0.0#)
                    'If (r <> cFRS_NORMAL) Then                          ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                    '    GoTo STP_ERR_EXIT                               ' �A�v�������I����
                    'End If
                    '----- V1.18.0.0�H�� -----
                End If
            End If

            '--------------------------------------------------------------------------
            '       THETAPARAM=�i0:�Ƃ����_�ɖ߂��Ȃ��i�W���j,1:�Ƃ����_�ɖ߂��j
            '       THETAPARAM�́u�ʒu�␳���[�h=1(�蓮)�ŕ␳���@=0(�␳)�Ȃ��v���L��
            '       �E�ʒu�␳���[�h=0(����),2(����+����)�̎��͖������ɃƂ����_�ɖ߂�
            '       �E�ʒu�␳���[�h=1(�蓮)��, �␳���@=1(1��̂�)�͌��_�ɖ߂��Ȃ�
            '                                            2(����)�͌��_�ɖ߂� ###233
            '--------------------------------------------------------------------------
            If (gSysPrm.stDEV.giTheta <> 0) Then                        ' �ƗL�� ?
                If (typPlateInfo.intReviseMode = 0) Or (typPlateInfo.intReviseMode = 2) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 2))) Then
                    '###233  ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1)) Then
                    Call ROUND4(0.0#)                                   ' �Ƃ����_�ɖ߂�
                    '----- V1.19.0.0-32   �� -----
                    gfCorrectPosX = 0.0                                 ' �␳�l������ 
                    gfCorrectPosY = 0.0
                    '----- V1.19.0.0-32   �� -----
                End If
            End If

            '----- ###161�� -----
            ' �c���菜�����b�Z�[�W�\��(�����^�]��Cancel(RESET��)������)���̓��[�_�A���[�����o�� ###124
            If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
                If (rtnCode = cFRS_ERR_RST) Then
                    ' �����^�]���~���b�Z�[�W�\��
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_RSTAUTO)  ' START�L�[�����҂���ʕ\��
                Else
                    ' �c���菜�����b�Z�[�W�\��
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_WKREMOVE) ' START�L�[�����҂���ʕ\��
                End If
                If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            End If
            '----- ###161�� -----

            ' �J�o�[�J�ネ�[�_���蓮���[�h�ɂ����ꍇ�A���_���A�ɐؑւ���
            If r = cGMODE_LDR_MNL Then
                r = Form1.System1.Form_Reset(cGMODE_ORG, gSysPrm, giAppMode, False, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r <= cFRS_ERR_EMG) Then
                    ' �����I��
                    Call Form1.AppEndDataSave()
                    Call Form1.AplicationForcedEnding()
                End If

                ' �����J�����֐؂�ւ�
                Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)

                ' �C���^�[���b�N��Ԃ̕\��/��\��
                Call Form1.DispInterLockSts()
            End If

            ' ����~���݂̊m�F
            If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET�����vOFF
            GoTo STP_EXIT                                               ' �I��������

            '---------------------------------------------------------------------------
            '   �G���[������
            '---------------------------------------------------------------------------
            ' ����~���o�� 
STP_EMERGENCY:
            r = Sub_CallFrmRset(Form1.System1, cGMODE_EMG)              ' ����~���b�Z�[�W�\��
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALT�����vOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET�����vOFF

            'If (r <= cFRS_ERR_EMG) Then                                ' ����~���̃G���[�Ȃ�A�v�������I��
            If (r = cFRS_ERR_EMG) Then                                  ' ����~���̃G���[�Ȃ�A�v�������I��
                GoTo STP_ERR_EXIT                                       ' �A�v�������I����
            End If
            GoTo STP_EXIT                                               ' �I�������� 

            ' �g���}�G���[�M�������[�_�֑��M����(SL432R�n�p) 
STP_TRIM_ERR:
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ? 
                ' ���[�_�[�o��(ON=�g���~���O�m�f,OFF=�Ȃ�)
                Call SetLoaderIO(COM_STS_TRM_NG, &H0)                   ' ���[�_�o��(ON=�g���~���O�m�f, OFF=�Ȃ�) ###035
            End If

            '' �V�O�i���^���[����(On=�ُ�+�޻ް1, Off=�S�ޯ�) ###007
            'Select Case (gSysPrm.stIOC.giSignalTower)
            '    Case SIGTOWR_NORMAL                                 ' �W��(�ԓ_��+�u�U�[�P)
            '        r = Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            '    Case SIGTOWR_SPCIAL                                 ' ����(�ԓ_��+�u�U�[�P)
            '        r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            'End Select

            ' �G���[�����̃��O��ʏo��("�g���~���O���s���ɃG���[���������܂����B")
            Call Form1.Z_PRINT(MSG_SPRASH23)

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
STP_EXIT:
            '----- V1.14.0.0�D�� -----
            '' CHIP�̂݁F�i090703�Fminato)
            'If gSysPrm.stSPF.giLedOnOff = 1 Then
            '    Call Form1.System1.Set_Led(0, 1, 0)                    ' �o�b�N���C�g�Ɩ��n�m�^�n�e�e(LED����)
            'End If
            '----- V1.14.0.0�D�� -----

            If gKeiTyp = KEY_TYPE_RS Then
                ''V4.3.0.0�F
                ''V4.1.0.0�L�^�C�~���O�ύX
                ' ''����I���łȂ��Ƃ����u���b�N�f�[�^��\�������� 
                'If gKeiTyp = KEY_TYPE_RS Then                   ' V2.0.0.0�I �V���v���g���}�̏ꍇ
                '    Call SimpleTrimmer.SetPlateEnd()                ' V2.0.0.0�I ��I����Ԃ̐ݒ�
                '    SimpleTrimmer.PlateTrimmingEnd()            ' V2.0.0.0�I �P�������̏���
                'End If                                          ' V2.0.0.0�I
                ' ''----- ###173�� -----
                ''V4.1.0.0�L�^�C�~���O�ύX
                'V4.3.0.0�F

                'V4.1.0.0�K
                r = Form1.System1.EX_BSIZE(gSysPrm, 0, 0)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '              ' �G���[�Ȃ�A�v�������I����(���G���[���b�Z�[�W�͕\���ς�) 
                r = BPMaintMove()
                'V4.1.0.0�K
                GroupBoxEnableChange(True)
            End If
            '---------------------------------------------------------------------------
            '   �X���C�h�J�o�[�̃I�[�v�� (SL432R�n��)
            '---------------------------------------------------------------------------
            ' �X���C�h�J�o�[���I�[�v������(�蓮/����)
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then       ' SL432R�n ? 
                '----- V1.13.0.0�G�� -----
                ' ��񌴓_�ʒu�ɃX�e�[�W���ړ�����
                'V5.0.0.6�A                If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                'V5.0.0.6�A                r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                If (GetLoaderBordTableOutPosX() <> 0) Or (GetLoaderBordTableOutPosY() <> 0) Then
                    r = Form1.System1.EX_SMOVE2(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                    If (r < cFRS_NORMAL) Then                           ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                        GoTo STP_ERR_EXIT                               ' �A�v�������I����
                    End If
                End If
                '----- V1.13.0.0�G�� -----

                Call Form1.System1.ReadHostCommand(gSysPrm, giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag) 'V5.0.0.6�F 

                If (gSysPrm.stSPF.giWithStartSw = 1) And (giHostMode <> cHOSTcMODEcAUTO) Then
                    ' �X���C�h�J�o�[�������I�[�v�����Ȃ� (����SW�����҂�(�I�v�V����) �Ń��[�_�����^�]���łȂ��ꍇ)
                    r = Form1.System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, True)
                Else
                    ' �X���C�h�J�o�[�������I�[�v������
                    ' XY_SLIDE�������� ? (0:OFFLINE, 1:ONLINE, 2:SLIDE COVER+XY�ړ�)
                    If gSysPrm.stTMN.giOnline = TYPE_ONLINE Then        ' XY_SLIDE�ʏ퓮��
                        r = Form1.System1.Z_COPEN(gSysPrm, giAppMode, giTrimErr, False)
                    End If
                    If gSysPrm.stTMN.giOnline = TYPE_MANUAL Then        '  XY_SLIDE��������
                        r = Form1.System1.Z_COPEN(gSysPrm, giAppMode, giTrimErr, True)
                    End If
                End If
            Else
                ' �N�����v�y�уo�L���[��OFF(SL436R��) '###001
                If (bFgAutoMode = False) Then                           ' �����^�]���̓N�����v�y�уo�L���[��OFF���Ȃ� ###107
                    r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                End If
                '----- V6.0.3.0_37�� -----
                If gVacuumIO = 1 Then
                    'V4.11.0.0�L
                    If gKeiTyp = KEY_TYPE_RS Then
                        r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                    End If
                    'V4.11.0.0�L
                End If
                '----- V6.0.3.0_37�� -----
            End If

            If (r <= cFRS_ERR_EMG) Then                                 ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                GoTo STP_ERR_EXIT                                       ' �A�v�������I����
            End If

            ' �摜�\���v���O�������I������
            If gKeiTyp = KEY_TYPE_RS Then
                'V6.0.0.0�D                End_SmallGazouProc(ObjGazou)
            Else
                If (Form1.chkDistributeOnOff.Checked = False) Then
                    ' ���v��ʔ�\�����͋N��
                    '' '' ''Form1.VideoLibrary1.Refresh()
                    'V6.0.0.0�D                    End_GazouProc(ObjGazou)
                Else
                    ' ���v��ʕ\�����̓{�^����L���ɖ߂�
                    gObjFrmDistribute.cmdGraphSave.Enabled = True
                    gObjFrmDistribute.cmdInitial.Enabled = True
                    gObjFrmDistribute.cmdFinal.Enabled = True
                End If
            End If

            VacuumeStateCheck() 'V4.11.0.0�G

            '---------------------------------------------------------------------------
            '   ���[�_�蓮�^�]�ؑւ�(SL436R��)
            '---------------------------------------------------------------------------
            r = Loader_ChangeMode(Form1.System1, bFgAutoMode, MODE_MANUAL)
            If (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDRTO) Then
                '                                                       ' ���[�_�A���[�����o(SL436R �����^�]��) ? ###073
                giErrLoader = r                                         ' ���[�_�A���[�����o ###073
                Call W_RESET()                                          ' �A���[�����Z�b�g�M�����o 
                Call W_START()                                          ' �X�^�[�g�M�����o 
                bFgAutoMode = False                                     ' V1.24.0.0�B

            ElseIf (r <= cFRS_ERR_EMG) Then                             ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                GoTo STP_ERR_EXIT                                       ' �A�v�������I����
            End If

            ' ###159 �����^�]�I�����b�Z�[�W�\���̓��[�_���_���A������Ɉړ�
            'r = Loader_EndAutoDrive(Form1.System1, bFgAutoMode_BK)      ' ��bFgAutoMode�͏�L�̃��[�_�蓮�^�]�ؑւ��Ŏ蓮�ɂȂ��Ă���
            'If (r < cFRS_NORMAL) Then                                   ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
            '    GoTo STP_ERR_EXIT                                       ' �A�v�������I����
            'End If

            '---------------------------------------------------------------------------
            '   ���[�_���_���A����(SL436x��)
            '---------------------------------------------------------------------------
STP_LDR_INIT:  ' ###137
            If (gbFgAutoOperation = True) Then                          ' ���[�_���� ? ###137

                ''V5.0.0.1-31��
                'If gKeiTyp = KEY_TYPE_RS Then
                '    If giErrLoader = cFRS_ERR_LDR1 Then
                '        GoTo STP_ERR_EXIT                                   ' �A�v�������I����
                '    End If
                'End If
                ''V5.0.0.1-31��

                '----- ###197�� ----- 
                ' NG�r�oBOX�����t�̏ꍇ�́A��菜�����܂ő҂�(���[�_�����_���A�ł��Ȃ�����)
                'r = NgBoxCheck(Form1.System1, APP_MODE_LOADERINIT)     ' V1.18.0.5�@
                r = NgBoxCheck(Form1.System1, APP_MODE_TRIM)            ' V1.18.0.5�@
                If (r < cFRS_NORMAL) Then
                    GoTo STP_ERR_EXIT                                   ' �A�v�������I����
                End If
                '----- ###197�� ----- 
                '----- V1.16.0.0�H�� ----- 
                ' �S�}�K�W���I��(����I��)���́A�S�}�K�W����������݂̂Łu���[�_���_���A�����v�͍s��Ȃ�
                'If (bFgAllMagagin = True) And (giErrLoader = cFRS_NORMAL) Then                                 ' V1.23.0.0�F
                If (bFgAllMagagin = True) And (giErrLoader = cFRS_NORMAL) And (rtnCode <> cFRS_ERR_RST) Then    ' V1.23.0.0�F

                Else
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_ORG)  ' ���[�_���_���A(�S�}�K�W�������ɂȂ��ƌ��_���A�ُ�ƂȂ�)
                    If r = cFRS_ERR_LDR1 Then
                        GoTo STP_ERR_EXIT
                    End If
                End If
                'r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_ORG)     ' ���[�_���_���A(�S�}�K�W�������ɂȂ��ƌ��_���A�ُ�ƂȂ�)
                '----- V1.16.0.0�H�� ----- 
                Call SetLoaderIO(&H0, LOUT_SUPLY + LOUT_PROC_CONTINUE)  ' ���[�_�o��(ON=�Ȃ�, OFF=��v��(�A���^�]�J�n)) ###111
                '###180
                Call W_RESET()                                          ' �A���[�����Z�b�g�M�����o 
                Call W_START()                                          ' �X�^�[�g�M�����o 
                '###180

                If (r < cFRS_NORMAL) Then                               ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                    '----- ###137��-----
                    Call W_RESET()                                      ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                      ' �X�^�[�g�M�����o 

                    '----- ###179��-----
                    '' "���[�_���_���A������","","START�L�[�F�������s�CRESET�L�[�F�����I��"
                    'r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                    '        MSG_SPRASH37, "", MSG_SPRASH35, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)

                    ' "���[�_���_���A������","","START�L�[����OK�{�^�������Ō��_���A���܂��B"
                    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                            MSG_SPRASH37, "", MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT '       ' �A�v�������I���� 
                    '----- ###179��-----

                    ' START�L�[�����Ȃ�ēx���_���A��
                    If (r = cFRS_ERR_START) Then GoTo STP_LDR_INIT
                    '----- ###137��-----
                    'GoTo STP_ERR_EXIT                                  ' �A�v�������I���� '###073
                End If
            End If

            '' �c���菜�����b�Z�[�W�\��(�����^�]��Cancel(RESET��)������) ###124(���[�_���_���A�����̑O�Ɉړ�)
            '' ���̓��[�_�A���[�����o�� ###073
            'If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
            '    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_WKREMOVE) ' START�L�[�����҂���ʕ\��
            '    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            'End If

            '----- V1.18.0.0�H�� -----
            ' �}�K�W���������b�Z�[�W�\�����Ď����^�]���s���I�����̎w���҂�(SL436R�����^�]�� ���[���a����)
            'V2.0.0.0�S If ((gSysPrm.stCTM.giSPECIAL = customROHM) And (bFgAutoMode_BK = True) And (giAutoModeContinue = 1) And _
            'If ((bFgAutoMode_BK = True) And (giAutoModeContinue = 1) And (giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Then  'V2.0.0.0�S V6.0.3.0_38
            If ((bFgAutoMode_BK = True) And (giAutoModeContinue = 1) And (giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Or
                ((giAutoModeContinue = 1) And (giReqLotSelect = 1)) Then  ' V6.0.3.0_38
                giReqLotSelect = 0                                      ' �}�K�W������ŃX�^�[�g�����Ƃ��Ƀ��b�g�I�����p�����̑I�����郁�b�Z�[�W��\�����邩�ǂ����@V6.0.3.0_38
                r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_MAGAGINE_EXCHG)
                If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT '           ' �G���[�Ȃ�A�v�������I���� 
                If (r = cFRS_ERR_START) Then                            ' ���s�w��(OK�{�^��(START��)����)�Ȃ玩���^�]���s��
                    gbFgContinue = True                                 ' �����^�]�p���t���OON   
                    GoTo STP_CONTINUE
                End If

                ' �t���O���̃N���A
                FormLotEnd.Processed = 0                                ' ����������N���A             V6.0.3.0_21
                QR_Read_Flg = 0                                         ' QR�R�[�h�ǂݍ��݃t���O�̃N���A V6.0.3.0_33     
                gbAutoOperating = False                                 ' �����^�]������s�t���O�N���A V6.0.3.0_31

                ' XY�X�e�[�W�̌��_�ړ�(��񌴓_�ʒu�ɃX�e�[�W���ړ�����)
                '----- V2.0.0.0�G�� -----
                'r = Form1.System1.XYtableMove(gSysPrm, 0.0#, 0.0#)
                'V5.0.0.6�A                r = Form1.System1.XYtableMove(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                r = Form1.System1.XYtableMove(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                '----- V2.0.0.0�G�� ----
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�)
                    GoTo STP_ERR_EXIT                                   ' �A�v�������I����
                End If

                '----- V6.0.3.0�H�� -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    TrimData.SetLotChange()                             ' ���b�g���N���A
                End If
                '----- V6.0.3.0�H�� -----
            End If
            '----- V1.18.0.0�H�� -----

            'V4.6.0.0�A�� 'V4.10.0.0�I gKeiTyp����gMachineType�ɕύX�@And����AndAlso�ɕύX
            If gMachineType = MACHINE_TYPE_436S AndAlso giTimeLotOnly = 1 Then
                SimpleTrimmer.SetLotEnd()
            End If
            'V4.6.0.0�A��

            '----- V6.1.1.0�B��(IAM�a�I�v�V����) -----
            ' �����^�]�I�����Ԃ�\������(SL436R��)
            If ((gMachineType = MACHINE_TYPE_436R) And (giDispEndTime = 1)) Then
                If (bFgAutoMode_BK = True) Then                         ' �����^�] ? 
                    Call DispTrimEndTime(cStartTime, cEndTime)          ' �����^�]�I�����ԕ\��
                End If
            End If
            '----- V6.1.1.0�B�� -----

            '----- ###159 -----
            '---------------------------------------------------------------------------
            '   �����^�]�I�����b�Z�[�W�\��(SL436R��)�����
            '   �V�O�i���^���[����(�S�}�K�W���I��,�����^�]OFF)
            '---------------------------------------------------------------------------
            Call SetTrimEndTime()                                       ' �g���~���O�I�����Ԃ�ݒ肷��(���[���a����) 'V1.18.0.0�B
            r = Loader_EndAutoDrive(Form1.System1, bFgAutoMode_BK)      ' �����^�]�I�����b�Z�[�W�\������уV�O�i���^���[���� (��bFgAutoMode�͏�L�̃��[�_�蓮�^�]�ؑւ��Ŏ蓮�ɂȂ��Ă���)
            If (r < cFRS_NORMAL) Then                                   ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                GoTo STP_ERR_EXIT                                       ' �A�v�������I����
            End If
            '----- V1.18.0.0�B�� -----
            ' �����^�]��(SL436R��)�̓g���~���O���ʂ��������(���[���a����)
            '----- V1.18.0.3�C�� -----
            ' ���[�_�A���[�����y�шꎞ��~��ʂ�RESET��(�y�̏᎞��RESET��)�͈�����Ȃ� 
            'If (bFgAutoMode_BK = True) Then                            ' �����^�] ? 
            If (bFgAutoMode_BK = True) And ((giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Then
                Call PrnTrimResult(0)                                   ' �g���~���O���ʈ��
            End If
            '----- V1.18.0.3�C�� -----
            '----- V1.18.0.0�B�� -----

            'V6.0.1.0�K      ��
            ' �g���~���O���ʕ\���}�b�v�摜���������
            If (digL < 3) AndAlso (rtnCode <> cFRS_ERR_RST) Then
                Form1.PrintTrimMap()
            End If
            'V6.0.1.0�K      ��
            '----- ###159 -----

            '----- V6.0.3.0_37�� �N�����v�y�уo�L���[��OFF  -----
            If gVacuumIO = 1 Then
                'V4.11.0.0�L
                If gKeiTyp = KEY_TYPE_RS Then
                    r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                End If
                'V4.11.0.0�L
            End If
            '----- V6.0.3.0_37�� -----

            ' ���[�_�[�o��(SL432R�̏ꍇ) ###035
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ?
                ' ���[�_�[�o��(ON=�g���~���O�m�f,OFF=�Ȃ�)(SL432R��)
                If bLoaderNG Then
                    Call SetLoaderIO(COM_STS_TRM_NG, &H0)               ' ���[�_�o��(ON=�g���~���O�m�f, OFF=�Ȃ�) ###035
                End If
            Else
                ' ���[�_�[�o��(ON=�g���}����~��,OFF=�Ȃ�)(SL436R��)
                Call SetLoaderIO(LOUT_STOP, &H0)                        ' ���[�_�o��(ON=�g���}����~��, OFF=�Ȃ�)
            End If
            Call Form1.System1.AutoLoaderFlgReset()

            ' �����v��Ԃ̕ύX
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALT�����vOFF
            Call LAMP_CTRL(LAMP_START, False)                           ' START�����vOFF

            ' XYZ�ړ����x�ؑւ�
            '###006            If (Form1.System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0 Then ' InterLockSwRead()���T�|�[�g�̂��ߏC���K�v
            r = INTERLOCK_CHECK(InterlockSts, Sw)                       ' �C���^�[���b�N��Ԏ擾
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' �C���^�[���b�N���(�����Ȃ�)�łȂ� ? ###108
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed
                gPrevInterlockSw = 1
            Else
                Call ZSELXYZSPD(0)                                      ' XYZ Normal speed
                gPrevInterlockSw = 0
            End If

            'V4.7.0.0-21��
            Dim coverSts As Integer
            If (InterlockSts = INTERLOCK_STS_DISABLE_NO) Then           ' �C���^�[���b�N��
                r = COVER_CHECK(coverSts)
                If (r = cFRS_NORMAL And coverSts = 0) Then              ' ➑̃J�o�[�J
                    Call Form1.System1.Form_AxisErrMsgDisp(ERR_OPN_CVR)
                    GoTo STP_ERR_EXIT
                End If
            End If
            'V4.7.0.0-21��

            '----- V6.1.1.0�@�� -----
            ' �g���~���O�I�����Ƀu�U�[��炷(SL432R���̃I�v�V����)
            If (gMachineType = MACHINE_TYPE_432R) Then
                If (rtnCode <> cFRS_ERR_RST) Then                       ' Cancel(RESET��)�ȊO ?
                    r = Sub_Buzzer_On(rtnCode)
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT         ' ����~���̃G���[�Ȃ�A�v�������I����(�G���[���b�Z�[�W�͕\���ς�) 
                End If
            End If
            '----- V6.1.1.0�@�� -----

            ' IDLE���[�h
            giAppMode = APP_MODE_IDLE                                   ' ����Ӱ�� = �g���}���u�A�C�h����
            If (gLoadDTFlag = True) Then
                Call LAMP_CTRL(LAMP_START, True)                        ' START�����vON
            End If

            Form1.txtLog.ShortcutsEnabled = True                        ' ###083 �E�N���b�N���j���[��\������ 
            Form1.SetMapOnOffButtonEnabled(True)                        ' MAP ON/OFF �{�^����L���ɂ���  'V4.12.2.0�@
            Form1.MoveHistoryDataLocation(False)                        'V6.0.1.0�J
            Form1.MoveTrimMapLocation(False)                            'V6.0.1.0�J
            Form1.SetTrimMapVisible(Form1.MapOn)                        'V6.0.1.0�J
            Form1.TimerInterLockSts.Enabled = False                     '  �C���^�[���b�N�X�e�[�^�X�Ď��^�C�}�[��~�@###178
            Console.WriteLine("Trimming() Return code=" + rtnCode.ToString)
            Call SetTrimStartTime()                                     ' V1.18.0.0�B �g���~���O�J�n���Ԃ�ݒ肷��(���[���a����)
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ�������������
            Call Form1.Set_StartBlkNum_Enabled(True)
            '----- V4.11.0.0�D�� -----
            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1�C��

            Return (rtnCode)                                            ' Return�l�ݒ� 

STP_ERR_EXIT:
            ' �A�v�������I��
            Form1.TimerInterLockSts.Enabled = False                     ' �C���^�[���b�N�X�e�[�^�X�Ď��^�C�}�[��~�@###178
            'V6.0.0.0�D           Call End_GazouProc(ObjGazou)                                ' �摜�\���v���O�������I������
            Call Form1.AppEndDataSave()
            Call Form1.AplicationForcedEnding()                         ' �A�v�������I��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.Trimming() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[���� 

        Finally                                         'V6.0.0.0-26
            Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(False)
        End Try
    End Function
#End Region

#Region "�g���~���O�f�[�^��INtime���֑��M����"
    '''=========================================================================
    '''<summary>�g���~���O�f�[�^��INtime���֑��M����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Function SendTrimData() As Short

        Dim i As Short
        Dim j As Short
        Dim wreg As Short
        Dim cut_type As Short
        'Dim mCut1 As Short
        'Dim mCut2 As Short
        Dim r As Short
        Dim RNO As Short
        Dim bpofx As Double
        Dim bpofy As Double
        Dim startpx As Double
        Dim startpy As Double
        Dim lenxy As Double
        Dim dirxy As Short
        Dim lenxy2 As Double
        Dim dirxy2 As Short
        Dim m As Short
        Dim mm As Short
        'Dim Cdir As Short
        Dim intRegIndex As Short
        Dim intTkyType As Short
        Dim strUCutData As String
        Dim strMSG As String

        Try
            ' �ϐ�������
            intTkyType = gTkyKnd                                ' TKY��ʂ̐ݒ�(0:TKY, 1:CHIP, 2:NET)                                                
            lenxy = 0                                           ' BP�͈̓`�F�b�N�ϐ��̏�����
            dirxy = 0
            lenxy2 = 0
            dirxy2 = 0
            giMarkingMode = typPlateInfo.intNGMark              ' �}�[�L���O���[�h(0:���Ȃ�,1:NG,2:OK,3:����)  
            strUCutData = ""                                    ' U�J�b�g�f�[�^��

            ' �T�[�L�b�g���̎擾
            If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                'gwCircuitCount = typPlateInfo.intCurcuitCnt        ' ###012
                gwCircuitCount = typPlateInfo.intCircuitCntInBlock  ' 1��ۯ��໰��Đ��@'###012
                If (gTkyKnd = KND_NET) Then                             '###164
                    gwCircuitCount = typPlateInfo.intGroupCntInBlockXBp '###164 1��ۯ��໰��Đ�
                End If                                                  '###164
            ElseIf (gTkyKnd = KND_CHIP) Then
                gwCircuitCount = 1
            End If
            'Call GetChipNum(wreg)                               ' ��R��
            wreg = typPlateInfo.intResistCntInBlock             ' �P�u���b�N����R��
            'gRegisterExceptMarkingCnt = 0

            If (gTkyKnd = KND_TKY) Then
                ' �T�[�L�b�g�f�[�^�e�[�u��
                If giMarkingMode <> 0 Then
                    For i = 1 To gwCircuitCount
                        gfCircuitX(i) = typCircuitInfoArray(i).dblIP2X
                        gfCircuitY(i) = typCircuitInfoArray(i).dblIP2Y
                    Next
                End If

                'For i = 1 To wreg
                '    If typResistorInfoArray(i).intResNo < 1000 Then
                '        gRegisterExceptMarkingCnt = gRegisterExceptMarkingCnt + 1
                '    End If
                'Next
            End If

            ' �v���[�g�f�[�^��n��
            r = SendTrimDataPlate(intTkyType, wreg, bpofx, bpofy)
            If r <> 0 Then
                SendTrimData = r
                Exit Function
            End If

            '----- V1.18.0.0�F�� -----
            ' GPIB(ADEX�p)��CHIP�ȊO�����M����
            '' CHIP�̂݁F�v���[�g�f�[�^�̐ݒ�
            'If (gTkyKnd = KND_CHIP) Then
            'r = SendTrimDataPlate2(intTkyType, wreg)   ' ###229
            r = SendTrimDataPlate2(intTkyType, wreg, 0) ' ###229
            If r <> 0 Then
                SendTrimData = r
                Exit Function
            End If
            'End If
            '----- V1.18.0.0�F�� -----

            '----- V6.0.3.0�F�� (�J�b�g�I�t�̕ۑ�) -----
            If gAdjustCutoffFunction = 1 And (giMachineKd = MACHINE_KD_RS) Then
                stCutOffAdjust.dblAdjustCutOff = typResistorInfoArray(1).ArrCut(typResistorInfoArray(1).intCutCount).dblCutOff
            End If
            '----- V6.0.3.0�F�� -----

            ' ��R�f�[�^��n��
            bGpib2Flg = 0                                               ' GP-IB����(�ėp)�t���O������(0=����Ȃ�, 1=���䂠��) ###229
            For i = 1 To wreg
                ' ��R�f�[�^��INtime���֑��M����
                r = SendTrimDataRegistor(intTkyType, i, intRegIndex, wreg, RNO)
                If r <> 0 Then
                    SendTrimData = r
                    Exit Function
                End If

                ' �J�b�g�f�[�^�i�e�p�^�[������)��INtime���֑��M����
                For j = 1 To typResistorInfoArray(intRegIndex).intCutCount
                    ' �e�J�b�g�p�^�[�����ʂ̃p�����[�^���M
                    r = SendTrimDataCut(intTkyType, i, j, intRegIndex, startpx, startpy, m, mm, cut_type)
                    If r <> 0 Then
                        SendTrimData = r
                        Exit Function
                    End If

                    ' �J�b�g�p�^�[���ʃf�[�^
                    Select Case cut_type
                        Case 1, 6, 8, 10, 12, 14, 28 ' ST, RETURN/RETRACE + NANAME, ST2
                            r = SendTrimDataCutST(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 2, 7, 9, 11, 13, 15 ' L, (RETURN or RETRACE) + NANAME
                            r = SendTrimDataCutL(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 3 ' HOOK
                            r = SendTrimDataCutHOOK(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 4 ' INDEX
                            r = SendTrimDataCutINDEX(intTkyType, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 5 ' SCAN CUT
                            r = SendTrimDataCutSCAN(intTkyType, m, mm)
                        Case 17, 18 ' U CUT, U CUT(RETRACE) V1.22.0.0�@
                            r = SendTrimDataCutHOOK(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                            If (strUCutData = "") Then          ' U�J�b�g�f�[�^����ޔ� 
                                strUCutData = typResistorInfoArray(m).ArrCut(mm).strDataName
                            End If
                            '----- V1.14.0.0�@�� -----
                        Case 20 ' ES2 CUT(CHIP/NET�p)
                            'r = SendTrimDataCutES(intTkyType, m, mm)
                            r = SendTrimDataCutES0(intTkyType, m, mm)
                        Case 21 'ES CUT(CHIP/NET�p)
                            r = SendTrimDataCutES(intTkyType, m, mm)
                            '----- V1.14.0.0�@�� -----
                        Case 22 ' "M"       SL436K�ł��K�v
                            r = SendTrimDataCutMarking(intTkyType, m, mm, typResistorInfoArray(m).ArrCut(mm).intCutAngle)
                        Case 33 ' INDEX2(�߼޼��ݸޖ������ޯ�����)(CHIP/NET�p)
                            r = SendTrimDataCutINDEX2(intTkyType, m, mm, lenxy, dirxy, lenxy2, dirxy2)

                            'Case 16 ' C CUT �� �폜
                            '    r = SendTrimDataCutC(intTkyType, i, j, m, mm, mCut1)
                            'Case 21 'ES2 CUT(CHIP/NET�p) �� �폜
                            '    r = SendTrimDataCutES2(intTkyType, i, j, m, mm, mCut1)
                            'Case 35 ' Z �� �폜
                            '    r = SendTrimDataCutZ(intTkyType, i, j)
                        Case Else
                            Debug.Print("Error cut_type=" & cut_type)
                            Stop
                    End Select

                    If r <> 0 Then
                        SendTrimData = r
                        Exit Function
                    End If

                    ' ���H�͈͂̃`�F�b�N(OcxSystem���g�p)
                    '----- V2.0.0.0�C�� -----
                    'r = Form1.System1.BpLimitCheck2(gSysPrm, bpofx, bpofy, _
                    '            typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    '            startpx, startpy)
                    '----- V5.0.0.4�B�� -----
                    'r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
                    '            typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    '            typResistorInfoArray(RNO).ArrCut(j).dblStartPointX, typResistorInfoArray(RNO).ArrCut(j).dblStartPointY)
                    'V5.0.0.6�I                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
                    'V5.0.0.6�I                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    'V5.0.0.6�I                                typResistorInfoArray(i).ArrCut(j).dblStartPointX, typResistorInfoArray(i).ArrCut(j).dblStartPointY)
                    '----- V5.0.0.4�B�� -----
                    '----- V2.0.0.0�C�� -----
                    'V5.0.0.6�I���O���J�����J�b�g�ʒu�␳�ʂ̉��Z
                    Dim PosX As Double, PosY As Double
                    PosX = typResistorInfoArray(i).ArrCut(j).dblStartPointX
                    PosY = typResistorInfoArray(i).ArrCut(j).dblStartPointY
                    If giTeachpointUse = 1 Then
                        If (Not GetCutStartPointAddTeachPoint(intRegIndex, j, PosX, PosY)) Then
                            PosX = typResistorInfoArray(i).ArrCut(j).dblStartPointX
                            PosY = typResistorInfoArray(i).ArrCut(j).dblStartPointY
                            Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + i.ToString() + "] CUT=[" + j.ToString() + "]")
                        End If
                    End If
                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir,
                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir,
                                PosX, PosY)
                    'V5.0.0.6�I��
                    If r <> 0 Then
                        '----- V2.0.0.0�C�� -----
                        '' "'Rxx�͉��H�͈͊O�ł��B"
                        'strMSG = "R" & RNO & "=" & MSG_TRIM_01 'V2.0.0.0�C
                        ' "'Rxx�͉��H�͈͊O�ł��B" "�f�[�^���m�F���Ă�������!"
                        strMSG = "R" & typResistorInfoArray(i).intResNo.ToString() + MSG_TRIM_01 + ERR_BP_LIMITOVER.ToString("0")
                        '----- V2.0.0.0�C�� -----
                        Call Form1.Z_PRINT(strMSG)
                        SendTrimData = 16
                        Exit Function
                    End If

                Next j
            Next i

            ' U�J�b�g�p�����[�^�𑗐M����(TKY���ڐA)
            If (gSysPrm.stSPF.giUCutKind <> 0) Then
                r = SendUCutParam(strUCutData)
                If r Then
                    SendTrimData = 17
                    Exit Function
                End If
            End If

            '----- V1.18.0.0�F�� -----
            ' �ėpGPIB�f�[�^�̓f�[�^���[�h��/�f�[�^�ҏW�I�����ɑ��M����
            ''----- ###229�� -----
            '' GPIB2�f�[�^��n��
            'r = SendTrimDataPlate2(intTkyType, wreg, 1) ' ###229
            'If r <> 0 Then
            '    SendTrimData = r
            '    Exit Function
            'End If
            ''----- ###229�� -----
            '----- V1.18.0.0�F�� -----
            'V4.9.0.0�A��
            ' �����l�A�؂�ւ��|�C���g�A�^�[���|�C���g�̃e�[�u����]������B 
            SendTrimDataCutPoint()
            'V4.9.0.0�A��

            SendTrimData = 0

        Catch ex As Exception
            strMSG = "i-TKY.SendTrimData() TRAP ERROR = " + ex.Message
            SendTrimData = 18   '2011/02/4�@�G���[�ԍ��̐ݒ肪�Ȃ����߈ꎞ�I�ɐݒ�
            MsgBox(strMSG)
        End Try
    End Function
#End Region
    '----- V2.0.0.0�C�� -----
#Region "���H�͈͂̃`�F�b�N"
    '''=========================================================================
    ''' <summary>���H�͈͂̃`�F�b�N</summary>
    ''' <param name="DspMd">(INP)���O�\����Ƀ��b�Z�[�W�\������(True). ���Ȃ�(False)</param>
    ''' <param name="sMSG"> (OUT)���b�Z�[�W�ݒ��</param>
    ''' <returns>cFRS_NORMAL  = ���� 
    '''          cFRS_ERR_RST = ���H�͈̓G���[(Cancel(RESET��)�ŕԂ�)</returns>
    '''=========================================================================
    Public Function Bp_Limit_Check(ByVal DspMd As Boolean, ByRef sMSG As String) As Integer

        Dim r As Integer
        Dim Rn As Integer
        Dim Cn As Integer
        Dim strMSG As String

        Try
            ' �S��R�`�F�b�N����
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' �P�u���b�N����R��
                '�I�[�v��/�V���[�g�`�F�b�N�p�̒�R��SKIP 
                If (typResistorInfoArray(Rn).intResNo >= 6000) And (typResistorInfoArray(Rn).intResNo <= 7999) Then
                    GoTo NEXT_REG
                End If

                ' �J�b�g�����`�F�b�N����
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount

                    ' ���H�͈͂̃`�F�b�N(OcxSystem���g�p)
                    'V5.0.0.6�I                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
                    'V5.0.0.6�I                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    'V5.0.0.6�I                                typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX, typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY)
                    'V5.0.0.6�I���O���J�����J�b�g�ʒu�␳�ʂ̉��Z
                    Dim PosX As Double, PosY As Double
                    PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                    PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                    If giTeachpointUse = 1 Then
                        If (Not GetCutStartPointAddTeachPoint(Rn, Cn, PosX, PosY)) Then
                            PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                            PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                            Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + Rn.ToString() + "] CUT=[" + Cn.ToString() + "]")
                        End If
                    End If
                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir,
                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir,
                                PosX, PosY)
                    'V5.0.0.6�I��
                    If (r <> 0) Then
                        ' "'Rxx�͉��H�͈͊O�ł��B" "�f�[�^���m�F���Ă�������!"
                        strMSG = "R" & typResistorInfoArray(Rn).intResNo.ToString() & MSG_TRIM_01 & vbCrLf & ERR_BP_LIMITOVER
                        sMSG = strMSG
                        If (DspMd = True) Then                          ' ���O�\����Ƀ��b�Z�[�W�\������ ?
                            Call Form1.Z_PRINT(strMSG)
                        End If
                        Return (cFRS_ERR_RST)                           ' Return�l = Cancel(RESET��) 
                    End If
                Next Cn
NEXT_REG:
            Next Rn

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.Bp_Limit_Check() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_ERR_RST)                                       ' Return�l = Cancel(RESET��) 
        End Try
    End Function
#End Region
    '----- V2.0.0.0�C�� -----
#Region "�f�B���C�g�����̐ݒ�`�F�b�N"
    '''=========================================================================
    '''<summary>�f�B���C�g�����̐ݒ�`�F�b�N</summary>
    '''<param name="intCutCnt">(INP) �Ώۂ̃J�b�g�ԍ�</param>
    '''<returns>�f�B���C ON�FTrue, OFF�FFalse</returns>
    '''=========================================================================
    Private Function DelayTrimCheck(ByRef intCutCnt As Short) As Boolean

        On Error GoTo ErrExit

        Dim intResCnt As Short
        Dim intForCnt As Short
        Dim blnCheckOk As Boolean
        Dim intSaveCutCnt As Short

        'Call GetChipNum(intResCnt)
        intResCnt = typPlateInfo.intResistCntInBlock                    ' �P�u���b�N����R��
        blnCheckOk = True

        ' INI�t�@�C���̐ݒ������������B
        If gSysPrm.stSPF.giDelayTrim2 = 1 Then

            ' ��R�f�[�^��n��
            For intForCnt = 1 To intResCnt
                ' ڼ�Ӱ�ނł��邩��������B
                If typResistorInfoArray(intForCnt).intTargetValType = 1 Then
                    ' ڼ�Ӱ�ނ̏ꍇ������NG�Ƃ���B
                    blnCheckOk = blnCheckOk And False
                Else
                    ' ڼ�Ӱ�ނłȂ��ꍇ������OK�Ƃ���B
                    blnCheckOk = blnCheckOk And True
                End If

                ' �ێ���Đ���0�̏ꍇ�́A�ŐV�̶�Đ����擾����B
                If intSaveCutCnt = 0 Then
                    intSaveCutCnt = typResistorInfoArray(intForCnt).intCutCount
                Else
                    ' �ێ���Đ���0���傫���ꍇ�ɂ́A���̶�Đ��Ɠ����ł��邩��������B
                    If intSaveCutCnt = typResistorInfoArray(intForCnt).intCutCount Then
                        ' �����ꍇ�ɂ�����OK�Ƃ���B
                        blnCheckOk = blnCheckOk And True
                    Else
                        ' �Ⴄ�ꍇ�ɂ�����NG�Ƃ���B
                        blnCheckOk = blnCheckOk And False
                    End If
                End If
            Next intForCnt

            ' �߂�l��ݒ肷��B
            DelayTrimCheck = blnCheckOk
            intCutCnt = intSaveCutCnt
            '----- V1.23.0.0�E�� -----
            ' �J�b�g�����P�̏ꍇ���ިڲ���2���䖳���Ƃ���
            If (blnCheckOk = True) And (intCutCnt = 1) Then
                DelayTrimCheck = False
            End If
            '----- V1.23.0.0�E�� -----
        Else
            ' �ިڲ���2���䖳���̏ꍇ�ɂ́A����False�Ƃ���B(�ިڲ���2�̏����͍s�Ȃ�Ȃ��B)
            DelayTrimCheck = False
            intCutCnt = 1

        End If

        Exit Function

ErrExit:
        MsgBox("DelayTrimCheck" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Function
#End Region
    '----- V1.18.0.0�F�� -----
#Region "�ėpGP-IB����̗L����Ԃ�"
    '''=========================================================================
    ''' <summary>�ėpGP-IB����̗L����Ԃ�</summary>
    ''' <param name="Gpib2Flg">(OUT)GP-IB����(�ėp)�t���O(0=����Ȃ�, 1=���䂠��)</param>
    ''' <param name="wreg">    (OUT)��R��</param>
    ''' <param name="Type">    (OUT)�^�C�v(0=GPIB, 1=GBIB2)</param>
    '''=========================================================================
    Public Sub Gpib2FlgCheck(ByRef Gpib2Flg As Integer, ByRef wreg As Short, ByRef Type As Integer)

        Dim Rn As Integer
        Dim Cn As Integer
        Dim strMSG As String

        Try
            ' �V�X�p����GP-IB���䂠��(2:�ėp)�łȂ����GP-IB����(�ėp)����Ȃ��Ƃ���
            Gpib2Flg = 0                                                ' GP-IB����(�ėp)�t���O = 0(����Ȃ�)
            Type = 0                                                    ' �^�C�v = 0(GPIB) 
            wreg = typPlateInfo.intResistCntInBlock                     ' �P�u���b�N����R��

            ' �V�X�p����GP-IB���䂠��(2:�ėp)�łȂ����GP-IB����(�ėp)����Ȃ��Ƃ���
            If (gSysPrm.stCTM.giGP_IB_flg <> 2) Then Return
            Type = 1                                                    ' �^�C�v = 1(GPIB2) 

            ' �S��R�`�F�b�N����
            For Rn = 1 To wreg
                If (typResistorInfoArray(Rn).intResNo >= 1000) Then GoTo NEXT_REG
                If (typResistorInfoArray(Rn).intResMeasType = 2) Then   ' ����^�C�v(0:���� ,1:�����x�A�Q�F�O��)
                    Gpib2Flg = 1                                        ' GP-IB����(�ėp)�t���O=1(���䂠��)
                    Return
                End If

                ' �J�b�g�����`�F�b�N����
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount
                    ' IX, IX2�J�b�g�̎��Ƀ`���b�N����
                    strMSG = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()
                    If (strMSG <> DataManager.CNS_CUTP_IX) And (strMSG <> DataManager.CNS_CUTP_IX2) Then
                        GoTo NEXT_CUT
                    End If

                    ' ���蔻��^�C�v 0:����, 1:�����x, 2:�O��)
                    If (typResistorInfoArray(Rn).ArrCut(Cn).intMeasType = 2) Then
                        Gpib2Flg = 1                                    ' GP-IB����(�ėp)�t���O=1(���䂠��)
                        'Return                                         ' V6.1.1.0�G
                        GoTo STP_END                                    ' V6.1.1.0�G
                    End If
NEXT_CUT:
                Next Cn
NEXT_REG:
            Next Rn

            '----- V6.1.1.0�G�� -----
STP_END:
            ' �ݒ�R�}���h���Ȃ�����GP-IB����Ȃ��Ƃ���
            If (typGpibInfo.strI.Trim(" ") = "") And (typGpibInfo.strI2.Trim(" ") = "") And (typGpibInfo.strI3.Trim(" ") = "") And (typGpibInfo.strT.Trim(" ") = "") Then
                Gpib2Flg = 0                                            ' GP-IB����(�ėp)�t���O = 0(����Ȃ�
            End If
            '----- V6.1.1.0�G�� -----

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.Gpib2FlgCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�F�� -----
#Region "���M���O���C������"
    '''=========================================================================
    ''' <summary>���M���O���C������</summary>
    ''' <param name="digH">            (INP)�f�W�^���X�C�b�`��ʐݒ�</param>
    ''' <param name="digL">            (INP)�f�W�^���X�C�b�`���ʐݒ�</param>
    ''' <param name="trimResult">      (INP)�g���~���O���s����</param>
    ''' <param name="pltNoX">          (INP)�v���[�g�ԍ�X</param>
    ''' <param name="pltNoY">          (INP)�v���[�g�ԍ�Y</param>
    ''' <param name="blkNoX">          (INP)�u���b�N�ԍ�X</param>
    ''' <param name="blkNoY">          (INP)�u���b�N�ԍ�Y</param>
    ''' <param name="strLogMsgBuf">    (OUT)���O�f�[�^</param> 
    ''' <param name="contNgCountError">(OUT)</param>
    '''=========================================================================
    Private Function TrimLogging_Main(ByVal digH As Integer, ByVal digL As Integer, ByVal trimResult As Integer,
                                      ByVal pltNoX As Integer, ByVal pltNoY As Integer,
                                      ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                      ByVal strLogMsgBuf As StringBuilder, ByRef contNgCountError As Integer) As Integer
        '#4.12.2.0�C    Private Function TrimLogging_Main(ByVal digH As Integer, ByVal digL As Integer, ByVal trimResult As Integer,
        '#4.12.2.0�C                        ByRef pltNoX As Integer, ByRef pltNoY As Integer,
        '#4.12.2.0�C                        ByRef blkNoX As Integer, ByRef blkNoY As Integer, ByRef strLogMsgBuf As String, ByRef contNgCountError As Integer) As Integer

        '#4.12.2.0�C        Dim tmp_m_intxmode10 As Short
        '#4.12.2.0�C        Dim strFileLogMsg As String
        Static strFileLogMsg As New StringBuilder(4096)     '#4.12.2.0�C
        Dim strLogMsg1 As String
        '#4.12.2.0�C        Dim strDispLogMsg As String
        Static strDispLogMsg As New StringBuilder(8192)     '#4.12.2.0�C
        Dim r As Integer = cFRS_NORMAL                                  ' V1.19.0.0-21

        '#4.12.2.0�C        On Error GoTo ErrExit
        Try                                                 '#4.12.2.0�C
            '#4.12.2.0�C        strFileLogMsg = ""
            strFileLogMsg.Length = 0                        '#4.12.2.0�C
            strLogMsg1 = ""
            '#4.12.2.0�C        strDispLogMsg = ""
            strDispLogMsg.Length = 0                        '#4.12.2.0�C

            'V6.0.1.0�J                  ��
            If (3 <= digL) Then
                Form1.ClearMapBorder()
            End If
            'V6.0.1.0�J                  ��

            ' �޼�SW��1���ڂ��������s�Ȃ��B
            Select Case digL
            ' �~0�FINITIAL TEST+TRIM+FINAL TEST
            ' �~1�FTRIM+FINAL TEST
            ' �~2�FFINAL TEST
                Case 0, 1, 2, 3
                    '===============================================
                    '   ���茋�ʂ̎擾 ��gwTrimResult()
                    '===============================================
                    Call TrimLoggingResult_Get(RSLTTYP_TRIMJUDGE, trimResult)

                    If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                        '�T�[�L�b�gOK/NG�̎擾
                        '(2011/06/25) �擾���K�v���������čēx
                    End If

                    ' x3 ���[�h�ȊO(x0,x1,x2)
                    If (digL <> 3) Then
                        ' NG���W�v
                        If (gTkyKnd = KND_CHIP) Then
                            'Call TrimLogging_NgCont_CHIP(digL)                 ' V1.23.0.0�E
                            'V6.0.1.0�G                            Call TrimLogging_NgCont_CHIP(digL, blkNoX, blkNoY)  ' V1.23.0.0�E
                            Dim ret As Integer = TrimLogging_NgCont_CHIP(digL, blkNoX, blkNoY)       'V6.0.1.0�G
                            ''V6.0.1.0�G      ��  MARUWA�a�d�l
                            'V6.0.1.3�@                            Form1.SetMapColor(blkNoX, blkNoY, Form1.GetResultColor(ret))
                            Form1.SetMapColor(pltNoX, pltNoY, blkNoX, blkNoY, Form1.GetResultColor(ret))    ' �v���[�g�Ή�    'V6.0.1.3�@
                            Form1.CountTrimmingResults(ret)
                            ''V6.0.1.0�G      ��
                        ElseIf (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                            Dim ret As Integer = TrimLogging_NgCont_Net()       'V6.0.1.0�G
                            ''V6.0.1.0�G      ��  MARUWA�a�d�l
                            'V6.0.1.3�@                            Form1.SetMapColor(blkNoX, blkNoY, Form1.GetResultColor(ret))
                            Form1.SetMapColor(pltNoX, pltNoY, blkNoX, blkNoY, Form1.GetResultColor(ret))    ' �v���[�g�Ή�    'V6.0.1.3�@
                            Form1.CountTrimmingResults(ret)
                        End If

                        '----------------------------------------
                        ' �C�j�V�����e�X�g���ʕۑ�
                        'KND-CHIP or KND-NET
                        ' �ިڲ���2���s���͍ŏI��Ă̎�����۸ޏo�́A۸ޕ\�����s�Ȃ��B
                        ' �ިڲ���2�łȂ��ꍇ�ɂ́A����۸ޏo�́A۸ޕ\�����s�Ȃ��B
                        If (gTkyKnd = KND_NET) Or
                           ((gTkyKnd = KND_CHIP) And ((m_blnDelayCheck And m_blnDelayLastCut) Or (Not m_blnDelayCheck))) Then

                            ' �A��NG-HIGH�װ��������
                            ' (x0���[�h�ŘA��NG-HIGH��R��ۯ�����0�ȏ�̎��Ƀ`�F�b�N���� ###129)
                            If (digL = 0) And (typPlateInfo.intContHiNgBlockCnt > 0) Then
                                Call TrimLogging_NgHiCountCheck(contNgCountError)

                                ' �A��NG-HIGH�װ�̏ꍇ�ɂ́A�����𔲂��āA�װ�޲�۸ޕ\���̏������s�Ȃ��B
                                If contNgCountError = CONTINUES_NG_HI Then
                                    Exit Function
                                End If
                            End If                                          ' ###129
                        End If

                        '-----------------------------------------------------------
                        ' �C�j�V�����e�X�g���茋��(����l)�̎擾��gfInitialTest()
                        '-----------------------------------------------------------
                        If (gTkyKnd = KND_CHIP) Then
                            If (Not (m_blnDelayCheck And m_blnDelayLastCut)) Then
                                Call TrimLoggingResult_Get(RSLTTYP_INTIAL_TEST, 0)
                            End If

                        Else
                            Call TrimLoggingResult_Get(RSLTTYP_INTIAL_TEST, 0)
                        End If

                        ''V1.14.0.0�@
                        'If (gESLog_flg = True) And (digL <= 1) Then     'V1.14.0.0�@
                        '    Call TrimLoggingResult_ES()
                        '    Call LoggingWrite_ES()
                        'End If
                        '(2011/06/25) ���L�̏����͕s�v '###012 ����
                        ' �T�[�L�b�g����OK/NG�J�E���g��OK/NG�}�[�L���O���s��(TKY/NET��)
                        If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                            Dim ng As Integer = m_lCircuitNgTotal           'V4.12.2.0�@
                            'Call TrimLoggingCircuit_OKNG(strLogMsg1)       '###012
                            'Call TrimLoggingCircuit_OKNG(digL, strLogMsg1) 'V1.23.0.0�C ###012
                            r = TrimLoggingCircuit_OKNG(digL, strLogMsg1)   'V1.23.0.0�C
                            If (r <> cFRS_NORMAL) Then                      'V1.19.0.0-36 ADD END
                                Return (r)                                  'V1.19.0.0-36 ADD END
                            End If                                          'V1.19.0.0-36 ADD END
#If False Then  ' ���Z���a�d�l
                            'V4.12.2.0�@             ��
                            If (digL < 3) Then
                                If (ng = m_lCircuitNgTotal) Then            ' ���H�I���u���b�N�̔w�i�F�ݒ�
                                    Form1.SetMapColor(blkNoX, blkNoY, TrimMap.ColorOK)
                                    'giMapColorOK = 0        'V4.12.2.0�P 
                                Else
                                    Form1.SetMapColor(blkNoX, blkNoY, TrimMap.ColorNG)
                                    'giMapColorOK = 1        'V4.12.2.0�P 
                                End If
                            End If
                            'V4.12.2.0�@             ��
#End If
                        End If
                    End If

                    '-----------------------------------------------------------
                    ' �t�@�C�i���e�X�g���茋��(����l)�̎擾��gfFinalTest()
                    '-----------------------------------------------------------
                    Call TrimLoggingResult_Get(RSLTTYP_FINAL_TEST, 0)

                    '-----------------------------------------------------------
                    ' ���V�I�ڕW�l���茋�ʂ̎擾��gfTargetVal()
                    '-----------------------------------------------------------
                    Call TrimLoggingResult_Get(RSLTTYP_RATIO_TARGET, 0)

                    '===============================================
                    ' �����}�[�L���O(�}�[�L���O���[�h=3�̏ꍇ)
                    '===============================================
                    r = TrimLoggingMarkingMode_3(digL)
                    If (r <> cFRS_NORMAL) Then                      'V1.19.0.0-36 ADD END
                        Return (r)                                  'V1.19.0.0-36 ADD END
                    End If                                          'V1.19.0.0-36 ADD END

                    '===============================================
                    ' �C�j�V�����e�X�g/�t�@�C�i���e�X�g���z�}����
                    '===============================================
                    'If (Form1.chkDistributeOnOff.Checked = True) Then      
                    If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then     ' ###139 �ꎞ��~��ʂ���̃O���t�\���ׂ̈ɓ��v�͖������Ɏ��(CHIP/NET��)
                        '----- ###150�� -----
                        ' x0���[�h����x1���[�h�Ń��M���O���[�h=3(INITIAL + FINAL)�̏ꍇ�A�C�j�V�����e�X�g���ʏW�v���s��
                        If (digL = 0) Or ((digL = 1) And (gLogMode = 3)) Then
                            ' �C�j�V�����e�X�g���ʏW�v
                            Call TrimLoggingGraph_RegistNumIT()
                        End If

                        ' �t�@�C�i���e�X�g���ʏW�v
                        Call TrimLoggingGraph_RegistNumFT()

                        'If (gObjFrmDistribute.DisplayInitialMode = True) Then   ' �\�����̎��(TRUE:IT FALSE:FT)
                        '    '�C�j�V�����e�X�g���ʕ\����
                        '    Call TrimLoggingGraph_RegistNumIT()
                        'Else
                        '    '�t�@�C�i���e�X�g���ʕ\����
                        '    Call TrimLoggingGraph_RegistNumFT()
                        'End If
                        '----- ###150�� -----
                    End If

                    '===============================================
                    '   ���/�t�@�C�����O�\�����C�������̌Ăяo��
                    '===============================================
                    '#4.12.2.0�E                    Call TrimLogging_LoggingDataMain(digH, digL, strFileLogMsg, strDispLogMsg)
                    TrimLogging_LoggingDataMain(digH, digL, blkNoX, blkNoY, strFileLogMsg, strDispLogMsg)   '#4.12.2.0�E

                    '===============================================
                    '   ���M���O����(���O�\����ʂƐ��Y�Ǘ����)
                    '===============================================
                    If gKeiTyp = KEY_TYPE_RS Then           'V2.0.0.0�I �V���v���g���}
                        Call SimpleTrim_LoggingStart()      'V2.0.0.0�I
                    End If
                    ' V1.23.0.0�E �ŏI�u���b�N����Call����
                    Call TrimLogging_LoggingStart(digH, digL, pltNoX, pltNoY, blkNoX, blkNoY,
                                        strLogMsgBuf, strFileLogMsg, strDispLogMsg)

                ' �~4�F�߼޼��ݸ�����
                Case 4

                ' �~5�F��èݸ�����
                Case 5
                    If (gTkyKnd = KND_CHIP) Then
                        ' �ިڲ���2���s���͍ŏI��Ă̎�����۸ޏo�́A۸ޕ\�����s�Ȃ��B
                        ' �ިڲ���2�łȂ��ꍇ�ɂ́A����۸ޏo�́A۸ޕ\�����s�Ȃ��B
                        If (m_blnDelayCheck And m_blnDelayLastCut) Or (Not m_blnDelayCheck) Then
                            ' �����}�[�L���O  (�}�[�L���O���[�h=3�̏ꍇ)
                            r = TrimLoggingMarkingMode_3(digL)
                            If (r <> cFRS_NORMAL) Then                      'V1.19.0.0-36 ADD END
                                Return (r)                                  'V1.19.0.0-36 ADD END
                            End If                                          'V1.19.0.0-36 ADD END
                        End If
                    Else
                        ' �����}�[�L���O  (�}�[�L���O���[�h=3�̏ꍇ)
                        'Call TrimLoggingMarkingMode_3(digL)                'V1.23.0.0�C
                        r = TrimLoggingMarkingMode_3(digL)                  'V1.23.0.0�C
                        If (r <> cFRS_NORMAL) Then                          'V1.19.0.0-36 ADD END
                            Return (r)                                      'V1.19.0.0-36 ADD END
                        End If                                              'V1.19.0.0-36 ADD END
                    End If

                ' �~6�F�w�xð�������
                Case 6

            End Select

            '#4.12.2.0�C            Return (cFRS_NORMAL)                                            'V1.19.0.0-36
        Catch ex As Exception
            '@@@20170810ErrExit:
            MsgBox("TrimLogging_Main" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

        Return (cFRS_NORMAL)                                            'V1.19.0.0-36

    End Function

#End Region

#Region "NG���W�v�yCHIP�p�z"
    '''=========================================================================
    ''' <summary>NG���W�v</summary>
    ''' <param name="digL">(INP)Dig-SW</param>
    ''' <param name="iBlkX">(INP)�u���b�N�ԍ�X(1-n) V1.23.0.0�E</param>
    ''' <param name="iBlkY">(INP)�u���b�N�ԍ�Y(1-n) V1.23.0.0�E</param>
    ''' <returns>�}�b�v�p�u���b�NOK/NG����l 'V6.0.1.0�G</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function TrimLogging_NgCont_CHIP(ByVal digL As Integer, ByVal iBlkX As Integer, ByVal iBlkY As Integer) As Integer  ' V1.23.0.0�E
        'V6.0.1.0�G    Private Sub TrimLogging_NgCont_CHIP(ByVal digL As Integer, ByVal iBlkX As Integer, ByVal iBlkY As Integer) ' V1.23.0.0�E

        'V6.0.1.0�G        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim BlkX As Integer                                             ' V1.23.0.0�E
        Dim BlkY As Integer                                             ' V1.23.0.0�E

        BlkX = iBlkX - 1                                                ' V1.23.0.0�E
        BlkY = iBlkY - 1                                                ' V1.23.0.0�E

        'V6.0.1.0�G      ��
        Dim ItHi As Integer = m_lITHINGCount
        Dim ItLo As Integer = m_lITLONGCount
        Dim FtHi As Integer = m_lFTHINGCount
        Dim FtLo As Integer = m_lFTLONGCount
        Dim OvRg As Integer = m_lITOVERCount
        Dim TmOk As Integer = 0
        Dim ret As Integer = TRIM_RESULT_NOTDO
        'V6.0.1.0�G      ��

        Try                             'V6.0.1.0�G
            For intForCnt = 1 To gRegistorCnt
                '---------------------------------------------------------------
                '   NG�����W�v����(�f�B���C�g�����Q�p �ŏI�u���b�N�ȊO)
                '---------------------------------------------------------------
                If (m_blnDelayCheck And Not m_blnDelayLastCut) Then
                    If (TRIM_RESULT_IT_HING = gwTrimResult(intForCnt - 1)) Then ' ###202
                        ' V1.23.0.0�E tDelayTrimNgCnt() �� stDelay2.NgAry()�ɕύX
                        ' �Ƽ��ý�HIGHNG�׸ނ�ON�ɂ���B
                        'stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intNGCnt = 1 ' ���g�p�̂��߃R�����g�� V1.23.0.0�E
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITHiNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_IT_LONG = gwTrimResult(intForCnt - 1)) Then
                        ' �Ƽ��ý�LOWNG�׸ނ�ON�ɂ���B
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITLoNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_FT_HING = gwTrimResult(intForCnt - 1)) Then
                        ' ̧���ý�HIGHNG�׸ނ�ON�ɂ���B
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intFTHiNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_FT_LONG = gwTrimResult(intForCnt - 1)) Then
                        ' ̧���ý�LOWNG�׸ނ�ON�ɂ���B
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intFTLoNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_OVERRANGE = gwTrimResult(intForCnt - 1)) Then
                        ' ���ް�ݼ�NG�׸ނ�ON�ɂ���B
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intOverNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    ' ���ݸތ��ʂ�1(����)�ł��邩��������B
                    If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_OK Then
                        ' ����NG���Ȃ��ꍇ�̂�OK�׸ނ�ON�ɂ���B
                        If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalNGCnt = 0 Then
                            stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalOkCnt = 1
                        End If
                    ElseIf gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_NG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE _
                            Or gwTrimResult(intForCnt - 1) = 11 _
                            Or gwTrimResult(intForCnt - 1) = 13 _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_CVERR _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERLOAD _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_ES2ERR _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_MIDIUM_CUT_NG Then     ' 'V1.20.0.1�@ ' V1.13.0.0�J
                        ' NG������ꍇ�ɂ�OK�׸ނ�0�ɖ߂��B
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalNGCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalOkCnt = 0
                    End If

                    ' �Ƽ��ýĂ̌���(����l)��ێ����Ă��� V1.23.0.0�E
                    stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).dblInitialTest = gfInitialTest(intForCnt - 1)

                Else
                    '---------------------------------------------------------------
                    '   NG�����W�v����(�f�B���C�g�����Q�p �ŏI�u���b�N��)
                    '---------------------------------------------------------------
                    ' �ިڲ���2���s���ōŏI��Ă̏ꍇ�̂݁A�Ƽ��ýĂ̌��������A�ݒ���s�Ȃ�
                    If (m_blnDelayCheck And m_blnDelayLastCut) Then
                        ' �ێ����Ă������C�j�V��������(����l)���Đݒ肷��B
                        gfInitialTest(intForCnt - 1) = stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).dblInitialTest

                        If digL = 0 Then
                            ' �Ƽ��ý�HighNG���������Ă�������������B
                            If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITHiNgCnt = 1 Then
                                gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING
                            End If
                            ' �Ƽ��ý�LOWNG���������Ă�������������B
                            If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITLoNgCnt = 1 Then
                                gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG
                            End If
                            '----- V1.23.0.0�E�� -----
                            ' ���ް�ݼނ��������Ă�������������B
                            If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intOverNgCnt = 1 Then
                                gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG
                            End If
                            '----- V1.23.0.0�E�� -----
                        End If
                    End If

                    '---------------------------------------------------------------
                    '   NG�����W�v����
                    '---------------------------------------------------------------
#If False Then
                    If (TRIM_RESULT_IT_HING = gwTrimResult(intForCnt - 1)) Then
                        m_intNgCount = m_intNgCount + 1
                        m_lITHINGCount = m_lITHINGCount + 1
                    End If

                    If (TRIM_RESULT_IT_LONG = gwTrimResult(intForCnt - 1)) Then
                        m_lITLONGCount = m_lITLONGCount + 1
                    End If

                    If (TRIM_RESULT_FT_HING = gwTrimResult(intForCnt - 1)) Then
                        m_lFTHINGCount = m_lFTHINGCount + 1
                    End If

                    If (TRIM_RESULT_FT_LONG = gwTrimResult(intForCnt - 1)) Then
                        m_lFTLONGCount = m_lFTLONGCount + 1
                    End If
                    If (TRIM_RESULT_OVERRANGE = gwTrimResult(intForCnt - 1)) Then
                        m_lITOVERCount = m_lITOVERCount + 1
                    End If
#Else
                    Select Case (gwTrimResult(intForCnt - 1))
                        Case TRIM_RESULT_IT_HING
                            m_intNgCount = m_intNgCount + 1
                            m_lITHINGCount = m_lITHINGCount + 1

                        Case TRIM_RESULT_IT_LONG
                            m_lITLONGCount = m_lITLONGCount + 1

                        Case TRIM_RESULT_FT_HING
                            m_lFTHINGCount = m_lFTHINGCount + 1

                        Case TRIM_RESULT_FT_LONG
                            m_lFTLONGCount = m_lFTLONGCount + 1

                        Case TRIM_RESULT_OVERRANGE
                            m_lITOVERCount = m_lITOVERCount + 1

                        Case TRIM_RESULT_OK
                            TmOk += 1

                        Case Else
                            ' DO NOTHING
                    End Select
#End If
                End If
            Next

            'V6.0.1.0�G      �� MARUWA�a�d�l
            ' ret��Form1._trimmingResult��Key�ɂ��邱��
            If (TmOk = gRegistorCnt) Then
                ret = TRIM_RESULT_OK
            ElseIf (FtHi < m_lFTHINGCount) Then
                ret = TRIM_RESULT_FT_HING
            ElseIf (ItHi < m_lITHINGCount) Then
                ret = TRIM_RESULT_FT_HING
            ElseIf (FtLo < m_lFTLONGCount) Then
                ret = TRIM_RESULT_FT_LONG
            ElseIf (ItLo < m_lITLONGCount) Then
                ret = TRIM_RESULT_FT_LONG
            Else
                ret = TRIM_RESULT_NOTDO
            End If
            'V6.0.1.0�G      ��            Exit Sub

        Catch ex As Exception
            'V6.0.1.0�GErrExit:
            MsgBox("TrimLogging_NgCont" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

        Return ret                      'V6.0.1.0�G

    End Function
#End Region

#Region "NG���W�v�yTKY/NET�p�z"
    '''=========================================================================
    ''' <summary>NG���W�v</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    'Private Sub TrimLogging_NgCont_Net()
    Private Function TrimLogging_NgCont_Net() As Integer        'V6.0.1.0�G      ��

        On Error GoTo ErrExit

        Dim intForCnt As Short

        'V6.0.1.0�G      ��
        Dim m_lPTNNGCount As Integer = 0
        Dim ItHi As Integer = m_lITHINGCount
        Dim ItLo As Integer = m_lITLONGCount
        Dim FtHi As Integer = m_lFTHINGCount
        Dim FtLo As Integer = m_lFTLONGCount
        Dim OvRg As Integer = m_lITOVERCount
        Dim PtnNg As Integer = m_lPTNNGCount
        Dim TmOk As Integer = 0
        Dim ret As Integer = TRIM_RESULT_NOTDO
        Dim NormalRegCnt As Integer = 0
        'V6.0.1.0�G      ��

        For intForCnt = 1 To gRegistorCnt
            ' NGϰ�ݸޏ����łȂ����Ƃ��m�F����B
            If typResistorInfoArray(intForCnt).intResNo < 1000 Then
                NormalRegCnt = NormalRegCnt + 1
            End If
            'If (TRIM_RESULT_IT_HING = gwTrimResult(intForCnt - 1)) Then
            '    m_intNgCount = m_intNgCount + 1
            '    m_lITHINGCount = m_lITHINGCount + 1                     ' IT HI NG��
            'End If

            'If (TRIM_RESULT_IT_LONG = gwTrimResult(intForCnt - 1)) Then
            '    m_lITLONGCount = m_lITLONGCount + 1                     ' IT LO NG��
            'End If

            'If (TRIM_RESULT_FT_HING = gwTrimResult(intForCnt - 1)) Then
            '    m_lFTHINGCount = m_lFTHINGCount + 1                     ' FT HI NG��
            'End If

            'If (TRIM_RESULT_FT_LONG = gwTrimResult(intForCnt - 1)) Then
            '    m_lFTLONGCount = m_lFTLONGCount + 1                     ' FT LO NG��
            'End If

            'If (TRIM_RESULT_OVERRANGE = gwTrimResult(intForCnt - 1)) Then
            '    m_lITOVERCount = m_lITOVERCount + 1                     ' IT���ް�ݼސ�
            'End If

            Select Case (gwTrimResult(intForCnt - 1))
                Case TRIM_RESULT_IT_HING
                    m_intNgCount = m_intNgCount + 1
                    m_lITHINGCount = m_lITHINGCount + 1

                Case TRIM_RESULT_IT_LONG
                    m_lITLONGCount = m_lITLONGCount + 1

                Case TRIM_RESULT_FT_HING
                    m_lFTHINGCount = m_lFTHINGCount + 1

                Case TRIM_RESULT_FT_LONG
                    m_lFTLONGCount = m_lFTLONGCount + 1

                Case TRIM_RESULT_OVERRANGE
                    m_lITOVERCount = m_lITOVERCount + 1

                Case TRIM_RESULT_OK
                    TmOk += 1

                Case Else
                    ' DO NOTHING
            End Select

            '''''2009/07/29 minato
            ''''    ���L��436K�̃R�[�h�B432��INTRTM�ł́A[12][14]���Z�b�g����鎖�͂Ȃ��B
            '        If (TRIM_RESULT_IT_HING <= gwTrimResult(intForCnt - 1) _
            ''            And 12 <> gwTrimResult(intForCnt - 1)) Then  ' NG��R��
            '            m_intNgCount = m_intNgCount + 1
            '        End If
            '
            '        If (14 = gwTrimResult(intForCnt - 1)) Then
            '            glITOVERCount = glITOVERCount + 1
            '        End If
        Next


        'V6.0.1.0�G      �� MARUWA�a�d�l
        ' ret��Form1._trimmingResult��Key�ɂ��邱��
        If (TmOk = NormalRegCnt) Then
            ret = TRIM_RESULT_OK
        ElseIf (OvRg < m_lITOVERCount) Then
            ret = TRIM_RESULT_OVERRANGE
        ElseIf (FtHi < m_lFTHINGCount) Then
            ret = TRIM_RESULT_FT_HING
        ElseIf (ItHi < m_lITHINGCount) Then
            ret = TRIM_RESULT_FT_HING
        ElseIf (FtLo < m_lFTLONGCount) Then
            ret = TRIM_RESULT_FT_LONG
        ElseIf (ItLo < m_lITLONGCount) Then
            ret = TRIM_RESULT_FT_LONG
        Else
            ret = TRIM_RESULT_NOTDO
        End If
        'V6.0.1.0�G      ��            Exit Sub

        Return ret

        Exit Function

ErrExit:
        MsgBox("TrimLogging_NgCont" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Function
#End Region

#Region "�T�[�L�b�g����NG�J�E���g���@OK/NG�}�[�L���O(TKY/NET�p)"
    '''=========================================================================
    ''' <summary>�T�[�L�b�g����NG�J�E���g���@OK/NG�}�[�L���O</summary>
    ''' <param name="digL"></param>
    ''' <param name="strLogMsg1"></param>
    ''' <returns></returns>
    ''' <remarks>TKY��NET�Ŏg�p�����</remarks>
    '''=========================================================================
    Private Function TrimLoggingCircuit_OKNG(ByVal digL As Integer, ByRef strLogMsg1 As String) As Integer

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim intGetCircuit As Short
        Dim intNgJudgeUnit As Short
        Dim r As Integer                                    'V1.19.0.0-36

        ' �T�[�L�b�g����NG���J�E���g���N���A
        For intForCnt = 1 To gwCircuitCount
            gwCircuitNgCount(intForCnt) = 0
        Next

        ' NG����P�ʂ��擾����
        intNgJudgeUnit = typPlateInfo.intNgJudgeUnit                                            ' NG����P�ʎ擾(0:BLOCK, 1:PLATE)		
        If intNgJudgeUnit = 0 Then
            m_intNgCount = 0
        End If

        ' �T�[�L�b�g����NG�����J�E���g
        For intForCnt = 1 To gRegistorCnt
            ' NGϰ�ݸޏ����łȂ����Ƃ��m�F����B
            If typResistorInfoArray(intForCnt).intResNo < 1000 Then
                intGetCircuit = typResistorInfoArray(intForCnt).intCircuitGrp                   ' �T�[�L�b�g�ԍ�
                'If gwTrimResult(intForCnt - 1) = 2 Or gwTrimResult(intForCnt - 1) = 3 Then     ' ###012    
                If gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE _
                        Or gwTrimResult(intForCnt - 1) = 11 _
                        Or gwTrimResult(intForCnt - 1) = 13 _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_CVERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERLOAD _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_ES2ERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_MIDIUM_CUT_NG Then     ' 'V1.20.0.1�@ ' �g���~���O���ʂ�NG ? V1.13.0.0�J ###012  
                    gwCircuitNgCount(intGetCircuit) = gwCircuitNgCount(intGetCircuit) + 1
                    m_intNgCount = m_intNgCount + 1
                End If
            End If
        Next

        ' ����Đ����������s�Ȃ��B
        For intForCnt = 1 To gwCircuitCount
            ' �����NG�����݂��邩��������B
            If gwCircuitNgCount(intForCnt) Then                                                 ' NG��R����ł����邩?
                ' �����NG������ݸ���Ă���B
                m_lCircuitNgTotal = m_lCircuitNgTotal + 1
                '----- ###167�� -----
                m_lNgCount = m_lNgCount + 1                                                     ' NG�J�E���g�X�V
                m_NG_RES_Count = m_NG_RES_Count + 1                                             ' NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��)
                '----- ###167�� -----
                strLogMsg1 = strLogMsg1 & "Circuit=" & intForCnt.ToString("00") & " NG"

                ' �}�[�L���O���[�h(1:NG�}�[�L���O)�ŁA�޼�SW��1���ڂ�0,1,2�ł��邩��������B
                If giMarkingMode = 1 And digL <= 2 Then
                    ' NG�}�[�L���O���s��
                    'Call TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)  '###012
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)   '###012
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"

                ElseIf giMarkingMode = 4 And digL <= 2 Then
                    ' �}�[�L���O���[�h(4:NG/OK�}�[�L���O)�ŁA�޼�SW��1���ڂ�0,1,2�ł��邩��������
                    ' NG�}�[�L���O���s��(1000�`4999���g�p����)
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"
                End If
                strLogMsg1 = strLogMsg1 & Chr(13) & Chr(10)

            Else
                ' �����OK������ݸ���Ă���B
                m_lCircuitGoodTotal = m_lCircuitGoodTotal + 1
                '----- ###167�� -----
                m_lGoodCount = m_lGoodCount + 1                                                 ' OK�J�E���g�X�V
                '----- ###167�� -----
                strLogMsg1 = strLogMsg1 & "Circuit=" & intForCnt.ToString("00") ' +

                ' �}�[�L���O���[�h(2:OK�}�[�L���O)�ŁA�A�޼�SW��1���ڂ�0,1,2�ł��邩��������B
                If giMarkingMode = 2 And digL <= 2 Then
                    ' OK�}�[�L���O���s��(5000�`5999�Ԃ��g�p���ă}�[�L���O)
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"

                ElseIf giMarkingMode = 4 And digL <= 2 Then
                    ' �}�[�L���O���[�h(4:NG/OK�}�[�L���O)�ŁA�޼�SW��1���ڂ�0,1,2�ł��邩��������
                    ' OK�}�[�L���O���s��(5000�`5999�Ԃ��g�p���ă}�[�L���O)
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"
                End If
                strLogMsg1 = strLogMsg1 & vbCrLf
            End If
        Next

        Return (cFRS_NORMAL)

ErrExit:
        MsgBox("TrimLoggingCircuit_OKNG" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
        Return (cFRS_NORMAL)
    End Function
#End Region

#Region "�����}�[�L���O  (�}�[�L���O���[�h=3�̏ꍇ)"
    '''=========================================================================
    '''<summary>�����}�[�L���O  (�}�[�L���O���[�h=3�̏ꍇ)</summary>
    ''' <param name="digL"></param>
    ''' <returns></returns>
    '''=========================================================================
    Private Function TrimLoggingMarkingMode_3(ByVal digL As Integer) As Integer

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim r As Integer 'V1.19.0.0-36

        ' �}�[�L���O���[�h(3:����) ���޼�SW��1���ڂ�0,1,2,5�ł��邩��������B
        If giMarkingMode = 3 And (digL <= 2 Or digL = 5) Then
            ' ����Đ����������s�Ȃ��B
            For intForCnt = 1 To gwCircuitCount
                ' �����}�[�L���O���s��(1000�`5999�Ԃ��g�p���ă}�[�L���O)
                r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)
                'V1.19.0.0-36 ADD START
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then
                    Return (r)
                End If
                'V1.19.0.0-36 ADD END
            Next

        ElseIf giMarkingMode <> 0 And (digL = 5) Then
            '(x0,1,2�́��̕��ŏ����Ă���̂ł����ł�x5����)
            For intForCnt = 1 To gwCircuitCount
                If giMarkingMode = 1 Then
                    ' NG�}�[�L���O���s��(1000�`5999�Ԃ��g�p���ă}�[�L���O)
                    'Call TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)  '###012
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)   '###012
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END

                ElseIf giMarkingMode = 2 Then
                    ' OK�}�[�L���O���s��(5000�`5999�Ԃ��g�p���ă}�[�L���O)
                    'Call TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)  '###012
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)   '###012
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END

                ElseIf giMarkingMode = 4 Then
                    ' NG�}�[�L���O(1000�`4999��)/OK�}�[�L���O(5000�`5999��)���s��
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                End If
            Next
        End If

        Return (cFRS_NORMAL)        'V1.19.0.0-36

ErrExit:
        MsgBox("TrimLoggingMarkingMode_3" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
        Return (cFRS_NORMAL)        'V1.19.0.0-36
    End Function
#End Region

#Region "���茋�ʂ̎擾"
    '''=========================================================================
    '''<summary>���茋�ʂ̎擾</summary>
    ''' <param name="resultType">���ʎ擾�^�C�v</param>
    ''' <param name="trimRes">�g���~���O���ʁi�߂�l�j</param>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TrimLoggingResult_Get(ByVal resultType As Integer, ByVal trimRes As Integer) 'V1.23.0.0�F

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim intSetData As Short
        Dim intForCnt As Short

        ' ��R��/15�̐��������擾����B
        intCntMax = Int(gRegistorCnt / 15)

        ' �ő�l�܂ŏ������s�Ȃ��B
        For intForCnt = 0 To intCntMax
            ' �������ő�l�ł��邩��������B
            If intForCnt < intCntMax Then
                ' �ő�l�̏ꍇ�́A15�ŏ������s�Ȃ��B
                intSetData = 15
            Else
                ' �ő�l�ȊO�̏ꍇ�́A15�ŏ��Z�����]����擾����B
                intSetData = gRegistorCnt Mod 15
            End If

            ' �擾�����l��0�̏ꍇ�ɂ́A�����𔲂���B
            If intSetData = 0 Then Exit For

#If cOFFLINEcDEBUG Then
#Else
            Select Case resultType
                Case RSLTTYP_TRIMJUDGE
                    ' ���茋�ʂ̎擾
                    Call TRIM_RESULT_WORD(resultType, intForCnt * 15, intSetData, 0, 0, gwTrimResult(intForCnt * 15))
                Case RSLTTYP_INTIAL_TEST
                    ' �C�j�V�����e�X�g���ʂ̎擾
                    Call TRIM_RESULT_Double(resultType, intForCnt * 15, intSetData, 0, 0, gfInitialTest(intForCnt * 15))
                Case RSLTTYP_FINAL_TEST
                    ' �t�@�C�i���e�X�g���ʂ̎擾
                    Call TRIM_RESULT_Double(resultType, intForCnt * 15, intSetData, 0, 0, gfFinalTest(intForCnt * 15))
                Case RSLTTYP_RATIO_TARGET
                    ' ���V�I���[�h�ڕW�l�̎擾
                    Call TRIM_RESULT_Double(resultType, intForCnt * 15, intSetData, 0, 0, gfTargetVal(intForCnt * 15))
            End Select
#End If
        Next

        ' ����۰��ݸގ��s
        If (resultType = RSLTTYP_TRIMJUDGE And trimRes = 5) Then
            For intForCnt = 1 To gRegistorCnt
                gwTrimResult(intForCnt - 1) = 13
            Next
        End If

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_Get" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "���V�I�ڕW�l���茋�ʂ̎擾"
    '''=========================================================================
    '''<summary>���V�I�ڕW�l���茋�ʂ̎擾</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingResult_TargetVal()

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim intSetData As Short
        Dim intForCnt As Short

        ' ��R��/15�̐��������擾����B
        intCntMax = Int(gRegistorCnt / 15)

        ' �ő�l�܂ŏ������s�Ȃ��B
        For intForCnt = 0 To intCntMax
            ' �������ő�l�ł��邩��������B
            If intForCnt < intCntMax Then
                ' �ő�l�̏ꍇ�́A15�ŏ������s�Ȃ��B
                intSetData = 15
            Else
                ' �ő�l�ȊO�̏ꍇ�́A15�ŏ��Z�����]����擾����B
                intSetData = gRegistorCnt Mod 15
            End If

            ' �擾�����l��0�̏ꍇ�ɂ́A�����𔲂���B
            If intSetData = 0 Then Exit For

            ' ���V�I�ڕW�l�擾
#If cOFFLINEcDEBUG Then
#Else
            Call TRIM_RESULT_Double(3, intForCnt * 15, intSetData, 0, 0, gfTargetVal(intForCnt * 15))
#End If
        Next
        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_TargetVal" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "�A��NG-HIGH�װ��������"
    '''=========================================================================
    ''' <summary>�A��NG-HIGH�װ��������</summary>
    ''' <param name="contNgCountError">(OUT)1=�A��NG-HIGH�װ����</param>
    ''' <remarks>���L�̏����̓I�v�V�����Ƃ���</remarks>
    '''=========================================================================
    Private Sub TrimLogging_NgHiCountCheck(ByRef contNgCountError As Integer)

        On Error GoTo ErrExit
        Dim intForCnt As Short
        Dim intHighNgCnt As Short

        '----- ###129 �� -----
        intHighNgCnt = 0                                                ' FINAL HI NG ���� = 0 
        For intForCnt = 1 To gRegistorCnt                               ' ��R�����J��Ԃ� 
            ' INITIAL HI NG
            '----- ###202�� -----
            ' ITHI NG�ɃI�[�o�����W���܂߂�
            'If (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) Then ' INITIAL HI NG ? 
            If (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) Or (gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE) Then
                iNgHiCount(intForCnt - 1) = iNgHiCount(intForCnt - 1) + 1
            Else
                iNgHiCount(intForCnt - 1) = 0
            End If
            '----- ###202�� -----

            ' �A��NG-HIGH��R��ۯ����ȏ�Ȃ�G���[�Ƃ���
            If (iNgHiCount(intForCnt - 1) >= typPlateInfo.intContHiNgBlockCnt) Then
                contNgCountError = CONTINUES_NG_HI                      ' �A��NG-HIGH�װ����
                Exit For
            End If
        Next

        'intHighNgCnt = 0                                    ' FINAL HI NG ���� = 0 
        'For intForCnt = 1 To gRegistorCnt                   ' ��R�����J��Ԃ� 
        '    ' FINAL HI NG
        '    If (8 = gwTrimResult(intForCnt - 1)) Then       ' FINAL HI NG ? 
        '        ''''            iNgHiCount(intForCnt - 1) = iNgHiCount(intForCnt - 1) + 1
        '        intHighNgCnt = intHighNgCnt + 1
        '        'Exit For                                    ' �������Ŕ����ėǂ��� ?
        '    Else
        '        ''''            iNgHiCount(intForCnt - 1) = 0
        '    End If
        'Next

        '' �A��High-NG��ۯ����̐ݒ肪0���傫���ꍇ�ɂ́A�A��High-NG�`�F�b�N���s���B
        'If typPlateInfo.intContHiNgBlockCnt > 0 Then
        '    ' �A��NG-HIGH��R��ۯ����̒l�𒴂����ꍇ�ɂʹװ�Ƃ���B
        '    If (typPlateInfo.intContHiNgBlockCnt <= intHighNgCnt) Then
        '        contNgCountError = contNgCountError                 ' �A��NG-HIGH�װFlg = True
        '    End If
        'End If
        '----- ###129 �� -----

        Exit Sub

ErrExit:
        MsgBox("TrimLogging_NgHiCountCheck" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "�C�j�V�����e�X�g���z�}"
    '''=========================================================================
    '''<summary>�C�j�V�����e�X�g���z�}</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingGraph_RegistNumIT()

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim dblGraphDiv As Double                                       ' �O���t�͈͍��ݒl
        Dim dblGraphTop As Double                                       ' �O���t�ŏ�i�l
        Dim dblGap As Double
        Dim dblGapIT As Double                                          ' �ώZ�덷�Ƽ��
        Dim dblX_2IT As Double                                          ' IT�W���΍��Z�o�p���[�N
        Dim Average As Double                                           ' ###154 
        'Dim lITTOTAL As Integer                                        ' IT�v�Z�Ώې� ###138
        dblX_2IT = 0
        'lITTOTAL = 0                                                   ' ###138 
        '----- V6.0.3.0_26�� -----
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        '----- V6.0.3.0_26�� -----

        ' �Ƽ��ý�(LOW�Я�)�ƲƼ��ý�(HIGH�Я�)�̒l�������s���B
        If ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
            ' �Ƽ��ý�(LOW�Я�)��0�ȉ��ŲƼ��ý�(HIGH�Я�)��0�ȏ�̏ꍇ
            dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5

        ElseIf ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 > typResistorInfoArray(1).dblInitTest_HighLimit)) Then
            ' �Ƽ��ý�(LOW�Я�)��0�ȏ�ŲƼ��ý�(HIGH�Я�)��0��菬�����ꍇ
            dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit / 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit / 1.5

        ElseIf ((0 < typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
            ' �Ƽ��ý�(LOW�Я�)��0���傫���ĲƼ��ý�(HIGH�Я�)��0�ȏ�̏ꍇ
            dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit / 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5

        Else
            ' ��L�����ȊO�̏ꍇ
            dblGraphDiv = 0.3
            dblGraphTop = 1.5
        End If

        '----- V6.0.3.0_26�� -----
        ' �f�W�X�C�b�`�̓ǎ��
        Call Form1.GetMoveMode(digL, digH, digSW)
        '----- V6.0.3.0_26�� -----

        ' 1��ٰ�ߓ���R�����������s�Ȃ��B
        For intForCnt = 1 To gRegistorCnt
            ' ���ݸތ��ʂ�RSLT_OK(1),RSLT_IT_NG(2),RSLT_IT_HING(6),RSLT_IT_LONG(7)��,
            ' RSLT_OPENCHK_NG(20),RSLT_SHORTCHK_NG(21)�ȊO�̏ꍇ�A�f�[�^��ǉ�����
            'If ((gwTrimResult(intForCnt - 1) = RSLT_OK) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_NG) Or _
            '    (gwTrimResult(intForCnt - 1) = RSLT_IT_HING) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_LONG)) _
            '    And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then
            'V4.0.0.0-48��
            If ((gwTrimResult(intForCnt - 1) = RSLT_OK) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_NG) Or
                (gwTrimResult(intForCnt - 1) = RSLT_IT_HING) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_LONG) _
                    Or (gwTrimResult(intForCnt - 1) = RSLT_FT_LONG) Or (gwTrimResult(intForCnt - 1) = RSLT_FT_HING)) _
                And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then
                'V4.0.0.0-48��

                'If ((gwTrimResult(intForCnt - 1) = RSLT_OK) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_NG) Or _
                '    (gwTrimResult(intForCnt - 1) = RSLT_IT_HING) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_LONG)) _
                '    And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then

                ' '' '' '' ���ݸތ��ʂ�0(�����{), 11(���g�p), 13(���g�p)�ȊO�ł��邱�Ƃ��m�F����B
                '' '' ''If (gwTrimResult(intForCnt - 1) <> 0) And (gwTrimResult(intForCnt - 1) <> 11) And (gwTrimResult(intForCnt - 1) <> 13) Then
                ' �����Z�o����B�@�Ƽ��ýČ���/���ݸޖڕW�l*100-100
                dblGap = (gfInitialTest(intForCnt - 1) / typResistorInfoArray(intForCnt).dblTrimTargetVal) * 100.0# - 100.0#
                If ((dblGraphTop - (dblGraphDiv * 0)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*0)�@<�@���̏ꍇ
                    glRegistNumIT(0) = glRegistNumIT(0) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 1)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*1)�@<�@���̏ꍇ
                    glRegistNumIT(1) = glRegistNumIT(1) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 2)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*2)�@<�@���̏ꍇ
                    glRegistNumIT(2) = glRegistNumIT(2) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 3)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*3)�@<�@���̏ꍇ
                    glRegistNumIT(3) = glRegistNumIT(3) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 4)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*4)�@<�@���̏ꍇ
                    glRegistNumIT(4) = glRegistNumIT(4) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 5)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*5)�@<�@���̏ꍇ
                    glRegistNumIT(5) = glRegistNumIT(5) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 6)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*6)�@<�@���̏ꍇ
                    glRegistNumIT(6) = glRegistNumIT(6) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 7)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*7)�@<�@���̏ꍇ
                    glRegistNumIT(7) = glRegistNumIT(7) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 8)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*8)�@<�@���̏ꍇ
                    glRegistNumIT(8) = glRegistNumIT(8) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 9)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*9)�@<�@���̏ꍇ
                    glRegistNumIT(9) = glRegistNumIT(9) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 10)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*10)�@<�@���̏ꍇ
                    glRegistNumIT(10) = glRegistNumIT(10) + 1
                Else
                    ' ��L�����ȊO�̏ꍇ
                    glRegistNumIT(11) = glRegistNumIT(11) + 1
                End If

                ' ���ݸތ��ʂ�1(OK)�܂���12(IT OKTEST OK(SL436K))�ł��邩��������B
                ''V4.2.0.0�C ��
                'If (gwTrimResult(intForCnt - 1) = 1) Or (gwTrimResult(intForCnt - 1) = 12) Then
                If (gwTrimResult(intForCnt - 1) = 1) Or (gwTrimResult(intForCnt - 1) = 12) _
                    Or (gwTrimResult(intForCnt - 1) = RSLT_FT_LONG) Or (gwTrimResult(intForCnt - 1) = RSLT_FT_HING) Then
                    ''V4.2.0.0�C ��
                    glITTOTAL = glITTOTAL + 1                           ' IT�v�Z�Ώې� ###138
                    dblGapIT = dblGapIT + dblGap                        ' �ώZ�덷
                    dblX_2IT = dblX_2IT + (dblGap * dblGap)

                    '----- V6.0.3.0_26�� -----
                    If digL = 0 Or digL = 1 Then
                        '����Ƀg���~���O�ł�����R�̒�R�l�擾 
                        TotalCntTrimming = TotalCntTrimming + 1
                        SetAverageFTValue(gfFinalTest(intForCnt - 1), TotalCntTrimming)
                    End If
                    '----- V6.0.3.0_26�� -----

                    gITNx_cnt = gITNx_cnt + 1
                    'ReDim Preserve gITNx(gITNx_cnt)                     ' �z��̍쐬                  '###154
                    ''(�W���΍��Z�o���C��)
                    'gITNx(gITNx_cnt) = dblGap                           ' ����덷���                 '###154
                    Average = GetAverageIT(dblGap, gITNx_cnt + 1)                                      '###154
                    dblDeviationIT = GetDeviationIT(dblGap, gITNx_cnt + 1, Average)                    '###154
                    dblAverageIT = Average                                                             '###154
                    If (1 = glITTOTAL) Then                             ' ###138
                        dblMinIT = dblGap
                        dblMaxIT = dblGap
                    End If
                    If (dblMinIT > dblGap) Then
                        dblMinIT = dblGap
                    End If
                    If (dblMaxIT < dblGap) Then
                        dblMaxIT = dblGap
                    End If
                Else
                    'NG�J�E���g�����L�^
                    gITNg_cnt = gITNg_cnt + 1
                End If
            End If
        Next

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingGraph_RegistNumIT" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "�t�@�C�i���e�X�g�e�X�g���z�}"
    '''=========================================================================
    '''<summary>�t�@�C�i���e�X�g�e�X�g���z�}</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingGraph_RegistNumFT()

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim dblGraphDiv As Double                                       ' �O���t�͈͍��ݒl
        Dim dblGraphTop As Double                                       ' �O���t�ŏ�i�l
        Dim dblGap As Double
        Dim dblGapFT As Double                                          ' �ώZ�덷̧���
        Dim dblX_2FT As Double                                          ' FT�W���΍��Z�o�p���[�N
        Dim Average As Double                                           ' ���ϗp�@###154
        'Dim lFTTOTAL As Integer                                        ' FT�v�Z�Ώې� ###138
        dblX_2FT = 0
        'lFTTOTAL = 0                                                   ' ###138

        ' ̧���ý�(LOW�Я�)��̧���ý�(HIGH�Я�)�̒l����������B
        If ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
            ' ̧���ý�(LOW�Я�)��0�ȉ���̧���ý�(HIGH�Я�)��0�ȏ�̏ꍇ
            dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5

        ElseIf ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 > typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
            ' ̧���ý�(LOW�Я�)��0�ȉ���̧���ý�(HIGH�Я�)����菬�����ꍇ
            dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit / 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5

        ElseIf ((0 < typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
            ' ̧���ý�(LOW�Я�)��0���傫����̧���ý�(HIGH�Я�)��0�ȏ�̏ꍇ
            dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit / 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
        Else
            ' ��L�����ȊO�̏ꍇ
            dblGraphDiv = 0.3
            dblGraphTop = 1.5
        End If

        ' 1��ٰ�ߓ���R�����������s�Ȃ��B
        For intForCnt = 1 To gRegistorCnt
            ' ���ݸތ��ʂ�1(OK), 8(FT HI NG), 9(FT LO NG)�̂����ꂩ��RSLT_OPENCHK_NG(20),RSLT_SHORTCHK_NG(21)�ȊO�̏ꍇ
            If (gwTrimResult(intForCnt - 1) = RSLT_OK Or gwTrimResult(intForCnt - 1) = RSLT_FT_HING Or gwTrimResult(intForCnt - 1) = RSLT_FT_LONG) _
                And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then
                ' �����Z�o����B�@̧���ýČ���/���ݸޖڕW�l*100�@-�@100
                dblGap = (gfFinalTest(intForCnt - 1) / typResistorInfoArray(intForCnt).dblTrimTargetVal) * 100.0# - 100.0#

                If ((dblGraphTop - (dblGraphDiv * 0)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*0)�@<�@���̏ꍇ
                    glRegistNumFT(0) = glRegistNumFT(0) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 1)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*1)�@<�@���̏ꍇ
                    glRegistNumFT(1) = glRegistNumFT(1) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 2)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*2)�@<�@���̏ꍇ
                    glRegistNumFT(2) = glRegistNumFT(2) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 3)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*3)�@<�@���̏ꍇ
                    glRegistNumFT(3) = glRegistNumFT(3) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 4)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*4)�@<�@���̏ꍇ
                    glRegistNumFT(4) = glRegistNumFT(4) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 5)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*5)�@<�@���̏ꍇ
                    glRegistNumFT(5) = glRegistNumFT(5) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 6)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*6)�@<�@���̏ꍇ
                    glRegistNumFT(6) = glRegistNumFT(6) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 7)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*7)�@<�@���̏ꍇ
                    glRegistNumFT(7) = glRegistNumFT(7) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 8)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*8)�@<�@���̏ꍇ
                    glRegistNumFT(8) = glRegistNumFT(8) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 9)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*9)�@<�@���̏ꍇ
                    glRegistNumFT(9) = glRegistNumFT(9) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 10)) < dblGap) Then
                    ' ���̍ŏ�i�l-(���͈͍̔��݈ʒu*10)�@<�@���̏ꍇ
                    glRegistNumFT(10) = glRegistNumFT(10) + 1
                Else
                    ' ��L�����ȊO�̏ꍇ
                    glRegistNumFT(11) = glRegistNumFT(11) + 1
                End If

                ' ���ݸތ��ʂ�1(OK)�ł��邩��������B
                If (gwTrimResult(intForCnt - 1) = 1) Then
                    glFTTOTAL = glFTTOTAL + 1                           ' FT�v�Z�Ώې� ###138
                    dblGapFT = dblGapFT + dblGap                        ' �ώZ�덷
                    dblX_2FT = dblX_2FT + (dblGap * dblGap)

                    gFTNx_cnt = gFTNx_cnt + 1
                    'ReDim Preserve gFTNx(gFTNx_cnt)                     ' �z��̍쐬      '###154
                    'gFTNx(gFTNx_cnt) = dblGap                           ' ����덷���     '###154
                    'GetDeviationFTOrg(gFTNx, gFTNx_cnt + 1, Average)                      '###154
                    Average = GetAverageFT(dblGap, gFTNx_cnt + 1)                          '###154
                    dblDeviationFT = GetDeviationFT(dblGap, gFTNx_cnt + 1, Average)        '###154
                    dblAverageFT = Average                                                 '###154
                    '(�W���΍��Z�o���C��)
                    If (1 = glFTTOTAL) Then                             ' ###138
                        dblMinFT = dblGap
                        dblMaxFT = dblGap
                    End If
                    If (dblMinFT > dblGap) Then
                        dblMinFT = dblGap
                    End If
                    If (dblMaxFT < dblGap) Then
                        dblMaxFT = dblGap
                    End If
                Else
                    'NG�J�E���g�����L�^
                    gFTNg_cnt = gFTNg_cnt + 1
                End If
            End If
        Next

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingGraph_RegistNumFT" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "�g���~���O���ʂ��o�͂���"
    '''=========================================================================
    ''' <summary>�g���~���O���ʂ��o�͂���</summary>
    ''' <param name="digH">         (INP)</param>
    ''' <param name="digL">         (INP)</param>
    ''' <param name="strFileLogMsg">(OUT)</param>
    ''' <param name="strDispLogMsg">(OUT)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_LoggingDataMain(ByVal digH As Integer, ByVal digL As Integer,
                                            ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                            ByVal strFileLogMsg As StringBuilder, ByVal strDispLogMsg As StringBuilder)
        '#4.12.2.0�E blkNoX,blkNoY�ǉ�
        '#4.12.2.0�C    Private Sub TrimLogging_LoggingDataMain(ByVal digH As Integer, ByVal digL As Integer,
        '#4.12.2.0�C                ByRef strFileLogMsg As String, ByRef strDispLogMsg As String)

        '#4.12.2.0�@        On Error GoTo ErrExit
        Try                             '#4.12.2.0�@
            Dim intForCnt As Short
            Dim strchKugiri As String
            Dim strchFloatPoint As String
            Dim calcWork As Integer

            ' ү���ތ��ꂪ���B�ȊO�ł��邩��������B
            If gSysPrm.stTMN.giMsgTyp <> 2 Then
                strchKugiri = ","
                strchFloatPoint = "."
            Else
                strchKugiri = Chr(KUGIRI_CHAR) ' TAB
                strchFloatPoint = ","
            End If

            If gKeiTyp = KEY_TYPE_RS Then              'V2.0.0.0�I �V���v���g���} �ڕW�l�A�K�i�l�̐ݒ�
                SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)    'V2.0.0.0�I
            End If                                      'V2.0.0.0�I
            '
            ' ��R�����A�������������s�Ȃ��B
            '' ''For intForCnt = 1 To gRegistorCnt
            '' ''giTrimResult0x(intForCnt - 1) = 0
            'V4.1.0.0�D
            '' ''If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE Then     ' ���ް�ݼ�
            '' ''    gfInitialTest(intForCnt - 1) = 9999999999.9
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''ElseIf gwTrimResult(intForCnt - 1) = 11 Then                    ' 4�[�q�����
            '' ''    gfInitialTest(intForCnt - 1) = 0
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''ElseIf gwTrimResult(intForCnt - 1) = 12 Then                    ' �Ƽ��OKý�
            '' ''    gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
            '' ''ElseIf gwTrimResult(intForCnt - 1) = 13 Then                    ' ����ہ[��ݸ�
            '' ''    gfInitialTest(intForCnt - 1) = 9999999999.9
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''    'ElseIf (m_intxmode <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
            '' ''ElseIf (digL <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
            '' ''                                Or (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG)) Then  ' �Ƽ��NG
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''    'ElseIf m_intxmode = 2 Then                                 ' ��L�װ�ȊO��x2
            '' ''ElseIf digL = 2 Then                                            ' ��L�װ�ȊO��x2
            '' ''    gfInitialTest(intForCnt - 1) = gfFinalTest(intForCnt - 1)
            '' ''    'ElseIf m_intxmode = 3 Then                                 ' ��L�װ�ȊO��x3
            '' ''ElseIf digL = 3 Then                                            ' ��L�װ�ȊO��x3
            '' ''    '(2011/06/25) ���L�̒l�͎擾���Ă����Ƃ��Ă������l�̂͂��B
            '' ''    '   �܂��Ax3���[�h�ł̓t�@�C�i���̒l�������擾����悤�ɂ���̂�,
            '' ''    '   ���L�̏����͕s�v�i�l���N���A����Ă��܂��\��������B
            '' ''    'gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
            '' ''End If
            '' ''Next intForCnt
            ''V4.1.0.0�D
            ' ү���ޗp�ϐ��̏������������s�Ȃ��B
            '#4.12.2.0�C        strFileLogMsg = ""
            strFileLogMsg.Length = 0                '#4.12.2.0�C 
            '#4.12.2.0�C        strDispLogMsg = ""
            strDispLogMsg.Length = 0                '#4.12.2.0�C

            ' ��R�����������s�Ȃ��B
            For intForCnt = 1 To gRegistorCnt
                ''V4.1.0.0�D
                ' �������[�v�Ŏ��s���Ă݂�
                giTrimResult0x(intForCnt - 1) = 0

                If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE Then     ' ���ް�ݼ�
                    gfInitialTest(intForCnt - 1) = 9999999999.9
                    gfFinalTest(intForCnt - 1) = 0
                ElseIf gwTrimResult(intForCnt - 1) = 11 Then                    ' 4�[�q�����
                    gfInitialTest(intForCnt - 1) = 0
                    gfFinalTest(intForCnt - 1) = 0
                ElseIf gwTrimResult(intForCnt - 1) = 12 Then                    ' �Ƽ��OKý�
                    gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
                ElseIf gwTrimResult(intForCnt - 1) = 13 Then                    ' ����ہ[��ݸ�
                    gfInitialTest(intForCnt - 1) = 9999999999.9
                    gfFinalTest(intForCnt - 1) = 0
                    'ElseIf (m_intxmode <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
                ElseIf (digL <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
                                            Or (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG)) Then  ' �Ƽ��NG
                    gfFinalTest(intForCnt - 1) = 0
                    'ElseIf m_intxmode = 2 Then                                 ' ��L�װ�ȊO��x2
                ElseIf digL = 2 Then                                            ' ��L�װ�ȊO��x2
                    gfInitialTest(intForCnt - 1) = gfFinalTest(intForCnt - 1)
                    'ElseIf m_intxmode = 3 Then                                 ' ��L�װ�ȊO��x3
                ElseIf digL = 3 Then                                            ' ��L�װ�ȊO��x3
                    '(2011/06/25) ���L�̒l�͎擾���Ă����Ƃ��Ă������l�̂͂��B
                    '   �܂��Ax3���[�h�ł̓t�@�C�i���̒l�������擾����悤�ɂ���̂�,
                    '   ���L�̏����͕s�v�i�l���N���A����Ă��܂��\��������B
                    'gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
                End If

                ''V4.1.0.0�D

                'V5.0.0.4�E If gKeiTyp = KEY_TYPE_RS Then              'V2.0.0.0�I �V���v���g���}
                If gKeiTyp = KEY_TYPE_RS And (Val(CStr(typResistorInfoArray(intForCnt).intResNo)) < 1000) Then              'V2.0.0.0�I �V���v���g���}
                    'SimpleTrimmer.TrimData.SetResData(intForCnt, gfInitialTest(intForCnt - 1), gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1))   'V2.0.0.0�I
                    '                SimpleTrimmer.TrimData.SetResData(intForCnt, gfInitialTest(intForCnt - 1), gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1), digL)       'V4.0.0.0-23
                    If digL = 2 Then                                            ' ��L�װ�ȊO��x2
                        SimpleTrimmer.TrimData.SetResData(intForCnt, 0, gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1), digL)       'V4.0.0.0-23
                    Else
                        SimpleTrimmer.TrimData.SetResData(intForCnt, gfInitialTest(intForCnt - 1), gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1), digL)       'V4.0.0.0-23

                    End If

                End If                                      'V2.0.0.0�I
                ' ��R������=0�̎������������s�Ȃ��B
                '==========================================
                ' ��ʕ\���p�̃f�[�^������𐶐�
                '==========================================

                If (digH = 0) Then
                    '�\���Ȃ��Ȃ牽���\�����Ȃ�
                Else
                    '---------------------------------------
                    '�\�����[�h�i�f�W�^���̏�ʐݒ�)
                    '   digH=0:�\�����Ȃ��B
                    '   digH=1-digL=0�`2:NG��R�̂ݕ\������B
                    '---------------------------------------
                    If intForCnt = 1 Then
                        ' ��ʃ��O�̃w�b�_�[�����̎擾
                        Call TrimLogging_MakeLogDisplayHeader(digH, digL, strDispLogMsg)
                    End If

                    If (digH = 1) And (digL <= 2) Then
                        calcWork = intForCnt Mod m_TrimDisp1xFormat(0)
                        If calcWork <> 0 Then
                            Call TrimLogging_MakeLogStringData(m_TrimDisp1xFormat, digH, digL, blkNoX, blkNoY,
                                        intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)
                        Else
                            Call TrimLogging_MakeLogStringData(m_TrimDisp1xFormat, digH, digL, blkNoX, blkNoY,
                                        intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)
                        End If
                    Else
                        ' �W���\��(3�~�A4�~)/0�~�A1�~�\��
                        Select Case digL
                            Case 0
                                calcWork = intForCnt Mod m_TrimDispx0Format(0)
                                If calcWork <> 0 Then
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)   '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, False)      '###238
                                Else
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)    '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, True)       '###238
                                End If
                            Case 1, 2
                                calcWork = intForCnt Mod m_TrimDispx1Format(0)
                                If calcWork <> 0 Then
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)   '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, False)      '###238
                                Else
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)    '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, True)       '###238
                                End If
                            Case 3
                                calcWork = intForCnt Mod m_MeasDispFormat(0)
                                If calcWork <> 0 Then
                                    'Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)   '###238
                                    Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, False)      '###238
                                Else
                                    'Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)    '###238
                                    Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, True)       '###238
                                End If
                        End Select
                    End If
                End If

                '==========================================================
                '   OK/NG�J�E���g�̎Z�o(x0,x1,x2���[�h��)
                '==========================================================
                If digL <= 2 Then
                    '----- ###167�� -----
                    ' NET��TKY��TrimLoggingCircuit_OKNG()�ŃT�[�L�b�g�P�ʂ�OK/NG�J�E���g�����߂Ă���̂�SKIP
                    If (gTkyKnd = KND_NET) Or (gTkyKnd = KND_TKY) Then
                        GoTo STP_MAKELOG
                    End If
                    '----- ###167�� -----
                    'V6.1.1.0�N��
                    ' ��R���̒���NG�}�[�L���O�̐����܂܂�Ă����̂ŁA1000�ȍ~�̂Ƃ��̓J�E���g���Ȃ�
                    If (Val(CStr(typResistorInfoArray(intForCnt).intResNo)) >= 1000) Then
                        Continue For
                    End If
                    'V6.1.1.0�N��

                    ' ���ݸތ��ʂ�OK�ł��邩��������B
                    If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK Or gwTrimResult(intForCnt - 1) = 12 Then
                        m_lGoodCount = m_lGoodCount + 1                         ' OK�J�E���g�X�V
                        '----- V1.18.0.0�B�� .Trim_OK�͖��g�p�̂��ߍ폜 -----
                        'If (gTkyKnd = KND_CHIP) Then
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Trim_OK = stPRT_ROHM.Trim_OK + 1    ' �Ǖi���ߐ�
                        '    End If
                        'End If
                        '----- V1.18.0.0�B�� -----
                        ' ���ݸތ��ʂ�NG�ł��邩��������B
                    ElseIf gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_RATIO _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE _
                        Or gwTrimResult(intForCnt - 1) = 11 _
                        Or gwTrimResult(intForCnt - 1) = 13 _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_CVERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERLOAD _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_ES2ERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_MIDIUM_CUT_NG Then     ' 'V1.20.0.1�@ ' V1.13.0.0�J
                        m_lNgCount = m_lNgCount + 1                             ' NG�J�E���g�X�V
                        'm_lTrimResult = cFRS_TRIM_NG                           ' ��P�ʂ̃g���~���O����(SL436R�p) = �g���~���ONG ###142 ###089
                        m_NG_RES_Count = m_NG_RES_Count + 1                     ' NG����(0-100%)�pNG��R��(�u���b�N�P��/�v���[�g�P��) ###142 
                        '----- V1.18.0.0�B�� .Trim_NG�͖��g�p�̂��ߍ폜 -----
                        'If (gTkyKnd = KND_CHIP) Then
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Trim_NG = stPRT_ROHM.Trim_NG + 1     ' �s�Ǖi���ߐ�
                        '    End If
                        'End If
                        '----- V1.18.0.0�B�� -----

                        m_NgCountInPlate = m_NgCountInPlate + 1     'V4.5.0.5�@ �P���NG���̃J�E���g 'V4.12.2.0�H�@'V6.0.5.0�C

                    End If
                End If

STP_MAKELOG:  ' ###167
                '==========================================
                ' ���O�t�@�C���o�͗p�̃f�[�^������𐶐�
                '==========================================
                If Not (Val(CStr(typResistorInfoArray(intForCnt).intResNo)) >= 1000) Then

                    '�t�@�C���ւ̏o�͂̓t�H�[�}�b�g�����킹�邽�߁ATrimMode�݂̂Ƃ���B
                    calcWork = intForCnt Mod m_TrimlogFileFormat(0)
                    If calcWork <> 0 Then
                        Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, digH, digL, blkNoX, blkNoY,
                                    intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, False)
                    Else
                        Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, digH, digL, blkNoX, blkNoY,
                                   intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, True)
                    End If
                    '' '' ''Select Case digL
                    '' '' ''    Case 3
                    '' '' ''        calcWork = intForCnt Mod m_MeasDispFormat(0)
                    '' '' ''        ' �t�@�C�����O(�~3���[�h�̏ꍇ)
                    '' '' ''        If calcWork <> 0 Then
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_MeaslogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, False)
                    '' '' ''        Else
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_MeaslogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, True)
                    '' '' ''        End If
                    '' '' ''    Case Else
                    '' '' ''        ' �t�@�C�����O(�~0�A�~1�A�~2���[�h�̏ꍇ)
                    '' '' ''        If calcWork <> 0 Then
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, False)
                    '' '' ''        Else
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, True)
                    '' '' ''        End If
                    '' '' ''End Select
                    'Call TrimLogging_MakeLogStringData(digL, m_logFileFormat, intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg)

                    ' �޼�SW��1���ڂ���������B
                    'Select Case m_intxmode
                    'Select Case digL
                    '    Case 3
                    '        ' �t�@�C�����O(�~3���[�h�̏ꍇ)
                    '        Call TrimLogging_FileLoggingData_mode3(intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg)
                    '    Case Else
                    '        ' �t�@�C�����O(�~0�A�~1�A�~2���[�h�̏ꍇ)
                    '        Call TrimLogging_FileLoggingData(intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg)
                    'End Select
                    'End If                                                     '###012 

                    ' �d������l���V�X�p���̍ő�d���l�𒴂�����IT����l��9999999999.9�ɂ���
                    If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                        'If typPlateInfo.intMeasType = 1 Then                           ' ###166
                        If (typResistorInfoArray(intForCnt).intResMeasMode = 1) Then    ' ###166 ���胂�[�h = 1(�d��) ?
                            If gfInitialTest(intForCnt - 1) > gSysPrm.stMES.gdVOLTAGE_MAX Then
                                gfInitialTest(intForCnt - 1) = 9999999999.9
                            End If
                            If gfFinalTest(intForCnt - 1) > gSysPrm.stMES.gdVOLTAGE_MAX Then
                                gfFinalTest(intForCnt - 1) = 9999999999.9
                            End If
                            If gfInitialTest(intForCnt - 1) < gSysPrm.stMES.gdVOLTAGE_MIN Then
                                gfInitialTest(intForCnt - 1) = -9999999999.9
                            End If
                            If gfFinalTest(intForCnt - 1) < gSysPrm.stMES.gdVOLTAGE_MIN Then
                                gfFinalTest(intForCnt - 1) = -9999999999.9
                            End If
                        End If
                    End If
                End If                                                          '###012 

            Next

            ' '' '' ''�\���p���O�̍Ō�̉��s���폜
            ' '' '' ''strDispLogMsg = strDispLogMsg & vbCrLf
            '' '' ''Dim lastVbCrlfPos As Integer
            '' '' ''Dim length As Integer
            '' '' ''lastVbCrlfPos = strDispLogMsg.LastIndexOf("%")
            '' '' ''length = strDispLogMsg.Length
            '' '' ''strDispLogMsg.Remove(lastVbCrlfPos - 1, length - lastVbCrlfPos + 1)
            '' '' '' '' '' ''lastKugiriPos = InStrRev(strLogMsg, strchKugiri)
            '' '' '' '' '' ''Replace(strDispLogMsg, vbCrLf, "", lastVbCrlfPos)

            '#4.12.2.0�@            Exit Sub
        Catch ex As Exception           '#4.12.2.0�@
            '#4.12.2.0�@ErrExit:
            MsgBox("TrimLogging_LoggingDataMain" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

    End Sub
#End Region

#Region "�g���~���O���ʃw�b�_�[�o�͕�������\�z����B"
    '''=========================================================================
    ''' <summary>�g���~���O���ʃw�b�_�[�o�͂̕�������\�z����B
    ''' �ݒ�ɏ]���A�w�b�_�����̏ڍׂȃt�H�[�}�b�g���쐬����B</summary>
    ''' <param name="logTarget"></param>
    ''' <param name="logType"></param>
    ''' <param name="separator"></param>
    ''' <param name="strLogMsg"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogHeader(ByVal logTarget() As Integer, ByVal logType As Integer,
                                          ByVal separator As String, ByVal strLogMsg As StringBuilder)
        '#4.12.2.0�C    Private Sub TrimLogging_MakeLogHeader(ByVal logTarget() As Integer, ByVal logType As Integer,
        '#4.12.2.0�C                                        ByVal separator As String, ByRef strLogMsg As String)

        '#4.12.2.0�C        On Error GoTo ErrExit

        '#4.12.2.0�C        Dim strMakeMsg As String
        Dim cnt As Integer
        Dim logResCnt As Integer

        'strLogMsg = strLogMsg & vbCrLf
        Try                             '#4.12.2.0�C
            For logResCnt = 1 To logTarget(0)
                For cnt = 1 To LOGTAR.END
                    ' �W���`���̃t�@�C�����O
                    Select Case logTarget(cnt)
                        Case LOGTAR.DATE
                            If logType = LOGTYPE_DISP Then
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "DATE" & separator & separator & separator
                                strLogMsg.Append("DATE" & separator & separator & separator)            '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "DATE" & separator
                                strLogMsg.Append("DATE" & separator)                                    '#4.12.2.0�C
                            End If
                        Case LOGTAR.MODE
                            '#4.12.2.0�C                        strLogMsg = strLogMsg & "MODE" & separator
                            strLogMsg.Append("MODE" & separator)                                        '#4.12.2.0�C
                        Case LOGTAR.LOTNO
                            '#4.12.2.0�C                        strLogMsg = strLogMsg & "LOT-NO" & separator
                            strLogMsg.Append("LOT-NO" & separator)                                      '#4.12.2.0�C

                        Case LOGTAR.BLOCKX                              '#4.12.2.0�C
                            strLogMsg.Append("BLOCK-X" & separator)
                        Case LOGTAR.BLOCKY                              '#4.12.2.0�C
                            strLogMsg.Append("BLOCK-Y" & separator)

                        Case LOGTAR.CIRCUIT
                            'strLogMsg = strLogMsg & "CIR-NO" & separator           '###238
                            If logType = LOGTYPE_DISP Then
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "CNo " & separator          '###238 4�����l��
                                strLogMsg.Append("CNo " & separator)        '###238 4�����l��           '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "CIR-NO" & separator
                                strLogMsg.Append("CIR-NO" & separator)                                  '#4.12.2.0�C
                            End If
                        Case LOGTAR.RESISTOR
                            'strLogMsg = strLogMsg & "RES-NO" & separator           '###238
                            If logType = LOGTYPE_DISP Then
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "RNo " & separator          '###238 4�����l��
                                strLogMsg.Append("RNo " & separator)        '###238 4�����l��           '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "RES-NO" & separator
                                strLogMsg.Append("RES-NO" & separator)                                  '#4.12.2.0�C
                            End If
                        Case LOGTAR.JUDGE
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "RESULT  " & separator     '###238
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "RESULT    " & separator    '###238 10�������l��
                                strLogMsg.Append("RESULT    " & separator)  '###238 10�������l��         '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "RESULT  " & separator
                                strLogMsg.Append("RESULT  " & separator)                                '#4.12.2.0�C
                            End If
                        Case LOGTAR.TARGET
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "TARGET" & separator & separator '###238 
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "   TARGGET      " & separator    '###238 16����
                                strLogMsg.Append("   TARGET       " & separator)    '###238 16����      '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "TARGET" & separator
                                strLogMsg.Append("TARGET" & separator)                                  '#4.12.2.0�C
                            End If
                        Case LOGTAR.INITIAL
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "INIVAL" & separator & separator '###238 
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "    INIVAL      " & separator    '###238 16����
                                strLogMsg.Append("    INIVAL      " & separator)    '###238 16����      '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "INIVAL" & separator
                                strLogMsg.Append("INIVAL" & separator)                                  '#4.12.2.0�C
                            End If
                        Case LOGTAR.FINAL
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "FINVAL" & separator & separator '###238 
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "    FINVAL      " & separator    '###238 16����
                                strLogMsg.Append("    FINVAL      " & separator)    '###238 16����      '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "FINVAL" & separator
                                strLogMsg.Append("FINVAL" & separator)                                  '#4.12.2.0�C
                            End If
                        Case LOGTAR.DEVIATION
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "DEVIAT" & separator & separator   '###238 
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "  DEVIAT" & separator              '###238 
                                strLogMsg.Append("  DEVIAT" & separator)            '###238             '#4.12.2.0�C
                            Else
                                '#4.12.2.0�C                            strLogMsg = strLogMsg & "DEVIAT" & separator
                                strLogMsg.Append("DEVIAT" & separator)                                  '#4.12.2.0�C
                            End If
                        Case LOGTAR.UCUTPRMNO
                            '#4.12.2.0�C                        strLogMsg = strLogMsg & "UcutPrmNo" & separator
                            strLogMsg.Append("UcutPrmNo" & separator)                                   '#4.12.2.0�C
                        Case LOGTAR.NOSET
                        '�������Ȃ�
                        Case LOGTAR.END
                            Exit For
                    End Select
                Next
            Next
            '�w�b�_�[�̍Ō���ɂ͉��s�R�[�h��ǉ�����
            '#4.12.2.0�C        strLogMsg = strLogMsg & vbCrLf
            strLogMsg.AppendLine()          '#4.12.2.0�C

            '#4.12.2.0�C            Exit Sub

        Catch ex As Exception
            '@@@20170810ErrExit:
            MsgBox("TrimLogging_MakeLogHeader" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

    End Sub
#End Region

#Region "���O�o�͗p�̃w�b�_��������\�z����"
    '''=========================================================================
    '''<summary>���O�o�͗p�̃w�b�_��������\�z����B
    ''' strLogMs2�ɑ��茋�ʕ\���̃w�b�_�[�����̐ݒ���s���B</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogDisplayHeader(ByVal digH As Integer, ByVal digL As Integer, ByVal strLogHeader As StringBuilder)
        '#4.12.2.0�C    Private Sub TrimLogging_MakeLogDisplayHeader(ByVal digH As Integer, ByVal digL As Integer, ByRef strLogHeader As String)

        Dim strCircuit As String
        Dim addRetFlg As Integer

        On Error GoTo ErrExit

        If (gTkyKnd = KND_TKY Or gTkyKnd = KND_NET) Then
            strCircuit = "CIRCUIT" & vbTab
        Else
            strCircuit = ""
        End If

        'If (digH = 1) And (digL <= 2) Then
        If (digH = 1) And (digL <= 3) Then                                                                  '###217

            ' �޼�SW��1���ڂ��������s��
            Select Case digL
                Case 0, 1, 2                                                                                '###217
                    '1x�p�ɐݒ��ύX���ăw�b�_�[���\�z
                    Call TrimLogging_MakeLogHeader(m_TrimDisp1xFormat, LOGTYPE_DISP, vbTab, strLogHeader)
            End Select                                                                                      '###217

        Else
            ' �޼�SW��1���ڂ��������s��
            Select Case digL
                Case 0
                    'x0�p�ɐݒ��ύX���ăw�b�_�[���\�z
                    'Call TrimLogging_MakeLogHeader(m_TrimDispx0Format, LOGTYPE_DISP, vbTab, strLogHeader)  '###238
                    Call TrimLogging_MakeLogHeader(m_TrimDispx0Format, LOGTYPE_DISP, " ", strLogHeader)     '###238
                Case 1, 2
                    'x1�p�ɐݒ��ύX���ăw�b�_�[���\�z
                    'Call TrimLogging_MakeLogHeader(m_TrimDispx1Format, LOGTYPE_DISP, vbTab, strLogHeader)  '###238
                    Call TrimLogging_MakeLogHeader(m_TrimDispx1Format, LOGTYPE_DISP, " ", strLogHeader)     '###238
                Case 3
                    'x3�p�ɐݒ��ύX���ăw�b�_�[���\�z
                    'Call TrimLogging_MakeLogHeader(m_MeasDispFormat, LOGTYPE_DISP, vbTab, strLogHeader)    '###238
                    Call TrimLogging_MakeLogHeader(m_MeasDispFormat, LOGTYPE_DISP, " ", strLogHeader)       '###238
            End Select
        End If

        Exit Sub

ErrExit:
        MsgBox("TrimLogging_MakeLogDisplayHeader" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "�g���~���O���ʏo�͕�������\�z����"
    '''=========================================================================
    ''' <summary>�g���~���O���ʏo�͕�������\�z����</summary>
    ''' <param name="logTarget">      </param>
    ''' <param name="digH">           </param>
    ''' <param name="digL">           </param>
    ''' <param name="blkNoX"></param>
    ''' <param name="blkNoY"></param>
    ''' <param name="intForCnt">      </param>
    ''' <param name="strchKugiri">    </param>
    ''' <param name="strchFloatPoint"></param>
    ''' <param name="strLogMsg">      </param>
    ''' <param name="addRetFlg">      </param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogStringData(ByVal logTarget() As Integer,
                                              ByVal digH As Integer, ByVal digL As Integer,
                                              ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                              ByVal intForCnt As Short, ByVal strchKugiri As String, ByVal strchFloatPoint As String,
                                              ByVal strLogMsg As StringBuilder, ByVal addRetFlg As Boolean)
        '#4.12.2.0�E blkNoX,blkNoY�ǉ�
        '#4.12.2.0�C    Private Sub TrimLogging_MakeLogStringData(ByVal logTarget() As Integer, ByVal digH As Integer, ByVal digL As Integer,
        '#4.12.2.0�C                                    ByVal intForCnt As Short, ByVal strchKugiri As String, ByVal strchFloatPoint As String,
        '#4.12.2.0�C                                    ByRef strLogMsg As String, ByVal addRetFlg As Boolean)

        On Error GoTo ErrExit

        Dim strMakeMsg As String = ""
        Dim dblDiff As Double
        Dim strBuf As String
        Dim cnt As Integer
        Dim Rn As Short
        'Dim curTime As DateTime = DateTime.Now
        Dim curTime As String
        Dim NowBlockNo As Integer 'V4.3.0.0�@


        curTime = Today.ToString("yyMMdd") & " " & TimeOfDay.ToString("HHmmss")

        ' �}�[�L���O��R��Skip @@@007
        If (typResistorInfoArray(intForCnt).intResNo >= 1000) Then
            Exit Sub
        End If

        ' NG�̂ݕ\���̏ꍇ
        If (digH = 1) Then
            'OK�܂��͖����{�̏ꍇ�̓��O�������ǉ����Ȃ�
            If ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO) Or
                 (gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK) Or
                 (gwTrimResult(intForCnt - 1) = TRIM_RESULT_SKIP)) Then
                Exit Sub
            End If
        End If

        '----- V6.0.3.0�L�� -----
        Dim DecPoint As Integer

        ' �����_�ȉ��̌��������߂� (gsEDIT_DIGITNUM("0.00000")'
        DecPoint = GetDecPointSize(gsEDIT_DIGITNUM)
        '----- V6.0.3.0�L�� -----

        For cnt = 1 To LOGTAR.END
            ' �W���`���̃t�@�C�����O
            Select Case logTarget(cnt)
                Case LOGTAR.DATE
                    '���t�f�[�^�̒ǉ�
                    '#4.12.2.0�C                    strLogMsg = strLogMsg & curTime & strchKugiri
                    strLogMsg.Append(curTime & strchKugiri)                         '#4.12.2.0�C

                Case LOGTAR.MODE
                    '�g���~���O���[�h�̒ǉ�
                    If (digL <> 3) Then
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & "TRIMx" & digL & strchKugiri
                        strLogMsg.Append("TRIMx" & digL & strchKugiri)              '#4.12.2.0�C
                    Else
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & "MEAS" & strchKugiri
                        strLogMsg.Append("MEAS" & strchKugiri)                      '#4.12.2.0�C
                    End If

                Case LOGTAR.LOTNO
                    '���b�g�f�[�^�̒ǉ�
                    '#4.12.2.0�C                    strLogMsg = strLogMsg & gSysPrm.stLOG.giLoggingLotNo.ToString() & strchKugiri
                    strLogMsg.Append(gSysPrm.stLOG.giLoggingLotNo & strchKugiri)    '#4.12.2.0�C

                Case LOGTAR.BLOCKX                                      '#4.12.2.0�E
                    ' �u���b�N�ԍ��w�̒ǉ�
                    strLogMsg.Append(blkNoX & strchKugiri)
                Case LOGTAR.BLOCKY                                      '#4.12.2.0�E
                    ' �u���b�N�ԍ��x�̒ǉ�
                    strLogMsg.Append(blkNoY & strchKugiri)

                Case LOGTAR.CIRCUIT
                    '----- ###238�� -----
                    '�T�[�L�b�g�f�[�^�̒ǉ�
                    'strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intCircuitGrp.ToString() & strchKugiri
                    If (strchKugiri = ",") Then
                        'V4.3.0.0�@��
                        If gKeiTyp = KEY_TYPE_RS Then
                            'SimpleTrimmer .SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit)    'V2.0.0.0�I
                            SimpleTrimmer.GetBlockDisplayNumber(NowBlockNo)
                            ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��4�����l�߂Ƃ��Ȃ�
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & NowBlockNo.ToString() & strchKugiri
                            strLogMsg.Append(NowBlockNo & strchKugiri)              '#4.12.2.0�C
                        Else
                            ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��4�����l�߂Ƃ��Ȃ�
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intCircuitGrp.ToString() & strchKugiri
                            strLogMsg.Append(typResistorInfoArray(intForCnt).intCircuitGrp & strchKugiri)   '#4.12.2.0�C
                        End If
                        'V4.3.0.0�@��
                    Else
                        'V4.3.0.0�@��
                        If gKeiTyp = KEY_TYPE_RS Then
                            SimpleTrimmer.GetBlockDisplayNumber(NowBlockNo)
                            ' �T�[�L�b�g�ԍ���4�����l�߂Őݒ肷��
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & String.Format("{0, -4}", NowBlockNo) & strchKugiri
                            strLogMsg.Append(String.Format("{0, -4}", NowBlockNo) & strchKugiri)    '#4.12.2.0�C
                        Else
                            ' �T�[�L�b�g�ԍ���4�����l�߂Őݒ肷��
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & String.Format("{0, -4}", typResistorInfoArray(intForCnt).intCircuitGrp) & strchKugiri
                            strLogMsg.Append(String.Format("{0, -4}", typResistorInfoArray(intForCnt).intCircuitGrp) & strchKugiri) '#4.12.2.0�C
                        End If
                        'V4.3.0.0�@��
                    End If
                    '----- ###238�� -----

                Case LOGTAR.RESISTOR
                    '----- ###238�� -----
                    '��R�f�[�^�̒ǉ�
                    'strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intResNo.ToString() & strchKugiri
                    If (strchKugiri = ",") Then
                        ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��4�����l�߂Ƃ��Ȃ�
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intResNo.ToString() & strchKugiri
                        strLogMsg.Append(typResistorInfoArray(intForCnt).intResNo & strchKugiri)    '#4.12.2.0�C
                    Else
                        ' �T�[�L�b�g�ԍ���4�����l�߂Őݒ肷��
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & String.Format("{0, -4}", typResistorInfoArray(intForCnt).intResNo) & strchKugiri
                        strLogMsg.Append(String.Format("{0, -4}", typResistorInfoArray(intForCnt).intResNo) & strchKugiri)  '#4.12.2.0�C
                    End If
                    '----- ###238�� -----

                Case LOGTAR.JUDGE
                    '===============================================
                    ' ���茋�ʂ̒ǉ�
                    '===============================================
                    ' ���ݸތ��ʂ�12�ȉ��ł��邩��������B
                    '   ��2009/07/30 432HW��INTRTM�ł́A11�ȍ~�̌��ʂ͐ݒ肳��Ȃ����A
                    '   �@436K�Ƃ̌݊����l�����R�[�h�Ƃ��Ă͂��̂܂�
                    '----- ###238�� -----
                    If (strchKugiri = ",") Then
                        ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��10�������l�߂Ƃ��Ȃ�
                        'If gwTrimResult(intForCnt - 1) <= 12 Then              '###248
                        If gwTrimResult(intForCnt - 1) <= MAX_RESULT_NUM Then   '###248
                            ' ү���ނ��쐬����B
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & gstrResult(gwTrimResult(intForCnt - 1))
                            strLogMsg.Append(gstrResult(gwTrimResult(intForCnt - 1)))               '#4.12.2.0�C
                        Else
                            ' �װү���ނ��쐬����B
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & "ERR=" & CStr(gwTrimResult(intForCnt - 1))
                            strLogMsg.Append("ERR=" & gwTrimResult(intForCnt - 1))                  '#4.12.2.0�C
                        End If
                    Else
                        'If gwTrimResult(intForCnt - 1) <= 12 Then              '###248
                        If gwTrimResult(intForCnt - 1) <= MAX_RESULT_NUM Then   '###248
                            ' ү���ނ��쐬����B(10�������l��)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & gstrResult(gwTrimResult(intForCnt - 1))
                            strLogMsg.Append(gstrResult(gwTrimResult(intForCnt - 1)))               '#4.12.2.0�C
                        Else
                            ' �װү���ނ��쐬����B(10�������l��)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & "ERR=" & String.Format("{0, -6}", gwTrimResult(intForCnt - 1))
                            strLogMsg.Append("ERR=" & String.Format("{0, -6}", gwTrimResult(intForCnt - 1)))    '#4.12.2.0�C
                        End If
                    End If
                    'If gwTrimResult(intForCnt - 1) <= 12 Then
                    '    ' ү���ނ��쐬����B
                    '    strLogMsg = strLogMsg & gstrResult(gwTrimResult(intForCnt - 1))
                    'Else
                    '    ' �װү���ނ��쐬����B
                    '    strLogMsg = strLogMsg & "ERR=" & CStr(gwTrimResult(intForCnt - 1))
                    'End If
                    '----- ###238�� -----
                    '#4.12.2.0�C                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0�C

                Case LOGTAR.TARGET
                    '�ڕW�l�f�[�^�̒ǉ�(###238 16�������l��) 
                    '----- ###238�� -----
                    If (strchKugiri = ",") Then
                        ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��16�������l�߂Ƃ��Ȃ�
                        If (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_RATIO) Then
                            strMakeMsg = "BaseR" + typResistorInfoArray(intForCnt).intBaseResNo.ToString + "*"
                            strMakeMsg = strMakeMsg + typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00")
                        ElseIf (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_CALC) Then
                            '----- ###123 �����V�I�v�Z���̖ڕW�l -----
                            'strMakeMsg = String.Format("{0,9}", gfTargetVal(typResistorInfoArray(intForCnt).intBaseResNo).ToString("0.00000")) ' ###003
                            Call GetRatio2Rn(typResistorInfoArray(intForCnt).intBaseResNo, Rn)
                            strMakeMsg = String.Format("{0,9}", gfTargetVal(Rn - 1).ToString("0.00000"))
                            '----- ###123 �� -----
                        Else
                            strMakeMsg = String.Format("{0,9}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000")) ' ###003
                        End If
                    Else
                        If (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_RATIO) Then
                            ' ���V�I��("BaseR999*9.99")
                            strMakeMsg = "BaseR" + typResistorInfoArray(intForCnt).intBaseResNo.ToString + "*"
                            strMakeMsg = String.Format("{0,16}", strMakeMsg + typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00"))
                        ElseIf (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_CALC) Then
                            '----- V6.0.3.0�L�� -----
                            ' ���V�I�v�Z���̖ڕW�l
                            Call GetRatio2Rn(typResistorInfoArray(intForCnt).intBaseResNo, Rn)
                            'strMakeMsg = String.Format("{0,16}", gfTargetVal(intForCnt - 1).ToString("0.00000"))
                            If (DecPoint = 5) Then                      ' �g���~���O�ڕW�l�̏����������w��(�O�F�ʏ�(5��), �P�F7��)
                                strMakeMsg = String.Format("{0,16}", gfTargetVal(intForCnt - 1).ToString("0.00000"))
                            Else
                                strMakeMsg = String.Format("{0,18}", gfTargetVal(intForCnt - 1).ToStringF(7))
                            End If
                        Else
                            ' ��Βl
                            'strMakeMsg = String.Format("{0,16}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000"))
                            If (DecPoint = 5) Then                      ' �g���~���O�ڕW�l�̏����������w��(�O�F�ʏ�(5��), �P�F7��)
                                strMakeMsg = String.Format("{0,16}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000"))
                            Else
                                strMakeMsg = String.Format("{0,18}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToStringF(7))
                                'strMakeMsg = String.Format("{0,18}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("F7"))
                            End If
                            '----- V6.0.3.0�L�� -----
                        End If
                    End If

                    'If (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_RATIO) Then
                    '    strMakeMsg = "BaseR(" + typResistorInfoArray(intForCnt).intBaseResNo.ToString + ")*"
                    '    strMakeMsg = strMakeMsg + typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00")
                    'ElseIf (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_CALC) Then
                    '    '----- ###123 �����V�I�v�Z���̖ڕW�l -----
                    '    'strMakeMsg = String.Format("{0,9}", gfTargetVal(typResistorInfoArray(intForCnt).intBaseResNo).ToString("0.00000")) ' ###003
                    '    Call GetRatio2Rn(typResistorInfoArray(intForCnt).intBaseResNo, Rn)
                    '    strMakeMsg = String.Format("{0,9}", gfTargetVal(Rn - 1).ToString("0.00000"))
                    '    '----- ###123 �� -----
                    'Else
                    '    strMakeMsg = String.Format("{0,9}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000")) ' ###003
                    'End If
                    '----- ###238�� -----
                    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '#4.12.2.0�C strLogMsg = strLogMsg & strMakeMsg & strchKugiri
                    strLogMsg.Append(strMakeMsg & strchKugiri)                                      '#4.12.2.0�C

                Case LOGTAR.INITIAL
                    '===============================================
                    ' �C�j�V�����e�X�g�̒ǉ�
                    '===============================================
                    '----- ###238�� -----
                    If (strchKugiri = ",") Then
                        ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��16�������l�߂Ƃ��Ȃ�
                        ' SKIP�łȂ��A�C�j�V�����o�͂���̏ꍇ�@
                        If ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                            ((gSysPrm.stLOG.giLoggingDataKind And cLoggingIT) Or (digL = 0))) Then ' ###183
                            ' �C�j�V�������ʂ��t�H�[�}�b�g�ϊ�����������\�z(###238 16�������l��)
                            'strMakeMsg = String.Format("{0,9}", gfInitialTest(intForCnt - 1).ToString("0.00000"))      ' V1.16.0.0�B ###003
                            strMakeMsg = String.Format("{0,9}", gfInitialTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM)) ' V1.16.0.0�B
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0�C
                        Else
                            'SKIP�⃍�O�f�[�^��ʂ̔���=�C�j�V�����o�͂Ȃ��̏ꍇ�́u0.000000�v
                            'strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000")      ' V1.16.0.0�B ###003
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & String.Format("{0,9}", gsEDIT_DIGITNUM) ' V1.16.0.0�B
                            strLogMsg.Append(String.Format("{0,9}", gsEDIT_DIGITNUM)) ' V1.16.0.0�B  '#4.12.2.0�C
                        End If
                    Else
                        ' SKIP�łȂ��A�C�j�V�����o�͂���̏ꍇ�@
                        If ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                            ((gSysPrm.stLOG.giLoggingDataKind And cLoggingIT) Or (digL = 0))) Then
                            ' �C�j�V�������ʂ��t�H�[�}�b�g�ϊ�����������\�z(16�������l��)
                            'strMakeMsg = String.Format("{0,16}", gfInitialTest(intForCnt - 1).ToString("0.00000"))         ' V1.16.0.0�B
                            strMakeMsg = String.Format("{0,16}", gfInitialTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))    ' V1.16.0.0�B
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0�C
                        Else
                            ' SKIP�⃍�O�f�[�^��ʂ̔���=�C�j�V�����o�͂Ȃ��̏ꍇ�́u0.000000(16�������l��)�v
                            'strLogMsg = strLogMsg & String.Format("{0,16}", "0.00000")         ' V1.16.0.0�B
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & String.Format("{0,16}", gsEDIT_DIGITNUM)    ' V1.16.0.0�B
                            strLogMsg.Append(String.Format("{0,16}", gsEDIT_DIGITNUM)) ' V1.16.0.0�B '#4.12.2.0�C
                        End If
                    End If

                    '' SKIP�łȂ��A�C�j�V�����o�͂���̏ꍇ�@
                    'If ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                    '        And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                    '        And gwTrimResult(intForCnt - 1) <> 11 _
                    '        And gwTrimResult(intForCnt - 1) <> 13) And _
                    '    ((gSysPrm.stLOG.giLoggingDataKind And cLoggingIT) Or (digL = 0))) Then ' ###183
                    '    '�C�j�V�������ʂ��t�H�[�}�b�g�ϊ�����������\�z(###238 16�������l��)
                    '    strMakeMsg = String.Format("{0,9}", gfInitialTest(intForCnt - 1).ToString("0.00000"))  ' ###003
                    '    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '    strLogMsg = strLogMsg & strMakeMsg
                    'Else
                    '    'SKIP�⃍�O�f�[�^��ʂ̔���=�C�j�V�����o�͂Ȃ��̏ꍇ�́u0.000000�v
                    '    strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000") ' ###003
                    'End If
                    '----- ###238�� -----
                    '#4.12.2.0�C                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0�C
                Case LOGTAR.FINAL
                    '===============================================
                    ' �t�@�C�i���e�X�g�̒ǉ�
                    '===============================================
                    '----- ###238�� -----
                    If (strchKugiri = ",") Then
                        ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��16�������l�߂Ƃ��Ȃ�
                        If (digL = 3) Then '���胂�[�h�ł͔��肵�Ă��Ȃ����߁A�������ɏo��
                            ' �t�@�C�i�����ʂ��t�H�[�}�b�g�ϊ�����������\�z
                            'strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000"))        ' V1.16.0.0�B ###003
                            strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))   ' V1.16.0.0�B
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0�C
                        ElseIf ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_IT_NG _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                                ((gSysPrm.stLOG.giLoggingDataKind And cLoggingFT) Or (digL <= 2))) Then ' ###183
                            '�t�@�C�i�����ʂ��t�H�[�}�b�g�ϊ�����������\�z(###238 16�������l��)
                            'strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000"))        ' V1.16.0.0�B ###003
                            strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))   ' V1.16.0.0�B
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0�C
                        Else
                            'IT ERR�⃍�O�f�[�^��ʂ̔���=�t�@�C�i���o�͂Ȃ��̏ꍇ�A
                            '�u0.000000�v�Ƀf�[�^��ݒ肵���茋�ʂ��N���A����
                            'strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000")      ' V1.16.0.0�B ###003
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & String.Format("{0,9}", gsEDIT_DIGITNUM) ' V1.16.0.0�B
                            strLogMsg.Append(String.Format("{0,9}", gsEDIT_DIGITNUM)) ' V1.16.0.0�B  '#4.12.2.0�C
                        End If
                    Else
                        If (digL = 3) Then '���胂�[�h�ł͔��肵�Ă��Ȃ����߁A�������ɏo��
                            '�t�@�C�i�����ʂ��t�H�[�}�b�g�ϊ�����������\�z(16�������l��)
                            'strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString("0.00000"))       ' V1.16.0.0�B
                            strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))  ' V1.16.0.0�B
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0�C
                        ElseIf ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_IT_NG _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                                ((gSysPrm.stLOG.giLoggingDataKind And cLoggingFT) Or (digL <= 2))) Then
                            '�t�@�C�i�����ʂ��t�H�[�}�b�g�ϊ�����������\�z(16�������l��)
                            'strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString("0.00000"))       ' V1.16.0.0�B
                            strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))  ' V1.16.0.0�B
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0�C
                        Else
                            'IT ERR�⃍�O�f�[�^��ʂ̔���=�t�@�C�i���o�͂Ȃ��̏ꍇ�A
                            '�u0.000000�v�Ƀf�[�^��ݒ肵���茋�ʂ��N���A����(16�������l��)
                            'strLogMsg = strLogMsg & String.Format("{0,16}", "0.00000")         ' V1.16.0.0�B
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & String.Format("{0,16}", gsEDIT_DIGITNUM)    ' V1.16.0.0�B
                            strLogMsg.Append(String.Format("{0,16}", gsEDIT_DIGITNUM)) ' V1.16.0.0�B '#4.12.2.0�C
                        End If
                    End If

                    'If (digL = 3) Then '���胂�[�h�ł͔��肵�Ă��Ȃ����߁A�������ɏo��
                    '    '�t�@�C�i�����ʂ��t�H�[�}�b�g�ϊ�����������\�z
                    '    strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000")) ' ###003
                    '    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '    strLogMsg = strLogMsg & strMakeMsg

                    'ElseIf ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                    '        And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_IT_NG _
                    '        And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                    '        And gwTrimResult(intForCnt - 1) <> 11 _
                    '        And gwTrimResult(intForCnt - 1) <> 13) And _
                    '        ((gSysPrm.stLOG.giLoggingDataKind And cLoggingFT) Or (digL <= 2))) Then ' ###183
                    '    '�t�@�C�i�����ʂ��t�H�[�}�b�g�ϊ�����������\�z(###238 16�������l��)
                    '    strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000")) ' ###003
                    '    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '    strLogMsg = strLogMsg & strMakeMsg
                    'Else
                    '    'IT ERR�⃍�O�f�[�^��ʂ̔���=�t�@�C�i���o�͂Ȃ��̏ꍇ�A
                    '    '�u0.000000�v�Ƀf�[�^��ݒ肵���茋�ʂ��N���A����
                    '    'strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000") ' ###003
                    'End If
                    '----- ###238�� -----
                    '#4.12.2.0�C                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0�C

                Case LOGTAR.DEVIATION
                    '===============================================
                    ' �������ʂ̒ǉ�
                    '===============================================
                    '----- ###238�� -----
                    'Call TrimLogging_MakeLogDivResult(typResistorInfoArray(intForCnt).intTargetValType, intForCnt, strLogMsg)
                    strMakeMsg = ""
                    Call TrimLogging_MakeLogDivResult(typResistorInfoArray(intForCnt).intTargetValType, intForCnt, strMakeMsg)
                    If (strchKugiri = ",") Then
                        ' ��؂蕶����","�Ȃ烍�O�t�@�C���o�͗p�f�[�^�Ɣ��f��10�������l�߂Ƃ��Ȃ�
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & strMakeMsg
                        strLogMsg.Append(strMakeMsg)                                                '#4.12.2.0�C
                    Else
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & String.Format("{0,10}", strMakeMsg)
                        strLogMsg.Append(String.Format("{0,10}", strMakeMsg))                       '#4.12.2.0�C
                    End If
                    '----- ###238�� -----
                    '#4.12.2.0�C                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0�C

                Case LOGTAR.UCUTPRMNO
                    ' (�޼�SW��1���ڂ�0����(���ݸތ��ʂ�1�܂��́A3))�܂��́A�޼�SW��1���ڂ�1�ł��邩��������B
                    If ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                            Or gwTrimResult(intForCnt - 1) = 12)) Then
                        Dim strWork As String
                        strWork = ""

                        ' U��Ď��s����
                        Call RetrieveUCutResult(intForCnt, strWork)

                        If Len(strWork) Then
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & ", " & strWork & Chr(13) & Chr(10)
                            strLogMsg.Append(", " & strWork & Chr(13) & Chr(10))                    '#4.12.2.0�C
                        Else
                            '#4.12.2.0�C                            strLogMsg = strLogMsg & Chr(13) & Chr(10)
                            strLogMsg.Append(Chr(13) & Chr(10))                                     '#4.12.2.0�C
                        End If
                    Else
                        '#4.12.2.0�C                        strLogMsg = strLogMsg & Chr(13) & Chr(10)
                        strLogMsg.Append(Chr(13) & Chr(10))                                         '#4.12.2.0�C
                    End If
                Case LOGTAR.NOSET
                    ' �������Ȃ�
                Case LOGTAR.END
                    Exit For
            End Select
        Next

        ' 1�f�[�^�����ɂ���؂蕶����}��
        If (addRetFlg = True) Then
            '#4.12.2.0�C            strLogMsg = strLogMsg & vbCrLf
            strLogMsg.AppendLine()                                                                  '#4.12.2.0�C
        Else
            '#4.12.2.0�C            strLogMsg = strLogMsg & strchKugiri
            strLogMsg.Append(strchKugiri)                                                           '#4.12.2.0�C
        End If

        Exit Sub

ErrExit:
        MsgBox("TrimLogging_MakeLogStringData" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region
    'V4.0.0.0-65        ��������
#Region "�J�b�g�������O�o�͗p�̓�����������擾����"
    '''=========================================================================
    ''' <summary>�J�b�g�������O�o�͗p�̓�����������擾����</summary>
    ''' <returns>�J�b�g�������O�o�͗p�̓���������</returns>
    ''' <remarks>V4.0.0.0-65</remarks>
    '''=========================================================================
    Private Function TrimLogging_GetDateFormatString() As String
        ' ү���ތ��ꂪ���B�ȊO�ł��邩��������B
        Dim s As String
        If (2 <> gSysPrm.stTMN.giMsgTyp) Then
            s = ","
        Else
            s = vbTab
        End If

        Return String.Format("{0:yyMMdd} {1:HHmmss}" & s & " ",
                             DateTime.Today, DateTime.Now)
    End Function
#End Region

#Region "�J�b�g���������O�o�̓f�[�^�Ƃ��č\�z����B"
    '''=========================================================================
    ''' <summary>�J�b�g���������O�o�̓f�[�^�Ƃ��č\�z����B</summary>
    ''' <param name="procMsg">File Load, Edit Start, Edit End</param>
    ''' <param name="strLogMsg">۸ޏo�͂��镶�����Ԃ�</param>
    ''' <param name="flgOut">&#38;H01: ��Ĕԍ�, 
    '''                      &#38;H02: �ިڲ, 
    '''                      &#38;H04: ����PX, 
    '''                      &#38;H08: ����PY, 
    '''                      &#38;H10: ��Č��߰��, 
    '''                      &#38;H20: �ؑ��߲��, 
    '''                      &#38;H40: RT�̾��</param>
    ''' <remarks>V4.0.0.0-65</remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogCutCondition(ByRef procMsg As String,
                                                ByRef strLogMsg As String,
                                               Optional ByVal flgOut As Integer = &H1)
        '#4.12.2.0�C    Private Sub TrimLogging_MakeLogCutCondition(ByVal procMsg As String,
        '#4.12.2.0�C                                               ByRef strLogMsg As String,
        '#4.12.2.0�C                                               Optional ByVal flgOut As Integer = &H1)
        ' &H01: ��Ĕԍ�
        ' &H02: �ިڲ
        ' &H04: ����PX
        ' &H08: ����PY
        ' &H10: ��Č��߰��
        ' &H20: �ؑ��߲��
        ' &H40: RT�̾��

        Dim ci As CultureInfo = Thread.CurrentThread.CurrentCulture
        Try
            ' ү���ތ��ꂪ���B�ȊO�ł��邩��������B
            Dim S As String
            If (2 <> gSysPrm.stTMN.giMsgTyp) Then
                S = ","
            Else
                S = vbTab
                ' �o�͕�����̏����_���ςɐݒ肷��
                Dim nfi As New NumberFormatInfo() With {.NumberDecimalSeparator = ","}
                Thread.CurrentThread.CurrentCulture =
                    New CultureInfo("ja-JP") With {.NumberFormat = nfi}
            End If

            Dim sb As New StringBuilder(1024)

            sb.AppendFormat(TrimLogging_GetDateFormatString())      ' ����
            sb.AppendLine(procMsg)      ' File Load, Edit Start, Edit End

            If ((&H1 And flgOut) = &H1) Then sb.Append("CUT-NO" & S) ' ��Ĕԍ�
            If ((&H2 And flgOut) = &H2) Then sb.Append("DELAY" & S) ' �ިڲ
            If ((&H4 And flgOut) = &H4) Then sb.Append("ST-PX" & S) ' ����PX
            If ((&H8 And flgOut) = &H8) Then sb.Append("ST-PY" & S) ' ����PY
            sb.Append("SPEED1" & S)     ' ���x1
            sb.Append("QRATE1" & S)     ' Qrate1
            sb.Append("STEG1" & S)      ' STEG1
            sb.Append("CURRENT1" & S)   ' �d���l1
            sb.Append("TARGET1" & S)    ' �ڕW��ܰ1
            sb.Append("RANGE1" & S)     ' �͈�1
            sb.Append("CUTOFF" & S)     ' ��ĵ�
            If ((&H10 And flgOut) = &H10) Then sb.Append("PAUSE-AFTER-CUT" & S) ' ��Č��߰��
            If ((&H20 And flgOut) = &H20) Then sb.Append("SWITCHING-POINT" & S) ' �ؑ��߲��
            If ((&H40 And flgOut) = &H40) Then sb.Append("RT-OFFSET" & S) ' RT�̾��

            sb.Append("CUT-TYPE" & S)   ' ��Ď��
            sb.Append("DIR" & S)        ' �p�x
            sb.Append("CUT-LEN1" & S)   ' ��Ē�1

            sb.Append("TURN-DIR" & S)   ' ��ݕ���
            sb.Append("R1" & S)         ' R1
            sb.Append("LTURN-P" & S)    ' L���P
            sb.Append("CUT-LEN2" & S)   ' ��Ē�2
            sb.Append("SPEED2" & S)     ' ���x2
            sb.Append("QRATE2" & S)     ' Qڰ�2
            sb.Append("STEG2" & S)      ' STEG2
            sb.Append("CURRENT2" & S)   ' �d���l2
            sb.Append("TARGET2" & S)    ' �ڕW��ܰ2
            sb.Append("RANGE2" & S)     ' �͈�2

            sb.Append("SPEED3" & S)     ' ��đ��x3
            sb.Append("QRATE3" & S)     ' Qڰ�3
            sb.Append("STEG3" & S)      ' STEG3
            sb.Append("CURRENT3" & S)   ' �d���l3
            sb.Append("TARGET3" & S)    ' �ڕW��ܰ3
            sb.Append("RANGE3" & S)     ' �͈�3

            sb.Append("SPEED4" & S)     ' ��đ��x4
            sb.Append("QRATE4" & S)     ' Qڰ�4
            sb.Append("STEG4" & S)      ' STEG4
            sb.Append("CURRENT4" & S)   ' �d���l4
            sb.Append("TARGET4" & S)    ' �ڕW��ܰ4
            sb.Append("RANGE4")         ' �͈�4

            sb.AppendLine()             ' ���s

            For cn As Integer = 1 To typResistorInfoArray(1).intCutCount Step 1
                With typResistorInfoArray(1).ArrCut(cn)
                    If ((&H1 And flgOut) = &H1) Then sb.AppendFormat("{0,2}" & S, .intCutNo) ' ��Ĕԍ�
                    If ((&H2 And flgOut) = &H2) Then sb.AppendFormat("{0,5}" & S, .intDelayTime) ' �ިڲ
                    If ((&H4 And flgOut) = &H4) Then sb.AppendFormat("{0,9:F5}" & S, .dblStartPointX) ' ����PX
                    If ((&H8 And flgOut) = &H8) Then sb.AppendFormat("{0,9:F5}" & S, .dblStartPointY) ' ����PY
                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed)                   ' ���x1
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate)                      ' Qrate1
                    sb.AppendFormat("{0,2}" & S, .FLSteg(0))                        ' STEG1
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(0))                     ' �d���l1
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(0))       ' �ڕW��ܰ1
                    sb.AppendFormat("{0,5:F2}" & S, .dblPowerAdjustToleLevel(0))    ' �͈�1
                    sb.AppendFormat("{0,6:F2}" & S, .dblCutOff)                     ' ��ĵ�
                    If ((&H10 And flgOut) = &H10) Then sb.AppendFormat("{0,5}" & S, .intCutAftPause) ' ��Č��߰��
                    If ((&H20 And flgOut) = &H20) Then sb.AppendFormat("{0,5:F1}" & S, .dblJudgeLevel) ' �ؑ��߲��
                    If ((&H40 And flgOut) = &H40) Then
                        If ((DataManager.CNS_CUTP_NSTr = .strCutType) OrElse
                            (DataManager.CNS_CUTP_NLr = .strCutType)) Then
                            sb.AppendFormat("{0,7:F4}" & S, .dblReturnPos)          ' RT�̾��
                        Else
                            sb.Append("       " & S)
                        End If
                    End If
                    Dim cutType As String
                    Select Case (.strCutType)
                        Case DataManager.CNS_CUTP_NST : cutType = "StCUT"
                        Case DataManager.CNS_CUTP_NL : cutType = "L-CUT"
                        Case DataManager.CNS_CUTP_NSTr : cutType = "StRTN"
                        Case DataManager.CNS_CUTP_NLr : cutType = "L-RTN"
                        Case DataManager.CNS_CUTP_NSTt : cutType = "StTRC"
                        Case DataManager.CNS_CUTP_NLt : cutType = "L-TRC"
                        Case Else : cutType = String.Empty
                    End Select
                    sb.AppendFormat("{0,5}" & S, cutType)                           ' ��Ď��
                    sb.AppendFormat("{0,3}" & S, .intCutAngle)                      ' �p�x
                    If (DataManager.CNS_CUTP_NST = .strCutType) Then
                        sb.AppendFormat("{0,8:F5}", .dblMaxCutLength)               ' ��Ē�1
                        sb.AppendLine()                                             ' ���s
                        Continue For                                                ' ST��Ă̏ꍇ
                    Else
                        sb.AppendFormat("{0,8:F5}" & S, .dblMaxCutLength)           ' ��Ē�1
                    End If

                    If (DataManager.CNS_CUTP_NL = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NLr = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NLt = .strCutType) Then
                        Dim LTurnDir As String
                        If (1 = .intLTurnDir) Then                                  ' CW=1,CCW=2�ɕύX
                            LTurnDir = "CW"
                        ElseIf (2 = .intLTurnDir) Then
                            LTurnDir = "CCW"
                        Else
                            LTurnDir = "??"
                        End If
                        sb.AppendFormat("{0,3}" & S, LTurnDir)                      ' ��ݕ���
                        sb.AppendFormat("{0,8:F5}" & S, .dblR1)                     ' R1
                        sb.AppendFormat("{0,5:F1}" & S, .dblLTurnPoint)             ' L���P
                    Else
                        sb.Append("   " & S & "        " & S & "     " & S)
                    End If
                    sb.AppendFormat("{0,8:F5}" & S, .dblMaxCutLengthL)              ' ��Ē�2
                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed2)                  ' ���x2
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate2)                     ' Qڰ�2
                    Dim idx As Integer
                    If (DataManager.CNS_CUTP_NSTr = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NSTt = .strCutType) Then
                        ' ST���RT�AST���TR�̏ꍇ
                        idx = 4
                    Else
                        ' L��āAL���RT�AL���TR�̏ꍇ
                        idx = 1
                    End If
                    sb.AppendFormat("{0,2}" & S, .FLSteg(idx))                     ' STEG2
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(idx))                  ' �d���l2
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(idx))    ' �ڕW��ܰ2
                    If (DataManager.CNS_CUTP_NL = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NSTr = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NSTt = .strCutType) Then
                        sb.AppendFormat("{0,5:F2}", .dblPowerAdjustToleLevel(idx))  ' �͈�2
                        sb.AppendLine()                                             ' ���s
                        Continue For                                                ' L���,ST���TR,ST���TR�̏ꍇ
                    Else
                        sb.AppendFormat("{0,5:F2}" & S, .dblPowerAdjustToleLevel(idx)) ' �͈�2
                    End If

                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed3)                  ' ��đ��x3
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate3)                     ' Qڰ�3
                    sb.AppendFormat("{0,2}" & S, .FLSteg(4))                        ' STEG3
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(4))                     ' �d���l3
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(4))       ' �ڕW��ܰ3
                    sb.AppendFormat("{0,5:F2}" & S, .dblPowerAdjustToleLevel(4))    ' �͈�3

                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed4)                  ' ��đ��x4
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate4)                     ' Qڰ�4
                    sb.AppendFormat("{0,2}" & S, .FLSteg(5))                        ' STEG4
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(5))                     ' �d���l4
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(5))       ' �ڕW��ܰ4
                    sb.AppendFormat("{0,5:F2}", .dblPowerAdjustToleLevel(5))        ' �͈�4

                    sb.AppendLine()                                                 ' ���s
                End With
            Next cn

            Debug.Print(MethodBase.GetCurrentMethod().Name &
                        "() : sb.Length = " & sb.Length & ", sb.Capacity = " & sb.Capacity &
                        Environment.NewLine & sb.ToString())

            If (strLogMsg Is Nothing) Then strLogMsg = String.Empty
            strLogMsg &= sb.ToString()

        Catch ex As Exception
            MsgBox(MethodBase.GetCurrentMethod().Name & ":" &
                   ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        Finally
            Thread.CurrentThread.CurrentCulture = ci
        End Try

    End Sub
#End Region
    'V4.0.0.0-65        ��������
#Region "�������Z���ʂ̃��O�o�̓f�[�^���\�z����B"
    '''=========================================================================
    ''' <summary>�������Z���ʂ̏o�̓f�[�^���\�z����B</summary>
    ''' <param name="tarValType">(INP)�ڕW�l�w��(0:��Βl,1:���V�I,2:�v�Z��, 3�`9:�������V�I)</param>
    ''' <param name="resCnt">    (INP)��R�f�[�^�̃C���f�b�N�X(1�`)</param>
    ''' <param name="strDivMsg"> (OUT)�ҏW��f�[�^</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogDivResult(ByVal tarValType As Integer, ByVal resCnt As Integer, ByRef strDivMsg As String)

        Dim strMakeMsg As String
        Dim dblDiffdata As Double
        Dim targetVal As Double
        Dim rationBaseRNo As Short

        Try

            strMakeMsg = ""
            dblDiffdata = 0
            rationBaseRNo = 0
            If (tarValType = TARGET_TYPE_CALC) Then
                targetVal = gfTargetVal(resCnt - 1)
            Else
                targetVal = typResistorInfoArray(resCnt).dblTrimTargetVal
            End If

            ' (��R�ް��j�ڕW�l�w����������s���B
            Select Case tarValType
                ' ��Βl/�v�Z���̏ꍇ
                Case TARGET_TYPE_ABSOLUTE, TARGET_TYPE_CALC
                    If targetVal <> 0.0# Then

                        ' (̧���ýĂ̌���/���ݸޖڕW�l*100-100)�̒l���擾����B
                        dblDiffdata = (gfFinalTest(resCnt - 1) / targetVal) * 100.0# - 100.0#

                        ' ��̌v�Z���ʂ�\��۸ޗp��̫�ϯĂ���B�@" xxxxxx%(�v�Z����"
                        strDivMsg = strDivMsg & " " & dblDiffdata.ToString("0.000").PadLeft(3 + 4) & "%"

                        ' ̧��ّ���l�덷(̧��۸ޗp)
                        gfFTest_Par(resCnt - 1) = dblDiffdata
                    Else
                        '----- ###227��-----
                        '' ڼ��ڕW�l��0�̏ꍇ�ɂ́A����ߗp�̕�������쐬����B
                        'strDivMsg = strDivMsg & " " & "    ---" & "%"

                        ' �d���g���~���O�ŖڕW�l��0�̏ꍇ
                        If (typResistorInfoArray(resCnt).intResMeasMode = MEASMODE_VOL) Then
                            ' (̧���ýĂ̌���*100-100)�̒l���擾����B
                            dblDiffdata = gfFinalTest(resCnt - 1) * 100.0#
                            If (typResistorInfoArray(resCnt).intSlope = 1) Then     ' �d��+�X���[�v
                                dblDiffdata = dblDiffdata
                            Else                                                    ' �d��-�X���[�v
                                dblDiffdata = dblDiffdata
                            End If

                            ' ��̌v�Z���ʂ�\��۸ޗp��̫�ϯĂ���B�@" xxxxxx%(�v�Z����"
                            strDivMsg = strDivMsg & " " & dblDiffdata.ToString("0.000").PadLeft(3 + 4) & "%"

                            ' ̧��ّ���l�덷(̧��۸ޗp)
                            gfFTest_Par(resCnt - 1) = dblDiffdata
                        Else
                            ' ڼ��ڕW�l��0�̏ꍇ�ɂ́A����ߗp�̕�������쐬����B
                            strDivMsg = strDivMsg & " " & "    ---" & "%"
                        End If
                        '----- ###227��-----
                    End If

                    ' ���V�I
                Case TARGET_TYPE_RATIO, TARGET_TYPE_CUSRTO_3, TARGET_TYPE_CUSRTO_4,
                        TARGET_TYPE_CUSRTO_5, TARGET_TYPE_CUSRTO_6, TARGET_TYPE_CUSRTO_7,
                        TARGET_TYPE_CUSRTO_8, TARGET_TYPE_CUSRTO_9

                    ' ڼ��ް�No.���擾����B
                    Call GetRatio3Br(resCnt, rationBaseRNo)   ' �ް���R�ԍ��擾
                    If rationBaseRNo < 0 Then
                        ' �װү���ނ��o�͂���B
                        MsgBox("Internal error X003", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                        rationBaseRNo = 0
                    End If

                    ' �ڕW�l(�ŏI�ڕW�l)=�x�[�X��R�����l�����V�I�����擾����B
                    dblDiffdata = gfFinalTest(rationBaseRNo) * targetVal
                    ' �擾�����ڕW�l��0�ȊO�ł��邱�Ƃ��m�F����B
                    If (gTkyKnd = KND_CHIP) Then
                        If (targetVal <> 0.0#) Then
                            dblDiffdata = targetVal
                        Else
                            dblDiffdata = 0
                        End If
                    End If

                    If dblDiffdata <> 0.0# Then
                        ' (̧���ýĂ̌���/���ݸޖڕW�l*100-100)�̒l���擾����B
                        dblDiffdata = (gfFinalTest(resCnt - 1) / dblDiffdata) * 100.0# - 100.0#

                        ' �擾�����v�Z���ʂ�\��۸ޗp��̫�ϯĂ���B
                        strDivMsg = strDivMsg & " " & dblDiffdata.ToString("0.000").PadLeft(3 + 4) & "%"
                        ' ̧��ّ���l�덷(̧��۸ޗp)
                        gfFTest_Par(resCnt - 1) = dblDiffdata
                    Else
                        ' �擾�����ڕW�l��0�̏ꍇ�ɂ́A����ߗp�̕�������쐬����B
                        strDivMsg = strDivMsg & " " & "    ---" & "%"
                    End If

                    '' '' ''    ' �v�Z��
                    '' '' ''Case TARGET_TYPE_CALC
                    '' '' ''    ' ڼ��ڕW�l��0�ȊO�ł��邱�Ƃ��m�F����B
                    '' '' ''    If gfTargetVal(resCnt - 1) <> 0.0# Then
                    '' '' ''        ''�v�Z�l�擾�ϐ�������������B
                    '' '' ''        dblDiffdata = 0

                    '' '' ''        ' (̧���ýČ���/ڼ��ڕW�l*100-100)�̌v�Z���ʂ��擾����B
                    '' '' ''        dblDiffdata = (gfFinalTest(resCnt - 1) / gfTargetVal(resCnt - 1)) * 100.0# - 100.0#

                    '' '' ''        ' �v�Z���ʂ�\��۸ޗp��̫�ϯĂ���B
                    '' '' ''        strDivMsg = strDivMsg & " " & Form1.Utility1.sFormat(dblDiffdata, "0.000", 3 + 4) & "%"
                    '' '' ''    Else
                    '' '' ''        ' ڼ��ڕW�l��0�̏ꍇ�ɂ́A����ߗp�̕�������쐬����B
                    '' '' ''        strDivMsg = strDivMsg & " " & "    ---" & "%"
                    '' '' ''    End If

                    '' '' ''    ' ү���ނ�ڼ��ڕW�l����׽����B(�\��۸ޗp��̫�ϯĂ���B)
                    '' '' ''    strDivMsg = strDivMsg & " " & Form1.Utility1.sFormat(gfTargetVal(resCnt - 1), "0.000000", 10 + 7)
            End Select

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub
#End Region

#Region "�t�@�C�i�����茋�ʂ̃��O�o�̓f�[�^���\�z����B"
    '''=========================================================================
    '''<summary>�t�@�C�i�����茋�ʂ̏o�̓f�[�^���\�z����B
    ''' �����v�Z�͕ʓr���{</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogFinalResult(ByVal tarValType As Integer, ByVal resCnt As Integer, ByRef strFinalMsg As String)

        Dim strMakeMsg As String
        Dim dblDiffdata As Double
        Dim rationBaseRNo As Short
        strMakeMsg = ""
        dblDiffdata = 0
        rationBaseRNo = 0


        Try
            ' (��R�ް��j���Ӱ�ނ��������s���B
            Select Case tarValType

                ' ��Βl, �v�Z��
                Case TARGET_TYPE_ABSOLUTE, TARGET_TYPE_CALC

                    ' ̧���ýČ��ʂ�\��۸ޗp��̫�ϯĂ���B
                    strMakeMsg = gfFinalTest(resCnt - 1).ToString("0.000000").PadLeft(10 + 7)
                    strFinalMsg = strFinalMsg & " FT=" & strMakeMsg
                    ' ���V�I
                Case TARGET_TYPE_RATIO, TARGET_TYPE_CUSRTO_3, TARGET_TYPE_CUSRTO_4,
                        TARGET_TYPE_CUSRTO_5, TARGET_TYPE_CUSRTO_6, TARGET_TYPE_CUSRTO_7,
                        TARGET_TYPE_CUSRTO_8, TARGET_TYPE_CUSRTO_9

                    ' ڼ��ް�No.���擾����B
                    Call GetRatio3Br(resCnt, rationBaseRNo)   ' �ް���R�ԍ��擾
                    If rationBaseRNo < 0 Then
                        ' �װү���ނ��o�͂���B
                        MsgBox("Internal error X003", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                        rationBaseRNo = 0
                    End If

                    ' ̧���ýČ��ʂ�0�ȊO�ł��邩��������B
                    If gfFinalTest(rationBaseRNo) <> 0 Then
                        ' �v�Z�l�擾�ϐ�������������B
                        dblDiffdata = 0

                        ' �䗦���擾����B(̧���ýČ��ʒl/�ް�̧���ýČ��ʒl)
                        If (gfFinalTest(rationBaseRNo) <> 0) Then
                            dblDiffdata = gfFinalTest(resCnt - 1) / gfFinalTest(rationBaseRNo)

                            ' �\��۸ޗp�Ɍv�Z���ʂ�̫�ϯĂ���B
                            strMakeMsg = dblDiffdata.ToString("0.000000").PadLeft(10 + 7)
                        Else
                            ' ̧���ýČ��ʂ�0�̏ꍇ�ͽ���ߗp�̕�������쐬����B
                            strMakeMsg = Space(14) & "---"
                        End If
                    End If
                    ' �쐬����ү���ނ�ݒ肷��B�@�@" FT=" + strMakeMsg
                    strFinalMsg = strFinalMsg & " FT=" & strMakeMsg
            End Select

            If (gESLog_flg = True) Then     'V1.14.0.0�@
                Call TrimLoggingResult_ES()
                Call LoggingWrite_ES()
            End If
        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub
#End Region

#Region "���M���O����(���O�\�����/���Y�Ǘ����)"
    '''=========================================================================
    ''' <summary>���M���O����</summary>
    ''' <param name="digH">         (INP) �\�����[�h�i�f�W�^���X�C�b�`��ʂP���j</param>
    ''' <param name="digL">         (INP) �g���~���O���[�h�i�f�W�^���X�C�b�`���ʂP���j</param>
    ''' <param name="pltNoX">       (INP) �v���[�g�ԍ�X</param>
    ''' <param name="pltNoY">       (INP) �v���[�g�ԍ�Y</param>
    ''' <param name="blkNoX">       (INP) �u���b�N�ԍ�X</param>
    ''' <param name="blkNoY">       (INP) �u���b�N�ԍ�Y</param>
    ''' <param name="strLogMsgBuf"> (INP) </param>
    ''' <param name="strFileLogMsg">(INP) ���O�t�@�C���o�͗p���O�f�[�^</param>   
    ''' <param name="strDispLogMsg">(INP) ��ʕ\���p���O�f�[�^</param>  
    '''=========================================================================
    Private Sub TrimLogging_LoggingStart(ByVal digH As Integer, ByVal digL As Integer,
                                         ByVal pltNoX As Integer, ByVal pltNoY As Integer,
                                         ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                         ByVal strLogMsgBuf As StringBuilder,
                                         ByVal strFileLogMsg As StringBuilder,
                                         ByVal strDispLogMsg As StringBuilder)
        '#4.12.2.0�C    Private Sub TrimLogging_LoggingStart(ByVal digH As Integer, ByVal digL As Integer, ByRef pltNoX As Integer, ByRef pltNoY As Integer,
        '#4.12.2.0�C                                        ByRef blkNoX As Integer, ByRef blkNoY As Integer,
        '#4.12.2.0�C                                        ByRef strLogMsgBuf As String, ByRef strFileLogMsg As String,
        '#4.12.2.0�C                                        ByRef strDispLogMsg As String)
        '''''''�X���b�h�𐶐����\�������̌������������B���̓��M���O�Ɏg�p���Ă���f�[�^
        '' '' ''Dim m_digH, m_digL As Integer
        '' '' ''Dim m_strLogMsgBuf, m_strFileLogMsg, m_strDispLogMsg As String

        '' '' ''Private Sub TrimLogging_LoggingStart()
        Dim dblTotalCnt As Double
        'Dim i As Integer
        Dim strMSG As String

        Try
            ' ۷�ݸ޽����׸ނ���������B
            If gSysPrm.stLOG.giLoggingMode = 1 Then
                '   �R�R�ł́A���M���O�f�[�^�̐ݒ���s���悤�ɂ���
                '   �����݂͍s��Ȃ��悤�ɕύX����B
                '#4.12.2.0�C                strLogMsgBuf = strLogMsgBuf + strFileLogMsg
                strLogMsgBuf.Append(strFileLogMsg.ToString())   '#4.12.2.0�C
                '        Call TrimLogging_Write(strLogMsg, False)   ' gsDataLogStr = gsDataLogStr + m_strMsg
                'm_strFileLogMsg = ""
                '#4.12.2.0�C                strFileLogMsg = ""
                strFileLogMsg.Length = 0                        '#4.12.2.0�C
            End If

            ' �\�����[�h�̃`�F�b�N
            '�@�\���Ȃ��idigH=0)�̏ꍇ�͉����\�����Ȃ��B
            If (digH = 0) Then
                '���O�\���Ȃ�
            Else
                ''V4.0.0.0-63
                If gKeiTyp = KEY_TYPE_RS Then
                Else
                    '��\�����[�h�ȊO�̓��O�G���A�Ƀ��b�Z�[�W�\��
                    Call Form1.Z_PRINT(strDispLogMsg)
                End If
            End If

            '��ʂ̃��t���b�V��                                         ' �ĕ`�悷��Ǝ��Ԃ������邽�ߍ폜 ###097
            'Call Form1.Refresh()

            '-----------------------------------------------------------------------
            '   Frame1���(���Y�Ǘ����)�\��
            '-----------------------------------------------------------------------
            ' �Ǖs�Ǎ��v���A�s�Ǘ��\��
            Fram1LblAry(FRAM1_ARY_GO).Text = m_lGoodCount.ToString("0")                         ' GO��(�T�[�L�b�g��)
            Fram1LblAry(FRAM1_ARY_NG).Text = m_lNgCount.ToString("0")                           ' NG��(�T�[�L�b�g��)
            Fram1LblAry(FRAM1_ARY_REGNUM).Text = (m_lGoodCount + m_lNgCount).ToString("0")      ' RESISTOR��
            If (m_lGoodCount + m_lNgCount) > 0 Then
                dblTotalCnt = CDbl(m_lNgCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_NGPER).Text = dblTotalCnt.ToString("0.00") + "%"          ' NG%
            Else
                Fram1LblAry(FRAM1_ARY_NGPER).Text = " - %"                                      ' NG%
            End If

            ' NG����\�����x���ɐݒ肷��
            Fram1LblAry(FRAM1_ARY_ITHING).Text = m_lITHINGCount.ToString("0")        ' IT HI NG��
            Fram1LblAry(FRAM1_ARY_FTHING).Text = m_lFTHINGCount.ToString("0")        ' FT HI NG��
            Fram1LblAry(FRAM1_ARY_ITLONG).Text = m_lITLONGCount.ToString("0")        ' IT LO NG��
            Fram1LblAry(FRAM1_ARY_FTLONG).Text = m_lFTLONGCount.ToString("0")        ' FT LO NG��
            Fram1LblAry(FRAM1_ARY_OVER).Text = m_lITOVERCount.ToString("0")          ' OVER��

            ' NG ����\������
            If (m_lGoodCount + m_lNgCount) > 0 Then
                dblTotalCnt = CDbl(m_lITHINGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_ITHINGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lITLONGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_ITLONGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lFTHINGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_FTHINGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lFTLONGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_FTLONGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lITOVERCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_OVERP).Text = dblTotalCnt.ToString("0.00")
            Else
                Fram1LblAry(FRAM1_ARY_ITHINGP).Text = " -.-- "                  ' IT HI NG%
                Fram1LblAry(FRAM1_ARY_ITLONGP).Text = " -.-- "                  ' IT LO NG%
                Fram1LblAry(FRAM1_ARY_FTHINGP).Text = " -.-- "                  ' FT HI NG%
                Fram1LblAry(FRAM1_ARY_FTLONGP).Text = " -.-- "                  ' FT LO NG%
                Fram1LblAry(FRAM1_ARY_OVERP).Text = " -.-- "                    ' OVER NG%
            End If

            ' �\�����x���ĕ`��                                          ' �ĕ`�悷��Ǝ��Ԃ������邽�ߍ폜 ###097
            'For i = 0 To (MAX_FRAM1_ARY - 1)
            '    Fram1LblAry(i).Refresh()
            'Next i

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.ClearCounter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Err.Clear()
        End Try

    End Sub

#End Region

#Region "Index2 ���茋�ʂ̎擾(ROHM)"
    '''=========================================================================
    '''<summary>Index2 ���茋�ʂ̎擾(ROHM)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingResult_Index2()

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim intSetData As Short
        Dim intForCnt As Short
        Dim i As Short
        Dim ii As Short

        '----- V1.18.0.0�E�� -----
        ' IX2���O�o�͂Ȃ��Ȃ�NOP
        If (giIX2LOG = 0) Or (gSysPrm.stDEV.rIX2Log_flg = False) Then
            Exit Sub
        End If
        '----- V1.18.0.0�E�� -----

        ' ��R����ٰ��
        For i = 0 To gRegistorCnt - 1
            ' ��Đ�/15�̐��������擾����B
            intCntMax = Int(typResistorInfoArray(i + 1).intCutCount / 15)
            ' �ő�l�܂ŏ������s�Ȃ��B
            For intForCnt = 0 To intCntMax
                ' �������ő�l�ł��邩��������B
                If intForCnt < intCntMax Then
                    ' �ő�l�̏ꍇ�́A15�ŏ������s�Ȃ��B
                    intSetData = 15
                Else
                    ' �ő�l�ȊO�̏ꍇ�́A15�ŏ��Z�����]����擾����B
                    intSetData = typResistorInfoArray(i + 1).intCutCount Mod 15
                End If
                ' �擾�����l��0�̏ꍇ�ɂ́A�����𔲂���B
                If intSetData = 0 Then Exit For

#If cOFFLINEcDEBUG Then
#Else
                ' INDEX2�J�b�g�񐔎擾 V1.18.0.0�E
                Call TRIM_RESULT_WORD(RSLTTYP_IX2_CUTCOUNT, i, intSetData, intForCnt * 15, 0, gfIndex2TestNum(intForCnt * 15))
                ' INDEX2����l�擾
                Call TRIM_RESULT_Double(RSLTTYP_IX2_MEAS, i, intSetData, intForCnt * 15, 0, gfIndex2Test(intForCnt * 15))
#End If
            Next

            ' �ޯ̧�Ɋi�[
            For ii = 0 To typResistorInfoArray(i + 1).intCutCount - 1
                gfIndex2ResultNum(i, ii) = gfIndex2TestNum(ii)  ' INDEX2�J�b�g��
                gfIndex2Result(i, ii) = gfIndex2Test(ii)        ' INDEX2����l
            Next ii
        Next i

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_Index2" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "Index2 ۸ޏo��(ROHM)"
    '''=========================================================================
    '''<summary>Index2 ۸ޏo��(ROHM)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub LoggingWrite_Index2()

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim i As Short
        Dim c As Short
        Dim CutNum As Short
        Dim strDATA As String
        'Dim ff As Integer

        '----- V1.18.0.0�E�� -----
        ' IX2���O�o�͂Ȃ��Ȃ�NOP
        If (giIX2LOG = 0) Or (gSysPrm.stDEV.rIX2Log_flg = False) Then
            Exit Sub
        End If
        '----- V1.18.0.0�E�� -----

        '��R�����擾
        intCntMax = gRegistorCnt
        strDATA = ""

        For i = 1 To intCntMax
            CutNum = typResistorInfoArray(i).intCutCount '��Đ��擾
            For c = 1 To CutNum
                ' ��R�ԍ�
                strDATA = strDATA & "R" & typResistorInfoArray(i).intResNo.ToString("000") & ","
                ' ��Ĕԍ�
                strDATA = strDATA & "C" & Form1.ToString("00") & ","
                ' ��
                strDATA = strDATA & gfIndex2ResultNum(i - 1, c - 1).ToString("0") & ","
                ' ����l
                strDATA = strDATA & gfIndex2Result(i - 1, c - 1).ToString("0.00000")
                ' CRLF
                If c <> CutNum Then strDATA = strDATA & vbCrLf
            Next c
            If i <> intCntMax Then strDATA = strDATA & vbCrLf
        Next

        'ff = FreeFile()
        'FileOpen(ff, sIX2LogFilePath, OpenMode.Append)
        'PrintLine(ff, strDATA)
        'FileClose(ff)
        Using sw As New StreamWriter(sIX2LogFilePath, True, Encoding.UTF8)      ' �ǋL UTF8 BOM�L notepad.exe ����   V4.4.0.0-0
            sw.WriteLine(strDATA)
        End Using

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_Index2" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "���M���O�J�n(�W��)"
    '''=========================================================================
    '''<summary>���M���O�J�n(�W��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TrimLogging_Start2()

        Dim s As String
        Dim chKugiri As String
        Dim chFloatPoint As String

        Try
            ' ���Ƀw�b�_�o�͍ς݂Ȃ甲����
            ''V1.13.0.1�@           '            If gLoggingHeader = False Then Exit Sub
            If (gLoggingHeader = False) And (typPlateInfo.intIDReaderUse = 0) Then Exit Sub

            '#4.12.2.0�C            s = ""
            's = typPlateInfo.strDataName & vbCrLf          'V4.0.0.0-65

            If gSysPrm.stTMN.giMsgTyp <> 2 Then
                chKugiri = ","
                chFloatPoint = "."
            Else
                chKugiri = Chr(KUGIRI_CHAR) ' TAB
                chFloatPoint = ","
            End If

            Dim sb As New StringBuilder(64)                                             '#4.12.2.0�C
            'x0�p�ɐݒ��ύX���ăw�b�_�[���\�z
            '#4.12.2.0�C            Call TrimLogging_MakeLogHeader(m_TrimlogFileFormat, LOGTYPE_FILE, chKugiri, s)
            TrimLogging_MakeLogHeader(m_TrimlogFileFormat, LOGTYPE_FILE, chKugiri, sb)  '#4.12.2.0�C
            s = sb.ToString()                                                           '#4.12.2.0�C

            ''V1.13.0.1�@
            If (gLoggingHeader = False) Then
            Else
                Call TrimLogging_Write(s)
            End If
            ' IDRead�p��Log�������� 'V1.13.0.0�E
            If (typPlateInfo.intIDReaderUse = 1) And (gLoggingHeader = False) Then
                s = typPlateInfo.strDataName & vbCrLf & s       'V4.0.0.0-65
                MakeTmpLogFile(globalLogFileName, s)
            End If
            ''V1.13.0.1�@

            gLoggingHeader = False

            Exit Sub

        Catch ex As Exception
        End Try
    End Sub

#End Region
    'V4.0.0.0-65        ��������
#Region "���O�t�@�C���V�K�쐬�܂��͒ǋL"
    '''=========================================================================
    ''' <summary>���O�t�@�C���V�K�쐬�܂��͒ǋL</summary>
    ''' <param name="procMsg">File Load, Edit Start, Edit End, Logging ON, Logging OFF �Ȃ�</param>
    ''' <param name="dataFileName">۸�̧�ِ擪�ɏ��������ް�̧�ٖ�(�w�肷��ƐV�K۸�̧�ٍ쐬)</param>
    ''' <param name="procMsgOnly">True:��ď������܂܂Ȃ�۸ޏo��(False�ł༽��ׂ̐ݒ肪0�̏ꍇ�͏o�͂��Ȃ�)</param>
    ''' <remarks>V4.0.0.0-65</remarks>
    '''=========================================================================
    Public Sub TrimLogging_CreateOrAppend(ByRef procMsg As String,
                                          Optional ByVal dataFileName As String = "",
                                          Optional ByVal procMsgOnly As Boolean = False)
        '#4.12.2.0�C    Public Sub TrimLogging_CreateOrAppend(ByVal procMsg As String,
        '#4.12.2.0�C                                          Optional ByVal dataFileName As String = "",
        '#4.12.2.0�C                                          Optional ByVal procMsgOnly As Boolean = False)
        Dim dataName As String
        If (String.Empty <> dataFileName) Then
            ' ۸�̧�ِV�K�쐬
            dataName = dataFileName & Environment.NewLine               ' �ް�̧�ٖ�����������
            TrimLogging_Write(dataName, True)                           ' �ʏ��۸�̧�ق�V�K�쐬����
            Globals_Renamed.gLoggingHeader = True                       ' ۸�ͯ�ް�����ݎw���׸�(TRUE:�o��)
        Else
            ' ����۸�̧�ق��g�p
            dataName = String.Empty                                     ' �ް�̧�ٖ��͏������܂Ȃ�
        End If

        If (True = procMsgOnly) Then
            ' ������t������ procMsg �̂ݏo�͂���
            Dim proc As String = TrimLogging_GetDateFormatString() & procMsg & Environment.NewLine

            Select Case gSysPrm.stLOG.giLogPutCutCond
                Case 1  ' ����۸�̧�قɏ�������
                    TrimLogging_Write(proc)                             ' �ʏ��۸�̧�قɏo�͂���

                Case 2  ' ��ď���۸�̧�قɂ̂ݏ�������
                    TrimLogging_Write(dataName & proc, outFile:=1)      ' ��ď���۸�̧�قɏo�͂���

                Case 3  ' ����۸�̧�قƶ�ď���۸�̧�قɏ�������
                    TrimLogging_Write(dataName & proc, outFile:=1)      ' ��ď���۸�̧�قɏo�͂���
                    TrimLogging_Write(proc)                             ' �ʏ��۸�̧�قɏo�͂���

                Case Else ' �����͏������܂Ȃ�
                    ' DO NOTHING
            End Select

        Else
            ' ��ď������o�͂���
            Dim cut As String = String.Empty
            If (String.Empty <> dataName) Then dataName &= Environment.NewLine

            Select Case gSysPrm.stLOG.giLogPutCutCond
                Case 1  ' ����۸�̧�قɶ�ď�������������
                    TrimLogging_MakeLogCutCondition(procMsg, cut)
                    TrimLogging_Write(cut)                              ' �ʏ��۸�̧�قɏo�͂���

                Case 2  ' ��ď���۸�̧�قɂ̂ݏ�������
                    TrimLogging_MakeLogCutCondition(procMsg, cut)
                    TrimLogging_Write(dataName & cut, outFile:=1)       ' ��ď���۸�̧�قɏo�͂���

                Case 3  ' ����۸�̧�قƶ�ď���۸�̧�قɏ�������
                    TrimLogging_MakeLogCutCondition(procMsg, cut)
                    TrimLogging_Write(dataName & cut, outFile:=1)       ' ��ď���۸�̧�قɏo�͂���
                    TrimLogging_Write(cut)                              ' �ʏ��۸�̧�قɏo�͂���

                Case Else ' ��ď����͏������܂Ȃ�
                    ' DO NOTHING
            End Select
        End If

    End Sub
#End Region
    'V4.0.0.0-65        ��������
#Region "���M���O ������"
    '''=========================================================================
    '''<summary>���M���O ������</summary>
    ''' <param name="createFile">True:�V�K�쐬, False:�쐬���Ȃ�  V4.0.0.0-65</param>
    ''' <param name="outFile">�o�͐�̧�َw�� 0:�ʏ�۸�̧��, 1:��ď���̧��  V4.0.0.0-65</param>
    '''<remarks>LoggingWrite�̓g���~���O�I�����Ɏ��{����</remarks>
    '''=========================================================================
    Private Function TrimLogging_Write(ByRef s As String,
                                       Optional ByVal createFile As Boolean = False,
                                       Optional ByVal outFile As Integer = 0) As Short
        Dim n As Short
        'Dim ff As Short
        Dim sPath As String
        Dim strFile As String
        Dim strExt As String

        '    gsDataLogStr = gsDataLogStr + s

        ' ���O�t�@�C����������
        '    If giLoggingMode Then
        'If (gLoggingHeader) Then ' ͯ�ް�������ݎw���Ȃ̂�̧�ٖ��쐬(�����b�܂�)
        If (True = createFile) Then ' ̧�ِV�K�쐬              'V4.0.0.0-65
            strLogFileNameDate = Today.ToString("yyyyMMdd") & TimeOfDay.ToString("HHmmss")
        End If

        ' �f�[�^�t�@�C�����̒��o
        n = InStrRev(gSysPrm.stLOG.gsLoggingFile, ".", , CompareMethod.Text)
        If n > 0 Then
            strFile = Left(gSysPrm.stLOG.gsLoggingFile, n - 1)
            strExt = Right(gSysPrm.stLOG.gsLoggingFile, Len(gSysPrm.stLOG.gsLoggingFile) - (n - 1))
        Else
            strFile = gSysPrm.stLOG.gsLoggingFile
            strExt = ""
        End If
        'V4.0.0.0-65                    ��������
        'sPath = gSysPrm.stLOG.gsLoggingDir & strFile & strLogFileNameDate & strExt
        Select Case (outFile)
            Case 1
                sPath = gSysPrm.stLOG.gsLoggingDir & strFile & "_" & strLogFileNameDate & "_Cut" & strExt
            Case Else
                sPath = gSysPrm.stLOG.gsLoggingDir & strFile & "_" & strLogFileNameDate & strExt
                LogFileSaveName = sPath                     'V1.13.0.0�K
        End Select
        'V4.0.0.0-65                    ��������

        On Error GoTo ERROR01
        'ff = FreeFile()
        Dim append As Boolean
        If (0 <> gSysPrm.stLOG.giLoggingAppend) Then               ' ���M���O�̓A�y���h���[�h ?
            'FileOpen(ff, sPath, OpenMode.Append)
            append = True                                               ' �ǋL    V4.4.0.0�@
        Else
            'FileOpen(ff, sPath, OpenMode.Output)
            append = False                                              ' �㏑    V4.4.0.0�@
        End If

        Using sw As New StreamWriter(sPath, append, Encoding.UTF8)      ' UTF8 BOM�L notepad.exe ����   V4.4.0.0-0
            sw.WriteLine(s)
        End Using

        'PrintLine(ff, s)
        'FileClose(ff)
        ' IDRead�p��Log�������� 'V1.13.0.0�E
        'If typPlateInfo.intIDReaderUse = 1 Then                        ' �����ł͉����o�͂������H V4.0.0.0-65
        If typPlateInfo.intIDReaderUse = 1 AndAlso (0 = outFile) Then   ' V4.0.0.0-65
            MakeTmpLogFile(globalLogFileName, s)
        End If
        On Error GoTo 0

        '    End If

        '    gsDataLogStr = ""

        Exit Function

ERROR01:
        Resume ERROR02
ERROR02:
        On Error GoTo 0
        ' "���O�t�@�C���̏����݂ŃG���[���������܂���"
        Call Form1.System1.TrmMsgBox(gSysPrm, MSG_LOGERROR, MsgBoxStyle.OkOnly, TITLE_4)
        Call Form1.System1.OperationLogging(gSysPrm, MSG_LOGERROR, gsDataLogPath)

    End Function
#End Region
    '----- V1.22.0.0�C�� -----
    '=========================================================================
    '   �T�}���[���M���O�֌W����(�I�v�V����)
    '=========================================================================
#Region "�T�}���[���M���O�p�f�[�^������������"
    '''=========================================================================
    ''' <summary>�T�}���[���M���O�p�f�[�^������������</summary>
    ''' <param name="stSummaryLog">(OUT)�T�}���[���M���O�p�f�[�^</param>
    '''=========================================================================
    Public Sub SummaryLoggingDataInit(ByRef stSummaryLog As SummaryLog)

        Dim Rn As Integer
        Dim strMSG As String

        Try
            ' �T�}���[���O�o�͂Ȃ��Ȃ�NOP
            If (giSummary_Log = SUMMARY_NONE) Then Return

            ' �T�}���[���M���O�p�f�[�^�ݒ�
            With stSummaryLog
                ' �T�}���[���M���O�ڍ׍���(�g�[�^��)
                .stTotalSubData.lngItLow = 0                            ' IT LO NG��R��
                .stTotalSubData.lngItHigh = 0                           ' IT HI NG��R��
                .stTotalSubData.lngFtLow = 0                            ' FT LO NG��R��
                .stTotalSubData.lngFtHight = 0                          ' FT HI NG��R��
                .stTotalSubData.lngOpen = 0                             ' �I�[�v���`�F�b�N�G���[��R��
                .stTotalSubData.lngItTotal = 0                          ' IT NG��R��
                .stTotalSubData.lngFtTotal = 0                          ' FT NG��R��

                ' �T�}���[���M���O���
                .strStartTime = ""                                      ' �T�}���[�J�n����
                .strEndTime = ""                                        ' �T�}���[�I������

                ' �T�}���[���M���O���(��R��1-999)
                For Rn = 1 To 999                                      ' �ő��R�����J��Ԃ� 
                    .RegAry(Rn).lngItLow = 0
                    .RegAry(Rn).lngItHigh = 0
                    .RegAry(Rn).lngFtLow = 0
                    .RegAry(Rn).lngItHigh = 0
                    .RegAry(Rn).lngFtHight = 0
                    .RegAry(Rn).lngOpen = 0
                    .RegAry(Rn).lngItTotal = 0
                    .RegAry(Rn).lngFtTotal = 0
                Next

            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.SummaryLoggingDataInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�T�}���[���M���O�p�f�[�^�ݒ菈��"
    '''=========================================================================
    ''' <summary>�T�}���[���M���O�p�f�[�^�ݒ菈��</summary>
    ''' <param name="stSummaryLog">(OUT)�T�}���[���M���O�p�f�[�^</param>
    '''=========================================================================
    Private Sub SummaryLoggingDataSet(ByRef stSummaryLog As SummaryLog, ByVal digH As Integer, ByVal digL As Integer)

        Dim Idx As Integer
        Dim Rn As Integer
        Dim strMSG As String

        Try
            ' �T�}���[���O�o�͂Ȃ��Ȃ�NOP
            If (giSummary_Log = SUMMARY_NONE) Then Return

            ' �g���~���O���[�h��x0,x1,x2���[�h�ȊO�Ȃ�NOP
            If (digL > 2) Then Return

            ' �T�}���[���M���O�p�f�[�^�ݒ�
            With stSummaryLog
                ' �T�}���[���M���O���(��R��1-999)
                For Idx = 1 To gRegistorCnt                             ' ��R�����J��Ԃ� 
                    If (typResistorInfoArray(Idx).intResNo < 1000) Then
                        Rn = typResistorInfoArray(Idx).intResNo         ' Rn = ��R�ԍ�(1-999) 

                        Select Case (gwTrimResult(Idx - 1))
                            Case TRIM_RESULT_IT_LONG
                                .RegAry(Rn).lngItLow = .RegAry(Rn).lngItLow + 1
                            Case TRIM_RESULT_IT_HING
                                .RegAry(Rn).lngItHigh = .RegAry(Rn).lngItHigh + 1
                            Case TRIM_RESULT_FT_LONG
                                .RegAry(Rn).lngFtLow = .RegAry(Rn).lngFtLow + 1
                            Case TRIM_RESULT_FT_HING
                                .RegAry(Rn).lngFtHight = .RegAry(Rn).lngFtHight + 1
                            Case TRIM_RESULT_OVERRANGE
                                .RegAry(Rn).lngOpen = .RegAry(Rn).lngOpen + 1
                        End Select
                    End If
                Next

            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.SummaryLoggingDataSet() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�T�}���[���O�f�[�^�t�@�C�����o�͂���"
    '''=========================================================================
    '''<summary>�T�}���[���O�f�[�^�t�@�C�����o�͂���</summary>
    '''<remarks>LoggingWrite�̓g���~���O�I�����Ɏ��{����</remarks>
    '''=========================================================================
    Public Function SummaryLoggingWrite(ByVal digH As Integer, ByVal digL As Integer) As Integer

        'Dim writer As System.IO.StreamWriter = Nothing
        'Dim bFlg As Boolean = False
        Dim blnRdataTitle As Boolean = False
        Dim RtnCode As Integer = cFRS_NORMAL
        Dim strFilePath As String = ""
        Dim strMSG As String
        Dim strOutText As String                                        ' �o�͓��e
        Dim intStrCnt As Integer                                        ' ��������
        Dim strDAT(2) As String                                         ' �o�͍���(�z��0:�^�C�g���A1:�l�A2:%�f�[�^)
        Dim intArrayCnt As Integer
        Dim intRno As Integer                                           ' ��R�ԍ�
        Dim TotalCnt As Long
        Dim ItTotalCnt As Long
        Dim FtTotalCnt As Long
        Dim OpenCnt As Long

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �T�}���[���O�o�͂Ȃ��Ȃ�NOP
            If (giSummary_Log = SUMMARY_NONE) Then Return (cFRS_NORMAL)

            ' �g���~���O���[�h��x0,x1,x2���[�h�ȊO�Ȃ�NOP
            If (digL > 2) Then Return (cFRS_NORMAL)

            ' �T�}���[���M���O�t�@�C�����̍쐬 = "C:\TRIMDATA\LOG\���H�f�[�^�� + "_SUMMARY.TXT"
            strFilePath = MakeSummaryFileName()
            If (strFilePath = "") Then Return (cFRS_NORMAL)

            '' �T�}�����O�t�@�C���I�[�v��(false = �㏑��(true = �ǉ�))
            'writer = New System.IO.StreamWriter(strFilePath, False, System.Text.Encoding.GetEncoding("Shift_JIS"))
            'bFlg = True

            '-------------------------------------------------------------------
            '   �T�}���[���M���O�o��
            '-------------------------------------------------------------------
            ' �g���~���O�f�[�^���A�J�n���ԁA�I�����Ԃ��o�͂���
            strOutText = ""
            Sub_GetFileName(strFilePath, strDAT(0))                     ' �p�X������T�}���[���M���O�t�@�C���������o��
            strOutText = strOutText & "FILE: " & strDAT(0) & vbCrLf     ' FILE :�g���~���O�f�[�^��  
            strOutText = strOutText & "START TIME: " & stSummaryLog.strStartTime & vbCrLf
            strOutText = strOutText & "END TIME  : " & stSummaryLog.strEndTime & vbCrLf
            strOutText = strOutText & vbCrLf

            ' Total Count���o�͂���
            intStrCnt = 16                                              ' �������� = 16����(�����ɋ󔒃p�f�B���O)
            strDAT(0) = "Total Count".PadLeft(intStrCnt)
            strDAT(1) = ((m_lGoodCount + m_lNgCount).ToString("0")).PadLeft(intStrCnt)
            strOutText = strOutText & vbCrLf & strDAT(0) & vbCrLf & strDAT(1) & vbCrLf & vbCrLf

            ' Go Count, Ng Count(���o��)���o�͂���
            intStrCnt = 13                                              ' �������� = 13����(�����ɋ󔒃p�f�B���O)
            strDAT(0) = "Go Count".PadLeft(intStrCnt) & ","
            strDAT(1) = "Ng Count".PadLeft(intStrCnt)
            strOutText = strOutText & vbCrLf & strDAT(0) & strDAT(1) & vbCrLf
            ' OK, NG�����o�͂���
            intStrCnt = 13                                              ' �������� = 13����(�����ɋ󔒃p�f�B���O)
            strDAT(0) = m_lGoodCount.ToString("0").PadLeft(intStrCnt) & ","
            strDAT(1) = m_lNgCount.ToString("0").PadLeft(intStrCnt)
            strOutText = strOutText & strDAT(0) & strDAT(1)
            ' OK, NG�䗦(%)���o�͂���
            intStrCnt = 13                                              ' �������� = 13����(�����ɋ󔒃p�f�B���O)
            TotalCnt = m_lGoodCount + m_lNgCount
            If (TotalCnt = 0) Then
                strDAT(0) = "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt)
            Else
                strDAT(0) = (((m_lGoodCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," & (((m_lNgCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt)
            End If
            strOutText = strOutText & vbCrLf & strDAT(0) & vbCrLf & vbCrLf

            ' IT.Low�`Open���o�͂���
            ItTotalCnt = m_lITLONGCount + m_lITHINGCount
            FtTotalCnt = m_lFTLONGCount + m_lFTHINGCount
            OpenCnt = m_lITOVERCount
            intStrCnt = 13                                              ' �������� = 16����(�����ɋ󔒃p�f�B���O)
            strDAT(0) = "     IT.Low,      IT.High,     IT.Total,       FT.Low,      FT.High,     FT.Total,         Open"
            If (TotalCnt = 0) Then
                strDAT(1) = "0".PadLeft(11) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt)
                strDAT(2) = "0.000%".PadLeft(11) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt)
            Else
                strDAT(1) = m_lITLONGCount.ToString("0").PadLeft(11) & "," & m_lITHINGCount.ToString("0").PadLeft(intStrCnt) & "," &
                            ItTotalCnt.ToString("0").PadLeft(intStrCnt) & "," & m_lFTLONGCount.ToString("0").PadLeft(intStrCnt) & "," &
                            m_lFTHINGCount.ToString("0").PadLeft(intStrCnt) & "," & FtTotalCnt.ToString("0").PadLeft(intStrCnt) & "," &
                            OpenCnt.ToString("0").PadLeft(intStrCnt)
                strDAT(2) = (((m_lITLONGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(11) & "," &
                            (((m_lITHINGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((ItTotalCnt / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((m_lFTLONGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((m_lFTHINGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((FtTotalCnt / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((OpenCnt / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt)
            End If
            strOutText = strOutText & vbCrLf & strDAT(0) & vbCrLf & strDAT(1) & vbCrLf & strDAT(2) & vbCrLf

            ' ��R���̃f�[�^���o�͂���
            intStrCnt = 13                                              ' �������� = 13����(�����ɋ󔒃p�f�B���O) 
            strDAT(0) = "     R-No,         Open,       IT.Low,      IT.High,       FT.Low,      FT.Higt,        Total"
            For intArrayCnt = 0 To (gRegistorCnt - 1)
                intRno = typResistorInfoArray(intArrayCnt + 1).intResNo ' ��R�ԍ�
                ' ��R�f�[�^�̏ꍇ
                If (intRno < 1000) Then

                    ' �Ή������R�ԍ��̏����o��
                    With stSummaryLog.RegAry(intRno)
                        TotalCnt = .lngItLow + .lngItHigh + .lngFtLow + .lngFtHight + .lngOpen
                        strDAT(1) = intRno.ToString("0").PadLeft(9) & "," & .lngOpen.ToString("0").PadLeft(intStrCnt) & "," &
                                    .lngItLow.ToString("0").PadLeft(intStrCnt) & "," & .lngItHigh.ToString("0").PadLeft(intStrCnt) & "," &
                                    .lngFtLow.ToString("0").PadLeft(intStrCnt) & "," & .lngFtHight.ToString("0").PadLeft(intStrCnt) & "," &
                                    TotalCnt.ToString("0").PadLeft(intStrCnt)

                        ' �^�C�g���s���܂��o�͂��Ă��Ȃ� ?
                        If (blnRdataTitle = False) Then
                            blnRdataTitle = True
                            strOutText = strOutText & vbCrLf & vbCrLf & strDAT(0)
                        End If

                        ' �o�͓��e�̐ݒ�
                        strOutText = strOutText & vbCrLf & strDAT(1)
                    End With
                End If
            Next intArrayCnt

            ' �t�@�C���֏�������
            ' �T�}�����O�t�@�C���I�[�v��(false = �㏑��(true = �ǉ�))
            Using writer As New StreamWriter(strFilePath, False, Encoding.UTF8) 'V4.4.0.0-0
                writer.WriteLine(strOutText)
            End Using

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
STP_END:
            'writer.Close()
            Return (RtnCode)                                            ' Return�l�ݒ�

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            ' "���O�t�@�C���̏����݂ŃG���[���������܂���"
            strMSG = MSG_LOGERROR + " File = " + strFilePath + " Error = " + ex.Message
            Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, TITLE_4)
            Call Form1.System1.OperationLogging(gSysPrm, strMSG, "")
            RtnCode = cERR_TRAP                                         ' Return�l = �g���b�v�G���[����
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V1.22.0.0�C�� -----

    '=========================================================================
    '   ��f�B���C(�f�B���C�g�����Q)���O�֌W����(�I�v�V����)
    '=========================================================================
#Region "�ިڲ���2�p�\���̂̏�����"
    '''=========================================================================
    '''<summary>�ިڲ���2�p�\���̂̏����� V1.23.0.0�E</summary>
    ''' <param name="BlkXCount">(INP)�u���b�N��X</param>
    ''' <param name="BlkYCount">(INP)�u���b�N��Y</param>
    ''' <param name="RegCount"> (INP)��R��</param>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_stDelay2(ByVal BlkXCount As Integer, ByVal BlkYCount As Integer, ByVal RegCount As Integer)

        Dim BXn As Integer
        Dim BYn As Integer
        Dim strMSG As String

        Try
            ' �ިڲ���2�p�\���̂̏�����
            stDelay2.Initialize(BlkXCount, BlkYCount)

            For BXn = 0 To (BlkXCount - 1)
                For BYn = 0 To (BlkYCount - 1)
                    stDelay2.NgAry(BXn, BYn).Initialize(RegCount)
                Next BYn
            Next BXn

            Exit Sub

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.Init_stDelay2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ިڲ���2���s����NG�����p�\���̂̏�����"
    '''=========================================================================
    '''<summary>�ިڲ���2���s����NG�����p�\���̂̏�����</summary>
    ''' <param name="BlkX">(INP)�u���b�N�ԍ�X(1-n) V1.23.0.0�E</param>
    ''' <param name="BlkY">(INP)�u���b�N�ԍ�Y(1-n) V1.23.0.0�E</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetDefaultData(ByVal BlkX As Integer, ByVal BlkY As Integer)

        Dim intForCnt As Integer
        Dim strMSG As String

        Try
            ' �z���NG�����N���A����
            If (m_blnDelayCheck = False) Then Return '              ' �f�B���C�g�����Q�łȂ����NOP V1.23.0.0�E
            For intForCnt = 0 To (gRegistorCnt - 1)                 ' V1.23.0.0�E
                stDelay2.NgAry(BlkX - 1, BlkY - 1).intNgFlag = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intITHiNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intITLoNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intFTHiNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intFTLoNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intOverNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intTotalNGCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intTotalOkCnt = 0
            Next intForCnt

            Exit Sub

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.SetDefaultData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ިڲ���2 �Ƽ��ýĂ̌���(����l)��ێ����Ă���"
    '''=========================================================================
    '''<summary>�ިڲ���2 �Ƽ��ýĂ̌���(����l)��ێ����Ă���</summary>
    ''' <param name="BlkX">(INP)�u���b�N�ԍ�X V1.23.0.0�E</param>
    ''' <param name="BlkY">(INP)�u���b�N�ԍ�Y V1.23.0.0�E</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetInitialTestResultData(ByVal BlkX As Integer, ByVal BlkY As Integer)

        Dim intForCnt As Short
        Dim strMSG As String

        Try
            ' �Ƽ��ýĂ̌���(����l)���R�����ێ����Ă���
            For intForCnt = 0 To (gRegistorCnt - 1)                     ' V1.23.0.0�E
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).dblInitialTest = gfInitialTest(intForCnt)
            Next

            Exit Sub

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.SetInitialTestResultData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '=========================================================================
    '   �d�r���O�֌W����(�I�v�V����)
    '=========================================================================
#Region "ES ���茋�ʂ̎擾"
    '''=========================================================================
    '''<summary>ES ���茋�ʂ̎擾</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingResult_ES()

        On Error GoTo ErrExit

        Dim intRegNum As Short                          ' ��Rٰ�߶���(��R�ԍ�)
        Dim intEsRegCnt As Short
        Dim blnRet As Boolean

        ' ������
        System.Array.Clear(gtyESTestResult, 0, gtyESTestResult.Length)

        intEsRegCnt = 0

        ' ��R����ٰ��
        For intRegNum = 1 To gRegistorCnt

            ' ��R����ES���茋�ʂ̑���
            blnRet = TrimLoggingResult_Es_Reg(intRegNum, intEsRegCnt)

            ' ES��Ă̂����R������
            If blnRet = True Then
                ' ES��Ă�����R���̶�������
                intEsRegCnt = intEsRegCnt + 1
            End If

            If intEsRegCnt >= 10 Then
                ' �ő�P�O�擾�ŏI��
                Exit For
            End If
        Next

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_ES" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "ES ���茋�ʂ̎擾(1��R��)"
    '''=========================================================================
    '''<summary>ES ���茋�ʂ̎擾(1��R��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function TrimLoggingResult_Es_Reg(ByRef intRegNum As Short, ByRef intEsRegCnt As Short) As Boolean

        Dim intCutMax As Short                              ' ��Đ�
        Dim intCutNum As Short                              ' ��Đ�ٰ�߶���(��Ĕԍ�)
        Dim intCutIndex As Short                            ' ��Ĳ��ޯ��(ES��Ă����Ő����ĉ��ڂ�)
        Dim intRetMeasMax(2) As UShort                      ' ����l���@���z��ԍ�1�`2�͂��ݏ��
        Dim intLoopMax As Short                             ' ٰ�ߍő�l
        Dim intLoopCnt As Short                             ' ٰ�߶���
        Dim dGetMeas(15) As Double                          ' ���ʊi�[(128�޲ĕ�) ���z��ԍ�15�͂��ݏ��
        Dim intArrayMeas As Short                           ' ����z����ޯ��
        Dim intTempMax As Short                             ' 1��Ŏ擾���鑪��l���̍ő�l
        Dim intTempCnt As Short                             ' 1��Ŏ擾���鑪��l���̶���
        Dim blnINtimeSend As Boolean                        ' INtime�ɑ��M����K�v�����邩�����׸�

        ' ��Đ����擾����B
        intCutMax = typResistorInfoArray(intRegNum).intCutCount

        ' ��ď��̔z��쐬
        'UPGRADE_WARNING: �z�� gtyESTestResult(intRegNum).iCutNo �̉����� 1 ���� 0 �ɕύX����܂����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' ���N���b�N���Ă��������B
        ReDim Preserve gtyESTestResult(intRegNum).iCutNo(intCutMax)

        ' ������
        intCutIndex = -1

        ' ��Đ���ٰ��
        For intCutNum = 1 To intCutMax

            Select Case UCase(typResistorInfoArray(intRegNum).ArrCut(intCutNum).strCutType)

                Case "K", "S" ' ��Ď�ʂ��G�b�W�Z���X
                    ' ES��Ă̑����׸ނ�ON
                    gtyESTestResult(intRegNum).bEsExsit = True
                    ' ���ڂ�ES���擾
                    intCutIndex = intCutIndex + 1
                    blnINtimeSend = True                    ' ES�Ȃ̂�INtime�ɑ��M
                Case Else
                    blnINtimeSend = False                   ' ES�łȂ��̂ŉ������Ȃ�
            End Select

            If intCutIndex > 3 Then
                blnINtimeSend = False
            End If

            If blnINtimeSend = True Then

                ' ����l�����擾
                '            AppInfo.cmdNo = CMD_TRIM_RESULT
                '            AppInfo.dwPara(0) = 7               '����l���擾�����(ES)
                '            AppInfo.dwPara(1) = intEsRegCnt     '��R�ԍ�(0�`) ��ES������R�����Ő����ĉ��ڂ���n��
                '            AppInfo.dwPara(3) = intCutIndex     '��Đ�(0�`) ��ES�����Ő����ĉ��ڂ���n��
                '
#If cOFFLINEcDEBUG Then
#Else
                ' ����l��(ES2)�̎擾
                Call TRIM_RESULT_WORD(7, intEsRegCnt, 0, intCutIndex, 0, intRetMeasMax(0))
#End If

                ' ����l���̌����Ŕz��쐬
                ReDim Preserve gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intRetMeasMax(0))

                ' 1��Ŏ擾�\�Ȍ���128�޲Ă܂łȂ̂ŁA������K�v�����邩�Z�o
                ' (�擾��Double�^�Ȃ̂�8�޲āB�A���A���݂̏��8�޲ĕK������̂�1��Ŏ������15�܂�)
                intLoopMax = intRetMeasMax(0) \ 15

                ' ����؂��H
                If (intRetMeasMax(0) Mod 15) > 0 Then
                    intLoopMax = intLoopMax + 1             ' 1�񑽂��Ƃ�
                End If

                intArrayMeas = -1
                For intLoopCnt = 1 To intLoopMax
                    System.Array.Clear(dGetMeas, 0, dGetMeas.Length)
                    ' �ŏI�̏ꍇ
                    If intLoopCnt = intLoopMax Then
                        ' �]����擾
                        intTempMax = intRetMeasMax(0) Mod 15
                        ' �]�薳���̏ꍇ
                        If intTempMax = 0 Then
                            intTempMax = 15
                        End If
                    Else
                        intTempMax = 15
                    End If

                    '����l�擾
                    '                AppInfo.cmdNo = CMD_TRIM_RESULT
                    '                AppInfo.dwPara(0) = 8               '����l�擾�����(ES)
                    '                AppInfo.dwPara(1) = intEsRegCnt     '��R�ԍ�(0�`) ��ES������R�����Ő����ĉ��ڂ���n��
                    '                AppInfo.dwPara(2) = intTempMax      '�擾��
                    '                AppInfo.dwPara(3) = intCutIndex             '��Ĕԍ�(0�`)   ��ES�����Ő����ĉ��ڂ���n��
                    '                AppInfo.dwPara(4) = (intLoopCnt - 1) * 15  '�ް��ԍ�(0�`)
                    '
#If cOFFLINEcDEBUG Then
#Else
                    Call TRIM_RESULT_Double(8, intEsRegCnt, intTempMax, intCutIndex, (intLoopCnt - 1) * 15, dGetMeas(0))
#End If

                    For intTempCnt = 1 To intTempMax
                        ' ����l���i�[
                        intArrayMeas = intArrayMeas + 1
                        ReDim Preserve gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intArrayMeas)
                        gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intArrayMeas) = dGetMeas(intTempCnt - 1)

                    Next

                Next  '����l�̎擾��ٰ��

            End If

        Next  ' ��Đ���ٰ��

        If gtyESTestResult(intRegNum).bEsExsit = True Then
            ' ES�J�b�g���P���ł��������ꍇ
            TrimLoggingResult_Es_Reg = True
        Else
            ' ES�J�b�g���P�����Ȃ������ꍇ
            TrimLoggingResult_Es_Reg = False
        End If

    End Function
#End Region

#Region "ES ۸ޏo��"
    '''=========================================================================
    '''<summary>ES ۸ޏo��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub LoggingWrite_ES()

        On Error GoTo ErrExit

        Dim intRegNum As Short
        'Dim intFileNo As Short
        Dim strRet As String
        Dim strDATA As String
        Dim strFileName As String
        Dim intPos As Short

        ' ��R����ٰ��
        strDATA = ""
        For intRegNum = 1 To gRegistorCnt
            ' ES��Ă����݂����R�H
            If gtyESTestResult(intRegNum).bEsExsit = True Then
                strRet = LoggingWrite_ES_GetData(intRegNum)
                strDATA = strDATA & strRet
            End If
        Next

        ' �o�͂�����e������
        If Len(strDATA) > 0 Then
            intPos = InStrRev(gsESLogFilePath, "\")
            strFileName = Mid(gsESLogFilePath, intPos + 1)  ' �t�@�C�����擾�i�g���q�܂ށj

            '�t�@�C�������݂��Ȃ��ꍇ�́A�t�@�C�������o�͂���
            'UPGRADE_WARNING: Dir �ɐV�������삪�w�肳��Ă��܂��B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' ���N���b�N���Ă��������B
            If Len(Dir(gsESLogFilePath)) = 0 Then
                strDATA = "File name:" & strFileName & vbCrLf & vbCrLf & strDATA
            End If

            ' ̧�ق֏�������
            'intFileNo = FreeFile()
            'FileOpen(intFileNo, gsESLogFilePath, OpenMode.Append)
            'PrintLine(intFileNo, strDATA)
            'FileClose(intFileNo)
            Using sw As New StreamWriter(gsESLogFilePath, True, Encoding.UTF8)  ' �ǋL UTF8 BOM�L notepad.exe ����   V4.4.0.0-0
                sw.WriteLine(strDATA)
            End Using

        End If

        Exit Sub

ErrExit:
        MsgBox("LoggingWrite_ES" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub

#End Region

#Region "ES ۸ޓ��e�̍쐬"
    '''=========================================================================
    '''<summary>ES ۸ޓ��e�̍쐬</summary>
    '''<param name="intRegNum">(INP) �Ώۂ̃J�b�g�ԍ�</param>
    '''<returns>ES ۸�</returns>
    '''=========================================================================
    Private Function LoggingWrite_ES_GetData(ByRef intRegNum As Short) As String
        On Error GoTo ErrExit

        Dim intCutMax As Short
        Dim intCutNum As Short
        Dim intMeasCnt As Short
        Dim strDATA As String
        Dim strDATAMeas As String

        ' ��Đ����擾����
        strDATA = ""
        intCutMax = typResistorInfoArray(intRegNum).intCutCount

        ' ��Đ���ٰ��
        For intCutNum = 1 To intCutMax
            strDATAMeas = ""
            Select Case UCase(typResistorInfoArray(intRegNum).ArrCut(intCutNum).strCutType)

                Case "K", "S"                           ' ��Ď�ʂ��G�b�W�Z���X
                    strDATA = strDATA & "R" & typResistorInfoArray(intRegNum).intResNo.ToString("000")
                    strDATA = strDATA & ","
                    strDATA = strDATA & "C" & intCutNum.ToString("00")

                    'For intMeasCnt = 1 To UBound(gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas)
                    For intMeasCnt = 1 To (UBound(gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas) + 1) ' V1.14.0.0�@
                        If Len(strDATAMeas) > 0 Then
                            strDATAMeas = strDATAMeas & ","
                        End If

                        ' ����l
                        'strDATAMeas = strDATAMeas & gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intMeasCnt).ToString("0.00000")
                        strDATAMeas = strDATAMeas & gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intMeasCnt - 1).ToString("0.00000") ' V1.14.0.0�@
                    Next
                    strDATA = strDATA & "," & strDATAMeas & vbCrLf

                Case Else

            End Select

        Next

        LoggingWrite_ES_GetData = strDATA

        Exit Function

ErrExit:
        LoggingWrite_ES_GetData = ""
        Err.Clear()
    End Function

#End Region

#Region "SetHighLow()�����g�p"
    '''=========================================================================
    '''<summary>SetHighLow()</summary>
    '''<param name="intForCnt">(INP)</param> 
    '''<param name="fTest">    (INP) </param>  
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetHighLow(ByRef intForCnt As Short, ByRef fTest() As Double, ByRef fTarget() As Double,
                           ByRef hl As Double, ByRef ll As Double)

        Dim dblr_diff As Double
        Dim intbr As Short

        Try
            With typResistorInfoArray(intForCnt)
                ' ���Ӱ�ނ��������s���B
                Select Case .intTargetValType

                    ' ��Βl
                    Case TARGET_TYPE_ABSOLUTE

                        ' (ýČ���/���ݸޖڕW�l*100-100)�̌v�Z���ʂ��擾����B
                        dblr_diff = (fTest(intForCnt - 1) / .dblTrimTargetVal) * 100.0# - 100.0#

                        ' ���V�I
                    Case TARGET_TYPE_RATIO
                        ' �ް���RNo.���Aڼ��ް�No.���擾����B
                        intbr = GetRatio1BaseNum(.intBaseResNo)
                        ' �擾�����ް�No.��0��菬�����Ȃ�����������B
                        If intbr < 0 Then
                            ' 0��菬�����ꍇ�ɂ́A�װү���ނ��o�͂���B
                            MsgBox("Internal error X003", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                            intbr = 0
                        End If

                        ' (�x�[�X��R�����l�����V�I)���ڕW�l���擾����B
                        dblr_diff = fTest(intbr) * .dblTrimTargetVal
                        ' �擾�����ڕW�l��0�ȊO�ł��邱�Ƃ��m�F����B
                        If dblr_diff <> 0.0# Then

                            ' 0�ȊO�̏ꍇ�́A(ýČ���/�擾�ڕW�l*100-100)�̌v�Z���ʂ��擾����B
                            dblr_diff = (fTest(intForCnt - 1) / dblr_diff) * 100.0# - 100.0#
                        Else
                            ' �ڕW�l��0�̏ꍇ�ɂ́A�v�Z���ʂ�0�Ƃ���B
                            dblr_diff = 0
                        End If

                        ' �v�Z��
                    Case TARGET_TYPE_CALC
                        ' ڼ��ڕW�l��0�ȊO�ł��邱�Ƃ��m�F����B
                        If fTarget(intForCnt - 1) <> 0.0# Then
                            dblr_diff = 0

                            ' (ýČ���/ڼ��ڕW�l*100-100)�̌v�Z���ʂ��擾����B
                            dblr_diff = (fTest(intForCnt - 1) / fTarget(intForCnt)) * 100.0# - 100.0#
                        Else
                            ' ڼ��ڕW�l��0�̏ꍇ�ɂ́A�v�Z���ʂ�0�Ƃ���B
                            dblr_diff = 0
                        End If
                End Select

                If dblr_diff > hl Then
                    giTrimResult0x(intForCnt - 1) = 1
                ElseIf dblr_diff < ll Then
                    giTrimResult0x(intForCnt - 1) = 2
                ElseIf dblr_diff = 0 Then
                    giTrimResult0x(intForCnt - 1) = 3
                Else
                    giTrimResult0x(intForCnt - 1) = 4
                End If
            End With

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub
#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090731�@���M���O�֌W����
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
#Region "���ݸތ���۸ދ��ʕ\�������̏�����"
    '''=========================================================================
    '''<summary>���ݸތ���۸ދ��ʕ\�������̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetTrimResultCmnStr()
        ' �ʏ탍�O
        '----- ###238�� -----
        ' 10�����ɑ�����
        'gstrResult(0) = "0.0000  "
        'gstrResult(1) = "OK      "
        'gstrResult(2) = "IT NG   "
        'gstrResult(3) = "FT NG   "
        'gstrResult(4) = "SKIP    "
        'gstrResult(5) = "BASE NG "
        'gstrResult(6) = "IT HI NG"
        'gstrResult(7) = "IT LO NG"
        'gstrResult(8) = "FT HI NG"
        'gstrResult(9) = "FT LO NG"
        'gstrResult(10) = "OVER RNG"
        'gstrResult(11) = "OPEN"
        'gstrResult(12) = "IT OK TEST OK"
        gstrResult(0) = "0.0000    "
        gstrResult(1) = "OK        "
        gstrResult(2) = "IT NG     "
        gstrResult(3) = "FT NG     "
        gstrResult(4) = "SKIP      "
        gstrResult(5) = "BASE NG   "
        gstrResult(6) = "IT HI NG  "
        gstrResult(7) = "IT LO NG  "
        gstrResult(8) = "FT HI NG  "
        gstrResult(9) = "FT LO NG  "
        gstrResult(10) = "OVER RNG  "
        gstrResult(11) = "OPEN      "   '��INtime������͋A���Ă��Ȃ�(���󖢎g�p)
        gstrResult(12) = "IT OK     "   '
        '----- ###248�� -----
        gstrResult(13) = "PATTERN NG"
        gstrResult(14) = "CUT OK    "   ' 
        gstrResult(15) = "IKEI SKIP "   '��INtime������͋A���Ă��Ȃ�(���󖢎g�p)
        '----- 'V1.13.0.0�J�� -----
        gstrResult(16) = "CV ERROR  "   ' ����΂�����o
        gstrResult(17) = "OVER LOAD "   ' �I�[�o���[�h���o
        gstrResult(18) = "RE PROB NG"   ' �ăv���[�r���O�G���[
        '----- 'V1.13.0.0�J�� -----
        gstrResult(19) = "ES2 ERROR "   ' ES2�G���[ V1.14.0.1�@
        gstrResult(20) = "OPEN NG   "
        gstrResult(21) = "SHORT NG  "
        '----- ###248�� -----
        '----- ###238�� -----
        'V1.20.0.1�@ ��
        gstrResult(22) = "MID CUT NG"
        'V1.20.0.1�@ ��

        '�����������O�\���I��
        If gSysPrm.stSPF.giMeasOKNG <> 0 Then
            'OK/NG�\���Ȃ����[�h
            gstrResult(1) = ""
            gstrResult(6) = "H"
            gstrResult(7) = "L"
            gstrResult(8) = "H"
            gstrResult(9) = "L"
        End If

        'NET�̂݁�432HW��INTRTM�ł́A11�ȍ~�̌��ʂ��ݒ肳��邱�Ƃ͂Ȃ��B
        '   13,14��436K�g���~���O���ʂׁ̈A�R�����g�A�E�g����B
        '    gstrResult(13) = "err13"
        '    gstrResult(14) = "ORL  "
        '===============================================

    End Sub
#End Region

#Region "�ƕ␳�̎��s"
    '''=========================================================================
    '''<summary>�ƕ␳�̎��s</summary>
    '''<param name="pltInfo">(INP) �v���[�g�f�[�^</param>
    '''<returns>cFRS_NORMAL   = ����
    '''         cFRS_ERR_RST  = Cancel(RESET��)
    '''         cFRS_ERR_PTN  = �p�^�[���F���G���[
    '''         ��L�ȊO      = ����~�����̑��G���[
    '''</returns>
    '''=========================================================================
    Private Function DoCorrectPos(ByRef pltInfo As PlateInfo, ByVal doAlign As Boolean, ByVal doRough As Boolean) As Integer 'V5.0.0.9�I
        'Private Function DoCorrectPos(ByRef pltInfo As PlateInfo) As Integer

        Dim strMSG As String = ""
        Dim r As Short
        Dim rtn As Integer = cFRS_NORMAL                                ' ###170

        Try
            ' ����̏ꍇ�ɂ́A�ƕ␳�������s�Ȃ��B
            'If (gSysPrm.stDEV.giTheta = 0) Then                         ' �ƂȂ� ? 
            If (gSysPrm.stDEV.giTheta = 0) OrElse
                ((False = doAlign) AndAlso (False = doRough)) Then      ' �ƂȂ� �܂��� �ƕ␳�����t�A���C�����g�����s���Ȃ�     'V5.0.0.9�I

                gfCorrectPosX = 0.0                                     ' �␳�l������ 
                gfCorrectPosY = 0.0
                gdCorrectTheta = 0.0   'V5.0.0.9�I
                Return (cFRS_NORMAL)
            End If

            ' �摜�\���v���O�������I������ ###156
            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)

            ' ''V5.0.1.0�B�� 'V6.0.5.0�G
            If giGazouClrTime <> 0 Then
                Call Form1.VideoLibrary1.VideoStart()
                Sleep(giGazouClrTime)
                Call Form1.VideoLibrary1.VideoStop()
            End If
            ' ''V5.0.1.0�B�� 'V6.0.5.0�G

            '----- V1.14.0.0�D�� -----
            ' LED�o�b�N���C�g�Ɩ�ON((�I�v�V����) ���[�_�������[�h���L��(432R))
            Call Set_Led(1, 1)                                          ' �o�b�N���C�g�Ɩ�ON 

            '' �o�b�N���C�g�Ɩ������ݒ�(CHIP�p)
            'If gSysPrm.stSPF.giLedOnOff = 1 Then                        ' LED���䂠�� ? 
            '    Call Form1.System1.Set_Led(1, stATLD.iLED, 1)           ' �o�b�N���C�g�Ɩ��n�m(LED����)
            'End If
            '----- V1.14.0.0�D�� -----

            '�ƕ␳����
            '----- V1.15.0.0�B�� -----
            ' Z��ҋ@�ʒu�ֈړ�����
            r = EX_ZMOVE(pltInfo.dblZWaitOffset, MOVE_ABSOLUTE)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)                                              ' Return�l�ݒ�
            End If
            r = PROBOFF_EX(pltInfo.dblZWaitOffset)                      ' ROUND4��ZOFF�ʒu��zWaitPos�Ƃ���
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            '----- V1.15.0.0�B�� -----

            strMSG = ""
            'r = Form1.SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG)
            r = Form1.SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG, doAlign, doRough) 'V5.0.0.9�I
            If (r <> cFRS_NORMAL) Then                                  ' ERROR ?
                rtn = r                                                 ' Return�l ###170
                If (r <= cFRS_VIDEO_PTN) Then                           ' �p�^�[���F���G���[ ?
                    Call Beep()                                         ' Beep��
                    If (strMSG = "") Then                               '###038
                        ' ���O��ʂɕ������\������(�}�b�`���O�G���[, �p�^�[���ԍ��G���[)
                        strMSG = MSG_LOADER_07 + " (" + gbRotCorrectCancel.ToString + ")"
                    Else
                        'If (pltInfo.intRecogDispMode = 0) Then          ' ���ʕ\�����Ȃ� ?
                        If (pltInfo.intRecogDispMode = 0) AndAlso
                            (0 = pltInfo.intRecogDispModeRgh) Then      ' ���ʕ\�����Ȃ� ? 'V5.0.0.9�D
                            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                            '    ' �p�^�[���}�b�`���O�G���[(臒l)
                            '    strMSG = MSG_LOADER_07 + "(臒l)"
                            'Else
                            '    strMSG = MSG_LOADER_07 + "(Thresh)"
                            'End If
                            strMSG = MSG_LOADER_07 & Form1_008
                        End If
                    End If
                    ' ���b�Z�[�W�\��
                    Call Form1.Z_PRINT(strMSG)
                    '----- ###172�� -----
                    If (bFgAutoMode = True) Then                        ' �����^�]��(SL436R) ? 
                        System.Threading.Thread.Sleep(500)              ' Wait(ms)
                    End If
                    '----- ###172�� -----
                    rtn = cFRS_ERR_PTN                                  ' Return�l = �p�^�[���F���G���[ ###170
                End If
            End If

            '�ƕ␳���\��
            If (r = cFRS_NORMAL) Then
                'If (pltInfo.intRecogDispMode = 1) Then                  ' ���ʕ\������ ? '###038
                If (pltInfo.intRecogDispMode = 1) OrElse
                    (0 <> pltInfo.intRecogDispModeRgh) Then             ' ���ʕ\������ ? 'V5.0.0.9�D
                    Call Form1.Z_PRINT(strMSG)
                End If
            End If

            '----- V1.14.0.0�D�� -----
            ' LED�o�b�N���C�g�Ɩ�OFF(�I�v�V����)
            Call Set_Led(1, 0)                                          ' �o�b�N���C�g�Ɩ�OFF 

            '' �o�b�N���C�g�Ɩ������ݒ�(CHIP�p)
            'If gSysPrm.stSPF.giLedOnOff = 1 Then                        ' LED���䂠�� ? 
            '    Call Form1.System1.Set_Led(1, stATLD.iLED, 0)           ' �o�b�N���C�g�Ɩ��n�e�e(LED����)
            'End If
            '----- V1.14.0.0�D�� -----
#If False Then                          'V6.0.0.0�D
            ' �摜�\���v���O�������N������ ###156
            If gKeiTyp = KEY_TYPE_RS Then
                '                r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                ' ������ V3.1.0.0�A 2014/12/01
                r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 0)
                ' ������ V3.1.0.0�A 2014/12/01
            Else

                If (bIniFlg <> 0) And (Form1.chkDistributeOnOff.Checked = False) Then
                    ' ����ȊO�œ��v��ʔ�\�����ɋN������
                    ' ������ V3.1.0.0�A 2014/12/01
                    r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                    ' ������ V3.1.0.0�A 2014/12/01
                End If
            End If
#End If
            Return (rtn)                                                ' ###170

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.DoCorrectPos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                           ' �ߒl = �ׯ�ߴװ����
        End Try
    End Function

#End Region

#Region "�v���[�g�̃`�F�b�N�E�\���X�V"
    '''=========================================================================
    '''<summary>�v���[�g�̃`�F�b�N�E�\���X�V</summary>
    '''<param name="Flg"> (INP) </param> 
    '''<param name="mode">(INP) </param>  
    '''<returns>True= , False= </returns>
    '''=========================================================================
    Private Function CheckPlate(ByRef Flg As Boolean, ByRef mode As Short) As Boolean

        Dim strMSG As String

        Try

            If Not Flg Then
                '----- V6.0.3.0�E�� -----
                'If mode < 3 Then
                If (mode < 3) Or ((giPltCountMode = 1) And ((mode = 3) Or (mode = 5))) Then
                    '----- V6.0.3.0�E�� -----
                    m_lPlateCount = m_lPlateCount + 1                           ' PLATE�� += 1
                    '----- V1.18.0.0�B�� -----
                    ' stPRT_ROHM.Tol_Sheet��m_lPlateCount���g�p���邽�߉��L�͍폜
                    'If (gTkyKnd = KND_CHIP) Then
                    '    ' (���ݸތ��ʈ��)
                    '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                    '        stPRT_ROHM.Pdt_Sheet = stPRT_ROHM.Pdt_Sheet + 1    ' ���ݸ޽ð�ނŏ������������
                    '    End If
                    'End If
                    '----- V1.18.0.0�B�� -----
                End If

                ' Frame1���(���Y�Ǘ����)�\��(PLATE��)
                Fram1LblAry(FRAM1_ARY_PLTNUM).Text = m_lPlateCount.ToString("0")
                Fram1LblAry(FRAM1_ARY_PLTNUM).Refresh()
                CheckPlate = True
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.CheckPlate() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "�J�b�g�ʒu�␳�̎��{�̗L������"   'V3.0.0.0�B ADD START
    Private IsCutPosCorrectExecute As Boolean = False

    Public Function IsCutPosCorrect() As Boolean
        Return (IsCutPosCorrectExecute)
    End Function
#End Region                                'V3.0.0.0�B END

#Region "�J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u����ݒ肷��"
    '''=========================================================================
    '''<summary>�J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u����ݒ肷��</summary>
    '''<param name="registorCnt">(INP)��R��</param>
    '''<param name="pstCutPos">  (OUT)�p�^�[���o�^���</param> 
    '''<returns>�J�b�g�ʒu�␳�����R��</returns>
    '''=========================================================================
    Public Function CutPosCorrectInit(ByVal registorCnt As Integer, ByRef pstCutPos() As CutPosCorrect_Info) As Short

        Dim rn As Integer
        Dim nm As Integer
        Dim Ix As Integer                                                                   '###059
        Dim strMSG As String

        IsCutPosCorrectExecute = False                                                      'V3.0.0.0�B ADD

        Try
            ' �J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u��������������
            nm = 0                                                                          ' �J�b�g�ʒu�␳�����R��
            For rn = 1 To registorCnt                                                       ' �S��R�����ݒ肷�� 
                If (typResistorInfoArray(rn).intResNo < 1) Then Exit For
                ' �p�^�[���F�����ʂ�����������
                Ix = rn - 1                                                                 ' �J�b�g�ʒu�␳�\���̂�0�I���W�� ###059  
                gStCutPosCorrData.corrResult(Ix) = 0                                        ' �p�^�[���F������ =0(�␳�Ȃ�)
                gStCutPosCorrData.corrPosX(Ix) = 0.0                                        ' �Y����X
                gStCutPosCorrData.corrPosY(Ix) = 0.0                                        ' �Y����Y

                'giCutPosRSLT(rn) = 0                                                        ' �p�^�[���F������ =0(�␳�Ȃ�)
                'gfCutPosDRX(rn) = 0.0                                                       ' �Y����X
                'gfCutPosDRY(rn) = 0.0                                                       ' �Y����Y
                gfCutPosCoef(rn) = 0.0                                                      ' ��v�x
                ' �J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u��������������
                If (typResistorInfoArray(rn).intCutReviseMode = 0) Then                     ' ��Ĉʒu�␳�Ȃ� ?
                    ' ��Ĉʒu�␳�Ȃ��̏ꍇ�̓p�^�[���o�^���\���̂�����������        
                    pstCutPos(rn).intFLG = 0                                                ' �J�b�g�ʒu�␳�t���O = 0(���Ȃ�)
                    pstCutPos(rn).intGRP = 0                                                ' ����ݸ�ٰ�ߔԍ�
                    pstCutPos(rn).intPTN = 0                                                ' �e���v���[�g�ԍ�
                    pstCutPos(rn).dblPosX = 0.0                                             ' �p�^�[���ʒuX
                    pstCutPos(rn).dblPosY = 0.0                                             ' �p�^�[���ʒuY
                    pstCutPos(rn).intDisp = 0                                               ' �p�^�[���F�����̌����g�\��(0:�Ȃ�, 1:����)
                Else
                    ' ��Ĉʒu�␳����̏ꍇ�́A��R�f�[�^����p�^�[���o�^���\���̂�ݒ肷��
                    nm = nm + 1                                                             ' �J�b�g�ʒu�␳�����R��
                    pstCutPos(rn).intFLG = 1                                                ' �J�b�g�ʒu�␳�t���O = 1(�␳����)
                    pstCutPos(rn).intGRP = typResistorInfoArray(rn).intCutReviseGrpNo       ' ����ݸ�ٰ�ߔԍ�
                    pstCutPos(rn).intPTN = typResistorInfoArray(rn).intCutRevisePtnNo       ' �e���v���[�g�ԍ�
                    pstCutPos(rn).dblPosX = typResistorInfoArray(rn).dblCutRevisePosX       ' �p�^�[���ʒuX
                    pstCutPos(rn).dblPosY = typResistorInfoArray(rn).dblCutRevisePosY       ' �p�^�[���ʒuY
                    pstCutPos(rn).intDisp = typResistorInfoArray(rn).intCutReviseDispMode   ' �p�^�[���F�����̌����g�\��(0:�Ȃ�, 1:����)
                    IsCutPosCorrectExecute = True                                           'V3.0.0.0�B ADD
                End If
            Next rn

            CutPosCorrectInit = nm                                                          ' �J�b�g�ʒu�␳�����R����Ԃ�

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BasTrimming.CutPosCorrectInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CutPosCorrectInit = 0
        End Try

    End Function
#End Region

#Region "�P�u���b�N���̒�R�̃J�b�g�ʒu�␳�l�����߂�"
    '''=========================================================================
    '''<summary>�P�u���b�N���̒�R�̃J�b�g�ʒu�␳�l�����߂�</summary>
    '''<param name="registorCnt">     (INP)��R��</param> 
    '''<param name="pstCutPos">       (INP)�p�^�[���o�^���</param> 
    '''<param name="cutPosCorDatSend">(OUT)True=1�ł��␳�����������s���ꂽ, False=�p�^�[���F���G���[</param> 
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function CutPosCorrection(ByVal registorCnt As Integer, ByRef pstCutPos() As CutPosCorrect_Info, ByRef cutPosCorDatSend As Boolean) As Integer

        Dim rn As Integer
        Dim x As Double
        Dim y As Double
        Dim ret As Integer
        Dim r As Integer
        Dim strMSG As String
        Dim swSts As Integer
        Dim Ix As Integer                                                   '###059

        Try
            r = cFRS_NORMAL                                                 ' Return�l = ����
            cutPosCorDatSend = False
            giTempGrpNo = -1                                                ' ���݂̃e���v���[�g�O���[�v�ԍ�������(1�`999)
            With gStCutPosCorrData
                For rn = 1 To registorCnt                                   ' �S��R�����ݒ肷��
                    Ix = rn - 1                                             ' �J�b�g�ʒu�␳�\���̂�0�I���W�� ###059  
                    If (pstCutPos(rn).intFLG <> 1) Then                     ' �J�b�g�ʒu�␳�Ȃ� ?
                        .corrResult(Ix) = 0                                 ' �p�^�[���F������ = 0(�␳�Ȃ�)
                        .corrPosX(Ix) = 0.0                                 ' �Y����X
                        .corrPosY(Ix) = 0.0                                 ' �Y����Y
                        'giCutPosRSLT(rn) = 0                               ' �p�^�[���F������ = 0(�␳�Ȃ�)
                        'gfCutPosDRX(rn) = 0.0                              ' �Y����X
                        'gfCutPosDRY(rn) = 0.0                              ' �Y����Y
                    Else
                        ' �p�^�[���F�����s��
                        ret = CutPosPatternMatching(rn, x, y)
                        If (ret = 0) Then
                            ' �J�b�g�I�t�␳�f�[�^���M�t���O
                            If (cutPosCorDatSend <> True) Then
                                cutPosCorDatSend = True
                            End If
                            .corrResult(Ix) = 1                             ' �p�^�[���F������ = 1(OK)
                            .corrPosX(Ix) = x                               ' �Y����X
                            .corrPosY(Ix) = y                               ' �Y����Y
                            'giCutPosRSLT(rn) = 1                           ' �p�^�[���F������ = 1(OK)
                            'gfCutPosDRX(rn) = x                            ' �Y����X
                            'gfCutPosDRY(rn) = y                            ' �Y����Y
                        Else
                            .corrResult(Ix) = 2                             ' �p�^�[���F������ = 2(NG)
                            .corrPosX(Ix) = 0.0                             ' �Y����X
                            .corrPosY(Ix) = 0.0                             ' �Y����Y
                            '    giCutPosRSLT(rn) = 2                       ' �p�^�[���F������ = 2(NG)
                            '    gfCutPosDRX(rn) = 0.0                      ' �Y����X
                            '    gfCutPosDRY(rn) = 0.0                      ' �Y����Y
                        End If

                        '' ADJ�L�[�����`�F�b�N
                        'If Form1.System1.AdjReqSw() Then
                        ' HALT�L�[�����`�F�b�N
                        Call HALT_SWCHECK(swSts)
                        If swSts = cSTS_HALTSW_ON Then
                            ' HALT����ON
                            Call LAMP_CTRL(LAMP_HALT, True)                 ' HALT�����vON

                            ' HALT�L�[��������START/RESET�L�[�����҂�
                            Call STARTRESET_SWWAIT(swSts)
                            If (swSts = cSTS_STARTSW_ON) Then
                                '���̂܂ܔ�����
                            ElseIf (swSts = cSTS_RESETSW_ON) Then
                                'ResetSw ������
                                Call LAMP_CTRL(LAMP_HALT, False)            ' HALT�����vOFF
                                Call LAMP_CTRL(LAMP_RESET, True)            ' RESET�����vON
                                ' "�g���~���O��RESET SW�����ɂ���~"
                                Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMRES, "")
                            End If
                            Call LAMP_CTRL(LAMP_HALT, False)                ' HALT�����vOFF
                        End If
                    End If
                Next rn
            End With

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BasTrimming.CutPosCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                  ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "�p�^�[���}�b�`���O�ɂ��P��R���̈ʒu�␳�����߂�"
    '''=========================================================================
    '''<summary>�p�^�[���}�b�`���O�ɂ��P��R���̈ʒu�␳�����߂�</summary>
    '''<param name="iResistorNum">(INP)�␳�����R�̃e�[�u���ԍ�</param>
    '''<param name="fCorrectX">   (OUT)�����X</param> 
    '''<param name="fCorrectY">   (OUT)�����Y</param> 
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function CutPosPatternMatching(ByVal iResistorNum As Integer, ByRef fCorrectX As Double, ByRef fCorrectY As Double) As Integer

        Dim ret As Short = cFRS_NORMAL
        Dim crx As Double = 0.0                                         ' �����X
        Dim cry As Double = 0.0                                         ' �����Y
        Dim fcoeff As Double = 0.0                                      ' ���֒l
        Dim Thresh As Double = 0.0                                      ' 臒l
        Dim r As Integer = cFRS_NORMAL                                  ' �֐��l
        Dim strMSG As String

        Try
#If VIDEO_CAPTURE = 1 Then
        fCorrectX = 0.0
        fCorrectY = 0.0
        Return (cFRS_NORMAL)   
#Else

            ' �p�^�[���}�b�`���O���̃e���v���[�g�O���[�v�ԍ���ݒ肷��(������ƒx���Ȃ�)
            'V5.0.0.6�B            If (giTempGrpNo <> stCutPos(iResistorNum).intGRP) Then  ' �e���v���[�g�O���[�v�ԍ����ς���� ?
            giTempGrpNo = stCutPos(iResistorNum).intGRP         ' ���݂̃e���v���[�g�O���[�v�ԍ���ޔ�
            Form1.VideoLibrary1.SelectTemplateGroup(giTempGrpNo)             ' �e���v���[�g�O���[�v�ԍ��ݒ�
            'V5.0.0.6�B            End If

            ' 臒l�擾
            Thresh = gDllSysprmSysParam_definst.GetPtnMatchThresh(giTempGrpNo, stCutPos(iResistorNum).intPTN)

            ' �p�[�^�[���ʒuXY��BP�ړ�(��Βl)
            r = Form1.System1.EX_MOVE(gSysPrm, stCutPos(iResistorNum).dblPosX, stCutPos(iResistorNum).dblPosY, 1)
            If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                Return (r)
            End If
            Form1.System1.WAIT(0.1)                                        ' Wait(Sec)
            Form1.VideoLibrary1.Refresh()                           'V3.0.0.0�B ADD �摜�\���������

            ' �p�^�[���}�b�`���O���̌����͈͘g�\��/��\����ݒ肷�� 
            If (stCutPos(iResistorNum).intDisp = 0) Then            ' �p�^�[���F�����̌����g�\��(0:�Ȃ�, 1:����)
                Call Form1.VideoLibrary1.PatternDisp(False)                      ' �����͈͘g��\�� 
            Else
                Call Form1.VideoLibrary1.PatternDisp(True)                       ' �����͈͘g�\�� 
            End If

            ' �p�^�[���}�b�`���O���s��(Video.ocx���g�p)
            'ret = Form1.VideoLibrary1.PatternMatching(iResistorNum, crx, cry, fcoeff)
            ret = Form1.VideoLibrary1.PatternMatching_EX(stCutPos(iResistorNum).intPTN, 0, True, crx, cry, fcoeff) ' ###059
            If (ret <> cFRS_NORMAL) Then
                r = cFRS_ERR_PTN                                    ' RETURN�l = �p�^�[���}�b�`�B���O�G���[
            ElseIf (fcoeff < Thresh) Then
                gfCutPosCoef(iResistorNum) = fcoeff                 ' ��v�x
                r = cFRS_ERR_PTN                                    ' RETURN�l = �p�^�[���}�b�`�B���O�G���[
            Else
                ' �}�b�`�����p�^�[���̑���ʒu���炸��ʂ����߂�
                'fCorrectX = crx / 1000.0#                          ' ###059
                'fCorrectY = -cry / 1000.0#
                fCorrectX = crx
                fCorrectY = cry
                gfCutPosCoef(iResistorNum) = fcoeff                 ' ��v�x
                r = cFRS_NORMAL                                     ' Return�l = ����
            End If

Exit1:
            ' �㏈��
            Debug.Print("X=" & fCorrectX.ToString("0.0000") & " Y=" & fCorrectY.ToString("0.0000"))
            Call Form1.VideoLibrary1.PatternDisp(False)                          ' �����͈͘g��\�� 
            Return (r)
#End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BasTrimming.CutPosPatternMatching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�J�b�g�ʒu�␳�����{����"
    '''=========================================================================
    ''' <summary>�J�b�g�ʒu�␳�����{����</summary>
    ''' <param name="mode">       (INP)���ݸ�Ӱ��(�޼�SW��1��)</param>
    ''' <param name="registorCnt">(INP)��R��</param> 
    ''' <param name="pstCutPos">  (INP)�p�^�[���o�^���</param>
    ''' <returns>�f�B���C ON�FTrue, OFF�FFalse</returns>
    '''=========================================================================
    Private Function DoCutPosCorrect(ByVal mode As Integer, ByVal registorCnt As Integer, ByRef pstCutPos() As CutPosCorrect_Info) As Integer

        Dim rn As Integer
        Dim r As Integer
        Dim bCpNg As Boolean
        Dim strMSG As String
        Dim bSendCutPosCorData As Boolean
        Dim Ix As Integer                                               '###059

        Try
            ' ��Ĉʒu�␳�����{���� ?  
            '###247            If (Form1.stFNC(F_CUTPOS).iDEF = 1) And (mode <> 3) And (registorCnt) Then
            If (Form1.stFNC(F_CUTPOS).iDEF = 1) And (mode <> 2) And (mode <> 3) And (mode <> 6) And (registorCnt) Then
                '------------------------------------------------------------------------
                '   �p�^�[���F�����s���J�b�g�ʒu�̕␳�ʂ����߂�
                '------------------------------------------------------------------------
                r = CutPosCorrection(registorCnt, pstCutPos, bSendCutPosCorData)
                If (r <> cFRS_NORMAL) Then                                              ' �G���[ ? 
                    Return (r)                                                          ' Return�l�ݒ�
                End If

                ' �J�b�g�␳���ʂɂ��ANG�X�L�b�v�ƁA���O�\��
                bCpNg = False                                                           ' NG SKIP�׸�OFF
                For rn = 1 To registorCnt                                               ' �S��R�����ݒ肷�� 
                    Ix = rn - 1                                                         ' �J�b�g�ʒu�␳�\���̂�0�I���W�� ###059  
                    If (pstCutPos(rn).intFLG = 1) Then                                  ' �J�b�g�ʒu�␳���� ?
                        If gStCutPosCorrData.corrResult(Ix) = 1 Then                    ' �p�^�[���F������ = 1(OK) ?
                            ' ���ʕ\������̏ꍇ�̂ݕ\������ '###039
                            If (typResistorInfoArray(rn).intCutReviseDispMode = 1) Then
                                ' �p�^�[���F������ = 1(OK)��
                                strMSG = "Cut Pos Correct: R" + typResistorInfoArray(rn).intResNo.ToString("000") + " " +
                                         gStCutPosCorrData.corrPosX(Ix).ToString("0.0000") + "," + gStCutPosCorrData.corrPosY(Ix).ToString("0.0000") +
                                         "(" + gfCutPosCoef(rn).ToString("##0.0000") + ")"  '###019 
                                Call Form1.Z_PRINT(strMSG)
                            End If

                            '----- ###248�� -----
                            '' NG SKIP�׸�ON�Ȃ�ȍ~��NG SKIP�Ƃ���
                            'If bCpNg Then
                            '    gStCutPosCorrData.corrResult(Ix) = 2                       ' �p�^�[���F������ = 2(NG SKIP)�Ƃ���
                            'End If
                            '----- ###248�� -----
                        Else
                            ' �p�^�[���F������NG��
                            If (gfCutPosCoef(rn) = 0.0) Then
                                strMSG = "Cut Pos Correct: R" + typResistorInfoArray(rn).intResNo.ToString("000") + " Pattern Matching NG"
                            Else
                                strMSG = "Cut Pos Correct: R" + typResistorInfoArray(rn).intResNo.ToString("000") + " Pattern Matching NG" +
                                         "(" + gfCutPosCoef(rn).ToString("##0.0000") + ")" '###019 
                            End If
                            Call Form1.Z_PRINT(strMSG)

                            '----- ###248�� -----
                            '' �摜�F��NG����L�� ?
                            'If (typResistorInfoArray(rn).intIsNG = 0) Then
                            '    bCpNg = True                                                ' NG SKIP�׸�ON
                            'End If

                            ' �摜�F��NG����L�� ?
                            If (typResistorInfoArray(rn).intIsNG = 0) Then                  ' �摜�F��NG����(0:����, 1:�Ȃ�, �蓮(���݂͖�����))
                                gStCutPosCorrData.corrResult(Ix) = 2                        ' �p�^�[���F������ = 2(NG SKIP)�Ƃ���
                            Else
                                gStCutPosCorrData.corrResult(Ix) = 0                        ' �p�^�[���F������ = 0(�␳�Ȃ�)�Ƃ���
                            End If
                            '----- ###248�� -----
                        End If
                    End If
                Next
            End If

            '----------------------------------------------------------------------------
            '   ��Ĉʒu�␳�ް���INtime���ɑ��M����(�ő�256��R���ꊇ�ő��M)
            '----------------------------------------------------------------------------
            ' TKY�̏ꍇ�͖������ɑ��M����(���M���Ȃ���INtime���Ɏc���Ă���O��̕␳�f�[�^�ŕ␳�����) ###092
            '###246 If (gTkyKnd = KND_TKY) Then
            Call CUTPOSCOR_ALL(registorCnt, gStCutPosCorrData)
            '###246 End If
            'If (bSendCutPosCorData = True) Then                         ' 1�ł��␳�����������s���ꂽ�ꍇ
            '    Call CUTPOSCOR_ALL(registorCnt, gStCutPosCorrData)
            'End If

            ' '' '' '' ��Ĉʒu�␳�ް���INtime���ɑ��M����
            '' '' ''Call CUTPOSCOR(gRegistorCnt, gfCutPosDRX, gfCutPosDRY, giCutPosRSLT)

            '' '' ''ReDim giCutPosRSLT(MaxRegNum)                       ' CUTPOSCOR()��Call����ƂȂ���
            '' '' ''ReDim gfCutPosDRX(MaxRegNum)                        ' �z��̗v�f������R���ɕύX�����
            '' '' ''ReDim gfCutPosDRY(MaxRegNum)                        ' �̂ōĒ�`����(���e�͏����������)
            Return (cFRS_NORMAL)                                        ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BasTrimming.DoCutPosCorrect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "�J�b�g�ʒu�␳�����{����"
    '''=========================================================================
    ''' <summary>�J�b�g�ʒu�␳�̏����Œ���N���A����</summary>
    ''' <param name="mode">       (INP)���ݸ�Ӱ��(�޼�SW��1��)</param>
    ''' <param name="registorCnt">(INP)��R��</param> 
    ''' <param name="pstCutPos">  (INP)�p�^�[���o�^���</param>
    ''' <returns>�f�B���C ON�FTrue, OFF�FFalse</returns>
    '''=========================================================================
    Private Function DoCutPosCorrectClr(ByVal registorCnt As Integer) As Integer

        Dim rn As Integer
        Dim strMSG As String

        Try

            For rn = 1 To registorCnt                                               ' �S��R�����ݒ肷�� 
                gStCutPosCorrData.corrResult(rn - 1) = 0                            ' �p�^�[���F������ = 0(�␳�Ȃ�)�Ƃ���
                gStCutPosCorrData.corrPosX(rn - 1) = 0.0                            ' �Y����X
                gStCutPosCorrData.corrPosY(rn - 1) = 0.0                            ' �Y����Y
            Next

            '----------------------------------------------------------------------------
            '   ��Ĉʒu�␳�ް���INtime���ɑ��M����(�ő�256��R���ꊇ�ő��M)
            '----------------------------------------------------------------------------
            ' TKY�̏ꍇ�͖������ɑ��M����(���M���Ȃ���INtime���Ɏc���Ă���O��̕␳�f�[�^�ŕ␳�����) ###092
            Call CUTPOSCOR_ALL(registorCnt, gStCutPosCorrData)

            Return (cFRS_NORMAL)                                        ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BasTrimming.DoCutPosCorrect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try

    End Function
#End Region


#Region "���u���b�N��XY�e�[�u�����ړ�����yTKY���z(���g�p)"
    '''=========================================================================
    '''<summary>���u���b�N��XY�e�[�u�����ړ�����</summary>
    '''<param name="stgx">         (INP)</param>
    '''<param name="stgy">          (INP)</param>
    '''<param name="blockx">(OUT)���O�f�[�^</param> 
    '''<param name="blocky">(OUT)���O�f�[�^</param> 
    '''<param name="XY">(OUT)���O�f�[�^</param> 
    '''<param name="pxy">(OUT)���O�f�[�^</param>  
    '''<param name="dblADDSZX">(OUT)���O�f�[�^</param> 
    '''<param name="dblADDSZY">(OUT)���O�f�[�^</param>       
    '''=========================================================================
    Private Function MoveNextBlockTkyMode(ByVal stgx As Double, ByVal stgy As Double,
                                          ByVal blockx As Double, ByVal blocky As Double,
                                          ByVal XY() As Short, ByVal pxy() As Short,
                                          ByVal dblADDSZX As Double, ByVal dblADDSZY As Double) As Integer

        Dim x As Double
        Dim y As Double
        Dim pspacex As Double
        Dim pspacey As Double
        Dim r As Integer

        MoveNextBlockTkyMode = cFRS_NORMAL              ' Return�l = ����

        pspacex = typPlateInfo.dblPlateItvXDir          ' �v���[�g�X�y�[�X�iX) mm
        pspacey = typPlateInfo.dblPlateItvYDir          ' �v���[�g�X�y�[�X�iY) mm

        ' ���̃u���b�N��XY�ړ�
        Select Case gSysPrm.stDEV.giBpDirXy
            Case 0 ' x��, y��
                x = stgx + blockx * XY(1) + pspacex * pxy(1) + dblADDSZX

                y = stgy + blocky * XY(2) + pspacey * pxy(2) + dblADDSZY

            Case 1 ' x��, y��
                x = stgx - (blockx * XY(1) + pspacex * pxy(1)) - dblADDSZX
                y = stgy + blocky * XY(2) + pspacey * pxy(2) + dblADDSZY

            Case 2 ' x��, y��
                x = stgx + blockx * XY(1) + pspacex * pxy(1) + dblADDSZX
                y = stgy - (blocky * XY(2) + pspacey * pxy(2)) - dblADDSZY

            Case 3 ' x��, y��
                x = stgx - (blockx * XY(1) + pspacex * pxy(1)) - dblADDSZX
                y = stgy - (blocky * XY(2) + pspacey * pxy(2)) - dblADDSZY
        End Select

        ' Z�ړ� ��~�m�F
        Call ZSTOPSTS()

        ' XY�e�[�u���ړ�(��Βl�w��)
        r = Form1.System1.XYtableMove(gSysPrm, x, y)
        MoveNextBlockTkyMode = r                        ' Retuen�l�ݒ�  

    End Function

#End Region

#Region "����ۯ��ւ̈ړ��ʒu�擾�p�̍\���̂��ް���ݒ肷��yCHIP/NET���z(���g�p)"
    '''=========================================================================
    '''<summary>����ۯ��ւ̈ړ��ʒu�擾�p�̍\���̂��ް���ݒ肷��</summary>
    '''<param name="tSetNextXY"> (OUT)����ۯ��ւ̈ړ��ʒu�擾�p�̍\����</param>
    '''<param name="blockx">(OUT)��ۯ�����X</param> 
    '''<param name="blocky">(OUT)��ۯ�����Y</param>
    '''<param name="stgx">(OUT)��ۯ�����X</param>   
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetBlockDataToStruct(ByRef tSetNextXY As TRIM_GETNEXTXY, ByRef blockx As Double, ByRef blocky As Double,
                                    ByRef stgx As Double, ByRef stgy As Double, ByRef pxy() As Short, ByRef XY() As Short,
                                    ByRef dblADDSZX As Double, ByRef dblADDSZY As Double, ByRef ptxy() As Short,
                                    ByRef gfStrp As Double, ByRef s As Short)

        Dim iCDir As Short
        Dim iStepRepeat As Short
        Dim bxy(2) As Short
        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim gspacex As Double
        Dim gspacey As Double
        Dim dblStepOffX As Double
        Dim dblStepOffY As Double
        Dim blockIntervalx As Double
        Dim blockIntervaly As Double
        Dim gbxy(2) As Short
        Dim ptspacex As Double
        Dim ptspacey As Double

        '-----------------------------------------------------------------------
        '   �f�[�^�̎擾
        '-----------------------------------------------------------------------
        ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
        iCDir = typPlateInfo.intResistDir
        ' �ï��&��߰Ă̎擾(0:�Ȃ�,1:X,2:Y)
        iStepRepeat = typPlateInfo.intDirStepRepeat
        ' ��ۯ����ނ̎擾
        Call CalcBlockSize(blockx, blocky)
        ' ���߻���X,Y
        dblCSx = typPlateInfo.dblChipSizeXDir
        dblCSy = typPlateInfo.dblChipSizeYDir
        ' ��ٰ�߲������X,Y
        gspacex = typPlateInfo.dblBpGrpItv      'BP�O���[�v�Ԋu
        gspacey = typPlateInfo.dblStgGrpItvY     'Stage�O���[�v�Ԋu
        'gspacex = typPlateInfo.dblGroupItvXDir
        'gspacey = typPlateInfo.dblGroupItvYDir

        ' �ï�ߵ̾��X,Y
        dblStepOffX = typPlateInfo.dblStepOffsetXDir
        dblStepOffY = typPlateInfo.dblStepOffsetYDir
        ' ��ۯ��ԊuX,Y
        blockIntervalx = typPlateInfo.dblBlockItvXDir
        blockIntervaly = typPlateInfo.dblBlockItvYDir
        ' �O���[�v��X,Y
        gbxy(1) = typPlateInfo.intGroupCntInBlockXBp
        gbxy(2) = typPlateInfo.intGroupCntInBlockYStage
        ' ��ۯ����̎擾
        bxy(1) = typPlateInfo.intBlockCntXDir
        bxy(2) = typPlateInfo.intBlockCntYDir
        ' ��ڰĊԊuX,Y(NET�̂�)
        ptspacex = typPlateInfo.dblPlateItvXDir
        ptspacey = typPlateInfo.dblPlateItvYDir

        '-----------------------------------------------------------------------
        '   �f�[�^���u����ۯ��ւ̈ړ��ʒu�擾�p�̍\���́v�ɐݒ肷��
        '-----------------------------------------------------------------------
        With tSetNextXY                                 ' ����ۯ��ւ̈ړ��ʒu�擾�p�̍\����
            .intCDir = iCDir
            .intStepR = iStepRepeat
            .dblblockx = blockx
            .dblblocky = blocky
            .dblCSx = dblCSx
            .dblCSy = dblCSy
            .dblgspacex = gspacex
            .dblgspacey = gspacey
            .dblStepOffX = dblStepOffX
            .dblStepOffY = dblStepOffY
            .dblstgx = stgx
            .dblstgy = stgy
            .intpxy1 = pxy(1)
            .intpxy2 = pxy(2)
            .intxy1 = XY(1)
            .intxy2 = XY(2)
            .dblblockIntervalx = blockIntervalx
            .dblblockIntervaly = blockIntervaly
            .intBlockCntXDir = bxy(1)
            .intBlockCntYDir = bxy(2)

            '----- NET�̂� -----
            .dblStrp = gfStrp
            .dblADDSZX = dblADDSZX
            .dblADDSZY = dblADDSZY
            .intptxy1 = ptxy(1)
            .intptxy2 = ptxy(1)
            .dblptspacex = ptspacex
            .dblptspacey = ptspacey

            '----- CHIP�̂� -----
            If gbxy(s) <> 1 Then
                If s = 1 Then
                    .intArrayCntX = XY(1) + (bxy(s) * pxy(1))
                    .intArrayCntY = XY(2)
                Else
                    .intArrayCntX = XY(1)
                    .intArrayCntY = XY(2) + (bxy(s) * pxy(2))
                End If
            Else
                .intArrayCntX = XY(1)
                .intArrayCntY = XY(2)
            End If
            .dblStrp = gfStrp
        End With

    End Sub

#End Region

#Region "���u���b�N��XY�e�[�u�����ړ�����yCHIP/NET���z(���g�p?)"
    '''=========================================================================
    '''<summary>����ۯ��ւ̈ړ�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Function MoveNextBlockChipNetMode(ByRef blockx As Double, ByRef blocky As Double,
                                             ByRef stgx As Double, ByRef stgy As Double, ByRef pxy() As Short, ByRef XY() As Short,
                                             ByRef dblADDSZX As Double, ByRef dblADDSZY As Double,
                                             ByRef ptxy() As Short, ByRef gfStrp As Double, ByRef s As Short) As Short

        Dim tSetNextXY As TRIM_GETNEXTXY                    ' ����ۯ��ւ̈ړ��ʒu�擾�p�̍\����
        Dim x As Double
        Dim y As Double

        MoveNextBlockChipNetMode = cFRS_NORMAL              ' Return�l = ���� 

        ' ����ۯ��ւ̈ړ��ʒu�擾�p�\���̂�ݒ肷��
        Call SetBlockDataToStruct(tSetNextXY, blockx, blocky, stgx, stgy, pxy, XY, dblADDSZX, dblADDSZY, ptxy, gfStrp, s)
        ' 
        Call GetTrimmingNextBlockXYdata(tSetNextXY, x, y)

        ' Z�ړ� ��~�m�F
        Call ZSTOPSTS()

        ' XY�ړ�(��Έʒu�w��)
        If Form1.System1.XYtableMove(gSysPrm, x, y) Then
            MoveNextBlockChipNetMode = -1
        End If

    End Function
#End Region

#Region "������ۯ��ʒuX,Y�̒l�Z�o(���ݸޏ���)�yCHIP���z(���g�p?)"
    '''=========================================================================
    '''<summary>������ۯ��ʒuX,Y�̒l�Z�o(���ݸޏ���)</summary>
    '''<remarks>��ݸޏ�������XY�̎Z�o�������s�Ȃ�</remarks>
    '''=========================================================================
    Private Sub GetTrimmingNextBlockXYdata(ByRef tSetNextXY As TRIM_GETNEXTXY, ByRef x As Double, ByRef y As Double)

        On Error GoTo ErrExit

        Dim dblInterval As Double
        Dim intForCnt As Short

        With tSetNextXY
            ' ���̃u���b�N��XY�ړ�
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then ' 1��R���炵
                            x = .dblstgx + ((.dblCSx / .intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy + dblInterval
                    Else ' Y����
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx + dblInterval

                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then ' 1��R���炵
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If
                    End If
                Case 1 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then ' 1��R���炵
                            x = .dblstgx - ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy + dblInterval
                    Else 'Y����
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx - dblInterval

                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then ' 1��R���炵
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If
                    End If
                Case 2 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then ' 1��R���炵
                            x = .dblstgx + ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy - dblInterval
                    Else 'Y����
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx + dblInterval

                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then ' 1��R���炵
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If
                    End If
                Case 3 ' x��, y��
                    ' ���ߕ������������s���B
                    If (0 = .intCDir) Then                  ' X����
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then             ' 1��R���炵
                            x = .dblstgx - ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy - dblInterval
                    Else 'Y����
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx - dblInterval

                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then             ' 1��R���炵
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If
                    End If
            End Select
        End With

        Exit Sub
ErrExit:

        MsgBox("GetTrimmingNextBlockXYdata" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "������ۯ��ʒuX,Y�̒l�Z�o(���ݸޏ���)�yNET���z(���g�p?)"
    '''=========================================================================
    '''<summary>������ۯ��ʒuX,Y�̒l�Z�o(���ݸޏ���)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetTrimmingNextBlockXYdataNET(ByVal tSetNextXY As TRIM_GETNEXTXY, ByVal x As Double, ByVal y As Double)

        On Error GoTo ErrExit

        With tSetNextXY

            ' ���̃u���b�N��XY�ړ�
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then         ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        End If
                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
                    Else  'Y����
                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblStrp + .dblADDSZX
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        End If
                    End If
                Case 1 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then         ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        End If
                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
                    Else  'Y����
                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        End If
                    End If
                Case 2 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then         ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        End If
                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
                    Else  'Y����
                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblADDSZX
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        End If
                    End If
                Case 3 ' x��, y��
                    ''���ߕ������������s���B
                    If (0 = .intCDir) Then         ' X����
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        End If
                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
                    Else  'Y����
                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        End If
                    End If
            End Select
        End With

        Exit Sub

ErrExit:
        Err.Clear()
    End Sub
#End Region

#Region "�����_�ϊ�"
    '''=========================================================================
    '''<summary>�����_�ϊ�</summary>
    '''<param name="s"> (INP)�ϊ��Ώە�����</param>
    '''<param name="ch">(INP)�����_�u����������</param> 
    '''<returns>�ϊ��ςݕ�����</returns>
    '''=========================================================================
    Public Function CnvFloatPointChar(ByRef s As String, ByRef ch As String) As String
        Dim i As Short
        Dim ln As Short

        ln = Len(s)
        For i = 1 To ln
            If Mid(s, i, 1) = "." Then
                Mid(s, i, 1) = ch
            End If
        Next
        CnvFloatPointChar = s
    End Function
#End Region

#Region "DispGazou.exe����"
#If False Then                          'V6.0.0.0�D
    '=========================================================================
    '   �摜�\���v���O�����̋N������
    '=========================================================================
    'V3.0.0.0�D��
    Private ipcChnl As New System.Runtime.Remoting.Channels.Ipc.IpcClientChannel
    Private IpcObj As Object = Activator.GetObject(GetType(IPCServiceClass), "ipc://TRIM_DISP_GAZOU_IPC_PORT_NM/" + GetType(IPCServiceClass).Name)
    Private refObj As IPCServiceClass = CType(IpcObj, IPCServiceClass)

#Region "�摜�\���v���O�������N������"
    '''=========================================================================
    ''' <summary>�摜�\���v���O�������N������</summary>
    ''' <param name="ObjProc"> (OUT)Process��޼ު��</param>
    ''' <param name="strFName">(INP)�N���v���O������</param>
    ''' <param name="Camera">  (INP)�J�����ԍ�(0-3)</param> 
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function Execute_GazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer) As Integer

        Dim strARG As String                                        ' ����() 

        Dim dispXPos As Integer
        Dim dispYPos As Integer
        Dim Cnt As Integer = 0

        Try
            If gKeiTyp = KEY_TYPE_RS Then
                TrimClassCommon.ForceEndProcess(DISPGAZOU_SMALL_PATH)       ' �v���Z�X�������I������B
            Else
                TrimClassCommon.ForceEndProcess(DISPGAZOU_PATH)       ' �v���Z�X�������I������B
            End If
            ' �\���ʒu�ݒ�
            dispXPos = FORM_X + Form1.VideoLibrary1.Location.X
            dispYPos = FORM_Y + Form1.VideoLibrary1.Location.Y

            ' �����ײ݈����ݒ�
            strARG = Camera.ToString("0") + " "                     ' args[0] :�J�����ԍ�(0-3)
            'strARG = "0 "                                           ' args[0] :�J�����ԍ�(0-3)"
            strARG = strARG + "1 "                                  ' args[1] :(0=�{�^���\������, 1=�{�^���\�����Ȃ�)
            strARG = strARG + dispXPos.ToString("0") + " "          ' args[2] :�t�H�[���̕\���ʒuX
            strARG = strARG + dispYPos.ToString("0")                ' args[3] :�t�H�[���̕\���ʒuY
            strARG = strARG + " 1"                                  ' args[4] :(0=���b�Z�[�W���䖳��, 1=���b�Z�[�W����L��)
            'V5.0.0.8�C strARG = strARG + " 1"                                  ' args[5] :(0=�V���v���g���}�p�T�C�Y�����, 1=�ʏ��ʃT�C�Y) 'V5.0.0.6�O

            ' �v���Z�X�̋N��
            ObjProc = New Process                                   ' Process��޼ު�Ă𐶐����� 
            ObjProc.StartInfo.FileName = strFName                   ' �v���Z�X�� 
            ObjProc.StartInfo.Arguments = strARG                    ' �����ײ݈����ݒ�
            ObjProc.StartInfo.WorkingDirectory = strWrk             ' ��ƃt�H���_
            ObjProc.Start()                                         ' �v���Z�X�N��

            ' �`���l����o�^
            'ChannelServices.RegisterChannel(ipcChnl, False)
IPC_RETRY_START:  ' �T�[�o�iDispGazou)�����~���Ē����ɋN�������ゾ�ƃ|�[�g�ɏ������߂Ȃ��G���[�ɂȂ�B�Ώ����@������Ȃ��̂ōĎ��s����B
            Try
                'refObj.CallServer("STOP")
                Sleep(2000)
                SendMsgToDispGazou(ObjProc, 2)       'STOP 'V4.0.0.0-87
            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou�����I��
                        If gKeiTyp = KEY_TYPE_RS Then
                            Execute_GazouProc(ObjProc, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    '�ċN��
                        Else
                            Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '�ċN��
                        End If
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.Execute_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try


            ' �g���b�v�G���[������ 
        Catch ex As Exception
            MsgBox("basTrimming.Execute_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�摜�\���v���O�����������I������"
    '''=========================================================================
    '''<summary>�摜�\���v���O�����������I������</summary>
    '''<param name="ObjProc"> (OUT)Process��޼ު��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function FinalEnd_GazouProc(ByRef ObjProc As Process) As Integer
        Dim ExecName As String

        Try
            ' �摜�\���v���O�������N���Ȃ�NOP
            'If Not ObjProc Is Nothing Then
            '    ' �v���Z�X�I�����b�Z�[�W�𑗐M����(����޳�̂�����؂̏ꍇ) 
            '    If (ObjProc.CloseMainWindow() = False) Then             ' Ҳݳ���޳�ɸ۰��ү���ނ𑗐M����
            '        ObjProc.Kill()                                      ' �I�����Ȃ������ꍇ�͋����I������
            '    End If

            '    ' �v���Z�X�̏I����҂�
            '    Do
            '        Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
            '    Loop While (ObjProc.HasExited <> True)                  ' �v���Z�X���I�����Ă���ꍇ�̂�True���Ԃ�
            'End If

            ''refObj.CallServer("END")

            ''ChannelServices.UnregisterChannel(ipcChnl)

            '' �㏈�� 
            'ObjProc.Dispose()                                       ' ���\�[�X�J�� 
            'ObjProc = Nothing

            '            TrimClassCommon.ForceEndProcess(DISPGAZOU_PATH)       ' �_�������Ńv���Z�X�������I������B
            TrimClassCommon.ForceEndProcess(DISPGAZOU_SMALL_PATH)       ' �_�������Ńv���Z�X�������I������B

            ' �摜�\���v���Z�X�������I������(��L�Ńv���Z�X���I�����Ȃ��ꍇ�����邽��) 
            'V4.1.0.0�B
            If gKeiTyp = KEY_TYPE_RS Then
                ExecName = "DispGazouSmall"
            Else
                ExecName = "DispGazou"
            End If
            '            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("DispGazou")
            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName(ExecName)
            'V4.1.0.0�B
            For Each p As System.Diagnostics.Process In ps
                p.Kill()
            Next


            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            '            MsgBox("basTrimming.FinalEnd_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    'V3.0.0.0�D��
#Region "�摜�\���v���O�������N������"
    '''=========================================================================
    ''' <summary>�摜�\���v���O�������N������</summary>
    ''' <param name="ObjProc"> (OUT)Process��޼ު��</param>
    ''' <param name="strFName">(INP)�N���v���O������</param>
    ''' <param name="Camera">  (INP)�J�����ԍ�(0-3)</param> 
    ''' <param name="nCommand">(INP)�v���Z�X�ɑ��M����R�}���h(0:START,1:START_SMALL)</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    ''' <remarks>V3.1.0.0�A 2014/12/01 �摜�\���v���O�����̃��W���[���𓝍�</remarks>
    '''=========================================================================
    Public Function Exec_GazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer, ByVal nCommand As Integer) As Integer

        Dim Cnt As Integer = 0
        ' Dim result As Integer

        Try
            ' VideoOcx�\�����~
            Call Form1.VideoLibrary1.VideoStop()

IPC_RETRY_START:  ' �T�[�o�iDispGazou)�����~���Ē����ɋN�������ゾ�ƃ|�[�g�ɏ������߂Ȃ��G���[�ɂȂ�B�Ώ����@������Ȃ��̂ōĎ��s����B
            Try
                ' ������ V3.1.0.0�A 2014/12/01
                If nCommand = 0 Then
                    'V4.0.0.0-87refObj.CallServer("START")'V4.0.0.0-87
                    'V4.4.0.0�B�@��
                    'SendMsgToDispGazou(1)                               ' START 'V4.0.0.0-87
                    '----- V4.3.0.0�C�� -----
                    If (gKeiTyp <> KEY_TYPE_RS) Then                    ' SL43xR ? 
                        SendMsgToDispGazou(ObjProc, 5)                           ' START_NORMAL
                    Else
                        SendMsgToDispGazou(ObjProc, 1)                               ' START 'V4.0.0.0-87
                    End If
                    'V4.4.0.0�B�@��
                    '----- V4.3.0.0�C�� -----
                Else
                    'V4.0.0.0-87refObj.CallServer("START_SMALL")'V4.0.0.0-87
                    SendMsgToDispGazou(ObjProc, 4)       'START_SMALL'V4.0.0.0-87
                End If
                ' ������ V3.1.0.0�A 2014/12/01
            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou�����I��
                        System.Threading.Thread.Sleep(100)
                        ' ������ V3.1.0.0�A 2014/12/01
                        'Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '�ċN��
                        Execute_GazouProc(ObjProc, strFName, strWrk, Camera)    '�ċN��
                        ' ������ V3.1.0.0�A 2014/12/01
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
    'Public Function Exec_GazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer) As Integer

    '    Dim strARG As String                                        ' ����() 
    '    Dim strMSG As String
    '    Dim dispXPos As Integer
    '    Dim dispYPos As Integer
    '    Try
    '        ' �\���ʒu�ݒ�
    '        dispXPos = FORM_X + Form1.VideoLibrary1.Location.X
    '        dispYPos = FORM_Y + Form1.VideoLibrary1.Location.Y

    '        ' �����ײ݈����ݒ�
    '        strARG = Camera.ToString("0") + " "                     ' args[0] :�J�����ԍ�(0-3)"
    '        'strARG = "0 "                                           ' args[0] :�J�����ԍ�(0-3)"
    '        strARG = strARG + "1 "                                  ' args[1] :(0=�{�^���\������, 1=�{�^���\�����Ȃ�)
    '        strARG = strARG + dispXPos.ToString("0") + " "          ' args[2] :�t�H�[���̕\���ʒuX
    '        strARG = strARG + dispYPos.ToString("0")                ' args[3] :�t�H�[���̕\���ʒuY

    '        ' VideoOcx�\�����~
    '        Call Form1.VideoLibrary1.VideoStop()

    '        ' �v���Z�X�̋N��
    '        ObjProc = New Process                                   ' Process��޼ު�Ă𐶐����� 
    '        ObjProc.StartInfo.FileName = strFName                   ' �v���Z�X�� 
    '        ObjProc.StartInfo.Arguments = strARG                    ' �����ײ݈����ݒ�
    '        ObjProc.StartInfo.WorkingDirectory = strWrk             ' ��ƃt�H���_
    '        ObjProc.Start()                                         ' �v���Z�X�N��

    '        ' �g���b�v�G���[������ 
    '    Catch ex As Exception
    '        strMSG = "basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message
    '        MsgBox(strMSG)
    '        Return (cERR_TRAP)
    '    End Try
    'End Function
#End Region

#End Region

#Region "�摜�\���v���O�����������I������"
    '''=========================================================================
    '''<summary>�摜�\���v���O�����������I������</summary>
    '''<param name="ObjProc"> (OUT)Process��޼ު��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function End_GazouProc(ByRef ObjProc As Process) As Integer

        Dim Cnt As Integer = 0
        '        Dim result As Integer 

        Try
            ' �摜�\���v���O�������N���Ȃ�NOP
            'If (ObjProc Is Nothing) Then
            '    Return (cFRS_NORMAL)
            'End If

IPC_RETRY_START:  ' �T�[�o�iDispGazou)�����~���Ē����ɋN�������ゾ�ƃ|�[�g�ɏ������߂Ȃ��G���[�ɂȂ�B�Ώ����@������Ȃ��̂ōĎ��s����B
            Try

                'V4.0.0.0-87refObj.CallServer("STOP")
                SendMsgToDispGazou(ObjProc, 2)       'STOP 
                '                result = SendMessage(hWnd, WM_USER, 0, 2)
                'V4.0.0.0-87

            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou�����I��
                        Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '�ċN��
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try

            Call Form1.VideoLibrary1.VideoStart()

            ' ��ʂ��X�V
            Call Form1.Refresh()

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
    'Public Function End_GazouProc(ByRef ObjProc As Process) As Integer

    '    Dim strMSG As String

    '    Try
    '        ' �摜�\���v���O�������N���Ȃ�NOP
    '        If (ObjProc Is Nothing) Then
    '            Return (cFRS_NORMAL)
    '        End If

    '        ' �v���Z�X�I�����b�Z�[�W�𑗐M����(����޳�̂�����؂̏ꍇ) 
    '        If (ObjProc.CloseMainWindow() = False) Then             ' Ҳݳ���޳�ɸ۰��ү���ނ𑗐M����
    '            ObjProc.Kill()                                      ' �I�����Ȃ������ꍇ�͋����I������
    '        End If

    '        ' �v���Z�X�̏I����҂�
    '        Do
    '            Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
    '        Loop While (ObjProc.HasExited <> True)                  ' �v���Z�X���I�����Ă���ꍇ�̂�True���Ԃ�

    '        ' VideoOcx�\���̍ĊJ
    '        Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
    '        Call Form1.VideoLibrary1.VideoStart()

    '        ' ��ʂ��X�V
    '        Call Form1.Refresh()

    '        ' �㏈�� 
    '        ObjProc.Dispose()                                       ' ���\�[�X�J�� 
    '        ObjProc = Nothing
    '        Return (cFRS_NORMAL)

    '        ' �g���b�v�G���[������ 
    '    Catch ex As Exception
    '        strMSG = "basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message
    '        MsgBox(strMSG)
    '        Return (cERR_TRAP)
    '    End Try
    'End Function
#End If                                 'V6.0.0.0�D
#End Region

#Region "FT���ϒl(Double)�̎Z�o"
    ' ###154
    '''=========================================================================
    '''<summary>FT���ϒl(Double)�̎Z�o</summary> 
    '''<param name="dblNx"> (IN)����̒l��޼ު��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function GetAverageFT(ByVal dblNx As Double, ByVal lngNxMax As Long) As Double

        Dim strMSG As String

        Try

            TotalAverageFT = TotalAverageFT + dblNx
            '' ����ٰ��
            'For lngCnt = 0 To (lngNxMax - 1)
            '    ' �덷���v
            '    dblSub = dblSub + dblNx(lngCnt)
            'Next
            '' ���ϒl
            'GetAverage = dblSub / lngNxMax
            GetAverageFT = TotalAverageFT / lngNxMax
            TotalAverageDebug = GetAverageFT

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.GetAverageFT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "IT���ϒl(Double)�̎Z�o"
    '''=========================================================================
    '''<summary>IT���ϒl(Double)�̎Z�o</summary>###154 
    '''<param name="dblNx"> (IN)����̑��茋��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function GetAverageIT(ByVal dblNx As Double, ByVal lngNxMax As Long) As Double
        Dim strMSG As String

        Try
            TotalAverageIT = TotalAverageIT + dblNx
            '' ����ٰ��
            'For lngCnt = 0 To (lngNxMax - 1)
            '    ' �덷���v
            '    dblSub = dblSub + dblNx(lngCnt)
            'Next
            '' ���ϒl
            'GetAverage = dblSub / lngNxMax
            GetAverageIT = TotalAverageIT / lngNxMax
            TotalAverageDebug = GetAverageIT

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.TotalAverageIT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "FT�W���΍�(Double)�̎Z�o"
    '''=========================================================================
    ''' <summary>FT�W���΍�(Double)�̎Z�o</summary>###154 
    ''' <param name="dblNx"> (INP)�l(Double�̔z��)  (�Y����0ORG)</param>
    ''' <param name="lngNxMax">(INP) : ��</param>
    ''' <param name="dblAverage">(INP) : ���ϒl</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function GetDeviationFT(ByVal dblNx As Double, ByVal lngNxMax As Long, ByVal dblAverage As Double) As Double

        Dim strMSG As String

        Try
            TotalSum2FT = TotalSum2FT + dblNx * dblNx
            TotalDeviationFT = Math.Sqrt((TotalSum2FT / lngNxMax) - (dblAverage * dblAverage))
            GetDeviationFT = TotalDeviationFT
            '' CHG 
            'TotalDeviationFT = TotalDeviationFT + (dblNx - dblAverage) * (dblNx - dblAverage)
            ' '' �����΍�
            'GetDeviationFT = Math.Sqrt(TotalDeviationFT / lngNxMax)
            'TotalDeviationDebug = TotalDeviationFT
            '' ORG
            '' ����ٰ��
            'For lngCnt = 0 To (lngNxMax - 1)

            '    ' ���ό덷���������l��2��
            '    dblXi = dblXi + ((dblNx(lngCnt) - dblAverage) * (dblNx(lngCnt) - dblAverage))

            'Next

            '' �����΍�
            'GetDeviation = Sqr(dblXi / lngNxMax)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.GetDeviationFT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "FT�W���΍�(Double)�̎Z�oORG "
    '===============================================================================
    '�y�@�@�\�z�W���΍�(Double)�̎Z�o
    '�y���@���zdblNx()    (INP) : �l(Double�̔z��)  (�Y����0ORG)
    '          lngNxMax   (INP) : ��
    '          dblAverage (INP) : ���ϒl
    '�y�߂�l�z�W���΍�
    '===============================================================================
    '''=========================================================================
    '''<summary>FT�W���΍�(Double)�̎Z�o</summary>###154 
    '''<param name="dblNx"> (IN)����̑��茋�ʵ�޼ު��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function GetDeviationFTOrg(ByVal dblNx() As Double, ByVal lngNxMax As Long, ByVal dblAverage As Double) As Double

        Dim strMSG As String
        Dim lngCnt As Long
        Dim dblXi As Double

        Try
            'TotalDeviationFT = TotalDeviationFT + (dblNx - dblAverage) * (dblNx - dblAverage)
            ' '' �����΍�
            'GetDeviationFTOrg = Math.Sqrt(TotalDeviationFT / lngNxMax)
            'TotalDeviationDebug = GetDeviationFTOrg

            ' ����ٰ��
            For lngCnt = 0 To (lngNxMax - 1)

                ' ���ό덷���������l��2��
                dblXi = dblXi + ((dblNx(lngCnt) - dblAverage) * (dblNx(lngCnt) - dblAverage))

            Next

            ' �����΍�
            GetDeviationFTOrg = Math.Sqrt(dblXi / lngNxMax)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.GetDeviationFT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "IT�W���΍�(Double)�̎Z�o"
    '===============================================================================
    '�y�@�@�\�z�W���΍�(Double)�̎Z�o
    '�y���@���zdblNx()    (INP) : �l(Double�̔z��)  (�Y����0ORG)
    '          lngNxMax   (INP) : ��
    '          dblAverage (INP) : ���ϒl
    '�y�߂�l�z�W���΍�
    '===============================================================================
    '''=========================================================================
    '''<summary>IT�W���΍�(Double)�̎Z�o</summary>###154 
    '''<param name="dblNx"> (IN)����̑��茋�ʵ�޼ު��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function GetDeviationIT(ByVal dblNx As Double, ByVal lngNxMax As Long, ByVal dblAverage As Double) As Double

        Dim strMSG As String

        Try
            TotalSum2IT = TotalSum2IT + dblNx * dblNx
            TotalDeviationIT = Math.Sqrt((TotalSum2IT / lngNxMax) - (dblAverage * dblAverage))
            GetDeviationIT = TotalDeviationIT
            'TotalDeviationIT = TotalDeviationFT + (dblNx - dblAverage) * (dblNx - dblAverage)
            ' '' �����΍�
            'GetDeviationIT = Math.Sqrt(TotalDeviationFT / lngNxMax)
            '            TotalDeviationDebug = GetDeviationIT
            '' ����ٰ��
            'For lngCnt = 0 To (lngNxMax - 1)

            '    ' ���ό덷���������l��2��
            '    dblXi = dblXi + ((dblNx(lngCnt) - dblAverage) * (dblNx(lngCnt) - dblAverage))

            'Next

            '' �����΍�
            'GetDeviation = Sqr(dblXi / lngNxMax)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.GetDeviationIT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "�v�Z�l�̃N���A"
    '''=========================================================================
    '''<summary>�v�Z�l�̃N���A</summary>###154 
    '''=========================================================================
    Public Sub ClearAvgDevCount()

        Try
            TotalDeviationIT = 0
            TotalAverageIT = 0
            TotalDeviationFT = 0
            TotalAverageFT = 0
            TotalSum2FT = 0
            TotalSum2IT = 0
            '----- V6.0.3.0_26�� -----
            TotalFTValue = 0
            TotalAverageFTValue = 0
            TotalCntTrimming = 0
            '----- V6.0.3.0_26�� -----

        Catch ex As Exception
        End Try
        Exit Sub

    End Sub
#End Region

#Region "�A��HI-NG�����ޯ̧������"
    '''=========================================================================
    '''<summary>�A��HI-NG�����ޯ̧������</summary> ###181 2013.01.25 
    '''=========================================================================
    Public Sub ClearNgHiCount()
        Dim r As Integer

        ' �A��HI-NG�����ޯ̧������ ###181
        For r = 0 To gRegistorCnt
            iNgHiCount(r) = 0
        Next r

    End Sub

#End Region
    '----- V1.13.0.0�J�� -----
#Region "����΂�����o/�I�[�o���[�h���o�`�F�b�N"
    '''=========================================================================
    ''' <summary>����΂�����o/�I�[�o���[�h���o�`�F�b�N</summary>
    ''' <param name="RtnCode">(INP)TrimBlockExe()�̖߂�l</param>
    ''' <returns>cFRS_ERR_START = ����΂�����o/�I�[�o���[�h���o�ŏ������s�w��
    '''          cFRS_ERR_RST   = ����΂�����o/�I�[�o���[�h���o�ŏ����Ő؎w��
    '''          ��L�ȊO       = �G���[
    ''' </returns>
    '''=========================================================================
    Private Function CV_OverLoadErrorCheck(ByVal RtnCode As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ����΂�����o/�I�[�o���[�h���o
            If (RtnCode = ERR_MEAS_CV) Then                             ' ����΂�����o ? 
                strMSG = MSG_SPRASH40                                   ' "����̂΂�������o���܂���"  
            ElseIf (RtnCode = ERR_MEAS_OVERLOAD) Then                   ' �I�[�o���[�h���o ?
                strMSG = MSG_SPRASH41                                   ' "���莞�I�[�o���[�h�����o���܂���"  
            ElseIf (RtnCode = ERR_MEAS_REPROBING) Then                  ' �ăv���[�r���O�G���[ ?
                strMSG = MSG_SPRASH42                                   ' "�ăv���[�r���O�G���["
                Call Form1.Z_PRINT(strMSG)                              ' �����O��ʂɂ��\������ 
            Else                                                        ' ��L�ȊO 
                Return (RtnCode)                                        ' Return�l = TrimBlockExe()�̖߂�l��Ԃ�
            End If

            ' ���[�_���������[�h���̓��O�\����Ƀ��b�Z�[�W�\������
            If (giHostMode = cHOSTcMODEcAUTO) Then
                If (RtnCode <> ERR_MEAS_REPROBING) Then
                    Call Form1.Z_PRINT(strMSG)
                End If
                Return (cFRS_ERR_RST)                                   ' Return�l = ����΂�����o/�I�[�o���[�h���o�ŏ����Ő؎w��
            End If

            ' ���b�Z�[�W�\��(START�L�[/RESET�L�[�����҂�)
            Call LAMP_CTRL(LAMP_START, True)                            ' START�����vON
            Call LAMP_CTRL(LAMP_RESET, True)                            ' RESET�����vON
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����

            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                    strMSG, MSG_SPRASH35, "", System.Drawing.Color.Blue, System.Drawing.Color.Black, System.Drawing.Color.Black)

            Call LAMP_CTRL(LAMP_START, False)                           ' START�����vOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET�����vOFF
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "basTrimming.CV_OverLoadErrorCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (RtnCode)                                            ' Return�l = TrimBlockExe()�̖߂�l��Ԃ�
        End Try
    End Function
#End Region
    '----- V1.13.0.0�J�� -----
    '----- V1.13.0.0�J�� -----
#Region "Log�t�@�C���p�t�@�C���̃N���A"
    '''=========================================================================
    ''' <summary>Log�������ݗp�t�@�C���̍폜</summary>
    ''' <param name="tmpLogFileName">(INP)�폜�������t�@�C����</param>
    ''' <returns>OK  
    ''' </returns>
    '''=========================================================================
    Private Function ClearTmpLogFile(ByVal tmpLogFileName As String) As Integer

        System.IO.File.Delete(tmpLogFileName)

    End Function

#End Region
    '----- V1.13.0.0�J�� -----
#Region "Log�t�@�C���p�t�@�C���̍쐬"
    '''=========================================================================
    ''' <summary>Log�������ݗp�t�@�C���̍쐬</summary>
    ''' <param name="tmpLogFileName">(INP)�쐬�������t�@�C����</param>
    ''' <returns>OK  
    ''' </returns>
    '''=========================================================================
    Private Function MakeTmpLogFile(ByVal tmpLogFileName As String, ByRef writestr As String) As Integer
        '#4.12.2.0�C    Private Function MakeTmpLogFile(ByVal tmpLogFileName As String, ByVal writestr As String) As Integer
        Dim strMSG As String
        'Dim RtnCode As Integer

        Try
            'RtnCode = 0
            'Dim sw As New System.IO.StreamWriter(tmpLogFileName, True, System.Text.Encoding.GetEncoding("shift_jis"))
            Using sw As New StreamWriter(tmpLogFileName, True, Encoding.UTF8)   'V4.4.0.0-0
                'RtnCode = 1
                sw.Write(writestr)
                'sw.Close()
            End Using

        Catch ex As Exception
            strMSG = "basTrimming.MakeTmpLogFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function

#End Region

    '=========================================================================
    '   �摜�\���v���O�����̋N������
    '=========================================================================
#If False Then
#Region "�摜�\���v���O�������N������"
    ' ������ V3.1.0.0�A 2014/12/01 �R�����g
    ' '''=========================================================================
    ' ''' <summary>�摜�\���v���O�������N������</summary>
    ' ''' <param name="ObjProc"> (OUT)Process��޼ު��</param>
    ' ''' <param name="strFName">(INP)�N���v���O������</param>
    ' ''' <param name="Camera">  (INP)�J�����ԍ�(0-3)</param> 
    ' ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    ' '''=========================================================================
    '    Public Function Exec_SmallGazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer) As Integer

    '        Dim Cnt As Integer = 0

    '        Try
    '            ' VideoOcx�\�����~
    '            Call Form1.VideoLibrary1.VideoStop()

    'IPC_RETRY_START:  ' �T�[�o�iDispGazou)�����~���Ē����ɋN�������ゾ�ƃ|�[�g�ɏ������߂Ȃ��G���[�ɂȂ�B�Ώ����@������Ȃ��̂ōĎ��s����B
    '            Try
    '                refObj.CallServer("START_SMALL")
    '            Catch ex As Exception
    '                Cnt = Cnt + 1
    '                If Cnt < 100 Then
    '                    If (Cnt Mod 10) = 0 Then
    '                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou�����I��
    '                        System.Threading.Thread.Sleep(100)
    '                        Execute_GazouProc(ObjProc, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    '�ċN��
    '                    End If
    '                    System.Threading.Thread.Sleep(10)
    '                    GoTo IPC_RETRY_START
    '                Else
    '                    MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
    '                End If
    '            End Try

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
    '            Return (cERR_TRAP)
    '        End Try
    '    End Function
    ' ������ V3.1.0.0�A 2014/12/01 �R�����g
#End Region

#Region "�摜�\���v���O�����������I������"
    '''=========================================================================
    '''<summary>�摜�\���v���O�����������I������</summary>
    '''<param name="ObjProc"> (OUT)Process��޼ު��</param>
    '''<returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Public Function End_SmallGazouProc(ByRef ObjProc As Process) As Integer

        Dim Cnt As Integer = 0
        '    Dim result As Integer 

        Try
            ' �摜�\���v���O�������N���Ȃ�NOP
            'If (ObjProc Is Nothing) Then
            '    Return (cFRS_NORMAL)
            'End If

IPC_RETRY_START:  ' �T�[�o�iDispGazou)�����~���Ē����ɋN�������ゾ�ƃ|�[�g�ɏ������߂Ȃ��G���[�ɂȂ�B�Ώ����@������Ȃ��̂ōĎ��s����B
            Try
                'V4.0.0.0-87refObj.CallServer("STOP")
                'V4.0.0.0-87 refObj.CallServer("STOP")
                SendMsgToDispGazou(ObjProc, 2)       'STOP 

            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou�����I��
                        Execute_GazouProc(ObjProc, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    '�ċN��
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try

            Call Form1.VideoLibrary1.VideoStart()

            ' ��ʂ��X�V
            Call Form1.Refresh()

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try

        'Dim strMSG As String

        'Try
        '    ' �摜�\���v���O�������N���Ȃ�NOP
        '    If (ObjProc Is Nothing) Then
        '        Return (cFRS_NORMAL)
        '    End If

        '    ' �v���Z�X�I�����b�Z�[�W�𑗐M����(����޳�̂�����؂̏ꍇ) 
        '    If (ObjProc.CloseMainWindow() = False) Then             ' Ҳݳ���޳�ɸ۰��ү���ނ𑗐M����
        '        ObjProc.Kill()                                      ' �I�����Ȃ������ꍇ�͋����I������
        '    End If

        '    ' �v���Z�X�̏I����҂�
        '    Do
        '        Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
        '    Loop While (ObjProc.HasExited <> True)                  ' �v���Z�X���I�����Ă���ꍇ�̂�True���Ԃ�

        '    ' VideoOcx�\���̍ĊJ
        '    Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
        '    Call Form1.VideoLibrary1.VideoStart()

        '    ' ��ʂ��X�V
        '    Call Form1.Refresh()

        '    ' �㏈�� 
        '    ObjProc.Dispose()                                       ' ���\�[�X�J�� 
        '    ObjProc = Nothing
        '    Return (cFRS_NORMAL)

        '    ' �g���b�v�G���[������ 
        'Catch ex As Exception
        '    strMSG = "SimpleTrimmer.End_SmallGazouProc() TRAP ERROR = " + ex.Message
        '    MsgBox(strMSG)
        '    Return (cERR_TRAP)
        'End Try
    End Function
#End Region

#Region "Dispgazou��Window���b�Z�[�W�𑗐M����"
    '''=========================================================================
    ''' <summary>
    ''' Dispgazou��Window���b�Z�[�W�𑗐M����
    ''' </summary>
    ''' <param name="ObjProc">'V5.0.0.6�O ADD</param>
    ''' <param name="No">(INP)���b�Z�[�W�ԍ�</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function SendMsgToDispGazou(ByRef ObjProc As Process, ByVal No As Integer) As Integer

        Dim result As Integer = cFRS_NORMAL
        Dim Cnt As Integer = 0
        Dim hWnd As Int32
        Try
SND_MSG_RETRY_START:
            '����̃E�B���h�E�n���h�����擾���܂�
            'Dim hWnd As Int32 = FindWindow(Nothing, "DispGazou(Camera1) V4.0.0.0")'V4.3.0.0�B
            hWnd = FindWindow(Nothing, "DispGazou") 'V4.3.0.0�B
            If hWnd = 0 Then
                '�n���h�����擾�ł��Ȃ�����
                'V5.0.0.6�O ADD START��
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou�����I��
                        Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '�ċN��
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo SND_MSG_RETRY_START
                Else                'V5.0.0.6�O ADD END��
                    MessageBox.Show("����Window�̃n���h�����擾�ł��܂���")
                End If
            End If

            '//'V4.0.0.0-89        result = SendMessage(hWnd, WM_APP, 0, No)
            result = SendNotifyMessage(hWnd, WM_APP, 0, No)

            Return result
        Catch ex As Exception
            MsgBox("basTrimming.SendMsgToDispGazou() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try

    End Function
#End Region

    'V4.3.0.0�@��
#Region "Dispgazou��Window���b�Z�[�W�𑗐M����"
    Private bDasouDispCrossLine As Boolean = False
    Public Sub DasouDispCrossLineOn()
        bDasouDispCrossLine = True
    End Sub
    Public Sub DasouDispCrossLineOff()
        bDasouDispCrossLine = False
    End Sub
    Public Function GetDasouDispCrossLine() As Boolean
        Return (bDasouDispCrossLine)
    End Function
#End Region
#End If
    '''=========================================================================
    ''' <summary>
    ''' Dispgazou��Window���b�Z�[�W�𑗐M���� 
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SendCrossLineMsgToDispGazou()
        'V6.0.0.0�D        Dim result As Integer
        'V6.0.0.0�D        Dim int1 As Int32 = 0
        'V6.0.0.0�D        Dim int2 As Int32 = 0
        Try
            'V6.0.0.0�D            Dim bpx, bpy, xpos, ypos, zpos As Double
            If gSysPrm.stCRL.giDspFlg = 1 Then      ' �����␳�L��
#If False Then
                '����̃E�B���h�E�n���h�����擾���܂�
                Dim hWnd As Int32 = FindWindow(Nothing, "DispGazou")
                LaserFront.Trimmer.DefTrimFnc.GET_STATUS(1, xpos, ypos, zpos, bpx, bpy)
                If hWnd = 0 Then
                    '�n���h�����擾�ł��Ȃ�����
                    MessageBox.Show("SendCrossLineMsgToDispGazou() ����Window�̃n���h�����擾�ł��܂���")
                End If
                bpx = bpx * 1000
                bpy = bpy * 1000
                ' Int�^�Ȃ̂ł������1000�{���āA�󂯑���1000�Ŋ����Ďg�p���� 
                '���l�ɐ������ϊ��o���邩�H
                int1 = CType(bpx, Int32)
                int2 = CType(bpy, Int32)
                result = SendMessage(hWnd, (WM_APP + 2), int1, int2)
#Else
                ObjCrossLine.CrossLineDispXY(0.0#, 0.0#)                'V6.0.0.0�D
#End If
            End If
        Catch ex As Exception
            MsgBox("SendCrossLineMsgToDispGazou() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    'V4.3.0.0�@��

    ''' <summary>'V4.1.0.0�K
    ''' BP�̎��������΂����߂ɁABP���ő�ɓ��삳����
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BPMaintMove() As Integer
        Dim r As Integer
        Dim strMsg As String

        Try
            r = Form1.System1.BPMOVE(gSysPrm, 0, 0, 0, 0, -40, -40, 1)
            If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                Return (r)
            End If
            r = Form1.System1.BPMOVE(gSysPrm, 0, 0, 0, 0, 40, 40, 1)
            If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                Return (r)
            End If
            r = Form1.System1.BPMOVE(gSysPrm, 0, 0, 0, 0, 0, 0, 1)
            If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\���ς�) 
                Return (r)
            End If

            Return r

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "BPMaintMove() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try

    End Function


    ''' <summary>'V4.1.0.0�N
    ''' Form1��ADJ�{�^���̏�Ԑݒ�
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetADJButton() As Integer

        If (gbChkboxHalt = True) Then
            Form1.BtnADJ.Text = "ADJ ON"
            Form1.BtnADJ.BackColor = System.Drawing.Color.Yellow
        Else
            Form1.BtnADJ.Text = "ADJ OFF"
            Form1.BtnADJ.BackColor = System.Drawing.SystemColors.Control
        End If

    End Function

    'V4.9.0.0�@�� 'High,Low�̗��������Ȃ����ꍇ�̒�~��ʗp
    ''' <summary>
    ''' NG���̔�����s����~���锻��
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function sub_JudgeLotStop() As Integer
        Dim r As Integer
        Dim rtnCode As Integer
        Dim strMSG As String
        Dim LaserPower As Double 'V5.0.0.1-32

        sub_JudgeLotStop = cFRS_NORMAL

        r = JudgeLotStop()
        If r <> cFRS_NORMAL Then
            NGJudgeResult = r

            ' 'V4.11.0.0�G��
            r = StageMveExchangePos()
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' ���b�Z�[�W�\��
                sub_JudgeLotStop = r
                Exit Function
            End If

            SetLotStopRequestBit(1)
            r = WaitLotStopReady()
            If r <> cFRS_NORMAL Then
                sub_JudgeLotStop = r
                Exit Function
            End If
            '�N�����vOFF�A�z��OFF
            ClampAndVacuum(0)
            ' 'V4.11.0.0�G��

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            ' �ꎞ��~�J�n���Ԃ�ݒ肷��(�I�v�V����)
            m_blnElapsedTime = True                             ' �o�ߎ��Ԃ�\������
            Call Set_PauseStartTime(StPauseTime)
            '----- V4.11.0.0�C�� -----

            ' NG�����ݒ���������Ȃ����ꍇ�̃G���[���b�Z�[�W�\��(SL432R/SL436R����)
            r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_RATE_NG) ' �L�[�����҂���ʕ\��
            If r = cFRS_ERR_RST Then

LOT_CANCEL:
                sub_JudgeLotStop = cFRS_ERR_RST

                ' ���[�_�[�o��(ON=�g���}����~��,OFF=�Ȃ�)(SL436R��)
                Call SetLoaderIO(LOUT_STOP, &H0)                        ' ���[�_�o��(ON=�g���}����~��, OFF=�Ȃ�)
                'V4.9.0.0�@��
                SetSubExistBit(1)
                ' W113.02��ON����
                SetLotStopBit(1)
                'V4.9.0.0�@��

            ElseIf r = cFRS_ERR_OTHER Then
                '�N���[�j���O�̎��s
            Else
                '�W�v���N���A���邩�̊m�F
                r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_TOTAL_CLEAR) ' �L�[�����҂���ʕ\��
                If r = cFRS_ERR_OTHER Then
                    '�������Ȃ��ő��s
                Else
                    'V5.0.0.1-32��
                    LaserPower = TrimData.GetLaserPower()
                    '�W�v���e�̃N���A
                    ClearTotalCount()
                    TrimData.SetLaserPower(LaserPower)
                    SimpleTrimmer.DspLaserPower()
                    'V5.0.0.1-32��
                End If

PLATE_CHECK_RETRY:
                r = SetSubExistBit(1)
                If r = cFRS_NORMAL Then
                    ' �����
                Else
                    ' ��Ȃ�
                    strMSG = MSG_SPRASH58
                    'r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                    '        "", strMSG, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_BTN_START + cFRS_ERR_RST, True, _
                            "", strMSG, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If r = cFRS_ERR_RST Then
                        GoTo LOT_CANCEL
                    ElseIf r = cFRS_ERR_START Then
                        GoTo PLATE_CHECK_RETRY
                    End If
                End If
            End If
            ' 'V4.11.0.0�G��
            SetLotStopRequestBit(0)
            ' 'V4.11.0.0�G��

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            ' �ꎞ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����(�I�v�V����)
            Call Set_PauseTotalTime(StPauseTime)
            ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��(�I�v�V����)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C�� -----


        End If
    End Function

    '----- V6.0.3.0�F�� -----
#Region "�O��̑��茋�ʂƖڕW�l�̍�������J�b�g�I�t�l���v�Z����"
    '''=========================================================================
    ''' <summary>�O��̑��茋�ʂƖڕW�l�̍�������J�b�g�I�t�l���v�Z����</summary>
    ''' <returns>
    ''' </returns>
    '''=========================================================================
    Public Function CalcCutOffVal() As Integer

        Dim i As Integer
        Dim HighLimitTarget As Double
        Dim LowLimitTarget As Double
        Dim turnning As Boolean = False
        Dim FinalCutOff As Double
        Dim SumFinalTest As Double = 0.0
        Dim FinalTestOKCnt As Long = 0
        Dim AvgFinal As Double = 0.0
        Dim sMsg As String


        ' �ڕW�l�`�ɑ΂��钲���g���������I�t�Z�b�g
        HighLimitTarget = stCutOffAdjust.TargetA * (1 + typResistorInfoArray(i + 1).dblAdjustCutOff_HighLimit / 100)    ' �J�b�g�I�t����HI(%)      ' V6.0.3.0�K
        ' �ڕW�l�`�ɑ΂��钲���k�������I�t�Z�b�g
        LowLimitTarget = stCutOffAdjust.TargetA * (1 + typResistorInfoArray(i + 1).dblAdjustCutOff_LowLimit / 100)      ' �J�b�g�I�t����LO(%)      ' V6.0.3.0�K

        For i = 0 To typPlateInfo.intResistCntInBlock - 1

            If gwTrimResult(i) = TRIM_RESULT_OK Then
                SumFinalTest = SumFinalTest + gfFinalTest(i)    ' ���肪����Ȓ�R�̕��ϒl
                FinalTestOKCnt = FinalTestOKCnt + 1             ' ���肪����Ȓ�R�̌�
            End If

            sMsg = "Judge=," + gwTrimResult(i).ToString("0") + " , ����l," + gfFinalTest(i).ToString("0.0000000")
            DbgLogDsp(sMsg)

        Next

        If FinalTestOKCnt > 0 Then                              ' ����ɑ���ł�����R������Ƃ��̂�
            AvgFinal = TrimData.CalcAverage(SumFinalTest, FinalTestOKCnt)

            If (AvgFinal < LowLimitTarget) OrElse (HighLimitTarget < AvgFinal) Then
                stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC

                '���񒲐��p�J�b�g�I�t�l
                FinalCutOff = TrimData.ConvValToPercent(stCutOffAdjust.TargetA, AvgFinal)
                stCutOffAdjust.dblAdjustCutOff = stCutOffAdjust.dblAdjustCutOff - FinalCutOff

                stCutOffAdjust.dblAdjustCutOff = Math.Round(stCutOffAdjust.dblAdjustCutOff, 2, MidpointRounding.AwayFromZero)

                sMsg = "Cnt=" + gAdjustCutoffCount.ToString("0") + " : ���蕽��=" + AvgFinal.ToString("0.0000000") + "," + "������J�b�g�I�t��" + stCutOffAdjust.dblAdjustCutOff.ToString("0.00")
                Call Form1.Z_PRINT(sMsg)
                DbgLogDsp(sMsg)

                For i = 0 To typPlateInfo.intResistCntInBlock - 1
                    typResistorInfoArray(i + 1).ArrCut(typResistorInfoArray(i + 1).intCutCount).dblCutOff = stCutOffAdjust.dblAdjustCutOff
                Next
                ' INtime�փf�[�^�]��
                SendTrimData()
            Else
                ' �����͈͓��ɓ����Ă���
                stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_ALREADY
                sMsg = "���������FCnt=" + gAdjustCutoffCount.ToString("0") + " : ���蕽�ρ�" + AvgFinal.ToString("0.0000000")
                ' V6.0.3.0�S�� ��ʏ�ɃJ�b�g�I�t�l��\��
                Form1.lblCutOff.Visible = True
                Form1.lblCutOff.Location = New System.Drawing.Point(615, 25)
                Form1.lblCutOff.Text = "CutOFF=" + stCutOffAdjust.dblAdjustCutOff.ToString("0.00")
                ' V6.0.3.0�S��

                Call Form1.Z_PRINT(sMsg)
                DbgLogDsp(sMsg)
            End If
        Else
            sMsg = "���푪��f�[�^�Ȃ�"
            Call Form1.Z_PRINT(sMsg)
        End If

    End Function
#End Region

#Region "�J�b�g�I�t�v�Z�p�̗̈�̏�����"
    '''=========================================================================
    ''' <summary>�J�b�g�I�t�v�Z�p�̗̈�̏�����</summary>
    ''' <remarks>
    ''' </remarks>
    '''=========================================================================
    Public Sub InitCalCutoff()

        Dim sMsg As String


        '�����O�̃J�b�g�I�t�l
        stCutOffAdjust.OrgCutOff = typResistorInfoArray(1).ArrCut(typResistorInfoArray(1).intCutCount).dblCutOff
        ' �ڕW�l�`
        ' stCutOffAdjust.TargetA = typResistorInfoArray(1).dblTrimTargetVal * (1 + (typResistorInfoArray(1).ArrCut(typResistorInfoArray(1).intCutCount).dblCutOff) / 100)
        stCutOffAdjust.TargetA = typResistorInfoArray(1).dblTrimTargetVal

        '�J�b�g�I�t�����l�ݒ�
        stCutOffAdjust.dblAdjustCutOff = stCutOffAdjust.OrgCutOff

        '�������ɐݒ�
        stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC

        sMsg = "�ڕWA=" + stCutOffAdjust.TargetA.ToString("0.0000000") + "," + "���J�b�g�I�t��" + stCutOffAdjust.OrgCutOff.ToString("0.00")
        Call Form1.Z_PRINT(sMsg)
        DbgLogDsp(sMsg)

    End Sub
#End Region

#Region "�f�o�b�O���O�o��"
    '''=========================================================================
    ''' <summary>�f�o�b�O���O�o��</summary>
    ''' <remarks>
    ''' </remarks>
    '''=========================================================================
    Public Sub DbgLogDsp(ByVal DspString As String)

        ' �f�o�b�O���O�o�͂��Ȃ��Ȃ�NOP
        If (0 = giCutOffLogOut) Then Exit Sub

        Dim fileDate As String = (DateTime.Today).ToString("yyyyMMdd") 'yyyyMMdd
        Dim fileName As String =
            "C:\TRIMDATA\LOG\CutOfflog" & fileDate & ".log" ' VideoDbglogyyyyMMdd.log
        Try
            Using fs As New FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
                Using sw As New StreamWriter(fs)
                    ' "yyyy/MM/dd HH:mm:ss:DspString"
                    Dim strDateTime As String = (DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss")
                    sw.WriteLine(strDateTime & " : " & DspString)
                End Using
            End Using

        Catch ex As Exception
            ' DO NOTHING
        End Try
    End Sub
#End Region
    '----- V6.0.3.0�F�� -----

    ' 'V4.11.0.0�G��
    ''' <summary>
    ''' ������ʒu�ֈړ����āA�N�����v�J�A�z��OFF����
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function StageMveExchangePos() As Integer
        Dim Idx As Integer
        Dim r As Integer
        Dim rtnCode As Integer

        StageMveExchangePos = cFRS_NORMAL

        '����o���ʒu�ւ̈ړ�
        Idx = typPlateInfo.intWorkSetByLoader - 1               ' Idx = ��i��ԍ� - 1
        r = SMOVE2(gfBordTableInPosX(Idx), gfBordTableInPosY(Idx))
        If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
            rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' ���b�Z�[�W�\��
        End If

        StageMveExchangePos = r

        Exit Function

    End Function
    'V4.9.0.0�@��

    '========================================================================================
    '   ���ʊ֐�
    '========================================================================================
#Region "�X�e�[�W�ړ�����"
    '''=========================================================================
    ''' <summary>�X�e�[�W�ړ�����</summary>
    ''' <param name="pltNo"></param>
    ''' <param name="blkNo"></param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function MoveTargetStagePos(ByVal pltNo As Integer, ByVal blkNo As Integer) As Integer

        Dim intRet As Integer
        Dim nextStgX As Double
        Dim nextStgY As Double
        Dim dispPltX As Integer
        Dim dispPltY As Integer
        Dim dispBlkX As Integer
        Dim dispBlkY As Integer
        'Dim retBlkNoX As Integer
        'Dim retBlkNoY As Integer
        Dim dispCurStgGrpNoX As Integer
        Dim dispCurStgGrpNoY As Integer
        Dim dispCurBlkNoX As Integer
        Dim dispCurBlkNoY As Integer
        Dim dispCurPltNoX As Integer
        Dim dispCurPltNoY As Integer
        Dim StgX As Double = 0.0 ' V4.0.0.0-40
        Dim StgY As Double = 0.0 ' V4.0.0.0-40

        Try
            MoveTargetStagePos = frmFineAdjust.MOVE_NEXT
            intRet = GetTargetStagePos(pltNo, blkNo, nextStgX, nextStgY, dispPltX, dispPltY, dispBlkX, dispBlkY)
            If intRet = BLOCK_END Then
                ' �������Ȃ��ŏI��
                MoveTargetStagePos = frmFineAdjust.MOVE_NOT
                Exit Function
            ElseIf intRet = PLATE_BLOCK_END Then
                ' �������Ȃ��ŏI��
                MoveTargetStagePos = frmFineAdjust.MOVE_NOT
                Exit Function
            End If

            '---------------------------------------------------------------------
            '   �\���p�e�|�W�V�����̔ԍ���ݒ�i�v���[�g/�X�e�[�W�O���[�v/�u���b�N�j
            '---------------------------------------------------------------------
            Dim bRet As Boolean
            bRet = GetDisplayPosInfo(dispBlkX, dispBlkY, _
                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
            '#4.12.2.0�E                 ��
            Globals_Renamed.gCurPlateNoX = dispPltX
            Globals_Renamed.gCurPlateNoY = dispPltY
            Globals_Renamed.gCurBlockNoX = dispCurBlkNoX
            Globals_Renamed.gCurBlockNoY = dispCurBlkNoY
            '#4.12.2.0�E                 ��

            '---------------------------------------------------------------------
            '   ���O�\��������̐ݒ�
            '---------------------------------------------------------------------
            dispCurPltNoX = dispPltX : dispCurPltNoY = dispPltY         '###056
            Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY, _
                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)

            'V5.0.0.9�O                  ��
            If (2 = giStartBlkAss) Then
                Form1.Set_StartBlkNum(dispCurBlkNoX, dispCurBlkNoY)
            End If
            'V5.0.0.9�O                  ��

            ' �X�e�[�W�̓���
            '----- V1.13.0.0�B�� -----
            ' �L�k�␳�p�p�����[�^�̐ݒ�
            GetShinsyukuData(dispBlkX, dispBlkY, nextStgX, nextStgY)
            '----- V2.0.0.0�H�� -----
            If (giMachineKd = MACHINE_KD_RS) Then
                '----- V4.0.0.0-40�� -----
                ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
                '��
                'V4.6.0.0�C�@If (giStageYOrg = STGY_ORG_UP) Then
                StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
                'V4.6.0.0�C�@Else
                'V4.6.0.0�C�@StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                'V4.6.0.0�C�@StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY ' + (typPlateInfo.dblBlockSizeYDir / 2)
                'V4.6.0.0�C�@End If
                'V4.6.0.0�C�@��
                intRet = Form1.System1.EX_START(gSysPrm, StgX, StgY, 0)

                'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
                '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
                '----- V4.0.0.0-40�� -----
            Else
                intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
                                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
            End If
            'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
            '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
            '----- V2.0.0.0�H�� ----
            '----- V1.13.0.0�B�� -----
            '#4.12.1.0�I��
            If (intRet < (-1 * ERR_STG_STATUS)) Then
                ' �����I��
                Call Form1.AppEndDataSave()
                Call Form1.AplicationForcedEnding()
            End If
            '#4.12.1.0�I��

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.btnTrimming_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "GrpStartBlk�t���[������PREV/NEXT�����L���ɂ���"
    ''' <summary>
    ''' GrpStartBlk�t���[������PREV/NEXT�����L���ɂ���
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GrpStartBlkPartEnable()

        Form1.GrpStartBlk.Enabled = True
        Form1.CbStartBlkX.Enabled = False
        Form1.CbStartBlkY.Enabled = False

        Form1.btnNEXT.Enabled = True
        Form1.btnPREV.Enabled = True

    End Sub
#End Region
    ''V5.0.0.1�C��
#Region "Judge�{�^���̗L�������ؑւ�"
    ''' <summary>
    ''' Judge�{�^���̗L�������ؑւ�
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <remarks></remarks>
    Public Sub btnJudgeEnable(ByVal flg As Boolean)

        Form1.btnJudge.Enabled = flg

    End Sub
#End Region
    'V5.0.0.1�C��
    'V5.0.0.6�@��
#Region "�O���R���g���[���C���^�[���b�N����"
    ''' <summary>
    ''' �O���R���g���[���C���^�[���b�N����
    ''' </summary>
    ''' <returns>����FcFRS_NORMAL,�L�����Z���FcFRS_ERR_RST,����~:cFRS_ERR_EMG</returns>
    ''' <remarks></remarks>
    Public Function CheckControllerInterlock() As Integer
        Try
            Dim digL As Integer
            Dim digH As Integer
            Dim digSW As Integer
            Dim iBitData As Integer
            Dim RtnStatus As Short = cFRS_NORMAL
            Dim BuzzerFirst As Boolean = True
            Dim sMessage As String = "���x�R���g���[��"

            Call Form1.GetMoveMode(digL, digH, digSW)

            If Not gbControllerInterlock Then        ' �R���g���[���C���^�[���b�N�L��̎�
                Return (cFRS_NORMAL)
            End If

            If typPlateInfo.intControllerInterlock = 0 Then
                Return (cFRS_NORMAL)
            End If

            If digL <> TRIM_MODE_ITTRFT And digL <> TRIM_MODE_TRFT And digL <> TRIM_MODE_FT And digL <> TRIM_MODE_MEAS Then
                Return (cFRS_NORMAL)
            End If



            Do
                Call EXTIN(iBitData)
                If (iBitData And EXT_IN3) Then  ' �R���g���[���A���[��
                    sMessage = "���x�R���g���[���E�A���[�������ł��B"
                    RtnStatus = cFRS_ERR_START
                ElseIf (iBitData And EXT_IN2) Then  ' �R���g���[���K�����x
                    sMessage = "���x�R���g���[���E�ݒ艷�x�͈͊O�ł��B"
                    RtnStatus = cFRS_ERR_START
                Else
                    If (RtnStatus = cFRS_ERR_START) Then
                        'V5.0.0.9�M ��
                        '                        Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)            ' OFF
                        'Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)            ' OFF
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                        'V5.0.0.9�M ��

                    End If
                    RtnStatus = cFRS_NORMAL
                End If

                If RtnStatus <> cFRS_NORMAL And BuzzerFirst Then

                    'V5.0.0.9�M ��
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF) ' �W��(�ԓ_��+�u�U�[�P)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                    'V5.0.0.9�M ��

                    Call System.Threading.Thread.Sleep(1000)                                    ' �R�b
                    'V5.0.0.9�M ��
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_BZ1_ON)                         ' �u�U�[�POFF
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_BUZZER_OFF)
                    'V5.0.0.9�M ��

                    BuzzerFirst = False
                    Call SetLoaderIO(COM_STS_TRM_ERR, 0)    ' ���[�_�[�o��(ON=�g���}�G���[)
                End If
                'Call Form1.System1.SetSignalTower(0, &HFFFF)            ' ������ܰ����(On=0, Off=�S�ޯ�)

                If RtnStatus <> cFRS_NORMAL Then
                    Call SetLoaderIO(COM_STS_TRM_ERR, 0)    ' ���[�_�[�o��(ON=�g���}�G���[)
                    RtnStatus = Form1.System1.Form_MsgDispStartReset("START�{�^���ōĎ��s���܂��B", sMessage, "RESET�{�^���Œ��f���܂��B", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black))
                    Call ZCONRST()
                    If (RtnStatus = cFRS_ERR_START) Then
                    ElseIf (RtnStatus = cFRS_ERR_RST) Then
                        'V5.0.0.9�M ��
                        ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)            ' OFF
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                        'V5.0.0.9�M ��

                        Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ���[�_�[�o��(OFF=�g���}�G���[)
                        Return (cFRS_ERR_RST)   ' ���Z�b�g����
                    Else
                        Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ���[�_�[�o��(OFF=�g���}�G���[)
                        Return (cFRS_ERR_EMG)   ' ����~
                    End If
                End If
            Loop While (RtnStatus = cFRS_ERR_START)

            If BuzzerFirst = False Then
                Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ���[�_�[�o��(OFF=�g���}�G���[)
            End If

            Return (RtnStatus)

        Catch ex As Exception
            MsgBox("CheckControllerInterlock() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try

    End Function
#End Region
    'V5.0.0.6�@��
    'V5.0.0.6�A��
#Region "��Q���_�����E�A�N�Z�T�[(accessor)"
    ' ���[�_�[�����A�r�o���̃e�[�u���ʒu
    Private LoaderBordTableInPosX As Double = 0.0
    Private LoaderBordTableInPosY As Double = 0.0
    Private LoaderBordTableOutPosX As Double = 0.0
    Private LoaderBordTableOutPosY As Double = 0.0
    ''' <summary>
    ''' ��Q���_���W�̐ݒ�
    ''' </summary>
    ''' <param name="InPosX"></param>
    ''' <param name="InPosY"></param>
    ''' <param name="OutPosX"></param>
    ''' <param name="OutPosY"></param>
    ''' <remarks></remarks>
    Private Sub SetLoaderBordTablePos(ByVal InPosX As Double, ByVal InPosY As Double, ByVal OutPosX As Double, ByVal OutPosY As Double)
        Try
            If gbLoaderSecondPosition Then
                LoaderBordTableInPosX = InPosX
                LoaderBordTableInPosY = InPosY
                LoaderBordTableOutPosX = OutPosX
                LoaderBordTableOutPosY = OutPosY
                'TKY�́AForm1.System1.XYtableMove���g�p���Ă���̂�SBACK�֌��_���W�ύX�ݒ�͍s��Ȃ��B
                'Dim Rtn As Integer = SETLOADPOS(LoaderBordTableOutPosX, LoaderBordTableOutPosY)
                'If Rtn <> cFRS_NORMAL Then
                '    Call Form1.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH63, &HFF, &HFF0000)
                '    '"����[�ʒu�ύX���ُ�I�����܂����B(SETLOADPOS)"
                'End If
            End If
        Catch ex As Exception
            MsgBox("SetLoaderBordTablePos() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    Public Function GetLoaderBordTableInPosX() As Double
        Return (LoaderBordTableInPosX)
    End Function
    Public Function GetLoaderBordTableInPosY() As Double
        Return (LoaderBordTableInPosY)
    End Function
    Public Function GetLoaderBordTableOutPosX() As Double
        If gbLoaderSecondPosition Then
            Return (LoaderBordTableOutPosX)
        Else
            Return (gdblStg2ndOrgX)
        End If
    End Function
    Public Function GetLoaderBordTableOutPosY() As Double
        If gbLoaderSecondPosition Then
            Return (LoaderBordTableOutPosY)
        Else
            Return (gdblStg2ndOrgY)
        End If
    End Function
#End Region

#Region "�g���~���O�f�[�^�̐��i��ʕύX���̏���"
    ''' <summary>
    ''' �g���~���O�f�[�^�̐��i��ʕύX���̏���
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChangeByTrimmingData()
        Try
            If gbLoaderSecondPosition Then
                Dim InPosX As Double, InPosY As Double, OutPosX As Double, OutPosY As Double
                If 1 <= typPlateInfo.intWorkSetByLoader And typPlateInfo.intWorkSetByLoader <= 10 Then
                    InPosX = Double.Parse(GetPrivateProfileString_S("SYSTEM", "INSERTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "X", Form1.LOADER_PARAMPATH, "0.0"))
                    InPosY = Double.Parse(GetPrivateProfileString_S("SYSTEM", "INSERTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "Y", Form1.LOADER_PARAMPATH, "0.0"))
                    OutPosX = Double.Parse(GetPrivateProfileString_S("SYSTEM", "EJECTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "X", Form1.LOADER_PARAMPATH, "0.0"))
                    OutPosY = Double.Parse(GetPrivateProfileString_S("SYSTEM", "EJECTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "Y", Form1.LOADER_PARAMPATH, "0.0"))
                    SetLoaderBordTablePos(InPosX, InPosY, OutPosX, OutPosY)
                Else
                    SetLoaderBordTablePos(0.0, 0.0, 0.0, 0.0)
                End If
            End If
        Catch ex As Exception
            MsgBox("ChangeByTrimmingData() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region
    'V5.0.0.6�A��
    'V6.0.1.0�N��
#Region "��P����NG���̔���"
    '''=========================================================================
    ''' <summary>��P����NG���̔���</summary>
    ''' <returns>0:����A1:NG��������</returns>
    '''=========================================================================
    Public Function JudgePlateNGCount(ByVal SetNgCount As Integer, ByVal NgCount As Integer) As Integer

        Dim r As Integer = 0
        Dim strMsg As String
        Dim strMsg1 As String


        ' �V�O�i���^���[��ԓ_��+�u�U�[ON����
        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)

        strMsg = MSG_SPRASH66 + SetNgCount.ToString("0")
        strMsg1 = MSG_SPRASH67 + NgCount.ToString("0")

        r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                strMsg, strMsg1, MSG_frmLimit_07, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)

        ' �W��
        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)

        Return r
        '-----------------------------------------------------------

    End Function
#End Region
    'V6.0.1.0�N��
    '----- V6.0.3.0_26�� -----
#Region "FT���ϒl(Double)�̎Z�o"
    '''=========================================================================
    '''<summary>FT���ϒl(Double)�̐ݒ�</summary> 
    '''=========================================================================
    Public Sub SetAverageFTValue(ByVal dblMesVal As Double, ByVal lngNxMax As Long)

        Dim strMSG As String

        Try

            TotalFTValue = TotalFTValue + dblMesVal

            TotalAverageFTValue = TotalFTValue / lngNxMax

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.SetAverageFTValue() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Exit Sub

    End Sub
#End Region

#Region "FT���ϒl�̎擾"
    '''=========================================================================
    '''<summary>FT���ϒl�̎擾</summary> 
    '''=========================================================================
    Public Function GetAverageFTValue() As Double

        Return TotalAverageFTValue

    End Function
#End Region
    '----- V6.0.3.0_26�� -----
    '----- V6.0.3.0�L�� -----
#Region "�����_�ȉ��̌��������߂�"
    '''=========================================================================
    '''<summary>�����_�ȉ��̌��������߂�</summary> 
    ''' <param name="strDigitNum">(INP)gsEDIT_DIGITNUM("0.00000"</param>
    ''' <returns>�����_�ȉ��̌���(5��,7��)</returns>
    '''=========================================================================
    Public Function GetDecPointSize(ByVal strDigitNum As String) As Integer

        Dim DecPoint As Integer
        Dim strMSG As String

        Try
            strDigitNum = strDigitNum.Trim()
            DecPoint = strDigitNum.IndexOf(".")                         ' �����_�ʒu������ (strDigitNum("0.00000")'
            If (DecPoint < 0) Then DecPoint = 1 '                       ' �������Ȃ�
            DecPoint = strDigitNum.Length - (DecPoint + 1)              ' �����_�ȉ��̌���

            Return (DecPoint)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GetDecPointSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (5)                                                  ' Return�l = �W���l��Ԃ�
        End Try
    End Function
#End Region
    '----- V6.0.3.0�L�� -----
    '----- V6.1.1.0�B�� -----
#Region "�����^�]�J�n���Ԃ��擾����(SL436R���I�v�V����)"
    '''=========================================================================
    ''' <summary>�����^�]�J�n���Ԃ��擾����</summary>
    ''' <param name="cStartTime">(OUT)�J�n����</param>
    ''' <remarks>IAM�a�I�v�V����</remarks>
    '''=========================================================================
    Public Sub DispTrimStartTime(ByRef cStartTime As DateTime)

        Dim strMSG As String

        Try
            ' �����^�]�J�n���Ԃ��擾����
            cStartTime = DateTime.Now
            cStartTime = TruncMillSecond(cStartTime)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.DispTrimStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�����^�]�I�����Ԃ�\������(SL436R���I�v�V����)"
    '''=========================================================================
    ''' <summary>�����^�]�I�����Ԃ�\������</summary>
    ''' <param name="cStartTime">(INP)�J�n����</param>
    ''' <param name="cEndTime">  (OUT)�I������</param>
    ''' <remarks>IAM�a�I�v�V����</remarks>
    '''=========================================================================
    Public Sub DispTrimEndTime(ByRef cStartTime As DateTime, ByRef cEndTime As DateTime)

        Dim ts As TimeSpan
        Dim strDat As String
        Dim strMSG As String

        Try
            ' �I�����Ԃ����߂�
            cEndTime = DateTime.Now
            cEndTime = TruncMillSecond(cEndTime)

            ' �o�ߎ��Ԃ����߂�
            ts = cEndTime - cStartTime
            strDat = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00")

            ' �����^�]�J�n���Ԃ�\������
            strMSG = "AUTOMATIC DRIVING START = " + cStartTime.ToString("H:mm:ss")
            Call Form1.Z_PRINT(strMSG)

            ' �����^�]�I�����Ԃ�\������
            strMSG = "AUTOMATIC DRIVING END   = " + cEndTime.ToString("H:mm:ss") + " (" + strDat + ")"
            Call Form1.Z_PRINT(strMSG)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "basTrimming.DispTrimEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
#Region "�~���b��؂�̂ĂĕԂ�"
    '''=========================================================================
    ''' <summary>�~���b��؂�̂ĂĕԂ�</summary>
    ''' <param name="source">(INP)����</param>
    ''' <returns>����</returns>
    '''=========================================================================
    Public Function TruncMillSecond(source As DateTime) As DateTime
        Dim d = New DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, 0)
        Return d
    End Function
#End Region
    '----- V6.1.1.0�B�� -----
    '----- V6.1.1.0�L�� -----
#Region "���݂̎�����Ԃ�"
    '''=========================================================================
    ''' <summary>���݂̎�����Ԃ�</summary>
    ''' <param name="sDate">(OUT)����</param>
    '''=========================================================================
    Public Sub Get_NowYYMMDDHHMMSS(ByRef sDate As String)

        Dim NowTime As DateTime

        NowTime = DateTime.Now
        sDate = NowTime.ToString("yyyy/MM/dd H:mm:ss")

    End Sub
#End Region
    '----- V6.1.1.0�L�� -----
#Region "��P����NG���̔���" 'V4.5.0.5�@ 'V4.12.2.0�H�@'V6.0.5.0�C
    '''=========================================================================
    ''' <summary>��P����NG���̔���</summary>
    ''' <returns>0:����A1:NG��������</returns>
    '''=========================================================================
    Public Function JudgePlateNGRate(ByVal automode As Boolean) As Integer

        Dim iResistor As Integer
        Dim i As Integer
        Dim PlateresCnt As Integer
        Dim dJudgeRate As Double
        Dim r As Integer = 0
        Dim strMsg As String
        Dim strMsg1 As String

        '-----------------------------------------------------------
        '   NG����臒l�̎Z�o
        '-----------------------------------------------------------

        ' �P�u���b�N�̒�R�����J�E���g
        iResistor = 0

        ' ��R�����������s�Ȃ��B
        For i = 1 To gRegistorCnt
            ' ��R�ԍ�<1000�ł��邩�`�F�b�N
            If typResistorInfoArray(i).intResNo < 1000 Then
                ' �P�u���b�N�̒�R���̃J�E���g
                iResistor = iResistor + 1
            End If
        Next

        ' �P��̒�R�����P�u���b�N����R���~�u���b�N��X�~�u���b�N��Y�~�v���[�g��X�~�v���[�g��Y
        PlateresCnt = iResistor * typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir * typPlateInfo.intPlateCntXDir * typPlateInfo.intPlateCntYDir

        ' ��R�����Ȃ��ꍇ�ɂ́A���̂܂ܐ���Ŗ߂�
        If (PlateresCnt = 0) Then
            Return r
        End If

        ' �S�̂ɑ΂���NG�� = NG�� / �P�����R��
        dJudgeRate = (m_NgCountInPlate / PlateresCnt) * 100.0#

        r = 0
        If (dJudgeRate >= gdblNgStopRate) And (gdblNgStopRate <> 0) Then

            ' �V�O�i���^���[��ԓ_��+�u�U�[ON����
            '            Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)

            strMsg = MSG_SPRASH79 + gdblNgStopRate.ToString("0.0") + " (%)"
            strMsg1 = MSG_SPRASH80 + dJudgeRate.ToString("0.0") + " (%) : " + m_NgCountInPlate.ToString + MSG_SPRASH81 + " / " + PlateresCnt.ToString + MSG_SPRASH81

            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                    strMsg, strMsg1, MSG_SPRASH82, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
            If (r = cFRS_ERR_START) Then
                ' ���b�g���s
                r = cFRS_ERR_START
            Else
                ' ���b�g���fRESET����
                r = cFRS_ERR_RST
            End If

            ' �W��(�ԓ_��+�u�U�[�P)
            'V4.12.2.2�F��
            If automode = True Then
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
            Else
                Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
            End If
            'V4.12.2.2�F��

            Return (r)
        End If
        Return r
        '-----------------------------------------------------------

    End Function
#End Region

    ''' <summary>
    ''' �O���t�\���t�H�[���̍Đݒ� 'V6.0.2.0�C
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GraphDispSet()

        Try

            If (Form1.chkDistributeOnOff.Checked = True) Then              ' ���v��ʕ\�����ɋN������ 

                'Form1.changefrmDistStatus(1)                            ' �O���t�\�� 
                'gObjFrmDistribute.Visible = True
                'gObjFrmDistribute.Show()
                gObjFrmDistribute.ShowGraph()
            End If
        Catch ex As Exception

        End Try

    End Sub

End Module
