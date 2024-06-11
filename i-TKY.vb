'===============================================================================
'   Description  : �g���~���O�v���O�������C������
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Collections.Generic      'V4.10.0.0�B
Imports System.Globalization            'V4.4.0.0-0
Imports System.Linq                     'V5.0.0.9�D
Imports System.Text                     'V4.4.0.0-0
Imports System.Threading                'V4.4.0.0-0
Imports DllPrintString                  'V6.0.1.0�K
Imports LaserFront.Trimmer
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.DllTeach.Teaching
Imports LaserFront.Trimmer.DllVideo     'V5.0.0.9�D
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports Microsoft.Win32
Imports TKY_ALL_SL432HW.My.Resources
Imports TrimClassLibrary                '#4.12.2.0�@
Imports TrimControlLibrary              'V6.0.1.0�K
Imports TrimDataEditor                  'V4.10.0.0�@

Friend Class Form1
    Inherits System.Windows.Forms.Form

    Friend Shared Instance As Form1     'V5.0.0.9�P

    Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_MOVE As Integer = &HF010

    Private Const WM_ACTIVATEAPP As Integer = &H1C                              'V6.0.1.0�C
    Private ReadOnly _keysNone As KeyEventArgs = New KeyEventArgs(Keys.None)    'V6.0.1.0�C
    '===============================================================================
    '   �萔��`
    '===============================================================================
    '----- API��` -----
    'Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long 'V1.14.0.0�A

#Region "���C���t�H�[���̃}�E�X�ł̈ʒu�ړ��𖳌��ɂ���"
    <System.Security.Permissions.SecurityPermission( _
        System.Security.Permissions.SecurityAction.LinkDemand, _
        Flags:=System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode), _
        System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub WndProc(ByRef m As Message)
        'V6.0.1.0�C        If m.Msg = WM_SYSCOMMAND AndAlso
        'V6.0.1.0�C            (m.WParam.ToInt32() And &HFFF0) = SC_MOVE Then
        'V6.0.1.0�C            m.Result = IntPtr.Zero
        'V6.0.1.0�C            Return
        'V6.0.1.0�C        End If

        Select Case (m.Msg)
            Case WM_SYSCOMMAND
                ' ���C���t�H�[���̃}�E�X�ł̈ʒu�ړ��𖳌��ɂ���
                If ((m.WParam.ToInt32() And &HFFF0) = SC_MOVE) Then
                    m.Result = IntPtr.Zero
                    Return
                End If

            Case WM_ACTIVATEAPP         'V6.0.1.0�C
                ' �A�v���P�[�V��������A�N�e�B�u�ɂȂ鎞�ɔO�̂���JogKeyUp����
                If (IntPtr.Zero = m.WParam) Then
                    If (Me._jogKeyUp IsNot Nothing) Then
                        Me._jogKeyUp.Invoke(_keysNone)
                        Debug.Print("WM_ACTIVATEAPP - JogKeyUp")
                    End If
                End If
        End Select

        MyBase.WndProc(m)
    End Sub
#End Region

#Region "�萔/�ϐ���`"
    '===========================================================================
    '   �萔/�ϐ���`
    '===========================================================================
    '----- WIN32 API -----
    Public Structure SECURITY_ATTRIBUTES
        Dim nLength As Integer              '�\���̂̃o�C�g��
        Dim lpSecurityDescriptor As Integer '�Z�L�����e�B�f�X�N���v�^(Win95,98�ł͖���)
        Dim bInheritHandle As Integer       '1�̂Ƃ��A�������p������
    End Structure

    '-------------------------------------------------------------------------------
    '   LogClient.dll���̊֐���`
    '-------------------------------------------------------------------------------
    Public Declare Function CreateEvent Lib "kernel32" Alias "CreateEventA" (ByRef lpEventAttributes As SECURITY_ATTRIBUTES, ByVal ManualReset As Integer, ByVal bInitialState As Integer, ByVal lpName As String) As Integer
    Public Declare Function WaitForSingleObject Lib "KERNEL32.DLL" (ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer
    Public Declare Function SetEvent Lib "kernel32" (ByVal hEvent As Integer) As Integer
    Public Declare Function ResetEvent Lib "kernel32" (ByVal hEvent As Integer) As Integer
    'Private Declare Function GetPrivateProfileInt Lib "kernel32" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer

    '----- �萔��` -----
    Private Const CUR_CRS_LINEX As Short = 45               ' �۽ײ�X�\���ʒu�̕␳�l
    Private Const CUR_CRS_LINEY As Short = 8                ' �۽ײ�Y�\���ʒu�̕␳�l
    Private Const cLOGcBUFFERcSIZE As Double = 8192 * 3.5   ' ۸މ�ʕ\���̍ő廲��

    Private Const cThetaAnglMin As Double = -5.0            ' �Ɖ�]�ŏ��p�x
    Private Const cThetaAnglMax As Double = 5.0             ' �Ɖ�]�ő�p�x

    '----- �f�[�^�ҏW�v���O�����I���R�[�h��` -----
    Private Const PRC_EXIT_CAN As Integer = 0               ' ����I��(Cancel�w��) 
    Private Const PRC_EXIT_NML As Integer = 1               ' ����I��(�f�[�^�Z�[�u�v���Ȃ�) 
    Private Const PRC_EXIT_NML2 As Integer = 2              ' ����I��(�f�[�^�Z�[�u�v������) 
    Private Const PRC_EXIT_MLTEXE As Integer = 900          ' ���d�N���G���[ 
    Private Const PRC_EXIT_ENDREQ As Integer = 901          ' �I�����b�Z�[�W��M
    Private Const PRC_EXIT_ERRPRM As Integer = 902          ' �s������
    Private Const PRC_EXIT_ERRFIO As Integer = 903          ' �t�@�C���h�n�G���[
    Private Const PRC_EXIT_ERRCNDIO As Integer = 904        ' ���H��������M�G���[
    Private Const PRC_EXIT_TRAP As Integer = 999            ' �g���b�v�G���[ 

    '----- �t�@�C���p�X�� -----                                         
    Private Const EditWorkFilePath As String = "C:\TRIMDATA\DATA\tmpTrimData"   ' �f�[�^�ҏW�p���ԃt�@�C����
    Private Const cTEMPLATPATH As String = "C:\TRIM\VIDEO"                      ' Video.OCX�p����ڰ�̧�ق̕ۑ��ꏊ
    Private Const WORK_DIR_PATH As String = "C:\TRIM"                           ' ��Ɨp̫��ް
    Private Const DATA_DIR_PATH As String = "C:\TRIMDATA\DATA"                  ' �ް�̧��̫��ް 
    Private Const SYSPARAMPATH As String = "C:\TRIM\tky.ini"                    ' �V�X�e���p�����[�^�p�X��
    Public Const TKYSYSPARAMPATH As String = "C:\TRIM\TKYSYS.INI"              ' �V�X�e���p�����[�^�p�X�� V2.0.0.0_29 'V6.1.4.0_39 Public��
    Public Const LOADER_PARAMPATH As String = "C:\TRIM\LOADER.ini"             ' �V�X�e���p�����[�^�p�X��(���[�_�����e�i���X�p)  ###069 'V5.0.0.6�A Private��Public�ύX
    Public Const ENTRY_PATH As String = "C:\TRIMDATA\ENTRYLOT\"                 'V6.1.4.0_50
    '----- �ϐ���` -----
    'V6.0.0.0    Public ObjIOMon As Object = Nothing                    ' �h�n���j�^�\���p
    Public ObjIOMon As FrmIOMon = Nothing                   ' �h�n���j�^�\���p      'V6.0.0.0

    '----- ���O��ʂ̊g��p -----
    Private TxtLog_NmlLocat As System.Drawing.Point         ' �ʏ탍�O��ʂ̈ʒu(X,Y)
    Private TxtLog_NmlSize As System.Drawing.Size           ' �ʏ탍�O��ʂ̻���(X,Y)
    Private TxtLog_ExpLocat As System.Drawing.Point         ' �g�働�O��ʂ̈ʒu(X,Y)
    Private TxtLog_ExpSize As System.Drawing.Size           ' �g�働�O��ʂ̻���(X,Y)

    '----- �t���O -----
    Private pbVideoInit As Boolean                          ' �r�f�IInit�t���O
    Private pbVideoCapture As Boolean                       ' �r�f�I�L���v�`���[�J�n�t���O
    Private gflgResetStart As Boolean                       ' �����ݒ�t���O(True=�����ݒ�ς�, False=�����ݒ�ς݂łȂ�)
    Private gflgCmpEndProcess As Boolean                    ' �I�����������t���O�iTrue=�I���������s�ς݁AFalse=�I���������s�ς݂łȂ��j

    '----- ������ԕێ��ϐ� -----
    '' '' ''Private m_keepHaltSwSts As Boolean                      ' timer��������HALT�X�C�b�`�ݒ�ۑ��ϐ�
    Private m_bStopRunning As Boolean                       ' �A���ғ��e�X�g���{�t���O
    Private m_bDispDistributeSts As Boolean                 ' ���v�����\����ԕۑ��ϐ�

    '-------------------------------------------------------------------------------
    '   �@�\�I���`�e�[�u��
    '-------------------------------------------------------------------------------
    '----- SL432R�n�p ----- 
    Private Const cDEF_FUNCNAME_TKY As String = "C:\TRIM\DefFunc_Tky.INI"
    Private Const cDEF_FUNCNAME_CHIP As String = "C:\TRIM\DefFunc_TkyChip.INI"
    Private Const cDEF_FUNCNAME_NET As String = "C:\TRIM\DefFunc_TkyNet.INI"
    '----- SL436R�n�p ----- 
    Private Const cDEF_FUNCNAME_CHIP_436 As String = "C:\TRIM\DefFunc_TkyChip_436.INI"
    Private Const cDEF_FUNCNAME_NET_436 As String = "C:\TRIM\DefFunc_TkyNet_436.INI"

    '----- �@�\�I���`�e�[�u���`����` (��`�t�@�C��(TKY_DEFFUNC.INI)���ݒ肷��) -----
    Public Structure FNC_DEF
        Dim iDEF As Short                                   ' �@�\�I���`(-1:��\��, 0:�I��s��, 1:�I����)
        Dim iPAS As Short                                   ' �p�X���[�h�w��(0:�p�X���[�h�Ȃ�, 1:�p�X���[�h����)
        Dim sCMD As String                                  ' �����(�L�[��)
    End Structure
    Public stFNC(MAX_FNCNO) As FNC_DEF                      ' �@�\�I���`�e�[�u��
    'Public flgLoginPWD As Short                             ' ۸޲��߽ܰ�ޓ��̗͂L��(0:��, 1:�L) V6.0.3.0_42

    Public strBTN(MAX_FNCNO + 1) As String                  ' �{�^�����̔z��(0 ORG) V1.18.0.1�@

    'V3.0.0.0�D    Private ObjGazou As Process = Nothing                   ' Process�I�u�W�F�N�g

    '----- V6.1.4.0�F��(KOA EW�aSL432RD�Ή�) -----
#Region "LblDataFileName��Text�ύX���ɑ�P��R�̶���ް��\�������������Ȃ�"
    '''=========================================================================
    ''' <summary>LblDataFileName��Text�ύX���ɑ�P��R�̶���ް��\�������������Ȃ�</summary>
    ''' <value>۰�ނ����ް�̧�ٖ�</value>
    ''' <returns>LblDataFileName.Text</returns>
    ''' <remarks>V6.1.4.0�F</remarks>
    '''=========================================================================
    Public Property LblDataFileNameText() As String
        Get
            Return LblDataFileName.Text
        End Get
        Set(ByVal value As String)
            ' ��P��R�̶���ް��\����ύX����
            If (String.Empty = value) Then
                SetFirstResData(True)
            Else
                SetFirstResData()
            End If
            LblDataFileName.Text = value
        End Set
    End Property
#End Region
    '----- V6.1.4.0�F�� -----

    Private _jogKeyDown As Action(Of KeyEventArgs) = Nothing
    Private _jogKeyUp As Action(Of KeyEventArgs) = Nothing

#Region "�\������JOG�𐧌䂷��KeyDown,KeyUp���̏��������C���t�H�[����"
    '''=========================================================================
    ''' <summary><para>�\������JOG�𐧌䂷��KeyDown,KeyUp���̏��������C���t�H�[���ɁA</para>
    ''' <para>�J�����摜MouseClick���̏�����DllVideo�ɐݒ肷��</para></summary>
    ''' <param name="keyDown"></param>
    ''' <param name="keyUp"></param>
    ''' <param name="moveToCenter">�J�����摜�N���b�N�ʒu���摜�Z���^�[�Ɉړ����鏈��</param>
    ''' <remarks>'V6.0.0.0�I</remarks>
    '''=========================================================================
    Friend Sub SetActiveJogMethod(ByVal keyDown As Action(Of KeyEventArgs),
                                  ByVal keyUp As Action(Of KeyEventArgs),
                                  ByVal moveToCenter As Action(Of Decimal, Decimal))
        _jogKeyDown = keyDown
        _jogKeyUp = keyUp

        '�J�����摜�\��PictureBox�N���b�N�ʒu��JOG�o�R�ŉ摜�Z���^�[�Ɉړ�����
        VideoLibrary1.MoveToCenter = moveToCenter
    End Sub
#End Region

    Private mExit_flg As Integer                            ' �{�^�����������ʊi�[�p
    Private _readFileVer As Double = FileIO.FILE_VER_10_10  ' ���[�h�����t�@�C���̃o�[�W������ێ�����   V4.0.0.0-28
    Private _editNewData As Boolean = False                 'V4.12.4.0�A
    Private _mapDisp As Boolean = False                     '#4.12.2.0�@
    Private _mapPdfDir As String                            'V6.0.1.0�K
    Private _PrintDisp As Boolean = False                   'V6.1.1.0�A
#End Region

#Region "�V���b�g�_�E������-�����I��"
    '''=========================================================================
    ''' <summary>�V���b�g�_�E������-�����I��</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub SystemEvents_SessionEnding(
            ByVal sender As Object,
            ByVal e As SessionEndingEventArgs)
        If e.Reason = SessionEndReasons.SystemShutdown Then
            Call AplicationForcedEnding()
        End If
    End Sub
#End Region

    '=========================================================================
    '   �t�H�[���̏�����/�I������
    '=========================================================================
#Region "�\������ݒ�"
    '''=========================================================================
    ''' <summary>�\������ݒ�</summary>
    ''' <remarks>InitializeComponent() �����O�ɌĂяo������  'V4.4.0.0-0</remarks>
    '''=========================================================================
    Private Sub SetCurrentUICulture()
        ' �\���̂̏�����
        Call Init_Struct()
        Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)     ' ����׎擾

        Dim culture As CultureInfo
        Select Case gSysPrm.stTMN.giMsgTyp
            Case 0      ' ���{��(����l)
                culture = New CultureInfo("ja")
            Case 1      ' �p��
                culture = New CultureInfo("en")
            Case 2      ' ְۯ��
                culture = New CultureInfo("en")
            Case 3      ' ������(�ȑ̎�)
                culture = New CultureInfo("zh-Hans")
            Case Else   ' ����l(���{��)
                culture = New CultureInfo("ja")
        End Select

        Thread.CurrentThread.CurrentCulture = culture           ' �s�v ?
        Thread.CurrentThread.CurrentUICulture = culture

    End Sub

#End Region
    '----- V6.1.1.0�F�� -----
#Region "��ʂ��\�����ꂽ���̏���"
    '''=========================================================================
    ''' <summary>��ʂ��\�����ꂽ���̏���</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Dim strMSG As String

        Try

            PanelMap.Enabled = _mapDisp
            PanelMap.Visible = _mapDisp                                 ' MAP ON/OFF�{�^����ݒ肷��
            CmdPrintMap.Visible = Not _PrintDisp                        ' PRINT ON/OFF�{�^��

            If (_mapDisp) Then
                PanelMap.Location = New Point(txtLog.Left, txtLog.Top - PanelMap.Height)
                CmdMapOnOff.Enabled = True
            Else
                CmdPrintMap.Enabled = False
                CmdMapOnOff.Enabled = False
            End If

            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
            ' ���Y�Ǘ����̓`�[�m��.�ƒ�R�l��\������(�y�p�q�R�[�h���[�h�@�\�z����)
            If (giQrCodeType = QrCodeType.KoaEw) Then
                Me.LblcLOTNUMBER.Visible = True                                     ' ���Y�Ǘ����(�`�[�m���D) 
                Me.LblcRESVALUE.Visible = True                                      ' ���Y�Ǘ����(��R�l)
                If (gTkyKnd = KND_CHIP) Then                                        'V6.1.4.9�C
                    Me.pnlFirstResData.Location = New System.Drawing.Point(75, 757)     'V6.1.4.4�@                                                                  'V6.1.4.14�@�ړ�
                    Me.chkDistributeOnOff.Location = New System.Drawing.Point(12, 743)  ' ��P��R�J�b�g�f�[�^�\����ɔ��̂Ő��Y�O���t�\���{�^���̈ʒu���グ��     'V6.1.4.14�@�ړ�
                    Me.pnlFirstResData.Visible = True                                   ' ��P��R�J�b�g�f�[�^�\����
                    Me.pnlFirstResData.Enabled = True                                   'V6.1.4.14�@
                End If                                                              'V6.1.4.9�C
                'V6.1.4.4�@                Me.pnlFirstResData.Location = New System.Drawing.Point(75, 783)
                'V6.1.4.14�@                Me.pnlFirstResData.Location = New System.Drawing.Point(75, 757)     'V6.1.4.4�@
                'V6.1.4.14�@                Me.chkDistributeOnOff.Location = New System.Drawing.Point(12, 743)  ' ��P��R�J�b�g�f�[�^�\����ɔ��̂Ő��Y�O���t�\���{�^���̈ʒu���グ��
                'V6.1.4.14�@��
                If (gTkyKnd = KND_NET) Then
                    Me.pnlFirstResDataNET.Location = New System.Drawing.Point(75, 750)
                    Me.chkDistributeOnOff.Location = New System.Drawing.Point(12, 728)  ' �\���{�^���̈ʒu
                    Me.pnlFirstResDataNET.Visible = True                            ' �m�d�s���͒�R�S�̃J�b�g�I�t�\��
                    Me.pnlFirstResDataNET.Enabled = True
                End If
                'V6.1.4.14�@��
            Else
                Me.LblcLOTNUMBER.Visible = False
                Me.LblcRESVALUE.Visible = False
                Me.pnlFirstResData.Visible = False
            End If
            '----- V6.1.4.0_22�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Form1_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V6.1.1.0�F�� -----
#Region "���C���t�H�[���̏���������"
    '''=========================================================================
    '''<summary>���C���t�H�[���̏���������</summary>
    '''<remarks>Form_Initialize �� Form_Initialize_Renamed �ɃA�b�v�O���[�h����܂����B</remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        '----- V1.16.0.2�@�� -----
        Dim strSECT As String
        Dim strKEY As String
        Dim sPath As String
        Dim i As Integer
        '----- V1.16.0.2�@�� -----
        Dim strMSG As String
        Dim r As Short

        Try
            '-----------------------------------------------------------------------
            '   ���d�N���h�~Mutex�n���h��
            '-----------------------------------------------------------------------
            If gmhTky.WaitOne(0, False) = False Then
                '' ���łɋN������Ă���ꍇ
                '   �����b�Z�[�W�{�b�N�X���r�s�`�q�s�{�^�����͑҂��Ȃǂ̏�ԂŁA���ɉ�邱�Ƃ�����̂ŁA�\���͂�߂�B
                'MessageBox.Show("Cannot run TKY's family.(Another Process of TKY's family is already running.", "Trimmer Program", _
                '                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, _
                '                MessageBoxOptions.ServiceNotification, False)
                End
            End If

            '�V���b�g�_�E���C�x���g�����֐�
            AddHandler SystemEvents.SessionEnding, AddressOf SystemEvents_SessionEnding

            '-----------------------------------------------------------------------
            '   ��������
            '-----------------------------------------------------------------------
            ChDir(WORK_DIR_PATH)                        ' ��Ɨp̫��ް(C:\TRIM)
            Timer1.Enabled = False                      ' �Ď��^�C�}�[��~
            ' INtime�ғ��`�F�b�N (��#If cOFFLINEcDEBUG���g�p�����INtimeGWInitialize�̒�`�Ȃ��ƂȂ�)
#If 0 = cOFFLINEcDEBUG Then
            r = ISALIVE_INTIME()
            If (r = ERR_INTIME_NOTMOVE) Then
                '�G���[���b�Z�[�W�̕\���B(System1.TrmMsgBox�͂����ł͎g�p�ł��Ȃ��ׁA�W�����b�Z�[�W�{�b�N�X)
                MessageBox.Show("Real-time control module has not loaded.", "Trimmer Program", MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly, False)
                'Call MsgBox("Real-time control module has not loaded.", vbOKOnly Or vbCritical, gAppName)
                End                                             ' �A�v���I�� 
            End If
#End If

            ' �R�}���h���C������(�A�v���P�[�V�������)�̎擾
            Dim strARG() As String
            strARG = Environment.GetCommandLineArgs()       ' �R�}���h���C�������̎擾
            If (strARG.Length <= 1) Then                    ' �����Ȃ� ?
                strMSG = Process.GetCurrentProcess().ProcessName
                Console.WriteLine("Process Name = " + strMSG)
                If (strMSG = "TKYCHIP") Then
                    gTkyKnd = KND_CHIP                      ' �A�v���P�[�V������� = TKYCHIP
                ElseIf (strMSG = "TKYNET") Then
                    gTkyKnd = KND_NET                       ' �A�v���P�[�V������� = TKYNET
                Else
                    gTkyKnd = KND_TKY                       ' �A�v���P�[�V������� = TKY
                End If
            Else                                            ' �������� 
                gTkyKnd = strARG(1)                         ' �A�v���P�[�V�������(0:TKY, 1=CHP, 2:NET)
                If (gTkyKnd <> KND_TKY) And (gTkyKnd <> KND_CHIP) And (gTkyKnd <> KND_NET) Then
                    MsgBox("ArgV Error.", vbOKOnly Or vbCritical, gAppName)
                    End                                     ' �A�v���I�� 
                End If
            End If

            DataManager.Initialize(DirectCast(CType(gTkyKnd, Integer), TkyKind))        'V5.0.0.8�@

            ' �A�v���P�[�V������ʂ̐ݒ�
            If (gTkyKnd = KND_CHIP) Then
                gAppName = APP_CHIP

                'INFO_MSG18 = "��1�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"
                Globals_Renamed.INFO_MSG18 = INFO_MSG18_CHIP
                'INFO_MSG20 = "�O���[�v�A�ŏI��R��ʒu�̃e�B�[�`���O"
                Globals_Renamed.INFO_MSG20 = INFO_MSG20_CHIP
                'ERR_TXNUM_E = "��R�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
                Globals_Renamed.ERR_TXNUM_E = ERR_TXNUM_E_CHIP

            ElseIf (gTkyKnd = KND_NET) Then
                gAppName = APP_NET

                'INFO_MSG18 = "��1�O���[�v�A��1�T�[�L�b�g�ʒu�̃e�B�[�`���O"
                Globals_Renamed.INFO_MSG18 = INFO_MSG18_NET
                'INFO_MSG20 = "�O���[�v�A�ŏI�T�[�L�b�g��ʒu�̃e�B�[�`���O"
                Globals_Renamed.INFO_MSG20 = INFO_MSG20_NET
                'ERR_TXNUM_E = "�T�[�L�b�g�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
                Globals_Renamed.ERR_TXNUM_E = ERR_TXNUM_E_NET

            Else
                gAppName = APP_TKY
            End If

            'V6.5.5.0�@��
            Dim CpuNo As Integer = Integer.Parse(GetPrivateProfileString_S("DEVICE_CONST", "PROCESS_AFFINITY", SYSPARAMPATH, "0"))
            If (1 <= CpuNo) AndAlso (CpuNo <= &H7F) Then 'CPU�w���BIT���蓖�Ă�CPU0�`CPU6�܂ł�L���Ƃ��Ă��̏ꍇ�̂ݐݒ肷��
                Dim tmpProcess As Process = Process.GetCurrentProcess()
                Dim vHandle As IntPtr
                vHandle = tmpProcess.ProcessorAffinity
                tmpProcess.ProcessorAffinity = CpuNo
            End If
            'V6.5.5.0�@��

            ' �t���O��������
            gbInitialized = False
            pbVideoInit = False
            pbVideoCapture = False                          ' �r�f�I�L���v�`���[�J�n�t���OOFF
            gflgResetStart = False                          ' �����ݒ�t���OOFF
            'bFgfrmDistribution = False                      ' ���Y���̕\���׸�OFF
            gLoadDTFlag = False                              ' �ް�۰�ލ��׸�(False:�ް���۰��, True:�ް�۰�ލ�)
            gCmpTrimDataFlg = 0                             ' �f�[�^�X�V�t���O(0=�X�V�Ȃ�, 1=�X�V����)
            giTrimErr = 0                                   ' ��ϰ �װ �׸ޏ�����
            giAppMode = APP_MODE_IDLE                       ' ����Ӱ�� = �g���}���u�A�C�h����
            giTempGrpNo = 1                                 ' �e���v���[�g�O���[�v�ԍ�(1�`999)
            gPrevTrimMode = -1                              ' �f�W�^���r�v�l������
            gESLog_flg = False                              ' ES���OOFF
            'giAdjKeybord = 0                                 ' �g���~���O��ADJ�@�\�L�[�{�[�h���(0:���͂Ȃ�)
            gLoggingHeader = False                          ' ̧��۸�ͯ�ް�o���׸�(TRUE:�o��)
            gwPrevHcmd = 0                                  ' ���[�_���̓f�[�^�ޔ���
            gManualThetaCorrection = True                   ' �V�[�^�␳���s�t���O = True(�V�[�^�␳�����s����)
            gflgCmpEndProcess = False                       ' �I�����������t���O
            gbFgAutoOperation = False                       ' �����^�]�t���O(True:�����^�]��, False:�����^�]���łȂ�) 
            giAutoDataFileNum = 0                           ' �A���^�]�o�^�f�[�^�t�@�C���� 
            giSubExistMsgFlag = True                        ' ��L���`�F�b�N���Ȃ��ꍇ�Ƀ��b�Z�[�W��\�����Ȃ� V4.11.0.0�G
            'm_keepHaltSwSts = False                         ' HALT�L�[���(True=ON, False=OFF) 

            'V4.0.0.0-69                ��������
            '' �\���̂̏�����                          'V4.4.0.0-0  SetCurrentUICulture()�Ɉړ�
            'Call Init_Struct()
            'Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)         ' ����׎擾

            Dim dir As String = String.Empty
            Try
                ' �w��̫��ނ����݂��Ȃ��ꍇ�ɍ쐬����
                '----- V6.1.4.0�L�� -----
                'dir = DATA_DIR_PATH                             'V5.0.0.8�A C:\TRIMDATA\DATA
                'If (False = IO.Directory.Exists(dir)) Then IO.Directory.CreateDirectory(dir)
                'dir = gSysPrm.stLOG.gsLoggingDir
                'If (False = IO.Directory.Exists(dir)) Then IO.Directory.CreateDirectory(dir)

                Dim dirs() As String = New String() {
                    DATA_DIR_PATH,
                    gSysPrm.stLOG.gsLoggingDir,
                    gSysPrm.stDIR.gsTrimFilePath
                }

                For Each dir In dirs
                    If (False = IO.Directory.Exists(dir)) Then IO.Directory.CreateDirectory(dir)
                Next
                '----- V6.1.4.0�L�� -----

            Catch ex As Exception
                ' �������Ȃ��ꍇ����ײ�ނ����݂��Ȃ��ꍇ�Ȃ�
                strMSG = Environment.NewLine &
                    "System.IO.CreateDirectory(""" & dir & """)" & Environment.NewLine & ex.Message
                Throw New Exception(strMSG)
            End Try
            'V4.0.0.0-69                ��������

            'V4.1.0.0�B
            'V6.0.0.0�D            FinalEnd_GazouProc(ObjGazou)

#If START_KEY_SOFT Then
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "START_KEY_SOFT", r)
            If r = 1 Then
                gbStartKeySoft = True
            Else
                gbStartKeySoft = False
            End If
#End If
            ''V5.0.0.9�B      ��
            'If (KEY_TYPE_R = gSysPrm.stTMN.giKeiTyp) Then
            '    ' �N�����v���X�ڕ���(0�łȂ��ꍇ�N�����v���X)
            '    giClampLessStage = Integer.Parse(GetPrivateProfileString_S(
            '                                     "DEVICE_CONST", "CLAMP_LESS_STAGE", SYSPARAMPATH, "0"))

            '    If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then
            '        ' �N�����v���X�ڕ���̏ꍇ�A�N�����v�Ȃ��Ƃ���
            '        gSysPrm.stIOC.giClamp = 0S

            '        'V5.0.0.9�E          ��
            '        gdClampLessOffsetX = Double.Parse(GetPrivateProfileString_S(
            '                                          "DEVICE_CONST", "CLAMP_LESS_OFFSET_X", SYSPARAMPATH, "0.0"))

            '        gdClampLessOffsetY = Double.Parse(GetPrivateProfileString_S(
            '                                          "DEVICE_CONST", "CLAMP_LESS_OFFSET_Y", SYSPARAMPATH, "0.0"))

            '        giClampLessRoughCount = Integer.Parse(GetPrivateProfileString_S(
            '                                              "DEVICE_CONST", "CLAMP_LESS_ROUGH_COUNT", SYSPARAMPATH, "0"))
            '        ''V6.0.2.0�E��
            '        'gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
            '        '                                "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))   'V5.0.0.9�H
            '        ''V6.0.2.0�E��
            '    Else
            '        gdClampLessOffsetX = 0.0
            '        gdClampLessOffsetY = 0.0
            '        giClampLessRoughCount = 0
            '        gdClampLessTheta = 0.0      'V5.0.0.9�H
            '        'V5.0.0.9�E          ��
            '    End If

            '    'V4.12.2.0�E         ��'V6.0.4.1�@
            '    If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then
            '        If (0 <> giClampLessStage) Then
            '            W_Write(LOFS_W192, LPC_CLAMP_EXEC)
            '        Else
            '            W_Write(LOFS_W192, 0)
            '        End If
            '    End If
            '    'V4.12.2.0�E         ��'V6.0.4.1�@

            '    'V6.0.2.0�E��
            '    gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
            '                                        "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))
            '    'V6.0.2.0�E��

            'End If
            ''V5.0.0.9�B                  ��

            ' ���[�_������
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then               ' SL436R�n ?
                '###148 Call SetLoaderIO(LOUT_NO_ALM, Not (LOUT_NO_ALM))        ' ���[�_������(ON=�g���}������, OFF=���L�ȊO) 
                Call SetLoaderIO(0, 0)                                          ' ���[�_������(ON=�g���}������, OFF=���L�ȊO) 
            Else
                Call SetLoaderIO(COM_STS_TRM_STATE, Not (COM_STS_TRM_STATE))    ' ���[�_�[�o��(ON=�g���}���쒆, OFF=���L�ȊO)
            End If

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
            If (giDoorLock = 1) Then                        ' �d�����b�N�L�� ?
                Call EXTOUT1(0, EXTOUT_EX_LOK_ON)
            End If
            '----- V1.18.0.1�G�� -----

            'V4.11.0.0�M
            ' �p���[���j�^�V���b�^�[ON/OFF(0=�V���b�^�A1=�V���b�^�J)
            Call PMON_SHUTCTRL(0)
            'V4.11.0.0�M

            '' �\���̂̏�����
            'Call Init_Struct()         'V4.0.0.0-69    ��Ɉړ�

            ' �z��̏�����
            Call Init_Arrey()

            '-----------------------------------------------------------------------
            '   �g�p����n�b�w�̏����ݒ���s��
            '-----------------------------------------------------------------------
            Call Ocx_Initialize()
            Call InitFunction()
            Call PROP_SET(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)     ' �V�X�e���ϐ������ݒ�(�v���[�uON/OFF�ʒu��)

            '-----------------------------------------------------------------------
            '   �V�X�e���ݒ�t�@�C�����[�h
            '   ���V�X�e���p�����[�^�̑��M��OcxSystem��SetOptionFlg()�ōs��
            '-----------------------------------------------------------------------
            'Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)        'V4.0.0.0-69    ��Ɉړ�
            Call Me.System1.SetMessageConst(gSysPrm)        ' OcxSystem��ү���ނ�ݒ肷��(�������Ұ���ذ�ތ�ōs��)
            Call GetFncDefParameter(gTkyKnd)                ' �@�\�I���`�e�[�u���ݒ�
            '                                               ' Video.ocx�p�I�v�V������`�擾
            Call gDllSysprmSysParam_definst.GetSysprmOptVideo(OptVideoPrm)
            Call Me.System1.SetSysParam(gSysPrm)            ' OcxSystem�p�̃V�X�e���p�����[�^��ݒ肷��

            '' ���b�Z�[�W�����ݒ菈��
            'Call PrepareMessages(gSysPrm.stTMN.giMsgTyp)
            'Call PrepareMessages_N(gSysPrm.stTMN.giMsgTyp)
            'Call PrepareMessagesCutPosCorrect_Calibration(gSysPrm.stTMN.giMsgTyp) 'V1.20.0.0�G

            '----- V1.16.0.2�@�� -----
            ' �d���l��0.5A�̏ꍇ�̓����W1-9�̕␳�l�����[�h������
            If (gSysPrm.stSPF.giProcPower2 = 3) Then             ' �d���l��0.5A ?
                sPath = "C:\TRIM\TKYSYS.ini"
                strSECT = "DRM_KG500MA"
                For i = 1 To 9
                    strKEY = "DRM_KGL_" + Format(i, "000")
                    ' ��gfDrm_kgL().NET��0 ORG����VB6(OcxSYStem)��1 ORG�Ȃ̂Œ��� 
                    ''V4.0.0.0-70                    gSysPrm.stDRM.gfDrm_kgL(i - 1) = Val(GetPrivateProfileString_S(strSECT, strKEY, sPath, "1.0"))
                    gSysPrm.stDRM.gfDrm_kgL(i) = Val(GetPrivateProfileString_S(strSECT, strKEY, sPath, "1.0"))
                    'V4.0.0.0-70
                Next i
            End If
            '----- V1.16.0.2�@�� -----

            ' ���̑��̃V�X�p�����擾����
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "GPIB_ADR", giGpibDefAdder)                    ' GPIB�A�h���X ###002
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "FULLCUT_ZPOS", giFullCutZpos)                 ' �����J�b�g���[�h(x5���[�h)����Z�̈ʒu�w��(0=OFF�ʒu, 1=STEP�ʒu, 2=ON�ʒu) V3.0.0.1�@
            giBuzerOn = Val(GetPrivateProfileString_S("CUSTOMIZE", "TRIMEND_BUZER_ON", SYSPARAMPATH, "0"))          ' V6.1.1.0�@
            giAlmTimeDsp = Val(GetPrivateProfileString_S("CUSTOMIZE", "ALARM_TIME_DISP", SYSPARAMPATH, "0"))        ' V6.1.1.0�L
            giNgCountAss = Val(GetPrivateProfileString_S("CUSTOMIZE", "NGBOX_COUNT_ASS", SYSPARAMPATH, "0"))        ' V6.1.1.0�H
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "DISP_ENDTIME", giDispEndTime)                 ' V6.1.1.0�B
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "ALARM_ONOFF_BTN", giAlarmOnOff)               ' V6.1.1.0�C
            Call Get_SystemParameterShort(SYSPARAMPATH, "LOGGING", "IX2LOG", giIX2LOG)                              ' IX2LOG ###231
            Call Get_SystemParameterShort(SYSPARAMPATH, "LOGGING", "SUMMARY_LOG", giSummary_Log)                    ' V1.22.0.0�C
            Call Get_SystemParameterShort(SYSPARAMPATH, "OPT_VIDEO", "TABLE_POS_UPDATE", giTablePosUpd)             ' ###234
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "TRIM_EXEC_NOWORK", giTrimExe_NoWork)    ' ###240
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "TENKEY_BTN", giTenKey_Btn)              ' ###268
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "BPADJ_HALTMENU", giBpAdj_HALT)          ' ###269
            giProbeCheck = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "PROBE_CHECK", SYSPARAMPATH, "0"))      ' V1.23.0.0�F
            giPltCountMode = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "PLATE_COUNT_MODE", SYSPARAMPATH, "0")) 'V6.0.3.0�@
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "CUTOFF_ADJUST", gAdjustCutoffFunction)  ' V6.0.3.0�F
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "CUTOFF_ADJUST_LOGOUT", giCutOffLogOut)  ' V6.0.3.0�F
            Call Get_SystemParameterShort(SYSPARAMPATH, "TMENU", "KEITYP", giMachineKd)                             ' V1.13.0.0�F
            Call Get_SystemParameterShort(SYSPARAMPATH, "RMCTRL", "AUTOPOWER_LASER_COMMAND", giAutoPwr)             ' V1.14.0.0�A
            Call Get_SystemParameterShort(SYSPARAMPATH, "RMCTRL", "NODSP_ATT_RATE", giNoDspAttRate)                 ' V1.16.0.0�M
            dblCalibHoseiX = Val(GetPrivateProfileString_S("CUSTOMIZE", "CALIBHOSEIX", SYSPARAMPATH, "1.0"))        ' V1.20.0.0�F

            ''V6.0.1.021�@��
            ' �t�@�C�o�[���[�U�ŌŒ�A�b�e�l�[�^ON/OFF�̂ݍs��    
            Call Get_SystemParameterShort(SYSPARAMPATH, "OPT_LASER_TEACH", "FL_FIX_ATT_USE", giFixAttOnly)
            ''V6.0.1.021�@��

            'V5.0.0.9�N                  ��
            Dim bt As BarcodeType
            If ([Enum].TryParse(Of BarcodeType)(
                GetPrivateProfileString_S("CUSTOMIZE", "BARCODE_TYPE", SYSPARAMPATH, "0"), bt)) Then
                ' 0(�Ȃ�), 1(Walsin), 2(���z��), 3(�W���I�v�V����)
                BarCode_Data.Type = bt
            Else
                BarCode_Data.Type = BarcodeType.None
            End If

            If (BarcodeType.None <> BarCode_Data.Type) Then
                For idx As Integer = 1 To 2 Step 1
                    Dim str As String = GetPrivateProfileString_S(
                        "CUSTOMIZE", "BARCODE_SUBSTR" & idx, SYSPARAMPATH, "")
                    If ("" = str) Then Continue For

                    Dim sp() As String = str.Split("-"c)
                    If (2 <> sp.Length) Then Continue For

                    Dim p1, p2 As Integer
                    If (Integer.TryParse(sp(0), p1) AndAlso (0 < p1) AndAlso
                        Integer.TryParse(sp(1), p2) AndAlso (0 < p2)) Then
                        p1 -= 1             ' string.Substring(p1, p2), p1��0�ؼ��
                        BarCode_Data.SubStr.Add(Tuple.Create(p1, p2))
                    End If
                Next idx
            End If
            'V5.0.0.9�N                  ��
            gsComPort = GetPrivateProfileString_S("CUSTOMIZE", "COM", SYSPARAMPATH, "COM6")                         ' V1.23.0.0�@
            strBCConvFileFullPath = GetPrivateProfileString_S("CUSTOMIZE", "BARCODE_FILENAME", SYSPARAMPATH, "C:\TRIMDATA\436R���グ�p�t�@�C��.CSV") ' V1.23.0.0�@
            gdblStg2ndOrgX = Val(GetPrivateProfileString_S("CUSTOMIZE", "STAGE_2NDPOSX", SYSPARAMPATH, "0.0"))      ' V1.13.0.0�G
            gdblStg2ndOrgY = Val(GetPrivateProfileString_S("CUSTOMIZE", "STAGE_2NDPOSY", SYSPARAMPATH, "0.0"))      ' V1.13.0.0�G
            gdblStgOrgMoveX = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "STAGE_ORG_MOVE_POSX", SYSPARAMPATH, "0.0"))    ' V1.13.0.0�I
            gdblStgOrgMoveY = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "STAGE_ORG_MOVE_POSY", SYSPARAMPATH, "0.0"))    ' V1.13.0.0�I
            gsEDIT_DIGITNUM = GetPrivateProfileString_S("CUSTOMIZE", "EDIT_DIGITNUM", SYSPARAMPATH, "0.00000")      ' V1.16.0.0�B
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "AUTOMODE_DATA_SELECT", giAutoModeDataSelect)  ' V1.18.0.0�G
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "AUTOMODE_CONTINUE", giAutoModeContinue)       ' V1.18.0.0�H
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "BTN_AUTOPOWER", giBtnPwrCtrl)                 ' V1.18.0.4�@
            giFLPrm_Ass = Val(GetPrivateProfileString_S("CUSTOMIZE", "FL_PARAM_ASSIGN", SYSPARAMPATH, "0"))         ' V2.0.0.0�D
            gsFLPrmFile = GetPrivateProfileString_S("CUSTOMIZE", "FL_PARAM_NAME", SYSPARAMPATH, "C:\TRIMDATA\DATA\FLParamFile.xml") ' V2.0.0.0�D
            giDspScreenKeybord = Val(GetPrivateProfileString_S("CUSTOMIZE", "DSP_SCREEN_KEYBORD", SYSPARAMPATH, "1")) ' V2.0.0.0�F(V1.22.0.0�G)
            giBtn_EdtLock = Val(GetPrivateProfileString_S("CUSTOMIZE", "BTN_EDITLOCK", SYSPARAMPATH, "0"))          ' V2.0.0.0_25
            giDspCmdName = Val(GetPrivateProfileString_S("CUSTOMIZE", "DSP_CMD_NAME", SYSPARAMPATH, "0"))           ' V1.18.0.1�@
            giDoorLock = Val(GetPrivateProfileString_S("CUSTOMIZE", "DOOR_LOCK", SYSPARAMPATH, "0"))                ' V1.18.0.1�G
            ' ������ V3.1.0.0�A 2014/12/01
            Call Get_SystemParameterShort(SYSPARAMPATH, "MEASUREMENT", "MEASUREMENT", giMeasurement)                ' ������@
            gdRESISTOR_MIN = Val(GetPrivateProfileString_S("MEASUREMENT", "RESISTOR_MIN", SYSPARAMPATH, "0.0"))     ' ��R�ŏ��l
            gdRESISTOR_MAX = Val(GetPrivateProfileString_S("MEASUREMENT", "RESISTOR_MAX", SYSPARAMPATH, "0.0"))     ' ��R�ő�l
            ' ������ V3.1.0.0�A 2014/12/01
            'V5.0.0.9�Q��
            Dim rngDiv As Double = Double.Parse(GetPrivateProfileString_S("MEASUREMENT", "POWER_RESOLUTION", SYSPARAMPATH, "0.0"))
            If rngDiv > 0.0 Then
                Call SETPOWERRESOLUTION(rngDiv)
            End If
            'V5.0.0.9�Q��
            giTeachpointUse = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CUTREVIDE_TEACHPOINT_USE", SYSPARAMPATH, "0"))      'V5.0.0.6�I
            'V5.0.0.6�@��
            If Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CONTROLLER_INTERLOCK", SYSPARAMPATH, "0")) = 1 Then
                gbControllerInterlock = True
            Else
                gbControllerInterlock = False
            End If
            'V5.0.0.6�@��
            'V5.0.0.6�A��
            If gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432 And Val(GetPrivateProfileString_S("SPECIALFUNCTION", "LOADER_SECOND_POS", SYSPARAMPATH, "0")) = 1 Then
                gbLoaderSecondPosition = True
            Else
                gbLoaderSecondPosition = False
            End If
            'V5.0.0.6�A��

            If gKeiTyp = KEY_TYPE_RS Then
                Call Get_SystemParameterShort(SYSPARAMPATH, "DEVICE_VAR", "SIMPLE_CROSSLINEX", CROSS_LINEX)
                Call Get_SystemParameterShort(SYSPARAMPATH, "DEVICE_VAR", "SIMPLE_CROSSLINEY", CROSS_LINEY)
                'V4.8.0.1�@�� �����܂�\����NG���\���ɂ��邩�̐ݒ�ǂݍ���
                Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "RATE_DISP", giRateDisp)
                'V4.8.0.1�@��
                Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "CHANGE_CUTPOINT", giChangePoint) 'V4.9.0.0�A
                ''V4.9.0.0�@��
                Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "NG_STOP", giNgStop)
                If giNgStop <> 0 Then
                    ReadLotStopData()
                End If
                ''V4.9.0.0�@��
            End If

            ' 'V4.5.0.1�@
            giClampSeq = Val(GetPrivateProfileString_S("CUSTOMIZE", "CLAMP_SEQ", SYSPARAMPATH, "0"))     ' ��R�ő�l

            'V4.6.0.0�@
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "MANUAL_CMD_START_SEQ", giManualSeq)
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "TIME_LOT_ONLY", giTimeLotOnly)
            'V4.6.0.0�@

            ' ���JOK �\���E��\��   'V5.0.0.9�P 
            Me.lblDoorOpen.Visible = ("0" <> GetPrivateProfileString_S("CUSTOMIZE", "DOOR_OPEN", SYSPARAMPATH, "0"))

            ' MAP ON/OFF�{�^���g�p����E���Ȃ�     '#4.12.2.0�@
            _mapDisp = ("0" <> GetPrivateProfileString_S("DEVICE_CONST", "MAP_DISP", SYSPARAMPATH, "0"))
            '----- V6.1.1.0�A�� -----
            ' PRINT ON/OFF�{�^����L���ɂ���(0)/�����ɂ���(1)�@
            _PrintDisp = ("0" <> GetPrivateProfileString_S("DEVICE_CONST", "PRINT_NOTDISP", SYSPARAMPATH, "0"))
            '----- V6.1.1.0�A�� -----
            ' �v�����^�I�t���C������ MAP.pdf �ۑ��t�H���_    'V6.0.1.0�K
            _mapPdfDir = GetPrivateProfileString_S("MAP", "PDF_DIR", SYSPARAMPATH, "C:\TRIMDATA\LOG\")

            ' �g���~���O�̔��茋�ʂƐF�E���v�����Ǘ�����         'V6.0.1.0�K
            _trimmingResults = New Dictionary(Of Integer, TrimmingResult) From
            {
                {TRIM_RESULT_OK, New TrimmingResult(TrimMap.ColorOK, "OK")},
                {TRIM_RESULT_FT_HING, New TrimmingResult(TrimMap.ColorNG_FtHigh, "NG-HI")},
                {TRIM_RESULT_FT_LONG, New TrimmingResult(TrimMap.ColorNG_FtLow, "NG-LO")},
                {TRIM_RESULT_OVERRANGE, New TrimmingResult(TrimMap.ColorNG, "OVER-RANGE")},
                {TRIM_RESULT_NOTDO, New TrimmingResult(TrimMap.ColorSelected, "PTN-NG")}
            }
            '{TRIM_RESULT_NOTDO, New TrimmingResult(TrimMap.ColorSelected, "NG")},

            HistoryDataLocation = frmHistoryData.Location               'V6.0.1.0�J
            HistoryDataSize = frmHistoryData.Size                       'V6.0.1.0�J

            '----- V4.0.0.0-40�� -----
            ' �X�e�[�WY�̌��_�ʒu���V�X�p�����烊�[�h����
            strMSG = GetPrivateProfileString_S("DEVICE_CONST", "STAGE_DIRY", SYSPARAMPATH, "0")
            giStageYOrg = Integer.Parse(strMSG)
            If (giStageYOrg = STGY_ORG_DW) Then
                giStageYDir = STGY_DIR_CW
            Else
                giStageYDir = STGY_DIR_CCW
            End If
            '----- V4.0.0.0-40�� -----
            '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
            giTargetOfs = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "TARGET_OFFSET", SYSPARAMPATH, "0"))
            giPwrChkPltNum = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "POWER_CHECK_PLATE_NUM", SYSPARAMPATH, "0"))
            giPwrChkTime = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "POWER_CHECK_TIME", SYSPARAMPATH, "0"))
            giTrimTimeOpt = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "TRIM_TIME_OPT", SYSPARAMPATH, "0"))
            giStartBlkAss = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "START_BLOCK_ASS", SYSPARAMPATH, "0"))
            'V5.0.0.9�O                  ��
            If (0 < giStartBlkAss) AndAlso (KEY_TYPE_R = gKeiTyp) Then
                giStartBlkAss = 2       ' R�� 2
            End If
            'V5.0.0.9�O                  ��
            giSubstrateInvBtn = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "SUBSTRATE_INVEST_BTN", SYSPARAMPATH, "0"))
            '----- V4.11.0.0�@�� -----
            '----- V6.1.4.0�@��(KOA EW�aSL432RD�Ή�) -----
            giLotChange = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "LOT_CHANGE", SYSPARAMPATH, "0"))
            giFileMsgNoDsp = Val(GetPrivateProfileString_S("CUSTOMIZE", "FILE_MSG_NODSP", SYSPARAMPATH, "0"))
            'V6.1.4.0_22��
            ' �p�q�R�[�h���[�h�@�\��TKY-CHIP�̂ݗL��
            If (gTkyKnd = KND_CHIP) And Integer.Parse(GetPrivateProfileString_S("QR_CODE", "QR_CODE_READER_USE", SYSPARAMPATH, "0")) = 1 Then
                gbQRCodeReaderUse = True
            Else
                gbQRCodeReaderUse = False
            End If
            'V6.1.4.10�A��
            ' �p�q�R�[�h���[�h�@�\��TKY-CHIP�̂ݗL��
            If (gTkyKnd = KND_NET) And Integer.Parse(GetPrivateProfileString_S("QR_CODE", "QR_CODE_READER_USE_TKYNET", SYSPARAMPATH, "0")) = 1 Then
                gbQRCodeReaderUseTKYNET = True
            Else
                gbQRCodeReaderUseTKYNET = False
            End If
            'V6.1.4.10�A��
            giQrCodeType = Val(GetPrivateProfileString_S("QR_CODE", "QR_CODE_TYPE", SYSPARAMPATH, "0"))
            Me.LblcLOTNUMBER.Text = ""                                  ' ���Y�Ǘ����(�`�[�m���D) ����
            Me.LblcRESVALUE.Text = ""                                   ' ���Y�Ǘ����(��R�l) �@�@����
            'V6.1.4.0_22��
            'V6.1.4.0_35�����[�U�[�p���[�̃��j�^�����O
            giLaserrPowerMonitoring = Integer.Parse(GetPrivateProfileString_S("ROT_ATT", "FULL_POWER_MONITORING", TKYSYSPARAMPATH, "0"))    '0�F����,1�F�����^�]�J�n��,2�F�G���g���[���b�g��
            gdFullPowerLimit = Double.Parse(GetPrivateProfileString_S("ROT_ATT", "FULL_POWER_LIMIT", TKYSYSPARAMPATH, "0.1"))
            gdFullPowerQrate = Double.Parse(GetPrivateProfileString_S("ROT_ATT", "FULL_POWER_QRATE", TKYSYSPARAMPATH, "10.000"))
            'V6.1.4.0_35��
            '----- V6.1.4.0�@�� -----
            'V6.1.4.2�@��
            giAutoCalibration = Integer.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CALIBRATION", SYSPARAMPATH, "0"))          ' �����L�����u���[�V�����␳���s0:�����A>0�F���s
            gbAutoCalibrationLog = Integer.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CALIBRATION_LOG", SYSPARAMPATH, "0"))   ' �J�b�g�ʒu���ꃍ�O�o�́@0:�����A1�F�o��
            'V6.1.4.2�@��
            If Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CPK_DISP_OFF", SYSPARAMPATH, "0")) = 1 Then  'V5.0.0.4�C
                giCpk_Disp_Off = True
            Else
                giCpk_Disp_Off = False
            End If
            '----- V6.0.3.0_37�� -----
            Call Get_SystemParameterShort(SYSPARAMPATH, "DEVICE_CONST", "VACUM_IO", gVacuumIO)
            strMSG = GetPrivateProfileString_S("DEVICE_CONST", "VACUM_IO", SYSPARAMPATH, "0")
            '----- V6.0.3.0_37�� -----

            ' V4.12.0.0�@��'V6.1.2.0�A
            iInverseStepY = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "INVERSE_STEPY", SYSPARAMPATH, "0")) ' �O���J�����J�b�g�ʒu�摜�o�^�̃u���b�NNoX����璹�Ή��ׂ̈ɂP�ɌŒ肷��B'V1.25.0.0�K'V4.8.0.0�A
            ' V4.12.0.0�@��'V6.1.2.0�A

            ' ���b�g�p�����I�����̑I�����b�Z�[�W��\�����邩�ǂ���
            giReqLotSelect = 0                                          ' V6.0.3.0_38

            ' �N���[�j���O�Ԋu�ݒ�(�g���~���O�f�[�^����ǂݍ��񂾐ݒ�ŏ㏑������)
            gProbeCleaningSpan = 0

            'V6.0.1.0�N��
            '1�����NG���ɂ���Ē�~����@�\�̗L���^����
            giNGCountInPlate = Val(GetPrivateProfileString_S("CUSTOMIZE", "NG_COUNT_IN_PLATE", SYSPARAMPATH, "0"))
            'V6.0.1.0�N��

            ' V4.5.0.5�@ 'V4.12.2.0�H�@'V6.0.5.0�C
            gdblNgStopRate = Val(GetPrivateProfileString_S("CUSTOMIZE", "NG_RATE_IN_PLATE", SYSPARAMPATH, "0.0"))      ' V1.13.0.0�G

            'V4.12.2.2�D��'V6.0.5.0�G
            giGazouClrTime = Val(GetPrivateProfileString_S("CUSTOMIZE", "GAZOU_CLR_TIME", SYSPARAMPATH, "0"))
            'V4.12.2.2�D��'V6.0.5.0�G
            '----- V6.0.3.0_47�� -----
            ' MoveMode���}�E�X�̃z�C�[���œ��삳����̂�L��/����
            giMoveModeWheelDisable = Val(GetPrivateProfileString_S("CUSTOMIZE", "MOVEMODE_MOUSEWHEELDISABLE", SYSPARAMPATH, "0"))
            '----- V6.0.3.0_47�� -----

            'V6.1.2.0�@��
            '�X�e�[�W����̊����҂����s�L���̃p�����[�^�ݒ�[0:�����҂�����A1:����w���̂�]
            giJogWaitMode = Convert.ToInt16(
                GetPrivateProfileString_S("CUSTOMIZE", "STAGEJOG_WAITMODE", SYSPARAMPATH, "0"))
            'V6.1.2.0�@��

            ' �N���[�j���O�Ԋu�ݒ�(�g���~���O�f�[�^����ǂݍ��񂾐ݒ�ŏ㏑������)
            gProbeCleaningSpan = 0

            '----- V1.18.0.0�A�� -----
            ' QR�f�[�^�̃I�t�Z�b�g�ʒu���V�X�p������擾����(���[���a����)
            Call GetSysPrm_QR_DataOfs(SYSPARAMPATH)
            '----- V1.18.0.0�A�� -----
            '----- V1.18.0.0�B�� -----
            If (gSysPrm.stDEV.rPrnOut_flg = True) Then                  ' Print�{�^���L�� ? 
                giPrint = 1
            Else
                giPrint = 0
            End If
            '----- V1.18.0.0�B�� -----

            'V4.10.0.0�I��
            'gSysPrm�́ASetCurrentUICulture�̒��ŁA giMachineKd�́A�����̏�ŃZ�b�g����Ă���B
            ' ���u��ʂ̐ݒ�
            Select Case (gSysPrm.stTMN.gsKeimei)
                Case MACHINE_TYPE_SL432
                    gMachineType = MACHINE_TYPE_432R
                Case MACHINE_TYPE_SL436
                    gMachineType = MACHINE_TYPE_436R
                Case MACHINE_TYPE_SL436S
                    gMachineType = MACHINE_TYPE_436S
                Case Else
                    gMachineType = MACHINE_TYPE_432R
            End Select

            Select Case (giMachineKd)
                Case MACHINE_KD_R
                Case MACHINE_KD_RW
                    gMachineType = MACHINE_TYPE_432RW
                Case MACHINE_KD_RS
                    gMachineType = MACHINE_TYPE_436S
                Case Else
            End Select
            'V4.10.0.0�I��

            '----- V6.0.3.0�S�� -----
            lblCutOff.Visible = False
            '----- V6.0.3.0�S�� -----

            '' --- V4.1.0.0�@��-----------------------------------------------------
            'Dim DspMsg As String
            'Dim strSetFullPath As String

            'strSetFullPath = "C:\TRIMDATA\DATA\"
            'DspMsg = strSetFullPath
            '' "�p�q�R�[�h�ɑΉ������g���~���O�f�[�^������܂���B","�t�@�C�����m�F���Ă��������B","�p�X�\��"
            'r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
            '        MSG_SPRASH48, MSG_SPRASH49, DspMsg, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            '' --- V4.1.0.0�@��-----------------------------------------------------

            ' ���O��ʕ\���N���A��������V�X�p�����擾���� ###013
            If (gAppName = APP_CHIP) Then
                strMSG = "DISP_CLS_CHIP"
            ElseIf (gAppName = APP_NET) Then
                strMSG = "DISP_CLS_NET"
            Else
                strMSG = "DISP_CLS_TKY"
            End If
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", strMSG, gDspClsCount)    ' ���O��ʕ\���N���A�����
            If (gDspClsCount <= 0) Then gDspClsCount = 1
            gDspCounter = 0                                                                         ' ���O��ʕ\��������J�E���^

            ' Shift�L�[�������Ȃ���̕ҏW��ʎ��s�ŏ����l�̃f�[�^��ҏW(�V�K�쐬)����           'V4.12.4.0�A
            _editNewData = ("0" <> (GetPrivateProfileString_S("TMENU", "EDIT_NEW", SYSPARAMPATH, "0")))

            ' EXTOUT LED����r�b�g(BIT4-7)���V�X�p�����ݒ肷�� '###061
            glLedBit = Val(GetPrivateProfileString_S("IO_CONTROL", "ILUM_BIT", SYSPARAMPATH, "16"))
#If SANADA_KOA Then
            Call EXTOUT1(glLedBit, 0)                       ' �o�b�N���C�g�Ɩ��n�m�E���ˏƖ�
#End If
            ' SL436R��CHIP�̏ꍇ��TrimDataEditorEx���g�p���Ȃ�=0,�g�p����=0�łȂ� 'V4.10.0.0�A
            Globals_Renamed.giChipEditEx = Convert.ToInt32(GetPrivateProfileString_S("TMENU", "CHIP_EDITEX", SYSPARAMPATH, "0"))

            ' �J�b�g�ʒu�␳�p�p�^�[���o�^���e�[�u��������(INtime��) ###092
            ' (TKY�ŃJ�b�g�ʒu�␳���s��ACHIP/NET�����s����ƃJ�b�g�ʒu�␳�����s�����ꍇ������)
            Call CUTPOSCOR_ALL(cMAXcRESISTORS, gStCutPosCorrData)

            '-----------------------------------------------------------------------
            '   ���[�_�ʐM�p�����ݒ菈��(SL436R�p)
            '-----------------------------------------------------------------------
            GrpNgBox.Visible = False                                '###149
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                ' SL436R�p�V�X�p�������[�h����
                Call GetSL436RSysparam()

                ' PLC�ʐM��������������
                Call Init_PlcIF()

                ' �V�X�p����胍�[�_�p�e��ݒ�l��ݒ肷��  ��������ǉ�����
                r = Loader_PutParameterFromSysparam()

                ' �V�X�p����胍�[�_�^�C�}�l��ݒ肷��@�@�@��������ǉ�����
                r = Loader_PutTimerValFromSysparam()

                'GrpNgBox.Visible = True                    '###195 ###149
                GrpNgBox.Visible = False                    '###195
            End If

            '----------------------------------------------------------------------------
            '   ���O�C���p�X���[�h�`�F�b�N(�I�v�V����)
            '----------------------------------------------------------------------------
            If (flgLoginPWD = 1) Then                       ' ۸޲��߽ܰ�ޓ��͗L ? 
                r = Password1.ShowDialog((gSysPrm.stTMN.giMsgTyp), (gSysPrm.stSYP.gsLoginPassword))
                '----- V1.18.0.0�@�� -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then  ' ���[���a�d�l ? 
                    If (r = cFRS_ERR_RST) Then                  ' Cancel ? 

                    Else
                        lblLoginResult.Visible = True           ' ���O�C���p�X���[�hOK�Ȃ�uAdministrator Mode�v�ƕ\�� 
                    End If
                Else
                    If (r = cFRS_ERR_RST) Then                  ' Cancel ? 
                        End                                     ' �A�v���I��
                    End If
                End If
                '----- V1.18.0.0�@�� -----
            End If
            Call Me.System1.OperationLogDelete(gSysPrm)        ' �Â����샍�O�t�@�C�����폜����
            Call Me.System1.OperationLogging(gSysPrm, MSG_OPLOG_WAKEUP, gAppName)

            ' �ϐ�������
            gSysPrm.stLOG.giLoggingAppend = 1                   ' ���M���O�͏�ɃA�y���h���[�h

            ' �V�O�i���^���[���䏉����(On=0, Off=�S�ޯĵ�)
            'V5.0.0.9�M �� V6.0.3.0�G
            ' Call Me.System1.SetSignalTower(0, &HFFFF)
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_ALL_OFF)
            'V5.0.0.9�M �� V6.0.3.0�G

            ' BP�L�����u���[�V�����̏����l�ݒ�
            Call BP_CALIBRATION(1.0#, 1.0#, 0.0#, 0.0#)

            ' �␳��N���X���C��X,Y��\��
            'V6.0.0.0�C            Me.CrosLineX.Visible = False
            'V6.0.0.0�C            Me.CrosLineY.Visible = False

            '----- ###232�� ----
            ' �N���X���C���␳�̏�����
            'V6.0.0.0�B            ObjCrossLine.CrossLineParamINitial(Me.Picture2, Me.Picture1, Me.CrosLineX, Me.CrosLineY, 0.0, 0.0)
            ObjCrossLine.CrossLineParamINitial(AddressOf VideoLibrary1.GetCrossLineCenter,
                                               AddressOf VideoLibrary1.SetCorrCrossVisible,
                                               AddressOf VideoLibrary1.SetCorrCrossCenter,
                                               0.0, 0.0)                                'V6.0.0.0�B
            '' �␳��N���X���C���\���p�p�����[�^�����ݒ�
            'gstCLC.k = 0                                    ' ���(0:�W��, 1:����ݽ)
            'gstCLC.x = 0                                    ' X�����̃J�b�g�J�n�ʒu(mm)
            'gstCLC.y = 0                                    ' Y�����̃J�b�g�J�n�ʒu(mm)
            'gstCLC.bpx = 0                                  ' Beem Position X OFFSET(mm)
            'gstCLC.bpy = 0                                  ' Beem Position Y OFFSET(mm)
            'gstCLC.Chk = gSysPrm.stCRL.giDspFlg             ' �␳��̃N���X���C����\��(0:���Ȃ�, 1:����)
            'gstCLC.LineX = Picture2                         ' �N���X���C���w(�c�����w�肷��)
            'gstCLC.LineY = Picture1                         ' �N���X���C���x(�������w�肷��)
            'gstCLC.LineX2 = CrosLineX                       ' �␳��N���X���C���w(�␳��̕\���p�c�����w�肷��)
            'gstCLC.LineY2 = CrosLineY                       ' �␳��N���X���C���x(�␳��̕\���p�������w�肷��)
            ''                                               ' �N���X���C���␳�l�e�[�u��
            ''                                               ' ��SysPrmذ�ގ��񎟌��z��͈ꎟ���z��ɂȂ�̂ňꎟ���z��ŏ�������
            'gstCLC.CorrectTBL = New Double(2 * 17 - 1) { _
            '    gSysPrm.stCRL.gfCrossLineCorrect(0), gSysPrm.stCRL.gfCrossLineCorrect(2), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(4), gSysPrm.stCRL.gfCrossLineCorrect(6), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(8), gSysPrm.stCRL.gfCrossLineCorrect(10), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(12), gSysPrm.stCRL.gfCrossLineCorrect(14), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(16), gSysPrm.stCRL.gfCrossLineCorrect(18), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(20), gSysPrm.stCRL.gfCrossLineCorrect(22), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(24), gSysPrm.stCRL.gfCrossLineCorrect(26), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(28), gSysPrm.stCRL.gfCrossLineCorrect(30), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(32), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(1), gSysPrm.stCRL.gfCrossLineCorrect(3), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(5), gSysPrm.stCRL.gfCrossLineCorrect(7), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(9), gSysPrm.stCRL.gfCrossLineCorrect(11), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(13), gSysPrm.stCRL.gfCrossLineCorrect(15), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(17), gSysPrm.stCRL.gfCrossLineCorrect(19), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(21), gSysPrm.stCRL.gfCrossLineCorrect(23), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(25), gSysPrm.stCRL.gfCrossLineCorrect(27), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(29), gSysPrm.stCRL.gfCrossLineCorrect(31), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(33)}
            '----- ###232 ----

            '----------------------------------------------------------------------------
            '   ��ʕ\�����ڂ�ݒ肷��
            '----------------------------------------------------------------------------
            Me.Text = gAppName                              ' ���ع���ݖ��\�� 
            '----- V6.1.4.0�F��(KOA EW�aSL432RD�Ή�) -----
            ' �g���~���O�f�[�^���\����N���A
            ' LblDataFileName.Text = ""                      ' ���ݸ��ް�̧�ٖ������� 
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = ""                    ' �g���~���O�f�[�^���\����Ƒ�P��R�f�[�^�\����N���A  
            Else
                LblDataFileName.Text = ""                   ' ���ݸ��ް�̧�ٖ������� 
            End If
            '----- V6.1.4.0�F�� -----
            Call Z_CLS()                                    ' ���O��ʃN���A 
            '                                               ' ���O��ʂ�̫�Ļ��ނ���ׂ��ݒ�
            txtLog.Font = New Font(txtLog.Font.FontFamily, CSng(gSysPrm.stLOG.gdLogTextFontSize))
            Call Form1LanguageSet()                         ' �t�H�[���̃��x����ݒ肷��
            'picGraphAccumulation.Visible = False            ' ���z�}�O���t��\��

            ' �N���X���C���\��
            'V6.0.0.0�C            Call SetCrossLine() Form1_Load()���ֈړ�

            ' ۷�ݸޏ��(Logging ON/OFF)�\��
            Call LoggingModeDisp()

            ' ���[�U�p���[�ݒ�l�\�� (RMCTRL2 >=3 �̏ꍇ�ɕ\������) ###029
            ' ����۸��ыN������ڰ�ް��ܰ�ݒ�l�́u-----�v�\���Ƃ���
            If (gSysPrm.stRMC.giRmCtrl2 >= 3) And (gSysPrm.stRMC.giPMonHi = 1) Then
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    strMSG = "���[�U�p���[�ݒ�l�@---- W"
                'Else
                '    strMSG = "Laser Power ---- W"
                'End If
                LblMes.Text = Form1_001                     ' ����p���[[W]�̕\��
                LblMes.Visible = True                       ' �ݒ�l�\��
            Else
                LblMes.Visible = False                      ' �ݒ�l��\��
            End If

            ' ���[�U�p���[�ݒ�l�\��(FL��)
            ' ��FL�Ńp���[���[�^�̃f�[�^�擾�^�C�v���u�h�^�n�ǎ��v/�u�t�r�a�v�Łu�p���[����l�\������v�̏ꍇ�ɕ\������
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And
               (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    strMSG = "���[�U�p���[�ݒ�l�@---- W"
                'Else
                '    strMSG = "Laser Power ---- W"
                'End If
                LblMes.Text = Form1_001                     ' ����p���[[W]�̕\��
                'LblMes.Visible = True                      ' �ݒ�l�\��
                LblMes.Visible = False                      ' �ݒ�l��\�� ###066
            Else
                LblMes.Visible = False                      ' �ݒ�l��\��
            End If

            ' ���Y�Ǘ���񏉊���
            Call SetTrimResultCmnStr()                      ' ���ݸތ���۸ދ��ʕ\�������̏�����
            Call ClearCounter(0)                            ' ���Y�Ǘ��f�[�^�̃N���A(��ʕ\���Ȃ�)

            '----- ###243�� -----
            ' ���Y�Ǘ���ʂ�RESISTOR�\����TKY/NET����CIRCUIT�ɕύX
            If (gTkyKnd = KND_CHIP) Then
                LblcREGNUM.Text = "RESISTOR="
            Else
                LblcREGNUM.Text = "CIRCUIT="
            End If
            '----- ###243�� -----

            ' �C���^�[���b�N��Ԃ̕\��/��\��
            Call DispInterLockSts()

            ' Digi-SW �\��(�������ɕ\������)
            '#If cOFFLINEcDEBUG = 1 Then
            LblDIGSW_HI.Visible = True
            LblDIGSW.Visible = True                         ' "DSW="�@�\��
            CbDigSwH.Visible = True
            CbDigSwL.Visible = True
            CbDigSwH.SelectedIndex = 2                      ' Digi-SW = 33
            CbDigSwL.SelectedIndex = 3
            '#End If

            BtnADJ.Text = "ADJ ON"                          '###009
            BtnADJ.BackColor = System.Drawing.Color.Yellow  '###009
            gbChkboxHalt = True                             '###009

            '-----------------------------------------------------------------------
            '   �I�v�V�����{�^���̕\��/��\��
            '-----------------------------------------------------------------------
            ' �uES۸ށv���ݕ\��/��\��
            If gSysPrm.stLOG.gEsLogUse = 1 Then             ' ES���O�g�p�� ?
                cmdEsLog.Visible = True                     ' ES۸����ݕ\��
            Else
                cmdEsLog.Visible = False                    ' ES۸����ݔ�\��
            End If
            cmdEsLog.Text = "ESLog OFF"
            cmdEsLog.BackColor = System.Drawing.SystemColors.Control

            ' �uIX2Log ON�v/�uIX2Log OFF�v���ݕ\��/��\��
            'If gSysPrm.stCTM.giGP_IB_flg <> 0 Then          ' GP-IB����@�\���� ?  '###231
            ' GP-IB����@�\�����IX2�L�� ?                                          '###231
            'If (gSysPrm.stCTM.giGP_IB_flg <> 0) And (giIX2LOG = 1) Then             V1.18.0.0�E
            If (giIX2LOG = 1) Then                          ' IX2���O���L���Ȃ�GPIB�Ɋ֌W�Ȃ��L���Ƃ��� V1.18.0.0�E
                CMdIX2Log.Visible = True                    '�uIX2Log On/OFF�v���ݕ\��
                If gSysPrm.stDEV.rIX2Log_flg = False Then   ' IX2LOG OFF ?
                    CMdIX2Log.Text = "IX2Log OFF"           '�uIX2Log OFF�v���ݕ\�� 
                    CMdIX2Log.BackColor = System.Drawing.SystemColors.Control
                Else                                        ' IX2LOG ON
                    CMdIX2Log.Text = "IX2Log ON"            '�uIX2Log ON�v���ݕ\�� 
                    CMdIX2Log.BackColor = System.Drawing.Color.Lime
                End If
            End If

            '----- V1.18.0.4�@�� -----
            ' Power Ctrl ON/OFF�{�^����1(�\������), 0(�\�����Ȃ�) ���[���a����
            If (giBtnPwrCtrl = 0) Then
                BtnPowerOnOff.Visible = False
            Else
                BtnPowerOnOff.Visible = True
            End If
            '----- V1.18.0.4�@�� -----

            '----- V6.1.1.0�C�� -----
            ' Alarm ON/OFF�{�^����1(�\������), 0(�\�����Ȃ�) IAM�a����(SL436R��)
            Me.System1.giAlarmBuzzer = 1                    ' �A���[������炷(����l��ݒ肷��)
            If (giAlarmOnOff = 0) Then
                BtnAlarmOnOff.Visible = False
            Else
                BtnAlarmOnOff.Visible = True
            End If
            If (gMachineType <> MACHINE_TYPE_436R) Then     ' SL436R ?
                BtnAlarmOnOff.Visible = False
            End If
            '----- V6.1.1.0�C�� -----

            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X������������
            Call Init_StartBlkComb()
            GrpStartBlk.Enabled = False                     ' �f�[�^���[�h�ςłȂ��̂Ŕ񊈐��� 
            ' �\���ʒu��ݒ肷��
            '            GrpStartBlk.Location = New System.Drawing.Point(BtnPowerOnOff.Location.X + 132, BtnPowerOnOff.Location.Y + BtnPowerOnOff.Height + 18)
            'V5.0.0.9�O            GrpStartBlk.Location = New System.Drawing.Point(BtnPowerOnOff.Location.X + 120, BtnPowerOnOff.Location.Y + BtnPowerOnOff.Height + 18)
            ' �g���~���O�J�n�u���b�N�ԍ��̕\��/��\����ݒ肷��
            If (giStartBlkAss = 0) Then                     ' �g���~���O�J�n�u���b�N�ԍ��w��̗L��/����(0=����, 1=�L��)�@
                GrpStartBlk.Visible = False
            Else
                GrpStartBlk.Visible = True

                'V5.0.0.9�O              ��
                If (1 < giStartBlkAss) Then
                    chkContinue.Visible = True              ' �p���`�F�b�N�{�b�N�X�\����Ԑݒ�
                    lblStartBlkX.Visible = True             ' X
                    lblStartBlkY.Visible = True             ' Y
                    CbStartBlkY.Visible = True              ' �R���{�{�b�N�XY

                    If (KEY_TYPE_RS = gKeiTyp) Then
                        btnPREV.UseVisualStyleBackColor = False
                        btnPREV.BackColor = Color.FromArgb(192, 255, 255)
                        btnPREV.Text = "Prev"

                        btnNEXT.UseVisualStyleBackColor = False
                        btnNEXT.BackColor = Color.FromArgb(255, 255, 128)
                        btnNEXT.Text = "Next"
                    Else
                        btnPREV.Enabled = False
                        btnPREV.Visible = False
                        btnNEXT.Enabled = False
                        btnNEXT.Visible = False
                    End If

                    With GrpStartBlk
                        .Location = New Point(
                            (LblCur.Left - .Margin.Right - (.Width - btnPREV.Width - btnNEXT.Width)),
                            0)
                    End With
                End If
                'V5.0.0.9�O              ��
            End If
            '----- V4.11.0.0�D�� -----

            PanelMap.Enabled = _mapDisp                                 ' V6.0.1.0�K
            PanelMap.Visible = _mapDisp                                 ' MAP ON/OFF�{�^����ݒ肷��    '#4.12.2.0�@
            CmdPrintMap.Visible = Not _PrintDisp                        ' PRINT ON/OFF�{�^�� V6.1.1.0�A
            'V6.0.1.0�K          ��
            If (_mapDisp) Then
                PanelMap.Location = New Point(txtLog.Left, txtLog.Top - PanelMap.Height)
            Else
                CmdPrintMap.Enabled = False
                CmdMapOnOff.Enabled = False
            End If
            'V6.0.1.0�K          ��
            SetBtnUserLogon()           ' ���O�I�����[�U�[�\���E�ؑփ{�^����ݒ肷��     V4.10.0.0�@

            '---------------------------------------------------------------------------
            '   �N����̍ŏ��̌��o��۰�ގ���Ӱ��/���쒆�̏ꍇ�́A��~�ɐؑւ���悤�m�F����(SL432R�n)
            '---------------------------------------------------------------------------
            ''----- V6.1.4.0_45��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
            'If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ?
            '    If (giLotChange = 1) Then                               ' ���b�g�ؑւ��@�\�L�� ?
            '        Call Me.System1.Z_ATLDSET(0, &HFFFF)                ' �S��OFF�Ƃ���
            '    End If
            'End If
            ''----- V6.1.4.0_45�� -----

            ' ���[�_����
            giHostMode = cHOSTcMODEcMANUAL                  ' ۰��Ӱ�� = �蓮Ӱ��
            gbHostConnected = False                         ' �z�X�g�ڑ���� = ���ڑ�(۰�ޖ�)
            giHostRun = 0                                   ' ۰�ޒ�~��
            Call Me.System1.ReadHostCommand(gSysPrm, giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag)

            ' �N����۰�ގ���Ӱ��/���쒆����(SL432R�n)
            r = Me.System1.Form_Reset(cGMODE_LDR_CHK, gSysPrm, giAppMode, gbInitialized, 0, 0, gLoadDTFlag)
            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then                             ' �G���[
                strMSG = "i-TKY.Form_Initialize()::Form_Reset(cGMODE_LDR_CHK) error." + vbCrLf _
                        + " Err Code = " + r.ToString
                MsgBox(strMSG)
                End                                         ' �A�v���I��
            ElseIf (r = cFRS_ERR_RST) Then
                '�A�v���P�[�V�����I��
                End
            End If

            '���M���O�o�͑Ώۃf�[�^�̎擾
            '   ���O�t�@�C���o��
            Call TrimLogging_SetLogTarget(m_TrimlogFileFormat, cTRIMLOGcSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_MeaslogFileFormat, cMEASLOGcSECTNAME, Me.Utility1)
            '   ���O�\���Ώ�
            Call TrimLogging_SetLogTarget(m_TrimDisp1xFormat, cTRIMDISP1xcSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_TrimDispx0Format, cTRIMDISPx0cSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_TrimDispx1Format, cTRIMDISPx1cSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_MeasDispFormat, cMEASDISPcSECTNAME, Me.Utility1)
            Call ClearAvgDevCount()                     ' ###154 

            ' ��ʕ\�����O����������       '#4.12.2.0�C
            If (False = Integer.TryParse(GetPrivateProfileString_S(
                                         "SPECIALFUNCTION", "DISP_CLS_LEN", TKY_INI, "5900000"),
                                         _txtLogClearLength)) Then
                _txtLogClearLength = 5900000                            ' .NET���������R�[�hUTF16��11.25MB���x
            End If
            '----- V6.1.4.0�B��(KOA EW�aSL432RD�Ή�) -----
            ' ���O�\����̃T�C�Y�ƈʒu��ύX
            If (gSysPrm.stLOG.giLoggingType2 = LogType2.Reg_KoaEw) Then
                Me.txtLog.Size = New System.Drawing.Size(TXTLOG_SIZEX_KOAEW, TXTLOG_SIZEY_KOAEW)
                Me.txtLog.Location = New Point(TXTLOG_LOCATIONX_KOAEW, TXTLOG_LOCATIONY_KOAEW)
                Me.GrpMode.Location = New Point(GRPMODE_LOCATIONX_KOAEW, GRPMODE_LOCATIONY_KOAEW)
                Me.tabCmd.Location = New Point(TABCMD_LOCATIONX_KOAEW, TABCMD_LOCATIONY_KOAEW)
                Me.CmdEnd.Location = New Point(CMDEND_LOCATIONX_KOAEW, CMDEND_LOCATIONY_KOAEW)
            End If
            '----- V6.1.4.0�B�� -----
            'V6.1.4.0_50��
            ' ENTRYLOT ̫��ޓ���̧�ق����ׂč폜����
            If System.IO.Directory.Exists(ENTRY_PATH) Then
                For Each tmpFile As String In (System.IO.Directory.GetFiles(ENTRY_PATH))
                    IO.File.Delete(tmpFile)
                Next
            End If
            'V6.1.4.0_50��
            ''---------------------------------------------------------------------------
            ''���u����������()
            ''---------------------------------------------------------------------------
            'Call Me.Initialize_VideoLib()
            'Call Me.Initialize_TrimMachine()
            'V5.0.0.9�B      ��
            If (KEY_TYPE_R = gSysPrm.stTMN.giKeiTyp) Then
                ' �N�����v���X�ڕ���(0�łȂ��ꍇ�N�����v���X)
                giClampLessStage = Integer.Parse(GetPrivateProfileString_S(
                                                 "DEVICE_CONST", "CLAMP_LESS_STAGE", SYSPARAMPATH, "0"))

                giClampLessOutPos = Integer.Parse(GetPrivateProfileString_S(
                                                 "DEVICE_CONST", "CLAMP_LESS_OUTPOS", SYSPARAMPATH, "0"))


                If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then
                    ' �N�����v���X�ڕ���̏ꍇ�A�N�����v�Ȃ��Ƃ���
                    'V6.0.5.0�B  gSysPrm.stIOC.giClamp = 0S

                    'V5.0.0.9�E          ��
                    gdClampLessOffsetX = Double.Parse(GetPrivateProfileString_S(
                                                      "DEVICE_CONST", "CLAMP_LESS_OFFSET_X", SYSPARAMPATH, "0.0"))

                    gdClampLessOffsetY = Double.Parse(GetPrivateProfileString_S(
                                                      "DEVICE_CONST", "CLAMP_LESS_OFFSET_Y", SYSPARAMPATH, "0.0"))

                    giClampLessRoughCount = Integer.Parse(GetPrivateProfileString_S(
                                                          "DEVICE_CONST", "CLAMP_LESS_ROUGH_COUNT", SYSPARAMPATH, "0"))
                    ''V6.0.2.0�E��
                    'gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
                    '                                "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))   'V5.0.0.9�H
                    ''V6.0.2.0�E��
                Else
                    gdClampLessOffsetX = 0.0
                    gdClampLessOffsetY = 0.0
                    giClampLessRoughCount = 0
                    gdClampLessTheta = 0.0      'V5.0.0.9�H
                    'V5.0.0.9�E          ��
                End If

                'V4.12.2.0�E         ��'V6.0.4.1�@
                If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then
                    If (0 <> giClampLessStage) Then
                        W_Write(LOFS_W192, LPC_CLAMP_EXEC)
                    Else
                        W_Write(LOFS_W192, 0)
                    End If
                End If
                'V4.12.2.0�E         ��'V6.0.4.1�@

                'V6.0.2.0�E��
                gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
                                                    "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))
                'V6.0.2.0�E��

            End If
            'V5.0.0.9�B                  ��



            '' V3.0.0.0�B MV10�̃{�[�h�̃r�f�I�[�q��ύX�\�Ƃ���B��               'V6.0.0.0�H �J�����������̌�Ɉړ�
            'Dim TkyIni As New TkyIni()
            'INTERNAL_CAMERA = TkyIni.OPT_VIDEO.INTERNAL_CAMERA_PORT.Get(Of Integer)()
            'EXTERNAL_CAMERA = TkyIni.OPT_VIDEO.EXTERNAL_CAMERA_PORT.Get(Of Integer)()
            'If EXTERNAL_CAMERA = 0 Then
            '    INTERNAL_CAMERA = 0
            '    EXTERNAL_CAMERA = 1
            'End If
            '' V3.0.0.0�B MV10�̃{�[�h�̃r�f�I�[�q��ύX�\�Ƃ���B��

            ' V6.1.4.0�I ���b�g�ؑւ��p�����^�]�t�H�[���I�u�W�F�N�g����(File_Read()�Ŏg�p����̂ŕW���łł���������)
            frmAutoObj = New FormDataSelect2(Me)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Form_Initialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "SL436R�p�̃V�X�e���p�����[�^�����[�h����"
    '''=========================================================================
    '''<summary>SL436R�p�̃V�X�e���p�����[�^�����[�h����</summary>
    '''<remarks>�z����g�p���Ă���\���̂̃C���X�^���X������������ɂ�"Initialize"���Ăяo���Ȃ���΂Ȃ�Ȃ�</remarks>
    '''=========================================================================
    Private Sub GetSL436RSysparam()

        Dim i As Integer
        Dim strKEY As String
        Dim strSEC As String
        Dim strMSG As String

        Try
            ' SL436R�p�V�X�p�������[�h����
            giActMode = Val(GetPrivateProfileString_S("DEVICE_CONST", "AUTOMODE", SYSPARAMPATH, "2"))                     ' �A���^�]���샂�[�h(0:϶޼��Ӱ�� 1:ۯ�Ӱ�� 2:����ڽӰ��)
            giOPLDTimeOutFlg = Val(GetPrivateProfileString_S("DEVICE_CONST", "LOADER_TIMEOUT_CHECK", SYSPARAMPATH, "1"))  ' ���[�_�ʐM�^�C���A�E�g���o(0=���o����, 1=���o����)
            giOPLDTimeOut = Val(GetPrivateProfileString_S("DEVICE_CONST", "LOADER_TIMEOUT", SYSPARAMPATH, "180000"))      ' ���[�_�ʐM�^�C���A�E�g����(msec)
            giOPVacFlg = Val(GetPrivateProfileString_S("DEVICE_CONST", "VACUME_CHECK", SYSPARAMPATH, "0"))                ' �蓮���[�h���̍ڕ���z���A���[�����o(0=���o����, 1=���o����)
            giOPVacTimeOut = Val(GetPrivateProfileString_S("DEVICE_CONST", "VACUME_TIMEOUT", SYSPARAMPATH, "3000"))       ' �蓮���[�h���̍ڕ���z���A���[���^�C���A�E�g����(msec)

            strKEY = "LOADER_SPEED"
            giLoaderSpeed = Val(GetPrivateProfileString_S("LOADER", strKEY, SYSPARAMPATH, "0"))                           ' ���[�_�������x
            strKEY = "LOADER_SETTING_NO"
            giLoaderPositionSetting = Val(GetPrivateProfileString_S("LOADER", strKEY, SYSPARAMPATH, "0"))                 ' ���[�_�ʒu�ݒ�I��ԍ�

            File.ConvertFileEncoding(LOADER_PARAMPATH)  ' LOADER.INI�̕������ނ�Shift_JIS����Unicode(UTF16LE BOM�L)�ɕϊ����� 'V4.4.0.0-1
            'V5.0.0.4�@��
            If Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CYCLE_STOP", SYSPARAMPATH, "0")) = 1 Then
                gbCycleStop = True
                Me.btnCycleStop.Enabled = True
                Me.btnCycleStop.Visible = True
            Else
                gbCycleStop = False
                Me.btnCycleStop.Enabled = False
                Me.btnCycleStop.Visible = False
            End If
            'V5.0.0.4�@��

            ' ���[�_��e�[�u���r�o�ʒuXY/�����ʒuXY��TKT.ini�łȂ�LOADER.ini����ݒ肷�� ###069
            strSEC = "SYSTEM"
            For i = 0 To (MAXWORK_KND - 1)
                strKEY = "EJECTPOS" + (i + 1).ToString("0") + "X"
                gfBordTableOutPosX(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))          ' ���[�_��e�[�u���r�o�ʒuX
                strKEY = "EJECTPOS" + (i + 1).ToString("0") + "Y"
                gfBordTableOutPosY(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))          ' ���[�_��e�[�u���r�o�ʒuY

                strKEY = "INSERTPOS" + (i + 1).ToString("0") + "X"
                gfBordTableInPosX(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))           ' ���[�_��e�[�u�������ʒuX
                strKEY = "INSERTPOS" + (i + 1).ToString("0") + "Y"
                gfBordTableInPosY(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))           ' ���[�_��e�[�u�������ʒuY
                '----- V4.0.0.0-26�� -----
                strKEY = "S_THICKNESS_" + (i + 1).ToString("00") 'V4.0.0.0-59
                gfTwoSubPickChkPos(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))          ' �񖇎��Z���T�m�F�ʒu���W(pls)
                strKEY = "S_THINFILM_" + (i + 1).ToString("00")
                glSubstrateType(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0"))                  ' ����Ή�(0=�ʏ�, 1=�����(�X���[�z�[��))
                '----- V4.0.0.0-26�� -----
            Next

            '  NG�r�oBOX�̎��[����(��i�핪)��LOADER.ini����ݒ肷�� ###089
            strSEC = "SYSTEM"
            For i = 0 To (MAXWORK_KND - 1)
                strKEY = "NGBOX_COUNT_" + (i + 1).ToString("00")
                giNgBoxCount(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "10"))
            Next

            '----- V4.0.0.0�E�� -----
            '-------------------------------------------------------------------------------
            '   SL436R��SL436S�̓d�����b�N����IO�A�h���X�̓���(���[���a�Ή�)
            '   �d�w�s�a�h�s(216A BIT4-7) �����[���a����(SL436R/SL436S)
            '-------------------------------------------------------------------------------
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ? 
                '-----�ySL436S�̏ꍇ�z��V4.0.0.0-52(SL436R�Ɠ���)-----
                ' �o�̓r�b�g
                EXTOUT_EX_YLW_ON = &H10                                 ' B4 : ���_��(���[�U�Ǝ˒�)
                EXTOUT_EX_LOK_ON = &H20                                 ' B5 : �d�����b�N(�ω����E�����b�N)

                ' ���̓r�b�g
                EXTINP_EX_LOK_ON = &H10                                 ' B4 : �d�����b�N(�ω����E�����b�N)

                ' ���̑�
                EX_LOK_STS = &H216A                                     ' �d�����b�N�X�e�[�^�X�A�h���X(SL436R�̏ꍇ)

            Else
                '-----�ySL436R�̏ꍇ�z-----
                ' �o�̓r�b�g
                EXTOUT_EX_YLW_ON = &H10                                 ' B4 : ���_��(���[�U�Ǝ˒�)
                EXTOUT_EX_LOK_ON = &H20                                 ' B5 : �d�����b�N(�ω����E�����b�N)

                ' ���̓r�b�g
                EXTINP_EX_LOK_ON = &H10                                 ' B4 : �d�����b�N(�ω����E�����b�N)

                ' ���̑�
                EX_LOK_STS = &H216A                                     ' �d�����b�N�X�e�[�^�X�A�h���X(SL436R�̏ꍇ)
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.GetSL436RSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

#End Region
    '----- V1.14.0.0�A�� -----
#Region "FL�p�p���[�������t�@�C�������[�h����"
    '''=========================================================================
    ''' <summary>FL�p�p���[�������t�@�C�������[�h����</summary>
    ''' <param name="sPath">(INP)�t�@�C���p�X��</param>
    ''' <param name="stPWR">(OUT)FL�p�p���[�������</param>
    ''' <param name="Md">   (INP)0=������, 0�ȊO=���[�h</param>
    '''=========================================================================
    Private Sub GetFlAttInfoData(ByRef sPath As String, ByRef stPWR As POWER_ADJUST_INFO, ByVal Md As Integer)

        Dim Idx As Integer
        Dim strKEY As String
        Dim strSEC As String
        Dim strMSG As String

        Try
            ' FL�p�p���[���������������܂��͐ݒ肷��
            If (Md = 0) Then

                ' FL�p�p���[������������������
                For Idx = 0 To MAX_BANK_NUM - 1
                    stPWR.AttInfoAry(Idx) = 0                           ' �Œ�ATT���z�������������(0(�Œ�ATT Off))
                    stPWR.CndNumAry(Idx) = 0                            ' ���H�����ԍ��z�������������(0=����, 1=�L��) 
                    stPWR.AdjustTargetAry(Idx) = 0.0                    ' �ڕW�p���[�z�������������(0.000W)
                    stPWR.AdjustLevelAry(Idx) = 0.0                     ' ���e�͈͔z�������������(0.000W)
                Next Idx

            Else
                ' �Œ�ATT���(0:�Œ�ATT Off, 1:�Œ�ATT On)�����[�h����
                strSEC = "FIXATT"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.AttInfoAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0"))
                Next Idx

                ' ���H�����ԍ��z��(0=����, 1=�L��)�����[�h����
                strSEC = "COND_NUM"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.CndNumAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0"))
                Next Idx

                ' �ڕW�p���[�z��(W)�����[�h����
                strSEC = "TARGET"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.AdjustTargetAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0.000"))
                Next Idx

                ' ���e�͈͔z��(�{�|W)�����[�h����
                strSEC = "HILO"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.AdjustLevelAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0.000"))
                Next Idx

            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.GetFlAttInfoData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "FL�p�p���[�������t�@�C�������C�g����"
    '''=========================================================================
    ''' <summary>FL�p�p���[�������t�@�C�������C�g����</summary>
    ''' <param name="sPath">(INP)�t�@�C���p�X��</param>
    ''' <param name="stPWR">(OUT)�p���[�������</param>
    '''=========================================================================
    Private Sub PutFlAttInfoData(ByRef sPath As String, ByRef stPWR As POWER_ADJUST_INFO)

        Dim Idx As Integer
        Dim strKEY As String
        Dim strSEC As String
        Dim strDAT As String
        Dim strMSG As String

        Try
            ' �Œ�ATT���(0:�Œ�ATT Off, 1:�Œ�ATT On)�����C�g����
            strDAT = sPath
            strSEC = "FIXATT"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.AttInfoAry(Idx).ToString("0")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' ���H�����ԍ��z��(0=����, 1=�L��)�����C�g����
            strSEC = "COND_NUM"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.CndNumAry(Idx).ToString("0")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' �ڕW�p���[�z��(W)�����C�g����
            strSEC = "TARGET"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.AdjustTargetAry(Idx).ToString("0.000")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' ���e�͈͔z��(�{�|W)�����C�g����
            strSEC = "HILO"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.AdjustLevelAry(Idx).ToString("0.000")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.PutFlAttInfoData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.14.0.0�A�� -----
#Region "�\���̂̏�����"
    '''=========================================================================
    '''<summary>�\���̂̏�����</summary>
    '''<remarks>�z����g�p���Ă���\���̂̃C���X�^���X������������ɂ�"Initialize"���Ăяo���Ȃ���΂Ȃ�Ȃ�</remarks>
    '''=========================================================================
    Private Sub Init_Struct()

        'V5.0.0.8�@        Dim i As Integer
        Dim strMSG As String

        Try
            'V5.0.0.8�@                  ��
            '' �g���~���O�f�[�^�\���̂̏�����
            'For i = 0 To MaxCntResist
            '    typResistorInfoArray(i).Initialize()    ' ����ް�
            '    markResistorInfoArray(i).Initialize()   ' ����ް�(NG Marking) 
            'Next i
            'V5.0.0.8�@                  ��

            ' �g���~���O�f�[�^�̍ŏ��l/�ő�l�`�F�b�N�p�\���̂̏�����
            typSPInputArea.Initialize()                 ' STEP  (0)Min/(1)Max
            typGPInputArea.Initialize()                 ' GROUP (0)Min/(1)Max
            typTy2InputArea.Initialize()                ' Ty2   (0)Min/(1)Max
            typRPInputArea.Initialize()                 ' RES.  (0)Min/(1)Max
            typCPInputArea.Initialize()                 ' CUT   (0)Min/(1)Max

            ' �p���[�������\����(FL�p)������ 
            stCND.Initialize()
            stPWR_LSR.Initialize()                      ' FL�p�p���[�������(���[�U�R�}���h�p) V1.14.0.0�A 

            ' �g���}�[���H�����\����(FL�p)������ ###066
            stPWR.Initialize()

            ' �J�b�g�ʒu�␳�p�\���̏�����
            gStCutPosCorrData.Initialize()

            '----- V1.22.0.0�C�� -----
            ' �T�}���[���M���O�p�f�[�^�̏�����
            stSummaryLog.Initialize()
            '----- V1.22.0.0�C�� -----

            ' �J�b�g�f�[�^����M�p�f�[�^
            stTGPI.Initialize()                         ' ###002 
            stTGPI2.Initialize()                        ' ###229
            stTCUT.Initialize()                         ' �J�b�g�f�[�^�\���̏�����
            stCutMK.Initialize()

            gSysPrm.Initialize()

            '----- ###188�� -----
            ' ���T�u�X�e�[�^�X�A�h���X�ݒ� -----
            AxisSubStsAry(AXIS_X) = ADRSUB_STS_X
            AxisSubStsAry(AXIS_Y) = ADRSUB_STS_Y
            AxisSubStsAry(AXIS_Z) = ADRSUB_STS_Z
            AxisSubStsAry(AXIS_T) = ADRSUB_STS_T
            '----- ###188�� -----

            '----- V4.0.0.0-58�� -----
            ' �g���}�[���H�����\����(���[�N) 
            gwkCND = Nothing
            gwkCND.Initialize()
            '----- V4.0.0.0-58�� -----
            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Init_Struct() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "�z��̏�����"
    '''=========================================================================
    '''<summary>�z��̏�����</summary>
    '''<remarks>Vs2010����VB6.xxxAry�͎g�p�ł��Ȃ�</remarks>
    '''=========================================================================
    Private Sub Init_Arrey()

        Dim strMSG As String

        Try
            ' ���Y�Ǘ����p���x���z��̏�����
            Fram1LblAry(FRAM1_ARY_GO) = Me.LblGO
            Fram1LblAry(FRAM1_ARY_NG) = Me.LblTNG
            Fram1LblAry(FRAM1_ARY_NGPER) = Me.LblNGPER
            Fram1LblAry(FRAM1_ARY_PLTNUM) = Me.LblPLTNUM
            Fram1LblAry(FRAM1_ARY_REGNUM) = Me.LblREGNUM
            Fram1LblAry(FRAM1_ARY_ITHING) = Me.LblITHING
            Fram1LblAry(FRAM1_ARY_FTHING) = Me.LblFTHING
            Fram1LblAry(FRAM1_ARY_ITLONG) = Me.LblITLONG
            Fram1LblAry(FRAM1_ARY_FTLONG) = Me.LblFTLONG
            Fram1LblAry(FRAM1_ARY_OVER) = Me.LblOVER
            Fram1LblAry(FRAM1_ARY_ITHINGP) = Me.LblITHINGP
            Fram1LblAry(FRAM1_ARY_FTHINGP) = Me.LblFTHINGP
            Fram1LblAry(FRAM1_ARY_ITLONGP) = Me.LblITLONGP
            Fram1LblAry(FRAM1_ARY_FTLONGP) = Me.LblFTLONGP
            Fram1LblAry(FRAM1_ARY_OVERP) = Me.LblOVERP


            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Init_Arrey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "�n�b�w�̏����ݒ�"
    '''=========================================================================
    '''<summary>�g�p����n�b�w�̏����ݒ���s��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Ocx_Initialize()

        Dim strMSG As String

        Try
            Dim r As Short
            Dim OptTbl(cMAXOptFlgNUM) As Short              ' ���߲ٵ�߼��(�ő吔)

            '-------------------------------------------------------------------
            '   OCX(VB6)�p�I�u�W�F�N�g��ݒ肷��
            '-------------------------------------------------------------------
            'ObjSys = System1                                ' OcxSystem.ocx
            'ObjUtl = Utility1                               ' OcxUtility.ocx
            '' '' ''ObjHlp = HelpVersion1                           ' (���g�p)�Ȃ��HOcxAbout.ocx
            'ObjPas = Password1                              ' OcxPassword.ocx
            '' '' ''ObjMTC = ManualTeach1                           ' (���g�p)OcxManualTeach.ocx
            'ObjTch = Teaching1                              ' Teach.ocx
            'ObjPrb = Probe1                                 ' Probe.ocx
            'ObjVdo = VideoLibrary1                          ' Video.ocx
            'ObjPrt = Print1                                ' OcxPrint.ocx

            '-------------------------------------------------------------------
            '   OcxSystem.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            ' OcxSystem�p�̃I�u�W�F�N�g��ݒ肷��
            Call Me.System1.SetOcxUtilityObject(Utility1)   ' OcxUtility1.ocx
            Call Me.System1.SetMainObject_EX()              ' Main��޼ު��(Dummy)
            Call Me.System1.SetSystemObject(System1)        ' System.ocx

            ' �e���W���[���̃��\�b�h��ݒ肷��(OcxSystem�p) ' ###061
            gparModules = New MainModules()                 ' �e�����\�b�h�ďo���I�u�W�F�N�g
            Call System1.SetMainObject(gparModules)
            'V6.0.1.0�E            System1.SetActiveJogMethod = AddressOf Me.SetActiveJogMethod        'V6.0.0.0�M

            ' ���߲ٵ�߼�݂�ݒ肷��
#If cOFFLINEcDEBUG = 0 Then                                 ' ���ޯ��Ӱ�ނłȂ� ?
            OptTbl(0) = 0                                       ' ���ޯ���׸�OFF
            Call DebugMode(0, 0)                                ' DllTrimFunc.dll�ޯ���׸�OFF
#Else
            OptTbl(0) = 1                                   ' ���ޯ���׸�ON
            Call DebugMode(1, 0)                            ' DllTrimFunc.dll�ޯ���׸�ON
#End If

#If cIOcMONITORcENABLED = 0 Then                            ' OcxSystem�p 
            OptTbl(1) = 0                                   ' IO����\��(0=�\�����Ȃ�, 1=�\������)
#Else
        OptTbl(1) = 1
#End If

            ' ���߲ٵ�߼�݂�ݒ肵�V�X�e���p�����[�^��INtime���֑��M����
            r = Me.System1.SetOptionFlg(cMAXOptFlgNUM, OptTbl)
            If (r <> cFRS_NORMAL) Then
                strMSG = "System1.SetOptionFlg Error (r = " & r.ToString("0") & ")"
                Call MsgBox(strMSG, MsgBoxStyle.OkOnly)
                End
            End If

            '-------------------------------------------------------------------
            '   OcxAbout.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            Call Me.HelpVersion1.SetOcxUtilityObject(Utility1) ' OcxUtility.ocx

            '-------------------------------------------------------------------
            '   OcxPassword.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            Call Me.Password1.SetOcxUtilityObject(Utility1)    ' OcxUtility.ocx

            '-------------------------------------------------------------------
            '   OcxManualTeach.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            Call Me.ManualTeach1.SetOcxUtilityObject(Utility1) ' OcxUtility1.ocx
            Call Me.ManualTeach1.SetSystemObject(System1)      ' System.ocx

            '-------------------------------------------------------------------
            '   DllSysprm.dll�p�̏����ݒ菈�����s��   
            '-------------------------------------------------------------------
            Call gDllSysprmSysParam_definst.SetOcxUtilityObjectForSysprm(Utility1)

            '-------------------------------------------------------------------
            '   Teach.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            Call Me.Teaching1.SetOcxUtilityObject(Utility1)     ' OcxUtility1.ocx
            Call Me.Teaching1.SetSystemObject(System1)          ' System.ocx
            'Call Me.Teaching1.SetCrossLineObject(gparModules)   ' �N���X���C���\���p ###232

            '-------------------------------------------------------------------
            '   Probe.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            Call Me.Probe1.SetOcxUtilityObject(Utility1)       ' OcxUtility1.ocx
            Call Me.Probe1.SetSystemObject(System1)            ' System.ocx

            '-------------------------------------------------------------------
            '   Video.ocx�p�̏����ݒ菈�����s��
            '-------------------------------------------------------------------
            Call Me.VideoLibrary1.SetOcxUtilityObject(Utility1) ' OcxUtility1.ocx
            Call Me.VideoLibrary1.SetSystemObject(System1)      ' System.ocx

            '----- V4.3.0.0�A�� -----
            '-------------------------------------------------------------------
            '   �N���X���C�u�������̃I�u�W�F�N�g��ݒ肷��
            '-------------------------------------------------------------------
            TrimData = New TrimClassLibrary.TrimData()

            '----- V4.3.0.0�A�� -----

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Ocx_Initialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�@�\�I���`�e�[�u���ݒ�"
    '''=========================================================================
    ''' <summary>�@�\�I���`�e�[�u���ݒ�</summary>
    ''' <param name="Knd">(INP)���(0=TKY, 1=CHIP, 2=NET)</param>
    '''=========================================================================
    Private Sub GetFncDefParameter(ByVal Knd As Short)

        Dim strMSG As String

        Try
            Dim i As Short                                  ' Counter
            Dim sPath As String                             ' ̧�ٖ�
            Dim sSect As String                             ' ����ݖ�
            Dim sSec2 As String                             ' ����ݖ�

            ' ������ݒ肷��
            stFNC(F_LOAD).sCMD = "LOAD"                     ' LOAD�{�^��
            stFNC(F_SAVE).sCMD = "SAVE"                     ' SAVE�{�^��
            stFNC(F_EDIT).sCMD = "EDIT"                     ' EDIT�{�^��
            stFNC(F_LASER).sCMD = "LASER"                   ' LASER�{�^��
            stFNC(F_LOG).sCMD = "LOG"                       ' LOGGING�{�^��
            stFNC(F_PROBE).sCMD = "PROBE"                   ' PROBE�{�^��
            stFNC(F_TEACH).sCMD = "TEACH"                   ' TEACH�{�^��
            stFNC(F_CUTPOS).sCMD = "CUTPOS"                 ' CUTPOS(��Ĉʒu�␳)�{�^��
            stFNC(F_RECOG).sCMD = "RECOG"                   ' RECOG(�摜�o�^)�{�^��
            ' CHIP,NET�n
            stFNC(F_TTHETA).sCMD = "TTHETA"                 ' T�ƃ{�^�� 
            stFNC(F_TX).sCMD = "TX"                         ' TX�{�^�� 
            stFNC(F_TY).sCMD = "TY"                         ' TY�{�^�� 
            stFNC(F_TY2).sCMD = "TY2"                       ' TY2�{�^��
            stFNC(F_EXR1).sCMD = "EXR1"                     ' �O�����R1è��ݸ����� 
            stFNC(F_EXTEACH).sCMD = "EXTEACH"               ' �O�����è��ݸ�����
            stFNC(F_CARREC).sCMD = "CARIBREC"               ' �����ڰ��ݕ␳�o�^����
            stFNC(F_CAR).sCMD = "CARIB"                     ' �����ڰ������� 
            stFNC(F_CUTREC).sCMD = "CUTREC"                 ' ��ĕ␳�o�^����
            stFNC(F_CUTREV).sCMD = "CUTREV"                 ' ��Ĉʒu�␳����
            ' NET�n
            stFNC(F_CIRCUIT).sCMD = "CIRCUIT"               ' �����è��ݸ�����
            ' SL436R CHIP,NET�n
            stFNC(F_AUTO).sCMD = "AUTO"                     ' AUTO�{�^�� 
            stFNC(F_LOADERINI).sCMD = "LOADERINI"           ' LOADER INIT�{�^�� 
            '----- V1.13.0.0�B�� -----
            ' TKY�n�I�v�V����
            stFNC(F_APROBEREC).sCMD = "APROBEREC"           ' ��ăv���[�u�o�^���� 
            stFNC(F_APROBEEXE).sCMD = "APROBEEXE"           ' ��ăv���[�u���s����
            stFNC(F_IDTEACH).sCMD = "IDTEACH"               ' IDè��ݸ�����
            stFNC(F_SINSYUKU).sCMD = "SINSYUKU"             ' �L�k�o�^���� 
            stFNC(F_MAP).sCMD = "MAP"                       ' MAP�{�^�� 
            '----- V1.13.0.0�B�� -----
            'V4.1.0.0�D
            stFNC(F_PROBE_CLEANING).sCMD = "PROBECLEAN"      ' �v���[�u�N���[�j���O�{�^��
            'V4.1.0.0�D
            'V4.1.0.0�E
            stFNC(F_MAINTENANCE).sCMD = "IOMAINT"           ' IO�m�F�{�^��
            'V4.1.0.0�E

            stFNC(F_INTEGRATED).sCMD = "INTEGRATED"         ' �����o�^�����{�^�� 'V4.10.0.0�B
            stFNC(F_RECOG_ROUGH).sCMD = "RECOG_ROUGH"       ' ���t�A���C�����g�p�摜�o�^�{�^��  'V5.0.0.9�C
            stFNC(F_FOLDEROPEN).sCMD = "FOLDEROPEN"         ' �t�H���_�\���{�^��(���Y�Ǘ��f�[�^) V6.1.4.0�E

            ' �t�@�C����/�Z�N�V��������ݒ肷��
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then        ' SL436R ? 
                If (Knd = KND_NET) Then
                    sPath = cDEF_FUNCNAME_NET_436           ' ��`�t�@�C����
                Else
                    sPath = cDEF_FUNCNAME_CHIP_436          ' ��`�t�@�C����
                End If
            Else
                If (Knd = KND_TKY) Then
                    sPath = cDEF_FUNCNAME_TKY               ' ��`�t�@�C����
                ElseIf (Knd = KND_CHIP) Then
                    sPath = cDEF_FUNCNAME_CHIP              ' ��`�t�@�C����
                Else
                    sPath = cDEF_FUNCNAME_NET               ' ��`�t�@�C����
                End If
            End If

            sSect = "FUNCDEF"
            sSec2 = "PASSWORD"

            ' �@�\�I���`�e�[�u����ݒ肷��(-1:��\��, 0:�I��s��, 1:�I����)
            For i = 0 To (MAX_FNCNO - 1)                    ' ��`�����J�Ԃ�
                stFNC(i).iDEF = GetPrivateProfileInt(sSect, stFNC(i).sCMD, -1, sPath)
                stFNC(i).iPAS = GetPrivateProfileInt(sSec2, stFNC(i).sCMD, 0, sPath)
            Next i

            ' ۸޲��߽ܰ�ޓ��̗͂L��(0:��, 1:�L) 
            flgLoginPWD = GetPrivateProfileInt("COMMON", "LOGINPWD", 0, sPath)

        Catch ex As Exception
            strMSG = "i-TKY.GetFncDefParameter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "���Y�Ǘ��f�[�^�̃N���A"
    '''=========================================================================
    '''<summary>���Y�Ǘ��f�[�^�̃N���A</summary>
    '''<param name="flag">(INP)0=��ʕ\���Ȃ�, 1=��ʕ\������</param> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub ClearCounter(ByVal flag As Short)

        Dim i As Short
        Dim strMSG As String

        Try

            ' ���Y�Ǘ���񏉊���
            Call ClearTrimResult()
            '' '' ''glPlateCount = 0                                        ' �v���[�g������
            '' '' ''glGoodCount = 0                                         ' �Ǖi��R��
            '' '' ''glNgCount = 0                                           ' �s�ǒ�R��
            '' '' ''glCircuitNgTotal = 0                                    ' �s�ǃT�[�L�b�g��
            '' '' ''glCircuitGoodTotal = 0                                  ' �Ǖi�T�[�L�b�g��

            '----- V1.22.0.0�C�� -----
            ' �T�}���[���M���O�p�f�[�^������������
            Call SummaryLoggingDataInit(stSummaryLog)
            '----- V1.22.0.0�C�� -----

            If flag Then
                Fram1LblAry(FRAM1_ARY_GO).Text = "0"                    ' GO��(�T�[�L�b�g�� or ��R��)
                Fram1LblAry(FRAM1_ARY_NG).Text = "0"                    ' NG��(�T�[�L�b�g�� or ��R��)
                Fram1LblAry(FRAM1_ARY_NGPER).Text = " - "               ' NG%
                Fram1LblAry(FRAM1_ARY_PLTNUM).Text = "0"                ' PLATE��
                Fram1LblAry(FRAM1_ARY_REGNUM).Text = "0"                ' RESISTOR��
                Fram1LblAry(FRAM1_ARY_ITHING).Text = "0"                ' IT HI NG��
                Fram1LblAry(FRAM1_ARY_FTHING).Text = "0"                ' FT HI NG��
                Fram1LblAry(FRAM1_ARY_ITLONG).Text = "0"                ' IT LO NG��
                Fram1LblAry(FRAM1_ARY_FTLONG).Text = "0"                ' FT LO NG��
                Fram1LblAry(FRAM1_ARY_OVER).Text = "0"                  ' OVER���@'V3.0.0.0�G(V1.22.0.0�J)
                Fram1LblAry(FRAM1_ARY_ITHINGP).Text = " - "             ' IT HI NG%
                Fram1LblAry(FRAM1_ARY_FTHINGP).Text = " - "             ' FT HI NG%
                Fram1LblAry(FRAM1_ARY_ITLONGP).Text = " - "             ' IT LO NG%
                Fram1LblAry(FRAM1_ARY_FTLONGP).Text = " - "             ' FT LO NG%
                Fram1LblAry(FRAM1_ARY_OVERP).Text = " - "               ' OVER NG%
            End If

            For i = 0 To 11
                glRegistNum(i) = 0                                      ' ���z�O���t��R��
                glRegistNumIT(i) = 0                                    ' ���z�O���t��R��
                glRegistNumFT(i) = 0                                    ' ���z�O���t��R��
            Next

            lOkChip = 0                                                 ' OK��
            lNgChip = 0                                                 ' NG��
            dblMinIT = 0                                                ' �ŏ��l
            dblMaxIT = 0                                                ' �ő�l
            dblMinFT = 0                                                ' �ŏ��l
            dblMaxFT = 0                                                ' �ő�l
            dblAverage = 0                                              ' ���ϒl
            dblAverageIT = 0                                            ' ���ϒl   
            dblAverageFT = 0                                            ' ���ϒl
            HEIHOUIT = 0                                                '  ..
            HEIHOUFT = 0                                                '  ..
            dblDeviationIT = 0                                          ' �W���΍�
            dblDeviationFT = 0                                          ' �W���΍�
            '' '' ''glITHINGCount = 0
            '' '' ''glITLONGCount = 0
            '' '' ''glFTHINGCount = 0
            '' '' ''glFTLONGCount = 0
            '' '' ''glITOVERCount = 0
            '' '' ''dblGapIT = 0.0#
            '' '' ''dblGapFT = 0.0#
            '' '' ''gfX_2IT = 0.0#
            '' '' ''gfX_2FT = 0.0#
            glITTOTAL = 0                                               ' IT�v�Z�Ώې� ###138
            glFTTOTAL = 0                                               ' FT�v�Z�Ώې� ###138
            Call ClearAvgDevCount()                                     ' ###198

            gITNx_cnt = -1
            gITNg_cnt = -1
            gFTNx_cnt = -1
            gFTNg_cnt = -1
            Erase gITNx                                                 ' IT ����덷(�X)
            Erase gFTNx                                                 ' FT ����덷(�X)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.ClearCounter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form��f�W�^���X�C�b�`�̐��l�擾"
    '''=========================================================================
    '''<summary>�f�W�^���X�C�b�`�̐��l�擾</summary>
    '''<param name="digL">(OUT)�f�W�^���X�C�b�`���샂�[�h�̐ݒ�擾�i���ʈꌅ�j</param> 
    '''<param name="digH">(OUT)�f�W�^���X�C�b�`�\�����[�h�̐ݒ�擾�i��ʈꌅ�j</param> 
    '''<param name="digSW">(OUT)�f�W�^���X�C�b�`�̐ݒ�擾�i��ʉ��ʁj</param> 
    '''<remarks>�f�W�^���X�C�b�`�̌��݂̐ݒ���擾����</remarks>
    '''=========================================================================
    Public Sub GetMoveMode(ByRef digL As Short, ByRef digH As Short, ByRef digSW As Short)


        Try
            '���ݒl�̐ݒ�
            digL = Me.CbDigSwL.SelectedIndex
            digH = Me.CbDigSwH.SelectedIndex
            digSW = digH * 10 + digL

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            Dim strMSG As String

            strMSG = "i-TKY.GetMoveMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form��f�W�^���X�C�b�`�̐��l�ݒ�"
    '''=========================================================================
    '''<summary>�f�W�^���X�C�b�`�̐��l�ݒ�</summary>
    '''<param name="digL">(IN)�f�W�^���X�C�b�`���샂�[�h�̐ݒ�擾�i���ʈꌅ�j</param> 
    '''<param name="digH">(IN)�f�W�^���X�C�b�`�\�����[�h�̐ݒ�擾�i��ʈꌅ�j</param> 
    '''<remarks>�f�W�^���X�C�b�`�̌��ݒl��ݒ肷��</remarks>
    '''=========================================================================
    Public Sub SetMoveMode(ByVal digL As Short, ByVal digH As Short)


        Try
            '���ݒl�̐ݒ�
            Me.CbDigSwL.SelectedIndex = digL
            Me.CbDigSwH.SelectedIndex = digH

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            Dim strMSG As String

            strMSG = "i-TKY.SetMoveMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "MV10 �r�f�I���C�u��������������"
    '''=========================================================================
    '''<summary>MV10 �r�f�I���C�u��������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Initialize_VideoLib()
        Dim strMSG As String
        Try

            Dim lRet As Integer
            'V6.0.0.0�A            Dim r As Short
            Dim s As String
            '---------------------------------------------------------------------------
            '   �r�f�I���C�u����������������
            '---------------------------------------------------------------------------
            If (pbVideoCapture = False) Then                ' �r�f�I�L���v�`���[�J�n�t���OOFF ?
                pbVideoCapture = True                       ' �r�f�I�L���v�`���[�J�n�t���OON
                ChDir(WORK_DIR_PATH)                        ' MvcPt2.ini�̂���̫��ް����Ɨp̫��ް�Ƃ���
                If (gSysPrm.stDEV.giEXCAM = 0) Then         ' �����J����?
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfPixelX       ' �s�N�Z���lX(um)
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfPixelY       ' �s�N�Z���lY(um)
                Else
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfEXCAM_PixelX ' �O������߸�ْlX(um)
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfEXCAM_PixelY ' �O������߸�ْlY(um)
                End If

                VideoLibrary1.OverLay = True                ' �����s�v ?
                lRet = VideoLibrary1.Init_Library()         ' �r�f�I���C�u����������
                If (lRet <> 0) Then                         ' Video.OCX�G���[ ?
                    Select Case lRet
                        Case cFRS_VIDEO_INI
                            s = "VIDEOLIB: Already initialized."
                        Case cFRS_VIDEO_PRP
                            s = "VIDEOLIB: Invalid property value."
                        Case cFRS_MVC_UTL
                            s = "VIDEOLIB: Error in MvcUtil"
                        Case cFRS_MVC_PT2
                            s = "VIDEOLIB: Error in MvcPt2"
                        Case cFRS_MVC_10
                            s = "VIDEOLIB: Error in Mvc10"
                        Case Else
                            s = "VIDEOLIB: Unexpected error 2"
                    End Select
                    Call System1.TrmMsgBox(gSysPrm, s, MsgBoxStyle.OkOnly, My.Application.Info.Title)
                Else
                    ' "���C�u��������������"
                    pbVideoInit = True

                    ' V3.0.0.0�B MV10�̃{�[�h�̃r�f�I�[�q��ύX�\�Ƃ���B��    'V6.0.0.0�H �J�����������̌�Ɉړ�
                    ' �f�o�b�O���s���� bin\Debug\TKY.exe �����s����邽�߁AC:\TRIM\*.dll �ł͂Ȃ� bin\Debug\*.dll ���g�p�����
                    ' Basler�o�[�W�����EMV10�o�[�W�����̓���ւ��������ꍇ�́Abin\Debug\DefMv10Fnc.dll ���蓮�œ���ւ��邩
                    ' TKY�����r���h���āA����ւ��� C:\TRIM\DefMv10Fnc.dll �� bin\Debug �ɃR�s�[������K�v������
                    INTERNAL_CAMERA = VideoLibrary1.InternalCameraPort
                    EXTERNAL_CAMERA = VideoLibrary1.ExternalCameraPort
                    ' V3.0.0.0�B MV10�̃{�[�h�̃r�f�I�[�q��ύX�\�Ƃ���B��

                    'V6.0.0.0-28                    'Video���~����
                    'V6.0.0.0-28                    VideoLibrary1.VideoStop()

                    ' OcxTeach�p�N���X���C���␳�p�p�����[�^�ݒ�
                    'V6.0.0.0�A                r = Teaching1.SetCrossLineCorrectParam(VideoLibrary1.gPicture2, VideoLibrary1.gPicture1, _
                    'V6.0.0.0�A                                                VideoLibrary1.gCrosLineXY2, VideoLibrary1.gCrosLineXY1)
                    'V6.0.0.0�A                If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                    'V6.0.0.0�A                    strMSG = "i-TKY.Form1_Activated() SetCrossLineCorrectParam ERROR = "
                    'V6.0.0.0�A                    MsgBox(strMSG)
                    'V6.0.0.0�A                End If

                    ' �N���X���C���\��
                    Call SetCrossLine()                                     ' Form_Initialize_Renamed()���炱���ֈړ�  'V6.0.0.0�C  

                    'If (Debugger.IsAttached) Then
                    VideoLibrary1.SetCameraOptionContextMenu(True, True)    ' fps �\���R���e�L�X�g���j���[
                    'End If

                End If
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Form1_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "���u����������-�r�f�I��荞�݊J�n�A���_���A"
    '''=========================================================================
    '''<summary>���u����������-�r�f�I��荞�݊J�n�A���_���A</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Initialize_TrimMachine()

        Dim strMSG As String = ""

        Try

            Dim lRet As Long                                            'V1.18.0.0�D
            Dim r As Short

            '---------------------------------------------------------------------------
            '   �r�f�I�X�^�[�g�ƌ��_���A������FL�ւ̏������t�@�C�����t
            '---------------------------------------------------------------------------
            If (gflgResetStart = False) Then                ' �����ݒ�ς݂łȂ� ?

                ' ����ڰ�̧�ق̕ۑ��ꏊ��"C:\TRIM"�ɐݒ肷��(VideoStart()��Ɏw�肷��)
                ' (��)�Ǘ�̧�فuPt2Template.xxx�v�͋N��̫��ނɍ쐬�����B
                r = Me.VideoLibrary1.SetTemplatePass(cTEMPLATPATH)

#If cOFFLINEcDEBUG = 0 Then
                ' ���_���A����()
                r = sResetStart()
                gflgResetStart = True                       ' �����ݒ�t���OON
                ''V5.0.0.1-23                If (r <= cFRS_ERR_EMG Or r = cFRS_ERR_RST) Then ' RESET�͏I������
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then      ' SL432R�n ? ###035
                    If (r <= cFRS_ERR_EMG Or r = cFRS_ERR_RST Or r = cFRS_ERR_CVR) Then ' RESET�͏I������
                        ' �����I��
                        Call AppEndDataSave()
                        Call AplicationForcedEnding()
                        Exit Sub
                    End If
                Else
                    If (r <= cFRS_ERR_EMG Or r = cFRS_ERR_RST) Then ' RESET�͏I������
                        ' �����I��
                        Call AppEndDataSave()
                        Call AplicationForcedEnding()
                        Exit Sub
                    End If
                End If
                'V5.0.0.1-23
                '----- V1.14.0.0�D�� -----
                ' LED�o�b�N���C�gON/OFF(�I�v�V����)
                Call Set_Led(0, 0)                          ' �o�b�N���C�g�Ɩ�ON����OFF 

                ' LED�o�b�N���C�gON(EXTOUT)                 '###061
                'Call EXTOUT1(glLedBit, 0)                   ' LED ON 
                '----- V1.14.0.0�D�� -----
#End If

                ''V6.0.1.022���ʏ�̑��x��]������B 
                SetXYStageSpeed(StageSpeed.NormalSpeed)
                ''V6.0.1.022��

                '-----------------------------------------------------------------------
                '   FL���։��H�����𑗐M����(FL���ŉ��H�����t�@�C��������ꍇ)
                '-----------------------------------------------------------------------
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                    ' '' �f�[�^���M���̃��b�Z�[�W�\��
                    ''strMSG = MSG_148
                    ''Call Z_PRINT(strMSG)                                ' ү���ޕ\��(���O���)
                    Dim strSetFileName As String = ""

                    ' FL�p���H�����t�@�C�������[�h����FL���։��H�����𑗐M����
                    r = SendTrimCondInfToFL(stCND, DEF_FLPRM_SETFILENAME, strSetFileName)
                    If (r <> SerialErrorCode.rRS_OK) Then
                        '----- V2.0.0.0�D�� -----
                        If (r <= SerialErrorCode.rRS_FLCND_XMLREADERR) Then
                            '"���H�����t�@�C�����[�h�G���[�B""
                            strMSG = MSG_158 + "(File = " + strSetFileName + ")"
                        Else
                            '"�e�k�ʐM�ُ�B�e�k�Ƃ̒ʐM�Ɏ��s���܂����B" + vbCrLf + "�e�k�Ɛ������ڑ��ł��Ă��邩�m�F���Ă��������B"
                            strMSG = MSG_150
                        End If
                        'strMSG = MSG_150                    '"�e�k�ʐM�ُ�B�e�k�Ƃ̒ʐM�Ɏ��s���܂����B" + vbCrLf + "�e�k�Ɛ������ڑ��ł��Ă��邩�m�F���Ă��������B"
                        Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                        '----- V2.0.0.0�D�� -----
                    End If
                End If
                'Call System1.sLampOnOff(LAMP_Z, False)     ' HALT����ON 
                '----- V1.14.0.0�A�� -----
                ' FL�p�p���[������񏉊���
                Call GetFlAttInfoData(strMSG, stPWR_LSR, 0)
                ' INtime���֌Œ�ATT���𑗐M����(������)
                lRet = SetFixAttInfo(stPWR_LSR.AttInfoAry(0)) 'V1.18.0.0�D
                '----- V1.14.0.0�A�� -----

                '-----------------------------------------------------------------------
                ' �R�}���h�{�^����L���ɂ���
                '-----------------------------------------------------------------------
                Call Form1Button(2)                         ' �{�^�����̕\��/��\��
                Call Form1Button(1)                         ' �{�^��������/�񊈐���
                frmHistoryData.Visible = True
                '----- V1.18.0.0�B�� -----
                If (giPrint = 1) Then                       ' Print�{�^���L�� ? 
                    gSysPrm.stDEV.rPrnOut_flg = True
                    BtnPrintOnOff.Text = "Print ON"
                    BtnPrintOnOff.BackColor = Color.Lime
                Else
                    giPrint = 0
                End If
                '----- V1.18.0.0�B�� -----

                '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
                ' �u������{�^���v
                BtnSubstrateSet.Text = LBL_BTN_SUBSTRAT                 ' "�����"
                BtnSubstrateSet.Visible = False                         '�u������{�^���v��\�� 
                If (giSubstrateInvBtn = 1) Then                         ' �ꎞ��~��ʂł́u������v�{�^���̗L�� ?�@
                    BtnSubstrateSet.Visible = True                      '�u������{�^���v�\��
                    BtnSubstrateSet.Enabled = False                     '�u������{�^���v�񊈐���
                End If
                '----- V4.11.0.0�E�� -----

                '-----------------------------------------------------------------------
                '   �ݒ�l�\��(�I�v�V����)
                '-----------------------------------------------------------------------
                ' ���������V�X�p�����\��("������ = 99.9%")
                ' ��۰�ر��Ȱ��̐ݒ��OcxSystem�̌��_���A�����ōs����
                If (gSysPrm.stRMC.giRmCtrl2 >= 2 And
                    gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then          ' ۰�ر��Ȱ�����L(RMCTRL2�Ή����L��) ? ###029
                    'If (gSysPrm.stRMC.giRmCtrl2 >= 1) Then          ' ۰�ر��Ȱ�����L(RMCTRL2�Ή����L��) ?
                    strMSG = LBL_ATT + "  " + CDbl(gSysPrm.stRAT.gfAttRate).ToString("##0.0") + " %"
                    Me.LblRotAtt.Text = strMSG
                    '----- V1.16.0.0�M�� -----
                    ' �������̕\��/��\�����V�X�p�����ݒ肷��
                    'Me.LblRotAtt.Visible = True
                    If (giNoDspAttRate = 1) Then
                        Me.LblRotAtt.Visible = False
                    Else
                        Me.LblRotAtt.Visible = True
                    End If
                    '----- V1.16.0.0�M�� -----
                End If

                '' FL���̃I�[�g�p���[�����p�d���l�\��(FL���͌������̑���ɓd���l��\������) ###066
                '' ��FL�Ńp���[���[�^�̃f�[�^�擾�^�C�v���u�h�^�n�ǎ��v/�u�t�r�a�v�Łu�p���[����l�\������v�̏ꍇ�ɕ\������
                'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And _
                '   (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                '    strMSG = LBL_FLCUR + " ----mA(Off:---)"
                '    Me.LblRotAtt.Text = strMSG
                '    Me.LblRotAtt.Visible = True
                'End If

                ' ��d���l�\��
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    strMSG = "��d���l "
                'Else
                '    strMSG = "CurrentVal "
                'End If
                Select Case (gSysPrm.stSPF.giProcPower2)
                    Case 0                                      ' �w��Ȃ�(�W��)
                        LblCur.Text = Form1_002 & "0.25A"          '  ��d���l 0.25A
                        'Case 1
                        '    LblCur.Text = strMSG & "1.00A"          '  ��d���l 1.00A
                        'Case 2
                        '    LblCur.Text = strMSG & "0.75A"          '  ��d���l 0.75A
                    Case 3
                        LblCur.Text = Form1_002 & "0.50A"          '  ��d���l 0.50A
                End Select

                ' ���H�d�͕\��/��\���ݒ�
                'If (gSysPrm.stSPF.giProcPower = 4) And (gSysPrm.stSPF.giProcPower2 <> 0) Then  '###250
                If (gSysPrm.stSPF.giProcPower2 <> 0) Then                                       '###250
                    LblCur.Visible = True
                Else
                    LblCur.Visible = False
                End If

                '-----------------------------------------------------------------------
                ' �R���\�[���L�[�̃��b�`����
                '-----------------------------------------------------------------------
                Call ZCONRST()

                ' 'V4.1.0.0�F
                CheckPLCLowBatteryAlarm()

                '-----------------------------------------------------------------------
                ' �Ď��^�C�}�[�J�n
                '-----------------------------------------------------------------------
                ''V6.0.1.0�L �� Timer1.Interval = 10                        ' �Ď��^�C�}�[�l(msec)
                Timer1.Interval = 100                                   ' �Ď��^�C�}�[�l(msec)
                ''V6.0.1.0�L��
                Timer1.Enabled = True                                   ' �Ď��^�C�}�[�J�n
                '----- V1.18.0.0�A�� -----
                ' QR�R�[�h��M�`�F�b�N�^�C�}�[�J�n(���[���a����)
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    TimerQR.Interval = 1000
                    TimerQR.Enabled = True
                End If
                '----- V1.18.0.0�A�� -----
                '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
                If (giQrCodeType = QrCodeType.KoaEw) Then
                    If (gbQRCodeReaderUse = True OrElse gbQRCodeReaderUseTKYNET = True) Then    ' �p�q�R�[�h���[�h�@�\��TKY-CHIP�̂ݗL�� 'V6.1.4.10�A TKY-NET�igbQRCodeReaderUseTKYNET�j�ǉ�
                        TimerQR.Interval = 1000                         ' �Ď��^�C�}�[�l(msec)
                        TimerQR.Enabled = True                          ' �Ď��^�C�}�[�J�n
                    End If
                End If
                '----- V6.1.4.0_22�� -----
#If False Then                          'V5.0.0.9�N
                '----- V1.23.0.0�@�� -----
                ' �o�[�R�[�h��M�`�F�b�N�^�C�}�[�J�n(���z�Гa����)
                If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then
                    TimerBC.Interval = 1000
                    TimerBC.Enabled = True
                End If
                '----- V1.23.0.0�@�� -----

                '----- V1.23.0.0�@�� -----
                ' �o�[�R�[�h��M�`�F�b�N�^�C�}�[�J�n(���z�Гa����)/(WALSIN�aSL436S�Ή�) V4.11.0.0�A
                'If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then   V4.11.0.0�A
                If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Or (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then ' V4.11.0.0�A
                    TimerBC.Interval = 1000
                    TimerBC.Enabled = True
                End If
                '----- V1.23.0.0�@�� -----
#Else                                   'V5.0.0.9�N
                ' �o�[�R�[�h��M�`�F�b�N�^�C�}�[�J�n
                If (BarcodeType.None <> BarCode_Data.Type) Then
                    'V5.0.0.9�R          ��
                    BarCode_Data.BC_ReadCount = 0
                    BarCode_Data.BC_ReadDataFirst = ""                  ' �o�[�R�[�h�P��ڂœǍ��񂾃f�[�^�ۑ��p
                    BarCode_Data.BC_ReadDataSecound = ""                ' �o�[�R�[�h�Q��ڂœǍ��񂾃f�[�^�ۑ��p
                    'V5.0.0.9�R          ��

                    TimerBC.Interval = 1000
                    TimerBC.Enabled = True
                End If
#End If
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Initialize_TrimMachine TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�\�t�g�I�����̃X�e�[�W�ړ�����(�I�v�V����)"
    '''=========================================================================
    ''' <summary>�\�t�g�I�����̃X�e�[�W�ړ�����(�I�v�V����) V1.13.0.0�I</summary>
    ''' <param name="PosX">(INP)�X�e�[�W�ʒuX</param>
    ''' <param name="PosY">(INP)�X�e�[�W�ʒuY</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function AppEndStageMove(ByVal PosX As Double, ByVal PosY As Double) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���������APP�I�����̃X�e�[�W�ʒu�w��Ȃ��Ȃ�NOP
            If (PosX = 0) And (PosY = 0) Then Return (cFRS_NORMAL)

            ' "XY�e�[�u�����ړ����܂�","�X���C�h�J�o�[����Ă�������"�\����X���C�h�J�o�[�҂�
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                r = System1.ClampVacume_Ctrl(gSysPrm, 1, giAppMode, 1)  ' �N�����v/�z�� ON
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)               
                    Return (r)
                End If
                r = System1.Form_Reset(cGMODE_XYMOVE, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)               
                    Return (r)
                End If
            End If
            Me.Refresh()                                                ' ���b�Z�[�W����������

            ' ���������APP�I�����̃X�e�[�W�ʒu�ֈړ�
            r = System1.EX_SMOVE2(gSysPrm, PosX, PosY)
            If (r < cFRS_NORMAL) Then                                   ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                Return (r)                                              ' �A�v�������I����
            End If

            ' "�X���C�h�J�o�[���J���Ă�������"�\����X���C�h�J�o�[�J�҂�
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                r = System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)               
                    Return (r)
                End If
                ' �N�����v/�z��OFF
                r = System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)
                If (r < cFRS_NORMAL) Then                               ' �G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)               
                    Return (r)
                End If
            End If

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.AppEndStageMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "��ċ����I�������ް��ۑ��m�F"
    '''=========================================================================
    '''<summary>��ċ����I�������ް��ۑ��m�F</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub AppEndDataSave()

        'Dim ret As Short
        Dim ret As MsgBoxResult         'V4.10.0.0�D

        '----- V6.1.4.0�I��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
        If (giLotChange = 1) Then                                   ' ���b�g�ؑւ��@�\�L�� ?
            ' ���[�_�[�o��(ON=�g���}���쒆 or �g���~���O�s�ǐM����A�������^�]���~�ʒm�Ɏg�p, OFF=�g���}���f�B)
            Call SetLoaderIO(COM_STS_TRM_STATE Or COM_STS_TRM_NG, COM_STS_TRM_READY)
        End If
        '----- V6.1.4.0�I�� -----

        ' �ҏW���̃f�[�^���� ?
        If gCmpTrimDataFlg = 1 Then
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    ret = MsgBox("�A�v���P�[�V�������I�����܂��B" & vbCrLf & "�g���~���O�f�[�^��ۑ����܂����H", MsgBoxStyle.OkCancel, "")
            'Else
            '    ret = MsgBox("Quits the program." & vbCrLf & "Do you store trimming data?", MsgBoxStyle.OkCancel, "")
            'End If
            'ret = MsgBox(Form1_003 & vbCrLf & Form1_004, MsgBoxStyle.OkCancel, "")
            ret = MsgBox(Form1_003 & vbCrLf & Form1_004, MsgBoxStyle.YesNo, "") 'V4.10.0.0�D
            'If ret = MsgBoxResult.Ok Then
            If (ret = MsgBoxResult.Yes) Then                                    'V4.10.0.0�D
                ' �f�[�^�ۑ�
                Call Me.CmdSave_Click(Me.CmdSave, New System.EventArgs())
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    ret = MsgBox("�f�[�^�̕ۑ����������܂����B" & vbCrLf & "�A�v���P�[�V�������I�����܂��B", MsgBoxStyle.OkOnly, "")
                'Else
                '    ret = MsgBox("A save of data was completed." & vbCrLf & "Quits the program.", MsgBoxStyle.OkOnly, "")
                'End If
                'V4.10.0.0�D              ��
                Dim msg As String
                If (0 = gCmpTrimDataFlg) Then
                    ' �ۑ�����
                    msg = (Form1_005 & vbCrLf & Form1_003)
                Else
                    ' �ۑ���ݾق���
                    msg = Form1_003
                End If
                'ret = MsgBox(Form1_005 & vbCrLf & Form1_003, MsgBoxStyle.OkOnly, "")
                ret = MsgBox(msg, MsgBoxStyle.OkOnly, "")
                'V4.10.0.0�D              ��
            Else
                ' �f�[�^�ۑ��Ȃ�
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    ret = MsgBox("�A�v���P�[�V�������I�����܂��B", MsgBoxStyle.OkOnly, "")
                'Else
                '    ret = MsgBox("Quits the program.", MsgBoxStyle.OkOnly, "")
                'End If
                ret = MsgBox(Form1_003, MsgBoxStyle.OkOnly, "")
            End If
        Else
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    ret = MsgBox("�A�v���P�[�V�������I�����܂��B", MsgBoxStyle.OkOnly, "")
            'Else
            '    ret = MsgBox("Quits the program.", MsgBoxStyle.OkOnly, "")
            'End If
            ret = MsgBox(Form1_003, MsgBoxStyle.OkOnly, "")
        End If

    End Sub

#End Region

#Region "��ċ����I������"
    '''=========================================================================
    '''<summary>��ċ����I������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub AplicationForcedEnding()

        Dim lRet As Integer
        Dim hProcInf As New System.Diagnostics.ProcessStartInfo()
        'Dim ret As Short

        Try
            'V6.0.0.0�D            Call FinalEnd_GazouProc(ObjGazou)  'V3.0.0.0�DDispGazou�I��

            '----- ###175��-----
            'V1.25.0.5�AIf (bFgAutoMode) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
            If (Not bEmergencyOccurs) And (bFgAutoMode) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                ' �c���菜�����b�Z�[�W�\��(START�L�[�����҂���ʕ\��)
                lRet = Sub_CallFrmRset(System1, cGMODE_LDR_WKREMOVE2)
            End If
            '----- ###175�� -----
            '----- V1.18.0.0�B�� -----
            ' �g���~���O���ʈ������(���[���a����)
            Call PrnTrimResult(2)

            ' ����I������(���[���a����)
            Call Print_End()
            '----- V1.18.0.0�B�� -----
            '----- V1.18.0.0�A�� -----
            ' �|�[�g�N���[�Y(QR�R�[�h��M�p ���[���a����)
            Call QR_Rs232c_Close()
            '----- V1.18.0.0�A�� -----
            '----- V1.23.0.0�@�� -----
            ' �|�[�g�N���[�Y(�o�[�R�[�h��M�p ���z�Гa����)
            Call BC_Rs232c_Close()
            '----- V1.23.0.0�@�� -----

            ' �I�[�g���[�_�I������
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ?
                '----- V6.1.4.0�I��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
                'Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_READY) ' ���[�_�[�o��(ON=�g���}���쒆, OFF=�g���}���f�B) ###035
                If (giLotChange = 1) Then                               ' ���b�g�ؑւ��@�\�L�� ?
                    ' �g���~���O�s�ǐM����A�������^�]���~�ʒm�Ɏg�p
                    Call SetLoaderIO(COM_STS_TRM_STATE Or COM_STS_TRM_NG, COM_STS_TRM_READY)
                Else
                    ' ���[�_�[�o��(ON=�g���}���쒆, OFF=�g���}���f�B)
                    Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_READY)
                End If
                '----- V6.1.4.0�I�� -----
            Else
                Call Loader_Term()                                      ' ���[�_�[�o��(ON=�Ȃ�,OFF=�g���}�����f�B)
            End If

            ' �V�O�i���^���[���䏉����(On=0, Off=�S�ޯ�)
            'V5.0.0.9�M �� V6.0.3.0�G
            ' Call System1.SetSignalTower(0, &HFFFF)                      ' ###007
            Call System1.SetSignalTowerCtrl(System1.SIGNAL_ALL_OFF)
            'V5.0.0.9�M �� V6.0.3.0�G

            ' �X���C�h�J�o�[�I�[�v��/�N���[�Y�o���uOFF
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then       ' SL432R�n ? 
                Call ZSLCOVERCLOSE(0)                                   ' �X���C�h�J�o�[�N���[�Y�o���uOFF
                Call ZSLCOVEROPEN(0)                                    ' �X���C�h�J�o�[�I�[�v���o���uOFF
            End If

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
            If (giDoorLock = 1) Then                                    ' �d�����b�N�L�� ?
                Call EXTOUT1(0, EXTOUT_EX_LOK_ON)
            End If
            '----- V1.18.0.1�G�� -----

            '----- V2.0.0.0�L�� -----
            ' �T�[�{�A���[���Ȃ�A���[���N���A����
            Call SrvAlm_Check()

            '' �T�[�{�A���[���N���A
            'Call CLEAR_SERVO_ALARM(1, 1)                                ' X/Y,Z/THETA '###031
            ''----- V1.13.0.0�@�� -----
            'If (gSysPrm.stDEV.giPrbTyp = 3) Then                        ' Z2���� ? 
            '    Call CLEAR_SERVO_ALARM_Z2(1)                            ' Z2/�\��
            'End If
            ''----- V1.13.0.0�@�� -----
            '----- V2.0.0.0�L�� -----

            ' �r�f�I���C�u�����I������
            If (pbVideoInit = True) Then
                lRet = VideoLibrary1.Close_Library
                If (lRet <> 0) Then
                    Select Case lRet
                        Case cFRS_VIDEO_INI
                            'Call System1.TrmMsgBox(gSysPrm, "Video library: Not initialized.", MsgBoxStyle.OkOnly, My.Application.Info.Title)
                            Call MsgBox("Video library: Not initialized.", MsgBoxStyle.OkOnly, My.Application.Info.Title) ' 2011.09.01
                        Case Else
                            ' "�\�����ʃG���["
                            'Call System1.TrmMsgBox(gSysPrm, "Video library: Unexpected error.", MsgBoxStyle.OkOnly, My.Application.Info.Title)
                            Call MsgBox("Video library: Unexpected error.", MsgBoxStyle.OkOnly, My.Application.Info.Title) ' 2011.09.01
                    End Select
                End If
            End If

            ' ���샍�O�o��("�g���}���u��~")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC10, "")
            gflgCmpEndProcess = True

            ' �N�����v�y�уo�L���[��OFF '###001
            Call Me.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)

            ' �����vOFF
            Call LAMP_CTRL(LAMP_START, False)   ' START�����vOFF 
            Call LAMP_CTRL(LAMP_RESET, False)   ' RESET�����vOFF 
            Call LAMP_CTRL(LAMP_Z, False)       ' Z�����vOFF 

            '----- V1.14.0.0�D�� -----
            ' LED�o�b�N���C�gOFF(�I�v�V����)
            Call Set_Led(2, 0)
            '----- V1.14.0.0�D�� -----

            '-----------------------------------------------------------------------
            '   Mutex�̉��
            '-----------------------------------------------------------------------
            gmhTky.ReleaseMutex()

            '-----------------------------------------------------------------------
            '   �C�x���g�̉��
            '-----------------------------------------------------------------------
            RemoveHandler SystemEvents.SessionEnding, AddressOf SystemEvents_SessionEnding

            '-----------------------------------------------------------------------
            '�I����Videolib�֌W�ŃG���[���������邽�ߋ����I�ɊO������A�v�����I��������B
            '-----------------------------------------------------------------------
            hProcInf.FileName = APP_FORCEEND
            'hProcInf.Arguments = gAppName
            hProcInf.Arguments = System.Diagnostics.Process.GetCurrentProcess.ProcessName
            Call System.Diagnostics.Process.Start(hProcInf)

            Call System.Threading.Thread.Sleep(2000)

        Catch ex As Exception
            ' ���샍�O�o��("�g���}���u��~")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC10, "")
            gflgCmpEndProcess = True

            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub

#End Region
    '----- V2.0.0.0�L�� -----
#Region "�T�[�{�A���[���`�F�b�N����"
    '''=========================================================================
    '''<summary>�T�[�{�A���[���`�F�b�N����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SrvAlm_Check()

        Dim SrvSts As Long = 0
        Dim Bit_Clr_XY As Integer = 0
        Dim Bit_Clr_ZT As Integer = 0
        Dim strMSG As String

        Try

            ' XY���T�[�{�A���[���`�F�b�N
            If (gSysPrm.stDEV.giXYtbl <> 0) Then                                    ' XY���� ? 
                Call INP16(SRVALM_XY_STS, SrvSts)
                If ((SrvSts And (SRVALM_X_BIT + SRVALM_Y_BIT)) <> 0) Then           ' XY���A���[�� ?
                    Bit_Clr_XY = 1                                                  ' XY���T�[�{�A���[���N���A
                End If
            End If

            ' Z/THETA���T�[�{�A���[���`�F�b�N
            If (gSysPrm.stDEV.giPrbTyp <> 0) Or (gSysPrm.stDEV.giTheta <> 0) Then   ' Z/THETA������ ? 
                Call INP16(SRVALM_ZT_STS, SrvSts)
                If ((SrvSts And (SRVALM_Z_BIT + SRVALM_T_BIT)) <> 0) Then           ' Z/THETA���A���[�� ?
                    Bit_Clr_ZT = 1                                                  ' Z/THETA���T�[�{�A���[���N���A
                End If
            End If

            ' X/Y,Z/THETA���̂ǂꂩ���T�[�{�A���[���Ȃ�T�[�{�A���[���N���A����
            If (Bit_Clr_XY <> 0) Or (Bit_Clr_ZT <> 0) Then
                Call SERVO_POWER(0, 0, 0, 0)                                        ' �T�[�{OFF(X,Y,Z,T) 
                Call CLEAR_SERVO_ALARM(Bit_Clr_XY, Bit_Clr_ZT)                      ' �T�[�{�A���[���N���A
            End If

            ' Z2���T�[�{�A���[���`�F�b�N
            If (gSysPrm.stDEV.giPrbTyp = 3) Then                                    ' Z2���� ? 
                Call INP16(SRVALM_Z2_STS, SrvSts)
                If (SrvSts And SRVALM_Z2_BIT) Then                                  ' �A���[���Ȃ�
                    Call SERVO_POWER_Z2(0, 0)                                       ' �T�[�{OFF(Z2,��) 
                    Call CLEAR_SERVO_ALARM_Z2(1)                                    ' �T�[�{�A���[���N���A
                End If
            End If

        Catch ex As Exception
            strMSG = "SrvAlm_Check() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V2.0.0.0�L�� -----
#Region "�V�X�p�����N���X���C���ʒu��ݒ肷��"
    '''=========================================================================
    '''<summary>�۽ײݕ␳</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetCrossLine()

        ' �V�X�p�����N���X���C���ʒu��ݒ肷�� 
        If gKeiTyp = KEY_TYPE_RS Then
            'Me.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
            'V6.0.0.0�C            Me.Picture2.Left = CROSS_LINEX + VideoLibrary1.Location.X
            'V6.0.0.0�C            Me.Picture1.Top = CROSS_LINEY + VideoLibrary1.Location.Y
            VideoLibrary1.SetCrossLineCenter(CROSS_LINEX, CROSS_LINEY)                              'V6.0.0.0�C
        Else
            'V6.0.0.0�C            Me.Picture1.Top = gSysPrm.stDVR.giCrossLineX + VideoLibrary1.Location.Y
            'V6.0.0.0�C            Me.Picture2.Left = gSysPrm.stDVR.giCrossLineY + VideoLibrary1.Location.X
            VideoLibrary1.SetCrossLineCenter(gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX) 'V6.0.0.0�C
        End If

        VideoLibrary1.SetCrossLineVisible(True) 'V6.0.0.0�C
    End Sub
#End Region

#Region "�C���^�[���b�N��Ԃ̕\��/��\��"
    '''=========================================================================
    '''<summary>�C���^�[���b�N��Ԃ̕\��/��\��</summary>
    ''' <returns>�C���^�[���b�N���
    '''          INTERLOCK_STS_DISABLE_FULL = �C���^�[���b�N�S����
    '''          INTERLOCK_STS_DISABLE_PART = �C���^�[���b�N�ꕔ�����i�X�e�[�W����\�j
    '''          INTERLOCK_STS_DISABLE_NO   = �C���^�[���b�N��ԁi�����Ȃ��j
    ''' </returns>
    '''<remarks></remarks>
    '''=========================================================================
    Public Function DispInterLockSts() As Integer

        Dim r As Integer
        Dim InterlockSts As Integer
        Dim SwitchSts As Long
        Dim strMSG As String

        Try
            ' �C���^�[���b�N��Ԃɂ��X�e�[�^�X�\����ύX
            r = INTERLOCK_CHECK(InterlockSts, SwitchSts)
#If cOFFLINEcDEBUG Then
            InterlockSts = INTERLOCK_STS_DISABLE_FULL
#End If
            ' �C���^�[���b�N�S�����̏ꍇ
            If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Then         ' �C���^�[���b�N�S���� ?
                Me.lblInterLockMSG.Text = MSG_SPRASH25                  '�u�C���^�[���b�N�S�������v�\��
                Me.lblInterLockMSG.Visible = True
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R���̓C���^�[���b�N��Ԃ��o�͂��� ###070 
                    '###131 Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)         ' ���[�_�[�o��(ON=�C���^�[���b�N������,OFF=�Ȃ�)
                    ' �C���^�[���b�N��ԑ��M(SL436R��) '###114
                    Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)      ' ���[�_�[�o��(ON=�C���^�[���b�N������,OFF=�Ȃ�)
                End If
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed ###108

                ' �C���^�[���b�N�ꕔ�����̏ꍇ
            ElseIf (InterlockSts = INTERLOCK_STS_DISABLE_PART) Then     ' �C���^�[���b�N�ꕔ�����i�X�e�[�W����\�j ?
                Me.lblInterLockMSG.Text = MSG_SPRASH26                  '�u�C���^�[���b�N�ꕔ�������v�\��
                Me.lblInterLockMSG.Visible = True
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R���̓C���^�[���b�N��Ԃ��o�͂��� ###070   
                    '###131 'Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)         ' ���[�_�[�o��(ON=�C���^�[���b�N������,OFF=�Ȃ�)
                    ' �C���^�[���b�N��ԑ��M(SL436R��) '###114
                    Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)      ' ���[�_�[�o��(ON=�C���^�[���b�N������,OFF=�Ȃ�)
                End If
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed ###108

                ' �C���^�[���b�N���̏ꍇ
            Else                                                        '�u�C���^�[���b�N���v
                Me.lblInterLockMSG.Visible = False
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R���̓C���^�[���b�N��Ԃ��o�͂��� ###070  
                    '###131 'Call SetLoaderIO(&H0, LOUT_INTLOK_DISABLE)         ' ���[�_�[�o��(ON=�Ȃ�,OFF=�C���^�[���b�N������)
                    ' �C���^�[���b�N��ԑ��M(SL436R��) '###114
                    Call SetLoaderIO(&H0, LOUT_INTLOK_DISABLE)      ' ���[�_�[�o��(ON=�Ȃ�,OFF=�C���^�[���b�N������)
                End If
                Call ZSELXYZSPD(0)                                      ' XYZ Normal speed ###108
            End If

            Return (InterlockSts)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.DispInterLockSts() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�t�H�[���̃��x����ݒ肷��"
    '''=========================================================================
    '''<summary>�t�H�[���̃��x����ݒ肷��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form1LanguageSet()

        ' ���Y�Ǘ���񃉃x����ݒ肷��
        If (gTkyKnd = KND_CHIP) Then
            ' CHIP�̏ꍇ
            LblUnit.Text = MSG_TOTAL_REGISTOR           ' "��R�P��"
        Else
            ' TKY-NET�̏ꍇ
            LblUnit.Text = MSG_TOTAL_CIRCUIT            ' "�T�[�L�b�g�P��"
        End If

        'LblPlate.Text = MSG_MAIN_LABEL01                    ' "�����="
        'LblcNGPER.Text = MSG_MAIN_LABEL02                   ' "NG��="
        'LblcITHINGP.Text = MSG_MAIN_LABEL03                 ' "IT H NG��="
        'LblcITLONGP.Text = MSG_MAIN_LABEL04                 ' "IT L NG��="
        'LblcFTHINGP.Text = MSG_MAIN_LABEL05                 ' "FT H NG��="
        'LblcFTLONGP.Text = MSG_MAIN_LABEL06                 ' "FT L NG��="
        'LblcOVERP.Text = MSG_MAIN_LABEL07                   ' "OVER NG��="

        'checkAutoTeach.Text = MSG_TRIM_06      '�����|�W�V����
        'cmdDataClear.Text = BTN_TRIM_01        '�����ް� �ر
        'cmdReEdit.Text = BTN_TRIM_02           '��ڰ��ް��ҏW
        'cmdGrpInitialTest.Text = BTN_TRIM_03   '�Ƽ��ýĕ��z�\��
        'cmdGrpFinalTest.Text = BTN_TRIM_04     '̧���ýĕ��z�\��

        ' �Ƽ��ý�/̧���ýĕ��z�}���x����ݒ肷��
        'lblRegistTitle.Text = PIC_TRIM_09                   ' "��R��"
        'lblGoodTitle.Text = PIC_TRIM_03                     ' "�Ǖi"
        'lblNgTitle.Text = PIC_TRIM_04                       ' "�s�Ǖi"
        'lblMinTitle.Text = PIC_TRIM_05                      ' "�ŏ�%"
        'lblMaxTitle.Text = PIC_TRIM_06                      ' "�ő�%"
        'lblAverage.Text = PIC_TRIM_07                       ' "����%"
        'lblDeviation.Text = PIC_TRIM_08                     ' "�W���΍�"
    End Sub
#End Region
    '----- ###266�� -----
#Region "�t�H�[���̃{�^�����̐ݒ�(���{��/�p��)"
    '''=========================================================================
    '''<summary>�t�H�[���̃{�^�����̐ݒ�(���{��/�p��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetButtonImage()

        Dim strMSG As String
        'Dim ObjTkyMsg As DllTkyMsgGet.TkyMsgGet                        ' TKY�p���b�Z�[�W�t�@�C�����[�h�I�u�W�F�N�g(DllTkyMsgGet.dll) 
        'Dim strBTN(MAX_FNCNO + 1) As String                            ' �{�^�����̔z��(0 ORG) V1.18.0.1�@ Public�ɕύX
        'Dim Count As Integer

        Try
            '-------------------------------------------------------------------
            '   �t�H�[���̃{�^������Tky_Msg.ini���ݒ肷��(���{��/�p��)
            '-------------------------------------------------------------------
            'ObjTkyMsg = New DllTkyMsgGet.TkyMsgGet                      ' TKY�p���b�Z�[�W�t�@�C�����[�h�I�u�W�F�N�g���� 
            'Count = ObjTkyMsg.Get_Button_Name(gSysPrm.stTMN.giMsgTyp, strBTN)

            strBTN(F_LOAD) = CmdLoad.Text                               ' "���[�h"
            strBTN(F_SAVE) = CmdSave.Text                               ' "�Z�[�u"
            strBTN(F_EDIT) = CmdEdit.Text                               ' "�ҏW"
            strBTN(F_LASER) = CmdLaser.Text                             ' "���[�U"
            strBTN(F_LOG) = CmdLoging.Text                              ' "���M���O"
            strBTN(F_PROBE) = CmdProbe.Text                             ' "�v���[�u"
            strBTN(F_TEACH) = CmdTeach.Text                             ' "�e�B�[�`���O"
            strBTN(F_CUTPOS) = CmdCutPos.Text                           ' "�J�b�g�ʒu�␳"
            strBTN(F_RECOG) = CmdPattern.Text                           ' "�摜�o�^"
            strBTN(MAX_FNCNO) = CmdEnd.Text                             ' "�I��"

            ' CHIP,NET���ʃ{�^��
            strBTN(F_TX) = CmdTx.Text                                   ' "BP�ʒu����"
            strBTN(F_TY) = CmdTy.Text                                   ' "�X�e�[�W�ʒu����"
            strBTN(F_EXR1) = CmdExCam.Text                              ' �O���J�����e�B�[�`���O "1��R"
            strBTN(F_EXTEACH) = CmdExCam1.Text                          ' �O���J�����e�B�[�`���O "�S��R"
            strBTN(F_CARREC) = CmdPtnCalibration.Text                   ' �L�����u���[�V���� "�摜�o�^"
            strBTN(F_CAR) = CmdCalibration.Text                         ' �L�����u���[�V���� "�␳���s"
            strBTN(F_CUTREC) = CmdPtnCutPosCorrect.Text                 ' �O���J�����J�b�g�ʒu "�摜�o�^"
            strBTN(F_CUTREV) = CmdCutPosCorrect.Text                    ' �O���J�����J�b�g�ʒu "�␳���s"

            ' CHIP�{�^��
            strBTN(F_TY2) = CmdTy2.Text                                 ' "�s�x�Q�e�B�[�`���O"
            strBTN(F_TTHETA) = CmdT_Theta.Text                          ' "T��"

            ' SL436R�{�^��
            strBTN(F_AUTO) = CmdAutoOperation.Text                      ' "�����^�]"
            strBTN(F_LOADERINI) = CmdLoaderInit.Text                    ' "���[�_���_���A"

            '----- V1.13.0.0�B�� -----
            ' TKY�n�I�v�V����
            strBTN(F_APROBEREC) = CmdAotoProbePtn.Text                  ' "�摜�o�^"
            strBTN(F_APROBEEXE) = CmdAotoProbeCorrect.Text              ' "���s" 
            strBTN(F_IDTEACH) = CmdIDTeach.Text                         ' "�h�c�e�B�[�`���O"
            strBTN(F_SINSYUKU) = CmdSinsyukuPtn.Text                    ' "�摜�o�^"
            strBTN(F_MAP) = CmdMap.Text                                 ' "MAP"
            '----- V1.13.0.0�B�� -----

            'V4.10.0.0�B                  ��
            strBTN(F_INTEGRATED) = CmdIntegrated.Text                   ' �����o�^����
            grpIntegrated.Text = strBTN(F_INTEGRATED)                   ' �����o�^����
            lblIntegRecog.Text = strBTN(F_RECOG)                        ' �摜�o�^
            lblIntegProbe.Text = strBTN(F_PROBE)                        ' �v���[�u
            lblIntegTX.Text = strBTN(F_TX)                              ' BP�ʒu����
            lblIntegTeach.Text = strBTN(F_TEACH)                        ' �e�B�[�`���O
            lblIntegTY.Text = strBTN(F_TY)                              ' �X�e�[�W�ʒu����
            'V4.10.0.0�B                  ��
            strBTN(F_PROBE_CLEANING) = CmdT_ProbeCleaning.Text          ' "�v���[�u�N���[�j���O"
            strBTN(F_RECOG_ROUGH) = CmdRecogRough.Text                  ' ���t�摜�o�^        'V5.0.0.9�C
            strBTN(F_FOLDEROPEN) = CmdFolderOpen.Text                   ' ̫��ޕ\�� V6.1.4.0�E

            ''-------------------------------------------------------------------
            ''   DIG-SWH��ݒ肷��(���{��/�p��) �������I�ɂ�Tky_Msg.ini���ݒ肷��
            ''-------------------------------------------------------------------
            'CbDigSwH.Items(0) = LBL_FINEADJ_003                         ' "�O�F�\���Ȃ�"
            'CbDigSwH.Items(1) = LBL_FINEADJ_004                         ' "�P�F�m�f�̂ݕ\��"
            'CbDigSwH.Items(2) = LBL_FINEADJ_005                         ' "�Q�F�S�ĕ\��"

            ''-------------------------------------------------------------------
            ''   ���x������ݒ肷��(���{��/�p��) �������I�ɂ�Tky_Msg.ini���ݒ肷��
            ''-------------------------------------------------------------------
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    lblExCamera.Text = "�O���J�����e�B�[�`���O"
            '    lblCalibration.Text = "�L�����u���[�V����"
            '    lblCutPos.Text = "�O���J�����J�b�g�ʒu"
            '    chkDistributeOnOff.Text = "���Y�O���t�@�\��"
            '    lblAutoProbe.Text = "�I�[�g�v���[�u"                    'V1.13.0.0�B
            '    lblSinsyuku.Text = "�L�k�␳"                           'V1.13.0.0�B
            'Else
            '    lblExCamera.Text = "ExCameraTeaching"
            '    lblCalibration.Text = "Calibration"
            '    lblCutPos.Text = "ExCamCutPosition"
            '    chkDistributeOnOff.Text = "Distribute ON"
            '    lblAutoProbe.Text = "Auto Probe"                        'V1.13.0.0�B
            '    lblSinsyuku.Text = "Elastic Compensation"               'V1.13.0.0�B
            'End If

            '' �㏈��
            'ObjTkyMsg = Nothing                                         ' �I�u�W�F�N�g�J��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetButtonImage() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###266�� -----
#Region "�t�H�[���̃R�}���h�{�^����L���E�����ɂ���"
    '''=========================================================================
    '''<summary>�t�H�[���̃R�}���h�{�^����L���E�����ɂ���</summary>
    '''<param name="Flg">(INP) 0=�{�^���񊈐���
    '''                        1=�{�^��������
    '''                        2=�{�^�����̕\��/��\��
    ''' �@�@�@�@�@�@�@�@�@�@�@ 3=�uSAVE�v���݂ƁuEND�v���݂̂ݗL���Ƃ���
    ''' �@�@�@�@�@�@�@�@�@�@�@ 4=�uLOAD�v/�u۷�ݸށv/�u۰�ތ��_���A�v/�uEND�v���݂̂ݗL���Ƃ���
    ''' </param>
    '''=========================================================================
    Public Sub Form1Button(ByVal Flg As Short)

        Dim strMSG As String
        Dim isDispOptGrp As Boolean
        Dim isDispOptGrp2 As Boolean                                    ' V1.13.0.0�B
        Dim isDispOptGrp3 As Boolean = False                            'V4.10.0.0�H

        Try

            If SimpleTrimmer.IsBlockDataDisp() Then                     'V2.0.0.0�I �u���b�N�f�[�^�\�����́A�\�������Ȃ���
                Exit Sub
            End If
            ' �I�v�V�����R���g���[���{�^���̔�\����
            isDispOptGrp = False
            isDispOptGrp2 = False                                       ' V1.13.0.0�B

            ' �uTRIMMING�v�{�^���\��/��\��(�f�o�b�O�p)
            '#If cOFFLINEcDEBUG Then
            'Me.btnTrimming.Visible = True  '###036
            Me.btnTrimming.Visible = False  '###036
            '#Else
            'Me.btnTrimming.Visible = False
            '#End If
            If (Flg = 1) Then
                '---------------------------------------------------------------------------
                '   �{�^��������������
                '---------------------------------------------------------------------------
                Me.btnCounterClear.Enabled = True           ' CLR����
                Me.btnTrimming.Enabled = True               ' GO�{�^��
                Me.CbDigSwH.Enabled = True                  ' DisplayMode�R���{�{�b�N�X���X�g
                Me.CbDigSwL.Enabled = True                  ' MoveMode�R���{�{�b�N�X���X�g
                '----- �R�}���h�{�^���̊����� -----
                Me.CmdEnd.Enabled = True                    ' END���݂͖������Ɋ��������� 
                If (stFNC(F_LOAD).iDEF = 1) Then            ' LOAD����(1:�I����)
                    Me.CmdLoad.Enabled = True
                Else
                    Me.CmdLoad.Enabled = False
                End If
                If (stFNC(F_SAVE).iDEF = 1) Then            ' SAVE����
                    Me.CmdSave.Enabled = True
                Else
                    Me.CmdSave.Enabled = False
                End If
                If (stFNC(F_EDIT).iDEF = 1) Then            ' EDIT����
                    Me.CmdEdit.Enabled = True
                Else
                    Me.CmdEdit.Enabled = False
                End If
                If (stFNC(F_LASER).iDEF = 1) Then           ' LASER����
                    Me.CmdLaser.Enabled = True
                Else
                    Me.CmdLaser.Enabled = False
                End If
                If (stFNC(F_LOG).iDEF = 1) Then             ' Loging����
                    Me.CmdLoging.Enabled = True
                Else
                    Me.CmdLoging.Enabled = False
                End If
                If (stFNC(F_PROBE).iDEF = 1) Then           ' Probe����
                    Me.CmdProbe.Enabled = True
                Else
                    Me.CmdProbe.Enabled = False
                End If
                If (stFNC(F_TEACH).iDEF = 1) Then           ' TEACH����
                    Me.CmdTeach.Enabled = True
                Else
                    Me.CmdTeach.Enabled = False
                End If
                If (stFNC(F_RECOG).iDEF = 1) Then           ' RECOG����
                    Me.CmdPattern.Enabled = True
                Else
                    Me.CmdPattern.Enabled = False
                End If

                ' TKY�p
                ' '###246 If (gTkyKnd = KND_TKY) Then
                If (stFNC(F_CUTPOS).iDEF = 1) Then          ' CUTPOS����
                    Me.CmdCutPos.Enabled = True
                Else
                    Me.CmdCutPos.Enabled = False
                End If
                ' '###246           End If

                '----- V6.1.4.0�I��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
                If (stFNC(F_AUTO).iDEF = 1) Then            ' AUTO�{�^�� (SL432R�ł��L��)
                    Me.CmdAutoOperation.Enabled = True
                Else
                    Me.CmdAutoOperation.Enabled = False
                End If
                '----- V6.1.4.0�I�� -----

                ' CHIP,NET�p
                If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                    If gKeiTyp <> KEY_TYPE_RS Then              'V2.0.0.0�I �V���v���g���}�ȊO
                        Me.chkDistributeOnOff.Enabled = True
                        Me.chkDistributeOnOff.Visible = True
                    End If                                      'V2.0.0.0�I

                    If (stFNC(F_TX).iDEF = 1) Then              ' TX����
                        Me.CmdTx.Enabled = True
                    Else
                        Me.CmdTx.Enabled = False
                    End If
                    If (stFNC(F_TY).iDEF = 1) Then              ' TY����
                        Me.CmdTy.Enabled = True
                    Else
                        Me.CmdTy.Enabled = False
                    End If

                    Me.CmdIntegrated.Enabled = (1 <= stFNC(F_INTEGRATED).iDEF)  ' �����o�^�����{�^�� 'V4.10.0.0�B

                    'Option functions
                    If (stFNC(F_TY2).iDEF = 1) Then             ' TY2����
                        Me.CmdTy2.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdTy2.Enabled = False
                    End If
                    If (stFNC(F_EXR1).iDEF = 1) Then         ' �O�����R1è��ݸ�����  
                        Me.CmdExCam.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdExCam.Enabled = False
                    End If
                    If (stFNC(F_EXTEACH).iDEF = 1) Then         ' �O�����è��ݸ�����  
                        Me.CmdExCam1.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdExCam1.Enabled = False
                    End If
                    If (Me.CmdExCam1.Enabled = False And Me.CmdExCam.Enabled = False) Then
                        '���x���\���������ɂ���
                        lblExCamera.Visible = False
                    Else
                        lblExCamera.Visible = True              ' ###103
                    End If
                    If (stFNC(F_CARREC).iDEF = 1) Then          ' �����ڰ��ݕ␳�o�^����  
                        Me.CmdPtnCalibration.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdPtnCalibration.Enabled = False
                    End If
                    If (stFNC(F_CAR).iDEF = 1) Then             ' �����ڰ�������
                        Me.CmdCalibration.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdCalibration.Enabled = False
                    End If
                    If (Me.CmdPtnCalibration.Enabled = False And Me.CmdCalibration.Enabled = False) Then
                        '���x���\���������ɂ���
                        lblCalibration.Visible = False
                    Else
                        lblCalibration.Visible = True           ' ###103
                    End If
                    If (stFNC(F_CUTREC).iDEF = 1) Then          ' ��ĕ␳�o�^����
                        Me.CmdPtnCutPosCorrect.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdPtnCutPosCorrect.Enabled = False
                    End If
                    If (stFNC(F_CUTREV).iDEF = 1) Then          ' ��Ĉʒu�␳����
                        Me.CmdCutPosCorrect.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdCutPosCorrect.Enabled = False
                    End If
                    If (Me.CmdPtnCutPosCorrect.Enabled = False And Me.CmdCutPosCorrect.Enabled = False) Then
                        '���x���\���������ɂ���
                        lblCutPos.Visible = False
                    Else
                        lblCutPos.Visible = True                ' ###103
                    End If
                    If (stFNC(F_TTHETA).iDEF = 1) Then          ' T��è��ݸ�����
                        Me.CmdT_Theta.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdT_Theta.Enabled = False
                    End If
                End If
                '#4.12.3.0�C��
                'TKY�ł�SL436�^�C�v�Ŏ����^�]����ꍇ������̂ł��̏�����CHIP�ANET�̎�ʂ���O���đS�����ʂƂ���
                ' SL436R��p�̃R�}���h
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    If (stFNC(F_AUTO).iDEF = 1) Then            ' AUTO�{�^��
                        Me.CmdAutoOperation.Enabled = True
                    Else
                        Me.CmdAutoOperation.Enabled = False
                    End If
                    If (stFNC(F_LOADERINI).iDEF = 1) Then       ' LOADER INI�{�^��
                        Me.CmdLoaderInit.Enabled = True
                    Else
                        Me.CmdLoaderInit.Enabled = False
                    End If
                End If
                '#4.12.3.0�C��

                '----- V1.13.0.0�B�� -----
                ' TKY�n�I�v�V����
                If (stFNC(F_APROBEREC).iDEF = 1) Then                   ' ��ăv���[�u�o�^���� 
                    Me.CmdAotoProbePtn.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbePtn.Enabled = False
                End If
                If (stFNC(F_APROBEEXE).iDEF = 1) Then                   ' ��ăv���[�u���s����
                    Me.CmdAotoProbeCorrect.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbeCorrect.Enabled = False
                End If
                If (Me.CmdAotoProbePtn.Enabled = False And Me.CmdAotoProbeCorrect.Enabled = False) Then
                    '���x���\���������ɂ���
                    lblAutoProbe.Visible = False
                Else
                    lblAutoProbe.Visible = True
                End If
                If (stFNC(F_IDTEACH).iDEF = 1) Then                     ' IDè��ݸ�����
                    Me.CmdIDTeach.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdIDTeach.Enabled = False
                End If
                If (Me.CmdIDTeach.Enabled = False) Then
                    '���x���\���������ɂ���
                    lblSinsyuku.Visible = False
                Else
                    lblSinsyuku.Visible = True
                End If
                If (stFNC(F_SINSYUKU).iDEF = 1) Then                    ' �L�k�o�^���� 
                    Me.CmdSinsyukuPtn.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdSinsyukuPtn.Enabled = False
                End If
                If (stFNC(F_SINSYUKU).iDEF = 1) Then                    ' MAP���� 
                    Me.CmdSinsyukuPtn.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdMap.Enabled = False
                End If
                '----- V1.13.0.0�B�� -----

                'V4.1.0.0�D
                If stFNC(F_PROBE_CLEANING).iDEF >= 1 Then
                    ProbeBtnEnableSet(True)
                    Me.CmdT_ProbeCleaning.Enabled = True                'V4.10.0.0�H
                    'V5.0.0.9�K��
                    If (gMachineType = MACHINE_TYPE_436S) Then
                    Else
                        isDispOptGrp3 = True                                'V4.10.0.0�H
                    End If
                    'V5.0.0.9�K��
                Else
                    ProbeBtnEnableSet(False)
                End If
                'V4.1.0.0�D
                'V4.1.0.0�E
                If stFNC(F_MAINTENANCE).iDEF >= 1 Then
                    MaintBtnEnableSet(True)
                Else
                    MaintBtnEnableSet(False)
                End If
                'V4.1.0.0�E

                'V6.1.4.0�E         ��
                If (1S <= stFNC(F_FOLDEROPEN).iDEF) Then                ' ̫��ޕ\������
                    Me.CmdFolderOpen.Enabled = True
                    isDispOptGrp = True
                Else
                    Me.CmdFolderOpen.Enabled = False
                End If
                'V6.1.4.0�E         ��

                ' ���������͕\������
                mnuHelpAbout.Visible = True
                btnGoClipboard.Visible = False                          '###036
                Me.tabCmd.Visible = True
                Me.CmdEnd.Visible = True
                '----- V1.18.0.0�B�� -----
                'If gSysPrm.stCTM.giGP_IB_flg <> 0 Then                  ' GP-IB����@�\���� ? '###006
                '    If (giIX2LOG = 1) Then                              '###231
                '        CMdIX2Log.Visible = True                        '�uIX2Log On/OFF�v���ݕ\��
                '    End If
                'End If
                If (giIX2LOG = 1) Then                                  ' IX2Log�{�^���L�� ?
                    Me.CMdIX2Log.Enabled = True
                Else
                    Me.CMdIX2Log.Enabled = False
                End If
                If (giPrint = 1) Then                                   ' Print�{�^���L�� ?
                    Me.BtnPrint.Enabled = True
                    Me.BtnPrintOnOff.Enabled = True                     ' Print On/Off�{�^��
                Else
                    Me.BtnPrint.Enabled = False
                    Me.BtnPrintOnOff.Enabled = False
                End If
                '----- V1.18.0.0�B�� -----
                If gSysPrm.stLOG.gEsLogUse = 1 Then                     ' ES���O�g�p�� ? '###006
                    cmdEsLog.Visible = True                             ' ES۸����ݕ\��
                End If

                ' �}�K�W���㉺����@�L��
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R�n ? ###182
                    GroupBox1.Visible = True
                End If

                'V4.7.3.5�@��
                If (gTkyKnd = KND_CHIP) Then
                    CutOffEsEditButton.Visible = True
                    CutOffEsEditButton.Enabled = True
                Else
                    CutOffEsEditButton.Visible = False
                    CutOffEsEditButton.Enabled = False
                End If
                'V4.7.3.5�@��
                'V6.1.4.14�@��
                If (gTkyKnd = KND_NET) Then
                    CutOffEsEditButtonNET.Visible = True
                    CutOffEsEditButtonNET.Enabled = True
                Else
                    CutOffEsEditButtonNET.Visible = False
                    CutOffEsEditButtonNET.Enabled = False
                End If
                'V6.1.4.14�@��

            ElseIf (Flg = 0) Then
                '---------------------------------------------------------------------------
                '   �{�^����񊈐�������
                '---------------------------------------------------------------------------
                Me.btnCounterClear.Enabled = False          ' CLR����
                Me.btnTrimming.Enabled = False              ' GO�{�^��
                Me.CbDigSwH.Enabled = False                 ' DisplayMode�R���{�{�b�N�X���X�g
                Me.CbDigSwL.Enabled = False                 ' MoveMode�R���{�{�b�N�X���X�g
                '----- �R�}���h�{�^���̔񊈐��� -----
                Me.CmdEnd.Enabled = False
                Me.CmdLoad.Enabled = False
                Me.CmdSave.Enabled = False
                Me.CmdEdit.Enabled = False
                Me.CmdLaser.Enabled = False
                Me.CmdLoging.Enabled = False
                Me.CmdProbe.Enabled = False
                Me.CmdTeach.Enabled = False
                Me.CmdPattern.Enabled = False
                Me.CmdCutPos.Enabled = False
                ' CHIP,NET�p
                Me.chkDistributeOnOff.Enabled = False
                Me.chkDistributeOnOff.Visible = False
                Me.CmdTx.Enabled = False
                Me.CmdTy.Enabled = False
                Me.CmdExCam.Enabled = False
                Me.CmdExCam1.Enabled = False
                Me.CmdPtnCalibration.Enabled = False
                Me.CmdCalibration.Enabled = False
                Me.CmdPtnCutPosCorrect.Enabled = False
                Me.CmdCutPosCorrect.Enabled = False
                Me.CmdT_Theta.Enabled = False
                Me.CmdTy2.Enabled = False
                Me.CmdAutoOperation.Enabled = False         ' AUTO�{�^�� 
                Me.CmdLoaderInit.Enabled = False            ' LOADER INI�{�^��
                Me.CmdIntegrated.Enabled = False            ' �����o�^�����{�^�� 'V4.10.0.0�B
                '----- V1.13.0.0�B�� -----
                ' TKY�n�I�v�V����
                Me.CmdAotoProbePtn.Enabled = False                      ' ��ăv���[�u�o�^����
                Me.CmdAotoProbeCorrect.Enabled = False                  ' ��ăv���[�u���s����
                Me.CmdIDTeach.Enabled = False                           ' IDè��ݸ�����
                Me.CmdSinsyukuPtn.Enabled = False                       ' �L�k�o�^���� 
                '----- V1.13.0.0�B�� -----
                '----- V1.18.0.0�B�� -----
                ' ���[���a�Ή�
                Me.CMdIX2Log.Enabled = False                            ' IX2Log�{�^��
                Me.BtnPrint.Enabled = False                             ' Print�{�^��
                Me.BtnPrintOnOff.Enabled = False                        ' Print On/Off�{�^��
                '----- V1.18.0.0�B�� -----

                '�������͕\��������
                mnuHelpAbout.Visible = False
                btnGoClipboard.Visible = False
                'CMdIX2Log.Visible = False                   ' V1.13.0.0�B ###006
                cmdEsLog.Visible = False                    ' ###006
                Me.tabCmd.Visible = False
                Me.CmdEnd.Visible = False

                ' �}�K�W���㉺����@����
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R�n ? ###182
                    GroupBox1.Visible = False
                End If

                ' ���O�I�����[�U�[�\���E�ؑփ{�^��
                SetBtnUserLogon(False)

                Me.CmdT_ProbeCleaning.Enabled = False       'V4.10.0.0�H

                '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
                If (giQrCodeType = QrCodeType.KoaEw) Then
                    ' �p�q�ǂݍ��ݏ�Ԃ̏�����
                    ObjQRCodeReader.ResetQRReadFlag()                   ' �p�q�R�[�h���ǂݍ��ݏ�ԂɃ��Z�b�g
                End If
                '----- V6.1.4.0_22�� -----

                'V4.7.3.5�@��
                If (gTkyKnd = KND_CHIP) Then
                    CutOffEsEditButton.Enabled = False
                Else
                    CutOffEsEditButton.Visible = False
                    CutOffEsEditButton.Enabled = False
                End If
                'V4.7.3.5�@��
                'V6.1.4.14�@��
                If (gTkyKnd = KND_NET) Then
                    CutOffEsEditButtonNET.Enabled = False
                Else
                    CutOffEsEditButtonNET.Visible = False
                    CutOffEsEditButtonNET.Enabled = False
                End If
                'V6.1.4.14�@��

            ElseIf (Flg = 2) Then
                '---------------------------------------------------------------------------
                '   �{�^���̕\��/��\����ݒ肷��(ctl��bit1 ON��)
                '---------------------------------------------------------------------------
                Me.CmdEnd.Visible = True                    ' END���݂͖������ɕ\������ 
                Me.btnTrimming.Visible = False              ' GO�{�^��
                If (stFNC(F_LOAD).iDEF >= 0) Then           ' LOAD����
                    Me.CmdLoad.Visible = True
                Else
                    Me.CmdLoad.Visible = False
                End If
                If (stFNC(F_SAVE).iDEF >= 0) Then           ' SAVE����
                    Me.CmdSave.Visible = True
                Else
                    Me.CmdSave.Visible = False
                End If
                If (stFNC(F_EDIT).iDEF >= 0) Then           ' EDIT����
                    Me.CmdEdit.Visible = True
                Else
                    Me.CmdEdit.Visible = False
                End If
                If (stFNC(F_LASER).iDEF >= 0) Then          ' LASER����
                    Me.CmdLaser.Visible = True
                Else
                    Me.CmdLaser.Visible = False
                End If
                If (stFNC(F_LOG).iDEF >= 0) Then            ' Loging����
                    Me.CmdLoging.Visible = True
                Else
                    Me.CmdLoging.Visible = False
                End If
                If (stFNC(F_PROBE).iDEF >= 0) Then          ' Probe����
                    Me.CmdProbe.Visible = True
                Else
                    Me.CmdProbe.Visible = False
                    Me.lblIntegProbe.Visible = False        'V4.10.0.0�B
                End If
                If (stFNC(F_TEACH).iDEF >= 0) Then          ' TEACH����
                    Me.CmdTeach.Visible = True
                Else
                    Me.CmdTeach.Visible = False
                    Me.lblIntegTeach.Visible = False        'V4.10.0.0�B
                End If
                If (stFNC(F_RECOG).iDEF >= 0) Then          ' RECOG����
                    If (gSysPrm.stDEV.giTheta = 0) Then     ' �ƂȂ� ? 
                        Me.CmdPattern.Visible = False
                        Me.lblIntegRecog.Visible = False    'V4.10.0.0�B
                    Else
                        Me.CmdPattern.Visible = True
                    End If
                Else
                    Me.CmdPattern.Visible = False
                    Me.lblIntegRecog.Visible = False        'V4.10.0.0�B
                End If
                ' TKY�p
                ''###246 If (gTkyKnd = KND_TKY) Then
                If (stFNC(F_CUTPOS).iDEF >= 0) Then         ' CUTPOS����
                    Me.CmdCutPos.Visible = True
                Else
                    Me.CmdCutPos.Visible = False
                End If
                ''###246  End If

                '----- V1.13.0.0�B�� -----
                ' TKY�n�I�v�V����
                If (stFNC(F_APROBEREC).iDEF >= 0) Then              ' ��ăv���[�u�o�^���� 
                    Me.CmdAotoProbePtn.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbePtn.Visible = False
                End If
                If (stFNC(F_APROBEEXE).iDEF >= 0) Then              ' ��ăv���[�u���s����
                    Me.CmdAotoProbeCorrect.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbeCorrect.Visible = False
                End If
                If (stFNC(F_IDTEACH).iDEF >= 0) Then                ' IDè��ݸ�����
                    Me.CmdIDTeach.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdIDTeach.Visible = False
                End If
                If (stFNC(F_SINSYUKU).iDEF >= 0) Then               ' �L�k�o�^���� 
                    Me.CmdSinsyukuPtn.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdSinsyukuPtn.Visible = False
                End If
                If (stFNC(F_MAP).iDEF >= 0) Then                    ' MAP���� 
                    Me.CmdMap.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdMap.Visible = False
                End If
                '----- V1.13.0.0�B�� -----
                '----- V6.1.4.0�I��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
                If (stFNC(F_AUTO).iDEF >= 0) Then               ' AUTO�{�^�� (SL432R�ł��L��)
                    Me.CmdAutoOperation.Visible = True
                Else
                    Me.CmdAutoOperation.Visible = False
                End If
                '----- V6.1.4.0�I�� -----

                ' CHIP,NET�p
                If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                    If gKeiTyp <> KEY_TYPE_RS Then              'V2.0.0.0�I �V���v���g���}�ȊO
                        Me.chkDistributeOnOff.Visible = True
                    End If                                      'V2.0.0.0�I

                    If (stFNC(F_TX).iDEF >= 0) Then             ' TX����
                        Me.CmdTx.Visible = True
                    Else
                        Me.CmdTx.Visible = False
                        Me.lblIntegTX.Visible = False           'V4.10.0.0�B
                    End If
                    If (stFNC(F_TY).iDEF >= 0) Then             ' TY����
                        Me.CmdTy.Visible = True
                    Else
                        Me.CmdTy.Visible = False
                        Me.lblIntegTY.Visible = False           'V4.10.0.0�B
                    End If
                    If (stFNC(F_TY2).iDEF >= 0) Then            ' TY2����
                        Me.CmdTy2.Visible = True
                        isDispOptGrp = True
                    Else
                        Me.CmdTy2.Visible = False
                    End If

                    ' �����o�^�������� 'V4.10.0.0�B     ��
                    If (0 <= stFNC(F_INTEGRATED).iDEF) AndAlso ((0 <= stFNC(F_PROBE).iDEF) OrElse
                        (0 <= stFNC(F_TEACH).iDEF) OrElse (0 <= stFNC(F_TX).iDEF) OrElse
                        (0 <= stFNC(F_TY).iDEF) OrElse (0 <= stFNC(F_RECOG).iDEF)) Then

                        Me.CmdIntegrated.Visible = True
                    Else
                        Me.CmdIntegrated.Visible = False
                    End If
                    ' �����o�^�������� 'V4.10.0.0�B     ��

                    ' �O���J�����L���ɂ��\���^��\��(CHIP/NET��)��ݒ肷��
                    If (gSysPrm.stDEV.giEXCAM = 0) Then
                        ' �O���J�������Ȃ��ꍇ,���L�̃{�^���͔�\���Ƃ���
                        Me.CmdExCam.Visible = False             ' �O�����R1è��ݸ�����   
                        Me.CmdExCam1.Visible = False            ' �O�����è��ݸ�����
                        Me.CmdPtnCalibration.Visible = False    ' �����ڰ��ݕ␳�o�^����
                        Me.CmdCalibration.Visible = False       ' �����ڰ�������
                        Me.CmdPtnCutPosCorrect.Visible = False  ' ��ĕ␳�o�^���� 
                        Me.CmdCutPosCorrect.Visible = False     ' ��Ĉʒu�␳���� 
                        Me.CmdT_Theta.Visible = False           ' T��è��ݸ����� 
                    Else
                        ' �O���J����������ꍇ,�ݒ�t�@�C���̎w��ɂ��\���^��\��
                        If (stFNC(F_EXR1).iDEF >= 0) Then    ' �O�����R1è��ݸ�����  
                            Me.CmdExCam.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdExCam.Visible = False
                        End If
                        If (stFNC(F_EXTEACH).iDEF >= 0) Then    ' �O�����è��ݸ�����  
                            Me.CmdExCam1.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdExCam1.Visible = False
                        End If
                        If (stFNC(F_CARREC).iDEF >= 0) Then     ' �����ڰ��ݕ␳�o�^����  
                            Me.CmdPtnCalibration.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdPtnCalibration.Visible = False
                        End If
                        If (stFNC(F_CAR).iDEF >= 0) Then        ' �����ڰ�������
                            Me.CmdCalibration.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdCalibration.Visible = False
                        End If
                        If (stFNC(F_CUTREC).iDEF >= 0) Then     ' ��ĕ␳�o�^����
                            Me.CmdPtnCutPosCorrect.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdPtnCutPosCorrect.Visible = False
                        End If
                        If (stFNC(F_CUTREV).iDEF >= 0) Then     ' ��Ĉʒu�␳����
                            Me.CmdCutPosCorrect.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdCutPosCorrect.Visible = False
                        End If
                        If (stFNC(F_TTHETA).iDEF >= 0) Then     ' T��è��ݸ�����
                            If (gSysPrm.stDEV.giTheta = 0) Then ' �ƂȂ� ? 
                                Me.CmdT_Theta.Visible = False
                            Else
                                Me.CmdT_Theta.Visible = True
                                isDispOptGrp = True
                            End If
                        Else
                            Me.CmdT_Theta.Visible = False
                        End If
                        If ((stFNC(F_RECOG_ROUGH).iDEF) >= 0) Then  ' �̱���ėp�摜�o�^����  'V5.0.0.9�C  ��
                            If (0 <> giClampLessStage) AndAlso (0 <> gSysPrm.stDEV.giTheta) Then ' �N�����v���X�ڕ��� ? 
                                Me.CmdRecogRough.Visible = True
                                ' isDispOptGrp = True
                            Else
                                Me.CmdRecogRough.Visible = False
                            End If
                        End If                                                              'V5.0.0.9�C  ��
                    End If
                End If

                '#4.12.3.0�C��
                'TKY�ł�SL436�^�C�v�Ŏ����^�]����ꍇ������̂ł��̏�����CHIP�ANET�̎�ʂ���O���đS�����ʂƂ���
                ' SL436R��p�̃R�}���h
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    If (stFNC(F_AUTO).iDEF >= 0) Then            ' AUTO�{�^�� 
                        Me.CmdAutoOperation.Visible = True
                    Else
                        Me.CmdAutoOperation.Visible = False
                    End If
                    If (stFNC(F_LOADERINI).iDEF >= 0) Then      ' LOADER INI�{�^��
                        Me.CmdLoaderInit.Visible = True
                    Else
                        Me.CmdLoaderInit.Visible = False
                    End If
                End If
                '#4.12.3.0�C��

                '----- V1.18.0.0�B�� -----
                ' ���[���a�Ή�
                If (giIX2LOG = 1) Then                                  ' IX2Log�{�^���L�� ?
                    Me.CMdIX2Log.Visible = True
                Else
                    Me.CMdIX2Log.Visible = False
                End If
                If (giPrint = 1) Then                                   ' Print�{�^���L�� ?
                    Me.BtnPrint.Visible = True
                    Me.BtnPrintOnOff.Visible = True                     ' Print On/Off�{�^��
                Else
                    Me.BtnPrint.Visible = False
                    Me.BtnPrintOnOff.Visible = False
                End If
                '----- V1.18.0.0�B�� -----

                'V4.1.0.0�D
                If stFNC(F_PROBE_CLEANING).iDEF >= 1 Then
                    ProbeBtnVisibleSet(True)
                    Me.CmdT_ProbeCleaning.Enabled = True                'V4.10.0.0�H
                    Me.CmdT_ProbeCleaning.Visible = True                'V4.10.0.0�H
                    'V5.0.0.9�K��
                    If (gMachineType = MACHINE_TYPE_436S) Then
                    Else
                        isDispOptGrp3 = True                                'V4.10.0.0�H
                    End If
                    'V5.0.0.9�K��
                Else
                    ProbeBtnVisibleSet(False)
                End If
                'V4.1.0.0�D
                'V4.1.0.0�E
                If stFNC(F_MAINTENANCE).iDEF >= 1 Then
                    MaintBtnVisibleSet(True)
                Else
                    MaintBtnVisibleSet(False)
                End If
                'V4.1.0.0�E

                'V6.1.4.0�E         ��
                If (1S <= stFNC(F_FOLDEROPEN).iDEF) Then                ' �t�H���_�\���{�^��
                    Me.CmdFolderOpen.Visible = True
                    Me.lblProductionData.Visible = True
                    isDispOptGrp = True
                Else
                    Me.CmdFolderOpen.Visible = False
                    Me.lblProductionData.Visible = False
                End If
                'V6.1.4.0�E         ��

                '---------------------------------------------------------------------------
                '   �I�v�V�����O���[�v�̕\��/��\���ݒ�
                '---------------------------------------------------------------------------
                If (isDispOptGrp <> True) Then
                    Me.tabCmd.TabPages.Remove(Me.tabOptCmnds)
                End If
                '----- V1.13.0.0�B�� -----
                ' TKY�n�I�v�V����
                If (isDispOptGrp2 <> True) Then
                    Me.tabCmd.TabPages.Remove(Me.tabOptCmnd2)
                End If
                '----- V1.13.0.0�B�� -----
                'V4.10.0.0�H��
                'V5.0.0.4�G                If (isDispOptGrp3 <> True) Then
                'V6.0.0.1�B                If (isDispOptGrp3 <> True) AndAlso (gMachineType = MACHINE_TYPE_436S) Then  'V5.0.0.9�K
                If (isDispOptGrp3 <> True) OrElse (gMachineType = MACHINE_TYPE_436S) Then      'V6.0.0.1�B
                    Me.tabCmd.TabPages.Remove(Me.tabOptCmnd3)
                End If
                'V5.0.0.4�G            End If
                'V4.10.0.0�H��

                'V4.7.3.5�@��
                If (gTkyKnd = KND_CHIP) Then
                    CutOffEsEditButton.Visible = True
                    CutOffEsEditButton.Enabled = True
                Else
                    CutOffEsEditButton.Visible = False
                    CutOffEsEditButton.Enabled = False
                End If
                'V4.7.3.5�@��
                'V6.1.4.14�@��
                If (gTkyKnd = KND_NET) Then
                    CutOffEsEditButtonNET.Visible = True
                    CutOffEsEditButtonNET.Enabled = True
                Else
                    CutOffEsEditButtonNET.Visible = False
                    CutOffEsEditButtonNET.Enabled = False
                End If
                'V6.1.4.14�@��

            ElseIf (Flg = 3) Then
                '---------------------------------------------------------------------------
                '   �uSAVE�v���݂ƁuEND�v���݂̂ݗL���Ƃ���
                '---------------------------------------------------------------------------
                Me.CmdSave.Enabled = True                   '�uSAVE�v����
                Me.CmdSave.Visible = True
                Me.CmdEnd.Enabled = True                    '�uEND�v����
                Me.CmdEnd.Visible = True
                Me.btnCounterClear.Enabled = True           '�uCLR�v����
                Me.btnCounterClear.Visible = True

            ElseIf (Flg = 4) Then
                '---------------------------------------------------------------------------
                '   �uLOAD�v/�u۷�ݸށv/�u۰�ތ��_���A�v/�uEND�v���݂̂ݗL���Ƃ���
                '---------------------------------------------------------------------------
                Me.btnTrimming.Visible = False                          ' GO�{�^��
                Me.CmdSave.Visible = False
                Me.CmdEdit.Visible = False
                Me.CmdLaser.Visible = False
                Me.CmdProbe.Visible = False
                Me.CmdTeach.Visible = False
                Me.CmdPattern.Visible = False
                Me.CmdCutPos.Visible = False
                ' CHIP,NET�p
                Me.CmdTx.Visible = False
                Me.CmdTy.Visible = False
                Me.CmdExCam.Visible = False
                Me.CmdExCam1.Visible = False
                Me.CmdPtnCalibration.Visible = False
                Me.CmdCalibration.Visible = False
                Me.CmdPtnCutPosCorrect.Visible = False
                Me.CmdCutPosCorrect.Visible = False
                Me.CmdT_Theta.Visible = False
                Me.CmdTy2.Visible = False
                Me.CmdAutoOperation.Visible = False                     ' AUTO�{�^�� 
                '----- V1.13.0.0�B�� -----
                ' TKY�n�I�v�V����
                Me.CmdAotoProbePtn.Visible = False                      ' ��ăv���[�u�o�^����
                Me.CmdAotoProbeCorrect.Visible = False                  ' ��ăv���[�u���s����
                Me.CmdIDTeach.Visible = False                           ' IDè��ݸ�����
                Me.CmdSinsyukuPtn.Visible = False                       ' �L�k�o�^���� 
                '----- V1.13.0.0�B�� -----
                '----- V1.18.0.0�B�� -----
                ' ���[���a�Ή�
                Me.CMdIX2Log.Visible = False                            ' IX2Log�{�^��
                Me.BtnPrint.Visible = False                             ' Print�{�^��
                Me.BtnPrintOnOff.Visible = False                        ' Print On/Off�{�^��
                '----- V1.18.0.0�B�� -----
                Me.CmdT_ProbeCleaning.Visible = False                   'V4.10.0.0�H

                '�uLOAD�v/�u۷�ݸށv/�u۰�ތ��_���A�v/�uEND�v���݂̂ݗL���Ƃ���
                Me.CmdLoad.Enabled = True
                Me.CmdLoad.Visible = True
                Me.CmdEnd.Enabled = True                    '�uEND�v����
                Me.CmdEnd.Visible = True
                Me.CmdLoging.Enabled = True                 '�uLoging�v����
                Me.CmdLoging.Visible = True
                Me.CmdLoaderInit.Enabled = True             '�uLOADER INI�v����
                Me.CmdLoaderInit.Visible = True
            End If
            'V2.0.0.0�I ADD START
            If Flg <> 0 Then
                Call SimpleTrimmer.ResistorDataDisp(False, 0, 0)
            End If
            'V2.0.0.0�I ADD END

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Form1Button() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[����\��/��\���ɐݒ肷��"
    '''=========================================================================
    '''<summary>�摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[����\��/��\���ɐݒ肷��</summary>
    '''<param name="Flg">(INP)True=�\��, False=��\��</param>
    '''<remarks>Video.OCX�͍Ŕw�ʂɂ���ׁA���C���t�H�[���̃R���g���[���̉�
    ''' �@�@�@�@�ɓo�^��ʂ��\������邽��</remarks>
    '''=========================================================================
    Public Sub SetVisiblePropForVideo(ByVal Flg As Boolean)

        Dim strMSG As String

        Try
            ' �摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[����\��/��\���ɐݒ肷��
            txtLog.Visible = Flg
            '' '' ''grpCmd.Visible = Flg
            GrpMode.Visible = Flg
            'btnTrimming.Visible = Flg '###036

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetVisiblePropForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���[�U�p���[�����֘A���ڂ̕\��/��\���ݒ�"
    '''=========================================================================
    '''<summary>���[�U�p���[�����֘A���ڂ̕\��/��\���ݒ� ###029</summary>
    ''' <param name="Md">(INP)0=�\�����Ȃ�, 1=�\������</param>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetLaserItemsVisible(ByVal Md As Integer)

        Dim strMSG As String

        Try
            ' ���������V�X�p�����\������("������ = 99.9%")
            Me.LblRotAtt.Visible = False                                ' ��������\��
            If (Md = 1) Then                                            ' �\������ ?
                If (gSysPrm.stRMC.giRmCtrl2 >= 2 And
                    gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then       ' ۰�ر��Ȱ�����L��FL�łȂ� ?
                    '----- V1.16.0.0�M�� -----
                    'Me.LblRotAtt.Visible = True
                    ' �������̕\��/��\�����V�X�p�����ݒ肷��
                    If (giNoDspAttRate = 1) Then
                        Me.LblRotAtt.Visible = False
                    Else
                        Me.LblRotAtt.Visible = True
                    End If
                    '----- V1.16.0.0�M�� -----
                End If

                '' FL���̃I�[�g�p���[�����p�d���l�\��(FL���͌������̑���ɓd���l��\������) ###066
                '' ��FL�Ńp���[���[�^�̃f�[�^�擾�^�C�v���u�h�^�n�ǎ��v/�u�t�r�a�v�Łu�p���[����l�\������v�̏ꍇ�ɕ\������
                'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And _
                '   (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                '    Me.LblRotAtt.Visible = True
                'End If
            End If

            ' ����l���V�X�p�����\������
            Me.LblMes.Visible = False                                   ' ����l��\��
            If (Md = 1) Then                                            ' �\������ ?
                ' RMCTRL2 >=3 �� ����l�\�� ?
                If (gSysPrm.stRMC.giRmCtrl2 >= 3) And (gSysPrm.stRMC.giPMonHi = 1) Then
                    LblMes.Visible = True                              ' ����l�\��
                End If

                '' ���[�U�p���[�ݒ�l�\��(FL��) ###066
                '' ��FL�Ńp���[���[�^�̃f�[�^�擾�^�C�v���u�h�^�n�ǎ��v/�u�t�r�a�v�Łu�p���[����l�\������v�̏ꍇ�ɕ\������
                'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And _
                '   (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                '    LblMes.Visible = True                               ' �ݒ�l�\��
                'End If
            End If

            ' ��d���l��\������
            LblCur.Visible = False                                      ' ��d���l��\��
            If (Md = 1) Then                                            ' �\������ ?
                ' ���H�d�͐ݒ� = 4(��d��1A)�̎��ɕ\��
                'If (gSysPrm.stSPF.giProcPower = 4) And (gSysPrm.stSPF.giProcPower2 <> 0) Then  '###250
                If (gSysPrm.stSPF.giProcPower2 <> 0) Then                                       '###250
                    LblCur.Visible = True
                End If
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "SetLaserItemsVisible() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�p�X���[�h�`�F�b�N"
    '''=========================================================================
    '''<summary>�p�X���[�h�`�F�b�N</summary>
    '''<param name="IntIndexNo">(INP)�@�\�I���`�e�[�u���̲��ޯ��</param>
    '''<returns>True=����, False=�G���[</returns>
    '''=========================================================================
    Public Function Func_Password(ByRef IntIndexNo As Short) As Short

        Dim r As Short

        Func_Password = True
        If (stFNC(IntIndexNo).iPAS = 1) Then
            'V6.1.4.9�G���z�}���\������Ă������\���ɂ��遫
            Call RedrawDisplayDistribution(True)    '���v�\�������̏�ԕύX'V6.1.4.9�G
            r = Password1.ShowDialog((gSysPrm.stTMN.giMsgTyp), (gSysPrm.stSYP.gstrPassword))
            If (r <> 1) Then
                Func_Password = False                   ' �߽ܰ�ޓ��ʹװ�Ȃ�EXIT
                'V4.7.3.5�@��
                If r <> cFRS_NORMAL And r <> cFRS_ERR_RST Then
                    Call Me.AppEndDataSave()
                    Call Me.AplicationForcedEnding()                         ' �A�v�������I��
                End If
                'V4.7.3.5�@��
            End If
            Call RedrawDisplayDistribution(False)   'V6.1.4.9�G
        End If

    End Function
#End Region

#Region "�\�t�g�E�F�A�L�[�{�[�h�̋N������"
    '#If cSOFTKYBOARDcUSE = 1 Then
    '''=========================================================================
    '''<summary>�\�t�g�E�F�A�L�[�{�[�h�̋N������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub StartSoftwareKeyBoard(ByRef ps As Process)
        Try
            If (giDspScreenKeybord = 1) Then Return ' �N���[���L�[�{�[�h�\�����Ȃ��Ȃ�NOP V2.0.0.0�F(V1.22.0.0�G)
            ps.StartInfo.FileName = "osk.exe"
            ps.Start()
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "iTKY.StartSoftwareKeyBoard() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�\�t�g�E�F�A�L�[�{�[�h�̏I������"
    '''=========================================================================
    '''<summary>�\�t�g�E�F�A�L�[�{�[�h�̏I������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub EndSoftwareKeyBoard(ByRef ps As Process)
        Try
            If (giDspScreenKeybord = 1) Then Return ' �N���[���L�[�{�[�h�\�����Ȃ��Ȃ�NOP V2.0.0.0�F(V1.22.0.0�G)
            '----- ###259��-----
            ' �v���Z�X�I�����b�Z�[�W���M(����޳�̂�����؂̏ꍇ) 
            If (ps.CloseMainWindow() = False) Then      ' Ҳݳ���޳�ɸ۰��ү���ނ𑗐M����
                ps.Kill()                               ' �I�����Ȃ������ꍇ�͋����I������
            End If

            ' �v���Z�X�̏I����҂�
            Do
                System1.WAIT(0.1)                       ' Wait(Sec) 
            Loop While (ps.HasExited <> True)           ' �v���Z�X���I�����Ă���ꍇ�̂�True���Ԃ�

            ' �I�u�W�F�N�g�J��
            ps.Close()                                  ' �I�u�W�F�N�g�J��
            ps.Dispose()                                ' ���\�[�X�J��

            'If (ps.HasExited <> True) Then
            '    ps.Kill()
            'End If
            '----- ###259��-----

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "iTKY.EndSoftwareKeyBoard() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
    '#End If
#End Region

    '========================================================================================
    '   �R�}���h�{�^���������̏���
    '========================================================================================
#Region "̧��۰��(F1)���݉���������"
    '''=========================================================================
    '''<summary>̧��۰��(F1)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdLoad_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLoad.Click

        Dim sPath As String
        Dim r As Integer
        Dim strMSG As String
        Dim result As System.Windows.Forms.DialogResult
        Dim NewFileName As String = ""       'V4.9.0.0�A

        Try
            '-----------------------------------------------------------------------
            '   ��������
            '-----------------------------------------------------------------------
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(True)

            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            sPath = ""
            r = TrimStateOn(F_LOAD, APP_MODE_LOAD, "", "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            '-----------------------------------------------------------------------
            '   �Ǎ��t�@�C���p�����[�^�̊g���q�ݒ�
            '-----------------------------------------------------------------------
            '----- V1.14.0.0�E�� -----
            ' DOS�ł̊g���q�͕\�����Ȃ�
            If (gTkyKnd = KND_TKY) Then                                 ' TKY�̏ꍇ
                comDlgOpen.Filter = "*.tdt|*.tdt|*.WDT|*.WDT"
            ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIP�̏ꍇ
                'comDlgOpen.Filter = "*.tdc|*.tdc|*.WTC|*.WTC|*.WTC2|*.WTC2"            'V1.23.0.0�G
                comDlgOpen.Filter = "*.tdc|*.tdc|*.WTC|*.WTC|*.WTC2|*.WTC2|*.wdc|*.wdc" 'V1.23.0.0�G
            Else                                                        ' NET�̏ꍇ
                comDlgOpen.Filter = "*.tdn|*.tdn|*.WTN|*.WTN"
            End If
            'If (gTkyKnd = KND_TKY) Then                                 ' TKY�̏ꍇ
            '    comDlgOpen.Filter = "*.tdt|*.tdt|*.WDT|*.WDT|*.DAT|*.DAT"
            'ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIP�̏ꍇ
            '    comDlgOpen.Filter = "*.tdc|*.tdc|*.WTC|*.WTC|*.WTC2|*.WTC2|*.DC|*.DC"
            'Else                                                        ' NET�̏ꍇ
            '    comDlgOpen.Filter = "*.tdn|*.tdn|*.WTN|*.WTN|*.DAT|*.DAT"
            'End If
            '----- V1.14.0.0�E�� -----
            '----- V4.0.0.0�B�� -----
            If (giMachineKd = MACHINE_KD_RS) Then                           ' SL436S ? 
                ' SL436S�p�̊g���q��\������(SL43xR�̃f�[�^�����[�h��)
                If (gTkyKnd = KND_TKY) Then                                 ' TKY�̏ꍇ
                    comDlgOpen.Filter = "*.tdts|*.tdts|*.tdt|*.tdt"
                ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIP�̏ꍇ
                    comDlgOpen.Filter = "*.tdcs|*.tdcs|*.tdc|*.tdc"
                Else                                                        ' NET�̏ꍇ
                    comDlgOpen.Filter = "*.tdns|*.tdns|*.tdn|*.tdn"
                End If
            End If
            '----- V4.0.0.0�B�� -----
            '#If cSOFTKYBOARDcUSE = 1 Then
            '-----------------------------------------------------------------------
            ' �\�t�g�E�F�A�L�[�{�[�h���N������
            '-----------------------------------------------------------------------
            Dim procHandle As Process
            procHandle = New Process
            Call StartSoftwareKeyBoard(procHandle)
            '#End If

            '-----------------------------------------------------------------------
            '   �y̧�ق��J���z�޲�۸ނ�\������
            '-----------------------------------------------------------------------
            comDlgOpen.ShowReadOnly = False
            comDlgOpen.CheckFileExists = True
            comDlgOpen.CheckPathExists = True
            comDlgOpen.InitialDirectory = gSysPrm.stDIR.gsTrimFilePath  ' �g�����f�[�^�t�@�C���i�[�ʒu
            'V6.1.4.14�A��' �m�d�s�̎��̃g�����f�[�^�t�@�C���i�[�ʒu
            If gTkyKnd = KND_NET Then
                comDlgOpen.InitialDirectory = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA_NET\")
            End If
            'V6.1.4.14�A��

            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)    'V5.0.0.6�M
            ' �y̧�ق��J���z�޲�۸ނ�\������
            result = comDlgOpen.ShowDialog()

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' �\�t�g�E�F�A�L�[�{�[�h���I������
            Call EndSoftwareKeyBoard(procHandle)
            '#End If

            ' OK�ȊO�̏ꍇ
            If (result <> Windows.Forms.DialogResult.OK) Then
                GoTo STP_END                                            ' Cansel�w��Ȃ�I��
            End If

            '-----------------------------------------------------------------------
            '   �g���~���O�f�[�^�t�@�C����Ǎ���
            '-----------------------------------------------------------------------
            ' �t�@�C���o�[�W�������擾
            Call SetMousePointer(Me, True)                              ' �����v�\��(ϳ��߲��)
            sPath = comDlgOpen.FileName                                 ' �g���~���O�f�[�^�t�@�C����
            r = Sub_FileLoad(sPath)                                     ' �g���~���O�f�[�^�t�@�C����Ǎ��� 
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' Cancel �Ȃ�Return V4.0.0.0-34
            '----- V1.18.0.0�A�� -----
            ' �f�[�^���蓮�Ń��[�h������QR��������������(���[���a����)
            If (r = cFRS_NORMAL) Then
                Call QR_Info_Disp(0)                                    ' QR��������������(���[���a����)
                Call BC_Info_Disp(0)                                    ' �o�[�R�[�h��������������(���z�Гa����) V1.23.0.0�@
                '----- V4.0.0.0�S�� -----
                '�ڕW�l�A��������̕\��
                SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)
                '----- V4.0.0.0�S�� -----
            End If
            '----- V1.18.0.0�A�� -----

            ' 'V4.9.0.0�A��
            If giChangePoint = 1 Then
                NewFileName = MakeutPointFileData(gStrTrimFileName)
                If System.IO.File.Exists(NewFileName) Then
                Else
                    ' �t�@�C�����Ȃ������ꍇ�ɂ́A�f�t�H���g�̃t�@�C�����R�s�[����
                    My.Computer.FileSystem.CopyFile(DEFAULT_CUTFILE, NewFileName)
                End If
            End If
            ' 'V4.9.0.0�A��

            '-----------------------------------------------------------------------
            '   �I������
            '-----------------------------------------------------------------------
STP_END:
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)
            Call basTrimming.ChangeByTrimmingData()                     ' V5.0.0.6�A�@�g���~���O�f�[�^�X�V���̏���
            Call Form1Button(2)                                         ' �R�}���h�{�^����L���ɂ���
            gManualThetaCorrection = True                               ' �V�[�^�␳���s�t���O = True(�V�[�^�␳�����s����)
            Call TrimStateOff()                                         ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��

            '----- V6.0.3.0�H�� -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TrimData.SetLotChange()                                 ' ���b�g���N���A
            End If
            '----- V6.0.3.0�H�� -----

            ' �K�x�[�W�R���N�V�����ɋ����I�Ƀ��������J��������
            '   �f�o�b�O���̂ݎg�p�B���t���̂��ߖ����I�ɂ͎��s���Ȃ��B
#If DEBUG Then
            GC.Collect()
#End If
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLoad_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try
    End Sub
#End Region

#Region "�t�@�C�����[�h����"
    '''=========================================================================
    '''<summary>�t�@�C�����[�h����</summary>
    '''<param name="sPath">(INP)���[�h����t�@�C����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Sub_FileLoad(ByVal sPath As String) As Integer

        Dim r As Integer
        'Dim dblVer As Double                                           ' V4.0.0.0-28 _readFileVer, Field�ϐ��ɕύX
        Dim strXmlFName As String
        Dim strLDR As String
        Dim strMSG As String = ""
        Dim strPath As String = ""                                      ' V4.0.0.0-34
        Dim wreg As Short                                               ' V1.18.0.0�F
        Dim GType As Integer                                            ' V1.18.0.0�F
        Dim doSave As Boolean                                           ' V4.0.0.0-28

        Try
            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                Me.LblcLOTNUMBER.Text = ""                              ' ���Y�Ǘ����(�`�[�m���D)
                Me.LblcRESVALUE.Text = ""                               ' ���Y�Ǘ����(��R�l) 
            End If
            '----- V6.1.4.0_22�� -----
            '----- V4.0.0.0-34�� -----
            '-----------------------------------------------------------------------
            '   SL436S�Ŋg���q��".tdc"�Ȃ�".tdcs"�ɕϊ���̃t�@�C�������݂��Ȃ������m�F����
            '-----------------------------------------------------------------------
            Call GetSaveFileName(sPath, strPath)                        ' ".tdc �� .tdsd" 

            ' SL436S�Ŋg���q��".tdc"�Ȃ�".tdcs"�Ńt�@�C�����Z�[�u����
            strMSG = GetFileNameExtension(sPath)                        ' �t�@�C��������g���q�����o�� 
            If ((giMachineKd = MACHINE_KD_RS) And (strMSG.Length = 4)) Then
                ' �f�[�^�t�@�C���̑��݃`�F�b�N
                If (System.IO.File.Exists(strPath) = True) Then
                    strMSG = strPath + "" + MSG_159
                    ' "�t�@�C�������݂��܂��B�㏑�����Ă��X�����ł����H"
                    r = MsgBox(strMSG, MsgBoxStyle.OkCancel, "")
                    If (r <> MsgBoxResult.Ok) Then
                        Return (cFRS_ERR_RST)
                    End If
                End If
                doSave = True                                           ' �Z�[�u��FL�ւ̓]����ɂ����Ȃ� V4.0.0.0-28
            End If
            '----- V4.0.0.0-34��    -----

            '-----------------------------------------------------------------------
            '   �g���~���O�f�[�^�t�@�C����Ǎ���
            '-----------------------------------------------------------------------
            ' ���O�o�͗p
            If (gbFgAutoOperation = True) Then                          ' �����^�]�t���O(True:�����^�]��, False:�����^�]���łȂ�) 
                strLDR = "' AUTO"
            Else
                strLDR = "' MANUAL"
            End If

            ' �t�@�C���o�[�W�������擾
            r = File_Read_Ver(sPath, _readFileVer)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            ' �g���~���O�f�[�^�t�@�C����Ǎ���
            If (_readFileVer >= FileIO.FILE_VER_10) Then
                ' �V�o�[�W�����ȍ~�̏ꍇ 
                r = File_Read(sPath)
            Else
                ' ���o�[�W�����̏ꍇ(TKY��)
                If (gTkyKnd = KND_TKY) Then
                    'V5.0.0.8�@                    r = DatConv_TKY(sPath)                              ' DAT�t�@�C���ϊ�
                    'V5.0.0.8�@                    If (r <> 0) Then GoTo STP_ERR
                    r = File_Read_Tky(sPath)                            ' �g�����f�[�^�Ǎ���(TKY)
                End If
                ' ���o�[�W�����̏ꍇ(CHIP/NET��)
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then
                    'V5.0.0.8�@                    r = DatConv_CHIPNET(sPath)                          ' DAT�t�@�C���ϊ�
                    'V5.0.0.8�@                    If (r <> 0) Then GoTo STP_ERR
                    r = FileLoadExe(sPath)                              ' �g�����f�[�^�Ǎ���(CHIP/NET)
                End If
                typPlateInfo.strDataName = sPath                        ' �g���~���O�f�[�^���ݒ� 
            End If

            ' �f�[�^�t�@�C���̓Ǎ��ݎ��s��
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            '----- V4.0.0.0-34�� -----
            'typPlateInfo.strDataName = sPath                           ' �g���~���O�f�[�^���ݒ� V1.16.0.2�B
            typPlateInfo.strDataName = strPath                           ' �g���~���O�f�[�^���ݒ�
            sPath = typPlateInfo.strDataName
            '----- V4.0.0.0-34��    -----

            ' GPIB�R�}���h��ݒ肷�� ###002
            'V5.0.0.8�@            Call SetGpibCommand(typPlateInfo)
            DataManager.SetGpibCommand()                                'V5.0.0.8�@

            ' �ǂݍ��񂾃f�[�^�ɓ���FL���H�����ԍ��ňقȂ�ڕW�p���[�E���e�͈͂��ۑ�����Ă���ꍇ�����邽��
            ' ������H�����ԍ��̏ꍇ�͎����p���[�����Ŏg�p�����l�ɕύX����
            DataManager.SetNormalizedArrCutFLData()                     '#5.0.0.8�@

            '-----------------------------------------------------------------------
            '   ���샍�O�����o�͂���
            '-----------------------------------------------------------------------
            ' �f�[�^�t�@�C���̓Ǎ��ݐ�����
            If (gTkyKnd = KND_CHIP) Then
                Call SetEsType(sPath)                                   ' ES�̃^�C�v��ݒ�
                Call ChkCutTypeEs()                                     ' ES�̒u������
            End If

            '----- V6.1.4.0�F��(KOA EW�aSL432RD�Ή�) -----
            ' �t�@�C���p�X���̕\��
            'LblDataFileName.Text = Form1_006 & sPath
            'gStrTrimFileName = sPath                                    ' ���ݸ��ް�̧�ٖ����۰��ٕϐ��ɐݒ肷��
            'LblDataFileName.Text = sPath
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = sPath                             ' �g���~���O�f�[�^���Ƒ�P��R�f�[�^�\�����ݒ肷�� 
            Else
                LblDataFileName.Text = sPath                            ' �g���~���O�f�[�^���\��
            End If
            gStrTrimFileName = sPath                                    ' ���ݸ��ް�̧�ٖ����۰��ٕϐ��ɐݒ肷��
            '----- V6.1.4.0�F�� -----
            gLoadDTFlag = True                                          ' �ް�۰�ލ��׸�(False:�ް���۰��, True:�ް�۰�ލ�)
            commandtutorial.SetDataLoad()                               'V2.0.0.0�I �f�[�^���[�h��Ԑݒ�

            '----- �I�[�g�v���[�u�p�I�t�Z�b�g������ -----
            gfStgOfsX = 0.0                                             ' XY�e�[�u���I�t�Z�b�gX(mm) ���I�[�g�v���[�u���s�R�}���h(FrmMatrix())�Őݒ�
            gfStgOfsY = 0.0                                             ' XY�e�[�u���I�t�Z�b�gY(mm)
            '----- V1.13.0.0�B�� -----

            ' �V�X�e���ϐ��ݒ�(�v���[�uON/OFF�ʒu��) 
            Call PROP_SET(typPlateInfo.dblZOffSet, typPlateInfo.dblZWaitOffset, gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, gSysPrm.stDEV.gfSmaxX, gSysPrm.stDEV.gfSmaxY)
            '----- V1.15.0.0�B�� -----
            ' INtime����ZOFF�ʒu��ݒ肷��
            'r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)                 ' INtime����ZOFF�ʒu��zWaitPos�Ƃ��� 'V1.15.0.0�C
            r = SETZOFFPOS(typPlateInfo.dblZWaitOffset)                  ' INtime����ZOFF�ʒu��zWaitPos�Ƃ��� 'V1.15.0.0�C
            r = System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                Return (r)
            End If
            '----- V1.15.0.0�B�� -----

            '----- V4.0.0.0-28 �� -----  FL���ւ̉��H�����]����ɂ����Ȃ�
            ''----- V2.0.0.0_26�� -----
            ''-----------------------------------------------------------------------
            ''   ���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��
            ''   (�V���v���g���}�p(FL���ŉ��H�����t�@�C��������ꍇ))
            ''-----------------------------------------------------------------------
            'r = SetCutDataCndInfFromCndNum(dblVer)
            ''----- V2.0.0.0_26�� -----
            '----- V4.0.0.0-28 �� -----

            ' ���O�o��("�f�[�^���[�h")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC01, "File='" & sPath & strLDR)
            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
            'Call Z_CLS()                                                ' �f�[�^���[�h�Ń��O��ʃN���A
            If (giQrCodeType = QrCodeType.KoaEw) Then
                If (QR_Read_Flg = 0) Then                               ' �p�q�R�[�h�\�����̓N���A���Ȃ�
                    Call Z_CLS()                                        ' �f�[�^���[�h�Ń��O��ʃN���A
                End If
                ' ���Y�Ǘ����̓`�[�m��.�ƒ�R�l��\������
                If (typQRDATAInfo.bStatus = True) Then
                    ObjQRCodeReader.LotNumberDisp()
                End If
            Else
                Call Z_CLS()                                            ' �f�[�^���[�h�Ń��O��ʃN���A(�W����)
            End If
            '----- V6.1.4.0_22�� -----
            gDspCounter = 0                                             ' ���O��ʕ\��������J�E���^�N���A ###013
            ' "�f�[�^�����[�h���܂���"
            strMSG = MSG_143 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                        ' ү���ޕ\��(���O���)
            'V4.0.0.0-65                ��������
            'gLoggingHeader = True                                       ' ̧��۸�ͯ�ް�o���׸�(TRUE:�o��)
            ' ۸ޕ\���J�n�׸ނ���������B
            If (1 = gSysPrm.stLOG.giLoggingMode) Then
                basTrimming.TrimLogging_CreateOrAppend("Main - Load", typPlateInfo.strDataName)
            End If
            'V4.0.0.0-65                ��������
            stPRT_ROHM.Lot_No = " "                                     ' QR�f�[�^���b�g�ԍ��N���A(���[���a�p)

            'V4.0.0.0-61
            If gKeiTyp = KEY_TYPE_RS Then
                TrimData.SetTrimmingData(sPath)
            End If
            'V4.0.0.0-61

            '----- V1.22.0.0�C�� -----
            ' �T�}�����O�o��(�V�i�W�[)�Ȃ�f�[�^���[�h�ɐ��Y�Ǘ��f�[�^���N���A����(TKY)
            If (giSummary_Log = SUMMARY_OUT) Then
                Call ClearCounter(1)                                    ' ���Y�Ǘ��f�[�^�N���A
            End If
            '----- V1.22.0.0�C�� -----

            ' TKY_CHIP ?
            If (gTkyKnd = KND_CHIP) Then
                '----- V1.18.0.0�B�� -----
                'If gSysPrm.stDEV.rPrnOut_flg = True Then               ' �uPrint ON/OFF�v�{�^�����uPrint ON�v ?
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ���[���a���� ? 
                    '----- V1.18.0.1�C�� -----
                    ' �f�[�^���[�h���̓N���A���Ȃ�
                    'If (giAppMode <> APP_MODE_LOAD) Then                ' ���[�h�R�}���h(�蓮)�̏ꍇ�� �N���A���Ȃ�
                    '    Call ClearCounter(1)                            ' ���Y�Ǘ��f�[�^�N���A
                    '    Call ClrTrimPrnData()                           ' ���ݸތ��ʈ�����ڂ��ް���ر����(���[���a����) 
                    'End If
                    '----- V1.18.0.1�C�� -----
                End If
                '----- V1.18.0.0�B�� -----

                ' ���ʑΉ�(����߼޼�݁A�O����׵̾�āA��]���S�Đݒ�)
                Call SetMechanicalParam()
            End If

            '-----------------------------------------------------------------------
            '   FL���։��H�����𑗐M����(FL���ŉ��H�����t�@�C��������ꍇ)
            '-----------------------------------------------------------------------
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                '----- V4.0.0.0-58�� -----
                ' SL436S���͉��H�����ԍ���ݒ肵�Ȃ���
                Call SetCndNum()
                '----- V4.0.0.0-58�� -----
                ' ���H�����t�@�C�������݂��邩�`�F�b�N
                strXmlFName = ""
                r = GetFLCndFileName(sPath, strXmlFName, True)
                If (r = SerialErrorCode.rRS_OK) Then                    ' ���H�����t�@�C�������݂��� ?
                    ' �f�[�^���M���̃��b�Z�[�W�\��
                    strMSG = MSG_148
                    Call Z_PRINT(strMSG)                                ' ү���ޕ\��(���O���)

                    ' FL�p���H�����t�@�C�������[�h����FL���։��H�����𑗐M����
                    r = SendTrimCondInfToFL(stCND, sPath, strXmlFName)
                    If (r = SerialErrorCode.rRS_OK) Then

                        ' '#4.12.3.0�E ��
                        'FL�����ꌳ�Ǘ��̏ꍇ�ɂ́A���O��ʂ�FL�����t�@�C������\�����Ȃ� 
                        If (giFLPrm_Ass = 1) Then
                            ' "FL�։��H�����𑗐M���܂����B"
                            strMSG = MSG_147 & vbCrLf
                        Else
                            ' "FL�։��H�����𑗐M���܂����B"
                            strMSG = MSG_147 & vbCrLf & " (SendDdata File Name = " & strXmlFName & ")"
                        End If
                        Call Z_PRINT(strMSG)                            ' ү���ޕ\��(���O���)
                        ' '#4.12.3.0�E ��

                    Else
                        strMSG = MSG_132                                ' "���H�����̑��M�Ɏ��s���܂����B�ēx�f�[�^�����[�h���邩�A�ҏW��ʂ�����H�����̐ݒ���s���Ă��������B"
                        Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                        Call Z_PRINT(strMSG)                            ' ү���ޕ\��(���O���)
                        Return (cFRS_FIOERR_INP)                        ' Return�l = �t�@�C�����̓G���[
                    End If
                    '----- V1.14.0.0�A�� -----
                    ' FL�p�p���[�������t�@�C�������[�h����
                    Call GetFlAttInfoData(strMSG, stPWR_LSR, 0)         ' FL�p�p���[������񏉊���
                    'V6.0.1.021�@��
                    '                    If (giAutoPwr = 1) Then                             ' �I�[�g�p���[������(1=����/0=���Ȃ�) 
                    If (giAutoPwr = 1) Or (giFixAttOnly = 1) Then                             ' �I�[�g�p���[������(1=����/0=���Ȃ�) 
                        'V6.0.1.021�@��
                        r = GetFLAttFileName(sPath, strMSG, True)       ' FL�p�p���[�������t�@�C�������擾����
                        If (r = SerialErrorCode.rRS_OK) Then            ' FL�p�p���[�������t�@�C�������݂��� ?
                            Call GetFlAttInfoData(strMSG, stPWR_LSR, 1) ' FL�p�p���[������񃊁[�h
                        End If
                    End If

                    ' INtime���֌Œ�ATT���𑗐M����
                    r = SetFixAttInfo(stPWR_LSR.AttInfoAry(0))
                    r = Me.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                        Return (r)
                    End If
                    '----- V1.14.0.0�A�� -----

                End If

                '----- V4.0.0.0-28 �� -----  ���H�����t�@�C�����Ȃ��ꍇ��
                ' Form_Load() Initialize_TrimMachine()��FL�֓]�����ꂽFLParam.xml�̒l���ݒ肳���
                '-----------------------------------------------------------------------
                '   ���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��
                '-----------------------------------------------------------------------
                r = SetCutDataCndInfFromCndNum(_readFileVer)
                '----- V4.0.0.0-28 �� -----
            End If

            '----- V4.0.0.0-28 �� -----
            If (True = doSave) Then
                ' �t�@�C�����Z�[�u����
                r = File_Save(sPath)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR2
            End If
            '----- V4.0.0.0-28 �� -----

            '----- V1.18.0.0�F�� -----
            ' �ėpGP-IB����̗L�����擾����
            Call Gpib2FlgCheck(bGpib2Flg, wreg, GType)
            ' �ėpGPIB�f�[�^��INtime���ɑ��M����
            If (GType = 1) Then                                         ' �ėpGPIB ?
                r = SendTrimDataPlate2(gTkyKnd, wreg, GType)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If
            End If
            '----- V1.18.0.0�F�� -----

            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                If (gbQRCodeReaderUse OrElse gbQRCodeReaderUseTKYNET) And (typQRDATAInfo.bStatus) Then ' �p�q�f�[�^�L�� ? 'V6.1.4.14�AgbQRCodeReaderUseTKYNET���ǉ�
                    ' �p�q�f�[�^�̌�����(%)���烍�[�^���[�A�b�e�l�[�^��ݒ肷��
                    'r = ObjQRCodeReader.SetAttenuater(typQRDATAInfo.dAttenuaterValue)      'V6.1.4.0_22
                    r = ObjQRCodeReader.SetAttenuater(CDbl(typQRDATAInfo.dAttenuaterValue)) 'V6.1.4.0_22
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                        ' "�A�b�e�l�[�^������=[x.xx(%)]�ݒ�ُ�I�����܂����B=[xxx]"
                        'strMSG = MSG_167 + "=[" & typQRDATAInfo.dAttenuaterValue.ToString("0.00") & "(%)]" + MSG_168 + "=[" & r.ToString & "]"         'V6.1.4.0_22
                        strMSG = MSG_167 + "=[" & CDbl(typQRDATAInfo.dAttenuaterValue).ToString("0.00") & "(%)]" + MSG_168 + "=[" & r.ToString & "]"    'V6.1.4.0_22
                        Call Me.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    End If
                End If
            End If
            '----- V6.1.4.0_22�� -----

            '-----------------------------------------------------------------------
            '   �I������
            '-----------------------------------------------------------------------
            gCmpTrimDataFlg = 0                                         ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�)    'V4.0.0.0-32
            Return (cFRS_NORMAL)                                        ' Return�l = ����

STP_ERR:
            ' �f�[�^�t�@�C���̓Ǎ��ݎ��s��("�f�[�^���[�hNG File=")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC01, "NG File='" & sPath & strLDR)
            ' "�f�[�^���[�h�m�f"
            strMSG = MSG_144 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                        ' ү���ޕ\��(���O���)
            Return (cFRS_FIOERR_INP)                                    ' Return�l = �t�@�C�����̓G���[

            '----- V4.0.0.0�H�� -----
STP_ERR2:
            ' �f�[�^�t�@�C���̏����ݎ��s��
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC01, "NG File='" & sPath & strLDR)
            ' "�f�[�^�Z�[�u�m�f"
            strMSG = MSG_146 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                        ' ү���ޕ\��(���O���)
            Return (cFRS_FIOERR_OUT)                                    ' Return�l = �t�@�C���o�̓G���[
            '----- V4.0.0.0�H��    -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Sub_FileLoad() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[

            'V4.12.2.0�@                 ��
        Finally
            ' �f�[�^�t�@�C���ǂݍ��ݐ���=True
            Dim doEnabled As Boolean = (cFRS_NORMAL = Sub_FileLoad)
            MapInitialize(doEnabled)                                    ' �}�b�v�ĕ`��
        End Try
        'V4.12.2.0�@                     ��
    End Function
#End Region

#Region "ES��Ă̓ǂݍ��ݔ��ʐݒ菈��"
    '''=========================================================================
    '''<summary>ES��Ă̓ǂݍ��ݔ��ʐݒ菈��</summary>
    '''<param name="sPath">(INP) ̧���߽��</param>
    '''=========================================================================
    Private Sub SetEsType(ByRef sPath As String)
        On Error GoTo ERR_PROC

        Dim iPos As Short
        Dim sExt As String

        ' �g���q�܂ł̕��������擾
        iPos = InStrRev(sPath, ".")

        ' �g���q���擾
        sExt = Mid(sPath, iPos + 1)

        Select Case UCase(sExt)

            Case "WTC"
                ' ES�͒ʏ��ES
                gEsCutFileType = 0
            Case "WTC2"
                ' ES��ES2�œǂݍ���
                gEsCutFileType = 1
            Case Else
                ' ES�͒ʏ��ES
                gEsCutFileType = 0
        End Select

        Exit Sub
ERR_PROC:
        System.Diagnostics.Debug.Assert((0), "")
        Err.Clear()
    End Sub
#End Region

#Region "ES��Ă̒u��������"
    '''=========================================================================
    '''<summary>ES��Ă̒u��������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub ChkCutTypeEs()
        On Error GoTo ERR_PROC

        Dim iTotalChipNum As Short
        Dim iTotalCutNum As Short
        Dim iRegCnt As Short
        Dim iRegNo As Short
        Dim iCutCnt As Short

        ' �J�����ɂ�ES�݂̂������������AES ES2���J�b�g�^�C�v�݂���׏��������{���Ȃ�    'V1.14.0.0�@
        Exit Sub
        ' �ް��擾
        'Call GetChipNum(iTotalChipNum)                      ' ��R���擾
        iTotalChipNum = typPlateInfo.intResistCntInBlock    ' �u���b�N����R��(�}�[�L���O��R�܂�)
        For iRegCnt = 1 To iTotalChipNum
            ' �ް��擾
            iRegNo = typResistorInfoArray(iRegCnt).intResNo ' ��R�ԍ��擾
            Call GetRegCutNum(iRegNo, iTotalCutNum)         ' ��Đ��擾

            'With typResistorInfoArray(iRegNo)              ' ###012
            With typResistorInfoArray(iRegCnt)              ' ###012
                ' 1��R�̶�Đ���ٰ��
                For iCutCnt = 1 To iTotalCutNum
                    ' �������ES�̓ǂݍ��݂�"ES"�ōs���ꍇ
                    If gEsCutFileType = 0 Then                      ' �r�k�S�R�Q�g�v�̃f�[�^�@V1.14.0.0�@
                        ' ES2�̏�񂪂����
                        If .ArrCut(iCutCnt).strCutType = "S" Then
                            ' ES�ɕύX
                            .ArrCut(iCutCnt).strCutType = "K"
                        End If
                        ' �������ES�̓ǂݍ��݂�"ES2"�ōs���ꍇ
                    ElseIf gEsCutFileType = 1 Then                  ' �r�k�S�R�Q�g�v�̃f�[�^�@V1.14.0.0�@
                        ' ES�̏�񂪂����
                        If .ArrCut(iCutCnt).strCutType = "K" Then
                            'ES2�ɕύX
                            .ArrCut(iCutCnt).strCutType = "S"
                        End If
                    Else                                            ' �r�k�S�R�Q�q�̃f�[�^ �@V1.14.0.0�@
                    End If
                Next
            End With
        Next

        '' NG�}�[�L���O����̏ꍇ                           ' ###012
        'If typPlateInfo.intNGMark = 1 Then
        '    iRegNo = 1000
        '    Call GetRegCutNum(iRegNo, iTotalCutNum)         ' ��Đ��擾
        '    With typResistorInfoArray(iRegNo)
        '        ' 1��R�̶�Đ���ٰ��
        '        For iCutCnt = 1 To iTotalCutNum
        '            ' �������ES�̓ǂݍ��݂�"ES"�ōs���ꍇ
        '            If gEsCutFileType = 0 Then
        '                ' ES2�̏�񂪂����
        '                If .ArrCut(iCutCnt).strCutType = "S" Then
        '                    ' ES�ɕύX
        '                    .ArrCut(iCutCnt).strCutType = "K"
        '                End If
        '                ' �������ES�̓ǂݍ��݂�"ES2"�ōs���ꍇ
        '            Else
        '                ' ES�̏�񂪂����
        '                If .ArrCut(iCutCnt).strCutType = "K" Then
        '                    'ES2�ɕύX
        '                    .ArrCut(iCutCnt).strCutType = "S"
        '                End If
        '            End If
        '        Next
        '    End With
        'End If

        '        Exit Sub

ERR_PROC:
        System.Diagnostics.Debug.Assert((0), "")
        Err.Clear()
    End Sub
#End Region

#Region "̧�پ���(F2)���݉���������"
    '''=========================================================================
    '''<summary>̧�پ���(F2)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub CmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdSave.Click

        Dim r As Integer
        Dim sPath As String
        Dim strXmlFName As String
        Dim strMSG As String = ""
        Dim result As System.Windows.Forms.DialogResult
        Dim NowFileName As String = ""       'V4.9.0.0�A
        Dim NewFileName As String = ""       'V4.9.0.0�A

        Try
            '-----------------------------------------------------------------------
            '   ��������
            '-----------------------------------------------------------------------
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(True)

            If (giAppMode <> APP_MODE_EDIT) Then                ' �ް��ҏW���Ȃ牺�L��SKIP
                ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
                r = TrimStateOn(F_SAVE, APP_MODE_SAVE, "", "")
                If (r <> cFRS_NORMAL) Then GoTo STP_END
            End If

            '-----------------------------------------------------------------------
            '   �����݃t�@�C���̊g���q�ݒ�
            '-----------------------------------------------------------------------
            If (gTkyKnd = KND_TKY) Then                         ' TKY�̏ꍇ
                comDlgSave.Filter = "*.tdt|*.tdt"
            End If
            If (gTkyKnd = KND_CHIP) Then                        ' CHIP�̏ꍇ
                comDlgSave.Filter = "*.tdc|*.tdc"
            End If
            If (gTkyKnd = KND_NET) Then                         ' NET�̏ꍇ
                comDlgSave.Filter = "*.tdn|*.tdn"
            End If
            '----- V4.0.0.0�B�� -----
            If (giMachineKd = MACHINE_KD_RS) Then                           ' SL436S ? 
                ' SL436S�p�̊g���q��\������
                If (gTkyKnd = KND_TKY) Then                                 ' TKY�̏ꍇ
                    comDlgSave.Filter = "*.tdts|*.tdts"
                ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIP�̏ꍇ
                    comDlgSave.Filter = "*.tdcs|*.tdcs"
                Else                                                        ' NET�̏ꍇ
                    comDlgSave.Filter = "*.tdns|*.tdns"
                End If
            End If
            '----- V4.0.0.0�B�� -----
            '#If cSOFTKYBOARDcUSE = 1 Then
            '-----------------------------------------------------------------------
            ' �\�t�g�E�F�A�L�[�{�[�h���N������
            '-----------------------------------------------------------------------
            Dim procHandle As Process
            procHandle = New Process
            Call StartSoftwareKeyBoard(procHandle)
            '#End If

            '-----------------------------------------------------------------------
            '   �y���O��t���ĕۑ��z�޲�۸ނ�\������
            '-----------------------------------------------------------------------
            ' �g�����f�[�^�t�@�C���i�[�ʒu
            'V6.1.4.14�A��
            comDlgSave.InitialDirectory = System.IO.Path.GetDirectoryName(typPlateInfo.strDataName) 'V6.1.4.14�A
            If System.IO.Directory.Exists(comDlgSave.InitialDirectory) = False Then
                'V6.1.4.14�A��
                comDlgSave.InitialDirectory = gSysPrm.stDIR.gsTrimFilePath
                'V6.1.4.14�A��' �m�d�s�̎��̃g�����f�[�^�t�@�C���i�[�ʒu
                If gTkyKnd = KND_NET Then
                    comDlgSave.InitialDirectory = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA_NET\")
                End If
            End If
            'V6.1.4.14�A��
            '----- V1.14.0.0�E�� -----
            ' ��)���f�[�^(xxxx.WTC)�̏ꍇ�A�g���q��xxx.tdc�łȂ�.WTC�̂܂܃Z�[�u�����̂Ł@�g���q��tdc�ɕύX����
            'comDlgSave.FileName = typPlateInfo.strDataName
            Call GetSaveFileName(typPlateInfo.strDataName, strMSG)
            comDlgSave.FileName = strMSG
            '----- V1.14.0.0�E�� -----
            comDlgSave.OverwritePrompt = True                   ' ���݂���ꍇ�͊m�Fү���ނ�\��
            'V6.0.0.0�D            Call End_GazouProc(ObjGazou)    'V5.0.0.6�M
            '�y���O��t���ĕۑ��z�޲�۸ނ�\������
            ' ��̧�ٖ��w��Ȃ��ł͖߂��Ă��Ȃ��A�g���q�t�Ŗ߂��Ă���
            result = comDlgSave.ShowDialog()

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' �\�t�g�E�F�A�L�[�{�[�h���I������
            Call EndSoftwareKeyBoard(procHandle)
            '#End If

            ' OK�ȊO�Ȃ�I��
            If (result <> Windows.Forms.DialogResult.OK) Then
                GoTo STP_END                                    ' Cansel�w��Ȃ�I��
            End If

            'V4.0.0.0-65                ��������
            ' ۸ޕ\���J�n�׸ނ���������B
            If (1 = gSysPrm.stLOG.giLoggingMode) Then
                If (comDlgSave.FileName <> typPlateInfo.strDataName) Then
                    ' �ʖ��ۑ����ꂽ�ꍇ
                    basTrimming.TrimLogging_CreateOrAppend(
                        "Main - Save (" & comDlgSave.FileName & ")", procMsgOnly:=True)             ' ����̧�قɕۑ�

                    basTrimming.TrimLogging_CreateOrAppend("Main - Save", comDlgSave.FileName)      ' �V�Ķ�قɕۑ�
                Else
                    basTrimming.TrimLogging_CreateOrAppend("Main - Save", procMsgOnly:=True)
                End If
            End If
            'V4.0.0.0-65                ��������

            '-----------------------------------------------------------------------
            '   �g���~���O�f�[�^�t�@�C����������
            '-----------------------------------------------------------------------
            ' �g���~���O�f�[�^�t�@�C����������
            Call SetMousePointer(Me, True)                      ' �����v�\��(ϳ��߲��)
            sPath = comDlgSave.FileName                         ' �g���~���O�f�[�^�t�@�C����
            typPlateInfo.strDataName = sPath                    ' �v���[�g�f�[�^�̃g���~���O�f�[�^�t�@�C�����X�V
            r = File_Save(sPath)                                ' �g�����f�[�^������
            ' �f�[�^�t�@�C���̏����ݎ��s��
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            '-----------------------------------------------------------------------
            '   �t�@�C���p�X����\������
            '-----------------------------------------------------------------------
            '----- V6.1.4.0�F��(KOA EW�aSL432RD�Ή�) -----
            ''V4.0.0.0�N
            'LblDataFileName.Text = sPath
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = sPath                     ' �g���~���O�f�[�^���Ƒ�P��R�f�[�^�\�����ݒ肷�� 
            Else
                LblDataFileName.Text = sPath                    ' �g���~���O�f�[�^���\��
            End If
            '----- V6.1.4.0�F�� -----

            '-----------------------------------------------------------------------
            '   FL�����猻�݂̉��H��������M����FL�p���H�����t�@�C�������C�g����(FL��)
            '-----------------------------------------------------------------------
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then   ' FL(̧��ްڰ��) ? 
                strXmlFName = ""
                r = RcvTrimCondInfToFL(stCND, sPath, strXmlFName)
                If (r = SerialErrorCode.rRS_OK) Then
                    ' "���H�����t�@�C�����쐬���܂����B"
                    ' '#4.12.3.0�E ��
                    '                    strMSG = MSG_142 + vbCrLf + " (File Name = " + strXmlFName + ")"
                    'FL�����ꌳ�Ǘ��̏ꍇ�ɂ́A���O��ʂ�FL�����t�@�C������\�����Ȃ� 
                    If (giFLPrm_Ass = 1) Then
                        ' "FL�։��H�����𑗐M���܂����B"
                        strMSG = MSG_142 & vbCrLf
                    Else
                        ' "FL�։��H�����𑗐M���܂����B"
                        strMSG = MSG_142 & vbCrLf & " (SendDdata File Name = " & strXmlFName & ")"
                    End If
                    Call Z_PRINT(strMSG)                        ' ү���ޕ\��(���O���)
                    ' '#4.12.3.0�E ��
                End If
                '----- V1.14.0.0�A��-----
                ' FL�p�p���[�������t�@�C��(�f�[�^�t�@�C����+.att)�����C�g����
                'V6.0.1.021�@��
                '                    If (giAutoPwr = 1) Then                             ' �I�[�g�p���[������(1=����/0=���Ȃ�) 
                If (giAutoPwr = 1) Or (giFixAttOnly = 1) Then                             ' �I�[�g�p���[������(1=����/0=���Ȃ�) 
                    'V6.0.1.021�@��
                    r = GetFLAttFileName(sPath, strMSG, False)  ' �Œ�ATT���t�@�C�������擾����
                    Call PutFlAttInfoData(strMSG, stPWR_LSR)    ' FL�p�p���[�������t�@�C�������C�g����
                End If
                '----- V1.14.0.0�A��-----
            End If

            'V4.9.0.0�A��
            If giChangePoint = 1 Then
                If gKeiTyp = KEY_TYPE_RS Then
                    NowFileName = MakeutPointFileData(gStrTrimFileName)
                End If
            End If
            'V4.9.0.0�A��
            gStrTrimFileName = sPath                            ' ���ݸ��ް�̧�ٖ����۰��ٕϐ��ɐݒ肷��
            'V4.2.0.0�B
            If gKeiTyp = KEY_TYPE_RS Then
                TrimData.SetTrimmingData(sPath)
                'V4.9.0.0�A��
                If giChangePoint = 1 Then
                    If System.IO.File.Exists(NowFileName) Then
                        NewFileName = MakeutPointFileData(gStrTrimFileName)
                        If NowFileName <> NewFileName Then
                            My.Computer.FileSystem.CopyFile(NowFileName, NewFileName)
                        End If
                    Else
                        ' �t�@�C�����Ȃ������ꍇ�ɂ́A�f�t�H���g�̃t�@�C�����R�s�[����
                        My.Computer.FileSystem.CopyFile(DEFAULT_CUTFILE, NewFileName)
                    End If
                End If
                'V4.9.0.0�A��
            End If
            'V4.2.0.0�B
            gCmpTrimDataFlg = 0                                 ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�)

            '-----------------------------------------------------------------------
            '   ���샍�O���o�͂���
            '-----------------------------------------------------------------------
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC02, "File='" & sPath & "' MANUAL") 'OK
            ' "�f�[�^���Z�[�u���܂���"
            strMSG = MSG_145 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                ' ү���ޕ\��(���O���)

STP_END:
            '-----------------------------------------------------------------------
            '   �I������
            '-----------------------------------------------------------------------
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            If (giAppMode <> APP_MODE_EDIT) Then                ' �ް��ҏW���Ȃ牺�L��SKIP
                Call TrimStateOff()                             ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            End If
            Exit Sub

STP_ERR:
            ' �f�[�^�t�@�C���̏����ݎ��s��
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC02, "File='" & sPath & "' MANUAL LOADERR")
            ' "�f�[�^�Z�[�u�m�f"
            strMSG = MSG_146 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                ' ү���ޕ\��(���O���)
            GoTo STP_END

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdSave_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try
    End Sub
#End Region

#Region "�ް��ҏW(F3)���݉���������"
    '''=========================================================================
    '''<summary>�ް��ҏW(F3)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdEdit.Click

        Dim piPP31 As Short
        Dim r As Integer
        Dim strMSG As String
        Dim shiftKeyDown As Boolean = (_editNewData AndAlso (Control.ModifierKeys = Keys.Shift))    'V4.12.4.0�A

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' ���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(True)                'V4.7.3.5�@ �p�X���[�h��ʕ\���O�iTrimStateOn�̒��j�ɓ��v��ʂ���Ȃ��ƃp�X���[�h��ʂ��B��Ă��܂��̂�TrimStateOn�̑O�Ɉړ�
            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            '#4.12.2.0�D            r = TrimStateOn(F_EDIT, APP_MODE_EDIT, MSG_OPLOG_FUNC03, "")
            r = TrimStateOn(F_EDIT, APP_MODE_EDIT, MSG_OPLOG_FUNC03, "", shiftKeyDown)      '#4.12.2.0�D
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' ���v�\�������̏�ԕύX
            'V4.7.3.5�@            Call RedrawDisplayDistribution(True)

            ' �R���\�[���{�^���̃����v��Ԃ�ݒ肷��
            Call LAMP_CTRL(LAMP_START, False)                       ' START���ߏ��� 
            Call LAMP_CTRL(LAMP_RESET, False)                       ' RESET���ߏ��� 

            piPP31 = typPlateInfo.intManualReviseType               ' �␳���@(0:�␳�Ȃ�, 1:1��̂�, 2:����)��ۑ����Ă���
            Call Form1Button(0)                                     ' �R�}���h�{�^���𖳌��ɂ���
            Call SetVisiblePropForVideo(False)

            '----- V6.0.3.0�B�� -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TimerQR.Enabled = False                             ' �Ď��^�C�}�[��~
                Call QR_Rs232c_Close()                              ' �|�[�g�N���[�Y(QR�R�[�h��M�p ���[���a����)
            End If
            '----- V6.0.3.0�B�� -----

            '-------------------------------------------------------------------
            '   �f�[�^�ҏW�v���O�����N��(�ʏ탂�[�h)���� ###063
            '-------------------------------------------------------------------
            '#4.12.2.0�D            r = ExecEditProgram(0)                                  ' �f�[�^�ҏW�v���O�����N��
            r = ExecEditProgram(0, shiftKeyDown)                    ' �f�[�^�ҏW�v���O�����N��  '#4.12.2.0�D
            If (r <> cFRS_NORMAL) And (r <> cFRS_FIOERR_INP) And (r <> cFRS_FIOERR_OUT) Then
                If (r <= cFRS_ERR_EMG) Then                         ' ����~�����o ?
                    Call Me.AppEndDataSave()
                    Call Me.AplicationForcedEnding()                ' �����I��
                End If
                GoTo STP_END
            End If

            '----- V4.0.0.0�S�� -----
            '�ڕW�l�A��������̕\��
            SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)
            '----- V4.0.0.0�S�� -----

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
STP_END:
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            '----- V6.0.3.0�B�� -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                Call QR_Rs232c_Open()                               ' �|�[�g�I�[�v��(QR�R�[�h��M�p ���[���a����)
                TimerQR.Enabled = True                              ' �Ď��^�C�}�[�J�n
            End If
            '----- V6.0.3.0�B�� -----

            'V5.0.0.6�R ADD START��
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                Dim swStatus As Integer
                Dim interlockStatus As Integer
                r = INTERLOCK_CHECK(interlockStatus, swStatus)
                If r = cFRS_NORMAL And interlockStatus = INTERLOCK_STS_DISABLE_NO Then
                    ' �X���C�h�J�o�[�̏�Ԏ擾�iINTRIM�ł�IO�擾�ׁ݂̂̈A�G���[���Ԃ鎖�͂Ȃ��j
                    r = SLIDECOVER_GETSTS(swStatus)
                    If r = cFRS_NORMAL And swStatus = SLIDECOVER_CLOSE Then
                        ' �X���C�h�J�o�[��
                        r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH62, &HFF, &HFF0000)
                    End If
                End If
            End If
            'V5.0.0.6�R ADD END��

            Call basTrimming.ChangeByTrimmingData()                     ' V5.0.0.6�A�@�g���~���O�f�[�^�X�V���̏���

            ' Form�����ɖ߂�
            'Me.WindowState = FormWindowState.Normal
            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��
            Call TrimStateOff()
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdEdit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try
    End Sub
#End Region

#Region "�V�K���[�h�p�̃f�[�^����ݒ肷��"
    '''=========================================================================
    '''<summary>�V�K���[�h�p�̃f�[�^����ݒ肷��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Replace_Data_Extension(ByRef FName As String)

        Dim strDAT As String
        Dim strMSG As String

        Try
            ' �V�K���[�h�̏ꍇ�́A�V�K���[�h�p�̃f�[�^����ݒ肷��
            strDAT = DateTime.Today.Year.ToString("0000") + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
            If (gTkyKnd = KND_TKY) Then                         ' TKY�̏ꍇ
                strDAT = "TkyData" + strDAT + ".tdt"            ' "TkyDatayyyymmddhhmmss.tdt"
            ElseIf (gTkyKnd = KND_CHIP) Then                    ' CHIP�̏ꍇ
                If (gKeiTyp = MACHINE_KD_RS) Then   'V4.0.0.0-38
                    strDAT = "ChipData" + strDAT + ".tdcs"      ' "ChipDatayyyymmddhhmmss.tdcs"
                Else
                    strDAT = "ChipData" + strDAT + ".tdc"       ' "ChipDatayyyymmddhhmmss.tdc"
                End If
            Else
                strDAT = "NetData" + strDAT + ".tdn"            ' "NetDatayyyymmddhhmmss.tdn"
            End If
            FName = strDAT

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Replace_Data_Extension() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "Laser(F5)���݉���������"
    '''=========================================================================
    '''<summary>Laser(F5)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdLaser_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLaser.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_LASER, APP_MODE_LASER, MSG_OPLOG_FUNC05, "")

            '----- V4.0.0.0-28 �� -----
            '-----------------------------------------------------------------------
            '   ���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��
            '   (�V���v���g���}�p(FL���ŉ��H�����t�@�C��������ꍇ))
            '-----------------------------------------------------------------------
            r = SetCutDataCndInfFromCndNum(_readFileVer)
            '----- V4.0.0.0-28 �� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLaser_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "Laser��������"
    '''=========================================================================
    '''<summary>Laser��������</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function LaserProc(ByRef pltInfo As PlateInfo) As Short

        Dim fQrate As Double                                            ' Q���[�g[KHz] 
        Dim SPower As Double                                            ' �ݒ�p���[[W](��߼��)
        Dim r As Short
        Dim strMSG As String = ""
        Dim stPWR As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach.POWER_ADJUST_INFO                      ' FL�p�p���[�������p�����[�^ ��Laser.OCX�Œ�`'V1.14.0.0�A
        ReDim stPWR.CndNumAry(MAX_BANK_NUM - 1)                         ' �\���̂̏����� 
        ReDim stPWR.AdjustTargetAry(MAX_BANK_NUM - 1)
        ReDim stPWR.AdjustLevelAry(MAX_BANK_NUM - 1)
        ReDim stPWR.AttInfoAry(MAX_BANK_NUM - 1)
        Dim parModules As MainModules
        parModules = New MainModules
        Dim StgX As Double = 0.0 ' V4.0.0.0-40
        Dim StgY As Double = 0.0 ' V4.0.0.0-40

        Try
            With pltInfo
                '-------------------------------------------------------------------
                '   ���[�U�[�����O����
                '-------------------------------------------------------------------
                LaserProc = cFRS_NORMAL                                 ' Return�l = ����

                '----- V3.0.0.2�@�� -----
                ' LASER�R�}���h���́AZ�̈ʒu��0�Ƃ���
                r = EX_ZMOVE(0.0, MOVE_ABSOLUTE)                        ' �G���[�Ȃ烁�b�Z�[�W�\���ς�
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If
                '----- V3.0.0.2�@�� -----

                If (gSysPrm.stIOC.giPM_Tp = 1) Then                     ' �p���[���[�^���u�X�e�[�W�ݒu�v�^�C�v ? ###132
                    'If (gSysPrm.stRMC.giRmCtrl2 >= 2) And (gSysPrm.stIOC.giPM_Tp = 1) Then
                    '---------------------------------------------------------------
                    '   XY�e�[�u�����V�X�p������ܰ�����ʒu�ֈړ�����
                    '   �p���[����l�\���Ȃ���ܰ�����ʒu�ֈړ����� (��ܰҰ��t���ڕ���̏ꍇ)
                    '---------------------------------------------------------------
                    ' �ݒ�p���[/Q���[�g�ݒ�
                    fQrate = .dblPowerAdjustQRate                       ' Q���[�g[KHz]
                    SPower = .dblPowerAdjustQRate                       ' �ݒ�p���[[W]

                    ' XY�e�[�u������ܰ�����ʒu�ֈړ�����
                    r = System1.EX_SMOVE2(gSysPrm, gSysPrm.stRA2.gfATTTableOffsetX, gSysPrm.stRA2.gfATTTableOffsetY)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                        LaserProc = r                                   ' (��)���Я�/��ѱ�Ĵװү���ނ͕\���ς�
                        Exit Function
                    End If
                    ' �u���b�N�T�C�Y�ݒ肵f�ƃZ���^��
                    'r = System1.EX_BSIZE(gSysPrm, 0, 0)                ' ��ۯ�����/BP�̾��(��ܰ�����ʒu)�ݒ�
                    '----- V1.24.0.0�@�� -----
                    If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                        r = System1.EX_BSIZE(gSysPrm, 60.0, 20.0)
                        'V6.0.4.0�@��
                    ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                        r = System1.EX_BSIZE(gSysPrm, 60.0, 30.0)
                        'V6.0.4.0�@��
                    ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then        'V4.4.0.0�@
                        r = System1.EX_BSIZE(gSysPrm, 90.0, 90.0)
                    Else
                        r = System1.EX_BSIZE(gSysPrm, 80.0, 80.0)
                    End If
                    'r = System1.EX_BSIZE(gSysPrm, 80.0, 80.0)           ' ��ۯ�����/BP�̾��(��ܰ�����ʒu)�ݒ� ###132
                    '----- V1.24.0.0�@�� -----
                    If (r <> cFRS_NORMAL) Then                          ' �װ ?
                        LaserProc = r                                   ' (��)���Я�/��ѱ�Ĵװү���ނ͕\���ς�
                        Exit Function
                    End If

                    ' BP�ړ�(��ܰ����BP�ʒu) ###132
                    'r = System1.EX_BPOFF(gSysPrm, gSysPrm.stRA2.gfATTBpOffsetX, gSysPrm.stRA2.gfATTBpOffsetY)
                    'If (r <> cFRS_NORMAL) Then                          ' �װ ?
                    '    LaserProc = r                                   ' (��)���Я�/��ѱ�Ĵװү���ނ͕\���ς�
                    '    Exit Function
                    'End If
                    r = System1.EX_MOVE(gSysPrm, gSysPrm.stRA2.gfATTBpOffsetX, gSysPrm.stRA2.gfATTBpOffsetY, 1)
                    If (r <> cFRS_NORMAL) Then                          ' �װ ?
                        LaserProc = r                                   ' (��)���Я�/��ѱ�Ĵװү���ނ͕\���ς�
                        Exit Function
                    End If

                Else
                    '--------------------------------------------------------------------------
                    '   �ƕ␳(��߼��) & XY�e�[�u�����g�����ʒu�ֈړ�����
                    '--------------------------------------------------------------------------
                    ' Q���[�g�����l�ݒ�
                    fQrate = gSysPrm.stDEV.gfLaserQrate                 ' Q���[�g[KHz]
                    SPower = 0.0                                        ' �ݒ�p���[[W](��߼��)

                    ' �u���b�N�T�C�Y(0,0)�֐ݒ肵f�ƃZ���^��
                    r = System1.EX_BSIZE(gSysPrm, 0, 0)                 ' ��ۯ�����/BP�̾��(��ܰ�����ʒu)�ݒ�
                    If (r <> cFRS_NORMAL) Then                          ' �װ ?
                        LaserProc = r                                   ' (��)���Я�/��ѱ�Ĵװү���ނ͕\���ς�
                        Exit Function
                    End If

                    ' XY�e�[�u�����g�����ʒu�ֈړ�(�g�����ʒu�{�I�t�Z�b�g�{�␳�l)
                    '----- V1.13.0.0�B�� -----
                    'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX, .dblTableOffsetYDir + gfCorrectPosY, 0)

                    '----- V2.0.0.0�H�� -----
                    If (giMachineKd = MACHINE_KD_RS) Then
                        '----- V4.0.0.0-40�� -----
                        ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
                        If (giStageYOrg = STGY_ORG_UP) Then
                            StgX = .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                            StgY = .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
                        Else
                            StgX = .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                            StgY = .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2)
                        End If
                        r = System1.EX_START(gSysPrm, StgX, StgY, 0)
                        'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
                        '----- V4.0.0.0-40�� -----
                    Else
                        r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
                    End If
                    'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
                    '----- V2.0.0.0�H�� -----
                    '----- V1.13.0.0�B�� -----
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                        LaserProc = r                                   ' Return�l�ݒ�
                        Exit Function
                    End If
                End If

                '--------------------------------------------------------------------------
                '   ���[�U�n�b�w�Ƀf�[�^��n��
                '--------------------------------------------------------------------------
                ' �����ݒ�(OcxSystem��޼ު��, OcxUtility��޼ު��, Q���[�g[KHz], �ݒ�p���[[W](��߼��), ����Ӱ��(0=�W��), ���sӰ��(0:�蓮))
                r = Me.Ctl_LaserTeach2.SetUp(System1, Utility1, gSysPrm.stDEV.gfLaserQrate, 0.1, 0, 0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    LaserProc = r                                       ' Return�l�ݒ�
                    Exit Function
                End If

                '----- V1.14.0.0�A�� -----
                ' FL�p�p���[��������ݒ肷��
                Call SetFlParamForLaserCmd(stPWR_LSR, stPWR)
                '----- V1.14.0.0�A�� -----
                '---------------------------------------------------------------------------
                '   ���[�U�[��������
                '---------------------------------------------------------------------------
                'V6.0.1.0�@                r = Me.Ctl_LaserTeach2.LaserProc()
                r = Me.Ctl_LaserTeach2.LaserProc(Text4.Left, Text4.Top + DISPOFF_SUBFORM_TOP) 'V6.0.1.0�@
                If (r <> cFRS_NORMAL) Then                              ' �װ ?
                    LaserProc = r                                       ' Return�l�ݒ�
                    Exit Function
                End If
                Me.Refresh()

                '----- V1.14.0.0�G�� -----
                ' FL�����猻�݂̉��H��������M����
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                    r = TrimCondInfoRcv(stCND)
                    If (r <> SerialErrorCode.rRS_OK) Then
                        ' "�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
                        Call System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                        LaserProc = r                                       ' Return�l�ݒ�
                        Exit Function
                    End If
                End If
                '----- V1.14.0.0�G�� -----
                '----- V1.14.0.0�A�� -----
                ' FL�p�p���[���������擾����
                Call GetFlParamForLaserCmd(stPWR_LSR, stPWR)
                '----- V1.14.0.0�A�� -----

                '---------------------------------------------------------------------------
                '   ���[�U�[�����㏈��
                '---------------------------------------------------------------------------
                ' RMCTRL2 >=2 �̏ꍇ�A ���������V�X�p�����ĕ\��("������ = 99.9%")  '###026
                Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)          '###029
                If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then
                    strMSG = LBL_ATT + "  " + CDbl(gSysPrm.stRAT.gfAttRate).ToString("##0.0") + " %"
                    Me.LblRotAtt.Text = strMSG
                End If

                ' ����l�\��(RMCTRL2 >=3 �ő���l�\���w��̏ꍇ)
                If (gSysPrm.stRMC.giRmCtrl2 >= 3) And (gSysPrm.stRMC.giPMonHi = 1) Then '###026
                    ' ����l�擾
                    r = Me.Ctl_LaserTeach2.GetMesPower((gSysPrm.stRAT.gfMesPower))
                    'Call gDllSysprmSysParam_definst.GetSysPrm_ROT_ATT((gSysPrm.stRAT)) '###029
                    ' ����p���[[W]�̕\��
                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    '    strMSG = "���[�U�[�p���[�ݒ�l�@"
                    '    strMSG = strMSG & gSysPrm.stRAT.gfMesPower.ToString("##0.00") & "W"
                    'Else
                    '    strMSG = "Laser Power "
                    '    strMSG = strMSG & gSysPrm.stRAT.gfMesPower.ToString("##0.00") & "W"
                    'End If
                    strMSG = Form1_007 & gSysPrm.stRAT.gfMesPower.ToString("##0.00") & "W"
                    Me.LblMes.Text = strMSG                             ' ����p���[[W]�̕\��
                End If

                ' FL(̧��ްڰ��)���̓f�[�^�X�V�t���O���X�V����Ƃ���(���H�������ύX����Ă���\������̈�)
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then       ' V1.14.0.0�A
                    gCmpTrimDataFlg = 1                                 ' �f�[�^�X�V�t���O = 1(�X�V����)
                End If
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.LaserProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            LaserProc = cERR_TRAP                                       ' Return�l = ��O�G���[
        End Try

    End Function
#End Region
    '----- V1.14.0.0�A�� -----
#Region "FL�p�p���[�����p�����[�^��ݒ肷��"
    '''=========================================================================
    ''' <summary>FL�p�p���[�����p�����[�^��ݒ肷��</summary>
    ''' <param name="stPWR_LSR"></param>
    ''' <param name="stPWR"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetFlParamForLaserCmd(ByRef stPWR_LSR As POWER_ADJUST_INFO, ByRef stPWR As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach.POWER_ADJUST_INFO)

        Dim strMSG As String
        Dim IDX As Integer

        Try
            ' FL�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return

            ' FL�p�p���[�����������[�U�R�}���h�p�p�����[�^�ɐݒ肷��
            For IDX = 0 To (MAX_BANK_NUM - 1)
                stPWR.CndNumAry(IDX) = stPWR_LSR.CndNumAry(IDX)                     ' ���H�����ԍ��z��(0-31)(0=����, 1=�L��)
                stPWR.AdjustTargetAry(IDX) = stPWR_LSR.AdjustTargetAry(IDX)         ' �����ڕW�p���[�z��
                stPWR.AdjustLevelAry(IDX) = stPWR_LSR.AdjustLevelAry(IDX)           ' �p���[�������e�͈͔z��
                stPWR.AttInfoAry(IDX) = stPWR_LSR.AttInfoAry(IDX)                   ' �Œ�ATT���(0:�Œ�ATT Off, 1:�Œ�ATT On)�z��
            Next IDX

            ' FL�p�p���[��������LaserOCX�ɓn��
            Call Me.Ctl_LaserTeach2.SetFLPowerInfo(stPWR)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetFlParamForLaserCmd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "FL�p�p���[�����p�����[�^���擾����"
    '''=========================================================================
    ''' <summary>FL�p�p���[�����p�����[�^���擾����</summary>
    ''' <param name="stPWR_LSR"></param>
    ''' <param name="stPWR"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub GetFlParamForLaserCmd(ByRef stPWR_LSR As POWER_ADJUST_INFO, ByRef stPWR As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach.POWER_ADJUST_INFO)

        Dim strMSG As String
        Dim IDX As Integer
        Dim r As Integer

        Try
            ' FL�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return

            ' FL�p�p���[�������ʂ��擾����
            r = Me.Ctl_LaserTeach2.GetFLPowerResult(stPWR)
            If (r <> cFRS_NORMAL) Then Exit Sub

            ' FL�p�p���[�����������[�U�R�}���h�p�p�����[�^�ɐݒ肷��
            For IDX = 0 To (MAX_BANK_NUM - 1)
                stPWR_LSR.CndNumAry(IDX) = stPWR.CndNumAry(IDX)                     ' ���H�����ԍ��z��(0-31)(0=����, 1=�L��)
                stPWR_LSR.AdjustTargetAry(IDX) = stPWR.AdjustTargetAry(IDX)         ' �����ڕW�p���[�z��
                stPWR_LSR.AdjustLevelAry(IDX) = stPWR.AdjustLevelAry(IDX)           ' �p���[�������e�͈͔z��
                stPWR_LSR.AttInfoAry(IDX) = stPWR.AttInfoAry(IDX)                   ' �Œ�ATT���(0:�Œ�ATT Off, 1:�Œ�ATT On)�z��
            Next IDX

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetFlParamForLaserCmd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.14.0.0�A�� -----
#Region "۷�ݸ�(F6)���݉���������"
    '''=========================================================================
    '''<summary>۷�ݸ�(F6)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdLoging_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLoging.Click

        Dim strMSG As String
        Dim objForm As Logging

        Try
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(True)

            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            Call TrimStateOn(F_LOG, APP_MODE_LOGGING, MSG_OPLOG_FUNC06, "MANUAL")

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' �\�t�g�E�F�A�L�[�{�[�h���N������
            Dim procHandle As Process
            procHandle = New Process
            Call StartSoftwareKeyBoard(procHandle)
            '#End If

            ' ���M���O�ݒ��ʂ̕\��
            objForm = New Logging()                         ' �I�u�W�F�N�g���� 
            ' �_�C�A���O�̕\��
            Call objForm.ShowDialog()                       ' ���M���O�ݒ��ʂ̕\��
            Call objForm.Close()                            ' �I�u�W�F�N�g�J��
            Call objForm.Dispose()                          ' ���\�[�X�J��
            Call LoggingModeDisp()                          ' ۷�ݸޏ��(Logging ON/OFF)�\��"

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' �\�t�g�E�F�A�L�[�{�[�h���I������
            Call EndSoftwareKeyBoard(procHandle)
            '#End If

            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLoging_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        ' �I������
        Call TrimStateOff()                                 ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��

    End Sub
#End Region

#Region "۷�ݸޏ��(Logging ON/OFF)�\��"
    '''=========================================================================
    '''<summary>۷�ݸޏ��(Logging ON/OFF)�\��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub LoggingModeDisp()

        If gSysPrm.stLOG.giLoggingMode Then
            lblLogging.Text = "Logging ON"
        Else
            lblLogging.Text = "Logging OFF"
        End If

    End Sub
#End Region

#Region "��۰��è��ݸ�(F7)���݉���������"
    '''=========================================================================
    '''<summary>��۰��è��ݸ�(F7)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdProbe_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdProbe.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_PROBE, APP_MODE_PROBE, MSG_OPLOG_FUNC07, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdProbe_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "��۰��è��ݸޏ���"
    '''=========================================================================
    '''<summary>��۰��è��ݸޏ���</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function ProbeTeaching(ByRef pltInfo As PlateInfo) As Short
        'Private Function ProbeTeaching(ByVal pltInfo As PlateInfo) As Short

        Dim r As Short
        Dim strMSG As String
        Dim strDAT As String
        Dim rn As Short
        Dim ManualThetaFlg As Short
        Dim W_bpox As Double                                ' Beem Position X OFFSET(mm)
        Dim W_bpoy As Double                                ' Beem Position Y OFFSET(mm)
        Dim W_Xoff As Double                                ' Trim Position Offset X(mm)
        Dim W_Yoff As Double                                ' Trim Position Offset Y(mm)
        'Dim W_XCor As Double                                ' �����X X(mm)
        'Dim W_YCor As Double                                ' �����Y Y(mm)
        '                                                   ' �v���[�u�ڐG�ʒu�m�F�pð���
        Dim PrbHiTBL(MaxRegNum, 2) As Double                '  H�̍��W�i�����̔z��=��R�ԍ�,�E���̔z��=1:X���W�A2:Y���W)
        Dim PrbLoTBL(MaxRegNum, 2) As Double                '  L�̍��W�i�����̔z��=��R�ԍ�,�E���̔z��=1:X���W�A2:Y���W)
        Dim RnoTbl(MaxRegNum) As Short                      ' ��R�ԍ��e�[�u��
        Dim PrbTbl(MaxRegNum, 7) As Short                   ' �v���[�u�ԍ��e�[�u��
        Dim NomTbl(MaxRegNum) As Double                     ' �ڕW�l�e�[�u��
        Dim SlpTbl(MaxRegNum) As Short                      ' �d���ω��X���[�v�e�[�u��
        Dim StPosXTbl(MaxRegNum) As Double                  ' �J�b�g�J�n�ʒuX�e�[�u��
        Dim StPosYTbl(MaxRegNum) As Double                  ' �J�b�g�J�n�ʒuY�e�[�u��
        Dim FtTbl(MaxRegNum, 2) As Double                   ' ̧���ýď���l/�����l(���g�p���������Ұ��ɂ���̂�)
        Dim parModules As MainModules
        parModules = New MainModules
        Dim gCntRegData As Integer                          ' �v���[�u�R�}���h�ɕK�v�ȏ���n��(��R��)     V6.0.1.023

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   �ƕ␳(��߼��) & XY�e�[�u�����g�����ʒu�ֈړ�����
                '--------------------------------------------------------------------------
                ProbeTeaching = cFRS_NORMAL                     ' Return�l = ����
                W_bpox = .dblBpOffSetXDir                       ' �ްшʒu�̾��X
                W_bpoy = .dblBpOffSetYDir                       ' �ްшʒu�̾��Y
                W_Xoff = .dblTableOffsetXDir                    ' ð��وʒu�̾��X
                W_Yoff = .dblTableOffsetYDir                    ' ð��وʒu�̾��Y
                'Call BSIZE(.dblBlockSizeXDir, .dblBlockSizeYDir) 
                r = Move_Trimposition(pltInfo, 0.0, 0.0)        ' �ƕ␳(��߼��) & XYð�����шʒu�ړ�
                SetTeachVideoSize()                             ' V6.0.3.0_49
                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    ProbeTeaching = r                           ' Return�l�ݒ�
                    Exit Function
                End If

                ' BP�̾�Đݒ�
                'Call System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then GoTo STP_END
                gCntRegData = 0  'V6.0.1.023
                '--------------------------------------------------------------------------
                '   �v���[�u�n�b�w�ɓn���e�[�u�����쐬����
                '--------------------------------------------------------------------------
                For rn = 1 To gRegistorCnt                              ' ��R�����ݒ肷��
                    If (typResistorInfoArray(rn).intResNo < 1000) Then  ' �}�[�L���O�p��R�ȊO ? ###055
                        ' ��R�ԍ�
                        'V5.0.0.6�P                        RnoTbl(rn) = rn
                        RnoTbl(rn) = typResistorInfoArray(rn).intResNo  'V5.0.0.6�P
                        ' �v���[�u�ԍ�(H,L,G1�`G5)
                        PrbTbl(rn, 1) = typResistorInfoArray(rn).intProbHiNo
                        PrbTbl(rn, 2) = typResistorInfoArray(rn).intProbLoNo
                        PrbTbl(rn, 3) = typResistorInfoArray(rn).intProbAGNo1
                        PrbTbl(rn, 4) = typResistorInfoArray(rn).intProbAGNo2
                        PrbTbl(rn, 5) = typResistorInfoArray(rn).intProbAGNo3
                        PrbTbl(rn, 6) = typResistorInfoArray(rn).intProbAGNo4
                        PrbTbl(rn, 7) = typResistorInfoArray(rn).intProbAGNo5
                        ' �ڕW�l
                        NomTbl(rn) = typResistorInfoArray(rn).dblTrimTargetVal
                        ' �d���ω��X���[�v(1:+�X���[�v, 2:-�X���[�v, 4:��R)
                        SlpTbl(rn) = typResistorInfoArray(rn).intSlope
                        'If (.intMeasType = 0) Then '###011
                        If (typResistorInfoArray(rn).intResMeasMode = 0) Then ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��) '###011
                            SlpTbl(rn) = 4
                        End If
                        ' �J�b�g�J�n�ʒuX,Y�e�[�u��
                        StPosXTbl(rn) = typResistorInfoArray(rn).ArrCut(1).dblStartPointX
                        StPosYTbl(rn) = typResistorInfoArray(rn).ArrCut(1).dblStartPointY

                        ' ������ V3.1.0.0�A 2014/12/01
                        ' ����͈�
                        If giMeasurement = 1 Then           ' �C�j�V�����e�X�g
                            ' HIGH
                            FtTbl(rn, 1) = typResistorInfoArray(rn).dblInitTest_HighLimit
                            ' LOW
                            FtTbl(rn, 2) = typResistorInfoArray(rn).dblInitTest_LowLimit
                        ElseIf giMeasurement = 2 Then       ' �t�@�C�i���e�X�g
                            ' HIGH
                            FtTbl(rn, 1) = typResistorInfoArray(rn).dblFinalTest_HighLimit
                            ' LOW
                            FtTbl(rn, 2) = typResistorInfoArray(rn).dblFinalTest_LowLimit
                        Else                                ' ���܂Œʂ�
                            ' HIGH
                            FtTbl(rn, 1) = gdRESISTOR_MAX
                            ' LOW
                            FtTbl(rn, 2) = gdRESISTOR_MIN
                        End If
                        ' ������ V3.1.0.0�A 2014/12/01

                        gCntRegData = gCntRegData + 1  'V6.0.1.023

                    End If
                Next rn

                ' 'V6.0.1.023�@�v���[�u�ɕK�v�ȏ���n�� 
                Probe1.SetTrimmingDataName(gStrTrimFileName, gCntRegData)
                Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)                'V6.0.4.0�A

                '--------------------------------------------------------------------------
                '   �v���[�u�n�b�w�Ƀf�[�^��n��
                '--------------------------------------------------------------------------
                Probe1.SetMainObject(parModules)                ' �e���W���[���̃��\�b�h��ݒ肷��
                r = Probe1.Setup(RnoTbl, W_bpox, W_bpoy, .dblBlockSizeXDir, .dblBlockSizeYDir,
                                 .dblTableOffsetXDir, .dblTableOffsetYDir,
                                 .dblZOffSet, .dblZWaitOffset,
                                 PrbTbl, SlpTbl, NomTbl, FtTbl, 0, StPosXTbl, StPosYTbl, 7500, 105,
                                 gfCorrectPosX, gfCorrectPosY,
                                 .intBlockCntXDir, .intBlockCntYDir,
                                 .dblLwPrbStpUpDist, 0.0)

                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    ProbeTeaching = r                           ' Return�l�ݒ�
                    strDAT = "Setup() "
                    GoTo STP_ERR
                End If

                '--------------------------------------------------------------------------
                '   �ƃ}�j���A������������(�蓮�␳���[�h�ŁA�␳�Ȃ��̎��L��)
                '   ���jSetup()�̌��Call����
                '--------------------------------------------------------------------------
                ' �␳���[�h���蓮�ŕ␳�Ȃ�?
                If (.intReviseMode = 1) And (.intManualReviseType = 0) Then
                    ManualThetaFlg = 1                          ' ���ƭ�ْ�����L���Ƃ���
                Else
                    ManualThetaFlg = 0                          ' ���ƭ�ْ����𖳌��Ƃ���
                End If

                ' �ƃ}�j���A������������
                r = Probe1.SetupTheta(ManualThetaFlg, .intReviseMode, .intManualReviseType,
                                      cThetaAnglMin, cThetaAnglMax, .dblRotateTheta)
                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    ProbeTeaching = r                           ' Return�l�ݒ�
                    strDAT = "SetupTheta() "
                    GoTo STP_ERR
                End If

                '--------------------------------------------------------------------------
                '   �v���[�u�ڐG�ʒu�m�F�@�\(��߼��)�O����
                '--------------------------------------------------------------------------
                ' �v���[�u�ڐG�ʒu�m�F�pð��ق�ݒ肷��
                For rn = 1 To gRegistorCnt                      ' ��R�����ݒ肷��
                    ' �v���[�u�m�F�ʒuHI X,Y���W 
                    PrbHiTBL(rn, 1) = typResistorInfoArray(rn).dblProbCfmPoint_Hi_X
                    PrbHiTBL(rn, 2) = typResistorInfoArray(rn).dblProbCfmPoint_Hi_Y
                    PrbLoTBL(rn, 1) = typResistorInfoArray(rn).dblProbCfmPoint_Lo_X
                    PrbLoTBL(rn, 2) = typResistorInfoArray(rn).dblProbCfmPoint_Lo_Y
                Next rn

                ' �v���[�u�ڐG�ʒu�m�F�@�\�����ݒ�(��߼��)
                r = Probe1.SetupPrbPosChk(gRegistorCnt, .dblAdjOffSetXDir, .dblAdjOffSetYDir,
                                          65, 650, PrbHiTBL, PrbLoTBL)

                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    ProbeTeaching = r                           ' Return�l�ݒ�
                    strDAT = "SetupPrbPosChk() "
                    GoTo STP_ERR
                End If

                '----- ###262�� -----
                r = SendTrimData()
                If (r <> cFRS_NORMAL) Then
                    ' "�g���~���O�f�[�^�̐ݒ�Ɏ��s���܂����B" & vbCrLf & "�g���~���O�f�[�^�ɖ�肪�Ȃ����m�F���Ă��������B"
                    Call Me.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    ProbeTeaching = cFRS_ERR_RST                ' Return�l = RESET
                    GoTo STP_ERR
                End If
                '----- ###262�� -----

                '--------------------------------------------------------------------------
                '   �v���[�u�e�B�[�`���O��ʕ\��
                '--------------------------------------------------------------------------
                Probe1.Visible = True
                Probe1.BringToFront()                           ' �őO�ʂ֕\��
                r = Probe1.START()                              ' �v���[�u����
                Probe1.Visible = False
                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    ProbeTeaching = r
                    strDAT = "START() "
                    GoTo STP_ERR
                End If

                '--------------------------------------------------------------------------
                '   �v���[�u�������ʂ��擾����
                '--------------------------------------------------------------------------
                ' �v���[�u�������ʎ擾(��шʒu�̾��X,Y/ZON�ʒu,Z2ON�ʒu�X�V)
                ''V1.13.0.0�C r = Probe1.GetResult(.dblTableOffsetXDir, .dblTableOffsetYDir, _
                '                     .dblZOffSet, WK_Z2ON)
                r = Probe1.GetResult(.dblTableOffsetXDir, .dblTableOffsetYDir,
                                     .dblZOffSet, .dblLwPrbStpUpDist)

                If (r = cFRS_NORMAL) Then                       ' ����(Cancel�łȂ�) ?

                    '' PROBE2����ނȂ�XY�ړ���BP�����炷(Teach����ނ��Ȃ���)
                    'If (giAppMode = APP_MODE_PROBE2) Then       ' PROBE2����� ?
                    '    W_XCor = W_Xoff - (stPLT.z_xoff - gfCorrectPosX) ' W_Xoff = XY�ړ��� X
                    '    W_YCor = W_Yoff - (stPLT.z_yoff - gfCorrectPosY) ' W_Yoff = XY�ړ��� Y
                    '    stPLT.BPOX = W_bpox - W_XCor            ' BP Offset X(mm)�}�����X
                    '    stPLT.BPOY = W_bpoy + W_YCor            ' BP Offset Y(mm)�}�����Y
                    'End If

                    ' ��шʒu�̾��X,Y���X�V����(XYð��ٕ␳��������)
                    .dblTableOffsetXDir = .dblTableOffsetXDir - gfCorrectPosX
                    .dblTableOffsetYDir = .dblTableOffsetYDir - gfCorrectPosY
                    'V4.7.0.0�N��
                    '----- V6.1.4.0�O��(KOA EW�aSL432RD�Ή�) -----
                    ' BP_INPUT=1(�e�[�u���I�t�Z�b�g�l�̋t����BP�I�t�Z�b�g�ɐݒ�) ?
                    If (gSysPrm.stCTM.giBPOffsetInput <> 0) Then
                        .dblBpOffSetXDir = -1.0 * .dblTableOffsetXDir
                        .dblBpOffSetYDir = -1.0 * .dblTableOffsetYDir
                    End If
                    '----- V6.1.4.0�O�� -----
                    ' �V�X�e���ϐ��ݒ�(�v���[�uON/OFF�ʒu��)
                    Call PROP_SET(.dblZOffSet, .dblZWaitOffset,
                                  gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, gSysPrm.stDEV.gfSmaxX, gSysPrm.stDEV.gfSmaxY)
                    gCmpTrimDataFlg = 1                         ' �f�[�^�X�V�t���O = 1(�X�V����)
                    commandtutorial.SetProbeExecute()           ' V2.0.0.0�I
                End If

                ' �ƃ}�j���A���������ʎ擾(�Ɖ�]�p�x�X�V)
                If (ManualThetaFlg = 1) Then                    ' ###258���ƭ�ْ������L���̎��Ɏ擾����
                    r = Probe1.GetResultTheta(.dblRotateTheta)
                    If (r = cFRS_NORMAL) Then                   ' ���� ?
                        gCmpTrimDataFlg = 1                     ' �f�[�^�X�V�t���O = 1(�X�V����)
                    End If
                End If                                          ' ###258

                ' �v���[�u�e�B�[�`���O����(�v���[�u�ڐG�ʒu)���擾����(��߼��)
                r = Probe1.GetResultPrbPosChk(PrbHiTBL, PrbLoTBL)
                If (r = cFRS_NORMAL) Then                       ' ���� ?
                    For rn = 1 To gRegistorCnt                  ' ��R�����ݒ肷��
                        ' �v���[�u�m�F�ʒuHI X,Y���W�X�V 
                        typResistorInfoArray(rn).dblProbCfmPoint_Hi_X = PrbHiTBL(rn, 1)
                        typResistorInfoArray(rn).dblProbCfmPoint_Hi_Y = PrbHiTBL(rn, 2)
                        typResistorInfoArray(rn).dblProbCfmPoint_Lo_X = PrbLoTBL(rn, 1)
                        typResistorInfoArray(rn).dblProbCfmPoint_Lo_Y = PrbLoTBL(rn, 2)
                        gCmpTrimDataFlg = 1                     ' �f�[�^�X�V�t���O = 1(�X�V����)
                    Next rn
                End If

                Call TRIMEND()                                  ' �������J�� ###262
                ProbeTeaching = r                               'V4.10.0.0�B
                Exit Function

STP_ERR:
                ' �v���[�u�n�b�w�G���[��
                'strMSG = "i-TKY.ProbeTeaching() PROBE.OCX ERROR = " + strDAT + r.ToString
                'MsgBox(strMSG)
                'ProbeTeaching = cERR_TRAP                       ' Return�l = ��O�G���[
                Call TRIMEND()                                  ' �������J�� ###262
                Exit Function
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.ProbeTeaching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            ProbeTeaching = cERR_TRAP                           ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "è��ݸ�(F8)���݉���������"
    '''=========================================================================
    '''<summary>è��ݸ�(F8)���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTeach_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTeach.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �␳�N���X���C���\��
            ' ���␳�N���X���C���\����VB6��.NET��PictueBox�̌`�����قȂ邽��Video.OCX�̃N���X���C�����g�p����
            'VideoLibrary1.gPicture1.Visible = True              ' Video.OCX�̃N���X���C����\������
            'VideoLibrary1.gPicture2.Visible = True
            'Picture1.Visible = True                            ' Form1�̃N���X���C���͔�\���Ƃ���
            'Picture2.Visible = True
            'CrosLineX.Visible = True
            'CrosLineY.Visible = True

            ' �R�}���h���s
            r = CmdExec_Proc(F_TEACH, APP_MODE_TEACH, MSG_OPLOG_FUNC08, "")

            '' �␳�N���X���C����\��
            'Picture1.Visible = True                             ' Form1�̃N���X���C����\������
            'Picture2.Visible = True
            'VideoLibrary1.gPicture1.Visible = False             ' Video.OCX�̃N���X���C�����\���Ƃ���
            'VideoLibrary1.gPicture2.Visible = False
            'VideoLibrary1.gCrosLineXY1.Visible = False
            'VideoLibrary1.gCrosLineXY2.Visible = False

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTeach_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�e�B�[�`���O����"
    '''=========================================================================
    '''<summary>�e�B�[�`���O����</summary>
    '''<param name="iAppMode">(INP)�A�v�����[�h(giAppMode�Q��)
    '''                            APP_MODE_TEACH         = �e�B�[�`���O
    '''                            APP_MODE_EXCAM_R1TEACH = �O���J����R1�e�B�[�`���O
    '''                            APP_MODE_EXCAM_TEACH   = �O���J�����e�B�[�`���O</param> 
    '''<param name="pltInfo"> (I/O)�v���[�g�f�[�^</param> 
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''<remarks>Teach.OCX���g�p</remarks>
    '''=========================================================================
    Private Function Teaching(ByVal iAppMode As Short, ByRef pltInfo As PlateInfo) As Short

        Dim strMSG As String
        Dim strDAT As String
        Dim r As Short
        Dim idx As Short
        Dim rn As Short
        Dim cn As Short
        Dim dirL1 As Short                                              ' �J�b�g����(1:-X��,2:+Y��, 3:+X:��, 4:-Y��)
        Dim dirL2 As Short
        Dim dblTmpBpX As Double
        Dim dblTmpBpY As Double
        Dim sRName() As String                                          ' ��R��(��R�ԍ�)�e�[�u��
        Dim RnCnTbl(,) As Short                                         ' ��R�ԍ�,�J�b�g�ԍ��e�[�u��
        Dim StartPosTbl(,) As Double                                    ' �J�b�g�J�n�ʒu�e�[�u��
        Dim TblOfsX As Double
        Dim TblOfsY As Double
        Dim dStartPointX As Double, dStartPointY As Double              ' V6.1.4.0�N

        ' �O���J�����e�B�[�`���O�p�p�����[�^
        Dim stPLT As TEACH_PLATE_INFO                                   ' �v���[�g��� (��Teach.OCX�Œ�`)
        Dim stSTP(MaxCntStep) As TEACH_STEP_INFO                        ' �X�e�b�v���
        Dim stGRP(MaxCntStep) As TEACH_GROP_INFO                        ' �O���[�v���
        Dim stTY2(MaxCntTy2) As TEACH_TY2_INFO                          ' �s�x�Q���

        Dim dX As Double, dY As Double                                  'V5.0.0.6�K
        Dim arrayCnt As Integer
        Dim parModules As MainModules
        parModules = New MainModules

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   �e�B�[�`���O�n�b�w�ɓn���J�b�g�J�n�ʒu�e�[�u�����쐬����
                '--------------------------------------------------------------------------
                idx = 0                                                 ' �e�[�u���C���f�b�N�X

                '�J�b�g�J�n�ʒu�ۑ��e�[�u���𓮓I�Ɋm��
                '   �S��R�̃J�b�g�����v�Z����B
                For rn = 1 To gRegistorCnt
                    arrayCnt = arrayCnt + typResistorInfoArray(rn).intCutCount
                Next
                ReDim sRName(arrayCnt)
                ReDim RnCnTbl(arrayCnt, 2)
                ReDim StartPosTbl(2, arrayCnt)

                For rn = 1 To gRegistorCnt                              ' ��R�����ݒ肷�� 
                    If (typResistorInfoArray(rn).intResNo < 1) Then Exit For
                    ' ��R���J�b�g�����ݒ肷��
                    For cn = 1 To typResistorInfoArray(rn).intCutCount
                        idx = idx + 1                                   ' �e�[�u���C���f�b�N�X + 1
                        ' ��R��(��R�ԍ�)
                        sRName(idx) = typResistorInfoArray(rn).intResNo.ToString()
                        ' ��R�ԍ�,�J�b�g�ԍ��e�[�u��
                        RnCnTbl(idx, 0) = rn                            ' ��R�ԍ�
                        RnCnTbl(idx, 1) = cn                            ' �J�b�g�ԍ�
                        ' �J�b�g�J�n�ʒuX,Y�e�[�u��(StartPosTbl)��ݒ肷��
                        If (iAppMode = APP_MODE_TEACH) Then
                            ' �e�B�[�`���O�̏ꍇ
                            StartPosTbl(1, idx) = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX
                            StartPosTbl(2, idx) = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY
                        Else
                            ' �O���J����R1�e�B�[�`���O/�O���J�����e�B�[�`���O�̏ꍇ
                            '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                            'If gSysPrm.stCTM.giTEACH_P <> 0 Then      ' �e�B�[�`���O�|�C���g�g�p�̎�
                            If (gSysPrm.stCTM.giTEACH_P = 1) Then      ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                                ' dStartPointX,Y�Ƀe�B�[�`���O�|�C���gX,Y��ޔ�����
                                dStartPointX = typResistorInfoArray(rn).ArrCut(cn).dblTeachPointX
                                dStartPointY = typResistorInfoArray(rn).ArrCut(cn).dblTeachPointY
                            Else
                                ' dStartPointX,Y�ɃX�^�[�g�|�C���gX,Y��ޔ�����
                                dStartPointX = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX
                                dStartPointY = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY
                            End If
                            '----- V6.1.4.0�N�� -----
                            '----- V1.14.0.0�C�� -----
                            ' �O���J�����̎�
                            '----- V6.1.4.0�N�� -----
                            ' dX,dY=�X�^�[�g�|�C���gX,Y���̓e�B�[�`���O�|�C���gX,Y 
                            'dX = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX 'V5.0.0.6�K
                            'dY = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY 'V5.0.0.6�K
                            dX = dStartPointX
                            dY = dStartPointY
                            '----- V6.1.4.0�N�� -----
                            UpdateByCrossLineToExt(dX, dY)                          'V5.0.0.6�K

                            ' �e�B�[�`���O��ʂ̃e�B�[�`���O�ʒuX,Y��ݒ�
                            Select Case gSysPrm.stDEV.giBpDirXy
                                Case 0 ' x��, y��
                                    StartPosTbl(1, idx) = dX
                                    StartPosTbl(2, idx) = dY
                                Case 1 ' x��, y��(X���])
                                    StartPosTbl(1, idx) = dX * -1
                                    StartPosTbl(2, idx) = dY
                                Case 2 ' x��, y��(Y���])
                                    StartPosTbl(1, idx) = dX
                                    StartPosTbl(2, idx) = dY * -1
                                Case 3 ' x��, y��(XY���])
                                    StartPosTbl(1, idx) = dX * -1
                                    StartPosTbl(2, idx) = dY * -1
                            End Select
                            '----- V1.14.0.0�C�� -----
                        End If
                    Next cn
                Next rn

                '--------------------------------------------------------------------------
                '   �e�B�[�`���O�n�b�w�Ƀf�[�^��n��
                '--------------------------------------------------------------------------
                ' �N���X���C���\���p  ###232
                r = Teaching1.SetCrossLineObject(gparModules)
                If (r <> cFRS_NORMAL) Then
                    MsgBox("i-TKY.teaching() SetCrossLineObject ERROR")
                End If

                ' �e�B�[�`���O�n�b�w�ɉ摜�\���v���O�����̕\���ʒu��n�� ###052
                Teaching1.dispXPos = VideoLibrary1.Location.X
                Teaching1.dispYPos = VideoLibrary1.Location.Y

                ' �e�B�[�`���O�n�b�w�Ƀf�[�^��n��
                r = Teaching1.Setup(RnCnTbl, .dblBpOffSetXDir, .dblBpOffSetYDir,
                                  .dblBlockSizeXDir, .dblBlockSizeYDir, sRName, gSysPrm.stDEV.giBpDirXy, iAppMode)  ' iAppMode�ǉ� V6.1.4.0�N
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Teaching = r                                        ' Return�l�ݒ�
                    strDAT = "Setup() "
                    GoTo STP_ERR
                End If
                ''V6.0.1.0�M��
                ' �e�B�[�`���O�n�b�w�Ƀf�[�^��n��
                'r = Teaching1.SetQrate(stCND.Freq(ADJ_CND_NUM))
                ''V6.0.1.0�M��

                '----- V1.13.0.0�@�� -----
                r = Teaching1.SetZ2Pos(.dblLwPrbStpUpDist, 0.0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Teaching = r                                        ' Return�l�ݒ�
                    GoTo STP_ERR
                End If
                '----- V1.13.0.0�@�� -----

                ' �J�b�g���̃J�b�g�ʒuXY�f�[�^���R���g���[���ɓn��
                idx = 0                                                 ' �e�[�u���C���f�b�N�X
                For rn = 1 To gRegistorCnt                              ' ��R�����ݒ肷�� 
                    ' ��R���J�b�g�����J�Ԃ�
                    For cn = 1 To typResistorInfoArray(rn).intCutCount
                        idx = idx + 1                                   ' �e�[�u���C���f�b�N�X + 1
                        ' �J�b�g�g���[�X�̂��߂̃J�b�g������ݒ肷��
                        Call Cnv_Cut_Dir(rn, cn, dirL1, dirL2)
                        ' �e�J�b�g�g���[�X�̂��߃Z�b�g�A�b�v����
                        Call Sub_Cut_Setup(rn, cn, dirL1, dirL2, RnCnTbl, StartPosTbl, idx)
                    Next cn
                Next rn

                ' �O���J�����e�B�[�`���O���͊O���J�����ɐؑւ���
                If (iAppMode = APP_MODE_TEACH) Then
                    TblOfsX = 0.0#
                    TblOfsY = 0.0#
                Else
                    ' XY�e�[�u�������R�̑��J�b�g�ʒu�ֈړ�����(�O���J�����e�B�[�`���O��)
                    '----- ###249�� -----
                    'TblOfsX = gSysPrm.stDEV.gfExCmX                     ' �O���J�����I�t�Z�b�gX�ݒ� ###075
                    'TblOfsY = gSysPrm.stDEV.gfExCmY                     ' �O���J�����I�t�Z�b�gY�ݒ� ###075
                    ' �u���b�N�T�C�Y/2�����Z���a�o�I�t�Z�b�g�����Z���� ###075
                    'TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                    'TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir

                    '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                    'If gSysPrm.stCTM.giTEACH_P <> 0 Then               ' �e�B�[�`���O�|�C���g�g�p�̎�
                    If (gSysPrm.stCTM.giTEACH_P = 1) Then               ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                        ' dStartPointX,Y�ɑ�1��R�̑�1�J�b�g�̃e�B�[�`���O�|�C���gX,Y��ޔ�����
                        dStartPointX = typResistorInfoArray(1).ArrCut(1).dblTeachPointX
                        dStartPointY = typResistorInfoArray(1).ArrCut(1).dblTeachPointY
                    Else
                        ' dStartPointX,Y�ɑ�1��R�̑�1�J�b�g�̃X�^�[�g�|�C���gX,Y��ޔ�����
                        dStartPointX = typResistorInfoArray(1).ArrCut(1).dblStartPointX
                        dStartPointY = typResistorInfoArray(1).ArrCut(1).dblStartPointY
                    End If
                    '----- V6.1.4.0�N�� -----
                    '----- V6.1.4.0�N�� -----
                    ' BP��R�[�i�[���l��(INtime��Cmd_Start()�Q��)
                    Select Case gSysPrm.stDEV.giBpDirXy
                        Case 0 ' �E��(x��, y��)
                            TblOfsX = gSysPrm.stDEV.gfExCmX + dStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                            TblOfsY = gSysPrm.stDEV.gfExCmY + dStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                            '----- V1.24.0.0�@�� -----
                            If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                                TblOfsX = gSysPrm.stDEV.gfExCmX + dStartPointX + .dblBpOffSetXDir
                                TblOfsY = gSysPrm.stDEV.gfExCmY + dStartPointY + .dblBpOffSetYDir
                            End If
                            '----- V1.24.0.0�@�� -----
                        Case 1 ' ����(x��, y��)
                            TblOfsX = (gSysPrm.stDEV.gfExCmX - dStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                            TblOfsY = gSysPrm.stDEV.gfExCmY + dStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                        Case 2 ' �E��(x��, y��)
                            TblOfsX = gSysPrm.stDEV.gfExCmX + dStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                            TblOfsY = (gSysPrm.stDEV.gfExCmY + dStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                        Case 3 ' ����(x��, y��)
                            TblOfsX = (gSysPrm.stDEV.gfExCmX + dStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                            TblOfsY = (gSysPrm.stDEV.gfExCmY + dStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                    End Select

                    'Select Case gSysPrm.stDEV.giBpDirXy
                    '    Case 0 ' �E��(x��, y��)
                    '        TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                    '        TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                    '        '----- V1.24.0.0�@�� -----
                    '        If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                    '            TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX + .dblBpOffSetXDir
                    '            TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY + .dblBpOffSetYDir
                    '        End If
                    '        '----- V1.24.0.0�@�� -----
                    '    Case 1 ' ����(x��, y��)
                    '        TblOfsX = (gSysPrm.stDEV.gfExCmX - typResistorInfoArray(1).ArrCut(1).dblStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                    '        TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                    '    Case 2 ' �E��(x��, y��)
                    '        TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                    '        TblOfsY = (gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                    '    Case 3 ' ����(x��, y��)
                    '        TblOfsX = (gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                    '        TblOfsY = (gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                    'End Select
                    '----- ###249�� -----
                    '----- V6.1.4.0�N�� -----

                    ' �O���J�����ɐؑւ���
                    Call VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)
                End If

                '--------------------------------------------------------------------------
                '   �ƕ␳(��߼��) & XY�e�[�u�� & Z���g�����ʒu�ֈړ�����
                '--------------------------------------------------------------------------
                Teaching = cFRS_NORMAL                                  ' Return�l = ����
                'Call BSIZE(stPLT.zsx, stPLT.zsy)                       ' �u���b�N�T�C�Y�ݒ�
                r = Move_Trimposition(pltInfo, TblOfsX, TblOfsY)        ' �ƕ␳(��߼��) & XYð�����шʒu�ړ�
                SetTeachVideoSize()                                     ' V6.0.3.0_49
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Teaching = r                                        ' Return�l�ݒ�
                    Exit Function
                End If

                ' BP�̾�Đݒ�
                'Call System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then GoTo STP_END

                ' �O���J�����e�B�[�`���O�p�p�����[�^�ݒ�
                If (iAppMode = APP_MODE_EXCAM_R1TEACH) Or (iAppMode = APP_MODE_EXCAM_TEACH) Then
                    Call VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)                  ' �O���J�����ɐؑւ���'V1.14.0.0�@
                    'V6.1.4.9�@                    Call SetExCamTeachParam(iAppMode, pltInfo, stPLT, stSTP, stGRP, stTY2)
                    Call SetExCamTeachParam(iAppMode, pltInfo, stPLT, stSTP, stGRP, stTY2, gTkyKnd)  'V6.1.4.9�@ gTkyKnd�ǉ�
                    Call Teaching1.SetupExcamTeach(iAppMode, stPLT, stSTP, stGRP, stTY2)
                End If

                '--------------------------------------------------------------------------
                '   �e�B�[�`���O��ʕ\��
                '--------------------------------------------------------------------------
                Teaching1.ZOFF = .dblZWaitOffset                        ' Z PROBE OFF OFFSET(mm)
                Teaching1.ZON = .dblZOffSet                             ' Z PROBE ON OFFSET(mm)
                ' �}�[�L���O�ʒu�̕\���̂��߁A�r�f�I���C�u�����̕`��I�u�W�F�N�g��n��
                'Teaching1.set_formObject(VideoLibrary1.shpObject)
                ' �e���W���[���̃��\�b�h��ݒ肷��B
                Teaching1.SetMainObject(parModules)

                ''ZOff�ʒu�ֈړ�
                'r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)

                SetTeachVideoSize()                                     'V2.0.0.0�Q

                ' �e�B�[�`���O�̃R���g���[����\������
                Teaching1.Visible = True
                Teaching1.BringToFront()                                ' �őO�ʂ֕\��

                ''V1.16.0.1�A
                ' ���H�����ԍ���ݒ肷��(FL��)
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then       ' FL ?
                    Call QRATE(stCND.Freq(ADJ_CND_NUM))                 ' Q���[�g�ݒ�(KHz)
                    r = FLSET(FLMD_CNDSET, ADJ_CND_NUM)                 ' ���H�����ԍ��ݒ�(�ꎞ��~��ʗp)
                Else
                    Call QRATE(gSysPrm.stDEV.gfLaserQrate)              ' Q���[�g�ݒ�(KHz) �����[�U�����pQ���[�g��ݒ�
                    ''V6.0.1.0�M��
                    ' �e�B�[�`���O�n�b�w�Ƀf�[�^��n��
                    r = Teaching1.SetQrate(gSysPrm.stDEV.gfLaserQrate)
                    ''V6.0.1.0�M��

                End If
                ''V1.16.0.1�A

                ' �e�B�[�`���O���������s����
                Select Case (iAppMode)
                    Case APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH
                        r = Teaching1.StartExcamTeach(iAppMode)         ' �O���J�����e�B�[�`���O
                    Case Else
                        r = Teaching1.START()                           ' �e�B�[�`���O
                End Select

                ' �e�B�[�`���O�̃R���g���[�����\���ɂ���
                Teaching1.Visible = False

                '--------------------------------------------------------------------------
                '   �e�B�[�`���O���ʎ擾
                '--------------------------------------------------------------------------
                If (r = cFRS_NORMAL) Then                               ' �e�B�[�`���O��������I�� ?
                    If (Teaching1.Getresult(dblTmpBpX, dblTmpBpY, StartPosTbl) = 0) Then
                        ' �r�[���|�W�V�����I�t�Z�b�g�l�X�V
                        If (iAppMode = APP_MODE_TEACH) Then
                            .dblBpOffSetXDir = dblTmpBpX
                            .dblBpOffSetYDir = dblTmpBpY
                        End If

                        If (iAppMode = APP_MODE_TEACH Or iAppMode = APP_MODE_EXCAM_TEACH) Then
                            ' ��R�����J�b�g�ʒuXY��ݒ肷��
                            idx = 0                                         ' �e�[�u���C���f�b�N�X
                            For rn = 1 To gRegistorCnt                      ' ��R�����ݒ肷�� 
                                ' ��R���J�b�g�����ݒ肷��
                                For cn = 1 To typResistorInfoArray(rn).intCutCount
                                    idx = idx + 1                           ' �e�[�u���C���f�b�N�X + 1
                                    '                                       ' �J�b�g�ʒuXY�X�V
                                    If (iAppMode = APP_MODE_TEACH) Then
                                        typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx)
                                        typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx)
                                    Else
                                        '----- V1.14.0.0�C�� -----
                                        ' �O���J�����̎�
                                        UpdateByCrossLineToIN(StartPosTbl(1, idx), StartPosTbl(2, idx)) 'V5.0.0.6�K
                                        '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                                        ' dStartPointX,Y = �X�^�[�g�|�C���gX,Y�܂��̓e�B�[�`���O�|�C���gX,Y
                                        Select Case gSysPrm.stDEV.giBpDirXy
                                            Case 0 ' x��, y��
                                                dStartPointX = StartPosTbl(1, idx)
                                                dStartPointY = StartPosTbl(2, idx)
                                            Case 1 ' x��, y��(X���])
                                                dStartPointX = StartPosTbl(1, idx) * -1
                                                dStartPointY = StartPosTbl(2, idx)
                                            Case 2 ' x��, y��(Y���])
                                                dStartPointX = StartPosTbl(1, idx)
                                                typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx) * -1
                                            Case 3 ' x��, y��(XY���])
                                                dStartPointX = StartPosTbl(1, idx) * -1
                                                dStartPointY = StartPosTbl(2, idx) * -1

                                                'Case 0 ' x��, y��
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx)
                                                'Case 1 ' x��, y��(X���])
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx) * -1
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx)
                                                'Case 2 ' x��, y��(Y���])
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx) * -1
                                                'Case 3 ' x��, y��(XY���])
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx) * -1
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx) * -1
                                        End Select
                                        '----- V1.14.0.0�C�� -----

                                        'If gSysPrm.stCTM.giTEACH_P <> 0 Then      ' �e�B�[�`���O�|�C���g�g�p�̎�
                                        If (gSysPrm.stCTM.giTEACH_P = 1) Then      ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                                            ' �X�^�[�g�|�C���gX,Y���A�e�B�[�`���O�|�C���g�Ƃ̍��������Z���čX�V
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX + (dStartPointX - typResistorInfoArray(rn).ArrCut(cn).dblTeachPointX)
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY + (dStartPointY - typResistorInfoArray(rn).ArrCut(cn).dblTeachPointY)
                                            ' �e�B�[�`���O�|�C���gX,Y�X�V
                                            typResistorInfoArray(rn).ArrCut(cn).dblTeachPointX = dStartPointX
                                            typResistorInfoArray(rn).ArrCut(cn).dblTeachPointY = dStartPointY
                                        Else
                                            ' �X�^�[�g�|�C���gX,Y�X�V
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = dStartPointX
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = dStartPointY
                                        End If
                                        '----- V6.1.4.0�N�� -----
                                    End If
                                Next cn
                            Next rn
                        ElseIf (iAppMode = APP_MODE_EXCAM_R1TEACH) Then
                            'R1�e�B�[�`���O�̏ꍇ�A�擪��R�̃f�[�^��S��R�֓W�J
                            Call SetR1Data2AllResistor(StartPosTbl)
                        End If
                        gCmpTrimDataFlg = 1                             ' �f�[�^�X�V�t���O = 1(�X�V����)
                        commandtutorial.SetTeachExecute()               ' V2.0.0.0�I
                    End If
                Else
                    Teaching = r                                        ' Return�l =cFRS_ERR_RST(�L�����Z��)�@����ȊO= �G���[
                End If

                ' �O���J�����e�B�[�`���O���͓����J�����ɐؑւ���
                If (iAppMode <> APP_MODE_TEACH) Then
                    Call VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                  ' �����J�����ɐؑւ���
                End If

                'SetSimpleVideoSize()           'V2.0.0.0�Q  'V4.10.0.0�K CmdExec_Proc()�ł����Ȃ��Ă��邽�߂����ł�NOP

                'V6.0.0.0�C                Me.CrosLineX.Visible = False                            ' �␳�N���X���C����\��
                'V6.0.0.0�C                Me.CrosLineY.Visible = False
                VideoLibrary1.SetCorrCrossVisible(False)                'V6.0.0.0�C
                Exit Function

STP_ERR:
                ' �e�B�[�`���O�n�b�w�G���[��
                'strMSG = "i-TKY.Teaching() TEACH.OCX ERROR = " + strDAT + r.ToString
                'MsgBox(strMSG)
                'Teaching = cERR_TRAP                                    ' Return�l = ��O�G���[
                Exit Function
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Teaching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Teaching = cERR_TRAP                                        ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region ""
    'V5.0.0.6�K �O���J�����L�����u���[�V�����ɃN���X���C���␳����������B��
    Private Sub UpdateByCrossLineToExt(ByRef X As Double, ByRef Y As Double)
        Try
            If gSysPrm.stCRL.giDspFlg = 0 Then      ' �����␳����
                Exit Sub
            End If
            Dim dX As Double, dY As Double, dPixX As Double, dPixY As Double
            dX = X + typPlateInfo.dblBpOffSetXDir + (82.0 - typPlateInfo.dblBlockSizeXDir) / 2.0
            dY = Y + typPlateInfo.dblBpOffSetYDir + (82.0 - typPlateInfo.dblBlockSizeYDir) / 2.0
            ObjCrossLine.GetCorrectPixelXY(dX, dY, dPixX, dPixY)
            X = X - (dPixX * gSysPrm.stGRV.gfPixelX / 1000.0)
            Y = Y - (dPixY * gSysPrm.stGRV.gfPixelY / 1000.0)
            ' INTRIM�ɐݒ肳��Ă��錻��̃L�����u���[�V�����␮�l���擾
            'r = BP_GET_CALIBDATA(dblCurGain(0), dblCurGain(1), dblCurCalibOff(0), dblCurCalibOff(1))

        Catch ex As Exception
            MsgBox("i-UpdateByCrossLineToExt() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub UpdateByCrossLineToIN(ByRef X As Double, ByRef Y As Double)
        Try
            Dim dX As Double, dY As Double, dPixX As Double, dPixY As Double
            If gSysPrm.stCRL.giDspFlg = 0 Then      ' �����␳����
                Exit Sub
            End If
            dX = X + typPlateInfo.dblBpOffSetXDir + (82.0 - typPlateInfo.dblBlockSizeXDir) / 2.0
            dY = Y + typPlateInfo.dblBpOffSetYDir + (82.0 - typPlateInfo.dblBlockSizeYDir) / 2.0
            ObjCrossLine.GetCorrectPixelXY(dX, dY, dPixX, dPixY)
            X = X + (dPixX * gSysPrm.stGRV.gfPixelX / 1000.0)
            Y = Y + (dPixY * gSysPrm.stGRV.gfPixelY / 1000.0)
            ' INTRIM�ɐݒ肳��Ă��錻��̃L�����u���[�V�����␮�l���擾
            'r = BP_GET_CALIBDATA(dblCurGain(0), dblCurGain(1), dblCurCalibOff(0), dblCurCalibOff(1))
        Catch ex As Exception
            MsgBox("i-TKY.UpdateByCrossLineToIN() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    'V5.0.0.6�K ��
#End Region

#Region "�O��R1�e�B�[�`���O��1�J�b�g�ڂ̃p�����[�^��S��R�ɓW�J����"
    '''=========================================================================
    ''' <summary>�O��R1�e�B�[�`���O��1�J�b�g�ڂ̃p�����[�^��S��R�ɓW�J����</summary>
    '''=========================================================================
    Private Sub SetR1Data2AllResistor(ByRef startPosTbl(,) As Double)

        Dim strMsg As String
        Dim deltaX(), deltaY() As Double
        Dim maxCutCnt As Integer
        Dim cutCnt As Integer
        Dim rno As Integer
        Dim dStartPointX As Double, dStartPointY As Double              ' V6.1.4.0�N
        Dim iCircuitCnt As Integer                                      'V6.1.4.9�@�@typResistorInfoArray(1)��typResistorInfoArray(iCircuitCnt)�ɒu������
        Dim iMaxCircuitCnt As Integer                                   'V6.1.4.9�@�@�T�[�L�b�g����R��
        Dim iTotalCutCnt As Integer = 0                                 'V6.1.4.9�@�@�ώZ�J�b�g���iDllTeach���̃e�[�u���z��)
        Dim iNgMarkNo As Integer = 0                                    'V6.1.4.9�@  �m�f�}�[�N�́A��P��R��P�J�b�g�ōX�V����B

        Try
            'V6.1.4.9�@��
            If gTkyKnd = KND_CHIP Then
                iMaxCircuitCnt = 1
            Else
                iMaxCircuitCnt = typPlateInfo.intResistCntInGroup
            End If
            For iCircuitCnt = 1 To iMaxCircuitCnt
                'V6.1.4.9�@��

                '���݂̐擪�f�[�^�ƕύX�l���獷�������Z���A�S��R�ɓW�J����B
                maxCutCnt = typResistorInfoArray(iCircuitCnt).intCutCount
                ReDim Preserve deltaX(maxCutCnt)                            ' �����i�[��X
                ReDim Preserve deltaY(maxCutCnt)                            ' �����i�[��Y

                ' �擪��R�̃J�b�g�������������߂�
                For cutCnt = 1 To maxCutCnt                                 ' �擪��R�̃J�b�g�����J��Ԃ�  
                    iTotalCutCnt = iTotalCutCnt + 1                         ' �ȉ�startPosTbl�e�[�u����cutCnt��iTotalCutCnt�֕ύX 'V6.1.4.9�@
                    '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                    'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' �e�B�[�`���O�|�C���g�g�p�̎�
                    If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                        ' dStartPointX,Y�Ƀe�B�[�`���O�|�C���gX,Y��ޔ�����
                        dStartPointX = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointX
                        dStartPointY = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointY
                    Else
                        ' dStartPointX,Y�ɃX�^�[�g�|�C���gX,Y��ޔ�����
                        dStartPointX = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX
                        dStartPointY = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY
                    End If
                    '----- V6.1.4.0�N�� -----
                    ' ----- V1.14.0.0�C�� -----
                    'deltaX(cutCnt) = startPosTbl(1, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                    'deltaY(cutCnt) = startPosTbl(2, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                    UpdateByCrossLineToIN(startPosTbl(1, iTotalCutCnt), startPosTbl(2, iTotalCutCnt)) 'V5.0.0.6�K

                    ' �����i�[��deltaX,Y�ɃX�^�[�g�|�C���gX,Y(���̓e�B�[�`���O�|�C���gX,Y)�̍�����ݒ肷��
                    Select Case gSysPrm.stDEV.giBpDirXy
                    '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                        Case 0 ' x��, y��
                            deltaX(cutCnt) = startPosTbl(1, iTotalCutCnt) - dStartPointX
                            deltaY(cutCnt) = startPosTbl(2, iTotalCutCnt) - dStartPointY
                        Case 1 ' x��, y��(X���])
                            deltaX(cutCnt) = (startPosTbl(1, iTotalCutCnt) * -1) - dStartPointX
                            deltaY(cutCnt) = startPosTbl(2, iTotalCutCnt) - dStartPointY
                        Case 2 ' x��, y��(Y���])
                            deltaX(cutCnt) = startPosTbl(1, iTotalCutCnt) - dStartPointX
                            deltaY(cutCnt) = (startPosTbl(2, iTotalCutCnt) * -1) - dStartPointY
                        Case 3 ' x��, y��(XY���])
                            deltaX(cutCnt) = (startPosTbl(1, iTotalCutCnt) * -1) - dStartPointX
                            deltaY(cutCnt) = (startPosTbl(2, iTotalCutCnt) * -1) - dStartPointY

                            'Case 0 ' x��, y��
                            '    deltaX(cutCnt) = startPosTbl(1, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = startPosTbl(2, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            'Case 1 ' x��, y��(X���])
                            '    deltaX(cutCnt) = (startPosTbl(1, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = startPosTbl(2, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            'Case 2 ' x��, y��(Y���])
                            '    deltaX(cutCnt) = startPosTbl(1, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = (startPosTbl(2, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            'Case 3 ' x��, y��(XY���])
                            '    deltaX(cutCnt) = (startPosTbl(1, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = (startPosTbl(2, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            '----- V6.1.4.0�N�� -----
                    End Select
                    '----- V1.14.0.0�C�� -----

                    ' �擪�̃f�[�^���X�V
                    ' ----- V1.14.0.0�C�� -----
                    'typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt)
                    'typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt)

                    ' dStartPointX,Y = �e�B�[�`���O��ʂ̃e�B�[�`���O�ʒuX,Y
                    Select Case gSysPrm.stDEV.giBpDirXy
                    '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                        Case 0 ' x��, y��
                            dStartPointX = startPosTbl(1, iTotalCutCnt)
                            dStartPointY = startPosTbl(2, iTotalCutCnt)
                        Case 1 ' x��, y��(X���])
                            dStartPointX = startPosTbl(1, iTotalCutCnt) * -1
                            dStartPointY = startPosTbl(2, iTotalCutCnt)
                        Case 2 ' x��, y��(Y���])
                            dStartPointX = startPosTbl(1, iTotalCutCnt)
                            dStartPointY = startPosTbl(2, iTotalCutCnt) * -1
                        Case 3 ' x��, y��(XY���])
                            dStartPointX = startPosTbl(1, iTotalCutCnt) * -1
                            dStartPointY = startPosTbl(2, iTotalCutCnt) * -1

                            'Case 0 ' x��, y��
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt)
                            'Case 1 ' x��, y��(X���])
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt) * -1
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt)
                            'Case 2 ' x��, y��(Y���])
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt) * -1
                            'Case 3 ' x��, y��(XY���])
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt) * -1
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt) * -1
                            '----- V6.1.4.0�N�� -----
                    End Select
                    '----- V1.14.0.0�C�� -----
                    '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                    'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' �e�B�[�`���O�|�C���g�g�p�̎�
                    If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                        ' �e�B�[�`���O�|�C���gX,Y�X�V
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointX = dStartPointX
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointY = dStartPointY
                        ' �X�^�[�g�|�C���gX,Y�����������Z���čX�V
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX + deltaX(cutCnt)
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY + deltaY(cutCnt)
                    Else
                        '�X�^�[�g�|�C���gX,Y�X�V
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX = dStartPointX
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY = dStartPointY
                    End If
                    '----- V6.1.4.0�N�� -----
                Next

                ' ��R����2�Ԗڈȍ~�̃J�b�g�ɑ΂��āA���������Z���Ă����B
                'V6.1.4.9�@                For rno = 2 To gRegistorCnt
                For rno = iCircuitCnt + iMaxCircuitCnt To gRegistorCnt Step iMaxCircuitCnt           'V6.1.4.9�@
                    If typResistorInfoArray(rno).intResNo > 1000 Then
                        iNgMarkNo = rno
                        Exit For
                    End If
                    For cutCnt = 1 To typResistorInfoArray(rno).intCutCount
                        ' �J�b�g�����擪��葽���ꍇ�͔�����
                        If (cutCnt > UBound(deltaX)) Then
                            Exit For
                        End If

                        ' X���W
                        typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointX =
                                    typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointX + deltaX(cutCnt)
                        ' Y���W
                        typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointY =
                                    typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointY + deltaY(cutCnt)

                        '----- V6.1.4.0�N��(KOA EW�aSL432RD�Ή�) -----
                        ' �e�B�[�`���O�|�C���g�����������Z���čX�V
                        'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' �e�B�[�`���O�|�C���g�g�p�̎�
                        If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                            typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointX = typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointX + deltaX(cutCnt)
                            typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointY = typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointY + deltaY(cutCnt)
                        End If
                        '----- V6.1.4.0�N�� -----
                    Next
                Next
                'V6.1.4.9�@��NG�}�[�N�ԍ��L��̎��́A�}�[�L���O�ʒu�ɑ�P��R��P�J�b�g�̏��𔽉f����B
                If iNgMarkNo > 0 And iCircuitCnt = 1 Then
                    '�}�[�L���O�́A�X�V���Ȃ��B
                    'For rno = iNgMarkNo To gRegistorCnt
                    '    For cutCnt = 1 To typResistorInfoArray(rno).intCutCount
                    '        ' X���W
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointX =
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointX + deltaX(1)
                    '        ' Y���W
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointY =
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointY + deltaY(1)

                    '        ' �e�B�[�`���O�|�C���g�����������Z���čX�V
                    '        'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' �e�B�[�`���O�|�C���g�g�p�̎�
                    '        If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' �e�B�[�`���O�|�C���g/�X�^�[�g�|�C���g�̗����ύX?
                    '            typResistorInfoArray(rno).ArrCut(1).dblTeachPointX = typResistorInfoArray(rno).ArrCut(1).dblTeachPointX + deltaX(1)
                    '            typResistorInfoArray(rno).ArrCut(1).dblTeachPointY = typResistorInfoArray(rno).ArrCut(1).dblTeachPointY + deltaY(1)
                    '        End If
                    '    Next
                    'Next
                End If
                'V6.1.4.9�@��
            Next                                                        'V6.1.4.9�@ iCircuitCnt
            Exit Sub

        Catch ex As Exception
            strMsg = "i-TKY.SetR1Data2AllResistor() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub
#End Region

#Region "�O���J�����e�B�[�`���O�p�p�����[�^��ݒ肷��"
    '''=========================================================================
    ''' <summary>�O���J�����e�B�[�`���O�p�p�����[�^��ݒ肷��</summary>
    ''' <param name="iAppMode">(INP)�A�v�����[�h(giAppMode�Q��)
    '''                            �@APP_MODE_TEACH          = �e�B�[�`���O
    '''                            �@APP_MODEi_EXCAM_R1TEACH = �O���J����R1�e�B�[�`���O
    '''                            �@APP_MODE_EXCAM_TEACH    = �O���J�����e�B�[�`���O</param> 
    ''' <param name="pltInfo"> (INP)�v���[�g�f�[�^</param>
    ''' <param name="stPLT">   (INP)�v���[�g���</param>
    ''' <param name="stSTP">�@ (INP)�X�e�b�v���</param>
    ''' <param name="stGRP">�@ (INP)�O���[�v���</param>
    ''' <param name="stTY2">�@ (INP)�s�x�Q���</param>
    '''=========================================================================
    Private Sub SetExCamTeachParam(ByVal iAppMode As Integer, ByRef pltInfo As PlateInfo, ByRef stPLT As TEACH_PLATE_INFO, ByRef stSTP() As TEACH_STEP_INFO,
                                   ByRef stGRP() As TEACH_GROP_INFO, ByRef stTY2() As TEACH_TY2_INFO, Optional ByVal iTkyKnd As Short = 1)  'V6.1.4.9�@ �A�v���P�[�V�������iTkyKnd�ǉ�

        Dim IDX As Integer
        Dim strMSG As String                                                    ' ���b�Z�[�W�ҏW��

        Try
            With pltInfo
                stPLT.iTkyKnd = iTkyKnd                                         '�A�v���P�[�V������� V6.1.4.9�@
                ' �v���[�g���
                stPLT.intResistDir = .intResistDir                              ' ��R���ѕ���
                'V6.1.4.9�@                stPLT.intResistCntInGroup = .intResistCntInBlock                ' �u���b�N����R��
                stPLT.intResistCntInGroup = .intResistCntInGroup                '  1�O���[�v��(1�T�[�L�b�g��)��R��
                stPLT.intBlockCntXDir = .intBlockCntXDir                        ' �u���b�N���w
                stPLT.intBlockCntYDir = .intBlockCntYDir                        ' �u���b�N���x
                stPLT.intGroupCntInBlockXBp = .intGroupCntInBlockXBp            ' �u���b�N��BP�O���[�v��(X�����j
                stPLT.intGroupCntInBlockYStage = .intGroupCntInBlockYStage      ' �u���b�N��Stage�O���[�v��(Y�����j
                stPLT.dblBlockSizeXDir = .dblBlockSizeXDir                      ' �u���b�N�T�C�Y�w
                stPLT.dblBlockSizeYDir = .dblBlockSizeYDir                      ' �u���b�N�T�C�Y�x
                stPLT.dblTableOffsetXDir = .dblTableOffsetXDir                  ' �e�[�u���ʒu�I�t�Z�b�gX
                stPLT.dblTableOffsetYDir = .dblTableOffsetYDir                  ' �e�[�u���ʒu�I�t�Z�b�gY
                stPLT.dblBpOffSetXDir = .dblBpOffSetXDir                        ' �a�o�I�t�Z�b�gX
                stPLT.dblBpOffSetYDir = .dblBpOffSetYDir                        ' �a�o�I�t�Z�b�gY
                stPLT.dblChipSizeXDir = .dblChipSizeXDir                        ' �`�b�v�T�C�YX
                stPLT.dblChipSizeYDir = .dblChipSizeYDir                        ' �`�b�v�T�C�YY
                stPLT.dblBpGrpItv = .dblBpGrpItv                                ' BP�O���[�v�Ԋu
                stPLT.dblblStgGrpItv = .dblStgGrpItvY                           ' Stage�O���[�v�Ԋu
                stPLT.dblRev1 = 0.0#                                            ' �\��
                stPLT.dblRev2 = 0.0#                                            ' �\��
                stPLT.CorrectTrimPosX = gfCorrectPosX                           ' �g�����|�W�V�����␳�lX
                stPLT.CorrectTrimPosX = gfCorrectPosY                           ' �g�����|�W�V�����␳�lY

                ' �X�e�b�v���
                For IDX = 1 To MaxStep
                    stSTP(IDX).intSP1 = typStepInfoArray(IDX).intSP1            ' �X�e�b�v�ԍ�
                    stSTP(IDX).intSP2 = typStepInfoArray(IDX).intSP2            ' �u���b�N��
                    stSTP(IDX).DblSP3 = typStepInfoArray(IDX).dblSP3            ' �X�e�b�v�ԃC���^�[�o��
                Next IDX

                ' �O���[�v���
                For IDX = 1 To MaxGrp
                    stGRP(IDX).intGP1 = typGrpInfoArray(IDX).intGP1             ' �O���[�v�ԍ�
                    stGRP(IDX).intGP2 = typGrpInfoArray(IDX).intGP2             ' ��R��
                    stGRP(IDX).dblGP3 = typGrpInfoArray(IDX).dblGP3             ' �O���[�v�ԃC���^�[�o��
                    stGRP(IDX).dblStgPosX = typGrpInfoArray(IDX).dblStgPosX     ' �X�e�[�WX�|�W�V����
                    stGRP(IDX).dblStgPosY = typGrpInfoArray(IDX).dblStgPosY     ' �X�e�[�WY�|�W�V����
                Next IDX

                ' �s�x�Q���
                For IDX = 1 To MaxTy2
                    stTY2(IDX).intTy21 = typTy2InfoArray(IDX).intTy21           ' �u���b�N�ԍ�
                    stTY2(IDX).dblTy22 = typTy2InfoArray(IDX).dblTy22           ' �e�u���b�N�Ԃ̃X�e�b�v����
                Next IDX
            End With

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetExCamTeachParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�J�b�g�g���[�X�̂��߂̃J�b�g������ݒ肷��"
    '''=========================================================================
    '''<summary>�J�b�g�g���[�X�̂��߂̃J�b�g������ݒ肷��</summary>
    '''<param name="rn">   (INP)��R�ԍ�</param>
    '''<param name="cn">   (INP)�J�b�g�ԍ�</param>
    '''<param name="dirL1">(I/O)�J�b�g����1</param>
    '''<param name="dirL2">(I/O)�J�b�g����2</param>
    '''<remarks>�EST���/IDX��Ď�(dirL2�͕Ԃ��Ȃ�)
    '''           ����        = �J�b�g�p�x(0��, 90��, 180��, 270��)
    '''           �o��(dirL1) = �J�b�g����(3:+X(0��), 2:+Y(90��), 1:-X(180��), 4:-Y(270��))
    '''         �EL ���/ HOOK ��Ď�(dirL2�͕Ԃ��Ȃ�)
    '''           ����        = �J�b�g�p�x(0��, 90��, 180��, 270��)
    '''                       = L��ݕ���(1:CW, 2:CCW) 
    '''           �o��(dirL1) = �J�b�g���� 3:+X+Y(����), 4:-Y+X(����), 1:-X-Y(����) ,2:+Y-X(����),
    '''                                    7:+X-Y(����), 8:-Y-X(����), 5:-X+Y(����) ,6:+Y+X(����))
    '''         �E�X�L�����J�b�g��
    '''           ����        = �J�b�g�p�x(0��, 90��, 180��, 270��)
    '''                       = �ï�ߕ���(0:0��, 1:90��, 2:180��, 3:270) 
    '''           �o��(dirL1) = �J�b�g����(1:-X, 2:+X, 3:-Y, 4:+Y)
    '''           �o��(dirL2) = �ï�ߕ��� (1:+X, 2:-X, 3:+Y, 4:-Y)
    ''' </remarks>
    '''=========================================================================
    Private Sub Cnv_Cut_Dir(ByRef rn As Short, ByRef cn As Short, ByRef dirL1 As Short, ByRef dirL2 As Short)

        Dim strMSG As String                                ' ���b�Z�[�W�ҏW��

        Try
            ' ST���/IDX��Ď�
            ' �����}�[�L���O��
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_ST) Or
                (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_M) Or
                (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_ES) Or
                (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_ES2) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_IX) Then
                Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
                    Case 0
                        dirL1 = 3                                   ' 0��(+X��) 
                    Case 90
                        dirL1 = 2                                   ' 90��(+Y��)
                    Case 180
                        dirL1 = 1                                   ' 180��(-X��) 
                    Case Else
                        dirL1 = 4                                   ' 270��(+Y��)
                End Select
            End If

            '' �����}�[�L���O��
            'If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_M) Then
            '    Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
            '        Case 0
            '            dirL1 =                                    ' 0��(+X��) 
            '        Case 90
            '            dirL1 = 4                                   ' 90��(+Y��)
            '        Case 180
            '            dirL1 = 1                                   ' 180��(-X��) 
            '        Case Else
            '            dirL1 = 3                                   ' 270��(+Y��)
            '    End Select
            'End If

            ' L���/HOOK���/U��Ď�
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_L) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_HK) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_U) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0�@

                Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
                    Case 0                                          ' �J�b�g����(1:+X(0��)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 7                               ' +X-Y(����)
                        Else
                            dirL1 = 3                               ' +X+Y(����)
                        End If
                    Case 90                                         ' �J�b�g����(2:+Y(90��)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 6                               ' +Y+X(����))
                        Else
                            dirL1 = 2                               ' +Y-X(����)
                        End If
                    Case 180                                        ' �J�b�g����(3:-X(180��)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 5                               ' -X+Y(����)
                        Else
                            dirL1 = 1                               ' -X-Y(����) 
                        End If
                    Case Else                                       ' �J�b�g����(4:-Y(270��)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 8                               ' -Y-X(����)
                        Else
                            dirL1 = 4                               ' -Y+X(����)
                        End If
                End Select
            End If

            ' �X�L�����J�b�g��
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_SC) Then
                ' �J�b�g���� 
                Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
                    Case 0                                          ' +X
                        dirL1 = 2                                   ' 0��(+X��) 
                    Case 90                                         ' +Y
                        dirL1 = 4                                   ' 90��(+Y��)
                    Case 180                                        ' -X
                        dirL1 = 1                                   ' 180��(-X��) 
                    Case Else                                       ' -Y
                        dirL1 = 3                                   ' 270��(-Y��)

                End Select

                ' �X�e�b�v���� 
                Select Case (typResistorInfoArray(rn).ArrCut(cn).intStepDir)
                    Case 0                                          ' +X
                        dirL2 = 1                                   '   0��(+X��) 
                    Case 1                                          ' +Y
                        dirL2 = 3                                   '  90��(+Y��)
                    Case 2                                          ' -X
                        dirL2 = 2                                   ' 180��(-X��) 
                    Case Else                                       ' -Y
                        dirL2 = 4                                   ' 270��(-Y��)
                End Select
            End If

            '----- V6.1.1.0�E�� -----
            ' �C���f�b�N�X�v�J�b�g���̓X�e�b�v����(0:0��, 1:90��, 2:180��, 3:270)��Ԃ�
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_IX) And (typResistorInfoArray(rn).ArrCut(cn).intCutCnt > 0) Then
                ' ��R�[�i�[
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0 ' �E��(x��, y��)
                        '    ' �J�b�g���� = 0����180���̏ꍇ
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 3                                               ' �ï�ߕ���(270��) 
                            Else
                                dirL2 = 1                                               ' �ï�ߕ���(90��) 
                            End If
                            ' �J�b�g���� = 90����270���̏ꍇ
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 2                                               ' �ï�ߕ���(180��) 
                            Else
                                dirL2 = 0                                               ' �ï�ߕ���(0��) 
                            End If
                        End If

                    Case 1 ' ����( x��, y��)
                        '    ' �J�b�g���� = 0����180���̏ꍇ
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 3                                               ' �ï�ߕ���(270��) 
                            Else
                                dirL2 = 1                                               ' �ï�ߕ���(90��) 
                            End If
                            ' �J�b�g���� = 90����270���̏ꍇ
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 0                                               ' �ï�ߕ���(0��) 
                            Else
                                dirL2 = 2                                               ' �ï�ߕ���(180��) 
                            End If
                        End If

                    Case 2 ' �E��(x��, y��)
                        '    ' �J�b�g���� = 0����180���̏ꍇ
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 1                                               ' �ï�ߕ���(90��) 
                            Else
                                dirL2 = 3                                               ' �ï�ߕ���(270��) 
                            End If
                            ' �J�b�g���� = 90����270���̏ꍇ
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 2                                               ' �ï�ߕ���(180��) 
                            Else
                                dirL2 = 0                                               ' �ï�ߕ���(0��) 
                            End If
                        End If

                    Case 3 ' ����(x��, y��)
                        '    ' �J�b�g���� = 0����180���̏ꍇ
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 1                                               ' �ï�ߕ���(90��) 
                            Else
                                dirL2 = 3                                               ' �ï�ߕ���(270��) 
                            End If
                            ' �J�b�g���� = 90����270���̏ꍇ
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' �X�e�b�v��+���� ?
                                dirL2 = 2                                               ' �ï�ߕ���(180��) 
                            Else
                                dirL2 = 0                                               ' �ï�ߕ���(0��) 
                            End If
                        End If
                End Select
            End If
            '----- V6.1.1.0�E�� -----

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Cnv_Cut_Dir() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�e�J�b�g�g���[�X�̂��߃Z�b�g�A�b�v����"
    '''=========================================================================
    '''<summary>�e�J�b�g�g���[�X�̂��߃Z�b�g�A�b�v����</summary>
    '''<param name="rn">         (INP)��R�ԍ�</param>
    '''<param name="cn">         (INP)�J�b�g�ԍ�</param>
    '''<param name="dirL1">      (INP)�J�b�g����1</param>
    '''<param name="dirL2">      (INP)�J�b�g����2</param>
    '''<param name="RnCnTbl">    (INP)��R�ԍ�,�J�b�g�ԍ��e�[�u��</param> 
    '''<param name="StartPosTbl">(INP)�J�b�g�J�n�ʒu�e�[�u��</param> 
    '''<param name="idx">        (INP)�e�[�u���C���f�b�N�X</param>   
    '''=========================================================================
    Private Sub Sub_Cut_Setup(ByRef rn As Short, ByRef cn As Short, ByRef dirL1 As Short, ByRef dirL2 As Short,
                              ByRef RnCnTbl(,) As Short, ByRef StartPosTbl(,) As Double, ByRef idx As Short)

        Dim strMSG As String                                ' ���b�Z�[�W�ҏW��
        Dim iWK As Short

        Try
            With typResistorInfoArray(rn).ArrCut(cn)
                ' �J�b�g�`�󖈂̃Z�b�g�A�b�v�������s��
                Select Case (typResistorInfoArray(rn).ArrCut(cn).strCutType)
                    '   ' ST�J�b�g(�ʏ�/���^�[��/���g���[�X) �����g�p(�΂�ST�J�b�g���g�p����)
                    Case CNS_CUTP_ST, CNS_CUTP_STr, CNS_CUTP_STt
                        ' �X�g���[�g�J�b�g(�J�b�g���� 1:-X��, 2:+Y��, 3:+X�� ,4:-Y��)
                        ' SetupCutST(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1)
                        Call Teaching1.SetupCutST(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength)

                        ' �΂�ST�J�b�g(�ʏ�/���^�[��/���g���[�X) 
                    Case CNS_CUTP_NST, CNS_CUTP_NSTr, CNS_CUTP_NSTt, CNS_CUTP_ES, CNS_CUTP_ES2         'V1.14.0.0�@
                        ' SetupCutST(��R�ԍ�, ��Ĕԍ�, ���ĈʒuX, ���ĈʒuY, ��Ē�1, �p�x)
                        Call Teaching1.SetupCutSST(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intCutAngle, typResistorInfoArray(rn).ArrCut(cn).strCutType)    'V3.0.0.0�B �Ō�̈����ɃJ�b�g��ʂ�ǉ�

                        '�k�J�b�g(�ʏ�/���^�[��/���g���[�X) �����g�p(�΂�L�J�b�g���g�p����)
                    Case CNS_CUTP_L, CNS_CUTP_Lr, CNS_CUTP_Lt
                        '�k�J�b�g(�J�b�g���� 1:-X-Y, 2:+Y-X, 3:+X+Y ,4:-Y+X, 5:-X+Y, 6:+Y+X, 7:+X-Y ,8:-Y-X)
                        ' SetupCutL(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1, R1, ����߲��, ��Ē�2)
                        Call Teaching1.SetupCutL(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblLTurnPoint, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL)

                        ' �΂߂k�J�b�g(�ʏ�/���^�[��/���g���[�X) 
                    Case CNS_CUTP_NL, CNS_CUTP_NLr, CNS_CUTP_NLt
                        ' SetupCutSL(��R�ԍ�, ��Ĕԍ�, ���ĈʒuX, ���ĈʒuY, ��Ē�1, R1(���g�p), ��Ē�2, �p�x, ��ݕ���)
                        Call Teaching1.SetupCutSL(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, typResistorInfoArray(rn).ArrCut(cn).intCutAngle, typResistorInfoArray(rn).ArrCut(cn).intLTurnDir, typResistorInfoArray(rn).ArrCut(cn).strCutType)    'V3.0.0.0�B �Ō�̈����ɃJ�b�g��ʂ�ǉ�)

                    Case CNS_CUTP_HK    ' �t�b�N�J�b�g(�J�b�g���� 1:-X-Y, 2:+Y-X, 3:+X+Y ,4:-Y+X, 5:-X+Y, 6:+Y+X, 7:+X-Y ,8:-Y-X)
                        ' SetupCutHK(��R�ԍ�,��Ĕԍ�,��ĕ���,���ĈʒuX, ���ĈʒuY,��Ē�1,R1���a(���g�p), ����߲��(���g�p), ��Ē�2, r2(-1�Œ�), ̯���Ĉړ���)
                        '' ''Call Teaching1.SetupCutHK(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), _
                        '' ''                StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblR1, 100.0#, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblR2, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthHook)
                        Call Teaching1.SetupCutHK(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx),
                                        StartPosTbl(2, idx), .dblMaxCutLength, .dblR1, 100.0#,
                                        .dblMaxCutLengthL, .dblR2, .dblMaxCutLengthHook)

                    Case CNS_CUTP_IX    ' �C���f�b�N�X�J�b�g(�J�b�g���� 1:-X��, 2:+Y��, 3:+X�� ,4:-Y��)
                        '----- V6.1.1.0�E�� -----
                        '  �C���f�b�N�X�v�J�b�g�Ή�
                        '' SetupCutIX(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1, IDX��, ����Ӱ��(���g�p))
                        'Call Teaching1.SetupCutIX(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intIndexCnt, typResistorInfoArray(rn).ArrCut(cn).intMeasMode)

                        If (.intCutCnt > 0) Then                                           ' �V�t�g��(1�`n)
                            'SetupCutIX(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1, IDX��, ����Ӱ��(�V�t�g��), �ï�ߕ���, �{��)
                            Call Teaching1.SetupCutIX(RnCnTbl(idx, 0), RnCnTbl(idx, 1), .intCutAngle, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intIndexCnt, .intCutCnt, dirL2, System.Math.Abs(.dblPitch))
                        Else
                            'SetupCutIX(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1, IDX��, ����Ӱ��(���g�p), �ï�ߕ���, �{��)
                            Call Teaching1.SetupCutIX(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intIndexCnt, .intCutCnt, 0, 0.0)
                        End If
                        '----- V6.1.1.0�E�� -----

                    Case CNS_CUTP_SC    ' �X�L�����J�b�g(��ĕ��� 1:-X, 2:+X, 3:-Y, 4:+Y)/�ï�ߕ���(1:+X, 2:-X, 3:+Y, 4:-Y)
                        ' SetupCutSC(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1, �߯�, �ï�ߕ���, �{��)
                        Call Teaching1.SetupCutSC(RnCnTbl(idx, 0), RnCnTbl(idx, 1), .intCutAngle,
                                                StartPosTbl(1, idx), StartPosTbl(2, idx), .dblMaxCutLength,
                                                .dblPitch, .intStepDir, .intCutCnt)
                        'Call Teaching1.SetupCutSC(RnCnTbl(idx, 0), RnCnTbl(idx, 1), typResistorInfoArray(rn).ArrCut(cn).intCutAngle, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).dblPitch, typResistorInfoArray(rn).ArrCut(cn).intStepDir, typResistorInfoArray(rn).ArrCut(cn).intCutCnt)
                        'Call Teaching1.SetupCutSC(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).dblPitch, dirL2, typResistorInfoArray(rn).ArrCut(cn).intCutCnt)

                    Case CNS_CUTP_U, CNS_CUTP_Ut     ' U�J�b�g(�J�b�g���� 1:-X-Y, 2:+X+Y, 3:-Y+X ,4:+Y-X) V1.22.0.0�@
                        ' SetupCutU(��R�ԍ�, ��Ĕԍ�, ��ĕ���, ���ĈʒuX, ���ĈʒuY, ��Ē�1, R1���a, ��Ē�2, L��݌�ړ�����(���g�p))
                        Call Teaching1.SetupCutU(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, 0)
                        'Call Teaching1.SetupCutU(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, 0)

                    Case CNS_CUTP_NOP   ' Z(NO CUT)
                        ' SetupCutZ(��R�ԍ�, ��Ĕԍ�, ���ĈʒuX, ���ĈʒuY)
                        Call Teaching1.SetupCutZ(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx))

                    Case CNS_CUTP_M     ' �����}�[�L���O 
                        ' SetupCutM(��R�ԍ�, ��Ĕԍ�, ��������(1:-X, 2:+X, 3:-Y, 4:+Y), ���ĈʒuX, ���ĈʒuY, �{��, ������)
                        iWK = typResistorInfoArray(rn).ArrCut(cn).dblZoom
                        '----- V6.0.3.0_51�� -----
                        'Call Teaching1.SetupCutM(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), iWK, Len(typResistorInfoArray(rn).ArrCut(cn).strChar))
                        'Call Teaching1.SetupCutMStr(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), iWK, Len(typResistorInfoArray(rn).ArrCut(cn).strChar), typResistorInfoArray(rn).ArrCut(cn).strChar)
                        Call Teaching1.SetupCutMStr(RnCnTbl(idx, 0), RnCnTbl(idx, 1), typResistorInfoArray(rn).ArrCut(cn).intCutAngle, StartPosTbl(1, idx), StartPosTbl(2, idx), iWK, Len(typResistorInfoArray(rn).ArrCut(cn).strChar), typResistorInfoArray(rn).ArrCut(cn).strChar)
                        '----- V6.0.3.0_51�� -----

                    Case CNS_CUTP_C     ' C�J�b�g(�~�ʃJ�b�g)
                        ' SetupCutCir(��R�ԍ�, ��Ĕԍ�, ���ĈʒuX, ���ĈʒuY,�~�ʕ��̔��a,�~�ʂ̊p�x, �n�߂̈ړ��p�x)
                        ' Call Teaching1.SetupCutCir(RnCnTbl(t, 0), RnCnTbl(t, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), 1, 45, -270)
                        Call Teaching1.SetupCutCir(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblR1, typResistorInfoArray(rn).ArrCut(cn).dblR2, typResistorInfoArray(rn).ArrCut(cn).intCutAngle)

                    Case Else
                        Call System1.TrmMsgBox(gSysPrm, "Cut Type Error Type = " & typResistorInfoArray(rn).ArrCut(cn).strCutType, MsgBoxStyle.OkOnly, My.Application.Info.Title)
                End Select
            End With
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Sub_Cut_Setup() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�ƕ␳��XYð�����шʒu�ړ���Z�ҋ@�ʒu�ړ�����"
    '''=========================================================================
    ''' <summary>�ƕ␳��XYð�����шʒu�ړ���Z�ҋ@�ʒu�ړ�����</summary>
    ''' <param name="pltInfo">(INP)�v���[�g�f�[�^</param>
    ''' <param name="TblOfsX">(INP)�e�[�u���I�t�Z�b�gX</param>
    ''' <param name="TblOfsY">(INP)�e�[�u���I�t�Z�b�gY</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''         cFRS_ERR_RST  = Cancel(RESET��)
    '''         cFRS_ERR_PTN  = �p�^�[���F���G���[
    '''         ��L�ȊO      = ����~�����̑��G���[
    ''' </returns>
    '''=========================================================================
    Private Function Move_Trimposition(ByVal pltInfo As PlateInfo, ByVal TblOfsX As Double, ByVal TblOfsY As Double) As Short

        Dim strMSG As String = ""
        Dim r As Short
        Dim StgOfsX As Double = 0.0                                     ' XY�e�[�u���I�t�Z�b�gX 'V1.13.0.0�B
        Dim StgOfsY As Double = 0.0                                     ' XY�e�[�u���I�t�Z�b�gY 'V1.13.0.0�B

        Try
            With pltInfo
                ' ��������
                Move_Trimposition = cFRS_NORMAL                         ' Return�l = ����
                'V4.10.0.0�B��
                If gbCorrectDone Then                                   ' �ꊇ�e�B�[�`���O���́A��x�␳�����{������͍s��Ȃ��B
                    GoTo STP_START
                End If
                'V4.10.0.0�B��
                'V1.19.0.0-27�@��
                ' �ƕ␳�蓮�ŁA�P��݂̂̏ꍇ�̓t���O���`�F�b�N���Ď��s�ς݂Ȃ牽�����Ȃ�
                If (.intReviseMode = 1) And (.intManualReviseType = 1) Then
                    If (gManualThetaCorrection = True) Then
                        gfCorrectPosX = 0.0                             ' �␳�l������ 
                        gfCorrectPosY = 0.0
                    End If
                Else
                    gfCorrectPosX = 0.0                                 ' �␳�l������ 
                    gfCorrectPosY = 0.0
                End If
                'V1.19.0.0-27�@��
                '----- V1.15.0.0�B�� -----
                ' Z��ҋ@�ʒu�ֈړ�����
                r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Move_Trimposition = r                               ' Return�l�ݒ�
                    Exit Function
                End If
                '----- V1.15.0.0�B�� -----

                '--------------------------------------------------------------------------
                '  �ƕ␳����
                '--------------------------------------------------------------------------
                ' �Ɩ����Ȃ�ƕ␳���Ȃ���XYð��ق���шʒu�Ɉړ�
                If (gSysPrm.stDEV.giTheta = 0) Then GoTo STP_START
                ' �ƗL��Łu�␳���[�h=1(�蓮)�v�ŕ␳���@=0(�␳�Ȃ�)�̏ꍇ�̓Ɖ�]���s��
                If ((.intReviseMode = 1) And (.intManualReviseType = 0)) Then
                    Call ROUND4(.dblRotateTheta)                        '�ƍڕ���̐�Ίp�x�w���] '###037
                    GoTo STP_START
                End If
                'V1.19.0.0-27�@��
                ' �ƕ␳�蓮�ŁA�P��݂̂̏ꍇ�̓t���O���`�F�b�N���Ď��s�ς݂Ȃ牽�����Ȃ�
                If (.intReviseMode = 1) And (.intManualReviseType = 1) Then
                    If (gManualThetaCorrection = True) Then
                    Else
                        GoTo STP_START
                    End If
                End If
                'V1.19.0.0-27�@��

                '�ƕ␳����
                'r = SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG)
                r = SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG, True, False) 'V5.0.0.9�I
                If (r <> cFRS_NORMAL) Then                              ' ERROR ?
                    If (r <= cFRS_VIDEO_PTN) Then                       ' �p�^�[���F���G���[ ?
                        Call Beep()                                     ' Beep��
                        If (strMSG = "") Then                           '###038
                            ' ���O��ʂɕ������\������(�}�b�`���O�G���[, �p�^�[���ԍ��G���[)
                            strMSG = MSG_LOADER_07 + " (" + gbRotCorrectCancel.ToString + ")"
                        Else
                            'If (pltInfo.intRecogDispMode = 0) Then      ' ���ʕ\�����Ȃ� ?
                            If (pltInfo.intRecogDispMode = 0) AndAlso
                                (0 = pltInfo.intRecogDispModeRgh) Then  ' ���ʕ\�����Ȃ� ? 'V5.0.0.9�D
                                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                                '    ' �p�^�[���}�b�`���O�G���[(臒l)
                                '    strMSG = MSG_LOADER_07 + "(臒l)"
                                'Else
                                '    strMSG = MSG_LOADER_07 + "(Thresh)"
                                'End If
                                strMSG = MSG_LOADER_07 & Form1_008
                            End If
                        End If
                        Call Z_PRINT(strMSG & vbCrLf)
                        Move_Trimposition = cFRS_ERR_PTN                ' Return�l = �p�^�[���F���G���[
                    Else
                        Move_Trimposition = r
                    End If
                    Exit Function
                End If

                'V1.19.0.0-27�@��
                gManualThetaCorrection = False
                'V1.19.0.0-27�@��

                ' �ƕ␳���\��
                'If (pltInfo.intRecogDispMode = 1) Then                  ' ���ʕ\������ ? '###038
                If (pltInfo.intRecogDispMode = 1) OrElse
                    (0 <> pltInfo.intRecogDispModeRgh) Then             ' ���ʕ\������ ?  'V5.0.0.9�D
                    Call Z_PRINT(strMSG)
                End If

                'V4.10.0.0�B��
                If gbIntegratedMode Then                                ' ���݈ꊇ�e�B�[�`���O���[�h�̎� True
                    gbCorrectDone = True                                ' �I�y���[�^�e�B�[�`���O�ȑf�� ��x�w�x���␳���s�������͎��{���Ȃ��ׂɎg�p
                End If
                'V4.10.0.0�B��
STP_START:
                '--------------------------------------------------------------------------
                '  XY�e�[�u�����g�����ʒu�ֈړ�����
                '--------------------------------------------------------------------------
                '----- V1.13.0.0�B�� -----
                ' �I�[�g�v���[�u�o�^/���s�Ȃ�ƕ␳�̂ݎ��s���␳�l(gfCorrectPosX, gfCorrectPosY)�����߂�
                If (giAppMode = APP_MODE_APROBEREC) Or (giAppMode = APP_MODE_APROBEEXE) Then
                    ' XY�e�[�u�������_�ֈړ�����
                    r = System1.EX_SMOVE2(gSysPrm, 0.0, 0.0)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                        Move_Trimposition = r                           ' Return�l�ݒ�
                        Exit Function
                    End If
                    Exit Function
                End If
                '----- V1.13.0.0�B�� -----
                '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
                ''----- V1.14.0.0�F�� -----
                ' �L�����u���[�V�������̓u���b�N�T�C�Y��80�p�Ƃ���
                If (giAppMode = APP_MODE_CARIB_REC) Or (giAppMode = APP_MODE_CARIB) Then
                    ' �u���b�N�T�C�Y�ݒ�
                    If (typPlateInfo.intResistDir = 0) Then             ' ��R(����)���ѕ���(0:X, 1:Y)
                        '----- V1.24.0.0�@�� -----
                        If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                            r = System1.EX_BSIZE(gSysPrm, 60.0, 20.0)
                            'V6.0.4.0�@��
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                            r = System1.EX_BSIZE(gSysPrm, 60.0, 30.0)
                            'V6.0.4.0�@��
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then    'V4.4.0.0�@
                            r = System1.EX_BSIZE(gSysPrm, 90.0, 0.0)
                        Else
                            r = System1.EX_BSIZE(gSysPrm, 80.0, 0.0)
                        End If
                        'r = System1.EX_BSIZE(gSysPrm, 80.0, 0.0)
                        '----- V1.24.0.0�@�� -----
                    Else
                        '----- V1.24.0.0�@�� -----
                        If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                            r = System1.EX_BSIZE(gSysPrm, 20.0, 60.0)
                            'V6.0.4.0�@��
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                            r = System1.EX_BSIZE(gSysPrm, 30.0, 60.0)
                            'V6.0.4.0�@��
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then    'V4.4.0.0�@
                            r = System1.EX_BSIZE(gSysPrm, 0.0, 90.0)
                        Else
                            r = System1.EX_BSIZE(gSysPrm, 0.0, 80.0)
                        End If
                        'r = System1.EX_BSIZE(gSysPrm, 0.0, 80.0)
                        '----- V1.24.0.0�@�� -----
                    End If

                    '' �L�����u���[�V�������̓u���b�N�T�C�Y��80�p�Ƃ���
                    'If (giAppMode = APP_MODE_CARIB_REC) Or (giAppMode = APP_MODE_CARIB) Then
                    '    ' �u���b�N�T�C�Y�ݒ�
                    '    '----- V1.20.0.1�A�� -----
                    '    ' �u���b�N�T�C�Y�͂��̂܂܂Ƃ���
                    '    'r = System1.EX_BSIZE(gSysPrm, 80.0, 80.0)                          
                    '    r = System1.EX_BSIZE(gSysPrm, .dblBlockSizeXDir, .dblBlockSizeYDir)
                    '    '----- V1.20.0.1�A�� -----
                    '----- V3.0.0.0�E(V1.22.0.0�F)�� -----
                    If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                        Move_Trimposition = r                               ' Return�l�ݒ�
                        Exit Function
                    End If
                    ' BP�I�t�Z�b�g�ݒ�(���u���b�N�T�C�Y��ݒ肷���BP�I�t�Z�b�g��INtime���ŏ����������)
                    r = System1.EX_BPOFF(gSysPrm, 0.0, 0.0)
                    If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                        Move_Trimposition = r                               ' Return�l�ݒ�
                        Exit Function
                    End If
                Else
                    ' �u���b�N�T�C�Y�ݒ�
                    r = System1.EX_BSIZE(gSysPrm, .dblBlockSizeXDir, .dblBlockSizeYDir)
                    If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                        Move_Trimposition = r                               ' Return�l�ݒ�
                        Exit Function
                    End If
                    ' BP�I�t�Z�b�g�ݒ�(���u���b�N�T�C�Y��ݒ肷���BP�I�t�Z�b�g��INtime���ŏ����������)
                    r = System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                    If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                        Move_Trimposition = r                               ' Return�l�ݒ�
                        Exit Function
                    End If
                End If
                '' �u���b�N�T�C�Y�ݒ�
                'r = System1.EX_BSIZE(gSysPrm, .dblBlockSizeXDir, .dblBlockSizeYDir)
                'If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                '    Move_Trimposition = r                               ' Return�l�ݒ�
                '    Exit Function
                'End If
                '' BP�I�t�Z�b�g�ݒ�(���u���b�N�T�C�Y��ݒ肷���BP�I�t�Z�b�g��INtime���ŏ����������)
                'r = System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                '    Move_Trimposition = r                               ' Return�l�ݒ�
                '    Exit Function
                'End If
                '----- V1.14.0.0�F�� -----

                'V6.0.0.0-28                ' �X�e�[�W�ړ��O�Ƀr�f�I�̍X�V��������U��~
                'V6.0.0.0-28                Call Me.VideoLibrary1.VideoStop()

                '----- V1.13.0.0�B�� -----
                ' XY�e�[�u���I�t�Z�b�g��ݒ肷��(�X�e�[�W�I�t�Z�b�g�{�ƕ␳�l+�I�t�Z�b�g+�I�[�g�v���[�u�I�t�Z�b�g)
                '----- V1.14.0.0�F�� -----
                ' �L�����u���[�V�������̓X�e�[�W�I�t�Z�b�g���������Ȃ�
                If (giAppMode = APP_MODE_CARIB_REC) Or (giAppMode = APP_MODE_CARIB) Then
                    '----- V1.22.0.0�F�� -----
                    ' �u���b�N�T�C�Y��0�ɂ����̂ŁA�I�t�Z�b�g�Ɋ���W+�I�t�Z�b�g�����Z����
                    If (typPlateInfo.intResistDir = 0) Then             ' ��R(����)���ѕ���(0:X, 1:Y)
                        'V4.7.1.0�@                        StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX
                        StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX + .dblCaribTableOffsetXDir  'V4.7.1.0�@
                        StgOfsY = gfCorrectPosY + TblOfsY + gfStgOfsY + .dblCaribBaseCordnt1YDir + .dblCaribTableOffsetYDir
                    Else
                        StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX + .dblCaribBaseCordnt1XDir + .dblCaribTableOffsetXDir
                        StgOfsY = gfCorrectPosY + TblOfsY + gfStgOfsY
                    End If
                    'StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX
                    'StgOfsY = gfCorrectPosY + TblOfsY + gfStgOfsY
                    '----- V1.22.0.0�F�� -----
                    '----- V1.24.0.0�@�� -----
                    ' �X�e�[�W�I�t�Z�b�g������
                    If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                        StgOfsX = StgOfsX + .dblTableOffsetXDir
                        StgOfsY = StgOfsY + .dblTableOffsetYDir
                    End If
                    '----- V1.24.0.0�@�� -----
                Else
                    StgOfsX = .dblTableOffsetXDir + gfCorrectPosX + TblOfsX + gfStgOfsX
                    StgOfsY = .dblTableOffsetYDir + gfCorrectPosY + TblOfsY + gfStgOfsY
                End If
                'StgOfsX = .dblTableOffsetXDir + gfCorrectPosX + TblOfsX + gfStgOfsX
                'StgOfsY = .dblTableOffsetYDir + gfCorrectPosY + TblOfsY + gfStgOfsY
                '----- V1.14.0.0�F�� -----
                '----- V1.13.0.0�B�� -----

                '----- V2.0.0.0�H�� -----
                If (giMachineKd = MACHINE_KD_RS) Then
                    '----- V4.0.0.0-40�� -----
                    ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
                    If (giStageYOrg = STGY_ORG_UP) Then
                        StgOfsY = StgOfsY
                    Else
                        StgOfsY = StgOfsY
                    End If
                    'StgOfsY = StgOfsY + (typPlateInfo.dblBlockSizeYDir / 2)
                    '----- V4.0.0.0-40�� -----
                End If
                '----- V2.0.0.0�H�� -----

                '----- V1.24.0.0�@�� -----
                ' ����F�V�[�^���ŊO���J�����ʒu�ֈړ�����ꍇ(�L�����u���[�V����������)
                If (giAppMode = APP_MODE_EXCAM_R1TEACH) Or (giAppMode = APP_MODE_EXCAM_TEACH) Or
                   (giAppMode = APP_MODE_CUTREVISE_REC) Or (giAppMode = APP_MODE_CUTREVIDE) Then
                    If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                        StgOfsX = StgOfsX - BSZ_6060_OFSX
                        StgOfsY = StgOfsY - BSZ_6060_OFSY
                    End If
                End If
                '----- V1.24.0.0�@�� -----

                ' XY�e�[�u�����g�����ʒu�ֈړ�(�g�����ʒu�{�X�e�[�W�I�t�Z�b�g�{�␳�l+�I�t�Z�b�g)
                'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + TblOfsX, .dblTableOffsetYDir + gfCorrectPosY + TblOfsY, 0)' V1.13.0.0�B
                r = System1.EX_START(gSysPrm, StgOfsX, StgOfsY, 0)      ' V1.13.0.0�B
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Move_Trimposition = r                               ' Return�l�ݒ�
                    Exit Function
                End If

                'V6.0.0.0-28                ' �X�e�[�W�ړ���Ƀr�f�I�̍X�V�������Ď��{
                'V6.0.0.0-28                Call Me.VideoLibrary1.VideoStart()
                VideoLibrary1.SetCrossLineObject(gparModules)   'V5.0.0.6�K
                gparModules.DispCrossLine(0, 0)                 'V5.0.0.6�K
                '--------------------------------------------------------------------------
                '   ZOff�ʒu�ֈړ�
                '--------------------------------------------------------------------------
                r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Move_Trimposition = r                               ' Return�l�ݒ�
                    Exit Function
                End If
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Move_Trimposition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Move_Trimposition = cERR_TRAP                               ' �ߒl = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�ƕ␳�̂��߂̉摜�o�^(F9)���݉���������"
    '''=========================================================================
    '''<summary>�ƕ␳�̂��߂̉摜�o�^(F9)����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdPattern_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdPattern.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_RECOG, APP_MODE_RECOG, MSG_OPLOG_FUNC09, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdPattern_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�N�����v���X�ڕ���E���t�A���C�����g�p�摜�o�^����"
    '''=========================================================================
    ''' <summary>�N�����v���X�ڕ���E���t�A���C�����g�p�摜�o�^����</summary>
    ''' <remarks>'V5.0.0.9�C</remarks>
    '''=========================================================================
    Private Sub CmdRecogRough_Click(sender As Object, e As EventArgs) Handles CmdRecogRough.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_RECOG_ROUGH, APP_MODE_RECOG, MSG_OPLOG_FUNC09, "", True)   ' TODO: MSG_OPLOG_FUNC09����ύX���邩

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdRecogRough_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�ƕ␳�̂��߂̃p�^�[���o�^�����ݒ菈��"
    '''=========================================================================
    '''<summary>�ƕ␳�̂��߂̃p�^�[���o�^�����ݒ菈��</summary>
    '''=========================================================================
    Private Function InitThetaCorrection(ByVal isRough As Boolean,
                                         Optional ByVal clampLessOffsetX As Double = 0.0, Optional ByVal clampLessOffsetY As Double = 0.0,
                                         Optional ByVal correctTrimPosX As Double = 0.0, Optional ByVal correctTrimPosY As Double = 0.0) As Integer   'V5.0.0.9�B 'V5.0.0.9�D
        'Private Function InitThetaCorrection() As Integer

        Dim strMSG As String                                            ' ���b�Z�[�W�ҏW��

        Try
            ' Video.OCX�̃v���p�e�B��ݒ肷��
            VideoLibrary1.pfTrim_x = gSysPrm.stDEV.gfTrimX              ' �g���~���O�|�W�V����X(mm)
            VideoLibrary1.pfTrim_y = gSysPrm.stDEV.gfTrimY              ' �g���~���O�|�W�V����Y(mm)
            VideoLibrary1.pfBlock_x = typPlateInfo.dblBlockSizeXDir     ' Block Size X
            VideoLibrary1.pfBlock_y = typPlateInfo.dblBlockSizeYDir     ' Block Size Y
            VideoLibrary1.zwaitpos = typPlateInfo.dblZWaitOffset        ' Z PROBE OFF OFFSET
            'VideoLibrary1.ThetaRCenterX = gSysPrm.stDEV.gfRot_X1        ' ��]���S���W X mm
            'V6.1.1.0�P            VideoLibrary1.ThetaRCenterX = gSysPrm.stDEV.gfRot_X1 + correctTrimPosX + clampLessOffsetX   ' ��]���S���W X mm  'V5.0.0.9�B
            VideoLibrary1.ThetaRCenterX = gSysPrm.stDEV.gfRot_X1   ' ��]���S���W X mm  'V5.0.0.9�B
            'VideoLibrary1.ThetaRCenterY = gSysPrm.stDEV.gfRot_Y1        ' ��]���S���W Y mm
            ''V6.1.1.0�P            VideoLibrary1.ThetaRCenterY = gSysPrm.stDEV.gfRot_Y1 + correctTrimPosY + clampLessOffsetY   ' ��]���S���W Y mm  'V5.0.0.9�B
            VideoLibrary1.ThetaRCenterY = gSysPrm.stDEV.gfRot_Y1   ' ��]���S���W Y mm  'V5.0.0.9�B
            VideoLibrary1.PP18 = 0                                      ' Z�ҋ@�ʒu
            VideoLibrary1.pfStgOffX = typPlateInfo.dblTableOffsetXDir   ' Trim Position Offset Y(mm) 'V1.13.0.0�B
            VideoLibrary1.pfStgOffY = typPlateInfo.dblTableOffsetYDir   ' Trim Position Offset Y(mm) 'V1.13.0.0�B

            If (True = isRough) Then
                ' �N�����v���X�ڕ���E���t�A���C�����g�摜�o�^        'V5.0.0.9�B  ��
                VideoLibrary1.PP30 = 0                                  ' �␳���[�h(0:����,1:�蓮, 2:����+����)
                VideoLibrary1.PP31 = 2                                  ' �␳���@(0:�Ȃ�,1:1��̂�,2:����j��PP30=0�̂Ƃ��͖���
                VideoLibrary1.pp32_x = typPlateInfo.dblReviseCordnt1XDirRgh + correctTrimPosX + clampLessOffsetX ' �p�^�[��1���Wx
                VideoLibrary1.pp32_y = typPlateInfo.dblReviseCordnt1YDirRgh + correctTrimPosY + clampLessOffsetY ' �p�^�[��1���Wy
                VideoLibrary1.PP33X = typPlateInfo.dblReviseCordnt2XDirRgh + correctTrimPosX + clampLessOffsetX ' �p�^�[��2���Wx
                VideoLibrary1.PP33Y = typPlateInfo.dblReviseCordnt2YDirRgh + correctTrimPosY + clampLessOffsetY ' �p�^�[��2���Wy
                VideoLibrary1.pp34_x = typPlateInfo.dblReviseOffsetXDirRgh      ' �␳�|�W�V�����I�t�Z�b�gx
                VideoLibrary1.pp34_y = typPlateInfo.dblReviseOffsetYDirRgh      ' �␳�|�W�V�����I�t�Z�b�gy
                ' �����J�����ł����Ȃ�
                VideoLibrary1.pp36_x = gSysPrm.stGRV.gfPixelX                   ' X�s�N�Z������\ um
                VideoLibrary1.pp36_y = gSysPrm.stGRV.gfPixelY                   ' Y�s�N�Z������\ um
                VideoLibrary1.PP37_1 = typPlateInfo.intRevisePtnNo1Rgh          ' �p�^�[��1 �e���v���[�g�ԍ�
                VideoLibrary1.PP37_2 = typPlateInfo.intRevisePtnNo2Rgh          ' �p�^�[��2 �e���v���[�g�ԍ�
                VideoLibrary1.PP52 = typPlateInfo.intRevisePtnNo1GroupNoRgh     ' �p�^�[��1 �O���[�v�ԍ�
                VideoLibrary1.PP52_1 = typPlateInfo.intRevisePtnNo2GroupNoRgh   ' �p�^�[��2 �O���[�v�ԍ�
                VideoLibrary1.PP53 = typPlateInfo.dblRotateTheta                ' �Ǝ��p�x
            Else                                           'V5.0.0.9�B  ��
                ' �ƕ␮
                VideoLibrary1.PP30 = typPlateInfo.intReviseMode         ' �␳���[�h(0:����,1:�蓮, 2:����+����)
                VideoLibrary1.PP31 = typPlateInfo.intManualReviseType   ' �␳���@(0:�Ȃ�,1:1��̂�,2:����j��PP30=0�̂Ƃ��͖���
                ' �蓮�␳���[�h�ŕ␳���� ?
                If (typPlateInfo.intReviseMode = 1) Then
                    VideoLibrary1.PP31 = 2                                  ' �蓮�␳���̓��� = ����
                End If
                VideoLibrary1.pp32_x = typPlateInfo.dblReviseCordnt1XDir + correctTrimPosX + clampLessOffsetX   ' �p�^�[��1���Wx  'V5.0.0.9�D
                VideoLibrary1.pp32_y = typPlateInfo.dblReviseCordnt1YDir + correctTrimPosY + clampLessOffsetY   ' �p�^�[��1���Wy  'V5.0.0.9�D
                VideoLibrary1.PP33X = typPlateInfo.dblReviseCordnt2XDir + correctTrimPosX + clampLessOffsetX    ' �p�^�[��2���Wx  'V5.0.0.9�D
                VideoLibrary1.PP33Y = typPlateInfo.dblReviseCordnt2YDir + correctTrimPosY + clampLessOffsetY    ' �p�^�[��2���Wy  'V5.0.0.9�D
                VideoLibrary1.pp34_x = typPlateInfo.dblReviseOffsetXDir     ' �␳�|�W�V�����I�t�Z�b�gx 'V5.0.0.9�D
                VideoLibrary1.pp34_y = typPlateInfo.dblReviseOffsetYDir     ' �␳�|�W�V�����I�t�Z�b�gy 'V5.0.0.9�D
                If (gSysPrm.stDEV.giEXCAM = 1) Then                         ' �O���J���� ?
                    'VideoLibrary1.pp34_x = 0.0#                             ' �蓮�ŕ␳���莞 pp34_x,y���@###100
                    'VideoLibrary1.pp34_y = 0.0#                             ' �����̂�0�ɂ���            ###100
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfEXCAM_PixelX     ' X�s�N�Z������\ um
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfEXCAM_PixelY     ' Y�s�N�Z������\ um
                Else
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfPixelX           ' X�s�N�Z������\ um
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfPixelY           ' Y�s�N�Z������\ um
                End If
                VideoLibrary1.PP37_1 = typPlateInfo.intRevisePtnNo1         ' �p�^�[��1 �e���v���[�g�ԍ�
                VideoLibrary1.PP37_2 = typPlateInfo.intRevisePtnNo2         ' �p�^�[��2 �e���v���[�g�ԍ�
                VideoLibrary1.PP52 = typPlateInfo.intRevisePtnNo1GroupNo    ' �p�^�[��1 �O���[�v�ԍ�
                ' 'V4.0.0.0-45
                VideoLibrary1.PP52_1 = typPlateInfo.intRevisePtnNo2GroupNo    ' �p�^�[��2 �O���[�v�ԍ�
                VideoLibrary1.PP53 = typPlateInfo.dblRotateTheta            ' �Ǝ��p�x
            End If

            'VideoLibrary1.PP35 = 1                                     ' Debug�p(1:on,0:off)
            VideoLibrary1.PP35 = 0                                      ' Debug�p(1:on,0:off)
            VideoLibrary1.RNASTMPNUM = False
            VideoLibrary1.frmLeft = Me.Text4.Location.Y                 ' �\���ʒu(Left)
            VideoLibrary1.frmTop = Me.Text4.Location.X                  ' �\���ʒu(Top)

            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.InitThetaCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            InitThetaCorrection = cERR_TRAP                             ' �ߒl = �ׯ�ߴװ����
        End Try

    End Function
#End Region

#Region "�ƕ␳���s�T�u���[�`��"
    '''=========================================================================
    '''<summary>�ƕ␳���s�T�u���[�`��</summary>
    '''<param name="pltInfo">    (INP) �v���[�g�f�[�^</param>
    '''<param name="dblCorrectX">(OUT) XY�e�[�u���␳�lX</param>
    '''<param name="dblCorrectY">(OUT) XY�e�[�u���␳�lY</param>
    '''<param name="strDAT">     (OUT) �ƕ␳���ʕҏW��</param>
    ''' <param name="doAlign">(INP)�ƕ␳���s����E���Ȃ�</param>
    ''' <param name="doRough">(INP)���t�A���C�����g���s����E���Ȃ�</param>
    '''<returns>cFRS_NORMAL   = ����
    '''         cFRS_ERR_RST  = Cancel(RESET��)
    '''         cFRS_ERR_PTN  = �p�^�[���F���G���[
    '''         ��L�ȊO      = ����~�����̑��G���[
    '''</returns>
    '''=========================================================================
    Public Function SUb_ThetaCorrection(ByVal pltInfo As PlateInfo, ByRef dblCorrectX As Double, ByRef dblCorrectY As Double,
                                        ByRef strDAT As String, ByVal doAlign As Boolean, ByVal doRough As Boolean) As Integer
        'Public Function SUb_ThetaCorrection(ByVal pltInfo As PlateInfo, ByRef dblCorrectX As Double, ByRef dblCorrectY As Double, _
        '                                     ByRef strDAT As String) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String                                            ' ���b�Z�[�W�ҏW��
        'Dim stResult As LaserFront.Trimmer.DllVideo.VideoLibrary.Theta_Cor_Info                          ' �ƕ␳���� ���Theta_Cor_Info���Video.OCX�Œ�`
        Dim results As New List(Of PatternRecog)                        ' �ƕ␳���� 'V5.0.0.9�D
        'Dim Thresh1 As Double
        'Dim Thresh2 As Double

        Try
            ' �ƕ␳�����s����
            r = ThetaCorrection(gfCorrectPosX, gfCorrectPosY, results, doAlign, doRough)  '�ƕ␳���s
            If (r <> cFRS_NORMAL) AndAlso (cFRS_ERR_PT2 <> r) Then      ' ERROR ?       ' 臒l�װ�ł͂Ȃ��H 'V5.0.0.9�D
                ' �p�^�[�����o�ł��Ȃ������ꍇ
                'V6.1.4.16�@��
                If (r <= ((-1) * ERR_TIMEOUT_BASE)) Then                            ' INtime���ŃX�e�[�W�h�G���[������
                    'If (r <= cFRS_VIDEO_PTN) Then                           ' �p�^�[���F���G���[ ?
                ElseIf (r <= cFRS_VIDEO_PTN) Then                           ' �p�^�[���F���G���[ ?
                    'V6.1.4.16�@��
                    r = cFRS_ERR_PTN                                    ' Return�l = �p�^�[���F���G���[
                End If
                Return (r)
            End If

#If True Then   'V5.0.0.9�D
                If (0 < results.Count) Then

                Dim sb As New StringBuilder(256)

                With pltInfo
                    Const HL As Integer = 80

                    ' ���t�A���C�����g���s���Ɍ��ʕ\������E���Ȃ�
                    If (True = doRough) AndAlso (0 <> .intRecogDispModeRgh) Then

                        Dim line As String = New String("-"c, HL)
                        ' ���t�A���C�����g�̗v�f�����o��
                        For Each ptr As PatternRecog In results.Where(Function(pr)
                                                                          Return pr.IsRough
                                                                      End Function)
                            sb.AppendLine(line)
                            r = GetRecogResult(0, 0.0, ptr, sb)

                            If (cFRS_NORMAL <> r) Then
                                Exit For
                            End If
                        Next

                        If (cFRS_NORMAL <> r) OrElse (False = doAlign) OrElse (0 = .intRecogDispMode) Then
                            ' ���t�A���C�����g�̌��ʂ̂ݕ\������ꍇ
                            sb.AppendLine(line)
                        End If
                    End If

                    ' ���t�A���C�����g����i�܂��͖����s�j���Ƀƕ␳���ʕ\������E���Ȃ�
                    If (cFRS_NORMAL = r) AndAlso (True = doAlign) AndAlso (0 <> .intRecogDispMode) Then
                        Dim result As PatternRecog = results(results.Count - 1)
                        If (False = result.IsRough) Then
                            Dim line As String = New String("="c, HL)
                            sb.AppendLine(line)
                            r = GetRecogResult(.intReviseMode, .dblRotateTheta, result, sb)
                            sb.AppendLine(line)
                        End If
                        'V4.12.2.3�A��'V6.0.1.0�R
                    Else
                        Dim result As PatternRecog = results(results.Count - 1)
                        If (False = result.IsRough) Then
                            If (False = result.IsMatch) Then
                                r = (cFRS_ERR_PT2)                    ' Return�l = �p�^�[���F���G���[(臒l�G���[)
                            End If
                        End If
                    End If
                    'V4.12.2.3�A�� 'V6.0.1.0�R
                End With

                Debug.Print(sb.ToString())
                If (cERR_TRAP <> r) Then
                    strDAT = sb.ToString()
                End If
            End If
#Else
            ' 臒l�擾 '###038
            Thresh1 = gDllSysprmSysParam_definst.GetPtnMatchThresh(typPlateInfo.intRevisePtnNo1GroupNo, typPlateInfo.intRevisePtnNo1)
            Thresh2 = gDllSysprmSysParam_definst.GetPtnMatchThresh(typPlateInfo.intRevisePtnNo1GroupNo, typPlateInfo.intRevisePtnNo2)

            ' �ƕ␳���ʎ擾
            Call VideoLibrary1.GetThetaResult(stResult)
            With pltInfo
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    ' �ƕ␳�\�����ݒ�(���{��)
                '    strDAT = "�Ɗp�x = " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "��" & vbCrLf
                '    If (.intReviseMode = 2) Then                        ' ����+�����̏ꍇ
                '        strDAT = "�Ɗp�x = " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "��"
                '        strDAT = strDAT & "+ " & .dblRotateTheta.ToString("0.0000").PadLeft(7) & "��" & vbCrLf
                '    End If
                '    strDAT = strDAT & "  �g�����ʒuX,Y=" & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPosy.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " �����X,Y    =" & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCory.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  �␳�ʒu1X,Y =" & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos1y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " �����1X,Y   =" & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor1y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  �␳�ʒu2X,Y =" & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos2y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " �����2X,Y   =" & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor2y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    If (.intReviseMode <> 1) Then                       ' �����␳���[�h ? '###038
                '        strDAT = strDAT & "  ��v�xPOS1   =" & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ","
                '        strDAT = strDAT & " ��v�xPOS2   =" & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                '    End If
                'Else
                '    ' �ƕ␳�\�����ݒ�(�p��)
                '    strDAT = "Theta = " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "degree" & vbCrLf
                '    If (.intReviseMode = 2) Then                        ' ����+�����̏ꍇ
                '        strDAT = "Theta= " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "degree"
                '        strDAT = strDAT & " + " & .dblRotateTheta.ToString("0.0000").PadLeft(7) & "degree" & vbCrLf
                '    End If
                '    strDAT = strDAT & "  Trim PositionXY=" & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPosy.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " Distance=" & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCory.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  Correct position1=" & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos1y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " Distance1=" & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor1y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  Correct position2=" & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos2y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " Distance2=" & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor2y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    If (.intReviseMode <> 1) Then                       ' �����␳���[�h ? '###038
                '        strDAT = strDAT & "  Correlation coefficient1=" & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ","
                '        strDAT = strDAT & " Correlation coefficient2=" & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                '    End If
                'End If

                ' �ƕ␳�\�����ݒ�
                strDAT = Form1_009 & stResult.fTheta.ToString("0.0000").PadLeft(7) & Form1_018 & vbCrLf
                If (.intReviseMode = 2) Then                        ' ����+�����̏ꍇ
                    strDAT = Form1_009 & stResult.fTheta.ToString("0.0000").PadLeft(7) & Form1_018
                    strDAT = strDAT & "+ " & .dblRotateTheta.ToString("0.0000").PadLeft(7) & Form1_018 & vbCrLf
                End If

                Const lftLen As Integer = 18
                Const rhtLen As Integer = 11
                Dim sp As Integer
                '    strDAT = strDAT & "  �g�����ʒuX,Y=" & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_010)
                strDAT = strDAT & "  " & Form1_010 & New String(" ", sp) & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fPosy.ToString("0.0000").PadLeft(9) & "  "

                '    strDAT = strDAT & " �����X,Y    =" & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_011)
                strDAT = strDAT & "  " & Form1_011 & New String(" ", sp) & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fCory.ToString("0.0000").PadLeft(9) & vbCrLf

                '    strDAT = strDAT & "  �␳�ʒu1X,Y =" & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_012)
                strDAT = strDAT & "  " & Form1_012 & New String(" ", sp) & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fPos1y.ToString("0.0000").PadLeft(9) & "  "

                '    strDAT = strDAT & " �����1X,Y   =" & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_013)
                strDAT = strDAT & "  " & Form1_013 & New String(" ", sp) & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fCor1y.ToString("0.0000").PadLeft(9) & vbCrLf

                '    strDAT = strDAT & "  �␳�ʒu2X,Y =" & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_014)
                strDAT = strDAT & "  " & Form1_014 & New String(" ", sp) & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fPos2y.ToString("0.0000").PadLeft(9) & "  "

                '    strDAT = strDAT & " �����2X,Y   =" & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_015)
                strDAT = strDAT & "  " & Form1_015 & New String(" ", sp) & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fCor2y.ToString("0.0000").PadLeft(9) & vbCrLf
                If (.intReviseMode <> 1) Then                       ' �����␳���[�h ? '###038
                    '        strDAT = strDAT & "  ��v�xPOS1   =" & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ","
                    strDAT = strDAT & "  " & Form1_016 & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ", "
                    '        strDAT = strDAT & " ��v�xPOS2   =" & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                    strDAT = strDAT & Form1_017 & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                End If
            End With

            ' 臒l���� '###038
            If (pltInfo.intReviseMode <> 1) Then                        ' �����␳���[�h ? 
                If (Thresh1 > stResult.fCorV1) Or (Thresh2 > stResult.fCorV2) Then
                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    '    ' �p�^�[���}�b�`���O�G���[(臒l)
                    '    strDAT = strDAT + vbCrLf + MSG_LOADER_07 + "(臒l)"
                    'Else
                    '    strDAT = strDAT + vbCrLf + MSG_LOADER_07 + "(Thresh)"
                    'End If
                    strDAT = strDAT + vbCrLf + MSG_LOADER_07 + Form1_008
                    Return (cFRS_ERR_PTN)                               ' Return�l = �p�^�[���F���G���[
                End If
            End If
#End If
            'Return (cFRS_NORMAL)
            Return r   'V5.0.0.9�D

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SUb_ThetaCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' �ߒl = �ׯ�ߴװ����
        End Try

    End Function
#End Region

#Region "�ƕ␳���s"
    '''=========================================================================
    '''<summary>�ƕ␳���s</summary>
    '''<param name="dblCorrectX">(OUT) XY�e�[�u���␳�lX</param>
    '''<param name="dblCorrectY">(OUT) XY�e�[�u���␳�lY</param>
    ''' <param name="results">�ƕ␳���ʃ��X�g</param>
    '''<returns>cFRS_NORMAL   = ����
    '''         cFRS_ERR_PTN  = �p�^�[���F���G���[
    '''         ��L�ȊO      = ���̑��G���[
    '''</returns>
    '''=========================================================================
    Private Function ThetaCorrection(ByRef dblCorrectX As Double, ByRef dblCorrectY As Double, ByVal results As List(Of PatternRecog),
                                     ByVal doAlign As Boolean, ByVal doRough As Boolean) As Integer 'V5.0.0.9�I
        'Private Function ThetaCorrection(ByRef dblCorrectX As Double, ByRef dblCorrectY As Double) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String                                ' ���b�Z�[�W�ҏW��

        Try
            ' ��������
            ThetaCorrection = cFRS_NORMAL                   ' Return�l = ����
#If False Then
            Call InitThetaCorrection()                      ' �p�^�[���o�^�����l�ݒ�

            ' �J�����ؑ�
            If (gSysPrm.stDEV.giEXCAM = 1) Then             ' �O���J�������g�p�H
                Call VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)          ' �J�����ؑ�(�O�����)
            End If

            ' �ƕ␳����
            VideoLibrary1.frmTop = Me.Text4.Location.Y      ' ###027
            VideoLibrary1.frmLeft = Me.Text4.Location.X     ' ###027

            Call VideoLibrary1.PatternDisp(True)            ' �p�^�[���}�b�`���O���̌����͈͘g�\�� '###155 
            r = VideoLibrary1.CorrectTheta(giAppMode)       ' �ƕ␳
            Call VideoLibrary1.PatternDisp(False)           ' �p�^�[���}�b�`���O���̌����͈͘g��\�� '###155

            ' XY�e�[�u���␳�l(�ƕ␳����XYð��ق����)�擾
            If (r = 0) Then
                dblCorrectX = VideoLibrary1.CorrectTrimPosX
                dblCorrectY = VideoLibrary1.CorrectTrimPosY
            Else
                dblCorrectX = 0
                dblCorrectY = 0
            End If
            '###252
            '----- V1.13.0.0�H(Video.OCX V8.0.13�Ή�) -----
            ''###252
            'Select Case gSysPrm.stDEV.giBpDirXy
            '    Case 0 ' �E��(x��, y��)
            '    Case 1
            '        dblCorrectX = -1 * dblCorrectX
            '    Case 2
            '    Case 3
            'End Select
            '----- V1.13.0.0�H -----
#Else
            'V5.0.0.9�E                  ��
            Dim threshPt1 As Double
            Dim threshPt2 As Double
            Dim correctTrimPosX As Double = 0.0
            Dim correctTrimPosY As Double = 0.0
            Dim clampLessOffsetX As Double
            Dim clampLessOffsetY As Double
            Dim correctTheta As Double = 0.0
            Dim result As PatternRecog

            '----- V6.0.3.0_49�� -----
            ' �p�^�[���}�b�`���O�͑傫����ʂōs��(SL436S��)
            If (gKeiTyp = MACHINE_KD_RS) Then
                SetTeachVideoSize()
                Form1.Instance.VideoLibrary1.BringToFront()
            End If
            '----- V6.0.3.0_49�� -----

            If (True = bFgAutoMode) AndAlso (0 <> giClampLessStage) Then
                ' �����������̃N�����v���X�ڕ������ڈʒu�I�t�Z�b�g
                clampLessOffsetX = gdClampLessOffsetX
                clampLessOffsetY = gdClampLessOffsetY

                If (True = doRough) Then
                    With typPlateInfo
                        ' ���t�A���C�����g臒l�擾
                        threshPt1 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo1GroupNoRgh, .intRevisePtnNo1Rgh)
                        threshPt2 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo2GroupNoRgh, .intRevisePtnNo2Rgh)
                    End With

                    For i As Integer = 1 To giClampLessRoughCount Step 1
                        ' ���t�A���C�����g���s
                        result = New PatternRecog(True, threshPt1, threshPt2)
                        r = SubThetaCorrection(True, clampLessOffsetX, clampLessOffsetY,
                                               correctTrimPosX, correctTrimPosY, correctTheta, result)
                        If (cFRS_NORMAL = r) Then
                            results.Add(result)
                        ElseIf (cFRS_ERR_PT2 = r) Then
                            results.Add(result)
                            Exit For
                        Else
                            Exit For
                        End If
                    Next i
                End If
            Else
                ' ��u���܂��̓N�����v���X�ڕ���ł͂Ȃ�
                clampLessOffsetX = 0.0
                clampLessOffsetY = 0.0
            End If

            If (cFRS_NORMAL = r) AndAlso (True = doAlign) Then
                ' �ƕ␳���s
                With typPlateInfo
                    ' �ƕ␮臒l�擾
                    threshPt1 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo1GroupNo, .intRevisePtnNo1)
                    threshPt2 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo2GroupNo, .intRevisePtnNo2)
                End With

                result = New PatternRecog(False, threshPt1, threshPt2)
                r = SubThetaCorrection(False, clampLessOffsetX, clampLessOffsetY,
                                       correctTrimPosX, correctTrimPosY, correctTheta, result)
                If (cFRS_NORMAL = r) Then
                    results.Add(result)
                End If
            End If

            ' XY�e�[�u���␳�l(�ƕ␳����XYð��ق����)�擾
            If (cFRS_NORMAL = r) AndAlso (True = doAlign OrElse True = doRough) Then
                dblCorrectX = correctTrimPosX + clampLessOffsetX
                dblCorrectY = correctTrimPosY + clampLessOffsetY
                gdCorrectTheta = correctTheta
            Else
                dblCorrectX = 0.0
                dblCorrectY = 0.0
                gdCorrectTheta = 0.0
            End If
            'V5.0.0.9�E                  ��
#End If
            ' �㏈��
            If (gSysPrm.stDEV.giEXCAM = 1) Then             ' �O���J�������g�p�H
                Call VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)          ' �J�����ؑ�(�������)
            End If

            '----- V6.0.3.0_49�� -----
            If (gKeiTyp = MACHINE_KD_RS) Then
                SetSimpleVideoSize()
                Form1.Instance.VideoLibrary1.SendToBack()
            End If
            '----- V6.0.3.0_49�� -----

            ThetaCorrection = r
            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.ThetaCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            ThetaCorrection = cERR_TRAP                     ' �ߒl = �ׯ�ߴװ����
        End Try

    End Function
#End Region

#Region "VideoLibrary.CorrectTheta �����s����"
    ''' <summary>
    ''' VideoLibrary.CorrectTheta �����s����
    ''' </summary>
    ''' <param name="isRough">True:���t�A���C�����g</param>
    ''' <param name="clampLessOffsetX">����ڈʒu�I�t�Z�b�gX</param>
    ''' <param name="clampLessOffsetY">����ڈʒu�I�t�Z�b�gY</param>
    ''' <param name="correctTrimPosX">�␳�ʒuX�̂���ʂ��w��E���̒l�����Z�������s���ʂ̂���ʂ�Ԃ�</param>
    ''' <param name="correctTrimPosY">�␳�ʒuY�̂���ʂ��w��E���̒l�����Z�������s���ʂ̂���ʂ�Ԃ�</param>
    ''' <param name="correctTheta">�␳��̃Ɗp�x���w��E���̒l�����Z���ꂽ�␳��̃Ɗp�x��Ԃ�</param>
    ''' <param name="result">VideoLibrary.GetThetaResult(result)�Ŏ擾�����\����</param>
    ''' <returns>VideoLibrary.CorrectTheta(AppMode)�̖߂�l�܂���cERR_TRAP</returns>
    ''' <remarks>'V5.0.0.9�E</remarks>
    Private Function SubThetaCorrection(ByVal isRough As Boolean,
                                        ByVal clampLessOffsetX As Double, ByVal clampLessOffsetY As Double,
                                        ByRef correctTrimPosX As Double, ByRef correctTrimPosY As Double,
                                        ByRef correctTheta As Double,
                                        ByVal result As PatternRecog) As Integer

        Dim ret As Integer = cFRS_NORMAL                        ' Return�l = ����
        Try
            InitThetaCorrection(isRough, clampLessOffsetX, clampLessOffsetY, correctTrimPosX, correctTrimPosY)  ' �p�^�[���o�^�����l�ݒ�

            If (False = isRough) Then
                ' �J�����ؑ�
                If (gSysPrm.stDEV.giEXCAM = 1) Then             ' �O���J�������g�p�H
                    VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA) ' �J�����ؑ�(�O�����)
                End If
            End If

            ' �ƕ␳����
            VideoLibrary1.frmTop = Me.Text4.Location.Y      ' ###027
            VideoLibrary1.frmLeft = Me.Text4.Location.X     ' ###027

            'V6.0.0.0-25            VideoLibrary1.PatternDisp(True)            ' �p�^�[���}�b�`���O���̌����͈͘g�\�� '###155 
            'ret = VideoLibrary1.CorrectTheta(giAppMode)       ' �ƕ␳
            ret = VideoLibrary1.CorrectTheta(giAppMode, isRough, correctTheta)  ' �ƕ␳           'V5.0.0.9�F
            VideoLibrary1.PatternDisp(False)           ' �p�^�[���}�b�`���O���̌����͈͘g��\�� '###155

            ' �␳���ʎ擾
            If (cFRS_NORMAL = ret) Then
                VideoLibrary1.GetThetaResult(result.ThetaCorInfo)

                If (False = isRough) OrElse (result.IsMatch) Then
                    ' XY�e�[�u���␳�l(�ƕ␳����XYð��ق����)�擾
                    correctTrimPosX += VideoLibrary1.CorrectTrimPosX    ' X�̂����(mm)
                    correctTrimPosY += VideoLibrary1.CorrectTrimPosY    ' Y�̂����(mm)
                    correctTheta = result.ThetaCorInfo.fTheta           ' �␳��̃Ɗp�x VideoLibrary���ŗ݌v
                Else
                    ' ���t�A���C�����g�̏ꍇ�A臒l���߂Ń}�b�`���O�G���[�Ƃ���
                    ret = cFRS_ERR_PT2  ' �p�^�[���F���G���[(臒l�G���[)
                End If
            End If

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.SubThetaCorrection() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            ret = cERR_TRAP                     ' �ߒl = �ׯ�ߴװ����
        End Try

        Return ret

    End Function
#End Region

#Region "�A���C�����g���ʕ�����𐶐�����"
    ''' <summary>
    ''' �A���C�����g���ʕ�����𐶐�����
    ''' </summary>
    ''' <param name="reviseMode"></param>
    ''' <param name="rotateTheta"></param>
    ''' <param name="result"></param>
    ''' <param name="sb"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRecogResult(ByVal reviseMode As Integer, ByVal rotateTheta As Double,
                                    ByVal result As PatternRecog, ByVal sb As StringBuilder) As Integer

        Dim ret As Integer = (cFRS_NORMAL)
        Dim tci As VideoLibrary.Theta_Cor_Info = result.ThetaCorInfo
        Try
            Const lftLen As Integer = 18
            Const rhtLen As Integer = 11
            Const F4 As String = "F4"

            '#Const CLAMPLESS_TEST = True            'V5.0.0.9�J

#If CLAMPLESS_TEST Then
            WriteClamplessTestLog((82 = sb.Length), reviseMode, result.IsRough, result.IsMatch, tci) 'V5.0.0.9�J
#End If
            ' �ƕ␳�\�����ݒ�
            If (False = result.IsRough) Then    'V5.0.0.9�I
                sb.Append(Form1_023)            ' �A���C�����g: 
            Else
                sb.Append(Form1_024)            ' ���t�A���C�����g: 
            End If
            sb.AppendLine(Form1_009 & tci.fTheta.ToString(F4).PadLeft(7) & Form1_018)
            If (2 = reviseMode) Then                        ' ����+�����̏ꍇ
                sb.Append(Form1_009 & tci.fTheta.ToString(F4).PadLeft(7) & Form1_018)
                sb.AppendLine("+ " & rotateTheta.ToString(F4).PadLeft(7) & Form1_018)
            End If

            Dim sp As Integer
            '    strDAT = strDAT & "  �g�����ʒuX,Y=" & result.fPosx.ToString(F4).PadLeft(9) & ","
            sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_010)
            sb.Append("  " & Form1_010 & New String(" "c, sp) & tci.fPosx.ToString(F4).PadLeft(9) & ",")
            sb.Append(tci.fPosy.ToString(F4).PadLeft(9) & "  ")

            '    strDAT = strDAT & " �����X,Y    =" & result.fCorx.ToString(F4).PadLeft(9) & ","
            sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_011)
            sb.Append("  " & Form1_011 & New String(" "c, sp) & tci.fCorx.ToString(F4).PadLeft(9) & ",")
            sb.AppendLine(tci.fCory.ToString(F4).PadLeft(9))

            '    strDAT = strDAT & "  �␳�ʒu1X,Y =" & result.fPos1x.ToString(F4).PadLeft(9) & ","
            sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_012)
            sb.Append("  " & Form1_012 & New String(" "c, sp) & tci.fPos1x.ToString(F4).PadLeft(9) & ",")
            sb.Append(tci.fPos1y.ToString(F4).PadLeft(9) & "  ")

            '    strDAT = strDAT & " �����1X,Y   =" & result.fCor1x.ToString(F4).PadLeft(9) & ","
            sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_013)
            sb.Append("  " & Form1_013 & New String(" "c, sp) & tci.fCor1x.ToString(F4).PadLeft(9) & ",")
            sb.AppendLine(tci.fCor1y.ToString(F4).PadLeft(9))

            '    strDAT = strDAT & "  �␳�ʒu2X,Y =" & result.fPos2x.ToString(F4).PadLeft(9) & ","
            sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_014)
            sb.Append("  " & Form1_014 & New String(" "c, sp) & tci.fPos2x.ToString(F4).PadLeft(9) & ",")
            sb.Append(tci.fPos2y.ToString(F4).PadLeft(9) & "  ")

            '    strDAT = strDAT & " �����2X,Y   =" & result.fCor2x.ToString(F4).PadLeft(9) & ","
            sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_015)
            sb.Append("  " & Form1_015 & New String(" "c, sp) & tci.fCor2x.ToString(F4).PadLeft(9) & ",")
            sb.AppendLine(tci.fCor2y.ToString(F4).PadLeft(9))

            If (1 <> reviseMode) Then                       ' �����␳���[�h ? '###038
                '        strDAT = strDAT & "  ��v�xPOS1   =" & result.fCorV1.ToString(F4).PadLeft(9) & ","
                sb.Append("  " & Form1_016 & tci.fCorV1.ToString(F4).PadLeft(9) & ", ")
                '        strDAT = strDAT & " ��v�xPOS2   =" & result.fCorV2.ToString(F4).PadLeft(9) & vbCrLf
                sb.AppendLine(Form1_017 & tci.fCorV2.ToString(F4).PadLeft(9))

                'If (result.fCorV1 < th1) OrElse (result.fCorV2 < th2) Then
                If (False = result.IsMatch) Then
                    'strDAT = strDAT + vbCrLf + MSG_LOADER_07 + Form1_008
                    sb.AppendLine()
                    sb.AppendLine(MSG_LOADER_07 & Form1_008)
                    ret = (cFRS_ERR_PT2)                    ' Return�l = �p�^�[���F���G���[(臒l�G���[)
                End If
            End If

            'V4.7.3.2�@�J�b�g�ʒu�����͗p��
            If gbThetaCorrectionLogOut Then
                Dim strLogMes As String = ""
                ' �ƕ␳�\�����ݒ�(���{��)
                strLogMes = "�Ɗp�x=," & tci.fTheta.ToString("0.0000") & ","
                If (reviseMode = 2) Then                        ' ����+�����̏ꍇ
                    strLogMes = "�Ɗp�x=," & tci.fTheta.ToString("0.0000") & ","
                    strLogMes = strLogMes & "+," & rotateTheta.ToString("0.0000") & ","
                End If
                strLogMes = strLogMes & "�g�����ʒuXY=," & tci.fPosx.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fPosy.ToString("0.0000") & ","
                strLogMes = strLogMes & "�����XY=," & tci.fCorx.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fCory.ToString("0.0000") & ","
                strLogMes = strLogMes & "�␳�ʒu1XY=," & tci.fPos1x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fPos1y.ToString("0.0000") & ","
                strLogMes = strLogMes & "�����1XY=," & tci.fCor1x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fCor1y.ToString("0.0000") & ","
                strLogMes = strLogMes & "�␳�ʒu2XY=," & tci.fPos2x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fPos2y.ToString("0.0000") & ","
                strLogMes = strLogMes & "�����2XY=," & tci.fCor2x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fCor2y.ToString("0.0000")
                If (reviseMode <> 1) Then                       ' �����␳���[�h ? '###038
                    strLogMes = strLogMes & ",��v�xPOS1=," & tci.fCorV1.ToString("0.0000") & ","
                    strLogMes = strLogMes & "��v�xPOS2=," & tci.fCorV2.ToString("0.0000")
                End If
                Call System1.OperationLogging(gSysPrm, strLogMes, "ANALYSIS")
            End If
            'V4.7.3.2�@��

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.GetRecogResult() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            ret = (cERR_TRAP)                               ' �ߒl = �ׯ�ߴװ����
        End Try

        Return ret

    End Function
#End Region

#Region "�N�����v���X�ڕ���␳���ʃ��O�t�@�C����������"
#If CLAMPLESS_TEST Then
    ''' <summary>
    ''' �N�����v���X�ڕ���␳���ʃ��O�t�@�C����������
    ''' </summary>
    ''' <param name="headerWrite"></param>
    ''' <param name="reviseMode"></param>
    ''' <param name="isRough"></param>
    ''' <param name="isMatch"></param>
    ''' <param name="tci"></param>
    ''' <remarks>'V5.0.0.9�J</remarks>
    Private Sub WriteClamplessTestLog(ByVal headerWrite As Boolean, ByVal reviseMode As Integer,
                                      ByVal isRough As Boolean, ByVal isMatch As Boolean,
                                      ByVal tci As VideoLibrary.Theta_Cor_Info)
        Const F4 As String = "F4"

        Dim head As New List(Of String)
        If (headerWrite) Then
            head.Add("����")
            head.Add("�Ɗp�x")
            head.Add("�g�����ʒuX")
            head.Add("�g�����ʒuY")
            head.Add("�����X")
            head.Add("�����Y")
            head.Add("�␳�ʒu1X")
            head.Add("�␳�ʒu1Y")
            head.Add("�����1X")
            head.Add("�����1Y")
            head.Add("�␳�ʒu2X")
            head.Add("�␳�ʒu2Y")
            head.Add("�����2X")
            head.Add("�����2Y")
            If (1 <> reviseMode) Then
                head.Add("��v�xPOS1")
                head.Add("��v�xPOS2")
                head.Add("����")
            End If
        End If

        Dim data As New List(Of String)
        If (False = isRough) Then
            data.Add("�����")
        Else
            data.Add("���t")
        End If
        data.Add(tci.fTheta.ToString(F4))       ' �Ɗp�x
        data.Add(tci.fPosx.ToString(F4))        ' �g�����ʒuX
        data.Add(tci.fPosy.ToString(F4))        ' �g�����ʒuY
        data.Add(tci.fCorx.ToString(F4))        ' �����X
        data.Add(tci.fCory.ToString(F4))        ' �����Y
        data.Add(tci.fPos1x.ToString(F4))       ' �␳�ʒu1X
        data.Add(tci.fPos1y.ToString(F4))       ' �␳�ʒu1Y
        data.Add(tci.fCor1x.ToString(F4))       ' �����1X
        data.Add(tci.fCor1y.ToString(F4))       ' �����1Y
        data.Add(tci.fPos2x.ToString(F4))       ' �␳�ʒu2X
        data.Add(tci.fPos2y.ToString(F4))       ' �␳�ʒu2Y
        data.Add(tci.fCor2x.ToString(F4))       ' �����2X
        data.Add(tci.fCor2y.ToString(F4))       ' �����2Y
        If (1 <> reviseMode) Then
            data.Add(tci.fCorV1.ToString(F4))   ' ��v�xPOS1
            data.Add(tci.fCorV2.ToString(F4))   ' ��v�xPOS2

            If (False = isMatch) Then
                data.Add(MSG_LOADER_07 & Form1_008)
            Else
                data.Add("OK")
            End If
        End If

        Using fs As New System.IO.FileStream("C:\TRIMDATA\LOG\ClamplessTest.csv",
                                             System.IO.FileMode.Append,
                                             System.IO.FileAccess.Write,
                                             System.IO.FileShare.Read)
            Using sw As New System.IO.StreamWriter(fs, Encoding.UTF8)

                If (headerWrite) Then
                    Dim h As String = String.Join(",", head)
                    sw.WriteLine()
                    sw.WriteLine(h)
                End If

                Dim d As String = String.Join(",", data)
                sw.WriteLine(d)
            End Using
        End Using

    End Sub
#End If
#End Region

#Region "�ƕ␳���̂��߂̃p�^�[���o�^����"
    '''=========================================================================
    ''' <summary>�ƕ␳���̂��߂̃p�^�[���o�^����</summary>
    ''' <param name="AppMode">(INP)�A�v�����[�h
    '''                        APP_MODE_RECOG         = �ƕ␳�ׂ̈̃p�^�[���o�^���[�h(TKY�p)
    '''                        APP_MODE_CARIB_REC     = �L�����u���[�V�����␳(�O���J����)�ׂ̈̃p�^�[���o�^���[�h(CHIP/NET�p)
    '''                        APP_MODE_CUTREVISE_REC = �J�b�g�␳�o�^(�O���J����)�ׂ̈̃p�^�[���o�^���[�h(CHIP/NET�p)
    '''                        APP_MODE_APROBEREC     = �I�[�g�v���[�u�ׂ̈̃p�^�[���o�^���[�h(TKY�p) V1.13.0.0�B
    ''' </param>
    ''' <param name="pltInfo"> (I/O)�v���[�g�f�[�^</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    '''=========================================================================
    Private Function PatternTeach(ByVal AppMode As Short, ByRef pltInfo As PlateInfo,
                                  Optional ByVal isRough As Boolean = False) As Short   'V5.0.0.9�B

        Dim r As Short
        Dim stPLTI As LaserFront.Trimmer.DllVideo.VideoLibrary.PLATE_Info           ' �v���[�g��� ��Video.OCX�Œ�`
        Dim stPosiRevis As LaserFront.Trimmer.DllVideo.VideoLibrary.CutPosiRevis_Info                   ' �J�b�g�␳�o�^�p�����[�^ ��Video.OCX�Œ�`
        Dim stCalib As LaserFront.Trimmer.DllVideo.VideoLibrary.Calib_Info                              ' �L�����u���[�V�����␳�p�����[�^ ��Video.OCX�Œ�`
        Dim strMSG As String
        Dim TblOfsX As Double
        Dim TblOfsY As Double
        Dim parModules As MainModules
        parModules = New MainModules
        Dim videoStcnd As New LaserFront.Trimmer.DllVideo.VideoLibrary.TrimCondInfo()
        Dim i As Integer

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   �r�f�I�n�b�w�p(�ƕ␳�̂��߂̃p�^�[���o�^����)�p�����[�^��ݒ肷��
                '--------------------------------------------------------------------------
                PatternTeach = cFRS_NORMAL                              ' Return�l = ����
                Call BSIZE(.dblBlockSizeXDir, .dblBlockSizeYDir)
                'Call InitThetaCorrection()                              ' �p�^�[���o�^(RECOG)�R���g���[�������l�ݒ�
                Call InitThetaCorrection(isRough)                         ' �p�^�[���o�^(RECOG)�R���g���[�������l�ݒ�  'V5.0.0.9�B
                '----- V1.15.0.0�B�� -----
                ' Z��ҋ@�ʒu�ֈړ�����
                r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?
                    Return (r)
                End If
                '----- V1.15.0.0�B�� -----

                '--------------------------------------------------------------------------
                '   �ƕ␳(��߼��) & XY�e�[�u�� & Z���g�����ʒu�ֈړ�����
                '--------------------------------------------------------------------------
                ' �L�����u���[�V�������͊���W1�ƃe�[�u���I�t�Z�b�g�l�����Z���Ĉړ����� 
                If (AppMode = APP_MODE_CARIB_REC) Then
                    '----- V1.20.0.1�A�� -----
                    'TblOfsX = pltInfo.dblCaribBaseCordnt1XDir + pltInfo.dblCaribTableOffsetXDir
                    'TblOfsY = pltInfo.dblCaribBaseCordnt1YDir + pltInfo.dblCaribTableOffsetYDir
                    TblOfsX = 0.0
                    TblOfsY = 0.0
                    '----- V1.20.0.1�A�� -----
                    '----- V1.13.0.0�D�� -----
                ElseIf (AppMode = APP_MODE_SINSYUKU) Then
                    TblOfsX = pltInfo.dblContExpPosX
                    TblOfsY = pltInfo.dblContExpPosY
                    '----- V1.13.0.0�D�� -----
                Else
                    TblOfsX = 0.0
                    TblOfsY = 0.0
                End If

                ' �ƕ␳(��߼��) & XY�e�[�u�� & Z���g�����ʒu�ֈړ�����
                '----- V1.13.0.0�D�� -----
                If (AppMode <> APP_MODE_RECOG) Then                     ' �ƕ␳�ׂ̈̃p�^�[���o�^���[�h�ȊO ?
                    'r = Move_Trimposition(pltInfo, 0.0, 0.0)            ' �ƕ␳(��߼��) & XYð�����шʒu�ړ�
                    r = Move_Trimposition(pltInfo, TblOfsX, TblOfsY)    ' �ƕ␳(��߼��) & XYð�����шʒu�ړ�
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                        Return (r)
                    End If
                End If
                '----- V1.13.0.0�D�� -----

                '--------------------------------------------------------------------------
                '   �p�^�[���o�^����
                '--------------------------------------------------------------------------
                If (KEY_TYPE_RS <> gKeiTyp) Then        'V4.10.0.0�K ������ǉ�
                    SetVisiblePropForVideo(False)                           ' �摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[�����\���ɐݒ肷��
                End If
                VideoLibrary1.SetMainObject(parModules)                 ' �e���W���[���̃��\�b�h��ݒ肷��B

                Select Case (AppMode)
                    Case APP_MODE_CUTREVISE_REC                         ' �J�b�g�␳�o�^(�O���J����)�ׂ̈̃p�^�[���o�^���[�h
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' �v���[�g���p�����[�^�ݒ�
                        Call SetPosiRevisForVideo(pltInfo, stPosiRevis) ' �J�b�g�␳�o�^�p�����[�^�ݒ�
                        videoStcnd.Initialize()
                        For i = 0 To MAX_BANK_NUM - 1
                            videoStcnd.Curr(i) = stCND.Curr(i)
                            videoStcnd.Freq(i) = stCND.Freq(i)
                            videoStcnd.Steg(i) = stCND.Steg(i)
                        Next i
                        VideoLibrary1.SetCrossLineObject(gparModules) 'V5.0.0.6�K
                        r = VideoLibrary1.SetFLCond(videoStcnd)

                        r = VideoLibrary1.PatternRegist_CutPosiRevis(stPLTI, stPosiRevis)
                        'V5.0.0.6�L��
                        pltInfo.dblCutPosiReviseOffsetXDir = stPosiRevis.dblCutPosiReviseOffsetXDir        ' ��Ĉʒu�␳ð��ٵ̾��X
                        pltInfo.dblCutPosiReviseOffsetYDir = stPosiRevis.dblCutPosiReviseOffsetYDir        ' ��Ĉʒu�␳ð��ٵ̾��Y
                        Call gparModules.CrossLineDispOff()
                        'V5.0.0.6�L��
                    Case APP_MODE_CARIB_REC                             ' �L�����u���[�V�����␳(�O���J����)�ׂ̈̃p�^�[���o�^���[�h
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' �v���[�g���p�����[�^�ݒ�
                        Call SetCalibInfoForVideo(pltInfo, stCalib)     ' �L�����u���[�V�����␳�p�����[�^�ݒ�
                        videoStcnd.Initialize()
                        For i = 0 To MAX_BANK_NUM - 1
                            videoStcnd.Curr(i) = stCND.Curr(i)
                            videoStcnd.Freq(i) = stCND.Freq(i)
                            videoStcnd.Steg(i) = stCND.Steg(i)
                        Next i
                        VideoLibrary1.SetCrossLineObject(gparModules) 'V5.0.0.6�K
                        r = VideoLibrary1.SetFLCond(videoStcnd)
                        r = VideoLibrary1.PatternRegist_Calibration(stPLTI, stCalib)
                        Call gparModules.CrossLineDispOff()

                        '----- V1.13.0.0�B�� -----
                    Case APP_MODE_APROBEREC                             ' �I�[�g�v���[�u�ׂ̈̃p�^�[���o�^���[�h
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' �v���[�g���p�����[�^�ݒ�
                        r = VideoLibrary1.PatternRegist_AutoProb(stPLTI)
                        '----- V1.13.0.0�B�� -----

                        '----- V1.13.0.0�D�� -----
                    Case APP_MODE_SINSYUKU                              ' �L�k�␳�p�o�^���[�h 
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' �v���[�g���p�����[�^�ݒ�
                        r = VideoLibrary1.PatternRegist_ShinSyukuHosei(stPLTI)
                        '----- V1.13.0.0�D�� -----

                    Case Else                                           ' �ƕ␳�ׂ̈̃p�^�[���o�^���[�h
                        'r = VideoLibrary1.PatternRegist()               ' �p�^�[���o�^(XYð�����шʒu�ړ����s��)
                        r = VideoLibrary1.PatternRegist(isRough)          ' �p�^�[���o�^(XYð�����шʒu�ړ����s��)     'V5.0.0.9�F

                End Select
                If (KEY_TYPE_RS <> gKeiTyp) Then        'V4.10.0.0�K ������ǉ�
                    SetVisiblePropForVideo(True)                            ' �摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[����\���ɐݒ肷��
                End If

                ' Video.OCX����̖߂�l�𔻒肷��
                If (r >= cFRS_NORMAL) Then                              ' ����I�� ?
                    If (r <> cFRS_ERR_RST) Then                         ' Cancel�ȊO ?
                        ' �g���~���O�f�[�^(�v���[�g�f�[�^)���X�V����
                        Select Case (AppMode)
                            Case APP_MODE_CUTREVISE_REC                 ' �J�b�g�␳�o�^(�O���J����)�ׂ̈̃p�^�[���o�^���[�h
                                r = VideoLibrary1.GetCutPosiRevisResult(stPosiRevis)                    ' ���ʂ��擾����
                                .dblCutPosiReviseOffsetXDir = stPosiRevis.dblCutPosiReviseOffsetXDir    ' ��ē_�␳ð��ٵ̾��X
                                .dblCutPosiReviseOffsetYDir = stPosiRevis.dblCutPosiReviseOffsetYDir    ' ��ē_�␳ð��ٵ̾��Y
                                .intCutPosiReviseGroupNo = stPosiRevis.intCutPosiReviseGroupNo          ' ��ٰ��No
                                .intCutPosiRevisePtnNo = stPosiRevis.intCutPosiRevisePtnNo              ' ��ē_�␳����ݓo�^No
                                '.xxx = stPosiRevis.intCondNo                                           ' ���H�����ԍ�(FL��)

                            Case APP_MODE_CARIB_REC                     ' �L�����u���[�V�����␳(�O���J����)�ׂ̈̃p�^�[���o�^���[�h
                                r = VideoLibrary1.GetCalibrationResult(stCalib)                         ' ���ʂ��擾����
                                '----- V1.20.0.1�A�� -----
                                '----- V1.14.0.0�F�� -----
                                'Select Case gSysPrm.stDEV.giBpDirXy
                                '    Case 0 ' x��, y��
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir      ' �����ڰ���ð��ٵ̾��X
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir      ' �����ڰ���ð��ٵ̾��Y
                                '    Case 1 ' x��, y��(X���])
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir * -1 ' �����ڰ���ð��ٵ̾��X
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir      ' �����ڰ���ð��ٵ̾��Y
                                '    Case 2 ' x��, y��(Y���])
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir      ' �����ڰ���ð��ٵ̾��X
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir * -1 ' �����ڰ���ð��ٵ̾��Y
                                '    Case 3 ' x��, y��(XY���])
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir * -1 ' �����ڰ���ð��ٵ̾��X
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir * -1 ' �����ڰ���ð��ٵ̾��Y
                                'End Select
                                .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir              ' �����ڰ���ð��ٵ̾��X
                                .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir              ' �����ڰ���ð��ٵ̾��Y
                                '----- V1.14.0.0�F�� -----
                                '----- V1.20.0.1�A�� -----
                                .intCaribPtnNo1GroupNo = stCalib.intCaribPtnNo1GroupNo                  ' �����ڰ�������ݓo�^No1�O���[�vNo
                                .intCaribPtnNo2GroupNo = stCalib.intCaribPtnNo2GroupNo                  ' �����ڰ�������ݓo�^No2�O���[�vNo
                                .intCaribPtnNo1 = stCalib.intCaribPtnNo1                                ' �����ڰ�������ݓo�^No1
                                .intCaribPtnNo2 = stCalib.intCaribPtnNo2                                ' �����ڰ�������ݓo�^No2
                                '.xxx = stCalib.intCondNo                                               ' ���H�����ԍ�(FL��)

                                '----- V1.13.0.0�B�� -----
                            Case APP_MODE_APROBEREC                     ' �I�[�g�v���[�u�ׂ̈̃p�^�[���o�^���[�h
                                r = VideoLibrary1.GetAutoProbResult(stPLTI)                             ' ���ʂ��擾����
                                .dblStepMeasBpPosX = stPLTI.dblAProbeBpPosX                             ' �p�^�[��1(����)�pBP�ʒuX
                                .dblStepMeasBpPosY = stPLTI.dblAProbeBpPosY                             ' �p�^�[��1(����)�pBP�ʒuY
                                .dblStepMeasTblOstX = stPLTI.dblAProbeStgPosX                           ' �p�^�[��2(���)�p�X�e�[�W�ʒuX
                                .dblStepMeasTblOstY = stPLTI.dblAProbeStgPosY                           ' �p�^�[��2(���)�p�X�e�[�W�ʒuY
                                '----- V1.13.0.0�B�� -----

                                '----- V1.13.0.0�D�� -----
                            Case APP_MODE_SINSYUKU                      ' �L�k�␳�p�o�^���[�h 
                                r = VideoLibrary1.GetShinsyukuResult(stPLTI)
                                .dblContExpPosX = stPLTI.dblShinsyukuPosX
                                .dblContExpPosY = stPLTI.dblShinsyukuPosY

                                '----- V1.13.0.0�D�� -----

                            Case Else                                   ' �ƕ␳�ׂ̈̃p�^�[���o�^���[�h
                                If (False = isRough) Then    'V5.0.0.9�C
                                    .dblReviseOffsetXDir = VideoLibrary1.pp34_x                             ' �␳�|�W�V�����I�t�Z�b�gX�X�V
                                    .dblReviseOffsetYDir = VideoLibrary1.pp34_y                             ' �␳�|�W�V�����I�t�Z�b�gY�X�V
                                    '----- ###234�� -----
                                    ' �V�X�p��(TKY.ini��OPT_VIDEO��TABLE_POS_UPDATE)�Łu�e�[�u��1,2���W���X�V����(VIDEO.OCX�p�I�v�V����)�v
                                    '�@�w�莞�́u�g���~���O�f�[�^(�p�^�[���o�^�f�[�^)�̃p�^�[�����W1,2���X�V����
                                    If (giTablePosUpd = 1) Then
                                        .dblReviseCordnt1XDir = VideoLibrary1.pp32_x                        ' �p�^�[��1���Wx
                                        .dblReviseCordnt1YDir = VideoLibrary1.pp32_y                        ' �p�^�[��1���Wy
                                        .dblReviseCordnt2XDir = VideoLibrary1.PP33X                         ' �p�^�[��2���Wx
                                        .dblReviseCordnt2YDir = VideoLibrary1.PP33Y                         ' �p�^�[��2���Wy
                                    End If
                                    '----- ###234�� -----
                                Else
                                    ' �N�����v���X�ڕ���E���t�A���C�����g�p�摜�o�^   'V5.0.0.9�C      ��
                                    .dblReviseOffsetXDirRgh = VideoLibrary1.pp34_x              ' �␳�|�W�V�����I�t�Z�b�gX�X�V
                                    .dblReviseOffsetYDirRgh = VideoLibrary1.pp34_y              ' �␳�|�W�V�����I�t�Z�b�gY�X�V
                                    ' �V�X�p��(TKY.ini��OPT_VIDEO��TABLE_POS_UPDATE)�Łu�e�[�u��1,2���W���X�V����(VIDEO.OCX�p�I�v�V����)�v
                                    '�@�w�莞�́u�g���~���O�f�[�^(�p�^�[���o�^�f�[�^)�̃p�^�[�����W1,2���X�V����
                                    If (giTablePosUpd = 1) Then
                                        .dblReviseCordnt1XDirRgh = VideoLibrary1.pp32_x         ' �p�^�[��1���Wx
                                        .dblReviseCordnt1YDirRgh = VideoLibrary1.pp32_y         ' �p�^�[��1���Wy
                                        .dblReviseCordnt2XDirRgh = VideoLibrary1.PP33X          ' �p�^�[��2���Wx
                                        .dblReviseCordnt2YDirRgh = VideoLibrary1.PP33Y          ' �p�^�[��2���Wy
                                    End If
                                    '                                               V5.0.0.9�C       ��
                                End If
                        End Select
                        gCmpTrimDataFlg = 1                             ' �f�[�^�X�V�t���O = 1(�X�V����)
                    End If

                    ' Video.OCX�G���[ ?
                ElseIf (r >= cFRS_MVC_10) And (r <= cFRS_VIDEO_INI) Then
                    Select Case r
                        Case cFRS_VIDEO_INI
                            strMSG = "VIDEOLIB: Not initialized."       ' "�������s���Ă��܂���B
                        Case cFRS_VIDEO_FRM
                            strMSG = "VIDEOLIB: Form Display Now"       ' �t�H�[���̕\�����ł��B
                        Case cFRS_VIDEO_PRP
                            strMSG = "VIDEOLIB: Invalid property value." ' �v���p�e�B�l���s���ł�
                        Case cFRS_VIDEO_UXP
                            strMSG = "VIDEOLIB: Unexpected error"       ' �\�����ʃG���[
                        Case Else
                            strMSG = "VIDEOLIB: Unexpected error 2"
                    End Select
                    Call System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, gAppName)

                Else                                                    ' ����~���G���[
                    PatternTeach = r                                    ' Return�l�ݒ�
                    Exit Function
                End If
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.PatternTeach() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            PatternTeach = cERR_TRAP                                    ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "�v���[�g���p�����[�^�ݒ菈��(�p�^�[���o�^�p)"
    '''=========================================================================
    ''' <summary>�v���[�g���p�����[�^�ݒ菈��(�p�^�[���o�^�p)</summary>
    ''' <param name="pltInfo">(INP)�v���[�g�f�[�^</param>
    ''' <param name="stPLTI"> (OUT)�v���[�g���</param>
    '''=========================================================================
    Private Sub SetPlateInfoForVideo(ByRef pltInfo As PlateInfo, ByRef stPLTI As LaserFront.Trimmer.DllVideo.VideoLibrary.PLATE_Info)

        Dim strMSG As String                                            ' ���b�Z�[�W�ҏW��

        Try
            With pltInfo
                stPLTI.intResistDir = .intResistDir                             ' ��R���ѕ���
                stPLTI.intResistCntInGroup = .intResistCntInBlock               ' �u���b�N����R��
                stPLTI.intBlockCntXDir = .intBlockCntXDir                       ' �u���b�N���w
                stPLTI.intBlockCntYDir = .intBlockCntYDir                       ' �u���b�N���x
                stPLTI.intGroupCntInBlockXBp = .intGroupCntInBlockXBp           ' �u���b�N��BP�O���[�v��(X�����j
                stPLTI.intGroupCntInBlockYStage = .intGroupCntInBlockYStage     ' �u���b�N��Stage�O���[�v��(Y�����j
                stPLTI.dblChipSizeXDir = .dblChipSizeXDir                       ' �`�b�v�T�C�YX
                stPLTI.dblChipSizeYDir = .dblChipSizeYDir                       ' �`�b�v�T�C�YY
                stPLTI.dblBpGrpItv = .dblBpGrpItv                               ' BP�O���[�v�Ԋu
                stPLTI.dblblStgGrpItv = .dblStgGrpItvY                          ' Stage�O���[�v�Ԋu
                stPLTI.dbldispXPos = FORM_X + VideoLibrary1.Location.X          ' �摜�\���v���O�����̕\���ʒuX
                stPLTI.dbldispYPos = FORM_Y + VideoLibrary1.Location.Y          ' �摜�\���v���O�����̕\���ʒuY

                stPLTI.dblBlockSizeXDir = .dblBlockSizeXDir                     ' �u���b�N�T�C�YX                   '###120
                stPLTI.dblBlockSizeYDir = .dblBlockSizeYDir                     ' �u���b�N�T�C�YY                   '###120
                stPLTI.dblbpoffX = .dblBpOffSetXDir                             ' Bp-OffsetX                        '###120
                stPLTI.dblbpoffY = .dblBpOffSetYDir                             ' Bp-OffsetyY                       '###120
                '----- V1.13.0.0�B�� -----
                ' �I�[�g�v���[�u�p
                stPLTI.CorrectTrimPosX = gfCorrectPosX                          ' �g�����|�W�V�����␳�lX
                stPLTI.CorrectTrimPosY = gfCorrectPosY                          ' �g�����|�W�V�����␳�lY
                stPLTI.intAProbeGroupNo1 = .intStepMeasLwGrpNo                  ' �p�^�[��1(����)�p�O���[�v�ԍ�
                stPLTI.intAProbePtnNo1 = .intStepMeasLwPtnNo                    ' �p�^�[��1(����)�p�p�^�[���ԍ�
                stPLTI.intAProbeGroupNo2 = .intStepMeasUpGrpNo                  ' �p�^�[��2(���)�p�O���[�v�ԍ�
                stPLTI.intAProbePtnNo2 = .intStepMeasUpPtnNo                    ' �p�^�[��2(���)�p�p�^�[���ԍ�
                stPLTI.dblAProbeBpPosX = .dblStepMeasBpPosX                     ' �p�^�[��1(����)�pBP�ʒuX
                stPLTI.dblAProbeBpPosY = .dblStepMeasBpPosY                     ' �p�^�[��1(����)�pBP�ʒuY
                stPLTI.dblAProbeStgPosX = .dblStepMeasTblOstX                   ' �p�^�[��2(���)�p�X�e�[�W�ʒuX
                stPLTI.dblAProbeStgPosY = .dblStepMeasTblOstY                   ' �p�^�[��2(���)�p�X�e�[�W�ʒuY
                stPLTI.intAProbeStepCount = .intStepMeasCnt                     ' �X�e�b�v���s�p�X�e�b�v��
                stPLTI.intAProbeStepCount2 = .intStepMeasReptCnt                ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v��
                stPLTI.dblAProbePitch = .dblStepMeasPitch                       ' �X�e�b�v���s�p�X�e�b�v�s�b�`
                stPLTI.dblAProbePitch2 = .dblStepMeasReptPitch                  ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v�s�b�`
                stPLTI.dblZ2OnPos = .dblLwPrbStpUpDist                          ' Z2 On�ʒu
                stPLTI.dblZ2OffPos = .dblLwPrbStpDwDist                         ' Z2 Off�ʒu
                '----- V1.13.0.0�B -----
                'V1.13.0.0�D
                stPLTI.dblShinsyukuPosX = .dblContExpPosX
                stPLTI.dblShinsyukuPosY = .dblContExpPosY
                'V1.13.0.0�D

            End With
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetPlateInfoForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�J�b�g�␳�o�^(�O���J����)�ׂ̈̃p�^�[���o�^�p�����[�^�ݒ菈��(�p�^�[���o�^�p)"
    '''=========================================================================
    ''' <summary>�J�b�g�␳�o�^(�O���J����)�ׂ̈̃p�^�[���o�^�p�����[�^�ݒ菈��(�p�^�[���o�^�p)</summary>
    ''' <param name="pltInfo">    (INP)�v���[�g�f�[�^</param>
    ''' <param name="stPosiRevis">(OUT)�J�b�g�␳�o�^(�O���J����)�ׂ̈̃p�^�[���o�^�p�����[�^</param>
    '''=========================================================================
    Private Sub SetPosiRevisForVideo(ByRef pltInfo As PlateInfo, ByRef stPosiRevis As LaserFront.Trimmer.DllVideo.VideoLibrary.CutPosiRevis_Info)

        Dim strMSG As String                                            ' ���b�Z�[�W�ҏW��

        Try
            With pltInfo
                stPosiRevis.dblCutPosiReviseOffsetXDir = .dblCutPosiReviseOffsetXDir        ' ��Ĉʒu�␳ð��ٵ̾��X
                stPosiRevis.dblCutPosiReviseOffsetYDir = .dblCutPosiReviseOffsetYDir        ' ��Ĉʒu�␳ð��ٵ̾��Y
                stPosiRevis.dblCutPosiReviseCutLength = .dblCutPosiReviseCutLength          ' ��Ĉʒu�␳��Ē�
                stPosiRevis.dblCutPosiReviseCutSpeed = .dblCutPosiReviseCutSpeed            ' ��Ĉʒu�␳��đ��x
                stPosiRevis.dblCutPosiReviseCutQRate = .dblCutPosiReviseCutQRate            ' ��Ĉʒu�␳ڰ��Qڰ�
                stPosiRevis.dblCutBpPosX = typResistorInfoArray(1).ArrCut(1).dblStartPointX ' �ŏ��̃J�b�g�ʒuX
                stPosiRevis.dblCutBpPosY = typResistorInfoArray(1).ArrCut(1).dblStartPointY ' �ŏ��̃J�b�g�ʒuY
                stPosiRevis.intCutPosiReviseGroupNo = .intCutPosiReviseGroupNo              ' ��Ĉʒu�␳��ٰ��No
                stPosiRevis.intCutPosiRevisePtnNo = .intCutPosiRevisePtnNo                  ' ��Ĉʒu�␳����ݓo�^No
                stPosiRevis.intCondNo = .intCutPosiReviseCondNo                             ' ��Ĉʒu�␳���H�����ԍ�(FL��)
            End With
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetPosiRevisForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�L�����u���[�V�����␳(�O���J����)�ׂ̈̃p�^�[���o�^�p�����[�^�ݒ菈��(�p�^�[���o�^�p)"
    '''=========================================================================
    ''' <summary>�L�����u���[�V�����␳(�O���J����)�ׂ̈̃p�^�[���o�^�p�����[�^�ݒ菈��(�p�^�[���o�^�p)</summary>
    ''' <param name="pltInfo">(INP)�v���[�g�f�[�^</param>
    ''' <param name="stCalib">(OUT)�L�����u���[�V�����␳(�O���J����)�ׂ̈̃p�^�[���o�^�p�����[�^</param>
    '''=========================================================================
    Private Sub SetCalibInfoForVideo(ByRef pltInfo As PlateInfo, ByRef stCalib As LaserFront.Trimmer.DllVideo.VideoLibrary.Calib_Info)

        Dim strMSG As String                                            ' ���b�Z�[�W�ҏW��

        Try
            With pltInfo
                stCalib.dblCaribBaseCordnt1XDir = .dblCaribBaseCordnt1XDir  ' �����ڰ��݊���W1X
                stCalib.dblCaribBaseCordnt1YDir = .dblCaribBaseCordnt1YDir  ' �����ڰ��݊���W1Y
                stCalib.dblCaribBaseCordnt2XDir = .dblCaribBaseCordnt2XDir  ' �����ڰ��݊���W2X
                stCalib.dblCaribBaseCordnt2YDir = .dblCaribBaseCordnt2YDir  ' �����ڰ��݊���W2Y
                '----- V1.20.0.1�A�� -----
                ' V1.14.0.0�F�̏C���͍폜
                '----- V1.14.0.0�F�� -----
                'Select Case gSysPrm.stDEV.giBpDirXy
                '    Case 0 ' x��, y��
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir      ' �����ڰ���ð��ٵ̾��X
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir      ' �����ڰ���ð��ٵ̾��Y
                '    Case 1 ' x��, y��(X���])
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir * -1 ' �����ڰ���ð��ٵ̾��X
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir      ' �����ڰ���ð��ٵ̾��Y
                '    Case 2 ' x��, y��(Y���])
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir      ' �����ڰ���ð��ٵ̾��X
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir * -1 ' �����ڰ���ð��ٵ̾��Y
                '    Case 3 ' x��, y��(XY���])
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir * -1 ' �����ڰ���ð��ٵ̾��X
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir * -1 ' �����ڰ���ð��ٵ̾��Y
                'End Select
                stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir  ' �����ڰ���ð��ٵ̾��X
                stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir  ' �����ڰ���ð��ٵ̾��Y
                '----- V1.14.0.0�F�� -----
                '----- V1.20.0.1�A�� -----
                stCalib.dblCaribCutLength = .dblCaribCutLength              ' �����ڰ��ݶ�Ē�
                stCalib.dblCaribCutSpeed = .dblCaribCutSpeed                ' �����ڰ��ݶ�đ��x
                stCalib.dblCaribCutQRate = .dblCaribCutQRate                ' �����ڰ���ڰ��Qڰ�
                stCalib.intCaribPtnNo1GroupNo = .intCaribPtnNo1GroupNo      ' �����ڰ�������ݓo�^No1�O���[�vNo
                stCalib.intCaribPtnNo2GroupNo = .intCaribPtnNo2GroupNo      ' �����ڰ�������ݓo�^No2�O���[�vNo
                stCalib.intCaribPtnNo1 = .intCaribPtnNo1                    ' �����ڰ�������ݓo�^No1
                stCalib.intCaribPtnNo2 = .intCaribPtnNo2                    ' �����ڰ�������ݓo�^No2
                stCalib.intCondNo = .intCaribCutCondNo                      ' �����ڰ��݉��H�����ԍ�(FL��)
            End With
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetCalibInfoForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�J�b�g�ʒu�␳(F10)���݉����������yTKY�p�z"
    '''=========================================================================
    '''<summary>�J�b�g�ʒu�␳(F10)���݉���������</summary>
    '''<remarks>�J�b�g�ʒu�␳�̂��߂̃p�^�[���o�^�������s��</remarks>
    '''=========================================================================
    Private Sub CmdCutPos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCutPos.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_CUTPOS, APP_MODE_CUTPOS, MSG_OPLOG_FUNC08S, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCutPos_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

STP_END:
    End Sub
#End Region

#Region "�J�b�g�ʒu�␳�̂��߂̃p�^�[���o�^�����yTKY�p�z"
    '''=========================================================================
    '''<summary>�J�b�g�ʒu�␳�̂��߂̃p�^�[���o�^�����yTKY�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function CutPosTeach(ByRef pltInfo As PlateInfo) As Short
        'Private Function CutPosTeach(ByVal pltInfo As PlateInfo) As Short

        Dim r As Short
        Dim strMSG As String

        Dim rn As Short
        Dim idx As Short
        Dim sRName(MaxRegNum) As String                         ' ��R��(��R�ԍ�)�e�[�u��
        Dim StartPosTbl(2, MaxRegNum) As Double                 ' �p�^�[���ʒuX,Y�e�[�u��
        Dim PtnGrpTbl(MaxRegNum) As Short                       ' �O���[�v�ԍ��e�[�u��
        Dim PtnNumTbl(MaxRegNum) As Short                       ' �p�^�[���ԍ��e�[�u��

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   �ƕ␳(��߼��) & XY�e�[�u�����g�����ʒu�ֈړ�����
                '--------------------------------------------------------------------------
                CutPosTeach = cFRS_NORMAL                           ' Return�l = ����
                ChDir(My.Application.Info.DirectoryPath)            ' exe�̂���̫��ް��MV10��DLL���Ȃ��ƃ_�� ?
                '                                                   ' (�f�o�b�O����\i-TKY_VS2005\bin��MV10��DLL���Ȃ��ƃ_��?)
                'Call BSIZE(.dblBlockSizeXDir, .dblBlockSizeYDir) 
                r = Move_Trimposition(pltInfo, 0.0, 0.0)            ' �ƕ␳(��߼��) & XYð�����шʒu�ړ�
                SetTeachVideoSize()                                 ' V6.0.3.0_49
                If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                    CutPosTeach = r                                 ' Return�l�ݒ�
                    Exit Function
                End If

                ' BP�̾�Đݒ�
                'Call System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then GoTo STP_END

                '--------------------------------------------------------------------------
                '   �r�f�I�n�b�w(�J�b�g�ʒu�␳����)�ɓn���e�[�u�����쐬����
                '---------------------------------------------------------------------------
                idx = 0                                             ' �e�[�u���C���f�b�N�X
                For rn = 1 To gRegistorCnt                          ' ��R�����ݒ肷��
                    ' �J�b�g�ʒu�␳�����R ?
                    If (typResistorInfoArray(rn).intCutReviseMode = 1) Then
                        idx = idx + 1                               ' �e�[�u���C���f�b�N�X + 1
                        ' ��R��(��R�ԍ�)�e�[�u��
                        sRName(idx) = "R" + typResistorInfoArray(rn).intResNo.ToString()
                        ' �p�^�[���ʒuX,Y�e�[�u��
                        StartPosTbl(1, idx) = typResistorInfoArray(rn).dblCutRevisePosX
                        StartPosTbl(2, idx) = typResistorInfoArray(rn).dblCutRevisePosY
                        ' �O���[�v�ԍ��e�[�u��
                        PtnGrpTbl(idx) = typResistorInfoArray(rn).intCutReviseGrpNo
                        ' �p�^�[���ԍ��e�[�u��
                        PtnNumTbl(idx) = typResistorInfoArray(rn).intCutRevisePtnNo
                    End If
                Next rn

                If (idx <= 0) Then
                    ' "�J�b�g�ʒu�␳�Ώۂ̒�R������܂���"
                    Call System1.TrmMsgBox(gSysPrm, MSG_39, vbExclamation Or vbOKOnly, gAppName)
                    CutPosTeach = cFRS_ERR_RST                      ' Return�l = Cancel
                    Exit Function
                End If
                ReDim Preserve sRName(idx)                          ' (��)��R�����ōĒ�`���Ȃ���Video.OCX�̃O���b�h�ɔz�񐔕���������Ă��܂�

                '--------------------------------------------------------------------------
                '   �r�f�I�n�b�w�p�p�����[�^��ݒ肷��
                '--------------------------------------------------------------------------
                VideoLibrary1.pp32_x = 0.0#                         ' CorStgPos1X
                VideoLibrary1.pp32_y = 0.0#                         ' CorStgPos1Y
                VideoLibrary1.pp34_x = .dblBpOffSetXDir             ' Bp Offset X
                VideoLibrary1.pp34_y = .dblBpOffSetYDir             ' Bp Offset Y
                VideoLibrary1.frmLeft = Me.Text4.Location.X         ' �\���ʒu(Left)
                VideoLibrary1.frmTop = Me.Text4.Location.Y          ' �\���ʒu(Top)
                VideoLibrary1.pfTrim_x = gSysPrm.stDEV.gfTrimX      ' �g�����ʒux
                VideoLibrary1.pfTrim_y = gSysPrm.stDEV.gfTrimY      ' �g�����ʒuy
                VideoLibrary1.pfStgOffX = .dblTableOffsetXDir       ' Trim Position Offset Y(mm)
                VideoLibrary1.pfStgOffY = .dblTableOffsetYDir       ' Trim Position Offset Y(mm)
                VideoLibrary1.pfBlock_x = .dblBlockSizeXDir         ' Block Size x(mm)
                VideoLibrary1.pfBlock_y = .dblBlockSizeYDir         ' Block Size y(mm)
                VideoLibrary1.zwaitpos = .dblZWaitOffset            ' Z PROBE OFF OFFSET(mm)
                VideoLibrary1.RNASTMPNUM = True                     ' �J�b�g�ʒu�␳���[�h 
                VideoLibrary1.ZON = .dblZOffSet                     ' Z ON�ʒu 
                VideoLibrary1.ZOFF = .dblZWaitOffset                ' Z OFF�ʒu 

                ' �e���v���[�g�O���[�v�I��/�e���v���[�g�ԍ��ݒ�
                Call VideoLibrary1.SelectTemplateGroup(PtnGrpTbl(1))       ' �ŏ��̃O���[�v�ԍ���ݒ�
                r = VideoLibrary1.SetTemplateNum_EX(idx, PtnNumTbl, PtnGrpTbl)

                '--------------------------------------------------------------------------
                '   �J�b�g�ʒu�␳��ʕ\��
                '--------------------------------------------------------------------------
                SetVisiblePropForVideo(False)                       ' �摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[�����\���ɐݒ肷��
                r = VideoLibrary1.CutPosTeach(sRName, StartPosTbl, .dblBlockSizeXDir, .dblBlockSizeYDir,
                                       .dblBpOffSetXDir, .dblBpOffSetYDir)
                SetVisiblePropForVideo(True)                        ' �摜�o�^�p�Ƀ��C���t�H�[���̃R���g���[����\���ɐݒ肷��                'V3.0.0.0�B �L�����Z�����y�т��̑��̃G���[�����f�[�^�X�V���Ă���s��C���ׂ̈h�e���S�̂�����������
                CutPosTeach = r                                             ' Return�l�ݒ� 
                If (r < cFRS_NORMAL) Or (r = cFRS_ERR_RST) Then
                    If (r <= cFRS_VIDEO_INI) Then                               ' Video.OCX�G���[ ?
                        Select Case r
                            Case cFRS_VIDEO_INI
                                strMSG = "VIDEOLIB: Not initialized."           ' ���������s���Ă��܂���B
                            Case cFRS_VIDEO_FRM
                                strMSG = "VIDEOLIB: Form Display Now"           ' �t�H�[���̕\�����ł��B
                            Case cFRS_VIDEO_PRP
                                strMSG = "VIDEOLIB: Invalid property value."    ' �v���p�e�B�l���s���ł�
                            Case cFRS_VIDEO_UXP
                                strMSG = "VIDEOLIB: Unexpected error"           ' �\�����ʃG���[
                            Case Else
                                strMSG = "VIDEOLIB: Unexpected error 2"
                        End Select
                        Call System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, gAppName)
                    End If
                    Exit Function
                Else
                    gCmpTrimDataFlg = 1                         ' �f�[�^�X�V�t���O = 1(�X�V����)
                End If
                'V3.0.0.0�B �L�����Z�����y�т��̑��̃G���[�����f�[�^�X�V���Ă���s��C���̈�IF������������

                '--------------------------------------------------------------------------
                '   �p�^�[���ʒuXY���擾����
                '--------------------------------------------------------------------------
                r = VideoLibrary1.GetResult(StartPosTbl)            ' ���ʎ擾
                If (r = cFRS_NORMAL) Then                           ' ����I�� ?
                    idx = 0                                         ' �e�[�u���C���f�b�N�X
                    For rn = 1 To gRegistorCnt                      ' ��R�����ݒ肷��
                        ' �J�b�g�ʒu�␳�����R ?
                        If (typResistorInfoArray(rn).intCutReviseMode = 1) Then
                            idx = idx + 1                           ' �e�[�u���C���f�b�N�X + 1
                            ' �p�^�[���ʒuX,Y�e�[�u��
                            typResistorInfoArray(rn).dblCutRevisePosX = StartPosTbl(1, idx)
                            typResistorInfoArray(rn).dblCutRevisePosY = StartPosTbl(2, idx)
                        End If
                    Next rn
                    gCmpTrimDataFlg = 1                             ' �f�[�^�X�V�t���O = 1(�X�V����)
                End If
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CutPosTeach() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CutPosTeach = cERR_TRAP                                 ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "TXè��ݸ����݉����������yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>TXè��ݸ����݉����������yCHIP/NET�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTx.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �t���O�����ݒ�
            g_blnTx2Ty2Exec = False                         ' TX2���s�t���O��FALSE�ɂ��Ă���
            g_blnTxTyExec = True                            ' TX���s�t���O��TRUE�ɂ���

            ' �R�}���h���s
            r = CmdExec_Proc(F_TX, APP_MODE_TX, MSG_OPLOG_TX_START, "")
            'If (r = cFRS_NORMAL) Then
            '    ' TX2���s�t���O��TRUE�̏ꍇ�Aè��ݸޏ������Ăяo���B
            '    If (g_blnTx2Ty2Exec) Then                   ' TX��ʏI�����ɁuTX2���݁v��I�����ꂽ ? 
            '        r = Teaching()
            '        If (r <= cFRS_ERR_EMG) Then
            '            Call AppEndDataSave()
            '            Call AplicationForcedEnding()       ' �����I��
            '        End If
            '        g_blnTx2Ty2Exec = False                 ' TX2���s�t���O��FALSE�ɂ���
            '    End If
            '    g_blnTxTyExec = False                       ' TX���s�t���O��FALSE�ɂ���
            'End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTx_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "TYè��ݸ����݉����������yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>TYè��ݸ����݉����������yCHIP/NET�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTy.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_TY, APP_MODE_TY, MSG_OPLOG_TY_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTy_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "TY2���݉���������"
    '''=========================================================================
    '''<summary>TY2���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTy2_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTy2.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_TY2, APP_MODE_TY2, MSG_OPLOG_TY2_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTy2_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "T�����݉���������"
    '''=========================================================================
    '''<summary>T�����݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdT_Theta_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdT_Theta.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_TTHETA, APP_MODE_TTHETA, MSG_OPLOG_T_THETA_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdT_Theta_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "�O�����R1è��ݸ����݉���������(�O���J����)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�O�����R1è��ݸ����݉���������(�O���J����)�yCHIP/NET�p�z</summary>
    '''<remarks>��P��R�̂�
    '''         �O���J�����̂���ꍇ�̂ݗL��</remarks>
    '''=========================================================================
    Private Sub CmdExCam_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdExCam.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_EXR1, APP_MODE_EXCAM_R1TEACH, MSG_OPLOG_ExCam_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExCam_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�O�����è��ݸ����݉���������(�O���J����)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�O�����è��ݸ����݉���������(�O���J����)�yCHIP/NET�p�z</summary>
    '''<remarks>�S��R
    '''         �O���J�����̂���ꍇ�̂ݗL��</remarks>
    '''=========================================================================
    Private Sub CmdExCam1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdExCam1.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_EXTEACH, APP_MODE_EXCAM_TEACH, MSG_OPLOG_ExCam1_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExCam1_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�����ڰ��ݕ␳�o�^���݉���������(�O���J����)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�����ڰ��ݕ␳�o�^���݉���������(�O���J����)�yCHIP/NET�p�z</summary>
    '''<remarks>�O���J�����̂���ꍇ�̂ݗL��</remarks>
    '''=========================================================================
    Private Sub CmdPtnCalibration_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdPtnCalibration.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_CARREC, APP_MODE_CARIB_REC, MSG_OPLOG_CALIBRATION_RECOG_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdPtnCalibration_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try


        '        Dim mFrmObj As System.Windows.Forms.Form
        '        Dim r As Short
        '        Dim strMSG As String

        '        Try

        '            ' ���[�_�[�o��(ON=�g���}���쒆,OFF=�Ȃ�)
        '            Call ZATLDSET(COM_STS_TRM_STATE, 0)
        '            ' ���[�h�̃Z�b�g
        '            giAppMode = APP_MODE_CARIB_REC

        '            ' �R�}���h���s�O��������
        '            'r = CmdExec_Init(giAppMode, MSG_OPLOG_CALIBRATION_RECOG_START)
        '            If (r <> cFRS_NORMAL) Then
        '                GoTo RETURNMENU
        '            End If

        '            ' �L�����u���[�V�����␳�o�^���s
        '            gbInPattern = True
        '            mFrmObj = New frmPtnCalibration
        '            mFrmObj.Show()

        '            'Do
        '            '    System.Windows.Forms.Application.DoEvents()
        '            'Loop While gbInPattern

        '            ' �g���b�v�G���[������ 
        '        Catch ex As Exception
        '            strMSG = "cmdPtnCalibration_Click() TRAP ERROR = " + ex.Message
        '            MsgBox(strMSG)
        '        End Try

        'RETURNMENU:
        '        ' �R�}���h���s�㏈��
        '        'Call CmdExec_Term()

        '        ' IDLE���[�h
        '        Call ZCONRST()                                  ' ���b�`����
        '        Timer1.Enabled = True
        '        giAppMode = APP_MODE_IDLE                       ' ����Ӱ�� = �g���}���u�A�C�h����

        '        ' ���[�_�[�o��(ON=�Ȃ�,OFF=�g���}���쒆)
        '        Call ZATLDSET(0, COM_STS_TRM_STATE)
    End Sub
#End Region

#Region "�����ڰ������݉���������(�O���J����)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�����ڰ������݉���������(�O���J����)�yCHIP/NET�p�z</summary>
    '''<remarks>�O���J�����̂���ꍇ�̂ݗL��</remarks>
    '''=========================================================================
    Private Sub CmdCalibration_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCalibration.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_CAR, APP_MODE_CARIB, MSG_OPLOG_CALIBRATION_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCalibration_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�J�b�g�␳�o�^�{�^������������(�O���J����)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�J�b�g�␳�o�^�{�^������������(�O���J����)�yCHIP/NET�p�z</summary>
    '''<remarks>�O���J�����̂���ꍇ�̂ݗL��</remarks>
    '''=========================================================================
    Private Sub CmdPtnCutPosCorrect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdPtnCutPosCorrect.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_CUTREC, APP_MODE_CUTREVISE_REC, MSG_OPLOG_CUT_POS_CORRECT_RECOG_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdPtnCutPosCorrect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�J�b�g�␳�{�^������������(�O���J����)�yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�J�b�g�ʒu�␳�{�^������������(�O���J����)�yCHIP/NET�p�z</summary>
    '''<remarks>�O���J�����̂���ꍇ�̂ݗL��</remarks>
    '''=========================================================================
    Private Sub CmdCutPosCorrect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCutPosCorrect.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_CUTREV, APP_MODE_CUTREVIDE, MSG_OPLOG_CUT_POS_CORRECT_START, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCutPosCorrect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "END���݉���������"
    '''=========================================================================
    '''<summary>END���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdEnd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdEnd.Click

        Dim strMSG As String
        Dim ret As Integer

        Try
            ' ���[�_�փg���}���쒆�M���𑗐M����
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ? ###035
                Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ���[�_�o��(ON=�g���}���쒆, OFF=�Ȃ�)
            Else
                Call SetLoaderIO(&H0, LOUT_STOP)                        ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}��~��)
            End If
            giAppMode = APP_MODE_EXIT                                   ' ����Ӱ�� = �I��
            Timer1.Enabled = False                                      ' �Ď��^�C�}�[��~
            Call ZCONRST()                                              ' ���b�`����

            ' �I���m�F���b�Z�[�W�\��
            If (gCmpTrimDataFlg = 1) Then
                '  "�ҏW���̃f�[�^������܂��B" "�A�v���P�[�V�������I�����Ă�낵���ł����H"
                ret = MsgBox(MSG_117 + vbCrLf + MSG_102,
                        MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground, gAppName)
            Else
                ' "�A�v���P�[�V�������I�����Ă�낵���ł����H"
                ret = MsgBox(MSG_102,
                        MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground, gAppName)
            End If

            '----- V6.0.3.0_46�� -----
            ' ���v�\���I�u�W�F�N�g
            If (IsNothing(gObjFrmDistribute) = False) Then
                GraphDispSet()
            End If
            '----- V6.0.3.0_46�� -----

            ' �I���L�����Z������
            If (ret = MsgBoxResult.No) Then                             ' �I���L�����Z�� ? 
                'e.Cancel = True                                        ' UserClosing�C�x���g�L�����Z��
                giAppMode = APP_MODE_IDLE                               ' ����Ӱ�� = �g���}���u�A�C�h����

                ' ���[�_�փg���}��~���M���𑗐M����
                If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then  ' SL432R�n ? ###035
                    Call SetLoaderIO(0, COM_STS_TRM_STATE)              ' ���[�_�[�o��(ON=�Ȃ�,OFF=�g���}���쒆)
                Else
                    Call SetLoaderIO(LOUT_STOP, &H0)                    ' ���[�_�o��(ON=�g���}��~��, OFF=�Ȃ�)
                End If

                Timer1.Enabled = True                                   ' �Ď��^�C�}�[�J�n
                Exit Sub
            End If

            ' �\�t�g�I�����̃X�e�[�W�ړ�����(�I�v�V����) V1.13.0.0�I
            ret = AppEndStageMove(gdblStgOrgMoveX, gdblStgOrgMoveY)

            ' �\�t�g�����I������
            Call AplicationForcedEnding()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdEnd_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            End
        End Try

    End Sub
#End Region

#Region "�����è��ݸ����݉����������yNET�̂݁z"
    '''=========================================================================
    '''<summary>�����è��ݸ����݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdCircuitTeach_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCircuitTeach.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_CIRCUIT, APP_MODE_CIRCUIT, MSG_OPLOG_FUNC30, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCircuitTeach_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        '        Dim r As Integer
        '        Dim mFrmObj As System.Windows.Forms.Form

        '        ' ���[�_�[�o��(ON=�g���}���쒆,OFF=�Ȃ�)
        '        Call ZATLDSET(COM_STS_TRM_STATE, 0)
        '        ' �T�[�L�b�g�e�B�[�`���O���[�h�̃Z�b�g
        '        giAppMode = APP_MODE_CIRCUIT

        '        ' �R�}���h���s�O��������
        '        'r = CmdExec_Init(giAppMode, MSG_OPLOG_FUNC30)
        '        If (r <> cFRS_NORMAL) Then
        '            GoTo RETURNMENU
        '        End If

        '        ' �����è��ݸޏ���
        '        mFrmObj = New frmCircuitTeach
        '        mFrmObj.ShowDialog()

        '        'Me.SetFocus()

        'RETURNMENU:
        '        ' �R�}���h���s�㏈��
        '        'Call CmdExec_Term()

        '        '�@IDLE���[�h
        '        Call ZCONRST()                                      ' ���b�`����
        '        Timer1.Enabled = True
        '        giAppMode = APP_MODE_IDLE                           ' ����Ӱ�� = �g���}���u�A�C�h����

        '        ' ���[�_�[�o��(ON=�Ȃ�,OFF=�g���}���쒆)
        '        Call ZATLDSET(0, COM_STS_TRM_STATE)

    End Sub
#End Region

#Region "�����^�]�{�^������������"
    '''=========================================================================
    ''' <summary>�����^�]�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SL436R/SL436S�p
    '''          SL432R(����)--���b�g�ؑւ��@�\�L����
    ''' </remarks>
    '''=========================================================================
    Private Sub CmdAutoOperation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAutoOperation.Click

        Dim r As Integer
        'V6.0.0.0�J        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   �����^�]�O����
            '-------------------------------------------------------------------
            '----- V6.1.4.0�@��(KOA EW�aSL432RD�Ή�) -----
            ' �����^�]���s(���b�g�ؑւ��@�\)
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then   ' SL432R�n ? 
                If (giLotChange = 1) Then                           ' ���b�g�ؑւ��@�\�L�� ?
                    LotChangeProc()                                 ' �����^�]���s(���b�g�ؑւ��@�\)
                End If
                Exit Sub
            End If
            '----- V6.1.4.0�@�� -----

            '���v�\�������̏�ԕύX
            '###112�@Call RedrawDisplayDistribution(True)

            ''V4.0.0.0-86
            r = GetLaserOffIO(True) 'V5.0.0.1�K
            If r = 1 Then
                Exit Sub
            End If
            ''V4.0.0.0-86

            '----- V6.0.3.0_27�� -----
            '----- V6.0.3.0_28�� -----
            '' ���b�Z�[�W�\��(�V�K�FSTART�{�^���A�p���FRESET�{�^�������҂�)
            'r = Sub_CallFrmMsgDisp(System1, cGMODE_QUEST_NEW_CONTINUE, cFRS_ERR_START, True, _
            '        "", "", "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)

            'If r = cFRS_ERR_START Then
            '    ' �V�K��I�������ꍇ 
            '' V6.0.3.0_30��
            'If CbDigSwL.SelectedIndex <= TRIM_MODE_FT Then
            '    If QR_Read_Flg = 0 Then
            '        If btnQRLmit.BackColor = Color.Lime Then
            '            If gSysPrm.stTMN.giMsgTyp = 0 Then
            '                MsgBox("QR�R�[�h��ǂݍ���ł��Ȃ��̂Ŏ����^�]���J�n�ł��܂���B")
            '            Else
            '                MsgBox("Cannot Auto Operation , Because not read Qr-Code. ")
            '            End If
            '            Return
            '        End If
            '    End If
            'End If

            '----- V6.0.3.0�D��(���[���a����) -----
            ' QR�R�[�h��ǂݍ��񂾂��`�F�b�N 
            r = ChkQRLimitStatus()
            If r <> cFRS_NORMAL Then
                Exit Sub
            End If
            ' V6.0.3.0_30��
            '----- V6.0.3.0�D�� -----
            'Else
            '    ' �p����I�������ꍇ
            '    ' �f�[�^���[�h�`�F�b�N
            '    If (gLoadDTFlag = False) Then                            ' �f�[�^�����[�h ?
            '        ' "�g���~���O�p�����[�^�f�[�^�����[�h���邩�V�K�쐬���Ă�������"
            '        Call System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
            '        Return                                    ' �g���}�G���[�M�����M��
            '    End If

            'End If
            '----- V6.0.3.0_28�� -----
            '----- V6.0.3.0_27�� -----

            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �����^�]���̓g���~���O�J�n�u���b�N�ԍ�XY��1,1�Ƃ���
            Call Set_StartBlkComb1St()
            Call Set_StartBlkNum_Enabled(False)
            '----- V4.11.0.0�D�� -----
            '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
            ' ������{�^���̔w�i�F���D�F�ɂ���
            gbChkSubstrateSet = False                                   ' ������t���OOFF
            BtnSubstrateSet.Enabled = False                             '�u������{�^���v�񊈐���
            Call Set_SubstrateSetBtn(gbChkSubstrateSet)
            '----- V4.11.0.0�E�� -----

            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            Call Form1Button(0)                                         ' �R�}���h�{�^���𖳌��ɂ��� ###236
            r = TrimStateOn(F_AUTO, APP_MODE_AUTO, MSG_OPLOG_AUTO, "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' �ҏW���̃f�[�^��ۑ�����
            If (gCmpTrimDataFlg = 1) Then                               ' �f�[�^�X�V�t���O = 1(�X�V����)
                '"�ҏW���̃f�[�^��ۑ����܂����H"
                r = MsgBox(MSG_AUTO_19, MsgBoxStyle.OkCancel)
                If (r = MsgBoxResult.Ok) Then
                    ' �f�[�^�ۑ�
                    Call Me.CmdSave_Click(Me.CmdSave, New System.EventArgs())
                    giAppMode = APP_MODE_AUTO                           ' APP Mode�������^�]���[�h�ɖ߂�(Save����A���0�ɂȂ邽��) ###201
                    gCmpTrimDataFlg = 0                                 ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�)
                End If
            End If

            '----- ###184�� -----
            ' �ڕ���Ɋ���Ȃ������`�F�b�N����
            ' �����̏C����START�L�[�����҂�(Sub_CallFrmRset())�̌�ɓ����Ɖ��̂����[�_��
            '   �����}�K�W���̏㏸��҂����Ɉړ�����(�f�o�b�O�̂��߂�BREAK���Ă�����)
            r = Sub_SubstrateCheck(System1, APP_MODE_AUTO)
            If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            ElseIf (r = cFRS_ERR_RST) Then                              ' ��L(RESET�L�[����)�Ȃ�Return
                GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            End If

            ' �}�K�W������o�Z���T��OFF�ł��鎖���`�F�b�N����
            r = Sub_SubstrateSensorOffCheck(System1)
            If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            ElseIf (r = cFRS_ERR_RST) Then                              ' �Z���T��ON(RESET�L�[����)�Ȃ�Return
                GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            End If
            '----- ###184�� -----

            '----- ###197�� ----- 
            ' NG�r�oBOX�����t�łȂ������`�F�b�N����
            r = Sub_NgBoxCheck(System1, APP_MODE_AUTO)
            If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            ElseIf (r = cFRS_ERR_RST) Then                              ' ��L(RESET�L�[����)�Ȃ�Return
                GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            End If
            '----- ###197�� -----

            '-------------------------------------------------------------------
            '   �����^�]�̂��߂̃f�[�^�I����ʂ�\������
            '-------------------------------------------------------------------
            If (gsQRInfo(0) = "") Then                                  ' V1.18.0.0�A 
                'V6.0.0.0�J                objForm = New FormDataSelect()                          ' �I�u�W�F�N�g����
                Dim objForm As New FormDataSelect()                     ' �I�u�W�F�N�g����
                Call objForm.ShowDialog(Me)                             ' �f�[�^�I����ʕ\�� 'V6.0.0.0�J
                r = objForm.sGetReturn()                                ' Return�l�擾(�A���^�]����Ӱ��(0:϶޼��Ӱ�� 1:ۯ�Ӱ�� 2:����ڽӰ��))
                ' �I�u�W�F�N�g�J��
                If (objForm Is Nothing = False) Then
                    Call objForm.Close()                                ' �I�u�W�F�N�g�J��
                    Call objForm.Dispose()                              ' ���\�[�X�J��
                End If
                Me.Refresh()                                            ' �ĕ`�悵�Ȃ��Ǝ���START�L�[�����҂��ł���ʕ\�����c�� 
                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' �f�[�^�I����ʂ���Cancel�{�^�������Ȃ�g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)

                '----- V1.18.0.0�A�� -----
            Else
                giAutoDataFileNum = 1
                ReDim gsAutoDataFileFullPath(giAutoDataFileNum - 1)
                gsAutoDataFileFullPath(0) = gStrTrimFileName
                giActMode = MODE_ENDLESS                                ' �G���h���X���[�h��ݒ� V4.0.0.0-88
                '----- V1.18.0.1�C�� -----
                ' ���Y�Ǘ��f�[�^�̓N���A���Ȃ�(���[���a����)
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ���[���a���� ? 

                Else
                    Call ClearCounter(1)                                ' ���Y�Ǘ��f�[�^�̃N���A
                    Call ClrTrimPrnData()                               ' ���ݸތ��ʈ�����ڂ��ް���ر����
                End If
                'Call ClearCounter(1)                                    ' ���Y�Ǘ��f�[�^�̃N���A
                'Call ClrTrimPrnData()                                   ' ���ݸތ��ʈ�����ڂ��ް���ر����
                '----- V1.18.0.1�C�� -----
            End If
            '----- V1.18.0.0�A�� -----

            '-------------------------------------------------------------------
            '   �����^�]�J�n�̂��߂�START�L�[�����҂�
            '-------------------------------------------------------------------
            Call LAMP_CTRL(LAMP_START, True)                            ' START�����vON
            Call LAMP_CTRL(LAMP_RESET, True)                            ' RESET�����vON

            ''V5.0.0.1-23��
            ' �e�R�}���h�������s���̽ײ�޶�ް�����۰�ޖ���START/RESET�L�[�����҂�
            r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            Me.Refresh()
            If (r <= cFRS_ERR_EMG) Then                     ' �װ(����~��)�Ȃ��ċ����I��
            ElseIf (r = cFRS_ERR_RST) Then                              ' RESET�L�[�����Ȃ�Return
                GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            End If
            'r = Sub_CallFrmRset(System1, cGMODE_LDR_START)              ' START�L�[�����҂���ʕ\��
            'If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
            '    GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            'ElseIf (r = cFRS_ERR_RST) Then                              ' RESET�L�[�����Ȃ�Return
            '    GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            'End If
            ''V5.0.0.1-23��

            '----- V4.0.0.0�R�� -----
            ' SL436S �T�C�N����~�p(���[���a����)
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALT�����vOFF
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            '----- V4.0.0.0�R�� -----
            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
            r = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (r = cFRS_TO_EXLOCK) Then r = cFRS_ERR_RST '             ' �O�ʔ����b�N�^�C���A�E�g�Ȃ�߂�l���uRESET�v�ɂ���
            If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            ElseIf (r = cFRS_ERR_RST) Then                              ' RESET�L�[�����Ȃ�Return
                GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            End If
            '----- V1.18.0.1�G�� -----

            ' �}�K�W������o�Z���T��OFF�ł��鎖���`�F�b�N����
            r = Sub_SubstrateSensorOffCheck(System1)
            If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            ElseIf (r = cFRS_ERR_RST) Then                              ' �Z���T��ON(RESET�L�[����)�Ȃ�Return
                GoTo STP_END                                            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            End If

            '-------------------------------------------------------------------
            '   �����^�]�J�n
            '-------------------------------------------------------------------
            gbFgAutoOperation = True                                    ' �����^�]�t���O(True:�����^�]��)
            'Call SetSignalTower(0, 0)                                  ' �V�O�i���^���[����(�蓮�^�]OFF)
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET�����vOFF '###160
            Timer1.Enabled = True                                       ' �Ď��^�C�}�[�J�n
            stPRT_ROHM.bAutoMode = True                                 ' �g���~���O���ʈ���f�[�^�Ɏ����^�]��ݒ�(���[���a����) V1.18.0.0�B
            '----- V6.0.3.0_32��(���[���a����) -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TimerQR.Enabled = False                                 ' �Ď��^�C�}�[��~
            End If
            '----- V6.0.3.0_32�� -----

            r = Start_Trimming()                                        ' �����^�]�J�n

            '----- V6.0.3.0_32��(���[���a����) -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TimerQR.Enabled = True                                  ' �Ď��^�C�}�[�J�n
            End If
            '----- V6.0.3.0_32�� -----
STP_END:
            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then r = cFRS_ERR_RST '             ' �O�ʔ����b�N�^�C���A�E�g�Ȃ�߂�l���uRESET�v�ɂ���
            If (r <= cFRS_ERR_EMG) Then                                 ' �ُ�I�����x���̃G���[ ? 
                GoTo STP_ERR                                            ' �A�v���P�[�V���������I��
            End If
            '----- V1.18.0.1�G�� -----
            '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
            ' ������{�^���̔w�i�F���D�F�ɂ���
            gbChkSubstrateSet = False                                   ' ������t���OOFF
            BtnSubstrateSet.Enabled = False                             '�u������{�^���v�񊈐���
            Call Set_SubstrateSetBtn(gbChkSubstrateSet)
            '----- V4.11.0.0�E�� -----

            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            'V5.0.0.9�M �� V6.0.3.0�G
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
            'V5.0.0.9�M �� V6.0.3.0�G

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
            giAppMode = APP_MODE_AUTO                                   ' �A�v���P�[�V�������[�h�Đݒ� 
            Call TrimStateOff()                                         ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            'Call Form1Button(4)                                         ' �uLOAD�v/�u۷�ݸށv/�u۰�ތ��_���A�v/�uEND�v���݂̂ݗL���Ƃ���
            gbFgAutoOperation = False                                   ' �����^�]�t���OOFF
            Call Form1Button(1)                                         ' �R�}���h�{�^����L���ɂ��� ###236
            Exit Sub

STP_ERR:
            ' �A�v���P�[�V���������I��
            Call AppEndDataSave()
            Call AplicationForcedEnding()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdAutoOperation_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            gbFgAutoOperation = False                                   ' �����^�]�t���OOFF
        End Try
    End Sub
#End Region
    '----- V6.1.4.0�@��(KOA EW�aSL432RD�Ή�) -----
#Region "�����^�](���b�g�ؑւ��@�\)����(����)"
    '''=========================================================================
    ''' <summary>�����^�](���b�g�ؑւ��@�\)����</summary>
    ''' <remarks>KOA EW�����łł�CmdAutoOperation_Click()�̒��ł���Ă�����
    '''          �{����SL436�n�p�Ȃ̂ŁASL432R�p��CmdAutoOperation_Click()���番������
    ''' </remarks>
    '''=========================================================================
    Private Sub LotChangeProc()

        Dim r As Integer
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   �����^�]�O����
            '-------------------------------------------------------------------
            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            Call Form1Button(0)                                         ' �R�}���h�{�^���𖳌��ɂ���
            r = TrimStateOn(F_AUTO, APP_MODE_AUTO, MSG_OPLOG_AUTO, "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' �ҏW���̃f�[�^��ۑ�����
            If (gCmpTrimDataFlg = 1) Then                               ' �f�[�^�X�V�t���O = 1(�X�V����)
                '"�ҏW���̃f�[�^��ۑ����܂����H"
                r = MsgBox(MSG_AUTO_19, MsgBoxStyle.OkCancel)
                If (r = MsgBoxResult.Ok) Then
                    ' �f�[�^�ۑ�
                    Call Me.CmdSave_Click(Me.CmdSave, New System.EventArgs())
                    giAppMode = APP_MODE_AUTO                           ' APP Mode�������^�]���[�h�ɖ߂�(Save����A���0�ɂȂ邽��)
                    gCmpTrimDataFlg = 0                                 ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�)
                End If
            End If

            '-------------------------------------------------------------------
            '   �����^�]�̂��߂̃f�[�^�I����ʂ�\������
            '-------------------------------------------------------------------
            Call frmAutoObj.ShowDialog()                                ' �f�[�^�I����ʕ\��
            r = frmAutoObj.sGetReturn()                                 ' Return�l�擾(�A���^�]����Ӱ��(0:϶޼��Ӱ�� 1:ۯ�Ӱ�� 2:����ڽӰ��))
            Me.Refresh()                                                ' �ĕ`�悵�Ȃ��Ǝ���START�L�[�����҂��ł���ʕ\�����c�� 
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' �f�[�^�I����ʂ���Cancel�{�^�������Ȃ�g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)

            ' ���[�_��ԃ`�F�b�N(�����^�]��),���[�_�������ɐ؂�ւ��܂ő҂�
            ' "���[�_�M�����蓮�ł�", "���[�_�������ɐ؂�ւ��Ă�������", "Cancel�{�^�������ŏ������I�����܂�"
            r = Me.System1.Form_Reset(cGMODE_LDR_CHK_AUTO, gSysPrm, giAppMode, gbInitialized, 0, 0, gLoadDTFlag)
            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then            ' �G���[
                '----- V6.1.4.0�@��(Dllsystem�Ń��b�Z�[�W�͕\����) -----
                'MsgBox("i-TKY::Form_Reset(cGMODE_LDR_MAGAGINE_EXCHG) error." + vbCrLf + " Err Code = " + r.ToString)
                If (r <= cFRS_ERR_EMG) Then                             ' �ُ�I�����x���̃G���[ ? 
                    GoTo STP_ERR                                        ' �A�v���P�[�V���������I��
                End If
            ElseIf (r = cFRS_ERR_RST) Then                              ' RESET(Cancel�{�^��)���� ? 
                frmAutoObj.SetAutoOpeCancel()                           ' ���[�_�o��(ON=�g���~���O�m�f, OFF=�Ȃ�)�g���~���O�s�ǐM����A�������^�]���~�ʒm�Ɏg�p
                frmAutoObj.gbFgAutoOperation432 = False                 ' �����^�]�t���O = �����^�]���łȂ�
                frmAutoObj.AutoOperationEnd()                           ' �A�������^�]�I������
                GoTo STP_END
            End If
            m_lTrimNgCount = 0                                          ' �A���g���~���O�m�f�����J�E���^�[ V6.1.4.0�H

            '-------------------------------------------------------------------
            '   �����^�]�J�n(���[�_�����START�M����҂�)
            '-------------------------------------------------------------------
            Call LAMP_CTRL(LAMP_START, False)                           ' START�����vOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET�����vOFF
            Timer1.Enabled = True                                       ' �Ď��^�C�}�[�J�n(TrimStateOff()���ł���Ă邯�ǌ��̃\�[�X�ɍ��킹��)
STP_END:
            ' ���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
            giAppMode = APP_MODE_AUTO                                   ' �A�v���P�[�V�������[�h�Đݒ� 
            Call TrimStateOff()                                         ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��(�Ď��^�C�}�[�J�n)
            Call Form1Button(1)                                         ' �R�}���h�{�^����L���ɂ���(TrimStateOff()���ł���Ă邯��)
            Exit Sub

STP_ERR:
            ' �A�v���P�[�V���������I��
            Call AppEndDataSave()
            Call AplicationForcedEnding()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.LotChangeProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            gbFgAutoOperation = False                                   ' �����^�]�t���OOFF
        End Try
    End Sub
#End Region
    '----- V6.1.4.0�@�� -----
#Region "���[�_���_���A�{�^������������"
    '''=========================================================================
    ''' <summary>���[�_���_���A�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub CmdLoaderInit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdLoaderInit.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            'V4.11.0.0�I
            gbInitialized = False                           ' ���_���A��
            ' �e�R�}���h�������s���̽ײ�޶�ް�����۰�ޖ���START/RESET�L�[�����҂�
            giAppMode = APP_MODE_LOADERINIT
            r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            Me.Refresh()
            If (r <= cFRS_ERR_EMG) Then                     ' �װ(����~��)�Ȃ��ċ����I��
                ''V5.0.0.1-23��
                'GoTo STP_END
                GoTo STP_ERR
                ''V5.0.0.1-23��
            ElseIf r = cFRS_ERR_RST Then
                GoTo STP_END
            End If
            'V4.11.0.0�I



            '���v�\�������̏�ԕύX
            Call Form1Button(0)                                     ' �R�}���h�{�^���𖳌��ɂ��� ###236
            Call RedrawDisplayDistribution(True)

            ' ��������
            r = TrimStateOn(F_LOADERINI, APP_MODE_LOADERINIT, MSG_OPLOG_LOADERINIT, "MANUAL")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            '----- ###185�� ----- 
            ' �ڕ���Ɋ������ꍇ�́A��菜�����܂ő҂�
            r = SubstrateCheck(System1, APP_MODE_LOADERINIT)
            If (r <> cFRS_NORMAL) Then GoTo STP_END
            '----- ###185�� ----- 

            '----- ###197�� ----- 
            ' NG�r�oBOX�����t�̏ꍇ�́A��菜�����܂ő҂�(���[�_�����_���A�ł��Ȃ�����)
            r = NgBoxCheck(System1, APP_MODE_LOADERINIT)
            If (r < cFRS_NORMAL) Then
                If (r <> cFRS_NORMAL) Then GoTo STP_END
            End If
            '----- ###197�� ----- 

            ' ���[�_���_���A����(�G���[�������̃��b�Z�[�W�͕\����)
            Call W_RESET()                                              ' �A���[�����Z�b�g�M�����o ###073
            Call W_START()                                              ' �X�^�[�g�M�����o ###073


            r = Sub_CallFrmRset(System1, cGMODE_LDR_ORG)
            'V5.0.0.1-21��
            If (r <= cFRS_ERR_EMG) Then                     ' �װ(����~��)�Ȃ��ċ����I��
                GoTo STP_ERR
            End If
            'V5.0.0.1-21��
            ' �R�}���h���s�㏈��(�ُ�I�����x���̃G���[����CmdExec_Term()�ŃA�v���I��)
            Call CmdExec_Term(APP_MODE_LOADERINIT, r)

            ' �I������ 
STP_END:

            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            Call TrimStateOff()                                         ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��
            Call Form1Button(1)                                         ' �R�}���h�{�^����L���ɂ��� ###236
            giAppMode = APP_MODE_IDLE            'V4.11.0.0�I

            Exit Sub

STP_ERR:
            ' �A�v���P�[�V���������I��
            Call AppEndDataSave()
            Call AplicationForcedEnding()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLoaderInit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�����o�^�����{�^������������"
    '''=========================================================================
    ''' <summary>�����o�^�����{�^������������</summary>
    ''' <remarks>'V4.10.0.0�B</remarks>
    '''=========================================================================
    Private Sub CmdIntegrated_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdIntegrated.Click
        Try
            gbIntegratedMode = True     'V4.10.0.0�B �ꊇ�e�B�[�`���O����True�ɂȂ�ϐ�
            gbCorrectDone = False       'V4.10.0.0�B �ꊇ�e�B�[�`���O���́A��x�␳�����{������͍s��Ȃ��B
            gbSubstExistChkDone = False 'V4.10.0.0�B �I�y���[�^�e�B�[�`���O�ȑf�� ��x��݉׃`�F�b�N���s�������͎��{���Ȃ��ׂɎg�p
            ' �R�}���h���s
            Dim r As Short = CmdExec_Proc(F_INTEGRATED, APP_MODE_INTEGRATED, MSG_OPLOG_INTEGRATED, "") ' UNDONE: MSG_OPLOG_INTEGRATED ؿ���Ή��v

            gbCorrectDone = False       'V4.10.0.0�B �ꊇ�e�B�[�`���O���́A��x�␳�����{������͍s��Ȃ��B
            gbIntegratedMode = False    'V4.10.0.0�B �ꊇ�e�B�[�`���O����True�ɂȂ�ϐ�
            gbSubstExistChkDone = False 'V4.10.0.0�B �I�y���[�^�e�B�[�`���O�ȑf�� ��x��݉׃`�F�b�N���s�������͎��{���Ȃ��ׂɎg�p

            ' �g���b�v�G���[������
        Catch ex As Exception
            Dim msg As String = "i-TKY.CmdIntegrated_Click() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, msg)
        Finally
            grpIntegrated.Visible = False
        End Try
    End Sub
#End Region
    '----- V1.13.0.0�B�� -----
#Region "�I�[�g�v���[�u�o�^�{�^������������"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u�o�^�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdAotoProbePtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_APROBEREC, APP_MODE_APROBEREC, MSG_OPLOG_APROBEREC, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdAotoProbePtn_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�I�[�g�v���[�u���s�{�^������������"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u���s�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdAotoProbeCorrect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_APROBEEXE, APP_MODE_APROBEEXE, MSG_OPLOG_APROBEEXE, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdAotoProbeCorrect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�I�[�g�v���[�u���s����"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u���s����</summary>
    ''' <param name="pltInfo"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function AutoProbing(ByRef pltInfo As PlateInfo) As Short

        Dim r As Short
        Dim strMSG As String
        Dim parModules As MainModules

        Try
            parModules = New MainModules
            With pltInfo
                '--------------------------------------------------------------------------
                '   �ƕ␳(��߼��)���s(�ƕ␳�̂ݎ��s���␳�l(gfCorrectPosX, gfCorrectPosY)�����߂�)
                '--------------------------------------------------------------------------
                r = Move_Trimposition(pltInfo, 0.0, 0.0)        ' �ƕ␳(��߼��) & XYð�����шʒu�ւ͈ړ����Ȃ�
                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    Return (r)                                  ' Return�l�ݒ�
                End If

                '--------------------------------------------------------------------------
                '   �I�[�g�v���[�u���s
                '--------------------------------------------------------------------------
                Call SetAProbParam(pltInfo, parModules)         ' �p�����[�^�ݒ� 
                r = parModules.Sub_CallFrmMatrix()              ' �v���[�u����
                If (r <> cFRS_NORMAL) Then                      ' �G���[ ?
                    Return (r)                                  ' Return�l�ݒ�
                End If

                Return (cFRS_NORMAL)
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.AutoProbing() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                  ' Return�l = �g���b�v�G���[
        End Try

    End Function
#End Region

#Region "�I�[�g�v���[�u���s�p�p�����[�^�ݒ菈��"
    '''=========================================================================
    ''' <summary>�I�[�g�v���[�u���s�p�p�����[�^�ݒ菈��</summary>
    ''' <param name="pltInfo">   (INP)�v���[�g�f�[�^</param>
    ''' <param name="parModules">(INP)parModules</param>
    '''=========================================================================
    Private Sub SetAProbParam(ByRef pltInfo As PlateInfo, ByRef parModules As MainModules)

        Dim strMSG As String                                                ' ���b�Z�[�W�ҏW��

        Try
            With pltInfo
                parModules.stAPRB.intAProbeGroupNo1 = .intStepMeasLwGrpNo   ' �p�^�[��1(����)�p�O���[�v�ԍ�
                parModules.stAPRB.intAProbePtnNo1 = .intStepMeasLwPtnNo     ' �p�^�[��1(����)�p�p�^�[���ԍ�
                parModules.stAPRB.intAProbeGroupNo2 = .intStepMeasUpGrpNo   ' �p�^�[��2(���)�p�O���[�v�ԍ�
                parModules.stAPRB.intAProbePtnNo2 = .intStepMeasUpPtnNo     ' �p�^�[��2(���)�p�p�^�[���ԍ�
                parModules.stAPRB.dblAProbeBpPosX = .dblStepMeasBpPosX      ' �p�^�[��1(����)�pBP�ʒuX
                parModules.stAPRB.dblAProbeBpPosY = .dblStepMeasBpPosY      ' �p�^�[��1(����)�pBP�ʒuY
                parModules.stAPRB.dblAProbeStgPosX = .dblStepMeasTblOstX    ' �p�^�[��2(���)�p�X�e�[�W�I�t�Z�b�g�ʒuX
                parModules.stAPRB.dblAProbeStgPosY = .dblStepMeasTblOstY    ' �p�^�[��1(����)�p�X�e�[�W�I�t�Z�b�g�ʒuY
                parModules.stAPRB.intAProbeStepCount = .intStepMeasCnt      ' �X�e�b�v���s�p�X�e�b�v��
                parModules.stAPRB.intAProbeStepCount2 = .intStepMeasReptCnt ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v��
                parModules.stAPRB.dblAProbePitch = .dblStepMeasPitch        ' �X�e�b�v���s�p�X�e�b�v�s�b�`
                parModules.stAPRB.dblAProbePitch2 = .dblStepMeasReptPitch   ' �X�e�b�v���s�p�J�Ԃ��X�e�b�v�s�b�`
            End With
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SetAProbParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�h�c�e�B�[�`���O�{�^������������"
    '''=========================================================================
    ''' <summary>�h�c�e�B�[�`���O�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdIDTeach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_IDTEACH, APP_MODE_IDTEACH, MSG_OPLOG_IDTEACH, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdIDTeach_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�L�k�␳(�摜�o�^)�{�^������������"
    '''=========================================================================
    ''' <summary>�L�k�␳(�摜�o�^)�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdSinsyukuPtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_SINSYUKU, APP_MODE_SINSYUKU, MSG_OPLOG_SINSYUKU, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdSinsyukuPtn_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0�B�� -----
#Region "�}�b�v�{�^������������"
    '''=========================================================================
    ''' <summary>�}�b�v�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim r As Integer
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            r = TrimStateOn(F_MAP, APP_MODE_MAP, MSG_OPLOG_MAP, "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' �R���\�[���{�^���̃����v��Ԃ�ݒ肷��
            Call LAMP_CTRL(LAMP_START, False)                       ' START���ߏ��� 
            Call LAMP_CTRL(LAMP_RESET, False)                       ' RESET���ߏ��� 

            Call Form1Button(0)                                     ' �R�}���h�{�^���𖳌��ɂ���
            Call SetVisiblePropForVideo(False)

            'V6.0.0.0�C            FormMapSelect.Show()
            Dim frm As New FormMapSelect    'V6.0.0.0�C
            frm.Show()                      'V6.0.0.0�C

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
STP_END:
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            ' Form�����ɖ߂�
            'Me.WindowState = FormWindowState.Normal
            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��
            Call TrimStateOff()
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdEdit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try

    End Sub
#End Region
    'V4.10.0.0�H��
#Region "�v���[�u�N���[�j���O���s�{�^������������"
    Private Sub CmdT_ProbeCleaning_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdT_ProbeCleaning.Click
        Dim strMSG As String
        Dim r As Short

        Try
            ' �R�}���h���s
            r = CmdExec_Proc(F_PROBE_CLEANING, APP_MODE_PROBE_CLEANING, MSG_OPLOG_MAINT, "")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdT_ProbeCleaning_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    'V4.10.0.0�H��
    '----- V6.1.4.0�E��(KOA EW�aSL432RD�Ή�) -----
#Region "̫��ޕ\���{�^������������"
    '''=========================================================================
    '''<summary>̫��ޕ\���{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub btnHistoryData_Click(sender As System.Object, e As System.EventArgs) Handles CmdFolderOpen.Click

        Try
            ' ���Y�Ǘ��ް�۸ޕۑ�̫��ނ𴸽��۰ׂŊJ��
            Call frmAutoObj.ProductControlFolderOpen()

        Catch ex As Exception
            Dim strMsg As String = "i-TKY.btnHistoryData_Click() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMsg)
        End Try
    End Sub
#End Region
    '----- V6.1.4.0�E�� -----

    '========================================================================================
    '   �e�R�}���h���s����
    '========================================================================================
#Region "�e�R�}���h���s���䏈��"
    '''=========================================================================
    '''<summary>�e�R�}���h���s���䏈��</summary>
    '''<param name="IdxFnc">  (INP)�@�\�I���`�e�[�u���̲��ޯ��</param>
    '''<param name="iAppMode">(INP)����Ӱ��(giAppMode�Q��)</param> 
    '''<param name="strLOG">  (INP)����۸�ү����</param>
    '''<param name="strNote"> (INP)����۸޺���</param>
    '''<returns>0=���� ,0�ȊO=�G���[</returns>
    '''<remarks>����~/�W�o�@�ُ펞�͓��֐����ſ�ċ����I������</remarks>
    '''=========================================================================
    Public Function CmdExec_Proc(ByVal IdxFnc As Short, ByRef iAppMode As Short, ByRef strLOG As String, ByRef strNote As String,
                                 Optional ByVal isRough As Boolean = False) As Short                'V5.0.0.9�C
        'Public Function CmdExec_Proc(ByVal IdxFnc As Short, ByRef iAppMode As Short, ByRef strLOG As String, ByRef strNote As String) As Short

        Dim strMSG As String
        Dim r As Short
        Dim InterlockSts As Integer                                     ' ###062
        Dim SwitchSts As Long                                           ' ###062
        'V6.0.0.0�J        Dim objForm As Object
        Dim objForm As Form                         'V6.0.0.0�J
        'Dim ObjGazou As Process = Nothing

        Try
            '-----------------------------------------------------------------------------
            '   ��������
            '-----------------------------------------------------------------------------
            ' �R�}���h���s�O�̃`�F�b�N
            r = CmdExec_Check(iAppMode)
            If (r <> cFRS_NORMAL) Then                                  ' �`�F�b�N�G���[ ?
                '// 'V4.0.0.0-90 
                CommandEnableSet(True)

                Return (r)                                              ' Return�l = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
            End If

            ' ���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(True)

            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(False)
            End If
            ''V5.0.0.1�C��
            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            objForm = Nothing
            r = TrimStateOn(IdxFnc, iAppMode, strLOG, strNote)
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' >>> V3.1.0.0�@ 2014/11/28
            ' �f�[�^�\�������������f����
            If IsDataDisplayCheck(iAppMode) = True Then
                ' �f�[�^�\�����\���ɂ���
                Call SetDataDisplayOff()
            End If
            ' <<< V3.1.0.0�@ 2014/11/28

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call SimpleTrimmer.Sub_StartResetButtonDispOff()
            End If
#End If
            ' �R�}���h���s�O����
            r = CmdExec_Init(iAppMode)
            If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESET��) ?
                If (gSysPrm.stSPF.giWithStartSw = 0) Then               ' ����SW�����҂�(��߼��)�łȂ� ?
                    GoTo STP_END
                Else                                                    ' ����SW�����҂�(��߼��)�Ȃ�
                    GoTo STP_TRM                                        ' �ײ�޶�ް�۽ނ���READY��Ԃ�
                End If
            End If

            ' �G���[�`�F�b�N
            If (r < cFRS_NORMAL) Then                                   ' �G���[ ?
                ' �J�o�[�J���o(����SW�����҂�(��߼��)����������)
                If (r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Or (r = cFRS_ERR_LATCH) Then
                    r = cFRS_NORMAL
                    GoTo STP_TRM                                        ' �J�o�[�J���o�Ȃ�ײ�޶�ް�۽ނ���READY��Ԃ�
                Else
                    GoTo STP_END
                End If
            End If

            '-----------------------------------------------------------------------------
            '   �摜�\���v���O�������N������
            '-----------------------------------------------------------------------------
            ' �e�B�[�`���O�R�}���h�̏ꍇ(�J�b�g�g���[�X�p)
            'V3.0.0.0�B            If (iAppMode = APP_MODE_TEACH) Then
            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0) '###052
            'V3.0.0.0�B            Else
            ''Video�̕\���𖳌��ɂ���B
            'Call Me.VideoLibrary1.VideoStop()
            'Me.VideoLibrary1.Visible = False
            'V3.0.0.0�B            End If

            '' �e�B�[�`���O�R�}���h���̏ꍇ
            'If (iAppMode = APP_MODE_TEACH) Or (iAppMode = APP_MODE_LASER) _
            '    Or (iAppMode = APP_MODE_PROBE) Or (iAppMode = APP_MODE_TX) _
            '    Or (iAppMode = APP_MODE_TY) Or (iAppMode = APP_MODE_TY2) _
            '    Or (iAppMode = APP_MODE_CIRCUIT) Then
            '    '' ''r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
            'Else
            '    ''Video�̕\���𖳌��ɂ���B
            '    'Call Me.VideoLibrary1.VideoStop()
            '    'Me.VideoLibrary1.Visible = False
            'End If

            '-----------------------------------------------------------------------------
            '   �����v�_��
            '-----------------------------------------------------------------------------
            If ((iAppMode = APP_MODE_TTHETA) Or (iAppMode = APP_MODE_TX) Or
                (iAppMode = APP_MODE_TY) Or (iAppMode = APP_MODE_TY2) Or
                (iAppMode = APP_MODE_CARIB) Or (iAppMode = APP_MODE_CUTREVIDE)) Then
                'START/RESET�����v��_��
                Call LAMP_CTRL(LAMP_START, True)
                Call LAMP_CTRL(LAMP_RESET, True)
            End If

            ' �V�O�i���^���[���_��(�e�B�[�`���O��) ###062
            ' ���A���C���^�[���b�N������(���_��)�D��
            r = INTERLOCK_CHECK(InterlockSts, SwitchSts)
            If (InterlockSts = INTERLOCK_STS_DISABLE_NO) Then           ' �C���^�[���b�N���Ȃ物�_��
                ' �V�O�i���^���[����(On=�e�B�[�`���O��,Off=�S�ޯ�) ###007
                Select Case (gSysPrm.stIOC.giSignalTower)
                    Case SIGTOWR_NORMAL                                 ' �W��(���_��(�A�����_�ŗD��)) ###024 
                        'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͉��_��)
                        ' Call Me.System1.SetSignalTower(SIGOUT_YLW_ON, &HFFFF)
                        Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_TEACHING)
                        'V5.0.0.9�M �� V6.0.3.0�G
                    Case SIGTOWR_SPCIAL                                 ' ����(�Ȃ�)
                End Select
            End If

            '' ''�X�^�[�g�X�C�b�`���b�`�N���A   20121001 M.KAMI ADD
            ' ''Call ZCONRST()
            If gKeiTyp = KEY_TYPE_RS Then
                If ChkVideoChangeMode(iAppMode) Then
                    SetTeachVideoSize() 'V2.0.0.0�Q
                End If
                GroupBoxEnableChange(False)
            End If

            '-----------------------------------------------------------------------------
            '   �e�R�}���h���s����
            '-----------------------------------------------------------------------------
            ' ========================================================= 'V4.10.0.0�B  ��
            Dim prmAppMode As Short = iAppMode
            Dim commands As New List(Of Short)
            Dim lblList As New List(Of Label)
            If (APP_MODE_INTEGRATED = iAppMode) Then
                ' ���s�������ؽĂ�\������
                For Each lbl As Label In flpIntegrated.Controls
                    lbl.BackColor = SystemColors.Control
                Next
                grpIntegrated.BringToFront()
                grpIntegrated.Visible = True
                grpIntegrated.Refresh()

                If (0 <= stFNC(F_RECOG).iDEF) AndAlso (0 <> gSysPrm.stDEV.giTheta) Then
                    commands.Add(APP_MODE_RECOG) ' �摜�o�^
                    lblList.Add(Me.lblIntegRecog)
                End If
                If (0 <= stFNC(F_PROBE).iDEF) Then
                    commands.Add(APP_MODE_PROBE) ' ��۰��
                    lblList.Add(Me.lblIntegProbe)
                End If
                If (0 <= stFNC(F_TX).iDEF) Then
                    commands.Add(APP_MODE_TX)   ' BP�ʒu����
                    lblList.Add(Me.lblIntegTX)
                End If
                If (0 <= stFNC(F_TEACH).iDEF) Then
                    commands.Add(APP_MODE_TEACH) ' è��ݸ�
                    lblList.Add(Me.lblIntegTeach)
                End If
                If (0 <= stFNC(F_TY).iDEF) Then
                    commands.Add(APP_MODE_TY)   ' �ð�ވʒu����
                    lblList.Add(Me.lblIntegTY)
                End If
            Else
                commands.Add(iAppMode)
            End If

            For i As Integer = 0 To (commands.Count - 1) Step 1         'V4.10.0.0�B
                iAppMode = commands(i)

                If (APP_MODE_INTEGRATED = prmAppMode) Then
                    ' ���s������ނ̔w�i�F
                    lblList(i).BackColor = Color.FromArgb(255, 255, 225)

                    If (1 <= i) Then
                        ' ���s�ςݺ���ނ̔w�i�F
                        lblList(i - 1).BackColor = SystemColors.Control

                        ' �I�u�W�F�N�g�J��
                        If (objForm IsNot Nothing) Then
                            objForm.Close()                             ' �I�u�W�F�N�g�J��
                            objForm.Dispose()                           ' ���\�[�X�J��
                        End If

                        ' lbl.Text�����s���܂�
                        If (cFRS_ERR_START <> System1.TrmMsgBox(gSysPrm,
                            String.Format(iTKY_001, lblList(i).Text) & New String(" "c, 20), vbOKCancel, gAppName)) Then
                            ' ��ݾق̏ꍇٰ�ߏI��
                            Exit For
                        End If

                        ' �R�}���h���s�O�̃`�F�b�N
                        r = CmdExec_Check(iAppMode)
                        If (cFRS_NORMAL <> r) Then                      ' �`�F�b�N�G���[ ?
                            If (cERR_TRAP = r) Then
                                CommandEnableSet(True)
                                Return (r)                              ' ��O�������͏I��
                            Else
                                If (APP_MODE_TX = iAppMode) OrElse (APP_MODE_TY = iAppMode) Then
                                    ' �u���b�N�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I
                                    ' ��R�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I
                                    ' TODO: �I�����H
                                End If
                                Continue For ' TODO: ���̏����ɐi�ށH
                            End If
                        End If

                    End If
                End If
                ' ===================================================== 'V4.10.0.0�B  ��

                Select Case (iAppMode)
                    Case APP_MODE_LASER                                     ' ���[�U�[������ʏ���(Laser.OCX)
                        If (gSysPrm.stRMC.giRmCtrl2 >= 2 And gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then     'V5.0.0.6�G RMCTRL2�Ή� ?
                            Call ATTRESET()                                                     'V1.25.0.0�M
                            Call LATTSET(gSysPrm.stRAT.giAttFix, gSysPrm.stRAT.giAttRot)        'V1.25.0.0�M
                        End If                                                                                  'V5.0.0.6�G
                        SetLaserItemsVisible(0)                             ' ���[�U�p���[�����֘A���ڂ��\���Ƃ��� ###029 
                        r = LaserProc(typPlateInfo)
                        SetLaserItemsVisible(1)                             ' ���[�U�p���[�����֘A���ڂ�\���Ƃ��� ###029 

                    Case APP_MODE_PROBE                                     ' �v���[�u�e�B�[�`���O��ʏ���(Probe.OCX)
                        r = ProbeTeaching(typPlateInfo)

                    Case APP_MODE_TEACH                                     ' �e�B�[�`���O��ʏ���(Teach.OCX)
                        r = Teaching(APP_MODE_TEACH, typPlateInfo)

                    Case APP_MODE_RECOG                                     ' �ƕ␳�p�p�^�[���o�^��ʏ���(Video.OCX)
                        'r = PatternTeach(iAppMode, typPlateInfo)
                        r = PatternTeach(iAppMode, typPlateInfo, isRough)  'V5.0.0.9�C

                    Case APP_MODE_CUTPOS                                    ' ��Ĉʒu�␳�p�p�^�[���o�^��ʏ���(Video.OCX)
                        r = CutPosTeach(typPlateInfo)

                    Case APP_MODE_TTHETA                                    ' �s��(�Ɗp�x�␳)�e�B�[�`���O
                        objForm = New frmTThetaTeach()
                        r = CmdCall_Proc(APP_MODE_TTHETA, objForm, 0, typPlateInfo)

                    Case APP_MODE_TX                                        ' TX�e�B�[�`���O
                        objForm = New frmTxTyTeach()
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)
                        'V5.0.0.6�I���L�����Z���łȂ���ΊO���J�����̃J�b�g�ʒu�␳�l������������B
                        If giTeachpointUse = 1 And (r <> cFRS_ERR_RST) Then
                            DataAccess.ResetCutTeachPoint()             ' �O���J�����̃J�b�g�ʒu�␳�l������������B
                        End If
                        'V5.0.0.6�I��
                        'If (r = cFRS_TxTy) Then                             ' TX2���� ?
                        If (r = cFRS_TxTy) AndAlso (APP_MODE_INTEGRATED <> prmAppMode) Then ' TX2���� ?   'V4.10.0.0�B
                            r = Teaching(APP_MODE_TEACH, typPlateInfo)
                        End If

                    Case APP_MODE_TY                                        ' TY�e�B�[�`���O
                        objForm = New frmTxTyTeach()
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)
                        If (r = cFRS_TxTy) Then                             ' TY2���� ?
                            Call objForm.Close()                            ' �I�u�W�F�N�g�J��
                            Call objForm.Dispose()                          ' ���\�[�X�J��
                            objForm = New frmTy2Teach()                     ' TY2�e�B�[�`���O���s
                            r = CmdCall_Proc(APP_MODE_TY2, objForm, 0, typPlateInfo)
                        End If

                    Case APP_MODE_TY2                                       ' TY2�e�B�[�`���O
                        objForm = New frmTy2Teach()
                        r = CmdCall_Proc(APP_MODE_TY2, objForm, 0, typPlateInfo)

                    Case APP_MODE_EXCAM_R1TEACH                             ' �O���J����R1�e�B�[�`���O�y�O���J�����z(Teach.OCX)
                        Call Set_Led(1, 1)                                  ' �o�b�N���C�g�Ɩ�ON V1.20.0.0�I
                        r = Teaching(APP_MODE_EXCAM_R1TEACH, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' �o�b�N���C�g�Ɩ�OFF V1.20.0.0�I

                    Case APP_MODE_EXCAM_TEACH                               ' �O���J�����e�B�[�`���O�y�O���J�����z(Teach.OCX)
                        Call Set_Led(1, 1)                                  ' �o�b�N���C�g�Ɩ�ON V1.20.0.0�I
                        r = Teaching(APP_MODE_EXCAM_TEACH, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' �o�b�N���C�g�Ɩ�OFF V1.20.0.0�I

                    Case APP_MODE_CARIB_REC                                 ' �摜�o�^(�L�����u���[�V�����␳�p)�y�O���J�����z(Video.OCX)
                        Call Set_Led(1, 1)                                  ' �o�b�N���C�g�Ɩ�ON V1.20.0.0�I
                        r = PatternTeach(iAppMode, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' �o�b�N���C�g�Ɩ�OFF V1.20.0.0�I

                    Case APP_MODE_CARIB                                     ' �L�����u���[�V�������s�y�O���J�����z
                        Call Set_Led(1, 1)                                  ' �o�b�N���C�g�Ɩ�ON V1.20.0.0�I
                        objForm = New FrmCalibration()
                        r = CmdCall_Proc(APP_MODE_CARIB, objForm, 0, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' �o�b�N���C�g�Ɩ�OFF V1.20.0.0�I

                    Case APP_MODE_CUTREVISE_REC                             ' �摜�o�^(�J�b�g�ʒu�␳�p)�y�O���J�����z(Video.OCX)
                        Call Set_Led(1, 1)                                  ' �o�b�N���C�g�Ɩ�ON V1.20.0.0�I
                        r = PatternTeach(iAppMode, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' �o�b�N���C�g�Ɩ�OFF V1.20.0.0�I

                    Case APP_MODE_CUTREVIDE                                 ' �J�b�g�ʒu�␳�y�O���J�����z
                        Call Set_Led(1, 1)                                  ' �o�b�N���C�g�Ɩ�ON V1.20.0.0�I
                        objForm = New FrmCutPosCorrect()
                        r = CmdCall_Proc(APP_MODE_CUTREVIDE, objForm, 0, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' �o�b�N���C�g�Ɩ�OFF V1.20.0.0�I

                    Case APP_MODE_CIRCUIT                                   ' �T�[�L�b�g�e�B�[�`���O
                        objForm = New frmCircuitTeach()
                        r = CmdCall_Proc(APP_MODE_CIRCUIT, objForm, 0, typPlateInfo)

                        '----- V1.13.0.0�B�� -----
                    Case APP_MODE_APROBEREC                                 ' �摜�o�^(�I�[�g�v���[�u�p)�y�����J�����z(Video.OCX)
                        r = PatternTeach(iAppMode, typPlateInfo)

                    Case APP_MODE_APROBEEXE                                 ' �I�[�g�v���[�u���s
                        r = AutoProbing(typPlateInfo)

                    Case APP_MODE_SINSYUKU
                        r = PatternTeach(iAppMode, typPlateInfo)

                        '----- V1.13.0.0�B�� -----
                    Case APP_MODE_IDTEACH                                   'V1.13.0.0�E �h�c���[�_�[���s
                        objForm = New frmIDReaderTeach()
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)

                    Case APP_MODE_MAINTENANCE                               ' �����e�i���X
                        objForm = New frmMaintenance()
                        'V6.0.0.0�J                        objForm.gdblCleaningPosX = 4  frmMaintenance �Ƀ����o�Ȃ�
                        'V6.0.0.0�J                        objForm.gdblCleaningPosY = 1  frmMaintenance �Ƀ����o�Ȃ�
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)

                    Case APP_MODE_PROBE_CLEANING                            ' �v���[�u�N���[�j���O
                        'objForm = New frmProbeCleaning()
                        'objForm.sSetInitVal()          'V6.0.0.0�J �R���X�g���N�^�̈����Őݒ肷��
                        objForm = New frmProbeCleaning(
                            typPlateInfo.dblPrbCleanPosX, typPlateInfo.dblPrbCleanPosY) 'V6.0.0.0�J

                        'V6.0.0.0�J                        Call objForm.ShowDialog()
                        objForm.Show(Me)           'V6.0.0.0�J
                        'V6.0.0.0�J                        r = objForm.sGetReturn()                         ' Return�l = �R�}���h�I������
                        r = (TryCast(objForm, ICommonMethods)).Execute()        ' Return�l = �R�}���h�I������    'V6.0.0.0�J

                        '                    r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)

                    Case Else
                        strMSG = "CmdExec_Proc() AppMode ERROR = " + iAppMode
                        MsgBox(strMSG)
                        r = -1 * ERR_CMD_PRM                                ' Return�l = �p�����[�^�G���[ 
                        GoTo STP_TRM
                End Select

                ' ===================================================== 'V4.10.0.0�B��
                If (r < cFRS_NORMAL) Then Exit For ' ʰ�޴װ
            Next i

            If (APP_MODE_INTEGRATED = prmAppMode) Then
                iAppMode = prmAppMode   ' TODO: �ȍ~�̏I�������������Ȃ����߂ɕʂ�Ӱ�ނɂ���K�v�����邩���H
                giAppMode = prmAppMode
                grpIntegrated.Visible = False
            Else
                iAppMode = prmAppMode
            End If
            ' ========================================================= 'V4.10.0.0�B��

            '-----------------------------------------------------------------------------
            '   �㏈��
            '-----------------------------------------------------------------------------
            ' �I�u�W�F�N�g�J��
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                ' �I�u�W�F�N�g�J��
                Call objForm.Dispose()                              ' ���\�[�X�J��
                objForm = Nothing                                       'V4.10.0.0�B
                ' �K�x�[�W�R���N�V�����ɂ�閾���I�ȊJ��
                Call GC.Collect()
            End If

            If gKeiTyp = KEY_TYPE_RS Then
                If ChkVideoChangeMode(iAppMode) Then
                    SetSimpleVideoSize() 'V2.0.0.0�Q
                End If
                GroupBoxEnableChange(True)
            End If

            '-----------------------------------------------------------------------------
            '   �����v����
            '-----------------------------------------------------------------------------
            If ((iAppMode = APP_MODE_TTHETA) Or (iAppMode = APP_MODE_TX) Or
                (iAppMode = APP_MODE_TY) Or (iAppMode = APP_MODE_TY2) Or
                (iAppMode = APP_MODE_CARIB) Or (iAppMode = APP_MODE_CUTREVIDE)) Then
                'START/RESET�����v��_��
                Call LAMP_CTRL(LAMP_START, False)
                Call LAMP_CTRL(LAMP_RESET, False)
            End If
            Call gparModules.CrossLineDispOff()                         'V5.0.0.6�K�N���X���C����\��
#If False Then                          'V6.0.0.0�D
            ' �摜�\���v���O�������I������
            If (iAppMode = APP_MODE_TEACH) Then
                End_GazouProc(ObjGazou)                                 ' �摜�\���v���O�������I������
            Else
                'Video�̕\����L���ɂ���B
                VideoLibrary1.Visible = True
                Call Me.VideoLibrary1.VideoStart()
            End If
#End If
            ' �V�O�i���^���[����(On=���f�B, Off=�S�ޯ�) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(���_��)
                    'V5.0.0.9�M �� V6.0.3.0�G
                    ' Call Me.System1.SetSignalTower(0, &HEFFF)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(�Ȃ�)

            End Select

STP_TRM:
            ' �R�}���h���s�㏈��
            Call CmdExec_Term(iAppMode, r)

STP_END:
            ' >>> V3.1.0.0�@ 2014/11/28
            ' �f�[�^�\����\���ɂ���
            Call SetDataDisplayOn()
            ' <<< V3.1.0.0�@ 2014/11/28

            '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
            ' �o�[�R�[�h���(WALSIN)��\��
            'V5.0.0.9�N            If (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
            If (BarcodeType.Walsin = BarCode_Data.Type) Then            'V5.0.0.9�N
                Me.GrpQrCode.Visible = True
            End If
            '----- V4.11.0.0�A�� -----

            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            ' �I������
            Call TrimStateOff()                                         ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��

            'V4.10.0.0�B���O�ׂ̈����ł��ݒ�
            gbCorrectDone = False                                       ' �I�y���[�^�e�B�[�`���O�ȑf�� ��x�w�x���␳���s�������͎��{���Ȃ��ׂɎg�p
            gbIntegratedMode = False                                    ' ���݈ꊇ�e�B�[�`���O���[�h�̎� True
            gbSubstExistChkDone = False                                 ' �I�y���[�^�e�B�[�`���O�ȑf�� ��x��݉׃`�F�b�N���s�������͎��{���Ȃ��ׂɎg�p
            'V4.10.0.0�B��

            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1�C��

            ' �V�O�i���^���[����(On=���f�B, Off=�S�ޯ�) ###007
            'V5.0.0.9�M �� V6.0.3.0�G
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(���_��)
                    ' Call Me.System1.SetSignalTower(0, &HEFFF)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                Case SIGTOWR_SPCIAL                                     ' ����(�Ȃ�)

            End Select
            'V5.0.0.9�M �� V6.0.3.0�G

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call SimpleTrimmer.Sub_StartResetButtonDispOn()
            End If
#End If
            Return (r)                                                  ' Return�l�ݒ� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)

        Finally
            SetActiveJogMethod(Nothing, Nothing, Nothing)               'V6.0.0.0�J  �ʏ�͕s�v
            Me.txtLog.ScrollToCaret()
        End Try

    End Function
#End Region

#Region "�e�R�}���h���s����"
    ''' =========================================================================
    ''' <summary>�e�R�}���h���s����</summary>
    ''' <param name="iAppMode">(INP)����Ӱ��(giAppMode�Q��)</param>
    ''' <param name="objForm"> (INP)Form�I�u�W�F�N�g</param>
    ''' <param name="prm">     (INP)����(�O���J�����e�B�[�`���O�p)</param>
    ''' <param name="pltInfo"> (I/O)�v���[�g�f�[�^</param>
    ''' <remarks></remarks>
    ''' =========================================================================
    Private Function CmdCall_Proc(ByVal iAppMode As Short, ByVal objForm As ICommonMethods, ByVal prm As Integer, ByRef pltInfo As PlateInfo) As Short
        'V6.0.0.0�J    Private Function CmdCall_Proc(ByRef iAppMode As Short, ByRef objForm As Object, ByRef prm As Integer, ByVal pltInfo As PlateInfo) As Short

        Dim strMSG As String
        Dim r As Short

        Try
            '--------------------------------------------------------------------------
            '   �ƕ␳(��߼��) & XY�e�[�u�����g�����ʒu�ֈړ�����
            '--------------------------------------------------------------------------
            CmdCall_Proc = cFRS_NORMAL                                  ' Return�l = ����
            'Call BSIZE(stPLT.zsx, stPLT.zsy)                           ' �u���b�N�T�C�Y�ݒ�
            r = Move_Trimposition(pltInfo, 0.0, 0.0)                    ' �ƕ␳(��߼��) & XYð�����шʒu�ړ�
            SetTeachVideoSize()                                         ' V6.0.3.0_49
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                CmdCall_Proc = r                                        ' Return�l�ݒ�
                Exit Function
            End If

            ' BP�̾�Đݒ�
            'Call System1.EX_BPOFF(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)
            'If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' �X�^�[�g�X�C�b�`���b�`�N���A   ###082
            Call ZCONRST()

            '--------------------------------------------------------------------------
            '   �e�R�}���h���������s����
            '--------------------------------------------------------------------------
            'V6.0.0.0�F      APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH �����邱�Ƃ͂Ȃ�
            ' �O���J����R1�e�B�[�`���O/�O���J�����e�B�[�`���O���͎�ʂ������Ƃ��ēn��
            'V6.0.0.0�F            If (iAppMode = APP_MODE_EXCAM_R1TEACH) Or (iAppMode = APP_MODE_EXCAM_TEACH) Then
            'V6.0.0.0�FCall objForm.ShowDialog(Me, prm)
            'V6.0.0.0�F            Else
            'V6.0.0.0�F            Call objForm.ShowDialog()
            'V6.0.0.0�J            objForm.Show(Me)            'V6.0.0.0�F
            TryCast(objForm, Form).Show(Me)                             'V6.0.0.0�J
            'V6.0.0.0�F            End If

            'V6.0.0.0�J            CmdCall_Proc = objForm.sGetReturn()                         ' Return�l = �R�}���h�I������
            'V6.0.0.0�L            CmdCall_Proc = objForm.sGetReturn                           ' Return�l = �R�}���h�I������    'V6.0.0.0�J
            CmdCall_Proc = objForm.Execute()                            'V6.0.0.0�L

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCall_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CmdCall_Proc = cERR_TRAP                                    ' Return�l = ��O�G���[

        Finally                         'V6.0.0.0�J
            SetActiveJogMethod(Nothing, Nothing, Nothing)               ' �ʏ�͕s�v
        End Try

    End Function
#End Region

#Region "�e�R�}���h���s�O����"
    '''=========================================================================
    '''<summary>�e�R�}���h���s�O����</summary>
    '''<param name="iAppMode">(INP) ����Ӱ��(giAppMode�Q��)</param>
    ''' <returns>  0  = ����
    '''            3  = Reset SW����
    '''            ��L�ȊO�̃G���[
    ''' </returns>
    '''<remarks>����~/�W�o�@�ُ펞�͓��֐����ſ�ċ����I������</remarks>
    '''=========================================================================
    Private Function CmdExec_Init(ByRef iAppMode As Short) As Short

        Dim strMSG As String
        Dim r As Short

        Try
            '' �R�}���h���s�O�̃`�F�b�N CmdExec_Proc()�Ɉړ�
            'r = CmdExec_Check(iAppMode)
            'If (r <> cFRS_NORMAL) Then                      ' �`�F�b�N�G���[ ?
            '    Return (r)                                  ' Return�l = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
            'End If

            gbInitialized = False                           ' ���_���A��
            'V6.0.0.0�I            Me.KeyPreview = False                           ' ̧ݸ��ݷ�����
            Call Form1Button(0)                             ' �R�}���h�{�^���𖳌��ɂ���
            Call SetVisiblePropForVideo(False)
            Me.GrpQrCode.Visible = False                    ' QR�R�[�h���\�����\�� 
            Me.GrpStartBlk.Visible = False                  '�g���~���O�J�n�u���b�N�ԍ���\�� V4.11.0.0�D

            ' �W�o�@�ُ�`�F�b�N
            r = System1.CheckDustVaccumeAlarm(gSysPrm)
            If (r <> cFRS_NORMAL) Then                      ' �G���[�Ȃ�W�o�@�ُ팟�o���b�Z�[�W�\��
                Call System1.Form_Reset(cGMODE_ERR_DUST, gSysPrm, iAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                GoTo STP_ERR_EXIT                           ' ��ċ����I��
            End If

            If gbAutoCalibration Then                                   'V6.1.4.2�@[�����L�����u���[�V�����␳���s]
                r = cFRS_NORMAL                                         'V6.1.4.2�@
            Else                                                        'V6.1.4.2�@
                ' �e�R�}���h�������s���̽ײ�޶�ް�����۰�ޖ���START/RESET�L�[�����҂�
                r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            End If
            Me.Refresh()
            If (r <= cFRS_ERR_EMG) Then                     ' �װ(����~��)�Ȃ��ċ����I��
                GoTo STP_ERR_EXIT
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                   ' Return�l = �ׯ�ߴװ����
        End Try

        Return (r)
        Exit Function

        ' ��ċ����I������
STP_ERR_EXIT:
        Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
        Call AplicationForcedEnding()                       ' ��ċ����I������
        Return (r)
    End Function
#End Region

#Region "�e�R�}���h���s�O�̃`�F�b�N����"
    '''=========================================================================
    '''<summary>�e�R�}���h���s�O�̃`�F�b�N����</summary>
    '''<param name="iAppMode">(INP) ����Ӱ��(giAppMode�Q��)</param>
    ''' <returns>  0  = ����
    '''            3  = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
    '''            ��L�ȊO�̃G���[
    ''' </returns>
    '''=========================================================================
    Private Function CmdExec_Check(ByRef iAppMode As Short) As Short

        Dim bFlg As Boolean
        Dim Rn As Integer
        Dim r As Short
        Dim Gn As Short
        Dim RnBn As Short
        Dim DblChipSz As Double
        Dim strMSG As String = ""

        Try
            '// 'V4.0.0.0-90
            CommandEnableSet(False)

            '-------------------------------------------------------------------
            '   �R�}���h���s�O�̃`�F�b�N���s��
            '-------------------------------------------------------------------
            '----- V1.13.0.0�B�� -----
            ' ���s�O�̃`�F�b�N�ŃG���[�ɂȂ�ƃN�����vON�̂܂܂ƂȂ�̂ōŌ�Ɉړ�

            ''----- ###240��-----
            '' �ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��(OPTION))
            'If (iAppMode <> APP_MODE_LASER) Then                    ' ���[�U�R�}���h��NO CHECK 
            '    giAppMode = iAppMode                                ' ###256  
            '    r = SubstrateExistCheck(System1)
            '    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? 
            '        If (r = cFRS_ERR_RST) Then                      ' �����(Cancel(RESET��)�@?
            '            giAppMode = APP_MODE_IDLE
            '            Timer1.Enabled = True                       ' ###256 �Ď��^�C�}�[�J�n
            '            Return (cFRS_ERR_RST)                       ' Return�l = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
            '        End If
            '        Call AppEndDataSave()                           ' ��ċ����I�������ް��ۑ��m�F
            '        Call AplicationForcedEnding()                   ' ��ċ����I������
            '        Return (r)
            '    End If
            'End If
            ''----- ###240��-----
            '----- VV1.13.0.0�B�� -----
            '----- V3.0.0.0�C(V1.22.0.0�B)�� -----
            ' �f�[�^���[�h�ς݃`�F�b�N
            If (iAppMode <> APP_MODE_LASER) Then                        ' ���[�U�R�}���h��NO CHECK 
                If ChkTrimDataLoaded() <> cFRS_NORMAL Then
                    Return (cFRS_ERR_RST)                               ' Return�l = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
                End If
            End If
            '----- V3.0.0.0�C(V1.22.0.0�B)�� -----
            '----- V1.19.0.0-33�� -----
            ' �v���[�u�R�}���h��
            If (iAppMode = APP_MODE_PROBE) Then
                ' �L���Ȓ�R�ԍ�(1-999)�����邩�`�F�b�N����
                r = CheckValidRegistance(APP_MODE_PROBE)                ' �L���Ȓ�R�ԍ�(1-999)�Ȃ� ? 
                If (r <> cFRS_NORMAL) Then
                    strMSG = ERR_PROB_EXE                               ' "�L���Ȓ�R(1-999)�f�[�^���Ȃ����߂��̃R�}���h�͎��s�ł��܂���I"
                    GoTo STP_ERR_EXIT                                   ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If

            ' �e�B�[�`���O�R�}���h��
            If (iAppMode = APP_MODE_TEACH) Then
                ' �L���Ȓ�R�ԍ�(1-999),���̓}�[�L���O��R�����邩�`�F�b�N����
                r = CheckValidRegistance(APP_MODE_TEACH)
                If (r <> cFRS_NORMAL) Then                              ' �L���Ȓ�R�ԍ�(1-999),���̓}�[�L���O��R�Ȃ� ? 
                    strMSG = ERR_TEACH_EXE                              ' "�L���Ȓ�R(1-999)���̓}�[�L���O��R�f�[�^���Ȃ����߂��̃R�}���h�͎��s�ł��܂���I"
                    GoTo STP_ERR_EXIT                                   ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If
            '----- V1.19.0.0-33�� -----
            '----- V2.0.0.0�C�� -----
            ' ���H�͈͂̃`�F�b�N���s��
            If (iAppMode = APP_MODE_PROBE) Or (iAppMode = APP_MODE_TEACH) Then
                ' ���H�͈͂̃`�F�b�N
                r = Bp_Limit_Check(False, strMSG)
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_ERR_EXIT                                   ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If
            '----- V2.0.0.0�C�� -----

            ' �J�b�g�ʒu�␳(TKY�p)�R�}���h��
            If (iAppMode = APP_MODE_CUTPOS) Then
                ' �J�b�g�ʒu�␳�Ώۂ̒�R�����邩�`�F�b�N����
                bFlg = False
                For Rn = 1 To gRegistorCnt                          ' ��R������������
                    ' �J�b�g�ʒu�␳�����R ?
                    If (typResistorInfoArray(Rn).intCutReviseMode = 1) Then
                        bFlg = True
                        Exit For
                    End If
                Next Rn

                ' �J�b�g�ʒu�␳�Ώۂ̒�R���Ȃ��ꍇ�͏������Ȃ�
                If (bFlg = False) Then
                    strMSG = MSG_39                                 ' "�J�b�g�ʒu�␳�Ώۂ̒�R������܂���"
                    GoTo STP_ERR_EXIT                               ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If

            ' T�ƃR�}���h��
            If (iAppMode = APP_MODE_TTHETA) Then
                ' �ƕ␳�Ȃ��̏ꍇ�͏������Ȃ�
                If typPlateInfo.intReviseMode = 1 And typPlateInfo.intManualReviseType = 0 Then
                    strMSG = MSG_118                                ' "�␳���[�h�������ɐݒ肳��Ă���ꍇ�́A���s�ł��܂���B"
                    GoTo STP_ERR_EXIT                               ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If

            ' �s�w/�s�x�R�}���h��
            If (iAppMode = APP_MODE_TX) Or (iAppMode = APP_MODE_TY) Then
                ' ��R���܂��̓u���b�N�����擾����
                r = GetChipNumAndSize(iAppMode, Gn, RnBn, DblChipSz)

                ' �s�w�R�}���h�Œ�R�����1��̏ꍇ�͏����ł��Ȃ��ׁA�G���[�Ƃ���
                If (iAppMode = APP_MODE_TX) And (RnBn <= 1) Then
                    strMSG = ERR_TXNUM_E                            ' "��R�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
                    GoTo STP_ERR_EXIT                               ' ���b�Z�[�W�\����G���[�߂�
                End If

                ' �s�x�R�}���h�Ńu���b�N�����1��̏ꍇ�͏����ł��Ȃ��ׁA�G���[�Ƃ���
                If (iAppMode = APP_MODE_TY) And (RnBn <= 1) Then
                    strMSG = ERR_TXNUM_B                            ' "�u���b�N�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
                    GoTo STP_ERR_EXIT                               ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If

            ' �T�[�L�b�g�e�B�[�`�R�}���h��(NET�p)
            If (iAppMode = APP_MODE_CIRCUIT) Then
                ' �T�[�L�b�g�� > 1�ł��邩�`�F�b�N����
                If (typPlateInfo.intCircuitCntInBlock < 2) Then
                    strMSG = ERR_TXNUM_C                            ' "�T�[�L�b�g�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
                    GoTo STP_ERR_EXIT                               ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If

            '----- V1.13.0.0�B�� -----
            ' �I�[�g�v���[�u���s/�摜�o�^�R�}���h��
            If (iAppMode = APP_MODE_APROBEEXE) Or (iAppMode = APP_MODE_APROBEREC) Then
                ' �X�e�b�v����� > 1�ł��邩�`�F�b�N����
                If (typPlateInfo.intStepMeasCnt < 1) Then
                    strMSG = ERR_TXNUM_S                            ' "�X�e�b�v����񐔂�0�̂��߂��̃R�}���h�͎��s�ł��܂���I"
                    GoTo STP_ERR_EXIT                               ' ���b�Z�[�W�\����G���[�߂�
                End If
            End If
            '----- V1.13.0.0�B�� -----

            '----- V1.13.0.0�B�� -----
            ' �ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��(OPTION))
            '            If (iAppMode <> APP_MODE_LASER) Then                    ' ���[�U�R�}���h��NO CHECK 
            If ((iAppMode <> APP_MODE_LASER) And (iAppMode <> APP_MODE_MAINTENANCE)) Then                    ' ���[�U�R�}���h��NO CHECK (IO�����e�i���X���`�F�b�N���Ȃ�)
                giAppMode = iAppMode                                ' ###256  
                'V6.1.2.0�B Return (cFRS_NORMAL)                                'V4.10.0.0�B
                If Not gbSubstExistChkDone Then                     'V4.10.0.0�B

                    r = SubstrateExistCheck(System1)
                    If (r <> cFRS_NORMAL) Then                          ' �G���[ ? 
                        If (r = cFRS_ERR_RST) Then                      ' �����(Cancel(RESET��)�@?
                            giAppMode = APP_MODE_IDLE
                            Timer1.Enabled = True                       ' ###256 �Ď��^�C�}�[�J�n
                            Return (cFRS_ERR_RST)                       ' Return�l = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
                        End If
                        Call AppEndDataSave()                           ' ��ċ����I�������ް��ۑ��m�F
                        Call AplicationForcedEnding()                   ' ��ċ����I������
                        Return (r)
                    End If
                    'V4.10.0.0�B��
                    If gbIntegratedMode Then                                ' ���݈ꊇ�e�B�[�`���O���[�h�̎� True
                        gbSubstExistChkDone = True                                ' �I�y���[�^�e�B�[�`���O�ȑf�� ��x�w�x���␳���s�������͎��{���Ȃ��ׂɎg�p
                    End If
                    'V4.10.0.0�B��
                End If                              'V4.10.0.0�B
            End If
            '----- VV1.13.0.0�B�� -----

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            '-------------------------------------------------------------------
            '   ���b�Z�[�W�\����G���[�߂�
            '-------------------------------------------------------------------
STP_ERR_EXIT:
            '----- ###235�� -----
            'MsgBox(strMSG, MsgBoxStyle.Exclamation)
            giAppMode = APP_MODE_LOAD                               ' �A�C�h�����[�h����START�L�[�Ńg���~���O���s����̂�LOAD���[�h�Ƃ��� 
            Call System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, TITLE_4)
            Call ZCONRST()                                          ' ���b�`����
            giAppMode = APP_MODE_IDLE
            Return (cFRS_ERR_RST)                                   ' Return�l = �`�F�b�N�G���[(Cancel(RESET��)��Ԃ�)
            '----- ###235�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Check() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�e�R�}���h���s�㏈��"
    '''=========================================================================
    '''<summary>�e�R�}���h���s�㏈��</summary>
    '''<param name="iAppMode">(INP) ����Ӱ��(giAppMode�Q��)</param>
    '''<param name="sts">     (INP) �R�}���h���s�X�e�[�^�X(�G���[�ԍ�)</param> 
    '''<remarks>STS������~/�W�o�@�ُ펞�͓��֐����ſ�ċ����I������</remarks>
    '''=========================================================================
    Private Sub CmdExec_Term(ByRef iAppMode As Short, ByRef sts As Short)

        Dim strMSG As String
        Dim r As Short

        Try
            ' �e�R�}���h�̎��s�X�e�[�^�X���`�F�b�N����
            'frmInfo.Visible = True                          ' ���ʕ\����\��
            If (sts < cFRS_NORMAL) Then                     ' �R�}���h���s�G���[ ?
                If (sts = cFRS_ERR_PTN) Then                ' �p�^�[���F���G���[ ?

                ElseIf (sts < cFRS_NORMAL) Then             ' ��ċ����I�����ق̴װ ?
                    ' �N�����v/�z��OFF
                    If (iAppMode = APP_MODE_LOADERINIT) Then GoTo STP_ERR_EXIT
                    r = System1.ClampVacume_Ctrl(gSysPrm, 0, iAppMode, giTrimErr)
                    If (r <> cFRS_ERR_EMG) Then
                        strMSG = "CmdExec_Term::ClampError occured" + vbCrLf + "Error code=" + r.ToString
                    End If
                    GoTo STP_ERR_EXIT                       ' ��ċ����I��
                End If
            End If

            ' �e�[�u�����_�ړ�
            If (iAppMode = APP_MODE_LOADERINIT) Then Exit Sub
            r = System1.Form_Reset(cGMODE_ORG_MOVE, gSysPrm, iAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            If (r <= cFRS_ERR_EMG) Then
                strMSG = "CmdExec_Term::System1.Form_Reset" + vbCrLf + "Error code=" + r.ToString
                GoTo STP_ERR_EXIT ' �G���[�Ȃ��ċ����I��
            End If

            ' �Ƃ����_�ɖ߂�(giThetaParam�́u�ʒu�␳���[�h=1(�蓮)�ŕ␳���@=0(�␳)�Ȃ��v���L��)
            '   giThetaParam = (0=�Ƃ����_�ɖ߂��Ȃ�(�W��), 1=�Ƃ����_�ɖ߂�)
            '   �E�ʒu�␳���[�h=0(����),2(����+����)�̎��͖������ɃƂ����_�ɖ߂�
            '   �E�ʒu�␳���[�h=1(�蓮)��, �␳���@=1(1��̂�)�͌��_�ɖ߂��Ȃ�
            '                                        2(����)�͌��_�ɖ߂� ###233
            If (gSysPrm.stDEV.giTheta <> 0) Then            ' �ƗL�� ?
                If (typPlateInfo.intReviseMode = 0) Or (typPlateInfo.intReviseMode = 2) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 2))) Then
                    '###233 ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1)) Then
                    Call ROUND4(0.0#)                       ' �Ƃ����_�ɖ߂�
                    '----- V1.19.0.0-32   �� -----
                    gfCorrectPosX = 0.0                     ' �␳�l������ 
                    gfCorrectPosY = 0.0
                    '----- V1.19.0.0-32   �� -----
                End If
            End If

            ' SL432�n�̏ꍇ�̏���
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then
                ''----- V1.13.0.0�G�� -----  V2.0.0.0�G(OcxSystem�ŋz��)
                '' ��񌴓_�ʒu�ɃX�e�[�W���ړ�����
                'If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                '    r = System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                '    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' �G���[�Ȃ�\�t�g�����I��
                'End If
                ''----- V1.13.0.0�G�� -----

                If gbAutoCalibration Then                                   'V6.1.4.2�@[�����L�����u���[�V�����␳���s]
                Else                                                        'V6.1.4.2�@
                    '�X���C�h�J�o�[�����I�[�v��
                    If (gSysPrm.stSPF.giWithStartSw = 0) Then       ' �X�^�[�gSW�����҂��i�I�v�V�����j�łȂ� ?
                        r = System1.Z_COPEN(gSysPrm, iAppMode, giTrimErr, False)
                        If (r <= cFRS_ERR_EMG) Then GoTo STP_ERR_EXIT ' �G���[�Ȃ�\�t�g�����I��
                    Else
                        ' �C���^�[���b�N���Ȃ�X���C�h�J�o�[�J�҂�
                        If (System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0 Then
                            r = System1.Form_Reset(cGMODE_OPT_END, gSysPrm, iAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                            If (r <= cFRS_ERR_EMG) Then GoTo STP_ERR_EXIT ' �G���[�Ȃ��ċ����I��
                        End If
                    End If
                End If                                                      'V6.1.4.2�@

                '----- V2.0.0.0�G�� -----
            Else
                ' SL436S/SL436R�̏ꍇ����񌴓_�ʒu�ɃX�e�[�W���ړ�����
                If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                    r = System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' �G���[�Ȃ�\�t�g�����I��
                End If
                '----- V2.0.0.0�G�� -----
            End If

            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Term() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Exit Sub

        ' ��ċ����I������
STP_ERR_EXIT:
        '----- ###163�� -----
        ' ���[�_�A���[���Ϳ�ċ����I�����Ȃ�
        If (sts = cFRS_ERR_LDR) Or (sts = cFRS_ERR_LDR1) Or (sts = cFRS_ERR_LDR2) Or (sts = cFRS_ERR_LDR3) Or (sts = cFRS_ERR_LDRTO) Then
            Exit Sub
        End If
        '----- ###163�� -----
        Call AppEndDataSave()                           ' ��ċ����I�������ް��ۑ��m�F
        Call AplicationForcedEnding()                   ' ��ċ����I������
    End Sub
#End Region

#Region "�g���}���u��Ԃ𓮍쒆�ɐݒ肷��"
    '''=========================================================================
    '''<summary>�g���}���u��Ԃ𓮍쒆�ɐݒ肷��</summary>
    '''<param name="IdxFnc">  (INP)�@�\�I���`�e�[�u���̲��ޯ��</param>
    '''<param name="iAppMode">(INP)����Ӱ��(giAppMode�Q��)</param> 
    '''<param name="strLOG">  (INP)����۸�ү����</param>
    '''<param name="strNote"> (INP)����۸޺���</param>
    '''<returns>0=���� ,0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function TrimStateOn(ByVal IdxFnc As Short, ByRef iAppMode As Short,
                                ByRef strLOG As String, ByRef strNote As String,
                                Optional ByVal shiftKeyDown As Boolean = False) As Short    '#4.12.2.0�D
        '#4.12.2.0�D    Public Function TrimStateOn(ByVal IdxFnc As Short, ByRef iAppMode As Short, ByRef strLOG As String, ByRef strNote As String) As Short

        Dim r As Short
        Dim rslt As Short
        Dim strMSG As String
        Dim stPos As System.Drawing.Point

        Try
            '// 'V4.0.0.0-90
            CommandEnableSet(False)

            ' ���[�_�փg���}���쒆�M���𑗐M����
            TrimStateOn = cFRS_NORMAL                                   ' Return�l = ����
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ? ###035
                Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ���[�_�o��(ON=�g���}���쒆, OFF=�Ȃ�)
            Else
                Call SetLoaderIO(&H0, LOUT_STOP)                        ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}��~��)
            End If
            giAppMode = iAppMode                                        ' ����Ӱ�ނ��۰��ٕϐ��ɐݒ�

            ' �f�[�^�ҏW���܂��̓��[�h�σ`�F�b�N
            If (giAppMode = APP_MODE_LOGGING) OrElse
                (giAppMode = APP_MODE_LASER) OrElse
                (giAppMode = APP_MODE_AUTO) OrElse
                (giAppMode = APP_MODE_LOADERINIT) Then                  ' ���M���O, ���[�U, �����^�], ���[�_���_���A�R�}���h�̏ꍇ�̓`�F�b�N�Ȃ�
                '#4.12.2.0�D    (giAppMode = APP_MODE_EDIT) Then

            ElseIf (giAppMode = APP_MODE_MAINTENANCE) Then              ' V3.1.0.0�@ 2014/11/28 �����e�i���X�Ńg���~���O�f�[�^���[�h���Ȃ��悤�ɂ���B

            ElseIf (giAppMode = APP_MODE_LOAD) Then                     ' ���[�h�R�}���h�̏ꍇ 
                If gCmpTrimDataFlg = 1 Then                             ' ���݂̃f�[�^���X�V����Ă��邩�`�F�b�N����
                    If (gLoadDTFlag = True) Then
                        ' "�ҏW���̃f�[�^������܂��B���[�h���܂����H"�@�@�@�@�@
                        r = MsgBox(MSG_115, MsgBoxStyle.OkCancel, MSG_116)
                    Else
                        ' "�t�@�C�o���[�U�̉��H������ύX���Ă���\��������܂��B���[�h���܂����H"�@�@�@�@�@
                        r = MsgBox(MSG_149, MsgBoxStyle.OkCancel, MSG_116)
                    End If
                    If (r = MsgBoxResult.Cancel) Then                   ' Cansel�w�� ?
                        TrimStateOn = cFRS_ERR_RST
                        GoTo STP_END
                    End If
                End If
                '#4.12.2.0�D             ��
            ElseIf (APP_MODE_EDIT = giAppMode) Then                     ' �ҏW���
                If (shiftKeyDown) Then
                    ' shift�L�[�����ɂ��V�K�쐬�ҏW��ʋN���̏ꍇ
                    If gCmpTrimDataFlg = 1 Then                         ' ���݂̃f�[�^���X�V����Ă��邩�`�F�b�N����
                        If (gLoadDTFlag = True) Then
                            ' "�ҏW���̃f�[�^������܂��B�V�K�Ƀf�[�^���쐬���܂����H"
                            r = MsgBox(MSG_110, MsgBoxStyle.OkCancel, MSG_109)
                        Else
                            ' "�t�@�C�o���[�U�̉��H������ύX���Ă���\��������܂��B�V�K�Ƀf�[�^���쐬���܂����H"
                            r = MsgBox(MSG_111, MsgBoxStyle.OkCancel, MSG_109)
                        End If
                        If (r = MsgBoxResult.Cancel) Then               ' Cansel�w�� ?
                            TrimStateOn = cFRS_ERR_RST
                            GoTo STP_END
                        End If
                    End If
                End If
                '#4.12.2.0�D             ��
            Else                                                        ' ���̑��̃R�}���h�̏ꍇ
                If (gLoadDTFlag = False) Then                           ' ���ݸ��ް�̧�ٖ�۰�� ?
                    ' "�g���~���O�p�����[�^�f�[�^�����[�h���邩�V�K�쐬���Ă�������"
                    Call System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    TrimStateOn = cFRS_ERR_RST
                    GoTo STP_END
                End If
            End If

            ' �p�X���[�h����(�I�v�V����)
            rslt = Func_Password(IdxFnc)
            If (rslt <> True) Then
                '// 'V4.1.0.0�G
                CommandEnableSet(True)
                TrimStateOn = cFRS_ERR_RST
                GoTo STP_END                                            ' �߽ܰ�ޓ��ʹװ�Ȃ�EXIT
            End If

            SetMapOnOffButtonEnabled(False)                             'V4.12.2.0�@
            SetTrimMapVisible(False)                                    'V4.12.2.0�@

            ' �N�����v/�z��ON(۰��/���޺���ޓ��ȊO) '###001
            ' V3.1.0.0�@ 2014/11/28 �����e�i���X�̎���ClampVacume_Ctrl�֐����R�[�����Ȃ��B�i�����ǉ��j
            If (giAppMode <> APP_MODE_LOAD) And (giAppMode <> APP_MODE_SAVE) And (giAppMode <> APP_MODE_EDIT) And (giAppMode <> APP_MODE_LOGGING) And
               (giAppMode <> APP_MODE_AUTO) And (giAppMode <> APP_MODE_LOADERINIT) And (giAppMode <> APP_MODE_MAP) And
               (giAppMode <> APP_MODE_MAINTENANCE) Then     ' 'V1.13.0.0�D

                'V4.5.0.1�@��
                '����Ή��̏ꍇ�ɂ́A�N�����v�V�[�P���X�ύX���� 
                'V4.5.0.1�A��
                '                If (giClampSeq = 1) And (giAppMode = APP_MODE_LASER) Then
                If (giClampSeq = 1) And (giAppMode <> APP_MODE_LASER) Then
                    'V4.5.0.1�A��

                    Dim lData As Long = 0
                    Dim lBit As Long = 0
                    Dim rtn As Integer = cFRS_NORMAL
                    Dim strMS2 As String = ""
                    Dim strMS3 As String = ""
                    Dim bFlg As Boolean = True
RETRY:
                    ' �ڕ���Ɋ�����鎖���`�F�b�N����
                    '�N�����vON
                    ''----- V1.16.0.0�K�� -----
                    'If (gSysPrm.stIOC.giClamp = 1) Then
                    '    Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                    '    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                    '    Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
                    'End If
                    '----- V1.16.0.0�K�� -----
                    '�z��ON
                    Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
                    System.Threading.Thread.Sleep(500)                          ' Wait(ms) ��200ms���ƃ��[�N�L�����o����Ȃ��ꍇ������
                    If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                        ' SL436S��
                        r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
                        lBit = LDSTS_VACUME
                    Else
                        ' SL436R��
                        r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                        lBit = LDST_VACUME
                    End If
                    ' ���[�N�����Ȃ烁�b�Z�[�W�\��
                    If (lData And lBit) Then                                    ' �ڕ���Ɋ�L ? V2.0.0.0�E
                        r = 0
                    Else
                        ' �u�Œ�J�o�[�J�P�`�F�b�N�Ȃ��v�ɂ���
                        Call COVERCHK_ONOFF(COVER_CHECK_OFF)
                        '�N�����v�J�A�z��OFF
                        r = System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)

                        ' �\�����b�Z�[�W��ݒ肷��
                        strMSG = MSG_LDALARM_06                                 ' "�ڕ���z���~�X"
                        strMS2 = MSG_SPRASH52                                   ' "���u���čēx���s���ĉ�����"
                        strMS3 = MSG_SPRASH53                                             ' ""

                        'r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        '        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        ' ���b�Z�[�W�\��(START�L�[�����҂�)
                        r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                                strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        If r = 1 Then ' START��Retry
                            If (gSysPrm.stIOC.giClamp = 1) Then
                                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
                            End If

                            GoTo RETRY
                        ElseIf r = 3 Then
                            ' Cancel�������ꂽ��I��
                            TrimStateOn = cFRS_ERR_RST
                            GoTo STP_END
                        End If
                    End If
                Else
                    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)     'V5.0.0.8�E
                    r = System1.ClampVacume_Ctrl(gSysPrm, 1, giAppMode, 1)
                    TrimStateOn = r
                End If
                'V4.5.0.1�@��

                'V6.0.0.0�E                  ��
                If (VideoLibrary1.SetTrackBarVisible(True)) Then
                    Dim sz As New Size(0, 30)
                    'V6.0.1.0�J                    frmHistoryData.Location = Point.Add(frmHistoryData.Location, sz)
                    frmHistoryData.Location = Point.Add(HistoryDataLocation, sz)    'V6.0.1.0�J
                    grpIntegrated.Location = Point.Add(grpIntegrated.Location, sz)
                End If
                'V6.0.0.0�E                  ��

            End If

            ' ���샍�O�o�� V1.13.0.0�B
            If gbAutoCalibration = False Then                          'V6.1.4.2�@[�����L�����u���[�V�����␳���s]�łȂ���
                Call System1.OperationLogging(gSysPrm, strLOG, "")
            End If                                                      'V6.1.4.2�@

            '----- V1.18.0.1�@�� -----
            ' ��ʂɃR�}���h����\������
            If (giDspCmdName = 1) Then
                Select Case (IdxFnc)
                    ' �I�v�V�����R�}���h�̏ꍇ
                    Case F_EXR1                                             ' �O�����R1è��ݸ�
                        LblComandName.Text = MSG_F_EXR1
                    Case F_EXTEACH                                          ' �O�����è��ݸ�
                        LblComandName.Text = MSG_F_EXTEACH
                    Case F_CARREC                                           ' �����ڰ��ݕ␳�o�^
                        LblComandName.Text = MSG_F_CARREC
                    Case F_CAR                                              ' �����ڰ���
                        LblComandName.Text = MSG_F_CAR
                    Case F_CUTREC                                           ' �O����׶�Ĉʒu�␳�o�^
                        LblComandName.Text = MSG_F_CUTREC
                    Case F_CUTREV                                           ' �O����׶�Ĉʒu�␳
                        LblComandName.Text = MSG_F_CUTREV

                        ' ��L�ȊO�̓R�}���h�̃{�^������\��
                    Case Else
                        LblComandName.Text = strBTN(IdxFnc) + MSG_OPLOG_CMD ' ��) "�v���[�u" + "�R�}���h"
                End Select
                stPos.X = 800
                stPos.Y = 0
                LblComandName.Location = stPos
                LblComandName.Visible = True
                LblComandName.BringToFront()
            Else
                LblComandName.Text = ""
                LblComandName.Visible = False
            End If
            '----- V1.18.0.1�@�� -----

            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.TrimStateOn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            TrimStateOn = cERR_TRAP                         ' Return�l = �ׯ�ߴװ����
        End Try

STP_END:
    End Function

#End Region

#Region "�g���}���u��Ԃ��A�C�h�����ɐݒ肷��"
    '''=========================================================================
    '''<summary>�g���}���u��Ԃ��A�C�h�����ɐݒ肷��</summary>
    '''<returns>0=���� ,0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function TrimStateOff() As Short

        Dim strMSG As String
        Dim SndDat As UShort
        Dim r As Short

        Try
            'V6.1.4.2�@[�����L�����u���[�V�����␳���s]��
            If gbAutoCalibration Then
                Call SetVisiblePropForVideo(True)
                'V6.1.4.14�C��
                If (VideoLibrary1.SetTrackBarVisible(False)) Then
                    Dim sz As New Size(0, 30)
                    frmHistoryData.Location = Point.Subtract(frmHistoryData.Location, sz)
                    grpIntegrated.Location = Point.Subtract(grpIntegrated.Location, sz)
                End If
                'V6.1.4.14�C��
                Return (cFRS_NORMAL)
            End If
            'V6.1.4.2�@��

            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                ' �p�q�ǂݍ��ݏ�Ԃ̏�����
                ObjQRCodeReader.ResetQRReadFlag()                       ' �p�q�R�[�h���ǂݍ��ݏ�ԂɃ��Z�b�g
            End If
            '----- V6.1.4.0_22�� -----

            '//'V4.0.0.0-90
            CommandEnableSet(True)

            ' ��������
            TrimStateOff = cFRS_NORMAL                                  ' Return�l = ����
            If (giAppMode = APP_MODE_LOAD) Or (giAppMode = APP_MODE_SAVE) Then
                '                                                       ' ۰��/���޺���� 
                Call SetMousePointer(Me, False)                         ' �����v����(ϳ��߲��)
            ElseIf (giAppMode = APP_MODE_EDIT) Then                     ' EDIT ����� ?

            ElseIf (giAppMode = APP_MODE_AUTO) Then                     ' �����^�] ����� ?

            ElseIf (giAppMode = APP_MODE_LOADERINIT) Then               ' ���[�_���_���A ����� ? 

                'ElseIf (giAppMode <> APP_MODE_MAP) Then                ' MAP��� V1.16.0.0�C V1.13.0.0�D
            ElseIf (giAppMode = APP_MODE_MAP) Then                      ' MAP��� V1.16.0.0�C

            Else                                                        ' ���̑��̺���� ?
                ' �N�����v/�z��OFF(SL436R�p)
                r = System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)
                '----- V1.16.0.0�C�� -----
                If (r < cFRS_NORMAL) Then                               ' ��ċ����I�����ق̴װ ?
                    Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
                    Call AplicationForcedEnding()                       ' ��ċ����I������
                    Return (r)
                End If
                '----- V1.16.0.0�C�� -----
            End If

            ' �R���\�[���{�^���̃����v��Ԃ�ݒ肷��
            Call LAMP_CTRL(LAMP_START, False)                           ' START���ߏ��� 
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESET���ߏ��� 
            Call LAMP_CTRL(LAMP_Z, False)                               ' PRB����OFF (INTRTM�̃v���[�u�R�}���h�Ő��䂷��?)

            '----- V1.18.0.1�@�� -----
            ' ��ʂ̃R�}���h���\��������
            LblComandName.Visible = False
            '----- V1.18.0.1�@�� -----

            ' ���C����� �{�^���\����Ԃ̕ύX
            Call SetVisiblePropForVideo(True)
            If giHostMode = cHOSTcMODEcMANUAL Then
                Call Form1Button(1)                                     ' �R�}���h�{�^����L���ɂ���
            Else
                Call Form1Button(3)                                     ' ð���������(F2), END����(F10)�̂ݗL���ɂ���
            End If

            ' QR���ޏ��/�o�[�R�[�h���(���z��)��\��
            'V5.0.0.9�N            If (gSysPrm.stCTM.giSPECIAL = customROHM) Or (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then
            'V5.0.0.9�S            If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.Taiyo = BarCode_Data.Type) Then   'V5.0.0.9�N
            If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.None <> BarCode_Data.Type) Then   'V5.0.0.9�S
                Me.GrpQrCode.Visible = True
            End If

            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ���\������
            'V5.0.0.9�O            If (giStartBlkAss = 1) Then                                 ' �g���~���O�J�n�u���b�N�ԍ��w��̗L�� ?
            If (giStartBlkAss <> 0) Then                                ' �g���~���O�J�n�u���b�N�ԍ��w��̗L�� ?  'V5.0.0.9�O
                GrpStartBlk.Visible = True
            End If
            '----- V4.11.0.0�D�� -----

            '�}�g���b�N�X�\����ʂ��\���Ƃ���
            Me.GrpMatrix.Visible = False

            ' �g���}���u�A�C�h�����ɐݒ肷��
            '' '' ''Call ExpansionOnOff(True)                           ' ���O��ʊg�����ݗL��
            Call ZCONRST()                                              ' �ݿ�ٷ� ׯ�����

            ' ���[�_�փg���}��~���M���𑗐M����
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ? ###035
                Call SetLoaderIO(&H0, COM_STS_TRM_STATE)                ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}���쒆)

            Else                                                        ' SL436R
                If (gLoadDTFlag = True) Then                            ' �f�[�^���[�h�ςȂ�g���}�����f�B�M��ON 
                    'SndDat = LOUT_STOP + LOUT_REDY                     ' ���[�_�o�̓f�[�^ = �g���}��~�� + �g���}�����f�B '###098
                    SndDat = LOUT_STOP                                  ' ���[�_�o�̓f�[�^ = �g���}��~�� + �g���}�����f�B '###098
                Else
                    SndDat = LOUT_STOP                                  ' ���[�_�o�̓f�[�^ = �g���}��~��
                End If
                Call SetLoaderIO(SndDat, &H0)                           ' ���[�_�o��(ON=�g���}��~��(+ �g���}�����f�B), OFF=�Ȃ�)
            End If

            ' �Ď��^�C�}�[�J�n(���L�̃R�}���h�ȊO)
            If (giAppMode <> APP_MODE_IDLE) And
               (giAppMode <> APP_MODE_LOAD) And
               (giAppMode <> APP_MODE_SAVE) And
               (giAppMode <> APP_MODE_LOGGING) Then
                Timer1.Enabled = True                                   ' �Ď��^�C�}�[�J�n
            End If

            giAppMode = APP_MODE_IDLE                                   ' ����Ӱ�� = �g���}���u�A�C�h����
            If (gLoadDTFlag = True) Then
                Call LAMP_CTRL(LAMP_START, True)                        ' START����ON 
            End If

            'V5.0.0.9�M �� V6.0.3.0�G
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
            'V5.0.0.9�M �� V6.0.3.0�G

            'V6.0.0.0�E                  ��
            If (VideoLibrary1.SetTrackBarVisible(False)) Then
                Dim sz As New Size(0, 30)
                frmHistoryData.Location = Point.Subtract(frmHistoryData.Location, sz)
                grpIntegrated.Location = Point.Subtract(grpIntegrated.Location, sz)
            End If
            'V6.0.0.0�E                  ��

            SetMapOnOffButtonEnabled(True)                              'V4.12.2.0�@
            SetTrimMapVisible(_mapOn)                                   'V4.12.2.0�@

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.TrimStateOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            TrimStateOff = cERR_TRAP                                    ' Return�l = �ׯ�ߴװ����
        End Try

    End Function
#End Region

    '========================================================================================
    '   ���̑��̃{�^���������̏���
    '========================================================================================
#Region "TRIMMING�{�^������������"
    '''=========================================================================
    '''<summary>TRIMMING�{�^������������</summary>
    '''<remarks>��ʏ���TRIMMING�{�^���̓��͂��Ď����ATrimming���s����</remarks>
    '''=========================================================================
    Private Sub btnTrimming_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Try
            Dim r As Short

            'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "MANUAL")'###030

            ' ���C���̕\����ԕύX
            Call Form1Button(0)                             ' �R�}���h�{�^����񊈐����ɂ���
            ' �^�C�}�[��~
            Timer1.Enabled = False

            ' SL432R�Ž���SW�����҂����Ȃ��ꍇ�̓��b�Z�[�W�\������
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                '-----V3.0.0.0�F(V1.22.0.0�D) -----
                ' "���ӁI�I�I�@�X���C�h�J�o�[�������ŕ��܂��B"(Red,Blue)
                'r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH32, &HFF, &HFF0000)

                ' ���b�Z�[�W�\��(START�L�[�����҂�)
                r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                        MSG_SPRASH31, MSG_SPRASH32, "", System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then
                    Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
                    Call AplicationForcedEnding()                       ' ��ċ����I������
                    End                                                 ' �A�v�������I��
                End If
                '----- V3.0.0.0�F(V1.22.0.0�D) -----
            End If

            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "START BUTTON")

            ' �g���~���O
            r = Start_Trimming()

            ' �^�C�}�[�J�n
            Timer1.Enabled = True

            'Call ZCONRST()
        Catch ex As Exception
            Dim strMSG As String
            strMSG = "i-TKY.btnTrimming_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "Version�{�^������������"
    '''=========================================================================
    '''<summary>Version�{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub mnuHelpAbout_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuHelpAbout.Click

        Dim r As Short
        Dim pstPRM As DllAbout.HelpVersion.HelpVer_PARAM                            ' �o�[�W�������\���֐��p�p�����[�^(OCX�Œ�`)
        Dim strVER(3) As String
        Dim strMSG As String
        Dim EqType As String 'V4.0.0.0-77

        Try
            '----- ###068(�ǉ�)�������� -----
            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ?
                Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ���[�_�o��(ON=�g���}���쒆, OFF=�Ȃ�)
            Else
                Call SetLoaderIO(&H0, LOUT_STOP)                        ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}��~��)
            End If
            giAppMode = APP_MODE_LOGGING                                ' ����Ӱ�ނ��uVersion�\���v�ɐݒ�(�����M���O���[�h���g�p)

            ' �{�^������񊈐������� 
            GrpMode.Enabled = False                                     ' �R���\�[���X�C�b�`�O���[�v�{�b�N�X�񊈐���
            tabCmd.Enabled = False                                      ' �R�}���h�{�^���O���[�v�{�b�N�X�񊈐���
            mnuHelpAbout.Enabled = False                                ' About�{�^���񊈐��� 
            CmdCnd.Enabled = False                                      ' EDITLOCK�{�^���񊈐���
            cmdEsLog.Enabled = False                                    ' ESLog�{�^���񊈐���
            CMdIX2Log.Enabled = False                                   ' IX2Log�{�^���񊈐���
            BtnPrintOnOff.Enabled = False                               ' Print ON/OFF�{�^���񊈐���  V1.18.0.0�B
            BtnPrint.Enabled = False                                    ' Print�{�^���񊈐���        V1.18.0.0�B
            '----- ###068(�ǉ�)�����܂� -----

            ' �\����pstPRM�̔z��̏����� ���z��̗v�f����DllAbout.dll�Œ�`�Ɠ����ɂ���K�v����
            pstPRM.strTtl = New String(4) {}
            pstPRM.strModule = New String(20) {}
            pstPRM.strVer = New String(20) {}

            ' �o�[�W�������\���֐��p�p�����[�^��ݒ肷��
            pstPRM.iTtlNum = 3                                          ' �^�C�g��������̐�
            pstPRM.strTtl(0) = gAppName                                 ' �A�v���� 

            ''V6.0.2.0�F��
            Dim strVersion = GetPrivateProfileString_S("TMENU", "VERSION_NAME", SYSPARAMPATH, "")
            If strVersion.ToString().Trim <> "" Then
                EqType = strVersion
            Else
                'V4.0.0.0-77��
                If (giMachineKd = MACHINE_KD_RS) Then
                    EqType = MACHINE_TYPE_SL436S
                Else
                    EqType = gSysPrm.stTMN.gsKeimei
                End If
            End If
            ''V6.0.2.0�F��

            '            pstPRM.strTtl(1) = "LMP-" + gSysPrm.stTMN.gsKeimei + gSysPrm.stDEV.gsDevice_No + "-000 " + _
            pstPRM.strTtl(1) = "LMP-" + EqType + gSysPrm.stDEV.gsDevice_No + "-000 " +
                               My.Application.Info.Version.Major.ToString("0") & "." &
                               My.Application.Info.Version.Minor.ToString("0") & "." &
                               My.Application.Info.Version.Build.ToString("0") & "." &
                               My.Application.Info.Version.Revision.ToString("0")
            'V4.0.0.0-77��
            pstPRM.strTtl(2) = "(c) TOWA LASERFRONT CORP."

            '----- V3.0.0.0�A��(Ocx��Dll�ɖ��̕ύX) -----
            pstPRM.iVerNum = 19                                         ' �o�[�W�������̐� ###266
            pstPRM.strModule(0) = "RT MODULE"                           ' 1."RT MODULE"
            pstPRM.strVer(0) = DLL_PATH + "INTRIM_SL432.rta"

            pstPRM.strModule(1) = "DllTrimFnc.dll"                      ' 2."DllTrimFnc.dll"
            pstPRM.strVer(1) = DLL_PATH + pstPRM.strModule(1)

            pstPRM.strModule(2) = "DllSysprm.dll"                       ' 3."DllSysprm.dll"
            pstPRM.strVer(2) = DLL_PATH + pstPRM.strModule(2)

            pstPRM.strModule(3) = "DllSystem.dll"                       ' 4."DllSystem.dll"
            pstPRM.strVer(3) = DLL_PATH + pstPRM.strModule(3)

            pstPRM.strModule(4) = "DllAbout.dll"                        ' 5."DllAbout.dll"
            pstPRM.strVer(4) = DLL_PATH + pstPRM.strModule(4)

            pstPRM.strModule(5) = "DllUtility.dll"                      ' 6."DllUtility.dll"
            pstPRM.strVer(5) = DLL_PATH + pstPRM.strModule(5)

            pstPRM.strModule(6) = "DllLaserTeach.dll"                ' 7."DllLaserTeach.dll"
            pstPRM.strVer(6) = DLL_PATH + pstPRM.strModule(6)

            pstPRM.strModule(7) = "DllManualTeach.dll"                  ' 8."DllManualTeach.dll"
            pstPRM.strVer(7) = DLL_PATH + pstPRM.strModule(7)

            pstPRM.strModule(8) = "DllPassword.dll"                     ' 9.DllPassword.dll"
            pstPRM.strVer(8) = DLL_PATH + pstPRM.strModule(8)

            pstPRM.strModule(9) = "DllProbeTeach.dll"                '10."DllProbeTeach.dll"
            pstPRM.strVer(9) = DLL_PATH + pstPRM.strModule(9)

            pstPRM.strModule(10) = "DllTeach.dll"                       '11."DllTeach.dll"
            pstPRM.strVer(10) = DLL_PATH + pstPRM.strModule(10)

            pstPRM.strModule(11) = "DllVideo.dll"                       '12."DllVideo.dll"
            pstPRM.strVer(11) = DLL_PATH + pstPRM.strModule(11)

            pstPRM.strModule(12) = "DllJog.dll"                         '13."DllJog.dll" ###067
            pstPRM.strVer(12) = DLL_PATH + pstPRM.strModule(12)

            'Call Rs232c_GetVersion(strVER)                             ' �VDll(C#�ō쐬) 
            pstPRM.strModule(13) = "DllSerialIO.dll"                    '14."DllSerialIO.dll"
            pstPRM.strVer(13) = DLL_PATH + pstPRM.strModule(13)

            pstPRM.strModule(14) = "DllCndXMLIO.dll"                    '15."DllCndXMLIO.dll"
            pstPRM.strVer(14) = DLL_PATH + pstPRM.strModule(14)

            pstPRM.strModule(15) = "DllFLCom.dll"                       '16."DllFLCom.dll"
            pstPRM.strVer(15) = DLL_PATH + pstPRM.strModule(15)
            '----- V2.0.0.0_27�� -----
            If (giMachineKd = MACHINE_KD_RS) Then
                pstPRM.strModule(16) = "TrimDataEditorEx"               '17."TrimDataEditorEx.exe"
                pstPRM.strVer(16) = DLL_PATH + "TrimDataEditorEx.exe"
            Else
                pstPRM.strModule(16) = "TrimDataEditor"                 '17."TrimDataEditor.exe" ###067
                pstPRM.strVer(16) = DLL_PATH + "TrimDataEditor.exe"
            End If
            '----- V2.0.0.0_27�� -----
            pstPRM.strModule(17) = "FLSetup"                            '18."FLSetup.exe" ###067
            pstPRM.strVer(17) = DLL_PATH + "FLSetup.exe"

            '----- ###266�� -----
            pstPRM.strModule(18) = "DllTkyMsgGet.dll"                   '19."DllTkyMsgGet.dll"
            pstPRM.strVer(18) = DLL_PATH + pstPRM.strModule(18)
            '----- ###266�� -----
            '----- V3.0.0.0�A�� -----

            ' �o�[�W�������\���ʒu��ݒ肷��
            HelpVersion1.Left = Text4.Location.X                        ' Left = Text4�ʒu 
            HelpVersion1.Top = mnuHelpAbout.Location.Y                  ' Top  = Version���݈ʒu 

            ' �o�[�W�������\��
            HelpVersion1.Visible = True
            HelpVersion1.BringToFront()                                 ' �őO�ʂ֕\��

            r = HelpVersion1.Version_Disp(pstPRM)
            HelpVersion1.Visible = False

            '----- ###068(�ǉ�)�������� -----
            ' �I������
            Call TrimStateOff()                                         ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��

            ' �{�^���������������� 
            GrpMode.Enabled = True                                      ' �R���\�[���X�C�b�`�O���[�v�{�b�N�X������
            tabCmd.Enabled = True                                       ' �R�}���h�{�^���O���[�v�{�b�N�X������
            mnuHelpAbout.Enabled = True                                 ' About�{�^�������� 
            CmdCnd.Enabled = True                                       ' EDITLOCK�{�^��������
            cmdEsLog.Enabled = True                                     ' ESLog�{�^��������
            CMdIX2Log.Enabled = True                                    ' IX2Log�{�^��������
            BtnPrintOnOff.Enabled = True                               ' Print ON/OFF�{�^��������  V1.18.0.0�B
            BtnPrint.Enabled = True                                    ' Print�{�^��������        V1.18.0.0�B
            '----- ###068(�ǉ�)�����܂� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.mnuHelpAbout_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "CLR�{�^������������"
    '''=========================================================================
    '''<summary>CLR�{�^������������</summary>
    '''<remarks>���Y����ؾ��</remarks>
    '''=========================================================================
    Private Sub btnCounterClear_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnCounterClear.Click

        Dim r As Short
        Try

            r = MsgBox(MSG_108, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)          ' �݌v���N���A���Ă���낵���ł����H
            If (r = MsgBoxResult.Yes) Then
                Call System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
                Call ClearCounter(1)                        ' ���Y�Ǘ��f�[�^�̃N���A
                Call ClrTrimPrnData()                       ' ���ݸތ��ʈ�����ڂ��ް���ر����

                '���v�\����ON�̏ꍇ�A�\�����X�V����
                If chkDistributeOnOff.Checked = True Then
                    gObjFrmDistribute.RedrawGraph()
                End If
                'Call ClearAvgDevCount()                    ' ###198 ###154 
            End If
            ''V4.12.2.2�E��'V6.0.2.0�C
            GraphDispSet()
            ''V4.12.2.2�E��'V6.0.2.0�C

        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "ES Log���݉���������"
    '''=========================================================================
    '''<summary>ES Log���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdEsLog_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEsLog.Click

        If gESLog_flg = True Then                       ' ON->OFF
            gESLog_flg = False                          ' ES���OOFF 
            cmdEsLog.Text = "ESLog OFF"
            cmdEsLog.BackColor = System.Drawing.SystemColors.Control
        Else                                            ' OFF->ON
            gESLog_flg = True                           ' ES���OON
            cmdEsLog.Text = "ESLog ON"
            cmdEsLog.BackColor = System.Drawing.Color.Lime
        End If

    End Sub

#End Region

#Region "IX2 Log Log���݉���������"
    '''=========================================================================
    '''<summary>IX2 Log���݉���������</summary>
    '''<remarks>IX2 Log���݂��uIX2Log ON�v���́uIX2Log OFF�v�ɐؑւ���</remarks>
    '''=========================================================================
    Private Sub CMdIX2Log_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        If gSysPrm.stDEV.rIX2Log_flg = True Then
            ' ON->OFF
            gSysPrm.stDEV.rIX2Log_flg = False                           ' IX2Log OFF 
            CMdIX2Log.Text = "IX2Log OFF"
            CMdIX2Log.BackColor = System.Drawing.ColorTranslator.FromOle(&H8000000F)
        Else
            ' OFF->ON
            gSysPrm.stDEV.rIX2Log_flg = True                            ' IX2Log ON
            CMdIX2Log.Text = "IX2Log ON"
            CMdIX2Log.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF00)
        End If
    End Sub
#End Region

#Region "EDIT LOCK/UNLOCK���݉���������"
    '''=========================================================================
    '''<summary>EDIT LOCK/UNLOCK���݉���������</summary>
    '''<remarks>�I�v�V�����@�\</remarks>
    '''=========================================================================
    Private Sub cmdEditLock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCnd.Click

        Dim r As Short
        Dim strMSG As String = ""                                       ' V2.0.0.0_25

        '�@V1.14.0.0�@ Add ��
        If gSysPrm.stDEV.sEditLock_flg = True Then
            ' UNLOCK->LOCK
            gSysPrm.stDEV.sEditLock_flg = False
            CmdCnd.Text = "EDIT LOCK"
            CmdCnd.BackColor = System.Drawing.SystemColors.Control
            giPassWord_Lock = 1
        Else
            ' LOCK->UNLOCK
            ' �f�[�^�ҏW�p�X���[�h�`�F�b�N
            r = Password1.ShowDialog((gSysPrm.stTMN.giMsgTyp), (gSysPrm.stSYP.gstrPassword))
            If (r = cFRS_ERR_START) Then                  ' OK 
                gSysPrm.stDEV.sEditLock_flg = True
                CmdCnd.Text = "EDIT UNLOCK"
                CmdCnd.BackColor = System.Drawing.Color.Lime
                giPassWord_Lock = 0
            End If
        End If
        '�@V1.14.0.0�@ ��

        '----- V2.0.0.0_25�� -----
        ' SL436S�̏ꍇ�̓f�[�^�ҏW�ƁuEDIT LOCK/UNLOCK���݁v�������N������׃V�X�p��(EDIT LOCK���)���X�V����
        If (giMachineKd = MACHINE_KD_RS) Then
            If (gSysPrm.stDEV.sEditLock_flg = False) Then               ' sEditLock_flg(�ҏW���b�N(0=����, 1=���Ȃ�))
                strMSG = "0"                                            ' �uEDIT LOCK�v
            Else
                strMSG = "1"                                            ' �uEDIT UNLOCK�v
            End If
            Call WritePrivateProfileString("DEVICE_CONST", "EDITLOCK", strMSG, SYSPARAMPATH)
        End If
        '----- V2.0.0.0_25�� -----

    End Sub
#End Region

#Region "ADJ���݉���������"
    '''=========================================================================
    '''<summary>ADJ���݉��������� ###009</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub BtnADJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnADJ.Click

        Dim strMSG As String

        Try
            If (BtnADJ.Text = "ADJ OFF") Then
                gbChkboxHalt = True
                BtnADJ.Text = "ADJ ON"
                BtnADJ.BackColor = System.Drawing.Color.Yellow
                'V5.0.0.1�A��
                If (gKeiTyp = KEY_TYPE_RS) Then
                    If (gbFgAutoOperation = True) Then
                        ''V5.0.0.1-22                        If giAppMode = APP_MODE_FINEADJ And bAllMagazineFinFlag = False  Then
                        If giAppMode = APP_MODE_FINEADJ And bAllMagazineFinFlag = False And gbChkSubstrateSet = False Then 'V5.0.0.1-22
                            BtnSubstrateSet.Enabled = True
                        End If
                    End If
                End If
                'V5.0.0.1�A��
            Else
                gbChkboxHalt = False
                BtnADJ.Text = "ADJ OFF"
                BtnADJ.BackColor = System.Drawing.SystemColors.Control
                'V5.0.0.1�A��
                If (gKeiTyp = KEY_TYPE_RS) Then
                    If (gbFgAutoOperation = True) Then
                        BtnSubstrateSet.Enabled = False
                    End If
                End If
                'V5.0.0.1�A��
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.BtnADJ_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    'V5.0.0.4�@��
#Region "SYCLE���݉���������"
    Private Sub btnCycleStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCycleStop.Click
        Try
            If bFgCyclStp Then
                'bFgCyclStp = False
                'btnCycleStop.Text = "CYCLE STOP OFF"
                'btnCycleStop.BackColor = System.Drawing.SystemColors.Control
            Else
                bFgCyclStp = True                                          ' �T�C�N����~�t���O V4.0.0.0�R
                btnCycleStop.Text = "CYCLE STOP ON"
                btnCycleStop.BackColor = System.Drawing.Color.Yellow
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            MsgBox("i-TKY.btnCycleStop_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region
    'V5.0.0.4�@��
    '----- V1.18.0.0�B�� -----
#Region "Print On/Off���݉���������(���[���a����)"
    '''=========================================================================
    ''' <summary>Print On/Off���݉���������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnPrintOnOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrintOnOff.Click

        Dim strMSG As String

        Try
            '----- V6.0.3.0_25�� -----
            'If (gSysPrm.stDEV.rPrnOut_flg = True) Then
            If (BtnPrintOnOff.BackColor = Color.Lime) Then
                '-----V6.0.3.0_25�� -----
                ' ON->OFF
                gSysPrm.stDEV.rPrnOut_flg = False
                BtnPrintOnOff.Text = "Print OFF"
                BtnPrintOnOff.BackColor = System.Drawing.SystemColors.Control
            Else
                ' OFF->ON
                gSysPrm.stDEV.rPrnOut_flg = True
                BtnPrintOnOff.Text = "Print ON"
                BtnPrintOnOff.BackColor = Color.Lime
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.BtnPrintOnOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Print���݉���������(���[���a����)"
    '''=========================================================================
    ''' <summary>Print���݉���������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrint.Click

        Dim strMSG As String

        Try
            '----- V6.0.3.0_39�� -----
            Timer1.Enabled = False                                      ' �Ď��^�C�}�[��~
            '----- V6.0.3.0_39�� -----

            '----- V6.0.3.0_31�� -----
            If gbAutoOperating = True Then                              ' �����^�]���Ȃ����s�Ƃ���
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "�����^�]���͎蓮�ň���ł��܂���B�����^�]���I�����Ă�����s���ĉ������B "
                Else
                    strMSG = "Cannot printout while auto operation mode. Please finish auto operation. "
                End If
                MsgBox(strMSG)
                GoTo STP_EXIT                                           ' V6.0.3.0_39
                'Exit Sub                                               ' V6.0.3.0_39
            End If
            '----- V6.0.3.0_31�� -----

            ' �g���~���O���ʈ��(Print�{�^������)
            Call PrnTrimResult(1)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.BtnPrint_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        '----- V6.0.3.0_39�� -----
STP_EXIT:
        Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
        Timer1.Enabled = True                                       ' �Ď��^�C�}�[�J�n
        '----- V6.0.3.0_39�� -----
    End Sub
#End Region
    '----- V1.18.0.0�B�� -----
    '----- V1.18.0.0�K�� -----
#Region "���[�}�K�W���Ɏ��[����������̃N���A�{�^������������(���[���a����)"
    '''=========================================================================
    ''' <summary>���[�}�K�W���Ɏ��[����������̃N���A�{�^������������(���[���a����)</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnStrageClr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnStrageClr.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' "��������N���A���Ă���낵���ł����H"
            r = Me.System1.TrmMsgBox(gSysPrm, MSG_155, MsgBoxStyle.OkCancel, gAppName)
            If (r <> cFRS_ERR_START) Then Return '                      ' OK�����ȊO��Return



            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.BtnStrageClr_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�K�� -----
    '----- V1.18.0.4�@�� -----
#Region "Power Ctrl ON/OFF���݉���������(���[���a����)"
    '''=========================================================================
    ''' <summary>Power Ctrl ON/OFF���݉���������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnPowerOnOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPowerOnOff.Click

        Dim strMSG As String

        Try
            If (BtnPowerOnOff.Text = "Power Ctrl ON") Then
                ' ON->OFF
                BtnPowerOnOff.Text = "Power Ctrl OFF"
                BtnPowerOnOff.BackColor = System.Drawing.SystemColors.Control
            Else
                ' OFF->ON
                BtnPowerOnOff.Text = "Power Ctrl ON"
                BtnPowerOnOff.BackColor = Color.Lime
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.BtnPowerOnOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.4�@�� -----
    'V4.10.0.0�@          ��
#Region "���O�I�����[�U�[�\���E�ؑփ{�^������������"
    Private Sub btnUserLogon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserLogon.Click
        Try
            GrpMode.Select()
            'Using frm As New FormLogon()
            '    If (DialogResult.OK = frm.ShowDialog(Me)) Then
            '        SetBtnUserLogon()
            '    End If
            'End Using

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.btnUserLogon_Click() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMSG)
        End Try

    End Sub
    ''' <summary>���O�I�����[�U�[�\���X�V</summary>
    ''' <param name="visible">False=��\���ɂ���</param>
    ''' <remarks>'V4.10.0.0�@</remarks>
    Private Sub SetBtnUserLogon(Optional ByVal visible As Boolean = True)
        With btnUserLogon
#If False Then  'V4.10.0.0�F
            If (UserInfo.EnableUserLevel) Then
                .BackColor = UserInfo.LogonUser.GetBackColor()
                .ForeColor = UserInfo.LogonUser.GetForeColor()
                .Text = UserInfo.LogonUser.GetDisplayName()
                .Visible = visible
            Else
                .Visible = False
            End If
#Else
            'V4.10.0.0�F �����J���ް�ޮ݂ł͎g�p���Ȃ�
            .Enabled = False
            .Visible = False
#End If
        End With

    End Sub
#End Region
    'V4.10.0.0�@          ��
    '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
#Region "������{�^������������"
    '''=========================================================================
    '''<summary>������{�^������������</summary>
    '''<param name="sender"></param>
    '''<param name="e"></param>
    '''=========================================================================
    Private Sub BtnSubstrateSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSubstrateSet.Click

        Dim strMSG As String
        Dim r As Integer

        Try

            If (BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control) Then
                gbChkSubstrateSet = True                                ' ������t���OON 
                BtnSubstrateSet.BackColor = System.Drawing.Color.Pink
                'V5.0.0.1�G
                'If IsNothing(gObjMSG) = True Then
                '    gObjMSG = New FrmWait()
                '    Call gObjMSG.Show(Me)
                'End If
                'V5.0.0.1�G
                TimerAdjust.Enabled = False 'V4.11.0.0�O
                GroupBoxEnableChange(False)
                ' ���������
                r = SubstrateSet_Proc(System1)
                'V5.0.0.1�I
                ''V5.0.0.1�M                 If r = cFRS_ERR_RST Or r = cFRS_ERR_LDR1 Then
                If r = cFRS_ERR_RST Then 'V5.0.0.1�M 
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call RESET_SWITCH_ON()
                    End If
#End If
                ElseIf r = cFRS_ERR_LDR1 Then
                    giErrLoader = cFRS_ERR_LDR1
                End If
                'V5.0.0.1�I
                GroupBoxEnableChange(True)
                TimerAdjust.Enabled = True 'V4.11.0.0�O
            Else
                gbChkSubstrateSet = False                               ' ������t���OOFF
                BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control
                'V4.11.0.0�J
                If IsNothing(gObjMSG) <> True Then
                    gObjMSG.MsgClose()
                    gObjMSG = Nothing
                End If
                'V4.11.0.0�J
            End If


            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.BtnSubstrateSet_MouseDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "������{�^���̔w�i�F��ݒ肷��"
    '''=========================================================================
    ''' <summary>������{�^���̔w�i�F��ݒ肷��</summary>
    ''' <param name="bFlg">(INP)True:�w�i�F=�s���N, False:�w�i�F=�D�F</param>
    '''=========================================================================
    Public Sub Set_SubstrateSetBtn(ByVal bFlg As Boolean)

        Dim strMSG As String

        Try
            If (bFlg = True) Then
                BtnSubstrateSet.BackColor = System.Drawing.Color.Pink
            Else
                BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.Set_SubstrateSetBtn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0�E�� -----
    '----- V6.1.1.0�C�� -----
#Region "�A���[������ON/OFF�{�^������������(�I�v�V����)"
    '''=========================================================================
    ''' <summary>�A���[������ON/OFF�{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>IAM�a����</remarks>
    '''=========================================================================
    Private Sub BtnAlarmOnOff_Click(sender As System.Object, e As System.EventArgs) Handles BtnAlarmOnOff.Click

        Dim strMSG As String

        Try
            If (BtnAlarmOnOff.Text = "Buzzer On") Then
                ' ON->OFF
                Me.System1.giAlarmBuzzer = 0                            ' �A���[������炳�Ȃ�
                BtnAlarmOnOff.Text = "Buzzer Off"
                BtnAlarmOnOff.BackColor = System.Drawing.SystemColors.Control
            Else
                ' OFF->ON
                Me.System1.giAlarmBuzzer = 1                            ' �A���[������炷
                BtnAlarmOnOff.Text = "Buzzer On"
                BtnAlarmOnOff.BackColor = Color.Lime
            End If


            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.BtnAlarmOnOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V6.1.1.0�C�� -----

    '========================================================================================
    '   ���ʊ֐�
    '========================================================================================
#Region "���O��ʂɕ������\������"
    '''=========================================================================
    '''<summary>���O��ʂɕ������\������</summary>
    '''<remarks>�</remarks>
    '''=========================================================================
    Public Function Z_PRINT(ByRef s As String) As Integer

        '#4.12.2.0�C        Z_PRINT = LogPrint(s)
        Debug.Print("CON:" & s)

        '#4.12.2.0�C                    ��
        Static hWnd As IntPtr = txtLog.Handle
        Const WM_GETTEXTLENGTH As Integer = &HE
        Const EM_SETSEL As Integer = &HB1
        Const EM_REPLACESEL As Integer = &HC2

        Try
            Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)      ' �������擾
            SendMessage(hWnd, EM_SETSEL, len, len)                              ' �J�[�\���𖖔���
            SendMessageString(hWnd, EM_REPLACESEL, 0, s & Environment.NewLine)  ' �e�L�X�g�ɕ������ǉ�����

            Z_PRINT = cFRS_NORMAL

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            Dim strMSG As String = "i-TKY.LogPrint() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMSG)
            Z_PRINT = cERR_TRAP
        End Try
        '#4.12.2.0�C                    ��
    End Function

    ''' <summary>���O��ʂɕ������\������</summary>
    ''' <param name="s"></param>
    ''' <remarks>'#4.12.2.0�C</remarks>
    Public Sub Z_PRINT(ByVal s As StringBuilder)

        Static hWnd As IntPtr = txtLog.Handle
        Const WM_SETREDRAW As Integer = &HB
        Const WM_GETTEXTLENGTH As Integer = &HE
        Const EM_SETSEL As Integer = &HB1
        Const EM_REPLACESEL As Integer = &HC2

        Try
            s.AppendLine()              ' ���s

            SendMessage(hWnd, WM_SETREDRAW, 0, 0)                           ' �`���~
            Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)  ' �������擾
            SendMessage(hWnd, EM_SETSEL, len, len)                          ' �J�[�\���𖖔���
            SendMessageStringBuilder(hWnd, EM_REPLACESEL, 0, s)             ' �e�L�X�g�ɕ������ǉ�����

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.Z_PRINT() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMSG)

        Finally
            SendMessage(hWnd, WM_SETREDRAW, 1, 0)                           ' �`��ĊJ
        End Try

    End Sub
#End Region

#Region "���O��ʃN���A"
    '''=========================================================================
    '''<summary>���O��ʃN���A</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Z_CLS()
        'Private Sub Z_CLS() '###013

        '#4.12.2.0�C                    ��
        '#4.12.2.0�C        txtLog.Text = ""
        Static hWnd As IntPtr = txtLog.Handle
        Const WM_SETTEXT As Integer = &HC

        Try
            SendMessageString(hWnd, WM_SETTEXT, 0, "")                  ' �폜

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.Z_CLS() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMSG)
        End Try
        '#4.12.2.0�C                    ��
    End Sub
#End Region

#Region "���O��ʂւ̕������s�폜"
    '''=========================================================================
    '''<summary>���O��ʂւ̕������s�폜</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Z_DEL() As Integer

        Dim stemp As String

        stemp = txtLog.Text
        txtLog.Text = Mid(stemp, 1, InStrRev(stemp, vbCrLf, Len(stemp) - 2) + 1)
        txtLog.Refresh()

        Z_DEL = cFRS_NORMAL

    End Function
#End Region

#Region "���O��ʂւ̕�����\��"
#If False Then                          '#4.12.2.0�C
    '''=========================================================================
    '''<summary>���O��ʂւ̕�����\��</summary>
    '''<param name="s">(INP) �\�����镶����</param>
    '''<returns>0=����, 0�ȊO=���s</returns>
    '''=========================================================================
    Private Function LogPrint(ByRef s As String) As Integer

        Dim strMSG As String

        Try
            Me.txtLog.AppendText(s + vbCrLf)                            ' �e�L�X�g�ɕ������ǉ����� 
            '----- ###097 ����ȉ��͂Ȃ��Ă��\�����ʂ͂����Ȃ����ߍ폜 -----
            'Me.txtLog.SelectionStart = Len(txtLog.Text)                 ' �e�L�X�g�̊J�n�_��ݒ�
            'Me.txtLog.SelectionLength = 0                               ' �I�𕶎��� = 0 
            'Me.txtLog.Refresh()                                         ' �ĕ`��
            'Me.txtLog.ScrollToCaret()                                   ' ���݂̃J���b�g�ʒu�܂ŃX�N���[������
            '----- ###097  -----

            LogPrint = cFRS_NORMAL

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.LogPrint() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            LogPrint = cERR_TRAP
        End Try

    End Function
#End If
#End Region

#Region "���݂̓���̧�ٖ����쐬(Index2��p)"
    '''=========================================================================
    '''<summary>���݂̓���̧�ٖ����쐬(Index2��p)</summary>
    '''<param name="sPath">(INP) ̧�ق��쐬�����߽��</param>
    '''<param name="sFrm"> (INP) �g���q(.LOG/.TXT/.CSV)</param>
    '''<param name="sHead">(INP) ̧�ٖ��̐擪������</param>
    '''<returns>����̧�ٖ�(Full Path)</returns>
    '''=========================================================================
    Private Function MakeDateFileName(ByRef sPath As String, ByRef sFrm As String, ByRef sHead As String) As String

        Dim pDate As String
        Dim pTime As String
        Dim pFName As String

        ' �����擾
        pDate = Today.ToString("yyyyMMdd") ' V1.18.0.0�E
        pTime = TimeOfDay.ToString("hhmmss")

        ' ̧�ٖ�
        pFName = sPath & "\" & sHead & pDate & pTime & sFrm
        MakeDateFileName = pFName

        Debug.Print("MakeDateFileName = " & pFName)
    End Function

#End Region

#Region "�g���~���O�J�n����"
    '''=========================================================================
    '''<summary>�g���~���O�J�n����</summary>
    '''<remarks>�g���~���O���s�O�̊e��`�F�b�N�A�ݒ���s�����B
    ''' GUI��̃X�^�[�g�{�^���ƃR���\�[�����START SW������
    ''' 2�n��������͂��󂯕t����B</remarks>
    '''=========================================================================
    Private Function Start_Trimming() As Integer

        Dim intRet As Integer = cFRS_NORMAL
        Dim intPos1 As Integer
        Dim intPos2 As Integer
        Dim strHead As String
        Dim strMSG As String

        Try
            intPos1 = 0
            intPos2 = 0
            strHead = ""

            ' �W�o�@�ُ�`�F�b�N
            intRet = System1.CheckDustVaccumeAlarm(gSysPrm)
            If (intRet <> cFRS_NORMAL) Then
                intRet = System1.Form_Reset(cGMODE_ERR_DUST, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                giAppMode = APP_MODE_EXIT
                If (intRet <= cFRS_ERR_EMG) Then
                    ' �����I��
                    Call AppEndDataSave()
                    Call AplicationForcedEnding()
                    Return (intRet)
                End If
            End If

            ' 'V4.1.0.0�F
            CheckPLCLowBatteryAlarm()

            ' IX2���O�o�̓p�X�擾 V1.18.0.0�E
            sIX2LogFilePath = MakeDateFileName((gSysPrm.stLOG.gsLoggingDir), ".txt", "IX2_")

            ' EdgeSense�̃��O�o�̓p�X�擾
            intPos1 = InStrRev(gStrTrimFileName, "\")
            intPos2 = InStrRev(gStrTrimFileName, ".")

            If intPos1 <> 0 And intPos2 <> 0 Then
                strHead = Mid(gStrTrimFileName, intPos1 + 1, intPos2 - (intPos1 + 1)) '�f�[�^�t�@�C�������擾�i�g���q�����j
            End If
            If gEsCutFileType = 0 Then
                gsESLogFilePath = MakeDateFileName((gSysPrm.stLOG.gsLoggingDir), ".txt", "ES_" & strHead & "_") '̧���߽�擾
            Else
                gsESLogFilePath = MakeDateFileName((gSysPrm.stLOG.gsLoggingDir), ".txt", "ES2_" & strHead & "_") '̧���߽�擾
            End If

            ' ���C���̕\����ԕύX
            Call Form1Button(0)                             ' �R�}���h�{�^����񊈐����ɂ���

            'V2.0.0.0�L�@���@
            'V4.10.0.0�L            If gKeiTyp = KEY_TYPE_RS Then
            MidiumCut.DetectFunc = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "MIDIUM_CUT_FUNC", SYSPARAMPATH, "0")) ' V2.0.0.0�F
            MidiumCut.JudgeCount = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "MIDIUM_CUT_JUDGE_COUNT", SYSPARAMPATH, "0")) ' V2.0.0.0�F
            MidiumCut.dblChangeRate = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "MIDIUM_CUT_CHANGE_RATE", SYSPARAMPATH, "0")) ' V2.0.0.0�F
            Dim longbuf(2) As Long
            Dim doublebuf(2) As Double
            longbuf(0) = MidiumCut.DetectFunc
            longbuf(1) = MidiumCut.JudgeCount
            doublebuf(0) = MidiumCut.dblChangeRate
            intRet = MIDIUMCUTPARAM(doublebuf(0), longbuf(0), longbuf(1))
            'V2.0.0.0�L�@��

            'V2.0.0.0�M�@��
            'V4.10.0.0�L�v���[�g�f�[�^����擾���Ă���̂ŃR�����g�A�E�g            glProbeRetryCount = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "PROBE_RETRY_COUNT", SYSPARAMPATH, "0")) ' V2.0.0.0�F
            'V2.0.0.0�M�@��
            'V4.10.0.0�L            End If

            '-----------------------------------------------------------------------
            '   �g���~���O�J�n
            '-----------------------------------------------------------------------
            intRet = Trimming()

            ' ###073
            If (intRet = cFRS_NORMAL) Or (intRet = cFRS_ERR_RST) Then

                ' ���[�_�A���[���͏I�����Ȃ�
            ElseIf (intRet = cFRS_ERR_LDR) Or (intRet = cFRS_ERR_LDR1) Or (intRet = cFRS_ERR_LDR2) Or (intRet = cFRS_ERR_LDR3) Or (intRet = cFRS_ERR_LDRTO) Then

            ElseIf (intRet = cFRS_ERR_EMG) Then
                ' �����I��
                Call AppEndDataSave()
                Call AplicationForcedEnding()
                Return (intRet)
            End If

            '���C���̕\����ԕύX
            If giHostMode <> cHOSTcMODEcAUTO Then
                Call Form1Button(1)                         ' �R�}���h�{�^����L���ɂ���
            Else
                Call Form1Button(3)                         ' ð���������(F2), END����(F10)�̂ݗL���ɂ���
            End If

            If giHostMode = cHOSTcMODEcMANUAL Then
                Call ZCONRST()
            End If

            ' 'V4.1.0.0�F
            CheckPLCLowBatteryAlarm()

            Return (intRet)

        Catch ex As Exception
            strMSG = "i-TKY.Start_Trimming() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "�f�[�^�ҏW�v���O�����N������"
    '''=========================================================================
    ''' <summary>�f�[�^�ҏW�v���O�����N������ ###063</summary>
    ''' <param name="Mode">(INP)���[�h2(0=�ʏ탂�[�h, 1=�ꎞ��~��ʃ��[�h)</param>
    ''' <param name="shiftKeyDown">shift�L�[����</param>
    ''' <remarks>�ꎞ��~��ʃ��[�h���́u��R�v�Ɓu�J�b�g�v�f�[�^�̂ݍX�V�\�Ƃ���</remarks>
    '''=========================================================================
    Public Function ExecEditProgram(ByVal Mode As Integer, Optional ByVal shiftKeyDown As Boolean = False) As Integer '#4.12.2.0�D 
        '#4.12.2.0�D    Public Function ExecEditProgram(ByVal Mode As Integer) As Integer

        Dim piPP31 As Short
        Dim br As Boolean
        Dim r As Integer
        Dim iPW As Integer = 0                                          ' V1.18.0.0�@
        Dim intRet As Integer
        Dim iRtn As Integer = cFRS_NORMAL                               ' Return�l
        Dim ExitCode As Integer
        Dim strFilePath As String
        Dim strLdFlf As String
        Dim Objproc As Process = Nothing                                ' �v���Z�X�N���p
        Dim strMSG As String
        Dim wreg As Short                                               ' V1.18.0.0�F
        Dim GType As Integer                                            ' V1.18.0.0�F
        Dim nAutoMode As Integer                                        ' V1.19.0.0-27
        Dim dblRotateTheta As Double                                    ' �Ɖ�]�p�x V1.19.0.0-27
        Dim dblReviseCordnt1XDir As Double                              ' �␳�ʒu���W1X V1.19.0.0-27
        Dim dblReviseCordnt1YDir As Double                              ' �␳�ʒu���W1Y V1.19.0.0-27
        Dim dblReviseCordnt2XDir As Double                              ' �␳�ʒu���W2X V1.19.0.0-27
        Dim dblReviseCordnt2YDir As Double                              ' �␳�ʒu���W2Y V1.19.0.0-27
        Dim dblReviseOffsetXDir As Double                               ' �␳�߼޼�ݵ̾��X V1.19.0.0-27
        Dim dblReviseOffsetYDir As Double                               ' �␳�߼޼�ݵ̾��Y V1.19.0.0-27
        '                                       'V5.0.0.9�C  ��
        Dim dblReviseCordnt1XDirRgh As Double                           ' �␳�ʒu���W1X
        Dim dblReviseCordnt1YDirRgh As Double                           ' �␳�ʒu���W1Y
        Dim dblReviseCordnt2XDirRgh As Double                           ' �␳�ʒu���W2X
        Dim dblReviseCordnt2YDirRgh As Double                           ' �␳�ʒu���W2Y
        Dim dblReviseOffsetXDirRgh As Double                            ' �␳�߼޼�ݵ̾��X
        Dim dblReviseOffsetYDirRgh As Double                            ' �␳�߼޼�ݵ̾��Y
        '                                       'V5.0.0.9�C  ��
        'V4.0.0.0-65            ��������
        Dim adjust As String = String.Empty
        ' �ꎞ��~��ʂ���̌Ăяo����
        If (1 = Mode) Then adjust = "Adjust : "
        'V4.0.0.0-65            ��������
        '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
        Dim BlkX As Integer = 1
        Dim BlkY As Integer = 1
        '----- V4.11.0.0�D�� -----

        Try
            '-------------------------------------------------------------------
            '   �g���~���O�f�[�^�𒆊ԃt�@�C���ɏ�����(�t�@�C���㏑)
            '-------------------------------------------------------------------
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' ���݂̃u���b�N�ԍ���ޔ����Ă���
            'V5.0.0.9�O            If (giStartBlkAss = 1) Then                             ' �g���~���O�J�n�u���b�N�ԍ��w��L�� ?
            If (giStartBlkAss <> 0) Then                                ' �g���~���O�J�n�u���b�N�ԍ��w��L�� ?   'V5.0.0.9�O
                Get_StartBlkNum(BlkX, BlkY)
            End If
            '----- V4.11.0.0�D�� -----

            ' ���ԃt�@�C������ݒ肷��
            If (gTkyKnd = KND_TKY) Then
                strFilePath = EditWorkFilePath + ".tdtw"                ' TKY�̏ꍇ
            ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIP�̏ꍇ
                strFilePath = EditWorkFilePath + ".tdcw"
            Else                                                        ' NET�̏ꍇ
                strFilePath = EditWorkFilePath + ".tdnw"
            End If

            ' �V�K���[�h���̓f�[�^�t�@�C���� = (�� TKY��)"TkyDatayyyymmddhhmmss.tdt"�Ƃ���
            '#4.12.2.0�D If (gLoadDTFlag = False) Then                  ' �V�K���[�h ?
            If (gLoadDTFlag = False) OrElse (shiftKeyDown) Then         ' �V�K���[�h     '#4.12.2.0�D shift�L�[�������̐V�K�쐬�ǉ�
                If (gLoadDTFlag) Then
                    ' shift�L�[�������̐V�K�쐬
                    Temporary.Backup()                                  ' �g���~���O�f�[�^�\���̂��o�b�N�A�b�v����
                End If
                Call Init_AllTrmmingData()                              ' ���ݸ����Ұ��̏�����
                'V5.0.0.8�@ Call Init_FileVer_Sub()                     ' ̧���ް�ޮݐݒ�(CHIP/NET�p)
                Replace_Data_Extension(gStrTrimFileName)                ' �f�[�^�t�@�C���� = (�� TKY��)"TkyDatayyyymmddhhmmss.tdt"
                '                                                       ' �v���[�g�f�[�^�Ƀf�[�^�t�@�C������ݒ肷��
                gStrTrimFileName = DATA_DIR_PATH + "\" + gStrTrimFileName
                typPlateInfo.strDataName = gStrTrimFileName
                strLdFlf = "0"                                          ' ���[�h = 0(�V�K)
            Else
                strLdFlf = "1"                                          ' ���[�h = 1(�X�V)
                'V4.0.0.0-65            ��������
                ' ۸ޕ\���J�n�׸ނ���������B
                If (1 = gSysPrm.stLOG.giLoggingMode) Then
                    basTrimming.TrimLogging_CreateOrAppend(adjust & "Edit - Start")
                End If
                'V4.0.0.0-65            ��������
            End If

            'V1.19.0.0-27
            ' ���݂̐ݒ�l��ۑ����Ă����B
            nAutoMode = typPlateInfo.intReviseMode
            piPP31 = typPlateInfo.intManualReviseType
            dblRotateTheta = typPlateInfo.dblRotateTheta
            dblReviseCordnt1XDir = typPlateInfo.dblReviseCordnt1XDir
            dblReviseCordnt1YDir = typPlateInfo.dblReviseCordnt1YDir
            dblReviseCordnt2XDir = typPlateInfo.dblReviseCordnt2XDir
            dblReviseCordnt2YDir = typPlateInfo.dblReviseCordnt2YDir
            dblReviseOffsetXDir = typPlateInfo.dblReviseOffsetXDir      'V5.0.0.9�C  �����H
            dblReviseOffsetYDir = typPlateInfo.dblReviseOffsetYDir      'V5.0.0.9�C  �����H
            'V1.19.0.0-27
            '                                       'V5.0.0.9�C  ��
            dblReviseCordnt1XDirRgh = typPlateInfo.dblReviseCordnt1XDirRgh      ' �␳�ʒu���W1X
            dblReviseCordnt1YDirRgh = typPlateInfo.dblReviseCordnt1YDirRgh      ' �␳�ʒu���W1Y
            dblReviseCordnt2XDirRgh = typPlateInfo.dblReviseCordnt2XDirRgh      ' �␳�ʒu���W2X
            dblReviseCordnt2YDirRgh = typPlateInfo.dblReviseCordnt2YDirRgh      ' �␳�ʒu���W2Y
            dblReviseOffsetXDirRgh = typPlateInfo.dblReviseOffsetXDirRgh        ' �␳�߼޼�ݵ̾��X
            dblReviseOffsetYDirRgh = typPlateInfo.dblReviseOffsetYDirRgh        ' �␳�߼޼�ݵ̾��Y
            '                                       'V5.0.0.9�C  ��
            ' �g���~���O�f�[�^�𒆊ԃt�@�C���ɏ�����
            r = File_Save(strFilePath)
            If (r <> cFRS_NORMAL) Then GoTo STP_WRITE_ERR

            ''V4.5.0.0�D ��
            ' ���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(True)
            ''V4.5.0.0�D ��

            '-------------------------------------------------------------------
            '   �f�[�^�ҏW�v���O�������N������
            '-------------------------------------------------------------------
            ' �v���Z�X�̋N��
            Objproc = New Process                                       ' Process�I�u�W�F�N�g�𐶐����� 

            ' �R�}���h���C�������ݒ�
            'If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ? 
            If (giMachineKd = MACHINE_KD_RS) OrElse (0 <> Globals_Renamed.giChipEditEx) Then 'V4.10.0.0�A
                '----- V2.0.0.0_25�� -----
                ' �R�}���h���C�������ݒ�
                Objproc.StartInfo.FileName = "C:\TRIM\TrimDataEditorEx.exe"
                '                                                           ' �R�}���h���C���������쐬����
                strMSG = gTkyKnd.ToString("0") + " "                        ' args[0]:�f�[�^���("0"=TKY, "1"=TKY-CHIP, "2"=TKY-NET
                strMSG = strMSG + """" + gStrTrimFileName + """" + " "      ' args[1]:�f�[�^�t�@�C����
                strMSG = strMSG + strFilePath + " "                         ' args[2]:���ԃt�@�C����
                strMSG = strMSG + strLdFlf + " "                            ' args[3]:���[�h(0=�V�K, 1=�X�V)
                strMSG = strMSG + Mode.ToString("0") + " "                  ' args[4]:���[�h2(0=�ʏ탂�[�h, 1=�ꎞ��~��ʃ��[�h) 
                iPW = Get_DataEdit_PasswordKind(gSysPrm.stCTM.giSPECIAL)    '         �p�X���[�h�@�\�擾                                    
                strMSG = strMSG + iPW.ToString("0")                         ' args[5]:�p�X���[�h�@�\(0=�Ȃ�, 1=����(�i�a�d�l), 2����(���[���a�d�l, 3=�V���v���g���})
                strMSG &= (" " & UserInfo.LogonUser.GetName())              ' args[6]:���O�I�����[�U�[  'V4.10.0.0�@ 'V5.0.0.8�@ �ݺ���
                Objproc.StartInfo.Arguments = strMSG                        ' �R�}���h���C�������ݒ�
                '----- V2.0.0.0_25�� -----
            Else
                ' �R�}���h���C�������ݒ�
                Objproc.StartInfo.FileName = "C:\TRIM\TrimDataEditor.exe"
                '                                                           ' �R�}���h���C���������쐬����
                strMSG = gTkyKnd.ToString("0") + " "                        ' args[0]:�f�[�^���("0"=TKY, "1"=TKY-CHIP, "2"=TKY-NET
                strMSG = strMSG + """" + gStrTrimFileName + """" + " "      ' args[1]:�f�[�^�t�@�C���� '###034
                strMSG = strMSG + strFilePath + " "                         ' args[2]:���ԃt�@�C����
                strMSG = strMSG + strLdFlf + " "                            ' args[3]:���[�h(0=�V�K, 1=�X�V)
                strMSG = strMSG + Mode.ToString("0") + " "                  ' args[4]:���[�h2(0=�ʏ탂�[�h, 1=�ꎞ��~��ʃ��[�h) V1.14.0.0�@
                'strMSG = strMSG + giPassWord_Lock.ToString("0")            ' args[5]:�p�X���[�h�@�\(0=�Ȃ�, 1=����)                V1.14.0.0�@
                iPW = Get_DataEdit_PasswordKind(gSysPrm.stCTM.giSPECIAL)    '         �p�X���[�h�@�\�擾                                          V1.18.0.0�@
                strMSG = strMSG + iPW.ToString("0")                         ' args[5]:�p�X���[�h�@�\(0=�Ȃ�, 1=����(�i�a�d�l), 2����(���[���a�d�l)  V1.18.0.0�@
                strMSG &= (" " & UserInfo.LogonUser.GetName())              ' args[6]:���O�I�����[�U�[  'V4.10.0.0�@ 'V5.0.0.8�@ �ݺ���
                Objproc.StartInfo.Arguments = strMSG                        ' �R�}���h���C�������ݒ�
            End If

            VideoLibrary1.VideoStop()                                   'V6.0.0.0-29

            Objproc.StartInfo.WorkingDirectory = "C:\\TRIM"             ' ��ƃt�H���_
            Objproc.Start()                                             ' �v���Z�X�N��

            ' Form���ŏ�������
            'Me.WindowState = FormWindowState.Minimized
            If (ObjIOMon Is Nothing = False) Then                       ' �h�n���j�^��\�� ?
                ObjIOMon.Hide()
            End If

            ' �v���Z�X�̏I���`�F�b�N
            br = False
            Do
                ' ����~���`�F�b�N
                intRet = System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
                If (intRet <> cFRS_NORMAL) Then                         ' �G���[���o ?

                    ' �v���Z�X�I�����b�Z�[�W���M(����޳�̂�����؂̏ꍇ) 
                    If (Objproc.CloseMainWindow() = False) Then         ' Ҳݳ���޳�ɸ۰��ү���ނ𑗐M����
                        Objproc.Kill()                                  ' �I�����Ȃ������ꍇ�͋����I������
                    End If

                    ' �v���Z�X�̏I����҂�
                    Do
                        System1.WAIT(0.1)                               ' Wait(Sec) 
                    Loop While (Objproc.HasExited <> True)              ' �v���Z�X���I�����Ă���ꍇ�̂�True���Ԃ�
                    Exit Do
                End If

                ' �v���Z�X�̏I���`�F�b�N
                br = Objproc.WaitForExit(300)                           ' �v���Z�X�̏I���҂�(ms) �����Ԏw��Ȃ��͏I���܂ŕԂ��Ă��Ȃ� 
            Loop While (br = False)

            ' �f�[�^�ҏW�v���O�����̏I���R�[�h���擾���� 
            'V4.10.0.0�@              ��
            'ExitCode = Objproc.ExitCode                                 ' �I���R�[�h�擾(HasExited=True�łȂ����ׯ�߂���������)
            ExitCode = UserInfo.GetExitCode(Objproc.ExitCode)           ' �I���R�[�h�擾(HasExited=True�łȂ����ׯ�߂���������)'V5.0.0.8�@
#If False Then  'V4.10.0.0�G �����J���ް�ޮ݂ł͕ҏW��ʂ�հ�ނ������p���Ȃ�
            UserInfo.LogonUser = UserInfo.GetLogonUser(Objproc.ExitCode) ' ���O�I�����[�U�[�擾
            UserInfo.ReadUserSettings()                                 ' �\�����E�p�X���[�h���ēǂݍ��݂���
#End If
            SetBtnUserLogon()                                           ' ���O�I�����[�U�[�\���X�V
            'V4.10.0.0�@              ��
            Objproc.Close()                                             ' �I�u�W�F�N�g�J��
            Objproc.Dispose()                                           ' ���\�[�X�J��
            Objproc = Nothing                                           'V4.10.0.0�@

            ' Form�����ɖ߂�
            'Me.WindowState = FormWindowState.Normal
            If (ObjIOMon Is Nothing = False) Then                       ' �h�n���j�^�\�� ?
                ObjIOMon.Show()
            End If

            ' �R�}���h�{�^����L���ɂ���(�ʏ탂�[�h��)
            If (Mode = 0) Then
                Call Form1Button(1)                                     ' �R�}���h�{�^����L���ɂ���
                Call SetVisiblePropForVideo(True)
                Me.Refresh()
            End If

            '-------------------------------------------------------------------
            '   ����~���̃G���[�����o�����ꍇ
            '-------------------------------------------------------------------
            If (intRet <> cFRS_NORMAL) Then                             ' �G���[���o ?
                If (intRet <= cFRS_ERR_EMG) Then                        ' ����~�����o ?
                    Call Me.AppEndDataSave()
                    Call Me.AplicationForcedEnding()                    ' �����I��
                End If
                iRtn = intRet                                           ' Return�l�ݒ�
                GoTo STP_END
            End If

            '-------------------------------------------------------------------
            '   ���ԃt�@�C����Ǎ���
            '-------------------------------------------------------------------
            ' �f�[�^�ҏW�v���O�����̏I���R�[�h�����킩�`�F�b�N����
            If (ExitCode = PRC_EXIT_CAN) Or (ExitCode = PRC_EXIT_NML) Or (ExitCode = PRC_EXIT_NML2) Then
                ' ���펞��NOP
            Else
                ' "�f�[�^�ҏW�G���[�B�f�[�^���ă��[�h���Ă��������B(ExitCode = xxx)"
                strMSG = MSG_135 + "(ExitCode = " + ExitCode.ToString("0") + ")"
                iRtn = cFRS_FIOERR_INP                                  ' Return�l = �t�@�C�����̓G���[
                GoTo STP_ERR_DSP
            End If

            ' ������ V3.1.0.0�A 2014/12/01
            ' �f�[�^�ҏW�̃L�����Z�����̓f�[�^��ǂݍ��܂Ȃ��悤�ɂ���
            If ExitCode = PRC_EXIT_CAN Then
#If False Then  '#4.12.2.0�D             ��
                'V4.0.0.0-65            ��������
                If (True = gLoadDTFlag) AndAlso (1 = gSysPrm.stLOG.giLoggingMode) Then
                    basTrimming.TrimLogging_CreateOrAppend( _
                        adjust & "Edit - Cancel", procMsgOnly:=True)
                End If
                'V4.0.0.0-65            ��������
#End If
                If (gLoadDTFlag) Then
                    If (shiftKeyDown) Then
                        ' shift�L�[�������̐V�K�쐬�ŃL�����Z��
                        Temporary.Restore()                             ' �g���~���O�f�[�^�\���̂𕜌�����
                        gStrTrimFileName = typPlateInfo.strDataName
                    Else
                        If (1 = gSysPrm.stLOG.giLoggingMode) Then
                            basTrimming.TrimLogging_CreateOrAppend(
                                adjust & "Edit - Cancel", procMsgOnly:=True)
                        End If
                    End If
                End If
                '#4.12.2.0�D             ��
                GoTo STP_END
            End If
            ' ������ V3.1.0.0�A 2014/12/01

            ' ���ԃt�@�C����Ǎ���
            r = File_Read(strFilePath)
            If (r <> cFRS_NORMAL) Then GoTo STP_READ_ERR

            'V4.0.0.0-65            ��������
            ' ۸ޕ\���J�n�׸ނ���������B
            If (1 = gSysPrm.stLOG.giLoggingMode) Then
                '#4.12.2.0�D    If (False = gLoadDTFlag) Then
                If (False = gLoadDTFlag) OrElse (shiftKeyDown) Then     '#4.12.2.0�D shift�L�[�����V�K�쐬�ǉ�
                    If (PRC_EXIT_NML2 = ExitCode) Then
                        ' �ް�̧�ٖ�۰�ގ��ɕҏW��ʂ���Save�Ŗ߂����ꍇ
                        basTrimming.TrimLogging_CreateOrAppend("Edit - Save", typPlateInfo.strDataName)
                    Else
                        ' �ް�̧�ٖ�۰�ގ��ɕҏW��ʂ���OK�Ŗ߂����ꍇ
                        basTrimming.TrimLogging_CreateOrAppend("Edit - New", typPlateInfo.strDataName)
                    End If
                Else
                    If (gStrTrimFileName <> typPlateInfo.strDataName) Then
                        ' �ҏW��ʂŕʖ��ۑ����ꂽ�ꍇ
                        If (0 = Mode) Then
                            ' �ҏW��ʂ�ʏ�N�������ꍇ
                            basTrimming.TrimLogging_CreateOrAppend("Edit - Save (" & typPlateInfo.strDataName _
                                                                   & ")", procMsgOnly:=True)        ' ����̧�قɕۑ�

                            basTrimming.TrimLogging_CreateOrAppend("Edit - Save",
                                                                   typPlateInfo.strDataName)        ' �V�Ķ�قɕۑ� 
                        Else
                            ' �ꎞ��~��ʂ���N�������ꍇ
                            basTrimming.TrimLogging_CreateOrAppend(
                                adjust & "Edit - Save (" & typPlateInfo.strDataName & ")")          ' ����̧�قɕۑ�
                        End If
                    Else
                        ' OK�ŏI���܂��͓����ۑ��ŏI�������ꍇ
                        basTrimming.TrimLogging_CreateOrAppend(adjust & "Edit - End")
                    End If
                End If
            End If
            'V4.0.0.0-65            ��������

            ' �f�[�^�t�@�C�����X�V
            gStrTrimFileName = typPlateInfo.strDataName
            '----- V6.1.4.0�F��(KOA EW�aSL432RD�Ή�) -----
            'LblDataFileName.Text = typPlateInfo.strDataName
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = typPlateInfo.strDataName          ' �g���~���O�f�[�^���Ƒ�P��R�f�[�^�\�����ݒ肷�� 
            Else
                LblDataFileName.Text = typPlateInfo.strDataName
            End If
            '----- V6.1.4.0�F�� -----
            'V4.2.0.0�B
            If gKeiTyp = KEY_TYPE_RS Then
                TrimData.SetTrimmingData(gStrTrimFileName)
            End If
            'V4.2.0.0�B

            gLoadDTFlag = True                                          ' gLoadDTFlag = �f�[�^���[�h��
            commandtutorial.SetDataLoad()                               'V2.0.0.0�I �f�[�^���[�h��Ԑݒ�
            If (ExitCode = PRC_EXIT_NML) Or (ExitCode = PRC_EXIT_NML2) Then
                gCmpTrimDataFlg = 1                                     ' �f�[�^�X�V�t���O = 1(�X�V����)
            End If

            If (ExitCode = PRC_EXIT_NML2) Then                          ' ����I��(�f�[�^�Z�[�u�v������) ?
                ' "�f�[�^���Z�[�u���܂���"
                strMSG = MSG_145 & vbCrLf & " (File Name = " + gStrTrimFileName + ")"
                Call Z_PRINT(strMSG)                                    ' ���b�Z�[�W�\��(���O���)
            End If

            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' ���݂̃u���b�N�ԍ����Đݒ肷��
            'V5.0.0.9�O            If (giStartBlkAss = 1) Then                             ' �g���~���O�J�n�u���b�N�ԍ��w��L�� ?
            If (giStartBlkAss <> 0) Then                                ' �g���~���O�J�n�u���b�N�ԍ��w��L�� ?   'V5.0.0.9�O
                Set_StartBlkNum(BlkX, BlkY)
            End If
            '----- V4.11.0.0�D�� -----

            '----- V1.14.0.0�G�� -----
            ' FL�����猻�݂̉��H��������M����
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then

                '----- V6.0.3.0�A�� -----
                ' SL436S���͉��H�����ԍ���ݒ肵�Ȃ���
                Call SetCndNum()
                '----- V6.0.3.0�A�� -----

                r = TrimCondInfoRcv(stCND)
                If (r <> SerialErrorCode.rRS_OK) Then
                    ' "�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
                    Call System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    iRtn = r                                         ' Return�l�ݒ�
                    GoTo STP_END
                End If
                '----- V4.0.0.0-28 �� -----
                '-----------------------------------------------------------------------
                '   ���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��
                '   (�V���v���g���}�p(FL���ŉ��H�����t�@�C��������ꍇ))
                '-----------------------------------------------------------------------
                r = SetCutDataCndInfFromCndNum(_readFileVer)
                '----- V4.0.0.0-28 �� -----

                '----- V3.0.0.0�I��-----
                ' FL�p�p���[�������t�@�C��(�f�[�^�t�@�C����+.att)�����C�g����
                'V6.0.1.021�@��
                '                    If (giAutoPwr = 1) Then                             ' �I�[�g�p���[������(1=����/0=���Ȃ�) 
                If (giAutoPwr = 1) Or (giFixAttOnly = 1) Then                             ' �I�[�g�p���[������(1=����/0=���Ȃ�) 
                    'V6.0.1.021�@��
                    r = GetFLAttFileName(gStrTrimFileName, strMSG, False)   ' �Œ�ATT���t�@�C�������擾����
                    Call PutFlAttInfoData(strMSG, stPWR_LSR)                ' FL�p�p���[�������t�@�C�������C�g����
                End If
                '----- V3.0.0.0�I��-----

            End If
            '----- V1.14.0.0�G�� -----
            '----- V1.18.0.0�F�� -----
            ' �ėpGP-IB����̗L�����擾����
            Call Gpib2FlgCheck(bGpib2Flg, wreg, GType)
            ' GPIB�f�[�^��INtime���ɑ��M����
            If (GType = 1) Then                                         ' �ėpGPIB ?
                r = SendTrimDataPlate2(gTkyKnd, wreg, GType)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���b�Z�[�W�͕\���ς�)
                    Return (r)
                End If
            End If
            '----- V1.18.0.0�F�� -----

            ' OK�{�^�����������ꂽ�ꍇ
            If (gCmpTrimDataFlg = 1) Then                               ' �f�[�^�X�V�t���O(0=�X�V�Ȃ�, 1=�X�V����)
                ' �ʒu�␳���[�h=1(�蓮)�ł��邩�`�F�b�N����
                If (typPlateInfo.intReviseMode = 1) Then
                    ' �ʒu�␳���@�����݂̂ɕύX���� ?
                    'V5.0.0.9�C              ��
                    'If typPlateInfo.intManualReviseType = 1 And (piPP31 <> 1 Or nAutoMode <> 1 Or _
                    '                        dblRotateTheta <> typPlateInfo.dblRotateTheta Or _
                    '                        dblReviseCordnt1XDir <> typPlateInfo.dblReviseCordnt1XDir Or _
                    '                        dblReviseCordnt1YDir <> typPlateInfo.dblReviseCordnt1YDir Or _
                    '                        dblReviseCordnt2XDir <> typPlateInfo.dblReviseCordnt2XDir Or _
                    '                        dblReviseCordnt2YDir <> typPlateInfo.dblReviseCordnt2YDir Or _
                    '                        dblReviseOffsetXDir <> typPlateInfo.dblReviseOffsetXDir Or _
                    '                        dblReviseOffsetYDir <> typPlateInfo.dblReviseOffsetYDir) Then ' V1.19.0.0-27
                    If (typPlateInfo.intManualReviseType = 1) AndAlso
                        ((piPP31 <> 1) OrElse
                         (nAutoMode <> 1) OrElse
                         (dblRotateTheta <> typPlateInfo.dblRotateTheta) OrElse
                         (dblReviseCordnt1XDir <> typPlateInfo.dblReviseCordnt1XDir) OrElse
                         (dblReviseCordnt1YDir <> typPlateInfo.dblReviseCordnt1YDir) OrElse
                         (dblReviseCordnt2XDir <> typPlateInfo.dblReviseCordnt2XDir) OrElse
                         (dblReviseCordnt2YDir <> typPlateInfo.dblReviseCordnt2YDir) OrElse
                         (dblReviseOffsetXDir <> typPlateInfo.dblReviseOffsetXDir) OrElse
                         (dblReviseOffsetYDir <> typPlateInfo.dblReviseOffsetYDir) OrElse
                         (dblReviseCordnt1XDirRgh <> typPlateInfo.dblReviseCordnt1XDirRgh) OrElse
                         (dblReviseCordnt1YDirRgh <> typPlateInfo.dblReviseCordnt1YDirRgh) OrElse
                         (dblReviseCordnt2XDirRgh <> typPlateInfo.dblReviseCordnt2XDirRgh) OrElse
                         (dblReviseCordnt2YDirRgh <> typPlateInfo.dblReviseCordnt2YDirRgh) OrElse
                         (dblReviseOffsetXDirRgh <> typPlateInfo.dblReviseOffsetXDirRgh) OrElse
                         (dblReviseOffsetYDirRgh <> typPlateInfo.dblReviseOffsetYDirRgh)) Then
                        'V5.0.0.9�C          ��
                        gManualThetaCorrection = True                   ' �V�[�^�␳���s�t���O = True(�V�[�^�␳�����s����)
                    End If
                End If
                '----- V1.15.0.0�B�� -----
                ' INtime����ZOFF�ʒu��ݒ肷��
                'r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)            ' INtime����ZOFF�ʒu��zWaitPos�Ƃ��� 'V1.15.0.0�C
                r = SETZOFFPOS(typPlateInfo.dblZWaitOffset)             ' INtime����ZOFF�ʒu��zWaitPos�Ƃ��� 'V1.15.0.0�C
                r = System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then                              ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)
                    Return (r)
                End If
                '----- V1.15.0.0�B�� -----
            End If

            '----- V2.0.0.0_25�� -----
            ' SL436S�̏ꍇ�̓f�[�^�ҏW�ƁuEDIT LOCK/UNLOCK���݁v�������N������׃V�X�p�������[�h����(EDIT LOCK/UNLOCK�{�^���\������̏ꍇ)
            If ((giMachineKd = MACHINE_KD_RS) And (giBtn_EdtLock = 1)) Then
                r = Val(GetPrivateProfileString_S("DEVICE_CONST", "EDITLOCK", SYSPARAMPATH, "0"))
                If (r = 0) Then
                    gSysPrm.stDEV.sEditLock_flg = False                 ' �uEDIT LOCK�v
                Else
                    gSysPrm.stDEV.sEditLock_flg = True                  ' �uEDIT UNLOCK�v
                End If

                '�uEDIT LOCK/UNLOCK���݁v�\��
                If (gSysPrm.stDEV.sEditLock_flg = False) Then
                    ' LOCK
                    CmdCnd.Text = "EDIT LOCK"
                    CmdCnd.BackColor = System.Drawing.SystemColors.Control
                    giPassWord_Lock = 1
                Else
                    ' UNLOCK
                    CmdCnd.Text = "EDIT UNLOCK"
                    CmdCnd.BackColor = System.Drawing.Color.Lime
                    giPassWord_Lock = 0
                End If
            End If
            '----- V2.0.0.0_25�� -----

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
STP_END:
            ''V4.5.0.0�D ��
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)
            ''V4.5.0.0�D ��
            Return (iRtn)

STP_READ_ERR:
            ' "���ԃt�@�C���̓Ǎ��݂Ɏ��s���܂����B"
            Call MsgBox(MSG_133, MsgBoxStyle.OkOnly, "")
            strMSG = MSG_135                                            ' "�f�[�^�ҏW�G���[�B�f�[�^���ă��[�h���Ă��������B"
            iRtn = cFRS_FIOERR_INP                                      ' Return�l = �t�@�C�����̓G���[
            GoTo STP_ERR_DSP

STP_WRITE_ERR:
            ' "���ԃt�@�C���̏������݂Ɏ��s���܂����B"
            Call MsgBox(MSG_134, MsgBoxStyle.OkOnly, "")
            strMSG = MSG_135                                            ' "�f�[�^�ҏW�G���[�B�f�[�^���ă��[�h���Ă��������B"
            iRtn = cFRS_FIOERR_OUT                                      ' Return�l = �t�@�C���o�̓G���[
            GoTo STP_ERR_DSP

STP_ERR_DSP:
            ' Form�����ɖ߂�
            Me.WindowState = FormWindowState.Normal
            ' "�f�[�^�ҏW�G���[�B�f�[�^���ă��[�h���Ă��������B"
            Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
            Call Z_PRINT(strMSG)                                        ' ���b�Z�[�W�\��(���O���)
            gStrTrimFileName = String.Empty                             ' V4.0.0.0-41
            '----- V6.1.4.0�F��(KOA EW�aSL432RD�Ή�) -----
            ' �g���~���O�f�[�^���\����N���A
            'LblDataFileName.Text = String.Empty                         ' V4.0.0.0-41
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = String.Empty                      ' �g���~���O�f�[�^���\����Ƒ�P��R�f�[�^�\����N���A  
            Else
                LblDataFileName.Text = String.Empty
            End If
            '----- V6.1.4.0�F�� -----
            gLoadDTFlag = False                                         ' gLoadDTFlag = �f�[�^���[�h�σt���OOFF
            GoTo STP_END

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.ExecEditProgram() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            iRtn = cERR_TRAP                                            ' Return�l = �g���b�v�G���[����
            GoTo STP_END

        Finally                                                         'V6.0.0.0-29
            'V4.12.2.0�@                 ��
            If (0 = Mode) AndAlso ((ExitCode = PRC_EXIT_NML) OrElse (ExitCode = PRC_EXIT_NML2)) Then
                ' �ꎞ��~��ʂ���̌Ăяo���ł͂Ȃ��ꍇ
                ' �ҏW��ʃL�����Z���ł͂Ȃ�����I��
                Dim doEnabled As Boolean = (cFRS_NORMAL = iRtn)
                MapInitialize(doEnabled)                                ' �}�b�v�ĕ`��
            End If
            'V4.12.2.0�@                 ��

            VideoLibrary1.VideoStart()
        End Try

    End Function
#End Region
    '----- ###240��-----
#Region "�ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��)"
    '''=========================================================================
    ''' <summary>�ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����
    '''          3=�����(Cancel(RESET��)�@
    '''          ��L�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function SubstrateExistCheck(ByVal ObjSys As Object) As Integer

        Dim strMSG As String
        Dim r As Integer

        Try
            ' �ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��)
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                r = Sub_SubstrateExistCheck_432R(ObjSys)                ' SL432R�p 
            Else
                r = Sub_SubstrateExistCheck(ObjSys)                     ' SL436R�p 
            End If
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.SubstrateExistCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###240��-----
    '----- V6.0.3.0�D�� -----
#Region "QR�R�[�h�ǂݍ��݂̐����ݒ�p(���[���a����)"
    '''=========================================================================
    ''' <summary>QR�R�[�h�ǂݍ��݂̐����ݒ�p(���[���a����)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnQRLmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQRLmit.Click

        Dim ret As Long

        If btnQRLmit.BackColor = Color.Lime Then
            ' �A�h�~�j�X�g���[�^���x���Ń��O�I�����Ă��Ȃ��ƕύX�ł��Ȃ�
            If lblLoginResult.Visible = True Then
                ret = Password1.ShowDialog(gSysPrm.stTMN.giMsgTyp, gSysPrm.stSYP.gsLoginPassword)
                If ret = 1 Then
                Else
                    Return
                End If
            Else
                If gSysPrm.stTMN.giMsgTyp = 0 Then
                    ret = MsgBox("�N�����p�X���[�h����͂��Ȃ��ƕύX�ł��܂���B", vbOKOnly)
                Else
                    ret = MsgBox("Cannot change , because not input initial password. ", vbOKOnly)
                End If

                Return
            End If

            btnQRLmit.BackColor = System.Drawing.SystemColors.Control
        Else
            ' 
            If gSysPrm.stTMN.giMsgTyp = 0 Then
                ret = MsgBox("QR���������ėǂ��ł����H", vbYesNo)
            Else
                ret = MsgBox("Do you set limit for QR-Read?", vbYesNo)
            End If
            If ret = vbNo Then
                Return
            End If

            btnQRLmit.BackColor = Color.Lime
        End If

    End Sub
#End Region
    '----- V6.0.3.0�D�� -----
    '----- V6.0.3.0_30�� -----
#Region "QR�����̏����𖞂����Ă��邩�̔�����s��(���[���a����)"
    '''=========================================================================
    ''' <summary>QR�����̏����𖞂����Ă��邩�̔�����s��(���[���a����)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function ChkQRLimitStatus() As Integer

        ChkQRLimitStatus = cFRS_NORMAL

        If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return (cFRS_NORMAL) ' ���[���a�����łȂ����NOP

        If CbDigSwL.SelectedIndex <= TRIM_MODE_FT Then                       ' x0,x1,x2���[�h���`�F�b�N
            If QR_Read_Flg = 0 Then

                If btnQRLmit.BackColor = Color.Lime Then
                    If gSysPrm.stTMN.giMsgTyp = 0 Then
                        MsgBox("QR�R�[�h��ǂݍ���ł��Ȃ��̂Ŏ����^�]���J�n�ł��܂���B")
                    Else
                        MsgBox("Cannot Auto Operation , Because not read Qr-Code. ")
                    End If
                    ChkQRLimitStatus = cFRS_ERR_RST

                End If

            End If
        End If

    End Function
#End Region
    '----- V6.0.3.0_30�� -----
    '----- V6.0.3.0�C�� -----
#Region "�o�b�N�A�b�v�p�t�@�C���̍쐬(���[���a����)"
    '''=========================================================================
    ''' <summary>�o�b�N�A�b�v�p�t�@�C���̍쐬(���[���a����)</summary>
    ''' <param name="OrgFullPath">(INP)�t�@�C����(Full Path)</param>
    '''<returns>�o�b�N�A�b�v�p�t�@�C����(Full Path)</returns>
    '''=========================================================================
    Private Function MakeBackupFile(ByVal OrgFullPath As String) As String

        Dim DateStr As String
        Dim RetStr As String
        Dim OrgFileName As String
        Dim OrgFolderName As String
        Dim FLCondFile As String

        Try

            MakeBackupFile = ""

            OrgFileName = System.IO.Path.GetFileName(OrgFullPath)
            OrgFolderName = System.IO.Path.GetDirectoryName(OrgFullPath)
            DateStr = Now.ToString("yyyyMMdd") + "_" + Now.ToString("HHmmss") + OrgFileName
            RetStr = DATA_DIR_PATH + "\" + Now.ToString("yyyyMMdd") + "\"
            If System.IO.Directory.Exists(RetStr) = False Then
                MkDir(RetStr)
            End If
            MakeBackupFile = RetStr + DateStr
            FileSystem.FileCopy(OrgFullPath, MakeBackupFile)            ' �t�@�C���������Ă��㏑�������

            OrgFileName = System.IO.Path.ChangeExtension(OrgFullPath, "xml")
            FLCondFile = System.IO.Path.ChangeExtension(MakeBackupFile, "xml")

            FileSystem.FileCopy(OrgFileName, FLCondFile)                ' �t�@�C���������Ă��㏑�������

        Catch ex As Exception
            MakeBackupFile = ""

            Dim strMSG As String
            strMSG = "MakeBackupFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        Exit Function

    End Function
#End Region
    '----- V6.0.3.0�C�� -----

    '========================================================================================
    '   �^�C�}�[�C�x���g����
    '========================================================================================
#Region "�����N���^�C�}�[����"
    '''=========================================================================
    '''<summary>�����N���^�C�}�[����</summary>
    '''<remarks>�z�X�g�R�}���h�̎擾�Ə���
    '''         START SW�����E�f�W�X�C�b�`�̎擾�Ə���
    '''         ����~���̃`�F�b�N</remarks>
    '''=========================================================================
    Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick

        Dim r As Integer
        Dim fStartTrim As Boolean
        Dim fClamp As Boolean
        Dim strHead As String
        Dim intRet As Short
        Dim swStatus As Integer
        Dim interlockStatus As Integer
        Dim sldCvrSts As Integer
        fClamp = False
        Dim InterlockSts As Integer     ' ###006
        Dim Sw As Long                  ' ###006
        Dim coverSts As Long            ' ###109
        Dim iRtn As Integer             'V5.0.0.6�A

        '---------------------------------------------------------------------------
        '   ��������
        '---------------------------------------------------------------------------
        Timer1.Enabled = False                              ' �Ď��^�C�}�[��~
        strHead = ""

        Try

            ' �����/LOAD/SAVE/EDIT/LOTCHG����ވȊO�̏ꍇ(OCX�g�p�����)����ϰ��~���Ă��̂܂ܔ�����
            '' OCX����Ԃ��Ă�����Ď���ϰ���J�n����
            'If (giAppMode <> APP_MODE_IDLE) And _
            '   (giAppMode <> APP_MODE_LOAD) And _
            '   (giAppMode <> APP_MODE_SAVE) And _
            '   (giAppMode <> APP_MODE_LOGGING) And _
            '   (giAppMode <> APP_MODE_EDIT) Then
            '    Call ZCONRST()                                  ' �R���\�[���L�[���b�`����
            '    Exit Sub
            'End If
            '###135 APP_MODE_AUTO�ǉ�
            If (giAppMode <> APP_MODE_IDLE) And
               (giAppMode <> APP_MODE_LOAD) And
               (giAppMode <> APP_MODE_SAVE) And
               (giAppMode <> APP_MODE_AUTO) And
               (giAppMode <> APP_MODE_LOGGING) Then
                Call ZCONRST()                                  ' �R���\�[���L�[���b�`����
                Exit Sub
            End If

            '---------------------------------------------------------------------------
            '   �Ď������J�n
            '---------------------------------------------------------------------------
            fStartTrim = False                                  ' �X�^�[�gTRIM�t���O=OFF

            ' ����~���`�F�b�N(�g���}���u�A�C�h����)
            r = System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
            If (r <> cFRS_NORMAL) Then                          ' ����~�����o ?
                GoTo TimerErr                                   ' �A�v�������I��
            End If
            '---------------------------------------------------------------------------
            '   HALT�X�C�b�`����
            '   ��(2012/01/27)
            '   �@TRIMMING���{���Ȃǂ�HALT�X�C�b�`���������ꂽ�ꍇ�A�\�t�g�ŏ�ԕω���
            '   �@�Ď����邱�Ƃ͕s�\�B����āAADJ�Ɠ��l�̓���ƂȂ�悤�Ƀn�[�h�ύX���K�v�B
            '   �@�n�[�h�ύX�����{�����ꍇ�A���L�̏����͕s�v�ƂȂ邽�߃R�����g������B
            '---------------------------------------------------------------------------
            '' '' ''r = HALT_SWCHECK(swStatus)
            '' '' ''If (r <> cFRS_NORMAL) Then
            '' '' ''    GoTo TimerErr
            '' '' ''End If
            '' '' ''If (swStatus = cSTS_HALTSW_ON) Then
            '' '' ''    If (m_keepHaltSwSts = False) Then
            '' '' ''        'HALT LAMP���I������B  
            '' '' ''        r = LAMP_CTRL(LAMP_HALT, True)
            '' '' ''        m_keepHaltSwSts = True
            '' '' ''    Else
            '' '' ''        'HALT LAMP���I�t����B  
            '' '' ''        r = LAMP_CTRL(LAMP_HALT, False)
            '' '' ''        m_keepHaltSwSts = False
            '' '' ''    End If
            '' '' ''End If
            '' '' ''If (r <> cFRS_NORMAL) Then
            '' '' ''    GoTo TimerErr
            '' '' ''End If

            '---------------------------------------------------------------------------
            '   �C���^�[���b�N��Ԏ擾
            '---------------------------------------------------------------------------
            r = DispInterLockSts()                                  ' �C���^�[���b�N��Ԃ̕\��/��\������ю擾
            ' �C���^�[���b�N�S����/�ꕔ�����ŁA�J�o�[�ُ͈�Ƃ���(SL436R��) ###109
            If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then 'V4.0.0.0�M
                'If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gKeiTyp <> KEY_TYPE_RS) Then
                r = COVER_CHECK(coverSts)                           ' �Œ�J�o�[��Ԏ擾(0=�Œ�J�o�[�J, 1=�Œ�J�o�[��))
                If (coverSts = 1) Then                              ' �Œ�J�o�[�� ?
                    ' �n�[�h�E�F�A�G���[(�J�o�[�X�C�b�`�I��)���b�Z�[�W�\��
                    Call System1.Form_Reset(cGMODE_ERR_HW, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                    GoTo TimerErr                                   ' �A�v�������I��
                End If
            End If

            If gKeiTyp = KEY_TYPE_RS Then
                SimpleTrimmer.CommandButtonTutorial()               ' V2.0.0.0�I �R�}���h�{�^���̃`���[�g���A��
            End If
            '-----------------------------------------------------------------------
            '   ���[�_�����Ȃ烍�[�_����f�[�^����͂���i�R�}���h��M�j(��SL432R�n��)
            '-----------------------------------------------------------------------
            Call GetLoaderIO(gLdRDate)                              ' ���[�_����f�[�^����͂���(���j�^�p)
            ' SL432�n�̂݉��L���s��
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (giAppMode = APP_MODE_IDLE) Then
                ' ���[�_�����Ń��[�_�L�肩�烍�[�_�����̕ω��̓G���[�Ƃ���
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = False) Then
                    ' ���[�_���蓮&��~�ɐ؂�ւ��܂ő҂�
                    intRet = System1.Form_Reset(cGMODE_LDR_ERR, gSysPrm, giAppMode, gbInitialized,
                                        typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                    If (intRet <= cFRS_ERR_EMG) Then GoTo TimerErr ' ����~���̃G���[�Ȃ�A�v�������I��
                End If

                ' ���[�_����f�[�^����͂���
                r = System1.ReadHostCommand(gSysPrm, giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag)
                'r = System1.ReadHostCommand_ForVBNET(giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag)

                ' ���[�_�������œ��쒆�̓{�^���񊈐��� ###035
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = True) Then
                    Call Form1Button(0)                             ' �R�}���h�{�^����񊈐����ɂ���
                    '----- V6.1.4.0_40��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
                    If (giLotChange = 1) Then                       ' ���b�g�ؑւ��@�\�L�� ?
                        If frmAutoObj.GetAutoOpeCancelStatus() Then ' �����^�]���~ ?
                            '' �u�g���}���쒆(BIT0)�v��OFF���Ă���łȂ���cmd_ZAtldGet()�Ń��[�_����(BIT1)�̂܂܂ƂȂ� V6.1.4.0_45
                            'Call SetLoaderIO(0, COM_STS_TRM_STATE)  ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}���쒆) V6.1.4.0_45
                            ' "���[�_�M���������ł�", "���[�_���蓮�ɐ؂�ւ��Ă�������", "Cancel�{�^�������Ńv���O�������I�����܂�"
                            r = Me.System1.Form_Reset(cGMODE_LDR_CHK, gSysPrm, giAppMode, gbInitialized, 0, 0, gLoadDTFlag)
                            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then ' ����~���̃G���[ ?
                                'MsgBox("i-TKY.Form_Initialize()::Form_Reset(cGMODE_LDR_CHK) error." + vbCrLf + " Err Code = " + r.ToString)
                                'End                                ' �A�v���I��
                                GoTo TimerErr                       ' �G���[�Ȃ�A�v�������I����
                            ElseIf (r = cFRS_ERR_RST) Then          ' RESET(Cancel�{�^��)���� ? 
                                If frmAutoObj.gbFgAutoOperation432 = True Then
                                    ' �A�������^�]�I������(�����^�]�I�������I�����ɐ��Y�Ǘ��f�[�^���o�͂���Ȃ������̂Œǉ�)
                                    Call frmAutoObj.AutoOperationEnd()
                                End If
                                GoTo TimerErr                       ' �A�v�������I����
                            End If
                            ' ���펞
                            Call SetLoaderIO(0, COM_STS_TRM_STATE)  ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}���쒆)
                        End If
                    End If
                    '----- V6.1.4.0_40�� -----
                Else
                    Call Form1Button(1)                             ' �R�}���h�{�^����L���ɂ���
                    '----- V6.1.4.0�I��(KOA EW�aSL432RD�Ή�)�y���b�g�ؑւ��@�\�z-----
                    If (giLotChange = 1) Then                       ' ���b�g�ؑւ��@�\�L�� ?
                        If (frmAutoObj.gbFgAutoOperation432 = True) Then    ' �����^�]�� ?
                            Call frmAutoObj.AutoOperationEnd()              ' �A�������^�]�I������
                            ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}���쒆, NG, �p�^�[��NG)
                            Call SetLoaderIO(0, COM_STS_TRM_STATE Or COM_STS_TRM_NG Or COM_STS_PTN_NG)
                        End If
                    End If
                    '----- V6.1.4.0�I�� -----
                End If
                'SimpleTrimmer.CommandButtonTutorial()               ' V2.0.0.0�I �R�}���h�{�^���̃`���[�g���A��

                '-----------------------------------------------------------------------
                '   ���[�_�����M�����f�[�^���`�F�b�N����
                '-----------------------------------------------------------------------
                If gbHostConnected = True And r >= 0 Then           ' �z�X�g�ڑ��Ńf�[�^��M ?
                    If giHostMode = cHOSTcMODEcAUTO Then            ' ۰�� = ����Ӱ�� ?
                        Select Case r
                            ' ��ϰ���ĐM����M��
                            'Case LINP_TRM_START                            'V6.1.4.0�I
                            ' �g���}�[�X�^�[�g�M���܂��̓��b�g�ؑւ��M��(����)��M��
                            Case LINP_TRM_START, LINP_TRM_LOTCHANGE_START   'V6.1.4.0�I

                                ' @@@888 ��
                                If (frmAutoObj.gbFgAutoOperation432 = False) Then    ' �����^�]���łȂ��̂ɁA���[�_����START/Lot�؂�ւ��M������M����

                                    Exit Select
                                End If
                                ' @@@888 ��


                                fStartTrim = True                   ' �X�^�[�gTRIM�t���O=ON
                                '----- V6.1.4.0�I��(KOA EW�aSL432RD�Ή��y���b�g�ؑւ��@�\�z) -----
                                If (giLotChange = 1) Then           ' ���b�g�ؑւ��@�\�L�� ?
                                    If (r = LINP_TRM_LOTCHANGE_START) Then                  ' �u���b�g�ؑւ��M���v��M ?
                                        If (frmAutoObj.gbFgAutoOperation432 = True) Then    ' �����^�]�� ?
                                            If frmAutoObj.LotChangeExecute() Then           ' ���b�g�ؑւ����� 
                                                fStartTrim = True                           ' �X�^�[�gTRIM�t���O=ON
                                                ' V6.1.4.0_35��
                                                ' ���[�U�[�p���[�̃��j�^�����O�t���O�ݒ�(0�F����,1�F�����^�]�J�n��,2�F�G���g���[���b�g��)
                                                If (giLaserrPowerMonitoring = 2) Then       ' �G���g���[���b�g���Ƀ��j�^�����O ?
                                                    gbLaserPowerMonitoring = True           ' ���j�^�����O�t���O ON
                                                Else
                                                    gbLaserPowerMonitoring = False          ' ���j�^�����O�t���O OFF
                                                End If
                                                ' V6.1.4.0_35��
                                                'V6.1.4.2�@��
                                                If Integer.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CALIBRATION_LOTCHANGE_EXEC", "C:\TRIM\tky.ini", "0")) = 1 Then
                                                    gbAutoCalibrationExecute = True
                                                End If
                                                'V6.1.4.2�@��
                                            Else
                                                fStartTrim = False                          ' �X�^�[�gTRIM�t���O=OFF
                                                frmAutoObj.SetAutoOpeCancel()               ' ���[�_�o��(ON=�g���~���O�m�f, OFF=�Ȃ�)�g���~���O�m�f�M����A�������^�]���~�ʒm�Ɏg�p
                                                ' "���b�g�؂�ւ��M�����󂯂܂������A���̃��b�g�̓G���g���[����Ă��܂���B"
                                                Call Z_PRINT(MSG_166 & vbCrLf)
                                            End If
                                        End If
                                    End If
                                End If
                                '----- V6.1.4.0�I�� -----

                                ' ���샍�O�o��
                                Call System1.OperationLogging(gSysPrm, MSG_OPLOG_HCMD_TRMCMD, "HOSTCMD")
                                '###006 If (System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0 Then
                                r = INTERLOCK_CHECK(InterlockSts, Sw)               ' �C���^�[���b�N��Ԏ擾
                                If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then  ' �C���^�[���b�N���(�����Ȃ�)�łȂ� ? ###108
                                    Call ZSELXYZSPD(1)              ' XYZ Slow speed
                                    gPrevInterlockSw = 1
                                Else
                                    Call ZSELXYZSPD(0)              ' XYZ Normal speed
                                    gPrevInterlockSw = 0
                                End If

                                'V5.0.0.6�A��
                            Case LINP_HST_MOVESUPPLY                ' �R�}���h�������ʒu�ړ��w���Ȃ�
                                If gbLoaderSecondPosition And giAppMode = APP_MODE_IDLE Then
                                    iRtn = Me.System1.EX_SLIDECOVERCHK(1)   ' �X���C�h�J�o�[�`�F�b�N���Ȃ�
                                    If (iRtn = cFRS_NORMAL) Then
                                        iRtn = Me.System1.EX_SMOVE2(gSysPrm, basTrimming.GetLoaderBordTableInPosX(), basTrimming.GetLoaderBordTableInPosY())
                                    End If
                                    If (iRtn = cFRS_NORMAL) Then
                                        Call SetLoaderIO(COM_STS_TRM_COMP_SUPPLY, 0)    ' ���[�_�[�o��(ON=�����ʒu�ړ�����)
                                        ' Call Sub_ATLDSET(COM_STS_TRM_COMP_SUPPLY, COM_STS_TRM_STATE)    ' ���[�_�[�o��(ON=�����ʒu�ړ�����,OFF=�g���}���쒆)
                                    Else
                                        Call SetLoaderIO(COM_STS_TRM_ERR, 0)    ' ���[�_�[�o��(ON=�g���}�G���[)
                                        Call Me.System1.TrmMsgBox(gSysPrm, MSG_SPRASH64, MsgBoxStyle.OkOnly, My.Application.Info.Title)    ' "�X�e�[�W�����ʒu�ړ��G���[���������܂����B"
                                        Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ���[�_�[�o��(OFF=�g���}�G���[)
                                    End If
                                    iRtn = Me.System1.EX_SLIDECOVERCHK(0)   ' �X���C�h�J�o�[�`�F�b�N����B
                                    If (iRtn <> cFRS_NORMAL) Then
                                        Call Me.System1.TrmMsgBox(gSysPrm, MSG_SPRASH65, MsgBoxStyle.OkOnly, My.Application.Info.Title)     ' "�X���C�h�J�o�[�`�F�b�N�iEX_SLIDECOVERCHK(0)�j�ݒ�G���[���������܂����B"
                                    End If
                                End If
                                'V5.0.0.6�A��
                        End Select
                    End If                                          ' �g���}����~��
                End If
            End If

            '-----------------------------------------------------------------------
            '   ���[�_�̏�Ԃ��m�F����(��SL436R�n��)
            '-----------------------------------------------------------------------
            ' ###073 �폜
            ''If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
            ''    Dim AlarmCount As Integer
            ''    Dim strLoaderAlarm(LALARM_COUNT) As String
            ''    Dim strLoaderAlarmInfo(LALARM_COUNT) As String
            ''    Dim strLoaderAlarmExec(LALARM_COUNT) As String

            ''    ' ���[�_�A���[�����擾
            ''    ' ���[�_�A���[�����b�Z�[�W�쐬 & ���[�_�A���[����ʕ\��
            ''    r = Loader_AlarmCheck(gSysPrm, False, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            ''    If (r <> cFRS_NORMAL) Then
            ''        Dim objForm As Object = Nothing

            ''        ' frmReset���g�p���ăG���[���b�Z�[�W�̕\�����s���B
            ''        objForm = New FrmReset()
            ''        Call objForm.ShowDialog(Nothing, cGMODE_LDR_ALARM, Me.System1)
            ''        r = objForm.sGetReturn()                                    ' Return�l�擾

            ''        ' �I�u�W�F�N�g�J��
            ''        If (objForm Is Nothing = False) Then
            ''            Call objForm.Close()                                    ' �I�u�W�F�N�g�J��
            ''            Call objForm.Dispose()                                  ' ���\�[�X�J��
            ''        End If

            ''        ' ���[�_�A���[�����o�̓A�v�������I�����Ȃ� 
            ''        If (r <> cFRS_NORMAL) Then
            ''            If (r = cFRS_ERR_RST) Or (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDRTO) Then
            ''                Call W_RESET()                                      ' �A���[�����Z�b�g
            ''            Else
            ''                GoTo TimerErr                                       ' PLC�X�e�[�^�X�ُ퓙�̓A�v�������I����
            ''            End If
            ''        End If

            ''    End If
            ''End If

            ' '' '' '' ���z�}�\��(TKY/CHIP�̂�)
            '' '' ''If (Me.chkDistributeOnOff.Checked = True) Then    ' ���Y���̕\���׸�ON ?
            '' '' ''    frmDistribution.RedrawGraph()           ' ���̍X�V
            '' '' ''End If

            '---------------------------------------------------------------------------
            '   ���[�_�[�Ȃ��̏ꍇ
            '---------------------------------------------------------------------------
            If (giHostMode = cHOSTcMODEcMANUAL) Then
                'swStatus = Me.System1.InterLockSwRead()
                r = INTERLOCK_CHECK(interlockStatus, swStatus)
                If (r <> ERR_CLEAR) Then                                    ' �����b�Z�[�W�\����ǉ����� 
                    '➑̃J�o�[�J�̏ꍇ�A�C���^�[���b�N�������J�o�[���Ď�����
                    If (r = ERR_OPN_CVR) Then
                        '----- V1.22.0.0�H�� -----
                        intRet = System1.Sub_CoverCheck(gSysPrm, 0, False)
                        'intRet = System1.Form_Reset(cGMODE_CVR_CLOSEWAIT, gSysPrm, giAppMode, False, _
                        '                   typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                        '----- V1.22.0.0�H�� -----
                        If (intRet <= cFRS_ERR_EMG) Then GoTo TimerErr ' ����~���̃G���[�Ȃ�A�v�������I��
                        '�X�^�[�g�X�C�b�`���b�`�N���A
                        Call ZCONRST()
                        'If (intRet = ERR_OPN_CVRLTC) Then COVERLATCH_CLEAR() ' ���b�`�N���A
                    ElseIf (r = ERR_OPN_SCVR Or r = ERR_OPN_CVRLTC) Then
                        ' SL432R�̏ꍇ�̓J�o�[�J���b�`�͖�������B
                        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then
                            '----- 'V4.0.0.0�O�� -----
                            intRet = System1.Sub_CoverCheck(gSysPrm, 0, False)
                            If (intRet <= cFRS_ERR_EMG) Then GoTo TimerErr ' ����~���̃G���[�Ȃ�A�v�������I��
                            '                            GoTo TimerErr
                            '----- 'V4.0.0.0�O�� -----
                        End If
                    Else
                        GoTo TimerErr
                    End If
                End If

                '---------------------------------------------------------------------------
                '   �r�s�`�q�s�@�r�v�̉����`�F�b�N(SL432R/SL436R����)
                '---------------------------------------------------------------------------
                If (giAppMode = APP_MODE_IDLE) Then         ' �A�C�h�����[�h���Ƀ`�F�b�N����
                    r = START_SWCHECK(0, swStatus)          ' �g���}�[ START SW �����`�F�b�N
                    If (swStatus = cSTS_STARTSW_ON) Then
                        '----- V3.0.0.0�C(V1.22.0.0�B)��-----
                        ' �f�[�^���[�h�ς݃`�F�b�N
                        If ChkTrimDataLoaded() <> cFRS_NORMAL Then
                            Timer1.Enabled = True                               ' �^�C�}�[�ċN��
#If START_KEY_SOFT Then
                            If gbStartKeySoft Then
                                Call ZCONRST()                                          ' ���b�`����
                            End If
#End If
                            Exit Sub
                        End If
                        '----- V3.0.0.0�C(V1.22.0.0�B)��-----
                        ''V4.0.0.0-86
                        r = GetLaserOffIO(True) 'V5.0.0.1�K
                        If r = 1 Then
                            Call ZCONRST()                                      ' �R���\�[���L�[���b�`����'V5.0.0.1�J
                            Timer1.Enabled = True                               ' �^�C�}�[�ċN��
                            Exit Sub
                        End If
                        ''V4.0.0.0-86

                        '----- V6.0.3.0_30�� -----
                        ' QR�R�[�h��ǂݍ��񂾂��`�F�b�N 
                        r = ChkQRLimitStatus()
                        If r <> cFRS_NORMAL Then
                            Timer1.Enabled = True                               ' �^�C�}�[�ċN��
                            Exit Sub
                        End If
                        '----- V6.0.3.0_30�� -----

                        '----- ###240��-----
                        ' �ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��(OPTION))
                        r = SubstrateExistCheck(System1)
                        If (r <> cFRS_NORMAL) Then                              ' �G���[ ? 
                            If (r = cFRS_ERR_RST) Then                          ' �����(Cancel(RESET��)�@?
                                Timer1.Enabled = True                           ' �^�C�}�[�ċN��
                                Exit Sub
                            End If
                            GoTo TimerErr                                       ' ���̑��̃G���[�Ȃ�A�v�������I��
                        End If
                        '----- ###240��-----

                        Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "START SW ON(VAC/CLAMP ON)")
RETRY_VACUME:           ' V1.16.0.2�A
                        ' �N�����v/�z��ON

                        'V4.5.0.1�@��
                        '����Ή��̏ꍇ�ɂ́A�N�����v�V�[�P���X�ύX���� 
                        If (giClampSeq = 1) Then

                            Dim lData As Long = 0
                            Dim lBit As Long = 0
                            Dim rtn As Integer = cFRS_NORMAL
                            Dim strMSG As String = ""
                            Dim strMS2 As String = ""
                            Dim strMS3 As String = ""
                            Dim bFlg As Boolean = True

                            ' �ڕ���Ɋ�����鎖���`�F�b�N����
                            '�N�����vON
                            ''----- V1.16.0.0�K�� -----
                            'If (gSysPrm.stIOC.giClamp = 1) Then
                            '    Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                            '    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                            '    Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
                            'End If
                            '----- V1.16.0.0�K�� -----
                            '�z��ON
                            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
                            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ��200ms���ƃ��[�N�L�����o����Ȃ��ꍇ������
                            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                                ' SL436S��
                                r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
                                lBit = LDSTS_VACUME
                            Else
                                ' SL436R��
                                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                                lBit = LDST_VACUME
                            End If
                            ' ���[�N�����Ȃ烁�b�Z�[�W�\��
                            If (lData And lBit) Then                                    ' �ڕ���Ɋ�L ? V2.0.0.0�E
                                r = 0
                            Else
                                ' �u�Œ�J�o�[�J�P�`�F�b�N�Ȃ��v�ɂ���
                                Call COVERCHK_ONOFF(COVER_CHECK_OFF)
                                ' �\�����b�Z�[�W��ݒ肷��
                                strMSG = MSG_LDALARM_06                                 ' "�ڕ���z���~�X"
                                strMS2 = MSG_SPRASH52                                   ' "���u���čēx���s���ĉ�����"
                                strMS3 = MSG_SPRASH53                                             ' ""

                                'r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                                '        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                                ' ���b�Z�[�W�\��(START�L�[�����҂�)
                                r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                                If r = 1 Then ' START��Retry
                                ElseIf r = 3 Then
                                    ' Cancel�������ꂽ��I��
                                    r = 2
                                End If
                            End If

                        Else
                            'Call Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)       '###060
                            r = Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)         '###060
                            If (r <> cFRS_NORMAL) Then
                                Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
                                Call AplicationForcedEnding()                       ' ��ċ����I������
                                End                                                 ' �A�v�������I��
                            End If

                            '----- V1.16.0.0�O�� -----
                            'r = Me.System1.AbsVaccume(gSysPrm, 1, giAppMode, giTrimErr)        ' V1.19.0.0-32
                            r = Me.System1.AbsVaccume(gSysPrm, 1, APP_MODE_TRIM, giTrimErr)     ' V1.19.0.0-32
                            'Call Me.System1.Adsorption(gSysPrm, 1)
                        End If
                        'V4.5.0.1�@��

                        Select Case r
                            Case 1                                              ' �z���G���[�Ń��g���C�w��
                                GoTo RETRY_VACUME
                            Case 2                                              ' �z���G���[�Œ��~�w��
                                GoTo TimerExit
                        End Select
                        '----- V1.16.0.0�O�� -----

                        ' �X�^�[�gSW�𗣂��܂ő҂�INTRIM�ɂĊĎ��̖������[�v
                        Call START_SWCHECK(1, swStatus)
                        'V5.0.0.6�R ADD START��
                        ' ����SW�����҂����Ȃ��ꍇ�̓��b�Z�[�W�\������
                        Call ZCONRST()                                                          ' ���b�`����
                        If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                            r = INTERLOCK_CHECK(interlockStatus, swStatus)
                            If interlockStatus = INTERLOCK_STS_DISABLE_NO Then
                                ' "���ӁI�I�I�@�X���C�h�J�o�[�������ŕ��܂��B"(Red,Blue)
                                r = Me.System1.Form_MsgDispStartReset(MSG_SPRASH31, MSG_SPRASH32, &HFF, &HFF0000)
                                Call ZCONRST()                                                  ' ���b�`����
                                If (r = cFRS_ERR_START) Then
                                    r = Me.System1.Z_CCLOSE(gSysPrm, giAppMode, 0, False, 0.0, 0.0)
                                    If r = cFRS_NORMAL Then
                                    ElseIf r = cFRS_TO_SCVR_CL Then '�^�C���A�E�g(�X���C�h�J�o�[�҂�)
                                        GoTo TimerErr
                                    Else
                                        Timer1.Enabled = True                                   ' �^�C�}�[�ċN��
                                        Exit Sub
                                    End If
                                ElseIf (r = cFRS_ERR_RST) Then
                                    ' RESET SW�����Ȃ�
                                    'V6.1.4.0_42��
                                    ' "�X���C�h�J�o�[���J���Ă�������"�\����X���C�h�J�o�[�J�҂�
                                    r = Me.System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                                    If r <= cFRS_ERR_EMG Then
                                        GoTo TimerErr
                                    End If
                                    'V6.1.4.0_42�� 
                                    Timer1.Enabled = True                                       ' �^�C�}�[�ċN��
                                    Exit Sub
                                Else
                                    GoTo TimerErr                                               ' ����~��
                                End If
                            End If
                        End If
                        'V5.0.0.6�R ADD END��

                        'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "START SW RELEASED(XY START)")'###030
                        'V5.0.0.6�R�R�����gSTART��
                        ' SL432R�Ž���SW�����҂����Ȃ��ꍇ�̓��b�Z�[�W�\������
                        'If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                        '    '----- V3.0.0.0�F(V1.22.0.0�D) �� -----
                        '    r = INTERLOCK_CHECK(interlockStatus, swStatus)
                        '    If (interlockStatus = INTERLOCK_STS_DISABLE_NO) Then         ' �C���^�[���b�N���(�����Ȃ�) ?
                        '        ' �X���C�h�J�o�[�����N���[�Y����START/RESET�L�[�����҂�
                        '        r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                        '        Me.Refresh()
                        '        If (r < cFRS_NORMAL) Then
                        '            Call AppEndDataSave()                       ' ��ċ����I�������ް��ۑ��m�F
                        '            Call AplicationForcedEnding()               ' ��ċ����I������
                        '            End                                         ' �A�v�������I��
                        '        End If
                        '    End If
                        '    '    ' "���ӁI�I�I�@�X���C�h�J�o�[�������ŕ��܂��B"(Red,Blue)
                        '    '    'r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH32, &HFF, &HFF0000)
                        '    '    ' ���b�Z�[�W�\��(START�L�[�����҂�)
                        '    '    r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        '    '            MSG_SPRASH31, MSG_SPRASH32, "", System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        '    '    If (r < cFRS_NORMAL) Then
                        '    '        Call AppEndDataSave()                       ' ��ċ����I�������ް��ۑ��m�F
                        '    '        Call AplicationForcedEnding()               ' ��ċ����I������
                        '    '        End                                         ' �A�v�������I��
                        '    '    End If

                        '    '    ' �X���C�h�J�o�[�������N���[�Y����
                        '    '    Call ZSLCOVEROPEN(0)                            ' �ײ�޶�ް����������OFF
                        '    '    Call ZSLCOVERCLOSE(1)                           ' �ײ�޶�ް�۰�������ON
                        '    '----- V3.0.0.0�F(V1.22.0.0�D)�� -----
                        'End If
                        'V5.0.0.6�R�R�����gEND��

                        'V4.6.0.0�@��
                        'V4.10.0.0�I                        If (giMachineKd = MACHINE_KD_RS) And (giManualSeq = 1) Then
                        If (gMachineType = MACHINE_TYPE_436S) AndAlso (giManualSeq = 1) Then
                            '// ����Start�L�[�҂�
                            ' �e�R�}���h�������s���̽ײ�޶�ް�����۰�ޖ���START/RESET�L�[�����҂�
                            r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                            Me.Refresh()
                            '�X�^�[�g�X�C�b�`���b�`�N���A
                            Call ZCONRST()
                            'V5.0.0.1�P��
                            If (r <= cFRS_ERR_EMG) Then
                                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
                                Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF)
                                '    GoTo TimerExit
                                GoTo TimerErr                                       ' ���̑��̃G���[�Ȃ�A�v�������I��
                            ElseIf (r = cFRS_ERR_RST) Then                     ' �װ(����~��)�Ȃ��ċ����I��
                                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
                                Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF)
                                GoTo TimerExit
                            End If
                            'V5.0.0.1�P��
                        End If
                        'V4.6.0.0�@��

                        fStartTrim = True                                       ' �X�^�[�gTRIM�t���OON
                        stPRT_ROHM.bAutoMode = False                            ' �g���~���O���ʈ���f�[�^�Ɏ蓮�^�]��ݒ�(���[���a����) V1.18.0.0�B
                        gbLaserPowerMonitoring = False                          ' �蓮���̓t���p���[������s��Ȃ��@V6.1.4.6�@
                    Else
                        '---------------------------------------------------------------------------
                        '   �X���C�h�J�o�[��Ԃ̃`�F�b�N(SL432R��)
                        '---------------------------------------------------------------------------
                        If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (interlockStatus = INTERLOCK_STS_DISABLE_NO) Then
                            ' �X���C�h�J�o�[�̏�Ԏ擾�iINTRIM�ł�IO�擾�ׁ݂̂̈A�G���[���Ԃ鎖�͂Ȃ��j
                            r = SLIDECOVER_GETSTS(sldCvrSts)

                            ' �X���C�h�J�o�[��Ԃ̃`�F�b�N
                            If (sldCvrSts = SLIDECOVER_MOVING) And (fClamp = False) Then
                                '----- ###240��-----
                                ' �ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h��(OPTION))
                                r = SubstrateExistCheck(System1)
                                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? 
                                    If (r = cFRS_ERR_RST) Then                          ' �����(Cancel(RESET��)�@?
                                        Timer1.Enabled = True                           ' �^�C�}�[�ċN��
                                        Exit Sub
                                    End If
                                    GoTo TimerErr                                       ' ���̑��̃G���[�Ȃ�A�v�������I��
                                End If
                                '----- ###240��-----

                                ' �X���C�h�J�o�[���ԁA�N�����vOFF�̏ꍇ�F�N�����v��ON����B
                                'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "SLIDE COVER CLOSING(CLAMP/VAC ON)")'###030
                                ' �N�����v/�z��ON
                                'Call Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)       '###060
                                r = Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)         '###060
                                If (r <> cFRS_NORMAL) Then
                                    Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
                                    Call AplicationForcedEnding()                       ' ��ċ����I������
                                    End                                                 ' �A�v�������I��
                                End If
                                Call Me.System1.AbsVaccume(gSysPrm, 1, giAppMode, giTrimErr)
                                Call Me.System1.Adsorption(gSysPrm, 1)
                                fClamp = True
                                'ElseIf (sldCvrSts = SLIDECOVER_OPEN) And (fClamp = True) Then  '###020 
                            ElseIf (sldCvrSts = SLIDECOVER_OPEN) Then                           '###020 
                                ' �X���C�h�J�o�[���I�[�v����ԂŁA�N�����vON�̏ꍇ�F�N�����v��OFF����B
                                'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "SLIDE COVER reOPEN(CLAMP/VAC OFF)")'###030
                                fClamp = False
                                ' �N�����v/�z��OFF
                                'Call Me.System1.ClampCtrl(gSysPrm, 0, giTrimErr)       '###060
                                r = Me.System1.ClampCtrl(gSysPrm, 0, giTrimErr)         '###060
                                If (r <> cFRS_NORMAL) Then
                                    Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
                                    Call AplicationForcedEnding()                       ' ��ċ����I������
                                    End                                                 ' �A�v�������I��
                                End If
                                Call Me.System1.AbsVaccume(gSysPrm, 0, giAppMode, giTrimErr)
                                Call Me.System1.Adsorption(gSysPrm, 0)
                            ElseIf (sldCvrSts = SLIDECOVER_CLOSE) Then
                                ' �X���C�h�J�o�[��
                                fClamp = False
                                Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "SLIDE COVER CLOSED")
                                fStartTrim = True           ' �X�^�[�gTRIM�t���OON
                                gbLaserPowerMonitoring = False                          ' �蓮���̓t���p���[������s��Ȃ��@V6.1.4.6�@
                            End If
                        End If
                    End If
                End If
            End If
#If 0 = cOFFLINEcDEBUG Then     'V4.0.0.0-38
            If gKeiTyp = KEY_TYPE_RS Then               'V2.0.0.0�I �V���v���g���}
                SimpleTrimmer.ElapsedTimeUpdate()       'V2.0.0.0�I �o�ߎ��Ԃ̍X�V
            End If                                      'V2.0.0.0�I
#End If
            '--------------------------------------------------------------------------
            '   �g���~���O�����s����(�X�^�[�gTRIM�t���O��ON�̏ꍇ)
            '--------------------------------------------------------------------------
            If fStartTrim = True Then                                           ' �X�^�[�gTRIM�t���OON ?
#If START_KEY_SOFT Then
                If gbStartKeySoft Then
                    Call ZCONRST()                                              ' ���b�`����
                End If
#End If
                '----- V6.0.3.0�B�� -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    TimerQR.Enabled = False                                     ' �Ď��^�C�}�[��~
                    ' �|�[�g�N���[�Y(QR�R�[�h��M�p ���[���a����)
                    Call QR_Rs232c_Close()
                End If
                '----- V6.0.3.0�B�� -----

                ' 'V6.1.4.15�@��
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = True) Then
                Else
                    'V5.0.0.9�M �� V6.0.3.0�G
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_MANUAL)
                    'V5.0.0.9�M �� V6.0.3.0�G
                End If
                ' 'V6.1.4.15�@��
                'V5.0.0.9�O              ��
                If (1 < giStartBlkAss) Then
                    If (False = chkContinue.Checked) Then
                        CbStartBlkX.SelectedIndex = 0
                        CbStartBlkY.SelectedIndex = 0
                    End If
                    chkContinue.Enabled = False

                    If (KEY_TYPE_RS <> gKeiTyp) Then
                        GrpStartBlk.Visible = False
                    End If
                End If
                'V5.0.0.9�O              ��

                ' �g���~���O�����s����
                r = Start_Trimming()

                'V5.0.0.9�O��
                If (1 < giStartBlkAss) Then
                    chkContinue.Enabled = True
                    GrpStartBlk.Visible = True
                End If
                'V5.0.0.9�O��

                ' 'V6.1.4.15�@��
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = True) Then
                Else
                    'V5.0.0.9�M �� V6.0.3.0�G
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                    'V5.0.0.9�M �� V6.0.3.0�G
                End If
                ' 'V6.1.4.15�@��

                '----- V6.0.3.0�B�� -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    ' V6.0.3.0_32 TrimData.SetLotChange()
                    Call QR_Rs232c_Open()
                    TimerQR.Enabled = True                                  ' �Ď��^�C�}�[�J�n
                End If
                '----- V6.0.3.0�B�� -----

                ' '' '' ''HALT�����v��ԕێ��ϐ���������
                '' '' ''m_keepHaltSwSts = False
                If (r = cFRS_ERR_EMG) Then
                    GoTo TimerErr
                End If

                '----- V6.1.4.0�H��(KOA EW�aSL432RD�Ή�) -----
                ' �A���g���~���O�m�f�����̃`�F�b�N(�����^�]��)
                If frmAutoObj.gbFgAutoOperation432 = True And typPlateInfo.intTrimNgCount > 0 And m_lTrimNgCount >= typPlateInfo.intTrimNgCount Then
                    ' �V�O�i���^���[����(�ԓ_��+�u�U�[�P)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_ALARM)
                    Call SetLoaderIO(COM_STS_TRM_STATE, &H0)            ' ���[�_�o��(ON=�g���}���쒆, OFF=�Ȃ�)
                    ' ���b�Z�[�W�\������START�L�[�����҂� "�m�f�i���w�肳�ꂽ�����A�����Ĕ���","",""
                    r = Sub_CallFrmMsgDisp(Me.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                            MSG_LOADER_54, "", "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
                    If (r < cFRS_NORMAL) Then
                        GoTo TimerErr
                    End If
                    ' �V�O�i���^���[���䏉����(�S�ޯĵ�)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_ALL_OFF)
                    frmAutoObj.SetAutoOpeCancel()                       ' ���[�_�o��(�A�������^�]���f)
                    Call Me.Refresh()
                    GoTo TimerExit
                End If
                '----- V6.1.4.0�H�� -----

                ' ���[�_�o��(�g���}��~��)(SL432R��) ###035
                If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                    Call SetLoaderIO(0, COM_STS_TRM_STATE)                  ' ���[�_�o��(ON=�Ȃ�, OFF=�g���}���쒆)
                End If

                '��ʕ\���̍X�V
                Call Me.Refresh()
            Else
                '--------------------------------------------------------------------------
                '   �X�^�[�gTRIM�t���O��OFF�Ȃ�A�ȉ��̏������s��
                '--------------------------------------------------------------------------
                ' �}�j���A�����[�_��RESET SW�������́u���_���A�����v���s��(�A�C�h�����[�h���Ƀ`�F�b�N����)
                If (giAppMode = APP_MODE_IDLE) Then
                    STARTRESET_SWCHECK(1, swStatus)
                    If giHostMode = cHOSTcMODEcMANUAL And swStatus = cSTS_RESETSW_ON Then
                        gbInitialized = False
                        'Call System1.sLampOnOff(LAMP_RESET, True)   ' RESET�����vON
                        Call LAMP_CTRL(LAMP_RESET, True)   ' RESET�����vON
                        ' ���_���A
                        intRet = sResetStart()
                        If (intRet <= cFRS_ERR_EMG) Then            ' ����~���Ȃ�A�v�������I��
                            Call AppEndDataSave()
                            Call AplicationForcedEnding()           ' �����I��
                        End If

                        ' V6.0.3.0�G(���[���a�łł͂����ɒǉ�����Ă���)
                        Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                        ' V6.0.3.0�G

                        ' �����J�����֐؂�ւ�
                        Call VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)

                        ' �C���^�[���b�N��Ԃ̕\��/��\��
                        Call DispInterLockSts()

                        gManualThetaCorrection = True               ' �V�[�^�␳���s�t���O = True(�V�[�^�␳�����s����)
                    End If
                End If
            End If

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
TimerExit:
            '�����I�Ƀ��������J��
            '   �f�o�b�O���̂ݎg�p�B���t���̂��ߖ����I�ɂ͎��s���Ȃ��B
#If DEBUG Then
            GC.Collect()
#End If

            ' �^�C�}�[�ċN��
            Timer1.Enabled = True                               ' �Ď��^�C�}�[�J�n
            Exit Sub

            '---------------------------------------------------------------------------
            '   �A�v�������I��
            '---------------------------------------------------------------------------
TimerErr:
            ' �G���[�R�[�h�̕\��
            '   INTRIM�̃G���[�R�[�h�����̂܂ܕԂ��Ă��Ă���ꍇ�iDllTrimFunc��IF�𒼐ڃR�[�������ꍇ�A
            '   �G���[���b�Z�[���̕\��������Ă��Ȃ����߁A���b�Z�[�W���R�R�ŕ\��
            If (r >= ERR_INTIME_BASE) Then  'INTRIM����̃G���[�R�[�h��100�Ԉȍ~�ƂȂ�
                Call System1.Form_AxisErrMsgDisp(r)
            End If
            Call AppEndDataSave()                               ' ��ċ����I�������ް��ۑ��m�F
            Call AplicationForcedEnding()                       ' ��ċ����I������
            End                                                 ' �A�v�������I��

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "Timer1.Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub

#End Region

#Region "�C���^�[���b�N�X�e�[�^�X�Ď��^�C�}�[����(�����^�]�� SL436R)"
    '''=========================================================================
    '''<summary>�C���^�[���b�N�X�e�[�^�X�Ď��^�C�}�[����(�����^�]�� SL436R) ###178</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TimerInterLockSts_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerInterLockSts.Tick

        Dim r As Integer

        TimerInterLockSts.Enabled = False                               ' �Ď��^�C�}�[��~
        r = DispInterLockSts()                                          ' �C���^�[���b�N��Ԃ̕\��/��\������ю擾
        TimerInterLockSts.Enabled = True                                ' �Ď��^�C�}�[�J�n

    End Sub
#End Region

#Region "QR�R�[�h��M�`�F�b�N�^�C�}�[����(���[���a����)"
    '''=========================================================================
    ''' <summary>QR�R�[�h��M�`�F�b�N�^�C�}�[����(���[���a����)</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>V1.18.0.0�A</remarks>
    '''=========================================================================
    Private Sub TimerQR_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerQR.Tick

        Dim r As Integer
        Dim strMSG As String = ""
        Dim QR_Data As String = ""
        Dim BackupFile As String                                        ' V6.0.3.0�C

        Try
            ' QR�f�[�^��M�`�F�b�N
            TimerQR.Enabled = False                                     ' �Ď��^�C�}�[��~
            r = QR_GetReceiveData(QR_Data)
            If (r <> cFRS_NORMAL) Then
                '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
                ' �V���A���|�[�g�I�[�v���G���[�Ȃ�^�C�}�[��~�̂܂ܔ�����(����ݴװү���ނ��\������Â���)
                If (giQrCodeType = QrCodeType.KoaEw) And (QR_Rs_Flag = 0) Then
                    Exit Sub
                End If
                '----- V6.1.4.0_22�� -----
            Else
                ' �g���}���u�A�C�h�����łȂ��Ȃ�NOP(��M�f�[�^�͎̂Ă�)
                If (giAppMode <> APP_MODE_IDLE) Then
                    QR_Read_Flg = 0
                    TimerQR.Enabled = True                              ' �Ď��^�C�}�[�J�n
                    Exit Sub
                End If

                ' ��M�f�[�^�����O��ʂɕ\��
                If (QR_Data <> "") Then
                    strMSG = "QR Code Receive=" + QR_Data
                    '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
                    Debug.WriteLine(strMSG)
                    If (giQrCodeType = QrCodeType.KoaEw) Then
                        Call Me.System1.OperationLogging(gSysPrm, strMSG, "QRCODE")
                    Else
                        Call Z_PRINT(strMSG)
                    End If
                    '----- V6.1.4.0_22�� -----
                End If

                '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
                If (giQrCodeType = QrCodeType.KoaEw) AndAlso (gTkyKnd = KND_CHIP) Then               'V6.1.4.10�A�@(gTkyKnd = KND_CHIP)�@����ǉ�
                    ' �p�q�R�[�h���[�_�����M�����f�[�^����������
                    Timer1.Stop()                                      'V6.1.4.14�C
                    Call ObjQRCodeReader.QRCodeDataExecute(QR_Data)
                    TimerQR.Enabled = True                              ' �Ď��^�C�}�[�J�n
                    Call ZCONRST()                                      'V6.1.4.14�C�X�^�[�g�X�C�b�`���b�`�N���A
                    Timer1.Start()                                      'V6.1.4.14�C
                    Exit Sub
                End If
                '----- V6.1.4.0_22�� -----

                'V6.1.4.10�A
                If (giQrCodeType = QrCodeType.KoaEw) AndAlso (gTkyKnd = KND_NET) Then
                    ' �p�q�R�[�h���[�_�����M�����f�[�^����������
                    Timer1.Stop()                                      'V6.1.4.14�C
                    Call ObjQRCodeReader.QRCodeDataExecuteForTKYNET(QR_Data)
                    TimerQR.Enabled = True                              ' �Ď��^�C�}�[�J�n
                    Call ZCONRST()                                      'V6.1.4.14�C�X�^�[�g�X�C�b�`���b�`�N���A
                    Timer1.Start()                                      'V6.1.4.14�C
                    Exit Sub
                End If
                'V6.1.4.10�A

                ' QR�R�[�h��M�f�[�^����w��̕������ҏW���t�@�C���p�X���쐬����
                strQRLoadFileFullPath = GetQrCodeFileName(QR_Data)
                If (strQRLoadFileFullPath = "") Then                    ' �t�@�C�������݂��Ȃ� ? 
                    QR_Read_Flg = 0
                Else
                    ' V6.0.3.0�C �g���~���O�f�[�^�o�b�N�A�b�v�ۑ� 
                    BackupFile = MakeBackupFile(strQRLoadFileFullPath)
                    ' �w��̃t�@�C�������[�h����(�ǂݍ��ނ̂̓}�X�^�[�̃t�@�C���Ƃ���) V6.0.3.0_34
                    Call DataLoadQR(strQRLoadFileFullPath)
                    '----- V6.0.3.0�C�� -----
                    typPlateInfo.strDataName = strQRLoadFileFullPath    ' �v���[�g�f�[�^�̃g���~���O�f�[�^�t�@�C�����X�V
                    ' �f�[�^�t�@�C���̏����ݎ��s��
                    If (r <> cFRS_NORMAL) Then
                        strMSG = "Fle Save Error =" + QR_Data
                        Call Z_PRINT(strMSG)
                    End If
                    '----- V6.0.3.0�C�� -----
                    '----- V6.0.3.0�H�� -----
                    If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                        ClearDispData()                                 ' V6.0.3.0_32
                        TrimData.SetLotChange()                         ' ���b�g���N���A
                    End If
                    '----- V6.0.3.0�H�� -----
                    QR_Read_Flg = 1                                     ' QR���ޓǂݍ��ݔ���(0)NG (1)OK
                End If
            End If

            TimerQR.Enabled = True                                      ' �Ď��^�C�}�[�J�n
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.TimerQR_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�o�[�R�[�h��M�`�F�b�N�^�C�}�[����"
    '''=========================================================================
    ''' <summary>�o�[�R�[�h��M�`�F�b�N�^�C�}�[����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>V1.23.0.0�@</remarks>
    '''=========================================================================
    Private Sub TimerBC_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerBC.Tick

        Dim r As Integer
        Dim strMSG As String = ""

        Try
            ' �o�[�R�[�h�f�[�^��M�`�F�b�N
            TimerBC.Enabled = False                                     ' �Ď��^�C�}�[��~
            r = BC_GetReceiveData(BC_Data)
            If (r <> cFRS_NORMAL) Then

            Else
                ' �g���}���u�A�C�h�����łȂ��Ȃ�NOP(��M�f�[�^�͎̂Ă�)
                If (giAppMode <> APP_MODE_IDLE) Then
                    BC_Read_Flg = 0
                    TimerBC.Enabled = True                              ' �Ď��^�C�}�[�J�n
                    Exit Sub
                End If

                ' ��M�f�[�^�����O��ʂɕ\��
                If (BC_Data <> "") Then                                 ' �o�[�R�[�h�f�[�^��M ? 
                    'V5.0.0.9�R                    strMSG = "Bar Code Receive=" + BC_Data
                    strMSG = String.Format("Barcode{0} Receive={1}",
                        If((BarcodeType.Taiyo = BarCode_Data.Type), (BarCode_Data.BC_ReadCount + 1), ""),
                        BarCode_Data.BC_Data)                           'V5.0.0.9�R

                    Call Z_PRINT(strMSG)
                Else
                    GoTo STP_END
                End If

                'V5.0.0.9�R                ' �ҏW���̃f�[�^�����邩�`�F�b�N����
                'V5.0.0.9�R                If (gCmpTrimDataFlg = 1) Then                           ' �f�[�^�X�V�t���O = 1(�X�V����))
                'V5.0.0.9�R                    r = MsgBox(MSG_115, MsgBoxStyle.OkCancel, MSG_116)  ' "�ҏW���̃f�[�^������܂��B���[�h���܂����H"
                'V5.0.0.9�R                    If (r = MsgBoxResult.Cancel) Then                   ' Cansel�w�� ?
                'V5.0.0.9�R                        GoTo STP_END
                'V5.0.0.9�R                    End If
                'V5.0.0.9�R                    gCmpTrimDataFlg = 0                                 ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�))
                'V5.0.0.9�R                End If

                ' �o�[�R�[�h��M�f�[�^����w��̕������ҏW���t�@�C���p�X���쐬����
                strBCLoadFileFullPath = GetBarCodeFileName(BC_Data)
                'V5.0.0.9�R              ��
                If (strBCLoadFileFullPath Is Nothing) Then
                    ' ���z�ЂP�x�ڂ̎�M��
                    TimerBC.Enabled = True                              ' �Ď��^�C�}�[�J�n
                    Exit Sub
                End If
                'V5.0.0.9�R              ��
                If (strBCLoadFileFullPath = "") Then                    ' �t�@�C�������݂��Ȃ� ? 
                    BC_Read_Flg = 0
                    ' �g���~���O�f�[�^�𖢃��[�h�Ƃ���(�����^�]��O�̃f�[�^�Ŏ��s�����Ȃ�����)
                    gStrTrimFileName = ""
                    LblDataFileName.Text = ""
                    gLoadDTFlag = False                                 ' gLoadDTFlag = �f�[�^�����[�h
                    SetMapOnOffButtonEnabled(False)                     'V4.12.2.0�@
                    SetTrimMapVisible(False)                            'V4.12.2.0�@
                    Call BC_Info_Disp(0)                                ' �o�[�R�[�h���\�����N���A����
                    BarCode_Data.BC_Data = ""                           'V5.0.0.9�N
                Else
                    'V5.0.0.9�R          ��
                    ' �ҏW���̃f�[�^�����邩�`�F�b�N����
                    If (gCmpTrimDataFlg = 1) Then                           ' �f�[�^�X�V�t���O = 1(�X�V����))
                        r = MsgBox(MSG_115, MsgBoxStyle.OkCancel, MSG_116)  ' "�ҏW���̃f�[�^������܂��B���[�h���܂����H"
                        If (r = MsgBoxResult.Cancel) Then                   ' Cansel�w�� ?
                            GoTo STP_END
                        End If
                        gCmpTrimDataFlg = 0                                 ' �f�[�^�X�V�t���O = 0(�X�V�Ȃ�))
                    End If
                    'V5.0.0.9�R          ��

                    ' �w��̃t�@�C�������[�h����
                    r = DataLoadBC(strBCLoadFileFullPath)
                    If (r = cFRS_NORMAL) Then
                        BC_Read_Flg = 1
                    Else
                        BC_Read_Flg = 0
                    End If
                End If
            End If
STP_END:
            TimerBC.Enabled = True                                      ' �Ď��^�C�}�[�J�n
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.TimerBC_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

    ''' <summary>�o�[�R�[�h�֘A�R���g���[���\���E�ڑ��������Ȃ�</summary>
    ''' <remarks>'V5.0.0.9�N</remarks>
    Private Sub SetBarcodeMode()
        Dim ret As Integer
        Dim strMsg As String

        With Me
            Select Case (BarCode_Data.Type)

                '----- V1.23.0.0�@�� -----
                ' ���z�Гa�d�l
                Case BarcodeType.Taiyo
                    ' �o�[�R�[�h���(������)��\��
                    Call BC_Info_Disp(0)                            ' �o�[�R�[�h���̕\��������
                    .GrpQrCode.Visible = True                       ' �o�[�R�[�h���(QR���ޏ�����g�p)�\���O���[�v�{�b�N�X�\��
                    .btnRest.Enabled = True                         'V5.0.0.9�R
                    .btnRest.Visible = True                         'V5.0.0.9�R

                    ' �|�[�g�I�[�v��(�o�[�R�[�h�f�[�^��M�p)
                    ret = BC_Rs232c_Open()
                    If (ret <> SerialErrorCode.rRS_OK) Then
                        ' "�V���A���|�[�g�n�o�d�m�G���["
                        strMsg = MSG_136 & "(" + gsComPort + ")"
                        Call MsgBox(strMsg, vbOKOnly)
                        'Return (ret)
                    End If
                    '----- V1.23.0.0�@�� -----

                    '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
                Case BarcodeType.Walsin
                    ' �o�[�R�[�h���(������)��\��
                    Call BC_Info_Disp(0)                            ' �o�[�R�[�h���̕\��������
                    .GrpQrCode.Visible = True                       ' �o�[�R�[�h���(QR���ޏ�����g�p)�\���O���[�v�{�b�N�X�\��

                    ' �|�[�g�I�[�v��(�o�[�R�[�h�f�[�^��M�p)
                    ret = BC_Rs232c_Open()
                    If (ret <> SerialErrorCode.rRS_OK) Then
                        ' "�V���A���|�[�g�n�o�d�m�G���["
                        strMsg = MSG_136 & "(" + gsComPort + ")"
                        Call MsgBox(strMsg, vbOKOnly)
                    End If

                    ' SizeCode.ini���\���̂�ݒ肷��
                    ret = IniSizeCodeData(StSizeCode)
                    '----- V4.11.0.0�A�� -----

                Case BarcodeType.Standard
                    ' �o�[�R�[�h���(������)��\��
                    Call BC_Info_Disp(0)                            ' �o�[�R�[�h���̕\��������
                    .GrpQrCode.Visible = True                       ' �o�[�R�[�h���(QR���ޏ�����g�p)�\���O���[�v�{�b�N�X�\�� 'V5.0.0.9�S
                    .btnRest.Enabled = True                         'V5.0.0.9�S
                    .btnRest.Visible = True                         'V5.0.0.9�S

                    ' �|�[�g�I�[�v��(�o�[�R�[�h�f�[�^��M�p)
                    ret = BC_Rs232c_Open()
                    If (ret <> SerialErrorCode.rRS_OK) Then
                        ' "�V���A���|�[�g�n�o�d�m�G���["
                        strMsg = MSG_136 & "(" + gsComPort + ")"
                        Call MsgBox(strMsg, vbOKOnly)
                    End If

                Case Else
                    ' DO NOTHING
            End Select
        End With

    End Sub
#End Region

    '========================================================================================
    '   �}�E�X�A�b�v�_�E�����̃C�x���g����
    '========================================================================================
#Region "�L�[�_�E��������"
    '''=========================================================================
    '''<summary>�L�[�_�E��������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form1_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Short = eventArgs.KeyCode
        'V6.0.0.0�I        Dim Shift As Short = eventArgs.KeyData \ &H10000
        Dim pbFuncShift As Boolean = False                  ' SHIFT KEY �������
        Dim pbFuncCtrl As Boolean = False
        Dim pbFuncAlt As Boolean = False

        If (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
            pbFuncShift = True  ' SHIFT KEY
        End If
        If (Control.ModifierKeys And Keys.Control) = Keys.Control Then
            pbFuncCtrl = True   ' CTRL KEY
        End If
        If (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pbFuncAlt = True    ' ALT KEY
        End If

        If (KeyCode = System.Windows.Forms.Keys.M) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            Me.grpDbg.Visible = True
        End If
        If (KeyCode = System.Windows.Forms.Keys.N) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            Me.grpDbg.Visible = False
        End If
        If (KeyCode = System.Windows.Forms.Keys.S) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            '            Call ZSELXYZSPD(0)          ' XYZ Slow speed ###108
        End If
        If (KeyCode = System.Windows.Forms.Keys.F) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            '           Call ZSELXYZSPD(1)          ' XYZ Normal speed ###108
        End If

        If (_jogKeyDown IsNot Nothing) Then         'V6.0.0.0�I
            _jogKeyDown.Invoke(eventArgs)
        End If
    End Sub
#End Region

#Region "�L�[�A�b�v������"
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0�I</remarks>
    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
        If (_jogKeyUp IsNot Nothing) Then
            _jogKeyUp.Invoke(e)
        End If
    End Sub
#End Region

#Region "�}�E�X�ړ�������(picGraphView)"
    '''=========================================================================
    '''<summary>�}�E�X�ړ�������(picGraphView)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub picGraphView_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs)

        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'Call ResistorGraphMouseMove(picGraphView, Button, Shift, X, y)

    End Sub
#End Region

#Region "�}�E�X�A�b�v������(picGraphView)"
    '''=========================================================================
    '''<summary>�}�E�X�A�b�v������(picGraphView)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub picGraphView_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs)
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'Call ResistorGraphClick(picGraphView, Button, Shift, X, y)

    End Sub
#End Region

    '========================================================================================
    '   �e�R�}���h(���[�U�R���g���[���Ƀt�H�[��������OCX�̏ꍇ)���t�H�[�J�X���������ꍇ�̏���
    '========================================================================================
#Region "�r�f�I�摜���N���b�N���ăt�H�[�J�X���������ꍇ�̏���"
#If False Then                          'V6.0.0.0�I
    '''=========================================================================
    ''' <summary>�r�f�I�摜���N���b�N���ăt�H�[�J�X���������ꍇ�̏��� ###051</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>�EOcxTeach��DispGazou.EXE���s���̂���Enter�C�x���g�͓����Ă��Ȃ�
    '''          �@DispGazou.EXE��OcxTeach�ŋN������###052
    '''          �EEnter�C�x���g��Form��ACTIVE�R���g���[���ɂȂ������ɔ���
    '''            �ꎞ��~��ʎ��͉��̂���x���������Ă��Ȃ�</remarks>
    '''=========================================================================
    Private Sub VideoLibrary1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String

        Try
            Select Case (giAppMode)
                Case APP_MODE_PROBE
                    ' �v���[�u�R�}���h���s�� ?
                    Probe1.Focus()                                      ' OcxProbe�Ƀt�H�[�J�X���Z�b�g���� 

                Case APP_MODE_TEACH
                    ' �e�B�[�`�R�}���h���s�� ?
                    Teaching1.Focus()                                   ' OcxTeach�Ƀt�H�[�J�X���Z�b�g���� 
                    Teaching1.JogSetFocus()                             ' ��Probe�ƈႢ���̂����ꂪ�Ȃ��ƃe���L�[�������Ȃ�

                    '----- ###122�� -----
                Case APP_MODE_CUTPOS, APP_MODE_RECOG, APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH, _
                     APP_MODE_CARIB_REC, APP_MODE_CARIB, APP_MODE_CUTREVISE_REC, APP_MODE_CUTREVIDE
                    VideoLibrary1.JogSetFocus()
                    '----- ###122�� -----

            End Select

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.VideoLibrary1_Enter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End If
#End Region

#Region "���C��Form�̃R���g���[�����N���b�N���ăt�H�[�J�X���������ꍇ�̏���(�ꎞ��~���)"
#If False Then                          'V6.0.0.0�I
    '''=========================================================================
    ''' <summary>���C��Form�̃R���g���[�����N���b�N���ăt�H�[�J�X���������ꍇ�̏���(�ꎞ��~���) ###053</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub Form1_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        ' VideoLibrary1_Enter�C�x���g�͈ꎞ��~��ʎ��͉��̂���x���������Ă��Ȃ��̂�
        ' Activated�C�x���g�ŏ���
        Call ADJSetFocu()                               ' �ꎞ��~��ʂɃt�H�[�J�X��ݒ肷��
    End Sub

    Private Sub Form1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click
        Call ADJSetFocu()                               ' �ꎞ��~��ʂɃt�H�[�J�X��ݒ肷��
    End Sub
    ' ���O�\���� 
    Private Sub txtLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLog.Click
        Call ADJSetFocu()
    End Sub

    Private Sub lblInterLockMSG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblInterLockMSG.Click
        Call ADJSetFocu()
    End Sub
    Private Sub LblDataFileName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblDataFileName.Click
        Call ADJSetFocu()
    End Sub

    Private Sub lblLoginResult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLoginResult.Click
        Call ADJSetFocu()
    End Sub

    Private Sub LblCur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblCur.Click
        Call ADJSetFocu()
    End Sub

    Private Sub LblMes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblMes.Click
        Call ADJSetFocu()
    End Sub

    Private Sub LblRotAtt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblRotAtt.Click

    End Sub

    Private Sub lblLogging_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLogging.Click
        Call ADJSetFocu()
    End Sub
#End If
#End Region

#Region "�ꎞ��~��ʂɃt�H�[�J�X��ݒ肷��"
#If False Then                          'V6.0.0.0�I
    '''=========================================================================
    ''' <summary> �ꎞ��~��ʂɃt�H�[�J�X��ݒ肷�� ###053</summary>
    '''=========================================================================
    Private Sub ADJSetFocu()

        Dim strMSG As String

        Try
            ' �ꎞ��~��ʕ\�����̏ꍇ
            If (gObjADJ Is Nothing = False) Then
                If (gbTenKeyFlg = True) Then                                '###057
                    gObjADJ.Activate()                                      ' �ꎞ��~��ʂɃt�H�[�J�X��ݒ肷��
                End If
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.ADJSetFocu() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
        End Try
    End Sub
#End If
#End Region

    '========================================================================================
    '   �ȉ��f�o�b�O�p��
    '========================================================================================
    Private Sub btnDbgForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDbgForm.Click
        Dim blkNo As Integer
        Dim pltNo As Integer
        Dim pltNoX, pltNoY, blkNoX, blkNoY As Integer                       '#4.12.2.0�E
        pltNo = 1
        blkNo = 1
        pltNoX = 1 : pltNoY = 1 : blkNoX = 1 : blkNoY = 1                   '#4.12.2.0�E
        '#4.12.2.0�E        Call HaltSwOnMove(pltNo, blkNo, False)
        HaltSwOnMove(pltNo, blkNo, pltNoX, pltNoY, blkNoX, blkNoY, False)   '#4.12.2.0�E
    End Sub

    Private Sub btnGoClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGoClipboard.Click
        Try
            Call Clipboard.Clear()
            Call Clipboard.SetText(txtLog.Text)

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try


    End Sub

    Public Sub changefrmDistStatus(ByVal DispOnOff As Integer)
        Try

            If (DispOnOff = 1) Then
                '���v�\����ON
                gObjFrmDistribute.Show()
                gObjFrmDistribute.RedrawGraph()  '###218 
                '�{�^���\���̕ύX
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    chkDistributeOnOff.Text = "���Y�O���t�@��\��"
                'Else
                '    chkDistributeOnOff.Text = "Distribute OFF"
                'End If
                chkDistributeOnOff.Text = Form1_019
            Else
                '���v�\����OFF
                gObjFrmDistribute.hide()

                '�{�^���\���̕ύX
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    chkDistributeOnOff.Text = "���Y�O���t�@�\��"
                'Else
                '    chkDistributeOnOff.Text = "Distribute ON"
                'End If
                chkDistributeOnOff.Text = Form1_020
            End If

            Exit Sub

        Catch ex As Exception
            MsgBox("changefrmDistStatus() Execption error." & vbCrLf & " error msg = " & ex.Message)
        End Try
    End Sub

#Region "�O���t�\��/��\���{�^������������"
    '''=========================================================================
    ''' <summary>�O���t�\��/��\���{�^������������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub chkDistributeOnOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDistributeOnOff.CheckedChanged
        Try
            If chkDistributeOnOff.Checked = True Then
                ''���v�\����ON
                '----- ###218�� -----
                ' ���v��ʃ{�^����L������
                gObjFrmDistribute.cmdGraphSave.Enabled = True
                gObjFrmDistribute.cmdInitial.Enabled = True
                gObjFrmDistribute.cmdFinal.Enabled = True
                '----- ###218�� -----
                changefrmDistStatus(1)
            Else
                ''���v�\����OFF
                changefrmDistStatus(0)
            End If
        Catch ex As Exception
            MsgBox("chkDistributeOnOff_CheckedChanged() Execption error." & vbCrLf & " error msg = " & ex.Message)

        End Try
    End Sub
#End Region

    Private Sub cmdTestRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTestRun.Click
        Try
            '�A���ғ�
            m_bStopRunning = False

            '�\����Ԃ̕ύX
            Me.cmdTestRun.BackColor = Color.Yellow
            Me.cmdStop.BackColor = Color.Silver
            Me.txtCount.Text = "0"

            Do
                If (m_bStopRunning = True) Then
                    '�\����Ԃ̕ύX
                    Me.cmdTestRun.BackColor = Color.Silver
                    Me.cmdStop.BackColor = Color.Yellow
                    Exit Do
                Else
                    Me.txtCount.Text = (Long.Parse(Me.txtCount.Text) + 1).ToString
                    Me.btnTrimming.PerformClick()
                End If
            Loop
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.HaltSwOnMove() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub

    Private Sub cmdStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        '�A���ғ���~
        m_bStopRunning = True
    End Sub

#Region "���v��ʕ\����ԕύX����"
    Private Sub RedrawDisplayDistribution(ByVal bEntry As Boolean)

        If gKeiTyp = KEY_TYPE_RS Then               'V2.0.0.0�I �V���v���g���}
            Return
        End If

        ' TKYCHIPorNET�̏ꍇ�͓��v�\��������ʂ̏�Ԏ擾
        If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
            If (bEntry = True) Then
                m_bDispDistributeSts = chkDistributeOnOff.Checked
                If (m_bDispDistributeSts = True) Then
                    '���v�\����OFF
                    changefrmDistStatus(0)
                    'frmDistribution.Close()

                    ''�{�^���\���̕ύX
                    'chkDistributeOnOff.Text = "Distribute ON"

                    ' �R���g���[���̏�ԕύX
                    ''###112                    Me.chkDistributeOnOff.Checked = False
                End If
            Else
                If (m_bDispDistributeSts = True) Then
                    '���v�\����ON
                    changefrmDistStatus(1)
                    'frmDistribution.Show()

                    ''�{�^���\���̕ύX
                    'chkDistributeOnOff.Text = "Distribute OFF"

                    ' �R���g���[���̏�ԕύX
                    Me.chkDistributeOnOff.Checked = True
                End If
            End If
        End If

    End Sub
#End Region

    Private Sub btnEndTrace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'CmdEnd.PerformClick()
    End Sub

    '###182
    Private Sub Mg_Up_MouseDownButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseDown
        Dim CmpData As Integer

        CmpData = 0

        If (CheckBox1.Checked = True) Then
            CmpData = CmpData Or &H1
        End If

        If (CheckBox2.Checked = True) Then
            CmpData = CmpData Or &H2
        End If

        If (CheckBox3.Checked = True) Then
            CmpData = CmpData Or &H4
        End If

        If (CheckBox4.Checked = True) Then
            CmpData = CmpData Or &H8
        End If

        Call MGMoveJog(CmpData, MG_UP)

    End Sub
    '###182
    Private Sub Mg_Up_MouseUpButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseUp
        Call MGStopJog()
    End Sub


    '###182
    Private Sub Mg_Down_MouseUpButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseUp
        Call MGStopJog()
    End Sub
    '###182
    Private Sub Mg_Down_MouseDownButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseDown
        Dim CmpData As Integer

        CmpData = 0

        If (CheckBox1.Checked = True) Then
            CmpData = CmpData Or &H1
        End If

        If (CheckBox2.Checked = True) Then
            CmpData = CmpData Or &H2
        End If

        If (CheckBox3.Checked = True) Then
            CmpData = CmpData Or &H4
        End If

        If (CheckBox4.Checked = True) Then
            CmpData = CmpData Or &H8
        End If

        Call MGMoveJog(CmpData, MG_DOWN)

    End Sub

#Region "�t�H�[�����[�h������"
    '''=========================================================================
    '''<summary>�t�H�[�����[�h������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strMSG As String                                            ' ү���ޕҏW��
        Dim dispSize As System.Drawing.Size
        Dim dispPos As System.Drawing.Point
        Dim r As Integer
        Dim lSts As Long = 0                                            ' ###207

        Try
            '---------------------------------------------------------------------------
            '   ���u����������
            '---------------------------------------------------------------------------
            r = Loader_Init()                                           ' �I�[�g���[�_��������(SL436�p)
            If (r < cFRS_NORMAL) Then
                ' �����I��
                Call AppEndDataSave()
                Call AplicationForcedEnding()
                Exit Sub
            End If

            Call Me.Initialize_VideoLib()
            'V6.0.0.0-28            Call Me.VideoLibrary1.VideoStop()                           ' ���_���A�����ŕ\�����t���[�Y�\�������邽�߈�U��~
            Call Me.Initialize_TrimMachine()

            ' '###241 
            'V6.0.0.0�I            AddHandler VideoLibrary1.Enter, AddressOf VideoLibrary1_Enter

            ' ��ʕ\���ʒu�̐ݒ�
            dispPos.X = 0
            dispPos.Y = 0
            Me.Location = dispPos

            ' ��ʃT�C�Y�̐ݒ�
            dispSize.Height = 1024
            dispSize.Width = 1280
            Me.Size = dispSize

            Call SetButtonImage()                                       ' �t�H�[���̃{�^�����̐ݒ�(���{��/�p��)

            ' Ocx(VB6)���ޯ��Ӱ�ސݒ�
#If cOFFLINEcDEBUG Then
            VideoLibrary1.cOFFLINEcDEBUG = &H3141S
            Teaching1.cOFFLINEcDEBUG = &H3141S
            Probe1.cOFFLINEcDEBUG = &H3141S
            Ctl_LaserTeach2.cOFFLINEcDEBUG = &H3141S
#End If
            ' Video.ocx��DbgOn/Off���݂̗L��/�����w��(�f�o�b�O�p)
            '�@�f�o�b�O���ϐ����e��\�������邽��
#If cDBGRdraw Then                                                      ' Video.ocx��DbgOn/Off���ݗL���Ƃ��� ?
            VideoLibrary1.cDBGRdraw = &H3142
#End If
            ' �R���g���[�����\���ɂ���
            Probe1.Visible = False
            Teaching1.Visible = False
            HelpVersion1.Visible = False

            ' V1.14.0.0�@
            '���[�U�J�X�^�}�C�Y��ʕ\��
            r = Set_UserSpecialCtrl(gSysPrm.stCTM.giSPECIAL)

            ' �o�[�R�[�h�֘A�R���g���[���\���E�ڑ��������Ȃ�
            SetBarcodeMode()            'V5.0.0.9�N

            ' �v���[�u�ʒu���킹�̃R���g���[���̕\���ʒu���w�肷��
            Probe1.Left = Text4.Location.X
            Probe1.Top = Text4.Location.Y

            ' �e�B�[�`���O�̃R���g���[���̕\���ʒu���w�肷��
            Teaching1.Left = Text4.Location.X
            Teaching1.Top = Text4.Location.Y

            ' TKYCHIPorNET�̏ꍇ�A���v�\���I�u�W�F�N�g�𐶐�����
            If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                gObjFrmDistribute = New frmDistribution
            End If

            ' ���[�_�փg���}��~���M���𑗐M����
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R�n ? ###035
                Call SetLoaderIO(LOUT_STOP, &H0)                        ' ���[�_�o��(ON=�g���}��~��, OFF=�Ȃ�)
            Else
                Call SetLoaderIO(0, COM_STS_TRM_STATE)                  ' ���[�_�o��(ON=�Ȃ�,OFF=�g���}���쒆)
            End If

            ' �}�K�W���̏㉺����
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R�n ? ###035
                GroupBox1.Visible = True
                '----- ###207�� -----
                r = H_Read(LOFS_H00, lSts)                              ' �����ݒ��Ԏ擾(H0.00-H0.15)
                If (lSts And LHST_MAGAZINE) Then                        ' 4�}�K�W�� ?
                    CheckBox3.Visible = True                            ' MG3�\�� 
                    CheckBox4.Visible = True                            ' MG4�\�� 
                Else
                    CheckBox3.Visible = False                           ' MG3��\�� 
                    CheckBox4.Visible = False                           ' MG4��\�� 
                End If
                '----- ###207�� -----
            Else
                GroupBox1.Visible = False
            End If

            ' ���ݸތ��ʈ�����ڂ��ް���ر����(���[���a����) 'V1.18.0.0�B
            Call ClrTrimPrnData()
#If False Then                          'V6.0.0.0�D
            ' �摜�\���v���O�������N������
            If gKeiTyp = KEY_TYPE_RS Then
                Execute_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    'V3.0.0.0�D
            Else
                Execute_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    'V3.0.0.0�D
            End If
#End If
            ' Initialize_VideoLib()����VideoLibrary1.Init_Library()�Ŏ��s�ς�  'V6.0.0.0-28
            'V6.0.0.0-28            ' �����J�����ɐ؂�ւ��� 
            'V6.0.0.0-28            Call Me.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
            'V6.0.0.0-28            Call Me.VideoLibrary1.VideoStart()

            '-------------------------------------------------------------------
            '   �V���v���g���}��ʏ���������(SL436S��)
            '-------------------------------------------------------------------
            Call SimpleTrimmer.SimpleTrimmerInit()              'V2.0.0.0�I ADD

            '----- V4.0.0.0-87�� -----
            If (giMachineKd = MACHINE_KD_RS) Then
                SimpleTrimmer.DspLaserPower()
            End If
            '----- V4.0.0.0-87�� -----

            'V4.8.0.1�@��
            If giRateDisp = 1 Then
                CheckNGRate.Checked = True
            End If
            'V4.8.0.1�@��
            'V4.9.0.0�@
            If giNgStop = 1 Then
                btnJudge.Visible = True
            Else
                btnJudge.Visible = False
            End If
            'V4.9.0.0�@
            'V4.11.0.0�H
            btnNEXT.Enabled = False
            btnPREV.Enabled = False
            'V4.11.0.0�H

            'V5.0.0.9�M V6.0.3.0�G
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
            'V5.0.0.9�M V6.0.3.0�G

            ' �\�����X�V
            Me.Refresh()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "i-TKY.Form1_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try


    End Sub

#End Region

    '''' <summary>
    '''' �V���v���g���}�p��Video�T�C�Y�ɕύX���� 
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub SetSimpleVideoSize()
    '    If gKeiTyp <> KEY_TYPE_RS Then
    '        Return
    '    End If
    '    VideoLibrary1.SetVideoSizeAndCross(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)

    'End Sub

    '''' <summary>
    '''' �e�B�[�`���O�p�ɏ]����Video�T�C�Y�ɕύX���� 
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub SetTeachVideoSize()
    '    If gKeiTyp <> KEY_TYPE_RS Then
    '        Return
    '    End If

    '    VideoLibrary1.SetVideoSizeAndCross(NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)

    'End Sub

    ''' <summary>
    ''' �摜�T�C�Y��ς��郂�[�h�����肷��
    ''' </summary>
    ''' <param name="iAppMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChkVideoChangeMode(ByVal iAppMode As Integer) As Integer
        ChkVideoChangeMode = 0
        Select Case (iAppMode)
            Case APP_MODE_LASER, APP_MODE_PROBE, APP_MODE_TEACH, APP_MODE_RECOG, APP_MODE_CUTPOS, APP_MODE_TTHETA, APP_MODE_TX, APP_MODE_TY,
                APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH, APP_MODE_CARIB_REC, APP_MODE_CARIB, APP_MODE_CUTREVISE_REC, APP_MODE_CUTREVIDE,
                APP_MODE_CIRCUIT, APP_MODE_APROBEREC, APP_MODE_APROBEEXE, APP_MODE_IDTEACH, APP_MODE_PROBE_CLEANING, APP_MODE_INTEGRATED 'V4.10.0.0�K
                ChkVideoChangeMode = 1
            Case Else
                ChkVideoChangeMode = 0
        End Select

    End Function

    ''' <summary>
    ''' �ꎞ��~���̊Ď��p�^�C�}�[
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TimerAdjust_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerAdjust.Tick

        Dim Ret As Integer
        Dim sw As Long
        Dim swStatus As Integer
        Dim r As Integer 'V4.0.0.0-86

        mExit_flg = cFRS_NORMAL
        TimerAdjust.Enabled = False

        ' 'V4.0.0.0-43
        If gKeiTyp = KEY_TYPE_RS Then               'V2.0.0.0�I �V���v���g���}
            SimpleTrimmer.ElapsedTimeUpdate()       'V2.0.0.0�I �o�ߎ��Ԃ̍X�V(�ꎞ��~��)
        Else
            Return
        End If                                      'V2.0.0.0�I
        ' 'V4.0.0.0-43

        ' START/RESET�L�[���̓`�F�b�N
        Ret = STARTRESET_SWCHECK(False, sw)                           ' START/RESET�L�[�����`�F�b�N(�Ď��Ȃ����[�h)
        If (sw = cFRS_ERR_START) Then
            ''V4.0.0.0-86
            r = GetLaserOffIO(True) 'V5.0.0.1�K
            If r = 1 Then
                TimerAdjust.Enabled = True      'V4.11.0.0�P
                Call ZCONRST()     'V4.11.0.0�P
                Exit Sub
            End If
            ''V4.0.0.0-86
            gbExitFlg = True
            TimerAdjust.Enabled = False
            Call START_SWCHECK(1, swStatus)
            Return
        ElseIf (sw = cFRS_ERR_RST) Then
            mExit_flg = cFRS_ERR_RST                                        ' Return�l = Cancel(RESET��)  
            gbExitFlg = True                                                ' �I���t���OON
            TimerAdjust.Enabled = False
            Call STARTRESET_SWCHECK(1, swStatus)
            Return
        End If


        TimerAdjust.Enabled = True

    End Sub

    ''' <summary>
    ''' �{�^�������������ʂ��i�[����
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetResultAdjust() As Integer

        GetResultAdjust = mExit_flg

    End Function

    ' >>> V3.1.0.0�@ 2014/11/28
#Region "�p�l���\��ON"
    '''=========================================================================
    ''' <summary>
    ''' �p�l���\��ON
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetDataDisplayOn()
        If (giMachineKd <> MACHINE_KD_RS) Then Return ' SL436S�łȂ����NOP V4.0.0.0-36
        Me.pnlDataDisplay.Visible = True
    End Sub
#End Region

#Region "�p�l���\��OFF"
    '''=========================================================================
    ''' <summary>
    ''' �p�l���\��OFF
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetDataDisplayOff()
        Me.pnlDataDisplay.Visible = False
    End Sub
#End Region

#Region "�f�[�^�\�������������f����"
    '''=========================================================================
    ''' <summary>
    ''' �f�[�^�\�������������f����
    ''' </summary>
    ''' <param name="iAppMode">�A�v���P�[�V�������[�h</param>
    ''' <returns>True:�Y���AFalse:��Y��</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function IsDataDisplayCheck(ByVal iAppMode As Integer) As Boolean
        ' �v���[�u�A�e�B�[�`���O�A�摜�o�^�ABP�ʒu�����A�X�e�[�W�ʒu����
        'Dim arMode As Integer() = New Integer() {APP_MODE_PROBE, APP_MODE_TEACH, APP_MODE_RECOG, APP_MODE_TX, APP_MODE_TY, APP_MODE_LASER, APP_MODE_PROBE_CLEANING}
        Dim arMode As Integer() = New Integer() {APP_MODE_PROBE, APP_MODE_TEACH, APP_MODE_RECOG, APP_MODE_TX,
                                                 APP_MODE_TY, APP_MODE_LASER, APP_MODE_PROBE_CLEANING, APP_MODE_INTEGRATED} 'V4.10.0.0�K
        Dim i As Integer

        IsDataDisplayCheck = False

        For i = 0 To UBound(arMode) Step 1
            If arMode(i) = iAppMode Then
                IsDataDisplayCheck = True
                Exit For
            End If
        Next i

    End Function
#End Region
    ' <<< V3.1.0.0�@ 2014/11/28

    Private Sub cmdGraphSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGraphSave.Click

        SavePanelImage()

    End Sub
    'V4.0.0.0�K
    '''=========================================================================
    ''' <summary>
    ''' �C�j�V�����\��������������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdInitial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInitial.Click
        gObjFrmDistribute.SetDispGrp(True)
        Call gObjFrmDistribute.RedrawGraph()

    End Sub
    'V4.0.0.0�K
    '''=========================================================================
    ''' <summary>
    ''' �t�@�C�i���\��������������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdFinal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFinal.Click
        gObjFrmDistribute.SetDispGrp(False)
        Call gObjFrmDistribute.RedrawGraph()                                              ' ���z�}�\������

    End Sub

#Region "���v�\���p�l����ON/OFF"
    ' V4.0.0.0�K
    '''=========================================================================
    ''' <summary>
    ''' ���v�\���p�l����ON/OFF
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub PanelGraphOnOff(ByVal flg As Boolean)
        PanelGraph.Visible = flg
    End Sub

#End Region
    'V4.0.0.0�K
#Region "���C����ʏ�̕��z�}�ۑ�����"
    '''=========================================================================
    '''<summary>���z�}�ۑ��{�^������������</summary>
    '''<remarks>���C����ʏ�̕��z�}��Bmp�t�@�C���ɕۑ�����</remarks>
    '''=========================================================================
    Private Sub SavePanelImage()

        Dim fileName As String
        Dim bFileSave As Boolean
        Dim bitMap As New Bitmap(PanelGraph.Width, PanelGraph.Height)

        Try

            bFileSave = False
            fileName = ""

            '�t�@�C�����̂̍쐬
            If gObjFrmDistribute.GetDispGrp() = True Then
                fileName = gSysPrm.stLOG.gsLoggingDir & "IT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            Else
                fileName = gSysPrm.stLOG.gsLoggingDir & "FT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            End If

            ' PanelGraph�̓��e��ۑ�
            PanelGraph.DrawToBitmap(bitMap, New Rectangle(0, 0, PanelGraph.Width, PanelGraph.Height))
            bitMap.Save(fileName)

            '���ʂ̕\��
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    MsgBox("�ۑ������I" & vbCrLf & " (" & fileName & ")")
            'Else
            '    MsgBox("Save completion." & vbCrLf & " (" & fileName & ")")
            'End If
            MsgBox(Form1_021 & vbCrLf & " (" & fileName & ")")
            bitMap.Dispose()

            Exit Sub

        Catch ex As Exception

            bitMap.Dispose()
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    MsgBox("�ۑ��ł��܂���ł����B")
            'Else
            '    MsgBox("I was not able to save it.")
            'End If
            MsgBox(Form1_022)

        End Try

    End Sub
#End Region

    ''' <summary> 'V4.1.0.0�D
    ''' �{�^���̕\����\����ݒ肷��
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChkButtonDisp()

        If stFNC(F_PROBE_CLEANING).iDEF >= 1 Then
            ProbeBtnVisibleSet(True)
        Else
            ProbeBtnVisibleSet(False)
        End If
        If stFNC(F_MAINTENANCE).iDEF >= 1 Then
            MaintBtnVisibleSet(True)
        Else
            MaintBtnVisibleSet(False)
        End If

    End Sub
    '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
    '========================================================================================
    '   �g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X����
    '========================================================================================
#Region "�g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X������������"
    '''=========================================================================
    '''<summary>�g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X������������</summary>
    '''=========================================================================
    Public Sub Init_StartBlkComb()

        Dim strMsg As String

        Try
            ' �g���~���O�J�n�u���b�N�ԍ��w�肪�����Ȃ�NOP
            If (giStartBlkAss = 0) Then Return

            ' DropDownStyle�v���p�e�B��DropDown�ɂ���(�e�L�X�g�����𒼐ڕҏW�ł���)
            '                          DropDownList�ɂ���ƃe�L�X�g�����𒼐ڕҏW�ł��Ȃ�
            Me.CbStartBlkX.DropDownStyle = ComboBoxStyle.DropDownList
            Me.CbStartBlkY.DropDownStyle = ComboBoxStyle.DropDownList

            ' �g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X������������
            Me.CbStartBlkX.Items.Clear()
            Me.CbStartBlkY.Items.Clear()
            Me.CbStartBlkX.Items.Add("  1")
            Me.CbStartBlkY.Items.Add("  1")
            Me.CbStartBlkX.SelectedIndex = 0
            Me.CbStartBlkY.SelectedIndex = 0

            SetGrpStartBlkText(1)       'V5.0.0.9�O

        Catch ex As Exception
            strMsg = "iTKY.Init_StartBlkComb() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�g���~���O�J�n�u���b�N�ԍ�XY��1,1�Ƃ���"
    '''=========================================================================
    '''<summary>�g���~���O�J�n�u���b�N�ԍ�XY��1,1�Ƃ���</summary>
    '''=========================================================================
    Public Sub Set_StartBlkComb1St()

        Dim strMsg As String

        Try
            ' �g���~���O�J�n�u���b�N�ԍ��w�肪�����Ȃ�NOP
            If (giStartBlkAss = 0) Then Return

            ' �g���~���O�J�n�u���b�N�ԍ�XY��1,1�Ƃ���
            Me.CbStartBlkX.SelectedIndex = 0
            Me.CbStartBlkY.SelectedIndex = 0

            SetGrpStartBlkText(1)       'V5.0.0.9�O

        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkComb1St() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X�̐ݒ�"
    '''=========================================================================
    '''<summary>�g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X�̐ݒ�</summary>
    '''=========================================================================
    Public Sub Set_StartBlkComb()

        Dim Idx As Integer
        Dim strMsg As String

        Try
            ' �g���~���O�J�n�u���b�N�ԍ��w�肪�����Ȃ�NOP
            If (giStartBlkAss = 0) Then Return

            ' �g���~���O�J�n�u���b�N�ԍ�X�R���{�{�b�N�X�̐ݒ�
            Me.CbStartBlkX.Items.Clear()
            For Idx = 1 To typPlateInfo.intBlockCntXDir
                Me.CbStartBlkX.Items.Add(Idx.ToString(0).PadLeft(3))
            Next Idx
            Me.CbStartBlkX.SelectedIndex = 0

            ' �g���~���O�J�n�u���b�N�ԍ�Y�R���{�{�b�N�X�̐ݒ�
            Me.CbStartBlkY.Items.Clear()
            For Idx = 1 To typPlateInfo.intBlockCntYDir
                Me.CbStartBlkY.Items.Add(Idx.ToString(0).PadLeft(3))
            Next Idx
            Me.CbStartBlkY.SelectedIndex = 0

            ' �J�n�u���b�N�ԍ�������/�񊈐���
            If (bFgAutoMode = False) Then                           ' �蓮�^�] ?
                GrpStartBlk.Enabled = True                          ' ������ 
            Else
                GrpStartBlk.Enabled = False                         ' ������ 
            End If

            SetGrpStartBlkText(1)       'V5.0.0.9�O

        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkComb() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�g���~���O�J�n�u���b�N�ԍ�XY���擾����"
    '''=========================================================================
    '''<summary>�g���~���O�J�n�u���b�N�ԍ�XY���擾����</summary>
    ''' <param name="BlkX">(OUT)�u���b�N�ԍ�X</param>
    ''' <param name="BlkY">(OUT)�u���b�N�ԍ�Y</param>
    '''=========================================================================
    Public Sub Get_StartBlkNum(ByRef BlkX As Integer, ByRef BlkY As Integer)

        Dim strMsg As String

        Try
            ' �g���~���O�J�n�u���b�N�ԍ��w�肪�����Ȃ�NOP
            If (giStartBlkAss = 0) Then Return

            ' �g���~���O�J�n�u���b�N�ԍ�XY���擾����
            BlkX = Me.CbStartBlkX.SelectedIndex + 1
            BlkY = Me.CbStartBlkY.SelectedIndex + 1

        Catch ex As Exception
            strMsg = "iTKY.Get_StartBlkNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�g���~���O�J�n�u���b�N�ԍ�XY��ݒ肷��"
    '''=========================================================================
    '''<summary>�g���~���O�J�n�u���b�N�ԍ�XY��ݒ肷��</summary>
    ''' <param name="BlkX">(INP)�u���b�N�ԍ�X</param>
    ''' <param name="BlkY">(INP)�u���b�N�ԍ�Y</param>
    '''=========================================================================
    Public Sub Set_StartBlkNum(ByVal BlkX As Integer, ByVal BlkY As Integer)

        Dim strMsg As String

        Try
            ' �g���~���O�J�n�u���b�N�ԍ��w�肪�����Ȃ�NOP
            If (giStartBlkAss = 0) Then Return

            If Me.CbStartBlkX.Items.Count >= BlkX Then
                ' �g���~���O�J�n�u���b�N�ԍ�X��ݒ肷��
                Me.CbStartBlkX.SelectedIndex = BlkX - 1
            Else
                Me.CbStartBlkX.SelectedIndex = 0
            End If
            If Me.CbStartBlkY.Items.Count >= BlkY Then
                ' �g���~���O�J�n�u���b�N�ԍ�Y��ݒ肷��
                Me.CbStartBlkY.SelectedIndex = BlkY - 1
            Else
                Me.CbStartBlkY.SelectedIndex = 0
            End If

            'V5.0.0.9�O                  ��
            If (2 = giStartBlkAss) Then
                Dim blockNo As Integer = basTrimming.GetProcessingOrder(BlkX, BlkY)
                SetGrpStartBlkText(blockNo)
            End If
            'V5.0.0.9�O                  ��
        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X��������/�񊈐�������"
    '''=========================================================================
    ''' <summary>�g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X��������/�񊈐�������</summary>
    ''' <param name="bFlg">(INP)TRUE=������, FALSE=�񊈐���</param>
    '''=========================================================================
    Public Sub Set_StartBlkNum_Enabled(ByVal bFlg As Boolean)

        Dim strMsg As String

        Try
            ' �g���~���O�J�n�u���b�N�ԍ��w�肪�����Ȃ�NOP
            If (giStartBlkAss = 0) Then Return

            ' �g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X��������/�񊈐�������
            Me.GrpStartBlk.Enabled = bFlg
            Me.CbStartBlkX.Enabled = bFlg
            Me.CbStartBlkY.Enabled = bFlg

        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkNum_Enabled() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0�D�� -----

    Private Sub btnJudge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJudge.Click
        Dim objform As New frmStopCond

        objform.Owner = Me
        objform.ShowDialog()

    End Sub

    ''' <summary>
    ''' ��ʏ��NEXT�{�^��������������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNEXT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNEXT.Click
        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer
        Dim plateCnt As Integer
        Dim blockCnt As Integer

        Try
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

            ' �X�e�[�W�ړ�
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = frmFineAdjust.MOVE_NEXT) Then
                ' �ړ���̃v���[�g�ԍ��A�u���b�N�ԍ���ۑ�����
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                '                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0�D
                'V5.0.0.9�O                Call Set_StartBlkNum(workBlockNo, 1)  ����

                'V4.0.0.0�L��
                If gKeiTyp = KEY_TYPE_RS Then
                    'V5.0.0.9�O                  ��
                    Select Case (giStartBlkAss)
                        Case 1          ' ����
                            Set_StartBlkNum(workBlockNo, 1)
                            SetBlockDisplayNumber(gCurBlockNo)

                        Case 2          'V5.0.0.9�O
                            SetBlockDisplayNumber(workBlockNo, True)
                    End Select
                    'V5.0.0.9�O                  ��
                    SetNowBlockDspNum(gCurBlockNo)                      'V4.1.0.0�Q
                End If
                'V4.0.0.0�L��
            End If

STP_END:

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "Form1.btnNEXT_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub

    ''' <summary>
    ''' ��ʏ��PREV�{�^��������������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPREV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPREV.Click

        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer

        Try
            ' �擪�v���[�g�A�擪�u���b�N�ł���Έړ����Ȃ�
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

            ' �X�e�[�W�ړ�
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = frmFineAdjust.MOVE_NEXT) Then
                ' �ړ���̃v���[�g�ԍ��A�u���b�N�ԍ���ۑ�����
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                ' lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0�D
                'V4.1.0.0�Q
                If gKeiTyp = KEY_TYPE_RS Then
                    'V5.0.0.9�O                  ��
                    Select Case (giStartBlkAss)
                        Case 1          ' ����
                            Set_StartBlkNum(workBlockNo, 1)

                        Case 2          'V5.0.0.9�O
                            SetBlockDisplayNumber(workBlockNo, True)
                    End Select
                    'V5.0.0.9�O                  ��
                    SetNowBlockDspNum(gCurBlockNo)
                End If
                'V4.1.0.0�Q

            End If
STP_END:

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "Form1.btnPREV_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub

#Region "�X�e�[�W���w��u���b�N�ʒu�Ɉړ�����"
    ''' <summary>�X�e�[�W���w��u���b�N�ʒu�Ɉړ�����</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V5.0.0.9�O</remarks>
    Private Sub CbStartBlkXY_SelectionChangeCommitted(ByVal sender As Object, ByVal e As EventArgs) Handles CbStartBlkY.SelectionChangeCommitted, CbStartBlkX.SelectionChangeCommitted
        If (1 < giStartBlkAss) Then
            Try
                Dim x, y As Integer : Get_StartBlkNum(x, y)
                Dim workBlockNo As Integer = basTrimming.GetProcessingOrder(x, y)
                SetGrpStartBlkText(workBlockNo)

                If (Globals_Renamed.gKeiTyp = Globals_Renamed.KEY_TYPE_RS) Then
                    SimpleTrimmer.SetBlockDisplayNumber(workBlockNo, True)
                End If

                If (btnPREV.Enabled OrElse btnNEXT.Enabled) Then
                    ' �X�e�[�W�ړ�
                    Dim intRet As Integer = basTrimming.MoveTargetStagePos(Globals_Renamed.gCurPlateNo, workBlockNo)
                    If (intRet = frmFineAdjust.MOVE_NEXT) Then
                        ' �ړ���̃u���b�N�ԍ���ۑ�����
                        Globals_Renamed.gCurBlockNo = workBlockNo
                    End If
                End If

            Catch ex As Exception
                Dim strMsg As String
                strMsg = "Form1.CbStartBlkXY_SelectionChangeCommitted() TRAP ERROR = " & ex.Message
                MessageBox.Show(Me, strMsg)
            End Try
        End If

    End Sub
#End Region

#Region "�u���b�N�ʒu�w��R���{�{�b�N�X�w�E�x�ɊY������u���b�N�ԍ���\������"
    ''' <summary>�u���b�N�ʒu�R���{�{�b�N�X�w�E�x�ɊY������u���b�N�ԍ���\������</summary>
    ''' <param name="blockNo">���H�Ώۃu���b�N�ԍ�</param>
    ''' <remarks>'V5.0.0.9�O</remarks>
    Private Sub SetGrpStartBlkText(ByVal blockNo As Integer)
        Static text As String = Me.GrpStartBlk.Text & " "
        If (2 = giStartBlkAss) Then
            Me.GrpStartBlk.Text = text & blockNo
        End If
    End Sub
#End Region

#Region "�p���`�F�b�N�{�b�N�X�̕`��������Ȃ�"
    ''' <summary>�p���`�F�b�N�{�b�N�X�̕`��������Ȃ�</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V5.0.0.9�O</remarks>
    Private Sub chkContinue_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles chkContinue.Paint
        Dim chk As CheckBox = DirectCast(sender, CheckBox)
        Dim rect As Rectangle = chk.DisplayRectangle
        If (rect.IsEmpty) Then Return

        Dim state As ButtonState
        Dim bgColor As Color
        If (chk.Checked) Then
            state = ButtonState.Checked
            bgColor = Color.Yellow
        Else
            state = ButtonState.Normal
            bgColor = SystemColors.Control
        End If
        If (False = chk.Enabled) Then
            state = ButtonState.Flat
        End If
        ' �{�^���Ƃ��ĕ`��
        ControlPaint.DrawButton(e.Graphics, rect, state)

        rect.Inflate(-2, -2)    ' �`��̈���k��
        Using b As Brush = New SolidBrush(bgColor)
            ' �w�i�F�Ƃ��ă{�^���̈����h��Ԃ�
            e.Graphics.FillRectangle(b, rect)
        End Using

        ' �����`��
        TextRenderer.DrawText(
            e.Graphics,
            chk.Text,
            chk.Font,
            rect,
            If(chk.Enabled, chk.ForeColor, SystemColors.GrayText),
            TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)

    End Sub
#End Region
    'V4.12.2.0�@                         ��
#Region "�}�b�v���v���[�g�f�[�^�̃u���b�N��X�EY�ŏ���������"
    ''' <summary>�}�b�v���v���[�g�f�[�^�̃u���b�N��X�EY�ŏ���������</summary>
    ''' <param name="doEnabled">�}�b�v�L��:true, ����:false (�f�[�^�t�@�C���ǂݍ��݌���)</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Public Shared Sub MapInitialize(ByVal doEnabled As Boolean)
        With Form1.Instance
            If (._mapDisp) Then         'tky.ini[DEVICE_CONST]MAP_DISP
                If (doEnabled) Then
                    Dim cntX As Integer = typPlateInfo.intBlockCntXDir
                    Dim cntY As Integer = typPlateInfo.intBlockCntYDir
#If False Then  'V6.0.1.3�@
                    .TrimMap1.TrimMapInitialize(.VideoLibrary1.Top, .VideoLibrary1.Left,
                                                .VideoLibrary1.Width, .VideoLibrary1.Height,
                                                cntX, cntY, gSysPrm.stDEV.giBpDirXy,
                                                True, True, (cntX <= 45), (cntY <= 50),
                                                False, True)

                    Globals_Renamed.ProcBlock = New Integer(cntX, cntY) {}      'V4.12.2.0�A
#Else
                    ' �v���[�g�Ή�    'V6.0.1.3�@
                    Dim area As Size = .VideoLibrary1.CameraArea
                    Dim Width As Integer = area.Width
                    Dim Height As Integer = area.Height

                    Dim pcx As Integer = typPlateInfo.intPlateCntXDir
                    Dim pcy As Integer = typPlateInfo.intPlateCntYDir

                    Dim col, row As Integer
                    If (1 < pcx) AndAlso (1 < pcy) Then
                        col = pcx
                        row = pcy
                    Else
                        col = cntX
                        row = cntY
                    End If

                    Dim args As New TrimMapPlate.InitializeArgs()
                    args.Top = .VideoLibrary1.Top
                    args.Left = .VideoLibrary1.Left
                    args.Width = Width
                    args.Height = Height
                    args.PlateCountX = pcx
                    args.PlateCountY = pcy
                    args.BlockCountX = cntX
                    args.BlockCountY = cntY
                    args.DirStepRepeat = typPlateInfo.intDirStepRepeat
                    args.ColumnHeaderVisible = True
                    args.RowHeaderVisible = True
                    args.DrawColumnNumber = (col <= 45)
                    args.DrawRowNumber = (row <= 50)
                    args.DrawBlockNumber = False
                    args.DrawBlockNumberStyle = DISPCELL_TYPE.CELL_XY   ' TODO: tky.ini[MAP]CELLNO_TYPE ���g�p���邩
                    args.ToolTipVisible = True
                    args.KeepBlockSelect = False
                    args.CanBlockSelect = False
                    args.DisableFourCorners = False

                    .TrimMap1.Initialize(args)
#End If
                Else
                    .TrimMap1.Visible = doEnabled       ' �}�b�v�\��
                End If
                .CmdMapOnOff.Enabled = doEnabled        ' �}�b�v�I���I�t�{�^��
            End If
        End With
    End Sub
#End Region

#Region "�I���ς݉��H�Ώۃu���b�N��ύX�s�ŕ\������"
    ''' <summary>�I���ς݉��H�Ώۃu���b�N��ύX�s�ŕ\������</summary>
    ''' <param name="onCameraArea">True:�J�����摜�ɏd�˂�,False:�J�����摜�̉�</param>
    ''' <remarks>'V4.12.2.0�A</remarks>
    Public Shared Sub ShowSelectedMap(ByVal onCameraArea As Boolean)    'V6.0.1.0�J
        'V6.0.1.0�J    Public Shared Sub ShowSelectedMap()
        With Form1.Instance
            If (._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
                Dim plateX As Integer = typPlateInfo.intPlateCntXDir    'V6.0.1.3�@
                Dim plateY As Integer = typPlateInfo.intPlateCntYDir    'V6.0.1.3�@
                Dim blockX As Integer = typPlateInfo.intBlockCntXDir    'V6.0.1.3�@ cntX -> blockX
                Dim blockY As Integer = typPlateInfo.intBlockCntYDir    'V6.0.1.3�@ cntY -> blockY

                Dim top, left, width, height As Integer
                Dim area As Size = .VideoLibrary1.CameraArea            'V6.0.1.0�H
                width = area.Width
                height = area.Height

                If (onCameraArea) Then
                    top = .VideoLibrary1.Top
                    left = .VideoLibrary1.Left
                Else
                    'V6.0.1.0�J          ��
                    top = .VideoLibrary1.Top + height + 1
                    left = .VideoLibrary1.Left
                    'V6.0.1.0�J          ��
                End If
#If False Then  'V6.0.1.3�@
                .TrimMap1.InitializeNotSelectable(top, left, width, height,
                                                  cntX, cntY, gSysPrm.stDEV.giBpDirXy,
                                                  True, True, (cntX <= 45), (cntY <= 50),
                                                  False, True, Globals_Renamed.ProcBlock, 1)
#Else
                ' �v���[�g�Ή�    'V6.0.1.3�@
                Dim col, row As Integer
                If (1 = plateX) AndAlso (1 = plateY) Then
                    col = blockX
                    row = blockY
                Else
                    col = plateX
                    row = plateY
                End If

                Dim args As New TrimMapPlate.InitializeArgs()
                args.Top = top
                args.Left = left
                args.Width = width
                args.Height = height
                args.PlateCountX = plateX
                args.PlateCountY = plateY
                args.BlockCountX = blockX
                args.BlockCountY = blockY
                args.DirStepRepeat = typPlateInfo.intDirStepRepeat
                args.ColumnHeaderVisible = True
                args.RowHeaderVisible = True
                args.DrawColumnNumber = (col <= 45)
                args.DrawRowNumber = (row <= 50)
                args.DrawBlockNumber = False
                args.DrawBlockNumberStyle = DISPCELL_TYPE.CELL_XY       ' TODO: tky.ini[MAP]CELLNO_TYPE ���g�p���邩
                args.ToolTipVisible = True
                args.KeepBlockSelect = False
                args.CanBlockSelect = False
                args.DisableFourCorners = False

                .TrimMap1.Initialize(args)
#End If
                .TrimMap1.BringToFront()        'V6.0.1.0�J
            End If
        End With

        ClearTrimmingResult()           'V6.0.1.0�K
    End Sub
#End Region

#Region "�}�b�v�\���ʒu���J�����摜��E���Ɉړ�����"
    ''' <summary>�}�b�v�\���ʒu���J�����摜��E���Ɉړ�����</summary>
    ''' <param name="startTrimming">True:�g���~���O�J�n��,False:�g���~���O�I����</param>
    ''' <remarks>'V6.0.1.0�J</remarks>
    Friend Shared Sub MoveTrimMapLocation(ByVal startTrimming As Boolean)
        With Form1.Instance
            If (._mapDisp) Then
                Dim location As Point
                If (startTrimming) Then
                    location = New Point(.VideoLibrary1.Left,
                                         .VideoLibrary1.Top + .VideoLibrary1.CameraArea.Height + 1)
                Else
                    location = .VideoLibrary1.Location
                End If
                .TrimMap1.Location = location
            End If
        End With
    End Sub
#End Region

#Region "frmHistoryData�̈ʒu�E�T�C�Y��ݒ肷��"
    ''' <summary>frmHistoryData�̈ʒu�E�T�C�Y��ݒ肷��</summary>
    ''' <param name="startTrimming">True:�g���~���O�J�n��,False:�g���~���O�I����</param>
    ''' <remarks>'V6.0.1.0�J</remarks>
    Friend Shared Sub MoveHistoryDataLocation(ByVal startTrimming As Boolean)
        With Form1.Instance
            ''V6.0.1.0�P��            If (._mapDisp) Then
            ' Map�{�^�����n�m�̂Ƃ��ɂ́A�\���ʒu��؂�ւ��� 
            If (Form1.MapOn) Then
                If (startTrimming) Then
                    .frmHistoryData.Location = .tabCmd.Location
                    .frmHistoryData.Size = .tabCmd.Size
                Else
                    .frmHistoryData.SetBounds(.HistoryDataLocation.X,
                                              .HistoryDataLocation.Y,
                                              .HistoryDataSize.Width,
                                              .HistoryDataSize.Height)
                End If
                .btnCounterClear.Visible = Not startTrimming
            Else
                .frmHistoryData.SetBounds(.HistoryDataLocation.X,
                                          .HistoryDataLocation.Y,
                                          .HistoryDataSize.Width,
                                          .HistoryDataSize.Height)
                .btnCounterClear.Visible = Not startTrimming
            End If
            ''V6.0.1.0�P��
        End With
    End Sub
    Friend HistoryDataLocation As Point 'V6.0.1.0�J
    Friend HistoryDataSize As Size      'V6.0.1.0�J
#End Region

#Region "�g���~���O�̔��茋�ʂ𐔂���"
    ''' <summary>�g���~���O�̔��茋�ʂ𐔂���</summary>
    ''' <param name="result">���茋��</param>
    ''' <returns>���v��</returns>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Public Shared Function CountTrimmingResults(ByVal result As Integer) As Integer
        Dim ret As Integer = (-1)

        Dim tr As TrimmingResult = Nothing
        If (Form1.Instance._trimmingResults.TryGetValue(result, tr)) Then
            tr.Count += 1
            ret = tr.Count
        End If

        Return ret
    End Function

    ''' <summary>�g���~���O�̔��茋�ʂ��N���A����</summary>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Public Shared Sub ClearTrimmingResult()
        With Form1.Instance
            For Each key As Integer In ._trimmingResults.Keys
                ._trimmingResults(key).Count = 0
            Next
        End With
    End Sub

    ''' <summary>�g���~���O�̔��茋�ʂŎg�p����F���擾����</summary>
    ''' <param name="result">���茋��</param>
    ''' <returns>���茋�ʂŎg�p����F</returns>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Public Shared Function GetResultColor(ByVal result As Integer) As Brush
        Dim ret As Brush = Nothing

        Dim tr As TrimmingResult = Nothing
        If (Form1.Instance._trimmingResults.TryGetValue(result, tr)) Then
            ret = tr.Color
        End If

        Return ret
    End Function

    ''' <summary>�g���~���O�̔��茋�ʂƐF�E���v�����Ǘ�����</summary>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Private _trimmingResults As New Dictionary(Of Integer, TrimmingResult)

    ''' <summary>�g���~���O���茋�ʐF�E��</summary>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Private Class TrimmingResult
        Private _color As Brush
        Friend ReadOnly Property Color() As Brush
            Get
                Return _color
            End Get
        End Property

        Private _colorName As String
        Friend ReadOnly Property ColorName() As String
            Get
                If (String.IsNullOrEmpty(_colorName)) Then
                    _colorName = DirectCast(_color, SolidBrush).Color.Name
                End If
                Return _colorName
            End Get
        End Property

        Private _keyname As String
        Friend ReadOnly Property KeyName() As String
            Get
                Return _keyname
            End Get
        End Property

        Friend Count As Integer

        Public Sub New(ByVal brush As Brush, ByVal keyName As String)
            _color = brush
            _keyname = keyName
            Count = 0
        End Sub

    End Class
#End Region

#Region "�g���~���O���ʕ\���}�b�v�摜���������"
    ''' <summary>�g���~���O���ʕ\���}�b�v�摜���������</summary>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Friend Shared Sub PrintTrimMap()
        With Form1.Instance
            If (._mapDisp AndAlso ._printMap) Then
                Using bmp As Bitmap = .TrimMap1.GetMapImage()
                    If (bmp IsNot Nothing) Then
                        Dim sb As New StringBuilder()
                        For Each tr As TrimmingResult In ._trimmingResults.Values
                            sb.Append(tr.ColorName)
                            sb.Append(",")
                            sb.Append(tr.KeyName)
                            sb.Append(",")
                            sb.Append(tr.Count.ToString())
                            sb.Append("_")
                        Next

                        If (PrintBase.DefaultPrinterIsOffline) Then
                            Using pp As New PrintPdf(gStrTrimFileName)
                                pp.SaveFilePath =
                                        ._mapPdfDir &
                                        IO.Path.GetFileNameWithoutExtension(gStrTrimFileName) &
                                        "_" & DateTime.Now.ToString("yyyyMMdd-HHmmss") & ".pdf"
                                pp.PrintMap(bmp, sb)

                                .Z_PRINT(Form1_025)                                 ' �u�ʏ�g���v�����^�[�v���I�t���C���ł��B
                                .Z_PRINT(String.Format(Form1_026, pp.SaveFilePath)) ' {0} ���쐬���܂����B
                            End Using
                        Else
                            Using pi As New PrintImage(gStrTrimFileName)
                                pi.PrintMap(bmp, sb)
                            End Using
                        End If
                    End If
                End Using
            End If

        End With
    End Sub
#End Region

#Region "�}�b�v�摜��������̃I���I�t��ݒ肷��"
    Private _printMap As Boolean = False
    ''' <summary>�}�b�v�摜��������̃I���I�t��ݒ肷��</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.1.0�K</remarks>
    Private Sub CmdPrintMap_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdPrintMap.Click
        _printMap = Not _printMap
        With CmdPrintMap
            If (_printMap) Then
                .BackColor = Color.Lime
                .Text = "Print ON"
            Else
                .BackColor = SystemColors.Control
                .Text = "Print OFF"
            End If
        End With
        LblDataFileName.Select()
    End Sub
#End Region

#Region "�w��u���b�N�̔w�i�F��ݒ肷��"
#If False Then  'V6.0.1.3�@
    ''' <summary>�w��u���b�N�ԍ�(���H��)�̔w�i�F��ݒ肷��</summary>
    ''' <param name="blockNo">�u���b�N�ԍ�</param>
    ''' <param name="color">�w�i�F</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Public Shared Sub SetMapColor(ByVal blockNo As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Dim x, y As Integer
            Globals_Renamed.GetBlockPosition(blockNo, x, y)
            Form1.Instance.TrimMap1.TrimMapColorSet(x, y, color)
        End If
    End Sub

    ''' <summary>�w��ʒu(X,Y)�u���b�N�̔w�i�F��ݒ肷��</summary>
    ''' <param name="blockPosX">�u���b�N�ʒu�w(1ORG)</param>
    ''' <param name="blockPosY">�u���b�N�ʒu�x(1ORG)</param>
    ''' <param name="color">�w�i�F</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Public Shared Sub SetMapColor(ByVal blockPosX As Integer, ByVal blockPosY As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapColorSet(blockPosX, blockPosY, color)
        End If
    End Sub
#Else
    ''' <summary>�w��u���b�N�ԍ�(���H��)�̔w�i�F��ݒ肷��</summary>
    ''' <param name="plateNo">�v���[�g�������ԍ�(1ORG)</param>
    ''' <param name="blockNo">�u���b�N�������ԍ�(1ORG)</param>
    ''' <param name="color">�w�i�F</param>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Public Shared Sub SetMapColor(ByVal plateNo As Integer, ByVal blockNo As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then
            Form1.Instance.TrimMap1.TrimMapColorSet(plateNo, blockNo, color)
        End If
    End Sub

    ''' <summary>�w��ʒu(X,Y)�u���b�N�̔w�i�F��ݒ肷��</summary>
    ''' <param name="platePosX">�v���[�g�ʒu�w(1ORG)</param>
    ''' <param name="platePosY">�v���[�g�ʒu�x(1ORG)</param>
    ''' <param name="blockPosX">�u���b�N�ʒu�w(1ORG)</param>
    ''' <param name="blockPosY">�u���b�N�ʒu�x(1ORG)</param>
    ''' <param name="color">�w�i�F</param>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Public Shared Sub SetMapColor(ByVal platePosX As Integer, ByVal platePosY As Integer,
                                  ByVal blockPosX As Integer, ByVal blockPosY As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapColorSet(platePosX, platePosY, blockPosX, blockPosY, color)
        End If
    End Sub
#End If
#End Region

#Region "�w��u���b�N�ɘg����\������"
#If False Then  'V6.0.1.3�@
    ''' <summary>�w��u���b�N�ԍ�(���H��)�̘g����\������</summary>
    ''' <param name="blockNo">�u���b�N�ԍ�</param>
    ''' <param name="color">�g���̐F</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Public Shared Sub SetMapBorder(ByVal blockNo As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Dim x, y As Integer
            Globals_Renamed.GetBlockPosition(blockNo, x, y)
            Form1.Instance.TrimMap1.TrimMapBorderSet(x, y, color)

        End If
    End Sub

    ''' <summary>�w��ʒu(X,Y)�u���b�N�̘g����\������</summary>
    ''' <param name="blockPosX">�u���b�N�ʒu�w(1ORG)</param>
    ''' <param name="blockPosY">�u���b�N�ʒu�x(1ORG)</param>
    ''' <param name="color">�g���̐F</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Public Shared Sub SetMapBorder(ByVal blockPosX As Integer, ByVal blockPosY As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapBorderSet(blockPosX, blockPosY, color)
        End If
    End Sub
#Else
    ''' <summary>�w��u���b�N�ԍ�(���H��)�̘g����\������</summary>
    ''' <param name="plateNo">�v���[�g�������ԍ�(1ORG)</param>
    ''' <param name="blockNo">�u���b�N�������ԍ�(1ORG)</param>
    ''' <param name="color">�g���̐F</param>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Public Shared Sub SetMapBorder(ByVal plateNo As Integer, ByVal blockNo As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapBorderSet(plateNo, blockNo, color)
        End If
    End Sub

    ''' <summary>�w��ʒu(X,Y)�u���b�N�̘g����\������</summary>
    ''' <param name="plateX">�v���[�g�ʒu�w(1ORG)</param>
    ''' <param name="plateY">�v���[�g�ʒu�x(1ORG)</param>
    ''' <param name="blockX">�u���b�N�ʒu�w(1ORG)</param>
    ''' <param name="blockY">�u���b�N�ʒu�x(1ORG)</param>
    ''' <param name="color">�g���̐F</param>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Public Shared Sub SetMapBorder(ByVal plateX As Integer, ByVal plateY As Integer,
                                   ByVal blockX As Integer, ByVal blockY As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapBorderSet(plateX, plateY, blockX, blockY, color)
        End If
    End Sub
#End If
    ''' <summary>�Ō�ɕ\�������u���b�N�̘g��������</summary>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Public Shared Sub ClearMapBorder()
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapClearMapBorder()
        End If
    End Sub
#End Region

#Region "�}�b�v�̕\����ԕύX����MAP ON/OFF�{�^���̕\����ݒ肷��"
    ''' <summary>�}�b�v�̕\����ԕύX����MAP ON/OFF�{�^���̕\����ݒ肷��</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Private Sub TrimMap1_VisibleChanged(sender As Object, e As EventArgs) Handles TrimMap1.VisibleChanged
        With CmdMapOnOff
            If (TrimMap1.Visible) Then
                .BackColor = Color.Lime
                .Text = "MAP ON"
            Else
                .BackColor = SystemColors.Control
                .Text = "MAP OFF"
            End If
        End With
    End Sub
#End Region

#Region "MAP ON/OFF�{�^���̃N���b�N�ɂ��}�b�v�̕\����Ԃ�؂�ւ���"
    ''' <summary>MAP ON/OFF�{�^���̃N���b�N�ɂ��}�b�v�̕\����Ԃ�؂�ւ���</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Private Sub CmdMapOnOff_Click(sender As Object, e As EventArgs) Handles CmdMapOnOff.Click
        Me.LblDataFileName.Select()
        Select Case (giAppMode)
            Case APP_MODE_FINEADJ, APP_MODE_IDLE, APP_MODE_TRIM, APP_MODE_TRIM_AUTO
                ' �ꎞ��~��, �A�C�h����
                _mapOn = Not TrimMap1.Visible
                SetTrimMapVisible(_mapOn)
            Case Else
                ' DO NOTHING
        End Select
    End Sub
    Private Shared _mapOn As Boolean = False  ' MAP ON/OFF �{�^���ɂ��ݒ��ێ�����
    ''' <summary>MAP ON�ł��邩</summary>
    ''' <returns>Ture:ON,False:OFF</returns>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Friend Shared ReadOnly Property MapOn As Boolean
        Get
            Return _mapOn
        End Get
    End Property

    ''' <summary>MAP ON/OFF �{�^����Enabled��ݒ肷��</summary>
    ''' <param name="enabled">Enabled</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Friend Shared Sub SetMapOnOffButtonEnabled(ByVal enabled As Boolean)
        With Form1.Instance
            If (._mapDisp) Then         'tky.ini[DEVICE_CONST]MAP_DISP
                .CmdMapOnOff.Enabled = (enabled) AndAlso (gLoadDTFlag)
            End If
        End With
    End Sub
#End Region

#Region "�}�b�v�̕\����Ԃ�ݒ肷��"
    ''' <summary>�}�b�v�̕\����Ԃ�ݒ肷��</summary>
    ''' <param name="visible">True:�\��,False:��\��</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Friend Shared Sub SetTrimMapVisible(ByVal visible As Boolean)
        'V6.0.1.0�I    Private Sub SetTrimMapVisible(ByVal visible As Boolean)
        With Form1.Instance
            'V6.0.1.0�P��
            '            If (._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            If (Form1.MapOn) Then
                .TrimMap1.Visible = visible
                If (visible) Then
                    .TrimMap1.BringToFront()
                Else
                    .TrimMap1.SendToBack()
                End If
            Else
                .TrimMap1.Visible = False
                .TrimMap1.SendToBack()
            End If
            'V6.0.1.0�P��
        End With
    End Sub
#End Region
    'V4.12.2.0�@                         ��
#Region "�}�b�v��̃u���b�N���I������Ă��邩���擾����"
    ''' <summary>�}�b�v��̃u���b�N���I������Ă��邩���擾����</summary>
    ''' <returns>True:�I������Ă���, False:�I������Ă��Ȃ�</returns>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Friend Shared ReadOnly Property TrimMapSelected() As Boolean

        Get
            Return (0 < Form1.Instance.TrimMap1.GetSelectBlockCount())
        End Get
    End Property
#End Region

#Region "���̏����Ώۃv���[�g�ԍ��E�u���b�N�ԍ����擾����"
    ''' <summary>���̏����Ώۃv���[�g�ԍ��E�u���b�N�ԍ����擾����</summary>
    ''' <param name="currentPlate">���݂̏������v���[�g�ԍ�(1ORG)</param>
    ''' <param name="currentBlock">���݂̏������u���b�N�ԍ�(1ORG)</param>
    ''' <param name="nextPlate">���̏������v���[�g�ԍ�(1ORG)</param>
    ''' <param name="nextBlock">���̏������u���b�N�ԍ�(1ORG)</param>
    ''' <returns>True:�����Ώۃu���b�N����, False:�����Ώۃu���b�N�Ȃ�</returns>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Friend Shared Function GetNextSelectBlock(ByVal currentPlate As Integer, ByVal currentBlock As Integer,
                                              ByRef nextPlate As Integer, ByRef nextBlock As Integer) As Boolean
        Return Form1.Instance.TrimMap1.GetNextSelectBlock(currentPlate, currentBlock, nextPlate, nextBlock)
    End Function
#End Region

#Region "�O�̏����Ώۃv���[�g�ԍ��E�u���b�N�ԍ����擾����"
    ''' <summary>�O�̏����Ώۃv���[�g�ԍ��E�u���b�N�ԍ����擾����</summary>
    ''' <param name="currentPlate">���݂̏������v���[�g�ԍ�(1ORG)</param>
    ''' <param name="currentBlock">���݂̏������u���b�N�ԍ�(1ORG)</param>
    ''' <param name="prevPlate">�O�̏������v���[�g�ԍ�(1ORG)</param>
    ''' <param name="prevBlock">�O�̏������u���b�N�ԍ�(1ORG)</param>
    ''' <returns>True:�����Ώۃu���b�N����, False:�����Ώۃu���b�N�Ȃ�</returns>
    ''' <remarks>�v���[�g�Ή� 'V6.0.1.3�@</remarks>
    Friend Shared Function GetPrevSelectBlock(ByVal currentPlate As Integer, ByVal currentBlock As Integer,
                                              ByRef prevPlate As Integer, ByRef prevBlock As Integer) As Boolean
        Return Form1.Instance.TrimMap1.GetPrevSelectBlock(currentPlate, currentBlock, prevPlate, prevBlock)
    End Function
#End Region

#Region "���O�\���e�L�X�g�{�b�N�X���ŏI�s�܂ŃX�N���[������"
    ''' <summary>���O�\���e�L�X�g�{�b�N�X���ŏI�s�܂ŃX�N���[������</summary>
    ''' <remarks>'#4.12.2.0�C</remarks>
    Friend Shared Sub TxtLogScrollToCaret()

        Static hWnd As IntPtr = Form1.Instance.txtLog.Handle
        Const WM_SETREDRAW As Integer = &HB
        Const EM_GETLINECOUNT As Integer = &HBA
        Const EM_LINESCROLL As Integer = &HB6
        Const WM_GETTEXTLENGTH As Integer = &HE
        Const EM_SETSEL As Integer = &HB1

        Try
            SendMessage(hWnd, WM_SETREDRAW, 0, 0)                           ' �`���~
            Dim lines As Integer = SendMessage(hWnd, EM_GETLINECOUNT, 0, 0)
            SendMessage(hWnd, EM_LINESCROLL, 0, lines)                      ' �ŏI�s��\��

            Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)  ' �������擾
            SendMessage(hWnd, EM_SETSEL, len, len)                          ' �J�[�\���𖖔���

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.TxtLogScrollToCaret() TRAP ERROR = " & ex.Message
            MessageBox.Show(Form1.Instance, strMSG)

        Finally
            SendMessage(hWnd, WM_SETREDRAW, 1, 0)                           ' �`��ĊJ
        End Try

    End Sub
#End Region

#Region "���O�\�����������������l�ȏ�ł��邩���擾����"
    ''' <summary>���O�\�����������������l�ȏ�ł��邩���擾����</summary>
    ''' <returns>True:�����ȏ�,False:��������</returns>
    ''' <remarks>'#4.12.2.0�C</remarks>
    Friend Shared Function TxtLogLengthLimit() As Boolean
        Static hWnd As IntPtr = Form1.Instance.txtLog.Handle
        Const WM_GETTEXTLENGTH As Integer = &HE
        Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)      ' �������擾
        Return (_txtLogClearLength <= len)
    End Function
    Private Shared _txtLogClearLength As Integer
#End Region

#Region "�A���C�����g�E�ƕ␳���ʊǗ��N���X"
    ''' <summary>
    ''' �A���C�����g�E�ƕ␳���ʊǗ��N���X
    ''' </summary>
    ''' <remarks>'V5.0.0.9�D</remarks>
    Private Class PatternRecog
        ''' <summary>
        ''' �p�^�[���P�E�Q�Ƃ���臒l�ȓ��Ń}�b�`�������ǂ���
        ''' </summary>
        ''' <returns>True:�}�b�`����,False:�}�b�`���Ȃ�</returns>
        Public ReadOnly Property IsMatch As Boolean
            Get
                Return (_th1 <= ThetaCorInfo.fCorV1) AndAlso (_th2 <= ThetaCorInfo.fCorV2)
            End Get
        End Property

        Private _isRough As Boolean
        ''' <summary>
        ''' ���t�A���C�����g�̎��s���ʂł��邩�ǂ���
        ''' </summary>
        ''' <returns>True:���t�A���C�����g,False:���t�A���C�����g�ł͂Ȃ�</returns>
        Public ReadOnly Property IsRough As Boolean
            Get
                Return _isRough
            End Get
        End Property

        Private _th1 As Double
        Private _th2 As Double

        ''' <summary>
        ''' VideoLibrary.CorrectTheta()�̎��s����
        ''' </summary>
        Public ThetaCorInfo As VideoLibrary.Theta_Cor_Info

        ''' <summary>
        ''' �R���X�g���N�^
        ''' </summary>
        ''' <param name="isRough">���t�A���C�����g�̏ꍇ�� True</param>
        ''' <param name="threshPt1">�p�^�[���P��臒l</param>
        ''' <param name="threshPt2">�p�^�[���Q��臒l</param>
        Public Sub New(ByVal isRough As Boolean, ByVal threshPt1 As Double, ByVal threshPt2 As Double)
            _isRough = isRough
            _th1 = threshPt1
            _th2 = threshPt2

            ThetaCorInfo = New VideoLibrary.Theta_Cor_Info()
        End Sub
    End Class
#End Region

#Region "�����J���Ă��A�v���P�[�V�����I�����Ȃ���Ԃ�\������"
    ''' <summary>�����J���Ă��A�v���P�[�V�����I�����Ȃ���Ԃ�\������</summary>
    ''' <param name="isOK">���J(True:OK, False:NG)</param>
    ''' <remarks>'V5.0.0.9�P</remarks>
    Friend Sub SetLblDoorOpen(ByVal isOK As Boolean)
        Static text As String = lblDoorOpen.Text

        With lblDoorOpen
            If (isOK) Then
                .BackColor = Color.Lime
                .Text = text & "OK"
            Else
                .BackColor = Color.Yellow
                .Text = text & "NG"
            End If
        End With
    End Sub
#End Region

#Region "�o�[�R�[�h�ǂݍ��ݓ��e�̃��Z�b�g"
    ''' <summary>�o�[�R�[�h�ǂݍ��ݓ��e�̃��Z�b�g</summary>
    ''' <remarks>'V5.0.0.9�R</remarks>
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRest.Click
        Dim r As MsgBoxResult

        ' �o�[�R�[�h�����N���A���Ă���낵���ł����H
        r = MsgBox(MSG_163, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)
        If (r = MsgBoxResult.Yes) Then
            BC_Info_Disp(0)
            BarCode_Data.BC_ReadCount = 0
            BarCode_Data.BC_ReadDataFirst = ""                          ' �o�[�R�[�h�P��ڂœǍ��񂾃f�[�^�ۑ��p
            BarCode_Data.BC_ReadDataSecound = ""                        ' �o�[�R�[�h�Q��ڂœǍ��񂾃f�[�^�ۑ��p
        End If
    End Sub
#End Region

    Private Sub CbDigSwL_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CbDigSwL.MouseWheel

        '----- V6.0.3.0_47�� -----
        If giMoveModeWheelDisable = 0 Then
            Return
        End If
        '----- V6.0.3.0_47�� -----

        Dim eventArgs As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        eventArgs.Handled = True

    End Sub

    '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
#Region "���ݓǂݍ��܂�Ă���f�[�^�̑�P��R�̃f�[�^��\��"
    '''=========================================================================
    ''' <summary>���ݓǂݍ��܂�Ă���f�[�^�̑�P��R�݈̂ȉ��̃f�[�^��\�����܂��B
    '''          �ڕW�l�A�J�b�g�I�t�A�G�b�W�Z���X�|�C���g�A�i�J�b�g�U�܂Łj�̃f�[�^��\�����܂��B
    ''' </summary>
    ''' <param name="init">true=�\����������</param>
    ''' <remarks>V6.1.4.0�F</remarks>
    '''=========================================================================
    Public Sub SetFirstResData(Optional ByVal init As Boolean = False) ' V6.1.4.0_22

        Const BLANK As String = "----"

        Try
            If gTkyKnd = KND_CHIP Then  'V6.1.4.14�@�@CHIP�̏ꍇ�͏]���ʂ�
                Me.tlpFirstResData.SuspendLayout()

                ' �\��������������
                lblFrdNomVal.Text = BLANK
                With Me.tlpFirstResData
                    For i As Integer = 1 To 6 Step 1
                        .Controls("lblFrdC" & i).Text = BLANK
                        .Controls("lblFrdE" & i).Text = BLANK
                    Next i
                End With

                If (False = init) Then
                    If (typResistorInfoArray Is Nothing) Then Return

                    Dim tmpRes As ResistorInfo = typResistorInfoArray(1)
                    lblFrdNomVal.Text = String.Format("{0:F5}", tmpRes.dblTrimTargetVal)

                    Dim tmpCut() As CutList = tmpRes.ArrCut
                    With Me.tlpFirstResData
                        If (tmpCut IsNot Nothing) AndAlso (tmpRes.intCutCount <= tmpCut.Length) Then
                            Dim min As Integer = Math.Min(tmpRes.intCutCount, tmpCut.Length)
                            min = Math.Min(min, 6)
                            ' �ő�6��Ă܂ő�1��R�̶���ް���\������
                            For i As Integer = 1 To min Step 1
                                'V6.1.4.0_51                            .Controls("lblFrdC" & i).Text = String.Format("{0:F2}", tmpCut(i).dblCutOff)
                                .Controls("lblFrdC" & i).Text = String.Format("{0:F3}", tmpCut(i).dblCutOff)        'V6.1.4.0_51
                                If tmpCut(i).strCutType = CNS_CUTP_ES Or tmpCut(i).strCutType = CNS_CUTP_ES2 Then
                                    .Controls("lblFrdE" & i).Text = String.Format("{0:F2}", tmpCut(i).dblESPoint)
                                End If
                            Next i
                        End If
                    End With
                End If
                'V6.1.4.14�@��NET�̏ꍇ��ǉ�
            Else
                Me.pnlFirstResDataNET.SuspendLayout()

                ' �\��������������
                lblNETNomVal.Text = BLANK
                With Me.tlpFirstResDataNET
                    For i As Integer = 1 To 4 Step 1
                        For j As Integer = 1 To 10 Step 1
                            .Controls("Res" & i & "Cut" & j).Text = BLANK
                        Next j
                    Next i
                End With

                If (False = init) Then
                    If (typResistorInfoArray Is Nothing) Then Return

                    lblNETNomVal.Text = String.Format("{0:F5}", typResistorInfoArray(1).dblTrimTargetVal)

                    With Me.tlpFirstResDataNET
                        For i As Integer = 1 To Math.Min(typPlateInfo.intResistCntInGroup, 4) Step 1
                            For j As Integer = 1 To Math.Min(typResistorInfoArray(i).intCutCount, 10) Step 1
                                .Controls("Res" & i & "Cut" & j).Text = String.Format("{0:F3}", typResistorInfoArray(i).ArrCut(j).dblCutOff)
                            Next j
                        Next i
                    End With
                End If
            End If
            'V6.1.4.14�@��

        Catch ex As Exception
            Dim strMsg As String = "i-TKY.SetFirstResData() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMsg)
        Finally
            Me.tlpFirstResData.ResumeLayout()
        End Try
    End Sub
#End Region
    '----- V6.1.4.0_22�� -----

    'V4.7.3.5�@��
    Private Sub CutOffEsEditButton_Click(sender As Object, e As EventArgs) Handles CutOffEsEditButton.Click
        Try



            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            Call RedrawDisplayDistribution(True)
            ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��

            ' ���[�_�փg���}���쒆�M���𑗐M����
            Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ���[�_�o��(ON=�g���}���쒆, OFF=�Ȃ�)
            giAppMode = APP_MODE_EDIT

            If (gLoadDTFlag = False) Then                           ' ���ݸ��ް�̧�ٖ�۰�� ?
                ' "�g���~���O�p�����[�^�f�[�^�����[�h���邩�V�K�쐬���Ă�������"
                Call System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                GoTo STP_END
            End If

            Call System1.OperationLogging(gSysPrm, "�J�b�g�I�t�l�ҏW", "")

            ' �R���\�[���{�^���̃����v��Ԃ�ݒ肷��
            Call LAMP_CTRL(LAMP_START, False)                       ' START���ߏ��� 
            Call LAMP_CTRL(LAMP_RESET, False)                       ' RESET���ߏ��� 

            Call Form1Button(0)                                     ' �R�}���h�{�^���𖳌��ɂ���

            If (gTkyKnd = KND_CHIP) Then     'V6.1.4.14�@
                'V6.1.4.14�@��
                Dim objFormCutOffEsPointEnter As New FormCutOffEsPointEnter()
                objFormCutOffEsPointEnter.ShowDialog()
                objFormCutOffEsPointEnter.Dispose()
            Else
                Dim objFormCutOffEnter As New FormCutOffEnter()                 'V6.1.4.14�@
                objFormCutOffEnter.ShowDialog()
                objFormCutOffEnter.Dispose()
            End If
            'V6.1.4.14�@��

            '-------------------------------------------------------------------
            '   �I������
            '-------------------------------------------------------------------
STP_END:
            '���v�\�������̏�ԕύX
            Call RedrawDisplayDistribution(False)

            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                Dim swStatus As Integer
                Dim interlockStatus As Integer
                Dim r As Short
                r = INTERLOCK_CHECK(interlockStatus, swStatus)
                If r = cFRS_NORMAL And interlockStatus = INTERLOCK_STS_DISABLE_NO Then
                    ' �X���C�h�J�o�[�̏�Ԏ擾�iINTRIM�ł�IO�擾�ׁ݂̂̈A�G���[���Ԃ鎖�͂Ȃ��j
                    r = SLIDECOVER_GETSTS(swStatus)
                    If r = cFRS_NORMAL And swStatus = SLIDECOVER_CLOSE Then
                        ' �X���C�h�J�o�[��
                        r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH52, &HFF, &HFF0000)
                    End If
                End If
            End If

            ' �g���}���u��Ԃ��A�C�h�����ɐݒ肷��
            Call TrimStateOff()
            Exit Sub

        Catch ex As Exception
            MessageBox.Show("i-TKY.CutOffEsEditButton_Click() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
    'V4.7.3.5�@��

    'V6.1.4.14�@��
    Private Sub CutOffEsEditButtonNET_Click(sender As Object, e As EventArgs) Handles CutOffEsEditButtonNET.Click
        Try
            Call CutOffEsEditButton_Click(sender, e)
        Catch ex As Exception
            MessageBox.Show("i-TKY.CutOffEsEditButtonNET_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    'V6.1.4.14�@��

End Class

#Region "�e�R�}���h���s�T�u�t�H�[���p���ʃC���^�[�t�F�[�X"
''' <summary>�e�R�}���h���s�T�u�t�H�[���p���ʃC���^�[�t�F�[�X</summary>
''' <remarks>'V6.0.0.0�J</remarks>
Public Interface ICommonMethods
    ''' <summary>�T�u�t�H�[���������s</summary>
    ''' <returns>���s���� sGetReturn</returns>
    ''' <remarks>'V6.0.0.0�L</remarks>
    Function Execute() As Integer

    ''' <summary>�T�u�t�H�[��KeyDown���̏���</summary>
    ''' <param name="e"></param>
    Sub JogKeyDown(ByVal e As KeyEventArgs)

    ''' <summary>�T�u�t�H�[��KeyUp���̏���</summary>
    ''' <param name="e"></param>
    Sub JogKeyUp(ByVal e As KeyEventArgs)

    ''' <summary>�J�����摜�N���b�N�ʒu���摜�Z���^�[�Ɉړ����鏈��</summary>
    ''' <param name="distanceX">�摜�Z���^�[����̋���X</param>
    ''' <param name="distanceY">�摜�Z���^�[����̋���Y</param>
    Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal)
End Interface
#End Region

'=============================== END OF FILE ===============================