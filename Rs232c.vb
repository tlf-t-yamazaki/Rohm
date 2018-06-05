'===============================================================================
'   Description  : �g���}�[���H������FL�����RS232C�ő���M����
'                  (C#�ō쐬���ꂽ�DllFLCom.dll����g�p)
'                   ���Ώۋ@��@FL�pFPGA�ݒ�p
'                     �ʐM����       : ��������������d�ʐM
'                     �{�[���[�g     : 38,400BPS
'                     �L�����N�^�[�� : 8 Bit
'                     �p���e�B       : �Ȃ�
'                     �X�g�b�v       : 1 BIT
'                     �f���~�^�R�[�h : CR
'
'                   ����M�f�[�^�`��
'                   1. ���M�f�[�^�`��(PC�@���� FL)
'                   -------------------------------------------
'                   |�R�}���h��(2) | �f�[�^(4)        | CR(1) |
'                   |A�`P��ASCII   |0�`9,a�`f��ASCII  |       |
'                   -------------------------------------------
'                   2. �����v���f�[�^�`��(PC�@�� FL)
'                   -----------------------------------
'                   |�R�}���h��(2) | ذ��(1) |�@CR(1) |
'                   |A�`P��ASCII   |  r      |�@      |
'                   -----------------------------------
'   Copyright(C) : OMRON LASERFRONT INC. 2011

'   Remarks      : ���LDLL(C#)���g�p(�u�Q�Ƃ̒ǉ��v�Œǉ�)
'          �@�@�@�@DllFLCom.dll, DllSerialIO.dll, DllCndXMLIO.dll
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module Rs232c
#Region "�ϐ���`"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    '----- �|�[�g��� -----
    Public Structure ComInfo
        Dim PortName As String                                  ' �q�r�Q�R�Q�b�|�[�g�ԍ�
        Dim BaudRate As Long                                    ' �`���X�s�[�h
        Dim Parity As Integer                                   ' �p���e�B(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
        Dim DataBits As Integer                                 ' �f�[�^�[��
        Dim StopBits As Integer                                 ' �X�g�b�v�r�b�g��
    End Structure

    '----- �N���X�I�u�W�F�N�g -----
    Private ObjPortInfo As DllSerialIO.PortInformation          ' �V���A���|�[�g���I�u�W�F�N(DllSerialIO.dll)
    Private ObjFLCom As DllFLCom.FLComIO                        ' �g���}�[���H��������M�I�u�W�F�N�g(DllFLCom.dll)

    '----- �o�[�W�������N���X -----
    Private ObjPortCVer As DllSerialIO.VersionInformation       ' DllSerialIO.dll
    Private ObjFLXMLVer As DllCndXMLIO.VersionInformation       ' DllCndXMLIO.dll
    Private ObjFLComVer As DllFLCom.VersionInformation          ' DllFLCom.dll

    '----- �g���}�[���H���� -----
    Public Const MAX_BANK_NUM As Integer = 32                   ' �ő���H������(0-31)
    Public Const MAX_STEG_NUM As Integer = 20                   ' STEG�g�`�ő�l(1-20)
    Public Const MAX_CURR_VAL As Integer = 8500                 ' �ő�d���l(mA)
    Public Const MIN_CURR_VAL As Integer = 1                    ' �ŏ��d���l(mA)
    Public Const MAX_FREQ_VAL As Integer = 100                  ' �ő���g��(KHz)
    Public Const MIN_FREQ_VAL As Integer = 1                    ' �ŏ����g��(KHz)

    Public Structure TrimCondInfo                               ' �g���}�[���H�����`����`
        Dim Curr() As Integer                                   ' �d���l(mA)
        Dim Freq() As Double                                    ' ���g��(KHz)
        Dim Steg() As Integer                                   ' STEG�g�`

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim Curr(MAX_BANK_NUM - 1)                        ' �z��(0-31) 
            ReDim Freq(MAX_BANK_NUM - 1)
            ReDim Steg(MAX_BANK_NUM - 1)
        End Sub
    End Structure

    '---------------------------------------------------------------------------
    '   �G���[�R�[�h(C#�ō쐬���ꂽdll���Ԃ��Ă������)
    '---------------------------------------------------------------------------
    Public Enum SerialErrorCode
        '----- 1-18��DllSerialIO�Ŏg�p -----
        rRS_OK = 0                                              '  0:����
        rRS_ReadTimeout                                         '  1:���[�h�^�C���A�E�g
        rRS_WriteTimeout                                        '  2:���C�g�^�C���A�E�g
        rRS_RespomseTimeout                                     '  3:�����^�C���A�E�g
        rRS_FailOpen                                            '  4:�ر��߰ĵ���ݎ��s
        rRS_FailClose                                           '  5:�ر��߰ĸ۰�ގ��s
        rRS_FailInit                                            '  6:�ر��߰ď��������s
        rRS_SerialErrorFrame                                    '  7:H/W���ڰѴװ���o
        rRS_SerialErrorOverrun                                  '  8:�����ޯ̧�̵��ް�ݔ���
        rRS_SerialErrorRXOver                                   '  9:�����ޯ̧�̵��ް�۰����
        rRS_SerialErrorRXParity                                 ' 10:H/W�����è�װ����
        rRS_SerialErrorTXFull                                   ' 11:���ع���݂͕����𑗐M���悤�Ƃ������o���ޯ̧����t
        rRS_InvalidSerialProtInfo                               ' 12:�V���A���|�[�g���s��
        rRS_InvalidValue                                        ' 13:�����ȃf�[�^
        rRS_FailSerialRead                                      ' 14:�ر��߰Ă���̓Ǎ����s
        rRS_FailSerialWrite                                     ' 15:�ر��߰Ăւ̏������s
        rRS_NotOpen                                             ' 16:�ر��߰Ă�����݂��Ă��Ȃ�
        rRS_Exception                                           ' 17:��O

        '----- �ȍ~�͓��֐��Ŏg�p -----
        rRS_FLCND_NONE = 101                                    ' 101:���H�����̐ݒ�Ȃ�
        rRS_FLCND_XMLNONE = 102                                 ' 102:���H�����t�@�C�������݂��Ȃ�
        rRS_FLCND_XMLREADERR = 103                              ' 103:���H�����t�@�C�����[�h�G���[
        rRS_FLCND_XMLWRITERR = 104                              ' 104:���H�����t�@�C�����C�g�G���[
        rRS_FLCND_SNDERR = 105                                  ' 105:���H�������M�G���[
        rRS_FLCND_RCVERR = 106                                  ' 106:���H������M�G���[
        rRS_FLCND_ATTNONE = 107                                 ' 107:�Œ�ATT���t�@�C�������݂��Ȃ� V1.14.0.0�A

        '----- �ȍ~��DllFLCom�Ŏg�p -----
        rRS_CndNum = 900                                        ' 900:���H�����ԍ��G���[
        rRS_Trap = 999                                          ' 999:�g���b�v�G���[����

    End Enum

    '----- ���̑� -----
    Private Rs_Flag As Integer                                  ' �׸�(0:�������, 1:����ݍ�)

#End Region

#Region "RS232C�p���\�b�h"
#Region "FL�p���H�����t�@�C�������[�h����FL���։��H�����𑗐M����"
    '''=========================================================================
    ''' <summary>FL�p���H�����t�@�C�������[�h����FL���։��H�����𑗐M����</summary>
    ''' <param name="stCND">   (OUT)�g���}�[���H�����\����</param>
    ''' <param name="DatFName">(INP)�f�[�^�t�@�C����</param>
    ''' <param name="CndFName">(OUT)�e�k�p���H�����t�@�C����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimCondInfToFL(ByVal stCND As TrimCondInfo, ByRef DatFName As String, ByRef CndFName As String) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �e�k�p���H�����t�@�C�������擾����(�t�@�C�����݃`�F�b�N����)
            r = GetFLCndFileName(DatFName, CndFName, True)
            If (r <> SerialErrorCode.rRS_OK) Then
                Return (r)
            End If

            ' �e�k�p���H������Ǎ���
            r = ReadFLCndFile(stCND, CndFName)
            If (r <> SerialErrorCode.rRS_OK) Then
                Return (r)
            End If

            ' ���H�����ԍ�31(�t���p���[����p)�ޔ� ###072
            '#4.12.3.0�E��
            '            If (CndFName = DEF_FLPRM_SETFILENAME) Then
            If (CndFName = DEF_FLPRM_SETFILENAME) Or ((DatFName = DEF_FLPRM_SETFILENAME) And (giFLPrm_Ass = 1)) Then
                '#4.12.3.0�E��
                ' �f�t�H���g�̐ݒ�t�@�C���Ȃ���H�����ԍ�31(�t���p���[����p)��ޔ�
                FLCnd31Curr = stCND.Curr(MAX_BANK_NUM - 1)              ' �d���l(mA)
                FLCnd31Freq = stCND.Freq(MAX_BANK_NUM - 1)              ' ���g��(KHz)
                FLCnd31Steg = stCND.Steg(MAX_BANK_NUM - 1)              ' STEG�g�`
            Else
                ' ���H�����ԍ�31(�t���p���[����p)�͍X�V���Ȃ� 
                stCND.Curr(MAX_BANK_NUM - 1) = FLCnd31Curr              ' �d���l(mA)
                stCND.Freq(MAX_BANK_NUM - 1) = FLCnd31Freq              ' ���g��(KHz)
                stCND.Steg(MAX_BANK_NUM - 1) = FLCnd31Steg              ' STEG�g�`
            End If

            ' FL���։��H�����𑗐M����
            r = TrimCondInfoSnd(stCND)
            If (r <> SerialErrorCode.rRS_OK) Then
                Return (r)
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.SendTrimCondInfToFL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "FL�����猻�݂̉��H��������M����FL�p���H�����t�@�C�������C�g����"
    '''=========================================================================
    ''' <summary>FL�����猻�݂̉��H��������M����FL�p���H�����t�@�C�������C�g����</summary>
    ''' <param name="stCND">   (OUT)�g���}�[���H�����\����</param>
    ''' <param name="DatFName">(INP)�f�[�^�t�@�C����</param>
    ''' <param name="CndFName">(OUT)�e�k�p���H�����t�@�C����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function RcvTrimCondInfToFL(ByVal stCND As TrimCondInfo, ByVal DatFName As String, ByRef CndFName As String) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' FL�����猻�݂̉��H��������M����
            r = TrimCondInfoRcv(stCND)
            If (r <> SerialErrorCode.rRS_OK) Then
                Return (r)
            End If

            ' �e�k�p���H�����t�@�C�������擾����(�t�@�C�����݃`�F�b�N�Ȃ�)
            r = GetFLCndFileName(DatFName, CndFName, False)
            If (r <> SerialErrorCode.rRS_OK) Then
                Return (r)
            End If

            ' �e�k�p���H�����t�@�C������������
            r = WriteFLCndFile(stCND, CndFName)
            If (r <> SerialErrorCode.rRS_OK) Then
                Return (r)
            End If

            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RcvTrimCondInfToFL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "FL���։��H�����𑗐M����"
    '''=========================================================================
    ''' <summary>FL���։��H�����𑗐M����</summary>
    ''' <param name="stCND">(INP)�g���}�[���H�����\����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function TrimCondInfoSnd(ByVal stCND As TrimCondInfo) As Integer

        Dim wkCND As TrimCondInfo
        Dim r, i As Integer
        Dim strMSG As String

        Try
            '^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            '   FL���։��H�����𑗐M����
            '^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �|�[�g����ݒ肷��
            'stCOM.PortName = "COM4"                             ' �|�[�g�ԍ�
            stCOM.PortName = "COM3"                             ' �|�[�g�ԍ�
            stCOM.BaudRate = 38400                              ' Speed
            stCOM.Parity = 0                                    ' �p���e�B(0:None)
            stCOM.DataBits = 8                                  ' �f�[�^�� = 8 Bit
            stCOM.StopBits = 1                                  ' Stop Bit = 1 Bit

            ' �|�[�g�I�[�v��
            r = Rs232c_Open(stCOM)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g�n�o�d�m�G���["
                strMSG = MSG_136 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
                Return (r)
            End If

            ' �V���A���|�[�g�փg���}�[���H�������ꊇ�ő��M����
            r = RsSendBankALL(stCND.Curr, stCND.Freq, stCND.Steg)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g���M�G���["
                strMSG = MSG_138 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
            End If

            ' �V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����
            wkCND = Nothing
            wkCND.Initialize()
            r = RsReceiveBankALL(wkCND.Curr, wkCND.Freq, wkCND.Steg, 10000)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g��M�G���["
                strMSG = MSG_139 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
                Rs232c_Close()
                Return (r)
            End If

            ' �|�[�g�N���[�Y
            Rs232c_Close()

            ' ���M�����g���}�[���H�������������ݒ肳��Ă��邩�m�F����
            For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H���������m�F����
                If (wkCND.Curr(i) <> stCND.Curr(i)) Then
                    Return (SerialErrorCode.rRS_FLCND_SNDERR)
                End If
                If (wkCND.Steg(i) <> stCND.Steg(i)) Then
                    Return (SerialErrorCode.rRS_FLCND_SNDERR)
                End If
            Next i

            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.TrimCondInfoSnd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "FL�����猻�݂̉��H��������M����"
    '''=========================================================================
    ''' <summary>FL�����猻�݂̉��H��������M����</summary>
    ''' <param name="stCND">(OUT)�g���}�[���H�����\����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function TrimCondInfoRcv(ByVal stCND As TrimCondInfo) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            '^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            '   FL�����猻�݂̉��H�������擾����
            '^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �|�[�g����ݒ肷��
            'stCOM.PortName = "COM4"                             ' �|�[�g�ԍ�
            stCOM.PortName = "COM3"                             ' �|�[�g�ԍ�
            stCOM.BaudRate = 38400                              ' Speed
            stCOM.Parity = 0                                    ' �p���e�B(0:None)
            stCOM.DataBits = 8                                  ' �f�[�^�� = 8 Bit
            stCOM.StopBits = 1                                  ' Stop Bit = 1 Bit

            ' �|�[�g�I�[�v��
            r = Rs232c_Open(stCOM)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g�n�o�d�m�G���["
                strMSG = MSG_136 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
                Return (r)
            End If

            ' �V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����
            r = RsReceiveBankALL(stCND.Curr, stCND.Freq, stCND.Steg, cTIMEOUT)
            If (r <> SerialErrorCode.rRS_OK) And (r <> SerialErrorCode.rRS_FLCND_NONE) Then
                ' "�V���A���|�[�g��M�G���["
                strMSG = MSG_139 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
            End If

            ' �|�[�g�N���[�Y
            Rs232c_Close()
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.TrimCondInfoRcv() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�e�k�p���H�����t�@�C����Ǎ���"
    '''=========================================================================
    ''' <summary>�e�k�p���H�����t�@�C����Ǎ���</summary>
    ''' <param name="stCND">(OUT)�g���}�[���H�����\����</param>
    ''' <param name="FName">(INP)���H�����t�@�C����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function ReadFLCndFile(ByVal stCND As TrimCondInfo, ByVal FName As String) As Integer

        Dim r As Boolean
        Dim strMSG As String

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �e�k�p���H�����t�@�C�������H�����\���̂ɓǍ���()
            Dim ObjFLXMLIO As DllCndXMLIO.CndXMLIO = New DllCndXMLIO.CndXMLIO           ' ���H�����t�@�C���h�n�I�u�W�F�N�g
            r = ObjFLXMLIO.Read_CndXMLFile(FName, stCND.Curr, stCND.Freq, stCND.Steg)
            If (r <> True) Then
                Return (SerialErrorCode.rRS_FLCND_XMLREADERR)
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.ReadFLCndFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�e�k�p���H�����t�@�C������������"
    '''=========================================================================
    ''' <summary>�e�k�p���H�����t�@�C������������</summary>
    ''' <param name="stCND">(INP)�g���}�[���H�����\����</param>
    ''' <param name="FName">(INP)���H�����t�@�C����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function WriteFLCndFile(ByVal stCND As TrimCondInfo, ByVal FName As String) As Integer

        Dim r As Boolean
        Dim strMSG As String

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �e�k�p���H�����t�@�C������������
            Dim ObjFLXMLIO As DllCndXMLIO.CndXMLIO = New DllCndXMLIO.CndXMLIO           ' ���H�����t�@�C���h�n�I�u�W�F�N�g
            r = ObjFLXMLIO.Write_CndXMLFile(FName, stCND.Curr, stCND.Freq, stCND.Steg)
            If (r <> True) Then
                Return (SerialErrorCode.rRS_FLCND_XMLWRITERR)
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.WriteFLCndFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�e�k�p���H�����t�@�C�������擾����"
    '''=========================================================================
    ''' <summary>�e�k�p���H�����t�@�C�������擾����</summary>
    ''' <param name="InpFName">(INP)�f�[�^�t�@�C����</param>
    ''' <param name="OutFName">(OUT)�e�k�p���H�����t�@�C����</param>
    ''' <param name="Flg">     (INP)���H�����t�@�C���̑��݃`�F�b�N�̗L��</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function GetFLCndFileName(ByRef InpFName As String, ByRef OutFName As String, ByVal Flg As Boolean) As Integer

        Dim len As Integer
        Dim strMSG As String = ""

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            '----- V2.0.0.0�D�� -----
            ' �e�k�p���H�����t�@�C������ݒ肷��
            If (giFLPrm_Ass = 1) Then                                   ' ���H�����t�@�C���w�肪1(1�Œ�) ?
                ' ���H�����t�@�C������FLParamFile.xml�Ƃ���
                OutFName = gsFLPrmFile                                  ' C:\TRIMDATA\DATA\FLParamFile.xml
            Else
                ' �f�[�^�t�@�C�����̊g���q��".xml"�ɕϊ�����
                len = InpFName.Length
                '----- V4.0.0.0�H�� -----
                If (InpFName <> DEF_FLPRM_SETFILENAME) And (giMachineKd = MACHINE_KD_RS) Then
                    OutFName = InpFName.Substring(0, len - 5)
                Else
                    OutFName = InpFName.Substring(0, len - 4)
                End If
                'OutFName = InpFName.Substring(0, len - 4)
                '----- V4.0.0.0�H�� -----
                OutFName = OutFName + ".xml"
            End If
            '----- V2.0.0.0�D�� -----

            ' �f�[�^�t�@�C���̑��݃`�F�b�N
            If (Flg = True) Then
                If (System.IO.File.Exists(OutFName) = False) Then
                    Return (SerialErrorCode.rRS_FLCND_XMLNONE)
                End If
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.GetFLCndFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region
    '----- V1.14.0.0�A�� -----
#Region "�e�k�p�Œ�ATT���t�@�C�������擾����"
    '''=========================================================================
    ''' <summary>�e�k�p�Œ�ATT���t�@�C�������擾����</summary>
    ''' <param name="InpFName">(INP)�f�[�^�t�@�C����</param>
    ''' <param name="OutFName">(OUT)�Œ�ATT���t�@�C��</param>
    ''' <param name="Flg">     (INP)�Œ�ATT���̑��݃`�F�b�N�̗L��</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function GetFLAttFileName(ByRef InpFName As String, ByRef OutFName As String, ByVal Flg As Boolean) As Integer

        Dim len As Integer
        Dim strMSG As String

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �f�[�^�t�@�C�����̊g���q��".att"�ɕϊ����ĕԂ�
            len = InpFName.Length
            '----- V4.0.0.0�H�� -----
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ?
                OutFName = InpFName.Substring(0, len - 5)
            Else
                OutFName = InpFName.Substring(0, len - 4)
            End If
            'OutFName = InpFName.Substring(0, len - 4)
            '----- V4.0.0.0�H�� -----
            OutFName = OutFName + ".att"

            ' �f�[�^�t�@�C���̑��݃`�F�b�N
            If (Flg = True) Then
                If (System.IO.File.Exists(OutFName) = False) Then
                    Return (SerialErrorCode.rRS_FLCND_ATTNONE)
                End If
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.GetFLAttFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region
    '----- V1.14.0.0�A�� -----
    '----- V1.14.0.0�A�� -----
#Region "SAVE�t�@�C�������擾����"
    '''=========================================================================
    ''' <summary>SAVE�t�@�C�������擾����</summary>
    ''' <param name="InpFName">(INP)�f�[�^�t�@�C����</param>
    ''' <param name="OutFName">(OUT)�f�[�^�t�@�C����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function GetSaveFileName(ByRef InpFName As String, ByRef OutFName As String) As Integer

        Dim Pos As Integer
        Dim len As Integer
        Dim strDAT As String = ""
        Dim strMSG As String = ""

        Try
            ' �g���q�ݒ�
            If (gTkyKnd = KND_TKY) Then                                 ' TKY�̏ꍇ
                strDAT = ".tdt"
            End If
            If (gTkyKnd = KND_CHIP) Then                                ' CHIP�̏ꍇ
                strDAT = ".tdc"
            End If
            If (gTkyKnd = KND_NET) Then                                 ' NET�̏ꍇ
                strDAT = ".tdn"
            End If
            '----- V4.0.0.0�H�� -----
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ?
                If (gTkyKnd = KND_TKY) Then                             ' TKY�̏ꍇ
                    strDAT = ".tdts"
                End If
                If (gTkyKnd = KND_CHIP) Then                            ' CHIP�̏ꍇ
                    strDAT = ".tdcs"
                End If
                If (gTkyKnd = KND_NET) Then                             ' NET�̏ꍇ
                    strDAT = ".tdns"
                End If
            End If
            '----- V4.0.0.0�H�� -----

            ' �f�[�^�t�@�C�����̊g���q��ϊ����ĕԂ�
            len = InpFName.Length
            'Pos = InpFName.IndexOf(".")                                'V1.16.0.0�N
            Pos = InpFName.LastIndexOf(".")                             'V1.16.0.0�N
            strMSG = InpFName.Substring(Pos, len - Pos)
            If (strMSG <> strDAT) Then                                  ' ".tdt",".tdc",".tdn"�ȊO? 
                OutFName = InpFName.Substring(0, len - (len - Pos))
                OutFName = OutFName + strDAT
            Else
                OutFName = InpFName                                     ' �g���q����L�̏ꍇ�͂��̂܂ܕԂ� 
            End If

            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.GetSaveFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region
    '----- V1.14.0.0�A�� -----
#Region "FL������G���[������M����"
    '''=========================================================================
    ''' <summary>FL������G���[������M����</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function ReceiveErrInfo(ByRef ErrInf As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' FL(̧��ްڰ��) �łȂ���� NOP
            ErrInf = 0
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �|�[�g����ݒ肷��
            stCOM.PortName = "COM3"                             ' �|�[�g�ԍ�
            stCOM.BaudRate = 38400                              ' Speed
            stCOM.Parity = 0                                    ' �p���e�B(0:None)
            stCOM.DataBits = 8                                  ' �f�[�^�� = 8 Bit
            stCOM.StopBits = 1                                  ' Stop Bit = 1 Bit

            ' �|�[�g�I�[�v��
            r = Rs232c_Open(stCOM)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g�n�o�d�m�G���["
                strMSG = MSG_136 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
                Return (r)
            End If

            ' FL������G���[������M����
            r = RsReceiveErrInfo(ErrInf, cTIMEOUT)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g��M�G���["
                strMSG = MSG_139 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
            End If

            ' �|�[�g�N���[�Y
            Rs232c_Close()

            ' INtime����FL�̃G���[���𑗐M����(���O�o�͗p)
            Call SET_FL_ERRLOG(ErrInf)

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.ReceiveErrInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "RS232C�|�[�g�̃I�[�v��"
    '''=========================================================================
    '''<summary>RS232C�|�[�g�̃I�[�v��</summary>
    '''<param name="pstCom">(INP) �|�[�g���</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Rs232c_Open(ByVal pstCom As ComInfo) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            r = SerialErrorCode.rRS_OK                          ' Return�l = ����
            ObjPortInfo = New DllSerialIO.PortInformation       ' �߰ď���޼ު�Đ���
            ObjFLCom = New DllFLCom.FLComIO                     ' �g���}�[���H��������M��޼ު�Đ���

            ' �|�[�g����ݒ肷��
            ObjPortInfo.PortName = pstCom.PortName              ' �|�[�g�ԍ�
            ObjPortInfo.BaudRate = pstCom.BaudRate              ' Speed
            ObjPortInfo.Parity = pstCom.Parity                  ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
            ObjPortInfo.DataBits = pstCom.DataBits              ' Char Data
            ObjPortInfo.StopBits = pstCom.StopBits              ' Stop Bit

            ' �|�[�g�I�[�v��
            r = ObjFLCom.Serial_Open(ObjPortInfo)               ' �|�[�g�I�[�v��
            Rs_Flag = 1                                         ' �׸�=1(����ݍ�)
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.Rs232c_Open() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "RS232C�|�[�g�̃N���[�Y"
    '''=========================================================================
    '''<summary>RS232C�|�[�g�̃N���[�Y</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Rs232c_Close() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �|�[�g�N���[�Y
            r = SerialErrorCode.rRS_OK                          ' Return�l = ����
            If (Rs_Flag = 0) Then Exit Function ' �׸�=0(�������)�Ȃ�NOP
            r = ObjFLCom.Serial_Close()                         ' �|�[�g�N���[�Y
            Rs_Flag = 0                                         ' �׸�=0(�������)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.Rs232c_Close() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = SerialErrorCode.rRS_Trap
        End Try

        ObjPortInfo = Nothing                                   ' �߰ď���޼ު�ĉ��
        ObjFLCom = Nothing                                      ' �g���}�[���H��������M��޼ު�ĉ��
        Return (r)

    End Function
#End Region

#Region "�V���A���|�[�g�փg���}�[���H�������ʂɑ��M����"
    '''=========================================================================
    '''<summary>�V���A���|�[�g�փg���}�[���H�������ʂɑ��M����</summary>
    '''<param name="CndNum">(INP) �����ԍ�</param>
    '''<param name="Curr">  (INP) �d���l</param>
    '''<param name="Freq">  (INP) ���g��</param> 
    '''<param name="Steg">  (INP) STEG�g�`</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function RsSendBankInfo(ByVal CndNum As Integer, ByVal Curr As Integer, ByVal Freq As Double, ByVal Steg As Integer) As Integer

        Dim wkFreq As Integer
        Dim r As Integer
        Dim dblWK As Double                                     '###040
        Dim strMSG As String

        Try
            ' ��������
            If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
                Return (SerialErrorCode.rRS_NotOpen)            ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
            End If

            ' ���g��(KHz)���J�Ԃ��Ԋu�ɕϊ�����
            ' �J�Ԃ��ԊuN = �J�Ԃ�����/200ns ��)10KHz�̏ꍇ�́A500(0x01f4)�𑗐M����B
            If (Freq <= 0) Then
                wkFreq = 0
            Else
                'wkFreq = 1000000 / 200 / Freq                  '###040
                dblWK = 1000000 / 200 / Freq
                'V6.0.0.1�@                wkFreq = dblWK
                wkFreq = Math.Truncate(dblWK)       'V6.0.0.1�@
            End If

            ' �g���}�[���H�������M
            r = ObjFLCom.Serial_SendBankInfo(CndNum, Curr, wkFreq, Steg)
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RsSendBankInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�V���A���|�[�g�փg���}�[���H�������ꊇ�ő��M����"
    '''=========================================================================
    '''<summary>�V���A���|�[�g�փg���}�[���H�������ꊇ�ő��M����</summary>
    '''<param name="Curr">  (INP) �d���l</param>
    '''<param name="Freq">  (INP) ���g��</param> 
    '''<param name="Steg">  (INP) STEG�g�`</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function RsSendBankALL(ByVal Curr() As Integer, ByVal Freq() As Double, ByVal Steg() As Integer) As Integer

        Dim i As Integer
        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
                Return (SerialErrorCode.rRS_NotOpen)            ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
            End If

            ' �V���A���|�[�g�փg���}�[���H�����𑗐M����
            For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H��������(0-31)���M����
                r = RsSendBankInfo(i, Curr(i), Freq(i), Steg(i))
                If (r <> SerialErrorCode.rRS_OK) Then           ' ���M�G���[ ?
                    Exit For
                End If
            Next i
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RsSendBankALL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�V���A���|�[�g����g���}�[���H�������ʂɎ�M����"
    '''=========================================================================
    '''<summary>�V���A���|�[�g����g���}�[���H�������ʂɎ�M����</summary>
    '''<param name="CndNum"> (INP) �����ԍ�</param>
    '''<param name="Curr">   (OUT) �d���l</param>
    '''<param name="Freq">   (OUT) ���g��</param> 
    '''<param name="Steg">   (OUT) STEG�g�`</param>  
    '''<param name="TimeOut">(INP) �����҃^�C�}�l(ms)</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function RsReceiveBankInfo(ByVal CndNum As Integer, ByRef Curr As Integer, ByRef Freq As Double, ByRef Steg As Integer, ByVal TimeOut As Integer) As Integer

        Dim wkFreq As Integer
        Dim r As Integer
        Dim dblWK As Double                                     '###040
        Dim dblWK2 As Double                                    '###040

        Dim strMSG As String

        Try
            ' ��������
            If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
                Return (SerialErrorCode.rRS_NotOpen)            ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
            End If

            ' �g���}�[���H������M
            r = ObjFLCom.Serial_ReceiveBankInfo(CndNum, Curr, wkFreq, Steg, TimeOut)
            If (r <> SerialErrorCode.rRS_OK) Then               ' ��M�G���[ ?
                Return (r)                                      ' Return�l�ݒ�
            End If

            ' �J�Ԃ��Ԋu�����g��(KHz)�ɕϊ�����
            If (wkFreq <= 0) Then
                Freq = 0
            Else
                ' Freq = 1000000 / (wkFreq * 200)               '###040
                dblWK = wkFreq * 200
                dblWK2 = 1000000
                Freq = dblWK2 / dblWK
                'V6.0.0.1�@                dblWK2 = Fix(10 * Freq)                         ' ###048 �����_�Q�ʈȉ���؂�̂Ă�
                dblWK2 = Math.Truncate(10 * Freq)               'V6.0.0.1�@
                Freq = dblWK2 / 10                              ' ###048
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RsReceiveBankInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����"
    '''=========================================================================
    '''<summary>�V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����</summary>
    '''<param name="Curr">   (OUT) �d���l</param>
    '''<param name="Freq">   (OUT) ���g��</param> 
    '''<param name="Steg">   (OUT) STEG�g�`</param> 
    '''<param name="TimeOut">(INP) �����҃^�C�}�l(ms)</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function RsReceiveBankALL(ByRef Curr() As Integer, ByRef Freq() As Double, ByRef Steg() As Integer, ByVal TimeOut As Integer) As Integer

        Dim i As Integer
        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
                Return (SerialErrorCode.rRS_NotOpen)            ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
            End If

            ' �V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����
            For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H����������M����
                r = RsReceiveBankInfo(i, Curr(i), Freq(i), Steg(i), TimeOut)
                If (r <> SerialErrorCode.rRS_OK) Then           ' �G���[ ?
                    Return (r)
                End If
            Next i

            ' FL���̐ݒ肪���邩�m�F����
            For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H����������M����
                If (Curr(i) <> 0) Then                          ' �d���l�ݒ�0�ȊO������������FL���̐ݒ肪����Ɣ��f����
                    Return (SerialErrorCode.rRS_OK)             ' Return�l = ����
                End If
            Next i
            Return (SerialErrorCode.rRS_FLCND_NONE)             ' Return�l = ���H�����̐ݒ�Ȃ�

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RsReceiveBankALL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�V���A���|�[�g����G���[������M����"
    '''=========================================================================
    '''<summary>�V���A���|�[�g����G���[������M����</summary>
    '''<param name="ErrInf"> (OUT) �G���[���</param>
    '''<param name="TimeOut">(INP) �����҃^�C�}�l(ms)</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function RsReceiveErrInfo(ByRef ErrInf As Integer, ByVal TimeOut As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
                Return (SerialErrorCode.rRS_NotOpen)            ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
            End If

            ' �G���[����M
            r = ObjFLCom.Serial_ReceiveErrInfo(ErrInf, TimeOut)
            If (r <> SerialErrorCode.rRS_OK) Then               ' ��M�G���[ ?
                Return (r)                                      ' Return�l�ݒ�
            End If
            Return (SerialErrorCode.rRS_OK)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RsReceiveErrInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "�V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����"
    '''=========================================================================
    '''<summary>�V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����(���H�����`�F�b�N�p�ɒǉ�)</summary>
    '''<param name="TimeOut">(INP) �����҃^�C�}�l(ms)</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function RsReceiveBankChkALL(ByVal TimeOut As Integer) As Integer

        Dim i As Integer
        Dim r As Integer
        Dim strMSG As String
        Dim hit As Integer
        Dim Curr() As Integer                                   ' �d���l(mA)
        Dim Freq() As Double                                    ' ���g��(KHz)
        Dim Steg() As Integer                                   ' STEG�g�`

        ' �\���̂̏�����
        ReDim Curr(MAX_BANK_NUM - 1)                        ' �z��(0-31) 
        ReDim Freq(MAX_BANK_NUM - 1)
        ReDim Steg(MAX_BANK_NUM - 1)

        Try

            ' FL(̧��ްڰ��) �łȂ���� NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then
                Return (SerialErrorCode.rRS_OK)
            End If

            ' �|�[�g����ݒ肷��
            'stCOM.PortName = "COM4"                             ' �|�[�g�ԍ�
            stCOM.PortName = "COM3"                             ' �|�[�g�ԍ�
            stCOM.BaudRate = 38400                              ' Speed
            stCOM.Parity = 0                                    ' �p���e�B(0:None)
            stCOM.DataBits = 8                                  ' �f�[�^�� = 8 Bit
            stCOM.StopBits = 1                                  ' Stop Bit = 1 Bit

            ' �|�[�g�I�[�v��
            r = Rs232c_Open(stCOM)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�V���A���|�[�g�n�o�d�m�G���["
                strMSG = MSG_136 + "(" + stCOM.PortName + ")"
                Call MsgBox(strMSG, vbOKOnly)
                Return (r)
            End If


            ' ��������
            If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
                Return (SerialErrorCode.rRS_NotOpen)            ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
            End If

            hit = 0

            ' �V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����
            For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H����������M����
                r = RsReceiveBankInfo(i, Curr(i), Freq(i), Steg(i), TimeOut)
                If (r <> SerialErrorCode.rRS_OK) Then           ' �G���[ ?
                    ' �|�[�g�N���[�Y
                    Rs232c_Close()
                    Return (r)
                End If
                If (Curr(i) <> 0) Then                          ' �d���l�ݒ�0�ȊO������������FL���̐ݒ肪����Ɣ��f����
                    hit = 1
                    ' �|�[�g�N���[�Y
                    Rs232c_Close()
                    Return (SerialErrorCode.rRS_OK)             ' Return�l = ����
                End If
            Next i

            ' �|�[�g�N���[�Y
            Rs232c_Close()

            ' FL���̐ݒ肪���邩�m�F����
            If hit = 0 Then                          ' �d���l�ݒ�0�ȊO������������FL���̐ݒ肪����Ɣ��f����
                Return (SerialErrorCode.rRS_FLCND_NONE)             ' Return�l = ���H�����̐ݒ�Ȃ�
            End If

            Return (SerialErrorCode.rRS_OK)             ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Rs232c.RsReceiveBankChkALL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region


    '#Region "�o�[�W�����̎擾"
    '    '''=========================================================================
    '    '''<summary>�o�[�W�����̎擾</summary>
    '    '''<param name="strVER">(OUT) strVER (0) = DllSerialIO.dll�̃o�[�W����
    '    '''                           strVER (1) = DllCndXMLIO.dll�̃o�[�W����
    '    '''                           strVER (2) = DllFLCom.dll�̃o�[�W����</param>
    '    '''=========================================================================
    '    Public Sub Rs232c_GetVersion(ByRef strVER() As String)

    '        Dim iMajor As Integer                                   ' Major Version
    '        Dim iMinor As Integer                                   ' Minor Version
    '        Dim iBNum As Integer                                    ' Build Number
    '        Dim iRev As Integer                                     ' Revision
    '        Dim strMSG As String

    '        Try
    '            ' �o�[�W�������N���X�I�u�W�F�N�g����
    '            ObjPortCVer = New DllSerialIO.VersionInformation
    '            ObjFLXMLVer = New DllCndXMLIO.VersionInformation
    '            ObjFLComVer = New DllFLCom.VersionInformation

    '            ' �o�[�W�����̎擾("Vx.x.x.x"�̌`���ŕԂ�)
    '            Call ObjPortCVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    '            strVER(0) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")

    '            Call ObjFLXMLVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    '            strVER(1) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")

    '            Call ObjFLComVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    '            strVER(2) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")

    '            ' �o�[�W�������N���X�I�u�W�F�N�g�J��
    '            ObjPortCVer = Nothing
    '            ObjFLXMLVer = Nothing
    '            ObjFLComVer = Nothing

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            strMSG = "Rs232c.Rs232c_GetVersion() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region

#End Region
End Module
